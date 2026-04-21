# 變更日誌 — Foundry Local 工作坊

本文檔記錄此工作坊所有重要的變更。

---

## 2026-03-11 — 第 12 & 13 部分，網頁介面，Whisper 重寫，WinML/QNN 修復及驗證

### 新增
- **第 12 部分：為 Zava 創意寫手建立網頁介面** — 新實驗室指導 (`labs/part12-zava-ui.md`)，涵蓋串流 NDJSON、瀏覽器 `ReadableStream`、即時代理狀態徽章及文章文字實時串流的練習
- **第 13 部分：工作坊總結** — 新總結實驗室 (`labs/part13-workshop-complete.md`)，回顧全部 12 部分，進一步的想法及資源連結
- **Zava UI 前端：** `zava-creative-writer-local/ui/index.html`、`style.css`、`app.js` — 所有三個後端共用的純 HTML/CSS/JS 瀏覽器介面
- **JavaScript HTTP 伺服器：** `zava-creative-writer-local/src/javascript/server.mjs` — 新增 Express 風格 HTTP 伺服器，包裝調度器以供瀏覽器訪問
- **C# ASP.NET Core 後端：** `zava-creative-writer-local/src/csharp-web/Program.cs` 和 `ZavaCreativeWriterWeb.csproj` — 新的極簡 API 專案，用於服務 UI 和串流 NDJSON
- **音訊範例產生器：** `samples/audio/generate_samples.py` — 使用 `pyttsx3` 離線 TTS 腳本，為第 9 部分生成以 Zava 為主題的 WAV 檔
- **音訊範例：** `samples/audio/zava-full-project-walkthrough.wav` — 新增更長的音訊範例用於轉錄測試
- **驗證腳本：** `validate-npu-workaround.ps1` — 自動 PowerShell 腳本，用於驗證所有 C# 範例的 NPU/QNN 解決方案
- **Mermaid 圖表 SVG：** `images/part12-architecture.svg`、`part12-message-types.svg`、`part12-streaming-sequence.svg`
- **WinML 跨平台支援：** 三個 C# `.csproj` 檔 (`csharp/csharp.csproj`、`zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`、`zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) 現在使用條件 TFM 與互斥套件參考以支援跨平台。Windows 平台：`net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML`（包含 QNN EP 插件的超集合）。非 Windows 平台：`net9.0` TFM + `Microsoft.AI.Foundry.Local`（基礎 SDK）。Zava 專案中硬編碼的 `win-arm64` RID 改為自動偵測。過渡性相依性解決方案排除帶有錯誤 win-arm64 參考的 `Microsoft.ML.OnnxRuntime.Gpu.Linux` 原生資產。所有 7 個 C# 檔案中的舊版 try/catch NPU 解決方案已移除。

### 變更
- **第 9 部分（Whisper）：** 大幅重寫 — JavaScript 現改用 SDK 內建的 `AudioClient` (`model.createAudioClient()`)，取代手動 ONNX Runtime 推理；更新架構描述、比較表與流程圖，反映 JS/C# `AudioClient` 方案與 Python ONNX Runtime 方案的不同
- **第 11 部分：** 更新導航連結（改指向第 12 部分）；新增工具調用流程及流程序列的渲染 SVG 圖
- **第 10 部分：** 更新導航，改為透過第 12 部分路由而非結束工作坊
- **Python Whisper (`foundry-local-whisper.py`)：** 增加更多音訊範例與改進錯誤處理
- **JavaScript Whisper (`foundry-local-whisper.mjs`)：** 重寫使用 `model.createAudioClient()` 和 `audioClient.transcribe()`，取代手動 ONNX Runtime 會話
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`)：** 更新以同時提供靜態 UI 檔案與 API
- **Zava C# 控制台 (`zava-creative-writer-local/src/csharp/Program.cs`)：** 移除 NPU 解決方案（現由 WinML 套件處理）
- **README.md：** 新增第 12 部分節，包含程式碼範例表及後端新增；新增第 13 部分節；更新學習目標及專案結構
- **KNOWN-ISSUES.md：** 移除已解決的 Issue #7（C# SDK NPU 模型變體 — 現由 WinML 套件處理）。剩餘問題重新編號為 #1～#6。並更新環境細節為 .NET SDK 10.0.104
- **AGENTS.md：** 更新專案結構樹，加入新 `zava-creative-writer-local` 項目（`ui/`、`csharp-web/`、`server.mjs`）；更新 C# 主要套件與條件 TFM 詳細資訊
- **labs/part2-foundry-local-sdk.md：** 更新 `.csproj` 範例，展示完整跨平台模式：條件 TFM、互斥套件參考與說明註解

### 驗證
- 三個 C# 專案 (`csharp`、`ZavaCreativeWriter`、`ZavaCreativeWriterWeb`) 在 Windows ARM64 平台成功編譯
- 聊天範例 (`dotnet run chat`)：模型透過 WinML/QNN 載入為 `phi-3.5-mini-instruct-qnn-npu:1` — NPU 變體可直接載入且不會回退 CPU
- Agent 範例 (`dotnet run agent`)：多輪對話端到端執行，結束碼為 0
- Foundry Local CLI v0.8.117 與 SDK v0.9.0 在 .NET SDK 9.0.312 上成功驗證

---

## 2026-03-11 — 程式碼修正、模型清理、Mermaid 圖表與驗證

### 修正
- **全部 21 個程式碼範例（7 Python、7 JavaScript、7 C#）：** 在結束時新增 `model.unload()` / `unload_model()` / `model.UnloadAsync()` 清理呼叫，以解決 OGA 記憶體洩漏警告（已知問題 #4）
- **csharp/WhisperTranscription.cs：** 以 `FindSamplesDirectory()` 取代易碎的 `AppContext.BaseDirectory` 相對路徑，該函式向上尋找目錄以穩定定位 `samples/audio`（已知問題 #7）
- **csharp/csharp.csproj：** 以自動偵測備援 `$(NETCoreSdkRuntimeIdentifier)` 取代硬編碼 `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>`，使 `dotnet run` 可在任意平台下無需 `-r` 參數執行 ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### 變更
- **第 8 部分：** 將評估驅動的迴圈由 ASCII 圖示轉為渲染 SVG 圖片
- **第 10 部分：** 將編譯流程圖由 ASCII 箭頭轉為渲染 SVG 圖片
- **第 11 部分：** 將工具調用流程與時序圖轉為渲染 SVG 圖片
- **第 10 部分：** 將「工作坊完成！」區段移至第 11 部分（最後實驗室）；改為「下一步」連結
- **KNOWN-ISSUES.md：** 對 CLI v0.8.117 全面重新驗證所有問題。移除已解決：OGA 記憶體洩漏（已新增清理）、Whisper 路徑（FindSamplesDirectory）、HTTP 500 持續推理錯誤（不可重現，[#494](https://github.com/microsoft/Foundry-Local/issues/494)）、tool_choice 限制（現可用於 `"required"` 及特定函式，qwen2.5-0.5b）。更新 JS Whisper 問題—所有檔案現皆回傳空輸出/二進位輸出（v0.9.x 回歸，嚴重性升至重大）。更新 #4 C# RID 自動偵測解決方案及 [#497](https://github.com/microsoft/Foundry-Local/issues/497) 連結。仍有 7 個未解決問題。
- **javascript/foundry-local-whisper.mjs：** 修正清理變數名稱 (`whisperModel` → `model`)

### 驗證
- Python：`foundry-local.py`、`foundry-local-rag.py`、`foundry-local-tool-calling.py` 執行成功並完成清理
- JavaScript：`foundry-local.mjs`、`foundry-local-rag.mjs`、`foundry-local-tool-calling.mjs` 執行成功並完成清理
- C#：`dotnet build` 成功，無警告、無錯誤（net9.0 目標）
- 所有 7 個 Python 檔案通過 `py_compile` 語法檢查
- 所有 7 個 JavaScript 檔案通過 `node --check` 語法驗證

---

## 2026-03-10 — 第 11 部分：工具調用、SDK API 擴充與模型涵蓋度

### 新增
- **第 11 部分：使用本地模型的工具調用** — 新實驗室指導 (`labs/part11-tool-calling.md`)，涵蓋工具結構、多輪流程、多工具調用、自訂工具、ChatClient 工具調用與 `tool_choice` 的 8 個練習
- **Python 範例：** `python/foundry-local-tool-calling.py` — 使用 OpenAI SDK 調用 `get_weather`/`get_population` 工具
- **JavaScript 範例：** `javascript/foundry-local-tool-calling.mjs` — 使用 SDK 原生 `ChatClient` (`model.createChatClient()`) 進行工具調用
- **C# 範例：** `csharp/ToolCalling.cs` — 使用 `ChatTool.CreateFunctionTool()` 透過 OpenAI C# SDK 進行工具調用
- **第 2 部分，練習 7：** 原生 `ChatClient` — JS 的 `model.createChatClient()` 與 C# 的 `model.GetChatClientAsync()`，作為 OpenAI SDK 替代方案
- **第 2 部分，練習 8：** 模型變體與硬體選擇 — `selectVariant()`、`variants`、7 種 NPU 變體表
- **第 2 部分，練習 9：** 模型升級與目錄更新 — `is_model_upgradeable()`、`upgrade_model()`、`updateModels()`
- **第 2 部分，練習 10：** 推理模型 — 使用 `phi-4-mini-reasoning` 並示範 `<think>` 標籤解析
- **第 3 部分，練習 4：** `createChatClient` 作為 OpenAI SDK 替代方案，包含串流回呼模式說明
- **AGENTS.md：** 新增工具調用、ChatClient 及推理模型的程式設計約定

### 變更
- **第 1 部分：** 擴充模型目錄 — 新增 phi-4-mini-reasoning、gpt-oss-20b、phi-4、qwen2.5-7b、qwen2.5-coder-7b、whisper-large-v3-turbo
- **第 2 部分：** 擴充 API 參考表 — 新增 `createChatClient`、`createAudioClient`、`removeFromCache`、`selectVariant`、`variants`、`isLoaded`、`stopWebService`、`is_model_upgradeable`、`upgrade_model`、`httpx_client`、`getModels`、`getCachedModels`、`getLoadedModels`、`updateModels`、`GetModelVariantAsync`、`UpdateModelsAsync`
- **第 2 部分：** 將練習 7–9 重新編號為 10–13 以納入新練習
- **第 3 部分：** 更新重要結論表以包含原生 ChatClient
- **README.md：** 新增第 11 部分節及程式碼範例表；新增學習目標 #11；更新專案結構樹
- **csharp/Program.cs：** 在 CLI 路由器加入 `toolcall` 案例並更新說明文字

---

## 2026-03-09 — SDK v0.9.0 更新，英式英語及驗證通過

### 變更
- **所有程式碼範例（Python、JavaScript、C#）：** 更新為 Foundry Local SDK v0.9.0 API — 修正 `await catalog.getModel()`（原缺少 `await`）、更新 `FoundryLocalManager` 初始化模式、修正端點發現
- **所有實驗室指導（第 1–10 部分）：** 改用英式英語拼字（colour、catalogue、optimised 等）
- **所有實驗室指導：** 更新 SDK 代碼範例以符合 v0.9.0 API 介面
- **所有實驗室指導：** 更新 API 參考表及練習程式碼區塊
- **JavaScript 重要修正：** 在 `catalog.getModel()` 補加缺失的 `await`，避免原回傳 `Promise` 非模型對象造成的靜默錯誤

### 驗證
- 所有 Python 範例成功連線並執行 Foundry Local 服務
- 所有 JavaScript 範例成功執行（Node.js 18+）
- C# 專案能成功編譯並執行於 .NET 9.0（從 net8.0 SDK 向前相容）
- 工作坊共計 29 個檔案已被修改與驗證

---

## 檔案索引

| 檔案 | 最後更新 | 說明 |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | 擴充模型目錄 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | 新增練習 7-10，擴充 API 表格 |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | 新增練習 4（ChatClient），更新重點總結 |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid 圖表 |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid 圖表，工作坊完成移至第 11 部分 |
| `labs/part11-tool-calling.md` | 2026-03-11 | 新實驗室，Mermaid 圖表，工作坊完成章節 |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | 新增：工具調用範例 |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | 新增：工具調用範例 |
| `csharp/ToolCalling.cs` | 2026-03-10 | 新增：工具調用範例 |
| `csharp/Program.cs` | 2026-03-10 | 新增 `toolcall` CLI 命令 |
| `README.md` | 2026-03-10 | 第 11 部分，專案結構 |
| `AGENTS.md` | 2026-03-10 | 工具調用 + ChatClient 慣例 |
| `KNOWN-ISSUES.md` | 2026-03-11 | 移除已解決的第 7 號問題，剩餘 6 個開放問題 |
| `csharp/csharp.csproj` | 2026-03-11 | 跨平台 TFM，WinML/基礎 SDK 條件引用 |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | 跨平台 TFM，自動偵測 RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | 跨平台 TFM，自動偵測 RID |
| `csharp/BasicChat.cs` | 2026-03-11 | 移除 NPU 的 try/catch 解決方法 |
| `csharp/SingleAgent.cs` | 2026-03-11 | 移除 NPU 的 try/catch 解決方法 |
| `csharp/MultiAgent.cs` | 2026-03-11 | 移除 NPU 的 try/catch 解決方法 |
| `csharp/RagPipeline.cs` | 2026-03-11 | 移除 NPU 的 try/catch 解決方法 |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | 移除 NPU 的 try/catch 解決方法 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | 跨平台 .csproj 範例 |
| `AGENTS.md` | 2026-03-11 | 更新 C# 套件和 TFM 詳細資訊 |
| `CHANGELOG.md` | 2026-03-11 | 本檔案 |