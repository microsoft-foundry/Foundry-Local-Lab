using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace Examples;

/// <summary>
/// Part 5: Single AI agent using the Microsoft Agent Framework.
/// Uses the AsAIAgent() extension from Microsoft.Agents.AI.OpenAI
/// to create a proper AIAgent backed by Foundry Local.
/// </summary>
public static class SingleAgent
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

        var key = new ApiKeyCredential("foundry-local");
        var client = new OpenAIClient(key, new OpenAIClientOptions
        {
            Endpoint = new Uri(manager.Urls[0] + "/v1")
        });

        // Create an AIAgent using the Agent Framework extension method
        AIAgent joker = client
            .GetChatClient(model.Id)
            .AsAIAgent(
                instructions: "You are good at telling jokes. Keep your jokes short and family-friendly.",
                name: "Joker"
            );

        Console.WriteLine($"Agent: Joker");
        Console.WriteLine(new string('-', 40));

        // Run the agent with a prompt (non-streaming)
        var prompt = "Tell me a joke about a pirate.";
        Console.WriteLine($"User: {prompt}\n");

        var response = await joker.RunAsync(prompt);
        Console.WriteLine($"Joker: {response}\n");

        // Demonstrate streaming response
        var followUp = "Now tell me one about a programmer.";
        Console.WriteLine($"User: {followUp}\n");

        Console.Write("Joker: ");
        await foreach (var update in joker.RunStreamingAsync(followUp))
        {
            Console.Write(update);
        }
        Console.WriteLine();
    }
}
