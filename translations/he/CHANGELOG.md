# יומן שינויים — Foundry Local Workshop

כל השינויים הבולטים בסדנה זו מתועדים למטה.

---

## 2026-03-11 — חלק 12 ו-13, ממשק רשת, שכתוב Whisper, תיקון WinML/QNN ואימות

### נוסף
- **חלק 12: בניית ממשק רשת ל-Zava Creative Writer** — מדריך מעבדה חדש (`labs/part12-zava-ui.md`) עם תרגילים המכסים שידור NDJSON, `ReadableStream` בדפדפן, תגי סטטוס סוכן חיים ושידור טקסט מאמר בזמן אמת  
- **חלק 13: סיום הסדנה** — מעבדה מסכמת חדשה (`labs/part13-workshop-complete.md`) עם סיכום כל 12 החלקים, רעיונות נוספים וקישורי משאבים  
- **ממשק Zava UI קדמי:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — ממשק דפדפן משותף ב-HTML/CSS/JS נאמן בו משתמשים כל שלושת ה-backends  
- **שרת HTTP ב-JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — שרת HTTP חדש בסגנון Express העוטף את האורקסטרטור לגישה מבוססת דפדפן  
- **Backend ASP.NET Core ב-C#:** `zava-creative-writer-local/src/csharp-web/Program.cs` ו-`ZavaCreativeWriterWeb.csproj` — פרויקט API מינימלי חדש המשרת את ה-UI ואת שידור NDJSON  
- **מחולל דוגמאות אודיו:** `samples/audio/generate_samples.py` — סקריפט TTS לא מקוון המשתמש ב-`pyttsx3` ליצירת קבצי WAV בנושא Zava עבור חלק 9  
- **דוגמת אודיו:** `samples/audio/zava-full-project-walkthrough.wav` — דוגמת אודיו ארוכה חדשה לבדיקות תמלול  
- **סקריפט אימות:** `validate-npu-workaround.ps1` — סקריפט PowerShell אוטומטי לאימות פיתרון עוקף NPU/QNN בכל דוגמאות ה-C#  
- **תרשימי Mermaid בפורמט SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`  
- **תמיכה חוצה פלטפורמות WinML:** כל 3 קבצי ה-`.csproj` ב-C# (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) משתמשים כעת ב-TFM מותנה ובאזכורי חבילות סותרים זה את זה לתמיכה חוצה פלטפורמות. ב-Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (סרשכול הכולל את תוסף QNN EP). במערכות שאינן Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK בסיסי). ה-RID הקשיח `win-arm64` בפרויקטים סופג אוטומטי. פתרון תלויתי עקיף שולל נכסים מקומיים מ-`Microsoft.ML.OnnxRuntime.Gpu.Linux` שיש לו הפניה שבורה ל-win-arm64. פתרון הניסיון/החריגה הקודם ל-NPU הוסר מכל 7 הקבצים ב-C#.

### שונה
- **חלק 9 (Whisper):** שכתוב משמעותי — JavaScript משתמש כעת ב-`AudioClient` המובנה ב-SDK (`model.createAudioClient()`) במקום אינפרנס ידני דרך ONNX Runtime; תיאורי ארכיטקטורה, טבלאות השוואה ותרשימי קו צינור עודכנו לשקף את הגישה של JS/C# עם `AudioClient` לעומת גישת Python עם ONNX Runtime  
- **חלק 11:** עדכון קישורי ניווט (כעת מופנים לחלק 12); הוספת תרשימי SVG מונפשים לפרויקט מתזמן הקריאות לכלים ולרצף  
- **חלק 10:** עדכון נתיב הניווט לעבור דרך חלק 12 במקום לסיים את הסדנה  
- **Python Whisper (`foundry-local-whisper.py`):** הורחב עם דוגמאות שמע נוספות ושיפור טיפול בשגיאות  
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** שכתב לשימוש ב-`model.createAudioClient()` עם `audioClient.transcribe()` במקום סשנים ידניים של ONNX Runtime  
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** עודכן לשרת קבצי UI סטטיים לצד API  
- **קונסולת Zava ב-C# (`zava-creative-writer-local/src/csharp/Program.cs`):** הוסר פתרון העקיפה ל-NPU (כעת מטופל באמצעות WinML)  
- **README.md:** הוספת סעיף חלק 12 עם טבלאות דוגמאות קוד ותרחיבי Backend; הוספת סעיף חלק 13; עדכון מטרות למידה ומבנה הפרויקט  
- **KNOWN-ISSUES.md:** הסרת תקלה #7 (גרסת מודל NPU SDK ב-C# — כעת מטופל על ידי WinML). מיספור מחדש לבעיות שנותרו ל-#1–#6. עדכון פרטי סביבה עם .NET SDK 10.0.104  
- **AGENTS.md:** עדכון עץ מבנה הפרויקט עם כניסות חדשות ל-`zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); עדכון חבילות מפתח C# ופרטי TFM מותנה  
- **labs/part2-foundry-local-sdk.md:** עדכון דוגמת `.csproj` להציג את תבנית חוצה הפלטפורמות המלאה עם TFM מותנה, אזכורי חבילות סותרים, והערה מוסברת  

### אומת
- שלושת פרויקטי ה-C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) נבנים בהצלחה על Windows ARM64  
- דוגמת צ'אט (`dotnet run chat`): טעינת מודל כ-`phi-3.5-mini-instruct-qnn-npu:1` דרך WinML/QNN — גרסת NPU נטענת ישירות ללא גיבוי CPU  
- דוגמת סוכן (`dotnet run agent`): רצה מקצה-לקצה עם שיחה מרובת סבבים, קוד יציאה 0  
- Foundry Local CLI v0.8.117 ו-SDK v0.9.0 על .NET SDK 9.0.312  

---

## 2026-03-11 — תיקוני קוד, ניקוי מודלים, תרשימי Mermaid ואימות

### תוקן
- **כל 21 דוגמאות הקוד (7 Python, 7 JavaScript, 7 C#):** נוסף ניקוי `model.unload()` / `unload_model()` / `model.UnloadAsync()` ביציאה לפתרון התראות דליפת זיכרון OGA (בעיה ידועה #4)  
- **csharp/WhisperTranscription.cs:** הוחלף הנתיב היחסי השברירי `AppContext.BaseDirectory` ב-`FindSamplesDirectory()` שמטפס תיקיות למיקום אמין של `samples/audio` (בעיה ידועה #7)  
- **csharp/csharp.csproj:** הוחלף `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` הקשיח בזיהוי אוטומטי עם `$(NETCoreSdkRuntimeIdentifier)` כך ש-`dotnet run` עובד על כל פלטפורמה ללא דגל `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))  

### שונה
- **חלק 8:** הומר לולאת איטרציה מבוססת הערכה מתרשים קופסה ASCII לתמונה בתצורת SVG מונפשת  
- **חלק 10:** הומר תרשים קו צינור הידור מחצים ASCII לתמונה בתצורת SVG מונפשת  
- **חלק 11:** הומרו תרשימי זרימת קריאת כלים ורצפים לתמונות SVG מונפשות  
- **חלק 10:** הסעיף "סיום הסדנה!" הועבר לחלק 11 (המעבדה הסופית); הוחלף בקישור "שלבים הבאים"  
- **KNOWN-ISSUES.md:** אימות מלא של כל הבעיות מול CLI v0.8.117. הוסר נפתרו: דליפת זיכרון OGA (נוסף ניקוי), נתיב Whisper (FindSamplesDirectory), HTTP 500 לא אינפרנס (לא ניתן לשחזור, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), מגבלות tool_choice (כעת עם `"required"` ופונקציה ספציפית ב-qwen2.5-0.5b). עדכון בעיית JS Whisper — כל הקבצים מחזירים פלט ריק/בינארי (רגרסיה מ-v0.9.x, חומרה מוגברת ל-Major). עדכון #4 RID ב-C# עם פתרון אוטומטי וקישור [#497](https://github.com/microsoft/Foundry-Local/issues/497). 7 בעיות פתוחות נשארו.  
- **javascript/foundry-local-whisper.mjs:** תוקן שם משתנה ניקוי (`whisperModel` → `model`)  

### אומת
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — ריצות מוצלחות עם ניקוי  
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — ריצות מוצלחות עם ניקוי  
- C#: `dotnet build` מצליח עם 0 אזהרות, 0 שגיאות (יעד net9.0)  
- כל 7 קבצי Python עוברים בדיקת תחביר `py_compile`  
- כל 7 קבצי JavaScript עוברים בדיקת תקינות תחביר עם `node --check`  

---

## 2026-03-10 — חלק 11: קריאת כלים, הרחבת SDK API וכיסוי מודלים

### נוסף
- **חלק 11: קריאת כלים עם מודלים מקומיים** — מדריך מעבדה חדש (`labs/part11-tool-calling.md`) עם 8 תרגילים המכסים סכימות כלים, זרימה מרובת סיבובים, קריאות כלים מרובות, כלים מותאמים, קריאת כלים עם ChatClient, ו-`tool_choice`  
- **דוגמת Python:** `python/foundry-local-tool-calling.py` — קריאת כלים עם כלים `get_weather`/`get_population` באמצעות OpenAI SDK  
- **דוגמת JavaScript:** `javascript/foundry-local-tool-calling.mjs` — קריאת כלים באמצעות ChatClient המובנה ב-SDK (`model.createChatClient()`)  
- **דוגמת C#:** `csharp/ToolCalling.cs` — קריאת כלים באמצעות `ChatTool.CreateFunctionTool()` עם OpenAI C# SDK  
- **חלק 2, תרגיל 7:** ChatClient מקומי — `model.createChatClient()` (JS) ו-`model.GetChatClientAsync()` (C#) כחלופות ל-OpenAI SDK  
- **חלק 2, תרגיל 8:** גרסאות מודלים ובחירת חומרה — `selectVariant()`, `variants`, טבלת וריאנטים של NPU (7 מודלים)  
- **חלק 2, תרגיל 9:** שדרוגי מודלים וחדשנות קטלוג — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`  
- **חלק 2, תרגיל 10:** מודלי היגיון — `phi-4-mini-reasoning` עם דוגמאות פרסור תגיות `<think>`  
- **חלק 3, תרגיל 4:** `createChatClient` כחלופה ל-OpenAI SDK, כולל תיעוד תבנית callback שידור  
- **AGENTS.md:** הוספת קונבנציות קידוד לקריאת כלים, ChatClient, ומודלי היגיון  

### שונה
- **חלק 1:** הרחבת קטלוג המודלים — הוספת phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo  
- **חלק 2:** הרחבת טבלאות התייחסות API — הוספת `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`  
- **חלק 2:** המרת מיספור תרגילים מ-7-9 → 10-13 כדי להכיל תרגילים חדשים  
- **חלק 3:** עדכון טבלת תובנות מפתח להכללת ChatClient מקורי  
- **README.md:** הוספת סעיף חלק 11 עם טבלת דוגמאות קוד; הוספת מטרה למידה מס' 11; עדכון עץ מבנה הפרויקט  
- **csharp/Program.cs:** הוספת מקרה `toolcall` לנתב CLI ועדכון טקסט הסיוע  

---

## 2026-03-09 — עדכון SDK v0.9.0, אנגלית בריטית ומעבר אימות

### שונה
- **כל דוגמאות הקוד (Python, JavaScript, C#):** עודכנו ל-Foundry Local SDK v0.9.0 API — תוקן `await catalog.getModel()` (היה חסר `await`), עדכון דפוסי איניציאליזציה של `FoundryLocalManager`, תיקון גילוי נקודות קצה  
- **כל מדריכי המעבדה (חלקים 1-10):** הומרו לאנגלית בריטית (colour, catalogue, optimised וכו')  
- **כל מדריכי המעבדה:** עדכון דוגמאות קוד SDK בהתאם לממשק v0.9.0  
- **כל מדריכי המעבדה:** עדכון טבלאות התייחסות API ובלוקי קוד תרגילים  
- **תיקון קריטי ב-JavaScript:** הוספת `await` חסר ב-`catalog.getModel()` — החזיר Promise במקום אובייקט Model, גרם לכשלים שקטים בהמשך  

### אומת
- כל דוגמאות ה-Python רצות בהצלחה מול Foundry Local service  
- כל דוגמאות JavaScript רצות בהצלחה (Node.js 18+)  
- פרויקט C# נבנה ורץ ב-.NET 9.0 (תאימות קדימה מ-net8.0 SDK assembly)  
- 29 קבצים שונו ואומתים לאורך כל הסדנה  

---

## אינדקס קבצים

| קובץ | עדכון אחרון | תיאור |
|------|-------------|--------|
| `labs/part1-getting-started.md` | 2026-03-10 | הרחבת קטלוג מודלים |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | תרגילים חדשים 7-10, הרחבת טבלאות API |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | תרגיל חדש 4 (ChatClient), עדכון תובנות |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + אנגלית בריטית |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + אנגלית בריטית |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + אנגלית בריטית |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + אנגלית בריטית |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | תרשים Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + אנגלית בריטית |
| `labs/part10-custom-models.md` | 2026-03-11 | תרשים Mermaid, העברת סיום סדנה לחלק 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | מעבדה חדשה, תרשימי Mermaid, חלק סיום סדנה |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | חדש: דוגמת קריאת כלים |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | חדש: דוגמת קריאת כלים |
| `csharp/ToolCalling.cs` | 2026-03-10 | חדש: דוגמת קריאת כלים |
| `csharp/Program.cs` | 2026-03-10 | נוסף פקודת CLI `toolcall` |
| `README.md` | 2026-03-10 | חלק 11, מבנה פרויקט |
| `AGENTS.md` | 2026-03-10 | קריאת כלים + קונבנציות ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | הוסר נושא #7 שנפתר, נשארו 6 נושאים פתוחים |
| `csharp/csharp.csproj` | 2026-03-11 | TFM רב-פלטפורמי, הפניות מותנות WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM רב-פלטפורמי, זיהוי אוטומטי של RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM רב-פלטפורמי, זיהוי אוטומטי של RID |
| `csharp/BasicChat.cs` | 2026-03-11 | הוסר פתרון לעקיפת try/catch של NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | הוסר פתרון לעקיפת try/catch של NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | הוסר פתרון לעקיפת try/catch של NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | הוסר פתרון לעקיפת try/catch של NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | הוסר פתרון לעקיפת try/catch של NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | דוגמה לקובץ .csproj רב-פלטפורמי |
| `AGENTS.md` | 2026-03-11 | עדכון חבילות C# ופרטי TFM |
| `CHANGELOG.md` | 2026-03-11 | הקובץ הזה |