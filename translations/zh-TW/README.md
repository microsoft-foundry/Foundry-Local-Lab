<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local 工作坊 - 在設備上構建 AI 應用程式

一個動手做的工作坊，教你如何在自己的機器上運行語言模型，並使用 [Foundry Local](https://foundrylocal.ai) 和 [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) 構建智能應用程式。

> **什麼是 Foundry Local？** Foundry Local 是一個輕量級的執行環境，讓你能完全在自己的硬體上下載、管理並提供語言模型服務。它提供一個 **OpenAI 相容的 API**，所以任何支援 OpenAI 的工具或 SDK 都能連接 - 無需雲端帳戶。

---

## 學習目標

完成本工作坊後，你將能夠：

| # | 目標 |
|---|------|
| 1 | 安裝 Foundry Local 並使用 CLI 管理模型 |
| 2 | 掌握 Foundry Local SDK API 以程式化管理模型 |
| 3 | 使用 Python、JavaScript 及 C# SDK 連接本地推論伺服器 |
| 4 | 建立一個基於檢索增強生成 (RAG) 的管線，讓回答依據你的資料 |
| 5 | 創建具有持久指令和角色設定的 AI 智能代理 |
| 6 | 協調多代理工作流程與回饋迴圈 |
| 7 | 探索生產級的壓軸應用 - Zava 創意寫手 |
| 8 | 建構評估框架，使用黃金資料集與 LLM 擔任判官的評分 |
| 9 | 使用 Whisper 進行語音轉錄 - 透過 Foundry Local SDK 在設備上執行語音轉文字 |
| 10 | 使用 ONNX Runtime GenAI 與 Foundry Local 編譯並執行自訂或 Hugging Face 模型 |
| 11 | 讓本地模型能以工具呼叫模式呼叫外部函式 |
| 12 | 為 Zava 創意寫手建立瀏覽器介面，支援即時串流 |

---

## 先備條件

| 要求 | 詳細說明 |
|------|----------|
| <strong>硬體</strong> | 至少 8 GB 記憶體（建議 16 GB）；支援 AVX2 的 CPU 或受支援的 GPU |
| <strong>作業系統</strong> | Windows 10/11 (x64/ARM)、Windows Server 2025 或 macOS 13 以上版本 |
| **Foundry Local CLI** | Windows 可用 `winget install Microsoft.FoundryLocal` 安裝；macOS 可用 `brew tap microsoft/foundrylocal && brew install foundrylocal` 安裝。詳情請參考[快速入門指南](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) 。 |
| <strong>程式語言執行環境</strong> | **Python 3.9+** 和/或 **.NET 9.0+** 和/或 **Node.js 18+** |
| **Git** | 用於複製此資源庫 |

---

## 快速開始

```bash
# 1. 複製此儲存庫
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. 確認已安裝 Foundry Local
foundry model list              # 列出可用模型
foundry model run phi-3.5-mini  # 開始互動式聊天

# 3. 選擇你的語言路線（完整設定請參閱第2部分實驗）
```

| 語言 | 快速開始 |
|------|----------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## 工作坊章節

### 第 1 部分：Foundry Local 快速入門

**實驗室指南：** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- 什麼是 Foundry Local 及其運作原理
- 在 Windows 和 macOS 上安裝 CLI
- 探索模型 - 列出、下載及運行
- 瞭解模型別名和動態埠口

---

### 第 2 部分：Foundry Local SDK 深入解析

**實驗室指南：** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- 為什麼開發應用程式時用 SDK 優於 CLI
- Python、JavaScript 與 C# 完整 SDK API 參考
- 服務管理、目錄瀏覽、模型生命週期 (下載、載入、卸載)
- 快速啟動模式：Python 建構子引導、JavaScript 的 `init()`、C# 的 `CreateAsync()`
- `FoundryModelInfo` 元資料、別名與最佳硬體模型選擇

---

### 第 3 部分：SDK 與 API

**實驗室指南：** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- 從 Python、JavaScript 和 C# 連接 Foundry Local
- 使用 Foundry Local SDK 程式化管理服務
- 透過 OpenAI 兼容 API 串流聊天完成結果
- 各語言 SDK 方法參考

**程式範例：**

| 語言 | 檔案 | 說明 |
|------|------|------|
| Python | `python/foundry-local.py` | 基本串流聊天 |
| C# | `csharp/BasicChat.cs` | 用 .NET 進行串流聊天 |
| JavaScript | `javascript/foundry-local.mjs` | 用 Node.js 進行串流聊天 |

---

### 第 4 部分：檢索增強生成 (RAG)

**實驗室指南：** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- 什麼是 RAG 以及它的重要性
- 建立記憶體內知識庫
- 關鍵字重疊檢索與評分
- 組成有基礎的系統提示
- 執行完整的本地 RAG 管線

**程式範例：**

| 語言 | 檔案 |
|------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 第 5 部分：建立 AI 智能代理

**實驗室指南：** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- 什麼是 AI 智能代理（與純粹的 LLM 呼叫比較）
- `ChatAgent` 模式與 Microsoft Agent Framework
- 系統指令、角色設定及多輪對話
- 代理產生的結構化輸出 (JSON)

**程式範例：**

| 語言 | 檔案 | 說明 |
|------|------|------|
| Python | `python/foundry-local-with-agf.py` | 使用 Agent Framework 的單一代理 |
| C# | `csharp/SingleAgent.cs` | 單一代理（ChatAgent 模式） |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | 單一代理（ChatAgent 模式） |

---

### 第 6 部分：多代理工作流程

**實驗室指南：** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- 多代理管線：研究員 → 寫手 → 編輯
- 順序編排與回饋迴圈
- 共享設定與結構化移交
- 設計自訂多代理工作流程

**程式範例：**

| 語言 | 檔案 | 說明 |
|------|------|------|
| Python | `python/foundry-local-multi-agent.py` | 三代理管線 |
| C# | `csharp/MultiAgent.cs` | 三代理管線 |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 三代理管線 |

---

### 第 7 部分：Zava 創意寫手 - 壓軸應用程式

**實驗室指南：** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 一個具備 4 個專門代理的生產級多代理應用
- 依評估員驅動的回饋迴圈進行的順序管線
- 串流輸出、產品目錄搜尋、結構化 JSON 移交
- Python (FastAPI)、JavaScript (Node.js CLI)、C# (.NET 控制台) 全面實作

**程式範例：**

| 語言 | 目錄 | 說明 |
|------|------|------|
| Python | `zava-creative-writer-local/src/api/` | 使用 FastAPI 的網路服務及協調器 |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI 應用程式 |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 控制台應用 |

---

### 第 8 部分：以評估為主導的開發

**實驗室指南：** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- 利用黃金資料集為 AI 代理建立系統化評估框架
- 規則檢查（長度、關鍵字覆蓋、禁用詞）+ 以 LLM 擔任判官的評分
- 以提示變體並列比較及累積分數卡
- 延伸第 7 部分中的 Zava 編輯代理模式，製作離線測試套件
- 支援 Python、JavaScript 與 C# 路線

**程式範例：**

| 語言 | 檔案 | 說明 |
|------|------|------|
| Python | `python/foundry-local-eval.py` | 評估框架 |
| C# | `csharp/AgentEvaluation.cs` | 評估框架 |
| JavaScript | `javascript/foundry-local-eval.mjs` | 評估框架 |

---

### 第 9 部分：Whisper 語音轉錄

**實驗室指南：** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- 於本地運行 OpenAI Whisper 完成語音轉文字
- 以保護隱私為先的音訊處理 - 音訊永遠不離開你的設備
- Python、JavaScript 與 C# 路線，使用 `client.audio.transcriptions.create()`（Python/JS）和 `AudioClient.TranscribeAudioAsync()`（C#）
- 包含 Zava 主題示例音訊檔，供實作練習

**程式範例：**

| 語言 | 檔案 | 說明 |
|------|------|------|
| Python | `python/foundry-local-whisper.py` | Whisper 語音轉錄 |
| C# | `csharp/WhisperTranscription.cs` | Whisper 語音轉錄 |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper 語音轉錄 |

> **注意：** 本實驗室使用 **Foundry Local SDK** 程式化下載和載入 Whisper 模型，然後將音訊送至本地 OpenAI 相容端點進行轉錄。Whisper 模型 (`whisper`) 列於 Foundry Local 目錄中，全程於設備上運行 - 無需雲端 API 金鑰或網路連線。

---

### 第 10 部分：使用自訂或 Hugging Face 模型

**實驗室指南：** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- 使用 ONNX Runtime GenAI 模型建構工具將 Hugging Face 模型編譯為優化的 ONNX 格式
- 硬體專屬編譯（CPU、NVIDIA GPU、DirectML、WebGPU）與量化（int4、fp16、bf16）
- 為 Foundry Local 建立聊天模板設定檔
- 將編譯後模型添加到 Foundry Local 快取
- 透過 CLI、REST API 和 OpenAI SDK 運行自訂模型
- 參考範例：端到端編譯 Qwen/Qwen3-0.6B

---

### 第 11 部分：本地模型的工具呼叫

**實驗室指南：** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- 讓本地模型可呼叫外部函式（工具/函式呼叫）
- 使用 OpenAI 函式呼叫格式定義工具結構
- 處理多輪工具呼叫會話流程
- 本地執行工具呼叫並將結果回傳至模型
- 為工具呼叫場景選擇合適模型（Qwen 2.5、Phi-4-mini）
- 使用 SDK 內建 `ChatClient` 進行工具呼叫（JavaScript）

**程式範例：**

| 語言 | 檔案 | 說明 |
|------|------|------|
| Python | `python/foundry-local-tool-calling.py` | 具備天氣/人口工具的工具呼叫 |
| C# | `csharp/ToolCalling.cs` | 使用 .NET 進行工具呼叫 |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | 使用 ChatClient 進行工具呼叫 |

---

### 第 12 部分：為 Zava 創意寫手建立網頁介面

**實驗室指南：** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- 為 Zava 創意寫手新增瀏覽器前端
- 從 Python (FastAPI)、JavaScript (Node.js HTTP) 與 C# (ASP.NET Core) 服務共享 UI
- 使用 Fetch API 與 ReadableStream 於瀏覽器端消費串流 NDJSON
- 實時代理狀態徽章及文章文字串流

**共用 UI 程式碼：**

| 檔案 | 說明 |
|------|------|
| `zava-creative-writer-local/ui/index.html` | 頁面佈局 |
| `zava-creative-writer-local/ui/style.css` | 樣式設定 |
| `zava-creative-writer-local/ui/app.js` | 串流讀取與 DOM 更新邏輯 |

**後端新增檔案：**

| 語言 | 檔案 | 說明 |
|------|------|------|
| Python | `zava-creative-writer-local/src/api/main.py` | 改為提供靜態 UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | 包裝協調器的新的 HTTP 伺服器 |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 新增 ASP.NET Core 極簡 API 專案 |

---

### 第 13 部分：工作坊完成
**實驗指引：** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 總結您在前12個部分中所建立的一切
- 擴展您的應用程式的進一步想法
- 資源與文件連結

---

## 專案結構

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

## 資源

| 資源 | 連結 |
|----------|------|
| Foundry Local 網站 | [foundrylocal.ai](https://foundrylocal.ai) |
| 模型目錄 | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| 新手指南 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 參考 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## 授權

此工作坊資源僅供教育用途。

---

**祝您構建愉快！🚀**