# Changelog — Foundry Local Workshop

所有對此工作坊的顯著變更記錄如下。

---

## 2026-03-11 — 第12及13部分、網頁介面、Whisper 重寫、WinML/QNN 修正及驗證

### 新增
- **第12部分：為 Zava 創意作家構建網頁介面** — 新實驗指南（`labs/part12-zava-ui.md`），包含串流 NDJSON、瀏覽器 `ReadableStream`、即時代理狀態徽章以及實時文章文字串流練習
- **第13部分：工作坊完成** — 新摘要實驗（`labs/part13-workshop-complete.md`），回顧全部12部分，進一步構想及資源連結
- **Zava UI 前端：** `zava-creative-writer-local/ui/index.html`、`style.css`、`app.js` — 三個後端共用的原生 HTML/CSS/JS 瀏覽器介面
- **JavaScript HTTP 伺服器：** `zava-creative-writer-local/src/javascript/server.mjs` — 新增 Express 風格 HTTP 伺服器，包裝協調器以供瀏覽器訪問
- **C# ASP.NET Core 後端：** `zava-creative-writer-local/src/csharp-web/Program.cs` 及 `ZavaCreativeWriterWeb.csproj` — 新增最簡 API 專案，服務 UI 及串流 NDJSON
- **音頻範例產生器：** `samples/audio/generate_samples.py` — 使用 `pyttsx3` 離線 TTS 腳本，產生第9部分的 Zava 主題 WAV 檔案
- **音頻範例：** `samples/audio/zava-full-project-walkthrough.wav` — 新增較長音頻範例用於轉錄測試
- **驗證腳本：** `validate-npu-workaround.ps1` — 自動 PowerShell 腳本，用以驗證所有 C# 範例中的 NPU/QNN 變通方法
- **Mermaid 圖表 SVG：** `images/part12-architecture.svg`、`part12-message-types.svg`、`part12-streaming-sequence.svg`
- **WinML 跨平台支援：** 三個 C# `.csproj` 檔案（`csharp/csharp.csproj`、`zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`、`zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`）現在使用條件化 TFM 與互斥套件參考以實現跨平台支援。在 Windows 上：使用 `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML`（包含 QNN EP 插件的超集）。非 Windows 上：使用 `net9.0` TFM + `Microsoft.AI.Foundry.Local`（基礎 SDK）。Zava 專案中硬編碼的 `win-arm64` RID 已改為自動偵測。間接相依變通方法排除了帶有錯誤 win-arm64 參考的 `Microsoft.ML.OnnxRuntime.Gpu.Linux` 原生資產。所有7個 C# 檔案中先前的 try/catch NPU 變通方法已移除。

### 變更
- **第9部分（Whisper）：** 大幅重寫 — JavaScript 現在使用 SDK 內建的 `AudioClient` (`model.createAudioClient()`)，不再手動使用 ONNX Runtime推理；更新架構說明、比較表和流程圖，反映 JS/C# `AudioClient` 方法與 Python ONNX Runtime 方法的比較
- **第11部分：** 更新導航連結（現指向第12部分）；新增工具呼叫流程和序列的 SVG 渲染圖
- **第10部分：** 更新導航路徑，指向第12部分，而非結束工作坊
- **Python Whisper（`foundry-local-whisper.py`）：** 新增更多音頻範例與改進錯誤處理
- **JavaScript Whisper（`foundry-local-whisper.mjs`）：** 重寫為使用 `model.createAudioClient()` 搭配 `audioClient.transcribe()`，不再手動初始化 ONNX Runtime 會話
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`)：** 更新同時提供靜態 UI 檔案與 API
- **Zava C# 控制台（`zava-creative-writer-local/src/csharp/Program.cs`）：** 移除 NPU 變通方法（現由 WinML 套件處理）
- **README.md：** 新增第12部分，包含程式碼範例表與後端新增內容；新增第13部分；更新學習目標與專案結構
- **KNOWN-ISSUES.md：** 移除已解決的第7號問題（C# SDK NPU 模型變體，現由 WinML 套件處理）。剩餘問題重新編號為 #1–#6。更新環境資訊為 .NET SDK 10.0.104
- **AGENTS.md：** 更新專案結構樹，新增 `zava-creative-writer-local` 條目（`ui/`、`csharp-web/`、`server.mjs`）；更新 C# 主要套件與條件式 TFM 詳情
- **labs/part2-foundry-local-sdk.md：** 更新 `.csproj` 範例，展示完整跨平台模式，包含條件式 TFM、互斥套件參考及說明註解

### 驗證
- 三個 C# 專案（`csharp`、`ZavaCreativeWriter`、`ZavaCreativeWriterWeb`）可於 Windows ARM64 平台成功建立
- 聊天範例（`dotnet run chat`）：模型透過 WinML/QNN 載入為 `phi-3.5-mini-instruct-qnn-npu:1` — NPU 變體直接載入，無 CPU 回退
- 代理範例（`dotnet run agent`）：多輪對話完整運行，退出碼為 0
- Foundry Local CLI v0.8.117 及 SDK v0.9.0 運行於 .NET SDK 9.0.312

---

## 2026-03-11 — 程式碼修正、模型清理、Mermaid 圖表及驗證

### 修正
- **全部21個程式碼範例（7 Python、7 JavaScript、7 C#）：** 於程式退出時新增 `model.unload()` / `unload_model()` / `model.UnloadAsync()` 釋放，解決 OGA 記憶體洩漏警告（已知問題 #4）
- **csharp/WhisperTranscription.cs：** 用 `FindSamplesDirectory()` 取代易碎的 `AppContext.BaseDirectory` 相對路徑，可靠向上尋找 `samples/audio` 目錄（已知問題 #7）
- **csharp/csharp.csproj：** 以自動偵測 `$(NETCoreSdkRuntimeIdentifier)` 取代硬編碼 `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>`，使 `dotnet run` 不需 `-r` 參數即可跨平台運行（[Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)）

### 變更
- **第8部分：** 將使用 eval 推動的迴圈從 ASCII 盒狀圖改為 SVG 渲染圖
- **第10部分：** 將編譯流程圖從 ASCII 箭頭改為 SVG 渲染圖
- **第11部分：** 工具呼叫流程及序列圖轉為 SVG 渲染圖
- **第10部分：** 將「工作坊完成！」段落移至第11部分（最終實驗），原本位置改為「下一步」連結
- **KNOWN-ISSUES.md：** 對 CLI v0.8.117 重新驗證全部問題，移除已解決項目：OGA 記憶體洩漏（新增清理）、Whisper 路徑（FindSamplesDirectory）、HTTP 500 持續推論錯誤（無法重現，[＃494](https://github.com/microsoft/Foundry-Local/issues/494)）、tool_choice 限制（現可用 `required` 及特定功能指向於 qwen2.5-0.5b）。更新 JavaScript Whisper 問題，目前所有檔案輸出皆空／二進位（v0.9.x 回歸，嚴重度升為重大）。更新 #4 C# RID 自動偵測變通方法，[＃497](https://github.com/microsoft/Foundry-Local/issues/497) 連結。仍有7個未解決問題。
- **javascript/foundry-local-whisper.mjs：** 修正清理變數名稱 (`whisperModel` → `model`)

### 驗證
- Python: `foundry-local.py`、`foundry-local-rag.py`、`foundry-local-tool-calling.py` 皆成功執行並完成清理
- JavaScript: `foundry-local.mjs`、`foundry-local-rag.mjs`、`foundry-local-tool-calling.mjs` 皆成功執行並完成清理
- C#: `dotnet build` 成功，無警告無錯誤（net9.0 目標）
- 所有7個 Python 檔案通過 `py_compile` 語法檢查
- 所有7個 JavaScript 檔案通過 `node --check` 語法驗證

---

## 2026-03-10 — 第11部分：工具呼叫、SDK API 擴展及模型覆蓋

### 新增
- **第11部分：本地模型的工具呼叫** — 新實驗指南（`labs/part11-tool-calling.md`），包含8個練習，涵蓋工具結構、多輪流程、多重工具呼叫、自訂工具、ChatClient 工具呼叫以及 `tool_choice`
- **Python 範例：** `python/foundry-local-tool-calling.py` — 使用 OpenAI SDK 以 `get_weather`/`get_population` 工具呼叫
- **JavaScript 範例：** `javascript/foundry-local-tool-calling.mjs` — 使用 SDK 原生 `ChatClient` (`model.createChatClient()`) 呼叫工具
- **C# 範例：** `csharp/ToolCalling.cs` — 利用 OpenAI C# SDK 的 `ChatTool.CreateFunctionTool()` 呼叫工具
- **第2部分，第7練習：** 原生 `ChatClient` — JS: `model.createChatClient()`，C#: `model.GetChatClientAsync()`，作為 OpenAI SDK 的替代方案
- **第2部分，第8練習：** 模型變體及硬體選擇 — `selectVariant()`、`variants`、NPU 變體表（7個模型）
- **第2部分，第9練習：** 模型升級與目錄刷新 — `is_model_upgradeable()`、`upgrade_model()`、`updateModels()`
- **第2部分，第10練習：** 推理模型 — 包含 `<think>` 標籤解析範例的 `phi-4-mini-reasoning`
- **第3部分，第4練習：** `createChatClient` 作為 OpenAI SDK 的替代，含串流回調模式說明
- **AGENTS.md：** 新增工具呼叫、ChatClient 及推理模型的程式規範

### 變更
- **第1部分：** 擴充模型目錄 — 新增 phi-4-mini-reasoning、gpt-oss-20b、phi-4、qwen2.5-7b、qwen2.5-coder-7b、whisper-large-v3-turbo
- **第2部分：** 擴充 API 參考表 — 增加 `createChatClient`、`createAudioClient`、`removeFromCache`、`selectVariant`、`variants`、`isLoaded`、`stopWebService`、`is_model_upgradeable`、`upgrade_model`、`httpx_client`、`getModels`、`getCachedModels`、`getLoadedModels`、`updateModels`、`GetModelVariantAsync`、`UpdateModelsAsync`
- **第2部分：** 將練習 7-9 重新編號為 10-13 以容納新練習
- **第3部分：** 更新重點總結表，加入原生 ChatClient
- **README.md：** 新增第11部分，包含程式碼示範表；新增學習目標 #11；更新專案結構樹
- **csharp/Program.cs：** CLI 路由新增 `toolcall` 節點並更新說明文字

---

## 2026-03-09 — SDK v0.9.0 更新、英式英文及驗證通過

### 變更
- **所有程式碼範例（Python、JavaScript、C#）：** 更新至 Foundry Local SDK v0.9.0 API — 修正 `await catalog.getModel()`（之前缺少 `await`），更新 `FoundryLocalManager` 初始化模式，修正端點發現
- **所有實驗指南（第1-10部分）：** 轉換為英式英文（colour、catalogue、optimised 等）
- **所有實驗指南：** 更新 SDK 程式碼範例以符合 v0.9.0 API 表面
- **所有實驗指南：** 更新 API 參考表及練習程式碼區塊
- **JavaScript 關鍵修正：** 在 `catalog.getModel()` 加入缺漏的 `await` — 此函式返回 `Promise` 非模型物件，導致下游無聲故障

### 驗證
- 所有 Python 範例與 Foundry Local 服務成功執行
- 所有 JavaScript 範例成功運行（Node.js 18+）
- C# 專案可在 .NET 9.0 （由 net8.0 SDK 組件向前相容）成功編譯運行
- 工作坊中共修改及驗證29個檔案

---

## 檔案索引

| 檔案 | 最後更新 | 描述 |
|------|---------|------|
| `labs/part1-getting-started.md` | 2026-03-10 | 擴充模型目錄 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | 新增練習7-10，擴充 API 表 |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | 新增練習4（ChatClient），更新重點總結 |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid 圖表 |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid 圖表，工作坊完成移至第 11 部分 |
| `labs/part11-tool-calling.md` | 2026-03-11 | 新實驗室，Mermaid 圖表，工作坊完成部分 |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | 新增：工具調用範例 |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | 新增：工具調用範例 |
| `csharp/ToolCalling.cs` | 2026-03-10 | 新增：工具調用範例 |
| `csharp/Program.cs` | 2026-03-10 | 新增 `toolcall` CLI 指令 |
| `README.md` | 2026-03-10 | 第 11 部分，專案結構 |
| `AGENTS.md` | 2026-03-10 | 工具調用 + ChatClient 規範 |
| `KNOWN-ISSUES.md` | 2026-03-11 | 移除已解決的問題 #7，尚有 6 個開放問題 |
| `csharp/csharp.csproj` | 2026-03-11 | 跨平台 TFM，WinML/基本 SDK 條件性參考 |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | 跨平台 TFM，自動偵測 RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | 跨平台 TFM，自動偵測 RID |
| `csharp/BasicChat.cs` | 2026-03-11 | 移除 NPU try/catch 解決方法 |
| `csharp/SingleAgent.cs` | 2026-03-11 | 移除 NPU try/catch 解決方法 |
| `csharp/MultiAgent.cs` | 2026-03-11 | 移除 NPU try/catch 解決方法 |
| `csharp/RagPipeline.cs` | 2026-03-11 | 移除 NPU try/catch 解決方法 |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | 移除 NPU try/catch 解決方法 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | 跨平台 .csproj 範例 |
| `AGENTS.md` | 2026-03-11 | 更新 C# 套件及 TFM 詳細資訊 |
| `CHANGELOG.md` | 2026-03-11 | 此檔案 |