![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ផ្នែកទី 9៖ ការបំលែងសំឡេងជាអត្ថបទជាមួយ Whisper និង Foundry Local

> **គោលបំណង៖** ប្រើម៉ូដែល OpenAI Whisper ដែលដំណើរការក្នុងម៉ាស៊ីនផ្ទាល់តាមរយៈ Foundry Local ដើម្បីបំលែងឯកសារសំឡេងជាអត្ថបទ - នៅលើឧបករណ៍ទាំងស្រុង មិនការពារការប្រើពពកទេ។

## ទិដ្ឋភាពទូទៅ

Foundry Local មិនគ្រាន់តែសម្រាប់បង្កើតអត្ថបទទេ វាក៏គាំទ្រម៉ូដែល **បំលែងសំឡេងទៅអត្ថបទ** ផងដែរ។ ក្នុងមន្ទីរពិសោធន៍នេះ អ្នកនឹងប្រើម៉ូដែល **OpenAI Whisper Medium** ដើម្បីបំលែងឯកសារសំឡេងទាំងស្រុងនៅលើម៉ាស៊ីនរបស់អ្នក។ វាគឺល្អសម្រាប់ស្ថានការណ៍ដូចជាការបំលែងការហៅសេវាកម្មអតិថិជន Zava, ការថតវិច្ឆ័យផលិតផល, ឬសម័យផែនការនៃសិក្ខាសាលាដែលទិន្នន័យសំឡេងមិនត្រូវបានបញ្ជូនចេញពីឧបករណ៍របស់អ្នកបានទេ។


---

## វិស័យសិក្សា

នៅចុងបញ្ចប់នៃមន្ទីរពិសោធន៍នេះ អ្នកនឹងអាច:

- យល់ពីម៉ូដែលបំលែងសំឡេងទៅអត្ថបទ Whisper និងសមត្ថភាពរបស់វា
- ទាញយក និងដំណើរការម៉ូដែល Whisper តាមរយៈ Foundry Local
- បំលែងឯកសារសំឡេងដោយប្រើ Foundry Local SDK ក្នុង Python, JavaScript, និង C#
- បង្កើតសេវាកម្មបំលែងអត្ថបទសាមញ្ញដែលដំណើរការលើឧបករណ៍ទាំងស្រុង
- យល់ពីភាពខុសគ្នារវាងម៉ូដែលជជែក/អត្ថបទ និងម៉ូដែលសំឡេងក្នុង Foundry Local

---

## តម្រូវការមុនពេលចាប់ផ្តើម

| តម្រូវការ | ព័ត៌មានលម្អិត |
|-------------|---------|
| **Foundry Local CLI** | ជំនាន់ **0.8.101 ឬខ្ពស់ជាងនេះ** (ម៉ូដែល Whisper អាចប្រើបានចាប់ពី v0.8.101 តទៅ) |
| **ប្រព័ន្ធប្រតិបត្តិការ** | Windows 10/11 (x64 ឬ ARM64) |
| **បរិវិសភាសា Runtime** | **Python 3.9+** និង/ឬ **Node.js 18+** និង/ឬ **.NET 9 SDK** ([ទាញយក .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **បានបញ្ចប់** | [ផ្នែកទី 1៖ ការចាប់ផ្តើម](part1-getting-started.md), [ផ្នែកទី 2៖ ជ្រៅជាំង Foundry Local SDK](part2-foundry-local-sdk.md), និង [ផ្នែកទី 3៖ SDKs និង APIs](part3-sdk-and-apis.md) |

> **ចំណាំ៖** ម៉ូដែល Whisper ត្រូវបានទាញយកតាមរយៈ **SDK** (មិនមែន CLI ទេ)។ CLI មិនគាំទ្រចំណុចបំលែងសំឡេង។ ពិនិត្យជំនាន់របស់អ្នកដោយ:
> ```bash
> foundry --version
> ```

---

## គំនិត៖ វិធីដែល Whisper ដំណើរការជាមួយ Foundry Local

ម៉ូដែល OpenAI Whisper គឺជាម៉ូដែលទូទៅសម្រាប់ការស្គាល់សំឡេងដែលបានបណ្តុះបណ្តាលលើទិន្នន័យសំឡេងផ្សេងៗច្រើន។ នៅពេលដំណើរការតាមរយៈ Foundry Local:

- ម៉ូដែលដំណើរការ **ពេញលេញនៅលើ CPU របស់អ្នក** - គ្មាន GPU ត្រូវការ
- សំឡេងមិនចេញពីឧបករណ៍អ្នកទេ - **ការការពារសម្ងាត់ពេញលេញ**
- Foundry Local SDK គ្រប់គ្រងការទាញយកម៉ូដែល និងការគ្រប់គ្រងផ្ទុកតម្លើង
- **JavaScript និង C#** ផ្តល់ `AudioClient` ដំណើរការជាស្រេចក្នុង SDK ដែលគ្រប់គ្រងដំណើរការបំលែងសំឡេងទាំងមូល — មិនចាំបាច់តំឡើង ONNX ដោយដៃ
- **Python** ប្រើ SDK សម្រាប់ការគ្រប់គ្រងម៉ូដែល និង ONNX Runtime សម្រាប់ការព្យាករណ៍ដោយផ្ទាល់លើម៉ូដែល encoder/decoder ONNX

### របៀបដំណើរការដំណាក់កាល (JavaScript និង C#) — SDK AudioClient

1. **Foundry Local SDK** ទាញយក និងផ្ទុកម៉ូដែល Whisper
2. `model.createAudioClient()` (JS) ឬ `model.GetAudioClientAsync()` (C#) បង្កើត `AudioClient`
3. `audioClient.transcribe(path)` (JS) ឬ `audioClient.TranscribeAudioAsync(path)` (C#) គ្រប់គ្រងដំណើរការបំលែងទាំងមូល — ការរៀបចំសំឡេងជាមុន, encoder, decoder និងការបំលែងតួអក្សរ
4. `AudioClient` បង្ហាញថាតម្លៃ `settings.language` (កំណត់តម្លៃជា `"en"` សម្រាប់ភាសាអង់គ្លេស) ដើម្បីផ្តល់ជំនួយក្នុងការបំលែងត្រឹមត្រូវ

### របៀបដំណើរការដំណាក់កាល (Python) — ONNX Runtime

1. **Foundry Local SDK** ទាញយក និងផ្ទុកឯកសារម៉ូដែល Whisper ONNX
2. **ការរៀបចំសំឡេងជាមុន** បម្លែងសំឡេង WAV ទៅជាសញ្ញាមេឡស្ពត្ត្រូក្រាម (80 mel bins x 3000 frames)
3. **Encoder** ដំណើរការសញ្ញាម៉េឡស្ពត្ត្រូក្រាម ហើយបង្កើតស្ថានភាពលាក់ និងតួសំខាន់/តម្លៃ cross-attention
4. **Decoder** ដំណើរការដោយ autoregressive ផលិតតួអក្សរមួយៗតាមលំដាប់រហូតដល់បញ្ចប់ពាក្យ
5. **Tokeniser** បំលែងអត្តសញ្ញាណតួអក្សរបញ្ចេញវិញទៅជាអត្ថបទអានបញ្ចេញ

### ការបែងចែកម៉ូដែល Whisper

| ឈ្មោះគូប្រូ | លេខម៉ូដែល | ឧបករណ៍ | ទំហំនៃម៉ូដែល | ការពិពណ៌នា |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | ល្បឿន GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | បង្កើតសម្រាប់ CPU (ណែនាំសម្រាប់ឧបករណ៍ភាគច្រើន) |

> **ចំណាំ៖** ឃ្លាំងម៉ូដែល Whisper មិនបង្ហាញដោយលំនាំដទៃដូចម៉ូដែលជជែកទេ ផ្ទុយទៅវិញវាត្រូវបានចាត់តាំងក្រោមភារកិច្ច `automatic-speech-recognition`។ ប្រើ `foundry model info whisper-medium` ដើម្បីមើលព័ត៌មានលម្អិត។

---

## ការប្រឡងរៀន

### ការប្រឡងទី 0 - ទទួលបានឯកសារសំឡេងគំរូ

មន្ទីរពិសោធន៍នេះមានឯកសារ WAV ត្រៀមរួចដែលផ្អែកលើសេណារីយ៉ូផលិតផល Zava DIY។ បង្កើតវាដោយស្គ្រីបបានរួច៖

```bash
# ពីឫស repo - បង្កើត និងដំណើរការ .venv មុន
python -m venv .venv

# វីនដូ (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

នេះបង្កើតឯកសារ WAV ប្រាំមួយនៅក្នុង `samples/audio/`៖

| ឯកសារ | សេណារីយ៉ូ |
|------|----------|
| `zava-customer-inquiry.wav` | អតិថិជនសួរអំពី **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | អតិថិជនវិភាគយក **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | ការហៅជំនួយអំពី **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | DIYer ផែនការការកសាងឋានៈជាមួយ **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | ដំណើរការសិក្ខាសាលាមួយប្រើប្រាស់ **ផលិតផល Zava ទាំងប្រាំ** |
| `zava-full-project-walkthrough.wav` | ការបង្ហាញផ្ទះ garage ដែលកំពុងបំលែងប្រើប្រាស់ **ផលិតផល Zava ទាំងអស់** (~4 នាទី សម្រាប់តេស្តសំឡេងរយៈវែង) |

> **ជំនួយ៖** អ្នកអាចប្រើឯកសារដែលជាមូលដ្ឋានជា WAV/MP3/M4A របស់អ្នកផ្ទាល់ ឬថតសម្លេងដោយវីស Windows Voice Recorder។

---

### ការប្រឡងទី 1 - ទាញយកម៉ូដែល Whisper តាម SDK

ដោយសារតែ CLI មិនសមហូរម៉ូដែល Whisper នៅក្នុងកំណែ Foundry Local ថ្មីៗ ត្រូវប្រើ **SDK** ដើម្បីទាញយក និងផ្ទុកម៉ូដែល។ ជ្រើសភាសារបស់អ្នក៖

<details>
<summary><b>🐍 Python</b></summary>

**ដំឡើង SDK៖**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# ចាប់ផ្តើមសេវាកម្ម
manager = FoundryLocalManager()
manager.start_service()

# ពិនិត្យព័ត៌មានបណ្ណាល័យ
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# ពិនិត្យថាតើបានទុករក្សាទុករួចហើយឬនៅ
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# ផ្ទុកម៉ូដែលទៅក្នុងអង្គចងចាំ
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

រក្សាទុកជា `download_whisper.py` ហើយបើកដំណើរការ៖
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**ដំឡើង SDK៖**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// បង្កើតអ្នកគ្រប់គ្រង និងចាប់ផ្តើមសេវាកម្ម
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ទាញយកម៉ូដែលពីកាតាឡុក
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

// ផ្ទុកម៉ូដែលចូលក្នុងចងចាំ
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

រក្សាទុកជា `download-whisper.mjs` ហើយបើកដំណើរការ៖
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**ដំឡើង SDK៖**
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

> **ហេតុអ្វីប្រើ SDK មិនប្រើ CLI?** Foundry Local CLI មិនគាំទ្រការទាញយក ឬ បម្រើម៉ូដែល Whisper ដោយផ្ទាល់ទេ។ SDK ផ្តល់វិធីដែលទុកចិត្តបានសម្រាប់ទាញយក និងគ្រប់គ្រងម៉ូដែលសំឡេងតាមកម្មវិធី។ JavaScript និង C# SDK មាន `AudioClient` សរសេរជាស្រេចដែលគ្រប់គ្រងដំណើរការបំលែងទាំងមូល។ Python ប្រើ ONNX Runtime សម្រាប់ព្យាករណ៍ដល់ម៉ូដែលបានដាក់ក្នុង cache។

---

### ការប្រឡងទី 2 - យល់ពី SDK Whisper

ការបំលែងសំឡេងដោយ Whisper ប្រើវិធីខុសៗគ្នាតាមភាសា។ **JavaScript និង C#** ផ្តល់ `AudioClient` សរសេរជាស្រេចនៅក្នុង Foundry Local SDK ដែលគ្រប់គ្រងដំណើរការទាំងមូល (រៀបចំសំឡេងជាមុន, encoder, decoder, បំលែងតួអក្សរ) ក្នុងមុខងារតែមួយ។ **Python** ប្រើ Foundry Local SDK សម្រាប់គ្រប់គ្រងម៉ូដែល និង ONNX Runtime សម្រាប់ព្យាករណ៍ដោយផ្ទាល់លើម៉ូដែល encoder/decoder ONNX។

| គ្រឿងផ្សំ | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **កញ្ចប់ SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **គ្រប់គ្រងម៉ូដែល** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **ដកស្រង់លក្ខណៈពិសេស** | `WhisperFeatureExtractor` + `librosa` | គ្រប់គ្រងដោយ SDK `AudioClient` | គ្រប់គ្រងដោយ SDK `AudioClient` |
| **ព្យាករណ៍** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **បំលែងតួអក្សរ** | `WhisperTokenizer` | គ្រប់គ្រងដោយ SDK `AudioClient` | គ្រប់គ្រងដោយ SDK `AudioClient` |
| **កំណត់ភាសា** | កំណត់តាម `forced_ids` នៅក្នុង token decoder | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **ទិន្នន័យចូល** | ផ្លូវឯកសារ WAV | ផ្លូវឯកសារ WAV | ផ្លូវឯកសារ WAV |
| **ទិន្នន័យចេញ** | ខ្សែអត្ថបទបំលែងវិញ | `result.text` | `result.Text` |

> **សំខាន់៖** តែងតែបំលែងភាសានៅលើ `AudioClient` (ឧ. `"en"` សម្រាប់ភាសាអង់គ្លេស)។ មិនមានការកំណត់ភាសាបញ្ជាក់ សម្ភារម៉ូដែលអាចបង្កើតអក្សរមិនត្រឹមត្រូវដោយសារ​ព្យាយាមស្វ័យប្រវត្តិក្នុងការប្រាប់ភាសា។

> **លំនាំ SDK៖** Python ប្រើ `FoundryLocalManager(alias)` ដើម្បីបើកដំណើរការ ហើយបន្ទាប់មក `get_cache_location()` ស្វែងរកឯកសារម៉ូដែល ONNX។ JavaScript និង C# ប្រើ `AudioClient` ដែលមានរួចក្នុង SDK — ទទួលបានតាម `model.createAudioClient()` (JS) ឬ `model.GetAudioClientAsync()` (C#) — ដែលគ្រប់គ្រងចូលដំណើរការបំលែង។ មើល [ផ្នែកទី 2៖ ជ្រៅជាំង Foundry Local SDK](part2-foundry-local-sdk.md) សម្រាប់ព័ត៌មានលម្អិត។

---

### ការប្រឡងទី 3 - បង្កើតកម្មវិធីបំលែងអត្ថបទសាមញ្ញ

ជ្រើសភាសារបស់អ្នក ហើយបង្កើតកម្មវិធីតូចមួយដើម្បីបំលែងឯកសារសំឡេងជាអត្ថបទ។

> **ទ្រង់ទ្រាយសំឡេងគាំទ្រ៖** WAV, MP3, M4A។ ដើម្បីទទួលបានលទ្ធផលល្អបំផុត សូមប្រើឯកសារ WAV ដែលមានអត្រាសំឡេង 16kHz។

<details>
<summary><h3>ផ្នែក Python</h3></summary>

#### ការតំឡើង

```bash
cd python
python -m venv venv

# បើកបរិវិស្វirtual:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### កូដបំលែងអត្ថបទ

បង្កើតឯកសារ `foundry-local-whisper.py`៖

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

# ជំហានទី ១: Bootstrap - ចាប់ផ្តើមសេវាកម្ម​, ទាញយក, និងផ្ទុកម៉ូដែល
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# បង្កើតផ្លូវទៅរកឯកសារ ONNX ម៉ូដែលដែលបានរក្សាទុក
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# ជំហានទី ២: ផ្ទុកសម័យ ONNX និងកម្មវិធីចំណេញលក្ខណៈ
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

# ជំហានទី ៣: ដកលក្ខណៈស្ពែគ្រូម៉ាហ្គ្រាម mel
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# ជំហានទី ៤: រត់កម្មវិធីបង្កូរ
enc_out = encoder.run(None, {"audio_features": input_features})
# អត្ថផលដំបូងគឺស្ថានភាពលាក់; អ្វីដែលនៅសល់គឺគូ KV ការយកចិត្តទុកដាក់ឆ្លងកាត់
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# ជំហានទី ៥: ការបកប្រែអូតោក្រេស៊ីវ
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, សរសេរឡើងវិញ, គ្មានសញ្ញាអេល
input_ids = np.array([initial_tokens], dtype=np.int32)

# បង្ហាប់ផ្ទុក KV ការយកចិត្តទុកដាក់ផ្ទាល់ខ្លួនទទេ
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

    if next_token == 50257:  # បញ្ចប់អត្ថបទ
        break
    generated.append(next_token)

    # ធ្វើបច្ចុប្បន្នភាពបង្ហាប់ KV ការយកចិត្តទុកដាក់ផ្ទាល់ខ្លួន
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### ដំណើរការ

```bash
# បកប្រែស្ថានភាពផលិតផល Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# ឬសាកល្បងផ្សេងៗ:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### ចំណុចសំខាន់ Python

| វិធីសាស្ត្រ | គោលបំណង |
|--------|---------|
| `FoundryLocalManager(alias)` | បើកដំណើរការ៖ ចាប់ផ្តើមសេវាកម្ម ទាញយក និងផ្ទុកម៉ូដែល |
| `manager.get_cache_location()` | ទទួលផ្លូវទៅកាន់ឯកសារម៉ូដែល ONNX បានផ្ទុក |
| `WhisperFeatureExtractor.from_pretrained()` | ផ្ទុកឧបករណ៍ដកលក្ខណៈពិសេស mel spectrogram |
| `ort.InferenceSession()` | បង្កើតសម័យ ONNX Runtime សម្រាប់ encoder និង decoder |
| `tokenizer.decode()` | បំលែង ID តួអក្សរបំលែងវិញទៅជាអត្ថបទ |

</details>

<details>
<summary><h3>ផ្នែក JavaScript</h3></summary>

#### ការតំឡើង

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### កូដបំលែងអត្ថបទ

បង្កើតឯកសារ `foundry-local-whisper.mjs`៖

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// ជំហ៊ាន 1: Bootstrap - បង្កើតអ្នកគ្រប់គ្រង, ចាប់ផ្តើមសេវាកម្ម, និងផ្ទុកម៉ូដែល
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

// ជំហ៊ាន 2: បង្កើតអតិថិជនសម្លេង និងបកប្រែ
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// សម្អាត
await model.unload();
```

> **ចំណាំ៖** Foundry Local SDK ផ្តល់ `AudioClient` សរសេរជាស្រេចតាម `model.createAudioClient()` ដែលគ្រប់គ្រងដំណើរការ ONNX បំលែងទាំងមូល — មិនត្រូវនាំចូល `onnxruntime-node` ទេ។ តែងតែបំលែង `audioClient.settings.language = "en"` ដើម្បីធានាបំលែងអត្ថបទភាសាអង់គ្លេសបានត្រឹមត្រូវ។

#### ដំណើរការ

```bash
# បញ្ចូលសេណារីយ៉ូផលិតផល Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# ឬព្យាយាមផ្សេងទៀត៖
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### ចំណុចសំខាន់ JavaScript

| វិធីសាស្ត្រ | គោលបំណង |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | បង្កើតមេណេជ័រតែមួយ |
| `await catalog.getModel(alias)` | ទទួលម៉ូដែលពីសារាងសកល |
| `model.download()` / `model.load()` | ទាញយក និងផ្ទុកម៉ូដែល Whisper |
| `model.createAudioClient()` | បង្កើត AudioClient សម្រាប់បំលែង |
| `audioClient.settings.language = "en"` | កំណត់ភាសាបំលែង (ចាំបាច់សម្រាប់លទ្ធផលត្រឹមត្រូវ) |
| `audioClient.transcribe(path)` | បំលែងឯកសារសំឡេង ហើយបានតម្លៃ `{ text, duration }` |

</details>

<details>
<summary><h3>ផ្នែក C#</h3></summary>

#### ការតំឡើង

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **ចំណាំ៖** ផ្នែក C# ប្រើកញ្ចប់ `Microsoft.AI.Foundry.Local` ដែលផ្តល់ `AudioClient` សរសេរជាស្រេចតាម `model.GetAudioClientAsync()`។ វាគ្រប់គ្រងដំណើរការបំលែងទាំងមូលនៅក្នុងកម្មវិធី — មិនចាំបាច់តំឡើង ONNX Runtime ផ្សេងទេ។

#### កូដបំលែងអត្ថបទ

ជំនួសមាតិកានៃ `Program.cs`៖

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

#### ដំណើរការ

```bash
# បញ្ចូលសេចក្តីស្នើសុំ​ផលិតផល Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# ឬសាកល្បងវត្ថុផ្សេងទៀត:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### ចំណុចសំខាន់ C#

| វិធីសាស្ត្រ | គោលបំណង |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | ចាប់ផ្តើម Foundry Local ជាមួយការកំណត់ |
| `catalog.GetModelAsync(alias)` | ទទួលម៉ូដែលពីសារាងសកល |
| `model.DownloadAsync()` | ទាញយកម៉ូដែល Whisper |
| `model.GetAudioClientAsync()` | ទទួល AudioClient (មិនមែន ChatClient!) |
| `audioClient.Settings.Language = "en"` | កំណត់ភាសាបំលែង (ចាំបាច់សម្រាប់លទ្ធផលត្រឹមត្រូវ) |
| `audioClient.TranscribeAudioAsync(path)` | បំលែងឯកសារសំឡេង |
| `result.Text` | អត្ថបទដែលបានបំលែង |


> **C# ប្រឆាំង Python/JS:** អ្នកប្រើប្រាស់ SDK C# មាន `AudioClient` ដែលបង្កើតរួចជាស្រេចសម្រាប់ការបកប្រែអត្ថបទក្នុងដំណើរការនៅក្នុងកម្មវិធីតាមរយៈ `model.GetAudioClientAsync()`, ដូចជា SDK JavaScript ។ ភាសា Python ប្រើកម្មវិធី ONNX Runtime ដូចគ្នាសម្រាប់ធ្វើការសន្និដ្ឋានទៅលើម៉ូដែល encoder/decoder ដែលបានរក្សាទុករួច។

</details>

---

### ហាត់ប្រាណ 4 - បកប្រែឯកសារគំរូ Zava ទាំងអស់ជាក្រុម

ឥឡូវនេះអ្នកមានកម្មវិធីបកប្រែដែលដំណើរការបាន សូមបកប្រែឯកសារគំរូ Zava ចំនួនប្រាំ និងប្រៀបធៀបទិន្នផល។

<details>
<summary><h3>ផ្លូវ Python</h3></summary>

ឯកសារគំរូពេញ `python/foundry-local-whisper.py` មានសមត្ថភាពសម្រាប់បកប្រែជាផ្នែក។ ពេលដំណើរការដោយគ្មានអាគុយម៉ង់វា បកប្រែឯកសារ `zava-*.wav` ទាំងអស់នៅក្នុង `samples/audio/`៖

```bash
cd python
python foundry-local-whisper.py
```

ឯកសារគំរូប្រើ `FoundryLocalManager(alias)` ដើម្បីចាប់ផ្តើម បន្ទាប់មកដំណើរការ ONNX សម័យ encoder និង decoder សម្រាប់ឯកសារនីមួយៗ។

</details>

<details>
<summary><h3>ផ្លូវ JavaScript</h3></summary>

ឯកសារគំរូពេញ `javascript/foundry-local-whisper.mjs` មានសមត្ថភាពសម្រាប់បកប្រែជាផ្នែក។ ពេលដំណើរការដោយគ្មានអាគុយម៉ង់វា បកប្រែឯកសារ `zava-*.wav` ទាំងអស់នៅក្នុង `samples/audio/`៖

```bash
cd javascript
node foundry-local-whisper.mjs
```

ឯកសារគំរូប្រើ `FoundryLocalManager.create()` និង `catalog.getModel(alias)` ដើម្បីចាប់ផ្តើម SDK បន្ទាប់មកប្រើ `AudioClient` (ដែលមាន `settings.language = "en"`) សម្រាប់បកប្រែឯកសារនីមួយៗ។

</details>

<details>
<summary><h3>ផ្លូវ C#</h3></summary>

ឯកសារគំរូពេញ `csharp/WhisperTranscription.cs` មានសមត្ថភាពសម្រាប់បកប្រែជាផ្នែក។ ពេលដំណើរការដោយគ្មានអាគុយម៉ង់ឯកសារពិសេស វាបកប្រែឯកសារ `zava-*.wav` ទាំងអស់នៅក្នុង `samples/audio/`៖

```bash
cd csharp
dotnet run whisper
```

ឯកសារគំរូប្រើ `FoundryLocalManager.CreateAsync()` និង `AudioClient` របស់ SDK (ដែលមាន `Settings.Language = "en"`) សម្រាប់បកប្រែអត្ថបទក្នុងដំណើរការ។

</details>

**អ្វីដែលត្រូវមើល:** ប្រៀបធៀបលទ្ធផលបកប្រែក្នុងការប្រៀបធៀបជាមួយអត្ថបទដើមក្នុង `samples/audio/generate_samples.py`។ Whisper ចាប់យកឈ្មោះផលិតផលដូចជា "Zava ProGrip" និងពាក្យបច្ចេកទេសដូចជា "brushless motor" ឬ "composite decking" មានភាពត្រឹមត្រូវប៉ុណ្ណា?

---

### ហាត់ប្រាណ 5 - យល់ដឹងពីបែបបទកូដសំខាន់ៗ

សិក្សាថាតើការបកប្រែ Whisper ខុសពីកិច្ចបញ្ចប់ជជែកនៅក្នុងភាសាទាំងបីយ៉ាងដូចម្តេច៖

<details>
<summary><b>Python - ភាពខុសប្លែកសំខាន់ពី Chat</b></summary>

```python
# បញ្ចប់ការជជែក (ផ្នែកទី 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# ការបម្លែងសំឡេងទៅអត្ថបទ (ផ្នែកនេះ):
# ប្រើ ONNX Runtime ដោយផ្ទាល់ ជំនួសឱ្យម៉ូឌែល OpenAI client
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... រង្វិល decoder ដែលព្យួរ autoregressive ...
print(tokenizer.decode(generated_tokens))
```

**ចំណុចសំខាន់ ៖** ម៉ូដែលជជែកប្រើ API ដែលសមស្របជាមួយ OpenAI តាមរយៈ `manager.endpoint`។ Whisper ប្រើ SDK ដើម្បីស្វែងរកកំណត់ត្រាម៉ូដែល ONNX ទុករួច ហើយបន្ទាប់មកដំណើរការសន្និដ្ឋានដោយផ្ទាល់ជាមួយ ONNX Runtime។

</details>

<details>
<summary><b>JavaScript - ភាពខុសប្លែកសំខាន់ពី Chat</b></summary>

```javascript
// ការសម្រួលជជែក (ផ្នែក 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// ការបំលែងសម្លេងទៅអត្ថបទ (ផ្នែកនេះ):
// ប្រើ AudioClient ដែលបញ្ចូលក្នុង SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // ចូរតែងតាំងភាសាឲ្យបានសម្រេចលទ្ធផលល្អបំផុតតែងតាំងមែនជាបាន
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**ចំណុចសំខាន់ ៖** ម៉ូដែលជជែកប្រើ API ដែលសមស្របជាមួយ OpenAI តាមរយៈ `manager.urls[0] + "/v1"`។ ការបកប្រែ Whisper ប្រើ `AudioClient` របស់ SDK ដែលទទួលបានពី `model.createAudioClient()`។ កំណត់ `settings.language` ដើម្បីជៀសវាងលទ្ធផលខូចពីការចាប់សញ្ញាអូតូម៉ាទិក។

</details>

<details>
<summary><b>C# - ភាពខុសប្លែកសំខាន់ពី Chat</b></summary>

វិធីសាស្រ្ត C# ប្រើ `AudioClient` ដែលមាននៅក្នុង SDK សម្រាប់បកប្រែអត្ថបទក្នុងដំណើរការ៖

**ការចាប់ផ្តើមម៉ូដែល៖**

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

**ការបកប្រែអត្ថបទ៖**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**ចំណុចសំខាន់ ៖** C# ប្រើ `FoundryLocalManager.CreateAsync()` និងទទួលបាន `AudioClient` តាមផ្ទាល់ — មិនចាំបាច់តំឡើង ONNX Runtime ទេ។ កំណត់ `Settings.Language` ដើម្បីជៀសវាងលទ្ធផលខូចពីការចាប់សញ្ញាអូតូម៉ាទិក។

</details>

> **សង្ខេប៖** Python ប្រើ Foundry Local SDK សម្រាប់គ្រប់គ្រងម៉ូដែល និង ONNX Runtime សម្រាប់សន្និដ្ឋានផ្ទាល់លើម៉ូដែល encoder/decoder ។ JavaScript និង C# សុទ្ធតែងប្រើ `AudioClient` ដែលមាននៅក្នុង SDK សម្រាប់បកប្រែដោយបង្រួម — បង្កើត client, កំណត់ភាសា ហើយហៅ `transcribe()` / `TranscribeAudioAsync()`។ តែងតែបន្ថែមភាសាត្រឹមត្រូវលើ AudioClient ដើម្បីទទួលបានលទ្ធផលត្រឹមត្រូវ។

---

### ហាត់ប្រាណ 6 - សាកល្បង

សាកល្បងកែប្រែខាងក្រោមដើម្បីបំភ្លឺយល់ដឹងរបស់អ្នក៖

1. **សាកល្បងឯកសារអូឌ្យ៉ូផ្សេងៗ** - ថតខ្លួនឯងនិយាយដោយប្រើកម្មវិធី Windows Voice Recorder, រក្សាទុកជា WAV ហើយបកប្រែវា

2. **ប្រៀបធៀបម៉ូដែលផ្សេងៗ** - បើអ្នកមាន GPU NVIDIA សាកល្បងប្រភេទ CUDA៖
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   ប្រៀបធៀបល្បឿនបកប្រែប្រឆាំងនឹងម៉ូដែល CPU ។

3. **បន្ថែមទ្រង់ទ្រាយលទ្ធផលចេញ** - ភាសា JSON អាចបញ្ចុងដោយ៖
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **បង្កើត REST API** - បង្រួមកូដបកប្រែរបស់អ្នកក្នុងម៉ាស៊ីនមេវែប៖

   | ភាសា | ប្លាសទវ័រ | ឧទាហរណ៍ |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` ជាមួយ `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` ជាមួយ `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` ជាមួយ `IFormFile` |

5. **ចលនាផ្សេងជាច្រើនជំហានជាមួយការបកប្រែ** - បង្កប់ Whisper ជាមួយភ្នាក់ងារជជែកពីផ្នែក 4៖ បកប្រែអូឌ្យ៉ូជាមុន បន្ទាប់បញ្ជូនអត្ថបទទៅភ្នាក់ងារសម្រាប់វិភាគឬសេចក្ដីសង្ខេប។

---

## ឯកសារតំណល់ Audio API របស់ SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — បង្កើតឧបករណ៍ `AudioClient` មួយ
> - `audioClient.settings.language` — កំណត់ភាសាសម្រាប់ការបកប្រែ (ឧ. `"en"`)
> - `audioClient.settings.temperature` — គ្រប់គ្រងភាពចៃដន្យ (ជាជម្រើស)
> - `audioClient.transcribe(filePath)` — បកប្រែឯកសារ ត្រឡប់ `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — ផ្សាយបន្តផ្នែកបកប្រែតាម callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — បង្កើត instance `OpenAIAudioClient`
> - `audioClient.Settings.Language` — កំណត់ភាសាសម្រាប់ការបកប្រែ (ឧ. `"en"`)
> - `audioClient.Settings.Temperature` — គ្រប់គ្រងភាពចៃដន្យ (ជាជម្រើស)
> - `await audioClient.TranscribeAudioAsync(filePath)` — បកប្រែឯកសារ ត្រឡប់វត្ថុដែលមាន `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — ត្រឡប់ `IAsyncEnumerable` នៃផ្នែកបកប្រែ

> **បញ្ជាក់:** តែងតែបន្ថែមភាសាមុនការបកប្រែ។ ពុំមានកំណត់ភាសា ម៉ូដែល Whisper ព្យាយាមចាប់សញ្ញាអូតូម៉ាទិក ដែលអាចបន្សល់ទិន្នផលខូច (តួរជំនួសតែមួយជំនួសអត្ថបទ)។

---

## ប្រៀបធៀប: ម៉ូដែល Chat ប្រៀបធៀបទៅ Whisper

| មុខងារ | ម៉ូដែល Chat (ផ្នែក 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **ប្រភេទភារកិច្ច** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **ទិន្នន័យបញ្ចូល** | សារអត្ថបទ (JSON) | ឯកសារអូឌ្យ៉ូ (WAV/MP3/M4A) | ឯកសារអូឌ្យ៉ូ (WAV/MP3/M4A) |
| **លទ្ធផលចេញ** | អត្ថបទបង្កើត (ផ្សាយបន្ត) | អត្ថបទបកប្រែ (ពេញលេញ) | អត្ថបទបកប្រែ (ពេញលេញ) |
| **កញ្ចប់ SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **វិធីសាស្រ្ត API** | `client.chat.completions.create()` | ONNX Runtime ដោយផ្ទាល់ | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **ការកំណត់ភាសា** | មិនមាន | នៅក្នុង prompt decoder | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **ការផ្សាយបន្ត** | មាន | មិនមាន | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **អត្ថប្រយោជន៍ខាងភាពឯកជន** | កូដ/ទិន្នន័យនៅក្នុងស្រុក | ទិន្នន័យអូឌ្យ៉ូនៅក្នុងស្រុក | ទិន្នន័យអូឌ្យ៉ូនៅក្នុងស្រុក |

---

## ចំណុចសំខាន់ៗដែលយកទៅបង្រៀន

| គំនិត | អ្វីដែលអ្នកបានរៀន |
|---------|-----------------|
| **Whisper រត់លើឧបករណ៍** | ការបម្លែងសំលេងទៅអត្ថបទដំណើរការលើឧបករណ៍ដោយផ្ទាល់ ដែលល្អសម្រាប់បកប្រែការហៅ និងមតិយោបល់ពីអតិថិជន Zava នៅលើឧបករណ៍ |
| **SDK AudioClient** | SDK ជាភាសា JavaScript និង C# ផ្តល់ `AudioClient` ដែលដំណើរការជារូបមន្តមួយសម្រាប់បកប្រែសំរាប់ការៅលើការហៅតែមួយ |
| **ការកំណត់ភាសា** | តែងតែកំណត់ភាសា AudioClient (ឧ. `"en"`) — ប្រសិនបើមិនមាន វាបង្កើតលទ្ធផលខូចពីចាប់សញ្ញាអូតូម៉ាទិក |
| **Python** | ប្រើ `foundry-local-sdk` សម្រាប់គ្រប់គ្រងម៉ូដែល + `onnxruntime` + `transformers` + `librosa` សម្រាប់សន្និដ្ឋានត្រង់ ONNX |
| **JavaScript** | ប្រើ `foundry-local-sdk` ជាមួយ `model.createAudioClient()` — កំណត់ `settings.language` ហើយហៅ `transcribe()` |
| **C#** | ប្រើ `Microsoft.AI.Foundry.Local` ជាមួយ `model.GetAudioClientAsync()` — កំណត់ `Settings.Language` ហើយហៅ `TranscribeAudioAsync()` |
| **គាំទ្រការផ្សាយបន្ត** | SDK JS និង C# ផ្តល់ការគាំទ្រ `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` សម្រាប់លទ្ធផលចេញជាផ្នែកៗ |
| **ថាមពល CPU ត្រូវបង្កើតខ្ពស់** | ម៉ូដែល CPU (3.05 GB) អាចដំណើរការលើឧបករណ៍ Windows គ្រប់ប្រភេទដោយមិនចាំបាច់ GPU |
| **ផ្តល់អធិប្បាយភាពឯកជនជាលំដាប់ដំបូង** | ថែរក្សាការប្រតិបត្តិការអតិថិជន និងទិន្នន័យក្រុមហ៊ុន Zava នៅលើឧបករណ៍ដោយផ្ទាល់ |

---

## ធនធាន

| ធនធាន | តំណទៅកាន់ |
|----------|------|
| ឯកសារ Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| ឯកសារ SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| ម៉ូដែល OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| វេបសាយ Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## ជំហានបន្ទាប់

បន្តទៅ [ផ្នែកទី 10: ការប្រើម៉ូដែលតាមបំណង ឬ Hugging Face](part10-custom-models.md) ដើម្បីបង្កើតម៉ូដែលផ្ទាល់ខ្លួនពី Hugging Face និងដំណើរការ​ពួកវាតាមរយៈ Foundry Local។