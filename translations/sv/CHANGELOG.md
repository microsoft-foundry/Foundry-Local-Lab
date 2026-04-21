# Changelog — Foundry Local Workshop

Alla noterbara förändringar i denna workshop dokumenteras nedan.

---

## 2026-03-11 — Del 12 & 13, Webb-UI, Whisper Omskrivning, WinML/QNN Fix och Validering

### Tillagt
- **Del 12: Bygga ett webb-UI för Zava Creative Writer** — ny labbguide (`labs/part12-zava-ui.md`) med övningar som täcker streaming av NDJSON, browserns `ReadableStream`, live agentstatus-badges och realtidsströmning av artikeltext
- **Del 13: Workshop slutförd** — ny summeringslabb (`labs/part13-workshop-complete.md`) med en sammanfattning av alla 12 delar, fler idéer och resurslänkar
- **Zava UI front-end:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — delat vanilla HTML/CSS/JS webbläsargränssnitt som konsumeras av alla tre backend
- **JavaScript HTTP-server:** `zava-creative-writer-local/src/javascript/server.mjs` — ny Express-liknande HTTP-server som omsluter orkestratorn för webbläsaråtkomst
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` och `ZavaCreativeWriterWeb.csproj` — nytt minimal API-projekt som serverar UI och streaming av NDJSON
- **Generator för ljudprov:** `samples/audio/generate_samples.py` — offline TTS-skript som använder `pyttsx3` för att generera Zava-tematiska WAV-filer för Del 9
- **Ljudprov:** `samples/audio/zava-full-project-walkthrough.wav` — nytt längre ljudprov för transkriberingstest
- **Valideringsskript:** `validate-npu-workaround.ps1` — automatiserat PowerShell-skript för att validera NPU/QNN-arbetsrundan över alla C#-exempel
- **Mermaid-diagram SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML korsplattform stöd:** Alla 3 C# `.csproj`-filer (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) använder nu villkorliga TFM och ömsesidigt exklusiva paketreferenser för korsplattformstöd. På Windows: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (supermängd som inkluderar QNN EP-plugin). På icke-Windows: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (bas-SDK). Den hårdkodade `win-arm64` RID i Zava-projekten ersattes med automatisk upptäckt. En transitiv beroendelösning exkluderar native tillgångar från `Microsoft.ML.OnnxRuntime.Gpu.Linux` som har en trasig win-arm64-referens. Den tidigare try/catch NPU-lösningen togs bort från alla 7 C#-filer.

### Ändrat
- **Del 9 (Whisper):** Stor omskrivning — JavaScript använder nu SDK:ns inbyggda `AudioClient` (`model.createAudioClient()`) istället för manuell ONNX Runtime inference; uppdaterade arkitekturbeskrivningar, jämförelsetabeller och pipeline-diagram för att spegla JS/C# `AudioClient`-ansats kontra Python ONNX Runtime-ansats
- **Del 11:** Uppdaterade navigationslänkar (pekar nu till Del 12); tillagda renderade SVG-diagram för verktygsanropsflöde och sekvens
- **Del 10:** Uppdaterad navigering för att ruttas via Del 12 istället för att avsluta workshopen
- **Python Whisper (`foundry-local-whisper.py`):** Utökad med ytterligare ljudprov och förbättrad felhantering
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Omskriven för att använda `model.createAudioClient()` med `audioClient.transcribe()` istället för manuella ONNX Runtime-sessioner
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Uppdaterad för att servera statiska UI-filer tillsammans med API
- **Zava C# konsol (`zava-creative-writer-local/src/csharp/Program.cs`):** Borttagen NPU-lösning (hanteras nu av WinML-paketet)
- **README.md:** Tillagd Del 12-sektion med kodexempelstabeller och backend-tillägg; tillagd Del 13-sektion; uppdaterade inlärningsmål och projektstruktur
- **KNOWN-ISSUES.md:** Borttagen löst Issue #7 (C# SDK NPU Model Variant — hanteras nu av WinML-paketet). Omsiffererade återstående problem till #1–#6. Uppdaterade miljödetaljer med .NET SDK 10.0.104
- **AGENTS.md:** Uppdaterad projektstruktur med nya `zava-creative-writer-local`-poster (`ui/`, `csharp-web/`, `server.mjs`); uppdaterade viktiga C#-paket och villkorlig TFM-information
- **labs/part2-foundry-local-sdk.md:** Uppdaterat `.csproj`-exempel för att visa full korsplattformsmönster med villkorlig TFM, ömsesidigt exklusiva paketreferenser och förklarande anteckning

### Validerat
- Alla 3 C# projekt (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) bygger framgångsrikt på Windows ARM64
- Chatt-exempel (`dotnet run chat`): modell laddas som `phi-3.5-mini-instruct-qnn-npu:1` via WinML/QNN — NPU-variant laddas direkt utan CPU-fallback
- Agent-exempel (`dotnet run agent`): körs end-to-end med flerstegs konversation, exit-kod 0
- Foundry Local CLI v0.8.117 och SDK v0.9.0 på .NET SDK 9.0.312

---

## 2026-03-11 — Kodfixar, Modellrensning, Mermaid-diagram och Validering

### Fixat
- **Alla 21 kodexempel (7 Python, 7 JavaScript, 7 C#):** Lade till `model.unload()` / `unload_model()` / `model.UnloadAsync()` rensning vid avslut för att lösa OGA-minnesläckvarningar (Känt Problem #4)
- **csharp/WhisperTranscription.cs:** Ersatte bräcklig relativ sökväg med `AppContext.BaseDirectory` till `FindSamplesDirectory()` som söker uppåt i katalogträd för att pålitligt hitta `samples/audio` (Känt Problem #7)
- **csharp/csharp.csproj:** Ersatte hårdkodad `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` med autoupptäckt fallback som använder `$(NETCoreSdkRuntimeIdentifier)` så att `dotnet run` fungerar på vilken plattform som helst utan `-r` flagga ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Ändrat
- **Del 8:** Konverterade eval-driven iterationsslinga från ASCII-boxdiagram till renderad SVG-bild
- **Del 10:** Konverterade kompilationspipelinesdiagram från ASCII-pilar till renderad SVG-bild
- **Del 11:** Konverterade verktygsanropsflödes- och sekvensdiagram till renderade SVG-bilder
- **Del 10:** Flyttade avsnittet "Workshop slutförd!" till Del 11 (slutliga labben); ersatt med länk "Nästa steg"
- **KNOWN-ISSUES.md:** Fullständig omvalidering av alla problem mot CLI v0.8.117. Tog bort lösta: OGA Memory Leak (rensning tillsatt), Whisper väg (FindSamplesDirectory), HTTP 500 sustained inference (ej reproducerbar, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice-begränsningar (fungerar nu med `"required"` och specifikt funktionsmål på qwen2.5-0.5b). Uppdaterade JS Whisper-problem — nu returnerar alla filer tomt/binärt output (regression från v0.9.x, svårighetsgrad höjd till Major). Uppdaterade #4 C# RID med autoupptäckt workaround och [#497](https://github.com/microsoft/Foundry-Local/issues/497) länk. 7 öppna ärenden kvarstår.
- **javascript/foundry-local-whisper.mjs:** Fixade rensningsvariabel-namn (`whisperModel` → `model`)

### Validerat
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — körs framgångsrikt med rensning
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — körs framgångsrikt med rensning
- C#: `dotnet build` lyckas med 0 varningar, 0 fel (net9.0 mål)
- Alla 7 Python-filer klarar `py_compile` syntaxkontroll
- Alla 7 JavaScript-filer klarar `node --check` syntaxvalidering

---

## 2026-03-10 — Del 11: Verktygsanrop, SDK API Expansion och Modellomfattning

### Tillagt
- **Del 11: Verktygsanrop med lokala modeller** — ny labbguide (`labs/part11-tool-calling.md`) med 8 övningar som täcker verktygsscheman, flerstegsflöde, flera verktygsanrop, egna verktyg, ChatClient verktygsanrop och `tool_choice`
- **Python-exempel:** `python/foundry-local-tool-calling.py` — verktygsanrop med `get_weather`/`get_population` verktyg via OpenAI SDK
- **JavaScript-exempel:** `javascript/foundry-local-tool-calling.mjs` — verktygsanrop med SDK:ns inbyggda `ChatClient` (`model.createChatClient()`)
- **C#-exempel:** `csharp/ToolCalling.cs` — verktygsanrop med `ChatTool.CreateFunctionTool()` med OpenAI C# SDK
- **Del 2, Övning 7:** Inbyggd `ChatClient` — `model.createChatClient()` (JS) och `model.GetChatClientAsync()` (C#) som alternativ till OpenAI SDK
- **Del 2, Övning 8:** Modellvarianter och hårdvaruval — `selectVariant()`, `variants`, NPU-variant tabell (7 modeller)
- **Del 2, Övning 9:** Modelluppgraderingar och kataloguppdatering — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Del 2, Övning 10:** Resoneringsmodeller — `phi-4-mini-reasoning` med exempel på `<think>` taggparsning
- **Del 3, Övning 4:** `createChatClient` som alternativ till OpenAI SDK, med dokumentation om streamad callback pattern
- **AGENTS.md:** Tillagt kodningskonventioner för Verktygsanrop, ChatClient och Resoneringsmodeller

### Ändrat
- **Del 1:** Utökad modellkatalog — tillagt phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Del 2:** Utökade API-referenstabeller — tillagt `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Del 2:** Övningar 7-9 numreras om till 10-13 för att rymma nya övningar
- **Del 3:** Uppdaterad tabell för nyckelpunkter för att inkludera inbyggd ChatClient
- **README.md:** Tillagd Del 11-sektion med kodexempelstabell; tillagt inlärningsmål #11; uppdaterat projektstrukturträd
- **csharp/Program.cs:** Tillagd `toolcall` case till CLI-router och uppdaterad hjälpttext

---

## 2026-03-09 — SDK v0.9.0 Uppdatering, Brittisk Engelska och Valideringspass

### Ändrat
- **Alla kodexempel (Python, JavaScript, C#):** Uppdaterade till Foundry Local SDK v0.9.0 API — fixade `await catalog.getModel()` (saknade `await`), uppdaterade initieringsmönster för `FoundryLocalManager`, fixade endpoint-upptäckt
- **Alla labbguider (Del 1-10):** Översatt och konverterat till brittisk engelska (colour, catalogue, optimised, etc.)
- **Alla labbguider:** Uppdaterade SDK-kodexempel för att matcha v0.9.0 API-yta
- **Alla labbguider:** Uppdaterade API-referenstabeller och övningskodblock
- **JavaScript kritisk fix:** Lade till saknat `await` på `catalog.getModel()` — returnerade en `Promise` inte ett `Model` objekt, vilket orsakade tysta fel nedströms

### Validerat
- Alla Python-exempel körs framgångsrikt mot Foundry Local-tjänsten
- Alla JavaScript-exempel körs framgångsrikt (Node.js 18+)
- C#-projekt bygger och körs på .NET 9.0 (framåtkopplat från net8.0 SDK-assembly)
- 29 filer modifierade och validerade i hela workshopen

---

## Filindex

| Fil | Senast Uppdaterad | Beskrivning |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Utökad modellkatalog |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nya övningar 7-10, utökade API-tabeller |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Ny övning 4 (ChatClient), uppdaterade nyckelpunkter |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + brittisk engelska |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + brittisk engelska |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + brittisk engelska |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + brittisk engelska |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid-diagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + brittisk engelska |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid-diagram, flyttade Workshop Complete till del 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Nytt labb, Mermaid-diagram, avsnitt Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Nytt: exempel på verktygsanrop |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Nytt: exempel på verktygsanrop |
| `csharp/ToolCalling.cs` | 2026-03-10 | Nytt: exempel på verktygsanrop |
| `csharp/Program.cs` | 2026-03-10 | Lade till `toolcall` CLI-kommando |
| `README.md` | 2026-03-10 | Del 11, projektstruktur |
| `AGENTS.md` | 2026-03-10 | Verktygsanrop + ChatClient-konventioner |
| `KNOWN-ISSUES.md` | 2026-03-11 | Tog bort löst Issue #7, 6 öppna ärenden kvar |
| `csharp/csharp.csproj` | 2026-03-11 | Plattformoberoende TFM, villkorsstyrda referenser för WinML/bas-SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Plattformoberoende TFM, automatisk RID-detektering |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Plattformoberoende TFM, automatisk RID-detektering |
| `csharp/BasicChat.cs` | 2026-03-11 | Tog bort NPU try/catch-lösning |
| `csharp/SingleAgent.cs` | 2026-03-11 | Tog bort NPU try/catch-lösning |
| `csharp/MultiAgent.cs` | 2026-03-11 | Tog bort NPU try/catch-lösning |
| `csharp/RagPipeline.cs` | 2026-03-11 | Tog bort NPU try/catch-lösning |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Tog bort NPU try/catch-lösning |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Plattformoberoende .csproj-exempel |
| `AGENTS.md` | 2026-03-11 | Uppdaterade C#-paket och TFM-detaljer |
| `CHANGELOG.md` | 2026-03-11 | Denna fil |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfriskrivning**:  
Detta dokument har översatts med hjälp av AI-översättningstjänsten [Co-op Translator](https://github.com/Azure/co-op-translator). Även om vi strävar efter noggrannhet, vänligen notera att automatiska översättningar kan innehålla fel eller avvikelser. Det ursprungliga dokumentet på dess modersmål bör betraktas som den auktoritativa källan. För kritisk information rekommenderas professionell mänsklig översättning. Vi ansvarar inte för några missförstånd eller feltolkningar som uppstår vid användning av denna översättning.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->