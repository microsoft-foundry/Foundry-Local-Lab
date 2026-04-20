![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# חלק 11: קריאה לכלים עם מודלים מקומיים

> **מטרה:** לאפשר למודל המקומי שלך לקרוא לפונקציות חיצוניות (כלים) כדי שיוכל לקבל נתונים בזמן אמת, לבצע חישובים או להפעיל APIs — הכל בפעולה פרטית במכשיר שלך.

## מהי קריאה לכלי?

קריאה לכלי (המכונה גם **קריאה לפונקציה**) מאפשרת למודל שפה לבקש ביצוע של פונקציות שהגדרת. במקום לנחש תשובה, המודל מזהה מתי כלי יעזור ומחזיר בקשה מובנית שהקוד שלך מפעיל. האפליקציה שלך מפעילה את הפונקציה, שולחת את התוצאה חזרה, והמודל משלב את המידע הזה בתשובה הסופית שלו.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

תבנית זו חיונית לבניית סוכנים שיכולים:

- **לאחזר נתונים חיים** (מזג אוויר, מחירי מניות, שאילתות למסדי נתונים)
- **לבצע חישובים מדויקים** (מתמטיקה, המרות יחידות)
- **לבצע פעולות** (שליחת אימיילים, יצירת כרטיסים, עדכון רשומות)
- **לגשת למערכות פרטיות** (APIs פנימיים, מערכות קבצים)

---

## איך קריאה לכלי פועלת

זרימת הקריאה לכלי כוללת ארבעה שלבים:

| שלב | מה קורה |
|-------|-------------|
| **1. הגדרת כלים** | אתה מתאר פונקציות זמינות באמצעות סכמת JSON — שם, תיאור ופרמטרים |
| **2. המודל מחליט** | המודל מקבל את ההודעה שלך יחד עם הגדרות הכלים. אם כלי יעזור, הוא מחזיר תגובת `tool_calls` במקום תשובה טקסטואלית |
| **3. הרצה מקומית** | הקוד שלך מפענח את קריאת הכלי, מריץ את הפונקציה ואוסף את התוצאה |
| **4. תשובה סופית** | אתה שולח את תוצאת הכלי למודל, שמפיק את התשובה הסופית שלו |

> **נקודה מרכזית:** המודל לעולם לא מבצע קוד. הוא רק *מבקש* שיקראו לכלי. האפליקציה שלך מחליטה אם למלא בקשה זו — וכך אתה שומר על שליטה מלאה.

---

## אילו מודלים תומכים בקריאה לכלים?

לא כל מודל תומך בקריאה לכלים. בקטלוג Foundry Local הנוכחי, המודלים הבאים תומכים ביכולת זו:

| מודל | גודל | קריאה לכלי |
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

> **עצה:** במעבדה זו נשתמש ב-**qwen2.5-0.5b** — מודל קטן (822 מגה להורדה), מהיר, ותומך בקריאה לכלים באופן אמין.

---

## מטרות הלימוד

בסוף מעבדה זו תוכל/י:

- להסביר את תבנית הקריאה לכלים ולמה היא חשובה לסוכני AI
- להגדיר סכמות כלים באמצעות פורמט קריאה לפונקציה של OpenAI
- לנהל זרימת שיחה מרובה סבבים עם קריאה לכלים
- לבצע קריאות לכלים באופן מקומי ולשלוח תוצאות למודל
- לבחור את המודל המתאים לתרחישי קריאה לכלים

---

## דרישות מוקדמות

| דרישה | פרטים |
|-------------|---------|
| **Foundry Local CLI** | מותקן וקיים ב-`PATH` שלך ([חלק 1](part1-getting-started.md)) |
| **Foundry Local SDK** | מותקן SDK של Python, JavaScript, או C# ([חלק 2](part2-foundry-local-sdk.md)) |
| **מודל עם קריאה לכלים** | qwen2.5-0.5b (יורד אוטומטית) |

---

## תרגילים

### תרגיל 1 — הבנת זרימת הקריאה לכלי

לפני כתיבת הקוד, למד/י את דיאגרמת הרצף הזו:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**תצפיות מפתח:**

1. אתה מגדיר את הכלים מראש כאובייקטי סכמת JSON
2. תגובת המודל מכילה `tool_calls` במקום תוכן רגיל
3. לכל קריאת כלי יש מזהה ייחודי `id` שעליך להפנות אליו בזמן החזרת התוצאות
4. המודל רואה את כל ההודעות הקודמות *בנוסף* לתוצאות הכלי בעת יצירת התשובה הסופית
5. אפשר לבקש קריאות כלים מרובות בתגובה אחת

> **דיון:** למה המודל מחזיר קריאות לכלים במקום להפעיל פונקציות ישירות? אילו יתרונות אבטחה זה מספק?

---

### תרגיל 2 — הגדרת סכמות כלים

כלים מוגדרים באמצעות פורמט קריאה לפונקציה של OpenAI. לכל כלי צריך:

- **`type`**: תמיד `"function"`
- **`function.name`**: שם פונקציה תיאורי (למשל `get_weather`)
- **`function.description`**: תיאור ברור — המודל משתמש בו להחליט מתי לקרוא לכלי
- **`function.parameters`**: אובייקט סכמת JSON שמתאר את הפרמטרים המצופים

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

> **נהלים מומלצים לתיאורי כלים:**
> - היו ספציפיים: "קבל את מזג האוויר הנוכחי לעיר נתונה" עדיף על "קבל מזג אוויר"
> - תארו את הפרמטרים בבירור: המודל קורא תיאורים אלה למילוי הערכים הנכונים
> - סמן פרמטרים חובה לעומת אופציונליים — תכונה זו עוזרת למודל להחליט מה לבקש

---

### תרגיל 3 — הפעלת דוגמאות קריאה לכלים

כל דוגמת שפה מגדירה שני כלים (`get_weather` ו-`get_population`), שולחת שאלה שגורמת להשתמש בכלים, מפעילה את הכלי באופן מקומי ושולחת את התוצאה חזרה לתשובה סופית.

<details>
<summary><strong>🐍 Python</strong></summary>

**דרישות מוקדמות:**
```bash
cd python
python -m venv venv

# ווינדוס (פאוורשל):
venv\Scripts\Activate.ps1
# מק או אס / לינוקס:
source venv/bin/activate

pip install -r requirements.txt
```

**הרצה:**
```bash
python foundry-local-tool-calling.py
```

**פלט צפוי:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**סקירת קוד** (`python/foundry-local-tool-calling.py`):

```python
# הגדר כלים כרשימת סכימות של פונקציות
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

# שלח עם כלים — המודל עשוי להחזיר קריאות כלים במקום תוכן
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# בדוק אם המודל רוצה לקרוא לכלי
if response.choices[0].message.tool_calls:
    # הרץ את הכלי ושלח את התוצאה בחזרה
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**דרישות מוקדמות:**
```bash
cd javascript
npm install
```

**הרצה:**
```bash
node foundry-local-tool-calling.mjs
```

**פלט צפוי:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**סקירת קוד** (`javascript/foundry-local-tool-calling.mjs`):

בדוגמה זו משתמשים ב-`ChatClient` המקורי של Foundry Local SDK במקום OpenAI SDK, המדגים את נוחות השימוש ב-`createChatClient()`:

```javascript
// קבל ChatClient ישירות מאובייקט הדגם
const chatClient = model.createChatClient();

// שלח עם כלים — ChatClient מטפל בפורמט התואם ל-OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// בדוק קריאות לכלים
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // הפעל כלים ושלח חזרה את התוצאות
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**דרישות מוקדמות:**
```bash
cd csharp
dotnet restore
```

**הרצה:**
```bash
dotnet run toolcall
```

**פלט צפוי:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**סקירת קוד** (`csharp/ToolCalling.cs`):

C# משתמש ב-helper `ChatTool.CreateFunctionTool` להגדרת כלים:

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

### תרגיל 4 — זרימת שיחה בקריאה לכלי

הבנת מבנה ההודעות קריטית. הנה הזרימה המלאה, מציגה את מערך ה-`messages` בכל שלב:

**שלב 1 — בקשה ראשונית:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**שלב 2 — המודל מגיב עם `tool_calls` (לא תוכן):**
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

**שלב 3 — אתה מוסיף את הודעת העוזר וגם את תוצאת הכלי:**
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

**שלב 4 — המודל מייצר את התשובה הסופית תוך שימוש בתוצאת הכלי.**

> **חשוב:** ערך `tool_call_id` בהודעת הכלי חייב להתאים ל-`id` של קריאת הכלי. כך המודל משייך תוצאות לבקשות.

---

### תרגיל 5 — קריאות כלים מרובות

מודל יכול לבקש מספר קריאות כלים בתגובה אחת. נסה לשנות את הודעת המשתמש כדי להפעיל קריאות מרובות:

```python
# בפייתון — שנה את הודעת המשתמש:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// ב־JavaScript — שנה את הודעת המשתמש:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

המודל צריך להחזיר שתי `tool_calls` — אחת ל-`get_weather` ואחת ל-`get_population`. הקוד שלך כבר מטפל בכך כי הוא עובר על כל קריאות הכלים.

> **נסה:** שנה את הודעת המשתמש והריץ שוב. האם המודל קורא לשני הכלים?

---

### תרגיל 6 — הוספת כלי משלך

הרחב אחד מהדוגמאות עם כלי חדש. למשל, הוסף כלי `get_time`:

1. הגדיר את סכמת הכלי:
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

2. הוסף את לוגיקת ההרצה:
```python
# פייתון
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # באפליקציה אמיתית, השתמש בספריית אזורי זמן
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... כלים קיימים ...
```

3. הוסף את הכלי למערך `tools` ובדוק עם: "מה השעה בטוקיו?"

> **אתגר:** הוסף כלי שמבצע חישוב, כמו `convert_temperature` הממיר בין צלסיוס לפרנהייט. בדוק עם: "המר 100°F לצלסיוס."

---

### תרגיל 7 — קריאה לכלים עם ChatClient של ה-SDK (JavaScript)

דוגמת JavaScript כבר משתמשת ב-`ChatClient` המקורי של ה-SDK במקום OpenAI SDK. זוהי פונקציה נוחה שמסירה את הצורך לבנות לקוח OpenAI בעצמך:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient נוצר ישירות מאובייקט המודל
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat מקבל כלים כפרמטר שני
const response = await chatClient.completeChat(messages, tools);
```

השווה זאת לגישת Python שמשתמשת במפורש ב-OpenAI SDK:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

שני הדגמים תקינים. `ChatClient` נוח יותר; OpenAI SDK נותן גישה מלאה לפרמטרים.

> **נסה:** שנה את דוגמת JavaScript להשתמש ב-OpenAI SDK במקום `ChatClient`. תצטרך `import OpenAI from "openai"` ולבנות את הלקוח עם ה-URL מ-`manager.urls[0]`.

---

### תרגיל 8 — הבנת tool_choice

פרמטר `tool_choice` שולט האם המודל *חייב* להשתמש בכלי או יכול לבחור באופן חופשי:

| ערך | התנהגות |
|-------|-----------|
| `"auto"` | המודל מחליט אם לקרוא לכלי (ברירת מחדל) |
| `"none"` | המודל לא יקרא לכלים, גם אם מסופקים |
| `"required"` | המודל חייב לקרוא לפחות לכלי אחד |
| `{"type": "function", "function": {"name": "get_weather"}}` | המודל חייב לקרוא לכלי שצוין |

נסה כל אפשרות בדוגמת Python:

```python
# לגרום למודל לקרוא ל-get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **הערה:** לא כל אפשרויות ה-`tool_choice` נתמכות על ידי כל מודל. אם מודל לא תומך ב-`"required"`, ייתכן שיתעלם מההגדרה ויתנהג כ-`"auto"`.

---

## בעיות נפוצות

| בעיה | פתרון |
|---------|----------|
| המודל לעולם לא קורא לכלים | ודא שאתה משתמש במודל שמטפל בקריאה לכלים (למשל qwen2.5-0.5b). בדוק בטבלה למעלה. |
| `tool_call_id` לא תואם | השתמש תמיד ב-`id` מהתגובה של קריאת הכלי, לא בערך קשיח |
| המודל מחזיר JSON שגוי ב-`arguments` | מודלים קטנים עלולים להפיק JSON לא תקין. עטוף את `JSON.parse()` ב-try/catch |
| המודל קורא לכלי שלא קיים | הוסף מטפל ברירת מחדל בפונקציה `execute_tool` שלך |
| לולאת קריאה לכלים אינסופית | הגדר מקסימום מספר סבבים (למשל 5) כדי למנוע לולאות בלתי נגמרות |

---

## נקודות מרכזיות לזכור

1. **קריאה לכלים** מאפשרת למודלים לבקש ביצוע פונקציות במקום לנחש תשובות
2. המודל **לעולם לא מבצע קוד**; האפליקציה שלך מחליטה מה להפעיל
3. כלים מוגדרים כאובייקטי **סכמת JSON** לפי פורמט קריאה לפונקציה של OpenAI
4. השיחה משתמשת ב**תבנית רב-סבבית**: משתמש, אז עוזר (tool_calls), אז כלי (תוצאות), ואז עוזר (תשובה סופית)
5. השתמש תמיד ב**מודל שתומך בקריאה לכלים** (Qwen 2.5, Phi-4-mini)
6. ה-SDK מספק דרך נוחה עם `createChatClient()` לקריאות כלים בלי לבנות לקוח OpenAI

---

המשך ל-[חלק 12: בניית ממשק UI ברשת ל-Zava Creative Writer](part12-zava-ui.md) להוספת ממשק מבוסס דפדפן לצנרת רב-סוכנים עם זרימה בזמן אמת.

---

[← חלק 10: מודלים מותאמים אישית](part10-custom-models.md) | [חלק 12: ממשק Zava Writer UI →](part12-zava-ui.md)