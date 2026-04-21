![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 9. rész: Hangátírás Whisperrel és Foundry Locallal

> **Cél:** Az OpenAI Whisper modell helyi futtatásával a Foundry Local segítségével hangfájlokat átírni – teljesen eszközön belül, felhő nélkül.

## Áttekintés

A Foundry Local nem csak szöveggenerálásra való; támogatja a **beszédfelismerő** modelleket is. Ebben a laborban az **OpenAI Whisper Medium** modellt használod, hogy a hangfájlokat teljes egészében a saját gépeden írd át. Ez ideális olyan helyzetekben, mint például a Zava ügyfélszolgálati hívások, termékértékelések vagy műhelytervezési megbeszélések átírása, ahol a hangadatoknak soha nem szabad elhagyniuk az eszközödet.

---

## Tanulási célok

A labor végére képes leszel:

- Megérteni a Whisper beszédfelismerő modellt és annak képességeit
- Letölteni és futtatni a Whisper modellt a Foundry Local segítségével
- Hangfájlokat átírni a Foundry Local SDK-val Pythonban, JavaScriptben és C#-ban
- Egyszerű átíró szolgáltatást építeni, amely teljesen helyileg fut
- Megérteni a chat/szöveg modellek és a hangmodellek közötti különbségeket a Foundry Localban

---

## Előfeltételek

| Követelmény | Részletek |
|-------------|-----------|
| **Foundry Local CLI** | **0.8.101 vagy újabb verzió** (Whisper modellek a 0.8.101-es verziótól érhetők el) |
| **Operációs rendszer** | Windows 10/11 (x64 vagy ARM64) |
| **Nyelvi futtatókörnyezet** | **Python 3.9+** és/vagy **Node.js 18+** és/vagy **.NET 9 SDK** ([.NET letöltése](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Elvégzett** | [1. rész: Első lépések](part1-getting-started.md), [2. rész: Foundry Local SDK részletek](part2-foundry-local-sdk.md), és [3. rész: SDK-k és API-k](part3-sdk-and-apis.md) |

> **Megjegyzés:** A Whisper modelleket kizárólag az **SDK**-n keresztül lehet letölteni (nem a CLI-n). A CLI nem támogatja a hangátírási végpontot. Verzió ellenőrzéshez:
> ```bash
> foundry --version
> ```

---

## Fogalom: Hogyan működik a Whisper a Foundry Locallal

Az OpenAI Whisper modell egy általános célú beszédfelismerő modell, amelyet nagy, sokféle audió adat alapján tanítottak. Foundry Localon futtatva:

- A modell teljes egészében a **CPU-don fut** – nincs szükség GPU-ra
- A hanganyag soha nem hagyja el az eszközöd – **teljes adatvédelem**
- A Foundry Local SDK kezeli a modell letöltését és gyorsítótárazását
- **JavaScript és C#** beépített `AudioClient`-et biztosít az SDK-ban, amely kezeli az egész átírási folyamatot — kézi ONNX beállítás nem szükséges
- **Python** az SDK-t használja a modellkezeléshez és ONNX Runtime-ot közvetlen kiértékeléshez az encoder/decoder ONNX modellek ellen

### Hogyan működik a folyamat (JavaScript és C#) — SDK AudioClient

1. A **Foundry Local SDK** letölti és gyorsítótárazza a Whisper modellt
2. A `model.createAudioClient()` (JS) vagy a `model.GetAudioClientAsync()` (C#) létrehozza az `AudioClient` példányt
3. Az `audioClient.transcribe(path)` (JS) vagy az `audioClient.TranscribeAudioAsync(path)` (C#) a teljes folyamatot belül kezeli — hang előfeldolgozás, encoder, decoder és token dekódolás
4. Az `AudioClient` rendelkezik `settings.language` beállítással (pl. `"en"` angolhoz) az pontos átíráshoz

### Hogyan működik a folyamat (Python) — ONNX Runtime

1. A **Foundry Local SDK** letölti és gyorsítótárazza a Whisper ONNX modell fájlokat
2. **Hang előfeldolgozás**: WAV hangot mel-spektrogrammá (80 mel sáv x 3000 keret) konvertálja
3. **Encoder** feldolgozza a mel-spektrogramot, és előállít rejtett állapotokat plusz kereszt figyelmi kulcs/érték tenzorokat
4. **Decoder** autoregresszívan fut, tokenenként generál, amíg el nem éri a szöveg vége tokenjét
5. **Tokenizáló** visszafordítja a kimeneti tokenazonosítókat olvasható szöveggé

### Whisper modell változatok

| Alias | Modell azonosító | Eszköz | Méret | Leírás |
|-------|------------------|--------|-------|--------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU gyorsított (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU optimalizált (ajánlott legtöbb eszközhöz) |

> **Megjegyzés:** A chat modellektől eltérően, amelyek alapértelmezés szerint listázódnak, a Whisper modellek az `automatic-speech-recognition` feladathoz vannak besorolva. Részletek megtekintéséhez használd a `foundry model info whisper-medium` parancsot.

---

## Laborfeladatok

### 0. gyakorlat - Minta hangfájlok beszerzése

Ez a labor előre elkészített WAV fájlokat tartalmaz, amelyek a Zava barkács termékszituációk alapján készültek. Hozd létre őket a mellékelt szkript használatával:

```bash
# A repó gyökérkönyvtárából - először hozz létre és aktiválj egy .venv-et
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Ez hat darab WAV fájlt hoz létre a `samples/audio/` mappában:

| Fájl | Forgatókönyv |
|------|--------------|
| `zava-customer-inquiry.wav` | Ügyfél érdeklődik a **Zava ProGrip Akkus Fúró** iránt |
| `zava-product-review.wav` | Ügyfél értékelést ad a **Zava UltraSmooth Belső Festékről** |
| `zava-support-call.wav` | Támogatási hívás a **Zava TitanLock Szerszámszekrényről** |
| `zava-project-planning.wav` | Barkácsoló tervez egy teraszt a **Zava EcoBoard Kompozit Teraszburkolatból** |
| `zava-workshop-setup.wav` | Bemutató egy műhelyről az **öt Zava termék használatával** |
| `zava-full-project-walkthrough.wav` | Hosszabb garázsfelújítás bemutató az **összes Zava termékkel** (~4 perc, hosszabb hanganyag teszthez) |

> **Tipp:** Saját WAV/MP3/M4A fájlokat is használhatsz, vagy felveheted a hangodat a Windows Hangrögzítővel.

---

### 1. gyakorlat - Whisper modell letöltése az SDK segítségével

Mivel az újabb Foundry Local verziókban a CLI nem kompatibilis a Whisper modellekkel, használd az **SDK-t** a modell letöltéséhez és betöltéséhez. Válaszd ki a nyelvet:

<details>
<summary><b>🐍 Python</b></summary>

**SDK telepítése:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Indítsa el a szolgáltatást
manager = FoundryLocalManager()
manager.start_service()

# Ellenőrizze a katalógus információkat
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Ellenőrizze, hogy már cache-elve van-e
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Töltse be a modellt a memóriába
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Mentés `download_whisper.py` néven és futtatás:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK telepítése:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Hozza létre a menedzsert és indítsa el a szolgáltatást
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Szerezze be a modellt a katalógusból
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

// Töltse be a modellt a memóriába
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Mentés `download-whisper.mjs` néven és futtatás:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK telepítése:**
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

> **Miért SDK és nem CLI?** A Foundry Local CLI nem támogatja a Whisper modellek közvetlen letöltését vagy kiszolgálását. Az SDK megbízható módot nyújt az audio modellek programozott kezelésére. A JavaScript és C# SDK-k tartalmaznak egy beépített `AudioClient`-et, amely az egész átírási folyamatot kezeli. A Python az ONNX Runtime-t használja a gyorsítótárazott modellfájlok közvetlen kiértékeléséhez.

---

### 2. gyakorlat - Whisper SDK megértése

A Whisper átírás különböző módokon működik a választott nyelvtől függően. A **JavaScript és a C#** beépített `AudioClient`-et biztosítanak a Foundry Local SDK-ban az egész folyamatra (hang előfeldolgozás, encoder, decoder, token dekódolás) egyetlen hívással. A **Python** az SDK-t használja modellkezeléshez és ONNX Runtime-ot az encoder/decoder ONNX modellekkel való közvetlen kiértékeléshez.

| Összetevő | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK csomagok** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Modell kezelés** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalógus |
| **Jellemző kinyerés** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` kezeli | SDK `AudioClient` kezeli |
| **Kiértékelés** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Token dekódolás** | `WhisperTokenizer` | SDK `AudioClient` kezeli | SDK `AudioClient` kezeli |
| **Nyelv beállítása** | `forced_ids` a decoder tokenekben | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Bemenet** | WAV fájl útvonal | WAV fájl útvonal | WAV fájl útvonal |
| **Kimenet** | Dekódolt szöveg string | `result.text` | `result.Text` |

> **Fontos:** Mindig állítsd be a nyelvet az `AudioClient`-en (pl. `"en"` angolhoz). Nyelv megadása nélkül a modell hibás szöveget generálhat, mert próbálja automatikusan felismerni a nyelvet.

> **SDK minták:** A Python a `FoundryLocalManager(alias)`-t használja az induláshoz, majd a `get_cache_location()`-t az ONNX modellfájlok megtalálásához. A JavaScript és C# az SDK beépített `AudioClient`-jét használja — `model.createAudioClient()` (JS) vagy `model.GetAudioClientAsync()` (C#), amely kezeli az egész átírási folyamatot. Teljes részletekért lásd a [2. rész: Foundry Local SDK részletek](part2-foundry-local-sdk.md) dokumentációt.

---

### 3. gyakorlat - Egyszerű átíró alkalmazás építése

Válaszd ki a nyelvi szálat, és építs egy minimális alkalmazást, amely átír egy hangfájlt.

> **Támogatott hangformátumok:** WAV, MP3, M4A. A legjobb eredményhez használj 16 kHz-es mintavételű WAV fájlokat.

<details>
<summary><h3>Python szál</h3></summary>

#### Beállítás

```bash
cd python
python -m venv venv

# Aktiválja a virtuális környezetet:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Átíró kód

Készíts egy `foundry-local-whisper.py` nevű fájlt:

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

# 1. lépés: Bootstrap - elindítja a szolgáltatást, letölti, és betölti a modellt
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Útvonal építése a gyorsítótárazott ONNX modell fájlokhoz
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# 2. lépés: ONNX munkamenetek és jellemző kinyerő betöltése
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

# 3. lépés: Mel spektrumgram jellemzők kinyerése
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# 4. lépés: Kódoló futtatása
enc_out = encoder.run(None, {"audio_features": input_features})
# Az első kimenet a rejtett állapotok; a többi kereszt-figyelem KV pár
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# 5. lépés: Autoregresszív dekódolás
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, átírás, időbélyegek nélkül
input_ids = np.array([initial_tokens], dtype=np.int32)

# Üres önfigyelem KV gyorsítótár
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

    if next_token == 50257:  # szöveg vége
        break
    generated.append(next_token)

    # Önfigyelem KV gyorsítótár frissítése
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Futtatás

```bash
# Egy Zava termék szcenárió leírása
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Vagy próbáljon másokat:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Fontos Python pontok

| Módszer | Cél |
|---------|-----|
| `FoundryLocalManager(alias)` | Indítás: szolgáltatás indítása, modell letöltése és betöltése |
| `manager.get_cache_location()` | Az ONNX modell fájlok gyorsítótárának elérési útját adja vissza |
| `WhisperFeatureExtractor.from_pretrained()` | Betölti a mel spektrogram jellemző kinyerőt |
| `ort.InferenceSession()` | ONNX Runtime munkamenetek létrehozása encoder és decoder számára |
| `tokenizer.decode()` | Kimeneti token ID-k visszaalakítása szöveggé |

</details>

<details>
<summary><h3>JavaScript szál</h3></summary>

#### Beállítás

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Átíró kód

Készíts egy `foundry-local-whisper.mjs` nevű fájlt:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// 1. lépés: Bootstrap - menedzser létrehozása, szolgáltatás indítása és a modell betöltése
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

// 2. lépés: Hozzon létre egy audio klienst és transzkripáljon
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Takarítás
await model.unload();
```

> **Megjegyzés:** A Foundry Local SDK beépített `AudioClient`-et biztosít a `model.createAudioClient()` segítségével, amely belül kezeli az egész ONNX kiértékelési folyamatot — az `onnxruntime-node` importja nem szükséges. Mindig állítsd be, hogy `audioClient.settings.language = "en"`, hogy pontos angol átírást kapj.

#### Futtatás

```bash
# Egy Zava termékszcenárió átirata
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Vagy próbáljon ki másokat:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Fontos JavaScript pontok

| Módszer | Cél |
|---------|-----|
| `FoundryLocalManager.create({ appName })` | Management singleton létrehozása |
| `await catalog.getModel(alias)` | Modell lekérése a katalógusból |
| `model.download()` / `model.load()` | Whisper modell letöltése és betöltése |
| `model.createAudioClient()` | Átírásra audio kliens létrehozása |
| `audioClient.settings.language = "en"` | Átírási nyelv beállítása (pontos eredményhez kötelező) |
| `audioClient.transcribe(path)` | Hangfájl átírása, visszatér `{ text, duration }` objektummal |

</details>

<details>
<summary><h3>C# szál</h3></summary>

#### Beállítás

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Megjegyzés:** A C# szál a `Microsoft.AI.Foundry.Local` csomagot használja, amely beépített `AudioClient`-et biztosít a `model.GetAudioClientAsync()` metóduson keresztül. Ez az egész átírási folyamatot kezeli egy folyamatban — külön ONNX Runtime beállítás nem szükséges.

#### Átíró kód

Cseréld le a `Program.cs` tartalmát:

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

#### Futtatás

```bash
# Egy Zava termék forgatókönyv átirata
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Vagy próbálj ki másokat:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Fontos C# pontok

| Módszer | Cél |
|---------|-----|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Local inicializálása konfigurációval |
| `catalog.GetModelAsync(alias)` | Modell lekérése a katalógusból |
| `model.DownloadAsync()` | Whisper modell letöltése |
| `model.GetAudioClientAsync()` | AudioClient lekérése (nem ChatClient!) |
| `audioClient.Settings.Language = "en"` | Átírási nyelv beállítása (pontos eredményhez kötelező) |
| `audioClient.TranscribeAudioAsync(path)` | Hangfájl átírása |
| `result.Text` | Az átírt szöveg |
> **C# vs Python/JS:** A C# SDK beépített `AudioClient`-et biztosít az azonnali átiratozáshoz a `model.GetAudioClientAsync()` segítségével, hasonlóan a JavaScript SDK-hoz. A Python közvetlenül az ONNX Runtime-ot használja az encoder/dekoder modellek gyorsítótárból történő kiértékeléséhez.

</details>

---

### 4. gyakorlat – Az összes Zava minta együttes átiratozása

Most, hogy van egy működő átiratozó alkalmazásod, írd át mind az öt Zava mintafájlt, és hasonlítsd össze az eredményeket.

<details>
<summary><h3>Python ág</h3></summary>

A teljes `python/foundry-local-whisper.py` mintakód már támogatja az együttes átiratozást. Argumentum nélkül futtatva az összes `zava-*.wav` fájlt átiratja a `samples/audio/` könyvtárban:

```bash
cd python
python foundry-local-whisper.py
```

A minta a `FoundryLocalManager(alias)` használatával indít, majd minden fájlra futtatja az encoder és decoder ONNX szekvenciákat.

</details>

<details>
<summary><h3>JavaScript ág</h3></summary>

A teljes `javascript/foundry-local-whisper.mjs` mintakód már támogatja az együttes átiratozást. Argumentum nélkül futtatva az összes `zava-*.wav` fájlt átiratja a `samples/audio/` könyvtárban:

```bash
cd javascript
node foundry-local-whisper.mjs
```

A minta a `FoundryLocalManager.create()` és `catalog.getModel(alias)` segítségével inicializálja az SDK-t, majd az `AudioClient`-et használja (`settings.language = "en"` beállítással) az egyes fájlok átiratozására.

</details>

<details>
<summary><h3>C# ág</h3></summary>

A teljes `csharp/WhisperTranscription.cs` mintakód már támogatja az együttes átiratozást. A konkrét fájl argumentum nélkül futtatva az összes `zava-*.wav` fájlt átiratja a `samples/audio/` könyvtárban:

```bash
cd csharp
dotnet run whisper
```

A minta a `FoundryLocalManager.CreateAsync()` és az SDK `AudioClient`-jét használja (`Settings.Language = "en"` beállítással) az azonnali átiratozáshoz.

</details>

**Mit figyelj:** Hasonlítsd össze az átiratot a `samples/audio/generate_samples.py` eredeti szövegével. Mennyire pontosan rögzíti a Whisper az olyan termékneveket, mint a „Zava ProGrip”, valamint a műszaki kifejezéseket, például a „brushless motor” vagy „composite decking”?

---

### 5. gyakorlat – A kulcsfontosságú kódminták megértése

Tanulmányozd, hogyan különbözik a Whisper átirat az összes három nyelven található chat-kiegészítésektől:

<details>
<summary><b>Python – Fontos különbségek a chattel szemben</b></summary>

```python
# Csevegés befejezése (2-6. részek):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Hangátírás (ez a rész):
# Közvetlenül az ONNX Runtime-ot használja az OpenAI kliens helyett
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregresszív dekóder ciklus ...
print(tokenizer.decode(generated_tokens))
```

**Fontos megállapítás:** A chat-modellek az OpenAI-kompatibilis API-t használják a `manager.endpoint`-en keresztül. A Whisper az SDK-t használja a helyileg tárolt ONNX modellfájlok megtalálásához, majd az ONNX Runtime-t közvetlenül futtatja.

</details>

<details>
<summary><b>JavaScript – Fontos különbségek a chattel szemben</b></summary>

```javascript
// Csevegés kiegészítés (2-6. részek):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Hang átirat (Ez a rész):
// Az SDK beépített AudioClient-jét használja
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // A legjobb eredmény érdekében mindig állítsa be a nyelvet
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Fontos megállapítás:** A chat-modellek az OpenAI-kompatibilis API-t használják a `manager.urls[0] + "/v1"` címen keresztül. A Whisper átirat az SDK beépített `AudioClient`-jét használja, amely a `model.createAudioClient()`-val érhető el. A `settings.language` beállítása nélkül auto-detekció miatt rossz eredmény jöhet létre.

</details>

<details>
<summary><b>C# – Fontos különbségek a chattel szemben</b></summary>

A C# megközelítés az SDK beépített `AudioClient`-jét használja az azonnali átiratozáshoz:

**Modell inicializálása:**

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

**Átiratozás:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Fontos megállapítás:** C# a `FoundryLocalManager.CreateAsync()`-t és az `AudioClient`-et használja közvetlenül – nincs szükség külön ONNX Runtime felállításra. A `Settings.Language` beállítása ajánlott az automatikus észlelésből származó hibák elkerülésére.

</details>

> **Összefoglalás:** A Python a Foundry Local SDK-t használja a modellkezeléshez és az ONNX Runtime-ot az encoder/dekoder modellek közvetlen kiértékeléséhez. A JavaScript és a C# mindkettő az SDK beépített `AudioClient`-jét használja az egyszerűsített átiratozáshoz – először létrehozza a klienst, beállítja a nyelvet, majd meghívja a `transcribe()` / `TranscribeAudioAsync()` metódust. Mindig állítsd be az AudioClient nyelvi tulajdonságát a pontos eredményekért.

---

### 6. gyakorlat – Kísérletezz

Próbáld ki a következő módosításokat a mélyebb megértés érdekében:

1. **Próbálj ki különféle hangfájlokat** – rögzítsd magad Windows Hangrögzítővel, mentsd WAV formátumban, majd írasd át

2. **Hasonlítsd össze a modellváltozatokat** – ha van NVIDIA GPU-d, próbáld ki a CUDA változatot:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Hasonlítsd össze az átiratozás sebességét a CPU változattal.

3. **Adj hozzá kimeneti formázást** – a JSON válasz tartalmazhatja:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Építs REST API-t** – csomagold az átiratozó kódot webszervizbe:

   | Nyelv | Keretrendszer | Példa |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` `UploadFile`-lel |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` `multer`-rel |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` `IFormFile`-lal |

5. **Többfordulós átiratozás** – kombináld a Whisper-t a 4. rész chat ágenseivel: előbb átíratod a hangot, majd a szöveget elemzésre vagy összefoglalásra továbbítod.

---

## SDK Audio API Referencia

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — létrehoz egy `AudioClient` példányt
> - `audioClient.settings.language` — beállítja az átirat nyelvét (pl. `"en"`)
> - `audioClient.settings.temperature` — a véletlenszerűség szabályozására (opcionális)
> - `audioClient.transcribe(filePath)` — egy fájl átiratozása, visszaadja a `{ text, duration }` objektumot
> - `audioClient.transcribeStreaming(filePath, callback)` — átirat streaming darabokra callback-en keresztül
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — létrehoz egy `OpenAIAudioClient` példányt
> - `audioClient.Settings.Language` — beállítja az átirat nyelvét (pl. `"en"`)
> - `audioClient.Settings.Temperature` — a véletlenszerűség szabályozására (opcionális)
> - `await audioClient.TranscribeAudioAsync(filePath)` — egy fájl átiratozása, visszaad egy `.Text` tulajdonságú objektumot
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — `IAsyncEnumerable`-en ad vissza az átirat darabjait

> **Tipp:** Mindig állítsd be a nyelvi tulajdonságot mielőtt átiratoznál. Enélkül a Whisper modell automatikus felismerésre törekszik, ami rossz vagy olvashatatlan eredményt adhat (helyettesező karakter egyéni szöveg helyett).

---

## Összehasonlítás: Chat modellek vs. Whisper

| Szempont | Chat modellek (3–7. rész) | Whisper – Python | Whisper – JS / C# |
|----------|--------------------------|------------------|-------------------|
| **Feladat típusa** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Bemenet** | Szöveges üzenetek (JSON) | Hangfájlok (WAV/MP3/M4A) | Hangfájlok (WAV/MP3/M4A) |
| **Kimenet** | Generált szöveg (streamelve) | Átirat (teljes) | Átirat (teljes) |
| **SDK csomag** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API metódus** | `client.chat.completions.create()` | ONNX Runtime közvetlen | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Nyelv beállítása** | N/A | Decoder prompt tokenek | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming támogatás** | Igen | Nem | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Adatvédelem előnye** | Kód/adat helyben marad | Hangadat helyben marad | Hangadat helyben marad |

---

## Főbb tanulságok

| Fogalom | Mit tanultál |
|---------|--------------|
| **Whisper helyi futtatás** | A beszéd szöveggé alakítása teljesen helyileg történik, ideális a Zava ügyféltámogatói hívások és termékértékelések helyi átiratozásához |
| **SDK AudioClient** | A JavaScript és C# SDK-k beépített `AudioClient`-et biztosítanak, ami a teljes átiratfolyamatot egyetlen hívásban kezeli |
| **Nyelv beállítása** | Mindig állítsd be az AudioClient nyelvét (pl. `"en"`), különben az automatikus felismerés hibás eredményt adhat |
| **Python** | A `foundry-local-sdk`-t használja modellkezeléshez, `onnxruntime`-t, `transformers`-t és `librosa`-t közvetlen ONNX kiértékeléshez |
| **JavaScript** | A `foundry-local-sdk`-t használja `model.createAudioClient()`-vel – beállítja a `settings.language`-t, majd hívja a `transcribe()`-t |
| **C#** | A `Microsoft.AI.Foundry.Local` SDK-t használja `model.GetAudioClientAsync()`-vel – beállítja a `Settings.Language` értékét, majd hívja a `TranscribeAudioAsync()`-t |
| **Streaming támogatás** | A JS és C# SDK-k támogatják a `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` metódusokat, chunkok szerinti feldolgozáshoz |
| **CPU-optimalizált** | A CPU változat (~3,05 GB) bármely Windows gépen fut GPU nélkül is |
| **Adatvédelmi szempontok** | Kiváló megoldás, hogy a Zava ügyfélinterakciói és a szellemi tulajdon helyben maradjon |

---

## Források

| Forrás | Link |
|--------|------|
| Foundry Local dokumentáció | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK referencia | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper modell | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local weboldal | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Következő lépés

Folytasd a [10. rész: Egyedi vagy Hugging Face modellek használata](part10-custom-models.md) olvasásával, hogy saját modelljeidet állítsd össze a Hugging Face-ről, és használd őket a Foundry Localon keresztül.