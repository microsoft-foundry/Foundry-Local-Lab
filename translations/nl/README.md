<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Bouw AI-apps op het apparaat

Een praktische workshop voor het uitvoeren van taalmodellen op je eigen machine en het bouwen van intelligente applicaties met [Foundry Local](https://foundrylocal.ai) en het [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Wat is Foundry Local?** Foundry Local is een lichtgewicht runtime waarmee je taalmodellen volledig op je eigen hardware kunt downloaden, beheren en bedienen. Het biedt een **OpenAI-compatibele API** zodat elke tool of SDK die OpenAI ondersteunt kan verbinden - geen cloudaccount vereist.

### 🌐 Meertalige ondersteuning

#### Ondersteund via GitHub Action (Automatisch & Altijd up-to-date)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabisch](../ar/README.md) | [Bengaals](../bn/README.md) | [Bulgaars](../bg/README.md) | [Birmaans (Myanmar)](../my/README.md) | [Chinees (Vereenvoudigd)](../zh-CN/README.md) | [Chinees (Traditioneel, Hong Kong)](../zh-HK/README.md) | [Chinees (Traditioneel, Macau)](../zh-MO/README.md) | [Chinees (Traditioneel, Taiwan)](../zh-TW/README.md) | [Kroatisch](../hr/README.md) | [Tsjechisch](../cs/README.md) | [Deens](../da/README.md) | [Nederlands](./README.md) | [Ests](../et/README.md) | [Fins](../fi/README.md) | [Frans](../fr/README.md) | [Duits](../de/README.md) | [Grieks](../el/README.md) | [Hebreeuws](../he/README.md) | [Hindi](../hi/README.md) | [Hongaars](../hu/README.md) | [Indonesisch](../id/README.md) | [Italiaans](../it/README.md) | [Japans](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Koreaans](../ko/README.md) | [Litouws](../lt/README.md) | [Maleis](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepalees](../ne/README.md) | [Nigeriaans Pidgin](../pcm/README.md) | [Noors](../no/README.md) | [Perzisch (Farsi)](../fa/README.md) | [Pools](../pl/README.md) | [Portugees (Brazilië)](../pt-BR/README.md) | [Portugees (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Roemeens](../ro/README.md) | [Russisch](../ru/README.md) | [Servisch (Cyrillisch)](../sr/README.md) | [Slowaaks](../sk/README.md) | [Sloveens](../sl/README.md) | [Spaans](../es/README.md) | [Swahili](../sw/README.md) | [Zweeds](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turks](../tr/README.md) | [Oekraïens](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamees](../vi/README.md)

> **Liever lokaal klonen?**
>
> Deze repository bevat meer dan 50 taalvertalingen, wat de downloadgrootte aanzienlijk vergroot. Om te klonen zonder vertalingen, gebruik sparse checkout:
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
> Dit geeft je alles wat je nodig hebt om de cursus te voltooien met een veel snellere download.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Leerdoelen

Aan het einde van deze workshop kun je:

| # | Doel |
|---|-----------|
| 1 | Foundry Local installeren en modellen beheren met de CLI |
| 2 | De Foundry Local SDK API beheersen voor programmatisch modelbeheer |
| 3 | Verbinden met de lokale inferentieserver via de Python-, JavaScript- en C# SDK's |
| 4 | Een Retrieval-Augmented Generation (RAG) pijplijn bouwen die antwoorden baseert op je eigen data |
| 5 | AI-agenten maken met persistente instructies en persona's |
| 6 | Multi-agent workflows orkestreren met feedback loops |
| 7 | Verken een productie-capstone app - de Zava Creative Writer |
| 8 | Evaluatiekaders bouwen met gouden datasets en LLM-als-rechter scoresystemen |
| 9 | Audio transcriberen met Whisper - spraak-naar-tekst op het apparaat via de Foundry Local SDK |
| 10 | Aangepaste of Hugging Face modellen compileren en uitvoeren met ONNX Runtime GenAI en Foundry Local |
| 11 | Lokale modellen toestaan externe functies aan te roepen via het tool-calling patroon |
| 12 | Een browser-gebaseerde UI bouwen voor de Zava Creative Writer met realtime streaming |

---

## Vereisten

| Vereiste | Details |
|-------------|---------|
| **Hardware** | Minimaal 8 GB RAM (16 GB aanbevolen); AVX2-compatibele CPU of ondersteunde GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, of macOS 13+ |
| **Foundry Local CLI** | Installeren via `winget install Microsoft.FoundryLocal` (Windows) of `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Zie de [startgids](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) voor details. |
| **Language runtime** | **Python 3.9+** en/of **.NET 9.0+** en/of **Node.js 18+** |
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

# 3. Kies je taaltraject (zie deel 2 lab voor volledige setup)
```

| Taal | Snelstart |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshopdelen

### Deel 1: Aan de slag met Foundry Local

**Lab gids:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Wat is Foundry Local en hoe het werkt
- De CLI installeren op Windows en macOS
- Modellen verkennen - opsommen, downloaden, uitvoeren
- Begrip van model-aliases en dynamische poorten

---

### Deel 2: Diepgaande Foundry Local SDK

**Lab gids:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Waarom de SDK gebruiken boven de CLI voor applicatieontwikkeling
- Volledige SDK API-referentie voor Python, JavaScript, en C#
- Servicemanagement, catalogusverkenning, modellevenscyclus (download, laden, ontladen)
- Snelstartpatronen: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, aliassen, en hardwareoptimale modelselectie

---

### Deel 3: SDK's en API's

**Lab gids:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Verbinden met Foundry Local vanuit Python, JavaScript, en C#
- De Foundry Local SDK gebruiken om de service programmatisch te beheren
- Streaming chat-completions via de OpenAI-compatibele API
- SDK-methode referentie per taal

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Basis streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat met .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat met Node.js |

---

### Deel 4: Retrieval-Augmented Generation (RAG)

**Lab gids:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Wat is RAG en waarom het belangrijk is
- Een kennisbasis in geheugen bouwen
- Keyword-overlap retrieval met scoring
- Componeren van gefundeerde systeem-prompts
- Een volledige RAG-pijplijn op het apparaat uitvoeren

**Codevoorbeelden:**

| Taal | Bestand |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Deel 5: AI-Agenten bouwen

**Lab gids:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Wat is een AI-agent (tegenover een ruwe LLM-aanroep)
- Het `ChatAgent` patroon en het Microsoft Agent Framework
- Systeem-instructies, persona's en multi-turn gesprekken
- Gestructureerde output (JSON) van agenten

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Enkele agent met Agent Framework |
| C# | `csharp/SingleAgent.cs` | Enkele agent (ChatAgent patroon) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Enkele agent (ChatAgent patroon) |

---

### Deel 6: Multi-Agent Workflows

**Lab gids:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Multi-agent pijplijnen: Researcher → Writer → Editor
- Sequentiële orkestratie en feedback loops
- Gedeelde configuratie en gestructureerde overdrachten
- Ontwerp je eigen multi-agent workflow

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Drie-agent pijplijn |
| C# | `csharp/MultiAgent.cs` | Drie-agent pijplijn |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Drie-agent pijplijn |

---

### Deel 7: Zava Creative Writer - Capstone Applicatie

**Lab gids:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Een productie-stijl multi-agent app met 4 gespecialiseerde agenten
- Sequentiële pijplijn met evaluator-gedreven feedback loops
- Streaming output, productcatalogus zoeken, gestructureerde JSON overdrachten
- Volledige implementatie in Python (FastAPI), JavaScript (Node.js CLI), en C# (.NET console)

**Codevoorbeelden:**

| Taal | Map | Beschrijving |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI webservice met orkestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI-applicatie |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 console-applicatie |

---

### Deel 8: Evaluatiegestuurde Ontwikkeling

**Lab gids:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Bouw een systematisch evaluatiekader voor AI-agenten met gouden datasets
- Regels-gebaseerde controles (lengte, keyword-dekking, verboden termen) + LLM-als-rechter score
- Zij-aan-zij vergelijking van promptvarianten met geaggregeerde scorekaarten
- Breidt het Zava Editor agent-patroon uit uit Deel 7 tot een offline testsuite
- Python-, JavaScript- en C#-trajecten

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evaluatiekader |
| C# | `csharp/AgentEvaluation.cs` | Evaluatiekader |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evaluatiekader |

---

### Deel 9: Stemtranscriptie met Whisper

**Lab gids:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Spraak-naar-tekst transcriptie met OpenAI Whisper die lokaal draait
- Privacygerichte audiobewerking - audio verlaat uw apparaat nooit
- Python-, JavaScript- en C#-voorbeelden met `client.audio.transcriptions.create()` (Python/JS) en `AudioClient.TranscribeAudioAsync()` (C#)
- Inclusief Zava-thema voorbeeld audiobestanden voor praktijkoefeningen

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper spraaktranscriptie |
| C# | `csharp/WhisperTranscription.cs` | Whisper spraaktranscriptie |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper spraaktranscriptie |

> **Opmerking:** Deze lab gebruikt de **Foundry Local SDK** om programmatically het Whisper-model te downloaden en laden, en verzendt vervolgens audio naar de lokale OpenAI-compatibele endpoint voor transcriptie. Het Whisper-model (`whisper`) staat vermeld in de Foundry Local catalogus en draait volledig op het apparaat - geen cloud API-sleutels of netwerktoegang vereist.

---

### Deel 10: Gebruik van Aangepaste of Hugging Face Modellen

**Labgids:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compileren van Hugging Face modellen naar geoptimaliseerd ONNX-formaat met de ONNX Runtime GenAI model builder
- Hardware-specifieke compilatie (CPU, NVIDIA GPU, DirectML, WebGPU) en kwantisatie (int4, fp16, bf16)
- Maken van chat-template configuratiebestanden voor Foundry Local
- Toevoegen van gecompileerde modellen aan de Foundry Local cache
- Lokale uitvoering van aangepaste modellen via de CLI, REST API en OpenAI SDK
- Referentievoorbeeld: end-to-end compilatie van Qwen/Qwen3-0.6B

---

### Deel 11: Tool Calling met Lokale Modellen

**Labgids:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Locatie modellen inschakelen om externe functies aan te roepen (tool/function calling)
- Definieer tool-schema's volgens het OpenAI function-calling formaat
- Afhandelen van meerstaps tool-calling conversatieflow
- Voer toolaanroepen lokaal uit en geef resultaten terug aan het model
- Kies het juiste model voor tool-calling scenario's (Qwen 2.5, Phi-4-mini)
- Gebruik de native `ChatClient` van de SDK voor tool calling (JavaScript)

**Codevoorbeelden:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tool calling met weer-/bevolkingshulpmiddelen |
| C# | `csharp/ToolCalling.cs` | Tool calling met .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool calling met ChatClient |

---

### Deel 12: Bouwen van een Web UI voor de Zava Creative Writer

**Labgids:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Voeg een browsergebaseerde frontend toe aan de Zava Creative Writer
- Serveer de gedeelde UI vanuit Python (FastAPI), JavaScript (Node.js HTTP) en C# (ASP.NET Core)
- Verwerk streaming NDJSON in de browser met de Fetch API en ReadableStream
- Live agent status badges en real-time streaming van artikeltekst

**Code (gedeelde UI):**

| Bestand | Beschrijving |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Pagina-indeling |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Stream reader en DOM update logica |

**Backend aanvullingen:**

| Taal | Bestand | Beschrijving |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Bijgewerkt om statische UI te serveren |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nieuwe HTTP-server die de orchestrator omvat |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nieuw ASP.NET Core minimal API project |

---

### Deel 13: Workshop Voltooid

**Labgids:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Samenvatting van alles wat je hebt gebouwd in alle 12 delen
- Verdere ideeën om je toepassingen uit te breiden
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
| Startgids | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referentie | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licentie

Dit workshopmateriaal wordt aangeboden voor educatieve doeleinden.

---

**Veel bouwplezier! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:  
Dit document is vertaald met behulp van de AI-vertalingsdienst [Co-op Translator](https://github.com/Azure/co-op-translator). Hoewel we streven naar nauwkeurigheid, dient u er rekening mee te houden dat automatische vertalingen fouten of onnauwkeurigheden kunnen bevatten. Het originele document in de oorspronkelijke taal dient als de gezaghebbende bron te worden beschouwd. Voor kritieke informatie wordt professionele menselijke vertaling aanbevolen. Wij zijn niet aansprakelijk voor enige misverstanden of verkeerde interpretaties die voortvloeien uit het gebruik van deze vertaling.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->