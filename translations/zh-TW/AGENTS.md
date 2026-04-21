# Coding Agent 指示

此檔案為在此存放庫中工作的 AI 程式代理（GitHub Copilot、Copilot Workspace、Codex 等）提供上下文。

## 專案概述

這是一個使用 [Foundry Local](https://foundrylocal.ai) 建立 AI 應用程式的<strong>動手實作工作坊</strong>——輕量級執行環境，透過與 OpenAI 相容的 API，完全在裝置上下載、管理和提供語言模型。工作坊包含逐步的實驗室指南及可執行的 Python、JavaScript 和 C# 程式碼範例。

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

## 語言與框架詳情

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
- **主要套件：** `Microsoft.AI.Foundry.Local` （非 Windows 系統）、`Microsoft.AI.Foundry.Local.WinML`（Windows —— 包含 QNN EP 的超集）、`OpenAI`、`Microsoft.Agents.AI.OpenAI`
- **目標框架：** .NET 9.0（條件 TFM：Windows 時為 `net9.0-windows10.0.26100`，其他環境為 `net9.0`）
- **執行：** `cd csharp && dotnet run [chat|rag|agent|multi]`

## 程式編寫慣例

### 一般
- 所有程式碼範例都是<strong>獨立單檔案示例</strong>——不依賴共用工具庫或抽象。
- 每個範例安裝自身相依套件後可獨立執行。
- API 金鑰均設為 `"foundry-local"` —— Foundry Local 使用此值作為占位符。
- 基底 URL 使用 `http://localhost:<port>/v1` —— 埠號為動態，由 SDK 執行時探索（JS 中為 `manager.urls[0]`，Python 為 `manager.endpoint`）。
- Foundry Local SDK 處理服務啟動及端點探索；宜使用 SDK 模式，避免硬編碼埠號。

### Python
- 使用帶 `OpenAI(base_url=..., api_key="not-required")` 的 `openai` SDK。
- 使用 `foundry_local` 的 `FoundryLocalManager()` 管理服務生命週期。
- 串流：用 `for chunk in stream:` 迭代串流結果。
- 範例檔不使用型別註釋（保持簡潔方便教學）。

### JavaScript
- ES 模組語法：`import ... from "..."`。
- 使用 `"openai"` 中的 `OpenAI` 及 `"foundry-local-sdk"` 中的 `FoundryLocalManager`。
- SDK 初始化模式：`FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`。
- 串流：用 `for await (const chunk of stream)` 迭代。
- 全域使用 top-level `await`。

### C#
- 啟用 Nullable、隱式 using、.NET 9。
- 使用 `FoundryLocalManager.StartServiceAsync()` 管理服務生命週期。
- 串流：以 `CompleteChatStreaming()` 和 `foreach (var update in completionUpdates)` 方式處理。
- 主要程式 `csharp/Program.cs` 為 CLI 路由器，呼叫靜態的 `RunAsync()` 方法。

### 工具呼叫
- 僅部分模型支援工具呼叫：**Qwen 2.5** 系列（`qwen2.5-*`）和 **Phi-4-mini** （`phi-4-mini`）。
- 工具架構遵循 OpenAI 函數呼叫 JSON 格式（`type: "function"`、`function.name`、`function.description`、`function.parameters`）。
- 對話遵循多輪模式：使用者 → 助理（tool_calls）→ 工具（結果）→ 助理（最終答案）。
- 工具結果訊息中的 `tool_call_id` 要與模型呼叫工具的 `id` 相符。
- Python 直接使用 OpenAI SDK；JavaScript 使用 SDK 的原生 `ChatClient`（`model.createChatClient()`）；C# 使用 OpenAI SDK 的 `ChatTool.CreateFunctionTool()`。

### ChatClient（原生 SDK 客戶端）
- JavaScript：`model.createChatClient()` 回傳帶 `completeChat(messages, tools?)` 及 `completeStreamingChat(messages, callback)` 的 `ChatClient`。
- C#：`model.GetChatClientAsync()` 回傳標準 `ChatClient`，無需額外引用 OpenAI NuGet 套件即可使用。
- Python 沒有原生 ChatClient —— 使用 `manager.endpoint` 和 `manager.api_key` 的 OpenAI SDK。
- **重要：** JavaScript 的 `completeStreamingChat` 採用<strong>回呼(callback)模式</strong>，非 async 迭代。

### 推理模型
- `phi-4-mini-reasoning` 於最終答案前，用 `<think>...</think>` 標籤包裹思考過程。
- 需要時解析標籤分離推理與答案。

## 實驗室指南

實驗室檔案位於 `labs/` ，為 Markdown 格式。格式統一包含：
- 標誌標題圖片
- 標題與目標提示區塊
- 概述、學習目標、先備知識
- 概念說明節段含圖表
- 編號練習包含程式碼區塊與預期輸出
- 摘要表、關鍵重點、延伸閱讀
- 通往下一部分的導覽連結

編輯實驗室內容時：
- 保持現有 Markdown 格式及章節層級。
- 程式碼區塊須標示語言（`python`、`javascript`、`csharp`、`bash`、`powershell`）。
- Shell 指令依 OS 提供 bash 與 PowerShell 版本。
- 使用 `> **Note:**`、`> **Tip:**` 和 `> **Troubleshooting:**` 呼叫式樣。
- 表格採用 `| 標題 | 標題 |` 管線格式。

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
| <strong>生成功能圖</strong> | `npx mmdc -i <input>.mmd -o <output>.svg`（需先全域安裝 `npm install -g @mermaid-js/mermaid-cli`） |

## 外部相依項目

- **Foundry Local CLI** 需安裝於開發者機器（`winget install Microsoft.FoundryLocal` 或 `brew install foundrylocal`）。
- **Foundry Local 服務** 在本機執行，並於動態埠開放兼容 OpenAI 的 REST API。
- 執行任一範例不需雲端服務、API 金鑰或 Azure 訂閱。
- 第 10 部分（自訂模型）還需 `onnxruntime-genai`，並從 Hugging Face 下載模型權重。

## 不應提交的檔案

`.gitignore` 應排除（且大多數已排除）：
- `.venv/` — Python 虛擬環境
- `node_modules/` — npm 相依套件
- `models/` — 編譯出的 ONNX 模型（大型二進位檔，由第 10 部分產生）
- `cache_dir/` — Hugging Face 模型下載快取
- `.olive-cache/` — Microsoft Olive 工作目錄
- `samples/audio/*.wav` — 產生的音訊範例（可用 `python samples/audio/generate_samples.py` 重製）
- 標準 Python 建置產物（`__pycache__/`、`*.egg-info/`、`dist/` 等）

## 授權

MIT 授權——詳見 `LICENSE`。