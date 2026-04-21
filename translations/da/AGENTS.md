# Instruktioner til Kodningsagent

Denne fil giver kontekst til AI-kodningsagenter (GitHub Copilot, Copilot Workspace, Codex osv.), der arbejder i dette repository.

## Projektoversigt

Dette er en **praktisk workshop** til at bygge AI-applikationer med [Foundry Local](https://foundrylocal.ai) — en let runtime, der downloader, håndterer og serverer sprogmodeller fuldstændigt på enheden via en OpenAI-kompatibel API. Workshoppen inkluderer trin-for-trin labvejledninger og kørbare kodeeksempler i Python, JavaScript og C#.

## Repository Struktur

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

## Sprog & Framework Detaljer

### Python
- **Placering:** `python/`, `zava-creative-writer-local/src/api/`
- **Afhængigheder:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Nøglepakker:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Min version:** Python 3.9+
- **Kør:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Placering:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Afhængigheder:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Nøglepakker:** `foundry-local-sdk`, `openai`
- **Modulsystem:** ES moduler (`.mjs` filer, `"type": "module"`)
- **Min version:** Node.js 18+
- **Kør:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Placering:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Projektfiler:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Nøglepakker:** `Microsoft.AI.Foundry.Local` (ikke-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset med QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Mål:** .NET 9.0 (betinget TFM: `net9.0-windows10.0.26100` på Windows, `net9.0` ellers)
- **Kør:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Kodningskonventioner

### Generelt
- Alle kodeeksempler er **selvstændige enkelt-fil eksempler** — ingen delte hjælpebiblioteker eller abstraktioner.
- Hvert eksempel kører uafhængigt efter installation af egne afhængigheder.
- API-nøgler sættes altid til `"foundry-local"` — Foundry Local bruger dette som en pladsholder.
- Base-URL'er bruger `http://localhost:<port>/v1` — porten er dynamisk og findes under køretid via SDK'en (`manager.urls[0]` i JS, `manager.endpoint` i Python).
- Foundry Local SDK håndterer servicestart og endpointopdagelse; foretræk SDK-mønstre frem for hårdkodede porte.

### Python
- Brug `openai` SDK med `OpenAI(base_url=..., api_key="not-required")`.
- Brug `FoundryLocalManager()` fra `foundry_local` til SDK-styret servicelivscyklus.
- Streaming: iterer over `stream` objekt med `for chunk in stream:`.
- Ingen typeannoteringer i eksempel filer (hold eksempler korte til workshop-deltagere).

### JavaScript
- ES modulesyntaks: `import ... from "..."`.
- Brug `OpenAI` fra `"openai"` og `FoundryLocalManager` fra `"foundry-local-sdk"`.
- SDK init-mønster: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Top-level `await` anvendes gennemgående.

### C#
- Nullable aktiveret, implicit usings, .NET 9.
- Brug `FoundryLocalManager.StartServiceAsync()` til SDK-styret livscyklus.
- Streaming: `CompleteChatStreaming()` med `foreach (var update in completionUpdates)`.
- Den primære `csharp/Program.cs` er en CLI-router, der dispatches til statiske `RunAsync()` metoder.

### Tool Calling
- Kun visse modeller understøtter tool calling: **Qwen 2.5** familien (`qwen2.5-*`) og **Phi-4-mini** (`phi-4-mini`).
- Tool-skemaer følger OpenAI function-calling JSON format (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Konversationen bruger et multi-turn mønster: bruger → assistent (tool_calls) → værktøj (resultater) → assistent (endeligt svar).
- `tool_call_id` i værktøjsresultatbeskeder skal matche `id` fra modellens tool call.
- Python bruger OpenAI SDK direkte; JavaScript bruger SDK’ens native `ChatClient` (`model.createChatClient()`); C# bruger OpenAI SDK med `ChatTool.CreateFunctionTool()`.

### ChatClient (Native SDK Client)
- JavaScript: `model.createChatClient()` returnerer en `ChatClient` med `completeChat(messages, tools?)` og `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` returnerer en standard `ChatClient`, som kan bruges uden at importere OpenAI NuGet pakken.
- Python har ikke en native ChatClient — brug OpenAI SDK med `manager.endpoint` og `manager.api_key`.
- **Vigtigt:** JavaScript `completeStreamingChat` anvender et **callback-mønster**, ikke async iteration.

### Reasoning Models
- `phi-4-mini-reasoning` omslutter sin tænkning i `<think>...</think>` tags før det endelige svar.
- Pars disse tags for at adskille ræsonnement fra svaret, når det er nødvendigt.

## Lab Vejledninger

Lab-filer findes i `labs/` som Markdown. De følger en ensartet struktur:
- Logo header billede
- Titel og mål-callout
- Oversigt, læringsmål, forudsætninger
- Konceptforklaring med diagrammer
- Nummererede øvelser med kodeblokke og forventet output
- Oversigtstabel, nøglepunkter, yderligere læsning
- Navigationslink til næste del

Ved redigering af lab-indhold:
- Bevar eksisterende Markdown-format og sektionhierarki.
- Kodeblokke skal specificere sproget (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Giv både bash- og PowerShell-varianter for shell-kommandolinjer, hvor OS spiller en rolle.
- Brug callout-stilarter som `> **Note:**`, `> **Tip:**` og `> **Troubleshooting:**`.
- Tabeller bruger pipe-formatet `| Header | Header |`.

## Build & Test Kommandoer

| Handling | Kommando |
|----------|----------|
| **Python eksempler** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS eksempler** | `cd javascript && npm install && node <script>.mjs` |
| **C# eksempler** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generer diagrammer** | `npx mmdc -i <input>.mmd -o <output>.svg` (kræver root `npm install`) |

## Eksterne Afhængigheder

- **Foundry Local CLI** skal installeres på udviklerens maskine (`winget install Microsoft.FoundryLocal` eller `brew install foundrylocal`).
- **Foundry Local service** kører lokalt og udsætter en OpenAI-kompatibel REST API på en dynamisk port.
- Ingen cloud-services, API-nøgler eller Azure-abonnementer kræves for at køre noget eksempel.
- Del 10 (tilpassede modeller) kræver desuden `onnxruntime-genai` og downloader modelvægte fra Hugging Face.

## Filer Der Ikke Skal Committes

`.gitignore` bør udelukke (og gør for det meste):
- `.venv/` — Python virtuelle miljøer
- `node_modules/` — npm-afhængigheder
- `models/` — kompilering af ONNX model output (store binære filer, genereret af Del 10)
- `cache_dir/` — Hugging Face model download cache
- `.olive-cache/` — Microsoft Olive arbejdsmappe
- `samples/audio/*.wav` — genererede lydsamples (regenereres via `python samples/audio/generate_samples.py`)
- Standard Python build artefakter (`__pycache__/`, `*.egg-info/`, `dist/`, osv.)

## Licens

MIT — se `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokument er oversat ved hjælp af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi stræber efter nøjagtighed, bedes du være opmærksom på, at automatiserede oversættelser kan indeholde fejl eller unøjagtigheder. Det originale dokument på dets oprindelige sprog bør betragtes som den autoritative kilde. For kritisk information anbefales professionel menneskelig oversættelse. Vi påtager os intet ansvar for misforståelser eller fejltolkninger, der opstår som følge af brugen af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->