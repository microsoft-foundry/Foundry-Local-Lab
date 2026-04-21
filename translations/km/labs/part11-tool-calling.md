![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ផ្នែក ១១៖ ការហៅឧបករណ៍ជាមួយម៉ូដែលក្នុងតំបន់

> **គោលបំណង៖** អនុញ្ញាតឲ្យម៉ូដែលក្នុងតំបន់របស់អ្នកហៅមុខងារទីភ្នាក់ងារ (ឧបករណ៍) ដើម្បីវាចូលទិន្នន័យពេលជាក់លាក់ បំពេញគណនាលទ្ធផល ឬធ្វើប្រតិបត្តិការជាមួយ API — រាល់អ្វីទាំងអស់ដំណើរការជាបម្រុងនៅលើឧបករណ៍របស់អ្នកឯង។

## ការហៅឧបករណ៍មានអ្វី?

ការហៅឧបករណ៍ (ដែលគេហៅថា **function calling**) អនុញ្ញាតឲ្យម៉ូដែលភាសាក្នុងបណ្តោយស្នើសុំអនុវត្តមុខងារដែលអ្នកកំណត់។ ជំនួសការប៉ានប្រមាណចម្លើយ ម៉ូដែលដឹងថាពេលណាដែលឧបករណ៍នឹងជួយ ហើយត្រឡប់ការស្នើសុំសម្រាប់កូដរបស់អ្នកអនុវត្តមុខងារ។ កម្មវិធីរបស់អ្នកដំណើរ​ការ​មុខងារ ផ្ញើ​លទ្ធផល​ត្រឡប់ ហើយម៉ូដែលបញ្ចូល​ព័ត៌មាន​នោះក្នុងចម្លើយចុងក្រោយរបស់ខ្លួន។

![Tool-calling flow](../../../images/tool-calling-flow.svg)

គំរូនេះមានសារៈសំខាន់សម្រាប់បង្កើតភ្នាក់ងារដែលអាច៖

- **ស្វែងរកទិន្នន័យពេលជាក់លាក់** (អាកាសធាតុ, តម្លៃភាគហ៊ុន, សំណួរសំណុំទិន្នន័យ)
- **ធ្វើគណនាដោយត្រឹមត្រូវ** (គណិតវិទ្យា, ការបំលែងឯកតា)
- **អនុវត្តសកម្មភាព** (ផ្ញើអ៊ីមែល, បង្កើតសំបុត្រ, បន្ទាន់​សម័យ​កំណត់ត្រា)
- **ចូលប្រព័ន្ធឯកជន** (API ខាងក្នុង, ប្រព័ន្ធឯកសារ)

---

## របៀបការហៅឧបករណ៍ដំណើរការ

លំនាំហៅឧបករណ៍មានបួនជំហាន៖

| ជំហាន | អ្វីកើតឡើង |
|--------|---------------|
| **១. កំណត់ឧបករណ៍** | អ្នកពិពណ៌នាមុខងារទាំងអស់ដោយប្រើ JSON Schema — ឈ្មោះ, ពិពណ៌នា និងប៉ារ៉ាម៉ា​ត្រ |
| **២. ម៉ូដែលសម្រេចចិត្ត** | ម៉ូដែលទទួលសាររបស់អ្នកបូកជាមួយពណ៌នាឧបករណ៍។ បើឧបករណ៍ជួយបាន វាត្រលប់ជំរើស `tool_calls` ជំនួសចម្លើយអត្ថបទ |
| **៣. អនុវត្តក្នុងតំបន់** | កូដរបស់អ្នកបកប្រែការហៅឧបករណ៍ ដំណើរមុខងារ ហើយប្រមូលលទ្ធផល |
| **៤. ចម្លើយចុងក្រោយ** | អ្នកផ្ញើលទ្ធផលឧបករណ៍ត្រឡប់ទៅម៉ូដែក ដែលផលិតចំលើយចុងក្រោយរបស់វា |

> **ចំណុចសំខាន់៖** ម៉ូដែលមិនដែលអនុវត្តកូដទេ។ វា *ស្នើសុំ* តែឲ្យហៅឧបករណ៍ប៉ុណ្ណោះ។ កម្មវិធីរបស់អ្នកសម្រេចថាតើធ្វើតាមសំណើនោះឬទេ — នេះធ្វើឲ្យអ្នកត្រួតគ្រប់គ្រងពេញលេញ។

---

## ម៉ូដែលណាដែលគាំទ្រការហៅឧបករណ៍?

មិនមួយម៉ូដែលគ្រប់គ្រាន់គាំទ្រការហៅឧបករណ៍ទេ។ នៅក្នុងបញ្ជី Foundry Local បច្ចុប្បន្ន ម៉ូដែលខាងក្រោមមានសក្ដានុពលហៅឧបករណ៍៖

| ម៉ូដែល | ទំហំ | ការហៅឧបករណ៍ |
|--------|--------|:--------------:|
| qwen2.5-0.5b | 822 មេកាបៃ | ✅ |
| qwen2.5-1.5b | 1.8 ហ្គីហ្គាបៃ | ✅ |
| qwen2.5-7b | 6.3 ហ្គីហ្គាបៃ | ✅ |
| qwen2.5-14b | 11.3 ហ្គីហ្គាបៃ | ✅ |
| qwen2.5-coder-0.5b | 822 មេកាបៃ | ✅ |
| qwen2.5-coder-1.5b | 1.8 ហ្គីហ្គាបៃ | ✅ |
| qwen2.5-coder-7b | 6.3 ហ្គីហ្គាបៃ | ✅ |
| qwen2.5-coder-14b | 11.3 ហ្គីហ្គាបៃ | ✅ |
| phi-4-mini | 4.6 ហ្គីហ្គាបៃ | ✅ |
| phi-3.5-mini | 2.6 ហ្គីហ្គាបៃ | ❌ |
| phi-4 | 10.4 ហ្គីហ្គាបៃ | ❌ |

> **ពន្លឹះ៖** សម្រាប់មនុស្សបង្រៀននេះយើងប្រើ **qwen2.5-0.5b** — វាមានទំហំតូច (ទាញយក 822 មេកាបៃ) លឿន ហើយគាំទ្រការហៅឧបករណ៍ជាក់លាក់។

---

## ប្រយោជន៍ការសិក្សា

នៅចុងបញ្ចប់មនុស្សបង្រៀននេះ អ្នកនឹងអាច៖

- ពន្យល់លំនាំការហៅឧបករណ៍ និងហេតុអត្ថប្រយោជន៍សម្រាប់ភ្នាក់ងារ AI
- កំណត់សៀមស្ក្រីបឧបករណ៍ដោយប្រើទ្រង់ទ្រាយ OpenAI function-calling
- គ្រប់គ្រងលំនាំការពិភាក្សាការហៅឧបករណ៍ពហុជំហាន
- អនុវត្តការហៅឧបករណ៍ក្នុងតំបន់ ហើយត្រឡប់លទ្ធផលទៅម៉ូដែល
- ជ្រើសរើសម៉ូដែលត្រឹមត្រូវសម្រាប់សកម្មភាពហៅឧបករណ៍

---

## លក្ខខណ្ឌមុនចាប់ផ្ដើម

| លក្ខខណ្ឌ | ព័ត៌មានលម្អិត |
|-----------|----------------|
| **Foundry Local CLI** | តំឡើងរួចហើយនៅលើ `PATH` របស់អ្នក ([ផ្នែក ១](part1-getting-started.md)) |
| **Foundry Local SDK** | តំឡើង Python, JavaScript, ឬ C# SDK ([ផ្នែក ២](part2-foundry-local-sdk.md)) |
| **ម៉ូដែលហៅឧបករណ៍** | qwen2.5-0.5b (នឹងទាញយកដោយស្វ័យប្រវត្តិ) |

---

## លំហាត់

### លំហាត់ ១ — យល់ដឹងពីលំនាំហៅឧបករណ៍

មុនសរសេរកូដ សូមអានតារាងតម្រៀបលំដាប់នេះ៖

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**ចំណុចកត្ដាព្យាយាម:**

1. អ្នកកំណត់ឧបករណ៍ជាមុនជារបស់ JSON Schema
2. ចម្លើយម៉ូដែលមាន `tool_calls` ជំនួសមាតិកាទូទៅ
3. រាល់ការហៅឧបករណ៍មាន `id` ឯកទេស ដែលអ្នកត្រូវយោងពេលត្រឡប់លទ្ធផល
4. ម៉ូដែលឃើញសារមុនគ្រប់សំបុត្រនិងលទ្ធផលឧបករណ៍ *បូកបន្ថែម* នៅពេលបង្កើតចម្លើយចុងក្រោយ
5. អាចមានការហៅឧបករណ៍ច្រើនក្នុងចម្លើយមួយ

> **ពិភាក្សា៖** ហេតុអ្វីម៉ូដែលត្រលប់ការហៅឧបករណ៍ ជំនួសការអនុវត្តមុខងារផ្ទាល់? តើតំបន់សុវត្ថិភាពណាដែលវាបន្ថែម?

---

### លំហាត់ ២ — កំណត់សៀមស្ក្រីបឧបករណ៍

ឧបករណ៍កំណត់ដោយប្រើទ្រង់ទ្រាយ OpenAI function-calling ស្តង់ដា។ រាល់ឧបករណ៍ត្រូវការ៖

- **`type`**: តែងតែជា `"function"`
- **`function.name`**: ឈ្មោះមុខងារពិពណ៌នា (ឧ. `get_weather`)
- **`function.description`**: ពិពណ៌នាច្បាស់ — ម៉ូដែលប្រើវាដើម្បីសម្រេចថាតើអាចហៅឧបករណ៍នេះទេ
- **`function.parameters`**: វត្ថុ JSON Schema ពិពណ៌នាពីអាគុយម៉ង់ដែលរំពឹងទុក

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

> **ការអនុវត្តល្អបំផុតសម្រាប់ពិពណ៌នាឧបករណ៍:**
> - ជាក់លាក់៖ "ទទួលបានអាកាសធាតុបច្ចុប្បន្នសម្រាប់ទីក្រុងមួយ" ថាធ្វើបានល្អជាង "ទទួលបានអាកាសធាតុ"
> - ពិពណ៌នាអាគុយម៉ង់ឲ្យច្បាស់៖ ម៉ូដែលអានពណ៌នាឲ្យដឹងពីតម្លៃត្រឹមត្រូវ
> - សម្គាល់អាគុយម៉ង់ដែលត្រូវការ ប្រៀបធៀបទៅនឹងជម្រើស — វាជួយឲ្យម៉ូដែលសម្រេចចិត្តត្រូវស្នើរអ្វី

---

### លំហាត់ ៣ — ដំណើរការឧបករណ៍ឧទាហរណ៍

រាល់ម៉ូដែលភាសាកំណត់ពីរឧបករណ៍ (`get_weather` និង `get_population`), ផ្ញើសំណួរដែលចាប់ការប្រើឧបករណ៍, អនុវត្តឧបករណ៍ក្នុងតំបន់, ហើយផ្ញើលទ្ធផលត្រឡប់សម្រាប់ចម្លើយចុងក្រោយ។

<details>
<summary><strong>🐍 Python</strong></summary>

**លក្ខខណ្ឌមុនចាប់ផ្ដើម:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**រត់:**
```bash
python foundry-local-tool-calling.py
```

**លទ្ធផលព្យាយាម:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ដំណើរការកូដ** (`python/foundry-local-tool-calling.py`):

```python
# កំណត់ឧបករណ៍ជាបញ្ជីនៃស្កីមម៉ាឧបករណ៍
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

# ផ្ញើជាមួយឧបករណ៍ — ម៉ូដែលអាចត្រឡប់មកវិញជាការហៅឧបករណ៍ជំនួសខ្លឹមសារ
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# ពិនិត្យមើលថាម៉ូដែលចង់ហៅឧបករណ៍ទេ
if response.choices[0].message.tool_calls:
    # អនុវត្តឧបករណ៍ហើយផ្ញើលទ្ធផលត្រឡប់មកវិញ
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**លក្ខខណ្ឌមុនចាប់ផ្ដើម:**
```bash
cd javascript
npm install
```

**រត់:**
```bash
node foundry-local-tool-calling.mjs
```

**លទ្ធផលព្យាយាម:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ដំណើរការកូដ** (`javascript/foundry-local-tool-calling.mjs`):

គំរូនេះប្រើ `ChatClient` ដើរតួជាមួយ Foundry Local SDK មិនប្រើ OpenAI SDK, បង្ហាញពីភាពងាយស្រួលនៃមុខងារ `createChatClient()`៖

```javascript
// ទទួលបាន ChatClient ពីវត្ថុម៉ូឌែលដោយផ្ទាល់
const chatClient = model.createChatClient();

// ផ្ញើជាមួយឧបករណ៍ — ChatClient គ្រប់គ្រងទ្រង់ទ្រាយដែលសមរម្យជាមួយ OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// ពិនិត្យការហៅឧបករណ៍
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // បញ្ចូលឧបករណ៍ និងផ្ញើលទ្ធផលត្រឡប់ទៅវិញ
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**លក្ខខណ្ឌមុនចាប់ផ្ដើម:**
```bash
cd csharp
dotnet restore
```

**រត់:**
```bash
dotnet run toolcall
```

**លទ្ធផលព្យាយាម:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ដំណើរការកូដ** (`csharp/ToolCalling.cs`):

C# ប្រើជំនួយការ `ChatTool.CreateFunctionTool` ដើម្បីកំណត់ឧបករណ៍៖

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

### លំហាត់ ៤ — លំនាំសន្ទនារហៅឧបករណ៍

ការយល់ដឹងរចនាសម្ព័ន្ធសារមានសារៈសំខាន់។ នេះជាការហូរពេញលេញបង្ហាញ `messages` នៅរៀងរាល់ជំហាន៖

**ជំហាន ១ — សំណើដំបូង៖**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**ជំហាន ២ — ម៉ូដែលប្រាប់ជាមួយ `tool_calls` (មិនមែនមាតិកា):**
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

**ជំហាន ៣ — អ្នកបន្ថែមសារជំនួយ និងលទ្ធផលឧបករណ៍:**
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

**ជំហាន ៤ — ម៉ូដែលបង្កើតចម្លើយចុងក្រោយដោយប្រើលទ្ធផលឧបករណ៍។**

> **សំខាន់៖** `tool_call_id` នៅក្នុងសារឧបករណ៍ត្រូវតែត្រូវគ្នា `id` ពីការហៅឧបករណ៍។ នេះជារបៀបម៉ូដែលភ្ជាប់លទ្ធផលជាមួយសំណើ។

---

### លំហាត់ ៥ — ការហៅឧបករណ៍ច្រើន

ម៉ូដែលអាចស្នើហៅឧបករណ៍ជាច្រើនក្នុងចំលើយតែមួយ។ ព្យាយាមផ្លាស់ប្ដូរសារ​ប្រាក់អ្នកប្រើដើម្បីបង្កើតការហៅច្រើន៖

```python
# ក្នុង Python — ប្ដូរពីសារ​អ្នកប្រើ:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// ក្នុង JavaScript — ប្ដូរសាររបស់អ្នកប្រើ:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

ម៉ូដែលគួរតែបង្វិលបញ្ចូន `tool_calls` ពីរយ៉ាង — មួយសម្រាប់ `get_weather` មួយសម្រាប់ `get_population`។ កូដរបស់អ្នកគ្រប់គ្រងបានហើយព្រោះវាលូបតាមគ្រប់ការហៅឧបករណ៍។

> **សាកល្បង៖** ផ្លាស់ប្ដូរសារ​ប្រាក់ និងរត់គំរូម្តងទៀត។ តើម៉ូដែលរំលោភសំរាប់ឧបករណ៍ទាំងពីរទេ?

---

### លំហាត់ ៦ — បន្ថែមឧបករណ៍ផ្ទាល់ខ្លួន

បន្ថែមឧបករណ៍ថ្មីមួយក្នុងគំរូមួយ។ ឧទាហរណ៍ ដាក់ឈ្មោះឧបករណ៍ `get_time`៖

1. កំណត់សៀមស្ក្រីបឧបករណ៍៖
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

2. បន្ថែមតុល្យភាពអនុវត្ត៖
```python
# ភាយថុន
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # ក្នុងកម្មវិធីពិតប្រាកដ ប្រើបណ្ណាល័យម៉ោងតំបន់
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... ឧបករណ៍ដែលមានស្រាប់ ...
```

3. បន្ថែមឧបករណ៍ទៅក្នុងអារេ `tools` ហើយសាកល្បងជាមួយ៖ "តើម៉ោងប៉ុន្មាននៅទីក្រុងតូក្យូ?"

> **បញ្ហា៖** បន្ថែមឧបករណ៍ដែលធ្វើគណនា ដូចជា `convert_temperature` បម្លែងចន្លោះសែលស្យិន និងផារ៉ែនហៃត៍។ សាកល្បងជាមួយ៖ "បម្លែង 100°F ទៅសែលស្យិន។"

---

### លំហាត់ ៧ — ហៅឧបករណ៍ជាមួយ SDK ChatClient (JavaScript)

គំរូ JavaScript ប្រើ `ChatClient` នៅក្នុង SDK ដើម្បីជំនួស OpenAI SDK។ វាជាផ្នែកងាយស្រួលដកឲ្យអ្នកមិនចាំបាច់បង្កើត OpenAI client ផ្ទាល់៖

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient ត្រូវបានបង្កើតដោយផ្ទាល់ពីវត្ថុម៉ូឌែល
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat ទទួលយកឧបករណ៍ជាអថេរការពីរបន្ទាប់
const response = await chatClient.completeChat(messages, tools);
```

ប្រៀបធៀបនឹងវិធី Python ដែលប្រើ OpenAI SDK តែមួយ:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

រៀងគ្នាដំណើរការល្អទាំងពីរ។ `ChatClient` ងាយស្រួល; OpenAI SDK ផ្តល់នូវជម្រើសពេញលេញរបស់ OpenAI។

> **សាកល្បង៖** ដំណើរការផ្លាស់ប្ដូរគំរូ JavaScript ដើម្បីប្រើ OpenAI SDK ជំនួស `ChatClient`។ អ្នកត្រូវការនាំចេញ `import OpenAI from "openai"` និងបង្កើត client ជាមួយចំណុចចេញពី `manager.urls[0]`។

---

### លំហាត់ ៨ — យល់ដឹងអំពី tool_choice

ប៉ារ៉ាម៉ែត្រ `tool_choice` គ្រប់គ្រងថាតើម៉ូដែល *ត្រូវ* ប្រើឧបករណ៍ ឬអាចជ្រើសរើសដោយស្មើ៖

| តម្លៃ | រៀបរាប់ |
|--------|----------|
| `"auto"` | ម៉ូដែលសម្រេចចិត្តថាតើហៅឧបករណ៍ (លំនាំដើម) |
| `"none"` | ម៉ូដែលមិនហៅឧបករណ៍ទោះបីមានផ្តល់ជូនក៏ដោយ |
| `"required"` | ម៉ូដែលត្រូវតែហៅឧបករណ៍ច្រើនតិចមួយដង |
| `{"type": "function", "function": {"name": "get_weather"}}` | ម៉ូដែលត្រូវហៅឧបករណ៍ដែលបានបញ្ជាក់ |

ព្យាយាមនីតិវិធីទាំងនេះនៅក្នុងគំរូ Python៖

```python
# បង្ខំឲ្យម៉ូដែលហៅ get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **កំណត់ចិត្ត៖** មិនមួយសម្រង់ `tool_choice` ទាំងអស់អាចគាំទ្រដោយម៉ូដែលគ្រប់ម៉ូដែលទេ។ ប្រសិនបើម៉ូដែលមិនគាំទ្រ `"required"`, វាអាចមិនទទួលយកការកំណត់នោះ និងប្រើ `"auto"` ជំនួស។

---

## ករណីកុហកទូទៅ

| ការបញ្ហា | ដំណោះស្រាយ |
|-----------|-------------|
| ម៉ូដែលមិនដែលហៅឧបករណ៍ | ពិនិត្យឲ្យប្រាកដថាអ្នកប្រើម៉ូដែលគាំទ្រការហៅឧបករណ៍ (ឧ. qwen2.5-0.5b)។ ពិនិត្យបញ្ជីខាងលើ។ |
| `tool_call_id` មិនត្រូវគ្នា | តែងតែប្រើ `id` ពីការឆ្លើយតបការហៅឧបករណ៍ មិនត្រូវកំណត់ផ្ទាល់ |
| ម៉ូដែលត្រឡប់ JSON មិនត្រឹមត្រូវនៅក្នុង `arguments` | ម៉ូដែលតូចៗពេលខ្លះចេញ JSON មិនត្រឹមត្រូវ។ ប្រឡងជុំក្នុង `try/catch` នៅ JSON.parse() |
| ម៉ូដែលហៅឧបករណ៍មិនមាន | បន្ថែមតុល្យភាព default នៅក្នុង `execute_tool` របស់អ្នក |
| លំនាំហៅឧបករណ៍រលាយមិនដាច់ | កំណត់កំណត់ចំនូនជុំខ្ពស់បំផុត (ឧ. 5 ជុំ) ដើម្បីបញ្ឈប់ការហៅរាបកំប្លែង |

---

## ចំណុចសំខាន់

1. **ការហៅឧបករណ៍** អនុញ្ញាតម៉ូដែលស្នើសុំអនុវត្តមុខងារជំនួសការប៉ានប្រមាណចម្លើយ
2. ម៉ូដែល **មិនអនុវត្តកូដ** ទេ; កម្មវិធីអ្នកសម្រេចតើអ្វីត្រូវដំណើរ
3. ឧបករណ៍កំណត់ជា **វត្ថុ JSON Schema** តាមទ្រង់ទ្រាយ OpenAI function-calling
4. សន្ទនាប្រើ **លំនាំពហុជុំ**៖ អ្នកប្រើ, ជំនួយការ (tool_calls), ឧបករណ៍ (លទ្ធផល), ជំនួយការ (ចម្លើយចុងក្រោយ)
5. តែងតែប្រើ **ម៉ូដែលគាំទ្រការហៅឧបករណ៍** (Qwen 2.5, Phi-4-mini)
6. SDK `createChatClient()` ផ្តល់វិធីងាយស្រួលក្នុងការស្នើសុំហៅឧបករណ៍ដោយមិនចាំបាច់បង្កើត OpenAI client

---

បន្តទៅ [ផ្នែក ១២៖ កសាង UI វេបសាយសម្រាប់អ្នកសរសេរច្នៃប្រឌិត Zava](part12-zava-ui.md) ដើម្បីបន្ថែមផ្នែកមុខមាត់នៅលើកម្មវិធីពហុភ្នាក់ងារជាមួយចរន្តពេលជាក់លាក់។

---

[← ផ្នែក ១០៖ ម៉ូដែលផ្ទាល់ខ្លួន](part10-custom-models.md) | [ផ្នែក ១២៖ UI អ្នកសរសេរ Zava →](part12-zava-ui.md)