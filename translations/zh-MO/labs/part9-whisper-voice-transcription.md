![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第9部分：使用 Whisper 及 Foundry Local 進行語音轉錄

> **目標：** 利用 OpenAI Whisper 模型，透過 Foundry Local 在本地端運行，對音訊檔進行轉錄 —— 完全在裝置上執行，無需雲端。

## 概述

Foundry Local 不僅能用於文字生成，也支援<strong>語音轉文字</strong>模型。在本實驗中，您將使用<strong>OpenAI Whisper Medium</strong>模型，在您本地機器完全轉錄音訊檔。這非常適合像是轉錄 Zava 客戶服務通話、產品評論錄音或工作坊規劃會議等場景，其中音訊資料絕不會離開您的設備。

---

## 學習目標

完成本實驗後，您將能夠：

- 了解 Whisper 語音轉文字模型及其功能
- 使用 Foundry Local 下載並運行 Whisper 模型
- 使用 Foundry Local SDK 以 Python、JavaScript 和 C# 轉錄音訊檔
- 建立完全在本地端運作的簡易轉錄服務
- 理解 Foundry Local 中文本聊天模型與音訊模型的差異

---

## 先決條件

| 需求 | 詳細 |
|-------------|---------|
| **Foundry Local CLI** | 版本 **0.8.101 或以上**（Whisper 模型從 v0.8.101 起提供） |
| <strong>作業系統</strong> | Windows 10/11（x64 或 ARM64） |
| <strong>語言執行環境</strong> | **Python 3.9+** 和／或 **Node.js 18+** 和／或 **.NET 9 SDK** ([下載 .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| <strong>已完成</strong> | [第1部分：入門](part1-getting-started.md)、[第2部分：Foundry Local SDK 深入探索](part2-foundry-local-sdk.md) 及 [第3部分：SDK 與 API](part3-sdk-and-apis.md) |

> **注意：** Whisper 模型必須透過 **SDK** 下載（CLI 無法）。CLI 不支援音訊轉錄端點。請用以下指令檢查您的版本：
> ```bash
> foundry --version
> ```

---

## 概念：Whisper 如何與 Foundry Local 配合運作

OpenAI Whisper 模型是一款通用的語音辨識模型，訓練於大量多樣化音訊資料上。透過 Foundry Local 執行時：

- 模型<strong>完全在您的 CPU 上執行</strong> — 無需 GPU
- 音訊不會離開您的裝置 — <strong>完整隱私保障</strong>
- Foundry Local SDK 管理模型下載及快取
- **JavaScript 與 C#** SDK 內建 `AudioClient` ，全程處理轉錄流程 — 無需手動設定 ONNX
- **Python** 利用 SDK 管理模型，用 ONNX Runtime 直接對編碼器／解碼器 ONNX 模型進行推論

### 工作流程說明（JavaScript 和 C#）— SDK AudioClient

1. **Foundry Local SDK** 下載並快取 Whisper 模型
2. `model.createAudioClient()` (JS) 或 `model.GetAudioClientAsync()` (C#) 建立一個 `AudioClient`
3. `audioClient.transcribe(path)` (JS) 或 `audioClient.TranscribeAudioAsync(path)` (C#) 內部處理整個流程 — 音訊前處理、編碼器、解碼器及標記解碼
4. `AudioClient` 提供 `settings.language` 屬性（設定為 `"en"` 表示英文）以協助準確轉錄

### 工作流程說明（Python）— ONNX Runtime

1. **Foundry Local SDK** 下載並快取 Whisper ONNX 模型檔案
2. <strong>音訊前處理</strong> 將 WAV 音訊轉換為梅爾頻譜（80個梅爾頻帶 × 3000影格）
3. <strong>編碼器</strong> 處理梅爾頻譜並產生隱藏狀態及交叉注意力的 key/value 張量
4. <strong>解碼器</strong> 自回歸運行，逐次生成標記直到產生結尾標記
5. <strong>標記器</strong> 將輸出標記 ID 解碼為可讀文字

### Whisper 模型版本

| 別名 | 模型 ID | 裝置 | 大小 | 說明 |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU 加速（CUDA） |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU 最佳化（大多數設備推薦） |

> **注意：** Whisper 模型不會列在預設的聊天模型列表中，分類於 `automatic-speech-recognition` 任務。請使用 `foundry model info whisper-medium` 查看詳情。

---

## 實驗練習

### 練習 0 - 取得範例音訊檔

本實驗包含以 Zava DIY 產品情境製作的 WAV 檔。請用內建腳本生成：

```bash
# 從倉庫根目錄開始 - 首先建立並啟用 .venv
python -m venv .venv

# Windows（PowerShell）：
.venv\Scripts\Activate.ps1
# macOS：
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

這會在 `samples/audio/` 產生六個 WAV 檔：

| 檔案 | 情境 |
|------|----------|
| `zava-customer-inquiry.wav` | 客戶詢問 **Zava ProGrip 無線電鑽** |
| `zava-product-review.wav` | 顧客評論 **Zava UltraSmooth 內牆漆** |
| `zava-support-call.wav` | 產品支援通話關於 **Zava TitanLock 工具箱** |
| `zava-project-planning.wav` | 自造者規劃使用 **Zava EcoBoard 複合材甲板** 的露台 |
| `zava-workshop-setup.wav` | 使用 **五款 Zava 產品** 的工作坊導覽 |
| `zava-full-project-walkthrough.wav` | 使用 **全部 Zava 產品** 的車庫改造完整導覽（約4分鐘，用於長音訊測試） |

> **提示：** 您也可使用自己的 WAV/MP3/M4A 檔案，或用 Windows 語音錄音機自行錄音。

---

### 練習 1 - 使用 SDK 下載 Whisper 模型

由於新版 Foundry Local 之 CLI 與 Whisper 模型不相容，請使用 **SDK** 下載與加載模型，選擇您的語言：

<details>
<summary><b>🐍 Python</b></summary>

**安裝 SDK：**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# 啟動服務
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

保存成 `download_whisper.py` 並執行：
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

// 創建管理員並啟動服務
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 從目錄中獲取模型
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

保存成 `download-whisper.mjs` 並執行：
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

> **為何用 SDK 而非 CLI？** Foundry Local CLI 不支援直接下載或服務 Whisper 模型。SDK 提供安全可靠的程式化方式管理音訊模型。JavaScript 和 C# SDK 包含內建的 `AudioClient`，可處理整條轉錄流程。Python 則用 ONNX Runtime 針對快取模型直接推論。

---

### 練習 2 - 了解 Whisper SDK

Whisper 轉錄根據編程語言採用不同方式。**JavaScript 和 C#** 的 Foundry Local SDK 內建 `AudioClient`，能以單一調用完成整個流程（音訊前處理、編碼器、解碼器、標記解碼）。**Python** 利用 Foundry Local SDK 管理模型，再用 ONNX Runtime 針對編碼器／解碼器 ONNX 模型直接推論。

| 元件 | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK 套件** | `foundry-local-sdk`、`onnxruntime`、`transformers`、`librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| <strong>模型管理</strong> | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| <strong>特徵抽取</strong> | `WhisperFeatureExtractor` + `librosa` | 由 SDK `AudioClient` 處理 | 由 SDK `AudioClient` 處理 |
| <strong>推論</strong> | `ort.InferenceSession`（編碼器 + 解碼器） | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| <strong>標記解碼</strong> | `WhisperTokenizer` | 由 SDK `AudioClient` 處理 | 由 SDK `AudioClient` 處理 |
| <strong>語言設定</strong> | 透過解碼器 token 的 `forced_ids` 設定 | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| <strong>輸入</strong> | WAV 檔路徑 | WAV 檔路徑 | WAV 檔路徑 |
| <strong>輸出</strong> | 解碼後文字字串 | `result.text` | `result.Text` |

> **重要：** 請務必在 `AudioClient` 物件上設定語言（例如 `"en"` 代表英文）。若不明確設定語言，模型可能自動偵測時產生亂碼。

> **SDK 使用範例：** Python 以 `FoundryLocalManager(alias)` 啟動，再用 `get_cache_location()` 找 ONNX 模型檔案。JavaScript 和 C# 則透過 SDK 內建 `AudioClient` — 用 `model.createAudioClient()`（JS）或 `model.GetAudioClientAsync()`（C#）獲取 — 處理整個轉錄流程。詳情見 [第2部分：Foundry Local SDK 深入探索](part2-foundry-local-sdk.md)。

---

### 練習 3 - 建立簡易轉錄應用

選擇您的程式語言軌跡，建立一個簡易應用程式來轉錄音訊檔。

> **支援音訊格式：** WAV、MP3、M4A。建議使用 16kHz 取樣率的 WAV 檔以取得最佳效果。

<details>
<summary><h3>Python 軌跡</h3></summary>

#### 環境設置

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

建立檔案 `foundry-local-whisper.py`：

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

# 第一步：引導程序 - 啟動服務，下載並載入模型
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# 建立到快取 ONNX 模型檔案的路徑
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# 第二步：載入 ONNX 會話及特徵擷取器
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

# 第三步：擷取梅爾頻譜特徵
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# 第四步：執行編碼器
enc_out = encoder.run(None, {"audio_features": input_features})
# 第一個輸出為隱藏狀態；其餘為交叉注意力 KV 配對
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# 第五步：自回歸解碼
initial_tokens = [50258, 50259, 50359, 50363]  # sot、英文、轉錄、無時間戳
input_ids = np.array([initial_tokens], dtype=np.int32)

# 清空自注意力 KV 快取
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

    # 更新自注意力 KV 快取
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### 執行程式

```bash
# 抄錄一個 Zava 產品場景
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# 或嘗試其他：
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Python 重點整理

| 方法 | 作用 |
|--------|---------|
| `FoundryLocalManager(alias)` | 啟動：啟動服務、下載並載入模型 |
| `manager.get_cache_location()` | 取得快取 ONNX 模型檔案路徑 |
| `WhisperFeatureExtractor.from_pretrained()` | 載入梅爾頻譜特徵擷取器 |
| `ort.InferenceSession()` | 建立 ONNX Runtime 的編碼器及解碼器推論會話 |
| `tokenizer.decode()` | 將輸出標記ID轉回文字 |

</details>

<details>
<summary><h3>JavaScript 軌跡</h3></summary>

#### 環境設置

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

// 第一步：引導 - 創建管理器，啟動服務，並載入模型
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

// 第二步：創建音頻客戶端並進行轉錄
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// 清理工作
await model.unload();
```

> **注意：** Foundry Local SDK 透過 `model.createAudioClient()` 提供內建的 `AudioClient`，內部即可完成整個 ONNX 推論流程 — 無須額外引入 `onnxruntime-node`。務必設定 `audioClient.settings.language = "en"` 以確保英文轉錄正確。

#### 執行程式

```bash
# 抄錄一個Zava產品場景
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# 或嘗試其他：
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### JavaScript 重點整理

| 方法 | 作用 |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | 建立管理器單例 |
| `await catalog.getModel(alias)` | 從目錄中取得模型 |
| `model.download()` / `model.load()` | 下載並載入 Whisper 模型 |
| `model.createAudioClient()` | 建立音訊用戶端進行轉錄 |
| `audioClient.settings.language = "en"` | 設定轉錄語言（準確輸出所必須） |
| `audioClient.transcribe(path)` | 轉錄音訊檔，回傳 `{ text, duration }` |

</details>

<details>
<summary><h3>C# 軌跡</h3></summary>

#### 環境設置

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **注意：** C# 軌跡使用 `Microsoft.AI.Foundry.Local` 套件，藉由 `model.GetAudioClientAsync()` 提供內建 `AudioClient`，可在程式內部完成全套轉錄流程 — 無需分開設定 ONNX Runtime。

#### 轉錄程式碼

取代 `Program.cs` 內容：

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
# 抄錄 Zava 產品情境
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# 或試試其他：
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### C# 重點整理

| 方法 | 作用 |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | 使用設定初始化 Foundry Local |
| `catalog.GetModelAsync(alias)` | 從目錄取模型 |
| `model.DownloadAsync()` | 下載 Whisper 模型 |
| `model.GetAudioClientAsync()` | 取得 AudioClient（非 ChatClient） |
| `audioClient.Settings.Language = "en"` | 設定轉錄語言（準確輸出所必須） |
| `audioClient.TranscribeAudioAsync(path)` | 轉錄音訊檔 |
| `result.Text` | 轉錄文字結果 |
> **C# 與 Python/JS：** C# SDK 提供內建的 `AudioClient`，可透過 `model.GetAudioClientAsync()` 進行內部程序的轉錄，類似於 JavaScript SDK。Python 則直接使用 ONNX Runtime 對快取的編碼器/解碼器模型進行推論。

</details>

---

### 練習 4 - 批次轉錄所有 Zava 範例

既然您已經有一個可用的轉錄應用程式，請轉錄所有五個 Zava 範例檔案，並比較結果。

<details>
<summary><h3>Python 路線</h3></summary>

完整範例 `python/foundry-local-whisper.py` 已支援批次轉錄。執行時不加參數會對 `samples/audio/` 中的所有 `zava-*.wav` 檔案進行轉錄：

```bash
cd python
python foundry-local-whisper.py
```

範例使用 `FoundryLocalManager(alias)` 作為引導，然後分別對每個檔案運行編碼器和解碼器 ONNX 會話。

</details>

<details>
<summary><h3>JavaScript 路線</h3></summary>

完整範例 `javascript/foundry-local-whisper.mjs` 已支援批次轉錄。執行時不加參數會對 `samples/audio/` 中的所有 `zava-*.wav` 檔案進行轉錄：

```bash
cd javascript
node foundry-local-whisper.mjs
```

範例使用 `FoundryLocalManager.create()` 與 `catalog.getModel(alias)` 初始化 SDK，然後使用 `AudioClient`（並設定 `settings.language = "en"`）來轉錄每個檔案。

</details>

<details>
<summary><h3>C# 路線</h3></summary>

完整範例 `csharp/WhisperTranscription.cs` 已支援批次轉錄。執行時不帶特定檔案參數會對 `samples/audio/` 中的所有 `zava-*.wav` 檔案進行轉錄：

```bash
cd csharp
dotnet run whisper
```

範例使用 `FoundryLocalManager.CreateAsync()` 與 SDK 內建的 `AudioClient`（並設定 `Settings.Language = "en"`）進行內部程序的轉錄。

</details>

**注意事項：** 對照 `samples/audio/generate_samples.py` 中的原始文字，檢視轉錄結果的準確度。Whisper 對於產品名稱如「Zava ProGrip」及技術術語如「brushless motor」或「composite decking」的捕捉效果如何？

---

### 練習 5 - 理解關鍵程式碼模式

研究 Whisper 轉錄與聊天完成在三種語言中的差異：

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

# 音訊轉錄（本部分）：
# 直接使用ONNX運行時，而非OpenAI客戶端
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... 自回歸解碼器循環 ...
print(tokenizer.decode(generated_tokens))
```

**關鍵洞察：** 聊天模型透過 `manager.endpoint` 使用與 OpenAI 兼容的 API。Whisper 則使用 SDK 定位快取的 ONNX 模型檔案，然後直接使用 ONNX Runtime 做推論。

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

**關鍵洞察：** 聊天模型透過 `manager.urls[0] + "/v1"` 使用與 OpenAI 兼容的 API。Whisper 轉錄則使用 SDK 的 `AudioClient`，由 `model.createAudioClient()` 取得。設定 `settings.language` 以避免自動偵測時輸出亂碼。

</details>

<details>
<summary><b>C# - 與聊天的主要差異</b></summary>

C# 方法使用 SDK 內建的 `AudioClient` 做內部程序的轉錄：

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

**關鍵洞察：** C# 使用 `FoundryLocalManager.CreateAsync()` 並直接取得 `AudioClient`，無需 ONNX Runtime 設定。設定 `Settings.Language` 避免自動偵測時輸出亂碼。

</details>

> **總結：** Python 使用 Foundry Local SDK 管理模型，再加上 ONNX Runtime 直接對編碼器/解碼器模型推論。JavaScript 與 C# 則都使用 SDK 內建的 `AudioClient` 來簡化轉錄流程——建立客戶端、設語言、呼叫 `transcribe()` 或 `TranscribeAudioAsync()`。務必設定 AudioClient 的語言屬性，才能獲得準確結果。

---

### 練習 6 - 實驗

嘗試以下修改，加深理解：

1. <strong>嘗試不同音檔</strong> — 使用 Windows 語音錄音機錄製自己說話，儲存為 WAV，並轉錄

2. <strong>比較模型變體</strong> — 若您有 NVIDIA GPU，可嘗試 CUDA 版本：
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   比較其轉錄速度與 CPU 版本。

3. <strong>新增輸出格式化</strong> — JSON 回應可以包含：
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **建立 REST API** — 將您的轉錄程式碼包裝成網頁伺服器：

   | 語言 | 框架 | 範例 |
   |----------|-----------|--------|
   | Python | FastAPI | 使用 `@app.post("/v1/audio/transcriptions")` 搭配 `UploadFile` |
   | JavaScript | Express.js | 使用 `app.post("/v1/audio/transcriptions")` 搭配 `multer` |
   | C# | ASP.NET Minimal API | 以 `app.MapPost("/v1/audio/transcriptions")` 搭配 `IFormFile` |

5. <strong>結合多輪與轉錄</strong> — 將 Whisper 轉錄與第 4 部分的聊天代理結合：先轉錄語音，再將文字傳給代理做分析或摘要。

---

## SDK 音訊 API 參考

> **JavaScript AudioClient：**
> - `model.createAudioClient()` — 建立一個 `AudioClient` 實例
> - `audioClient.settings.language` — 設定轉錄語言（例如 `"en"`）
> - `audioClient.settings.temperature` — 控制隨機性（可選）
> - `audioClient.transcribe(filePath)` — 轉錄檔案，回傳 `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — 透過回呼串流傳回轉錄片段
>
> **C# AudioClient：**
> - `await model.GetAudioClientAsync()` — 建立一個 `OpenAIAudioClient` 實例
> - `audioClient.Settings.Language` — 設定轉錄語言（例如 `"en"`）
> - `audioClient.Settings.Temperature` — 控制隨機性（可選）
> - `await audioClient.TranscribeAudioAsync(filePath)` — 轉錄檔案，回傳含 `.Text` 的物件
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — 回傳轉錄片段的 `IAsyncEnumerable`

> **提示：** 轉錄前務必設定語言屬性。若未設定，Whisper 模型將嘗試自動偵測，可能產生亂碼（一個替代字符代替文本）。

---

## 比較：聊天模型 vs. Whisper

| 項目 | 聊天模型（第 3-7 部分） | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| <strong>任務類型</strong> | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| <strong>輸入</strong> | 文字訊息（JSON） | 音訊檔案（WAV/MP3/M4A） | 音訊檔案（WAV/MP3/M4A） |
| <strong>輸出</strong> | 產生文字（串流） | 轉錄文字（完整） | 轉錄文字（完整） |
| **SDK 套件** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk`（JS） / `Microsoft.AI.Foundry.Local`（C#） |
| **API 方法** | `client.chat.completions.create()` | ONNX Runtime 直接執行 | `audioClient.transcribe()`（JS）/ `audioClient.TranscribeAudioAsync()`（C#） |
| <strong>語言設定</strong> | 無 | 解碼器提示詞 | `audioClient.settings.language`（JS）/ `audioClient.Settings.Language`（C#） |
| <strong>串流支援</strong> | 有 | 無 | `transcribeStreaming()`（JS）/ `TranscribeAudioStreamingAsync()`（C#） |
| <strong>隱私優勢</strong> | 程式碼/資料保留本地 | 音訊資料保留本地 | 音訊資料保留本地 |

---

## 主要心得

| 概念 | 您學到的內容 |
|---------|-----------------|
| **Whisper 本地運作** | 語音轉文字完全在本地執行，適合轉錄 Zava 客戶通話和產品評論 |
| **SDK AudioClient** | JavaScript 與 C# SDK 提供內建的 `AudioClient`，一次呼叫即可完成整個轉錄流程 |
| <strong>語言設定</strong> | 必須設定 AudioClient 語言（例如 `"en"`），否則自動偵測可能導致亂碼輸出 |
| **Python** | 使用 `foundry-local-sdk` 管理模型，搭配 `onnxruntime`、`transformers` 及 `librosa` 直接進行 ONNX 推論 |
| **JavaScript** | 透過 `foundry-local-sdk` 的 `model.createAudioClient()` 建立客戶端，設定 `settings.language`，再呼叫 `transcribe()` |
| **C#** | 使用 `Microsoft.AI.Foundry.Local` 的 `model.GetAudioClientAsync()`，設定 `Settings.Language`，再呼叫 `TranscribeAudioAsync()` |
| <strong>串流支援</strong> | JS 和 C# SDK 亦提供 `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` 進行片段輸出 |
| **CPU 優化版** | CPU 版本（3.05 GB）可在無 GPU 的任何 Windows 設備上執行 |
| <strong>隱私優先</strong> | 完美適合將 Zava 客戶互動與專有產品資料留在本地設備 |

---

## 資源

| 資源 | 連結 |
|----------|------|
| Foundry Local 文件 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 參考 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper 模型 | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local 官網 | [foundrylocal.ai](https://foundrylocal.ai) |

---

## 下一步

繼續進入 [第 10 部分：使用自訂或 Hugging Face 模型](part10-custom-models.md)，自行從 Hugging Face 編譯模型並於 Foundry Local 執行。