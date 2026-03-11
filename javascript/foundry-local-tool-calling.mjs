import { FoundryLocalManager } from "foundry-local-sdk";

// Tool calling requires a model that supports it.
// Qwen 2.5 models and Phi-4-mini support tool calling in Foundry Local.
const alias = "qwen2.5-0.5b";

// Step 1: Start Foundry Local and get the model
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "ToolCallingDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel(alias);
if (!model.isCached) {
  console.log(`Downloading ${alias}...`);
  await model.download();
}
await model.load();

// Step 2: Get the SDK's ChatClient (no OpenAI SDK needed)
const chatClient = model.createChatClient();

// Step 3: Define tools the model can call
const tools = [
  {
    type: "function",
    function: {
      name: "get_weather",
      description: "Get the current weather for a given city",
      parameters: {
        type: "object",
        properties: {
          city: { type: "string", description: "The city name, e.g. London" },
        },
        required: ["city"],
      },
    },
  },
  {
    type: "function",
    function: {
      name: "get_population",
      description: "Get the population of a given city",
      parameters: {
        type: "object",
        properties: {
          city: { type: "string", description: "The city name, e.g. London" },
        },
        required: ["city"],
      },
    },
  },
];

// Step 4: Simulate tool execution
function executeTool(name, args) {
  if (name === "get_weather") {
    return JSON.stringify({
      city: args.city,
      temperature: "18°C",
      condition: "Partly cloudy",
    });
  }
  if (name === "get_population") {
    const pops = {
      london: "8.8 million",
      paris: "2.1 million",
      tokyo: "14 million",
    };
    return JSON.stringify({
      city: args.city,
      population: pops[args.city?.toLowerCase()] || "Unknown",
    });
  }
  return JSON.stringify({ error: "Unknown tool" });
}

// Step 5: Send a message that should trigger tool calls
const messages = [
  {
    role: "system",
    content:
      "You are a helpful assistant. Use the provided tools to answer questions.",
  },
  { role: "user", content: "What is the weather like in London?" },
];

console.log("\nUser: What is the weather like in London?\n");

const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Step 6: Handle tool calls if the model requests them
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
  console.log(
    `Model requested ${assistantMessage.tool_calls.length} tool call(s):`
  );
  messages.push(assistantMessage);

  for (const toolCall of assistantMessage.tool_calls) {
    const fnName = toolCall.function.name;
    const fnArgs = JSON.parse(toolCall.function.arguments);
    console.log(`  → ${fnName}(${JSON.stringify(fnArgs)})`);

    const result = executeTool(fnName, fnArgs);
    messages.push({
      role: "tool",
      tool_call_id: toolCall.id,
      content: result,
    });
  }

  // Step 7: Send the tool results back for the final answer
  console.log("\nFinal response:");
  const final = await chatClient.completeChat(messages, tools);
  console.log(final.choices[0].message.content);
} else {
  // Model answered directly without calling tools
  console.log("Response:", assistantMessage.content);
}
