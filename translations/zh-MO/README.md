<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local 工作坊 - 在裝置上建構 AI 應用程式

一個實作工作坊，教你如何在自己的機器上執行語言模型並利用 [Foundry Local](https://foundrylocal.ai) 以及 [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) 建立智慧應用。

> **甚麼是 Foundry Local?** Foundry Local 是一個輕量級運行時環境，讓你能夠完全在自己的硬件上下載、管理及提供語言模型。它提供一個 **相容於 OpenAI 的 API**，任何可連接 OpenAI 的工具或 SDK 都能接入，毋需雲端帳戶。

---

## 學習目標

完成此工作坊後，你將能夠：

| # | 目標 |
|---|-----------|
| 1 | 安裝 Foundry Local 並用 CLI 管理模型 |
| 2 | 精通 Foundry Local SDK API 以程式化管理模型 |
| 3 | 使用 Python、JavaScript 和 C# SDK 連接本地推理伺服器 |
| 4 | 建立搭配訓練資料的檢索增強生成（RAG）流程 |
| 5 | 建立具有持續指令和角色設定的 AI 代理人 |
| 6 | 組織多代理人工作流程並加入反饋迴路 |
| 7 | 探索生產級結業應用 - Zava 創意作家 |
| 8 | 建立評估框架，使用黃金資料集及以 LLM 作評審的評分機制 |
| 9 | 使用 Whisper 進行音訊轉錄－直接在裝置上做語音轉文字 |
| 10 | 使用 ONNX Runtime GenAI 和 Foundry Local 編譯並運行自訂或 Hugging Face 模型 |
| 11 | 啟用本地模型調用外部函數，透過工具調用模式 |
| 12 | 為 Zava 創意作家建置基於瀏覽器的即時串流用戶介面 |

---

## 前置需求

| 需求 | 詳情 |
|-------------|---------|
| <strong>硬件</strong> | 至少 8 GB 記憶體（建議 16 GB）；具 AVX2 指令集的 CPU 或符合支援的 GPU |
| <strong>作業系統</strong> | Windows 10/11 (x64/ARM)、Windows Server 2025、或 macOS 13+ |
| **Foundry Local CLI** | Windows 使用 `winget install Microsoft.FoundryLocal` 安裝，macOS 使用 `brew tap microsoft/foundrylocal && brew install foundrylocal` 安裝。詳見[入門指南](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) 。 |
| <strong>程式語言運行環境</strong> | **Python 3.9+** 和 / 或 **.NET 9.0+** 和 / 或 **Node.js 18+** |
| **Git** | 用於克隆本存儲庫 |

---

## 開始使用

```bash
# 1. 複製儲存庫
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. 確認已安裝 Foundry Local
foundry model list              # 列出可用模型
foundry model run phi-3.5-mini  # 開始互動式聊天

# 3. 選擇你的語言路線（完整設置請參閱第二部分實驗）
```

| 語言 | 快速開始 |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## 工作坊章節

### 第 1 部分：Foundry Local 入門

**實驗室指南：** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local 是甚麼及其運作方式
- 在 Windows 和 macOS 上安裝 CLI
- 探索模型：列出、下載、執行
- 理解模型別名與動態連接埠

---

### 第 2 部分：Foundry Local SDK 深入解析

**實驗室指南：** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- 為何開發應用程式時選用 SDK 取代 CLI
- Python、JavaScript、C# 完整 SDK API 參考
- 服務管理、目錄瀏覽、模型生命週期（下載、載入、釋放）
- 快速啟動範例：Python 建構函數初始化、JavaScript `init()`、C# `CreateAsync()`
- `FoundryModelInfo` 元資料、別名及硬件最佳化模型選擇

---

### 第 3 部分：SDK 與 API

**實驗室指南：** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- 從 Python、JavaScript 和 C# 連接 Foundry Local
- 使用 Foundry Local SDK 以程式化方式管理服務
- 透過相容 OpenAI 的 API 流式傳輸聊天完成
- 各語言 SDK 方法參考

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local.py` | 基礎串流聊天 |
| C# | `csharp/BasicChat.cs` | 使用 .NET 的串流聊天 |
| JavaScript | `javascript/foundry-local.mjs` | 使用 Node.js 的串流聊天 |

---

### 第 4 部分：檢索增強生成（RAG）

**實驗室指南：** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- 什麼是 RAG 及為何重要
- 建立記憶體內知識庫
- 關鍵字重疊檢索與評分
- 組成有依據的系統提示
- 在裝置上執行完整 RAG 流程

**程式碼範例：**

| 語言 | 檔案 |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 第 5 部分：建立 AI 代理人

**實驗室指南：** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI 代理人是什麼（對比直接呼叫 LLM）
- `ChatAgent` 模式與 Microsoft Agent Framework
- 系統指令、角色設定及多輪對話
- 代理人輸出的結構化（JSON）

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | 使用 Agent Framework 的單一代理人 |
| C# | `csharp/SingleAgent.cs` | 單一代理人（ChatAgent 模式） |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | 單一代理人（ChatAgent 模式） |

---

### 第 6 部分：多代理人工作流程

**實驗室指南：** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- 多代理人流程：研究者 → 作者 → 編輯
- 連續編排與反饋迴路
- 共用設定與結構化移交
- 設計你自己的多代理人工作流程

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | 三代理人流程 |
| C# | `csharp/MultiAgent.cs` | 三代理人流程 |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 三代理人流程 |

---

### 第 7 部分：Zava 創意作家 - 結業應用

**實驗室指南：** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 一個有 4 名專業代理人的生產級多代理人應用
- 使用評估者驅動的反饋迴路的連續流程
- 串流輸出、產品目錄搜尋、結構化 JSON 移交
- 完整實作包括 Python (FastAPI)、JavaScript (Node.js CLI)、及 C# (.NET 控制台)

**程式碼範例：**

| 語言 | 目錄 | 說明 |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | 帶協調器的 FastAPI 網頁服務 |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI 應用 |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 控制台應用 |

---

### 第 8 部分：評估主導開發

**實驗室指南：** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- 使用黃金資料集，建立 AI 代理人的系統性評估框架
- 規則檢查（長度、關鍵字覆蓋、禁用詞）+ 使用 LLM 作評審的評分
- 比較提示變體並產生整體分數卡
- 將第 7 部分的 Zava 編輯代理人模式擴展成離線測試套件
- 包含 Python、JavaScript 及 C# 路徑

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | 評估框架 |
| C# | `csharp/AgentEvaluation.cs` | 評估框架 |
| JavaScript | `javascript/foundry-local-eval.mjs` | 評估框架 |

---

### 第 9 部分：使用 Whisper 進行語音轉錄

**實驗室指南：** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- 執行本地 OpenAI Whisper 進行語音轉文字轉錄
- 優先保護隱私的音訊處理－音訊資料不會離開你的裝置
- Python、JavaScript 和 C# 範例，使用 `client.audio.transcriptions.create()` (Python/JS) 與 `AudioClient.TranscribeAudioAsync()` (C#)
- 附帶 Zava 主題範例音訊檔供練習

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper 語音轉錄 |
| C# | `csharp/WhisperTranscription.cs` | Whisper 語音轉錄 |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper 語音轉錄 |

> **注意：** 本實驗室使用 **Foundry Local SDK** 程式化下載與載入 Whisper 模型，並將音訊送往本地相容 OpenAI 的端點進行轉錄。Whisper 模型（`whisper`）列在 Foundry Local 目錄中，完全在裝置上運行－無需雲端 API 金鑰或網路存取。

---

### 第 10 部分：使用自訂或 Hugging Face 模型

**實驗室指南：** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- 使用 ONNX Runtime GenAI 模型建構工具編譯 Hugging Face 模型至最佳化 ONNX 格式
- 針對硬件特定編譯（CPU、NVIDIA GPU、DirectML、WebGPU）與量化（int4、fp16、bf16）
- 建立 Foundry Local 對話模板設定檔
- 將編譯模型新增到 Foundry Local 快取
- 透過 CLI、REST API 及 OpenAI SDK 執行自訂模型
- 參考範例：Qwen/Qwen3-0.6B 模型端到端編譯

---

### 第 11 部分：本地模型的工具調用

**實驗室指南：** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- 啟用本地模型調用外部函數（工具/函數調用）
- 使用 OpenAI 函數調用格式定義工具結構
- 處理多輪工具調用的對話流程
- 本地執行工具調用並將結果返回模型
- 選擇適合工具調用的模型（Qwen 2.5、Phi-4-mini）
- 使用 SDK 的原生 `ChatClient` 進行工具調用（JavaScript）

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | 使用天氣/人口工具的工具調用 |
| C# | `csharp/ToolCalling.cs` | 使用 .NET 的工具調用 |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | 使用 ChatClient 的工具調用 |

---

### 第 12 部分：為 Zava 創意作家建置網頁用戶介面

**實驗室指南：** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- 為 Zava 創意作家新增基於瀏覽器的前端
- 由 Python（FastAPI）、JavaScript（Node.js HTTP）、及 C#（ASP.NET Core）提供共用 UI
- 使用 Fetch API 和 ReadableStream 在瀏覽器中消費串流 NDJSON
- 即時代理狀態徽章及文章文字即時串流顯示

**程式碼（共用 UI）：**

| 檔案 | 說明 |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | 頁面佈局 |
| `zava-creative-writer-local/ui/style.css` | 樣式 |
| `zava-creative-writer-local/ui/app.js` | 串流閱讀器與 DOM 更新邏輯 |

**後端新增：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | 更新供應靜態 UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | 包裝協調器的新 HTTP 伺服器 |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 新增 ASP.NET Core Minimal API 專案 |

---

### 第 13 部分：工作坊完成
**實驗室指南：** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 回顧你在全部12個部分中所構建的內容摘要
- 延伸應用的進一步想法
- 資源和文件的連結

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
| 入門指南 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 參考 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## 授權

本工作坊教材僅供教育用途。

---

**祝你構建愉快！🚀**