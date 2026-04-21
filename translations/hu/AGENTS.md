# Kódoló Ügynök Útmutató

Ez a fájl kontextust nyújt az AI kódoló ügynökök (GitHub Copilot, Copilot Workspace, Codex stb.) számára, akik ebben a tárolóban dolgoznak.

## Projekt Áttekintés

Ez egy **gyakorlati műhelymunka** AI alkalmazások építéséhez a [Foundry Local](https://foundrylocal.ai) segítségével — egy könnyű futtatókörnyezet, amely nyelvi modelleket tölt le, kezel és szolgáltat teljes egészében eszközön belül, OpenAI-kompatibilis API-n keresztül. A műhelymenet lépésről lépésre vezetett labor útmutatókat és futtatható kódmintákat tartalmaz Python, JavaScript és C# nyelven.

## Tároló Felépítése

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## Nyelv és Keretrendszer Részletek

### Python
- **Hely:** `python/`, `zava-creative-writer-local/src/api/`
- **Függőségek:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Kulcs csomagok:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Min verzió:** Python 3.9+
- **Futtatás:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Hely:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Függőségek:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Kulcs csomagok:** `foundry-local-sdk`, `openai`
- **Modul rendszer:** ES modulok (`.mjs` fájlok, `"type": "module"`)
- **Min verzió:** Node.js 18+
- **Futtatás:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Hely:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Projekt fájlok:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Kulcs csomagok:** `Microsoft.AI.Foundry.Local` (nem Windowsos), `Microsoft.AI.Foundry.Local.WinML` (Windows — QNN EP-vel bővített), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Cél:** .NET 9.0 (feltételes TFM: `net9.0-windows10.0.26100` Windows esetén, máshol `net9.0`)
- **Futtatás:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Kódolási Szokások

### Általános
- Minden kódminta **önálló, egylapos példa** — nincs megosztott segédkönyvtár vagy absztrakció.
- Minden minta külön futtatható a saját függőségeinek telepítése után.
- API kulcs mindig `"foundry-local"` — a Foundry Local ezt helyőrzőként használja.
- Az alap URL-ek `http://localhost:<port>/v1` formátumúak — a port dinamikus, futásidőben az SDK fedezi fel (`manager.urls[0]` JS-ben, `manager.endpoint` Pythonban).
- A Foundry Local SDK kezeli a szolgáltatás indítását és az elérési végpontok felfedezését; preferáld az SDK mintákat a keménykódolt portok helyett.

### Python
- Használd az `openai` SDK-t az `OpenAI(base_url=..., api_key="not-required")` konstruktorral.
- Használd a `FoundryLocalManager()`-t a `foundry_local`-ból az SDK által kezelt szolgáltatás életciklushoz.
- Streamelés: iterálj a `stream` objektumon `for chunk in stream:` formában.
- Mintafájlokban ne használj típusannotációkat (tömör minták a workshop résztvevőknek).

### JavaScript
- ES modul szintaxis: `import ... from "..."`.
- Használd az `OpenAI`-t az `"openai"`-ból és a `FoundryLocalManager`-t a `"foundry-local-sdk"`-ból.
- SDK inicializációs minta: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streamelés: `for await (const chunk of stream)`.
- Top-level `await` használata mindenhol.

### C#
- Nullable engedélyezve, implicit using, .NET 9.
- Használd a `FoundryLocalManager.StartServiceAsync()` metódust az SDK által kezelt életciklushoz.
- Streamelés: `CompleteChatStreaming()` `foreach (var update in completionUpdates)` ciklussal.
- A fő `csharp/Program.cs` egy CLI útválasztó, amely statikus `RunAsync()` metódusokat hív meg.

### Eszköz Hívások
- Csak bizonyos modellek támogatják az eszköz hívást: **Qwen 2.5** család (`qwen2.5-*`) és **Phi-4-mini** (`phi-4-mini`).
- Az eszköz sémák az OpenAI funkcióhívás JSON formátumot követik (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- A beszélgetés többfordulós mintát követ: felhasználó → asszisztens (tool_calls) → eszköz (eredmények) → asszisztens (végső válasz).
- Az eszköz eredmény üzenetekben a `tool_call_id`-nek meg kell egyeznie a modell eszköz hívásában szereplő `id`-vel.
- Python közvetlenül az OpenAI SDK-t használja; JavaScript az SDK natív `ChatClient`-jét (`model.createChatClient()`); C# az OpenAI SDK-t használja a `ChatTool.CreateFunctionTool()`-lal.

### ChatClient (Natív SDK Ügyfél)
- JavaScript: `model.createChatClient()` egy `ChatClient`-et ad vissza, amely rendelkezik `completeChat(messages, tools?)` és `completeStreamingChat(messages, callback)` metódusokkal.
- C#: `model.GetChatClientAsync()` egy szabványos `ChatClient`-et ad vissza, amely OpenAI NuGet csomag importálása nélkül is használható.
- Python nem rendelkezik natív ChatClient-tel — használd az OpenAI SDK-t a `manager.endpoint` és `manager.api_key` paraméterekkel.
- **Fontos:** JavaScript `completeStreamingChat` **callback mintát használ**, nem aszinkron iterációt.

### Érvelő Modellek
- A `phi-4-mini-reasoning` a gondolkodását `<think>...</think>` tagek közé csomagolja a végső válasz előtt.
- A tageket elemezd a logika és a válasz elkülönítéséhez szükség szerint.

## Labor Útmutatók

A laborfájlok a `labs/` könyvtárban vannak Markdown formátumban. Egységes szerkezetet követnek:
- Logó fejléc kép
- Cím és cél kiemelés
- Áttekintés, Tanulási célok, Előfeltételek
- Fogalmi magyarázatok diagramokkal
- Sorszámozott gyakorlatok kódtömbökkel és elvárt kimenettel
- Összegző táblázat, Fontos tanulságok, További olvasmányok
- Navigációs link a következő részhez

Labor tartalom szerkesztésekor:
- Tartsd meg a meglévő Markdown formázási stílust és szakaszrendet.
- A kódtömbök nyelvét meg kell határozni (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Adj meg mind bash, mind PowerShell változatot a shell parancsokra, ahol az OS számít.
- Használj `> **Note:**`, `> **Tip:**` és `> **Troubleshooting:**` callout stílusokat.
- A táblázatok `| Fejléc | Fejléc |` csőformátumúak.

## Építés és Tesztelés Parancsok

| Művelet | Parancs |
|--------|---------|
| **Python minták** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS minták** | `cd javascript && npm install && node <script>.mjs` |
| **C# minták** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Diagram generálás** | `npx mmdc -i <input>.mmd -o <output>.svg` (root jogokkal `npm install` szükséges) |

## Külső Függőségek

- A **Foundry Local CLI** telepítve kell legyen a fejlesztő gépén (`winget install Microsoft.FoundryLocal` vagy `brew install foundrylocal`).
- A **Foundry Local szolgáltatás** helyileg fut, és egy OpenAI-kompatibilis REST API-t nyit egy dinamikus porton.
- Egyetlen minta futtatásához sem kell cloud szolgáltatás, API kulcs vagy Azure előfizetés.
- A 10. rész (egyedi modellek) továbbá megköveteli az `onnxruntime-genai`-t és a modell súlyokat Hugging Face-ről tölti le.

## Fájlok, Amiket Nem Szabad Feltölteni

A `.gitignore` kizárja (és a legtöbb esetben kizárja is):
- `.venv/` — Python virtuális környezetek
- `node_modules/` — npm függőségek
- `models/` — lefordított ONNX modell kimenet (nagy bináris fájlok, a 10. rész generálja)
- `cache_dir/` — Hugging Face modell letöltés cache
- `.olive-cache/` — Microsoft Olive munkakönyvtár
- `samples/audio/*.wav` — generált audio minták (újragenerálható a `python samples/audio/generate_samples.py`-vel)
- Szokásos Python build fájlok (`__pycache__/`, `*.egg-info/`, `dist/` stb.)

## Licenc

MIT — lásd `LICENSE`.