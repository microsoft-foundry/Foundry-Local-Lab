# Wijzigingslogboek — Foundry Local Workshop

Alle belangrijke wijzigingen voor deze workshop worden hieronder gedocumenteerd.

---

## 2026-03-11 — Deel 12 & 13, Web UI, Whisper-herschrijving, WinML/QNN-fix en validatie

### Toegevoegd
- **Deel 12: Een web-UI bouwen voor de Zava Creative Writer** — nieuwe labgids (`labs/part12-zava-ui.md`) met oefeningen over streaming NDJSON, browser `ReadableStream`, live agent statusbadges en realtime artikeltekst streaming
- **Deel 13: Workshop voltooid** — nieuwe samenvattingslab (`labs/part13-workshop-complete.md`) met een terugblik op alle 12 delen, verdere ideeën en bronnenlinks
- **Zava UI front-end:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — gedeelde vanilla HTML/CSS/JS browserinterface gebruikt door alle drie de backends
- **JavaScript HTTP-server:** `zava-creative-writer-local/src/javascript/server.mjs` — nieuwe Express-stijl HTTP-server die de orkestrator omhult voor browsertoegang
- **C# ASP.NET Core-backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` en `ZavaCreativeWriterWeb.csproj` — nieuw minimal API-project dat de UI en streaming NDJSON levert
- **Audio sample generator:** `samples/audio/generate_samples.py` — offline TTS-script met `pyttsx3` om Zava-thema WAV-bestanden te genereren voor Deel 9
- **Audio sample:** `samples/audio/zava-full-project-walkthrough.wav` — nieuwe langere audio sample voor transcriptietests
- **Validatiescript:** `validate-npu-workaround.ps1` — geautomatiseerd PowerShell-script om de NPU/QNN work-around te valideren voor alle C#-samples
- **Mermaid diagram SVG’s:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML cross-platform ondersteuning:** Alle 3 C# `.csproj` bestanden (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) gebruiken nu conditionele TFM en wederzijds exclusieve pakketreferenties voor cross-platform ondersteuning. Op Windows: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (superset inclusief QNN EP-plugin). Op niet-Windows: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (basis SDK). De hardcoded `win-arm64` RID in Zava-projecten is vervangen door autodetectie. Een transitieve afhankelijkheidswork-around sluit native assets uit van `Microsoft.ML.OnnxRuntime.Gpu.Linux` die een kapotte win-arm64 reference heeft. De vorige try/catch NPU work-around is verwijderd uit alle 7 C# bestanden.

### Gewijzigd
- **Deel 9 (Whisper):** Grote herschrijving — JavaScript gebruikt nu de SDK ingebouwde `AudioClient` (`model.createAudioClient()`) in plaats van handmatige ONNX Runtime inferentie; bijgewerkte architectuurbeschrijvingen, vergelijkingstabellen en pijplijndiagrammen om JS/C# `AudioClient` benadering versus Python ONNX Runtime te reflecteren
- **Deel 11:** Bijgewerkte navigatielinks (wijst nu naar Deel 12); toegevoegde gerenderde SVG-diagrammen voor tool-aanroep flow en sequentie
- **Deel 10:** Navigatie bijgewerkt om via Deel 12 te lopen in plaats van de workshop te beëindigen
- **Python Whisper (`foundry-local-whisper.py`):** Uitgebreid met extra audio samples en verbeterde foutafhandeling
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Herschreven om `model.createAudioClient()` met `audioClient.transcribe()` te gebruiken in plaats van handmatige ONNX Runtime sessies
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Bijgewerkt om statische UI-bestanden te serveren naast de API
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU work-around verwijderd (nu afgehandeld door WinML-pakket)
- **README.md:** Toegevoegd deel 12 sectie met codevoorbeelden tabellen en backend toevoegingen; toegevoegd deel 13 sectie; bijgewerkte leerdoelen en projectstructuur
- **KNOWN-ISSUES.md:** Opgeloste Issue #7 verwijderd (C# SDK NPU Model Variant – nu afgehandeld door WinML-pakket). Overgebleven issues hernummerd naar #1–#6. Bijgewerkte omgevingsdetails met .NET SDK 10.0.104
- **AGENTS.md:** Bijgewerkte projectstructuurbomen met nieuwe `zava-creative-writer-local` items (`ui/`, `csharp-web/`, `server.mjs`); bijgewerkte C# kernpakketten en conditionele TFM details
- **labs/part2-foundry-local-sdk.md:** `.csproj`-voorbeeld bijgewerkt om volledige cross-platform patroon met conditionele TFM, wederzijds exclusieve pakketreferenties en verklarende noot weer te geven

### Gevalideerd
- Alle 3 C# projecten (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) bouwen succesvol op Windows ARM64
- Chat sample (`dotnet run chat`): model laadt als `phi-3.5-mini-instruct-qnn-npu:1` via WinML/QNN — NPU variant laadt direct zonder CPU fallback
- Agent sample (`dotnet run agent`): draait end-to-end met multi-turn conversatie, exit code 0
- Foundry Local CLI v0.8.117 en SDK v0.9.0 op .NET SDK 9.0.312

---

## 2026-03-11 — Codefixes, model opschoning, Mermaid diagrammen en validatie

### Gefixt
- **Alle 21 code samples (7 Python, 7 JavaScript, 7 C#):** Toegevoegd `model.unload()` / `unload_model()` / `model.UnloadAsync()` opruiming bij afsluiten om OGA geheugenvlek waarschuwingen op te lossen (Known Issue #4)
- **csharp/WhisperTranscription.cs:** Vervangen fragiele `AppContext.BaseDirectory` relatieve pad door `FindSamplesDirectory()` die directories omhoog doorloopt om betrouwbaar `samples/audio` te vinden (Known Issue #7)
- **csharp/csharp.csproj:** Vervangen hardcoded `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` door autodetect fallback via `$(NETCoreSdkRuntimeIdentifier)` zodat `dotnet run` werkt op elk platform zonder `-r` vlag ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Gewijzigd
- **Deel 8:** Omgezet eval-gedreven iteratielus van ASCII vakdiagram naar gerenderde SVG afbeelding
- **Deel 10:** Omgezet compilatiepijplijn diagram van ASCII pijlen naar gerenderde SVG afbeelding
- **Deel 11:** Omgezet tool-aanroep flow en sequentie diagrammen naar gerenderde SVG-afbeeldingen
- **Deel 10:** "Workshop Complete!" sectie verplaatst naar Deel 11 (laatste lab); vervangen door "Vervolgstappen" link
- **KNOWN-ISSUES.md:** Volledige hervalidatie van alle issues tegen CLI v0.8.117. Verwijderde opgeloste: OGA geheugenvlek (cleanup toegevoegd), Whisper pad (FindSamplesDirectory), HTTP 500 sustained inference (niet reproduceerbaar, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice beperkingen (werkt nu met `"required"` en specifieke functietargeting op qwen2.5-0.5b). Bijgewerkte JS Whisper issue — nu geven alle bestanden lege/binaire output terug (regressie vanaf v0.9.x, ernst opgehoogd naar Major). Bijgewerkte #4 C# RID met autodetect work-around en [#497](https://github.com/microsoft/Foundry-Local/issues/497) link. 7 open issues blijven.
- **javascript/foundry-local-whisper.mjs:** Variabele naam cleanup gefixed (`whisperModel` → `model`)

### Gevalideerd
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — draaien succesvol met cleanup
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — draaien succesvol met cleanup
- C#: `dotnet build` slaagt met 0 waarschuwingen, 0 fouten (net9.0 target)
- Alle 7 Python-bestanden slagen `py_compile` syntax check
- Alle 7 JavaScript-bestanden slagen `node --check` syntax validatie

---

## 2026-03-10 — Deel 11: Tool-aanroepen, SDK API-uitbreiding en modeldekking

### Toegevoegd
- **Deel 11: Tool-aanroepen met lokale modellen** — nieuwe labgids (`labs/part11-tool-calling.md`) met 8 oefeningen over toolschemas, multi-turn flow, meerdere tool-aanroepen, aangepaste tools, ChatClient tool-aanroepen en `tool_choice`
- **Python voorbeeld:** `python/foundry-local-tool-calling.py` — tool-aanroepen met `get_weather`/`get_population` tools via OpenAI SDK
- **JavaScript voorbeeld:** `javascript/foundry-local-tool-calling.mjs` — tool-aanroepen met SDK’s native `ChatClient` (`model.createChatClient()`)
- **C# voorbeeld:** `csharp/ToolCalling.cs` — tool-aanroepen met `ChatTool.CreateFunctionTool()` via de OpenAI C# SDK
- **Deel 2, Oefening 7:** Native `ChatClient` — `model.createChatClient()` (JS) en `model.GetChatClientAsync()` (C#) als alternatieven voor OpenAI SDK
- **Deel 2, Oefening 8:** Modelvarianten en hardwarekeuze — `selectVariant()`, `variants`, NPU variant tabel (7 modellen)
- **Deel 2, Oefening 9:** Model upgrades en catalogus update — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Deel 2, Oefening 10:** Redeneringsmodellen — `phi-4-mini-reasoning` met `<think>` tag parsing voorbeelden
- **Deel 3, Oefening 4:** `createChatClient` als alternatief voor OpenAI SDK, met streaming callback patroon documentatie
- **AGENTS.md:** Toegevoegd Tool Calling, ChatClient en Reasoning Models codeerconventies

### Gewijzigd
- **Deel 1:** Uitgebreide modelcatalogus — toegevoegd phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Deel 2:** Uitgebreide API referentietabellen — toegevoegd `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Deel 2:** Oefeningen 7-9 hernummerd naar 10-13 om nieuwe oefeningen te accommoderen
- **Deel 3:** Bijgewerkte belangrijkste leerpunten tabel om native ChatClient op te nemen
- **README.md:** Toegevoegd deel 11 sectie met codevoorbeelden tabel; toegevoegd leerdoel #11; bijgewerkte projectstructuurbomen
- **csharp/Program.cs:** Toegevoegd `toolcall` case aan CLI-router en helptekst bijgewerkt

---

## 2026-03-09 — SDK v0.9.0 update, Brits Engels en validatieronde

### Gewijzigd
- **Alle codevoorbeelden (Python, JavaScript, C#):** Bijgewerkt naar Foundry Local SDK v0.9.0 API — `await catalog.getModel()` gefixt (ontbrak `await`), `FoundryLocalManager` init patronen bijgewerkt, endpoint discovery gefixt
- **Alle labgidsen (Delen 1-10):** Omgezet naar Brits Engels (colour, catalogue, optimised, enz.)
- **Alle labgidsen:** SDK codevoorbeelden bijgewerkt om v0.9.0 API te weerspiegelen
- **Alle labgidsen:** API referentietabellen en oefencodeblokken bijgewerkt
- **JavaScript kritieke fix:** Ontbrekende `await` toegevoegd bij `catalog.getModel()` — gaf een `Promise` terug in plaats van een `Model` object, leidde tot stille fouten downstream

### Gevalideerd
- Alle Python samples draaien succesvol tegen Foundry Local service
- Alle JavaScript samples draaien succesvol (Node.js 18+)
- C# project bouwt en draait op .NET 9.0 (voorwaartse compatibiliteit van net8.0 SDK-assembly)
- 29 bestanden gewijzigd en gevalideerd in de workshop

---

## Bestandsindex

| Bestand | Laatst bijgewerkt | Beschrijving |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Uitgebreide modelcatalogus |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nieuwe oefeningen 7-10, uitgebreide API tabellen |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Nieuwe oefening 4 (ChatClient), bijgewerkte leerpunten |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + Brits Engels |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + Brits Engels |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Brits Engels |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Brits Engels |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Brits Engels |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diagram, Workshop Voltooid verplaatst naar Deel 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Nieuwe lab, Mermaid diagrams, Workshop Voltooid sectie |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Nieuw: voorbeeld tool aanroep |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Nieuw: voorbeeld tool aanroep |
| `csharp/ToolCalling.cs` | 2026-03-10 | Nieuw: voorbeeld tool aanroep |
| `csharp/Program.cs` | 2026-03-10 | Toegevoegd `toolcall` CLI-commando |
| `README.md` | 2026-03-10 | Deel 11, projectstructuur |
| `AGENTS.md` | 2026-03-10 | Tool aanroepen + ChatClient conventies |
| `KNOWN-ISSUES.md` | 2026-03-11 | Verwijderd opgelost Issue #7, 6 openstaande issues blijven |
| `csharp/csharp.csproj` | 2026-03-11 | Cross-platform TFM, WinML/base SDK voorwaardelijke verwijzingen |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Cross-platform TFM, automatisch detecteren RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Cross-platform TFM, automatisch detecteren RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Verwijderd NPU try/catch workaround |
| `csharp/SingleAgent.cs` | 2026-03-11 | Verwijderd NPU try/catch workaround |
| `csharp/MultiAgent.cs` | 2026-03-11 | Verwijderd NPU try/catch workaround |
| `csharp/RagPipeline.cs` | 2026-03-11 | Verwijderd NPU try/catch workaround |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Verwijderd NPU try/catch workaround |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Cross-platform .csproj voorbeeld |
| `AGENTS.md` | 2026-03-11 | Bijgewerkte C# pakketten en TFM details |
| `CHANGELOG.md` | 2026-03-11 | Dit bestand |