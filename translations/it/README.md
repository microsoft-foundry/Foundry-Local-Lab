<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Costruisci App AI sul Dispositivo

Un workshop pratico per eseguire modelli linguistici sulla tua macchina e costruire applicazioni intelligenti con [Foundry Local](https://foundrylocal.ai) e il [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Cos'è Foundry Local?** Foundry Local è un runtime leggero che ti permette di scaricare, gestire e servire modelli linguistici interamente sul tuo hardware. Espone un'**API compatibile con OpenAI** così qualsiasi strumento o SDK che usa OpenAI può connettersi - nessun account cloud necessario.

### 🌐 Supporto Multilingue

#### Supportato tramite GitHub Action (Automatizzato e Sempre Aggiornato)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](./README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Preferisci Clonare Localmente?**
>
> Questo repository include più di 50 traduzioni linguistiche che aumentano significativamente la dimensione del download. Per clonare senza traduzioni, usa sparse checkout:
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
> Questo ti dà tutto il necessario per completare il corso con un download molto più veloce.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Obiettivi di Apprendimento

Al termine di questo workshop sarai in grado di:

| # | Obiettivo |
|---|-----------|
| 1 | Installare Foundry Local e gestire modelli con la CLI |
| 2 | Padroneggiare l'API SDK di Foundry Local per la gestione programmatica dei modelli |
| 3 | Connettersi al server di inferenza locale usando gli SDK Python, JavaScript e C# |
| 4 | Costruire una pipeline di Retrieval-Augmented Generation (RAG) che fonda le risposte sui propri dati |
| 5 | Creare agenti AI con istruzioni e persone persistenti |
| 6 | Orchestrare flussi di lavoro multi-agente con loop di feedback |
| 7 | Esplorare un'app capstone di produzione: lo Zava Creative Writer |
| 8 | Costruire framework di valutazione con dataset golden e punteggi LLM-as-judge |
| 9 | Trascrivere audio con Whisper - riconoscimento vocale on-device usando l'SDK Foundry Local |
| 10 | Compilare ed eseguire modelli custom o Hugging Face con ONNX Runtime GenAI e Foundry Local |
| 11 | Abilitare modelli locali a chiamare funzioni esterne con il pattern tool-calling |
| 12 | Costruire un'interfaccia browser per Zava Creative Writer con streaming in tempo reale |

---

## Prerequisiti

| Requisito | Dettagli |
|-------------|---------|
| **Hardware** | Minimo 8 GB RAM (consigliati 16 GB); CPU con supporto AVX2 o GPU supportata |
| **SO** | Windows 10/11 (x64/ARM), Windows Server 2025, o macOS 13+ |
| **Foundry Local CLI** | Installa tramite `winget install Microsoft.FoundryLocal` (Windows) oppure `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Vedi la [guida introduttiva](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) per dettagli. |
| **Runtime linguistici** | **Python 3.9+** e/o **.NET 9.0+** e/o **Node.js 18+** |
| **Git** | Per clonare questo repository |

---

## Per Iniziare

```bash
# 1. Clona il repository
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verifica che Foundry Local sia installato
foundry model list              # Elenca i modelli disponibili
foundry model run phi-3.5-mini  # Avvia una chat interattiva

# 3. Scegli il percorso linguistico (consulta il laboratorio Parte 2 per la configurazione completa)
```

| Linguaggio | Avvio Rapido |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Parti del Workshop

### Parte 1: Prime Nozioni su Foundry Local

**Guida al laboratorio:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Cos'è Foundry Local e come funziona
- Installazione della CLI su Windows e macOS
- Esplorare modelli - elencare, scaricare, eseguire
- Comprendere alias dei modelli e porte dinamiche

---

### Parte 2: Approfondimento SDK Foundry Local

**Guida al laboratorio:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Perché usare l’SDK rispetto alla CLI per lo sviluppo di applicazioni
- Riferimento completo API SDK per Python, JavaScript e C#
- Gestione del servizio, navigazione catalogo, ciclo di vita del modello (scaricare, caricare, scaricare dalla memoria)
- Pattern di avvio rapido: bootstrap del costruttore Python, `init()` in JavaScript, `CreateAsync()` in C#
- Metadati `FoundryModelInfo`, alias, e selezione modello ottimale per hardware

---

### Parte 3: SDK e API

**Guida al laboratorio:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Connessione a Foundry Local da Python, JavaScript e C#
- Uso dell’SDK Foundry Local per gestire il servizio programmaticamente
- Completamenti chat in streaming tramite API compatibile OpenAI
- Riferimento metodi SDK per ogni linguaggio

**Esempi di codice:**

| Linguaggio | File | Descrizione |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Chat in streaming base |
| C# | `csharp/BasicChat.cs` | Chat in streaming con .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat in streaming con Node.js |

---

### Parte 4: Retrieval-Augmented Generation (RAG)

**Guida al laboratorio:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Cos’è RAG e perché è importante
- Costruzione di una base di conoscenza in memoria
- Recupero tramite sovrapposizione di parole chiave con punteggio
- Composizione di prompt di sistema contestualizzati
- Esecuzione completa di una pipeline RAG on-device

**Esempi di codice:**

| Linguaggio | File |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Parte 5: Costruire Agenti AI

**Guida al laboratorio:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Cos’è un agente AI (rispetto a una semplice chiamata LLM)
- Pattern `ChatAgent` e Microsoft Agent Framework
- Istruzioni di sistema, personalità e conversazioni multi-turno
- Output strutturato (JSON) dagli agenti

**Esempi di codice:**

| Linguaggio | File | Descrizione |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agente singolo con Agent Framework |
| C# | `csharp/SingleAgent.cs` | Agente singolo (pattern ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Agente singolo (pattern ChatAgent) |

---

### Parte 6: Flussi Multi-Agente

**Guida al laboratorio:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline multi-agente: Ricercatore → Scrittore → Editor
- Orchestrazione sequenziale e loop di feedback
- Configurazione condivisa e passaggi strutturati
- Progettare il proprio flusso multi-agente

**Esempi di codice:**

| Linguaggio | File | Descrizione |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline con tre agenti |
| C# | `csharp/MultiAgent.cs` | Pipeline con tre agenti |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline con tre agenti |

---

### Parte 7: Zava Creative Writer - Applicazione Capstone

**Guida al laboratorio:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- App multi-agente in stile produzione con 4 agenti specializzati
- Pipeline sequenziale con loop di feedback guidati da valutatore
- Output in streaming, ricerca catalogo prodotti, passaggi JSON strutturati
- Implementazione completa in Python (FastAPI), JavaScript (CLI Node.js), e C# (console .NET)

**Esempi di codice:**

| Linguaggio | Directory | Descrizione |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Servizio web FastAPI con orchestratore |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Applicazione CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Applicazione console .NET 9 |

---

### Parte 8: Sviluppo Guidato da Valutazione

**Guida al laboratorio:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Costruire un framework sistematico di valutazione per agenti AI usando dataset golden
- Controlli basati su regole (lunghezza, copertura parole chiave, termini proibiti) + punteggio LLM-as-judge
- Confronto affiancato di varianti di prompt con schede punteggio aggregate
- Estende il pattern agente Editor Zava della Parte 7 in una suite di test offline
- Percorsi Python, JavaScript e C#

**Esempi di codice:**

| Linguaggio | File | Descrizione |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Framework di valutazione |
| C# | `csharp/AgentEvaluation.cs` | Framework di valutazione |
| JavaScript | `javascript/foundry-local-eval.mjs` | Framework di valutazione |

---

### Parte 9: Trascrizione Vocale con Whisper

**Guida al laboratorio:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Trascrizione vocale in testo con OpenAI Whisper eseguito localmente
- Elaborazione audio con privacy prioritaria - l'audio non lascia mai il tuo dispositivo
- Tracce Python, JavaScript e C# con `client.audio.transcriptions.create()` (Python/JS) e `AudioClient.TranscribeAudioAsync()` (C#)
- Include file audio di esempio a tema Zava per esercitazioni pratiche

**Esempi di codice:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Trascrizione vocale Whisper |
| C# | `csharp/WhisperTranscription.cs` | Trascrizione vocale Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Trascrizione vocale Whisper |

> **Nota:** Questo laboratorio utilizza il **Foundry Local SDK** per scaricare e caricare programmaticamente il modello Whisper, quindi invia l'audio all'endpoint locale compatibile con OpenAI per la trascrizione. Il modello Whisper (`whisper`) è elencato nel catalogo Foundry Local e viene eseguito interamente sul dispositivo - non sono necessarie chiavi API cloud né accesso alla rete.

---

### Parte 10: Uso di Modelli Personalizzati o Hugging Face

**Guida del laboratorio:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compilazione di modelli Hugging Face nel formato ONNX ottimizzato utilizzando il generatore di modelli GenAI ONNX Runtime
- Compilazione specifica per hardware (CPU, GPU NVIDIA, DirectML, WebGPU) e quantizzazione (int4, fp16, bf16)
- Creazione di file di configurazione template chat per Foundry Local
- Aggiunta di modelli compilati alla cache di Foundry Local
- Esecuzione di modelli personalizzati tramite CLI, REST API e OpenAI SDK
- Esempio di riferimento: compilazione completa di Qwen/Qwen3-0.6B

---

### Parte 11: Chiamate di Strumenti con Modelli Locali

**Guida del laboratorio:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Abilitare i modelli locali a chiamare funzioni esterne (tool/function calling)
- Definire schemi degli strumenti usando il formato di chiamata funzione OpenAI
- Gestire il flusso di conversazione multi-turno per le chiamate agli strumenti
- Eseguire chiamate agli strumenti localmente e restituire i risultati al modello
- Scegliere il modello giusto per scenari di chiamata a strumenti (Qwen 2.5, Phi-4-mini)
- Usare il `ChatClient` nativo dell'SDK per le chiamate a strumenti (JavaScript)

**Esempi di codice:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tool calling con strumenti meteo/popolazione |
| C# | `csharp/ToolCalling.cs` | Tool calling con .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool calling con ChatClient |

---

### Parte 12: Costruire un'interfaccia Web per il Zava Creative Writer

**Guida del laboratorio:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Aggiungere un front end basato su browser per il Zava Creative Writer
- Erogare l'interfaccia condivisa da Python (FastAPI), JavaScript (Node.js HTTP) e C# (ASP.NET Core)
- Consumare NDJSON in streaming nel browser con Fetch API e ReadableStream
- Badge di stato agente live e streaming in tempo reale del testo dell’articolo

**Codice (UI condivisa):**

| File | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Layout della pagina |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Lettore di streaming e logica di aggiornamento DOM |

**Aggiunte backend:**

| Language | File | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Aggiornato per erogare UI statica |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nuovo server HTTP che incapsula l’orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nuovo progetto API minimal ASP.NET Core |

---

### Parte 13: Workshop Completato

**Guida del laboratorio:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Riepilogo di tutto ciò che hai costruito nelle 12 parti
- Idee aggiuntive per estendere le tue applicazioni
- Link a risorse e documentazione

---

## Struttura del Progetto

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

## Risorse

| Risorsa | Link |
|----------|------|
| Sito Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Catalogo modelli | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local su GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Guida introduttiva | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Riferimento SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licenza

Questo materiale del workshop è fornito a scopo educativo.

---

**Buona costruzione! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:  
Questo documento è stato tradotto utilizzando il servizio di traduzione automatica [Co-op Translator](https://github.com/Azure/co-op-translator). Pur impegnandoci per l'accuratezza, si prega di considerare che le traduzioni automatiche possono contenere errori o inesattezze. Il documento originale nella sua lingua nativa deve essere considerato la fonte autorevole. Per informazioni critiche, è consigliata la traduzione professionale umana. Non siamo responsabili per eventuali malintesi o interpretazioni errate derivanti dall'uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->