# Упутства за кодирање агента

Овај фајл пружа контекст за AI кодирање агенте (GitHub Copilot, Copilot Workspace, Codex, итд.) који раде у овом репозиторијуму.

## Преглед пројекта

Ово је **практични радионичарски рад** за изградњу AI апликација са [Foundry Local](https://foundrylocal.ai) — лаком покретачем који преузима, управља и сервира језичке моделе у потпуности на уређају преко OpenAI- компатибилног API-ја. Радионица укључује корак-по-корак лабораторијске водиче и покретачке примерке кода у Python-у, JavaScript-у и C#-у.

## Структура репозиторијума

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

## Детаљи језика и оквира

### Python
- **Локација:** `python/`, `zava-creative-writer-local/src/api/`
- **Зависности:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Кључне пакете:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Минимална верзија:** Python 3.9+
- **Покретање:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Локација:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Зависности:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Кључне пакете:** `foundry-local-sdk`, `openai`
- **Систем модула:** ES модули (`.mjs` фајлови, `"type": "module"`)
- **Минимална верзија:** Node.js 18+
- **Покретање:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Локација:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Фајлови пројекта:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Кључне пакете:** `Microsoft.AI.Foundry.Local` (не-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — надскуп са QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Циљ:** .NET 9.0 (условни TFM: `net9.0-windows10.0.26100` на Windows-у, `net9.0` иначе)
- **Покретање:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Конвенције кодирања

### Опште
- Сви примери кода су **самостални примери у једном фајлу** — нема заједничких библиотека или абстракција.
- Сваке пример се покреће независно након инсталације својих зависности.
- API кључеви су увек постављени на `"foundry-local"` — Foundry Local користи ово као резервисану вредност.
- Основни URL-ови користе `http://localhost:<port>/v1` — порт је динамичан и открива се током извршавања преко SDK-а (`manager.urls[0]` у JS, `manager.endpoint` у Python-у).
- Foundry Local SDK управља покретањем сервиса и откривањем ендпоинта; преферирајте SDK образце уместо фиксних портова.

### Python
- Користити `openai` SDK са `OpenAI(base_url=..., api_key="not-required")`.
- Користити `FoundryLocalManager()` из `foundry_local` за SDK-ом управљани животни циклус сервиса.
- Стреаминг: итерација преко `stream` објекта са `for chunk in stream:`.
- Нема типских анотација у примерима (одржати примере концизном за учеснике радионице).

### JavaScript
- ES модулски синтакс: `import ... from "..."`.
- Користити `OpenAI` из `"openai"` и `FoundryLocalManager` из `"foundry-local-sdk"`.
- Образац иницијализације SDK-а: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Стреаминг: `for await (const chunk of stream)`.
- Топ-левел `await` коришћен кроз целу базу.

### C#
- Nullable је омогућено, implicit usings, .NET 9.
- Користити `FoundryLocalManager.StartServiceAsync()` за SDK-ом управљани животни циклус.
- Стреаминг: `CompleteChatStreaming()` са `foreach (var update in completionUpdates)`.
- Главни `csharp/Program.cs` је CLI рутер који позива статичке `RunAsync()` методе.

### Позив алата
- Само одређени модели подржавају позив алата: **Qwen 2.5** породица (`qwen2.5-*`) и **Phi-4-mini** (`phi-4-mini`).
- Шеме алата прате OpenAI JSON формат за позив функција (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Конверзација користи мулти-турн образац: корисник → асистент (tool_calls) → алат (резултати) → асистент (коначни одговор).
- `tool_call_id` у порукама са резултатом алата мора одговарати `id` из позива алата модела.
- Python користи OpenAI SDK директно; JavaScript користи нативни SDK `ChatClient` (`model.createChatClient()`); C# користи OpenAI SDK са `ChatTool.CreateFunctionTool()`.

### ChatClient (Нативни SDK клијент)
- JavaScript: `model.createChatClient()` враћа `ChatClient` са `completeChat(messages, tools?)` и `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` враћа стандардни `ChatClient` који се може користити без увоза OpenAI NuGet пакета.
- Python нема нативни ChatClient — користити OpenAI SDK са `manager.endpoint` и `manager.api_key`.
- **Важно:** ЈаваСкрипт `completeStreamingChat` користи **позив по позиву (callback) образац**, а не асинхрони итератор.

### Размишљајући модели
- `phi-4-mini-reasoning` умотава размишљање у `<think>...</think>` тагове пре коначног одговора.
- Парсирајте тагове да одвојите размишљање од одговора када је потребно.

## Лабораторијски Водичи

Лабораторијски фајлови су у `labs/` као Markdown. Следе доследну структуру:
- Заглавље са логом
- Наслов и позив на циљ
- Преглед, циљеви учења, предуслови
- Секције са објашњењем концепата и дијаграмима
- Нумерисани задаци са кодом и очекиваним излазом
- Резиме табела, кључни закључци, додатна литература
- Линк за навигацију до наредног дела

Када уређујете лабораторијски садржај:
- Одржати постојећи Markdown стил форматирања и хијерархију секција.
- Код блокови морају да дефинишу језик (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Пружити и баш и PowerShell варијанте за shell команде где је ОС важно.
- Користити стилове позива: `> **Напомена:**`, `> **Савет:**`, и `> **Троке:**`.
- Табеле се користе у формату са `| Заглавље | Заглавље |`.

## Команде за грађење и тестирање

| Акција | Команда |
|--------|---------|
| **Python примери** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS примери** | `cd javascript && npm install && node <script>.mjs` |
| **C# примери** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Генерисање дијаграма** | `npx mmdc -i <input>.mmd -o <output>.svg` (захтева root `npm install`) |

## Спољне зависности

- **Foundry Local CLI** мора бити инсталиран на рачунару програмера (`winget install Microsoft.FoundryLocal` или `brew install foundrylocal`).
- **Foundry Local сервис** ради локално и излаже OpenAI компатибилан REST API на динамичком порту.
- Није потребно коришћење cloud сервиса, API кључева или Azure претплата да би се покренуо било који пример.
- Део 10 (прилагођени модели) додатно захтева `onnxruntime-genai` и преузима тежине модела са Hugging Face.

## Фајлови који НЕ смеју бити објављени у репозиторијум

`.gitignore` искључује (и јесте искључено у већини случајева):
- `.venv/` — Python виртуелна окружења
- `node_modules/` — npm зависности
- `models/` — компајлирани ONNX модел излаз (велики бинарни фајлови, генерисани у делу 10)
- `cache_dir/` — кеш за преузимање модела са Hugging Face
- `.olive-cache/` — радни директоријум Microsoft Olive
- `samples/audio/*.wav` — генерисани аудио примери (поновно генерисани преко `python samples/audio/generate_samples.py`)
- Стандардни Python build артефакти (`__pycache__/`, `*.egg-info/`, `dist/`, итд.)

## Лиценца

MIT — види `LICENSE`.