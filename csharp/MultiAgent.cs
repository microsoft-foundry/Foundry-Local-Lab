using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace Examples;

/// <summary>
/// Part 6: Multi-agent workflow — Researcher → Writer → Editor pipeline.
/// Three AIAgent instances collaborate sequentially to produce a reviewed blog post.
/// Uses the Microsoft Agent Framework's AsAIAgent() extension method.
/// </summary>
public static class MultiAgent
{
    public static async Task RunAsync()
    {
        var alias = "phi-3.5-mini";

        // Step 1: Start the Foundry Local service
        Console.WriteLine("Starting Foundry Local service...");
        await FoundryLocalManager.CreateAsync(
            new Configuration
            {
                AppName = "FoundryLocalSamples",
                Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
            }, NullLogger.Instance, default);
        var manager = FoundryLocalManager.Instance;
        await manager.StartWebServiceAsync(default);

        // Step 2: Get the model from the catalog
        var catalog = await manager.GetCatalogAsync(default);
        var model = await catalog.GetModelAsync(alias, default);

        // Step 3: Check if the model is already downloaded
        var isCached = await model.IsCachedAsync(default);

        if (isCached)
        {
            Console.WriteLine($"Model already downloaded: {alias}");
        }
        else
        {
            Console.WriteLine($"Downloading model: {alias} (this may take several minutes)...");
            await model.DownloadAsync(null, default);
            Console.WriteLine($"Download complete: {alias}");
        }

        // Step 4: Load the model into memory
        Console.WriteLine($"Loading model: {alias}...");
        await model.LoadAsync(default);
        Console.WriteLine($"Model: {model.Id}");
        Console.WriteLine($"Endpoint: {manager.Urls[0]}\n");

        var key = new ApiKeyCredential("foundry-local");
        var openAiClient = new OpenAIClient(key, new OpenAIClientOptions
        {
            Endpoint = new Uri(manager.Urls[0] + "/v1")
        });
        var chatClient = openAiClient.GetChatClient(model.Id);

        // ── Define agents using AsAIAgent() ─────────────────────────────
        AIAgent researcher = chatClient.AsAIAgent(
            name: "Researcher",
            instructions:
                "You are a research assistant. When given a topic, provide a concise " +
                "collection of key facts, statistics, and background information. " +
                "Organize your findings as bullet points."
        );

        AIAgent writer = chatClient.AsAIAgent(
            name: "Writer",
            instructions:
                "You are a skilled blog writer. Using the research notes provided, " +
                "write a short, engaging blog post (3-4 paragraphs). " +
                "Include a catchy title. Do not make up facts beyond what is given."
        );

        AIAgent editor = chatClient.AsAIAgent(
            name: "Editor",
            instructions:
                "You are a senior editor. Review the blog post below for clarity, " +
                "grammar, and factual consistency with the research notes. " +
                "Provide a brief editorial verdict: ACCEPT if the post is " +
                "publication-ready, or REVISE with specific suggestions."
        );

        var topic = "The history and future of renewable energy";

        // ── Agent workflow: Researcher → Writer → Editor ────────────────
        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"Topic: {topic}");
        Console.WriteLine(new string('=', 60));

        // Step 1 — Research
        Console.WriteLine("\n[Researcher] Gathering information...");
        var researchNotes = await researcher.RunAsync(
            $"Research the following topic and provide 5 key facts as bullet points:\n{topic}");
        Console.WriteLine($"\n--- Research Notes ---\n{researchNotes}\n");

        // Step 2 — Write
        Console.WriteLine("[Writer] Drafting the article...");
        var draft = await writer.RunAsync(
            $"Write a short blog post (2-3 paragraphs) based on these research notes:\n\n{researchNotes}");
        Console.WriteLine($"\n--- Draft Article ---\n{draft}\n");

        // Step 3 — Edit
        Console.WriteLine("[Editor] Reviewing the article...");
        var verdict = await editor.RunAsync(
            $"Review this article for quality and accuracy.\n\n" +
            $"Research notes:\n{researchNotes}\n\n" +
            $"Article:\n{draft}");
        Console.WriteLine($"\n--- Editor Verdict ---\n{verdict}\n");

        Console.WriteLine(new string('=', 60));
        Console.WriteLine("Multi-agent workflow complete!");

        // Cleanup: unload the model to release resources
        await model.UnloadAsync(default);
    }
}
