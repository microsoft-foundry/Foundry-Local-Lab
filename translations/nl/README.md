<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Bouw AI-apps op het apparaat

Een hands-on workshop voor het uitvoeren van taalmodellen op je eigen machine en het bouwen van intelligente toepassingen met [Foundry Local](https://foundrylocal.ai) en het [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Wat is Foundry Local?** Foundry Local is een lichtgewicht runtime waarmee je taalmodellen volledig op je eigen hardware kunt downloaden, beheren en bedienen. Het biedt een **OpenAI-compatibele API**, zodat elk hulpmiddel of SDK dat OpenAI spreekt verbinding kan maken - geen cloudaccount nodig.

---

## Leerdoelen

Aan het einde van deze workshop kun je:

| # | Doel |
|---|-----------|
| 1 | Foundry Local installeren en modellen beheren met de CLI |
| 2 | De Foundry Local SDK API beheersen voor programmatisch modelbeheer |
| 3 | Verbinden met de lokale inferentieserver met behulp van de Python-, JavaScript- en C# SDK's |
| 4 | Een Retrieval-Augmented Generation (RAG) pipeline bouwen die antwoorden baseert op je eigen data |
| 5 | AI-agenten creëren met persistente instructies en persoonlijkheden |
| 6 | Multi-agent workflows orkestreren met feedback loops |
| 7 | Een productie-waardige capstone-app verkennen - de Zava Creative Writer |
| 8 | Evaluatiekaders bouwen met gouden datasets en LLM-als-rechter scoring |
| 9 | Audio transcriberen met Whisper - spraak-naar-tekst op het apparaat met de Foundry Local SDK |
| 10 | Aangepaste of Hugging Face modellen compileren en uitvoeren met ONNX Runtime GenAI en Foundry Local |
| 11 | Lokale modellen functies laten aanroepen via het tool-calling patroon |
| 12 | Een browser-gebaseerde UI bouwen voor de Zava Creative Writer met realtime streaming |

---

## Vereisten

| Vereiste | Details |
|-------------|---------|
| **Hardware** | Minimaal 8 GB RAM (16 GB aanbevolen); AVX2-compatibele CPU of een ondersteunde GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025 of macOS 13+ |
| **Foundry Local CLI** | Installeren via `winget install Microsoft.FoundryLocal` (Windows) of `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Zie de [startgids](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) voor details. |
| **Taalruntime** | **Python 3.9+** en/of **.NET 9.0+** en/of **Node.js 18+** |
| **Git** | Voor het klonen van deze repository |

---

## Aan de slag

```bash
# 1. Clone de repository
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Controleer of Foundry Local is geïnstalleerd
foundry model list              # Lijst beschikbare modellen
foundry model run phi-3.5-mini  # Start een interactieve chat

# 3. Kies je taalcursus (zie Deel 2 lab voor volledige setup)
```

| Taal | Snelstart |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshop Onderdelen

### Deel 1: Aan de slag met Foundry Local

**Lab-gids:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Wat is Foundry Local en hoe het werkt
- De CLI installeren op Windows en macOS
- Modellen verkennen - lijst, downloaden, uitvoeren
- Model-aliases en dynamische poorten begrijpen

---

### Deel 2: Foundry Local SDK Deep Dive

**Lab-gids:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Waarom de SDK gebruiken boven de CLI voor applicatieontwikkeling
- Volledige SDK API-referentie voor Python, JavaScript en C#
- Servicemanagement, catalogusverkenning, model lifecycle (download, laden, ontladen)
- Quick-start patronen: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, aliassen en hardware-optimale modelselectie

---

### Deel 3: SDK's en API's

**Lab-gids:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Verbinden met Foundry Local vanuit Python, JavaScript en C#
- Het programma matig beheren van de service met de Foundry Local SDK
- Streaming chatcompleties via de OpenAI-compatibele API
- SDK methode-referentie per taal

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Basis streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat met .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat met Node.js |

---

### Deel 4: Retrieval-Augmented Generation (RAG)

**Lab-gids:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Wat is RAG en waarom het belangrijk is
- Een in-memory kennisbank bouwen
- Zoekwoorden-overlap retrieval met scoring
- Samengestelde gegronde systeem prompts
- Een complete RAG pipeline lokaal uitvoeren

**Codevoorbeelden:**

| Taal | Bestand |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Deel 5: AI-Agenten bouwen

**Lab-gids:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Wat is een AI-agent (vs. een ruwe LLM-aanroep)
- Het `ChatAgent` patroon en het Microsoft Agent Framework
- Systeeminstructies, persoonlijkheden en meerbeurten-gesprekken
- Gestructureerde output (JSON) van agenten

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Enkelvoudige agent met Agent Framework |
| C# | `csharp/SingleAgent.cs` | Enkelvoudige agent (ChatAgent patroon) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Enkelvoudige agent (ChatAgent patroon) |

---

### Deel 6: Multi-Agent Workflows

**Lab-gids:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Multi-agent pipelines: Onderzoeker → Schrijver → Redacteur
- Sequentiële orkestratie en feedback loops
- Gedeelde configuratie en gestructureerde overdrachten
- Ontwerp je eigen multi-agent workflow

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Drie-agent pipeline |
| C# | `csharp/MultiAgent.cs` | Drie-agent pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Drie-agent pipeline |

---

### Deel 7: Zava Creative Writer - Capstone Applicatie

**Lab-gids:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Een productie-waardige multi-agent app met 4 gespecialiseerde agenten
- Sequentiële pipeline met evaluator-gedreven feedback loops
- Streaming output, productcatalogus zoeken, gestructureerde JSON-overdrachten
- Volledige implementatie in Python (FastAPI), JavaScript (Node.js CLI) en C# (.NET console)

**Codevoorbeelden:**

| Taal | Map | Beschrijving |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI-webservice met orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI-applicatie |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 console applicatie |

---

### Deel 8: Evaluatie-gestuurde Ontwikkeling

**Lab-gids:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Bouw een systematisch evaluatiekader voor AI-agenten met gouden datasets
- Regels-gebaseerde controles (lengte, zoekwoorddekkingsgebied, verboden termen) + LLM-als-rechter scoring
- Zij-aan-zij vergelijking van promptvarianten met geaggregeerde scorekaarten
- Breidt het Zava Editor agent patroon uit uit Deel 7 tot een offline testpakket
- Tracks in Python, JavaScript en C#

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evaluatiekader |
| C# | `csharp/AgentEvaluation.cs` | Evaluatiekader |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evaluatiekader |

---

### Deel 9: Spraaktranscriptie met Whisper

**Lab-gids:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Spraak-naar-tekst transcriptie met OpenAI Whisper lokaal draaiend
- Privacygerichte audiobewerking - audio verlaat nooit je apparaat
- Tracks in Python, JavaScript en C# met `client.audio.transcriptions.create()` (Python/JS) en `AudioClient.TranscribeAudioAsync()` (C#)
- Inclusief Zava-thema voorbeeld audio-bestanden voor hands-on oefening

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper spraaktranscriptie |
| C# | `csharp/WhisperTranscription.cs` | Whisper spraaktranscriptie |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper spraaktranscriptie |

> **Opmerking:** Deze lab gebruikt de **Foundry Local SDK** om programmatisch het Whisper-model te downloaden en laden, en stuurt vervolgens audio naar de lokale OpenAI-compatibele endpoint voor transcriptie. Het Whisper-model (`whisper`) staat vermeld in de Foundry Local-catalogus en draait volledig op het apparaat - geen cloud API-sleutels of netwerktoegang vereist.

---

### Deel 10: Aangepaste of Hugging Face Modellen gebruiken

**Lab-gids:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face modellen compileren naar geoptimaliseerd ONNX-formaat met de ONNX Runtime GenAI model builder
- Hardware-specifieke compilatie (CPU, NVIDIA GPU, DirectML, WebGPU) en kwantisatie (int4, fp16, bf16)
- Chat-template configuratiebestanden maken voor Foundry Local
- Gecompileerde modellen toevoegen aan de Foundry Local cache
- Aangepaste modellen uitvoeren via de CLI, REST API en OpenAI SDK
- Referentievoorbeeld: Qwen/Qwen3-0.6B compileren end-to-end

---

### Deel 11: Tool Calling met Lokale Modellen

**Lab-gids:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Lokale modellen in staat stellen externe functies aan te roepen (tool/function calling)
- Tool-schema's definiëren met behulp van het OpenAI function-calling formaat
- Het multi-turn tool-calling conversatieproces afhandelen
- Tool-calls lokaal uitvoeren en resultaten teruggeven aan het model
- De juiste modellen kiezen voor tool-calling scenario's (Qwen 2.5, Phi-4-mini)
- Het gebruik van de SDK's native `ChatClient` voor tool calling (JavaScript)

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tool calling met weer-/bevolkingshulpmiddelen |
| C# | `csharp/ToolCalling.cs` | Tool calling met .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool calling met ChatClient |

---

### Deel 12: Een Web UI bouwen voor de Zava Creative Writer

**Lab-gids:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Voeg een browser-gebaseerde frontend toe aan de Zava Creative Writer
- Serveer de gedeelde UI vanuit Python (FastAPI), JavaScript (Node.js HTTP) en C# (ASP.NET Core)
- Streaming NDJSON in de browser consumeren met de Fetch API en ReadableStream
- Live agent-status badges en realtime artikelltekst streaming

**Code (gedeelde UI):**

| Bestand | Beschrijving |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Pagina-indeling |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Stream reader en DOM update-logica |

**Backend toevoegingen:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Bijgewerkt voor het serveren van statische UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nieuwe HTTP-server die de orchestrator omhult |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nieuw minimal API-project in ASP.NET Core |

---

### Deel 13: Workshop voltooid
**Labgids:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Samenvatting van alles wat je hebt gebouwd in alle 12 delen
- Verdere ideeën voor het uitbreiden van je toepassingen
- Links naar bronnen en documentatie

---

## Projectstructuur

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

## Bronnen

| Bron | Link |
|----------|------|
| Foundry Local website | [foundrylocal.ai](https://foundrylocal.ai) |
| Modelcatalogus | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Startersgids | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK-referentie | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licentie

Dit workshopmateriaal wordt aangeboden voor educatieve doeleinden.

---

**Veel bouwplezier! 🚀**