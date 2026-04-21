# Инструкции для программирования агента

Этот файл предоставляет контекст для AI-агентов программирования (GitHub Copilot, Copilot Workspace, Codex и др.), работающих в этом репозитории.

## Обзор проекта

Это **практический семинар** по созданию AI-приложений с помощью [Foundry Local](https://foundrylocal.ai) — легковесного рантайма, который загружает, управляет и обслуживает языковые модели полностью на устройстве через совместимый с OpenAI API. Семинар включает пошаговые лабораторные руководства и исполняемые примеры кода на Python, JavaScript и C#.

## Структура репозитория

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

## Детали языков и фреймворков

### Python
- **Расположение:** `python/`, `zava-creative-writer-local/src/api/`
- **Зависимости:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Ключевые пакеты:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Мин. версия:** Python 3.9+
- **Запуск:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Расположение:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Зависимости:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Ключевые пакеты:** `foundry-local-sdk`, `openai`
- **Система модулей:** ES модули (`.mjs` файлы, `"type": "module"`)
- **Мин. версия:** Node.js 18+
- **Запуск:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Расположение:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Проектные файлы:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Ключевые пакеты:** `Microsoft.AI.Foundry.Local` (не-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — надмножество с QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Цель:** .NET 9.0 (условный TFM: `net9.0-windows10.0.26100` на Windows, `net9.0` в других случаях)
- **Запуск:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Конвенции кодирования

### Общее
- Все примеры кода — **одиночные файлы без внешних библиотек или абстракций**.
- Каждый пример запускается самостоятельно после установки своих зависимостей.
- Ключи API всегда установлены в `"foundry-local"` — Foundry Local использует это как заполнитель.
- Базовые URL используют `http://localhost:<port>/v1` — порт динамический и определяется во время выполнения через SDK (`manager.urls[0]` в JS, `manager.endpoint` в Python).
- SDK Foundry Local управляет запуском сервиса и обнаружением эндпоинтов; предпочтительнее использовать SDK, а не жестко заданные порты.

### Python
- Используйте SDK `openai` с `OpenAI(base_url=..., api_key="not-required")`.
- Используйте `FoundryLocalManager()` из `foundry_local` для управления жизненным циклом сервиса через SDK.
- Для потоковой передачи итерируйтесь по объекту `stream` через `for chunk in stream:`.
- В примерах нет аннотаций типов (чтобы сохранять компактность для обучающихся).

### JavaScript
- Синтаксис ES модулей: `import ... from "..."`.
- Используйте `OpenAI` из `"openai"` и `FoundryLocalManager` из `"foundry-local-sdk"`.
- Паттерн инициализации SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Потоковая передача: `for await (const chunk of stream)`.
- Используется top-level `await` повсеместно.

### C#
- Включена поддержка Nullable, неявные using, .NET 9.
- Используйте `FoundryLocalManager.StartServiceAsync()` для управления жизненным циклом сервиса через SDK.
- Потоковая передача: `CompleteChatStreaming()` с `foreach (var update in completionUpdates)`.
- Главный файл `csharp/Program.cs` — это маршрутизатор CLI, который вызывает статические методы `RunAsync()`.

### Вызов инструментов
- Только некоторые модели поддерживают вызов инструментов: семейство **Qwen 2.5** (`qwen2.5-*`) и **Phi-4-mini** (`phi-4-mini`).
- Схемы инструментов следуют формату OpenAI function-calling JSON (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Диалог использует многоходовой паттерн: пользователь → ассистент (tool_calls) → инструмент (результаты) → ассистент (окончательный ответ).
- `tool_call_id` в сообщениях с результатами инструмента должен совпадать с `id` из вызова инструмента модели.
- Python использует напрямую OpenAI SDK; JavaScript использует нативный `ChatClient` SDK (`model.createChatClient()`); C# использует OpenAI SDK с `ChatTool.CreateFunctionTool()`.

### ChatClient (нативный SDK клиент)
- JavaScript: `model.createChatClient()` возвращает `ChatClient` с методами `completeChat(messages, tools?)` и `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` возвращает стандартный `ChatClient`, который можно использовать без импорта NuGet пакета OpenAI.
- Python не имеет нативного ChatClient — используйте OpenAI SDK с `manager.endpoint` и `manager.api_key`.
- **Важно:** в JavaScript `completeStreamingChat` использует **callback-паттерн**, а не асинхронную итерацию.

### Модели рассуждения
- `phi-4-mini-reasoning` оборачивает рассуждения в теги `<think>...</think>` перед конечным ответом.
- При необходимости парсите эти теги, чтобы отделить рассуждения от ответа.

## Руководства лабораторных работ

Файлы лабораторий находятся в `labs/` в формате Markdown. Они имеют последовательную структуру:
- Заголовок с логотипом
- Название и цель
- Обзор, цели обучения, предварительные условия
- Разделы объяснения концепций с диаграммами
- Нумерованные упражнения с блоками кода и ожидаемым выводом
- Итоговая таблица, основные выводы, дополнительные материалы
- Ссылка навигации к следующей части

При редактировании содержимого лабораторий:
- Сохраняйте существующий стиль разметки Markdown и иерархию разделов.
- Для блоков кода обязательно указывать язык (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Предоставляйте как bash, так и PowerShell варианты команд там, где зависит от ОС.
- Используйте стили врезок `> **Примечание:**`, `> **Совет:**`, `> **Решение проблем:**`.
- Таблицы оформляйте в формате с трубами `| Заголовок | Заголовок |`.

## Команды сборки и тестирования

| Действие | Команда |
|--------|---------|
| **Примеры Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **Примеры JS** | `cd javascript && npm install && node <script>.mjs` |
| **Примеры C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (веб)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (веб)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Генерация диаграмм** | `npx mmdc -i <input>.mmd -o <output>.svg` (требует глобальной установки `npm install`) |

## Внешние зависимости

- **Foundry Local CLI** должен быть установлен на машине разработчика (`winget install Microsoft.FoundryLocal` или `brew install foundrylocal`).
- **Foundry Local сервис** работает локально и предоставляет совместимый с OpenAI REST API на динамическом порту.
- Для запуска примеров не требуются облачные сервисы, ключи API или подписки Azure.
- В части 10 (пользовательские модели) дополнительно требуется `onnxruntime-genai` и загрузка весов моделей с Hugging Face.

## Файлы, которые не должны загружаться в репозиторий

В `.gitignore` должны быть исключены (и так имеется для большинства):
- `.venv/` — виртуальные окружения Python
- `node_modules/` — npm зависимости
- `models/` — скомпилированные ONNX модели (большие бинарные файлы, генерируемые в Части 10)
- `cache_dir/` — кеш загрузки моделей Hugging Face
- `.olive-cache/` — рабочая директория Microsoft Olive
- `samples/audio/*.wav` — сгенерированные аудио примеры (перегенерируются через `python samples/audio/generate_samples.py`)
- Стандартные модули сборки Python (`__pycache__/`, `*.egg-info/`, `dist/` и т.п.)

## Лицензия

MIT — см. `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Отказ от ответственности**:  
Этот документ был переведен с помощью службы автоматического перевода [Co-op Translator](https://github.com/Azure/co-op-translator). Несмотря на наши усилия обеспечить точность, имейте в виду, что автоматический перевод может содержать ошибки или неточности. Оригинальный документ на исходном языке следует считать авторитетным источником. Для критически важной информации рекомендуется использовать профессиональный перевод, выполненный человеком. Мы не несем ответственности за любые недоразумения или неправильные толкования, возникшие в результате использования данного перевода.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->