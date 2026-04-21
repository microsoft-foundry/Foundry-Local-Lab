<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Workshop Foundry Local – Vytvářejte AI aplikace přímo na zařízení

Praktický workshop pro spuštění jazykových modelů na vašem vlastním počítači a tvorbu inteligentních aplikací s [Foundry Local](https://foundrylocal.ai) a [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Co je Foundry Local?** Foundry Local je lehké runtime, které vám umožní stahovat, spravovat a nasazovat jazykové modely zcela na vašem hardwaru. Nabízí **API kompatibilní s OpenAI**, takže jakýkoli nástroj nebo SDK kompatibilní s OpenAI může připojit – bez nutnosti účtu v cloudu.

---

## Výukové cíle

Na konci tohoto workshopu budete umět:

| # | Cíl |
|---|-----|
| 1 | Nainstalovat Foundry Local a spravovat modely přes CLI |
| 2 | Ovládnout API Foundry Local SDK pro programatickou správu modelů |
| 3 | Připojit se k lokálnímu inference serveru pomocí SDK pro Python, JavaScript a C# |
| 4 | Vytvořit Retrieval-Augmented Generation (RAG) pipeline, která zakládá odpovědi na vašich datech |
| 5 | Vytvořit AI agenty s perzistentními instrukcemi a personami |
| 6 | Orchestrace multi-agentních workflow s feedback loop |
| 7 | Prozkoumat produkční capstone aplikaci - Zava Creative Writer |
| 8 | Vytvořit hodnotící framework s golden datasetem a hodnocením LLM jako soudce |
| 9 | Přepisovat audio pomocí Whisper – převod řeči na text přímo na zařízení s Foundry Local SDK |
| 10 | Kompilovat a spouštět vlastní nebo Hugging Face modely pomocí ONNX Runtime GenAI a Foundry Local |
| 11 | Umožnit lokálním modelům volat externí funkce pomocí vzoru tool-calling |
| 12 | Postavit webové uživatelské rozhraní pro Zava Creative Writer s real-time streamováním |

---

## Předpoklady

| Požadavek | Detaily |
|-----------|---------|
| **Hardware** | Minimálně 8 GB RAM (doporučeno 16 GB); CPU s podporou AVX2 nebo podporovaná GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025 nebo macOS 13+ |
| **Foundry Local CLI** | Instalace přes `winget install Microsoft.FoundryLocal` (Windows) nebo `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Podrobnosti v [průvodci začátkem](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| **Jazykové runtime** | **Python 3.9+** a/nebo **.NET 9.0+** a/nebo **Node.js 18+** |
| **Git** | Pro klonování tohoto repozitáře |

---

## Začínáme

```bash
# 1. Naklonujte repozitář
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Zkontrolujte, zda je nainstalován Foundry Local
foundry model list              # Vypsat dostupné modely
foundry model run phi-3.5-mini  # Spustit interaktivní konverzaci

# 3. Vyberte svou jazykovou verzi (viz část 2 laboratoře pro úplné nastavení)
```

| Jazyk | Rychlý start |
|--------|--------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Části workshopu

### Část 1: Začínáme s Foundry Local

**Průvodce laboratoří:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Co je Foundry Local a jak funguje
- Instalace CLI na Windows a macOS
- Prozkoumání modelů – výpis, stahování, spouštění
- Porozumění aliasům modelů a dynamickým portům

---

### Část 2: Hloubkový pohled na Foundry Local SDK

**Průvodce laboratoří:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Proč používat SDK místo CLI při vývoji aplikací
- Kompletní API reference SDK pro Python, JavaScript a C#
- Správa služby, prohlížení katalogu, životní cyklus modelu (stahování, načítání, uvolňování)
- Vzory rychlého startu: bootstrap konstruktoru v Pythonu, `init()` v JavaScriptu, `CreateAsync()` v C#
- Metadatová třída `FoundryModelInfo`, aliasy a výběr hardwarově optimálních modelů

---

### Část 3: SDK a API

**Průvodce laboratoří:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Připojení k Foundry Local z Pythonu, JavaScriptu a C#
- Používání Foundry Local SDK pro programatickou správu služby
- Streamování chatových odpovědí přes API kompatibilní s OpenAI
- Referenční přehled metod SDK pro každý jazyk

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|-------|--------|-------|
| Python | `python/foundry-local.py` | Základní streamovaný chat |
| C# | `csharp/BasicChat.cs` | Streamovaný chat s .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streamovaný chat s Node.js |

---

### Část 4: Retrieval-Augmented Generation (RAG)

**Průvodce laboratoří:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Co je RAG a proč je důležitý
- Vytvoření znalostní základny v paměti
- Vyhledávání na základě překryvu klíčových slov s ohodnocením
- Sestavování promyšlených systémových promptů
- Spuštění kompletního RAG pipeline přímo na zařízení

**Ukázky kódu:**

| Jazyk | Soubor |
|-------|--------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Část 5: Vytváření AI agentů

**Průvodce laboratoří:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Co je AI agent (oproti přímému volání LLM)
- Vzor `ChatAgent` a Microsoft Agent Framework
- Systémové instrukce, persony a vícerychlostní konverzace
- Strukturovaný výstup (JSON) od agentů

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|-------|--------|-------|
| Python | `python/foundry-local-with-agf.py` | Jediný agent s Agent Framework |
| C# | `csharp/SingleAgent.cs` | Jediný agent (vzor ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Jediný agent (vzor ChatAgent) |

---

### Část 6: Víceagentní pracovní postupy

**Průvodce laboratoří:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Víceagentní pipeline: Výzkumník → Spisovatel → Editor
- Sekvenční orchestrace a smyčky zpětné vazby
- Sdílená konfigurace a strukturované předávání
- Návrh vlastních multi-agentních pracovních postupů

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|-------|--------|-------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline se třemi agenty |
| C# | `csharp/MultiAgent.cs` | Pipeline se třemi agenty |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline se třemi agenty |

---

### Část 7: Zava Creative Writer – capstone aplikace

**Průvodce laboratoří:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Produkční styl víceagentní aplikace se 4 specializovanými agenty
- Sekvenční pipeline s evaluátorem řízenými smyčkami zpětné vazby
- Streamovaný výstup, vyhledávání v produktovém katalogu, strukturované JSON předávání
- Kompletní implementace v Pythonu (FastAPI), JavaScriptu (Node.js CLI) a C# (.NET konzole)

**Ukázky kódu:**

| Jazyk | Adresář | Popis |
|-------|---------|-------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI webová služba s orchestrátorem |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI aplikace |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konzolová aplikace |

---

### Část 8: Vývoj řízený evaluací

**Průvodce laboratoří:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Vytvořit systematický evaluační framework pro AI agenty pomocí golden datasetů
- Kontroly založené na pravidlech (délka, pokrytí klíčových slov, zakázané výrazy) + hodnocení LLM jako soudce
- Porovnání variant promptů vedle sebe s agregovanými výsledkovými kartami
- Rozšíření vzoru Zava Editor agenta z Části 7 do offline testovací sady
- Sledy pro Python, JavaScript a C#

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|-------|--------|-------|
| Python | `python/foundry-local-eval.py` | Evaluační framework |
| C# | `csharp/AgentEvaluation.cs` | Evaluační framework |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evaluační framework |

---

### Část 9: Přepis hlasu pomocí Whisper

**Průvodce laboratoří:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Přepis řeči na text pomocí OpenAI Whisper běžící lokálně
- Zpracování audia s prioritou soukromí – audio nikdy neopouští vaše zařízení
- Sledy pro Python, JavaScript a C# s metodami `client.audio.transcriptions.create()` (Python/JS) a `AudioClient.TranscribeAudioAsync()` (C#)
- Obsahuje ukázkové audio soubory s tématikou Zava pro praktický trénink

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|-------|--------|-------|
| Python | `python/foundry-local-whisper.py` | Přepis hlasu Whisper |
| C# | `csharp/WhisperTranscription.cs` | Přepis hlasu Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Přepis hlasu Whisper |

> **Poznámka:** Tato laboratoř používá **Foundry Local SDK** pro programatické stažení a načtení modelu Whisper a poté posílá audio do lokálního OpenAI-kompatibilního endpointu pro přepis. Model Whisper (`whisper`) je uveden v katalogu Foundry Local a běží zcela na zařízení – nejsou potřeba žádné cloudové API klíče ani přístup k síti.

---

### Část 10: Používání vlastních nebo Hugging Face modelů

**Průvodce laboratoří:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilace Hugging Face modelů do optimalizovaného formátu ONNX pomocí ONNX Runtime GenAI model builderu
- Hardwarově specifická kompilace (CPU, NVIDIA GPU, DirectML, WebGPU) a kvantizace (int4, fp16, bf16)
- Vytváření konfiguračních souborů chat šablon pro Foundry Local
- Přidávání kompilovaných modelů do cache Foundry Local
- Spouštění vlastních modelů přes CLI, REST API a OpenAI SDK
- Referenční příklad: end-to-end kompilace Qwen/Qwen3-0.6B

---

### Část 11: Volání nástrojů s lokálními modely

**Průvodce laboratoří:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Umožnění lokálním modelům volat externí funkce (tool/function calling)
- Definování schémat nástrojů pomocí formátu OpenAI pro volání funkcí
- Řízení vícerychlostního konverzačního toku tool-calling
- Lokální vykonání volání nástrojů a předání výsledků modelu
- Volba vhodného modelu pro tool-calling scénáře (Qwen 2.5, Phi-4-mini)
- Použití nativního `ChatClient` SDK pro volání nástrojů (JavaScript)

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|-------|--------|-------|
| Python | `python/foundry-local-tool-calling.py` | Volání nástrojů s nástroji počasí/populace |
| C# | `csharp/ToolCalling.cs` | Volání nástrojů s .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Volání nástrojů s ChatClient |

---

### Část 12: Vytvoření webového UI pro Zava Creative Writer

**Průvodce laboratoří:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Přidání prohlížečového front-endu ke Zava Creative Writeru
- Servírování sdíleného UI z Pythonu (FastAPI), JavaScriptu (Node.js HTTP) a C# (ASP.NET Core)
- Konzumace streamovaného NDJSON v prohlížeči pomocí Fetch API a ReadableStream
- Live odznaky stavu agenta a streamování textu článku v reálném čase

**Kód (sdílené UI):**

| Soubor | Popis |
|--------|--------|
| `zava-creative-writer-local/ui/index.html` | Rozvržení stránky |
| `zava-creative-writer-local/ui/style.css` | Stylování |
| `zava-creative-writer-local/ui/app.js` | Logika čtečky streamu a aktualizace DOM |

**Backend doplňky:**

| Jazyk | Soubor | Popis |
|-------|--------|-------|
| Python | `zava-creative-writer-local/src/api/main.py` | Aktualizováno pro servírování statického UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nový HTTP server obalující orchestrátor |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nový ASP.NET Core minimal API projekt |

---

### Část 13: Workshop dokončený
**Průvodce laboratoří:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Přehled všeho, co jste vybudovali ve všech 12 částech
- Další nápady na rozšíření vašich aplikací
- Odkazy na zdroje a dokumentaci

---

## Struktura projektu

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
| Katalog modelů | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Průvodce začátkem | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Dokumentace Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licence

Materiál tohoto workshopu je poskytován pro vzdělávací účely.

---

**Přeji hodně úspěchů při stavbě! 🚀**