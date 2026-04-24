<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Byg AI-apps på enheden

En praktisk workshop til at køre sprogmodeller på din egen maskine og bygge intelligente applikationer med [Foundry Local](https://foundrylocal.ai) og [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Hvad er Foundry Local?** Foundry Local er en letvægts runtime, der lader dig downloade, administrere og betjene sprogmodeller fuldstændigt på dit eget hardware. Det eksponerer en **OpenAI-kompatibel API**, så ethvert værktøj eller SDK, der taler OpenAI, kan oprette forbindelse – ingen cloud-konto kræves.

### 🌐 Multisprogunderstøttelse

#### Understøttet via GitHub Action (Automatiseret og altid opdateret)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](./README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Foretrækker du at klone lokalt?**
>
> Dette repository indeholder 50+ sprogoversættelser, hvilket væsentligt øger downloadstørrelsen. For at klone uden oversættelser, brug sparse checkout:
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
> Dette giver dig alt, du behøver for at gennemføre kurset med en meget hurtigere download.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Læringsmål

Ved slutningen af denne workshop vil du kunne:

| # | Mål |
|---|-----------|
| 1 | Installere Foundry Local og administrere modeller med CLI |
| 2 | Mestre Foundry Local SDK API til programmæssig modeladministration |
| 3 | Forbinde til den lokale inferensserver ved hjælp af Python-, JavaScript- og C#-SDK'er |
| 4 | Bygge en Retrieval-Augmented Generation (RAG) pipeline, der baserer svar på dine egne data |
| 5 | Oprette AI-agenter med vedvarende instruktioner og personaer |
| 6 | Orkestrere multi-agent workflows med feedback loops |
| 7 | Udforske en produktions-capstone-app - Zava Creative Writer |
| 8 | Bygge evalueringsrammer med gyldne datasæt og LLM-as-judge scoring |
| 9 | Transskribere lyd med Whisper – tale-til-tekst på enheden ved hjælp af Foundry Local SDK |
| 10 | Kompilere og køre brugerdefinerede eller Hugging Face modeller med ONNX Runtime GenAI og Foundry Local |
| 11 | Muliggøre lokale modeller til at kalde eksterne funktioner med tool-calling mønsteret |
| 12 | Bygge et browser-baseret UI til Zava Creative Writer med real-time streaming |

---

## Forudsætninger

| Krav | Detaljer |
|-------------|---------|
| **Hardware** | Minimum 8 GB RAM (16 GB anbefalet); AVX2-kompatibel CPU eller en understøttet GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, eller macOS 13+ |
| **Foundry Local CLI** | Installer via `winget install Microsoft.FoundryLocal` (Windows) eller `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Se [kom godt i gang-guiden](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) for detaljer. |
| **Sprog runtime** | **Python 3.9+** og/eller **.NET 9.0+** og/eller **Node.js 18+** |
| **Git** | Til at klone dette repository |

---

## Kom godt i gang

```bash
# 1. Klon arkivet
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Bekræft at Foundry Local er installeret
foundry model list              # Vis tilgængelige modeller
foundry model run phi-3.5-mini  # Start en interaktiv chat

# 3. Vælg dit sprogspor (se Del 2 laboratoriet for fuld opsætning)
```

| Sprog | Hurtigstart |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshopdele

### Del 1: Kom godt i gang med Foundry Local

**Laboratorieguide:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Hvad er Foundry Local og hvordan det fungerer
- Installation af CLI på Windows og macOS
- Udforskning af modeller – liste, download, kørsel
- Forståelse af model-aliaser og dynamiske porte

---

### Del 2: Foundry Local SDK dybdegående

**Laboratorieguide:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Hvorfor bruge SDK fremfor CLI til applikationsudvikling
- Fuld SDK API reference for Python, JavaScript og C#
- Serviceadministration, kataloggennemgang, modellivscyklus (download, indlæs, aflast)
- Hurtigstartsmønstre: Python konstruktor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, aliaser og hardware-optimal modelvalg

---

### Del 3: SDK'er og API'er

**Laboratorieguide:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Forbindelse til Foundry Local fra Python, JavaScript og C#
- Brug af Foundry Local SDK til at administrere tjenesten programmæssigt
- Streaming af chatcompletioner via OpenAI-kompatibel API
- SDK metode-reference for hvert sprog

**Kodeeksempler:**

| Sprog | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Basis streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat med .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat med Node.js |

---

### Del 4: Retrieval-Augmented Generation (RAG)

**Laboratorieguide:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Hvad er RAG, og hvorfor det betyder noget
- Opbygning af en in-memory vidensbase
- Keyword-overlap retrieval med scoring
- Sammensætning af grundlagte systemprompter
- Køre en komplet RAG-pipeline på enheden

**Kodeeksempler:**

| Sprog | Fil |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Del 5: Opbygning af AI-agenter

**Laboratorieguide:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Hvad er en AI-agent (vs. et råt LLM-kald)
- `ChatAgent` mønsteret og Microsoft Agent Framework
- Systeminstruktioner, personaer og samtaler med flere omgange
- Struktureret output (JSON) fra agenter

**Kodeeksempler:**

| Sprog | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Enkel agent med Agent Framework |
| C# | `csharp/SingleAgent.cs` | Enkel agent (ChatAgent mønster) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Enkel agent (ChatAgent mønster) |

---

### Del 6: Multi-agent workflows

**Laboratorieguide:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Multi-agent pipelines: Forskeren → Forfatteren → Redaktøren
- Sekventiel orkestrering og feedback loops
- Delt konfiguration og strukturerede overdragelser
- Design af din egen multi-agent workflow

**Kodeeksempler:**

| Sprog | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Tre-agent pipeline |
| C# | `csharp/MultiAgent.cs` | Tre-agent pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Tre-agent pipeline |

---

### Del 7: Zava Creative Writer - Capstone-applikation

**Laboratorieguide:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- En produktionsstil multi-agent app med 4 specialiserede agenter
- Sekventiel pipeline med evaluator-drevne feedback loops
- Streaming af output, produktkatalogsøgning, strukturerede JSON-overdragelser
- Fuld implementering i Python (FastAPI), JavaScript (Node.js CLI) og C# (.NET console)

**Kodeeksempler:**

| Sprog | Mappe | Beskrivelse |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI webtjeneste med orkestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI-applikation |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 console-applikation |

---

### Del 8: Evaluationsdrevet udvikling

**Laboratorieguide:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Byg et systematisk evalueringsframework for AI-agenter med gyldne datasæt
- Regelbaserede checks (længde, nøgleordsdækning, forbudte termer) + LLM-som-dommer scoring
- Side-om-side sammenligning af promptvarianter med samlede scorecard
- Udvider Zava Editor agentmønsteret fra Del 7 til en offline testsuite
- Python, JavaScript og C# spor

**Kodeeksempler:**

| Sprog | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evalueringsframework |
| C# | `csharp/AgentEvaluation.cs` | Evalueringsframework |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evalueringsframework |

---

### Del 9: Tale-transskription med Whisper

**Laboratorieguide:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Tale-til-tekst-transskription ved hjælp af OpenAI Whisper, der kører lokalt  
- Privatlivsfokuseret lydbehandling - lyd forlader aldrig din enhed  
- Python-, JavaScript- og C#-spor med `client.audio.transcriptions.create()` (Python/JS) og `AudioClient.TranscribeAudioAsync()` (C#)  
- Indeholder Zava-tema eksempler på lydfiler til praktisk træning  

**Kodeeksempler:**  

| Sprog | Fil | Beskrivelse |  
|----------|------|-------------|  
| Python | `python/foundry-local-whisper.py` | Whisper tale-transskription |  
| C# | `csharp/WhisperTranscription.cs` | Whisper tale-transskription |  
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper tale-transskription |  

> **Bemærk:** Dette laboratorium bruger **Foundry Local SDK** til programmatisk at downloade og indlæse Whisper-modellen, og sender derefter lyd til det lokale OpenAI-kompatible endepunkt til transskription. Whisper-modellen (`whisper`) er opført i Foundry Local-kataloget og kører udelukkende på enheden - ingen cloud API-nøgler eller netværksadgang kræves.  

---  

### Del 10: Brug af tilpassede eller Hugging Face-modeller  

**Laboratorieguide:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)  

- Kompilering af Hugging Face-modeller til optimeret ONNX-format ved brug af ONNX Runtime GenAI modelbygger  
- Hardware-specifik kompilering (CPU, NVIDIA GPU, DirectML, WebGPU) og kvantisering (int4, fp16, bf16)  
- Oprettelse af chat-skabelonkonfigurationsfiler til Foundry Local  
- Tilføjelse af kompilerede modeller til Foundry Local-cachen  
- Kørsel af tilpassede modeller via CLI, REST API og OpenAI SDK  
- Referenceeksempel: komplet kompilering af Qwen/Qwen3-0.6B  

---  

### Del 11: Værktøjskald med lokale modeller  

**Laboratorieguide:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)  

- Muliggør, at lokale modeller kan kalde eksterne funktioner (værktøj/funktionskald)  
- Definer værktøjsskemaer ved hjælp af OpenAI-funktionskaldsformatet  
- Håndter den flertrins værktøjskalds-samtaleflow  
- Udfør værktøjskald lokalt og returner resultater til modellen  
- Vælg den rette model til værktøjskaldsscenarier (Qwen 2.5, Phi-4-mini)  
- Brug SDK'ens native `ChatClient` til værktøjskald (JavaScript)  

**Kodeeksempler:**  

| Sprog | Fil | Beskrivelse |  
|----------|------|-------------|  
| Python | `python/foundry-local-tool-calling.py` | Værktøjskald med vejrfunktion og befolkningsværktøj |  
| C# | `csharp/ToolCalling.cs` | Værktøjskald med .NET |  
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Værktøjskald med ChatClient |  

---  

### Del 12: Bygning af et web UI til Zava Creative Writer  

**Laboratorieguide:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)  

- Tilføj en browserbaseret front-end til Zava Creative Writer  
- Server det delte UI fra Python (FastAPI), JavaScript (Node.js HTTP) og C# (ASP.NET Core)  
- Forbrug streaming NDJSON i browseren med Fetch API og ReadableStream  
- Live agent-status badges og realtidstekst streaming af artikler  

**Kode (delt UI):**  

| Fil | Beskrivelse |  
|------|-------------|  
| `zava-creative-writer-local/ui/index.html` | Sidelayout |  
| `zava-creative-writer-local/ui/style.css` | Styling |  
| `zava-creative-writer-local/ui/app.js` | Streamlæsning og DOM-opdateringslogik |  

**Backend-tilføjelser:**  

| Sprog | Fil | Beskrivelse |  
|----------|------|-------------|  
| Python | `zava-creative-writer-local/src/api/main.py` | Opdateret til at serve statisk UI |  
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Ny HTTP-server der wrapper orkestratoren |  
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nyt ASP.NET Core minimal API-projekt |  

---  

### Del 13: Workshop fuldført  

**Laboratorieguide:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)  

- Opsummering af alt, hvad du har bygget gennem alle 12 dele  
- Yderligere idéer til udvidelse af dine applikationer  
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
| Modelkatalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |  
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |  
| Kom godt i gang-guide | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |  
| Foundry Local SDK Reference | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |  
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |  
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |  
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |  

---  

## Licens  

Dette workshopmateriale er leveret til uddannelsesformål.  

---  

**God fornøjelse med byggeriet! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokument er blevet oversat ved hjælp af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi bestræber os på nøjagtighed, skal du være opmærksom på, at automatiserede oversættelser kan indeholde fejl eller unøjagtigheder. Det oprindelige dokument på dets oprindelige sprog bør betragtes som den autoritative kilde. For kritisk information anbefales professionel menneskelig oversættelse. Vi er ikke ansvarlige for eventuelle misforståelser eller fejltolkninger, der opstår som følge af brugen af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->