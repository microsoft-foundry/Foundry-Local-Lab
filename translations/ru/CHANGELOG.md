# Журнал изменений — Foundry Local Workshop

Все заметные изменения в этом воркшопе задокументированы ниже.

---

## 2026-03-11 — Части 12 и 13, Web UI, переписанный Whisper, исправление WinML/QNN и валидация

### Добавлено
- **Часть 12: Создание Web UI для Zava Creative Writer** — новое руководство лаборатории (`labs/part12-zava-ui.md`) с упражнениями по стримингу NDJSON, браузерному `ReadableStream`, живым статусам агентов и потоковой передаче текста статьи в реальном времени
- **Часть 13: Завершение воркшопа** — новое итоговое руководство лаборатории (`labs/part13-workshop-complete.md`) с обзором всех 12 частей, дополнительными идеями и ссылками на ресурсы
- **Фронтенд Zava UI:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — общий интерфейс браузера на ванильном HTML/CSS/JS, используемый всеми тремя бекендами
- **JavaScript HTTP сервер:** `zava-creative-writer-local/src/javascript/server.mjs` — новый HTTP сервер в стиле Express, обёртывающий оркестратор для доступа из браузера
- **C# ASP.NET Core бекенд:** `zava-creative-writer-local/src/csharp-web/Program.cs` и `ZavaCreativeWriterWeb.csproj` — новый минимальный API-проект, обслуживающий UI и стриминг NDJSON
- **Генератор аудиосэмплов:** `samples/audio/generate_samples.py` — оффлайн скрипт TTS с использованием `pyttsx3` для генерации WAV файлов в стиле Zava для Части 9
- **Аудиосэмпл:** `samples/audio/zava-full-project-walkthrough.wav` — новый более длинный аудиосэмпл для тестирования транскрипции
- **Скрипт валидации:** `validate-npu-workaround.ps1` — автоматизированный PowerShell скрипт для проверки обходного решения NPU/QNN во всех C# примерах
- **SVG диаграммы Mermaid:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Кроссплатформенная поддержка WinML:** Все 3 C# `.csproj` файла (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) теперь используют условный TFM и взаимно исключающие ссылки на пакеты для кроссплатформенной поддержки. На Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (надмножество с включённым QNN EP плагином). На не-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (базовый SDK). Жёстко заданный RID `win-arm64` в проектах Zava заменён на автоопределение. Обходная зависимость исключает нативные активы из `Microsoft.ML.OnnxRuntime.Gpu.Linux`, у которого есть сломанная ссылка на win-arm64. Предыдущее try/catch обходное решение для NPU удалено из всех 7 C# файлов.

### Изменено
- **Часть 9 (Whisper):** Полный переписанный — JavaScript теперь использует встроенный `AudioClient` SDK (`model.createAudioClient()`) вместо ручного инференса ONNX Runtime; обновлены описания архитектуры, сравнительные таблицы и диаграммы пайплайна, чтобы отразить подход JS/C# с `AudioClient` vs Python с ONNX Runtime
- **Часть 11:** Обновлены навигационные ссылки (теперь указывают на Часть 12); добавлены отрендеренные SVG диаграммы для потока вызова инструментов и последовательности
- **Часть 10:** Обновлён маршрут навигации через Часть 12 вместо завершения воркшопа
- **Python Whisper (`foundry-local-whisper.py`):** Расширен дополнительными аудиосэмплами и улучшенной обработкой ошибок
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Переписан с использованием `model.createAudioClient()` и `audioClient.transcribe()` вместо ручных сессий ONNX Runtime
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Обновлен для обслуживания статических UI файлов вместе с API
- **Zava C# консоль (`zava-creative-writer-local/src/csharp/Program.cs`):** Убран обходной метод для NPU (теперь обрабатывается пакетом WinML)
- **README.md:** Добавлен раздел по Части 12 с таблицами кодовых примеров и изменениями на бекенде; добавлен раздел по Части 13; обновлены учебные цели и структура проекта
- **KNOWN-ISSUES.md:** Удалена решённая проблема #7 (вариант модели NPU для C# SDK — теперь обрабатывается пакетом WinML). Переиндексация оставшихся проблем на #1–#6. Обновлены детали окружения для .NET SDK 10.0.104
- **AGENTS.md:** Обновлена структура проекта с новыми записями `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); обновлены ключевые пакеты C# и детали условного TFM
- **labs/part2-foundry-local-sdk.md:** Обновлён пример `.csproj` с показом полной кроссплатформенной схемы с условным TFM, взаимно исключающими ссылками на пакеты и пояснительной заметкой

### Проверено
- Все 3 C# проекта (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) успешно собираются на Windows ARM64
- Пример chat (`dotnet run chat`): модель загружается как `phi-3.5-mini-instruct-qnn-npu:1` через WinML/QNN — вариант NPU загружается напрямую без падения на CPU
- Пример agent (`dotnet run agent`): работает полностью с многократным диалогом, код завершения 0
- Foundry Local CLI v0.8.117 и SDK v0.9.0 на .NET SDK 9.0.312

---

## 2026-03-11 — Исправления кода, очистка моделей, диаграммы Mermaid и валидация

### Исправлено
- **Все 21 кодовый пример (7 Python, 7 JavaScript, 7 C#):** Добавлены вызовы `model.unload()` / `unload_model()` / `model.UnloadAsync()` при выходе для устранения предупреждений утечки памяти OGA (Известная проблема #4)
- **csharp/WhisperTranscription.cs:** Заменён ненадёжный относительный путь `AppContext.BaseDirectory` на `FindSamplesDirectory()`, который надёжно ищет папку `samples/audio` двигаясь вверх по директориям (Известная проблема #7)
- **csharp/csharp.csproj:** Заменён жёстко заданный `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` на автоопределение с помощью `$(NETCoreSdkRuntimeIdentifier)`, чтобы `dotnet run` работал на любой платформе без флага `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Изменено
- **Часть 8:** Итерационный цикл, основанный на eval, преобразован из ASCII-диаграммы в отрендеренное SVG изображение
- **Часть 10:** Диаграмма компиляционного пайплайна преобразована из ASCII стрелок в отрендеренное SVG изображение
- **Часть 11:** Диаграммы потока вызова инструментов и последовательности преобразованы в отрендеренные SVG изображения
- **Часть 10:** Раздел "Workshop Complete!" перенесён в Часть 11 (финальная лаборатория); вместо него добавлена ссылка "Следующие шаги"
- **KNOWN-ISSUES.md:** Полная переоценка всех проблем с CLI v0.8.117. Удалены решённые: утечка памяти OGA (добавлена очистка), путь Whisper (FindSamplesDirectory), HTTP 500 при длительном инференсе (не воспроизводится, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), ограничения tool_choice (теперь работает с `"required"` и конкретным выбором функции на qwen2.5-0.5b). Обновлена проблема с JS Whisper — теперь все файлы возвращают пустой или бинарный вывод (регрессия с v0.9.x, серьёзность повышена до Major). Обновлён пункт #4 C# RID с обходом автоопределения и ссылкой на [#497](https://github.com/microsoft/Foundry-Local/issues/497). Осталось 7 открытых проблем.
- **javascript/foundry-local-whisper.mjs:** Исправлено название переменной очистки (`whisperModel` → `model`)

### Проверено
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — успешно запускаются с очисткой
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — успешно запускаются с очисткой
- C#: `dotnet build` проходит с 0 предупреждений и 0 ошибок (цель net9.0)
- Все 7 Python файлов проходят проверку синтаксиса `py_compile`
- Все 7 JavaScript файлов проходят проверку синтаксиса `node --check`

---

## 2026-03-10 — Часть 11: Вызов инструментов, расширение SDK API и покрытие моделей

### Добавлено
- **Часть 11: Вызов инструментов с локальными моделями** — новое руководство лаборатории (`labs/part11-tool-calling.md`) с 8 упражнениями, охватывающими схемы инструментов, многошаговые потоки, множественные вызовы инструментов, кастомные инструменты, вызовы через ChatClient и параметр `tool_choice`
- **Python пример:** `python/foundry-local-tool-calling.py` — вызов инструментов `get_weather`/`get_population` с использованием OpenAI SDK
- **JavaScript пример:** `javascript/foundry-local-tool-calling.mjs` — вызов инструментов с использованием нативного `ChatClient` SDK (`model.createChatClient()`)
- **C# пример:** `csharp/ToolCalling.cs` — вызов инструментов с помощью `ChatTool.CreateFunctionTool()` и OpenAI C# SDK
- **Часть 2, упражнение 7:** Нативный `ChatClient` — `model.createChatClient()` (JS) и `model.GetChatClientAsync()` (C#) как альтернатива OpenAI SDK
- **Часть 2, упражнение 8:** Варианты моделей и выбор оборудования — `selectVariant()`, `variants`, таблица вариантов NPU (7 моделей)
- **Часть 2, упражнение 9:** Обновления моделей и обновление каталога — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Часть 2, упражнение 10:** Модели рассуждения — `phi-4-mini-reasoning` с примерами парсинга тегов `<think>`
- **Часть 3, упражнение 4:** `createChatClient` как альтернатива OpenAI SDK с документацией по паттерну потоковой передачи данных с обратным вызовом
- **AGENTS.md:** Добавлены кодовые соглашения по вызову инструментов, ChatClient и моделям рассуждения

### Изменено
- **Часть 1:** Расширен каталог моделей — добавлены phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Часть 2:** Расширены таблицы API — добавлены `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Часть 2:** Переиндексация упражнений 7-9 на 10-13 для учёта новых упражнений
- **Часть 3:** Обновлена таблица ключевых выводов с включением нативного ChatClient
- **README.md:** Добавлен раздел по Части 11 с таблицей примеров кода; добавлена учебная цель #11; обновлено дерево структуры проекта
- **csharp/Program.cs:** Добавлен случай `toolcall` в маршрутизатор CLI и обновлён текст помощи

---

## 2026-03-09 — Обновление SDK v0.9.0, британский английский и проход валидации

### Изменено
- **Все кодовые примеры (Python, JavaScript, C#):** Обновлены под API Foundry Local SDK v0.9.0 — исправлен пропущенный `await` в `catalog.getModel()`, обновлены паттерны инициализации `FoundryLocalManager`, устранены проблемы обнаружения endpoint
- **Все инструкции лабораторий (Части 1-10):** Переведены на британский английский (colour, catalogue, optimised и пр.)
- **Все инструкции:** Обновлены примеры кода SDK под API v0.9.0
- **Все инструкции:** Обновлены таблицы API и блоки кода упражнений
- **Критическое исправление в JavaScript:** Добавлен пропущенный `await` на `catalog.getModel()` — возвращался `Promise` вместо объекта `Model`, что вызывало скрытые ошибки

### Проверено
- Все Python-примеры успешно запускаются с сервисом Foundry Local
- Все JavaScript-примеры успешно запускаются (Node.js 18+)
- C# проект успешно собирается и запускается на .NET 9.0 (совместимость с net8.0 SDK)
- Изменено и проверено 29 файлов воркшопа

---

## Индекс файлов

| Файл | Дата последнего обновления | Описание |
|------|----------------------------|----------|
| `labs/part1-getting-started.md` | 2026-03-10 | Расширен каталог моделей |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Добавлены новые упражнения 7-10, расширены таблицы API |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Новое упражнение 4 (ChatClient), обновлены ключевые выводы |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + британский английский |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + британский английский |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + британский английский |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + британский английский |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Диаграмма Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + британский английский |
| `labs/part10-custom-models.md` | 2026-03-11 | Диаграмма Mermaid, раздел «Завершение мастерской» перенесён в Часть 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Новая лабораторная, диаграммы Mermaid, раздел «Завершение мастерской» |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Новое: пример вызова инструмента |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Новое: пример вызова инструмента |
| `csharp/ToolCalling.cs` | 2026-03-10 | Новое: пример вызова инструмента |
| `csharp/Program.cs` | 2026-03-10 | Добавлена команда CLI `toolcall` |
| `README.md` | 2026-03-10 | Часть 11, структура проекта |
| `AGENTS.md` | 2026-03-10 | Вызов инструментов + соглашения ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Удалена решённая проблема №7, остались 6 открытых проблем |
| `csharp/csharp.csproj` | 2026-03-11 | Кроссплатформенный TFM, условные ссылки на WinML/базовый SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Кроссплатформенный TFM, автоопределение RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Кроссплатформенный TFM, автоопределение RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Удалено обходное решение с try/catch для NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Удалено обходное решение с try/catch для NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Удалено обходное решение с try/catch для NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Удалено обходное решение с try/catch для NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Удалено обходное решение с try/catch для NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Пример кроссплатформенного .csproj |
| `AGENTS.md` | 2026-03-11 | Обновлены пакеты C# и детали TFM |
| `CHANGELOG.md` | 2026-03-11 | Этот файл |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Отказ от ответственности**:  
Этот документ был переведен с помощью сервиса автоматического перевода [Co-op Translator](https://github.com/Azure/co-op-translator). Несмотря на наши усилия обеспечить точность, имейте в виду, что автоматический перевод может содержать ошибки или неточности. Оригинальный документ на его родном языке следует считать авторитетным источником. Для критически важной информации рекомендуется профессиональный перевод человеком. Мы не несем ответственности за любые недоразумения или неправильные толкования, возникшие из использования данного перевода.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->