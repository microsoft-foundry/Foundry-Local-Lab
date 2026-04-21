<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Byg AI-apps på enheden

En praktisk workshop til at køre sprogmodeller på din egen maskine og bygge intelligente applikationer med [Foundry Local](https://foundrylocal.ai) og [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Hvad er Foundry Local?** Foundry Local er en letvægts-runtime, der lader dig downloade, administrere og betjene sprogmodeller helt på dit eget hardware. Den udstiller en **OpenAI-kompatibel API**, så ethvert værktøj eller SDK, der taler OpenAI, kan forbinde - ingen cloud-konto kræves.

---

## Læringsmål

Når du har gennemført denne workshop, vil du kunne:

| # | Mål |
|---|-----------|
| 1 | Installere Foundry Local og administrere modeller med CLI |
| 2 | Mestre Foundry Local SDK API for programmatisk modeladministration |
| 3 | Forbinde til den lokale inferensserver ved hjælp af Python-, JavaScript- og C#-SDK’er |
| 4 | Bygge en Retrieval-Augmented Generation (RAG) pipeline, der funderer svar i dine egne data |
| 5 | Oprette AI-agenter med vedvarende instruktioner og personaer |
| 6 | Orkestrere multi-agent workflows med feedbackloops |
| 7 | Udforske en produktionsklar capstone-app - Zava Creative Writer |
| 8 | Bygge evalueringsrammer med gulddatasæt og LLM-as-dommer scoring |
| 9 | Transskribere lyd med Whisper - tale-til-tekst på enheden ved hjælp af Foundry Local SDK |
| 10 | Kompilere og køre brugerdefinerede eller Hugging Face-modeller med ONNX Runtime GenAI og Foundry Local |
| 11 | Muliggøre lokale modeller til at kalde eksterne funktioner med værktøj-kaldsmønsteret |
| 12 | Bygge et browserbaseret UI til Zava Creative Writer med streaming i realtid |

---

## Forudsætninger

| Krav | Detaljer |
|-------------|---------|
| **Hardware** | Minimum 8 GB RAM (16 GB anbefales); AVX2-kompatibel CPU eller understøttet GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025 eller macOS 13+ |
| **Foundry Local CLI** | Installér via `winget install Microsoft.FoundryLocal` (Windows) eller `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Se [kom godt i gang-guiden](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) for detaljer. |
| **Sprogruntime** | **Python 3.9+** og/eller **.NET 9.0+** og/eller **Node.js 18+** |
| **Git** | Til kloning af dette repository |

---

## Kom godt i gang

```bash
# 1. Klon arkivet
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Bekræft at Foundry Local er installeret
foundry model list              # List tilgængelige modeller
foundry model run phi-3.5-mini  # Start en interaktiv chat

# 3. Vælg dit sprogspor (se Del 2 laboratoriet for fuld opsætning)
```

| Sprog | Hurtigstart |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshop dele

### Del 1: Kom godt i gang med Foundry Local

**Lab-guide:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Hvad er Foundry Local, og hvordan det fungerer
- Installation af CLI på Windows og macOS
- Udforskning af modeller - liste, download, kørsel
- Forståelse af modelaliaser og dynamiske porte

---

### Del 2: Foundry Local SDK Dybt dyk

**Lab-guide:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Hvorfor bruge SDK frem for CLI til applikationsudvikling
- Fuld SDK API-reference for Python, JavaScript og C#
- Serviceadministration, kataloggennemgang, modellivscyklus (download, load, unload)
- Hurtigstartsmønstre: Python-konstruktør bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, aliaser og hardware-optimal modelvalg

---

### Del 3: SDK'er og APIs

**Lab-guide:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Forbindelse til Foundry Local fra Python, JavaScript og C#
- Brug af Foundry Local SDK til programmatisk serviceadministration
- Streaming chat-kompletteringer via OpenAI-kompatibel API
- SDK-metodereference for hvert sprog

**Kodeeksempler:**

| Sprog | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Grundlæggende streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat med .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat med Node.js |

---

### Del 4: Retrieval-Augmented Generation (RAG)

**Lab-guide:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Hvad er RAG, og hvorfor det er vigtigt
- Opbygning af en in-memory vidensbase
- Nøgleords-overlap retrieval med scoring
- Sammensætning af funderede systemprompter
- Kørsel af komplet RAG-pipeline på enheden

**Kodeeksempler:**

| Sprog | Fil |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Del 5: Bygning af AI-agenter

**Lab-guide:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Hvad er en AI-agent (vs. et råt LLM-kald)
- `ChatAgent`-mønstret og Microsoft Agent Framework
- Systeminstruktioner, personaer og flersporede samtaler
- Struktureret output (JSON) fra agenter

**Kodeeksempler:**

| Sprog | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Enkelt agent med Agent Framework |
| C# | `csharp/SingleAgent.cs` | Enkelt agent (ChatAgent-mønster) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Enkelt agent (ChatAgent-mønster) |

---

### Del 6: Multi-agent Workflows

**Lab-guide:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Multi-agent pipelines: Forsker → Forfatter → Redaktør
- Sekventiel orkestrering og feedbackloops
- Fælles konfiguration og strukturerede overdragelser
- Design din egen multi-agent workflow

**Kodeeksempler:**

| Sprog | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Tre-agent pipeline |
| C# | `csharp/MultiAgent.cs` | Tre-agent pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Tre-agent pipeline |

---

### Del 7: Zava Creative Writer - Capstone-applikation

**Lab-guide:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- En produktionsstil multi-agent app med 4 specialiserede agenter
- Sekventiel pipeline med evaluator-drevne feedbackloops
- Streaming output, produktkatalogsøgning, strukturerede JSON-overdragelser
- Fuld implementering i Python (FastAPI), JavaScript (Node.js CLI) og C# (.NET console)

**Kodeeksempler:**

| Sprog | Mappe | Beskrivelse |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI webservice med orkestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI-applikation |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsolapplikation |

---

### Del 8: Evalueringsstyret Udvikling

**Lab-guide:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Byg en systematisk evalueringsramme for AI-agenter med gulddatasæt
- Regelbaserede checks (længde, nøgleordsdækning, forbudte termer) + LLM-as-dommer scoring
- Side-om-side sammenligning af prompt-varianter med samlede scorecards
- Udvider Zava Editor agentmønstret fra Del 7 til et offline test-suite
- Python-, JavaScript- og C#-spor

**Kodeeksempler:**

| Sprog | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evalueringsramme |
| C# | `csharp/AgentEvaluation.cs` | Evalueringsramme |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evalueringsramme |

---

### Del 9: Tale-transskription med Whisper

**Lab-guide:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Tale-til-tekst transskription ved hjælp af OpenAI Whisper kørt lokalt
- Privatlivs-først lydbehandling - lyd forlader aldrig din enhed
- Python-, JavaScript- og C#-spor med `client.audio.transcriptions.create()` (Python/JS) og `AudioClient.TranscribeAudioAsync()` (C#)
- Indeholder Zava-tema prøvelydfiler til praktisk øvelse

**Kodeeksempler:**

| Sprog | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper tale-transskription |
| C# | `csharp/WhisperTranscription.cs` | Whisper tale-transskription |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper tale-transskription |

> **Bemærk:** Dette laboratorium bruger **Foundry Local SDK** til programmatisk at downloade og loade Whisper-modellen, og sender derefter lyd til den lokale OpenAI-kompatible endpoint til transskription. Whisper-modellen (`whisper`) er opført i Foundry Local-kataloget og kører helt på enheden - ingen cloud API-nøgler eller netværksadgang kræves.

---

### Del 10: Brug af brugerdefinerede eller Hugging Face modeller

**Lab-guide:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilering af Hugging Face-modeller til optimeret ONNX-format ved hjælp af ONNX Runtime GenAI model builder
- Hardware-specifik kompilering (CPU, NVIDIA GPU, DirectML, WebGPU) og kvantisering (int4, fp16, bf16)
- Oprettelse af chat-template konfigurationsfiler til Foundry Local
- Tilføjelse af kompilerede modeller til Foundry Local-cachen
- Kørsel af brugerdefinerede modeller via CLI, REST API og OpenAI SDK
- Referencemodel: kompilering af Qwen/Qwen3-0.6B fra ende til anden

---

### Del 11: Værktøjskald med lokale modeller

**Lab-guide:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Muliggøre lokale modeller til at kalde eksterne funktioner (værktøj/funktionskald)
- Definere værktøjs-skemaer med OpenAI funktionskald-format
- Håndtere den flersporede værktøjskalds-konversationsflow
- Udføre værktøjskald lokalt og returnere resultater til modellen
- Vælge den rette model til værktøjskaldsscenarier (Qwen 2.5, Phi-4-mini)
- Brug SDK’ets native `ChatClient` til værktøjskald (JavaScript)

**Kodeeksempler:**

| Sprog | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Værktøjskald med vejrudsigts-/befolkningsværktøjer |
| C# | `csharp/ToolCalling.cs` | Værktøjskald med .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Værktøjskald med ChatClient |

---

### Del 12: Byg et web-UI til Zava Creative Writer

**Lab-guide:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Tilføj et browserbaseret front-end til Zava Creative Writer
- Server det delte UI fra Python (FastAPI), JavaScript (Node.js HTTP) og C# (ASP.NET Core)
- Forbrug streaming NDJSON i browseren med Fetch API og ReadableStream
- Live agent-status badges og tekststreaming af artikler i realtid

**Kode (delt UI):**

| Fil | Beskrivelse |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Sidelayout |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Stream-læser og DOM-opdateringslogik |

**Backend-tilføjelser:**

| Sprog | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Opdateret til at servere statisk UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Ny HTTP-server, der pakker orkestratoren |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nyt ASP.NET Core minimal API-projekt |

---

### Del 13: Workshop færdiggjort
**Lab guide:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Oversigt over alt det, du har bygget gennem alle 12 dele
- Yderligere idéer til at udvide dine applikationer
- Links til ressourcer og dokumentation

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

## Ressourcer

| Ressource | Link |
|----------|------|
| Foundry Local hjemmeside | [foundrylocal.ai](https://foundrylocal.ai) |
| Model katalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Kom godt i gang guide | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Reference | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licens

Dette workshopmateriale stilles til rådighed til uddannelsesformål.

---

**God byggeglæde! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokument er oversat ved hjælp af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi bestræber os på nøjagtighed, bedes du være opmærksom på, at automatiserede oversættelser kan indeholde fejl eller unøjagtigheder. Det originale dokument på dets oprindelige sprog skal betragtes som den autoritative kilde. For kritisk information anbefales professionel menneskelig oversættelse. Vi påtager os intet ansvar for eventuelle misforståelser eller fejltolkninger, der opstår som følge af brugen af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->