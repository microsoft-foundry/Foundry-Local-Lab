# 已知問題 — Foundry Local 工作坊

在搭建和測試此工作坊時，於運行 Windows 的 **Snapdragon X Elite (ARM64)** 裝置上，使用 Foundry Local SDK v0.9.0，CLI v0.8.117，以及 .NET SDK 10.0 遇到的問題。

> **最後驗證日期：** 2026-03-11

---

## 1. Snapdragon X Elite CPU 未被 ONNX Runtime 識別

**狀態：** 開放中  
**嚴重程度：** 警告（非阻塞）  
**組件：** ONNX Runtime / cpuinfo  
**重現：** 在 Snapdragon X Elite 硬件上每次啟動 Foundry Local 服務

每次 Foundry Local 服務啟動時，都會發出兩條警告：

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**影響：** 這些警告屬於視覺上的問題——推理仍然正常運作。但它們會在每次執行時出現，可能會令工作坊參與者感到混淆。需要更新 ONNX Runtime 的 cpuinfo 函式庫，以識別 Qualcomm Oryon CPU 核心。

**預期：** Snapdragon X Elite 應被識別為支援的 ARM64 CPU，且不產生錯誤等級訊息。

---

## 2. SingleAgent 在首次執行時發生 NullReferenceException

**狀態：** 開放中（間歇性）  
**嚴重程度：** 致命（崩潰）  
**組件：** Foundry Local C# SDK + Microsoft Agent Framework  
**重現：** 執行 `dotnet run agent` — 於模型載入後立即崩潰

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**上下文:** 第 37 行調用 `model.IsCachedAsync(default)`。崩潰發生於於新執行的代理程式首次運行，在執行過 `foundry service stop` 後。之後使用相同代碼的執行則成功。

**影響：** 間歇性發生 — 表示 SDK 的服務初始化或目錄查詢中存在競態條件。`GetModelAsync()` 呼叫可能在服務完全準備好前返回。

**預期：** `GetModelAsync()` 應在服務準備好前阻塞，或在服務尚未完成初始化時返回明確錯誤訊息。

---

## 3. C# SDK 需要明確設定 RuntimeIdentifier

**狀態：** 開放 — 已在 [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) 追蹤  
**嚴重程度：** 文件不足  
**組件：** `Microsoft.AI.Foundry.Local` NuGet 套件  
**重現：** 建立未於 `.csproj` 中指定 `<RuntimeIdentifier>` 的 .NET 8+ 專案

構建失敗，顯示：

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**根本原因：** 需要指定 RID 是預期行為——SDK 包含本機原生二進制檔（對 `Microsoft.AI.Foundry.Local.Core` 和 ONNX Runtime 的 P/Invoke），因此 .NET 需要知道解析哪個平台特定的庫。

此情況已於 MS Learn 文檔中記錄（[如何使用本機聊天完成](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)），執行指令示例如下：

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
但用戶每次都需記得 `-r` 旗標，容易被遺忘。

**解決方法：** 將自動檢測的回退添加至 `.csproj`，以便 `dotnet run` 無需任何參數也能運行：

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` 是內建的 MSBuild 屬性，會自動解析主機的 RID。SDK 自己的測試專案已經使用此模式。提供明確的 `-r` 旗標時仍然有效。

> **注意：** 工作坊的 `.csproj` 模板包含此回退，確保 `dotnet run` 在任意平台能開箱即用。

**預期：** MS Learn 文檔中的 `.csproj` 範本應包含此自動檢測模式，讓使用者無需每次都記得 `-r` 旗標。

---

## 4. JavaScript Whisper — 音頻轉錄回傳空或二進制輸出

**狀態：** 開放（迴歸 — 比初報時更嚴重）  
**嚴重程度：** 主要問題  
**組件：** JavaScript Whisper 實作 (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**重現：** 執行 `node foundry-local-whisper.mjs` — 所有音訊檔案皆回傳空或二進制輸出，非文字轉錄

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
最初僅第 5 個音訊檔案回傳空值；自 v0.9.x 版本起，所有 5 個檔案回傳單一字元 (`\ufffd`)，而非轉錄文字。使用 OpenAI SDK 的 Python Whisper 實作對同一批檔案能正確轉錄。

**預期：** `createAudioClient()` 應回傳與 Python/C# 實作相同的文字轉錄結果。

---

## 5. C# SDK 僅發行 net8.0 — 無官方 .NET 9 或 .NET 10 目標框架

**狀態：** 開放  
**嚴重程度：** 文件不足  
**組件：** `Microsoft.AI.Foundry.Local` NuGet 套件 v0.9.0  
**安裝指令：** `dotnet add package Microsoft.AI.Foundry.Local`

該 NuGet 套件僅包含單一目標框架：

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
未包含 `net9.0` 或 `net10.0` TFM。相比之下，同伴套件 `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) 支援 `net8.0`、`net9.0`、`net10.0`、`net472`、及 `netstandard2.0`。

### 相容性測試

| 目標框架 | 建置 | 執行 | 備註 |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | 官方支援 |
| net9.0 | ✅ | ✅ | 透過向前相容建置 — 工作坊範例使用 |
| net10.0 | ✅ | ✅ | 透過 .NET 10.0.3 运行时的向前相容建置及運行 |

net8.0 程式碼可透過 .NET 的向前相容機制在較新執行時加載，因此構建成功。但此情況未被 SDK 團隊記錄和測試。

### 為什麼範例使用 net9.0 為目標框架

1. **.NET 9 是最新穩定版本** — 大部分工作坊參與者會安裝
2. <strong>向前相容可行</strong> — NuGet 套件內的 net8.0 程式碼可在 .NET 9 執行時環境正常運作
3. **.NET 10（預覽版/RC）** 對於應適用於所有用戶的工作坊來說過於新穎

**預期：** 未來 SDK 發行應考慮繼 net8.0 之外新增 net9.0 和 net10.0 TFM，以匹配 `Microsoft.Agents.AI.OpenAI` 使用模式並提供經過驗證的新版執行時支持。

---

## 6. JavaScript ChatClient 串流使用回呼函式，非 async 迭代器

**狀態：** 開放  
**嚴重程度：** 文件不足  
**組件：** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

由 `model.createChatClient()` 回傳的 `ChatClient` 提供了 `completeStreamingChat()` 方法，但採用的是 <strong>回呼函式模式</strong>，而非回傳 async iterable：

```javascript
// ❌ 呢個唔啱用 — 會拋出「stream 不是非同步可疊代嘅」
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ 正確嘅用法 — 傳入一個回調函數
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**影響：** 熟悉 OpenAI SDK async iteration 模型 (`for await`) 的開發者會遇到混淆錯誤。回呼必須是一個有效函式，否則 SDK 將拋出「Callback must be a valid function」錯誤。

**預期：** SDK 參考文件應說明此回呼模式。或者可提供 async iterable 支持，以保持與 OpenAI SDK 一致。

---

## 環境詳情

| 組件 | 版本 |
|-----------|---------|
| 作業系統 | Windows 11 ARM64 |
| 硬件 | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |