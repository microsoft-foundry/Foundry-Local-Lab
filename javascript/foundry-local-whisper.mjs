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

// Step 1: Bootstrap — starts the service, downloads, and loads the model
console.log(`Initializing Foundry Local with model: ${modelAlias}...`);
const manager = new FoundryLocalManager();
const modelInfo = await manager.init(modelAlias);
console.log(`Model ready: ${modelInfo.id}`);
console.log(`Endpoint: ${manager.endpoint}`);

// Step 2: Create the OpenAI client pointing to the local service
const client = new OpenAI({
  baseURL: manager.endpoint,
  apiKey: manager.apiKey,
});

// Step 3: Transcribe each audio file
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
