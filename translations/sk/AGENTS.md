# Pokyny pre kódovacieho agenta

Tento súbor poskytuje kontext pre AI kódovacie agentov (GitHub Copilot, Copilot Workspace, Codex atď.) pracujúcich v tomto repozitári.

## Prehľad projektu

Toto je **praktický workshop** na vytváranie AI aplikácií s [Foundry Local](https://foundrylocal.ai) — ľahké runtime, ktoré sťahuje, spravuje a poskytuje jazykové modely kompletne na zariadení cez API kompatibilné s OpenAI. Workshop obsahuje detailné lab príručky a spustiteľné ukážky kódu v Pythone, JavaScripte a C#.

## Štruktúra repozitára

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

## Podrobnosti o jazykoch a frameworkoch

### Python
- **Umiestnenie:** `python/`, `zava-creative-writer-local/src/api/`
- **Závislosti:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Kľúčové balíčky:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Minimálna verzia:** Python 3.9+
- **Spustenie:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Umiestnenie:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Závislosti:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Kľúčové balíčky:** `foundry-local-sdk`, `openai`
- **Modulový systém:** ES moduly (`.mjs` súbory, `"type": "module"`)
- **Minimálna verzia:** Node.js 18+
- **Spustenie:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Umiestnenie:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Projektové súbory:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Kľúčové balíčky:** `Microsoft.AI.Foundry.Local` (non-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset s QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Cieľ:** .NET 9.0 (podmienený TFM: `net9.0-windows10.0.26100` na Windows, `net9.0` inde)
- **Spustenie:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Kódovacie konvencie

### Všeobecné
- Všetky ukážky sú **samostatné jednosúborové príklady** — bez spoločných utilitných knižníc alebo abstrakcií.
- Každý príklad beží nezávisle po inštalácii svojich závislostí.
- API kľúče sú vždy nastavené na `"foundry-local"` — Foundry Local používa túto hodnotu ako zástupný symbol.
- Základné URL sú `http://localhost:<port>/v1` — port je dynamický a zisti sa za behu pomocou SDK (`manager.urls[0]` v JS, `manager.endpoint` v Pythone).
- Foundry Local SDK spravuje štart služby a zisťovanie endpointov; uprednostňujte SDK mechanizmy pred pevne zakódovanými portami.

### Python
- Používajte `openai` SDK s `OpenAI(base_url=..., api_key="not-required")`.
- Používajte `FoundryLocalManager()` z `foundry_local` na spravovanie životného cyklu služby cez SDK.
- Streaming: iterujte cez `stream` objekt pomocou `for chunk in stream:`.
- V ukážkach nechýbajú typové anotácie (zachovajte príklady stručné pre účastníkov workshopu).

### JavaScript
- Syntax ES modulu: `import ... from "..."`.
- Používajte `OpenAI` z `"openai"` a `FoundryLocalManager` z `"foundry-local-sdk"`.
- Init vzor SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Používa sa top-level `await` všade.

### C#
- Povolené nullable, implicitné usingy, .NET 9.
- Používajte `FoundryLocalManager.StartServiceAsync()` na spravovanie životného cyklu cez SDK.
- Streaming: `CompleteChatStreaming()` s `foreach (var update in completionUpdates)`.
- Hlavný súbor `csharp/Program.cs` je CLI smerovač volajúci statické metódy `RunAsync()`.

### Volanie nástrojov
- Iba určité modely podporujú volanie nástrojov: **Qwen 2.5** rodina (`qwen2.5-*`) a **Phi-4-mini** (`phi-4-mini`).
- Schémy nástrojov používajú OpenAI JSON formát pre volanie funkcií (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Konverzácia používa vzor s viacerými kolami: používateľ → asistent (tool_calls) → nástroj (výsledky) → asistent (konečná odpoveď).
- `tool_call_id` v správach s výsledkami nástroja musí zodpovedať `id` z volania nástroja modelom.
- Python používa priamo OpenAI SDK; JavaScript natívny ChatClient SDK (`model.createChatClient()`); C# OpenAI SDK s `ChatTool.CreateFunctionTool()`.

### ChatClient (natívny klient SDK)
- JavaScript: `model.createChatClient()` vracia `ChatClient` s metódami `completeChat(messages, tools?)` a `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` vracia štandardný `ChatClient`, ktorý je použiteľný bez importovania balíka OpenAI NuGet.
- Python natívny ChatClient nemá — používa sa OpenAI SDK s `manager.endpoint` a `manager.api_key`.
- **Dôležité:** JavaScript `completeStreamingChat` používa **callback vzor**, nie asynchrónnu iteráciu.

### Modely na rozumovanie
- `phi-4-mini-reasoning` obalí svoje uvažovanie do značiek `<think>...</think>` pred finálnou odpoveďou.
- Pri potrebe sa značenie parsuje na oddelenie uvažovania od odpovede.

## Lab príručky

Lab súbory sú v `labs/` ako Markdown. Majú konzistentnú štruktúru:
- Logo na hlavičke
- Názov a cieľ
- Prehľad, učené ciele, predpoklady
- Vysvetľujúce sekcie s diagramami
- Číslované cvičenia s kódovými blokmi a očakávaným výstupom
- Súhrnná tabuľka, kľúčové poznatky, ďalšie čítanie
- Navigačný odkaz na ďalšiu časť

Pri úprave obsahu labov:
- Zachovajte existujúci štýl Markdown formátovania a hierarchiu sekcií.
- Kódové bloky vždy so špecifikovaním jazyka (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Poskytnite variácie príkazov v bash aj PowerShell, ak má OS vplyv.
- Používajte výzvy `> **Note:**`, `> **Tip:**`, a `> **Troubleshooting:**`.
- Tabuľky vo formáte so zvislými čiarami `| Hlavička | Hlavička |`.

## Príkazy na zostavenie a testovanie

| Akcia | Príkaz |
|--------|---------|
| **Python ukážky** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS ukážky** | `cd javascript && npm install && node <script>.mjs` |
| **C# ukážky** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generovanie diagramov** | `npx mmdc -i <input>.mmd -o <output>.svg` (vyžaduje globálnu inštaláciu `npm install -g @mermaid-js/mermaid-cli`) |

## Externé závislosti

- **Foundry Local CLI** musí byť nainštalovaný na vývojárskom stroji (`winget install Microsoft.FoundryLocal` alebo `brew install foundrylocal`).
- **Foundry Local služba** beží lokálne a vystavuje OpenAI-kompatibilné REST API na dynamickom porte.
- Na spustenie žiadneho príkladu nie sú potrebné cloudové služby, API kľúče ani predplatné Azure.
- Časť 10 (vlastné modely) navyše vyžaduje `onnxruntime-genai` a sťahuje váhy modelov z Hugging Face.

## Súbory, ktoré by sa nemali commitovať

`.gitignore` by mal vylúčiť (a väčšina to aj robí):
- `.venv/` — Python virtuálne prostredia
- `node_modules/` — npm závislosti
- `models/` — skompilované ONNX modely (veľké binárne súbory, generované časťou 10)
- `cache_dir/` — cache sťahovaných modelov z Hugging Face
- `.olive-cache/` — pracovný adresár Microsoft Olive
- `samples/audio/*.wav` — generované zvukové ukážky (generované znova cez `python samples/audio/generate_samples.py`)
- Štandardné Python build artefakty (`__pycache__/`, `*.egg-info/`, `dist/` atď.)

## Licencia

MIT — pozri `LICENSE`.