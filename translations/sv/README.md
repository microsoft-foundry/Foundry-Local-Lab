<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Bygg AI-appar på enheten

En praktisk workshop för att köra språkmodeller på din egen dator och bygga intelligenta applikationer med [Foundry Local](https://foundrylocal.ai) och [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Vad är Foundry Local?** Foundry Local är en lättviktig runtime som låter dig ladda ner, hantera och servera språkmodeller helt på din hårdvara. Den exponerar ett **OpenAI-kompatibelt API** så att alla verktyg eller SDK:er som använder OpenAI kan ansluta - inget molnkonto krävs.

### 🌐 Flerspråkigt stöd

#### Stöds via GitHub Action (Automatiserad & Alltid Uppdaterad)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabiska](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgariska](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Kinesiska (Förenklad)](../zh-CN/README.md) | [Kinesiska (Traditionell, Hongkong)](../zh-HK/README.md) | [Kinesiska (Traditionell, Macau)](../zh-MO/README.md) | [Kinesiska (Traditionell, Taiwan)](../zh-TW/README.md) | [Kroatiska](../hr/README.md) | [Tjeckiska](../cs/README.md) | [Danska](../da/README.md) | [Nederländska](../nl/README.md) | [Estniska](../et/README.md) | [Finska](../fi/README.md) | [Franska](../fr/README.md) | [Tyska](../de/README.md) | [Grekiska](../el/README.md) | [Hebreiska](../he/README.md) | [Hindi](../hi/README.md) | [Ungerska](../hu/README.md) | [Indonesiska](../id/README.md) | [Italienska](../it/README.md) | [Japanska](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Koreanska](../ko/README.md) | [Litauiska](../lt/README.md) | [Malajiska](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigeriansk Pidgin](../pcm/README.md) | [Norska](../no/README.md) | [Persiska (Farsi)](../fa/README.md) | [Polska](../pl/README.md) | [Portugisiska (Brasilien)](../pt-BR/README.md) | [Portugisiska (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Rumänska](../ro/README.md) | [Ryska](../ru/README.md) | [Serbiska (Kyrilliska)](../sr/README.md) | [Slovakiska](../sk/README.md) | [Slovenska](../sl/README.md) | [Spanska](../es/README.md) | [Swahili](../sw/README.md) | [Svenska](./README.md) | [Tagalog (Filippinska)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thailändska](../th/README.md) | [Turkiska](../tr/README.md) | [Ukrainska](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamesiska](../vi/README.md)

> **Föredrar du att klona lokalt?**
>
> Detta arkiv inkluderar över 50 språköversättningar vilket markant ökar nedladdningsstorleken. För att klona utan översättningar, använd sparse checkout:
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
> Detta ger dig allt du behöver för att slutföra kursen med mycket snabbare nedladdning.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Lärandemål

I slutet av denna workshop kommer du att kunna:

| # | Mål |
|---|-----------|
| 1 | Installera Foundry Local och hantera modeller med CLI |
| 2 | Bemästra Foundry Local SDK API för programmatisk modellhantering |
| 3 | Ansluta till den lokala inferensservern med Python, JavaScript och C# SDK:er |
| 4 | Bygga en Retrieval-Augmented Generation (RAG) pipeline som förankrar svar i dina egna data |
| 5 | Skapa AI-agenter med persistenta instruktioner och personas |
| 6 | Orkestrera multi-agent arbetsflöden med feedbackloopar |
| 7 | Utforska en produktionsnära examensapplikation - Zava Creative Writer |
| 8 | Bygga evalueringsramverk med gulddatasets och LLM-som-domare bedömning |
| 9 | Transkribera ljud med Whisper - tal-till-text på enheten med Foundry Local SDK |
| 10 | Kompilera och kör egna eller Hugging Face-modeller med ONNX Runtime GenAI och Foundry Local |
| 11 | Möjliggöra för lokala modeller att anropa externa funktioner med verktygsanropsmönstret |
| 12 | Bygga en webbläsarbaserad UI för Zava Creative Writer med realtidsströmning |

---

## Förutsättningar

| Krav | Detaljer |
|-------------|---------|
| **Hårdvara** | Minst 8 GB RAM (16 GB rekommenderas); CPU med AVX2 eller ett stödjat GPU |
| **Operativsystem** | Windows 10/11 (x64/ARM), Windows Server 2025 eller macOS 13+ |
| **Foundry Local CLI** | Installera via `winget install Microsoft.FoundryLocal` (Windows) eller `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Se [kom igång-guiden](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) för detaljer. |
| **Språkruntime** | **Python 3.9+** och/eller **.NET 9.0+** och/eller **Node.js 18+** |
| **Git** | För att klona detta arkiv |

---

## Kom igång

```bash
# 1. Klona förrådet
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verifiera att Foundry Local är installerat
foundry model list              # Lista tillgängliga modeller
foundry model run phi-3.5-mini  # Starta en interaktiv chatt

# 3. Välj din språkbana (se Del 2 laboratorium för fullständig installation)
```

| Språk | Snabbstart |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshopdelar

### Del 1: Kom igång med Foundry Local

**Labguide:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Vad är Foundry Local och hur det fungerar
- Installera CLI på Windows och macOS
- Utforska modeller - lista, ladda ner, köra
- Förstå modellalias och dynamiska portar

---

### Del 2: Fördjupning i Foundry Local SDK

**Labguide:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Varför använda SDK istället för CLI för applikationsutveckling
- Komplett SDK API-referens för Python, JavaScript och C#
- Tjänstehantering, kataloggenomgång, modellens livscykel (nedladdning, laddning, avlastning)
- Snabbstartsmönster: Python-konstruktör bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, alias och hårdvaruoptimalt modellval

---

### Del 3: SDK:er och API:er

**Labguide:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Ansluta till Foundry Local från Python, JavaScript och C#
- Använda Foundry Local SDK för programmatisk tjänstehantering
- Strömmande chat-kompletteringar via OpenAI-kompatibelt API
- SDK metoder referens för varje språk

**Kodexempel:**

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Grundläggande strömmande chatt |
| C# | `csharp/BasicChat.cs` | Strömmande chatt med .NET |
| JavaScript | `javascript/foundry-local.mjs` | Strömmande chatt med Node.js |

---

### Del 4: Retrieval-Augmented Generation (RAG)

**Labguide:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Vad är RAG och varför det är viktigt
- Bygga en kunskapsbas i minnet
- Nyckelordsövergripande hämtning med poängsättning
- Komponera förankrade systemuppmaningar
- Köra en komplett RAG-pipeline på enheten

**Kodexempel:**

| Språk | Fil |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Del 5: Bygga AI-agenter

**Labguide:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Vad är en AI-agent (jämfört med ett direkt LLM-anrop)
- `ChatAgent`-mönstret och Microsoft Agent Framework
- Systeminstruktioner, personas och fleromgångs-konversationer
- Strukturerad output (JSON) från agenter

**Kodexempel:**

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Enskild agent med Agent Framework |
| C# | `csharp/SingleAgent.cs` | Enskild agent (ChatAgent-mönster) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Enskild agent (ChatAgent-mönster) |

---

### Del 6: Multi-agent arbetsflöden

**Labguide:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Multi-agent pipelines: Forskare → Författare → Redaktör
- Sekventiell orkestrering och feedbackloopar
- Delad konfiguration och strukturerade överlämningar
- Designa ditt eget multi-agent arbetsflöde

**Kodexempel:**

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline med tre agenter |
| C# | `csharp/MultiAgent.cs` | Pipeline med tre agenter |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline med tre agenter |

---

### Del 7: Zava Creative Writer - Examensapplikation

**Labguide:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- En produktionslik multi-agent app med 4 specialiserade agenter
- Sekventiell pipeline med utvärderingsdrivna feedbackloopar
- Strömmande output, produktkatalogsökning, strukturerade JSON-överföringar
- Fullständig implementation i Python (FastAPI), JavaScript (Node.js CLI) och C# (.NET-konsol)

**Kodexempel:**

| Språk | Katalog | Beskrivning |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI-webbtjänst med orkestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI-applikation |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsolapplikation |

---

### Del 8: Utveckling med utvärderingsledning

**Labguide:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Bygga ett systematiskt utvärderingsramverk för AI-agenter med gulddatasets
- Regelbaserade kontroller (längd, nyckelordstäckning, förbjudna termer) + LLM-som-domare-poängsättning
- Sido-vid-sido jämförelse av promptvarianter med sammanfattande resultatkort
- Utvidgar Zava Editor agent-mönstret från Del 7 till en offline testsvit
- Spår för Python, JavaScript och C#

**Kodexempel:**

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Utvärderingsramverk |
| C# | `csharp/AgentEvaluation.cs` | Utvärderingsramverk |
| JavaScript | `javascript/foundry-local-eval.mjs` | Utvärderingsramverk |

---

### Del 9: Rösttranskribering med Whisper

**Labguide:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Tal-till-text-transkription med OpenAI Whisper som körs lokalt  
- Sekretessfokuserad ljudbearbetning - ljud lämnar aldrig din enhet  
- Python-, JavaScript- och C#-spår med `client.audio.transcriptions.create()` (Python/JS) och `AudioClient.TranscribeAudioAsync()` (C#)  
- Inkluderar Zava-tematiska ljudfiler för praktisk övning  

**Kodexempel:**  

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper rösttranskription |
| C# | `csharp/WhisperTranscription.cs` | Whisper rösttranskription |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper rösttranskription |

> **Obs:** Detta laboration använder **Foundry Local SDK** för att programmatisk ladda ner och ladda Whisper-modellen, sedan skicka ljud till den lokala OpenAI-kompatibla slutpunkten för transkription. Whisper-modellen (`whisper`) listas i Foundry Local-katalogen och körs helt på enheten - inga moln-API-nycklar eller nätverksåtkomst krävs.  

---

### Del 10: Använda Egna eller Hugging Face-modeller  

**Laborationsguide:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)  

- Kompilera Hugging Face-modeller till optimerat ONNX-format med ONNX Runtime GenAI-modellbyggare  
- Hårdvaruspecifik kompilering (CPU, NVIDIA GPU, DirectML, WebGPU) och kvantisering (int4, fp16, bf16)  
- Skapa chatt-mallkonfigurationsfiler för Foundry Local  
- Lägga till kompilerade modeller i Foundry Local-cache  
- Köra egna modeller via CLI, REST API och OpenAI SDK  
- Referensexempel: kompilering av Qwen/Qwen3-0.6B från början till slut  

---

### Del 11: Verktygsanrop med Lokala Modeller  

**Laborationsguide:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)  

- Aktivera lokala modeller att anropa externa funktioner (verktygs-/funktionsanrop)  
- Definiera verktygsscheman med OpenAI:s format för funktionsanrop  
- Hantera konversationens flerstegsflöde för verktygsanrop  
- Utför verktygsanrop lokalt och returnera resultat till modellen  
- Välj rätt modell för verktygsanropsscenarier (Qwen 2.5, Phi-4-mini)  
- Använd SDK:s inbyggda `ChatClient` för verktygsanrop (JavaScript)  

**Kodexempel:**  

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Verktygsanrop med väder-/befolkningsverktyg |
| C# | `csharp/ToolCalling.cs` | Verktygsanrop med .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Verktygsanrop med ChatClient |

---

### Del 12: Bygga ett webbgränssnitt för Zava Creative Writer  

**Laborationsguide:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)  

- Lägg till ett webbläsarbaserat frontend till Zava Creative Writer  
- Servera det delade gränssnittet från Python (FastAPI), JavaScript (Node.js HTTP) och C# (ASP.NET Core)  
- Konsumera strömmande NDJSON i webbläsaren med Fetch API och ReadableStream  
- Live-badges för agentstatus och realtidsströmmande artiklar  

**Kod (delat UI):**  

| Fil | Beskrivning |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Sidlayout |
| `zava-creative-writer-local/ui/style.css` | Stil |
| `zava-creative-writer-local/ui/app.js` | Strömläsare och DOM-uppdateringslogik |

**Backend-tillägg:**  

| Språk | Fil | Beskrivning |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Uppdaterad för att serva statiskt UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Ny HTTP-server som kapslar orkestratorn |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Ny ASP.NET Core minimal API-projekt |

---

### Del 13: Workshop Slutförd  

**Laborationsguide:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)  

- Sammanfattning av allt du byggt i alla 12 delar  
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
|----------|------|
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
Detta dokument har översatts med AI-översättningstjänsten [Co-op Translator](https://github.com/Azure/co-op-translator). Även om vi strävar efter noggrannhet, vänligen var medveten om att automatiska översättningar kan innehålla fel eller brister. Det ursprungliga dokumentet på dess modersmål ska anses vara den auktoritativa källan. För kritisk information rekommenderas professionell mänsklig översättning. Vi ansvarar inte för några missförstånd eller feltolkningar som uppstår från användningen av denna översättning.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->