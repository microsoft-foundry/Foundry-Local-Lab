import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

// ---------------------------------------------------------------------------
// Simple Agent class — wraps an OpenAI chat client with persistent
// system instructions, mirroring the ChatAgent pattern from the
// Microsoft Agent Framework (Python / C#).
// ---------------------------------------------------------------------------
class ChatAgent {
  constructor({ client, modelId, instructions, name }) {
    this.client = client;
    this.modelId = modelId;
    this.instructions = instructions;
    this.name = name;
    this.history = [];
  }

  async run(userMessage) {
    const messages = [
      { role: "system", content: this.instructions },
      ...this.history,
      { role: "user", content: userMessage },
    ];

    const response = await this.client.chat.completions.create({
      model: this.modelId,
      messages,
    });

    const assistantMessage = response.choices[0].message.content;

    // Keep conversation history for multi-turn interactions
    this.history.push({ role: "user", content: userMessage });
    this.history.push({ role: "assistant", content: assistantMessage });

    return { text: assistantMessage };
  }
}

// ---------------------------------------------------------------------------
// Main
// ---------------------------------------------------------------------------
async function main() {
  const alias = "phi-3.5-mini";

  // Step 1: Create a FoundryLocalManager and start the service
  console.log("Starting Foundry Local service...");
  FoundryLocalManager.create({ appName: "FoundryLocalWorkshop" });
  const manager = FoundryLocalManager.instance;
  await manager.startWebService();

  // Step 2: Get the model from the catalog
  const catalog = manager.catalog;
  const model = await catalog.getModel(alias);

  if (model.isCached) {
    console.log(`Model already downloaded: ${alias}`);
  } else {
    console.log(
      `Downloading model: ${alias} (this may take several minutes)...`
    );
    await model.download();
    console.log(`Download complete: ${alias}`);
  }

  // Step 3: Load the model into memory
  console.log(`Loading model: ${alias}...`);
  await model.load();
  console.log(`Model loaded: ${model.id}`);
  console.log(`Foundry Local endpoint: ${manager.urls[0]}`);

  // Create an OpenAI client pointing to the local Foundry service
  const client = new OpenAI({
    baseURL: manager.urls[0] + "/v1",
    apiKey: "foundry-local",
  });

  // Create an agent with a specific persona
  const agent = new ChatAgent({
    client,
    modelId: model.id,
    instructions: "You are good at telling jokes.",
    name: "Joker",
  });

  // Run the agent
  const result = await agent.run("Tell me a joke about a pirate.");
  console.log(result.text);
}

main();
