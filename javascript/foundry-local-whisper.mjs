/**
 * Whisper Voice Transcription with Foundry Local
 * Transcribes WAV audio files using the OpenAI Whisper model running locally.
 * Usage: node foundry-local-whisper.mjs [path-to-wav-file]
 */

import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const modelAlias = "whisper-medium";

// Default: transcribe all Zava sample WAV files
const samplesDir = path.join(__dirname, "..", "samples", "audio");

let audioFiles;
if (process.argv[2]) {
  // Specific file provided as argument
  const filePath = process.argv[2];
  if (!fs.existsSync(filePath)) {
    console.error(`Audio file not found: ${filePath}`);
    console.error("Usage: node foundry-local-whisper.mjs [path-to-wav-file]");
    process.exit(1);
  }
  audioFiles = [filePath];
} else {
  // Find all Zava WAV files
  if (!fs.existsSync(samplesDir)) {
    console.error(`Samples directory not found: ${samplesDir}`);
    console.error(
      "Run 'python samples/audio/generate_samples.py' first to generate them."
    );
    process.exit(1);
  }
  audioFiles = fs
    .readdirSync(samplesDir)
    .filter((f) => f.startsWith("zava-") && f.endsWith(".wav"))
    .sort()
    .map((f) => path.join(samplesDir, f));

  if (audioFiles.length === 0) {
    console.error(`No WAV files found in ${samplesDir}`);
    console.error(
      "Run 'python samples/audio/generate_samples.py' first to generate them."
    );
    process.exit(1);
  }
}

/**
 * Renders a CLI progress bar for model download.
 * @param {number} progress - Download progress percentage (0-100).
 */
function renderProgressBar(progress) {
  const barWidth = 30;
  const filled = Math.round((progress / 100) * barWidth);
  const empty = barWidth - filled;
  const bar = "\u2588".repeat(filled) + "\u2591".repeat(empty);
  process.stdout.write(`\r[Download] [${bar}] ${progress.toFixed(1)}%`);
  if (progress >= 100) {
    process.stdout.write("\n");
  }
}

// Step 1: Start the Foundry Local service
console.log("Starting Foundry Local service...");
const manager = new FoundryLocalManager();
await manager.startService();

// Step 2: Check if the model is already downloaded
const cachedModels = await manager.listCachedModels();
const catalogInfo = await manager.getModelInfo(modelAlias);
const isAlreadyCached = cachedModels.some((m) => m.id === catalogInfo?.id);

if (isAlreadyCached) {
  console.log(`Model already downloaded: ${modelAlias}`);
} else {
  console.log(
    `Downloading model: ${modelAlias} (this may take several minutes)...`
  );
  await manager.downloadModel(modelAlias, undefined, false, (progress) => {
    renderProgressBar(progress);
  });
  console.log(`Download complete: ${modelAlias}`);
}

// Step 3: Load the model into memory
console.log(`Loading model: ${modelAlias}...`);
const modelInfo = await manager.loadModel(modelAlias);
console.log(`Model loaded: ${modelInfo.id}`);

// Step 4: Create the OpenAI client pointing to the local service
const client = new OpenAI({
  baseURL: manager.endpoint,
  apiKey: manager.apiKey,
});

// Step 5: Transcribe each audio file
for (const audioPath of audioFiles) {
  const filename = path.basename(audioPath);
  console.log(`\n${"=".repeat(60)}`);
  console.log(`File: ${filename}`);
  console.log("=".repeat(60));

  const result = await client.audio.transcriptions.create({
    model: modelInfo.id,
    file: fs.createReadStream(audioPath),
  });

  console.log(result.text);
}

console.log(`\nDone — transcribed ${audioFiles.length} file(s).`);
