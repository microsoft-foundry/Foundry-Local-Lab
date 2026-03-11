# Changelog — Foundry Local Workshop

All notable changes to this workshop are documented below.

---

## 2026-03-11 — Part 12 & 13, Web UI, Whisper Rewrite, WinML/QNN Fix, and Validation

### Added
- **Part 12: Building a Web UI for the Zava Creative Writer** — new lab guide (`labs/part12-zava-ui.md`) with exercises covering streaming NDJSON, browser `ReadableStream`, live agent status badges, and real-time article text streaming
- **Part 13: Workshop Complete** — new summary lab (`labs/part13-workshop-complete.md`) with a recap of all 12 parts, further ideas, and resource links
- **Zava UI front end:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — shared vanilla HTML/CSS/JS browser interface consumed by all three backends
- **JavaScript HTTP server:** `zava-creative-writer-local/src/javascript/server.mjs` — new Express-style HTTP server wrapping the orchestrator for browser-based access
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` and `ZavaCreativeWriterWeb.csproj` — new minimal API project serving the UI and streaming NDJSON
- **Audio sample generator:** `samples/audio/generate_samples.py` — offline TTS script using `pyttsx3` to generate Zava-themed WAV files for Part 9
- **Audio sample:** `samples/audio/zava-full-project-walkthrough.wav` — new longer audio sample for transcription testing
- **Validation script:** `validate-npu-workaround.ps1` — automated PowerShell script to validate the NPU/QNN workaround across all C# samples
- **Mermaid diagram SVGs:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML package (Issue #8):** All 3 C# `.csproj` files (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) now use conditional TFM and mutually exclusive package references for cross-platform support. On Windows: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (superset that includes QNN EP plugin). On non-Windows: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (base SDK). The hardcoded `win-arm64` RID in Zava projects was replaced with auto-detect. A transitive dependency workaround excludes native assets from `Microsoft.ML.OnnxRuntime.Gpu.Linux` which has a broken win-arm64 reference. The previous try/catch NPU workaround has been removed from all 7 C# files.

### Changed
- **Part 9 (Whisper):** Major rewrite — JavaScript now uses the SDK's built-in `AudioClient` (`model.createAudioClient()`) instead of manual ONNX Runtime inference; updated architecture descriptions, comparison tables, and pipeline diagrams to reflect JS/C# `AudioClient` approach vs Python ONNX Runtime approach
- **Part 11:** Updated navigation links (now points to Part 12); added rendered SVG diagrams for tool-calling flow and sequence
- **Part 10:** Updated navigation to route through Part 12 instead of ending the workshop
- **Python Whisper (`foundry-local-whisper.py`):** Expanded with additional audio samples and improved error handling
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Rewritten to use `model.createAudioClient()` with `audioClient.transcribe()` instead of manual ONNX Runtime sessions
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Updated to serve static UI files alongside the API
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** Removed NPU workaround (now handled by WinML package)
- **README.md:** Added Part 12 section with code sample tables and backend additions; added Part 13 section; updated learning objectives and project structure
- **KNOWN-ISSUES.md:** Updated Issue #8 (C# SDK NPU model variant — resolved by adding `Microsoft.AI.Foundry.Local.WinML` package which provides QNN EP as a plugin); updated Issue #2 to explain QNN as a WinML plugin EP; renumbered and reorganised issues; updated environment details with .NET SDK 10.0.104
- **AGENTS.md:** Updated project structure tree with new `zava-creative-writer-local` entries (`ui/`, `csharp-web/`, `server.mjs`)

### Validated
- QNN EP resolved on Snapdragon X Elite (ARM64) via `Microsoft.AI.Foundry.Local.WinML` package — NPU variant loads directly without CPU fallback
- C# agent sample runs successfully with WinML package (exit code 0)
- Foundry Local CLI v0.8.117 and SDK v0.9.0 on .NET SDK 9.0.312

---

## 2026-03-11 — Code Fixes, Model Cleanup, Mermaid Diagrams, and Validation

### Fixed
- **All 21 code samples (7 Python, 7 JavaScript, 7 C#):** Added `model.unload()` / `unload_model()` / `model.UnloadAsync()` cleanup on exit to resolve OGA memory leak warnings (Known Issue #4)
- **csharp/WhisperTranscription.cs:** Replaced fragile `AppContext.BaseDirectory` relative path with `FindSamplesDirectory()` that walks up directories to locate `samples/audio` reliably (Known Issue #7)
- **csharp/csharp.csproj:** Replaced hardcoded `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` with auto-detect fallback using `$(NETCoreSdkRuntimeIdentifier)` so `dotnet run` works on any platform without `-r` flag ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Changed
- **Part 8:** Converted eval-driven iteration loop from ASCII box diagram to rendered SVG image
- **Part 10:** Converted compilation pipeline diagram from ASCII arrows to rendered SVG image
- **Part 11:** Converted tool-calling flow and sequence diagrams to rendered SVG images
- **Part 10:** Moved "Workshop Complete!" section to Part 11 (the final lab); replaced with "Next Steps" link
- **KNOWN-ISSUES.md:** Full revalidation of all issues against CLI v0.8.117. Removed resolved: OGA Memory Leak (cleanup added), Whisper path (FindSamplesDirectory), HTTP 500 sustained inference (not reproducible, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice limitations (now works with `"required"` and specific function targeting on qwen2.5-0.5b). Updated JS Whisper issue — now all files return empty/binary output (regression from v0.9.x, severity raised to Major). Updated #4 C# RID with auto-detect workaround and [#497](https://github.com/microsoft/Foundry-Local/issues/497) link. 7 open issues remain.
- **javascript/foundry-local-whisper.mjs:** Fixed cleanup variable name (`whisperModel` → `model`)

### Validated
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — run successfully with cleanup
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — run successfully with cleanup
- C#: `dotnet build` succeeds with 0 warnings, 0 errors (net9.0 target)
- All 7 Python files pass `py_compile` syntax check
- All 7 JavaScript files pass `node --check` syntax validation

---

## 2026-03-10 — Part 11: Tool Calling, SDK API Expansion, and Model Coverage

### Added
- **Part 11: Tool Calling with Local Models** — new lab guide (`labs/part11-tool-calling.md`) with 8 exercises covering tool schemas, multi-turn flow, multiple tool calls, custom tools, ChatClient tool calling, and `tool_choice`
- **Python sample:** `python/foundry-local-tool-calling.py` — tool calling with `get_weather`/`get_population` tools using OpenAI SDK
- **JavaScript sample:** `javascript/foundry-local-tool-calling.mjs` — tool calling using the SDK's native `ChatClient` (`model.createChatClient()`)
- **C# sample:** `csharp/ToolCalling.cs` — tool calling using `ChatTool.CreateFunctionTool()` with the OpenAI C# SDK
- **Part 2, Exercise 7:** Native `ChatClient` — `model.createChatClient()` (JS) and `model.GetChatClientAsync()` (C#) as alternatives to the OpenAI SDK
- **Part 2, Exercise 8:** Model variants and hardware selection — `selectVariant()`, `variants`, NPU variant table (7 models)
- **Part 2, Exercise 9:** Model upgrades and catalogue refresh — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Part 2, Exercise 10:** Reasoning models — `phi-4-mini-reasoning` with `<think>` tag parsing examples
- **Part 3, Exercise 4:** `createChatClient` as alternative to OpenAI SDK, with streaming callback pattern documentation
- **KNOWN-ISSUES.md:** Added issue #10 (ChatClient streaming uses callbacks, not async iterators) and #11 (tool_choice limitations per model)
- **AGENTS.md:** Added Tool Calling, ChatClient, and Reasoning Models coding conventions

### Changed
- **Part 1:** Expanded model catalogue — added phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Part 2:** Expanded API reference tables — added `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Part 2:** Renumbered exercises 7-9 → 10-13 to accommodate new exercises
- **Part 3:** Updated Key Takeaways table to include native ChatClient
- **README.md:** Added Part 11 section with code sample table; added learning objective #11; updated project structure tree
- **csharp/Program.cs:** Added `toolcall` case to CLI router and updated help text

---

## 2026-03-09 — SDK v0.9.0 Update, British English, and Validation Pass

### Changed
- **All code samples (Python, JavaScript, C#):** Updated to Foundry Local SDK v0.9.0 API — fixed `await catalog.getModel()` (was missing `await`), updated `FoundryLocalManager` init patterns, fixed endpoint discovery
- **All lab guides (Parts 1-10):** Converted to British English (colour, catalogue, optimised, etc.)
- **All lab guides:** Updated SDK code examples to match v0.9.0 API surface
- **All lab guides:** Updated API reference tables and exercise code blocks
- **JavaScript critical fix:** Added missing `await` on `catalog.getModel()` — returned a `Promise` not a `Model` object, causing silent failures downstream

### Validated
- All Python samples run successfully against Foundry Local service
- All JavaScript samples run successfully (Node.js 18+)
- C# project builds and runs on .NET 9.0 (forward-compat from net8.0 SDK assembly)
- 29 files modified and validated across the workshop

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
| `KNOWN-ISSUES.md` | 2026-03-11 | Issues #10-11, updated #4 root cause |
| `CHANGELOG.md` | 2026-03-11 | This file |
