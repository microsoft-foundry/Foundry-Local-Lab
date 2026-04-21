<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Vytvárajte AI aplikácie priamo na zariadení

Praktický workshop pre spúšťanie jazykových modelov na vašom vlastnom zariadení a tvorbu inteligentných aplikácií s [Foundry Local](https://foundrylocal.ai) a [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Čo je Foundry Local?** Foundry Local je ľahké runtime prostredie, ktoré umožňuje sťahovať, spravovať a poskytovať jazykové modely úplne na vašom hardvéri. Ponúka **OpenAI-kompatibilné API**, takže sa k nemu môže pripojiť ktorýkoľvek nástroj alebo SDK, ktorý komunikuje s OpenAI - nie je potrebný žiadny cloudový účet.

---

## Ciele učenia

Na konci tohto workshopu budete schopní:

| # | Cieľ |
|---|-----------|
| 1 | Nainštalovať Foundry Local a spravovať modely cez CLI |
| 2 | Ovládnuť Foundry Local SDK API pre programatickú správu modelov |
| 3 | Pripojiť sa k lokálnemu inference serveru pomocou Python, JavaScript a C# SDK |
| 4 | Vytvoriť Retrieval-Augmented Generation (RAG) pipeline, ktorý zakladá odpovede na vlastných dátach |
| 5 | Vytvoriť AI agentov s perzistentnými inštrukciami a personami |
| 6 | Orchestrovať workflowy s viacerými agentmi s spätnoväzbovými slučkami |
| 7 | Preskúmať produkčnú capstone aplikáciu - Zava Creative Writer |
| 8 | Vytvárať vyhodnocovacie rámce s "golden" datasetmi a hodnotením LLM ako sudca |
| 9 | Prepisovať audio pomocou Whisper - prevod reči na text priamo na zariadení s Foundry Local SDK |
| 10 | Kompilovať a spúšťať vlastné alebo Hugging Face modely pomocou ONNX Runtime GenAI a Foundry Local |
| 11 | Umožniť lokálnym modelom volať externé funkcie pomocou vzoru tool-calling |
| 12 | Vybudovať prehliadačové UI pre Zava Creative Writer s realtime streamovaním |

---

## Požiadavky

| Požiadavka | Detail |
|-------------|---------|
| **Hardvér** | Minimálne 8 GB RAM (odporúča sa 16 GB); CPU s podporou AVX2 alebo podporovaná GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025 alebo macOS 13+ |
| **Foundry Local CLI** | Inštalácia cez `winget install Microsoft.FoundryLocal` (Windows) alebo `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Viac v [návode na začiatok](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Runtime jazyka** | **Python 3.9+** a/alebo **.NET 9.0+** a/alebo **Node.js 18+** |
| **Git** | Pre klonovanie tohto repozitára |

---

## Začína sa

```bash
# 1. Klonujte úložisko
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Overte, či je nainštalovaný Foundry Local
foundry model list              # Zoznam dostupných modelov
foundry model run phi-3.5-mini  # Spustite interaktívny chat

# 3. Vyberte si jazykovú stopu (pozri časť 2 laboratórium pre úplné nastavenie)
```

| Jazyk | Rýchly štart |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Časti workshopu

### Časť 1: Začíname s Foundry Local

**Laboratórny návod:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Čo je Foundry Local a ako funguje
- Inštalácia CLI na Windows a macOS
- Preskúmanie modelov - zoznam, sťahovanie, spúšťanie
- Pochopenie aliasov modelov a dynamických portov

---

### Časť 2: Hlboký ponor do Foundry Local SDK

**Laboratórny návod:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Prečo použiť SDK namiesto CLI pri vývoji aplikácií
- Kompletná referenčná dokumentácia SDK API pre Python, JavaScript a C#
- Správa služby, prehliadanie katalógu, životný cyklus modelu (sťahovanie, načítanie, uvoľňovanie)
- Vzory rýchleho štartu: Python konštruktor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metadáta, aliasy a výber modelu optimalizovaného pre hardvér

---

### Časť 3: SDK a API

**Laboratórny návod:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Pripojenie k Foundry Local z Python, JavaScript a C#
- Použitie Foundry Local SDK na programatickú správu služby
- Streamovanie chat dokončení cez OpenAI-kompatibilné API
- Referencia metód SDK pre každý jazyk

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Základný streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat s .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat v Node.js |

---

### Časť 4: Retrieval-Augmented Generation (RAG)

**Laboratórny návod:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Čo je RAG a prečo je dôležitý
- Vytváranie pamäťovej znalostnej bázy
- Vyhľadávanie cez prekrývanie kľúčových slov s bodovaním
- Kompozícia uzemnených systémových promptov
- Spustenie kompletnej RAG pipeline priamo na zariadení

**Ukážky kódu:**

| Jazyk | Súbor |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Časť 5: Tvorba AI agentov

**Laboratórny návod:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Čo je AI agent (oproti priamej LLM požiadavke)
- Vzor `ChatAgent` a Microsoft Agent Framework
- Systémové inštrukcie, persony a viackrokové konverzácie
- Štruktúrovaný výstup (JSON) od agentov

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Jediný agent s Agent Framework |
| C# | `csharp/SingleAgent.cs` | Jediný agent (ChatAgent vzor) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Jediný agent (ChatAgent vzor) |

---

### Časť 6: Workflowy s viacerými agentmi

**Laboratórny návod:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline s viacerými agentmi: Výskumník → Spisovateľ → Editor
- Sekvenčná orchestrácia a spätnoväzbové slučky
- Zdieľaná konfigurácia a štruktúrované predávanie úloh
- Navrhnutie vlastného multiagentného workflowu

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline s tromi agentmi |
| C# | `csharp/MultiAgent.cs` | Pipeline s tromi agentmi |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline s tromi agentmi |

---

### Časť 7: Zava Creative Writer - Capstone aplikácia

**Laboratórny návod:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Produkčná multiagentná aplikácia so 4 špecializovanými agentmi
- Sekvenčná pipeline so spätnoväzbovými slučkami riadenými hodnotiteľom
- Streamovaný výstup, vyhľadávanie v katalógu produktov, štruktúrované JSON predávanie
- Plná implementácia v Pythone (FastAPI), JavaScript (Node.js CLI) a C# (.NET konzola)

**Ukážky kódu:**

| Jazyk | Adresár | Popis |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI web služba s orchestrátorom |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI aplikácia |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konzolová aplikácia |

---

### Časť 8: Vývoj riadený hodnotením

**Laboratórny návod:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Vytvorenie systematického vyhodnocovacieho rámca pre AI agentov pomocou golden datasetov
- Pravidlové kontroly (dĺžka, pokrytie kľúčových slov, zakázané výrazy) + hodnotenie LLM ako sudca
- Porovnanie variant promptov vedľa seba s agregovanými hodnoteniami
- Rozšírenie vzoru Zava Editor agenta z Časti 7 o offline testovaciu sadu
- Python, JavaScript a C# vetvy

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Vyhodnocovací rámec |
| C# | `csharp/AgentEvaluation.cs` | Vyhodnocovací rámec |
| JavaScript | `javascript/foundry-local-eval.mjs` | Vyhodnocovací rámec |

---

### Časť 9: Prepis hlasu s Whisper

**Laboratórny návod:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Prepis reči na text pomocou OpenAI Whisper spusteného lokálne
- Spracovanie audio súkromia - audio nikdy neopúšťa vaše zariadenie
- Python, JavaScript a C# vetvy s `client.audio.transcriptions.create()` (Python/JS) a `AudioClient.TranscribeAudioAsync()` (C#)
- Obsahuje ukážkové audio súbory na prax s témou Zava

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Prepis hlasu Whisper |
| C# | `csharp/WhisperTranscription.cs` | Prepis hlasu Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Prepis hlasu Whisper |

> **Poznámka:** Tento laboratórny blok využíva **Foundry Local SDK** na programatické stiahnutie a načítanie Whisper modelu, potom odosiela audio na lokálny OpenAI-kompatibilný endpoint pre prepis. Model Whisper (`whisper`) je uvedený v katalógu Foundry Local a beží úplne lokálne - nie sú potrebné žiadne cloud API kľúče ani sieťový prístup.

---

### Časť 10: Použitie vlastných alebo Hugging Face modelov

**Laboratórny návod:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilácia Hugging Face modelov do optimalizovaného ONNX formátu pomocou ONNX Runtime GenAI model buildera
- Hardvérová špecifická kompilácia (CPU, NVIDIA GPU, DirectML, WebGPU) a kvantizácia (int4, fp16, bf16)
- Vytvorenie konfiguračných súborov chat-šablón pre Foundry Local
- Pridanie skompilovaných modelov do cache Foundry Local
- Spúšťanie vlastných modelov cez CLI, REST API a OpenAI SDK
- Referenčný príklad: end-to-end kompilácia Qwen/Qwen3-0.6B

---

### Časť 11: Volanie nástrojov s lokálnymi modelmi

**Laboratórny návod:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Umožniť lokálnym modelom volať externé funkcie (volanie nástrojov/funkcií)
- Definovanie schém nástrojov pomocou formátu OpenAI function-calling
- Riešenie multi-turn konverzácie pri volaní nástrojov
- Lokálne vykonávanie volaní nástrojov a vrátenie výsledkov modelu
- Výber správneho modelu pre scenáre tool-calling (Qwen 2.5, Phi-4-mini)
- Použitie natívneho `ChatClient` SDK pre volania nástrojov (JavaScript)

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Volanie nástrojov so službou počasia/populácie |
| C# | `csharp/ToolCalling.cs` | Volanie nástrojov s .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Volanie nástrojov s ChatClient |

---

### Časť 12: Vytváranie webového UI pre Zava Creative Writer

**Laboratórny návod:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Pridanie prehliadačového frontendu pre Zava Creative Writer
- Podávanie zdieľaného UI z Python (FastAPI), JavaScript (Node.js HTTP) a C# (ASP.NET Core)
- Konzumovanie streamovaného NDJSON v prehliadači cez Fetch API a ReadableStream
- Živé statusové značky agenta a realtime streamovanie textu článkov

**Kód (zdieľané UI):**

| Súbor | Popis |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Rozloženie stránky |
| `zava-creative-writer-local/ui/style.css` | Štýlovanie |
| `zava-creative-writer-local/ui/app.js` | Logika čítania streamu a aktualizácie DOM |

**Backend doplnky:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Aktualizované podávanie statického UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nový HTTP server s orchestrátorom |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nový ASP.NET Core minimal API projekt |

---

### Časť 13: Workshop dokončený
**Lab manuál:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Zhrnutie všetkého, čo ste postavili počas všetkých 12 častí
- Ďalšie nápady na rozšírenie vašich aplikácií
- Odkazy na zdroje a dokumentáciu

---

## Štruktúra projektu

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

## Zdroje

| Zdroj | Odkaz |
|----------|------|
| Web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Katalóg modelov | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Sprievodca začiatkom | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referencia Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licencia

Tento materiál z workshopu je poskytovaný na vzdelávacie účely.

---

**Veľa šťastia pri budovaní! 🚀**