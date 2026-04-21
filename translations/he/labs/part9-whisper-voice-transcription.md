![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# חלק 9: תמלול קול עם Whisper ו-Foundry Local

> **מטרה:** להשתמש בדגם OpenAI Whisper שרץ מקומית דרך Foundry Local כדי לתמלל קבצי שמע - כל זה במכשיר בלבד, ללא צורך בענן.

## סקירה כללית

Foundry Local הוא לא רק ליצירת טקסט; הוא תומך גם בדגמי **דיבור לטקסט**. במעבדה זו תשתמשו בדגם **OpenAI Whisper Medium** כדי לתמלל קבצי שמע כלילית על המחשב שלכם. זה אידיאלי למצבים כמו תמלול שיחות שירות לקוחות של זאבה, הקלטות סקירות מוצרים או תכנוני סדנה שבהם נתוני שמע אסור שיצאו מהמכשיר.

---

## מטרות הלמידה

בסיום מעבדה זו תוכלו:

- להבין את דגם הדיבור לטקסט Whisper ואת יכולותיו
- להוריד ולהריץ את דגם Whisper באמצעות Foundry Local
- לתמלל קבצי שמע באמצעות SDK של Foundry Local בפייתון, ג'אווהסקריפט ו-C#
- לבנות שירות תמלול פשוט הפועל כלילית במכשיר
- להבין את ההבדלים בין דגמי צ'אט/טקסט לדגמי שמע ב-Foundry Local

---

## דרישות מוקדמות

| דרישה | פרטים |
|-------------|---------|
| **Foundry Local CLI** | גרסה **0.8.101 ומעלה** (דגמי Whisper זמינים מגרסה v0.8.101 ואילך) |
| **מערכת הפעלה** | Windows 10/11 (x64 או ARM64) |
| **סביבת ריצה** | **Python 3.9+** ו/או **Node.js 18+** ו/או **.NET 9 SDK** ([הורד .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **הושלם** | [חלק 1: התחלה](part1-getting-started.md), [חלק 2: מעמיק ב-SDK של Foundry Local](part2-foundry-local-sdk.md), ו-[חלק 3: SDK ו-API](part3-sdk-and-apis.md) |

> **הערה:** דגמי Whisper חייבים להיות מורדים דרך ה-**SDK** (לא דרך CLI). ה-CLI אינו תומך בנקודת הקצה לתמלול שמע. בדקו את הגרסה שלכם עם:
> ```bash
> foundry --version
> ```

---

## מושג: כיצד Whisper פועל עם Foundry Local

דגם OpenAI Whisper הוא דגם זיהוי דיבור כללי שאומן על מערך נתונים גדול ומגוון של שמע. כשמריצים אותו דרך Foundry Local:

- הדגם רץ **כלילית על המעבד (CPU)** - אין צורך ב-GPU
- השמע אינו יוצא מהמכשיר שלכם - **פרטיות מלאה**
- SDK של Foundry Local מטפל בהורדה וניהול מטמון של הדגם
- **JavaScript ו-C#** מספקים לקוח `AudioClient` מובנה ב-SDK שמטפל בכל תהליך התמלול — אין צורך בהגדרת ONNX ידנית
- **Python** משתמש ב-SDK לניהול דגם וב-ONNX Runtime להפעלה ישירה מול דגמי הקידוד/פענוח ONNX

### כיצד פועל תהליך העבודה (JavaScript ו-C#) — SDK AudioClient

1. **SDK של Foundry Local** מוריד ושומר במטמון את דגם Whisper
2. `model.createAudioClient()` (JS) או `model.GetAudioClientAsync()` (C#) יוצרים `AudioClient`
3. `audioClient.transcribe(path)` (JS) או `audioClient.TranscribeAudioAsync(path)` (C#) מטפלים בכל התהליך פנימית — עיבוד מקדים של שמע, מקודד, מפענח, ופענוח טוקנים
4. `AudioClient` מציג מאפיין `settings.language` (נקבע ל-`"en"` לאנגלית) להכוונת תמלול מדויק

### כיצד פועל תהליך העבודה (Python) — ONNX Runtime

1. **SDK של Foundry Local** מוריד ושומר במטמון את קבצי דגם ONNX של Whisper
2. **עיבוד מקדים של שמע** ממיר שמע WAV לספקטרוגרמה מל (80 סלילי מל x 3000 פריימים)
3. **מקודד** מעבד את ספקטרוגרמת המל ומפיק מצבים נסתרים יחד עם טנסורים של מפתח/ערך בקשת צולבת
4. **מפענח** פועל אוטורגרסיבי, יוצר טוקן אחד בכל פעם עד שמפיק טוקן סוף טקסט
5. **מקודד טוקנים** מפענח את מזהי הטוקן שהופקו חזרה לטקסט קריא

### וריאנטים של דגם Whisper

| כינוי | מזהה דגם | מכשיר | גודל | תיאור |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | מואץ GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | מותאם ל-CPU (מומלץ לרוב המכשירים) |

> **הערה:** בניגוד לדגמי צ'אט שמופיעים כברירת מחדל, דגמי Whisper ממוקמים תחת המשימה `automatic-speech-recognition`. השתמשו ב-`foundry model info whisper-medium` כדי לראות פרטים.

---

## תרגילים במעבדה

### תרגיל 0 - השגת דגימות שמע

במעבדה זו קיימים קבצי WAV מוכנים המבוססים על תרחישי מוצר של זאבה DIY. צרו אותם עם הסקריפט הכלול:

```bash
# מהשורש של המאגר - קודם כל צור והפעל סביבת וירטואלית .venv
python -m venv .venv

# ווינדוס (PowerShell):
.venv\Scripts\Activate.ps1
# מק-אוס:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

זה יוצר שישה קבצי WAV בנתיב `samples/audio/`:

| קובץ | תרחיש |
|------|----------|
| `zava-customer-inquiry.wav` | שיחה של לקוח על **מקצוע נטען Zava ProGrip** |
| `zava-product-review.wav` | סקירת מוצר של **צבע פנימי Zava UltraSmooth** |
| `zava-support-call.wav` | שיחת תמיכה על **ארגז כלי TitanLock של Zava** |
| `zava-project-planning.wav` | תכנון דק ע"י DIY עם **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | סיור בסדנה המשתמש ב**חמישה מוצרי Zava** |
| `zava-full-project-walkthrough.wav` | סיור מורחב בשיפוץ המוסך עם **כל מוצרי Zava** (~4 דקות, לבדיקה של שמע ארוך) |

> **טיפ:** אפשר גם להשתמש בקבצי WAV/MP3/M4A משלכם, או להקליט עם Windows Voice Recorder.

---

### תרגיל 1 - הורדת דגם Whisper באמצעות SDK

עקב חוסר תאימות של CLI עם דגמי Whisper בגרסאות חדשות של Foundry Local, השתמשו ב-**SDK** להורדה וטעינת הדגם. בחרו בשפת התכנות:

<details>
<summary><b>🐍 Python</b></summary>

**התקנת ה-SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# התחל את השירות
manager = FoundryLocalManager()
manager.start_service()

# בדוק מידע קטלוג
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# בדוק אם כבר מאוחסן במטמון
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# טען את המודל לזיכרון
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

שמרו כ- `download_whisper.py` והריצו:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**התקנת ה-SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// צור מנהל והפעל את השירות
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// קבל דגם מהקטלוג
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.id}`);

if (model.isCached) {
  console.log("Whisper model already downloaded.");
} else {
  console.log("Downloading Whisper model (this may take several minutes)...");
  await model.download();
  console.log("Download complete.");
}

// טען את הדגם לזיכרון
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

שמרו כ- `download-whisper.mjs` והריצו:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**התקנת ה-SDK:**
```bash
dotnet add package Microsoft.AI.Foundry.Local
```

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "whisper-medium";

// Start the service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);
Console.WriteLine($"Model: {model.Id}");

// Check if already cached
var isCached = await model.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine("Whisper model already downloaded.");
}
else
{
    Console.WriteLine("Downloading Whisper model (this may take several minutes)...");
    await model.DownloadAsync(null, default);
    Console.WriteLine("Download complete.");
}

// Load the model into memory
await model.LoadAsync(default);
Console.WriteLine($"Whisper model loaded: {model.Id}");
```

</details>

> **למה SDK ולא CLI?** CLI של Foundry Local אינו תומך בהורדה או בשירות דגמי Whisper ישירות. ה-SDK מספק דרך אמינה להוריד ולנהל דגמי שמע בצורה תוכניתית. SDK ב-JavaScript וב-C# כולל `AudioClient` מובנה שמטפל בכל תהליך התמלול. Python משתמש ב-ONNX Runtime להפעלה ישירה של הדגמים השמורים.

---

### תרגיל 2 - מבנה ה-SDK של Whisper

תמלול ב-Whisper משתמש בגישות שונות לפי השפה. **JavaScript ו-C#** מספקים `AudioClient` מובנה ב-SDK שמטפל בכל התהליך (עיבוד שמע מראש, מקודד, מפענח, ופענוח טוקנים) בשיטה אחת. **Python** משתמש ב-SDK לניהול הדגם ו-ONNX Runtime להפעלה ישירה מול דגמי ONNX של מקודד ומפענח.

| רכיב | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **חבילות SDK** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **ניהול דגם** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + קטלוג |
| **חלוץ מאפיינים** | `WhisperFeatureExtractor` + `librosa` | מטופל על ידי `AudioClient` ב-SDK | מטופל על ידי `AudioClient` ב-SDK |
| **הסקה** | `ort.InferenceSession` (מקודד + מפענח) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **פענוח טוקנים** | `WhisperTokenizer` | מטופל על ידי `AudioClient` ב-SDK | מטופל על ידי `AudioClient` ב-SDK |
| **הגדרת שפה** | נקבע דרך `forced_ids` בטוקנים של המפענח | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **קלט** | נתיב קובץ WAV | נתיב קובץ WAV | נתיב קובץ WAV |
| **פלט** | מחרוזת טקסט מפוענחת | `result.text` | `result.Text` |

> **חשוב:** תמיד הגדירו שפה על האובייקט `AudioClient` (למשל `"en"` לאנגלית). בלי הגדרת שפה מפורשת, הדגם עלול להפיק פלט שגוי כשמנסה לזהות את השפה אוטומטית.

> **תבניות SDK:** Python משתמש ב-`FoundryLocalManager(alias)` לאתחול, אחר כך `get_cache_location()` למציאת קבצי הדגם ONNX. JavaScript ו-C# משתמשים ב-`AudioClient` מובנה ב-SDK שהושג על ידי `model.createAudioClient()` (JS) או `model.GetAudioClientAsync()` (C#) — המטפל בכל תהליך התמלול. ראו [חלק 2: מעמיק ב-SDK של Foundry Local](part2-foundry-local-sdk.md) לפרטים מלאים.

---

### תרגיל 3 - בניית אפליקציית תמלול פשוטה

בחרו בשפת התכנות הרצויה ובנו אפליקציה מינימלית לתמלול קובץ שמע.

> **פורמטים נתמכים:** WAV, MP3, M4A. לתוצאות מיטביות, השתמשו בקבצי WAV בקצב דגימה 16kHz.

<details>
<summary><h3>מסלול Python</h3></summary>

#### הכנה

```bash
cd python
python -m venv venv

# הפעל את סביבת העבודה הווירטואלית:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### קוד לתמלול

צרו את הקובץ `foundry-local-whisper.py`:

```python
import sys
import os
import numpy as np
import onnxruntime as ort
import librosa
from transformers import WhisperFeatureExtractor, WhisperTokenizer
from foundry_local import FoundryLocalManager

model_alias = "whisper-medium"
audio_file = sys.argv[1] if len(sys.argv) > 1 else "sample.wav"

if not os.path.exists(audio_file):
    print(f"Audio file not found: {audio_file}")
    sys.exit(1)

# שלב 1: אתחול - מפעיל שירות, מוריד ונטען את המודל
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# בניית נתיב לקבצי המודל ONNX המוחזקים במטמון
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# שלב 2: טעינת מפגשי ONNX ומחלץ התכונות
encoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_encoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
decoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_decoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
fe = WhisperFeatureExtractor.from_pretrained(model_dir)
tokenizer = WhisperTokenizer.from_pretrained(model_dir)

# שלב 3: חילוץ תכונות ספקטרוגרמת מל
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# שלב 4: הרצת המקודד
enc_out = encoder.run(None, {"audio_features": input_features})
# הפלט הראשון הוא מצבים נסתרים; השאר הם זוגות KV של תשומת לב חוצה
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# שלב 5: פענוח אוטורגסי
initial_tokens = [50258, 50259, 50359, 50363]  # sot, אנגלית, תמלול, ללא חותמות זמן
input_ids = np.array([initial_tokens], dtype=np.int32)

# מטמון KV ריק לתשומת לב עצמית
self_kv = {}
for i in range(24):
    self_kv[f"past_key_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)
    self_kv[f"past_value_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)

generated = []
for _ in range(448):
    feeds = {"input_ids": input_ids, **cross_kv, **self_kv}
    outputs = decoder.run(None, feeds)
    logits = outputs[0]
    next_token = int(np.argmax(logits[0, -1, :]))

    if next_token == 50257:  # סוף הטקסט
        break
    generated.append(next_token)

    # עדכון מטמון KV של תשומת לב עצמית
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### הרצה

```bash
# תעתיק תרחיש מוצר של זאבה
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# או נסה אחרים:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### נקודות מפתח בפייתון

| שיטה | מטרה |
|--------|---------|
| `FoundryLocalManager(alias)` | אתחול: הפעלת השירות, הורדה, וטעינת הדגם |
| `manager.get_cache_location()` | השגת הנתיב לקבצי הדגם ONNX במטמון |
| `WhisperFeatureExtractor.from_pretrained()` | טעינת חלוץ ספקטרוגרמת מל |
| `ort.InferenceSession()` | יצירת סשנים של ONNX Runtime למקודד ומפענח |
| `tokenizer.decode()` | המרת מזהי טוקנים שהופקו חזרה לטקסט |

</details>

<details>
<summary><h3>מסלול JavaScript</h3></summary>

#### הכנה

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### קוד לתמלול

צרו קובץ `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// שלב 1: אתחול - יצירת מנהל, הפעלת שירות וטעינת המודל
console.log(`Initialising Foundry Local with model: ${modelAlias}...`);
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);

if (!model.isCached) {
  console.log("Downloading Whisper model...");
  await model.download();
}
await model.load();

// שלב 2: יצירת לקוח שמע ותמלול
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// ניקוי
await model.unload();
```

> **הערה:** SDK של Foundry Local מספק `AudioClient` מובנה באמצעות `model.createAudioClient()` שמטפל בכל התהליך של ONNX בדחיפה פנימית — לא צריך לייבא `onnxruntime-node`. תמיד הגדירו `audioClient.settings.language = "en"` כדי להבטיח תמלול מדויק באנגלית.

#### הרצה

```bash
# לתמלל תרחיש מוצר של זאבה
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# או לנסות אחרים:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### נקודות מפתח בג'אווהסקריפט

| שיטה | מטרה |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | יצירת מנהל יחיד |
| `await catalog.getModel(alias)` | קבלת דגם מהקטלוג |
| `model.download()` / `model.load()` | הורדה וטעינת דגם Whisper |
| `model.createAudioClient()` | יצירת לקוח שמע לתמלול |
| `audioClient.settings.language = "en"` | הגדרת שפת התמלול (נדרש לפלט מדויק) |
| `audioClient.transcribe(path)` | תמלול קובץ שמע, מחזיר `{ text, duration }` |

</details>

<details>
<summary><h3>מסלול C#</h3></summary>

#### הכנה

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **הערה:** מסלול C# משתמש בחבילת `Microsoft.AI.Foundry.Local` המספקת `AudioClient` מובנה דרך `model.GetAudioClientAsync()`. זה מטפל בתהליך התמלול המלא בתהליך — ללא צורך בהגדרת ONNX Runtime נפרדת.

#### קוד לתמלול

החליפו את תוכן `Program.cs`:

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// --- Configuration ---
var modelAlias = "whisper-medium";
var audioFile = args.Length > 0 ? args[0] : "sample.wav";

if (!File.Exists(audioFile))
{
    Console.WriteLine($"Audio file not found: {audioFile}");
    Console.WriteLine("Usage: dotnet run <path-to-audio-file>");
    return;
}

// --- Step 1: Initialize Foundry Local ---
Console.WriteLine("Initializing Foundry Local...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// --- Step 2: Load the Whisper model ---
Console.WriteLine($"Loading model: {modelAlias}...");
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading model...");
    await model.DownloadAsync(null, default);
}

// Load model into memory
Console.WriteLine("Loading model into memory...");
await model.LoadAsync(default);

// --- Step 3: Transcribe audio ---
Console.WriteLine($"Transcribing: {audioFile}");

var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en";

var result = await audioClient.TranscribeAudioAsync(audioFile);

Console.WriteLine("\n--- Transcription ---");
Console.WriteLine(result.Text);
Console.WriteLine("---------------------");
```

#### הרצה

```bash
# תעד תרחיש מוצר של זאבה
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# או נסה אחרים:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### נקודות מפתח ב-C#

| שיטה | מטרה |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | אתחול Foundry Local עם הגדרות |
| `catalog.GetModelAsync(alias)` | קבלת דגם מהקטלוג |
| `model.DownloadAsync()` | הורדת דגם Whisper |
| `model.GetAudioClientAsync()` | קבלת AudioClient (לא ChatClient!) |
| `audioClient.Settings.Language = "en"` | הגדרת שפת התמלול (נדרש לפלט מדויק) |
| `audioClient.TranscribeAudioAsync(path)` | תמלול קובץ שמע |
| `result.Text` | הטקסט המתומלל |
> **C# מול Python/JS:** חבילת ה-SDK של C# מספקת `AudioClient` מובנה לתמלול בתהליך באמצעות `model.GetAudioClientAsync()`, בדומה ל-SDK של JavaScript. Python משתמשת ב-ONNX Runtime ישירות לביצוע האינפרנס מול דגמי המוצפן/מפענח שמאוחסנים במטמון.

</details>

---

### תרגיל 4 - תמלול באצווה של כל דגימות זאבה

כעת כשיש לכם אפליקציית תמלול עובדת, תמללו את כל חמשת הקבצים של דגימות זאבה והשוו את התוצאות.

<details>
<summary><h3>מסלול Python</h3></summary>

הדוגמה המלאה `python/foundry-local-whisper.py` כבר תומכת בתמלול באצווה. כשהיא מורצת ללא ארגומנטים, היא מתמללת את כל קבצי `zava-*.wav` בתיקיית `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

הדוגמה משתמשת ב-`FoundryLocalManager(alias)` לאתחול, ואז מריצה את סשני ONNX של המוצפן והמפענח עבור כל קובץ.

</details>

<details>
<summary><h3>מסלול JavaScript</h3></summary>

הדוגמה המלאה `javascript/foundry-local-whisper.mjs` כבר תומכת בתמלול באצווה. כשהיא מורצת ללא ארגומנטים, היא מתמללת את כל קבצי `zava-*.wav` בתיקיית `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

הדוגמה משתמשת ב-`FoundryLocalManager.create()` ו-`catalog.getModel(alias)` לאתחול ה-SDK, ואז משתמשת ב-`AudioClient` (בהגדרת `settings.language = "en"`) כדי לתמלל כל קובץ.

</details>

<details>
<summary><h3>מסלול C#</h3></summary>

הדוגמה המלאה `csharp/WhisperTranscription.cs` כבר תומכת בתמלול באצווה. כשמריצים בלי ארגומנט קובץ ספציפי, היא מתמללת את כל קבצי `zava-*.wav` בתיקיית `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

הדוגמה משתמשת ב-`FoundryLocalManager.CreateAsync()` וב-`AudioClient` של ה-SDK (בהגדרת `Settings.Language = "en"`) לתמלול בתהליך.

</details>

**מה כדאי לבדוק:** השוו את פלט התמלול לטקסט המקורי ב-`samples/audio/generate_samples.py`. עד כמה Whisper מתעד נכון שמות מוצרים כמו "Zava ProGrip" ומונחים טכניים כמו "brushless motor" או "composite decking"?

---

### תרגיל 5 - הבנת דפוסי קוד מרכזיים

למדו כיצד תמלול Whisper שונה מהשלמות צ'אט בשלוש השפות:

<details>
<summary><b>Python - הבדלים מרכזיים מצ'אט</b></summary>

```python
# השלמת שיחה (חלקים 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# תמלול אודיו (חלק זה):
# משתמש ב-ONNX Runtime ישירות במקום בלקוח OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... לולאת דקדוק אוטורגרסיבית ...
print(tokenizer.decode(generated_tokens))
```

**תובנה מרכזית:** מודלים לצ'אט משתמשים ב-API התואם ל-OpenAI דרך `manager.endpoint`. Whisper משתמש ב-SDK כדי לאתר את מודלי ONNX שהוקצו למטמון, ואז מבצע אינפרנס ישירות עם ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - הבדלים מרכזיים מצ'אט</b></summary>

```javascript
// השלמת שיחה (חלקים 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// תמלול שמע (חלק זה):
// משתמש ב-AudioClient המובנה ב-SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // תמיד קבע שפה לתוצאות מיטביות
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**תובנה מרכזית:** מודלים לצ'אט משתמשים ב-API התואם ל-OpenAI דרך `manager.urls[0] + "/v1"`. תמלול Whisper משתמש ב-`AudioClient` של ה-SDK, שמתקבל מ-`model.createAudioClient()`. יש להגדיר את `settings.language` כדי למנוע פלט מקודד שגוי בזיהוי אוטומטי.

</details>

<details>
<summary><b>C# - הבדלים מרכזיים מצ'אט</b></summary>

הגישה ב-C# משתמשת ב-`AudioClient` המובנה ב-SDK לתמלול בתהליך:

**איתחול מודל:**

```csharp
// 1. Create the manager with configuration
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// 2. Get model from catalog, download, and load
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("whisper-medium", default);
await model.DownloadAsync(null, default);
await model.LoadAsync(default);
```

**תמלול:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**תובנה מרכזית:** C# משתמש ב-`FoundryLocalManager.CreateAsync()` ומקבל `AudioClient` ישירות — אין צורך בהגדרת ONNX Runtime. יש להגדיר `Settings.Language` כדי למנוע פלט מקודד שגוי בזיהוי אוטומטי.

</details>

> **סיכום:** Python משתמשת ב-Foundry Local SDK לניהול מודלים וב-ONNX Runtime לאינפרנס ישיר מול דגמי המוצפן/מפענח. JavaScript ו-C# משתמשים ב-`AudioClient` המובנה ב-SDK לתמלול פשוט — יוצרים את הלקוח, מגדירים את השפה, וקוראים ל-`transcribe()` / `TranscribeAudioAsync()`. תמיד הגדירו את שפת ה-AudioClient לתוצאות מדויקות.

---

### תרגיל 6 - התנסות

נסו את השינויים הללו להעמקת ההבנה:

1. **נסו קבצי שמע שונים** - הקליטו את עצמכם מדברים בעזרת Windows Voice Recorder, שמרו כ-WAV, ותמללו

2. **השוו בין וריאנטים של מודל** - אם יש לכם כרטיס NVIDIA, נסו את וריאנט CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   השוו את מהירות התמלול מול וריאנט ה-CPU.

3. **הוספת עיצוב פלט** - תגובת JSON יכולה לכלול:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **בנו REST API** - עטפו את קוד התמלול בשרת וובי:

   | שפה | מסגרת עבודה | דוגמה |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` עם `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` עם `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` עם `IFormFile` |

5. **רב-סיבובים עם תמלול** - שלבו את Whisper עם סוכן צ'אט מחלקה 4: תמללו קודם את השמע, ואז העבירו את הטקסט לסוכן לניתוח או לסיכום.

---

## רפרנס ל-API שמע של ה-SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — יוצר מופע של `AudioClient`
> - `audioClient.settings.language` — הגדרת שפת התמלול (למשל `"en"`)
> - `audioClient.settings.temperature` — שליטה ברמת הרנדומליות (אופציונלי)
> - `audioClient.transcribe(filePath)` — מתמלל קובץ, מחזיר `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — זרימת מקטעי תמלול דרך callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — יוצר מופע של `OpenAIAudioClient`
> - `audioClient.Settings.Language` — הגדרת שפת התמלול (למשל `"en"`)
> - `audioClient.Settings.Temperature` — שליטה ברמת הרנדומליות (אופציונלי)
> - `await audioClient.TranscribeAudioAsync(filePath)` — מתמלל קובץ, מחזיר אובייקט עם `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — מחזיר `IAsyncEnumerable` של מקטעי תמלול

> **טיפ:** תמיד הגדירו את מאפיין השפה לפני התמלול. בלעדיו, מודל Whisper מנסה זיהוי אוטומטי, מה שעלול לייצר פלט מקודד שגוי (תו החלפה בודד במקום טקסט).

---

## השוואה: מודלי צ'אט מול Whisper

| היבט | מודלי צ'אט (חלקים 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **סוג המשימה** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **קלט** | הודעות טקסט (JSON) | קבצי שמע (WAV/MP3/M4A) | קבצי שמע (WAV/MP3/M4A) |
| **פלט** | טקסט שנוצר (בזרם) | טקסט מתומלל (שלם) | טקסט מתומלל (שלם) |
| **חבילת SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **שיטת API** | `client.chat.completions.create()` | ONNX Runtime ישיר | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **הגדרת שפה** | לא רלוונטי | טוקנים לדחיפת המפענח | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **זרימה בשידור חי** | כן | לא | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **יתרון פרטיות** | קוד/נתונים נשארים מקומיים | נתוני שמע נשארים מקומיים | נתוני שמע נשארים מקומיים |

---

## נקודות עיקריות שנלמדו

| מושג | מה שלמדת |
|---------|-----------------|
| **Whisper על המכשיר** | המרה מדיבור לטקסט רצה באופן מקומי לחלוטין, אידיאלית לתמלול שיחות לקוחות זאבה וביקורות מוצרים על המכשיר |
| **SDK AudioClient** | חבילות ה-JavaScript ו-C# של ה-SDK מספקות `AudioClient` מובנה שמנהל את כל תהליך התמלול בקריאה אחת |
| **הגדרת שפה** | תמיד הגדירו את שפת ה-AudioClient (למשל `"en"`) — בלעדיה, זיהוי אוטומטי עלול לייצר פלט מקודד שגוי |
| **Python** | משתמשת ב-`foundry-local-sdk` לניהול מודלים + `onnxruntime` + `transformers` + `librosa` לביצוע אינפרנס ONNX ישיר |
| **JavaScript** | משתמש ב-`foundry-local-sdk` עם `model.createAudioClient()` — הגדרת `settings.language`, ואז קריאה ל-`transcribe()` |
| **C#** | משתמש ב-`Microsoft.AI.Foundry.Local` עם `model.GetAudioClientAsync()` — הגדרת `Settings.Language`, ואז קריאה ל-`TranscribeAudioAsync()` |
| **תמיכה בזרימה בשידור חי** | SDKs של JS ו-C# מציעים גם `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` לפלט מקטע אחר מקטע |
| **מניפולציה מיטבית CPU** | גרסת ה-CPU (3.05 GB) עובדת על כל מכשיר Windows ללא GPU |
| **פרטיות במרכז** | מושלם לשמירה על אינטראקציות לקוחות זאבה ונתוני מוצר קנייניים על המכשיר |

---

## משאבים

| מקור | קישור |
|----------|------|
| תיעוד Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| רפרנס Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| מודל OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| אתר Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## השלב הבא

המשיכו ל-[חלק 10: שימוש במודלים מותאמים אישית או Hugging Face](part10-custom-models.md) ליצירת מודלים משלכם מ-Hugging Face ולהרצתם דרך Foundry Local.