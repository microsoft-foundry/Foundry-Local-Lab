![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ഭാഗം 11: പ്രാദേശിക മോഡലുകളുമായി ടൂൾ കോളിംഗ്

> **ലക്ഷ്യം:** നിങ്ങളുടെ പ്രാദേശിക മോഡലിന് വെളിപ്പെടുത്തിയ ഫംഗ്ഷനുകൾ (ടൂളുകൾ) വിളിക്കാൻ അനുവദിക്കുക, അതിലൂടെ അത് യാഥാർത്ഥ്യസമയ ഡാറ്റ ശേഖരിക്കാനും കണക്കുകൂട്ടലുകൾ നടത്താനും API കളുമായി ഇടപഴകാനും കഴിയും — എല്ലാം നിങ്ങളുടെ ഡിവൈസിൽ പ്രൈവറ്റ് ആയി പ്രവർത്തിക്കുന്നു.

## ടൂൾ കോളിംഗ് എന്താണ്?

ടൂൾ കോളിംഗ് (അല്ലെങ്കിൽ **ഫംഗ്ഷൻ കോളിംഗ്** എന്നും അറിയപ്പെടുന്നത്) ഒരു ഭാഷാ മോഡലിന് നിങ്ങൾ നിർവ്വചിച്ച ഫംഗ്ഷനുകൾ പ്രവർത്തിപ്പിക്കാൻ അഭ്യർത്ഥിക്കാൻ അനുവദിക്കുന്നു. ഉത്തരം അനുമാനിക്കുന്നതിന് പകരം, ടൂൾ ഉപയോഗിക്കുന്നത് ഏതാണ് എന്ന് മോഡൽ തിരിച്ചറിഞ്ഞ് നിങ്ങളുടെ കോഡ് പ്രവർത്തിപ്പിക്കാനും വേണ്ടി ഒരു ഘടിത അഭ്യർത്ഥന നൽകുന്നു. നിങ്ങളുടെ അപ്ലിക്കേഷൻ ആ ഫംഗ്ഷൻ പ്രവർത്തിപ്പിച്ച് ഫലം തിരിച്ചയച്ച ശേഷം മോഡൽ ആ വിവരത്തിൻറെ അടിസ്ഥാനത്തിൽ അവസാനത്തെ ഉത്തരം നൽകുന്നു.

![ടൂൾ-കോളിംഗ് സ്രോതസ്സ്](../../../images/tool-calling-flow.svg)

ഈ മാതൃക നിർമിക്കുന്നു ഏജന്റുകൾക്ക്:

- **സജീവ ഡാറ്റ പരിശോധിക്കാൻ** (വൈത്ത്, സ്റ്റോക്ക് വിലകൾ, ഡാറ്റാബേസ് ക്വറിയുകൾ)
- **സൂക്ഷ്മ കണക്കുകൂട്ടലുകൾ നടത്താൻ** (ഗണിതം, യൂണിറ്റ് പരിവർത്തനങ്ങൾ)
- **നടപടികൾ സ്വീകരിക്കാനായി** (ഇമെയിലുകൾ അയക്കൽ, ടിക്കറ്റ് സൃഷ്ടിക്കൽ, റെക്കോർഡുകൾ അപ്ഡേറ്റ് ചെയ്യൽ)
- **സ്വകാര്യ സിസ്റ്റമുകൾ ആക്സസ് ചെയ്യാൻ** (ആന്തരിക API കൾ, ഫയൽ സിസ്റ്റങ്ങൾ)

---

## ടൂൾ കോളിംഗ് എങ്ങനെ പ്രവർത്തിക്കുന്നു

ടൂൾ-കോളിംഗ് സ്രോതസിന്റെ നാല് ഘട്ടങ്ങൾ ഉണ്ട്:

| ഘട്ടം | സംഭവിക്കുന്നത് |
|-------|---------------|
| **1. ടൂളുകൾ നിർവ്വചിക്കുക** | JSON സ്കീമ ഉപയോഗിച്ച് ലഭ്യമായ ഫംഗ്ഷനുകൾ വിവരിക്കുക — പേര്, വിവരണം, പാരാമീറ്ററുകൾ |
| **2. മോഡൽ തീരുമാനിക്കുന്നു** | മോഡൽ നിങ്ങളുടെ സന്ദേശവും ടൂൾ നിർവചനങ്ങളും സ്വീകരിക്കുന്നു. ടൂൾ സഹായകമെങ്കിൽ ടെക്‌സ്‌റ്റ് മറുപടിക്ക് പകരം `tool_calls` മറുപടി നൽകുന്നു |
| **3. പ്രാദേശികമായി പ്രവർത്തിപ്പിക്കുക** | നിങ്ങളുടെ കോഡ് ടൂൾ കോളിനെ पार്സ് ചെയ്ത് ഫംഗ്ഷൻ പ്രവർത്തിപ്പിക്കുകയും ഫലം ശേഖരിക്കുകയും ചെയ്യുന്നു |
| **4. അവസാന ഉത്തരം** | ടൂൾ ഫലം മോഡലിന് തിരിച്ചയക്കുന്നു, അത് അവസാനത്തെ മറുപടി സൃഷ്ടിക്കുന്നു |

> **പ്രധാന കുറിപ്പ്:** മോഡൽ എപ്പോഴും കോഡ് പ്രവർത്തിപ്പിക്കുന്നില്ല. ടൂൾ വിളിക്കാൻ മാത്രം *അഭ്യർത്ഥിക്കുന്നു*. നിങ്ങൾ ആ അഭ്യർത്ഥന അംഗീകരിക്കുമോ എന്നത് നിങ്ങളുടെ നിയന്ത്രണത്തിലാണ് — ഇതുമൂലം നിങ്ങൾക്ക് പൂർണ്ണ നിയന്ത്രണം ഉണ്ടാവുന്നു.

---

## ടൂൾ കോളിംഗ് പിന്തുണയ്‌ക്കുന്ന മോഡലുകൾ ഏവ?

എല്ലാ മോഡലുകളും ടൂൾ-കോളിംഗ് പിന്തുണയ്ക്കുന്നില്ല. നിലവിലെ ഫോണ്ട്രി ലോക്കൽ കാറ്റലോഗിൽ, താഴെപ്പെട്ട മോഡലുകൾ ടൂൾ-കോളിംഗ് കഴിവ് ഉൾക്കൊള്ളുന്നു:

| മോഡൽ | വലുപ്പം | ടൂൾ കോളിംഗ് |
|-------|---------|:-------------:|
| qwen2.5-0.5b | 822 എംബി | ✅ |
| qwen2.5-1.5b | 1.8 ജിബി | ✅ |
| qwen2.5-7b | 6.3 ജിബി | ✅ |
| qwen2.5-14b | 11.3 ജിബി | ✅ |
| qwen2.5-coder-0.5b | 822 എംബി | ✅ |
| qwen2.5-coder-1.5b | 1.8 ജിബി | ✅ |
| qwen2.5-coder-7b | 6.3 ജിബി | ✅ |
| qwen2.5-coder-14b | 11.3 ജിബി | ✅ |
| phi-4-mini | 4.6 ജിബി | ✅ |
| phi-3.5-mini | 2.6 ജിബി | ❌ |
| phi-4 | 10.4 ജിബി | ❌ |

> **ടിപ്പ്:** ഇത്ത ലാബിനായി നാം **qwen2.5-0.5b** ഉപയോഗിക്കുന്നു — ഇത് ചെറുതും (822 എംബി ഡൗൺലോഡ്), വേഗമേറിയതും വിശ്വസ്ഥമായ ടൂൾ-കോളിംഗ് പിന്തുണയുള്ളതാണ്.

---

## പഠന ലക്ഷ്യങ്ങൾ

ഈ ലാബിന്റെ അവസാനം നിങ്ങൾക്ക് കഴിയും:

- ടൂൾ-കോളിംഗ് മാതൃകയും അത് എതിനു വേണ്ടി പ്രാധാന്യമുള്ളതാണ് എന്ന് വിശദീകരിക്കുക
- OpenAI ഫംഗ്ഷൻ-കോളിംഗ് ഫോർമാറ്റ് ഉപയോഗിച്ച് ടൂൾ സ്കീമകൾ നിർവ്വചിക്കുക
- മൾട്ടി-ടേൺ ടൂൾ-കോളിംഗ് സംഭാഷണ പ്രക്രിയ കൈകാര്യം ചെയ്യുക
- പ്രാദേശികമായി ടൂൾ കോളുകൾ പ്രവർത്തിപ്പിച്ച് ഫലങ്ങൾ മോഡൽക്ക് തിരിച്ചയക്കുക
- ടൂൾ-കോളിംഗ് സംഭവങ്ങൾക്ക് അനുയോജ്യമായ മോഡൽ തിരഞ്ഞെടുക്കുക

---

## മുൻ‌അവശ്യങ്ങൾ

| ആവശ്യകത | വിശദാംശങ്ങൾ |
|----------|------------|
| **Foundry Local CLI** | ഇൻസ്റ്റാൾ ചെയ്‌ത് നിങ്ങളുടെ `PATH` ൽ ([Part 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript, അല്ലെങ്കിൽ C# SDK ഇൻസ്റ്റാൾ ചെയ്‌തിട്ടുള്ളത് ([Part 2](part2-foundry-local-sdk.md)) |
| **ടൂൾ-കോളിംഗ് മോഡൽ** | qwen2.5-0.5b (സ്വയം ഡൗൺലോഡ് ചെയ്യും) |

---

## അഭ്യസനങ്ങൾ

### അഭ്യാസം 1 — ടൂൾ-കോളിംഗ് സ്രോതസ് മനസ്സിലാക്കുക

കോട് എഴുതുന്നതിന് മുമ്പ്, ഈ സീക്വൻസ് ഡയഗ്രാം പഠിക്കുക:

![ടൂൾ-കോളിംഗ് സീക്വൻസ് ഡയഗ്രാം](../../../images/tool-calling-sequence.svg)

**പ്രധാന നിരീക്ഷണങ്ങൾ:**

1. ടൂളുകൾ മുൻകൂട്ടി JSON സ്കീമ ഒബ്ജക്റ്റുകളായി നിർവചിക്കുന്നു
2. മോഡലിന്റെ മറുപടി പതിവുള്ള ഉള്ളടക്കത്തിന് പകരം `tool_calls` ഉൾക്കൊള്ളുന്നു
3. ഓരോ ടൂൾ കോളിനും തിരികെ ഫലങ്ങൾ നൽകിയപ്പോൾ ഉപയോഗിക്കേണ്ട ഒരു അപൂർവ്വമായ `id` ഉണ്ട്
4. ഫലം ഉണ്ട് കൂടാതെ പൂർവ്വ സന്ദേശങ്ങളും മോഡലിന് ദൃശ്യമാണ് അവസാന ഉത്തരം ഉണ്ടാക്കുമ്പോൾ
5. ഒരു മറുപടിയിൽ ഒരേ സമയം ഒന്നിലധികം ടൂൾ കോളുകൾ ഉണ്ടായേക്കും

> ** ചർച്ച:** മൊഡൽ നേരിട്ട് ഫംഗ്ഷനുകൾ പ്രവർത്തിപ്പിക്കാതെ ടൂൾ കോളുകൾ മടക്കം നൽകുന്നത് എന്തുകൊണ്ടാണ്? ഇത് ഏത് സുരക്ഷാ ഗുണങ്ങൾ നൽകുന്നു?

---

### അഭ്യാസം 2 — ടൂൾ സ്കീമ നിർവചിക്കൽ

ടൂളുകൾ സ്വതന്ത്രമായി OpenAI ഫംഗ്ഷൻ-കോളിംഗ് ഫോർമാറ്റ് ഉപയോഗിച്ച് നിർവചിക്കുന്നു. ഓരോ ടൂളിനും വേണ്ടത്:

- **`type`**: എപ്പോഴും `"function"`
- **`function.name`**: വിവരണാത്മകമായി ഒരു ഫംഗ്ഷൻ പേര് (ഉദാ. `get_weather`)
- **`function.description`**: ഒരു വ്യക്തമായ വിവരണം — ടൂൾ വിളിക്കാൻ മോഡലിന് സഹായം നൽകും
- **`function.parameters`**: പ്രതീക്ഷിച്ച അർഗ്യുമെന്റുകൾ രേഖപ്പെടുത്തിയ JSON സ്കീമ ഒബ്ജക്റ്റ്

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

> **ടൂൾ വിവരണത്തിന് മികച്ച രീതികൾ:**
> - വ്യക്തമായിരിക്കണം: "ഒരു പ്രത്യേക നഗരത്തിന് ഇപ്പോഴത്തെ കാലാവസ്ഥ ലഭിക്കുക" എന്നത് "കാലാവസ്ഥ ലഭിക്കുക" എന്നതിനേക്കാൾ നല്ലത്
> - പാരാമീറ്ററുകൾ സ്പഷ്ടമായി വിശദീകരിക്കുക: മോഡൽ ഇവ വായിച്ച് ശരിയായ മൂല്യങ്ങൾ നൽകുന്നു
> - ആവശ്യമായ പാരാമീറ്ററുകളും ഓപ്ഷണലുകളും പ്രത്യേകം സൂചിപ്പിക്കുക — ഇത് മോഡലിനുള്ള തീരുമാനമെടുക്കുന്നതിൽ സഹായിക്കുന്നു

---

### അഭ്യാസം 3 — ടൂൾ-കോളിംഗ് ഉദാഹരണങ്ങൾ പ്രവർത്തിപ്പിക്കുക

ഓരോ ഭാഷാ സാമ്പിൾ ഓർജിനായി രണ്ട് ടൂളുകൾ (`get_weather`യും `get_population`ഉം) നിർവചിക്കപ്പെട്ടു, ടൂൾ ഉപയോഗം പ്രേരിപ്പിക്കുന്ന ചോദ്യം അയയ്ക്കുന്നു, പ്രാദേശികമായി ടൂൾ പ്രവർത്തിപ്പിക്കുകയും ഫലം തിരികെ അയയ്ക്കുകയും ചെയ്തു, അവസാന ഉത്തരം ലഭിക്കുന്നു.

<details>
<summary><strong>🐍 Python</strong></summary>

**മുൻ‌അവശ്യങ്ങൾ:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / ലിനക്സ്:
source venv/bin/activate

pip install -r requirements.txt
```

**പ്രവർത്തിപ്പിക്കുക:**
```bash
python foundry-local-tool-calling.py
```

**പ്രതീക്ഷിക്കാവുന്ന ഔട്ട്പുട്ട്:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**കോഡ് വിശദീകരണം** (`python/foundry-local-tool-calling.py`):

```python
# ഫംഗ്ഷൻ സ്കീമകളുടെ പട്ടികയായി ടൂളുകൾนิർവചിക്കുക
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

# ടൂളുകളോടുകൂടെ അയയ്ക്കുക — മോഡൽ കണ്ടന്റ് പകരം tool_calls മടക്കും
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# മോഡൽ ഒരു ടൂൾ വിളിക്കാൻ ആഗ്രഹിക്കുന്നുണ്ടോ എന്ന് പരിശോധിക്കുക
if response.choices[0].message.tool_calls:
    # ടൂൾ പ്രവർത്തിപ്പിച്ച് ഫലം മടക്കം അയയ്ക്കുക
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**മുൻ‌അവശ്യങ്ങൾ:**
```bash
cd javascript
npm install
```

**പ്രവർത്തിപ്പിക്കുക:**
```bash
node foundry-local-tool-calling.mjs
```

**പ്രതീക്ഷിക്കാവുന്ന ഔട്ട്പുട്ട്:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**കോഡ് വിശദീകരണം** (`javascript/foundry-local-tool-calling.mjs`):

ഈ ഉദാഹരണം OpenAI SDK അല്ലാതെ ഫൗണ്ട്രി ലോക്കലിന്റെ ജന്മസദ് `ChatClient` ഉപയോഗിക്കുന്നു, ഇത് ഉപയോഗിക്കാൻ എളുപ്പമാണ് `createChatClient()` მეთോഡ്:

```javascript
// മോഡൽ ഒബ്ജക്റ്റിൽ നിന്ന് നേരിട്ട് ഒരു ChatClient നേടുക
const chatClient = model.createChatClient();

// ടൂളുകളോടെ അയയ്ക്കുക — ChatClient OpenAI-സമ്മതമായ ഫോർമാറ്റ് കൈകാര്യം ചെയ്യുന്നു
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// ടൂൾ വിളിപ്പെടുത്തലുകൾ പരിശോധിക്കുക
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // ടൂളുകൾ പ്രവർത്തിപ്പിച്ച് ഫലങ്ങൾ തിരിച്ചു അയയ്ക്കുക
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**മുൻ‌അവശ്യങ്ങൾ:**
```bash
cd csharp
dotnet restore
```

**പ്രവർത്തിപ്പിക്കുക:**
```bash
dotnet run toolcall
```

**പ്രതീക്ഷിക്കാവുന്ന ഔട്ട്പുട്ട്:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**കോഡ് വിശദീകരണം** (`csharp/ToolCalling.cs`):

C# ൽ ടൂൾ നിർവചിക്കാൻ `ChatTool.CreateFunctionTool` സഹായം ഉപയോഗിക്കുന്നു:

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

### അഭ്യാസം 4 — ടൂൾ-കോളിംഗ് സംഭാഷണ സ്രോതസ്

സന്ദേശ ഘടന മനസ്സിലാക്കുന്നത് അനിവാര്യമാണ്. ഓരോ ഘട്ടവുമാണ് ഈ `messages` അറേ volledig:

**ഘട്ടം 1 — തുടക്ക അഭ്യർത്ഥന:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**ഘട്ടം 2 — മോഡൽ `tool_calls` (ഉള്ളടക്കം അല്ല) അയക്കുന്നു:**
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

**ഘട്ടം 3 — നിങ്ങൾ അസിസ്റ്റന്റ് സന്ദേശവും ടൂൾ ഫലവും ചേർക്കുന്നു:**
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

**ഘട്ടം 4 — മോഡൽ ടൂൾ ഫലം ഉപയോഗിച്ച് അവസാന ഉത്തരം നൽകുന്നു.**

> **പ്രധാനപ്പെട്ടതും:** ടൂൾ സന്ദേശത്തിലെ `tool_call_id` ടൂൾ-കാൾന്റെ `id` യുമായി പൊരുത്തപ്പെടണം. ഇത് മോഡലിന് ഫലത്തെയും അഭ്യർത്ഥനയും ബന്ധിപ്പിക്കാൻ സഹായിക്കുന്നു.

---

### അഭ്യാസം 5 — ഒട്ടേറെ ടൂൾ കോളുകൾ

ഒരു മോഡൽ ഒരിക്കൽ മറുപടിയിൽ ഒന്നിലധികം ടൂൾ കോളുകൾ അഭ്യർത്ഥിക്കാനാകും. പല കോളുകൾ ട്രിഗർ ചെയ്യാൻ ഉപയോക്തൃ സന്ദേശം മാറ്റി പരീക്ഷിക്കുക:

```python
# Python-ൽ — ഉപയോക്തൃ സന്ദേശം മാറ്റുക:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// ജാവാസ്ക്രിപ്റ്റിൽ — ഉപയോക്തൃ സന്ദേശം മാറ്റുക:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

മോഡൽ രണ്ട് `tool_calls` മടങ്ങി നൽകണം — ഒന്ന് `get_weather`നും മറ്റൊന്ന് `get_population`നും ഉള്ളത്. നിങ്ങളുടെ കോഡ് ഇതിനകം എല്ലാ ടൂൾ കോളുകളിലൂടെയും ലൂപ്പ് ചെയ്ത് ഈ കൈകാര്യം ചെയ്യുന്നു.

> **പ്രയത്നിക്കുക:** ഉപയോക്തൃ സന്ദേശം മാറ്റി വീണ്ടും സാമ്പിൾ ഓടിക്കുക. മോഡൽ രണ്ടും ടൂളുകളും വിളിക്കുന്നുണ്ടോ?

---

### അഭ്യാസം 6 — നിങ്ങളുടെ സ്വന്തം ടൂൾ ചേർക്കുക

സാമ്പിൾകളിൽ ഒന്ന് വിപുലീകരിച്ച് പുതിയ ഒരു ടൂൾ ചേർക്കുക. ഉദാഹരണത്തിന്, `get_time` ടൂൾ ചേർക്കുക:

1. ടൂൾ സ്കീമ നിർവചിക്കുക:
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

2. പ്രവർത്തന ലൊജിക് ചേർക്കുക:
```python
# പൈത്തൺ
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # യഥാർത്ഥ അപ്ലിക്കേഷനിൽ, ഒരു ടൈംസോൺ ലൈബ്രറി ഉപയോഗിക്കുക
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... നിലവിലുള്ള ഉപകരണങ്ങൾ ...
```

3. ടൂൾ `tools` അറേയിൽ ചേർക്കുകയും: "ടോക്കിയോയിൽ സമയം എത്രയാണ്?" എന്ന ചോദ്യം പരീക്ഷിക്കുകയും ചെയ്യുക

> **ചലഞ്ച്:** ഒരു കണക്കുകൂട്ടൽ നടത്തുന്ന ടൂൾ ചേർക്കുക, ഉദാഹരണത്തിന് `convert_temperature` സെൽഷ്യസ്-ഫാറൻഹീറ്റ് പരിവർത്തനം നടത്തുന്നത്. ഇതായി: "100°F ന്റെ സെൽഷ്യസിൽ മാറ്റുക" എന്ന ചോദ്യം പരീക്ഷിക്കൂ.

---

### അഭ്യാസം 7 — SDK യുടെ ChatClient ഉപയോഗിച്ച് ടൂൾ കോളിംഗ് (ജാവാസ്ക്രിപ്റ്റ്)

ജാവാസ്ക്രിപ്റ്റ് സാമ്പിൾ ഇതിനകം OpenAI SDK അല്ലാതെ SDK യുടെ ജന്മസദ് `ChatClient` ഉപയോഗിക്കുന്നു. ഇത് സംരംഭകർക്ക് വെറും ഒരു ഹൈഫ്ലെക്‌സ് ആയി OpenAI ക്ലയന്റ് നിർമ്മിക്കുന്നത് ഒഴിവാക്കുന്നു:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient മോഡൽ ഓബ്ജക്റ്റിൽ നിന്ന് നേരിട്ട് സൃഷ്ടിക്കപ്പെടുന്നു
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat രണ്ടാമത്തെ പാരാമീറ്ററായി ടൂളുകൾ സ്വീകരിക്കുന്നു
const response = await chatClient.completeChat(messages, tools);
```

Python സമീപനവുമായി താരതമ്യം ചെയ്യുക, അത് OpenAI SDK പ്രസക്തമായി ഉപയോഗിക്കുന്നു:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

ഇരുവയ്പ്പുകളും സാധുവാണ്. `ChatClient` കൂടുതൽ സൗകര്യമേറിയതാണ്; OpenAI SDK നിങ്ങൾക്കു മുഴുവൻ OpenAI പാരാമീറ്ററുകളിൽ ആക്സസ് നൽകുന്നു.

> **പ്രയത്നിക്കുക:** ജാവാസ്ക്രിപ്റ്റ് സാമ്പിൾ `ChatClient` പോലെ അല്ലാതെ OpenAI SDK ഉപയോഗി്ക്കാൻ മാറ്റി. `import OpenAI from "openai"` ആവശ്യമാണ്, ക്ലയന്റ് `manager.urls[0]` എന്ന എಂಡ്പോയിന്റ് ഉപയോഗിച്ച് നിർമ്മിക്കുക.

---

### അഭ്യാസം 8 — `tool_choice` മനസ്സിലാക്കുക

`tool_choice` പാരാമീറ്റർ മോഡൽ *ചിലപ്പോൾ* ടൂൾ ഉപയോഗിക്കണം അല്ലെങ്കിൽ സ്വതന്ത്രമായി തിരഞ്ഞെടുക്കാം എന്നും നിയന്ത്രിക്കുന്നു:

| മൂല്യം | പെരുമാറ്റം |
|--------|-----------|
| `"auto"` | മോഡൽ ടൂൾ വിളിക്കണോ എന്നത് തീരുമാനിക്കുന്നു (ഡീഫോൾട്ട്) |
| `"none"` | മോഡൽ ടൂളുകൾ വിളിക്കില്ല, നൽകിയിട്ടുണ്ടെങ്കിലും |
| `"required"` | മോഡൽ కనీసം ഒരു ടൂൾ വിളിക്കണം |
| `{"type": "function", "function": {"name": "get_weather"}}` | മോഡൽ നൽകിയ ടൂൾ മാത്രം വിളിക്കണം |

Python സാമ്പിൾ ഓരോ ഓപ്ഷനുകളും പരീക്ഷിക്കുക:

```python
# മാതൃക get_weather വിളിക്കാൻ ബലപ്പെടുത്തുക
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **ഗുരുതരമായ കുറിപ്പ്:** എല്ലാർക്കും എല്ലാ `tool_choice` ഓപ്ഷനുകൾ പിന്തുണയ്‌ക്കണമെന്നില്ല. ഒരു മോഡൽ `"required"` പിന്തുണയ്ക്കാത്ത പക്ഷം, സജ്ജീകരണം പരിഗണിക്കാതെ `"auto"` പോലെ പ്രവർത്തിക്കാം.

---

## സാധാരണ പിഴവുകൾ

| പ്രശ്‌നം | പരിഹാരം |
|----------|-----------|
| മോഡൽ ഒരിക്കലും ടൂളുകൾ വിളിക്കുന്നില്ല | ഒരു ടൂൾ-കോളിംഗ് മോഡൽ ഉപയോഗിക്കുന്നുണ്ടോ എന്ന് ഉറപ്പാക്കുക (എ.ജി. qwen2.5-0.5b). മുകളിൽ നൽകിയ പട്ടിക പരിശോധിക്കുക. |
| `tool_call_id` പൊരുത്തക്കേടുകൾ | ടൂൾ-കാൾ ഫലത്തിൽ നിന്ന് നൽകി അഡ് ഉപയോഗിക്കുക, ഹാർഡ്‌കോഡഡ് മൂല്യം ഒഴിവാക്കുക |
| മോഡൽ `arguments` ഇൽ അസാധുവായ JSON നൽകുന്നു | ചെറിയ മോഡലുകൾ ചിലപ്പോൾ അസാധുവായ JSON ഉൽപ്പാദിപ്പിക്കാം. `JSON.parse()` ട്രീ/catch വൃത്തത്തിലാക്കി ഇടുക |
| മോഡൽ ഉള്ളടക്കമില്ലാത്ത ടൂൾ വിളിക്കുന്നു | നിങ്ങളുടെ `execute_tool` ഫംഗ്ഷനിൽ ഡിഫാൾട്ട് ഹാൻഡ്ലർ ചേർക്കുക |
| അനന്ത ടൂൾ-കോളിംഗ് ലൂപ് | പരമാവധി ചുറ്റുവട്ടം (ഉദാ. 5) നിശ്ചയിച്ച് അനിമിത ലൂപുകൾ തടയുക |

---

## പ്രധാനപ്പെട്ട ആശയങ്ങൾ

1. **ടൂൾ കോളിംഗ്** മോഡലുകൾക്ക് അനുമാനമല്ല, ഫംഗ്ഷൻ പ്രവർത്തനം അഭ്യർത്ഥിക്കാൻ അനുവദിക്കുന്നു
2. മോഡൽ ഒരിക്കലും കോഡ് പ്രവർത്തിപ്പിക്കാറില്ല; നിങ്ങൾ ആ തീരുമാനമെടുക്കും
3. ടൂളുകൾ OpenAI ഫംഗ്ഷൻ-കോളിംഗ് ഫോർമാറ്റ് അനുസരിച്ച JSON സ്കീമ ഒബ്ജക്റ്റുകളാണ്
4. സംഭാഷണം **മൾട്ടി-ടേൺ മാതൃക** ഉപയോഗിക്കുന്നു: ഉപയോക്താവ് > അസിസ്റ്റന്റ് (tool_calls) > ടൂൾ (ഫലം) > അസിസ്റ്റന്റ് (അവസാന ഉത്തരവ്)
5. നിങ്ങൾ *ടൂൾ കോളിംഗ്* പിന്തുണയുള്ള മോഡൽ എപ്പോഴും ഉപയോഗിക്കുക (Qwen 2.5, Phi-4-mini)
6. SDK യുടെ `createChatClient()` സൗകര്യമേറിയ മാർഗമാണ് OpenAI ക്ലയന്റ് നിർമ്മിക്കാതെ ടൂൾ-കോളിംഗ് അഭ്യർത്ഥനകൾ നിർവഹിക്കാൻ

---

ഇലേഖനത്തിന് തുടരണം [ഭാഗം 12: സാവ ക്രിയേറ്റീവ് റൈറ്റർ വെബ് UI നിർമ്മിക്കൽ](part12-zava-ui.md) — യഥാർത്ഥ സമയം സ്ട്രീമിങ്ങ് സപ്പോർട്ടോടെയുള്ള മൾട്ടി-എജന്റ് പൈപ്പ്‌ലൈൻ ബ്രൗസർ അടിസ്ഥാന എഫ്രണ്ടിന്.

---

[← ഭാഗം 10: കസ്റ്റം മോഡലുകൾ](part10-custom-models.md) | [ഭാഗം 12: സാവ റൈറ്റർ UI →](part12-zava-ui.md)