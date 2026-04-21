# Változásnapló — Foundry Local Workshop

Minden jelentős változást az alábbiakban dokumentáltunk.

---

## 2026-03-11 — 12. és 13. rész, Web UI, Whisper újraírás, WinML/QNN javítás és ellenőrzés

### Hozzáadva
- **12. rész: Web UI építése a Zava Creative Writer-hez** — új labor útmutató (`labs/part12-zava-ui.md`) gyakorlatokkal, amelyek lefedik a streaming NDJSON-t, böngésző `ReadableStream`-et, az élő ügynök státusz jelzőket és valós idejű cikk szöveg streaminget
- **13. rész: Workshop befejezése** — új összefoglaló labor (`labs/part13-workshop-complete.md`), amelyben az összes 12 részt átnézzük, további ötletek és erőforrás linkek
- **Zava UI front end:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — közös vanilla HTML/CSS/JS böngészői felület, amit mindhárom backend használ
- **JavaScript HTTP szerver:** `zava-creative-writer-local/src/javascript/server.mjs` — új Express-stílusú HTTP szerver, amely az orchestrator-t böngésző alapú eléréshez burkolja
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` és `ZavaCreativeWriterWeb.csproj` — új minimális API projekt, amely szolgálja a UI-t és streameli az NDJSON-t
- **Audio minta generátor:** `samples/audio/generate_samples.py` — offline TTS szkript `pyttsx3` használatával, Zava témájú WAV fájlok generálásához a 9. részhez
- **Audio minta:** `samples/audio/zava-full-project-walkthrough.wav` — új hosszabb audio minta átíráshoz való teszteléshez
- **Ellenőrző szkript:** `validate-npu-workaround.ps1` — automatizált PowerShell szkript az NPU/QNN megoldás ellenőrzéséhez minden C# mintán végig
- **Mermaid diagram SVG-k:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML többplatformos támogatás:** Mindhárom C# `.csproj` fájl (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) most feltételes TFM-et és kizáró csomaghivatkozásokat használ a többplatformos támogatáshoz. Windows alatt: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (szuperszett, ami tartalmazza a QNN EP plugint). Nem-Windows alatt: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (alap SDK). Az előre megadott `win-arm64` RID-et Zava projektekben automatikus érzékelés váltotta fel. Egy átviteli függőség megoldás kizárja a natív eszközöket a `Microsoft.ML.OnnxRuntime.Gpu.Linux`-ból, amelyben hibás win-arm64 referencia van. A korábbi try/catch-es NPU megoldás eltávolításra került mind a 7 C# fájlból.

### Módosítva
- **9. rész (Whisper):** Jelentős újraírás — a JavaScript most már az SDK beépített `AudioClient`-jét használja (`model.createAudioClient()`) a kézi ONNX Runtime inferencia helyett; frissítve az architektúra leírások, összehasonlító táblázatok és pipeline diagramok, hogy tükrözzék a JS/C# `AudioClient` megközelítést a Python ONNX Runtime megközelítés helyett
- **11. rész:** Frissített navigációs linkek (most már a 12. részre mutatnak); SVG diagramok hozzáadva az eszköz-hívási folyamathoz és szekvenciához
- **10. rész:** Navigáció frissítve, hogy a 12. részen keresztül vezessen a workshop vége helyett
- **Python Whisper (`foundry-local-whisper.py`):** További audio mintákkal bővítve és hibakezelés fejlesztve
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Újraírásra került a `model.createAudioClient()` és `audioClient.transcribe()` használatával a kézi ONNX Runtime sessions helyett
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Frissítve, hogy statikus UI fájlokat is szolgáltasson az API mellett
- **Zava C# konzol (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU megoldás eltávolítva (most a WinML csomag kezeli)
- **README.md:** Hozzáadva a 12. rész szekciója kódminta táblázatokkal és backend kiegészítésekkel; hozzáadva 13. rész; frissítve tanulási célok és projekt struktúra
- **KNOWN-ISSUES.md:** Eltávolítva megoldott 7. probléma (C# SDK NPU modell változat — most a WinML csomag kezeli). Maradék problémák újraszámozva #1–#6-ig. Frissítve a környezeti részletek a .NET SDK 10.0.104-gyel
- **AGENTS.md:** Frissítve a projekt struktúra fája az új `zava-creative-writer-local` bejegyzésekkel (`ui/`, `csharp-web/`, `server.mjs`); frissítve C# kulcs csomagok és feltételes TFM részletek
- **labs/part2-foundry-local-sdk.md:** `.csproj` példa frissítve, hogy teljes többplatformos mintázatot mutasson feltételes TFM-mel, kizáró csomaghivatkozásokkal és magyarázó jegyzettel

### Ellenőrizve
- Mindhárom C# projekt (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) sikeresen buildel Windows ARM64 alatt
- Chat minta (`dotnet run chat`): modell betöltődik `phi-3.5-mini-instruct-qnn-npu:1` néven WinML/QNN segítségével — az NPU változat közvetlenül töltődik be CPU fallback nélkül
- Ügynök minta (`dotnet run agent`): végigfut többszörös beszélgetéssel, kilépési kód 0
- Foundry Local CLI v0.8.117 és SDK v0.9.0 .NET SDK 9.0.312 alatt

---

## 2026-03-11 — Kódjavítások, modell tisztítás, Mermaid diagramok és ellenőrzés

### Javítva
- **Mind a 21 kódminta (7 Python, 7 JavaScript, 7 C#):** Hozzáadva `model.unload()` / `unload_model()` / `model.UnloadAsync()` tisztítás kilépéskor az OGA memória szivárgás figyelmeztetések megoldására (Ismert probléma #4)
- **csharp/WhisperTranscription.cs:** A törékeny `AppContext.BaseDirectory` relatív útvonalat kicseréltük `FindSamplesDirectory()`-re, amely felfelé tallózva megbízhatóan megtalálja a `samples/audio` könyvtárat (Ismert probléma #7)
- **csharp/csharp.csproj:** Az előre megadott `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` helyére automatikus érzékelést használó fallback került `$(NETCoreSdkRuntimeIdentifier)`-rel, így a `dotnet run` bármely platformon működik `-r` flag nélkül ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Módosítva
- **8. rész:** Eredetileg ASCII-s dobozos ábrázolásról SVG képre váltás az értékelésvezérelt iterációs ciklusnál
- **10. rész:** Fordítási pipeline diagram ASCII nyilakról SVG képre váltás
- **11. rész:** Eszköz-hívási folyamat és sorrendi diagramok SVG képekké konvertálva
- **10. rész:** "Workshop befejezve!" rész áthelyezve a 11. részbe (utolsó labor); helyette "Következő lépések" link
- **KNOWN-ISSUES.md:** Teljes újraellenőrzés a CLI v0.8.117-re. Megoldott problémák törölve: OGA memória szivárgás (tisztítás hozzáadva), Whisper elérési út (FindSamplesDirectory), HTTP 500 tartós inferencia (nem reprodukálható, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice korlátok (most működik `"required"`-vel és konkrét funkció célzással a qwen2.5-0.5b-n). Frissült JS Whisper probléma — most minden fájl üres/bináris kimenetet ad (regresszió a v0.9.x-hez képest, súlyosság Major-ra emelve). Frissült #4 C# RID automatikus érzékeléssel valamint [#497](https://github.com/microsoft/Foundry-Local/issues/497) linkkel. 7 nyitott probléma maradt.
- **javascript/foundry-local-whisper.mjs:** Javítva tisztító változó neve (`whisperModel` → `model`)

### Ellenőrizve
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — sikeres futtatás tisztítással
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — sikeres futtatás tisztítással
- C#: `dotnet build` sikeres 0 figyelmeztetéssel, 0 hibával (net9.0 cél platform)
- Mind a 7 Python fájl átmegy `py_compile` szintaktikai ellenőrzésen
- Mind a 7 JavaScript fájl átmegy a `node --check` szintaktikai ellenőrzésen

---

## 2026-03-10 — 11. rész: Eszköz-hívás, SDK API bővítés és modell lefedés

### Hozzáadva
- **11. rész: Eszköz-hívás helyi modellekkel** — új labor útmutató (`labs/part11-tool-calling.md`) 8 gyakorlatával, amelyek lefedik az eszköz sémákat, többturnusú folyamatot, több eszköz hívást, egyéni eszközöket, ChatClient eszköz-hívást és `tool_choice`-t
- **Python minta:** `python/foundry-local-tool-calling.py` — eszköz-hívás `get_weather`/`get_population` eszközökkel az OpenAI SDK segítségével
- **JavaScript minta:** `javascript/foundry-local-tool-calling.mjs` — eszköz-hívás az SDK natív `ChatClient`-jével (`model.createChatClient()`)
- **C# minta:** `csharp/ToolCalling.cs` — eszköz-hívás `ChatTool.CreateFunctionTool()`-lal az OpenAI C# SDK-val
- **2. rész, 7. gyakorlat:** Natív `ChatClient` — `model.createChatClient()` (JS) és `model.GetChatClientAsync()` (C#) alternatívák az OpenAI SDK-hoz képest
- **2. rész, 8. gyakorlat:** Modell változatok és hardver választás — `selectVariant()`, `variants`, NPU változat táblázat (7 modell)
- **2. rész, 9. gyakorlat:** Modell frissítések és katalógus frissítés — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **2. rész, 10. gyakorlat:** Érvelő modellek — `phi-4-mini-reasoning` `<think>` címke elemzési példákkal
- **3. rész, 4. gyakorlat:** `createChatClient` alternatíva az OpenAI SDK-hoz, streames visszahívás minta dokumentációval
- **AGENTS.md:** Hozzáadva az Eszköz-hívás, ChatClient és Érvelő Modellek kódolási konvenciói

### Módosítva
- **1. rész:** Bővített modell katalógus — hozzáadva phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **2. rész:** Bővített API referencia táblázatok — hozzáadva `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **2. rész:** A 7-9. gyakorlatokat átnevezve 10-13-ra az új gyakorlatok miatt
- **3. rész:** Frissítve a Főbb Tanulságok táblázata, hogy tartalmazza a natív ChatClient-et
- **README.md:** Hozzáadva 11. rész szakasz kódminta táblázattal; hozzáadva a 11. tanulási cél; frissítve projekt struktúra fa
- **csharp/Program.cs:** Hozzáadva `toolcall` eset a CLI routerhez és frissített segítség szöveg

---

## 2026-03-09 — SDK v0.9.0 frissítés, brit angol és ellenőrzés

### Módosítva
- **Minden kódminta (Python, JavaScript, C#):** Frissítve Foundry Local SDK v0.9.0 API-hoz — javítva `await catalog.getModel()` (hiányzott az await), frissítve a `FoundryLocalManager` inicializációs minták, javítva végpont felfedezés
- **Minden labor útmutató (1–10. rész):** Átállás brit angolra (colour, catalogue, optimised, stb.)
- **Minden labor útmutató:** Frissítve SDK kódpéldák v0.9.0 API felülethez
- **Minden labor útmutató:** Frissítve API referencia táblák és gyakorlat kódblokkok
- **JavaScript kritikus javítás:** Hiányzó `await` hozzáadása a `catalog.getModel()`-hez — a visszatérési érték Promise volt nem Model objektum, ami hangtalan hibákat okozott később

### Ellenőrizve
- Minden Python minta sikeresen fut a Foundry Local szolgáltatás ellen
- Minden JavaScript minta sikeresen fut (Node.js 18+)
- C# projekt buildel és fut .NET 9.0 alatt (forward-compat a net8.0 SDK assembllyvel)
- 29 fájl módosítva és ellenőrizve a workshop során

---

## Fájlindex

| Fájl | Utolsó frissítés | Leírás |
|------|------------------|---------|
| `labs/part1-getting-started.md` | 2026-03-10 | Bővített modell katalógus |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Új gyakorlatok 7–10, bővített API táblák |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Új 4. gyakorlat (ChatClient), frissített tanulságok |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + brit angol |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + brit angol |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + brit angol |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + brit angol |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + brit angol |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diagram, a Workshop Complete átmozgatva a 11. részhez |
| `labs/part11-tool-calling.md` | 2026-03-11 | Új labor, Mermaid diagramok, Workshop Complete szakasz |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Új: eszköz hívási példa |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Új: eszköz hívási példa |
| `csharp/ToolCalling.cs` | 2026-03-10 | Új: eszköz hívási példa |
| `csharp/Program.cs` | 2026-03-10 | Hozzáadva `toolcall` parancssori parancs |
| `README.md` | 2026-03-10 | 11. rész, projekt struktúra |
| `AGENTS.md` | 2026-03-10 | Eszköz hívás + ChatClient konvenciók |
| `KNOWN-ISSUES.md` | 2026-03-11 | Eltávolítva a megoldott 7-es probléma, 6 nyitott probléma maradt |
| `csharp/csharp.csproj` | 2026-03-11 | Keresztplatformos TFM, WinML/alap SDK feltételes hivatkozások |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Keresztplatformos TFM, automatikus RID felismerés |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Keresztplatformos TFM, automatikus RID felismerés |
| `csharp/BasicChat.cs` | 2026-03-11 | Eltávolítva az NPU try/catch megoldás |
| `csharp/SingleAgent.cs` | 2026-03-11 | Eltávolítva az NPU try/catch megoldás |
| `csharp/MultiAgent.cs` | 2026-03-11 | Eltávolítva az NPU try/catch megoldás |
| `csharp/RagPipeline.cs` | 2026-03-11 | Eltávolítva az NPU try/catch megoldás |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Eltávolítva az NPU try/catch megoldás |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Keresztplatformos .csproj példa |
| `AGENTS.md` | 2026-03-11 | Frissített C# csomagok és TFM részletek |
| `CHANGELOG.md` | 2026-03-11 | Ez a fájl |