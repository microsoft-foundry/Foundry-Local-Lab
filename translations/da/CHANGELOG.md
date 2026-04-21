# Changelog — Foundry Local Workshop

Alle bemærkelsesværdige ændringer til denne workshop dokumenteres nedenfor.

---

## 2026-03-11 — Del 12 & 13, Web UI, Whisper Omskrivning, WinML/QNN Fix og Validering

### Tilføjet
- **Del 12: Bygning af en Web UI til Zava Creative Writer** — ny laborationsvejledning (`labs/part12-zava-ui.md`) med øvelser, der dækker streaming af NDJSON, browser `ReadableStream`, live statusbadges for agent, og realtids streaming af artikeltekst
- **Del 13: Workshop Fuldført** — nyt opsummeringslaboratorium (`labs/part13-workshop-complete.md`) med opsummering af alle 12 dele, yderligere idéer og ressource links
- **Zava UI frontend:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — delt vanilje HTML/CSS/JS browsergrænseflade brugt af alle tre backends
- **JavaScript HTTP server:** `zava-creative-writer-local/src/javascript/server.mjs` — ny Express-stil HTTP server der pakker orkestratoren til browserbaseret adgang
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` og `ZavaCreativeWriterWeb.csproj` — nyt minimal API projekt, der serverer UI og streaming af NDJSON
- **Audio prøvegenerator:** `samples/audio/generate_samples.py` — offline TTS script, der bruger `pyttsx3` til at generere Zava-tema WAV-filer til Del 9
- **Audio prøve:** `samples/audio/zava-full-project-walkthrough.wav` — ny længere lydprøve til transskriptionstest
- **Valideringsscript:** `validate-npu-workaround.ps1` — automatiseret PowerShell script til at validere NPU/QNN workaround på tværs af alle C# prøver
- **Mermaid diagram SVGs:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML tværplatforms support:** Alle 3 C# `.csproj` filer (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) bruger nu betinget TFM og gensidigt udelukkende pakkereferencer for tværplatforms support. På Windows: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (supersæt, der inkluderer QNN EP plugin). På ikke-Windows: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (basis SDK). Den hårdkodede `win-arm64` RID i Zava projekterne er erstattet med automatisk detektion. En transitiv afhængighedsworkaround udelukker native assets fra `Microsoft.ML.OnnxRuntime.Gpu.Linux`, som har en brudt win-arm64 reference. Den tidligere try/catch NPU workaround er fjernet fra alle 7 C# filer.

### Ændret
- **Del 9 (Whisper):** Stor omskrivning — JavaScript bruger nu SDK'ets indbyggede `AudioClient` (`model.createAudioClient()`) i stedet for manuel ONNX Runtime inferens; opdaterede arkitekturbeskrivelser, sammenligningstabeller og pipeline-diagrammer for at afspejle JS/C# `AudioClient` tilgangen vs Python ONNX Runtime tilgangen
- **Del 11:** Opdaterede navigationslinks (peger nu til Del 12); tilføjede rendererede SVG diagrammer til tool-calling flow og sekvens
- **Del 10:** Opdateret navigation til at lede via Del 12 i stedet for at afslutte workshoppen
- **Python Whisper (`foundry-local-whisper.py`):** Udvidet med flere lydprøver og forbedret fejlhåndtering
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Omskrevet til at bruge `model.createAudioClient()` med `audioClient.transcribe()` i stedet for manuel ONNX Runtime sessioner
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Opdateret til at servere statiske UI filer sammen med API'en
- **Zava C# konsol (`zava-creative-writer-local/src/csharp/Program.cs`):** Fjernet NPU workaround (håndteres nu af WinML pakken)
- **README.md:** Tilføjet Del 12 sektion med kodeeksempeltabeller og backend tilføjelser; tilføjet Del 13 sektion; opdaterede læringsmål og projektstruktur
- **KNOWN-ISSUES.md:** Fjernet løst Problem #7 (C# SDK NPU Model Variant — håndteres nu af WinML pakken). Omsorteret resterende problemer til #1–#6. Opdaterede miljødetaljer med .NET SDK 10.0.104
- **AGENTS.md:** Opdateret projektstruktur træ med nye `zava-creative-writer-local` poster (`ui/`, `csharp-web/`, `server.mjs`); opdaterede C# nøglepakker og betingede TFM detaljer
- **labs/part2-foundry-local-sdk.md:** Opdateret `.csproj` eksempel til at vise fuldt tværplatformsmønster med betinget TFM, gensidigt udelukkende pakkereferencer og forklarende note

### Valideret
- Alle 3 C# projekter (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) bygger succesfuldt på Windows ARM64
- Chat prøve (`dotnet run chat`): model loader som `phi-3.5-mini-instruct-qnn-npu:1` via WinML/QNN — NPU varianten loader direkte uden CPU fallback
- Agent prøve (`dotnet run agent`): kører end-to-end med multi-turn samtale, exit kode 0
- Foundry Local CLI v0.8.117 og SDK v0.9.0 på .NET SDK 9.0.312

---

## 2026-03-11 — Code Fixes, Model Rydning, Mermaid Diagrammer og Validering

### Rettet
- **Alle 21 kodeprøver (7 Python, 7 JavaScript, 7 C#):** Tilføjet `model.unload()` / `unload_model()` / `model.UnloadAsync()` oprydning ved afslutning for at løse OGA hukommelseslæksadvarsler (Kendt Problem #4)
- **csharp/WhisperTranscription.cs:** Erstattet sårbar relativ sti med `AppContext.BaseDirectory` med `FindSamplesDirectory()`, der går op gennem mapper for pålideligt at finde `samples/audio` (Kendt Problem #7)
- **csharp/csharp.csproj:** Erstattet hårdkodet `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` med auto-detektion fallback ved brug af `$(NETCoreSdkRuntimeIdentifier)`, så `dotnet run` fungerer på enhver platform uden `-r` flag ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Ændret
- **Del 8:** Konverteret eval-drevet iterationssløjfe fra ASCII-boksdiagram til rendereret SVG billede
- **Del 10:** Konverteret compilationspipeline diagram fra ASCII pile til rendereret SVG billede
- **Del 11:** Konverteret tool-calling flow og sekvensdiagrammer til rendererede SVG billeder
- **Del 10:** Flyttet "Workshop Fuldført!" sektion til Del 11 (det sidste laboratorium); erstattet med "Næste Skridt" link
- **KNOWN-ISSUES.md:** Fuld revurdering af alle problemer mod CLI v0.8.117. Fjernet løste: OGA Hukommelseslæk (oprydning tilføjet), Whisper sti (FindSamplesDirectory), HTTP 500 varig inferens (kan ikke reproduceres, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice begrænsninger (virker nu med `"required"` og specifik funktionsretning på qwen2.5-0.5b). Opdateret JS Whisper problem — nu returnerer alle filer tom/ binær output (regression fra v0.9.x, alvorlighed hævet til Major). Opdateret #4 C# RID med auto-detektion workaround og [#497](https://github.com/microsoft/Foundry-Local/issues/497) link. 7 åbne problemer tilbage.
- **javascript/foundry-local-whisper.mjs:** Rettet oprydningsvariabelnavn (`whisperModel` → `model`)

### Valideret
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — kører succesfuldt med oprydning
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — kører succesfuldt med oprydning
- C#: `dotnet build` lykkes uden advarsler og fejl (net9.0 mål)
- Alle 7 Python filer består `py_compile` syntakstjek
- Alle 7 JavaScript filer består `node --check` syntaksvalidering

---

## 2026-03-10 — Del 11: Tool Calling, SDK API Udvidelse og Modeldækning

### Tilføjet
- **Del 11: Tool Calling med Lokale Modeller** — ny laborationsvejledning (`labs/part11-tool-calling.md`) med 8 øvelser, der dækker tool skemaer, multi-turn flow, flere tool-kald, brugerdefinerede tools, ChatClient tool calling og `tool_choice`
- **Python prøve:** `python/foundry-local-tool-calling.py` — tool calling med `get_weather`/`get_population` tools ved brug af OpenAI SDK
- **JavaScript prøve:** `javascript/foundry-local-tool-calling.mjs` — tool calling ved brug af SDK'ets native `ChatClient` (`model.createChatClient()`)
- **C# prøve:** `csharp/ToolCalling.cs` — tool calling ved brug af `ChatTool.CreateFunctionTool()` med OpenAI C# SDK
- **Del 2, Øvelse 7:** Native `ChatClient` — `model.createChatClient()` (JS) og `model.GetChatClientAsync()` (C#) som alternativer til OpenAI SDK
- **Del 2, Øvelse 8:** Modelvarianter og hardwarevalg — `selectVariant()`, `variants`, NPU variant tabel (7 modeller)
- **Del 2, Øvelse 9:** Modelopgraderinger og katalogopfriskning — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Del 2, Øvelse 10:** Reasoning modeller — `phi-4-mini-reasoning` med `<think>` tag parsing eksempler
- **Del 3, Øvelse 4:** `createChatClient` som alternativ til OpenAI SDK, med streaming callback mønster dokumentation
- **AGENTS.md:** Tilføjet Tool Calling, ChatClient og Reasoning Models kodekonventioner

### Ændret
- **Del 1:** Udvidet modelkatalog — tilføjet phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Del 2:** Udvidede API referencetabeller — tilføjet `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Del 2:** Omnummereret øvelser 7-9 → 10-13 for at rumme nye øvelser
- **Del 3:** Opdateret Key Takeaways tabel til at inkludere native ChatClient
- **README.md:** Tilføjet Del 11 sektion med kodeeksempeltabel; tilføjet læringsmål #11; opdateret projektstrukturtree
- **csharp/Program.cs:** Tilføjet `toolcall` case til CLI router og opdateret hjælptekst

---

## 2026-03-09 — SDK v0.9.0 Opdatering, Britisk Engelsk og Valideringsgennemgang

### Ændret
- **Alle kodeprøver (Python, JavaScript, C#):** Opdateret til Foundry Local SDK v0.9.0 API — rettet `await catalog.getModel()` (manglede `await`), opdateret `FoundryLocalManager` init mønstre, rettet endpoint discovery
- **Alle laborationsvejledninger (Del 1-10):** Konverteret til britisk engelsk (colour, catalogue, optimised osv.)
- **Alle laborationsvejledninger:** Opdateret SDK kodeeksempler til at matche v0.9.0 API overflade
- **Alle laborationsvejledninger:** Opdateret API referencetabeller og øvelsekodeblokke
- **JavaScript kritisk fix:** Tilføjet manglende `await` på `catalog.getModel()` — returnerede en `Promise` og ikke et `Model` objekt, hvilket forårsagede stille fejlfunktioner downstream

### Valideret
- Alle Python prøver kører succesfuldt mod Foundry Local service
- Alle JavaScript prøver kører succesfuldt (Node.js 18+)
- C# projekt bygger og kører på .NET 9.0 (fremadkompatibelt fra net8.0 SDK assembly)
- 29 filer ændret og valideret på tværs af workshoppen

---

## Filindeks

| Fil | Sidst Opdateret | Beskrivelse |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Udvidet modelkatalog |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nye øvelser 7-10, udvidede API tabeller |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Ny Øvelse 4 (ChatClient), opdaterede takeaways |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + britisk engelsk |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + britisk engelsk |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diagram, flyttet Workshop Complete til Del 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Nyt laboratorium, Mermaid-diagrammer, Workshop Complete sektion |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Ny: eksempel på værktøj kald |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Ny: eksempel på værktøj kald |
| `csharp/ToolCalling.cs` | 2026-03-10 | Ny: eksempel på værktøj kald |
| `csharp/Program.cs` | 2026-03-10 | Tilføjet `toolcall` CLI kommando |
| `README.md` | 2026-03-10 | Del 11, projektstruktur |
| `AGENTS.md` | 2026-03-10 | Værktøjskald + ChatClient konventioner |
| `KNOWN-ISSUES.md` | 2026-03-11 | Fjernet løst problem #7, 6 åbne problemer tilbage |
| `csharp/csharp.csproj` | 2026-03-11 | Tværplatform TFM, WinML/base SDK betingede refs |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Tværplatform TFM, automatisk detektion af RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Tværplatform TFM, automatisk detektion af RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Fjernet NPU try/catch workaround |
| `csharp/SingleAgent.cs` | 2026-03-11 | Fjernet NPU try/catch workaround |
| `csharp/MultiAgent.cs` | 2026-03-11 | Fjernet NPU try/catch workaround |
| `csharp/RagPipeline.cs` | 2026-03-11 | Fjernet NPU try/catch workaround |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Fjernet NPU try/catch workaround |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Tværplatform .csproj eksempel |
| `AGENTS.md` | 2026-03-11 | Opdateret C# pakker og TFM detaljer |
| `CHANGELOG.md` | 2026-03-11 | Denne fil |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokument er oversat ved hjælp af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi bestræber os på nøjagtighed, skal du være opmærksom på, at automatiserede oversættelser kan indeholde fejl eller unøjagtigheder. Det oprindelige dokument på dets modersmål bør betragtes som den autoritative kilde. For kritisk information anbefales professionel menneskelig oversættelse. Vi påtager os intet ansvar for eventuelle misforståelser eller fejltolkninger, der opstår ved brug af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->