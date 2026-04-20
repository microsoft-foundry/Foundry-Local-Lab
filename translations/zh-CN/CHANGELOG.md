# 变更记录 — Foundry Local 研讨会

本研讨会的所有重要变更都记录于下文。

---

## 2026-03-11 — 第 12 & 13 部分，Web UI，Whisper 重写，WinML/QNN 修复和验证

### 新增
- **第 12 部分：为 Zava 创意写作器构建 Web UI** — 新实验指导 (`labs/part12-zava-ui.md`)，包含流式 NDJSON、浏览器 `ReadableStream`、实时代理状态徽章和实时文章文本流练习
- **第 13 部分：研讨会完成** — 新总结实验 (`labs/part13-workshop-complete.md`)，包含所有 12 部分回顾、进一步思路和资源链接
- **Zava UI 前端：** `zava-creative-writer-local/ui/index.html`，`style.css`，`app.js` — 共享的原生 HTML/CSS/JS 浏览器界面，供所有三个后端使用
- **JavaScript HTTP 服务器：** `zava-creative-writer-local/src/javascript/server.mjs` — 新的 Express 风格 HTTP 服务器，封装协调器以供浏览器访问
- **C# ASP.NET Core 后端：** `zava-creative-writer-local/src/csharp-web/Program.cs` 和 `ZavaCreativeWriterWeb.csproj` — 新的最简 API 项目，用于服务 UI 和流式 NDJSON
- **音频样本生成器：** `samples/audio/generate_samples.py` — 离线 TTS 脚本，使用 `pyttsx3` 生成带有 Zava 主题的 WAV 文件供第 9 部分使用
- **音频样本：** `samples/audio/zava-full-project-walkthrough.wav` — 新的较长音频样本用于转录测试
- **验证脚本：** `validate-npu-workaround.ps1` — 自动 PowerShell 脚本，用于验证所有 C# 示例中的 NPU/QNN 变通方案
- **Mermaid 图示 SVG 文件：** `images/part12-architecture.svg`，`part12-message-types.svg`，`part12-streaming-sequence.svg`
- **WinML 跨平台支持：** 所有 3 个 C# `.csproj` 文件 (`csharp/csharp.csproj`，`zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`，`zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) 现在使用条件 TFM 和互斥的包引用实现跨平台支持。在 Windows 平台：`net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML`（包含 QNN EP 插件的超集）。在非 Windows 平台：`net9.0` TFM + `Microsoft.AI.Foundry.Local`（基础 SDK）。Zava 项目中硬编码的 `win-arm64` RID 被自动检测替代。一个传递依赖的变通方案排除了具有损坏 win-arm64 引用的 `Microsoft.ML.OnnxRuntime.Gpu.Linux` 本地资产。之前所有 7 个 C# 文件中的 try/catch NPU 变通方案已被移除。

### 变更
- **第 9 部分（Whisper）：** 重大重写 — JavaScript 现使用 SDK 内置的 `AudioClient` (`model.createAudioClient()`) 替代手动 ONNX Runtime 推理；更新架构描述、比较表和管道图以反映 JS/C# 版 `AudioClient` 方法与 Python ONNX Runtime 方法的区别
- **第 11 部分：** 更新导航链接（现指向第 12 部分）；添加渲染的 SVG 工具调用流程和序列图示
- **第 10 部分：** 更新导航为通过第 12 部分路由，而非结束研讨会
- **Python Whisper (`foundry-local-whisper.py`)：** 扩展额外音频样本并改进错误处理
- **JavaScript Whisper (`foundry-local-whisper.mjs`)：** 重写为使用 `model.createAudioClient()` 及 `audioClient.transcribe()`，替代手动 ONNX Runtime 会话
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`)：** 更新以同时服务静态 UI 文件和 API
- **Zava C# 控制台 (`zava-creative-writer-local/src/csharp/Program.cs`)：** 移除 NPU 变通方案（现由 WinML 包处理）
- **README.md：** 添加第 12 部分包含代码示例表和后端新增内容；添加第 13 部分；更新学习目标和项目结构
- **KNOWN-ISSUES.md：** 移除已解决的第 7 号问题（C# SDK NPU 模型变体—现由 WinML 包处理）。剩余问题重新编号为 #1–#6。更新环境详情为 .NET SDK 10.0.104
- **AGENTS.md：** 更新项目结构树，新增 `zava-creative-writer-local` 相关条目（`ui/`，`csharp-web/`，`server.mjs`）；更新 C# 关键包和条件 TFM 详情
- **labs/part2-foundry-local-sdk.md：** 更新 `.csproj` 示例，展示完整跨平台模式，含条件 TFM、互斥包引用及说明注释

### 验证
- 三个 C# 项目 (`csharp`、`ZavaCreativeWriter`、`ZavaCreativeWriterWeb`) 在 Windows ARM64 上均能成功构建
- 聊天示例 (`dotnet run chat`)：模型通过 WinML/QNN 以 `phi-3.5-mini-instruct-qnn-npu:1` 加载 — NPU 变体直接加载无 CPU 回退
- 代理示例 (`dotnet run agent`)：运行完备的多轮对话，退出码 0
- Foundry Local CLI v0.8.117 和 SDK v0.9.0 运行于 .NET SDK 9.0.312

---

## 2026-03-11 — 代码修复，模型清理，Mermaid 图示与验证

### 修复
- **全部 21 个代码示例（7 Python，7 JavaScript，7 C#）：** 在退出时添加 `model.unload()` / `unload_model()` / `model.UnloadAsync()` 清理，解决 OGA 内存泄露警告（已知问题 #4）
- **csharp/WhisperTranscription.cs：** 用 `FindSamplesDirectory()` 替代脆弱的 `AppContext.BaseDirectory` 相对路径，递归向上查找 `samples/audio` 目录，确保定位可靠（已知问题 #7）
- **csharp/csharp.csproj：** 用自动检测和回退的 `$(NETCoreSdkRuntimeIdentifier)` 替代硬编码的 `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>`，使 `dotnet run` 可在任何平台无需 `-r` 参数运行（[Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)）

### 变更
- **第 8 部分：** 将由评估驱动的迭代循环 ASCII 箱形图转换为渲染 SVG 图片
- **第 10 部分：** 将编译流水线图 ASCII 箭头转换为渲染 SVG 图片
- **第 11 部分：** 将工具调用流程及序列图转换为渲染 SVG 图片
- **第 10 部分：** “研讨会完成！”章节移至第 11 部分（最终实验）；新增“下一步骤”链接替代
- **KNOWN-ISSUES.md：** 全面重新验证所有问题针对 CLI v0.8.117。移除已解决：OGA 内存泄露（已加入清理）、Whisper 路径（FindSamplesDirectory）、HTTP 500 持续推理（不可复现，[#494](https://github.com/microsoft/Foundry-Local/issues/494)）、tool_choice 限制（现支持 `"required"` 及针对 qwen2.5-0.5b 的具体函数调用）。更新 JavaScript Whisper 问题 — 所有文件现返回空/二进制输出（v0.9.x 回归，严重性升至重大）。更新 #4 C# RID，增加自动检测变通方案及链接 [#497](https://github.com/microsoft/Foundry-Local/issues/497)。剩余 7 个未解决问题。
- **javascript/foundry-local-whisper.mjs：** 修复清理变量名错误（`whisperModel` → `model`）

### 验证
- Python：`foundry-local.py`，`foundry-local-rag.py`，`foundry-local-tool-calling.py` — 成功运行并清理
- JavaScript：`foundry-local.mjs`，`foundry-local-rag.mjs`，`foundry-local-tool-calling.mjs` — 成功运行并清理
- C#：`dotnet build` 成功，0 警告，0 错误（net9.0 目标）
- 全部 7 个 Python 文件均通过 `py_compile` 语法检查
- 全部 7 个 JavaScript 文件通过 `node --check` 语法验证

---

## 2026-03-10 — 第 11 部分：工具调用，SDK API 扩展及模型覆盖

### 新增
- **第 11 部分：使用本地模型进行工具调用** — 新实验指导 (`labs/part11-tool-calling.md`)，包含 8 个练习，涵盖工具模式、多轮流程、多次工具调用、自定义工具、ChatClient 工具调用和 `tool_choice`
- **Python 示例：** `python/foundry-local-tool-calling.py` — 使用 OpenAI SDK 调用 `get_weather`/`get_population` 工具
- **JavaScript 示例：** `javascript/foundry-local-tool-calling.mjs` — 使用 SDK 原生 `ChatClient` (`model.createChatClient()`) 进行工具调用
- **C# 示例：** `csharp/ToolCalling.cs` — 使用 OpenAI C# SDK 的 `ChatTool.CreateFunctionTool()` 进行工具调用
- **第 2 部分第 7 练习：** 原生 `ChatClient` — `model.createChatClient()`（JS）和 `model.GetChatClientAsync()`（C#），作为 OpenAI SDK 替代方案
- **第 2 部分第 8 练习：** 模型变体与硬件选择 — `selectVariant()`，`variants`，NPU 变体表（7 个模型）
- **第 2 部分第 9 练习：** 模型升级与目录刷新 — `is_model_upgradeable()`，`upgrade_model()`，`updateModels()`
- **第 2 部分第 10 练习：** 推理模型 — `phi-4-mini-reasoning` 包含 `<think>` 标签解析示例
- **第 3 部分第 4 练习：** `createChatClient` 作为 OpenAI SDK 替代，含流式回调模式文档
- **AGENTS.md：** 新增工具调用、ChatClient 和推理模型编码规范

### 变更
- **第 1 部分：** 扩展模型目录 — 新增 phi-4-mini-reasoning，gpt-oss-20b，phi-4，qwen2.5-7b，qwen2.5-coder-7b，whisper-large-v3-turbo
- **第 2 部分：** 扩展 API 参考表 — 新增 `createChatClient`、`createAudioClient`、`removeFromCache`、`selectVariant`、`variants`、`isLoaded`、`stopWebService`、`is_model_upgradeable`、`upgrade_model`、`httpx_client`、`getModels`、`getCachedModels`、`getLoadedModels`、`updateModels`、`GetModelVariantAsync`、`UpdateModelsAsync`
- **第 2 部分：** 练习编号 7-9 → 10-13，以便新增练习
- **第 3 部分：** 更新关键要点表，新增原生 ChatClient 内容
- **README.md：** 添加第 11 部分代码示例表；新增学习目标 #11；更新项目结构树
- **csharp/Program.cs：** 在 CLI 路由中添加 `toolcall` 选项，更新帮助文本

---

## 2026-03-09 — SDK v0.9.0 更新，英式英语及验证通过

### 变更
- **所有代码示例（Python，JavaScript，C#）：** 更新为 Foundry Local SDK v0.9.0 API — 修复 `await catalog.getModel()`（之前缺少 `await`），更新 `FoundryLocalManager` 初始化模式，修正端点发现逻辑
- **所有实验指导（第 1-10 部分）：** 转为英式英语（colour、catalogue、optimised 等）
- **所有实验指导：** 更新 SDK 代码示例，匹配 v0.9.0 API 界面
- **所有实验指导：** 更新 API 参考表和练习代码块
- **JavaScript 关键修复：** 补充缺失 `await` 于 `catalog.getModel()` — 返回了 `Promise` 而非 `Model` 对象，导致下游静默失败

### 验证
- 所有 Python 示例均成功运行于 Foundry Local 服务
- 所有 JavaScript 示例成功运行（Node.js 18+）
- C# 项目成功构建并运行于 .NET 9.0（从 net8.0 SDK 程序集向前兼容）
- 研讨会共修改并验证 29 个文件

---

## 文件索引

| 文件 | 最后更新 | 描述 |
|------|---------|-------|
| `labs/part1-getting-started.md` | 2026-03-10 | 扩展模型目录 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | 新增练习 7-10，扩展 API 表 |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | 新增练习 4（ChatClient），更新要点 |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + 英式英语 |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + 英式英语 |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + 英式英语 |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + 英式英语 |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid 图表 |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + 英式英语 |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid 图表，Workshop Complete 移至第 11 部分 |
| `labs/part11-tool-calling.md` | 2026-03-11 | 新实验室，Mermaid 图表，Workshop Complete 部分 |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | 新增：工具调用示例 |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | 新增：工具调用示例 |
| `csharp/ToolCalling.cs` | 2026-03-10 | 新增：工具调用示例 |
| `csharp/Program.cs` | 2026-03-10 | 新增 `toolcall` CLI 命令 |
| `README.md` | 2026-03-10 | 第 11 部分，项目结构 |
| `AGENTS.md` | 2026-03-10 | 工具调用 + ChatClient 约定 |
| `KNOWN-ISSUES.md` | 2026-03-11 | 移除已解决的问题 #7，剩余 6 个未解决问题 |
| `csharp/csharp.csproj` | 2026-03-11 | 跨平台 TFM，WinML/base SDK 条件引用 |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | 跨平台 TFM，自动检测 RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | 跨平台 TFM，自动检测 RID |
| `csharp/BasicChat.cs` | 2026-03-11 | 移除 NPU 异常捕获解决方案 |
| `csharp/SingleAgent.cs` | 2026-03-11 | 移除 NPU 异常捕获解决方案 |
| `csharp/MultiAgent.cs` | 2026-03-11 | 移除 NPU 异常捕获解决方案 |
| `csharp/RagPipeline.cs` | 2026-03-11 | 移除 NPU 异常捕获解决方案 |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | 移除 NPU 异常捕获解决方案 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | 跨平台 .csproj 示例 |
| `AGENTS.md` | 2026-03-11 | 更新 C# 包和 TFM 详细信息 |
| `CHANGELOG.md` | 2026-03-11 | 此文件 |