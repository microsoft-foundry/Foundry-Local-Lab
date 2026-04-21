# 已知問題 — Foundry Local 工作坊

在運行 Windows 的 **Snapdragon X Elite (ARM64)** 裝置上使用 Foundry Local SDK v0.9.0、CLI v0.8.117 和 .NET SDK 10.0 建構及測試此工作坊時遇到的問題。

> **最後驗證時間：** 2026-03-11

---

## 1. Snapdragon X Elite CPU 未被 ONNX Runtime 辨識

**狀態：** 開放中  
**嚴重性：** 警告（非阻斷）  
**組件：** ONNX Runtime / cpuinfo  
**重現方式：** 每次在 Snapdragon X Elite 硬體上啟動 Foundry Local 服務時

每次啟動 Foundry Local 服務時，會發出兩條警告：

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**影響：** 這些警告只是表面現象 — 推論功能正常。然而每次執行都會出現，可能會讓工作坊參與者感到困惑。ONNX Runtime 的 cpuinfo 函式庫需要更新，以辨識 Qualcomm Oryon CPU 核心。

**預期：** Snapdragon X Elite 應該被辨識為支援的 ARM64 CPU，並且不會發出錯誤級訊息。

---

## 2. SingleAgent 首次執行時發生 NullReferenceException

**狀態：** 開放中（間歇性）  
**嚴重性：** 致命（崩潰）  
**組件：** Foundry Local C# SDK + Microsoft Agent Framework  
**重現方式：** 執行 `dotnet run agent` — 載入模型後立即崩潰

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**背景：** 第 37 行呼叫 `model.IsCachedAsync(default)`。崩潰發生在剛執行 `foundry service stop` 後的代理程式首次運行。相同程式碼的後續運行皆成功。

**影響：** 間歇性問題 — 表示 SDK 的服務初始化或目錄查詢中可能存在競態情況。`GetModelAsync()` 可能在服務完全就緒前返回。

**預期：** `GetModelAsync()` 要麼阻塞直到服務就緒，要麼如果服務尚未完成初始化，回傳明確的錯誤訊息。

---

## 3. C# SDK 需要明確指定 RuntimeIdentifier

**狀態：** 開放中 — 已在 [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) 跟蹤  
**嚴重性：** 文件缺失  
**組件：** `Microsoft.AI.Foundry.Local` NuGet 套件  
**重現方式：** 建立不含 `<RuntimeIdentifier>` 的 .NET 8+ 專案 `.csproj`

建置失敗訊息：

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**根本原因：** 需要指定 RID 是預期行為 — SDK 包含原生二進位檔（P/Invoke 呼叫 `Microsoft.AI.Foundry.Local.Core` 和 ONNX Runtime），因此 .NET 需要知道使用哪個平台相關的程式庫。

此點在 MS Learn 文件 ([如何使用原生聊天完成](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)) 有記載，執行說明如下：

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
不過，用戶必須每次都記得加上 `-r` 參數，很容易忘記。

**解決方法：** 在 `.csproj` 中加入自動偵測替代設定，使得 `dotnet run` 不須任何旗標便能運作：

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` 是 MSBuild 內建屬性，可自動解析宿主機的 RID。SDK 自身的測試專案已使用此模式。若提供明確的 `-r` 旗標，依然會被尊重。

> **注意：** 工作坊的 `.csproj` 已包含此替代設定，使 `dotnet run` 可在任何平台上直接使用。

**預期：** MS Learn 文件中的 `.csproj` 模板應該加入此自動偵測模式，免得使用者每次都要記得 `-r` 旗標。

---

## 4. JavaScript Whisper — 音訊轉錄回傳為空或二進位輸出

**狀態：** 開放中（回歸問題 — 比首報更嚴重）  
**嚴重性：** 嚴重  
**組件：** JavaScript Whisper 實作 (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**重現方式：** 執行 `node foundry-local-whisper.mjs` — 所有音訊檔案回傳空或二進位內容，而非文字轉錄

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
原本只第五個音訊檔回傳為空；在 v0.9.x 版本中，所有 5 個音訊檔皆回傳單一字節 (`\ufffd`)，而非文字轉錄。Python Whisper 使用 OpenAI SDK 的實作對相同檔案能正常轉錄。

**預期：** `createAudioClient()` 應回傳與 Python/C# 實作相符的文字轉錄結果。

---

## 5. C# SDK 僅提供 net8.0 目標 — 無官方 .NET 9 或 .NET 10 目標框架

**狀態：** 開放中  
**嚴重性：** 文件缺失  
**組件：** `Microsoft.AI.Foundry.Local` NuGet 套件 v0.9.0  
**安裝命令：** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet 套件僅包含單一目標框架：

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
無 `net9.0` 或 `net10.0` TFM。相比之下，同伴套件 `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) 支援 `net8.0`、`net9.0`、`net10.0`、`net472` 和 `netstandard2.0`。

### 相容性測試

| 目標框架      | 建置 | 執行 | 備註                    |
|---------------|------|------|-------------------------|
| net8.0        | ✅   | ✅   | 官方支援                |
| net9.0        | ✅   | ✅   | 使用向前相容性建置 — 用於工作坊範例 |
| net10.0       | ✅   | ✅   | 使用向前相容性建置並於 .NET 10.0.3 執行 |

net8.0 組件會透過 .NET 的向前相容機制載入新版本執行階段，因此建置成功。但此行為未經 SDK 團隊文件記錄和測試。

### 範例使用 net9.0 的理由

1. **.NET 9 是最新穩定版** — 多數工作坊參與者會安裝此版本  
2. <strong>向前相容運作正常</strong> — NuGet 套件中的 net8.0 組件在 .NET 9 執行階段正常運行  
3. **.NET 10 (預覽版 / RC)** 太新，不適合用於普遍可用的工作坊

**預期：** 未來 SDK 版本應考慮除 net8.0 外，增加 net9.0 和 net10.0 目標框架，以符合 `Microsoft.Agents.AI.OpenAI` 的模式，並提供對新版本執行階段的驗證支援。

---

## 6. JavaScript ChatClient 串流使用回調而非非同步迭代器

**狀態：** 開放中  
**嚴重性：** 文件缺失  
**組件：** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

由 `model.createChatClient()` 返回的 `ChatClient` 提供 `completeStreamingChat()` 方法，但此方法採用<strong>回調模式</strong>，未回傳非同步可疊代物件：

```javascript
// ❌ 這行不 WORK — 拋出 "stream 不是非同步可迭代物件" 錯誤
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ 正確模式 — 傳入回呼函式
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**影響：** 熟悉 OpenAI SDK 非同步迭代 (`for await`) 模式的開發者會遇到混淆錯誤。回調必須是有效函式，否則 SDK 會拋出「Callback must be a valid function」錯誤。

**預期：** SDK 參考文件應說明回調模式。或者支援非同步疊代模式，以與 OpenAI SDK 保持一致。

---

## 環境詳情

| 組件                     | 版本              |
|--------------------------|-------------------|
| 作業系統                 | Windows 11 ARM64  |
| 硬體                     | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI         | 0.8.117           |
| Foundry Local SDK (C#)    | 0.9.0             |
| Microsoft.Agents.AI.OpenAI| 1.0.0-rc3         |
| OpenAI C# SDK            | 2.9.0             |
| .NET SDK                 | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python)| 0.5.x             |
| foundry-local-sdk (JS)    | 0.9.x             |
| Node.js                  | 18+               |
| ONNX Runtime             | 1.18+             |