![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# အပိုင်း ၁၁: ဒေသဆိုင်ရာ မော်ဒယ်များဖြင့် ကိရိယာခေါ်ဆိုခြင်း

> **ရည်ရွယ်ချက်:** သင့်ဒေသဆိုင်ရာ မော်ဒယ်ကို သင့်စက်ပေါ်တွင် ပုဂ္ဂလိကအသုံးပြု၍ အချက်အလက်ကို အချိန် Real-time ရယူရန်၊ တွက်ချက်မှုများ ပြုလုပ်ရန် သို့မဟုတ် API များနှင့် ဆက်သွယ်ရန် အပြင်ဘက်လုပ်ဆောင်ချက်များ (ကိရိယာများ) ကို ခေါ်ဆိုနိုင်စေရန်ဖြစ်သည်။

## ကိရိယာခေါ်ဆိုခြင်းဆိုသည်မှာ?

ကိရိယာခေါ်ဆိုခြင်း (function calling ဟုပင် သိကြသည်) သည် ဘာသာစကားမော်ဒယ်တစ်ခုကို သင့်ဖန်တီးထားသော လုပ်ဆောင်ချက်များကို အကောင်အထည်ဖော်ရန် တောင်းဆိုခွင့်ပေးသည်။ ဖြေကြားချက်ကို ခန့်မှန်းခြေနည်းမဟုတ်ပဲ ကိရိယာအသုံးပြုရန်လိုအပ်သည်ကို သိရှိပြီး သင့်ကုဒ်များအတွက် ဖွဲ့စည်းထားသော တောင်းဆိုမှုကို ပြန်လည်ပေးပို့သည်။ သင့်အက်ပ်လီကေးရှင်းမှာ လုပ်ဆောင်ချက်ကို လိုက်လံဆောင်ရွက်ပြီး စိစစ်ချက်ကို မော်ဒယ်ထံ ပြန်ပို့ပေးကာ မော်ဒယ်သည် နောက်ဆုံးဖြေကြားချက်မှာ ထည့်သွင်းပါသည်။

![Tool-calling flow](../../../images/tool-calling-flow.svg)

ဒီပုံစံသည် agent များကို တည်ဆောက်ရာတွင် အရေးပါတဲ့ အချက်များမှာ -

- **နောက်ဆုံးအချိန်ဒေတာများကို ရှာဖွေသည်** (ရာသီဥတုပြောင်းလဲမှု၊ စတော့မှတ်ဈေးနှုန်းများ၊ ဒေတာဘေ့စ်မေးခွန်းများ)
- **တိကျသော တွက်ချက်ချက်များ ပြုလုပ်သည်** (သင်္ချာ၊ယူနစ်ပြောင်းလဲခြင်းများ)
- **လုပ်ဆောင်ချက်များ လုပ်ဆောင်သည်** (အီးမေးလ်ပို့ခြင်း၊ တစ်ကတ်ရေးဆွဲခြင်း၊ မှတ်တမ်းများအား ပြန်လည်ပြုပြင်ခြင်း)
- **ပုဂ္ဂလိကစနစ်များကို ချိတ်ဆက်ရယူသည်** (အတွင်းပိုင်း API များ၊ ဖိုင်စနစ်များ)

---

## ကိရိယာခေါ်ဆိုခြင်း၏ လည်ပတ်ပုံ

ကိရိယာခေါ်ဆိုမှု လည်ပတ်မှုအဆင့်များမှာ ၄ ခုဖြစ်သည်-

| အဆင့် | ဖြစ်ပေါ်သောအရာ |
|-------|---------------|
| **၁. ကိရိယာများ သတ်မှတ်ခြင်း** | JSON Schema ဖြင့် ဖေါ်ပြထားသော လုပ်ဆောင်ချက်များ — အမည်၊ ဖော်ပြချက်နှင့် ပါရမီတာများ |
| **၂. မော်ဒယ်ဆုံးဖြတ်ခြင်း** | မော်ဒယ်သည် သင့်မက်ဆေ့ခ်ျနှင့် ကိရိယာသတ်မှတ်ချက်များကို လက်ခံယူသည်။ ကိရိယာလိုအပ်ပါက စာသားဖြေကြားချက်မဟုတ်ပဲ `tool_calls` ဖြင့် တုံ့ပြန်သည် |
| **၃. ဒေသတွင် ပြုလုပ်ခြင်း** | သင့်ကုဒ်က ကိရိယာခေါ်ဆိုမှုကို ခွဲခြမ်းစိတ်ဖြာပြီး လုပ်ဆောင်ချက်ကို လည်ပတ်ကာ ရလဒ်စုဆောင်းသည် |
| **၄. နောက်ဆုံးဖြေကြားချက်** | သင့်အက်ပ်က ကိရိယာရလဒ်ကို မော်ဒယ်ထံ ပြန်ပို့ပြီး နောက်ဆုံးဖြေကြားချက် ထုတ်ပေးသည် |

> **အဓိကအချက်:** မော်ဒယ်သည် ကုဒ်မဆောင်ရွက်ပဲ *တောင်းဆိုခြင်း* ပဲ လုပ်ဆောင်သည်။ သင့်အက်ပ်လီကေးရှင်းမှာ တောင်းဆိုမှုကို လေးစားဆောင်ရွက်မလုပ်မလုပ်ဆုံးဖြတ်နိုင်သဖြင့် သင်အပြည့်အဝ ထိန်းချုပ်နိုင်ပါသည်။

---

## မည်သည့် မော်ဒယ်များက ကိရိယာခေါ်ဆိုခြင်းကို ထောက်ပံ့သနည်း?

မော်ဒယ်အားလုံးတွင် ကိရိယာခေါ်ဆိုခြင်း ထောက်ပံ့မှုမရှိပါ။ ဒါ့ကြောင့် Foundry Local ကတ်တာလော့ဂျ်တွင် tool-calling လုပ်ငန်းစဉ်ကို ထောက်ပံ့ထားသည့် မော်ဒယ်များမှာ -

| မော်ဒယ် | အရွယ်အစား | ကိရိယာခေါ်ဆိုခြင်း |
|-------|-------------|:-------------:|
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

> **အကြံပြုချက်:** ဒီလက်တွေ့သင်တန်းမှာ **qwen2.5-0.5b** ကို အသုံးပြုပါမည် - အရွယ်အသေး (822 MB ဒေါင်းလုပ်), မြန်ဆန်ပြီး tool-calling ထောက်ပံ့မှု ယုံကြည်စိတ်ချရသည်။

---

## သင်ယူရမည့် အကြောင်းအရာများ

ဒီသင်တန်းလုပြီးဆုံးတိုင်း သင်နိုင်မှာဖြစ်သည် -

- ကိရိယာခေါ်ဆိုမှု ပုံစံနှင့် AI agent များအတွက် အရေးပါချက်ကိုရှင်းပြနိုင်ခြင်း
- OpenAI function-calling ပုံစံဖြင့် ကိရိယာ schema များကို သတ်မှတ်နိုင်ခြင်း
- မျိုးစုံချိန် tool-calling ဆွေးနွေးချက်လုပ်ငန်းစဉ်ကို ကိုင်တွယ်နိုင်ခြင်း
- ဒေသ့တွင် tool calls ကို လည်ပတ်ပြီး မော်ဒယ်ထံ ရလဒ်ပေးပို့နိုင်ခြင်း
- Tool-calling အခြေအနေများအတွက် သင့်တော်သော မော်ဒယ် ရွေးချယ်နိုင်ခြင်း

---

## မလိုအပ်မီ များ

| လိုအပ်ချက် | အသေးစိတ် |
|-------------|---------|
| **Foundry Local CLI** | ထည့်သွင်းပြီး `PATH` တွင်ပါရှိ ([Part 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python၊ JavaScript သို့မဟုတ် C# SDK ထည့်သွင်းထား ([Part 2](part2-foundry-local-sdk.md)) |
| **Tool-calling မော်ဒယ်** | qwen2.5-0.5b (အလိုအလျောက် ဒေါင်းလုပ်မည်) |

---

## လေ့ကျင့်ခန်းများ

### လေ့ကျင့်ခန်း ၁ — ကိရိယာခေါ်ဆိုမှု လည်ပတ်မှုကို နားလည်ခြင်း

ကုဒ်ရေးရန်မစမီ ဒီစီကွင့်စနစ်ကို လေ့လာပါ-

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**အသေးစိတ်တွေ့ရှိချက်များ**

1. သင့်ဖော်ပြထားသော ကိရိယာများကို JSON Schema အဖြစ် ကြိုတင် သတ်မှတ်ထားသည်
2. မော်ဒယ်၏ တုံ့ပြန်ချက်တွင် `tool_calls` ပါဝင်ပြီး ပုံမှန်အကြောင်းအရာ မပါဝင်ပါ
3. ကိရိယာခေါ်ဆိုမှုတိုင်းတွင် သင့်ထောက်ပြရန် လိုအပ်သော တခုတည်းသော `id` ပါရှိသည်
4. မော်ဒယ်သည် နောက်ဆုံးဖြေကြားချက် ထုတ်စဉ် သုံးစွဲသူအကြောင်းအရာအပြင် ကိရိယာရလဒ်များအားလုံးကို မြင်ကွင်းဝင်သည်
5. တုံ့ပြန်ချက်တစ်ခုတွင် ကိရိယာခေါ်ဆိုမှု အများအပြား ဖြစ်ပေါ်နိုင်သည်

> **ဆွေးနွေးရန်:** မော်ဒယ်သည် တိုက်ရိုက်လုပ်ဆောင်ချက်များကို လုပ်ဆောင်ခြင်းမပြုဘဲ ကိရိယာခေါ်ဆိုမှု ပြန်ပေးခြင်းဖြင့် ဘာကြောင့်ရှိသနည်း? ဒါက ဘယ်လိုလုံခြုံမှု အကျိုးအမြတ်တွေ ရရှိစေသနည်း?

---

### လေ့ကျင့်ခန်း ၂ — ကိရိယာ Schema များ သတ်မှတ်ခြင်း

ကိရိယာများသည် OpenAI function-calling ပုံစံ အတိုင်း သတ်မှတ်ထားသော ဖော်မတ်ကို အသုံးပြုသည်။ ကိရိယာတစ်ခုစီသည် -

- **`type`**: အမြဲ `"function"` ဖြစ်ရမည်
- **`function.name`**: ဖော်ပြချက်ပြည့်စုံသော function အမည် (ဥပမာ `get_weather`)
- **`function.description`**: မော်ဒယ်သည် ကိရိယာကို ဘယ်အခါခေါ်သင့်သည်ကို ဆုံးဖြတ်ရာတွင် အသုံးပြုသော ဖော်ပြချက်သေချာမှု
- **`function.parameters`**: မျှော်မှန်းထားသော ပါရမီတာများကို ဖော်ပြထားသော JSON Schema object

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

> **ကိရိယာဖော်ပြချက်အတွက် အကောင်းဆုံးလုပ်ထုံးလုပ်နည်းများ:**
> - သေချာပြောဆိုပါ: "ပေးသည့်မြို့အတွက် လက်ရှိရာသီဥတုရယူခြင်း" သည် "ရာသီဥတုရယူခြင်း" ထက်ပိုသေချာသည်။
> - ပါရမီတာများကို သေချာ ဖော်ပြပါ: မော်ဒယ်သည် ဤဖော်ပြချက်များကို ဖတ်ပြီး မှန်ကန်သောတန်ဖိုးများ ဖြည့်စွက်ရန် အသုံးပြုသည်။
> - လိုအပ်သောနှင့် ရွေးချယ်နိုင်သော ပါရမီတာများကို မှတ်သားပါ - မော်ဒယ်သုံးစွဲသူကို မေးရန် ဆုံးဖြတ်ရာတွင် အထောက်အကူပြုသည်။

---

### လေ့ကျင့်ခန်း ၃ — ကိရိယာခေါ်ဆိုမှု နမူနာများ လည်ပတ်ခြင်း

ဘာသာစကားပစ္စည်းတိုင်းတွင် ကိရိယာ နှစ်ခု (`get_weather` နှင့် `get_population`) ကို သတ်မှတ်ပြီး Tool ခေါ်ခြင်း ဖြစ်စေမည့် မေးခွန်းတစ်ခု ပို့ပြီး ဒေသတွင်ကိရိယာကို ဆောင်ရွက်ပြီး နောက်ဆုံးဖြေကြားချက်အတွက် ရလဒ်ကို ပြန်ပို့သည်။

<details>
<summary><strong>🐍 Python</strong></summary>

**လိုအပ်ချက်များ:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**လည်ပတ်ရန်:**
```bash
python foundry-local-tool-calling.py
```

**မျှော်မှန်းသော output:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ကုဒ် စတင်ရှင်းလင်းချက်** (`python/foundry-local-tool-calling.py`):

```python
# ကိရိယာများကို function schemas စာရင်းအဖြစ် သတ်မှတ်ပါ
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

# ကိရိယာများနှင့်အတူ ပို့ပါ — မော်ဒယ်သည် မူလအကြောင်းအရာအစား tool_calls ကို ပြန်ပေးနိုင်သည်
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# မော်ဒယ်က ကိရိယာခေါ်မလား စစ်ဆေးပါ
if response.choices[0].message.tool_calls:
    # ကိရိယာကို အကောင်အထည်ဖေါ်ပြီး ရလဒ်ကို ပြန်ပို့ပါ
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**လိုအပ်ချက်များ:**
```bash
cd javascript
npm install
```

**လည်ပတ်ရန်:**
```bash
node foundry-local-tool-calling.mjs
```

**မျှော်မှန်းသော output:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ကုဒ် စတင်ရှင်းလင်းချက်** (`javascript/foundry-local-tool-calling.mjs`):

ဒီနမူနာတွင် OpenAI SDK မဟုတ်ဘဲ native Foundry Local SDK ရဲ့ `ChatClient` ကို အသုံးပြုထားပြီး `createChatClient()` နည်းလမ်းအား ပြသထားသည်-

```javascript
// ပုံစံအရာဝတ္ထုမှ တိုက်ရိုက် ChatClient ကို ရယူပါ
const chatClient = model.createChatClient();

// ကိရိယာများနှင့် ပို့ရန် — ChatClient သည် OpenAI-ကိုက်ညီသော ဖော်မတ်ကို ကိုင်တွယ်သည်
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// ကိရိယာ ခေါ်ဆိုမှုများကို စစ်ဆေးပါ
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // ကိရိယာများကို အကောင်အထည်ဖော်ပြီး ရလဒ်များကို ပြန်ပို့ပါ
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**လိုအပ်ချက်များ:**
```bash
cd csharp
dotnet restore
```

**လည်ပတ်ရန်:**
```bash
dotnet run toolcall
```

**မျှော်မှန်းသော output:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ကုဒ် စတင်ရှင်းလင်းချက်** (`csharp/ToolCalling.cs`):

C# တွင် ကိရိယာသတ်မှတ်ရာတွင် `ChatTool.CreateFunctionTool` helper ကို အသုံးပြုသည်-

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

### လေ့ကျင့်ခန်း ၄ — ကိရိယာခေါ်ဆိုမှု ဆွေးနွေးချက် လည်ပတ်မှု

မက်ဆေ့ချ်ဖွဲ့စည်းမှုနားလည်ခြင်းမှာ အရေးကြီးသည်။ အဆင့်များစွာတွင် `messages` array ကို ပြသထားပါသည်။

**အဆင့် ၁ — စတင် တောင်းဆိုမှု:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**အဆင့် ၂ — မော်ဒယ်သည် tool_calls ဖြင့် တုံ့ပြန်သည် (စာသား မဟုတ်ဘူး):**
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

**အဆင့် ၃ — သင်သည် assistant message နှင့် ကိရိယာရလဒ်ထည့်သည်:**
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

**အဆင့် ၄ — မော်ဒယ်သည် ကိရိယာရလဒ် အသုံးပြု၍ နောက်ဆုံး ဖြေကြားချက် ထုတ်ပေးသည်။**

> **အရေးကြီးချက်:** tool message တွင်ပါသော `tool_call_id` သည် ကိရိယာခေါ်ဆိုမှု response မှ `id` နှင့် တူညီရမည်။ ဒီနည်းနဲ့ မော်ဒယ်သည် ရလဒ်နှင့် တောင်းဆိုမှုကို ဆက်စပ်ထားသည်။

---

### လေ့ကျင့်ခန်း ၅ — ကိရိယာခေါ်ဆိုမှု အများအပြား

မော်ဒယ်တစ်ခုသည် တုံ့ပြန်မှုတစ်ခုအတွင်း ကိရိယာခေါ်ဆိုမှုများ အများအပြား တောင်းဆိုနိုင်သည်။ အသုံးပြုသူ၏ မက်ဆေ့ချ်ကို ပြောင်းလဲပြီး ခေါ်ဆိုမှုများပြုလုပ်ကြည့်ပါ -

```python
# Python တွင် — user စာတိုကိုပြောင်းပါ:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// JavaScript တွင် — အသုံးပြုသူဆုတောင်းစာကို ပြင်ဆင်ပါ ။
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

မော်ဒယ်သည် `get_weather` နှင့် `get_population` ဆိုသော ကိရိယာ `tool_calls` နှစ်ခုကို ပြန်ပေးသင့်သည်။ သင်၏ကုဒ်မှာ ကိရိယာခေါ်ဆိုမှုအားလုံးကို စီမံနိုင်သည်။

> **စမ်းသပ်ကြည့်ပါ:** အသုံးပြုသူ မက်ဆေ့ခ်ျကို ပြောင်းပြီး ဒီနမူနာကို ပြန်လည် အသုံးပြုပါ။ မော်ဒယ်က ကိရိယာနှစ်ခုစလုံးကို ခေါ်နေပါသလား?

---

### လေ့ကျင့်ခန်း ၆ — သင့်ကိုယ်ပိုင် ကိရိယာ ထပ်ထည့်ခြင်း

နမူနာတစ်ခုကို ကိရိယာအသစ်ဖြင့် တိုးချဲ့ပါ။ ဥပမာ `get_time` ကိရိယာကို ထည့်ပါ။

1. ကိရိယာ schema သတ်မှတ်ပါ:
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

2. လုပ်ဆောင်မှုပြုရန် နည်းလမ်း ထည့်ပါ:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # တကယ့်အက်ပ်မှာတော့ timezone စာကြည့်တော်ကို အသုံးပြုပါ
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... ရှိပြီးသားကိရိယာများ ...
```

3. `tools` array တွင် ကိရိယာ ထည့်ပြီး "Tokyo တွင် အချိန်ဘယ်လိုသလဲ?" ကျန်းမေးခြင်းဖြင့် စမ်းသပ်ပါ။

> **စိန်ခေါ်မှု:** ဆင့်သွယ်မှုပြောင်းလဲခြင်း ကိရိယာ တစ်ခုအသစ်ထည့်ပါ၊ ဥပမာ `convert_temperature` ဟုခေါ်ပြီး ဆဲလ်ရှီးနှင့် ဖာရင်ဟိုက် ကြား ပြောင်းလဲချက်ပြုလုပ်သည်။ "100°F ကို ဆဲလ်ရှီးသို့ ပြောင်းပေးပါ" လို့ စမ်းသပ်ပါ။

---

### လေ့ကျင့်ခန်း ၇ — SDK ရဲ့ ChatClient နှင့် ကိရိယာခေါ်ဆိုခြင်း (JavaScript)

JavaScript နမူနာသည် OpenAI SDK မဟုတ်ပဲ SDK ရဲ့ native `ChatClient` ကို အသုံးပြုထားသည်။ ၎င်းက OpenAI client ကို ကိုယ်တိုင် တည်ဆောက်ရန် မလိုအပ်စေသည့် အဆင်ပြေမှုဖြစ်သည်။

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient ကို မော်ဒယ်အရာဝတ္ထုမှ တိုက်ရိုက်ဖန်တီးထားသည်
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat သည် ဒုတိယပါရာမီတာအဖြစ် tools ကို လက်ခံသည်
const response = await chatClient.completeChat(messages, tools);
```

Python နည်းလမ်းနှင့် နှိုင်းယှဉ်ပါ၊ Python တွင် OpenAI SDK ကို တိတိကျကျ အသုံးပြုထားသည်-

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

နှစ်ခုစလုံးမှန်ကန်သော ပုံစံဖြစ်သည်။ `ChatClient` သည် ပိုမိုအဆင်ပြေသော်လည်း OpenAI SDK သည် OpenAI ရဲ့ parameter အသုံးပြုမှုအပြည့်အဝကို ထိန်းချုပ်ခွင့် ပေးသည်။

> **စမ်းကြည့်ပါ:** JavaScript နမူနာကို OpenAI SDK သို့ပြောင်းပေးပါ။ `import OpenAI from "openai"` ခေါ်ပြီး client ကို `manager.urls[0]` မှ နောက်ဆုံးဆုံးဖြတ်နောက်ခံ URL ဖြင့် တည်ဆောက်ရမည်။

---

### လေ့ကျင့်ခန်း ၈ — tool_choice နားလည်မှု

`tool_choice` ပါရာမီတာသည် မော်ဒယ်သည် ကိရိယာအသုံးပြုခြင်းကို အတောအတွင်း သတ်မှတ်ခွင့် ပေးသည်။

| တန်ဖိုး | အပြုအမူ |
|----------|-----------|
| `"auto"` | မော်ဒယ်သည် ကိရိယာခေါ်ဆိုမလား ဆုံးဖြတ်သည် (ဧ။်ယဥ်) |
| `"none"` | မော်ဒယ်သည် ကိရိယာ မခေါ်ရှိရန် |
| `"required"` | မော်ဒယ်သည် အနည်းဆုံး ကိရိယာတစ်ခုကို ခေါ်ရမည် |
| `{"type": "function", "function": {"name": "get_weather"}}` | သတ်မှတ်ထားသော ကိရိယာကို မော်ဒယ်သည် လိုက်နာခေါ်ဆိုရမည် |

Python နမူနာတွင် အစီအစဉ်တစ်ခုချင်းစီကို စမ်းသပ်ပါ-

```python
# မော်ဒယ်ကို get_weather ကိုခေါ်ရန် အတင်းအဟုတ်ပေးပါ
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **မှတ်ချက်:** မော်ဒယ်အားလုံးမှာ `tool_choice` ရွေးချယ်မှုအား တစ်ခုချင်းစီ ထောက်ပံ့ထားခြင်း မရှိနိုင်ပါ။ "required" ကို မထောက်ပံ့သော မော်ဒယ်သည် ဒီရွေးချယ်မှုကို လွဲမပေးဘဲ `auto` အတိုင်း လုပ်ဆောင်နိုင်သည်။

---

## လူသုံး ကြုံတွေ့မှုများ

| ပြဿနာ | ဖြေရှင်းနည်း |
|---------|------------|
| မော်ဒယ်က ကိရိယာ မခေါ်ပါ | tool-calling ကို ထောက်ပံ့သည့် မော်ဒယ် (ဥပမာ qwen2.5-0.5b) အသုံးပြုသည်ကို စစ်ဆေးပါ |
| `tool_call_id` မကိုက်ညီခြင်း | ကိရိယာ response မှ `id` ကို အမြဲအသုံးပြုပါ၊ စံပြအတည်မဟုတ်ပါ |
| မော်ဒယ်က `arguments` တြင် မမှန်ကန်သော JSON ပေးပို့ခြင်း | အသေးစားမော်ဒယ်များတွင် မမှန်ကန်သော JSON ပေါ်ပြဿနာရှိနိုင်သည်။ `JSON.parse()` ကို try/catch ဖြင့် ဝိုင်းဆောင်ပါ |
| မရှိသော ကိရိယာကို မော်ဒယ်က ခေါ်သုံးခြင်း | သင့် `execute_tool` လုပ်ငန်းစဉ်၌ ပုံမှန် handler ထည့်ပါ |
| ကိရိယာခေါ်ဆိုမှု loop မပြတ်နောက်ကျခြင်း | အမြင့်ဆုံး လည်ပတ်မှုအကြိမ်ရေ (ဥပမာ ၅ ကြိမ်) သတ်မှတ်၍ loop မဖြစ်စေရန် ကာကွယ်ပါ |

---

## အဓိကအချက်များ

1. **ကိရိယာခေါ်ဆိုခြင်း** သည် မော်ဒယ်များအား ဖြေကြားချက် ခန့်မှန်း ယူခြင်း မဟုတ်ဘဲ Function Execution များကို တောင်းဆိုခွင့်ပြုသည်
2. မော်ဒယ်သည် **ကုဒ် မဆောင်ရွက်ပါ** - သင့်အက်ပ်လီကေးရှင်းသည် ဘာလုပ်ဆောင်မည်ကို ဆုံးဖြတ်သည်
3. ကိရိယာများကို OpenAI function-calling ဖော်မတ်ငယ်သော **JSON Schema Object** အဖြစ် သတ်မှတ်သည်
4. ဆွေးနွေးချက်မှာ **မျိုးစုံချိန်ပုံစံ** ဖြစ်ပြီး အသုံးပြုသူ၊ assistant (tool_calls), tool (ရလဒ်များ), assistant (နောက်ဆုံးဖြေကြားချက်) အဆင့်များလိုက်ဆောင်သည်
5. ကိရိယာခေါ်ဆိုမှု ထောက်ပံ့သည့် မော်ဒယ်ကို အမြဲ သုံးစွဲပါ (Qwen 2.5, Phi-4-mini)
6. SDK ရဲ့ `createChatClient()` သည် OpenAI client တည်ဆောက်ခြင်းမလိုဘဲ ကိရိယာခေါ်ဆိုမှုများ ဆောင်ရွက်ရန် အဆင်ပြေသော နည်းလမ်းဖြစ်သည်

---

[Part 12: Zava Creative Writer အတွက် ဝဘ် UI ဖန်တီးခြင်းဆက်လက်လုပ်ဆောင်ရန်](part12-zava-ui.md) မှာ မူလတန်းမှ ရေကြောင်းအသစ်လွှတ်ပေးမှုအပြင် multi-agent pipeline အတွက် ဘရောက်ဇာအခြေခံ မျက်နှာပြင် ထည့်သွင်းရေးဆွဲနိုင်ပါသည်။

---

[← အပိုင်း ၁၀: စိတ်ကြိုက် မော်ဒယ်များ](part10-custom-models.md) | [အပိုင်း ၁၂: Zava Writer UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**အတည်မပြုချက်**:  
ဒီစာရွက်စာတမ်းကို AI ဘာသာပြန်ဝန်ဆောင်မှု [Co-op Translator](https://github.com/Azure/co-op-translator) ကို အသုံးပြု၍ ဘာသာပြန်ထားသည်။ တိကျမှုအတွက် ကြိုးပမ်းနေသော်လည်း တို автоматိတ်အိတည်ထားသော ဘာသာပြန်ချက်များတွင် မှားယွင်းချက် သို့မဟုတ် အမှားအစွန်းများ ပါဝင်နိုင်သည်ကို โปรดသတိပြုပါ။ မူလစာရွက်စာတမ်းကို မိမိဘာသာစကားဖြင့်သာ တရားဝင်အရင်းအမြစ်အဖြစ် သတ်မှတ်ရန်လိုအပ်သည်။ အရေးကြီးသော အချက်အလက်များအတွက် ကုသမား လူကြီးမင်းတို့ maîtres du လက်မှတ်ရေးကြောင်းအပြည့်အစုံ ပြုလုပ်ရန် အကြံပြုလိုသည်။ ဒီဘာသာပြန်ချက်အသုံးပြုမှုကြောင့် ဖြစ်ပေါ်နိုင်သည့် မမှန်ကန်မှု သို့မဟုတ် မမှတ်မိမှုများအတွက် ကျွန်ုပ်တို့တာဝန်မရှိပါ။
<!-- CO-OP TRANSLATOR DISCLAIMER END -->