<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Gumawa ng Mga AI Apps On-Device

Isang praktikal na workshop para sa pagpapatakbo ng mga language model sa iyong sariling makina at pagbuo ng mga intelihenteng aplikasyon gamit ang [Foundry Local](https://foundrylocal.ai) at ang [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Ano ang Foundry Local?** Ang Foundry Local ay isang magaan na runtime na nagpapahintulot sa iyo na mag-download, mag-manage, at mag-serve ng mga language model nang buong-buo sa iyong hardware. Nagbibigay ito ng **OpenAI-compatible API** kaya anumang tool o SDK na gumagamit ng OpenAI ay maaaring kumonekta - hindi na kailangan ng cloud account.

---

## Mga Layunin sa Pagkatuto

Sa pagtatapos ng workshop na ito ay magagawa mo:

| # | Layunin |
|---|---------|
| 1 | I-install ang Foundry Local at mag-manage ng mga modelo gamit ang CLI |
| 2 | Maging bihasa sa Foundry Local SDK API para sa programmatic na pamamahala ng modelo |
| 3 | Kumonekta sa lokal na inference server gamit ang Python, JavaScript, at C# SDKs |
| 4 | Gumawa ng Retrieval-Augmented Generation (RAG) pipeline na naka-base sa iyong sariling data |
| 5 | Gumawa ng AI agents na may persistent instructions at mga persona |
| 6 | Mag-orchestrate ng multi-agent workflows na may feedback loops |
| 7 | Siyasatin ang production capstone app - ang Zava Creative Writer |
| 8 | Gumawa ng evaluation frameworks gamit ang golden datasets at LLM-as-judge scoring |
| 9 | Mag-transcribe ng audio gamit ang Whisper - speech-to-text on-device gamit ang Foundry Local SDK |
| 10 | I-compile at patakbuhin ang custom o Hugging Face models gamit ang ONNX Runtime GenAI at Foundry Local |
| 11 | Payagan ang lokal na mga modelo na tumawag ng external na mga function gamit ang tool-calling pattern |
| 12 | Gumawa ng browser-based UI para sa Zava Creative Writer na may real-time streaming |

---

## Mga Kinakailangan

| Kinakailangan | Detalye |
|---------------|---------|
| **Hardware** | Minimum na 8 GB RAM (16 GB inirerekomenda); AVX2-capable CPU o suportadong GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, o macOS 13+ |
| **Foundry Local CLI** | I-install gamit ang `winget install Microsoft.FoundryLocal` (Windows) o `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Tingnan ang [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) para sa detalye. |
| **Language runtime** | **Python 3.9+** at/o **.NET 9.0+** at/o **Node.js 18+** |
| **Git** | Para sa pag-clone ng repository |

---

## Pagsisimula

```bash
# 1. Kopyahin ang repositoryo
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Tiyakin na naka-install ang Foundry Local
foundry model list              # Ilan sa mga magagamit na modelo
foundry model run phi-3.5-mini  # Simulan ang isang interaktibong chat

# 3. Piliin ang iyong track ng wika (tingnan ang Part 2 lab para sa buong setup)
```

| Wika | Mabilis na Pagsisimula |
|-------|---------------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Mga Bahagi ng Workshop

### Bahagi 1: Pagsisimula sa Foundry Local

**Lab guide:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Ano ang Foundry Local at paano ito gumagana
- Pag-install ng CLI sa Windows at macOS
- Pag-explore ng mga modelo - paglista, pag-download, pagpapatakbo
- Pag-unawa sa mga alias ng modelo at dynamic ports

---

### Bahagi 2: Malalimang Pag-aaral ng Foundry Local SDK

**Lab guide:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Bakit gamitin ang SDK kaysa sa CLI para sa pag-develop ng aplikasyon
- Buong SDK API reference para sa Python, JavaScript, at C#
- Pamamahala ng serbisyo, pag-browse ng katalogo, lifecycle ng modelo (download, load, unload)
- Mga quick-start pattern: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- Metadata ng `FoundryModelInfo`, mga alias, at pagpili ng modelo na optimal sa hardware

---

### Bahagi 3: Mga SDK at API

**Lab guide:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Pagkonekta sa Foundry Local mula sa Python, JavaScript, at C#
- Paggamit ng Foundry Local SDK para sa programmatic na pamamahala ng serbisyo
- Streaming chat completions gamit ang OpenAI-compatible API
- Reference ng SDK method para sa bawat wika

**Mga sample ng code:**

| Wika | File | Deskripsyon |
|-------|------|-------------|
| Python | `python/foundry-local.py` | Basic streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat gamit ang .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat gamit ang Node.js |

---

### Bahagi 4: Retrieval-Augmented Generation (RAG)

**Lab guide:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Ano ang RAG at bakit ito mahalaga
- Pagbuo ng in-memory knowledge base
- Keyword-overlap retrieval na may scoring
- Pagsulat ng grounded system prompts
- Pagpapatakbo ng kumpletong RAG pipeline on-device

**Mga sample ng code:**

| Wika | File |
|-------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Bahagi 5: Pagbuo ng AI Agents

**Lab guide:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Ano ang AI agent (kumpara sa raw LLM call)
- Ang `ChatAgent` pattern at ang Microsoft Agent Framework
- Mga system instructions, mga persona, at multi-turn conversations
- Structured output (JSON) mula sa mga agent

**Mga sample ng code:**

| Wika | File | Deskripsyon |
|-------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Isang agent gamit ang Agent Framework |
| C# | `csharp/SingleAgent.cs` | Isang agent (ChatAgent pattern) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Isang agent (ChatAgent pattern) |

---

### Bahagi 6: Multi-Agent Workflows

**Lab guide:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Mga multi-agent pipeline: Researcher → Writer → Editor
- Sunud-sunod na orchestration at feedback loops
- Pinagsamang configuration at structured hand-offs
- Pagdisenyo ng sariling multi-agent workflow

**Mga sample ng code:**

| Wika | File | Deskripsyon |
|-------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Tatlong-agent pipeline |
| C# | `csharp/MultiAgent.cs` | Tatlong-agent pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Tatlong-agent pipeline |

---

### Bahagi 7: Zava Creative Writer - Capstone Application

**Lab guide:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Isang production-style multi-agent app na may 4 na specialized agents
- Sunud-sunod na pipeline na may evaluator-driven feedback loops
- Streaming output, paghahanap sa product catalog, structured JSON hand-offs
- Kumpletong implementasyon sa Python (FastAPI), JavaScript (Node.js CLI), at C# (.NET console)

**Mga sample ng code:**

| Wika | Direktoryo | Deskripsyon |
|-------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI web service with orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI application |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 console application |

---

### Bahagi 8: Evaluation-Led Development

**Lab guide:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Gumawa ng sistematikong evaluation framework para sa AI agents gamit ang golden datasets
- Mga rule-based na tsek (haba, keyword coverage, bawal na termino) + LLM-as-judge scoring
- Paghahambing ng prompt variants na may aggregate scorecards
- Pinapalawak ang Zava Editor agent pattern mula sa Part 7 sa offline test suite
- Mga track para sa Python, JavaScript, at C#

**Mga sample ng code:**

| Wika | File | Deskripsyon |
|-------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evaluation framework |
| C# | `csharp/AgentEvaluation.cs` | Evaluation framework |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evaluation framework |

---

### Bahagi 9: Voice Transcription gamit ang Whisper

**Lab guide:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Speech-to-text transcription gamit ang OpenAI Whisper na tumatakbo lokal
- Privacy-first na audio processing - hindi lumalabas sa device ang audio
- Mga track para sa Python, JavaScript, at C# gamit ang `client.audio.transcriptions.create()` (Python/JS) at `AudioClient.TranscribeAudioAsync()` (C#)
- Kasama ang Zava-themed sample audio files para sa praktikal na pagsasanay

**Mga sample ng code:**

| Wika | File | Deskripsyon |
|-------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper voice transcription |
| C# | `csharp/WhisperTranscription.cs` | Whisper voice transcription |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper voice transcription |

> **Paalala:** Ginagamit ng lab na ito ang **Foundry Local SDK** upang programmatically i-download at i-load ang Whisper model, at pagkatapos ay nagpapadala ng audio sa lokal na OpenAI-compatible endpoint para sa transcription. Ang Whisper model (`whisper`) ay nakalista sa Foundry Local catalog at tumatakbo nang buong-buo sa device - hindi kailangan ng cloud API keys o network access.

---

### Bahagi 10: Paggamit ng Custom o Hugging Face Models

**Lab guide:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Pag-compile ng Hugging Face models sa optimized ONNX format gamit ang ONNX Runtime GenAI model builder
- Hardware-specific na compilation (CPU, NVIDIA GPU, DirectML, WebGPU) at quantisation (int4, fp16, bf16)
- Paglikha ng chat-template configuration files para sa Foundry Local
- Pagdaragdag ng compiled models sa Foundry Local cache
- Pagpapatakbo ng custom models gamit ang CLI, REST API, at OpenAI SDK
- Halimbawa ng reference: pag-compile ng Qwen/Qwen3-0.6B end-to-end

---

### Bahagi 11: Tool Calling gamit ang Local Models

**Lab guide:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Paganahin ang lokal na mga modelo na tumawag ng external functions (tool/function calling)
- Mag-define ng tool schemas gamit ang OpenAI function-calling format
- Pamahalaan ang multi-turn na daloy ng tool-calling conversation
- Isagawa ang tool calls lokal at ibalik ang resulta sa modelo
- Piliin ang tamang modelo para sa tool-calling scenarios (Qwen 2.5, Phi-4-mini)
- Gamitin ang native `ChatClient` ng SDK para sa tool calling (JavaScript)

**Mga sample ng code:**

| Wika | File | Deskripsyon |
|-------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tool calling gamit ang weather/population tools |
| C# | `csharp/ToolCalling.cs` | Tool calling gamit ang .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool calling gamit ang ChatClient |

---

### Bahagi 12: Pagbuo ng Web UI para sa Zava Creative Writer

**Lab guide:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Magdagdag ng browser-based front end sa Zava Creative Writer
- I-serve ang shared UI mula sa Python (FastAPI), JavaScript (Node.js HTTP), at C# (ASP.NET Core)
- Gumamit ng streaming NDJSON sa browser gamit ang Fetch API at ReadableStream
- Live na mga badge ng status ng agent at real-time na pag-stream ng article text

**Code (shared UI):**

| File | Deskripsyon |
|-------|------------|
| `zava-creative-writer-local/ui/index.html` | Page layout |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Stream reader at DOM update logic |

**Backend na mga dagdag:**

| Wika | File | Deskripsyon |
|-------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Na-update upang mag-serve ng static UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Bagong HTTP server na bumabalot sa orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Bagong ASP.NET Core minimal API project |

---

### Bahagi 13: Kumpleto na ang Workshop
**Gabayan sa Lab:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Buod ng lahat ng iyong naitayo sa lahat ng 12 bahagi
- Karagdagang mga ideya para sa pagpapalawak ng iyong mga aplikasyon
- Mga link sa mga mapagkukunan at dokumentasyon

---

## Istruktura ng Proyekto

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

## Mga Mapagkukunan

| Mapagkukunan | Link |
|----------|------|
| Website ng Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Katalogo ng Modelo | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Gabay sa Pagsisimula | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Reference | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Lisensya

Ang materyal na ito para sa workshop ay ibinibigay para sa mga layuning pang-edukasyon.

---

**Maligayang pagbuo! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Paunawa**:  
Ang dokumentong ito ay isinalin gamit ang AI translation service na [Co-op Translator](https://github.com/Azure/co-op-translator). Bagaman nagsusumikap kaming maging tumpak, pakatandaan na maaaring may mga kamalian o hindi eksaktong pagsasalin ang awtomatikong mga pagsasalin. Ang orihinal na dokumento sa kanyang sariling wika ang dapat ituring na awtoritatibong sanggunian. Para sa mahahalagang impormasyon, inirerekomenda ang propesyonal na pagsasalin ng tao. Hindi kami mananagot sa anumang maling pagkaunawa o maling interpretasyon na nagmumula sa paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->