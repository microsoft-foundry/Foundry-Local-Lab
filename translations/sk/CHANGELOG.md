# Zmeny — Foundry Local Workshop

Všetky významné zmeny tohto workshopu sú zdokumentované nižšie.

---

## 2026-03-11 — Časť 12 & 13, Webové UI, Prepísanie Whisper, Oprava WinML/QNN a Validácia

### Pridané
- **Časť 12: Vytváranie webového UI pre Zava Creative Writer** — nový návod v labáku (`labs/part12-zava-ui.md`) s cvičeniami pokrývajúcimi streamovanie NDJSON, prehliadačový `ReadableStream`, live značky statusu agenta a streamovanie textu článku v reálnom čase
- **Časť 13: Workshop dokončený** — nový súhrnný labák (`labs/part13-workshop-complete.md`) s rekapituláciou všetkých 12 častí, ďalšími nápadmi a odkazmi na zdroje
- **Frontend Zava UI:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — spoločné vanilla HTML/CSS/JS prehliadačové rozhranie používané všetkými tromi backendmi
- **JavaScript HTTP server:** `zava-creative-writer-local/src/javascript/server.mjs` — nový Express-štýl HTTP server obalujúci orchestrátor pre prístup z prehliadača
- **Backend C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` a `ZavaCreativeWriterWeb.csproj` — nový minimalistický API projekt slúžiaci UI a streamujúci NDJSON
- **Generátor audio vzoriek:** `samples/audio/generate_samples.py` — offline TTS skript využívajúci `pyttsx3` na generovanie WAV súborov so Zava témou pre Časť 9
- **Audio vzorka:** `samples/audio/zava-full-project-walkthrough.wav` — nová dlhšia audio vzorka na testovanie transkripcie
- **Validácia skriptu:** `validate-npu-workaround.ps1` — automatizovaný PowerShell skript na validáciu obchádzky NPU/QNN naprieč všetkými C# vzorkami
- **Mermaid diagramy SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML multiplatformová podpora:** Všetky 3 C# `.csproj` ( `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) teraz používajú podmienený TFM a navzájom si vylučujúce odkazy na balíčky pre multiplatformovú podporu. Na Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (superset obsahujúci QNN EP plugin). Na ne-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (základné SDK). Tvrdé kódovanie RID `win-arm64` v Zava projektoch bolo nahradené automatickou detekciou. Obchádzka zprostredkovanej závislosti vylučuje natívne assety z `Microsoft.ML.OnnxRuntime.Gpu.Linux`, ktoré majú zlý odkaz na win-arm64. Predchádzajúca try/catch obchádzka NPU bola odstránená zo všetkých 7 C# súborov.

### Zmenené
- **Časť 9 (Whisper):** Výrazné prepísanie — JavaScript teraz používa zabudovaný SDK `AudioClient` (`model.createAudioClient()`) namiesto manuálneho inference ONNX Runtime; aktualizované popisy architektúry, porovnávacie tabuľky a diagramy pipeline reflektujúce prístup JS/C# `AudioClient` vs Python ONNX Runtime
- **Časť 11:** Aktualizované navigačné odkazy (teraz ukazuje na Časť 12); pridané renderované SVG diagramy pre tok volania nástrojov a sekvenciu
- **Časť 10:** Aktualizovaná navigácia vedie cez Časť 12 namiesto ukončenia workshopu
- **Python Whisper (`foundry-local-whisper.py`):** Rozšírený o ďalšie audio vzorky a vylepšenú správu chýb
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Prepísaný na používanie `model.createAudioClient()` s `audioClient.transcribe()` namiesto manuálnych ONNX Runtime session
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Aktualizovaný na službu statických UI súborov spolu s API
- **Zava C# konzola (`zava-creative-writer-local/src/csharp/Program.cs`):** Odstránená obchádzka NPU (teraz riešené WinML balíčkom)
- **README.md:** Pridaná sekcia Časť 12 s tabuľkami ukážok kódu a doplnkami backendu; pridaná sekcia Časť 13; aktualizované vzdelávacie ciele a štruktúra projektu
- **KNOWN-ISSUES.md:** Odstránený vyriešený problém č. 7 (C# SDK NPU Model Variant — teraz riešené WinML balíčkom). Očíslovanie zostávajúcich problémov na #1–#6. Aktualizované detaily prostredia s .NET SDK 10.0.104
- **AGENTS.md:** Aktualizovaný strom štruktúry projektu s novými položkami `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); aktualizované kľúčové balíčky C# a detaily podmieneného TFM
- **labs/part2-foundry-local-sdk.md:** Aktualizovaný príklad `.csproj` na plný cross-platformový vzor s podmieneným TFM, navzájom si vylučujúcimi odkazmi na balíčky a vysvetľujúcou poznámkou

### Validované
- Všetky 3 C# projekty (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) sa úspešne zostavujú na Windows ARM64
- Chat vzorka (`dotnet run chat`): model sa načítava ako `phi-3.5-mini-instruct-qnn-npu:1` cez WinML/QNN — NPU varianta sa načíta priamo bez CPU fallbacku
- Agent vzorka (`dotnet run agent`): beží end-to-end s viacnásobnou konverzáciou, exit code 0
- Foundry Local CLI v0.8.117 a SDK v0.9.0 na .NET SDK 9.0.312

---

## 2026-03-11 — Opravy Kódu, Vyčistenie Modelu, Mermaid Diagramy a Validácia

### Opravené
- **Všetky 21 ukážok kódu (7 Python, 7 JavaScript, 7 C#):** Pridané `model.unload()` / `unload_model()` / `model.UnloadAsync()` čistenie pri ukončení na vyriešenie upozornení na únik pamäte OGA (známy problém #4)
- **csharp/WhisperTranscription.cs:** Nahradená krehká relatívna cesta `AppContext.BaseDirectory` funkciou `FindSamplesDirectory()`, ktorá spoľahlivo vyhľadáva `samples/audio` chodením hore adresármi (známy problém #7)
- **csharp/csharp.csproj:** Nahradený pevne daný `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` fallbackom s automatickou detekciou použitím `$(NETCoreSdkRuntimeIdentifier)`, aby `dotnet run` fungoval na každej platforme bez `-r` flagu ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Zmenené
- **Časť 8:** Prevedený eval-driven iteratívny cyklus z ASCII box diagramu na renderovaný SVG obrázok
- **Časť 10:** Prevedený diagram pipeline kompilácie z ASCII šípok na renderovaný SVG obrázok
- **Časť 11:** Prevedené diagramy toku volania nástrojov a sekvencie na renderované SVG obrázky
- **Časť 10:** Sekcia "Workshop dokončený!" presunutá do Časti 11 (záverečný labák); nahradená odkazom "Ďalšie kroky"
- **KNOWN-ISSUES.md:** Komplexná revalidácia všetkých problémov proti CLI v0.8.117. Odstránené vyriešené: OGA Memory Leak (pridané čistenie), Whisper cesta (FindSamplesDirectory), HTTP 500 počas udržaného inference (nereprodukovateľné, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), obmedzenia tool_choice (teraz funguje s `"required"` a špecifickým cielením funkcií na qwen2.5-0.5b). Aktualizovaný JS Whisper problém — teraz všetky súbory vracajú prázdny/binárny výstup (regresia od v0.9.x, závažnosť zvýšená na Major). Aktualizovaný #4 C# RID s automatickým workaroundom a odkazom [#497](https://github.com/microsoft/Foundry-Local/issues/497). Zostáva 7 otvorených problémov.
- **javascript/foundry-local-whisper.mjs:** Opravený názov premennej pri čistení (`whisperModel` → `model`)

### Validované
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — úspešne spustené s čistením
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — úspešne spustené s čistením
- C#: `dotnet build` prebieha bez varovaní a chýb (target net9.0)
- Všetkých 7 Python súborov prešlo syntax kontrolou `py_compile`
- Všetkých 7 JavaScript súborov prešlo syntax validáciou cez `node --check`

---

## 2026-03-10 — Časť 11: Volanie Nástrojov, Rozšírenie SDK API a Pokrytie Modelov

### Pridané
- **Časť 11: Volanie nástrojov s lokálnymi modelmi** — nový návod v labáku (`labs/part11-tool-calling.md`) s 8 cvičeniami pokrývajúcimi schémy nástrojov, multi-turn tok, viacnásobné volania nástrojov, vlastné nástroje, volanie nástrojov ChatClientom a `tool_choice`
- **Python vzorka:** `python/foundry-local-tool-calling.py` — volanie nástrojov `get_weather`/`get_population` pomocou OpenAI SDK
- **JavaScript vzorka:** `javascript/foundry-local-tool-calling.mjs` — volanie nástrojov pomocou natívneho SDK `ChatClient` (`model.createChatClient()`)
- **C# vzorka:** `csharp/ToolCalling.cs` — volanie nástrojov s použitím `ChatTool.CreateFunctionTool()` s OpenAI C# SDK
- **Časť 2, Cvičenie 7:** Natívny `ChatClient` — `model.createChatClient()` (JS) a `model.GetChatClientAsync()` (C#) ako alternatívy OpenAI SDK
- **Časť 2, Cvičenie 8:** Varianty modelu a výber hardvéru — `selectVariant()`, `variants`, tabuľka NPU varianty (7 modelov)
- **Časť 2, Cvičenie 9:** Upgrade modelov a obnova katalógu — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Časť 2, Cvičenie 10:** Modely na uvažovanie — `phi-4-mini-reasoning` s príkladmi analýzy tagu `<think>`
- **Časť 3, Cvičenie 4:** `createChatClient` ako alternatíva k OpenAI SDK, dokumentácia patternu callback v streamovaní
- **AGENTS.md:** Pridané kódovacie konvencie pre Volanie Nástrojov, ChatClient a Modely uvažovania

### Zmenené
- **Časť 1:** Rozšírený katalóg modelov — pridané phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Časť 2:** Rozšírené API referenčné tabuľky — pridané `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Časť 2:** Prečíslovanie cvičení 7-9 → 10-13 kvôli novým cvičeniam
- **Časť 3:** Aktualizovaná tabuľka kľúčových poznatkov o natívnom ChatClientovi
- **README.md:** Pridaná sekcia Časť 11 s tabuľkou ukážok kódu; pridaný vzdelávací cieľ #11; aktualizovaný strom štruktúry projektu
- **csharp/Program.cs:** Pridaný prípad `toolcall` do CLI routera a aktualizovaný pomocný text

---

## 2026-03-09 — Aktualizácia SDK v0.9.0, Britská angličtina a Validácia

### Zmenené
- **Všetky ukážky kódu (Python, JavaScript, C#):** Aktualizované na Foundry Local SDK v0.9.0 API — opravený chýbajúci `await` pri `catalog.getModel()`, aktualizované inicializačné vzory `FoundryLocalManager`, opravené zisťovanie endpointov
- **Všetky labáky (Časti 1-10):** Prevedené do britskej angličtiny (colour, catalogue, optimised, atď.)
- **Všetky labáky:** Aktualizované ukážky SDK kódu podľa API v0.9.0
- **Všetky labáky:** Aktualizované API tabuľky a kódové bloky cvičení
- **JavaScript kritická oprava:** Pridaný chýbajúci `await` na `catalog.getModel()` — vrátilo sa `Promise`, nie objekt `Model`, spôsobujúce tiché chyby ďaleko v kóde

### Validované
- Všetky Python vzorky úspešne bežia s Foundry Local službou
- Všetky JavaScript vzorky úspešne bežia (Node.js 18+)
- C# projekt sa zostavuje a spúšťa na .NET 9.0 (zpětná kompatibilita z net8.0 SDK assembly)
- Upravených a validovaných 29 súborov v celom workshope

---

## Index súborov

| Súbor | Posledná aktualizácia | Popis |
|-------|----------------------|--------|
| `labs/part1-getting-started.md` | 2026-03-10 | Rozšírený katalóg modelov |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Nové cvičenia 7-10, rozšírené API tabuľky |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Nové Cvičenie 4 (ChatClient), aktualizované poznatky |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + Britská angličtina |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + Britská angličtina |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + britská angličtina |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + britská angličtina |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + britská angličtina |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diagram, presunuté Workshop Complete do časti 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Nový lab, Mermaid diagramy, sekcia Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Novinka: príklad volania nástrojov |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Novinka: príklad volania nástrojov |
| `csharp/ToolCalling.cs` | 2026-03-10 | Novinka: príklad volania nástrojov |
| `csharp/Program.cs` | 2026-03-10 | Pridaný príkaz CLI `toolcall` |
| `README.md` | 2026-03-10 | Časť 11, štruktúra projektu |
| `AGENTS.md` | 2026-03-10 | Volanie nástrojov + konvencie ChatClienta |
| `KNOWN-ISSUES.md` | 2026-03-11 | Odstránený vyriešený problém č. 7, zostáva 6 otvorených problémov |
| `csharp/csharp.csproj` | 2026-03-11 | Cross-platform TFM, podmienené odkazy WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Cross-platform TFM, automatické rozpoznanie RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Cross-platform TFM, automatické rozpoznanie RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Odstránené riešenie chyby NPU try/catch |
| `csharp/SingleAgent.cs` | 2026-03-11 | Odstránené riešenie chyby NPU try/catch |
| `csharp/MultiAgent.cs` | 2026-03-11 | Odstránené riešenie chyby NPU try/catch |
| `csharp/RagPipeline.cs` | 2026-03-11 | Odstránené riešenie chyby NPU try/catch |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Odstránené riešenie chyby NPU try/catch |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Cross-platform príklad .csproj |
| `AGENTS.md` | 2026-03-11 | Aktualizované balíčky C# a detaily TFM |
| `CHANGELOG.md` | 2026-03-11 | Tento súbor |