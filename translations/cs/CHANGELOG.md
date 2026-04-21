# Změny — Foundry Local Workshop

Veškeré významné změny tohoto workshopu jsou zdokumentovány níže.

---

## 2026-03-11 — Část 12 & 13, Webové UI, Přepis Whisperu, Oprava WinML/QNN a Validace

### Přidáno
- **Část 12: Tvorba webového UI pro Zava Creative Writer** — nový průvodce („labs/part12-zava-ui.md“) s cvičeními pokrývajícími streamování NDJSON, browser `ReadableStream`, odznaky stavu agenta v reálném čase a streamování textu článku v reálném čase
- **Část 13: Workshop dokončen** — nový shrnující lab („labs/part13-workshop-complete.md“) s rekapitulací všech 12 částí, dalšími nápady a odkazy na zdroje
- **Zava UI front end:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — sdílené vanilla HTML/CSS/JS rozhraní v prohlížeči využívané všemi třemi backendy
- **JavaScript HTTP server:** `zava-creative-writer-local/src/javascript/server.mjs` — nový Express-style HTTP server obalující orchestrátor pro přístup v prohlížeči
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` a `ZavaCreativeWriterWeb.csproj` — nový minimální API projekt pro poskytování UI a streamování NDJSON
- **Generátor audio vzorků:** `samples/audio/generate_samples.py` — offline TTS skript využívající `pyttsx3` generující WAV soubory na téma Zava pro Část 9
- **Audio vzorek:** `samples/audio/zava-full-project-walkthrough.wav` — nový delší audio vzorek pro testování přepisu
- **Validace skriptu:** `validate-npu-workaround.ps1` — automatizovaný PowerShell skript pro validaci workaroundu NPU/QNN ve všech C# příkladech
- **SVG Mermaid diagramy:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Podpora WinML multiplatformy:** Všechny 3 C# `.csproj` soubory (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) nyní používají podmíněné TFM a vzájemně vyhrazené odkazy na balíčky pro multiplatformní podporu. Na Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (superset zahrnující plugin QNN EP). Na non-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (základní SDK). Hardcoded RID `win-arm64` v projektech Zava byl nahrazen automatickou detekcí. Workaround tranzitivní závislosti vyloučil nativní aktiva z `Microsoft.ML.OnnxRuntime.Gpu.Linux`, které mělo rozbité odkazy pro win-arm64. Předchozí try/catch workaround pro NPU byl odstraněn ze všech 7 C# souborů.

### Změněno
- **Část 9 (Whisper):** Hlavní přepis — JavaScript nyní používá vestavěný SDK `AudioClient` (`model.createAudioClient()`) místo manuální inference ONNX Runtime; aktualizovány popisy architektury, srovnávací tabulky a diagramy pipeline odrážející přístup JS/C# `AudioClient` oproti Python ONNX Runtime
- **Část 11:** Aktualizovány navigační odkazy (nyní směřují na Část 12); přidány vykreslené SVG diagramy průběhu volání nástrojů a sekvence
- **Část 10:** Aktualizována navigace, nyní vede přes Část 12 místo konce workshopu
- **Python Whisper (`foundry-local-whisper.py`):** Rozšířeno o další audio vzorky a zlepšená obsluha chyb
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Přepsáno na použití `model.createAudioClient()` s `audioClient.transcribe()` místo manuálních ONNX Runtime session
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Aktualizováno pro podávání statických UI souborů vedle API
- **Zava C# konzole (`zava-creative-writer-local/src/csharp/Program.cs`):** Odstraněn workaround NPU (nyní ošetřeno balíčkem WinML)
- **README.md:** Přidána sekce Část 12 s tabulkami příkladů kódu a doplnění backendu; přidána sekce Část 13; aktualizovány cíle učení a struktura projektu
- **KNOWN-ISSUES.md:** Odstraněn vyřešený problém #7 (C# SDK NPU Model Variant — nyní ošetřeno WinML balíčkem). Přesunuty zbývající problémy na čísla #1–#6. Aktualizace detailů prostředí s .NET SDK 10.0.104
- **AGENTS.md:** Aktualizována struktura projektu s novými položkami `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); aktualizováno klíčové C# balíčky a detaily podmíněného TFM
- **labs/part2-foundry-local-sdk.md:** Aktualizován příklad `.csproj` ukazující plný multiplatformní vzor s podmíněným TFM, vzájemně vyhrazenými odkazy na balíčky a vysvětlující poznámkou

### Validováno
- Všechny 3 C# projekty (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) úspěšně sestaveny na Windows ARM64
- Chat příklad (`dotnet run chat`): model se načte jako `phi-3.5-mini-instruct-qnn-npu:1` přes WinML/QNN — NPU varianta se načítá přímo bez fallbacku na CPU
- Agent příklad (`dotnet run agent`): běží end-to-end s víceturnovým konverzačním režimem, kód ukončení 0
- Foundry Local CLI v0.8.117 a SDK v0.9.0 na .NET SDK 9.0.312

---

## 2026-03-11 — Opravy kódu, Čištění modelu, Mermaid diagramy a Validace

### Opraveno
- **Všechny 21 příkladů kódu (7 Python, 7 JavaScript, 7 C#):** Přidán `model.unload()` / `unload_model()` / `model.UnloadAsync()` na ukončení pro vyřešení varování o úniku paměti OGA (Známý problém #4)
- **csharp/WhisperTranscription.cs:** Nahrazení křehké relativní cesty `AppContext.BaseDirectory` funkcí `FindSamplesDirectory()`, která spolehlivě prochází adresáře nahoru a lokalizuje `samples/audio` (Známý problém #7)
- **csharp/csharp.csproj:** Nahrazení hardcoded `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` automatickou detekcí pomocí `$(NETCoreSdkRuntimeIdentifier)`, aby příkaz `dotnet run` fungoval na jakékoli platformě bez parametru `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Změněno
- **Část 8:** Převod eval-driven iteračního cyklu z ASCII box diagramu na vykreslený SVG obrázek
- **Část 10:** Převod diagramu kompilace pipeline z ASCII šipek na vykreslený SVG obrázek
- **Část 11:** Převod diagramů toku volání nástrojů a sekvence na vykreslené SVG obrázky
- **Část 10:** Přemístění sekce „Workshop dokončen!“ do Části 11 (poslední lab); nahrazeno odkazem „Další kroky“
- **KNOWN-ISSUES.md:** Kompletní převasidování všech problémů proti CLI v0.8.117. Odstraněno vyřešené: Únik paměti OGA (přidáno čištění), cesta Whisper (FindSamplesDirectory), HTTP 500 pro trvalou inferenci (nereprodukovatelné, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), omezení tool_choice (nyní funguje s `"required"` a specifickým cílením funkcí na qwen2.5-0.5b). Aktualizace problému JS Whisper — nyní všechny soubory vracejí prázdný/binární výstup (regrese z v0.9.x, stupeň závažnosti zvýšen na Major). Aktualizace #4 C# RID s workaroundem autodetekce a odkazem [#497](https://github.com/microsoft/Foundry-Local/issues/497). Zůstává 7 otevřených problémů.
- **javascript/foundry-local-whisper.mjs:** Oprava názvu proměnné při čištění (`whisperModel` → `model`)

### Validováno
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — úspěšné spuštění s čištěním
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — úspěšné spuštění s čištěním
- C#: `dotnet build` proběhne bez varování a chyb (cíl net9.0)
- Všechny 7 Python souborů projdou syntaxovou kontrolou `py_compile`
- Všechny 7 JavaScript souborů projdou syntaxovou kontrolou `node --check`

---

## 2026-03-10 — Část 11: Volání nástrojů, Rozšíření SDK API a Pokrytí modelů

### Přidáno
- **Část 11: Volání nástrojů s lokálními modely** — nový průvodce laby („labs/part11-tool-calling.md“) s 8 cvičeními pokrývajícími schémata nástrojů, víceturnový tok, více volání nástrojů, vlastní nástroje, volání ChatClient nástrojů a `tool_choice`
- **Python příklad:** `python/foundry-local-tool-calling.py` — volání nástrojů s nástroji `get_weather`/`get_population` pomocí OpenAI SDK
- **JavaScript příklad:** `javascript/foundry-local-tool-calling.mjs` — volání nástrojů s nativním SDK `ChatClient` (`model.createChatClient()`)
- **C# příklad:** `csharp/ToolCalling.cs` — volání nástrojů pomocí `ChatTool.CreateFunctionTool()` s OpenAI C# SDK
- **Část 2, Cvičení 7:** Nativní `ChatClient` — `model.createChatClient()` (JS) a `model.GetChatClientAsync()` (C#) jako alternativa ke OpenAI SDK
- **Část 2, Cvičení 8:** Varianty modelů a výběr hardwaru — `selectVariant()`, `variants`, tabulka variant NPU (7 modelů)
- **Část 2, Cvičení 9:** Upgrade modelů a aktualizace katalogu — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Část 2, Cvičení 10:** Modely pro uvažování — `phi-4-mini-reasoning` s příklady parsování tagu `<think>`
- **Část 3, Cvičení 4:** `createChatClient` jako alternativa k OpenAI SDK, dokumentace callback vzoru streamování
- **AGENTS.md:** Přidány konvence pro kódování volání nástrojů, ChatClient and modelů uvažování

### Změněno
- **Část 1:** Rozšířen katalog modelů — přidáno phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Část 2:** Rozšířeny tabulky referencí API — přidáno `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Část 2:** Přesun cvičení 7-9 na 10-13 kvůli novým cvičením
- **Část 3:** Aktualizována tabulka hlavních poznatků o native ChatClient
- **README.md:** Přidána sekce Část 11 s tabulkou příkladů kódu; přidán cíl učení #11; aktualizováno stromová struktura projektu
- **csharp/Program.cs:** Přidán případ `toolcall` do CLI routeru a aktualizován text nápovědy

---

## 2026-03-09 — Aktualizace SDK v0.9.0, Britská angličtina a Validace

### Změněno
- **Všechny příklady kódu (Python, JavaScript, C#):** Aktualizováno na Foundry Local SDK API v0.9.0 — opraven `await catalog.getModel()` (chyběl `await`), aktualizovány init vzory `FoundryLocalManager`, opraveno zjišťování endpointu
- **Všechny labové průvodce (Části 1–10):** Převod na britskou angličtinu (colour, catalogue, optimised, apod.)
- **Všechny labové průvodce:** Aktualizovány příklady kódu SDK pro API v0.9.0
- **Všechny labové průvodce:** Aktualizace tabulek referencí API a bloků kódu v cvičeních
- **Kritická oprava JavaScriptu:** Přidáno chybějící `await` u `catalog.getModel()` — vracelo `Promise` místo `Model` objektu, způsobující tichá selhání

### Validováno
- Všechny Python příklady běží úspěšně proti Foundry Local službě
- Všechny JavaScript příklady běží úspěšně (Node.js 18+)
- C# projekt sestavuje a běží na .NET 9.0 (zpětná kompatibilita z net8.0 SDK sestavení)
- 29 souborů bylo změněno a validováno v rámci workshopu

---

## Index souborů

| Soubor | Poslední aktualizace | Popis |
|--------|---------------------|-------|
| `labs/part1-getting-started.md` | 2026-03-10 | Rozšířen katalog modelů |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nová cvičení 7-10, rozšířené tabulky API |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Nové cvičení 4 (ChatClient), aktualizované poznatky |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + britská angličtina |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + britská angličtina |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + britská angličtina |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + britská angličtina |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + britská angličtina |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diagram, přesunuto Workshop Complete do části 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Nová laboratoř, Mermaid diagramy, sekce Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Nové: ukázka volání nástroje |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Nové: ukázka volání nástroje |
| `csharp/ToolCalling.cs` | 2026-03-10 | Nové: ukázka volání nástroje |
| `csharp/Program.cs` | 2026-03-10 | Přidán příkaz CLI `toolcall` |
| `README.md` | 2026-03-10 | Část 11, struktura projektu |
| `AGENTS.md` | 2026-03-10 | Volání nástroje + konvence ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Vyřešená Issue #7 odstraněna, zbývá 6 otevřených problémů |
| `csharp/csharp.csproj` | 2026-03-11 | Cross-platform TFM, podmíněné reference WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Cross-platform TFM, automatické zjištění RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Cross-platform TFM, automatické zjištění RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Odstraněno řešení NPU try/catch |
| `csharp/SingleAgent.cs` | 2026-03-11 | Odstraněno řešení NPU try/catch |
| `csharp/MultiAgent.cs` | 2026-03-11 | Odstraněno řešení NPU try/catch |
| `csharp/RagPipeline.cs` | 2026-03-11 | Odstraněno řešení NPU try/catch |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Odstraněno řešení NPU try/catch |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Ukázka cross-platform .csproj |
| `AGENTS.md` | 2026-03-11 | Aktualizované C# balíčky a detaily TFM |
| `CHANGELOG.md` | 2026-03-11 | Tento soubor |