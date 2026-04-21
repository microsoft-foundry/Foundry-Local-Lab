# Інструкції для агента кодування

Цей файл надає контекст для AI агентів кодування (GitHub Copilot, Copilot Workspace, Codex тощо), які працюють у цьому репозиторії.

## Огляд проєкту

Це **практичний воркшоп** зі створення AI додатків з [Foundry Local](https://foundrylocal.ai) — легковажним середовищем виконання, яке завантажує, керує та обслуговує мовні моделі повністю на пристрої через сумісний з OpenAI API. Воркшоп включає покрокові керівництва по лабораторіях і код, що запускається, на Python, JavaScript і C#.

## Структура репозиторію

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## Деталі мов і фреймворків

### Python
- **Розташування:** `python/`, `zava-creative-writer-local/src/api/`
- **Залежності:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Ключові пакети:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Мінімальна версія:** Python 3.9+
- **Запуск:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Розташування:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Залежності:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Ключові пакети:** `foundry-local-sdk`, `openai`
- **Система модулів:** ES модулі (`.mjs` файли, `"type": "module"`)
- **Мінімальна версія:** Node.js 18+
- **Запуск:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Розташування:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Файли проєкту:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Ключові пакети:** `Microsoft.AI.Foundry.Local` (не-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — надмножина з QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Ціль:** .NET 9.0 (умовний TFM: `net9.0-windows10.0.26100` для Windows, `net9.0` для інших)
- **Запуск:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Конвенції кодування

### Загальні
- Всі зразки коду — це **автономні приклади в одному файлі** — без спільних утиліт або абстракцій.
- Кожен зразок запускається окремо після встановлення власних залежностей.
- API ключі завжди встановлені в `"foundry-local"` — Foundry Local використовує це як заповнювач.
- Базові URL використовують `http://localhost:<port>/v1` — порт динамічний і визначається під час виконання через SDK (`manager.urls[0]` у JS, `manager.endpoint` у Python).
- Foundry Local SDK керує запуском сервісу та виявленням кінцевої точки; віддавайте перевагу паттернам SDK над жорстко закодованими портами.

### Python
- Використовуйте SDK `openai` з `OpenAI(base_url=..., api_key="not-required")`.
- Використовуйте `FoundryLocalManager()` зі `foundry_local` для керування життєвим циклом сервісу через SDK.
- Потік: ітеруйте об’єкт `stream` через `for chunk in stream:`.
- Без анотацій типів у прикладах (щоб приклади були стислими для учнів воркшопу).

### JavaScript
- Синтаксис ES модуля: `import ... from "..."`.
- Використовуйте `OpenAI` з `"openai"` і `FoundryLocalManager` з `"foundry-local-sdk"`.
- Паттерн ініціалізації SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Потік: `for await (const chunk of stream)`.
- Широко використовується top-level `await`.

### C#
- Включено Nullable, implicit usings, .NET 9.
- Для керування життєвим циклом використовують `FoundryLocalManager.StartServiceAsync()`.
- Потік: `CompleteChatStreaming()` з `foreach (var update in completionUpdates)`.
- Головний `csharp/Program.cs` — CLI маршрутизатор, якій викликає статичні методи `RunAsync()`.

### Виклик інструментів
- Лише певні моделі підтримують виклик інструментів: сімейство **Qwen 2.5** (`qwen2.5-*`) і **Phi-4-mini** (`phi-4-mini`).
- Схеми інструментів відповідають JSON формату OpenAI для виклику функцій (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Розмова має багатократний паттерн: користувач → асистент (tool_calls) → інструмент (результати) → асистент (кінцева відповідь).
- `tool_call_id` у повідомленнях з результатом інструменту має співпадати з `id` виклику інструменту моделі.
- Python використовує OpenAI SDK напряму; JavaScript — власний `ChatClient` SDK (`model.createChatClient()`); C# — OpenAI SDK із `ChatTool.CreateFunctionTool()`.

### ChatClient (нативний клієнт SDK)
- JavaScript: `model.createChatClient()` повертає `ChatClient` з методами `completeChat(messages, tools?)` та `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` повертає стандартний `ChatClient`, який можна використовувати без імпорту пакету OpenAI NuGet.
- Python не має рідного ChatClient — використовуйте OpenAI SDK з `manager.endpoint` та `manager.api_key`.
- **Важливо:** JavaScript `completeStreamingChat` має **callback паттерн**, а не асинхронну ітерацію.

### Моделі міркувань
- `phi-4-mini-reasoning` обгортає власне мислення в теги `<think>...</think>` перед остаточною відповіддю.
- При потребі розбирайте теги, щоб відокремити логіку від відповіді.

## Керівництва лабораторій

Файли лаб у `labs/` у форматі Markdown. Вони мають послідовну структуру:
- Логотип у шапці
- Назва і мета у виділенні
- Огляд, навчальні цілі, передумови
- Розділи з поясненнями концептів та діаграмами
- Нумеровані вправи з блоками коду та очікуваним виводом
- Підсумкова таблиця, ключові висновки, подальше читання
- Навігаційне посилання до наступної частини

Під час редагування лабораторного контенту:
- Зберігайте поточний стиль форматування Markdown та ієрархію розділів.
- Вказуйте мову для блоків коду (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Надавайте варіанти команд для bash і PowerShell там, де це важливо для ОС.
- Використовуйте стилі викликів `> **Note:**`, `> **Tip:**`, `> **Troubleshooting:**`.
- Таблиці оформлюються у форматі з горизонтальними лініями і `| Header | Header |` піп-форматуванням.

## Команди збірки та тестування

| Дія | Команда |
|--------|---------|
| **Python зразки** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS зразки** | `cd javascript && npm install && node <script>.mjs` |
| **C# зразки** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (веб)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (веб)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Генерація діаграм** | `npx mmdc -i <input>.mmd -o <output>.svg` (потребує глобальної інсталяції `npm install`) |

## Зовнішні залежності

- **Foundry Local CLI** має бути встановлений на машині розробника (`winget install Microsoft.FoundryLocal` або `brew install foundrylocal`).
- **Foundry Local service** працює локально і відкриває OpenAI-сумісний REST API на динамічному порту.
- Для запуску жоден зразок не потребує хмарних сервісів, API ключів чи підписок Azure.
- Частина 10 (кастомні моделі) додатково потребує `onnxruntime-genai` і завантаження ваг моделей з Hugging Face.

## Файли, які не слід комітити

У `.gitignore` виключено (і більшість уже виключено):
- `.venv/` — віртуальні середовища Python
- `node_modules/` — npm залежності
- `models/` — скомпільовані ONNX моделі (великі бінарні файли, генеруються у Частині 10)
- `cache_dir/` — кеш завантаження моделей Hugging Face
- `.olive-cache/` — робочий каталог Microsoft Olive
- `samples/audio/*.wav` — згенеровані аудіо зразки (перегенеруються через `python samples/audio/generate_samples.py`)
- Стандартні артефакти збірки Python (`__pycache__/`, `*.egg-info/`, `dist/` тощо)

## Ліцензія

MIT — див. `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Відмова від відповідальності**:  
Цей документ було перекладено за допомогою сервісу штучного інтелекту [Co-op Translator](https://github.com/Azure/co-op-translator). Хоча ми прагнемо до точності, зверніть увагу, що автоматизовані переклади можуть містити помилки або неточності. Оригінальний документ рідною мовою слід вважати авторитетним джерелом. Для критичної інформації рекомендується звертатися до професійного людського перекладу. Ми не несемо відповідальності за будь-які непорозуміння чи неправильні тлумачення, що виникли внаслідок використання цього перекладу.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->