<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local 工作坊 - 本地構建 AI 應用程式

一個實作工作坊，教你如何在自己的機器上運行語言模型，並使用 [Foundry Local](https://foundrylocal.ai) 及 [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) 建立智能應用程式。

> **什麼是 Foundry Local？** Foundry Local 是一個輕量級運行時，讓你能夠完全在自己的硬件上下載、管理及提供語言模型服務。它暴露了一個<strong>與 OpenAI 相容的 API</strong>，因此任何支援 OpenAI 的工具或 SDK 都能連接 — 無需雲端帳戶。

---

## 學習目標

完成此工作坊後，你將能：

| # | 目標 |
|---|-----------|
| 1 | 安裝 Foundry Local 並使用 CLI 管理模型 |
| 2 | 精通 Foundry Local SDK API 以程式方式管理模型 |
| 3 | 使用 Python、JavaScript 和 C# SDK 連接本地推理服務器 |
| 4 | 建立基於檢索增強生成（RAG）的管道，使回答根據你的資料 |
| 5 | 創建帶有持久指令和角色設定的 AI 代理 |
| 6 | 編排多代理工作流程並建立反饋回路 |
| 7 | 探索生產級壓軸應用 - Zava 創意寫作助手 |
| 8 | 建立黃金數據集與以 LLM 作評分的評估框架 |
| 9 | 使用 Whisper 進行語音轉錄 — 在本地運行的語音轉文字功能 |
| 10 | 使用 ONNX Runtime GenAI 和 Foundry Local 編譯並運行自訂或 Hugging Face 模型 |
| 11 | 使本地模型能以工具呼叫模式調用外部函數 |
| 12 | 為 Zava 創意寫作助手構建基於瀏覽器的實時串流 UI |

---

## 前置條件

| 需求 | 詳細說明 |
|-------------|---------|
| <strong>硬件</strong> | 至少 8 GB 記憶體（建議 16 GB）；支援 AVX2 的 CPU 或合適 GPU |
| <strong>作業系統</strong> | Windows 10/11（x64/ARM）、Windows Server 2025 或 macOS 13+ |
| **Foundry Local CLI** | Windows：`winget install Microsoft.FoundryLocal`，macOS：`brew tap microsoft/foundrylocal && brew install foundrylocal`。詳見 [快速上手指南](https://learn.microsoft.com/en-us/azure/foundry-local/get-started)。 |
| <strong>語言執行環境</strong> | **Python 3.9+** 和／或 **.NET 9.0+** 和／或 **Node.js 18+** |
| **Git** | 用於 clone 本 repo |

---

## 開始使用

```bash
# 1. 複製儲存庫
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. 驗證已安裝 Foundry Local
foundry model list              # 列出可用模型
foundry model run phi-3.5-mini  # 開始互動式對話

# 3. 選擇你的語言路線（完整設置請參閱第2部分實驗）
```

| 語言 | 快速啟動 |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## 工作坊內容

### 第一部分：Foundry Local 基礎入門

**實驗室指南：** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local 是什麼及其工作原理
- 在 Windows 和 macOS 安裝 CLI
- 探索模型：列出、下載、運行
- 理解模型別名和動態端口

---

### 第二部分：Foundry Local SDK 深入解析

**實驗室指南：** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- 為什麼用 SDK 而非 CLI 來開發應用程式
- Python、JavaScript 和 C# 的完整 SDK API 參考
- 服務管理，目錄瀏覽，模型生命周期（下載、載入、卸載）
- 快速啟動範例：Python 建構函數啟動，JavaScript `init()`，C# `CreateAsync()`
- `FoundryModelInfo` 元資料、別名及最佳硬件模型選擇

---

### 第三部分：SDK 與 API

**實驗室指南：** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- 使用 Python、JavaScript 和 C# 連接 Foundry Local
- 使用 Foundry Local SDK 程式化管理服務
- 透過與 OpenAI 相容的 API 進行串流聊天補全
- 各語言 SDK 方法參考

**程式範例：**

| 語言 | 檔案 | 描述 |
|----------|------|-------------|
| Python | `python/foundry-local.py` | 基本串流聊天 |
| C# | `csharp/BasicChat.cs` | 使用 .NET 的串流聊天 |
| JavaScript | `javascript/foundry-local.mjs` | 使用 Node.js 的串流聊天 |

---

### 第四部分：檢索增強生成（RAG）

**實驗室指南：** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG 是什麼及其重要性
- 建立記憶體內知識庫
- 基於關鍵字重疊的檢索與評分
- 撰寫有根據的系統提示
- 在本地運行完整的 RAG 管線

**程式範例：**

| 語言 | 檔案 |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 第五部分：建立 AI 代理

**實驗室指南：** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI 代理是什麼（與直接呼叫 LLM 的差異）
- `ChatAgent` 模式與 Microsoft Agent Framework
- 系統指令、角色設定和多輪對話
- 代理輸出結構化結果（JSON）

**程式範例：**

| 語言 | 檔案 | 描述 |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | 使用 Agent Framework 的單一代理 |
| C# | `csharp/SingleAgent.cs` | 單一代理（ChatAgent 模式） |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | 單一代理（ChatAgent 模式） |

---

### 第六部分：多代理工作流程

**實驗室指南：** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- 多代理管線：研究員 → 作家 → 編輯
- 串聯編排及反饋回路
- 共享配置與結構化交接
- 設計自訂多代理工作流程

**程式範例：**

| 語言 | 檔案 | 描述 |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | 三代理管線 |
| C# | `csharp/MultiAgent.cs` | 三代理管線 |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 三代理管線 |

---

### 第七部分：Zava 創意寫作助手 - 壓軸應用

**實驗室指南：** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 一個生產級的多代理應用，配備 4 個專職代理
- 串聯管線與評估者驅動的反饋回路
- 串流輸出、產品目錄搜索與結構化 JSON 交接
- Python（FastAPI）、JavaScript（Node.js CLI）、C#（.NET 控制台）完整實作

**程式範例：**

| 語言 | 目錄 | 描述 |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI 網服務與編排者 |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI 應用 |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 控制台應用 |

---

### 第八部分：評估驅動開發

**實驗室指南：** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- 利用黃金數據集建構系統性的 AI 代理評估框架
- 規則式檢查（長度、關鍵字覆蓋、禁用詞）+ LLM 評分機制
- 提示變體的並列比較與綜合評分卡
- 將第七部分的 Zava 編輯代理模式擴展到離線測試套件
- Python、JavaScript 及 C# 路徑

**程式範例：**

| 語言 | 檔案 | 描述 |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | 評估框架 |
| C# | `csharp/AgentEvaluation.cs` | 評估框架 |
| JavaScript | `javascript/foundry-local-eval.mjs` | 評估框架 |

---

### 第九部分：Whisper 語音轉錄

**實驗室指南：** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- 本地運行 OpenAI Whisper 進行語音轉文字轉錄
- 隱私至上音頻處理 — 音頻絕不離開裝置
- Python、JavaScript 與 C# 路徑，使用 `client.audio.transcriptions.create()`（Python/JS）與 `AudioClient.TranscribeAudioAsync()`（C#）
- 附帶 Zava 主題範例音訊文件供實戰

**程式範例：**

| 語言 | 檔案 | 描述 |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper 語音轉錄 |
| C# | `csharp/WhisperTranscription.cs` | Whisper 語音轉錄 |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper 語音轉錄 |

> **注意：** 本實驗使用 **Foundry Local SDK** 程式化下載與載入 Whisper 模型，並將音訊發送到本地 OpenAI 相容端點完成轉錄。Whisper 模型（`whisper`）在 Foundry Local 目錄中，完全本地運行 — 無需雲端 API 金鑰或網絡連結。

---

### 第十部分：使用自訂或 Hugging Face 模型

**實驗室指南：** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- 使用 ONNX Runtime GenAI 模型建構器編譯 Hugging Face 模型到優化 ONNX 格式
- 針對硬件編譯（CPU、NVIDIA GPU、DirectML、WebGPU）及量化（int4、fp16、bf16）
- 創建 Foundry Local 聊天模板配置文件
- 將編譯模型加入 Foundry Local 快取
- 透過 CLI、REST API 及 OpenAI SDK 運行自訂模型
- 參考範例：從頭到尾編譯 Qwen/Qwen3-0.6B

---

### 第十一部分：本地模型工具呼叫

**實驗室指南：** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- 使本地模型能調用外部函數（工具／函數呼叫）
- 使用 OpenAI 函數呼叫格式定義工具架構
- 處理多輪工具呼叫會話流程
- 在本地執行工具呼叫並將結果回饋模型
- 選擇合適工具呼叫模型（Qwen 2.5、Phi-4-mini）
- 使用 SDK 原生的 `ChatClient` 進行工具呼叫（JavaScript）

**程式範例：**

| 語言 | 檔案 | 描述 |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | 使用天氣／人口工具的工具呼叫 |
| C# | `csharp/ToolCalling.cs` | 使用 .NET 的工具呼叫 |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | 使用 ChatClient 的工具呼叫 |

---

### 第十二部分：為 Zava 創意寫作助手建立網頁 UI

**實驗室指南：** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- 為 Zava 創意寫作助手添加基於瀏覽器的前端介面
- Python（FastAPI）、JavaScript（Node.js HTTP）及 C#（ASP.NET Core）提供共享 UI
- 在瀏覽器中使用 Fetch API 和 ReadableStream 消費串流 NDJSON
- 實時代理狀態徽章及文章文字串流

**共享 UI 程式碼：**

| 檔案 | 描述 |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | 頁面布局 |
| `zava-creative-writer-local/ui/style.css` | 樣式設計 |
| `zava-creative-writer-local/ui/app.js` | 串流讀取器與 DOM 更新邏輯 |

**後端新增內容：**

| 語言 | 檔案 | 描述 |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | 更新以提供靜態 UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | 新增基於 HTTP 的編排者服務器 |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 新增 ASP.NET Core 最小 API 專案 |

---

### 第十三部分：工作坊結束
**實驗室指南：** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 總結你在所有12個部分中所建立的一切
- 拓展你的應用程式的更多想法
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

## 授權條款

此工作坊教材僅供教育用途。

---

**祝你建設愉快！🚀**