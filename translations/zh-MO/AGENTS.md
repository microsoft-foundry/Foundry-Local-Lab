# 編碼代理說明

此檔案為在此存放庫中運作的 AI 編碼代理（GitHub Copilot、Copilot Workspace、Codex 等）提供上下文。

## 專案概覽

這是一個使用 [Foundry Local](https://foundrylocal.ai) 建立 AI 應用程式的<strong>實作工作坊</strong> — 一個透過 OpenAI 相容 API，在裝置上完整下載、管理與服務語言模型的輕量級運行時。工作坊包含分步實驗室指南與可執行的 Python、JavaScript 和 C# 範例代碼。

## 存放庫結構

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

## 語言及框架細節

### Python
- **位置：** `python/`、`zava-creative-writer-local/src/api/`
- **相依：** `python/requirements.txt`、`zava-creative-writer-local/src/api/requirements.txt`
- **主要套件：** `foundry-local-sdk`、`openai`、`agent-framework-foundry-local`、`fastapi`、`uvicorn`
- **最低版本：** Python 3.9 以上
- **執行：** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **位置：** `javascript/`、`zava-creative-writer-local/src/javascript/`
- **相依：** `javascript/package.json`、`zava-creative-writer-local/src/javascript/package.json`
- **主要套件：** `foundry-local-sdk`、`openai`
- **模組系統：** ES 模組（`.mjs` 檔案，`"type": "module"`）
- **最低版本：** Node.js 18 以上
- **執行：** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **位置：** `csharp/`、`zava-creative-writer-local/src/csharp/`
- **專案檔案：** `csharp/csharp.csproj`、`zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **主要套件：** `Microsoft.AI.Foundry.Local`（非 Windows）、`Microsoft.AI.Foundry.Local.WinML`（Windows — 含 QNN EP 的超集）、`OpenAI`、`Microsoft.Agents.AI.OpenAI`
- **目標：** .NET 9.0（條件 TFM：Windows 上為 `net9.0-windows10.0.26100`，其他為 `net9.0`）
- **執行：** `cd csharp && dotnet run [chat|rag|agent|multi]`

## 編碼慣例

### 一般
- 所有範例代碼皆為<strong>自含單一檔案範例</strong> — 無共用的工具函式庫或抽象層。
- 每個範例在安裝其相依後能獨立執行。
- API 金鑰皆設定為 `"foundry-local"` — Foundry Local 將此用作佔位符。
- 基本 URL 使用 `http://localhost:<port>/v1` — 埠為動態，透過 SDK 在執行時發現（JavaScript 中為 `manager.urls[0]`，Python 中為 `manager.endpoint`）。
- Foundry Local SDK 負責服務啟動與端點發現；優先使用 SDK 模式避免硬編碼埠號。

### Python
- 使用 `openai` SDK，配合 `OpenAI(base_url=..., api_key="not-required")`。
- 使用 `foundry_local` 的 `FoundryLocalManager()` 管理 SDK 服務生命週期。
- 串流：透過 `for chunk in stream:` 迭代 `stream` 物件。
- 範例檔不含型別註記（為工作坊學員保持簡潔範例）。

### JavaScript
- ES 模組語法：`import ... from "..."`。
- 使用來自 `"openai"` 的 `OpenAI` 和來自 `"foundry-local-sdk"` 的 `FoundryLocalManager`。
- SDK 初始化流程：`FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`。
- 串流：`for await (const chunk of stream)`。
- 廣泛使用頂層 `await`。

### C#
- 啟用 Nullable、隱含 using、.NET 9。
- 使用 `FoundryLocalManager.StartServiceAsync()` 管理 SDK 生命週期。
- 串流：`CompleteChatStreaming()` 配合 `foreach (var update in completionUpdates)`。
- 主要執行檔 `csharp/Program.cs` 為 CLI 路由器，呼叫靜態 `RunAsync()` 方法。

### 工具呼叫
- 僅有特定模型支援工具呼叫：**Qwen 2.5** 系列（`qwen2.5-*`）與 **Phi-4-mini**（`phi-4-mini`）。
- 工具 schema 遵循 OpenAI 函數呼叫 JSON 格式（`type: "function"`、`function.name`、`function.description`、`function.parameters`）。
- 對話採多回合模式：使用者 → 助理（tool_calls）→ 工具（結果）→ 助理（最終答案）。
- 工具結果訊息中的 `tool_call_id` 必須匹配模型工具呼叫傳回的 `id`。
- Python 直接使用 OpenAI SDK；JavaScript 使用 SDK 原生的 `ChatClient` (`model.createChatClient()`)；C# 使用 OpenAI SDK 搭配 `ChatTool.CreateFunctionTool()`。

### ChatClient（原生 SDK 客戶端）
- JavaScript：`model.createChatClient()` 傳回帶有 `completeChat(messages, tools?)` 和 `completeStreamingChat(messages, callback)` 的 `ChatClient`。
- C#：`model.GetChatClientAsync()` 傳回標準 `ChatClient`，無需另外引用 OpenAI NuGet 套件。
- Python 無原生 ChatClient — 請使用 OpenAI SDK 搭配 `manager.endpoint` 及 `manager.api_key`。
- **重要：** JavaScript 的 `completeStreamingChat` 採用<strong>回呼模式</strong>，非 async 迭代。

### 推理模型
- `phi-4-mini-reasoning` 在最終答案前，將思考內容包裹於 `<think>...</think>` 標籤中。
- 需要時可解析標籤分離推理與答案。

## 實驗室指南

實驗室檔案置於 `labs/`，皆為 Markdown。遵循一致結構：
- 標誌標頭圖片
- 標題及目標提示框
- 概述、學習目標、先決條件
- 概念解說章節，附有圖解
- 編號練習，包含程式代碼區塊及預期輸出
- 總結表格、重點摘錄、延伸閱讀
- 導覽連結至下一部分

編輯實驗室內容時：
- 保留既有 Markdown 格式及章節階層。
- 程式碼區塊須明確標註語言（`python`, `javascript`, `csharp`, `bash`, `powershell`）。
- Shell 指令附上 bash 與 PowerShell 版本因作業系統差異。
- 使用 `> **Note:**`、`> **Tip:**`、`> **Troubleshooting:**` 提示格式。
- 表格使用管線 (`| Header | Header |`) 格式。

## 編譯與測試指令

| 操作 | 指令 |
|--------|---------|
| **Python 範例** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS 範例** | `cd javascript && npm install && node <script>.mjs` |
| **C# 範例** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS（網頁）** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C#（網頁）** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`，`foundry model run <model>`，`foundry service status` |
| <strong>產生圖表</strong> | `npx mmdc -i <input>.mmd -o <output>.svg`（須事先全域安裝 `npm install -g @mermaid-js/mermaid-cli`） |

## 外部相依

- **Foundry Local CLI** 必須安裝於開發者電腦（`winget install Microsoft.FoundryLocal` 或 `brew install foundrylocal`）。
- **Foundry Local 服務** 本機執行，並於動態埠口提供 OpenAI 相容的 REST API。
- 執行任何範例不需雲端服務、API 金鑰或 Azure 訂閱。
- 第 10 部分（自訂模型）另需 `onnxruntime-genai` 並會從 Hugging Face 下載模型權重。

## 不應提交之檔案

`.gitignore` 應排除（且多數已排除）：
- `.venv/` — Python 虛擬環境
- `node_modules/` — npm 相依套件
- `models/` — 編譯後 ONNX 模型（大型二進位檔，由第 10 部分產生）
- `cache_dir/` — Hugging Face 下載快取
- `.olive-cache/` — Microsoft Olive 工作目錄
- `samples/audio/*.wav` — 生成的音訊範例（可用 `python samples/audio/generate_samples.py` 重新產生）
- 標準 Python 建構產物（`__pycache__/`、`*.egg-info/`、`dist/` 等）

## 授權

MIT — 請參閱 `LICENSE`。