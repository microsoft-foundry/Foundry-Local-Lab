![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagi 11: Pagtawag ng Tool gamit ang Lokal na mga Modelo

> **Layunin:** Paganahin ang iyong lokal na modelo na tumawag ng mga panlabas na function (mga tool) upang makakuha ng real-time na data, magsagawa ng mga kalkulasyon, o makipag-ugnayan sa mga API — lahat ay tumatakbo nang pribado sa iyong aparato.

## Ano ang Pagtawag ng Tool?

Ang pagtawag ng tool (kilala rin bilang **pagtawag ng function**) ay nagpapahintulot sa isang language model na humiling ng pagpapatupad ng mga function na iyong tinukoy. Sa halip na hulaan ang sagot, nakikilala ng modelo kung kailan makakatulong ang isang tool at nagbabalik ng isang istrukturadong kahilingan para sa iyong code na ipatupad ito. Pinapatakbo ng iyong aplikasyon ang function, ipinapadala ang resulta pabalik, at isinasama ng modelo ang impormasyong iyon sa huling tugon.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Mahalaga ang pattern na ito para sa pagbuo ng mga ahente na kaya:

- **Maghanap ng live na data** (panahon, presyo ng stock, mga query sa database)
- **Magsagawa ng tumpak na kalkulasyon** (math, pag-convert ng yunit)
- **Gumawa ng mga aksyon** (magpadala ng email, gumawa ng tiket, i-update ang mga talaan)
- **Ma-access ang mga pribadong sistema** (internal na API, file system)

---

## Paano Gumagana ang Pagtawag ng Tool

May apat na yugto ang daloy ng pagtawag ng tool:

| Yugto | Nangyayari |
|-------|-------------|
| **1. Tukuyin ang mga tool** | Inilalarawan mo ang mga magagamit na function gamit ang JSON Schema — pangalan, paglalarawan, at mga parameter |
| **2. Nagpapasya ang modelo** | Tinatanggap ng modelo ang iyong mensahe kasama ang mga depinisyon ng tool. Kung makakatulong ang isang tool, magbabalik ito ng `tool_calls` na tugon sa halip na text na sagot |
| **3. Patakbuhin nang lokal** | Ini-parse ng iyong code ang tawag sa tool, pinaandar ang function, at kinokolekta ang resulta |
| **4. Panghuling sagot** | Ipinapadala mo ang resulta ng tool pabalik sa modelo, na gumagawa ng panghuling sagot |

> **Pangunahing punto:** Hindi kailanman nagpapatupad ng code ang modelo. Humihiling lamang itong tawagan ang isang tool. Ikaw ang nagpapasya kung tatanggapin ang kahilingang iyon — ito ang nagpapanatili ng iyong ganap na kontrol.

---

## Aling mga Modelo ang Sumusuporta sa Pagtawag ng Tool?

Hindi lahat ng modelo ay sumusuporta sa pagtawag ng tool. Sa kasalukuyang katalogo ng Foundry Local, ang mga sumusunod na modelo ay may kakayahan sa pagtawag ng tool:

| Modelo | Laki | Pagtawag ng Tool |
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

> **Tip:** Para sa labo na ito, gagamit tayo ng **qwen2.5-0.5b** — ito ay maliit (822 MB download), mabilis, at may maaasahang suporta sa pagtawag ng tool.

---

## Mga Layunin sa Pagkatuto

Sa pagtatapos ng labo na ito, magagawa mong:

- Ipaliwanag ang pattern ng pagtawag ng tool at bakit ito mahalaga para sa mga AI agent
- Tukuyin ang mga schema ng tool gamit ang OpenAI function-calling format
- Pamahalaan ang multi-turn na daloy ng pag-uusap sa pagtawag ng tool
- Patakbuhin ang mga tawag sa tool nang lokal at bumalik sa mga resulta sa modelo
- Pumili ng tamang modelo para sa mga senaryo ng pagtawag ng tool

---

## Mga Kinakailangan

| Kinakailangan | Detalye |
|-------------|---------|
| **Foundry Local CLI** | Naka-install at nasa iyong `PATH` ([Bahagi 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Naka-install na Python, JavaScript, o C# SDK ([Bahagi 2](part2-foundry-local-sdk.md)) |
| **Isang modelong may tool-calling** | qwen2.5-0.5b (ida-download nang awtomatiko) |

---

## Mga Ehersisyo

### Ehersisyo 1 — Unawain ang Daloy ng Pagtawag ng Tool

Bago magsulat ng code, pag-aralan ang sequence diagram na ito:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Pangunahing mga obserbasyon:**

1. Tinukoy mo nang maaga ang mga tool bilang mga JSON Schema object
2. Naglalaman ng `tool_calls` ang tugon ng modelo sa halip na karaniwang nilalaman
3. Bawat tawag sa tool ay may natatanging `id` na dapat mong gamitin kapag nagbalik ng mga resulta
4. Nakikita ng modelo ang lahat ng mga naunang mensahe *kasama* ang mga resulta ng tool kapag bumubuo ng pangwakas na sagot
5. Maaaring mangyari ang maraming tawag sa tool sa isang tugon

> **Diskusyon:** Bakit nagbabalik ng mga tawag sa tool ang modelo sa halip na direktang magpatakbo ng mga function? Anong mga benepisyo sa seguridad ang hatid nito?

---

### Ehersisyo 2 — Pagtukoy ng Mga Schema ng Tool

Ang mga tool ay tinutukoy gamit ang karaniwang OpenAI function-calling format. Bawat tool ay nangangailangan ng:

- **`type`**: Palaging `"function"`
- **`function.name`**: Isang mapanuring pangalan ng function (hal. `get_weather`)
- **`function.description`**: Isang malinaw na paglalarawan — ginagamit ito ng modelo upang magpasya kung kailan tatawagin ang tool
- **`function.parameters`**: Isang JSON Schema na object na naglalarawan ng mga inaasahang argumento

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

> **Pinakamainam na gawain para sa mga paglalarawan ng tool:**
> - Maging tiyak: mas mabuti ang "kuha ng kasalukuyang panahon para sa isang lungsod" kaysa "kuha ng panahon"
> - Ilahad nang malinaw ang mga parameter: binabasa ng modelo ang paglalarawang ito upang punan ang tamang mga halaga
> - Tukuyin ang mga kinakailangang vs opsyonal na parameter — ito ang tumutulong sa modelo para malaman kung ano ang hihingin

---

### Ehersisyo 3 — Patakbuhin ang Mga Halimbawa ng Pagtawag ng Tool

Nagde-define ang bawat sample na wika ng dalawang tool (`get_weather` at `get_population`), nagpapadala ng tanong na nagpapagana ng paggamit ng tool, pinapatakbo ang tool nang lokal, at ipinapadala pabalik ang resulta para sa pangwakas na sagot.

<details>
<summary><strong>🐍 Python</strong></summary>

**Mga kinakailangan:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Patakbuhin:**
```bash
python foundry-local-tool-calling.py
```

**Inaasahang output:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Paliwanag ng code** (`python/foundry-local-tool-calling.py`):

```python
# Tukuyin ang tools bilang isang listahan ng mga schema ng function
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

# Magpadala gamit ang tools — maaaring magbalik ang modelo ng tool_calls sa halip na nilalaman
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Suriin kung nais ng modelo na tumawag ng tool
if response.choices[0].message.tool_calls:
    # Isagawa ang tool at ibalik ang resulta
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Mga kinakailangan:**
```bash
cd javascript
npm install
```

**Patakbuhin:**
```bash
node foundry-local-tool-calling.mjs
```

**Inaasahang output:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Paliwanag ng code** (`javascript/foundry-local-tool-calling.mjs`):

Ginagamit ng halimbawang ito ang native na Foundry Local SDK `ChatClient` sa halip na OpenAI SDK, na nagpapakita ng kaginhawaan ng `createChatClient()` method:

```javascript
// Kumuha ng ChatClient direkta mula sa model object
const chatClient = model.createChatClient();

// Magpadala gamit ang mga tool — Pinamamahalaan ng ChatClient ang OpenAI-compatible na format
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Suriin para sa mga tawag sa tool
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Isagawa ang mga tool at ipadala ang mga resulta pabalik
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Mga kinakailangan:**
```bash
cd csharp
dotnet restore
```

**Patakbuhin:**
```bash
dotnet run toolcall
```

**Inaasahang output:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Paliwanag ng code** (`csharp/ToolCalling.cs`):

Gumagamit ang C# ng `ChatTool.CreateFunctionTool` helper para tukuyin ang mga tool:

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

### Ehersisyo 4 — Ang Daloy ng Pag-uusap sa Pagtawag ng Tool

Mahalaga ang pag-unawa sa estruktura ng mensahe. Narito ang kumpletong daloy, ipinapakita ang `messages` array sa bawat yugto:

**Yugto 1 — Paunang kahilingan:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Yugto 2 — Tugon ng modelo na may `tool_calls` (hindi content):**
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

**Yugto 3 — Idinagdag mo ang mensahe ng assistant AT ang resulta ng tool:**
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

**Yugto 4 — Gumagawa ang modelo ng pangwakas na sagot gamit ang resulta ng tool.**

> **Mahalaga:** Ang `tool_call_id` sa mensahe ng tool ay dapat tumugma sa `id` mula sa tawag sa tool. Ito ang paraan kung paano inilalagay ng modelo ang mga resulta sa mga kahilingan.

---

### Ehersisyo 5 — Maramihang Tawag sa Tool

Maaaring humiling ang modelo ng maraming tawag sa tool sa isang tugon. Subukang baguhin ang user message upang mag-trigger ng maraming tawag:

```python
# Sa Python — palitan ang mensahe ng gumagamit:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// Sa JavaScript — palitan ang mensahe ng gumagamit:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

Dapat magbalik ang modelo ng dalawang `tool_calls` — isa para sa `get_weather` at isa para sa `get_population`. Hawak na ito ng iyong code dahil inuulit nito ang lahat ng tawag sa tool.

> **Subukan ito:** Baguhin ang user message at patakbuhin muli ang sample. Tinatawagan ba ng modelo ang parehong mga tool?

---

### Ehersisyo 6 — Magdagdag ng Sariling Tool

Palawakin ang isa sa mga sample gamit ang bagong tool. Halimbawa, magdagdag ng tool na `get_time`:

1. Tukuyin ang schema ng tool:
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

2. Idagdag ang lohika ng pagpapatupad:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Sa isang totoong app, gumamit ng isang timezone library
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... umiiral na mga kasangkapan ...
```

3. Idagdag ang tool sa `tools` array at subukan gamit ang: "Anong oras na sa Tokyo?"

> **Hamunin:** Magdagdag ng tool na nagsasagawa ng kalkulasyon, tulad ng `convert_temperature` na nagko-convert mula Celsius papuntang Fahrenheit. Subukan ito gamit ang: "I-convert ang 100°F sa Celsius."

---

### Ehersisyo 7 — Pagtawag ng Tool gamit ang ChatClient ng SDK (JavaScript)

Gamit na ang halimbawa ng JavaScript ang native na `ChatClient` ng SDK sa halip na OpenAI SDK. Isa itong kaginhawaan na nag-aalis ng pangangailangang gumawa ng OpenAI client sa iyong sarili:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Ang ChatClient ay nilikha nang direkta mula sa model object
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// tumatanggap ang completeChat ng tools bilang pangalawang parameter
const response = await chatClient.completeChat(messages, tools);
```

Ihambing ito sa paraan ng Python na gumagamit ng OpenAI SDK nang tahasan:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Parehong valid ang mga pattern. Mas maginhawa ang `ChatClient`; binibigyan ka naman ng OpenAI SDK ng access sa buong hanay ng mga parameter ng OpenAI.

> **Subukan:** Baguhin ang halimbawa ng JavaScript upang gamitin ang OpenAI SDK sa halip na `ChatClient`. Kakailanganin mo ang `import OpenAI from "openai"` at gumawa ng client gamit ang endpoint mula sa `manager.urls[0]`.

---

### Ehersisyo 8 — Pag-unawa sa tool_choice

Kinokontrol ng `tool_choice` parameter kung kailangan *tamang* gumamit ng tool ang modelo o malaya itong pumili:

| Halaga | Pag-uugali |
|-------|-----------|
| `"auto"` | Pinipili ng modelo kung tatawag ng tool (default) |
| `"none"` | Hindi tatawag ang modelo ng kahit anong tool kahit na ibinigay |
| `"required"` | Kailangan ng modelo na tumawag ng kahit isang tool |
| `{"type": "function", "function": {"name": "get_weather"}}` | Kailangan ng modelo na tawagan ang tukoy na tool |

Subukan ang bawat pagpipilian sa halimbawa ng Python:

```python
# Pilitin ang modelo na tawagan ang get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Tandaan:** Hindi lahat ng pagpipilian sa `tool_choice` ay maaaring suportado ng bawat modelo. Kung ang modelo ay hindi sumusuporta sa `"required"`, maaaring balewalain nito ang setting at kumilos bilang `"auto"`.

---

## Karaniwang Mga Problema

| Problema | Solusyon |
|---------|----------|
| Hindi tumatawag ng mga tool ang modelo | Siguraduhing gumagamit ka ng tool-calling model (hal. qwen2.5-0.5b). Tingnan ang talahanayan sa itaas. |
| Hindi tumutugmang `tool_call_id` | Laging gamitin ang `id` mula sa tugon ng tawag sa tool, hindi ang hardcoded na halaga |
| Nagbabalik ang modelo ng maling JSON sa `arguments` | Paminsan-minsan ay gumagawa ng invalid JSON ang mas maliit na mga modelo. Balutin ang `JSON.parse()` sa try/catch |
| Tumatawag ang modelo ng tool na wala | Magdagdag ng default handler sa iyong `execute_tool` function |
| Walang katapusang loop ng pagtawag ng tool | Magtakda ng maximum na bilang ng rounds (hal. 5) upang maiwasan ang runaway loops |

---

## Mahahalagang Pagsasama

1. **Pinapahintulutan ng tool calling** ang mga modelo na humiling ng pagpapatupad ng function sa halip na mahulaan lang ang mga sagot
2. Hindi kailanman nagpapatupad ng code ang modelo; ikaw ang nagdedesisyon kung ano ang ipo-proseso
3. Ang mga tool ay itinukoy bilang **JSON Schema** na mga object na sumusunod sa OpenAI function-calling format
4. Ginagamit ng pag-uusap ang **pattern na multi-turn**: user, assistant (tool_calls), tool (mga resulta), at assistant (panghuling sagot)
5. Laging gumamit ng **modelo na sumusuporta sa tool calling** (Qwen 2.5, Phi-4-mini)
6. Nagbibigay ang `createChatClient()` ng SDK ng maginhawang paraan upang gumawa ng mga kahilingan sa tool calling nang hindi kailangan gumawa ng OpenAI client

---

Magpatuloy sa [Bahagi 12: Pagtatayo ng Web UI para sa Zava Creative Writer](part12-zava-ui.md) upang magdagdag ng browser-based na front end sa multi-agent pipeline na may real-time streaming.

---

[← Bahagi 10: Custom Models](part10-custom-models.md) | [Bahagi 12: Zava Writer UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Paunawa**:  
Ang dokumentong ito ay isinalin gamit ang serbisyo ng AI na pagsasalin na [Co-op Translator](https://github.com/Azure/co-op-translator). Bagaman nagsusumikap kami para sa katumpakan, pakatandaan na ang awtomatikong mga pagsasalin ay maaaring maglaman ng mga pagkakamali o di-tiyak na impormasyon. Ang orihinal na dokumento sa kanyang likas na wika ang dapat ituring na pangunahing sanggunian. Para sa mahahalagang impormasyon, inirerekomenda ang propesyonal na pagsasalin ng tao. Hindi kami mananagot sa anumang hindi pagkakaunawaan o maling interpretasyon na nagmumula sa paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->