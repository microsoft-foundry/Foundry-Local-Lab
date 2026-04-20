# Changelog — Foundry Local Workshop

All important changes dem for dis workshop dey documented below.

---

## 2026-03-11 — Part 12 & 13, Web UI, Whisper Rewrite, WinML/QNN Fix, and Validation

### Added
- **Part 12: Building a Web UI for the Zava Creative Writer** — new lab guide (`labs/part12-zava-ui.md`) wit exercises wey cover streaming NDJSON, browser `ReadableStream`, live agent status badges, and real-time article text streaming
- **Part 13: Workshop Complete** — new summary lab (`labs/part13-workshop-complete.md`) wey dey recap all 12 parts, more ideas, and resource links
- **Zava UI front end:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — shared vanilla HTML/CSS/JS browser interface wey all three backends dey use
- **JavaScript HTTP server:** `zava-creative-writer-local/src/javascript/server.mjs` — new Express-style HTTP server wey dey wrap the orchestrator for browser-based access
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` and `ZavaCreativeWriterWeb.csproj` — new minimal API project wey dey serve the UI and stream NDJSON
- **Audio sample generator:** `samples/audio/generate_samples.py` — offline TTS script wey dey use `pyttsx3` to generate Zava-themed WAV files for Part 9
- **Audio sample:** `samples/audio/zava-full-project-walkthrough.wav` — new longer audio sample for transcription testing
- **Validation script:** `validate-npu-workaround.ps1` — automated PowerShell script to validate the NPU/QNN workaround across all C# samples
- **Mermaid diagram SVGs:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML cross-platform support:** All 3 C# `.csproj` files (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) now dey use conditional TFM and mutually exclusive package references for cross-platform support. For Windows: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (superset wey include QNN EP plugin). For non-Windows: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (base SDK). The hardcoded `win-arm64` RID for Zava projects don change to auto-detect. One transitive dependency workaround dey exclude native assets from `Microsoft.ML.OnnxRuntime.Gpu.Linux` wey get broken win-arm64 reference. The previous try/catch NPU workaround don comot from all 7 C# files.

### Changed
- **Part 9 (Whisper):** Major rewrite — JavaScript now dey use the SDK built-in `AudioClient` (`model.createAudioClient()`) instead of manual ONNX Runtime inference; updated architecture descriptions, comparison tables, and pipeline diagrams to show JS/C# `AudioClient` method against Python ONNX Runtime approach
- **Part 11:** Updated navigation links (now dey point to Part 12); added rendered SVG diagrams for tool-calling flow and sequence
- **Part 10:** Updated navigation to route through Part 12 instead of finishing the workshop
- **Python Whisper (`foundry-local-whisper.py`):** Expanded with more audio samples and better error handling
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Rewritten to use `model.createAudioClient()` with `audioClient.transcribe()` instead of manual ONNX Runtime sessions
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Updated to serve static UI files alongside API
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** Removed NPU workaround (now handled by WinML package)
- **README.md:** Added Part 12 section wit code sample tables and backend additions; added Part 13 section; updated learning objectives and project structure
- **KNOWN-ISSUES.md:** Removed resolved Issue #7 (C# SDK NPU Model Variant — now handled by WinML package). Renumbered remaining issues to #1–#6. Updated environment details wit .NET SDK 10.0.104
- **AGENTS.md:** Updated project structure tree wit new `zava-creative-writer-local` entries (`ui/`, `csharp-web/`, `server.mjs`); updated C# key packages and conditional TFM details
- **labs/part2-foundry-local-sdk.md:** Updated `.csproj` example to show full cross-platform pattern wit conditional TFM, mutually exclusive package references, and explanatory note

### Validated
- All 3 C# projects (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) build well for Windows ARM64
- Chat sample (`dotnet run chat`): model load as `phi-3.5-mini-instruct-qnn-npu:1` via WinML/QNN — NPU variant load direct without CPU fallback
- Agent sample (`dotnet run agent`): runs end-to-end wit multi-turn talk, exit code 0
- Foundry Local CLI v0.8.117 and SDK v0.9.0 on .NET SDK 9.0.312

---

## 2026-03-11 — Code Fixes, Model Cleanup, Mermaid Diagrams, and Validation

### Fixed
- **All 21 code samples (7 Python, 7 JavaScript, 7 C#):** Added `model.unload()` / `unload_model()` / `model.UnloadAsync()` cleanup on exit to fix OGA memory leak warnings (Known Issue #4)
- **csharp/WhisperTranscription.cs:** Replaced fragile `AppContext.BaseDirectory` relative path wit `FindSamplesDirectory()` wey dey climb directories to find `samples/audio` steady (Known Issue #7)
- **csharp/csharp.csproj:** Replaced hardcoded `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` wit auto-detect fallback using `$(NETCoreSdkRuntimeIdentifier)` so `dotnet run` go work for any platform without `-r` flag ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Changed
- **Part 8:** Converted eval-driven iteration loop from ASCII box diagram to rendered SVG image
- **Part 10:** Converted compilation pipeline diagram from ASCII arrows to rendered SVG image
- **Part 11:** Converted tool-calling flow and sequence diagrams to rendered SVG images
- **Part 10:** Moved "Workshop Complete!" section to Part 11 (the final lab); replaced with "Next Steps" link
- **KNOWN-ISSUES.md:** Full revalidation of all issues against CLI v0.8.117. Removed resolved: OGA Memory Leak (cleanup added), Whisper path (FindSamplesDirectory), HTTP 500 sustained inference (no fit reproduce, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice limitations (now fit work wit `"required"` and specific function targeting on qwen2.5-0.5b). Updated JS Whisper issue — now all files return empty/binary output (regression from v0.9.x, severity raised to Major). Updated #4 C# RID with auto-detect workaround and [#497](https://github.com/microsoft/Foundry-Local/issues/497) link. 7 open issues still dey.
- **javascript/foundry-local-whisper.mjs:** Fixed cleanup variable name (`whisperModel` → `model`)

### Validated
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — run well wit cleanup
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — run well wit cleanup
- C#: `dotnet build` succeed wit 0 warnings, 0 errors (net9.0 target)
- All 7 Python files pass `py_compile` syntax check
- All 7 JavaScript files pass `node --check` syntax validation

---

## 2026-03-10 — Part 11: Tool Calling, SDK API Expansion, and Model Coverage

### Added
- **Part 11: Tool Calling with Local Models** — new lab guide (`labs/part11-tool-calling.md`) wit 8 exercises wey cover tool schemas, multi-turn flow, plenty tool calls, custom tools, ChatClient tool calling, and `tool_choice`
- **Python sample:** `python/foundry-local-tool-calling.py` — tool calling wit `get_weather`/`get_population` tools dey use OpenAI SDK
- **JavaScript sample:** `javascript/foundry-local-tool-calling.mjs` — tool calling dey use the SDK native `ChatClient` (`model.createChatClient()`)
- **C# sample:** `csharp/ToolCalling.cs` — tool calling dey use `ChatTool.CreateFunctionTool()` wit OpenAI C# SDK
- **Part 2, Exercise 7:** Native `ChatClient` — `model.createChatClient()` (JS) and `model.GetChatClientAsync()` (C#) alternatives to OpenAI SDK
- **Part 2, Exercise 8:** Model variants and hardware select — `selectVariant()`, `variants`, NPU variant table (7 models)
- **Part 2, Exercise 9:** Model upgrades and catalogue refresh — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Part 2, Exercise 10:** Reasoning models — `phi-4-mini-reasoning` wit `<think>` tag parsing examples
- **Part 3, Exercise 4:** `createChatClient` as alternative to OpenAI SDK, wit streaming callback pattern documentation
- **AGENTS.md:** Added Tool Calling, ChatClient, and Reasoning Models coding conventions

### Changed
- **Part 1:** Expanded model catalogue — added phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Part 2:** Expanded API reference tables — added `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Part 2:** Renumbered exercises 7-9 → 10-13 to make space for new exercises
- **Part 3:** Updated Key Takeaways table to include native ChatClient
- **README.md:** Added Part 11 section wit code sample table; added learning objective #11; updated project structure tree
- **csharp/Program.cs:** Added `toolcall` case to CLI router and updated help text

---

## 2026-03-09 — SDK v0.9.0 Update, British English, and Validation Pass

### Changed
- **All code samples (Python, JavaScript, C#):** Updated to Foundry Local SDK v0.9.0 API — fixed `await catalog.getModel()` (was missing `await`), updated `FoundryLocalManager` init patterns, fixed endpoint discovery
- **All lab guides (Parts 1-10):** Converted to British English (colour, catalogue, optimised, etc.)
- **All lab guides:** Updated SDK code examples to match v0.9.0 API surface
- **All lab guides:** Updated API reference tables and exercise code blocks
- **JavaScript critical fix:** Added missing `await` on `catalog.getModel()` — returned a `Promise` no na `Model` object, wey dey cause silent failure downstream

### Validated
- All Python samples run well against Foundry Local service
- All JavaScript samples run well (Node.js 18+)
- C# project build and run on .NET 9.0 (forward-compat from net8.0 SDK assembly)
- 29 files modify and validated across the workshop

---

## File Index

| File | Last Updated | Description |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Expanded model catalogue |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | New exercises 7-10, expanded API tables |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | New Exercise 4 (ChatClient), updated takeaways |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diagram, moved Workshop Complete to Part 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | New lab, Mermaid diagrams, Workshop Complete section |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | New: tool calling sample |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | New: tool calling sample |
| `csharp/ToolCalling.cs` | 2026-03-10 | New: tool calling sample |
| `csharp/Program.cs` | 2026-03-10 | Added `toolcall` CLI command |
| `README.md` | 2026-03-10 | Part 11, project structure |
| `AGENTS.md` | 2026-03-10 | Tool calling + ChatClient conventions |
| `KNOWN-ISSUES.md` | 2026-03-11 | Removed resolved Issue #7, 6 open issues remain |
| `csharp/csharp.csproj` | 2026-03-11 | Cross-platform TFM, WinML/base SDK conditional refs |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Cross-platform TFM, auto-detect RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Cross-platform TFM, auto-detect RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Removed NPU try/catch workaround |
| `csharp/SingleAgent.cs` | 2026-03-11 | Removed NPU try/catch workaround |
| `csharp/MultiAgent.cs` | 2026-03-11 | Removed NPU try/catch workaround |
| `csharp/RagPipeline.cs` | 2026-03-11 | Removed NPU try/catch workaround |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Removed NPU try/catch workaround |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Cross-platform .csproj example |
| `AGENTS.md` | 2026-03-11 | Updated C# packages and TFM details |
| `CHANGELOG.md` | 2026-03-11 | Dis file |