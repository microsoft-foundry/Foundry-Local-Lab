<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Мастерская Foundry Local — Создание AI-приложений на устройстве

Практическая мастерская по запуску языковых моделей на вашем собственном устройстве и созданию интеллектуальных приложений с помощью [Foundry Local](https://foundrylocal.ai) и [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Что такое Foundry Local?** Foundry Local — это легковесная среда выполнения, которая позволяет загружать, управлять и обслуживать языковые модели полностью на вашем оборудовании. Она предоставляет **совместимый с OpenAI API**, поэтому любой инструмент или SDK, поддерживающий OpenAI, может подключаться — облачный аккаунт не требуется.

---

## Цели обучения

К концу этой мастерской вы сможете:

| # | Цель |
|---|-------|
| 1 | Установить Foundry Local и управлять моделями через CLI |
| 2 | Освоить API Foundry Local SDK для программного управления моделями |
| 3 | Подключиться к локальному серверу вывода с помощью SDK для Python, JavaScript и C# |
| 4 | Построить конвейер Retrieval-Augmented Generation (RAG), который основывает ответы на ваших данных |
| 5 | Создавать AI-агентов с постоянными инструкциями и персонажами |
| 6 | Организовать многопользовательские рабочие процессы с обратными связями |
| 7 | Исследовать производственное финальное приложение — Zava Creative Writer |
| 8 | Создавать системы оценки с золотыми наборами данных и оценкой LLM-as-judge |
| 9 | Транскрибировать аудио с Whisper — преобразование речи в текст на устройстве с помощью Foundry Local SDK |
| 10 | Компилировать и запускать кастомные или Hugging Face модели с ONNX Runtime GenAI и Foundry Local |
| 11 | Позволять локальным моделям вызывать внешние функции с помощью шаблона вызова инструментов |
| 12 | Создавать браузерный UI для Zava Creative Writer с потоковой передачей в реальном времени |

---

## Требования

| Требование | Подробности |
|------------|-------------|
| **Оборудование** | Минимум 8 ГБ RAM (рекомендуется 16 ГБ); процессор с поддержкой AVX2 или совместимый GPU |
| **ОС** | Windows 10/11 (x64/ARM), Windows Server 2025 или macOS 13+ |
| **Foundry Local CLI** | Установка через `winget install Microsoft.FoundryLocal` (Windows) или `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). См. [руководство по началу работы](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) для деталей. |
| **Языковое окружение** | **Python 3.9+** и/или **.NET 9.0+** и/или **Node.js 18+** |
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

# 3. Выберите свой языковой трек (см. лабораторную работу Часть 2 для полной настройки)
```

| Язык | Быстрый старт |
|------|---------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Части мастерской

### Часть 1: Начало работы с Foundry Local

**Руководство лаборатории:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Что такое Foundry Local и как он работает
- Установка CLI в Windows и macOS
- Исследование моделей — список, загрузка, запуск
- Понимание псевдонимов моделей и динамических портов

---

### Часть 2: Глубокое погружение в Foundry Local SDK

**Руководство лаборатории:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Почему для разработки приложений лучше использовать SDK, а не CLI
- Полная документация API SDK для Python, JavaScript и C#
- Управление сервисом, просмотр каталога, жизненный цикл моделей (загрузка, выгрузка, удаление)
- Шаблоны быстрого старта: конструктор Python, `init()` в JavaScript, `CreateAsync()` в C#
- Метаданные `FoundryModelInfo`, псевдонимы и выбор оптимальной модели под оборудование

---

### Часть 3: SDK и API

**Руководство лаборатории:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Подключение к Foundry Local из Python, JavaScript и C#
- Использование Foundry Local SDK для управления сервисом программно
- Потоковые чат-завершения через совместимый с OpenAI API
- Референс методов SDK для каждого языка

**Примеры кода:**

| Язык | Файл | Описание |
|-------|------|----------|
| Python | `python/foundry-local.py` | Базовый потоковый чат |
| C# | `csharp/BasicChat.cs` | Потоковый чат в .NET |
| JavaScript | `javascript/foundry-local.mjs` | Потоковый чат на Node.js |

---

### Часть 4: Retrieval-Augmented Generation (RAG)

**Руководство лаборатории:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Что такое RAG и почему это важно
- Создание базы знаний в памяти
- Поиск перекрывающихся ключевых слов с оценкой
- Составление основополагающих системных подсказок
- Запуск полного конвейера RAG на устройстве

**Примеры кода:**

| Язык | Файл |
|-------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Часть 5: Создание AI-агентов

**Руководство лаборатории:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Что такое AI-агент (в отличие от прямого вызова LLM)
- Паттерн `ChatAgent` и Microsoft Agent Framework
- Системные инструкции, персонажи и многошаговые диалоги
- Структурированный вывод (JSON) от агентов

**Примеры кода:**

| Язык | Файл | Описание |
|-------|------|----------|
| Python | `python/foundry-local-with-agf.py` | Один агент с Agent Framework |
| C# | `csharp/SingleAgent.cs` | Один агент (паттерн ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Один агент (паттерн ChatAgent) |

---

### Часть 6: Рабочие процессы с несколькими агентами

**Руководство лаборатории:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Многопользовательские конвейеры: Исследователь → Писатель → Редактор
- Последовательная оркестрация и циклы обратной связи
- Общая конфигурация и структурированные передачи
- Проектирование собственного многопользовательского рабочего процесса

**Примеры кода:**

| Язык | Файл | Описание |
|-------|------|----------|
| Python | `python/foundry-local-multi-agent.py` | Конвейер из трёх агентов |
| C# | `csharp/MultiAgent.cs` | Конвейер из трёх агентов |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Конвейер из трёх агентов |

---

### Часть 7: Zava Creative Writer — итоговое приложение

**Руководство лаборатории:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Производственное многоагентное приложение с 4 специализированными агентами
- Последовательный конвейер с контрольными циклами по оценке
- Потоковый вывод, поиск по каталогу продуктов, структурированные JSON-передачи
- Полная реализация на Python (FastAPI), JavaScript (Node.js CLI) и C# (.NET консоль)

**Примеры кода:**

| Язык | Каталог | Описание |
|-------|----------|----------|
| Python | `zava-creative-writer-local/src/api/` | Веб-сервис FastAPI с оркестратором |
| JavaScript | `zava-creative-writer-local/src/javascript/` | CLI-приложение Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Консольное приложение .NET 9 |

---

### Часть 8: Разработка, ориентированная на оценку

**Руководство лаборатории:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Создание систематической системы оценки для AI-агентов с использованием золотых наборов данных
- Правила проверки (длина, охват ключевых слов, запрещённые термины) + оценка LLM-as-judge
- Параллельное сравнение вариантов подсказок с агрегированными таблицами оценок
- Расширяет паттерн Zava Editor из Части 7 в офлайн-тестовую систему
- Треки для Python, JavaScript и C#

**Примеры кода:**

| Язык | Файл | Описание |
|-------|------|----------|
| Python | `python/foundry-local-eval.py` | Система оценки |
| C# | `csharp/AgentEvaluation.cs` | Система оценки |
| JavaScript | `javascript/foundry-local-eval.mjs` | Система оценки |

---

### Часть 9: Голосовая транскрипция с Whisper

**Руководство лаборатории:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Транскрипция речи в текст с использованием OpenAI Whisper, работающего локально
- Конфиденциальная обработка аудио — звук не покидает устройство
- Треки на Python, JavaScript и C# с `client.audio.transcriptions.create()` (Python/JS) и `AudioClient.TranscribeAudioAsync()` (C#)
- Включает тематические аудиофайлы Zava для практики

**Примеры кода:**

| Язык | Файл | Описание |
|-------|------|----------|
| Python | `python/foundry-local-whisper.py` | Транскрипция голоса Whisper |
| C# | `csharp/WhisperTranscription.cs` | Транскрипция голоса Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Транскрипция голоса Whisper |

> **Примечание:** В этой лаборатории используется **Foundry Local SDK** для программной загрузки и загрузки модели Whisper, затем аудио отправляется на локальную совместимую с OpenAI конечную точку для транскрипции. Модель Whisper (`whisper`) указана в каталоге Foundry Local и работает полностью на устройстве — ключи облачного API или сетевой доступ не требуются.

---

### Часть 10: Использование кастомных или Hugging Face моделей

**Руководство лаборатории:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Компиляция моделей Hugging Face в оптимизированный формат ONNX с помощью ONNX Runtime GenAI model builder
- Аппаратно-специфичная компиляция (CPU, GPU NVIDIA, DirectML, WebGPU) и квантование (int4, fp16, bf16)
- Создание конфигурационных файлов шаблонов чата для Foundry Local
- Добавление скомпилированных моделей в кеш Foundry Local
- Запуск кастомных моделей через CLI, REST API и OpenAI SDK
- Пример из практики: полная компиляция Qwen/Qwen3-0.6B

---

### Часть 11: Вызов инструментов с локальными моделями

**Руководство лаборатории:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Позволять локальным моделям вызывать внешние функции (вызов инструментов/функций)
- Определять схемы инструментов с использованием формата вызовов функций OpenAI
- Обработка многоступенчатого диалогового потока вызова инструментов
- Выполнение вызовов инструментов локально и возврат результатов модели
- Выбор оптимальной модели для сценариев вызова инструментов (Qwen 2.5, Phi-4-mini)
- Использование нативного `ChatClient` SDK для вызова инструментов (JavaScript)

**Примеры кода:**

| Язык | Файл | Описание |
|-------|------|----------|
| Python | `python/foundry-local-tool-calling.py` | Вызов инструментов с погодой и населением |
| C# | `csharp/ToolCalling.cs` | Вызов инструментов с .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Вызов инструментов с ChatClient |

---

### Часть 12: Создание веб-UI для Zava Creative Writer

**Руководство лаборатории:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Добавление браузерного интерфейса для Zava Creative Writer
- Обслуживание общего UI с Python (FastAPI), JavaScript (HTTP-сервер Node.js) и C# (ASP.NET Core)
- Потребление потоковых NDJSON в браузере с помощью Fetch API и ReadableStream
- Значки статуса агента в реальном времени и потоковая передача текста статьи

**Код (общий UI):**

| Файл | Описание |
|-------|----------|
| `zava-creative-writer-local/ui/index.html` | Макет страницы |
| `zava-creative-writer-local/ui/style.css` | Стилизация |
| `zava-creative-writer-local/ui/app.js` | Логика чтения потока и обновления DOM |

**Добавления в backend:**

| Язык | Файл | Описание |
|-------|------|----------|
| Python | `zava-creative-writer-local/src/api/main.py` | Обновлено для обслуживания статического UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Новый HTTP-сервер-обёртка оркестратора |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Новый минимальный API-проект ASP.NET Core |

---

### Часть 13: Мастерская завершена
**Руководство по лабораторной работе:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Краткое резюме всего, что вы создали за все 12 частей
- Идеи для дальнейшего расширения ваших приложений
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

| Ресурс | Ссылка |
|----------|------|
| Веб-сайт Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Каталог моделей | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local на GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Руководство по началу работы | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Справочник по Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Лицензия

Материалы этой лабораторной работы предоставляются в образовательных целях.

---

**Удачного строительства! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Отказ от ответственности**:  
Этот документ был переведен с использованием сервиса AI-перевода [Co-op Translator](https://github.com/Azure/co-op-translator). Хотя мы стремимся к точности, пожалуйста, имейте в виду, что автоматические переводы могут содержать ошибки или неточности. Оригинальный документ на родном языке должен рассматриваться как авторитетный источник. Для критически важной информации рекомендуется профессиональный перевод человеком. Мы не несем ответственности за любые недоразумения или неправильные толкования, возникающие при использовании данного перевода.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->