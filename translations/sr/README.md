<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local радионица - Прављење AI апликација на уређају

Практична радионица за покретање језичких модела на вашем уређају и прављење интелигентних апликација са [Foundry Local](https://foundrylocal.ai) и [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Шта је Foundry Local?** Foundry Local је лагано извршно окружење које вам омогућава преузимање, управљање и сервирање језичких модела у потпуности на вашем хардверу. Излаже **OpenAI-компатибилан API** тако да било који алат или SDK који користи OpenAI може да се повеже - није потребан ниједан облачни налог.

### 🌐 Подршка за више језика

#### Подржано преко GitHub Action (Аутоматски и увек ажурно)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](./README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Више волите да клонирате локално?**
>
> Ово складиште садржи преко 50 превода што знатно повећава величину преузимања. Да бисте клонирали без превода, користите sparse checkout:
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
> Ово вам даје све што вам је потребно за завршетак курса уз много брже преузимање.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Циљеви учења

До краја ове радионице моћи ћете да:

| # | Циљ |
|---|-----------|
| 1 | Инсталирате Foundry Local и управљате моделима преко CLI |
| 2 | Савладате Foundry Local SDK API за програмско управљање моделима |
| 3 | Повежете се са локалним inference сервером користећи Python, JavaScript и C# SDK |
| 4 | Направите Retrieval-Augmented Generation (RAG) проток који утемељује одговоре на вашим подацима |
| 5 | Креирате AI агенте са упутствима и персоналитетима који се чувају |
| 6 | Оркестрирате мулти-агентске токове рада са повратним везама |
| 7 | Истражите production capstone апликацију - Zava Creative Writer |
| 8 | Направите оцену са оквирима користећи golden datasets и LLM-as-judge оцењивање |
| 9 | Транскриптујете аудио са Whisper - говор у текст на уређају користећи Foundry Local SDK |
| 10 | Компилујете и покрећете прилагођене или Hugging Face моделе са ONNX Runtime GenAI и Foundry Local |
| 11 | Омогућите локалним моделима да позивају екстерне функције помоћу tool-calling патерна |
| 12 | Направите UI заснован на прегледачу за Zava Creative Writer са реал-тиме стримовањем |

---

## Захтеви

| Захтев | Детаљи |
|-------------|---------|
| **Хардвер** | Минимум 8 GB RAM-а (препоручено 16 GB); CPU са AVX2 или подржана GPU |
| **Оперативни систем** | Windows 10/11 (x64/ARM), Windows Server 2025, или macOS 13+ |
| **Foundry Local CLI** | Инсталирајте преко `winget install Microsoft.FoundryLocal` (Windows) или `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Погледајте [водич за почетак](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) за детаље. |
| **Језичко извршно окружење** | **Python 3.9+** и/или **.NET 9.0+** и/или **Node.js 18+** |
| **Git** | За клонирање овог репозиторијума |

---

## Почетак

```bash
# 1. Клонирајте репозиторијум
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Проверите да ли је Foundry Local инсталиран
foundry model list              # Прикажите листу доступних модела
foundry model run phi-3.5-mini  # Покрените интерактивни ћаскање

# 3. Изаберите свој језички правац (погледајте лабораторију Парт 2 за потпуно подешавање)
```

| Језик | Брзи почетак |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Делови радионице

### Део 1: Почетак рада са Foundry Local

**Водич радионице:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Шта је Foundry Local и како ради
- Инсталирање CLI на Windows и macOS
- Истраживање модела - листање, преузимање, покретање
- Разумевање псеудонима модела и динамичких портова

---

### Део 2: Детаљан преглед Foundry Local SDK

**Водич радионице:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Зашто користити SDK уместо CLI за развој апликација
- Потпуна SDK API референца за Python, JavaScript и C#
- Управљање сервисом, преглед каталога, животни циклус модела (преузимање, учитавање, ослобађање)
- Шаблони брзог почетка: конструктор у Python-у, `init()` у JavaScript-у, `CreateAsync()` у C#
- Мета-подаци `FoundryModelInfo`, псеудоними и избор модела оптималног за хардвер

---

### Део 3: SDK-ови и API-ји

**Водич радионице:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Повезивање на Foundry Local са Python, JavaScript и C#
- Коришћење Foundry Local SDK за програмско управљање сервисом
- Стримовање chat завршетака преко OpenAI-компатибилног API-ја
- Референца метода SDK-а по језику

**Примери кода:**

| Језик | Фајл | Опис |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Основни streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat са .NET-ом |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat са Node.js |

---

### Део 4: Retrieval-Augmented Generation (RAG)

**Водич радионице:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Шта је RAG и зашто је важан
- Прављење базе знања у меморији
- Претраживање преко преклапања кључних речи са бодовањем
- Састављање утемељених системских упита
- Покретање комплетног RAG протока на уређају

**Примери кода:**

| Језик | Фајл |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Део 5: Прављење AI агената

**Водич радионице:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Шта је AI агент (у односу на директан позив LLM-а)
- Патерн `ChatAgent` и Microsoft Agent Framework
- Системска упутства, персоне и вишеслојни разговори
- Структурирани излаз (JSON) из агената

**Примери кода:**

| Језик | Фајл | Опис |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Један агент са Agent Framework-ом |
| C# | `csharp/SingleAgent.cs` | Један агент (ChatAgent патерн) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Један агент (ChatAgent патерн) |

---

### Део 6: Мулти-агентски токови рада

**Водич радионице:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Мулти-агентски протоколи: Истраживач → Писац → Уређивач
- Каскадно оркестрирање и повратне везе
- Заједничка конфигурација и структурирана предаја задатака
- Дизајнирање сопственог мулти-агентског тока рада

**Примери кода:**

| Језик | Фајл | Опис |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | проток са три агента |
| C# | `csharp/MultiAgent.cs` | проток са три агента |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | проток са три агента |

---

### Део 7: Zava Creative Writer - Капстоун апликација

**Водич радионице:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Производна мулти-агент апликација са 4 специјализована агента
- Каскадни проток са повратним информацијама које покреће evaluator
- Стриминг излаз, претрага каталога производа, структурирана предаја у JSON-у
- Потпуна имплементација у Python-у (FastAPI), JavaScript-у (Node.js CLI) и C# (.NET конзола)

**Примери кода:**

| Језик | Директоријум | Опис |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI веб сервис са оркестратором |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI апликација |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 конзолна апликација |

---

### Део 8: Развој вођен евалуацијом

**Водич радионице:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Направите системски оквир за евалуацију AI агената користећи golden datasets
- Провере на основу правила (дужина, покривеност кључних речи, забрањени термини) + LLM-ас-џџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџџ Џ) и статус агената |
- Поређење варијанти упита са сажетим резултатима
- Продужава pattern агента Zava Editor из дела 7 у офлајн тест пакет
- Трацкови за Python, JavaScript и C#

**Примери кода:**

| Језик | Фајл | Опис |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Евалуациони оквир |
| C# | `csharp/AgentEvaluation.cs` | Евалуациони оквир |
| JavaScript | `javascript/foundry-local-eval.mjs` | Евалуациони оквир |

---

### Део 9: Транскрипција гласа са Whisper

**Водич радионице:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Транскрипција говора у текст користећи OpenAI Whisper који ради локално  
- Обрада звука с приоритетом приватности - звук никада не напушта ваш уређај  
- Прати рад у Python, JavaScript-у и C# са `client.audio.transcriptions.create()` (Python/JS) и `AudioClient.TranscribeAudioAsync()` (C#)  
- Укључује примерне аудио фајлове са темом Zava за практичан рад  

**Примери кода:**  

| Језик | Фајл | Опис |  
|----------|------|-------------|  
| Python | `python/foundry-local-whisper.py` | Транскрипција гласа Whisper |  
| C# | `csharp/WhisperTranscription.cs` | Транскрипција гласа Whisper |  
| JavaScript | `javascript/foundry-local-whisper.mjs` | Транскрипција гласа Whisper |  

> **Напомена:** Ова лабораторија користи **Foundry Local SDK** за програмско преузимање и учитавање Whisper модела, а затим шаље звук ка локалном OpenAI компатибилном крајњем тачком за транскрипцију. Whisper модел (`whisper`) се налази у Foundry Local каталогу и ради искључиво на уређају - није потребан cloud API кључ или приступ мрежи.  

---  

### Део 10: Коришћење прилагођених или Hugging Face модела  

**Водич кроз лабораторију:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)  

- Компилирање Hugging Face модела у оптимизовани ONNX формат помоћу ONNX Runtime GenAI модел билдера  
- Хардверски специјализовано компилирање (CPU, NVIDIA GPU, DirectML, WebGPU) и квантизација (int4, fp16, bf16)  
- Креирање конфигурационих фајлова шаблона за цхат за Foundry Local  
- Додавање компајлираних модела у кеш Foundry Local  
- Покретање прилагођених модела кроз CLI, REST API и OpenAI SDK  
- Референтни пример: компилација Qwen/Qwen3-0.6B од почетка до краја  

---  

### Део 11: Позив функција помоћу локалних модела  

**Водич кроз лабораторију:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)  

- Омогућавање локалним моделима да позивају екстерне функције (позив алата/функције)  
- Дефинисање шема алата користећи OpenAI формат за позив функција  
- Управљање вишетурним током разговора приликом позива алата  
- Извршавање позива алата локално и враћање резултата моделу  
- Избор одговарајућег модела за сценарије позива алата (Qwen 2.5, Phi-4-mini)  
- Коришћење нативне `ChatClient` класе SDK-а за позив алата (JavaScript)  

**Примери кода:**  

| Језик | Фајл | Опис |  
|----------|------|-------------|  
| Python | `python/foundry-local-tool-calling.py` | Позив алата са метео/популационим алатима |  
| C# | `csharp/ToolCalling.cs` | Позив алата са .NET |  
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Позив алата са ChatClient |  

---  

### Део 12: Израда веб корисничког интерфејса за Zava Creative Writer  

**Водич кроз лабораторију:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)  

- Додавање браузерског фронтенда Zava Creative Writer-у  
- Сервирање заједничког UI-а из Python-а (FastAPI), JavaScript-а (Node.js HTTP) и C# (ASP.NET Core)  
- Конзумирање стримованог NDJSON у прегледачу помоћу Fetch API и ReadableStream  
- Ознаке статуса агента уживо и стримовање текста чланка у реалном времену  

**Код (заједнички UI):**  

| Фајл | Опис |  
|------|-------------|  
| `zava-creative-writer-local/ui/index.html` | Распоред странице |  
| `zava-creative-writer-local/ui/style.css` | Стилизација |  
| `zava-creative-writer-local/ui/app.js` | Логика читача стрима и ажурирања DOM-а |  

**Додаци на серверској страни:**  

| Језик | Фајл | Опис |  
|----------|------|-------------|  
| Python | `zava-creative-writer-local/src/api/main.py` | Измењен за сервирање статичког UI-а |  
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Нови HTTP сервер који обавија оркестратор |  
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Нови ASP.NET Core минимални API пројекат |  

---  

### Део 13: Ратоница завршена  

**Водич кроз лабораторију:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)  

- Резиме свега што сте изградили кроз свих 12 делова  
- Додатне идеје за проширење ваших апликација  
- Линкови ка ресурсима и документацији  

---  

## Структура пројекта  

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

## Ресурси  

| Ресурс | Линк |  
|----------|------|  
| Foundry Local вебсајт | [foundrylocal.ai](https://foundrylocal.ai) |  
| Каталог модела | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |  
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |  
| Водич за почетак | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |  
| Foundry Local SDK референца | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |  
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |  
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |  
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |  

---  

## Лиценца  

Ова радна материја је обезбеђена у образовне сврхе.  

---  

**Срећно у грађењу! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Одрицање од одговорности**:  
Овај документ је преведен помоћу AI услуге за превођење [Co-op Translator](https://github.com/Azure/co-op-translator). Иако се трудимо да превод буде тачан, имајте у виду да аутоматски преводи могу садржати грешке или нетачности. Оригинални документ на његовом изворном језику треба сматрати ауторитетним извором. За критичне информације препоручује се професионални превод који ради људски преводилац. Нисмо одговорни за било каква неспоразума или погрешне тумачења која произилазе из коришћења овог превода.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->