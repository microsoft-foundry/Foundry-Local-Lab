![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# الجزء 11: استدعاء الأدوات مع النماذج المحلية

> **الهدف:** تمكين نموذجك المحلي من استدعاء الوظائف الخارجية (الأدوات) بحيث يمكنه استرجاع البيانات في الوقت الحقيقي، إجراء الحسابات، أو التفاعل مع واجهات برمجة التطبيقات — كل ذلك يعمل بشكل خاص على جهازك.

## ما هو استدعاء الأدوات؟

استدعاء الأدوات (المعروف أيضًا باسم **استدعاء الوظائف**) يتيح لنموذج اللغة طلب تنفيذ الوظائف التي تعرفها. بدلاً من التخمين بالجواب، يدرك النموذج متى يمكن للأداة أن تساعد ويُرجع طلبًا منظمًا ليتم تنفيذه بواسطة الكود الخاص بك. تقوم تطبيقك بتشغيل الوظيفة، إرسال النتيجة مرة أخرى، ويُدرج النموذج تلك المعلومات في الرد النهائي.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

هذا النمط ضروري لبناء وكلاء يمكنهم:

- **البحث عن بيانات حية** (الطقس، أسعار الأسهم، استعلامات قواعد البيانات)
- **إجراء حسابات دقيقة** (رياضيات، تحويلات وحدات)
- **اتخاذ إجراءات** (إرسال بريد إلكتروني، إنشاء تذاكر، تحديث سجلات)
- **الوصول إلى أنظمة خاصة** (واجهات برمجة تطبيقات داخلية، أنظمة ملفات)

---

## كيف يعمل استدعاء الأدوات

يتضمن تدفق استدعاء الأدوات أربع مراحل:

| المرحلة | ما يحدث |
|-------|-------------|
| **1. تعريف الأدوات** | تصف الوظائف المتاحة باستخدام مخطط JSON — الاسم، الوصف، والمعاملات |
| **2. قرار النموذج** | يتلقى النموذج رسالتك بالإضافة إلى تعريفات الأدوات. إذا كانت الأداة ستساعد، يرجع استجابة `tool_calls` بدلاً من الإجابة النصية |
| **3. التنفيذ محليًا** | يقوم كودك بتحليل استدعاء الأداة، تشغيل الوظيفة، وجمع النتيجة |
| **4. الجواب النهائي** | ترسل نتيجة الأداة مرة أخرى إلى النموذج، الذي ينتج الرد النهائي |

> **نقطة رئيسية:** النموذج لا ينفذ الكود أبداً. هو فقط *يطلب* استدعاء أداة. تطبيقك يقرر ما إذا كان سينفذ الطلب — هذا يبقيك في تحكم كامل.

---

## أي النماذج تدعم استدعاء الأدوات؟

ليس كل نموذج يدعم استدعاء الأدوات. في كتالوج Foundry Local الحالي، النماذج التالية لديها قدرة استدعاء الأدوات:

| النموذج | الحجم | استدعاء الأدوات |
|-------|------|:---:|
| qwen2.5-0.5b | 822 ميجابايت | ✅ |
| qwen2.5-1.5b | 1.8 جيجابايت | ✅ |
| qwen2.5-7b | 6.3 جيجابايت | ✅ |
| qwen2.5-14b | 11.3 جيجابايت | ✅ |
| qwen2.5-coder-0.5b | 822 ميجابايت | ✅ |
| qwen2.5-coder-1.5b | 1.8 جيجابايت | ✅ |
| qwen2.5-coder-7b | 6.3 جيجابايت | ✅ |
| qwen2.5-coder-14b | 11.3 جيجابايت | ✅ |
| phi-4-mini | 4.6 جيجابايت | ✅ |
| phi-3.5-mini | 2.6 جيجابايت | ❌ |
| phi-4 | 10.4 جيجابايت | ❌ |

> **نصيحة:** لهذا التدريب نستخدم **qwen2.5-0.5b** — فهو صغير (822 ميجابايت للتحميل)، سريع، ويدعم استدعاء الأدوات بشكل موثوق.

---

## أهداف التعلم

بحلول نهاية هذا التدريب ستكون قادرًا على:

- شرح نمط استدعاء الأدوات ولماذا هو مهم لوكلاء الذكاء الاصطناعي
- تعريف مخططات الأدوات باستخدام تنسيق استدعاء الوظائف الخاص بـ OpenAI
- التعامل مع تدفق المحادثة متعدد الأدوار لاستدعاء الأدوات
- تنفيذ استدعاءات الأدوات محليًا وإرجاع النتائج للنموذج
- اختيار النموذج المناسب لسيناريوهات استدعاء الأدوات

---

## المتطلبات السابقة

| المتطلب | التفاصيل |
|-------------|---------|
| **Foundry Local CLI** | مثبت ومضاف إلى `PATH` الخاص بك ([الجزء 1](part1-getting-started.md)) |
| **Foundry Local SDK** | SDK لـ Python أو JavaScript أو C# مثبت ([الجزء 2](part2-foundry-local-sdk.md)) |
| **نموذج يدعم استدعاء الأدوات** | qwen2.5-0.5b (سيتم تحميله تلقائيًا) |

---

## التمارين

### التمرين 1 — فهم تدفق استدعاء الأدوات

قبل كتابة الكود، ادرس مخطط التسلسل التالي:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**ملاحظات رئيسية:**

1. تعرف الأدوات مسبقًا ككائنات مخطط JSON
2. استجابة النموذج تحتوي على `tool_calls` بدل المحتوى المعتاد
3. لكل استدعاء أداة معرف `id` فريد يجب أن تشير إليه عند إرجاع النتائج
4. النموذج يرى كل الرسائل السابقة *بالإضافة إلى* نتائج الأدوات عند إنشاء الإجابة النهائية
5. يمكن أن تحدث عدة استدعاءات أدوات ضمن استجابة واحدة

> **نقاش:** لماذا يعيد النموذج استدعاءات الأدوات بدلاً من تنفيذ الوظائف مباشرة؟ ما مميزات الأمان التي يوفرها هذا؟

---

### التمرين 2 — تعريف مخططات الأدوات

يتم تعريف الأدوات باستخدام تنسيق استدعاء الوظيفة القياسي لـ OpenAI. كل أداة تحتاج إلى:

- **`type`**: دائمًا `"function"`
- **`function.name`**: اسم وظيفي وصفي (مثل `get_weather`)
- **`function.description`**: وصف واضح — يستخدمه النموذج ليقرر متى يستدعي الأداة
- **`function.parameters`**: كائن مخطط JSON يصف المعاملات المتوقعة

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

> **أفضل الممارسات لوصف الأدوات:**
> - كن محددًا: "الحصول على الطقس الحالي لمدينة معينة" أفضل من "الحصول على الطقس"
> - وصف المعاملات بوضوح: يقرأ النموذج هذه الأوصاف لملء القيم الصحيحة
> - تمييز المعاملات الإلزامية مقابل الاختيارية — هذا يساعد النموذج على معرفة ما يجب طلبه

---

### التمرين 3 — تشغيل أمثلة استدعاء الأدوات

كل نموذج لغة يعرف أداتين (`get_weather` و`get_population`)، ويرسل سؤالًا يستدعي استخدام الأدوات، ينفذ الأداة محليًا، ويرسل النتيجة مرة أخرى للحصول على إجابة نهائية.

<details>
<summary><strong>🐍 بايثون</strong></summary>

**المتطلبات:**
```bash
cd python
python -m venv venv

# ويندوز (باورشيل):
venv\Scripts\Activate.ps1
# ماك أو إس / لينكس:
source venv/bin/activate

pip install -r requirements.txt
```

**تشغيل:**
```bash
python foundry-local-tool-calling.py
```

**الناتج المتوقع:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**مراجعة الكود** (`python/foundry-local-tool-calling.py`):

```python
# عرّف الأدوات كقائمة من مخططات الوظائف
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

# أرسل مع الأدوات — قد يُرجع النموذج tool_calls بدلاً من المحتوى
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# تحقق إذا كان النموذج يريد استدعاء أداة
if response.choices[0].message.tool_calls:
    # نفّذ الأداة وأرسل النتيجة مرة أخرى
    ...
```

</details>

<details>
<summary><strong>🟨 جافاسكريبت (Node.js)</strong></summary>

**المتطلبات:**
```bash
cd javascript
npm install
```

**تشغيل:**
```bash
node foundry-local-tool-calling.mjs
```

**الناتج المتوقع:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**مراجعة الكود** (`javascript/foundry-local-tool-calling.mjs`):

يستخدم هذا المثال `ChatClient` الخاص بـ Foundry Local SDK بدلاً من OpenAI SDK، مما يوضح سهولة استخدام دالة `createChatClient()`:

```javascript
// احصل على ChatClient مباشرة من كائن النموذج
const chatClient = model.createChatClient();

// الإرسال باستخدام الأدوات — يتعامل ChatClient مع التنسيق المتوافق مع OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// التحقق من استدعاءات الأدوات
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // تنفيذ الأدوات وإرسال النتائج مرة أخرى
    ...
}
```

</details>

<details>
<summary><strong>🟦 سي شارب (.NET)</strong></summary>

**المتطلبات:**
```bash
cd csharp
dotnet restore
```

**تشغيل:**
```bash
dotnet run toolcall
```

**الناتج المتوقع:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**مراجعة الكود** (`csharp/ToolCalling.cs`):

يستخدم C# المساعد `ChatTool.CreateFunctionTool` لتعريف الأدوات:

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

### التمرين 4 — تدفق محادثة استدعاء الأدوات

فهم هيكل الرسائل أمر حاسم. هنا التدفق الكامل، يعرض مصفوفة `messages` في كل مرحلة:

**المرحلة 1 — الطلب الأولي:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**المرحلة 2 — النموذج يرد بـ `tool_calls` (ليس محتوى):**
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

**المرحلة 3 — تضيف رسالة المساعد ونتيجة الأداة:**
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

**المرحلة 4 — النموذج ينتج الإجابة النهائية باستخدام نتيجة الأداة.**

> **مهم:** يجب أن يطابق `tool_call_id` في رسالة الأداة معرف `id` من استدعاء الأداة. بهذه الطريقة يربط النموذج النتائج بالطلبات.

---

### التمرين 5 — استدعاءات أدوات متعددة

يمكن للنموذج طلب عدة استدعاءات أدوات في رد واحد. جرب تغيير رسالة المستخدم لتفعيل عدة استدعاءات:

```python
# في بايثون — قم بتغيير رسالة المستخدم:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// في جافا سكريبت — قم بتغيير رسالة المستخدم:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

يجب أن يعيد النموذج `tool_calls` اثنين — واحدة لـ `get_weather` وواحدة لـ `get_population`. كودك يتعامل مع هذا لأن لديه حلقة تمر عبر جميع استدعاءات الأدوات.

> **جربه:** غيّر رسالة المستخدم وشغل المثال مرة أخرى. هل يستدعي النموذج كلا الأداتين؟

---

### التمرين 6 — أضف أداتك الخاصة

وسع أحد الأمثلة بأداة جديدة. على سبيل المثال، أضف أداة `get_time`:

1. عرّف مخطط الأداة:
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

2. أضف منطق التنفيذ:
```python
# بايثون
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # في تطبيق حقيقي، استخدم مكتبة المنطقة الزمنية
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... الأدوات الموجودة ...
```

3. أضف الأداة إلى مصفوفة `tools` وجرب السؤال: "ما الوقت في طوكيو؟"

> **تحدي:** أضف أداة تقوم بحساب، مثل `convert_temperature` التي تحول بين سيلسيوس وفهرنهايت. جربها مع: "حوّل 100°F إلى سيلسيوس."

---

### التمرين 7 — استدعاء الأدوات مع ChatClient الخاص بـ SDK (جافاسكريبت)

يستخدم المثال في جافاسكريبت بالفعل `ChatClient` المدمج في SDK بدلاً من OpenAI SDK. هذه ميزة تسهيلية تلغي الحاجة لإنشاء عميل OpenAI بنفسك:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// يتم إنشاء ChatClient مباشرة من كائن النموذج
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// تقبل completeChat الأدوات كمعامل ثاني
const response = await chatClient.completeChat(messages, tools);
```

قارن هذا بالنهج في بايثون الذي يستخدم OpenAI SDK صراحة:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

كلا النمطين صحيحان. `ChatClient` أكثر سهولة؛ OpenAI SDK يتيح لك الوصول الكامل إلى معلمات OpenAI.

> **جرب:** غيّر مثال جافاسكريبت لاستخدام OpenAI SDK بدل `ChatClient`. ستحتاج إلى `import OpenAI from "openai"` وإنشاء العميل باستخدام نقطة النهاية من `manager.urls[0]`.

---

### التمرين 8 — فهم `tool_choice`

مُعامل `tool_choice` يتحكم فيما إذا كان النموذج *يجب* أن يستخدم أداة أو يمكنه الاختيار بحرية:

| القيمة | السلوك |
|-------|-----------|
| `"auto"` | يقرر النموذج ما إذا كان يستدعي أداة (الافتراضي) |
| `"none"` | النموذج لن يستدعي أية أدوات، حتى لو موجودة |
| `"required"` | يجب على النموذج استدعاء أداة واحدة على الأقل |
| `{"type": "function", "function": {"name": "get_weather"}}` | يجب على النموذج استدعاء الأداة المحددة |

جرّب كل خيار في مثال بايثون:

```python
# إجبار النموذج على استدعاء get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **ملاحظة:** قد لا تدعم كل النماذج كل خيارات `tool_choice`. إذا لم يدعم النموذج `"required"`, قد يتجاهل الإعداد ويتصرف كما في `"auto"`.

---

## المشاكل الشائعة

| المشكلة | الحل |
|---------|----------|
| النموذج لا يستدعي الأدوات أبدًا | تأكد من استخدامك نموذج يدعم استدعاء الأدوات (مثل qwen2.5-0.5b). راجع الجدول أعلاه. |
| عدم تطابق `tool_call_id` | استخدم دائمًا `id` من استجابة استدعاء الأداة، وليس قيمة ثابتة |
| النموذج يعيد JSON خاطئ في `arguments` | النماذج الصغيرة قد تنتج JSON غير صالح أحيانًا. غلف `JSON.parse()` بكتلة try/catch |
| النموذج يستدعي أداة غير موجودة | أضف معالجًا افتراضيًا في دالة `execute_tool` |
| حلقة استدعاء أدوات لا نهائية | حدد عدد جولات أقصى (مثلاً 5) لمنع التكرار اللانهائي |

---

## النقاط الأساسية

1. **استدعاء الأدوات** يسمح للنماذج بطلب تنفيذ الوظائف بدل التخمين
2. النموذج **لا ينفذ كودًا أبداً**؛ تطبيقك يقرر ما يتم تشغيله
3. الأدوات تُعرف ككائنات **مخطط JSON** تتبع تنسيق استدعاء الوظائف لـ OpenAI
4. المحادثة تستخدم **نمط متعدد الأدوار**: مستخدم، ثم مساعد (استدعاء أدوات)، ثم أداة (النتائج)، ثم مساعد (الجواب النهائي)
5. استخدم دائمًا **نموذج يدعم استدعاء الأدوات** (Qwen 2.5، Phi-4-mini)
6. توفر دالة `createChatClient()` في SDK طريقة سهلة لطلب استدعاء الأدوات دون الحاجة لإنشاء عميل OpenAI بنفسك

---

تابع إلى [الجزء 12: بناء واجهة ويب لكاتب زافا الإبداعي](part12-zava-ui.md) لإضافة واجهة أمامية تعتمد على المتصفح لسلسلة وكلاء متعددة مع البث في الوقت الحقيقي.

---

[← الجزء 10: النماذج المخصصة](part10-custom-models.md) | [الجزء 12: واجهة كاتب زافا →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**تنبيه**:  
تمت ترجمة هذا المستند باستخدام خدمة الترجمة بالذكاء الاصطناعي [Co-op Translator](https://github.com/Azure/co-op-translator). بينما نسعى لتحقيق الدقة، يرجى العلم أن الترجمات الآلية قد تحتوي على أخطاء أو عدم دقة. يجب اعتبار المستند الأصلي بلغته الأصلية المصدر الرسمي. بالنسبة للمعلومات الهامة، ننصح بالاعتماد على الترجمة البشرية المهنية. نحن غير مسؤولين عن أي سوء فهم أو تفسير خاطئ ينشأ من استخدام هذه الترجمة.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->