<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Майстер-клас Foundry Local — Створення ІІ-додатків на пристрої

Практичний майстер-клас зі запуску мовних моделей на власній машині та створення інтелектуальних додатків з використанням [Foundry Local](https://foundrylocal.ai) і [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Що таке Foundry Local?** Foundry Local — це легковажне середовище виконання, яке дозволяє завантажувати, керувати та запускати мовні моделі повністю на вашому обладнанні. Він надає **API, сумісний з OpenAI**, щоб будь-який інструмент або SDK, який підтримує OpenAI, міг підключатися — жодного хмарного облікового запису не потрібно.

---

## Цілі навчання

Наприкінці цього майстер-класу ви зможете:

| # | Мета |
|---|----------|
| 1 | Встановити Foundry Local і керувати моделями через CLI |
| 2 | Опанувати API SDK Foundry Local для програмного керування моделями |
| 3 | Підключатися до локального inference-сервера за допомогою SDK Python, JavaScript і C# |
| 4 | Створити конвеєр Retrieval-Augmented Generation (RAG), що підкріплює відповіді вашими даними |
| 5 | Створити ІІ-агентів із постійними інструкціями та персонажами |
| 6 | Організувати багатоагентні робочі процеси з петлями зворотного зв’язку |
| 7 | Ознайомитися з виробничим capstone-додатком — Zava Creative Writer |
| 8 | Побудувати системи оцінки з використанням золотих наборів даних і LLM як судді |
| 9 | Транскрибувати аудіо за допомогою Whisper — перетворення мови в текст на пристрої з використанням SDK Foundry Local |
| 10 | Компілювати й запускати кастомні або моделі Hugging Face за допомогою ONNX Runtime GenAI і Foundry Local |
| 11 | Дозволити локальним моделям викликати зовнішні функції за допомогою патерну tool-calling |
| 12 | Створити браузерний UI для Zava Creative Writer з потоковим оновленням у реальному часі |

---

## Необхідні умови

| Вимога | Деталі |
|--------|---------|
| **Обладнання** | Мінімум 8 ГБ ОЗП (рекомендовано 16 ГБ); процесор із підтримкою AVX2 або підтримуваний GPU |
| **ОС** | Windows 10/11 (x64/ARM), Windows Server 2025 або macOS 13+ |
| **Foundry Local CLI** | Встановлення через `winget install Microsoft.FoundryLocal` (Windows) або `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Деталі див. у [керівництві для початку роботи](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Середовище виконання мов** | **Python 3.9+** і/або **.NET 9.0+** і/або **Node.js 18+** |
| **Git** | Для клонування цього репозиторію |

---

## Початок роботи

```bash
# 1. Клонуйте репозиторій
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Перевірте, чи встановлено Foundry Local
foundry model list              # Перелік доступних моделей
foundry model run phi-3.5-mini  # Почніть інтерактивний чат

# 3. Виберіть вашу мовну трасу (див. лабораторію Частина 2 для повної установки)
```

| Мова | Швидкий старт |
|------|---------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Частини майстер-класу

### Частина 1: Початок роботи з Foundry Local

**Керівництво лабораторії:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Що таке Foundry Local і як він працює
- Встановлення CLI на Windows і macOS
- Огляд моделей — перелік, завантаження, запуск
- Розуміння псевдонімів моделей і динамічних портів

---

### Частина 2: Глибокий огляд SDK Foundry Local

**Керівництво лабораторії:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Чому варто використовувати SDK замість CLI для розробки додатків
- Повний посібник API SDK для Python, JavaScript і C#
- Керування сервісом, перегляд каталогу, життєвий цикл моделі (завантаження, активація, деактивація)
- Шаблони швидкого старту: ініціалізація конструктора Python, `init()` у JavaScript, `CreateAsync()` у C#
- Метадані `FoundryModelInfo`, псевдоніми та вибір оптимальної для обладнання моделі

---

### Частина 3: SDK і API

**Керівництво лабораторії:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Підключення до Foundry Local із Python, JavaScript та C#
- Використання SDK Foundry Local для програмного керування сервісом
- Потокове створення чат-відповідей через сумісний з OpenAI API
- Посібник методів SDK для кожної мови

**Приклади коду:**

| Мова | Файл | Опис |
|------|-------|--------|
| Python | `python/foundry-local.py` | Базове потокове чат-спілкування |
| C# | `csharp/BasicChat.cs` | Потокове чат-спілкування з .NET |
| JavaScript | `javascript/foundry-local.mjs` | Потокове чат-спілкування з Node.js |

---

### Частина 4: Retrieval-Augmented Generation (RAG)

**Керівництво лабораторії:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Що таке RAG і чому це важливо
- Створення бази знань у пам’яті
- Пошук з перетином ключових слів із оцінкою
- Формування системних підказок з підкріпленням
- Запуск повного конвеєра RAG на пристрої

**Приклади коду:**

| Мова | Файл |
|------|-------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Частина 5: Створення ІІ-агентів

**Керівництво лабораторії:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Що таке ІІ-агент (на відміну від прямого виклику LLM)
- Патерн `ChatAgent` і Microsoft Agent Framework
- Системні інструкції, персонажі та багатоетапні розмови
- Структурований JSON-вивід від агентів

**Приклади коду:**

| Мова | Файл | Опис |
|------|-------|--------|
| Python | `python/foundry-local-with-agf.py` | Одиночний агент з Agent Framework |
| C# | `csharp/SingleAgent.cs` | Одиночний агент (патерн ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Одиночний агент (патерн ChatAgent) |

---

### Частина 6: Багатоагентні робочі процеси

**Керівництво лабораторії:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Багатоагентні конвеєри: Дослідник → Автор → Редактор
- Послідовна оркестрація й петлі зворотного зв’язку
- Спільні налаштування і структуровані передачі
- Проектування власного багатоагентного робочого процесу

**Приклади коду:**

| Мова | Файл | Опис |
|------|-------|--------|
| Python | `python/foundry-local-multi-agent.py` | Конвеєр із трьох агентів |
| C# | `csharp/MultiAgent.cs` | Конвеєр із трьох агентів |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Конвеєр із трьох агентів |

---

### Частина 7: Zava Creative Writer — підсумковий додаток

**Керівництво лабораторії:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Виробничий багатоагентний додаток із 4 спеціалізованими агентами
- Послідовний конвеєр з петлями зворотного зв’язку, керованими оцінювачем
- Потоковий вивід, пошук у каталозі продуктів, структуровані JSON-передачі
- Повна імплементація на Python (FastAPI), JavaScript (Node.js CLI) та C# (.NET консоль)

**Приклади коду:**

| Мова | Директорія | Опис |
|------|-------------|--------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI веб-сервіс із оркестратором |
| JavaScript | `zava-creative-writer-local/src/javascript/` | CLI-додаток Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Консольний додаток .NET 9 |

---

### Частина 8: Розробка, керована оцінкою

**Керівництво лабораторії:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Побудова системного фреймворку оцінки ІІ-агентів із золотими наборами даних
- Перевірки за правилами (довжина, охоплення ключових слів, заборонені терміни) + оцінювання за допомогою LLM як судді
- Порівняння варіантів підказок бок-о-бок із агрегованими скор-картами
- Розширює патерн агента Zava Editor з Частини 7 в офлайновий тестовий пакет
- Треки Python, JavaScript і C#

**Приклади коду:**

| Мова | Файл | Опис |
|------|-------|--------|
| Python | `python/foundry-local-eval.py` | Система оцінки |
| C# | `csharp/AgentEvaluation.cs` | Система оцінки |
| JavaScript | `javascript/foundry-local-eval.mjs` | Система оцінки |

---

### Частина 9: Транскрипція голосу з Whisper

**Керівництво лабораторії:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Перетворення мови в текст за допомогою OpenAI Whisper, що працює локально
- Конфіденційна обробка аудіо — аудіо не залишає ваш пристрій
- Треки Python, JavaScript і C# із використанням `client.audio.transcriptions.create()` (Python/JS) та `AudioClient.TranscribeAudioAsync()` (C#)
- Включає тематичні зразки аудіо Zava для практики

**Приклади коду:**

| Мова | Файл | Опис |
|------|-------|--------|
| Python | `python/foundry-local-whisper.py` | Транскрипція голосу Whisper |
| C# | `csharp/WhisperTranscription.cs` | Транскрипція голосу Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Транскрипція голосу Whisper |

> **Примітка:** Ця лабораторія використовує **Foundry Local SDK** для програмного завантаження та завантаження моделі Whisper, після чого відправляє аудіо на локальний сумісний з OpenAI endpoint для транскрипції. Модель Whisper (`whisper`) зберігається у каталозі Foundry Local і працює повністю на пристрої — жодних ключів API та доступу до мережі не потрібно.

---

### Частина 10: Використання кастомних або Hugging Face моделей

**Керівництво лабораторії:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Компіляція моделей Hugging Face в оптимізований формат ONNX за допомогою конструктору моделей ONNX Runtime GenAI
- Апаратно-специфічна компіляція (CPU, NVIDIA GPU, DirectML, WebGPU) та квантизація (int4, fp16, bf16)
- Створення конфігураційних файлів шаблонів чатів для Foundry Local
- Додавання скомпільованих моделей у кеш Foundry Local
- Запуск кастомних моделей через CLI, REST API та OpenAI SDK
- Зразок: кінцева компіляція Qwen/Qwen3-0.6B

---

### Частина 11: Виклики інструментів із локальних моделей

**Керівництво лабораторії:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Дозволити локальним моделям викликати зовнішні функції (tool/function calling)
- Визначення схем інструментів за форматом виклику функцій OpenAI
- Обробка багатокрокового діалогу виклику інструментів
- Виконання викликів інструментів локально з поверненням результатів моделі
- Вибір відповідної моделі для сценаріїв виклику інструментів (Qwen 2.5, Phi-4-mini)
- Використання нативного `ChatClient` SDK для виклику інструментів (JavaScript)

**Приклади коду:**

| Мова | Файл | Опис |
|------|-------|--------|
| Python | `python/foundry-local-tool-calling.py` | Виклики інструментів із погодою/населенням |
| C# | `csharp/ToolCalling.cs` | Виклики інструментів із .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Виклики інструментів із ChatClient |

---

### Частина 12: Створення веб-інтерфейсу для Zava Creative Writer

**Керівництво лабораторії:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Додати браузерний front-end для Zava Creative Writer
- Сервер спільного UI на Python (FastAPI), JavaScript (Node.js HTTP) і C# (ASP.NET Core)
- Використання потоку NDJSON у браузері з Fetch API і ReadableStream
- Беджі статусу агентів і потокове відображення тексту статті в режимі реального часу

**Код (спільний UI):**

| Файл | Опис |
|-------|---------|
| `zava-creative-writer-local/ui/index.html` | Макет сторінки |
| `zava-creative-writer-local/ui/style.css` | Стилі |
| `zava-creative-writer-local/ui/app.js` | Логіка читача потоку та оновлення DOM |

**Додатки бекенду:**

| Мова | Файл | Опис |
|-------|-------|-------|
| Python | `zava-creative-writer-local/src/api/main.py` | Оновлено для подачі статичного UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Новий HTTP-сервер, що обгортає оркестратор |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Новий мінімальний проект ASP.NET Core API |

---

### Частина 13: Майстер-клас завершено
**Посібник лабораторії:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Підсумок усього, що ви створили протягом усіх 12 частин
- Подальші ідеї для розширення ваших застосунків
- Посилання на ресурси та документацію

---

## Структура проєкту

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

| Ресурс | Посилання |
|----------|------|
| Вебсайт Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Каталог моделей | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Посібник з початку роботи | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Довідник SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Ліцензія

Цей матеріал семінару призначений для освітніх цілей.

---

**Успішного створення! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Відмова від відповідальності**:
Цей документ був перекладений за допомогою сервісу автоматичного перекладу [Co-op Translator](https://github.com/Azure/co-op-translator). Незважаючи на наші зусилля забезпечити точність, враховуйте, що автоматичні переклади можуть містити помилки або неточності. Оригінальний документ рідною мовою слід вважати авторитетним джерелом. Для критично важливої інформації рекомендується професійний переклад людиною. Ми не несемо відповідальності за будь-які непорозуміння чи неправильні тлумачення, що виникли внаслідок використання цього перекладу.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->