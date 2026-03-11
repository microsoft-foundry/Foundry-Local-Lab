// Zava Creative Writer — C# Minimal API with Web UI
//
// Exposes the same multi-agent pipeline as a REST endpoint and
// serves the shared UI from ../ui/.
//
// Endpoints:
//   GET  /            — serves the static UI
//   POST /api/article — runs the pipeline, streams NDJSON
//
// Usage: dotnet run

using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Text;
using System.Text.Json;

// ── Model loading ───────────────────────────────────────────────────────
var alias = "phi-3.5-mini";

Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "ZavaCreativeWriterWeb",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

var catalog = await manager.GetCatalogAsync(default);
var catalogModel = await catalog.GetModelAsync(alias, default);
var isCached = await catalogModel.IsCachedAsync(default);

if (isCached)
    Console.WriteLine($"Model already downloaded: {alias}");
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

// ── Product catalogue ───────────────────────────────────────────────────
var products = new[]
{
    new Product("1", "Zava ProGrip Cordless Drill",
        "Take on any DIY project with the Zava ProGrip Cordless Drill. Equipped with a brushless motor and 20V lithium-ion battery, this drill delivers up to 500 in-lbs of torque."),
    new Product("2", "Zava UltraSmooth Interior Paint",
        "Transform any room with Zava UltraSmooth Interior Paint. Low-VOC, water-based latex paint with exceptional one-coat coverage. Available in over 200 designer colours."),
    new Product("3", "Zava TitanLock Tool Chest",
        "Keep your workshop organised with the Zava TitanLock Tool Chest. Heavy-gauge steel with powder-coated finish, five ball-bearing drawer slides, and lockable drawers."),
    new Product("4", "Zava EcoBoard Composite Decking",
        "Build the backyard of your dreams with Zava EcoBoard Composite Decking. Made from 95% recycled materials, resists rot, warping, and insect damage."),
    new Product("5", "Zava BrightBeam LED Work Light",
        "Light up any job site with the Zava BrightBeam LED Work Light. 5,000 lumens, 120-degree beam angle, rugged aluminium housing, IP65 weather rating.")
};

// ── Build the web app ───────────────────────────────────────────────────
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Serve the shared UI
var uiPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "ui"));
if (!Directory.Exists(uiPath))
    uiPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "ui"));

if (Directory.Exists(uiPath))
{
    app.UseDefaultFiles(new DefaultFilesOptions
    {
        FileProvider = new PhysicalFileProvider(uiPath)
    });
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(uiPath)
    });
}
else
{
    Console.WriteLine($"WARNING: UI directory not found at {uiPath}");
}

// ── POST /api/article ───────────────────────────────────────────────────
app.MapPost("/api/article", async (HttpContext ctx) =>
{
    var body = await JsonSerializer.DeserializeAsync<ArticleRequest>(ctx.Request.Body,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (body is null)
    {
        ctx.Response.StatusCode = 400;
        await ctx.Response.WriteAsync("Invalid request body.");
        return;
    }

    ctx.Response.ContentType = "text/event-stream; charset=utf-8";
    ctx.Response.Headers.CacheControl = "no-cache";
    ctx.Response.Headers.Connection = "keep-alive";

    var feedback = "No Feedback";

    async Task SendLine(object obj)
    {
        var json = JsonSerializer.Serialize(obj).Replace("\n", "") + "\n";
        await ctx.Response.WriteAsync(json);
        await ctx.Response.Body.FlushAsync();
    }

    try
    {
        // 1. Researcher
        await SendLine(new { type = "message", message = "Starting researcher agent task...", data = new { } });
        var researchResult = RunResearcher(body.Research, feedback);
        await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

        // 2. Product search
        await SendLine(new { type = "message", message = "Starting marketing agent task...", data = new { } });
        var productResult = RunProductSearch(body.Products);
        await SendLine(new { type = "marketing", message = "Completed marketing task", data = productResult.Select(p => new { p.Title, p.Content }) });

        // 3. Writer (streaming)
        await SendLine(new { type = "message", message = "Starting writer agent task...", data = new { } });
        await SendLine(new { type = "writer", message = "Writer started", data = new { start = true } });

        var writerOutput = new StringBuilder();
        var completionUpdates = chatClient.CompleteChatStreaming(
            BuildWriterMessages(body.Research, researchResult, body.Products, productResult, body.Assignment, feedback),
            new ChatCompletionOptions { MaxOutputTokenCount = 1500 });

        foreach (var update in completionUpdates)
        {
            if (update.ContentUpdate.Count > 0)
            {
                var text = update.ContentUpdate[0].Text;
                writerOutput.Append(text);
                await SendLine(new { type = "partial", message = "token", data = new { text } });
            }
        }

        await SendLine(new { type = "writer", message = "Writer complete", data = new { complete = true } });

        var (article, writerFeedback) = SplitArticleFeedback(writerOutput.ToString());

        // 4. Editor
        await SendLine(new { type = "message", message = "Starting editor agent task...", data = new { } });
        var editorResponse = RunEditor(article, writerFeedback);
        await SendLine(new { type = "editor", message = "Completed editor task", data = (object)editorResponse });

        // Feedback loop (max 2 retries)
        var retryCount = 0;
        while (editorResponse.Decision.StartsWith("revise", StringComparison.OrdinalIgnoreCase) && retryCount < 2)
        {
            retryCount++;
            var resFeedback = (editorResponse.ResearchFeedback ?? "No Feedback")[..Math.Min(500, editorResponse.ResearchFeedback?.Length ?? 0)];
            var edFeedback = (editorResponse.EditorFeedback ?? "No Feedback")[..Math.Min(500, editorResponse.EditorFeedback?.Length ?? 0)];

            await SendLine(new { type = "message", message = $"Revision {retryCount}: re-running pipeline...", data = new { } });

            researchResult = RunResearcher(body.Research, resFeedback);
            await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

            await SendLine(new { type = "writer", message = "Writer started", data = new { start = true } });

            writerOutput.Clear();
            var retryUpdates = chatClient.CompleteChatStreaming(
                BuildWriterMessages(body.Research, researchResult, body.Products, productResult, body.Assignment, edFeedback),
                new ChatCompletionOptions { MaxOutputTokenCount = 1500 });

            foreach (var update in retryUpdates)
            {
                if (update.ContentUpdate.Count > 0)
                {
                    var text = update.ContentUpdate[0].Text;
                    writerOutput.Append(text);
                    await SendLine(new { type = "partial", message = "token", data = new { text } });
                }
            }

            await SendLine(new { type = "writer", message = "Writer complete", data = new { complete = true } });

            (article, writerFeedback) = SplitArticleFeedback(writerOutput.ToString());

            editorResponse = RunEditor(article, writerFeedback);
            await SendLine(new { type = "editor", message = "Completed editor task", data = (object)editorResponse });
        }
    }
    catch (Exception ex)
    {
        await SendLine(new { type = "error", message = ex.Message, data = new { error = ex.ToString() } });
    }
});

Console.WriteLine("\nZava Creative Writer UI is running at http://localhost:5000\n");
app.Run("http://localhost:5000");

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
    var systemPrompt = "You produce search queries for a product catalogue. " +
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

ChatMessage[] BuildWriterMessages(string resContext, string research, string prodContext, List<Product> prods, string assign, string fb)
{
    const int maxResearchChars = 1500;
    const int maxProductChars = 150;
    const int maxFeedbackChars = 500;

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
        You take research from a web researcher as well as product information from the Zava product catalogue
        to produce a fun and engaging article that can be used as a magazine article or a blog post.
        The goal is to engage DIY enthusiasts and provide them with a fun, informative article about
        home improvement, renovation projects, and the tools and materials that make them possible.
        The article should be between 800 and 1000 words.

        After the article, add a line with "---" and then provide brief feedback notes about what could be
        improved in the article.
        """;

    return
    [
        new SystemChatMessage(systemPrompt),
        new UserChatMessage(userMessage)
    ];
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
record ArticleRequest(string Research, string Products, string Assignment);
