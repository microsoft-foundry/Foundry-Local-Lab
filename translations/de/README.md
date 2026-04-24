<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop – KI-Anwendungen direkt auf dem Gerät erstellen

Ein praxisorientierter Workshop, um Sprachmodelle auf Ihrem eigenen Gerät auszuführen und intelligente Anwendungen mit [Foundry Local](https://foundrylocal.ai) und dem [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) zu entwickeln.

> **Was ist Foundry Local?** Foundry Local ist eine leichtgewichtige Laufzeitumgebung, die es Ihnen ermöglicht, Sprachmodelle vollständig auf Ihrer eigenen Hardware herunterzuladen, zu verwalten und auszuführen. Es stellt eine **OpenAI-kompatible API** bereit, sodass jedes Tool oder SDK, das OpenAI unterstützt, eine Verbindung herstellen kann – kein Cloud-Account erforderlich.

### 🌐 Mehrsprachige Unterstützung

#### Unterstützt via GitHub Action (Automatisiert & stets aktuell)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabisch](../ar/README.md) | [Bengalisch](../bn/README.md) | [Bulgarisch](../bg/README.md) | [Birmanisch (Myanmar)](../my/README.md) | [Chinesisch (vereinfacht)](../zh-CN/README.md) | [Chinesisch (traditionell, Hongkong)](../zh-HK/README.md) | [Chinesisch (traditionell, Macau)](../zh-MO/README.md) | [Chinesisch (traditionell, Taiwan)](../zh-TW/README.md) | [Kroatisch](../hr/README.md) | [Tschechisch](../cs/README.md) | [Dänisch](../da/README.md) | [Niederländisch](../nl/README.md) | [Estnisch](../et/README.md) | [Finnisch](../fi/README.md) | [Französisch](../fr/README.md) | [Deutsch](./README.md) | [Griechisch](../el/README.md) | [Hebräisch](../he/README.md) | [Hindi](../hi/README.md) | [Ungarisch](../hu/README.md) | [Indonesisch](../id/README.md) | [Italienisch](../it/README.md) | [Japanisch](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Koreanisch](../ko/README.md) | [Litauisch](../lt/README.md) | [Malaiisch](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepalesisch](../ne/README.md) | [Nigerianisches Pidgin](../pcm/README.md) | [Norwegisch](../no/README.md) | [Persisch (Farsi)](../fa/README.md) | [Polnisch](../pl/README.md) | [Portugiesisch (Brasilien)](../pt-BR/README.md) | [Portugiesisch (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Rumänisch](../ro/README.md) | [Russisch](../ru/README.md) | [Serbisch (Kyrillisch)](../sr/README.md) | [Slowakisch](../sk/README.md) | [Slowenisch](../sl/README.md) | [Spanisch](../es/README.md) | [Suaheli](../sw/README.md) | [Schwedisch](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Türkisch](../tr/README.md) | [Ukrainisch](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamesisch](../vi/README.md)

> **Bevorzugen Sie einen lokalen Klon?**
>
> Dieses Repository enthält mehr als 50 Sprachübersetzungen, was die Downloadgröße erheblich erhöht. Um ohne Übersetzungen zu klonen, verwenden Sie Sparse Checkout:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> Dies liefert Ihnen alles, was Sie benötigen, um den Kurs mit deutlich schnellerem Download abzuschließen.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Lernziele

Am Ende dieses Workshops werden Sie in der Lage sein:

| # | Ziel |
|---|-----------|
| 1 | Foundry Local installieren und Modelle mit der CLI verwalten |
| 2 | Die Foundry Local SDK API für programmatische Modellverwaltung meistern |
| 3 | Verbindung zum lokalen Inferenzserver über die Python-, JavaScript- und C#-SDKs herstellen |
| 4 | Eine Retrieval-Augmented-Generation (RAG) Pipeline bauen, die Antworten auf eigene Daten stützt |
| 5 | KI-Agenten mit persistierenden Anweisungen und Personas erstellen |
| 6 | Multi-Agent-Arbeitsabläufe mit Feedback-Schleifen orchestrieren |
| 7 | Eine produktionsreife Abschlussanwendung erkunden – den Zava Creative Writer |
| 8 | Bewertungsframeworks mit Gold-Datensätzen und LLM-as-Judge-Bewertungen erstellen |
| 9 | Audio mit Whisper transkribieren – Sprache-zu-Text direkt auf dem Gerät mit dem Foundry Local SDK |
| 10 | Eigene oder Hugging Face Modelle mit ONNX Runtime GenAI und Foundry Local kompilieren und ausführen |
| 11 | Lokalen Modellen erlauben, externe Funktionen mit dem Tool-Calling-Muster aufzurufen |
| 12 | Eine browserbasierte UI für den Zava Creative Writer mit Echtzeit-Streaming erstellen |

---

## Voraussetzungen

| Voraussetzung | Details |
|-------------|---------|
| **Hardware** | Mindestens 8 GB RAM (16 GB empfohlen); AVX2-fähige CPU oder unterstützte GPU |
| **Betriebssystem** | Windows 10/11 (x64/ARM), Windows Server 2025 oder macOS 13+ |
| **Foundry Local CLI** | Installation via `winget install Microsoft.FoundryLocal` (Windows) oder `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Details finden Sie im [Einstiegsleitfaden](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Laufzeitumgebung** | **Python 3.9+** und/oder **.NET 9.0+** und/oder **Node.js 18+** |
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

# 3. Wählen Sie Ihre Sprachspur (siehe Teil 2 Labor für vollständige Einrichtung)
```

| Sprache | Schneller Einstieg |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshop-Teile

### Teil 1: Einstieg mit Foundry Local

**Lab-Anleitung:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Was ist Foundry Local und wie es funktioniert
- Installation der CLI unter Windows und macOS
- Modelle erkunden – auflisten, herunterladen, ausführen
- Verständnis von Modell-Aliasen und dynamischen Ports

---

### Teil 2: Foundry Local SDK Deep Dive

**Lab-Anleitung:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Warum die SDK gegenüber der CLI für App-Entwicklung bevorzugt wird
- Vollständige SDK API-Referenz für Python, JavaScript und C#
- Service-Management, Katalog-Durchsicht, Modelllebenszyklus (Download, Laden, Entladen)
- Schnellstart-Patterns: Python-Konstruktor-Bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` Metadaten, Aliase und hardwareoptimale Modellauswahl

---

### Teil 3: SDKs und APIs

**Lab-Anleitung:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Verbindung zu Foundry Local von Python, JavaScript und C# herstellen
- Verwaltung des Dienstes programmatisch mit dem Foundry Local SDK
- Streaming-Chat-Completions über die OpenAI-kompatible API
- SDK Methodenreferenz für jede Sprache

**Code-Beispiele:**

| Sprache | Datei | Beschreibung |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Einfacher Streaming-Chat |
| C# | `csharp/BasicChat.cs` | Streaming-Chat mit .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming-Chat mit Node.js |

---

### Teil 4: Retrieval-Augmented Generation (RAG)

**Lab-Anleitung:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Was ist RAG und warum es wichtig ist
- Aufbau einer In-Memory-Wissensbasis
- Stichwort-Überlappung bei der Abfrage mit Bewertung
- Zusammenstellung fundierter System-Prompts
- Ausführung einer vollständigen RAG-Pipeline direkt auf dem Gerät

**Code-Beispiele:**

| Sprache | Datei |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Teil 5: KI-Agenten erstellen

**Lab-Anleitung:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Was ist ein KI-Agent (im Vergleich zu einem reinen LLM-Aufruf)
- Das `ChatAgent`-Pattern und das Microsoft Agent Framework
- Systemanweisungen, Personas und mehrstufige Dialoge
- Strukturierte Ausgabe (JSON) von Agenten

**Code-Beispiele:**

| Sprache | Datei | Beschreibung |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Einzelner Agent mit Agent Framework |
| C# | `csharp/SingleAgent.cs` | Einzelner Agent (ChatAgent-Pattern) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Einzelner Agent (ChatAgent-Pattern) |

---

### Teil 6: Multi-Agent-Arbeitsabläufe

**Lab-Anleitung:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Multi-Agent-Pipelines: Forscher → Autor → Redakteur
- Sequenzielle Orchestrierung und Feedback-Schleifen
- Gemeinsame Konfiguration und strukturierte Übergabe
- Eigene Multi-Agent-Workflows entwerfen

**Code-Beispiele:**

| Sprache | Datei | Beschreibung |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Drei-Agenten-Pipeline |
| C# | `csharp/MultiAgent.cs` | Drei-Agenten-Pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Drei-Agenten-Pipeline |

---

### Teil 7: Zava Creative Writer – Abschlussanwendung

**Lab-Anleitung:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Eine produktionsreife Multi-Agent-Anwendung mit 4 spezialisierten Agenten
- Sequenzielle Pipeline mit bewertungsgetriebenen Feedback-Schleifen
- Streaming-Ausgabe, Produktsuche im Katalog, strukturierte JSON-Übergaben
- Volle Implementierung in Python (FastAPI), JavaScript (Node.js CLI) und C# (Konsolenanwendung)

**Code-Beispiele:**

| Sprache | Verzeichnis | Beschreibung |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI Webservice mit Orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI-Anwendung |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 Konsolenanwendung |

---

### Teil 8: Bewertungsorientierte Entwicklung

**Lab-Anleitung:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Erstellung eines systematischen Evaluationsframeworks für KI-Agenten mit Gold-Datensätzen
- Regelbasierte Checks (Länge, Stichwortabdeckung, verbotene Begriffe) + LLM-as-Judge-Bewertung
- Nebeneinanderstellung von Prompt-Varianten mit aggregierten Scorecards
- Erweiterung des Zava Editor Agent-Patterns aus Teil 7 zu einem Offline-Test-Toolkit
- Python-, JavaScript- und C#-Spuren

**Code-Beispiele:**

| Sprache | Datei | Beschreibung |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evaluationsframework |
| C# | `csharp/AgentEvaluation.cs` | Evaluationsframework |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evaluationsframework |

---

### Teil 9: Sprachtranskription mit Whisper

**Lab-Anleitung:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Sprach-zu-Text-Transkription mit lokal ausgeführtem OpenAI Whisper
- Datenschutzorientierte Audioverarbeitung – Audio verlässt das Gerät niemals
- Python-, JavaScript- und C#-Beispiele mit `client.audio.transcriptions.create()` (Python/JS) und `AudioClient.TranscribeAudioAsync()` (C#)
- Enthält Zava-Themenbeispiele mit Audiodateien zum praktischen Üben

**Codebeispiele:**

| Sprache | Datei | Beschreibung |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper-Sprachtranskription |
| C# | `csharp/WhisperTranscription.cs` | Whisper-Sprachtranskription |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper-Sprachtranskription |

> **Hinweis:** Dieses Labor verwendet das **Foundry Local SDK**, um das Whisper-Modell programmatisch herunterzuladen und zu laden, und sendet dann Audio zur Transkription an den lokalen OpenAI-kompatiblen Endpunkt. Das Whisper-Modell (`whisper`) ist im Foundry Local-Katalog gelistet und läuft vollständig lokal auf dem Gerät – keine Cloud-API-Schlüssel oder Netzwerkzugang erforderlich.

---

### Teil 10: Verwendung benutzerdefinierter oder Hugging Face-Modelle

**Labormanual:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilierung von Hugging Face-Modellen in optimiertes ONNX-Format mit dem ONNX Runtime GenAI Model Builder
- Gerätespezifische Kompilierung (CPU, NVIDIA GPU, DirectML, WebGPU) und Quantisierung (int4, fp16, bf16)
- Erstellen von Chat-Vorlagenkonfigurationsdateien für Foundry Local
- Hinzufügen kompilierter Modelle zum Foundry Local-Cache
- Ausführen benutzerdefinierter Modelle über CLI, REST API und OpenAI SDK
- Referenzbeispiel: End-to-End-Kompilierung von Qwen/Qwen3-0.6B

---

### Teil 11: Tool-Calling mit lokalen Modellen

**Labormanual:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Ermöglichen, dass lokale Modelle externe Funktionen aufrufen (Tool-/Funktionsaufruf)
- Definieren von Toolschemas im OpenAI-Funktionsaufruf-Format
- Handhabung des mehrstufigen Tool-Calling-Konversationsablaufs
- Ausführen von Tool-Aufrufen lokal und Rückgabe der Ergebnisse an das Modell
- Auswahl des passenden Modells für Tool-Calling-Szenarien (Qwen 2.5, Phi-4-mini)
- Nutzung des SDK-eigenen `ChatClient` für Tool-Calling (JavaScript)

**Codebeispiele:**

| Sprache | Datei | Beschreibung |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tool-Calling mit Wetter-/Bevölkerungstools |
| C# | `csharp/ToolCalling.cs` | Tool-Calling mit .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool-Calling mit ChatClient |

---

### Teil 12: Aufbau einer Web-Benutzeroberfläche für den Zava Creative Writer

**Labormanual:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Hinzufügen eines browserbasierten Frontends zum Zava Creative Writer
- Bereitstellung der gemeinsamen UI über Python (FastAPI), JavaScript (Node.js HTTP) und C# (ASP.NET Core)
- Verwendung von Streaming-NDJSON im Browser mit der Fetch API und ReadableStream
- Live-Agent-Statusanzeigen und Echtzeit-Textstreaming von Artikeln

**Code (gemeinsame UI):**

| Datei | Beschreibung |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Seitenlayout |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Stream-Reader- und DOM-Aktualisierungslogik |

**Backend-Ergänzungen:**

| Sprache | Datei | Beschreibung |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Aktualisiert zur Bereitstellung statischer UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Neuer HTTP-Server, der den Orchestrator umschließt |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Neues ASP.NET Core Minimal-API-Projekt |

---

### Teil 13: Workshop abgeschlossen

**Labormanual:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Zusammenfassung aller in den 12 Teilen erstellten Inhalte
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
| Foundry Local Webseite | [foundrylocal.ai](https://foundrylocal.ai) |
| Modellkatalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Einstiegshilfe | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referenz | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Lizenz

Dieses Workshop-Material wird zu Bildungszwecken bereitgestellt.

---

**Viel Erfolg beim Bauen! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Haftungsausschluss**:  
Dieses Dokument wurde mit dem KI-Übersetzungsdienst [Co-op Translator](https://github.com/Azure/co-op-translator) übersetzt. Obwohl wir Genauigkeit anstreben, beachten Sie bitte, dass automatisierte Übersetzungen Fehler oder Ungenauigkeiten enthalten können. Das Originaldokument in seiner ursprünglichen Sprache ist als maßgebliche Quelle zu betrachten. Für kritische Informationen wird eine professionelle menschliche Übersetzung empfohlen. Wir übernehmen keine Haftung für Missverständnisse oder Fehlinterpretationen, die aus der Verwendung dieser Übersetzung entstehen.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->