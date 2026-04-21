![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Часть 11: Вызов инструментов с локальными моделями

> **Цель:** Позволить вашей локальной модели вызывать внешние функции (инструменты), чтобы она могла получать данные в реальном времени, выполнять вычисления или взаимодействовать с API — всё это работает приватно на вашем устройстве.

## Что такое вызов инструментов?

Вызов инструментов (также известный как **вызов функций**) позволяет языковой модели запросить выполнение определённых вами функций. Вместо того чтобы гадать ответ, модель распознаёт, когда может помочь инструмент, и возвращает структурированный запрос для выполнения вашей программой. Ваше приложение запускает функцию, отправляет результат модели, а она использует эту информацию в своём итоговом ответе.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Этот подход важен для создания агентов, которые могут:

- **Получать актуальные данные** (погода, цены на акции, запросы к базам данных)
- **Выполнять точные вычисления** (математика, преобразование единиц)
- **Выполнять действия** (отправка писем, создание тикетов, обновление записей)
- **Доступ к приватным системам** (внутренние API, файловые системы)

---

## Как работает вызов инструментов

Процесс вызова инструментов состоит из четырёх этапов:

| Этап | Что происходит |
|-------|-------------|
| **1. Определение инструментов** | Вы описываете доступные функции с помощью JSON Schema — имя, описание и параметры |
| **2. Решение модели** | Модель получает ваше сообщение и определения инструментов. Если инструмент поможет, она возвращает `tool_calls` вместо текстового ответа |
| **3. Локальное выполнение** | Ваш код разбирает вызов инструмента, запускает функцию и собирает результат |
| **4. Итоговый ответ** | Вы отправляете результат инструмента обратно модели, которая формирует итоговый ответ |

> **Ключевой момент:** Модель никогда не выполняет код. Она только *запрашивает* вызов инструмента. Решение, выполнять запрос или нет, остаётся за вашим приложением — это обеспечивает полный контроль.

---

## Какие модели поддерживают вызов инструментов?

Не все модели поддерживают вызов инструментов. В текущем каталоге Foundry Local следующие модели имеют возможность вызова инструментов:

| Модель | Размер | Вызов инструментов |
|-------|--------|:-----------------:|
| qwen2.5-0.5b | 822 МБ | ✅ |
| qwen2.5-1.5b | 1.8 ГБ | ✅ |
| qwen2.5-7b | 6.3 ГБ | ✅ |
| qwen2.5-14b | 11.3 ГБ | ✅ |
| qwen2.5-coder-0.5b | 822 МБ | ✅ |
| qwen2.5-coder-1.5b | 1.8 ГБ | ✅ |
| qwen2.5-coder-7b | 6.3 ГБ | ✅ |
| qwen2.5-coder-14b | 11.3 ГБ | ✅ |
| phi-4-mini | 4.6 ГБ | ✅ |
| phi-3.5-mini | 2.6 ГБ | ❌ |
| phi-4 | 10.4 ГБ | ❌ |

> **Совет:** Для этой лаборатории мы используем **qwen2.5-0.5b** — она небольшая (загрузка 822 МБ), быстрая и надёжно поддерживает вызов инструментов.

---

## Цели обучения

К концу этой лаборатории вы сможете:

- Объяснять паттерн вызова инструментов и почему он важен для AI агентов
- Определять схемы инструментов с использованием формата вызова функций OpenAI
- Обрабатывать многоступенчатый поток диалога с вызовом инструментов
- Локально выполнять вызовы инструментов и возвращать результаты модели
- Выбирать подходящую модель для сценариев вызова инструментов

---

## Предварительные требования

| Требование | Подробности |
|-------------|-------------|
| **Foundry Local CLI** | Установлен и в вашем `PATH` ([Часть 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Установлен Python, JavaScript или C# SDK ([Часть 2](part2-foundry-local-sdk.md)) |
| **Модель с вызовом инструментов** | qwen2.5-0.5b (скачается автоматически) |

---

## Упражнения

### Упражнение 1 — Понимание потока вызова инструментов

Перед написанием кода изучите диаграмму последовательности:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Ключевые наблюдения:**

1. Вы заранее определяете инструменты как объекты JSON Schema
2. Ответ модели содержит `tool_calls` вместо обычного контента
3. Каждый вызов инструмента имеет уникальный `id`, на который нужно ссылаться при возврате результата
4. Модель видит все предыдущие сообщения *и* результаты инструментов при формировании итогового ответа
5. В одном ответе может быть несколько вызовов инструментов

> **Обсуждение:** Почему модель возвращает вызовы инструментов вместо прямого выполнения функций? Какие преимущества безопасности это даёт?

---

### Упражнение 2 — Определение схем инструментов

Инструменты описываются с использованием стандартного формата вызова функций OpenAI. Каждый инструмент должен иметь:

- **`type`**: всегда `"function"`
- **`function.name`**: описательное имя функции (например, `get_weather`)
- **`function.description`**: понятное описание — модель использует его, чтобы решить, когда вызывать инструмент
- **`function.parameters`**: объект JSON Schema, описывающий ожидаемые аргументы

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

> **Лучшие практики для описаний инструментов:**
> - Будьте конкретны: "Получить текущую погоду для заданного города" лучше, чем просто "Получить погоду"
> - Чётко опишите параметры: модель читает эти описания, чтобы подставлять правильные значения
> - Отметьте обязательные и необязательные параметры — это помогает модели понять, что спрашивать

---

### Упражнение 3 — Запуск примеров вызова инструментов

В каждом примере на языке программирования определяются два инструмента (`get_weather` и `get_population`), отправляется вопрос, который вызывает использование инструмента, выполняется функция локально, и результат отправляется обратно модели для итогового ответа.

<details>
<summary><strong>🐍 Python</strong></summary>

**Предварительные требования:**
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

**Ожидаемый вывод:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Разбор кода** (`python/foundry-local-tool-calling.py`):

```python
# Определите инструменты как список схем функций
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

# Отправляйте с инструментами — модель может возвращать вызовы инструментов вместо содержимого
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Проверьте, хочет ли модель вызвать инструмент
if response.choices[0].message.tool_calls:
    # Выполните инструмент и отправьте результат обратно
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Предварительные требования:**
```bash
cd javascript
npm install
```

**Запуск:**
```bash
node foundry-local-tool-calling.mjs
```

**Ожидаемый вывод:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Разбор кода** (`javascript/foundry-local-tool-calling.mjs`):

Этот пример использует нативный `ChatClient` SDK Foundry Local вместо OpenAI SDK, что демонстрирует удобство метода `createChatClient()`:

```javascript
// Получить ChatClient напрямую из объекта модели
const chatClient = model.createChatClient();

// Отправлять с помощью инструментов — ChatClient обрабатывает формат, совместимый с OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Проверить вызовы инструментов
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Выполнить инструменты и отправить результаты обратно
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Предварительные требования:**
```bash
cd csharp
dotnet restore
```

**Запуск:**
```bash
dotnet run toolcall
```

**Ожидаемый вывод:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Разбор кода** (`csharp/ToolCalling.cs`):

В C# для определения инструментов используется помощник `ChatTool.CreateFunctionTool`:

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

### Упражнение 4 — Поток диалога с вызовом инструментов

Важно понимать структуру сообщений. Вот полный поток с массивом `messages` на каждом этапе:

**Этап 1 — Изначальный запрос:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Этап 2 — Модель отвечает с `tool_calls` (без содержимого):**
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

**Этап 3 — Вы добавляете сообщение ассистента И результат инструмента:**
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

**Этап 4 — Модель выдаёт итоговый ответ с использованием результата инструмента.**

> **Важно:** `tool_call_id` в сообщении инструмента должен совпадать с `id` из вызова инструмента. Так модель связывает результаты с запросами.

---

### Упражнение 5 — Несколько вызовов инструментов

Модель может запрашивать несколько вызовов инструментов в одном ответе. Попробуйте изменить сообщение пользователя, чтобы вызвать несколько вызовов:

```python
# В Python — изменить сообщение пользователя:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// В JavaScript — изменить сообщение пользователя:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Модель должна вернуть два `tool_calls` — один для `get_weather` и один для `get_population`. Ваш код уже это поддерживает, так как обрабатывает все вызовы по очереди.

> **Попробуйте:** Измените сообщение пользователя и запустите пример снова. Модель вызовет оба инструмента?

---

### Упражнение 6 — Добавьте собственный инструмент

Расширьте один из примеров новым инструментом. Например, добавьте инструмент `get_time`:

1. Определите схему инструмента:
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

2. Добавьте логику выполнения:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # В реальном приложении используйте библиотеку часовых поясов
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... существующие инструменты ...
```

3. Добавьте инструмент в массив `tools` и протестируйте с вопросом: "Который час в Токио?"

> **Задача:** Добавьте инструмент, выполняющий вычисление, например, `convert_temperature`, который преобразует между Цельсием и Фаренгейтом. Проверьте с: "Преобразуй 100°F в Цельсии."

---

### Упражнение 7 — Вызов инструментов с использованием ChatClient SDK (JavaScript)

Пример на JavaScript уже использует нативный `ChatClient` SDK, а не OpenAI SDK. Это удобная функция, позволяющая не создавать клиента OpenAI вручную:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient создается напрямую из объекта модели
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat принимает инструменты в качестве второго параметра
const response = await chatClient.completeChat(messages, tools);
```

Сравните с подходом в Python, где явно используется OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Оба паттерна валидны. `ChatClient` удобнее, OpenAI SDK даёт полный набор параметров OpenAI.

> **Попробуйте:** Измените пример на JavaScript, чтобы использовать OpenAI SDK вместо `ChatClient`. Понадобится `import OpenAI from "openai"` и создание клиента с endpoint из `manager.urls[0]`.

---

### Упражнение 8 — Понимание параметра tool_choice

Параметр `tool_choice` управляет тем, должен ли модель *обязательно* использовать инструмент или может выбирать:

| Значение | Поведение |
|----------|-----------|
| `"auto"` | Модель сама решает, вызывать ли инструмент (по умолчанию) |
| `"none"` | Модель не будет вызывать инструменты, даже если они доступны |
| `"required"` | Модель обязана вызвать хотя бы один инструмент |
| `{"type": "function", "function": {"name": "get_weather"}}` | Модель обязана вызвать указанный инструмент |

Попробуйте все варианты в примере на Python:

```python
# Принудительно заставить модель вызвать get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Примечание:** Не все варианты `tool_choice` поддерживаются каждой моделью. Если модель не поддерживает `"required"`, она может игнорировать настройку и вести себя как `"auto"`.

---

## Частые ошибки

| Проблема | Решение |
|----------|---------|
| Модель никогда не вызывает инструменты | Убедитесь, что вы используете модель с поддержкой вызова инструментов (например, qwen2.5-0.5b). Проверьте таблицу выше. |
| Несовпадение `tool_call_id` | Используйте всегда `id` из ответа с вызовом инструмента, а не жестко заданное значение |
| Модель возвращает некорректный JSON в `arguments` | Меньшие модели иногда генерируют некорректный JSON. Оборачивайте `JSON.parse()` в try/catch |
| Модель вызывает несуществующий инструмент | Добавьте обработчик по умолчанию в вашей функции `execute_tool` |
| Бесконечный цикл вызовов инструментов | Установите максимальное количество раундов (например, 5), чтобы избежать зацикливания |

---

## Ключевые выводы

1. **Вызов инструментов** позволяет моделям запрашивать выполнение функций вместо угадывания ответов
2. Модель **никогда не выполняет код**; запуск остается за вашим приложением
3. Инструменты определяются как объекты **JSON Schema** по формату вызова функций OpenAI
4. Диалог происходит по **многоходовому паттерну**: пользователь — ассистент (tool_calls) — инструмент (результаты) — ассистент (итоговый ответ)
5. Всегда используйте **модель с поддержкой вызова инструментов** (Qwen 2.5, Phi-4-mini)
6. Метод SDK `createChatClient()` удобно позволяет делать вызовы инструментов без ручного построения клиента OpenAI

---

Продолжите с [Часть 12: Создание веб-интерфейса для Zava Creative Writer](part12-zava-ui.md), чтобы добавить браузерный фронтенд к мультиагентному пайплайну с потоковой передачей в реальном времени.

---

[← Часть 10: Пользовательские модели](part10-custom-models.md) | [Часть 12: Интерфейс Zava Writer →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Отказ от ответственности**:  
Этот документ был переведен с использованием сервиса автоматического перевода [Co-op Translator](https://github.com/Azure/co-op-translator). Несмотря на наши усилия по обеспечению точности, просим учитывать, что автоматические переводы могут содержать ошибки или неточности. Оригинальный документ на исходном языке следует считать авторитетным источником. Для получения критически важной информации рекомендуется обратиться к профессиональному переводу с участием человека. Мы не несем ответственности за любые недоразумения или неправильные трактовки, возникшие вследствие использования данного перевода.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->