<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local 工作坊 - 在设备上构建 AI 应用

一个动手实践的工作坊，教你如何在自己的机器上运行语言模型，并使用 [Foundry Local](https://foundrylocal.ai) 和 [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) 构建智能应用。

> **什么是 Foundry Local？** Foundry Local 是一个轻量级运行时，允许你完全在本地硬件上下载、管理和服务语言模型。它暴露一个 **兼容 OpenAI 的 API**，因此任何支持 OpenAI 的工具或 SDK 都能连接，无需云账户。

### 🌐 多语言支持

#### 通过 GitHub Action 支持（自动且始终最新）

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[阿拉伯语](../ar/README.md) | [孟加拉语](../bn/README.md) | [保加利亚语](../bg/README.md) | [缅甸语](../my/README.md) | [中文（简体）](./README.md) | [中文（繁体，香港）](../zh-HK/README.md) | [中文（繁体，澳门）](../zh-MO/README.md) | [中文（繁体，台湾）](../zh-TW/README.md) | [克罗地亚语](../hr/README.md) | [捷克语](../cs/README.md) | [丹麦语](../da/README.md) | [荷兰语](../nl/README.md) | [爱沙尼亚语](../et/README.md) | [芬兰语](../fi/README.md) | [法语](../fr/README.md) | [德语](../de/README.md) | [希腊语](../el/README.md) | [希伯来语](../he/README.md) | [印地语](../hi/README.md) | [匈牙利语](../hu/README.md) | [印尼语](../id/README.md) | [意大利语](../it/README.md) | [日语](../ja/README.md) | [卡纳达语](../kn/README.md) | [高棉语](../km/README.md) | [韩语](../ko/README.md) | [立陶宛语](../lt/README.md) | [马来语](../ms/README.md) | [马拉雅拉姆语](../ml/README.md) | [马拉地语](../mr/README.md) | [尼泊尔语](../ne/README.md) | [尼日利亚皮钦语](../pcm/README.md) | [挪威语](../no/README.md) | [波斯语（法尔西语）](../fa/README.md) | [波兰语](../pl/README.md) | [葡萄牙语（巴西）](../pt-BR/README.md) | [葡萄牙语（葡萄牙）](../pt-PT/README.md) | [旁遮普语（古鲁穆奇文）](../pa/README.md) | [罗马尼亚语](../ro/README.md) | [俄语](../ru/README.md) | [塞尔维亚语（西里尔字母）](../sr/README.md) | [斯洛伐克语](../sk/README.md) | [斯洛文尼亚语](../sl/README.md) | [西班牙语](../es/README.md) | [斯瓦希里语](../sw/README.md) | [瑞典语](../sv/README.md) | [他加禄语（菲律宾语）](../tl/README.md) | [泰米尔语](../ta/README.md) | [泰卢固语](../te/README.md) | [泰语](../th/README.md) | [土耳其语](../tr/README.md) | [乌克兰语](../uk/README.md) | [乌尔都语](../ur/README.md) | [越南语](../vi/README.md)

> **偏好本地克隆？**
>
> 本仓库包含 50 多种语言的翻译，这会显著增加下载大小。若要克隆时不包含翻译，可使用稀疏检出：
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> 这样可以让你以更快的速度下载，获取完成课程所需的所有内容。
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## 学习目标

完成本工作坊后，你将能够：

| # | 目标 |
|---|-------|
| 1 | 安装 Foundry Local 并使用 CLI 管理模型 |
| 2 | 精通 Foundry Local SDK API，实现程序化模型管理 |
| 3 | 使用 Python、JavaScript 和 C# SDK 连接本地推理服务器 |
| 4 | 构建基于检索增强生成（RAG）的管道，使回答基于你的数据 |
| 5 | 创建带有持久指令和角色设定的 AI 代理 |
| 6 | 编排带反馈循环的多代理工作流 |
| 7 | 探索生产级压轴应用——Zava 创意写作助手 |
| 8 | 基于黄金数据集和大模型评分构建评估框架 |
| 9 | 使用 Whisper 完成音频转录——通过 Foundry Local SDK 在设备上实现语音转文本 |
| 10 | 使用 ONNX Runtime GenAI 和 Foundry Local 编译并运行自定义或 Hugging Face 模型 |
| 11 | 使本地模型通过工具调用模式调用外部函数 |
| 12 | 为 Zava 创意写作助手构建基于浏览器的实时流式 UI |

---

## 先决条件

| 要求 | 详情 |
|-------|-------|
| <strong>硬件</strong> | 最少 8 GB 内存（推荐 16 GB）；AVX2 支持的 CPU 或支持的 GPU |
| <strong>操作系统</strong> | Windows 10/11（x64/ARM）、Windows Server 2025 或 macOS 13 及以上版本 |
| **Foundry Local CLI** | 通过 `winget install Microsoft.FoundryLocal`（Windows）或 `brew tap microsoft/foundrylocal && brew install foundrylocal`（macOS）安装。详情请见 [入门指南](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) 。 |
| <strong>语言运行时</strong> | **Python 3.9+** 和/或 **.NET 9.0+** 和/或 **Node.js 18+** |
| **Git** | 用于克隆本仓库 |

---

## 快速开始

```bash
# 1. 克隆仓库
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. 验证 Foundry Local 是否已安装
foundry model list              # 列出可用模型
foundry model run phi-3.5-mini  # 开始交互式聊天

# 3. 选择你的语言路线（完整设置见第2部分实验）
```

| 语言 | 快速开始 |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## 工作坊部分

### 第一部分：Foundry Local 入门

**实验指南：** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- 什么是 Foundry Local 以及它的工作原理
- 在 Windows 和 macOS 上安装 CLI
- 探索模型 —— 列表、下载、运行
- 理解模型别名和动态端口

---

### 第二部分：Foundry Local SDK 深入

**实验指南：** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- 为什么开发应用时使用 SDK 而非 CLI
- Python、JavaScript 和 C# 的完整 SDK API 参考
- 服务管理、目录浏览、模型生命周期（下载、加载、卸载）
- 快速启动模式：Python 构造函数引导、JavaScript 的 `init()`、C# 的 `CreateAsync()`
- `FoundryModelInfo` 元数据、别名及硬件优化模型选择

---

### 第三部分：SDK 与 API

**实验指南：** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- 从 Python、JavaScript 和 C# 连接 Foundry Local
- 使用 Foundry Local SDK 程序化管理服务
- 通过兼容 OpenAI 的 API 进行流式聊天生成
- 各语言 SDK 方法参考

**代码示例：**

| 语言 | 文件 | 说明 |
|----------|------|-------|
| Python | `python/foundry-local.py` | 基础流式聊天 |
| C# | `csharp/BasicChat.cs` | 使用 .NET 的流式聊天 |
| JavaScript | `javascript/foundry-local.mjs` | 使用 Node.js 的流式聊天 |

---

### 第四部分：检索增强生成（RAG）

**实验指南：** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- 什么是 RAG 及其重要性
- 构建内存知识库
- 关键词重叠检索及评分
- 组合有具体依据的系统提示
- 在设备上运行完整的 RAG 管道

**代码示例：**

| 语言 | 文件 |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 第五部分：构建 AI 代理

**实验指南：** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- 什么是 AI 代理（与直接调用大模型的区别）
- `ChatAgent` 模式及 Microsoft Agent Framework
- 系统指令、角色设定与多轮对话
- 代理的结构化输出（JSON）

**代码示例：**

| 语言 | 文件 | 说明 |
|----------|------|-------|
| Python | `python/foundry-local-with-agf.py` | 单代理配合 Agent Framework |
| C# | `csharp/SingleAgent.cs` | 单代理（ChatAgent 模式） |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | 单代理（ChatAgent 模式） |

---

### 第六部分：多代理工作流

**实验指南：** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- 多代理管道：研究员 → 作家 → 编辑
- 顺序编排与反馈循环
- 共享配置与结构化交接
- 设计你自己的多代理工作流

**代码示例：**

| 语言 | 文件 | 说明 |
|----------|------|-------|
| Python | `python/foundry-local-multi-agent.py` | 三代理管道 |
| C# | `csharp/MultiAgent.cs` | 三代理管道 |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 三代理管道 |

---

### 第七部分：Zava 创意写作助手 - 压轴应用

**实验指南：** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 一个具备 4 个专业代理的生产级多代理应用
- 评估者驱动的反馈循环顺序管道
- 流式输出、产品目录搜索、结构化 JSON 交接
- Python (FastAPI)、JavaScript (Node.js CLI)、C# (.NET 控制台) 的完整实现

**代码示例：**

| 语言 | 目录 | 说明 |
|----------|-----------|-------|
| Python | `zava-creative-writer-local/src/api/` | 带编排器的 FastAPI Web 服务 |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI 应用 |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 控制台应用 |

---

### 第八部分：评估驱动开发

**实验指南：** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- 使用黄金数据集构建 AI 代理的系统评估框架
- 规则检查（长度、关键词覆盖、禁止词）+ 大模型评判评分
- 不同提示变体的并排对比和总评分卡
- 将第七部分中的 Zava 编辑代理模式扩展为离线测试套件
- Python、JavaScript 和 C# 三个分支

**代码示例：**

| 语言 | 文件 | 说明 |
|----------|------|-------|
| Python | `python/foundry-local-eval.py` | 评估框架 |
| C# | `csharp/AgentEvaluation.cs` | 评估框架 |
| JavaScript | `javascript/foundry-local-eval.mjs` | 评估框架 |

---

### 第九部分：使用 Whisper 进行语音转写

**实验指南：** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- 使用本地运行的 OpenAI Whisper 进行语音转文本转录
- 隐私优先的音频处理——音频从不离开您的设备
- 使用 Python、JavaScript 和 C#，通过 `client.audio.transcriptions.create()`（Python/JS）和 `AudioClient.TranscribeAudioAsync()`（C#）接口
- 包含 Zava 主题的示例音频文件供动手练习

**代码示例：**

| 语言 | 文件 | 描述 |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper 语音转录 |
| C# | `csharp/WhisperTranscription.cs` | Whisper 语音转录 |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper 语音转录 |

> **注意：** 本实验使用 **Foundry Local SDK** 编程式地下载并加载 Whisper 模型，然后将音频发送到本地兼容 OpenAI 的端点进行转录。Whisper 模型（`whisper`）在 Foundry Local 目录中列出，完全本地运行——无需云 API 密钥或网络访问。

---

### 第10部分：使用自定义或 Hugging Face 模型

**实验指南：** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- 使用 ONNX Runtime GenAI 模型构建器将 Hugging Face 模型编译为优化的 ONNX 格式
- 针对硬件进行特定编译（CPU、NVIDIA GPU、DirectML、WebGPU）和量化（int4、fp16、bf16）
- 创建 Foundry Local 的聊天模板配置文件
- 将编译模型添加到 Foundry Local 缓存
- 通过 CLI、REST API 及 OpenAI SDK 运行自定义模型
- 参考示例：端到端编译 Qwen/Qwen3-0.6B

---

### 第11部分：使用本地模型进行工具调用

**实验指南：** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- 使本地模型能够调用外部函数（工具/函数调用）
- 使用 OpenAI 函数调用格式定义工具架构
- 处理多轮工具调用的对话流程
- 本地执行工具调用并将结果返回给模型
- 针对工具调用场景选择合适模型（Qwen 2.5，Phi-4-mini）
- 使用 SDK 原生的 `ChatClient` 进行工具调用（JavaScript）

**代码示例：**

| 语言 | 文件 | 描述 |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | 使用天气/人口等工具进行工具调用 |
| C# | `csharp/ToolCalling.cs` | 使用 .NET 进行工具调用 |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | 使用 ChatClient 进行工具调用 |

---

### 第12部分：为 Zava 创意写作器构建 Web UI

**实验指南：** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- 为 Zava 创意写作器添加基于浏览器的前端
- 从 Python（FastAPI）、JavaScript（Node.js HTTP）和 C#（ASP.NET Core）提供共享 UI
- 在浏览器中使用 Fetch API 和 ReadableStream 处理流式 NDJSON
- 实时代理状态徽章和实时文章文本流

**代码（共享 UI）：**

| 文件 | 描述 |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | 页面布局 |
| `zava-creative-writer-local/ui/style.css` | 样式 |
| `zava-creative-writer-local/ui/app.js` | 流读取器和 DOM 更新逻辑 |

**后端新增内容：**

| 语言 | 文件 | 描述 |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | 更新以提供静态 UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | 新增封装协调器的 HTTP 服务器 |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 新增 ASP.NET Core 最小 API 项目 |

---

### 第13部分：工作坊总结

**实验指南：** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 总结您在 12 个部分中构建的全部内容
- 拓展应用的更多思路
- 资源和文档链接

---

## 项目结构

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## 资源

| 资源 | 链接 |
|----------|------|
| Foundry Local 官网 | [foundrylocal.ai](https://foundrylocal.ai) |
| 模型目录 | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| 入门指南 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 参考 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft 代理框架 | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## 许可证

本工作坊资料仅供教学用途。

---

**祝构建愉快！🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**免责声明**：  
本文件使用 AI 翻译服务 [Co-op Translator](https://github.com/Azure/co-op-translator) 进行翻译。虽然我们力求准确，但请注意自动翻译可能包含错误或不准确之处。原始语言的文档应视为权威来源。对于重要信息，建议进行专业人工翻译。我们不对因使用此翻译而产生的任何误解或错误解释承担责任。
<!-- CO-OP TRANSLATOR DISCLAIMER END -->