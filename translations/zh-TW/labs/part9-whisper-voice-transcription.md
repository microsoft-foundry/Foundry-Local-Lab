![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第9部分：使用 Whisper 與 Foundry Local 進行語音轉錄

> **目標：** 使用 OpenAI Whisper 模型，透過 Foundry Local 在本地運行，對音訊檔案進行轉錄 - 完全在裝置上運行，無需雲端。

## 概覽

Foundry Local 不僅可用於文字生成；它也支援 <strong>語音轉文字</strong> 模型。在此實驗中，您將使用 **OpenAI Whisper Medium** 模型，完全在您的機器上對音訊檔案進行轉錄。這非常適合用於轉錄 Zava 客戶服務通話、產品評論錄音、或工作坊計劃會議等，這些情況下音訊資料絕不離開您的裝置。

---

## 學習目標

在本實驗結束時，您將能夠：

- 理解 Whisper 語音轉文字模型及其能力
- 使用 Foundry Local 下載及運行 Whisper 模型
- 使用 Python、JavaScript 和 C# 的 Foundry Local SDK 對音訊檔案進行轉錄
- 建立一個完全在裝置上運行的簡易轉錄服務
- 理解 Foundry Local 中聊天/文字模型與語音模型的差異

---

## 預備知識

| 要求 | 詳細資訊 |
|-------------|---------|
| **Foundry Local CLI** | 版本 **0.8.101 或以上**（Whisper 模型自 v0.8.101 起提供） |
| <strong>作業系統</strong> | Windows 10/11（x64 或 ARM64） |
| <strong>語言執行環境</strong> | **Python 3.9+** 和/或 **Node.js 18+** 和/或 **.NET 9 SDK** ([下載 .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| <strong>完成</strong> | [第1部分：入門](part1-getting-started.md)、[第2部分：Foundry Local SDK 深入](part2-foundry-local-sdk.md)、及 [第3部分：SDK 與 API](part3-sdk-and-apis.md) |

> **注意：** Whisper 模型必須透過 **SDK** 下載（非 CLI）。CLI 不支援語音轉錄端點。請用以下指令檢查您的版本：
> ```bash
> foundry --version
> ```

---

## 概念：Whisper 如何與 Foundry Local 一起運作

OpenAI Whisper 模型是一個通用語音辨識模型，經過大量多樣化音訊資料訓練。透過 Foundry Local 運行時：

- 模型完全在您的 **CPU** 上執行 — 無需 GPU
- 音訊絕不離開您的裝置 — <strong>完整隱私保障</strong>
- Foundry Local SDK 負責模型下載與快取管理
- **JavaScript 與 C#** 的 SDK 內建 `AudioClient`，處理整個轉錄流程 — 無需手動 ONNX 設定
- **Python** 使用 SDK 進行模型管理，並利用 ONNX Runtime 直接針對編碼器/解碼器 ONNX 模型進行推論

### 流程概覽（JavaScript 與 C#）— SDK AudioClient

1. **Foundry Local SDK** 下載並快取 Whisper 模型
2. `model.createAudioClient()`（JS）或 `model.GetAudioClientAsync()`（C#）建立 `AudioClient`
3. `audioClient.transcribe(path)`（JS）或 `audioClient.TranscribeAudioAsync(path)`（C#）於內部處理完整流程 — 包含音訊前置處理、編碼器、解碼器及標記解碼
4. `AudioClient` 提供 `settings.language` 屬性（預設 "en" 為英文），以協助精準轉錄

### 流程概覽（Python）— ONNX Runtime

1. **Foundry Local SDK** 下載並快取 Whisper ONNX 模型檔案
2. <strong>音訊前置處理</strong> 將 WAV 音訊轉成 mel 頻譜圖（80個 mel 座標 × 3000幀）
3. <strong>編碼器</strong> 處理 mel 頻譜圖，產生隱藏狀態及跨注意力的 key/value 張量
4. <strong>解碼器</strong> 自回歸方式逐一產生 token，直到輸出結束符號
5. <strong>標記解碼器</strong> 將輸出的 token IDs 解碼回可讀文字

### Whisper 模型變體

| 別名 | 模型 ID | 裝置 | 大小 | 說明 |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU 加速（CUDA） |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU 優化（大多數裝置推薦） |

> **注意：** 與預設列出的聊天模型不同，Whisper 模型分類在 `automatic-speech-recognition` 任務。使用 `foundry model info whisper-medium` 查詢詳細資訊。

---

## 實驗練習

### 練習0 - 取得範例音訊檔案

此實驗包含基於 Zava DIY 產品場景的預先製作 WAV 檔，您可利用附帶的腳本產生它們：

```bash
# 從倉庫根目錄開始 - 先建立並啟用 .venv
python -m venv .venv

# Windows（PowerShell）：
.venv\Scripts\Activate.ps1
# macOS：
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

此腳本會在 `samples/audio/` 建立六個 WAV 檔案：

| 檔案 | 場景 |
|------|----------|
| `zava-customer-inquiry.wav` | 客戶詢問 **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | 客戶審查 **Zava UltraSmooth Interior Paint** |
| `zava-support-call.wav` | 關於 **Zava TitanLock Tool Chest** 的支援通話 |
| `zava-project-planning.wav` | DIYer 計劃用 **Zava EcoBoard Composite Decking** 建造甲板 |
| `zava-workshop-setup.wav` | 使用 **所有五個 Zava 產品** 的工作坊介紹 |
| `zava-full-project-walkthrough.wav` | 4分鐘長的車庫整修完整介紹（使用所有 Zava 產品） |

> **提示：** 您也可以使用自己錄製的 WAV/MP3/M4A 檔案，或使用 Windows 語音錄音機錄音。

---

### 練習1 - 使用 SDK 下載 Whisper 模型

由於新版 Foundry Local 中 CLI 與 Whisper 模型不完全相容，請使用 **SDK** 下載並載入模型。選擇以下對應語言：

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

另存為 `download_whisper.py`，執行：
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

// 從目錄取得模型
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

另存為 `download-whisper.mjs`，執行：
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

> **為何用 SDK 而非 CLI？** Foundry Local CLI 不支援直接下載或服務 Whisper 模型。SDK 提供可靠的程式化方式來下載和管理音訊模型。JavaScript 與 C# SDK 內建 `AudioClient`，可處理整個轉錄流程；Python 則用 ONNX Runtime 針對快取模型檔案直接推論。

---

### 練習2 - 了解 Whisper SDK

Whisper 轉錄在不同語言中用不同做法。**JavaScript 和 C#** 透過 Foundry Local SDK 內建的 `AudioClient`，僅以一行方法呼叫就完成整個流程（音訊前置處理、編碼器、解碼器、標記解碼）。**Python** 則使用 Foundry Local SDK 進行模型管理，並以 ONNX Runtime 直接對編碼器／解碼器 ONNX 模型推論。

| 組件 | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK 套件** | `foundry-local-sdk`、`onnxruntime`、`transformers`、`librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| <strong>模型管理</strong> | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| <strong>特徵擷取</strong> | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` 處理 | SDK `AudioClient` 處理 |
| <strong>推論</strong> | `ort.InferenceSession`（編碼器 + 解碼器） | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| <strong>標記解碼</strong> | `WhisperTokenizer` | SDK `AudioClient` 處理 | SDK `AudioClient` 處理 |
| <strong>語言設定</strong> | 解碼器標記用 `forced_ids` 設定 | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| <strong>輸入</strong> | WAV 檔路徑 | WAV 檔路徑 | WAV 檔路徑 |
| <strong>輸出</strong> | 解碼後文字字串 | `result.text` | `result.Text` |

> **重要：** 請務必在 `AudioClient` 上設定語言（例如 `"en"` 表示英文），若未指定語言，模型會嘗試自動偵測，可能導致錯亂輸出。

> **SDK 使用模式：** Python 用 `FoundryLocalManager(alias)` 啟動，然後 `get_cache_location()` 找 ONNX 模型檔。JavaScript 和 C# 利用 SDK 內建的 `AudioClient`（透過 `model.createAudioClient()` 或 `model.GetAudioClientAsync()` 取得），處理完整轉錄流程。詳見 [第2部分：Foundry Local SDK 深入](part2-foundry-local-sdk.md)。

---

### 練習3 - 建立簡易轉錄應用程式

選擇您的語言路線，打造一個最小應用程式，對音訊檔案進行轉錄。

> **支援音訊格式：** WAV、MP3、M4A。建議使用取樣率為16kHz的 WAV 檔案以獲得最佳效果。

<details>
<summary><h3>Python 路線</h3></summary>

#### 設定

```bash
cd python
python -m venv venv

# 啟用虛擬環境：
# Windows（PowerShell）：
venv\Scripts\Activate.ps1
# macOS：
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### 轉錄程式碼

建立 `foundry-local-whisper.py`：

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

# 第一步驟：啟動引導 - 啟動服務，下載並載入模型
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# 建立快取的 ONNX 模型檔案路徑
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# 第二步驟：載入 ONNX 會話和特徵擷取器
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

# 第三步驟：擷取梅爾頻譜特徵
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# 第四步驟：執行編碼器
enc_out = encoder.run(None, {"audio_features": input_features})
# 第一個輸出是隱藏狀態；其餘是交叉注意力的 KV 配對
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# 第五步驟：自回歸解碼
initial_tokens = [50258, 50259, 50359, 50363]  # sot、en、轉錄、不帶時間戳
input_ids = np.array([initial_tokens], dtype=np.int32)

# 空的自注意力 KV 快取
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

    if next_token == 50257:  # 文字結尾
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
# 轉錄一個Zava產品場景
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# 或嘗試其他：
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Python 重要要點

| 方法 | 目的 |
|--------|---------|
| `FoundryLocalManager(alias)` | 啟動：啟動服務，下載並載入模型 |
| `manager.get_cache_location()` | 取得快取的 ONNX 模型檔路徑 |
| `WhisperFeatureExtractor.from_pretrained()` | 載入 mel 頻譜圖特徵擷取器 |
| `ort.InferenceSession()` | 建立編碼器與解碼器的 ONNX Runtime 會話 |
| `tokenizer.decode()` | 將輸出 token IDs 轉回文字 |

</details>

<details>
<summary><h3>JavaScript 路線</h3></summary>

#### 設定

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### 轉錄程式碼

建立 `foundry-local-whisper.mjs`：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// 第一步：啟動引導 - 建立管理器、啟動服務並載入模型
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

// 第二步：建立音訊客戶端並進行轉錄
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

> **注意：** Foundry Local SDK 提供內建 `AudioClient`，透過 `model.createAudioClient()` 取得，內部處理整個 ONNX 推論流程 — 無需匯入 `onnxruntime-node`。請務必設定 `audioClient.settings.language = "en"` 以確保英文準確轉錄。

#### 執行程式

```bash
# 抄寫一個Zava產品場景
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# 或嘗試其他：
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### JavaScript 重要要點

| 方法 | 目的 |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | 建立管理器 singleton |
| `await catalog.getModel(alias)` | 從目錄取得模型 |
| `model.download()` / `model.load()` | 下載並載入 Whisper 模型 |
| `model.createAudioClient()` | 建立音訊客戶端用於轉錄 |
| `audioClient.settings.language = "en"` | 設定轉錄語言（確保準確輸出） |
| `audioClient.transcribe(path)` | 轉錄音訊檔案，返回 `{ text, duration }` |

</details>

<details>
<summary><h3>C# 路線</h3></summary>

#### 設定

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **注意：** C# 路線使用 `Microsoft.AI.Foundry.Local` 套件，提供內建 `AudioClient` 透過 `model.GetAudioClientAsync()` 取得，處理完整轉錄流程 — 無需額外設定 ONNX Runtime。

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
# 轉錄一個Zava產品場景
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# 或嘗試其他：
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### C# 重要要點

| 方法 | 目的 |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | 使用設定初始化 Foundry Local |
| `catalog.GetModelAsync(alias)` | 從目錄取得模型 |
| `model.DownloadAsync()` | 下載 Whisper 模型 |
| `model.GetAudioClientAsync()` | 取得 AudioClient（非 ChatClient！） |
| `audioClient.Settings.Language = "en"` | 設定轉錄語言（確保準確輸出） |
| `audioClient.TranscribeAudioAsync(path)` | 轉錄音訊檔案 |
| `result.Text` | 轉錄的文字 |


> **C# vs Python/JS：** C# SDK 提供內建的 `AudioClient`，可透過 `model.GetAudioClientAsync()` 進行內部轉錄，與 JavaScript SDK 類似。Python 則直接使用 ONNX Runtime 執行快取的編碼器/解碼器模型做推理。

</details>

---

### 練習 4 - 批次轉錄所有 Zava 範例

既然你已經有一個可用的轉錄應用程式，現在批次轉錄五個 Zava 範例檔案並比較結果。

<details>
<summary><h3>Python 途徑</h3></summary>

完整範例 `python/foundry-local-whisper.py` 已支持批次轉錄。不帶參數執行時，它會轉錄 `samples/audio/` 資料夾中所有 `zava-*.wav` 檔案：

```bash
cd python
python foundry-local-whisper.py
```

此範例使用 `FoundryLocalManager(alias)` 進行啟動，然後對每個音訊檔案執行編碼器及解碼器的 ONNX 會話。

</details>

<details>
<summary><h3>JavaScript 途徑</h3></summary>

完整範例 `javascript/foundry-local-whisper.mjs` 已支持批次轉錄。不帶參數執行時，它會轉錄 `samples/audio/` 資料夾中所有 `zava-*.wav` 檔案：

```bash
cd javascript
node foundry-local-whisper.mjs
```

此範例使用 `FoundryLocalManager.create()` 和 `catalog.getModel(alias)` 初始化 SDK，接著用帶有 `settings.language = "en"` 的 `AudioClient` 轉錄每個檔案。

</details>

<details>
<summary><h3>C# 途徑</h3></summary>

完整範例 `csharp/WhisperTranscription.cs` 已支持批次轉錄。不帶特定檔案參數執行時，它會轉錄 `samples/audio/` 資料夾中所有 `zava-*.wav` 檔案：

```bash
cd csharp
dotnet run whisper
```

此範例使用 `FoundryLocalManager.CreateAsync()` 和 SDK 內建的 `AudioClient` （設置 `Settings.Language = "en"`）來進行內部轉錄。

</details>

**注意點：** 將轉錄輸出與 `samples/audio/generate_samples.py` 裡的原始文字作比較。Whisper 在捕捉像 "Zava ProGrip" 這類產品名稱與 "brushless motor" 或 "composite decking" 這類技術用詞時準確度如何？

---

### 練習 5 - 理解關鍵程式碼模式

研究 Whisper 轉錄與聊天完成在三種語言中的差異：

<details>
<summary><b>Python - 與聊天的主要差異</b></summary>

```python
# 聊天完成（第2-6部分）：
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# 音訊轉錄（本部分）：
# 直接使用 ONNX Runtime，而非 OpenAI 用戶端
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# … 自回歸解碼器循環 …
print(tokenizer.decode(generated_tokens))
```

**關鍵洞察：** 聊天模型透過 `manager.endpoint` 使用相容 OpenAI 的 API。Whisper 透過 SDK 找到快取的 ONNX 模型檔，然後直接使用 ONNX Runtime 執行推理。

</details>

<details>
<summary><b>JavaScript - 與聊天的主要差異</b></summary>

```javascript
// 聊天補全（第2至6部分）：
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// 音訊轉錄（本部分）：
// 使用 SDK 內建的 AudioClient
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // 為獲得最佳效果請務必設定語言
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**關鍵洞察：** 聊天模型透過 `manager.urls[0] + "/v1"` 使用相容 OpenAI 的 API。Whisper 轉錄則使用 SDK 的 `AudioClient`（來自 `model.createAudioClient()`）。設定 `settings.language` 可避免自動偵測造成的雜訊輸出。

</details>

<details>
<summary><b>C# - 與聊天的主要差異</b></summary>

C# 方法使用 SDK 內建的 `AudioClient` 來進行內部轉錄：

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

**轉錄過程：**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**關鍵洞察：** C# 使用 `FoundryLocalManager.CreateAsync()` 並直接取得 `AudioClient` — 不需額外設定 ONNX Runtime。設定 `Settings.Language` 以避免自動偵測產生雜訊。

</details>

> **總結：** Python 使用 Foundry Local SDK 進行模型管理，並用 ONNX Runtime 對編碼器/解碼器模型做直接推理。JavaScript 和 C# 皆使用 SDK 內建的 `AudioClient` 來簡化轉錄流程 — 建立客戶端、設定語言、呼叫 `transcribe()` / `TranscribeAudioAsync()`。務必為 AudioClient 設定語言屬性以取得準確結果。

---

### 練習 6 - 實驗

嘗試以下修改深化理解：

1. <strong>試試不同的音訊檔案</strong> — 使用 Windows 語音錄製器錄音，儲存為 WAV，然後轉錄

2. <strong>比較模型變體</strong> — 如果你有 NVIDIA GPU，試試 CUDA 變體：
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   比較其與 CPU 變體的轉錄速度。

3. <strong>增加輸出格式化</strong> — JSON 回應可以包括：
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **建立 REST API** — 將轉錄程式包裝在網路伺服器中：

   | 語言 | 框架 | 範例 |
   |----------|-----------|--------|
   | Python | FastAPI | 使用 `@app.post("/v1/audio/transcriptions")` 搭配 `UploadFile` |
   | JavaScript | Express.js | 使用 `app.post("/v1/audio/transcriptions")` 搭配 `multer` |
   | C# | ASP.NET Minimal API | 使用 `app.MapPost("/v1/audio/transcriptions")` 搭配 `IFormFile` |

5. <strong>多輪對話與轉錄</strong> — 將 Whisper 與第 4 部分的聊天代理結合：先轉錄音訊，再將結果文字傳給代理，做分析或摘要。

---

## SDK 音訊 API 參考

> **JavaScript AudioClient：**
> - `model.createAudioClient()` — 建立一個 `AudioClient` 實例
> - `audioClient.settings.language` — 設定轉錄語言（例如 `"en"`）
> - `audioClient.settings.temperature` — 控制隨機性（選用）
> - `audioClient.transcribe(filePath)` — 轉錄檔案，回傳 `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — 透過回呼串流回傳轉錄片段
>
> **C# AudioClient：**
> - `await model.GetAudioClientAsync()` — 建立一個 `OpenAIAudioClient` 實例
> - `audioClient.Settings.Language` — 設定轉錄語言（例如 `"en"`）
> - `audioClient.Settings.Temperature` — 控制隨機性（選用）
> - `await audioClient.TranscribeAudioAsync(filePath)` — 轉錄檔案，回傳含 `.Text` 的物件
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — 回傳轉錄片段的 `IAsyncEnumerable`

> **提示：** 建議始終在轉錄前設定語言屬性。未設定時，Whisper 模型會嘗試自動偵測，可能產生雜訊輸出（以單一替代字元表示，而非文字）。

---

## 比較：聊天模型 vs. Whisper

| 面向 | 聊天模型（第 3-7 部分） | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| <strong>任務類型</strong> | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| <strong>輸入</strong> | 文字訊息（JSON） | 音訊檔案（WAV/MP3/M4A） | 音訊檔案（WAV/MP3/M4A） |
| <strong>輸出</strong> | 產生的文字（串流） | 轉錄文字（完整） | 轉錄文字（完整） |
| **SDK 套件** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk`（JS） / `Microsoft.AI.Foundry.Local`（C#） |
| **API 方法** | `client.chat.completions.create()` | 直接使用 ONNX Runtime | `audioClient.transcribe()`（JS） / `audioClient.TranscribeAudioAsync()`（C#） |
| <strong>語言設定</strong> | 不適用 | 解碼器提示符令牌 | `audioClient.settings.language`（JS） / `audioClient.Settings.Language`（C#） |
| <strong>串流</strong> | 有 | 無 | `transcribeStreaming()`（JS） / `TranscribeAudioStreamingAsync()`（C#） |
| <strong>隱私好處</strong> | 程式碼/資料保留在本地 | 音訊資料保留在本地 | 音訊資料保留在本地 |

---

## 重要收穫

| 概念 | 你學到了什麼 |
|---------|-----------------|
| **Whisper 裝置端** | 語音轉文字完全在本地執行，適合在裝置端轉錄 Zava 客戶通話與產品評論 |
| **SDK AudioClient** | JavaScript 和 C# SDK 提供內建 `AudioClient`，可單次呼叫完成整個轉錄流程 |
| <strong>語言設定</strong> | 始終設定 AudioClient 語言（如 `"en"`）— 不設定會自動偵測並可能產生雜訊輸出 |
| **Python** | 使用 `foundry-local-sdk` 管理模型，搭配 `onnxruntime` + `transformers` + `librosa` 做直接 ONNX 推理 |
| **JavaScript** | 使用 `foundry-local-sdk` 並透過 `model.createAudioClient()` — 設定 `settings.language`，然後呼叫 `transcribe()` |
| **C#** | 使用 `Microsoft.AI.Foundry.Local`，透過 `model.GetAudioClientAsync()` — 設定 `Settings.Language`，然後呼叫 `TranscribeAudioAsync()` |
| <strong>串流支援</strong> | JS 和 C# SDK 也提供 `transcribeStreaming()` / `TranscribeAudioStreamingAsync()`，可逐片段輸出 |
| **CPU 優化** | CPU 變體（3.05 GB）可於任何無 GPU 的 Windows 裝置上運行 |
| <strong>隱私為先</strong> | 完美保護 Zava 客戶互動與專有產品資料保留在本地端 |

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

繼續閱讀 [第 10 部分：使用自訂或 Hugging Face 模型](part10-custom-models.md)，從 Hugging Face 編譯你自己的模型並於 Foundry Local 執行。