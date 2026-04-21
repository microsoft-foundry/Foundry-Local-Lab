# Koding Agent Instruksjoner

Denne filen gir kontekst for AI koding agenter (GitHub Copilot, Copilot Workspace, Codex, osv.) som jobber i dette depotet.

## Prosjektoversikt

Dette er en **praktisk workshop** for å bygge AI-applikasjoner med [Foundry Local](https://foundrylocal.ai) — en lettvekts runtime som laster ned, administrerer og leverer språkmodeller helt på enheten via en OpenAI-kompatibel API. Workshopen inkluderer steg-for-steg lab guider og kjørbare kodeeksempler i Python, JavaScript og C#.

## Depotstruktur

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

## Språk & Rammeverksdetaljer

### Python
- **Plassering:** `python/`, `zava-creative-writer-local/src/api/`
- **Avhengigheter:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Nøkkelpakker:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Min versjon:** Python 3.9+
- **Kjør:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Plassering:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Avhengigheter:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Nøkkelpakker:** `foundry-local-sdk`, `openai`
- **Modulsystem:** ES moduler (`.mjs` filer, `"type": "module"`)
- **Min versjon:** Node.js 18+
- **Kjør:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Plassering:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Prosjektfiler:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Nøkkelpakker:** `Microsoft.AI.Foundry.Local` (ikke-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset med QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Mål:** .NET 9.0 (betinget TFM: `net9.0-windows10.0.26100` på Windows, `net9.0` ellers)
- **Kjør:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Kodekonvensjoner

### Generelt
- Alle kodeeksempler er **enkelfilseksempler som er selvstendige** — ingen delte nyttelibs eller abstraksjoner.
- Hvert eksempel kjører uavhengig etter å ha installert sine egne avhengigheter.
- API-nøkler settes alltid til `"foundry-local"` — Foundry Local bruker dette som en plassholder.
- Base-URLer bruker `http://localhost:<port>/v1` — porten er dynamisk og oppdages ved kjøring via SDK (`manager.urls[0]` i JS, `manager.endpoint` i Python).
- Foundry Local SDK håndterer oppstart av tjenesten og endepunktsoppdagelse; foretrekk SDK-mønstre fremfor hardkodede porter.

### Python
- Bruk `openai` SDK med `OpenAI(base_url=..., api_key="not-required")`.
- Bruk `FoundryLocalManager()` fra `foundry_local` for SDK-administrert tjenestelivssyklus.
- Streaming: iterer over `stream` objektet med `for chunk in stream:`.
- Ingen typeannotasjoner i eksempel-filene (hold eksemplene konsise for workshop-deltakere).

### JavaScript
- ES modul-syntaks: `import ... from "..."`.
- Bruk `OpenAI` fra `"openai"` og `FoundryLocalManager` fra `"foundry-local-sdk"`.
- SDK initmønster: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Toppnivå `await` brukes gjennomgående.

### C#
- Nullable aktivert, implisitte usinger, .NET 9.
- Bruk `FoundryLocalManager.StartServiceAsync()` for SDK-administrert livssyklus.
- Streaming: `CompleteChatStreaming()` med `foreach (var update in completionUpdates)`.
- Hovedfilen `csharp/Program.cs` er en CLI-ruter som sender til statiske `RunAsync()` metoder.

### Verktøyanrop
- Kun visse modeller støtter verktøysanrop: **Qwen 2.5** familien (`qwen2.5-*`) og **Phi-4-mini** (`phi-4-mini`).
- Verktøyskjema følger OpenAI funksjonsanrop JSON-format (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Samtalen bruker et multitrinnsmønster: bruker → assistent (tool_calls) → verktøy (resultater) → assistent (endelig svar).
- `tool_call_id` i verktøyresultatmeldinger må matche `id` fra modellens verktøysanrop.
- Python bruker OpenAI SDK direkte; JavaScript bruker SDKs native `ChatClient` (`model.createChatClient()`); C# bruker OpenAI SDK med `ChatTool.CreateFunctionTool()`.

### ChatClient (Native SDK-klient)
- JavaScript: `model.createChatClient()` returnerer en `ChatClient` med `completeChat(messages, tools?)` og `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` returnerer en standard `ChatClient` som kan brukes uten å importere OpenAI NuGet pakken.
- Python har ikke en native ChatClient — bruk OpenAI SDK med `manager.endpoint` og `manager.api_key`.
- **Viktig:** JavaScript `completeStreamingChat` bruker en **callback-mønster**, ikke asynkron iterasjon.

### Resonneringsmodeller
- `phi-4-mini-reasoning` omslutter sin tenkning i `<think>...</think>` tagger før det endelige svaret.
- Pars taggene for å skille resonnement fra svaret når det er nødvendig.

## Lab Guider

Labfiler ligger i `labs/` som Markdown. De følger en konsistent struktur:
- Logo-headerbilde
- Tittel og mål-kall
- Oversikt, Læringsmål, Forutsetninger
- Konseptforklaringsseksjoner med diagrammer
- Nummererte øvelser med kodeblokker og forventet output
- Oppsummeringstabell, Nøkkeltrekk, Videre lesning
- Navigasjonslenke til neste del

Når du redigerer lab-innhold:
- Oppretthold eksisterende Markdown-formatstil og seksjonshierarki.
- Kodeblokker må spesifisere språk (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Gi både bash- og PowerShell-varianter for shell-kommandoer hvor OS betyr noe.
- Bruk kallout-stiler `> **Note:**`, `> **Tip:**` og `> **Troubleshooting:**`.
- Tabeller bruker `| Header | Header |` pipe-format.

## Bygge- & Testkommandoer

| Handling | Kommando |
|--------|---------|
| **Python eksempler** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS eksempler** | `cd javascript && npm install && node <script>.mjs` |
| **C# eksempler** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generer diagrammer** | `npx mmdc -i <input>.mmd -o <output>.svg` (krever global `npm install`) |

## Eksterne Avhengigheter

- **Foundry Local CLI** må være installert på utviklerens maskin (`winget install Microsoft.FoundryLocal` eller `brew install foundrylocal`).
- **Foundry Local tjeneste** kjører lokalt og eksponerer en OpenAI-kompatibel REST API på en dynamisk port.
- Ingen skyløsninger, API-nøkler eller Azure-abonnementer kreves for å kjøre noen prøver.
- Del 10 (egendefinerte modeller) krever i tillegg `onnxruntime-genai` og laster ned modellvekter fra Hugging Face.

## Filer som ikke skal kommitteres

`.gitignore` bør utelate (og gjør det for de fleste):
- `.venv/` — Python virtuelle miljøer
- `node_modules/` — npm avhengigheter
- `models/` — kompilerte ONNX modellfiler (store binærfiler, generert av Del 10)
- `cache_dir/` — Hugging Face modellnedlastingscache
- `.olive-cache/` — Microsoft Olive arbeidsmappe
- `samples/audio/*.wav` — genererte lydprøver (genereres på nytt via `python samples/audio/generate_samples.py`)
- Standard Python byggeartefakter (`__pycache__/`, `*.egg-info/`, `dist/`, osv.)

## Lisens

MIT — se `LICENSE`.