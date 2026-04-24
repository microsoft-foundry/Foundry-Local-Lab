<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop – Vytvářejte AI aplikace přímo na zařízení

Praktický workshop pro spuštění jazykových modelů na vašem počítači a vytváření inteligentních aplikací s [Foundry Local](https://foundrylocal.ai) a [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Co je Foundry Local?** Foundry Local je lehké runtime, které vám umožní stáhnout, spravovat a obsluhovat jazykové modely zcela na vašem hardwaru. Nabízí **OpenAI-kompatibilní API**, takže se může připojit jakýkoliv nástroj či SDK, které umí OpenAI – není potřeba žádný cloudový účet.

### 🌐 Podpora více jazyků

#### Podporováno přes GitHub Action (automatizováno a vždy aktuální)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabština](../ar/README.md) | [Bengálština](../bn/README.md) | [Bulharština](../bg/README.md) | [Barmština (Myanmar)](../my/README.md) | [Čínština (zjednodušená)](../zh-CN/README.md) | [Čínština (tradiční, Hongkong)](../zh-HK/README.md) | [Čínština (tradiční, Macao)](../zh-MO/README.md) | [Čínština (tradiční, Tchaj-wan)](../zh-TW/README.md) | [Chorvatština](../hr/README.md) | [Čeština](./README.md) | [Dánština](../da/README.md) | [Nizozemština](../nl/README.md) | [Estonština](../et/README.md) | [Finština](../fi/README.md) | [Francouzština](../fr/README.md) | [Němčina](../de/README.md) | [Řečtina](../el/README.md) | [Hebrejština](../he/README.md) | [Hindština](../hi/README.md) | [Maďarština](../hu/README.md) | [Indonéština](../id/README.md) | [italština](../it/README.md) | [Japonština](../ja/README.md) | [Kannadština](../kn/README.md) | [Khmer](../km/README.md) | [Korejština](../ko/README.md) | [Litevština](../lt/README.md) | [Malajština](../ms/README.md) | [Malajalámština](../ml/README.md) | [Maráthština](../mr/README.md) | [Nepálština](../ne/README.md) | [Nigerijská pidžinština](../pcm/README.md) | [Norština](../no/README.md) | [Perština (Farsi)](../fa/README.md) | [Polština](../pl/README.md) | [Portugalština (Brazílie)](../pt-BR/README.md) | [Portugalština (Portugalsko)](../pt-PT/README.md) | [Paňdžábština (Gurmukhí)](../pa/README.md) | [Rumunština](../ro/README.md) | [Ruština](../ru/README.md) | [Srbština (cyrilice)](../sr/README.md) | [Slovenština](../sk/README.md) | [Slovinština](../sl/README.md) | [Španělština](../es/README.md) | [Svahilština](../sw/README.md) | [Švédština](../sv/README.md) | [Tagalog (filipínština)](../tl/README.md) | [Tamilština](../ta/README.md) | [Telugština](../te/README.md) | [Thajština](../th/README.md) | [Turečtina](../tr/README.md) | [Ukrajinština](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamština](../vi/README.md)

> **Raději klonovat lokálně?**
>
> Tento repozitář obsahuje přes 50 jazykových překladů, což výrazně zvětšuje velikost ke stažení. Pro klonování bez překladů použijte sparse checkout:
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
> To vám poskytne vše potřebné pro dokončení kurzu s mnohem rychlejším stažením.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Cíle učení

Na konci tohoto workshopu budete umět:

| # | Cíl |
|---|-----|
| 1 | Nainstalovat Foundry Local a spravovat modely přes CLI |
| 2 | Ovládnout Foundry Local SDK API pro programové řízení modelů |
| 3 | Připojit se k lokálnímu inference serveru pomocí SDK pro Python, JavaScript a C# |
| 4 | Vytvořit pipeline pro Retrieval-Augmented Generation (RAG), která zakládá odpovědi na vlastních datech |
| 5 | Vytvářet AI agenty s přetrvávajícími instrukcemi a personami |
| 6 | Orchestrace vícero-agentních workflow s zpětnými vazbami |
| 7 | Prozkoumat produkční závěrečnou aplikaci – Zava Creative Writer |
| 8 | Vytvořit evaluační rámce s golden datasets a hodnocením LLM-as-judge |
| 9 | Přepisovat zvuk pomocí Whisper – převod řeči na text přímo na zařízení pomocí Foundry Local SDK |
| 10 | Kompilovat a spouštět vlastní nebo Hugging Face modely s ONNX Runtime GenAI a Foundry Local |
| 11 | Umožnit lokálním modelům volat externí funkce pomocí vzoru tool-calling |
| 12 | Vytvořit prohlížečové uživatelské rozhraní pro Zava Creative Writer s real-time streamováním |

---

## Předešlé požadavky

| Požadavek | Detail |
|-----------|--------|
| **Hardware** | Minimálně 8 GB RAM (doporučeno 16 GB); CPU s AVX2 podporou nebo podporovaná GPU |
| **Operační systém** | Windows 10/11 (x64/ARM), Windows Server 2025 nebo macOS 13+ |
| **Foundry Local CLI** | Instalujte přes `winget install Microsoft.FoundryLocal` (Windows) nebo `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Podrobnosti viz [průvodce začátkem](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Runtime jazyka** | **Python 3.9+** a/nebo **.NET 9.0+** a/nebo **Node.js 18+** |
| **Git** | Pro klonování tohoto repozitáře |

---

## Začínáme

```bash
# 1. Naklonujte repozitář
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Ověřte, zda je nainstalován Foundry Local
foundry model list              # Vypsat dostupné modely
foundry model run phi-3.5-mini  # Spustit interaktivní chat

# 3. Vyberte si jazykovou cestu (viz část 2 laboratoř pro kompletní nastavení)
```

| Jazyk | Rychlý start |
|-------|--------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Části workshopu

### Část 1: Začínáme s Foundry Local

**Návod k laboratoři:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Co je Foundry Local a jak funguje
- Instalace CLI na Windows a macOS
- Prozkoumání modelů – výpis, stažení, spuštění
- Pochopení aliasů modelů a dynamických portů

---

### Část 2: Hloubkový průzkum Foundry Local SDK

**Návod k laboratoři:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Proč použít SDK místo CLI pro vývoj aplikací
- Kompletní referenční API SDK pro Python, JavaScript a C#
- Správa služby, prohlížení katalogu, životní cyklus modelů (stažení, načtení, uvolnění)
- Vzory rychlého startu: Python konstruktor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- Metadata `FoundryModelInfo`, aliasy a výběr modelu optimalizovaného pro hardware

---

### Část 3: SDK a API

**Návod k laboratoři:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Připojení k Foundry Local pomocí Pythonu, JavaScriptu a C#
- Používání Foundry Local SDK pro programové řízení služby
- Streamování chatových odpovědí přes OpenAI-kompatibilní API
- Přehled metod SDK pro každý jazyk

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|-------|--------|-------|
| Python | `python/foundry-local.py` | Základní streamovaný chat |
| C# | `csharp/BasicChat.cs` | Streamovaný chat s .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streamovaný chat s Node.js |

---

### Část 4: Retrieval-Augmented Generation (RAG)

**Návod k laboratoři:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Co je RAG a proč je důležitý
- Budování in-memory znalostní báze
- Obrácení klíčových slov a skórování overlapu
- Skládání založených systémových promptů
- Spuštění kompletní RAG pipeline přímo na zařízení

**Ukázky kódu:**

| Jazyk | Soubor |
|-------|--------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Část 5: Vytváření AI agentů

**Návod k laboratoři:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Co je AI agent (oproti běžnému volání LLM)
- Vzor `ChatAgent` a Microsoft Agent Framework
- Systémové instrukce, persony a vícetahové konverzace
- Strukturovaný výstup (JSON) od agentů

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|-------|--------|-------|
| Python | `python/foundry-local-with-agf.py` | Jednotlivý agent s Agent Framework |
| C# | `csharp/SingleAgent.cs` | Jednotlivý agent (vzor ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Jednotlivý agent (vzor ChatAgent) |

---

### Část 6: Víceagentní workflow

**Návod k laboratoři:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Víceagentní pipeline: Výzkumník → Spisovatel → Editor
- Sekvenční orchestrace a zpětné smyčky
- Sdílená konfigurace a strukturované předávání
- Navržení vlastního víceagentního workflow

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|-------|--------|-------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline tří agentů |
| C# | `csharp/MultiAgent.cs` | Pipeline tří agentů |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline tří agentů |

---

### Část 7: Zava Creative Writer – závěrečná aplikace

**Návod k laboratoři:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Produkční multi-agentní aplikace se 4 specializovanými agenty
- Sekvenční pipeline se zpětnovazební smyčkou řízenou hodnotitelem
- Streaming výstupu, vyhledávání v katalogu produktů, strukturované JSON předávání
- Plná implementace v Pythonu (FastAPI), JavaScriptu (Node.js CLI) a C# (.NET konzole)

**Ukázky kódu:**

| Jazyk | Složka | Popis |
|-------|--------|-------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI webová služba s orchestrátorem |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI aplikace |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konzolová aplikace |

---

### Část 8: Vývoj řízený evaluací

**Návod k laboratoři:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Vytvoření systematického evaluačního rámce pro AI agenty pomocí golden datasets
- Kontroly pravidly (délka, pokrytí klíčových slov, zakázané termíny) + hodnocení LLM-as-judge
- Porovnání variant promptů vedle sebe s agregovanými skóre
- Rozšíření vzoru Zava Editor agenta z časti 7 do offline testovacích sad
- Python, JavaScript a C# varianty

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|-------|--------|-------|
| Python | `python/foundry-local-eval.py` | Evaluační rámec |
| C# | `csharp/AgentEvaluation.cs` | Evaluační rámec |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evaluační rámec |

---

### Část 9: Přepis hlasu pomocí Whisper

**Návod k laboratoři:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Přepis řeči na text pomocí OpenAI Whisper spuštěného lokálně
- Zpracování audia s prioritou ochrany soukromí – audio nikdy neopouští vaše zařízení
- Python, JavaScript a C# varianty s `client.audio.transcriptions.create()` (Python/JS) a `AudioClient.TranscribeAudioAsync()` (C#)
- Obsahuje ukázková zvuková data se zava tématikou pro praktické cvičení

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Přepis hlasu pomocí Whisper |
| C# | `csharp/WhisperTranscription.cs` | Přepis hlasu pomocí Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Přepis hlasu pomocí Whisper |

> **Poznámka:** Tento workshop používá **Foundry Local SDK** k programovému stažení a načtení modelu Whisper, poté odesílá audio na lokální endpoint kompatibilní s OpenAI pro přepis. Model Whisper (`whisper`) je uveden v katalogu Foundry Local a běží kompletně lokálně – nejsou potřeba žádné klíče cloud API ani přístup k síti.

---

### Část 10: Použití vlastních nebo Hugging Face modelů

**Návod k workshopu:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilace modelů Hugging Face do optimalizovaného ONNX formátu pomocí ONNX Runtime GenAI nástroje pro tvorbu modelů
- Kompilace specifická podle hardwaru (CPU, NVIDIA GPU, DirectML, WebGPU) a kvantizace (int4, fp16, bf16)
- Vytváření konfiguračních souborů chat šablon pro Foundry Local
- Přidání zkompilovaných modelů do cache Foundry Local
- Spouštění vlastních modelů pomocí CLI, REST API a OpenAI SDK
- Referenční příklad: kompletní kompilace Qwen/Qwen3-0.6B

---

### Část 11: Volání nástrojů s lokálními modely

**Návod k workshopu:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Umožnit lokálním modelům volat externí funkce (volání nástrojů/funkcí)
- Definice schémat nástrojů pomocí formátu volání funkcí OpenAI
- Řízení víceturné konverzace s voláním nástrojů
- Lokální vykonávání volání nástrojů a vracení výsledků modelu
- Výběr správného modelu pro scénáře volání nástrojů (Qwen 2.5, Phi-4-mini)
- Použití nativního `ChatClient` SDK pro volání nástrojů (JavaScript)

**Ukázky kódu:**

| Jazyk | Soubor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Volání nástrojů s počasím/populací |
| C# | `csharp/ToolCalling.cs` | Volání nástrojů s .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Volání nástrojů s ChatClient |

---

### Část 12: Vývoj webového UI pro Zava Creative Writer

**Návod k workshopu:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Přidání webového frontendu pro Zava Creative Writer
- Podávání sdíleného UI z Pythonu (FastAPI), JavaScriptu (Node.js HTTP) a C# (ASP.NET Core)
- Spotřeba streamovaného NDJSON v prohlížeči pomocí Fetch API a ReadableStream
- Živé odznaky stavu agentů a streamování textu článků v reálném čase

**Kód (sdílené UI):**

| Soubor | Popis |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Rozvržení stránky |
| `zava-creative-writer-local/ui/style.css` | Stylování |
| `zava-creative-writer-local/ui/app.js` | Čtečka streamu a logika aktualizace DOM |

**Backendové doplňky:**

| Jazyk | Soubor | Popis |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Aktualizováno pro podávání statického UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nový HTTP server obalující orchestrátor |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nový ASP.NET Core minimal API projekt |

---

### Část 13: Workshop dokončen

**Návod k workshopu:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Shrnutí všeho, co jste vybudovali ve všech 12 částech
- Další nápady pro rozšíření vašich aplikací
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

Tento workshopový materiál je poskytován pro vzdělávací účely.

---

**Příjemné tvoření! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Prohlášení o vyloučení odpovědnosti**:  
Tento dokument byl přeložen pomocí AI překladatelské služby [Co-op Translator](https://github.com/Azure/co-op-translator). Přestože usilujeme o přesnost, mějte na paměti, že automatizované překlady mohou obsahovat chyby nebo nepřesnosti. Původní dokument v jeho mateřském jazyce by měl být považován za autoritativní zdroj. Pro zásadní informace se doporučuje profesionální lidský překlad. Nejsme odpovědní za jakékoli nedorozumění nebo nesprávné interpretace vyplývající z použití tohoto překladu.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->