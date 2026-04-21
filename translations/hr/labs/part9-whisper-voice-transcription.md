![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Dio 9: Prepis govora pomoću Whisper i Foundry Local

> **Cilj:** Koristiti OpenAI Whisper model koji radi lokalno putem Foundry Local za prepis audio datoteka - potpuno na uređaju, bez potrebe za cloudom.

## Pregled

Foundry Local nije namijenjen samo generiranju teksta; također podržava modele za **pretvaranje govora u tekst**. U ovom laboratoriju koristit ćete **OpenAI Whisper Medium** model za prepis audio datoteka u potpunosti na vašem računalu. Ovo je idealno za scenarije poput prepisivanja poziva korisničke službe Zava, snimki recenzija proizvoda ili planiranja radionica gdje audio podaci nikada ne smiju napustiti vaš uređaj.


---

## Ciljevi učenja

Do kraja ovog laboratorija moći ćete:

- Razumjeti Whisper model za pretvaranje govora u tekst i njegove mogućnosti
- Preuzeti i pokrenuti Whisper model koristeći Foundry Local
- Prepisivati audio datoteke koristeći Foundry Local SDK u Pythonu, JavaScriptu i C#
- Izgraditi jednostavnu uslugu transkripcije koja radi u potpunosti na uređaju
- Razumjeti razlike između chat/tekst modela i audio modela u Foundry Local

---

## Preduvjeti

| Zahtjev | Detalji |
|-------------|---------|
| **Foundry Local CLI** | Verzija **0.8.101 ili novija** (Whisper modeli dostupni od verzije v0.8.101 nadalje) |
| **OS** | Windows 10/11 (x64 ili ARM64) |
| **Okruženje za programski jezik** | **Python 3.9+** i/ili **Node.js 18+** i/ili **.NET 9 SDK** ([Preuzmi .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Završeno** | [Dio 1: Početak](part1-getting-started.md), [Dio 2: Dubinska analiza Foundry Local SDK](part2-foundry-local-sdk.md) i [Dio 3: SDK-ovi i API-ji](part3-sdk-and-apis.md) |

> **Napomena:** Whisper modele je potrebno preuzeti putem **SDK-a** (ne CLI-a). CLI ne podržava endpoint za prepisivanje zvuka. Provjerite svoju verziju s:
> ```bash
> foundry --version
> ```

---

## Koncept: Kako Whisper radi s Foundry Local

OpenAI Whisper model je univerzalni model za prepoznavanje govora treniran na velikom skupu raznolike audio građe. Kada se pokreće putem Foundry Local:

- Model radi **u potpunosti na vašem CPU-u** - GPU nije potreban
- Zvuk nikada ne napušta vaš uređaj - **potpuna privatnost**
- Foundry Local SDK upravlja preuzimanjem modela i cache memorijom
- **JavaScript i C#** nude ugrađeni `AudioClient` u SDK-u koji rukuje cijelim transkripcijskim procesom — nije potrebna ručna ONNX konfiguracija
- **Python** koristi SDK za upravljanje modelom i ONNX Runtime za izravno izvođenje prema encoder/decoder ONNX modelima

### Kako radi pipeline (JavaScript i C#) — SDK AudioClient

1. **Foundry Local SDK** preuzima i kešira Whisper model
2. `model.createAudioClient()` (JS) ili `model.GetAudioClientAsync()` (C#) kreira `AudioClient`
3. `audioClient.transcribe(path)` (JS) ili `audioClient.TranscribeAudioAsync(path)` (C#) interno obrađuje cijeli pipeline — predobrada zvuka, encoder, decoder, i dekodiranje tokena
4. `AudioClient` ima svojstvo `settings.language` (postavljeno na `"en"` za engleski) za preciznu transkripciju

### Kako radi pipeline (Python) — ONNX Runtime

1. **Foundry Local SDK** preuzima i kešira Whisper ONNX datoteke modela
2. **Predobrada zvuka** pretvara WAV audio u mel spektrogram (80 mel kanala x 3000 frejmova)
3. **Encoder** obrađuje mel spektrogram i proizvodi skrivene statuse plus ključne/vrijednosne tenzore za cross-attention
4. **Decoder** radi autoregresivno, generirajući token po token dok ne proizvede kraj teksta
5. **Tokenizator** dekodira izlazne token ID-ove natrag u čitljiv tekst

### Varijante Whisper modela

| Nadimak | ID modela | Uređaj | Veličina | Opis |
|---------|-----------|--------|----------|-------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1,53 GB | GPU ubrzanje (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3,05 GB | Optimizirano za CPU (preporučeno za većinu uređaja) |

> **Napomena:** Za razliku od chat modela koji se po defaultu prikazuju, Whisper modeli su kategorizirani pod zadatkom `automatic-speech-recognition`. Za detalje koristite `foundry model info whisper-medium`.

---

## Lab vježbe

### Vježba 0 - Preuzmi uzorke audio datoteka

Ovaj lab uključuje prethodno pripremljene WAV datoteke temeljene na Zava DIY scenarijima. Generirajte ih pomoću uključenog skripta:

```bash
# Iz korijena repozitorija - prvo kreirajte i aktivirajte .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Ovo kreira šest WAV datoteka u `samples/audio/`:

| Datoteka | Scenarij |
|----------|----------|
| `zava-customer-inquiry.wav` | Kupac se raspituje o **Zava ProGrip bežičnoj bušilici** |
| `zava-product-review.wav` | Kupac recenzira **Zava UltraSmooth unutarnju boju** |
| `zava-support-call.wav` | Poziv tehničke podrške za **Zava TitanLock alatni ormar** |
| `zava-project-planning.wav` | DIY entuzijast planira palubu s **Zava EcoBoard kompozitnom palubom** |
| `zava-workshop-setup.wav` | Pregled radionice koristeći **sva pet Zava proizvoda** |
| `zava-full-project-walkthrough.wav` | Detaljni pregled renovacije garaže koristeći **sve Zava proizvode** (~4 min, za testiranje dugotrajnog zvuka) |

> **Savjet:** Možete koristiti i vlastite WAV/MP3/M4A datoteke ili se snimiti Windows Voice Recorderom.

---

### Vježba 1 - Preuzmi Whisper model korištenjem SDK-a

Zbog nekompatibilnosti CLI-a s Whisper modelima u novijim verzijama Foundry Local, koristite **SDK** za preuzimanje i učitavanje modela. Odaberite svoj programski jezik:

<details>
<summary><b>🐍 Python</b></summary>

**Instalirajte SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Pokreni servis
manager = FoundryLocalManager()
manager.start_service()

# Provjeri informacije o katalogu
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Provjeri je li već u predmemoriji
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Učitaj model u memoriju
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Spremite kao `download_whisper.py` i pokrenite:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Instalirajte SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Kreirajte upravitelja i pokrenite uslugu
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Preuzmite model iz kataloga
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

// Učitajte model u memoriju
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Spremite kao `download-whisper.mjs` i pokrenite:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Instalirajte SDK:**
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

> **Zašto SDK, a ne CLI?** Foundry Local CLI ne podržava preuzimanje ili serviranje Whisper modela izravno. SDK pruža pouzdan način za preuzimanje i upravljanje audio modelima programatski. JavaScript i C# SDK-ovi uključuju ugrađeni `AudioClient` koji upravlja cijelim pipeline-om transkripcije. Python koristi ONNX Runtime za izravno izvođenje prema keširanim model datotekama.

---

### Vježba 2 - Razumjeti Whisper SDK

Whisper transkripcija koristi različite pristupe ovisno o jeziku. **JavaScript i C#** nude ugrađeni `AudioClient` u Foundry Local SDK-u koji upravlja cijelim pipeline-om (predobrada zvuka, encoder, decoder, dekodiranje tokena) u jednom pozivu metode. **Python** koristi Foundry Local SDK za upravljanje modelom i ONNX Runtime za izravno izvođenje prema encoder/decoder ONNX modelima.

| Komponenta | Python | JavaScript | C# |
|------------|--------|------------|----|
| **SDK paketi** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Upravljanje modelom** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalog |
| **Extractiranje značajki** | `WhisperFeatureExtractor` + `librosa` | Rukuje SDK `AudioClient` | Rukuje SDK `AudioClient` |
| **Izdvajanje zaključaka** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Dekodiranje tokena** | `WhisperTokenizer` | Rukuje SDK `AudioClient` | Rukuje SDK `AudioClient` |
| **Postavka jezika** | Postavlja se preko `forced_ids` u tokenima dekodera | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | Putanja do WAV datoteke | Putanja do WAV datoteke | Putanja do WAV datoteke |
| **Output** | Dekodirani tekstualni niz | `result.text` | `result.Text` |

> **Važno:** Uvijek postavite jezik na `AudioClient` (npr. `"en"` za engleski). Bez eksplicitne postavke jezika model može proizvesti nerazumljiv izlaz jer pokušava samostalno otkriti jezik.

> **SDK Obrasci:** Python koristi `FoundryLocalManager(alias)` za inicijalizaciju, zatim `get_cache_location()` za pronalazak ONNX model datoteka. JavaScript i C# koriste ugrađeni SDK `AudioClient` — pomoću `model.createAudioClient()` (JS) ili `model.GetAudioClientAsync()` (C#) — koji upravlja cijelim pipeline-om transkripcije. Pogledajte [Dio 2: Dubinska analiza Foundry Local SDK](part2-foundry-local-sdk.md) za detalje.

---

### Vježba 3 - Izgradite jednostavnu aplikaciju za transkripciju

Odaberite svoj programski jezik i izgradite minimalnu aplikaciju koja prepisuje audio datoteku.

> **Podržani audio formati:** WAV, MP3, M4A. Za najbolje rezultate koristite WAV datoteke s uzorkom od 16 kHz.

<details>
<summary><h3>Python staza</h3></summary>

#### Postavljanje

```bash
cd python
python -m venv venv

# Aktivirajte virtualno okruženje:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Kod za transkripciju

Kreirajte datoteku `foundry-local-whisper.py`:

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

# Korak 1: Bootstrap - pokreće servis, preuzima i učitava model
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Izradi putanju do keširanih ONNX model datoteka
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Korak 2: Učitaj ONNX sesije i ekstraktor značajki
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

# Korak 3: Izvuci mel spektrogram značajke
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Korak 4: Pokreni enkoder
enc_out = encoder.run(None, {"audio_features": input_features})
# Prvi izlaz su skrivena stanja; preostali su parovi KV iz cross-attentiona
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Korak 5: Autoregresivno dekodiranje
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcribe, bez vremenskih oznaka
input_ids = np.array([initial_tokens], dtype=np.int32)

# Prazan KV keš self-attentiona
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

    if next_token == 50257:  # kraj teksta
        break
    generated.append(next_token)

    # Ažuriraj KV keš self-attentiona
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Pokrenite je

```bash
# Prepiši scenarij proizvoda Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Ili isprobaj druge:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Ključne Python točke

| Metoda | Svrha |
|--------|--------|
| `FoundryLocalManager(alias)` | Pokretanje: startanje servisa, preuzimanje i učitavanje modela |
| `manager.get_cache_location()` | Dohvat putanje do keširanih ONNX model datoteka |
| `WhisperFeatureExtractor.from_pretrained()` | Učitavanje ekstraktora mel spektrograma |
| `ort.InferenceSession()` | Kreiranje ONNX Runtime sesija za encoder i decoder |
| `tokenizer.decode()` | Pretvaranje izlaznih token ID-ova natrag u tekst |

</details>

<details>
<summary><h3>JavaScript staza</h3></summary>

#### Postavljanje

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Kod za transkripciju

Kreirajte datoteku `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Korak 1: Bootstrap - kreirajte upravitelja, pokrenite uslugu i učitajte model
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

// Korak 2: Kreirajte audio klijenta i transkribirajte
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Očisti
await model.unload();
```

> **Napomena:** Foundry Local SDK pruža ugrađeni `AudioClient` putem `model.createAudioClient()` koji interno upravlja cijelim ONNX inference pipeline-om — nije potreban import `onnxruntime-node`. Uvijek postavite `audioClient.settings.language = "en"` kako biste osigurali točnu englesku transkripciju.

#### Pokrenite je

```bash
# Prepiši scenarij proizvoda Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Ili isprobaj druge:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Ključne JavaScript točke

| Metoda | Svrha |
|--------|-------|
| `FoundryLocalManager.create({ appName })` | Kreiranje singleton managera |
| `await catalog.getModel(alias)` | Dohvat modela iz kataloga |
| `model.download()` / `model.load()` | Preuzimanje i učitavanje Whisper modela |
| `model.createAudioClient()` | Kreiranje audio klijenta za transkripciju |
| `audioClient.settings.language = "en"` | Postavljanje jezika transkripcije (potrebno za precizan izlaz) |
| `audioClient.transcribe(path)` | Transkripcija audio datoteke, vraća `{ text, duration }` |

</details>

<details>
<summary><h3>C# staza</h3></summary>

#### Postavljanje

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Napomena:** C# staza koristi paket `Microsoft.AI.Foundry.Local` koji pruža ugrađeni `AudioClient` putem `model.GetAudioClientAsync()`. Ovo upravlja cijelim pipeline-om transkripcije u procesu — nije potrebna zasebna konfiguracija ONNX Runtime.

#### Kod za transkripciju

Zamijenite sadržaj `Program.cs`:

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

#### Pokrenite je

```bash
# Transkribirajte scenarij proizvoda Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Ili pokušajte druge:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Ključne C# točke

| Metoda | Svrha |
|--------|-------|
| `FoundryLocalManager.CreateAsync(config)` | Inicijalizacija Foundry Local s konfiguracijom |
| `catalog.GetModelAsync(alias)` | Dohvat modela iz kataloga |
| `model.DownloadAsync()` | Preuzimanje Whisper modela |
| `model.GetAudioClientAsync()` | Dohvat AudioClienta (ne ChatClienta!) |
| `audioClient.Settings.Language = "en"` | Postavljanje jezika za transkripciju (potrebno za točan izlaz) |
| `audioClient.TranscribeAudioAsync(path)` | Transkripcija audio datoteke |
| `result.Text` | Transkribirani tekst |
> **C# vs Python/JS:** C# SDK pruža ugrađeni `AudioClient` za transkripciju u procesu putem `model.GetAudioClientAsync()`, slično kao JavaScript SDK. Python koristi ONNX Runtime direktno za izvođenje prema keširanim enkoder/dekoder modelima.

</details>

---

### Vježba 4 - Batch Transkripcija svih Zava Primjera

Sada kada imate radnu aplikaciju za transkripciju, prepišite svih pet Zava uzoraka i usporedite rezultate.

<details>
<summary><h3>Python Staza</h3></summary>

Cijeli uzorak `python/foundry-local-whisper.py` već podržava batch transkripciju. Kada se pokrene bez argumenata, transkribira sve `zava-*.wav` datoteke u `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Uzorak koristi `FoundryLocalManager(alias)` za inicijalizaciju, zatim pokreće ONNX sesije enkodera i dekodera za svaku datoteku.

</details>

<details>
<summary><h3>JavaScript Staza</h3></summary>

Cijeli uzorak `javascript/foundry-local-whisper.mjs` već podržava batch transkripciju. Kada se pokrene bez argumenata, transkribira sve `zava-*.wav` datoteke u `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Uzorak koristi `FoundryLocalManager.create()` i `catalog.getModel(alias)` za inicijalizaciju SDK-a, zatim koristi `AudioClient` (s `settings.language = "en"`) za transkripciju svake datoteke.

</details>

<details>
<summary><h3>C# Staza</h3></summary>

Cijeli uzorak `csharp/WhisperTranscription.cs` već podržava batch transkripciju. Kada se pokrene bez specifičnog argumenta za datoteku, transkribira sve `zava-*.wav` datoteke u `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Uzorak koristi `FoundryLocalManager.CreateAsync()` i SDK-ev `AudioClient` (s `Settings.Language = "en"`) za transkripciju u procesu.

</details>

**Na što obratiti pažnju:** Usporedite izlaz transkripcije s izvornim tekstom u `samples/audio/generate_samples.py`. Koliko točno Whisper prepoznaje nazive proizvoda poput "Zava ProGrip" i tehničke pojmove poput "brushless motor" ili "composite decking"?

---

### Vježba 5 - Razumijevanje Ključnih Obrasca Koda

Proučite kako se Whisper transkripcija razlikuje od chat dopuna u sva tri jezika:

<details>
<summary><b>Python - Ključne Razlike u Odnosu na Chat</b></summary>

```python
# Dovršetak razgovora (Dijelovi 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Prepoznavanje govora (Ovaj dio):
# Koristi ONNX Runtime izravno umjesto OpenAI klijenta
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregresivna petlja dekodera ...
print(tokenizer.decode(generated_tokens))
```

**Ključni uvid:** Chat modeli koriste OpenAI-kompatibilan API putem `manager.endpoint`. Whisper koristi SDK za lociranje keširanih ONNX model datoteka pa direktno izvodi inferenciju s ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Ključne Razlike u Odnosu na Chat</b></summary>

```javascript
// Dovršetak chat-a (Dijelovi 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Prepisivanje zvuka (Ovaj dio):
// Koristi ugrađeni AudioClient SDK-a
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Uvijek postavite jezik za najbolje rezultate
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Ključni uvid:** Chat modeli koriste OpenAI-kompatibilan API putem `manager.urls[0] + "/v1"`. Whisper transkripcija koristi SDK-ev `AudioClient`, dobijen iz `model.createAudioClient()`. Postavite `settings.language` kako biste izbjegli nečitljiv ishod automatskog prepoznavanja.

</details>

<details>
<summary><b>C# - Ključne Razlike u Odnosu na Chat</b></summary>

C# pristup koristi SDK-ev ugrađeni `AudioClient` za transkripciju u procesu:

**Inicijalizacija modela:**

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

**Transkripcija:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Ključni uvid:** C# koristi `FoundryLocalManager.CreateAsync()` i direktno dobiva `AudioClient` — nije potrebna konfiguracija ONNX Runtime. Postavite `Settings.Language` da biste izbjegli nečitljiv ishod automatskog prepoznavanja.

</details>

> **Sažetak:** Python koristi Foundry Local SDK za upravljanje modelom i ONNX Runtime za direktnu inferenciju na enkoderskim/dekoderskim modelima. JavaScript i C# koriste SDK-ev ugrađeni `AudioClient` za pojednostavljenu transkripciju — kreirajte klijenta, postavite jezik, i pozovite `transcribe()` / `TranscribeAudioAsync()`. Uvijek postavite svojstvo jezika na AudioClientu za točne rezultate.

---

### Vježba 6 - Eksperimentirajte

Isprobajte ove izmjene za dublje razumijevanje:

1. **Isprobajte različite audio datoteke** - snimite sebe koristeći Windows Voice Recorder, spremite kao WAV i transkribirajte

2. **Usporedite varijante modela** - ako imate NVIDIA GPU, isprobajte CUDA varijantu:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Usporedite brzinu transkripcije s CPU varijantom.

3. **Dodajte formatiranje izlaza** - JSON odgovor može uključivati:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Napravite REST API** - omotajte svoj kod za transkripciju u web poslužitelj:

   | Jezik | Okvir | Primjer |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` s `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` s `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` s `IFormFile` |

5. **Višekratna interakcija s transkripcijom** - kombinirajte Whisper s chat agentom iz Dela 4: prvo transkribirajte audio, a zatim proslijedite tekst agentu za analizu ili sumiranje.

---

## SDK Audio API Referenca

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — stvara instancu `AudioClient`
> - `audioClient.settings.language` — postavite jezik transkripcije (npr. `"en"`)
> - `audioClient.settings.temperature` — kontrola slučajnosti (opcionalno)
> - `audioClient.transcribe(filePath)` — transkribira datoteku, vraća `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — stream transkripcijskih dijelova putem povratne funkcije
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — stvara instancu `OpenAIAudioClient`
> - `audioClient.Settings.Language` — postavite jezik transkripcije (npr. `"en"`)
> - `audioClient.Settings.Temperature` — kontrola slučajnosti (opcionalno)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transkribira datoteku, vraća objekt s `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — vraća `IAsyncEnumerable` transkripcijskih dijelova

> **Savjet:** Uvijek postavite svojstvo jezika prije transkripcije. Bez toga Whisper model pokušava automatski prepoznati jezik, što može proizvesti nečitljiv ishod (pojedinačni zamjenski znak umjesto teksta).

---

## Usporedba: Chat Modeli vs. Whisper

| Aspekt | Chat Modeli (Dijelovi 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|-----------------------------|-----------------|-------------------|
| **Vrsta zadatka** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Ulaz** | Tekstualne poruke (JSON) | Audio datoteke (WAV/MP3/M4A) | Audio datoteke (WAV/MP3/M4A) |
| **Izlaz** | Generirani tekst (stream) | Transkribirani tekst (kompletan) | Transkribirani tekst (kompletan) |
| **SDK paket** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API metoda** | `client.chat.completions.create()` | ONNX Runtime direktno | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Postavka jezika** | N/A | Dekoderski prompt tokeni | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Da | Ne | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Prednost privatnosti** | Kod/podaci ostaju lokalno | Audio podaci ostaju lokalno | Audio podaci ostaju lokalno |

---

## Ključni Zaključci

| Koncept | Što ste naučili |
|---------|-----------------|
| **Whisper na uređaju** | Prevođenje govora u tekst u potpunosti lokalno, idealno za transkripciju Zava poziva i recenzija proizvoda na uređaju |
| **SDK AudioClient** | JavaScript i C# SDK-ovi pružaju ugrađeni `AudioClient` koji rukuje cijelim procesom transkripcije u pozivu |
| **Postavka jezika** | Uvijek postavite jezik AudioClientu (npr. `"en"`) — bez toga, automatsko prepoznavanje može proizvesti nečitljiv ishod |
| **Python** | Koristi `foundry-local-sdk` za upravljanje modelima + `onnxruntime` + `transformers` + `librosa` za direktnu ONNX inferenciju |
| **JavaScript** | Koristi `foundry-local-sdk` s `model.createAudioClient()` — postavite `settings.language`, zatim pozovite `transcribe()` |
| **C#** | Koristi `Microsoft.AI.Foundry.Local` s `model.GetAudioClientAsync()` — postavite `Settings.Language`, zatim pozovite `TranscribeAudioAsync()` |
| **Podrška za streaming** | JS i C# SDK-ovi nude i `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` za ispis po dijelovima |
| **Optimizirano za CPU** | CPU varijanta (3,05 GB) radi na bilo kojem Windows uređaju bez GPU-a |
| **Privatnost na prvom mjestu** | Idealno za čuvanje interakcija s kupcima i povjerljivih podataka o proizvodu lokalno na uređaju |

---

## Resursi

| Resurs | Link |
|----------|------|
| Foundry Local dokumentacija | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referenca | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper model | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local web stranica | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Sljedeći korak

Nastavite na [Dio 10: Korištenje prilagođenih ili Hugging Face modela](part10-custom-models.md) za sastavljanje vlastitih modela s Hugging Face i njihovo pokretanje kroz Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Odricanje od odgovornosti**:
Ovaj dokument preveden je korištenjem AI prevoditeljskog servisa [Co-op Translator](https://github.com/Azure/co-op-translator). Iako nastojimo osigurati točnost, imajte na umu da automatizirani prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na njegovom izvornom jeziku treba se smatrati autoritativnim izvorom. Za kritične informacije preporučuje se profesionalni ljudski prijevod. Ne odgovaramo za bilo kakve nesporazume ili pogrešne interpretacije koje proizlaze iz korištenja ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->