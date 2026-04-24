<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Build AI Apps On-Device

Na hands-on workshop wey go help you run language models for your own machine and build smart applications wit [Foundry Local](https://foundrylocal.ai) and di [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Wetin be Foundry Local?** Foundry Local na small runtime wey make you fit download, manage, and run language models complete for your own hardware. E get **OpenAI-compatible API** so any tool or SDK wey sabi OpenAI fit connect - no cloud account needed.

### 🌐 Multi-Language Support

#### Supported via GitHub Action (Automated & Always Up-to-Date)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](./README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **You prefer make you Clone Am for Your Machine?**
>
> Dis repository get pass 50 language translations wey go make di download size big well well. If you want clone without di translations, make you use sparse checkout:
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
> Dis one go give you everything wey you need to finish di course faster.

<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Learning Objectives

By di time you finish dis workshop, you go fit:

| # | Objective |
|---|-----------|
| 1 | Install Foundry Local and manage models with the CLI |
| 2 | Master di Foundry Local SDK API for programmatic model management |
| 3 | Connect to di local inference server using di Python, JavaScript, and C# SDKs |
| 4 | Build Retrieval-Augmented Generation (RAG) pipeline wey dey base answers on your own data |
| 5 | Create AI agents with instructions wey no go change and personas |
| 6 | Run multiple-agent workflows with feedback loops |
| 7 | Check out one production-level app - di Zava Creative Writer |
| 8 | Build evaluation frameworks using golden datasets and LLM-as-judge scoring |
| 9 | Transcribe audio with Whisper - speech-to-text on-device using di Foundry Local SDK |
| 10 | Compile and run custom or Hugging Face models with ONNX Runtime GenAI and Foundry Local |
| 11 | Make local models fit call external functions with di tool-calling pattern |
| 12 | Build one browser-based UI for di Zava Creative Writer wit real-time streaming |

---

## Prerequisites

| Requirement | Details |
|-------------|---------|
| **Hardware** | 8 GB RAM minimum (16 GB better); AVX2-capable CPU or one GPU wey dem support |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, or macOS 13+ |
| **Foundry Local CLI** | Install am with `winget install Microsoft.FoundryLocal` (Windows) or `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Check di [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) for more info. |
| **Language runtime** | **Python 3.9+** and/or **.NET 9.0+** and/or **Node.js 18+** |
| **Git** | To clone this repository |

---

## Getting Started

```bash
# 1. Copy di repository
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Make sure say Foundry Local don install
foundry model list              # Show di models wey dey available
foundry model run phi-3.5-mini  # Start interactive chat

# 3. Pick your language track (look Part 2 lab for full setup)
```

| Language | Quick Start |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshop Parts

### Part 1: Getting Started with Foundry Local

**Lab guide:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Wetin be Foundry Local and how e dey work
- How to install di CLI for Windows and macOS
- Explore models - list am, download am, run am
- Understand model aliases and dynamic ports

---

### Part 2: Foundry Local SDK Deep Dive

**Lab guide:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Why you suppose use SDK pass di CLI for app development
- Full SDK API reference for Python, JavaScript, and C#
- Service management, catalog browsing, model lifecycle (download, load, unload)
- Quick-start patterns: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, aliases, and best hardware model selection

---

### Part 3: SDKs and APIs

**Lab guide:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- How to connect to Foundry Local from Python, JavaScript, and C#
- How to use Foundry Local SDK to manage the service programmatically
- Streaming chat completions through OpenAI-compatible API
- SDK method reference for each language

**Code samples:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Basic streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat with .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat with Node.js |

---

### Part 4: Retrieval-Augmented Generation (RAG)

**Lab guide:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Wetin be RAG and why e important
- Build in-memory knowledge base
- Keyword-overlap retrieval with scoring
- Compose grounded system prompts
- Run full RAG pipeline on-device

**Code samples:**

| Language | File |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Part 5: Building AI Agents

**Lab guide:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Wetin be AI agent (compared to direct LLM call)
- Di `ChatAgent` pattern and Microsoft Agent Framework
- System instructions, personas, and multi-turn talks
- Structured output (JSON) from agents

**Code samples:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Single agent with Agent Framework |
| C# | `csharp/SingleAgent.cs` | Single agent (ChatAgent pattern) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Single agent (ChatAgent pattern) |

---

### Part 6: Multi-Agent Workflows

**Lab guide:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Multi-agent pipeline: Researcher → Writer → Editor
- Sequential organisation and feedback loops
- Shared configuration and structured hand-offs
- Design your own multi-agent workflow

**Code samples:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Three-agent pipeline |
| C# | `csharp/MultiAgent.cs` | Three-agent pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Three-agent pipeline |

---

### Part 7: Zava Creative Writer - Capstone Application

**Lab guide:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Production-style multi-agent app with 4 specialised agents
- Sequential pipeline with evaluator-driven feedback loops
- Streaming output, product catalog search, structured JSON hand-offs
- Full implementation in Python (FastAPI), JavaScript (Node.js CLI), and C# (.NET console)

**Code samples:**

| Language | Directory | Description |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI web service with orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI app |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 console app |

---

### Part 8: Evaluation-Led Development

**Lab guide:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Build systematic evaluation framework for AI agents using golden datasets
- Rule-based checks (length, keyword coverage, forbidden words) + LLM-as-judge scoring
- Side-by-side comparison of prompt versions with aggregate scorecards
- Extend di Zava Editor agent pattern from Part 7 into offline test suite
- Python, JavaScript, and C# tracks

**Code samples:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evaluation framework |
| C# | `csharp/AgentEvaluation.cs` | Evaluation framework |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evaluation framework |

---

### Part 9: Voice Transcription with Whisper

**Lab guide:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Speech-to-text transcription wey dey use OpenAI Whisper wey dey run locally
- Privacy-first audio processing - audio no go ever comot from your device
- Python, JavaScript, and C# tracks with `client.audio.transcriptions.create()` (Python/JS) and `AudioClient.TranscribeAudioAsync()` (C#)
- Get Zava-themed sample audio files wey you fit use practice hand for hand

**Code samples:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper voice transcription |
| C# | `csharp/WhisperTranscription.cs` | Whisper voice transcription |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper voice transcription |

> **Note:** Dis lab dey use the **Foundry Local SDK** to programmatically download and load the Whisper model, den e dey send audio to the local OpenAI-compatible endpoint for transcription. The Whisper model (`whisper`) dey listed for the Foundry Local catalog and e dey run full on-device - no cloud API keys or network access need.

---

### Part 10: Using Custom or Hugging Face Models

**Lab guide:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compiling Hugging Face models to optimized ONNX format using the ONNX Runtime GenAI model builder
- Hardware-specific compilation (CPU, NVIDIA GPU, DirectML, WebGPU) and quantisation (int4, fp16, bf16)
- Creating chat-template configuration files for Foundry Local
- Adding compiled models to the Foundry Local cache
- Running custom models via the CLI, REST API, and OpenAI SDK
- Example wey fit be reference: compiling Qwen/Qwen3-0.6B end-to-end

---

### Part 11: Tool Calling with Local Models

**Lab guide:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Enable local models to call external functions (tool/function calling)
- Define tool schemas using the OpenAI function-calling format
- Handle the multi-turn tool-calling conversation flow
- Execute tool calls locally and return results to the model
- Choose the correct model for tool-calling scenarios (Qwen 2.5, Phi-4-mini)
- Use the SDK's native `ChatClient` for tool calling (JavaScript)

**Code samples:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tool calling with weather/population tools |
| C# | `csharp/ToolCalling.cs` | Tool calling with .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool calling with ChatClient |

---

### Part 12: Building a Web UI for the Zava Creative Writer

**Lab guide:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Add browser-based front end for the Zava Creative Writer
- Serve the shared UI from Python (FastAPI), JavaScript (Node.js HTTP), and C# (ASP.NET Core)
- Consume streaming NDJSON in the browser with the Fetch API and ReadableStream
- Live agent status badges and real-time article text streaming

**Code (shared UI):**

| File | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Page layout |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Stream reader and DOM update logic |

**Backend additions:**

| Language | File | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Updated to serve static UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | New HTTP server wrapping the orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | New ASP.NET Core minimal API project |

---

### Part 13: Workshop Complete

**Lab guide:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Summary of everything wey you don build for all 12 parts
- More ideas to extend your applications
- Links to resources and documentation

---

## Project Structure

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

## Resources

| Resource | Link |
|----------|------|
| Foundry Local website | [foundrylocal.ai](https://foundrylocal.ai) |
| Model catalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Getting started guide | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Reference | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licence

Dis workshop material na for educational purposes.

---

**Happy building! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:  
Dis document don translate wit AI translation service [Co-op Translator](https://github.com/Azure/co-op-translator). Even though we dey try make am correct, abeg sabi say automated translations fit get mistake or no too correct. Di original document for dia own language na im be di correct source. For important information, e good make person wey sabi human translation do am. We no go take any blame for any misunderstanding or wrong meaning wey fit show because of dis translation.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->