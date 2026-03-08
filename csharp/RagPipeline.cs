using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace Examples;

/// <summary>
/// Part 4: Retrieval-Augmented Generation (RAG) running entirely on-device.
/// A small knowledge base is searched with keyword overlap, and the most
/// relevant chunks are injected into the prompt as context.
/// </summary>
public static class RagPipeline
{
    // ── Local knowledge base ────────────────────────────────────────────
    private static readonly List<(string Title, string Content)> KnowledgeBase =
    [
        ("Foundry Local Overview",
         "Foundry Local brings the power of Azure AI Foundry to your local " +
         "device without requiring an Azure subscription. It allows you to " +
         "run Generative AI models directly on your local hardware with no " +
         "sign-up required, keeping all data processing on-device for " +
         "enhanced privacy and security."),

        ("Supported Hardware",
         "Foundry Local automatically selects the best model variant for " +
         "your hardware. If you have an Nvidia CUDA GPU it downloads the " +
         "CUDA-optimized model. For a Qualcomm NPU it downloads the " +
         "NPU-optimized model. Otherwise it uses the CPU-optimized model. " +
         "Performance is optimized through ONNX Runtime and hardware " +
         "acceleration."),

        ("OpenAI-Compatible API",
         "Foundry Local exposes an OpenAI-compatible REST API so you can " +
         "use the standard OpenAI Python, JavaScript or C# SDKs to " +
         "interact with local models. The endpoint is dynamically assigned " +
         "— always obtain it from the SDK's manager.endpoint property " +
         "rather than hard-coding a port number."),

        ("Model Catalog",
         "You can browse all available models at foundrylocal.ai or by " +
         "running 'foundry model list' in your terminal. Popular models " +
         "include Phi-3.5-mini, Phi-4-mini, Qwen 2.5, Mistral, and " +
         "DeepSeek-R1. Models are downloaded on first use and cached " +
         "locally for future sessions."),

        ("Installation",
         "On Windows install Foundry Local with: " +
         "winget install Microsoft.FoundryLocal. " +
         "On macOS install with: " +
         "brew install microsoft/foundrylocal/foundrylocal. " +
         "You can also download installers from the GitHub releases page."),
    ];

    // ── Simple keyword retrieval ────────────────────────────────────────
    private static List<(string Title, string Content)> Retrieve(string query, int topK = 2)
    {
        var queryWords = new HashSet<string>(
            query.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));

        return KnowledgeBase
            .Select(chunk =>
            {
                var chunkWords = new HashSet<string>(
                    chunk.Content.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));
                var overlap = queryWords.Intersect(chunkWords).Count();
                return (Overlap: overlap, Chunk: chunk);
            })
            .OrderByDescending(x => x.Overlap)
            .Take(topK)
            .Select(x => x.Chunk)
            .ToList();
    }

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
        Console.WriteLine($"Loaded model: {model.Id}");

        var key = new ApiKeyCredential("foundry-local");
        var client = new OpenAIClient(key, new OpenAIClientOptions
        {
            Endpoint = new Uri(manager.Urls[0] + "/v1")
        });
        var chatClient = client.GetChatClient(model.Id);

        // User question
        var question = "How do I install Foundry Local and what hardware does it support?";
        Console.WriteLine($"\nQuestion: {question}\n");

        // Retrieve relevant context
        var contextChunks = Retrieve(question);
        var contextText = string.Join("\n\n",
            contextChunks.Select(c => $"### {c.Title}\n{c.Content}"));

        Console.WriteLine("--- Retrieved Context ---");
        Console.WriteLine(contextText);
        Console.WriteLine("-------------------------\n");

        // Build grounded prompt
        var systemPrompt =
            "You are a helpful assistant. Answer the user's question using ONLY " +
            "the information provided in the context below. If the context does " +
            "not contain enough information, say so.\n\n" +
            $"Context:\n{contextText}";

        // Stream the answer
        Console.WriteLine("Answer:");
        var updates = chatClient.CompleteChatStreaming(
        [
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(question),
        ]);

        foreach (var update in updates)
        {
            if (update.ContentUpdate.Count > 0)
            {
                Console.Write(update.ContentUpdate[0].Text);
            }
        }
        Console.WriteLine();
    }
}
