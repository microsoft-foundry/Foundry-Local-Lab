<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Bygg AI-applikasjoner på enheten

En praktisk workshop for å kjøre språkmodeller på din egen maskin og bygge intelligente applikasjoner med [Foundry Local](https://foundrylocal.ai) og [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Hva er Foundry Local?** Foundry Local er en lettvekts runtime som lar deg laste ned, administrere og kjøre språkmodeller helt på din egen maskinvare. Den eksponerer et **OpenAI-kompatibelt API** slik at ethvert verktøy eller SDK som støtter OpenAI kan koble til – ingen sky-konto kreves.

---

## Læringsmål

Ved slutten av denne workshopen vil du kunne:

| # | Mål |
|---|-----------|
| 1 | Installere Foundry Local og administrere modeller med CLI |
| 2 | Mestre Foundry Local SDK API for programmatisk modelladministrasjon |
| 3 | Koble til lokal inferensserver med Python-, JavaScript- og C#-SDKer |
| 4 | Bygge en Retrieval-Augmented Generation (RAG) pipeline som baserer svar på din egen data |
| 5 | Lage AI-agenter med vedvarende instruksjoner og personas |
| 6 | Orkestrere arbeidsflyter med flere agenter og tilbakemeldingssløyfer |
| 7 | Utforske en produksjonsnær capstone-app – Zava Creative Writer |
| 8 | Bygge evalueringsrammeverk med gullstandard datasett og LLM-som-dommer scoring |
| 9 | Transkribere lyd med Whisper – tale-til-tekst på enheten med Foundry Local SDK |
| 10 | Kompilere og kjøre egne eller Hugging Face-modeller med ONNX Runtime GenAI og Foundry Local |
| 11 | Aktivere lokale modeller til å kalle eksterne funksjoner med verktøysanropsmønsteret |
| 12 | Bygge et nettleserbasert brukergrensesnitt for Zava Creative Writer med sanntidsstrømming |

---

## Forutsetninger

| Krav | Detaljer |
|-------------|---------|
| **Maskinvare** | Minst 8 GB RAM (16 GB anbefales); AVX2-kompatibel CPU eller støttet GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, eller macOS 13+ |
| **Foundry Local CLI** | Installer via `winget install Microsoft.FoundryLocal` (Windows) eller `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Se [kom i gang-guiden](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) for detaljer. |
| **Språkruntime** | **Python 3.9+** og/eller **.NET 9.0+** og/eller **Node.js 18+** |
| **Git** | For å klone dette repositoriet |

---

## Kom i gang

```bash
# 1. Klon lageret
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Bekreft at Foundry Local er installert
foundry model list              # List tilgjengelige modeller
foundry model run phi-3.5-mini  # Start en interaktiv chat

# 3. Velg ditt språkløp (se Del 2 lab for full oppsett)
```

| Språk | Rask start |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshop-deler

### Del 1: Kom i gang med Foundry Local

**Laboratoriumsveiledning:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Hva er Foundry Local og hvordan det fungerer
- Installasjon av CLI på Windows og macOS
- Utforske modeller – liste opp, laste ned, kjøre
- Forstå modellaliaser og dynamiske porter

---

### Del 2: Dypdykk i Foundry Local SDK

**Laboratoriumsveiledning:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Hvorfor bruke SDK fremfor CLI for applikasjonsutvikling
- Full SDK API-referanse for Python, JavaScript og C#
- Tjenesteadministrasjon, katalogutforsking, modell-livssyklus (nedlast, last inn, last ut)
- Raskstartsmønstre: Python-konstruktør bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, aliaser og maskinvareoptimalisert modellvalg

---

### Del 3: SDKer og APIer

**Laboratoriumsveiledning:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Koble til Foundry Local fra Python, JavaScript og C#
- Bruke Foundry Local SDK for programmatisk tjenesteadministrasjon
- Strømming av chat fullføringer via OpenAI-kompatibelt API
- SDK metode-referanse for hvert språk

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Grunnleggende strømmende chat |
| C# | `csharp/BasicChat.cs` | Strømmende chat med .NET |
| JavaScript | `javascript/foundry-local.mjs` | Strømmende chat med Node.js |

---

### Del 4: Retrieval-Augmented Generation (RAG)

**Laboratoriumsveiledning:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Hva er RAG og hvorfor det er viktig
- Bygge en kunnskapsbase i minnet
- Nøkkelord-overlappingssøk med scoring
- Sette sammen grunnlag for systemprompter
- Kjøre en komplett RAG pipeline på enheten

**Kodeeksempler:**

| Språk | Fil |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Del 5: Bygge AI-agenter

**Laboratoriumsveiledning:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Hva er en AI-agent (vs. et rått LLM-anrop)
- `ChatAgent`-mønsteret og Microsoft Agent Framework
- Systeminstruksjoner, personas og samtaler med flere runder
- Strukturert utdata (JSON) fra agenter

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Enkeltagent med Agent Framework |
| C# | `csharp/SingleAgent.cs` | Enkeltagent (ChatAgent-mønster) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Enkeltagent (ChatAgent-mønster) |

---

### Del 6: Arbeidsflyter med flere agenter

**Laboratoriumsveiledning:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline med flere agenter: Forsker → Forfatter → Redaktør
- Sekvensiell orkestrering og tilbakemeldingssløyfer
- Felles konfigurasjon og strukturert overlevering
- Designe din egen arbeidsflyt med flere agenter

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline med tre agenter |
| C# | `csharp/MultiAgent.cs` | Pipeline med tre agenter |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline med tre agenter |

---

### Del 7: Zava Creative Writer - Capstone-applikasjon

**Laboratoriumsveiledning:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- En produksjonsstil app med flere agenter og 4 spesialiserte agenter
- Sekvensiell pipeline med evaluatorstyrte tilbakemeldingssløyfer
- Strømmet utdata, produktkatalogsøk, strukturert JSON-overlevering
- Full implementasjon i Python (FastAPI), JavaScript (Node.js CLI) og C# (.NET konsoll)

**Kodeeksempler:**

| Språk | Katalog | Beskrivelse |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI webtjeneste med orkestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI-applikasjon |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsollapplikasjon |

---

### Del 8: Evaluering-ledet utvikling

**Laboratoriumsveiledning:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Bygge et systematisk evalueringsrammeverk for AI-agenter med gullstandard datasett
- Regelbaserte kontroller (lengde, søkeorddekning, forbudte ord) + LLM-som-dommer scoring
- Side-ved-side sammenligning av promptvarianter med aggregerte scorekort
- Utvider Zava Editor-agentmønsteret fra Del 7 til en offline testpakke
- Python, JavaScript og C# spor

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evalueringsrammeverk |
| C# | `csharp/AgentEvaluation.cs` | Evalueringsrammeverk |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evalueringsrammeverk |

---

### Del 9: Tale-transkripsjon med Whisper

**Laboratoriumsveiledning:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Tale-til-tekst transkripsjon ved hjelp av OpenAI Whisper som kjører lokalt
- Personvern-fokusert lydbehandling - lyd forlater aldri enheten din
- Python, JavaScript og C# spor med `client.audio.transcriptions.create()` (Python/JS) og `AudioClient.TranscribeAudioAsync()` (C#)
- Inkluderer Zava-tema lydprøver for praktisk øvelse

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper tale-transkripsjon |
| C# | `csharp/WhisperTranscription.cs` | Whisper tale-transkripsjon |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper tale-transkripsjon |

> **Merk:** Dette laboratoriet bruker **Foundry Local SDK** for å programmatiskt laste ned og laste Whisper-modellen, og sender deretter lyd til den lokale OpenAI-kompatible endepunktet for transkripsjon. Whisper-modellen (`whisper`) er oppført i Foundry Local-katalogen og kjører helt på enheten – ingen sky-API-nøkler eller nettverkstilgang kreves.

---

### Del 10: Bruke egne eller Hugging Face-modeller

**Laboratoriumsveiledning:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilere Hugging Face-modeller til optimalisert ONNX-format ved hjelp av ONNX Runtime GenAI modellbygger
- Maskinvarespesifikk kompilering (CPU, NVIDIA GPU, DirectML, WebGPU) og kvantisering (int4, fp16, bf16)
- Lage chat-mal konfigurasjonsfiler for Foundry Local
- Legge til kompilert modeller i Foundry Local cachen
- Kjøre egne modeller via CLI, REST API og OpenAI SDK
- Referanseeksempel: ende-til-ende-kompilering av Qwen/Qwen3-0.6B

---

### Del 11: Verktøysanrop med lokale modeller

**Laboratoriumsveiledning:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Aktivere lokale modeller til å kalle eksterne funksjoner (verktøy-/funksjonsanrop)
- Definere verktøyskjemastruktur med OpenAI funksjonsanropsformat
- Håndtere fler-runde samtaleflyt for verktøysanrop
- Utføre verktøysanrop lokalt og returnere resultater til modellen
- Velge riktig modell for verktøysanrop (Qwen 2.5, Phi-4-mini)
- Bruke SDKs native `ChatClient` for verktøysanrop (JavaScript)

**Kodeeksempler:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Verktøysanrop med vær-/populasjonsverktøy |
| C# | `csharp/ToolCalling.cs` | Verktøysanrop med .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Verktøysanrop med ChatClient |

---

### Del 12: Bygge et web-UI for Zava Creative Writer

**Laboratoriumsveiledning:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Legg til et nettleserbasert front-end for Zava Creative Writer
- Server det delte UI-et fra Python (FastAPI), JavaScript (Node.js HTTP) og C# (ASP.NET Core)
- Konsumere strømmende NDJSON i nettleseren med Fetch API og ReadableStream
- Live statusmerker for agenter og sanntidsstrømming av artikkeltekst

**Kode (delt UI):**

| Fil | Beskrivelse |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Sideoppsett |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Strømleser og DOM-oppdateringslogikk |

**Backend-tillegg:**

| Språk | Fil | Beskrivelse |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Oppdatert for å servere statisk UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Ny HTTP-server som omslutter orkestratoren |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nytt ASP.NET Core minimal API-prosjekt |

---

### Del 13: Workshop fullført
**Lab-guide:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Sammendrag av alt du har bygget gjennom alle 12 delene
- Videre ideer for å utvide applikasjonene dine
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
| Foundry Local-nettsted | [foundrylocal.ai](https://foundrylocal.ai) |
| Modellkatalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Kom i gang-guide | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referanse | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Lisens

Dette workshop-materialet tilbys for utdanningsformål.

---

**Lykke til med byggingen! 🚀**