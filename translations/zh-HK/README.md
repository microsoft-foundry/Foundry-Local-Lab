<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local 工作坊 - 在裝置上構建 AI 應用程式

一個實踐工作坊，教你如何在自己的機器上執行語言模型，並利用 [Foundry Local](https://foundrylocal.ai) 及 [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) 建立智能應用程式。

> **什麼是 Foundry Local？** Foundry Local 是一個輕量級執行時，讓你可以完全在你的硬體上下載、管理及提供語言模型服務。它提供一個 **OpenAI 相容的 API**，任何能使用 OpenAI 的工具或 SDK 都可以連接 — 無需雲端帳戶。

### 🌐 多語言支援

#### 透過 GitHub Action 支援（自動且保持最新）

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[阿拉伯語](../ar/README.md) | [孟加拉語](../bn/README.md) | [保加利亞語](../bg/README.md) | [緬甸語 (Myanmar)](../my/README.md) | [中文 (簡體)](../zh-CN/README.md) | [中文 (繁體, 香港)](./README.md) | [中文 (繁體, 澳門)](../zh-MO/README.md) | [中文 (繁體, 台灣)](../zh-TW/README.md) | [克羅地亞語](../hr/README.md) | [捷克語](../cs/README.md) | [丹麥語](../da/README.md) | [荷蘭語](../nl/README.md) | [愛沙尼亞語](../et/README.md) | [芬蘭語](../fi/README.md) | [法語](../fr/README.md) | [德語](../de/README.md) | [希臘語](../el/README.md) | [希伯來語](../he/README.md) | [印地語](../hi/README.md) | [匈牙利語](../hu/README.md) | [印尼語](../id/README.md) | [義大利語](../it/README.md) | [日語](../ja/README.md) | [坎納達語](../kn/README.md) | [高棉語](../km/README.md) | [韓語](../ko/README.md) | [立陶宛語](../lt/README.md) | [馬來語](../ms/README.md) | [馬拉雅拉姆語](../ml/README.md) | [馬拉地語](../mr/README.md) | [尼泊爾語](../ne/README.md) | [尼日利亞洋泾浜語](../pcm/README.md) | [挪威語](../no/README.md) | [波斯語 (法爾西語)](../fa/README.md) | [波蘭語](../pl/README.md) | [葡萄牙語 (巴西)](../pt-BR/README.md) | [葡萄牙語 (葡萄牙)](../pt-PT/README.md) | [旁遮普語 (Gurmukhi)](../pa/README.md) | [羅馬尼亞語](../ro/README.md) | [俄語](../ru/README.md) | [塞爾維亞語 (西里爾字母)](../sr/README.md) | [斯洛伐克語](../sk/README.md) | [斯洛維尼亞語](../sl/README.md) | [西班牙語](../es/README.md) | [斯瓦希里語](../sw/README.md) | [瑞典語](../sv/README.md) | [他加祿語 (菲律賓語)](../tl/README.md) | [泰米爾語](../ta/README.md) | [特盧固語](../te/README.md) | [泰語](../th/README.md) | [土耳其語](../tr/README.md) | [烏克蘭語](../uk/README.md) | [烏爾都語](../ur/README.md) | [越南語](../vi/README.md)

> **想本機 Clone？**
>
> 這個倉庫包含 50+ 種語言翻譯，會大幅增加下載大小。若想不帶翻譯克隆，請使用稀疏檢出：
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> 這樣能更快取得完成課程所需的所有內容。
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## 學習目標

完成此工作坊後，你將能夠：

| # | 目標 |
|---|-----------|
| 1 | 安裝 Foundry Local 並使用 CLI 管理模型 |
| 2 | 精通 Foundry Local SDK API 以程式化管理模型 |
| 3 | 使用 Python、JavaScript 和 C# SDK 連接本地推理服務器 |
| 4 | 建立檢索增強生成 (RAG) 流程，根據你的數據提供答案根據 |
| 5 | 創建具持久指令與角色的 AI 代理 |
| 6 | 編排多代理工作流程並引入反饋迴圈 |
| 7 | 探索生產級別的結業作品 — Zava 創意寫作助手 |
| 8 | 利用黃金數據集和 LLM 評判制定評估框架 |
| 9 | 使用 Whisper 進行語音轉文字 — 內建語音辨識 |
| 10 | 使用 ONNX Runtime GenAI 及 Foundry Local 編譯和運行自訂或 Hugging Face 模型 |
| 11 | 讓本地模型能呼叫外部函數，使用工具呼叫範式 |
| 12 | 為 Zava 創意寫作助手構建瀏覽器 UI，實現實時串流 |

---

## 先決條件

| 要求 | 詳情 |
|-------------|---------|
| <strong>硬體</strong> | 最少 8 GB RAM（建議 16 GB）；支援 AVX2 的 CPU 或相容 GPU |
| <strong>作業系統</strong> | Windows 10/11 (x64/ARM)、Windows Server 2025 或 macOS 13+ |
| **Foundry Local CLI** | 透過 `winget install Microsoft.FoundryLocal` (Windows) 或 `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) 安裝。詳見 [入門指南](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) 。 |
| <strong>語言執行環境</strong> | **Python 3.9+** 和／或 **.NET 9.0+** 和／或 **Node.js 18+** |
| **Git** | 用於克隆此倉庫 |

---

## 入門指南

```bash
# 1. 複製倉庫
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. 確認已安裝 Foundry Local
foundry model list              # 列出可用模型
foundry model run phi-3.5-mini  # 開始互動式聊天

# 3. 選擇你的語言路線（參閱第2部分實驗室作完整設置）
```

| 語言 | 快速開始 |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## 工作坊章節

### 第一部分：Foundry Local 入門

**實驗指南：** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- 什麼是 Foundry Local 及其運作方式
- 在 Windows 和 macOS 上安裝 CLI
- 探索模型：列出、下載、運行
- 了解模型別名和動態端口

---

### 第二部分：Foundry Local SDK 深入探討

**實驗指南：** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- 為什麼開發應用時要用 SDK 而非 CLI
- Python、JavaScript 和 C# 的完整 SDK API 參考
- 服務管理、目錄瀏覽、模型生命週期（下載、載入、卸載）
- 快速啟動模式：Python 建構子引導、JavaScript `init()`、C# `CreateAsync()`
- `FoundryModelInfo` 元資料、別名及硬體最佳模型選擇

---

### 第三部分：SDK 與 API

**實驗指南：** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- 從 Python、JavaScript 和 C# 連接 Foundry Local
- 使用 Foundry Local SDK 程式化管理服務
- 透過 OpenAI 相容 API 進行串流聊天補全
- 各語言的 SDK 方法參考

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local.py` | 基本串流聊天 |
| C# | `csharp/BasicChat.cs` | 使用 .NET 的串流聊天 |
| JavaScript | `javascript/foundry-local.mjs` | 使用 Node.js 的串流聊天 |

---

### 第四部分：檢索增強生成 (RAG)

**實驗指南：** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- 什麼是 RAG 及其重要性
- 建立記憶體內知識庫
- 關鍵詞重疊檢索及打分
- 組合有根據的系統提示
- 在裝置上完整執行 RAG 流程

**程式碼範例：**

| 語言 | 檔案 |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 第五部分：建構 AI 代理

**實驗指南：** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- 何謂 AI 代理（與純 LLM 呼叫的比較）
- `ChatAgent` 範式與 Microsoft Agent Framework
- 系統指令、角色設定及多回合對話
- 代理輸出結構化資料（JSON）

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | 使用 Agent Framework 的單一代理 |
| C# | `csharp/SingleAgent.cs` | 單一代理 (ChatAgent 範式) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | 單一代理 (ChatAgent 範式) |

---

### 第六部分：多代理工作流程

**實驗指南：** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- 多代理流程：研究員 → 寫手 → 編輯
- 順序編排與反饋迴圈
- 共享配置與結構化轉接
- 設計自己的多代理工作流程

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | 三代理流程 |
| C# | `csharp/MultiAgent.cs` | 三代理流程 |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 三代理流程 |

---

### 第七部分：Zava 創意寫作助手 - 結業作品

**實驗指南：** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 一個擁有 4 個專業代理的生產級多代理應用
- 順序管線搭配評估員驅動的反饋迴圈
- 串流輸出、產品目錄查詢、結構化 JSON 交接
- 完整實現於 Python (FastAPI)、JavaScript (Node.js CLI) 和 C# (.NET 控制台應用)

**程式碼範例：**

| 語言 | 目錄 | 說明 |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI 網路服務及協調器 |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI 應用 |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 控制台應用 |

---

### 第八部分：評估驅動開發

**實驗指南：** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- 使用黃金數據集建構 AI 代理系統評估框架
- 規則檢查（長度、關鍵詞涵蓋、禁用詞）+ 以 LLM 為評判的評分
- 提示變體並排比較與彙總評分表
- 將第七部分的 Zava 編輯代理模式擴展為離線測試套件
- Python、JavaScript 與 C# 版本

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | 評估框架 |
| C# | `csharp/AgentEvaluation.cs` | 評估框架 |
| JavaScript | `javascript/foundry-local-eval.mjs` | 評估框架 |

---

### 第九部分：使用 Whisper 進行語音轉譯

**實驗指南：** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- 使用本地運行的 OpenAI Whisper 進行語音轉文字轉錄
- 以隱私為先的音訊處理——音訊永遠不會離開您的裝置
- Python、JavaScript 和 C# 範例，使用 `client.audio.transcriptions.create()`（Python/JS）及 `AudioClient.TranscribeAudioAsync()`（C#）
- 包含 Zava 主題的範例音訊檔，便於實作練習

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper 語音轉錄 |
| C# | `csharp/WhisperTranscription.cs` | Whisper 語音轉錄 |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper 語音轉錄 |

> **注意：** 本實驗室使用 **Foundry Local SDK** 程式化下載及載入 Whisper 模型，然後將音訊傳送到本地 OpenAI 相容端點進行轉錄。Whisper 模型 (`whisper`) 已列於 Foundry Local 目錄中，完全在裝置上執行——無需雲端 API 金鑰或網絡存取。

---

### 第 10 部分：使用自訂或 Hugging Face 模型

**實驗室指南：** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- 使用 ONNX Runtime GenAI 模型建構器將 Hugging Face 模型編譯為優化的 ONNX 格式
- 針對硬體特性編譯（CPU、NVIDIA GPU、DirectML、WebGPU）及量化（int4、fp16、bf16）
- 為 Foundry Local 建立聊天範本配置檔
- 將編譯後模型加入 Foundry Local 快取
- 透過 CLI、REST API 及 OpenAI SDK 執行自訂模型
- 參考範例：端對端編譯 Qwen/Qwen3-0.6B

---

### 第 11 部分：使用本地模型進行工具呼叫

**實驗室指南：** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- 使本地模型能夠呼叫外部功能（工具／功能呼叫）
- 使用 OpenAI 函數呼叫格式定義工具結構
- 處理多輪工具呼叫對話流程
- 在本地執行工具呼叫並將結果回傳給模型
- 選擇適合工具呼叫場景的模型（Qwen 2.5、Phi-4-mini）
- 使用 SDK 內建的 `ChatClient` 執行工具呼叫（JavaScript）

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | 天氣／人口工具呼叫 |
| C# | `csharp/ToolCalling.cs` | 使用 .NET 的工具呼叫 |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | 使用 ChatClient 的工具呼叫 |

---

### 第 12 部分：為 Zava 創意寫作器打造網頁 UI

**實驗室指南：** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- 為 Zava 創意寫作器新增瀏覽器前端
- 由 Python（FastAPI）、JavaScript（Node.js HTTP）及 C#（ASP.NET Core）提供共用 UI
- 使用 Fetch API 和 ReadableStream 在瀏覽器中接收 NDJSON 串流
- 即時代理狀態徽章及文章文字串流呈現

**程式碼（共用 UI）：**

| 檔案 | 說明 |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | 頁面佈局 |
| `zava-creative-writer-local/ui/style.css` | 樣式設定 |
| `zava-creative-writer-local/ui/app.js` | 串流讀取和 DOM 更新邏輯 |

**後端新增項目：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | 更新以提供靜態 UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | 新增 HTTP 伺服器，包裝協調器 |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 新增 ASP.NET Core Minimal API 專案 |

---

### 第 13 部分：工作坊結束

**實驗室指南：** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 整理您於所有 12 部分中完成的內容
- 延伸應用的進一步想法
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
| 開始指南 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 參考 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## 授權

本工作坊教材僅供教育用途。

---

**祝您開發愉快！🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**免責聲明**：  
本文件由 AI 翻譯服務 [Co-op Translator](https://github.com/Azure/co-op-translator) 進行翻譯。雖然我們努力追求準確性，但請注意自動翻譯可能包含錯誤或不準確之處。請將原始文件的原文版本視為權威來源。對於重要資訊，建議聘請專業人工翻譯。本公司不對因使用此翻譯而產生的任何誤解或誤譯承擔責任。
<!-- CO-OP TRANSLATOR DISCLAIMER END -->