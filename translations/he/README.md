<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# סדנת Foundry Local – בניית יישומי AI במכשיר

סדנה מעשית להרצת דגמי שפה על המחשב האישי שלך ובניית אפליקציות חכמות עם [Foundry Local](https://foundrylocal.ai) ו-[Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **מה זה Foundry Local?** Foundry Local הוא זמן ריצה קל משקל המאפשר לך להוריד, לנהל ולשרת דגמי שפה באופן מלא על החומרה שלך. הוא מציע **API תואם OpenAI** כך שכל כלי או SDK שמדבר OpenAI יכולים להתחבר – ללא צורך בחשבון ענן.

### 🌐 תמיכה רב-לשונית

#### נתמך באמצעות GitHub Action (אוטומטי ותמיד מעודכן)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[ערבית](../ar/README.md) | [בנגלית](../bn/README.md) | [בולגרית](../bg/README.md) | [בורמזית (מיאנמר)](../my/README.md) | [סינית (מפושטת)](../zh-CN/README.md) | [סינית (מסורתית, הונג קונג)](../zh-HK/README.md) | [סינית (מסורתית, מקאו)](../zh-MO/README.md) | [סינית (מסורתית, טאיוואן)](../zh-TW/README.md) | [קרואטית](../hr/README.md) | [צ'כית](../cs/README.md) | [דנית](../da/README.md) | [הולנדית](../nl/README.md) | [אסטונית](../et/README.md) | [פינית](../fi/README.md) | [צרפתית](../fr/README.md) | [גרמנית](../de/README.md) | [יוונית](../el/README.md) | [עברית](./README.md) | [הינדית](../hi/README.md) | [הונגרית](../hu/README.md) | [אינדונזית](../id/README.md) | [איטלקית](../it/README.md) | [יפנית](../ja/README.md) | [קנדית](../kn/README.md) | [חמרית](../km/README.md) | [קוריאנית](../ko/README.md) | [ליטאית](../lt/README.md) | [מלאית](../ms/README.md) | [מלאלאם](../ml/README.md) | [מרתית](../mr/README.md) | [נפאלית](../ne/README.md) | [פידג'ין ניגרי](../pcm/README.md) | [נורווגית](../no/README.md) | [פרסית (פארסית)](../fa/README.md) | [פולנית](../pl/README.md) | [פורטוגזית (ברזיל)](../pt-BR/README.md) | [פורטוגזית (פורטוגל)](../pt-PT/README.md) | [פנג'אבית (גורמוקי)](../pa/README.md) | [רומנית](../ro/README.md) | [רוסית](../ru/README.md) | [סרבית (קירילית)](../sr/README.md) | [סלובקית](../sk/README.md) | [סלובנית](../sl/README.md) | [ספרדית](../es/README.md) | [סווהילית](../sw/README.md) | [שבדית](../sv/README.md) | [טגלוג (פיליפינית)](../tl/README.md) | [טמילית](../ta/README.md) | [טלוגו](../te/README.md) | [תאית](../th/README.md) | [טורקית](../tr/README.md) | [אוקראינית](../uk/README.md) | [אורדו](../ur/README.md) | [וויאטנמית](../vi/README.md)

> **מעדיף לשכפל מקומית?**
>
> מאגר זה כולל מעל 50 תרגומים לשפות שמגדילים משמעותית את גודל ההורדה. כדי לשכפל ללא תרגומים, השתמש ב-sparse checkout:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> זה נותן לך את כל מה שאתה צריך כדי להשלים את הקורס במהירות הורדה גבוהה הרבה יותר.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## יעדי למידה

בסיום הסדנה תוכל:

| # | יעד |
|---|-----------|
| 1 | להתקין את Foundry Local ולנהל דגמים עם CLI |
| 2 | לשלוט ב-Foundry Local SDK API לניהול דגמים בתכנות |
| 3 | להתחבר לשרת האינפרנס המקומי בעזרת SDKs של Python, JavaScript ו-C# |
| 4 | לבנות צינור Retrieval-Augmented Generation (RAG) המאפשר מענה מבוסס על הנתונים האישיים שלך |
| 5 | ליצור סוכני AI עם הוראות ופרסונות קבועות |
| 6 | לארגן זרימות עבודה של סוכנים מרובים עם לולאות משוב |
| 7 | לחקור אפליקציית קאפסטון בייצור – Zava Creative Writer |
| 8 | לבנות מסגרות הערכה עם ערכי זהב ודירוג LLM כשופט |
| 9 | לתמלל קול עם Whisper – דיבור לטקסט במכשיר תוך שימוש ב-Foundry Local SDK |
| 10 | לקמפל ולהריץ דגמים מותאמים אישית או Hugging Face עם ONNX Runtime GenAI ו-Foundry Local |
| 11 | לאפשר דגמים מקומיים לבצע קריאות לפונקציות חיצוניות עם דפוס tool-calling |
| 12 | לבנות ממשק משתמש מבוסס דפדפן ל-Zava Creative Writer עם סטרימינג בזמן אמת |

---

## דרישות מוקדמות

| דרישה | פרטים |
|-------------|---------|
| **חומרה** | מינימום 8GB RAM (מומלץ 16GB); CPU תומך AVX2 או GPU נתמך |
| **מערכת הפעלה** | Windows 10/11 (x64/ARM), Windows Server 2025, או macOS 13+ |
| **Foundry Local CLI** | התקן דרך `winget install Microsoft.FoundryLocal` (Windows) או `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). ראה את [מדריך ההתחלה](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) לפרטים. |
| **סביבת ריצה לשפה** | **Python 3.9+** ו/או **.NET 9.0+** ו/או **Node.js 18+** |
| **Git** | לשכפל את המאגר |

---

## התחלה מהירה

```bash
# 1. שכפל את המאגר
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. אמת ש-Foundry Local מותקן
foundry model list              # הצג את הדגמים הזמינים
foundry model run phi-3.5-mini  # התחל שיחה אינטראקטיבית

# 3. בחר את מסלול השפה שלך (ראה את מעבדת חלק 2 להתקנה מלאה)
```

| שפה | התחלה מהירה |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## חלקי הסדנה

### חלק 1: התחלה עם Foundry Local

**מדריך מעבדה:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- מה זה Foundry Local ואיך הוא עובד
- התקנת CLI ב-Windows ו-macOS
- בחינת דגמים – רשימה, הורדה, הרצה
- הבנת כינויים לדגמים ויציאות דינמיות

---

### חלק 2: מעמיק ב-Foundry Local SDK

**מדריך מעבדה:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- למה להשתמש ב-SDK על פני CLI לפיתוח אפליקציות
- הפניה מלאה ל-API של SDK ב-Python, JavaScript ו-C#
- ניהול שירות, גלישה בקטלוג, מחזור חיים של דגם (הורדה, טעינה, פריקה)
- דפוסי התחלה מהירה: Bootstrap ב-Python, `init()` ב-JavaScript, `CreateAsync()` ב-C#
- מטא-דאטה של `FoundryModelInfo`, כינויים ובחירת דגם מיטבית לחומרה

---

### חלק 3: SDKs ו-APIs

**מדריך מעבדה:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- התחברות ל-Foundry Local ב-Python, JavaScript ו-C#
- שימוש ב-Foundry Local SDK לניהול השירות בתכנות
- סטרימינג של תגובות צ'אט דרך API תואם OpenAI
- הפניה לשיטות SDK לכל שפה

**דוגמאות קוד:**

| שפה | קובץ | תיאור |
|----------|------|-------------|
| Python | `python/foundry-local.py` | צ'אט בסיסי בזרם |
| C# | `csharp/BasicChat.cs` | צ'אט בזרם עם .NET |
| JavaScript | `javascript/foundry-local.mjs` | צ'אט בזרם עם Node.js |

---

### חלק 4: Retrieval-Augmented Generation (RAG)

**מדריך מעבדה:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- מה זה RAG ומדוע הוא חשוב
- בניית בסיס ידע בזיכרון
- שליפת מילות מפתח עם ניקוד
- יצירת פקודות מערכת מונחות
- הרצת צינור RAG מלא במכשיר

**דוגמאות קוד:**

| שפה | קובץ |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### חלק 5: בניית סוכני AI

**מדריך מעבדה:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- מהו סוכן AI (בניגוד לקריאה ישירה ל-LLM)
- דפוס `ChatAgent` ו-Microsoft Agent Framework
- הוראות מערכת, פרסונות ושיחות רב-סיבוביות
- פלט מובנה (JSON) מהסוכנים

**דוגמאות קוד:**

| שפה | קובץ | תיאור |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | סוכן יחיד עם Agent Framework |
| C# | `csharp/SingleAgent.cs` | סוכן יחיד (דפוס ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | סוכן יחיד (דפוס ChatAgent) |

---

### חלק 6: זרימות עבודה של סוכנים מרובים

**מדריך מעבדה:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- צינורות עם סוכנים מרובים: חוקר → כותב → עורך
- ארגון עוקב ולולאות משוב
- שיתוף תצורה ומעברים מובנים
- עיצוב זרימת עבודה של סוכנים מרובים משלך

**דוגמאות קוד:**

| שפה | קובץ | תיאור |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | צינור שלושה סוכנים |
| C# | `csharp/MultiAgent.cs` | צינור שלושה סוכנים |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | צינור שלושה סוכנים |

---

### חלק 7: Zava Creative Writer – אפליקציית קאפסטון

**מדריך מעבדה:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- אפליקציה בסגנון ייצור עם 4 סוכנים מתמחים
- צינור עוקב עם לולאות משוב מונעות על ידי מעריך
- פלט בזרם, חיפוש קטלוג מוצר, מעברים מובנים ב-JSON
- יישום מלא ב-Python (FastAPI), JavaScript (Node.js CLI) ו-C# (.NET קונסולה)

**דוגמאות קוד:**

| שפה | ספרייה | תיאור |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | שירות רשת FastAPI עם אורסטרטור |
| JavaScript | `zava-creative-writer-local/src/javascript/` | אפליקציית CLI של Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | אפליקציית קונסולה .NET 9 |

---

### חלק 8: פיתוח מודרך הערכה

**מדריך מעבדה:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- לבנות מסגרת הערכה שיטתית לסוכני AI עם ערכי זהב
- בדיקות מבוססות כללים (אורך, כיסוי מילות מפתח, מונחים אסורים) + דירוג LLM כשופט
- השוואה צד לצד של וריאציות פקודות עם כרטיסי ניקוד מצטברים
- מרחיב את דפוס סוכן העורך של Zava מהחלק 7 אל ערכת בדיקות לא מקוונת
- מסלולים ב-Python, JavaScript ו-C#

**דוגמאות קוד:**

| שפה | קובץ | תיאור |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | מסגרת הערכה |
| C# | `csharp/AgentEvaluation.cs` | מסגרת הערכה |
| JavaScript | `javascript/foundry-local-eval.mjs` | מסגרת הערכה |

---

### חלק 9: תמלול קול עם Whisper

**מדריך מעבדה:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- תמלול דיבור לטקסט באמצעות OpenAI Whisper הפועל מקומית  
- עיבוד שמע תוך שמירה על פרטיות - השמע לעולם לא עוזב את המכשיר שלך  
- מסלולי Python, JavaScript ו-C# עם `client.audio.transcriptions.create()` (Python/JS) ו-`AudioClient.TranscribeAudioAsync()` (C#)  
- כולל קבצי שמע לדוגמה בסגנון זאבה לתרגול מעשי  

**דוגמאות קוד:**  

| Language | File | Description |  
|----------|------|-------------|  
| Python | `python/foundry-local-whisper.py` | תמלול קולי של Whisper |  
| C# | `csharp/WhisperTranscription.cs` | תמלול קולי של Whisper |  
| JavaScript | `javascript/foundry-local-whisper.mjs` | תמלול קולי של Whisper |  

> **הערה:** מעבדה זו משתמשת ב **Foundry Local SDK** להורדה וטעינה תכ-programmatית של מודל Whisper, ולאחר מכן שולחת שמע לנקודת קצה תואמת OpenAI הפועלת באופן מקומי לתמלול. מודל Whisper (`whisper`) מופיע בקטלוג Foundry Local ופועל כולו על המכשיר - לא נדרשים מפתחות API לענן או גישה לרשת.

---

### חלק 10: שימוש במודלים מותאמים אישית או Hugging Face  

**מדריך מעבדה:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)  

- הידור מודלים של Hugging Face לפורמט ONNX אופטימלי באמצעות בונה מודלי GenAI ל-ONNX Runtime  
- הידור מותאם לחומרה (CPU, NVIDIA GPU, DirectML, WebGPU) וקואנטיזציה (int4, fp16, bf16)  
- יצירת קבצי תצורת תבנית שיחה עבור Foundry Local  
- הוספת מודלים מוכללים למטמון Foundry Local  
- הרצת מודלים מותאמים באמצעות CLI, REST API ו-SDK של OpenAI  
- דוגמה להדגמה: הידור מלא של Qwen/Qwen3-0.6B   

---

### חלק 11: קריאת כלים עם מודלים מקומיים  

**מדריך מעבדה:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)  

- לאפשר למודלים מקומיים לקרוא לפונקציות חיצוניות (קריאת כלים / פונקציות)  
- הגדרת סכמות כלים בפורמט קריאת הפונקציות של OpenAI  
- טיפול בזרימת שיחה של קריאת כלים רב-סבבית  
- ביצוע קריאות כלים במכשיר והחזרת תוצאות למודל  
- בחירת המודל הנכון לתרחישי קריאת כלים (Qwen 2.5, Phi-4-mini)  
- שימוש ב`ChatClient` המובנה ב-SDK לקריאת כלים (JavaScript)  

**דוגמאות קוד:**  

| Language | File | Description |  
|----------|------|-------------|  
| Python | `python/foundry-local-tool-calling.py` | קריאת כלים עם כלים למזג אוויר ואוכלוסייה |  
| C# | `csharp/ToolCalling.cs` | קריאת כלים עם .NET |  
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | קריאת כלים עם ChatClient |  

---

### חלק 12: בניית ממשק אינטרנטי ל-Zava Creative Writer  

**מדריך מעבדה:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)  

- הוספת ממשק קדמי בדפדפן ל-Zava Creative Writer  
- הפצת ממשק מאוחד מ-Python (FastAPI), JavaScript (Node.js HTTP) ו-C# (ASP.NET Core)  
- צריכת NDJSON זורם בדפדפן באמצעות Fetch API ו-ReadableStream  
- סמלוני סטטוס חיים של סוכן וזרימת טקסט למאמר בזמן אמת  

**קוד (ממשק משותף):**  

| File | Description |  
|------|-------------|  
| `zava-creative-writer-local/ui/index.html` | פריסת עמוד |  
| `zava-creative-writer-local/ui/style.css` | עיצוב |  
| `zava-creative-writer-local/ui/app.js` | לוגיקת קורא זרם ועדכון DOM |  

**הוספות בשרת:**  

| Language | File | Description |  
|----------|------|-------------|  
| Python | `zava-creative-writer-local/src/api/main.py` | עודכן להפצת ממשק סטטי |  
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | שרת HTTP חדש שעוטף את האורקסטרטור |  
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | פרויקט API מינימלי חדש ב-ASP.NET Core |  

---

### חלק 13: סיום הסדנה  

**מדריך מעבדה:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)  

- סיכום כל מה שבנית בכל 12 החלקים  
- רעיונות להרחבה נוספת של האפליקציות שלך  
- קישורים למשאבים ולתיעוד  

---

## מבנה הפרויקט  

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```
  
---

## משאבים  

| Resource | Link |  
|----------|------|  
| אתר Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |  
| קטלוג מודלים | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |  
| Foundry Local ב-GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |  
| מדריך התחלה מהירה | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |  
| הפניה ל-SDK של Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |  
| מסגרת העבודה של Microsoft Agent | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |  
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |  
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |  

---

## רישיון  

חומר הסדנה מוצע למטרות חינוכיות בלבד.  

---

**בנייה מהנה! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**כתב ויתור**:  
מסמך זה תורגם באמצעות שירות התרגום האוטומטי [Co-op Translator](https://github.com/Azure/co-op-translator). בעוד שאנו שואפים לדיוק, יש להיות מודעים לכך שתרגומים אוטומטיים עשויים להכיל שגיאות או אי-דיוקים. המסמך המקורי בשפת המקור נחשב למקור הסמכותי. למידע קריטי מומלץ להיעזר בתרגום מקצועי אנושי. איננו אחראים לכל הבנה שגויה או פרשנות שגויה הנובעת מהשימוש בתרגום זה.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->