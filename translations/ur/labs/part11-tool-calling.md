![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# حصہ 11: مقامی ماڈلز کے ساتھ ٹول کالنگ

> **مقصد:** اپنے لوکل ماڈل کو خارجی فنکشنز (ٹولز) کال کرنے کے قابل بنائیں تاکہ وہ حقیقی وقت کے ڈیٹا حاصل کر سکے، حسابات انجام دے سکے، یا APIs کے ساتھ تعامل کر سکے — یہ سب آپ کے ڈیوائس پر نجی طور پر چل رہا ہو۔

## ٹول کالنگ کیا ہے؟

ٹول کالنگ (جسے **فنکشن کالنگ** بھی کہا جاتا ہے) ایک زبان کے ماڈل کو آپ کی تعریف کردہ فنکشنز کے عملدرآمد کی درخواست کرنے دیتا ہے۔ جواب کا اندازہ لگانے کی بجائے، ماڈل پہچانتا ہے کہ کب کوئی ٹول مددگار ہوگا اور ایک منظم درخواست بھیجتا ہے تاکہ آپ کا کوڈ اسے چلا سکے۔ آپ کی ایپلیکیشن فنکشن چلاتی ہے، نتیجہ واپس بھیجتی ہے، اور ماڈل اس معلومات کو اپنے حتمی جواب میں شامل کرتا ہے۔

![Tool-calling flow](../../../images/tool-calling-flow.svg)

یہ پیٹرن ایسے ایجنٹس بنانے کے لئے ضروری ہے جو:

- **زندہ ڈیٹا دیکھ سکیں** (موسم، اسٹاک کی قیمتیں، ڈیٹابیس کی تلاش)
- **صحیح حسابات کریں** (ریاضی، یونٹ کنورژن)
- **عملیات انجام دیں** (ای میل بھیجیں، ٹکٹ بنائیں، ریکارڈ اپ ڈیٹ کریں)
- **نجی نظام تک رسائی حاصل کریں** (داخلی APIs، فائل سسٹمز)

---

## ٹول کالنگ کیسے کام کرتی ہے

ٹول کالنگ کا عمل چار مراحل پر مشتمل ہے:

| مرحلہ | کیا ہوتا ہے |
|-------|-------------|
| **1. ٹولز کی تعریف کریں** | آپ JSON سکیمہ استعمال کرتے ہوئے دستیاب فنکشنز کو بیان کرتے ہیں — نام، وضاحت، اور پیرامیٹرز |
| **2. ماڈل فیصلہ کرتا ہے** | ماڈل آپ کا پیغام اور ٹول کی تعریفیں وصول کرتا ہے۔ اگر کوئی ٹول مددگار ہوگا، تو ماڈل `tool_calls` جواب دیتا ہے بجائے معمول کے متن کے |
| **3. مقامی طور پر اجرا کریں** | آپ کا کوڈ ٹول کال کو پارس کرتا ہے، فنکشن چلاتا ہے، اور نتیجہ جمع کرتا ہے |
| **4. حتمی جواب** | آپ ٹول کا نتیجہ ماڈل کو بھیجتے ہیں، جو اپنا حتمی جواب تیار کرتا ہے |

> **اہم نکتہ:** ماڈل کبھی بھی کوڈ نہیں چلاتا۔ یہ صرف *درخواست* کرتا ہے کہ کوئی ٹول کال کیا جائے۔ آپ کی ایپلیکیشن فیصلہ کرتی ہے کہ اس درخواست کو قبول کرنا ہے یا نہیں — اس طرح آپ مکمل کنٹرول میں رہتے ہیں۔

---

## کون سے ماڈلز ٹول کالنگ کو سپورٹ کرتے ہیں؟

ہر ماڈل ٹول کالنگ کو سپورٹ نہیں کرتا۔ موجودہ Foundry Local کیٹیلاگ میں مندرجہ ذیل ماڈلز ٹول کالنگ کی صلاحیت رکھتے ہیں:

| ماڈل | سائز | ٹول کالنگ |
|-------|------|:---------:|
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

> **مشورہ:** اس لیب کے لیے ہم **qwen2.5-0.5b** استعمال کرتے ہیں — یہ چھوٹا ہے (822 MB ڈاؤنلوڈ)، تیز ہے، اور اس میں قابل اعتماد ٹول کالنگ کی حمایت موجود ہے۔

---

## سیکھنے کے مقاصد

اس لیب کے اختتام تک آپ قابل ہوں گے:

- ٹول کالنگ پیٹرن اور اس کی AI ایجنٹس کے لیے اہمیت سمجھانا
- OpenAI فنکشن کالنگ فارمیٹ استعمال کرتے ہوئے ٹول سکیمہ کی تعریف کرنا
- ملٹی ٹرن ٹول کالنگ بات چیت کے فلو کو سنبھالنا
- ٹول کالز کو مقامی طور پر چلانا اور نتائج ماڈل کو واپس بھیجنا
- ٹول کالنگ کے حالات کے لیے صحیح ماڈل کا انتخاب کرنا

---

## شرائط

| ضرورت | تفصیلات |
|-------------|---------|
| **Foundry Local CLI** | انسٹال شدہ اور آپ کے `PATH` پر ([حصہ 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python، JavaScript، یا C# SDK انسٹال شدہ ([حصہ 2](part2-foundry-local-sdk.md)) |
| **ٹول کالنگ ماڈل** | qwen2.5-0.5b (خودکار طور پر ڈاؤنلوڈ ہو جائے گا) |

---

## مشقیں

### مشق 1 — ٹول کالنگ فلو کو سمجھیں

کوڈ لکھنے سے پہلے اس سیکوینس ڈایاگرام کا مطالعہ کریں:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**اہم مشاہدات:**

1. آپ ٹولز کو پہلے JSON سکیمہ آبجیکٹس کے طور پر تعریف کرتے ہیں
2. ماڈل کا جواب `tool_calls` رکھتا ہے بجائے عام مواد کے
3. ہر ٹول کال کا ایک منفرد `id` ہوتا ہے جسے آپ کو نتائج واپس کرتے وقت حوالہ دینا ہوتا ہے
4. ماڈل تمام پچھلے پیغامات *اور* ٹول کے نتائج دیکھ کر حتمی جواب تیار کرتا ہے
5. ایک ہی جواب میں متعدد ٹول کالز ہو سکتی ہیں

> **بحث:** ماڈل براہِ راست فنکشنز چلانے کی بجائے ٹول کالز کیوں لوٹاتا ہے؟ یہ کون سے سیکورٹی فوائد فراہم کرتا ہے؟

---

### مشق 2 — ٹول سکیمہ کی تعریف کریں

ٹولز کو معیاری OpenAI فنکشن کالنگ فارمیٹ کے تحت تعریف کیا جاتا ہے۔ ہر ٹول کو چاہیے:

- **`type`**: ہمیشہ `"function"`
- **`function.name`**: فنکشن کا وضاحتی نام (مثلاً `get_weather`)
- **`function.description`**: واضح وضاحت — ماڈل اسے فیصلہ کرنے کے لیے استعمال کرتا ہے کہ کب ٹول کال کرنا ہے
- **`function.parameters`**: JSON سکیمہ آبجیکٹ جو متوقع دلائل کی وضاحت کرتا ہے

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

> **ٹول وضاحتوں کے لیے بہترین عمل:**
> - مخصوص رہیں: "مخصوص شہر کا موجودہ موسم حاصل کریں" "موسم معلوم کریں" کے مقابلے میں بہتر ہے
> - پیرامیٹرز کو واضح طور پر بیان کریں: ماڈل ان وضاحتوں کو پڑھ کر درست اقدار بھرتا ہے
> - ضروری اور اختیاری پیرامیٹرز کی نشاندہی کریں — یہ ماڈل کو معلوم کرنے میں مدد دیتا ہے کہ کیا پوچھنا ہے

---

### مشق 3 — ٹول کالنگ کی مثالیں چلائیں

ہر زبان کا سیمپل دو ٹولز (`get_weather` اور `get_population`) کی تعریف کرتا ہے، ایک سوال بھیجتا ہے جو ٹول کے استعمال کو متحرک کرتا ہے، ٹول کو مقامی طور پر چلاتا ہے، اور نتیجہ واپس بھیج کر حتمی جواب حاصل کرتا ہے۔

<details>
<summary><strong>🐍 پائیتھن</strong></summary>

**شرائط:**
```bash
cd python
python -m venv venv

# ونڈوز (پاور شیل):
venv\Scripts\Activate.ps1
# میک او ایس / لینکس:
source venv/bin/activate

pip install -r requirements.txt
```

**چلائیں:**
```bash
python foundry-local-tool-calling.py
```

**متوقع نتیجہ:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**کوڈ وضاحت** (`python/foundry-local-tool-calling.py`):

```python
# ٹولز کو فنکشن اسکیموں کی فہرست کے طور پر تعریف کریں
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

# ٹولز کے ساتھ بھیجیں — ماڈل مواد کے بجائے tool_calls واپس کر سکتا ہے
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# چیک کریں کہ آیا ماڈل کوئی ٹول کال کرنا چاہتا ہے
if response.choices[0].message.tool_calls:
    # ٹول کو چلائیں اور نتیجہ واپس بھیجیں
    ...
```

</details>

<details>
<summary><strong>🟨 جاوا اسکرپٹ (Node.js)</strong></summary>

**شرائط:**
```bash
cd javascript
npm install
```

**چلائیں:**
```bash
node foundry-local-tool-calling.mjs
```

**متوقع نتیجہ:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**کوڈ وضاحت** (`javascript/foundry-local-tool-calling.mjs`):

یہ مثال OpenAI SDK کی بجائے نیٹیو Foundry Local SDK کا `ChatClient` استعمال کرتی ہے، جو `createChatClient()` طریقہ کی سہولت دکھاتی ہے:

```javascript
// ماڈل آبجیکٹ سے براہ راست ChatClient حاصل کریں
const chatClient = model.createChatClient();

// ٹولز کے ساتھ بھیجیں — ChatClient اوپن اے آئی سے مطابقت رکھنے والے فارمیٹ کو سنبھالتا ہے
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// ٹول کالز کے لیے چیک کریں
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // ٹولز کو چلائیں اور نتائج واپس بھیجیں
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**شرائط:**
```bash
cd csharp
dotnet restore
```

**چلائیں:**
```bash
dotnet run toolcall
```

**متوقع نتیجہ:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**کوڈ وضاحت** (`csharp/ToolCalling.cs`):

C# `ChatTool.CreateFunctionTool` ہیلپر استعمال کرتا ہے ٹولز کی تعریف کے لیے:

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

### مشق 4 — ٹول کالنگ بات چیت کا فلو

پیغام کے ڈھانچے کو سمجھنا بہت ضروری ہے۔ یہاں مکمل فلو ہے، ہر مرحلے پر `messages` ارے دکھایا گیا ہے:

**مرحلہ 1 — ابتدائی درخواست:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**مرحلہ 2 — ماڈل `tool_calls` کے ساتھ جواب دیتا ہے (مواد نہیں):**
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

**مرحلہ 3 — آپ اسسٹنٹ کا پیغام اور ٹول کا نتیجہ شامل کرتے ہیں:**
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

**مرحلہ 4 — ماڈل ٹول کے نتیجے کے ساتھ حتمی جواب تیار کرتا ہے۔**

> **اہم:** ٹول میسج میں `tool_call_id` کو ٹول کال کے `id` سے ملا دینا چاہیے۔ یہی ماڈل کو نتائج اور درخواستوں کا تعلق قائم کرنے دیتا ہے۔

---

### مشق 5 — متعدد ٹول کالز

ایک ماڈل ایک جواب میں کئی ٹول کالز کی درخواست کر سکتا ہے۔ یوزر پیغام تبدیل کرکے ایک سے زائد کالز متحرک کریں:

```python
# پائتھون میں — صارف کا پیغام تبدیل کریں:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// جاوا اسکرپٹ میں — صارف کا پیغام تبدیل کریں:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

ماڈل کو دو `tool_calls` لوٹانے چاہئیں — ایک `get_weather` کے لیے اور ایک `get_population` کے لیے۔ آپ کا کوڈ پہلے ہی تمام ٹول کالز پر لوپ کرتا ہے اس لیے یہ سنبھال لیتا ہے۔

> **آزمائیں:** یوزر پیغام میں ترمیم کریں اور سیمپل پھر سے چلائیں۔ کیا ماڈل دونوں ٹول کال کرتا ہے؟

---

### مشق 6 — اپنا ٹول شامل کریں

نمونوں میں سے کسی ایک میں نیا ٹول شامل کریں۔ مثلاً، `get_time` ٹول:

1. ٹول سکیمہ کی تعریف کریں:
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

2. اجرا کا منطق شامل کریں:
```python
# پائیتھون
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # ایک اصل ایپ میں، ایک ٹائم زون لائبریری استعمال کریں
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... موجودہ اوزار ...
```

3. `tools` ارے میں ٹول شامل کریں اور آزمائیں: "ٹوکیو میں کیا وقت ہے؟"

> **چیلنج:** ایسا ٹول شامل کریں جو حساب کرے، جیسے `convert_temperature` جو سیلسیس اور فارن ہائیٹ کے درمیان کنورژن کرے۔ اسے آزمائیں: "100°F کو سیلسیس میں تبدیل کریں۔"

---

### مشق 7 — SDK کے ChatClient کے ساتھ ٹول کالنگ (جاوا اسکرپٹ)

جاوا اسکرپٹ سیمپل پہلے سے SDK کے نیٹیو `ChatClient` کو استعمال کرتا ہے جو OpenAI SDK کی جگہ ہے۔ یہ ایک سہولت ہے جو OpenAI کلائنٹ بنانے کی ضرورت ختم کرتی ہے:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient براہ راست ماڈل آبجیکٹ سے بنایا جاتا ہے
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat دوسرے پیرامیٹر کے طور پر ٹولز کو قبول کرتا ہے
const response = await chatClient.completeChat(messages, tools);
```

اس کا موازنہ Python طریقہ کار سے کریں جو صراحتاً OpenAI SDK استعمال کرتا ہے:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

دونوں پیٹرنز درست ہیں۔ `ChatClient` زیادہ آسان ہے؛ OpenAI SDK آپ کو مکمل OpenAI پیرا میٹرز تک رسائی دیتا ہے۔

> **آزمائیں:** جاوا اسکرپٹ سیمپل کو بدل کر OpenAI SDK کے بجائے `ChatClient` استعمال کریں۔ آپ کو `import OpenAI from "openai"` کرنا ہوگا اور `manager.urls[0]` سے اینڈ پوائنٹ لے کر کلائنٹ بنانا ہوگا۔

---

### مشق 8 — tool_choice کو سمجھنا

`tool_choice` پیرامیٹر کنٹرول کرتا ہے کہ ماڈل *ضروری طور پر* ٹول استعمال کرے یا آزادانہ انتخاب کر سکے:

| قیمت | رویہ |
|-------|-----------|
| `"auto"` | ماڈل فیصلہ کرتا ہے کہ ٹول کال کرنی ہے یا نہیں (ڈیفالٹ) |
| `"none"` | ماڈل کسی بھی ٹول کو کال نہیں کرے گا، چاہے دستیاب ہو |
| `"required"` | ماڈل کو کم از کم ایک ٹول کال کرنا ضروری ہے |
| `{"type": "function", "function": {"name": "get_weather"}}` | ماڈل کو مخصوص ٹول کو کال کرنا لازمی ہے |

ہر آپشن کو Python سیمپل میں آزمائیں:

```python
# ماڈل کو get_weather کال کرنے پر مجبور کریں
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **نوٹ:** ہر ماڈل تمام `tool_choice` آپشنز کو سپورٹ نہیں کر سکتا۔ اگر کسی ماڈل میں `"required"` کی حمایت نہیں، تو وہ اسے نظر انداز کر کے `"auto"` کی طرح سلوک کر سکتا ہے۔

---

## عام مسائل

| مسئلہ | حل |
|---------|----------|
| ماڈل کبھی ٹولز کال نہیں کرتا | یقینی بنائیں کہ آپ ٹول کالنگ ماڈل استعمال کر رہے ہیں (مثلاً qwen2.5-0.5b). اوپر دی گئی جدول چیک کریں۔ |
| `tool_call_id` عدم مطابقت | ہمیشہ ٹول کال کے جواب میں موجود `id` استعمال کریں، کسی ہارڈ کوڈڈ ویلیو کو نہیں |
| ماڈل `arguments` میں خراب JSON دیتا ہے | چھوٹے ماڈل بعض اوقات غلط JSON بناتے ہیں۔ `JSON.parse()` کو try/catch میں لپیٹیں |
| ماڈل کسی وجود نہ رکھنے والے ٹول کو کال کرتا ہے | اپنے `execute_tool` فنکشن میں ڈیفالٹ ہینڈلر شامل کریں |
| لامتناہی ٹول کالنگ لوپ | زیادہ سے زیادہ چکر (مثلاً 5) مقرر کریں تاکہ غیر ضروری لوپس سے بچا جا سکے |

---

## اہم نکات

1. **ٹول کالنگ** ماڈلز کو جوابات کا اندازہ لگانے کی بجائے فنکشنز چلانے کی درخواست کرنے دیتی ہے
2. ماڈل **کبھی کوڈ نہیں چلائے گا**؛ آپ کی ایپلیکیشن فیصلہ کرتی ہے کہ کیا چلے
3. ٹولز کو **JSON سکیمہ** آبجیکٹس کے طور پر OpenAI فنکشن کالنگ فارمیٹ کے مطابق تعریف کیا جاتا ہے
4. بات چیت ایک **ملٹی ٹرن پیٹرن** استعمال کرتی ہے: یوزر، پھر اسسٹنٹ (tool_calls)، پھر ٹول (نتائج)، پھر اسسٹنٹ (حتمی جواب)
5. ہمیشہ ایسا **ماڈل منتخب کریں جو ٹول کالنگ سپورٹ کرتا ہو** (Qwen 2.5، Phi-4-mini)
6. SDK کا `createChatClient()` ایک آسان طریقہ فراہم کرتا ہے کہ بغیر OpenAI کلائنٹ بنائے ٹول کالنگ کی درخواست کی جا سکے

---

جاری رکھیں [حصہ 12: Zava تخلیقی رائٹر کے لیے ویب UI کی تعمیر](part12-zava-ui.md) پر تاکہ ملٹی ایجنٹ پائپ لائن کے لیے براوزر پر مبنی فرنٹ اینڈ اصل وقت کی اسٹریمنگ کے ساتھ شامل کیا جا سکے۔

---

[← حصہ 10: حسب ضرورت ماڈلز](part10-custom-models.md) | [حصہ 12: Zava Writer UI →](part12-zava-ui.md)