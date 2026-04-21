![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Частина 11: Виклик інструментів за допомогою локальних моделей

> **Мета:** Дозволити вашій локальній моделі викликати зовнішні функції (інструменти), щоб вона могла отримувати дані в режимі реального часу, виконувати обчислення або взаємодіяти з API — все це працює приватно на вашому пристрої.

## Що таке виклик інструментів?

Виклик інструментів (також відомий як **function calling**) дозволяє мовній моделі запитувати виконання функцій, які ви визначаєте. Замість того, щоб гадати відповідь, модель розпізнає, коли інструмент міг би допомогти, і повертає структурований запит для виконання вашим кодом. Ваш додаток виконує функцію, надсилає результат назад, і модель включає цю інформацію у свій остаточний відповідь.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Цей шаблон необхідний для побудови агентів, які можуть:

- **Шукати актуальні дані** (погода, ціни на акції, запити в базу даних)
- **Виконувати точні розрахунки** (математика, перетворення одиниць)
- **Вживати дії** (відправляти електронні листи, створювати заявки, оновлювати записи)
- **Отримувати доступ до приватних систем** (внутрішні API, файлові системи)

---

## Як працює виклик інструментів

Процес виклику інструментів має чотири етапи:

| Етап | Що відбувається |
|-------|-------------|
| **1. Визначте інструменти** | Ви описуєте доступні функції за допомогою JSON Schema — ім'я, опис і параметри |
| **2. Модель приймає рішення** | Модель отримує ваше повідомлення разом із визначеннями інструментів. Якщо інструмент допоможе, вона повертає `tool_calls` замість текстової відповіді |
| **3. Виконання локально** | Ваш код розбирає виклик інструмента, виконує функцію і збирає результат |
| **4. Остаточна відповідь** | Ви надсилаєте результат інструмента назад моделі, яка формує кінцеву відповідь |

> **Ключовий момент:** Модель ніколи не виконує код. Вона тільки *запитує* виклик інструмента. Ваш додаток вирішує, чи виконувати цей запит — це дає вам повний контроль.

---

## Які моделі підтримують виклик інструментів?

Не всі моделі підтримують виклик інструментів. У поточному каталозі Foundry Local наступні моделі мають цю можливість:

| Модель | Розмір | Виклик інструментів |
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

> **Порада:** Для цієї лабораторної роботи ми використовуємо **qwen2.5-0.5b** — вона мала (822 МБ для завантаження), швидка і має надійну підтримку виклику інструментів.

---

## Навчальні цілі

До кінця цієї лабораторної роботи ви зможете:

- Пояснити шаблон виклику інструментів і чому він важливий для AI агентів
- Визначати схеми інструментів за допомогою формату OpenAI function-calling
- Обробляти багатокроковий діалог із викликом інструментів
- Локально виконувати виклики інструментів і повертати результати моделі
- Вибирати правильну модель для сценаріїв виклику інструментів

---

## Передумови

| Вимога | Деталі |
|-------------|---------|
| **Foundry Local CLI** | Встановлений і доступний у вашому `PATH` ([Частина 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Встановлений Python, JavaScript або C# SDK ([Частина 2](part2-foundry-local-sdk.md)) |
| **Модель з підтримкою виклику інструментів** | qwen2.5-0.5b (буде завантажена автоматично) |

---

## Вправи

### Вправа 1 — Зрозуміти процес виклику інструментів

Перед написанням коду вивчіть цю діаграму послідовності:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Головні спостереження:**

1. Ви визначаєте інструменти заздалегідь як об’єкти JSON Schema
2. Відповідь моделі містить `tool_calls` замість звичайного контенту
3. Кожний виклик інструмента має унікальний `id`, який потрібно використовувати при поверненні результатів
4. Модель бачить усі попередні повідомлення *плюс* результати інструментів при формуванні остаточної відповіді
5. Кілька викликів інструментів можуть відбутися в одній відповіді

> **Обговорення:** Чому модель повертає виклики інструментів замість прямого виконання функцій? Які переваги з безпеки це дає?

---

### Вправа 2 — Визначення схем інструментів

Інструменти визначаються за стандартним форматом OpenAI function-calling. Кожен інструмент потребує:

- **`type`**: Завжди `"function"`
- **`function.name`**: Описове ім'я функції (наприклад, `get_weather`)
- **`function.description`**: Чіткий опис — модель використовує це, щоб вирішити, коли викликати інструмент
- **`function.parameters`**: Об'єкт JSON Schema, що описує очікувані аргументи

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

> **Кращі практики для опису інструментів:**
> - Будьте конкретними: "Отримати поточну погоду для заданого міста" краще ніж "Отримати погоду"
> - Чітко опишіть параметри: модель читає ці описи для правильного заповнення значень
> - Визначайте обов’язкові та необов’язкові параметри — це допомагає моделі вирішувати, що запитувати

---

### Вправа 3 — Запустіть приклади виклику інструментів

Кожен приклад мови визначає два інструменти (`get_weather` та `get_population`), надсилає питання, яке ініціює використання інструмента, виконує інструмент локально та надсилає результат назад для остаточної відповіді.

<details>
<summary><strong>🐍 Python</strong></summary>

**Передумови:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Запуск:**
```bash
python foundry-local-tool-calling.py
```

**Очікуваний результат:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Пояснення коду** (`python/foundry-local-tool-calling.py`):

```python
# Визначте інструменти як список схем функцій
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

# Відправляйте з інструментами — модель може повертати виклики інструментів замість вмісту
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Перевірте, чи модель хоче викликати інструмент
if response.choices[0].message.tool_calls:
    # Виконайте інструмент і надішліть результат назад
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Передумови:**
```bash
cd javascript
npm install
```

**Запуск:**
```bash
node foundry-local-tool-calling.mjs
```

**Очікуваний результат:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Пояснення коду** (`javascript/foundry-local-tool-calling.mjs`):

Цей приклад використовує вбудований у Foundry Local SDK `ChatClient` замість OpenAI SDK, демонструючи зручний метод `createChatClient()`:

```javascript
// Отримайте ChatClient безпосередньо з об'єкта моделі
const chatClient = model.createChatClient();

// Надіслати за допомогою інструментів — ChatClient обробляє формат, сумісний з OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Перевірте виклики інструментів
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Виконайте інструменти та надішліть результати назад
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Передумови:**
```bash
cd csharp
dotnet restore
```

**Запуск:**
```bash
dotnet run toolcall
```

**Очікуваний результат:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Пояснення коду** (`csharp/ToolCalling.cs`):

У C# використовується допоміжний метод `ChatTool.CreateFunctionTool` для визначення інструментів:

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

### Вправа 4 — Потік розмови виклику інструментів

Розуміння структури повідомлень критично. Ось повний потік з масивом `messages` на кожному етапі:

**Етап 1 — Початковий запит:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Етап 2 — Модель відповідає з `tool_calls` (без контенту):**
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

**Етап 3 — Ви додаєте повідомлення асистента І результат інструмента:**
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

**Етап 4 — Модель формує остаточну відповідь з урахуванням результату інструмента.**

> **Важливо:** `tool_call_id` у повідомленні інструмента має співпадати з `id` від виклику інструмента. Це спосіб, як модель пов’язує результати з запитами.

---

### Вправа 5 — Кілька викликів інструментів

Модель може запросити кілька викликів інструментів за однією відповіддю. Спробуйте змінити повідомлення користувача, щоб ініціювати кілька викликів:

```python
# У Python — змінити повідомлення користувача:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// У JavaScript — змінити повідомлення користувача:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Модель має повернути два `tool_calls` — для `get_weather` і для `get_population`. Ваш код уже обробляє це, бо проходить циклом по всім викликам.

> **Спробуйте:** Змініть повідомлення користувача і знову запустіть приклад. Чи викликає модель обидва інструменти?

---

### Вправа 6 — Додайте свій інструмент

Розширте один із прикладів новим інструментом. Наприклад, додайте інструмент `get_time`:

1. Визначте схему інструмента:
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

2. Додайте логіку виконання:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # У реальному додатку використовуйте бібліотеку для роботи з часовими зонами
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... існуючі інструменти ...
```

3. Додайте інструмент до масиву `tools` і протестуйте з: "Який час у Токіо?"

> **Виклик:** Додайте інструмент для виконання розрахунку, наприклад `convert_temperature`, який конвертує між Цельсієм та Фаренгейтом. Перевірте з: "Конвертуйте 100°F у Цельсій."

---

### Вправа 7 — Виклик інструментів за допомогою ChatClient SDK (JavaScript)

Приклад JavaScript вже використовує вбудований у SDK `ChatClient` замість OpenAI SDK. Це зручна функція, яка усуває необхідність самостійно створювати клієнт OpenAI:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient створюється безпосередньо з об'єкта моделі
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat приймає інструменти як другий параметр
const response = await chatClient.completeChat(messages, tools);
```

Порівняйте це з підходом на Python, де OpenAI SDK використовується явно:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Обидва підходи валідні. `ChatClient` зручніший; OpenAI SDK надає повний доступ до параметрів OpenAI.

> **Спробуйте:** Змініть JavaScript приклад, щоб використовувати OpenAI SDK замість `ChatClient`. Вам знадобиться `import OpenAI from "openai"` і створення клієнта з ендпоінтом `manager.urls[0]`.

---

### Вправа 8 — Розуміння `tool_choice`

Параметр `tool_choice` контролює, чи модель *повинна* використовувати інструмент, або може обирати вільно:

| Значення | Поведінка |
|-------|-----------|
| `"auto"` | Модель вирішує, чи викликати інструмент (за замовчуванням) |
| `"none"` | Модель не викликатиме інструменти, навіть якщо вони надані |
| `"required"` | Модель мусить викликати принаймні один інструмент |
| `{"type": "function", "function": {"name": "get_weather"}}` | Модель мусить викликати вказаний інструмент |

Спробуйте кожен варіант у прикладі на Python:

```python
# Примусити модель викликати get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Примітка:** Не всі варіанти `tool_choice` підтримуються кожною моделлю. Якщо модель не підтримує `"required"`, вона може ігнорувати цей параметр і поводитись як `"auto"`.

---

## Поширені проблеми

| Проблема | Вирішення |
|---------|----------|
| Модель ніколи не викликає інструменти | Переконайтеся, що ви використовуєте модель з підтримкою виклику інструментів (наприклад qwen2.5-0.5b). Перевірте таблицю вище. |
| Несумістність `tool_call_id` | Завжди використовуйте `id` із відповіді на виклик інструмента, а не прописане вручну значення |
| Модель повертає пошкоджений JSON в `arguments` | Малі моделі іноді генерують некоректний JSON. Обгорніть `JSON.parse()` у try/catch |
| Модель викликає неіснуючий інструмент | Додайте обробник за замовчуванням у вашій функції `execute_tool` |
| Нескінченний цикл виклику інструментів | Встановіть максимальну кількість раундів (наприклад, 5), щоб уникнути безконтрольних циклів |

---

## Головні висновки

1. **Виклик інструментів** дозволяє моделям запитувати виконання функцій замість гадання відповідей
2. Модель **ніколи не виконує код**; ваш додаток вирішує, що запускати
3. Інструменти визначаються у вигляді **об’єктів JSON Schema** за форматом OpenAI function-calling
4. Розмова використовує **багатокроковий шаблон**: користувач, потім асистент (tool_calls), інструмент (результати), потім асистент (фінальна відповідь)
5. Завжди використовуйте **модель з підтримкою виклику інструментів** (Qwen 2.5, Phi-4-mini)
6. Метод SDK `createChatClient()` забезпечує зручний спосіб робити запити з викликом інструментів без створення клієнта OpenAI вручну

---

Продовжуйте далі до [Частина 12: Побудова веб-інтерфейсу для Zava Creative Writer](part12-zava-ui.md), щоб додати браузерний фронтенд до мультиагентної ланцюжка з потоковою обробкою в режимі реального часу.

---

[← Частина 10: Користувацькі моделі](part10-custom-models.md) | [Частина 12: Zava Writer UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Відмова від відповідальності**:  
Цей документ було перекладено за допомогою сервісу автоматичного перекладу [Co-op Translator](https://github.com/Azure/co-op-translator). Хоча ми прагнемо до точності, просимо враховувати, що автоматизовані переклади можуть містити помилки або неточності. Оригінальний документ рідною мовою слід вважати авторитетним джерелом. Для критично важливої інформації рекомендується звертатися до професійного людського перекладу. Ми не несемо відповідальності за будь-які непорозуміння чи неправильні тлумачення, що виникли внаслідок використання цього перекладу.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->