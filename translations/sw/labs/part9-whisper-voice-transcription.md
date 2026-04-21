![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Sehemu ya 9: Ubadilishaji wa Sauti kwa Maandishi kwa kutumia Whisper na Foundry Local

> **Lengo:** Tumia modeli ya OpenAI Whisper inayotendeka kifaa kwa kutumia Foundry Local kubadilisha mafaili ya sauti kuwa maandishi - kikamilifu kwenye kifaa, bila hitaji la wingu.

## Muhtasari

Foundry Local si kwa ajili ya kizazi cha maandishi tu; pia inaunga mkono modeli za **kugeuza hotuba kuwa maandishi**. Katika maabara hii utatumia modeli ya **OpenAI Whisper Medium** kubadilisha mafaili ya sauti kikamilifu kwenye kompyuta yako. Hii ni bora kwa hali kama vile kurekodi mazungumzo ya huduma kwa wateja wa Zava, kurekodi maoni ya bidhaa, au mikutano ya upangaji wa warsha ambapo data ya sauti haipaswi kutoka kwenye kifaa chako.

---

## Malengo ya Kujifunza

Mwisho wa maabara hii utaweza:

- Kuelewa modeli ya Whisper ya kugeuza hotuba kuwa maandishi na uwezo wake
- Kushusha na kuendesha modeli ya Whisper kutumia Foundry Local
- Kubadilisha mafaili ya sauti kwa kutumia Foundry Local SDK kwa Python, JavaScript, na C#
- Kujenga huduma rahisi ya uandishi wa sauti inayotendeka kikamilifu kwenye kifaa
- Kuelewa tofauti kati ya modeli za mazungumzo/maandishi na modeli za sauti katika Foundry Local

---

## Masharti ya Awali

| Sharti | Maelezo |
|-------------|---------|
| **Foundry Local CLI** | Toleo **0.8.101 au zaidi** (Modeli za Whisper zinapatikana kuanzia v0.8.101 na kuendelea) |
| **MFumo wa uendeshaji** | Windows 10/11 (x64 au ARM64) |
| **Lugha ya runtime** | **Python 3.9+** na/au **Node.js 18+** na/au **.NET 9 SDK** ([Pakua .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Imekamilika** | [Sehemu ya 1: Kuanza](part1-getting-started.md), [Sehemu ya 2: Udhibiti wa SDK ya Foundry Local](part2-foundry-local-sdk.md), na [Sehemu ya 3: SDKs na APIs](part3-sdk-and-apis.md) |

> **Kumbuka:** Modeli za Whisper lazima zishushwe kupitia **SDK** (sio CLI). CLI haitegemee huduma ya kubadilisha sauti kuwa maandishi. Angalia toleo lako kwa:
> ```bash
> foundry --version
> ```

---

## Dhana: Jinsi Whisper Inavyofanya kazi na Foundry Local

Modeli ya OpenAI Whisper ni modeli ya utambuzi wa hotuba ya matumizi mengi iliyoanzishwa kwa seti kubwa ya sauti mbalimbali. Inapotekelezwa kupitia Foundry Local:

- Modeli inafanya kazi **kikamilifu kwenye CPU yako** - haihitaji GPU
- Sauti haiondoki kifaa chako - **usalama wa faragha kamili**
- Foundry Local SDK hushughulikia kupakua modeli na usimamizi wa hifadi
- **JavaScript na C#** hutoa `AudioClient` iliyojengwa ndani ya SDK inayoshughulikia njia kamili ya kubadilisha - haidhamini usanidi wa ONNX kwa mikono
- **Python** hutumia SDK kwa usimamizi wa modeli na ONNX Runtime kwa uchambuzi wa moja kwa moja dhidi ya modeli za encoder/decoder za ONNX

### Jinsi Mtiririko Unavyofanya Kazi (JavaScript na C#) — SDK AudioClient

1. **Foundry Local SDK** inapakua na kuhifadhi modeli ya Whisper
2. `model.createAudioClient()` (JS) au `model.GetAudioClientAsync()` (C#) huunda `AudioClient`
3. `audioClient.transcribe(path)` (JS) au `audioClient.TranscribeAudioAsync(path)` (C#) inashughulikia mtiririko mzima ndani — uandaji wa sauti, encoder, decoder, na ukusanyaji wa tokeni
4. `AudioClient` inaonyesha mali ya `settings.language` (iwekwa kuwa `"en"` kwa Kiingereza) kuongoza ubadilishaji sahihi

### Jinsi Mtiririko Unavyofanya Kazi (Python) — ONNX Runtime

1. **Foundry Local SDK** inapakua na kuhifadhi mafaili ya modeli ya Whisper ONNX
2. **Uandaji wa awali wa sauti** hubadilisha sauti ya WAV kuwa mel spectrogram (80 mel bins x fremu 3000)
3. **Encoder** hushughulikia mel spectrogram na kutoa hali fiche pamoja na tensors za ufunguo/thamani za utambuzi msalaba
4. **Decoder** inafanya kazi kwa kujiendesha yenyewe, ikizalisha tokeni moja kwa moja hadi itoke tokeni ya mwisho wa maandishi
5. **Tokeniser** hubadilisha tokeni zilizotolewa kurudi kuwa maandishi yanayoweza kusomwa

### Aina za Modeli za Whisper

| Jina la Kijulishaji | Kitambulisho cha Modeli | Kifaa | Ukubwa | Maelezo |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Kuawezesha GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Imeboreshwa kwa CPU (inapendekezwa kwa vifaa vingi) |

> **Kumbuka:** Tofauti na modeli za mazungumzo zinazoorodheshwa kwa chaguo-msingi, modeli za Whisper zipo chini ya jukumu la `automatic-speech-recognition`. Tumia `foundry model info whisper-medium` kuona maelezo.

---

## Mazoezi ya Maabara

### Zoefzoezi 0 - Pata Mafaili ya Sauti ya Mfano

Maabara hii inajumuisha mafaili ya WAV yaliyotengenezwa awali kulingana na hali za bidhaa za Zava DIY. Yayotengeneza kwa kutumia skripti iliyojumuishwa:

```bash
# Kutoka kwenye mzizi wa repo - tengeneza na uanzishe .venv kwanza
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Hii inaunda mafaili sita ya WAV katika `samples/audio/`:

| Faili | Tukio |
|------|----------|
| `zava-customer-inquiry.wav` | Mteja akiuliza kuhusu **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | Mteja akitathmini **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | Simu ya usaidizi kuhusu **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | Mfanyabiashara anapanga uashi wa ghorofa na **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Mwongozo wa warsha ukitumia **bidhaa zote tano za Zava** |
| `zava-full-project-walkthrough.wav` | Mwongozo wa mrekebishaji wa gereji ukitumia **bidhaa zote za Zava** (~dakika 4, kwa majaribio ya sauti ndefu) |

> **Ushauri:** Unaweza pia kutumia mafaili yako mwenyewe ya WAV/MP3/M4A, au kujirekodi kwa kutumia Windows Voice Recorder.

---

### Zoefzoezi 1 - Pakua Modeli ya Whisper Kutumia SDK

Kutokana na ukosefu wa usaidizi wa CLI kwa modeli za Whisper katika matoleo mapya ya Foundry Local, tumia **SDK** kupakua na kupakia modeli. Chagua lugha yako:

<details>
<summary><b>🐍 Python</b></summary>

**Sakinisha SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Anza huduma
manager = FoundryLocalManager()
manager.start_service()

# Angalia taarifa ya orodha
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Angalia kama tayari imehifadhiwa
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Pakia mfano kwenye kumbukumbu
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Hifadhi kama `download_whisper.py` na endesha:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Sakinisha SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Tengeneza meneja na anza huduma
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Pata mfano kutoka kwenye katalogi
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

// Pakia mfano kwenye kumbukumbu
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Hifadhi kama `download-whisper.mjs` na endesha:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Sakinisha SDK:**
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

> **Kwa nini SDK badala ya CLI?** CLI ya Foundry Local haitegemezi kupakua au kuhudumia modeli za Whisper moja kwa moja. SDK inatoa njia ya kuaminika ya kupakua na kusimamia modeli za sauti kwa mpangilio wa programu. SDK za JavaScript na C# zina `AudioClient` iliyojengwa ndani inayoshughulikia mtiririko mzima wa uandishi. Python hutumia ONNX Runtime kwa uchambuzi wa moja kwa moja dhidi ya mafaili ya modeli yaliyohifadhiwa.

---

### Zoefzoezi 2 - Elewa SDK ya Whisper

Ubadilishaji wa Whisper hutumia njia tofauti kulingana na lugha. **JavaScript na C#** hutoa `AudioClient` iliyojengwa ndani ya SDK ya Foundry Local inayoshughulikia mtiririko mzima (uandaji wa sauti, encoder, decoder, ukusanyaji wa tokeni) kwa wito mmoja wa kifuniko. **Python** hutumia SDK ya Foundry Local kwa usimamizi wa modeli na ONNX Runtime kwa uchambuzi wa moja kwa moja dhidi ya modeli za encoder/decoder za ONNX.

| Kipengele | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **Vipakiti vya SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Usimamizi wa modeli** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + katalogi |
| **Mchambuzi wa sifa** | `WhisperFeatureExtractor` + `librosa` | Imesimamiwa na SDK `AudioClient` | Imesimamiwa na SDK `AudioClient` |
| **Uchambuzi** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Ukusanyaji tokeni** | `WhisperTokenizer` | Imesimamiwa na SDK `AudioClient` | Imesimamiwa na SDK `AudioClient` |
| **Kuweka lugha** | Weka kupitia `forced_ids` katika tokeni za decoder | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Ingizo** | Njia ya faili la WAV | Njia ya faili la WAV | Njia ya faili la WAV |
| **Matokeo** | Mstari wa maandishi yaliyotafsiriwa | `result.text` | `result.Text` |

> **Muhimu:** Daima weka lugha kwenye `AudioClient` (mfano `"en"` kwa Kiingereza). Bila kuweka lugha wazi, modeli inaweza kutoa matokeo yasiyoeleweka ikijaribu kutambua lugha kwa auto.

> **Mifumo ya SDK:** Python hutumia `FoundryLocalManager(alias)` kuanzisha, kisha `get_cache_location()` kupata mafaili ya modeli ya ONNX yaliyohifadhiwa. JavaScript na C# hutumia `AudioClient` iliyojengwa ndani ya SDK — inayopatikana kupitia `model.createAudioClient()` (JS) au `model.GetAudioClientAsync()` (C#) — inayoshughulikia mtiririko mzima wa uandishi. Angalia [Sehemu ya 2: Udhibiti wa SDK ya Foundry Local](part2-foundry-local-sdk.md) kwa maelezo kamili.

---

### Zoefzoezi 3 - Jenga Programu Rahisi ya Ubadilishaji

Chagua lugha yako na tengeneza programu ndogo inayobadilisha faili la sauti kuwa maandishi.

> **Muundo wa sauti unaotegemea:** WAV, MP3, M4A. Kwa matokeo bora, tumia mafaili ya WAV yenye sampuli ya 16kHz.

<details>
<summary><h3>Njia ya Python</h3></summary>

#### Usanidi

```bash
cd python
python -m venv venv

# Washa mazingira pepe:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Msimbo wa Ubadilishaji

Unda faili `foundry-local-whisper.py`:

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

# Hatua ya 1: Bootstrapping - anza huduma, pakua, na pakia mfano
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Tengeneza njia ya faili za mfano wa ONNX zilizohifadhiwa
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Hatua ya 2: Pakua vikao vya ONNX na kivunja sifa
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

# Hatua ya 3: Chukua sifa za mel spectrogram
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Hatua ya 4: Endesha encoder
enc_out = encoder.run(None, {"audio_features": input_features})
# Matokeo ya kwanza ni hali zilizofichwa; zilizobaki ni jozi za KV za usikivu wa msalaba
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Hatua ya 5: Ukodishaji wa autoregressive
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, andika, bila alama za wakati
input_ids = np.array([initial_tokens], dtype=np.int32)

# Hifadhi tupu ya KV ya usikivu wa binafsi
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

    if next_token == 50257:  # mwisho wa maandishi
        break
    generated.append(next_token)

    # Sasisha hifadhi ya KV ya usikivu wa binafsi
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Kuendesha

```bash
# Andika hali ya bidhaa ya Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Au jaribu zingine:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Muhimu wa Python

| Njia | Kusudi |
|--------|---------|
| `FoundryLocalManager(alias)` | Kuanzisha: anza huduma, pakua, na pakia modeli |
| `manager.get_cache_location()` | Pata njia ya mafaili ya modeli yaliyohifadhiwa ya ONNX |
| `WhisperFeatureExtractor.from_pretrained()` | Pakia kichakataji cha sifa cha mel spectrogram |
| `ort.InferenceSession()` | Unda vikao vya ONNX Runtime kwa encoder na decoder |
| `tokenizer.decode()` | Badilisha tokeni za matokeo kurudi kuwa maandishi |

</details>

<details>
<summary><h3>Njia ya JavaScript</h3></summary>

#### Usanidi

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Msimbo wa Ubadilishaji

Unda faili `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Hatua ya 1: Anza msingi - tengeneza meneja, anza huduma, na pakia modeli
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

// Hatua ya 2: Tengeneza mteja wa sauti na tengeneza manukuu
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Safisha
await model.unload();
```

> **Kumbuka:** SDK ya Foundry Local hutoa `AudioClient` iliyojengwa ndani kupitia `model.createAudioClient()` inayoshughulikia mtiririko mzima wa ONNX ndani — haijahitajika kuingiza `onnxruntime-node`. Daima weka `audioClient.settings.language = "en"` ili kuhakikisha ubadilishaji sahihi wa Kiingereza.

#### Kuendesha

```bash
# Andika tena hali ya bidhaa ya Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Au jaribu zingine:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Muhimu wa JavaScript

| Njia | Kusudi |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Unda usimamizi singleton |
| `await catalog.getModel(alias)` | Pata modeli kutoka katalogi |
| `model.download()` / `model.load()` | Pakua na pakuia modeli ya Whisper |
| `model.createAudioClient()` | Unda mteja wa sauti kwa ubadilishaji |
| `audioClient.settings.language = "en"` | Weka lugha ya ubadilishaji (inahitajika kwa matokeo sahihi) |
| `audioClient.transcribe(path)` | Badilisha faili la sauti, hurudisha `{ text, duration }` |

</details>

<details>
<summary><h3>Njia ya C#</h3></summary>

#### Usanidi

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Kumbuka:** Njia ya C# hutumia kifurushi `Microsoft.AI.Foundry.Local` kinachotoa `AudioClient` iliyojengwa ndani kupitia `model.GetAudioClientAsync()`. Hii inashughulikia mtiririko mzima wa uandishi ndani ya mchakato — haina hitaji la usanidi tofauti wa ONNX Runtime.

#### Msimbo wa Ubadilishaji

Badilisha yaliyomo ya `Program.cs`:

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

#### Kuendesha

```bash
# Andika hali ya bidhaa ya Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Au jaribu zingine:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Muhimu wa C#

| Njia | Kusudi |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Anzisha Foundry Local na usanidi |
| `catalog.GetModelAsync(alias)` | Pata modeli kutoka katalogi |
| `model.DownloadAsync()` | Pakua modeli ya Whisper |
| `model.GetAudioClientAsync()` | Pata AudioClient (si ChatClient!) |
| `audioClient.Settings.Language = "en"` | Weka lugha ya ubadilishaji (inahitajika kwa matokeo sahihi) |
| `audioClient.TranscribeAudioAsync(path)` | Badilisha faili la sauti |
| `result.Text` | Maandishi yaliyorejeshwa |


> **C# vs Python/JS:** SDK ya C# hutoa `AudioClient` iliyojengwa ndani kwa uandishi wa maneno kwa kutumia mchakato mmoja kupitia `model.GetAudioClientAsync()`, kama vile SDK ya JavaScript. Python hutumia ONNX Runtime moja kwa moja kwa ajili ya kutabiri dhidi ya mifano ya enkoda/dekoda iliyohifadhiwa.

</details>

---

### Zozo la 4 - Kamilisha Kuuandika Sampuli Zote za Zava kwa Kundi

Sasa ukiwa na app ya kuuandika maneno inayofanya kazi, andika maneno ya faili zote tano za sampuli za Zava na linganisha matokeo.

<details>
<summary><h3>Njia ya Python</h3></summary>

Sampuli kamili `python/foundry-local-whisper.py` tayari inaunga mkono kuuandika kwa kundi. Inapotekelezwa bila hoja, inaandika maneno yote ya faili za `zava-*.wav` katika `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Sampuli hutumia `FoundryLocalManager(alias)` kuanzisha, kisha inatekeleza vikao vya enkoda na dekoda vya ONNX kwa kila faili.

</details>

<details>
<summary><h3>Njia ya JavaScript</h3></summary>

Sampuli kamili `javascript/foundry-local-whisper.mjs` tayari inaunga mkono kuuandika kwa kundi. Inapotekelezwa bila hoja, inaandika maneno yote ya faili za `zava-*.wav` katika `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Sampuli hutumia `FoundryLocalManager.create()` na `catalog.getModel(alias)` kuanzisha SDK, kisha hutumia `AudioClient` (ikiwa na `settings.language = "en"`) kuandika maneno ya kila faili.

</details>

<details>
<summary><h3>Njia ya C#</h3></summary>

Sampuli kamili `csharp/WhisperTranscription.cs` tayari inaunga mkono kuuandika kwa kundi. Inapotekelezwa bila hoja ya faili maalum, inaandika maneno yote ya faili za `zava-*.wav` katika `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Sampuli hutumia `FoundryLocalManager.CreateAsync()` na SDK ya `AudioClient` (ikiwa na `Settings.Language = "en"`) kwa uandishi wa maneno kwa mchakato mmoja.

</details>

**Jambo la kuangalia:** Linganisha matokeo ya kuuandika maneno dhidi ya maandishi ya asili yaliyo ndani ya `samples/audio/generate_samples.py`. Je, Whisper inafikia usahihi gani katika kupata majina ya bidhaa kama "Zava ProGrip" na istilahi za kiufundi kama "brushless motor" au "composite decking"?

---

### Zozo la 5 - Elewa Mifumo Muhimu ya Msimbo

Soma jinsi uandishi wa maneno wa Whisper unavyotofautiana na mazungumzo katika lugha tatu:

<details>
<summary><b>Python - Tofauti Muhimu Kutoka Mazungumzo</b></summary>

```python
# Ukomeshaji wa mazungumzo (Sehemu 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Uandika sauti (Sehemu hii):
# Inatumia ONNX Runtime moja kwa moja badala ya mteja wa OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... mzunguko wa kisahihishaji cha kiotomatiki ...
print(tokenizer.decode(generated_tokens))
```

**Ufafanuzi muhimu:** Mifano ya mazungumzo hutumia API inayolingana na OpenAI kupitia `manager.endpoint`. Whisper hutumia SDK kupata faili zilizohifadhiwa za mfano wa ONNX, kisha hufanya utabiri moja kwa moja kwa ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Tofauti Muhimu Kutoka Mazungumzo</b></summary>

```javascript
// Ufafanuzi wa mazungumzo (Sehemu 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Uandishi wa sauti (Sehemu hii):
// Inatumia AudioClient iliyo ndani ya SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Daima weka lugha kwa matokeo bora
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Ufafanuzi muhimu:** Mifano ya mazungumzo hutumia API inayolingana na OpenAI kupitia `manager.urls[0] + "/v1"`. Uandishi wa maneno wa Whisper hutumia `AudioClient` ya SDK, inayopatikana kutoka kwa `model.createAudioClient()`. Weka `settings.language` ili kuepuka matokeo yasiyoeleweka kutoka kwa kugundua kiotomatiki.

</details>

<details>
<summary><b>C# - Tofauti Muhimu Kutoka Mazungumzo</b></summary>

Njia ya C# hutumia `AudioClient` iliyojengwa ndani ya SDK kwa uandishi wa maneno kwa mchakato mmoja:

**Uanzishaji wa Mfano:**

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

**Uandishi wa maneno:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Ufafanuzi muhimu:** C# hutumia `FoundryLocalManager.CreateAsync()` na hupata `AudioClient` moja kwa moja — hakuna hitaji la kuanzisha ONNX Runtime. Weka `Settings.Language` ili kuepuka matokeo yasiyoeleweka kutokana na kugundua kiotomatiki.

</details>

> **Muhtasari:** Python hutumia Foundry Local SDK kwa usimamizi wa mifano na ONNX Runtime kwa utabiri wa moja kwa moja dhidi ya mifano ya enkoda/dekoda. JavaScript na C# zote hutumia `AudioClient` iliyojengwa ndani ya SDK kwa uandishi wa maneno wa haraka — tengeneza kliento, weka lugha, kisha itaje `transcribe()` / `TranscribeAudioAsync()`. Daima weka sifa ya lugha kwenye AudioClient kwa matokeo sahihi.

---

### Zozo la 6 - Jaribu

Jaribu mabadiliko haya ili kuimarisha uelewa wako:

1. **Jaribu faili tofauti za sauti** - jisikie huru kurekodi ukizungumza kwa kutumia Windows Voice Recorder, hifadhi kama WAV, kisha andika maneno

2. **Linganishwa mifano tofauti** - kama una GPU ya NVIDIA, jaribu toleo la CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Linganisha kasi ya kuuandika maneno dhidi ya toleo la CPU.

3. **Ongeza muundo wa matokeo** - jibu la JSON linaweza kujumuisha:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Jenga REST API** - jifunge msimbo wako wa kuuandika maneno katika seva ya wavuti:

   | Lugha | Fremu | Mfano |
   |--------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` kwa `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` kwa `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` kwa `IFormFile` |

5. **Mazungumzo ya mizunguko mingi na kuuandika maneno** - changanya Whisper na wakala wa mazungumzo kutoka Sehemu ya 4: andika maneno ya sauti kwanza, kisha pitisha maandishi kwa wakala kwa uchambuzi au muhtasari.

---

## Marejeleo ya SDK Audio API

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — hutoa mfano wa `AudioClient`
> - `audioClient.settings.language` — weka lugha ya kuuandika maneno (mfano `"en"`)
> - `audioClient.settings.temperature` — simamia mkanganyiko (hiari)
> - `audioClient.transcribe(filePath)` — andika faili, hurudisha `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — ongoza uandishi mfululizo wa vipande kupitia callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — hutoa mfano wa `OpenAIAudioClient`
> - `audioClient.Settings.Language` — weka lugha ya kuuandika maneno (mfano `"en"`)
> - `audioClient.Settings.Temperature` — simamia mkanganyiko (hiari)
> - `await audioClient.TranscribeAudioAsync(filePath)` — andika faili, hurudisha kitu chenye `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — hurudisha `IAsyncEnumerable` ya vipande vya uandishi

> **Vidokezo:** Daima weka sifa ya lugha kabla ya kuandika maneno. Bila hiyo, mfano wa Whisper hujaribu kugundua lugha kiotomatiki, jambo ambalo linaweza kusababisha matokeo yasiyoeleweka (alama moja badala ya maandishi).

---

## Mlinganisho: Mifano ya Mazungumzo vs. Whisper

| Kipengele | Mifano ya Mazungumzo (Sehemu 3-7) | Whisper - Python | Whisper - JS / C# |
|-----------|-----------------------------------|-----------------|-------------------|
| **Aina ya kazi** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Ingizo** | Ujumbe wa maandishi (JSON) | Faili za sauti (WAV/MP3/M4A) | Faili za sauti (WAV/MP3/M4A) |
| **Matokeo** | Maandishi yaliyotengenezwa (mfululizo) | Maandishi yaliyoandikwa (kamilifu) | Maandishi yaliyoandikwa (kamilifu) |
| **Pakiti SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Njia ya API** | `client.chat.completions.create()` | ONNX Runtime moja kwa moja | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Mipangilio ya lugha** | Haipo | Tokeni za hoji ya dekoda | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Uendeshaji wa mfululizo** | Ndiyo | Hapana | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Faida ya faragha** | Kodi/data hstay ndani ya kifaa | Data ya sauti hstay ndani ya kifaa | Data ya sauti hstay ndani ya kifaa |

---

## Muhimu Kuelewa

| Dhana | Ulivyo Jifunza |
|--------|----------------|
| **Whisper kifaa** | Uandishi wa sauti kwa maandishi hufanya kazi kabisa ndani ya kifaa, bora kwa kuuandika simu za wateja wa Zava na mapitio ya bidhaa kifaa |
| **SDK AudioClient** | JavaScript na C# SDK hutoa `AudioClient` iliyojengwa ndani inayoshughulikia mchakato mzima wa kuuandika maneno kwa wito mmoja |
| **Mipangilio ya lugha** | Daima weka lugha ya AudioClient (mfano `"en"`) — bila hiyo, kugundua kiotomatiki kunaweza kusababisha matokeo yasiyoeleweka |
| **Python** | Hutumia `foundry-local-sdk` kwa usimamizi wa mifano + `onnxruntime` + `transformers` + `librosa` kwa utabiri wa moja kwa moja wa ONNX |
| **JavaScript** | Hutumia `foundry-local-sdk` na `model.createAudioClient()` — weka `settings.language`, kisha itaje `transcribe()` |
| **C#** | Hutumia `Microsoft.AI.Foundry.Local` na `model.GetAudioClientAsync()` — weka `Settings.Language`, kisha itaje `TranscribeAudioAsync()` |
| **Msaada wa mfululizo** | SDK za JS na C# pia hutoa `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` kwa matokeo vipande kwa vipande |
| **Imeboreshwa kwa CPU** | Toleo la CPU (3.05 GB) hufanya kazi kwenye kifaa chochote cha Windows bila GPU |
| **Faragha kwanza** | Inafaa kuhifadhi ushirikiano na data za wateja wa Zava na bidhaa miliki kwenye kifaa |

---

## Rasilimali

| Rasilimali | Kiungo |
|-----------|---------|
| Hati za Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Marejeleo ya SDK ya Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Mfano wa OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Tovuti ya Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Hatua Ifuatayo

Endelea kwa [Sehemu 10: Kutumia Mifano Maalum au Hugging Face](part10-custom-models.md) ili kukusanya mifano yako kutoka Hugging Face na kuitekeleza kupitia Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Tangazo la Kutojibu**:  
Hati hii imetafsiriwa kwa kutumia huduma ya tafsiri ya AI [Co-op Translator](https://github.com/Azure/co-op-translator). Wakati tunajitahidi kwa usahihi, tafadhali fahamu kuwa tafsiri za kitaalamu zinaweza kuwa na makosa au upungufu wa usahihi. Hati ya asili katika lugha yake ya mama inapaswa kuchukuliwa kama chanzo cha mamlaka. Kwa taarifa muhimu, tafsiri ya kitaalamu ya mwanadamu inapendekezwa. Hatuwezi kuwajibika kwa kutoelewana au tafsiri potofu zinazotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->