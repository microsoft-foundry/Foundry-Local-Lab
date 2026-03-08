using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace Examples;

/// <summary>
/// Part 3: Basic streaming chat completion with Foundry Local.
/// </summary>
public static class BasicChat
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
        Console.WriteLine($"Loaded model: {model.Id}");
        Console.WriteLine($"Endpoint: {manager.Urls[0]}\n");

        // Create an OpenAI client pointing to the local service
        var key = new ApiKeyCredential("foundry-local");
        var client = new OpenAIClient(key, new OpenAIClientOptions
        {
            Endpoint = new Uri(manager.Urls[0] + "/v1")
        });

        var chatClient = client.GetChatClient(model.Id);

        // Stream a response
        var completionUpdates = chatClient.CompleteChatStreaming("What is the golden ratio?");

        foreach (var update in completionUpdates)
        {
            if (update.ContentUpdate.Count > 0)
            {
                Console.Write(update.ContentUpdate[0].Text);
            }
        }
        Console.WriteLine();
    }
}
