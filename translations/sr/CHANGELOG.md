# Дневник промена — Foundry Local радионица

Сви значајније промене у овој радионици су забележене испод.

---

## 2026-03-11 — Део 12 и 13, Веб кориснички интерфејс, преписивање Whisper-а, WinML/QNN поправка и валидација

### Додато
- **Део 12: Креирање веб корисничког интерфејса за Zava Creative Writer** — нови лабораторијски водич (`labs/part12-zava-ui.md`) са вежбама које обухватају стримовање NDJSON, браузер `ReadableStream`, ознаке статуса живог агента и стримовање текста чланка у реалном времену
- **Део 13: Завршетак радионице** — нови резиме лабораторија (`labs/part13-workshop-complete.md`) са прегледом свих 12 делова, даљим идејама и линковима ка ресурсима
- **Zava UI фронтенд:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — заједнички ванила HTML/CSS/JS браузерски интерфејс који користе сва три бекенда
- **JavaScript HTTP сервер:** `zava-creative-writer-local/src/javascript/server.mjs` — нови Express-стил HTTP сервер који омотава оркестратор за приступ преко браузера
- **C# ASP.NET Core бекенд:** `zava-creative-writer-local/src/csharp-web/Program.cs` и `ZavaCreativeWriterWeb.csproj` — нови минимални API пројекат који служи UI и стримовање NDJSON
- **Генератор аудио узорака:** `samples/audio/generate_samples.py` — офлајн скрипта за TTS користећи `pyttsx3` за генерисање WAV фајлова на тему Zava за Део 9
- **Аудио узорак:** `samples/audio/zava-full-project-walkthrough.wav` — нови дужи аудио узорак за тестирање транскрипције
- **Скрипта за валидацију:** `validate-npu-workaround.ps1` — аутоматизована PowerShell скрипта за проверу NPU/QNN заобилазног решења у свим C# примерима
- **SVG дијаграми у Mermaid-у:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML крос-платформска подршка:** Сва 3 C# `.csproj` фајла (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) сада користе условни TFM и међусобно искључиве референце пакета за крос-платформску подршку. На Виндоус-у: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (надмножина која укључује QNN EP плагин). На не-Виндоус платформама: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (базни SDK). Тврдокодирани `win-arm64` RID у Zava пројектима је замењен аутоматским детектовањем. Заобилазно решење транзитивне зависности искључује домаће ресурсе из `Microsoft.ML.OnnxRuntime.Gpu.Linux` који има поломљену win-arm64 референцу. Претходно NPU заобилазно решење са try/catch је уклоњено из свих 7 C# фајлова.

### Измењено
- **Део 9 (Whisper):** Главно преписивање — JavaScript сада користи уграђени SDK `AudioClient` (`model.createAudioClient()`) уместо ручног ONNX Runtime инференса; ажурирани описи архитектуре, табеле упоређивања и дијаграми пипелина да одражавају приступ JS/C# `AudioClient` у односу на Python ONNX Runtime
- **Део 11:** Ажурирани навигациони линкови (сада воде ка Делу 12); додати SVG дијаграми тока позива алата и секвенце
- **Део 10:** Навигација је ажурирана да води кроз Део 12 уместо да завршава радионицу
- **Python Whisper (`foundry-local-whisper.py`):** Проширен са додатним аудио узорцима и побољшаном обрадом грешака
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Преписан да користи `model.createAudioClient()` са `audioClient.transcribe()` уместо ручних ONNX Runtime сесија
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Ажуриран да служи статичке UI фајлове уз API
- **Zava C# конзола (`zava-creative-writer-local/src/csharp/Program.cs`):** Уклонио NPU заобилазно решење (сада обрађује WinML пакет)
- **README.md:** Додат одељак за Део 12 са табелама примера кода и додацима бекенда; додат одељак за Део 13; ажурирани циљеви учења и структура пројекта
- **KNOWN-ISSUES.md:** Уклоњен решени Проблем #7 (C# SDK NPU модел варијанта — сада обрађује WinML пакет). Пребројани преостали проблеми на #1–#6. Ажурирани детаљи о окружењу са .NET SDK 10.0.104
- **AGENTS.md:** Ажурирано стабло структуре пројекта са новим уносима у `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); ажуриране кључне C# пакете и детаље о условном TFM
- **labs/part2-foundry-local-sdk.md:** Ажуриран `.csproj` пример да приказује пун крос-платформски образац са условним TFM, међусобно искључивим референцама пакета и објашњавајућом напоменом

### Потврђено
- Сва 3 C# пројекта (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) успешно се граде на Windows ARM64
- Пример ћаскања (`dotnet run chat`): модел се учитава као `phi-3.5-mini-instruct-qnn-npu:1` преко WinML/QNN — NPU варијанта се учитава директно без падa на CPU
- Пример агента (`dotnet run agent`): ради од почетка до краја са вишекратним окретајима разговора, код изласка 0
- Foundry Local CLI v0.8.117 и SDK v0.9.0 на .NET SDK 9.0.312

---

## 2026-03-11 — Поправке кода, чишћење модела, Mermaid дијаграми и валидација

### Поправљено
- **Сви 21 пример кода (7 Python, 7 JavaScript, 7 C#):** Додато `model.unload()` / `unload_model()` / `model.UnloadAsync()` чишћење на излазу да се реше упозорења о curenju меморије у OGA (Познат проблем #4)
- **csharp/WhisperTranscription.cs:** Замена крхког релативног пута `AppContext.BaseDirectory` са `FindSamplesDirectory()` који поуздано проналази `samples/audio` пешачећи изнад директоријума (Познат проблем #7)
- **csharp/csharp.csproj:** Замена тврдокодиране `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` са аутоматским откривањем путем `$(NETCoreSdkRuntimeIdentifier)` тако да `dotnet run` ради на било којој платформи без `-r` параметра ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Измењено
- **Део 8:** Претворена петља за евалуацију из ASCII бокса у произведену SVG слику
- **Део 10:** Претворен дијаграм пипелина компилације из ASCII стрелица у произведену SVG слику
- **Део 11:** Претворени дијаграми тока позива алата и секвенце у произведене SVG слике
- **Део 10:** Померен одељак „Завршетак радионице!“ у Део 11 (последњу лабораторију); замењен ликом „Следећи кораци“
- **KNOWN-ISSUES.md:** Потпуна ревалидација свих проблема у односу на CLI v0.8.117. Уклоњени решени: curenje меморије у OGA (чишћење додато), Whisper пут (FindSamplesDirectory), HTTP 500 непоновљив инференс ([#494](https://github.com/microsoft/Foundry-Local/issues/494)), ограничења tool_choice (сада ради са `"required"` и специфичним циљевима функција на qwen2.5-0.5b). Ажурирано JS Whisper питање — сада сви фајлови враћају празан/бинарни излаз (регресија од v0.9.x, степен озбиљности повећан на Важно). Ажуриран #4 C# RID са аутоматским заобилазним решењем и [#497](https://github.com/microsoft/Foundry-Local/issues/497) линком. Преостаје 7 отворених проблема.
- **javascript/foundry-local-whisper.mjs:** Исправљено име променљиве за чишћење (`whisperModel` → `model`)

### Потврђено
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — успешно покренути са чишћењем
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — успешно покренути са чишћењем
- C#: `dotnet build` успешно са 0 упозорења и 0 грешака (net9.0 таргет)
- Сви 7 Python фајлова пролазе синтаксну проверу `py_compile`
- Сви 7 JavaScript фајлова пролазе валидaцију синтаксе `node --check`

---

## 2026-03-10 — Део 11: Позивање алата, проширење SDK API-ја и покриће модела

### Додато
- **Део 11: Позивање алата са локалним моделима** — нови лабораторијски водич (`labs/part11-tool-calling.md`) са 8 вежби о шемама алата, вишекратном току, више позива алата, прилагођеним алатима, позивању алата у ChatClient-у и `tool_choice`
- **Python пример:** `python/foundry-local-tool-calling.py` — позив алата са `get_weather`/`get_population` алатима користећи OpenAI SDK
- **JavaScript пример:** `javascript/foundry-local-tool-calling.mjs` — позив алата користећи нативни SDK `ChatClient` (`model.createChatClient()`)
- **C# пример:** `csharp/ToolCalling.cs` — позив алата користећи `ChatTool.CreateFunctionTool()` са OpenAI C# SDK
- **Део 2, Вежба 7:** Нативни `ChatClient` — `model.createChatClient()` (JS) и `model.GetChatClientAsync()` (C#) као алтернативе OpenAI SDK-ју
- **Део 2, Вежба 8:** Варијанте модела и избор хардвера — `selectVariant()`, `variants`, табела NPU варијанти (7 модела)
- **Део 2, Вежба 9:** Надоградње модела и освежавање каталога — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Део 2, Вежба 10:** Разлучивачки модели — `phi-4-mini-reasoning` са примерима парсирања `<think>` тагова
- **Део 3, Вежба 4:** `createChatClient` као алтернатива OpenAI SDK-ју, са документацијом шаблона повратних позива за стримовање
- **AGENTS.md:** Додате конвенције кодирања за позивање алата, ChatClient и разлучивачке моделе

### Измењено
- **Део 1:** Проширен каталог модела — додати phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Део 2:** Проширене табеле API референци — додати `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Део 2:** Пребројане вежбе 7-9 → 10-13 због нових вежби
- **Део 3:** Ажурирана табела кључних поуке да укључи нативни ChatClient
- **README.md:** Додат одељак за Део 11 са табелом примера кода; додат циљ учења #11; ажурирано стабло структуре пројекта
- **csharp/Program.cs:** Додат случај `toolcall` у CLI рутирање и ажуриран помоћни текст

---

## 2026-03-09 — Ажурирање SDK v0.9.0, британски енглески и још валидације

### Измењено
- **Сви примери кода (Python, JavaScript, C#):** Ажурирани на Foundry Local SDK v0.9.0 API — исправљен `await catalog.getModel()` (недостајао `await`), ажурирани обрасци иницијализације `FoundryLocalManager`, поправљено откривање крајње тачке
- **Сви лабораторијски водичи (Делови 1-10):** Преведени на британски енглески (colour, catalogue, optimised итд.)
- **Сви лабораторијски водичи:** Ажурирани примерни кôд SDK-а да одговара API-ју v0.9.0
- **Сви лабораторијски водичи:** Ажуриране табеле API референци и блокови кода вежби
- **JavaScript критична поправка:** Додат недостајући `await` на `catalog.getModel()` — враћао је `Promise` уместо `Model` објекта што је изазивало тиха неуспеха касније

### Потврђено
- Сви Python примери успешно раде са Foundry Local сервисом
- Сви JavaScript примери успешно раде (Node.js 18+)
- C# пројекат се гради и покреће на .NET 9.0 (напредна компатибилност са net8.0 SDK саставом)
- Измењено и верификовано 29 фајлова кроз радионицу

---

## Индекс фајлова

| Фајл | Последња измена | Опис |
|------|-----------------|------|
| `labs/part1-getting-started.md` | 2026-03-10 | Проширен каталог модела |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Нове вежбе 7-10, проширене API табеле |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Нова вежба 4 (ChatClient), ажуриране поуке |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + британски енглески |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + британски енглески |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + британски енглески |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + британски енглески |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid дијаграм |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + британски енглески |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid дијаграм, Радионица Завршена премештена у део 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Нова лабораторија, Mermaid дијаграми, одељак Радионица Завршена |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Ново: пример позива алата |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Ново: пример позива алата |
| `csharp/ToolCalling.cs` | 2026-03-10 | Ново: пример позива алата |
| `csharp/Program.cs` | 2026-03-10 | Додата `toolcall` CLI команда |
| `README.md` | 2026-03-10 | Део 11, структура пројекта |
| `AGENTS.md` | 2026-03-10 | Позивање алата + конвенције ChatClient-a |
| `KNOWN-ISSUES.md` | 2026-03-11 | Уклоњен решени проблем бр. 7, остало 6 отворених |
| `csharp/csharp.csproj` | 2026-03-11 | Крос-платформски TFM, WinML/base SDK условне референце |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Крос-платформски TFM, аутоматско препознавање RID-а |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Крос-платформски TFM, аутоматско препознавање RID-а |
| `csharp/BasicChat.cs` | 2026-03-11 | Уклоњен NPU try/catch начин заобилажења |
| `csharp/SingleAgent.cs` | 2026-03-11 | Уклоњен NPU try/catch начин заобилажења |
| `csharp/MultiAgent.cs` | 2026-03-11 | Уклоњен NPU try/catch начин заобилажења |
| `csharp/RagPipeline.cs` | 2026-03-11 | Уклоњен NPU try/catch начин заобилажења |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Уклоњен NPU try/catch начин заобилажења |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Крос-платформски .csproj пример |
| `AGENTS.md` | 2026-03-11 | Ажурирани C# пакети и TFM детаљи |
| `CHANGELOG.md` | 2026-03-11 | Ова датотека |