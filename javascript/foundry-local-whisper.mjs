/**
 * Whisper Voice Transcription with Foundry Local
 * Transcribes WAV audio files using the OpenAI Whisper model running locally.
 *
 * Uses the Foundry Local SDK's built-in AudioClient for transcription,
 * which handles all ONNX inference and audio preprocessing internally.
 *
 * Requires: foundry-local-sdk
 * Usage: node foundry-local-whisper.mjs [path-to-wav-file]
 */

import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const modelAlias = "whisper-medium";

// ---- Audio file discovery ------------------------------------------------

const samplesDir = path.join(__dirname, "..", "samples", "audio");
let audioFiles;

if (process.argv[2]) {
  const filePath = process.argv[2];
  if (!fs.existsSync(filePath)) {
    console.error(`Audio file not found: ${filePath}`);
    process.exit(1);
  }
  audioFiles = [filePath];
} else {
  if (!fs.existsSync(samplesDir)) {
    console.error(`Samples directory not found: ${samplesDir}`);
    process.exit(1);
  }
  audioFiles = fs
    .readdirSync(samplesDir)
    .filter((f) => f.startsWith("zava-") && f.endsWith(".wav"))
    .sort()
    .map((f) => path.join(samplesDir, f));
  if (audioFiles.length === 0) {
    console.error(`No WAV files found in ${samplesDir}`);
    process.exit(1);
  }
}

// ---- Step 1: Start Foundry Local and prepare the model -------------------

console.log(`Initialising Foundry Local with model: ${modelAlias}...`);
FoundryLocalManager.create({ appName: "FoundryLocalWorkshop" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);

if (!model.isCached) {
  console.log(`Downloading model: ${modelAlias} (this may take several minutes)...`);
  await model.download();
  console.log(`Download complete: ${modelAlias}`);
}

await model.load();
console.log(`Model ready: ${model.id}`);

// ---- Step 2: Create an AudioClient for transcription ---------------------

const audioClient = model.createAudioClient();
console.log("Audio client ready.\n");

// ---- Step 3: Transcribe each audio file ----------------------------------

for (const audioPath of audioFiles) {
  const filename = path.basename(audioPath);
  console.log("=".repeat(60));
  console.log(`File: ${filename}`);
  console.log("=".repeat(60));

  const t0 = Date.now();
  const result = await audioClient.transcribe(audioPath);
  const elapsed = ((Date.now() - t0) / 1000).toFixed(1);

  console.log(result);
  console.log(`(${elapsed}s)\n`);
}

console.log(`Done — transcribed ${audioFiles.length} file(s).`);
