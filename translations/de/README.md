<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop – KI-Apps lokal auf dem Gerät erstellen

Ein praxisorientierter Workshop zum Ausführen von Sprachmodellen auf dem eigenen Rechner und zum Erstellen intelligenter Anwendungen mit [Foundry Local](https://foundrylocal.ai) und dem [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Was ist Foundry Local?** Foundry Local ist eine leichtgewichtige Laufzeitumgebung, mit der Sie Sprachmodelle vollständig auf Ihrer eigenen Hardware herunterladen, verwalten und bereitstellen können. Es bietet eine **OpenAI-kompatible API**, sodass jedes Tool oder SDK, das OpenAI unterstützt, sich verbinden kann – kein Cloud-Konto erforderlich.

---

## Lernziele

Am Ende dieses Workshops werden Sie in der Lage sein:

| # | Ziel |
|---|------|
| 1 | Foundry Local installieren und Modelle mit der CLI verwalten |
| 2 | Die Foundry Local SDK API für die programmatische Modellverwaltung beherrschen |
| 3 | Verbindung zum lokalen Inferenzserver mit den SDKs für Python, JavaScript und C# herstellen |
| 4 | Eine Retrieval-Augmented Generation (RAG) Pipeline bauen, die Antworten auf eigene Daten stützt |
| 5 | KI-Agenten mit persistenter Anleitung und Personas erstellen |
| 6 | Multi-Agent-Workflows mit Feedback-Schleifen orchestrieren |
| 7 | Eine produktionsreife Abschlussanwendung – den Zava Creative Writer – erkunden |
| 8 | Evaluierungsframeworks mit Gold-Datensätzen und LLM-als-Richter-Bewertung aufbauen |
| 9 | Audio mit Whisper transkribieren – Sprach-zu-Text lokal mit dem Foundry Local SDK |
| 10 | Eigene oder Hugging Face Modelle mit ONNX Runtime GenAI und Foundry Local kompilieren und ausführen |
| 11 | Lokale Modelle externe Funktionen über das Tool-Calling-Pattern aufrufen lassen |
| 12 | Eine browserbasierte UI für den Zava Creative Writer mit Echtzeit-Streaming erstellen |

---

## Voraussetzungen

| Anforderung | Details |
|-------------|---------|
| **Hardware** | Mindestens 8 GB RAM (16 GB empfohlen); AVX2-fähige CPU oder eine unterstützte GPU |
| **Betriebssystem** | Windows 10/11 (x64/ARM), Windows Server 2025 oder macOS 13+ |
| **Foundry Local CLI** | Installation über `winget install Microsoft.FoundryLocal` (Windows) oder `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Details im [Einstiegsleitfaden](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Programmiersprachenlaufzeit** | **Python 3.9+** und/oder **.NET 9.0+** und/oder **Node.js 18+** |
| **Git** | Zum Klonen dieses Repositories |

---

## Erste Schritte

```bash
# 1. Klonen Sie das Repository
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Überprüfen Sie, ob Foundry Local installiert ist
foundry model list              # Verfügbare Modelle auflisten
foundry model run phi-3.5-mini  # Starten Sie einen interaktiven Chat

# 3. Wählen Sie Ihre Sprachspur (siehe Teil 2 Labor für die vollständige Einrichtung)
```

| Sprache | Schnellstart |
|---------|--------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshop-Teile

### Teil 1: Einstieg in Foundry Local

**Lab-Anleitung:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Was ist Foundry Local und wie es funktioniert
- Installation der CLI unter Windows und macOS
- Modelle erkunden – auflisten, herunterladen, ausführen
- Modell-Aliase und dynamische Ports verstehen

---

### Teil 2: Foundry Local SDK im Detail

**Lab-Anleitung:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Warum man für die Anwendungsentwicklung die SDK statt der CLI nutzt
- Vollständige SDK API-Referenz für Python, JavaScript und C#
- Service-Management, Katalogdurchsuchung, Modelllebenszyklus (Download, Laden, Entladen)
- Schnellstart-Patterns: Python-Konstruktor-Bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` Metadaten, Aliase und hardwareoptimale Modellauswahl

---

### Teil 3: SDKs und APIs

**Lab-Anleitung:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Verbindung zu Foundry Local aus Python, JavaScript und C#
- Verwendung des Foundry Local SDK zur programmatischen Steuerung des Services
- Streaming von Chat-Antworten über die OpenAI-kompatible API
- SDK Methodenreferenz für jede Sprache

**Codebeispiele:**

| Sprache | Datei | Beschreibung |
|---------|-------|--------------|
| Python | `python/foundry-local.py` | Basis-Streaming-Chat |
| C# | `csharp/BasicChat.cs` | Streaming-Chat mit .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming-Chat mit Node.js |

---

### Teil 4: Retrieval-Augmented Generation (RAG)

**Lab-Anleitung:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Was ist RAG und warum ist es wichtig
- Aufbau einer In-Memory Wissensbasis
- Schlagwort-basierte Suche mit Bewertung
- Komposition geerdeter Systems-Prompts
- Ausführen einer kompletten RAG-Pipeline lokal

**Codebeispiele:**

| Sprache | Datei |
|---------|-------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Teil 5: KI-Agenten bauen

**Lab-Anleitung:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Was ist ein KI-Agent (gegenüber einem reinen LLM-Aufruf)
- Das `ChatAgent`-Pattern und das Microsoft Agent Framework
- Systemanweisungen, Personas und Mehrfachdialoge
- Strukturierte Ausgabe (JSON) von Agenten

**Codebeispiele:**

| Sprache | Datei | Beschreibung |
|---------|-------|--------------|
| Python | `python/foundry-local-with-agf.py` | Einzelner Agent mit Agent Framework |
| C# | `csharp/SingleAgent.cs` | Einzelner Agent (ChatAgent-Pattern) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Einzelner Agent (ChatAgent-Pattern) |

---

### Teil 6: Multi-Agent-Workflows

**Lab-Anleitung:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Multi-Agent-Pipelines: Researcher → Writer → Editor
- Sequenzielle Orchestrierung und Feedback-Schleifen
- Gemeinsame Konfiguration und strukturierte Übergaben
- Eigene Multi-Agent-Workflows entwerfen

**Codebeispiele:**

| Sprache | Datei | Beschreibung |
|---------|-------|--------------|
| Python | `python/foundry-local-multi-agent.py` | Drei-Agenten-Pipeline |
| C# | `csharp/MultiAgent.cs` | Drei-Agenten-Pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Drei-Agenten-Pipeline |

---

### Teil 7: Zava Creative Writer – Abschlussanwendung

**Lab-Anleitung:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Eine produktionsreife Multi-Agent-App mit 4 spezialisierten Agenten
- Sequenzielle Pipeline mit evaluatorengestützten Feedback-Schleifen
- Streaming-Ausgabe, Produktsuche im Katalog, strukturierte JSON-Übergaben
- Vollständige Implementierung in Python (FastAPI), JavaScript (Node.js CLI) und C# (.NET Konsolen-App)

**Codebeispiele:**

| Sprache | Verzeichnis | Beschreibung |
|---------|-------------|--------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI Webservice mit Orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI-Anwendung |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 Konsolenanwendung |

---

### Teil 8: Evaluation-gesteuerte Entwicklung

**Lab-Anleitung:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Systematisches Evaluierungsframework für KI-Agenten mit Gold-Datensätzen aufbauen
- Regelbasierte Prüfungen (Länge, Schlüsselwortabdeckung, verbotene Begriffe) + LLM-als-Richter Punktauswertung
- Vergleich von Prompt-Varianten mit aggregierten Scorecards
- Das Zava Editor Agent Pattern aus Teil 7 in eine Offline-Test-Suite erweitern
- Python-, JavaScript- und C#-Tracks

**Codebeispiele:**

| Sprache | Datei | Beschreibung |
|---------|-------|--------------|
| Python | `python/foundry-local-eval.py` | Evaluierungsframework |
| C# | `csharp/AgentEvaluation.cs` | Evaluierungsframework |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evaluierungsframework |

---

### Teil 9: Sprachtranskription mit Whisper

**Lab-Anleitung:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Sprach-zu-Text-Transkription mit lokal laufendem OpenAI Whisper
- Datenschutzorientierte Audioverarbeitung – Audio verlässt nie Ihr Gerät
- Python-, JavaScript- und C#-Tracks mit `client.audio.transcriptions.create()` (Python/JS) und `AudioClient.TranscribeAudioAsync()` (C#)
- Enthält Zava-Themen-Audiodateien für praktische Übungen

**Codebeispiele:**

| Sprache | Datei | Beschreibung |
|---------|-------|--------------|
| Python | `python/foundry-local-whisper.py` | Whisper Sprachtranskription |
| C# | `csharp/WhisperTranscription.cs` | Whisper Sprachtranskription |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper Sprachtranskription |

> **Hinweis:** Dieses Labor verwendet das **Foundry Local SDK**, um das Whisper Modell programmatisch herunterzuladen und zu laden, und sendet dann Audio zur Transkription an den lokalen OpenAI-kompatiblen Endpunkt. Das Whisper Modell (`whisper`) ist im Foundry Local Katalog gelistet und läuft vollständig lokal – keine Cloud-API-Schlüssel oder Netzwerkzugriff erforderlich.

---

### Teil 10: Nutzung von eigenen oder Hugging Face Modellen

**Lab-Anleitung:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilierung von Hugging Face Modellen ins optimierte ONNX-Format mit dem ONNX Runtime GenAI Model Builder
- Hardware-spezifische Kompilierung (CPU, NVIDIA GPU, DirectML, WebGPU) und Quantisierung (int4, fp16, bf16)
- Erstellung von Chat-Vorlagen-Konfigurationsdateien für Foundry Local
- Hinzufügen kompilierter Modelle zum Foundry Local Cache
- Ausführen eigener Modelle über CLI, REST API und OpenAI SDK
- Referenzbeispiel: End-to-End-Kompilierung von Qwen/Qwen3-0.6B

---

### Teil 11: Tool-Calling mit lokalen Modellen

**Lab-Anleitung:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Lokale Modelle zum Aufrufen externer Funktionen befähigen (Tool-/Funktionsaufrufe)
- Definition von Werkzeug-Schemas im OpenAI Function-Calling-Format
- Umgang mit Multi-Turn-Konversationen für Tool-Calls
- Lokale Ausführung von Tool-Aufrufen und Rückgabe der Ergebnisse zum Modell
- Auswahl des passenden Modells für Tool-Calling-Szenarien (Qwen 2.5, Phi-4-mini)
- Nutzung des nativen `ChatClient` der SDK für Tool-Calling (JavaScript)

**Codebeispiele:**

| Sprache | Datei | Beschreibung |
|---------|-------|--------------|
| Python | `python/foundry-local-tool-calling.py` | Tool-Calling mit Wetter-/Bevölkerungstools |
| C# | `csharp/ToolCalling.cs` | Tool-Calling mit .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool-Calling mit ChatClient |

---

### Teil 12: Web-UI für den Zava Creative Writer erstellen

**Lab-Anleitung:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Browserbasierte Frontend-Ergänzung für den Zava Creative Writer
- Gemeinsame UI-Auslieferung aus Python (FastAPI), JavaScript (Node.js HTTP) und C# (ASP.NET Core)
- Nutzung von Streaming NDJSON im Browser mit Fetch API und ReadableStream
- Live-Agent Status-Badges und Echtzeit-Text-Streaming von Artikeln

**Code (gemeinsame UI):**

| Datei | Beschreibung |
|-------|--------------|
| `zava-creative-writer-local/ui/index.html` | Seitenlayout |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Stream-Reader und DOM-Aktualisierungslogik |

**Backend-Ergänzungen:**

| Sprache | Datei | Beschreibung |
|---------|-------|--------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Aktualisiert zur Auslieferung der statischen UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Neuer HTTP-Server, der den Orchestrator umhüllt |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Neues ASP.NET Core Minimal-API-Projekt |

---

### Teil 13: Workshop abgeschlossen
**Lab-Anleitung:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Zusammenfassung von allem, was Sie in allen 12 Teilen aufgebaut haben
- Weitere Ideen zur Erweiterung Ihrer Anwendungen
- Links zu Ressourcen und Dokumentation

---

## Projektstruktur

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## Ressourcen

| Ressource | Link |
|----------|------|
| Foundry Local Website | [foundrylocal.ai](https://foundrylocal.ai) |
| Modellkatalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Einstiegshilfe | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referenz | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Lizenz

Dieses Workshop-Material wird zu Ausbildungszwecken bereitgestellt.

---

**Viel Erfolg beim Bauen! 🚀**