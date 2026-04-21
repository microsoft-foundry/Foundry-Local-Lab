<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Bygg AI-appar på enheten

En praktisk workshop för att köra språkmodeller på din egen dator och bygga intelligenta applikationer med [Foundry Local](https://foundrylocal.ai) och [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Vad är Foundry Local?** Foundry Local är en lättviktsruntime som låter dig ladda ner, hantera och köra språkmodeller helt på din egen hårdvara. Det exponerar ett **OpenAI-kompatibelt API** så att verktyg eller SDK:er som talar OpenAI kan ansluta - inget molnkonto krävs.

---

## Inlärningsmål

I slutet av denna workshop kommer du att kunna:

| # | Mål |
|---|-----------|
| 1 | Installera Foundry Local och hantera modeller med CLI |
| 2 | Bemästra Foundry Local SDK API för programmatisk modellhantering |
| 3 | Ansluta till den lokala inferensservern med Python, JavaScript och C# SDK:er |
| 4 | Bygga en Retrieval-Augmented Generation (RAG)-pipeline som grundar svar i dina egna data |
| 5 | Skapa AI-agenter med persistenta instruktioner och personligheter |
| 6 | Orkestrera arbetsflöden med flera agenter med feedbackloopar |
| 7 | Utforska en produktionsfokuserad capstone-app – Zava Creative Writer |
| 8 | Bygga utvärderingsramverk med guld-datasets och LLM-som-domare-poängsättning |
| 9 | Transkribera ljud med Whisper – tal-till-text på enheten med Foundry Local SDK |
| 10 | Kompilera och kör anpassade eller Hugging Face-modeller med ONNX Runtime GenAI och Foundry Local |
| 11 | Möjliggöra att lokala modeller kan anropa externa funktioner med tool-calling-mönstret |
| 12 | Bygga ett webbläsarbaserat UI för Zava Creative Writer med realtidsströmning |

---

## Förutsättningar

| Krav | Detaljer |
|-------------|---------|
| **Hårdvara** | Minst 8 GB RAM (16 GB rekommenderas); AVX2-kompatibel CPU eller ett stödd GPU |
| **Operativsystem** | Windows 10/11 (x64/ARM), Windows Server 2025 eller macOS 13+ |
| **Foundry Local CLI** | Installera via `winget install Microsoft.FoundryLocal` (Windows) eller `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Se [kom igång-guiden](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) för detaljer. |
| **Språkruntimes** | **Python 3.9+** och/eller **.NET 9.0+** och/eller **Node.js 18+** |
| **Git** | För att klona detta repository |

---

## Kom igång

```bash
# 1. Klona arkivet
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verifiera att Foundry Local är installerat
foundry model list              # Lista tillgängliga modeller
foundry model run phi-3.5-mini  # Starta en interaktiv chatt

# 3. Välj ditt språkkurs (se Del 2 laboration för fullständig installation)
```

| Språk | Snabbstart |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshopdelar

### Del 1: Kom igång med Foundry Local

**Labbinstruktion:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Vad är Foundry Local och hur det fungerar
- Installera CLI på Windows och macOS
- Utforska modeller – lista, ladda ner, köra
- Förstå modellalias och dynamiska portar

---

### Del 2: Foundry Local SDK Djupdykning

**Labbinstruktion:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Varför använda SDK över CLI för applikationsutveckling
- Fullständig SDK API-reference för Python, JavaScript och C#
- Tjänstehantering, katalogbläddring, modelllivscykel (ladda ner, ladda, avlasta)
- Snabbstartsmönster: Python-konstruktör bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, alias och hårdvaruoptimerad modellval

---

### Del 3: SDK:er och API:er

**Labbinstruktion:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Ansluta till Foundry Local från Python, JavaScript och C#
- Använda Foundry Local SDK för att hantera tjänsten programmatisk
- Streaming chat completions via det OpenAI-kompatibla API:t
- SDK-metodreferens för varje språk

**Kodexempel:**

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Grundläggande streaming-chat |
| C# | `csharp/BasicChat.cs` | Streaming-chat med .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming-chat med Node.js |

---

### Del 4: Retrieval-Augmented Generation (RAG)

**Labbinstruktion:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Vad är RAG och varför det är viktigt
- Bygga en in-memory kunskapsbas
- Keyword-overlapp-sökning med poängsättning
- Skapa systempromptar med grund
- Köra en komplett RAG-pipeline på enheten

**Kodexempel:**

| Språk | Fil |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Del 5: Bygga AI-agenter

**Labbinstruktion:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Vad är en AI-agent (vs. ett direkt LLM-anrop)
- `ChatAgent`-mönstret och Microsoft Agent Framework
- Systeminstruktioner, personligheter och flerpartssamtal
- Strukturutdata (JSON) från agenter

**Kodexempel:**

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Enskild agent med Agent Framework |
| C# | `csharp/SingleAgent.cs` | Enskild agent (ChatAgent-mönster) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Enskild agent (ChatAgent-mönster) |

---

### Del 6: Arbetsflöden med flera agenter

**Labbinstruktion:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipelines med flera agenter: Researcher → Writer → Editor
- Sekventiell orkestrering och feedbackloopar
- Delad konfiguration och strukturerade överlämningar
- Designa egna arbetsflöden med flera agenter

**Kodexempel:**

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline med tre agenter |
| C# | `csharp/MultiAgent.cs` | Pipeline med tre agenter |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline med tre agenter |

---

### Del 7: Zava Creative Writer – Capstone-applikation

**Labbinstruktion:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- En produktionsliknande multi-agent-app med 4 specialiserade agenter
- Sekventiell pipeline med evaluator-drivna feedbackloopar
- Streaming-utdata, produktkatalogsökning, strukturerade JSON-överlämningar
- Full implementation i Python (FastAPI), JavaScript (Node.js CLI) och C# (.NET console)

**Kodexempel:**

| Språk | Katalog | Beskrivning |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI-webbtjänst med orkestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI-applikation |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsolapplikation |

---

### Del 8: Utvärderingsledd utveckling

**Labbinstruktion:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Bygga ett systematiskt utvärderingsramverk för AI-agenter med guld-datasets
- Reglerbaserade kontroller (längd, sökordstäckning, förbjudna termer) + LLM-som-domare-poängsättning
- Sida vid sida-jämförelse av promptvarianter med aggregerade poängkort
- Utökar Zava Editor agent-mönstret från del 7 till en offlinetestsvit
- Python-, JavaScript- och C#-spår

**Kodexempel:**

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Utvärderingsramverk |
| C# | `csharp/AgentEvaluation.cs` | Utvärderingsramverk |
| JavaScript | `javascript/foundry-local-eval.mjs` | Utvärderingsramverk |

---

### Del 9: Rösttranskribering med Whisper

**Labbinstruktion:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Tal-till-text-transkribering med OpenAI Whisper som körs lokalt
- Sekretessvänlig ljudbearbetning – ljudet lämnar aldrig din enhet
- Python-, JavaScript- och C#-spår med `client.audio.transcriptions.create()` (Python/JS) och `AudioClient.TranscribeAudioAsync()` (C#)
- Inkluderar Zava-tematiska exempel på ljudfiler för praktisk träning

**Kodexempel:**

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper-rösttranskribering |
| C# | `csharp/WhisperTranscription.cs` | Whisper-rösttranskribering |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper-rösttranskribering |

> **Notera:** Denna labb använder **Foundry Local SDK** för att programmatisk ladda ner och ladda Whisper-modellen, och skickar sedan ljud till den lokala OpenAI-kompatibla endpointen för transkribering. Whisper-modellen (`whisper`) listas i Foundry Local-katalogen och körs helt på enheten – inga moln-API-nycklar eller nätverksåtkomst krävs.

---

### Del 10: Använda anpassade eller Hugging Face-modeller

**Labbinstruktion:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilera Hugging Face-modeller till optimerat ONNX-format med ONNX Runtime GenAI-modelbyggaren
- Hårdvaruspecifik kompilering (CPU, NVIDIA GPU, DirectML, WebGPU) och kvantisering (int4, fp16, bf16)
- Skapa chatt-templates konfigurationsfiler för Foundry Local
- Lägga till kompilerade modeller i Foundry Local-cachen
- Köra anpassade modeller via CLI, REST API och OpenAI SDK
- Exempelreferens: kompilera Qwen/Qwen3-0.6B end-to-end

---

### Del 11: Tool Calling med lokala modeller

**Labbinstruktion:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Möjliggör att lokala modeller kan anropa externa funktioner (tool/function calling)
- Definiera tool-scheman med OpenAI:s function-calling-format
- Hantera flervändskonversationer vid tool-calling
- Utföra tool-anrop lokalt och returnera resultat till modellen
- Välj rätt modell för tool-calling-scenarier (Qwen 2.5, Phi-4-mini)
- Använd SDK:s inbyggda `ChatClient` för tool calling (JavaScript)

**Kodexempel:**

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tool calling med väder-/befolkningsverktyg |
| C# | `csharp/ToolCalling.cs` | Tool calling med .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool calling med ChatClient |

---

### Del 12: Bygga ett webbaserat UI för Zava Creative Writer

**Labbinstruktion:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Lägg till ett webbläsarbaserat front-end till Zava Creative Writer
- Servera det delade UI:et från Python (FastAPI), JavaScript (Node.js HTTP) och C# (ASP.NET Core)
- Konsumera strömmande NDJSON i webbläsaren med Fetch API och ReadableStream
- Live status-badges för agent och realtidsströmmande artikeltext

**Kod (delat UI):**

| Fil | Beskrivning |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Sidlayout |
| `zava-creative-writer-local/ui/style.css` | Stilning |
| `zava-creative-writer-local/ui/app.js` | Strömmläsare och DOM-uppdateringslogik |

**Backend-tillägg:**

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Uppdaterad för att serva statiskt UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Ny HTTP-server som omsluter orkestratorn |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nytt ASP.NET Core minimal API-projekt |

---

### Del 13: Workshop slutförd
**Labguide:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Sammanfattning av allt du har byggt under alla 12 delar
- Fler idéer för att utöka dina applikationer
- Länkar till resurser och dokumentation

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

## Resurser

| Resurs | Länk |
|--------|------|
| Foundry Local webbplats | [foundrylocal.ai](https://foundrylocal.ai) |
| Modellkatalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Kom igång-guide | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK-referens | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licens

Detta workshop-material tillhandahålls för utbildningsändamål.

---

**Lycka till med byggandet! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfriskrivning**:
Detta dokument har översatts med hjälp av AI-översättningstjänsten [Co-op Translator](https://github.com/Azure/co-op-translator). Även om vi strävar efter noggrannhet, vänligen var medveten om att automatöversättningar kan innehålla fel eller unexactheter. Det ursprungliga dokumentet på dess ursprungliga språk bör betraktas som den auktoritativa källan. För kritisk information rekommenderas professionell mänsklig översättning. Vi ansvarar inte för några missförstånd eller feltolkningar som uppstår från användningen av denna översättning.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->