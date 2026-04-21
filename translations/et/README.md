<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local töötoa - ehita tehisintellekti rakendusi seadmes

Praktiline töötuba keelemudelite jooksutamiseks enda masinas ja intelligentsed rakenduste loomiseks koos [Foundry Local](https://foundrylocal.ai) ja [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) raamistikega.

> **Mis on Foundry Local?** Foundry Local on kerge käituskeskkond, mis võimaldab keelemudeleid täielikult sinu riistvaral alla laadida, hallata ja teenindada. See pakub **OpenAI-ühilduvat API-d**, nii et mistahes tööriist või SDK, mis toetab OpenAI-d, saab sellega ühenduda – pilvekonto pole vajalik.

---

## Õpieesmärgid

Selle töötoa lõpuks oskad:

| # | Eesmärk |
|---|---------|
| 1 | Paigaldada Foundry Local ja hallata mudeleid käsurealt (CLI) |
| 2 | Valdada Foundry Local SDK API-t programmipõhiseks mudelite haldamiseks |
| 3 | Ühenduda kohaliku inference serveriga Python, JavaScript ja C# SDK-dega |
| 4 | Luua otsingupõhine (RAG) torujuhe, mis tugineb sinu enda andmetele vastuste toetamiseks |
| 5 | Luua AI agendid püsivate juhiste ja persoonidega |
| 6 | Orkestreerida mitme-agendi töövooge tagasisilmustega |
| 7 | Avastada tootmistaseme projekt – Zava Creative Writer |
| 8 | Luua hindamiskehastud kullastandarditega ja LLM-eeskuju hinde süsteemiga |
| 9 | Üles kirjutada heli Whisperiga – kõnest tekstiks seadmes Foundry Local SDK-ga |
| 10 | Kokkupakkida ja käivitada kohandatud või Hugging Face mudeleid ONNX Runtime GenAI ja Foundry Localiga |
| 11 | Lubada kohalikel mudelitel kutsuda välisfunktsioone tööriista-kutsumise mustri kaudu |
| 12 | Ehita veebipõhine kasutajaliides Zava Creative Writerile reaalajas voogedastusega |

---

## Nõuded

| Nõue | Detailid |
|-------------|---------|
| **Riistvara** | Vähemalt 8 GB RAM (soovitatav 16 GB); AVX2-tugi protsessor või toetatud GPU |
| **Operatsioonisüsteem** | Windows 10/11 (x64/ARM), Windows Server 2025 või macOS 13+ |
| **Foundry Local CLI** | Paigalda `winget install Microsoft.FoundryLocal` (Windows) või `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Vaata [algusjuhendit](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) täpsemaks info saamiseks. |
| **Keeletõlgendusmasin** | **Python 3.9+** ja/või **.NET 9.0+** ja/või **Node.js 18+** |
| **Git** | Selle repositooriumi kloonimiseks |

---

## Algus

```bash
# 1. Klooni hoidla
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Kontrolli, kas Foundry Local on installitud
foundry model list              # Loetle saadaolevad mudelid
foundry model run phi-3.5-mini  # Alusta interaktiivset vestlust

# 3. Vali oma keele rada (täieliku seadistuse jaoks vaata osa 2 laborit)
```

| Keel | Kiiralgus |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Töötoa osad

### Osa 1: Tutvumine Foundry Localiga

**Laborijuhend:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Mis on Foundry Local ja kuidas see töötab
- CLI paigaldamine Windowsis ja macOS-is
- Mudelite uurimine – loetelu, allalaadimine, käivitamine
- Mudeli hüüdnimede ja dünaamiliste portide mõistmine

---

### Osa 2: Foundry Local SDK süvitsi

**Laborijuhend:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Miks eelistada SDK-d CLI-le rakenduste arendamisel
- Täielik SDK API viide Pythonile, JavaScriptile ja C#-le
- Teenuse haldus, kataloogi sirvimine, mudelite elutsükkel (allalaadimine, laadimine, mahalaadimine)
- Kiiralguse mustrid: Python konstruktor, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metaandmed, hüüdnimed ja riistvaraliselt optimaalse mudeli valik

---

### Osa 3: SDK-d ja API-d

**Laborijuhend:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Ühendamine Foundry Localiga Pythonist, JavaScriptist ja C#-st
- Foundry Local SDK kasutamine teenuse programmipõhiseks haldamiseks
- Vestluse voogedastus OpenAI-ühilduva API kaudu
- SDK meetodite viited iga keele kohta

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Lihtne voogedastuse vestlus |
| C# | `csharp/BasicChat.cs` | Vestlus voogedastusena .NET-is |
| JavaScript | `javascript/foundry-local.mjs` | Node.js voogedastuse vestlus |

---

### Osa 4: Otsingupõhine genereerimine (RAG)

**Laborijuhend:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Mis on RAG ja miks see oluline on
- Mälu põhine teadmistebaasi loomine
- Märksõnade kattuvuse alusel otsimine ja skoorimine
- Alusbaasil põhinevate süsteemipäringute loomine
- Täieliku RAG-toru seadistamine ja käivitamine seadmes

**Koodinäited:**

| Keel | Fail |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Osa 5: AI agentide ehitamine

**Laborijuhend:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Mis on AI agent (võrreldes tavalise LLM-kutsega)
- `ChatAgent` muster ja Microsoft Agent Framework
- Süsteemi juhised, persoonad ja mitme vooru vestlused
- Struktureeritud väljund (JSON) agentidelt

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Üksik agent Agent Frameworkiga |
| C# | `csharp/SingleAgent.cs` | Üksik agent (ChatAgent muster) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Üksik agent (ChatAgent muster) |

---

### Osa 6: Mitme-agendi töövood

**Laborijuhend:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Mitme-agendi torud: uurija → kirjanik → toimetaja
- Järjestikune orkestreerimine ja tagasisilmused
- Jagatud konfiguratsioon ja struktureeritud üleandmised
- Oma mitme-agendi töövoo kavandamine

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Kolme-agentne toru |
| C# | `csharp/MultiAgent.cs` | Kolme-agentne toru |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Kolme-agentne toru |

---

### Osa 7: Zava Creative Writer - lõpetav rakendus

**Laborijuhend:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Tootmisele orienteeritud mitme-agendi rakendus nelja spetsialiseerunud agentiga
- Järjestikune torujuhe hindaja juhitud tagasisilmustega
- Voogedastuse väljund, toodete kataloogi otsing, struktureeritud JSON üleandmised
- Täisrakendus Pythonis (FastAPI), JavaScriptis (Node.js CLI) ja C#-s (.NET konsool)

**Koodinäited:**

| Keel | Kaust | Kirjeldus |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI veebiteenus koos orchestratoriga |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI rakendus |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konsoolirakendus |

---

### Osa 8: Hindamispõhine arendus

**Laborijuhend:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Süsteemse hindamiskehastiku loomine AI agentidele kullastandardite andmestikega
- Reeglipõhised kontrollid (pikkus, märksõnade katvus, keelatud terminid) + LLM kohtumõistjana hinde andmine
- Põhjalik arvutuskavadega võrdlus erinevate päringuvariantide vahel
- Zava Toimetaja agentuuri mustri laiendus osa 7-st offline testipaketiks
- Python, JavaScript ja C# harud

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Hindamiskehastik |
| C# | `csharp/AgentEvaluation.cs` | Hindamiskehastik |
| JavaScript | `javascript/foundry-local-eval.mjs` | Hindamiskehastik |

---

### Osa 9: Heli transkriptsioon Whisperiga

**Laborijuhend:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Kõnest tekstiks transkriptsioon lokaalselt töötava OpenAI Whisper abil
- Privaatsustähtis helitöötlus – heli ei lahku kunagi sinu seadmest
- Python, JavaScript ja C# harud koos `client.audio.transcriptions.create()` (Python/JS) ja `AudioClient.TranscribeAudioAsync()` (C#)
- Sisaldab Zava-teemalisi helinäidiseid praktiliseks harjutamiseks

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisperi häältranskriptsioon |
| C# | `csharp/WhisperTranscription.cs` | Whisperi häältranskriptsioon |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisperi häältranskriptsioon |

> **Märkus:** See labor kasutab **Foundry Local SDK-d** Whisperi mudeli programmipõhiseks allalaadimiseks ja laadimiseks, seejärel saadab heli lokaalsele OpenAI-ühilduvale endpointile transkriptsiooniks. Whisper mudel (`whisper`) on listitud Foundry Local kataloogis ja töötab täielikult seadmes – pilve API võti ega võrguühendus pole vajalik.

---

### Osa 10: Kohandatud või Hugging Face mudelite kasutamine

**Laborijuhend:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face mudelite kokkupanek optimeeritud ONNX formaati ONNX Runtime GenAI mudeli ehitajaga
- Riistvaraspetsiifiline kokkupanek (CPU, NVIDIA GPU, DirectML, WebGPU) ja kvantiseerimine (int4, fp16, bf16)
- Vestlusmallide konfiguratsioonifailide loomine Foundry Localile
- Kokkupandud mudelite lisamine Foundry Local vahemällu
- Kohandatud mudelite jooksutamine CLI, REST API ja OpenAI SDK kaudu
- Näide: Qwen/Qwen3-0.6B lõpp-lõpuni kokkupanek

---

### Osa 11: Tööriistakutsete võimaldamine kohalike mudelitega

**Laborijuhend:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Luba kohalikel mudelitel kutsuda välisfunktsioone (tööriistade/funktsioonide kutsumine)
- Defineeri tööriistad skeemidega OpenAI funktsiooni-kutsumise formaadis
- Töötle mitme vooruga tööriistakutsete vestlust
- Täida tööriistakutsed kohapeal ja tagasta tulemused mudelile
- Vali tööriistakutsete stsenaariumides sobiv mudel (Qwen 2.5, Phi-4-mini)
- Kasuta SDK natiivset `ChatClient`-i tööriistakutseteks (JavaScript)

**Koodinäited:**

| Keel | Fail | Kirjeldus |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tööriistakutsed ilmastiku/rahvastiku tööriistadega |
| C# | `csharp/ToolCalling.cs` | Tööriistakutsed .NET-is |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tööriistakutsed ChatClientiga |

---

### Osa 12: Veebikasutajaliidese ehitamine Zava Creative Writerile

**Laborijuhend:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Lisa Zava Creative Writerile veebipõhine kasutajaliides
- Serveeri ühiskasutuses olevat UI-d Pythonist (FastAPI), JavaScriptist (Node.js HTTP) ja C#-st (ASP.NET Core)
- Tarbi voogedastuse NDJSONit brauseris Fetch API ja ReadableStream abil
- Otseagentide staatuse märgid ja artikli teksti reaalajas voogedastus

**Kood (ühiskasutuses olev UI):**

| Fail | Kirjeldus |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Lehekülje paigutus |
| `zava-creative-writer-local/ui/style.css` | Stiilid |
| `zava-creative-writer-local/ui/app.js` | Voo lugeja ja DOM-i uuendamise loogika |

**Tagapõhja täiendused:**

| Keel | Fail | Kirjeldus |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Uuendatud staatilise UI teenindamiseks |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Uus HTTP-server, mis ümbritseb orchestratorit |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Uus ASP.NET Core minimaalne API projekt |

---

### Osa 13: Töötuba valmis
**Laborijuhend:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Ülevaade kõigest, mida olete ehitanud kõigi 12 osa jooksul
- Edasised ideed oma rakenduste laiendamiseks
- Lingid ressurssidele ja dokumentatsioonile

---

## Projektistruktuur

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

## Ressursid

| Ressurss | Link |
|----------|------|
| Foundry Local veebisait | [foundrylocal.ai](https://foundrylocal.ai) |
| Mudelite kataloog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Alustamise juhend | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK viited | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Litsents

See töötoa materjal on mõeldud hariduslikel eesmärkidel.

---

**Edu ehitamisel! 🚀**