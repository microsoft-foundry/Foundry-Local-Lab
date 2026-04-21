![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Phần 9: Chuyển đổi giọng nói thành văn bản với Whisper và Foundry Local

> **Mục tiêu:** Sử dụng mô hình OpenAI Whisper chạy trên thiết bị cục bộ thông qua Foundry Local để chuyển đổi các tệp âm thanh thành văn bản - hoàn toàn trên thiết bị, không cần đám mây.

## Tổng quan

Foundry Local không chỉ dành cho tạo văn bản; nó còn hỗ trợ các mô hình **chuyển đổi giọng nói thành văn bản**. Trong phòng thí nghiệm này, bạn sẽ sử dụng mô hình **OpenAI Whisper Medium** để chuyển đổi các tệp âm thanh hoàn toàn trên máy của bạn. Điều này lý tưởng cho các tình huống như chuyển đổi các cuộc gọi dịch vụ khách hàng của Zava, ghi âm đánh giá sản phẩm hoặc các phiên lập kế hoạch hội thảo mà dữ liệu âm thanh không được phép rời khỏi thiết bị của bạn.

---

## Mục tiêu học tập

Sau khi hoàn thành phòng thí nghiệm này, bạn sẽ có thể:

- Hiểu mô hình chuyển đổi giọng nói thành văn bản Whisper và khả năng của nó
- Tải xuống và chạy mô hình Whisper sử dụng Foundry Local
- Chuyển đổi các tập tin âm thanh thành văn bản sử dụng Foundry Local SDK trong Python, JavaScript và C#
- Xây dựng dịch vụ chuyển đổi giọng nói đơn giản chạy hoàn toàn trên thiết bị
- Hiểu sự khác biệt giữa các mô hình chat/văn bản và mô hình âm thanh trong Foundry Local

---

## Yêu cầu tiên quyết

| Yêu cầu | Chi tiết |
|-------------|---------|
| **Foundry Local CLI** | Phiên bản **0.8.101 trở lên** (mô hình Whisper có sẵn từ phiên bản v0.8.101 trở đi) |
| **Hệ điều hành** | Windows 10/11 (x64 hoặc ARM64) |
| **Ngôn ngữ chạy** | **Python 3.9+** và/hoặc **Node.js 18+** và/hoặc **.NET 9 SDK** ([Tải .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Hoàn thành** | [Phần 1: Bắt đầu](part1-getting-started.md), [Phần 2: Tìm hiểu sâu về Foundry Local SDK](part2-foundry-local-sdk.md), và [Phần 3: SDK và APIs](part3-sdk-and-apis.md) |

> **Lưu ý:** Các mô hình Whisper phải được tải xuống qua **SDK** (không phải CLI). CLI không hỗ trợ điểm cuối chuyển đổi âm thanh thành văn bản. Kiểm tra phiên bản của bạn với:
> ```bash
> foundry --version
> ```

---

## Khái niệm: Cách Whisper hoạt động với Foundry Local

Mô hình OpenAI Whisper là một mô hình nhận dạng giọng nói tổng quát được huấn luyện trên một bộ dữ liệu lớn các âm thanh đa dạng. Khi chạy qua Foundry Local:

- Mô hình chạy **hoàn toàn trên CPU của bạn** - không yêu cầu GPU
- Âm thanh không bao giờ rời thiết bị của bạn - **bảo mật tuyệt đối**
- Foundry Local SDK xử lý việc tải xuống và quản lý bộ nhớ đệm mô hình
- **JavaScript và C#** cung cấp `AudioClient` tích hợp trong SDK, xử lý toàn bộ quy trình chuyển đổi — không cần thiết lập thủ công ONNX
- **Python** sử dụng SDK để quản lý mô hình và ONNX Runtime để suy luận trực tiếp trên các mô hình encoder/decoder ONNX

### Quy trình hoạt động (JavaScript và C#) — SDK AudioClient

1. **Foundry Local SDK** tải xuống và lưu bộ mô hình Whisper vào bộ nhớ đệm
2. `model.createAudioClient()` (JS) hoặc `model.GetAudioClientAsync()` (C#) tạo một `AudioClient`
3. `audioClient.transcribe(path)` (JS) hoặc `audioClient.TranscribeAudioAsync(path)` (C#) xử lý toàn bộ quy trình nội bộ — tiền xử lý âm thanh, encoder, decoder, và giải mã token
4. `AudioClient` có thuộc tính `settings.language` (đặt thành `"en"` cho tiếng Anh) để hướng dẫn chuyển đổi chính xác

### Quy trình hoạt động (Python) — ONNX Runtime

1. **Foundry Local SDK** tải xuống và lưu các tệp mô hình ONNX của Whisper vào bộ nhớ đệm
2. **Tiền xử lý âm thanh** chuyển đổi âm thanh WAV thành phổ mel (80 mel bins x 3000 khung hình)
3. **Encoder** xử lý phổ mel và tạo ra các trạng thái ẩn kèm các tensor khóa/giá trị chú ý chéo
4. **Decoder** chạy tự hồi quy, tạo ra từng token một cho đến khi tạo token kết thúc văn bản
5. **Tokeniser** giải mã các ID token đầu ra thành văn bản đọc được

### Các biến thể mô hình Whisper

| Bí danh | ID mô hình | Thiết bị | Kích thước | Mô tả |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Tăng tốc GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Tối ưu cho CPU (khuyến nghị với hầu hết thiết bị) |

> **Lưu ý:** Khác với các mô hình chat được liệt kê mặc định, các mô hình Whisper được phân loại dưới tác vụ `automatic-speech-recognition`. Dùng `foundry model info whisper-medium` để xem chi tiết.

---

## Bài tập thực hành trong phòng thí nghiệm

### Bài tập 0 - Lấy mẫu các tập tin âm thanh

Phòng thí nghiệm này bao gồm các tệp WAV được tạo sẵn dựa trên các kịch bản sản phẩm DIY của Zava. Tạo chúng bằng tập lệnh kèm theo:

```bash
# Từ thư mục gốc của repo - tạo và kích hoạt một .venv trước
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Điều này tạo ra sáu tệp WAV trong `samples/audio/`:

| Tệp | Kịch bản |
|------|----------|
| `zava-customer-inquiry.wav` | Khách hàng hỏi về **Khoan không dây Zava ProGrip** |
| `zava-product-review.wav` | Khách hàng đánh giá **Sơn nội thất Zava UltraSmooth** |
| `zava-support-call.wav` | Cuộc gọi hỗ trợ về **Tủ dụng cụ Zava TitanLock** |
| `zava-project-planning.wav` | Người làm DIY lên kế hoạch làm sàn boong với **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Hướng dẫn thiết lập hội thảo sử dụng **tất cả năm sản phẩm Zava** |
| `zava-full-project-walkthrough.wav` | Hướng dẫn sửa chữa gara kéo dài sử dụng **tất cả sản phẩm Zava** (~4 phút, dùng để kiểm tra âm thanh dài) |

> **Mẹo:** Bạn cũng có thể dùng tệp WAV/MP3/M4A của riêng bạn hoặc ghi âm bằng trình ghi âm giọng nói Windows.

---

### Bài tập 1 - Tải mô hình Whisper bằng SDK

Do CLI không tương thích với các mô hình Whisper trong các phiên bản Foundry Local mới hơn, hãy dùng **SDK** để tải xuống và tải mô hình. Chọn ngôn ngữ của bạn:

<details>
<summary><b>🐍 Python</b></summary>

**Cài đặt SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Bắt đầu dịch vụ
manager = FoundryLocalManager()
manager.start_service()

# Kiểm tra thông tin danh mục
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Kiểm tra xem đã được lưu trong bộ nhớ đệm chưa
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Tải mô hình vào bộ nhớ
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Lưu thành `download_whisper.py` và chạy:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Cài đặt SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Tạo quản lý và khởi động dịch vụ
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Lấy mô hình từ danh mục
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

// Tải mô hình vào bộ nhớ
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Lưu thành `download-whisper.mjs` và chạy:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Cài đặt SDK:**
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

> **Tại sao dùng SDK thay vì CLI?** CLI Foundry Local không hỗ trợ tải hoặc phục vụ các mô hình Whisper trực tiếp. SDK cung cấp cách đáng tin cậy để tải xuống và quản lý mô hình âm thanh theo lập trình. SDK JavaScript và C# bao gồm `AudioClient` tích hợp để xử lý toàn bộ quy trình chuyển đổi. Python dùng ONNX Runtime để suy luận trực tiếp trên các tệp mô hình đã lưu.

---

### Bài tập 2 - Hiểu SDK Whisper

Chuyển đổi giọng nói với Whisper sử dụng các phương pháp khác nhau tùy ngôn ngữ. **JavaScript và C#** cung cấp `AudioClient` tích hợp trong Foundry Local SDK xử lý toàn bộ quy trình (tiền xử lý âm thanh, encoder, decoder, giải mã token) trong một lần gọi phương thức. **Python** dùng Foundry Local SDK để quản lý mô hình và ONNX Runtime để suy luận trực tiếp trên các mô hình encoder/decoder ONNX.

| Thành phần | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **Gói SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Quản lý mô hình** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **Trích xuất đặc trưng** | `WhisperFeatureExtractor` + `librosa` | Được xử lý bởi SDK `AudioClient` | Được xử lý bởi SDK `AudioClient` |
| **Suy luận** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Giải mã token** | `WhisperTokenizer` | Được xử lý bởi SDK `AudioClient` | Được xử lý bởi SDK `AudioClient` |
| **Cài đặt ngôn ngữ** | Đặt qua `forced_ids` trong token decoder | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Đầu vào** | Đường dẫn tập tin WAV | Đường dẫn tập tin WAV | Đường dẫn tập tin WAV |
| **Đầu ra** | Chuỗi văn bản đã giải mã | `result.text` | `result.Text` |

> **Quan trọng:** Luôn đặt ngôn ngữ trên `AudioClient` (ví dụ `"en"` cho tiếng Anh). Nếu không có cài đặt ngôn ngữ rõ ràng, mô hình có thể tạo ra kết quả lộn xộn khi cố gắng tự động nhận diện ngôn ngữ.

> **Mẫu SDK:** Python dùng `FoundryLocalManager(alias)` để khởi động, sau đó `get_cache_location()` để tìm đường dẫn tệp mô hình ONNX. JavaScript và C# dùng `AudioClient` tích hợp sẵn của SDK — lấy qua `model.createAudioClient()` (JS) hoặc `model.GetAudioClientAsync()` (C#) — xử lý toàn bộ quy trình chuyển đổi. Xem thêm [Phần 2: Tìm hiểu sâu về Foundry Local SDK](part2-foundry-local-sdk.md) để biết chi tiết đầy đủ.

---

### Bài tập 3 - Tạo ứng dụng chuyển đổi đơn giản

Chọn ngôn ngữ lập trình của bạn và xây dựng ứng dụng tối giản để chuyển đổi tệp âm thanh thành văn bản.

> **Định dạng âm thanh được hỗ trợ:** WAV, MP3, M4A. Để có kết quả tốt nhất, dùng tệp WAV với tần số lấy mẫu 16kHz.

<details>
<summary><h3>Ngôn ngữ Python</h3></summary>

#### Cài đặt

```bash
cd python
python -m venv venv

# Kích hoạt môi trường ảo:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Code chuyển đổi

Tạo tệp `foundry-local-whisper.py`:

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

# Bước 1: Khởi động - bắt đầu dịch vụ, tải về và nạp mô hình
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Xây dựng đường dẫn đến các tập tin mô hình ONNX đã được lưu đệm
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Bước 2: Nạp các phiên ONNX và bộ trích xuất đặc trưng
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

# Bước 3: Trích xuất đặc trưng phổ mel
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Bước 4: Chạy bộ mã hóa
enc_out = encoder.run(None, {"audio_features": input_features})
# Đầu ra đầu tiên là các trạng thái ẩn; các đầu ra còn lại là cặp KV chú ý chéo
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Bước 5: Giải mã tự hồi quy
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, phiên âm, không dấu thời gian
input_ids = np.array([initial_tokens], dtype=np.int32)

# Làm trống bộ nhớ đệm KV chú ý tự thân
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

    if next_token == 50257:  # kết thúc văn bản
        break
    generated.append(next_token)

    # Cập nhật bộ nhớ đệm KV chú ý tự thân
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Chạy chương trình

```bash
# Chép lại một kịch bản sản phẩm của Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Hoặc thử những cái khác:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Các điểm chính của Python

| Phương thức | Mục đích |
|--------|---------|
| `FoundryLocalManager(alias)` | Khởi động: bắt đầu dịch vụ, tải xuống và tải mô hình |
| `manager.get_cache_location()` | Lấy đường dẫn các tệp mô hình ONNX được lưu |
| `WhisperFeatureExtractor.from_pretrained()` | Tải bộ trích xuất đặc trưng phổ mel |
| `ort.InferenceSession()` | Tạo phiên chạy ONNX Runtime cho encoder và decoder |
| `tokenizer.decode()` | Chuyển mã ID token đầu ra thành văn bản |

</details>

<details>
<summary><h3>Ngôn ngữ JavaScript</h3></summary>

#### Cài đặt

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Code chuyển đổi

Tạo tệp `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Bước 1: Khởi tạo - tạo bộ quản lý, khởi động dịch vụ và tải mô hình
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

// Bước 2: Tạo một khách hàng âm thanh và chuyển đổi văn bản
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Dọn dẹp
await model.unload();
```

> **Lưu ý:** Foundry Local SDK cung cấp `AudioClient` tích hợp qua `model.createAudioClient()` xử lý toàn bộ pipeline suy luận ONNX bên trong — không cần nhập `onnxruntime-node`. Luôn đặt `audioClient.settings.language = "en"` để đảm bảo chuyển đổi tiếng Anh chính xác.

#### Chạy chương trình

```bash
# Chép lại một kịch bản sản phẩm của Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Hoặc thử các kịch bản khác:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Các điểm chính của JavaScript

| Phương thức | Mục đích |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | Tạo singleton quản lý |
| `await catalog.getModel(alias)` | Lấy mô hình từ danh mục |
| `model.download()` / `model.load()` | Tải xuống và tải mô hình Whisper |
| `model.createAudioClient()` | Tạo client âm thanh để chuyển đổi |
| `audioClient.settings.language = "en"` | Đặt ngôn ngữ chuyển đổi (bắt buộc để kết quả chính xác) |
| `audioClient.transcribe(path)` | Chuyển đổi tệp âm thanh, trả về `{ text, duration }` |

</details>

<details>
<summary><h3>Ngôn ngữ C#</h3></summary>

#### Cài đặt

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Lưu ý:** Ngôn ngữ C# dùng gói `Microsoft.AI.Foundry.Local` cung cấp `AudioClient` tích hợp qua `model.GetAudioClientAsync()`. Điều này xử lý toàn bộ pipeline chuyển đổi trong quy trình — không cần cài đặt ONNX Runtime riêng biệt.

#### Code chuyển đổi

Thay thế nội dung tệp `Program.cs`:

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

#### Chạy chương trình

```bash
# Phiên âm một kịch bản sản phẩm Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Hoặc thử các kịch bản khác:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Các điểm chính của C#

| Phương thức | Mục đích |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Khởi tạo Foundry Local với cấu hình |
| `catalog.GetModelAsync(alias)` | Lấy mô hình từ catalog |
| `model.DownloadAsync()` | Tải mô hình Whisper |
| `model.GetAudioClientAsync()` | Lấy AudioClient (không phải ChatClient!) |
| `audioClient.Settings.Language = "en"` | Đặt ngôn ngữ chuyển đổi (bắt buộc để kết quả chính xác) |
| `audioClient.TranscribeAudioAsync(path)` | Chuyển đổi tệp âm thanh |
| `result.Text` | Văn bản được chuyển đổi |


> **C# vs Python/JS:** SDK C# cung cấp `AudioClient` tích hợp sẵn cho việc chuyển đổi giọng nói thành văn bản trong cùng tiến trình thông qua `model.GetAudioClientAsync()`, tương tự như SDK JavaScript. Python sử dụng ONNX Runtime trực tiếp để suy luận trên các mô hình mã hóa/giải mã được lưu trong bộ nhớ đệm.

</details>

---

### Bài tập 4 - Chuyển đổi hàng loạt tất cả các mẫu Zava

Bây giờ bạn đã có một ứng dụng chuyển đổi giọng nói thành văn bản hoạt động, hãy chuyển đổi tất cả năm tệp mẫu Zava và so sánh kết quả.

<details>
<summary><h3>Python Track</h3></summary>

Mẫu đầy đủ `python/foundry-local-whisper.py` đã hỗ trợ chuyển đổi hàng loạt. Khi chạy mà không có tham số, nó sẽ chuyển đổi tất cả các tệp `zava-*.wav` trong `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Mẫu sử dụng `FoundryLocalManager(alias)` để khởi tạo, sau đó chạy các phiên ONNX mã hóa và giải mã cho mỗi tệp.

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

Mẫu đầy đủ `javascript/foundry-local-whisper.mjs` đã hỗ trợ chuyển đổi hàng loạt. Khi chạy mà không có tham số, nó sẽ chuyển đổi tất cả các tệp `zava-*.wav` trong `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Mẫu sử dụng `FoundryLocalManager.create()` và `catalog.getModel(alias)` để khởi tạo SDK, sau đó sử dụng `AudioClient` (với `settings.language = "en"`) để chuyển đổi mỗi tệp.

</details>

<details>
<summary><h3>C# Track</h3></summary>

Mẫu đầy đủ `csharp/WhisperTranscription.cs` đã hỗ trợ chuyển đổi hàng loạt. Khi chạy mà không có tham số tệp cụ thể, nó sẽ chuyển đổi tất cả các tệp `zava-*.wav` trong `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Mẫu sử dụng `FoundryLocalManager.CreateAsync()` và `AudioClient` của SDK (với `Settings.Language = "en"`) cho việc chuyển đổi trong cùng tiến trình.

</details>

**Điều cần chú ý:** So sánh đầu ra chuyển đổi với văn bản gốc trong `samples/audio/generate_samples.py`. Whisper ghi lại tên sản phẩm như "Zava ProGrip" và các thuật ngữ kỹ thuật như "brushless motor" hoặc "composite decking" chính xác đến mức nào?

---

### Bài tập 5 - Hiểu các mẫu mã chính

Nghiên cứu cách chuyển đổi giọng nói bằng Whisper khác với chat completions ở cả ba ngôn ngữ:

<details>
<summary><b>Python - Khác biệt chính so với Chat</b></summary>

```python
# Hoàn thành chat (Các phần 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Chuyển đổi âm thanh thành văn bản (Phần này):
# Sử dụng ONNX Runtime trực tiếp thay vì client OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... vòng giải mã tự hồi quy ...
print(tokenizer.decode(generated_tokens))
```

**Thông tin chính:** Mô hình chat sử dụng API tương thích OpenAI qua `manager.endpoint`. Whisper sử dụng SDK để định vị các tệp mô hình ONNX lưu trong bộ nhớ đệm, sau đó chạy suy luận trực tiếp với ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Khác biệt chính so với Chat</b></summary>

```javascript
// Hoàn thành trò chuyện (Phần 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Phiên âm âm thanh (Phần này):
// Sử dụng AudioClient tích hợp sẵn trong SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Luôn đặt ngôn ngữ để có kết quả tốt nhất
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Thông tin chính:** Mô hình chat sử dụng API tương thích OpenAI qua `manager.urls[0] + "/v1"`. Chuyển đổi Whisper dùng `AudioClient` của SDK, lấy từ `model.createAudioClient()`. Cài đặt `settings.language` để tránh đầu ra lỗi do tự động phát hiện.

</details>

<details>
<summary><b>C# - Khác biệt chính so với Chat</b></summary>

Phương pháp C# dùng `AudioClient` tích hợp sẵn của SDK cho việc chuyển đổi trong cùng tiến trình:

**Khởi tạo mô hình:**

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

**Chuyển đổi:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Thông tin chính:** C# sử dụng `FoundryLocalManager.CreateAsync()` và lấy trực tiếp `AudioClient` — không cần thiết lập ONNX Runtime. Cài đặt `Settings.Language` để tránh đầu ra lỗi do tự động phát hiện.

</details>

> **Tóm lược:** Python sử dụng Foundry Local SDK để quản lý mô hình và ONNX Runtime để suy luận trực tiếp với các mô hình encoder/decoder. JavaScript và C# đều sử dụng `AudioClient` tích hợp sẵn của SDK cho chuyển đổi mượt mà — tạo client, thiết lập ngôn ngữ, và gọi `transcribe()` / `TranscribeAudioAsync()`. Luôn luôn thiết lập thuộc tính ngôn ngữ trên AudioClient để có kết quả chính xác.

---

### Bài tập 6 - Thử nghiệm

Thử các thay đổi sau để hiểu rõ hơn:

1. **Thử các tệp âm thanh khác nhau** - ghi âm giọng nói của bạn bằng Windows Voice Recorder, lưu dưới định dạng WAV và chuyển đổi nó

2. **So sánh các biến thể mô hình** - nếu bạn có GPU NVIDIA, thử biến thể CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   So sánh tốc độ chuyển đổi với biến thể CPU.

3. **Thêm định dạng đầu ra** - phản hồi JSON có thể bao gồm:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Xây dựng REST API** - bao bọc mã chuyển đổi của bạn trong một web server:

   | Ngôn ngữ | Framework | Ví dụ |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` với `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` với `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` với `IFormFile` |

5. **Đa lượt với chuyển đổi giọng nói** - kết hợp Whisper với agent chat trong Phần 4: chuyển đổi âm thanh trước, rồi chuyển văn bản cho agent để phân tích hoặc tóm tắt.

---

## Tham khảo API Audio SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — tạo một đối tượng `AudioClient`
> - `audioClient.settings.language` — đặt ngôn ngữ chuyển đổi (ví dụ `"en"`)
> - `audioClient.settings.temperature` — điều khiển mức độ ngẫu nhiên (tuỳ chọn)
> - `audioClient.transcribe(filePath)` — chuyển đổi một tệp, trả về `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — truyền dòng các đoạn chuyển đổi qua callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — tạo một đối tượng `OpenAIAudioClient`
> - `audioClient.Settings.Language` — đặt ngôn ngữ chuyển đổi (ví dụ `"en"`)
> - `audioClient.Settings.Temperature` — điều khiển mức độ ngẫu nhiên (tuỳ chọn)
> - `await audioClient.TranscribeAudioAsync(filePath)` — chuyển đổi tệp, trả về đối tượng có `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — trả về `IAsyncEnumerable` các đoạn chuyển đổi

> **Mẹo:** Luôn đặt thuộc tính ngôn ngữ trước khi chuyển đổi. Nếu không, mô hình Whisper cố tự động nhận diện, có thể tạo ra đầu ra lỗi (ký tự thay thế duy nhất thay vì văn bản).

---

## So sánh: Mô hình Chat và Whisper

| Khía cạnh | Mô hình Chat (Phần 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Loại tác vụ** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Đầu vào** | Tin nhắn văn bản (JSON) | Tệp âm thanh (WAV/MP3/M4A) | Tệp âm thanh (WAV/MP3/M4A) |
| **Đầu ra** | Văn bản sinh ra (truyền dòng) | Văn bản chuyển đổi (hoàn chỉnh) | Văn bản chuyển đổi (hoàn chỉnh) |
| **Gói SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Phương thức API** | `client.chat.completions.create()` | ONNX Runtime trực tiếp | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Cài đặt ngôn ngữ** | Không áp dụng | Tham số prompt giải mã | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Hỗ trợ streaming** | Có | Không | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Lợi ích về bảo mật** | Mã/dữ liệu ở cục bộ | Dữ liệu âm thanh ở cục bộ | Dữ liệu âm thanh ở cục bộ |

---

## Những điểm cần nhớ

| Khái niệm | Điều bạn học được |
|---------|-----------------|
| **Whisper chạy trên thiết bị** | Nhận dạng giọng nói thành văn bản chạy hoàn toàn cục bộ, lý tưởng cho việc chuyển đổi cuộc gọi khách hàng Zava và đánh giá sản phẩm trên thiết bị |
| **SDK AudioClient** | SDK JavaScript và C# cung cấp `AudioClient` tích hợp xử lý toàn bộ pipeline chuyển đổi trong một lệnh gọi |
| **Cài đặt ngôn ngữ** | Luôn đặt ngôn ngữ cho AudioClient (ví dụ `"en"`) — nếu không, tự động nhận diện có thể tạo ra đầu ra lỗi |
| **Python** | Dùng `foundry-local-sdk` để quản lý mô hình + `onnxruntime` + `transformers` + `librosa` để suy luận ONNX trực tiếp |
| **JavaScript** | Dùng `foundry-local-sdk` với `model.createAudioClient()` — thiết lập `settings.language`, rồi gọi `transcribe()` |
| **C#** | Dùng `Microsoft.AI.Foundry.Local` với `model.GetAudioClientAsync()` — thiết lập `Settings.Language`, rồi gọi `TranscribeAudioAsync()` |
| **Hỗ trợ streaming** | SDK JS và C# cũng cung cấp `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` cho đầu ra từng đoạn |
| **Tối ưu CPU** | Phiên bản CPU (3.05 GB) chạy trên mọi thiết bị Windows không cần GPU |
| **Bảo mật ưu tiên** | Hoàn hảo để giữ tương tác khách hàng Zava và dữ liệu sản phẩm độc quyền trên thiết bị |

---

## Tài nguyên

| Tài nguyên | Liên kết |
|----------|------|
| Tài liệu Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Tham khảo SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Mô hình OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Trang web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Bước tiếp theo

Tiếp tục tới [Phần 10: Sử dụng Mô hình Tùy chỉnh hoặc Hugging Face](part10-custom-models.md) để biên dịch các mô hình của bạn từ Hugging Face và chạy chúng qua Foundry Local.