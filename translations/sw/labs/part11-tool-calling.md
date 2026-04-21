![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Sehemu ya 11: Kupiga Zana kwa Modeli za Ndani

> **Lengo:** Ruhusu modeli yako ya ndani kupiga simu za nje (zile zana) ili iweze kupata data za wakati halisi, kufanya mahesabu, au kuingiliana na API — zote zikifanywa kwa faragha kwenye kifaa chako.

## Kwa Nini Kupiga Simu Zana?

Kupiga simu za zana (pia huitwa **kupiga simu za kazi**) huruhusu modeli ya lugha kuomba utekelezaji wa kazi unazozitengeneza. Badala ya kubahatisha jibu, modeli hutambua wakati zana itakavyosaidia na kurudisha ombi lililoandaliwa ili msimbo wako utekeleze. Programu yako inaendesha kazi, inatuma matokeo nyuma, na modeli hujumuisha habari hiyo kwenye jibu lake la mwisho.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Mfumo huu ni muhimu kwa kujenga mawakala wanaoweza:

- **Kuchukua data ya moja kwa moja** (hali ya hewa, bei za hisa, maswali ya hifadhidata)
- **Kufanya mahesabu sahihi** (hesabu, kubadilisha vipimo)
- **Kutenda matendo** (kutuma barua pepe, kuunda tiketi, kusasisha rekodi)
- **Kupata mifumo binafsi** (API za ndani, mifumo ya faili)

---

## Jinsi Kupiga Simu Zana Kunywea

Mtiririko wa kupiga simu za zana una hatua nne:

| Hatua | Kinachotokea |
|-------|--------------|
| **1. Eleza zana** | Ufafanua kazi zinazopatikana kwa kutumia JSON Schema — jina, maelezo, na vigezo |
| **2. Modeli inaamua** | Modeli inapokea ujumbe wako pamoja na maelezo ya zana. Ikiwa zana ingesaidia, inarudisha jibu la `tool_calls` badala ya jibu la maandishi |
| **3. Tekeleza ndani** | Msimbo wako huchambua simu ya zana, inatekeleza kazi, na kukusanya matokeo |
| **4. Jibu la mwisho** | Unatumia matokeo ya zana kumtumia modeli, ambayo hutengeneza jibu lake la mwisho |

> **Mambo ya muhimu:** Modeli haitekelezi msimbo kamwe. Inatoa *ombi* tu liitwe zana. Programu yako inaamua kama itaheshimu ombi hilo — hii inakuweka chini ya udhibiti kamili.

---

## Modeli Gani Zinazounga Mkono Kupiga Simu Zana?

Siyo modeli zote zinaweza kupiga simu za zana. Katika orodha ya Foundry Local ya sasa, modeli zifuatazo zina uwezo wa kupiga simu zana:

| Modeli | Ukubwa | Kupiga Simu Zana |
|--------|---------|:----------------:|
| qwen2.5-0.5b | 822 MB | ✅ |
| qwen2.5-1.5b | 1.8 GB | ✅ |
| qwen2.5-7b   | 6.3 GB  | ✅ |
| qwen2.5-14b  | 11.3 GB | ✅ |
| qwen2.5-coder-0.5b | 822 MB | ✅ |
| qwen2.5-coder-1.5b | 1.8 GB | ✅ |
| qwen2.5-coder-7b   | 6.3 GB | ✅ |
| qwen2.5-coder-14b  | 11.3 GB | ✅ |
| phi-4-mini | 4.6 GB  | ✅ |
| phi-3.5-mini | 2.6 GB | ❌ |
| phi-4       | 10.4 GB | ❌ |

> **Vidokezo:** Kwa mazoezi haya tunatumia **qwen2.5-0.5b** — ni ndogo (822 MB kupakua), haraka, na ina msaada wa kuaminika wa kupiga simu za zana.

---

## Malengo ya Kujifunza

Mwisho wa mazoezi haya utaweza:

- Kufafanua mfano wa kupiga simu zana na kwa nini ni muhimu kwa mawakala wa AI
- Kueleza schema za zana kwa kutumia muundo wa OpenAI wa kupiga simu kazi
- Kudhibiti mtiririko wa mazungumzo ya zana ya zamu nyingi
- Kutekeleza simu za zana ndani yako na kurudisha matokeo kwenye modeli
- Kuchagua modeli sahihi kwa hali za kupiga simu zana

---

## Mahitaji Muhimu

| Mahitaji | Maelezo |
|----------|---------|
| **CLI ya Foundry Local** | Imesakinishwa na iko kwenye `PATH` yako ([Sehemu 1](part1-getting-started.md)) |
| **SDK ya Foundry Local** | SDK ya Python, JavaScript, au C# imewekwa ([Sehemu 2](part2-foundry-local-sdk.md)) |
| **Modeli inayopiga simu zana** | qwen2.5-0.5b (itapakuliwa moja kwa moja) |

---

## Mazoezi

### Zoezi 1 — Elewa Mtiririko wa Kupiga Simu za Zana

Kabla ya kuandika msimbo, chunguza mchoro huu wa utaratibu:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Matukio muhimu:**

1. Unaeleza zana mapema kama vitu vya JSON Schema
2. Jibu la modeli lina `tool_calls` badala ya maudhui ya kawaida
3. Kila simu ya zana ina `id` ya kipekee lazima uitaje unapotoa matokeo
4. Modeli inaona ujumbe wote uliopita *pamoja na* matokeo ya zana wakati wa kutoa jibu la mwisho
5. Simu nyingi za zana zinaweza kutokea katika jibu moja

> **Majadiliano:** Kwa nini modeli hurudisha simu za zana badala ya kutekeleza kazi moja kwa moja? Faida gani za usalama hii inaleta?

---

### Zoezi 2 — Eleza Schema za Zana

Zana zinaelezwa kwa kutumia muundo wa kawaida wa OpenAI wa kupiga simu kazi. Kila zana inahitaji:

- **`type`**: Daima `"function"`
- **`function.name`**: Jina la kazi linaloelezea (mfano, `get_weather`)
- **`function.description`**: Maelezo wazi — modeli hutumia hii kuamua wakati wa kupiga simu zana
- **`function.parameters`**: Kitu cha JSON Schema kinachoelezea hoja zinazotarajiwa

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

> **Mafunzo bora kwa maelezo ya zana:**
> - Kuwa maalum: "Pata hali ya hewa ya sasa kwa jiji fulani" ni bora kuliko "Pata hali ya hewa"
> - Elezea vigezo kwa uwazi: modeli huchukua maelezo haya kujaza thamani sahihi
> - Taja vigezo vilivyohitajika na visivyo vya lazima — hii husaidia modeli kujua itahitaji kuuliza nini

---

### Zoezi 3 — Endesha Mifano ya Kupiga Simu za Zana

Kila sampuli ya lugha inaeleza zana mbili (`get_weather` na `get_population`), inatuma swali linalochochea matumizi ya zana, inatekeleza zana ndani, na kutuma matokeo nyuma kwa jibu la mwisho.

<details>
<summary><strong>🐍 Python</strong></summary>

**Mahitaji:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Endesha:**
```bash
python foundry-local-tool-calling.py
```

**Matokeo Yanayotarajiwa:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Maelezo ya msimbo** (`python/foundry-local-tool-calling.py`):

```python
# Tafsiri zana kama orodha ya mifumo ya kazi
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

# Tuma kwa zana — mfano unaweza kurudisha tool_calls badala ya maudhui
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Angalia ikiwa mfano unataka kuita zana
if response.choices[0].message.tool_calls:
    # Tekeleza zana na tuma matokeo kurudi nyuma
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Mahitaji:**
```bash
cd javascript
npm install
```

**Endesha:**
```bash
node foundry-local-tool-calling.mjs
```

**Matokeo Yanayotarajiwa:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Maelezo ya msimbo** (`javascript/foundry-local-tool-calling.mjs`):

Mfano huu unatumia `ChatClient` asilia wa SDK ya Foundry Local badala ya SDK ya OpenAI, unaonyesha urahisi wa njia ya `createChatClient()`:

```javascript
// Pata ChatClient moja kwa moja kutoka kwa kitu cha mfano
const chatClient = model.createChatClient();

// Tuma kwa zana — ChatClient hushughulikia muundo unaolingana na OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Angalia kwa wito wa zana
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Tekeleza zana na tuma matokeo kurudi
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Mahitaji:**
```bash
cd csharp
dotnet restore
```

**Endesha:**
```bash
dotnet run toolcall
```

**Matokeo Yanayotarajiwa:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Maelezo ya msimbo** (`csharp/ToolCalling.cs`):

C# inatumia msaidizi `ChatTool.CreateFunctionTool` kufafanua zana:

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

### Zoezi 4 — Mtiririko wa Mazungumzo ya Kupiga Simu za Zana

Kuelewa muundo wa ujumbe ni muhimu. Huu hapa mtiririko kamili, unaonyesha safu ya `messages` katika kila hatua:

**Hatua 1 — Ombi la awali:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Hatua 2 — Modeli inajibu na tool_calls (si maudhui):**
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

**Hatua 3 — Unaongeza ujumbe wa msaidizi NA matokeo ya zana:**
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

**Hatua 4 — Modeli hutengeneza jibu la mwisho kwa kutumia matokeo ya zana.**

> **Muhimu:** `tool_call_id` katika ujumbe wa zana lazima uendane na `id` ya simu ya zana. Hili ndilo njia modeli huhusisha matokeo na maombi.

---

### Zoezi 5 — Simu Nyingi za Zana

Modeli inaweza kuomba simu kadhaa za zana katika jibu moja. Jaribu kubadilisha ujumbe wa mtumiaji kusababisha simu nyingi:

```python
# Katika Python — badilisha ujumbe wa mtumiaji:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// Katika JavaScript — badilisha ujumbe wa mtumiaji:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Modeli inapaswa kurudisha `tool_calls` mbili — moja kwa `get_weather` na moja kwa `get_population`. Msimbo wako tayari unashughulikia hili kwa kwenda kupitia simu zote za zana.

> **Jaribu:** Badilisha ujumbe wa mtumiaji na endesha tena sampuli. Je, modeli inaita zana zote mbili?

---

### Zoezi 6 — Ongeza Zana Yako

Panua moja ya sampuli na zana mpya. Kwa mfano, ongeza zana ya `get_time`:

1. Eleza schema ya zana:
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

2. Ongeza mantiki ya utekelezaji:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Katika programu halisi, tumia maktaba ya eneo la wakati
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... zana zilizopo ...
```

3. Ongeza zana kwenye safu ya `tools` na jaribu: "Saa gani Tokyo?"

> **Changamoto:** Ongeza zana inayofanya hesabu, kama `convert_temperature` inayobadilisha kati ya Celsius na Fahrenheit. Jaribu na: "Badilisha 100°F kwa Celsius."

---

### Zoezi 7 — Kupiga Simu Zana kwa ChatClient wa SDK (JavaScript)

Mfano wa JavaScript tayari unatumia `ChatClient` asilia wa SDK badala ya SDK ya OpenAI. Hii ni kipengele cha urahisi kinachokutoa usumbufu wa kutengeneza mteja wa OpenAI mwenyewe:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient imetengenezwa moja kwa moja kutoka kwa kitu cha mfano
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat inakubali zana kama parameter ya pili
const response = await chatClient.completeChat(messages, tools);
```

Linganishwa na njia ya Python inayotumia SDK ya OpenAI waziwazi:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Mifumo yote miwili ni halali. `ChatClient` ni rahisi zaidi; SDK ya OpenAI inakupa ufikiaji wa vigezo vyote vya OpenAI.

> **Jaribu:** Badilisha sampuli ya JavaScript kutumia SDK ya OpenAI badala ya `ChatClient`. Utahitaji `import OpenAI from "openai"` na kutengeneza mteja kwa kiungo kutoka `manager.urls[0]`.

---

### Zoezi 8 — Kuelewa tool_choice

Kigezo `tool_choice` hudhibiti kama modeli *lazima* itumie zana au inaweza kuchagua kwa uhuru:

| Thamani | Tabia |
|---------|--------|
| `"auto"` | Modeli inaamua kama ipige simu ya zana (chaguo la kawaida) |
| `"none"` | Modeli haitapiga simu za zana hata kama zimetolewa |
| `"required"` | Modeli lazima ipige simu ya zana angalau moja |
| `{"type": "function", "function": {"name": "get_weather"}}` | Modeli lazima ipige simu ya zana iliyobainishwa |

Jaribu kila chaguo kwenye sampuli ya Python:

```python
# Lazimisha mfano kuitisha get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Kumbuka:** Sio chaguzi zote za `tool_choice` zinaweza kuungwa mkono na modeli zote. Ikiwa modeli haikuunga mkono `"required"`, inaweza kupuuza mipangilio na kufanya kama `"auto"`.

---

## Makosa Yanayojirudia

| Tatizo | Suluhisho |
|--------|-----------|
| Modeli haipigi simu za zana | Hakikisha unatumia modeli inayopiga simu zana (mfano qwen2.5-0.5b). Angalia jedwali hapo juu. |
| `tool_call_id` hailingani | Tumia daima `id` kutoka kwa jibu la simu ya zana, si thamani ngumu |
| Modeli inarudisha JSON isiyo sahihi katika `arguments` | Modeli ndogo mara nyingine hutengeneza JSON isiyo sahihi. Fungua `JSON.parse()` kwa jaribu/kamata |
| Modeli inapiga simu ya zana isiyopo | Ongeza mshughulikiaji asilia ndani ya `execute_tool` |
| Mzunguko wa kupiga simu zana usioisha | Weka idadi ya zamu za juu (mfano 5) kuzuia mzunguko usio na mwisho |

---

## Muhimu Kumbuka

1. **Kupiga simu zana** huruhusu modeli kuomba utekelezaji wa kazi badala ya kubahatisha majibu
2. Modeli **haitekelezi msimbo kamwe**; programu yako inaamua nini kitatekelezwa
3. Zana zimefafanuliwa kama vitu vya **JSON Schema** kufuatia muundo wa kupiga simu kazi wa OpenAI
4. Mazungumzo yanatumia **mfumo wa zamu nyingi**: mtumiaji, kisha msaidizi (tool_calls), kisha zana (matokeo), kisha msaidizi (jibu la mwisho)
5. Tumia **modeli inayounga mkono kupiga simu zana** kila wakati (Qwen 2.5, Phi-4-mini)
6. `createChatClient()` ya SDK inatoa njia rahisi ya kuomba kupiga simu zana bila kuunda mteja wa OpenAI

---

Endelea na [Sehemu 12: Kuunda UI ya Wavuti kwa Mwandishi wa Zava Creative](part12-zava-ui.md) kuongeza kiolesura cha kivinjari kwenye mfululizo wa mawakala wengi kwa mtiririko wa moja kwa moja.

---

[← Sehemu 10: Modeli Maalum](part10-custom-models.md) | [Sehemu 12: Zava Writer UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Angalizo**:
Hati hii imetafsiriwa kwa kutumia huduma ya tafsiri ya AI [Co-op Translator](https://github.com/Azure/co-op-translator). Ingawa tunajitahidi kuhakikisha usahihi, tafadhali fahamu kwamba tafsiri za kiotomatiki zinaweza kuwa na makosa au kasoro. Nguvu ya hati asili katika lugha yake ya asili inapaswa kuzingatiwa kama chanzo cha mamlaka. Kwa taarifa muhimu, tafsiri ya mtaalamu wa binadamu inashauriwa. Hatutawajibika kwa kutilia shaka au tafsiri potofu zinazotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->