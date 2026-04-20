# Инструкции за програмен агент

Този файл предоставя контекст за AI програмни агенти (GitHub Copilot, Copilot Workspace, Codex и т.н.), работещи в това хранилище.

## Обзор на проекта

Това е **практически семинар** за изграждане на AI приложения с [Foundry Local](https://foundrylocal.ai) — леко изпълнително време, което изтегля, управлява и обслужва езикови модели изцяло на устройството чрез OpenAI-съвместим API. Семинарът включва стъпка по стъпка лабораторни ръководства и изпълними кодови примери на Python, JavaScript и C#.

## Структура на хранилището

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

## Езици и рамки

### Python
- **Локация:** `python/`, `zava-creative-writer-local/src/api/`
- **Зависимости:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Ключови пакети:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Минимална версия:** Python 3.9+
- **Стартиране:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Локация:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Зависимости:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Ключови пакети:** `foundry-local-sdk`, `openai`
- **Модулна система:** ES модули (`.mjs` файлове, `"type": "module"`)
- **Минимална версия:** Node.js 18+
- **Стартиране:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Локация:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Проектни файлове:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Ключови пакети:** `Microsoft.AI.Foundry.Local` (не-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — надмножество с QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Цел:** .NET 9.0 (условен TFM: `net9.0-windows10.0.26100` на Windows, `net9.0` другаде)
- **Стартиране:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Конвенции за кодиране

### Общо
- Всички кодови примери са **самостоятелни еднофайлови примери** — без споделени библиотеки или абстракции.
- Всеки пример се изпълнява независимо след инсталиране на собствените си зависимости.
- API ключовете винаги са зададени на `"foundry-local"` — Foundry Local използва това като заместител.
- Базовите URL адреси използват `http://localhost:<port>/v1` — портът е динамичен и се открива по време на изпълнение чрез SDK (`manager.urls[0]` в JS, `manager.endpoint` в Python).
- Foundry Local SDK отговаря за стартиране на услугата и откриване на крайни точки; предпочитайте SDK модели пред фиксирани портове.

### Python
- Използвайте `openai` SDK с `OpenAI(base_url=..., api_key="not-required")`.
- Използвайте `FoundryLocalManager()` от `foundry_local` за SDK управляван жизнен цикъл на услугата.
- Поточно предаване: итерирайте през обекта `stream` с `for chunk in stream:`.
- Без анотации за типове в примерните файлове (за по-кратки примери за участниците).

### JavaScript
- Синтаксис на ES модули: `import ... from "..."`.
- Използвайте `OpenAI` от `"openai"` и `FoundryLocalManager` от `"foundry-local-sdk"`.
- Патерн за инициализация на SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Поточно предаване: `for await (const chunk of stream)`.
- Използва се top-level `await` навсякъде.

### C#
- Nullable активиран, имплицитни usings, .NET 9.
- Използвайте `FoundryLocalManager.StartServiceAsync()` за SDK управляван жизнен цикъл.
- Поточно предаване: `CompleteChatStreaming()` с `foreach (var update in completionUpdates)`.
- Главният `csharp/Program.cs` е CLI маршрутизатор, който пуска статични методи `RunAsync()`.

### Извикване на инструменти
- Само определени модели поддържат извикване на инструменти: семейство **Qwen 2.5** (`qwen2.5-*`) и **Phi-4-mini** (`phi-4-mini`).
- Схемите за инструменти следват OpenAI JSON формат за извикване на функции (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Разговорът използва многостъпков модел: потребител → асистент (tool_calls) → инструмент (резултати) → асистент (краен отговор).
- `tool_call_id` в съобщенията с резултата от инструмента трябва да съответства на `id` от извикването на инструмента на модела.
- Python използва директно OpenAI SDK; JavaScript използва родния ChatClient от SDK (`model.createChatClient()`); C# използва OpenAI SDK с `ChatTool.CreateFunctionTool()`.

### ChatClient (роден SDK клиент)
- JavaScript: `model.createChatClient()` връща `ChatClient` с методи `completeChat(messages, tools?)` и `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` връща стандартен `ChatClient`, който може да се използва без да се импортира OpenAI NuGet пакетът.
- Python няма роден ChatClient — използвайте OpenAI SDK с `manager.endpoint` и `manager.api_key`.
- **Важно:** JavaScript `completeStreamingChat` използва **callback патерн**, не async итериране.

### Модели за разсъждение
- `phi-4-mini-reasoning` обгражда мисленето си в тагове `<think>...</think>` преди крайния отговор.
- При нужда парсвайте таговете за отделяне на разсъждението от отговора.

## Лабораторни ръководства

Лабораторните файлове са в `labs/` като Markdown. Те следват постоянна структура:
- Лого като заглавно изображение
- Заглавие и цел на задачата
- Обзор, учебни цели, предпоставки
- Обяснителни раздели с диаграми
- Номерирани упражнения с кодови блокове и очакван резултат
- Обобщителна таблица, ключови изводи, допълнително четиво
- Навигационна връзка към следващата част

При редакция на лабораторни материали:
- Запазвайте съществуващия Markdown стил и структура на секциите.
- Кодови блокове трябва да задават езика (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Предлагайте както bash, така и PowerShell варианти за shell команди при зависимости от ОС.
- Използвайте стилове за обаждане `> **Note:**`, `> **Tip:**`, `> **Troubleshooting:**`.
- Таблиците използват pipe формат `| Header | Header |`.

## Команди за билд и тестване

| Действие | Команда |
|--------|---------|
| **Python примери** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS примери** | `cd javascript && npm install && node <script>.mjs` |
| **C# примери** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (уеб)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (уеб)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Генериране на диаграми** | `npx mmdc -i <input>.mmd -o <output>.svg` (изисква инсталиране с root `npm install`) |

## Външни зависимости

- **Foundry Local CLI** трябва да е инсталиран на машината на разработчика (`winget install Microsoft.FoundryLocal` или `brew install foundrylocal`).
- **Foundry Local service** работи локално и предоставя OpenAI-съвместим REST API на динамичен порт.
- Не са необходими облачни услуги, API ключове или абонаменти в Azure за изпълнение на какъвто и да е пример.
- Част 10 (персонализирани модели) изисква допълнително `onnxruntime-genai` и теглене на тегла за модели от Hugging Face.

## Файлове, които не трябва да се комитират

`.gitignore` трябва да изключва (и е конфигуриран за повечето):
- `.venv/` — Python виртуални среди
- `node_modules/` — npm зависимости
- `models/` — компилиран ONNX модел (големи бинарни файлове, генерирани в Част 10)
- `cache_dir/` — кеш за изтегляне на модели от Hugging Face
- `.olive-cache/` — работна директория Microsoft Olive
- `samples/audio/*.wav` — генерирани аудио примери (генерират се повторно чрез `python samples/audio/generate_samples.py`)
- Стандартни Python билд артефакти (`__pycache__/`, `*.egg-info/`, `dist/` и др.)

## Лиценз

MIT — виж `LICENSE`.