# Changelog — Foundry Local Workshop

Toate modificările notabile ale acestui atelier sunt documentate mai jos.

---

## 2026-03-11 — Partea 12 & 13, Interfață Web, Rescriere Whisper, Corecție WinML/QNN și Validare

### Adăugat
- **Partea 12: Construirea unei Interfețe Web pentru Zava Creative Writer** — nou ghid de laborator (`labs/part12-zava-ui.md`) cu exerciții care acoperă streaming NDJSON, `ReadableStream` în browser, insigne de stare live pentru agenți și streaming în timp real al textului articolului
- **Partea 13: Atelier Finalizat** — nou laborator sumar (`labs/part13-workshop-complete.md`) cu o recapitulare a tuturor celor 12 părți, idei suplimentare și linkuri spre resurse
- **Interfața UI Zava:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — interfață comună simplă HTML/CSS/JS folosită de toate cele trei backend-uri
- **Server HTTP JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — nou server HTTP în stil Express ce înfășoară orchestratorul pentru acces browser
- **Backend C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` și `ZavaCreativeWriterWeb.csproj` — nou proiect API minimal care servește UI-ul și streaming NDJSON
- **Generator de mostre audio:** `samples/audio/generate_samples.py` — script TTS offline folosind `pyttsx3` pentru generarea fișierelor WAV tematizate Zava pentru Partea 9
- **Mostră audio:** `samples/audio/zava-full-project-walkthrough.wav` — mostre audio noi, mai lungi, pentru testarea transcripției
- **Script de validare:** `validate-npu-workaround.ps1` — script automat PowerShell pentru validarea soluției de ocolire NPU/QNN în toate mostrele C#
- **Diagrame Mermaid SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Suport WinML cross-platform:** Toate cele 3 fișiere `.csproj` C# (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) folosesc acum TFM condițional și referințe mutual exclusive pentru suport cross-platform. Pe Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (superset ce include pluginul QNN EP). Pe non-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK de bază). RID hardcodat `win-arm64` din proiectele Zava a fost înlocuit cu autodetectare. O soluție de ocolire transituă exclude active native din `Microsoft.ML.OnnxRuntime.Gpu.Linux` care conține o referință defectă la win-arm64. Soluția try/catch NPU anterioară a fost eliminată din toate cele 7 fișiere C#.

### Modificat
- **Partea 9 (Whisper):** Rescriere majoră — JavaScript folosește acum `AudioClient` încorporat în SDK (`model.createAudioClient()`) în locul inferenței manuale ONNX Runtime; descrieri arhitecturale, tabele de comparație și diagrame pipeline actualizate pentru a reflecta abordarea JS/C# `AudioClient` versus Python ONNX Runtime
- **Partea 11:** Legături de navigare actualizate (acum indică Partea 12); adăugate diagrame SVG generate pentru fluxul de apelare a uneltelor și secvență
- **Partea 10:** Navigare actualizată să redirecționeze prin Partea 12 în loc să încheie atelierul
- **Python Whisper (`foundry-local-whisper.py`):** Extins cu mostre audio suplimentare și gestionare de erori îmbunătățită
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Rescris să folosească `model.createAudioClient()` cu `audioClient.transcribe()` în loc de sesiuni ONNX Runtime manuale
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Actualizat pentru a servi fișiere UI statice împreună cu API-ul
- **Zava C# consolă (`zava-creative-writer-local/src/csharp/Program.cs`):** Eliminată soluția de ocolire NPU (acum gestionată de pachetul WinML)
- **README.md:** Adăugată secțiunea Partea 12 cu tabele de mostre de cod și adăugiri backend; adăugată secțiunea Partea 13; actualizate obiectivele de învățare și structura proiectului
- **KNOWN-ISSUES.md:** Eliminată problema #7 rezolvată (variantă model SDK C# NPU — acum gestionată de pachetul WinML). Problemele rămase reindexate #1–#6. Actualizate detaliile mediului cu .NET SDK 10.0.104
- **AGENTS.md:** Actualizat arborele structurii proiectului cu noi intrări `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); actualizate pachetele cheie C# și detalii TFM condiționale
- **labs/part2-foundry-local-sdk.md:** Exemplul `.csproj` actualizat pentru a arăta modelul complet cross-platform cu TFM condițional, referințe mutual exclusive și notă explicativă

### Validat
- Toate cele 3 proiecte C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) se compilează cu succes pe Windows ARM64
- Mostră chat (`dotnet run chat`): modelul se încarcă ca `phi-3.5-mini-instruct-qnn-npu:1` prin WinML/QNN — varianta NPU se încarcă direct fără fallback CPU
- Mostră agent (`dotnet run agent`): rulează complet cu conversație multi-turn, cod de ieșire 0
- Foundry Local CLI v0.8.117 și SDK v0.9.0 pe .NET SDK 9.0.312

---

## 2026-03-11 — Corecții de Cod, Curățare Model, Diagrame Mermaid și Validare

### Corectat
- **Toate cele 21 mostre de cod (7 Python, 7 JavaScript, 7 C#):** Adăugat `model.unload()` / `unload_model()` / `model.UnloadAsync()` pentru curățare la ieșire pentru a rezolva avertismentele de scurgere de memorie OGA (Problemă cunoscută #4)
- **csharp/WhisperTranscription.cs:** Înlocuit calea relativă fragilă `AppContext.BaseDirectory` cu `FindSamplesDirectory()` care urcă în directoare pentru a localiza `samples/audio` în mod fiabil (Problemă cunoscută #7)
- **csharp/csharp.csproj:** Înlocuit `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` hardcodat cu fallback autodetectare folosind `$(NETCoreSdkRuntimeIdentifier)` astfel încât `dotnet run` să funcționeze pe orice platformă fără argumentul `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Modificat
- **Partea 8:** Buclă iterativă condusă de evaluare transformată din diagramă ASCII într-o imagine SVG generată
- **Partea 10:** Diagrama pipeline-ului de compilare convertită de la săgeți ASCII la imagine SVG generată
- **Partea 11:** Diagramele fluxului de apel a uneltelor și secvenței convertite în imagini SVG generate
- **Partea 10:** Secțiunea „Atelier Finalizat!” mutată la Partea 11 (ultimul laborator); înlocuită cu link „Următorii Pași”
- **KNOWN-ISSUES.md:** Revalidare completă pentru toate problemele cu CLI v0.8.117. Eliminat elementele rezolvate: scurgere de memorie OGA (curățare adăugată), calea Whisper (FindSamplesDirectory), inferență HTTP 500 sustenabilă (nereproductibilă, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), limitări `tool_choice` (acum funcționează cu `"required"` și țintire funcție specifică pe qwen2.5-0.5b). Actualizată problema JS Whisper — acum toate fișierele returnează output gol/binare (regresie din v0.9.x, severitate ridicată la Major). Actualizat RID C# #4 cu workaround autodetectare și link [#497](https://github.com/microsoft/Foundry-Local/issues/497). Rămân 7 probleme deschise.
- **javascript/foundry-local-whisper.mjs:** Corectat numele variabilei de curățare (`whisperModel` → `model`)

### Validat
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — rulează cu succes cu curățare
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — rulează cu succes cu curățare
- C#: `dotnet build` reușește cu 0 avertismente, 0 erori (target net9.0)
- Toate cele 7 fișiere Python trec verificarea sintaxei `py_compile`
- Toate cele 7 fișiere JavaScript trec validarea sintaxei `node --check`

---

## 2026-03-10 — Partea 11: Apelarea uneltelor, Extinderea API SDK și Acoperirea Modellor

### Adăugat
- **Partea 11: Apelarea uneltelor cu modelele locale** — nou ghid de laborator (`labs/part11-tool-calling.md`) cu 8 exerciții ce acoperă scheme de unelte, flux multi-turn, apeluri multiple de unelte, unelte personalizate, apelarea cu ChatClient și `tool_choice`
- **Mostră Python:** `python/foundry-local-tool-calling.py` — apelare unelte cu uneltele `get_weather`/`get_population` folosind SDK OpenAI
- **Mostră JavaScript:** `javascript/foundry-local-tool-calling.mjs` — apelare unelte folosind nativ `ChatClient` al SDK (`model.createChatClient()`)
- **Mostră C#:** `csharp/ToolCalling.cs` — apelare unelte folosind `ChatTool.CreateFunctionTool()` cu SDK-ul OpenAI C#
- **Partea 2, Exercițiul 7:** `ChatClient` nativ — `model.createChatClient()` (JS) și `model.GetChatClientAsync()` (C#) ca alternative la SDK OpenAI
- **Partea 2, Exercițiul 8:** Variante de modele și selecție hardware — `selectVariant()`, `variants`, tabel variantă NPU (7 modele)
- **Partea 2, Exercițiul 9:** Upgrade-uri de modele și reîmprospătare catalog — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Partea 2, Exercițiul 10:** Modele de raționament — `phi-4-mini-reasoning` cu exemple de parsare tag `<think>`
- **Partea 3, Exercițiul 4:** `createChatClient` ca alternativă la SDK OpenAI, cu documentație pentru modelul callback streaming
- **AGENTS.md:** Adăugate convenții de codare pentru Apelarea uneltelor, ChatClient și Modele de raționament

### Modificat
- **Partea 1:** Catalogul modelelor extins — adăugat phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Partea 2:** Tabelele de referință API extinse — adăugat `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Partea 2:** Exercițiile 7-9 renumerotate în 10-13 pentru a include noile exerciții
- **Partea 3:** Tabelul punctelor-cheie actualizat pentru a include ChatClient nativ
- **README.md:** Adăugată secțiunea Partea 11 cu tabel mostre de cod; adăugat obiectiv de învățare #11; structură de proiect actualizată
- **csharp/Program.cs:** Adăugat caz `toolcall` la routerul CLI și actualizat textul de ajutor

---

## 2026-03-09 — Actualizare SDK v0.9.0, Engleză Britanică și Validare

### Modificat
- **Toate mostrele de cod (Python, JavaScript, C#):** Actualizate la API Foundry Local SDK v0.9.0 — corectat `await catalog.getModel()` (lipsă `await`), actualizate pattern-urile de inițializare `FoundryLocalManager`, corectată descoperirea endpoint-urilor
- **Toate ghidurile de laborator (Părțile 1-10):** Convertite în engleză britanică (colour, catalogue, optimised etc.)
- **Toate ghidurile de laborator:** Actualizate exemple API SDK pentru a corespunde suprafeței API v0.9.0
- **Toate ghidurile de laborator:** Actualizate tabele de referință API și blocuri de cod pentru exerciții
- **Corecție critică JavaScript:** Adăugat `await` lipsă la `catalog.getModel()` — returna un `Promise` nu un obiect `Model`, cauzând erori tăcute downstream

### Validat
- Toate mostrele Python rulează cu succes cu serviciul Foundry Local
- Toate mostrele JavaScript rulează cu succes (Node.js 18+)
- Proiectul C# se compilează și rulează pe .NET 9.0 (compatibilitate înainte din net8.0 SDK assembly)
- 29 fișiere modificate și validate prin tot atelierul

---

## Index Fișiere

| Fișier | Ultima Actualizare | Descriere |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | Catalog extins de modele |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Exerciții noi 7-10, tabele API extinse |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Exercițiul 4 nou (ChatClient), puncte-cheie actualizate |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + engleză britanică |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + engleză britanică |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + Engleză britanică |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + Engleză britanică |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Diagramă Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + Engleză britanică |
| `labs/part10-custom-models.md` | 2026-03-11 | Diagramă Mermaid, mutat Workshop Complete la Partea 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Laborator nou, diagrame Mermaid, secțiunea Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Nou: exemplu de apelare unelte |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Nou: exemplu de apelare unelte |
| `csharp/ToolCalling.cs` | 2026-03-10 | Nou: exemplu de apelare unelte |
| `csharp/Program.cs` | 2026-03-10 | Comandă CLI `toolcall` adăugată |
| `README.md` | 2026-03-10 | Partea 11, structura proiectului |
| `AGENTS.md` | 2026-03-10 | Apelare unelte + convenții ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Eliminat problema rezolvată #7, mai rămân 6 probleme deschise |
| `csharp/csharp.csproj` | 2026-03-11 | TFM cross-platform, referințe condiționale WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM cross-platform, detectare automată RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM cross-platform, detectare automată RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Eliminat workaround try/catch NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Eliminat workaround try/catch NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Eliminat workaround try/catch NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Eliminat workaround try/catch NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Eliminat workaround try/catch NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Exemplu .csproj cross-platform |
| `AGENTS.md` | 2026-03-11 | Actualizate detalii pachete C# și TFM |
| `CHANGELOG.md` | 2026-03-11 | Acest fișier |