<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Atelier Foundry Local - Construiește Aplicații AI Pe Dispozitiv

Un atelier practic pentru a rula modele lingvistice pe propriul tău calculator și a construi aplicații inteligente cu [Foundry Local](https://foundrylocal.ai) și [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Ce este Foundry Local?** Foundry Local este un runtime ușor care îți permite să descarci, gestionezi și să servești modele lingvistice complet pe hardware-ul tău. Oferă o **API compatibilă OpenAI** astfel încât orice unealtă sau SDK care suportă OpenAI se poate conecta - nu este nevoie de cont cloud.

---

## Obiective de Învățare

La sfârșitul acestui atelier vei putea:

| # | Obiectiv |
|---|-----------|
| 1 | Instalează Foundry Local și gestionează modelele folosind CLI-ul |
| 2 | Stăpânește API-ul Foundry Local SDK pentru gestionarea programatică a modelelor |
| 3 | Conectează-te la serverul local de inferență folosind SDK-urile Python, JavaScript și C# |
| 4 | Construiește un flux Retrieval-Augmented Generation (RAG) care să ancoreze răspunsuri în datele tale |
| 5 | Creează agenți AI cu instrucțiuni și personalități persistente |
| 6 | Orchestrează fluxuri multi-agent cu bucle de feedback |
| 7 | Explorează o aplicație capstone de producție - Zava Creative Writer |
| 8 | Construiește cadre de evaluare cu seturi de date de aur și scorare LLM-ca-judecător |
| 9 | Transcrie audio cu Whisper - conversie vorbire-text pe dispozitiv folosind Foundry Local SDK |
| 10 | Compilează și rulează modele personalizate sau Hugging Face cu ONNX Runtime GenAI și Foundry Local |
| 11 | Permite modelelor locale să apeleze funcții externe prin pattern-ul tool-calling |
| 12 | Construiește o interfață bazată pe browser pentru Zava Creative Writer cu streaming în timp real |

---

## Cerințe Prealabile

| Cerință | Detalii |
|-------------|---------|
| **Hardware** | Minim 8 GB RAM (16 GB recomandat); CPU compatibil AVX2 sau GPU suportat |
| **Sistem de operare** | Windows 10/11 (x64/ARM), Windows Server 2025 sau macOS 13+ |
| **Foundry Local CLI** | Instalează folosind `winget install Microsoft.FoundryLocal` (Windows) sau `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Vezi [ghidul de început](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) pentru detalii. |
| **Limbaj de rulare** | **Python 3.9+** și/sau **.NET 9.0+** și/sau **Node.js 18+** |
| **Git** | Pentru clonarea acestui depozit |

---

## Introducere

```bash
# 1. Clonează depozitul
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verifică dacă Foundry Local este instalat
foundry model list              # Listează modelele disponibile
foundry model run phi-3.5-mini  # Pornește o conversație interactivă

# 3. Alege-ți limba de studiu (vezi laboratorul Partea 2 pentru configurarea completă)
```

| Limbaj | Pornire rapidă |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Părțile Atelierului

### Partea 1: Început cu Foundry Local

**Ghid lab:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Ce este Foundry Local și cum funcționează
- Instalarea CLI pe Windows și macOS
- Explorarea modelelor - listare, descărcare, rulare
- Înțelegerea aliasurilor modelelor și porturilor dinamice

---

### Partea 2: Explorare detaliată Foundry Local SDK

**Ghid lab:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- De ce să folosești SDK-ul în locul CLI-ului pentru dezvoltarea aplicațiilor
- Referință completă API SDK pentru Python, JavaScript și C#
- Gestionarea serviciilor, navigarea în catalog, ciclul de viață al modelului (descărcare, încărcare, descărcare)
- Modele rapide de start: bootstrap constructor Python, `init()` JavaScript, `CreateAsync()` C#
- Metadatele `FoundryModelInfo`, aliasuri și selectarea modelului optim pentru hardware

---

### Partea 3: SDK-uri și API-uri

**Ghid lab:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Conectarea la Foundry Local din Python, JavaScript și C#
- Folosirea Foundry Local SDK pentru a gestiona serviciul programatic
- Transmisiuni chat în streaming prin API-ul compatibil OpenAI
- Referință metode SDK pentru fiecare limbaj

**Exemple de cod:**

| Limbaj | Fișier | Descriere |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Chat streaming de bază |
| C# | `csharp/BasicChat.cs` | Chat streaming cu .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat streaming cu Node.js |

---

### Partea 4: Retrieval-Augmented Generation (RAG)

**Ghid lab:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Ce este RAG și de ce este important
- Construirea unei baze de cunoștințe în memorie
- Recuperare cu suprapunere de cuvinte cheie și scorare
- Compoziția prompturilor de sistem ancorate
- Rularea unui flux complet RAG pe dispozitiv

**Exemple de cod:**

| Limbaj | Fișier |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Partea 5: Construirea Agenților AI

**Ghid lab:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Ce este un agent AI (vs. apelul direct LLM)
- Pattern-ul `ChatAgent` și Microsoft Agent Framework
- Instrucțiuni de sistem, personalități și conversații cu mai multe tururi
- Ieșire structurată (JSON) de la agenți

**Exemple de cod:**

| Limbaj | Fișier | Descriere |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent unic cu Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agent unic (pattern ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agent unic (pattern ChatAgent) |

---

### Partea 6: Fluxuri Multi-Agent

**Ghid lab:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipelines multi-agent: Cercetător → Scriitor → Editor
- Orchestrare sequentială și bucle de feedback
- Configurare partajată și predări structurate
- Proiectarea propriului tău flux multi-agent

**Exemple de cod:**

| Limbaj | Fișier | Descriere |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline cu trei agenți |
| C# | `csharp/MultiAgent.cs` | Pipeline cu trei agenți |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline cu trei agenți |

---

### Partea 7: Zava Creative Writer - Aplicație Capstone

**Ghid lab:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- O aplicație multi-agent tip producție cu 4 agenți specializați
- Pipeline secvențial cu bucle de feedback conduse de evaluator
- Ieșire în streaming, căutare în catalog produse, predări JSON structurate
- Implementare completă în Python (FastAPI), JavaScript (Node.js CLI) și C# (consolă .NET)

**Exemple de cod:**

| Limbaj | Director | Descriere |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Serviciu web FastAPI cu orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplicație CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplicație consolă .NET 9 |

---

### Partea 8: Dezvoltare Condusă de Evaluare

**Ghid lab:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Construiește un cadru sistematic de evaluare pentru agenți AI folosind seturi de date de aur
- Verificări bazate pe reguli (lungime, acoperire cuvânt cheie, termeni interziși) + scorare LLM-ca-judecător
- Comparare paralelă a variantelor de prompt cu foi de scor agregate
- Extinde pattern-ul agentului Editor Zava din Partea 7 într-un pachet de teste offline
- Trase Python, JavaScript și C#

**Exemple de cod:**

| Limbaj | Fișier | Descriere |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Cadru de evaluare |
| C# | `csharp/AgentEvaluation.cs` | Cadru de evaluare |
| JavaScript | `javascript/foundry-local-eval.mjs` | Cadru de evaluare |

---

### Partea 9: Transcriere Vocală cu Whisper

**Ghid lab:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Transcriere vorbire-text folosind OpenAI Whisper în modul local
- Procesare audio cu prioritate pentru confidențialitate - audio-ul nu părăsește niciodată dispozitivul tău
- Trase Python, JavaScript și C# cu `client.audio.transcriptions.create()` (Python/JS) și `AudioClient.TranscribeAudioAsync()` (C#)
- Include fișiere audio tematice Zava pentru practică practică

**Exemple de cod:**

| Limbaj | Fișier | Descriere |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transcriere vocală Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transcriere vocală Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transcriere vocală Whisper |

> **Notă:** Acest laborator folosește **Foundry Local SDK** pentru a descărca și încărca programatic modelul Whisper, apoi trimite audio către endpoint-ul local compatibil OpenAI pentru transcriere. Modelul Whisper (`whisper`) este listat în catalogul Foundry Local și rulează complet pe dispozitiv - nu sunt necesare chei API cloud sau acces la rețea.

---

### Partea 10: Folosirea modelelor personalizate sau Hugging Face

**Ghid lab:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compilarea modelelor Hugging Face în format ONNX optimizat folosind ONNX Runtime GenAI model builder
- Compilare specifică hardware-ului (CPU, GPU NVIDIA, DirectML, WebGPU) și cuantizare (int4, fp16, bf16)
- Crearea fișierelor de configurare chat-template pentru Foundry Local
- Adăugarea modelelor compilate în cache-ul Foundry Local
- Rularea modelelor personalizate prin CLI, REST API și OpenAI SDK
- Exemplu de referință: compilarea completă a Qwen/Qwen3-0.6B

---

### Partea 11: Apelarea uneltelor cu Modele Locale

**Ghid lab:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Permite modelelor locale să apeleze funcții externe (tool/function calling)
- Definirea schemelor uneltelor folosind formatul OpenAI function-calling
- Gestionarea fluxului conversațional multi-turn de apelare unelte
- Executarea apelurilor de unelte local și întoarcerea rezultatelor către model
- Alegerea modelului potrivit pentru scenarii tool-calling (Qwen 2.5, Phi-4-mini)
- Folosirea nativei `ChatClient` a SDK pentru tool-calling (JavaScript)

**Exemple de cod:**

| Limbaj | Fișier | Descriere |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Apelare unelte cu uneltele meteo/populație |
| C# | `csharp/ToolCalling.cs` | Apelare unelte cu .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Apelare unelte cu ChatClient |

---

### Partea 12: Construirea unei UI Web pentru Zava Creative Writer

**Ghid lab:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Adaugă un front-end bazat pe browser pentru Zava Creative Writer
- Servește UI-ul partajat din Python (FastAPI), JavaScript (Node.js HTTP) și C# (ASP.NET Core)
- Consumă NDJSON stream în browser cu Fetch API și ReadableStream
- Insigne starea agentului live și streaming text articol în timp real

**Cod (UI partajat):**

| Fișier | Descriere |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Layout-ul paginii |
| `zava-creative-writer-local/ui/style.css` | Stilizare |
| `zava-creative-writer-local/ui/app.js` | Logică cititor stream și actualizare DOM |

**Adăugiri backend:**

| Limbaj | Fișier | Descriere |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Actualizat pentru servirea UI static |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Server HTTP nou care învelește orchestratorul |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Proiect nou ASP.NET Core minimal API |

---

### Partea 13: Atelier finalizat
**Ghid de laborator:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Rezumatul a tot ce ai construit în toate cele 12 părți
- Idei suplimentare pentru extinderea aplicațiilor tale
- Linkuri către resurse și documentație

---

## Structura Proiectului

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

## Resurse

| Resursă | Link |
|----------|------|
| Site-ul Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Catalog de modele | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Ghid de început | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referință SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licență

Acest material de workshop este oferit în scopuri educaționale.

---

**Construiește cu plăcere! 🚀**