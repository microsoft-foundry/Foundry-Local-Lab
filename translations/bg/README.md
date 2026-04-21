<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Workshop - Създаване на AI приложения на устройството

Практически работен семинар за изпълнение на езикови модели на вашия собствен компютър и създаване на интелигентни приложения с [Foundry Local](https://foundrylocal.ai) и [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Какво е Foundry Local?** Foundry Local е леко изпълнимо средище (runtime), което ви позволява да изтегляте, управлявате и предоставяте езикови модели изцяло на вашия хардуер. Тя предлага **OpenAI-съвместим API**, така че всеки инструмент или SDK, който поддържа OpenAI, може да се свърже - без да е необходим облачен акаунт.

---

## Учебни цели

В края на този семинар ще можете да:

| # | Цел |
|---|-----------|
| 1 | Инсталирате Foundry Local и управлявате модели чрез CLI |
| 2 | Усвоите SDK API на Foundry Local за програмно управление на модели |
| 3 | Свържете се с локалния сървър за заключване с помощта на Python, JavaScript и C# SDK |
| 4 | Създадете Retrieval-Augmented Generation (RAG) pipeline, който основава отговорите върху ваши собствени данни |
| 5 | Създавате AI агенти с постоянни инструкции и персонажи |
| 6 | Оркестрирате многоредови работни процеси с цикли за обратна връзка |
| 7 | Разгледате продукционно приложение - Zava Creative Writer |
| 8 | Изградите оценъчни рамки с „златни“ набори от данни и оценка чрез LLM-as-judge |
| 9 | Транскрибирате аудио с Whisper - реч-към-текст на устройството чрез Foundry Local SDK |
| 10 | Компилирате и стартирате персонализирани или Hugging Face модели с ONNX Runtime GenAI и Foundry Local |
| 11 | Позволите локални модели да извикват външни функции чрез шаблона за(tool-calling) |
| 12 | Създадете браузър-базиран потребителски интерфейс за Zava Creative Writer с поточно предаване в реално време |

---

## Предварителни изисквания

| Изискване | Подробности |
|-------------|---------|
| **Хардуер** | Минимум 8 GB RAM (препоръчително 16 GB); процесор с AVX2 или поддържан GPU |
| **ОС** | Windows 10/11 (x64/ARM), Windows Server 2025 или macOS 13+ |
| **Foundry Local CLI** | Инсталирайте чрез `winget install Microsoft.FoundryLocal` (Windows) или `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Вижте [ръководството за започване](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) за подробности. |
| **Езиково средище (runtime)** | **Python 3.9+** и/или **.NET 9.0+** и/или **Node.js 18+** |
| **Git** | За клониране на това хранилище |

---

## Започване

```bash
# 1. Клонирайте репозитория
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Уверете се, че Foundry Local е инсталиран
foundry model list              # Изброяване на наличните модели
foundry model run phi-3.5-mini  # Стартирайте интерактивен чат

# 3. Изберете вашия езиков път (вижте лаборатория Част 2 за пълна настройка)
```

| Език | Бърз старт |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Части на семинара

### Част 1: Запознаване с Foundry Local

**Ръководство за лаборатория:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Какво е Foundry Local и как работи
- Инсталиране на CLI на Windows и macOS
- Разглеждане на модели - изброяване, изтегляне, стартиране
- Разбиране на псевдоними на модели и динамични портове

---

### Част 2: Дълбок поглед върху Foundry Local SDK

**Ръководство за лаборатория:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Защо да използвате SDK вместо CLI за разработка на приложения
- Пълен справочник на SDK API за Python, JavaScript и C#
- Управление на услуга, разглеждане на каталог, жизнен цикъл на моделите (изтегляне, зареждане, разтоварване)
- Шаблони за бърз старт: Python конструктор bootstrap, JavaScript `init()`, C# `CreateAsync()`
- Метаданни `FoundryModelInfo`, псевдоними и подбор на оптимални модели за хардуера

---

### Част 3: SDK-та и API-та

**Ръководство за лаборатория:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Свързване с Foundry Local от Python, JavaScript и C#
- Използване на Foundry Local SDK за програмно управление на услугата
- Поточно излъчване на чат завършвания през OpenAI-съвместим API
- Справочник с методи на SDK за всеки език

**Примери с код:**

| Език | Файл | Описание |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Основен поток за чат |
| C# | `csharp/BasicChat.cs` | Поточно предаване на чат с .NET |
| JavaScript | `javascript/foundry-local.mjs` | Поточно предаване на чат с Node.js |

---

### Част 4: Retrieval-Augmented Generation (RAG)

**Ръководство за лаборатория:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Какво представлява RAG и защо е важно
- Създаване на база знания в паметта
- Извличане чрез припокриване на ключови думи с оценяване
- Подреждане на системни заявки с основа
- Стартиране на цялостен RAG pipeline на устройството

**Примери с код:**

| Език | Файл |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Част 5: Създаване на AI агенти

**Ръководство за лаборатория:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Какво е AI агент (срещу директно извикване на LLM)
- Шаблонът `ChatAgent` и Microsoft Agent Framework
- Системни инструкции, персонажи и многократни разговори
- Структуриран изход (JSON) от агентите

**Примери с код:**

| Език | Файл | Описание |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Един агент с Agent Framework |
| C# | `csharp/SingleAgent.cs` | Един агент (шаблон ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Един агент (шаблон ChatAgent) |

---

### Част 6: Многоредови работни процеси с агенти

**Ръководство за лаборатория:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Многоредови pipeline-и: Изследовател → Писател → Редактор
- Последователна оркестрация и цикли за обратна връзка
- Споделена конфигурация и структурирани предавания
- Проектиране на собствен многоредов работен процес

**Примери с код:**

| Език | Файл | Описание |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Пайплайн с три агента |
| C# | `csharp/MultiAgent.cs` | Пайплайн с три агента |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Пайплайн с три агента |

---

### Част 7: Zava Creative Writer - Финално приложение

**Ръководство за лаборатория:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Продукционно приложение с 4 специализирани агенти
- Последователен pipeline с цикли за обратна връзка, водени от оценител
- Поточно предаване на изход, търсене в продуктов каталог, структурирани JSON предавания
- Пълна реализация на Python (FastAPI), JavaScript (Node.js CLI) и C# (.NET конзола)

**Примери с код:**

| Език | Папка | Описание |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI уеб услуга с оркестратор |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI приложение |
| C# | `zava-creative-writer-local/src/csharp/` | Конзолно приложение .NET 9 |

---

### Част 8: Разработка, водена от оценка

**Ръководство за лаборатория:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Изграждане на систематична оценъчна рамка за AI агенти с „златни“ набори от данни
- Правилно базирани проверки (дължина, покритие на ключови думи, забранени термини) + оценка с LLM-as-judge
- Сравнение на варианти на заявки със съвкупни карти на резултатите
- Разширява шаблона за Zava Editor агента от Част 7 в офлайн тестов пакет
- Тракове за Python, JavaScript и C#

**Примери с код:**

| Език | Файл | Описание |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Оценъчна рамка |
| C# | `csharp/AgentEvaluation.cs` | Оценъчна рамка |
| JavaScript | `javascript/foundry-local-eval.mjs` | Оценъчна рамка |

---

### Част 9: Транскрипция на глас с Whisper

**Ръководство за лаборатория:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Превод на реч в текст с OpenAI Whisper, работещ локално
- Защита на личните данни при аудио обработка - аудиото никога не напуска вашето устройство
- Python, JavaScript и C# трасета с `client.audio.transcriptions.create()` (Python/JS) и `AudioClient.TranscribeAudioAsync()` (C#)
- Включва примерни аудио файлове с тема Zava за практическо упражнение

**Примери с код:**

| Език | Файл | Описание |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Транскрипция на глас с Whisper |
| C# | `csharp/WhisperTranscription.cs` | Транскрипция на глас с Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Транскрипция на глас с Whisper |

> **Забележка:** Тази лаборатория използва **Foundry Local SDK** за програмно изтегляне и зареждане на Whisper модела, след което изпраща аудио към локалната OpenAI-съвместима крайна точка за транскрипция. Моделът Whisper (`whisper`) е изброен в каталога на Foundry Local и работи изцяло на устройството - без нужда от ключове за облачен API или достъп до мрежата.

---

### Част 10: Използване на персонализирани или Hugging Face модели

**Ръководство за лаборатория:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Компилиране на Hugging Face модели в оптимизиран ONNX формат с помощта на ONNX Runtime GenAI модел билдър
- Хардуерно-специфично компилиране (CPU, NVIDIA GPU, DirectML, WebGPU) и квантуване (int4, fp16, bf16)
- Създаване на конфигурационни файлове за чат шаблони за Foundry Local
- Добавяне на компилирани модели в кеша на Foundry Local
- Стартиране на персонализирани модели чрез CLI, REST API и OpenAI SDK
- Пример за справка: крайно-до-край компилиране на Qwen/Qwen3-0.6B

---

### Част 11: Извикване на функции с локални модели

**Ръководство за лаборатория:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Позволяване на локални модели да извикват външни функции (tool/function calling)
- Дефиниране на схеми за инструменти чрез формата за извикване на функции на OpenAI
- Обработка на многократен разговорен поток за извикване на инструменти
- Изпълнение на инструменти локално и връщане на резултати към модела
- Избор на правилен модел за сценарии с извикване на инструменти (Qwen 2.5, Phi-4-mini)
- Използване на нативния `ChatClient` от SDK за извикване на инструменти (JavaScript)

**Примери с код:**

| Език | Файл | Описание |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Извикване на инструменти с метеорологични и демографски данни |
| C# | `csharp/ToolCalling.cs` | Извикване на инструменти с .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Извикване на инструменти с ChatClient |

---

### Част 12: Създаване на уеб интерфейс за Zava Creative Writer

**Ръководство за лаборатория:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Добавяне на браузър-базиран фронтенд към Zava Creative Writer
- Предоставяне на споделен UI от Python (FastAPI), JavaScript (Node.js HTTP) и C# (ASP.NET Core)
- Консумиране на поточно NDJSON в браузъра с Fetch API и ReadableStream
- Живи значки за статус на агент и стрийминг на текста на статии в реално време

**Код (споделен UI):**

| Файл | Описание |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Оформление на страницата |
| `zava-creative-writer-local/ui/style.css` | Стилове |
| `zava-creative-writer-local/ui/app.js` | Четец на поток и логика за обновяване на DOM |

**Промени в бекенд:**

| Език | Файл | Описание |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Ъпдейтнат за обслужване на статичен UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Нов HTTP сървър, обвиващ оркестратора |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Нов ASP.NET Core минимален API проект |

---

### Част 13: Семинарът приключи


**Ръководство за лаборатория:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Обобщение на всичко, което сте създали през всички 12 части
- Допълнителни идеи за разширяване на приложенията ви
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
| Уебсайт Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Каталог модели | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Ръководство за започване | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Справочник на Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Лиценз

Този материал за лаборатория е предоставен за образователни цели.

---

**Приятно създаване! 🚀**