# Журнал змін — Foundry Local Workshop

Усі важливі зміни для цього воркшопу задокументовані нижче.

---

## 2026-03-11 — Частини 12 і 13, Веб-інтерфейс, Переписування Whisper, Виправлення WinML/QNN та Валідація

### Додано
- **Частина 12: Створення веб-інтерфейсу для Zava Creative Writer** — новий посібник лабораторії (`labs/part12-zava-ui.md`) з вправами, що охоплюють потокову передачу NDJSON, браузерний `ReadableStream`, бейджі статусу агента в режимі реального часу та трансляцію тексту статті в реальному часі
- **Частина 13: Воркшоп завершено** — новий оглядовий лабораторний посібник (`labs/part13-workshop-complete.md`) з підсумком усіх 12 частин, подальшими ідеями та посиланнями на ресурси
- **Фронтенд інтерфейсу Zava UI:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — спільний ванільний HTML/CSS/JS інтерфейс браузера, що використовується всіма трьома бекендами
- **HTTP сервер на JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — новий HTTP сервер у стилі Express, що обгортає оркестратор для доступу через браузер
- **Бекенд на C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` і `ZavaCreativeWriterWeb.csproj` — новий мінімальний API проект, що обслуговує UI та потоковий NDJSON
- **Генератор аудіо зразків:** `samples/audio/generate_samples.py` — офлайн скрипт TTS з `pyttsx3` для генерації WAV-файлів на тему Zava для Частини 9
- **Аудіо зразок:** `samples/audio/zava-full-project-walkthrough.wav` — новий довший аудіо зразок для тестування транскрипції
- **Скрипт валідації:** `validate-npu-workaround.ps1` — автоматизований PowerShell скрипт для перевірки обхідного рішення NPU/QNN у всіх C# прикладах
- **SVG-діаграми Mermaid:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Кросплатформена підтримка WinML:** Усі 3 C# `.csproj` файли (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) тепер використовують умовні TFM та взаємовиключні посилання на пакети для кросплатформеної підтримки. На Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (надмножина, що включає плагін QNN EP). На не-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (базовий SDK). Жорсткокодований RID `win-arm64` у проєктах Zava замінено на автоматичне виявлення. Обхідна залежність виключає нативні активи з `Microsoft.ML.OnnxRuntime.Gpu.Linux`, який має пошкоджене посилання на win-arm64. Попередній обхід NPU через try/catch видалено з усіх 7 C# файлів.

### Змінено
- **Частина 9 (Whisper):** Капітальний перепис — JavaScript тепер використовує вбудований SDK `AudioClient` (`model.createAudioClient()`) замість ручного виклику ONNX Runtime; оновлені архітектурні описи, таблиці порівняння та діаграми пайплайнів для відображення підходу JS/C# `AudioClient` проти Python ONNX Runtime
- **Частина 11:** Оновлені посилання навігації (тепер вказує на Частину 12); додані відрендерені SVG діаграми для потоку виклику інструментів і послідовності
- **Частина 10:** Оновлена навігація для маршрутизації через Частину 12 замість завершення воркшопу
- **Python Whisper (`foundry-local-whisper.py`):** Розширено додатковими аудіо зразками та покращено обробку помилок
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Переписано з використанням `model.createAudioClient()` і `audioClient.transcribe()` замість ручних сесій ONNX Runtime
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Оновлено для сервування статичних UI файлів разом з API
- **Консольний додаток Zava C# (`zava-creative-writer-local/src/csharp/Program.cs`):** Видалено обходження NPU (тепер обробляється пакетом WinML)
- **README.md:** Додано розділ Частини 12 з таблицями прикладів коду і додатками бекенду; додано розділ Частини 13; оновлено навчальні цілі та структуру проєкту
- **KNOWN-ISSUES.md:** Видалено вирішене питання #7 (C# SDK NPU Model Variant — тепер обробляється пакетом WinML). Перезнумеровано інші питання на #1–#6. Оновлено деталі середовища з .NET SDK 10.0.104
- **AGENTS.md:** Оновлено дерево структури проєкту новими записами `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); оновлено ключові пакети C# і деталі умовних TFM
- **labs/part2-foundry-local-sdk.md:** Оновлено приклад `.csproj` для відображення повної кросплатформеної схеми з умовним TFM, взаємовиключними посиланнями на пакети та пояснювальною приміткою

### Перевірено
- Усі 3 C# проєкти (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) успішно збираються на Windows ARM64
- Чат-приклад (`dotnet run chat`): модель завантажується як `phi-3.5-mini-instruct-qnn-npu:1` через WinML/QNN — варіант NPU завантажується напряму без переходу на CPU
- Приклад агента (`dotnet run agent`): працює повністю з багатокроковою розмовою, код виходу 0
- Foundry Local CLI v0.8.117 і SDK v0.9.0 на .NET SDK 9.0.312

---

## 2026-03-11 — Виправлення коду, очищення моделей, діаграми Mermaid і Валідація

### Виправлено
- **Усі 21 приклад коду (7 Python, 7 JavaScript, 7 C#):** Додано `model.unload()` / `unload_model()` / `model.UnloadAsync()` очистку при виході для усунення попереджень про витоки пам’яті OGA (відоме питання #4)
- **csharp/WhisperTranscription.cs:** Замінено крихкий відносний шлях `AppContext.BaseDirectory` на `FindSamplesDirectory()`, що піднімається по директоріях для надійного знаходження `samples/audio` (відоме питання #7)
- **csharp/csharp.csproj:** Замінено жорсткокодований `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` на автоматичне виявлення з використанням `$(NETCoreSdkRuntimeIdentifier)` для роботи `dotnet run` на будь-якій платформі без прапорця `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Змінено
- **Частина 8:** Перетворено ітеровану петлю eval-driven з ASCII box діаграми на відрендерене SVG зображення
- **Частина 10:** Перетворено діаграму пайплайну компіляції з ASCII стрілок на відрендерене SVG зображення
- **Частина 11:** Перетворено діаграми потоку виклику інструментів і послідовності на відрендерені SVG зображення
- **Частина 10:** Розділ "Воркшоп завершено!" перенесено до Частини 11 (остаточна лабораторія); замінено посиланням "Наступні кроки"
- **KNOWN-ISSUES.md:** Повна перевірка всіх питань для CLI v0.8.117. Видалено вирішені: Витік пам’яті OGA (додано очищення), шлях Whisper (FindSamplesDirectory), HTTP 500 при тривалому висновку (не відтворюється, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), обмеження tool_choice (тепер працює з `"required"` і цільовим функціоналом на qwen2.5-0.5b). Оновлено проблему JS Whisper — тепер усі файли повертають порожній/бінарний вихід (регресія з v0.9.x, рівень серйозності підвищено до Major). Оновлено #4 C# RID з обходом автоматичного виявлення і посиланням [#497](https://github.com/microsoft/Foundry-Local/issues/497). 7 відкритих питань залишаються.
- **javascript/foundry-local-whisper.mjs:** Виправлено ім’я змінної очищення (`whisperModel` → `model`)

### Перевірено
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — успішне виконання з очищенням
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — успішне виконання з очищенням
- C#: `dotnet build` успішний без попереджень, без помилок (цільова платформа net9.0)
- Усі 7 Python файлів проходять перевірку синтаксису `py_compile`
- Усі 7 JavaScript файлів проходять валідацію синтаксису `node --check`

---

## 2026-03-10 — Частина 11: Виклик інструментів, Розширення API SDK і Покриття моделей

### Додано
- **Частина 11: Виклик інструментів з локальними моделями** — новий лабораторний посібник (`labs/part11-tool-calling.md`) з 8 вправами, що охоплюють схеми інструментів, багатокрокові потоки, множинні виклики інструментів, користувацькі інструменти, виклик інструментів через ChatClient і `tool_choice`
- **Приклад на Python:** `python/foundry-local-tool-calling.py` — виклик інструментів `get_weather`/`get_population` за допомогою OpenAI SDK
- **Приклад на JavaScript:** `javascript/foundry-local-tool-calling.mjs` — виклик інструментів із рідним SDK через `ChatClient` (`model.createChatClient()`)
- **Приклад на C#:** `csharp/ToolCalling.cs` — виклик інструментів через `ChatTool.CreateFunctionTool()` з OpenAI C# SDK
- **Частина 2, вправа 7:** Рідний `ChatClient` — `model.createChatClient()` (JS) і `model.GetChatClientAsync()` (C#) як альтернатива OpenAI SDK
- **Частина 2, вправа 8:** Варіанти моделей і вибір апаратного забезпечення — `selectVariant()`, `variants`, таблиця варіантів NPU (7 моделей)
- **Частина 2, вправа 9:** Оновлення моделей та поновлення каталогу — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Частина 2, вправа 10:** Моделі для міркувань — `phi-4-mini-reasoning` з прикладами парсингу тегу `<think>`
- **Частина 3, вправа 4:** `createChatClient` як альтернатива OpenAI SDK із документацією шаблону потокового виклику колбеків
- **AGENTS.md:** Додано керівництво по кодуванню для Виклику інструментів, ChatClient і Моделей для міркувань

### Змінено
- **Частина 1:** Розширений каталог моделей — додані phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Частина 2:** Розширені таблиці API — додано `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Частина 2:** Перенумеровано вправи 7-9 → 10-13 для врахування нових вправ
- **Частина 3:** Оновлено таблицю ключових висновків з урахуванням рідного ChatClient
- **README.md:** Додано розділ Частини 11 з таблицею прикладів коду; додано навчальну ціль #11; оновлено дерево структури проєкту
- **csharp/Program.cs:** Додано кейс `toolcall` до маршрутизатора CLI та оновлено текст довідки

---

## 2026-03-09 — Оновлення SDK v0.9.0, Британська англійська та проходження валідації

### Змінено
- **Усі приклади коду (Python, JavaScript, C#):** Оновлені до Foundry Local SDK v0.9.0 API — виправлено `await catalog.getModel()` (бракувало `await`), оновлені патерни ініціалізації `FoundryLocalManager`, виправлено виявлення кінцевих точок
- **Всі лабораторні посібники (Частини 1-10):** Перекладено на британську англійську (colour, catalogue, optimised тощо)
- **Всі лабораторні посібники:** Оновлені приклади коду SDK відповідно до API v0.9.0
- **Всі лабораторні посібники:** Оновлені таблиці API і фрагменти коду вправ
- **Критичне виправлення JavaScript:** Додано пропущене `await` на `catalog.getModel()` — він повертав `Promise`, а не об’єкт `Model`, що викликало приховані помилки

### Перевірено
- Усі Python приклади успішно працюють з Foundry Local сервісом
- Усі JavaScript приклади успішно працюють (Node.js 18+)
- C# проект збирається і працює на .NET 9.0 (сумісність з net8.0 SDK складанням)
- Модифіковано 29 файлів та валідовано через воркшоп

---

## Індекс файлів

| Файл | Останнє оновлення | Опис |
|------|-------------------|------|
| `labs/part1-getting-started.md` | 2026-03-10 | Розширений каталог моделей |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Нові вправи 7-10, розширені таблиці API |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Нова вправа 4 (ChatClient), оновлені висновки |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + британська англійська |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + британська англійська |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + британська англійська |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + британська англійська |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Діаграма Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + британська англійська |
| `labs/part10-custom-models.md` | 2026-03-11 | Діаграма Mermaid, розділ Завершення майстер-класу перенесено до Частини 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Новий лаб, діаграми Mermaid, розділ Завершення майстер-класу |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Новий: приклад виклику інструменту |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Новий: приклад виклику інструменту |
| `csharp/ToolCalling.cs` | 2026-03-10 | Новий: приклад виклику інструменту |
| `csharp/Program.cs` | 2026-03-10 | Додано CLI команду `toolcall` |
| `README.md` | 2026-03-10 | Частина 11, структура проєкту |
| `AGENTS.md` | 2026-03-10 | Виклик інструменту + конвенції ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Видалено розв’язану проблему #7, залишилось 6 відкритих проблем |
| `csharp/csharp.csproj` | 2026-03-11 | Кросплатформовий TFM, WinML/base SDK умовні посилання |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Кросплатформовий TFM, авто-детекція RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Кросплатформовий TFM, авто-детекція RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Видалено обхід try/catch для NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Видалено обхід try/catch для NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Видалено обхід try/catch для NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Видалено обхід try/catch для NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Видалено обхід try/catch для NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Кросплатформовий приклад .csproj |
| `AGENTS.md` | 2026-03-11 | Оновлено пакунки C# та деталі TFM |
| `CHANGELOG.md` | 2026-03-11 | Цей файл |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Відмова від відповідальності**:  
Цей документ було перекладено за допомогою сервісу автоматичного перекладу штучного інтелекту [Co-op Translator](https://github.com/Azure/co-op-translator). Хоча ми прагнемо до точності, будь ласка, майте на увазі, що автоматичні переклади можуть містити помилки або неточності. Оригінальний документ рідною мовою слід вважати авторитетним джерелом. Для критично важливої інформації рекомендується звертатися до професійного людського перекладу. Ми не несемо відповідальності за будь-які непорозуміння або неправильні тлумачення, що виникли внаслідок використання цього перекладу.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->