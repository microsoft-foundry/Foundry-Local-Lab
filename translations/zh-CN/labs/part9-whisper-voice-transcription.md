![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第9部分：使用 Whisper 和 Foundry Local 进行语音转录

> **目标：** 通过 Foundry Local 本地运行 OpenAI Whisper 模型来转录音频文件——完全在设备上，无需云端。

## 概述

Foundry Local 不仅用于文本生成；它还支持<strong>语音到文本</strong>模型。在本实验中，您将使用<strong>OpenAI Whisper Medium</strong>模型在您的机器上完整转录音频文件。这非常适合诸如转录 Zava 客户服务电话、产品评论录音或工作坊计划会议等场景，其中音频数据绝对不会离开您的设备。

---

## 学习目标

完成本实验后，您将能够：

- 理解 Whisper 语音转文本模型及其功能
- 使用 Foundry Local 下载并运行 Whisper 模型
- 使用 Foundry Local SDK 在 Python、JavaScript 和 C# 中转录音频文件
- 构建一个完全在设备上运行的简单转录服务
- 理解 Foundry Local 中聊天/文本模型与音频模型的区别

---

## 先决条件

| 要求 | 详情 |
|-------------|---------|
| **Foundry Local CLI** | 版本 **0.8.101 或更高**（Whisper 模型从 v0.8.101 开始提供） |
| <strong>操作系统</strong> | Windows 10/11（x64 或 ARM64） |
| <strong>语言运行时</strong> | **Python 3.9+** 和/或 **Node.js 18+** 和/或 **.NET 9 SDK** ([下载 .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| <strong>完成了</strong> | [第1部分：快速入门](part1-getting-started.md), [第2部分：Foundry Local SDK 深入](part2-foundry-local-sdk.md), 以及 [第3部分：SDK 和 API](part3-sdk-and-apis.md) |

> **注意：** Whisper 模型必须通过<strong>SDK</strong>下载（而非 CLI）。CLI 不支持音频转录端点。使用以下命令检查您的版本：
> ```bash
> foundry --version
> ```

---

## 概念：Whisper 与 Foundry Local 的工作原理

OpenAI Whisper 模型是一款通用语音识别模型，基于大量多样的音频数据进行训练。通过 Foundry Local 运行时：

- 模型<strong>完全在您的 CPU 上运行</strong>——不需要 GPU
- 音频永远不会离开您的设备 —— <strong>完全隐私保护</strong>
- Foundry Local SDK 负责模型下载和缓存管理
- **JavaScript 和 C#** SDK 提供内置 `AudioClient`，处理整个转录流程——无需手动设置 ONNX
- **Python** 使用 SDK 进行模型管理，并使用 ONNX Runtime 直接针对编码器和解码器 ONNX 模型进行推理

### 管道工作流程（JavaScript 和 C#）——SDK AudioClient

1. **Foundry Local SDK** 下载并缓存 Whisper 模型
2. `model.createAudioClient()`（JS）或 `model.GetAudioClientAsync()`（C#）创建一个 `AudioClient`
3. `audioClient.transcribe(path)`（JS）或 `audioClient.TranscribeAudioAsync(path)`（C#）在内部处理完整管道——音频预处理、编码器、解码器和标记解码
4. `AudioClient` 公开 `settings.language` 属性（设置为 `"en"` 表示英语）以指导准确转录

### 管道工作流程（Python）——ONNX Runtime

1. **Foundry Local SDK** 下载并缓存 Whisper ONNX 模型文件
2. <strong>音频预处理</strong> 将 WAV 音频转换为梅尔频谱图（80 个梅尔频率桶 × 3000 帧）
3. <strong>编码器</strong> 处理梅尔频谱图，并生成隐藏状态及交叉注意力的键/值张量
4. <strong>解码器</strong> 自回归运行，一次生成一个标记，直至生成文本结束标记
5. <strong>分词器</strong> 将输出标记 ID 解码回可读文本

### Whisper 模型变体

| 别名 | 模型 ID | 设备 | 大小 | 描述 |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU 加速（CUDA） |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU 优化（推荐多数设备使用） |

> **注意：** 与默认列出的聊天模型不同，Whisper 模型归类在 `automatic-speech-recognition` 任务下。使用 `foundry model info whisper-medium` 查看详情。

---

## 实验练习

### 练习 0 - 获取示例音频文件

本实验包含基于 Zava DIY 产品场景预制的 WAV 文件。使用内置脚本生成：

```bash
# 从仓库根目录开始 - 首先创建并激活一个 .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

此脚本会在 `samples/audio/` 生成六个 WAV 文件：

| 文件 | 场景 |
|------|----------|
| `zava-customer-inquiry.wav` | 客户询问 **Zava ProGrip 无绳电钻** |
| `zava-product-review.wav` | 客户评价 **Zava UltraSmooth 室内漆** |
| `zava-support-call.wav` | 关于 **Zava TitanLock 工具柜** 的支持电话 |
| `zava-project-planning.wav` | DIY 规划使用 **Zava EcoBoard 复合甲板材料** 建造甲板 |
| `zava-workshop-setup.wav` | 使用 **全部五款 Zava 产品** 的车间介绍 |
| `zava-full-project-walkthrough.wav` | 使用 **所有 Zava 产品** 的车库翻新完整讲解（约4分钟，适合长音频测试） |

> **提示：** 您也可以使用自己的 WAV/MP3/M4A 文件，或使用 Windows 语音录音机录音。

---

### 练习 1 - 使用 SDK 下载 Whisper 模型

由于较新版 Foundry Local 中 CLI 对 Whisper 模型支持不足，使用<strong>SDK</strong>进行下载和加载。请选择您的语言：

<details>
<summary><b>🐍 Python</b></summary>

**安装 SDK：**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# 启动服务
manager = FoundryLocalManager()
manager.start_service()

# 检查目录信息
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# 检查是否已缓存
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# 将模型加载到内存中
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

保存为 `download_whisper.py` 并运行：
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**安装 SDK：**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// 创建管理器并启动服务
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 从目录中获取模型
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

// 将模型加载到内存中
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

保存为 `download-whisper.mjs` 并运行：
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**安装 SDK：**
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

> **为什么用 SDK 而不用 CLI？** Foundry Local CLI 不支持直接下载或提供 Whisper 模型。SDK 提供可靠的方式以编程方式下载和管理音频模型。JavaScript 和 C# SDK 内置了一个处理完整转录管线的 `AudioClient`。Python 则使用 ONNX Runtime 对缓存的模型文件进行直接推理。

---

### 练习 2 - 理解 Whisper SDK

Whisper 转录根据语言采取不同方法。**JavaScript 和 C#** 在 Foundry Local SDK 中提供内置 `AudioClient`，能够通过单一方法调用完成整个流程（音频预处理、编码器、解码器和标记解码）。**Python** 使用 Foundry Local SDK 进行模型管理，利用 ONNX Runtime 直接运行编码器和解码器 ONNX 模型。

| 组件 | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK 包** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| <strong>模型管理</strong> | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| <strong>特征提取</strong> | `WhisperFeatureExtractor` + `librosa` | 由 SDK `AudioClient` 处理 | 由 SDK `AudioClient` 处理 |
| <strong>推理</strong> | `ort.InferenceSession`（编码器 + 解码器） | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| <strong>标记解码</strong> | `WhisperTokenizer` | 由 SDK `AudioClient` 处理 | 由 SDK `AudioClient` 处理 |
| <strong>语言设置</strong> | 通过解码器中的 `forced_ids` 设定 | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| <strong>输入</strong> | WAV 文件路径 | WAV 文件路径 | WAV 文件路径 |
| <strong>输出</strong> | 解码后的文本字符串 | `result.text` | `result.Text` |

> **重要提示：** 一定要在 `AudioClient` 上设置语言（例如设置为 `"en"` 表示英语）。如果不明确设置语言，模型可能会试图自动检测，从而导致输出错误或乱码。

> **SDK 模式：** Python 使用 `FoundryLocalManager(alias)` 引导，再用 `get_cache_location()` 找到 ONNX 模型文件。JavaScript 和 C# 使用 SDK 内置的 `AudioClient`——通过 `model.createAudioClient()`（JS）或 `model.GetAudioClientAsync()`（C#）获得——处理整个转录管线。详情参见 [第2部分：Foundry Local SDK 深入](part2-foundry-local-sdk.md)。

---

### 练习 3 - 构建简易转录应用

选择您喜爱的语言轨道，构建一个最小化的应用来转录音频文件。

> **支持的音频格式：** WAV、MP3、M4A。为获得最佳效果，请使用采样率为16kHz的 WAV 文件。

<details>
<summary><h3>Python 路线</h3></summary>

#### 环境准备

```bash
cd python
python -m venv venv

# 激活虚拟环境：
# Windows（PowerShell）：
venv\Scripts\Activate.ps1
# macOS：
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### 转录代码

创建文件 `foundry-local-whisper.py`：

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

# 第一步：引导 - 启动服务，下载并加载模型
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# 构建缓存的 ONNX 模型文件路径
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# 第二步：加载 ONNX 会话和特征提取器
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

# 第三步：提取梅尔频谱图特征
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# 第四步：运行编码器
enc_out = encoder.run(None, {"audio_features": input_features})
# 第一个输出是隐藏状态；其余是交叉注意力的 KV 对
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# 第五步：自回归解码
initial_tokens = [50258, 50259, 50359, 50363]  # sot，en，转录，无时间戳
input_ids = np.array([initial_tokens], dtype=np.int32)

# 清空自注意力 KV 缓存
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

    if next_token == 50257:  # 文本结束
        break
    generated.append(next_token)

    # 更新自注意力 KV 缓存
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### 运行程序

```bash
# 转录一个Zava产品场景
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# 或尝试其他：
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Python 关键点

| 方法 | 作用 |
|--------|---------|
| `FoundryLocalManager(alias)` | 启动服务，下载并加载模型 |
| `manager.get_cache_location()` | 获取缓存 ONNX 模型文件路径 |
| `WhisperFeatureExtractor.from_pretrained()` | 加载梅尔频谱特征提取器 |
| `ort.InferenceSession()` | 创建编码器和解码器的 ONNX Runtime 会话 |
| `tokenizer.decode()` | 将输出的标记 ID 解码成文本 |

</details>

<details>
<summary><h3>JavaScript 路线</h3></summary>

#### 环境准备

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### 转录代码

创建文件 `foundry-local-whisper.mjs`：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// 第一步：引导 - 创建管理器，启动服务，并加载模型
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

// 第二步：创建音频客户端并进行转录
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

> **注意：** Foundry Local SDK 通过 `model.createAudioClient()` 提供内置的 `AudioClient`，它内部处理整个 ONNX 推理管线——无需引入 `onnxruntime-node`。 请务必设置 `audioClient.settings.language = "en"` 以确保英语转录准确。

#### 运行程序

```bash
# 转录一个Zava产品场景
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# 或尝试其他：
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### JavaScript 关键点

| 方法 | 作用 |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | 创建管理器单例 |
| `await catalog.getModel(alias)` | 从目录获取模型 |
| `model.download()` / `model.load()` | 下载并加载 Whisper 模型 |
| `model.createAudioClient()` | 创建用于转录的音频客户端 |
| `audioClient.settings.language = "en"` | 设置转录语言（保证准确输出） |
| `audioClient.transcribe(path)` | 转录音频文件，返回 `{ text, duration }` |

</details>

<details>
<summary><h3>C# 路线</h3></summary>

#### 环境准备

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **注意：** C# 路线使用 `Microsoft.AI.Foundry.Local` 包，通过 `model.GetAudioClientAsync()` 内置 `AudioClient`，全流程转录在进程内完成——无需单独配置 ONNX Runtime。

#### 转录代码

替换 `Program.cs` 内容：

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

#### 运行程序

```bash
# 转录一个Zava产品场景
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# 或尝试其他：
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### C# 关键点

| 方法 | 作用 |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | 用配置初始化 Foundry Local |
| `catalog.GetModelAsync(alias)` | 从目录获取模型 |
| `model.DownloadAsync()` | 下载 Whisper 模型 |
| `model.GetAudioClientAsync()` | 获取 AudioClient（非 ChatClient） |
| `audioClient.Settings.Language = "en"` | 设置转录语言（保证准确输出） |
| `audioClient.TranscribeAudioAsync(path)` | 转录音频文件 |
| `result.Text` | 转录文本 |


> **C# vs Python/JS：** C# SDK 提供了内置的 `AudioClient`，用于通过 `model.GetAudioClientAsync()` 进行进程内转录，类似于 JavaScript SDK。Python 直接使用 ONNX Runtime 对缓存的编码器/解码器模型进行推断。

</details>

---

### 练习 4 - 批量转录所有 Zava 样本

现在你已经有了一个可用的转录应用，转录所有五个 Zava 样本文件并比较结果。

<details>
<summary><h3>Python 轨迹</h3></summary>

完整示例 `python/foundry-local-whisper.py` 已支持批量转录。无参数运行时，它会转录 `samples/audio/` 中所有的 `zava-*.wav` 文件：

```bash
cd python
python foundry-local-whisper.py
```

该示例使用 `FoundryLocalManager(alias)` 引导，然后对每个文件运行编码器和解码器的 ONNX 会话。

</details>

<details>
<summary><h3>JavaScript 轨迹</h3></summary>

完整示例 `javascript/foundry-local-whisper.mjs` 已支持批量转录。无参数运行时，它会转录 `samples/audio/` 中所有的 `zava-*.wav` 文件：

```bash
cd javascript
node foundry-local-whisper.mjs
```

示例使用 `FoundryLocalManager.create()` 和 `catalog.getModel(alias)` 初始化 SDK，然后使用 `AudioClient`（设置 `settings.language = "en"`）转录每个文件。

</details>

<details>
<summary><h3>C# 轨迹</h3></summary>

完整示例 `csharp/WhisperTranscription.cs` 已支持批量转录。无特定文件参数时，它会转录 `samples/audio/` 中所有的 `zava-*.wav` 文件：

```bash
cd csharp
dotnet run whisper
```

示例使用 `FoundryLocalManager.CreateAsync()` 和 SDK 的 `AudioClient`（设置 `Settings.Language = "en"`）进行进程内转录。

</details>

**关注点：** 将转录输出与原始文本 `samples/audio/generate_samples.py` 进行比较。Whisper 对产品名称如 “Zava ProGrip” 以及专业术语如 “brushless motor” 或 “composite decking” 的识别准确度如何？

---

### 练习 5 - 理解关键代码模式

学习 Whisper 转录与聊天补全在三种语言中的差异：

<details>
<summary><b>Python - 与聊天的关键区别</b></summary>

```python
# 聊天完成（第2-6部分）：
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# 音频转录（本部分）：
# 直接使用ONNX运行时，而不是OpenAI客户端
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... 自回归解码器循环 ...
print(tokenizer.decode(generated_tokens))
```

**关键洞察：** 聊天模型通过兼容 OpenAI 的 API（`manager.endpoint`）调用。Whisper 使用 SDK 定位缓存的 ONNX 模型文件，然后直接用 ONNX Runtime 执行推断。

</details>

<details>
<summary><b>JavaScript - 与聊天的关键区别</b></summary>

```javascript
// 聊天完成（第2-6部分）：
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// 音频转录（本部分）：
// 使用SDK内置的AudioClient
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // 始终设置语言以获得最佳效果
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**关键洞察：** 聊天模型通过兼容 OpenAI 的 API（`manager.urls[0] + "/v1"`）调用。Whisper 转录使用 SDK 的 `AudioClient`，由 `model.createAudioClient()` 获取。设置 `settings.language` 避免自动检测导致的乱码。

</details>

<details>
<summary><b>C# - 与聊天的关键区别</b></summary>

C# 方式使用 SDK 内置的 `AudioClient` 进行进程内转录：

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

**转录：**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**关键洞察：** C# 通过 `FoundryLocalManager.CreateAsync()` 获取 `AudioClient`，无需 ONNX Runtime 设置。设置 `Settings.Language` 避免自动检测导致乱码。

</details>

> **总结：** Python 使用 Foundry Local SDK 管理模型，并结合 ONNX Runtime 直接推断编码器/解码器模型。JavaScript 和 C# 都使用 SDK 内置的 `AudioClient`，流程更简洁——创建客户端，设置语言，调用 `transcribe()` / `TranscribeAudioAsync()`。务必设置 AudioClient 的语言属性以获得准确结果。

---

### 练习 6 - 实验

尝试以下修改以加深理解：

1. <strong>尝试不同的音频文件</strong> - 使用 Windows 语音记录器录音，保存为 WAV 文件，进行转录

2. <strong>比较模型变体</strong> - 如果你有 NVIDIA GPU，尝试 CUDA 变体：
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   对比转录速度与 CPU 变体。

3. <strong>添加输出格式</strong> - JSON 响应中可包含：
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **构建 REST API** - 将转录代码包装在网络服务器中：

   | 语言 | 框架 | 示例 |
   |-------|---------|---------|
   | Python | FastAPI | 使用 `@app.post("/v1/audio/transcriptions")` 及 `UploadFile` |
   | JavaScript | Express.js | 使用 `app.post("/v1/audio/transcriptions")` 及 `multer` |
   | C# | ASP.NET Minimal API | 使用 `app.MapPost("/v1/audio/transcriptions")` 及 `IFormFile` |

5. <strong>多轮对话结合转录</strong> - 结合第4部分的聊天代理：先转录音频，再将文本传给代理进行分析或总结。

---

## SDK 音频 API 参考

> **JavaScript AudioClient：**
> - `model.createAudioClient()` — 创建 `AudioClient` 实例
> - `audioClient.settings.language` — 设置转录语言（例如 `"en"`）
> - `audioClient.settings.temperature` — 控制随机性（可选）
> - `audioClient.transcribe(filePath)` — 转录文件，返回 `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — 通过回调流式转录分块
>
> **C# AudioClient：**
> - `await model.GetAudioClientAsync()` — 创建 `OpenAIAudioClient` 实例
> - `audioClient.Settings.Language` — 设置转录语言（例如 `"en"`）
> - `audioClient.Settings.Temperature` — 控制随机性（可选）
> - `await audioClient.TranscribeAudioAsync(filePath)` — 转录文件，返回带 `.Text` 的对象
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — 返回分块转录的 `IAsyncEnumerable`

> **提示：** 转录前务必设置语言属性。若不设置，Whisper 模型会尝试自动检测，可能产生乱码（单个替代字符而非文本）。

---

## 比较：聊天模型 vs. Whisper

| 方面 | 聊天模型（第3-7部分） | Whisper - Python | Whisper - JS / C# |
|--------|-----------------------|------------------|-------------------|
| <strong>任务类型</strong> | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| <strong>输入</strong> | 文本消息（JSON） | 音频文件（WAV/MP3/M4A） | 音频文件（WAV/MP3/M4A） |
| <strong>输出</strong> | 生成文本（流式） | 转录文本（完整） | 转录文本（完整） |
| **SDK 包** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk`（JS）/ `Microsoft.AI.Foundry.Local`（C#） |
| **API 方法** | `client.chat.completions.create()` | ONNX Runtime 直接调用 | `audioClient.transcribe()`（JS）/ `audioClient.TranscribeAudioAsync()`（C#） |
| <strong>语言设置</strong> | 无 | 解码器提示标记 | `audioClient.settings.language`（JS）/ `audioClient.Settings.Language`（C#） |
| <strong>流式支持</strong> | 是 | 否 | `transcribeStreaming()`（JS）/ `TranscribeAudioStreamingAsync()`（C#） |
| <strong>隐私优势</strong> | 代码/数据本地 | 音频数据本地 | 音频数据本地 |

---

## 关键总结

| 概念 | 你学到了什么 |
|---------|--------------|
| **Whisper 设备端** | 语音转文本完全本地运行，适合在设备端转录 Zava 客户通话及产品评价 |
| **SDK AudioClient** | JavaScript 和 C# SDK 提供内置 `AudioClient`，一次调用完成整个转录流程 |
| <strong>语言设置</strong> | 始终设置 AudioClient 语言（例如 `"en"`）——否则自动检测可能产生乱码 |
| **Python** | 使用 `foundry-local-sdk` 管理模型 + `onnxruntime` + `transformers` + `librosa` 直接 ONNX 推断 |
| **JavaScript** | 使用 `foundry-local-sdk` 的 `model.createAudioClient()` — 设置 `settings.language` 后调用 `transcribe()` |
| **C#** | 使用 `Microsoft.AI.Foundry.Local` 的 `model.GetAudioClientAsync()` — 设置 `Settings.Language` 后调用 `TranscribeAudioAsync()` |
| <strong>流式支持</strong> | JS 与 C# SDK 也支持 `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` 逐块输出 |
| **CPU 优化** | CPU 版本（3.05 GB）适用于任何无 GPU 的 Windows 设备 |
| <strong>隐私优先</strong> | 非常适合在设备端保留 Zava 客户互动和专有产品数据 |

---

## 资源

| 资源 | 链接 |
|-------|-------|
| Foundry Local 文档 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 参考 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper 模型 | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local 官网 | [foundrylocal.ai](https://foundrylocal.ai) |

---

## 下一步

继续阅读 [第10部分：使用自定义或 Hugging Face 模型](part10-custom-models.md)，将 Hugging Face 的模型编译并运行于 Foundry Local。