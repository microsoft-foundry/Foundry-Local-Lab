![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# অংশ ১১: লোকাল মডেল দিয়ে টুল কলিং

> **লক্ষ্য:** আপনার লোকাল মডেলকে বাইরের ফাংশন (টুল) কল করার ক্ষমতা দিন যাতে এটি রিয়েল-টাইম ডেটা সংগ্রহ করতে পারে, হিসাব করতে পারে, অথবা API-এর সাথে ইন্টারঅ্যাক্ট করতে পারে — সবই সম্পূর্ণ আপনার ডিভাইসে প্রাইভেটলি চলবে।

## টুল কলিং কী?

টুল কলিং (যা **ফাংশন কলিং** নামেও পরিচিত) একটি ভাষা মডেলকে আপনার দ্বারা সংজ্ঞায়িত ফাংশনগুলো চালানোর জন্য অনুরোধ করার সুযোগ দেয়। উত্তরের জন্য অনুমান করার পরিবর্তে, মডেল বুঝতে পারে কখন একটি টুল সাহায্য করবে এবং আপনার কোডকে চালানোর জন্য একটি স্ট্রাকচার্ড অনুরোধ ফেরত দেয়। আপনার অ্যাপ্লিকেশন ফাংশনটি চালায়, ফলাফল পাঠায়, এবং মডেল সেই তথ্যকে তার চূড়ান্ত উত্তরে অন্তর্ভুক্ত করে।

![Tool-calling flow](../../../images/tool-calling-flow.svg)

এই প্যাটার্নটি এমন এজেন্ট তৈরিতে অপরিহার্য যেগুলো করতে পারে:

- **লাইভ ডেটা সন্ধান** (আবহাওয়া, স্টক দাম, ডাটাবেস ক্যোয়ারি)
- **নির্ভুল হিসাব করা** (গণিত, একক রূপান্তর)
- **কর্ম গ্রহণ** (ইমেইল পাঠানো, টিকিট তৈরি, রেকর্ড আপডেট)
- **প্রাইভেট সিস্টেমে প্রবেশ** (অভ্যন্তরীণ API, ফাইল সিস্টেম)

---

## টুল কলিং কীভাবে কাজ করে

টুল কলিং ফ্লো চারটি ধাপে সম্পন্ন হয়:

| ধাপ | কী ঘটে |
|-------|-------------|
| **১. টুলস সংজ্ঞায়িত করুন** | আপনি JSON Schema ব্যবহার করে উপলব্ধ ফাংশনগুলো বর্ণনা করেন — নাম, বিবরণ, এবং প্যারামিটার |
| **২. মডেল সিদ্ধান্ত নেয়** | মডেল আপনার মেসেজ এবং টুল ডেফিনিশন পায়। যদি একটি টুল সাহায্য করে, তখন এটি টেক্সট উত্তরের বদলে একটি `tool_calls` রেসপন্স দেয় |
| **৩. লোকালি চালানো** | আপনার কোড টুল কল পার্স করে, ফাংশন চালায় এবং ফলাফল সংগ্রহ করে |
| **৪. চূড়ান্ত উত্তর** | আপনি ফলাফল মডেলের কাছে পাঠান এবং মডেল চূড়ান্ত উত্তর দেয় |

> **মূল কথা:** মডেল কখনো কোড এক্সিকিউট করে না। এটি শুধুমাত্র *অনুরোধ* করে যে একটি টুল কল করা হোক। আপনি সিদ্ধান্ত নেন সেই অনুরোধ পূরণ করতে — এতে আপনি সম্পূর্ণ নিয়ন্ত্রণে থাকেন।

---

## কোন মডেলগুলো টুল কলিং সমর্থন করে?

প্রতিটি মডেল টুল কলিং সমর্থন করে না। বর্তমান Foundry Local ক্যাটালগে, নিম্নলিখিত মডেলগুলো টুল কলিং সক্ষমতা রাখে:

| মডেল | সাইজ | টুল কলিং |
|-------|------|:---:|
| qwen2.5-0.5b | ৮২২ এমবি | ✅ |
| qwen2.5-1.5b | ১.৮ জিবি | ✅ |
| qwen2.5-7b | ৬.৩ জিবি | ✅ |
| qwen2.5-14b | ১১.৩ জিবি | ✅ |
| qwen2.5-coder-0.5b | ৮২২ এমবি | ✅ |
| qwen2.5-coder-1.5b | ১.৮ জিবি | ✅ |
| qwen2.5-coder-7b | ৬.৩ জিবি | ✅ |
| qwen2.5-coder-14b | ১১.৩ জিবি | ✅ |
| phi-4-mini | ৪.৬ জিবি | ✅ |
| phi-3.5-mini | ২.৬ জিবি | ❌ |
| phi-4 | ১০.৪ জিবি | ❌ |

> **টিপ:** এই ল্যাবে আমরা **qwen2.5-0.5b** ব্যবহার করি — এটি ছোট (৮২২ এমবি ডাউনলোড), দ্রুত এবং নির্ভরযোগ্য টুল কলিং সাপোর্ট আছে।

---

## শেখার উদ্দেশ্যসমূহ

এই ল্যাব শেষ করার পর আপনি পারবেন:

- টুল কলিং প্যাটার্ন ব্যাখ্যা করা এবং কেন এটি এআই এজেন্টদের জন্য গুরুত্বপূর্ণ
- OpenAI function-calling ফরম্যাট ব্যবহার করে টুল স্কিমা সংজ্ঞায়িত করা
- মাল্টি-টার্ন টুল কলিং কথোপকথন ফ্লো পরিচালনা করা
- টুল কল লোকালি এক্সিকিউট করা এবং ফলাফল মডেলে ফেরত পাঠানো
- টুল-কলিং পরিস্থিতির জন্য সঠিক মডেল নির্বাচন করা

---

## প্রয়োজনীয়তা

| প্রয়োজনীয়তা | বিবরণ |
|-------------|---------|
| **Foundry Local CLI** | ইনস্টল করা এবং আপনার `PATH`-এ থাকা ([অংশ ১](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript, অথবা C# SDK ইনস্টল ([অংশ ২](part2-foundry-local-sdk.md)) |
| **একটি টুল-কলিং মডেল** | qwen2.5-0.5b (স্বয়ংক্রিয়ভাবে ডাউনলোড হবে) |

---

## অনুশীলনসমূহ

### অনুশীলন ১ — টুল কলিং ফ্লো বোঝা

কোড লেখার আগে এই সিকোয়েন্স ডায়াগ্রামটি অধ্যয়ন করুন:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**প্রধান পর্যবেক্ষণ:**

1. আপনি আগেই টুলগুলো JSON Schema অবজেক্ট হিসেবে সংজ্ঞায়িত করেন
2. মডেলের রেসপন্সে `tool_calls` থাকে, নিয়মিত কন্টেন্টের বদলে
3. প্রতিটি টুল কলের একটি ইউনিক `id` থাকে যেটির উল্লেখ করতে হবে ফলাফল ফেরত দেয়ার সময়
4. মডেল পূর্ববর্তী সব মেসেজ *সাথে* টুলের ফলাফল দেখে চূড়ান্ত উত্তর গঠন করে
5. একাধিক টুল কল একক রেসপন্সে হতে পারে

> **আলোচনা:** মডেল সরাসরি ফাংশন চালানোর পরিবর্তে টুল কল কেন ফেরত দেয়? এর নিরাপত্তাগত সুবিধাগুলো কী কী?

---

### অনুশীলন ২ — টুল স্কিমা সংজ্ঞায়িত করা

টুলগুলো OpenAI function-calling ফরম্যাট অনুসারে সংজ্ঞায়িত করতে হয়। প্রতিটির জন্য প্রয়োজন:

- **`type`**: সর্বদা `"function"`
- **`function.name`**: একটি বর্ণনামূলক ফাংশন নাম (যেমন `get_weather`)
- **`function.description`**: স্পষ্ট বর্ণনা — মডেল এটা দেখে কখন টুল কল করতে হবে তা সিদ্ধান্ত নেয়
- **`function.parameters`**: JSON Schema অবজেক্ট যা প্রত্যাশিত আর্গুমেন্ট বর্ণনা করে

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

> **টুল বর্ণনার সেরা অভ্যাস:**
> - নির্দিষ্ট থাকুন: "নির্দিষ্ট শহরের বর্তমান আবহাওয়া পাওয়া" ভাল, "আবহাওয়া পাওয়া" থেকে
> - প্যারামিটার পরিষ্কারভাবে বর্ণনা করুন: মডেল এগুলো পড়ে সঠিক মান পূরণ করে
> - প্রয়োজনীয় বনাম ঐচ্ছিক প্যারামিটার চিহ্নিত করুন — এটা মডেলকে সিদ্ধান্ত নিতে সাহায্য করে কী জিজ্ঞাসা করতে হবে

---

### অনুশীলন ৩ — টুল কলিং উদাহরণ চালানো

প্রতিটি ভাষার স্যাম্পল দুটি টুল (`get_weather` এবং `get_population`) সংজ্ঞায়িত করে, একটি প্রশ্ন পাঠায় যা টুল ব্যবহার ট্রিগার করে, টুল লোকালি চালায়, এবং চূড়ান্ত উত্তর ফেরত দেয়।

<details>
<summary><strong>🐍 পাইথন</strong></summary>

**প্রয়োজনীয়তা:**
```bash
cd python
python -m venv venv

# উইন্ডোজ (পাওয়ারশেল):
venv\Scripts\Activate.ps1
# ম্যাকওএস / লিনাক্স:
source venv/bin/activate

pip install -r requirements.txt
```

**চালান:**
```bash
python foundry-local-tool-calling.py
```

**প্রত্যাশিত আউটপুট:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**কোড Walkthrough** (`python/foundry-local-tool-calling.py`):

```python
# টুলসকে একটি ফাংশন স্কিমার তালিকা হিসেবে নির্ধারণ করুন
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

# টুলসসহ প্রেরণ করুন — মডেলটি কন্টেন্টের পরিবর্তে tool_calls ফেরত দিতে পারে
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# মডেল কি একটি টুল কল করতে চায় তা পরীক্ষা করুন
if response.choices[0].message.tool_calls:
    # টুলটি সম্পাদন করুন এবং ফলাফল ফেরত পাঠান
    ...
```

</details>

<details>
<summary><strong>🟨 জাভাস্ক্রিপ্ট (Node.js)</strong></summary>

**প্রয়োজনীয়তা:**
```bash
cd javascript
npm install
```

**চালান:**
```bash
node foundry-local-tool-calling.mjs
```

**প্রত্যাশিত আউটপুট:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**কোড Walkthrough** (`javascript/foundry-local-tool-calling.mjs`):

এই উদাহরণে OpenAI SDK এর পরিবর্তে নেটিভ Foundry Local SDK এর `ChatClient` ব্যবহার করা হয়েছে, যা `createChatClient()` পদ্ধতির সুবিধা দেখায়:

```javascript
// মডেল অবজেক্ট থেকে সরাসরি একটি ChatClient পান
const chatClient = model.createChatClient();

// টুলস দিয়ে পাঠান — ChatClient OpenAI-সঙ্গত ফর্ম্যাট পরিচালনা করে
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// টুল কলের জন্য পরীক্ষা করুন
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // টুলস চালান এবং ফলাফল ফেরত পাঠান
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**প্রয়োজনীয়তা:**
```bash
cd csharp
dotnet restore
```

**চালান:**
```bash
dotnet run toolcall
```

**প্রত্যাশিত আউটপুট:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**কোড Walkthrough** (`csharp/ToolCalling.cs`):

C# এ `ChatTool.CreateFunctionTool` হেল্পার দিয়ে টুলগুলো সংজ্ঞায়িত করা হয়:

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

### অনুশীলন ৪ — টুল কলিং কথোপকথন ফ্লো

মেসেজ স্ট্রাকচার বোঝা গুরুত্বপূর্ণ। এখানে পুরো ফ্লো, প্রতিটি ধাপে `messages` অ্যারে দেখানো হলো:

**ধাপ ১ — প্রাথমিক অনুরোধ:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**ধাপ ২ — মডেল `tool_calls` সহ উত্তর দেয় (কন্টেন্ট নয়):**
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

**ধাপ ৩ — আপনি অ্যাসিস্ট্যান্ট মেসেজ এবং টুল ফলাফল যুক্ত করেন:**
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

**ধাপ ৪ — মডেল টুল ফলাফল ব্যবহার করে চূড়ান্ত উত্তর তৈরি করে।**

> **গুরুত্বপূর্ণ:** টুল মেসেজে `tool_call_id` অবশ্যই টুল কলের `id` এর সাথে মেলে। এভাবেই মডেল ফলাফলগুলোকে অনুরোধের সাথে সংযুক্ত করে।

---

### অনুশীলন ৫ — একাধিক টুল কল

একটি মডেল একক রেসপন্সে একাধিক টুল কলের অনুরোধ করতে পারে। ব্যবহারকারী মেসেজ বদলিয়ে একাধিক কল ট্রিগার করার চেষ্টা করুন:

```python
# পাইথন-এ — ব্যবহারকারীর বার্তা পরিবর্তন করুন:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// জাভাস্ক্রিপ্টে — ব্যবহারকারীর বার্তা পরিবর্তন করুন:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

মডেল দুইটি `tool_calls` ফেরত দেবে — একটি `get_weather` এবং একটি `get_population` এর জন্য। আপনার কোড ইতিমধ্যেই সব টুল কল লুপ করে হ্যান্ডেল করে।

> **চেষ্টা করুন:** ব্যবহারকারী মেসেজ পরিবর্তন করে আবার রান দিন। মডেল কি দুই টুলই কল করে?

---

### অনুশীলন ৬ — নিজের টুল যোগ করা

একটি স্যাম্পলে নতুন টুল যোগ করুন। উদাহরণস্বরূপ, একটি `get_time` টুল যোগ করুন:

১. টুল স্কিমা সংজ্ঞায়িত করুন:
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

২. এক্সিকিউশন লজিক যোগ করুন:
```python
# পাইথন
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # একটি বাস্তব অ্যাপে, একটি টাইমজোন লাইব্রেরি ব্যবহার করুন
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... বিদ্যমান সরঞ্জাম ...
```

৩. টুলটি `tools` অ্যারেতে যোগ করুন এবং পরীক্ষার জন্য বলুন: "টোকিওতে এখন কয়টা সময়?"

> **চ্যালেঞ্জ:** এমন একটি টুল যোগ করুন যা হিসাব করে, যেমন `convert_temperature` যা সেলসিয়াস এবং ফারেনহাইট রূপান্তর করে। টেস্ট করুন: "১০০°F কে সেলসিয়াসে রূপান্তর করুন।"

---

### অনুশীলন ৭ — SDK এর ChatClient দিয়ে টুল কলিং (জাভাস্ক্রিপ্ট)

জাভাস্ক্রিপ্ট স্যাম্পল যথাক্রমে OpenAI SDK এর পরিবর্তে SDK এর নেটিভ `ChatClient` ব্যবহার করে। এটি একটি সুবিধাজনক ফিচার যেটি OpenAI ক্লায়েন্ট নিজে তৈরি করার দরকার অপসারণ করে:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient সরাসরি মডেল অবজেক্ট থেকে তৈরি করা হয়েছে
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat টুলসকে দ্বিতীয় প্যারামিটার হিসেবে গ্রহণ করে
const response = await chatClient.completeChat(messages, tools);
```

Python পদ্ধতির সাথে তুলনা করুন যা স্পষ্টভাবে OpenAI SDK ব্যবহার করে:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

উভয় প্যাটার্নই বৈধ। `ChatClient` বেশি সুবিধাজনক; OpenAI SDK আপনাকে OpenAI এর সব প্যারামিটারে প্রবেশাধিকার দেয়।

> **চেষ্টা করুন:** JavaScript স্যাম্পলটি OpenAI SDK ব্যবহার করতে পরিবর্তন করুন `ChatClient` এর পরিবর্তে। আপনার `import OpenAI from "openai"` দরকার হবে এবং `manager.urls[0]` থেকে এন্ডপয়েন্ট নিয়ে ক্লায়েন্ট নির্মাণ করতে হবে।

---

### অনুশীলন ৮ — tool_choice বোঝা

`tool_choice` প্যারামিটার কন্ট্রোল করে মডেল *অবশ্যই* টুল ব্যবহার করবে কিনা অথবা স্বাধীনভাবে নির্বাচন করতে পারে:

| মান | আচরণ |
|-------|-----------|
| `"auto"` | মডেল সিদ্ধান্ত নেয় টুল কল করবে কিনা (ডিফল্ট) |
| `"none"` | মডেল কোনো টুল কল করবে না, যদিও সরবরাহ করা হয় |
| `"required"` | মডেল অন্তত একটি টুল কল করতে বাধ্য |
| `{"type": "function", "function": {"name": "get_weather"}}` | মডেল অবশ্যই নির্দিষ্ট টুল কল করবে |

Python স্যাম্পলে প্রতিটি অপশন চেষ্টা করুন:

```python
# মডেলকে get_weather কল করতে বাধ্য করুন
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **দ্রষ্টব্য:** সব মডেল সম্ভবত সমস্ত `tool_choice` অপশন সমর্থন নাও করতে পারে। যদি একটি মডেল `"required"` সমর্থন করে না, তাহলে সেটি সেটিং উপেক্ষা করে `"auto"` এর মতো আচরণ করতে পারে।

---

## সাধারণ সমস্যা ও সমাধান

| সমস্যা | সমাধান |
|---------|----------|
| মডেল কখনো টুল কল করে না | নিশ্চিত করুন আপনি টুল-কলিং মডেল ব্যবহার করছেন (যেমন qwen2.5-0.5b)। উপরের টেবিল চেক করুন। |
| `tool_call_id` মিলছে না | সর্বদা টুল কল রেসপন্স থেকে `id` ব্যবহার করুন, হার্ডকোডেড মান নয় |
| মডেল `arguments` এ খারাপ JSON ফেরত দেয় | ছোট মডেল মাঝে মাঝে ভুল JSON তৈরি করে। `JSON.parse()` কে try/catch ব্লকে রাখুন |
| মডেল এমন একটি টুল কল করে যা নেই | আপনার `execute_tool` ফাংশনে ডিফল্ট হ্যান্ডলার রাখুন |
| অসীম টুল কলিং লুপ | সর্বোচ্চ রাউন্ড সংখ্যা সেট করুন (যেমন ৫) যাতে লুপ রোধ হয় |

---

## মূল বিষয়গুলো

1. **টুল কলিং** মডেলগুলোকে অনুমতি দেয় ফাংশন এক্সিকিউট করার অনুরোধ করতে, অনুমান করার বদলে
2. মডেল **কখনো কোড চালায় না**; আপনার অ্যাপ্লিকেশন চলানোর সিদ্ধান্ত নেয়
3. টুলগুলো **JSON Schema** অবজেক্ট হিসেবে সংজ্ঞায়িত হয় OpenAI function-calling ফরম্যাটে
4. কথোপকথন একটি **মাল্টি-টার্ন প্যাটার্ন** অনুসরণ করে: ব্যবহারকারী, তারপর অ্যাসিস্ট্যান্ট (tool_calls), তারপর টুল (ফলাফল), তারপর অ্যাসিস্ট্যান্ট (চূড়ান্ত উত্তর)
5. সর্বদা **টুল কলিং সমর্থন করে এমন মডেল** ব্যবহার করুন (Qwen 2.5, Phi-4-mini)
6. SDK এর `createChatClient()` একটি সুবিধাজনক উপায় প্রদান করে OpenAI ক্লায়েন্ট তৈরি না করেও টুল কলিং রিকোয়েস্ট করার

---

চালিয়ে যান [অংশ ১২: Zava ক্রিয়েটিভ রাইটারের জন্য ওয়েব UI তৈরি](part12-zava-ui.md) এ, যেখানে মাল্টি-এজেন্ট পাইপলাইনের জন্য ব্রাউজার-ভিত্তিক ফ্রন্ট এন্ড যুক্ত করবেন রিয়েল-টাইম স্ট্রিমিং সহ।

---

[← অংশ ১০: কাস্টম মডেল](part10-custom-models.md) | [অংশ ১২: Zava Writer UI →](part12-zava-ui.md)