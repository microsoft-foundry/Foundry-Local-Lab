<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Създаване на AI приложения на устройството

Практически работилница за стартиране на езикови модели на вашата собствена машина и създаване на интелигентни приложения с [Foundry Local](https://foundrylocal.ai) и [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Какво е Foundry Local?** Foundry Local е леко изпълнимо обкръжение, което ви позволява да изтегляте, управлявате и обслужвате езикови модели изцяло на вашия хардуер. Той предоставя **OpenAI-съвместимо API**, така че всякакъв инструмент или SDK, който говори OpenAI, може да се свърже - без нужда от облачен акаунт.

### 🌐 Многоезична поддръжка

#### Поддържани чрез GitHub Action (Автоматизирано и винаги актуално)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](./README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Предпочитате да клонирате локално?**
>
> Това хранилище включва над 50 езикови превода, което значително увеличава размера на изтеглянето. За да клонирате без преводи, използвайте sparse checkout:
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
> Това ви дава всичко необходимо за завършване на курса с много по-бързо изтегляне.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Обучителни цели

Към края на този практически курс ще можете:

| # | Цел |
|---|-----------|
| 1 | Инсталирате Foundry Local и управлявате модели чрез CLI |
| 2 | Умело използвате Foundry Local SDK API за програмно управление на модели |
| 3 | Свързвате се с локалния сървър за инференция чрез Python, JavaScript и C# SDK |
| 4 | Създавате Retrieval-Augmented Generation (RAG) pipeline, който основава отговорите на ваши собствени данни |
| 5 | Създавате AI агенти с постоянни инструкции и персони |
| 6 | Оркестрирате многократен агентски работни процеси с обратни връзки |
| 7 | Изследвате продукционно capstone приложение - Zava Creative Writer |
| 8 | Създавате рамки за оценка с „златни“ набори от данни и оценяване с LLM-като-съдия |
| 9 | Транскрибирате аудио с Whisper - реч към текст на устройството чрез Foundry Local SDK |
| 10 | Компилирате и пускате персонализирани или Hugging Face модели с ONNX Runtime GenAI и Foundry Local |
| 11 | Позволявате на локални модели да извикват външни функции с шаблона tool-calling |
| 12 | Създавате уеб базиран потребителски интерфейс за Zava Creative Writer с реално време стрийминг |

---

## Предварителни изисквания

| Изискване | Подробности |
|-------------|---------|
| **Хардуер** | Минимум 8 GB RAM (препоръчително 16 GB); CPU със AVX2 или поддържан GPU |
| **Операционна система** | Windows 10/11 (x64/ARM), Windows Server 2025 или macOS 13+ |
| **Foundry Local CLI** | Инсталирайте чрез `winget install Microsoft.FoundryLocal` (Windows) или `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Вижте [ръководството за започване](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) за подробности. |
| **Езикова среда** | **Python 3.9+** и/или **.NET 9.0+** и/или **Node.js 18+** |
| **Git** | За клониране на това хранилище |

---

## Започване

```bash
# 1. Клонирайте хранилището
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Проверете дали Foundry Local е инсталиран
foundry model list              # Избройте наличните модели
foundry model run phi-3.5-mini  # Стартирайте интерактивен чат

# 3. Изберете вашия езиков път (вижте лаборатория Част 2 за пълна настройка)
```

| Език | Бързо начало |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Части на работилницата

### Част 1: Започване с Foundry Local

**Ръководство за упражнения:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Какво е Foundry Local и как работи
- Инсталиране на CLI на Windows и macOS
- Изследване на модели - изброяване, изтегляне, стартиране
- Разбиране на модели с алиаси и динамични портове

---

### Част 2: Задълбочено изучаване на Foundry Local SDK

**Ръководство за упражнения:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Защо да използвате SDK вместо CLI за разработка на приложения
- Пълен SDK API справочник за Python, JavaScript и C#
- Управление на услугата, разглеждане на каталог, жизнен цикъл на моделите (изтегляне, зареждане, разтоварване)
- Шаблони за бързо стартиране: Python конструктор bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` метаданни, алиаси и избор на най-оптимален за хардуер модел

---

### Част 3: SDK и API

**Ръководство за упражнения:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Свързване с Foundry Local чрез Python, JavaScript и C#
- Използване на Foundry Local SDK за програмно управление на услугата
- Поточно предоставяне на чат завършвания чрез OpenAI-съвместимия API
- Справочник с методи на SDK за всеки език

**Примерен код:**

| Език | Файл | Описание |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Основен потоков чат |
| C# | `csharp/BasicChat.cs` | Потоков чат с .NET |
| JavaScript | `javascript/foundry-local.mjs` | Потоков чат с Node.js |

---

### Част 4: Retrieval-Augmented Generation (RAG)

**Ръководство за упражнения:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Какво е RAG и защо е важен
- Създаване на база знания в паметта
- Извличане чрез съвпадение на ключови думи с оценяване
- Създаване на обосновани системни подсказки
- Стартиране на пълна RAG линия на устройството

**Примерен код:**

| Език | Файл |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Част 5: Изграждане на AI агенти

**Ръководство за упражнения:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Какво е AI агент (в сравнение със стандартно LLM обаждане)
- Моделът `ChatAgent` и Microsoft Agent Framework
- Системни инструкции, персони и многократни разговори
- Структуриран изход (JSON) от агентите

**Примерен код:**

| Език | Файл | Описание |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Единичен агент с Agent Framework |
| C# | `csharp/SingleAgent.cs` | Единичен агент (патерн ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Единичен агент (патерн ChatAgent) |

---

### Част 6: Многократен агентски работни процеси

**Ръководство за упражнения:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Многократни агентски линии: Изследовател → Писател → Редактор
- Последователна оркестрация и цикли на обратна връзка
- Споделена конфигурация и структурирани предавания
- Дизайн на собствен многократен агентски работен процес

**Примерен код:**

| Език | Файл | Описание |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Тричленна агентска линия |
| C# | `csharp/MultiAgent.cs` | Тричленна агентска линия |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Тричленна агентска линия |

---

### Част 7: Zava Creative Writer - Ключово приложение

**Ръководство за упражнения:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Продукционно многоагентско приложение с 4 специализирани агенти
- Последователна линия с цикли на обратна връзка, водени от оценители
- Поточно изход, търсене в продуктов каталог, структурирани JSON предавания
- Пълна имплементация на Python (FastAPI), JavaScript (Node.js CLI) и C# (.NET конзола)

**Примерен код:**

| Език | Директория | Описание |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI уеб услуга с оркестратор |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI приложение |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 конзолно приложение |

---

### Част 8: Разработка, водена от оценка

**Ръководство за упражнения:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Създаване на систематична рамка за оценка на AI агенти чрез „златни“ набори от данни
- Проверки на правила (дължина, покритие на ключови думи, забранени термини) + LLM-като-съдия оценяване
- Сравнение на различни варианти на подсказки с общи оценки
- Разширение на агентския модел Zava Editor от Част 7 в офлайн тестова среда
- Тройки Python, JavaScript и C#

**Примерен код:**

| Език | Файл | Описание |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Рамка за оценка |
| C# | `csharp/AgentEvaluation.cs` | Рамка за оценка |
| JavaScript | `javascript/foundry-local-eval.mjs` | Рамка за оценка |

---

### Част 9: Транскрипция на глас с Whisper

**Ръководство за упражнения:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Преобразуване от реч в текст с OpenAI Whisper, работещ локално  
- Обработка на аудио с приоритет към поверителността - аудиото никога не напуска устройството ви  
- Треци за Python, JavaScript и C# с `client.audio.transcriptions.create()` (Python/JS) и `AudioClient.TranscribeAudioAsync()` (C#)  
- Включва аудио файлове със Zava тематика за практически упражнения  

**Примери за код:**  

| Език | Файл | Описание |  
|----------|------|-------------|  
| Python | `python/foundry-local-whisper.py` | Преобразуване на глас с Whisper |  
| C# | `csharp/WhisperTranscription.cs` | Преобразуване на глас с Whisper |  
| JavaScript | `javascript/foundry-local-whisper.mjs` | Преобразуване на глас с Whisper |  

> **Забележка:** Този лабораторен модул използва **Foundry Local SDK** за програмно изтегляне и зареждане на Whisper модела, след което изпраща аудио към локалната OpenAI-съвместима крайна точка за транскрипция. Моделът Whisper (`whisper`) е в каталога на Foundry Local и работи изцяло на устройството - не са необходими облачни API ключове или достъп до мрежата.

---

### Част 10: Използване на персонализирани или Hugging Face модели  

**Лабораторен наръчник:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)  

- Компилиране на Hugging Face модели до оптимизиран ONNX формат чрез създателя на модели ONNX Runtime GenAI  
- Хардуерно специфично компилиране (CPU, NVIDIA GPU, DirectML, WebGPU) и квантизация (int4, fp16, bf16)  
- Създаване на конфигурационни файлове за чат шаблони за Foundry Local  
- Добавяне на компилирани модели към кеша на Foundry Local  
- Стартиране на персонализирани модели чрез CLI, REST API и OpenAI SDK  
- Референтен пример: компилиране на Qwen/Qwen3-0.6B от край до край  

---

### Част 11: Извикване на инструменти с локални модели  

**Лабораторен наръчник:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)  

- Позволяване на локални модели да извикват външни функции (извикване на инструменти/функции)  
- Определяне на схеми за инструменти с формата за извикване на функции на OpenAI  
- Управление на многоходова комуникация за извикване на инструменти  
- Изпълнение на извиквания на инструменти локално и връщане на резултати към модела  
- Избор на подходящ модел за сценарии с извикване на инструменти (Qwen 2.5, Phi-4-mini)  
- Използване на вградения `ChatClient` на SDK за извикване на инструменти (JavaScript)  

**Примери за код:**  

| Език | Файл | Описание |  
|----------|------|-------------|  
| Python | `python/foundry-local-tool-calling.py` | Извикване на инструменти за прогноза за времето и население |  
| C# | `csharp/ToolCalling.cs` | Извикване на инструменти с .NET |  
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Извикване на инструменти чрез ChatClient |  

---

### Част 12: Изграждане на уеб потребителски интерфейс за Zava Creative Writer  

**Лабораторен наръчник:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)  

- Добавяне на браузър-базиран фронтенд към Zava Creative Writer  
- Обслужване на споделения UI от Python (FastAPI), JavaScript (Node.js HTTP) и C# (ASP.NET Core)  
- Консумиране на поточен NDJSON в браузъра с Fetch API и ReadableStream  
- Значки за статус на живи агенти и поток на текст в реално време на статии  

**Код (споделен UI):**  

| Файл | Описание |  
|------|-------------|  
| `zava-creative-writer-local/ui/index.html` | Подредба на страница |  
| `zava-creative-writer-local/ui/style.css` | Стилове |  
| `zava-creative-writer-local/ui/app.js` | Логика за четец на потоци и обновяване на DOM |  

**Допълнения към бекенда:**  

| Език | Файл | Описание |  
|----------|------|-------------|  
| Python | `zava-creative-writer-local/src/api/main.py` | Обновен за обслужване на статичен UI |  
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Нов HTTP сървър обгръщащ оркестратора |  
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Нов ASP.NET Core минимален API проект |  

---

### Част 13: Завършване на работилницата  

**Лабораторен наръчник:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)  

- Обобщение на всичко, което сте изградили през 12-те части  
- Допълнителни идеи за разширяване на вашите приложения  
- Връзки към ресурси и документация  

---

## Структура на проекта  

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

| Ресурс | Връзка |  
|----------|------|  
| Уебсайт на Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |  
| Каталог с модели | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |  
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |  
| Ръководство за начинаещи | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |  
| Референция на Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |  
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |  
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |  
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |  

---

## Лиценз  

Тези материали за работилницата се предоставят с образователна цел.  

---

**Приятно програмиране! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Отказ от отговорност**:  
Този документ е преведен с помощта на AI преводаческа услуга [Co-op Translator](https://github.com/Azure/co-op-translator). Въпреки че се стремим към точност, моля имайте предвид, че автоматичните преводи могат да съдържат грешки или неточности. Оригиналният документ на неговия език трябва да се счита за авторитетен източник. За критична информация се препоръчва професионален човешки превод. Ние не носим отговорност за каквито и да е разбирателства или погрешни тълкувания, произтичащи от използването на този превод.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->