# הוראות סוכן קידוד

קובץ זה מספק הקשר לסוכני קידוד בינה מלאכותית (GitHub Copilot, Copilot Workspace, Codex וכו') הפועלים במאגר זה.

## סקירת פרויקט

זהו **סדנא מעשית** לבניית יישומי בינה מלאכותית עם [Foundry Local](https://foundrylocal.ai) — ריצה קלה להורדה, ניהול, והפעלה של מודלי שפה במכשיר עצמו באמצעות API תואם OpenAI. הסדנא כוללת מדריכי מפעל צעד אחר צעד ודוגמאות קוד הפעלות בפייתון, JavaScript ו-C#.

## מבנה המאגר

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## פרטים על שפות ומסגרות עבודה

### פייתון
- **מיקום:** `python/`, `zava-creative-writer-local/src/api/`
- **תלויות:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **חבילות מרכזיות:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **גרסה מינימלית:** Python 3.9+
- **הרצה:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **מיקום:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **תלויות:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **חבילות מרכזיות:** `foundry-local-sdk`, `openai`
- **מערכת מודולים:** מודולים ES (`.mjs` קבצים, `"type": "module"`)
- **גרסה מינימלית:** Node.js 18+
- **הרצה:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **מיקום:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **קבצי פרויקט:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **חבילות מרכזיות:** `Microsoft.AI.Foundry.Local` (לא-וינדוס), `Microsoft.AI.Foundry.Local.WinML` (וינדוס — מעטפת עם QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **מטרה:** .NET 9.0 (TFM מותנה: `net9.0-windows10.0.26100` בוינדוס, `net9.0` במקום אחר)
- **הרצה:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## קונבנציות קידוד

### כללי
- כל דוגמאות הקוד הן **דוגמאות חד-קובץ עצמאיות** — ללא ספריות שירות משותפות או מופשטות.
- כל דוגמה פועלת באופן עצמאי לאחר התקנת התלויות שלה.
- מפתחות API מוגדרים תמיד ל־`"foundry-local"` — Foundry Local משתמש בזה כמייצג.
- כתובות בסיס הן `http://localhost:<port>/v1` — הפורט דינמי ונמצא בזמן ריצה באמצעות ה-SDK (`manager.urls[0]` ב-JS, `manager.endpoint` בפייתון).
- SDK של Foundry Local מנהל הפעלת השירות וגילוי נקודות הקצה; העדיפו להשתמש בתבניות SDK על פני פורטים מקודדים.

### פייתון
- השתמשו ב-SDK `openai` עם `OpenAI(base_url=..., api_key="not-required")`.
- השתמשו ב-`FoundryLocalManager()` מ־`foundry_local` לניהול מחזור חיים של השירות.
- סטרימינג: איטרציה על אובייקט `stream` עם `for chunk in stream:`.
- אין להוסיף הערות טיפוס בקבצי הדוגמאות (שמרו על קבצים תמציתיים ללומדים בסדנא).

### JavaScript
- תחביר ES מודולים: `import ... from "..."`.
- השתמשו ב־`OpenAI` מ־`"openai"` ו־`FoundryLocalManager` מ־`"foundry-local-sdk"`.
- תבנית אתחול SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- סטרימינג: `for await (const chunk of stream)`.
- נעשה שימוש ב־`await` ברמת קוד עליונה.

### C#
- Nullable מופעל, שימושים מפורשים, .NET 9.
- השתמשו ב־`FoundryLocalManager.StartServiceAsync()` לניהול מחזור החיים ע"י SDK.
- סטרימינג: `CompleteChatStreaming()` עם `foreach (var update in completionUpdates)`.
- `csharp/Program.cs` הראשי הוא מפצל CLI קורא ל־`RunAsync()` סטטיים.

### קריאת כלים
- רק דגמים מסוימים תומכים בקריאת כלים: משפחת **Qwen 2.5** (`qwen2.5-*`) ו־**Phi-4-mini** (`phi-4-mini`).
- סכמות הכלים עוקבות אחרי פורמט JSON של קריאת פונקציה ב-OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- השיחה משתמשת בדפוס ריבוי סבבים: משתמש → עוזר (tool_calls) → כלי (תוצאות) → עוזר (תשובה סופית).
- `tool_call_id` בהודעות תוצאות הכלי חייב להתאים ל־`id` שנשלח בקריאת הכלי בדגם.
- בפייתון משתמשים ישירות ב-SDK של OpenAI; ב־JavaScript משתמשים ב־`ChatClient` המקומי של ה-SDK (`model.createChatClient()`); ב־C# משתמשים ב־SDK של OpenAI עם `ChatTool.CreateFunctionTool()`.

### ChatClient (לקוח SDK מקורי)
- JavaScript: `model.createChatClient()` מחזיר `ChatClient` עם `completeChat(messages, tools?)` ו־`completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` מחזיר `ChatClient` סטנדרטי לשימוש ללא צורך בייבוא חבילת OpenAI.
- בפייתון אין ChatClient מקורי — השתמשו ב-SDK של OpenAI עם `manager.endpoint` ו־`manager.api_key`.
- **חשוב:** ב־JavaScript `completeStreamingChat` משתמש בתבנית קריאה חזרה (callback), לא באיטרציה אסינכרונית.

### דגמי הסקה
- `phi-4-mini-reasoning` עוטף את החשיבה שלו ב־`<think>...</think>` לפני התשובה הסופית.
- יש לפרש את התגים כדי להפריד בין ההסקה לתשובה במידת הצורך.

## מדריכי מעבדה

קבצי המעבדה נמצאים בתיקיית `labs/` בפורמט Markdown. הם כוללים מבנה עקבי:
- תמונת לוגו בראש
- כותרת וקול אאוט של המטרה
- סקירה כללית, מטרות למידה, דרישות מוקדמות
- הסברים קונספטואליים עם דיאגרמות
- תרגילים ממוספרים עם בלוקי קוד ופלט צפוי
- טבלת סיכום, תובנות מפתח, קריאה נוספת
- קישור ניווט לחלק הבא

בעת עריכת תוכן המעבדה:
- שמרו על סגנון העיצוב של Markdown ומבנה הסעיפים הקיים.
- בלוקים של קוד חייבים לציין את השפה (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- ספקו גם גרסאות bash וגם PowerShell לפקודות shell כאשר יש חשיבות למערכת ההפעלה.
- השתמשו בסגנונות דיווחים: `> **הערה:**`, `> **טיפ:**`, ו־`> **פתרון בעיות:**`.
- טבלאות ישלוטו בפורמט הצינור `| כותרת | כותרת |`.

## פקודות בנייה ובדיקה

| פעולה | פקודה |
|--------|---------|
| **דוגמאות פייתון** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **דוגמאות JS** | `cd javascript && npm install && node <script>.mjs` |
| **דוגמאות C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **יצירת דיאגרמות** | `npx mmdc -i <input>.mmd -o <output>.svg` (דורש התקנה של root `npm install`) |

## תלות חיצוניות

- יש להתקין את **Foundry Local CLI** במכונה של המפתח (`winget install Microsoft.FoundryLocal` או `brew install foundrylocal`).
- שירות Foundry Local רץ מקומית ופותח API REST תואם OpenAI בפורט דינמי.
- אין צורך בשירותי ענן, מפתחות API או מנויים ל-Azure כדי להריץ דוגמאות כלשהן.
- חלק 10 (מודלים מותאמים אישית) דורש בנוסף את `onnxruntime-genai` ומוריד משקלי מודל מ-Hugging Face.

## קבצים שלא צריכים להתחייב

קובץ `.gitignore` מחריג (ובאופן כללי מחריג את):
- `.venv/` — סביבות וירטואליות בפייתון
- `node_modules/` — תלויות npm
- `models/` — פלט מודל ONNX מקומפל (קבצים בינאריים גדולים, נוצרים בחלק 10)
- `cache_dir/` — מטמון הורדת מודל Hugging Face
- `.olive-cache/` — תיקיית עבודה של Microsoft Olive
- `samples/audio/*.wav` — דוגמאות אודיו שנוצרו (נוצרות מחדש דרך `python samples/audio/generate_samples.py`)
- ארטיפקטים סטנדרטיים של בניית פייתון (`__pycache__/`, `*.egg-info/`, `dist/`, וכו')

## רישיון

MIT — ראו `LICENSE`.