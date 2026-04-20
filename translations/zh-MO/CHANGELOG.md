# 變更記錄 — Foundry 本地工作坊

本工作坊的所有重要變更皆記錄於下方。

---

## 2026-03-11 — 第 12 與 13 部分、網頁介面、Whisper 重寫、WinML/QNN 修正及驗證

### 新增
- **第 12 部分：為 Zava 創意作家建立網頁介面** — 新實驗指南（`labs/part12-zava-ui.md`），涵蓋串流 NDJSON、瀏覽器 `ReadableStream`、即時代理狀態徽章及即時文章文字串流
- **第 13 部分：工作坊完成** — 新總結實驗（`labs/part13-workshop-complete.md`），包含所有 12 部分回顧、進階想法與資源連結
- **Zava UI 前端：** `zava-creative-writer-local/ui/index.html`、`style.css`、`app.js` — 三個後端共用的純淨 HTML/CSS/JS 瀏覽器介面
- **JavaScript HTTP 伺服器：** `zava-creative-writer-local/src/javascript/server.mjs` — 新增 Express 風格 HTTP 伺服器，包裝協調器以供瀏覽器存取
- **C# ASP.NET Core 後端：** `zava-creative-writer-local/src/csharp-web/Program.cs` 與 `ZavaCreativeWriterWeb.csproj` — 新增極簡 API 專案，提供 UI 服務及串流 NDJSON
- **語音樣本產生器：** `samples/audio/generate_samples.py` — 離線 TTS 腳本，使用 `pyttsx3` 產製 Zava 主題 WAV 檔，用於第 9 部分
- **語音樣本：** `samples/audio/zava-full-project-walkthrough.wav` — 新增較長語音樣本供轉錄測試
- **驗證腳本：** `validate-npu-workaround.ps1` — 自動化 PowerShell 腳本，用於驗證所有 C# 範例中 NPU/QNN 變通方法
- **Mermaid 圖表 SVG 檔案：** `images/part12-architecture.svg`、`part12-message-types.svg`、`part12-streaming-sequence.svg`
- **WinML 跨平台支援：** 三個 C# `.csproj` 檔案（`csharp/csharp.csproj`、`zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`、`zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`）現使用條件目標框架 (TFM) 及互斥套件參考以達跨平台。在 Windows 上：`net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML`（超集含 QNN EP 插件）。非 Windows 平台上：`net9.0` TFM + `Microsoft.AI.Foundry.Local`（基礎 SDK）。Zava 專案中的硬編碼 `win-arm64` RID 改用自動檢測。透過轉移相依性變通方法排除 `Microsoft.ML.OnnxRuntime.Gpu.Linux` 中損壞的 win-arm64 本機資產參考。先前所有 7 個 C# 檔案中的 try/catch NPU 變通已移除。

### 變更
- **第 9 部分（Whisper）：** 大幅重寫 — JavaScript 現使用 SDK 內建的 `AudioClient`（`model.createAudioClient()`），取代手動 ONNX Runtime 推理；更新架構說明、比較表及管線圖，反映 JS/C# `AudioClient` 與 Python ONNX Runtime 方法差異
- **第 11 部分：** 更新導覽連結（改指向第 12 部分）；新增渲染 SVG 圖表呈現工具呼叫流程與序列
- **第 10 部分：** 更新導覽改經由第 12 部分，取代工作坊結束頁面
- **Python Whisper (`foundry-local-whisper.py`)：** 擴充更多音訊樣本與強化錯誤處理
- **JavaScript Whisper (`foundry-local-whisper.mjs`)：** 重寫為使用 `model.createAudioClient()` 搭配 `audioClient.transcribe()`，取代手動 ONNX Runtime Session
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`)：** 更新以同時提供靜態 UI 檔案與 API
- **Zava C# 主控台 (`zava-creative-writer-local/src/csharp/Program.cs`)：** 移除 NPU 變通（現由 WinML 套件負責）
- **README.md：** 新增第 12 部分章節及程式碼範例表與後端新增項目；新增第 13 部分章節；更新學習目標與專案架構
- **KNOWN-ISSUES.md：** 移除已解決的第 7 號問題（C# SDK NPU 模型變體，現由 WinML 套件管理）；重新編號剩餘問題為 #1–#6；更新環境細節為 .NET SDK 10.0.104
- **AGENTS.md：** 更新專案結構樹狀圖，加入新的 `zava-creative-writer-local` 項目（`ui/`、`csharp-web/`、`server.mjs`）；更新 C# 主要套件與條件 TFM 細節
- **labs/part2-foundry-local-sdk.md：** 更新 `.csproj` 範例以顯示完整跨平台模式，包含條件 TFM、互斥套件參考及說明註解

### 驗證
- 所有 3 個 C# 專案（`csharp`、`ZavaCreativeWriter`、`ZavaCreativeWriterWeb`）於 Windows ARM64 上成功建置
- 聊天範例（`dotnet run chat`）：模型以 WinML/QNN 載入為 `phi-3.5-mini-instruct-qnn-npu:1` — NPU 變體直接載入無需 CPU 回退
- 代理範例（`dotnet run agent`）：能全流程執行含多輪對話，退出碼 0
- Foundry Local CLI v0.8.117 及 SDK v0.9.0 於 .NET SDK 9.0.312 上通過測試

---

## 2026-03-11 — 程式修正、模型清理、Mermaid 圖表及驗證

### 修正
- **所有 21 個程式範例（7 Python、7 JavaScript、7 C#）：** 加入 `model.unload()` / `unload_model()` / `model.UnloadAsync()` 清理邏輯於結束時，解決 OGA 記憶體洩漏警告（已知問題 #4）
- **csharp/WhisperTranscription.cs：** 將不穩定的 `AppContext.BaseDirectory` 相對路徑替換為 `FindSamplesDirectory()`，透過向上尋找目錄穩定定位 `samples/audio`（已知問題 #7）
- **csharp/csharp.csproj：** 將硬編碼的 `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` 替換為自動檢測降級方案，使用 `$(NETCoreSdkRuntimeIdentifier)`，令 `dotnet run` 可於任意平台執行無需 `-r` 參數（參考 [Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)）

### 變更
- **第 8 部分：** 將基於評估驅動的迭代循環由 ASCII 框圖轉換為渲染 SVG 圖像
- **第 10 部分：** 將編譯管線圖由 ASCII 箭頭轉換為渲染 SVG 圖像
- **第 11 部分：** 將工具呼叫流程與序列圖由 ASCII 轉為渲染 SVG
- **第 10 部分：** 將「工作坊完成！」章節移至第 11 部分（最終實驗），以「後續步驟」鏈結取代
- **KNOWN-ISSUES.md：** 於 CLI v0.8.117 上全面重新驗證所有問題。移除已解決項目：OGA 記憶體洩漏（已新增清理）、Whisper 路徑（FindSamplesDirectory）、HTTP 500 持續推理錯誤（不可重現，[＃494](https://github.com/microsoft/Foundry-Local/issues/494)）、tool_choice 限制（現支援 `"required"` 與 qwen2.5-0.5b 特定函式目標）。更新 JS Whisper 問題 — 所有檔案均回傳空白/二進位輸出（v0.9.x 退化，嚴重性調升為主要）。更新 #4 C# RID 自動檢測變通與 [＃497](https://github.com/microsoft/Foundry-Local/issues/497) 聯結。剩餘 7 個未解決問題。
- **javascript/foundry-local-whisper.mjs：** 修正清理變數名稱（`whisperModel` 改為 `model`）

### 驗證
- Python：`foundry-local.py`、`foundry-local-rag.py`、`foundry-local-tool-calling.py` 執行成功並含清理
- JavaScript：`foundry-local.mjs`、`foundry-local-rag.mjs`、`foundry-local-tool-calling.mjs` 執行成功並含清理
- C#：`dotnet build` 無警告、無錯誤成功（目標 net9.0）
- 所有 7 個 Python 檔案通過 `py_compile` 語法檢查
- 所有 7 個 JavaScript 檔案通過 `node --check` 語法驗證

---

## 2026-03-10 — 第 11 部分：工具呼叫、SDK API 擴充及模型涵蓋範圍

### 新增
- **第 11 部分：使用本地模型的工具呼叫** — 新實驗指南（`labs/part11-tool-calling.md`），涵蓋 8 個練習，包含工具結構、多輪流程、多重工具呼叫、自訂工具、ChatClient 工具呼叫及 `tool_choice`
- **Python 範例：** `python/foundry-local-tool-calling.py` — 使用 OpenAI SDK 呼叫 `get_weather`/`get_population` 工具
- **JavaScript 範例：** `javascript/foundry-local-tool-calling.mjs` — 使用 SDK 原生 `ChatClient`（`model.createChatClient()`）進行工具呼叫
- **C# 範例：** `csharp/ToolCalling.cs` — 使用 OpenAI C# SDK 的 `ChatTool.CreateFunctionTool()` 進行工具呼叫
- **第 2 部分，練習 7：** 原生 `ChatClient` — `model.createChatClient()`（JS）與 `model.GetChatClientAsync()`（C#）作為 OpenAI SDK 之替代方案
- **第 2 部分，練習 8：** 模型變體與硬體選擇 — `selectVariant()`、`variants`、NPU 變體表（7 種模型）
- **第 2 部分，練習 9：** 模型升級與型錄更新 — `is_model_upgradeable()`、`upgrade_model()`、`updateModels()`
- **第 2 部分，練習 10：** 推理模型 — 含 `<think>` 標籤解析範例的 `phi-4-mini-reasoning`
- **第 3 部分，練習 4：** `createChatClient` 作為 OpenAI SDK 替代方案，含串流回呼模式說明
- **AGENTS.md：** 新增工具呼叫、ChatClient 及推理模型程式碼慣例

### 變更
- **第 1 部分：** 擴充模型型錄 — 新增 phi-4-mini-reasoning、gpt-oss-20b、phi-4、qwen2.5-7b、qwen2.5-coder-7b、whisper-large-v3-turbo
- **第 2 部分：** 擴充 API 參考表 — 新增 `createChatClient`、`createAudioClient`、`removeFromCache`、`selectVariant`、`variants`、`isLoaded`、`stopWebService`、`is_model_upgradeable`、`upgrade_model`、`httpx_client`、`getModels`、`getCachedModels`、`getLoadedModels`、`updateModels`、`GetModelVariantAsync`、`UpdateModelsAsync`
- **第 2 部分：** 練習 7-9 重新編號為 10-13，以配合新增練習
- **第 3 部分：** 更新重點摘要表，加入原生 ChatClient
- **README.md：** 新增第 11 部分章節及程式碼範例表；新增學習目標 #11；更新專案結構樹狀圖
- **csharp/Program.cs：** CLI 路由器加入 `toolcall` 案例，更新說明文字

---

## 2026-03-09 — SDK v0.9.0 更新、英式英語及驗證通過

### 變更
- **所有程式範例（Python、JavaScript、C#）：** 升級至 Foundry Local SDK v0.9.0 API — 修正 `await catalog.getModel()`（先前缺少 `await`）、更新 `FoundryLocalManager` 初始化模式、修正端點發現邏輯
- **所有實驗指南（第 1-10 部分）：** 語言統一為英式英語（colour、catalogue、optimised 等）
- **所有實驗指南：** 更新 SDK 程式碼範例，符合 v0.9.0 API 介面
- **所有實驗指南：** 更新 API 參考表與練習程式碼區塊
- **JavaScript 重要修正：** 補上遺漏的 `await` 於 `catalog.getModel()` 呼叫 — 先前回傳 `Promise` 而非直接 `Model` 物件，導致下游靜默失敗

### 驗證
- 所有 Python 範例均成功連線與執行 Foundry Local 服務
- 所有 JavaScript 範例皆成功執行（Node.js 18+）
- C# 專案成功建置並執行於 .NET 9.0（向下兼容 net8.0 SDK 組件）
- 工作坊共 29 個檔案已修改與驗證

---

## 檔案索引

| 檔案 | 最後更新時間 | 說明 |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | 擴充模型型錄 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | 新增練習 7-10、擴充 API 表格 |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | 新增練習 4（ChatClient）、更新重點摘要 |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 更新 + 英式英語 |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 更新 + 英式英語 |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid 圖表 |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + 英式英語 |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid 圖表，將工作坊完成移至第11部分 |
| `labs/part11-tool-calling.md` | 2026-03-11 | 新實驗室，Mermaid 圖表，工作坊完成部分 |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | 新增：工具呼叫示例 |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | 新增：工具呼叫示例 |
| `csharp/ToolCalling.cs` | 2026-03-10 | 新增：工具呼叫示例 |
| `csharp/Program.cs` | 2026-03-10 | 新增 `toolcall` CLI 指令 |
| `README.md` | 2026-03-10 | 第11部分，專案結構 |
| `AGENTS.md` | 2026-03-10 | 工具呼叫 + ChatClient 慣例 |
| `KNOWN-ISSUES.md` | 2026-03-11 | 移除已解決的問題＃7，仍有6個未解決問題 |
| `csharp/csharp.csproj` | 2026-03-11 | 跨平台 TFM，WinML/基礎 SDK 條件參考 |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | 跨平台 TFM，自動偵測 RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | 跨平台 TFM，自動偵測 RID |
| `csharp/BasicChat.cs` | 2026-03-11 | 移除 NPU try/catch 解決方法 |
| `csharp/SingleAgent.cs` | 2026-03-11 | 移除 NPU try/catch 解決方法 |
| `csharp/MultiAgent.cs` | 2026-03-11 | 移除 NPU try/catch 解決方法 |
| `csharp/RagPipeline.cs` | 2026-03-11 | 移除 NPU try/catch 解決方法 |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | 移除 NPU try/catch 解決方法 |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | 跨平台 .csproj 範例 |
| `AGENTS.md` | 2026-03-11 | 更新 C# 套件及 TFM 詳情 |
| `CHANGELOG.md` | 2026-03-11 | 本檔案 |