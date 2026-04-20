![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# חלק 10: שימוש במודלים מותאמים אישית או Hugging Face עם Foundry Local

> **מטרה:** לקמפל מודל Hugging Face לפורמט ONNX המיטבי ש-Foundry Local דורש, להגדיר אותו עם תבנית שיחה, להוסיף אותו למטמון המקומי, ולהריץ השערה מולו באמצעות CLI, REST API ו-OpenAI SDK.

## סקירה כללית

Foundry Local מגיע עם קטלוג מובחר של מודלים שקדמו לקימפול, אך אינך מוגבל לרשימה זו. כל מודל שפה מבוסס טרנספורמר הזמין ב-[Hugging Face](https://huggingface.co/) (או מאוחסן באופן מקומי בפורמט PyTorch / Safetensors) ניתן לקמפל למודל ONNX מיטבי ולהגיש דרך Foundry Local.

צינור הקימפול משתמש ב**ONNX Runtime GenAI Model Builder**, כלי שורת פקודה הכלול בחבילת `onnxruntime-genai`. בונה המודל מטפל בעבודת הכבד: הורדת המשקלים, המרתם לפורמט ONNX, יישום כימות (int4, fp16, bf16), והפקת קבצי התצורה (כולל תבנית שיחה ומטמיע הטוקנים) ש-Foundry Local מצפה להם.

במעבדה זו תקומפל את **Qwen/Qwen3-0.6B** מ-Hugging Face, תרשום אותו ב-Foundry Local, ותנהל שיחה איתו לגמרי במכשיר שלך.

---

## יעדי הלמידה

בסיום מעבדה זו תוכל:

- להסביר מדוע קימפול מודל מותאם שימושי ומתי ייתכן שתצטרך אותו
- להתקין את ONNX Runtime GenAI Model Builder
- לקמפל מודל Hugging Face לפורמט ONNX מיטבי באמצעות פקודה אחת
- להבין את פרמטרי הקימפול המרכזיים (execution provider, precision)
- ליצור את קובץ התצורה `inference_model.json` של תבנית השיחה
- להוסיף מודל מקומפל למטמון Foundry Local
- להריץ השערה מול המודל המותאם באמצעות CLI, REST API ו-OpenAI SDK

---

## דרישות מוקדמות

| דרישה | פרטים |
|-------------|---------|
| **Foundry Local CLI** | מותקן ובנתיב שלך `PATH` ([חלק 1](part1-getting-started.md)) |
| **Python 3.10+** | דרוש ל-ONNX Runtime GenAI Model Builder |
| **pip** | מנהל החבילות של Python |
| **שטח דיסק** | לפחות 5GB פנוי עבור קבצי המקור והמודל המוקמפל |
| **חשבון Hugging Face** | חלק מהמודלים דורשים אישור רישיון לפני ההורדה. Qwen3-0.6B משתמש ברישיון Apache 2.0 וזמין בחופשיות. |

---

## הגדרת הסביבה

קימפול המודל דורש כמה חבילות Python גדולות (PyTorch, ONNX Runtime GenAI, Transformers). צור סביבה וירטואלית ייעודית כדי שלא יתנגשו עם Python המערכת או פרויקטים אחרים.

```bash
# משורש המאגר
python -m venv .venv
```
  
הפעל את הסביבה:

**Windows (PowerShell):**  
```powershell
.venv\Scripts\Activate.ps1
```
  
**macOS / Linux:**  
```bash
source .venv/bin/activate
```
  
עדכן את pip כדי למנוע בעיות תלות:  

```bash
python -m pip install --upgrade pip
```
  
> **טיפ:** אם כבר יש לך `.venv` ממעבדות קודמות, תוכל להשתמש בו מחדש. רק וודא שהוא מופעל לפני שממשיכים.

---

## מושג: צינור הקימפול

Foundry Local דורש מודלים בפורמט ONNX עם תצורת ONNX Runtime GenAI. רוב מודלים בקוד פתוח ב-Hugging Face מופצים כמשקלי PyTorch או Safetensors, לכן צריך שלב המרה.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### מה עושה בונה המודל?

1. **מוריד** את מודל המקור מ-Hugging Face (או קורא אותו מנתיב מקומי).  
2. **ממיר** את משקלי PyTorch / Safetensors לפורמט ONNX.  
3. **מבצע כימות** למודל בדיוק קטן יותר (למשל int4) כדי להפחית זיכרון ולשפר ביצועים.  
4. **מפיק** את קובץ התצורה של ONNX Runtime GenAI (`genai_config.json`), תבנית השיחה (`chat_template.jinja`), וכל קבצי המטמיע הטוקנים כדי ש-Foundry Local יוכל לטעון ולהגיש את המודל.

### ONNX Runtime GenAI Model Builder לעומת Microsoft Olive

ייתכן ותיתקל בהפניות ל**Microsoft Olive** ככלי חלופי לאופטימיזציית מודלים. שני הכלים יכולים להפיק מודלי ONNX, אך הם משרתים מטרות שונות ומציעים פשרות שונות:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **חבילה** | `onnxruntime-genai` | `olive-ai` |
| **מטרה עיקרית** | המרה וכימות מודלי AI גנרטיביים לתחזוקה ב-ONNX Runtime GenAI | מסגרת אופטימיזציית מודלים מקצה לקצה עם תמיכה בריבוי backends וחומרות |
| **קלות השימוש** | פקודה אחת – המרה וכימות בשלב אחד | מבוסס Workflows – צינורות מרובי שלבים עם YAML/JSON |
| **פורמט פלט** | פורמט ONNX Runtime GenAI (מוכן ל-Foundry Local) | ONNX גנרי, ONNX Runtime GenAI, או פורמטים אחרים בהתאם ל-workflow |
| **יעדי חומרה** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN ועוד |
| **אפשרויות כימות** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, ועוד אופטימיזציות גרפיות וכיול שכבה |
| **תחום מודלים** | מודלי AI גנרטיביים (LLMs, SLMs) | כל מודל ניתן להמרה ל-ONNX (חזון, NLP, אודיו, מולטימודל) |
| **מתאים ביותר ל** | קימפול מהיר של מודל יחיד למטרות השערה מקומית | צינורות הפקה שדורשים שליטה מדויקת על האופטימיזציה |
| **טביעת רגל תלות** | בינונית (PyTorch, Transformers, ONNX Runtime) | גדולה יותר (כולל Olive ויתרונות אופציונליים לפי workflow) |
| **שילוב עם Foundry Local** | ישיר – הפלט תואם מייד ל-Foundry Local | דורש דגל `--use_ort_genai` ותצורה נוספת |

> **למה המעבדה משתמשת ב-Model Builder:** למשימה של קימפול מודל יחיד מ-Hugging Face והרשמתו ל-Foundry Local, Model Builder הוא הדרך הפשוטה והמהימנה ביותר. הוא מייצר את פורמט הפלט המדויק ש-Foundry Local מצפה לו בפקודה אחת. אם תצטרך בעתיד פיצ'רים מתקדמים לאופטימיזציה – כמו כימות מדויק, ניתוח גרפים או כיול מרובה שלבים – Olive תהיה אפשרות חזקה לחקור. עיין ב[תיעוד Microsoft Olive](https://microsoft.github.io/Olive/) לפרטים נוספים.

---

## תרגילים במעבדה

### תרגיל 1: התקנת ONNX Runtime GenAI Model Builder

התקן את חבילת ONNX Runtime GenAI, הכוללת את בונה המודל:

```bash
pip install onnxruntime-genai
```
  
אמת את ההתקנה על ידי בדיקת זמינות בונה המודל:

```bash
python -m onnxruntime_genai.models.builder --help
```
  
עליך לראות פלט עזרה כולל פרמטרים כמו `-m` (שם המודל), `-o` (נתיב הפלט), `-p` (דיוק), ו-`-e` (execution provider).

> **הערה:** בונה המודל תלוי ב-PyTorch, Transformers, ועוד חבילות. ההתקנה עשויה להימשך מספר דקות.

---

### תרגיל 2: קימפול Qwen3-0.6B עבור CPU

הרץ את הפקודה הבאה להורדת מודל Qwen3-0.6B מ-Hugging Face ולקימפולו להערכה ב-CPU עם כימות int4:

**macOS / Linux:**  
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```
  
**Windows (PowerShell):**  
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```
  
#### פירוט הפרמטרים

| פרמטר | מטרה | ערך |
|-----------|---------|------------|
| `-m` | מזהה מודל Hugging Face או נתיב למדריך מקומי | `Qwen/Qwen3-0.6B` |
| `-o` | ספריה לשמירת מודל ONNX המוקמפל | `models/qwen3` |
| `-p` | דיוק הכימות המיושם בקימפול | `int4` |
| `-e` | execution provider של ONNX Runtime (חומרה יעד) | `cpu` |
| `--extra_options hf_token=false` | דילוג על אימות Hugging Face (מתאים למודלים ציבוריים) | `hf_token=false` |

> **כמה זמן זה לוקח?** זמן הקימפול תלוי בחומרה ובגודל המודל. עבור Qwen3-0.6B בכימות int4 על CPU מודרני, זה כ-5 עד 15 דקות. מודלים גדולים יותר לוקחים פרופורציונלית יותר זמן.

בסיום הפקודה תראה ספריית `models/qwen3` עם קובצי המודל המוקמלים. אמת את הפלט:

```bash
ls models/qwen3
```
  
עליך לראות קבצים כולל:  
- `model.onnx` ו-`model.onnx.data` — משקלי המודל המוקמפלים  
- `genai_config.json` — תצורת ONNX Runtime GenAI  
- `chat_template.jinja` — תבנית השיחה של המודל (נוצרה אוטומטית)  
- `tokenizer.json`, `tokenizer_config.json` — קבצי מטמיע טוקנים  
- קבצי אוצר מילים ותצורה נוספים

---

### תרגיל 3: קימפול עבור GPU (אופציונלי)

אם יש לך כרטיס NVIDIA עם תמיכת CUDA, תוכל לקמפל גרסה מותאמת GPU להערכה מהירה יותר:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```
  
> **הערה:** קימפול GPU דורש `onnxruntime-gpu` והתקנת CUDA תקינה. אם אינם מותקנים, בונה המודל ידווח על שגיאה. ניתן לדלג על תרגיל זה ולהמשיך עם גרסת ה-CPU.

#### התייחסות לקימפול לפי חומרה

| יעד | execution provider (`-e`) | דיוק מומלץ (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` או `int4` |
| DirectML (GPU Windows) | `dml` | `fp16` או `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### פשרות דיוק

| דיוק | גודל | מהירות | איכות |
|-----------|------|-------|---------|
| `fp32` | הגדול ביותר | האיטי ביותר | דיוק גבוה מאוד |
| `fp16` | גדול | מהיר (GPU) | דיוק טוב מאוד |
| `int8` | קטן | מהיר | אובדן דיוק קל |
| `int4` | הקטן ביותר | המהיר ביותר | אובדן דיוק בינוני |

לרוב פיתוחים מקומיים, `int4` על CPU מספק את האיזון הטוב ביותר בין מהירות ומשאבים. ליציאה באיכות הפקה, מומלץ `fp16` על GPU עם CUDA.

---

### תרגיל 4: יצירת תבנית שיחה

בונה המודל יוצר אוטומטית קובץ `chat_template.jinja` וקובץ `genai_config.json` בתיקיית הפלט. עם זאת, Foundry Local זקוק גם לקובץ `inference_model.json` שמגדיר כיצד לעצב את ההנחות למודל שלך. קובץ זה מגדיר את שם המודל ואת תבנית ההנחות העוטפת את הודעות המשתמש בטוקנים מיוחדים נכונים.

#### שלב 1: בדוק את הפלט המוקמפלים

רשום את תוכן התיקייה של המודל המוקמפלים:

```bash
ls models/qwen3
```
  
עליך לראות קבצים כמו:  
- `model.onnx` ו-`model.onnx.data` — משקלי המודל  
- `genai_config.json` — תצורת ONNX Runtime GenAI (נוצר אוטומטית)  
- `chat_template.jinja` — תבנית השיחה של המודל (נוצרה אוטומטית)  
- `tokenizer.json`, `tokenizer_config.json` — קבצי מטמיע הטוקנים  
- קבצים אחרים של אוצר מילים ותצורה

#### שלב 2: צור את קובץ inference_model.json

קובץ `inference_model.json` מספר ל-Foundry Local כיצד לעצב הנחות. צור סקריפט Python בשם `generate_chat_template.py` **בשורש המחסן** (אותה ספריה המכילה את התיקייה `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# לבנות שיחה מינימלית כדי להפיק את תבנית הצ'אט
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# לבנות את מבנה inference_model.json
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```
  
הרץ את הסקריפט משורש המחסן:

```bash
python generate_chat_template.py
```
  
> **הערה:** חבילת `transformers` כבר מותקנת כתלות ב-`onnxruntime-genai`. אם תראה `ImportError`, הרץ קודם `pip install transformers`.

הסקריפט מייצר קובץ `inference_model.json` בתוך תיקיית `models/qwen3`. הקובץ מסביר ל-Foundry Local כיצד לעטוף את קלט המשתמש בטוקנים מיוחדים נכונים עבור Qwen3.

> **חשוב:** השדה `"Name"` בקובץ `inference_model.json` (שהוגדר כ-`qwen3-0.6b` בסקריפט) הוא **הכינוי של המודל** שתשתמש בו בכל הפקודות וקריאות ה-API הבאות. אם תשנה שם זה, עדכן את שם המודל בתרגילים 6–10 בהתאם.

#### שלב 3: אמת את התצורה

פתח את הקובץ `models/qwen3/inference_model.json` ואמת שהוא מכיל שדה `Name` ואובייקט `PromptTemplate` עם מפתחות `assistant` ו-`prompt`. תבנית ההנחות צריכה לכלול טוקנים מיוחדים כגון `<|im_start|>` ו-`<|im_end|>` (הטוקנים המדויקים תלויים בתבנית השיחה של המודל).

> **אפשרות ידנית:** אם אינך רוצה להפעיל את הסקריפט, אפשר ליצור את הקובץ באופן ידני. הדרישה המרכזית היא ששדה ה-`prompt` יכיל את תבנית השיחה המלאה של המודל עם `{Content}` כמקום להודעת המשתמש.

---

### תרגיל 5: אמת את מבנה ספריית המודל
בונה המודל ממקם את כל הקבצים המוגמרים ישירות בתיקיית הפלט שצוינה על ידך. אמת שהמבנה הסופי נראה תקין:

```bash
ls models/qwen3
```

התיקייה אמורה להכיל את הקבצים הבאים:

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **הערה:** שלא כמו כמה כלי הידור אחרים, בונה המודל אינו יוצר תיקיות משנה מקוננות. כל הקבצים נמצאים ישירות בתיקיית הפלט, וזה בדיוק מה ש-Foundry Local מצפה לו.

---

### תרגיל 6: הוסף את המודל אל מטמון Foundry Local

הודע ל-Foundry Local היכן ניתן למצוא את המודל המוגמר על ידי הוספת התיקייה למטמון שלו:

```bash
foundry cache cd models/qwen3
```

אמת שהמודל מופיע במטמון:

```bash
foundry cache ls
```

אתה אמור לראות את המודל המותאם אישית שלך מופיע לצד כל המודלים שנשמרו במטמון קודם (כגון `phi-3.5-mini` או `phi-4-mini`).

---

### תרגיל 7: הרץ את המודל המותאם באמצעות ה-CLI

הפעל חוויית שיחה אינטראקטיבית עם המודל שהרכבת זה עתה (הכינוי `qwen3-0.6b` מגיע מהשדה `Name` שקבעת בקובץ `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

הדגל `--verbose` מציג מידע אבחוני נוסף, שהוא מועיל כשבודקים מודל מותאם בפעם הראשונה. אם המודל נטען בהצלחה, תראה הנחיה אינטראקטיבית. נסה כמה הודעות:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

הקלד `exit` או לחץ על `Ctrl+C` כדי לסיים את הסשן.

> **פתרון בעיות:** אם המודל נכשל בטעינה, בדוק את הדברים הבאים:
> - קובץ `genai_config.json` נוצר על ידי בונה המודל.
> - קובץ `inference_model.json` קיים ותקין כ-JSON.
> - קבצי מודל ONNX נמצאים בתיקייה הנכונה.
> - יש לך מספיק זיכרון RAM פנוי (Qwen3-0.6B int4 זקוק בערך ל-1GB).
> - Qwen3 הוא מודל הסקה שמייצר תגים מסוג `<think>`. אם אתה רואה תגי `<think>...</think>` מקדימים לתגובות, זה התנהגות נורמלית. תבנית ההנחיה בקובץ `inference_model.json` ניתנת להתאמה כדי לדכא את פלט המחשבה.

---

### תרגיל 8: שאילתא אל המודל המותאם דרך REST API

אם יצאת מהסשן האינטראקטיבי בתרגיל 7, ייתכן שהמודל כבר לא נטען. הפעל את שירות Foundry Local וטעון את המודל תחילה:

```bash
foundry service start
foundry model load qwen3-0.6b
```

בדוק באיזה פורט השירות רץ:

```bash
foundry service status
```

ואז שלח בקשה (החלף את `5273` בפורט האמיתי אם שונה):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **הערת Windows:** פקודת `curl` למעלה משתמשת בסינטקס של bash. ב-Windows השתמש במקום זאת ב-cmdlet של PowerShell `Invoke-RestMethod`.

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### תרגיל 9: השתמש במודל המותאם עם OpenAI SDK

אתה יכול להתחבר למודל המותאם בדיוק עם קוד OpenAI SDK שבו השתמשת למודלים המובנים (ראה [חלק 3](part3-sdk-and-apis.md)). ההבדל היחיד הוא שם המודל.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local לא מאמת מפתחות API
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local אינו מאמת מפתחות API
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **נקודה חשובה:** מאחר ש-Foundry Local חושף API תואם OpenAI, כל קוד שעובד עם המודלים המובנים עובד גם עם המודלים המותאמים. כל מה שצריך הוא לשנות את הפרמטר `model`.

---

### תרגיל 10: בדוק את המודל המותאם עם Foundry Local SDK

בניסויים קודמים השתמשת ב-Foundry Local SDK כדי להפעיל את השירות, לגלות את נקודת הקצה ולנהל מודלים באופן אוטומטי. אתה יכול לעקוב בדיוק אחרי אותה תבנית עם המודל שהרכבת. ה-SDK מטפל בהפעלת השירות ובגילוי נקודת הקצה, לכן הקוד שלך אינו צריך לקבוע את `localhost:5273` בקוד קשיח.

> **הערה:** ודא שה-Foundry Local SDK מותקן לפני שתפעיל את הדוגמאות האלה:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** הוסף את חבילות NuGet `Microsoft.AI.Foundry.Local` ו-`OpenAI`
>
> שמור כל סקריפט **בשורש מאגר הקוד** (בתיקייה זהה לתיקיית `models/` שלך).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# שלב 1: הפעל את שירות Foundry Local וטעין את המודל המותאם אישית
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# שלב 2: בדוק את המטמון עבור המודל המותאם אישית
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# שלב 3: טען את המודל לזיכרון
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# שלב 4: צור לקוח OpenAI באמצעות נקודת הקצה שהתגלתה על ידי ה-SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# שלב 5: שלח בקשת השלמת שיחה בזרימה
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

הרץ אותו:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// שלב 1: התחל את שירות Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// שלב 2: קבל את הדגם המותאם מהקטלוג
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// שלב 3: טען את הדגם לזיכרון
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// שלב 4: צור לקוח OpenAI באמצעות נקודת הקצה שנמצאה על ידי SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// שלב 5: שלח בקשת השלמת שיחה בזרם
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

הרץ אותו:

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **נקודה חשובה:** Foundry Local SDK מגלה את נקודת הקצה בצורה דינמית, ולכן לעולם אינך קובע מספר פורט בקוד. זו הגישה המומלצת ליישומים בייצור. המודל שהרכבת עובד זהה למודלים מקטלוג מובנים דרך ה-SDK.

---

## בחירת מודל להרכבה

Qwen3-0.6B משמש כדוגמה מרכזית במעבדה זו כי הוא קטן, מהיר להרכבה וזמין בחינם תחת רישיון Apache 2.0. עם זאת, ניתן להרכיב מודלים רבים אחרים. הנה כמה הצעות:

| מודל | Hugging Face מזהה | פרמטרים | רישיון | הערות |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | קטן מאוד, הידור מהיר, טוב לבדיקות |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | איכות טובה יותר, עדיין מהיר להרכבה |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | איכות חזקה, דורש זיכרון RAM נוסף |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | דורש אישור רישיון ב-Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | איכות גבוהה, הורדה גדולה והרכבה ארוכה יותר |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | כבר נמצא בקטלוג Foundry Local (שימושי להשוואה) |

> **תזכורת רישיון:** תמיד בדוק את רישיון המודל ב-Hugging Face לפני השימוש. חלק מהמודלים (כגון Llama) מחייבים אישור הסכם רישיון ואימות באמצעות `huggingface-cli login` לפני ההורדה.

---

## עקרונות: מתי להשתמש במודלים מותאמים אישית

| תרחיש | מדוע להרכיב לבד? |
|----------|----------------------|
| **מודל שאתה צריך אינו בקטלוג** | קטלוג Foundry Local מנוהל בקפידה. אם המודל הרצוי אינו ברשימה, הרכב אותו בעצמך. |
| **מודלים עם למידה מותאמת** | אם כיוונת מודל על נתונים ספציפיים לתחום, יש צורך להדר בעצמך את המשקלים. |
| **דרישות כימות מיוחדות** | ייתכן שתרצה רמת דיוק או אסטרטגיית כימות השונה מהברירת מחדל בקטלוג. |
| **שחרורי מודלים חדשים** | כשמודל חדש מופיע ב-Hugging Face, ייתכן שהוא עדיין לא בקטלוג Foundry Local. הרכבה עצמית מביאה גישה מיידית. |
| **מחקר וניסויים** | לנסות ארכיטקטורות, גדלים או תצורות מודל שונות מקומית לפני התחייבות לייצור. |

---

## סיכום

במעבדה זו למדת כיצד:

| שלב | מה עשית |
|------|-------------|
| 1 | התקנת את ONNX Runtime GenAI model builder |
| 2 | הרכבת את `Qwen/Qwen3-0.6B` מ-Hugging Face למודל ONNX מוקצן |
| 3 | יצרת קובץ תצורת תבנית שיחה `inference_model.json` |
| 4 | הוספת את המודל המוקצן למטמון Foundry Local |
| 5 | הרצת שיחה אינטראקטיבית עם המודל המותאם דרך ה-CLI |
| 6 | שאלת את המודל באמצעות REST API תואם OpenAI |
| 7 | התחברת מפייתון, JavaScript ו-C# בעזרת OpenAI SDK |
| 8 | בדקת את המודל המותאם מקצה לקצה עם Foundry Local SDK |

הנקודה המרכזית היא ש**כל מודל מבוסס טרנספורמר יכול לפעול דרך Foundry Local** ברגע שהורכב לפורמט ONNX. ה-API התואם OpenAI הופך את כל קוד היישום הקיים לעבוד ללא שינויים; כל מה שצריך זה להחליף את שם המודל.

---

## נקודות מפתח

| עיקרון | פרטים |
|---------|--------|
| ONNX Runtime GenAI Model Builder | ממיר מודלים מ-Hugging Face לפורמט ONNX עם כימות בפקודה אחת |
| פורמט ONNX | Foundry Local דורש מודלים בפורמט ONNX עם תצורת ONNX Runtime GenAI |
| תבניות שיחה | קובץ `inference_model.json` מודיע ל-Foundry Local איך לעצב הנחיות למודל נתון |
| יעדי חומרה | הידור עבור CPU, GPU של NVIDIA (CUDA), DirectML (GPU ב-Windows), או WebGPU תלוי בחומרה שלך |
| כימות | דיוק נמוך יותר (int4) מצמצם גודל ומשפר מהירות על חשבון מעט דיוק; fp16 שומר על איכות גבוהה ב-GPU |
| תאימות API | מודלים מותאמים משתמשים באותו API תואם OpenAI כמו מודלים מובנים |
| Foundry Local SDK | ה-SDK מטפל בהפעלת השירות, גילוי נקודת קצה וטעינת מודלים באופן אוטומטי למודלים מקטלוג ומותאמים |

---

## המשך קריאה

| משאב | קישור |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| מדריך מודלים מותאמים ל-Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| משפחת המודל Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| תיעוד Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## שלבים הבאים

המשך ל[חלק 11: קריאה לכלים עם מודלים מקומיים](part11-tool-calling.md) כדי ללמוד איך לאפשר למודלים המקומיים שלך לקרוא לפונקציות חיצוניות.

[← חלק 9: תמלול קולי Whisper](part9-whisper-voice-transcription.md) | [חלק 11: קריאת כלים →](part11-tool-calling.md)