<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Bygg AI-applikasjoner på enheten

En praktisk workshop for å kjøre språkmodeller på din egen maskin og bygge intelligente applikasjoner med [Foundry Local](https://foundrylocal.ai) og [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Hva er Foundry Local?** Foundry Local er en lettvekts runtime som lar deg laste ned, administrere og tilby språkmodeller helt på maskinvaren din. Den eksponerer et **OpenAI-kompatibelt API** slik at alle verktøy eller SDK-er som snakker OpenAI kan koble til - ingen skyløsning nødvendig.

### 🌐 Flerspråklig støtte

#### Støttet via GitHub Action (Automatisert & alltid oppdatert)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](./README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Foretrekker du å klone lokalt?**
>
> Dette depotet inkluderer over 50 språköversettelser som betydelig øker nedlastingsstørrelsen. For å klone uten oversettelser, bruk sparse checkout:
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
> Dette gir deg alt du trenger for å fullføre kurset med en mye raskere nedlasting.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Læringsmål

Innen slutten av denne workshopen vil du kunne:

| # | Mål |
|---|-----------|
| 1 | Installere Foundry Local og administrere modeller med CLI |
| 2 | Mestre Foundry Local SDK API for programmatisk modelladministrasjon |
| 3 | Koble til den lokale inferensserveren ved bruk av Python-, JavaScript- og C# SDK-er |
| 4 | Bygge en Retrieval-Augmented Generation (RAG) pipeline som baserer svar på dine egne data |
| 5 | Lage AI-agenter med persistente instruksjoner og personligheter |
| 6 | Orkestrere arbeidsflyter med flere agenter med tilbakemeldingssløyfer |
| 7 | Utforske en produksjonsstil capstone-app – Zava Creative Writer |
| 8 | Lage evalueringsrammeverk med gullsett og LLM-som-dommer scoring |
| 9 | Transkribere lyd med Whisper - tale-til-tekst på enheten med Foundry Local SDK |
| 10 | Kompilere og kjøre tilpassede eller Hugging Face-modeller med ONNX Runtime GenAI og Foundry Local |
| 11 | Aktivere lokale modeller til å kalle eksterne funksjoner med verktøy-kallingsmønsteret |
| 12 | Bygge et nettleserbasert UI for Zava Creative Writer med sanntidsstrømming |

---

## Forutsetninger

| Krav | Detaljer |
|-------------|---------|
| **Maskinvare** | Minimum 8 GB RAM (16 GB anbefalt); AVX2-kompatibel CPU eller støttet GPU |
| **Operativsystem** | Windows 10/11 (x64/ARM), Windows Server 2025, eller macOS 13+ |
| **Foundry Local CLI** | Installer via `winget install Microsoft.FoundryLocal` (Windows) eller `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Se [kom i gang-guiden](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) for detaljer. |
| **Språk runtime** | **Python 3.9+** og/eller **.NET 9.0+** og/eller **Node.js 18+** |
| **Git** | For å klone dette depotet |

---

## Komme i gang

```bash
# 1. Klon depotet
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Bekreft at Foundry Local er installert
foundry model list              # List tilgjengelige modeller
foundry model run phi-3.5-mini  # Start en interaktiv chat

# 3. Velg ditt språkspor (se del 2 lab for full oppsett)
```

| Språk | Kom i gang |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshop-deler

### Del 1: Komme i gang med Foundry Local

**Lab-guide:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Hva er Foundry Local og hvordan det fungerer
- Installere CLI på Windows og macOS
- Utforske modeller - listing, nedlasting, kjøring
- Forstå modellaliaser og dynamiske porter

---

### Del 2: Dypdykk i Foundry Local SDK

**Lab-guide:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Hvorfor bruke SDK over CLI for applikasjonsutvikling
- Full SDK API-referanse for Python, JavaScript, og C#
- Tjenesteadministrasjon, kataloggjennomgang, modellens livssyklus (nedlasting, lasting, avslasting)
- Raskstartmønstre: Python-konstruktør bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, aliaser og maskinvareriktig modellvalg

---

### Del 3: SDK-er og API-er

**Lab-guide:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Koble til Foundry Local fra Python, JavaScript og C#
- Bruke Foundry Local SDK for å programmere tjenestestyring
- Strømming av chat fullføringer via OpenAI-kompatibelt API
- SDK-metodereferanse for hvert språk

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Grunnleggende strømmende chat |
| C# | `csharp/BasicChat.cs` | Strømmende chat med .NET |
| JavaScript | `javascript/foundry-local.mjs` | Strømmende chat med Node.js |

---

### Del 4: Retrieval-Augmented Generation (RAG)

**Lab-guide:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Hva er RAG og hvorfor det er viktig
- Bygge en kunnskapsbase i minnet
- Søking med nøkkelord-overlapp og scoring
- Sette sammen systemprompt som er forankret i data
- Kjøre en komplett RAG-pipeline på enheten

**Kodeeksempler:**

| Språk | Fil |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Del 5: Bygge AI-agenter

**Lab-guide:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Hva er en AI-agent (vs. et rått LLM-anrop)
- `ChatAgent`-mønsteret og Microsoft Agent Framework
- Systeminstruksjoner, personligheter, og multitrinns samtaler
- Strukturert utdata (JSON) fra agenter

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Enkel agent med Agent Framework |
| C# | `csharp/SingleAgent.cs` | Enkel agent (ChatAgent-mønster) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Enkel agent (ChatAgent-mønster) |

---

### Del 6: Arbeidsflyter med flere agenter

**Lab-guide:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Arbeidsflyter med flere agenter: Forsker → Skribent → Redaktør
- Sekvensiell orkestrering og tilbakemeldingssløyfer
- Delt konfigurasjon og strukturerte overleveringer
- Designe din egen arbeidsflyt med flere agenter

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Tre-agenters pipeline |
| C# | `csharp/MultiAgent.cs` | Tre-agenters pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Tre-agenters pipeline |

---

### Del 7: Zava Creative Writer - Capstone-applikasjon

**Lab-guide:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- En produksjonsstil multi-agent-app med 4 spesialiserte agenter
- Sekvensiell pipeline med evaluatorstyrte tilbakemeldingssløyfer
- Strømming av utdata, produktkatalogsøk, strukturerte JSON-overleveringer
- Full implementasjon i Python (FastAPI), JavaScript (Node.js CLI), og C# (.NET-konsoll)

**Kodeeksempler:**

| Språk | Mappe | Beskrivelse |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI webtjeneste med orkestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI-applikasjon |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsollapplikasjon |

---

### Del 8: Evaluering-ledet utvikling

**Lab-guide:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Bygg et systematisk evalueringsrammeverk for AI-agenter ved å bruke gullsett
- Regelbaserte sjekker (lengde, nøkkelordsdekning, forbudte termer) + LLM-som-dommer scoring
- Side-ved-side sammenligning av promptvarianter med aggregerte scorekort
- Utvider Zava Editor-agentmønsteret fra Del 7 til en offline test-suite
- Baner for Python, JavaScript og C#

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evalueringsrammeverk |
| C# | `csharp/AgentEvaluation.cs` | Evalueringsrammeverk |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evalueringsrammeverk |

---

### Del 9: Tale-transkripsjon med Whisper

**Lab-guide:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Tale-til-tekst transkripsjon ved bruk av OpenAI Whisper som kjører lokalt
- Personvern-fokusert lydbehandling - lyd forlater aldri enheten din
- Python-, JavaScript- og C#-spor med `client.audio.transcriptions.create()` (Python/JS) og `AudioClient.TranscribeAudioAsync()` (C#)
- Inkluderer Zava-tema eksempel lydfiler for praktisk øvelse

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper tale-transkripsjon |
| C# | `csharp/WhisperTranscription.cs` | Whisper tale-transkripsjon |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper tale-transkripsjon |

> **Merk:** Dette laboratoriet bruker **Foundry Local SDK** for å programmessig laste ned og laste inn Whisper-modellen, og sender deretter lyd til det lokale OpenAI-kompatible endepunktet for transkripsjon. Whisper-modellen (`whisper`) er oppført i Foundry Local katalogen og kjøres helt på enheten - ingen sky-API-nøkler eller nettverkstilgang kreves.

---

### Del 10: Bruke egendefinerte eller Hugging Face-modeller

**Lab-guide:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilering av Hugging Face-modeller til optimalisert ONNX-format ved bruk av ONNX Runtime GenAI modellbygger
- Maskinvare-spesifikk kompilering (CPU, NVIDIA GPU, DirectML, WebGPU) og kvantisering (int4, fp16, bf16)
- Lage chat-mal konfigurasjonsfiler for Foundry Local
- Legge til kompilerte modeller i Foundry Local cache
- Kjøre egendefinerte modeller via CLI, REST API og OpenAI SDK
- Referanseeksempel: kompilere Qwen/Qwen3-0.6B ende-til-ende

---

### Del 11: Verktøy-kalling med lokale modeller

**Lab-guide:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Aktiver lokale modeller til å kalle eksterne funksjoner (verktøy-/funksjonskalling)
- Definere verktøyskjemaer ved bruk av OpenAI funksjonskalling-formatet
- Håndtere samtaleflyt med flere runder av verktøys-kalling
- Utføre verktøyskall lokalt og returnere resultater til modellen
- Velge riktig modell for verktøy-kalling scenarier (Qwen 2.5, Phi-4-mini)
- Bruke SDKs native `ChatClient` for verktøy-kalling (JavaScript)

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Verktøy-kalling med vær-/befolkningsverktøy |
| C# | `csharp/ToolCalling.cs` | Verktøy-kalling med .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Verktøy-kalling med ChatClient |

---

### Del 12: Bygge et webgrensesnitt for Zava Creative Writer

**Lab-guide:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Legg til en nettleserbasert frontend til Zava Creative Writer
- Serve den delte UI-en fra Python (FastAPI), JavaScript (Node.js HTTP) og C# (ASP.NET Core)
- Bruke strømmet NDJSON i nettleseren med Fetch API og ReadableStream
- Live agent-statusmerker og sanntid artikkeltekst-strømming

**Kode (delt UI):**

| Fil | Beskrivelse |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Sideoppsett |
| `zava-creative-writer-local/ui/style.css` | Stiling |
| `zava-creative-writer-local/ui/app.js` | Strømleser og DOM oppdateringslogikk |

**Backend-tillegg:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Oppdatert for å serve statisk UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Ny HTTP-server som pakker inn orkestratoren |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nytt ASP.NET Core minimal API-prosjekt |

---

### Del 13: Workshop fullført

**Lab-guide:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Oppsummering av alt du har bygget gjennom alle 12 delene
- Ytterligere ideer for å utvide dine applikasjoner
- Lenker til ressurser og dokumentasjon

---

## Prosjektstruktur

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

## Ressurser

| Ressurs | Lenke |
|----------|------|
| Foundry Local nettside | [foundrylocal.ai](https://foundrylocal.ai) |
| Modell-katalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Kom i gang-guide | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referanse | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Lisens

Dette workshop-materialet leveres for utdanningsformål.

---

**Lykke til med byggingen! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokumentet er oversatt ved bruk av AI-oversettelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selv om vi streber etter nøyaktighet, vennligst vær oppmerksom på at automatiserte oversettelser kan inneholde feil eller unøyaktigheter. Det opprinnelige dokumentet på originalspråket skal betraktes som den autoritative kilden. For kritisk informasjon anbefales profesjonell menneskelig oversettelse. Vi er ikke ansvarlige for noen misforståelser eller feiltolkninger som oppstår ved bruk av denne oversettelsen.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->