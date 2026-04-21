<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local 工作坊 - 在裝置上構建 AI 應用程式

一個實作工作坊，教你如何在自己的機器上運行語言模型，並使用 [Foundry Local](https://foundrylocal.ai) 和 [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) 建立智慧型應用程式。

> **什麼是 Foundry Local？** Foundry Local 是一個輕量級執行環境，讓你可以在完全由硬體運作的環境中下載、管理並提供語言模型服務。它透過 **OpenAI 相容的 API** 介面，讓任何能使用 OpenAI 的工具或 SDK 都能連接使用—不需要雲端帳戶。

### 🌐 多語言支援

#### 透過 GitHub Action 支援（自動化且始終保持最新）

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[阿拉伯語](../ar/README.md) | [孟加拉語](../bn/README.md) | [保加利亞語](../bg/README.md) | [緬甸語 (Myanmar)](../my/README.md) | [中文 (簡體)](../zh-CN/README.md) | [中文 (繁體，香港)](../zh-HK/README.md) | [中文 (繁體，澳門)](../zh-MO/README.md) | [中文 (繁體，台灣)](./README.md) | [克羅埃西亞語](../hr/README.md) | [捷克語](../cs/README.md) | [丹麥語](../da/README.md) | [荷蘭語](../nl/README.md) | [愛沙尼亞語](../et/README.md) | [芬蘭語](../fi/README.md) | [法語](../fr/README.md) | [德語](../de/README.md) | [希臘語](../el/README.md) | [希伯來語](../he/README.md) | [印地語](../hi/README.md) | [匈牙利語](../hu/README.md) | [印尼語](../id/README.md) | [義大利語](../it/README.md) | [日語](../ja/README.md) | [坎納達語](../kn/README.md) | [高棉語](../km/README.md) | [韓語](../ko/README.md) | [立陶宛語](../lt/README.md) | [馬來語](../ms/README.md) | [馬拉雅拉姆語](../ml/README.md) | [馬拉地語](../mr/README.md) | [尼泊爾語](../ne/README.md) | [奈及利亞皮欽語](../pcm/README.md) | [挪威語](../no/README.md) | [波斯語 (法爾西語)](../fa/README.md) | [波蘭語](../pl/README.md) | [葡萄牙語 (巴西)](../pt-BR/README.md) | [葡萄牙語 (葡萄牙)](../pt-PT/README.md) | [旁遮普語 (Gurmukhi)](../pa/README.md) | [羅馬尼亞語](../ro/README.md) | [俄語](../ru/README.md) | [塞爾維亞語 (西里爾字母)](../sr/README.md) | [斯洛伐克語](../sk/README.md) | [斯洛文尼亞語](../sl/README.md) | [西班牙語](../es/README.md) | [斯瓦希里語](../sw/README.md) | [瑞典語](../sv/README.md) | [塔加洛語 (菲律賓語)](../tl/README.md) | [泰米爾語](../ta/README.md) | [泰盧固語](../te/README.md) | [泰語](../th/README.md) | [土耳其語](../tr/README.md) | [烏克蘭語](../uk/README.md) | [烏爾都語](../ur/README.md) | [越南語](../vi/README.md)

> **想要本機端複製？**
>
> 此儲存庫包含 50+ 種語言翻譯，會大幅增加下載大小。若想不下載翻譯內容，可使用 sparse checkout：
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
> 這樣即可快速下載所有完成課程所需的內容。
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## 學習目標

完成此工作坊後，你將能：

| # | 目標 |
|---|-------|
| 1 | 安裝 Foundry Local 並使用 CLI 管理模型 |
| 2 | 精通 Foundry Local SDK API 以程式化管理模型 |
| 3 | 使用 Python、JavaScript 與 C# SDK 連接本地推理伺服器 |
| 4 | 建立以檢索增強生成 (RAG) 的管線，答案依賴於你的資料庫 |
| 5 | 建立具有持久指令及角色的 AI 智能代理人 |
| 6 | 協調多代理工作流程並植入反饋迴路 |
| 7 | 探索生產級的壓軸應用——Zava 創意作家 |
| 8 | 使用黃金資料集和 LLM 作為評判的評估框架 |
| 9 | 使用 Whisper 進行音訊轉錄—裝置內語音轉文字 |
| 10 | 使用 ONNX Runtime GenAI 與 Foundry Local 編譯並執行自訂或 Hugging Face 模型 |
| 11 | 讓本地模型調用外部函式，實現工具調用模式 |
| 12 | 建構基於瀏覽器的 Zava 創意作家 UI，並支援實時串流 |

---

## 先決條件

| 需求 | 詳細說明 |
|-------|-----------|
| <strong>硬體</strong> | 至少 8 GB 記憶體（建議 16 GB）；支援 AVX2 的 CPU 或合適的 GPU |
| <strong>作業系統</strong> | Windows 10/11 (x64/ARM)、Windows Server 2025 或 macOS 13+ |
| **Foundry Local CLI** | Windows 使用 `winget install Microsoft.FoundryLocal`，macOS 使用 `brew tap microsoft/foundrylocal && brew install foundrylocal` 安裝。詳情見 [入門指南](https://learn.microsoft.com/en-us/azure/foundry-local/get-started)。 |
| <strong>程式語言執行環境</strong> | **Python 3.9+** 或 **.NET 9.0+** 或 **Node.js 18+** |
| **Git** | 用於複製此儲存庫 |

---

## 快速開始

```bash
# 1. 複製倉庫
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. 驗證已安裝 Foundry Local
foundry model list              # 列出可用模型
foundry model run phi-3.5-mini  # 開始互動式聊天

# 3. 選擇你的語言軌道（完整設定請參見第二部分實驗）
```

| 語言 | 快速啟動指令 |
|--------|--------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## 工作坊各單元

### 第 1 部分：Foundry Local 入門

**實驗指導：** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- 認識 Foundry Local 及其運作方式
- Windows 與 macOS 安裝 CLI
- 探索模型——列出、下載、執行
- 了解模型別名與動態連接埠

---

### 第 2 部分：Foundry Local SDK 深入解析

**實驗指導：** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- 為何在應用開發使用 SDK 優於 CLI
- Python、JavaScript、C# 全套 SDK API 參考
- 服務管理、目錄瀏覽、模型生命周期管理（下載、載入、卸載）
- 快速啟動範例：Python 建構子啟動、JavaScript 的 `init()`、C# 的 `CreateAsync()`
- `FoundryModelInfo` 元資料、別名與硬體優化模型選擇

---

### 第 3 部分：SDKs 與 API

**實驗指導：** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- 從 Python、JavaScript 與 C# 連接 Foundry Local
- 使用 Foundry Local SDK 程式化服務管理
- 透過 OpenAI 兼容 API 進行串流聊天完成
- 三種語言的 SDK 方法參考

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|-------|-------|-------|
| Python | `python/foundry-local.py` | 基本串流聊天 |
| C# | `csharp/BasicChat.cs` | 使用 .NET 的串流聊天 |
| JavaScript | `javascript/foundry-local.mjs` | 使用 Node.js 的串流聊天 |

---

### 第 4 部分：檢索增強生成 (RAG)

**實驗指導：** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- 什麼是 RAG 且其重要性
- 建構記憶體內知識庫
- 以關鍵字重疊為基礎的檢索與打分
- 撰寫有根據的系統提示
- 在裝置上完整執行 RAG 流程

**程式碼範例：**

| 語言 | 檔案 |
|-------|-------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### 第 5 部分：打造 AI 智能代理人

**實驗指導：** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- 什麼是 AI 代理人（與直接呼叫大型語言模型的不同）
- `ChatAgent` 模式與 Microsoft Agent Framework
- 系統指令、角色設定及多輪對話
- 代理人的結構化輸出 (JSON)

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|-------|-------|-------|
| Python | `python/foundry-local-with-agf.py` | 使用 Agent Framework 的單一代理人 |
| C# | `csharp/SingleAgent.cs` | 單一代理人 (`ChatAgent` 模式) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | 單一代理人 (`ChatAgent` 模式) |

---

### 第 6 部分：多代理工作流程

**實驗指導：** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- 多代理管線：研究員 → 寫手 → 編輯
- 順序協同與反饋迴路
- 共享配置與結構化交接
- 設計你自己的多代理工作流程

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|-------|-------|-------|
| Python | `python/foundry-local-multi-agent.py` | 三代理管線 |
| C# | `csharp/MultiAgent.cs` | 三代理管線 |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | 三代理管線 |

---

### 第 7 部分：Zava 創意作家 - 壓軸應用程式

**實驗指導：** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 具備四個專職代理人的生產級多代理應用
- 以評估者驅動的反饋迴路順序執行管線
- 實現串流輸出、產品目錄檢索、結構化 JSON 交接
- Python (FastAPI)、JavaScript (Node.js CLI)、C# (.NET 控制台) 全方位實作

**程式碼範例：**

| 語言 | 目錄 | 說明 |
|-------|----------|-------|
| Python | `zava-creative-writer-local/src/api/` | 具協調者的 FastAPI Web 服務 |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI 應用 |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 控制台應用 |

---

### 第 8 部分：評估引導的開發

**實驗指導：** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- 使用黃金資料集建立 AI 代理的系統化評估框架
- 規則檢查（長度、關鍵字覆蓋、禁止詞彙）與 LLM 作為評判的打分
- 對比多種提示變體並合成評分報告
- 將第 7 部分的 Zava 編輯代理擴展成離線測試套件
- Python、JavaScript 與 C# 分支實作

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|-------|-------|-------|
| Python | `python/foundry-local-eval.py` | 評估框架 |
| C# | `csharp/AgentEvaluation.cs` | 評估框架 |
| JavaScript | `javascript/foundry-local-eval.mjs` | 評估框架 |

---

### 第 9 部分：使用 Whisper 的語音轉錄

**實驗指導：** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- 使用本地運行的 OpenAI Whisper 進行語音到文字轉錄
- 注重隱私的音訊處理 — 音頻資料永遠不會離開您的設備
- 透過 `client.audio.transcriptions.create()`（Python/JS）和 `AudioClient.TranscribeAudioAsync()`（C#）實現 Python、JavaScript 和 C# 範例
- 包含以 Zava 為主題的示例音訊檔案以供實作練習

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper 語音轉錄 |
| C# | `csharp/WhisperTranscription.cs` | Whisper 語音轉錄 |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper 語音轉錄 |

> **注意：** 本實驗室使用 **Foundry Local SDK** 程式化下載並載入 Whisper 模型，接著將音訊發送至本地相容 OpenAI 的端點進行轉錄。Whisper 模型（`whisper`）列在 Foundry Local 目錄中，完全在設備上運行 — 無需雲端 API 金鑰或網路存取。

---

### 第10部分：使用自訂或 Hugging Face 模型

**實驗指引：** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- 使用 ONNX Runtime GenAI 模型建構器將 Hugging Face 模型編譯成優化的 ONNX 格式
- 硬體特定編譯（CPU、NVIDIA GPU、DirectML、WebGPU）及量化（int4、fp16、bf16）
- 為 Foundry Local 建立對話模板配置檔
- 將編譯後的模型新增至 Foundry Local 快取
- 透過 CLI、REST API 及 OpenAI SDK 運行自訂模型
- 範例參考：完整編譯 Qwen/Qwen3-0.6B

---

### 第11部分：使用本地模型進行工具呼叫

**實驗指引：** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- 啟用本地模型呼叫外部函式（工具/函式呼叫）
- 使用 OpenAI 函式呼叫格式定義工具結構
- 處理多輪工具呼叫對話流程
- 本地執行工具呼叫並回傳結果給模型
- 為工具呼叫場景選擇適合的模型（Qwen 2.5、Phi-4-mini）
- 使用 SDK 原生的 `ChatClient` 進行工具呼叫（JavaScript）

**程式碼範例：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | 天氣/人口工具的工具呼叫 |
| C# | `csharp/ToolCalling.cs` | 使用 .NET 的工具呼叫 |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | 使用 ChatClient 的工具呼叫 |

---

### 第12部分：為 Zava 創意作家建構網頁使用者介面

**實驗指引：** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- 為 Zava 創意作家新增瀏覽器前端
- 從 Python（FastAPI）、JavaScript（Node.js HTTP）、C#（ASP.NET Core）提供共享的 UI 服務
- 在瀏覽器中使用 Fetch API 和 ReadableStream 接收串流 NDJSON
- 實時代理狀態徽章及文章文字串流

**程式碼（共享 UI）：**

| 檔案 | 說明 |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | 頁面配置 |
| `zava-creative-writer-local/ui/style.css` | 樣式設計 |
| `zava-creative-writer-local/ui/app.js` | 串流讀取器及 DOM 更新邏輯 |

**後端新增：**

| 語言 | 檔案 | 說明 |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | 更新以提供靜態 UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | 用以包裝協調器之新的 HTTP 伺服器 |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | 新增 ASP.NET Core 最小 API 專案 |

---

### 第13部分：工作坊完成

**實驗指引：** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- 總結您在 12 個部分建構的所有內容
- 擴充應用程式的進階想法
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
| Foundry Local 官方網站 | [foundrylocal.ai](https://foundrylocal.ai) |
| 模型目錄 | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| 入門指南 | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK 參考 | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## 授權條款

本工作坊教材僅供教育用途。

---

**祝您開發愉快！🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**免責聲明**：  
本文件係使用 AI 翻譯服務 [Co-op Translator](https://github.com/Azure/co-op-translator)翻譯而成。雖然我們力求準確，但請注意，自動翻譯可能包含錯誤或不準確之處。原始文件的母語版本應被視為權威來源。對於關鍵資訊，建議尋求專業人工翻譯。我們對因使用本翻譯產生的任何誤解或錯誤詮釋不承擔任何責任。
<!-- CO-OP TRANSLATOR DISCLAIMER END -->