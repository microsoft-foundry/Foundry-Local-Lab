# 编码代理指南

此文件为在此仓库中工作的 AI 编码代理（GitHub Copilot、Copilot Workspace、Codex 等）提供上下文。

## 项目概述

这是一个使用 [Foundry Local](https://foundrylocal.ai) 构建 AI 应用的<strong>实战工作坊</strong> — 这是一个轻量级运行时，通过兼容 OpenAI 的 API 完全在本地设备下载、管理和服务语言模型。工作坊包含逐步实验指导和可运行的 Python、JavaScript 及 C# 代码示例。

## 仓库结构

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## 语言与框架详情

### Python
- **位置：** `python/`，`zava-creative-writer-local/src/api/`
- **依赖项：** `python/requirements.txt`，`zava-creative-writer-local/src/api/requirements.txt`
- **关键包：** `foundry-local-sdk`，`openai`，`agent-framework-foundry-local`，`fastapi`，`uvicorn`
- **最低版本：** Python 3.9+
- **运行方式：** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **位置：** `javascript/`，`zava-creative-writer-local/src/javascript/`
- **依赖项：** `javascript/package.json`，`zava-creative-writer-local/src/javascript/package.json`
- **关键包：** `foundry-local-sdk`，`openai`
- **模块系统：** ES 模块（`.mjs` 文件，`"type": "module"`）
- **最低版本：** Node.js 18+
- **运行方式：** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **位置：** `csharp/`，`zava-creative-writer-local/src/csharp/`
- **项目文件：** `csharp/csharp.csproj`，`zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **关键包：** `Microsoft.AI.Foundry.Local`（非 Windows），`Microsoft.AI.Foundry.Local.WinML`（Windows — 包含 QNN EP），`OpenAI`，`Microsoft.Agents.AI.OpenAI`
- **目标平台：** .NET 9.0（条件 TFM：Windows 上为 `net9.0-windows10.0.26100`，其他为 `net9.0`）
- **运行方式：** `cd csharp && dotnet run [chat|rag|agent|multi]`

## 编码规范

### 通用
- 所有代码示例均为<strong>自包含单文件示例</strong> — 无共享实用库或抽象。
- 每个示例安装各自依赖后独立运行。
- API 密钥始终设为 `"foundry-local"` — Foundry Local 使用该占位符。
- 基础 URL 使用 `http://localhost:<port>/v1` — 端口动态且通过 SDK 在运行时发现（JS 中为 `manager.urls[0]`，Python 中为 `manager.endpoint`）。
- Foundry Local SDK 处理服务启动和端点发现；优先使用 SDK 模式而非硬编码端口。

### Python
- 使用 `openai` SDK 和 `OpenAI(base_url=..., api_key="not-required")`。
- 使用 `foundry_local` 的 `FoundryLocalManager()` 管理 SDK 服务生命周期。
- 流式处理：用 `for chunk in stream:` 迭代 `stream` 对象。
- 示例文件中不使用类型注解（简洁方便学习）。

### JavaScript
- ES 模块语法：`import ... from "..."`。
- 使用 `"openai"` 的 `OpenAI` 和 `"foundry-local-sdk"` 的 `FoundryLocalManager`。
- SDK 初始化流程：`FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`。
- 流式处理：`for await (const chunk of stream)`。
- 全文使用顶级 `await`。

### C#
- 启用可空类型，隐式 using，.NET 9。
- 使用 `FoundryLocalManager.StartServiceAsync()` 管理 SDK 生命周期。
- 流式处理：`CompleteChatStreaming()` 中使用 `foreach (var update in completionUpdates)`。
- 主程序 `csharp/Program.cs` 作为 CLI 路由，分派到静态 `RunAsync()` 方法。

### 工具调用
- 仅部分模型支持工具调用：**Qwen 2.5** 系列（`qwen2.5-*`）和 **Phi-4-mini**（`phi-4-mini`）。
- 工具 schema 遵循 OpenAI 函数调用 JSON 格式（`type: "function"`，`function.name`，`function.description`，`function.parameters`）。
- 对话模式为多轮：用户 → 助理（tool_calls）→ 工具（结果）→ 助理（最终答案）。
- 工具结果消息中的 `tool_call_id` 必须匹配模型工具调用中的 `id`。
- Python 直用 OpenAI SDK；JavaScript 用 SDK 原生 `ChatClient`（`model.createChatClient()`）；C# 用 OpenAI SDK 配合 `ChatTool.CreateFunctionTool()`。

### ChatClient（原生 SDK 客户端）
- JavaScript：`model.createChatClient()` 返回具有 `completeChat(messages, tools?)` 和 `completeStreamingChat(messages, callback)` 的 `ChatClient`。
- C#：`model.GetChatClientAsync()` 返回的标准 `ChatClient` 可无需引用 OpenAI NuGet 包使用。
- Python 无原生 ChatClient — 用 OpenAI SDK 配合 `manager.endpoint` 和 `manager.api_key`。
- **重要：** JavaScript 的 `completeStreamingChat` 使用<strong>回调模式</strong>，非异步迭代。

### 推理模型
- `phi-4-mini-reasoning` 在最终答案前用 `<think>...</think>` 标签封装推理过程。
- 需要时解析标签以分离推理与答案部分。

## 实验指导

实验文件位于 `labs/`，采用 Markdown 格式，结构统一：
- Logo 头图
- 标题及目标提示
- 概览、学习目标、前置条件
- 概念讲解部分含图示
- 按序号的练习，含代码块和预期输出
- 总结表格、要点回顾、拓展阅读
- 指向下一部分的导航链接

编辑实验内容时：
- 保持现有 Markdown 格式和章节层级。
- 代码块请指定语言（`python`、`javascript`、`csharp`、`bash`、`powershell`）。
- 对于操作系统相关的 Shell 命令，提供 Bash 和 PowerShell 两个版本。
- 使用 `> **Note:**`、`> **Tip:**`、`> **Troubleshooting:**` 提示样式。
- 表格使用 `| Header | Header |` 管道格式。

## 构建与测试命令

| 操作 | 命令 |
|--------|---------|
| **Python 示例** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS 示例** | `cd javascript && npm install && node <script>.mjs` |
| **C# 示例** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS（Web）** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C#（Web）** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`，`foundry model run <model>`，`foundry service status` |
| <strong>生成图表</strong> | `npx mmdc -i <input>.mmd -o <output>.svg`（需要先全局安装 `npm install -g @mermaid-js/mermaid-cli`）|

## 外部依赖

- **Foundry Local CLI** 必须安装在开发者机器上（`winget install Microsoft.FoundryLocal` 或 `brew install foundrylocal`）。
- **Foundry Local 服务** 本地运行，暴露兼容 OpenAI 的 REST API，端口动态分配。
- 运行示例无需云服务、API 密钥或 Azure 订阅。
- 第 10 部分（自定义模型）还需 `onnxruntime-genai`，并从 Hugging Face 下载模型权重。

## 不应提交的文件

`.gitignore` 需排除（大多数已包含）：
- `.venv/` — Python 虚拟环境
- `node_modules/` — npm 依赖
- `models/` — 编译后的 ONNX 模型输出（二进制大文件，10 部分自动生成）
- `cache_dir/` — Hugging Face 模型下载缓存
- `.olive-cache/` — Microsoft Olive 工作目录
- `samples/audio/*.wav` — 生成的音频样本（通过 `python samples/audio/generate_samples.py` 重新生成）
- 标准 Python 构建产物（`__pycache__/`、`*.egg-info/`、`dist/` 等）

## 许可

MIT — 请参见 `LICENSE`。