![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग 11: स्थानीय मॉडलों के साथ टूल कॉलिंग

> **लक्ष्य:** अपने स्थानीय मॉडल को बाहरी फ़ंक्शंस (टूल्स) कॉल करने में सक्षम बनाएं ताकि यह वास्तविक समय का डेटा प्राप्त कर सके, गणनाएँ कर सके, या APIs के साथ इंटरैक्ट कर सके — ये सभी आपके डिवाइस पर निजी रूप से चलते हैं।

## टूल कॉलिंग क्या है?

टूल कॉलिंग (जिसे **फ़ंक्शन कॉलिंग** के नाम से भी जाना जाता है) एक भाषा मॉडल को आपके द्वारा परिभाषित फ़ंक्शंस के निष्पादन का अनुरोध करने देती है। उत्तर अनुमान लगाने के बजाय, मॉडल पहचानता है कि कब एक टूल मदद करेगा और आपकी कोड के निष्पादन के लिए एक संरचित अनुरोध लौटाता है। आपका एप्लिकेशन फ़ंक्शन चलाता है, परिणाम वापस भेजता है, और मॉडल उस जानकारी को अपने अंतिम उत्तर में सम्मिलित करता है।

![Tool-calling flow](../../../images/tool-calling-flow.svg)

यह पैटर्न एजेंट बनाने के लिए आवश्यक है जो कर सकते हैं:

- **लाइव डेटा देखें** (मौसम, स्टॉक कीमतें, डेटाबेस क्वेरीज़)
- **सटीक गणनाएँ करें** (गणित, इकाई रूपांतरण)
- **कार्रवाइयाँ करें** (ईमेल भेजना, टिकट बनाना, रिकॉर्ड अपडेट करना)
- **निजी सिस्टम्स को एक्सेस करें** (आंतरिक APIs, फाइल सिस्टम)

---

## टूल कॉलिंग कैसे काम करता है

टूल कॉलिंग फ्लो में चार चरण होते हैं:

| चरण | क्या होता है |
|-------|-------------|
| **1. टूल्स परिभाषित करें** | आप JSON Schema का उपयोग करके उपलब्ध फ़ंक्शंस का वर्णन करते हैं — नाम, विवरण, और पैरामीटर |
| **2. मॉडल निर्णय लेता है** | मॉडल आपका संदेश और टूल परिभाषाएँ प्राप्त करता है। अगर टूल मदद करेगा, तो वह पाठ्य उत्तर के बजाय `tool_calls` प्रतिक्रिया लौटाता है |
| **3. स्थानीय रूप से निष्पादित करें** | आपका कोड टूल कॉल को पार्स करता है, फ़ंक्शन चलाता है, और परिणाम एकत्र करता है |
| **4. अंतिम उत्तर दें** | आप टूल परिणाम मॉडल को वापस भेजते हैं, जो अपना अंतिम जवाब बनाता है |

> **महत्वपूर्ण:** मॉडल कभी भी कोड निष्पादित नहीं करता। यह केवल *अनुरोध* करता है कि टूल को कॉल किया जाए। आपकी एप्लिकेशन यह निर्णय लेती है कि उस अनुरोध को मानना है या नहीं — यह आपको पूर्ण नियंत्रण में रखता है।

---

## कौन से मॉडल टूल कॉलिंग का समर्थन करते हैं?

हर मॉडल टूल कॉलिंग का समर्थन नहीं करता। वर्तमान Foundry Local कैटलॉग में निम्नलिखित मॉडल टूल-कॉलिंग क्षमता रखते हैं:

| मॉडल | आकार | टूल कॉलिंग |
|-------|------|:---:|
| qwen2.5-0.5b | 822 एमबी | ✅ |
| qwen2.5-1.5b | 1.8 जीबी | ✅ |
| qwen2.5-7b | 6.3 जीबी | ✅ |
| qwen2.5-14b | 11.3 जीबी | ✅ |
| qwen2.5-coder-0.5b | 822 एमबी | ✅ |
| qwen2.5-coder-1.5b | 1.8 जीबी | ✅ |
| qwen2.5-coder-7b | 6.3 जीबी | ✅ |
| qwen2.5-coder-14b | 11.3 जीबी | ✅ |
| phi-4-mini | 4.6 जीबी | ✅ |
| phi-3.5-mini | 2.6 जीबी | ❌ |
| phi-4 | 10.4 जीबी | ❌ |

> **सुझाव:** इस लैब के लिए हम **qwen2.5-0.5b** का उपयोग करते हैं — यह छोटा है (822 एमबी डाउनलोड), तेज़ है, और विश्वसनीय टूल-कॉलिंग समर्थन प्रदान करता है।

---

## सीखने के उद्देश्य

इस लैब के अंत तक आप सक्षम होंगे:

- टूल-कॉलिंग पैटर्न को समझाना और यह AI एजेंट्स के लिए क्यों महत्वपूर्ण है
- OpenAI फ़ंक्शन-कॉलिंग प्रारूप का उपयोग करके टूल स्कीमा परिभाषित करना
- मल्टी-टर्न टूल-कॉलिंग संवाद प्रवाह संभालना
- टूल कॉल्स को स्थानीय रूप से निष्पादित करना और मॉडल को परिणाम वापस भेजना
- टूल-कॉलिंग परिदृश्यों के लिए सही मॉडल चुनना

---

## आवश्यकताएँ

| आवश्यकता | विवरण |
|-------------|---------|
| **Foundry Local CLI** | स्थापित और आपके `PATH` पर ([भाग 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript, या C# SDK स्थापित ([भाग 2](part2-foundry-local-sdk.md)) |
| **एक टूल-कॉलिंग मॉडल** | qwen2.5-0.5b (स्वचालित रूप से डाउनलोड होगा) |

---

## अभ्यास

### अभ्यास 1 — टूल-कॉलिंग फ्लो को समझें

कोड लिखने से पहले, इस अनुक्रम चित्र का अध्ययन करें:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**मुख्य अवलोकन:**

1. आप टूल्स को पहले JSON Schema ऑब्जेक्ट के रूप में परिभाषित करते हैं
2. मॉडल की प्रतिक्रिया में `tool_calls` होते हैं, सामान्य सामग्री के बजाय
3. प्रत्येक टूल कॉल का एक अनूठा `id` होता है जिसे आपको परिणाम लौटाते समय संदर्भित करना होता है
4. मॉडल अंतिम उत्तर बनाते समय सभी पिछली संदेशों *साथ ही* टूल परिणाम भी देखता है
5. एक ही प्रतिक्रिया में एक से अधिक टूल कॉल हो सकते हैं

> **चर्चा:** मॉडल सीधे फंक्शन निष्पादित करने के बजाय टूल कॉल क्यों लौटाता है? यह कौन से सुरक्षा लाभ प्रदान करता है?

---

### अभ्यास 2 — टूल स्कीमा परिभाषित करना

टूल OpenAI फ़ंक्शन-कॉलिंग मानक प्रारूप का उपयोग करके परिभाषित किए जाते हैं। प्रत्येक टूल को चाहिए:

- **`type`**: हमेशा `"function"`
- **`function.name`**: एक वर्णनात्मक फ़ंक्शन नाम (जैसे `get_weather`)
- **`function.description`**: स्पष्ट विवरण — मॉडल यह तय करने के लिए इसका उपयोग करता है कि कब टूल कॉल करना है
- **`function.parameters`**: JSON Schema ऑब्जेक्ट जो अपेक्षित आर्ग्यूमेंट्स का वर्णन करता है

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

> **टूल विवरण के लिए सर्वोत्तम अभ्यास:**
> - विशिष्ट बनें: "Get the current weather for a given city" बेहतर है बजाय "Get weather" के
> - पैरामीटर स्पष्ट रूप से वर्णन करें: मॉडल इन विवरणों को पढ़कर सही मान भरता है
> - आवश्यक बनाम वैकल्पिक पैरामीटर को चिह्नित करें — इससे मॉडल को पता चलता है कि क्या पूछना है

---

### अभ्यास 3 — टूल-कॉलिंग उदाहरण चलाना

प्रत्येक भाषा उदाहरण दो टूल्स (`get_weather` और `get_population`) परिभाषित करता है, एक सवाल भेजता है जो टूल इस्तेमाल को ट्रिगर करता है, टूल स्थानीय रूप से निष्पादित करता है, और परिणाम वापस भेजता है अंतिम उत्तर के लिए।

<details>
<summary><strong>🐍 Python</strong></summary>

**आवश्यकताएँ:**
```bash
cd python
python -m venv venv

# विंडोज़ (पावरशेल):
venv\Scripts\Activate.ps1
# मैकओएस / लिनक्स:
source venv/bin/activate

pip install -r requirements.txt
```

**चलाएँ:**
```bash
python foundry-local-tool-calling.py
```

**अपेक्षित आउटपुट:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**कोड व्याख्या** (`python/foundry-local-tool-calling.py`):

```python
# उपकरणों को फ़ंक्शन स्कीमों की एक सूची के रूप में परिभाषित करें
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

# उपकरणों के साथ भेजें — मॉडल कंटेंट के बजाय टूल_कॉल्स लौटा सकता है
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# जांचें कि क्या मॉडल कोई उपकरण कॉल करना चाहता है
if response.choices[0].message.tool_calls:
    # उपकरण को निष्पादित करें और परिणाम वापस भेजें
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**आवश्यकताएँ:**
```bash
cd javascript
npm install
```

**चलाएँ:**
```bash
node foundry-local-tool-calling.mjs
```

**अपेक्षित आउटपुट:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**कोड व्याख्या** (`javascript/foundry-local-tool-calling.mjs`):

यह उदाहरण OpenAI SDK की बजाय नेटिव Foundry Local SDK के `ChatClient` का उपयोग करता है, जो `createChatClient()` विधि की सुविधा दिखाता है:

```javascript
// मॉडल ऑब्जेक्ट से सीधे एक ChatClient प्राप्त करें
const chatClient = model.createChatClient();

// टूल्स के साथ भेजें — ChatClient OpenAI-संगत प्रारूप को संभालता है
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// टूल कॉल्स की जांच करें
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // टूल्स को निष्पादित करें और परिणाम वापस भेजें
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**आवश्यकताएँ:**
```bash
cd csharp
dotnet restore
```

**चलाएँ:**
```bash
dotnet run toolcall
```

**अपेक्षित आउटपुट:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**कोड व्याख्या** (`csharp/ToolCalling.cs`):

C# टूल परिभाषित करने के लिए `ChatTool.CreateFunctionTool` हेल्पर का उपयोग करता है:

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

### अभ्यास 4 — टूल-कॉलिंग संवाद प्रवाह

संदेश संरचना को समझना महत्वपूर्ण है। यहाँ पूरा फ्लो है, जो प्रत्येक चरण पर `messages` ऐरे दिखाता है:

**चरण 1 — प्रारंभिक अनुरोध:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**चरण 2 — मॉडल टूल_कॉल्स के साथ जवाब देता है (सामग्री नहीं):**
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

**चरण 3 — आप सहायक संदेश और टूल परिणाम जोड़ते हैं:**
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

**चरण 4 — मॉडल टूल परिणाम के साथ अंतिम उत्तर बनाता है।**

> **महत्वपूर्ण:** टूल संदेश में `tool_call_id` को टूल कॉल के `id` से मेल खाना चाहिए। इससे मॉडल परिणामों को अनुरोधों के साथ जोड़ता है।

---

### अभ्यास 5 — एकाधिक टूल कॉल

एक मॉडल एक ही प्रतिक्रिया में कई टूल कॉल कर सकता है। उपयोगकर्ता संदेश को बदलकर कई कॉल ट्रिगर करें:

```python
# पायथन में — उपयोगकर्ता संदेश बदलें:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// JavaScript में — उपयोगकर्ता संदेश बदलें:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

मॉडल को दो `tool_calls` लौटाने चाहिए — एक `get_weather` के लिए और एक `get_population` के लिए। आपका कोड पहले से ही सभी टूल कॉल्स को लूप करता है।

> **प्रयास करें:** उपयोगकर्ता संदेश संशोधित करें और उदाहरण पुनः चलाएँ। क्या मॉडल दोनों टूल्स कॉल करता है?

---

### अभ्यास 6 — अपना टूल जोड़ें

किसी एक उदाहरण में एक नया टूल जोड़ें। उदाहरण के लिए, `get_time` टूल जोड़ें:

1. टूल स्कीमा परिभाषित करें:
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

2. निष्पादन लॉजिक जोड़ें:
```python
# पाइथन
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # एक वास्तविक ऐप में, एक टाइमज़ोन लाइब्रेरी का उपयोग करें
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... मौजूदा टूल्स ...
```

3. टूल्स ऐरे में टूल जोड़ें और टेस्ट करें: "What time is it in Tokyo?"

> **चुनौती:** एक ऐसा टूल जोड़ें जो गणना करता हो, जैसे `convert_temperature` जो सेल्सियस और फ़ारेनहाइट के बीच रूपांतरण करता हो। इसे टेस्ट करें: "Convert 100°F to Celsius."

---

### अभ्यास 7 — SDK के ChatClient के साथ टूल कॉलिंग (JavaScript)

JavaScript उदाहरण पहले से ही OpenAI SDK के बजाय SDK के नेटिव `ChatClient` का उपयोग करता है। यह एक सुविधा है जो OpenAI क्लाइंट बनाने की आवश्यकता को समाप्त करता है:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient मॉडल ऑब्जेक्ट से सीधे बनाया गया है
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat दूसरे पैरामीटर के रूप में टूल्स स्वीकार करता है
const response = await chatClient.completeChat(messages, tools);
```

इसकी तुलना Python दृष्टिकोण से करें जो OpenAI SDK का स्पष्ट उपयोग करता है:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

दोनों पैटर्न मान्य हैं। `ChatClient` अधिक सुविधाजनक है; OpenAI SDK आपको OpenAI के सभी पैरामीटर का उपयोग करने देता है।

> **प्रयास करें:** JavaScript उदाहरण को OpenAI SDK के बजाय `ChatClient` के साथ उपयोग करने के लिए संशोधित करें। आपको `import OpenAI from "openai"` करना होगा और `manager.urls[0]` से एंडपॉइंट के साथ क्लाइंट बनाना होगा।

---

### अभ्यास 8 — tool_choice को समझना

`tool_choice` पैरामीटर नियंत्रित करता है कि मॉडल *जरूर* टूल का उपयोग करे या स्वतंत्र हो:

| मान | व्यवहार |
|-------|-----------|
| `"auto"` | मॉडल तय करता है कि टूल कॉल करना है या नहीं (डिफ़ॉल्ट) |
| `"none"` | मॉडल कोई टूल कॉल नहीं करेगा, भले टूल प्रदान किए गए हों |
| `"required"` | मॉडल को कम से कम एक टूल कॉल करना अनिवार्य है |
| `{"type": "function", "function": {"name": "get_weather"}}` | मॉडल को निर्दिष्ट टूल कॉल करना अनिवार्य है |

Python उदाहरण में प्रत्येक विकल्प आज़माएं:

```python
# मॉडल को get_weather कॉल करने के लिए बाध्य करें
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **ध्यान दें:** सभी `tool_choice` विकल्प हर मॉडल द्वारा समर्थित नहीं हो सकते। यदि मॉडल `"required"` का समर्थन नहीं करता, तो यह सेटिंग को अनदेखा कर सकता है और `"auto"` की तरह व्यवहार कर सकता है।

---

## सामान्य गलतियाँ

| समस्या | समाधान |
|---------|----------|
| मॉडल कभी टूल कॉल नहीं करता | सुनिश्चित करें कि आप टूल-कॉलिंग मॉडल का उपयोग कर रहे हैं (जैसे qwen2.5-0.5b)। ऊपर दी गई तालिका देखें। |
| `tool_call_id` मेल नहीं खाता | हमेशा टूल कॉल प्रतिक्रिया से `id` का उपयोग करें, हार्डकोडेड मान नहीं |
| मॉडल `arguments` में दोषपूर्ण JSON लौटाता है | छोटे मॉडल कभी-कभी अवैध JSON उत्पन्न करते हैं। `JSON.parse()` को try/catch में लपेटें |
| मॉडल ऐसे टूल को कॉल करता है जो मौजूद नहीं है | अपनी `execute_tool` फ़ंक्शन में डिफ़ॉल्ट हैंडलर जोड़ें |
| अनंत टूल-कॉलिंग लूप | सीमित राउंड्स (जैसे 5) सेट करें जिससे अनियंत्रित लूप रोका जा सके |

---

## मुख्य निष्कर्ष

1. **टूल कॉलिंग** मॉडल को उत्तर अनुमानित करने की बजाय फ़ंक्शन निष्पादन का अनुरोध करने देती है
2. मॉडल **कभी कोड निष्पादित नहीं करता**; आपकी एप्लिकेशन निर्णय लेती है कि क्या चलाना है
3. टूल को **JSON Schema** वस्तुओं के रूप में OpenAI फ़ंक्शन-कॉलिंग प्रारूप के अनुसार परिभाषित किया जाता है
4. संवाद में **मल्टी-टर्न पैटर्न** होता है: उपयोगकर्ता, फिर सहायक (tool_calls), फिर टूल (परिणाम), फिर सहायक (अंतिम उत्तर)
5. हमेशा **टूल-कॉलिंग का समर्थन करने वाले मॉडल** का उपयोग करें (Qwen 2.5, Phi-4-mini)
6. SDK का `createChatClient()` एक सुविधाजनक तरीका प्रदान करता है जिससे OpenAI क्लाइंट बनाए बिना टूल-कॉलिंग अनुरोध किया जा सके

---

आगे बढ़ें [भाग 12: Zava Creative Writer के लिए वेब UI बनाना](part12-zava-ui.md) को ताकि वास्तविक समय स्ट्रीमिंग के साथ मल्टी-एजेंट पाइपलाइन में ब्राउज़र आधारित फ्रंट एंड जोड़ा जा सके।

---

[← भाग 10: कस्टम मॉडल्स](part10-custom-models.md) | [भाग 12: Zava Writer UI →](part12-zava-ui.md)