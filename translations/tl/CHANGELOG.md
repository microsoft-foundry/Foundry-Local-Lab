# Changelog — Foundry Local Workshop

All notable changes to this workshop are documented below.

---

## 2026-03-11 — Part 12 & 13, Web UI, Whisper Rewrite, WinML/QNN Fix, and Validation

### Added
- **Part 12: Building a Web UI for the Zava Creative Writer** — bagong lab guide (`labs/part12-zava-ui.md`) na may mga ehersisyo tungkol sa streaming NDJSON, browser `ReadableStream`, mga live na badge ng agent status, at real-time na pag-stream ng teksto ng artikulo
- **Part 13: Workshop Complete** — bagong summary lab (`labs/part13-workshop-complete.md`) na may recap ng lahat ng 12 bahagi, higit pang mga ideya, at mga link ng mga mapagkukunan
- **Zava UI front end:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — pinag-isang vanilla HTML/CSS/JS browser interface na ginagamit ng lahat ng tatlong backend
- **JavaScript HTTP server:** `zava-creative-writer-local/src/javascript/server.mjs` — bagong Express-style HTTP server na bumabalot sa orchestrator para sa browser-based na access
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` at `ZavaCreativeWriterWeb.csproj` — bagong minimal API project na nagsisilbi ng UI at streaming NDJSON
- **Audio sample generator:** `samples/audio/generate_samples.py` — offline TTS script gamit ang `pyttsx3` para gumawa ng Zava-themed WAV files para sa Part 9
- **Audio sample:** `samples/audio/zava-full-project-walkthrough.wav` — bagong mas mahaba na audio sample para sa pagsusulit ng transcription
- **Validation script:** `validate-npu-workaround.ps1` — automated PowerShell script para i-validate ang NPU/QNN workaround sa lahat ng C# sample
- **Mermaid diagram SVGs:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML cross-platform support:** Lahat ng 3 C# `.csproj` na files (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) ay gumagamit na ngayon ng conditional TFM at mutually exclusive package references para sa cross-platform support. Sa Windows: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (superset kasama ang QNN EP plugin). Sa non-Windows: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (base SDK). Ang hardcoded na `win-arm64` RID sa mga proyektong Zava ay pinalitan ng auto-detect. May transitive dependency workaround na nagsasantabi ng native assets mula sa `Microsoft.ML.OnnxRuntime.Gpu.Linux` na may sirang win-arm64 reference. Tinanggal na ang dating try/catch NPU workaround mula sa lahat ng 7 C# files.

### Changed
- **Part 9 (Whisper):** Malaking rewrite — Ang JavaScript ngayon ay gumagamit ng built-in `AudioClient` ng SDK (`model.createAudioClient()`) imbes na manwal na ONNX Runtime inference; in-update ang mga paglalarawan ng arkitektura, comparison tables, at mga pipeline diagram para ipakita ang JS/C# `AudioClient` approach kontra Python ONNX Runtime approach
- **Part 11:** In-update ang mga navigation links (ngaun ay tumuturo sa Part 12); nagdagdag ng mga rendered SVG diagrams para sa tool-calling flow at sequence
- **Part 10:** In-update ang navigation para dumaan sa Part 12 sa halip na wakasan ang workshop
- **Python Whisper (`foundry-local-whisper.py`):** Pinalawak na may dagdag na mga audio sample at pinahusay na error handling
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Nirewrite upang gamitin ang `model.createAudioClient()` na may `audioClient.transcribe()` sa halip na manwal na ONNX Runtime sessions
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** In-update para magsilbi ng static UI files kasabay ng API
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** Tinanggal ang NPU workaround (ngayon ay hawak na ng WinML package)
- **README.md:** Nagdagdag ng Part 12 na seksyon na may mga code sample table at mga backend additions; nagdagdag ng Part 13 na seksyon; in-update ang mga learning objectives at project structure
- **KNOWN-ISSUES.md:** Tinanggal ang nalutas na Isyu #7 (C# SDK NPU Model Variant — ngayon ay hawak ng WinML package). In-renumber ang natitirang mga isyu sa #1–#6. In-update ang detalye ng environment gamit ang .NET SDK 10.0.104
- **AGENTS.md:** In-update ang project structure tree na may bagong `zava-creative-writer-local` entries (`ui/`, `csharp-web/`, `server.mjs`); in-update ang mga key packages ng C# at conditional TFM na detalye
- **labs/part2-foundry-local-sdk.md:** In-update ang `.csproj` example upang ipakita ang buong cross-platform pattern na may conditional TFM, mutually exclusive package references, at paliwanag na nota

### Validated
- Lahat ng 3 C# projects (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) ay matagumpay na nabuo sa Windows ARM64
- Chat sample (`dotnet run chat`): naglo-load ang model bilang `phi-3.5-mini-instruct-qnn-npu:1` gamit ang WinML/QNN — direktang naglo-load ang NPU variant nang walang CPU fallback
- Agent sample (`dotnet run agent`): tumakbo end-to-end na may multi-turn conversation, exit code 0
- Foundry Local CLI v0.8.117 at SDK v0.9.0 sa .NET SDK 9.0.312

---

## 2026-03-11 — Code Fixes, Model Cleanup, Mermaid Diagrams, and Validation

### Fixed
- **Lahat ng 21 code samples (7 Python, 7 JavaScript, 7 C#):** Nagdagdag ng `model.unload()` / `unload_model()` / `model.UnloadAsync()` na cleanup sa pag-exit upang lutasin ang OGA memory leak warnings (Known Issue #4)
- **csharp/WhisperTranscription.cs:** Pinalitan ang marupok na `AppContext.BaseDirectory` relative path ng `FindSamplesDirectory()` na naglalakad pataas sa mga direktor upang hanapin nang maaasahan ang `samples/audio` (Known Issue #7)
- **csharp/csharp.csproj:** Pinalitan ang hardcoded na `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` ng auto-detect fallback gamit ang `$(NETCoreSdkRuntimeIdentifier)` para gumana ang `dotnet run` sa anumang platform nang walang `-r` flag ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Changed
- **Part 8:** Kinonvert ang eval-driven iteration loop mula sa ASCII box diagram patungo sa rendered SVG image
- **Part 10:** Kinonvert ang compilation pipeline diagram mula sa ASCII arrows patungo sa rendered SVG image
- **Part 11:** Kinonvert ang tool-calling flow at sequence diagrams sa rendered SVG images
- **Part 10:** Inilipat ang "Workshop Complete!" na seksyon sa Part 11 (ang huling lab); pinalitan ng link na "Next Steps"
- **KNOWN-ISSUES.md:** Buong revalidation ng lahat ng isyu laban sa CLI v0.8.117. Tinanggal ang mga nalutas: OGA Memory Leak (nadagdagang cleanup), Whisper path (FindSamplesDirectory), HTTP 500 sustained inference (hindi nareproduce, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice limitations (ngayon ay gumagana sa `"required"` at specific function targeting sa qwen2.5-0.5b). In-update ang JS Whisper issue — lahat ng files ay nagbabalik na ng walang laman/binary output (regression mula sa v0.9.x, tumaas ang severity sa Major). In-update ang #4 C# RID gamit ang auto-detect workaround at [#497](https://github.com/microsoft/Foundry-Local/issues/497) link. May natitira pang 7 open issues.
- **javascript/foundry-local-whisper.mjs:** Inayos ang pangalan ng cleanup variable (`whisperModel` → `model`)

### Validated
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — matagumpay na tumakbo na may cleanup
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — matagumpay na tumakbo na may cleanup
- C#: `dotnet build` ay matagumpay na may 0 warnings, 0 errors (net9.0 target)
- Lahat ng 7 Python files pumasa sa `py_compile` syntax check
- Lahat ng 7 JavaScript files pumasa sa `node --check` syntax validation

---

## 2026-03-10 — Part 11: Tool Calling, SDK API Expansion, and Model Coverage

### Added
- **Part 11: Tool Calling with Local Models** — bagong lab guide (`labs/part11-tool-calling.md`) na may 8 ehersisyo tungkol sa tool schemas, multi-turn flow, maraming tawag sa tool, custom tools, ChatClient tool calling, at `tool_choice`
- **Python sample:** `python/foundry-local-tool-calling.py` — tool calling gamit ang `get_weather`/`get_population` tools sa OpenAI SDK
- **JavaScript sample:** `javascript/foundry-local-tool-calling.mjs` — tool calling gamit ang native `ChatClient` ng SDK (`model.createChatClient()`)
- **C# sample:** `csharp/ToolCalling.cs` — tool calling gamit ang `ChatTool.CreateFunctionTool()` sa OpenAI C# SDK
- **Part 2, Exercise 7:** Native `ChatClient` — `model.createChatClient()` (JS) at `model.GetChatClientAsync()` (C#) bilang alternatibo sa OpenAI SDK
- **Part 2, Exercise 8:** Mga variant ng model at pagpili ng hardware — `selectVariant()`, `variants`, table ng NPU variant (7 models)
- **Part 2, Exercise 9:** Mga pag-upgrade ng model at pag-refresh ng catalogue — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Part 2, Exercise 10:** Mga reasoning models — `phi-4-mini-reasoning` na may mga halimbawa ng `<think>` tag parsing
- **Part 3, Exercise 4:** `createChatClient` bilang alternatibo sa OpenAI SDK, na may dokumentasyon ng streaming callback pattern
- **AGENTS.md:** Nadagdag Tool Calling, ChatClient, at Reasoning Models na mga coding convention

### Changed
- **Part 1:** Pinalawak na model catalogue — nadagdag ang phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Part 2:** Pinalawak ang API reference tables — nadagdag ang `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Part 2:** In-renumber ang mga ehersisyo 7-9 → 10-13 upang masaklaw ang mga bagong ehersisyo
- **Part 3:** In-update ang Key Takeaways table para isama ang native ChatClient
- **README.md:** Nagdagdag ng Part 11 na seksyon na may code sample table; nagdagdag ng learning objective #11; in-update ang project structure tree
- **csharp/Program.cs:** Nagdagdag ng `toolcall` case sa CLI router at in-update ang help text

---

## 2026-03-09 — SDK v0.9.0 Update, British English, and Validation Pass

### Changed
- **Lahat ng code samples (Python, JavaScript, C#):** In-update sa Foundry Local SDK v0.9.0 API — inayos ang `await catalog.getModel()` (nawala ang `await`), in-update ang mga `FoundryLocalManager` init patterns, inayos ang endpoint discovery
- **Lahat ng lab guides (Parts 1-10):** Kinonvert sa British English (colour, catalogue, optimised, atbp.)
- **Lahat ng lab guides:** In-update ang SDK code examples upang tumugma sa v0.9.0 API surface
- **Lahat ng lab guides:** In-update ang API reference tables at mga exercise code blocks
- **JavaScript critical fix:** Nagdagdag ng nawawalang `await` sa `catalog.getModel()` — nagbalik dapat ng `Promise` hindi `Model` object, na nagdulot ng silent failures sa downstream

### Validated
- Lahat ng Python samples ay matagumpay na tumakbo laban sa Foundry Local service
- Lahat ng JavaScript samples ay matagumpay na tumakbo (Node.js 18+)
- C# project ay nabuo at tumakbo sa .NET 9.0 (forward-compat mula sa net8.0 SDK assembly)
- 29 files ang binago at na-validate sa buong workshop

---

## File Index

| File | Last Updated | Description |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Pinalawak na model catalogue |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Bagong mga ehersisyo 7-10, pinalawak na API tables |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Bagong Exercise 4 (ChatClient), in-update ang takeaways |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diagram, inilipat ang Workshop Complete sa Part 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Bagong lab, Mermaid diagrams, seksyon ng Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Bago: halimbawa ng tool calling |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Bago: halimbawa ng tool calling |
| `csharp/ToolCalling.cs` | 2026-03-10 | Bago: halimbawa ng tool calling |
| `csharp/Program.cs` | 2026-03-10 | Idinagdag ang `toolcall` na utos sa CLI |
| `README.md` | 2026-03-10 | Part 11, istruktura ng proyekto |
| `AGENTS.md` | 2026-03-10 | Tool calling + mga kaugalian sa ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Tinanggal ang nalutas na Isyu #7, 6 na bukas na isyu ang natira |
| `csharp/csharp.csproj` | 2026-03-11 | Cross-platform TFM, WinML/base SDK conditional refs |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Cross-platform TFM, awtomatikong pagtukoy ng RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Cross-platform TFM, awtomatikong pagtukoy ng RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Tinanggal ang NPU try/catch workaround |
| `csharp/SingleAgent.cs` | 2026-03-11 | Tinanggal ang NPU try/catch workaround |
| `csharp/MultiAgent.cs` | 2026-03-11 | Tinanggal ang NPU try/catch workaround |
| `csharp/RagPipeline.cs` | 2026-03-11 | Tinanggal ang NPU try/catch workaround |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Tinanggal ang NPU try/catch workaround |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Halimbawa ng cross-platform .csproj |
| `AGENTS.md` | 2026-03-11 | Na-update ang mga pakete ng C# at mga detalye ng TFM |
| `CHANGELOG.md` | 2026-03-11 | Ang file na ito |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Paunawa**:  
Ang dokumentong ito ay naisalin gamit ang AI translation service na [Co-op Translator](https://github.com/Azure/co-op-translator). Bagamat nagsusumikap kami para sa katumpakan, pakatandaan na ang mga awtomatikong pagsasalin ay maaaring maglaman ng mga pagkakamali o hindi pagkakatugma. Ang orihinal na dokumento sa orihinal nitong wika ang dapat ituring na opisyal na sanggunian. Para sa mahahalagang impormasyon, inirerekomenda ang propesyonal na pagsasalin ng tao. Hindi kami mananagot para sa anumang hindi pagkakaunawaan o maling interpretasyon na bunga ng paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->