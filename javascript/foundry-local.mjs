import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

// By using an alias, the most suitable model variant will be
// downloaded for your end-user's device hardware.
const alias = "phi-3.5-mini";

// Step 1: Create a FoundryLocalManager and start the service
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "FoundryLocalWorkshop" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Step 2: Get the model from the catalog
const catalog = manager.catalog;
const model = await catalog.getModel(alias);

// Step 3: Download model if needed, or skip
if (model.isCached) {
  console.log(`Model already downloaded: ${alias}`);
} else {
  console.log(
    `Downloading model: ${alias} (this may take several minutes)...`
  );
  await model.download();
  console.log(`Download complete: ${alias}`);
}

// Step 4: Load the model into memory
console.log(`Loading model: ${alias}...`);
await model.load();
console.log(`Model loaded: ${model.id}`);

// Configure the OpenAI client to use the local Foundry service
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local", // API key is not required for local usage
});

// Generate a streaming chat completion
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "What is the golden ratio?" }],
  stream: true,
});

// Print the streaming response
for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log(); // newline at end
