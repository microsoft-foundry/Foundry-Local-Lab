![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 9: Tale-transkripsjon med Whisper og Foundry Local

> **Mål:** Bruk OpenAI Whisper-modellen som kjører lokalt gjennom Foundry Local for å transkribere lydfiler - helt på enheten, uten behov for sky.

## Oversikt

Foundry Local er ikke bare for tekstgenerering; det støtter også **tale-til-tekst**-modeller. I dette laboratoriet vil du bruke **OpenAI Whisper Medium**-modellen for å transkribere lydfiler helt på din maskin. Dette er ideelt for scenarier som transkribering av Zava kundeservicesamtaler, produktanmeldelser, eller planleggingsmøter for workshops hvor lyddata aldri må forlate enheten din.


---

## Læringsmål

Etter avsluttet lab skal du kunne:

- Forstå Whisper tale-til-tekst-modellen og dens egenskaper
- Laste ned og kjøre Whisper-modellen med Foundry Local
- Transkribere lydfiler ved bruk av Foundry Local SDK i Python, JavaScript og C#
- Bygge en enkel transkripsjonstjeneste som kjører helt på enhet
- Forstå forskjellene mellom chat-/tekstmodeller og lydmodeller i Foundry Local

---

## Forutsetninger

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Versjon **0.8.101 eller nyere** (Whisper-modeller er tilgjengelige fra v0.8.101 og oppover) |
| **OS** | Windows 10/11 (x64 eller ARM64) |
| **Språk-runtime** | **Python 3.9+** og/eller **Node.js 18+** og/eller **.NET 9 SDK** ([Last ned .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Fullført** | [Del 1: Komme i gang](part1-getting-started.md), [Del 2: Foundry Local SDK Dypdykk](part2-foundry-local-sdk.md), og [Del 3: SDKer og APIer](part3-sdk-and-apis.md) |

> **Merk:** Whisper-modeller må lastes ned via **SDK-en** (ikke CLI). CLI støtter ikke endepunkt for lydtranskripsjon. Sjekk versjonen din med:
> ```bash
> foundry --version
> ```

---

## Konsept: Hvordan Whisper fungerer med Foundry Local

OpenAI Whisper-modellen er en generell talegjenkjenningsmodell trent på et stort datasett med ulik lyd. Ved kjøring gjennom Foundry Local:

- Modellen kjører **helt på CPU-en din** - ingen GPU nødvendig
- Lyddata forlater aldri enheten din - **fullstendig personvern**
- Foundry Local SDK håndterer modell-nedlasting og cache-administrasjon
- **JavaScript og C#** tilbyr en innebygd `AudioClient` i SDK som håndterer hele transkripsjonspipelinen — ingen manuell ONNX-oppsett nødvendig
- **Python** bruker SDK for modellhåndtering og ONNX Runtime for direkte inferens mot encoder/decoder ONNX-modellene

### Hvordan pipelinen fungerer (JavaScript og C#) — SDK AudioClient

1. **Foundry Local SDK** laster ned og cacher Whisper-modellen
2. `model.createAudioClient()` (JS) eller `model.GetAudioClientAsync()` (C#) oppretter en `AudioClient`
3. `audioClient.transcribe(path)` (JS) eller `audioClient.TranscribeAudioAsync(path)` (C#) håndterer hele pipelinen internt — lydforbehandling, encoder, decoder og token-avkoding
4. `AudioClient` eksponerer en `settings.language`-egenskap (settes til `"en"` for engelsk) for å styre nøyaktig transkripsjon

### Hvordan pipelinen fungerer (Python) — ONNX Runtime

1. **Foundry Local SDK** laster ned og cacher Whisper ONNX-modellfiler
2. **Lydforbehandling** konverterer WAV-lyd til mel-spektrogram (80 mel-bin x 3000 rammer)
3. **Encoder** behandler mel-spektrogrammet og produserer skjulte tilstander samt nøkkel/verdi-tensorer for kryssoppmerksomhet
4. **Decoder** kjører autoregressivt, genererer ett token av gangen til den produserer et slutten-av-tekst-token
5. **Tokeniser** dekoder output token-IDer tilbake til lesbar tekst

### Whisper-modellvarianter

| Alias | Modell-ID | Enhet | Størrelse | Beskrivelse |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1,53 GB | GPU-akselerert (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3,05 GB | CPU-optimalisert (anbefalt for de fleste enheter) |

> **Merk:** I motsetning til chat-modeller som listes som standard, kategoriseres Whisper-modeller under oppgaven `automatic-speech-recognition`. Bruk `foundry model info whisper-medium` for å se detaljer.

---

## Laboratorieoppgaver

### Oppgave 0 - Skaff eksempler på lydfiler

Denne labben inkluderer forhåndslagde WAV-filer basert på Zava DIY produkt-scenarier. Generer dem med det medfølgende skriptet:

```bash
# Fra rotmappen til repoet - opprett og aktiver først et .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Dette lager seks WAV-filer i `samples/audio/`:

| Fil | Scenario |
|------|----------|
| `zava-customer-inquiry.wav` | Kunde som spør om **Zava ProGrip trådløs drill** |
| `zava-product-review.wav` | Kunde som vurderer **Zava UltraSmooth Interiørmaling** |
| `zava-support-call.wav` | Support-samtale om **Zava TitanLock Verktøyskrin** |
| `zava-project-planning.wav` | DIYer planlegger en terrasse med **Zava EcoBoard Komposittdekking** |
| `zava-workshop-setup.wav` | Gjennomgang av et verksted med **alle fem Zava-produktene** |
| `zava-full-project-walkthrough.wav` | Utvidet gjennomgang av garasjerestaurering med **alle Zava-produktene** (~4 min, for testing av lange lydfiler) |

> **Tips:** Du kan også bruke egne WAV/MP3/M4A-filer, eller ta opp selv med Windows Voice Recorder.

---

### Oppgave 1 - Last ned Whisper-modellen med SDK

På grunn av CLI-inkompatibilitet med Whisper-modeller i nyere Foundry Local-versjoner, bruk **SDK-en** for å laste ned og laste modellen. Velg ditt språk:

<details>
<summary><b>🐍 Python</b></summary>

**Installer SDK-en:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Start tjenesten
manager = FoundryLocalManager()
manager.start_service()

# Sjekk kataloginformasjon
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Sjekk om allerede bufret
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Last modellen inn i minnet
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Lagre som `download_whisper.py` og kjør:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Installer SDK-en:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Opprett manager og start tjenesten
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Hent modell fra katalog
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

// Last modellen inn i minnet
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Lagre som `download-whisper.mjs` og kjør:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Installer SDK-en:**
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

> **Hvorfor SDK i stedet for CLI?** Foundry Local CLI støtter ikke nedlasting eller bruk av Whisper-modeller direkte. SDK-en gir en pålitelig måte å laste ned og administrere lydmodeller programmert. JavaScript og C# SDK-ene inkluderer en innebygd `AudioClient` som håndterer hele transkripsjonspipelinen. Python bruker ONNX Runtime for direkte inferens mot bufrede modelfiler.

---

### Oppgave 2 - Forstå Whisper SDK

Whisper-transkripsjon bruker ulike tilnærminger avhengig av språk. **JavaScript og C#** tilbyr en innebygd `AudioClient` i Foundry Local SDK som håndterer hele pipelinen (lydforbehandling, encoder, decoder, token-avkoding) i én metodekall. **Python** bruker Foundry Local SDK for modellstyring og ONNX Runtime for direkte inferens mot encoder/decoder ONNX-modellene.

| Komponent | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK-pakker** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Modellhåndtering** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalog |
| **Egenskapsuttrekk** | `WhisperFeatureExtractor` + `librosa` | Håndteres av SDK `AudioClient` | Håndteres av SDK `AudioClient` |
| **Inferens** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Token-avkoding** | `WhisperTokenizer` | Håndteres av SDK `AudioClient` | Håndteres av SDK `AudioClient` |
| **Språkinnstilling** | Settes via `forced_ids` i dekodertokens | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Inndata** | Filbane til WAV | Filbane til WAV | Filbane til WAV |
| **Utdata** | Dekodet tekststreng | `result.text` | `result.Text` |

> **Viktig:** Sett alltid språk i `AudioClient` (f.eks. `"en"` for engelsk). Uten eksplisitt språkinnstilling kan modellen produsere uleselig output da den prøver å autodeteksere språk.

> **SDK-mønstre:** Python bruker `FoundryLocalManager(alias)` for å starte opp, så `get_cache_location()` for å finne ONNX-modellfiler. JavaScript og C# bruker SDK-ens innebygde `AudioClient` — oppnådd via `model.createAudioClient()` (JS) eller `model.GetAudioClientAsync()` (C#) — som håndterer hele transkripsjonspipelinen. Se [Del 2: Foundry Local SDK Dypdykk](part2-foundry-local-sdk.md) for fullstendige detaljer.

---

### Oppgave 3 - Lag en enkel transkripsjonsapp

Velg ditt språkspor og bygg en minimal applikasjon som transkriberer en lydfil.

> **Støttede lydformater:** WAV, MP3, M4A. For best resultat, bruk WAV-filer med 16kHz samplingsfrekvens.

<details>
<summary><h3>Python-spor</h3></summary>

#### Oppsett

```bash
cd python
python -m venv venv

# Aktiver det virtuelle miljøet:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Transkripsjonskode

Opprett en fil `foundry-local-whisper.py`:

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

# Trinn 1: Bootstrap - starter tjenesten, laster ned og laster modellen
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Bygg sti til de bufrede ONNX-modellfilene
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Trinn 2: Last ONNX-økter og funksjonsuttrekker
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

# Trinn 3: Ekstraher mel-spektrogramfunksjoner
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Trinn 4: Kjør koder
enc_out = encoder.run(None, {"audio_features": input_features})
# Første utdata er skjulte tilstander; resten er kryssoppmerksomhets-KV-par
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Trinn 5: Autoregressiv dekoding
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transkriber, ingen tidsstempler
input_ids = np.array([initial_tokens], dtype=np.int32)

# Tom selvoppmerksomhets-KV-buffer
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

    if next_token == 50257:  # slutt på tekst
        break
    generated.append(next_token)

    # Oppdater selvoppmerksomhets-KV-buffer
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Kjør den

```bash
# Transkriber et Zava produktscenario
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Eller prøv andre:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Viktige punkter for Python

| Metode | Formål |
|--------|---------|
| `FoundryLocalManager(alias)` | Bootstrap: start tjenesten, last ned og last inn modellen |
| `manager.get_cache_location()` | Få banen til bufrede ONNX-modellfiler |
| `WhisperFeatureExtractor.from_pretrained()` | Last inn mel-spektrogram egenskapsuttrekker |
| `ort.InferenceSession()` | Opprett ONNX Runtime-økter for encoder og decoder |
| `tokenizer.decode()` | Konverter output token-IDer tilbake til tekst |

</details>

<details>
<summary><h3>JavaScript-spor</h3></summary>

#### Oppsett

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Transkripsjonskode

Opprett en fil `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Trinn 1: Bootstrap - opprett manager, start tjeneste, og last modellen
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

// Trinn 2: Opprett en lydklient og transkriber
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Opprydding
await model.unload();
```

> **Merk:** Foundry Local SDK tilbyr en innebygd `AudioClient` via `model.createAudioClient()` som håndterer hele ONNX-inferenspipen internt — ingen import av `onnxruntime-node` trengs. Sett alltid `audioClient.settings.language = "en"` for å sikre nøyaktig transkripsjon på engelsk.

#### Kjør den

```bash
# Transkriber et Zava-produktscenario
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Eller prøv andre:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Viktige punkter for JavaScript

| Metode | Formål |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Opprett manager singleton |
| `await catalog.getModel(alias)` | Hent modell fra katalogen |
| `model.download()` / `model.load()` | Last ned og last inn Whisper-modellen |
| `model.createAudioClient()` | Opprett en lydklient for transkripsjon |
| `audioClient.settings.language = "en"` | Sett transkripsjonsspråk (kreves for nøyaktig output) |
| `audioClient.transcribe(path)` | Transkriber lydfil, returnerer `{ text, duration }` |

</details>

<details>
<summary><h3>C#-spor</h3></summary>

#### Oppsett

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Merk:** C#-sporet bruker `Microsoft.AI.Foundry.Local`-pakken som tilbyr en innebygd `AudioClient` via `model.GetAudioClientAsync()`. Denne håndterer hele transkripsjonspipelinen internt — ingen separat ONNX Runtime-oppsett nødvendig.

#### Transkripsjonskode

Erstatt innholdet i `Program.cs`:

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

#### Kjør den

```bash
# Transkriber et Zava produkt scenario
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Eller prøv andre:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Viktige punkter for C#

| Metode | Formål |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Start Foundry Local med konfigurasjon |
| `catalog.GetModelAsync(alias)` | Hent modell fra katalogen |
| `model.DownloadAsync()` | Last ned Whisper-modellen |
| `model.GetAudioClientAsync()` | Hent AudioClient (ikke ChatClient!) |
| `audioClient.Settings.Language = "en"` | Sett transkripsjonsspråk (kreves for nøyaktig output) |
| `audioClient.TranscribeAudioAsync(path)` | Transkriber lydfil |
| `result.Text` | Den transkriberte teksten |


> **C# vs Python/JS:** C# SDK-en tilbyr en innebygd `AudioClient` for transkripsjon i prosess via `model.GetAudioClientAsync()`, lik den i JavaScript SDK-en. Python bruker ONNX Runtime direkte for inferens mot de cacherte encoder/decoder-modellene.

</details>

---

### Øvelse 4 - Batchtranskribér Alle Zava Eksempel

Nå som du har en fungerende transkripsjonsapp, transkribér alle fem Zava prøvefiler og sammenlign resultatene.

<details>
<summary><h3>Python-løype</h3></summary>

Det fullstendige eksempelet `python/foundry-local-whisper.py` støtter allerede batchtranskripsjon. Når det kjøres uten argumenter, transkriberer det alle `zava-*.wav` filer i `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Eksempelet bruker `FoundryLocalManager(alias)` for å starte opp, og kjører deretter encoder og decoder ONNX-økter for hver fil.

</details>

<details>
<summary><h3>JavaScript-løype</h3></summary>

Det fullstendige eksempelet `javascript/foundry-local-whisper.mjs` støtter allerede batchtranskripsjon. Når det kjøres uten argumenter, transkriberer det alle `zava-*.wav` filer i `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Eksempelet bruker `FoundryLocalManager.create()` og `catalog.getModel(alias)` for å initialisere SDK, og bruker deretter `AudioClient` (med `settings.language = "en"`) for å transkribere hver fil.

</details>

<details>
<summary><h3>C#-løype</h3></summary>

Det fullstendige eksempelet `csharp/WhisperTranscription.cs` støtter allerede batchtranskripsjon. Når det kjøres uten spesifikt filargument, transkriberer det alle `zava-*.wav` filer i `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Eksempelet bruker `FoundryLocalManager.CreateAsync()` og SDK-ens `AudioClient` (med `Settings.Language = "en"`) for transkripsjon i prosess.

</details>

**Hva du skal se etter:** Sammenlign transkripsjonsutdata med originalteksten i `samples/audio/generate_samples.py`. Hvor nøyaktig fanger Whisper opp produktnavn som "Zava ProGrip" og tekniske begreper som "brushless motor" eller "composite decking"?

---

### Øvelse 5 - Forstå Nøkkel Kodemønstre

Studer hvordan Whisper-transkripsjon skiller seg fra chat fullføringer på tvers av alle tre språk:

<details>
<summary><b>Python - Viktige forskjeller fra Chat</b></summary>

```python
# Chat fullføring (Del 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Lyd transkripsjon (Denne delen):
# Bruker ONNX Runtime direkte i stedet for OpenAI-klienten
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregressiv dekoder løkke ...
print(tokenizer.decode(generated_tokens))
```

**Viktig innsikt:** Chat-modeller bruker OpenAI-kompatibel API via `manager.endpoint`. Whisper bruker SDK for å finne de cacherte ONNX-modellfilene, og kjører inferens direkte med ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Viktige forskjeller fra Chat</b></summary>

```javascript
// Chat fullføring (Deler 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Lydd transkripsjon (Denne delen):
// Bruker SDKs innebygde AudioClient
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Sett alltid språk for best resultat
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Viktig innsikt:** Chat-modeller bruker OpenAI-kompatibel API via `manager.urls[0] + "/v1"`. Whisper-transkripsjon bruker SDKs `AudioClient` hented fra `model.createAudioClient()`. Sett `settings.language` for å unngå uforståelig output fra autodeteksjon.

</details>

<details>
<summary><b>C# - Viktige forskjeller fra Chat</b></summary>

C#-metoden bruker SDKs innebygde `AudioClient` for transkripsjon i prosess:

**Modellinitialisering:**

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

**Transkripsjon:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Viktig innsikt:** C# bruker `FoundryLocalManager.CreateAsync()` og får en `AudioClient` direkte — ingen ONNX Runtime-oppsett nødvendig. Sett `Settings.Language` for å unngå uforståelig output fra autodeteksjon.

</details>

> **Oppsummering:** Python bruker Foundry Local SDK for modellhåndtering og ONNX Runtime for direkte inferens på encoder/decoder-modellene. JavaScript og C# bruker begge SDKs innebygde `AudioClient` for effektiv transkripsjon — opprett klienten, sett språk, og kall `transcribe()` / `TranscribeAudioAsync()`. Sett alltid språk på AudioClient for nøyaktige resultater.

---

### Øvelse 6 - Eksperimenter

Prøv disse endringene for å utdype forståelsen din:

1. **Prøv forskjellige lydfiler** - ta opp deg selv som snakker med Windows Voice Recorder, lagre som WAV, og transkribér det

2. **Sammenlign modelvarianter** - hvis du har en NVIDIA GPU, prøv CUDA-varianten:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Sammenlign transkripsjonshastigheten med CPU-varianten.

3. **Legg til utdataformatering** - JSON-responsen kan inkludere:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Bygg et REST API** - pakk transkripsjonskoden din inn i en webserver:

   | Språk | Rammeverk | Eksempel |
   |-------|-----------|----------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` med `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` med `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` med `IFormFile` |

5. **Multi-turn med transkripsjon** - kombiner Whisper med en chat-agent fra Del 4: transkribér lyd først, deretter send teksten til en agent for analyse eller oppsummering.

---

## SDK Audio API Referanse

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — oppretter en instans av `AudioClient`
> - `audioClient.settings.language` — sett transkripsjonsspråk (f.eks. `"en"`)
> - `audioClient.settings.temperature` — styrer tilfeldighet (valgfritt)
> - `audioClient.transcribe(filePath)` — transkribér en fil, returnerer `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — strøm transkripsjonsbiter via callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — oppretter en instans av `OpenAIAudioClient`
> - `audioClient.Settings.Language` — sett transkripsjonsspråk (f.eks. `"en"`)
> - `audioClient.Settings.Temperature` — styrer tilfeldighet (valgfritt)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transkribér en fil, returnerer objekt med `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — returnerer `IAsyncEnumerable` av transkripsjonsbiter

> **Tips:** Sett alltid språk-egenskapen før du transkriberer. Uten den forsøker Whisper-modellen autodeteksjon, noe som kan gi uforståelig output (en enkelt erstatningskarakter i stedet for tekst).

---

## Sammenligning: Chat-modeller vs. Whisper

| Aspekt | Chat-modeller (Del 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|-------------------------|-----------------|-------------------|
| **Oppgavetype** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Input** | Tekstmeldinger (JSON) | Lydfiler (WAV/MP3/M4A) | Lydfiler (WAV/MP3/M4A) |
| **Output** | Generert tekst (strømmet) | Transkribert tekst (komplett) | Transkribert tekst (komplett) |
| **SDK-pakke** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API-metode** | `client.chat.completions.create()` | ONNX Runtime direkte | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Språkinnstilling** | Ikke relevant | Dekoder prompt tokens | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Strømming** | Ja | Nei | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Personvernsfordel** | Kode/data forblir lokalt | Lyddata forblir lokalt | Lyddata forblir lokalt |

---

## Viktige Læringspunkter

| Konsept | Hva Du Lærte |
|---------|--------------|
| **Whisper on-device** | Tale-til-tekst kjøres helt lokalt, ideelt for å transkribere Zava kundeanrop og produktanmeldelser på enheten |
| **SDK AudioClient** | JavaScript og C# SDKer tilbyr en innebygd `AudioClient` som håndterer hele transkripsjonspipelinen i ett kall |
| **Språkinnstilling** | Sett alltid AudioClient språk (f.eks. `"en"`) — uten det kan autodeteksjon gi uforståelig output |
| **Python** | Bruker `foundry-local-sdk` for modellhåndtering + `onnxruntime` + `transformers` + `librosa` for direkte ONNX-inferens |
| **JavaScript** | Bruker `foundry-local-sdk` med `model.createAudioClient()` — sett `settings.language`, så kall `transcribe()` |
| **C#** | Bruker `Microsoft.AI.Foundry.Local` med `model.GetAudioClientAsync()` — sett `Settings.Language`, så kall `TranscribeAudioAsync()` |
| **Strømmestøtte** | JS og C# SDKer tilbyr også `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` for bitvis output |
| **CPU-optimalisert** | CPU-varianten (3.05 GB) fungerer på alle Windows-enheter uten GPU |
| **Personvern først** | Perfekt for å holde Zava kundedata og proprietære produktdata på enheten |

---

## Ressurser

| Ressurs | Lenke |
|---------|-------|
| Foundry Local-dokumentasjon | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referanse | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper modell | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local nettsted | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Neste Steg

Fortsett til [Del 10: Bruke egendefinerte eller Hugging Face-modeller](part10-custom-models.md) for å kompilere dine egne modeller fra Hugging Face og kjøre dem med Foundry Local.