![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagi 9: Voice Transcription gamit ang Whisper at Foundry Local

> **Layunin:** Gamitin ang OpenAI Whisper model na tumatakbo nang lokal sa pamamagitan ng Foundry Local upang i-transcribe ang audio files - ganap na nasa device, walang kinakailangang cloud.

## Pangkalahatang-ideya

Ang Foundry Local ay hindi lamang para sa text generation; sinusuportahan din nito ang mga **speech-to-text** na modelo. Sa lab na ito gagamitin mo ang **OpenAI Whisper Medium** model upang i-transcribe ang mga audio files nang ganap na nasa iyong makina. Ito ay ideal para sa mga sitwasyon tulad ng pag-transcribe ng mga tawag sa customer service ng Zava, mga recording ng review ng produkto, o mga session ng pagpaplano ng workshop kung saan ang audio data ay hindi kailanman dapat lumabas sa iyong device.

---

## Mga Layunin sa Pagkatuto

Sa pagtatapos ng lab na ito, magagawa mo ang mga sumusunod:

- Maunawaan ang Whisper speech-to-text model at ang mga kakayahan nito
- I-download at patakbuhin ang Whisper model gamit ang Foundry Local
- I-transcribe ang mga audio files gamit ang Foundry Local SDK sa Python, JavaScript, at C#
- Gumawa ng simpleng transcription service na tumatakbo nang ganap sa device
- Maunawaan ang mga pagkakaiba sa pagitan ng chat/text models at audio models sa Foundry Local

---

## Mga Kinakailangan

| Kinakailangan | Detalye |
|-------------|---------|
| **Foundry Local CLI** | Bersyon **0.8.101 o pataas** (Ang Whisper models ay available mula sa v0.8.101 pataas) |
| **OS** | Windows 10/11 (x64 o ARM64) |
| **Language runtime** | **Python 3.9+** at/o **Node.js 18+** at/o **.NET 9 SDK** ([I-download ang .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Nakompleto** | [Bahagi 1: Pagsisimula](part1-getting-started.md), [Bahagi 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md), at [Bahagi 3: SDKs at APIs](part3-sdk-and-apis.md) |

> **Tandaan:** Ang mga Whisper model ay kailangang i-download sa pamamagitan ng **SDK** (hindi sa CLI). Hindi sinusuportahan ng CLI ang audio transcription endpoint. Tingnan ang iyong bersyon gamit ang:
> ```bash
> foundry --version
> ```

---

## Konsepto: Paano Gumagana ang Whisper sa Foundry Local

Ang OpenAI Whisper model ay isang general-purpose speech recognition model na sinanay gamit ang malawak na dataset ng iba't ibang audio. Kapag tumatakbo sa Foundry Local:

- Ang modelo ay tumatakbo **ganap sa iyong CPU** - walang kinakailangang GPU
- Ang audio ay hindi lumalabas sa iyong device - **kumpletong privacy**
- Ang Foundry Local SDK ang humahawak sa pag-download at pamamahala ng cache ng modelo
- Ang **JavaScript at C#** ay may built-in na `AudioClient` sa SDK na sumasaklaw sa buong transcription pipeline — hindi na kailangan ng manual ONNX setup
- Ang **Python** ay gumagamit ng SDK para sa management ng modelo at ONNX Runtime para sa direktang inferensya laban sa encoder/decoder na ONNX models

### Paano Gumagana ang Pipeline (JavaScript at C#) — SDK AudioClient

1. Dinadownload at kino-cache ng **Foundry Local SDK** ang Whisper model
2. Gumagawa ng `AudioClient` sa pamamagitan ng `model.createAudioClient()` (JS) o `model.GetAudioClientAsync()` (C#)
3. Ang `audioClient.transcribe(path)` (JS) o `audioClient.TranscribeAudioAsync(path)` (C#) ang humahawak ng buong pipeline sa loob — audio preprocessing, encoder, decoder, at token decoding
4. Nag-eexpose ang `AudioClient` ng `settings.language` property (itinakda sa `"en"` para sa English) upang makatulong sa tumpak na transcription

### Paano Gumagana ang Pipeline (Python) — ONNX Runtime

1. Dinadownload at kino-cache ng **Foundry Local SDK** ang Whisper ONNX model files
2. **Audio preprocessing** nagko-convert ng WAV audio sa mel spectrogram (80 mel bins x 3000 frames)
3. **Encoder** ang nagpoproseso ng mel spectrogram at gumagawa ng hidden states pati na ang cross-attention key/value tensors
4. **Decoder** ay autoregressive, lumilikha ng isang token kada oras hanggang makagawa ng end-of-text token
5. **Tokenizer** nagde-decode ng output token IDs pabalik sa nababasang teksto

### Mga Variant ng Whisper Model

| Alias | Model ID | Device | Sukat | Paglalarawan |
|-------|----------|--------|-------|--------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-accelerated (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU-optimised (inirerekomenda para sa karamihan ng devices) |

> **Tandaan:** Hindi tulad ng chat models na awtomatikong nakalista, ang Whisper models ay nakategorya sa ilalim ng task na `automatic-speech-recognition`. Gamitin ang `foundry model info whisper-medium` para makita ang mga detalye.

---

## Mga Gawain sa Lab

### Gawain 0 - Kumuha ng Mga Sample na Audio Files

Ang lab na ito ay may kasamang pre-built na mga WAV files batay sa mga Zava DIY product scenarios. Gumawa ng mga ito gamit ang kasama na script:

```bash
# Mula sa ugat ng repo - lumikha at i-activate muna ang .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Lumilikha ito ng anim na WAV files sa `samples/audio/`:

| File | Scenario |
|------|----------|
| `zava-customer-inquiry.wav` | Customer na nagtatanong tungkol sa **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | Customer na nagrereview ng **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | Suporta na tawag tungkol sa **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | DIYer na nagpaplano ng deck gamit ang **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Walkthrough ng workshop gamit ang **lahat ng limang Zava products** |
| `zava-full-project-walkthrough.wav` | Pinalawig na walkthrough ng garage renovation gamit ang **lahat ng Zava products** (~4 min, para sa long-audio testing) |

> **Tip:** Maaari mo ring gamitin ang sarili mong WAV/MP3/M4A files, o mag-record gamit ang Windows Voice Recorder.

---

### Gawain 1 - I-download ang Whisper Model Gamit ang SDK

Dahil sa incompatibilities ng CLI sa Whisper models sa mas bagong bersyon ng Foundry Local, gamitin ang **SDK** upang i-download at i-load ang modelo. Piliin ang iyong wika:

<details>
<summary><b>🐍 Python</b></summary>

**I-install ang SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Simulan ang serbisyo
manager = FoundryLocalManager()
manager.start_service()

# Suriin ang impormasyon ng katalogo
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Suriin kung naka-cache na
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# I-load ang modelo sa memorya
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

I-save bilang `download_whisper.py` at patakbuhin:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**I-install ang SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Lumikha ng manager at simulan ang serbisyo
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Kumuha ng modelo mula sa katalogo
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

// I-load ang modelo sa memorya
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

I-save bilang `download-whisper.mjs` at patakbuhin:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**I-install ang SDK:**
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

> **Bakit SDK sa halip na CLI?** Hindi sinusuportahan ng Foundry Local CLI ang pag-download o pag-serve ng Whisper models nang diretso. Nagbibigay ang SDK ng maaasahang paraan upang i-download at pamahalaan ang mga audio models programmatically. Kasama sa JavaScript at C# SDKs ang built-in na `AudioClient` na nag-aasikaso ng buong transcription pipeline. Ang Python ay gumagamit ng ONNX Runtime para sa direktang inferensya laban sa naka-cache na mga model files.

---

### Gawain 2 - Unawain ang Whisper SDK

Ang Whisper transcription ay gumagamit ng iba't ibang pamamaraan depende sa wika. Ang **JavaScript at C#** ay may built-in na `AudioClient` sa Foundry Local SDK na humahawak ng buong pipeline (audio preprocessing, encoder, decoder, token decoding) sa isang tawag ng method. Ang **Python** ay gumagamit ng Foundry Local SDK para sa management ng modelo at ONNX Runtime para sa direktang inferensya laban sa encoder/decoder ONNX models.

| Komponent | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK packages** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Pamamahala ng modelo** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **Pagkuha ng features** | `WhisperFeatureExtractor` + `librosa` | Hinahandle ng SDK `AudioClient` | Hinahandle ng SDK `AudioClient` |
| **Inferensya** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Token decoding** | `WhisperTokenizer` | Hinahandle ng SDK `AudioClient` | Hinahandle ng SDK `AudioClient` |
| **Pagtatakda ng wika** | Itinakda gamit ang `forced_ids` sa decoder tokens | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | Path ng WAV file | Path ng WAV file | Path ng WAV file |
| **Output** | Decoded na string ng teksto | `result.text` | `result.Text` |

> **Mahalaga:** Laging itakda ang wika sa `AudioClient` (hal. `"en"` para sa English). Kung wala ang nakatakdang wika, maaaring maglabas ng magulong output ang modelo habang sinusubukang awtomatikong tuklasin ang wika.

> **Mga Pattern ng SDK:** Gumagamit ang Python ng `FoundryLocalManager(alias)` para i-bootstrap, pagkatapos ay `get_cache_location()` upang hanapin ang mga ONNX model files. Ang JavaScript at C# ay gumagamit ng built-in na `AudioClient` ng SDK — na nakuha sa pamamagitan ng `model.createAudioClient()` (JS) o `model.GetAudioClientAsync()` (C#) — na humahawak ng buong transcription pipeline. Tingnan ang [Bahagi 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md) para sa buong detalye.

---

### Gawain 3 - Gumawa ng Simpleng Transcription App

Pumili ng iyong wika at gumawa ng minimal na aplikasyon na magta-transcribe ng audio file.

> **Sinusuportahang audio format:** WAV, MP3, M4A. Para sa pinakamahusay na resulta, gumamit ng WAV files na may 16kHz sample rate.

<details>
<summary><h3>Python Track</h3></summary>

#### Setup

```bash
cd python
python -m venv venv

# I-activate ang virtual na kapaligiran:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Transcription Code

Gumawa ng file na `foundry-local-whisper.py`:

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

# Hakbang 1: Bootstrap - nagsisimula ng serbisyo, nagda-download, at naglo-load ng modelo
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Bumuo ng landas sa mga naka-cache na ONNX model files
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Hakbang 2: I-load ang mga ONNX session at feature extractor
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

# Hakbang 3: Kunin ang mga mel spectrogram na tampok
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Hakbang 4: Patakbuhin ang encoder
enc_out = encoder.run(None, {"audio_features": input_features})
# Ang unang output ay mga hidden states; ang natitira ay mga cross-attention KV pair
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Hakbang 5: Autoregressive decoding
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, isalin, walang mga timestamp
input_ids = np.array([initial_tokens], dtype=np.int32)

# Walang laman na self-attention KV cache
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

    if next_token == 50257:  # katapusan ng teksto
        break
    generated.append(next_token)

    # I-update ang self-attention KV cache
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Patakbuhin ito

```bash
# I-transcribe ang isang senaryo ng produkto ng Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# O subukan ang iba pa:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Mahahalagang Punto sa Python

| Metodo | Layunin |
|--------|---------|
| `FoundryLocalManager(alias)` | Bootstrap: simulan ang serbisyo, i-download, at i-load ang modelo |
| `manager.get_cache_location()` | Kunin ang path ng cached ONNX model files |
| `WhisperFeatureExtractor.from_pretrained()` | I-load ang mel spectrogram feature extractor |
| `ort.InferenceSession()` | Gumawa ng ONNX Runtime sessions para sa encoder at decoder |
| `tokenizer.decode()` | I-convert ang output token IDs pabalik sa teksto |

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

#### Setup

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Transcription Code

Gumawa ng file na `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Hakbang 1: Bootstrap - gumawa ng manager, simulan ang serbisyo, at i-load ang modelo
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

// Hakbang 2: Gumawa ng audio client at isalin ang salita
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Linisin
await model.unload();
```

> **Tandaan:** Ang Foundry Local SDK ay nagbibigay ng built-in na `AudioClient` sa pamamagitan ng `model.createAudioClient()` na humahawak ng buong ONNX inference pipeline sa loob — hindi na kailangan mag-import ng `onnxruntime-node`. Palaging itakda ang `audioClient.settings.language = "en"` para masigurong tumpak ang transcription sa English.

#### Patakbuhin ito

```bash
# Isalin ang isang senaryo ng produkto ng Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# O subukan ang iba:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Mahahalagang Punto sa JavaScript

| Metodo | Layunin |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Gumawa ng manager singleton |
| `await catalog.getModel(alias)` | Kumuha ng modelo mula sa catalogue |
| `model.download()` / `model.load()` | I-download at i-load ang Whisper model |
| `model.createAudioClient()` | Gumawa ng audio client para sa transcription |
| `audioClient.settings.language = "en"` | Itakda ang wika ng transcription (kailangan para sa tamang output) |
| `audioClient.transcribe(path)` | Mag-transcribe ng audio file, nagbabalik ng `{ text, duration }` |

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

> **Tandaan:** Ginagamit ng C# track ang `Microsoft.AI.Foundry.Local` package na nagbibigay ng built-in na `AudioClient` sa pamamagitan ng `model.GetAudioClientAsync()`. Hinahandle nito ang buong transcription pipeline sa proseso — hindi na kailangan ang hiwalay na ONNX Runtime setup.

#### Transcription Code

Palitan ang nilalaman ng `Program.cs`:

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

#### Patakbuhin ito

```bash
# Isalin ang senaryo ng isang produkto ng Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# O subukan ang iba pa:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Mahahalagang Punto sa C#

| Metodo | Layunin |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | I-initialise ang Foundry Local gamit ang config |
| `catalog.GetModelAsync(alias)` | Kumuha ng modelo mula sa catalog |
| `model.DownloadAsync()` | I-download ang Whisper model |
| `model.GetAudioClientAsync()` | Kumuha ng AudioClient (hindi ChatClient!) |
| `audioClient.Settings.Language = "en"` | Itakda ang wika ng transcription (kailangan para sa tamang output) |
| `audioClient.TranscribeAudioAsync(path)` | Mag-transcribe ng audio file |
| `result.Text` | Ang na-transcribe na teksto |
> **C# vs Python/JS:** Nagbibigay ang C# SDK ng built-in na `AudioClient` para sa in-process na transkripsiyon gamit ang `model.GetAudioClientAsync()`, katulad ng JavaScript SDK. Direktang ginagamit ng Python ang ONNX Runtime para sa inference laban sa naka-cache na encoder/decoder models.

</details>

---

### Exercise 4 - Batch Transcribe All Zava Samples

Ngayon na mayroon kang gumaganang transcription app, transcribe lahat ng limang Zava sample files at ihambing ang mga resulta.

<details>
<summary><h3>Python Track</h3></summary>

Suportado na ng buong sample na `python/foundry-local-whisper.py` ang batch transcription. Kapag pinatakbo nang walang argumento, tinatranscribe nito lahat ng `zava-*.wav` files sa `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Gumagamit ang sample ng `FoundryLocalManager(alias)` para sa bootstrap, pagkatapos ay pinapatakbo ang encoder at decoder ONNX sessions para sa bawat file.

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

Suportado na ng buong sample na `javascript/foundry-local-whisper.mjs` ang batch transcription. Kapag pinatakbo nang walang argumento, tinatranscribe nito lahat ng `zava-*.wav` files sa `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Gumagamit ang sample ng `FoundryLocalManager.create()` at `catalog.getModel(alias)` para i-initialize ang SDK, pagkatapos ay ginagamit ang `AudioClient` (na may `settings.language = "en"`) para itranscribe ang bawat file.

</details>

<details>
<summary><h3>C# Track</h3></summary>

Suportado na ng buong sample na `csharp/WhisperTranscription.cs` ang batch transcription. Kapag pinatakbo nang walang specific na file argument, tinatranscribe nito lahat ng `zava-*.wav` files sa `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Gumagamit ang sample ng `FoundryLocalManager.CreateAsync()` at ng `AudioClient` ng SDK (na may `Settings.Language = "en"`) para sa in-process transcription.

</details>

**Ano ang hahanapin:** Ihambing ang transcription output sa orihinal na teksto sa `samples/audio/generate_samples.py`. Gaano ka-accurate na nahahawakan ng Whisper ang mga pangalan ng produkto tulad ng "Zava ProGrip" at mga teknikal na termino tulad ng "brushless motor" o "composite decking"?

---

### Exercise 5 - Understand the Key Code Patterns

Pag-aralan kung paano nagkakaiba ang Whisper transcription mula sa chat completions sa lahat ng tatlong wika:

<details>
<summary><b>Python - Key Differences from Chat</b></summary>

```python
# Kumpletong chat (Mga Bahagi 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Transkripsyon ng audio (Bahaging Ito):
# Direktang gumagamit ng ONNX Runtime sa halip na ang OpenAI client
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregressive decoder loop ...
print(tokenizer.decode(generated_tokens))
```

**Pangunahing insight:** Ang mga chat model ay gumagamit ng OpenAI-compatible na API sa pamamagitan ng `manager.endpoint`. Ginagamit ng Whisper ang SDK upang hanapin ang naka-cache na ONNX model files, pagkatapos ay direktang nagpapatakbo ng inference gamit ang ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Key Differences from Chat</b></summary>

```javascript
// Kumpletuhan ng chat (Mga Bahagi 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Transkripsiyon ng audio (Ang Bahaging Ito):
// Ginagamit ang SDK na nakapaloob na AudioClient
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Palaging itakda ang wika para sa pinakamahusay na mga resulta
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Pangunahing insight:** Ang mga chat model ay gumagamit ng OpenAI-compatible na API sa pamamagitan ng `manager.urls[0] + "/v1"`. Ang Whisper transcription ay gumagamit ng SDK’s `AudioClient`, na nakuha mula sa `model.createAudioClient()`. Itakda ang `settings.language` upang maiwasan ang magulong output mula sa auto-detection.

</details>

<details>
<summary><b>C# - Key Differences from Chat</b></summary>

Gumagamit ang C# approach ng built-in na `AudioClient` ng SDK para sa in-process transcription:

**Pag-iinitialize ng Model:**

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

**Pangunahing insight:** Ginagamit ng C# ang `FoundryLocalManager.CreateAsync()` at direktang kumukuha ng `AudioClient` — hindi kailangan ang ONNX Runtime setup. Itakda ang `Settings.Language` upang maiwasan ang magulong output mula sa auto-detection.

</details>

> **Buod:** Ginagamit ng Python ang Foundry Local SDK para sa pamamahala ng modelo at ONNX Runtime para sa direktang inference laban sa encoder/decoder models. Ginagamit naman ng JavaScript at C# ang built-in na `AudioClient` ng SDK para sa streamline na transcription — gumawa ng client, itakda ang wika, at tawagin ang `transcribe()` / `TranscribeAudioAsync()`. Laging itakda ang language property sa AudioClient para sa tumpak na resulta.

---

### Exercise 6 - Experiment

Subukan ang mga pagbabagong ito upang palalimin ang iyong pag-unawa:

1. **Subukan ang iba't ibang audio files** - magrekord ng sarili mong pagsasalita gamit ang Windows Voice Recorder, i-save bilang WAV, at itranscribe ito

2. **Ihambing ang mga variant ng modelo** - kung mayroon kang NVIDIA GPU, subukan ang CUDA variant:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Ihambing ang bilis ng transcription kumpara sa CPU variant.

3. **Magdagdag ng output formatting** - maaaring isama sa JSON response ang:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Gumawa ng REST API** - balutin ang transcription code sa isang web server:

   | Wika | Framework | Halimbawa |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` gamit ang `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` gamit ang `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` gamit ang `IFormFile` |

5. **Multi-turn gamit ang transcription** - pagsamahin ang Whisper sa isang chat agent mula sa Part 4: muna itranscribe ang audio, tapos ipasa ang teksto sa agent para sa pagsusuri o pagbuod.

---

## SDK Audio API Reference

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — gumagawa ng isang `AudioClient` instance
> - `audioClient.settings.language` — itakda ang wika ng transcription (hal. `"en"`)
> - `audioClient.settings.temperature` — kontrolin ang randomness (opsyonal)
> - `audioClient.transcribe(filePath)` — itranscribe ang isang file, nagbabalik ng `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — nag-stream ng transcription chunks sa pamamagitan ng callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — gumagawa ng isang `OpenAIAudioClient` instance
> - `audioClient.Settings.Language` — itakda ang wika ng transcription (hal. `"en"`)
> - `audioClient.Settings.Temperature` — kontrolin ang randomness (opsyonal)
> - `await audioClient.TranscribeAudioAsync(filePath)` — itranscribe ang isang file, nagbabalik ng object na may `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — nagbabalik ng `IAsyncEnumerable` ng transcription chunks

> **Tip:** Laging itakda ang language property bago mag-transcribe. Kung hindi, susubukan ng Whisper model ang auto-detection na maaaring magresulta sa magulong output (isang single replacement character imbes na teksto).

---

## Comparison: Chat Models vs. Whisper

| Aspeto | Chat Models (Parts 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Uri ng gawain** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Input** | Mga text message (JSON) | Mga audio file (WAV/MP3/M4A) | Mga audio file (WAV/MP3/M4A) |
| **Output** | Nabuong teksto (streamed) | Na-transcribe na teksto (kumpleto) | Na-transcribe na teksto (kumpleto) |
| **SDK package** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API method** | `client.chat.completions.create()` | Direktang ONNX Runtime | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Pagtatakda ng wika** | N/A | Decoder prompt tokens | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Oo | Hindi | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Benepisyo sa privacy** | Code/data nananatili lokal | Audio data nananatili lokal | Audio data nananatili lokal |

---

## Key Takeaways

| Konsepto | Ang Iyong Natutunan |
|---------|---------------------|
| **Whisper on-device** | Speech-to-text ay tumatakbo nang lokal, perpekto para sa pag-transcribe ng mga tawag ng customer ng Zava at mga review ng produkto on-device |
| **SDK AudioClient** | Nagbibigay ang JavaScript at C# SDKs ng built-in na `AudioClient` na humahawak ng buong transcription pipeline sa isang tawag |
| **Pagtatakda ng wika** | Laging itakda ang audioClient language (hal. `"en"`) — kung wala ito, maaaring mag-auto-detect at magresulta sa magulong output |
| **Python** | Gumagamit ng `foundry-local-sdk` para sa pamamahala ng modelo + `onnxruntime` + `transformers` + `librosa` para sa direktang ONNX inference |
| **JavaScript** | Gumagamit ng `foundry-local-sdk` na may `model.createAudioClient()` — itakda ang `settings.language`, pagkatapos tawagin ang `transcribe()` |
| **C#** | Gumagamit ng `Microsoft.AI.Foundry.Local` na may `model.GetAudioClientAsync()` — itakda ang `Settings.Language`, pagkatapos tawagin ang `TranscribeAudioAsync()` |
| **Streaming support** | Nag-aalok din ang JS at C# SDKs ng `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` para sa output na piraso-piraso |
| **CPU-optimised** | Ang CPU variant (3.05 GB) ay gumagana sa kahit anong Windows device na walang GPU |
| **Privacy-first** | Perpekto para mapanatili ang mga pakikipag-ugnayan ng customer ng Zava at mga proprietary na data ng produkto on-device |

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

Magpatuloy sa [Part 10: Using Custom or Hugging Face Models](part10-custom-models.md) upang i-compile ang iyong sariling mga modelo mula sa Hugging Face at patakbuhin ang mga ito gamit ang Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Pagtatangi**:  
Ang dokumentong ito ay isinalin gamit ang serbisyong AI na pagsasalin na [Co-op Translator](https://github.com/Azure/co-op-translator). Bagama't nagsusumikap kami para sa katumpakan, pakitandaan na ang mga awtomatikong pagsasalin ay maaaring maglaman ng mga pagkakamali o kamalian. Ang orihinal na dokumento sa orihinal na wika nito ang dapat ituring na pinagbabatayan na sanggunian. Para sa mahahalagang impormasyon, inirerekomenda ang propesyonal na pagsasalin ng tao. Hindi kami mananagot sa anumang hindi pagkakaunawaan o maling interpretasyon na nagmula sa paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->