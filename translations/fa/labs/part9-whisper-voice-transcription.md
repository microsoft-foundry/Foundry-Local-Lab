![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# بخش ۹: رونویسی صوتی با Whisper و Foundry Local

> **هدف:** استفاده از مدل OpenAI Whisper که به صورت محلی از طریق Foundry Local اجرا می‌شود برای رونویسی فایل‌های صوتی – کاملاً روی دستگاه، بدون نیاز به ابر.

## مرور کلی

Foundry Local تنها برای تولید متن نیست؛ بلکه از مدل‌های **تبدیل گفتار به نوشتار** نیز پشتیبانی می‌کند. در این آزمایشگاه شما از مدل **OpenAI Whisper Medium** برای رونویسی فایل‌های صوتی به‌طور کامل روی دستگاه خود استفاده خواهید کرد. این برای سناریوهایی مانند رونویسی تماس‌های خدمات مشتری Zava، ضبط‌های بازبینی محصول، یا جلسات برنامه‌ریزی کارگاه که داده‌های صوتی نباید هرگز از دستگاه شما خارج شوند، ایده‌آل است.

---

## اهداف یادگیری

تا پایان این آزمایشگاه شما قادر خواهید بود:

- درک مدل تبدیل گفتار به نوشتار Whisper و قابلیت‌های آن
- دانلود و اجرای مدل Whisper با استفاده از Foundry Local
- رونویسی فایل‌های صوتی با استفاده از Foundry Local SDK در پایتون، جاوااسکریپت، و سی‌شارپ
- ساخت یک سرویس ساده رونویسی که کاملاً روی دستگاه اجرا می‌شود
- درک تفاوت‌های بین مدل‌های چت/متنی و مدل‌های صوتی در Foundry Local

---

## پیش‌نیازها

| نیازمندی | جزئیات |
|-------------|---------|
| **Foundry Local CLI** | نسخه **۰.۸.۱۰۱ یا بالاتر** (مدل‌های Whisper از v0.8.101 به بعد در دسترس هستند) |
| **سیستم عامل** | ویندوز ۱۰/۱۱ (x64 یا ARM64) |
| **محیط اجرای زبان** | **پایتون ۳.۹+** و/یا **Node.js 18+** و/یا **.NET 9 SDK** ([دانلود .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **تکمیل شده** | [بخش ۱: شروع کار](part1-getting-started.md)، [بخش ۲: مرور عمیق SDK Foundry Local](part2-foundry-local-sdk.md)، و [بخش ۳: SDK‌ها و API‌ها](part3-sdk-and-apis.md) |

> **توجه:** مدل‌های Whisper باید از طریق **SDK** دانلود شوند (نه CLI). CLI از نقطه انتهایی رونویسی صوتی پشتیبانی نمی‌کند. نسخه خود را با دستور زیر بررسی کنید:
> ```bash
> foundry --version
> ```

---

## مفهوم: نحوه کار Whisper با Foundry Local

مدل OpenAI Whisper یک مدل کلی تبدیل گفتار به نوشتار است که روی مجموعه بزرگی از داده‌های صوتی متنوع آموزش دیده است. هنگام اجرا از طریق Foundry Local:

- مدل **کاملاً روی پردازنده مرکزی (CPU)** اجرا می‌شود – نیازی به GPU نیست
- صوت هرگز از دستگاه شما خارج نمی‌شود – **حفظ کامل حریم خصوصی**
- SDK Foundry Local مدیریت دانلود مدل و مدیریت کش را بر عهده دارد
- **جاوااسکریپت و سی‌شارپ** در SDK یک `AudioClient` داخلی دارند که کل خط لوله رونویسی را بدون نیاز به تنظیم دستی ONNX مدیریت می‌کند
- **پایتون** از SDK برای مدیریت مدل و ONNX Runtime برای استنتاج مستقیم روی مدل‌های ONNX رمزگذار/رمزگشا استفاده می‌کند

### نحوه عملکرد خط لوله (جاوااسکریپت و سی‌شارپ) — AudioClient در SDK

1. **SDK Foundry Local** مدل Whisper را دانلود و در کش ذخیره می‌کند
2. `model.createAudioClient()` (JS) یا `model.GetAudioClientAsync()` (C#) یک `AudioClient` ایجاد می‌کند
3. `audioClient.transcribe(path)` (JS) یا `audioClient.TranscribeAudioAsync(path)` (C#) به صورت داخلی کل خط لوله را انجام می‌دهد — پیش‌پردازش صوت، رمزگذار، رمزگشا، و رمزگشایی توکن
4. `AudioClient` یک خاصیت `settings.language` دارد (بر روی `"en"` برای انگلیسی تنظیم شود) تا رونویسی دقیق را هدایت کند

### نحوه عملکرد خط لوله (پایتون) — ONNX Runtime

1. **SDK Foundry Local** فایل‌های مدل ONNX Whisper را دانلود و کش می‌کند
2. **پیش‌پردازش صوت** تبدیل فایل WAV به اسپکتروگرام مل (۸۰ بازه مل × ۳۰۰۰ فریم)
3. **رمزگذار** اسپکتروگرام مل را پردازش کرده و حالت‌های مخفی به همراه تنسورهای کلید/مقدار توجه متقابل تولید می‌کند
4. **رمزگشا** به‌صورت خودرگرسیو اجرا می‌شود، یک توکن در هر بار تولید می‌کند تا توکن پایان متن تولید شود
5. **توکنایزر** شناسه‌های توکن خروجی را به متن خوانا تبدیل می‌کند

### انواع مدل‌های Whisper

| نام مستعار | شناسه مدل | دستگاه | حجم | توضیحات |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | ۱.۵۳ گیگابایت | شتاب‌داده شده با GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | ۳.۰۵ گیگابایت | بهینه‌شده برای CPU (پیشنهاد شده برای بیشتر دستگاه‌ها) |

> **توجه:** بر خلاف مدل‌های چت که به طور پیش‌فرض لیست می‌شوند، مدل‌های Whisper زیر دسته وظیفه `automatic-speech-recognition` قرار دارند. برای مشاهده جزئیات از دستور `foundry model info whisper-medium` استفاده کنید.

---

## تمرین‌های آزمایشگاه

### تمرین ۰ - دریافت فایل‌های صوتی نمونه

این آزمایشگاه شامل فایل‌های WAV ساخته شده براساس سناریوهای محصول DIY از Zava است. آن‌ها را با اسکریپت موجود ایجاد کنید:

```bash
# از ریشه مخزن - ابتدا یک .venv ایجاد کرده و فعال کنید
python -m venv .venv

# ویندوز (PowerShell):
.venv\Scripts\Activate.ps1
# مک‌اواس:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

این فایل‌ها شش فایل WAV در مسیر `samples/audio/` ایجاد می‌کند:

| فایل | سناریو |
|------|----------|
| `zava-customer-inquiry.wav` | مشتری درباره **دریل بی‌سیم Zava ProGrip** سوال می‌پرسد |
| `zava-product-review.wav` | مشتری بازبینی محصول **رنگ داخلی فوق‌العاده صاف Zava** را انجام می‌دهد |
| `zava-support-call.wav` | تماس پشتیبانی درباره **جعبه ابزار Zava TitanLock** |
| `zava-project-planning.wav` | برنامه‌ریزی یک دک توسط DIYer با **تخته کامپوزیت Zava EcoBoard** |
| `zava-workshop-setup.wav` | معرفی کارگاه با استفاده از **هر پنج محصول Zava** |
| `zava-full-project-walkthrough.wav` | مرور کامل بازسازی گاراژ با استفاده از **تمام محصولات Zava** (~۴ دقیقه برای تست صوت بلند) |

> **نکته:** شما همچنین می‌توانید از فایل‌های WAV/MP3/M4A خود استفاده کنید یا با ویندوز Voice Recorder ضبط کنید.

---

### تمرین ۱ - دانلود مدل Whisper با استفاده از SDK

به دلیل ناسازگاری‌های CLI با مدل‌های Whisper در نسخه‌های جدید Foundry Local، از **SDK** برای دانلود و بارگذاری مدل استفاده کنید. زبان خود را انتخاب کنید:

<details>
<summary><b>🐍 پایتون</b></summary>

**نصب SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# شروع سرویس
manager = FoundryLocalManager()
manager.start_service()

# بررسی اطلاعات فهرست
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# بررسی اینکه آیا قبلاً کش شده است
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# بارگذاری مدل در حافظه
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

به عنوان فایل `download_whisper.py` ذخیره و اجرا کنید:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 جاوااسکریپت</b></summary>

**نصب SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// مدیر را ایجاد کرده و سرویس را شروع کنید
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// گرفتن مدل از فهرست
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

// بارگذاری مدل در حافظه
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

به عنوان فایل `download-whisper.mjs` ذخیره و اجرا کنید:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 سی‌شارپ</b></summary>

**نصب SDK:**
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

> **چرا SDK به جای CLI؟** CLI Foundry Local پشتیبانی از دانلود یا سرو مدل‌های Whisper به‌صورت مستقیم ندارد. SDK روشی قابل اعتماد برای دانلود و مدیریت مدل‌های صوتی به‌صورت برنامه‌نویسی فراهم می‌کند. SDK های جاوااسکریپت و سی‌شارپ شامل `AudioClient` داخلی هستند که کل خط لوله رونویسی را مدیریت می‌کند. پایتون از ONNX Runtime برای استنتاج مستقیم مدل‌های کش شده استفاده می‌کند.

---

### تمرین ۲ - درک SDK Whisper

رونویسی Whisper بسته به زبان به روش‌های متفاوتی انجام می‌شود. **جاوااسکریپت و سی‌شارپ** یک `AudioClient` داخلی در SDK Foundry Local دارند که کل خط لوله (پیش‌پردازش صوت، رمزگذار، رمزگشا، رمزگشایی توکن) را در یک فراخوانی متد انجام می‌دهد. **پایتون** از SDK برای مدیریت مدل و ONNX Runtime برای استنتاج مستقیم مدل‌های ONNX رمزگذار/رمزگشا استفاده می‌کند.

| مؤلفه | پایتون | جاوااسکریپت | سی‌شارپ |
|-----------|--------|------------|----|
| **پکیج‌های SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **مدیریت مدل** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **استخراج ویژگی‌ها** | `WhisperFeatureExtractor` + `librosa` | مدیریت شده توسط `AudioClient` از SDK | مدیریت شده توسط `AudioClient` از SDK |
| **استنتاج** | `ort.InferenceSession` (رمزگذار + رمزگشا) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **رمزگشایی توکن** | `WhisperTokenizer` | مدیریت شده توسط `AudioClient` از SDK | مدیریت شده توسط `AudioClient` از SDK |
| **تنظیم زبان** | تنظیم از طریق `forced_ids` در توکن‌های رمزگشا | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **ورودی** | مسیر فایل WAV | مسیر فایل WAV | مسیر فایل WAV |
| **خروجی** | متن رمزگشایی شده | `result.text` | `result.Text` |

> **مهم:** همیشه زبان را روی `AudioClient` تنظیم کنید (مثلاً `"en"` برای انگلیسی). بدون تنظیم زبان صریح مدل ممکن است خروجی ناخوانا تولید کند چون سعی در شناسایی خودکار زبان دارد.

> **الگوهای SDK:** پایتون با `FoundryLocalManager(alias)` بوت‌استرپ می‌کند، سپس از `get_cache_location()` برای پیدا کردن فایل‌های مدل ONNX استفاده می‌کند. جاوااسکریپت و سی‌شارپ از `AudioClient` داخلی SDK که از طریق `model.createAudioClient()` (JS) یا `model.GetAudioClientAsync()` (C#) به دست می‌آید، استفاده می‌کنند – که کل خط لوله رونویسی را مدیریت می‌کند. جزئیات کامل را در [بخش ۲: مرور عمیق SDK Foundry Local](part2-foundry-local-sdk.md) ببینید.

---

### تمرین ۳ - ساخت یک اپ ساده رونویسی

مسیر زبان خود را انتخاب کرده و یک برنامه حداقلی بسازید که یک فایل صوتی را رونویسی کند.

> **فرمت‌های صوتی پشتیبانی شده:** WAV، MP3، M4A. برای بهترین نتیجه از فایل‌های WAV با نرخ نمونه‌برداری ۱۶ کیلوهرتز استفاده کنید.

<details>
<summary><h3>مسیر پایتون</h3></summary>

#### راه‌اندازی

```bash
cd python
python -m venv venv

# فعال‌سازی محیط مجازی:
# ویندوز (PowerShell):
venv\Scripts\Activate.ps1
# مک‌اواس:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### کد رونویسی

یک فایل به نام `foundry-local-whisper.py` ایجاد کنید:

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

# مرحله ۱: بوت استرپ - شروع سرویس، دانلود و بارگذاری مدل
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# ساخت مسیر به فایل‌های مدل ONNX ذخیره شده در کش
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# مرحله ۲: بارگذاری جلسات ONNX و استخراج‌کننده ویژگی
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

# مرحله ۳: استخراج ویژگی‌های مل‌اسپکتروگرام
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# مرحله ۴: اجرای رمزگذار
enc_out = encoder.run(None, {"audio_features": input_features})
# خروجی اول حالات مخفی است؛ بقیه جفت‌های KV توجه متقابل هستند
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# مرحله ۵: رمزگشایی خودرجعی
initial_tokens = [50258, 50259, 50359, 50363]  # sot، en، رونویسی، بدون نشانگرهای زمانی
input_ids = np.array([initial_tokens], dtype=np.int32)

# پاک کردن کش KV توجه به خود
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

    if next_token == 50257:  # پایان متن
        break
    generated.append(next_token)

    # به‌روزرسانی کش KV توجه به خود
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### اجرا

```bash
# سناریوی یک محصول ژاوا را رونویسی کنید
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# یا موارد دیگر را امتحان کنید:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### نکات کلیدی پایتون

| متد | هدف |
|--------|---------|
| `FoundryLocalManager(alias)` | بوت‌استرپ: شروع سرویس، دانلود و بارگذاری مدل |
| `manager.get_cache_location()` | دریافت مسیر فایل‌های مدل کش شده ONNX |
| `WhisperFeatureExtractor.from_pretrained()` | بارگذاری استخراج‌کننده ویژگی اسپکتروگرام مل |
| `ort.InferenceSession()` | ساخت جلسه‌های ONNX Runtime برای رمزگذار و رمزگشا |
| `tokenizer.decode()` | تبدیل شناسه‌های توکن خروجی به متن |

</details>

<details>
<summary><h3>مسیر جاوااسکریپت</h3></summary>

#### راه‌اندازی

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### کد رونویسی

یک فایل به نام `foundry-local-whisper.mjs` ایجاد کنید:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// مرحله ۱: بوت‌استرپ - ایجاد مدیر، راه‌اندازی سرویس و بارگذاری مدل
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

// مرحله ۲: ایجاد کلاینت صوتی و تبدیل گفتار به متن
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// پاک‌سازی
await model.unload();
```

> **توجه:** SDK Foundry Local یک `AudioClient` داخلی از طریق `model.createAudioClient()` فراهم می‌کند که کل خط لوله استنتاج ONNX را به صورت داخلی مدیریت می‌کند — نیازی به وارد کردن `onnxruntime-node` نیست. همیشه `audioClient.settings.language = "en"` را تنظیم کنید تا رونویسی دقیق به زبان انگلیسی انجام شود.

#### اجرا

```bash
# رونویسی یک سناریوی محصول زاوآ
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# یا سایر موارد را امتحان کنید:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### نکات کلیدی جاوااسکریپت

| متد | هدف |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | ایجاد مدیریت‌کننده سِنگلتون |
| `await catalog.getModel(alias)` | دریافت مدل از کاتالوگ |
| `model.download()` / `model.load()` | دانلود و بارگذاری مدل Whisper |
| `model.createAudioClient()` | ایجاد کلاینت صوتی برای رونویسی |
| `audioClient.settings.language = "en"` | تنظیم زبان رونویسی (برای خروجی دقیق ضروری) |
| `audioClient.transcribe(path)` | رونویسی فایل صوتی، بازگشت `{ text, duration }` |

</details>

<details>
<summary><h3>مسیر سی‌شارپ</h3></summary>

#### راه‌اندازی

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **توجه:** مسیر سی‌شارپ از پکیج `Microsoft.AI.Foundry.Local` استفاده می‌کند که یک `AudioClient` داخلی از طریق `model.GetAudioClientAsync()` فراهم می‌کند. این کل خط لوله رونویسی را درون پردازش مدیریت می‌کند — نیازی به تنظیم جداگانه ONNX Runtime نیست.

#### کد رونویسی

محتویات `Program.cs` را جایگزین کنید:

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

#### اجرا

```bash
# ضبط یک سناریوی محصول Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# یا دیگران را امتحان کنید:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### نکات کلیدی سی‌شارپ

| متد | هدف |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | مقداردهی اولیه Foundry Local با تنظیمات پیکربندی |
| `catalog.GetModelAsync(alias)` | دریافت مدل از کاتالوگ |
| `model.DownloadAsync()` | دانلود مدل Whisper |
| `model.GetAudioClientAsync()` | دریافت AudioClient (نه ChatClient!) |
| `audioClient.Settings.Language = "en"` | تنظیم زبان رونویسی (برای خروجی دقیق ضروری) |
| `audioClient.TranscribeAudioAsync(path)` | رونویسی فایل صوتی |
| `result.Text` | متن رونویسی شده |


> **C# در مقابل Python/JS:** SDK زبان C# یک `AudioClient` داخلی برای رونویسی درون پردازشی از طریق `model.GetAudioClientAsync()` ارائه می‌دهد، مشابه SDK جاوااسکریپت. پایتون به طور مستقیم از ONNX Runtime برای استنتاج مدل‌های رمزگذار/رمزگشا در حافظه پنهان استفاده می‌کند.

</details>

---

### تمرین 4 - رونویسی دسته‌ای از همه نمونه‌های زاوآ

حالا که اپلیکیشن رونویسی شما به درستی کار می‌کند، همه پنج فایل نمونه زاوآ را رونویسی کرده و نتایج را مقایسه کنید.

<details>
<summary><h3>ردیف پایتون</h3></summary>

نمونه کامل `python/foundry-local-whisper.py` از قبل از رونویسی دسته‌ای پشتیبانی می‌کند. وقتی بدون آرگومان اجرا شود، همه فایل‌های `zava-*.wav` در `samples/audio/` را رونویسی می‌کند:

```bash
cd python
python foundry-local-whisper.py
```

نمونه از `FoundryLocalManager(alias)` برای بوت‌استرپ استفاده می‌کند، سپس جلسات ONNX رمزگذار و رمزگشا را برای هر فایل اجرا می‌کند.

</details>

<details>
<summary><h3>ردیف جاوااسکریپت</h3></summary>

نمونه کامل `javascript/foundry-local-whisper.mjs` از قبل از رونویسی دسته‌ای پشتیبانی می‌کند. وقتی بدون آرگومان اجرا شود، همه فایل‌های `zava-*.wav` در `samples/audio/` را رونویسی می‌کند:

```bash
cd javascript
node foundry-local-whisper.mjs
```

نمونه از `FoundryLocalManager.create()` و `catalog.getModel(alias)` برای راه‌اندازی SDK استفاده می‌کند، سپس از `AudioClient` (با `settings.language = "en"`) برای رونویسی هر فایل بهره می‌برد.

</details>

<details>
<summary><h3>ردیف C#</h3></summary>

نمونه کامل `csharp/WhisperTranscription.cs` از قبل از رونویسی دسته‌ای پشتیبانی می‌کند. وقتی بدون آرگومان فایل خاص اجرا شود، همه فایل‌های `zava-*.wav` در `samples/audio/` را رونویسی می‌کند:

```bash
cd csharp
dotnet run whisper
```

نمونه از `FoundryLocalManager.CreateAsync()` و `AudioClient` SDK (با `Settings.Language = "en"`) برای رونویسی درون پردازشی استفاده می‌کند.

</details>

**چه چیزی را بررسی کنیم:** خروجی رونویسی را با متن اصلی در `samples/audio/generate_samples.py` مقایسه کنید. مدل Whisper چقدر دقیق نام‌های محصول مانند "Zava ProGrip" و اصطلاحات فنی مانند "brushless motor" یا "composite decking" را بازمی‌آورد؟

---

### تمرین 5 - درک الگوهای اصلی کد

مطالعه کنید که رونویسی Whisper چگونه با تکمیل چت در هر سه زبان متفاوت است:

<details>
<summary><b>پایتون - تفاوت‌های کلیدی با چت</b></summary>

```python
# تکمیل چت (بخش‌های ۲-۶):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# رونویسی صوتی (این بخش):
# به جای استفاده از کلاینت OpenAI، مستقیماً از ONNX Runtime استفاده می‌کند
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... حلقه دیکدر خودرگرسی ...
print(tokenizer.decode(generated_tokens))
```

**بینش کلیدی:** مدل‌های چت از API سازگار با OpenAI از طریق `manager.endpoint` استفاده می‌کنند. Whisper برای یافتن فایل‌های مدل ONNX کش شده از SDK بهره می‌برد و سپس مستقیماً با ONNX Runtime استنتاج را اجرا می‌کند.

</details>

<details>
<summary><b>جاوااسکریپت - تفاوت‌های کلیدی با چت</b></summary>

```javascript
// تکمیل گفتگو (بخش‌های ۲-۶):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// رونویسی صوتی (این بخش):
// استفاده از AudioClient داخلی SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // همیشه برای بهترین نتایج زبان را تنظیم کنید
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**بینش کلیدی:** مدل‌های چت از API سازگار با OpenAI از طریق `manager.urls[0] + "/v1"` استفاده می‌کنند. رونویسی Whisper از `AudioClient` SDK استفاده می‌کند که از `model.createAudioClient()` گرفته شده است. برای جلوگیری از خروجی ناهماهنگ ناشی از تشخیص خودکار، `settings.language` را تنظیم کنید.

</details>

<details>
<summary><b>C# - تفاوت‌های کلیدی با چت</b></summary>

روش C# از `AudioClient` داخلی SDK برای رونویسی درون پردازشی استفاده می‌کند:

**ابتدایی سازی مدل:**

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

**رونویسی:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**بینش کلیدی:** C# از `FoundryLocalManager.CreateAsync()` استفاده کرده و به طور مستقیم یک `AudioClient` می‌گیرد — نیازی به راه‌اندازی ONNX Runtime نیست. برای جلوگیری از خروجی ناهماهنگ تشخیص خودکار، `Settings.Language` را تنظیم کنید.

</details>

> **خلاصه:** پایتون از Foundry Local SDK برای مدیریت مدل و ONNX Runtime برای استنتاج مستقیم روی مدل‌های رمزگذار/رمزگشا استفاده می‌کند. جاوااسکریپت و C# هر دو از `AudioClient` داخلی SDK برای رونویسی سریع بهره می‌برند — کلاینت را بسازید، زبان را تنظیم کنید و `transcribe()` / `TranscribeAudioAsync()` را فراخوانی کنید. همیشه پیش از رونویسی، ویژگی زبان را روی AudioClient تنظیم کنید تا نتایج دقیق باشند.

---

### تمرین 6 - آزمایش

این تغییرات را امتحان کنید تا درک خود را عمیق‌تر کنید:

1. **فایل‌های صوتی مختلف را امتحان کنید** - خودتان را با ضبط صدا در Windows Voice Recorder ضبط کنید، به صورت WAV ذخیره کنید و رونویسی کنید

2. **مقایسه گونه‌های مدل** - اگر GPU انویدیا دارید، نسخه CUDA را امتحان کنید:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   سرعت رونویسی را با نسخه CPU مقایسه کنید.

3. **افزودن قالب‌بندی خروجی** - پاسخ JSON می‌تواند شامل موارد زیر باشد:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **ساخت API REST** - کد رونویسی خود را در یک وب‌سرور بپیچید:

   | زبان | فریمورک | مثال |
   |----------|-----------|--------|
   | پایتون | FastAPI | `@app.post("/v1/audio/transcriptions")` با `UploadFile` |
   | جاوااسکریپت | Express.js | `app.post("/v1/audio/transcriptions")` با `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` با `IFormFile` |

5. **چند چرخه با رونویسی** - Whisper را با یک عامل چت از بخش 4 ترکیب کنید: ابتدا صدا را رونویسی کنید، سپس متن را برای تحلیل یا خلاصه‌سازی به عامل بفرستید.

---

## مرجع API صوتی SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — نمونه‌ای از `AudioClient` می‌سازد
> - `audioClient.settings.language` — زبان رونویسی را تنظیم کنید (مثلاً `"en"`)
> - `audioClient.settings.temperature` — کنترل تصادفی بودن (اختیاری)
> - `audioClient.transcribe(filePath)` — یک فایل را رونویسی می‌کند، خروجی `{ text, duration }` می‌دهد
> - `audioClient.transcribeStreaming(filePath, callback)` — رونویسی جریان داده‌ها به صورت تکه‌ای از طریق کال‌بک
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — نمونه‌ای از `OpenAIAudioClient` می‌سازد
> - `audioClient.Settings.Language` — زبان رونویسی را تنظیم کنید (مثلاً `"en"`)
> - `audioClient.Settings.Temperature` — کنترل تصادفی بودن (اختیاری)
> - `await audioClient.TranscribeAudioAsync(filePath)` — یک فایل را رونویسی می‌کند، خروجی شیئی با `.Text` می‌دهد
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — `IAsyncEnumerable` تکه‌های رونویسی را بازمی‌گرداند

> **نکته:** همیشه پیش از رونویسی، مقدار زبان را تنظیم کنید. بدون آن، مدل Whisper سعی در تشخیص خودکار دارد که ممکن است خروجی ناواضح (یک کاراکتر جایگزین منفرد به جای متن) تولید کند.

---

## مقایسه: مدل‌های چت در مقابل Whisper

| جنبه | مدل‌های چت (بخش‌های 3-7) | Whisper - پایتون | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **نوع کار** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **ورودی** | پیام‌های متنی (JSON) | فایل‌های صوتی (WAV/MP3/M4A) | فایل‌های صوتی (WAV/MP3/M4A) |
| **خروجی** | متن تولید شده (جریان داده) | متن رونویسی شده (کامل) | متن رونویسی شده (کامل) |
| **بسته SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **روش API** | `client.chat.completions.create()` | ONNX Runtime مستقیم | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **تنظیم زبان** | ندارد | توکن‌های پرامپت رمزگشا | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **پشتیبانی جریان** | بله | خیر | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **مزیت حریم خصوصی** | کد/داده محلی می‌ماند | داده صوتی محلی می‌ماند | داده صوتی محلی می‌ماند |

---

## نکات کلیدی

| مفهوم | آنچه یاد گرفتید |
|---------|-----------------|
| **Whisper روی دستگاه** | گفتار به متن کاملاً به صورت محلی اجرا می‌شود، ایده‌آل برای رونویسی تماس‌های مشتری زاوآ و بازخوردهای محصول به صورت محلی |
| **SDK AudioClient** | SDKهای جاوااسکریپت و C# یک `AudioClient` داخلی ارائه می‌دهند که تمام روند رونویسی را در یک فراخوانی مدیریت می‌کند |
| **تنظیم زبان** | همیشه زبان را در AudioClient تنظیم کنید (مثلاً `"en"`) — بدون آن، تشخیص خودکار ممکن است خروجی ناواضح ایجاد کند |
| **پایتون** | از `foundry-local-sdk` برای مدیریت مدل + `onnxruntime` + `transformers` + `librosa` برای استنتاج مستقیم ONNX استفاده می‌کند |
| **جاوااسکریپت** | از `foundry-local-sdk` با `model.createAudioClient()` استفاده می‌کند — `settings.language` را تنظیم کرده و سپس `transcribe()` را صدا می‌زند |
| **C#** | از `Microsoft.AI.Foundry.Local` با `model.GetAudioClientAsync()` استفاده می‌کند — `Settings.Language` را تنظیم کرده و سپس `TranscribeAudioAsync()` را صدا می‌زند |
| **پشتیبانی جریان** | SDKهای JS و C# همچنین `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` برای خروجی تکه‌ای ارائه می‌دهند |
| **بهینه‌شده برای CPU** | نسخه CPU (3.05 گیگابایت) روی هر دستگاه ویندوز بدون GPU کار می‌کند |
| **حریم خصوصی اول** | عالی برای حفظ تعاملات مشتری زاوآ و داده‌های محصول اختصاصی روی دستگاه |

---

## منابع

| منبع | لینک |
|----------|------|
| مستندات Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| مرجع SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| مدل Whisper اوپن‌ای‌آی | [github.com/openai/whisper](https://github.com/openai/whisper) |
| وب‌سایت Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## گام بعدی

به [بخش 10: استفاده از مدل‌های سفارشی یا Hugging Face](part10-custom-models.md) ادامه دهید تا مدل‌های خود را از Hugging Face کامپایل کرده و از طریق Foundry Local اجرا کنید.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**تذکر**:  
این سند با استفاده از سرویس ترجمه هوش مصنوعی [Co-op Translator](https://github.com/Azure/co-op-translator) ترجمه شده است. اگرچه ما به دقت تلاش می‌کنیم، لطفاً توجه داشته باشید که ترجمه‌های خودکار ممکن است حاوی خطاها یا نادرستی‌هایی باشند. سند اصلی به زبان بومی خود باید به عنوان منبع معتبر در نظر گرفته شود. برای اطلاعات حساس، ترجمه حرفه‌ای انسانی توصیه می‌شود. ما مسئول هیچ سوءتفاهم یا برداشت نادرستی که از استفاده این ترجمه ناشی شود، نیستیم.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->