# Instrukce pro kódovací agenty

Tento soubor poskytuje kontext pro AI kódovací agenty (GitHub Copilot, Copilot Workspace, Codex atd.) pracující v tomto repozitáři.

## Přehled projektu

Toto je **praktický workshop** na tvorbu AI aplikací s [Foundry Local](https://foundrylocal.ai) — lehké runtime prostředí, které stahuje, spravuje a poskytuje jazykové modely zcela lokálně pomocí API kompatibilního s OpenAI. Workshop obsahuje krok za krokem laboratorní návody a spustitelné příklady v Pythonu, JavaScriptu a C#.

## Struktura repozitáře

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

## Podrobnosti o jazycích a rámcích

### Python
- **Umístění:** `python/`, `zava-creative-writer-local/src/api/`
- **Závislosti:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Klíčové balíčky:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Minimální verze:** Python 3.9+
- **Spuštění:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Umístění:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Závislosti:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Klíčové balíčky:** `foundry-local-sdk`, `openai`
- **Modulový systém:** ES moduly (`.mjs` soubory, `"type": "module"`)
- **Minimální verze:** Node.js 18+
- **Spuštění:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Umístění:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Projektové soubory:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Klíčové balíčky:** `Microsoft.AI.Foundry.Local` (ne-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — nadmnožina s QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Cílový framework:** .NET 9.0 (podmíněný TFM: `net9.0-windows10.0.26100` na Windows, `net9.0` jinde)
- **Spuštění:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Kódovací konvence

### Obecně
- Všechny ukázky jsou **samostatné jednosoňové příklady** — žádné sdílené knihovny ani abstrakce.
- Každý příklad se spouští nezávisle po instalaci svých závislostí.
- API klíče jsou vždy nastaveny na `"foundry-local"` — Foundry Local používá tuto hodnotu jako zástupný symbol.
- Základní URL používají `http://localhost:<port>/v1` — port je dynamický a je zjišťován za běhu pomocí SDK (`manager.urls[0]` v JS, `manager.endpoint` v Pythonu).
- Foundry Local SDK řeší spuštění služby a objevení endpointu; preferujte SDK vzory před hardcodovanými porty.

### Python
- Používejte `openai` SDK s `OpenAI(base_url=..., api_key="not-required")`.
- Používejte `FoundryLocalManager()` z `foundry_local` pro správu lifecycle služby přes SDK.
- Streaming: iterujte přes objekt `stream` pomocí `for chunk in stream:`.
- V ukázkách nepoužívejte typové anotace (ukázky jsou stručné pro učící se workshop).

### JavaScript
- Syntaxe ES module: `import ... from "..."`.
- Používejte `OpenAI` z `"openai"` a `FoundryLocalManager` z `"foundry-local-sdk"`.
- Iniciační vzor SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Používá se top-level `await` po celou dobu.

### C#
- Nullable enabled, implicitní usingy, .NET 9.
- Používejte `FoundryLocalManager.StartServiceAsync()` pro SDK správu lifecycle.
- Streaming: `CompleteChatStreaming()` s `foreach (var update in completionUpdates)`.
- Hlavní `csharp/Program.cs` je CLI směrovač volající statické `RunAsync()` metody.

### Volání nástrojů
- Volání nástrojů podporují pouze některé modely: rodina **Qwen 2.5** (`qwen2.5-*`) a **Phi-4-mini** (`phi-4-mini`).
- Schéma nástrojů následuje JSON formát pro volání funkcí z OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Konverzace má multi-turn vzor: uživatel → asistent (tool_calls) → nástroj (výsledky) → asistent (konečná odpověď).
- `tool_call_id` ve zprávách s výsledky nástroje musí odpovídat `id` volání nástroje modelu.
- Python používá přímo OpenAI SDK; JavaScript SDK natívní `ChatClient` (`model.createChatClient()`); C# OpenAI SDK s `ChatTool.CreateFunctionTool()`.

### ChatClient (nativní SDK klient)
- JavaScript: `model.createChatClient()` vrací `ChatClient` s metodami `completeChat(messages, tools?)` a `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` vrací standardní `ChatClient`, který lze používat bez importu OpenAI NuGet balíčku.
- Python nativní ChatClient nemá — použijte OpenAI SDK s `manager.endpoint` a `manager.api_key`.
- **Důležité:** JavaScript `completeStreamingChat` používá **callback vzor**, nikoliv async iteraci.

### Modely pro rozumování
- `phi-4-mini-reasoning` obaluje své uvažování do tagů `<think>...</think>` před finální odpovědí.
- Pokud je potřeba, parsujte tagy, abyste oddělili uvažování od odpovědi.

## Laboratorní návody

Soubory laboratoří jsou v `labs/` jako Markdown. Mají konzistentní strukturu:
- Hlavička s logem
- Titulek a cíl
- Přehled, učební cíle, předpoklady
- Sekce s vysvětlením konceptů včetně diagramů
- Číslované cvičení s kódovými bloky a očekávaným výstupem
- Shrnutí v tabulce, klíčové postřehy, další čtení
- Navigační odkaz na další část

Při úpravách lab obsahů:
- Zachovejte stávající stylistiku Markdownu a hierarchii sekcí.
- Kódové bloky musí uvádět jazyk (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Poskytněte varianty v bash a PowerShell tam, kde záleží na OS.
- Používejte volací styly jako `> **Note:**`, `> **Tip:**`, a `> **Troubleshooting:**`.
- Tabulky používají formát s trubkami (`| Header | Header |`).

## Příkazy pro build a testování

| Akce | Příkaz |
|--------|---------|
| **Python ukázky** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS ukázky** | `cd javascript && npm install && node <script>.mjs` |
| **C# ukázky** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generování diagramů** | `npx mmdc -i <input>.mmd -o <output>.svg` (vyžaduje root `npm install`) |

## Externí závislosti

- **Foundry Local CLI** musí být nainstalováno na vývojářském stroji (`winget install Microsoft.FoundryLocal` nebo `brew install foundrylocal`).
- **Foundry Local služba** běží lokálně a vystavuje OpenAI kompatibilní REST API na dynamickém portu.
- K žádnému příkladu nemusíte pro běh používat cloudové služby, API klíče ani Azure předplatné.
- Část 10 (vlastní modely) navíc vyžaduje `onnxruntime-genai` a stahuje váhy modelů z Hugging Face.

## Soubory, které by se neměly commitovat

`.gitignore` vylučuje (a většinou vylučuje) tyto:
- `.venv/` — Python virtuální prostředí
- `node_modules/` — npm závislosti
- `models/` — sestavené ONNX modely (velké binární soubory, generované částí 10)
- `cache_dir/` — cache stahování modelů Hugging Face
- `.olive-cache/` — pracovní adresář Microsoft Olive
- `samples/audio/*.wav` — generované audio vzorky (znovuvytvářené pomocí `python samples/audio/generate_samples.py`)
- Standardní Python build artefakty (`__pycache__/`, `*.egg-info/`, `dist/`, atd.)

## Licence

MIT — viz `LICENSE`.