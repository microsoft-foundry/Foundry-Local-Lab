![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 9: Hääle transkriptsioon Whisperi ja Foundry Localiga

> **Eesmärk:** Kasutada OpenAI Whisper mudelit, mis töötab lokaalselt Foundry Locali kaudu, et transkribeerida helifaile – täiesti seadmes, pilve ühendust pole vaja.

## Ülevaade

Foundry Local ei ole ainult teksti genereerimiseks; see toetab ka **häälest tekstiks** mudeleid. Selles laboris kasutad **OpenAI Whisper Medium** mudelit, et transkribeerida helifaile täielikult oma arvutis. See sobib ideaalselt olukordadele, kus tuleb transkribeerida Zava klienditeeninduse kõnesid, tooteülevaadete salvestusi või töötoa planeerimissessioone, kus helidata ei tohi kunagi seadmest väljuda.

---

## Õpieesmärgid

Selle labori lõpuks oskad:

- Mõista Whisper häälest tekstiks mudelit ja selle võimalusi
- Laadida alla ja käivitada Whisper mudelit kasutades Foundry Locali
- Transkribeerida helifaile Foundry Local SDK-ga Pythonis, JavaScriptis ja C#-s
- Luua lihtne transkriptsiooniteenus, mis töötab täielikult seadmes
- Mõista erinevusi chat-/tekstimudelite ja helimudelite vahel Foundry Localis

---

## Eeldused

| Nõue | Detailid |
|-------------|---------|
| **Foundry Local CLI** | Versioon **0.8.101 või uuem** (Whisper mudelid on saadaval alates v0.8.101) |
| **Operatsioonisüsteem** | Windows 10/11 (x64 või ARM64) |
| **Keele käituskeskkond** | **Python 3.9+** ja/või **Node.js 18+** ja/või **.NET 9 SDK** ([Laadi alla .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Läbitud** | [Osa 1: Alustamine](part1-getting-started.md), [Osa 2: Foundry Local SDK süvaülevaade](part2-foundry-local-sdk.md) ja [Osa 3: SDKd ja APId](part3-sdk-and-apis.md) |

> **Märkus:** Whisper mudeleid tuleb alla laadida läbi **SDK** (mitte CLI). CLI ei toeta helitranskriptsiooni lõpp-punkti. Kontrolli oma versiooni käsuga:
> ```bash
> foundry --version
> ```

---

## Kontseptsioon: Kuidas Whisper töötab Foundry Localiga

OpenAI Whisper mudel on üldotstarbeline kõnetuvastuse mudel, mis on treenitud suurel mitmekesise helimaterjaliga andmestikul. Kui see töötab läbi Foundry Locali:

- Mudel töötab **täielikult sinu CPU-l** – GPU-d pole vaja
- Heli ei jäta kunagi sinu seadet – **täielik privaatsus**
- Foundry Local SDK haldab mudeli allalaadimist ja vahemälu
- **JavaScript ja C#** pakuvad SDK-s sisseehitatud `AudioClient`i, mis tegeleb kogu transkriptsioonitoruga – ONNX seadistust pole vaja käsitsi teha
- **Python** kasutab SDK-d mudeli haldamiseks ja ONNX Runtime’i otseseks inference’ks encoder/decoder ONNX mudelitega

### Kuidas torujuhtme töö toimib (JavaScript ja C#) — SDK AudioClient

1. **Foundry Local SDK** laadib alla ja salvestab Whisper mudeli vahemällu
2. `model.createAudioClient()` (JS) või `model.GetAudioClientAsync()` (C#) loob `AudioClient`i
3. `audioClient.transcribe(path)` (JS) või `audioClient.TranscribeAudioAsync(path)` (C#) haldab kogu torujuhtme sisemisi protsesse — helitöötlus, encoder, decoder ja tokeni dekodeerimine
4. `AudioClient`il on `settings.language` omadus (mille saab seada `"en"` inglise keele jaoks), et transkriptsioon oleks täpsem

### Kuidas torujuhtme töö toimib (Python) — ONNX Runtime

1. **Foundry Local SDK** laadib alla ja salvestab Whisper ONNX mudelifailid vahemällu
2. **Helitöötlus** teisendab WAV heli mel spektribrogrammiks (80 mel vahemikku x 3000 kaadrit)
3. **Encoder** töötleb mel spektribrogrammi ja toodab peidetud seisundid ning risttaju võtme-/väärtusmatriksid
4. **Decoder** töötab autoregressiivselt, genereerides ühe tokeni korraga kuni lõputeksti tokenini
5. **Tokenisaator** dekodeerib väljund tokeni ID-d tagasi loetavaks tekstiks

### Whisperi mudelivariandid

| Alias | Mudeli ID | Seade | Suurus | Kirjeldus |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU kiirendusega (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU-le optimeeritud (soovitatav enamikele seadmetele) |

> **Märkus:** Erinevalt vaikimisi loetletavatest chat mudelitest on Whisper mudelid kategoriseeritud `automatic-speech-recognition` ülesande alla. Vaata üksikasju käsuga `foundry model info whisper-medium`.

---

## Laboratoorse töö harjutused

### Harjutus 0 - Hangi näidis helifailid

Labor sisaldab ettevalmistatud WAV-faile, mis põhinevad Zava DIY tootekohtadel. Loo need kaasasoleva skriptiga:

```bash
# Repopuu juurest - loo ja aktiveeri esmalt .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

See loob kuus WAV-faili kaustas `samples/audio/`:

| Fail | Stsenaarium |
|------|-------------|
| `zava-customer-inquiry.wav` | Klient küsib **Zava ProGrip juhtmevaba puurmasina** kohta |
| `zava-product-review.wav` | Klient annab tagasisidet **Zava UltraSmooth sisevärvi** kohta |
| `zava-support-call.wav` | Tugikõne **Zava TitanLock tööriistakapi** kohta |
| `zava-project-planning.wav` | Tee-ise projekt plaane rõdu jaoks kasutades **Zava EcoBoard komposiitmaterjali** |
| `zava-workshop-setup.wav` | Töötuba kõigi **viie Zava tootega** tutvustuseks |
| `zava-full-project-walkthrough.wav` | Pikem garaaži renoveerimise läbivaatus kõigi Zava toodetega (~4 minutit, pikkade helide testimiseks) |

> **Nipp:** Võid kasutada ka oma WAV/MP3/M4A faile või salvestada end Windows Voice Recorderiga.

---

### Harjutus 1 - Laadi Whisper mudel SDK-ga alla

Kuna uuemates Foundry Local versioonides ei toeta CLI Whisper mudelitega töötamist, kasuta mudeli allalaadimiseks ja laadimiseks **SDK-d**. Vali oma keel:

<details>
<summary><b>🐍 Python</b></summary>

**Paigalda SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Käivita teenus
manager = FoundryLocalManager()
manager.start_service()

# Kontrolli kataloogi teavet
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Kontrolli, kas juba vahemällu salvestatud
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Laadi mudel mällu
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Salvesta faili nimega `download_whisper.py` ja käivita:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Paigalda SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Loo haldur ja alusta teenust
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Hangi mudel kataloogist
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

// Laadi mudel mällu
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Salvesta faili nimega `download-whisper.mjs` ja käivita:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Paigalda SDK:**
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

> **Miks SDK, mitte CLI?** Foundry Local CLI ei toeta Whisper mudelite otsest allalaadimist või võimalust neid teenindada. SDK pakub usaldusväärset viisi audio mudelite programmimiseks allalaadimiseks ja haldamiseks. JavaScripti ja C# SDK sisaldavad sisseehitatud `AudioClient`i, mis tegeleb kogu transkriptsioonitoruga. Python kasutab ONNX Runtime’i otseseks inference’ks vahemällu salvestatud mudelifailide kallal.

---

### Harjutus 2 - Mõista Whisper SDK-d

Whisper transkriptsioon kasutab keelte lõikes erinevaid lähenemisi. **JavaScript ja C#** pakuvad Foundry Local SDK-s sisseehitatud `AudioClient`i, mis haldab kogu torujuhtme (helitöötlus, encoder, decoder, tokeni dekodeerimine) ühe meetodi kaudu. **Python** kasutab Foundry Local SDK-d mudelihalduseks ja ONNX Runtime’i encoder-/decoder ONNX mudelite otseseks inference’ks.

| Komponent | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK paketid** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Mudeli haldus** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + kataloog |
| **Tunnuste ekstraheerimine** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient`i poolt hallatud | SDK `AudioClient`i poolt hallatud |
| **Inference** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Tokenite dekodeerimine** | `WhisperTokenizer` | SDK `AudioClient`i poolt hallatud | SDK `AudioClient`i poolt hallatud |
| **Keele seadistus** | Seatakse `forced_ids` kaudu decoder tokenites | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Sisend** | WAV faili tee | WAV faili tee | WAV faili tee |
| **Väljund** | Dekodeeritud tekst | `result.text` | `result.Text` |

> **Oluline:** Sea alati `AudioClienti` keel (nt `"en"` inglise jaoks). Keeleseadistuseta võib mudel toota ebaõiget väljundit, püüdes automaatselt keelt tuvastada.

> **SDK muster:** Python kasutab `FoundryLocalManager(alias)` algatamiseks ja seejärel `get_cache_location()` ONNX mudelifailide leidmiseks. JavaScript ja C# kasutavad SDK sisseehitatud `AudioClient`i — mida saab `model.createAudioClient()` (JS) või `model.GetAudioClientAsync()` (C#) kaudu — mis haldab täielikku transkriptsioonitoru. Täpsem info [Osa 2: Foundry Local SDK süvaülevaade](part2-foundry-local-sdk.md).

---

### Harjutus 3 - Ehita lihtne transkriptsioonirakendus

Vali endale sobiv keel ja loo minimaalne rakendus, mis transkribeerib helifaili.

> **Toetatavad helivormingud:** WAV, MP3, M4A. Parim tulemus saavutatakse 16kHz proovivõtusagedusega WAV-failidega.

<details>
<summary><h3>Python rada</h3></summary>

#### Eeltingimused

```bash
cd python
python -m venv venv

# Aktiveeri virtuaalne keskkond:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Transkriptsioonikood

Loo fail `foundry-local-whisper.py`:

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

# Samm 1: Bootstrap - käivitab teenuse, allalaadib ja laadib mudeli
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Koosta tee vahemällu salvestatud ONNX mudelifailidele
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Samm 2: Lae ONNX sessioonid ja omaduste väljavõtja
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

# Samm 3: Eemalda mel-spektrogrammi omadused
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Samm 4: Käivita kodeerija
enc_out = encoder.run(None, {"audio_features": input_features})
# Esimene väljund on peidetud olekud; ülejäänud on rist-tähelepanu KV paarid
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Samm 5: Autoregressiivne dekodeerimine
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transkribeeri, ilma ajastusteta
input_ids = np.array([initial_tokens], dtype=np.int32)

# Tühi ise-tähelepanu KV vahemälu
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

    if next_token == 50257:  # teksti lõpp
        break
    generated.append(next_token)

    # Uuenda ise-tähelepanu KV vahemälu
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Käivita see

```bash
# Kirjuta ümber Zava toote stsenaarium
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Või proovi teisi:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Olulised Python punktid

| Meetod | Eesmärk |
|--------|---------|
| `FoundryLocalManager(alias)` | Algatamine: käivitab teenuse, laeb alla ja laadib mudeli |
| `manager.get_cache_location()` | Leiab vahemällu salvestatud ONNX mudelifailide asukoha |
| `WhisperFeatureExtractor.from_pretrained()` | Laeb mel spektribrogrammi tunnuste ekstraheerija |
| `ort.InferenceSession()` | Loob ONNX Runtime sessioonid encoderi ja decoderi jaoks |
| `tokenizer.decode()` | Muudab väljund tokeni ID-d tekstiks |

</details>

<details>
<summary><h3>JavaScript rada</h3></summary>

#### Eeltingimused

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Transkriptsioonikood

Loo fail `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Samm 1: Bootstrap - loo haldur, käivita teenus ja laadi mudel
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

// Samm 2: Loo audio klient ja transkribeeri
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Puhasta
await model.unload();
```

> **Märkus:** Foundry Local SDK pakub sisseehitatud `AudioClient`i läbi `model.createAudioClient()`, mis haldab kogu ONNX inference torujuhet – eraldi `onnxruntime-node` importi pole vaja. Sea alati `audioClient.settings.language = "en"`, et tagada täpne ingliskeelne transkriptsioon.

#### Käivita see

```bash
# Transkribeeri Zava toote stsenaarium
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Või proovi teisi:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Olulised JavaScripti punktid

| Meetod | Eesmärk |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Loob juhtivateenuse singletoni |
| `await catalog.getModel(alias)` | Hangib mudeli kataloogist |
| `model.download()` / `model.load()` | Laeb alla ja laadib Whisper mudeli |
| `model.createAudioClient()` | Loob transkriptsiooniks audio kliendi |
| `audioClient.settings.language = "en"` | Seadistab keele (täpseks väljundiks vajalik) |
| `audioClient.transcribe(path)` | Transkribeerib helifaili, tagastab `{ text, duration }` |

</details>

<details>
<summary><h3>C# rada</h3></summary>

#### Eeltingimused

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Märkus:** C# rada kasutab `Microsoft.AI.Foundry.Local` paketti, mis omab sisseehitatud `AudioClient`i läbi `model.GetAudioClientAsync()`. See haldab kogu transkriptsioonitoru lokaalselt – ONNX Runtime’i eraldi seadistust pole vaja.

#### Transkriptsioonikood

Asenda faili `Program.cs` sisu:

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

#### Käivita see

```bash
# Kirjuta Zava toote stsenaarium
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Või proovi teisi:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Olulised C# punktid

| Meetod | Eesmärk |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Algatab Foundry Locali koos konfiguratsiooniga |
| `catalog.GetModelAsync(alias)` | Hangib mudeli kataloogist |
| `model.DownloadAsync()` | Laeb alla Whisper mudeli |
| `model.GetAudioClientAsync()` | Hangib AudioClienti (mitte ChatClienti!) |
| `audioClient.Settings.Language = "en"` | Seadistab keele (täpseks väljundiks vajalik) |
| `audioClient.TranscribeAudioAsync(path)` | Transkribeerib helifaili |
| `result.Text` | Transkribeeritud tekst |


> **C# vs Python/JS:** C# SDK pakub sisseehitatud `AudioClient`-i protsessi sees toimuvaks transkriptsiooniks `model.GetAudioClientAsync()` abil, sarnaselt JavaScript SDK-le. Python kasutab ONNX Runtime'i otse ennustamiseks vahemällu salvestatud kodeerija/dekodeerija mudelite vastu.

</details>

---

### Harjutus 4 - Kõikide Zava näidiste hulgiline transkriptsioon

Nüüd, kui sul on töötab transkriptsiooni rakendus, transkribeeri kõik viis Zava näidiste faili ja võrdle tulemusi.

<details>
<summary><h3>Python rajahoidja</h3></summary>

Täismahus näide `python/foundry-local-whisper.py` toetab juba hulgilõiku transkriptsiooni. Kui seda käivitada ilma argumentideta, transkribeeritakse kõik `samples/audio/` kaustas olevad `zava-*.wav` failid:

```bash
cd python
python foundry-local-whisper.py
```

Näide kasutab käivitamiseks `FoundryLocalManager(alias)`, seejärel täidab kodeerija ja dekodeerija ONNX sessioonid iga faili kohta.

</details>

<details>
<summary><h3>JavaScript rajahoidja</h3></summary>

Täismahus näide `javascript/foundry-local-whisper.mjs` toetab juba hulgilõiku transkriptsiooni. Kui seda käivitada ilma argumentideta, transkribeeritakse kõik `samples/audio/` kaustas olevad `zava-*.wav` failid:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Näide kasutab SDK initsialiseerimiseks `FoundryLocalManager.create()` ja `catalog.getModel(alias)`, seejärel kasutab transkriptsiooniks `AudioClient`-i (seades `settings.language = "en"`).

</details>

<details>
<summary><h3>C# rajahoidja</h3></summary>

Täismahus näide `csharp/WhisperTranscription.cs` toetab juba hulgilõiku transkriptsiooni. Kui seda käivitada ilma konkreetse faili argumendita, transkribeeritakse kõik `samples/audio/` kaustas olevad `zava-*.wav` failid:

```bash
cd csharp
dotnet run whisper
```

Näide kasutab `FoundryLocalManager.CreateAsync()` ja SDK `AudioClient`-i (seades `Settings.Language = "en"`) protsessi sees toimuvaks transkriptsiooniks.

</details>

**Mida jälgida:** Võrdle transkriptsiooni väljundit originaaltekstiga failis `samples/audio/generate_samples.py`. Kui täpselt Whisper tabab tootenimesid nagu "Zava ProGrip" ja tehnilisi termineid nagu "brushless motor" või "composite decking"?

---

### Harjutus 5 - Sõnumikoodi mustrite mõistmine

Uuri, kuidas Whisperi transkriptsioon erineb juturobotitest kõigis kolmes keeles:

<details>
<summary><b>Python – võtmeerinevused juturobotist</b></summary>

```python
# Vestluse täiendamine (osad 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Audio transkriptsioon (see osa):
# Kasutab otse ONNX Runtime'i, mitte OpenAI klienti
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregressiivne dekoodri tsükkel ...
print(tokenizer.decode(generated_tokens))
```

**Võtmetolmu:** Juturobotite mudelid kasutavad OpenAI ühilduvat API-d `manager.endpoint` kaudu. Whisper kasutab SDK-d, et leida vahemällu salvestatud ONNX mudelfailid, ja käivitab ennustamise otse ONNX Runtime’iga.

</details>

<details>
<summary><b>JavaScript – võtmeerinevused juturobotist</b></summary>

```javascript
// Vestluse lõpetamine (osad 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Hääle ülekirjutus (see osa):
// Kasutab SDK sisseehitatud AudioClienti
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Parimate tulemuste saamiseks seadke alati keel
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Võtmetolmu:** Juturobotite mudelid kasutavad OpenAI ühilduvat API-d aadressil `manager.urls[0] + "/v1"`. Whisperi transkriptsioon kasutab SDK `AudioClient`-i, mida hangitakse `model.createAudioClient()` kaudu. Seadista `settings.language`, et vältida automaattuvastusel tekkivat kribu.

</details>

<details>
<summary><b>C# – võtmeerinevused juturobotist</b></summary>

C# kasutab SDK sisseehitatud `AudioClient`-i protsessi sees toimuvaks transkriptsiooniks:

**Mudeli initsialiseerimine:**

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

**Transkriptsioon:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Võtmetolmu:** C# kasutab `FoundryLocalManager.CreateAsync()` ja hangib `AudioClient` otse — ONNX Runtime’i sätted pole vajalikud. Seadista `Settings.Language`, et vältida automaattuvastusel tekkivat kribu.

</details>

> **Kokkuvõte:** Python kasutab mudelite haldamiseks Foundry Local SDK-d ja ennustamiseks ONNX Runtime’i, töötades otse kodeerija/dekodeerija mudelitega. JavaScript ja C# kasutavad mõlemad SDK sisseehitatud `AudioClient`-i sujuvaks transkriptsiooniks — loo klient, sea keel ja kutsu `transcribe()` / `TranscribeAudioAsync()`. Keeleseadet AudioClient'il tuleb alati määrata täpsemate tulemuste saamiseks.

---

### Harjutus 6 – Katseta

Proovi neid muudatusi, et süvendada oma arusaamist:

1. **Proovi erinevaid helifaile** – salvestada end kõnelemas Windows Voice Recorderiga, salvesta WAV-ina ja transkribeeri

2. **Võrdle mudelivariante** – kui sul on NVIDIA GPU, proovi CUDA varianti:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Võrdle CPU variandiga transkriptsiooni kiirust.

3. **Lisa väljundi vormindus** – JSON-vastus võib sisaldada:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Loo REST API** – pane oma transkriptsioonikood veebiserverisse:

   | Keel | Raamistik | Näide |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` koos `UploadFile`-iga |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` koos `multer`-iga |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` koos `IFormFile`-iga |

5. **Mitme vooru vestlus koos transkriptsiooniga** – ühenda Whisper osaga vestlusagendist (osa 4): kõigepealt transkribeeri heli, seejärel edasta tekst analüüsi või kokkuvõtte tegemiseks agendile.

---

## SDK Audio API viited

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — loob `AudioClient` näite
> - `audioClient.settings.language` — sea transkriptsiooni keel (nt `"en"`)
> - `audioClient.settings.temperature` — juhuslikkuse kontroll (valikuline)
> - `audioClient.transcribe(filePath)` — transkribeeri fail, tagastab `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — voogedasta transkriptsiooni tükke läbi tagasikutse
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — loob `OpenAIAudioClient` näite
> - `audioClient.Settings.Language` — sea transkriptsiooni keel (nt `"en"`)
> - `audioClient.Settings.Temperature` — juhuslikkuse kontroll (valikuline)
> - `await audioClient.TranscribeAudioAsync(filePath)` — transkribeeri fail, tagastab objekti koos `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — tagastab `IAsyncEnumerable` transkriptsiooni tükkidest

> **Nipp:** Sea keeleseade enne transkribeerimist alati paika. Ilma selleta proovib Whisper automaatselt keelt tuvastada, mis võib anda segase väljundi (üks asendusmärk tekstireale asemel).

---

## Võrdlus: juturobotite mudelid vs Whisper

| Aspekt | Juturobotite mudelid (osa 3-7) | Whisper – Python | Whisper – JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Ülesande tüüp** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Sisend** | Tekstisõnumid (JSON) | Helifailid (WAV/MP3/M4A) | Helifailid (WAV/MP3/M4A) |
| **Väljund** | Genereeritud tekst (voogedastus) | Transkribeeritud tekst (täielik) | Transkribeeritud tekst (täielik) |
| **SDK pakett** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API meetod** | `client.chat.completions.create()` | ONNX Runtime otse | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Keele seadistus** | Puudub | Dekodeerija sissetulevad tokenid | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Voogedastus** | Jah | Ei | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Privaatsuse eelis** | Kood/andmed jäävad kohalikuks | Helimaterjal jääb kohalikuks | Helimaterjal jääb kohalikuks |

---

## Peamised õppetunnid

| Mõiste | Mida sa õppisid |
|---------|-----------------|
| **Whisper seadmes** | Kõnetekstiks muutmine toimub täielikult lokaalselt, ideaalne Zava kliendikõnede ja tootearvustuste seadmes töötlemiseks |
| **SDK AudioClient** | JavaScript ja C# SDK-d pakuvad sisseehitatud `AudioClient`-i, mis haldab transkriptsiooniprotsessi ühe kõnega |
| **Keele seadistus** | Sea alati AudioClient'i keel (nt `"en"`) — ilma selleta võib automaattuvastus anda ebaõigeid tulemusi |
| **Python** | Kasutab mudelite haldamiseks `foundry-local-sdk` + `onnxruntime` + `transformers` + `librosa` otse ONNX ennustamiseks |
| **JavaScript** | Kasutab `foundry-local-sdk` koos `model.createAudioClient()` — sea keel, siis kutsu `transcribe()` |
| **C#** | Kasutab `Microsoft.AI.Foundry.Local` koos `model.GetAudioClientAsync()` — sea keel, siis kutsu `TranscribeAudioAsync()` |
| **Voogedastuse tugi** | JS ja C# SDK pakuvad ka `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` tükikaupa väljundi saamiseks |
| **CPU-optimiseeritud** | CPU variant (3,05 GB) töötab mis tahes Windowsi seadmes ilma GPU-ta |
| **Privaatsus esikohal** | Täiuslik Zava kliendisuhtluse ja ärisaladuste hoimiseks seadmes |

---

## Ressursid

| Ressurss | Link |
|----------|------|
| Foundry Local dokumentatsioon | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK viited | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper mudel | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local veebisait | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Järgmine samm

Jätka [osa 10: kohandatud või Hugging Face mudelite kasutamine](part10-custom-models.md), et kompileerida enda mudeleid Hugging Face’ist ja käivitada neid Foundry Local-is.