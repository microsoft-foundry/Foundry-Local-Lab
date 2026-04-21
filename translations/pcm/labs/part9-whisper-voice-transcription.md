![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 9: Voice Transcription wit Whisper an Foundry Local

> **Goal:** Use di OpenAI Whisper model wey dey run locally tru Foundry Local to transcribe audio files - complete on-device, no cloud wey you need.

## Overview

Foundry Local no be just for text generation; e still dey support **speech-to-text** models. For dis lab you go use di **OpenAI Whisper Medium** model to transcribe audio files complete on your machine. Dis dey good for tins like transcribing Zava customer service calls, product review recordings, or workshop planning sessions wey audio data no suppose leave your device.


---

## Learning Objectives

By di time you finish dis lab you go fit:

- Understand di Whisper speech-to-text model and wetin e fit do
- Download and run di Whisper model using Foundry Local
- Transcribe audio files using di Foundry Local SDK for Python, JavaScript, and C#
- Build simple transcription service wey run complete on-device
- Understand di difference between chat/text models an audio models for Foundry Local

---

## Prerequisites

| Requirement | Details |
|-------------|---------|
| **Foundry Local CLI** | Version **0.8.101 or above** (Whisper models dey available from v0.8.101 onwards) |
| **OS** | Windows 10/11 (x64 or ARM64) |
| **Language runtime** | **Python 3.9+** and/or **Node.js 18+** and/or **.NET 9 SDK** ([Download .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Completed** | [Part 1: Getting Started](part1-getting-started.md), [Part 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md), and [Part 3: SDKs and APIs](part3-sdk-and-apis.md) |

> **Note:** Whisper models dem suppose download tru di **SDK** (no be CLI). CLI no support di audio transcription endpoint. Check your version with:
> ```bash
> foundry --version
> ```

---

## Concept: How Whisper Dey Work wit Foundry Local

Di OpenAI Whisper model na general-purpose speech recognition model trained on plenty diverse audio data. When you dey run am tru Foundry Local:

- Di model dey run **complete on your CPU** - no GPU dey required
- Audio no ever go your device - **complete privacy**
- Di Foundry Local SDK na im dey handle model download an cache management
- **JavaScript and C#** get built-in `AudioClient` inside SDK wey dey handle di whole transcription pipeline — no need manual ONNX setup
- **Python** dey use SDK for model management and ONNX Runtime for direct inference against encoder/decoder ONNX models

### How di Pipeline Dey Work (JavaScript and C#) — SDK AudioClient

1. **Foundry Local SDK** go download an cache di Whisper model
2. `model.createAudioClient()` (JS) or `model.GetAudioClientAsync()` (C#) go create `AudioClient`
3. `audioClient.transcribe(path)` (JS) or `audioClient.TranscribeAudioAsync(path)` (C#) go handle di full pipeline inside — audio preprocessing, encoder, decoder, token decoding
4. Di `AudioClient` get `settings.language` property (set am to `"en"` for English) to make transcription dey accurate

### How di Pipeline Dey Work (Python) — ONNX Runtime

1. **Foundry Local SDK** dey download an cache di Whisper ONNX model files
2. **Audio preprocessing** go convert WAV audio to mel spectrogram (80 mel bins x 3000 frames)
3. **Encoder** go process di mel spectrogram an produce hidden states plus cross-attention key/value tensors
4. **Decoder** dey run autoregressively, e dey generate one token every time till e produce end-of-text token
5. **Tokeniser** go decode di output token IDs come back as text wey person fit read

### Whisper Model Variants

| Alias | Model ID | Device | Size | Description |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-accelerated (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU-optimised (recommended for most devices) |

> **Note:** Different from chat models wey dem dey list by default, Whisper models dey under `automatic-speech-recognition` task. Use `foundry model info whisper-medium` to check details.

---

## Lab Exercises

### Exercise 0 - Get Sample Audio Files

Dis lab get pre-built WAV files wey base on Zava DIY product scenarios. Make you create dem with dis script:

```bash
# From the repo root - make and start one .venv first
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Dis one go create six WAV files for `samples/audio/`:

| File | Scenario |
|------|----------|
| `zava-customer-inquiry.wav` | Customer wey dey ask about di **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | Customer wey dey review di **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | Support call about di **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | DIY person dey plan deck with **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Walkthrough of workshop wey dey use **all five Zava products** |
| `zava-full-project-walkthrough.wav` | Extended garage renovation walkthrough wey use **all Zava products** (~4 min, for long-audio testing) |

> **Tip:** You fit also use your own WAV/MP3/M4A files, or record yourself with Windows Voice Recorder.

---

### Exercise 1 - Download di Whisper Model Using SDK

Because CLI no too sabi play well wit Whisper models for new Foundry Local versions, use di **SDK** make you download an load di model. Pick di language wey you sabi:

<details>
<summary><b>🐍 Python</b></summary>

**Install di SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Strat di service
manager = FoundryLocalManager()
manager.start_service()

# Check di catalog info
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Check if e don already dey cache
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Load di model inside memory
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Save as `download_whisper.py` an run am:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Install di SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Make manager and start di service
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Comot model from catalogue
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

// Put di model enter memory
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Save am as `download-whisper.mjs` an run:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Install di SDK:**
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

> **Why SDK no be CLI?** Foundry Local CLI no fit download or serve Whisper models directly. SDK na di sure way to take download and manage audio models programmatically. Di JavaScript and C# SDKs get built-in `AudioClient` wey handle full transcription pipeline. Python dey use ONNX Runtime for direct inference against cached model files.

---

### Exercise 2 - Understand di Whisper SDK

Whisper transcription dey use diffren way depending on wetin language you dey use. **JavaScript and C#** get built-in `AudioClient` inside Foundry Local SDK wey handle full pipeline (audio preprocessing, encoder, decoder, token decoding) inside one method call. **Python** use Foundry Local SDK for model management and ONNX Runtime for direct inference against encoder/decoder ONNX models.

| Component | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK packages** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Model management** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **Feature extraction** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` dey handle am | SDK `AudioClient` dey handle am |
| **Inference** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Token decoding** | `WhisperTokenizer` | SDK `AudioClient` dey handle am | SDK `AudioClient` dey handle am |
| **Language setting** | Set via `forced_ids` for decoder tokens | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | WAV file path | WAV file path | WAV file path |
| **Output** | Decoded text string | `result.text` | `result.Text` |

> **Important:** Always set di language for `AudioClient` (example: `"en"` for English). If you no explicitly set language, di model fit give rubbish output as e go try autodetect di language.

> **SDK Patterns:** Python use `FoundryLocalManager(alias)` to bootstrap, then `get_cache_location()` to find ONNX model files. JavaScript and C# dey use SDK built-in `AudioClient` — through `model.createAudioClient()` (JS) or `model.GetAudioClientAsync()` (C#) — wey dey handle full transcription pipeline. See [Part 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md) for full gist.

---

### Exercise 3 - Build Simple Transcription App

Pick your language track an build small app wey fit transcribe audio file.

> **Supported audio formats:** WAV, MP3, M4A. For best result, use WAV files wey get 16kHz sample rate.

<details>
<summary><h3>Python Track</h3></summary>

#### Setup

```bash
cd python
python -m venv venv

# Make you activate di virtual environment:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Transcription Code

Create file `foundry-local-whisper.py`:

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

# Step 1: Bootstrap - e dey start service, e dey download, and load di model
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Build path go di cached ONNX model files
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
# First output na hidden states; di rest na cross-attention KV pairs
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Step 5: Autoregressive decoding
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcribe, no timestamps
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

#### Run am

```bash
# Write down how di Zava product go be
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Or try oda ones:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Key Python Points

| Method | Purpose |
|--------|---------|
| `FoundryLocalManager(alias)` | Bootstrap: start service, download and load di model |
| `manager.get_cache_location()` | Find di path wey ONNX model files dey cache |
| `WhisperFeatureExtractor.from_pretrained()` | Load mel spectrogram feature extractor |
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

Create file `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Step 1: Bootstrap - mak manager, stat di service, an load di model
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

// Step 2: Mak audio client an transcribe
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Clean up
await model.unload();
```

> **Note:** Foundry Local SDK get built-in `AudioClient` via `model.createAudioClient()` wey handle full ONNX inference pipeline inside — no need import `onnxruntime-node`. Make sure sey you set `audioClient.settings.language = "en"` to get accurate English transcription.

#### Run am

```bash
# Make you write down how Zava product be
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Or try oda ones:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Key JavaScript Points

| Method | Purpose |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Create the manager singleton |
| `await catalog.getModel(alias)` | Get model from catalogue |
| `model.download()` / `model.load()` | Download an load Whisper model |
| `model.createAudioClient()` | Create audio client for transcription |
| `audioClient.settings.language = "en"` | Set transcription language (needed to get correct output) |
| `audioClient.transcribe(path)` | Transcribe audio file, returns `{ text, duration }` |

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

> **Note:** C# track dey use `Microsoft.AI.Foundry.Local` package wey get built-in `AudioClient` via `model.GetAudioClientAsync()`. E handle full transcription pipeline inside one process — no extra ONNX Runtime setup needed.

#### Transcription Code

Replace di contents for `Program.cs`:

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

#### Run am

```bash
# Make you write down how Zava product go be
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Or try oda ones:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Key C# Points

| Method | Purpose |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Initialise Foundry Local wit configuration |
| `catalog.GetModelAsync(alias)` | Get model from catalog |
| `model.DownloadAsync()` | Download di Whisper model |
| `model.GetAudioClientAsync()` | Get di AudioClient (not ChatClient!) |
| `audioClient.Settings.Language = "en"` | Set transcription language (required for accurate output) |
| `audioClient.TranscribeAudioAsync(path)` | Transcribe audio file |
| `result.Text` | Di transcribed text |


> **C# vs Python/JS:** Di C# SDK get built-in `AudioClient` fo in-process transcription through `model.GetAudioClientAsync()`, wey dey similar to di JavaScript SDK. Python dey use ONNX Runtime direct for inference against di cached encoder/decoder models.

</details>

---

### Exercise 4 - Batch Transcribe All Zava Samples

Now wey you don get working transcription app, transcribe all five Zava sample files den compare di results.

<details>
<summary><h3>Python Track</h3></summary>

Di full sample `python/foundry-local-whisper.py` don already support batch transcription. When you run am without arguments, e go transcribe all `zava-*.wav` files wey dey `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Di sample dey use `FoundryLocalManager(alias)` to bootstrap, den e run di encoder and decoder ONNX sessions for each file.

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

Di full sample `javascript/foundry-local-whisper.mjs` don already support batch transcription. When you run am without arguments, e go transcribe all `zava-*.wav` files wey dey `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Di sample dey use `FoundryLocalManager.create()` and `catalog.getModel(alias)` to initialize di SDK, den e use di `AudioClient` (wit `settings.language = "en"`) to transcribe each file.

</details>

<details>
<summary><h3>C# Track</h3></summary>

Di full sample `csharp/WhisperTranscription.cs` don already support batch transcription. When you run am without specific file argument, e go transcribe all `zava-*.wav` files wey dey `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Di sample dey use `FoundryLocalManager.CreateAsync()` and di SDK’s `AudioClient` (wit `Settings.Language = "en"`) fo in-process transcription.

</details>

**Wetyn you suppose look for:** Compare di transcription output wit di original text wey dey for `samples/audio/generate_samples.py`. How correct Whisper dey capture product names like "Zava ProGrip" and technical terms like "brushless motor" or "composite decking"?

---

### Exercise 5 - Understand the Key Code Patterns

Study how Whisper transcription different from chat completions across all three languages:

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

# Audio transcription (Dis Part):
# Dey use ONNX Runtime straight straight no be OpenAI client
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregressive decoder loop ...
print(tokenizer.decode(generated_tokens))
```

**Key insight:** Chat models dey use di OpenAI-compatible API via `manager.endpoint`. Whisper dey use di SDK to find di cached ONNX model files, den e run inference direct wit ONNX Runtime.

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

// Audio transcription (Dis Paat):
// Dem dey use the SDK wey get AudioClient wey dey inside am
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Make you always set language to get better result
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Key insight:** Chat models dey use di OpenAI-compatible API via `manager.urls[0] + "/v1"`. Whisper transcription dey use di SDK’s `AudioClient`, wey you fit get from `model.createAudioClient()`. Make you set `settings.language` to avoid garbled output from auto-detection.

</details>

<details>
<summary><b>C# - Key Differences from Chat</b></summary>

Di C# approach dey use di SDK’s built-in `AudioClient` for in-process transcription:

**Model initialization:**

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

**Key insight:** C# dey use `FoundryLocalManager.CreateAsync()` and e get `AudioClient` direct — no ONNX Runtime setup dey needed. Make you set `Settings.Language` to avoid garbled output from auto-detection.

</details>

> **Summary:** Python dey use di Foundry Local SDK for model management and ONNX Runtime for direct inference against di encoder/decoder models. JavaScript and C# both dey use di SDK’s built-in `AudioClient` for easy transcription — create the client, set di language, then call `transcribe()` / `TranscribeAudioAsync()`. Always set di language property on di AudioClient to get correct results.

---

### Exercise 6 - Experiment

Try these modifications to understand better:

1. **Try different audio files** - record yourself talking using Windows Voice Recorder, save am as WAV, then transcribe am

2. **Compare model variants** - if you get NVIDIA GPU, try di CUDA variant:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Compare di transcription speed to di CPU variant.

3. **Add output formatting** - di JSON response fit get:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Build a REST API** - put your transcription code inside web server:

   | Language | Framework | Example |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` with `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` with `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` with `IFormFile` |

5. **Multi-turn with transcription** - combine Whisper wit chat agent from Part 4: transcribe audio first, den pass di text to agent for analysis or summarisation.

---

## SDK Audio API Reference

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — create `AudioClient` instance
> - `audioClient.settings.language` — set transcription language (ex: `"en"`)
> - `audioClient.settings.temperature` — control randomness (optional)
> - `audioClient.transcribe(filePath)` — transcribe file, returns `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — stream transcription chunks via callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — create `OpenAIAudioClient` instance
> - `audioClient.Settings.Language` — set transcription language (ex: `"en"`)
> - `audioClient.Settings.Temperature` — control randomness (optional)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transcribe file, returns object with `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — returns `IAsyncEnumerable` of transcription chunks

> **Tip:** Always set di language property before you start transcription. If you no do am, Whisper model go try auto-detect, wey fit produce garbled output (like one replacement character instead of text).

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
| **Privacy benefit** | Code/data dey local | Audio data dey local | Audio data dey local |

---

## Key Takeaways

| Concept | Wetin You Learn |
|---------|-----------------|
| **Whisper on-device** | Speech-to-text dey run fully locally, beta for transcribing Zava customer calls and product reviews on-device |
| **SDK AudioClient** | JavaScript and C# SDKs get built-in `AudioClient` wey handle di full transcription pipeline in one call |
| **Language setting** | Always set di AudioClient language (ex: `"en"`) — if you no do am, auto-detection fit produce garbled output |
| **Python** | Dey use `foundry-local-sdk` for model management + `onnxruntime` + `transformers` + `librosa` for direct ONNX inference |
| **JavaScript** | Dey use `foundry-local-sdk` with `model.createAudioClient()` — set `settings.language`, then call `transcribe()` |
| **C#** | Dey use `Microsoft.AI.Foundry.Local` with `model.GetAudioClientAsync()` — set `Settings.Language`, then call `TranscribeAudioAsync()` |
| **Streaming support** | JS and C# SDKs also get `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` for chunk-by-chunk output |
| **CPU-optimised** | CPU variant (3.05 GB) fit run for any Windows device without GPU |
| **Privacy-first** | Perfect to keep Zava customer interactions and product data on-device |

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

Continue to [Part 10: Using Custom or Hugging Face Models](part10-custom-models.md) to compile your own models from Hugging Face and run dem through Foundry Local.