![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 9: Trascrizione Vocale con Whisper e Foundry Local

> **Obiettivo:** Usare il modello OpenAI Whisper in esecuzione locale tramite Foundry Local per trascrivere file audio - completamente sul dispositivo, senza bisogno del cloud.

## Panoramica

Foundry Local non è solo per la generazione di testo; supporta anche modelli di **speech-to-text**. In questo laboratorio utilizzerai il modello **OpenAI Whisper Medium** per trascrivere file audio interamente sul tuo computer. Questo è ideale per scenari come trascrivere chiamate del servizio clienti Zava, registrazioni di recensioni prodotto o sessioni di pianificazione workshop dove i dati audio non devono mai uscire dal dispositivo.


---

## Obiettivi di Apprendimento

Al termine di questo laboratorio sarai in grado di:

- Comprendere il modello di riconoscimento vocale Whisper e le sue capacità
- Scaricare ed eseguire il modello Whisper usando Foundry Local
- Trascrivere file audio usando il Foundry Local SDK in Python, JavaScript e C#
- Costruire un semplice servizio di trascrizione che funziona completamente sul dispositivo
- Comprendere le differenze tra modelli chat/testo e modelli audio in Foundry Local

---

## Prerequisiti

| Requisito | Dettagli |
|-------------|---------|
| **Foundry Local CLI** | Versione **0.8.101 o superiore** (i modelli Whisper sono disponibili da v0.8.101 in poi) |
| **OS** | Windows 10/11 (x64 o ARM64) |
| **Runtime linguaggio** | **Python 3.9+** e/o **Node.js 18+** e/o **.NET 9 SDK** ([Scarica .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Completato** | [Parte 1: Getting Started](part1-getting-started.md), [Parte 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md), e [Parte 3: SDKs and APIs](part3-sdk-and-apis.md) |

> **Nota:** I modelli Whisper devono essere scaricati tramite **SDK** (non tramite CLI). La CLI non supporta l’endpoint di trascrizione audio. Controlla la tua versione con:
> ```bash
> foundry --version
> ```

---

## Concetto: Come Funziona Whisper con Foundry Local

Il modello OpenAI Whisper è un modello di riconoscimento vocale a uso generale addestrato su un ampio dataset di audio diversificato. Quando eseguito tramite Foundry Local:

- Il modello viene eseguito **interamente sulla CPU** - nessuna GPU richiesta
- L’audio non lascia mai il dispositivo - **massima privacy**
- Il Foundry Local SDK si occupa del download del modello e della gestione della cache
- **JavaScript e C#** forniscono un `AudioClient` integrato nell’SDK che gestisce tutta la pipeline di trascrizione — nessuna configurazione manuale di ONNX necessaria
- **Python** usa l’SDK per la gestione del modello e ONNX Runtime per l’inferenza diretta contro i modelli encoder/decoder ONNX

### Come Funziona la Pipeline (JavaScript e C#) — SDK AudioClient

1. **Foundry Local SDK** scarica e mette in cache il modello Whisper
2. `model.createAudioClient()` (JS) o `model.GetAudioClientAsync()` (C#) crea un `AudioClient`
3. `audioClient.transcribe(path)` (JS) o `audioClient.TranscribeAudioAsync(path)` (C#) gestisce internamente tutta la pipeline — preprocessamento audio, encoder, decoder, e decoding dei token
4. `AudioClient` espone una proprietà `settings.language` (impostata a `"en"` per inglese) per guidare la trascrizione accurata

### Come Funziona la Pipeline (Python) — ONNX Runtime

1. **Foundry Local SDK** scarica e mette in cache i file del modello ONNX Whisper
2. **Preprocessamento audio** converte l’audio WAV in uno spettrogramma mel (80 banchi mel x 3000 frame)
3. **Encoder** elabora lo spettrogramma mel e produce hidden states più tensori chiave/valore di cross-attention
4. **Decoder** funziona autoregressivamente, generando un token alla volta finché non produce un token di fine testo
5. **Tokenizer** decodifica gli ID token di output in testo leggibile

### Varianti del Modello Whisper

| Alias | ID Modello | Dispositivo | Dimensione | Descrizione |
|-------|------------|-------------|------------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Accelerato GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Ottimizzato CPU (consigliato per la maggior parte dei dispositivi) |

> **Nota:** A differenza dei modelli chat che sono elencati di default, i modelli Whisper sono categorizzati sotto il task `automatic-speech-recognition`. Usa `foundry model info whisper-medium` per vedere i dettagli.

---

## Esercizi del Laboratorio

### Esercizio 0 - Ottieni i File Audio di Esempio

Questo laboratorio include file WAV preconfigurati basati su scenari di prodotto Zava DIY. Generali con lo script incluso:

```bash
# Dalla radice del repository - crea e attiva prima un .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Questo crea sei file WAV in `samples/audio/`:

| File | Scenario |
|------|----------|
| `zava-customer-inquiry.wav` | Cliente che chiede informazioni sulla **Trapano senza fili Zava ProGrip** |
| `zava-product-review.wav` | Cliente che recensisce la **Vernice Interna Zava UltraSmooth** |
| `zava-support-call.wav` | Chiamata di supporto sul **Baule Utensili Zava TitanLock** |
| `zava-project-planning.wav` | Fai-da-te che pianifica un decking con **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Tour di un’officina che usa **tutti e cinque i prodotti Zava** |
| `zava-full-project-walkthrough.wav` | Tour esteso di ristrutturazione garage con **tutti i prodotti Zava** (~4 min, per test audio lungo) |

> **Consiglio:** Puoi anche usare i tuoi file WAV/MP3/M4A, o registrarti con Registratore Vocale di Windows.

---

### Esercizio 1 - Scarica il Modello Whisper Usando l'SDK

A causa di incompatibilità della CLI con i modelli Whisper nelle nuove versioni di Foundry Local, usa l’**SDK** per scaricare e caricare il modello. Scegli il tuo linguaggio:

<details>
<summary><b>🐍 Python</b></summary>

**Installa l’SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Avvia il servizio
manager = FoundryLocalManager()
manager.start_service()

# Controlla le informazioni del catalogo
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Controlla se già memorizzato nella cache
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Carica il modello in memoria
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Salvalo come `download_whisper.py` ed esegui:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Installa l’SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Crea il manager e avvia il servizio
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Ottieni il modello dal catalogo
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

// Carica il modello in memoria
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Salvalo come `download-whisper.mjs` ed esegui:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Installa l’SDK:**
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

> **Perché usare l'SDK invece della CLI?** La CLI Foundry Local non supporta il download o il serving diretto dei modelli Whisper. L’SDK offre un modo affidabile per scaricare e gestire i modelli audio programmaticamente. Gli SDK JavaScript e C# includono un `AudioClient` integrato che gestisce tutta la pipeline di trascrizione. Python utilizza ONNX Runtime per l’inferenza diretta sui file del modello in cache.

---

### Esercizio 2 - Comprendere l’SDK Whisper

La trascrizione con Whisper utilizza approcci differenti a seconda del linguaggio. **JavaScript e C#** forniscono un `AudioClient` integrato nell’SDK Foundry Local che gestisce la pipeline completa (preprocessamento audio, encoder, decoder, decoding dei token) con una singola chiamata di metodo. **Python** usa l’SDK Foundry Local per la gestione del modello e ONNX Runtime per l’inferenza diretta sui modelli encoder/decoder ONNX.

| Componente | Python | JavaScript | C# |
|------------|--------|------------|----|
| **Pacchetti SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Gestione modello** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **Estrazione caratteristiche** | `WhisperFeatureExtractor` + `librosa` | Gestito da `AudioClient` SDK | Gestito da `AudioClient` SDK |
| **Inferenza** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Decoding token** | `WhisperTokenizer` | Gestito da `AudioClient` SDK | Gestito da `AudioClient` SDK |
| **Impostazione lingua** | Impostata con `forced_ids` nei token del decoder | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | Percorso file WAV | Percorso file WAV | Percorso file WAV |
| **Output** | Testo decodificato | `result.text` | `result.Text` |

> **Importante:** Imposta sempre la lingua su `AudioClient` (es. `"en"` per l’inglese). Senza un’impostazione lingua esplicita, il modello potrebbe produrre output confusi mentre tenta di rilevare automaticamente la lingua.

> **Modelli d’uso SDK:** Python usa `FoundryLocalManager(alias)` per il bootstrap, poi `get_cache_location()` per trovare i file ONNX del modello. JavaScript e C# usano il `AudioClient` integrato nell’SDK — ottenuto tramite `model.createAudioClient()` (JS) o `model.GetAudioClientAsync()` (C#) — che gestisce tutta la pipeline di trascrizione. Vedi [Parte 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md) per dettagli completi.

---

### Esercizio 3 - Costruisci una Semplice Applicazione di Trascrizione

Scegli il tuo linguaggio e costruisci un’applicazione minima che trascrive un file audio.

> **Formati audio supportati:** WAV, MP3, M4A. Per risultati migliori, usa file WAV con frequenza di campionamento 16kHz.

<details>
<summary><h3>Corso Python</h3></summary>

#### Setup

```bash
cd python
python -m venv venv

# Attiva l'ambiente virtuale:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Codice di Trascrizione

Crea un file `foundry-local-whisper.py`:

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

# Passo 1: Bootstrap - avvia il servizio, scarica e carica il modello
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Costruisci il percorso ai file modello ONNX memorizzati nella cache
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Passo 2: Carica le sessioni ONNX e l'estrattore di caratteristiche
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

# Passo 3: Estrai le caratteristiche del mel spettrogramma
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Passo 4: Esegui l'encoder
enc_out = encoder.run(None, {"audio_features": input_features})
# La prima uscita sono gli stati nascosti; le restanti sono le coppie KV di cross-attention
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Passo 5: Decodifica autoregressiva
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, trascrivi, nessun timestamp
input_ids = np.array([initial_tokens], dtype=np.int32)

# Cache KV di self-attention vuota
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

    if next_token == 50257:  # fine del testo
        break
    generated.append(next_token)

    # Aggiorna la cache KV di self-attention
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Esegui

```bash
# Trascrivi uno scenario di prodotto Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Oppure prova altri:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Punti Chiave Python

| Metodo | Scopo |
|--------|-------|
| `FoundryLocalManager(alias)` | Bootstrap: avvia servizio, scarica e carica modello |
| `manager.get_cache_location()` | Ottieni percorso dei file ONNX in cache |
| `WhisperFeatureExtractor.from_pretrained()` | Carica l’estrattore di spettrogrammi mel |
| `ort.InferenceSession()` | Crea sessioni ONNX Runtime per encoder e decoder |
| `tokenizer.decode()` | Converti ID token in testo |

</details>

<details>
<summary><h3>Corso JavaScript</h3></summary>

#### Setup

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Codice di Trascrizione

Crea un file `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Passo 1: Bootstrap - creare il manager, avviare il servizio e caricare il modello
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

// Passo 2: Creare un client audio e trascrivere
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Pulizia
await model.unload();
```

> **Nota:** Il Foundry Local SDK fornisce un `AudioClient` integrato tramite `model.createAudioClient()` che gestisce internamente tutta la pipeline di inferenza ONNX — non serve importare `onnxruntime-node`. Imposta sempre `audioClient.settings.language = "en"` per garantire trascrizioni accurate in inglese.

#### Esegui

```bash
# Trascrivi uno scenario di prodotto Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Oppure prova altri:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Punti Chiave JavaScript

| Metodo | Scopo |
|--------|-------|
| `FoundryLocalManager.create({ appName })` | Crea il singleton manager |
| `await catalog.getModel(alias)` | Ottieni un modello dal catalogo |
| `model.download()` / `model.load()` | Scarica e carica il modello Whisper |
| `model.createAudioClient()` | Crea un client audio per la trascrizione |
| `audioClient.settings.language = "en"` | Imposta la lingua della trascrizione (necessario per output accurato) |
| `audioClient.transcribe(path)` | Trascrivi un file audio, restituisce `{ text, duration }` |

</details>

<details>
<summary><h3>Corso C#</h3></summary>

#### Setup

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Nota:** Il corso C# usa il pacchetto `Microsoft.AI.Foundry.Local` che fornisce un `AudioClient` integrato tramite `model.GetAudioClientAsync()`. Questo gestisce tutta la pipeline di trascrizione in-process — non serve configurare separatamente ONNX Runtime.

#### Codice di Trascrizione

Sostituisci il contenuto di `Program.cs`:

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

#### Esegui

```bash
# Trascrivi uno scenario di prodotto Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Oppure prova altri:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Punti Chiave C#

| Metodo | Scopo |
|--------|-------|
| `FoundryLocalManager.CreateAsync(config)` | Inizializza Foundry Local con configurazione |
| `catalog.GetModelAsync(alias)` | Ottieni modello dal catalogo |
| `model.DownloadAsync()` | Scarica il modello Whisper |
| `model.GetAudioClientAsync()` | Ottieni l'AudioClient (non ChatClient!) |
| `audioClient.Settings.Language = "en"` | Imposta la lingua della trascrizione (necessario per output accurato) |
| `audioClient.TranscribeAudioAsync(path)` | Trascrivi un file audio |
| `result.Text` | Testo trascritto |


> **C# vs Python/JS:** L’SDK C# fornisce un `AudioClient` integrato per la trascrizione in-process tramite `model.GetAudioClientAsync()`, simile all’SDK JavaScript. Python usa direttamente ONNX Runtime per l’inferenza sui modelli encoder/decoder memorizzati nella cache.

</details>

---

### Esercizio 4 - Trascrivere in batch tutti i campioni Zava

Ora che hai un’app di trascrizione funzionante, trascrivi tutti e cinque i file campione Zava e confronta i risultati.

<details>
<summary><h3>Traccia Python</h3></summary>

Il campione completo `python/foundry-local-whisper.py` supporta già la trascrizione in batch. Eseguito senza argomenti, trascrive tutti i file `zava-*.wav` in `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Il campione usa `FoundryLocalManager(alias)` per l’avvio, quindi esegue le sessioni ONNX di encoder e decoder per ogni file.

</details>

<details>
<summary><h3>Traccia JavaScript</h3></summary>

Il campione completo `javascript/foundry-local-whisper.mjs` supporta già la trascrizione in batch. Eseguito senza argomenti, trascrive tutti i file `zava-*.wav` in `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Il campione usa `FoundryLocalManager.create()` e `catalog.getModel(alias)` per inizializzare l’SDK, quindi utilizza l’`AudioClient` (con `settings.language = "en"`) per trascrivere ogni file.

</details>

<details>
<summary><h3>Traccia C#</h3></summary>

Il campione completo `csharp/WhisperTranscription.cs` supporta già la trascrizione in batch. Eseguito senza un argomento file specifico, trascrive tutti i file `zava-*.wav` in `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Il campione usa `FoundryLocalManager.CreateAsync()` e l’`AudioClient` dell’SDK (con `Settings.Language = "en"`) per la trascrizione in-process.

</details>

**Cosa osservare:** Confronta il risultato della trascrizione con il testo originale in `samples/audio/generate_samples.py`. Quanto accuratamente Whisper cattura nomi di prodotto come "Zava ProGrip" e termini tecnici come "brushless motor" o "composite decking"?

---

### Esercizio 5 - Comprendere i modelli chiave del codice

Studia come la trascrizione Whisper differisce dalle chat completion in tutti e tre i linguaggi:

<details>
<summary><b>Python - Differenze chiave dalle chat</b></summary>

```python
# Completamento chat (Parti 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Trascrizione audio (Questa parte):
# Utilizza ONNX Runtime direttamente invece del client OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... ciclo decoder autoregressivo ...
print(tokenizer.decode(generated_tokens))
```

**Insight chiave:** I modelli chat usano l’API compatibile OpenAI tramite `manager.endpoint`. Whisper usa l’SDK per localizzare i file modello ONNX memorizzati nella cache, poi esegue direttamente l’inferenza con ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Differenze chiave dalle chat</b></summary>

```javascript
// Completamento della chat (Parti 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Trascrizione audio (Questa parte):
// Utilizza il AudioClient integrato nell'SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Impostare sempre la lingua per risultati ottimali
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Insight chiave:** I modelli chat usano l’API compatibile OpenAI tramite `manager.urls[0] + "/v1"`. La trascrizione Whisper usa l’`AudioClient` dell’SDK, ottenuto da `model.createAudioClient()`. Imposta `settings.language` per evitare output confusi da auto-rilevamento.

</details>

<details>
<summary><b>C# - Differenze chiave dalle chat</b></summary>

L’approccio C# usa l’`AudioClient` integrato nell’SDK per la trascrizione in-process:

**Inizializzazione del modello:**

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

**Trascrizione:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Insight chiave:** C# usa `FoundryLocalManager.CreateAsync()` e ottiene direttamente un `AudioClient` — non serve configurare ONNX Runtime. Imposta `Settings.Language` per evitare output confusi da auto-rilevamento.

</details>

> **Riassunto:** Python usa Foundry Local SDK per la gestione del modello e ONNX Runtime per inferenza diretta sugli encoder/decoder. JavaScript e C# usano entrambi l’`AudioClient` integrato dell’SDK per una trascrizione semplificata — crea il client, imposta la lingua e chiama `transcribe()` / `TranscribeAudioAsync()`. Imposta sempre la proprietà lingua su AudioClient per risultati accurati.

---

### Esercizio 6 - Sperimenta

Prova queste modifiche per approfondire la comprensione:

1. **Prova file audio diversi** - registra te stesso che parli usando Windows Voice Recorder, salva in WAV e trascrivilo

2. **Confronta varianti del modello** - se hai una GPU NVIDIA, prova la variante CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Confronta la velocità di trascrizione rispetto alla variante CPU.

3. **Aggiungi formattazione output** - la risposta JSON può includere:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Costruisci un’API REST** - incapsula il codice di trascrizione in un server web:

   | Linguaggio | Framework | Esempio |
   |------------|-----------|---------|
   | Python     | FastAPI   | `@app.post("/v1/audio/transcriptions")` con `UploadFile` |
   | JavaScript | Express.js| `app.post("/v1/audio/transcriptions")` con `multer`       |
   | C#         | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` con `IFormFile` |

5. **Dialogo multi-turn con trascrizione** - combina Whisper con un agente chat di Parte 4: trascrivi prima l’audio, poi passa il testo all’agente per analisi o riassunti.

---

## Riferimento API Audio SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — crea un’istanza `AudioClient`
> - `audioClient.settings.language` — imposta la lingua della trascrizione (es. `"en"`)
> - `audioClient.settings.temperature` — controlla casualità (opzionale)
> - `audioClient.transcribe(filePath)` — trascrive un file, restituisce `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — trascrive a streaming a blocchi via callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — crea un’istanza `OpenAIAudioClient`
> - `audioClient.Settings.Language` — imposta la lingua della trascrizione (es. `"en"`)
> - `audioClient.Settings.Temperature` — controlla casualità (opzionale)
> - `await audioClient.TranscribeAudioAsync(filePath)` — trascrive un file, restituisce un oggetto con `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — restituisce `IAsyncEnumerable` di blocchi di trascrizione

> **Suggerimento:** Imposta sempre la lingua prima di trascrivere. Senza questa impostazione, il modello Whisper tenta l’auto-rilevamento, che può produrre output confusi (un unico carattere di sostituzione invece del testo).

---

## Confronto: Modelli Chat vs. Whisper

| Aspetto              | Modelli Chat (Parti 3-7) | Whisper - Python       | Whisper - JS / C#                    |
|----------------------|--------------------------|-----------------------|------------------------------------|
| **Tipo di attività**  | `chat`                   | `automatic-speech-recognition` | `automatic-speech-recognition`      |
| **Input**             | Messaggi di testo (JSON) | File audio (WAV/MP3/M4A) | File audio (WAV/MP3/M4A)             |
| **Output**            | Testo generato (streaming)| Testo trascritto (completo) | Testo trascritto (completo)           |
| **Pacchetto SDK**     | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Metodo API**        | `client.chat.completions.create()` | ONNX Runtime diretto | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Impostazione lingua** | N/A                     | Token prompt decoder   | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming**         | Sì                       | No                     | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Vantaggio privacy** | Il codice/dati restano locali | I dati audio restano locali | I dati audio restano locali           |

---

## Punti chiave da ricordare

| Concetto              | Cosa hai imparato                                  |
|-----------------------|---------------------------------------------------|
| **Whisper on-device**  | Il riconoscimento vocale funziona interamente in locale, ideale per trascrivere chiamate e recensioni clienti Zava on-device |
| **SDK AudioClient**    | Gli SDK JavaScript e C# forniscono un `AudioClient` integrato che gestisce tutta la pipeline di trascrizione in una chiamata |
| **Impostazione lingua**| Imposta sempre la lingua di AudioClient (es. `"en"`) — senza, l’auto-rilevamento può produrre output confusi |
| **Python**             | Usa `foundry-local-sdk` per la gestione modello + `onnxruntime` + `transformers` + `librosa` per inferenza ONNX diretta |
| **JavaScript**         | Usa `foundry-local-sdk` con `model.createAudioClient()` — imposta `settings.language`, poi chiama `transcribe()` |
| **C#**                 | Usa `Microsoft.AI.Foundry.Local` con `model.GetAudioClientAsync()` — imposta `Settings.Language`, poi chiama `TranscribeAudioAsync()` |
| **Supporto streaming** | Gli SDK JS e C# offrono anche `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` per output a blocchi |
| **Ottimizzato CPU**    | La variante CPU (3.05 GB) funziona su qualsiasi dispositivo Windows senza GPU |
| **Privacy-first**      | Perfetto per mantenere le interazioni clienti e dati di prodotto proprietari on-device |

---

## Risorse

| Risorsa              | Link                                                                                   |
|----------------------|----------------------------------------------------------------------------------------|
| Documentazione Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Riferimento SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Modello OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper)                        |
| Sito Foundry Local    | [foundrylocal.ai](https://foundrylocal.ai)                                            |

---

## Prossimo passo

Continua con [Parte 10: Usare modelli Custom o Hugging Face](part10-custom-models.md) per compilare i tuoi modelli da Hugging Face ed eseguirli su Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:  
Questo documento è stato tradotto utilizzando il servizio di traduzione AI [Co-op Translator](https://github.com/Azure/co-op-translator). Sebbene ci impegniamo per l'accuratezza, si prega di considerare che le traduzioni automatizzate possono contenere errori o imprecisioni. Il documento originale nella sua lingua nativa deve essere considerato la fonte autorevole. Per informazioni critiche, si raccomanda una traduzione professionale effettuata da un essere umano. Non ci assumiamo alcuna responsabilità per eventuali malintesi o interpretazioni errate derivanti dall'uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->