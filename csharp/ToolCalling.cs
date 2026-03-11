using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;

namespace Examples;

/// <summary>
/// Part 11: Tool/function calling with a local model.
/// </summary>
public static class ToolCalling
{
    public static async Task RunAsync()
    {
        // Tool calling requires a compatible model — Qwen 2.5 models support it
        var alias = "qwen2.5-0.5b";

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

        // Step 2: Get and load the model
        var catalog = await manager.GetCatalogAsync(default);
        var model = await catalog.GetModelAsync(alias, default);

        var isCached = await model.IsCachedAsync(default);
        if (!isCached)
        {
            Console.WriteLine($"Downloading model: {alias} (this may take several minutes)...");
            await model.DownloadAsync(null, default);
        }

        Console.WriteLine($"Loading model: {alias}...");
        await model.LoadAsync(default);
        Console.WriteLine($"Loaded model: {model.Id}");

        // Step 3: Create the OpenAI chat client
        var key = new ApiKeyCredential("foundry-local");
        var client = new OpenAIClient(key, new OpenAIClientOptions
        {
            Endpoint = new Uri(manager.Urls[0] + "/v1")
        });
        var chatClient = client.GetChatClient(model.Id);

        // Step 4: Define tools the model can call
        ChatTool getWeatherTool = ChatTool.CreateFunctionTool(
            functionName: "get_weather",
            functionDescription: "Get the current weather for a given city",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "city": {
                        "type": "string",
                        "description": "The city name, e.g. London"
                    }
                },
                "required": ["city"]
            }
            """));

        ChatTool getPopulationTool = ChatTool.CreateFunctionTool(
            functionName: "get_population",
            functionDescription: "Get the population of a given city",
            functionParameters: BinaryData.FromString("""
            {
                "type": "object",
                "properties": {
                    "city": {
                        "type": "string",
                        "description": "The city name, e.g. London"
                    }
                },
                "required": ["city"]
            }
            """));

        var options = new ChatCompletionOptions();
        options.Tools.Add(getWeatherTool);
        options.Tools.Add(getPopulationTool);

        // Step 5: Send a message that should trigger tool use
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are a helpful assistant. Use the provided tools to answer questions."),
            new UserChatMessage("What is the weather like in London?")
        };

        Console.WriteLine("\nUser: What is the weather like in London?\n");

        var completion = await chatClient.CompleteChatAsync(messages, options);

        // Step 6: Handle tool calls
        if (completion.Value.FinishReason == ChatFinishReason.ToolCalls)
        {
            Console.WriteLine($"Model requested {completion.Value.ToolCalls.Count} tool call(s):");
            messages.Add(new AssistantChatMessage(completion.Value));

            foreach (var toolCall in completion.Value.ToolCalls)
            {
                Console.WriteLine($"  → {toolCall.FunctionName}({toolCall.FunctionArguments})");
                var result = ExecuteTool(toolCall.FunctionName, toolCall.FunctionArguments.ToString());
                messages.Add(new ToolChatMessage(toolCall.Id, result));
            }

            // Step 7: Send tool results back for the final answer
            Console.WriteLine("\nFinal response:");
            var finalCompletion = await chatClient.CompleteChatAsync(messages, options);
            Console.WriteLine(finalCompletion.Value.Content[0].Text);
        }
        else
        {
            Console.WriteLine($"Response: {completion.Value.Content[0].Text}");
        }
    }

    private static string ExecuteTool(string name, string argsJson)
    {
        using var doc = JsonDocument.Parse(argsJson);
        var city = doc.RootElement.GetProperty("city").GetString() ?? "Unknown";

        return name switch
        {
            "get_weather" => JsonSerializer.Serialize(new { city, temperature = "18°C", condition = "Partly cloudy" }),
            "get_population" => JsonSerializer.Serialize(new { city, population = GetPopulation(city) }),
            _ => JsonSerializer.Serialize(new { error = "Unknown tool" })
        };
    }

    private static string GetPopulation(string city)
    {
        return city.ToLowerInvariant() switch
        {
            "london" => "8.8 million",
            "paris" => "2.1 million",
            "tokyo" => "14 million",
            _ => "Unknown"
        };
    }
}
