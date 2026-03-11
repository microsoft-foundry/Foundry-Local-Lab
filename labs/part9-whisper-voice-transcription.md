![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 9: Voice Transcription with Whisper and Foundry Local

> **Goal:** Use the OpenAI Whisper model running locally through Foundry Local to transcribe audio files - completely on-device, no cloud required.

## Overview

Foundry Local is not just for text generation; it also supports **speech-to-text** models. In this lab you will use the **OpenAI Whisper Medium** model to transcribe audio files entirely on your machine. This is ideal for scenarios like transcribing Zava customer service calls, product review recordings, or workshop planning sessions where audio data must never leave your device.


---

## Learning Objectives

By the end of this lab you will be able to:

- Understand the Whisper speech-to-text model and its capabilities
- Download and run the Whisper model using Foundry Local
- Transcribe audio files using the Foundry Local SDK in Python, JavaScript, and C#
- Build a simple transcription service that runs entirely on-device
- Understand the differences between chat/text models and audio models in Foundry Local

---

## Prerequisites

| Requirement | Details |
|-------------|---------|
| **Foundry Local CLI** | Version **0.8.101 or above** (Whisper models are available from v0.8.101 onwards) |
| **OS** | Windows 10/11 (x64 or ARM64) |
| **Language runtime** | **Python 3.9+** and/or **Node.js 18+** and/or **.NET 9 SDK** ([Download .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Completed** | [Part 1: Getting Started](part1-getting-started.md), [Part 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md), and [Part 3: SDKs and APIs](part3-sdk-and-apis.md) |

> **Note:** Whisper models must be downloaded via the **SDK** (not the CLI). The CLI does not support the audio transcription endpoint. Check your version with:
> ```bash
> foundry --version
> ```

---

## Concept: How Whisper Works with Foundry Local

The OpenAI Whisper model is a general-purpose speech recognition model trained on a large dataset of diverse audio. When running through Foundry Local:

- The model runs **entirely on your CPU** - no GPU required
- Audio never leaves your device - **complete privacy**
- The Foundry Local SDK handles model download and cache management
- **JavaScript and C#** provide a built-in `AudioClient` in the SDK that handles the entire transcription pipeline — no manual ONNX setup required
- **Python** uses the SDK for model management and ONNX Runtime for direct inference against the encoder/decoder ONNX models

### How the Pipeline Works (JavaScript and C#) — SDK AudioClient

1. **Foundry Local SDK** downloads and caches the Whisper model
2. `model.createAudioClient()` (JS) or `model.GetAudioClientAsync()` (C#) creates an `AudioClient`
3. `audioClient.transcribe(path)` (JS) or `audioClient.TranscribeAudioAsync(path)` (C#) handles the full pipeline internally — audio preprocessing, encoder, decoder, and token decoding
4. The `AudioClient` exposes a `settings.language` property (set to `"en"` for English) to guide accurate transcription

### How the Pipeline Works (Python) — ONNX Runtime

1. **Foundry Local SDK** downloads and caches the Whisper ONNX model files
2. **Audio preprocessing** converts WAV audio into a mel spectrogram (80 mel bins x 3000 frames)
3. **Encoder** processes the mel spectrogram and produces hidden states plus cross-attention key/value tensors
4. **Decoder** runs autoregressively, generating one token at a time until it produces an end-of-text token
5. **Tokeniser** decodes the output token IDs back into readable text

### Whisper Model Variants

| Alias | Model ID | Device | Size | Description |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-accelerated (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU-optimised (recommended for most devices) |

> **Note:** Unlike chat models that list by default, Whisper models are categorised under the `automatic-speech-recognition` task. Use `foundry model info whisper-medium` to see details.

---

## Lab Exercises

### Exercise 0 - Get Sample Audio Files

This lab includes pre-built WAV files based on Zava DIY product scenarios. Generate them with the included script:

```bash
# From the repo root - create and activate a .venv first
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

This creates six WAV files in `samples/audio/`:

| File | Scenario |
|------|----------|
| `zava-customer-inquiry.wav` | Customer asking about the **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | Customer reviewing the **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | Support call about the **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | DIYer planning a deck with **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Walkthrough of a workshop using **all five Zava products** |
| `zava-full-project-walkthrough.wav` | Extended garage renovation walkthrough using **all Zava products** (~4 min, for long-audio testing) |

> **Tip:** You can also use your own WAV/MP3/M4A files, or record yourself with Windows Voice Recorder.

---

### Exercise 1 - Download the Whisper Model Using the SDK

Due to CLI incompatibilities with Whisper models in newer Foundry Local versions, use the **SDK** to download and load the model. Choose your language:

<details>
<summary><b>🐍 Python</b></summary>

**Install the SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Start the service
manager = FoundryLocalManager()
manager.start_service()

# Check catalog info
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Check if already cached
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Load the model into memory
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Save as `download_whisper.py` and run:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Install the SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Create manager and start the service
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Get model from catalogue
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.id}`);

if (model.isCached) {
  console.log("Whisper model already downloaded.");
} else {
  console.log("Downloading Whisper model (this may take several minutes)...");
  await model.download();
  console.log("Download complete.");
}

// Load the model into memory
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Save as `download-whisper.mjs` and run:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Install the SDK:**
```bash
dotnet add package Microsoft.AI.Foundry.Local
```

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "whisper-medium";

// Start the service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);
Console.WriteLine($"Model: {model.Id}");

// Check if already cached
var isCached = await model.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine("Whisper model already downloaded.");
}
else
{
    Console.WriteLine("Downloading Whisper model (this may take several minutes)...");
    await model.DownloadAsync(null, default);
    Console.WriteLine("Download complete.");
}

// Load the model into memory
await model.LoadAsync(default);
Console.WriteLine($"Whisper model loaded: {model.Id}");
```

</details>

> **Why SDK instead of CLI?** The Foundry Local CLI does not support downloading or serving Whisper models directly. The SDK provides a reliable way to download and manage audio models programmatically. The JavaScript and C# SDKs include a built-in `AudioClient` that handles the entire transcription pipeline. Python uses ONNX Runtime for direct inference against the cached model files.

---

### Exercise 2 - Understand the Whisper SDK

Whisper transcription uses different approaches depending on the language. **JavaScript and C#** provide a built-in `AudioClient` in the Foundry Local SDK that handles the full pipeline (audio preprocessing, encoder, decoder, token decoding) in a single method call. **Python** uses the Foundry Local SDK for model management and ONNX Runtime for direct inference against the encoder/decoder ONNX models.

| Component | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK packages** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Model management** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **Feature extraction** | `WhisperFeatureExtractor` + `librosa` | Handled by SDK `AudioClient` | Handled by SDK `AudioClient` |
| **Inference** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Token decoding** | `WhisperTokenizer` | Handled by SDK `AudioClient` | Handled by SDK `AudioClient` |
| **Language setting** | Set via `forced_ids` in decoder tokens | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | WAV file path | WAV file path | WAV file path |
| **Output** | Decoded text string | `result.text` | `result.Text` |

> **Important:** Always set the language on the `AudioClient` (e.g. `"en"` for English). Without an explicit language setting, the model may produce garbled output as it attempts to auto-detect the language.

> **SDK Patterns:** Python uses `FoundryLocalManager(alias)` to bootstrap, then `get_cache_location()` to find the ONNX model files. JavaScript and C# use the SDK’s built-in `AudioClient` — obtained via `model.createAudioClient()` (JS) or `model.GetAudioClientAsync()` (C#) — which handles the entire transcription pipeline. See [Part 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md) for full details.

---

### Exercise 3 - Build a Simple Transcription App

Choose your language track and build a minimal application that transcribes an audio file.

> **Supported audio formats:** WAV, MP3, M4A. For best results, use WAV files with 16kHz sample rate.

<details>
<summary><h3>Python Track</h3></summary>

#### Setup

```bash
cd python
python -m venv venv

# Activate the virtual environment:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Transcription Code

Create a file `foundry-local-whisper.py`:

```python
import sys
import os
import numpy as np
import onnxruntime as ort
import librosa
from transformers import WhisperFeatureExtractor, WhisperTokenizer
from foundry_local import FoundryLocalManager

model_alias = "whisper-medium"
audio_file = sys.argv[1] if len(sys.argv) > 1 else "sample.wav"

if not os.path.exists(audio_file):
    print(f"Audio file not found: {audio_file}")
    sys.exit(1)

# Step 1: Bootstrap - starts service, downloads, and loads the model
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Build path to the cached ONNX model files
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Step 2: Load ONNX sessions and feature extractor
encoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_encoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
decoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_decoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
fe = WhisperFeatureExtractor.from_pretrained(model_dir)
tokenizer = WhisperTokenizer.from_pretrained(model_dir)

# Step 3: Extract mel spectrogram features
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Step 4: Run encoder
enc_out = encoder.run(None, {"audio_features": input_features})
# First output is hidden states; remaining are cross-attention KV pairs
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Step 5: Autoregressive decoding
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcribe, notimestamps
input_ids = np.array([initial_tokens], dtype=np.int32)

# Empty self-attention KV cache
self_kv = {}
for i in range(24):
    self_kv[f"past_key_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)
    self_kv[f"past_value_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)

generated = []
for _ in range(448):
    feeds = {"input_ids": input_ids, **cross_kv, **self_kv}
    outputs = decoder.run(None, feeds)
    logits = outputs[0]
    next_token = int(np.argmax(logits[0, -1, :]))

    if next_token == 50257:  # end of text
        break
    generated.append(next_token)

    # Update self-attention KV cache
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Run it

```bash
# Transcribe a Zava product scenario
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Or try others:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Key Python Points

| Method | Purpose |
|--------|---------|
| `FoundryLocalManager(alias)` | Bootstrap: start service, download, and load the model |
| `manager.get_cache_location()` | Get the path to cached ONNX model files |
| `WhisperFeatureExtractor.from_pretrained()` | Load the mel spectrogram feature extractor |
| `ort.InferenceSession()` | Create ONNX Runtime sessions for encoder and decoder |
| `tokenizer.decode()` | Convert output token IDs back to text |

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

#### Setup

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Transcription Code

Create a file `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Step 1: Bootstrap - create manager, start service, and load the model
console.log(`Initialising Foundry Local with model: ${modelAlias}...`);
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);

if (!model.isCached) {
  console.log("Downloading Whisper model...");
  await model.download();
}
await model.load();

// Step 2: Create an audio client and transcribe
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Cleanup
await model.unload();
```

> **Note:** The Foundry Local SDK provides a built-in `AudioClient` via `model.createAudioClient()` that handles the entire ONNX inference pipeline internally — no `onnxruntime-node` import needed. Always set `audioClient.settings.language = "en"` to ensure accurate English transcription.

#### Run it

```bash
# Transcribe a Zava product scenario
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Or try others:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Key JavaScript Points

| Method | Purpose |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Create the manager singleton |
| `await catalog.getModel(alias)` | Get a model from the catalogue |
| `model.download()` / `model.load()` | Download and load the Whisper model |
| `model.createAudioClient()` | Create an audio client for transcription |
| `audioClient.settings.language = "en"` | Set the transcription language (required for accurate output) |
| `audioClient.transcribe(path)` | Transcribe an audio file, returns `{ text, duration }` |

</details>

<details>
<summary><h3>C# Track</h3></summary>

#### Setup

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Note:** The C# track uses the `Microsoft.AI.Foundry.Local` package which provides a built-in `AudioClient` via `model.GetAudioClientAsync()`. This handles the full transcription pipeline in-process — no separate ONNX Runtime setup needed.

#### Transcription Code

Replace the contents of `Program.cs`:

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// --- Configuration ---
var modelAlias = "whisper-medium";
var audioFile = args.Length > 0 ? args[0] : "sample.wav";

if (!File.Exists(audioFile))
{
    Console.WriteLine($"Audio file not found: {audioFile}");
    Console.WriteLine("Usage: dotnet run <path-to-audio-file>");
    return;
}

// --- Step 1: Initialize Foundry Local ---
Console.WriteLine("Initializing Foundry Local...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// --- Step 2: Load the Whisper model ---
Console.WriteLine($"Loading model: {modelAlias}...");
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading model...");
    await model.DownloadAsync(null, default);
}

// Load model into memory
Console.WriteLine("Loading model into memory...");
await model.LoadAsync(default);

// --- Step 3: Transcribe audio ---
Console.WriteLine($"Transcribing: {audioFile}");

var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en";

var result = await audioClient.TranscribeAudioAsync(audioFile);

Console.WriteLine("\n--- Transcription ---");
Console.WriteLine(result.Text);
Console.WriteLine("---------------------");
```

#### Run it

```bash
# Transcribe a Zava product scenario
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Or try others:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Key C# Points

| Method | Purpose |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Initialise Foundry Local with configuration |
| `catalog.GetModelAsync(alias)` | Get model from catalog |
| `model.DownloadAsync()` | Download the Whisper model |
| `model.GetAudioClientAsync()` | Get the AudioClient (not ChatClient!) |
| `audioClient.Settings.Language = "en"` | Set the transcription language (required for accurate output) |
| `audioClient.TranscribeAudioAsync(path)` | Transcribe an audio file |
| `result.Text` | The transcribed text |

> **C# vs Python/JS:** The C# SDK provides a built-in `AudioClient` for in-process transcription via `model.GetAudioClientAsync()`, similar to the JavaScript SDK. Python uses ONNX Runtime directly for inference against the cached encoder/decoder models.

</details>

---

### Exercise 4 - Batch Transcribe All Zava Samples

Now that you have a working transcription app, transcribe all five Zava sample files and compare the results.

<details>
<summary><h3>Python Track</h3></summary>

The full sample `python/foundry-local-whisper.py` already supports batch transcription. When run without arguments, it transcribes all `zava-*.wav` files in `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

The sample uses `FoundryLocalManager(alias)` to bootstrap, then runs the encoder and decoder ONNX sessions for each file.

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

The full sample `javascript/foundry-local-whisper.mjs` already supports batch transcription. When run without arguments, it transcribes all `zava-*.wav` files in `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

The sample uses `FoundryLocalManager.create()` and `catalog.getModel(alias)` to initialise the SDK, then uses the `AudioClient` (with `settings.language = "en"`) to transcribe each file.

</details>

<details>
<summary><h3>C# Track</h3></summary>

The full sample `csharp/WhisperTranscription.cs` already supports batch transcription. When run without a specific file argument, it transcribes all `zava-*.wav` files in `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

The sample uses `FoundryLocalManager.CreateAsync()` and the SDK’s `AudioClient` (with `Settings.Language = "en"`) for in-process transcription.

</details>

**What to look for:** Compare the transcription output against the original text in `samples/audio/generate_samples.py`. How accurately does Whisper capture product names like "Zava ProGrip" and technical terms like "brushless motor" or "composite decking"?

---

### Exercise 5 - Understand the Key Code Patterns

Study how Whisper transcription differs from chat completions across all three languages:

<details>
<summary><b>Python - Key Differences from Chat</b></summary>

```python
# Chat completion (Parts 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Audio transcription (This Part):
# Uses ONNX Runtime directly instead of the OpenAI client
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregressive decoder loop ...
print(tokenizer.decode(generated_tokens))
```

**Key insight:** Chat models use the OpenAI-compatible API via `manager.endpoint`. Whisper uses the SDK to locate the cached ONNX model files, then runs inference directly with ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Key Differences from Chat</b></summary>

```javascript
// Chat completion (Parts 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Audio transcription (This Part):
// Uses the SDK's built-in AudioClient
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Always set language for best results
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Key insight:** Chat models use the OpenAI-compatible API via `manager.urls[0] + "/v1"`. Whisper transcription uses the SDK’s `AudioClient`, obtained from `model.createAudioClient()`. Set `settings.language` to avoid garbled output from auto-detection.

</details>

<details>
<summary><b>C# - Key Differences from Chat</b></summary>

The C# approach uses the SDK’s built-in `AudioClient` for in-process transcription:

**Model initialisation:**

```csharp
// 1. Create the manager with configuration
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// 2. Get model from catalog, download, and load
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("whisper-medium", default);
await model.DownloadAsync(null, default);
await model.LoadAsync(default);
```

**Transcription:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Key insight:** C# uses `FoundryLocalManager.CreateAsync()` and gets an `AudioClient` directly — no ONNX Runtime setup needed. Set `Settings.Language` to avoid garbled output from auto-detection.

</details>

> **Summary:** Python uses the Foundry Local SDK for model management and ONNX Runtime for direct inference against the encoder/decoder models. JavaScript and C# both use the SDK’s built-in `AudioClient` for streamlined transcription — create the client, set the language, and call `transcribe()` / `TranscribeAudioAsync()`. Always set the language property on the AudioClient for accurate results.

---

### Exercise 6 - Experiment

Try these modifications to deepen your understanding:

1. **Try different audio files** - record yourself speaking using Windows Voice Recorder, save as WAV, and transcribe it

2. **Compare model variants** - if you have an NVIDIA GPU, try the CUDA variant:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Compare the transcription speed against the CPU variant.

3. **Add output formatting** - the JSON response can include:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Build a REST API** - wrap your transcription code in a web server:

   | Language | Framework | Example |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` with `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` with `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` with `IFormFile` |

5. **Multi-turn with transcription** - combine Whisper with a chat agent from Part 4: transcribe audio first, then pass the text to an agent for analysis or summarisation.

---

## SDK Audio API Reference

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — creates an `AudioClient` instance
> - `audioClient.settings.language` — set the transcription language (e.g. `"en"`)
> - `audioClient.settings.temperature` — control randomness (optional)
> - `audioClient.transcribe(filePath)` — transcribe a file, returns `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — stream transcription chunks via callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — creates an `OpenAIAudioClient` instance
> - `audioClient.Settings.Language` — set the transcription language (e.g. `"en"`)
> - `audioClient.Settings.Temperature` — control randomness (optional)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transcribe a file, returns object with `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — returns `IAsyncEnumerable` of transcription chunks

> **Tip:** Always set the language property before transcribing. Without it, the Whisper model attempts auto-detection, which can produce garbled output (a single replacement character instead of text).

---

## Comparison: Chat Models vs. Whisper

| Aspect | Chat Models (Parts 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Task type** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Input** | Text messages (JSON) | Audio files (WAV/MP3/M4A) | Audio files (WAV/MP3/M4A) |
| **Output** | Generated text (streamed) | Transcribed text (complete) | Transcribed text (complete) |
| **SDK package** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API method** | `client.chat.completions.create()` | ONNX Runtime direct | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Language setting** | N/A | Decoder prompt tokens | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Yes | No | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Privacy benefit** | Code/data stays local | Audio data stays local | Audio data stays local |

---

## Key Takeaways

| Concept | What You Learned |
|---------|-----------------|
| **Whisper on-device** | Speech-to-text runs entirely locally, ideal for transcribing Zava customer calls and product reviews on-device |
| **SDK AudioClient** | JavaScript and C# SDKs provide a built-in `AudioClient` that handles the full transcription pipeline in a single call |
| **Language setting** | Always set the AudioClient language (e.g. `"en"`) — without it, auto-detection may produce garbled output |
| **Python** | Uses `foundry-local-sdk` for model management + `onnxruntime` + `transformers` + `librosa` for direct ONNX inference |
| **JavaScript** | Uses `foundry-local-sdk` with `model.createAudioClient()` — set `settings.language`, then call `transcribe()` |
| **C#** | Uses `Microsoft.AI.Foundry.Local` with `model.GetAudioClientAsync()` — set `Settings.Language`, then call `TranscribeAudioAsync()` |
| **Streaming support** | JS and C# SDKs also offer `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` for chunk-by-chunk output |
| **CPU-optimised** | The CPU variant (3.05 GB) works on any Windows device without a GPU |
| **Privacy-first** | Perfect for keeping Zava customer interactions and proprietary product data on-device |

---

## Resources

| Resource | Link |
|----------|------|
| Foundry Local docs | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Reference | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper model | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local website | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Next Step

Continue to [Part 10: Using Custom or Hugging Face Models](part10-custom-models.md) to compile your own models from Hugging Face and run them through Foundry Local.
