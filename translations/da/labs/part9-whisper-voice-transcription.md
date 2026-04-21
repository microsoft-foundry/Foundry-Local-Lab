![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 9: Tale Transskription med Whisper og Foundry Local

> **Mål:** Brug OpenAI Whisper-modellen, der kører lokalt via Foundry Local, til at transskribere lydfiler – helt på enheden, uden behov for skyen.

## Oversigt

Foundry Local er ikke kun til tekstgenerering; det understøtter også **tale-til-tekst** modeller. I dette laboratorium vil du bruge **OpenAI Whisper Medium** modellen til at transskribere lydfiler fuldstændigt på din maskine. Dette er ideelt til scenarier som at transskribere Zava kundeservicesamtaler, produktanmeldelser eller planlægningsmøder, hvor lyddata aldrig må forlade din enhed.


---

## Læringsmål

Når du er færdig med dette laboratorie, vil du kunne:

- Forstå Whisper tale-til-tekst modellen og dens funktioner
- Hente og køre Whisper modellen ved hjælp af Foundry Local
- Transskribere lydfiler ved brug af Foundry Local SDK i Python, JavaScript og C#
- Bygge en simpel transskriptionsservice, der kører helt på enheden
- Forstå forskellene mellem chat-/tekstmodeller og audiomodeller i Foundry Local

---

## Forudsætninger

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Version **0.8.101 eller nyere** (Whisper-modeller er tilgængelige fra v0.8.101 og op) |
| **Styresystem** | Windows 10/11 (x64 eller ARM64) |
| **Sprogmiljø** | **Python 3.9+** og/eller **Node.js 18+** og/eller **.NET 9 SDK** ([Download .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Afsluttet** | [Del 1: Kom godt i gang](part1-getting-started.md), [Del 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md), og [Del 3: SDKs og APIs](part3-sdk-and-apis.md) |

> **Bemærk:** Whisper-modeller skal hentes via **SDK’en** (ikke CLI). CLI understøtter ikke audio transskriptions-endpointet. Tjek din version med:
> ```bash
> foundry --version
> ```

---

## Koncept: Hvordan Whisper arbejder med Foundry Local

OpenAI Whisper-modellen er en general-purpose talegenkendelsesmodel trænet på et stort datasæt med forskelligartet lyd. Når den kører gennem Foundry Local:

- Modellen kører **helt på din CPU** – ingen GPU nødvendig
- Lyddata forlader aldrig din enhed – **fuld privatliv**
- Foundry Local SDK håndterer model-download og cache-administration
- **JavaScript og C#** tilbyder en indbygget `AudioClient` i SDK’en, som håndterer hele transskriptions-pipelinen — ingen manuel ONNX-opsætning kræves
- **Python** bruger SDK’en til modelhåndtering og ONNX Runtime til direkte inferens mod encoder/decoder ONNX-modellerne

### Hvordan pipelinen fungerer (JavaScript og C#) — SDK AudioClient

1. **Foundry Local SDK** henter og cacher Whisper-modellen
2. `model.createAudioClient()` (JS) eller `model.GetAudioClientAsync()` (C#) opretter en `AudioClient`
3. `audioClient.transcribe(path)` (JS) eller `audioClient.TranscribeAudioAsync(path)` (C#) håndterer hele pipelinen internt — lydforbehandling, encoder, decoder og token-dekoding
4. `AudioClient` udstiller en `settings.language` egenskab (sæt til `"en"` for engelsk) for at sikre præcis transskription

### Hvordan pipelinen fungerer (Python) — ONNX Runtime

1. **Foundry Local SDK** henter og cacher Whisper ONNX model-filerne
2. **Lydforbehandling** konverterer WAV-lyd til et mel-spektrogram (80 mel-bænke x 3000 frames)
3. **Encoder** behandler mel-spektrogrammet og producerer skjulte tilstande samt cross-attention key/value tensors
4. **Decoder** kører autoregressivt og genererer et token ad gangen, indtil den producerer et slut-af-tekst token
5. **Tokeniser** dekoder output token IDs tilbage til læsbar tekst

### Whisper Modelvarianter

| Alias | Model ID | Enhed | Størrelse | Beskrivelse |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-accelereret (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU-optimeret (anbefales til de fleste enheder) |

> **Bemærk:** I modsætning til chatmodeller, som vises som standard, er Whisper-modeller kategoriseret under `automatic-speech-recognition` opgaven. Brug `foundry model info whisper-medium` for at se detaljer.

---

## Laboratorieøvelser

### Øvelse 0 - Hent Eksempel Lydfiler

Dette laboratorium inkluderer forudbyggede WAV-filer baseret på Zava DIY produkt-scenarier. Generer dem med det inkluderede script:

```bash
# Fra repository-roden - opret og aktiver først en .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Dette opretter seks WAV-filer i `samples/audio/`:

| Fil | Scenario |
|------|----------|
| `zava-customer-inquiry.wav` | Kunde, der spørger om **Zava ProGrip Ledningsfrie Boremaskine** |
| `zava-product-review.wav` | Kunde, der anmelder **Zava UltraSmooth Indendørs Maling** |
| `zava-support-call.wav` | Support-samtale om **Zava TitanLock Værktøjskasse** |
| `zava-project-planning.wav` | DIY’er, der planlægger en terrasse med **Zava EcoBoard Komposit Terrassebrædder** |
| `zava-workshop-setup.wav` | Gennemgang af et værksted med **alle fem Zava produkter** |
| `zava-full-project-walkthrough.wav` | Udvidet garage-renovering med gennemgang af **alle Zava produkter** (~4 min, til test af lange lydfiler) |

> **Tip:** Du kan også bruge dine egne WAV/MP3/M4A filer, eller optage med Windows Stemmememo.

---

### Øvelse 1 - Hent Whisper Modellen med SDK’en

På grund af CLI inkompatibiliteter med Whisper-modeller i nyere Foundry Local versioner, brug **SDK’en** til at hente og indlæse modellen. Vælg dit sprog:

<details>
<summary><b>🐍 Python</b></summary>

**Installer SDK’en:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Start tjenesten
manager = FoundryLocalManager()
manager.start_service()

# Tjek katalogoplysninger
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Tjek om allerede gemt i cache
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Indlæs modellen i hukommelsen
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Gem som `download_whisper.py` og kør:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Installer SDK’en:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Opret manager og start tjenesten
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Hent model fra katalog
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

// Indlæs modellen i hukommelsen
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Gem som `download-whisper.mjs` og kør:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Installer SDK’en:**
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

> **Hvorfor SDK i stedet for CLI?** Foundry Local CLI understøtter ikke at downloade eller serve Whisper-modeller direkte. SDK’en giver en pålidelig måde at hente og administrere audiomodeller programmatisk. JavaScript og C# SDK’erne inkluderer en indbygget `AudioClient`, som håndterer hele transskriptions-pipelinen. Python bruger ONNX Runtime til direkte inferens mod de cachede modelfiler.

---

### Øvelse 2 - Forstå Whisper SDK’en

Whisper transskription bruger forskellige tilgange alt efter sproget. **JavaScript og C#** har en indbygget `AudioClient` i Foundry Local SDK, som håndterer hele pipelinen (lydforbehandling, encoder, decoder, token-dekoding) i ét metodekald. **Python** bruger Foundry Local SDK til modelhåndtering og ONNX Runtime til direkte inferens mod encoder/decoder ONNX-modellerne.

| Komponent | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK pakker** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Modelhåndtering** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **Feature ekstraktion** | `WhisperFeatureExtractor` + `librosa` | Håndteres af SDK `AudioClient` | Håndteres af SDK `AudioClient` |
| **Inferens** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Token-dekodning** | `WhisperTokenizer` | Håndteres af SDK `AudioClient` | Håndteres af SDK `AudioClient` |
| **Sprogindstilling** | Sæt via `forced_ids` i decoder tokens | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | WAV filsti | WAV filsti | WAV filsti |
| **Output** | Dekodet tekststreng | `result.text` | `result.Text` |

> **Vigtigt:** Sæt altid sproget på `AudioClient` (f.eks. `"en"` for engelsk). Uden eksplicit sprogindstilling kan modellen producere uforståeligt output, da den forsøger at autodetektere sproget.

> **SDK Mønstre:** Python bruger `FoundryLocalManager(alias)` til bootstrap, og `get_cache_location()` til at finde ONNX modellerne. JavaScript og C# bruger SDK’ens indbyggede `AudioClient` — opnået med `model.createAudioClient()` (JS) eller `model.GetAudioClientAsync()` (C#) — som håndterer hele transskriptions-pipelinen. Se [Del 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md) for fulde detaljer.

---

### Øvelse 3 - Byg en Simpel Transskriptions App

Vælg din sprogskinne og byg en minimal applikation, der transskriberer en lydfil.

> **Understøttede lydformater:** WAV, MP3, M4A. For bedste resultat, brug WAV-filer med 16kHz sample rate.

<details>
<summary><h3>Python Skinne</h3></summary>

#### Opsætning

```bash
cd python
python -m venv venv

# Aktiver det virtuelle miljø:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Transskriptionskode

Opret en fil `foundry-local-whisper.py`:

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

# Trin 1: Bootstrap - starter tjenesten, downloader og indlæser modellen
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Byg sti til de cachede ONNX-model filer
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Trin 2: Indlæs ONNX-sessioner og feature extractor
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

# Trin 3: Uddrag mel-spektrogram funktioner
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Trin 4: Kør encoder
enc_out = encoder.run(None, {"audio_features": input_features})
# Første output er skjulte tilstande; de resterende er cross-attention KV-par
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Trin 5: Autoregressiv dekodning
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcribe, ikke tidsstempler
input_ids = np.array([initial_tokens], dtype=np.int32)

# Tom self-attention KV-cache
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

    if next_token == 50257:  # slutning af tekst
        break
    generated.append(next_token)

    # Opdater self-attention KV-cache
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Kør den

```bash
# Transkriber et Zava produkt scenarie
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Eller prøv andre:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Centrale Python-punkter

| Metode | Formål |
|--------|---------|
| `FoundryLocalManager(alias)` | Bootstrap: start service, hent og indlæs modellen |
| `manager.get_cache_location()` | Få stien til cachede ONNX modelfiler |
| `WhisperFeatureExtractor.from_pretrained()` | Indlæs mel spektrogram feature extractor |
| `ort.InferenceSession()` | Opret ONNX Runtime sessioner til encoder og decoder |
| `tokenizer.decode()` | Konverter output token IDs tilbage til tekst |

</details>

<details>
<summary><h3>JavaScript Skinne</h3></summary>

#### Opsætning

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Transskriptionskode

Opret en fil `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Trin 1: Bootstrap - opret manager, start tjeneste, og indlæs modellen
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

// Trin 2: Opret en lydklient og transskriber
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Oprydning
await model.unload();
```

> **Bemærk:** Foundry Local SDK leverer en indbygget `AudioClient` via `model.createAudioClient()` som håndterer hele ONNX inferens-pipelinen internt — ingen `onnxruntime-node` import nødvendig. Sæt altid `audioClient.settings.language = "en"` for at sikre præcis engelsk transskription.

#### Kør den

```bash
# Transskriber et Zava produkt scenario
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Eller prøv andre:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Centrale JavaScript-punkter

| Metode | Formål |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Opret manager singleton |
| `await catalog.getModel(alias)` | Hent model fra kataloget |
| `model.download()` / `model.load()` | Hent og indlæs Whisper modellen |
| `model.createAudioClient()` | Opret en audio client til transskription |
| `audioClient.settings.language = "en"` | Sæt transskriptionssprog (påkrævet for korrekt output) |
| `audioClient.transcribe(path)` | Transskriber en lydfil, returnerer `{ text, duration }` |

</details>

<details>
<summary><h3>C# Skinne</h3></summary>

#### Opsætning

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Bemærk:** C# skinnet bruger `Microsoft.AI.Foundry.Local` pakken, der leverer en indbygget `AudioClient` via `model.GetAudioClientAsync()`. Dette håndterer hele transskriptions-pipelinen in-process — ingen separat ONNX Runtime opsætning nødvendig.

#### Transskriptionskode

Erstat indholdet af `Program.cs`:

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

#### Kør den

```bash
# Transskriber et Zava-produkt scenarie
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Eller prøv andre:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Centrale C#-punkter

| Metode | Formål |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Initialiser Foundry Local med konfiguration |
| `catalog.GetModelAsync(alias)` | Hent model fra katalog |
| `model.DownloadAsync()` | Hent Whisper modellen |
| `model.GetAudioClientAsync()` | Få AudioClient (ikke ChatClient!) |
| `audioClient.Settings.Language = "en"` | Sæt transskriptionssprog (påkrævet for korrekt output) |
| `audioClient.TranscribeAudioAsync(path)` | Transskriber en lydfil |
| `result.Text` | Den transskriberede tekst |


> **C# vs Python/JS:** C# SDK’en indeholder en indbygget `AudioClient` til transskription i processen via `model.GetAudioClientAsync()`, svarende til JavaScript SDK’en. Python bruger ONNX Runtime direkte til inferens mod de cachede encoder-/decoder-modeller.

</details>

---

### Øvelse 4 - Batchtransskribér alle Zava-eksempler

Nu hvor du har en fungerende transskriptionsapp, skal du transskribere alle fem Zava-lydfiler og sammenligne resultaterne.

<details>
<summary><h3>Python-sporet</h3></summary>

Det fulde eksempel `python/foundry-local-whisper.py` understøtter allerede batchtransskription. Når det køres uden argumenter, transskriberer det alle `zava-*.wav` filer i `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Eksemplet bruger `FoundryLocalManager(alias)` til at starte op, og kører derefter encoder- og decoder-ONNX-sessionerne for hver fil.

</details>

<details>
<summary><h3>JavaScript-sporet</h3></summary>

Det fulde eksempel `javascript/foundry-local-whisper.mjs` understøtter allerede batchtransskription. Når det køres uden argumenter, transskriberer det alle `zava-*.wav` filer i `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Eksemplet bruger `FoundryLocalManager.create()` og `catalog.getModel(alias)` til at initialisere SDK’en, og bruger derefter `AudioClient` (med `settings.language = "en"`) til at transskribere hver fil.

</details>

<details>
<summary><h3>C#-sporet</h3></summary>

Det fulde eksempel `csharp/WhisperTranscription.cs` understøtter allerede batchtransskription. Når det køres uden et specifikt filargument, transskriberer det alle `zava-*.wav` filer i `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Eksemplet bruger `FoundryLocalManager.CreateAsync()` og SDK’ens `AudioClient` (med `Settings.Language = "en"`) til transskription i processen.

</details>

**Hvad skal du kigge efter:** Sammenlign transskriptionsoutput med den originale tekst i `samples/audio/generate_samples.py`. Hvor nøjagtigt fanger Whisper produktnavne som "Zava ProGrip" og tekniske termer som "brushless motor" eller "composite decking"?

---

### Øvelse 5 - Forstå nøglekode-mønstrene

Studér hvordan Whisper-transskription adskiller sig fra chat-completions i alle tre sprog:

<details>
<summary><b>Python - Nøgleforskelle fra Chat</b></summary>

```python
# Chatfærdiggørelse (Dele 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Lydtransskription (Denne del):
# Bruger ONNX Runtime direkte i stedet for OpenAI-klienten
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregressiv dekoder løkke ...
print(tokenizer.decode(generated_tokens))
```

**Nøgleresultat:** Chatmodeller bruger OpenAI-kompatibel API via `manager.endpoint`. Whisper bruger SDK’en til at finde de cachede ONNX-modelfiler, og kører derefter inferens direkte med ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Nøgleforskelle fra Chat</b></summary>

```javascript
// Chatfuldførelse (Del 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Lydtransskription (Denne del):
// Bruger SDK'ens indbyggede AudioClient
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Indstil altid sprog for bedste resultater
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Nøgleresultat:** Chatmodeller bruger OpenAI-kompatibel API via `manager.urls[0] + "/v1"`. Whisper-transskription bruger SDK’ens `AudioClient`, opnået fra `model.createAudioClient()`. Sæt `settings.language` for at undgå forvrænget output ved autodetektion.

</details>

<details>
<summary><b>C# - Nøgleforskelle fra Chat</b></summary>

C#-tilgangen bruger SDK’ens indbyggede `AudioClient` til transskription i processen:

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

**Transskription:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Nøgleresultat:** C# bruger `FoundryLocalManager.CreateAsync()` og får en `AudioClient` direkte — ingen ONNX Runtime-opsætning nødvendig. Sæt `Settings.Language` for at undgå forvrænget output ved autodetektion.

</details>

> **Opsummering:** Python bruger Foundry Local SDK til modelstyring og ONNX Runtime til direkte inferens mod encoder-/decoder-modellerne. JavaScript og C# bruger begge SDK’ens indbyggede `AudioClient` til en strømlinet transskription — opret klienten, sæt sproget, og kald `transcribe()` / `TranscribeAudioAsync()`. Sæt altid sprogindstillinger på AudioClient for præcise resultater.

---

### Øvelse 6 - Eksperimentér

Prøv disse ændringer for at uddybe din forståelse:

1. **Prøv forskellige lydfiler** - optag dig selv med Windows Voice Recorder, gem som WAV, og transskribér det

2. **Sammenlign modelvarianter** - hvis du har et NVIDIA GPU, prøv CUDA-varianten:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Sammenlign transskriptionshastigheden med CPU-varianten.

3. **Tilføj outputformatering** - JSON-svaret kan indeholde:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Byg en REST API** - pak din transskriptionskode ind i en webserver:

   | Sprog | Framework | Eksempel |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` med `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` med `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` med `IFormFile` |

5. **Multi-turn med transskription** - kombiner Whisper med en chat-agent fra Del 4: transskribér lyd først, og send derefter teksten til agenten til analyse eller opsummering.

---

## SDK Audio API Reference

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — opretter en `AudioClient`-instans
> - `audioClient.settings.language` — sæt transskriptionssproget (fx `"en"`)
> - `audioClient.settings.temperature` — styr tilfældighed (valgfrit)
> - `audioClient.transcribe(filePath)` — transskriberer en fil, returnerer `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — streamer transskriptionsdele via callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — opretter en `OpenAIAudioClient`-instans
> - `audioClient.Settings.Language` — sæt transskriptionssproget (fx `"en"`)
> - `audioClient.Settings.Temperature` — styr tilfældighed (valgfrit)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transskriberer en fil, returnerer objekt med `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — returnerer `IAsyncEnumerable` med transskriptionselementer

> **Tip:** Indstil altid sprogindstillingen før transskription. Uden det prøver Whisper-modellen autodetektion, som kan give forvrænget output (et enkelt udskiftnings-tegn i stedet for tekst).

---

## Sammenligning: Chatmodeller vs. Whisper

| Aspekt | Chatmodeller (Del 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Opgavetype** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Input** | Tekstbeskeder (JSON) | Lydfiler (WAV/MP3/M4A) | Lydfiler (WAV/MP3/M4A) |
| **Output** | Genereret tekst (streamet) | Transskriberet tekst (fuldstændig) | Transskriberet tekst (fuldstændig) |
| **SDK-pakke** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API-metode** | `client.chat.completions.create()` | ONNX Runtime direkte | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Sprogindstilling** | Ikke relevant | Decoder prompt tokens | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Ja | Nej | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Privatlivsfordel** | Kode/data forbliver lokalt | Lyddata forbliver lokalt | Lyddata forbliver lokalt |

---

## Vigtigste pointer

| Koncept | Hvad du lærte |
|---------|---------------|
| **Whisper on-device** | Tale-til-tekst køres fuldstændigt lokalt, ideelt til at transskribere Zava kundesamtaler og produktanmeldelser på enheden |
| **SDK AudioClient** | JavaScript- og C# SDK’er indeholder en indbygget `AudioClient`, som håndterer hele transskriptionspipelinjen i et enkelt kald |
| **Sprogindstilling** | Sæt altid AudioClient sprog (fx `"en"`) — uden det kan autodetektion give forvrænget output |
| **Python** | Bruger `foundry-local-sdk` til modelstyring + `onnxruntime` + `transformers` + `librosa` til direkte ONNX-inferens |
| **JavaScript** | Bruger `foundry-local-sdk` med `model.createAudioClient()` — sæt `settings.language`, og kald `transcribe()` |
| **C#** | Bruger `Microsoft.AI.Foundry.Local` med `model.GetAudioClientAsync()` — sæt `Settings.Language`, og kald `TranscribeAudioAsync()` |
| **Streaming-understøttelse** | JS og C# SDK’er tilbyder også `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` til output i bidder |
| **CPU-optimeret** | CPU-varianten (3,05 GB) fungerer på alle Windows-enheder uden GPU |
| **Privatliv først** | Perfekt til at holde Zava kundedialoger og proprietære produktdata på enheden |

---

## Ressourcer

| Ressource | Link |
|----------|------|
| Foundry Local docs | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Reference | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper model | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local website | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Næste skridt

Fortsæt til [Del 10: Brug af Custom eller Hugging Face modeller](part10-custom-models.md) for at kompilere dine egne modeller fra Hugging Face og køre dem via Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokument er blevet oversat ved hjælp af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi bestræber os på nøjagtighed, skal du være opmærksom på, at automatiserede oversættelser kan indeholde fejl eller unøjagtigheder. Det oprindelige dokument på dets oprindelige sprog bør betragtes som den autoritative kilde. For kritiske oplysninger anbefales professionel menneskelig oversættelse. Vi kan ikke holdes ansvarlige for eventuelle misforståelser eller fortolkningsfejl, der opstår som følge af brugen af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->