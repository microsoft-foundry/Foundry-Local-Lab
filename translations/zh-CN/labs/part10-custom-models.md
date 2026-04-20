![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第10部分：使用 Foundry Local 自定义模型或 Hugging Face 模型

> **目标：** 将 Hugging Face 模型编译成 Foundry Local 所需的优化 ONNX 格式，配置聊天模板，将其添加到本地缓存，并通过 CLI、REST API 和 OpenAI SDK 运行推理。

## 概述

Foundry Local 附带了精选的预编译模型目录，但您并不限于此列表。任何基于 Transformer 的语言模型，无论是托管于 [Hugging Face](https://huggingface.co/) 还是以 PyTorch / Safetensors 格式存储在本地，都可以编译成优化的 ONNX 模型，并通过 Foundry Local 提供服务。

编译流程使用 **ONNX Runtime GenAI Model Builder**，这是一个包含在 `onnxruntime-genai` 包中的命令行工具。模型构建器负责繁重工作：下载源权重，将其转换为 ONNX 格式，应用量化（int4、fp16、bf16），并生成 Foundry Local 期望的配置文件（包括聊天模板和分词器）。

在本实验中，您将编译来自 Hugging Face 的 **Qwen/Qwen3-0.6B**，将其注册到 Foundry Local 上，并完全在您的设备上与其对话。

---

## 学习目标

完成本实验后，您将能够：

- 解释为什么自定义模型编译有用以及何时需要使用
- 安装 ONNX Runtime GenAI 模型构建器
- 使用单条命令将 Hugging Face 模型编译为优化的 ONNX 格式
- 理解关键的编译参数（执行提供程序、精度）
- 创建 `inference_model.json` 聊天模板配置文件
- 将编译好的模型添加到 Foundry Local 缓存中
- 使用 CLI、REST API 和 OpenAI SDK 运行自定义模型的推理

---

## 前置条件

| 需求 | 详情 |
|-------------|---------|
| **Foundry Local CLI** | 已安装并加入您的 `PATH` 环境变量（[第1部分](part1-getting-started.md)） |
| **Python 3.10+** | ONNX Runtime GenAI 模型构建器所需 |
| **pip** | Python 包管理器 |
| <strong>磁盘空间</strong> | 至少 5 GB 可用空间用于存放源文件和编译模型文件 |
| **Hugging Face 账号** | 部分模型需要您接受许可证协议后才能下载。Qwen3-0.6B 使用 Apache 2.0 许可证，免费可用。 |

---

## 环境配置

模型编译需要多个大型 Python 包（PyTorch、ONNX Runtime GenAI、Transformers）。建议创建一个专用的虚拟环境，避免干扰系统 Python 或其它项目。

```bash
# 从代码仓库根目录
python -m venv .venv
```

激活环境：

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

升级 pip 以避免依赖解析问题：

```bash
python -m pip install --upgrade pip
```

> **提示：** 如果您之前的实验已创建 `.venv`，可以复用该环境。只需确保它已被激活即可继续。

---

## 概念：编译流水线

Foundry Local 需要 ONNX 格式并带有 ONNX Runtime GenAI 配置的模型。大多数开源 Hugging Face 模型以 PyTorch 或 Safetensors 权重形式发布，因此需要一步转换。

![自定义模型编译流程](../../../images/custom-model-pipeline.svg)

### 模型构建器做了什么？

1. <strong>下载</strong> Hugging Face 中的源模型（或从本地路径读取）。
2. <strong>转换</strong> PyTorch / Safetensors 权重为 ONNX 格式。
3. <strong>量化</strong> 模型为较小精度（例如 int4），以减少内存使用并提高吞吐量。
4. <strong>生成</strong> ONNX Runtime GenAI 配置文件 (`genai_config.json`)，聊天模板 (`chat_template.jinja`) 以及所有分词器文件，确保 Foundry Local 能加载并提供模型服务。

### ONNX Runtime GenAI Model Builder 与 Microsoft Olive

您可能会看到关于 **Microsoft Olive** 作为模型优化的替代工具的提及。两者都能生成 ONNX 模型，但用途不同、权衡不同：

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| <strong>包</strong> | `onnxruntime-genai` | `olive-ai` |
| <strong>主要用途</strong> | 转换和量化生成式 AI 模型，用于 ONNX Runtime GenAI 推理 | 端到端模型优化框架，支持多后端及硬件目标 |
| <strong>易用性</strong> | 单条命令 —— 一步转换 + 量化 | 工作流程式 —— 可配置多遍流水线，支持 YAML/JSON |
| <strong>输出格式</strong> | ONNX Runtime GenAI 格式（适配 Foundry Local） | 通用 ONNX、ONNX Runtime GenAI 或其他格式，取决于流水线 |
| <strong>硬件支持</strong> | CPU、CUDA、DirectML、TensorRT RTX、WebGPU | CPU、CUDA、DirectML、TensorRT、高通 QNN 等 |
| <strong>量化选项</strong> | int4、int8、fp16、fp32 | int4（AWQ、GPTQ、RTN）、int8、fp16，外加图优化、逐层调优 |
| <strong>模型范围</strong> | 生成式 AI 模型（大型语言模型、特定语言模型） | 任意可转换为 ONNX 的模型（视觉、NLP、音频、多模态） |
| <strong>最适合</strong> | 快速单模型编译，适合本地推理 | 生产流水线，需精细优化控制 |
| <strong>依赖资源</strong> | 中等（PyTorch、Transformers、ONNX Runtime） | 较大（包含 Olive 框架，按流水线可选额外组件） |
| **Foundry Local 集成** | 直接兼容，产出即用 | 需 `--use_ort_genai` 标记和额外配置 |

> **为何本实验使用模型构建器：** 对于编译单个 Hugging Face 模型并注册到 Foundry Local，模型构建器是最简单且最可靠的方法。它能在一条命令中生成 Foundry Local 精确需要的输出格式。如果将来您需要高级优化功能，如精度感知量化、图修复或多遍调优，Olive 是强大的选择。详情请参阅 [Microsoft Olive 文档](https://microsoft.github.io/Olive/)。

---

## 实验练习

### 练习 1：安装 ONNX Runtime GenAI 模型构建器

安装包含模型构建器工具的 ONNX Runtime GenAI 包：

```bash
pip install onnxruntime-genai
```

验证安装是否成功，通过查看模型构建器是否可用：

```bash
python -m onnxruntime_genai.models.builder --help
```

您应该能看到帮助信息，列出诸如 `-m`（模型名）、`-o`（输出路径）、`-p`（精度）、`-e`（执行提供程序）等参数。

> **注意：** 模型构建器依赖 PyTorch、Transformers 和其它多个包，安装可能需要几分钟时间。

---

### 练习 2：为 CPU 编译 Qwen3-0.6B

运行以下命令，从 Hugging Face 下载 Qwen3-0.6B 模型并编译，目标为 CPU 推理，使用 int4 量化：

**macOS / Linux:**
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```

**Windows (PowerShell):**
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```

#### 每个参数的作用

| 参数 | 作用 | 使用的值 |
|-----------|---------|------------|
| `-m` | Hugging Face 模型 ID 或本地目录路径 | `Qwen/Qwen3-0.6B` |
| `-o` | 编译后 ONNX 模型保存目录 | `models/qwen3` |
| `-p` | 编译时应用的量化精度 | `int4` |
| `-e` | ONNX Runtime 执行提供程序（目标硬件） | `cpu` |
| `--extra_options hf_token=false` | 跳过 Hugging Face 认证（公开模型可用） | `hf_token=false` |

> **需要多长时间？** 编译时间取决于硬件和模型大小。Qwen3-0.6B 在现代 CPU 上使用 int4 量化大约需要 5 到 15 分钟。更大模型耗时更长。

命令完成后，您应看到 `models/qwen3` 目录，包含编译模型文件。验证输出：

```bash
ls models/qwen3
```

您会看到以下文件：
- `model.onnx` 和 `model.onnx.data` — 编译后的模型权重
- `genai_config.json` — ONNX Runtime GenAI 配置
- `chat_template.jinja` — 模型聊天模板（自动生成）
- `tokenizer.json`、`tokenizer_config.json` — 分词器文件
- 其他词汇表和配置文件

---

### 练习 3：为 GPU 编译（可选）

如果您有带 CUDA 支持的 NVIDIA GPU，可以编译一个 GPU 优化版本，加速推理：

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **注意：** GPU 编译需要 `onnxruntime-gpu` 包和可用的 CUDA 环境。若无则模型构建器会报错。您可以跳过本练习，继续使用 CPU 版本。

#### 硬件特定编译参考

| 目标 | 执行提供程序 (`-e`) | 推荐精度 (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` 或 `int4` |
| DirectML（Windows GPU） | `dml` | `fp16` 或 `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### 精度折衷

| 精度 | 大小 | 速度 | 质量 |
|-----------|------|-------|---------|
| `fp32` | 最大 | 最慢 | 最高准确度 |
| `fp16` | 大 | 快（GPU） | 非常好准确度 |
| `int8` | 小 | 快 | 轻微准确度损失 |
| `int4` | 最小 | 最快 | 中等准确度损失 |

对于本地开发，CPU 上的 `int4` 在速度和资源使用间达最佳平衡。生产环境建议使用 CUDA GPU 上的 `fp16`。

---

### 练习 4：创建聊天模板配置

模型构建器会自动生成输出目录下的 `chat_template.jinja` 文件和 `genai_config.json` 文件。但 Foundry Local 还需要一个 `inference_model.json` 文件，告诉它如何格式化提示。该文件定义模型名称及对用户消息进行特殊 token 包装的提示模板。

#### 第1步：检查编译输出

列出编译模型目录的内容：

```bash
ls models/qwen3
```

您应看到文件如：
- `model.onnx` 和 `model.onnx.data` — 编译模型权重
- `genai_config.json` — ONNX Runtime GenAI 配置（自动生成）
- `chat_template.jinja` — 模型聊天模板（自动生成）
- `tokenizer.json`、`tokenizer_config.json` — 分词器文件
- 多个其他配置和词汇表文件

#### 第2步：生成 inference_model.json 文件

创建 Python 脚本 `generate_chat_template.py` <strong>放在仓库根目录</strong>（即包含 `models/` 文件夹的目录）：

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# 构建一个最简对话以提取聊天模板
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# 构建 inference_model.json 结构
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```

在仓库根目录运行该脚本：

```bash
python generate_chat_template.py
```

> **注意：** `transformers` 包是随 `onnxruntime-genai` 依赖安装的。如果出现 `ImportError`，请先运行 `pip install transformers`。

该脚本在 `models/qwen3` 目录生成 `inference_model.json` 文件。该文件告诉 Foundry Local 如何用正确特殊 tokens 包装用户输入，以匹配 Qwen3 模型。

> **重要：** `inference_model.json` 中 `"Name"` 字段（本脚本示例为 `qwen3-0.6b`）是您后续命令和 API 调用中使用的<strong>模型别名</strong>。若修改此名称，请同步更新练习6至10中的模型名称。

#### 第3步：验证配置文件

打开 `models/qwen3/inference_model.json`，确认其包含 `Name` 字段和带有 `assistant`、`prompt` 键的 `PromptTemplate` 对象。提示模板应包括诸如 `<|im_start|>` 和 `<|im_end|>` 的特殊 tokens（具体 tokens 依模型聊天模板而定）。

> **手动替代方案：** 若不愿运行脚本，也可手动创建该文件。关键要求是 `prompt` 字段包含模型完整聊天模板，且 `{Content}` 占位符用于放置用户消息。

---

### 练习 5：验证模型目录结构
模型构建器将所有编译后的文件直接放入您指定的输出目录。请确认最终结构是否正确：

```bash
ls models/qwen3
```

该目录应包含以下文件：

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **注意：** 与一些其他编译工具不同，模型构建器不会创建嵌套子目录。所有文件都直接放在输出文件夹中，这正是 Foundry Local 预期的结构。

---

### 练习 6：将模型添加到 Foundry Local 缓存

通过将目录添加到缓存中，告诉 Foundry Local 在哪里可以找到您编译的模型：

```bash
foundry cache cd models/qwen3
```

确认模型是否显示在缓存中：

```bash
foundry cache ls
```

您应该会看到您的自定义模型与之前缓存的任何模型（如 `phi-3.5-mini` 或 `phi-4-mini`）一同列出。

---

### 练习 7：使用 CLI 运行自定义模型

启动一个交互式聊天会话，使用您新编译的模型（`qwen3-0.6b` 别名来自您在 `inference_model.json` 中设置的 `Name` 字段）：

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` 标志显示额外的诊断信息，当首次测试自定义模型时非常有用。如果模型加载成功，您将看到一个交互式提示。尝试输入几条消息：

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

输入 `exit` 或按 `Ctrl+C` 结束会话。

> **故障排除：** 如果模型加载失败，请检查以下内容：
> - `genai_config.json` 文件是否由模型构建器生成。
> - `inference_model.json` 文件是否存在且为有效的 JSON 格式。
> - ONNX 模型文件是否位于正确的目录中。
> - 您的可用内存是否足够（Qwen3-0.6B int4 大约需要 1 GB）。
> - Qwen3 是一个推理模型，会生成 `<think>` 标签。如果您看到响应前面带有 `<think>...</think>`，这是正常行为。可以调整 `inference_model.json` 中的提示模板以抑制思考输出。

---

### 练习 8：通过 REST API 查询自定义模型

如果您在练习 7 中退出了交互式会话，模型可能已不再加载。请先启动 Foundry Local 服务并加载模型：

```bash
foundry service start
foundry model load qwen3-0.6b
```

检查服务运行的端口：

```bash
foundry service status
```

然后发送请求（如果实际端口不同，请将 `5273` 替换为您自己的端口）：

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows 注意：** 上述 `curl` 命令使用的是 bash 语法。在 Windows 上，请改用 PowerShell 的 `Invoke-RestMethod` 命令，如下所示。

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### 练习 9：使用 OpenAI SDK 调用自定义模型

您可以使用与调用内置模型完全相同的 OpenAI SDK 代码连接到您的自定义模型（参见[第3部分](part3-sdk-and-apis.md)）。唯一的区别是模型名称。

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local 不验证 API 密钥
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry本地不会验证API密钥
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **关键点：** 因为 Foundry Local 提供了兼容 OpenAI 的 API，所有适用于内置模型的代码也适用于您的自定义模型。您只需更改 `model` 参数即可。

---

### 练习 10：使用 Foundry Local SDK 测试自定义模型

在之前的实验中，您使用 Foundry Local SDK 启动服务、发现端点并自动管理模型，您也可以使用完全相同的方式来操作自定义编译的模型。SDK 会处理服务启动和端点发现，因此您的代码无需硬编码 `localhost:5273`。

> **注意：** 运行以下示例前，请确保已安装 Foundry Local SDK：
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** 添加 `Microsoft.AI.Foundry.Local` 和 `OpenAI` NuGet 包
>
> 请将每个脚本文件保存在 <strong>仓库根目录</strong>（即与您的 `models/` 文件夹相同的目录）下。

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# 第1步：启动Foundry本地服务并加载自定义模型
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# 第2步：检查自定义模型的缓存
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# 第3步：将模型加载到内存中
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# 第4步：使用SDK发现的端点创建OpenAI客户端
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# 第5步：发送流式聊天补全请求
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

运行它：

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// 第一步：启动 Foundry 本地服务
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 第二步：从目录中获取自定义模型
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// 第三步：将模型加载到内存中
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// 第四步：使用 SDK 发现的端点创建 OpenAI 客户端
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// 第五步：发送流式聊天完成请求
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

运行它：

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **关键点：** Foundry Local SDK 会动态发现端点，因此您永远不需要硬编码端口号。这是生产应用的推荐做法。您的自定义编译模型通过 SDK 的行为与内置目录模型完全相同。

---

## 选择要编译的模型

本实验中以 Qwen3-0.6B 作为示例模型，因为它体积小、编译快速，并且在 Apache 2.0 许可证下免费提供。不过，您也可以编译许多其他模型。以下是一些建议：

| 模型 | Hugging Face ID | 参数数量 | 许可证 | 备注 |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | 非常小，编译快，适合测试 |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | 更好质量，仍快速编译 |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | 质量强，需要更多内存 |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | 需要在 Hugging Face 接受许可协议 |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | 高质量，下载大，编译时间长 |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | 已在 Foundry Local 目录中（便于比较） |

> **许可证提醒：** 在使用模型前，请务必检查 Hugging Face 上的模型许可证。某些模型（如 Llama）需要您接受许可协议并用 `huggingface-cli login` 认证后才能下载。

---

## 概念：何时使用自定义模型

| 场景 | 为什么要编译自己的模型？ |
|----------|----------------------|
| <strong>目录没有你需要的模型</strong> | Foundry Local 目录是精选的。如果您想要的模型不在列表中，就自己编译。 |
| <strong>微调模型</strong> | 如果您在特定领域数据上微调了模型，需要编译自己的权重。 |
| <strong>特定量化要求</strong> | 您可能需要与目录默认不同的精度或量化策略。 |
| <strong>更新的模型发布</strong> | 当 Hugging Face 发布新模型，但 Foundry Local 目录尚未收录时，自己编译可立即使用。 |
| <strong>研究和实验</strong> | 在正式选择生产模型前，尝试不同架构、大小和配置的本地模型。 |

---

## 总结

本实验您学习了如何：

| 步骤 | 您的操作 |
|------|-------------|
| 1 | 安装 ONNX Runtime GenAI 模型构建器 |
| 2 | 将 Hugging Face 上的 `Qwen/Qwen3-0.6B` 编译为优化的 ONNX 模型 |
| 3 | 创建 `inference_model.json` 聊天模板配置文件 |
| 4 | 将编译模型添加到 Foundry Local 缓存 |
| 5 | 通过 CLI 使用自定义模型进行交互式聊天 |
| 6 | 通过兼容 OpenAI 的 REST API 查询模型 |
| 7 | 使用 Python、JavaScript 和 C# 的 OpenAI SDK 连接模型 |
| 8 | 通过 Foundry Local SDK 端到端测试自定义模型 |

关键点是，**任何基于 Transformer 的模型，只要编译成 ONNX 格式，就能通过 Foundry Local 运行**。兼容 OpenAI 的 API 意味着您现有的应用代码无需更改，只需替换模型名称。

---

## 关键要点

| 概念 | 详情 |
|---------|--------|
| ONNX Runtime GenAI 模型构建器 | 一条命令将 Hugging Face 模型转换成带量化的 ONNX 格式 |
| ONNX 格式 | Foundry Local 需要带 ONNX Runtime GenAI 配置的 ONNX 模型 |
| 聊天模板 | `inference_model.json` 文件告诉 Foundry Local 如何格式化给定模型的提示 |
| 硬件目标 | 可针对 CPU、NVIDIA GPU（CUDA）、DirectML（Windows GPU）或 WebGPU 编译 |
| 量化 | 低精度（int4）体积更小、速度更快但精度稍降；fp16 在 GPU 上保持高质量 |
| API 兼容性 | 自定义模型使用与内置模型相同的兼容 OpenAI API |
| Foundry Local SDK | 自动处理服务启动、端点发现和模型加载，支持目录和自定义模型 |

---

## 进一步阅读

| 资源 | 链接 |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local 自定义模型指南 | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 模型家族 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive 文档 | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## 后续步骤

继续学习 [第11部分：本地模型工具调用](part11-tool-calling.md)，了解如何让您的本地模型调用外部函数。

[← 第9部分：Whisper 语音转录](part9-whisper-voice-transcription.md) | [第11部分：工具调用 →](part11-tool-calling.md)