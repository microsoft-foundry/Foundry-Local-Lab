![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग ११: स्थानीय मोडेलहरूसँग उपकरण कलिंग

> **लक्ष्य:** तपाईंको स्थानीय मोडेललाई बाह्य फंक्शनहरू (उपकरणहरू) कल गर्न सक्षम पार्नुहोस् ताकि यसले वास्तविक-समय डाटा प्राप्त गर्न, गणना गर्न, वा API हरूसँग अन्तरक्रिया गर्न सकोस् — सबै तपाईंको उपकरणमा निजी रूपमा चलाइरहेको।

## उपकरण कलिंग भनेको के हो?

उपकरण कलिंग (जसलाई **फंक्शन कलिंग** पनि भनिन्छ) ले भाषा मोडेललाई तपाईंले परिभाषित गरेको फंक्शनहरूको कार्यान्वयन अनुरोध गर्न दिन्छ। उत्तर अनुमान लगाउनको सट्टा, मोडेलले जब उपकरणले मद्दत गर्नेछ भनेर पहिचान गर्छ र तपाईंको कोडले कार्यान्वयन गर्नको लागि संरचित अनुरोध फिर्ता गर्छ। तपाईंको अनुप्रयोगले फंक्शन चलाउँछ, परिणाम फिर्ता पठाउँछ, र मोडेलले त्यो जानकारी अन्तिम उत्तरमा समावेश गर्छ।

![Tool-calling flow](../../../images/tool-calling-flow.svg)

यो ढाँचा एजेन्टहरू निर्माण गर्न आवश्यक छ जसले गर्न सक्छन्:

- **प्रत्यक्ष डाटा हेर्नुहोस्** (मौसम, सेयर मूल्य, डाटाबेस सोधपुछ)
- **ठ्याक्कै गणना गर्नुहोस्** (गणित, एकाइ रूपांतरण)
- **कार्यहरू लिनुहोस्** (इमेल पठाउनुहोस्, टिकट सिर्जना गर्नुहोस्, रेकर्ड अद्यावधिक गर्नुहोस्)
- **निजी प्रणालीहरू पहुँच गर्नुहोस्** (आन्तरिक API हरू, फाइल प्रणालीहरू)

---

## उपकरण कलिंग कसरी काम गर्छ

उपकरण कलिंग प्रक्रिया चार चरणहरूमा हुन्छ:

| चरण | के हुन्छ |
|-------|-------------|
| **१. उपकरण परिभाषित गर्नुहोस्** | तपाईं JSON Schema प्रयोग गरेर उपलब्ध फंक्शनहरू वर्णन गर्नुहुन्छ — नाम, वर्णन, र प्यारामिटरहरू |
| **२. मोडेल निर्णय गर्छ** | मोडेल तपाईंको सन्देश र उपकरण परिभाषाहरू प्राप्त गर्छ। यदि उपकरणले मद्दत गर्ने हो भने, यो `tool_calls` प्रतिक्रिया फिर्ता गर्छ, सामान्य पाठको सट्टा |
| **३. स्थानीय रूपमा कार्यान्वय गर्नुहोस्** | तपाईंको कोड उपकरण कल पार्स गर्छ, फंक्शन चलाउँछ, र परिणाम सङ्कलन गर्छ |
| **४. अन्तिम उत्तर दिनुहोस्** | तपाईं उपकरण नतिजा मोडेललाई फिर्ता पठाउनुहुन्छ, जसले अन्तिम प्रतिक्रिया उत्पादन गर्छ |

> **महत्त्वपूर्ण कुरा:** मोडेलले कहिल्यै कोड चलाउँदैन। यो केवल उपकरण कल गर्न अनुरोध मात्र गर्दछ। त्यो अनुरोधलाई सम्मान गर्ने वा नगर्ने निर्णय तपाईंको अनुप्रयोगले गर्छ — जसले तपाईंलाई पूर्ण नियन्त्रणमा राख्छ।

---

## कुन मोडेलहरूले उपकरण कलिंग समर्थन गर्छन्?

सबै मोडेलहरूले उपकरण कलिंग समर्थन गर्दैनन्। हालको Foundry Local क्याटलगमा, तलका मोडेलहरूले उपकरण कलिंग क्षमता राख्छन्:

| मोडेल | साइज | उपकरण कलिंग |
|-------|------|:---:|
| qwen2.5-0.5b | ८२२ MB | ✅ |
| qwen2.5-1.5b | १.८ GB | ✅ |
| qwen2.5-7b | ६.३ GB | ✅ |
| qwen2.5-14b | ११.३ GB | ✅ |
| qwen2.5-coder-0.5b | ८२२ MB | ✅ |
| qwen2.5-coder-1.5b | १.८ GB | ✅ |
| qwen2.5-coder-7b | ६.३ GB | ✅ |
| qwen2.5-coder-14b | ११.३ GB | ✅ |
| phi-4-mini | ४.६ GB | ✅ |
| phi-3.5-mini | २.६ GB | ❌ |
| phi-4 | १०.४ GB | ❌ |

> **सल्लाह:** यस ल्याबका लागि हामी **qwen2.5-0.5b** प्रयोग गर्छौं — यो सानो (८२२ MB डाउनलोड), छिटो, र भरपर्दो उपकरण कलिंग समर्थन भएको हो।

---

## सिक्ने उद्देश्यहरू

यस ल्याबको अन्त्यसम्म तपाईं सक्षम हुनुहुनेछ:

- उपकरण कलिंग ढाँचा र किन AI एजेन्टहरूका लागि महत्वपूर्ण छ व्याख्या गर्न
- OpenAI फंक्शन-कलिंग ढाँचाको प्रयोग गरेर उपकरण स्कीमा परिभाषित गर्न
- बहु-घुम्ती उपकरण कलिंग संवाद प्रवाह सम्हाल्न
- उपकरण कलहरू स्थानीय रूपमा कार्यान्वयन गर्न र मोडेललाई परिणाम फिर्ता गर्न
- उपकरण कलिंगका परिदृश्यहरूका लागि सही मोडेल चयन गर्न

---

## पूर्व शर्तहरू

| आवश्यकता | विवरण |
|-------------|---------|
| **Foundry Local CLI** | स्थापना गरिएको र तपाईंको `PATH` मा छ ([भाग १](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript वा C# SDK स्थापना गरिएको ([भाग २](part2-foundry-local-sdk.md)) |
| **उपकरण-कलिंग मोडेल** | qwen2.5-0.5b (स्वचालित रूपमा डाउनलोड हुनेछ) |

---

## अभ्यासहरू

### अभ्यास १ — उपकरण-कलिंग प्रवाह बुझ्नुहोस्

कोड लेख्नुअघि, यो श्रृंखला डायग्राम अध्ययन गर्नुहोस्:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**मुख्य अवलोकनहरू:**

१. तपाईंले उपकरणहरू अगाडि JSON Schema वस्तुहरूका रूपमा परिभाषित गर्नुहुन्छ  
२. मोडेलको प्रतिक्रिया सामान्य सामग्रीको सट्टा `tool_calls` समावेश गर्दछ  
३. प्रत्येक उपकरण कलसँग एउटा अनन्य `id` हुन्छ जुन परिणाम फिर्ता गर्दा सन्दर्भ दिनु पर्छ  
४. मोडेलले अन्तिम उत्तर बनाउन सबै अघिल्ला सन्देशहरू *र* उपकरण परिणामहरू देख्छ  
५. एउटा प्रतिसादमा धेरै उपकरण कलहरू हुन सक्छन्

> **चर्चा:** मोडेलले फंक्शनहरू सिधै कार्यान्वयन नगरी उपकरण कल किन फिर्ता गर्छ? यसले के सुरक्षा लाभ प्रदान गर्दछ?

---

### अभ्यास २ — उपकरण स्कीमा परिभाषित गर्नुहोस्

उपकरणहरू मानक OpenAI फंक्शन-कलिंग ढाँचाको प्रयोग गरेर परिभाषित गरिन्छ। प्रत्येक उपकरणलाई चाहिन्छ:

- **`type`**: सँधै `"function"`  
- **`function.name`**: वर्णनात्मक फंक्शन नाम (जस्तै `get_weather`)  
- **`function.description`**: स्पष्ट वर्णन — मोडेल यसले उपकरण कल कहिले गर्ने निर्णय गर्छ  
- **`function.parameters`**: अपेक्षित आर्गुमेन्ट विवरण गर्ने JSON Schema वस्तु  

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
  
> **उपकरण विवरणका लागि सर्वोत्तम अभ्यासहरू:**  
> - विशेष हुनुहोस्: "एक शहरको हालको मौसम पत्ता लगाउनुहोस्" भन्दा "मौसम पत्ता लगाउनुहोस्" राम्रो हुन्छ  
> - प्यारामिटरहरू स्पष्ट वर्णन गर्नुहोस्: मोडेल मूल्यहरू भर्नको लागि यी विवरणहरू पढ्छ  
> - आवश्यक र वैकल्पिक प्यारामिटरहरू मार्क गर्नुहोस् — यसले मोडेललाई के सोध्ने निर्णय गर्न मद्दत गर्छ

---

### अभ्यास ३ — उपकरण-कलिंग उदाहरणहरू चलाउनुहोस्

हरेक भाषा नमूनाले दुई उपकरणहरू परिभाषित गर्दछ (`get_weather` र `get_population`), उपकरण प्रयोग सुरु गर्ने प्रश्न पठाउँछ, उपकरण स्थानीय रूपमा चलाउँछ, र अन्तिम उत्तरका लागि परिणाम फिर्ता पठाउँछ।

<details>
<summary><strong>🐍 Python</strong></summary>

**पूर्व शर्तहरू:**  
```bash
cd python
python -m venv venv

# विन्डोज (पावरशेल):
venv\Scripts\Activate.ps1
# म्याकओएस / लिनक्स:
source venv/bin/activate

pip install -r requirements.txt
```
  
**चलाउनुहोस्:**  
```bash
python foundry-local-tool-calling.py
```
  
**अपेक्षित नतिजा:**  
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**कोड walkthrough** (`python/foundry-local-tool-calling.py`):
  
```python
# उपकरणहरूलाई एक फंक्शन स्कीमाहरूको सूचीको रूपमा परिभाषित गर्नुहोस्
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

# उपकरणहरूसँग पठाउनुहोस् — मोडेलले सामग्रीको सट्टा tool_calls फिर्ता गर्न सक्छ
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# जाँच गर्नुहोस् कि मोडेलले उपकरण कल गर्न चाहन्छ वा छैन
if response.choices[0].message.tool_calls:
    # उपकरण कार्यान्वयन गर्नुहोस् र परिणाम पुन: पठाउनुहोस्
    ...
```
  
</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**पूर्व शर्तहरू:**  
```bash
cd javascript
npm install
```
  
**चलाउनुहोस्:**  
```bash
node foundry-local-tool-calling.mjs
```
  
**अपेक्षित नतिजा:**  
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**कोड walkthrough** (`javascript/foundry-local-tool-calling.mjs`):

यो उदाहरणले OpenAI SDK को सट्टा मूल Foundry Local SDK को `ChatClient` प्रयोग गर्छ, जसले `createChatClient()` मेथडको सुविधा देखाउँछ:

```javascript
// मोडेल वस्तुबाट सिधै ChatClient प्राप्त गर्नुहोस्
const chatClient = model.createChatClient();

// उपकरणहरू सहित पठाउनुहोस् — ChatClient ले OpenAI अनुरूप ढाँचा व्यवस्थापन गर्दछ
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// उपकरण कलहरूको जाँच गर्नुहोस्
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // उपकरणहरू कार्यान्वयन गर्नुहोस् र परिणामहरू फिर्ता पठाउनुहोस्
    ...
}
```
  
</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**पूर्व शर्तहरू:**  
```bash
cd csharp
dotnet restore
```
  
**चलाउनुहोस्:**  
```bash
dotnet run toolcall
```
  
**अपेक्षित नतिजा:**  
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**कोड walkthrough** (`csharp/ToolCalling.cs`):

C# ले उपकरणहरू परिभाषित गर्न `ChatTool.CreateFunctionTool` सहयोगी प्रयोग गर्छ:

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

### अभ्यास ४ — उपकरण-कलिंग संवाद प्रवाह

सन्देश संरचना बुझ्न महत्वपूर्ण छ। यहाँ सम्पूर्ण प्रवाह छ, हरेक चरणमा `messages` एरे देखाउँदै:

**चरण १ — प्रारम्भिक अनुरोध:**  
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```
  
**चरण २ — मोडेलले उपकरण कलहरू सहित प्रतिक्रिया गर्छ (सामग्री होइन):**  
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
  
**चरण ३ — तपाईंले सहायक सन्देश र उपकरण परिणाम थप्नुहुन्छ:**  
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
  
**चरण ४ — मोडेलले उपकरण परिणाम प्रयोग गरेर अन्तिम उत्तर उत्पादन गर्छ।**

> **महत्त्वपूर्ण:** उपकरण सन्देशमा `tool_call_id` उपकरण कलको `id` सँग मेल खानु पर्छ। मोडेल यसरी परिणामहरूलाई अनुरोधहरूसँग जोड्छ।

---

### अभ्यास ५ — एकै पटक धेरै उपकरण कलहरू

एउटा मोडेलले एउटै प्रतिक्रियामा धेरै उपकरण कलहरू अनुरोध गर्न सक्छ। प्रयोगकर्ता सन्देश परिवर्तन गरेर धेरै कलहरू ट्रिगर गर्नुहोस्:

```python
# Python मा — प्रयोगकर्ता सन्देश परिवर्तन गर्नुहोस्:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```
  
```javascript
// जाभास्क्रिप्टमा — प्रयोगकर्ता सन्देश परिवर्तन गर्नुहोस्:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```
  
मोडेलले दुई `tool_calls` फिर्ता गर्नेछ — एउटा `get_weather` र अर्को `get_population` का लागि। तपाईंको कोडले पहिले नै सबै उपकरण कलहरूमा लूप गर्छ।

> **कोशिश गर्नुहोस्:** प्रयोगकर्ता सन्देश परिवर्तन गरी नमूना फेरि चलाउनुहोस्। के मोडेलले दुबै उपकरण कल गर्छ?

---

### अभ्यास ६ — आफ्नो उपकरण थप्नुहोस्

एक नमूनामा नयाँ उपकरण थप्नुहोस्। उदाहरणका लागि, `get_time` उपकरण थप्नुहोस्:

1. उपकरण स्कीमा परिभाषित गर्नुहोस्:  
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
  
2. कार्यान्वयन लॉजिक थप्नुहोस्:  
```python
# पाइथन
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # वास्तविक अनुप्रयोगमा, टाइमजोन पुस्तकालय प्रयोग गर्नुहोस्
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... अवस्थित उपकरणहरू ...
```
  
3. उपकरणलाई `tools` एरैमामा थप्नुहोस् र परीक्षण गर्नुहोस्: "टोकियोमा कति बजिरहेको छ?"

> **चुनौती:** यस्तो उपकरण थप्नुहोस् जसले गणना गर्छ, जस्तै `convert_temperature` जसले सेल्सियस र फारेनहाइट बीच रूपान्तरण गर्छ। यसलाई "100°F लाई सेल्सियसमा रूपान्तरण गर्नुहोस्।" बाट परीक्षण गर्नुहोस्।

---

### अभ्यास ७ — SDK को ChatClient प्रयोग गरेर उपकरण कलिंग (JavaScript)

JavaScript नमूनाले OpenAI SDK को सट्टा SDK को मूल `ChatClient` नै प्रयोग गर्छ। यसले OpenAI क्लाइन्ट निर्माण गर्न आवश्यक हटाउँछ:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient मोडेल वस्तुबाट सिधै बनाइन्छ
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat ले उपकरणहरूलाई दोस्रो प्यारामिटरको रूपमा स्वीकार्छ
const response = await chatClient.completeChat(messages, tools);
```
  
Python दृष्टिकोणसँग तुलना गर्नुहोस् जसले OpenAI SDK स्पष्ट रूपमा प्रयोग गर्छ:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```
  
दुबै ढाँचाहरू मान्य छन्। `ChatClient` सुविधाजनक छ; OpenAI SDK ले OpenAI का पूर्ण पैरामीटरहरू पहुँच प्रदान गर्छ।

> **कोशिश गर्नुहोस्:** JavaScript नमूनालाई `ChatClient` को सट्टा OpenAI SDK प्रयोग गर्न परिवर्तन गर्नुहोस्। तपाईंलाई `import OpenAI from "openai"` आवश्यक पर्छ र क्लाइन्ट `manager.urls[0]` बाट endpoint सहित भवन गर्नु पर्छ।

---

### अभ्यास ८ — tool_choice बुझ्न

`tool_choice` प्यारामिटर मोडेलले उपकरण प्रयोग अनिवार्य हो या स्वतन्त्र छनौट गर्न सक्छ नियन्त्रण गर्छ:

| मान | व्यवहार |
|-------|-----------|
| `"auto"` | मोडेलले उपकरण कल गर्नु छ कि छैन निर्णय गर्छ (डिफल्ट) |
| `"none"` | मोडेलले कुनै उपकरण कल गर्दैन, चाहे उपलब्ध किन न होस् |
| `"required"` | मोडेलले कम्तीमा एक उपकरण कल गर्नै पर्छ |
| `{"type": "function", "function": {"name": "get_weather"}}` | मोडेलले निर्दिष्ट उपकरण कल गर्नै पर्छ |

Python नमूनामा हरेक विकल्प प्रयास गर्नुहोस्:

```python
# मोडललाई get_weather कल गर्न बाध्य पार्नुहोस्
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```
  
> **सूचना:** सबै मोडेलहरूले सबै `tool_choice` विकल्पहरू समर्थन नगर्न सक्छन्। यदि मोडेलले `"required"` समर्थन गर्दैन भने, यसले यो सेटिंगलाई बेवास्ता गरी `"auto"` जस्तो व्यवहार गर्न सक्छ।

---

## सामान्य समस्या

| समस्या | समाधान |
|---------|----------|
| मोडेलले कहिल्यै उपकरण कल गर्दैन | सुनिश्चित गर्नुहोस् उपकरण-कलिंग मोडेल प्रयोग भइरहेको छ (जस्तै qwen2.5-0.5b)। माथिको तालिका जाँच गर्नुहोस्। |
| `tool_call_id` मेल खाँदैन | जहिले पनि उपकरण कल प्रतिक्रियाको `id` प्रयोग गर्नुहोस्, हार्डकोड गरिएको मान होइन |
| मोडेलले `arguments` मा बिग्रिएको JSON फिर्ता गर्छ | साना मोडेलहरू कहिलेकाहीं अमान्य JSON उत्पादन गर्न सक्छन्। `JSON.parse()` लाई try/catch मा राख्नुहोस् |
| मोडेलले अवस्थित नभएको उपकरण कल गर्छ | तपाईंको `execute_tool` फंक्शनमा डिफल्ट ह्याण्डलर थप्नुहोस् |
| अनन्त उपकरण-कलिंग लूप | अधिकतम घुम्ती संख्या (जस्तै ५) सेट गर्नुहोस्, जसले अनियन्त्रित लूप रोक्छ |

---

## मुख्य सिकाइहरू

१. **उपकरण कलिंग** ले मोडेलहरूलाई उत्तर अनुमान गर्नुभन्दा फंक्शन कार्यान्वयन अनुरोध गर्न दिन्छ  
२. मोडेलले **कहिल्यै कोड चलाउँदैन**; तपाईंको अनुप्रयोगले के चलाउने निर्णय गर्छ  
३. उपकरणहरू OpenAI फंक्शन-कलिंग ढाँचालाई पालना गर्ने **JSON Schema** वस्तुहरू हुन्  
४. संवादले **बहु-घुम्ती ढाँचा** प्रयोग गर्छ: प्रयोगकर्ता, त्यसपछि सहायक (tool_calls), त्यसपछि उपकरण (परिणाम), त्यसपछि सहायक (अन्तिम उत्तर)  
५. सधैं उपकरण कलिंग समर्थन गर्ने मोडेल प्रयोग गर्नुहोस् (Qwen 2.5, Phi-4-mini)  
६. SDK को `createChatClient()` ले OpenAI क्लाइन्ट बनाए बिना उपकरण कलिंग अनुरोध गर्न सजिलो तरीका दिन्छ

---

[भाग १२: Zava क्रिएटिभ लेखकको लागि वेब UI निर्माण गर्दै](part12-zava-ui.md) मा जारी राख्नुहोस् र बहु-एजेन्ट पाइपलाइनमा वास्तविक-समय स्ट्रिमिंगको साथ ब्राउजर-आधारित फ्रन्टएण्ड थप्नुहोस्।

---

[← भाग १०: अनुकूलित मोडेलहरू](part10-custom-models.md) | [भाग १२: Zava लेखक UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:  
यो दस्तावेज AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) प्रयोग गरी अनुवाद गरिएको हो। हामी शुद्धताका लागि प्रयास गर्छौं, तर कृपया अधिसूचना हुनुहोस् कि स्वचालित अनुवादमा गल्ती वा त्रुटिहरू हुन सक्दछन्। मूल दस्तावेज यसको मूल भाषामा आधिकारिक स्रोत मानिनु पर्छ। महत्वपूर्ण जानकारीका लागि, व्यावसायिक मान्छे द्वारा अनुवाद सिफारिस गरिन्छ। यस अनुवादको प्रयोगबाट उत्पन्न कुनै पनि गलतफहमी वा भ्रमका लागि हामी जिम्मेवार छैनौं।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->