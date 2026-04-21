# Endringslogg — Foundry Local Workshop

Alle merkbare endringer i denne workshopen er dokumentert nedenfor.

---

## 2026-03-11 — Del 12 & 13, Web UI, Whisper-omskriving, WinML/QNN-fiks og validering

### Lagt til
- **Del 12: Lage et Web UI for Zava Creative Writer** — ny lab-guide (`labs/part12-zava-ui.md`) med øvelser som dekker streaming NDJSON, nettleserens `ReadableStream`, live agent statusmerker og sanntids streaming av artikkeltekst
- **Del 13: Workshop fullført** — ny oppsummeringslab (`labs/part13-workshop-complete.md`) med en gjennomgang av alle 12 deler, flere ideer og ressurslenker
- **Zava UI frontend:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — delt vanilla HTML/CSS/JS nettlesergrensesnitt brukt av alle tre backendene
- **JavaScript HTTP-server:** `zava-creative-writer-local/src/javascript/server.mjs` — ny Express-stil HTTP-server som omslutter orkestratoren for nettleserbasert tilgang
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` og `ZavaCreativeWriterWeb.csproj` — nytt minimalt API-prosjekt som server UI og streaming NDJSON
- **Lydprøvegenerator:** `samples/audio/generate_samples.py` — offline TTS-skript som bruker `pyttsx3` for å generere Zava-tema WAV-filer for Del 9
- **Lydprøve:** `samples/audio/zava-full-project-walkthrough.wav` — ny lengre lydprøve for transkripsjonstesting
- **Valideringsskript:** `validate-npu-workaround.ps1` — automatisert PowerShell-skript for å validere NPU/QNN workaround på tvers av alle C#-eksempler
- **Mermaid-diagram SVGer:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML kryssplattformstøtte:** Alle 3 C# `.csproj` filer (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) bruker nå betinget TFM og gjensidig ekskluderende pakkereferanser for kryssplattformstøtte. På Windows: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (superset som inkluderer QNN EP-plugin). På ikke-Windows: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (basis SDK). Den hardkodede `win-arm64` RID i Zava-prosjektene ble erstattet med automatisk oppdagelse. En transitiv avhengighetsløsning ekskluderer native assets fra `Microsoft.ML.OnnxRuntime.Gpu.Linux` som har en ødelagt win-arm64 referanse. Den tidligere try/catch NPU workaround er fjernet fra alle 7 C# filer.

### Endret
- **Del 9 (Whisper):** Større omskriving — JavaScript bruker nå SDKs innebygde `AudioClient` (`model.createAudioClient()`) i stedet for manuell ONNX Runtime inferens; oppdaterte arkitekturbeskrivelser, sammenligningstabeller og pipeline-diagrammer for å reflektere JS/C# `AudioClient` tilnærming kontra Python ONNX Runtime tilnærming
- **Del 11:** Oppdaterte navigasjonslenker (peker nå til Del 12); la til gjengitte SVG-diagrammer for verktøy-kall flyt og sekvens
- **Del 10:** Oppdaterte navigasjon til å rute via Del 12 i stedet for å avslutte workshopen
- **Python Whisper (`foundry-local-whisper.py`):** Utvidet med flere lydprøver og forbedret feilhåndtering
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Omskrevet til å bruke `model.createAudioClient()` med `audioClient.transcribe()` i stedet for manuelle ONNX Runtime-sesjoner
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Oppdatert for å serve statiske UI-filer sammen med API
- **Zava C# konsoll (`zava-creative-writer-local/src/csharp/Program.cs`):** Fjernet NPU workaround (håndteres nå av WinML-pakken)
- **README.md:** La til Del 12 seksjon med kodeeksempel-tabeller og backend-tillegg; la til Del 13 seksjon; oppdaterte læringsmål og prosjektstruktur
- **KNOWN-ISSUES.md:** Fjernet løst Problem #7 (C# SDK NPU Model Variant — nå håndteres av WinML-pakken). Omdøpte gjenværende problemer til #1–#6. Oppdaterte miljødetaljer med .NET SDK 10.0.104
- **AGENTS.md:** Oppdatert prosjektstruktur-tre med nye `zava-creative-writer-local` oppføringer (`ui/`, `csharp-web/`, `server.mjs`); oppdaterte nøkkelpakker for C# og betinget TFM detaljer
- **labs/part2-foundry-local-sdk.md:** Oppdatert `.csproj`-eksempel for å vise full kryssplattformsmønster med betinget TFM, gjensidig ekskluderende pakkereferanser og forklarende note

### Validert
- Alle 3 C# prosjekter (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) kompilerer vellykket på Windows ARM64
- Chat-eksempel (`dotnet run chat`): modell lastes som `phi-3.5-mini-instruct-qnn-npu:1` via WinML/QNN — NPU variant lastes direkte uten CPU fallback
- Agent-eksempel (`dotnet run agent`): kjører ende-til-ende med flerturns samtale, avslutningskode 0
- Foundry Local CLI v0.8.117 og SDK v0.9.0 på .NET SDK 9.0.312

---

## 2026-03-11 — Kodefikser, modellrensing, Mermaid-diagrammer og validering

### Fikset
- **Alle 21 kodeeksempler (7 Python, 7 JavaScript, 7 C#):** La til `model.unload()` / `unload_model()` / `model.UnloadAsync()` opprydding ved avslutning for å løse OGA minnelekkasje-advarsler (Kjent problem #4)
- **csharp/WhisperTranscription.cs:** Erstattet sårbar `AppContext.BaseDirectory` relativ bane med `FindSamplesDirectory()` som går oppover i kataloger for å pålitelig finne `samples/audio` (Kjent problem #7)
- **csharp/csharp.csproj:** Erstattet hardkodet `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` med auto-detektert fallback ved bruk av `$(NETCoreSdkRuntimeIdentifier)` slik at `dotnet run` fungerer på alle plattformer uten `-r` flagg ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Endret
- **Del 8:** Konvertert eval-drevet iterasjonsløkke fra ASCII-boksdiagram til rendret SVG-bilde
- **Del 10:** Konvertert kompilering-pipeline diagram fra ASCII-piler til rendret SVG-bilde
- **Del 11:** Konvertert verktøy-kall flyt og sekvensdiagrammer til rendret SVG-bilder
- **Del 10:** Flyttet "Workshop ferdig!" seksjon til Del 11 (den siste laben); erstattet med "Neste steg" lenke
- **KNOWN-ISSUES.md:** Full revalidering av alle problemer mot CLI v0.8.117. Fjernet løste: OGA Memory Leak (opprydding lagt til), Whisper bane (FindSamplesDirectory), HTTP 500 vedvarende inferenser (ikke reproduserbar, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), begrensninger i tool_choice (fungerer nå med `"required"` og spesifikk funksjonstildeling på qwen2.5-0.5b). Oppdatert JS Whisper problem — nå returnerer alle filer tom/ binær utdata (regresjon fra v0.9.x, alvorlighetsgrad økt til Major). Oppdatert #4 C# RID med auto-detect workaround og [#497](https://github.com/microsoft/Foundry-Local/issues/497) lenke. 7 åpne problemer gjenstår.
- **javascript/foundry-local-whisper.mjs:** Fikset oppryddingsvariabelnavn (`whisperModel` → `model`)

### Validert
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — kjører vellykket med opprydding
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — kjører vellykket med opprydding
- C#: `dotnet build` fullføres med 0 advarsler, 0 feil (net9.0 mål)
- Alle 7 Python-filer består `py_compile` syntaks-sjekk
- Alle 7 JavaScript-filer består `node --check` syntaksvalidering

---

## 2026-03-10 — Del 11: Verktøy-kall, SDK API-utvidelse og modelldekning

### Lagt til
- **Del 11: Verktøy-kall med lokale modeller** — ny lab-guide (`labs/part11-tool-calling.md`) med 8 øvelser som dekker verktøyskjemaer, flerturns flyt, flere verktøykall, egendefinerte verktøy, ChatClient verktøykall og `tool_choice`
- **Python-eksempel:** `python/foundry-local-tool-calling.py` — verktøy-kall med `get_weather`/`get_population` verktøy ved bruk av OpenAI SDK
- **JavaScript-eksempel:** `javascript/foundry-local-tool-calling.mjs` — verktøy-kall ved bruk av SDKs native `ChatClient` (`model.createChatClient()`)
- **C#-eksempel:** `csharp/ToolCalling.cs` — verktøy-kall ved bruk av `ChatTool.CreateFunctionTool()` med OpenAI C# SDK
- **Del 2, Øvelse 7:** Native `ChatClient` — `model.createChatClient()` (JS) og `model.GetChatClientAsync()` (C#) som alternativer til OpenAI SDK
- **Del 2, Øvelse 8:** Modellvarianter og maskinvarevalg — `selectVariant()`, `variants`, NPU variant tabell (7 modeller)
- **Del 2, Øvelse 9:** Modelloppgraderinger og katalogoppdatering — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Del 2, Øvelse 10:** Resonering modeller — `phi-4-mini-reasoning` med `<think>` tag parsing-eksempler
- **Del 3, Øvelse 4:** `createChatClient` som alternativ til OpenAI SDK, med dokumentasjon på streaming callback-mønster
- **AGENTS.md:** La til Verktøy-kall, ChatClient og Resonering Modeller kodekonvensjoner

### Endret
- **Del 1:** Utvidet modellkatalog — la til phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Del 2:** Utvidet API-referansetabeller — la til `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Del 2:** Omdøpte øvelser 7-9 → 10-13 for å imøtekomme nye øvelser
- **Del 3:** Oppdatert Nøkkelpunkter-tabell for å inkludere native ChatClient
- **README.md:** Lagt til Del 11 seksjon med kodeeksempel-tabell; la til læringsmål #11; oppdatert prosjektstruktur-tre
- **csharp/Program.cs:** Lagt til `toolcall` case i CLI-ruter og oppdatert hjelpetekst

---

## 2026-03-09 — SDK v0.9.0 Oppdatering, Britisk engelsk og valideringsrunde

### Endret
- **Alle kodeeksempler (Python, JavaScript, C#):** Oppdatert til Foundry Local SDK v0.9.0 API — rettet `await catalog.getModel()` (manglet `await`), oppdaterte `FoundryLocalManager` init-mønstre, fikset endepunkt-oppdagelse
- **Alle lab-guider (Del 1-10):** Konvertert til britisk engelsk (colour, catalogue, optimised, osv.)
- **Alle lab-guider:** Oppdaterte SDK kodeeksempler til å matche v0.9.0 API-flate
- **Alle lab-guider:** Oppdaterte API-referansetabeller og øvelseskodeblokker
- **JavaScript kritisk fiks:** La til manglende `await` på `catalog.getModel()` — returnerte en `Promise` og ikke et `Model`-objekt, som forårsaket tause feil videre

### Validert
- Alle Python-eksempler kjører vellykket mot Foundry Local-tjenesten
- Alle JavaScript-eksempler kjører vellykket (Node.js 18+)
- C# prosjekt bygger og kjører på .NET 9.0 (fremoverkompatibel fra net8.0 SDK-assembly)
- 29 filer endret og validert gjennom hele workshopen

---

## Filindeks

| Fil | Sist oppdatert | Beskrivelse |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Utvidet modellkatalog |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nye øvelser 7-10, utvidede API-tabeller |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Ny øvelse 4 (ChatClient), oppdaterte nøkkelpunkter |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + britisk engelsk |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + britisk engelsk |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Britisk engelsk |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Britisk engelsk |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid-diagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Britisk engelsk |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid-diagram, flyttet Workshop Fullført til Del 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Nytt laboratorium, Mermaid-diagrammer, Workshop Fullført seksjon |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Nytt: eksempel på verktøyring |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Nytt: eksempel på verktøyring |
| `csharp/ToolCalling.cs` | 2026-03-10 | Nytt: eksempel på verktøyring |
| `csharp/Program.cs` | 2026-03-10 | Lagt til `toolcall` CLI-kommando |
| `README.md` | 2026-03-10 | Del 11, prosjektstruktur |
| `AGENTS.md` | 2026-03-10 | Verktøyring + ChatClient-konvensjoner |
| `KNOWN-ISSUES.md` | 2026-03-11 | Fjernet løst problem #7, 6 åpne problemer gjenstår |
| `csharp/csharp.csproj` | 2026-03-11 | Plattformuavhengig TFM, WinML/base SDK betingede referanser |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Plattformuavhengig TFM, automatisk oppdagelse av RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Plattformuavhengig TFM, automatisk oppdagelse av RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Fjernet NPU try/catch-løsning |
| `csharp/SingleAgent.cs` | 2026-03-11 | Fjernet NPU try/catch-løsning |
| `csharp/MultiAgent.cs` | 2026-03-11 | Fjernet NPU try/catch-løsning |
| `csharp/RagPipeline.cs` | 2026-03-11 | Fjernet NPU try/catch-løsning |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Fjernet NPU try/catch-løsning |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Plattformuavhengig .csproj-eksempel |
| `AGENTS.md` | 2026-03-11 | Oppdaterte C#-pakker og TFM-detaljer |
| `CHANGELOG.md` | 2026-03-11 | Denne filen |