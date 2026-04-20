![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# অংশ ৯: Whisper এবং Foundry Local দিয়ে ভয়েস ট্রান্সক্রিপশন

> **লক্ষ্য:** Foundry Local এর মাধ্যমে লোকালি চালিত OpenAI Whisper মডেল ব্যবহার করে অডিও ফাইল ট্রান্সক্রাইব করা - সম্পূর্ণ ডিভাইসে, কোনো ক্লাউডের প্রয়োজন নেই।

## ওভারভিউ

Foundry Local শুধু টেক্সট জেনারেশনের জন্য নয়; এটি **স্পিচ-টু-টেক্সট** মডেলগুলোকেও সমর্থন করে। এই ল্যাবে আপনি **OpenAI Whisper Medium** মডেল ব্যবহার করে আপনার মেশিনে সম্পূর্ণ অডিও ফাইল ট্রান্সক্রাইব করবেন। এটি আদর্শ যেমন Zava কাস্টমার সার্ভিস কল, প্রোডাক্ট রিভিউ রেকর্ডিং, অথবা ওয়ার্কশপ প্ল্যানিং সেশন যেখানে অডিও ডেটা কখনোই আপনার ডিভাইস ছেড়ে যাবে না।

---

## শেখার লক্ষ্যসমূহ

এই ল্যাব শেষে আপনি সক্ষম হবেন:

- Whisper স্পিচ-টু-টেক্সট মডেল এবং এর সক্ষমতাগুলো বোঝতে
- Foundry Local ব্যবহার করে Whisper মডেল ডাউনলোড এবং চালাতে
- Foundry Local SDK দিয়ে Python, JavaScript, এবং C# এ অডিও ফাইল ট্রান্সক্রাইব করতে
- সম্পূর্ণ ডিভাইসে চলে এমন একটি সহজ ট্রান্সক্রিপশন সার্ভিস তৈরি করতে
- Foundry Local এর চ্যাট/টেক্সট মডেল এবং অডিও মডেলগুলোর মধ্যে পার্থক্য বুঝতে

---

## পূর্বশর্তসমূহ

| প্রয়োজনীয়তা | বিবরণ |
|-------------|---------|
| **Foundry Local CLI** | ভার্সন **0.8.101 বা তার উপরে** (Whisper মডেলগুলি v0.8.101 থেকে উপলব্ধ) |
| **অপারেটিং সিস্টেম** | Windows 10/11 (x64 বা ARM64) |
| **ভাষা রানটাইম** | **Python 3.9+** এবং/অথবা **Node.js 18+** এবং/অথবা **.NET 9 SDK** ([Download .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **সম্পন্ন হয়েছে** | [Part 1: Getting Started](part1-getting-started.md), [Part 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md), এবং [Part 3: SDKs and APIs](part3-sdk-and-apis.md) |

> **নোট:** Whisper মডেলগুলি **SDK** এর মাধ্যমে ডাউনলোড করতে হবে (CLI নয়)। CLI অডিও ট্রান্সক্রিপশন এন্ডপয়েন্ট সমর্থন করে না। আপনার ভার্সন চেক করুন:
> ```bash
> foundry --version
> ```

---

## ধারণা: কিভাবে Whisper Foundry Local এর সাথে কাজ করে

OpenAI Whisper মডেল একটি সাধারণ-উদ্দেশ্য স্পিচ রেকগনিশন মডেল যা বৈচিত্র্যময় অডিও ডেটাসেটে প্রশিক্ষিত। Foundry Local এর মাধ্যমে চলার সময়:

- মডেলটি **সম্পূর্ণ আপনার CPU** তেই চলে - কোনো GPU দরকার নেই
- অডিও কখনোই আপনার ডিভাইস ছেড়ে যায় না - **সম্পূর্ণ ব্যক্তিগততা**
- Foundry Local SDK মডেল ডাউনলোড এবং ক্যাশ ব্যবস্থাপনা করে
- **JavaScript এবং C#** SDK তে বিল্ট-ইন আছে `AudioClient` যা সম্পূর্ণ ট্রান্সক্রিপশন পাইপলাইন হ্যান্ডল করে — কোনো ম্যানুয়াল ONNX সেটআপ দরকার নেই
- **Python** SDK ব্যবহার করে মডেল ব্যবস্থাপনা এবং ONNX Runtime দিয়ে encoder/decoder ONNX মডেলের বিরুদ্ধে সরাসরি ইনফারেন্স করে

### পাইপলাইন কিভাবে কাজ করে (JavaScript এবং C#) — SDK AudioClient

১. **Foundry Local SDK** Whisper মডেল ডাউনলোড এবং ক্যাশ করে  
২. `model.createAudioClient()` (JS) অথবা `model.GetAudioClientAsync()` (C#) একটি `AudioClient` তৈরি করে  
৩. `audioClient.transcribe(path)` (JS) অথবা `audioClient.TranscribeAudioAsync(path)` (C#) সম্পূর্ণ পাইপলাইন অন্তর্ভুক্ত করে — অডিও প্রি-প্রসেসিং, এনকোডার, ডিকোডার, এবং টোকেন ডিকোডিং  
৪. `AudioClient` একটি `settings.language` প্রপার্টি প্রকাশ করে (ইংরেজির জন্য `"en"` সেট করা হয়) যা সঠিক ট্রান্সক্রিপশন পরিচালনা করে  

### পাইপলাইন কিভাবে কাজ করে (Python) — ONNX Runtime

১. **Foundry Local SDK** Whisper ONNX মডেল ফাইল ডাউনলোড এবং ক্যাশ করে  
২. **অডিও প্রি-প্রসেসিং** WAV অডিওকে একটি মেল স্পেকট্রোগ্রামে রূপান্তর করে (80 মেল বিন x 3000 ফ্রেম)  
৩. **এনকোডার** মেল স্পেকট্রোগ্রাম প্রক্রিয়াকরণ করে এবং হিডেন স্টেটস ও ক্রস-অ্যাটেনশন কি/ভ্যালু টেনসর তৈরি করে  
৪. **ডিকোডার** স্বয়ংক্রিয়ভাবে রান করে, টোকেন একটি একটি করে তৈরি করে যতক্ষণ না শেষ টেক্সট টোকেন আসে  
৫. **টোকেনাইজার** আউটপুট টোকেন আইডি গুলো ফেরাউন্ডে পড়ার মতো টেক্সটে রূপান্তর করে  

### Whisper মডেল ভ্যারিয়ান্টস

| উপনাম | মডেল আইডি | ডিভাইস | সাইজ | বর্ণনা |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-ত্বরান্বিত (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU-অপ্টিমাইজড (অধিকাংশ ডিভাইসের জন্য সুপারিশকৃত) |

> **নোট:** ডিফল্টরূপে যেভাবে চ্যাট মডেলগুলো তালিকাভুক্ত হয় না, Whisper মডেলগুলো `automatic-speech-recognition` টাস্কের অন্তর্ভুক্ত। বিস্তারিত দেখতে `foundry model info whisper-medium` ব্যবহার করুন।

---

## ল্যাব অনুশীলনসমূহ

### অনুশীলন ০ - স্যাম্পল অডিও ফাইল সংগ্রহ করুন

এই ল্যাবে Zava DIY প্রোডাক্টের বিভিন্ন সিচুয়েশনের উপর তৈরি প্রি-বিল্ট WAV ফাইল অন্তর্ভুক্ত আছে। সেগুলো নিচের স্ক্রিপ্ট দিয়ে তৈরি করুন:

```bash
# রিপো রুট থেকে - প্রথমে একটি .venv তৈরি করুন এবং সক্রিয় করুন
python -m venv .venv

# উইন্ডোজ (পাওয়ারশেল):
.venv\Scripts\Activate.ps1
# ম্যাকওএস:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

এটি `samples/audio/` এ ছয়টি WAV ফাইল তৈরি করে:

| ফাইল | সিচুয়েশন |
|------|----------|
| `zava-customer-inquiry.wav` | কাস্টমার **Zava ProGrip Cordless Drill** নিয়ে প্রশ্ন করছে |
| `zava-product-review.wav` | কাস্টমার **Zava UltraSmooth Interior Paint** এর রিভিউ দিচ্ছে |
| `zava-support-call.wav` | **Zava TitanLock Tool Chest** এর সাথে সম্পর্কিত সাপোর্ট কল |
| `zava-project-planning.wav` | DIY ব্যবহারকারী **Zava EcoBoard Composite Decking** সহ ডেক পরিকল্পনা করছে |
| `zava-workshop-setup.wav` | **সব পাঁচটি Zava পণ্য** ব্যবহার করে ওয়ার্কশপ ওভারভিউ |
| `zava-full-project-walkthrough.wav` | **সব Zava পণ্য** দিয়ে বিস্তৃত গ্যারাজ রেনোভেশন ওভারভিউ (~৪ মিনিট, লম্বা অডিও টেস্টিং এর জন্য) |

> **সুত্র:** আপনি আপনার নিজস্ব WAV/MP3/M4A ফাইলও ব্যবহার করতে পারেন, অথবা Windows Voice Recorder দিয়ে রেকর্ড করতে পারেন।

---

### অনুশীলন ১ - SDK দিয়ে Whisper মডেল ডাউনলোড করুন

নতুন Foundry Local ভার্সনে CLI হিসেবে Whisper মডেল ডাউনলোড সমর্থিত না হওয়ায়, মডেল ডাউনলোড ও লোডের জন্য **SDK** ব্যবহার করুন। আপনার পছন্দের ভাষা নির্বাচন করুন:

<details>
<summary><b>🐍 Python</b></summary>

**SDK ইনস্টল করুন:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# সার্ভিস শুরু করুন
manager = FoundryLocalManager()
manager.start_service()

# ক্যাটালগ তথ্য চেক করুন
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# এটি ইতিমধ্যে ক্যাশে আছে কি না চেক করুন
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# মডেলটি মেমরিতে লোড করুন
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py` নামে সেভ করে রান করুন:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK ইনস্টল করুন:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// ম্যানেজার তৈরি করুন এবং সার্ভিস শুরু করুন
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ক্যাটালগ থেকে মডেল পাওয়া
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

// মেমরিতে মডেল লোড করুন
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs` নামে সেভ করে রান করুন:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK ইনস্টল করুন:**
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

> **কেন SDK ব্যবহার করবেন CLI এর পরিবর্তে?** Foundry Local CLI সরাসরি Whisper মডেল ডাউনলোড বা সার্ভ করার সমর্থন দেয় না। SDK একটি নির্ভরযোগ্য পন্থা প্রদান করে অডিও মডেল প্রোগ্রাম্যাটিকালি ডাউনলোড এবং ব্যবস্থাপনার জন্য। JavaScript এবং C# SDK তে বদ্ধভাবে বিল্ট-ইন `AudioClient` রয়েছে যা পুরো ট্রান্সক্রিপশন পাইপলাইন পরিচালনা করে। Python সরাসরি ক্যাশ করা মডেল ফাইলগুলোর বিরুদ্ধে ONNX Runtime ব্যবহার করে ইনফারেন্স করে।

---

### অনুশীলন ২ - Whisper SDK বোঝাপড়া

Whisper ট্রান্সক্রিপশন বিভিন্ন ভাষায় বিভিন্ন পদ্ধতি ব্যবহার করে। **JavaScript এবং C#** Foundry Local SDK তে বিল্ট-ইন `AudioClient` দেয় যা সম্পূর্ণ পাইপলাইন (অডিও প্রি-প্রসেসিং, এনকোডার, ডিকোডার, টোকেন ডিকোডিং) একক মেথড কলেই হ্যান্ডেল করে। **Python** Foundry Local SDK ব্যবহার করে মডেল ব্যবস্থাপনা এবং ONNX Runtime ব্যবহার করে encoder/decoder ONNX মডেলের বিরুদ্ধে সরাসরি ইনফারেন্স করে।

| উপাদান | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK প্যাকেজ** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **মডেল ব্যবস্থাপনা** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **ফিচার এক্সট্রাকশন** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` পরিচালনা করে | SDK `AudioClient` পরিচালনা করে |
| **ইনফারেন্স** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **টোকেন ডিকোডিং** | `WhisperTokenizer` | SDK `AudioClient` পরিচালনা করে | SDK `AudioClient` পরিচালনা করে |
| **ভাষা সেটিং** | ডিকোডার টোকেনের `forced_ids` দ্বারা সেট | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **ইনপুট** | WAV ফাইল পাথ | WAV ফাইল পাথ | WAV ফাইল পাথ |
| **আউটপুট** | ডিকোডেড টেক্সট স্ট্রিং | `result.text` | `result.Text` |

> **জরুরি:** সর্বদা `AudioClient` এ ভাষা সেট করুন (যেমন ইংরেজির জন্য `"en"`). ভাষা স্পষ্ট না হলে মডেল ভুল আউটপুট তৈরি করতে পারে কারণ এটি স্বয়ংক্রিয় ভাষা সনাক্তকরণ চেষ্টা করে।

> **SDK প্যাটার্ন:** Python `FoundryLocalManager(alias)` দিয়ে বুটস্ট্র্যাপ করে, তারপর `get_cache_location()` দিয়ে ONNX মডেল ফাইলগুলি খুঁজে পায়। JavaScript এবং C# SDK এর বিল্ট-ইন `AudioClient` ব্যবহার করে — যা `model.createAudioClient()` (JS) অথবা `model.GetAudioClientAsync()` (C#) দিয়ে পাওয়া যায় — যা সম্পূর্ণ ট্রান্সক্রিপশন পাইপলাইন পরিচালনা করে। বিস্তারিত জানতে দেখুন [Part 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md)।

---

### অনুশীলন ৩ - একটি সহজ ট্রান্সক্রিপশন অ্যাপ তৈরি করুন

আপনার ভাষা নির্বাচন করুন এবং একটি ন্যূনতম অ্যাপ তৈরি করুন যা একটি অডিও ফাইল ট্রান্সক্রাইব করে।

> **সমর্থিত অডিও ফরম্যাট:** WAV, MP3, M4A। সর্বোত্তম ফলাফলের জন্য 16kHz স্যাম্পল রেটের WAV ফাইল ব্যবহার করুন।

<details>
<summary><h3>Python ট্র্যাক</h3></summary>

#### সেটআপ

```bash
cd python
python -m venv venv

# ভার্চুয়াল পরিবেশ সক্রিয় করুন:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### ট্রান্সক্রিপশন কোড

`foundry-local-whisper.py` নামে একটি ফাইল তৈরি করুন:

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

# ধাপ ১: বুটস্ট্র্যাপ - সার্ভিস শুরু করে, ডাউনলোড করে, এবং মডেল লোড করে
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# ক্যাশ করা ONNX মডেল ফাইলগুলির পাথ তৈরি করুন
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# ধাপ ২: ONNX সেশন এবং ফিচার এক্সট্রাক্টর লোড করুন
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

# ধাপ ৩: মেল স্পেকট্রোগ্রাম ফিচার এক্সট্রাক্ট করুন
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# ধাপ ৪: এঙ্কোডার চালান
enc_out = encoder.run(None, {"audio_features": input_features})
# প্রথম আউটপুট হলো হিডেন স্টেট; বাকি হলো ক্রস-অ্যাটেনশন KV পেয়ার
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# ধাপ ৫: অটোরিগ্রেসিভ ডিকোডিং
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, ট্রান্সক্রাইব, নটটাইমস্ট্যাম্পস
input_ids = np.array([initial_tokens], dtype=np.int32)

# খালি সেল্ফ-অ্যাটেনশন KV ক্যাশ
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

    if next_token == 50257:  # টেক্সটের শেষ
        break
    generated.append(next_token)

    # সেল্ফ-অ্যাটেনশন KV ক্যাশ আপডেট করুন
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### রান করুন

```bash
# একটি জাভা পণ্য পরিস্থিতি প্রতিলিপি করুন
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# অথবা অন্য কিছু চেষ্টা করুন:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### গুরুত্বপূর্ণ Python পয়েন্টসমূহ

| মেথড | উদ্দেশ্য |
|--------|---------|
| `FoundryLocalManager(alias)` | বুটস্ট্র্যাপ: সার্ভিস শুরু, মডেল ডাউনলোড এবং লোড |
| `manager.get_cache_location()` | ক্যাশ করা ONNX মডেল ফাইল পথ পেতে |
| `WhisperFeatureExtractor.from_pretrained()` | মেল স্পেকট্রোগ্রাম ফিচার এক্সট্রাক্টর লোড করার জন্য |
| `ort.InferenceSession()` | এনকোডার এবং ডিকোডারের জন্য ONNX Runtime সেশন তৈরি |
| `tokenizer.decode()` | আউটপুট টোকেন আইডিগুলো টেক্সটে রূপান্তর করার জন্য |

</details>

<details>
<summary><h3>JavaScript ট্র্যাক</h3></summary>

#### সেটআপ

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### ট্রান্সক্রিপশন কোড

`foundry-local-whisper.mjs` নামে একটি ফাইল তৈরি করুন:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// ধাপ ১: বুটস্ট্র্যাপ - ম্যানেজার তৈরি করুন, সার্ভিস শুরু করুন, এবং মডেল লোড করুন
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

// ধাপ ২: একটি অডিও ক্লায়েন্ট তৈরি করুন এবং ট্রান্সক্রাইব করুন
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// পরিস্কার করা
await model.unload();
```

> **নোট:** Foundry Local SDK মডেলে অন্তর্নির্মিত একটি `AudioClient` রয়েছে যা `model.createAudioClient()` এর মাধ্যমে পাওয়া যায় এবং সম্পূর্ণ ONNX ইনফারেন্স পাইপলাইন পরিচালনা করে — `onnxruntime-node` ইম্পোর্টের কোনো দরকার নেই। সর্বদা `audioClient.settings.language = "en"` সেট করুন যাতে সঠিক ইংরেজি ট্রান্সক্রিপশন নিশ্চিত হয়।

#### রান করুন

```bash
# একটি জাভা পণ্য দৃশ্যপট ট্রান্সক্রাইব করুন
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# অথবা অন্যগুলি চেষ্টা করুন:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### গুরুত্বপূর্ণ JavaScript পয়েন্টসমূহ

| মেথড | উদ্দেশ্য |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | ম্যানেজার সিঙ্গেলটন তৈরি করা |
| `await catalog.getModel(alias)` | ক্যাটালগ থেকে মডেল নেয়া |
| `model.download()` / `model.load()` | Whisper মডেল ডাউনলোড এবং লোড করা |
| `model.createAudioClient()` | ট্রান্সক্রিপশনের জন্য অডিও ক্লায়েন্ট তৈরি করা |
| `audioClient.settings.language = "en"` | ট্রান্সক্রিপশন ভাষা সেট করা (সঠিক আউটপুটের জন্য প্রয়োজন) |
| `audioClient.transcribe(path)` | একটি অডিও ফাইল ট্রান্সক্রাইব করা, রিটার্ন `{ text, duration }` |

</details>

<details>
<summary><h3>C# ট্র্যাক</h3></summary>

#### সেটআপ

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **নোট:** C# ট্র্যাকটি `Microsoft.AI.Foundry.Local` প্যাকেজ ব্যবহার করে যা `model.GetAudioClientAsync()` দিয়ে বিল্ট-ইন `AudioClient` সরবরাহ করে। এটি সম্পূর্ণ ট্রান্সক্রিপশন পাইপলাইন ইন-প্রসেসে হ্যান্ডেল করে — আলাদা ONNX Runtime সেটআপের দরকার নেই।

#### ট্রান্সক্রিপশন কোড

`Program.cs` এর বিষয়বস্তু পরিবর্তন করুন:

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

#### রান করুন

```bash
# একটি Zava পণ্য পরিস্থিতি ট্রান্সক্রাইব করুন
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# অথবা অন্যান্য চেষ্টা করুন:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### গুরুত্বপূর্ণ C# পয়েন্টসমূহ

| মেথড | উদ্দেশ্য |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Local কনফিগারেশন দিয়ে ইনিশিয়ালাইজ করা |
| `catalog.GetModelAsync(alias)` | ক্যাটালগ থেকে মডেল আনা |
| `model.DownloadAsync()` | Whisper মডেল ডাউনলোড করা |
| `model.GetAudioClientAsync()` | AudioClient পাওয়া (ChatClient নয়!) |
| `audioClient.Settings.Language = "en"` | ট্রান্সক্রিপশন ভাষা সেট করা (সঠিক আউটপুটের জন্য প্রয়োজন) |
| `audioClient.TranscribeAudioAsync(path)` | অডিও ফাইল ট্রান্সক্রাইব করা |
| `result.Text` | ট্রান্সক্রাইব করা টেক্সট |
> **C# বনাম Python/JS:** C# SDK একটি বিল্ট-ইন `AudioClient` প্রদান করে যা `model.GetAudioClientAsync()` এর মাধ্যমে ইন-প্রসেস ট্রান্সক্রিপশন উপলব্ধ করে, যা জাভাস্ক্রিপ্ট SDK-এর মতো। Python সরাসরি ONNX Runtime ব্যবহার করে ক্যাশ্ড এনকোডার/ডিকোডার মডেলগুলোর উপর ইনফারেন্স করার জন্য।

</details>

---

### Exercise 4 - সব Zava স্যাম্পল ব্যাচ ট্রান্সক্রাইব করুন

এখন যেহেতু আপনার একটি কার্যকর ট্রান্সক্রিপশন অ্যাপ আছে, সব পাঁচটি Zava স্যাম্পল ফাইল ট্রান্সক্রাইব করুন এবং ফলাফলগুলো তুলনা করুন।

<details>
<summary><h3>Python Track</h3></summary>

সম্পূর্ণ স্যাম্পল `python/foundry-local-whisper.py` ইতিমধ্যেই ব্যাচ ট্রান্সক্রিপশন সমর্থন করে। যখন এটি আর্গুমেন্ট ছাড়া চালানো হয়, তখন এটি `samples/audio/` এর সব `zava-*.wav` ফাইল ট্রান্সক্রাইব করে:

```bash
cd python
python foundry-local-whisper.py
```

স্যাম্পলটি `FoundryLocalManager(alias)` ব্যবহার করে বুটস্ট্র্যাপে শুরু করে, তারপর প্রতিটি ফাইলের জন্য এনকোডার এবং ডিকোডার ONNX সেশন চালায়।

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

সম্পূর্ণ স্যাম্পল `javascript/foundry-local-whisper.mjs` ইতিমধ্যেই ব্যাচ ট্রান্সক্রিপশন সমর্থন করে। যখন এটি আর্গুমেন্ট ছাড়া চালানো হয়, তখন এটি `samples/audio/` এর সব `zava-*.wav` ফাইল ট্রান্সক্রাইব করে:

```bash
cd javascript
node foundry-local-whisper.mjs
```

স্যাম্পলটি `FoundryLocalManager.create()` এবং `catalog.getModel(alias)` ব্যবহার করে SDK ইনিশিয়ালাইজ করে, তারপর প্রতিটি ফাইল ট্রান্সক্রাইব করার জন্য `AudioClient` (সেটিংসে `language = "en"`) ব্যবহার করে।

</details>

<details>
<summary><h3>C# Track</h3></summary>

সম্পূর্ণ স্যাম্পল `csharp/WhisperTranscription.cs` ইতিমধ্যেই ব্যাচ ট্রান্সক্রিপশন সমর্থন করে। যখন এটা নির্দিষ্ট কোনো ফাইল আর্গুমেন্ট ছাড়া চালানো হয়, তখন এটি `samples/audio/` এর সব `zava-*.wav` ফাইল ট্রান্সক্রাইব করে:

```bash
cd csharp
dotnet run whisper
```

স্যাম্পলটি `FoundryLocalManager.CreateAsync()` এবং SDK এর `AudioClient` (সেটিংসে `Language = "en"`) ব্যবহার করে ইন-প্রসেস ট্রান্সক্রিপশন করে।

</details>

**যা খেয়াল করবেন:** `samples/audio/generate_samples.py` এর মূল টেক্সটের সাথে ট্রান্সক্রিপশন আউটপুট তুলনা করুন। Whisper কীভাবে "Zava ProGrip" এর মত পণ্য নাম এবং "brushless motor" বা "composite decking" এর মতো প্রযুক্তিগত শব্দগুলো সঠিকভাবে ক্যাপচার করেছে?

---

### Exercise 5 - মূল কোড প্যাটার্নগুলি বোঝা

Whisper ট্রান্সক্রিপশন তিনটি ভাষার মধ্যে চ্যাট কমপ্লিশন থেকে কীভাবে ভিন্ন:

<details>
<summary><b>Python - চ্যাট থেকে মূল ফারাক</b></summary>

```python
# চ্যাট সম্পূর্ণ করা (অংশ ২-৬):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# অডিও ট্রান্সক্রিপশন (এই অংশ):
# OpenAI ক্লায়েন্টের পরিবর্তে সরাসরি ONNX Runtime ব্যবহার করে
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... স্বয়ংক্রিয় প্রজেক্টর ডিকোডার লুপ ...
print(tokenizer.decode(generated_tokens))
```

**মূল ধারণা:** চ্যাট মডেলগুলো OpenAI-সঙ্গত API ব্যবহার করে `manager.endpoint` মাধ্যমে। Whisper SDK ব্যবহার করে ক্যাশ্ড ONNX মডেল ফাইলগুলি খুঁজে, তারপর সরাসরি ONNX Runtime দিয়ে ইনফারেন্স চালায়।

</details>

<details>
<summary><b>JavaScript - চ্যাট থেকে মূল ফারাক</b></summary>

```javascript
// চ্যাট সম্পূর্ণকরণ (অংশ ২-৬):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// অডিও ট্রান্সক্রিপশন (এই অংশ):
// SDK-এর বিল্ট-ইন AudioClient ব্যবহার করে
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // সর্বোত্তম ফলাফলের জন্য সর্বদা ভাষা নির্ধারণ করুন
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**মূল ধারণা:** চ্যাট মডেলগুলো OpenAI-সঙ্গত API ব্যবহার করে `manager.urls[0] + "/v1"` মাধ্যমে। Whisper ট্রান্সক্রিপশন SDK’র `AudioClient` ব্যবহার করে, যা `model.createAudioClient()` থেকে পাওয়া যায়। গারবেজ আউটপুট থেকে বাঁচতে `settings.language` সেট করুন।

</details>

<details>
<summary><b>C# - চ্যাট থেকে মূল ফারাক</b></summary>

C# পদ্ধতিটি SDK’র বিল্ট-ইন `AudioClient` ব্যবহার করে ইন-প্রসেস ট্রান্সক্রিপশনের জন্য:

**মডেল ইনিশিয়ালাইজেশন:**

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

**ট্রান্সক্রিপশন:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**মূল ধারণা:** C# `FoundryLocalManager.CreateAsync()` ব্যবহার করে এবং সরাসরি একটি `AudioClient` নেয় — কোনো ONNX Runtime সেটআপ দরকার নেই। গারবেজ আউটপুট থেকে বাঁচতে `Settings.Language` সেট করুন।

</details>

> **সংক্ষিপ্তসার:** Python মডেল ম্যানেজমেন্টের জন্য Foundry Local SDK ব্যবহার করে + এনকোডার/ডিকোডার মডেলগুলোর জন্য সরাসরি ONNX Runtime। JavaScript এবং C# উভয়ই SDK’র বিল্ট-ইন `AudioClient` ব্যবহার করে সহজ ট্রান্সক্রিপশন — ক্লায়েন্ট তৈরি করুন, ভাষা সেট করুন, তারপর `transcribe()` / `TranscribeAudioAsync()` কল করুন। সবসময় AudioClient এর ভাষা প্রোপার্টি সেট করুন সঠিক ফলাফলের জন্য।

---

### Exercise 6 - পরীক্ষা-নিরীক্ষা করুন

আপনার বোঝার গভীরতা বাড়াতে এইসকল পরিবর্তন ট্রাই করুন:

1. **বিভিন্ন অডিও ফাইল ব্যবহার করুন** - Windows Voice Recorder দিয়ে নিজের কথা রেকর্ড করুন, WAV হিসেবে সেভ করুন, এবং ট্রান্সক্রাইব করুন

2. **মডেল ভ্যারিয়েন্ট তুলনা করুন** - যদি NVIDIA GPU থাকে, CUDA ভ্যারিয়েন্ট চেষ্টা করুন:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   CPU ভ্যারিয়েন্টের সাথে ট্রান্সক্রিপশন স্পিড তুলনা করুন।

3. **আউটপুট ফরম্যাটিং যোগ করুন** - JSON রেসপন্সে অন্তর্ভুক্ত থাকতে পারে:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **একটি REST API তৈরি করুন** - আপনার ট্রান্সক্রিপশন কোড একটি ওয়েব সার্ভারে মোড়া:

   | ভাষা | ফ্রেমওয়ার্ক | উদাহরণ |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` সাথে `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` সাথে `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` সাথে `IFormFile` |

5. **মাল্টি-টার্ন সহ ট্রান্সক্রিপশন** - Part 4 এর চ্যাট এজেন্টের সাথে Whisper একত্র করুন: প্রথমে অডিও ট্রান্সক্রাইব করুন, তারপর টেক্সটটি বিশ্লেষণ বা সারাংশের জন্য এজেন্টকে পাঠান।

---

## SDK অডিও API রেফারেন্স

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — একটি `AudioClient` ইনস্ট্যান্স তৈরি করে
> - `audioClient.settings.language` — ট্রান্সক্রিপশন ভাষা সেট করে (যেমন `"en"`)
> - `audioClient.settings.temperature` — এলোমেলোতা নিয়ন্ত্রণ (ঐচ্ছিক)
> - `audioClient.transcribe(filePath)` — ফাইল ট্রান্সক্রাইব করে, `{ text, duration }` রিটার্ন করে
> - `audioClient.transcribeStreaming(filePath, callback)` — কলব্যাকের মাধ্যমে ট্রান্সক্রিপশন চাঙ্ক স্ট্রিম করে
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — একটি `OpenAIAudioClient` ইনস্ট্যান্স তৈরি করে
> - `audioClient.Settings.Language` — ট্রান্সক্রিপশন ভাষা সেট করে (যেমন `"en"`)
> - `audioClient.Settings.Temperature` — এলোমেলোতা নিয়ন্ত্রণ (ঐচ্ছিক)
> - `await audioClient.TranscribeAudioAsync(filePath)` — ফাইল ট্রান্সক্রাইব করে, `.Text` সহ অবজেক্ট রিটার্ন করে
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — ট্রান্সক্রিপশন চাঙ্কের `IAsyncEnumerable` রিটার্ন করে

> **টিপ:** সর্বদা ট্রান্সক্রাইব করার আগে ভাষা প্রোপার্টি সেট করুন। সেট না করলে Whisper মডেল স্বয়ংক্রিয় সনাক্তকরণ চালায়, যা গারবেজ আউটপুট (একটি রিপ্লেসমেন্ট ক্যারেক্টার) সৃষ্টি করতে পারে।

---

## তুলনা: চ্যাট মডেল বনাম Whisper

| দিক | চ্যাট মডেল (পার্ট ৩-৭) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **কার্যকারিতা টাইপ** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **ইনপুট** | টেক্সট মেসেজ (JSON) | অডিও ফাইল (WAV/MP3/M4A) | অডিও ফাইল (WAV/MP3/M4A) |
| **আউটপুট** | জেনারেটেড টেক্সট (স্ট্রিম করা) | সম্পূর্ণ ট্রান্সক্রাইব টেক্সট | সম্পূর্ণ ট্রান্সক্রাইব টেক্সট |
| **SDK প্যাকেজ** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API পদ্ধতি** | `client.chat.completions.create()` | ONNX Runtime সরাসরি | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **ভাষা সেটিং** | প্রযোজ্য নয় | ডিকোডার প্রম্পট টোকেন | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **স্ট্রিমিং** | হ্যাঁ | না | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **প্রাইভেসি সুবিধা** | কোড/ডাটা লোকাল থাকে | অডিও ডাটা লোকাল থাকে | অডিও ডাটা লোকাল থাকে |

---

## মূল উপসংহার

| ধারণা | আপনি যা শিখলেন |
|---------|-----------------|
| **Whisper ডিজাইনে** | স্পিচ-টু-টেক্সট সম্পূর্ণ লোকালি চলে, Zava গ্রাহক কল ও পণ্য রিভিউ অন-ডিভাইসে ট্রান্সক্রাইব করার জন্য আদর্শ |
| **SDK AudioClient** | JavaScript ও C# SDK বিল্ট-ইন `AudioClient` প্রদান করে যা সম্পূর্ণ ট্রান্সক্রিপশন পাইপলাইন একবারে হ্যান্ডেল করে |
| **ভাষা সেটিং** | সর্বদা AudioClient-এর ভাষা সেট করুন (যেমন `"en"`) — না করলে স্বয়ংক্রিয় সনাক্তকরণ গারবেজ আউটপুট দিতে পারে |
| **Python** | মডেল ম্যানেজমেন্টে `foundry-local-sdk` + সরাসরি ONNX ইনফারেন্সে `onnxruntime` + `transformers` + `librosa` ব্যবহার করে |
| **JavaScript** | `foundry-local-sdk` ব্যবহার করে `model.createAudioClient()` — `settings.language` সেট করে, তারপর `transcribe()` কল করে |
| **C#** | `Microsoft.AI.Foundry.Local` ব্যবহার করে `model.GetAudioClientAsync()` — `Settings.Language` সেট করে, তারপর `TranscribeAudioAsync()` কল করে |
| **স্ট্রিমিং সাপোর্ট** | JS ও C# SDK তে `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` ফিচারও আছে চাঙ্ক-বাই-চাঙ্ক আউটপুটের জন্য |
| **CPU-অপটিমাইজড** | CPU ভ্যারিয়েন্ট (3.05 GB) যেকোনো Windows ডিভাইসে GPU ছাড়াই কাজ করে |
| **প্রাইভেসি-ফার্স্ট** | Zava গ্রাহক ইন্টার‍্যাকশন ও প্রোপাইটারি প্রোডাক্ট ডেটা অন-ডিভাইসে রাখার জন্য পারফেক্ট |

---

## রিসোর্সসমূহ

| রিসোর্স | লিঙ্ক |
|----------|------|
| Foundry Local ডকুমেন্টেশন | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK রেফারেন্স | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper মডেল | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local ওয়েবসাইট | [foundrylocal.ai](https://foundrylocal.ai) |

---

## পরবর্তী ধাপ

[Part 10: Using Custom or Hugging Face Models](part10-custom-models.md) এ এগিয়ে যান যাতে আপনি Hugging Face থেকে আপনার নিজস্ব মডেল তৈরি করে Foundry Local দিয়ে চালাতে পারেন।