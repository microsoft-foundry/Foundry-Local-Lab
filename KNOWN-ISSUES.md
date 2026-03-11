# Known Issues — Foundry Local Workshop

Issues encountered while building and testing this workshop on a **Snapdragon X Elite (ARM64)** device running Windows, with Foundry Local SDK v0.9.0 and .NET SDK 10.0.

---

## 1. Snapdragon X Elite CPU Not Recognised by ONNX Runtime

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

---

## 3. HTTP 500 Internal Server Error During Sustained LLM Inference

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

## 4. OGA Memory Leak — Model and Tokenizer Instances Not Disposed

**Severity:** Warning
**Component:** ONNX GenAI Runtime (OGA)
**Reproduction:** Run any example to completion — the leak message appears on process exit

```
OGA Error: 1 instances of struct Generators::Model were leaked.
OGA Error: 1 instances of struct Generators::Tokenizer were leaked.
    Please see the documentation for the API being used to ensure proper cleanup.
```

In the Whisper scenario, the leak count was higher:

```
OGA Error: 1 instances of struct Generators::Model were leaked.
OGA Error: 2 instances of struct Generators::Tokenizer were leaked.
```

**Impact:** The Foundry Local SDK does not expose a `Dispose()` or `Unload()` method for models in the C# API. There is no documented way for application code to release these native resources.

**Expected:** Either:
- Expose `IDisposable`/`IAsyncDisposable` on the model or manager objects, or
- Suppress the leak warnings when the SDK is managing the lifecycle, or
- Document the proper cleanup pattern.

---

## 5. SingleAgent NullReferenceException on First Run

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

## 6. C# SDK Requires Explicit RuntimeIdentifier

**Severity:** Documentation gap
**Component:** `Microsoft.AI.Foundry.Local` NuGet package
**Reproduction:** Create a .NET 8 project without `<RuntimeIdentifier>` in the `.csproj`

Build fails with:

```
NETSDK1047: Assets file doesn't have a target for 'net8.0/win-arm64'.
```

**Impact:** The package requires `<RuntimeIdentifier>` to be explicitly set (e.g., `win-arm64`, `win-x64`). This is unusual for a NuGet package and not documented. Users on different hardware must know to change this value.

**Expected:** Either:
- Support RID-less builds via a managed fallback, or
- Document the requirement prominently in the getting started guide

---

## 7. Whisper C# Sample Path Resolution Issue

**Severity:** Minor (configuration)
**Component:** Workshop sample code
**Reproduction:** Run `dotnet run whisper` from a working directory different from the project root

The samples directory path is resolved relative to `AppContext.BaseDirectory`:

```
Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "samples", "audio")
```

On a build with output in `bin/Debug/net8.0/win-arm64/`, this resolves correctly only when the directory hierarchy matches exactly. A clean build or different RID can change the output depth.

First run output:
```
Samples directory not found: ...\bin\Debug\net8.0\win-arm64\..\..\..\..\samples\audio
Run 'python samples/audio/generate_samples.py' first.
```

**Workaround:** After `foundry service stop` and re-running, the path resolved correctly. This may be related to build output path changes between clean and incremental builds.

**Expected:** Use a more robust path resolution strategy or accept the samples directory as a command-line argument.

---

## 8. JavaScript Whisper — Last Audio File Returns Empty Transcription

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

## 9. C# SDK Only Ships net8.0 — No Official .NET 9 or .NET 10 Target

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

## 10. JavaScript ChatClient Streaming Uses Callbacks, Not Async Iterators

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

## 11. Tool Calling — Model May Not Support All tool_choice Options

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
| Foundry Local SDK (C#) | 0.9.0 |
| Microsoft.Agents.AI.OpenAI | 1.0.0-rc3 |
| OpenAI C# SDK | 2.9.0 |
| .NET SDK | 10.0.103 |
| foundry-local-sdk (Python) | 0.5.x |
| foundry-local-sdk (JS) | 0.9.x |
| Node.js | 18+ |
| ONNX Runtime | 1.18+ |
