![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Deel 9: Spraaktranscriptie met Whisper en Foundry Local

> **Doel:** Gebruik het OpenAI Whisper-model dat lokaal via Foundry Local draait om audiobestanden te transcriberen - volledig op het apparaat, geen cloud nodig.

## Overzicht

Foundry Local is niet alleen voor tekstgeneratie; het ondersteunt ook **spraak-naar-tekst** modellen. In deze lab ga je het **OpenAI Whisper Medium** model gebruiken om audiobestanden volledig op je machine te transcriberen. Dit is ideaal voor scenario's zoals het transcriberen van klantenservicegesprekken van Zava, productbeoordelingopnames of workshopplanningssessies waarbij audiogegevens nooit je apparaat mogen verlaten.


---

## Leerdoelen

Aan het einde van dit lab kun je:

- Het Whisper spraak-naar-tekstsysteem en de mogelijkheden ervan begrijpen
- Het Whisper-model downloaden en uitvoeren met Foundry Local
- Audiobestanden transcriberen met de Foundry Local SDK in Python, JavaScript en C#
- Een eenvoudige transcriptieservice bouwen die volledig op het apparaat draait
- De verschillen begrijpen tussen chat-/tekstmodellen en audiomodellen in Foundry Local

---

## Vereisten

| Vereiste | Details |
|-------------|---------|
| **Foundry Local CLI** | Versie **0.8.101 of hoger** (Whisper-modellen zijn beschikbaar vanaf v0.8.101) |
| **OS** | Windows 10/11 (x64 of ARM64) |
| **Taalruntime** | **Python 3.9+** en/of **Node.js 18+** en/of **.NET 9 SDK** ([Download .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Voltooid** | [Deel 1: Aan de slag](part1-getting-started.md), [Deel 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md), en [Deel 3: SDKs en APIs](part3-sdk-and-apis.md) |

> **Opmerking:** Whisper-modellen moeten via de **SDK** worden gedownload (niet via de CLI). De CLI ondersteunt de audio-transcriptie endpoint niet. Controleer je versie met:
> ```bash
> foundry --version
> ```

---

## Concept: Hoe Whisper werkt met Foundry Local

Het OpenAI Whisper-model is een algemeen spraakherkenningsmodel getraind op een grote dataset met diverse audio. Wanneer het via Foundry Local draait:

- Draait het model **volledig op je CPU** - geen GPU nodig
- Audio verlaat nooit je apparaat - **volledige privacy**
- De Foundry Local SDK regelt het downloaden en beheren van de modelcache
- **JavaScript en C#** bieden een ingebouwde `AudioClient` in de SDK die de volledige transcriptiepijplijn afhandelt — geen handmatige ONNX-setup nodig
- **Python** gebruikt de SDK voor modelbeheer en ONNX Runtime voor directe inferentie op de encoder/decoder ONNX-modellen

### Hoe de pijplijn werkt (JavaScript en C#) — SDK AudioClient

1. **Foundry Local SDK** downloadt en cachet het Whisper-model
2. `model.createAudioClient()` (JS) of `model.GetAudioClientAsync()` (C#) maakt een `AudioClient` aan
3. `audioClient.transcribe(path)` (JS) of `audioClient.TranscribeAudioAsync(path)` (C#) regelt intern de volledige pijplijn — audio preprocessing, encoder, decoder en token decoding
4. De `AudioClient` biedt een eigenschap `settings.language` (instellen op `"en"` voor Engels) om nauwkeurige transcriptie te sturen

### Hoe de pijplijn werkt (Python) — ONNX Runtime

1. **Foundry Local SDK** downloadt en cachet de Whisper ONNX-modelbestanden
2. **Audio preprocessing** zet WAV-audio om in een mel-spectrogram (80 mel-bins x 3000 frames)
3. **Encoder** verwerkt het mel-spectrogram en produceert verborgen toestanden plus key/value tensors voor cross-attentie
4. **Decoder** draait autoregressief en genereert telkens één token totdat een end-of-text token geproduceerd wordt
5. **Tokenizer** decodeert de output token-ID's terug naar leesbare tekst

### Varianten van het Whisper-model

| Alias | Model ID | Apparaat | Grootte | Beschrijving |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1,53 GB | GPU-versneld (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3,05 GB | CPU-geoptimaliseerd (aanbevolen voor de meeste apparaten) |

> **Opmerking:** In tegenstelling tot chatmodellen die standaard worden vermeld, zijn Whisper-modellen gecategoriseerd onder de taak `automatic-speech-recognition`. Gebruik `foundry model info whisper-medium` om details te zien.

---

## Lab Oefeningen

### Oefening 0 - Verkrijg voorbeeld audiobestanden

Dit lab bevat vooraf gebouwde WAV-bestanden gebaseerd op Zava doe-het-zelf productscenario's. Genereer ze met het bijgevoegde script:

```bash
# Vanaf de root van de repo - maak eerst een .venv aan en activeer deze
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Dit maakt zes WAV-bestanden in `samples/audio/`:

| Bestand | Scenario |
|------|----------|
| `zava-customer-inquiry.wav` | Klant die vraagt over de **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | Klant beoordeelt de **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | Supportgesprek over de **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | Doe-het-zelver plant een terras met **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Rondleiding door een werkplaats met **alle vijf Zava-producten** |
| `zava-full-project-walkthrough.wav` | Uitgebreide garage-renovatie walkthrough met **alle Zava-producten** (~4 min, voor lange-audio tests) |

> **Tip:** Je kunt ook je eigen WAV/MP3/M4A-bestanden gebruiken of jezelf opnemen met Windows Voice Recorder.

---

### Oefening 1 - Download het Whisper-model met de SDK

Vanwege incompatibiliteiten van de CLI met Whisper-modellen in nieuwere Foundry Local-versies, gebruik de **SDK** om het model te downloaden en te laden. Kies je taal:

<details>
<summary><b>🐍 Python</b></summary>

**Installeer de SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Start de dienst
manager = FoundryLocalManager()
manager.start_service()

# Controleer catalogusinformatie
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Controleer of al in cache staat
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Laad het model in het geheugen
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Sla op als `download_whisper.py` en voer uit:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Installeer de SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Maak manager aan en start de dienst
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Haal model uit catalogus
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

// Laad het model in het geheugen
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Sla op als `download-whisper.mjs` en voer uit:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Installeer de SDK:**
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

> **Waarom SDK in plaats van CLI?** De Foundry Local CLI ondersteunt niet het rechtstreeks downloaden of draaien van Whisper-modellen. De SDK biedt een betrouwbare manier om audiomodellen programmatisch te downloaden en te beheren. De JavaScript en C# SDK’s bevatten een ingebouwde `AudioClient` die de volledige transcriptiepijplijn afhandelt. Python gebruikt ONNX Runtime voor directe inferentie op de gecachte modelbestanden.

---

### Oefening 2 - Begrijp de Whisper SDK

Whisper-transcriptie gebruikt verschillende benaderingen afhankelijk van de taal. **JavaScript en C#** bieden een ingebouwde `AudioClient` in de Foundry Local SDK die de volledige pijplijn (audio preprocessing, encoder, decoder, token decoding) afhandelt in één methode-aanroep. **Python** gebruikt de Foundry Local SDK voor modelbeheer en ONNX Runtime voor directe inferentie op de encoder/decoder ONNX-modellen.

| Component | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK pakketten** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Modelbeheer** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalogus |
| **Feature extractie** | `WhisperFeatureExtractor` + `librosa` | Afgehandeld door SDK `AudioClient` | Afgehandeld door SDK `AudioClient` |
| **Inferentie** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Token decoding** | `WhisperTokenizer` | Afgehandeld door SDK `AudioClient` | Afgehandeld door SDK `AudioClient` |
| **Taal instellen** | Via `forced_ids` in decoder tokens | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | WAV-bestandspad | WAV-bestandspad | WAV-bestandspad |
| **Output** | Gedecodeerde tekststring | `result.text` | `result.Text` |

> **Belangrijk:** Stel altijd de taal in op de `AudioClient` (bijv. `"en"` voor Engels). Zonder een expliciete taalinstelling kan het model onleesbare output geven omdat het probeert de taal automatisch te detecteren.

> **SDK-patronen:** Python gebruikt `FoundryLocalManager(alias)` voor het opstarten, vervolgens `get_cache_location()` om de ONNX modelbestanden te vinden. JavaScript en C# gebruiken de ingebouwde `AudioClient` van de SDK — verkregen via `model.createAudioClient()` (JS) of `model.GetAudioClientAsync()` (C#) — die de volledige transcriptiepijplijn afhandelt. Zie [Deel 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md) voor volledige details.

---

### Oefening 3 - Bouw een eenvoudige transcriptie-app

Kies je taalroute en bouw een minimale applicatie die een audiobestand transcribeert.

> **Ondersteunde audioformaten:** WAV, MP3, M4A. Gebruik bij voorkeur WAV-bestanden met een samplefrequentie van 16kHz voor het beste resultaat.

<details>
<summary><h3>Python-route</h3></summary>

#### Setup

```bash
cd python
python -m venv venv

# Activeer de virtuele omgeving:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Transcriptiecode

Maak een bestand `foundry-local-whisper.py`:

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

# Stap 1: Bootstrap - start service, downloadt en laadt het model
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Bouw pad naar de gecachte ONNX-modelfiles
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Stap 2: Laad ONNX-sessies en feature extractor
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

# Stap 3: Extraheer mel spectrogram kenmerken
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Stap 4: Voer encoder uit
enc_out = encoder.run(None, {"audio_features": input_features})
# Eerste output is verborgen toestanden; de overige zijn cross-attention KV-paren
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Stap 5: Autoregressieve decodering
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcriptie, geen tijdstempels
input_ids = np.array([initial_tokens], dtype=np.int32)

# Lege self-attention KV-cache
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

    if next_token == 50257:  # einde van tekst
        break
    generated.append(next_token)

    # Werk self-attention KV-cache bij
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Uitvoeren

```bash
# Transcribeer een Zava productscenario
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Of probeer andere:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Belangrijke punten Python

| Methode | Doel |
|--------|---------|
| `FoundryLocalManager(alias)` | Opstarten: start service, download en laad het model |
| `manager.get_cache_location()` | Krijg het pad naar gecachte ONNX-modelbestanden |
| `WhisperFeatureExtractor.from_pretrained()` | Laad de mel spectrogram feature extractor |
| `ort.InferenceSession()` | Maak ONNX Runtime-sessies voor encoder en decoder |
| `tokenizer.decode()` | Zet token-ID's om naar tekst |

</details>

<details>
<summary><h3>JavaScript-route</h3></summary>

#### Setup

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Transcriptiecode

Maak een bestand `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Stap 1: Bootstrap - maak manager aan, start dienst en laad het model
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

// Stap 2: Maak een audioclient en transcribeer
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Opruimen
await model.unload();
```

> **Opmerking:** De Foundry Local SDK biedt een ingebouwde `AudioClient` via `model.createAudioClient()` die de volledige ONNX-inferentie pijplijn intern afhandelt — import van `onnxruntime-node` is niet nodig. Stel altijd `audioClient.settings.language = "en"` in om nauwkeurige Engelse transcriptie te garanderen.

#### Uitvoeren

```bash
# Transcribeer een Zava productscenario
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Of probeer anderen:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Belangrijke punten JavaScript

| Methode | Doel |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Maak de manager singleton aan |
| `await catalog.getModel(alias)` | Verkrijg een model uit de catalogus |
| `model.download()` / `model.load()` | Download en laad het Whisper-model |
| `model.createAudioClient()` | Maak een audio client voor transcriptie |
| `audioClient.settings.language = "en"` | Stel de transcriptietaal in (vereist voor nauwkeurige output) |
| `audioClient.transcribe(path)` | Transcribeer een audiobestand, retourneert `{ text, duration }` |

</details>

<details>
<summary><h3>C#-route</h3></summary>

#### Setup

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Opmerking:** De C#-route gebruikt de `Microsoft.AI.Foundry.Local` package die een ingebouwde `AudioClient` biedt via `model.GetAudioClientAsync()`. Dit handelt de volledige transcriptiepijplijn in-proces af — geen aparte ONNX Runtime configuratie nodig.

#### Transcriptiecode

Vervang de inhoud van `Program.cs`:

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

#### Uitvoeren

```bash
# Transcribeer een Zava-productscenario
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Of probeer anderen:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Belangrijke punten C#

| Methode | Doel |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Initialiseer Foundry Local met configuratie |
| `catalog.GetModelAsync(alias)` | Haal model uit catalogus |
| `model.DownloadAsync()` | Download het Whisper-model |
| `model.GetAudioClientAsync()` | Verkrijg de AudioClient (niet ChatClient!) |
| `audioClient.Settings.Language = "en"` | Stel de transcriptietaal in (vereist voor nauwkeurige output) |
| `audioClient.TranscribeAudioAsync(path)` | Transcribeer een audiobestand |
| `result.Text` | De getranscribeerde tekst |
> **C# vs Python/JS:** De C# SDK biedt een ingebouwde `AudioClient` voor transcriberen binnen het proces via `model.GetAudioClientAsync()`, vergelijkbaar met de JavaScript SDK. Python gebruikt ONNX Runtime direct voor inferentie tegen de gecachte encoder/decoder modellen.

</details>

---

### Oefening 4 - Batch Transcribeer Alle Zava Voorbeelden

Nu je een werkende transcriptie-app hebt, transcribeer alle vijf Zava voorbeeldbestanden en vergelijk de resultaten.

<details>
<summary><h3>Python Track</h3></summary>

Het volledige voorbeeld `python/foundry-local-whisper.py` ondersteunt al batch-transcriptie. Wanneer uitgevoerd zonder argumenten, transcribeert het alle `zava-*.wav` bestanden in `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Het voorbeeld gebruikt `FoundryLocalManager(alias)` om op te starten, en voert vervolgens de encoder- en decoder ONNX-sessies voor elk bestand uit.

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

Het volledige voorbeeld `javascript/foundry-local-whisper.mjs` ondersteunt al batch-transcriptie. Wanneer uitgevoerd zonder argumenten, transcribeert het alle `zava-*.wav` bestanden in `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Het voorbeeld gebruikt `FoundryLocalManager.create()` en `catalog.getModel(alias)` om de SDK te initialiseren, en gebruikt dan de `AudioClient` (met `settings.language = "en"`) om elk bestand te transcriberen.

</details>

<details>
<summary><h3>C# Track</h3></summary>

Het volledige voorbeeld `csharp/WhisperTranscription.cs` ondersteunt al batch-transcriptie. Wanneer uitgevoerd zonder een specifiek bestandsargument, transcribeert het alle `zava-*.wav` bestanden in `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Het voorbeeld gebruikt `FoundryLocalManager.CreateAsync()` en de SDK’s `AudioClient` (met `Settings.Language = "en"`) voor transcriberen binnen het proces.

</details>

**Waarop te letten:** Vergelijk de transcriptie-uitvoer met de originele tekst in `samples/audio/generate_samples.py`. Hoe nauwkeurig herkent Whisper productnamen zoals "Zava ProGrip" en technische termen zoals "brushless motor" of "composite decking"?

---

### Oefening 5 - Begrijp de Belangrijkste Codepatronen

Bestudeer hoe Whisper-transcriptie verschilt van chat-afrondingen in alle drie talen:

<details>
<summary><b>Python - Belangrijkste Verschillen met Chat</b></summary>

```python
# Chat voltooiing (Delen 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Audio transcriptie (Dit Deel):
# Gebruikt direct ONNX Runtime in plaats van de OpenAI-client
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregressieve decoder lus ...
print(tokenizer.decode(generated_tokens))
```

**Belangrijk inzicht:** Chatmodellen gebruiken de OpenAI-compatibele API via `manager.endpoint`. Whisper gebruikt de SDK om de gecachte ONNX-modellen te vinden en voert vervolgens inferentie direct uit met ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Belangrijkste Verschillen met Chat</b></summary>

```javascript
// Chat voltooiing (Delen 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Audio transcriptie (Dit Deel):
// Gebruikt de ingebouwde AudioClient van de SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Stel altijd de taal in voor de beste resultaten
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Belangrijk inzicht:** Chatmodellen gebruiken de OpenAI-compatibele API via `manager.urls[0] + "/v1"`. Whisper-transcriptie gebruikt de SDK’s `AudioClient`, verkregen via `model.createAudioClient()`. Stel `settings.language` in om corrupte uitvoer van autodetectie te vermijden.

</details>

<details>
<summary><b>C# - Belangrijkste Verschillen met Chat</b></summary>

De C# aanpak gebruikt de ingebouwde `AudioClient` van de SDK voor transcriberen binnen het proces:

**Modelinitialisatie:**

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

**Transcriptie:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Belangrijk inzicht:** C# gebruikt `FoundryLocalManager.CreateAsync()` en krijgt direct een `AudioClient` — geen ONNX Runtime setup nodig. Stel `Settings.Language` in om corrupte uitvoer van autodetectie te vermijden.

</details>

> **Samenvatting:** Python gebruikt de Foundry Local SDK voor modelbeheer en ONNX Runtime voor directe inferentie op de encoder/decoder modellen. JavaScript en C# gebruiken beiden de ingebouwde `AudioClient` van de SDK voor gestroomlijnde transcriptie — maak de client aan, stel de taal in, en roep `transcribe()` / `TranscribeAudioAsync()` aan. Stel altijd de taal in op de AudioClient voor accurate resultaten.

---

### Oefening 6 - Experimenteer

Probeer deze aanpassingen om je begrip te verdiepen:

1. **Probeer andere audiobestanden** - neem jezelf op met Windows Voice Recorder, sla op als WAV en transcribeer het

2. **Vergelijk modelvarianten** - als je een NVIDIA GPU hebt, probeer dan de CUDA-variant:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Vergelijk de transcriptiesnelheid met de CPU-variant.

3. **Voeg uitvoerformattering toe** - de JSON-respons kan bevatten:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Bouw een REST API** - pak je transcriptiecode in een webserver:

   | Taal | Framework | Voorbeeld |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` met `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` met `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` met `IFormFile` |

5. **Multi-turn met transcriptie** - combineer Whisper met een chat-agent uit Deel 4: transcribeer eerst audio, en geef dan de tekst door aan een agent voor analyse of samenvatting.

---

## SDK Audio API Referentie

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — maakt een `AudioClient` instantie aan
> - `audioClient.settings.language` — stel de transcriptietaal in (bijv. `"en"`)
> - `audioClient.settings.temperature` — beheer willekeurigheid (optioneel)
> - `audioClient.transcribe(filePath)` — transcribeer een bestand, retourneert `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — streaming transcriptie van stukjes via callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — maakt een `OpenAIAudioClient` instantie aan
> - `audioClient.Settings.Language` — stel de transcriptietaal in (bijv. `"en"`)
> - `audioClient.Settings.Temperature` — beheer willekeurigheid (optioneel)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transcribeer een bestand, retourneert object met `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — retourneert `IAsyncEnumerable` van transcriptie-chunks

> **Tip:** Stel altijd de taal in voordat je gaat transcriberen. Zonder deze instelling probeert het Whisper-model autodetectie, wat kan leiden tot corrupte uitvoer (een enkel vervangingskarakter in plaats van tekst).

---

## Vergelijking: Chatmodellen vs. Whisper

| Aspect | Chatmodellen (Delen 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Taaktype** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Input** | Tekstberichten (JSON) | Audio bestanden (WAV/MP3/M4A) | Audio bestanden (WAV/MP3/M4A) |
| **Output** | Gegeneerde tekst (gestreamd) | Getranscribeerde tekst (volledig) | Getranscribeerde tekst (volledig) |
| **SDK pakket** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API methode** | `client.chat.completions.create()` | ONNX Runtime direct | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Taalinstelling** | N.v.t. | Decoder prompt tokens | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Ja | Nee | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Privacy voordeel** | Code/data blijft lokaal | Audio data blijft lokaal | Audio data blijft lokaal |

---

## Belangrijkste Inzichten

| Concept | Wat Je Leerde |
|---------|-----------------|
| **Whisper on-device** | Spraak-naar-tekst draait volledig lokaal, ideaal voor het transcriberen van Zava klantgesprekken en productreviews on-device |
| **SDK AudioClient** | JavaScript en C# SDK’s bieden een ingebouwde `AudioClient` die de volledige transcriptie-pijplijn in één oproep afhandelt |
| **Taalinstelling** | Stel altijd de AudioClient taal in (bijv. `"en"`) — zonder dit kan autodetectie corrupte uitvoer geven |
| **Python** | Gebruikt `foundry-local-sdk` voor modelbeheer + `onnxruntime` + `transformers` + `librosa` voor directe ONNX inferentie |
| **JavaScript** | Gebruikt `foundry-local-sdk` met `model.createAudioClient()` — stel `settings.language` in, roep dan `transcribe()` aan |
| **C#** | Gebruikt `Microsoft.AI.Foundry.Local` met `model.GetAudioClientAsync()` — stel `Settings.Language` in, roep dan `TranscribeAudioAsync()` aan |
| **Streaming ondersteuning** | JS en C# SDK’s bieden ook `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` voor uitvoer per stuk |
| **CPU-geoptimaliseerd** | De CPU-variant (3,05 GB) werkt op ieder Windows apparaat zonder GPU |
| **Privacy-first** | Perfect om Zava klantinteracties en vertrouwelijke productdata on-device te houden |

---

## Bronnen

| Bron | Link |
|----------|------|
| Foundry Local docs | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referentie | [Microsoft Learn - SDK Referentie](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper model | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local website | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Volgende Stap

Ga verder naar [Deel 10: Gebruik van Custom of Hugging Face Modellen](part10-custom-models.md) om je eigen modellen van Hugging Face te compileren en via Foundry Local uit te voeren.