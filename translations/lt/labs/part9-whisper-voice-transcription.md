![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 9 dalis: balso transkripcija naudojant Whisper ir Foundry Local

> **Tikslas:** naudoti OpenAI Whisper modelį, veikiančią vietoje per Foundry Local, kad būtų galima transkribuoti garso failus – visiškai įrenginyje, nereikia debesies.

## Apžvalga

Foundry Local nėra skirtas tik teksto generavimui; jis taip pat palaiko **balso į tekstą** modelius. Šiame laboratoriniame darbe naudosite **OpenAI Whisper Medium** modelį, kad transkribuotumėte garso failus visiškai savo mašinoje. Tai idealiai tinka situacijoms, tokioms kaip Zava klientų aptarnavimo skambučių, produktų apžvalgų įrašų ar dirbtuvių planavimo sesijų, kurių garso duomenys neturi palikti įrenginio, transkribavimas.

---

## Mokymosi tikslai

Iki šio laboratorinio darbo pabaigos jūs sugebėsite:

- Suprasti Whisper balso į tekstą modelį ir jo galimybes
- Atsisiųsti ir paleisti Whisper modelį naudojant Foundry Local
- Transkribuoti garso failus naudojant Foundry Local SDK Python, JavaScript ir C# kalbomis
- Sukurti paprastą transkripcijos paslaugą, kuri veikia visiškai įrenginyje
- Suprasti skirtumus tarp pokalbių/teksto modelių ir garso modelių Foundry Local

---

## Reikalavimai

| Reikalavimas | Informacija |
|--------------|-------------|
| **Foundry Local CLI** | Versija **0.8.101 arba naujesnė** (Whisper modeliai pasiekiami nuo v0.8.101) |
| **OS** | Windows 10/11 (x64 arba ARM64) |
| **Programavimo kalbos aplinka** | **Python 3.9+** ir/arba **Node.js 18+** ir/arba **.NET 9 SDK** ([Atsisiųsti .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Atlikta** | [1 dalis: Pradžia](part1-getting-started.md), [2 dalis: Foundry Local SDK giluminis aprašymas](part2-foundry-local-sdk.md), ir [3 dalis: SDK ir API](part3-sdk-and-apis.md) |

> **Pastaba:** Whisper modelius reikia atsisiųsti per **SDK** (ne per CLI). CLI nepalaiko garso transkripcijos pabaigos taško. Patikrinkite versiją komandą:
> ```bash
> foundry --version
> ```

---

## Koncepcija: kaip Whisper veikia su Foundry Local

OpenAI Whisper modelis yra bendros paskirties balso atpažinimo modelis, apmokytas dideliame įvairių garso įrašų rinkinyje. Veikiant per Foundry Local:

- Modelis veikia **visiškai jūsų procesoriuje** – nereikia GPU
- Garso duomenys niekada neišvyksta iš jūsų įrenginio – **visiška privatumas**
- Foundry Local SDK tvarko modelio atsisiuntimą ir kešavimo valdymą
- **JavaScript ir C#** SDK turi įmontuotą `AudioClient`, kuris valdo visą transkripcijos srautą – nereikia rankinio ONNX nustatymo
- **Python** naudoja SDK modelio valdymui ir ONNX Runtime tiesioginiam koduotuvo/dekoduotuvo ONNX modeliui apdoroti

### Kaip veikia srautas (JavaScript ir C#) — SDK AudioClient

1. **Foundry Local SDK** atsisiunčia ir kešuoja Whisper modelį
2. `model.createAudioClient()` (JS) arba `model.GetAudioClientAsync()` (C#) sukuria `AudioClient`
3. `audioClient.transcribe(path)` (JS) arba `audioClient.TranscribeAudioAsync(path)` (C#) viduje apdoroja visą srautą – garso priekinį apdorojimą, kodavimą, dekodavimą ir ženklų dekodavimą
4. `AudioClient` turi `settings.language` savybę (nustatytą į `"en"` anglų kalbai) tikslių transkripcijų gaires

### Kaip veikia srautas (Python) — ONNX Runtime

1. **Foundry Local SDK** atsisiunčia ir kešuoja Whisper ONNX modeliui failus
2. **Garso priekinis apdorojimas** paverčia WAV garsą į mel spektrogramą (80 mel juostų x 3000 kadrų)
3. **Koduotuvas** apdoroja mel spektrogramą ir generuoja paslėptas būsenas bei kryžminio dėmesio rakto/reikšmės tensorius
4. **Dekoduotuvas** veikia autoregresiškai, generuodamas po vieną ženklą, kol pasiekia teksto pabaigos ženklą
5. **Ženklų dekoderis** grąžina išvesties ženklų ID į skaitomą tekstą

### Whisper modelio variantai

| Aliasas | Modelio ID | Įrenginys | Dydis | Aprašymas |
|---------|------------|-----------|-------|-----------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1,53 GB | GPU pagreitintas (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3,05 GB | CPU optimizuotas (rekomenduojamas daugeliui įrenginių) |

> **Pastaba:** Skirtingai nei pokalbių modeliai, kurie pagal nutylėjimą rodomi sąraše, Whisper modeliai priskirti prie `automatic-speech-recognition` užduoties. Naudokite komandą `foundry model info whisper-medium`, kad pamatytumėte detales.

---

## Praktinės užduotys

### Užduotis 0 – Gauti pavyzdinius garso failus

Šiame laboratoriniame darbe yra paruošti WAV failai, pagrįsti Zava DIY produktų scenarijais. Generuokite juos naudodami pridėtą scenarijų:

```bash
# Iš repozitorijos šaknies sukurkite ir aktyvuokite .venv pirmiausia
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Tai sukurs šešis WAV failus kataloge `samples/audio/`:

| Failas | Scenarijus |
|--------|------------|
| `zava-customer-inquiry.wav` | Klientas domisi **Zava ProGrip bevieliu gręžtuvu** |
| `zava-product-review.wav` | Kliento atsiliepimas apie **Zava UltraSmooth vidaus dažus** |
| `zava-support-call.wav` | Pagalbos skambutis dėl **Zava TitanLock įrankių spintelės** |
| `zava-project-planning.wav` | „Pasidaryk pats“ kuris planuoja terasą su **Zava EcoBoard kompozitine terasa** |
| `zava-workshop-setup.wav` | Dirbtuvių apžvalga naudojant **visus penkis Zava produktus** |
| `zava-full-project-walkthrough.wav` | Išsamus garažo renovacijos apžvalgos įrašas naudojant **visus Zava produktus** (~4 min, ilgo garso testavimui) |

> **Patarimas:** Taip pat galite naudoti savo WAV/MP3/M4A failus arba įrašyti save su Windows Voice Recorder.

---

### Užduotis 1 – Atsisiųsti Whisper modelį naudojant SDK

Dėl CLI nesuderinamumo naujesnėse Foundry Local versijose naudokite **SDK** modelio atsisiuntimui ir įkėlimui. Pasirinkite kalbą:

<details>
<summary><b>🐍 Python</b></summary>

**Įdiekite SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Paleisti paslaugą
manager = FoundryLocalManager()
manager.start_service()

# Patikrinti katalogo informaciją
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Patikrinti, ar jau talpykloje
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Įkelti modelį į atmintį
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Išsaugokite kaip `download_whisper.py` ir paleiskite:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Įdiekite SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Sukurkite vadybininką ir paleiskite paslaugą
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Gaukite modelį iš katalogo
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

// Įkelkite modelį į atmintį
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Išsaugokite kaip `download-whisper.mjs` ir paleiskite:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Įdiekite SDK:**
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

> **Kodėl SDK, o ne CLI?** Foundry Local CLI nepalaiko Whisper modelių tiesioginio atsisiuntimo ar paleidimo. SDK suteikia patikimą būdą programuotiniam garso modelių atsisiuntimui ir valdymui. JavaScript ir C# SDK turi įmontuotą `AudioClient`, kuris valdo visą transkripcijos srautą. Python naudoja ONNX Runtime tiesioginiam inferencijai pagal kešiuojamus modelio failus.

---

### Užduotis 2 – Suprasti Whisper SDK

Whisper transkripcija naudoja skirtingus metodus priklausomai nuo kalbos. **JavaScript ir C#** Foundry Local SDK turi įmontuotą `AudioClient`, kuris vienu metodu apdoroja visą srautą (garso priekinis apdorojimas, koduotuvas, dekoduotuvas, ženklų dekodavimas). **Python** naudoja Foundry Local SDK modelio valdymui ir ONNX Runtime tiesioginiam koduotuvo/dekoduotuvo ONNX modeliui apdoroti.

| Komponentas | Python | JavaScript | C# |
|-------------|--------|------------|----|
| **SDK paketai** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Modelio valdymas** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalogas |
| **Savybių išgavimas** | `WhisperFeatureExtractor` + `librosa` | Tvarko SDK `AudioClient` | Tvarko SDK `AudioClient` |
| **Inferencija** | `ort.InferenceSession` (koduotuvas + dekoduotuvas) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Ženklų dekodavimas** | `WhisperTokenizer` | Tvarko SDK `AudioClient` | Tvarko SDK `AudioClient` |
| **Kalbos nustatymas** | Nustatoma per `forced_ids` dekodavime | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Įvestis** | WAV failo kelias | WAV failo kelias | WAV failo kelias |
| **Išvestis** | Dekoduotas tekstas | `result.text` | `result.Text` |

> **Svarbu:** Visada nustatykite kalbą `AudioClient` objekte (pvz. `"en"` anglų kalbai). Be aiškaus kalbos nustatymo modelis gali generuoti nesuprantamą išvestį, bandydamas automatiškai nustatyti kalbą.

> **SDK šablonai:** Python naudoja `FoundryLocalManager(alias)` pradžiai, tada `get_cache_location()` ONNX modelio failams surasti. JavaScript ir C# naudoja SDK įmontuotą `AudioClient`, jo gauna per `model.createAudioClient()` (JS) arba `model.GetAudioClientAsync()` (C#), kuris valdo visą transkripcijos srautą. Pilną aprašymą rasite [2 dalyje: Foundry Local SDK giluminis aprašymas](part2-foundry-local-sdk.md).

---

### Užduotis 3 – Sukurti paprastą transkripcijos programą

Pasirinkite savo kalbą ir sukurkite minimalų programos pavyzdį, kuris transkribuotų garso failą.

> **Palaikomi garso formatai:** WAV, MP3, M4A. Geriausiam rezultatui naudokite 16 kHz mėginių ėmimo dažnio WAV failus.

<details>
<summary><h3>Python</h3></summary>

#### Diegimas

```bash
cd python
python -m venv venv

# Suaktyvinti virtualią aplinką:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Transkripcijos kodas

Sukurkite failą `foundry-local-whisper.py`:

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

# 1 žingsnis: Bootstrap - paleidžia paslaugą, atsisiunčia ir įkelia modelį
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Sukurti kelią į kešą ONNX modelio failams
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# 2 žingsnis: Įkelti ONNX sesijas ir funkcijų išgavimo įrankį
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

# 3 žingsnis: Išgauti mel spektrogramų funkcijas
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# 4 žingsnis: Paleisti koduotoją
enc_out = encoder.run(None, {"audio_features": input_features})
# Pirmas rezultatas yra paslėptos būsenos; likusieji yra kryžminio dėmesio KV poros
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# 5 žingsnis: Autoregresinis dekodavimas
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, įrašyti, be laiko žymių
input_ids = np.array([initial_tokens], dtype=np.int32)

# Tuščias savidėmesio KV kešas
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

    if next_token == 50257:  # teksto pabaiga
        break
    generated.append(next_token)

    # Atnaujinti savidėmesio KV kešą
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Paleidimas

```bash
# Užrašykite Zava produkto scenarijų
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Arba išbandykite kitus:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Svarbiausi Python aspektai

| Metodas | Paskirtis |
|---------|-----------|
| `FoundryLocalManager(alias)` | Pradžia: paleidžia paslaugą, atsisiunčia ir įkelia modelį |
| `manager.get_cache_location()` | Gauk kelią į kešuotus ONNX modelio failus |
| `WhisperFeatureExtractor.from_pretrained()` | Įkelti mel spektrogramų savybių išgavėją |
| `ort.InferenceSession()` | Sukurti ONNX Runtime sesijas koduotojui ir dekoduotojui |
| `tokenizer.decode()` | Konvertuoti išeinančius tokenų ID į tekstą |

</details>

<details>
<summary><h3>JavaScript</h3></summary>

#### Diegimas

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Transkripcijos kodas

Sukurkite failą `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// 1 žingsnis: Bootstrap - sukurkite valdytoją, paleiskite paslaugą ir įkelkite modelį
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

// 2 žingsnis: Sukurkite garso klientą ir transkribuokite
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Išvalymas
await model.unload();
```

> **Pastaba:** Foundry Local SDK suteikia įmontuotą `AudioClient` per `model.createAudioClient()`, kuris viduje tvarko visą ONNX inferencijos srautą – nereikia importuoti `onnxruntime-node`. Visada nustatykite `audioClient.settings.language = "en"` tiksliai anglų kalbos transkripcijai.

#### Paleidimas

```bash
# Užrašykite Zava produkto scenarijų
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Arba išbandykite kitus:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Svarbiausi JavaScript aspektai

| Metodas | Paskirtis |
|---------|-----------|
| `FoundryLocalManager.create({ appName })` | Sukurti valdytojo singletą |
| `await catalog.getModel(alias)` | Gauti modelį iš katalogo |
| `model.download()` / `model.load()` | Atsisiųsti ir įkelti Whisper modelį |
| `model.createAudioClient()` | Sukurti garso klientą transkripcijai |
| `audioClient.settings.language = "en"` | Nustatyti transkripcijos kalbą (reikalinga tiksliam rezultatui) |
| `audioClient.transcribe(path)` | Transkribuoti garso failą, grąžina `{ text, duration }` |

</details>

<details>
<summary><h3>C#</h3></summary>

#### Diegimas

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Pastaba:** C# pavyzdyje naudojama `Microsoft.AI.Foundry.Local` biblioteka, kuri suteikia įmontuotą `AudioClient` per `model.GetAudioClientAsync()`. Tai leidžia vykdyti visą transkripcijos srautą procese – nereikia atskiro ONNX Runtime nustatymo.

#### Transkripcijos kodas

Pakeiskite `Program.cs` turinį:

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

#### Paleidimas

```bash
# Užrašykite Zava produkto scenarijų
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Arba išbandykite kitus:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Svarbiausi C# aspektai

| Metodas | Paskirtis |
|---------|-----------|
| `FoundryLocalManager.CreateAsync(config)` | Inicializuoja Foundry Local su konfigūracija |
| `catalog.GetModelAsync(alias)` | Gauti modelį iš katalogo |
| `model.DownloadAsync()` | Atsisiųsti Whisper modelį |
| `model.GetAudioClientAsync()` | Gauti `AudioClient` (ne ChatClient!) |
| `audioClient.Settings.Language = "en"` | Nustatyti transkripcijos kalbą (reikalinga tiksliam rezultatui) |
| `audioClient.TranscribeAudioAsync(path)` | Transkribuoti garso failą |
| `result.Text` | Ištranskribuotas tekstas |


> **C# vs Python/JS:** C# SDK suteikia įmontuotą `AudioClient` procesinio transkribavimo funkcijai per `model.GetAudioClientAsync()`, panašiai kaip JavaScript SDK. Python tiesiogiai naudoja ONNX Runtime, kad atliktų spėjimą naudodamas talpykloje esančius koduotojo/dekoduotojo modelius.

</details>

---

### Užduotis 4 - Visų Zava pavyzdžių masinis transkribavimas

Dabar, kai turite veikiantį transkribavimo programą, transkribuokite visus penkis Zava pavyzdžių failus ir palyginkite rezultatus.

<details>
<summary><h3>Python sekcija</h3></summary>

Pilnas pavyzdys `python/foundry-local-whisper.py` jau palaiko masinį transkribavimą. Paleidus be argumentų, jis transkribuoja visus `zava-*.wav` failus `samples/audio/` kataloge:

```bash
cd python
python foundry-local-whisper.py
```

Pavyzdyje naudojamas `FoundryLocalManager(alias)` inicijavimui, tada vykdomos koduotojo ir dekoduotojo ONNX sesijos kiekvienam failui.

</details>

<details>
<summary><h3>JavaScript sekcija</h3></summary>

Pilnas pavyzdys `javascript/foundry-local-whisper.mjs` jau palaiko masinį transkribavimą. Paleidus be argumentų, jis transkribuoja visus `zava-*.wav` failus `samples/audio/` kataloge:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Pavyzdyje naudojamas `FoundryLocalManager.create()` ir `catalog.getModel(alias)` SDK inicializavimui, tada naudojamas `AudioClient` (su `settings.language = "en"`) kiekvienam failui transkribuoti.

</details>

<details>
<summary><h3>C# sekcija</h3></summary>

Pilnas pavyzdys `csharp/WhisperTranscription.cs` jau palaiko masinį transkribavimą. Paleidus be nurodyto failo argumento, jis transkribuoja visus `zava-*.wav` failus `samples/audio/` kataloge:

```bash
cd csharp
dotnet run whisper
```

Pavyzdyje naudojamas `FoundryLocalManager.CreateAsync()` ir SDK `AudioClient` (su `Settings.Language = "en"`) procesiniam transkribavimui.

</details>

**Ką stebėti:** Palyginkite transkribavimo rezultatus su originaliu tekstu faile `samples/audio/generate_samples.py`. Kaip tiksliai Whisper užfiksuoja produktų pavadinimus kaip "Zava ProGrip" ir techninius terminus kaip "brushless motor" ar "composite decking"?

---

### Užduotis 5 - Suprasti pagrindinius kodo šablonus

Išnagrinėkite, kaip Whisper transkribavimas skiriasi nuo pokalbių papildymo visomis trimis kalbomis:

<details>
<summary><b>Python – pagrindiniai skirtumai nuo pokalbių</b></summary>

```python
# Pokalbio užbaigimas (2-6 dalys):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Garso transkripcija (ši dalis):
# Naudoja tiesiogiai ONNX Runtime, o ne OpenAI klientą
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregresinis dekoderio ciklas ...
print(tokenizer.decode(generated_tokens))
```

**Pagrindinė įžvalga:** Pokalbių modeliai naudoja OpenAI suderinamą API per `manager.endpoint`. Whisper naudoja SDK, kad rastų talpykloje esančius ONNX modelio failus, ir tiesiogiai vykdo spėjimą su ONNX Runtime.

</details>

<details>
<summary><b>JavaScript – pagrindiniai skirtumai nuo pokalbių</b></summary>

```javascript
// Pokalbio užbaigimas (2-6 dalys):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Garso transkripcija (Ši dalis):
// Naudoja SDK integruotą AudioClient
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Visada nurodykite kalbą geriausiems rezultatams
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Pagrindinė įžvalga:** Pokalbių modeliai naudoja OpenAI suderinamą API per `manager.urls[0] + "/v1"`. Whisper transkribavimas naudoja SDK `AudioClient`, gautą iš `model.createAudioClient()`. Nustatykite `settings.language`, kad išvengtumėte klaidingo automatinio atpažinimo.

</details>

<details>
<summary><b>C# – pagrindiniai skirtumai nuo pokalbių</b></summary>

C# požiūris naudoja SDK įmontuotą `AudioClient` procesiniam transkribavimui:

**Modelio inicializavimas:**

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

**Transkribavimas:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Pagrindinė įžvalga:** C# naudoja `FoundryLocalManager.CreateAsync()` ir tiesiogiai gauna `AudioClient` — nereikia jokių ONNX Runtime nustatymų. Nustatykite `Settings.Language`, kad išvengtumėte klaidingo automatinio atpažinimo.

</details>

> **Santrauka:** Python naudoja Foundry Local SDK modeliui valdyti ir ONNX Runtime tiesioginiam spėjimui prieš koduotojo/dekoduotojo modelius. JavaScript ir C# abi naudoja SDK įmontuotą `AudioClient` patogiam transkribavimui – sukurkite klientą, nustatykite kalbą ir iškvieskite `transcribe()` / `TranscribeAudioAsync()`. Visada nurodykite kalbos savybę AudioClient objektui norint gauti tikslius rezultatus.

---

### Užduotis 6 – Eksperimentuokite

Išbandykite šiuos pakeitimus, kad gilintumėte supratimą:

1. **Išbandykite skirtingus garso failus** – įrašykite save kalbant naudodami Windows Voice Recorder, išsaugokite kaip WAV ir transkribuokite

2. **Palyginkite modelių variantus** – jei turite NVIDIA GPU, išbandykite CUDA variantą:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Palyginkite transkribavimo greitį su CPU variantu.

3. **Pridėkite išvesties formatavimą** – JSON atsakyme gali būti:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Sukurkite REST API** – apjuoskite transkribavimo kodą interneto serveriu:

   | Kalba | Frameworkas | Pavyzdys |
   |-------|-------------|----------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` su `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` su `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` su `IFormFile` |

5. **Daugkartinis su transkribavimu** – sujunkite Whisper su pokalbių agentu iš 4 dalies: pirmiausia transkribuokite garsą, tada perduokite tekstą agentui analizei ar santraukai.

---

## SDK garso API nuoroda

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — sukuria `AudioClient` egzempliorių
> - `audioClient.settings.language` — nustato transkribavimo kalbą (pvz. `"en"`)
> - `audioClient.settings.temperature` — valdo atsitiktinumą (pasirinktinai)
> - `audioClient.transcribe(filePath)` — transkribuoja failą, grąžina `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — transkribavimo gabalėlius perduoda per atgalinį kvietimą
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — sukuria `OpenAIAudioClient` egzempliorių
> - `audioClient.Settings.Language` — nustato transkribavimo kalbą (pvz. `"en"`)
> - `audioClient.Settings.Temperature` — valdo atsitiktinumą (pasirinktinai)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transkribuoja failą, grąžina objektą su `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — grąžina `IAsyncEnumerable` transkribavimo gabalėlių

> **Patarimas:** Visada nustatykite kalbos savybę prieš transkribuojant. Be to, Whisper modelis bando automatiškai atpažinti kalbą, kas gali sukelti nesuprantamą išvestį (vienas pakeitimo simbolis vietoje teksto).

---

## Palyginimas: Pokalbių modeliai vs. Whisper

| Aspektas | Pokalbių modeliai (3-7 dalys) | Whisper – Python | Whisper – JS / C# |
|----------|------------------------------|-----------------|------------------|
| **Užduoties tipas** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Įvestis** | Teksto žinutės (JSON) | Garso failai (WAV/MP3/M4A) | Garso failai (WAV/MP3/M4A) |
| **Išvestis** | Sugeneruotas tekstas (srautinė) | Transkribuotas tekstas (pilnas) | Transkribuotas tekstas (pilnas) |
| **SDK paketas** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API metodas** | `client.chat.completions.create()` | ONNX Runtime tiesiogiai | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Kalbos nustatymas** | Nėra | Dekoderio pradžios žetonai | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Srautinis palaikymas** | Taip | Ne | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Privatumo nauda** | Kodas/duomenys lieka vietoje | Garso duomenys lieka vietoje | Garso duomenys lieka vietoje |

---

## Pagrindiniai pastebėjimai

| Sąvoka | Ko išmokote |
|--------|-------------|
| **Whisper įrenginyje** | Kalbos į tekstą apdorojama visiškai lokaliai, idealiai tinka transkribuoti Zava klientų skambučius ir produktų apžvalgas įrenginyje |
| **SDK AudioClient** | JavaScript ir C# SDK turi įmontuotą `AudioClient`, kuris vienu kvietimu atlieka visą transkribavimo procesą |
| **Kalbos nustatymas** | Visada nustatykite AudioClient kalbą (pvz. `"en"`) – be jos automatinis atpažinimas gali duoti klaidinančią išvestį |
| **Python** | Naudoja `foundry-local-sdk` modeliui valdyti + `onnxruntime` + `transformers` + `librosa` tiesioginiam ONNX spėjimui |
| **JavaScript** | Naudoja `foundry-local-sdk` su `model.createAudioClient()` – nustatykite `settings.language`, tada kvieskite `transcribe()` |
| **C#** | Naudoja `Microsoft.AI.Foundry.Local` su `model.GetAudioClientAsync()` – nustatykite `Settings.Language`, tada kvieskite `TranscribeAudioAsync()` |
| **Srautinio palaikymas** | JS ir C# SDK taip pat siūlo `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` transkribavimo gabalėliais išvestį |
| **CPU optimizavimas** | CPU variantas (3.05 GB) veikia bet kuriame Windows įrenginyje be GPU |
| **Privatumas pirmiausia** | Puikiai tinka laikyti Zava klientų sąveikas ir savavališką produktų duomenų apdorojimą vietoje |

---

## Ištekliai

| Išteklius | Nuoroda |
|-----------|---------|
| Foundry Local dokumentacija | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK nuoroda | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper modelis | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local svetainė | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Kitas žingsnis

Tęskite prie [10 dalies: Naudojant pasirinktinius arba Hugging Face modelius](part10-custom-models.md), kad sudėtumėte savo modelius iš Hugging Face ir paleistumėte juos per Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės apribojimas**:  
Šis dokumentas buvo išverstas naudojant dirbtinio intelekto vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors siekiame tikslumo, atkreipkite dėmesį, kad automatizuoti vertimai gali turėti klaidų ar netikslumų. Originalus dokumentas jo gimtąja kalba turėtų būti laikomas oficialiu šaltiniu. Kritinei informacijai rekomenduojamas profesionalus žmogiškas vertimas. Mes neprisiimame atsakomybės už bet kokius nesusipratimus ar klaidingas interpretacijas, kylančias naudojant šį vertimą.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->