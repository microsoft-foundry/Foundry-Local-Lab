<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# סדנת Foundry Local - בניית יישומי AI במכשיר

סדנה מעשית להרצת דגמי שפה על המחשב האישי שלך ולבניית יישומים אינטליגנטיים עם [Foundry Local](https://foundrylocal.ai) ו-[Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **מה זה Foundry Local?** Foundry Local הוא runtime קל משקל המאפשר להוריד, לנהל ולהפעיל דגמי שפה כוללים במכשיר שלך. הוא חושף **API תואם OpenAI** כך שכל כלי או SDK המדבר OpenAI יכול להתחבר — ללא צורך בחשבון ענן.

---

## יעדי למידה

בסיום הסדנה תוכל:

| # | יעד |
|---|-----------|
| 1 | להתקין את Foundry Local ולנהל דגמים דרך CLI |
| 2 | לשלוט ב-Foundry Local SDK API לניהול דגמים בתכנות |
| 3 | להתחבר לשרת האינפרנס המקומי באמצעות SDK של Python, JavaScript ו-C# |
| 4 | לבנות צינור RAG (יצירה מוגברת של החזרה) המושתת על הנתונים שלך |
| 5 | ליצור סוכני AI עם הוראות פרסיסטנטיות ופרסונות |
| 6 | לארגן תהליכי עבודה עם סוכנים מרובים עם לולאות משוב |
| 7 | לחקור אפליקציית קפסטון בשימוש ייצור – Zava Creative Writer |
| 8 | לבנות מסגרות הערכה עם מערכי נתונים זהב ודירוג LLM כ"שופט" |
| 9 | להמיר קול לטקסט עם Whisper – זיהוי דיבור במכשיר ע"י Foundry Local SDK |
| 10 | לקמפל ולהריץ דגמים מותאמים או Hugging Face עם ONNX Runtime GenAI ו-Foundry Local |
| 11 | לאפשר לדגמים מקומיים לקרוא לפונקציות חיצוניות בדפוס קריאת כלי |
| 12 | לבנות ממשק משתמש בדפדפן ל-Zava Creative Writer עם זרימה בזמן אמת |

---

## דרישות מוקדמות

| דרישה | פרטים |
|-------------|---------|
| **חומרה** | זיכרון RAM מינימלי 8 GB (מומלץ 16 GB); CPU עם AVX2 או GPU נתמך |
| **מערכת הפעלה** | Windows 10/11 (x64/ARM), Windows Server 2025, או macOS 13 ומעלה |
| **Foundry Local CLI** | התקנה באמצעות `winget install Microsoft.FoundryLocal` (Windows) או `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). ראה את מדריך [התחלה](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) לפרטים. |
| **סביבת ריצה לשפה** | **Python 3.9+** ו/או **.NET 9.0+** ו/או **Node.js 18+** |
| **Git** | לשכפול המאגר |

---

## התחלה

```bash
# 1. שכפל את המאגר
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. אמת שה-foundry Local מותקן
foundry model list              # הצג דגמים זמינים
foundry model run phi-3.5-mini  # התחל שיחה אינטראקטיבית

# 3. בחר את מסלול השפה שלך (ראה מעבדת חלק 2 להגדרה מלאה)
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
- חקר דגמים – רשימה, הורדה, הרצה
- הבנת שמות כינויים של דגמים ונמלים דינמיים

---

### חלק 2: ניתוח מעמיק של Foundry Local SDK

**מדריך מעבדה:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- למה להשתמש ב-SDK במקום ב-CLI לפיתוח אפליקציות
- הפניה מלאה ל-SDK API עבור Python, JavaScript ו-C#
- ניהול שירות, דפדוף בקטלוג, מחזור חיים של דגם (הורדה, טעינה, פריקה)
- דפוסי התחלה מהירה: אתחול ב-Python, `init()` ב-JavaScript, `CreateAsync()` ב-C#
- מטא-נתוני `FoundryModelInfo`, כינויים ובחירת דגם מיטבית לפי חומרה

---

### חלק 3: SDKs ו-APIs

**מדריך מעבדה:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- התחברות ל-Foundry Local מ-Python, JavaScript ו-C#
- שימוש ב-Foundry Local SDK לניהול השירות בתכנות
- שיחות צ'אט מתמשכות דרך API תואם OpenAI
- הפניה לשיטות SDK בכל שפה

**דוגמות קוד:**

| שפה | קובץ | תיאור |
|----------|------|-------------|
| Python | `python/foundry-local.py` | צ׳אט בסיסי עם סטרימינג |
| C# | `csharp/BasicChat.cs` | צ׳אט סטרימינג ב-.NET |
| JavaScript | `javascript/foundry-local.mjs` | צ׳אט סטרימינג ב-Node.js |

---

### חלק 4: יצירה מוגברת של החזרה (RAG)

**מדריך מעבדה:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- מה זה RAG ולמה זה חשוב
- בניית מאגר ידע בזיכרון
- איסוף בהתבסס על חפיפה במילות מפתח עם דירוג
- הרכבת הפעלות מערכת מונחות-נתונים
- הפעלת צינור RAG שלם במכשיר

**דוגמות קוד:**

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
- הוראות מערכת, פרסונות ושיחות עם סבבים מרובים
- פלט מובנה (JSON) מהסוכנים

**דוגמות קוד:**

| שפה | קובץ | תיאור |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | סוכן יחיד עם Agent Framework |
| C# | `csharp/SingleAgent.cs` | סוכן יחיד (דפוס ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | סוכן יחיד (דפוס ChatAgent) |

---

### חלק 6: תהליכי עבודה עם סוכנים מרובים

**מדריך מעבדה:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- צינורות עבודה עם סוכנים מרובים: חוקר → כותב → עורך
- תיאום רציף ולולאות משוב
- תצורה משותפת והעברות מבניות
- עיצוב תהליך עבודה עם סוכנים מרובים משלך

**דוגמות קוד:**

| שפה | קובץ | תיאור |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | צינור עבודה עם שלושה סוכנים |
| C# | `csharp/MultiAgent.cs` | צינור עבודה עם שלושה סוכנים |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | צינור עבודה עם שלושה סוכנים |

---

### חלק 7: Zava Creative Writer - אפליקציית קפסטון

**מדריך מעבדה:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- אפליקציית ייצור עם 4 סוכנים מתמחים
- צינור רציף עם לולאות משוב מונעות על-ידי מעריך
- פלט סטרימינג, חיפוש קטלוג מוצר, העברות JSON מובנות
- יישום מלא ב-Python (FastAPI), JavaScript (CLI ב-Node.js) ו-C# (קונסולת .NET)

**דוגמות קוד:**

| שפה | ספרייה | תיאור |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | שירות ווב FastAPI עם מווסת |
| JavaScript | `zava-creative-writer-local/src/javascript/` | אפליקציית CLI ב-Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | אפליקציית קונסולה ב-.NET 9 |

---

### חלק 8: פיתוח מונחה הערכה

**מדריך מעבדה:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- לבנות מסגרת הערכה שיטתית לסוכני AI עם מערכי נתונים זהב
- בדיקות מבוססות חוק (אורך, כיסוי מילות מפתח, מונחים אסורים) + ניקוד LLM כשופט
- השוואה צד-אל-צד של וריאציות פרומפט עם כרטיסי ניקוד מצטברים
- מרחיב את דפוס סוכן העורך של Zava מהחלק ה-7 למערכת בדיקה לא מקוונת
- מסלולי Python, JavaScript ו-C#

**דוגמות קוד:**

| שפה | קובץ | תיאור |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | מסגרת הערכה |
| C# | `csharp/AgentEvaluation.cs` | מסגרת הערכה |
| JavaScript | `javascript/foundry-local-eval.mjs` | מסגרת הערכה |

---

### חלק 9: תמלול קול עם Whisper

**מדריך מעבדה:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- תמלול דיבור לטקסט באמצעות OpenAI Whisper הפועל באופן מקומי
- עיבוד שמע בפרטיות - השמע אינו עוזב את המכשיר שלך
- מסלולי Python, JavaScript ו-C# עם `client.audio.transcriptions.create()` (Python/JS) ו-`AudioClient.TranscribeAudioAsync()` (C#)
- כולל קבצי דוגמה בנושא Zava לתרגול מעשי

**דוגמות קוד:**

| שפה | קובץ | תיאור |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | תמלול קול ב-Whisper |
| C# | `csharp/WhisperTranscription.cs` | תמלול קול ב-Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | תמלול קול ב-Whisper |

> **הערה:** מעבדה זו משתמשת ב-**Foundry Local SDK** להורדה וטעינה בתכנות של מודל Whisper, ואז שולחת שמע לנקודת קצה תואמת OpenAI מקומית לתמלול. מודל Whisper (`whisper`) רשום בקטלוג Foundry Local ופועל כולו במכשיר — ללא מפתחות API ענן או גישה לרשת.

---

### חלק 10: שימוש בדגמים מותאמים או Hugging Face

**מדריך מעבדה:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- קימפול דגמי Hugging Face לפורמט ONNX אופטימלי עם ONNX Runtime GenAI
- קומפילציה מותאמת חומרה (CPU, NVIDIA GPU, DirectML, WebGPU) וכימות (int4, fp16, bf16)
- יצירת קבצי תצורה לתבנית צ׳אט ל-Foundry Local
- הוספת דגמים מקומפלים למטמון Foundry Local
- הרצת דגמים מותאמים דרך CLI, REST API ו-OpenAI SDK
- דוגמה להפניה: קימפול Qwen/Qwen3-0.6B מקצה לקצה

---

### חלק 11: קריאת כלי עם דגמים מקומיים

**מדריך מעבדה:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- לאפשר לדגמים מקומיים לקרוא לפונקציות חיצוניות (קריאת כלי/פונקציה)
- להגדיר סכמת כלי באמצעות פורמט קריאת פונקציות OpenAI
- ניהול שיחת קריאת כלי בסבבים מרובים
- ביצוע קריאות כלי במכשיר והחזרת תוצאות לדגם
- לבחור את הדגם המתאים לתרחישי קריאת כלי (Qwen 2.5, Phi-4-mini)
- שימוש ב-`ChatClient` של ה-SDK לקריאת כלי (JavaScript)

**דוגמות קוד:**

| שפה | קובץ | תיאור |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | קריאת כלי עם כלים למזג אוויר ואוכלוסייה |
| C# | `csharp/ToolCalling.cs` | קריאת כלי ב-.NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | קריאת כלי עם ChatClient |

---

### חלק 12: בניית ממשק WEB ל-Zava Creative Writer

**מדריך מעבדה:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- הוספת ממשק משתמש בדפדפן ל-Zava Creative Writer
- הגשת ממשק משותף מ-Python (FastAPI), JavaScript (Node.js HTTP), ו-C# (ASP.NET Core)
- צריכת NDJSON בזרם בדפדפן עם Fetch API ו-ReadableStream
- תגי מצב חיים לסוכן וזרימת טקסט בזמן אמת

**קוד (ממשק משותף):**

| קובץ | תיאור |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | פריסת דף |
| `zava-creative-writer-local/ui/style.css` | עיצוב |
| `zava-creative-writer-local/ui/app.js` | קורא זרם ועדכון DOM |

**תוספות ב-backend:**

| שפה | קובץ | תיאור |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | מעודכן להגיש ממשק סטטי |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | שרת HTTP חדש העוטף את המווסת |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | פרויקט ASP.NET Core מינימלי חדש |

---

### חלק 13: סיום הסדנה
**מדריך המעבדה:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- סיכום של כל מה שבנית בכל 12 החלקים
- רעיונות נוספים להרחבת היישומים שלך
- קישורים למשאבים ותיעוד

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

| משאב | קישור |
|----------|------|
| אתר Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| קטלוג מודלים | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local ב-GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| מדריך התחלה מהירה | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| הפניות SDK של Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| מסגרת סוכן של Microsoft | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## רישיון

חומר סדנת העבודה הזה מסופק למטרות חינוכיות.

---

**בניית מוצלחת! 🚀**