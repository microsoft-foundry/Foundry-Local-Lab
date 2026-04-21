![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Част 11: Извикване на инструменти с локални модели

> **Цел:** Позволете на вашия локален модел да извиква външни функции (инструменти), за да може да получава данни в реално време, да извършва изчисления или да взаимодейства с API-та — всичко това, изпълнявано частно на вашето устройство.

## Какво е Извикване на Инструменти?

Извикването на инструменти (известно още като **function calling**) позволява на езиков модел да поиска изпълнението на функции, които вие дефинирате. Вместо да гадае отговора, моделът разпознава кога инструмент би бил полезен и връща структурирана заявка за изпълнение от вашия код. Вашето приложение изпълнява функцията, изпраща резултата обратно и моделът включва тази информация в крайния си отговор.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Този модел е съществен за изграждане на агенти, които могат да:

- **Търсят актуални данни** (време, цени на акции, заявки към база данни)
- **Извършват точни изчисления** (математика, конвертиране на мерки)
- **Извършват действия** (изпращане на имейли, създаване на билети, обновяване на записи)
- **Достъпват частни системи** (вътрешни API-та, файлови системи)

---

## Как Работи Извикването на Инструменти

Потокът на извикване на инструменти има четири етапа:

| Етап | Какво се случва |
|-------|-------------|
| **1. Дефиниране на инструменти** | Описвате наличните функции чрез JSON Schema — име, описание и параметри |
| **2. Моделът решава** | Моделът получава вашето съобщение плюс дефинициите на инструментите. Ако инструмент би помогнал, връща отговор `tool_calls` вместо текстов |
| **3. Изпълнение локално** | Вашият код обработва извикването на инструмента, изпълнява функцията и събира резултата |
| **4. Краен отговор** | Изпращате резултата от инструмента обратно към модела, който генерира крайния отговор |

> **Ключов момент:** Моделът никога не изпълнява код. Той само *иска* инструмент да бъде извикан. Вашето приложение решава дали да удовлетвори тази заявка — това ви държи под пълен контрол.

---

## Кои Модели Поддържат Извикване на Инструменти?

Не всеки модел поддържа извикване на инструменти. В текущия каталог на Foundry Local следните модели имат тази възможност:

| Модел | Размер | Извикване на инструменти |
|-------|------|:---:|
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

> **Съвет:** За тази лаборатория използваме **qwen2.5-0.5b** — малък (822 MB изтегляне), бърз и с надеждна поддръжка на извикване на инструменти.

---

## Учебни Цели

Към края на тази лаборатория ще можете да:

- Обясните модела за извикване на инструменти и защо е важен за AI агенти
- Дефинирате схеми на инструменти с формата на OpenAI function-calling
- Работите с многоходова конверсация за извикване на инструменти
- Изпълнявате извиквания на инструменти локално и връщате резултатите към модела
- Избирате правилния модел за сценарии с извикване на инструменти

---

## Предварителни Изисквания

| Изискване | Детайли |
|-------------|---------|
| **Foundry Local CLI** | Инсталиран и наличен в `PATH` ([Част 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Инсталиран Python, JavaScript или C# SDK ([Част 2](part2-foundry-local-sdk.md)) |
| **Модел с възможност за извикване на инструменти** | qwen2.5-0.5b (ще се изтегли автоматично) |

---

## Упражнения

### Упражнение 1 — Разберете потока на извикване на инструменти

Преди да пишете код, изучете тази секвенционна диаграма:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Ключови наблюдения:**

1. Дефинирате инструментите предварително като JSON Schema обекти
2. Отговорът на модела съдържа `tool_calls` вместо обичайното съдържание
3. Всяко извикване на инструмент има уникален `id`, който трябва да използвате при връщане на резултатите
4. Моделът вижда всички предишни съобщения *плюс* резултатите от инструментите при генериране на крайния отговор
5. В един отговор могат да се случат множество извиквания на инструменти

> **Обсъждане:** Защо моделът връща извиквания на инструменти вместо директно изпълнение на функции? Какви предимства за сигурността това дава?

---

### Упражнение 2 — Дефиниране на схеми на инструменти

Инструментите се дефинират с използване на стандартния формат за извикване на функции на OpenAI. Всеки инструмент трябва да съдържа:

- **`type`**: Винаги `"function"`
- **`function.name`**: Описателно име на функция (например `get_weather`)
- **`function.description`**: Ясно описание — моделът го използва, за да реши кога да извика инструмента
- **`function.parameters`**: JSON Schema обект, описващ очакваните аргументи

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

> **Добри практики за описания на инструменти:**
> - Бъдете конкретни: "Вземи текущото време за даден град" е по-добро от "Вземи времето"
> - Описвайте параметрите ясно: моделът чете тези описания, за да попълни правилните стойности
> - Отбелязвайте кои параметри са задължителни и кои са опционални — това помага на модела да реши какво да поиска

---

### Упражнение 3 — Стартирайте примерите за извикване на инструменти

Всеки пример на език дефинира два инструмента (`get_weather` и `get_population`), изпраща въпрос, който задейства използване на инструмента, изпълнява инструмента локално и изпраща резултата за краен отговор.

<details>
<summary><strong>🐍 Python</strong></summary>

**Предварителни изисквания:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Стартиране:**
```bash
python foundry-local-tool-calling.py
```

**Очакван изход:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Обяснение на кода** (`python/foundry-local-tool-calling.py`):

```python
# Определете инструментите като списък от схеми на функции
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

# Изпратете с инструментите — моделът може да върне tool_calls вместо съдържание
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Проверете дали моделът иска да извика инструмент
if response.choices[0].message.tool_calls:
    # Изпълнете инструмента и изпратете резултата обратно
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Предварителни изисквания:**
```bash
cd javascript
npm install
```

**Стартиране:**
```bash
node foundry-local-tool-calling.mjs
```

**Очакван изход:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Обяснение на кода** (`javascript/foundry-local-tool-calling.mjs`):

Този пример използва нативния `ChatClient` от Foundry Local SDK вместо OpenAI SDK, демонстрирайки удобството на метода `createChatClient()`:

```javascript
// Вземете ChatClient директно от обекта на модела
const chatClient = model.createChatClient();

// Изпращане с инструменти — ChatClient обработва формата, съвместим с OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Проверка за извиквания на инструменти
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Изпълнение на инструменти и връщане на резултатите назад
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Предварителни изисквания:**
```bash
cd csharp
dotnet restore
```

**Стартиране:**
```bash
dotnet run toolcall
```

**Очакван изход:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Обяснение на кода** (`csharp/ToolCalling.cs`):

C# използва помощника `ChatTool.CreateFunctionTool` за дефиниране на инструменти:

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

### Упражнение 4 — Потокът на конверсацията при извикване на инструменти

Разбирането на структурата на съобщенията е критично. Ето пълният поток, показващ масива `messages` на всеки етап:

**Етап 1 — Начална заявка:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Етап 2 — Моделът отговаря с `tool_calls` (без съдържание):**
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

**Етап 3 — Вие добавяте съобщението на асистента И резултата от инструмента:**
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

**Етап 4 — Моделът генерира крайния отговор, използвайки резултата от инструмента.**

> **Важно:** `tool_call_id` в съобщението на инструмента трябва да съвпада с `id` от извикването на инструмента. Това е как моделът асоциира резултатите със заявките.

---

### Упражнение 5 — Множество Извиквания на Инструменти

Моделът може да изиска няколко извиквания на инструменти в един отговор. Опитайте да промените потребителското съобщение, за да задействате множество извиквания:

```python
# В Python — променете съобщението на потребителя:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// В JavaScript — променете съобщението до потребителя:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Моделът трябва да върне две `tool_calls` — една за `get_weather` и една за `get_population`. Вашият код вече ги обработва, тъй като преминава през всички извиквания.

> **Опитайте:** Променете съобщението и стартирайте примера отново. Моделът извиква ли двата инструмента?

---

### Упражнение 6 — Добавете Собствен Инструмент

Разширете един от примерите с нов инструмент. Например добавете инструмент `get_time`:

1. Дефинирайте схемата на инструмента:
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

2. Добавете логиката за изпълнение:
```python
# Питон
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # В реално приложение използвайте библиотека за часови зони
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... съществуващи инструменти ...
```

3. Добавете инструмента в масива `tools` и тествайте с: "Колко е часът в Токио?"

> **Предизвикателство:** Добавете инструмент, който извършва изчисление, като `convert_temperature`, който конвертира между Целзий и Фаренхайт. Тествайте го с: "Конвертирай 100°F в Целзий."

---

### Упражнение 7 — Извикване на Инструменти с ChatClient на SDK (JavaScript)

JavaScript примерът вече използва native `ChatClient` от SDK, вместо OpenAI SDK. Това е удобна функция, която премахва нуждата сами да конфигурирате OpenAI клиент:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient се създава директно от обекта модел
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat приема инструменти като втори параметър
const response = await chatClient.completeChat(messages, tools);
```

Сравнете го с подхода на Python, който експлицитно използва OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

И двата модела са валидни. `ChatClient` е по-удобен; OpenAI SDK предоставя пълен достъп до всички параметри на OpenAI.

> **Опитайте:** Модифицирайте JavaScript примера да използва OpenAI SDK вместо `ChatClient`. Ще трябва да импортирате `OpenAI` от "openai" и да създадете клиент с крайна точка от `manager.urls[0]`.

---

### Упражнение 8 — Разбиране на `tool_choice`

Параметърът `tool_choice` контролира дали моделът *трябва* да използва инструмент или може да избира свободно:

| Стойност | Поведение |
|-------|-----------|
| `"auto"` | Моделът решава дали да извика инструмент (по подразбиране) |
| `"none"` | Моделът няма да извика инструменти, дори ако са предоставени |
| `"required"` | Моделът трябва да извика поне един инструмент |
| `{"type": "function", "function": {"name": "get_weather"}}` | Моделът трябва да извика посочения инструмент |

Опитайте всяка опция в Python примера:

```python
# Принуди модела да извика get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Забележка:** Не всички опции на `tool_choice` могат да се поддържат от всеки модел. Ако модел не поддържа `"required"`, може да игнорира настройката и да се държи като при `"auto"`.

---

## Често Срещани Проблеми

| Проблем | Решение |
|---------|----------|
| Моделът никога не извиква инструменти | Убедете се, че използвате модел с възможност за извикване на инструменти (например qwen2.5-0.5b). Проверете таблицата по-горе. |
| Несъответствие на `tool_call_id` | Винаги използвайте `id` от отговора с извикването на инструмента, не фиксирана стойност |
| Модел връща невалиден JSON в `arguments` | По-малките модели понякога генерират невалиден JSON. Обвийте `JSON.parse()` в try/catch |
| Модел извиква несъществуващ инструмент | Добавете стандартен обработчик в `execute_tool` функцията ви |
| Безкраен цикъл на извикване на инструменти | Задайте максимален брой рундове (напр. 5), за да предотвратите безкрайни цикли |

---

## Основни Изводи

1. **Извикването на инструменти** позволява на моделите да искат изпълнение на функции, вместо да гадаят отговори
2. Моделът **никога не изпълнява код**; вашето приложение решава какво да стартира
3. Инструментите се дефинират като **JSON Schema** обекти, следвайки формата OpenAI function-calling
4. Конверсацията използва **многоходов модел**: потребител, след това асистент (tool_calls), после инструмент (резултати) и след това асистент (краен отговор)
5. Винаги използвайте **модел, който поддържа извикване на инструменти** (Qwen 2.5, Phi-4-mini)
6. `createChatClient()` на SDK предоставя удобен начин за правене на заявки с извикване на инструменти без конструкция на OpenAI клиент

---

Продължете към [Част 12: Създаване на Web UI за Zava Creative Writer](part12-zava-ui.md), за да добавите браузър-базирана фронт-енд част към мултиагентния пайплайн с реално време стрийминг.

---

[← Част 10: Потребителски Модели](part10-custom-models.md) | [Част 12: Потребителски Интерфейс на Zava Writer →](part12-zava-ui.md)