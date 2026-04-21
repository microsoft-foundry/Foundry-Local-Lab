![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# பகுதி 11: உள்ளூரான மாதிரிகளுடன் கருவி அழைக்கும்

> **நோக்கம்:** உங்கள் உள்ளூர் மாதிரி வெளிப்புற செயல்பாடுகளை (கருவிகள்) அழைக்க இயலுமாறு செயல் படுத்துவது, இதன் மூலம் அது நேரடி தரவைப் பெற, கணக்கீடுகளை செய்ய, அல்லது APIகளுடன் தொடர்பு கொள்ள முடியும் — எல்லாம் உங்கள் சாதனத்தில் தனிப்பட்ட முறையில் ஓடுகிறது.

## கருவி அழைத்தல் என்றால் என்ன?

கருவி அழைத்தல் (மற்றொரு பெயராக **செயல்பாட்டை அழைத்தல்**) என்பது ஒரு மொழி மாதிரி நீங்கள் வரையறுத்த செயல்பாடுகளை இயக்கக் கோரிக்கையிடும் வசதி. பதிலை ஊகிக்கின்ற பதிலுக்கு பதிலாக, மாதிரி ஒரு கருவி உதவும் போது அதை உணர்ந்து உங்கள் குறியீட்டை இயக்க ஒரு கட்டமைக்கப்பட்ட கோரிக்கையை வழங்குகிறது. உங்கள் பயன்பாடு அந்த செயல்பாட்டை இயக்கி, முடிவை திருப்பி அனுப்பி, மாதிரி அந்த தகவலை இறுதி பதிலில் ஒருங்கிணைக்கிறது.

![கருவி-அழைக்கும் ஓட்டம்](../../../images/tool-calling-flow.svg)

இந்த மாதிரி பொறியாளர்கள் கீழ்காணும் செயல்களை செய்ய முக்கியமாகிறது:

- **நேரடி தரவை தேடுதல்** (காலநிலை, பங்கு விலை, தரவுத்தள வினவல்கள்)
- **துல்லியமான கணக்கீடுகளைச் செய்வது** (கணிதம், அளவுரு மாற்றங்கள்)
- **செயல்கள் மேற்கொள்வது** (மின்னஞ்சல்கள் அனுப்புதல், டிக்கெட்டுகள் உருவாக்குதல், பதிவுகளை புதுப்பித்தல்)
- **தனிப்பட்ட அமைப்புகளுக்கு அணுகுதல்** (உள் APIகள், கோப்பு அமைப்புகள்)

---

## கருவி அழைத்தல் எப்படி வேலை செய்கிறது

கருவி அழைத்தல் ஓட்டம் நான்கு கட்டங்களைக் கொண்டுள்ளது:

| கட்டம் | என்ன நடக்கிறது |
|-------|---------------|
| **1. கருவிகளை வரையறு** | நீங்கள் JSON ஸ்கீமாவைப் பயன்படுத்தி கிடைக்கும் செயல்பாடுகளை விவரிக்கின்றீர்கள் — பெயர், விளக்கம் மற்றும் அளவுருக்கள் |
| **2. மாதிரி முடிவு செய்கிறது** | உங்கள் செய்தியையும் கருவி வரையறைகளையும் மாதிரி பெறுகிறது. ஒரு கருவி உதவும் என்றால், அது எழுத்து பதிலுக்கு பதிலாக `tool_calls` பதிலை வழங்குகிறது |
| **3. உள்ளூர் இயக்கு** | உங்கள் குறியீடு கருவி அழைப்பை பகுப்பாய்வு செய்து, செயல்பாட்டை இயக்கி, முடிவை சேகரிக்கிறது |
| **4. இறுதி பதில்** | நீங்கள் கருவி முடிவை மாதிரிக்கு அனுப்பி, அது இறுதி பதிலை உருவாக்குகிறது |

> **முக்கிய குறிப்பு:** மாதிரி ஒருபோதும் குறியீட்டை இயக்காது. அது ஒரு கருவி அழைக்கப்பட வேண்டும் என்று மட்டும் *கோரிக்கிறது*. உங்கள் பயன்பாடு அந்த கோரிக்கையை மதிப்பிட்டு செயல் படுத்துகிறது — இது முழு கட்டுப்பாட்டை உங்களுக்கு வழங்குகிறது.

---

## எந்த மாதிரிகள் கருவி அழைப்பை ஆதரிக்கின்றன?

ஒவ்வொரு மாதிரியும் கருவி அழைப்பை ஆதரிக்காது. தற்போதைய Foundry Local பட்டியலில், கீழ்காணும் மாதிரிகள் கருவி-அழைப்பை ஆதரிக்கின்றன:

| மாதிரி | அளவு | கருவி அழைக்கும் திறன் |
|-------|------|:---------------------:|
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

> **குறிப்பு:** இந்த பயிற்சிக்காக நாம் **qwen2.5-0.5b** பயன்படுத்துகிறோம் — அது சிறியது (822 MB பதிவிறக்கம்), வேகமானது, நம்பகமான கருவி அழைக்கும் ஆதரவு உள்ளது.

---

## கற்கல் இலக்குகள்

இந்த பயிற்சியின் முடிவில் நீங்கள்:

- கருவி அழைக்கும் மாதிரியை விளக்க முடியும் மற்றும் ஏன் இது AI முகவர்களுக்குப் பயன்படும்
- OpenAI செயல் அழைக்கும் முறையைப் பயன்படுத்தி கருவி ஸ்கீமைகளை வரையறுக்க முடியும்
- பல சுற்றுகளாக நடக்கும் கருவி அழைக்கும் உரையாடல் ஓட்டத்தை கையாள முடியும்
- உள்ளூர் கருவி அழைப்புகளை இயக்கி முடிவுகளை மாதிரிக்கு திருப்பி அனுப்ப முடியும்
- கருவி அழைக்கும் சூழल்களுக்கு சரியான மாதிரியை தேர்வு செய்ய முடியும்

---

## முன் தேவைகள்

| தேவைகள் | விவரங்கள் |
|----------|-----------|
| **Foundry Local CLI** | நிறுவப்பட்டு PATH இல் இருக்க வேண்டும் ([பகுதி 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript அல்லது C# SDK நிறுவப்பட்டு ([பகுதி 2](part2-foundry-local-sdk.md)) |
| **ஒரு கருவி-அழைக்கும் மாதிரி** | qwen2.5-0.5b (தானாக பதிவிறக்கம் செய்யப்படும்) |

---

## பயிற்சிகள்

### பயிற்சி 1 — கருவி-அழைக்கும் ஓட்டத்தை புரிந்து கொள்

குறியீடு எழுதுவதற்கு முன், இந்த தொடர் விளக்கப்படத்தைப் படியுங்கள்:

![கருவி-அழைக்கும் தொடர் விளக்கம்](../../../images/tool-calling-sequence.svg)

**முக்கிய கவனத்துக்குள்ளாக:**

1. நீங்கள் கருவிகளை முன் காண JSON ஸ்கீமா பொருட்களாக வரையறுக்கின்றீர்கள்
2. மாதிரி பதில் `tool_calls` கொண்டது, சாதாரண உள்ளடக்கத்திற்கு பதிலாக
3. ஒவ்வொரு கருவி அழைப்புக்கும் தனித்துவமான `id` உண்டு, முடிவை திருப்பி அனுப்பும்போது அதைப் பயன்படுத்தவேண்டும்
4. மாதிரி அனைத்து முந்தைய செய்திகள் *மற்றும்* கருவி முடிவுகளைக் காண்கிறது இறுதி பதிலை உருவாக்கும் போது
5. பல கருவி அழைப்புகள் ஒரு பதிலில் நிகழலாம்

> **காணோசனை:** மாதிரி நேரடியான செயல்பாடுகளை இயக்காமல் கருவி அழைப்புகளை மட்டுமே திருப்பி வழங்குவது ஏன்? இதன் பாதுகாப்பு நன்மைகள் என்ன?

---

### பயிற்சி 2 — கருவி ஸ்கீமைகளை வரையறு

கருவிகள் OpenAI செயல்-அழைக்கும் தரவுத்தரமான முறையில் வரையறுக்கப்படுகின்றன. ஒவ்வொரு கருவிக்கும் தேவையானவை:

- **`type`**: எப்பொழுதும் `"function"`
- **`function.name`**: ஒரு விளக்கமான செயல்பாட்டு பெயர் (எ.கா. `get_weather`)
- **`function.description`**: தெளிவான விளக்கம் — மாதிரி எப்போது கருவியை அழைக்கவேண்டும் என்பதை முடிவு செய்கிறது
- **`function.parameters`**: JSON ஸ்கீமா பொருள் எதிர்பார்க்கப்படும் அளவுருக்களை விவரிக்கும்

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

> **கருவி விளக்கங்களுக்கு சிறந்த நடைமுறைகள்:**
> - தெளிவாக இருக்கவும்: "குறித்த நகரத்தின் தற்போதைய காலநிலையை பெறுதல்" என்பது "காலநிலையை பெறுதல்" காட்டிலும் சிறந்தது
> - அளவுருக்களை தெளிவாக விவரிக்கவும்: மாதிரி இதைப் படித்து சரியான மதிப்புகளை நிரப்புகிறது
> - தேவையான மற்றும் விரும்பிய அளவுருக்களை குறியுங்கள் — இதனால் மாதிரி எனக்குத் கேட்கவேண்டியதை அறிந்துகொள்கிறது

---

### பயிற்சி 3 — கருவி-அழைப்புச் சுட்டிகள் இயக்குக

ஒவ்வொரு மொழி மாதிரியும் இரண்டு கருவிகளை (`get_weather` மற்றும் `get_population`) வரையறுக்கிறது, ஒரு கேள்வி அனுப்பி கருவி பயன்படுத்தப்படுகிறது, கருவி உள்ளூர் இயக்கப்படு, மற்றும் முடிவு இறுதி பதிலுக்காக திருப்பி அனுப்பப்படுகிறது.

<details>
<summary><strong>🐍 Python</strong></summary>

**முன் தேவைகள்:**
```bash
cd python
python -m venv venv

# விண்டோஸ் (பவர்‌ஷெல்):
venv\Scripts\Activate.ps1
# மேக்ஓஎஸ் / லினக்ஸ்:
source venv/bin/activate

pip install -r requirements.txt
```

**இயக்கு:**
```bash
python foundry-local-tool-calling.py
```

**எதிர்பார்க்கப்படும் வெளியீடு:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**குறியீடு விளக்கம்** (`python/foundry-local-tool-calling.py`):

```python
# கருவிகளை செயல்பாட்டு உருவரடைவுகளின் பட்டியலாக வரையறு
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

# கருவிகளுடன் அனுப்பு — மாதிரி உள்ளடக்கத்தின் மாற்றாக tool_calls ஐத் திருப்பலாம்
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# மாதிரி கருவியை அழைக்க விரும்புகிறதா என்று சரிபார்
if response.choices[0].message.tool_calls:
    # கருவியை செயல்படுத்தி முடிவை திருப்பி அனுப்பு
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**முன் தேவைகள்:**
```bash
cd javascript
npm install
```

**இயக்கு:**
```bash
node foundry-local-tool-calling.mjs
```

**எதிர்பார்க்கப்படும் வெளியீடு:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**குறியீடு விளக்கம்** (`javascript/foundry-local-tool-calling.mjs`):

இந்த எடுத்துக்காட்டு OpenAI SDKஐ விட உருவாக்கப்பட்ட Foundry Local SDKயின் கிடைக்கத்தக்க `ChatClient`-ஐப் பயன்படுத்துகிறது, இது `createChatClient()` மூலம் எளிதாய்ச் செயற்கிறது:

```javascript
// மாடல் பொருளிலிருந்து நேரடியாக ChatClient ஐப் பெறுங்கள்
const chatClient = model.createChatClient();

// உபகரணங்களுடன் அனுப்பவும் — ChatClient OpenAI-உருவாக்கத்தை வழங்குகிறது
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// உபகரண அழைப்புகளை சரிபார்க்கவும்
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // உபகரணங்களை இயக்கி முடிவுகளை திரும்ப அனுப்பவும்
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**முன் தேவைகள்:**
```bash
cd csharp
dotnet restore
```

**இயக்கு:**
```bash
dotnet run toolcall
```

**எதிர்பார்க்கப்படும் வெளியீடு:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**குறியீடு விளக்கம்** (`csharp/ToolCalling.cs`):

C# `ChatTool.CreateFunctionTool` உதவியுடன் கருவிகளை வரையறுக்கிறது:

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

### பயிற்சி 4 — கருவி-அழைக்கும் உரையாடல் ஓட்டம்

செய்தி அமைப்பை புரிந்துகொள்வது அவசியம். இதோ ஒவ்வொரு கட்டத்திலும் `messages` வரிசை முழுமையாக:

**கட்டம் 1 — ஆரம்ப கோரிக்கை:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**கட்டம் 2 — மாதிரி `tool_calls` (உள்ளடக்கம் அல்ல) உடன் பதிலளிக்கிறது:**
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

**கட்டம் 3 — நீங்கள் உதவியாளர் செய்தியும் கருவி முடிவும் சேர்க்கின்றீர்கள்:**
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

**கட்டம் 4 — மாதிரி கருவி முடிவைப் பயன்படுத்தி இறுதி பதிலை உருவாக்கும்.**

> **முக்கியம்:** கருவி செய்தியில் `tool_call_id` என்பது கருவி அழைத்தல் `id` மையப்படுத்தப்பட வேண்டும். இதுவே மாதிரிக்கு முடிவுகளை கோரிக்கைகளுடன் இணைக்க உதவும்.

---

### பயிற்சி 5 — பல கருவி அழைப்புகள்

ஒரே பதிலில் பல கருவி அழைப்புகள் கோரப்படலாம். பயனர் செய்தியை மாற்றி பல அழைப்புகளை நேர்த்துக் கொள்ள முயற்சி செய்க:

```python
# Python இல் — பயனர் செய்தியை மாற்றவும்:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// ஜாவாஸ்கிரிப்ட் இல் — பயனர் செய்தியை மாற்றவும்:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

மாதிரி இரண்டு `tool_calls`-ஐ நேர்த்துக் காட்சி செய்ய வேண்டும் — ஒன்று `get_weather`க்கு மற்றும் ஒன்று `get_population`க்கு. உங்கள் குறியீடு ஏற்கனவே அனைத்து கருவி அழைப்புகளையும் இடைமுகமாக சுற்றி கையாள்கிறது.

> **முயற்சி செய்க:** பயனர் செய்தியை மாற்றி மீண்டும் மாதிரியை இயக்கவும். இரு கருவிகளும் அழைக்கப்படுகிறதா?

---

### பயிற்சி 6 — உங்கள் சொந்த கருவியைச் சேர்க்கவும்

ஒன்று மாதிரிகளில் புதிய கருவி சேர்க்கவும். உதாரணமாக, `get_time` கருவியைச் சேர்க்கலாம்:

1. கருவி ஸ்கீமாவை வரையறு:
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

2. செயல் நிரலாக்கத்தைச் சேர்க்கவும்:
```python
# பைதான்
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # ஒரு உண்மையான செயலியில், நேர மண்டலம் நூலகத்தைப் பயன்படுத்தவும்
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... உள்ளமைந்த கருவிகள் ...
```

3. கருவியை `tools` வரிசையில் சேர்த்து "டோக்கியோவில் நேரம் என்ன?" என சோதனை நடத்தவும்

> **சவால்:** ஒரு கணக்கிடும் கருவி சேர்க்கவும், உதாரணமாக `convert_temperature` என்பது செல்சியசும் பாரன்ஹீட்டும் மாற்றும். இதனை "100°F-ஐ செல்சியசுக்கு மாற்று" என்று சோதித்துப் பாருங்கள்.

---

### பயிற்சி 7 — SDKயின் ChatClient கொண்டு கருவி அழைக்கும் (JavaScript)

JavaScript எடுத்துக்காட்டு OpenAI SDKஐ விட SDKயின் உள்ளூர் பெயரிலுள்ள `ChatClient`-ஐக் பயன்படுத்துகிறது. இது OpenAI கிளையண்டைத் தானாக உருவாக்க தேவையில்லாமல் வசதியாக்கிறது:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient மாதிரி பொருளிலிருந்து நேரடியாக உருவாக்கப்படுகிறது
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat இரண்டாவது அளவுகோலாக கருவிகளை ஏற்றுக்கொள்கிறது
const response = await chatClient.completeChat(messages, tools);
```

இதனை Python வழியுடன் ஒப்பிடுக, Python OpenAI SDKயை நேரடியாகப் பயன்படுத்துகிறது:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

இரு முறையும் செல்லுபடியாகும். `ChatClient` வசதியானது; OpenAI SDK முழு OpenAI அளவுருக்களை வழங்குகிறது.

> **முயற்சி செய்க:** JavaScript எடுத்துக்காட்டை `ChatClient` பதிலாக OpenAI SDKயைப் பயன்படுத்த மாற்றவும். `import OpenAI from "openai"` மற்றும் `manager.urls[0]` இலிருந்து முடிவிடத்தைப் பயன்படுத்தி கிளையண்டை உருவாக்க வேண்டும்.

---

### பயிற்சி 8 — tool_choice என்பதைப் புரிந்துகொள்

`tool_choice` அளவுரு மாதிரி கருவியை *பணியில் கடைசியாக* பயன்படுத்த வேண்டுமா இல்லையா என்பதை கட்டுப்படுத்துகிறது:

| மதிப்பு | நடத்தை |
|---------|----------|
| `"auto"` | மாதிரி கருவி அழைக்க வேண்டும் என்று தீர்மானிக்கிறது (இயல்புநிலை) |
| `"none"` | மாதிரி கருவி அழைக்காது, இருந்தாலும் வழங்கப்பட்டாலும் |
| `"required"` | மாதிரிக்கு குறைந்த பட்சம் ஒரு கருவி அழைக்க வேண்டும் |
| `{"type": "function", "function": {"name": "get_weather"}}` | மாதிரிக்கு குறிப்பிட்ட கருவி அழைக்கவேண்டும் |

Python எடுத்துக்காட்டில் ஒவ்வொன்றையும் முயற்சிக்கவும்:

```python
# மாதிரியை get_weather ஐப் அழைக்க வலியுறுத்து
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **குறிப்பு:** எல்லா மாதிரிகளும் `"required"` போன்ற `tool_choice` விருப்பங்களை ஆதரிக்கmayாது. ஆதரிக்காதால், வடிவமைப்பை புறக்கணித்து `"auto"` போல நடந்து கொள்கிறது.

---

## பொதுவான சிக்கல்கள்

| பிரச்சனை | தீர்வு |
|---------|----------|
| மாதிரி ஒருபோதும் கருவிகளை அழைக்காது | நீங்கள் கருவி-அழைக்கும் மாதிரியை பயன்படுத்துகிறீர்களா என சரிபார்க்கவும் (எ.கா. qwen2.5-0.5b). மேலே உள்ள அட்டவணையைப் பார்த்து ஒப்பிடவும். |
| `tool_call_id` பொருந்தவில்லை | கருவி அழைப்பின் பதிலில் வழங்கப்பட்ட `id` ஐ மட்டுமே பயன்படுத்தவும், கடைசியில் நிரப்பப்பட்ட மதிப்பை பயன்படுத்த வேண்டாம் |
| மாதிரி `arguments` இல் தவறான JSON வழங்குகிறது | சிறிய மாதிரிகள் சில சமயங்களில் தவறான JSON அளிக்கலாம். `JSON.parse()` ஐ try/catch மூலம் சுற்றி பாதுகாக்கவும் |
| மாதிரி இல்லாத கருவியை அழைக்கிறது | உங்கள் `execute_tool` செயல்பாட்டில் டீஃபால்ட் கைமுறைல்முறை சேர்க்கவும் |
| முடிவற்ற கருவி-அழைக்கும் சுற்று | அதிகபட்ச சுற்றுக்களின் எண்ணிக்கையை (எ.கா. 5) அமைக்க, கட்டுப்பாட்டு லூபுகளைத் தடுக்கும் |

---

## முக்கிய எடுத்துக்காட்டுகள்

1. **கருவி அழைத்தல்** மாதிரிகள் பதில்களை ஊகிக்கின்றதற்குப் பதிலாக செயல்பாடுகளை இயக்கக் கோர உதவுகிறது
2. மாதிரி **ஒருபோதும் குறியீட்டை இயக்காது**; நீங்கள் என்ன இயக்கவேண்டும் என்பதை தீர்மானிக்கின்றீர்கள்
3. கருவிகள் **JSON ஸ்கீமா** பொருட்களாகவும் OpenAI செயல்பாட்டு அழைக்கும் வடிவத்தில் வரையறுக்கப்படுகின்றன
4. உரையாடல் **பல சுற்றுகளைக் கொண்ட மாதிரியை** பின்பற்றுகிறது: பயனர் → உதவியாளர் (tool_calls) → கருவி (முடிவுகள்) → உதவியாளர் (இறுதி பதில்)
5. எப்போதும் **கருவி அழைக்கும் திறன் கொண்ட மாதிரிகளை** பயன்படுத்தவும் (Qwen 2.5, Phi-4-mini)
6. SDKயின் `createChatClient()` OpenAI கிளையண்டை உருவாக்காமலும் கருவி அழைப்புகளை எளிதாகச் செய்ய உதவுகிறது

---

[பகுதி 12: Zava Creative Writer க்கான வலை UI உருவாக்குதல்](part12-zava-ui.md) க்கு தொடரவும்; இது பல முகவர்களுக்கான குழாய் பாதுகாப்பான நேரடி ஸ்ட்ரீமிங்குடன் உலாவி சார்ந்த முன் கடை சேர்க்கும்.

---

[← பகுதி 10: தனிப்பயன் மாதிரிகள்](part10-custom-models.md) | [பகுதி 12: Zava எழுத்தாளர் UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**முறையீடு**:  
இந்த ஆவணம் AI மொழிபெயர்ப்பு சேவை [Co-op Translator](https://github.com/Azure/co-op-translator) பயன்படுத்தி மொழிமாற்றம் செய்யப்பட்டு உள்ளது. நாங்கள் துல்லியத்திற்காக முயற்சித்தாலும், தானியங்கிய மொழிபெயர்ப்புகளில் பிழைகள் அல்லது தவறுகள் இருக்க வாய்ப்பு உண்டு என்பதை கவனிக்கவும். யாராவது முக்கியமான தகவலுக்கு, தொழில்முறை மனித மொழிபெயர்ப்பு பரிந்துரைக்கப்படுகிறது. இந்த மொழிபெயர்ப்பின் பயன்படுத்தலால் ஏற்படும் எந்த விளக்கவியல் தவறுகள் அல்லது தவறான உணர்வுகளுக்கு நாங்கள் பொறுப்பில்லை.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->