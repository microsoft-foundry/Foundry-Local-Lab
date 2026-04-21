![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 9: Glasovna transkripcija z Whisper in Foundry Local

> **Cilj:** Uporabiti model OpenAI Whisper, ki teče lokalno prek Foundry Local, za prepis zvočnih datotek - popolnoma na napravi, brez potrebe po oblaku.

## Pregled

Foundry Local ni namenjen samo generiranju besedila; podpira tudi **modeli za govor v besedilo**. V tej vadnici boste uporabili model **OpenAI Whisper Medium** za prepis zvočnih datotek popolnoma na svojem računalniku. To je idealno za scenarije, kot so prepisovanje klicev službe za podporo strankam Zava, posnetkov mnenj o izdelkih ali načrtovanje delavnic, kjer zvočni podatki nikoli ne zapustijo vaše naprave.


---

## Cilji učenja

Ob koncu te vadnice boste lahko:

- Razumeli model Whisper za govor v besedilo in njegove zmogljivosti
- Prenesli in zagnali model Whisper z uporabo Foundry Local
- Prepisovali zvočne datoteke z uporabo Foundry Local SDK v Pythonu, JavaScriptu in C#
- Zgradili preprosto storitev za transkripcijo, ki teče popolnoma na napravi
- Razumeli razlike med klepetalnimi/besedilnimi modeli in avdio modeli v Foundry Local

---

## Predpogoji

| Zahteva | Podrobnosti |
|-------------|---------|
| **Foundry Local CLI** | Verzija **0.8.101 ali novejša** (Whisper modeli so na voljo od v. 0.8.101 naprej) |
| **Operacijski sistem** | Windows 10/11 (x64 ali ARM64) |
| **Jezični runtime** | **Python 3.9+** in/ali **Node.js 18+** in/ali **.NET 9 SDK** ([Prenos .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Zaključeno** | [Del 1: Prvi koraki](part1-getting-started.md), [Del 2: Poglobljen pregled Foundry Local SDK](part2-foundry-local-sdk.md) in [Del 3: SDK-ji in API-ji](part3-sdk-and-apis.md) |

> **Opomba:** Whisper modele je potrebno prenesti preko **SDK-ja** (ne CLI-ja). CLI ne podpira končne točke za avdio transkripcijo. Verzijo preverite z:
> ```bash
> foundry --version
> ```

---

## Koncept: Kako Whisper deluje s Foundry Local

Model OpenAI Whisper je model za prepoznavanje govora splošnega namena, treniran na velikem naboru raznolikega zvoka. Ko teče prek Foundry Local:

- Model teče **popolnoma na vašem CPU-ju** - brez potrebe po GPU-ju
- Zvok nikoli ne zapusti vaše naprave - **popolna zasebnost**
- Foundry Local SDK poskrbi za prenos modela in upravljanje predpomnilnika
- **JavaScript in C#** vključujeta v SDK vgrajen `AudioClient`, ki upravlja celotno transkripcijsko cevovod — brez ročne nastavitve ONNX
- **Python** uporablja SDK za upravljanje modela in ONNX Runtime za neposredno inferenco nad ONNX modeli encoderja/dekoderja

### Kako cevovod deluje (JavaScript in C#) — SDK AudioClient

1. **Foundry Local SDK** prenese in predpomni Whisper model
2. `model.createAudioClient()` (JS) ali `model.GetAudioClientAsync()` (C#) ustvari `AudioClient`
3. `audioClient.transcribe(path)` (JS) ali `audioClient.TranscribeAudioAsync(path)` (C#) interno upravlja celoten cevovod — predobdelava zvoka, enkoder, dekoder in dekodiranje tokenov
4. `AudioClient` izpostavi lastnost `settings.language` (nastavljeno na `"en"` za angleščino) za natančen prepis

### Kako cevovod deluje (Python) — ONNX Runtime

1. **Foundry Local SDK** prenese in predpomni Whisper ONNX modelne datoteke
2. **Predobdelava zvoka** pretvori WAV avdio v mel spektrogram (80 mel košev x 3000 sličic)
3. **Enkoder** obdela mel spektrogram in ustvari skrite stanje ter tenzorje za ključe/vrzeli križne pozornosti
4. **Dekoder** deluje avtorregresivno, generira en token naenkrat dokler ne proizvede končnega tokena besedila
5. **Tokenizator** dekodira izhodne ID-je tokenov nazaj v berljivo besedilo

### Variante Whisper modela

| Alias | ID modela | Naprava | Velikost | Opis |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Pospešeno z GPU-jem (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Optimizirano za CPU (priporočeno za večino naprav) |

> **Opomba:** Za razliko od klepetalnih modelov, ki so privzeto na seznamu, so Whisper modeli kategorizirani pod opravilo `automatic-speech-recognition`. Podrobnosti si oglejte z ukazom `foundry model info whisper-medium`.

---

## Lab vaje

### Vaja 0 - Pridobite vzorčne zvočne datoteke

Ta vadnica vključuje že ustvarjene WAV datoteke, ki temeljijo na scenarijih izdelkov Zava DIY. Ustvarite jih z vključenim skriptom:

```bash
# Iz korena repozitorija - najprej ustvarite in aktivirajte .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Ustvari šest WAV datotek v mapi `samples/audio/`:

| Datoteka | Scenarij |
|------|----------|
| `zava-customer-inquiry.wav` | Kupec sprašuje o **Zava ProGrip Brezžičnem vrtalniku** |
| `zava-product-review.wav` | Kupec ocenjuje **Zava UltraSmooth notranjo barvo** |
| `zava-support-call.wav` | Klic podpore glede **Zava TitanLock orodjarne** |
| `zava-project-planning.wav` | Samograditelj načrtuje teraso s **Zava EcoBoard kompozitno teraso** |
| `zava-workshop-setup.wav` | Pregled delavnice z uporabo **vseh petih Zava izdelkov** |
| `zava-full-project-walkthrough.wav` | Podroben pregled prenove garaže z uporabo **vseh Zava izdelkov** (~4 min, za testiranje dolgih posnetkov) |

> **Namig:** Uporabite lahko tudi svoje WAV/MP3/M4A datoteke ali posnemite s Windows Voice Recorderjem.

---

### Vaja 1 - Prenesite Whisper model z uporabo SDK-ja

Zaradi nezdružljivosti CLI z Whisper modeli v novejših različicah Foundry Local uporabite **SDK** za prenos in nalaganje modela. Izberite svoj programski jezik:

<details>
<summary><b>🐍 Python</b></summary>

**Namestite SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Začni storitev
manager = FoundryLocalManager()
manager.start_service()

# Preveri informacije kataloga
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Preveri, ali je že v predpomnilniku
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Naloži model v pomnilnik
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Shranjite kot `download_whisper.py` in zaženite:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Namestite SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Ustvari upravitelja in zaženi storitev
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Pridobi model iz kataloga
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

// Naloži model v pomnilnik
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Shranjite kot `download-whisper.mjs` in zaženite:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Namestite SDK:**
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

> **Zakaj SDK namesto CLI?** Foundry Local CLI ne podpira neposrednega prenosa ali serviranja Whisper modelov. SDK zagotavlja zanesljivo metodo za prenos in upravljanje avdio modelov programsko. JavaScript in C# SDK imata vgrajen `AudioClient`, ki upravlja celotno cevovod transkripcije. Python uporablja ONNX Runtime za neposredno inferenco nad predpomnjenimi modelnimi datotekami.

---

### Vaja 2 - Razumevanje Whisper SDK-ja

Whisper transkripcija uporablja različne pristope glede na jezik. **JavaScript in C#** v Foundry Local SDK vključita vgrajen `AudioClient`, ki upravlja celoten cevovod (predobdelava zvoka, enkoder, dekoder, dekodiranje tokenov) v enem klicu metode. **Python** uporablja Foundry Local SDK za upravljanje modela in ONNX Runtime za neposredno inferenco nad ONNX modeli enkoderja/dekoderja.

| Komponenta | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK paketi** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Upravljanje modela** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalog |
| **Ekstrakcija lastnosti** | `WhisperFeatureExtractor` + `librosa` | Upravljanje prek SDK `AudioClient` | Upravljanje prek SDK `AudioClient` |
| **Inferenca** | `ort.InferenceSession` (enkoder + dekoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Dekodiranje tokenov** | `WhisperTokenizer` | Upravljanje prek SDK `AudioClient` | Upravljanje prek SDK `AudioClient` |
| **Nastavitev jezika** | Nastavljeno prek `forced_ids` v dekoderju | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Vnos** | Pot do WAV datoteke | Pot do WAV datoteke | Pot do WAV datoteke |
| **Izhod** | Dekodirana besedilna niz | `result.text` | `result.Text` |

> **Pomembno:** Vedno nastavite jezik na `AudioClient` (npr. `"en"` za angleščino). Brez eksplicitne nastaviteve jezika lahko model ustvari neberljiv izhod, saj poskuša samodejno zaznati jezik.

> **SDK vzorci:** Python uporablja `FoundryLocalManager(alias)` za inicializacijo, nato `get_cache_location()` za iskanje ONNX modelnih datotek. JavaScript in C# uporabljata vgrajen `AudioClient` iz SDK — pridobljen z `model.createAudioClient()` (JS) ali `model.GetAudioClientAsync()` (C#) — ki upravlja celoten cevovod transkripcije. Celotne podrobnosti so na voljo v [Del 2: Poglobljen pregled Foundry Local SDK](part2-foundry-local-sdk.md).

---

### Vaja 3 - Zgradite preprosto aplikacijo za transkripcijo

Izberite svojo programsko pot in zgradite minimalno aplikacijo, ki prepisuje zvočno datoteko.

> **Podprti avdio formati:** WAV, MP3, M4A. Za najboljše rezultate uporabite WAV datoteke z vzorčno frekvenco 16kHz.

<details>
<summary><h3>Python pot</h3></summary>

#### Namestitev

```bash
cd python
python -m venv venv

# Aktivirajte virtualno okolje:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Koda za transkripcijo

Ustvarite datoteko `foundry-local-whisper.py`:

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

# Korak 1: Bootstrap - zažene storitev, prenese in naloži model
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Zgradi pot do predpomnjenih ONNX modelnih datotek
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Korak 2: Naloži ONNX seje in ekstraktor lastnosti
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

# Korak 3: Izvleci značilnosti mel spekrograma
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Korak 4: Zaženi kodirnik
enc_out = encoder.run(None, {"audio_features": input_features})
# Prvi izhod so skriti stanja; preostali so pari ključ-vrednost za križno pozornost
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Korak 5: Avtoregresivno dekodiranje
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, prepiši, brez časovnih oznak
input_ids = np.array([initial_tokens], dtype=np.int32)

# Prazen predpomnilnik ključ-vrednost samopozornosti
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

    if next_token == 50257:  # konec besedila
        break
    generated.append(next_token)

    # Posodobi predpomnilnik ključ-vrednost samopozornosti
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Zaženite jo

```bash
# Prepiši scenarij izdelka Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Ali poskusi druge:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Ključne točke za Python

| Metoda | Namen |
|--------|---------|
| `FoundryLocalManager(alias)` | Inicializacija: zagon storitve, prenos in nalaganje modela |
| `manager.get_cache_location()` | Pridobitev poti do predpomnjenih ONNX modelnih datotek |
| `WhisperFeatureExtractor.from_pretrained()` | Naloži ekstraktor lastnosti mel spektrograma |
| `ort.InferenceSession()` | Ustvari ONNX Runtime seje za enkoder in dekoder |
| `tokenizer.decode()` | Pretvori izhodne ID-je tokenov nazaj v besedilo |

</details>

<details>
<summary><h3>JavaScript pot</h3></summary>

#### Namestitev

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Koda za transkripcijo

Ustvarite datoteko `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Korak 1: Bootstrap - ustvarite upravitelja, zaženite storitev in naložite model
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

// Korak 2: Ustvarite avdio odjemalca in pretvorite govor v besedilo
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Čiščenje
await model.unload();
```

> **Opomba:** Foundry Local SDK vključi vgrajen `AudioClient` prek `model.createAudioClient()`, ki interno upravlja celoten ONNX inferenčni cevovod — ni potrebe po uvozu `onnxruntime-node`. Vedno nastavite `audioClient.settings.language = "en"`, da zagotovite natančen prepis angleščine.

#### Zaženite jo

```bash
# Prepiši scenarij izdelka Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Ali poskusi druge:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Ključne točke za JavaScript

| Metoda | Namen |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Ustvari instanco upravitelja (singleton) |
| `await catalog.getModel(alias)` | Pridobi model iz kataloga |
| `model.download()` / `model.load()` | Prenesi in naloži Whisper model |
| `model.createAudioClient()` | Ustvari audio klienta za transkripcijo |
| `audioClient.settings.language = "en"` | Nastavi jezik transkripcije (zahtevano za natančen izhod) |
| `audioClient.transcribe(path)` | Prepiši avdio datoteko, vrne `{ text, duration }` |

</details>

<details>
<summary><h3>C# pot</h3></summary>

#### Namestitev

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Opomba:** C# pot uporablja paket `Microsoft.AI.Foundry.Local`, ki vključi vgrajen `AudioClient` prek `model.GetAudioClientAsync()`. Ta upravlja celoten cevovod transkripcije v procesu — ni potrebe po ločeni nastavitvi ONNX Runtime.

#### Koda za transkripcijo

Zamenjajte vsebino datoteke `Program.cs`:

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

#### Zaženite jo

```bash
# Prepiši scenarij izdelka Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Ali poskusi druge:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Ključne točke za C#

| Metoda | Namen |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Inicializacija Foundry Local s konfiguracijo |
| `catalog.GetModelAsync(alias)` | Pridobi model iz kataloga |
| `model.DownloadAsync()` | Prenos Whisper modela |
| `model.GetAudioClientAsync()` | Pridobi AudioClient (ne ChatClienta!) |
| `audioClient.Settings.Language = "en"` | Nastavi jezik transkripcije (zahtevano za natančen izhod) |
| `audioClient.TranscribeAudioAsync(path)` | Prepiši avdio datoteko |
| `result.Text` | Prepisano besedilo |
> **C# vs Python/JS:** C# SDK nudi vgrajen `AudioClient` za transkripcijo v procesu prek `model.GetAudioClientAsync()`, podobno kot JavaScript SDK. Python uporablja ONNX Runtime neposredno za inferenco nad predpomnjenimi modeli enkoder/dekoder.

</details>

---

### Vadba 4 - Serijsko prepiši vse vzorce Zava

Zdaj, ko imaš delujočo aplikacijo za transkripcijo, prepiši vseh pet vzorčnih datotek Zava in primerjaj rezultate.

<details>
<summary><h3>Python potek</h3></summary>

Celoten vzorec `python/foundry-local-whisper.py` že podpira serijsko transkripcijo. Ko se zažene brez argumentov, prepiše vse `zava-*.wav` datoteke v `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Vzorec uporablja `FoundryLocalManager(alias)` za inicializacijo, nato pa za vsako datoteko zažene ONNX seje enkoderja in dekoderja.

</details>

<details>
<summary><h3>JavaScript potek</h3></summary>

Celoten vzorec `javascript/foundry-local-whisper.mjs` že podpira serijsko transkripcijo. Ko se zažene brez argumentov, prepiše vse `zava-*.wav` datoteke v `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Vzorec uporablja `FoundryLocalManager.create()` in `catalog.getModel(alias)` za inicializacijo SDK, nato pa uporabi `AudioClient` (z `settings.language = "en"`) za transkripcijo vsake datoteke.

</details>

<details>
<summary><h3>C# potek</h3></summary>

Celoten vzorec `csharp/WhisperTranscription.cs` že podpira serijsko transkripcijo. Ko se zažene brez določenega argumenta datoteke, prepiše vse `zava-*.wav` datoteke v `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Vzorec uporablja `FoundryLocalManager.CreateAsync()` in SDK-jev `AudioClient` (z `Settings.Language = "en"`) za transkripcijo v procesu.

</details>

**Na kaj paziti:** Primerjaj izhod transkripcije z izvirnim besedilom v `samples/audio/generate_samples.py`. Kako natančno Whisper zajame imena izdelkov, kot je "Zava ProGrip", in tehnične izraze, kot so "brushless motor" ali "composite decking"?

---

### Vadba 5 - Razumi ključne vzorce kode

Preuči, kako se transkripcija Whisper razlikuje od zaključkov klepeta v vseh treh jezikih:

<details>
<summary><b>Python - Ključne razlike od klepeta</b></summary>

```python
# Dokončanje klepeta (Del 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Prepis zvoka (Ta del):
# Uporablja ONNX Runtime neposredno namesto OpenAI odjemalca
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... zanka avtoregresivnega dekoderja ...
print(tokenizer.decode(generated_tokens))
```

**Ključni vpogled:** Klepetalni modeli uporabljajo API, združljiv z OpenAI, prek `manager.endpoint`. Whisper uporablja SDK za iskanje predpomnjenih ONNX modelnih datotek in nato izvaja inferenco neposredno z ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Ključne razlike od klepeta</b></summary>

```javascript
// Dokončanje klepeta (del 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Prepis zvoka (ta del):
// Uporablja vgrajen AudioClient v SDK-ju
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Vedno nastavite jezik za najboljše rezultate
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Ključni vpogled:** Klepetalni modeli uporabljajo API, združljiv z OpenAI, prek `manager.urls[0] + "/v1"`. Transkripcija Whisper uporablja SDK-jevega `AudioClient`, pridobljenega iz `model.createAudioClient()`. Nastavi `settings.language`, da se izogneš nerazumljivemu izhodu zaradi samodejnega zaznavanja.

</details>

<details>
<summary><b>C# - Ključne razlike od klepeta</b></summary>

C# pristop uporablja SDK-jevega vgrajenega `AudioClient` za transkripcijo v procesu:

**Inicializacija modela:**

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

**Ključni vpogled:** C# uporablja `FoundryLocalManager.CreateAsync()` in pridobi `AudioClient` neposredno — nastavitev ONNX Runtime ni potrebna. Nastavi `Settings.Language`, da se izogneš nerazumljivemu izhodu zaradi samodejnega zaznavanja.

</details>

> **Povzetek:** Python uporablja Foundry Local SDK za upravljanje modelov in ONNX Runtime za neposredno inferenco nad modeli enkoderja/dekoderja. JavaScript in C# oba uporabljata SDK-jevega vgrajenega `AudioClient` za poenostavljeno transkripcijo — ustvari klienta, nastavi jezik in pokliči `transcribe()` / `TranscribeAudioAsync()`. Vedno nastavi lastnost jezika na AudioClient za natančne rezultate.

---

### Vadba 6 - Eksperimentiraj

Preizkusi te spremembe, da poglobiš svoje razumevanje:

1. **Preizkusi različne zvočne datoteke** – posnemi sebe, kako govoriš z Windows Voice Recorderjem, shrani kot WAV in jih prepiši

2. **Primerjaj različice modela** – če imaš NVIDIA GPU, preizkusi CUDA različico:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Primerjaj hitrost transkripcije z različico za CPU.

3. **Dodaj oblikovanje izhoda** – JSON odgovor lahko vključuje:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Ustvari REST API** – zavij svojo kodo za transkripcijo v spletni strežnik:

   | Jezik | Okvir | Primer |
   |-------|-------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` z `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` z `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` z `IFormFile` |

5. **Večkratni klic s transkripcijo** – združi Whisper s klepetalnim agentom iz dela 4: najprej prepiši zvok, nato posreduj besedilo agentu za analizo ali povzetek.

---

## Referenca SDK Audio API

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — ustvari instanco `AudioClient`
> - `audioClient.settings.language` — nastavi jezik transkripcije (npr. `"en"`)
> - `audioClient.settings.temperature` — nadzor naključnosti (izbirno)
> - `audioClient.transcribe(filePath)` — prepiši datoteko, vrne `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — pretaka transkripcijske kose prek klica nazaj
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — ustvari instanco `OpenAIAudioClient`
> - `audioClient.Settings.Language` — nastavi jezik transkripcije (npr. `"en"`)
> - `audioClient.Settings.Temperature` — nadzor naključnosti (izbirno)
> - `await audioClient.TranscribeAudioAsync(filePath)` — prepiši datoteko, vrne objekt z `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — vrne `IAsyncEnumerable` transkripcijskih kosov

> **Namig:** Vedno nastavi lastnost jezika pred transkripcijo. Brez nje Whisper model poskuša samodejno zaznavo, kar lahko povzroči nerazumljiv izhod (ena sama nadomestna koda namesto besedila).

---

## Primerjava: Klepetni modeli vs. Whisper

| Vidik | Klepetni modeli (deli 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|----------------------------|-----------------|-------------------|
| **Vrsta naloge** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Vhod** | Besedilna sporočila (JSON) | Zvočne datoteke (WAV/MP3/M4A) | Zvočne datoteke (WAV/MP3/M4A) |
| **Izhod** | Generirano besedilo (pretok) | Prepisano besedilo (popolno) | Prepisano besedilo (popolno) |
| **SDK paket** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API metoda** | `client.chat.completions.create()` | ONNX Runtime neposredno | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Nastavitev jezika** | Ni na voljo | Dekoderski pozivni tokeni | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Pretakanje** | Da | Ne | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Prednost zasebnosti** | Koda/podatki ostanejo lokalno | Zvočni podatki ostanejo lokalno | Zvočni podatki ostanejo lokalno |

---

## Ključne ugotovitve

| Koncept | Kaj si se naučil |
|---------|------------------|
| **Whisper na napravi** | Preobrazba govora v besedilo poteka popolnoma lokalno, idealno za prepisovanje klicev strank Zava in ocen izdelkov na napravi |
| **SDK AudioClient** | JavaScript in C# SDK ponujata vgrajen `AudioClient`, ki v enem klicu obdela celotno transkripcijsko cevovod |
| **Nastavitev jezika** | Vedno nastavi jezik AudioClient (npr. `"en"`) — brez tega lahko samodejna zaznava povzroči nerazumljiv izhod |
| **Python** | Uporablja `foundry-local-sdk` za upravljanje modelov + `onnxruntime` + `transformers` + `librosa` za neposredno ONNX inferenco |
| **JavaScript** | Uporablja `foundry-local-sdk` z `model.createAudioClient()` — nastavi `settings.language`, nato pokliči `transcribe()` |
| **C#** | Uporablja `Microsoft.AI.Foundry.Local` z `model.GetAudioClientAsync()` — nastavi `Settings.Language`, nato pokliči `TranscribeAudioAsync()` |
| **Podpora pretakanju** | JS in C# SDK ponujata tudi `transcribeStreaming()` / `TranscribeAudioStreamingAsync()`, da izhod dobivaš kos za kosom |
| **Optimizirano za CPU** | CPU različica (3.05 GB) deluje na vsaki Windows napravi brez GPU |
| **Zasebnost na prvem mestu** | Popolno za ohranjanje interakcij strank Zava in lastnih podatkov o izdelkih na napravi |

---

## Viri

| Vir | Povezava |
|----------|----------|
| Dokumentacija Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referenca Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper model | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Spletna stran Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Naslednji korak

Nadaljuj na [Del 10: Uporaba lastnih ali Hugging Face modelov](part10-custom-models.md), da sestaviš svoje modele iz Hugging Face in jih zaženeš prek Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Omejitev odgovornosti**:
Ta dokument je bil preveden z uporabo storitve za prevajanje z umetno inteligenco [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za natančnost, vas prosimo, da upoštevate, da lahko samodejni prevodi vsebujejo napake ali netočnosti. Izvirni dokument v njegovem maternem jeziku je treba šteti za avtoritativni vir. Za ključne informacije se priporoča strokovni človeški prevod. Nismo odgovorni za kakršne koli nesporazume ali napačne razlage, ki izhajajo iz uporabe tega prevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->