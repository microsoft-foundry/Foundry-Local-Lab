![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# חלק 12: בניית ממשק ווב ל-Zava Creative Writer

> **מטרה:** להוסיף ממשק משתמש בדפדפן ל-Zava Creative Writer כדי שתוכלו לצפות בצינור הריבוי-סוכנים פועל בזמן אמת, עם סיכות סטטוס סוכן בחיים וטקסט המאמר זורם, וכל זאת משרת על ידי שרת ווב מקומי יחיד.

ב-[חלק 7](part7-zava-creative-writer.md) חקרתם את Zava Creative Writer כ**יישום CLI** (JavaScript, C#) וכ**-API ללא ראש** (Python). במעבדה זו תחברו ממשק משותף של **HTML/CSS/JavaScript פשוט** לכל backend כך שמשתמשים יוכלו לתקשר עם הצינור דרך הדפדפן במקום דרך טרמינל.

---

## מה תלמדו

| מטרה | תיאור |
|-----------|-------------|
| להגיש קבצים סטטיים מ-backend | להרכיב ספריית HTML/CSS/JS לצד נתיב ה-API שלכם |
| לצרוך NDJSON זורם בדפדפן | להשתמש ב-Fetch API עם `ReadableStream` כדי לקרוא JSON מופרד שורות |
| פרוטוקול שידור אחיד | להבטיח של-Python, JavaScript ו-C# יש את אותו פורמט הודעות |
| עדכוני UI מתקדמים | לעדכן סיכות סטטוס סוכן ולשדר טקסט מאמר מפתח אחר מפתח |
| להוסיף שכבת HTTP ליישום CLI | לעטוף לוגיקת אורקסטרציה קיימת בשרת בסגנון Express (JS) או API מינימלי ב-ASP.NET Core (C#) |

---

## ארכיטקטורה

ממשק המשתמש הוא סט יחיד של קבצים סטטיים (`index.html`, `style.css`, `app.js`) שמשותפים לכל שלושת ה-backends. כל backend חושף את אותם שני נתיבים:

![ארכיטקטורת ממשק Zava — ממשק קדמי משותף עם שלושה backends](../../../images/part12-architecture.svg)

| נתיב | מתודולוגיה | מטרה |
|-------|--------|---------|
| `/` | GET | מגיש את ממשק המשתמש הסטטי |
| `/api/article` | POST | מריץ את צינור הריבוי-סוכנים ומשדר NDJSON |

הממשק הקדמי שולח גוף JSON וקורא את התגובה כזרם של הודעות JSON מופרדות שורות. לכל הודעה יש שדה `type` שהממשק משתמש בו כדי לעדכן את הפאנל הרלוונטי:

| סוג ההודעה | משמעות |
|-------------|---------|
| `message` | עדכון סטטוס (למשל "מתחיל משימת סוכן החוקר...") |
| `researcher` | תוצאות המחקר מוכנות |
| `marketing` | תוצאות חיפוש מוצר מוכנות |
| `writer` | הכותב התחיל או סיים (מכיל `{ start: true }` או `{ complete: true }`) |
| `partial` | טוקן אחד שזורם מהכותב (מכיל `{ text: "..." }`) |
| `editor` | פסק דין העורך מוכן |
| `error` | אירעה שגיאה |

![ניתוב סוגי הודעות בדפדפן](../../../images/part12-message-types.svg)

![רצף זרימה — תקשורת מהדפדפן ל-backend](../../../images/part12-streaming-sequence.svg)

---

## דרישות מוקדמות

- להשלמת [חלק 7: Zava Creative Writer](part7-zava-creative-writer.md)
- Foundry Local CLI מותקן ודגם `phi-3.5-mini` מורד
- דפדפן אינטרנט מודרני (Chrome, Edge, Firefox, או Safari)

---

## ממשק המשתמש המשותף

לפני שמגעים בקוד backend כלשהו, קחו רגע לחקור את הממשק הקדמי שכל שלושת המסלולים משתמשים בו. הקבצים יושבים ב-`zava-creative-writer-local/ui/`:

| קובץ | מטרה |
|------|---------|
| `index.html` | פריסת העמוד: טופס קלט, סיכות סטטוס סוכן, אזור פלט מאמר, פאנלים מפורטים מתכווצים |
| `style.css` | עיצוב מינימלי עם מצבי צבע לסיכות סטטוס (ממתין, רץ, גמור, שגיאה) |
| `app.js` | קריאת Fetch, קורא שורות ב-`ReadableStream`, ולוגיקת עדכון DOM |

> **טיפ:** פתחו את `index.html` ישירות בדפדפן כדי לצפות בפריסה. כלום לא יעבוד עדיין כי אין backend, אבל תוכלו לראות את המבנה.

### איך קורא הזרם עובד

הפונקציה המרכזית ב-`app.js` קוראת את גוף התגובה חתיכה חתיכה ומפצלת בהתבסס על גבולות שורות:

```javascript
async function readStream(response) {
  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = "";

  while (true) {
    const { done, value } = await reader.read();
    if (done) break;

    buffer += decoder.decode(value, { stream: true });
    const lines = buffer.split("\n");
    buffer = lines.pop(); // שמור על השורה הסופית הלא שלמה

    for (const line of lines) {
      const trimmed = line.trim();
      if (!trimmed) continue;
      try {
        const msg = JSON.parse(trimmed);
        if (msg && msg.type) handleMessage(msg);
      } catch { /* skip non-JSON lines */ }
    }
  }
}
```

כל הודעה מפוענחת נשלחת ל-`handleMessage()`, שמעדכן את אלמנט ה-DOM הרלוונטי בהתבסס על `msg.type`.

---

## תרגילים

### תרגיל 1: הרצת ה-Python Backend עם הממשק

הגרסה Python (FastAPI) כבר כוללת נקודת קצה API זורם. השינוי היחיד הוא להרכיב את תיקיית `ui/` כקבצים סטטיים.

**1.1** נווטו לתיקיית ה-Python API והתקינו תלותים:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** התחילו את השרת:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** פתחו בדפדפן בכתובת `http://localhost:8000`. אמור להיווצר ממשק Zava Creative Writer עם שלושה שדות טקסט וכפתור "Generate Article".

**1.4** לחצו על **Generate Article** עם הערכים ברירת מחדל. צפו בסיכות הסטטוס של הסוכנים משתנות מ"ממתין" ל"רץ" ל"גמור" כשכל סוכן מסיים את עבודתו, וראו את טקסט המאמר זורם לפאנל הפלט מפתח אחר מפתח.

> **פתרון בעיות:** אם העמוד מציג תגובת JSON במקום הממשק, בדקו שאתם מריצים את `main.py` המעודכן שמרכיב את הקבצים הסטטיים. נקודת הקצה `/api/article` עדיין פועלת בנתיב המקורי שלה; רכיב הקבצים הסטטיים משרת את הממשק בכל נתיב אחר.

**איך זה עובד:** `main.py` המעודכן מוסיף שורה אחת בתחתית:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

זה משרת כל קובץ מתיקיית `zava-creative-writer-local/ui/` כמשאב סטטי, כאשר `index.html` הוא המסמך ברירת המחדל. נתיב POST ל-`/api/article` רשום לפני ההרכבה הסטטית, ולכן מקבל עדיפות.

---

### תרגיל 2: הוספת שרת ווב לגרסת JavaScript

הגרסה JavaScript היא כעת יישום CLI (`main.mjs`). קובץ חדש, `server.mjs`, עוטף את אותן מודולי סוכנים מאחורי שרת HTTP ומשרת את הממשק המשותף.

**2.1** נווטו לתיקיית JavaScript והתקינו את התלויות:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** הפעלו את שרת הווב:

```bash
node server.mjs
```

```powershell
node server.mjs
```

עליכם לראות:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** פתחו `http://localhost:3000` בדפדפן ולחצו על **Generate Article**. אותו ממשק עובד בצורה זהה נגד backend ה-JavaScript.

**למדו את הקוד:** פתחו את `server.mjs` ושימו לב לתבניות המרכזיות:

- **שירות קבצים סטטיים** משתמש במודולי `http`, `fs`, ו-`path` המובנים ב-Node.js ללא צורך במסגרת חיצונית.
- **הגנה מפני מסלול מסלול** מבצעת נרמול של הנתיב המבוקש ומוודאת שהוא נשאר בתוך תיקיית `ui/`.
- **זרימת NDJSON** משתמשת בעוזר `sendLine()` שמסדרר כל אובייקט, מסיר שורות פנימיות, ומוסיף שורת סיום.
- **אורקסטרציה של סוכנים** משתמשת מחדש במודולים הקיימים `researcher.mjs`, `product.mjs`, `writer.mjs`, ו-`editor.mjs` ללא שינוי.

<details>
<summary>קטע מרכזי מתוך server.mjs</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// חוקר
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// סופר (שידור חי)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### תרגיל 3: הוספת API מינימלי לגרסת C#

הגרסה C# היא כיום יישום קונסולה. פרויקט חדש, `csharp-web`, משתמש ב-ASP.NET Core Minimal APIs כדי לחשוף את אותו הצינור כשירות ווב.

**3.1** נווטו לפרויקט הווב של C# ושלפו את החבילות:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** הריצו את שרת הווב:

```bash
dotnet run
```

```powershell
dotnet run
```

עליכם לראות:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** פתחו `http://localhost:5000` בדפדפן ולחצו על **Generate Article**.

**למדו את הקוד:** פתחו את `Program.cs` בתיקיית `csharp-web` ושימו לב:

- קובץ הפרויקט משתמש ב-`Microsoft.NET.Sdk.Web` במקום `Microsoft.NET.Sdk`, שמוסיף תמיכת ASP.NET Core.
- קבצים סטטיים מוגשים באמצעות `UseDefaultFiles` ו-`UseStaticFiles` הפונים לתיקיית `ui/` המשותפת.
- נקודת הקצה `/api/article` כותבת שורות NDJSON ישירות ל-`HttpContext.Response` ושוטפת אחרי כל שורה להזנה בזמן אמת.
- כל לוגיקת הסוכן (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) זהה לגרסת הקונסולה.

<details>
<summary>קטע מרכזי מתוך csharp-web/Program.cs</summary>

```csharp
app.MapPost("/api/article", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/event-stream; charset=utf-8";

    async Task SendLine(object obj)
    {
        var json = JsonSerializer.Serialize(obj).Replace("\n", "") + "\n";
        await ctx.Response.WriteAsync(json);
        await ctx.Response.Body.FlushAsync();
    }

    // Researcher
    await SendLine(new { type = "message", message = "Starting researcher agent task...", data = new { } });
    var researchResult = RunResearcher(body.Research, feedback);
    await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

    // Writer (streaming)
    foreach (var update in completionUpdates)
    {
        if (update.ContentUpdate.Count > 0)
        {
            var text = update.ContentUpdate[0].Text;
            await SendLine(new { type = "partial", message = "token", data = new { text } });
        }
    }
});
```

</details>

---

### תרגיל 4: חקר סיכות סטטוס הסוכנים

עכשיו כשיש לכם ממשק עובד, הסתכלו כיצד הממשק הקדמי מעדכן את סיכות הסטטוס.

**4.1** פתחו את `zava-creative-writer-local/ui/app.js` בעורך.

**4.2** מצאו את הפונקציה `handleMessage()`. שימו לב כיצד היא ממפה סוגי הודעות לעדכוני DOM:

| סוג הודעה | פעולה בממשק |
|-------------|-----------|
| `message` המכילה "researcher" | מגדירה את סיכת החוקר ל"רץ" |
| `researcher` | מגדירה את סיכת החוקר ל"גמור" וממלאת את פאנל תוצאות המחקר |
| `marketing` | מגדירה את סיכת חיפוש מוצר ל"גמור" וממלאת את פאנל התאמות המוצר |
| `writer` עם `data.start` | מגדירה את סיכת הכותב ל"רץ" ומנקה את פלט המאמר |
| `partial` | מוסיפה את טקסט הטוקן לפלט המאמר |
| `writer` עם `data.complete` | מגדירה את סיכת הכותב ל"גמור" |
| `editor` | מגדירה את סיכת העורך ל"גמור" וממלאת את פאנל הפידבק של העורך |

**4.3** פתחו את הפאנלים המתכווצים "תוצאות מחקר", "התאמות מוצר", ו"פידבק עורך" מתחת למאמר כדי לבדוק את ה-JSON הגולמי שכל סוכן הפיק.

---

### תרגיל 5: התאמת הממשק (Stretch)

נסו אחד או יותר מהשיפורים הבאים:

**5.1 הוספת ספירת מילים.** לאחר שהכותב מסיים, הציגו את ספירת המילים של המאמר מתחת לפאנל הפלט. ניתן לחשב זאת בתוך `handleMessage` כאשר `type === "writer"` ו-`data.complete` הוא true:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 הוספת אינדיקטור ניסיון חוזר.** כאשר העורך מבקש תיקון, הצינור מורץ מחדש. הציגו באנר "Revision 1" או "Revision 2" בפאנל הסטטוס. הקשיבו לסוג `message` המכיל "Revision" ועדכנו אלמנט DOM חדש.

**5.3 מצב כהה.** הוסיפו כפתור הפעלה וכיבוי וקטגוריית `.dark` ל-`<body>`. ביטלו צבעי רקע, טקסט ופאנלים ב-`style.css` עם בורר `body.dark`.

---

## סיכום

| מה עשיתם | איך |
|-------------|-----|
| הגשת הממשק מ-backend הפייתון | הרכבת תיקיית `ui/` עם `StaticFiles` ב-FastAPI |
| הוספת שרת HTTP לגרסת JavaScript | יצירת `server.mjs` באמצעות מודול `http` המובנה של Node.js |
| הוספת API ווב לגרסת C# | יצירת פרויקט `csharp-web` עם ASP.NET Core Minimal APIs |
| צריכת NDJSON משתף זרם בדפדפן | שימוש ב-`fetch()` עם `ReadableStream` וניתוח JSON שורה שורה |
| עדכון ממשק בזמן אמת | מיפוי סוגי הודעות לעדכוני DOM (סיכות, טקסט, פאנלים מתכווצים) |

---

## נקודות מהותיות

1. ממשק סטטי משותף יכול לעבוד עם כל backend שמדבר את אותו פרוטוקול שידור, מה שמחזק את ערך תבנית ה-API התואמת ל-OpenAI.
2. JSON מופרד שורות (NDJSON) הוא פורמט שידור פשוט שעובד באופן טבעי עם ה-ReadableStream בדפדפן.
3. גרסת Python דרשה את השינוי הכי קטן כי כבר הייתה לה נקודת FastAPI; גרסאות JavaScript ו-C# זקוקות לעטיפת HTTP דקה.
4. שמירת הממשק כ-HTML/CSS/JS פשוט מונעת צורך בכלי בניה, תלות במסגרת, ומורכבות נוספת ללומדי הסדנה.
5. אותם מודולי סוכן (Researcher, Product, Writer, Editor) משומשים ללא שינוי; רק שכבת התחבורה משתנה.

---

## קריאה נוספת

| משאב | קישור |
|----------|------|
| MDN: שימוש ב-Readable Streams | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| קבצים סטטיים ב-FastAPI | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| קבצים סטטיים ב-ASP.NET Core | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| מפרט NDJSON | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

המשיכו ל-[חלק 13: סיום הסדנה](part13-workshop-complete.md) לסיכום כל מה שבניתם במהלך הסדנה. 

---
[← חלק 11: קריאת כלי](part11-tool-calling.md) | [חלק 13: סדנה הושלמה →](part13-workshop-complete.md)