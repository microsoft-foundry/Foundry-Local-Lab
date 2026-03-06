/**
 * Zava Creative Writer — Foundry Local (JavaScript)
 *
 * Shared configuration: starts the service, ensures the model is cached,
 * loads it, and exports the OpenAI client + model ID for all agents.
 */

import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const MODEL_ALIAS = "phi-3.5-mini";

const manager = new FoundryLocalManager();

// Step 1: Start the Foundry Local service
console.log("Starting Foundry Local service...");
await manager.startService();

// Step 2: Check if the model is already downloaded
const cachedModels = await manager.listCachedModels();
const catalogInfo = await manager.getModelInfo(MODEL_ALIAS);
const isAlreadyCached = cachedModels.some((m) => m.id === catalogInfo?.id);

if (isAlreadyCached) {
  console.log(`Model already downloaded: ${MODEL_ALIAS}`);
} else {
  console.log(
    `Downloading model: ${MODEL_ALIAS} (this may take several minutes)...`
  );
  await manager.downloadModel(MODEL_ALIAS, undefined, false, (progress) => {
    const barWidth = 30;
    const filled = Math.round((progress / 100) * barWidth);
    const empty = barWidth - filled;
    const bar = "\u2588".repeat(filled) + "\u2591".repeat(empty);
    process.stdout.write(`\r[Download] [${bar}] ${progress.toFixed(1)}%`);
    if (progress >= 100) process.stdout.write("\n");
  });
  console.log(`Download complete: ${MODEL_ALIAS}`);
}

// Step 3: Load the model into memory
console.log(`Loading model: ${MODEL_ALIAS}...`);
const modelInfo = await manager.loadModel(MODEL_ALIAS);
const modelId = modelInfo.id;
console.log(`Model ready: ${modelId}`);

// Shared OpenAI client pointing at the local endpoint
const client = new OpenAI({
  baseURL: manager.endpoint,
  apiKey: manager.apiKey,
});

export { client, modelId, MODEL_ALIAS };
