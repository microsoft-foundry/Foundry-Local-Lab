![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partea 9: Transcriere Vocală cu Whisper și Foundry Local

> **Scop:** Folosește modelul OpenAI Whisper care rulează local prin Foundry Local pentru a transcrie fișiere audio - complet pe dispozitiv, fără cloud.

## Prezentare generală

Foundry Local nu este doar pentru generare de text; suportă și modele **speech-to-text**. În acest laborator vei folosi modelul **OpenAI Whisper Medium** pentru a transcrie fișiere audio integral pe mașina ta. Acest lucru este ideal pentru scenarii precum transcrierea apelurilor serviciului clienți Zava, înregistrări de recenzii de produs sau sesiuni de planificare a workshop-urilor unde datele audio nu trebuie să părăsească dispozitivul.

---

## Obiective de învățare

Până la finalul acestui laborator vei putea:

- Înțelege modelul Whisper speech-to-text și capabilitățile sale
- Descărca și rula modelul Whisper folosind Foundry Local
- Transcrie fișiere audio utilizând Foundry Local SDK în Python, JavaScript și C#
- Construiește un serviciu simplu de transcriere care rulează complet pe dispozitiv
- Înțelege diferențele dintre modelele chat/text și modelele audio în Foundry Local

---

## Cerințe preliminare

| Cerință | Detalii |
|-------------|---------|
| **Foundry Local CLI** | Versiunea **0.8.101 sau mai nouă** (modelele Whisper sunt disponibile din v0.8.101 încolo) |
| **Sistem de operare** | Windows 10/11 (x64 sau ARM64) |
| **Runtime limbaj** | **Python 3.9+** și/sau **Node.js 18+** și/sau **.NET 9 SDK** ([Descarcă .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Completat** | [Partea 1: Primii pași](part1-getting-started.md), [Partea 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md), și [Partea 3: SDK-uri și API-uri](part3-sdk-and-apis.md) |

> **Notă:** Modelele Whisper trebuie descărcate prin intermediul **SDK-ului** (nu CLI-ul). CLI-ul nu suportă endpoint-ul de transcriere audio. Verifică versiunea cu:
> ```bash
> foundry --version
> ```

---

## Concept: Cum funcționează Whisper cu Foundry Local

Modelul OpenAI Whisper este un model general de recunoaștere vocală antrenat pe un set mare de date audio diverse. Când rulează prin Foundry Local:

- Modelul rulează **complet pe CPU-ul tău** - nu este necesar GPU
- Audio nu părăsește niciodată dispozitivul - **confidențialitate completă**
- SDK Foundry Local se ocupă de descărcarea modelului și gestionarea cache-ului
- **JavaScript și C#** oferă un `AudioClient` integrat în SDK care gestionează întreg procesul de transcriere — fără configurări ONNX manuale necesare
- **Python** folosește SDK-ul pentru managementul modelului și ONNX Runtime pentru inferență directă pe modelele encoder/decoder ONNX

### Cum funcționează pipeline-ul (JavaScript și C#) — SDK AudioClient

1. **Foundry Local SDK** descarcă și pune în cache modelul Whisper
2. `model.createAudioClient()` (JS) sau `model.GetAudioClientAsync()` (C#) creează un `AudioClient`
3. `audioClient.transcribe(path)` (JS) sau `audioClient.TranscribeAudioAsync(path)` (C#) gestionează intern tot pipeline-ul — preprocesare audio, encoder, decoder și decodare tokeni
4. `AudioClient` expune o proprietate `settings.language` (setată pe `"en"` pentru engleză) pentru a ghida transcrierea corectă

### Cum funcționează pipeline-ul (Python) — ONNX Runtime

1. **Foundry Local SDK** descarcă și pune în cache fișierele modelului Whisper ONNX
2. **Preprocesare audio** convertește audio WAV într-un spectrogramă mel (80 de benzi mel x 3000 cadre)
3. **Encoder** procesează spectrograma mel și produce stări ascunse plus tensori cheie/valoare cross-attention
4. **Decoder** rulează autoregresiv, generând câte un token pe rând până la tokenul de sfârșit-text
5. **Tokenizatorul** decodează ID-urile tokenilor generați înapoi în text lizibil

### Variante ale modelului Whisper

| Alias | ID Model | Dispozitiv | Mărime | Descriere |
|-------|----------|------------|--------|-----------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Accelerat GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Optim pentru CPU (recomandat pentru majoritatea dispozitivelor) |

> **Notă:** Spre deosebire de modelele de chat care apar implicit, modelele Whisper sunt clasificate sub sarcina `automatic-speech-recognition`. Folosește `foundry model info whisper-medium` pentru detalii.

---

## Exerciții din laborator

### Exercițiul 0 - Obține Fișiere Audio Exemplu

Acest laborator include fișiere WAV preconstruite bazate pe scenarii DIY Zava. Generează-le cu scriptul inclus:

```bash
# Din rădăcina depozitului - creează și activează mai întâi un .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Acesta creează șase fișiere WAV în `samples/audio/`:

| Fișier | Scenariu |
|------|----------|
| `zava-customer-inquiry.wav` | Client care întreabă despre **Zava ProGrip Mașină de găurit fără fir** |
| `zava-product-review.wav` | Client care review-uiește **Vopsea Interioară UltraSmooth Zava** |
| `zava-support-call.wav` | Apel suport despre **Zava TitanLock Casetă Scule** |
| `zava-project-planning.wav` | DIYer care planifică o terasă cu **Zava EcoBoard Compus Terasa** |
| `zava-workshop-setup.wav` | Tur printr-un atelier folosind **toate cele cinci produse Zava** |
| `zava-full-project-walkthrough.wav` | Tur extins de renovare garaj folosind **toate produsele Zava** (~4 min, pentru testare audio lungă) |

> **Sfat:** Poți folosi și propriile fișiere WAV/MP3/M4A sau poți înregistra cu Windows Voice Recorder.

---

### Exercițiul 1 - Descarcă Modelul Whisper Folosind SDK-ul

Datorită incompatibilităților CLI cu modelele Whisper în versiunile noi Foundry Local, folosește **SDK-ul** pentru a descărca și încărca modelul. Alege-ți limbajul:

<details>
<summary><b>🐍 Python</b></summary>

**Instalează SDK-ul:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Porniți serviciul
manager = FoundryLocalManager()
manager.start_service()

# Verificați informațiile catalogului
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Verificați dacă este deja în cache
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Încărcați modelul în memorie
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Salvează ca `download_whisper.py` și rulează:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Instalează SDK-ul:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Creează managerul și pornește serviciul
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Obține modelul din catalog
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

// Încarcă modelul în memorie
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Salvează ca `download-whisper.mjs` și rulează:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Instalează SDK-ul:**
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

> **De ce SDK în loc de CLI?** CLI-ul Foundry Local nu suportă descărcarea sau servirea directă a modelelor Whisper. SDK-ul oferă o cale sigură de a descărca și gestiona modelele audio programatic. SDK-urile JavaScript și C# includ un `AudioClient` integrat care gestionează întreg pipeline-ul de transcriere. Python folosește ONNX Runtime pentru inferență directă pe fișierele de model puse în cache.

---

### Exercițiul 2 - Înțelege SDK-ul Whisper

Transcrierea Whisper folosește abordări diferite în funcție de limbaj. **JavaScript și C#** oferă un `AudioClient` încorporat în Foundry Local SDK care gestionează tot pipeline-ul (preprocesare audio, encoder, decoder, decodare tokeni) într-o singură metodă. **Python** folosește Foundry Local SDK pentru managementul modelului și ONNX Runtime pentru inferență directă pe modelele encoder/decoder ONNX.

| Componentă | Python | JavaScript | C# |
|------------|--------|------------|----|
| **Pachete SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Management model** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **Extracție caracteristici** | `WhisperFeatureExtractor` + `librosa` | Gestionat de SDK `AudioClient` | Gestionat de SDK `AudioClient` |
| **Inferență** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Decodare tokeni** | `WhisperTokenizer` | Gestionat de SDK `AudioClient` | Gestionat de SDK `AudioClient` |
| **Setarea limbii** | Setare via `forced_ids` în tokenii decoderului | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | Cale fișier WAV | Cale fișier WAV | Cale fișier WAV |
| **Output** | Șir text decodat | `result.text` | `result.Text` |

> **Important:** Setează întotdeauna limba pe `AudioClient` (ex: `"en"` pentru engleză). Fără limbă explicit setată, modelul poate produce un output confuz deoarece încearcă să detecteze automat limba.

> **Tipare SDK:** Python folosește `FoundryLocalManager(alias)` pentru bootstrap, apoi `get_cache_location()` pentru a găsi fișierele ONNX. JavaScript și C# folosesc `AudioClient` integrat în SDK, obținut prin `model.createAudioClient()` (JS) sau `model.GetAudioClientAsync()` (C#), care se ocupa de întreg pipeline-ul. Vezi [Partea 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md) pentru detalii complete.

---

### Exercițiul 3 - Construiește o Aplicație Simplă de Transcriere

Alege limbajul dorit și construiește o aplicație minimală care transcrie un fișier audio.

> **Formate audio suportate:** WAV, MP3, M4A. Pentru cele mai bune rezultate, folosește fișiere WAV cu rata de eșantionare 16kHz.

<details>
<summary><h3>Traseul Python</h3></summary>

#### Setup

```bash
cd python
python -m venv venv

# Activează mediul virtual:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Cod pentru Transcriere

Creează un fișier `foundry-local-whisper.py`:

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

# Pasul 1: Bootstrap - pornește serviciul, descarcă și încarcă modelul
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Construiește calea către fișierele model ONNX în cache
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Pasul 2: Încarcă sesiunile ONNX și extractorul de caracteristici
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

# Pasul 3: Extrage caracteristicile spectrogramelor mel
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Pasul 4: Rulează encoder-ul
enc_out = encoder.run(None, {"audio_features": input_features})
# Prima ieșire sunt stările ascunse; restul sunt perechi KV de atenție încrucișată
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Pasul 5: Decodare autoregresivă
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcrie, fără timestamp-uri
input_ids = np.array([initial_tokens], dtype=np.int32)

# Cache KV de auto-atenție gol
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

    if next_token == 50257:  # sfârșitul textului
        break
    generated.append(next_token)

    # Actualizează cache-ul KV de auto-atenție
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Rulează-l

```bash
# Transcrie un scenariu de produs Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Sau încearcă altele:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Puncte cheie Python

| Metodă | Scop |
|--------|-------|
| `FoundryLocalManager(alias)` | Bootstrap: pornește serviciul, descarcă și încarcă modelul |
| `manager.get_cache_location()` | Obține calea către fișierele ONNX în cache |
| `WhisperFeatureExtractor.from_pretrained()` | Încarcă extractoare spectrograf mel |
| `ort.InferenceSession()` | Creează sesiuni ONNX Runtime pentru encoder și decoder |
| `tokenizer.decode()` | Convertește ID-urile tokenilor înapoi în text |

</details>

<details>
<summary><h3>Traseul JavaScript</h3></summary>

#### Setup

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Cod pentru Transcriere

Creează un fișier `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Pasul 1: Bootstrap - creează managerul, pornește serviciul și încarcă modelul
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

// Pasul 2: Creează un client audio și transcrie
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Curățare
await model.unload();
```

> **Notă:** Foundry Local SDK oferă un `AudioClient` integrat prin `model.createAudioClient()` care gestionează intern întreg pipeline-ul ONNX — nu este necesar import `onnxruntime-node`. Setează întotdeauna `audioClient.settings.language = "en"` pentru o transcriere corectă în engleză.

#### Rulează-l

```bash
# Transcrie un scenariu de produs Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Sau încearcă altele:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Puncte cheie JavaScript

| Metodă | Scop |
|--------|-------|
| `FoundryLocalManager.create({ appName })` | Creează singleton-ul managerului |
| `await catalog.getModel(alias)` | Obține un model din catalog |
| `model.download()` / `model.load()` | Descarcă și încarcă modelul Whisper |
| `model.createAudioClient()` | Creează un client audio pentru transcriere |
| `audioClient.settings.language = "en"` | Setează limba transcrierii (necesar pentru rezultate precise) |
| `audioClient.transcribe(path)` | Transcrie un fișier audio, returnează `{ text, duration }` |

</details>

<details>
<summary><h3>Traseul C#</h3></summary>

#### Setup

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Notă:** Traseul C# folosește pachetul `Microsoft.AI.Foundry.Local` care oferă un `AudioClient` integrat prin `model.GetAudioClientAsync()`. Acesta gestionează pipeline-ul complet de transcriere în proces — nu este nevoie de configurare ONNX Runtime separată.

#### Cod pentru Transcriere

Înlocuiește conținutul din `Program.cs`:

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

#### Rulează-l

```bash
# Transcrie un scenariu de produs Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Sau încearcă altele:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Puncte cheie C#

| Metodă | Scop |
|--------|-------|
| `FoundryLocalManager.CreateAsync(config)` | Inițializează Foundry Local cu configurația |
| `catalog.GetModelAsync(alias)` | Obține modelul din catalog |
| `model.DownloadAsync()` | Descarcă modelul Whisper |
| `model.GetAudioClientAsync()` | Obține AudioClient (nu ChatClient!) |
| `audioClient.Settings.Language = "en"` | Setează limba pentru transcriere (necesar pentru rezultate precise) |
| `audioClient.TranscribeAudioAsync(path)` | Transcrie un fișier audio |
| `result.Text` | Textul transcris |


> **C# vs Python/JS:** SDK-ul C# oferă un `AudioClient` încorporat pentru transcriere în proces prin `model.GetAudioClientAsync()`, similar cu SDK-ul JavaScript. Python folosește direct ONNX Runtime pentru inferență pe modelele encoder/decoder cache-uite.

</details>

---

### Exercițiul 4 - Transcrie în lot toate probele Zava

Acum că ai o aplicație de transcriere funcțională, transcrie toate cele cinci fișiere de probă Zava și compară rezultatele.

<details>
<summary><h3>Traseul Python</h3></summary>

Întregul exemplu `python/foundry-local-whisper.py` suportă deja transcriere în lot. Când este rulat fără argumente, transcrie toate fișierele `zava-*.wav` din `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Exemplul folosește `FoundryLocalManager(alias)` pentru inițializare, apoi rulează sesiunile ONNX encoder și decoder pentru fiecare fișier.

</details>

<details>
<summary><h3>Traseul JavaScript</h3></summary>

Întregul exemplu `javascript/foundry-local-whisper.mjs` suportă deja transcriere în lot. Când este rulat fără argumente, transcrie toate fișierele `zava-*.wav` din `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Exemplul folosește `FoundryLocalManager.create()` și `catalog.getModel(alias)` pentru a inițializa SDK-ul, apoi folosește `AudioClient` (cu `settings.language = "en"`) pentru a transcrie fiecare fișier.

</details>

<details>
<summary><h3>Traseul C#</h3></summary>

Întregul exemplu `csharp/WhisperTranscription.cs` suportă deja transcriere în lot. Când este rulat fără un argument specificat de fișier, transcrie toate fișierele `zava-*.wav` din `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Exemplul folosește `FoundryLocalManager.CreateAsync()` și `AudioClient`-ul SDK-ului (cu `Settings.Language = "en"`) pentru transcriere în proces.

</details>

**Ce să urmărești:** Compară rezultatul transcrierii cu textul original din `samples/audio/generate_samples.py`. Cât de precis reușește Whisper să captureze nume de produse precum "Zava ProGrip" și termeni tehnici precum "brushless motor" sau "composite decking"?

---

### Exercițiul 5 - Înțelege principalele modele de cod

Studiu asupra diferențelor între transcrierea Whisper și completările de chat în toate cele trei limbi:

<details>
<summary><b>Python - Diferențe cheie față de chat</b></summary>

```python
# Finalizare chat (Părțile 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Transcriere audio (Această parte):
# Folosește ONNX Runtime direct în loc de clientul OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... buclă decoder autoregresivă ...
print(tokenizer.decode(generated_tokens))
```

**Aspect esențial:** Modelele de chat folosesc API-ul compatibil OpenAI prin `manager.endpoint`. Whisper folosește SDK-ul pentru a localiza fișierele model cache-uite ONNX, apoi rulează inferența direct cu ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Diferențe cheie față de chat</b></summary>

```javascript
// Completarea chatului (Părțile 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Transcrierea audio (Această parte):
// Folosește AudioClient-ul încorporat în SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Setează întotdeauna limba pentru cele mai bune rezultate
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Aspect esențial:** Modelele de chat folosesc API-ul compatibil OpenAI prin `manager.urls[0] + "/v1"`. Transcrierea Whisper folosește `AudioClient`-ul SDK-ului, obținut din `model.createAudioClient()`. Setează `settings.language` pentru a evita rezultate ilizibile din detecția automată.

</details>

<details>
<summary><b>C# - Diferențe cheie față de chat</b></summary>

Abordarea C# folosește `AudioClient` încorporat al SDK-ului pentru transcriere în proces:

**Inițializarea modelului:**

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

**Transcriere:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Aspect esențial:** C# folosește `FoundryLocalManager.CreateAsync()` și obține un `AudioClient` direct — fără necesitatea configurării ONNX Runtime. Setează `Settings.Language` pentru a evita rezultate ilizibile din detecția automată.

</details>

> **Rezumat:** Python folosește Foundry Local SDK pentru managementul modelelor și ONNX Runtime pentru inferență directă pe modelele encoder/decoder. JavaScript și C# folosesc ambele `AudioClient` încorporat în SDK pentru transcriere simplificată — creează clientul, setează limba și apelează `transcribe()` / `TranscribeAudioAsync()`. Setează întotdeauna proprietatea limbii pe AudioClient pentru rezultate precise.

---

### Exercițiul 6 - Experimentează

Încearcă aceste modificări pentru a-ți aprofunda înțelegerea:

1. **Încearcă fișiere audio diferite** - înregistrează-te vorbind cu Windows Voice Recorder, salvează ca WAV și transcrie-l

2. **Compară variantele de model** - dacă ai GPU NVIDIA, încearcă varianta CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Compară viteza de transcriere cu varianta CPU.

3. **Adaugă formatare a rezultatului** - răspunsul JSON poate include:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Construiește un API REST** - învelește codul de transcriere într-un server web:

   | Limbaj | Cadru | Exemplu |
   |---------|--------|---------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` cu `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` cu `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` cu `IFormFile` |

5. **Multi-turn cu transcriere** - combină Whisper cu un agent de chat din Partea 4: transcrie mai întâi audio, apoi transmite textul agentului pentru analiză sau rezumat.

---

## Referință API Audio SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — creează o instanță `AudioClient`
> - `audioClient.settings.language` — setează limba transcrierii (ex. `"en"`)
> - `audioClient.settings.temperature` — controlează aleatorietatea (opțional)
> - `audioClient.transcribe(filePath)` — transcrie un fișier, returnează `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — transmite transcrierea în bucăți prin callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — creează o instanță `OpenAIAudioClient`
> - `audioClient.Settings.Language` — setează limba transcrierii (ex. `"en"`)
> - `audioClient.Settings.Temperature` — controlează aleatorietatea (opțional)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transcrie un fișier, returnează obiect cu `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — returnează `IAsyncEnumerable` cu bucăți de transcriere

> **Sfat:** Setează întotdeauna proprietatea limbii înainte de transcriere. Fără ea, modelul Whisper încearcă detecția automată, care poate produce rezultate ilizibile (un singur caracter de înlocuire în loc de text).

---

## Comparație: Modele Chat vs. Whisper

| Aspect | Modele Chat (Partea 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|--------------------------|------------------|-------------------|
| **Tip sarcină** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Input** | Mesaje text (JSON) | Fișiere audio (WAV/MP3/M4A) | Fișiere audio (WAV/MP3/M4A) |
| **Output** | Text generat (în streaming) | Text transcris (complet) | Text transcris (complet) |
| **Pachet SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Metodă API** | `client.chat.completions.create()` | ONNX Runtime direct | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Setare limbă** | N/A | Prompt tokens decoder | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Da | Nu | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Beneficiu confidențialitate** | Cod/date rămân local | Audio rămâne local | Audio rămâne local |

---

## Concluzii cheie

| Concept | Ce ai învățat |
|---------|---------------|
| **Whisper local** | Transcrierea audio-text rulează complet local, ideal pentru apeluri clienți Zava și recenzii produse pe dispozitiv |
| **SDK AudioClient** | SDK-urile JavaScript și C# oferă un `AudioClient` încorporat care gestionează întreaga pipeline de transcriere într-un singur apel |
| **Setare limbă** | Setează întotdeauna limba pe AudioClient (ex. `"en"`) — fără ea, detecția automată poate produce rezultate ilizibile |
| **Python** | Folosește `foundry-local-sdk` pentru management modele + `onnxruntime` + `transformers` + `librosa` pentru inferență ONNX directă |
| **JavaScript** | Folosește `foundry-local-sdk` cu `model.createAudioClient()` — setează `settings.language`, apoi apelează `transcribe()` |
| **C#** | Folosește `Microsoft.AI.Foundry.Local` cu `model.GetAudioClientAsync()` — setează `Settings.Language`, apoi apelează `TranscribeAudioAsync()` |
| **Suport streaming** | SDK-urile JS și C# oferă și `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` pentru rezultate fragmentate |
| **Optimizat CPU** | Varianta CPU (3.05 GB) funcționează pe orice dispozitiv Windows fără GPU |
| **Confidențialitate first** | Perfect pentru a păstra interacțiunile clienților Zava și datele proprietare ale produselor pe dispozitiv |

---

## Resurse

| Resursă | Link |
|----------|------|
| Documentație Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referință SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Model OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Site Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Pasul următor

Continuă cu [Partea 10: Folosirea modelelor personalizate sau Hugging Face](part10-custom-models.md) pentru a-ți compila propriile modele de la Hugging Face și a le rula prin Foundry Local.