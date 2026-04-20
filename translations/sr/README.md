<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local радионица - Прављење AI апликација на уређају

Практична радионица за покретање језичких модела на вашем рачунару и креирање интелигентних апликација помоћу [Foundry Local](https://foundrylocal.ai) и [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Шта је Foundry Local?** Foundry Local је лагано окружење за извршавање које вам омогућава да преузмете, управљате и пружате језичке моделе у потпуности на вашем хардверу. Означава **OpenAI-компатибилан API** тако да било који алат или SDK који користи OpenAI може да се повеже - није потребан рачун у облаку.

---

## Циљеви учења

На крају ове радионице моћи ћете да:

| # | Циљ |
|---|-----|
| 1 | Инсталирате Foundry Local и управљате моделима помоћу CLI |
| 2 | Савладате Foundry Local SDK API за програмско управљање моделима |
| 3 | Повежете се са локалним inference сервером користећи Python, JavaScript и C# SDK-ове |
| 4 | Конструишете Retrieval-Augmented Generation (RAG) пипелaјн који заснива одговоре на вашим сопственим подацима |
| 5 | Креирате AI агенте са перзистентним упутствима и персоналитетима |
| 6 | Оркестрирате мулти-агентске токове рада са петљама повратне информације |
| 7 | Истражите производну завршну апликацију - Zava Creative Writer |
| 8 | Направите оквир за евалуацију са златним скупом података и LLM-as-judge бодовањем |
| 9 | Транскрибујете звук помоћу Whisper - претварање говора у текст на уређају користећи Foundry Local SDK |
| 10 | Компилујете и извршавате прилагођене или Hugging Face моделе помоћу ONNX Runtime GenAI и Foundry Local |
| 11 | Омогућите локалним моделима да позивају екстерне функције помоћу обрасца за позив алата |
| 12 | Направите кориснички интерфејс у претраживачу за Zava Creative Writer са стримингом у реалном времену |

---

## Предуслови

| Захтев | Детаљи |
|--------|---------|
| **Хардвер** | Минимум 8 GB RAM-а (препоручено 16 GB); CPU са подршком за AVX2 или подржани GPU |
| **ОС** | Windows 10/11 (x64/ARM), Windows Server 2025, или macOS 13+ |
| **Foundry Local CLI** | Инсталирајте путем `winget install Microsoft.FoundryLocal` (Windows) или `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Погледајте [водич за почетак](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) за детаље. |
| **Језичко окружење** | **Python 3.9+** и/или **.NET 9.0+** и/или **Node.js 18+** |
| **Git** | За клонирање овог репозиторијума |

---

## Почетак рада

```bash
# 1. Клонирај репозиторијум
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Проверите да ли је Foundry Local инсталиран
foundry model list              # Прикажи доступне моделе
foundry model run phi-3.5-mini  # Покрени интерактивни чет

# 3. Изаберите свој језички правац (погледајте лабораторију дела 2 за пуну подешавање)
```

| Језик | Брзи почетак |
|-------|----------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Делови радионице

### Део 1: Почетак рада са Foundry Local

**Водич за лабораторију:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Шта је Foundry Local и како функционише
- Инсталирање CLI на Windows и macOS
- Истраживање модела - листање, преузимање, покретање
- Разумевање имена модела и динамичких портова

---

### Део 2: Дубински преглед Foundry Local SDK-а

**Водич за лабораторију:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Зашто користити SDK уместо CLI за развој апликација
- Комплетна SDK API референца за Python, JavaScript и C#
- Управљање сервисом, преглед каталога, животни циклус модела (преузимање, учитавање, ослобађање)
- Обрасци за брзи почетак: Python bootstrap конструктор, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` метаподати, алтернативни називи и одабир оптималног модела за хардвер

---

### Део 3: SDK-ови и API-ји

**Водич за лабораторију:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Повезивање на Foundry Local са Python-ом, JavaScript-ом и C#-ом
- Коришћење Foundry Local SDK-а за програмско управљање сервисом
- Стриминг chat завршетака преко OpenAI-компатибилног API-ја
- Референца SDK метода за сваки језик

**Примери кода:**

| Језик | Фајл | Опис |
|-------|------|------|
| Python | `python/foundry-local.py` | Основни стриминг чат |
| C# | `csharp/BasicChat.cs` | Стриминг чат са .NET |
| JavaScript | `javascript/foundry-local.mjs` | Стриминг чат са Node.js |

---

### Део 4: Retrieval-Augmented Generation (RAG)

**Водич за лабораторију:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Шта је RAG и зашто је важан
- Креирање знања у меморији
- Претрага заснована на поклапању кључних речи са бодовањем
- Компоновaње утемељених системских упутстава
- Извођење комплетног RAG пипелaјна на уређају

**Примери кода:**

| Језик | Фајл |
|-------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Део 5: Креирање AI агената

**Водич за лабораторију:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Шта је AI агент (у односу на директни позив LLM-а)
- Образац `ChatAgent` и Microsoft Agent Framework
- Системска упутства, персоналитети и вишекратни дијалози
- Структурирани излаз (JSON) од агената

**Примери кода:**

| Језик | Фајл | Опис |
|-------|------|------|
| Python | `python/foundry-local-with-agf.py` | Један агент са Agent Framework-ом |
| C# | `csharp/SingleAgent.cs` | Један агент (ChatAgent образац) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Један агент (ChatAgent образац) |

---

### Део 6: Мулти-агентски токови рада

**Водич за лабораторију:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Мулти-агентски пипелaјни: Истраживач → Писац → Уредник
- Секвенцијална оркестрација и петље повратне информације
- Заједничка конфигурација и структурирани преноси задатака
- Дизајнирање свог мулти-агентског тока рада

**Примери кода:**

| Језик | Фајл | Опис |
|-------|------|------|
| Python | `python/foundry-local-multi-agent.py` | Пипелaјн са три агента |
| C# | `csharp/MultiAgent.cs` | Пипелaјн са три агента |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Пипелaјн са три агента |

---

### Део 7: Zava Creative Writer - Завршна апликација

**Водич за лабораторију:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Производна апликација са више агената и 4 специјализована агента
- Секвенцијални пипелaјн са петљама повратне информације вођеним оцењивачем
- Стриминг излаз, претрага каталога производа, структурирани JSON задаци
- Потпуна имплементација у Python-у (FastAPI), JavaScript-у (Node.js CLI) и C#-у (.NET конзола)

**Примери кода:**

| Језик | Директоријум | Опис |
|-------|--------------|------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI веб сервис са оркестратором |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI апликација |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 конзолна апликација |

---

### Део 8: Развој вођен евалуацијом

**Водич за лабораторију:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Направите системски оквир за евалуацију AI агената користећи златне скупове података
- Правилом засноване провере (дужина, покривеност кључних речи, забрањени термини) + бодовање LLM-as-judge
- Упоредна анализа варијанти упутстава са агрегираним резултатима
- Проширење обрасца Zava Editor агента из дела 7 у офлајн тестни скуп
- Пратећи садржаји за Python, JavaScript и C#

**Примери кода:**

| Језик | Фајл | Опис |
|-------|------|------|
| Python | `python/foundry-local-eval.py` | Оквир за евалуацију |
| C# | `csharp/AgentEvaluation.cs` | Оквир за евалуацију |
| JavaScript | `javascript/foundry-local-eval.mjs` | Оквир за евалуацију |

---

### Део 9: Транскрипција гласа са Whisper

**Водич за лабораторију:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Претварање говора у текст коришћењем OpenAI Whisper који ради локално
- Претходност приватности у обради звука - звук никада не напушта уређај
- Пратећи садржаји за Python, JavaScript и C# са `client.audio.transcriptions.create()` (Python/JS) и `AudioClient.TranscribeAudioAsync()` (C#)
- Укључује примерке аудио фајлова са Zava темом за практичну вежбу

**Примери кода:**

| Језик | Фајл | Опис |
|-------|------|------|
| Python | `python/foundry-local-whisper.py` | Whisper транскрипција гласа |
| C# | `csharp/WhisperTranscription.cs` | Whisper транскрипција гласа |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper транскрипција гласа |

> **Напомена:** Ова лабораторија користи **Foundry Local SDK** за програмско преузимање и учитавање Whisper модела, након чега шаље аудио на локални OpenAI-компатибилан крај за транскрипцију. Whisper модел (`whisper`) се налази у каталогу Foundry Local и у потпуности ради на уређају - није потребна cloud API кључеви или приступ мрежи.

---

### Део 10: Коришћење прилагођених или Hugging Face модела

**Водич за лабораторију:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Компилација Hugging Face модела у оптимизовани ONNX формат помоћу ONNX Runtime GenAI мод билдера
- Компилација специфична за хардвер (CPU, NVIDIA GPU, DirectML, WebGPU) и квантизација (int4, fp16, bf16)
- Креирање конфигурационих фајлова chat-обрасца за Foundry Local
- Додавање компајлираних модела у кеш Foundry Local
- Покретање прилагођених модела преко CLI, REST API и OpenAI SDK
- Пример у пракси: компилација Qwen/Qwen3-0.6B крај-до-краја

---

### Део 11: Позивање алата уз локалне моделе

**Водич за лабораторију:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Омогућите локалним моделима позивање екстерних функција (позивање алата/функција)
- Дефинисање шема алата користећи OpenAI формат за позивање функција
- Управљање разговором са више корака у позивању алата
- Извршавање позива алата локално и враћање резултата моделу
- Избор правог модела за сценарије позивања алата (Qwen 2.5, Phi-4-mini)
- Коришћење нативног `ChatClient` у SDK-у за позивање алата (JavaScript)

**Примери кода:**

| Језик | Фајл | Опис |
|-------|------|------|
| Python | `python/foundry-local-tool-calling.py` | Позивање алата за време и популацију |
| C# | `csharp/ToolCalling.cs` | Позивање алата са .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Позивање алата са ChatClient |

---

### Део 12: Прављење веб UI за Zava Creative Writer

**Водич за лабораторију:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Додајте веб интерфејс за Zava Creative Writer
- Сервисирање заједничког UI са Python-а (FastAPI), JavaScript-а (Node.js HTTP) и C# (ASP.NET Core)
- Конзумирање стримовања NDJSON у претраживачу помоћу Fetch API-а и ReadableStream-а
- Ознаке статуса агента уживо и стриминг текста чланака у реалном времену

**Код (заједнички UI):**

| Фајл | Опис |
|-------|------|
| `zava-creative-writer-local/ui/index.html` | Лаиаут странице |
| `zava-creative-writer-local/ui/style.css` | Стилови |
| `zava-creative-writer-local/ui/app.js` | Логика читања стрима и ажурирања DOM-а |

**Додаци за Бекенд:**

| Језик | Фајл | Опис |
|-------|------|------|
| Python | `zava-creative-writer-local/src/api/main.py` | Ажурирано за сервисирање статичког UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Нови HTTP сервер који омотава оркестратор |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Нови ASP.NET Core minimal API пројекат |

---

### Део 13: Радионица завршена
**Водич за лабораторију:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Резиме свега што сте направили кроз свих 12 делова
- Даље идеје за проширење ваших апликација
- Линкови ка ресурсима и документацији

---

## Структура пројекта

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

| Ресурс | Линк |
|----------|------|
| Foundry Local вебсајт | [foundrylocal.ai](https://foundrylocal.ai) |
| Каталог модела | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local на GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Упутство за почетак | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK референца | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Лиценца

Овај материјал радионице се пружа у сврху образовања.

---

**Срећно у изради! 🚀**