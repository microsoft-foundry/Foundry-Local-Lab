# Changelog — Foundry Local Workshop

Tutte le modifiche rilevanti a questo workshop sono documentate di seguito.

---

## 2026-03-11 — Parte 12 e 13, UI Web, Riscrittura Whisper, Correzione WinML/QNN e Validazione

### Aggiunto
- **Parte 12: Costruire una UI Web per lo Zava Creative Writer** — nuova guida laboratorio (`labs/part12-zava-ui.md`) con esercizi che coprono streaming NDJSON, browser `ReadableStream`, badge di stato agente live e streaming in tempo reale del testo dell’articolo
- **Parte 13: Workshop Completo** — nuovo laboratorio riassuntivo (`labs/part13-workshop-complete.md`) con riepilogo di tutte le 12 parti, ulteriori idee e link a risorse
- **Frontend UI Zava:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — interfaccia browser condivisa vanilla HTML/CSS/JS consumata da tutti e tre i backend
- **Server HTTP JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — nuovo server HTTP stile Express che incapsula l’orchestratore per l’accesso basato su browser
- **Backend C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` e `ZavaCreativeWriterWeb.csproj` — nuovo progetto API minimale che serve la UI e lo streaming NDJSON
- **Generatore di campioni audio:** `samples/audio/generate_samples.py` — script TTS offline che usa `pyttsx3` per generare file WAV a tema Zava per la Parte 9
- **Campione audio:** `samples/audio/zava-full-project-walkthrough.wav` — nuovo campione audio più lungo per test di trascrizione
- **Script di validazione:** `validate-npu-workaround.ps1` — script PowerShell automatizzato per validare la soluzione alternativa NPU/QNN su tutti i campioni C#
- **Diagrammi Mermaid SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Supporto WinML cross-platform:** Tutti e 3 i file `.csproj` C# (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) usano ora TFM condizionale e riferimenti pacchetto mutuamente esclusivi per supporto cross-platform. Su Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (superset che include plugin QNN EP). Su non-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK base). Il RID `win-arm64` codificato nei progetti Zava è stato sostituito da rilevamento automatico. Una soluzione alternativa di dipendenza transitiva esclude asset nativi da `Microsoft.ML.OnnxRuntime.Gpu.Linux` che ha un riferimento rotto win-arm64. La precedente soluzione try/catch NPU è stata rimossa da tutti i 7 file C#.

### Modificato
- **Parte 9 (Whisper):** Riscrittura radicale — JavaScript ora usa `AudioClient` integrato del SDK (`model.createAudioClient()`) invece di inferenza manuale ONNX Runtime; descrizioni architetturali aggiornate, tabelle di confronto e diagrammi pipeline per riflettere approccio JS/C# `AudioClient` vs approccio Python ONNX Runtime
- **Parte 11:** Aggiornati link di navigazione (ora puntano alla Parte 12); aggiunti diagrammi SVG renderizzati per il flusso e sequenza di chiamata strumenti
- **Parte 10:** Aggiornata navigazione per passare tramite la Parte 12 invece di terminare il workshop
- **Python Whisper (`foundry-local-whisper.py`):** Espanso con ulteriori campioni audio e migliorata gestione errori
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Riscritto per usare `model.createAudioClient()` con `audioClient.transcribe()` invece di sessioni ONNX Runtime manuali
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Aggiornato per servire file UI statici insieme all’API
- **Console Zava C# (`zava-creative-writer-local/src/csharp/Program.cs`):** Rimossa soluzione alternativa NPU (ora gestita dal pacchetto WinML)
- **README.md:** Aggiunta sezione Parte 12 con tabelle esempi codice e aggiunte backend; aggiunta sezione Parte 13; aggiornati obiettivi di apprendimento e struttura progetto
- **KNOWN-ISSUES.md:** Rimossa Issue #7 risolta (Variante Modello NPU SDK C# — ora gestita dal pacchetto WinML). Rinumero restanti issue a #1–#6. Aggiornati dettagli ambiente con .NET SDK 10.0.104
- **AGENTS.md:** Aggiornato albero struttura progetto con nuove voci `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); aggiornati pacchetti chiave C# e dettagli TFM condizionali
- **labs/part2-foundry-local-sdk.md:** Aggiornato esempio `.csproj` per mostrare pattern completo cross-platform con TFM condizionale, riferimenti pacchetto mutuamente esclusivi e nota esplicativa

### Validato
- Tutti e 3 i progetti C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) compilano con successo su Windows ARM64
- Esempio chat (`dotnet run chat`): modello carica come `phi-3.5-mini-instruct-qnn-npu:1` via WinML/QNN — variante NPU carica direttamente senza fallback CPU
- Esempio agente (`dotnet run agent`): esegue end-to-end con conversazione multi-turno, codice uscita 0
- Foundry Local CLI v0.8.117 e SDK v0.9.0 su .NET SDK 9.0.312

---

## 2026-03-11 — Correzioni codice, Pulizia modello, Diagrammi Mermaid e Validazione

### Corretto
- **Tutti i 21 esempi codice (7 Python, 7 JavaScript, 7 C#):** Aggiunti `model.unload()` / `unload_model()` / `model.UnloadAsync()` alla chiusura per risolvere avvisi di perdita memoria OGA (Known Issue #4)
- **csharp/WhisperTranscription.cs:** Sostituito percorso fragile relativo `AppContext.BaseDirectory` con `FindSamplesDirectory()` che risale le directory per trovare `samples/audio` in modo affidabile (Known Issue #7)
- **csharp/csharp.csproj:** Sostituito `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` hardcoded con fallback insieme a rilevamento auto usando `$(NETCoreSdkRuntimeIdentifier)` così `dotnet run` funziona su qualsiasi piattaforma senza flag `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Modificato
- **Parte 8:** Convertito ciclo di iterazione guidato da eval da diagramma box ASCII a immagine SVG renderizzata
- **Parte 10:** Convertito diagramma pipeline di compilazione da frecce ASCII a immagine SVG renderizzata
- **Parte 11:** Convertiti diagrammi flusso chiamata strumenti e sequenza in immagini SVG renderizzate
- **Parte 10:** Sezione "Workshop Completo!" spostata alla Parte 11 (ultimo laboratorio); sostituita con link "Prossimi Passi"
- **KNOWN-ISSUES.md:** Riconvalida completa di tutte le issue rispetto a CLI v0.8.117. Rimosse risolte: Perdita memoria OGA (cleanup aggiunto), percorso Whisper (FindSamplesDirectory), inferenza sostenuta HTTP 500 (non riproducibile, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), limitazioni tool_choice (ora funziona con `"required"` e targeting funzionale specifico su qwen2.5-0.5b). Aggiornata issue JS Whisper — ora tutti i file restituiscono output vuoto/binario (regressione da v0.9.x, severità alzata a Maggiore). Aggiornato #4 C# RID con workaround auto-detect e link [#497](https://github.com/microsoft/Foundry-Local/issues/497). Restano 7 issue aperte.
- **javascript/foundry-local-whisper.mjs:** Corretto nome variabile cleanup (`whisperModel` → `model`)

### Validato
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — esecuzioni con cleanup riuscite
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — esecuzioni con cleanup riuscite
- C#: `dotnet build` riesce senza avvisi, senza errori (target net9.0)
- Tutti i 7 file Python passano controllo sintassi `py_compile`
- Tutti i 7 file JavaScript passano validazione sintassi `node --check`

---

## 2026-03-10 — Parte 11: Chiamata Strumenti, Espansione API SDK e Copertura Modello

### Aggiunto
- **Parte 11: Chiamata Strumenti con Modelli Locali** — nuova guida laboratorio (`labs/part11-tool-calling.md`) con 8 esercizi su schemi di strumenti, flusso multi-turno, chiamate multiple di strumenti, strumenti personalizzati, chiamata strumenti con ChatClient e `tool_choice`
- **Esempio Python:** `python/foundry-local-tool-calling.py` — chiamata strumenti con tool `get_weather`/`get_population` usando OpenAI SDK
- **Esempio JavaScript:** `javascript/foundry-local-tool-calling.mjs` — chiamata strumenti con `ChatClient` nativo SDK (`model.createChatClient()`)
- **Esempio C#:** `csharp/ToolCalling.cs` — chiamata strumenti usando `ChatTool.CreateFunctionTool()` con SDK OpenAI C#
- **Parte 2, Esercizio 7:** ChatClient nativo — `model.createChatClient()` (JS) e `model.GetChatClientAsync()` (C#) come alternative all’OpenAI SDK
- **Parte 2, Esercizio 8:** Varianti modello e selezione hardware — `selectVariant()`, `variants`, tabella varianti NPU (7 modelli)
- **Parte 2, Esercizio 9:** Aggiornamenti modello e refresh catalogo — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Parte 2, Esercizio 10:** Modelli di ragionamento — `phi-4-mini-reasoning` con esempi di parsing tag `<think>`
- **Parte 3, Esercizio 4:** `createChatClient` come alternativa all’OpenAI SDK, con documentazione pattern callback streaming
- **AGENTS.md:** Aggiunte convenzioni di coding su Chiamata Strumenti, ChatClient e Modelli di Ragionamento

### Modificato
- **Parte 1:** Catalogo modello ampliato — aggiunti phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Parte 2:** Tabelle di riferimento API ampliate — aggiunti `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Parte 2:** Esercizi rinumerati 7-9 → 10-13 per accogliere nuovi esercizi
- **Parte 3:** Tabella dei punti chiave aggiornata per includere ChatClient nativo
- **README.md:** Aggiunta sezione Parte 11 con tabella esempi codice; aggiunto obiettivo apprendimento #11; aggiornato albero struttura progetto
- **csharp/Program.cs:** Aggiunto caso `toolcall` al router CLI e aggiornata guida help

---

## 2026-03-09 — Aggiornamento SDK v0.9.0, Inglese Britannico e Pass di Validazione

### Modificato
- **Tutti gli esempi codice (Python, JavaScript, C#):** Aggiornati alla API Foundry Local SDK v0.9.0 — corretto `await catalog.getModel()` (mancava `await`), aggiornati pattern init `FoundryLocalManager`, corretto discovery endpoint
- **Tutte le guide laboratorio (Parti 1-10):** Tradotte in inglese britannico (colour, catalogue, optimised, ecc.)
- **Tutte le guide laboratorio:** Aggiornati esempi codice SDK per riflettere API v0.9.0
- **Tutte le guide laboratorio:** Aggiornate tabelle di riferimento API e blocchi codice esercizi
- **Correzione critica JavaScript:** Aggiunto `await` mancante su `catalog.getModel()` — ritornava un `Promise` e non un oggetto `Model`, causando fallimenti silenziosi a valle

### Validato
- Tutti gli esempi Python eseguiti con successo contro servizio Foundry Local
- Tutti gli esempi JavaScript eseguiti con successo (Node.js 18+)
- Progetto C# compila ed esegue su .NET 9.0 (compatibilità avanti da net8.0 SDK assembly)
- 29 file modificati e convalidati in tutto il workshop

---

## Indice File

| File | Ultimo Aggiornamento | Descrizione |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Catalogo modello ampliato |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nuovi esercizi 7-10, tabelle API ampliate |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Nuovo Esercizio 4 (ChatClient), punti chiave aggiornati |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + inglese britannico |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + inglese britannico |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Inglese britannico |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Inglese britannico |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Diagramma Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Inglese britannico |
| `labs/part10-custom-models.md` | 2026-03-11 | Diagramma Mermaid, Workshop Completo spostato alla Parte 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Nuovo laboratorio, diagrammi Mermaid, sezione Workshop Completo |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Nuovo: esempio di chiamata a tool |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Nuovo: esempio di chiamata a tool |
| `csharp/ToolCalling.cs` | 2026-03-10 | Nuovo: esempio di chiamata a tool |
| `csharp/Program.cs` | 2026-03-10 | Aggiunto comando CLI `toolcall` |
| `README.md` | 2026-03-10 | Parte 11, struttura del progetto |
| `AGENTS.md` | 2026-03-10 | Chiamata a tool + convenzioni ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Rimossa issue risolta #7, rimangono 6 problemi aperti |
| `csharp/csharp.csproj` | 2026-03-11 | TFM multipiattaforma, riferimenti condizionali WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM multipiattaforma, rilevamento automatico RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM multipiattaforma, rilevamento automatico RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Rimosso workaround try/catch NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Rimosso workaround try/catch NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Rimosso workaround try/catch NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Rimosso workaround try/catch NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Rimosso workaround try/catch NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Esempio .csproj multipiattaforma |
| `AGENTS.md` | 2026-03-11 | Aggiornati dettagli pacchetti C# e TFM |
| `CHANGELOG.md` | 2026-03-11 | Questo file |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:
Questo documento è stato tradotto utilizzando il servizio di traduzione AI [Co-op Translator](https://github.com/Azure/co-op-translator). Sebbene ci impegniamo per l'accuratezza, si prega di essere consapevoli che le traduzioni automatizzate possono contenere errori o imprecisioni. Il documento originale nella sua lingua nativa deve essere considerato la fonte autorevole. Per informazioni critiche, si raccomanda la traduzione professionale umana. Non siamo responsabili per eventuali incomprensioni o interpretazioni errate derivanti dall'uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->