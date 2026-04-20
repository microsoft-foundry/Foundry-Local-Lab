![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Deo 11: Позивање алата са локалним моделима

> **Циљ:** Омогућите вашем локалном моделу да позива спољне функције (алате) како би прузимао податке у реалном времену, израчунавао или комуницирао са API-јима — све то приватно покрећући на вашем уређају.

## Шта је позивање алата?

Позивање алата (такође познато као **позивање функција**) омогућава језичком моделу да затражи извршавање функција које дефинишете. Уместо да погађа одговор, модел препознаје када би алат помогао и враћа структуриран захтев за извршавање вашег кода. Ваша апликација извршава функцију, шаље резултат назад, а модел укључује те податке у свој коначни одговор.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Овај образац је битан за изградњу агената који могу:

- **Претраживати ажурне податке** (временска прогноза, цене акција, упити базе података)
- **Обављати прецизне рачуне** (математика, конверзије јединица)
- **Предузимати радње** (слање е-поште, креирање тикета, ажурирање записа)
- **Приступати приватним системима** (интерни API-ји, фајл системи)

---

## Како функционише позивање алата

Ток позивања алата има четири фазе:

| Фаза | Шта се дешава |
|-------|-------------|
| **1. Дефинисање алата** | Описујете доступне функције помоћу JSON шеме — име, опис и параметри |
| **2. Модел одлучује** | Модел добија вашу поруку и дефиниције алата. Ако алат може помоћи, враћа `tool_calls` одговор уместо текстуалног одговора |
| **3. Извршавање локално** | Ваш код обрађује позив алата, извршава функцију и приказује резултат |
| **4. Коначни одговор** | Шаљете резултат алата назад моделу, који производи коначни одговор |

> **Кључна напомена:** Модел никада не извршава код. Само *захтева* да се алат позове. Ваша апликација одлучује да ли ће испунити тај захтев — на тај начин имате пуну контролу.

---

## Који модели подржавају позивање алата?

Ниједан модел не подржава позивање алата. У тренутном каталогу Foundry Local, следећи модели имају могућност позивања алата:

| Модел | Величина | Позивање алата |
|-------|----------|:--------------:|
| qwen2.5-0.5b | 822 MB | ✅ |
| qwen2.5-1.5b | 1.8 GB | ✅ |
| qwen2.5-7b | 6.3 GB | ✅ |
| qwen2.5-14b | 11.3 GB | ✅ |
| qwen2.5-coder-0.5b | 822 MB | ✅ |
| qwen2.5-coder-1.5b | 1.8 GB | ✅ |
| qwen2.5-coder-7b | 6.3 GB | ✅ |
| qwen2.5-coder-14b | 11.3 GB | ✅ |
| phi-4-mini | 4.6 GB | ✅ |
| phi-3.5-mini | 2.6 GB | ❌ |
| phi-4 | 10.4 GB | ❌ |

> **Савет:** За ову вежбу користимо **qwen2.5-0.5b** — мали је (822 MB за преузимање), брз и има поуздану подршку за позивање алата.

---

## Циљеви учења

На крају ове вежбе моћи ћете да:

- Објасните образац позивања алата и значај за AI агенте
- Дефинишете шеме алата коришћењем OpenAI формата позивања функција
- Обрађујете ток разговора са више корака за позивање алата
- Локално извршавате позиве алата и враћате резултате моделу
- Изаберете прави модел за сценарије позивања алата

---

## Предуслови

| Захтев | Детаљи |
|-------------|---------|
| **Foundry Local CLI** | Инсталиран и доступан у вашем `PATH`-у ([Deo 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript или C# SDK инсталиран ([Deo 2](part2-foundry-local-sdk.md)) |
| **Модел са подршком за позивање алата** | qwen2.5-0.5b (биће аутоматски преузет) |

---

## Вежбе

### Вежба 1 — Разумевање тока позивања алата

Пре писања кода, изучите овај дијаграм секвенци:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Кључне посматрања:**

1. Дефинишете алате унапред као JSON Schema објекте
2. Моделов одговор садржи `tool_calls` уместо уобичајеног садржаја
3. Сваком позиву алата додељен је јединствени `id` којим морате да се позивате приликом враћања резултата
4. Модел види све претходне поруке *плус* резултате алата када генерише коначни одговор
5. У једном одговору могу се догодити више позива алата

> **Дискусија:** Зашто модел враћа позиве алата уместо директног извршења функција? Које безбедносне предности то доноси?

---

### Вежба 2 — Дефинисање шема алата

Алате дефинишете користећи стандардни OpenAI формат позивања функција. Сваки алат треба да има:

- **`type`**: Увек `"function"`
- **`function.name`**: Описно име функције (нпр. `get_weather`)
- **`function.description`**: Јасан опис — модел користи да одлучи када да позове алат
- **`function.parameters`**: JSON Schema објекат који описује очекиване аргументе

```json
{
  "type": "function",
  "function": {
    "name": "get_weather",
    "description": "Get the current weather for a given city",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. London"
        }
      },
      "required": ["city"]
    }
  }
}
```

> **Најбоље праксе за описе алата:**
> - Будите конкретни: "Примaјте тренутну временску прогнозу за град" је боље од "Примaјте временске услове"
> - Јасно опишите параметре: модел чита ове описе да попуни исправне вредности
> - Обележите обавезне и опционе параметре — ово помаже моделу да одлучи шта да тражи

---

### Вежба 3 — Покретање примера позивања алата

Свак пример на језику дефинише два алата (`get_weather` и `get_population`), шаље питање које покреће коришћење алата, извршава алат локално, и шаље резултат назад за коначни одговор.

<details>
<summary><strong>🐍 Python</strong></summary>

**Предуслови:**
```bash
cd python
python -m venv venv

# Виндоус (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Линукс:
source venv/bin/activate

pip install -r requirements.txt
```

**Покретање:**
```bash
python foundry-local-tool-calling.py
```

**Очекује се излаз:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Преглед кода** (`python/foundry-local-tool-calling.py`):

```python
# Дефинишите алате као листу шема функција
tools = [
    {
        "type": "function",
        "function": {
            "name": "get_weather",
            "description": "Get the current weather for a given city",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {"type": "string", "description": "The city name"}
                },
                "required": ["city"]
            }
        }
    }
]

# Пошаљите са алатима — модел може вратити позиве алата уместо садржаја
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Проверите да ли модел жели да позове алат
if response.choices[0].message.tool_calls:
    # Извршите алат и пошаљите резултат назад
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Предуслови:**
```bash
cd javascript
npm install
```

**Покретање:**
```bash
node foundry-local-tool-calling.mjs
```

**Очекује се излаз:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Преглед кода** (`javascript/foundry-local-tool-calling.mjs`):

Овај пример користи изворни Foundry Local SDK `ChatClient` уместо OpenAI SDK, показујући погодност методе `createChatClient()`:

```javascript
// Добијте ChatClient директно из објекта модела
const chatClient = model.createChatClient();

// Пошаљите са алаткама — ChatClient обрађује OpenAI-компатибилни формат
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Провера позива алата
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Извршите алатке и пошаљите резултате назад
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Предуслови:**
```bash
cd csharp
dotnet restore
```

**Покретање:**
```bash
dotnet run toolcall
```

**Очекује се излаз:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Преглед кода** (`csharp/ToolCalling.cs`):

C# користи помоћну методу `ChatTool.CreateFunctionTool` за дефинисање алата:

```csharp
ChatTool getWeatherTool = ChatTool.CreateFunctionTool(
    functionName: "get_weather",
    functionDescription: "Get the current weather for a given city",
    functionParameters: BinaryData.FromString("""
    {
        "type": "object",
        "properties": {
            "city": { "type": "string", "description": "The city name" }
        },
        "required": ["city"]
    }
    """));

var options = new ChatCompletionOptions();
options.Tools.Add(getWeatherTool);

// Check FinishReason to see if tools were called
if (completion.Value.FinishReason == ChatFinishReason.ToolCalls)
{
    // Execute tools and send results back
    ...
}
```

</details>

---

### Вежба 4 — Ток разговора при позивању алата

Разумевање структуре порука је кључно. Ево комплетног тока, приказујући низ `messages` у свакој фази:

**Фаза 1 — Почетни захтев:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Фаза 2 — Модел одговара са tool_calls (не садржај):**
```json
{
  "role": "assistant",
  "tool_calls": [
    {
      "id": "call_abc123",
      "type": "function",
      "function": {
        "name": "get_weather",
        "arguments": "{\"city\": \"London\"}"
      }
    }
  ]
}
```

**Фаза 3 — Додајете поруку асистента И резултат алата:**
```json
[
  {"role": "system", "content": "..."},
  {"role": "user", "content": "What is the weather like in London?"},
  {"role": "assistant", "tool_calls": [...]},
  {
    "role": "tool",
    "tool_call_id": "call_abc123",
    "content": "{\"city\": \"London\", \"temperature\": \"18°C\", \"condition\": \"Partly cloudy\"}"
  }
]
```

**Фаза 4 — Модел производи коначни одговор користећи резултат алата.**

> **Важно:** `tool_call_id` у поруци о алату мора одговарати `id` из позива алата. Тако модел повезује резултате са захтевима.

---

### Вежба 5 — Више позива алата

Модел може затражити више позива алата у једном одговору. Покушајте да промените корисничку поруку да покренете више позива:

```python
# У Питону — промени корисничку поруку:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// У ЈаваСкрипт-у — промените поруку корисника:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Модел треба да врати два `tool_calls` — један за `get_weather` и један за `get_population`. Ваш код то већ обрађује јер пролази кроз све позиве алата.

> **Пробајте:** Измените корисничку поруку и поново покрените пример. Да ли модел позива оба алата?

---

### Вежба 6 — Додавање вашег алата

Проширите један од примера новим алатом. На пример, додајте алат `get_time`:

1. Дефинишите шему алата:
```json
{
  "type": "function",
  "function": {
    "name": "get_time",
    "description": "Get the current time in a given city's timezone",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. Tokyo"
        }
      },
      "required": ["city"]
    }
  }
}
```

2. Додајте логику извршавања:
```python
# Пајтон
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # У правом апликацији користите библиотеку за временску зону
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... постојећи алати ...
```

3. Додајте алат у низ `tools` и тестирате питањем: "Колико је сати у Токију?"

> **Изазов:** Додајте алат који извршава израчун, као што је `convert_temperature` за конверзију између Целзијуса и Фаренхајта. Тестирајте га питањем: "Конвертуј 100°F у Целзијус."

---

### Вежба 7 — Позивање алата са SDK ChatClient (JavaScript)

JavaScript пример већ користи изворни SDK `ChatClient` уместо OpenAI SDK. Ово је погодност која уклања потребу да сами конструишете OpenAI клијента:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient се прави директно из модела објекта
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat прихвата алате као други параметар
const response = await chatClient.completeChat(messages, tools);
```

Упоредите ово са приступом у Python-у који експлицитно користи OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Оба образаца су валидна. `ChatClient` је практичнији; OpenAI SDK вам даје приступ свим параметрима OpenAI-а.

> **Пробајте:** Измените JavaScript пример да користи OpenAI SDK уместо `ChatClient`. Биће вам потребан `import OpenAI from "openai"` и конструисање клијента са крајном тачком из `manager.urls[0]`.

---

### Вежба 8 — Разумевање параметра tool_choice

Параметар `tool_choice` контролише да ли модел *мора* да користи алат или може слободно да бира:

| Вредност | Понашање |
|----------|----------|
| `"auto"` | Модел одлучује да ли ће позвати алат (подразумевано) |
| `"none"` | Модел неће позивати алате, чак и ако су обезбеђени |
| `"required"` | Модел мора позвати најмање један алат |
| `{"type": "function", "function": {"name": "get_weather"}}` | Модел мора позвати специфични алат |

Испробајте све опције у Python примеру:

```python
# Приморате модел да позове get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Напомена:** Не све опције `tool_choice` морају бити подржане од стране сваког модела. Ако модел не подржава `"required"`, може игнорисати ту поставку и понашати се као да је `"auto"`.

---

## Чести проблеми

| Проблем | Решење |
|---------|---------|
| Модел никад не позива алате | Проверите да користите модел са подршком за позивање алата (нпр. qwen2.5-0.5b). Погледајте табелу изнад. |
| Неслагање `tool_call_id` | Увек користите `id` из одговора са позивом алата, не тврдо кодиране вредности |
| Модел враћа неисправан JSON у `arguments` | Мањи модели понекад генеришу неважећи JSON. Окружите `JSON.parse()` блоком за хватање грешака |
| Модел позива алат који не постоји | Додајте подразумевани руковалац у вашој функцији `execute_tool` |
| Бесконачна петља позива алата | Поставите максималан број рунди (нпр. 5) да спречите бесконачне петље |

---

## Кључни закључци

1. **Позивање алата** омогућава моделима да захтевају извршавање функција уместо да погађају одговоре
2. Модел **никада не извршава код**; ваша апликација одлучује шта ће се покренути
3. Алати су дефинисани као **JSON Schema** објекти у складу са OpenAI форматом позивања функција
4. Разговор користи **вишекратни образац**: корисник, па асистент (tool_calls), па алат (резултати), па асистент (коначни одговор)
5. Увек користите **модел који подржава позивање алата** (Qwen 2.5, Phi-4-mini)
6. SDK-ова функција `createChatClient()` пружа практичан начин за позиве алата без потребе за конструисањем OpenAI клијента

---

Наставите на [Deo 12: Изградња веб корисничког сучеља за Zava Creative Writer](part12-zava-ui.md) да додате веб интерфејс више агената са стримингом у реалном времену.

---

[← Deo 10: Прилагођени модели](part10-custom-models.md) | [Deo 12: Zava Writer UI →](part12-zava-ui.md)