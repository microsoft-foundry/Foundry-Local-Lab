![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# భాగం 11: స్థానిక మోడల్స్‌తో టూల్ కాలింగ్

> **లక్ష్యం:** మీ స్థానిక మోడల్‌ను బయట ఫంక్షన్లు (టూల్స్) పిలవడానికి సక్రమం చేయండి, తద్వారా అది రియల్ టైం డేటాను తీసుకోగలదు, లెక్కింపులు చేయగలదు లేదా APIsతో ఇంటరాక్ట్ చేయగలదు — ఇవన్నీ మీ పరికరంలో ప్రైవేటుగా నడుస్తాయి.

## టూల్ కాలింగ్ అంటే ఏమిటి?

టూల్ కాలింగ్ (**function calling** అని కూడా పిలువబడుతుంది) ఒక భాషా మోడల్ మీరే నిర్వచించిన functionల అమలు అభ్యర్థన చేయడానికి అనుమతిస్తుంది. ఒక సమాధానాన్ని అంచనా వేయుట బదులు, మోడల్ ఎప్పుడు ఒక టూల్ ఉపయోగపడుతుందో గుర్తించి, మీ కోడ్ అమలు చేయడానికి రూపొందించిన అభ్యర్థనను తిరిగి ఇస్తుంది. మీ అప్లికేషన్ ఆ function ను నడిపిస్తుంది, ఫలితాన్ని తిరిగి పంపుతుంది, మరియు మోడల్ ఆ సమాచారాన్ని చివరి స్పందనలో జోడిస్తుంది.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

ఈ నమూనా ఏజెంట్స్ నిర్మాణంలో చాలా ముఖ్యం అవుతుంది, వీటిని మీరు సృష్టించవచ్చు:

- **లైవ్ డేటాను చూడడం** (వాతావరణం, స్టాక్ ధరలు, డేటాబేస్ క్వెరీలు)
- **స్పష్టమైన లెక్కింపులు చేయడం** (గణితం, యూనిట్ మార్పిడులు)
- **చర్యలు తీసుకోవడం** (ఇమెయిల్స్ పంపడం, టికెట్లు సృష్టించడం, రికార్డ్స్ ని నవీకరించడం)
- **ప్రైవేట్ సిస్టమ్స్ యాక్సెస్ చేయడం** (అంతర్గత APIs, ఫైల్ సిస్టమ్స్)

---

## టూల్ కాలింగ్ ఎలా పనిచేస్తుంది

టూల్ కాలింగ్ ఫ్లో నాలుగు దశలుంటాయి:

| దశ | ఏమి జరుగుతుంది |
|-------|-------------|
| **1. టూల్స్ నిర్వచించండి** | JSON Schema ఉపయోగించి అందుబాటులో ఉన్న ఫంక్షన్లను వివరిస్తారు — పేరు, వివరణ, మరియు పరామితులు |
| **2. మోడల్ నిర్ణయిస్తది** | మోడల్ మీ సందేశం మరియు టూల్ నిర్వచనలు అందుకుంటుంది. ఒక టూల్ అవసరమైతే, ఇది టెక్ట్స్ సమాధానానికి బదులుగా `tool_calls` ప్రత్యుత్తరాన్ని ఇస్తుంది |
| **3. స్థానికంగా అమలు చేయండి** | మీ కోడ్ టూల్ కాల్ ని పార్స్ చేసి, ఫంక్షన్ నడిపించి, ఫలితాన్ని సేకరిస్తుంది |
| **4. చివరి సమాధానం** | మీరు టూల్ ఫలితాన్ని మోడల్ కు పంపుతారు, మోడల్ చివరి సమాధానాన్ని ఇస్తుంది |

> **ప్రధాన విషయం:** మోడల్ ఎప్పుడూ కోడ్ ను నడపదు. ఇది కేవలం టూల్ పిలవాల్సిన అభ్యర్థనను మాత్రమే చేస్తుంది. ఆ అభ్యర్థనను ఫాలో చేయడం లేదా చేయకపోవడం మీ అప్లికేషన్ నిర్ణయించుకుంటుంది — ఇది మీకు పూర్తి నియంత్రణ ఇస్తుంది.

---

## ఏ మోడల్స్ టూల్ కాలింగ్‌ని సపోర్ట్ చేస్తాయి?

ప్రతి మోడల్ టూల్ కాలింగ్‌ని సపोर्ट్ చేయదు. ప్రస్తుత Foundry Local క్యాటలాగులో, క్రింద ఉన్న మోడల్స్ టూల్-కెలింగ్ సమర్థత కలిగి ఉన్నాయి:

| మోడల్ | పరిమాణం | టూల్ కాలింగ్ |
|-------|---------|:------------:|
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

> **సూచన:** ఈ లాబ్ కోసం మేము **qwen2.5-0.5b** ఉపయోగిస్తాం — ఇది చిన్నది (822 MB డౌన్లోడ్), వేగవంతంగా ఉంటుంది, మరియు విశ్వసనీయమైన టూల్-కాలింగ్ మద్దతు కలిగి ఉంది.

---

## నేర్చుకోవాల్సిన లక్ష్యాలు

ఈ లాబ్ ముగింపు వరకు మీరు చేయగలరు:

- టూల్-కాలింగ్ నమూనాను వివరిద్దాం మరియు అది AI ఏజెంట్స్ కోసం ఎందుకు అవసరం వాస్తవం తెలుసుకోండి
- OpenAI ఫంక్షన్-కాలింగ్ ఫార్మాట్ ఉపయోగించి టూల్ స్కీమా‌లను నిర్వచించండి
- బహుళ-తిరుగుడు టూల్-కాలింగ్ సంభాషణ ఫ్లోను నిర్వహించండి
- స్థానికంగా టూల్ కాల్స్ నడిపించి ఫలితాలను మోడల్ కు పంపండి
- టూల్-కాలింగ్ సందర్భాలకు సరైన మోడల్ ఎంచుకోండి

---

## తప్పనిసరి జాగ్రత్తలు

| అవసరం | వివరాలు |
|-------------|---------|
| **Foundry Local CLI** | ఇనిస్టాల్ గా `PATH` లో ఉండాలి ([Part 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript, లేదా C# SDK ఇనిస్టాల్ ([Part 2](part2-foundry-local-sdk.md)) |
| **ఒక టూల్-కాలింగ్ మోడల్** | qwen2.5-0.5b (స్వయంచాలకంగా డౌన్లోడ్ అవుతుంది) |

---

## వ్యాయామాలు

### వ్యాయామం 1 — టూల్-కాలింగ్ ఫ్లో అర్థం చేసుకోండి

కోడ్ రాయక ముందుగా ఈ సీక్వెన్స్ డయాగ్రామ్ పరిశీలించండి:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**ప్రధాన గమనికలు:**

1. మీరు ప్రారంభంలో టూల్స్‌ను JSON Schema ఆబ్జెక్టులుగా నిర్వచిస్తారు
2. మోడల్ ప్రతిస్పందనలో సాదారణ కంటెంట్ బదులు `tool_calls` ఉంటుంది
3. ప్రతి టూల్ కాల్‌కు ఒక ప్రత్యేక `id` ఉంటుంది, మీరు ఫలితాలు తిరిగి పంపేటప్పుడు దీన్ని ఉపయోగించాలి
4. మోడల్ చివరి సమాధానం రూపొందించే సమయంలో, అన్ని పూర్వ సందేశాలను *తో పాటు* టూల్ ఫలితాలను కూడా చూస్తుంది
5. ఒకే తిలకంలో బహుళ టూల్ కాల్స్ చోటు చేసుకోగలవు

> **చర్చ:** మోడల్ నేరుగా ఫంక్షన్లు అమలు చేయడం బదులు టూల్ కాల్స్ ఎందుకు ఇస్తుంది? దీనివల్ల ఎలాంటి భద్రత లాభాలు ఉంటాయి?

---

### వ్యాయామం 2 — టూల్ స్కీమాలు నిర్వచించడం

టూల్స్‌ను OpenAI ఫంక్షన్-కాలింగ్ ఫార్మాట్ ఉపయోగించి నిర్వచిస్తారు. ప్రతి టూల్‌కు కావాల్సినవి:

- **`type`**: ఎల్లప్పుడూ `"function"`
- **`function.name`**: వివరణాత్మకమైన ఫంక్షన్ పేరు (ఉదా. `get_weather`)
- **`function.description`**: స్పష్టమైన వివరణ — మోడల్ టూల్ పిలవడానికిగానీ ఫలితంగా దీన్ని ఉపయోగిస్తుంది
- **`function.parameters`**: అంచనా వేయబడిన argument‌లను JSON Schemaగా వివరించే ఆబ్జెక్ట్

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

> **టూల్ వివరణల కోసం ఉత్తమ ఆచరణలు:**
> - ప్రత్యేకంగా ఉండండి: "నిర్దిష్ట నగరానికి ప్రస్తుత వాతావరణాన్ని పొందండి" అనేది "వాతావరణం పొందండి" కంటే మంచిది
> - పారామితులను స్పష్టంగా వివరించండి: మోడల్ ఈ వివరాలను చదవడం ద్వారా సరైన విలువలను పూరించగలదు
> - అవసరం మరియు ఐచ్ఛిక పారామితులను గుర్తించండి — ఇది మోడల్ అడగాల్సిన విషయాలను నిర్ణయించడంలో సహాయపడుతుంది

---

### వ్యాయామం 3 — టూల్-కాలింగ్ ఉదాహరణలను నడపండి

ప్రతి భాషా నమూనా రెండు టూల్స్ (`get_weather` మరియు `get_population`) ను నిర్వచిస్తుంది, టూల్ ఉపయోగానికి ప్రేరేపించే ప్రశ్నలను పంపుతుంది, స్థానికంగా టూల్ అమలు చేస్తుంది, మరియు ఫలితాన్ని తిరిగిఅవి పంపించి చివరి సమాధానాన్ని పొందుతుంది.

<details>
<summary><strong>🐍 Python</strong></summary>

**తయారీ:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**నడపండి:**
```bash
python foundry-local-tool-calling.py
```

**నిరీక్షిత అవుట్‌పుట్:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**కోడ్ వివరణ** (`python/foundry-local-tool-calling.py`):

```python
# టూల్స్‌ని ఫంక్షన్ స్కీమాలు జాబితాగా నిర్వచించండి
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

# టూల్స్‌తో పంపండి — కంటెంట్‌కు బదులు మోడల్ tool_calls ని తిరిగి అందించవచ్చు
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# మోడల్ టూల్‌ని పిలవాలని కోరుకుంటుందో yoxగించండి
if response.choices[0].message.tool_calls:
    # టూల్‌ని అమలు చేసి ఫలితాన్ని తిరిగి పంపండి
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**తయారీ:**
```bash
cd javascript
npm install
```

**నడపండి:**
```bash
node foundry-local-tool-calling.mjs
```

**నిరీక్షిత అవుట్‌పుట్:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**కోడ్ వివరణ** (`javascript/foundry-local-tool-calling.mjs`):

ఈ ఉదాహరణ OpenAI SDK బదులు స్థానిక Foundry Local SDK లో `ChatClient` ఉపయోగిస్తుంది, ఇది `createChatClient()` పద్ధతితో సౌలభ్యం చూపిస్తుంది:

```javascript
// మోడల్ ఆబ్జెక్ట్ నుండి నేరుగా ChatClient ను పొందండి
const chatClient = model.createChatClient();

// వస్తువులతో పంపండి — ChatClient OpenAI-అనుకూల ఫార్మాట్‌ను నిర్వహిస్తుంది
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// టూల్ కాల్స్ కోసం తనిఖీ చేయండి
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // టూల్స్‌ను అమలు చేసి ఫలితాలను తిరిగి పంపండి
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**తయారీ:**
```bash
cd csharp
dotnet restore
```

**నడపండి:**
```bash
dotnet run toolcall
```

**నిరీక్షిత అవుట్‌పుట్:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**కోడ్ వివరణ** (`csharp/ToolCalling.cs`):

C# లో `ChatTool.CreateFunctionTool` సహాయకాన్ని ఉపయోగించి టూల్స్ నిర్వచిస్తారు:

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

### వ్యాయామం 4 — టూల్-కాలింగ్ సంభాషణ ఫ్లో

సందేశ నిర్మాణం అర్థం చేసుకోవటం మెను. ఇక్కడ ప్రతి దశలో `messages` అర్రే పూర్తి ఫ్లో ఉంది:

**దశ 1 — ప్రారంభ అభ్యర్థన:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**దశ 2 — మోడల్ టూల్ కాల్స్‌తో ప్రతిస్పందిస్తుంది (కంటెంట్ కాదు):**
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

**దశ 3 — మీరు అసిస్టెంట్ సందేశం మరియు టూల్ ఫలితాన్ని జత చేస్తారు:**
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

**దశ 4 — మోడల్ టూల్ ఫలితం ఉపయోగించి చివరి సమాధానం ఇస్తుంది.**

> **ముఖ్యమైనది:** టూల్ సందేశంలో `tool_call_id` టూల్ కాల్ `id`కు సరిపోలాలి. ఇది మోడల్ ఫలితాలను అభ్యర్థనలకు అనుసంధానించడానికి ఉపయోగపడుతుంది.

---

### వ్యాయామం 5 — బహుళ టూల్ కాల్స్

ఒక మోడల్ ఒకే సమాధానంలో అనేక టూల్ కాల్స్ అభ్యర్థించవచ్చు. బహుళ కాల్స్‌ను ట్రిగ్గర్ చేయడానికి వాడుకరి సందేశాలను మార్చి చూడండి:

```python
# Python లో — యూజర్ సందేశాన్ని మార్చండి:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// జావాస్క్రిప్ట్‌లో - యూజర్ సందేశాన్ని మార్చండి:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

మోడల్ రెండు `tool_calls` తిరిగి ఇస్తుంది — ఒకటి `get_weather` కోసం, రెండోది `get_population` కోసం. మీ కోడ్ ఇప్పటికే అన్ని టూల్ కాల్స్‌ని సపోర్ట్ చేస్తుంది ఎందుకంటే ఇది అన్ని టూల్ కాల్స్ పై లూప్ అవుతుంది.

> **ప్రయత్నం:** వాడుకరి సందేశాన్ని మార్చి నమూనాను మళ్ళీ నడపండి. మోడల్ రెండూ టూల్స్ కాల్ చేస్తుందా?

---

### వ్యాయామం 6 — మీ స్వంత టూల్‌ను జోడించండి

ఏదైనా వెర్షన్‌కి కొత్త టూల్ జోడించండి. ఉదాహరణకి, `get_time` అనే టూల్ జోడించండి:

1. టూల్ స్కీమాను నిర్వచించండి:
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

2. అమలు లాజిక్ జోడించండి:
```python
# పైథాన్
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # ఒక వాస్తవ యాప్‌లో, ఒక టైమ్ జోన్ లైబ్రరీని ఉపయోగించండి
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... ఉన్న సాధనాలు ...
```

3. `tools` యార్రేలో టూల్ జత చేసి "టోక్యోలో సమయం ఎంత?" అని పరీక్షించండి.

> **సవాలు:** ఒక లెక్కింపు చేసే టూల్ జోడించండి, ఉదా: `convert_temperature` దీనికి సెల్సియస్ మరియు ఫారెన్హైట్ మధ్య మార్పిడి ఉంటుంది. "100°F ని సెల్సియస్‌లోకి మార్చు" అని పరీక్షించండి.

---

### వ్యాయామం 7 — SDK యొక్క ChatClient తో టూల్ కాలింగ్ (JavaScript)

JavaScript నమూనా ఇప్పటికే OpenAI SDK బదులు SDK స్థానిక `ChatClient` ఉపయోగిస్తుంది. ఇది OpenAI క్లయింట్ ని సృష్టించకుండానే సులభంగా టూల్-కాలింగ్ అభ్యర్థనలు చేయడానికి సౌలభ్యాన్ని ఇస్తుంది:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient మోడల్ ఆబ్జెక్ట్ నుండి నేరుగా సృష్టించబడుతుంది
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat రెండవ పరామితిగా టూల్స్‌ను స్వీకరించును
const response = await chatClient.completeChat(messages, tools);
```

Python పద్ధతితో పోల్చండి, అది OpenAI SDK ని స్పష్టంగా ఉపయోగిస్తుంది:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

రెండు విధానాలు చెల్లుబాటు. `ChatClient` సౌకర్యవంతం; OpenAI SDK పూర్తి OpenAI పారామీటర్లు అందిస్తుంది.

> **ప్రయత్నం:** JavaScript నమూనాను OpenAI SDK బదులు `ChatClient` ఉపయోగించడం నుండి మార్చండి. మీరు `import OpenAI from "openai"` అవసరం మరియు `manager.urls[0]` నుండి ఎండ్‌పాయింట్‌తో క్లయింట్ సృష్టించాలి.

---

### వ్యాయామం 8 — tool_choice అర్థం చేసుకోండి

`tool_choice` పారామీటర్ మోడల్ టూల్ *వినియోగించాల్సినది* లేదా *స్వేచ్ఛగా ఎంచుకునే*దిగా నియంత్రిస్తుంది:

| విలువు | ప్రవర్తన |
|-------|-----------|
| `"auto"` | మోడల్ టూల్ పిలవడం నిర్ణయిస్తుంది (డిఫాల్ట్) |
| `"none"` | అందుబాటులో ఉన్నా కూడా మోడల్ టూల్స్ పిలవదు |
| `"required"` | మోడల్ కనీసం ఒక టూల్ పిలవాల్సినది |
| `{"type": "function", "function": {"name": "get_weather"}}` | మోడల్ నిర్దిష్ట టూల్ తప్పకుండా పిలవాలి |

Python నమూనాలో ఈ ఎంపికలను ప్రయత్నించండి:

```python
# మోడల్‌ను get_weather ను పిలవడానికి బలవంతం చేయండి
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **గమనిక:** ప్రతి మోడల్ అన్ని `tool_choice` ఎంపికలను మద్దతు ఇవ్వకపోవచ్చు. `"required"` మద్దతు లేకుంటె మోడల్ దీన్ని పట్టీ చూపకుండా `"auto"` గా పనిచేయవచ్చు.

---

## సాధారణ గమనికలు

| సమస్య | పరిష్కారం |
|---------|----------|
| మోడల్ ఎప్పుడూ టూల్స్ పిలవదు | మీరు టూల్-కాలింగ్ మోడల్ (`qwen2.5-0.5b` వంటిది) ఉపయోగిస్తున్నారా అని నిర్ధారించండి. పై పట్టిక చూడండి. |
| `tool_call_id` సరిపోలదు | టూల్ కాల్ ప్రతిస్పందన నుండి `id` ను ఎప్పుడూ ఉపయోగించండి, హార్డ్‌కోడ్ చేయకండి |
| మోడల్ `arguments`లో తప్పు JSON తిరిగిస్తుంది | చిన్న మోడల్స్ కొన్ని సార్లు చెల్లని JSON ఉత్పత్తి చేస్తాయి. `JSON.parse()` ని try/catch లో ర్యాప్ చేయండి |
| మోడల్ ఉనికిలో లేని టూల్ పిలుస్తుంది | `execute_tool` ఫంక్షన్‌లో డిఫాల్ట్ హ్యాండ్లర్ జత చేయండి |
| అంతరాయ రహిత టూల్-కాలింగ్ లూప్ | అత్యధిక రౌండ్స్ సంఖ్య (ఉదా. 5) సెట్ చేయండి, పరపాటి లూప్స్ నివారించడానికి |

---

## ప్రధాన విద్యలు

1. **టూల్ కాలింగ్** మోడల్స్ సమాధానాలను అంచనా వేయడం బదులు functionల అమలును అభ్యర్థించగలవు
2. మోడల్ ఎల్లప్పుడూ కోడ్ నడపదు; నడపాల్సింది లేదా కాకపోవాల్సింది మీ అప్లికేషన్ నిర్ణయిస్తుంది
3. టూల్స్ OpenAI function-calling ఫార్మాట్ లో JSON Schema ఆబ్జెక్టులుగా నిర్వచిస్తారు
4. సంభాషణ బహుళ-తిరుగుడు నమూనాను అనుసరిస్తుంది: యూజర్ → అసిస్టెంట్ (tool_calls) → టూల్ (ఫలితాలు) → అసిస్టెంట్ (చివరి సమాధానం)
5. ఎప్పుడూ **టూల్-కాలింగ్ మద్దతు ఉన్న మోడల్** ఉపయోగించండి (Qwen 2.5, Phi-4-mini)
6. SDK లోని `createChatClient()` సౌకర్యవంతమైన పద్దతిగా OpenAI క్లయింట్ అవసరం లేకుండా టూల్-కాలింగ్ అభ్యర్థనలను తయారు చేస్తుంది

---

[ముందుకు వెళ్లండి: [భాగం 12: Zava Creative Writer కోసం వెబ్ UI నిర్మాణం](part12-zava-ui.md) ](part12-zava-ui.md) — భారీ ఏజెంట్ పైప్‌లైన్‌కు బ్రౌజర్ ఆధారిత ఫ్రంట్ ఎండ్ జోడించడానికి.

---

[← భాగం 10: కస్టమ్ మోడల్స్](part10-custom-models.md) | [భాగం 12: Zava Writer UI →](part12-zava-ui.md)