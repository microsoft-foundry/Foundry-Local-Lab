<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local 研讨会 - 在设备上构建 AI 应用

一个动手研讨会，教你如何在自己的机器上运行语言模型，并使用 [Foundry Local](https://foundrylocal.ai) 和 [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) 构建智能应用。

> **什么是 Foundry Local？** Foundry Local 是一个轻量级运行时，允许你完全在硬件上下载、管理和服务语言模型。它暴露出一个<strong>兼容 OpenAI 的 API</strong>，任何支持 OpenAI 的工具或 SDK 都可以连接——无需云账户。

---

## 学习目标

完成此研讨会后，你将能够：

| # | 目标 |
|---|-----------|
| 1 | 安装 Foundry Local 并使用 CLI 管理模型 |
| 2 | 掌握 Foundry Local SDK API 以编程方式管理模型 |
| 3 | 使用 Python、JavaScript 和 C# SDK 连接本地推理服务器 |
| 4 | 构建基于检索增强生成（RAG）的流水线，实现数据驱动的答案输出 |
| 5 | 创建带有持久指令和角色设定的 AI 代理 |
| 6 | 协调多代理工作流及反馈循环 |
| 7 | 探索生产级封顶应用 - Zava 创意写作助手 |
| 8 | 使用黄金数据集和 LLM 评分构建评估框架 |
| 9 | 使用 Whisper 转录音频——通过 Foundry Local SDK 在设备上执行语音转文字 |
| 10 | 使用 ONNX Runtime GenAI 和 Foundry Local 编译并运行自定义或 Hugging Face 模型 |
| 11 | 通过工具调用模式让本地模型调用外部函数 |
| 12 | 为 Zava 创意写作助手构建基于浏览器的实时流式 UI |

---

## 先决条件

| 要求 | 详情 |
|-------------|---------|
| <strong>硬件</strong> | 至少 8 GB 内存（推荐 16 GB）；支持 AVX2 的 CPU 或受支持的 GPU |
| <strong>操作系统</strong> | Windows 10/11 (x64/ARM)，Windows Server 2025，或 macOS 13+ |
| **Foundry Local CLI** | 通过 `winget install Microsoft.FoundryLocal`（Windows）或 `brew tap microsoft/foundrylocal && brew install foundrylocal`（macOS）安装。详见[入门指南](https://learn.microsoft.com/en-us/azure/foundry-local/get-started)。 |
| <strong>语言运行时</strong> | **Python 3.9+** 和/或 **.NET 9.0+** 和/或 **Node.js 18+** |
| **Git** | 用于克隆此仓库 |

---

## 快速开始

```bash
# 1. 克隆仓库
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. 验证已安装 Foundry Local
foundry model list              # 列出可用模型
foundry model run phi-3.5-mini  # 开始交互聊天

# 3. 选择你的语言课程（完整设置见第二部分实验）
```

| 语言 | 快速开始 |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## 研讨会部分

### 第1部分：Foundry Local 入门

**实验指南：**[`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- 什么是 Foundry Local 及其工作原理
- 在 Windows 和 macOS 上安装 CLI
- 探索模型：列出、下载、运行
- 理解模型别名和动态端口

---

### 第2部分：Foundry Local SDK 深入

**实验指南：**[`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- 为什么应用开发推荐使用 SDK 而非 CLI
- Python、JavaScript 和 C# 的完整 SDK API 参考
- 服务管理、目录浏览、模型生命周期（下载、加载、卸载）
- 快速入门模式：Python 构造函数引导，JavaScript 的 `init()`，C# 的 `CreateAsync()`
- `FoundryModelInfo` 元数据、别名与硬件优化模型选择

---

### 第3部分：SDK 与 API

**实验指南：**[`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- 从 Python、JavaScript 和 C# 连接 Foundry Local
- 通过 Foundry Local SDK 编程管理服务
- 通过兼容 OpenAI 的 API 进行流式聊天补全
- 各语言的 SDK 方法参考

**代码示例：**

| 语言 | 文件 | 说明 |
|----------|------|-------------|
| Python | `python/foundry-local.py` | 基础流式聊天 |
| C# | `csharp/BasicChat.cs` | 使用 .NET 的流式聊天 |
| JavaScript | `javascript/foundry-local.mjs` | 使用 Node.js 的流式聊天 |

---

### 第4部分：检索增强生成（RAG）

**实验指南：**[`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- 什么是 RAG 及其重要性
- 构建内存中知识库
- 基于关键词重叠的检索与评分
- 组合根植系统提示
- 在设备上运行完整 RAG 流水线

**代码示例：**

| 语言 | 文件 |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 第5部分：构建 AI 代理

**实验指南：**[`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- 什么是 AI 代理（区别于直接调用 LLM）
- `ChatAgent` 模式和 Microsoft Agent Framework
- 系统指令、角色扮演和多轮对话
- 代理输出的结构化（JSON）

**代码示例：**

| 语言 | 文件 | 说明 |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | 使用 Agent Framework 的单代理 |
| C# | `csharp/SingleAgent.cs` | 单代理（ChatAgent 模式） |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | 单代理（ChatAgent 模式） |

---

### 第6部分：多代理工作流

**实验指南：**[`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- 多代理流水线示例：研究员→写手→编辑
- 顺序编排与反馈循环
- 共享配置与结构化交接
- 设计你自己的多代理工作流

**代码示例：**

| 语言 | 文件 | 说明 |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | 三代理流水线 |
| C# | `csharp/MultiAgent.cs` | 三代理流水线 |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 三代理流水线 |

---

### 第7部分：Zava 创意写作助手 - 封顶应用

**实验指南：**[`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 一个生产级多代理应用，包含4个专业代理
- 由评估者驱动的顺序流水线反馈循环
- 流式输出、产品目录搜索、结构化 JSON 交接
- Python（FastAPI）、JavaScript（Node.js CLI）、C#（.NET 控制台）全实现

**代码示例：**

| 语言 | 目录 | 说明 |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | 带编排器的 FastAPI Web 服务 |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI 应用 |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 控制台应用 |

---

### 第8部分：评估驱动开发

**实验指南：**[`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- 使用黄金数据集为 AI 代理构建系统化评估框架
- 基于规则的检查（长度、关键词覆盖、禁用词）+ LLM 评分法
- 并排比较提示变体及综合记分卡
- 扩展第7部分中的 Zava 编辑代理为离线测试套件
- Python、JavaScript 和 C# 线路

**代码示例：**

| 语言 | 文件 | 说明 |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | 评估框架 |
| C# | `csharp/AgentEvaluation.cs` | 评估框架 |
| JavaScript | `javascript/foundry-local-eval.mjs` | 评估框架 |

---

### 第9部分：使用 Whisper 进行语音转录

**实验指南：**[`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- 使用本地运行的 OpenAI Whisper 进行语音转文字转录
- 隐私优先的音频处理——音频不会离开设备
- Python、JavaScript 和 C# 线路，通过 `client.audio.transcriptions.create()`（Python/JS）和 `AudioClient.TranscribeAudioAsync()`（C#）
- 包含 Zava 主题示例音频文件供实践使用

**代码示例：**

| 语言 | 文件 | 说明 |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper 语音转录 |
| C# | `csharp/WhisperTranscription.cs` | Whisper 语音转录 |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper 语音转录 |

> **注意：** 本实验使用 **Foundry Local SDK** 编程方式下载并加载 Whisper 模型，然后将音频发送到本地兼容 OpenAI 的端点进行转录。Whisper 模型（`whisper`）列在 Foundry Local 目录中，完全在设备上运行——不需要云 API 密钥或网络访问。

---

### 第10部分：使用自定义或 Hugging Face 模型

**实验指南：**[`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- 使用 ONNX Runtime GenAI 模型构建器将 Hugging Face 模型编译为优化的 ONNX 格式
- 硬件特定编译（CPU、NVIDIA GPU、DirectML、WebGPU）和量化（int4、fp16、bf16）
- 创建 Foundry Local 聊天模板配置文件
- 将编译模型添加到 Foundry Local 缓存
- 通过 CLI、REST API 和 OpenAI SDK 运行自定义模型
- 参考示例：Qwen/Qwen3-0.6B 的端到端编译

---

### 第11部分：本地模型的工具调用

**实验指南：**[`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- 使本地模型能够调用外部函数（工具/函数调用）
- 使用 OpenAI 函数调用格式定义工具模式
- 处理多轮工具调用对话流程
- 本地执行工具调用并返回结果给模型
- 为工具调用场景选择合适模型（Qwen 2.5，Phi-4-mini）
- 使用 SDK 的原生 `ChatClient` 进行工具调用（JavaScript）

**代码示例：**

| 语言 | 文件 | 说明 |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | 天气/人口工具调用 |
| C# | `csharp/ToolCalling.cs` | 使用 .NET 的工具调用 |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | 使用 ChatClient 的工具调用 |

---

### 第12部分：为 Zava 创意写作助手构建 Web UI

**实验指南：**[`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- 为 Zava 创意写作助手添加浏览器前端
- 使用 Python（FastAPI）、JavaScript（Node.js HTTP）、C#（ASP.NET Core）提供共享 UI 服务
- 在浏览器端通过 Fetch API 和 ReadableStream 处理流式 NDJSON
- 实时显示代理状态徽章和文章文本流

**代码（共享 UI）：**

| 文件 | 说明 |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | 页面布局 |
| `zava-creative-writer-local/ui/style.css` | 样式 |
| `zava-creative-writer-local/ui/app.js` | 流读取和 DOM 更新逻辑 |

**后端新增内容：**

| 语言 | 文件 | 说明 |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | 更新以服务静态 UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | 包装编排器的新 HTTP 服务器 |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 新建 ASP.NET Core 最小 API 项目 |

---

### 第13部分：研讨会完成
**实验指南：** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 总结你在所有12个部分中构建的所有内容
- 进一步扩展你的应用程序的想法
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
| Foundry Local 网站 | [foundrylocal.ai](https://foundrylocal.ai) |
| 模型目录 | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| 入门指南 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 参考 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent 框架 | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## 许可证

本研讨会材料仅供教育用途。

---

**祝你构建愉快！🚀**