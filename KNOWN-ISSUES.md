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

## 2. QNN Execution Provider Backend Path Warning

**Status:** Open (partially mitigated — QNN EP autoregisters as of CLI v0.8.117)
**Severity:** Warning (non-blocking)
**Component:** ONNX Runtime QNN Execution Provider
**Reproduction:** Loading any model on Snapdragon hardware with NPU support

```
[W:onnxruntime:FoundryLocalCore, qnn_execution_provider.cc:370 onnxruntime::QNNExecutionProvider::QNNExecutionProvider]
Unable to determine backend path from provider options. Using default.
```

Followed by node assignment warnings:

```
[W:onnxruntime:, session_state.cc:1316 onnxruntime::VerifyEachNodeIsAssignedToAnEp]
Some nodes were not assigned to the preferred execution providers which may or may not have a negative impact on performance.
```

**Impact:** Unclear whether the NPU is actually being utilised or if everything falls back to CPU. The warning offers no actionable guidance for developers.

**Expected:** Either suppress the warning when default fallback is the intended behaviour, or document which backend is actually selected.

> **Update (2026-03-11):** As of CLI v0.8.117, QNN EP autoregistration succeeds (`Successfully downloaded and registered the following EPs: QNNExecutionProvider`). The backend path warning still appears during model loads but the NPU is functional.

---

## 3. HTTP 500 Internal Server Error During Sustained LLM Inference

**Status:** Open
**Severity:** Critical
**Component:** Foundry Local inference server
**Reproduction:** Run the evaluation framework (`dotnet run eval`) — crashes partway through the second prompt variant after ~8-10 sequential completions

```
System.ClientModel.ClientResultException: Service request failed.
Status: 500 (Internal Server Error)
   at OpenAI.ClientPipelineExtensions.ProcessMessageAsync(...)
   at Examples.AgentEvaluation.LlmJudge(...) in AgentEvaluation.cs:line 128
```

**Context:** The evaluation pipeline runs 5 test cases × 2 prompt variants, each requiring two LLM calls (one for the agent response, one for the LLM-as-judge). The crash occurs after approximately 13-15 successful completions, suggesting a resource exhaustion or model context issue under sustained load.

**Workaround:** Added `try/catch` around the LLM judge call with a fallback score of 3. On the second attempt (after `foundry service stop` and restart), the full evaluation completed successfully.

**Expected:** The local inference server should handle sequential completion requests reliably without returning 500 errors. If resources are exhausted, a more informative error (e.g., 503 with retry hint) would be preferable.

---

## 4. SingleAgent NullReferenceException on First Run

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

## 5. C# SDK Requires Explicit RuntimeIdentifier

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

## 6. JavaScript Whisper — Last Audio File Returns Empty Transcription

**Status:** Open
**Severity:** Minor
**Component:** JavaScript Whisper implementation (`foundry-local-whisper.mjs`)
**Reproduction:** Run `node foundry-local-whisper.mjs` — the 5th audio file (`zava-workshop-setup.wav`) returns an empty transcription

```
============================================================
File: zava-workshop-setup.wav
============================================================

(10.6s)
```

All other 4 files transcribed correctly. The same file transcribed successfully via the C# implementation, suggesting a JavaScript SDK or ONNX Runtime Node.js binding issue with certain audio file lengths or characteristics.

**Expected:** All audio files should transcribe consistently across SDK implementations.

---

## 7. C# SDK Only Ships net8.0 — No Official .NET 9 or .NET 10 Target

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

## 8. JavaScript ChatClient Streaming Uses Callbacks, Not Async Iterators

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

## 9. Tool Calling — Model May Not Support All tool_choice Options

**Status:** Open
**Severity:** Minor
**Component:** Local inference server
**Reproduction:** Use `tool_choice: "required"` or a specific function name with smaller models

Some tool calling models (particularly qwen2.5-0.5b) may not fully support all `tool_choice` values. The `"auto"` setting works reliably, but `"required"` and specific function targeting may be ignored or produce unpredictable results.

**Workaround:** Use `tool_choice: "auto"` (the default) for the most reliable behaviour. Design tool descriptions to be clear enough that the model calls the right tool without forcing.

**Expected:** Document which `tool_choice` options are supported per model.

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
| .NET SDK | 10.0.103 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |
