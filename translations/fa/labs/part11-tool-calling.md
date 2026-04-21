![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# بخش ۱۱: فراخوانی ابزار با مدل‌های محلی

> **هدف:** این‌که مدل محلی شما قادر باشد توابع خارجی (ابزارها) را فراخوانی کند تا بتواند داده‌های زنده را واکشی کند، محاسبات انجام دهد یا با APIها تعامل داشته باشد — همگی به صورت خصوصی روی دستگاه شما اجرا می‌شوند.

## فراخوانی ابزار چیست؟

فراخوانی ابزار (که با نام **فراخوانی توابع** نیز شناخته می‌شود) به مدل زبان اجازه می‌دهد اجرای توابع تعریف‌شده توسط شما را درخواست کند. به جای حدس زدن پاسخ، مدل تشخیص می‌دهد چه زمانی ابزار مفید است و درخواست ساختارمندی برای اجرای کد شما بازمی‌گرداند. برنامه شما تابع را اجرا می‌کند، نتیجه را ارسال می‌کند و مدل آن اطلاعات را در پاسخ نهایی خود لحاظ می‌کند.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

این الگو برای ساخت عامل‌هایی که می‌توانند:

- **داده‌های زنده را جستجو کنند** (آب‌وهوا، قیمت سهام، پرس‌وجوهای پایگاه داده)
- **محاسبات دقیق انجام دهند** (ریاضی، تبدیل واحدها)
- **اقدامات انجام دهند** (ارسال ایمیل، ایجاد تیکت، به‌روزرسانی سوابق)
- **به سیستم‌های خصوصی دسترسی داشته باشند** (APIهای داخلی، سامانه‌های فایل)

ضروری است.

---

## چگونه فراخوانی ابزار کار می‌کند

جریان فراخوانی ابزار چهار مرحله دارد:

| مرحله | چه اتفاقی می‌افتد |
|-------|------------------|
| **۱. تعریف ابزارها** | شما توابع موجود را با استفاده از JSON Schema توصیف می‌کنید — نام، توضیح و پارامترها |
| **۲. تصمیم مدل** | مدل پیام شما به‌علاوه تعریف ابزارها را دریافت می‌کند. اگر ابزاری کمک‌کننده باشد، به جای پاسخ متنی، پاسخ `tool_calls` بازمی‌گرداند |
| **۳. اجرای محلی** | کد شما فراخوانی ابزار را تجزیه می‌کند، تابع را اجرا می‌کند و نتیجه را جمع‌آوری می‌کند |
| **۴. پاسخ نهایی** | شما نتیجه ابزار را به مدل ارسال می‌کنید، که پاسخ نهایی را تولید می‌کند |

> **نکته کلیدی:** مدل هرگز کد را اجرا نمی‌کند. فقط *درخواست* می‌کند که ابزار فراخوانی شود. برنامه شما تعیین می‌کند که آیا این درخواست را بپذیرد — این موضوع شما را در کنترل کامل نگه می‌دارد.

---

## کدام مدل‌ها از فراخوانی ابزار پشتیبانی می‌کنند؟

هر مدلی از فراخوانی ابزار پشتیبانی نمی‌کند. در فهرست فعلی Foundry Local، مدل‌های زیر قابلیت فراخوانی ابزار را دارند:

| مدل | اندازه | فراخوانی ابزار |
|-------|---------|:--------------:|
| qwen2.5-0.5b | ۸۲۲ مگابایت | ✅ |
| qwen2.5-1.5b | ۱.۸ گیگابایت | ✅ |
| qwen2.5-7b | ۶.۳ گیگابایت | ✅ |
| qwen2.5-14b | ۱۱.۳ گیگابایت | ✅ |
| qwen2.5-coder-0.5b | ۸۲۲ مگابایت | ✅ |
| qwen2.5-coder-1.5b | ۱.۸ گیگابایت | ✅ |
| qwen2.5-coder-7b | ۶.۳ گیگابایت | ✅ |
| qwen2.5-coder-14b | ۱۱.۳ گیگابایت | ✅ |
| phi-4-mini | ۴.۶ گیگابایت | ✅ |
| phi-3.5-mini | ۲.۶ گیگابایت | ❌ |
| phi-4 | ۱۰.۴ گیگابایت | ❌ |

> **نکته:** برای این آزمایش ما از **qwen2.5-0.5b** استفاده می‌کنیم — اندازه کوچکی دارد (۸۲۲ مگابایت برای دانلود)، سریع است و پشتیبانی قابل اعتماد فراخوانی ابزار دارد.

---

## اهداف آموزشی

تا پایان این آزمایش قادر خواهید بود:

- الگوی فراخوانی ابزار و اهمیت آن برای عامل‌های هوش مصنوعی را توضیح دهید
- اسکیمای ابزارها را با استفاده از فرمت فراخوانی توابع OpenAI تعریف کنید
- جریان گفتگوهای چندنوبتی فراخوانی ابزار را مدیریت کنید
- فراخوانی ابزار را به صورت محلی اجرا و نتیجه را به مدل بازگردانید
- مدل مناسب برای سناریوهای فراخوانی ابزار را انتخاب کنید

---

## پیش‌نیازها

| نیازمندی | جزئیات |
|-----------|---------|
| **Foundry Local CLI** | نصب شده و در مسیر `PATH` شما باشد ([بخش ۱](part1-getting-started.md)) |
| **Foundry Local SDK** | نسخه پایتون، جاوااسکریپت یا C# نصب شده ([بخش ۲](part2-foundry-local-sdk.md)) |
| **یک مدل با قابلیت فراخوانی ابزار** | qwen2.5-0.5b (به‌صورت خودکار دانلود می‌شود) |

---

## تمرین‌ها

### تمرین ۱ — درک جریان فراخوانی ابزار

قبل از نوشتن کد، این نمودار توالی را مطالعه کنید:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**مشاهدات کلیدی:**

۱. شما ابزارها را از ابتدا به صورت اشیاء JSON Schema تعریف می‌کنید  
۲. پاسخ مدل شامل `tool_calls` به جای محتوای عادی است  
۳. هر فراخوانی ابزار یک شناسه (`id`) یکتا دارد که باید هنگام بازگرداندن نتایج به آن ارجاع دهید  
۴. مدل هنگام تولید پاسخ نهایی همه پیام‌های قبلی *به‌علاوه* نتایج ابزار را می‌بیند  
۵. چندین فراخوانی ابزار می‌تواند در یک پاسخ اتفاق بیفتد

> **بحث:** چرا مدل به جای اجرای توابع مستقیما، فراخوانی ابزارها را بازمی‌گرداند؟ چه مزایای امنیتی دارد؟

---

### تمرین ۲ — تعریف اسکیمای ابزار

ابزارها با استفاده از فرمت استاندارد فراخوانی توابع OpenAI تعریف می‌شوند. هر ابزار به موارد زیر نیاز دارد:

- **`type`**: همیشه `"function"`  
- **`function.name`**: نام توصیفی تابع (مثلا `get_weather`)  
- **`function.description`**: توضیح واضح — مدل برای تصمیم‌گیری فراخوانی ابزار از آن استفاده می‌کند  
- **`function.parameters`**: یک شی JSON Schema که پارامترهای مورد انتظار را توصیف می‌کند  

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

> **بهترین شیوه‌ها برای توضیحات ابزار:**  
> - مشخص باشید: "دریافت وضعیت فعلی آب‌وهوا برای یک شهر" بهتر از "دریافت آب‌وهوا" است  
> - پارامترها را واضح توصیف کنید: مدل این توضیحات را برای پر کردن مقادیر به‌کار می‌برد  
> - پارامترهای اجباری و اختیاری را مشخص کنید — این به مدل کمک می‌کند تصمیم بگیرد چه بپرسد

---

### تمرین ۳ — اجرای مثال‌های فراخوانی ابزار

هر نمونه زبان دو ابزار (`get_weather` و `get_population`) تعریف می‌کند، سوالی می‌فرستد که استفاده از ابزار را به‌کار می‌اندازد، ابزار را محلی اجرا کرده و نتیجه را برای پاسخ نهایی می‌فرستد.

<details>
<summary><strong>🐍 پایتون</strong></summary>

**پیش‌نیازها:**  
```bash
cd python
python -m venv venv

# ویندوز (پاورشل):
venv\Scripts\Activate.ps1
# مک‌اواس / لینوکس:
source venv/bin/activate

pip install -r requirements.txt
```
  
**اجرا:**  
```bash
python foundry-local-tool-calling.py
```
  
**خروجی مورد انتظار:**  
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**شرح کد** (`python/foundry-local-tool-calling.py`):

```python
# ابزارها را به عنوان یک لیست از طرح‌واره‌های تابع تعریف کنید
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

# ارسال به همراه ابزارها — مدل ممکن است به جای محتوا، فراخوانی ابزارها را برگرداند
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# بررسی کنید که آیا مدل می‌خواهد از یک ابزار استفاده کند
if response.choices[0].message.tool_calls:
    # ابزار را اجرا کرده و نتیجه را بازگردانید
    ...
```

</details>

<details>
<summary><strong>🟨 جاوااسکریپت (Node.js)</strong></summary>

**پیش‌نیازها:**  
```bash
cd javascript
npm install
```
  
**اجرا:**  
```bash
node foundry-local-tool-calling.mjs
```
  
**خروجی مورد انتظار:**  
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**شرح کد** (`javascript/foundry-local-tool-calling.mjs`):

این مثال از `ChatClient` بومی Foundry Local SDK به جای OpenAI SDK استفاده می‌کند، که متد راحت `createChatClient()` را نشان می‌دهد:

```javascript
// مستقیماً یک ChatClient از شی مدل دریافت کنید
const chatClient = model.createChatClient();

// ارسال با ابزارها — ChatClient قالب سازگار با OpenAI را مدیریت می‌کند
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// بررسی تماس‌های ابزار
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // اجرای ابزارها و ارسال نتایج بازگشتی
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**پیش‌نیازها:**  
```bash
cd csharp
dotnet restore
```
  
**اجرا:**  
```bash
dotnet run toolcall
```
  
**خروجی مورد انتظار:**  
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```
  
**شرح کد** (`csharp/ToolCalling.cs`):

در C# از کمک‌تابع `ChatTool.CreateFunctionTool` برای تعریف ابزارها استفاده می‌شود:

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

### تمرین ۴ — جریان گفتگو در فراخوانی ابزار

درک ساختار پیام بسیار مهم است. در اینجا جریان کامل را می‌بینید، که آرایه `messages` در هر مرحله را نمایش می‌دهد:

**مرحله ۱ — درخواست اولیه:**  
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```
  
**مرحله ۲ — مدل پاسخ با `tool_calls` (نه محتوا):**  
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
  
**مرحله ۳ — شما پیام دستیار و نتیجه ابزار را اضافه می‌کنید:**  
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
  
**مرحله ۴ — مدل پاسخ نهایی را با استفاده از نتیجه ابزار تولید می‌کند.**

> **مهم:** `tool_call_id` در پیام ابزار باید با `id` فراخوانی ابزار مطابقت داشته باشد. این روش مدل برای پیوند نتایج با درخواست‌ها است.

---

### تمرین ۵ — چندین فراخوانی ابزار

یک مدل می‌تواند چندین فراخوانی ابزار را در یک پاسخ درخواست کند. پیام کاربر را تغییر دهید تا چندین فراخوانی ایجاد شود:

```python
# در پایتون — پیام کاربر را تغییر دهید:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```
  
```javascript
// در جاوااسکریپت — پیام کاربر را تغییر دهید:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```
  
مدل باید دو `tool_calls` بازگرداند — یکی برای `get_weather` و دیگری برای `get_population`. کد شما این مورد را می‌تواند مدیریت کند چون روی همه فراخوانی‌ها حلقه می‌زند.

> **امتحان کنید:** پیام کاربر را تغییر داده و نمونه را دوباره اجرا کنید. آیا مدل هر دو ابزار را فراخوانی می‌کند؟

---

### تمرین ۶ — افزودن ابزار خودتان

یکی از نمونه‌ها را با یک ابزار جدید گسترش دهید. مثلا افزودن ابزار `get_time`:

۱. اسکمای ابزار را تعریف کنید:  
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
  
۲. منطق اجرای آن را اضافه کنید:  
```python
# پایتون
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # در یک برنامه واقعی، از یک کتابخانه منطقه زمانی استفاده کنید
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... ابزارهای موجود ...
```
  
۳. ابزار را به آرایه `tools` اضافه کرده و با پرسش "الان ساعت چند است در توکیو؟" تست کنید.

> **چالش:** ابزاری بسازید که محاسبه انجام می‌دهد، مثل `convert_temperature` که دما را بین سانتیگراد و فارنهایت تبدیل می‌کند. با عبارت "۱۰۰ درجه فارنهایت به سانتیگراد تبدیل شود." تست نمایید.

---

### تمرین ۷ — فراخوانی ابزار با ChatClient در SDK (جاوااسکریپت)

نمونه جاوااسکریپت در حال حاضر از `ChatClient` بومی SDK به جای OpenAI SDK استفاده می‌کند. این ویژگی راحتی است که نیاز به ساخت مشتری OpenAI را از بین می‌برد:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient مستقیماً از شی مدل ایجاد می‌شود
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat ابزارها را به عنوان پارامتر دوم می‌پذیرد
const response = await chatClient.completeChat(messages, tools);
```
  
این را با روش پایتون که مستقیما از OpenAI SDK استفاده می‌کند مقایسه کنید:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```
  
هر دو الگو معتبرند. `ChatClient` راحت‌تر است؛ OpenAI SDK دسترسی به پارامترهای کامل OpenAI را می‌دهد.

> **امتحان کنید:** نمونه جاوااسکریپت را تغییر دهید تا به جای `ChatClient` از OpenAI SDK استفاده کند. باید `import OpenAI from "openai"` را داشته باشید و کلاینت را با آدرس از `manager.urls[0]` بسازید.

---

### تمرین ۸ — درک `tool_choice`

پارامتر `tool_choice` کنترل می‌کند که مدل *باید* از ابزار استفاده کند یا آزاد است:

| مقدار | رفتار |
|--------|-------|
| `"auto"` | مدل تصمیم می‌گیرد ابزار را فراخوانی کند (پیش‌فرض) |
| `"none"` | مدل هیچ ابزاری را فراخوانی نمی‌کند، حتی اگر موجود باشد |
| `"required"` | مدل باید حداقل یک ابزار را فراخوانی کند |
| `{"type": "function", "function": {"name": "get_weather"}}` | مدل باید ابزار مشخص‌شده را فراخوانی کند |

همه گزینه‌ها را در نمونه پایتون امتحان کنید:

```python
# مدل را مجبور کنید تا get_weather را فراخوانی کند
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```
  
> **توجه:** همه مدل‌ها ممکن است تمام گزینه‌های `tool_choice` را پشتیبانی نکنند. اگر مدلی `"required"` را پشتیبانی نکند، ممکن است آن را نادیده گرفته و رفتار پیش‌فرض `"auto"` داشته باشد.

---

## مشکلات متداول

| مشکل | راه‌حل |
|--------|--------|
| مدل هرگز ابزارها را فراخوانی نمی‌کند | مطمئن شوید از مدل فراخوانی ابزار استفاده می‌کنید (مثلاً qwen2.5-0.5b). جدول بالا را چک کنید. |
| عدم تطابق `tool_call_id` | همیشه از شناسه (`id`) پاسخ فراخوانی ابزار استفاده کنید، نه مقدار سخت‌کد شده |
| مدل JSON معیوب در `arguments` بازمی‌گرداند | مدل‌های کوچک‌تر گاه JSON نامعتبری تولید می‌کنند. از `try/catch` برای `JSON.parse()` استفاده کنید |
| مدل ابزاری را فراخوانی می‌کند که وجود ندارد | یک هندلر پیش‌فرض در تابع `execute_tool` اضافه کنید |
| حلقه بی‌نهایت فراخوانی ابزار | تعداد دور ماکزیمم (مثلاً ۵) تنظیم کنید تا از حلقه‌های بی‌پایان جلوگیری شود |

---

## نکات کلیدی

۱. **فراخوانی ابزار** به مدل‌ها اجازه می‌دهد درخواست اجرای تابع کنند به جای حدس زدن پاسخ  
۲. مدل **هرگز کد را اجرا نمی‌کند**؛ برنامه شما تصمیم می‌گیرد چه چیزی اجرا شود  
۳. ابزارها به صورت اشیاء **JSON Schema** مطابق فرمت فراخوانی توابع OpenAI تعریف می‌شوند  
۴. گفتگو از الگوی **چندنوبتی** استفاده می‌کند: کاربر، سپس دستیار (tool_calls)، سپس ابزار (نتایج)، سپس دستیار (پاسخ نهایی)  
۵. همیشه از **مدلی که فراخوانی ابزار را پشتیبانی می‌کند** استفاده کنید (Qwen 2.5، Phi-4-mini)  
۶. متد `createChatClient()` در SDK روشی راحت برای درخواست فراخوانی ابزار بدون ساخت مشتری OpenAI است

---

برای افزودن رابط وب به خط‌لوله چندعامله با استریم زنده، به [بخش ۱۲: ساخت UI وب برای Zava Creative Writer](part12-zava-ui.md) مراجعه کنید.

---

[← بخش ۱۰: مدل‌های سفارشی](part10-custom-models.md) | [بخش ۱۲: رابط نویسنده Zava →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**سلب مسئولیت**:  
این سند با استفاده از سرویس ترجمه هوش مصنوعی [Co-op Translator](https://github.com/Azure/co-op-translator) ترجمه شده است. در حالی که ما در تلاش برای دقت هستیم، لطفاً آگاه باشید که ترجمه‌های خودکار ممکن است حاوی خطاها یا نادرستی‌هایی باشند. سند اصلی به زبان بومی آن باید به عنوان منبع معتبر در نظر گرفته شود. برای اطلاعات حیاتی، ترجمه حرفه‌ای انسانی توصیه می‌شود. ما مسئول هیچ‌گونه سوءتفاهم یا برداشت نادرست ناشی از استفاده از این ترجمه نیستیم.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->