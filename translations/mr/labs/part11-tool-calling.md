![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग 11: स्थानिक मॉडेल्ससह टूल कॉलिंग

> **उद्दिष्ट:** आपल्या स्थानिक मॉडेलला बाह्य फंक्शन्स (टूल्स) कॉल करण्याची परवानगी द्या जेणे करून ते रिअल-टाइम डेटा प्राप्त करू शकेल, गणना करू शकेल, किंवा API शी संवाद साधू शकेल — हे सर्व आपल्याच उपकरणावर खाजगीपणे चालते.

## टूल कॉलिंग म्हणजे काय?

टूल कॉलिंग (ज्याला **फंक्शन कॉलिंग** देखील म्हणतात) भाषेच्या मॉडेलला आपल्याने परिभाषित केलेल्या फंक्शन्सच्या कार्यान्वयनासाठी विनंती करण्याची अनुमती देते. उत्तराचा अंदाज लावण्याऐवजी, मॉडेल ओळखते की टूल मदत करू शकेल आणि आपला कोड कार्यान्वित करण्यासाठी एका संरचित विनंतीला परत देते. आपला अनुप्रयोग फंक्शन चालवतो, निकाल परत पाठवतो आणि मॉडेल त्या माहितीस आपल्या अंतिम प्रतिक्रियेत समाविष्ट करतो.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

या पॅटर्नचे उपयोग करून एजंट्स तयार करता येतात जे:

- **प्रत्यक्ष डेटा शोधतात** (हवामान, स्टॉकची किंमत, डेटाबेस क्वेरीज)
- **अचूक गणना करतात** (गणित, युनिट रूपांतरणे)
- **कारवाया करतात** (ईमेल पाठवणे, तिकीट तयार करणे, नोंदी अद्ययावत करणे)
- **खाजगी प्रणाली वापरतात** (आंतरिक API, फाइल सिस्टम)

---

## टूल कॉलिंग कसे कार्य करते

टूल कॉलिंग प्रवाहात चार टप्पे असतात:

| टप्पा | काय घडते |
|-------|-----------|
| **1. टूल्स Define करा** | JSON Schema वापरून उपलब्ध फंक्शन्स वर्णन करा — नाव, वर्णन, आणि पॅरामीटर्स |
| **2. मॉडेल निर्णय घेते** | मॉडेलला आपला संदेश आणि टूल्सची व्याख्या मिळते. जर टूल मदत करेल, तर ते टेक्स्ट उत्तराऐवजी `tool_calls` प्रतिसाद परत करते |
| **3. स्थानिकपणे कार्यान्वित करा** | आपला कोड टूल कॉल पार्स करतो, फंक्शन चालवतो आणि निकाल गोळा करतो |
| **4. अंतिम उत्तर देणे** | आपण टूलचा निकाल मॉडेलला परत पाठवता, ज्यामुळे ते आपली अंतिम प्रतिक्रिया तयार करते |

> **महत्त्वाचा मुद्दा:** मॉडेल कधीही कोड चालवणार नाही. ते फक्त टूल कॉल करण्याची *विनंती* करते. आपला अनुप्रयोग तो विनंती मान्य करणार की नाही हे ठरवतो — आपण पूर्ण नियंत्रणात राहता.

---

## कोणते मॉडेल टूल कॉलिंगला समर्थन देतात?

प्रत्येक मॉडेल टूल कॉलिंगला समर्थन देत नाही. सध्याच्या Foundry Local कॅटलॉगमध्ये खालील मॉडेल्सला टूल कॉलिंगची क्षमता आहे:

| मॉडेल | आकार | टूल कॉलिंग |
|-------|-------|:---------:|
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

> **टीप:** या प्रयोगशाळेसाठी आपण **qwen2.5-0.5b** वापरतो — हे लहान (822 MB डाउनलोड), जलद आणि विश्वासार्ह टूल कॉलिंग समर्थन असलेले मॉडेल आहे.

---

## शिका काय काय

या प्रयोगशाळेच्या शेवटी आपण सक्षम असाल:

- टूल कॉलिंग पॅटर्न समजावून सांगणे आणि AI एजंट्ससाठी त्याचे महत्त्व
- OpenAI फंक्शन कॉलिंग फॉरमॅट वापरून टूल स्कीमास डिफाइन करणे
- बहु-चरण टूल कॉलिंग संभाषण प्रवाह हाताळणे
- स्थानिकपणे टूल कॉल्स चालवणे आणि निकाल मॉडेलला परत पाठवणे
- टूल कॉलिंग परिस्थितीसाठी योग्य मॉडेल निवडणे

---

## पूर्वापेक्षा आवश्यक

| गरज | तपशील |
|----------|---------|
| **Foundry Local CLI** | इंस्टॉल केलेले आणि तुमच्या `PATH` मध्ये ([भाग 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript किंवा C# SDK इंस्टॉल केलेले ([भाग 2](part2-foundry-local-sdk.md)) |
| **टूल कॉलिंग मॉडेल** | qwen2.5-0.5b (स्वतःहून डाउनलोड होईल) |

---

## सराव

### सराव 1 — टूल कॉलिंग फ्लो समजून घ्या

कोड लिहण्याआधी, हा सिक्वेन्स डायग्राम अभ्यासा:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**महत्त्वाच्या निरीक्षणे:**

1. आपण टूल्स JSON Schema ऑब्जेक्ट्स म्हणून आधीच परिभाषित करता
2. मॉडेलचा प्रतिसाद नियमित सामग्रीऐवजी `tool_calls` समाविष्ट करतो
3. प्रत्येक टूल कॉलचे एक अनन्य `id` असते ज्याचा संदर्भ निकाल परत करताना द्यावा लागतो
4. अंतिम उत्तर तयार करताना मॉडेलला आधीचे सर्व संदेश *आणि* टूल परिणाम दिसतात
5. एका प्रतिसादात अनेक टूल कॉल होऊ शकतात

> **चर्चा:** मॉडेल थेट फंक्शन्स चालवण्याऐवजी टूल कॉल परत का करते? यामुळे सुरक्षा कशी वाढते?

---

### सराव 2 — टूल स्कीमास डिफाइन करणे

टूल्स ओपनAI फंक्शन कॉलिंग फॉरमॅट वापरून परिभाषित केले जातात. प्रत्येक टूलसाठी आवश्यक:

- **`type`**: नेहमी `"function"`
- **`function.name`**: समजण्यास सोपे फंक्शन नाव (उदा. `get_weather`)
- **`function.description`**: स्पष्ट वर्णन — मॉडेलला टूल कॉल केव्हा करायचा हे ठरवण्यासाठी
- **`function.parameters`**: अपेक्षित आर्ग्युमेंट्सचे JSON Schema ऑब्जेक्ट

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

> **टूल वर्णनासाठी सर्वोत्तम पद्धती:**
> - विशिष्ट व्हा: "एका विशिष्ट शहरासाठी सद्य हवामान मिळवा" हे "हवामान मिळवा" पेक्षा चांगले आहे
> - पॅरामिटर्स स्पष्ट लिहा: मॉडेल या वर्णनांवरून योग्य मूल्ये भरते
> - आवश्यक आणि ऐच्छिक पॅरामिटर्स स्पष्ट करा — यामुळे मॉडेल काय विचारायचे ते ठरवते

---

### सराव 3 — टूल कॉलिंग उदाहरणे चालवा

प्रत्येक भाषेचा नमुना दोन टूल्स (`get_weather` आणि `get_population`) परिभाषित करतो, टूल वापरणारी एक प्रश्न पाठवतो, स्थानिकपणे टूल कार्यान्वित करतो आणि निकाल परत पाठवून अंतिम उत्तर मिळवतो.

<details>
<summary><strong>🐍 Python</strong></summary>

**पूर्वापेक्षा आवश्यक:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**चालवा:**
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

**कोड विश्लेषण** (`python/foundry-local-tool-calling.py`):

```python
# टूल्सना फंक्शन स्कीमांच्या यादीप्रमाणे परिभाषित करा
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

# टूल्ससह पाठवा — मॉडेल कंटेंटऐवजी tool_calls परत करू शकते
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# मॉडेलला टूल कॉल करायचा आहे का ते तपासा
if response.choices[0].message.tool_calls:
    # टूल कार्यान्वित करा आणि निकाल परत पाठवा
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**पूर्वापेक्षा आवश्यक:**
```bash
cd javascript
npm install
```

**चालवा:**
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

**कोड विश्लेषण** (`javascript/foundry-local-tool-calling.mjs`):

हे उदाहरण OpenAI SDK ऐवजी स्थानिक Foundry Local SDK ची `ChatClient` वापरले आहे, ज्यात `createChatClient()` या सोयीस्कर पद्धतीचा उपयोग दाखवला आहे:

```javascript
// मॉडेल ऑब्जेक्टमधून थेट ChatClient मिळवा
const chatClient = model.createChatClient();

// टूल्ससह पाठवा — ChatClient OpenAI-सुसंगत प्रारूप हाताळतो
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// टूल कॉल्ससाठी तपासा
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // टूल्स अंमलात आणा आणि निकाल परत पाठवा
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**पूर्वापेक्षा आवश्यक:**
```bash
cd csharp
dotnet restore
```

**चालवा:**
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

**कोड विश्लेषण** (`csharp/ToolCalling.cs`):

C# मध्ये `ChatTool.CreateFunctionTool` सहाय्यक वापरून टूल्स डिफाइन करतात:

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

### सराव 4 — टूल कॉलिंग संभाषण प्रवाह

संदेश संरचना समजून घेणे महत्वाचे आहे. येथे पूर्ण प्रवाह दाखवले आहे, प्रत्येक टप्प्यावर `messages` अ‍ॅरे दाखवले आहे:

**टप्पा 1 — प्रारंभिक विनंती:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**टप्पा 2 — मॉडेल `tool_calls` सह प्रतिसाद देते (सामग्री नाही):**
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

**टप्पा 3 — आपण सहाय्यक संदेश आणि टूल निकाल दोन्ही जोडता:**
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

**टप्पा 4 — मॉडेल टूल निकाल वापरून अंतिम उत्तर तयार करते.**

> **महत्त्वाचे:** टूल संदेशातील `tool_call_id` हा त्या टूल कॉलमधील `id` शी जुळला पाहिजे. यामुळे मॉडेल निकालाला विनंतीशी जोडू शकते.

---

### सराव 5 — अनेक टूल कॉल्स

एका प्रतिसादात अनेक टूल कॉल होऊ शकतात. वापरकर्ता संदेश बदलून अनेक कॉल्स चालू करा:

```python
# Python मध्ये — वापरकर्ता संदेश बदला:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// JavaScript मध्ये — वापरकर्त्याचा संदेश बदला:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

मॉडेलने दोन `tool_calls` परत कराव्यात — एक `get_weather` साठी आणि एक `get_population` साठी. आपला कोड आधीच सर्व टूल कॉल्सवर लूप करतो म्हणून याचा विचार केला आहे.

> **प्रयत्न करा:** वापरकर्ता संदेश बदला आणि पुन्हा नमुना चालवा. मॉडेल दोन्ही टूल्स कॉल करते का?

---

### सराव 6 — आपला स्वतःचा टूल जोडा

नमुना एकट्याला नवीन टूल जोडा. उदा., `get_time` टूल जोडा:

1. टूल स्कीमा डिफाइन करा:
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

2. कार्यान्वयन लॉजिक जोडा:
```python
# पाइथन
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # खऱ्या अ‍ॅपमध्ये, वेळ क्षेत्र लायब्ररी वापरा
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... विद्यमान साधने ...
```

3. `tools` अ‍ॅरेमध्ये टूल जोडा आणि चाचणी करा: "टोकियोमध्ये किती वाजले?"

> **चुनौती:** असा टूल जोडा जो गणना करतो, जसे की `convert_temperature`, जो सेल्सियस आणि फॅरेनहाइटमध्ये रूपांतरण करतो. "100°F सेल्सियस मध्ये रूपांतर करा" हे प्रश्न विचारून तपासा.

---

### सराव 7 — SDK च्या ChatClient सह टूल कॉलिंग (JavaScript)

JavaScript नमुन्यात आधीच SDK चा स्थानिक `ChatClient` वापरलेला आहे, OpenAI SDK वापरण्याऐवजी. ही एक सोय आहे ज्यामुळे OpenAI क्लायंट स्वतः तयार करायची गरज नाही:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient मॉडेल ऑब्जेक्टपासून थेट तयार केले जाते
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat दुसऱ्या पॅरामीटर म्हणून टूल्स स्वीकारतो
const response = await chatClient.completeChat(messages, tools);
```

Python पद्धतीशी तुलना करा ज्यात OpenAI SDK स्पष्टपणे वापरले आहे:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

दोन्ही पॅटर्न बरोबर आहेत. `ChatClient` अधिक सोयीस्कर आहे; OpenAI SDK तुम्हाला OpenAI च्या सर्व पर्यायांपर्यंत प्रवेश देते.

> **प्रयत्न करा:** JavaScript नमुन्यात `ChatClient` ऐवजी OpenAI SDK वापरण्यासाठी बदल करा. `import OpenAI from "openai"` वापरून आणि `manager.urls[0]` मधून एन्डपॉईंट मिळवून क्लायंट तयार करा.

---

### सराव 8 — tool_choice समजून घेणे

`tool_choice` पॅरामीटर नियंत्रित करतो की मॉडेल *टूल वापरलेच पाहिजे* किंवा स्वातंत्र्याने निवडू शकते:

| मूल्य | वर्तन |
|--------|----------|
| `"auto"` | मॉडेल ठरवते की टूल कॉल करायचा की नाही (डिफॉल्ट) |
| `"none"` | मॉडेल कोणतेही टूल कॉल करणार नाही जरी उपलब्ध असतील |
| `"required"` | मॉडेलने किमान एक टूल कॉल करणे आवश्यक |
| `{"type": "function", "function": {"name": "get_weather"}}` | मॉडेलने निर्दिष्ट टूल कॉल करायचा आहे |

Python नमुन्यात प्रत्येक पर्याय तपासा:

```python
# मॉडेलला get_weather कॉल करण्यास भाग पाडा
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **टीप:** प्रत्येक मॉडेल `tool_choice` चे सर्व पर्याय समर्थन करीत नाही. जर मॉडेल `"required"` समर्थन करत नसेल तर ते सेटिंग दुर्लक्ष करेल आणि `"auto"` प्रमाणे वागत जाईल.

---

## सामान्य चुकांपासून बचाव

| समस्या | उपाय |
|------------|---------|
| मॉडेल कधीच टूल कॉल करत नाही | खात्री करा की आपण टूल कॉलिंग मॉडेल (उदा. qwen2.5-0.5b) वापरत आहात. वरच्या तक्तीची तपासणी करा. |
| `tool_call_id` मध्ये विसंगती | नेहमी टूल कॉल प्रतिसादातील `id` वापरा, हार्डकोडेड मूल्य नाही |
| मॉडेल `arguments` मध्ये चुकीचा JSON परत करते | लहान मॉडेल कधी कधी अवैध JSON तयार करतात. `JSON.parse()` ला try/catch मध्ये ठेवा |
| मॉडेल असलेला टूल कॉल करते जो अस्तित्वात नाही | आपला `execute_tool` फंक्शनमध्ये डिफॉल्ट हँडलर जोडा |
| अनंत टूल कॉलिंग लूप | जास्तीत जास्त राउंड वर सेट करा (उदा. 5) जेणेकरून लूपमध्ये अडकलं जाणार नाही |

---

## महत्त्वाचे मुद्दे

1. **टूल कॉलिंग** मॉडेल्सना उत्तराचा अंदाज लावण्याऐवजी फंक्शन कार्यान्वित करण्याची विनंती करण्याची परवानगी देते
2. मॉडेल कधीही कोड चालवत नाही; आपला अनुप्रयोग काय चालवायचे ठरवतो
3. टूल्स OpenAI फंक्शन कॉलिंग फॉरमॅटनुसार **JSON Schema** ऑब्जेक्ट्स म्हणून डिफाइन केले जातात
4. संभाषणात **मल्टि-टर्न पॅटर्न** वापरला जातो: वापरकर्ता → सहाय्यक (`tool_calls`) → टूल (निकाल) → सहाय्यक (अंतिम उत्तर)
5. नेहमी टूल कॉलिंग सपोर्ट करणारे मॉडेल वापरा (Qwen 2.5, Phi-4-mini)
6. SDK चे `createChatClient()` OpenAI क्लायंट निर्माण न करता टूल कॉलिंग विनंत्यांसाठी सोईस्कर मार्ग देते

---

पुढे जा [भाग 12: Zava Creative Writer साठी वेब UI तयार करणे](part12-zava-ui.md) मध्ये, जेणेकरून बहु-एजंट पाईपलाइनसाठी रिअल-टाइम स्ट्रीमिंगसह ब्राउझर-आधारित फ्रंटएंड जोडता येईल.

---

[← भाग 10: सानुकूल मॉडेल्स](part10-custom-models.md) | [भाग 12: Zava Writer UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:
हा दस्तऐवज AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) वापरून अनुवादित करण्यात आला आहे. आम्ही अचूकतेसाठी प्रयत्नशील आहोत, तरी कृपया लक्षात ठेवा की स्वयंचलित अनुवादांमध्ये चुका किंवा अचूकतेत कमतरता असू शकते. मूळ दस्तऐवज त्याच्या मूळ भाषेत अधिकृत स्रोत मानला पाहिजे. महत्त्वपूर्ण माहितीसाठी व्यावसायिक मानवी अनुवाद स्वीकारण्याची शिफारस केली जाते. या अनुवादाच्या वापरामुळे उद्भवणाऱ्या कोणत्याही गैरसमजुती किंवा चुकीच्या समजुतींसाठी आम्ही जबाबदार नाहीत.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->