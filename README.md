<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Build AI Apps On-Device

A hands-on workshop for running language models on your own machine and building intelligent applications with [Foundry Local](https://foundrylocal.ai) and the [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **What is Foundry Local?** Foundry Local is a lightweight runtime that lets you download, manage, and serve language models entirely on your hardware. It exposes an **OpenAI-compatible API** so any tool or SDK that speaks OpenAI can connect - no cloud account required.

---

## Learning Objectives

By the end of this workshop you will be able to:

| # | Objective |
|---|-----------|
| 1 | Install Foundry Local and manage models with the CLI |
| 2 | Master the Foundry Local SDK API for programmatic model management |
| 3 | Connect to the local inference server using the Python, JavaScript, and C# SDKs |
| 4 | Build a Retrieval-Augmented Generation (RAG) pipeline that grounds answers in your own data |
| 5 | Create AI agents with persistent instructions and personas |
| 6 | Orchestrate multi-agent workflows with feedback loops |
| 7 | Explore a production capstone app - the Zava Creative Writer |
| 8 | Build evaluation frameworks with golden datasets and LLM-as-judge scoring |
| 9 | Transcribe audio with Whisper - speech-to-text on-device using the Foundry Local SDK |
| 10 | Compile and run custom or Hugging Face models with ONNX Runtime GenAI and Foundry Local |
| 11 | Enable local models to call external functions with the tool-calling pattern |

---

## Prerequisites

| Requirement | Details |
|-------------|---------|
| **Hardware** | 8 GB RAM minimum (16 GB recommended); AVX2-capable CPU or a supported GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, or macOS 13+ |
| **Foundry Local CLI** | Install via `winget install Microsoft.FoundryLocal` (Windows) or `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). See the [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) for details. |
| **Language runtime** | **Python 3.9+** and/or **.NET 9.0+** and/or **Node.js 18+** |
| **Git** | For cloning this repository |

---

## Getting Started

```bash
# 1. Clone the repository
git clone https://github.com/microsoft/foundry-local-workshop.git
cd foundry-local-lab

# 2. Verify Foundry Local is installed
foundry model list              # List available models
foundry model run phi-3.5-mini  # Start an interactive chat

# 3. Choose your language track (see Part 2 lab for full setup)
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

- What is Foundry Local and how it works
- Installing the CLI on Windows and macOS
- Exploring models - listing, downloading, running
- Understanding model aliases and dynamic ports

---

### Part 2: Foundry Local SDK Deep Dive

**Lab guide:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Why use the SDK over the CLI for application development
- Full SDK API reference for Python, JavaScript, and C#
- Service management, catalog browsing, model lifecycle (download, load, unload)
- Quick-start patterns: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, aliases, and hardware-optimal model selection

---

### Part 3: SDKs and APIs

**Lab guide:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Connecting to Foundry Local from Python, JavaScript, and C#
- Using the Foundry Local SDK to manage the service programmatically
- Streaming chat completions via the OpenAI-compatible API
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

- What is RAG and why it matters
- Building an in-memory knowledge base
- Keyword-overlap retrieval with scoring
- Composing grounded system prompts
- Running a complete RAG pipeline on-device

**Code samples:**

| Language | File |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Part 5: Building AI Agents

**Lab guide:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- What is an AI agent (vs. a raw LLM call)
- The `ChatAgent` pattern and the Microsoft Agent Framework
- System instructions, personas, and multi-turn conversations
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
- Designing your own multi-agent workflow

**Code samples:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Three-agent pipeline |
| C# | `csharp/MultiAgent.cs` | Three-agent pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Three-agent pipeline |

---

### Part 7: Zava Creative Writer - Capstone Application

**Lab guide:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- A production-style multi-agent app with 4 specialised agents
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

- Build a systematic evaluation framework for AI agents using golden datasets
- Rule-based checks (length, keyword coverage, forbidden terms) + LLM-as-judge scoring
- Side-by-side comparison of prompt variants with aggregate scorecards
- Extends the Zava Editor agent pattern from Part 7 into an offline test suite
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

- Speech-to-text transcription using OpenAI Whisper running locally
- Privacy-first audio processing - audio never leaves your device
- Python, JavaScript, and C# tracks with `client.audio.transcriptions.create()` (Python/JS) and `AudioClient.TranscribeAudioAsync()` (C#)
- Includes Zava-themed sample audio files for hands-on practice

**Code samples:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper voice transcription |
| C# | `csharp/WhisperTranscription.cs` | Whisper voice transcription |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper voice transcription |

> **Note:** This lab uses the **Foundry Local SDK** to programmatically download and load the Whisper model, then sends audio to the local OpenAI-compatible endpoint for transcription. The Whisper model (`whisper`) is listed in the Foundry Local catalog and runs entirely on-device - no cloud API keys or network access required.

---

### Part 10: Using Custom or Hugging Face Models

**Lab guide:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Compiling Hugging Face models to optimised ONNX format using the ONNX Runtime GenAI model builder
- Hardware-specific compilation (CPU, NVIDIA GPU, DirectML, WebGPU) and quantisation (int4, fp16, bf16)
- Creating chat-template configuration files for Foundry Local
- Adding compiled models to the Foundry Local cache
- Running custom models via the CLI, REST API, and OpenAI SDK
- Reference example: compiling Qwen/Qwen3-0.6B end-to-end

---

### Part 11: Tool Calling with Local Models

**Lab guide:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Enable local models to call external functions (tool/function calling)
- Define tool schemas using the OpenAI function-calling format
- Handle the multi-turn tool-calling conversation flow
- Execute tool calls locally and return results to the model
- Choose the right model for tool-calling scenarios (Qwen 2.5, Phi-4-mini)
- Use the SDK's native `ChatClient` for tool calling (JavaScript)

**Code samples:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tool calling with weather/population tools |
| C# | `csharp/ToolCalling.cs` | Tool calling with .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool calling with ChatClient |

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
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI app
│       │   ├── main.mjs
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       └── csharp/                # .NET 9 console app
│           ├── Program.cs
│           └── ZavaCreativeWriter.csproj
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
│   └── part10-custom-models.md
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

This workshop material is provided for educational purposes.

---

**Happy building! 🚀**
