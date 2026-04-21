<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local 工作坊 - 在裝置上建立 AI 應用程式

一個動手操作工作坊，教你如何在自己的機器上運行語言模型，並使用 [Foundry Local](https://foundrylocal.ai) 及 [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) 建立智能應用程式。

> **甚麼是 Foundry Local？** Foundry Local 是一個輕量級的運行環境，讓你能夠完全在自己的硬件上下載、管理及提供語言模型服務。它支援一個 **OpenAI 相容的 API**，任何支援 OpenAI 的工具或 SDK 都能連接，不需使用雲端帳戶。

### 🌐 多語言支援

#### 通過 GitHub Action 支援（自動且持續更新）

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](./README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **想要本地端克隆？**
>
> 本倉庫包含超過 50 種語言的翻譯，會大幅增加下載大小。若要不包含翻譯克隆，請使用稀疏檢出：
>
> **Bash / macOS / Linux：**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD（Windows）：**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> 這樣你可以更快速地下載，並有完成課程所需的所有內容。
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## 學習目標

完成工作坊後，你將能：

| # | 目標 |
|---|------|
| 1 | 安裝 Foundry Local 並使用 CLI 管理模型 |
| 2 | 精通 Foundry Local SDK API，程式化管理模型 |
| 3 | 使用 Python、JavaScript 和 C# SDK 連接本地推論伺服器 |
| 4 | 建立 Retrieval-Augmented Generation (RAG) 流程，使答案基於自身資料 |
| 5 | 創建帶永久指令和角色設定的 AI 代理 |
| 6 | 編排多代理工作流程並建立回饋循環 |
| 7 | 探索一個生產級的頂點應用程式 - Zava 創意寫作助手 |
| 8 | 使用黃金數據集和大型語言模型評分建立評估框架 |
| 9 | 使用 Whisper 進行語音轉錄 - 在裝置上利用 Foundry Local SDK 進行語音轉文字 |
| 10 | 使用 ONNX Runtime GenAI 和 Foundry Local 編譯並執行自訂或 Hugging Face 模型 |
| 11 | 讓本地模型通過工具呼叫模式呼叫外部函數 |
| 12 | 為 Zava 創意寫作助手建立具備實時串流的瀏覽器介面 |

---

## 預備條件

| 需求 | 詳細資訊 |
|-------|----------|
| <strong>硬件</strong> | 至少 8 GB RAM（建議 16 GB）；AVX2 支援的 CPU 或支援的 GPU |
| <strong>作業系統</strong> | Windows 10/11 (x64/ARM)、Windows Server 2025 或 macOS 13+ |
| **Foundry Local CLI** | Windows 可用 `winget install Microsoft.FoundryLocal` 安裝，macOS 可用 `brew tap microsoft/foundrylocal && brew install foundrylocal` 安裝。詳見 [入門指南](https://learn.microsoft.com/en-us/azure/foundry-local/get-started)。 |
| <strong>語言運行時</strong> | **Python 3.9+** 和/或 **.NET 9.0+** 和/或 **Node.js 18+** |
| **Git** | 用於克隆此儲存庫 |

---

## 快速開始

```bash
# 1. 複製存儲庫
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. 確認已安裝 Foundry Local
foundry model list              # 列出可用模型
foundry model run phi-3.5-mini  # 開始互動式聊天

# 3. 選擇你的語言路徑（完整設定請參閱第 2 部分實驗）
```

| 語言 | 快速開始 |
|-------|------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## 工作坊模組

### 模組 1：Foundry Local 入門

**實驗室指南：** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local 是甚麼及其工作原理
- 在 Windows 和 macOS 安裝 CLI
- 探索模型：列出、下載、運行
- 理解模型別名及動態端口

---

### 模組 2：Foundry Local SDK 深入解析

**實驗室指南：** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- 為何開發應用程式時選用 SDK 而非 CLI
- Python、JavaScript 和 C# 的 SDK 全API參考
- 服務管理、目錄瀏覽、模型生命週期（下載、加載、卸載）
- 快速入門模式：Python 建構子初始化、JavaScript `init()`、C# `CreateAsync()`
- `FoundryModelInfo` 元資料、別名及硬件最佳模型選擇

---

### 模組 3：SDK 與 API

**實驗室指南：** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- 從 Python、JavaScript 和 C# 連接 Foundry Local
- 使用 Foundry Local SDK 程式化管理服務
- 通過 OpenAI 相容 API 串流聊天完成
- 各語言 SDK 方法參考

**程式範例：**

| 語言 | 檔案 | 描述 |
|-------|------|-------|
| Python | `python/foundry-local.py` | 基本串流聊天 |
| C# | `csharp/BasicChat.cs` | 使用 .NET 的串流聊天 |
| JavaScript | `javascript/foundry-local.mjs` | 使用 Node.js 的串流聊天 |

---

### 模組 4：Retrieval-Augmented Generation (RAG)

**實驗室指南：** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- 甚麼是 RAG 以及其重要性
- 建立記憶體內知識庫
- 使用關鍵字重疊檢索與評分
- 撰寫具根據性的系統提示
- 在裝置上執行完整的 RAG 流程

**程式範例：**

| 語言 | 檔案 |
|-------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 模組 5：建立 AI 代理

**實驗室指南：** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- 何謂 AI 代理（與直接呼叫大型語言模型的差異）
- `ChatAgent` 模式與 Microsoft Agent Framework
- 系統指令、角色設定及多輪對話
- 代理的結構化輸出（JSON）

**程式範例：**

| 語言 | 檔案 | 描述 |
|-------|--------|--------|
| Python | `python/foundry-local-with-agf.py` | 使用 Agent Framework 的單一代理 |
| C# | `csharp/SingleAgent.cs` | 單一代理（ChatAgent 模式） |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | 單一代理（ChatAgent 模式） |

---

### 模組 6：多代理工作流程

**實驗室指南：** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- 多代理流程：研究員 → 作家 → 編輯
- 依序編排及回饋迴圈
- 共享設定及結構化交接
- 設計你自己的多代理工作流程

**程式範例：**

| 語言 | 檔案 | 描述 |
|-------|---------|---------|
| Python | `python/foundry-local-multi-agent.py` | 三代理流程 |
| C# | `csharp/MultiAgent.cs` | 三代理流程 |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 三代理流程 |

---

### 模組 7：Zava 創意寫作助手 - 頂點應用

**實驗室指南：** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 一個生產級多代理應用，包含 4 個專精代理
- 依序流程及評估者驅動的回饋迴圈
- 串流輸出、產品目錄搜尋、結構化 JSON 交接
- Python (FastAPI)、JavaScript (Node.js CLI) 和 C# (.NET 控制台) 的完整實現

**程式範例：**

| 語言 | 目錄 | 描述 |
|-------|-------------------------|--------------------------|
| Python | `zava-creative-writer-local/src/api/` | 領航器的 FastAPI 網路服務 |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI 應用 |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 控制台應用 |

---

### 模組 8：評估驅動開發

**實驗室指南：** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- 使用黃金數據集為 AI 代理建立系統性評估框架
- 規則基檢查（長度、關鍵字覆蓋、禁用詞）及 LLM 評判評分
- 並行比較不同提示變體並彙整分數表
- 擴展模組 7 中 Zava 編輯者代理模式至離線測試套件
- Python、JavaScript 及 C# 三個分支

**程式範例：**

| 語言 | 檔案 | 描述 |
|-------|------|-------|
| Python | `python/foundry-local-eval.py` | 評估框架 |
| C# | `csharp/AgentEvaluation.cs` | 評估框架 |
| JavaScript | `javascript/foundry-local-eval.mjs` | 評估框架 |

---

### 模組 9：使用 Whisper 進行語音轉錄

**實驗室指南：** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- 使用本地運行的 OpenAI Whisper 進行語音轉文字轉錄
- 私隱優先的音訊處理 — 音訊永不離開您的裝置
- Python、JavaScript 和 C# 範例，分別使用 `client.audio.transcriptions.create()`（Python/JS）和 `AudioClient.TranscribeAudioAsync()`（C#）
- 包含以 Zava 為主題的範例音訊檔案，供實際操作練習

**程式碼範例：**

| 語言 | 檔案 | 描述 |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper 語音轉錄 |
| C# | `csharp/WhisperTranscription.cs` | Whisper 語音轉錄 |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper 語音轉錄 |

> **注意：** 此實驗利用 **Foundry Local SDK** 程式化下載並載入 Whisper 模型，然後將音訊發送至本地兼容 OpenAI 的端點進行轉錄。Whisper 模型（`whisper`）列於 Foundry Local 目錄中，完全在裝置上執行 — 無需使用雲端 API 金鑰或網絡存取。

---

### 第10部分：使用自訂或 Hugging Face 模型

**實驗指南：** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- 使用 ONNX Runtime GenAI 模型生成器將 Hugging Face 模型編譯為優化的 ONNX 格式
- 針對特定硬體編譯（CPU、NVIDIA GPU、DirectML、WebGPU）及數值量化（int4、fp16、bf16）
- 為 Foundry Local 建立聊天模板配置檔
- 將編譯後的模型新增至 Foundry Local 快取
- 透過 CLI、REST API 及 OpenAI SDK 運行自訂模型
- 範例參考：Qwen/Qwen3-0.6B 端到端編譯

---

### 第11部分：本地模型的工具調用

**實驗指南：** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- 讓本地模型能調用外部函式（工具／函式調用）
- 使用 OpenAI 函式調用格式定義工具模式
- 處理多輪工具調用對話流程
- 本地執行工具調用並向模型返回結果
- 選擇合適模型應對工具調用場景（Qwen 2.5、Phi-4-mini）
- 使用 SDK 原生的 `ChatClient` 做工具調用（JavaScript）

**程式碼範例：**

| 語言 | 檔案 | 描述 |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | 使用氣象／人口工具的工具調用 |
| C# | `csharp/ToolCalling.cs` | .NET 平台上的工具調用 |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | 使用 ChatClient 進行工具調用 |

---

### 第12部分：為 Zava 創意寫作員建置 Web UI

**實驗指南：** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- 為 Zava 創意寫作員新增瀏覽器前端
- 使用 Python（FastAPI）、JavaScript（Node.js HTTP）、C#（ASP.NET Core）服務共用 UI
- 在瀏覽器中利用 Fetch API 與 ReadableStream 消費串流 NDJSON
- 實時顯示代理狀態徽章及文章文字串流

**程式碼（共用 UI）：**

| 檔案 | 描述 |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | 頁面佈局 |
| `zava-creative-writer-local/ui/style.css` | 樣式設定 |
| `zava-creative-writer-local/ui/app.js` | 串流閱讀器與 DOM 更新邏輯 |

**後端新增項目：**

| 語言 | 檔案 | 描述 |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | 更新以服務靜態 UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | 封裝協調器的新 HTTP 伺服器 |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 新 ASP.NET Core 最小 API 專案 |

---

### 第13部分：工作坊完成

**實驗指南：** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 回顧您在全部12個部分建置的內容總結
- 擴展應用的進一步構想
- 相關資源與文件連結

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
| Foundry Local 官方網站 | [foundrylocal.ai](https://foundrylocal.ai) |
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

**祝您建置愉快！🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**免責聲明**：
本文件是使用 AI 翻譯服務 [Co-op Translator](https://github.com/Azure/co-op-translator) 翻譯而成。雖然我們致力於確保準確性，但請注意，自動翻譯可能包含錯誤或不準確之處。原始文件的原文版本應視為權威來源。對於重要資訊，建議採用專業人工翻譯。我們不對因使用本翻譯而引起的任何誤解或誤釋承擔責任。
<!-- CO-OP TRANSLATOR DISCLAIMER END -->