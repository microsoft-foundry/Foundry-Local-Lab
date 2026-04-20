![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第9部分：使用 Whisper 和 Foundry Local 進行語音轉錄

> **目標：** 使用透過 Foundry Local 本地運行的 OpenAI Whisper 模型轉錄音頻文件 — 完全在裝置上運行，無需雲端。

## 概覽

Foundry Local 不僅支援文本生成；它還支援 <strong>語音轉文字</strong> 模型。在此實驗中，您將使用 **OpenAI Whisper Medium** 模型全程在您的機器上轉錄音頻檔。這非常適合比如轉錄 Zava 客戶服務電話、產品評論錄音或工作坊規劃會議等音頻資料絕不能離開裝置的場景。


---

## 學習目標

完成此實驗後，您將能：

- 理解 Whisper 語音轉文字模型及其功能
- 使用 Foundry Local 下載並運行 Whisper 模型
- 使用 Foundry Local SDK 透過 Python、JavaScript 與 C# 轉錄音頻檔案
- 構建完全在裝置上運行的簡單轉錄服務
- 理解 Foundry Local 中聊天/文字模型和音頻模型的區別

---

## 先決條件

| 要求 | 詳情 |
|-------------|---------|
| **Foundry Local CLI** | 版本 **0.8.101 或以上**（Whisper 模型從 v0.8.101 開始提供） |
| <strong>作業系統</strong> | Windows 10/11 (x64 或 ARM64) |
| <strong>語言執行環境</strong> | **Python 3.9+** 和/或 **Node.js 18+** 和/或 **.NET 9 SDK** ([下載 .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| <strong>已完成</strong> | [第1部分：入門](part1-getting-started.md)、[第2部分：Foundry Local SDK 深入探討](part2-foundry-local-sdk.md)、[第3部分：SDK與API](part3-sdk-and-apis.md) |

> **注意：** Whisper 模型必須通過 **SDK** 下載（而非 CLI）。CLI 不支援音頻轉錄端點。請使用以下命令檢查您的版本：
> ```bash
> foundry --version
> ```

---

## 概念：Whisper 如何與 Foundry Local 配合運作

OpenAI Whisper 模型是一個通用語音辨識模型，透過大量多樣化音頻資料訓練。透過 Foundry Local 運行時：

- 模型 **全部在您的 CPU 上運行** — 無需 GPU
- 音頻資料絕不離開您的裝置 — <strong>完全隱私保護</strong>
- Foundry Local SDK 負責模型下載及快取管理
- **JavaScript 與 C#** SDK 內建 `AudioClient`，處理完整轉錄管線 — 無需手動 ONNX 設定
- **Python** 則使用 SDK 管理模型，並使用 ONNX Runtime 對編碼器/解碼器 ONNX 模型進行直接推理

### 管線運作方式（JavaScript 和 C#）— SDK AudioClient

1. **Foundry Local SDK** 下載並快取 Whisper 模型
2. `model.createAudioClient()`（JS）或 `model.GetAudioClientAsync()`（C#）建立一個 `AudioClient`
3. `audioClient.transcribe(path)`（JS）或 `audioClient.TranscribeAudioAsync(path)`（C#）內部處理完整流程 — 音頻前處理、編碼器、解碼器及標記解碼
4. `AudioClient` 提供 `settings.language` 屬性（設為 `"en"` 為英文）以輔助準確轉錄

### 管線運作方式（Python）— ONNX Runtime

1. **Foundry Local SDK** 下載並快取 Whisper ONNX 模型檔案
2. <strong>音頻前處理</strong> 將 WAV 音頻轉成 mel 頻譜圖（80 個 mel 頻點 × 3000 幀）
3. <strong>編碼器</strong> 處理 mel 頻譜圖，產生隱藏狀態與交叉注意力鍵/值張量
4. <strong>解碼器</strong> 以自回歸方式一次生成一個標記，直到產生結束文本標記
5. <strong>標記器</strong> 將輸出的標記 ID 解碼回可讀文本

### Whisper 模型變體

| 別名 | 模型 ID | 裝置 | 大小 | 描述 |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU 加速 (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU 優化（大多數裝置推薦） |

> **注意：** 與默認列出的聊天模型不同，Whisper 模型歸類於 `automatic-speech-recognition` 任務。使用 `foundry model info whisper-medium` 查看詳細資訊。

---

## 實驗練習

### 練習 0 - 取得範例音頻檔案

此實驗附帶基於 Zava DIY 產品情境預製的 WAV 檔，透過內建腳本產生：

```bash
# 從儲存庫根目錄開始 - 首先建立並啟動一個 .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

此腳本會在 `samples/audio/` 產生六個 WAV 檔：

| 檔案 | 情境 |
|------|----------|
| `zava-customer-inquiry.wav` | 顧客詢問 **Zava ProGrip 無繩電鑽** |
| `zava-product-review.wav` | 顧客評論 **Zava UltraSmooth 室內漆** |
| `zava-support-call.wav` | 關於 **Zava TitanLock 工具箱** 的客服通話 |
| `zava-project-planning.wav` | DIY 使用 **Zava EcoBoard 複合甲板材** 計畫露臺 |
| `zava-workshop-setup.wav` | 使用 **所有五種 Zava 產品** 的工作坊導覽 |
| `zava-full-project-walkthrough.wav` | 長達約4分鐘的車庫整修全程導覽（長音頻測試）|

> **提示：** 您也可以使用自有的 WAV/MP3/M4A 檔，或用 Windows 聲音錄音機錄製。

---

### 練習 1 - 使用 SDK 下載 Whisper 模型

因新版 Foundry Local 中 CLI 與 Whisper 模型不兼容，請使用 **SDK** 下載並載入模型。視您使用語言選擇對應操作：

<details>
<summary><b>🐍 Python</b></summary>

**安裝 SDK：**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# 開始服務
manager = FoundryLocalManager()
manager.start_service()

# 檢查目錄資訊
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# 檢查是否已快取
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# 將模型載入記憶體
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

儲存為 `download_whisper.py` 並執行：
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**安裝 SDK：**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// 建立管理員並啟動服務
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 從目錄獲取模型
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

// 將模型載入記憶體
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

儲存為 `download-whisper.mjs` 並執行：
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**安裝 SDK：**
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

> **為何使用 SDK 而非 CLI？** Foundry Local CLI 不支援直接下載或提供 Whisper 模型。SDK 以程式化方式可靠地下載並管理音頻模型。JavaScript 與 C# SDK 內建 `AudioClient`，處理整個轉錄流程。Python 則使用 ONNX Runtime 直接對快取模型檔案推理。

---

### 練習 2 - 了解 Whisper SDK

Whisper 轉錄在不同語言中使用的方法不同。**JavaScript 和 C#** 的 Foundry Local SDK 內建 `AudioClient`，單一方法呼叫即可覆蓋完整處理流程 (音頻前處理、編碼器、解碼器、標記解碼)。**Python** 使用 Foundry Local SDK 進行模型管理，並用 ONNX Runtime 直接對編碼器/解碼器 ONNX 模型推理。

| 元件 | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK 套件** | `foundry-local-sdk`、`onnxruntime`、`transformers`、`librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| <strong>模型管理</strong> | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| <strong>特徵擷取</strong> | `WhisperFeatureExtractor` + `librosa` | 由 SDK `AudioClient` 處理 | 由 SDK `AudioClient` 處理 |
| <strong>推理</strong> | `ort.InferenceSession`（編碼器 + 解碼器） | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| <strong>標記解碼</strong> | `WhisperTokenizer` | 由 SDK `AudioClient` 處理 | 由 SDK `AudioClient` 處理 |
| <strong>語言設定</strong> | 透過解碼器標記 `forced_ids` 設定 | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| <strong>輸入</strong> | WAV 檔案路徑 | WAV 檔案路徑 | WAV 檔案路徑 |
| <strong>輸出</strong> | 解碼後文本字串 | `result.text` | `result.Text` |

> **重要：** 請務必在 `AudioClient` 上設定語言(例如 `"en"` 為英語)。若未明確設定語言，模型可能嘗試自動偵測導致輸出錯亂。

> **SDK 範式：** Python 透過 `FoundryLocalManager(alias)` 啟動，然後用 `get_cache_location()` 尋找 ONNX 模型檔。JavaScript 和 C# 用 SDK 內建的 `AudioClient` — 透過 `model.createAudioClient()` (JS) 或 `model.GetAudioClientAsync()` (C#) 取得 — 處理整條轉錄流程。詳情見 [第2部分：Foundry Local SDK 深入探討](part2-foundry-local-sdk.md)。

---

### 練習 3 - 建立簡易轉錄應用程式

選擇您的語言流程，構建一個簡易應用來轉錄音頻檔案。

> **支援音頻格式：** WAV、MP3、M4A。建議使用 16kHz 取樣率 WAV 檔以獲最佳效果。

<details>
<summary><h3>Python 軌道</h3></summary>

#### 環境設定

```bash
cd python
python -m venv venv

# 啟動虛擬環境：
# Windows（PowerShell）：
venv\Scripts\Activate.ps1
# macOS：
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### 轉錄程式碼

創建檔案 `foundry-local-whisper.py`：

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

# 第一步：引導 - 啟動服務，下載及載入模型
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# 建立緩存 ONNX 模型檔案的路徑
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# 第二步：載入 ONNX 會話及特徵提取器
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

# 第三步：提取梅爾頻譜圖特徵
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# 第四步：運行編碼器
enc_out = encoder.run(None, {"audio_features": input_features})
# 第一個輸出是隱藏狀態；餘下的是交叉注意力 KV 對
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# 第五步：自回歸解碼
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, 轉錄, 無時間戳
input_ids = np.array([initial_tokens], dtype=np.int32)

# 清空自注意力 KV 緩存
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

    if next_token == 50257:  # 文字結束
        break
    generated.append(next_token)

    # 更新自注意力 KV 緩存
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### 執行程式

```bash
# 轉錄一個 Zava 產品場景
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# 或者試試其他：
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Python 重要重點

| 方法 | 功用 |
|--------|---------|
| `FoundryLocalManager(alias)` | 啟動服務，下載及載入模型 |
| `manager.get_cache_location()` | 取得快取的 ONNX 模型檔路徑 |
| `WhisperFeatureExtractor.from_pretrained()` | 載入 mel 頻譜特徵擷取器 |
| `ort.InferenceSession()` | 建立編碼器與解碼器的 ONNX Runtime 執行會話 |
| `tokenizer.decode()` | 將輸出標記 ID 解碼為文字 |

</details>

<details>
<summary><h3>JavaScript 軌道</h3></summary>

#### 環境設定

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### 轉錄程式碼

建立檔案 `foundry-local-whisper.mjs`：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// 第一步：引導 - 創建管理器，啟動服務，並加載模型
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

// 第二步：創建音訊客戶端並進行轉錄
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// 清理
await model.unload();
```

> **注意：** Foundry Local SDK 透過 `model.createAudioClient()` 提供內建 `AudioClient`，內部自動處理整套 ONNX 推理管線 — 無需額外引入 `onnxruntime-node`。請務必設定 `audioClient.settings.language = "en"`，確保英語轉錄準確。

#### 執行程式

```bash
# 書寫一個Zava產品情境
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# 或嘗試其他：
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### JavaScript 重要重點

| 方法 | 功用 |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | 建立管理單例 |
| `await catalog.getModel(alias)` | 從目錄取得模型 |
| `model.download()` / `model.load()` | 下載並載入 Whisper 模型 |
| `model.createAudioClient()` | 建立音頻用戶端進行轉錄 |
| `audioClient.settings.language = "en"` | 設定轉錄語言（確保輸出準確） |
| `audioClient.transcribe(path)` | 轉錄音頻檔，回傳 `{ text, duration }` |

</details>

<details>
<summary><h3>C# 軌道</h3></summary>

#### 環境設定

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **注意：** C# 軌道使用 `Microsoft.AI.Foundry.Local` 套件，透過 `model.GetAudioClientAsync()` 提供內建 `AudioClient`，全程於進程內處理轉錄管線 — 無需額外 ONNX Runtime 設定。

#### 轉錄程式碼

替換 `Program.cs` 內容：

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

#### 執行程式

```bash
# 把 Zava 產品情景轉錄
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# 或試試其他：
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### C# 重要重點

| 方法 | 功用 |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | 初始化 Foundry Local 並設定配置 |
| `catalog.GetModelAsync(alias)` | 從目錄取得模型 |
| `model.DownloadAsync()` | 下載 Whisper 模型 |
| `model.GetAudioClientAsync()` | 取得 AudioClient（非 ChatClient） |
| `audioClient.Settings.Language = "en"` | 設定轉錄語言（確保輸出準確） |
| `audioClient.TranscribeAudioAsync(path)` | 轉錄音頻檔案 |
| `result.Text` | 轉錄出的文本 |


> **C# vs Python/JS:** C# SDK 提供內建的 `AudioClient` 以便透過 `model.GetAudioClientAsync()` 進行程序內轉錄，類似 JavaScript SDK。Python 直接使用 ONNX Runtime 對快取的編碼器/解碼器模型執行推論。

</details>

---

### 練習 4 - 批次轉錄所有 Zava 範例檔案

既然你已經有一個可用的轉錄應用程式，請轉錄全部五個 Zava 範例檔案並比較結果。

<details>
<summary><h3>Python 路線</h3></summary>

完整範例 `python/foundry-local-whisper.py` 已經支援批次轉錄。若不帶參數執行，則會轉錄 `samples/audio/` 中所有符合 `zava-*.wav` 的檔案：

```bash
cd python
python foundry-local-whisper.py
```

該範例使用 `FoundryLocalManager(alias)` 啟動，然後對每個檔案執行編碼器和解碼器的 ONNX 會話。

</details>

<details>
<summary><h3>JavaScript 路線</h3></summary>

完整範例 `javascript/foundry-local-whisper.mjs` 已經支援批次轉錄。若不帶參數執行，則會轉錄 `samples/audio/` 中所有符合 `zava-*.wav` 的檔案：

```bash
cd javascript
node foundry-local-whisper.mjs
```

該範例使用 `FoundryLocalManager.create()` 和 `catalog.getModel(alias)` 來初始化 SDK，然後使用帶有 `settings.language = "en"` 的 `AudioClient` 轉錄每個檔案。

</details>

<details>
<summary><h3>C# 路線</h3></summary>

完整範例 `csharp/WhisperTranscription.cs` 已經支援批次轉錄。若不帶指定檔案參數執行，則會轉錄 `samples/audio/` 中所有符合 `zava-*.wav` 的檔案：

```bash
cd csharp
dotnet run whisper
```

該範例使用 `FoundryLocalManager.CreateAsync()` 和 SDK 內建帶有 `Settings.Language = "en"` 的 `AudioClient` 來進行程序內轉錄。

</details>

**請注意：** 將轉錄結果與 `samples/audio/generate_samples.py` 中的原始文字比較。Whisper 對於產品名稱如「Zava ProGrip」以及技術名詞如「brushless motor」或「composite decking」的捕捉有多精準？

---

### 練習 5 - 了解主要程式碼模式

研究 Whisper 轉錄如何與三種語言中的聊天完成不同：

<details>
<summary><b>Python - 與聊天的主要差異</b></summary>

```python
# 聊天完成（第2至6部分）：
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# 音頻轉錄（本部分）：
# 直接使用ONNX Runtime，而非OpenAI客戶端
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... 自回歸解碼器循環 ...
print(tokenizer.decode(generated_tokens))
```

**關鍵洞察：** 聊天模型使用透過 `manager.endpoint` 的 OpenAI 相容 API。Whisper 使用 SDK 定位快取的 ONNX 模型檔案，然後直接用 ONNX Runtime 執行推論。

</details>

<details>
<summary><b>JavaScript - 與聊天的主要差異</b></summary>

```javascript
// 聊天完成（第2至6部分）：
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// 音頻轉錄（本部分）：
// 使用 SDK 內置的 AudioClient
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // 始終設置語言以獲得最佳效果
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**關鍵洞察：** 聊天模型使用透過 `manager.urls[0] + "/v1"` 的 OpenAI 相容 API。Whisper 轉錄使用 SDK 的 `AudioClient`，由 `model.createAudioClient()` 取用。設定 `settings.language` 以避免自動偵測時產生亂碼。

</details>

<details>
<summary><b>C# - 與聊天的主要差異</b></summary>

C# 方式使用 SDK 內建的 `AudioClient` 進行程序內轉錄：

**模型初始化：**

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

**轉錄：**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**關鍵洞察：** C# 使用 `FoundryLocalManager.CreateAsync()` 並直接取得 `AudioClient` — 不需要 ONNX Runtime 設定。設定 `Settings.Language` 以避免自動偵測造成亂碼。

</details>

> **總結：** Python 使用 Foundry Local SDK 管理模型，並用 ONNX Runtime 對編碼器/解碼器模型直接推論。JavaScript 與 C# 則都使用 SDK 內建的 `AudioClient` 以簡化流程 — 建立客戶端、設定語言，然後呼叫 `transcribe()` / `TranscribeAudioAsync()`。務必在 AudioClient 上設定語言屬性以確保結果準確。

---

### 練習 6 - 實驗

嘗試以下修改以加深理解：

1. <strong>試試不同音訊檔案</strong> - 使用 Windows 語音錄音機錄音，儲存為 WAV，然後轉錄。

2. <strong>比較模型版本</strong> - 如果你有 NVIDIA GPU，試試 CUDA 版本：
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   並將轉錄速度與 CPU 版本比較。

3. <strong>加入輸出格式</strong> - JSON 回應可包含：
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```


4. **建立 REST API** - 將轉錄程式碼包裝為網頁伺服器：

   | 語言 | 框架 | 範例 |
   |----------|-----------|--------|
   | Python | FastAPI | 使用 `@app.post("/v1/audio/transcriptions")` 搭配 `UploadFile` |
   | JavaScript | Express.js | 使用 `app.post("/v1/audio/transcriptions")` 搭配 `multer` |
   | C# | ASP.NET Minimal API | 使用 `app.MapPost("/v1/audio/transcriptions")` 搭配 `IFormFile` |

5. <strong>結合多輪對話與轉錄</strong> - 將 Whisper 與第 4 部分的聊天代理結合：先轉錄音訊，再將文字傳給代理進行分析或摘要。

---

## SDK Audio API 參考

> **JavaScript AudioClient：**
> - `model.createAudioClient()` — 建立 `AudioClient` 實例
> - `audioClient.settings.language` — 設定轉錄語言（例如 `"en"`）
> - `audioClient.settings.temperature` — 控制隨機性（可選）
> - `audioClient.transcribe(filePath)` — 轉錄檔案，回傳 `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — 透過回呼串流輸出轉錄片段
>
> **C# AudioClient：**
> - `await model.GetAudioClientAsync()` — 建立 `OpenAIAudioClient` 實例
> - `audioClient.Settings.Language` — 設定轉錄語言（例如 `"en"`）
> - `audioClient.Settings.Temperature` — 控制隨機性（可選）
> - `await audioClient.TranscribeAudioAsync(filePath)` — 轉錄檔案，回傳含 `.Text` 的物件
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — 回傳 `IAsyncEnumerable` 的轉錄片段

> **提示：** 請務必在轉錄前設定語言屬性。若未設定，Whisper 模型會嘗試自動偵測，這可能導致亂碼輸出（僅顯示單一替代字元，無文字）。

---

## 比較：聊天模型 vs. Whisper

| 項目 | 聊天模型（第 3-7 部分） | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| <strong>任務類型</strong> | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| <strong>輸入</strong> | 文字訊息（JSON） | 音訊檔案（WAV/MP3/M4A） | 音訊檔案（WAV/MP3/M4A） |
| <strong>輸出</strong> | 產生文字（串流） | 轉錄文字（完整） | 轉錄文字（完整） |
| **SDK 套件** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk`（JS） / `Microsoft.AI.Foundry.Local`（C#） |
| **API 方法** | `client.chat.completions.create()` | 直接使用 ONNX Runtime | `audioClient.transcribe()`（JS） / `audioClient.TranscribeAudioAsync()`（C#） |
| <strong>語言設定</strong> | 無 | 解碼器提示標記 | `audioClient.settings.language`（JS） / `audioClient.Settings.Language`（C#） |
| <strong>串流支援</strong> | 有 | 無 | `transcribeStreaming()`（JS） / `TranscribeAudioStreamingAsync()`（C#） |
| <strong>隱私優勢</strong> | 程式碼/資料維持在本機 | 音訊資料維持在本機 | 音訊資料維持在本機 |

---

## 主要重點

| 概念 | 你所學到的 |
|---------|------------|
| **Whisper 本地運行** | 語音轉文字完全在本地進行，完美適合在裝置上轉錄 Zava 客戶電話與產品評論 |
| **SDK AudioClient** | JavaScript 與 C# SDK 提供內建的 `AudioClient`，單次呼叫可完整處理轉錄流程 |
| <strong>語言設定</strong> | 請務必設定 AudioClient 語言（例如 `"en"`）— 未設定會導致自動偵測產生亂碼 |
| **Python** | 使用 `foundry-local-sdk` 管理模型 + `onnxruntime` + `transformers` + `librosa` 以直接 ONNX 推論 |
| **JavaScript** | 使用 `foundry-local-sdk` with `model.createAudioClient()` — 設定 `settings.language`，然後調用 `transcribe()` |
| **C#** | 使用 `Microsoft.AI.Foundry.Local` with `model.GetAudioClientAsync()` — 設定 `Settings.Language`，然後調用 `TranscribeAudioAsync()` |
| <strong>串流支援</strong> | JS 與 C# SDK 也提供 `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` 以產出分段轉錄結果 |
| **CPU 優化版本** | CPU 版本（3.05 GB）可在無 GPU 的任何 Windows 裝置上運作 |
| <strong>隱私優先</strong> | 非常適合將 Zava 客戶互動與專有產品資料留在本地裝置 |

---

## 資源

| 資源 | 連結 |
|----------|-------|
| Foundry Local 文件 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 參考 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper 模型 | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local 官方網站 | [foundrylocal.ai](https://foundrylocal.ai) |

---

## 下一步

繼續閱讀 [第 10 部分：使用自訂或 Hugging Face 模型](part10-custom-models.md)，自行從 Hugging Face 編譯模型並在 Foundry Local 上執行。