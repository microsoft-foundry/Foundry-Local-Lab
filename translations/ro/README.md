<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Atelier Foundry Local - Construiește aplicații AI pe dispozitiv

Un atelier practic pentru rularea modelelor de limbaj pe propriul computer și crearea de aplicații inteligente cu [Foundry Local](https://foundrylocal.ai) și [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Ce este Foundry Local?** Foundry Local este un runtime ușor care îți permite să descarci, să gestionezi și să servești modele de limbaj direct pe hardware-ul tău. Oferă o **API compatibilă cu OpenAI** astfel încât orice instrument sau SDK care comunică cu OpenAI se poate conecta - nu este nevoie de un cont cloud.

### 🌐 Suport Multi-limbă

#### Suportat prin GitHub Action (Automatizat și Întotdeauna Actualizat)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](./README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Preferi să clonezi local?**
>
> Acest depozit include peste 50 de traduceri de limbă, ceea ce crește semnificativ dimensiunea descărcării. Pentru a clona fără traduceri, folosește sparse checkout:
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
> Astfel vei avea tot ce îți trebuie pentru a finaliza cursul, cu o descărcare mult mai rapidă.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Obiective de Învățare

La finalul acestui atelier vei putea:

| # | Obiectiv |
|---|-----------|
| 1 | Instalează Foundry Local și gestionează modelele cu CLI |
| 2 | Stăpânește API-ul Foundry Local SDK pentru management programatic al modelelor |
| 3 | Conectează-te la serverul local de inferență folosind SDK-urile Python, JavaScript și C# |
| 4 | Construiește un pipeline Retrieval-Augmented Generation (RAG) care fundamentează răspunsurile pe datele tale proprii |
| 5 | Creează agenți AI cu instrucțiuni și personalități persistente |
| 6 | Orchestrază fluxuri de lucru multi-agent cu bucle de feedback |
| 7 | Explorează o aplicație capstone de producție - Zava Creative Writer |
| 8 | Construiește cadre de evaluare cu seturi de date aurite și scoruri LLM ca judecător |
| 9 | Transcrie audio cu Whisper - recunoaștere vocală pe dispozitiv folosind Foundry Local SDK |
| 10 | Compilează și rulează modele personalizate sau Hugging Face cu ONNX Runtime GenAI și Foundry Local |
| 11 | Permite modelelor locale să apeleze funcții externe cu modelul tool-calling |
| 12 | Construiește o interfață bazată pe browser pentru Zava Creative Writer cu streaming în timp real |

---

## Cerințe

| Cerință | Detalii |
|-------------|---------|
| **Hardware** | Minimum 8 GB RAM (recomandat 16 GB); CPU compatibil AVX2 sau GPU suportat |
| **Sistem de operare** | Windows 10/11 (x64/ARM), Windows Server 2025 sau macOS 13+ |
| **Foundry Local CLI** | Instalează cu `winget install Microsoft.FoundryLocal` (Windows) sau `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Vezi [ghidul de început](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) pentru detalii. |
| **Runtime limbaj** | **Python 3.9+** și/sau **.NET 9.0+** și/sau **Node.js 18+** |
| **Git** | Pentru clonarea acestui depozit |

---

## Începutul Lucrului

```bash
# 1. Clonați depozitul
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verificați dacă Foundry Local este instalat
foundry model list              # Listați modelele disponibile
foundry model run phi-3.5-mini  # Porniți o discuție interactivă

# 3. Alegeți fluxul de limbă (vedeți laboratorul Partea 2 pentru configurare completă)
```

| Limbaj | Pornire rapidă |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Părți ale Atelierului

### Partea 1: Început cu Foundry Local

**Ghid lab:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Ce este Foundry Local și cum funcționează
- Instalarea CLI pe Windows și macOS
- Explorarea modelelor - listare, descărcare, rulare
- Înțelegerea alias-urilor modelelor și a porturilor dinamice

---

### Partea 2: Deep Dive în SDK Foundry Local

**Ghid lab:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- De ce să folosești SDK în loc de CLI pentru dezvoltarea aplicațiilor
- Referință completă API SDK pentru Python, JavaScript și C#
- Managementul serviciilor, navigarea catalogului, ciclul de viață al modelelor (descărcare, încărcare, descărcare din memorie)
- Modele rapide de pornire: bootstrap constructor Python, `init()` JavaScript, `CreateAsync()` C#
- Metadatele `FoundryModelInfo`, alias-uri și selecția modelelor optime pentru hardware

---

### Partea 3: SDK-uri și API-uri

**Ghid lab:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Conectarea la Foundry Local din Python, JavaScript și C#
- Utilizarea Foundry Local SDK pentru a gestiona serviciul programatic
- Streaming chat completions prin API compatibil OpenAI
- Referință metode SDK pentru fiecare limbaj

**Exemple de cod:**

| Limbaj | Fișier | Descriere |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Chat de streaming de bază |
| C# | `csharp/BasicChat.cs` | Chat de streaming cu .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat de streaming cu Node.js |

---

### Partea 4: Retrieval-Augmented Generation (RAG)

**Ghid lab:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Ce este RAG și de ce este important
- Construirea unei baze de cunoștințe în memorie
- Recuperare prin suprapunere de cuvinte cheie cu scorare
- Compunerea prompturilor sistem fundamentate
- Rularea unui pipeline RAG complet pe dispozitiv

**Exemple de cod:**

| Limbaj | Fișier |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Partea 5: Construirea Agenților AI

**Ghid lab:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Ce este un agent AI (versus un apel brut LLM)
- Modelul `ChatAgent` și Microsoft Agent Framework
- Instrucțiuni sistem, personalități și conversații multi-tur
- Ieșire structurată (JSON) de la agenți

**Exemple de cod:**

| Limbaj | Fișier | Descriere |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent unic cu Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agent unic (model ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agent unic (model ChatAgent) |

---

### Partea 6: Fluxuri de lucru Multi-Agent

**Ghid lab:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline-uri multi-agent: Cercetător → Scriitor → Editor
- Orchestrare secvențială și bucle de feedback
- Configurație partajată și predări structurate
- Proiectează propriul tău flux de lucru multi-agent

**Exemple de cod:**

| Limbaj | Fișier | Descriere |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline cu trei agenți |
| C# | `csharp/MultiAgent.cs` | Pipeline cu trei agenți |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline cu trei agenți |

---

### Partea 7: Zava Creative Writer - Aplicație Capstone

**Ghid lab:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- O aplicație multi-agent de producție cu 4 agenți specializați
- Pipeline secvențial cu bucle de feedback conduse de evaluator
- Ieșire în streaming, căutare catalog produse, predări JSON structurate
- Implementare completă în Python (FastAPI), JavaScript (CLI Node.js) și C# (consolă .NET)

**Exemple de cod:**

| Limbaj | Director | Descriere |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Serviciu web FastAPI cu orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Aplicație CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Aplicație consolă .NET 9 |

---

### Partea 8: Dezvoltare condusă de evaluare

**Ghid lab:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Construiește un cadru sistematic de evaluare pentru agenții AI folosind seturi de date aurite
- Verificări bazate pe reguli (lungime, acoperire cuvinte cheie, termeni interziși) + scorare LLM ca judecător
- Comparare paralelă a variantelor de prompt cu rapoarte agregate
- Extinde modelul agent Zava Editor din Partea 7 într-un test offline
- Trasee Python, JavaScript și C#

**Exemple de cod:**

| Limbaj | Fișier | Descriere |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Cadru de evaluare |
| C# | `csharp/AgentEvaluation.cs` | Cadru de evaluare |
| JavaScript | `javascript/foundry-local-eval.mjs` | Cadru de evaluare |

---

### Partea 9: Transcriere vocală cu Whisper

**Ghid lab:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Transcriere speech-to-text folosind OpenAI Whisper care rulează local
- Procesare audio cu confidențialitate - audio-ul nu părăsește niciodată dispozitivul tău
- Trasee Python, JavaScript și C# cu `client.audio.transcriptions.create()` (Python/JS) și `AudioClient.TranscribeAudioAsync()` (C#)
- Include fișiere audio de probă tematice Zava pentru practică hands-on

**Exemple de cod:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Transcriere vocală Whisper |
| C# | `csharp/WhisperTranscription.cs` | Transcriere vocală Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Transcriere vocală Whisper |

> **Notă:** Acest laborator utilizează **Foundry Local SDK** pentru a descărca și încărca programatic modelul Whisper, apoi trimite audio către endpoint-ul local compatibil OpenAI pentru transcriere. Modelul Whisper (`whisper`) este listat în catalogul Foundry Local și rulează complet pe dispozitiv - nu sunt necesare chei API cloud sau acces la rețea.

---

### Partea 10: Utilizarea modelelor personalizate sau Hugging Face

**Ghidul laboratorului:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compilarea modelelor Hugging Face în format ONNX optimizat folosind ONNX Runtime GenAI model builder
- Compilare specifică hardware-ului (CPU, GPU NVIDIA, DirectML, WebGPU) și cuantizare (int4, fp16, bf16)
- Crearea fișierelor de configurare chat-template pentru Foundry Local
- Adăugarea modelelor compilate în cache-ul Foundry Local
- Rularea modelelor personalizate prin CLI, REST API și OpenAI SDK
- Exemplu de referință: compilarea Qwen/Qwen3-0.6B end-to-end

---

### Partea 11: Apelarea uneltelor cu modele locale

**Ghidul laboratorului:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Permiterea ca modelele locale să apeleze funcții externe (apelarea uneltelor/functii)
- Definirea schemelor uneltelor folosind formatul OpenAI pentru apelarea funcțiilor
- Gestionarea fluxului conversațional multi-turn pentru apelarea uneltelor
- Executarea apelurilor uneltelor local și returnarea rezultatelor către model
- Alegerea modelului potrivit pentru scenarii de apelare unelte (Qwen 2.5, Phi-4-mini)
- Utilizarea `ChatClient` nativ din SDK pentru apelarea uneltelor (JavaScript)

**Exemple de cod:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Apelare unelte cu unelte meteo/populație |
| C# | `csharp/ToolCalling.cs` | Apelare unelte cu .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Apelare unelte cu ChatClient |

---

### Partea 12: Construirea unei interfețe web pentru Zava Creative Writer

**Ghidul laboratorului:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Adăugarea unei interfețe front-end bazate pe browser pentru Zava Creative Writer
- Servirea interfeței UI partajate din Python (FastAPI), JavaScript (Node.js HTTP) și C# (ASP.NET Core)
- Consumarea fluxului NDJSON în browser cu Fetch API și ReadableStream
- Badge-uri de stare agent live și streaming în timp real al textului articolului

**Cod (UI partajat):**

| File | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Layout-ul paginii |
| `zava-creative-writer-local/ui/style.css` | Stilizare |
| `zava-creative-writer-local/ui/app.js` | Logică citire stream și actualizare DOM |

**Adăugiri backend:**

| Language | File | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Actualizat pentru a servi UI static |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nou server HTTP ce înfășoară orchestratorul |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Proiect nou API minimal ASP.NET Core |

---

### Partea 13: Workshop terminat

**Ghidul laboratorului:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Rezumat al tot ceea ce ați construit în toate cele 12 părți
- Idei suplimentare pentru extinderea aplicațiilor
- Linkuri către resurse și documentație

---

## Structura proiectului

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

| Resource | Link |
|----------|------|
| Site Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Catalog modele | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| GitHub Foundry Local | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Ghid de pornire | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referință SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licență

Acest material de workshop este oferit în scopuri educaționale.

---

**Construiește cu plăcere! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Declinare de responsabilitate**:  
Acest document a fost tradus folosind serviciul de traducere AI [Co-op Translator](https://github.com/Azure/co-op-translator). Deși ne străduim pentru acuratețe, vă rugăm să rețineți că traducerile automate pot conține erori sau inexactități. Documentul original în limba sa nativă trebuie considerat sursa autoritară. Pentru informații critice, se recomandă traducerea profesională umană. Nu suntem responsabili pentru eventuale neînțelegeri sau interpretări greșite rezultate din utilizarea acestei traduceri.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->