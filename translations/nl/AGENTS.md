# Instructies voor Coding Agent

Dit bestand biedt context voor AI-coding agents (GitHub Copilot, Copilot Workspace, Codex, enz.) die in deze repository werken.

## Projectoverzicht

Dit is een **hands-on workshop** voor het bouwen van AI-toepassingen met [Foundry Local](https://foundrylocal.ai) — een lichte runtime die taalmodellen geheel op het apparaat downloadt, beheert en serveert via een OpenAI-compatibele API. De workshop bevat stapsgewijze labgidsen en uitvoerbare codevoorbeelden in Python, JavaScript en C#.

## Repository-structuur

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

## Details over Taal & Framework

### Python
- **Locatie:** `python/`, `zava-creative-writer-local/src/api/`
- **Afhankelijkheden:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Belangrijke pakketten:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Minimale versie:** Python 3.9+
- **Uitvoeren:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Locatie:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Afhankelijkheden:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Belangrijke pakketten:** `foundry-local-sdk`, `openai`
- **Modulesysteem:** ES modules (`.mjs`-bestanden, `"type": "module"`)
- **Minimale versie:** Node.js 18+
- **Uitvoeren:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Locatie:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Projectbestanden:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Belangrijke pakketten:** `Microsoft.AI.Foundry.Local` (niet-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset met QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Doel:** .NET 9.0 (voorwaardelijke TFM: `net9.0-windows10.0.26100` op Windows, `net9.0` elders)
- **Uitvoeren:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Programmeerstijlconventies

### Algemeen
- Alle codevoorbeelden zijn **zelfstandige voorbeeldbestanden** — geen gedeelde hulpprogramma-libraries of abstracties.
- Elk voorbeeld werkt onafhankelijk na installatie van zijn eigen afhankelijkheden.
- API-sleutels zijn altijd ingesteld op `"foundry-local"` — Foundry Local gebruikt dit als tijdelijke aanduiding.
- Basis-URL's gebruiken `http://localhost:<poort>/v1` — de poort is dynamisch en wordt tijdens runtime ontdekt via de SDK (`manager.urls[0]` in JS, `manager.endpoint` in Python).
- De Foundry Local SDK verzorgt het starten van de service en het ontdekken van de endpoint; geef de voorkeur aan SDK-patronen boven vast ingestelde poorten.

### Python
- Gebruik de `openai` SDK met `OpenAI(base_url=..., api_key="not-required")`.
- Gebruik `FoundryLocalManager()` van `foundry_local` voor SDK-beheerd service lifecycle.
- Streaming: doorloop het `stream`-object via `for chunk in stream:`.
- Geen type-annotaties in voorbeeldbestanden (houd voorbeelden beknopt voor workshopdeelnemers).

### JavaScript
- ES module-syntaxis: `import ... from "..."`.
- Gebruik `OpenAI` uit `"openai"` en `FoundryLocalManager` uit `"foundry-local-sdk"`.
- SDK-init-patroon: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Top-level `await` wordt overal gebruikt.

### C#
- Nullable aan, impliciete usings, .NET 9.
- Gebruik `FoundryLocalManager.StartServiceAsync()` voor SDK-beheerde lifecycle.
- Streaming: `CompleteChatStreaming()` met `foreach (var update in completionUpdates)`.
- De hoofd-`csharp/Program.cs` is een CLI-router die naar statische `RunAsync()`-methoden doorstuurt.

### Tool-aanroepen
- Alleen bepaalde modellen ondersteunen tool-aanroepen: **Qwen 2.5** familie (`qwen2.5-*`) en **Phi-4-mini** (`phi-4-mini`).
- Tool-schema's volgen het OpenAI-function-calling JSON-formaat (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Het gesprek gebruikt een multi-turn patroon: gebruiker → assistent (tool_calls) → tool (resultaten) → assistent (eindantwoord).
- De `tool_call_id` in tool result berichten moet overeenkomen met de `id` van het model's tool call.
- Python gebruikt de OpenAI SDK direct; JavaScript gebruikt de native SDK `ChatClient` (`model.createChatClient()`); C# gebruikt de OpenAI SDK met `ChatTool.CreateFunctionTool()`.

### ChatClient (Native SDK Client)
- JavaScript: `model.createChatClient()` retourneert een `ChatClient` met `completeChat(messages, tools?)` en `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` retourneert een standaard `ChatClient` die kan worden gebruikt zonder de OpenAI NuGet-package te importeren.
- Python heeft geen native ChatClient — gebruik de OpenAI SDK met `manager.endpoint` en `manager.api_key`.
- **Belangrijk:** JavaScript `completeStreamingChat` gebruikt een **callback-patroon**, geen async-iteratie.

### Redeneringsmodellen
- `phi-4-mini-reasoning` wikkelt zijn denken in `<think>...</think>` tags vóór het uiteindelijke antwoord.
- Parse de tags om redenering en antwoord te scheiden wanneer nodig.

## Labgidsen

Lab-bestanden staan in `labs/` als Markdown. Ze volgen een consistente structuur:
- Logo header afbeelding
- Titel en doelomschrijving
- Overzicht, leerdoelen, vereisten
- Conceptuele uitlegsecties met diagrammen
- Genummerde oefeningen met codeblokken en verwachte output
- Samenvattingstabel, belangrijkste inzichten, verder lezen
- Navigatielink naar het volgende deel

Bij het bewerken van labinhoud:
- Behoud de bestaande Markdown opmaakstijl en sectiehiërarchie.
- Codeblokken moeten de taal specificeren (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Bied zowel bash- als PowerShell-varianten voor shell-commando's waar OS van belang is.
- Gebruik callout-stijlen `> **Note:**`, `> **Tip:**`, en `> **Troubleshooting:**`.
- Tabellen gebruiken het pijpformaat `| Header | Header |`.

## Build- & Testcommando's

| Actie | Commando |
|--------|---------|
| **Python voorbeelden** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS voorbeelden** | `cd javascript && npm install && node <script>.mjs` |
| **C# voorbeelden** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Genereer diagrammen** | `npx mmdc -i <input>.mmd -o <output>.svg` (vereist root `npm install`) |

## Externe afhankelijkheden

- **Foundry Local CLI** moet op de ontwikkelaarsmachine geïnstalleerd zijn (`winget install Microsoft.FoundryLocal` of `brew install foundrylocal`).
- **Foundry Local service** draait lokaal en is bereikbaar via een OpenAI-compatibele REST API op een dynamische poort.
- Geen cloudservices, API-sleutels of Azure-abonnementen zijn vereist om voorbeelden uit te voeren.
- Deel 10 (aangepaste modellen) vereist daarnaast `onnxruntime-genai` en downloadt modelgewichten van Hugging Face.

## Bestanden die niet gedeeld mogen worden

De `.gitignore` sluit uit (en doet dat voor de meeste):
- `.venv/` — Python virtuele omgevingen
- `node_modules/` — npm-afhankelijkheden
- `models/` — gecompileerde ONNX modeluitvoer (grote binaire bestanden, gegenereerd door Deel 10)
- `cache_dir/` — Hugging Face model download cache
- `.olive-cache/` — Microsoft Olive-werkmap
- `samples/audio/*.wav` — gegenereerde audiomonsters (opnieuw gegenereerd via `python samples/audio/generate_samples.py`)
- Standaard Python build-artifacten (`__pycache__/`, `*.egg-info/`, `dist/`, enz.)

## Licentie

MIT — zie `LICENSE`.