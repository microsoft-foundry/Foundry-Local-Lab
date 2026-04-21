<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Vytvárajte AI aplikácie priamo na zariadení

Praktický workshop na spúšťanie jazykových modelov na vlastnom zariadení a vytváranie inteligentných aplikácií pomocou [Foundry Local](https://foundrylocal.ai) a [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Čo je Foundry Local?** Foundry Local je ľahký runtime, ktorý vám umožňuje sťahovať, spravovať a obsluhovať jazykové modely úplne lokálne na vašom hardvéri. Poskytuje **API kompatibilné s OpenAI**, takže sa môže pripojiť akýkoľvek nástroj alebo SDK, ktoré podporuje OpenAI – bez potreby cloudového účtu.

### 🌐 Podpora viacerých jazykov

#### Podporované cez GitHub Action (automatizované a vždy aktuálne)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](./README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Preferujete klonovanie lokálne?**
>
> Tento repozitár obsahuje viac ako 50 jazykových prekladov, čo výrazne zvyšuje veľkosť sťahovania. Ak chcete klonovať bez prekladov, použite sparse checkout:
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
> Tým získate všetko potrebné na dokončenie kurzu so značne rýchlejším sťahovaním.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Ciele učenia

Na konci tohto workshopu budete schopní:

| # | Cieľ |
|---|-----------|
| 1 | Nainštalovať Foundry Local a spravovať modely cez CLI |
| 2 | Ovládnuť API Foundry Local SDK na programatickú správu modelov |
| 3 | Pripojiť sa k lokálnemu inferenčnému serveru pomocou Python, JavaScript a C# SDK |
| 4 | Vytvoriť Retrieval-Augmented Generation (RAG) pipeline, ktorá zakladá odpovede na vlastných dátach |
| 5 | Vytvoriť AI agenty s trvalými inštrukciami a personami |
| 6 | Orchestruvať multi-agentné pracovné postupy so spätnou väzbou |
| 7 | Preskúmať produkčnú záverečnú aplikáciu - Zava Creative Writer |
| 8 | Vytvárať evaluačné rámce so zlatými datasetmi a LLM ako rozhodcom |
| 9 | Prepisovať audio pomocou Whisper – prevod reči na text priamo na zariadení cez Foundry Local SDK |
| 10 | Kompilovať a spúšťať vlastné alebo Hugging Face modely s ONNX Runtime GenAI a Foundry Local |
| 11 | Umožniť lokálnym modelom volať externé funkcie pomocou patrónu tool-calling |
| 12 | Vytvoriť webové UI pre Zava Creative Writer s prenosom v reálnom čase |

---

## Predpoklady

| Požiadavka | Detaily |
|-------------|---------|
| **Hardvér** | Minimálne 8 GB RAM (odporúča sa 16 GB); CPU s podporou AVX2 alebo podporovaná GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025 alebo macOS 13+ |
| **Foundry Local CLI** | Inštalácia cez `winget install Microsoft.FoundryLocal` (Windows) alebo `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Pozrite si [návod na začiatok](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) pre podrobnosti. |
| **Runtime jazyka** | **Python 3.9+** a/alebo **.NET 9.0+** a/alebo **Node.js 18+** |
| **Git** | Na klonovanie tohto repozitára |

---

## Začíname

```bash
# 1. Klonujte repozitár
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Overte, či je nainštalovaný Foundry Local
foundry model list              # Zoznam dostupných modelov
foundry model run phi-3.5-mini  # Spustite interaktívny chat

# 3. Vyberte svoju jazykovú stopu (pozri časť 2 laboratória pre úplnú inštaláciu)
```

| Jazyk | Rýchly štart |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Časti workshopu

### Časť 1: Začíname s Foundry Local

**Návod:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Čo je Foundry Local a ako funguje
- Inštalácia CLI na Windows a macOS
- Preskúmanie modelov – zoznam, sťahovanie, spúšťanie
- Pochopenie aliasov modelov a dynamických portov

---

### Časť 2: Hlboký ponor do Foundry Local SDK

**Návod:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Prečo používať SDK namiesto CLI na vývoj aplikácií
- Kompletná referenčná API SDK pre Python, JavaScript a C#
- Správa služby, prezeranie katalógu, životný cyklus modelov (stiahnutie, načítanie, uvoľnenie)
- Rýchle štartovacie vzory: bootstrap konštruktora Python, `init()` JavaScript, `CreateAsync()` C#
- `FoundryModelInfo` metadata, aliasy a výber optimálneho modelu podľa hardvéru

---

### Časť 3: SDK a API

**Návod:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Pripojenie k Foundry Local z Python, JavaScript a C#
- Používanie Foundry Local SDK na programatickú správu služby
- Streaming chatových dokončení cez API kompatibilné s OpenAI
- Referencia metód SDK pre každý jazyk

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Základný streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat s .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat s Node.js |

---

### Časť 4: Retrieval-Augmented Generation (RAG)

**Návod:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Čo je RAG a prečo je dôležitý
- Vytváranie znalostnej databázy v pamäti
- Vyhľadávanie podľa kľúčových slov s hodnotením
- Tvorba zakotvených systémových promptov
- Spustenie kompletnej RAG pipeline priamo na zariadení

**Ukážky kódu:**

| Jazyk | Súbor |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Časť 5: Vytváranie AI agentov

**Návod:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Čo je AI agent (oproti priamej výzve na LLM)
- Vzor `ChatAgent` a Microsoft Agent Framework
- Systémové inštrukcie, persony a viackrokové rozhovory
- Štruktúrovaný výstup (JSON) od agentov

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Jeden agent s Agent Framework |
| C# | `csharp/SingleAgent.cs` | Jeden agent (vzor ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Jeden agent (vzor ChatAgent) |

---

### Časť 6: Multi-agentné pracovné postupy

**Návod:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Multi-agentné pipeline: Výskumník → Spisovateľ → Editor
- Sekvenčná orchestrácia a spätné slučky
- Zdieľaná konfigurácia a štruktúrované predávanie
- Návrh vlastného multi-agentného pracovného postupu

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Trojagentový pipeline |
| C# | `csharp/MultiAgent.cs` | Trojagentový pipeline |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Trojagentový pipeline |

---

### Časť 7: Zava Creative Writer - záverečná aplikácia

**Návod:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Produkčná multi-agentná aplikácia so 4 špecializovanými agentmi
- Sekvenčný pipeline so spätnými slučkami riadenými evaluátorom
- Streaming výstup, vyhľadávanie v produktovom katalógu, štruktúrované JSON odovzdávanie
- Plná implementácia v Pythone (FastAPI), JavaScripte (Node.js CLI) a C# (.NET konzola)

**Ukážky kódu:**

| Jazyk | Zložka | Popis |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI webová služba s orchestrátorom |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI aplikácia |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konzolová aplikácia |

---

### Časť 8: Vývoj riadený evaluáciou

**Návod:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Vytvoriť systematický evaluačný rámec pre AI agentov s použitím zlatých datasetov
- Pravidlové kontroly (dĺžka, pokrytie kľúčových slov, zakázané výrazy) + skóre LLM ako rozhodcu
- Porovnanie variantov promptu vedľa seba s agregovanými hodnoteniami
- Rozšírenie vzoru Zava Editor agent z Časti 7 do offline testovacej sady
- Pátracky pre Python, JavaScript a C#

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evaluačný rámec |
| C# | `csharp/AgentEvaluation.cs` | Evaluačný rámec |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evaluačný rámec |

---

### Časť 9: Prepis hlasu pomocou Whisper

**Návod:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Prepis reči na text pomocou OpenAI Whisper bežiaceho lokálne
- Spracovanie zvuku s dôrazom na súkromie – zvuk nikdy neopustí vaše zariadenie
- Python, JavaScript a C# príklady s `client.audio.transcriptions.create()` (Python/JS) a `AudioClient.TranscribeAudioAsync()` (C#)
- Obsahuje ukážkové zvukové súbory s témou Zava na praktické cvičenia

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Prepis hlasu pomocou Whisper |
| C# | `csharp/WhisperTranscription.cs` | Prepis hlasu pomocou Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Prepis hlasu pomocou Whisper |

> **Poznámka:** Tento workshop používa **Foundry Local SDK** na programatické stiahnutie a načítanie modelu Whisper, potom odosiela zvuk na lokálny OpenAI-kompatibilný endpoint na prepis. Model Whisper (`whisper`) je uvedený v katalógu Foundry Local a beží úplne na zariadení – nie sú potrebné žiadne cloudové API kľúče ani sieťový prístup.

---

### Časť 10: Použitie vlastných alebo Hugging Face modelov

**Návod k workshopu:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompilácia modelov Hugging Face do optimalizovaného formátu ONNX pomocou ONNX Runtime GenAI konštruktora modelov
- Hardvérovo špecifická kompilácia (CPU, NVIDIA GPU, DirectML, WebGPU) a kvantizácia (int4, fp16, bf16)
- Vytváranie konfiguračných súborov šablón chatov pre Foundry Local
- Pridávanie kompilovaných modelov do cache Foundry Local
- Spúšťanie vlastných modelov cez CLI, REST API a OpenAI SDK
- Referenčný príklad: kompletná kompilácia Qwen/Qwen3-0.6B

---

### Časť 11: Volanie nástrojov s lokálnymi modelmi

**Návod k workshopu:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Povolenie lokálnych modelov volať externé funkcie (volanie nástrojov/funkcií)
- Definovanie schém nástrojov pomocou OpenAI formátu volania funkcií
- Spracovanie konverzácie viacerých kôl volania nástrojov
- Lokálne vykonávanie volaní nástrojov a vracanie výsledkov modelu
- Výber správneho modelu pre scenáre volania nástrojov (Qwen 2.5, Phi-4-mini)
- Použitie natívneho `ChatClient` z SDK pre volanie nástrojov (JavaScript)

**Ukážky kódu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Volanie nástrojov pre počasie/populáciu |
| C# | `csharp/ToolCalling.cs` | Volanie nástrojov s .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Volanie nástrojov s ChatClient |

---

### Časť 12: Budovanie webového UI pre Zava Creative Writer

**Návod k workshopu:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Pridanie prehliadačového front-endu pre Zava Creative Writer
- Servovanie spoločného UI z Pythonu (FastAPI), JavaScriptu (Node.js HTTP) a C# (ASP.NET Core)
- Spracovanie streamovaného NDJSON v prehliadači pomocou Fetch API a ReadableStream
- Živé stavové odznaky agenta a streamovanie textu článku v reálnom čase

**Kód (spoločné UI):**

| Súbor | Popis |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Rozloženie stránky |
| `zava-creative-writer-local/ui/style.css` | Štýlovanie |
| `zava-creative-writer-local/ui/app.js` | Logika čítania streamu a aktualizácie DOM |

**Doplnky backendu:**

| Jazyk | Súbor | Popis |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Aktualizované servovanie statického UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Nový HTTP server obalujúci orchestrátor |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Nový ASP.NET Core minimal API projekt |

---

### Časť 13: Workshop dokončený

**Návod k workshopu:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Zhrnutie všetkého, čo ste vytvorili vo všetkých 12 častiach
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
| Webová stránka Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Katalóg modelov | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Návod na začiatok | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Referencia Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licencia

Materiál z tohto workshopu je poskytovaný na vzdelávacie účely.

---

**Prajeme príjemné tvorenie! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Zrieknutie sa zodpovednosti**:  
Tento dokument bol preložený pomocou AI prekladateľskej služby [Co-op Translator](https://github.com/Azure/co-op-translator). Hoci sa snažíme o presnosť, vezmite prosím na vedomie, že automatizované preklady môžu obsahovať chyby alebo nepresnosti. Originálny dokument v jeho pôvodnom jazyku by mal byť považovaný za autoritatívny zdroj. Pre kritické informácie sa odporúča profesionálny ľudský preklad. Nie sme zodpovední za akékoľvek nedorozumenia alebo nesprávne interpretácie vyplývajúce z použitia tohto prekladu.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->