# Known Issues — Foundry Local Workshop

Issues encountered while building and testing this workshop on a **Snapdragon X Elite (ARM64)** device running Windows, with Foundry Local SDK v0.9.0, CLI v0.8.117, and .NET SDK 10.0.

> **Last validated:** 2026-03-11

---

## 1. Snapdragon X Elite CPU Not Recognised by ONNX Runtime

**Status:** Open
**Severity:** Warning (non-blocking)
**Component:** ONNX Runtime / cpuinfo
**Reproduction:** Every Foundry Local service start on Snapdragon X Elite hardware

Every time the Foundry Local service starts, two warnings are emitted:

```
Error in cpuinfo: Unknown chip model name 'Snapdragon(R) X Elite - X1E78100 - Qualcomm(R) Oryon(TM) CPU'.
Please add new Windows on Arm SoC/chip support to arm/windows/init.c!
onnxruntime cpuid_info warning: Unknown CPU vendor. cpuinfo_vendor value: 0
```

**Impact:** The warnings are cosmetic — inference works correctly. However, they appear on every run and can confuse workshop participants. The ONNX Runtime cpuinfo library needs to be updated to recognise Qualcomm Oryon CPU cores.

**Expected:** Snapdragon X Elite should be recognised as a supported ARM64 CPU without emitting error-level messages.

---

## 2. SingleAgent NullReferenceException on First Run

**Status:** Open (intermittent)
**Severity:** Critical (crash)
**Component:** Foundry Local C# SDK + Microsoft Agent Framework
**Reproduction:** Run `dotnet run agent` — crashes immediately after model load

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Examples.SingleAgent.RunAsync() in SingleAgent.cs:line 37
```

**Context:** Line 37 calls `model.IsCachedAsync(default)`. The crash occurred on the first run of the agent after a fresh `foundry service stop`. Subsequent runs with the same code succeeded.

**Impact:** Intermittent — suggests a race condition in the SDK's service initialisation or catalog query. The `GetModelAsync()` call may return before the service is fully ready.

**Expected:** `GetModelAsync()` should either block until the service is ready or return a clear error message if the service hasn't finished initialising.

---

## 3. C# SDK Requires Explicit RuntimeIdentifier

**Status:** Open — tracked in [microsoft/Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497)
**Severity:** Documentation gap
**Component:** `Microsoft.AI.Foundry.Local` NuGet package
**Reproduction:** Create a .NET 8+ project without `<RuntimeIdentifier>` in the `.csproj`

Build fails with:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**Root cause:** The RID requirement is expected — the SDK ships native binaries (P/Invoke into `Microsoft.AI.Foundry.Local.Core` and ONNX Runtime), so .NET needs to know which platform-specific library to resolve.

This is documented on MS Learn ([How to use native chat completions](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-use-native-chat-completions?tabs=windows&pivots=programming-language-csharp)), where the run instructions show:

```bash
dotnet run -r:win-x64
dotnet run -r:win-arm64
```

However, users must remember the `-r` flag every time, which is easy to forget.

**Workaround:** Add an auto-detect fallback to your `.csproj` so `dotnet run` works without any flags:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)'==''">
  <RuntimeIdentifier>$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
</PropertyGroup>
```

`$(NETCoreSdkRuntimeIdentifier)` is a built-in MSBuild property that resolves to the host machine's RID automatically. The SDK's own test projects already use this pattern. Explicit `-r` flags are still honoured when provided.

> **Note:** The workshop `.csproj` includes this fallback so `dotnet run` works out of the box on any platform.

**Expected:** The `.csproj` template in the MS Learn docs should include this auto-detect pattern so users do not need to remember the `-r` flag.

---

## 4. JavaScript Whisper — Audio Transcription Returns Empty/Binary Output

**Status:** Open (regression — worsened since initial report)
**Severity:** Major
**Component:** JavaScript Whisper implementation (`foundry-local-whisper.mjs`) / `model.createAudioClient()`
**Reproduction:** Run `node foundry-local-whisper.mjs` — all audio files return empty or binary output instead of text transcription

```
============================================================
File: zava-product-description.wav
============================================================
�
(1.2s)
```

Originally only the 5th audio file returned empty; as of v0.9.x, all 5 files return a single byte (`\ufffd`) instead of transcribed text. The Python Whisper implementation using the OpenAI SDK transcribes the same files correctly.

**Expected:** `createAudioClient()` should return text transcription matching the Python/C# implementations.

---

## 5. C# SDK Only Ships net8.0 — No Official .NET 9 or .NET 10 Target

**Status:** Open
**Severity:** Documentation gap
**Component:** `Microsoft.AI.Foundry.Local` NuGet package v0.9.0
**Install command:** `dotnet add package Microsoft.AI.Foundry.Local`

The NuGet package only ships a single target framework:

```
lib/
  net8.0/
    Microsoft.AI.Foundry.Local.dll
```

No `net9.0` or `net10.0` TFM is included. By contrast, the companion package `Microsoft.Agents.AI.OpenAI` (v1.0.0-rc3) ships `net8.0`, `net9.0`, `net10.0`, `net472`, and `netstandard2.0`.

### Compatibility Testing

| Target Framework | Build | Run | Notes |
|-----------------|-------|-----|-------|
| net8.0 | ✅ | ✅ | Officially supported |
| net9.0 | ✅ | ✅ | Builds via forward-compat — used in workshop samples |
| net10.0 | ✅ | ✅ | Builds and runs via forward-compat with .NET 10.0.3 runtime |

The net8.0 assembly loads on newer runtimes through .NET's forward-compatibility mechanism, so the build succeeds. However, this is undocumented and untested by the SDK team.

### Why the Samples Target net9.0

1. **.NET 9 is the latest stable release** — most workshop participants will have it installed
2. **Forward compatibility works** — the net8.0 assembly in the NuGet package runs on the .NET 9 runtime without issues
3. **.NET 10 (preview/RC)** is too new to target in a workshop that should work for everyone

**Expected:** Future SDK releases should consider adding `net9.0` and `net10.0` TFMs alongside `net8.0` to match the pattern used by `Microsoft.Agents.AI.OpenAI` and to provide validated support for newer runtimes.

---

## 6. JavaScript ChatClient Streaming Uses Callbacks, Not Async Iterators

**Status:** Open
**Severity:** Documentation gap
**Component:** `foundry-local-sdk` JavaScript v0.9.x — `ChatClient.completeStreamingChat()`

The `ChatClient` returned by `model.createChatClient()` provides a `completeStreamingChat()` method, but it uses a **callback pattern** rather than returning an async iterable:

```javascript
// ❌ This does NOT work — throws "stream is not async iterable"
for await (const chunk of chatClient.completeStreamingChat(messages)) { ... }

// ✅ Correct pattern — pass a callback
await chatClient.completeStreamingChat(messages, (chunk) => {
  process.stdout.write(chunk.choices?.[0]?.delta?.content ?? "");
});
```

**Impact:** Developers familiar with the OpenAI SDK's async iteration pattern (`for await`) will encounter confusing errors. The callback must be a valid function or the SDK throws "Callback must be a valid function."

**Expected:** Document the callback pattern in the SDK reference. Alternatively, support the async iterable pattern for consistency with the OpenAI SDK.

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
