# 編碼代理指引

此檔案為在此儲存庫中運行的 AI 編碼代理（GitHub Copilot、Copilot Workspace、Codex 等）提供背景資訊。

## 專案概述

這是一個使用 [Foundry Local](https://foundrylocal.ai) 建構 AI 應用程式的<strong>實作工作坊</strong> — 一個輕量的執行時環境，能夠全程於裝置上下載、管理並透過兼容 OpenAI 的 API 提供語言模型。工作坊包含逐步實驗指南以及 Python、JavaScript 與 C# 的可執行範例程式碼。

## 儲存庫結構

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

## 語言與框架細節

### Python
- **位置：** `python/`、`zava-creative-writer-local/src/api/`
- **相依套件：** `python/requirements.txt`、`zava-creative-writer-local/src/api/requirements.txt`
- **主要套件：** `foundry-local-sdk`、`openai`、`agent-framework-foundry-local`、`fastapi`、`uvicorn`
- **最低版本：** Python 3.9+
- **執行：** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **位置：** `javascript/`、`zava-creative-writer-local/src/javascript/`
- **相依套件：** `javascript/package.json`、`zava-creative-writer-local/src/javascript/package.json`
- **主要套件：** `foundry-local-sdk`、`openai`
- **模組系統：** ES 模組（`.mjs` 檔案，`"type": "module"`）
- **最低版本：** Node.js 18+
- **執行：** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **位置：** `csharp/`、`zava-creative-writer-local/src/csharp/`
- **專案檔案：** `csharp/csharp.csproj`、`zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **主要套件：** `Microsoft.AI.Foundry.Local`（非 Windows）、`Microsoft.AI.Foundry.Local.WinML`（Windows — 含 QNN 執行端的超集）、`OpenAI`、`Microsoft.Agents.AI.OpenAI`
- **目標平台：** .NET 9.0（條件 TFM：Windows 為 `net9.0-windows10.0.26100`，其他為 `net9.0`）
- **執行：** `cd csharp && dotnet run [chat|rag|agent|multi]`

## 編碼慣例

### 一般
- 所有範例程式碼皆為<strong>獨立單檔範例</strong> — 無共用工具庫或抽象層。
- 各範例安裝相依後可獨立執行。
- API 金鑰皆設為 `"foundry-local"` — Foundry Local 以此作為占位符。
- 基底 URL 使用 `http://localhost:<port>/v1` — 埠號為動態且由 SDK 在運行時覓得（JS 中為 `manager.urls[0]`，Python 為 `manager.endpoint`）。
- Foundry Local SDK 處理服務啟動與端點發現，優先使用 SDK 模式而非硬編碼埠號。

### Python
- 使用 `openai` SDK 並以 `OpenAI(base_url=..., api_key="not-required")` 初始化。
- 使用 `foundry_local` 的 `FoundryLocalManager()` 進行 SDK 管理的服務生命週期控制。
- 流式串流時，以 `for chunk in stream:` 迭代 `stream` 物件。
- 範例檔無型別註記（為工作坊學習者保持簡潔）。

### JavaScript
- 使用 ES 模組語法：`import ... from "..."`。
- 使用 `"openai"` 的 `OpenAI` 與 `"foundry-local-sdk"` 的 `FoundryLocalManager`。
- SDK 初始化流程：`FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`。
- 流式串流採用 `for await (const chunk of stream)`。
- 全程使用頂層 `await`。

### C#
- 啟用 Nullable、隱含 using，使用 .NET 9。
- 使用 `FoundryLocalManager.StartServiceAsync()` 進行 SDK 管理的生命週期。
- 流式串流用 `CompleteChatStreaming()` 並藉由 `foreach (var update in completionUpdates)` 迭代。
- 主要 `csharp/Program.cs` 為 CLI 路由器，將命令分派至靜態 `RunAsync()` 方法。

### 工具呼叫
- 只有特定模型支援工具呼叫：**Qwen 2.5** 系列（`qwen2.5-*`）和 **Phi-4-mini**（`phi-4-mini`）。
- 工具架構依循 OpenAI 函式呼叫 JSON 格式（`type: "function"`、`function.name`、`function.description`、`function.parameters`）。
- 對話使用多回合模式：使用者 → 助手（tool_calls）→ 工具（結果）→ 助手（最終回答）。
- 工具結果訊息的 `tool_call_id` 必須與模型工具呼叫時的 `id` 相符。
- Python 直接使用 OpenAI SDK；JavaScript 以 SDK 原生 `ChatClient` (`model.createChatClient()`)；C# 使用 OpenAI SDK 搭配 `ChatTool.CreateFunctionTool()`。

### ChatClient（原生 SDK 用戶端）
- JavaScript：`model.createChatClient()` 回傳具備 `completeChat(messages, tools?)` 與 `completeStreamingChat(messages, callback)` 的 `ChatClient`。
- C#：`model.GetChatClientAsync()` 回傳可用的標準 `ChatClient`，無需額外引入 OpenAI NuGet。
- Python 無原生 ChatClient — 使用 OpenAI SDK 搭配 `manager.endpoint` 與 `manager.api_key`。
- **重要：** JavaScript 的 `completeStreamingChat` 採用<strong>回呼模式</strong>，非非同步迭代。

### 推理模型
- `phi-4-mini-reasoning` 在最終答案前會以 `<think>...</think>` 標籤包裹思考過程。
- 如需，可解析這些標籤以分離推理與答案。

## 實驗室指引

實驗室檔案位於 `labs/`，以 Markdown 格式撰寫。遵循固定結構：
- 標誌頁首圖片
- 標題與目標說明
- 概覽、學習目標、先備知識
- 概念說明章節搭配圖表
- 編號練習包含程式碼區塊與預期輸出
- 摘要表格、重點摘要、進階閱讀
- 導覽連結指向下一部分

編輯實驗內容時：
- 保持既有 Markdown 格式風格及章節層級。
- 程式碼區塊標明語言（`python`、`javascript`、`csharp`、`bash`、`powershell`）。
- 命令依系統需求提供 bash 與 PowerShell 兩版。
- 使用 `> **Note:**`、`> **Tip:**`、`> **Troubleshooting:**` 等提示樣式。
- 資料表使用 `| 標題 | 標題 |` 管道格式。

## 建置與測試指令

| 動作 | 指令 |
|--------|---------|
| **Python 範例** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS 範例** | `cd javascript && npm install && node <script>.mjs` |
| **C# 範例** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS（網頁）** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C#（網頁）** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`、`foundry model run <model>`、`foundry service status` |
| <strong>產生圖表</strong> | `npx mmdc -i <input>.mmd -o <output>.svg`（需全域安裝 `npm install`） |

## 外部相依

- **Foundry Local CLI** 必須安裝於開發者主機（使用 `winget install Microsoft.FoundryLocal` 或 `brew install foundrylocal` 安裝）。
- **Foundry Local 服務** 本機運行，並會於動態埠暴露兼容 OpenAI 的 REST API。
- 執行任何範例不需雲端服務、API 金鑰或 Azure 訂閱。
- 第 10 部分（自訂模型）另需 `onnxruntime-genai` 並會從 Hugging Face 下載模型權重。

## 不應提交的檔案

`.gitignore` 會排除（且大多已排除）：
- `.venv/` — Python 虛擬環境
- `node_modules/` — npm 相依套件
- `models/` — 編譯後的 ONNX 模型輸出（大型二進位檔，10 部分生成）
- `cache_dir/` — Hugging Face 模型下載快取
- `.olive-cache/` — Microsoft Olive 工作目錄
- `samples/audio/*.wav` — 生成的音訊樣本（可透過 `python samples/audio/generate_samples.py` 再生）
- 標準 Python 建置產物（`__pycache__/`、`*.egg-info/`、`dist/` 等）

## 授權

MIT — 請參閱 `LICENSE`。