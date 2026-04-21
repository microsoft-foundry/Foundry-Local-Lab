# 已知問題 — Foundry Local 工作坊

在配備 SnapDragon X Elite（ARM64）裝置並執行 Windows，使用 Foundry Local SDK v0.9.0、CLI v0.8.117 及 .NET SDK 10.0 建構與測試此工作坊時遇到的問題。

> **最後驗證時間：** 2026-03-11

---

## 1. Snapdragon X Elite CPU 無法被 ONNX Runtime 辨識

**狀態：** 開放中  
**嚴重性：** 警告（非阻斷性）  
**組件：** ONNX Runtime / cpuinfo  
**重現方式：** 在 Snapdragon X Elite 硬件上每次啟動 Foundry Local 服務均會發生

每次 Foundry Local 服務啟動時會產生兩個警告：

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**影響：** 這些警告為表面警示 — 推論功能依然正常。但每次執行時都會出現，可能會讓工作坊參與者感到混淆。ONNX Runtime cpuinfo 函式庫需要更新以辨識 Qualcomm Oryon CPU 核心。

**期望：** Snapdragon X Elite 應被辨識為支援的 ARM64 CPU，且不會拋出錯誤級訊息。

---

## 2. SingleAgent 首次執行 NullReferenceException

**狀態：** 開放中（偶發）  
**嚴重性：** 致命（崩潰）  
**組件：** Foundry Local C# SDK + Microsoft Agent Framework  
**重現方式：** 執行 `dotnet run agent` — 在模型載入後立即崩潰

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**背景：** 第 37 行執行 `model.IsCachedAsync(default)`。崩潰發生在代理器首次執行（新執行 `foundry service stop` 後）。同一程式碼重複執行後即成功。

**影響：** 偶發問題 — 疑似 SDK 的服務初始化或目錄查詢有競態條件。`GetModelAsync()` 呼叫可能在服務尚未完全準備好時就返回。

**期望：** `GetModelAsync()` 應該要麼阻塞直到服務準備完成，要麼返回明確的錯誤訊息以表示服務尚未完成初始化。

---

## 3. C# SDK 需要明確指定 RuntimeIdentifier

**狀態：** 開放中 — 已在 [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497) 跟蹤  
**嚴重性：** 文件缺失  
**組件：** `Microsoft.AI.Foundry.Local` NuGet 套件  
**重現方式：** 建立 .NET 8+ 專案但 `.csproj` 中未包含 `<RuntimeIdentifier>`

編譯失敗，錯誤訊息為：

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**根本原因：** RID（Runtime Identifier）需求是預期的 — SDK 內含本機二進位檔（P/Invoke 呼叫 `Microsoft.AI.Foundry.Local.Core` 及 ONNX Runtime），因此 .NET 必須辨識並載入相應的平台特定函式庫。

MS Learn 文件中有記錄（[如何使用原生聊天完成](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)），執行指令部分列示：

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
不過使用者每次執行都必須自己記得 `-r` 參數，且很容易忘記。

**解決辦法：** 在 `.csproj` 加入自動偵測後備機制，使 `dotnet run` 不需要任何旗標即可執行：

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` 是 MSBuild 內建屬性，可自動解析目前主機的 RID。SDK 自己的測試專案皆已採用此模式。當提供明確的 `-r` 參數時仍會被接受。

> **注意：** 工作坊的 `.csproj` 已包含此後備方案，讓 `dotnet run` 在任意平台上皆可開箱即用。

**期望：** MS Learn 文檔中的 `.csproj` 範本應包含自動偵測模式，讓使用者不需要每次記住 `-r` 旗標。

---

## 4. JavaScript Whisper — 音訊轉錄回傳空或二進位輸出

**狀態：** 開放中（回歸問題 — 自初次報告以來惡化）  
**嚴重性：** 重大  
**組件：** JavaScript Whisper 實作（`foundry-local-whisper.mjs`）/ `model.createAudioClient()`  
**重現方式：** 執行 `node foundry-local-whisper.mjs` — 所有音訊檔案皆回傳為空或二進位輸出，未產生文字轉錄結果

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
最初只有第 5 個音訊檔案回傳空訊息；自 v0.9.x 起，全部 5 個檔案皆回傳單一位元組（`\ufffd`），未產生文字轉錄。使用 Python Whisper 透過 OpenAI SDK可正確轉錄相同檔案。

**期望：** `createAudioClient()` 應回傳與 Python/C# 實作一致的文字轉錄結果。

---

## 5. C# SDK 只提供 net8.0 版本 — 無官方 .NET 9 或 .NET 10 目標框架

**狀態：** 開放中  
**嚴重性：** 文件缺失  
**組件：** `Microsoft.AI.Foundry.Local` NuGet 套件 v0.9.0  
**安裝指令：** `dotnet add package Microsoft.AI.Foundry.Local`

NuGet 套件僅提供單一目標框架：

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
未包含 `net9.0` 或 `net10.0` TFM。相比之下，搭配套件 `Microsoft.Agents.AI.OpenAI`（v1.0.0-rc3）提供了 `net8.0`、`net9.0`、`net10.0`、`net472` 與 `netstandard2.0`。

### 相容性測試

| 目標框架 | 建構 | 執行 | 備註 |
|----------|------|------|-------|
| net8.0   | ✅   | ✅   | 官方支援 |
| net9.0   | ✅   | ✅   | 透過前向相容建置 — 使用於工作坊範例 |
| net10.0  | ✅   | ✅   | 使用 .NET 10.0.3 執行時透過前向相容建置並正確執行 |

net8.0 組件可透過 .NET 的前向相容機制在較新執行環境中載入，因此建置成功，但此情況尚未被 SDK 團隊正式文件化或測試。

### 為何範例採用 net9.0

1. **.NET 9 是最新穩定版本** — 大部分工作坊參與者皆已安裝  
2. <strong>前向相容有效</strong> — NuGet 套件中的 net8.0 組件可在 .NET 9 執行環境運行良好  
3. **.NET 10（預覽版/釋出候選版）** 太新，不適合用於需通用性工作坊

**期望：** 未來 SDK 版本可考慮於發佈時同時包含 `net9.0` 與 `net10.0` TFM，以符合 `Microsoft.Agents.AI.OpenAI` 的發佈策略，並提供更完整的新版執行環境支援與驗證。

---

## 6. JavaScript ChatClient Streaming 使用回呼函式而非異步迭代器

**狀態：** 開放中  
**嚴重性：** 文件缺失  
**組件：** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

由 `model.createChatClient()` 取得的 `ChatClient` 物件的 `completeStreamingChat()` 方法採用 <strong>回呼函式模式</strong>，而非回傳 async iterable：

```javascript
// ❌ 呢個唔得 — 拋出「stream 係唔係非同步可迭代嘅」
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ 正確嘅模式 — 傳入一個回調函數
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**影響：** 熟悉 OpenAI SDK 異步迭代（`for await`）模式的開發者會遇到困惑錯誤。回呼函式必須是有效的函式，否則 SDK 會丟出 "Callback must be a valid function." 的異常。

**期望：** SDK 參考文件應記錄此回呼函式模式。或者，為與 OpenAI SDK 一致，可考慮同時支援 async iterable 模式。

---

## 環境細節

| 組件 | 版本 |
|-------|---------|
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