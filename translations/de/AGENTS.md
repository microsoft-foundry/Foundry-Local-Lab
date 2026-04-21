# Anweisungen für Coding-Agenten

Diese Datei bietet Kontext für AI-Coding-Agenten (GitHub Copilot, Copilot Workspace, Codex usw.), die in diesem Repository arbeiten.

## Projektübersicht

Dies ist ein **praxisorientierter Workshop** zum Erstellen von AI-Anwendungen mit [Foundry Local](https://foundrylocal.ai) — einer leichtgewichtigen Laufzeitumgebung, die Sprachmodelle vollständig lokal auf dem Gerät über eine OpenAI-kompatible API herunterlädt, verwaltet und bereitstellt. Der Workshop enthält schrittweise Laboranleitungen und ausführbaren Beispielcode in Python, JavaScript und C#.

## Repository-Struktur

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

## Sprach- & Framework-Details

### Python
- **Ort:** `python/`, `zava-creative-writer-local/src/api/`
- **Abhängigkeiten:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Wichtige Pakete:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Minimale Version:** Python 3.9+
- **Ausführen:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Ort:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Abhängigkeiten:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Wichtige Pakete:** `foundry-local-sdk`, `openai`
- **Modulsystem:** ES-Module (`.mjs`-Dateien, `"type": "module"`)
- **Minimale Version:** Node.js 18+
- **Ausführen:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Ort:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Projektdateien:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Wichtige Pakete:** `Microsoft.AI.Foundry.Local` (non-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — Obermenge mit QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Ziel:** .NET 9.0 (bedingtes TFM: `net9.0-windows10.0.26100` unter Windows, sonst `net9.0`)
- **Ausführen:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Programmierkonventionen

### Allgemein
- Alle Codebeispiele sind **selbstständige Einzeldateibeispiele** — keine gemeinsamen Utility-Bibliotheken oder Abstraktionen.
- Jedes Beispiel läuft eigenständig nach Installation der eigenen Abhängigkeiten.
- API-Schlüssel sind immer auf `"foundry-local"` gesetzt — Foundry Local verwendet dies als Platzhalter.
- Basis-URLs nutzen `http://localhost:<port>/v1` — der Port ist dynamisch und wird zur Laufzeit über das SDK entdeckt (`manager.urls[0]` in JS, `manager.endpoint` in Python).
- Das Foundry Local SDK übernimmt den Dienst-Start und die Endpunkt-Erkennung; bevorzugen Sie SDK-Muster gegenüber fest codierten Ports.

### Python
- Verwenden Sie das `openai` SDK mit `OpenAI(base_url=..., api_key="not-required")`.
- Verwenden Sie `FoundryLocalManager()` aus `foundry_local` für den SDK-verwalteten Dienstlebenszyklus.
- Streaming: Iterieren Sie über das `stream`-Objekt mit `for chunk in stream:`.
- Keine Typannotationen in Beispiel-Dateien (halten Sie die Beispiele für Workshop-Teilnehmer knapp).

### JavaScript
- ES-Modul-Syntax: `import ... from "..."`.
- Verwenden Sie `OpenAI` aus `"openai"` und `FoundryLocalManager` aus `"foundry-local-sdk"`.
- SDK-Initialisierungsmuster: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Top-Level-`await` wird durchgehend verwendet.

### C#
- Nullable aktiviert, implizite Usings, .NET 9.
- Verwenden Sie `FoundryLocalManager.StartServiceAsync()` für den SDK-verwalteten Lebenszyklus.
- Streaming: `CompleteChatStreaming()` mit `foreach (var update in completionUpdates)`.
- Die Hauptdatei `csharp/Program.cs` ist ein CLI-Router, der an statische `RunAsync()`-Methoden weiterleitet.

### Tool-Aufrufe
- Nur bestimmte Modelle unterstützen Tool-Aufrufe: **Qwen 2.5** Familie (`qwen2.5-*`) und **Phi-4-mini** (`phi-4-mini`).
- Tool-Schemas folgen dem OpenAI-Funktionsaufruf-JSON-Format (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Das Gespräch verwendet ein Multi-Turn-Muster: Nutzer → Assistent (tool_calls) → Tool (Ergebnisse) → Assistent (Endantwort).
- Die `tool_call_id` in Tool-Ergebnisnachrichten muss mit der `id` aus dem Modell-Tool-Aufruf übereinstimmen.
- Python verwendet direkt das OpenAI SDK; JavaScript nutzt den nativen SDK-`ChatClient` (`model.createChatClient()`); C# verwendet das OpenAI SDK mit `ChatTool.CreateFunctionTool()`.

### ChatClient (Native SDK-Clients)
- JavaScript: `model.createChatClient()` gibt einen `ChatClient` mit `completeChat(messages, tools?)` und `completeStreamingChat(messages, callback)` zurück.
- C#: `model.GetChatClientAsync()` gibt einen Standard-`ChatClient` zurück, der verwendet werden kann, ohne das OpenAI NuGet-Paket zu importieren.
- Python hat keinen nativen ChatClient — verwenden Sie das OpenAI SDK mit `manager.endpoint` und `manager.api_key`.
- **Wichtig:** JavaScript `completeStreamingChat` verwendet ein **Callback-Muster**, nicht asynchrone Iteration.

### Reasoning-Modelle
- `phi-4-mini-reasoning` umschließt sein Denken in `<think>...</think>` Tags vor der Endantwort.
- Parsen Sie die Tags, um Denken und Antwort bei Bedarf zu trennen.

## Laboranleitungen

Labor-Dateien befinden sich in `labs/` als Markdown. Sie folgen einer konsistenten Struktur:
- Logo-Kopfbild
- Titel und Zielausruf
- Überblick, Lernziele, Voraussetzungen
- Konzept-Erklärungsabschnitte mit Diagrammen
- Nummerierte Übungen mit Codeblöcken und erwarteter Ausgabe
- Zusammenfassungstabelle, wichtige Erkenntnisse, weiterführende Literatur
- Navigations-Link zum nächsten Teil

Beim Bearbeiten von Laborinhalten:
- Erhalten Sie den vorhandenen Markdown-Formatierungsstil und die Abschnittshierarchie.
- Codeblöcke müssen die Sprache angeben (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Bieten Sie sowohl Bash- als auch PowerShell-Varianten für Shell-Befehle an, wenn das OS relevant ist.
- Verwenden Sie die Callout-Stile `> **Note:**`, `> **Tip:**` und `> **Troubleshooting:**`.
- Tabellen nutzen das Pipe-Format `| Header | Header |`.

## Build- & Test-Befehle

| Aktion | Befehl |
|--------|---------|
| **Python-Beispiele** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS-Beispiele** | `cd javascript && npm install && node <script>.mjs` |
| **C#-Beispiele** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (Web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (Web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Diagramme erzeugen** | `npx mmdc -i <input>.mmd -o <output>.svg` (erfordert root-`npm install`) |

## Externe Abhängigkeiten

- **Foundry Local CLI** muss auf der Entwicklermaschine installiert sein (`winget install Microsoft.FoundryLocal` oder `brew install foundrylocal`).
- **Foundry Local Dienst** läuft lokal und bietet eine OpenAI-kompatible REST-API auf einem dynamischen Port an.
- Es sind keine Cloud-Dienste, API-Schlüssel oder Azure-Abonnements erforderlich, um Beispiele auszuführen.
- Teil 10 (benutzerdefinierte Modelle) benötigt zusätzlich `onnxruntime-genai` und lädt Modellgewichte von Hugging Face herunter.

## Dateien, die nicht eingecheckt werden sollten

Die `.gitignore` schließt aus (und tut dies bei den meisten):
- `.venv/` — Python-Virtual Environments
- `node_modules/` — npm-Abhängigkeiten
- `models/` — kompilierte ONNX-Modell-Ausgabe (große Binärdateien, erzeugt in Teil 10)
- `cache_dir/` — Hugging Face Modelldownload-Cache
- `.olive-cache/` — Microsoft Olive-Arbeitsverzeichnis
- `samples/audio/*.wav` — generierte Audiodateien (erneut erzeugt via `python samples/audio/generate_samples.py`)
- Standard-Python-Build-Artefakte (`__pycache__/`, `*.egg-info/`, `dist/` usw.)

## Lizenz

MIT — siehe `LICENSE`.