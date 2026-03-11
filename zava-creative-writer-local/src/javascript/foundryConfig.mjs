/**
 * Zava Creative Writer — Foundry Local (JavaScript)
 *
 * Shared configuration: starts the service, ensures the model is cached,
 * loads it, and exports the OpenAI client + model ID for all agents.
 */

import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const MODEL_ALIAS = "phi-3.5-mini";

// Step 1: Create a FoundryLocalManager and start the service
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "ZavaCreativeWriter" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Step 2: Get the model from the catalog
const catalog = manager.catalog;
const model = await catalog.getModel(MODEL_ALIAS);

if (model.isCached) {
  console.log(`Model already downloaded: ${MODEL_ALIAS}`);
} else {
  console.log(
    `Downloading model: ${MODEL_ALIAS} (this may take several minutes)...`
  );
  await model.download();
  console.log(`Download complete: ${MODEL_ALIAS}`);
}

// Step 3: Load the model into memory
console.log(`Loading model: ${MODEL_ALIAS}...`);
await model.load();
const modelId = model.id;
console.log(`Model ready: ${modelId}`);

// Shared OpenAI client pointing at the local endpoint
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

export { client, modelId, MODEL_ALIAS };
