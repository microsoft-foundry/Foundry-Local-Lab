![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# الجزء 9: النسخ الصوتي باستخدام Whisper وFoundry Local

> **الهدف:** استخدام نموذج OpenAI Whisper الذي يعمل محليًا عبر Foundry Local لنسخ ملفات الصوت - بالكامل على الجهاز، بدون حاجة للسحابة.

## نظرة عامة

ليس Foundry Local فقط لتوليد النص؛ بل يدعم أيضًا نماذج **تحويل الكلام إلى نص**. في هذا المختبر ستستخدم نموذج **OpenAI Whisper Medium** لنسخ ملفات الصوت بالكامل على جهازك. هذا مثالي لسيناريوهات مثل نسخ مكالمات خدمة عملاء Zava، تسجيلات مراجعات المنتجات، أو جلسات تخطيط الورش حيث يجب ألا تغادر بيانات الصوت جهازك أبدًا.

---

## أهداف التعلم

بحلول نهاية هذا المختبر ستتمكن من:

- فهم نموذج Whisper لتحويل الكلام إلى نص وقدراته
- تنزيل وتشغيل نموذج Whisper باستخدام Foundry Local
- نسخ ملفات الصوت باستخدام Foundry Local SDK في بايثون، جافاسكريبت، وC#
- بناء خدمة نسخ بسيطة تعمل بالكامل على الجهاز
- فهم الفروقات بين نماذج الدردشة/النص ونماذج الصوت في Foundry Local

---

## المتطلبات الأساسية

| المتطلب | التفاصيل |
|-------------|---------|
| **Foundry Local CLI** | الإصدار **0.8.101 أو أعلاه** (نماذج Whisper متوفرة من الإصدار 0.8.101 فصاعدًا) |
| **نظام التشغيل** | ويندوز 10/11 (x64 أو ARM64) |
| **بيئة التشغيل** | **بايثون 3.9+** و/أو **Node.js 18+** و/أو **.NET 9 SDK** ([تحميل .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **مكتمل** | [الجزء 1: البدء](part1-getting-started.md)، [الجزء 2: الغوص في Foundry Local SDK](part2-foundry-local-sdk.md)، و[الجزء 3: SDKs وAPIs](part3-sdk-and-apis.md) |

> **ملاحظة:** يجب تنزيل نماذج Whisper عبر **SDK** (وليس CLI). الـ CLI لا يدعم نقطة نهاية النسخ الصوتي. تحقق من نسختك عبر:
> ```bash
> foundry --version
> ```

---

## المفهوم: كيف يعمل Whisper مع Foundry Local

نموذج OpenAI Whisper هو نموذج تعرف على الكلام متعدد الأغراض تدرب على مجموعة بيانات ضخمة ومتنوعة من الصوت. عند التشغيل عبر Foundry Local:

- النموذج يعمل **كليًا على الـCPU الخاص بك** - لا حاجة لـGPU
- الصوت لا يغادر جهازك - **خصوصية تامة**
- يتولى Foundry Local SDK تنزيل النموذج وإدارة التخزين المؤقت
- **جافاسكريبت وC#** يوفران ضمن SDK كائنًا مدمجًا `AudioClient` يتولى كامل عملية النسخ — لا حاجة لإعداد ONNX يدويًا
- **بايثون** يستخدم SDK لإدارة النموذج وONNX Runtime للاستدلال المباشر على نماذج التشفير/فك التشفير بصيغة ONNX

### كيف تعمل سلسلة المعالجة (جافاسكريبت وC#) — SDK AudioClient

1. **Foundry Local SDK** يقوم بتنزيل وتخزين نموذج Whisper مؤقتًا
2. `model.createAudioClient()` (جافاسكريبت) أو `model.GetAudioClientAsync()` (C#) ينشئ كائن `AudioClient`
3. `audioClient.transcribe(path)` (جافاسكريبت) أو `audioClient.TranscribeAudioAsync(path)` (C#) يعالج كامل السلسلة داخليًا — تحويل الصوت، التشفير، فك التشفير، وفك ترميز رموز النص
4. يعرض كائن `AudioClient` خاصية `settings.language` (مضبوطة على `"en"` للإنجليزية) لتوجيه النسخ بدقة

### كيف تعمل سلسلة المعالجة (بايثون) — ONNX Runtime

1. **Foundry Local SDK** ينزل ويخزن مؤقتًا ملفات نموذج Whisper ONNX
2. **تحويل الصوت** يحول صوت WAV إلى مخطط طيف ميل (80 حاوية ميل × 3000 إطار)
3. **المشفر (Encoder)** يعالج مخطط الطيف الميل وينتج حالات مخفية بالإضافة إلى تينسورات مفاتيح/قيم التركيز المتقاطع
4. **فك الشفرة (Decoder)** يعمل بشكل تلقائي توليدي، ينتج رمزًا واحدًا في كل مرة حتى ينتج رمز نهاية النص
5. **فك الترميز** يحول معرّفات الرموز الناتجة إلى نص مقروء

### أنواع نماذج Whisper

| الاسم المستعار | معرف النموذج | الجهاز | الحجم | الوصف |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 جيجابايت | معجل GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 جيجابايت | مخصص لـCPU (موصى به لمعظم الأجهزة) |

> **ملاحظة:** بعكس نماذج الدردشة المدرجة بشكل افتراضي، تُصنف نماذج Whisper تحت مهمة `automatic-speech-recognition`. استخدم `foundry model info whisper-medium` لرؤية التفاصيل.

---

## تمارين المختبر

### التمرين 0 - الحصول على ملفات الصوت النموذجية

يحتوي هذا المختبر على ملفات WAV مبنية مسبقًا بناءً على سيناريوهات منتجات Zava DIY. أنشئها باستخدام السكريبت المرفق:

```bash
# من جذر المستودع - قم بإنشاء وتفعيل .venv أولاً
python -m venv .venv

# ويندوز (PowerShell):
.venv\Scripts\Activate.ps1
# ماك أو إس:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

هذا ينشئ ستة ملفات WAV في `samples/audio/`:

| الملف | السيناريو |
|------|----------|
| `zava-customer-inquiry.wav` | عميل يستفسر عن **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | مراجعة منتج من عميل لـ **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | مكالمة دعم حول **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | مخطط مشاريع DIY لسطح باستخدام **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | جولة في ورشة تستخدم **كل منتجات Zava الخمسة** |
| `zava-full-project-walkthrough.wav` | جولة مطولة لتجديد المرآب تستخدم **كل منتجات Zava** (~4 دقائق، لاختبار الصوت الطويل) |

> **نصيحة:** يمكنك أيضًا استخدام ملفات WAV/MP3/M4A خاصة بك، أو تسجيل صوتك باستخدام مسجل صوت ويندوز.

---

### التمرين 1 - تنزيل نموذج Whisper باستخدام الـ SDK

بسبب عدم توافق CLI مع نماذج Whisper في إصدارات Foundry Local الحديثة، استخدم **SDK** لتنزيل وتحميل النموذج. اختر لغتك:

<details>
<summary><b>🐍 بايثون</b></summary>

**تثبيت SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# ابدأ الخدمة
manager = FoundryLocalManager()
manager.start_service()

# تحقق من معلومات الكتالوج
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# تحقق مما إذا كان مخزنًا مؤقتًا بالفعل
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# حمّل النموذج في الذاكرة
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

احفظه باسم `download_whisper.py` وشغّله:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 جافاسكريبت</b></summary>

**تثبيت SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// إنشاء المدير وبدء الخدمة
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// الحصول على النموذج من الكتالوج
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

// تحميل النموذج في الذاكرة
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

احفظه باسم `download-whisper.mjs` وشغّله:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**تثبيت SDK:**
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

> **لماذا SDK وليس CLI؟** لا يدعم Foundry Local CLI تنزيل أو خدمة نماذج Whisper مباشرةً. يوفر SDK طريقة موثوقة لتنزيل وإدارة نماذج الصوت برمجيًا. تتضمن SDK لجافاسكريبت وC# كائن `AudioClient` مدمج يتولى كامل عملية النسخ. تستخدم بايثون ONNX Runtime للاستدلال المباشر على ملفات النموذج المخزنة مؤقتًا.

---

### التمرين 2 - فهم SDK الخاص بنموذج Whisper

يستخدم النسخ الصوتي لWhisper طرقًا مختلفة وفقًا للغة. توفر **جافاسكريبت وC#** كائنًا مدمجًا `AudioClient` في Foundry Local SDK يعالج كامل السلسلة (تحويل الصوت، التشفير، فك التشفير، فك الترميز) في دالة واحدة. تستخدم **بايثون** Foundry Local SDK لإدارة النموذج وONNX Runtime للاستدلال المباشر على نماذج التشفير/فك التشفير بتنسيق ONNX.

| المكون | بايثون | جافاسكريبت | C# |
|-----------|--------|------------|----|
| **حزم SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **إدارة النموذج** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + الكاتالوج |
| **استخراج الخصائص** | `WhisperFeatureExtractor` + `librosa` | تعالجها SDK `AudioClient` | تعالجها SDK `AudioClient` |
| **الاستدلال** | `ort.InferenceSession` (المشفر + المفكك) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **فك الترميز** | `WhisperTokenizer` | تعالجه SDK `AudioClient` | تعالجه SDK `AudioClient` |
| **إعداد اللغة** | يتم عبر `forced_ids` في رموز المفكك | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **الإدخال** | مسار ملف WAV | مسار ملف WAV | مسار ملف WAV |
| **الإخراج** | نص مفكك | `result.text` | `result.Text` |

> **مهم:** اضبط دائمًا اللغة على `AudioClient` (مثل `"en"` للإنجليزية). بدون إعداد لغة صريح، قد ينتج النموذج مخرجات مشوشة أثناء محاولته الكشف التلقائي عن اللغة.

> **أنماط SDK:** تستخدم بايثون `FoundryLocalManager(alias)` للتهيئة، ثم `get_cache_location()` للعثور على ملفات نموذج ONNX. تستخدم جافاسكريبت وC# كائن `AudioClient` المدمج في SDK — الذي يتم الحصول عليه عبر `model.createAudioClient()` (جافاسكريبت) أو `model.GetAudioClientAsync()` (C#) — ويتولى كامل عملية النسخ. راجع [الجزء 2: الغوص في Foundry Local SDK](part2-foundry-local-sdk.md) للتفاصيل الكاملة.

---

### التمرين 3 - بناء تطبيق نسخ بسيط

اختر مسارك اللغوي وابنِ تطبيقًا بسيطًا ينسخ ملف صوتي.

> **الصيغ المدعومة للصوت:** WAV، MP3، M4A. لأفضل النتائج، استخدم ملفات WAV بمعدل عينة 16 كيلو هرتز.

<details>
<summary><h3>مسار بايثون</h3></summary>

#### الإعداد

```bash
cd python
python -m venv venv

# تفعيل البيئة الافتراضية:
# ويندوز (PowerShell):
venv\Scripts\Activate.ps1
# ماك أو إس:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### كود النسخ

أنشئ ملف `foundry-local-whisper.py`:

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

# الخطوة 1: التمهيد - يبدأ الخدمة، ينزل، ويحمل النموذج
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# بناء المسار لملفات نموذج ONNX المؤقتة
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# الخطوة 2: تحميل جلسات ONNX ومستخرج الميزات
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

# الخطوة 3: استخراج ميزات طيف ميل
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# الخطوة 4: تشغيل المشفر
enc_out = encoder.run(None, {"audio_features": input_features})
# الإخراج الأول هو الحالات المخفية؛ الباقي هي أزواج KV للانتباه المتقاطع
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# الخطوة 5: فك الترميز التلقائي التتابعي
initial_tokens = [50258, 50259, 50359, 50363]  # sot، en، التفريغ، بدون علامات زمنية
input_ids = np.array([initial_tokens], dtype=np.int32)

# تفريغ ذاكرة تخزين KV للانتباه الذاتي
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

    if next_token == 50257:  # نهاية النص
        break
    generated.append(next_token)

    # تحديث ذاكرة تخزين KV للانتباه الذاتي
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### تشغيله

```bash
# نسخ سيناريو منتج زافا
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# أو جرب أخرى:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### نقاط رئيسية في بايثون

| الدالة | الغرض |
|--------|---------|
| `FoundryLocalManager(alias)` | التهيئة: بدء الخدمة، التنزيل، وتحميل النموذج |
| `manager.get_cache_location()` | الحصول على مسار ملفات نموذج ONNX المخزنة مؤقتًا |
| `WhisperFeatureExtractor.from_pretrained()` | تحميل مستخرج خصائص مخطط الطبقة الصوتية |
| `ort.InferenceSession()` | إنشاء جلسات ONNX Runtime للمشفر والمفكك |
| `tokenizer.decode()` | تحويل معرّفات الرموز الناتجة إلى نص |

</details>

<details>
<summary><h3>مسار جافاسكريبت</h3></summary>

#### الإعداد

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### كود النسخ

أنشئ ملف `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// الخطوة ١: التمهيد - إنشاء المدير، بدء الخدمة، وتحميل النموذج
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

// الخطوة ٢: إنشاء عميل صوتي وتحويل الكلام إلى نص
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// التنظيف
await model.unload();
```

> **ملاحظة:** يوفر Foundry Local SDK كائن `AudioClient` مدمج عبر `model.createAudioClient()` يتولى كامل خط أنابيب استدلال ONNX داخليًا — لا حاجة لاستيراد `onnxruntime-node`. اضبط دائمًا `audioClient.settings.language = "en"` لضمان نسخ دقيق باللغة الإنجليزية.

#### تشغيله

```bash
# نسخ سيناريو منتج زافا
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# أو جرب أخرى:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### نقاط رئيسية في جافاسكريبت

| الدالة | الغرض |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | إنشاء مفردة المدير |
| `await catalog.getModel(alias)` | الحصول على نموذج من الكتالوج |
| `model.download()` / `model.load()` | تنزيل وتحميل نموذج Whisper |
| `model.createAudioClient()` | إنشاء عميل صوتي للنسخ |
| `audioClient.settings.language = "en"` | تعيين لغة النسخ (مطلوب لخروج دقيق) |
| `audioClient.transcribe(path)` | نسخ ملف صوتي، يعيد `{ text, duration }` |

</details>

<details>
<summary><h3>مسار C#</h3></summary>

#### الإعداد

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **ملاحظة:** يستخدم مسار C# الحزمة `Microsoft.AI.Foundry.Local` التي توفر كائنًا مدمجًا `AudioClient` عبر `model.GetAudioClientAsync()`. هذا يتولى خط أنابيب النسخ الكامل في العملية — لا حاجة لإعداد ONNX Runtime منفصل.

#### كود النسخ

استبدل محتويات `Program.cs`:

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

#### تشغيله

```bash
# نسخ سيناريو منتج Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# أو جرب أخرى:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### نقاط رئيسية في C#

| الدالة | الغرض |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | تهيئة Foundry Local بالتكوين |
| `catalog.GetModelAsync(alias)` | الحصول على النموذج من الكتالوج |
| `model.DownloadAsync()` | تنزيل نموذج Whisper |
| `model.GetAudioClientAsync()` | الحصول على AudioClient (ليس ChatClient!) |
| `audioClient.Settings.Language = "en"` | تعيين لغة النسخ (مطلوب لخروج دقيق) |
| `audioClient.TranscribeAudioAsync(path)` | نسخ ملف صوتي |
| `result.Text` | النص المنسوخ |
> **C# مقابل Python/JS:** يوفر SDK الخاص بـ C# كائن `AudioClient` مدمجًا للتفريغ النصي داخل العملية عبر `model.GetAudioClientAsync()`، مماثل لـ SDK الجافاسكريبت. يستخدم Python ONNX Runtime مباشرة للاستدلال مقابل نماذج المشفر/فك التشفير المخزنة مؤقتًا.

</details>

---

### التمرين 4 - تفريغ دفعي لجميع عينات Zava

الآن بعد أن لديك تطبيق تفريغ نصي يعمل، قم بتفريغ جميع ملفات العينات الخمسة لـ Zava وقارن النتائج.

<details>
<summary><h3>مسار Python</h3></summary>

يدعم العينة الكاملة `python/foundry-local-whisper.py` بالفعل التفريغ الدفعي. عند التشغيل بدون معطيات، يتم تفريغ جميع ملفات `zava-*.wav` في `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

تستخدم العينة `FoundryLocalManager(alias)` للتهيئة، ثم تشغل جلسات ONNX للمشفر وفك التشفير لكل ملف.

</details>

<details>
<summary><h3>مسار JavaScript</h3></summary>

يدعم العينة الكاملة `javascript/foundry-local-whisper.mjs` بالفعل التفريغ الدفعي. عند التشغيل بدون معطيات، يتم تفريغ جميع ملفات `zava-*.wav` في `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

تستخدم العينة `FoundryLocalManager.create()` و `catalog.getModel(alias)` لتهيئة SDK، ثم تستخدم `AudioClient` (مع `settings.language = "en"`) لتفريغ كل ملف.

</details>

<details>
<summary><h3>مسار C#</h3></summary>

يدعم العينة الكاملة `csharp/WhisperTranscription.cs` بالفعل التفريغ الدفعي. عند التشغيل بدون تحديد ملف معين، يتم تفريغ جميع ملفات `zava-*.wav` في `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

تستخدم العينة `FoundryLocalManager.CreateAsync()` وكائن `AudioClient` من SDK (مع `Settings.Language = "en"`) للتفريغ داخل العملية.

</details>

**ماذا تبحث عنه:** قارن النص المُفرغ مع النص الأصلي في `samples/audio/generate_samples.py`. ما مدى دقة Whisper في التقاط أسماء المنتجات مثل "Zava ProGrip" والمصطلحات التقنية مثل "brushless motor" أو "composite decking"؟

---

### التمرين 5 - فهم أنماط الكود الأساسية

ادرس كيف يختلف تفريغ Whisper النصي عن استكمالات الدردشة عبر اللغات الثلاث:

<details>
<summary><b>Python - الاختلافات الرئيسية عن الدردشة</b></summary>

```python
# إكمال الدردشة (الأجزاء 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# تحويل الصوت إلى نص (هذا الجزء):
# يستخدم ONNX Runtime مباشرة بدلاً من عميل OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... حلقة فك الترميز التلقائي التسلسلي ...
print(tokenizer.decode(generated_tokens))
```

**النقطة الأساسية:** تستخدم نماذج الدردشة API متوافق مع OpenAI عبر `manager.endpoint`. يستخدم Whisper SDK لتحديد أماكن ملفات نموذج ONNX المخزنة مؤقتًا، ثم يشغل الاستدلال مباشرة مع ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - الاختلافات الرئيسية عن الدردشة</b></summary>

```javascript
// إكمال الدردشة (الأجزاء 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// نسخ الصوت (هذا الجزء):
// يستخدم AudioClient المدمج في SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // قم دائمًا بتعيين اللغة للحصول على أفضل النتائج
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**النقطة الأساسية:** تستخدم نماذج الدردشة API متوافق مع OpenAI عبر `manager.urls[0] + "/v1"`. يستخدم تفريغ Whisper في SDK كائن `AudioClient`، الموجود عبر `model.createAudioClient()`. عيّن `settings.language` لتجنب نتائج مشوشة من الكشف التلقائي.

</details>

<details>
<summary><b>C# - الاختلافات الرئيسية عن الدردشة</b></summary>

تستخدم طريقة C# كائن `AudioClient` المدمج في SDK للتفريغ داخل العملية:

**تهيئة النموذج:**

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

**التفريغ:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**النقطة الأساسية:** تستخدم C# `FoundryLocalManager.CreateAsync()` وتحصل مباشرة على `AudioClient` — لا حاجة لإعداد ONNX Runtime. عيّن `Settings.Language` لتجنب نتائج مشوشة من الكشف التلقائي.

</details>

> **الملخص:** يستخدم Python SDK الخاص بـ Foundry Local لإدارة النموذج و ONNX Runtime للاستدلال المباشر مقابل نماذج المشفر/فك التشفير. يستخدم JavaScript و C# كائن `AudioClient` المدمج في SDK لتفريغ نصي مبسط — أنشئ العميل، عيّن اللغة، واستدعِ `transcribe()` / `TranscribeAudioAsync()`. يجب دائمًا تعيين خاصية اللغة على AudioClient للحصول على نتائج دقيقة.

---

### التمرين 6 - تجربة

جرّب هذه التعديلات لتعميق فهمك:

1. **جرب ملفات صوتية مختلفة** - سجل صوتك باستخدام مسجل الصوت في ويندوز، احفظه بصيغة WAV، وافرغ النص

2. **قارن بين نسخ النماذج** - إذا كان لديك وحدة معالجة رسومات NVIDIA، جرب النسخة CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   قارن سرعة التفريغ مع النسخة التي تعمل على المعالج المركزي (CPU).

3. **أضف تنسيقًا للنص الناتج** - يمكن أن تتضمن استجابة JSON:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **بناء REST API** - غلف كود التفريغ النصي الخاص بك في خادم ويب:

   | اللغة | الإطار | مثال |
   |--------|---------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` مع `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` مع `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` مع `IFormFile` |

5. **دورات متعددة مع التفريغ** - اجمع Whisper مع وكيل دردشة من الجزء 4: افرد الصوت أولاً، ثم مرر النص إلى الوكيل للتحليل أو التلخيص.

---

## مرجع API الصوتي لـ SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — ينشئ مثيل `AudioClient`
> - `audioClient.settings.language` — تعيين لغة التفريغ النصي (مثل `"en"`)
> - `audioClient.settings.temperature` — التحكم بالعشوائية (اختياري)
> - `audioClient.transcribe(filePath)` — يفريغ ملف نصيًا، يرجع `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — تدفق أجزاء التفريغ عبر رد الاتصال
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — ينشئ مثيل `OpenAIAudioClient`
> - `audioClient.Settings.Language` — تعيين لغة التفريغ النصي (مثل `"en"`)
> - `audioClient.Settings.Temperature` — التحكم بالعشوائية (اختياري)
> - `await audioClient.TranscribeAudioAsync(filePath)` — يفريغ ملف نصيًا، يرجع كائن مع `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — يرجع `IAsyncEnumerable` لأجزاء التفريغ

> **نصيحة:** يجب دائمًا تعيين خاصية اللغة قبل التفريغ. بدونه، يحاول نموذج Whisper الكشف التلقائي، مما قد ينتج عنه نص مشوش (حرف استبدال مفرد بدلًا من نص).

---

## مقارنة: نماذج الدردشة مقابل Whisper

| الجانب | نماذج الدردشة (الأجزاء 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|----------------------------|------------------|--------------------|
| **نوع المهمة** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **المدخلات** | رسائل نصية (JSON) | ملفات صوتية (WAV/MP3/M4A) | ملفات صوتية (WAV/MP3/M4A) |
| **المخرجات** | نص مولد (مُتدفق) | نص مفروغ (كامل) | نص مفروغ (كامل) |
| **حزمة SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **طريقة API** | `client.chat.completions.create()` | ONNX Runtime مباشر | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **إعداد اللغة** | لا ينطبق | رموز طلب فك التشفير | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **البث** | نعم | لا | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **ميزة الخصوصية** | يبقى الكود/البيانات محليًا | تبقى بيانات الصوت محليًا | تبقى بيانات الصوت محليًا |

---

## النقاط الأساسية

| المفهوم | ما تعلمته |
|---------|------------|
| **Whisper على الجهاز** | تحويل الكلام إلى نص يتم بالكامل محليًا، مثالي لتفريغ مكالمات عملاء Zava ومراجعات المنتجات محليًا |
| **SDK AudioClient** | يقدم SDK لجافاسكريبت و C# `AudioClient` مدمج يدير خط أنابيب التفريغ الكامل في استدعاء واحد |
| **إعداد اللغة** | يجب دائمًا تعيين لغة AudioClient (مثل `"en"`) — بدونها قد ينتج الكشف التلقائي نصًا مشوشًا |
| **Python** | يستخدم `foundry-local-sdk` لإدارة النماذج + `onnxruntime` + `transformers` + `librosa` للاستدلال المباشر مع ONNX |
| **JavaScript** | يستخدم `foundry-local-sdk` مع `model.createAudioClient()` — يعين `settings.language` ثم يستدعي `transcribe()` |
| **C#** | يستخدم `Microsoft.AI.Foundry.Local` مع `model.GetAudioClientAsync()` — يعين `Settings.Language` ثم يستدعي `TranscribeAudioAsync()` |
| **دعم البث** | توفر SDK لكل من JS و C# أيضاً `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` للإخراج على دفعات |
| **محسّن للمعالج المركزي** | النسخة التي تعمل على المعالج (3.05 جيجابايت) تعمل على أي جهاز ويندوز بدون وحدة معالجة رسوميات |
| **خصوصية أولاً** | مثالي للحفاظ على تفاعلات عملاء Zava وبيانات المنتجات السرية على الجهاز |

---

## الموارد

| المورد | الرابط |
|--------|---------|
| وثائق Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| مرجع SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| نموذج OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| موقع Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## الخطوة التالية

تابع إلى [الجزء 10: استخدام نماذج مخصصة أو من Hugging Face](part10-custom-models.md) لتجميع نماذجك الخاصة من Hugging Face وتشغيلها عبر Foundry Local.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**إخلاء المسؤولية**:  
تمت ترجمة هذا المستند باستخدام خدمة الترجمة الآلية [Co-op Translator](https://github.com/Azure/co-op-translator). بينما نسعى للدقة، يُرجى العلم أن الترجمات الآلية قد تحتوي على أخطاء أو عدم دقة. يجب اعتبار المستند الأصلي بلغته الأصلية المصدر المعتمد. للمعلومات الحساسة، يُنصح بالترجمة البشرية المهنية. نحن غير مسؤولين عن أي سوء فهم أو تفسير ناتج عن استخدام هذه الترجمة.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->