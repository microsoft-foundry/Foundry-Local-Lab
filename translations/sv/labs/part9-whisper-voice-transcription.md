![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 9: Rösttranskribering med Whisper och Foundry Local

> **Mål:** Använd OpenAI Whisper-modellen som körs lokalt via Foundry Local för att transkribera ljudfiler – helt på enheten, ingen molnanslutning krävs.

## Översikt

Foundry Local är inte bara för textgenerering; det stöder även **tal-till-text**-modeller. I denna labb kommer du att använda **OpenAI Whisper Medium**-modellen för att transkribera ljudfiler helt på din maskin. Detta är idealiskt för scenarier som att transkribera Zavas kundtjänstsamtal, produktrecensionsinspelningar eller planeringsmöten för workshops där ljuddata aldrig får lämna din enhet.

---

## Läromål

I slutet av denna labb ska du kunna:

- Förstå Whisper tal-till-text-modellen och dess kapabiliteter
- Ladda ner och köra Whisper-modellen med Foundry Local
- Transkribera ljudfiler med Foundry Local SDK i Python, JavaScript och C#
- Bygga en enkel transkriberingstjänst som körs helt på enheten
- Förstå skillnaderna mellan chatt-/textmodeller och ljudmodeller i Foundry Local

---

## Förkunskaper

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Version **0.8.101 eller senare** (Whisper-modeller finns från v0.8.101 och framåt) |
| **Operativsystem** | Windows 10/11 (x64 eller ARM64) |
| **Språkruntimer** | **Python 3.9+** och/eller **Node.js 18+** och/eller **.NET 9 SDK** ([Ladda ner .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Avklarade** | [Del 1: Komma igång](part1-getting-started.md), [Del 2: Foundry Local SDK Genomgång](part2-foundry-local-sdk.md), och [Del 3: SDKs och API:er](part3-sdk-and-apis.md) |

> **Notera:** Whisper-modeller måste laddas ner via **SDK** (inte CLI). CLI stödjer inte ljudtranskriberingsendpunkten. Kontrollera din version med:
> ```bash
> foundry --version
> ```

---

## Koncept: Hur Whisper fungerar med Foundry Local

OpenAI Whisper-modellen är en allmän taligenkänningsmodell tränad på en stor dataset med varierande ljud. När den körs via Foundry Local:

- Modellen körs **helt på din CPU** – ingen GPU krävs
- Ljud lämnar aldrig din enhet – **fullständig sekretess**
- Foundry Local SDK hanterar modellnedladdning och cachehantering
- **JavaScript och C#** erbjuder en inbyggd `AudioClient` i SDK som hanterar hela transkriberingsprocessen – ingen manuell ONNX-konfiguration krävs
- **Python** använder SDK för modellhantering och ONNX Runtime för direkt inferens mot encoder-/decoder-ONNX-modellerna

### Hur pipelinen fungerar (JavaScript och C#) — SDK AudioClient

1. **Foundry Local SDK** laddar ner och cachar Whisper-modellen
2. `model.createAudioClient()` (JS) eller `model.GetAudioClientAsync()` (C#) skapar en `AudioClient`
3. `audioClient.transcribe(path)` (JS) eller `audioClient.TranscribeAudioAsync(path)` (C#) hanterar hela processen internt – ljudförbehandling, encoder, decoder och avkodning av tokens
4. `AudioClient` exponerar en `settings.language`-egenskap (sätt till `"en"` för engelska) för exakt transkribering

### Hur pipelinen fungerar (Python) — ONNX Runtime

1. **Foundry Local SDK** laddar ner och cachar Whisper ONNX-modellfilerna
2. **Ljudförbehandling** konverterar WAV-ljud till en mel-spektrogram (80 mel-bin x 3000 ramar)
3. **Encoder** bearbetar mel-spektrogrammet och producerar dolda tillstånd och cross-attention nyckel-/värdetensorer
4. **Decoder** kör autoregressivt, genererar en token i taget tills ett end-of-text-token produceras
5. **Tokeniserare** avkodar output token-IDs tillbaka till läsbar text

### Whisper Modellvarianter

| Alias | Modell-ID | Enhet | Storlek | Beskrivning |
|-------|-----------|-------|---------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-acceleration (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU-optimerad (rekommenderas för de flesta enheter) |

> **Notera:** Till skillnad från chattmodeller som listas som standard, kategoriseras Whisper-modeller under `automatic-speech-recognition`-uppgiften. Använd `foundry model info whisper-medium` för detaljer.

---

## Labövningar

### Övning 0 - Hämta Exempelljudfiler

Denna labb inkluderar förbyggda WAV-filer baserade på Zavas DIY-produkt-scenarier. Generera dem med det inkluderade skriptet:

```bash
# Från rotmappen i repot - skapa och aktivera först en .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Detta skapar sex WAV-filer i `samples/audio/`:

| Fil | Scenario |
|------|----------|
| `zava-customer-inquiry.wav` | Kund som frågar om **Zava ProGrip Sladdlös Borrmaskin** |
| `zava-product-review.wav` | Kund som recenserar **Zava UltraSmooth Inomhusfärg** |
| `zava-support-call.wav` | Support-samtal om **Zava TitanLock Verktygslåda** |
| `zava-project-planning.wav` | DIY-entusiast planerar en altan med **Zava EcoBoard Komposittrall** |
| `zava-workshop-setup.wav` | Genomgång av en verkstad med **alla fem Zava-produkterna** |
| `zava-full-project-walkthrough.wav` | Utökad genomgång av garage-renovering med **alla Zava-produkter** (~4 min, för test av långt ljud) |

> **Tips:** Du kan också använda dina egna WAV/MP3/M4A-filer, eller spela in själv med Windows Voice Recorder.

---

### Övning 1 - Ladda ner Whisper-modellen med SDK

På grund av inkompatibiliteter i CLI för Whisper-modeller i nyare Foundry Local-versioner, använd **SDK** för att ladda ner och ladda modellen. Välj ditt språk:

<details>
<summary><b>🐍 Python</b></summary>

**Installera SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Starta tjänsten
manager = FoundryLocalManager()
manager.start_service()

# Kontrollera kataloginformation
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Kontrollera om redan cachad
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Ladda modellen till minnet
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Spara som `download_whisper.py` och kör:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Installera SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Skapa manager och starta tjänsten
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Hämta modell från katalogen
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

// Ladda modellen i minnet
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Spara som `download-whisper.mjs` och kör:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Installera SDK:**
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

> **Varför SDK istället för CLI?** Foundry Local CLI stödjer inte nedladdning eller servering av Whisper-modeller direkt. SDK ger en pålitlig väg att ladda ner och hantera ljudmodeller programmässigt. JavaScript- och C#-SDK:erna inkluderar en inbyggd `AudioClient` som hanterar hela transkriberingsprocessen. Python använder ONNX Runtime för direkt inferens mot cachade modelfiler.

---

### Övning 2 - Förstå Whisper SDK

Whisper-transkribering använder olika tillvägagångssätt beroende på språk. **JavaScript och C#** erbjuder en inbyggd `AudioClient` i Foundry Local SDK som hanterar hela pipelinen (ljudförbehandling, encoder, decoder, tokenavkodning) i ett enda metodanrop. **Python** använder Foundry Local SDK för modellhantering och ONNX Runtime för direkt inferens mot encoder-/decoder-ONNX-modellerna.

| Komponent | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK-paket** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Modellhantering** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalog |
| **Funktionsutvinning** | `WhisperFeatureExtractor` + `librosa` | Hanteras av SDK:s `AudioClient` | Hanteras av SDK:s `AudioClient` |
| **Inferens** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Tokenavkodning** | `WhisperTokenizer` | Hanteras av SDK:s `AudioClient` | Hanteras av SDK:s `AudioClient` |
| **Språkinställning** | Sätts via `forced_ids` i decoder-tokens | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | WAV-filspår | WAV-filspår | WAV-filspår |
| **Output** | Avkodad textsträng | `result.text` | `result.Text` |

> **Viktigt:** Ställ alltid in språket på `AudioClient` (t.ex. `"en"` för engelska). Utan explicit språkinställning kan modellen producera osammanhängande output när den försöker automatiskt identifiera språket.

> **SDK-mönster:** Python använder `FoundryLocalManager(alias)` för att starta, sedan `get_cache_location()` för att hitta ONNX-modelfilerna. JavaScript och C# använder SDK:s inbyggda `AudioClient` – erhållen via `model.createAudioClient()` (JS) eller `model.GetAudioClientAsync()` (C#) – som hanterar hela transkriberingsprocessen. Se [Del 2: Foundry Local SDK Genomgång](part2-foundry-local-sdk.md) för fullständiga detaljer.

---

### Övning 3 - Bygg en Enkel Transkriberingsapp

Välj din språkspårning och bygg en minimal applikation som transkriberar en ljudfil.

> **Stödda audioformat:** WAV, MP3, M4A. För bästa resultat, använd WAV-filer med 16kHz samplingsfrekvens.

<details>
<summary><h3>Python-spår</h3></summary>

#### Installationssteg

```bash
cd python
python -m venv venv

# Aktivera den virtuella miljön:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Transkriberingskod

Skapa en fil `foundry-local-whisper.py`:

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

# Steg 1: Bootstrap - startar tjänsten, laddar ner och laddar modellen
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Bygg sökväg till de cachelagrade ONNX-modellfilerna
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Steg 2: Ladda ONNX-sessioner och funktionsutdragare
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

# Steg 3: Extrahera mel-spektrogramfunktioner
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Steg 4: Kör kodaren
enc_out = encoder.run(None, {"audio_features": input_features})
# Första utgången är dolda tillstånd; resterande är korsuppmärksamhets KV-par
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Steg 5: Autoregressiv avkodning
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transkribera, inga tidsstämplar
input_ids = np.array([initial_tokens], dtype=np.int32)

# Tom cache för självuppmärksamhets KV
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

    if next_token == 50257:  # slut på text
        break
    generated.append(next_token)

    # Uppdatera cache för självuppmärksamhets KV
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Kör den

```bash
# Transkribera ett Zava-produktscenario
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Eller prova andra:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Viktiga punkter för Python

| Metod | Syfte |
|--------|---------|
| `FoundryLocalManager(alias)` | Bootstrap: starta tjänst, ladda ner och ladda modellen |
| `manager.get_cache_location()` | Hämta sökväg till cachade ONNX-modelfiler |
| `WhisperFeatureExtractor.from_pretrained()` | Ladda mel-spektrogram-extraheraren |
| `ort.InferenceSession()` | Skapa ONNX Runtime-sessioner för encoder och decoder |
| `tokenizer.decode()` | Konvertera output-token IDs tillbaka till text |

</details>

<details>
<summary><h3>JavaScript-spår</h3></summary>

#### Installationssteg

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Transkriberingskod

Skapa en fil `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Steg 1: Bootstrap - skapa manager, starta tjänst och ladda modellen
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

// Steg 2: Skapa en ljudklient och transkribera
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Rensa upp
await model.unload();
```

> **Notera:** Foundry Local SDK tillhandahåller en inbyggd `AudioClient` via `model.createAudioClient()` som hanterar hela ONNX-inferenspipelinen internt – ingen import av `onnxruntime-node` behövs. Ställ alltid in `audioClient.settings.language = "en"` för korrekt engelsk transkribering.

#### Kör den

```bash
# Transkribera ett Zava-produktscenario
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Eller prova andra:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Viktiga punkter för JavaScript

| Metod | Syfte |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Skapa manager-instans |
| `await catalog.getModel(alias)` | Hämta modell från katalogen |
| `model.download()` / `model.load()` | Ladda ner och ladda Whisper-modellen |
| `model.createAudioClient()` | Skapa en audio client för transkribering |
| `audioClient.settings.language = "en"` | Ställ in språk för transkribering (krävs för korrekt output) |
| `audioClient.transcribe(path)` | Transkribera en ljudfil, returnerar `{ text, duration }` |

</details>

<details>
<summary><h3>C#-spår</h3></summary>

#### Installationssteg

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Notera:** C#-spåret använder `Microsoft.AI.Foundry.Local`-paketet som tillhandahåller en inbyggd `AudioClient` via `model.GetAudioClientAsync()`. Detta hanterar hela transkriberingspipen internt – ingen separat ONNX Runtime-uppsättning behövs.

#### Transkriberingskod

Byt ut innehållet i `Program.cs`:

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

#### Kör den

```bash
# Transkribera ett Zava-produktscenario
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Eller prova andra:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Viktiga punkter för C#

| Metod | Syfte |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Initiera Foundry Local med konfiguration |
| `catalog.GetModelAsync(alias)` | Hämta modell från katalog |
| `model.DownloadAsync()` | Ladda ner Whisper-modellen |
| `model.GetAudioClientAsync()` | Hämta AudioClient (inte ChatClient!) |
| `audioClient.Settings.Language = "en"` | Ställ in transkriberingsspråk (krävs för korrekt output) |
| `audioClient.TranscribeAudioAsync(path)` | Transkribera en ljudfil |
| `result.Text` | Den transkriberade texten |
> **C# vs Python/JS:** C# SDK erbjuder en inbyggd `AudioClient` för in-process transkribering via `model.GetAudioClientAsync()`, liknande JavaScript SDK. Python använder ONNX Runtime direkt för inferens mot de cachade encoder-/decoder-modellerna.

</details>

---

### Övning 4 - Batch-transkribera alla Zava-exempel

Nu när du har en fungerande transkriptionsapp, transkribera alla fem Zava-exempelfiler och jämför resultaten.

<details>
<summary><h3>Python-spår</h3></summary>

Det fullständiga exemplet `python/foundry-local-whisper.py` stödjer redan batch-transkribering. När det körs utan argument transkriberar det alla `zava-*.wav`-filer i `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Exemplet använder `FoundryLocalManager(alias)` för att starta upp, och kör sedan encoder- och decoder-ONNX-sessionerna för varje fil.

</details>

<details>
<summary><h3>JavaScript-spår</h3></summary>

Det fullständiga exemplet `javascript/foundry-local-whisper.mjs` stödjer redan batch-transkribering. När det körs utan argument transkriberar det alla `zava-*.wav`-filer i `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Exemplet använder `FoundryLocalManager.create()` och `catalog.getModel(alias)` för att initiera SDK:n, och använder sedan `AudioClient` (med `settings.language = "en"`) för att transkribera varje fil.

</details>

<details>
<summary><h3>C#-spår</h3></summary>

Det fullständiga exemplet `csharp/WhisperTranscription.cs` stödjer redan batch-transkribering. När det körs utan ett specifikt filargument, transkriberar det alla `zava-*.wav`-filer i `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Exemplet använder `FoundryLocalManager.CreateAsync()` och SDK:ns `AudioClient` (med `Settings.Language = "en"`) för in-process transkribering.

</details>

**Vad du bör titta efter:** Jämför transkriptionsutdata mot originaltexten i `samples/audio/generate_samples.py`. Hur exakt fångar Whisper produktnamn som "Zava ProGrip" och tekniska termer som "brushless motor" eller "composite decking"?

---

### Övning 5 - Förstå de viktigaste kodmönstren

Studera hur Whisper-transkribering skiljer sig från chat-kompletteringar i alla tre språken:

<details>
<summary><b>Python - Viktiga skillnader från chat</b></summary>

```python
# Chattkomplettering (Delar 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Ljudtranskription (Denna del):
# Använder ONNX Runtime direkt istället för OpenAI-klienten
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregressiv avkodarlop...
print(tokenizer.decode(generated_tokens))
```

**Viktig insikt:** Chatmodeller använder den OpenAI-kompatibla API:n via `manager.endpoint`. Whisper använder SDK:n för att hitta de cachade ONNX-modellfilerna och kör sedan inferens direkt med ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Viktiga skillnader från chat</b></summary>

```javascript
// Chattkomplettering (Del 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Ljudtranskribering (Denna del):
// Använder SDK:ns inbyggda AudioClient
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Sätt alltid språk för bästa resultat
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Viktig insikt:** Chatmodeller använder den OpenAI-kompatibla API:n via `manager.urls[0] + "/v1"`. Whisper-transkribering använder SDK:ns `AudioClient`, som hämtas från `model.createAudioClient()`. Sätt `settings.language` för att undvika förvrängd output från automatisk igenkänning.

</details>

<details>
<summary><b>C# - Viktiga skillnader från chat</b></summary>

C#-metoden använder SDK:ns inbyggda `AudioClient` för in-process transkribering:

**Modelinitialisering:**

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

**Transkribering:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Viktig insikt:** C# använder `FoundryLocalManager.CreateAsync()` och får en `AudioClient` direkt — ingen ONNX Runtime-inställning behövs. Sätt `Settings.Language` för att undvika förvrängd output från automatisk igenkänning.

</details>

> **Sammanfattning:** Python använder Foundry Local SDK för modellhantering och ONNX Runtime för direkt inferens mot encoder-/decodermodeller. JavaScript och C# använder båda SDK:ns inbyggda `AudioClient` för effektiv transkribering — skapa klienten, sätt språket och kalla `transcribe()` / `TranscribeAudioAsync()`. Sätt alltid språkegenskapen på AudioClient för korrekta resultat.

---

### Övning 6 - Experimentera

Testa dessa ändringar för att fördjupa din förståelse:

1. **Testa olika ljudfiler** - spela in dig själv med Windows Voice Recorder, spara som WAV, och transkribera filen

2. **Jämför modellvarianter** - om du har ett NVIDIA GPU, testa CUDA-varianten:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Jämför transkriptionshastigheten mot CPU-varianten.

3. **Lägg till utdataformattering** - JSON-svaret kan inkludera:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Bygg ett REST API** - kapsla in din transkriptionskod i en webbserver:

   | Språk | Ramverk | Exempel |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` med `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` med `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` med `IFormFile` |

5. **Flervarvsdialog med transkribering** - kombinera Whisper med en chatbot från Del 4: transkribera ljud först, och skicka sedan texten till en agent för analys eller sammanfattning.

---

## SDK:s Audio API Referens

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — skapar en `AudioClient`-instans
> - `audioClient.settings.language` — sätt transkriptionsspråk (t.ex. `"en"`)
> - `audioClient.settings.temperature` — styr slumpmässighet (valfritt)
> - `audioClient.transcribe(filePath)` — transkriberar en fil, returnerar `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — strömmar transkriptionsdelar via callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — skapar en `OpenAIAudioClient`-instans
> - `audioClient.Settings.Language` — sätt transkriptionsspråk (t.ex. `"en"`)
> - `audioClient.Settings.Temperature` — styr slumpmässighet (valfritt)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transkriberar en fil, returnerar objekt med `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — returnerar `IAsyncEnumerable` av transkriptionsdelar

> **Tips:** Sätt alltid språkegenskapen innan transkribering. Utan den försöker Whisper-modellen autoavkänna, vilket kan ge förvrängd output (en enda ersättningskaraktär istället för text).

---

## Jämförelse: Chatmodeller vs Whisper

| Aspekt | Chatmodeller (Delar 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Uppgiftstyp** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Inmatning** | Textmeddelanden (JSON) | Ljudfiler (WAV/MP3/M4A) | Ljudfiler (WAV/MP3/M4A) |
| **Utmatning** | Genererad text (strömmande) | Transkriberad text (komplett) | Transkriberad text (komplett) |
| **SDK-paket** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API-metod** | `client.chat.completions.create()` | ONNX Runtime direkt | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Språkinställning** | Ej tillämpligt | Decoder prompt tokens | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Ja | Nej | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Integritetsfördel** | Kod/data stannar lokalt | Ljuddata stannar lokalt | Ljuddata stannar lokalt |

---

## Viktiga insikter

| Koncept | Vad du lärde dig |
|---------|-----------------|
| **Whisper lokalt på enheten** | Tal-till-text körs helt lokalt, perfekt för att transkribera Zava-kundsamtal och produktrecensioner på enheten |
| **SDK AudioClient** | JavaScript- och C#-SDK:er erbjuder en inbyggd `AudioClient` som hanterar hela transkriptionsflödet i ett enda anrop |
| **Språkinställning** | Sätt alltid AudioClient-språket (t.ex. `"en"`) — utan det kan automatisk igenkänning ge förvrängd output |
| **Python** | Använder `foundry-local-sdk` för modellhantering + `onnxruntime` + `transformers` + `librosa` för direkt ONNX-inferens |
| **JavaScript** | Använder `foundry-local-sdk` med `model.createAudioClient()` — sätt `settings.language`, och anropa sedan `transcribe()` |
| **C#** | Använder `Microsoft.AI.Foundry.Local` med `model.GetAudioClientAsync()` — sätt `Settings.Language`, och anropa sedan `TranscribeAudioAsync()` |
| **Streamingstöd** | JS och C# SDK:er erbjuder också `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` för delvis output |
| **CPU-optimerad** | CPU-varianten (3.05 GB) fungerar på vilken Windows-enhet som helst utan GPU |
| **Integritetsfokus** | Perfekt för att hålla Zava-kundinteraktioner och proprietär produktdata på enheten |

---

## Resurser

| Resurs | Länk |
|----------|------|
| Foundry Local-dokumentation | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK-referens | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper-modell | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local webbplats | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Nästa steg

Fortsätt till [Del 10: Använda egna eller Hugging Face-modeller](part10-custom-models.md) för att kompilera egna modeller från Hugging Face och köra dem via Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfriskrivning**:  
Detta dokument har översatts med hjälp av AI-översättningstjänsten [Co-op Translator](https://github.com/Azure/co-op-translator). Även om vi strävar efter noggrannhet, var vänlig observera att automatiska översättningar kan innehålla fel eller felaktigheter. Det ursprungliga dokumentet på dess ursprungliga språk ska betraktas som den auktoritativa källan. För kritisk information rekommenderas professionell mänsklig översättning. Vi ansvarar inte för några missförstånd eller feltolkningar som uppstår från användningen av denna översättning.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->