<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop — Build AI Apps On-Device

A hands-on workshop for running language models on your own machine and building intelligent applications with [Foundry Local](https://foundrylocal.ai) and the [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **What is Foundry Local?** Foundry Local is a lightweight runtime that lets you download, manage, and serve language models entirely on your hardware. It exposes an **OpenAI-compatible API** so any tool or SDK that speaks OpenAI can connect — no cloud account required.

---

## Learning Objectives

By the end of this workshop you will be able to:

| # | Objective |
|---|-----------|
| 1 | Install Foundry Local and manage models with the CLI |
| 2 | Connect to the local inference server using the Python, JavaScript, and C# SDKs |
| 3 | Build a Retrieval-Augmented Generation (RAG) pipeline that grounds answers in your own data |
| 4 | Create AI agents with persistent instructions and personas |
| 5 | Orchestrate multi-agent workflows with feedback loops |
| 6 | Explore a production capstone app — the Contoso Creative Writer |

---

## Prerequisites

| Requirement | Details |
|-------------|---------|
| **Hardware** | 8 GB RAM minimum (16 GB recommended); AVX2-capable CPU or a supported GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, or macOS 13+ |
| **Foundry Local CLI** | Install via `winget install Microsoft.FoundryLocal` (Windows) or `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). See the [getting started guide](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-local/get-started) for details. |
| **Language runtime** | **Python 3.9+** and/or **.NET 9.0+** and/or **Node.js 18+** |
| **Git** | For cloning this repository |

---

## Getting Started

```bash
# 1. Clone the repository
git clone https://github.com/microsoft/foundry-local-workshop.git
cd foundry-local-workshop

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
- Exploring models — listing, downloading, running
- Understanding model aliases and dynamic ports

---

### Part 2: SDKs and APIs

**Lab guide:** [`labs/part2-sdk-and-apis.md`](labs/part2-sdk-and-apis.md)

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

### Part 3: Retrieval-Augmented Generation (RAG)

**Lab guide:** [`labs/part3-rag-fundamentals.md`](labs/part3-rag-fundamentals.md)

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

### Part 4: Building AI Agents

**Lab guide:** [`labs/part4-single-agents.md`](labs/part4-single-agents.md)

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

### Part 5: Multi-Agent Workflows

**Lab guide:** [`labs/part5-multi-agent-workflows.md`](labs/part5-multi-agent-workflows.md)

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

### Part 6: Contoso Creative Writer — Capstone Application

**Lab guide:** [`labs/part6-contoso-creative-writer.md`](labs/part6-contoso-creative-writer.md)

- A production-style multi-agent app with 4 specialized agents
- Sequential pipeline with evaluator-driven feedback loops
- Streaming output, product catalog search, structured JSON hand-offs
- Full implementation in Python (FastAPI), JavaScript (Node.js CLI), and C# (.NET console)

**Code samples:**

| Language | Directory | Description |
|----------|-----------|-------------|
| Python | `contoso-creative-writer-local/src/api/` | FastAPI web service with orchestrator |
| JavaScript | `contoso-creative-writer-local/src/javascript/` | Node.js CLI application |
| C# | `contoso-creative-writer-local/src/csharp/` | .NET 9 console application |

---

## Project Structure

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   └── package.json
├── contoso-creative-writer-local/ # Production multi-agent app
│   └── src/api/
│       ├── main.py                # FastAPI server
│       ├── orchestrator.py        # Pipeline coordinator
│       ├── foundry_config.py      # Shared Foundry Local config
│       └── agents/                # Researcher, Product, Writer, Editor
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-sdk-and-apis.md
│   ├── part3-rag-fundamentals.md
│   ├── part4-single-agents.md
│   ├── part5-multi-agent-workflows.md
│   └── part6-contoso-creative-writer.md
└── README.md
```

---

## Resources

| Resource | Link |
|----------|------|
| Foundry Local website | [foundrylocal.ai](https://foundrylocal.ai) |
| Model catalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Getting started guide | [Microsoft Learn — Foundry Local](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-local/get-started) |
| Microsoft Agent Framework | [Microsoft Learn — Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |

---

## License

This workshop material is provided for educational purposes.

---

**Happy building! 🚀**
