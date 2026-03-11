using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Examples;

/// <summary>
/// Part 8: Evaluation-Led Development with Foundry Local.
/// Tests agent quality using golden datasets, rule-based checks,
/// and LLM-as-judge scoring — all running on-device.
/// </summary>
public static class AgentEvaluation
{
    // ── 1. Golden Dataset ──────────────────────────────────────────────
    private record TestCase(string Input, string[] Expected, string Category);

    private static readonly TestCase[] GoldenDataset =
    [
        new("What tools do I need to build a wooden deck?",
            ["saw", "drill", "screws", "level", "tape measure"],
            "product-recommendation"),

        new("How do I fix a leaky kitchen faucet?",
            ["wrench", "washer", "plumber", "valve", "seal"],
            "repair-guidance"),

        new("What type of paint should I use for a bathroom?",
            ["moisture", "mildew", "semi-gloss", "primer", "ventilation"],
            "product-recommendation"),

        new("How do I safely use a circular saw?",
            ["safety", "glasses", "guard", "clamp", "blade"],
            "safety-advice"),

        new("What is the best way to organize a small workshop?",
            ["pegboard", "shelves", "storage", "tool chest", "workbench"],
            "workspace-setup"),
    ];

    // ── 2. Prompt Variants ─────────────────────────────────────────────
    private static readonly Dictionary<string, string> PromptVariants = new()
    {
        ["baseline"] =
            "You are a helpful assistant. Answer the user's question clearly and concisely.",
        ["specialised"] =
            "You are a Zava DIY expert and home improvement specialist. " +
            "When answering questions, recommend specific tools and materials, " +
            "provide step-by-step guidance, and include safety tips. " +
            "Keep answers practical and actionable for a weekend DIYer.",
    };

    // ── 3. Rule-Based Scoring ──────────────────────────────────────────
    private static readonly string[] ForbiddenTerms = ["home depot", "lowes", "amazon"];

    private record RuleScore(
        double LengthScore, double KeywordScore, string[] KeywordsFound,
        string[] KeywordsMissing, double ForbiddenScore, string[] ForbiddenFound,
        double Combined);

    private static RuleScore ScoreRules(string response, string[] expectedKeywords)
    {
        var words = response.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var wordCount = words.Length;
        var responseLower = response.ToLowerInvariant();

        // Length check: 50-500 words
        var lengthScore = wordCount >= 50 && wordCount <= 500 ? 1.0 : 0.0;

        // Keyword coverage
        var found = expectedKeywords
            .Where(kw => responseLower.Contains(kw.ToLowerInvariant()))
            .ToArray();
        var missing = expectedKeywords
            .Where(kw => !responseLower.Contains(kw.ToLowerInvariant()))
            .ToArray();
        var keywordScore = expectedKeywords.Length > 0
            ? (double)found.Length / expectedKeywords.Length
            : 1.0;

        // Forbidden terms
        var forbiddenFound = ForbiddenTerms
            .Where(t => responseLower.Contains(t))
            .ToArray();
        var forbiddenScore = forbiddenFound.Length > 0 ? 0.0 : 1.0;

        var combined = Math.Round((lengthScore + keywordScore + forbiddenScore) / 3.0, 2);

        return new RuleScore(lengthScore, keywordScore, found, missing,
            forbiddenScore, forbiddenFound, combined);
    }

    // ── 4. LLM-as-Judge ───────────────────────────────────────────────
    private const string JudgeSystemPrompt = """
        You are an impartial quality evaluator. Rate the following response on a scale of 1-5.

        Rubric:
        - 1: Completely wrong or irrelevant
        - 2: Partially correct but missing key information
        - 3: Adequate but could be improved significantly
        - 4: Good response with only minor issues
        - 5: Excellent, comprehensive, well-structured response

        Respond ONLY with valid JSON (no code fences):
        {"score": <1-5>, "reasoning": "<brief explanation>"}
        """;

    private record JudgeResult(int Score, string Reasoning);

    private static async Task<JudgeResult> LlmJudge(
        ChatClient chatClient, string question, string response)
    {
        // Truncate long responses to stay within context limits
        if (response.Length > 1500)
            response = response[..1500] + "...";

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(JudgeSystemPrompt),
            new UserChatMessage($"Question: {question}\n\nResponse to evaluate:\n{response}")
        };

        var options = new ChatCompletionOptions
        {
            Temperature = 0.1f,
            MaxOutputTokenCount = 256,
        };

        var result = await chatClient.CompleteChatAsync(messages, options);
        var raw = result.Value.Content[0].Text.Trim();

        // Strip code fences
        raw = raw.TrimStart('`');
        if (raw.StartsWith("json", StringComparison.OrdinalIgnoreCase))
            raw = raw[4..];
        raw = raw.TrimEnd('`').Trim();

        try
        {
            using var doc = JsonDocument.Parse(raw);
            var score = doc.RootElement.TryGetProperty("score", out var s) ? s.GetInt32() : 3;
            score = Math.Clamp(score, 1, 5);
            var reasoning = doc.RootElement.TryGetProperty("reasoning", out var r)
                ? r.GetString() ?? ""
                : "";
            return new JudgeResult(score, reasoning);
        }
        catch
        {
            // Fallback: extract a number
            var match = Regex.Match(raw, @"\b([1-5])\b");
            return new JudgeResult(match.Success ? int.Parse(match.Groups[1].Value) : 3, raw);
        }
    }

    // ── 5. Run Agent ──────────────────────────────────────────────────
    private static async Task<string> RunAgent(
        ChatClient chatClient, string systemPrompt, string userInput)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userInput)
        };

        var options = new ChatCompletionOptions
        {
            Temperature = 0.7f,
            MaxOutputTokenCount = 512,
        };

        var result = await chatClient.CompleteChatAsync(messages, options);
        return result.Value.Content[0].Text.Trim();
    }

    // ── 6. Main Pipeline ──────────────────────────────────────────────
    private record EvalResult(
        TestCase TestCase, string Response, RuleScore RuleScores, JudgeResult JudgeResult);

    public static async Task RunAsync()
    {
        var alias = "phi-3.5-mini";

        Console.WriteLine(new string('=', 60));
        Console.WriteLine("  EVALUATION-LED DEVELOPMENT WITH FOUNDRY LOCAL");
        Console.WriteLine(new string('=', 60));

        // Start Foundry Local
        Console.WriteLine("\nStarting Foundry Local service...");
        await FoundryLocalManager.CreateAsync(
            new Configuration
            {
                AppName = "FoundryLocalSamples",
                Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
            }, NullLogger.Instance, default);
        var manager = FoundryLocalManager.Instance;
        await manager.StartWebServiceAsync(default);

        var catalog = await manager.GetCatalogAsync(default);
        var model = await catalog.GetModelAsync(alias, default);

        var isCached = await model.IsCachedAsync(default);
        if (isCached)
            Console.WriteLine($"Model already downloaded: {alias}");
        else
        {
            Console.WriteLine($"Downloading model: {alias} (this may take several minutes)...");
            await model.DownloadAsync(null, default);
            Console.WriteLine($"Download complete: {alias}");
        }

        Console.WriteLine($"Loading model: {alias}...");
        await model.LoadAsync(default);
        Console.WriteLine($"Loaded model: {model.Id}");
        Console.WriteLine($"Endpoint: {manager.Urls[0]}");
        Console.WriteLine($"Test cases: {GoldenDataset.Length}");
        Console.WriteLine($"Prompt variants: {PromptVariants.Count}\n");

        var key = new ApiKeyCredential("foundry-local");
        var openAiClient = new OpenAIClient(key, new OpenAIClientOptions
        {
            Endpoint = new Uri(manager.Urls[0] + "/v1")
        });
        var chatClient = openAiClient.GetChatClient(model.Id);

        // Run evaluation for each prompt variant
        var results = new Dictionary<string, List<EvalResult>>();

        foreach (var (variantName, systemPrompt) in PromptVariants)
        {
            Console.WriteLine($"\n{new string('─', 60)}");
            Console.WriteLine($"  Evaluating variant: {variantName.ToUpperInvariant()}");
            Console.WriteLine(new string('─', 60));

            var variantResults = new List<EvalResult>();

            for (var i = 0; i < GoldenDataset.Length; i++)
            {
                var testCase = GoldenDataset[i];
                var truncated = testCase.Input.Length > 50
                    ? testCase.Input[..50] + "..."
                    : testCase.Input;
                Console.WriteLine($"\n  Test {i + 1}/{GoldenDataset.Length}: {truncated}");

                // Run the agent
                var response = await RunAgent(chatClient, systemPrompt, testCase.Input);

                // Rule-based scoring
                var ruleScores = ScoreRules(response, testCase.Expected);
                Console.WriteLine(
                    $"    Rule score: {ruleScores.Combined:F2}  " +
                    $"(length={ruleScores.LengthScore:F0}, " +
                    $"keywords={ruleScores.KeywordScore:F2}, " +
                    $"forbidden={ruleScores.ForbiddenScore:F0})");

                if (ruleScores.KeywordsFound.Length > 0)
                    Console.WriteLine($"    Keywords found: {string.Join(", ", ruleScores.KeywordsFound)}");
                if (ruleScores.KeywordsMissing.Length > 0)
                    Console.WriteLine($"    Keywords missing: {string.Join(", ", ruleScores.KeywordsMissing)}");

                // LLM-as-judge scoring
                JudgeResult judgeResult;
                try
                {
                    judgeResult = await LlmJudge(chatClient, testCase.Input, response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    LLM judge error: {ex.Message}");
                    judgeResult = new JudgeResult(3, "Judge unavailable — defaulting to 3");
                }
                var reasoning = judgeResult.Reasoning.Length > 80
                    ? judgeResult.Reasoning[..80] + "..."
                    : judgeResult.Reasoning;
                Console.WriteLine($"    LLM judge: {judgeResult.Score}/5  ({reasoning})");

                variantResults.Add(new EvalResult(testCase, response, ruleScores, judgeResult));
            }

            results[variantName] = variantResults;
        }

        // ── Print Scorecard ────────────────────────────────────────────
        Console.WriteLine($"\n\n{new string('=', 60)}");
        Console.WriteLine("  EVALUATION SCORECARD");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"\n  {"Variant",-16} {"Rule Score",12} {"LLM Score",12} {"Combined",12}");
        Console.WriteLine($"  {new string('─', 16)} {new string('─', 12)} {new string('─', 12)} {new string('─', 12)}");

        foreach (var (variantName, variantResults) in results)
        {
            var avgRule = variantResults.Average(r => r.RuleScores.Combined);
            var avgLlm = variantResults.Average(r => r.JudgeResult.Score);
            var combined = (avgRule + avgLlm / 5.0) / 2.0;

            Console.WriteLine(
                $"  {variantName,-16} {avgRule,10:F2}   {avgLlm,6:F1}/5   {combined,10:F2}");
        }

        Console.WriteLine($"\n  {new string('─', 52)}");

        // Per-category breakdown
        var categories = GoldenDataset.Select(t => t.Category).Distinct().Order().ToList();
        Console.WriteLine("\n  Per-Category Breakdown (Rule Score):");
        Console.Write($"  {"Category",-24} ");
        foreach (var vn in results.Keys) Console.Write($"{vn,14}");
        Console.WriteLine();
        Console.Write($"  {new string('─', 24)} ");
        foreach (var _ in results.Keys) Console.Write(new string('─', 14));
        Console.WriteLine();

        foreach (var cat in categories)
        {
            Console.Write($"  {cat,-24} ");
            foreach (var variantResults in results.Values)
            {
                var catResults = variantResults.Where(r => r.TestCase.Category == cat).ToList();
                if (catResults.Count > 0)
                {
                    var avg = catResults.Average(r => r.RuleScores.Combined);
                    Console.Write($"{avg,12:F2}  ");
                }
                else
                    Console.Write($"{"N/A",12}  ");
            }
            Console.WriteLine();
        }

        Console.WriteLine($"\n{new string('=', 60)}");
        Console.WriteLine("  Evaluation complete!");
        Console.WriteLine(new string('=', 60));

        // Cleanup: unload the model to release resources
        await model.UnloadAsync(default);
    }
}
