# Зимен журнал — Foundry Local Workshop

Всички забележителни промени в този уъркшоп са документирани по-долу.

---

## 2026-03-11 — Част 12 и 13, Уеб UI, Пренаписване на Whisper, Корекция на WinML/QNN и Валидация

### Добавени
- **Част 12: Създаване на Уеб UI за Zava Creative Writer** — ново ръководство за лаборатория (`labs/part12-zava-ui.md`) с упражнения, обхващащи стрийминг на NDJSON, браузърен `ReadableStream`, значки за статус на живо на агент и стрийминг на текст на статия в реално време
- **Част 13: Уъркшопът завършен** — ново обобщително ръководство (`labs/part13-workshop-complete.md`) с преглед на всички 12 части, допълнителни идеи и връзки към ресурси
- **Zava UI фронтенд:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — споделен ванилов HTML/CSS/JS браузърен интерфейс, използван от трите бекенда
- **JavaScript HTTP сървър:** `zava-creative-writer-local/src/javascript/server.mjs` — нов Express-стил HTTP сървър, обвиващ оркестратора за достъп през браузъра
- **C# ASP.NET Core бекенд:** `zava-creative-writer-local/src/csharp-web/Program.cs` и `ZavaCreativeWriterWeb.csproj` — нов минимален API проект, обслужващ UI и стрийминг на NDJSON
- **Генератор на аудио семпли:** `samples/audio/generate_samples.py` — офлайн TTS скрипт, използващ `pyttsx3` за генериране на Zava-тематични WAV файлове за Част 9
- **Аудио семпъл:** `samples/audio/zava-full-project-walkthrough.wav` — нов по-дълъг аудио семпъл за тестване на транскрипция
- **Скрипт за валидация:** `validate-npu-workaround.ps1` — автоматизиран PowerShell скрипт за валидация на NPU/QNN заобиколката във всички C# семпли
- **Mermaid диаграми SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML кросплатформена поддръжка:** Всички 3 C# `.csproj` файла (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) вече използват условен TFM и взаимно изключващи се препратки към пакети за кросплатформена поддръжка. На Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (суперпакет, който включва QNN EP плъгина). На не-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (базов SDK). Жестко кодираната `win-arm64` RID в Zava проектите беше заменена с автооткриване. Заобиколка на транзитивна зависимост изключва родни ресурси от `Microsoft.ML.OnnxRuntime.Gpu.Linux`, който има счупена референция към win-arm64. Предишната try/catch NPU заобиколка е премахната от всички 7 C# файла.

### Променени
- **Част 9 (Whisper):** Основен пренапис — JavaScript вече използва вградения `AudioClient` на SDK (`model.createAudioClient()`) вместо ръчна ONNX Runtime инференция; обновени описания на архитектурата, сравнителни таблици и диаграми на потока, за да отразяват JS/C# подхода с `AudioClient` срещу Python ONNX Runtime подхода
- **Част 11:** Обновени навигационни връзки (сега сочат към Част 12); добавени визуализирани SVG диаграми за потока на повиквания на инструменти и последователност
- **Част 10:** Обновена навигация, за да минава през Част 12 вместо да приключва уъркшопа
- **Python Whisper (`foundry-local-whisper.py`):** Разширен с допълнителни аудио семпли и подобрено обработване на грешки
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Пренаписан да използва `model.createAudioClient()` с `audioClient.transcribe()` вместо ръчни ONNX Runtime сесии
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Обновен да обслужва статични UI файлове заедно с API
- **Zava C# конзола (`zava-creative-writer-local/src/csharp/Program.cs`):** Премахната NPU заобиколка (сега се обработва от WinML пакета)
- **README.md:** Добавен раздел Част 12 с таблици на примерен код и бекенд добавки; добавен раздел Част 13; обновени учебни цели и структура на проекта
- **KNOWN-ISSUES.md:** Премахнато решено Проблем #7 (C# SDK NPU Model Variant — сега се обработва от WinML пакета). Преномерирани останалите проблеми до #1–#6. Обновени детайли за средата с .NET SDK 10.0.104
- **AGENTS.md:** Обновено дървото на структурата на проекта с нови записи за `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); обновени ключови пакети за C# и детайли за условен TFM
- **labs/part2-foundry-local-sdk.md:** Обновен пример `.csproj`, показващ пълния кросплатформен модел с условен TFM, взаимно изключващи се препратки към пакети и обяснителна бележка

### Валидация
- Всички 3 C# проекта (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) се компилират успешно на Windows ARM64
- Пример за чат (`dotnet run chat`): моделът се зарежда като `phi-3.5-mini-instruct-qnn-npu:1` чрез WinML/QNN — NPU вариант се зарежда директно без резервен CPU fallback
- Пример за агент (`dotnet run agent`): изпълнява се изцяло с многократен разговор, код за изход 0
- Foundry Local CLI v0.8.117 и SDK v0.9.0 на .NET SDK 9.0.312

---

## 2026-03-11 — Корекции на код, почистване на модели, Mermaid диаграми и валидация

### Коригирани
- **Всички 21 примерни кода (7 Python, 7 JavaScript, 7 C#):** Добавено `model.unload()` / `unload_model()` / `model.UnloadAsync()` почистване при изход за решаване на предупреждения за изтичане на памет в OGA (Известен проблем #4)
- **csharp/WhisperTranscription.cs:** Заменен неустойчивият относителен път `AppContext.BaseDirectory` с `FindSamplesDirectory()`, който обхожда директории нагоре, за да намери надеждно `samples/audio` (Известен проблем #7)
- **csharp/csharp.csproj:** Заменен жестко кодиран `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` с автооткриваемо резервно решение чрез `$(NETCoreSdkRuntimeIdentifier)`, така че `dotnet run` да работи на всяка платформа без флаг `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Променени
- **Част 8:** Променен цикъл за итерация чрез оценка от ASCII кутия към визуализирано SVG изображение
- **Част 10:** Променена диаграма на компилационния пайплайн от ASCII стрелки към визуализирано SVG изображение
- **Част 11:** Променени диаграми на потока за повикване на инструменти и последователност в визуализирани SVG изображения
- **Част 10:** Прехвърлена секция "Уъркшопът завършен!" към Част 11 (крайна лаборатория); заменена с линк за "Следващи стъпки"
- **KNOWN-ISSUES.md:** Пълна повторна валидация на всички проблеми срещу CLI v0.8.117. Премахнати вече решени: OGA Memory Leak (добавено почистване), Whisper път (FindSamplesDirectory), HTTP 500 продължителна инференция (не може да се възпроизведе, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), ограничения на tool_choice (сега работи с `"required"` и специфично таргетиране на функции на qwen2.5-0.5b). Обновен проблем с JS Whisper — всички файлове връщат празен/бинарен изход (регресия от v0.9.x, повишена тежест до Основен). Обновена #4 C# RID с автооткриваща се заобиколка и линк към [#497](https://github.com/microsoft/Foundry-Local/issues/497). Остават 7 отворени проблема.
- **javascript/foundry-local-whisper.mjs:** Поправено име на променлива за почистване (`whisperModel` → `model`)

### Валидация
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — изпълняват се успешно с почистване
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — изпълняват се успешно с почистване
- C#: `dotnet build` преминава с 0 предупреждения, 0 грешки (целеви net9.0)
- Всички 7 Python файла преминават синтактичен тест с `py_compile`
- Всички 7 JavaScript файла преминават синтактичен тест с `node --check`

---

## 2026-03-10 — Част 11: Извикване на инструменти, Разширение на SDK API и Покритие на модели

### Добавени
- **Част 11: Извикване на инструменти с локални модели** — ново ръководство за лаборатория (`labs/part11-tool-calling.md`) с 8 упражнения, обхващащи схеми на инструменти, мултиоборотен поток, множество повиквания на инструменти, персонализирани инструменти, извикване на ChatClient инструменти и `tool_choice`
- **Python пример:** `python/foundry-local-tool-calling.py` — извикване на инструменти с `get_weather`/`get_population` инструменти чрез OpenAI SDK
- **JavaScript пример:** `javascript/foundry-local-tool-calling.mjs` — извикване на инструменти чрез нативния `ChatClient` на SDK (`model.createChatClient()`)
- **C# пример:** `csharp/ToolCalling.cs` — извикване на инструменти чрез `ChatTool.CreateFunctionTool()` с OpenAI C# SDK
- **Част 2, Упражнение 7:** Нативен `ChatClient` — `model.createChatClient()` (JS) и `model.GetChatClientAsync()` (C#) като алтернативи на OpenAI SDK
- **Част 2, Упражнение 8:** Варианти на модели и избор на хардуер — `selectVariant()`, `variants`, таблица с NPU варианти (7 модела)
- **Част 2, Упражнение 9:** Ъпгрейди на модели и обновяване на каталог — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Част 2, Упражнение 10:** Модели за разсъждение — `phi-4-mini-reasoning` с примери за парсване на таг `<think>`
- **Част 3, Упражнение 4:** `createChatClient` като алтернатива на OpenAI SDK с документация за патерн на стрийминг callback
- **AGENTS.md:** Добавени кодови конвенции за Извикване на инструменти, ChatClient и Модели за разсъждение

### Променени
- **Част 1:** Разширен каталог с модели — добавени phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Част 2:** Разширени таблици с API референции — добавени `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Част 2:** Препрена номериране на упражнения 7-9 → 10-13 за добавяне на нови упражнения
- **Част 3:** Обновена таблица с ключови изводи, включваща нативен ChatClient
- **README.md:** Добавен раздел Част 11 с таблица с примерен код; добавена учебна цел #11; обновено дърво на структурата на проекта
- **csharp/Program.cs:** Добавен случай `toolcall` в CLI маршрутизатора и обновен текст за помощта

---

## 2026-03-09 — Ъпдейт на SDK v0.9.0, Британски английски и проход на валидация

### Променени
- **Всички примерни кодове (Python, JavaScript, C#):** Обновени към Foundry Local SDK API v0.9.0 — оправено `await catalog.getModel()` (липсваше `await`), обновени начини за инициализация на `FoundryLocalManager`, поправено откриване на крайна точка
- **Всички ръководства за лаборатории (Части 1-10):** Конвертирани на британски английски (colour, catalogue, optimised и др.)
- **Всички ръководства за лаборатории:** Обновени SDK кодови примери, за да съответстват на API на v0.9.0
- **Всички ръководства:** Обновени таблици с API референции и кодови блокове за упражнения
- **Критична поправка в JavaScript:** Добавено липсващо `await` на `catalog.getModel()` — връщаше `Promise`, а не обект `Model`, което предизвикваше тихи грешки по-надолу

### Валидация
- Всички Python примери се изпълняват успешно срещу Foundry Local услуга
- Всички JavaScript примери се изпълняват успешно (Node.js 18+)
- C# проектът се компилира и изпълнява на .NET 9.0 (съвместим нагоре от net8.0 SDK assembly)
- 29 файла са модифицирани и валидирани в целия уъркшоп

---

## Индекс на файлове

| Файл | Последна актуализация | Описание |
|------|----------------------|----------|
| `labs/part1-getting-started.md` | 2026-03-10 | Разширен каталог с модели |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Нови упражнения 7-10, разширени API таблици |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Ново упражнение 4 (ChatClient), обновени изводи |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + Британски английски |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + Британски английски |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + британски английски |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + британски английски |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid диаграма |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + британски английски |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid диаграма, преместен Workshop Complete в Част 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Нов лаб, Mermaid диаграми, секция Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Ново: пример за повикване на инструмент |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Ново: пример за повикване на инструмент |
| `csharp/ToolCalling.cs` | 2026-03-10 | Ново: пример за повикване на инструмент |
| `csharp/Program.cs` | 2026-03-10 | Добавена команда за CLI `toolcall` |
| `README.md` | 2026-03-10 | Част 11, структура на проекта |
| `AGENTS.md` | 2026-03-10 | Повикване на инструменти + конвенции за ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Премахнат решен проблем #7, оставащи 6 отворени проблема |
| `csharp/csharp.csproj` | 2026-03-11 | Кръстосан TFM платформа, условни препратки WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Кръстосан TFM платформа, автоматично откриване на RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Кръстосан TFM платформа, автоматично откриване на RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Премахнат workaround за NPU try/catch |
| `csharp/SingleAgent.cs` | 2026-03-11 | Премахнат workaround за NPU try/catch |
| `csharp/MultiAgent.cs` | 2026-03-11 | Премахнат workaround за NPU try/catch |
| `csharp/RagPipeline.cs` | 2026-03-11 | Премахнат workaround за NPU try/catch |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Премахнат workaround за NPU try/catch |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Кръстосан пример за .csproj платформа |
| `AGENTS.md` | 2026-03-11 | Актуализирани C# пакети и подробности за TFM |
| `CHANGELOG.md` | 2026-03-11 | Този файл |