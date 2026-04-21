![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ส่วนที่ 9: การถอดเสียงด้วยเสียงพูดโดยใช้ Whisper และ Foundry Local

> **เป้าหมาย:** ใช้โมเดล OpenAI Whisper ที่รันในเครื่องผ่าน Foundry Local เพื่อถอดเสียงไฟล์เสียง — ทำงานทั้งหมดบนอุปกรณ์โดยไม่ต้องใช้คลาวด์เลย

## ภาพรวม

Foundry Local ไม่ได้ใช้แค่สำหรับการสร้างข้อความเท่านั้น แต่ยังรองรับโมเดล **speech-to-text (คำพูดเป็นข้อความ)** ด้วย ในแลบนี้คุณจะใช้โมเดล **OpenAI Whisper Medium** เพื่อถอดเสียงไฟล์เสียงทั้งหมดบนเครื่องของคุณ เหมาะสำหรับสถานการณ์เช่น การถอดเสียงโทรศัพท์บริการลูกค้าของ Zava, การบันทึกรีวิวสินค้า หรือการวางแผนเวิร์กช็อปที่ข้อมูลเสียงไม่ควรออกจากอุปกรณ์ของคุณ


---

## วัตถุประสงค์การเรียนรู้

เมื่อจบแลบนี้คุณจะสามารถ:

- เข้าใจโมเดล Whisper สำหรับแปลงเสียงเป็นข้อความและความสามารถของมัน
- ดาวน์โหลดและรันโมเดล Whisper โดยใช้ Foundry Local
- ถอดเสียงไฟล์เสียงโดยใช้ Foundry Local SDK ใน Python, JavaScript และ C#
- สร้างบริการถอดเสียงง่ายๆ ที่ทำงานทั้งหมดบนเครื่อง
- เข้าใจความแตกต่างระหว่างโมเดลแชท/ข้อความและโมเดลเสียงใน Foundry Local

---

## ข้อกำหนดพื้นฐาน

| ข้อกำหนด | รายละเอียด |
|-------------|---------|
| **Foundry Local CLI** | เวอร์ชัน **0.8.101 ขึ้นไป** (โมเดล Whisper ใช้งานได้ตั้งแต่ v0.8.101 เป็นต้นไป) |
| **ระบบปฏิบัติการ** | Windows 10/11 (x64 หรือ ARM64) |
| **รันไทม์ภาษา** | **Python 3.9+** และ/หรือ **Node.js 18+** และ/หรือ **.NET 9 SDK** ([ดาวน์โหลด .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **ที่ทำเสร็จแล้ว** | [ส่วนที่ 1: เริ่มต้นใช้งาน](part1-getting-started.md), [ส่วนที่ 2: เจาะลึก Foundry Local SDK](part2-foundry-local-sdk.md) และ [ส่วนที่ 3: SDK และ API](part3-sdk-and-apis.md) |

> **หมายเหตุ:** โมเดล Whisper ต้องดาวน์โหลดผ่าน **SDK** (ไม่ใช่ CLI) เนื่องจาก CLI ไม่รองรับ endpoint การถอดเสียง ตรวจสอบเวอร์ชันของคุณด้วย:
> ```bash
> foundry --version
> ```

---

## แนวคิด: วิธีที่ Whisper ทำงานกับ Foundry Local

โมเดล OpenAI Whisper คือโมเดลรู้จำเสียงพูดทั่วไปที่ถูกฝึกด้วยชุดข้อมูลเสียงที่หลากหลายขนาดใหญ่ เมื่อตัวโมเดลทำงานผ่าน Foundry Local:

- โมเดลทำงาน **ทั้งหมดบน CPU ของคุณ** — ไม่ต้องใช้ GPU
- ข้อมูลเสียงไม่ออกจากอุปกรณ์ของคุณ — **ความเป็นส่วนตัวเต็มรูปแบบ**
- Foundry Local SDK ดูแลการดาวน์โหลดและจัดการแคชโมเดล
- **JavaScript และ C#** มีคลาส `AudioClient` ใน SDK ที่จัดการ pipeline การถอดเสียงทั้งหมด — ไม่ต้องตั้งค่า ONNX ด้วยตัวเอง
- **Python** ใช้ SDK สำหรับการจัดการโมเดลและใช้ ONNX Runtime สำหรับการ inference ตรงกับไฟล์โมเดล encoder/decoder ONNX

### วิธีการทำงานของ Pipeline (JavaScript และ C#) — SDK AudioClient

1. **Foundry Local SDK** ดาวน์โหลดและแคชโมเดล Whisper
2. `model.createAudioClient()` (JS) หรือ `model.GetAudioClientAsync()` (C#) สร้าง `AudioClient`
3. `audioClient.transcribe(path)` (JS) หรือ `audioClient.TranscribeAudioAsync(path)` (C#) จัดการ pipeline ทั้งหมด — preprocessing ของเสียง, encoder, decoder และ token decoding
4. `AudioClient` เปิดเผยคุณสมบัติ `settings.language` (ตั้งให้เป็น `"en"` สำหรับภาษาอังกฤษ) เพื่อให้การถอดเสียงมีความแม่นยำ

### วิธีการทำงานของ Pipeline (Python) — ONNX Runtime

1. **Foundry Local SDK** ดาวน์โหลดและแคชไฟล์โมเดล ONNX ของ Whisper
2. **การเตรียมข้อมูลเสียง** แปลงไฟล์ WAV เป็น mel spectrogram (80 mel bins x 3000 เฟรม)
3. **Encoder** ประมวลผล mel spectrogram และสร้าง hidden states พร้อม cross-attention key/value tensors
4. **Decoder** ทำงานแบบ autoregressive สร้างทีละ token จนกว่าจะเจอโทเค็นสิ้นสุดข้อความ
5. **Tokeniser** ถอดรหัส token ID กลับเป็นข้อความที่อ่านได้

### รุ่นโมเดล Whisper

| ชื่อเล่น | รหัสโมเดล | อุปกรณ์ | ขนาด | คำอธิบาย |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | เร่งความเร็วบน GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | ปรับแต่งสำหรับ CPU (แนะนำสำหรับอุปกรณ์ส่วนใหญ่) |

> **หมายเหตุ:** ต่างจากโมเดลแชทที่แสดงโดยค่าเริ่มต้น, โมเดล Whisper จะถูกจัดหมวดหมู่ภายใต้ task `automatic-speech-recognition` ใช้คำสั่ง `foundry model info whisper-medium` เพื่อดูรายละเอียด

---

## แบบฝึกหัดแลบ

### แบบฝึกหัด 0 - รับไฟล์เสียงตัวอย่าง

แลบนี้มีไฟล์ WAV ที่เตรียมไว้แล้วซึ่งจำลองสถานการณ์ผลิตภัณฑ์ Zava DIY สร้างไฟล์เหล่านี้โดยใช้สคริปต์ที่ให้มา:

```bash
# จากโฟลเดอร์รากของรีโพ - สร้างและเปิดใช้งาน .venv ก่อน
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

จะสร้างไฟล์ WAV หกไฟล์ในไดเรกทอรี `samples/audio/`:

| ไฟล์ | สถานการณ์ |
|------|----------|
| `zava-customer-inquiry.wav` | ลูกค้าสอบถามเกี่ยวกับ **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | ลูกค้ารีวิว **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | โทรศัพท์ฝ่ายสนับสนุนเรื่อง **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | คน DIY วางแผนทำเด็คด้วย **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | สาธิตเวิร์กช็อปโดยใช้ **สินค้าทั้ง 5 ของ Zava** |
| `zava-full-project-walkthrough.wav` | สาธิตรีโนเวทโรงรถแบบยาวโดยใช้ **สินค้าทั้งหมดของ Zava** (~4 นาที สำหรับทดสอบเสียงยาว) |

> **เคล็ดลับ:** คุณสามารถใช้ไฟล์ WAV/MP3/M4A ของตัวเอง หรือบันทึกเสียงด้วย Windows Voice Recorder ก็ได้

---

### แบบฝึกหัด 1 - ดาวน์โหลดโมเดล Whisper ด้วย SDK

เนื่องจาก CLI ไม่รองรับโมเดล Whisper ในเวอร์ชัน Foundry Local ใหม่ๆ ให้ใช้ **SDK** ในการดาวน์โหลดและโหลดโมเดล เลือกภาษาของคุณ:

<details>
<summary><b>🐍 Python</b></summary>

**ติดตั้ง SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# เริ่มบริการ
manager = FoundryLocalManager()
manager.start_service()

# ตรวจสอบข้อมูลแคตตาล็อก
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# ตรวจสอบว่ามีการแคชแล้วหรือไม่
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# โหลดโมเดลเข้าสู่หน่วยความจำ
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

บันทึกไฟล์เป็น `download_whisper.py` แล้วรัน:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**ติดตั้ง SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// สร้างผู้จัดการและเริ่มต้นบริการ
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ดึงโมเดลจากแคตตาล็อก
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

// โหลดโมเดลเข้าสู่หน่วยความจำ
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

บันทึกไฟล์เป็น `download-whisper.mjs` แล้วรัน:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**ติดตั้ง SDK:**
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

> **ทำไมใช้ SDK แทน CLI?** Foundry Local CLI ไม่รองรับการดาวน์โหลดหรือให้บริการโมเดล Whisper โดยตรง SDK ให้วิธีที่เสถียรสำหรับการดาวน์โหลดและจัดการโมเดลเสียงผ่านโปรแกรม JavaScript และ C# มี `AudioClient` ในตัวจัดการ pipeline ถอดเสียงทั้งหมด ส่วน Python ใช้ ONNX Runtime สำหรับ inference ตรงกับไฟล์โมเดลที่แคชไว้

---

### แบบฝึกหัด 2 - เข้าใจ SDK ของ Whisper

การถอดเสียง Whisper มีวิธีต่างกันตามภาษา **JavaScript และ C#** มีคลาส `AudioClient` ใน Foundry Local SDK ที่จัดการ pipeline ทั้งหมด (preprocessing เสียง, encoder, decoder, token decoding) ในคำสั่งเดียว ส่วน **Python** ใช้ SDK สำหรับการจัดการโมเดลและ ONNX Runtime สำหรับ inference ตรงกับโมเดล encoder/decoder ONNX

| องค์ประกอบ | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **แพ็กเกจ SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **การจัดการโมเดล** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **การแยกคุณสมบัติ** | `WhisperFeatureExtractor` + `librosa` | จัดการโดย SDK `AudioClient` | จัดการโดย SDK `AudioClient` |
| **Inference** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **ถอดรหัส token** | `WhisperTokenizer` | จัดการโดย SDK `AudioClient` | จัดการโดย SDK `AudioClient` |
| **การตั้งค่าภาษา** | ตั้งค่าใน `forced_ids` ของ token decoder | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **อินพุต** | เส้นทางไฟล์ WAV | เส้นทางไฟล์ WAV | เส้นทางไฟล์ WAV |
| **ผลลัพธ์** | ข้อความที่ถอดรหัสแล้ว | `result.text` | `result.Text` |

> **สำคัญ:** ต้องตั้งค่าภาษาใน `AudioClient` เสมอ เช่น `"en"` สำหรับภาษาอังกฤษ หากไม่ตั้งค่าอย่างชัดเจน โมเดลอาจถอดเสียงออกมาเป็นข้อความผิดเพี้ยนเพราะพยายามตรวจจับภาษาเอง

> **รูปแบบ SDK:** Python ใช้ `FoundryLocalManager(alias)` บูตสตาร์ท แล้วใช้ `get_cache_location()` หาตำแหน่งไฟล์โมเดล ONNX JavaScript และ C# ใช้ `AudioClient` ใน SDK ที่ได้จาก `model.createAudioClient()` (JS) หรือ `model.GetAudioClientAsync()` (C#) ซึ่งจัดการ pipeline ถอดเสียงทั้งหมด ดูรายละเอียดได้ที่ [ส่วนที่ 2: เจาะลึก Foundry Local SDK](part2-foundry-local-sdk.md)

---

### แบบฝึกหัด 3 - สร้างแอปถอดเสียงง่ายๆ

เลือกภาษาที่คุณใช้และสร้างแอปพลิเคชันขนาดเล็กที่ถอดเสียงไฟล์เสียง

> **ฟอร์แมตเสียงที่รองรับ:** WAV, MP3, M4A สำหรับผลลัพธ์ดีที่สุดควรใช้ไฟล์ WAV ที่อัตราสุ่มตัวอย่าง 16kHz

<details>
<summary><h3>เส้นทาง Python</h3></summary>

#### การตั้งค่า

```bash
cd python
python -m venv venv

# เปิดใช้งานสภาพแวดล้อมเสมือน:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### โค้ดถอดเสียง

สร้างไฟล์ `foundry-local-whisper.py`:

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

# ขั้นตอนที่ 1: บูตสแตรป - เริ่มต้นบริการ ดาวน์โหลด และโหลดโมเดล
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# สร้างเส้นทางไปยังไฟล์โมเดล ONNX ที่แคชไว้
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# ขั้นตอนที่ 2: โหลดเซสชัน ONNX และตัวดึงคุณสมบัติ
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

# ขั้นตอนที่ 3: ดึงคุณสมบัติเมลสเปกโตรแกรม
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# ขั้นตอนที่ 4: รันเอนโค้ดเดอร์
enc_out = encoder.run(None, {"audio_features": input_features})
# ผลลัพธ์แรกคือสถานะที่ซ่อนอยู่; ที่เหลือคือคู่ค่าวิธี cross-attention KV
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# ขั้นตอนที่ 5: การถอดรหัสแบบออโต้รีเกรสซีฟ
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, ถอดเสียง, ไม่มีเวลาประทับ
input_ids = np.array([initial_tokens], dtype=np.int32)

# แคช KV ของ self-attention ว่างเปล่า
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

    if next_token == 50257:  # สิ้นสุดข้อความ
        break
    generated.append(next_token)

    # อัปเดตแคช KV ของ self-attention
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### รันโปรแกรม

```bash
# ถอดเสียงสถานการณ์ผลิตภัณฑ์ของ Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# หรือลองอันอื่น:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### จุดสำคัญของ Python

| เมธอด | จุดประสงค์ |
|--------|---------|
| `FoundryLocalManager(alias)` | บูตสตาร์ทบริการ ดาวน์โหลด และโหลดโมเดล |
| `manager.get_cache_location()` | ดึงเส้นทางไฟล์โมเดล ONNX ที่แคชไว้ |
| `WhisperFeatureExtractor.from_pretrained()` | โหลดตัวดึงคุณสมบัติ mel spectrogram |
| `ort.InferenceSession()` | สร้างเซสชัน ONNX Runtime สำหรับ encoder และ decoder |
| `tokenizer.decode()` | แปลง token ID ที่ได้กลับเป็นข้อความ |

</details>

<details>
<summary><h3>เส้นทาง JavaScript</h3></summary>

#### การตั้งค่า

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### โค้ดถอดเสียง

สร้างไฟล์ `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// ขั้นตอนที่ 1: Bootstrap - สร้างผู้จัดการ เริ่มบริการ และโหลดโมเดล
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

// ขั้นตอนที่ 2: สร้างไคลเอนต์เสียงและถอดความ
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// ทำความสะอาด
await model.unload();
```

> **หมายเหตุ:** Foundry Local SDK มี `AudioClient` ในตัวผ่าน `model.createAudioClient()` ที่จัดการ pipeline inference ONNX ทั้งหมดภายใน — ไม่ต้องนำเข้า `onnxruntime-node` ตั้งค่า `audioClient.settings.language = "en"` เสมอเพื่อให้ถอดเสียงภาษาอังกฤษแม่นยำ

#### รันโปรแกรม

```bash
# ถอดความสถานการณ์ผลิตภัณฑ์ Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# หรือลองอย่างอื่น:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### จุดสำคัญของ JavaScript

| เมธอด | จุดประสงค์ |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | สร้าง singleton manager |
| `await catalog.getModel(alias)` | ดึงโมเดลจากแคตตาล็อก |
| `model.download()` / `model.load()` | ดาวน์โหลดและโหลดโมเดล Whisper |
| `model.createAudioClient()` | สร้าง audio client สำหรับถอดเสียง |
| `audioClient.settings.language = "en"` | ตั้งค่าภาษาเพื่อถอดเสียงอย่างแม่นยำ |
| `audioClient.transcribe(path)` | ถอดเสียงไฟล์ ให้ผลลัพธ์เป็น `{ text, duration }` |

</details>

<details>
<summary><h3>เส้นทาง C#</h3></summary>

#### การตั้งค่า

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **หมายเหตุ:** เส้นทาง C# ใช้แพ็กเกจ `Microsoft.AI.Foundry.Local` ซึ่งมี `AudioClient` ในตัวผ่าน `model.GetAudioClientAsync()` จัดการ pipeline ถอดเสียงทั้งหมดในโปรเซสเดียว — ไม่ต้องตั้งค่า ONNX Runtime แยกต่างหาก

#### โค้ดถอดเสียง

แทนที่เนื้อหาใน `Program.cs`:

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

#### รันโปรแกรม

```bash
# ถอดความสถานการณ์ผลิตภัณฑ์ Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# หรือ ลองแบบอื่น ๆ:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### จุดสำคัญของ C#

| เมธอด | จุดประสงค์ |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | เริ่มต้น Foundry Local ด้วยการตั้งค่า |
| `catalog.GetModelAsync(alias)` | ดึงโมเดลจากแคตตาล็อก |
| `model.DownloadAsync()` | ดาวน์โหลดโมเดล Whisper |
| `model.GetAudioClientAsync()` | ดึง AudioClient (ไม่ใช่ ChatClient!) |
| `audioClient.Settings.Language = "en"` | ตั้งค่าภาษาเพื่อถอดเสียงอย่างแม่นยำ |
| `audioClient.TranscribeAudioAsync(path)` | ถอดเสียงไฟล์เสียง |
| `result.Text` | ข้อความที่ถอดเสียงได้ |


> **C# vs Python/JS:** SDK ของ C# มี `AudioClient` ในตัวสำหรับการถอดเสียงแบบในกระบวนการผ่าน `model.GetAudioClientAsync()` คล้ายกับ SDK ของ JavaScript ส่วน Python ใช้ ONNX Runtime โดยตรงสำหรับการคาดการณ์กับโมเดล encoder/decoder ที่แคชไว้

</details>

---

### แบบฝึกหัด 4 - ถอดเสียงชุดตัวอย่าง Zava ทั้งหมด

ตอนนี้คุณมีแอปถอดเสียงที่ทำงานได้แล้ว ให้ถอดเสียงไฟล์ตัวอย่าง Zava ทั้ง 5 ไฟล์และเปรียบเทียบผลลัพธ์

<details>
<summary><h3>เส้นทาง Python</h3></summary>

ไฟล์ตัวอย่างเต็ม `python/foundry-local-whisper.py` รองรับการถอดเสียงแบบชุดแล้ว เมื่อรันโดยไม่ใส่อาร์กิวเมนต์ จะถอดเสียงไฟล์ `zava-*.wav` ทั้งหมดในโฟลเดอร์ `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

ไฟล์ตัวอย่างใช้ `FoundryLocalManager(alias)` เพื่อเริ่มต้น จากนั้นรันเซสชัน encoder และ decoder ONNX สำหรับแต่ละไฟล์

</details>

<details>
<summary><h3>เส้นทาง JavaScript</h3></summary>

ไฟล์ตัวอย่างเต็ม `javascript/foundry-local-whisper.mjs` รองรับการถอดเสียงแบบชุดแล้ว เมื่อรันโดยไม่ใส่อาร์กิวเมนต์ จะถอดเสียงไฟล์ `zava-*.wav` ทั้งหมดในโฟลเดอร์ `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

ไฟล์ตัวอย่างใช้ `FoundryLocalManager.create()` และ `catalog.getModel(alias)` เพื่อเริ่ม SDK จากนั้นใช้ `AudioClient` (พร้อม `settings.language = "en"`) เพื่อถอดเสียงแต่ละไฟล์

</details>

<details>
<summary><h3>เส้นทาง C#</h3></summary>

ไฟล์ตัวอย่างเต็ม `csharp/WhisperTranscription.cs` รองรับการถอดเสียงแบบชุดแล้ว เมื่อรันโดยไม่ระบุไฟล์เฉพาะ จะถอดเสียงไฟล์ `zava-*.wav` ทั้งหมดในโฟลเดอร์ `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

ไฟล์ตัวอย่างใช้ `FoundryLocalManager.CreateAsync()` และ `AudioClient` ของ SDK (พร้อม `Settings.Language = "en"`) สำหรับการถอดเสียงในกระบวนการ

</details>

**สิ่งที่ควรสังเกต:** เปรียบเทียบผลลัพธ์การถอดเสียงกับข้อความต้นฉบับใน `samples/audio/generate_samples.py` Whisper สามารถจับชื่อผลิตภัณฑ์เช่น "Zava ProGrip" และคำศัพท์ทางเทคนิค เช่น "brushless motor" หรือ "composite decking" ได้แม่นยำแค่ไหน?

---

### แบบฝึกหัด 5 - ทำความเข้าใจรูปแบบโค้ดสำคัญ

ศึกษาว่าการถอดเสียง Whisper แตกต่างจากการทำ chat completions ในทั้งสามภาษาอย่างไร:

<details>
<summary><b>Python - ความแตกต่างสำคัญจาก Chat</b></summary>

```python
# การเติมข้อความแชท (ตอนที่ 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# การถอดเสียงเสียง (ตอนนี้):
# ใช้ ONNX Runtime โดยตรงแทนที่จะใช้ไคลเอนต์ OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... วนลูปดีโค้ดเดอร์ออโตเรเกรสซีฟ ...
print(tokenizer.decode(generated_tokens))
```

**ข้อสังเกตหลัก:** โมเดล Chat ใช้ API ที่เข้ากันได้กับ OpenAI ผ่าน `manager.endpoint` Whisper ใช้ SDK เพื่อค้นหาไฟล์โมเดล ONNX ที่แคชไว้ จากนั้นรัน inference โดยตรงกับ ONNX Runtime

</details>

<details>
<summary><b>JavaScript - ความแตกต่างสำคัญจาก Chat</b></summary>

```javascript
// การแชทแบบสมบูรณ์ (ส่วนที่ 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// การถอดเสียงเสียง (ส่วนนี้):
// ใช้ AudioClient ในตัวของ SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // กำหนดภาษาตลอดเวลาเพื่อผลลัพธ์ที่ดีที่สุด
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**ข้อสังเกตหลัก:** โมเดล Chat ใช้ API ที่เข้ากันได้กับ OpenAI ผ่าน `manager.urls[0] + "/v1"` การถอดเสียง Whisper ใช้ `AudioClient` ของ SDK ที่ได้จาก `model.createAudioClient()` ตั้งค่า `settings.language` เพื่อป้องกันผลลัพธ์ที่ผิดเพี้ยนจากการตรวจจับอัตโนมัติ

</details>

<details>
<summary><b>C# - ความแตกต่างสำคัญจาก Chat</b></summary>

วิธีของ C# ใช้ `AudioClient` ในตัวของ SDK สำหรับการถอดเสียงในกระบวนการ:

**การเริ่มต้นโมเดล:**

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

**การถอดเสียง:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**ข้อสังเกตหลัก:** C# ใช้ `FoundryLocalManager.CreateAsync()` และได้ `AudioClient` โดยตรง — ไม่ต้องตั้งค่า ONNX Runtime ตั้งค่า `Settings.Language` เพื่อป้องกันผลลัพธ์ที่ผิดเพี้ยนจากการตรวจจับอัตโนมัติ

</details>

> **สรุป:** Python ใช้ Foundry Local SDK สำหรับการจัดการโมเดลและ ONNX Runtime เพื่อทำ inference โดยตรงกับโมเดล encoder/decoder JavaScript และ C# ต่างใช้ `AudioClient` ในตัวของ SDK สำหรับการถอดเสียงที่ง่ายขึ้น — สร้าง client ตั้งค่าภาษา แล้วเรียก `transcribe()` / `TranscribeAudioAsync()` เสมอให้ตั้งค่าภาษาใน AudioClient เพื่อผลลัพธ์ที่แม่นยำ

---

### แบบฝึกหัด 6 - ทดลองเล่น

ลองแก้ไขเหล่านี้เพื่อเพิ่มพูนความเข้าใจของคุณ:

1. **ลองไฟล์เสียงต่างๆ** - อัดเสียงพูดของคุณเองโดยใช้ Windows Voice Recorder บันทึกเป็นไฟล์ WAV แล้วถอดเสียง

2. **เปรียบเทียบโมเดลเวอร์ชันต่างๆ** - หากคุณมี GPU NVIDIA ให้ลองใช้รุ่น CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   เปรียบเทียบความเร็วการถอดเสียงกับรุ่น CPU

3. **เพิ่มรูปแบบผลลัพธ์** - การตอบกลับแบบ JSON สามารถรวม:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **สร้าง REST API** - ห่อโค้ดถอดเสียงของคุณในเว็บเซิร์ฟเวอร์:

   | ภาษา | เฟรมเวิร์ก | ตัวอย่าง |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` กับ `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` กับ `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` กับ `IFormFile` |

5. **หลายรอบด้วยการถอดเสียง** - รวม Whisper กับแชทเอเยนต์จากส่วนที่ 4: ถอดเสียงก่อน แล้วส่งข้อความให้เอเยนต์วิเคราะห์หรือสรุป

---

## เอกสารอ้างอิง SDK Audio API

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — สร้างอินสแตนซ์ `AudioClient`
> - `audioClient.settings.language` — ตั้งค่าภาษาในการถอดเสียง (เช่น `"en"`)
> - `audioClient.settings.temperature` — ควบคุมความสุ่ม (ตัวเลือก)
> - `audioClient.transcribe(filePath)` — ถอดเสียงไฟล์, คืนค่า `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — สตรีมการถอดเสียงแบบชิ้นส่วนผ่าน callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — สร้างอินสแตนซ์ `OpenAIAudioClient`
> - `audioClient.Settings.Language` — ตั้งค่าภาษาในการถอดเสียง (เช่น `"en"`)
> - `audioClient.Settings.Temperature` — ควบคุมความสุ่ม (ตัวเลือก)
> - `await audioClient.TranscribeAudioAsync(filePath)` — ถอดเสียงไฟล์, คืนวัตถุที่มี `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — คืน `IAsyncEnumerable` ของชิ้นส่วนถอดเสียง

> **คำแนะนำ:** ตั้งค่าคุณสมบัติภาษาเสมอก่อนถอดเสียง หากไม่ตั้งค่า โมเดล Whisper จะพยายามตรวจจับภาษาอัตโนมัติ ซึ่งอาจให้ผลลัพธ์ที่เพี้ยน (เป็นสัญลักษณ์แทนข้อความ)

---

## การเปรียบเทียบ: โมเดลแชทกับ Whisper

| แง่มุม | โมเดลแชท (ส่วนที่ 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **ประเภทงาน** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **อินพุต** | ข้อความ (JSON) | ไฟล์เสียง (WAV/MP3/M4A) | ไฟล์เสียง (WAV/MP3/M4A) |
| **เอาต์พุต** | ข้อความที่สร้าง (สตรีม) | ข้อความถอดเสียง (สมบูรณ์) | ข้อความถอดเสียง (สมบูรณ์) |
| **แพ็กเกจ SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **เมธอด API** | `client.chat.completions.create()` | ONNX Runtime โดยตรง | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **การตั้งค่าภาษา** | ไม่มี | โทเค็น prompt decoder | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **รองรับสตรีมมิ่ง** | มี | ไม่มี | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **ประโยชน์ด้านความเป็นส่วนตัว** | โค้ด/ข้อมูลอยู่ในเครื่อง | ข้อมูลเสียงอยู่ในเครื่อง | ข้อมูลเสียงอยู่ในเครื่อง |

---

## ข้อสรุปสำคัญ

| แนวคิด | สิ่งที่คุณเรียนรู้ |
|---------|-----------------|
| **Whisper บนอุปกรณ์** | การแปลงเสียงเป็นข้อความทำงานทั้งหมดในเครื่อง เหมาะสำหรับการถอดเสียงการโทรและรีวิวสินค้าของลูกค้า Zava บนอุปกรณ์ |
| **SDK AudioClient** | SDK JavaScript และ C# มี `AudioClient` ในตัวที่จัดการกระบวนการถอดเสียงทั้งหมดในคำสั่งเดียว |
| **การตั้งค่าภาษา** | ตั้งค่าภาษาใน AudioClient เสมอ (เช่น `"en"`) หากไม่ตั้งค่า การตรวจจับอัตโนมัติอาจให้ผลลัพธ์เพี้ยน |
| **Python** | ใช้ `foundry-local-sdk` สำหรับจัดการโมเดล + `onnxruntime` + `transformers` + `librosa` เพื่อ inference ONNX โดยตรง |
| **JavaScript** | ใช้ `foundry-local-sdk` พร้อม `model.createAudioClient()` — ตั้งค่า `settings.language` แล้วเรียก `transcribe()` |
| **C#** | ใช้ `Microsoft.AI.Foundry.Local` กับ `model.GetAudioClientAsync()` — ตั้งค่า `Settings.Language` แล้วเรียก `TranscribeAudioAsync()` |
| **รองรับสตรีมมิ่ง** | SDK JS และ C# ยังมี `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` สำหรับผลลัพธ์เป็นชิ้น |
| **เหมาะกับ CPU** | รุ่น CPU (3.05 GB) ทำงานบนอุปกรณ์ Windows ใดก็ได้โดยไม่ต้องใช้ GPU |
| **เน้นความเป็นส่วนตัว** | เหมาะสำหรับเก็บการโต้ตอบกับลูกค้า Zava และข้อมูลผลิตภัณฑ์ลับบนเครื่อง |

---

## แหล่งข้อมูล

| ทรัพยากร | ลิงก์ |
|----------|------|
| เอกสาร Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| อ้างอิง SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| โมเดล OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| เว็บไซต์ Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## ก้าวต่อไป

ไปที่ [ส่วนที่ 10: การใช้โมเดล Custom หรือ Hugging Face](part10-custom-models.md) เพื่อรวบรวมโมเดลของคุณเองจาก Hugging Face และรันผ่าน Foundry Local ต่อไป

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ข้อจำกัดความรับผิดชอบ**:  
เอกสารนี้ได้รับการแปลโดยใช้บริการแปลภาษาด้วย AI [Co-op Translator](https://github.com/Azure/co-op-translator) แม้ว่าเราจะพยายามให้ความถูกต้อง โปรดทราบว่าการแปลอัตโนมัติอาจมีข้อผิดพลาดหรือความไม่ถูกต้อง เอกสารต้นฉบับในภาษาต้นทางควรถูกพิจารณาว่าเป็นแหล่งข้อมูลที่เป็นทางการ สำหรับข้อมูลที่มีความสำคัญ แนะนำให้ใช้บริการแปลภาษามนุษย์มืออาชีพ เราไม่รับผิดชอบต่อความเข้าใจผิดหรือการตีความผิดที่เกิดจากการใช้การแปลนี้
<!-- CO-OP TRANSLATOR DISCLAIMER END -->