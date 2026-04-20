<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Build AI Apps On-Device

Na hands-on workshop wey go show how you fit run language models for your own machine and build smart apps with [Foundry Local](https://foundrylocal.ai) and the [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Wetín be Foundry Local?** Foundry Local na light-weight runtime wey make you fit download, manage, and serve language models complete for your hardware. E get **OpenAI-compatible API** so any tool or SDK wey sabi OpenAI fit connect - no need cloud account.

---

## Wetin You Go Learn

By di time you finish dis workshop, you go fit:

| # | Wetin You Go Fit Do |
|---|---------------------|
| 1 | Install Foundry Local and manage models with di CLI |
| 2 | Master di Foundry Local SDK API for programmatic model management |
| 3 | Connect to di local inference server with Python, JavaScript, and C# SDKs |
| 4 | Build Retrieval-Augmented Generation (RAG) pipeline wey dey base answer for your own data |
| 5 | Create AI agents wey get persistent instructions and personas |
| 6 | Orchestrate multi-agent workflows with feedback loops |
| 7 | Explore production level capstone app - Zava Creative Writer |
| 8 | Build evaluation frameworks with golden datasets and LLM-as-judge scoring |
| 9 | Transcribe audio with Whisper - speech-to-text on-device using Foundry Local SDK |
| 10 | Compile and run custom or Hugging Face models with ONNX Runtime GenAI and Foundry Local |
| 11 | Enable local models to call external functions with tool-calling pattern |
| 12 | Build browser-based UI for Zava Creative Writer with real-time streaming |

---

## Wetin You Need Get Ready

| Requirement | Details |
|-------------|---------|
| **Hardware** | 8 GB RAM minimum (16 GB better); AVX2-capable CPU or supported GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, or macOS 13+ |
| **Foundry Local CLI** | Install with `winget install Microsoft.FoundryLocal` (Windows) or `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Check [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) for details. |
| **Language runtime** | **Python 3.9+** and/or **.NET 9.0+** and/or **Node.js 18+** |
| **Git** | To clone this repository |

---

## How To Start

```bash
# 1. Make copy of di repo
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Check say Foundry Local don install
foundry model list              # Show di models wey dey
foundry model run phi-3.5-mini  # Start better chat wey you fit yan wit

# 3. Pick di language way you want (check Part 2 lab for full setup)
```

| Language | How To Start Quick |
|----------|--------------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Workshop Parts

### Part 1: How To Start with Foundry Local

**Lab guide:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Wetin be Foundry Local and how e dey work
- How to install CLI for Windows and macOS
- How to explore models - list, download, run
- Understand model aliases and dynamic ports

---

### Part 2: Deep Dive into Foundry Local SDK

**Lab guide:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Why you go prefer SDK instead CLI for app development
- Full SDK API reference for Python, JavaScript, and C#
- How to manage service, browse catalog, model lifecycle (download, load, unload)
- Quick-start methods: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, aliases, and how to choose hardware-friendly models

---

### Part 3: SDKs and APIs

**Lab guide:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- How to connect Foundry Local from Python, JavaScript, and C#
- Use Foundry Local SDK to manage service programmatically
- Stream chat completions with OpenAI-compatible API
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

- Wetin RAG be and why e important
- Build in-memory knowledge base
- Keyword-overlap retrieval with scoring
- Compose grounded system prompts
- Run complete RAG pipeline on-device

**Code samples:**

| Language | File |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Part 5: Building AI Agents

**Lab guide:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Wetin AI agent be (compare am to raw LLM call)
- The `ChatAgent` pattern and Microsoft Agent Framework
- System instructions, personas, multi-turn conversations
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

- Multi-agent pipelines: Researcher → Writer → Editor
- Sequential orchestration and feedback loops
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
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI application |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 console application |

---

### Part 8: Evaluation-Led Development

**Lab guide:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Build systematic evaluation framework for AI agents with golden datasets
- Rule-based checks (length, keyword coverage, forbidden terms) + LLM-as-judge scoring
- Side-by-side prompt variant comparison with aggregate scorecards
- Extend Zava Editor agent pattern from Part 7 to offline test suite
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

- Speech-to-text transcription with OpenAI Whisper run locally
- Privacy-first audio processing - audio no dey leave your device
- Python, JavaScript, and C# tracks with `client.audio.transcriptions.create()` (Python/JS) and `AudioClient.TranscribeAudioAsync()` (C#)
- Zava-themed sample audio files for hands-on practice

**Code samples:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper voice transcription |
| C# | `csharp/WhisperTranscription.cs` | Whisper voice transcription |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper voice transcription |

> **Note:** Dis lab dey use **Foundry Local SDK** to programmatically download and load Whisper model, den e send audio to local OpenAI-compatible endpoint for transcription. Whisper model (`whisper`) dey inside Foundry Local catalog and e run complete on-device - no need cloud API keys or network access.

---

### Part 10: Using Custom or Hugging Face Models

**Lab guide:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compile Hugging Face models to optimized ONNX format using ONNX Runtime GenAI model builder
- Hardware-specific compilation (CPU, NVIDIA GPU, DirectML, WebGPU) and quantisation (int4, fp16, bf16)
- Create chat-template configuration files for Foundry Local
- Add compiled models to Foundry Local cache
- Run custom models via CLI, REST API, and OpenAI SDK
- Reference example: compile Qwen/Qwen3-0.6B end-to-end

---

### Part 11: Tool Calling with Local Models

**Lab guide:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Enable local models to call external functions (tool/function calling)
- Define tool schemas with OpenAI function-calling format
- Handle multi-turn tool-calling conversation flow
- Execute tool calls locally and return results to model
- Choose correct model for tool-calling (Qwen 2.5, Phi-4-mini)
- Use SDK native `ChatClient` for tool calling (JavaScript)

**Code samples:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tool calling with weather/population tools |
| C# | `csharp/ToolCalling.cs` | Tool calling with .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool calling with ChatClient |

---

### Part 12: Build Web UI for Zava Creative Writer

**Lab guide:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Add browser-based front end to Zava Creative Writer
- Serve shared UI from Python (FastAPI), JavaScript (Node.js HTTP), and C# (ASP.NET Core)
- Consume streaming NDJSON in browser with Fetch API and ReadableStream
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
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | New HTTP server wrapping orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | New ASP.NET Core minimal API project |

---

### Part 13: Workshop Don Finish
**Lab guide:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Summary of everything wey you don build for all di 12 parts
- More ideas to take extend your applications
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