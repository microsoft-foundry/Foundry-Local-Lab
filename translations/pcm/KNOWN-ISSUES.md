# Known Issues — Foundry Local Workshop

Issues wey dem face wen dem dey build and test dis workshop on top **Snapdragon X Elite (ARM64)** device wey dey run Windows, wit Foundry Local SDK v0.9.0, CLI v0.8.117, and .NET SDK 10.0.

> **Last validated:** 2026-03-11

---

## 1. Snapdragon X Elite CPU No Dey Recognise by ONNX Runtime

**Status:** Open  
**Severity:** Warning (no dey block)  
**Component:** ONNX Runtime / cpuinfo  
**Reproduction:** Every Foundry Local service start for Snapdragon X Elite hardware

Every time wey Foundry Local service start, e go show two warnings:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```
  
**Impact:** The warnings na only for show — inference dey work well. But, dem dey come out every run and fit scatter workshop participants mind. The ONNX Runtime cpuinfo library need update make e fit recognise Qualcomm Oryon CPU cores.

**Expected:** Snapdragon X Elite suppose dey recognise as correct ARM64 CPU without show error-level message.

---

## 2. SingleAgent NullReferenceException on First Run

**Status:** Open (sometimes)  
**Severity:** Critical (crash)  
**Component:** Foundry Local C# SDK + Microsoft Agent Framework  
**Reproduction:** Run `dotnet run agent` — e crash sharp sharp after model load

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```
  
**Context:** Line 37 dey call `model.IsCachedAsync(default)`. The crash happen for the first time wey the agent run after fresh `foundry service stop`. Later runs with the same code dey succeed.

**Impact:** Na sometimes e dey happen — shows say race condition dey inside SDK service initialization or catalog query. The `GetModelAsync()` fit return before service full ready.

**Expected:** `GetModelAsync()` suppose block until service ready or give clear error message if service never finish initialize.

---

## 3. C# SDK Need Explicit RuntimeIdentifier

**Status:** Open — dem dey track am for [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)  
**Severity:** Documentation wahala  
**Component:** `Microsoft.AI.Foundry.Local` NuGet package  
**Reproduction:** Create .NET 8+ project without `<RuntimeIdentifier>` for `.csproj`

Build go fail with:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```
  
**Root cause:** RID requirement na normal — the SDK get native binaries (P/Invoke inside `Microsoft.AI.Foundry.Local.Core` and ONNX Runtime), so .NET need sabi which platform-specific library to run.

This one dey documented for MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), the run instructions show:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```
  
But people must remember `-r` flag every time, wey e easy to forget.

**Workaround:** Add automatic detect fallback to your `.csproj` so `dotnet run` go run without flags:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```
  
`$(NETCoreSdkRuntimeIdentifier)` na built-in MSBuild property wey go automatically find host machine's RID. The SDK own test projects already dey use this style. Explicit `-r` flags still dey work if you put am.

> **Note:** The workshop `.csproj` get dis fallback so `dotnet run` fit work from the start for any platform.

**Expected:** The `.csproj` template for MS Learn docs suppose get dis auto-detect pattern so people no go forget the `-r` flag.

---

## 4. JavaScript Whisper — Audio Transcription Dey Return Empty/Binary Output

**Status:** Open (regression — e don spoil more since first report)  
**Severity:** Major  
**Component:** JavaScript Whisper implementation (`foundry-local-whisper.mjs`) / `model.createAudioClient()`  
**Reproduction:** Run `node foundry-local-whisper.mjs` — all audio files go return empty or binary output instead of text transcription

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```
  
Before before only the 5th audio file dey return empty; now for v0.9.x, all 5 files dey return single byte (`\ufffd`) instead of text wey dem transcribe. The Python Whisper implementation using OpenAI SDK dey transcribe the same files well well.

**Expected:** `createAudioClient()` suppose return text transcription wey match Python/C# implementations.

---

## 5. C# SDK Only Get net8.0 — No Official .NET 9 or .NET 10 Target

**Status:** Open  
**Severity:** Documentation wahala  
**Component:** `Microsoft.AI.Foundry.Local` NuGet package v0.9.0  
**Install command:** `dotnet add package Microsoft.AI.Foundry.Local`

The NuGet package only get one target framework:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```
  
No `net9.0` or `net10.0` TFM inside. But, the companion package `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) get `net8.0`, `net9.0`, `net10.0`, `net472`, and `netstandard2.0`.

### Compatibility Testing

| Target Framework | Build | Run | Notes |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | Officially supported |
| net9.0 | ✅ | ✅ | Builds via forward-compat — used in workshop samples |
| net10.0 | ✅ | ✅ | Builds and runs via forward-compat with .NET 10.0.3 runtime |

The net8.0 assembly fit load on newer runtimes through .NET forward-compatibility, so build go succeed. But, dis one no dey documented and SDK team never test am.

### Why Samples Dey Target net9.0

1. **.NET 9 na the latest stable release** — most workshop participants get am installed  
2. **Forward compatibility dey work** — the net8.0 assembly inside NuGet package fit run on .NET 9 runtime without issues  
3. **.NET 10 (preview/RC)** is too new to use for workshop wey en go work for everybody  

**Expected:** Future SDK releases suppose consider to add `net9.0` and `net10.0` TFMs along with `net8.0` to match the pattern wey `Microsoft.Agents.AI.OpenAI` dey use and to provide confirmed support for newer runtimes.

---

## 6. JavaScript ChatClient Streaming Use Callbacks, No Async Iterators

**Status:** Open  
**Severity:** Documentation wahala  
**Component:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

The `ChatClient` wey `model.createChatClient()` return get `completeStreamingChat()` method, but e dey use **callback pattern** instead of async iterable:

```javascript
// ❌ Dis no dey work — e go throw "stream no be async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Di correct waya — pass one callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```
  
**Impact:** Developers wey sabi the OpenAI SDK async iteration pattern (`for await`) go meet confusing errors. The callback suppose be valid function otherwise SDK go throw "Callback must be a valid function."

**Expected:** Make dem document the callback pattern for SDK reference. Or else, support async iterable pattern to dey consistent with OpenAI SDK.

---

## Environment Details

| Component | Version |
|-----------|---------|
| OS | Windows 11 ARM64 |
| Hardware | Snapdragon X Elite (X1E78100) |
| Foundry Local CLI | 0.8.117 |
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 9.0.312, 10.0.104 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |