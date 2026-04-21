<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Практичний семінар Foundry Local – Створення AI додатків на пристрої

Практичний семінар з запуску мовних моделей на власному пристрої та побудови інтелектуальних додатків із використанням [Foundry Local](https://foundrylocal.ai) та [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Що таке Foundry Local?** Foundry Local — це легковажне середовище виконання, яке дозволяє завантажувати, керувати та обслуговувати мовні моделі повністю на вашому обладнанні. Воно відкриває **API, сумісне з OpenAI**, тому будь-який інструмент або SDK, що підтримує OpenAI, може підключатися – без облікового запису в хмарі.

### 🌐 Підтримка багатьох мов

#### Підтримується через GitHub Action (автоматично та постійно оновлюється)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Арабська](../ar/README.md) | [Бенгальська](../bn/README.md) | [Болгарська](../bg/README.md) | [Бирманська (М’янма)](../my/README.md) | [Китайська (спрощена)](../zh-CN/README.md) | [Китайська (традиційна, Гонконг)](../zh-HK/README.md) | [Китайська (традиційна, Макао)](../zh-MO/README.md) | [Китайська (традиційна, Тайвань)](../zh-TW/README.md) | [Хорватська](../hr/README.md) | [Чеська](../cs/README.md) | [Данська](../da/README.md) | [Нідерландська](../nl/README.md) | [Естонська](../et/README.md) | [Фінська](../fi/README.md) | [Французька](../fr/README.md) | [Німецька](../de/README.md) | [Грецька](../el/README.md) | [Гебрайська](../he/README.md) | [Гінді](../hi/README.md) | [Угорська](../hu/README.md) | [Індонезійська](../id/README.md) | [Італійська](../it/README.md) | [Японська](../ja/README.md) | [Каннада](../kn/README.md) | [Кхмер](../km/README.md) | [Корейська](../ko/README.md) | [Литовська](../lt/README.md) | [Малайська](../ms/README.md) | [Малаялам](../ml/README.md) | [Маратхі](../mr/README.md) | [Непальська](../ne/README.md) | [Нігерійський пиджин](../pcm/README.md) | [Норвезька](../no/README.md) | [Перська (фарсі)](../fa/README.md) | [Польська](../pl/README.md) | [Португальська (Бразилія)](../pt-BR/README.md) | [Португальська (Португалія)](../pt-PT/README.md) | [Пенджабі (гурмухі)](../pa/README.md) | [Румунська](../ro/README.md) | [Російська](../ru/README.md) | [Сербська (кирилиця)](../sr/README.md) | [Словацька](../sk/README.md) | [Словенська](../sl/README.md) | [Іспанська](../es/README.md) | [Свахілі](../sw/README.md) | [Шведська](../sv/README.md) | [Тагальська (філіппінська)](../tl/README.md) | [Тамільська](../ta/README.md) | [Телугу](../te/README.md) | [Тайська](../th/README.md) | [Турецька](../tr/README.md) | [Українська](./README.md) | [Урду](../ur/README.md) | [Вʼєтнамська](../vi/README.md)

> **Віддаєте перевагу клонуванню локально?**
>
> У цьому репозиторії є понад 50 перекладів мовами, що значно збільшує розмір завантаження. Щоб клонувати без перекладів, використайте управління sparse checkout:
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
> Таким чином ви отримаєте все необхідне для проходження курсу значно швидше.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Цілі навчання

Після завершення цього семінару ви зможете:

| # | Ціль |
|---|-------|
| 1 | Встановити Foundry Local і керувати моделями за допомогою CLI |
| 2 | Опанувати SDK API Foundry Local для програмного управління моделями |
| 3 | Підключатися до локального сервера висновків за допомогою SDK для Python, JavaScript і C# |
| 4 | Побудувати pipeline Retrieval-Augmented Generation (RAG), що базується на ваших власних даних |
| 5 | Створювати AI агенти зі збереженими інструкціями та персонажами |
| 6 | Організовувати багатоагентні робочі процеси з циклами зворотного зв’язку |
| 7 | Ознайомитися з capstone додатком для виробництва – Zava Creative Writer |
| 8 | Розробляти оцінювальні фреймворки з золотими датасетами і LLM-оцінюванням |
| 9 | Транскрибувати аудіо за допомогою Whisper – розпізнавання мови на пристрої з використанням Foundry Local SDK |
| 10 | Компілювати та запускати власні або моделі Hugging Face за допомогою ONNX Runtime GenAI і Foundry Local |
| 11 | Надавати локальним моделям можливість виклику зовнішніх функцій за патерном виклику інструментів |
| 12 | Створювати браузерний UI для Zava Creative Writer із потоковою трансляцією в реальному часі |

---

## Вимоги

| Вимога | Деталі |
|---------|---------|
| **Обладнання** | Мінімум 8 ГБ ОЗП (рекомендовано 16 ГБ); CPU з підтримкою AVX2 або підтримуваний GPU |
| **ОС** | Windows 10/11 (x64/ARM), Windows Server 2025 або macOS 13+ |
| **Foundry Local CLI** | Встановлення через `winget install Microsoft.FoundryLocal` (Windows) або `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Деталі див. у [посібнику з початку роботи](https://learn.microsoft.com/en-us/azure/foundry-local/get-started). |
| **Середовище виконання мови** | **Python 3.9+** та/або **.NET 9.0+** та/або **Node.js 18+** |
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

# 3. Виберіть свою мовну гілку (див. Лабораторія Частина 2 для повної настройки)
```

| Мова | Швидкий старт |
|-------|----------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Частини семінару

### Частина 1: Початок роботи з Foundry Local

**Посібник:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Що таке Foundry Local і як він працює
- Встановлення CLI на Windows і macOS
- Огляд моделей – перелік, завантаження, запуск
- Розуміння псевдонімів моделей і динамічних портів

---

### Частина 2: Глибокий занурення в Foundry Local SDK

**Посібник:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Чому SDK кращий за CLI для розробки додатків
- Повний довідник API SDK для Python, JavaScript і C#
- Управління сервісом, перегляд каталогу, життєвий цикл моделі (завантаження, завантаження, розвантаження)
- Патерни швидкого старту: конструктора bootstrap у Python, `init()` в JavaScript, `CreateAsync()` у C#
- Метадані `FoundryModelInfo`, псевдоніми та вибір оптимальних моделей для обладнання

---

### Частина 3: SDK та API

**Посібник:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Підключення до Foundry Local з Python, JavaScript і C#
- Використання Foundry Local SDK для програмного керування сервісом
- Потокове завершення чатів через OpenAI-сумісне API
- Довідник методів SDK для кожної мови

**Приклади коду:**

| Мова | Файл | Опис |
|-------|-------|-------|
| Python | `python/foundry-local.py` | Базовий потоковий чат |
| C# | `csharp/BasicChat.cs` | Потоковий чат з .NET |
| JavaScript | `javascript/foundry-local.mjs` | Потоковий чат з Node.js |

---

### Частина 4: Retrieval-Augmented Generation (RAG)

**Посібник:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Що таке RAG і чому це важливо
- Побудова бази знань у пам’яті
- Пошук за ключовими словами з оцінюванням
- Складання системних підказок із підставою
- Запуск повного RAG pipeline на пристрої

**Приклади коду:**

| Мова | Файл |
|-------|-------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Частина 5: Побудова AI агентів

**Посібник:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Що таке AI агент (на відміну від простого виклику LLM)
- Патерн `ChatAgent` і Microsoft Agent Framework
- Системні інструкції, персонажі, багатократні діалоги
- Структурований вивід (JSON) від агентів

**Приклади коду:**

| Мова | Файл | Опис |
|-------|-------|-------|
| Python | `python/foundry-local-with-agf.py` | Одиночний агент із Agent Framework |
| C# | `csharp/SingleAgent.cs` | Одиночний агент (патерн ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Одиночний агент (патерн ChatAgent) |

---

### Частина 6: Багатоагентні робочі процеси

**Посібник:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Багатоагентні pipeline: Дослідник → Письменник → Редактор
- Послідовна оркестрація і цикли зворотного зв’язку
- Спільна конфігурація і структуровані передачі
- Проєктування власного багатоагентного робочого процесу

**Приклади коду:**

| Мова | Файл | Опис |
|-------|-------|-------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline з трьох агентів |
| C# | `csharp/MultiAgent.cs` | Pipeline з трьох агентів |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline з трьох агентів |

---

### Частина 7: Zava Creative Writer – capstone додаток

**Посібник:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Виробничий мультиагентний додаток із 4 спеціалізованими агентами
- Послідовний pipeline з циклом зворотного зв’язку, що керується оцінювачем
- Потоковий вивід, пошук по каталогу продуктів, структуровані JSON передачі
- Повна реалізація на Python (FastAPI), JavaScript (Node.js CLI) та C# (.NET консоль)

**Приклади коду:**

| Мова | Директорія | Опис |
|-------|------------|------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI веб-сервіс з оркестратором |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI додаток |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 консольний додаток |

---

### Частина 8: Розробка з керівництвом оцінками

**Посібник:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Створення систематичного фреймворку оцінювання AI агентів за золотими датасетами
- Перевірки за правилами (довжина, покриття ключових слів, заборонені терміни) + оцінка LLM як судді
- Порівняння варіантів підказок поряд із агрегованими оцінками
- Розширення патерну агента Zava Editor із Частини 7 у офлайн тестовий набір
- Треки Python, JavaScript та C#

**Приклади коду:**

| Мова | Файл | Опис |
|-------|-------|-------|
| Python | `python/foundry-local-eval.py` | Фреймворк оцінювання |
| C# | `csharp/AgentEvaluation.cs` | Фреймворк оцінювання |
| JavaScript | `javascript/foundry-local-eval.mjs` | Фреймворк оцінювання |

---

### Частина 9: Голосова транскрипція за допомогою Whisper

**Посібник:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Транслітерація мови в текст за допомогою OpenAI Whisper, що запускається локально
- Обробка аудіо з пріоритетом на конфіденційність – аудіо ніколи не покидає ваш пристрій
- Треки Python, JavaScript і C# з `client.audio.transcriptions.create()` (Python/JS) та `AudioClient.TranscribeAudioAsync()` (C#)
- Включає приклади аудіофайлів із темою Zava для практичних занять

**Приклади коду:**

| Мова | Файл | Опис |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Транскрипція голосу Whisper |
| C# | `csharp/WhisperTranscription.cs` | Транскрипція голосу Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Транскрипція голосу Whisper |

> **Примітка:** Ця лабораторна робота використовує **Foundry Local SDK** для програмного завантаження та завантаження моделі Whisper, а потім надсилає аудіо на локальний кінцевий OpenAI-сумісний сервер для транскрипції. Модель Whisper (`whisper`) представлена в каталозі Foundry Local і запускається повністю на пристрої – не потрібні ключі API хмари або доступ до мережі.

---

### Частина 10: Використання власних або Hugging Face моделей

**Керівництво лабораторії:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Компіляція моделей Hugging Face в оптимізований формат ONNX за допомогою ONNX Runtime GenAI model builder
- Апаратно-специфічна компіляція (CPU, NVIDIA GPU, DirectML, WebGPU) та квантизація (int4, fp16, bf16)
- Створення конфігураційних файлів шаблонів чатів для Foundry Local
- Додавання скомпільованих моделей у кеш Foundry Local
- Запуск власних моделей через CLI, REST API та OpenAI SDK
- Приклад: кінцеве компілювання Qwen/Qwen3-0.6B

---

### Частина 11: Виклик інструментів з локальними моделями

**Керівництво лабораторії:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Активуйте виклики зовнішніх функцій локальними моделями (tool/function calling)
- Визначайте схеми інструментів у форматі виклику функцій OpenAI
- Керуйте багатокроковим сценарієм розмови викликів інструментів
- Виконуйте виклики інструментів локально та повертайте результати моделі
- Виберіть правильну модель для сценаріїв виклику інструментів (Qwen 2.5, Phi-4-mini)
- Використовуйте рідний `ChatClient` SDK для виклику інструментів (JavaScript)

**Приклади коду:**

| Мова | Файл | Опис |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Виклики інструментів з погодою/населенням |
| C# | `csharp/ToolCalling.cs` | Виклики інструментів у .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Виклики інструментів з ChatClient |

---

### Частина 12: Побудова веб-інтерфейсу для Zava Creative Writer

**Керівництво лабораторії:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Додайте веб-інтерфейс у браузері для Zava Creative Writer
- Обслуговуйте спільний UI з Python (FastAPI), JavaScript (Node.js HTTP) і C# (ASP.NET Core)
- Споживайте потокові NDJSON у браузері за допомогою Fetch API та ReadableStream
- Статуси живих агентів та потокова передача тексту статей у реальному часі

**Код (спільний UI):**

| Файл | Опис |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Макет сторінки |
| `zava-creative-writer-local/ui/style.css` | Стилі |
| `zava-creative-writer-local/ui/app.js` | Логіка читача потоку та оновлення DOM |

**Додатки бекенду:**

| Мова | Файл | Опис |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Оновлено для обслуговування статичного UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Новий HTTP сервер, що обгортає оркестратор |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Новий мінімальний проект ASP.NET Core API |

---

### Частина 13: Завершення воркшопу

**Керівництво лабораторії:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Підсумок всього, що ви створили за 12 частин
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
| Посібник для початку роботи | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Документація SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Ліцензія

Ці матеріали воркшопу надаються для освітніх цілей.

---

**Бажаємо вдалого створення! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Відмова від відповідальності**:  
Цей документ був перекладений за допомогою сервісу автоматичного перекладу [Co-op Translator](https://github.com/Azure/co-op-translator). Хоча ми прагнемо до точності, будь ласка, майте на увазі, що автоматичні переклади можуть містити помилки або неточності. Оригінальний документ його рідною мовою слід вважати авторитетним джерелом. Для критично важливої інформації рекомендується звертатися до професійного людського перекладу. Ми не несемо відповідальності за будь-які непорозуміння або неправильне тлумачення, що виникли внаслідок використання цього перекладу.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->