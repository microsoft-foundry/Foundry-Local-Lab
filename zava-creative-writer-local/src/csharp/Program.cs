// Zava Creative Writer — C# Console Application
// Multi-agent pipeline: Researcher → Product → Writer → Editor
// with feedback loop (max 2 retries).
//
// Usage: dotnet run

using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;

// ── Model loading (best-practice 4-step pattern) ────────────────────────
var alias = "phi-3.5-mini";

Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "ZavaCreativeWriter",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

var catalog = await manager.GetCatalogAsync(default);
var catalogModel = await catalog.GetModelAsync(alias, default);
var isCached = await catalogModel.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine($"Model already downloaded: {alias}");
}
else
{
    Console.WriteLine($"Downloading model: {alias} (this may take several minutes)...");
    await catalogModel.DownloadAsync(null, default);
    Console.WriteLine($"Download complete: {alias}");
}

Console.WriteLine($"Loading model: {alias}...");
await catalogModel.LoadAsync(default);
var modelId = catalogModel.Id;
Console.WriteLine($"Model ready: {modelId}");

var key = new ApiKeyCredential("foundry-local");
var openAiClient = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls[0] + "/v1")
});
var chatClient = openAiClient.GetChatClient(modelId);

// ── Product catalog (embedded) ──────────────────────────────────────────
var products = new[]
{
    new Product("1", "Zava ProGrip Cordless Drill",
        "Take on any DIY project with the Zava ProGrip Cordless Drill. Equipped with a brushless motor and 20V lithium-ion battery, this drill delivers up to 500 in-lbs of torque. Two-speed gearbox, integrated LED work light, ergonomic soft-grip handle, and keyless chuck accepting bits up to 1/2 inch."),
    new Product("2", "Zava UltraSmooth Interior Paint",
        "Transform any room with Zava UltraSmooth Interior Paint. Low-VOC, water-based latex paint with exceptional one-coat coverage. Available in over 200 designer colours with a velvety matte finish. Built-in primer, splatter-resistant consistency, and easy soap-and-water cleanup."),
    new Product("3", "Zava TitanLock Tool Chest",
        "Keep your workshop organised with the Zava TitanLock Tool Chest. Heavy-gauge steel with powder-coated finish, five ball-bearing drawer slides, integrated power strip, lockable drawers, foam liner inserts, and rolling casters with brakes."),
    new Product("4", "Zava EcoBoard Composite Decking",
        "Build the backyard of your dreams with Zava EcoBoard Composite Decking. Made from 95% recycled materials, resists rot, warping, and insect damage. Hidden fastener grooves, slip-resistant surface, available in six nature-inspired shades. 25-year structural warranty."),
    new Product("5", "Zava BrightBeam LED Work Light",
        "Light up any job site with the Zava BrightBeam LED Work Light. 5,000 lumens, 120-degree beam angle, rugged aluminium housing, IP65 weather rating, adjustable tripod stand, and stepless dimmer control. 50,000-hour rated lifespan.")
};

// ── Default inputs ──────────────────────────────────────────────────────
var researchContext = "Can you find the latest DIY home improvement trends and weekend renovation projects?";
var productContext = "Can you use a selection of power tools and paints as context?";
var assignment = "Write a fun and engaging article that includes the research and product information. The article should be between 800 and 1000 words.";

var sep = new string('=', 60);
Console.WriteLine(sep);
Console.WriteLine("Zava Creative Writer — Multi-Agent Pipeline");
Console.WriteLine(sep);

var feedback = "No Feedback";

// ── Step 1: Researcher Agent ────────────────────────────────────────────
Console.WriteLine("\n[Researcher] Gathering information...");
var researchResult = RunResearcher(researchContext, feedback);
Console.WriteLine($"[Researcher] Done.");

// ── Step 2: Product Agent ───────────────────────────────────────────────
Console.WriteLine("\n[Product] Searching product catalog...");
var productResult = RunProductSearch(productContext);
Console.WriteLine($"[Product] Found {productResult.Count} product(s): {string.Join(", ", productResult.Select(p => p.Title))}");

// ── Step 3: Writer Agent (streaming) ────────────────────────────────────
Console.WriteLine("\n[Writer] Drafting article...\n");
var writerOutput = RunWriter(researchContext, researchResult, productContext, productResult, assignment, feedback);
Console.WriteLine();

var (article, writerFeedback) = SplitArticleFeedback(writerOutput);

// ── Step 4: Editor Agent ────────────────────────────────────────────────
Console.WriteLine("\n[Editor] Reviewing article...");
var editorResponse = RunEditor(article, writerFeedback);
Console.WriteLine($"[Editor] Decision: {editorResponse.Decision.ToUpper()}");
if (!string.IsNullOrEmpty(editorResponse.EditorFeedback))
    Console.WriteLine($"[Editor] Feedback: {editorResponse.EditorFeedback}");

// ── Feedback loop (max 2 retries) ───────────────────────────────────────
var retryCount = 0;
while (editorResponse.Decision.StartsWith("revise", StringComparison.OrdinalIgnoreCase) && retryCount < 2)
{
    retryCount++;
    Console.WriteLine($"\n--- Revision {retryCount} ---");

    var resFeedback = editorResponse.ResearchFeedback ?? "No Feedback";
    if (resFeedback.Length > 500) resFeedback = resFeedback[..500];
    var edFeedback = editorResponse.EditorFeedback ?? "No Feedback";
    if (edFeedback.Length > 500) edFeedback = edFeedback[..500];

    Console.WriteLine("[Researcher] Re-researching with feedback...");
    researchResult = RunResearcher(researchContext, resFeedback);

    Console.WriteLine("[Writer] Re-drafting article...\n");
    writerOutput = RunWriter(researchContext, researchResult, productContext, productResult, assignment, edFeedback);
    Console.WriteLine();

    (article, writerFeedback) = SplitArticleFeedback(writerOutput);

    Console.WriteLine("[Editor] Reviewing revised article...");
    editorResponse = RunEditor(article, writerFeedback);
    Console.WriteLine($"[Editor] Decision: {editorResponse.Decision.ToUpper()}");
    if (!string.IsNullOrEmpty(editorResponse.EditorFeedback))
        Console.WriteLine($"[Editor] Feedback: {editorResponse.EditorFeedback}");
}

Console.WriteLine($"\n{sep}");
Console.WriteLine("Multi-agent pipeline complete!");
Console.WriteLine(sep);

// ═══════════════════════════════════════════════════════════════════════
// Agent implementations
// ═══════════════════════════════════════════════════════════════════════

string RunResearcher(string instructions, string fb)
{
    var userContent = $"Topic: {instructions}";
    if (fb != "No feedback" && fb != "No Feedback")
        userContent += $"\n\nPrevious feedback to address: {fb}";

    var systemPrompt = """
        You are a research assistant equipped with broad knowledge.
        Given a topic, produce a structured JSON response with relevant findings.
        Return ONLY valid JSON (no markdown code fences) with this structure:
        {"web": [
          {"url": "", "name": "Source Title", "description": "Concise summary of the finding."},
          ...
        ]}
        Include 3-5 items. If feedback is provided, refine your research accordingly.
        """;

    var completion = chatClient.CompleteChat(
        new ChatMessage[]
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userContent)
        },
        new ChatCompletionOptions { Temperature = 0.7f, MaxOutputTokenCount = 1500 });

    return completion.Value.Content[0].Text;
}

List<Product> RunProductSearch(string context)
{
    var systemPrompt = "You produce search queries for a product catalog. " +
                       "Given a context, return a JSON array of 3-5 short search " +
                       "query strings. Return ONLY the JSON array, no other text.";

    var completion = chatClient.CompleteChat(
        new ChatMessage[]
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(context)
        },
        new ChatCompletionOptions { Temperature = 0.3f, MaxOutputTokenCount = 300 });

    var raw = completion.Value.Content[0].Text.Trim()
        .TrimStart('`').TrimEnd('`');
    if (raw.StartsWith("json", StringComparison.OrdinalIgnoreCase))
        raw = raw[4..].TrimStart();

    List<string> queries;
    try
    {
        queries = JsonSerializer.Deserialize<List<string>>(raw) ?? [context];
    }
    catch
    {
        queries = [context];
    }

    var seen = new HashSet<string>();
    var results = new List<Product>();
    foreach (var q in queries)
    {
        foreach (var p in KeywordSearch(q, 3))
        {
            if (seen.Add(p.Id))
                results.Add(p);
        }
    }
    return results;
}

List<Product> KeywordSearch(string query, int topK)
{
    var queryWords = new HashSet<string>(query.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries));
    var scored = products.Select(p =>
    {
        var text = $"{p.Title} {p.Content}".ToLower();
        var productWords = new HashSet<string>(text.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        var overlap = queryWords.Intersect(productWords).Count();
        return (Score: overlap, Product: p);
    })
    .OrderByDescending(x => x.Score)
    .Take(topK)
    .Select(x => x.Product)
    .ToList();

    return scored;
}

string RunWriter(string resContext, string research, string prodContext, List<Product> prods, string assign, string fb)
{
    const int maxResearchChars = 1500;
    const int maxProductChars = 150;
    const int maxFeedbackChars = 500;
    const int maxTokens = 1500;

    // Build context strings
    var researchLines = "";
    try
    {
        using var doc = JsonDocument.Parse(research);
        if (doc.RootElement.TryGetProperty("web", out var web))
        {
            foreach (var item in web.EnumerateArray())
            {
                var name = item.TryGetProperty("name", out var n) ? n.GetString() : "";
                var desc = item.TryGetProperty("description", out var d) ? d.GetString() : "";
                researchLines += $"- {name}: {desc}\n";
            }
        }
    }
    catch
    {
        researchLines = $"- {research}\n";
    }
    if (researchLines.Length > maxResearchChars)
        researchLines = researchLines[..maxResearchChars];

    var productLines = string.Join("\n", prods.Select(p =>
        $"- {p.Title}: {(p.Content.Length > maxProductChars ? p.Content[..maxProductChars] : p.Content)}"));

    var trimmedFeedback = fb.Length > maxFeedbackChars ? fb[..maxFeedbackChars] : fb;

    var userMessage = $"""
        # Assignment
        {assign}

        # Research Context
        {resContext}

        # Web Research
        {researchLines}

        # Product Context
        {prodContext}

        # Products
        {productLines}

        # Feedback from editor
        {trimmedFeedback}
        """;

    var systemPrompt = """
        You are an expert copywriter for Zava Retail, a DIY and home improvement company.
        You take research from a web researcher as well as product information from the Zava product catalog
        to produce a fun and engaging article that can be used as a magazine article or a blog post.
        The goal is to engage DIY enthusiasts and provide them with a fun, informative article about
        home improvement, renovation projects, and the tools and materials that make them possible.
        The article should be between 800 and 1000 words.

        After the article, add a line with "---" and then provide brief feedback notes about what could be
        improved in the article.
        """;

    // Stream the response
    var result = new System.Text.StringBuilder();
    var completionUpdates = chatClient.CompleteChatStreaming(
        new ChatMessage[]
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userMessage)
        },
        new ChatCompletionOptions { MaxOutputTokenCount = maxTokens });

    try
    {
        foreach (var update in completionUpdates)
        {
            if (update.ContentUpdate.Count > 0)
            {
                var text = update.ContentUpdate[0].Text;
                Console.Write(text);
                result.Append(text);
            }
        }
    }
    catch (Exception ex) when (ex.Message.Contains("Premature") || ex.Message.Contains("closed"))
    {
        Console.WriteLine("\n[Writer] Stream closed early \u2014 using partial output.");
    }

    return result.ToString();
}

EditorDecision RunEditor(string art, string fb)
{
    var systemPrompt = """
        You are an editor at a publishing company. Review the article and feedback below.
        Decide whether to accept or request revisions.

        Respond ONLY with valid JSON (no code fences) in one of these formats:

        If the article is publication-ready:
        {"decision": "accept", "researchFeedback": "...", "editorFeedback": "..."}

        If the article needs more work:
        {"decision": "revise", "researchFeedback": "...", "editorFeedback": "..."}
        """;

    var completion = chatClient.CompleteChat(
        new ChatMessage[]
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage($"# Article\n{art}\n\n# Feedback\n{fb}")
        },
        new ChatCompletionOptions { Temperature = 0.2f, MaxOutputTokenCount = 512 });

    var raw = completion.Value.Content[0].Text.Trim()
        .TrimStart('`').TrimEnd('`');
    if (raw.StartsWith("json", StringComparison.OrdinalIgnoreCase))
        raw = raw[4..].TrimStart();

    try
    {
        var parsed = JsonSerializer.Deserialize<EditorDecision>(raw, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return parsed ?? new EditorDecision("accept", "No Feedback", raw);
    }
    catch
    {
        return new EditorDecision("accept", "No Feedback", raw);
    }
}

(string Article, string Feedback) SplitArticleFeedback(string writerOutput)
{
    var parts = writerOutput.Split("---", 2);
    var art = parts[0].Trim();
    var fb = parts.Length > 1 ? parts[1].Trim() : "No Feedback";
    return (art, fb);
}

// ── Records ─────────────────────────────────────────────────────────────
record Product(string Id, string Title, string Content);
record EditorDecision(string Decision, string ResearchFeedback, string EditorFeedback);
