<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Bumuo ng AI Apps On-Device

Isang hands-on na workshop para sa pagpapatakbo ng mga language model sa iyong sariling makina at pagbubuo ng mga intelihenteng aplikasyon gamit ang [Foundry Local](https://foundrylocal.ai) at ang [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Ano ang Foundry Local?** Ang Foundry Local ay isang magaan na runtime na nagpapahintulot sa iyo na mag-download, mag-manage, at mag-serve ng mga language model nang buo sa iyong hardware. Nagbibigay ito ng isang **OpenAI-compatible API** kaya anumang tool o SDK na gumagamit ng OpenAI ay maaaring kumonekta - hindi kailangan ng cloud account.

### 🌐 Suporta sa Maramihang Wika

#### Sinusuportahan sa pamamagitan ng GitHub Action (Awtomatiko at Palaging Napapanahon)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](./README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Mas Gustong Mag-Clone Nang Lokal?**
>
> Ang repository na ito ay may kasamang 50+ na pagsasalin ng wika na malaki ang ipinapataas sa laki ng pag-download. Para mag-clone nang walang mga pagsasalin, gamitin ang sparse checkout:
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
> Bibigyan ka nito ng lahat ng kailangan mo para makumpleto ang kurso na may mas mabilis na pag-download.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Mga Layunin sa Pagkatuto

Sa pagtatapos ng workshop na ito ay magiging kaya mo:

| # | Layunin |
|---|---------|
| 1 | I-install ang Foundry Local at pamahalaan ang mga modelo gamit ang CLI |
| 2 | Maging bihasa sa Foundry Local SDK API para sa programmatic na pag-manage ng modelo |
| 3 | Kumonekta sa lokal na inference server gamit ang Python, JavaScript, at C# SDKs |
| 4 | Bumuo ng Retrieval-Augmented Generation (RAG) pipeline na naglalagay ng mga sagot base sa iyong sariling data |
| 5 | Gumawa ng AI agents na may permanenteng mga tagubilin at mga persona |
| 6 | I-orchestrate ang multi-agent workflows na may feedback loops |
| 7 | Tuklasin ang isang production capstone app - ang Zava Creative Writer |
| 8 | Bumuo ng evaluation frameworks gamit ang golden datasets at LLM-as-judge scoring |
| 9 | Mag-transcribe ng audio gamit ang Whisper - speech-to-text on-device gamit ang Foundry Local SDK |
| 10 | Mag-compile at magpatakbo ng custom o Hugging Face models gamit ang ONNX Runtime GenAI at Foundry Local |
| 11 | Paganahin ang local models na tumawag ng external functions gamit ang tool-calling pattern |
| 12 | Gumawa ng browser-based UI para sa Zava Creative Writer na may real-time streaming |

---

## Mga Kinakailangan

| Kinakailangan | Detalye |
|---------------|---------|
| **Hardware** | Minimum 8 GB RAM (16 GB inirerekomenda); CPU na AVX2-capable o suportadong GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, o macOS 13+ |
| **Foundry Local CLI** | I-install gamit ang `winget install Microsoft.FoundryLocal` (Windows) o `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Tingnan ang [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) para sa mga detalye. |
| **Language runtime** | **Python 3.9+** at/o **.NET 9.0+** at/o **Node.js 18+** |
| **Git** | Para sa pag-clone ng repository na ito |

---

## Pagsisimula

```bash
# 1. Kopyahin ang repositoryo
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Siguraduhing naka-install ang Foundry Local
foundry model list              # Ilista ang mga magagamit na modelo
foundry model run phi-3.5-mini  # Magsimula ng isang interaktibong chat

# 3. Piliin ang iyong track ng wika (tingnan ang Part 2 lab para sa buong setup)
```

| Wika | Mabilis na Simula |
|------|-------------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Mga Bahagi ng Workshop

### Bahagi 1: Pagsisimula sa Foundry Local

**Lab guide:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Ano ang Foundry Local at paano ito gumagana
- Pag-install ng CLI sa Windows at macOS
- Pagsaliksik sa mga modelo - paglilista, pag-download, pagpapatakbo
- Pag-unawa sa mga alias ng modelo at dynamic ports

---

### Bahagi 2: Mas Malalim sa Foundry Local SDK

**Lab guide:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Bakit gamitin ang SDK kaysa CLI para sa pag-develop ng aplikasyon
- Buong SDK API reference para sa Python, JavaScript, at C#
- Pamamahala ng serbisyo, pag-browse ng katalogo, daloy ng buhay ng modelo (download, load, unload)
- Mga quick-start na pattern: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadata, mga alias, at pagpili ng hardware-optimal na modelo

---

### Bahagi 3: SDKs at APIs

**Lab guide:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Pagkonekta sa Foundry Local mula sa Python, JavaScript, at C#
- Paggamit ng Foundry Local SDK para programmatic na pamamahala ng serbisyo
- Streaming chat completions gamit ang OpenAI-compatible API
- SDK method reference para sa bawat wika

**Mga halimbawa ng code:**

| Wika | File | Deskripsyon |
|------|------|-------------|
| Python | `python/foundry-local.py` | Basic streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat gamit ang .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat gamit ang Node.js |

---

### Bahagi 4: Retrieval-Augmented Generation (RAG)

**Lab guide:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Ano ang RAG at bakit ito mahalaga
- Pagtatayo ng in-memory knowledge base
- Keyword-overlap retrieval gamit ang scoring
- Pagsusulat ng grounded system prompts
- Pagpapatakbo ng buong RAG pipeline on-device

**Mga halimbawa ng code:**

| Wika | File |
|------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Bahagi 5: Pagbuo ng AI Agents

**Lab guide:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Ano ang AI agent (kumpara sa simpleng LLM call)
- Ang `ChatAgent` pattern at ang Microsoft Agent Framework
- Mga system instructions, persona, at multi-turn na usapan
- Structured output (JSON) mula sa mga agent

**Mga halimbawa ng code:**

| Wika | File | Deskripsyon |
|------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Single agent gamit ang Agent Framework |
| C# | `csharp/SingleAgent.cs` | Single agent (ChatAgent pattern) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Single agent (ChatAgent pattern) |

---

### Bahagi 6: Multi-Agent Workflows

**Lab guide:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Mga multi-agent pipeline: Researcher → Writer → Editor
- Sunud-sunod na orchestration at feedback loops
- Shared configuration at structured hand-offs
- Pagdidisenyo ng sarili mong multi-agent workflow

**Mga halimbawa ng code:**

| Wika | File | Deskripsyon |
|------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Three-agent pipeline |
| C# | `csharp/MultiAgent.cs` | Three-agent pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Three-agent pipeline |

---

### Bahagi 7: Zava Creative Writer - Capstone Application

**Lab guide:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Isang production-style na multi-agent app na may 4 na espesyal na agent
- Sunud-sunod na pipeline na may evaluator-driven na feedback loops
- Streaming output, paghahanap sa product catalog, structured JSON hand-offs
- Buong implementasyon sa Python (FastAPI), JavaScript (Node.js CLI), at C# (.NET console)

**Mga halimbawa ng code:**

| Wika | Directory | Deskripsyon |
|------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI web service na may orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI application |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 console application |

---

### Bahagi 8: Evaluation-Led Development

**Lab guide:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Bumuo ng sistematikong evaluation framework para sa AI agents gamit ang golden datasets
- Mga rule-based na pagsusuri (haba, keyword coverage, ipinagbabawal na mga termino) + LLM-as-judge scoring
- Paghahambing ng prompt variants na magkatabi gamit ang aggregate scorecards
- Pinalalawak ang Zava Editor agent pattern mula sa Bahagi 7 sa isang offline na test suite
- Mga track para sa Python, JavaScript, at C#

**Mga halimbawa ng code:**

| Wika | File | Deskripsyon |
|------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evaluation framework |
| C# | `csharp/AgentEvaluation.cs` | Evaluation framework |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evaluation framework |

---

### Bahagi 9: Voice Transcription with Whisper

**Lab guide:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Pagsasalin ng pananalita-sa-teksto gamit ang OpenAI Whisper na tumatakbo nang lokal
- Privacy-first na pagproseso ng audio - hindi umaalis ang audio sa iyong aparato
- Mga track para sa Python, JavaScript, at C# gamit ang `client.audio.transcriptions.create()` (Python/JS) at `AudioClient.TranscribeAudioAsync()` (C#)
- Kasama ang mga audio sample file na may temang Zava para sa praktikal na pagsasanay

**Mga halimbawa ng code:**

| Wika | File | Paglalarawan |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Pagsasalin ng boses gamit ang Whisper |
| C# | `csharp/WhisperTranscription.cs` | Pagsasalin ng boses gamit ang Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Pagsasalin ng boses gamit ang Whisper |

> **Note:** Ginagamit ng lab na ito ang **Foundry Local SDK** upang programatikong i-download at i-load ang Whisper model, pagkatapos ay magpadala ng audio sa lokal na OpenAI-compatible endpoint para sa pagsasalin. Ang Whisper model (`whisper`) ay nakalista sa Foundry Local catalog at tumatakbo nang buo sa aparato - walang kinakailangang cloud API keys o access sa network.

---

### Bahagi 10: Paggamit ng Custom o Hugging Face Models

**Gabayan sa lab:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Pag-compile ng Hugging Face models sa optimised ONNX format gamit ang ONNX Runtime GenAI model builder
- Hardware-specific na pag-compile (CPU, NVIDIA GPU, DirectML, WebGPU) at quantisation (int4, fp16, bf16)
- Paglikha ng mga chat-template configuration file para sa Foundry Local
- Pagdaragdag ng mga na-compile na modelo sa Foundry Local cache
- Pagpapatakbo ng mga custom na modelo gamit ang CLI, REST API, at OpenAI SDK
- Halimbawa ng reference: end-to-end na pag-compile ng Qwen/Qwen3-0.6B

---

### Bahagi 11: Tool Calling gamit ang Lokal na Mga Modelo

**Gabayan sa lab:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Paganahin ang lokal na mga modelo na tumawag ng mga panlabas na function (tool/function calling)
- Tukuyin ang mga schema ng tool gamit ang OpenAI function-calling format
- Pamahalaan ang multi-turn tool-calling na daloy ng pag-uusap
- Ipatupad ang mga tool call nang lokal at ibalik ang mga resulta sa modelo
- Piliin ang tamang modelo para sa mga senaryo ng tool-calling (Qwen 2.5, Phi-4-mini)
- Gamitin ang native na `ChatClient` ng SDK para sa tool calling (JavaScript)

**Mga halimbawa ng code:**

| Wika | File | Paglalarawan |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Tool calling gamit ang weather/population tools |
| C# | `csharp/ToolCalling.cs` | Tool calling gamit ang .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Tool calling gamit ang ChatClient |

---

### Bahagi 12: Paggawa ng Web UI para sa Zava Creative Writer

**Gabayan sa lab:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Magdagdag ng browser-based na front end sa Zava Creative Writer
- I-serve ang shared UI mula sa Python (FastAPI), JavaScript (Node.js HTTP), at C# (ASP.NET Core)
- Gumamit ng streaming NDJSON sa browser gamit ang Fetch API at ReadableStream
- Live agent status badges at real-time na streaming ng teksto ng artikulo

**Code (shared UI):**

| File | Paglalarawan |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Layout ng pahina |
| `zava-creative-writer-local/ui/style.css` | Styling |
| `zava-creative-writer-local/ui/app.js` | Logic ng stream reader at pag-update ng DOM |

**Mga karagdagan sa Backend:**

| Wika | File | Paglalarawan |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Na-update upang mag-serve ng static UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Bagong HTTP server na nakapaloob sa orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Bagong ASP.NET Core minimal API project |

---

### Bahagi 13: Kumpletong Workshop

**Gabayan sa lab:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Buod ng lahat ng iyong ginawa sa lahat ng 12 bahagi
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
| Foundry Local website | [foundrylocal.ai](https://foundrylocal.ai) |
| Model catalog | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Gabay sa pagsisimula | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Reference | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Lisensya

Ang materyal na ito sa workshop ay ibinibigay para sa layuning pang-edukasyon.

---

**Maligayang pagbuo! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Pagtatanggal ng Pananagutan**:  
Ang dokumentong ito ay isinalin gamit ang AI translation service na [Co-op Translator](https://github.com/Azure/co-op-translator). Habang aming nilalayon ang katumpakan, mangyaring tandaan na ang mga awtomatikong pagsasalin ay maaaring maglaman ng mga pagkakamali o hindi pagkakatugma. Ang orihinal na dokumento sa orihinal nitong wika ang dapat ituring na pangunahing sanggunian. Para sa mga mahahalagang impormasyon, inirerekomenda ang propesyonal na pagsasalin ng tao. Hindi kami mananagot sa anumang hindi pagkakaunawaan o maling interpretasyon na maaaring magmula sa paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->