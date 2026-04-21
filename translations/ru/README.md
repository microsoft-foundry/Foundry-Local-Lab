<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Мастерская Foundry Local — создание AI-приложений на устройстве

Практическая мастерская по запуску языковых моделей на собственном компьютере и созданию интеллектуальных приложений с помощью [Foundry Local](https://foundrylocal.ai) и [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Что такое Foundry Local?** Foundry Local — это легковесная среда выполнения, которая позволяет скачивать, управлять и предоставлять языковые модели полностью на вашем оборудовании. Она предоставляет **совместимый с OpenAI API**, так что любое средство или SDK, поддерживающее OpenAI, может подключаться — облачный аккаунт не требуется.

### 🌐 Поддержка нескольких языков

#### Поддерживается через GitHub Action (автоматически и всегда актуально)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Арабский](../ar/README.md) | [Бенгальский](../bn/README.md) | [Болгарский](../bg/README.md) | [Бирманский (Мьянма)](../my/README.md) | [Китайский (упрощённый)](../zh-CN/README.md) | [Китайский (традиционный, Гонконг)](../zh-HK/README.md) | [Китайский (традиционный, Макао)](../zh-MO/README.md) | [Китайский (традиционный, Тайвань)](../zh-TW/README.md) | [Хорватский](../hr/README.md) | [Чешский](../cs/README.md) | [Датский](../da/README.md) | [Нидерландский](../nl/README.md) | [Эстонский](../et/README.md) | [Финский](../fi/README.md) | [Французский](../fr/README.md) | [Немецкий](../de/README.md) | [Греческий](../el/README.md) | [Иврит](../he/README.md) | [Хинди](../hi/README.md) | [Венгерский](../hu/README.md) | [Индонезийский](../id/README.md) | [Итальянский](../it/README.md) | [Японский](../ja/README.md) | [Каннада](../kn/README.md) | [Кхмерский](../km/README.md) | [Корейский](../ko/README.md) | [Литовский](../lt/README.md) | [Малайский](../ms/README.md) | [Малаялам](../ml/README.md) | [Маратхи](../mr/README.md) | [Непальский](../ne/README.md) | [Нигерийский пиджин](../pcm/README.md) | [Норвежский](../no/README.md) | [Персидский (фарси)](../fa/README.md) | [Польский](../pl/README.md) | [Португальский (Бразилия)](../pt-BR/README.md) | [Португальский (Португалия)](../pt-PT/README.md) | [Пенджаби (гурмукхи)](../pa/README.md) | [Румынский](../ro/README.md) | [Русский](./README.md) | [Сербский (кириллица)](../sr/README.md) | [Словацкий](../sk/README.md) | [Словенский](../sl/README.md) | [Испанский](../es/README.md) | [Суахили](../sw/README.md) | [Шведский](../sv/README.md) | [Тагальский (филиппинский)](../tl/README.md) | [Тамильский](../ta/README.md) | [Телугу](../te/README.md) | [Тайский](../th/README.md) | [Турецкий](../tr/README.md) | [Украинский](../uk/README.md) | [Урду](../ur/README.md) | [Вьетнамский](../vi/README.md)

> **Предпочитаете клонировать локально?**
>
> Этот репозиторий включает более 50 переводов, что существенно увеличивает размер скачивания. Чтобы клонировать без переводов, используйте спарс-чекаут:
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
> Это даст вам всё необходимое для прохождения курса с гораздо более быстрой загрузкой.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Цели обучения

По окончании мастерской вы сможете:

| # | Цель |
|---|-------|
| 1 | Установить Foundry Local и управлять моделями с помощью CLI |
| 2 | Овладеть API SDK Foundry Local для программного управления моделями |
| 3 | Подключаться к локальному серверу вывода через SDK для Python, JavaScript и C# |
| 4 | Построить пайплайн Retrieval-Augmented Generation (RAG), который основывает ответы на ваших данных |
| 5 | Создавать AI-агентов с постоянными инструкциями и персонажами |
| 6 | Оркестрировать многоагентные рабочие процессы с обратными связями |
| 7 | Изучить производственное прикладное приложение — Zava Creative Writer |
| 8 | Создавать фреймворки для оценки с золотыми наборами данных и оцениванием LLM как судьи |
| 9 | Транскрибировать аудио с Whisper — распознавание речи на устройстве с использованием Foundry Local SDK |
| 10 | Компилировать и запускать кастомные или Hugging Face модели с ONNX Runtime GenAI и Foundry Local |
| 11 | Позволять локальным моделям вызывать внешние функции с помощью паттерна tool-calling |
| 12 | Создавать браузерный UI для Zava Creative Writer с потоковой трансляцией в реальном времени |

---

## Требования

| Требование | Подробности |
|-------------|-------------|
| **Оборудование** | Минимум 8 ГБ ОЗУ (рекомендуется 16 ГБ); CPU с поддержкой AVX2 или поддерживаемый GPU |
| **ОС** | Windows 10/11 (x64/ARM), Windows Server 2025 или macOS 13+ |
| **Foundry Local CLI** | Установка через `winget install Microsoft.FoundryLocal` (Windows) или `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Подробнее в [гайде по началу работы](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| **Среда выполнения языка** | **Python 3.9+** и/или **.NET 9.0+** и/или **Node.js 18+** |
| **Git** | Для клонирования этого репозитория |

---

## Начало работы

```bash
# 1. Клонируйте репозиторий
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Убедитесь, что Foundry Local установлен
foundry model list              # Список доступных моделей
foundry model run phi-3.5-mini  # Начать интерактивный чат

# 3. Выберите языковую дорожку (см. лабораторную работу Часть 2 для полной настройки)
```

| Язык | Быстрый старт |
|-------|---------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Части мастерской

### Часть 1: Начало работы с Foundry Local

**Руководство по лаборатории:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Что такое Foundry Local и как он работает
- Установка CLI на Windows и macOS
- Изучение моделей — список, загрузка, запуск
- Понимание псевдонимов моделей и динамических портов

---

### Часть 2: Глубокое изучение SDK Foundry Local

**Руководство по лаборатории:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Зачем использовать SDK вместо CLI для разработки приложений
- Полная справка по API SDK для Python, JavaScript и C#
- Управление сервисом, просмотр каталога, жизненный цикл модели (скачать, загрузить, выгрузить)
- Быстрые шаблоны: инициализация Python-конструктора, JavaScript `init()`, C# `CreateAsync()`
- Метаданные `FoundryModelInfo`, псевдонимы и выбор оптимальной модели под оборудование

---

### Часть 3: SDK и API

**Руководство по лаборатории:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Подключение к Foundry Local из Python, JavaScript и C#
- Использование Foundry Local SDK для программного управления сервисом
- Потоковые чат-запросы через совместимый с OpenAI API
- Справочник методов SDK для каждого языка

**Примеры кода:**

| Язык | Файл | Описание |
|-------|-------|----------|
| Python | `python/foundry-local.py` | Базовый потоковый чат |
| C# | `csharp/BasicChat.cs` | Потоковый чат на .NET |
| JavaScript | `javascript/foundry-local.mjs` | Потоковый чат на Node.js |

---

### Часть 4: Retrieval-Augmented Generation (RAG)

**Руководство по лаборатории:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Что такое RAG и почему это важно
- Создание внутренней базы знаний в памяти
- Поиск по ключевым словам с подсчётом совпадений
- Составление основанных на данных системных запросов
- Запуск полного RAG-пайплайна на устройстве

**Примеры кода:**

| Язык | Файл |
|-------|-------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Часть 5: Создание AI-агентов

**Руководство по лаборатории:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Что такое AI-агент (в отличие от прямого вызова LLM)
- Паттерн `ChatAgent` и Microsoft Agent Framework
- Системные инструкции, персонажи и многошаговые диалоги
- Структурированный вывод (JSON) от агентов

**Примеры кода:**

| Язык | Файл | Описание |
|-------|-------|----------|
| Python | `python/foundry-local-with-agf.py` | Один агент с Agent Framework |
| C# | `csharp/SingleAgent.cs` | Один агент (паттерн ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Один агент (паттерн ChatAgent) |

---

### Часть 6: Многоагентные рабочие процессы

**Руководство по лаборатории:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Многоагентные пайплайны: исследователь → писатель → редактор
- Последовательная оркестрация и циклы обратной связи
- Общая конфигурация и структурированные передачи между агентами
- Проектирование собственного многоагентного рабочего процесса

**Примеры кода:**

| Язык | Файл | Описание |
|-------|-------|----------|
| Python | `python/foundry-local-multi-agent.py` | Пайплайн с тремя агентами |
| C# | `csharp/MultiAgent.cs` | Пайплайн с тремя агентами |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Пайплайн с тремя агентами |

---

### Часть 7: Zava Creative Writer — итоговое приложение

**Руководство по лаборатории:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Производственное многоагентное приложение с 4 специализированными агентами
- Последовательный пайплайн с петлями обратной связи от оценщика
- Потоковый вывод, поиск по каталогу продуктов, структурированные JSON-передачи
- Полная реализация на Python (FastAPI), JavaScript (Node.js CLI) и C# (.NET консоль)

**Примеры кода:**

| Язык | Каталог | Описание |
|-------|----------|----------|
| Python | `zava-creative-writer-local/src/api/` | Веб-сервис FastAPI с оркестратором |
| JavaScript | `zava-creative-writer-local/src/javascript/` | CLI приложение на Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Консольное приложение .NET 9 |

---

### Часть 8: Разработка на основе оценки

**Руководство по лаборатории:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Создание системного фреймворка оценки AI-агентов с использованием золотых наборов данных
- Правила проверки (длина, покрытие ключевых слов, запрещённые термины) + оценивание LLM как судьи
- Сравнение вариантов подсказок с агрегированными картами оценок
- Расширение паттерна агента Zava Editor из части 7 в офлайн-тесты
- Треки для Python, JavaScript и C#

**Примеры кода:**

| Язык | Файл | Описание |
|-------|-------|----------|
| Python | `python/foundry-local-eval.py` | Фреймворк оценки |
| C# | `csharp/AgentEvaluation.cs` | Фреймворк оценки |
| JavaScript | `javascript/foundry-local-eval.mjs` | Фреймворк оценки |

---

### Часть 9: Распознавание голоса с Whisper

**Руководство по лаборатории:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Распознавание речи с помощью OpenAI Whisper, работающего локально
- Обработка аудио с приоритетом конфиденциальности — аудио никогда не покидает ваше устройство
- Поддержка Python, JavaScript и C# с использованием `client.audio.transcriptions.create()` (Python/JS) и `AudioClient.TranscribeAudioAsync()` (C#)
- Включены образцы аудио файлов в стиле Zava для практических занятий

**Примеры кода:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Распознавание речи Whisper |
| C# | `csharp/WhisperTranscription.cs` | Распознавание речи Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Распознавание речи Whisper |

> **Примечание:** В этой лабораторной работе используется **Foundry Local SDK** для программной загрузки и подгрузки модели Whisper, затем аудио отправляется на локальный совместимый с OpenAI эндпоинт для транскрипции. Модель Whisper (`whisper`) указана в каталоге Foundry Local и полностью выполняется на устройстве — не требуются ключи облачного API или доступ в сеть.

---

### Часть 10: Использование Пользовательских или Hugging Face моделей

**Руководство по лабораторной работе:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Компиляция моделей Hugging Face в оптимизированный формат ONNX с помощью построителя моделей ONNX Runtime GenAI
- Аппаратно-специфичная компиляция (CPU, NVIDIA GPU, DirectML, WebGPU) и квантизация (int4, fp16, bf16)
- Создание конфигурационных файлов шаблонов чата для Foundry Local
- Добавление скомпилированных моделей в кэш Foundry Local
- Запуск пользовательских моделей через CLI, REST API и OpenAI SDK
- Пример для справки: полная компиляция Qwen/Qwen3-0.6B

---

### Часть 11: Вызов инструментов с локальными моделями

**Руководство по лабораторной работе:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Позволяет локальным моделям вызывать внешние функции (tool/function calling)
- Определение схем инструментов с использованием формата вызова функций OpenAI
- Обработка многошагового потока общения с вызовом инструментов
- Локальное выполнение вызовов инструментов и возврат результатов модели
- Выбор подходящей модели для сценариев вызова инструментов (Qwen 2.5, Phi-4-mini)
- Использование нативного `ChatClient` SDK для вызова инструментов (JavaScript)

**Примеры кода:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Вызов инструментов с погодой/населенностью |
| C# | `csharp/ToolCalling.cs` | Вызов инструментов с .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Вызов инструментов с ChatClient |

---

### Часть 12: Создание веб-интерфейса для Zava Creative Writer

**Руководство по лабораторной работе:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Добавление браузерного фронтенда для Zava Creative Writer
- Сервис общего UI на Python (FastAPI), JavaScript (Node.js HTTP) и C# (ASP.NET Core)
- Обработка потокового NDJSON в браузере с использованием Fetch API и ReadableStream
- Значки статуса агента в реальном времени и потоковая передача текста статьи

**Код (общий UI):**

| File | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Макет страницы |
| `zava-creative-writer-local/ui/style.css` | Стили |
| `zava-creative-writer-local/ui/app.js` | Логика чтения потока и обновления DOM |

**Добавления на бэкенде:**

| Language | File | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Обновлен для обслуживания статического UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Новый HTTP-сервер, оборачивающий оркестратор |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Новый минимальный API проект ASP.NET Core |

---

### Часть 13: Завершение мастерской

**Руководство по лабораторной работе:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Итоги всего, что вы создали за все 12 частей
- Дополнительные идеи для расширения ваших приложений
- Ссылки на ресурсы и документацию

---

## Структура проекта

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

## Ресурсы

| Resource | Link |
|----------|------|
| Сайт Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Каталог моделей | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Руководство для начала работы | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Справочник Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Лицензия

Данный материал мастерской предоставляется для образовательных целей.

---

**Успешной работы! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Отказ от ответственности**:
Этот документ был переведен с помощью сервиса машинного перевода [Co-op Translator](https://github.com/Azure/co-op-translator). Несмотря на то, что мы стремимся к точности, пожалуйста, имейте в виду, что автоматический перевод может содержать ошибки или неточности. Оригинальный документ на исходном языке следует считать авторитетным источником. Для важной информации рекомендуется обращаться к профессиональному человеческому переводу. Мы не несем ответственности за любые недоразумения или неправильные толкования, возникшие в результате использования этого перевода.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->