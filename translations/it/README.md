<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Workshop Foundry Local - Costruisci App AI On-Device

Un workshop pratico per eseguire modelli di linguaggio sulla tua macchina e costruire applicazioni intelligenti con [Foundry Local](https://foundrylocal.ai) e il [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Cos'è Foundry Local?** Foundry Local è un runtime leggero che ti permette di scaricare, gestire e servire modelli di linguaggio interamente sul tuo hardware. Espone un'**API compatibile con OpenAI** così qualsiasi strumento o SDK che utilizza OpenAI può connettersi - senza bisogno di un account cloud.

---

## Obiettivi di Apprendimento

Al termine di questo workshop sarai in grado di:

| # | Obiettivo |
|---|-----------|
| 1 | Installare Foundry Local e gestire i modelli tramite CLI |
| 2 | Padroneggiare l'API SDK di Foundry Local per la gestione programmata dei modelli |
| 3 | Collegarti al server di inferenza locale usando gli SDK Python, JavaScript e C# |
| 4 | Costruire una pipeline Retrieval-Augmented Generation (RAG) che basi le risposte sui tuoi dati |
| 5 | Creare agenti AI con istruzioni e personalità persistenti |
| 6 | Orchestrare flussi di lavoro multi-agente con cicli di feedback |
| 7 | Esplorare un'app di produzione capstone - lo Zava Creative Writer |
| 8 | Costruire framework di valutazione con dataset golden e punteggi LLM-as-judge |
| 9 | Trascrivere audio con Whisper - riconoscimento vocale on-device usando il Foundry Local SDK |
| 10 | Compilare ed eseguire modelli personalizzati o Hugging Face con ONNX Runtime GenAI e Foundry Local |
| 11 | Abilitare i modelli locali a chiamare funzioni esterne con il pattern tool-calling |
| 12 | Costruire un UI browser-based per lo Zava Creative Writer con streaming in tempo reale |

---

## Prerequisiti

| Requisito | Dettagli |
|-------------|---------|
| **Hardware** | Minimo 8 GB di RAM (16 GB raccomandati); CPU con supporto AVX2 o GPU supportata |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, o macOS 13+ |
| **Foundry Local CLI** | Installa tramite `winget install Microsoft.FoundryLocal` (Windows) o `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Consulta la [guida introduttiva](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) per i dettagli. |
| **Runtime linguaggi** | **Python 3.9+** e/o **.NET 9.0+** e/o **Node.js 18+** |
| **Git** | Per clonare questo repository |

---

## Iniziare

```bash
# 1. Clona il repository
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Verifica che Foundry Local sia installato
foundry model list              # Elenca i modelli disponibili
foundry model run phi-3.5-mini  # Avvia una chat interattiva

# 3. Scegli il tuo percorso linguistico (vedi il laboratorio Parte 2 per la configurazione completa)
```

| Linguaggio | Avvio Rapido |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Parti del Workshop

### Parte 1: Introduzione a Foundry Local

**Guida al laboratorio:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Cos'è Foundry Local e come funziona
- Installazione della CLI su Windows e macOS
- Esplorazione dei modelli - elenco, download, esecuzione
- Comprendere gli alias dei modelli e le porte dinamiche

---

### Parte 2: Approfondimento SDK Foundry Local

**Guida al laboratorio:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Perché usare l’SDK invece della CLI per lo sviluppo di applicazioni
- Riferimento completo dell’API SDK per Python, JavaScript e C#
- Gestione del servizio, navigazione del catalogo, ciclo di vita del modello (download, caricamento, scaricamento)
- Pattern di avvio rapido: bootstrap costruttore Python, `init()` JavaScript, `CreateAsync()` C#
- Metadata `FoundryModelInfo`, alias e selezione del modello ottimale per hardware

---

### Parte 3: SDK e API

**Guida al laboratorio:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Connessione a Foundry Local da Python, JavaScript e C#
- Utilizzo dell’SDK Foundry Local per gestire il servizio in modo programmato
- Completamenti chat in streaming tramite API compatibile OpenAI
- Riferimento metodi SDK per ogni linguaggio

**Esempi di codice:**

| Linguaggio | File | Descrizione |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Chat streaming base |
| C# | `csharp/BasicChat.cs` | Chat streaming con .NET |
| JavaScript | `javascript/foundry-local.mjs` | Chat streaming con Node.js |

---

### Parte 4: Retrieval-Augmented Generation (RAG)

**Guida al laboratorio:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Cos’è RAG e perché è importante
- Costruzione di una knowledge base in memoria
- Recupero basato su sovrapposizione di parole chiave con punteggio
- Composizione di prompt di sistema basati su dati concreti
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

- Cos’è un agente AI (a differenza di una semplice chiamata a LLM)
- Pattern `ChatAgent` e Microsoft Agent Framework
- Istruzioni di sistema, personalità e conversazioni multi-turno
- Output strutturato (JSON) dagli agenti

**Esempi di codice:**

| Linguaggio | File | Descrizione |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Singolo agente con Agent Framework |
| C# | `csharp/SingleAgent.cs` | Singolo agente (pattern ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Singolo agente (pattern ChatAgent) |

---

### Parte 6: Flussi di Lavoro Multi-Agente

**Guida al laboratorio:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline multi-agente: Ricercatore → Scrittore → Editore
- Orchestrazione sequenziale e cicli di feedback
- Configurazione condivisa e passaggi strutturati
- Progettazione del proprio flusso di lavoro multi-agente

**Esempi di codice:**

| Linguaggio | File | Descrizione |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline a tre agenti |
| C# | `csharp/MultiAgent.cs` | Pipeline a tre agenti |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline a tre agenti |

---

### Parte 7: Zava Creative Writer - Applicazione Capstone

**Guida al laboratorio:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Un’app multi-agente in stile produzione con 4 agenti specializzati
- Pipeline sequenziale con cicli di feedback guidati da valutatori
- Output in streaming, ricerca nel catalogo prodotti, passaggi JSON strutturati
- Implementazione completa in Python (FastAPI), JavaScript (Node.js CLI), e C# (.NET console)

**Esempi di codice:**

| Linguaggio | Directory | Descrizione |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Servizio web FastAPI con orchestratore |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Applicazione CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Applicazione console .NET 9 |

---

### Parte 8: Sviluppo Guidato da Valutazione

**Guida al laboratorio:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Costruire un framework di valutazione sistematica per agenti AI usando dataset golden
- Controlli basati su regole (lunghezza, copertura parole chiave, termini vietati) + punteggi LLM-as-judge
- Confronto affiancato di varianti di prompt con schede punteggio aggregate
- Estende il pattern agente Zava Editor della Parte 7 in una suite di test offline
- Tracce Python, JavaScript e C#

**Esempi di codice:**

| Linguaggio | File | Descrizione |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Framework di valutazione |
| C# | `csharp/AgentEvaluation.cs` | Framework di valutazione |
| JavaScript | `javascript/foundry-local-eval.mjs` | Framework di valutazione |

---

### Parte 9: Trascrizione Vocale con Whisper

**Guida al laboratorio:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Trascrizione da parlato a testo usando OpenAI Whisper in locale
- Elaborazione audio incentrata sulla privacy - l’audio non lascia mai il tuo dispositivo
- Tracce Python, JavaScript e C# con `client.audio.transcriptions.create()` (Python/JS) e `AudioClient.TranscribeAudioAsync()` (C#)
- Include file audio di esempio a tema Zava per esercitazioni pratiche

**Esempi di codice:**

| Linguaggio | File | Descrizione |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Trascrizione vocale Whisper |
| C# | `csharp/WhisperTranscription.cs` | Trascrizione vocale Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Trascrizione vocale Whisper |

> **Nota:** Questo laboratorio usa il **Foundry Local SDK** per scaricare e caricare programmaticamente il modello Whisper, quindi invia audio all’endpoint locale compatibile OpenAI per la trascrizione. Il modello Whisper (`whisper`) è elencato nel catalogo Foundry Local e funziona interamente on-device - nessuna chiave API cloud o accesso a rete richiesti.

---

### Parte 10: Uso di Modelli Personalizzati o Hugging Face

**Guida al laboratorio:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compilazione di modelli Hugging Face in formato ONNX ottimizzato usando ONNX Runtime GenAI model builder
- Compilazione specifica per hardware (CPU, GPU NVIDIA, DirectML, WebGPU) e quantizzazione (int4, fp16, bf16)
- Creazione di file di configurazione template chat per Foundry Local
- Aggiunta di modelli compilati alla cache di Foundry Local
- Esecuzione di modelli personalizzati tramite CLI, REST API, e SDK OpenAI
- Esempio di riferimento: compilazione end-to-end di Qwen/Qwen3-0.6B

---

### Parte 11: Tool Calling con Modelli Locali

**Guida al laboratorio:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Abilitare modelli locali a chiamare funzioni esterne (tool/function calling)
- Definire schemi strumenti usando il formato OpenAI function-calling
- Gestire il flusso di conversazione multi-turno di tool-calling
- Eseguire chiamate strumenti localmente e restituire risultati al modello
- Scegliere il modello giusto per scenari tool-calling (Qwen 2.5, Phi-4-mini)
- Usare il `ChatClient` nativo dell’SDK per tool calling (JavaScript)

**Esempi di codice:**

| Linguaggio | File | Descrizione |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tool calling con strumenti meteo/popolazione |
| C# | `csharp/ToolCalling.cs` | Tool calling con .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool calling con ChatClient |

---

### Parte 12: Costruire una UI Web per lo Zava Creative Writer

**Guida al laboratorio:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Aggiungere un front end browser-based allo Zava Creative Writer
- Servire l’UI condivisa da Python (FastAPI), JavaScript (Node.js HTTP), e C# (ASP.NET Core)
- Consumare streaming NDJSON nel browser con Fetch API e ReadableStream
- Badge stato agenti live e streaming testo articolo in tempo reale

**Codice (UI condivisa):**

| File | Descrizione |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Layout pagina |
| `zava-creative-writer-local/ui/style.css` | Stile |
| `zava-creative-writer-local/ui/app.js` | Lettore stream e logica aggiornamento DOM |

**Integrazioni backend:**

| Linguaggio | File | Descrizione |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Aggiornato per servire UI statica |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nuovo server HTTP che incapsula l’orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nuovo progetto API minimale ASP.NET Core |

---

### Parte 13: Workshop Completo
**Guida al laboratorio:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Riepilogo di tutto ciò che hai costruito attraverso tutte le 12 parti
- Ulteriori idee per estendere le tue applicazioni
- Link a risorse e documentazione

---

## Struttura del progetto

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
| Sito web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Catalogo modelli | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Guida introduttiva | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Riferimento SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licenza

Questo materiale del laboratorio è fornito a scopo educativo.

---

**Buona costruzione! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Avvertenza**:  
Questo documento è stato tradotto utilizzando il servizio di traduzione AI [Co-op Translator](https://github.com/Azure/co-op-translator). Pur impegnandoci per l’accuratezza, si prega di essere consapevoli che le traduzioni automatiche possono contenere errori o imprecisioni. Il documento originale nella sua lingua nativa deve essere considerato la fonte autorevole. Per informazioni critiche si raccomanda una traduzione professionale effettuata da un umano. Non siamo responsabili per eventuali malintesi o interpretazioni errate derivanti dall’uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->