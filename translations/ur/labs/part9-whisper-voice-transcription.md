![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# حصہ 9: Whisper اور Foundry Local کے ساتھ وائس ٹرانسکرپشن

> **مقصد:** OpenAI Whisper ماڈل کو مقامی طور پر Foundry Local کے ذریعے چلانا تاکہ آڈیو فائلوں کی ٹرانسکرپشن کی جا سکے - مکمل طور پر ڈیوائس پر، بغیر کسی کلاؤڈ کے۔

## جائزہ

Foundry Local صرف متن کی تخلیق کے لیے نہیں ہے؛ یہ **تقریر سے متن** ماڈلز کی بھی حمایت کرتا ہے۔ اس لیب میں آپ **OpenAI Whisper Medium** ماڈل کو استعمال کریں گے تاکہ آڈیو فائلوں کی مکمل طور پر آپ کے مشین پر ٹرانسکرپشن کی جا سکے۔ یہ ایسے مناظرات کے لیے مثالی ہے جیسے Zava کسٹمر سروس کالز، پراڈکٹ ریویو ریکارڈنگز، یا ورکشاپ کی منصوبہ بندی کے سیشنز جہاں آڈیو ڈیٹا آپ کے ڈیوائس سے باہر نہیں جانا چاہیے۔

---

## سیکھنے کے مقاصد

اس لیب کے اختتام پر آپ کے قابل ہو جائیں گے:

- Whisper تقریر سے متن ماڈل اور اس کی صلاحیتوں کو سمجھنا  
- Foundry Local کا استعمال کرتے ہوئے Whisper ماڈل کو ڈاؤن لوڈ اور چلانا  
- Python، JavaScript، اور C# میں Foundry Local SDK کے ذریعے آڈیو فائلوں کی ٹرانسکرپشن کرنا  
- ایک سادہ ٹرانسکرپشن سروس بنانا جو مکمل طور پر ڈیوائس پر چلتی ہو  
- Foundry Local میں چیٹ/متنی ماڈلز اور آڈیو ماڈلز کے مابین فرق کو سمجھنا  

---

## ضروریات

| ضرورت | تفصیلات |
|-------------|---------|
| **Foundry Local CLI** | ورژن **0.8.101 یا اس سے اوپر** (Whisper ماڈلز v0.8.101 سے دستیاب ہیں) |
| **آپریٹنگ سسٹم** | Windows 10/11 (x64 یا ARM64) |
| **زبان رن ٹائم** | **Python 3.9+** اور/یا **Node.js 18+** اور/یا **.NET 9 SDK** ([.NET ڈاؤن لوڈ کریں](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **مکمل شدہ** | [حصہ 1: شروعات](part1-getting-started.md), [حصہ 2: Foundry Local SDK کی گہرائی](part2-foundry-local-sdk.md), اور [حصہ 3: SDKs اور APIs](part3-sdk-and-apis.md) |

> **نوٹ:** Whisper ماڈلز کو صرف **SDK** کے ذریعے ڈاؤن لوڈ کیا جا سکتا ہے (CLI سے نہیں)۔ CLI آڈیو ٹرانسکرپشن اینڈ پوائنٹ کی حمایت نہیں کرتا۔ اپنا ورژن چیک کریں:
> ```bash
> foundry --version
> ```

---

## تصور: Whisper کس طرح Foundry Local کے ساتھ کام کرتا ہے

OpenAI Whisper ماڈل ایک جنرل پرپز تقریر شناخت کا ماڈل ہے جو وسیع اور متنوع آڈیو ڈیٹا پر تربیت پایا ہے۔ جب اسے Foundry Local کے ذریعے چلایا جاتا ہے:

- ماڈل **مکمل طور پر آپ کے CPU پر چلتا ہے** - کسی GPU کی ضرورت نہیں  
- آڈیو آپ کے ڈیوائس سے کبھی باہر نہیں جاتا - **مکمل پرائیویسی**  
- Foundry Local SDK ماڈل ڈاؤن لوڈ اور کیش مینجمنٹ کو سنبھالتا ہے  
- **JavaScript اور C#** SDK میں `AudioClient` بلٹ ان فراہم کرتے ہیں جو پورے ٹرانسکرپشن عمل کو خودکار طریقے سے سنبھالتا ہے — کوئی دستی ONNX سیٹ اپ ضروری نہیں  
- **Python** ماڈل مینجمنٹ کے لیے SDK استعمال کرتا ہے اور encoder/decoder ONNX ماڈلز کے خلاف براہ راست اندازے کے لیے ONNX Runtime استعمال کرتا ہے  

### پائپ لائن کیسے کام کرتی ہے (JavaScript اور C#) — SDK AudioClient

1. **Foundry Local SDK** Whisper ماڈل کو ڈاؤن لوڈ اور کیش کرتا ہے  
2. `model.createAudioClient()` (JS) یا `model.GetAudioClientAsync()` (C#) `AudioClient` بناتا ہے  
3. `audioClient.transcribe(path)` (JS) یا `audioClient.TranscribeAudioAsync(path)` (C#) مکمل پائپ لائن کو اندرونی طور پر سنبھالتا ہے — آڈیو پری پروسیسنگ، encoder، decoder، اور ٹوکن ڈیکوڈنگ  
4. `AudioClient` ایک `settings.language` پراپرٹی ظاہر کرتا ہے (انگریزی کے لیے `"en"` سیٹ کریں) تاکہ درست ٹرانسکرپشن کی رہنمائی کرے  

### پائپ لائن کیسے کام کرتی ہے (Python) — ONNX Runtime

1. **Foundry Local SDK** Whisper ONNX ماڈل فائلز ڈاؤن لوڈ اور کیش کرتا ہے  
2. **آڈیو پری پروسیسنگ** WAV آڈیو کو میلو سپیکٹروگرام میں تبدیل کرتی ہے (80 میلو بنز × 3000 فریمز)  
3. **encoder** میلو سپیکٹروگرام کو پراسیس کرتا ہے اور hidden states کے ساتھ cross-attention key/value tensors تیار کرتا ہے  
4. **decoder** آٹو ریگریسو طریقے سے چلاتا ہے، ایک وقت میں ایک ٹوکن جنریٹ کرتا ہے جب تک ایک end-of-text ٹوکن پیدا نہ ہو جائے  
5. **tokeniser** آؤٹ پٹ ٹوکن IDز کو دوبارہ قابل پڑھائی متن میں تبدیل کرتا ہے  

### Whisper ماڈل کی اقسام

| عرفی نام | ماڈل ID | ڈیوائس | سائز | وضاحت |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-تیزی (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU بہتر (زیادہ تر ڈیوائسز کے لیے تجویز کردہ) |

> **نوٹ:** چیٹ ماڈلز کے برعکس جو ڈیفالٹ کے طور پر لسٹ ہوتے ہیں، Whisper ماڈلز `automatic-speech-recognition` ٹاسک کے تحت درجہ بندی کیے گئے ہیں۔ تفصیلات دیکھنے کے لیے `foundry model info whisper-medium` استعمال کریں۔

---

## لیب مشقیں

### مشق 0 - نمونہ آڈیو فائلیں حاصل کریں

یہ لیب پہلے سے تیار کردہ WAV فائلز شامل کرتی ہے جو Zava DIY پراڈکٹ مناظر پر مبنی ہیں۔ انہیں دی گئی اسکرپٹ کے ساتھ بنائیں:

```bash
# ریپو جڑ سے - پہلے ایک .venv بنائیں اور فعال کریں
python -m venv .venv

# ونڈوز (پاور شیل):
.venv\Scripts\Activate.ps1
# میک او ایس:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

یہ `samples/audio/` میں چھ WAV فائلز بناتا ہے:

| فائل | منظرنامہ |
|------|----------|
| `zava-customer-inquiry.wav` | گاہک Zava ProGrip Cordless Drill کے بارے میں پوچھ رہا ہے |
| `zava-product-review.wav` | گاہک Zava UltraSmooth Interior Paint کا جائزہ لے رہا ہے |
| `zava-support-call.wav` | Zava TitanLock Tool Chest پر سپورٹ کال |
| `zava-project-planning.wav` | DIYر ایک ڈیک کی منصوبہ بندی کر رہا ہے Zava EcoBoard Composite Decking کے ساتھ |
| `zava-workshop-setup.wav` | ورکشاپ کی چالیں چلانا جس میں تمام پانچ Zava پراڈکٹس استعمال ہو رہے ہیں |
| `zava-full-project-walkthrough.wav` | گراج کی لمبی مرمت کا معائنہ جس میں تمام Zava پراڈکٹس استعمال ہیں (~4 منٹ، لمبی آڈیو ٹیسٹنگ کے لیے) |

> **مشورہ:** آپ اپنی WAV/MP3/M4A فائلز بھی استعمال کر سکتے ہیں، یا Windows Voice Recorder سے ریکارڈ کر سکتے ہیں۔

---

### مشق 1 - SDK کا استعمال کرتے ہوئے Whisper ماڈل ڈاؤن لوڈ کریں

Foundry Local کے نئے ورژنز میں Whisper ماڈلز کے لیے CLI کی مطابقت کی کمی کے باعث، ماڈل ڈاؤن لوڈ اور لوڈ کرنے کے لیے **SDK** استعمال کریں۔ اپنی زبان منتخب کریں:

<details>
<summary><b>🐍 Python</b></summary>

**SDK انسٹال کریں:**  
```bash
pip install foundry-local-sdk
```
  
```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# سروس شروع کریں
manager = FoundryLocalManager()
manager.start_service()

# کیٹلاگ کی معلومات چیک کریں
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# چیک کریں کہ پہلے سے کیش کیا گیا ہے یا نہیں
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# ماڈل کو میموری میں لوڈ کریں
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```
  
`download_whisper.py` کے طور پر محفوظ کریں اور چلائیں:  
```bash
python download_whisper.py
```
  
</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK انسٹال کریں:**  
```bash
npm install foundry-local-sdk
```
  
```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// منیجر بنائیں اور سروس شروع کریں
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// کیٹلاگ سے ماڈل حاصل کریں
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

// ماڈل کو میموری میں لوڈ کریں
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```
  
`download-whisper.mjs` کے طور پر محفوظ کریں اور چلائیں:  
```bash
node download-whisper.mjs
```
  
</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK انسٹال کریں:**  
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

> **کیوں SDK CLI کے بجائے؟** Foundry Local CLI براہ راست Whisper ماڈلز کو ڈاؤن لوڈ یا سروس نہیں کرتا۔ SDK پروگرام کے ذریعے آڈیو ماڈلز کو ڈاؤن لوڈ اور منظم کرنے کا معتبر ذریعہ فراہم کرتا ہے۔ JavaScript اور C# SDKs میں `AudioClient` بنایا ہوا ہوتا ہے جو پورے ٹرانسکرپشن لائن کو سنبھالتا ہے۔ Python encoder/decoder ONNX ماڈلز کے لیے براہ راست اندازے کے لیے ONNX Runtime استعمال کرتا ہے۔

---

### مشق 2 - Whisper SDK کو سمجھیں

Whisper ٹرانسکرپشن زبان کے لحاظ سے مختلف طریقے استعمال کرتی ہے۔ **JavaScript اور C#** میں Foundry Local SDK میں `AudioClient` بلٹ ان ہے جو پوری لائن ایک میتھڈ کال میں سنبھال لیتا ہے (آڈیو پری پروسیسنگ، encoder، decoder، ٹوکن ڈیکوڈنگ)۔ **Python** ماڈل مینجمنٹ کے لیے SDK استعمال کرتا ہے اور encoder/decoder ONNX ماڈلز کے لیے براہ راست اندازے کے لیے ONNX Runtime۔

| جزو | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK پیکجز** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **ماڈل مینجمنٹ** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **خصوصیت نکالنا** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` کے ذریعے سنبھالا جاتا ہے | SDK `AudioClient` کے ذریعے سنبھالا جاتا ہے |
| **انفرنس** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **ٹوکن ڈیکوڈنگ** | `WhisperTokenizer` | SDK `AudioClient` کے ذریعے | SDK `AudioClient` کے ذریعے |
| **زبان سیٹنگ** | decoder tokens میں `forced_ids` کے ذریعے | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **ان پٹ** | WAV فائل کا راستہ | WAV فائل کا راستہ | WAV فائل کا راستہ |
| **آؤٹ پٹ** | ڈیکوڈ شدہ متن | `result.text` | `result.Text` |

> **اہم:** ہمیشہ `AudioClient` پر زبان سیٹ کریں (مثلاً انگریزی کے لیے `"en"`)۔ زبان کے بغیر ماڈل پرانا آؤٹ پٹ دے سکتا ہے کیونکہ یہ زبان کو خود پتہ لگانے کی کوشش کرتا ہے۔

> **SDK پیٹرنز:** Python `FoundryLocalManager(alias)` کو استعمال کرتا ہے، پھر ONNX ماڈل فائلوں کے کیش لوکیشن کے لیے `get_cache_location()`۔ JavaScript اور C# SDK کے بلٹ ان `AudioClient` کو استعمال کرتے ہیں جو `model.createAudioClient()` (JS) یا `model.GetAudioClientAsync()` (C#) کے ذریعے حاصل کیا جاتا ہے اور مکمل ٹرانسکرپشن لائن سنبھالتا ہے۔ تفصیلی معلومات کے لیے [حصہ 2: Foundry Local SDK کی گہرائی](part2-foundry-local-sdk.md) دیکھیں۔

---

### مشق 3 - ایک سادہ ٹرانسکرپشن ایپ بنائیں

اپنی زبان منتخب کریں اور ایک کم از کم ایپلیکیشن بنائیں جو آڈیو فائل کی ٹرانسکرپشن کرے۔

> **حمایت شدہ آڈیو فارمیٹس:** WAV، MP3، M4A۔ بہترین نتائج کے لیے 16kHz سیمپل ریٹ کے ساتھ WAV فائل استعمال کریں۔

<details>
<summary><h3>Python ٹریک</h3></summary>

#### سیٹ اپ

```bash
cd python
python -m venv venv

# ورچوئل ماحول کو فعال کریں:
# ونڈوز (پاور شیل):
venv\Scripts\Activate.ps1
# میک او ایس:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```
  
#### ٹرانسکرپشن کوڈ

ایک فائل `foundry-local-whisper.py` بنائیں:  

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

# مرحلہ 1: بوٹسٹریپ - سروس شروع کرتا ہے، ڈاؤن لوڈ کرتا ہے، اور ماڈل لوڈ کرتا ہے
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# کیشڈ ONNX ماڈل فائلوں کا راستہ بنائیں
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# مرحلہ 2: ONNX سیشنز اور فیچر ایکسٹریکٹر لوڈ کریں
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

# مرحلہ 3: میل اسپیکٹوگرام فیچرز نکالیں
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# مرحلہ 4: انکوڈر چلائیں
enc_out = encoder.run(None, {"audio_features": input_features})
# پہلا آؤٹ پٹ پوشیدہ حالات ہے؛ باقی کراس اٹینشن KV جوڑے ہیں
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# مرحلہ 5: آٹو ریگریسیو ڈی کوڈنگ
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcribe, notimestamps
input_ids = np.array([initial_tokens], dtype=np.int32)

# خالی سیلف اٹینشن KV کیش
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

    if next_token == 50257:  # متن کا اختتام
        break
    generated.append(next_token)

    # سیلف اٹینشن KV کیش کو اپ ڈیٹ کریں
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```
  
#### اسے چلائیں

```bash
# زوا پروڈکٹ منظرنامہ نقل کریں
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# یا دوسرے کوشش کریں:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```
  
#### Python کے اہم نکات

| طریقہ | مقصد |
|--------|---------|
| `FoundryLocalManager(alias)` | بوٹ اسٹریپ: سروس شروع کریں، ڈاؤنلوڈ کریں، اور ماڈل لوڈ کریں |
| `manager.get_cache_location()` | کیش کیے ہوئے ONNX ماڈل فائلوں کا راستہ حاصل کریں |
| `WhisperFeatureExtractor.from_pretrained()` | میلو سپیکٹروگرام فیچر ایکسٹریکٹر لوڈ کریں |
| `ort.InferenceSession()` | encoder اور decoder کے لیے ONNX Runtime سیشن بنائیں |
| `tokenizer.decode()` | آؤٹ پٹ ٹوکن IDز کو متن میں تبدیل کریں |

</details>

<details>
<summary><h3>JavaScript ٹریک</h3></summary>

#### سیٹ اپ

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```
  
#### ٹرانسکرپشن کوڈ

ایک فائل `foundry-local-whisper.mjs` بنائیں:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// مرحلہ 1: بوٹ اسٹرپ - مینیجر بنائیں، سروس شروع کریں، اور ماڈل لوڈ کریں
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

// مرحلہ 2: ایک آڈیو کلائنٹ بنائیں اور نقل تیار کریں
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// صفائی کریں
await model.unload();
```
  
> **نوٹ:** Foundry Local SDK `model.createAudioClient()` کے ذریعے بلٹ ان `AudioClient` فراہم کرتا ہے جو مکمل ONNX اندازے کے عمل کو خود بخود سنبھالتا ہے — `onnxruntime-node` امپورٹ کی ضرورت نہیں۔ انگریزی ٹرانسکرپشن کی درستگی کے لیے ہمیشہ `audioClient.settings.language = "en"` سیٹ کریں۔

#### اسے چلائیں

```bash
# ایک زاوہ پروڈکٹ منظر نامہ نقل کریں
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# یا دوسرے آزمائیں:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```
  
#### JavaScript کے اہم نکات

| طریقہ | مقصد |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | مینیجر سنگلٹن بنائیں |
| `await catalog.getModel(alias)` | کیٹلاگ سے ماڈل حاصل کریں |
| `model.download()` / `model.load()` | Whisper ماڈل ڈاؤنلوڈ اور لوڈ کریں |
| `model.createAudioClient()` | ٹرانسکرپشن کے لیے آڈیو کلائنٹ بنائیں |
| `audioClient.settings.language = "en"` | ٹرانسکرپشن زبان سیٹ کریں (صحیح نتائج کے لیے ضروری) |
| `audioClient.transcribe(path)` | آڈیو فائل کی ٹرانسکرپشن کریں، `{ text, duration }` واپس کرے گا |

</details>

<details>
<summary><h3>C# ٹریک</h3></summary>

#### سیٹ اپ

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```
  
> **نوٹ:** C# ٹریک `Microsoft.AI.Foundry.Local` پیکیج استعمال کرتا ہے جو `model.GetAudioClientAsync()` کے ذریعے بلٹ ان `AudioClient` فراہم کرتا ہے۔ یہ پورے ٹرانسکرپشن پائپ لائن کو پراسیس کے اندر سنبھالتا ہے — ONNX Runtime کا علحیدہ سیٹ اپ نہیں چاہیے۔

#### ٹرانسکرپشن کوڈ

`Program.cs` کا مواد تبدیل کریں:

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
  
#### اسے چلائیں

```bash
# ایک Zava پروڈکٹ منظر نامہ نقل کریں
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# یا دوسروں کو آزمائیں:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```
  
#### C# کے اہم نکات  

| طریقہ | مقصد |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Local کو کانفیگریشن کے ساتھ شروع کریں |
| `catalog.GetModelAsync(alias)` | کیٹلاگ سے ماڈل حاصل کریں |
| `model.DownloadAsync()` | Whisper ماڈل ڈاؤنلوڈ کریں |
| `model.GetAudioClientAsync()` | AudioClient حاصل کریں (ChatClient نہیں!) |
| `audioClient.Settings.Language = "en"` | ٹرانسکرپشن زبان سیٹ کریں (درست نتیجہ کے لیے ضروری) |
| `audioClient.TranscribeAudioAsync(path)` | آڈیو فائل کی ٹرانسکرپشن کریں |
| `result.Text` | ٹرانسکرائب شدہ متن |


> **سی# بمقابلہ پائتھن/جے ایس:** سی# ایس ڈی کے ایک بلٹ ان `AudioClient` فراہم کرتا ہے جو `model.GetAudioClientAsync()` کے ذریعے ان-پراسس ٹرانسکرپشن کے لیے ہے، بالکل جاوا اسکرپٹ ایس ڈی کے کی طرح۔ پائتھن ONNX رن ٹائم کو براہ راست تفہیم کے لیے استعمال کرتا ہے جو کیش کردہ انکوڈر/ڈیکوڈر ماڈلز کے خلاف ہے۔

</details>

---

### مشق 4 - تمام زاوہ نمونوں کا بیچ میں ٹرانسکرائب کریں

اب جبکہ آپ کے پاس ایک کام کرنے والا ٹرانسکرپشن ایپ ہے، تمام پانچ زاوہ سیمپل فائلز کی ٹرانسکرپشن کریں اور نتائج کا موازنہ کریں۔

<details>
<summary><h3>پائتھن ٹریک</h3></summary>

مکمل سیمپل `python/foundry-local-whisper.py` پہلے سے بیچ ٹرانسکرپشن کی حمایت کرتا ہے۔ بغیر کسی دلائل کے چلانے پر، یہ `samples/audio/` میں موجود تمام `zava-*.wav` فائلز کی ٹرانسکرپشن کرتا ہے:

```bash
cd python
python foundry-local-whisper.py
```

نمونہ `FoundryLocalManager(alias)` کو بوٹ اسٹریپ کرنے کے لیے استعمال کرتا ہے، پھر ہر فائل کے لیے انکوڈر اور ڈیکوڈر ONNX سیشنز چلائے جاتے ہیں۔

</details>

<details>
<summary><h3>جاوا اسکرپٹ ٹریک</h3></summary>

مکمل سیمپل `javascript/foundry-local-whisper.mjs` پہلے سے بیچ ٹرانسکرپشن کی حمایت کرتا ہے۔ بغیر کسی دلائل کے چلانے پر، یہ `samples/audio/` میں موجود تمام `zava-*.wav` فائلز کی ٹرانسکرپشن کرتا ہے:

```bash
cd javascript
node foundry-local-whisper.mjs
```

نمونہ `FoundryLocalManager.create()` اور `catalog.getModel(alias)` کو ایس ڈی کے کو ابتدائی کرنے کے لیے استعمال کرتا ہے، پھر `AudioClient` (ساتھ `settings.language = "en"`) ہر فائل کی ٹرانسکرپشن کے لیے استعمال ہوتا ہے۔

</details>

<details>
<summary><h3>سی# ٹریک</h3></summary>

مکمل سیمپل `csharp/WhisperTranscription.cs` پہلے سے بیچ ٹرانسکرپشن کی حمایت کرتا ہے۔ بغیر کسی مخصوص فائل آرگیومنٹ کے چلانے پر، یہ `samples/audio/` میں موجود تمام `zava-*.wav` فائلز کی ٹرانسکرپشن کرتا ہے:

```bash
cd csharp
dotnet run whisper
```

نمونہ `FoundryLocalManager.CreateAsync()` اور ایس ڈی کے کے `AudioClient` (ساتھ `Settings.Language = "en"`) کو ان-پراسس ٹرانسکرپشن کے لیے استعمال کرتا ہے۔

</details>

**دیکھنے کے لیے کیا ہے:** ٹرانسکرپشن آؤٹ پٹ کا موازنہ `samples/audio/generate_samples.py` میں اصل ٹیکسٹ سے کریں۔ یہ دیکھیں کہ وائسپر مصنوعات کے نام جیسے "Zava ProGrip" اور تکنیکی اصطلاحات جیسے "brushless motor" یا "composite decking" کو کتنی درستگی سے کیپچر کرتا ہے؟

---

### مشق 5 - کلیدی کوڈ پیٹرنز کو سمجھیں

مِن تینوں زبانوں میں وائسپر ٹرانسکرپشن چیٹ کمپلیشنز سے کیسے مختلف ہے، اس کا مطالعہ کریں:

<details>
<summary><b>پائتھن - چیٹ سے کلیدی فرق</b></summary>

```python
# چیٹ تکمیل (حصے 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# آڈیو تحریر (یہ حصہ):
# براہ راست ONNX رن ٹائم استعمال کرتا ہے بجائے OpenAI کلائنٹ کے
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... خودکار تسلسلی ڈی کوڈر لوپ ...
print(tokenizer.decode(generated_tokens))
```

**کلیدی بصیرت:** چیٹ ماڈلز `manager.endpoint` کے ذریعے OpenAI-مطابق API استعمال کرتے ہیں۔ وائسپر ایس ڈی کے کا استعمال کر کے کیش شدہ ONNX ماڈل فائلز کو تلاش کرتا ہے، پھر ONNX رن ٹائم کے ساتھ براہ راست تفہیم کرتا ہے۔

</details>

<details>
<summary><b>جاوا اسکرپٹ - چیٹ سے کلیدی فرق</b></summary>

```javascript
// چیٹ مکمل کرنا (حصے 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// آڈیو تحریر (یہ حصہ):
// SDK کے بلٹ ان AudioClient کا استعمال کرتا ہے
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // بہترین نتائج کے لیے ہمیشہ زبان سیٹ کریں
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**کلیدی بصیرت:** چیٹ ماڈلز `manager.urls[0] + "/v1"` کے ذریعے OpenAI-مطابق API استعمال کرتے ہیں۔ وائسپر ٹرانسکرپشن ایس ڈی کے کے `AudioClient` کو استعمال کرتا ہے، جو `model.createAudioClient()` سے حاصل ہوتا ہے۔ `settings.language` سیٹ کریں تاکہ خودکار شناخت سے خراب آؤٹ پٹ نہ آئے۔

</details>

<details>
<summary><b>سی# - چیٹ سے کلیدی فرق</b></summary>

سی# طریقہ کار ایس ڈی کے کے بلٹ ان `AudioClient` کو ان-پراسس ٹرانسکرپشن کے لیے استعمال کرتا ہے:

**ماڈل کی ابتدائی تشکیل:**

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

**ٹرانسکرپشن:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**کلیدی بصیرت:** سی# `FoundryLocalManager.CreateAsync()` استعمال کرتا ہے اور براہ راست ایک `AudioClient` حاصل کرتا ہے — ONNX رن ٹائم کی ضرورت نہیں۔ `Settings.Language` سیٹ کریں تاکہ خودکار شناخت سے خراب آؤٹ پٹ نہ آئے۔

</details>

> **خلاصہ:** پائتھن ماڈل مینجمنٹ کے لیے Foundry Local SDK اور انکوڈر/ڈیکوڈر ماڈلز کے خلاف براہ راست انفیرنس کے لیے ONNX رن ٹائم استعمال کرتا ہے۔ جاوا اسکرپٹ اور سی# دونوں ایس ڈی کے کے بلٹ ان `AudioClient` کو ٹرانسکرپشن کے لیے استعمال کرتے ہیں — کلائنٹ بنائیں، زبان سیٹ کریں، اور `transcribe()` / `TranscribeAudioAsync()` کال کریں۔ درست نتائج کے لیے ہمیشہ AudioClient پر زبان کی پراپرٹی سیٹ کریں۔

---

### مشق 6 - تجربہ کریں

سمجھ کو گہرا کرنے کے لیے یہ تبدیلیاں آزما کر دیکھیں:

1. **مختلف آڈیو فائلز آزمائیں** - ونڈوز وائس ریکارڈر استعمال کر کے خود بولیں، WAV میں محفوظ کریں، اور اسے ٹرانسکرائب کریں

2. **ماڈل ویرینٹس کا موازنہ کریں** - اگر آپ کے پاس NVIDIA GPU ہے، تو CUDA ویرینٹ آزمائیں:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   ٹرانسکرپشن کی رفتار کا موازنہ CPU ویرینٹ سے کریں۔

3. **آؤٹ پٹ فارمیٹنگ شامل کریں** - JSON جواب میں یہ شامل ہو سکتا ہے:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **REST API بنائیں** - اپنی ٹرانسکرپشن کوڈ کو ایک ویب سرور میں لپیٹیں:

   | زبان | فریم ورک | مثال |
   |----------|-----------|--------|
   | پائتھن | FastAPI | `@app.post("/v1/audio/transcriptions")` کے ساتھ `UploadFile` |
   | جاوا اسکرپٹ | Express.js | `app.post("/v1/audio/transcriptions")` کے ساتھ `multer` |
   | سی# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` کے ساتھ `IFormFile` |

5. **کئی مرحلوں میں ٹرانسکرپشن کے ساتھ ملائیں** - وائسپر کو پارٹ 4 کے چیٹ ایجنٹ کے ساتھ جوڑیں: پہلے آڈیو ٹرانسکرائب کریں، پھر متن ایجنٹ کو تجزیے یا خلاصے کے لیے دیں۔

---

## ایس ڈی کے آڈیو API حوالہ

> **جاوا اسکرپٹ AudioClient:**
> - `model.createAudioClient()` — ایک `AudioClient` کی مثال بناتا ہے
> - `audioClient.settings.language` — ٹرانسکرپشن زبان سیٹ کریں (مثلاً `"en"`)
> - `audioClient.settings.temperature` — رینڈم نیس کنٹرول کریں (اختیاری)
> - `audioClient.transcribe(filePath)` — فائل ٹرانسکرائب کرتا ہے، `{ text, duration }` لوٹاتا ہے
> - `audioClient.transcribeStreaming(filePath, callback)` — کال بیک کے ذریعے ٹرانسکرپشن کے ٹکڑے سٹریم کرتا ہے
>
> **سی# AudioClient:**
> - `await model.GetAudioClientAsync()` — ایک `OpenAIAudioClient` کی مثال بناتا ہے
> - `audioClient.Settings.Language` — ٹرانسکرپشن زبان سیٹ کریں (مثلاً `"en"`)
> - `audioClient.Settings.Temperature` — رینڈم نیس کنٹرول کریں (اختیاری)
> - `await audioClient.TranscribeAudioAsync(filePath)` — فائل ٹرانسکرائب کرتا ہے، `.Text` سمیت آبجیکٹ لوٹاتا ہے
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — ٹرانسکرپشن کے ٹکڑوں کا `IAsyncEnumerable` لوٹاتا ہے

> **ٹپ:** ٹرانسکرپشن شروع کرنے سے پہلے زبان کی پراپرٹی ہمیشہ سیٹ کریں۔ اگر یہ نہ ہو تو وائسپر ماڈل خودکار شناخت کی کوشش کرتا ہے، جو گڑبڑ شدہ آؤٹ پٹ پیدا کر سکتا ہے (متن کی جگہ ایک علامتی حرف آنا)۔

---

## موازنہ: چیٹ ماڈلز بمقابلہ وائسپر

| پہلو | چیٹ ماڈلز (حصے 3-7) | وائسپر - پائتھن | وائسپر - جے ایس / سی# |
|--------|------------------------|--------------------|--------------------|
| **ٹاسک کی قسم** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **ان پٹ** | ٹیکسٹ میسجز (JSON) | آڈیو فائلز (WAV/MP3/M4A) | آڈیو فائلز (WAV/MP3/M4A) |
| **آؤٹ پٹ** | تیار شدہ متن (سٹریم شدہ) | ٹرانسکرائب شدہ متن (مکمل) | ٹرانسکرائب شدہ متن (مکمل) |
| **ایس ڈی کے پیکیج** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API طریقہ** | `client.chat.completions.create()` | ONNX رن ٹائم براہ راست | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **زبان کی ترتیبات** | لاگو نہیں | ڈیکوڈر پرامپٹ ٹوکنز | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **سٹریمینگ** | ہاں | نہیں | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **پرائیویسی فائدہ** | کوڈ/ڈیٹا لوکل رہتا ہے | آڈیو ڈیٹا لوکل رہتا ہے | آڈیو ڈیٹا لوکل رہتا ہے |

---

## اہم نکات

| تصور | آپ نے کیا سیکھا |
|---------|-----------------|
| **وائسپر آن-ڈیوائس** | تقریر سے متن کی تبدیلی مکمل طور پر لوکل چلتی ہے، جو زاوہ کسٹمر کالز اور مصنوعات کے جائزے آن-ڈیوائس ٹرانسکرائب کرنے کے لیے مثالی ہے |
| **ایس ڈی کے آڈیو کلائنٹ** | جاوا اسکرپٹ اور سی# ایس ڈی کےز ایک بلٹ ان `AudioClient` فراہم کرتے ہیں جو مکمل ٹرانسکرپشن پائپ لائن کو ایک ہی کال میں سنبھالتا ہے |
| **زبان کی ترتیبات** | ہمیشہ AudioClient کی زبان سیٹ کریں (مثلاً `"en"`) — ورنہ خودکار شناخت سے گڑبڑ شدہ آؤٹ پٹ پیدا ہو سکتی ہے |
| **پائتھن** | ماڈل مینجمنٹ کے لیے `foundry-local-sdk` + `onnxruntime` + `transformers` + `librosa` براہ راست ONNX انفیرنس کے لیے استعمال کرتا ہے |
| **جاوا اسکرپٹ** | `foundry-local-sdk` کے ساتھ `model.createAudioClient()` استعمال کرتا ہے — `settings.language` سیٹ کریں، پھر `transcribe()` کال کریں |
| **سی#** | `Microsoft.AI.Foundry.Local` کے ساتھ `model.GetAudioClientAsync()` استعمال کرتا ہے — `Settings.Language` سیٹ کریں، پھر `TranscribeAudioAsync()` کال کریں |
| **سٹریمینگ کی حمایت** | JS اور سی# ایس ڈی کےز `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` بھی فراہم کرتے ہیں تاکہ چنکس میں آؤٹ پٹ ملے |
| **CPU-بہتر شدہ** | CPU ویرینٹ (3.05 جی بی) بغیر GPU کے کسی بھی ونڈوز ڈیوائس پر کام کرتا ہے |
| **پرائیویسی فرسٹ** | زاوہ کسٹمر تعاملات اور مخصوص پروڈکٹ ڈیٹا کو ڈیوائس پر ہی رکھنے کے لیے بہترین |

---

## وسائل

| وسیلہ | لنک |
|----------|------|
| Foundry Local دستاویزات | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK حوالہ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI وائسپر ماڈل | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local ویب سائٹ | [foundrylocal.ai](https://foundrylocal.ai) |

---

## اگلا قدم

[حصہ 10: کسٹم یا ہگزنگ فیس ماڈلز کا استعمال](part10-custom-models.md) پر جاری رکھیں تاکہ ہگزنگ فیس سے اپنے ماڈلز کمپائل کریں اور انہیں Foundry Local کے ذریعے چلائیں۔