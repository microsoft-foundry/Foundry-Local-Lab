![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# חלק 2: מעמיקה ב-Foundry Local SDK

> **מטרה:** לשלוט ב-Foundry Local SDK לניהול מודלים, שירותים, ו-cache באופן תכנותי - ולהבין מדוע ה-SDK הוא הגישה המומלצת לעומת ה-CLI לבניית אפליקציות.

## סקירה כללית

בחלק 1 השתמשת ב-**Foundry Local CLI** להורדה והפעלת מודלים באופן אינטראקטיבי. ה-CLI נהדר לחקירה, אבל כשבונים אפליקציות אמיתיות צריך **שליטה תכנותית**. ה-Foundry Local SDK מעניק לך זאת - הוא מנהל את **מישור השליטה** (הפעלת השירות, גילוי מודלים, הורדה, טעינה) כך שקוד האפליקציה שלך יכול להתמקד ב**מישור הנתונים** (שליחת פרומפטים, קבלת השלמות).

המעבדה הזו מלמדת אותך את כל ממשק ה-API של ה-SDK בפייתון, JavaScript ו-C#. בסוף תבין כל שיטה זמינה ומתי להשתמש בכל אחת.

## מטרות למידה

בסיום מעבדה זו תוכל:

- להסביר מדוע ה-SDK מועדף על ה-CLI לפיתוח אפליקציות
- להתקין את Foundry Local SDK לפייתון, JavaScript, או C#
- להשתמש ב-`FoundryLocalManager` כדי להפעיל את השירות, לנהל מודלים, ולשאול את הקטלוג
- לרשום, להוריד, לטעון ולפרוק מודלים באופן תכנותי
- לבדוק מטא-דאטה של מודל באמצעות `FoundryModelInfo`
- להבין את ההבדל בין קטלוג, cache, ומודלים שטעונים בזיכרון
- להשתמש בבוסטרפ הקונסטרקטור (פייתון) ובתבנית `create()` + קטלוג (JavaScript)
- להבין את עיצוב מחדש של ה-C# SDK ואת ממשק ה-API מונחה האובייקטים שלו

---

## דרישות מוקדמות

| דרישה | פרטים |
|-------------|---------|
| **Foundry Local CLI** | מותקן ונמצא ב-`PATH` שלך ([חלק 1](part1-getting-started.md)) |
| **סביבת ריצה** | **Python 3.9+** ו/או **Node.js 18+** ו/או **.NET 9.0+** |

---

## מושג: SDK מול CLI - למה להשתמש ב-SDK?

| היבט | CLI (`foundry` פקודה) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **שימוש** | חקירה, בדיקות ידניות | אינטגרציה לאפליקציה |
| **ניהול שירות** | ידני: `foundry service start` | אוטומטי: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **גילוי פורט** | קריאה מפלט CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **הורדת מודל** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **טיפול בשגיאות** | קודי יציאה, stderr | החרגות, טיפוסי שגיאות |
| **אוטומציה** | סקריפטים של Shell | אינטגרציה בשפת תכנות מקורית |
| **פריסה** | דורש CLI במחשב המשתמש | C# SDK יכול להיות עצמאי (ללא צורך ב-CLI) |

> **תובנה מרכזית:** ה-SDK מנהל את כל מחזור החיים: הפעלת השירות, בדיקת cache, הורדת מודלים חסרים, טעינתם, וגילוי נקודת הקצה, בכמה שורות קוד. האפליקציה שלך לא צריכה לפרש פלט CLI או לנהל תהליכים נלווים.

---

## תרגילי מעבדה

### תרגיל 1: התקנת ה-SDK

<details>
<summary><h3>🐍 פייתון</h3></summary>

```bash
pip install foundry-local-sdk
```

אמת את ההתקנה:

```python
from foundry_local import FoundryLocalManager
print("SDK installed successfully")
```

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```bash
npm install foundry-local-sdk
```

אמת את ההתקנה:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

יש שני חבילות NuGet:

| חבילה | פלטפורמה | תיאור |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | רוחב פלטפורמות | עובד על Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | רק Windows | מוסיף האצה חומרתית WinML; מוריד ומתקין ספקי ביצוע תוספים (כגון QNN ל-Qualcomm NPU) |

**הגדרת Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

ערוך את קובץ `.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <!-- Windows: windows-specific TFM so WinML (QNN EP plugin) can load -->
    <TargetFramework Condition="$([MSBuild]::IsOSPlatform('Windows'))">net9.0-windows10.0.26100</TargetFramework>
    <!-- Non-Windows: plain TFM (WinML is not available) -->
    <TargetFramework Condition="!$([MSBuild]::IsOSPlatform('Windows'))">net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <!-- WinML requires these properties on Windows -->
  <PropertyGroup Condition="$([MSBuild]::IsOSPlatform('Windows'))">
    <WindowsAppSDKSelfContained>false</WindowsAppSDKSelfContained>
    <WindowsPackageType>None</WindowsPackageType>
    <EnableCoreMrtTooling>false</EnableCoreMrtTooling>
  </PropertyGroup>

  <!-- Windows: WinML is a superset (base SDK + QNN EP plugin) -->
  <ItemGroup Condition="$([MSBuild]::IsOSPlatform('Windows'))">
    <PackageReference Include="Microsoft.AI.Foundry.Local.WinML" Version="[0.9.0,1.0.0)" />
  </ItemGroup>

  <!-- Non-Windows: base SDK only -->
  <ItemGroup Condition="!$([MSBuild]::IsOSPlatform('Windows'))">
    <PackageReference Include="Microsoft.AI.Foundry.Local" Version="[0.9.0,1.0.0)" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Logging" Version="9.0.10" />
  </ItemGroup>
</Project>
```

> **הערה:** ב-Windows, חבילת WinML היא על-קבוצה הכוללת את ה-SDK הבסיסי ועוד ספק ביצוע QNN. בלינוקס/macOS משתמשים ב-SDK הבסיסי. TFM מותנה ופקודות חבילות שומרות על תמיכה רוחב פלטפורמות מלאה.

צור קובץ `nuget.config` בשורש הפרויקט:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="ORT" value="https://aiinfra.pkgs.visualstudio.com/PublicPackages/_packaging/ORT/nuget/v3/index.json" />
  </packageSources>
  <packageSourceMapping>
    <packageSource key="nuget.org">
      <package pattern="*" />
    </packageSource>
    <packageSource key="ORT">
      <package pattern="*Foundry*" />
    </packageSource>
  </packageSourceMapping>
</configuration>
```

שחזר חבילות:

```bash
dotnet restore
```

</details>

---

### תרגיל 2: הפעלת השירות ורשימת הקטלוג

הדבר הראשון שכל אפליקציה עושה הוא להפעיל את שירות Foundry Local ולגלות אילו מודלים זמינים.

<details>
<summary><h3>🐍 פייתון</h3></summary>

```python
from foundry_local import FoundryLocalManager

# צור מנהל והתחל את השירות
manager = FoundryLocalManager()
manager.start_service()

# רשום את כל הדגמים הזמינים בקטלוג
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### פייתון SDK - שיטות ניהול שירות

| שיטה | חתימה | תיאור |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | בדוק אם השירות רץ |
| `start_service()` | `() -> None` | הפעל את שירות Foundry Local |
| `service_uri` | `@property -> str` | כתובת השירות הבסיסית |
| `endpoint` | `@property -> str` | נקודת API (כתובת שירות + `/v1`) |
| `api_key` | `@property -> str` | מפתח API (מהסביבה או פלייסהולדר ברירת מחדל) |

#### פייתון SDK - שיטות ניהול קטלוג

| שיטה | חתימה | תיאור |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | רשום את כל המודלים בקטלוג |
| `refresh_catalog()` | `() -> None` | רענן את הקטלוג מהשירות |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | קבל מידע למודל ספציפי |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// צור מנהל והפעל את השירות
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// דפדף בקטלוג
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - שיטות מנהל

| שיטה | חתימה | תיאור |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | אתחל את הסינגלטון של ה-SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | גש לסינגלטון מנג'ר |
| `manager.startWebService()` | `() => Promise<void>` | הפעל את שירות הווב של Foundry Local |
| `manager.urls` | `string[]` | מערך כתובות בסיס לשירות |

#### JavaScript SDK - שיטות קטלוג ומודל

| שיטה | חתימה | תיאור |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | גש לקטלוג המודלים |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | קבל אובייקט מודל לפי כינוי |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

ה-SDK ב-C# גרסה 0.8.0+ משתמש בארכיטקטורה מונחה אובייקטים עם אובייקטים של `Configuration`, `Catalog`, ו-`Model`:

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// Step 1: Configure
var config = new Configuration
{
    AppName = "SDKDemo",
    Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
};

// Step 2: Create the manager
await FoundryLocalManager.CreateAsync(config, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 3: Browse the catalog
var catalog = await manager.GetCatalogAsync(default);
var models = await catalog.ListModelsAsync(default);

Console.WriteLine($"Models available in catalog: {models.Count()}");

foreach (var model in models)
{
    Console.WriteLine($"  - {model.Alias} ({model.ModelId})");
}
```

#### C# SDK - מחלקות מפתח

| מחלקה | מטרה |
|-------|---------|
| `Configuration` | הגדרת שם אפליקציה, רמת לוג, תיקיית cache, כתובות שרת ווב |
| `FoundryLocalManager` | נקודת כניסה עיקרית - נוצר דרך `CreateAsync()`, נגיש דרך `.Instance` |
| `Catalog` | גלישה, חיפוש, וקבלת מודלים מהקטלוג |
| `Model` | מייצג מודל ספציפי - הורדה, טעינה, קבלת לקוחות |

#### C# SDK - שיטות מנהל וקטלוג

| שיטה | תיאור |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | אתחל את המנהל |
| `FoundryLocalManager.Instance` | גש למנהל הסינגלטון |
| `manager.GetCatalogAsync()` | קבל את קטלוג המודלים |
| `catalog.ListModelsAsync()` | רשום את כל המודלים הזמינים |
| `catalog.GetModelAsync(alias: "alias")` | קבל מודל ספציפי לפי כינוי |
| `catalog.GetCachedModelsAsync()` | רשום מודלים שהורדו |
| `catalog.GetLoadedModelsAsync()` | רשום מודלים שנטענו כרגע |

> **הערת ארכיטקטורה C#:** עיצוב מחדש ב-SDK בגרסה 0.8.0+ הופך את האפליקציה ל**עצמאית**; אין צורך ב-Foundry Local CLI במחשב המשתמש. ה-SDK מנהל את ניהול המודלים והפענוח באופן מקורי.

</details>

---

### תרגיל 3: הורדה וטעינת מודל

ה-SDK מפריד בין הורדה (לכונן) לטעינה (לזיכרון). זה מאפשר להוריד מודלים מראש בשלב ההתקנה ולטעון על פי דרישה.

<details>
<summary><h3>🐍 פייתון</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# אפשרות א: שלב-אחר-שלב ידני
manager = FoundryLocalManager()
manager.start_service()

# בדוק את המטמון תחילה
cached = manager.list_cached_models()
model_info = manager.get_model_info(alias)
is_cached = any(m.id == model_info.id for m in cached) if model_info else False

if not is_cached:
    print(f"Downloading {alias}...")
    manager.download_model(alias)

print(f"Loading {alias}...")
loaded = manager.load_model(alias)
print(f"Loaded: {loaded.id}")
print(f"Endpoint: {manager.endpoint}")

# אפשרות ב: הרצת אתחול בשורה אחת (מומלץ)
# העבר כינוי לבנאי - זה מפעיל את השירות, מוריד וטוען אוטומטית
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### פייתון - שיטות ניהול מודלים

| שיטה | חתימה | תיאור |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | הורד מודל ל-cache מקומי |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | טען מודל לשרת הפענוח |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | פרוק מודל מהשרת |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | רשום את כל המודלים שנטענו כרגע |

#### פייתון - שיטות ניהול cache

| שיטה | חתימה | תיאור |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | קבל את נתיב תיקיית ה-cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | רשום את כל המודלים שהורדו |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// גישה שלב אחר שלב
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(alias);

if (!model.isCached) {
  console.log(`Downloading ${alias}...`);
  await model.download();
}

console.log(`Loading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);
console.log(`Endpoint: ${manager.urls[0]}/v1`);
```

#### JavaScript - שיטות מודל

| שיטה | חתימה | תיאור |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | האם המודל כבר הורד |
| `model.download()` | `() => Promise<void>` | הורד את המודל ל-cache מקומי |
| `model.load()` | `() => Promise<void>` | טען לשרת פענוח |
| `model.unload()` | `() => Promise<void>` | פנה מהשרת פענוח |
| `model.id` | `string` | מזהה יחודי של המודל |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "phi-3.5-mini";

var config = new Configuration
{
    AppName = "SDKDemo",
    Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
};

await FoundryLocalManager.CreateAsync(config, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);

// View available variants
Console.WriteLine($"Model: {model.Alias}");
foreach (var variant in model.Variants)
{
    Console.WriteLine($"  Variant: {variant.Info.ModelId}");
    Console.WriteLine($"    Device: {variant.Info.Runtime?.DeviceType}");
}

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading...");
    await model.DownloadAsync(null, default);
}

// Load into memory
Console.WriteLine("Loading...");
await model.LoadAsync(default);
Console.WriteLine($"Model loaded: {model.Id}");
```

#### C# - שיטות מודל

| שיטה | תיאור |
|--------|-------------|
| `model.DownloadAsync(progress?)` | הורד את הווריאנט הנבחר |
| `model.LoadAsync()` | טען את המודל לזיכרון |
| `model.UnloadAsync()` | פרוק את המודל |
| `model.SelectVariant(variant)` | בחר וריאנט ספציפי (CPU/GPU/NPU) |
| `model.SelectedVariant` | הווריאנט הנבחר כרגע |
| `model.Variants` | כל הווריאנטים הזמינים למודל זה |
| `model.GetPathAsync()` | קבל את נתיב הקובץ המקומי |
| `model.GetChatClientAsync()` | קבל לקוח צ'אט מקורי (ללא צורך ב-SDK של OpenAI) |
| `model.GetAudioClientAsync()` | קבל לקוח אודיו לתמלול |

</details>

---

### תרגיל 4: בדיקת מטא-דאטה של מודל

האובייקט `FoundryModelInfo` מכיל מטא-דאטה עשירה על כל מודל. הבנת שדות אלה עוזרת לך לבחור את המודל המתאים לאפליקציה שלך.

<details>
<summary><h3>🐍 פייתון</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# לקבל מידע מפורט על דגם ספציפי
info = manager.get_model_info("phi-3.5-mini")
if info:
    print(f"Alias:              {info.alias}")
    print(f"Model ID:           {info.id}")
    print(f"Version:            {info.version}")
    print(f"Task:               {info.task}")
    print(f"Device Type:        {info.device_type}")
    print(f"Execution Provider: {info.execution_provider}")
    print(f"File Size (MB):     {info.file_size_mb}")
    print(f"Publisher:          {info.publisher}")
    print(f"License:            {info.license}")
    print(f"Tool Calling:       {info.supports_tool_calling}")
```

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Alias:              ${model.alias}`);
console.log(`Model ID:           ${model.id}`);
console.log(`Cached:             ${model.isCached}`);
```

</details>

#### שדות FoundryModelInfo

| שדה | סוג | תיאור |
|-------|------|-------------|
| `alias` | מחרוזת | שם קצר (לדוגמה `phi-3.5-mini`) |
| `id` | מחרוזת | מזהה ייחודי של מודל |
| `version` | מחרוזת | גרסת מודל |
| `task` | מחרוזת | `chat-completions` או `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, או NPU |
| `execution_provider` | מחרוזת | Backend ריצה (CUDA, CPU, QNN, WebGPU וכו') |
| `file_size_mb` | int | גודל בדיסק במגה-בייט |
| `supports_tool_calling` | בוליאני | האם המודל תומך בקריאה לפונקציות/כלים |
| `publisher` | מחרוזת | מי פרסם את המודל |
| `license` | מחרוזת | שם הרישיון |
| `uri` | מחרוזת | כתובת המודל |
| `prompt_template` | מילון / ריק | תבנית פרומפט אם קיימת |

---

### תרגיל 5: ניהול מחזור חיים של מודל

התנסות במחזור החיים המלא: רישום → הורדה → טעינה → שימוש → פריקה.

<details>
<summary><h3>🐍 פייתון</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # מודל קטן לבדיקה מהירה

manager = FoundryLocalManager()
manager.start_service()

# 1. בדוק מה שיש בקטלוג
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. בדוק מה כבר הורד
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. הורד מודל
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. אמת שהוא במטמון עכשיו
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. טען אותו
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. בדוק מה נטען
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. הפרק אותו
print(f"\nUnloading {alias}...")
manager.unload_model(alias)
loaded = manager.list_loaded_models()
print(f"Loaded models after unload: {len(loaded)}")
```

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "qwen2.5-0.5b"; // מודל קטן לבדיקות מהירות

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. קבל את המודל מהקטלוג
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. הורד אם יש צורך
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. טען אותו
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. פרוק אותו
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### תרגיל 6: דפוסי התחלה מהירה

כל שפה מספקת קיצור דרך להפעיל את השירות ולטעון דגם בקריאה אחת. אלה הם **דפוסי ההמלצה** לרוב היישומים.

<details>
<summary><h3>🐍 Python - אתחול בקונסטרקטור</h3></summary>

```python
from foundry_local import FoundryLocalManager

# העבר כינוי לבונה - הוא מטפל בכל דבר:
# 1. מפעיל את השירות אם אינו פועל
# 2. מוריד את הדגם אם אינו מאוחסן במטמון
# 3. טוען את הדגם לשרת האינפרנס
manager = FoundryLocalManager("phi-3.5-mini")

# מוכן לשימוש מיידי
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

הפרמטר `bootstrap` (ברירת מחדל `True`) שולט בהתנהגות זו. הגדר `bootstrap=False` אם ברצונך שליטה ידנית:

```python
# מצב ידני - שום דבר לא קורה אוטומטית
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + קטלוג</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() מטפל בכל דבר:
// 1. מפעיל את השירות
// 2. מקבל את המודל מהקטלוג
// 3. מוריד במידת הצורך וטוען את המודל
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// מוכן לשימוש מיידי
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + קטלוג</h3></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var config = new Configuration
{
    AppName = "QuickStart",
    Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
};

await FoundryLocalManager.CreateAsync(config, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("phi-3.5-mini", default);

var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);

Console.WriteLine($"Model loaded: {model.Id}");
```

> **הערת C#:** SDK של C# משתמש ב-`Configuration` לשליטה על שם האפליקציה, רישום הלוגים, תיקיות מטמון, ואף נעילת פורט ספציפי של שרת האינטרנט. זה עושה אותו הניתן ביותר לתצורה מבין שלושת ה-SDK.

</details>

---

### תרגיל 7: ה-ChatClient המקומי (אין צורך ב-SDK של OpenAI)

ה-SDK של JavaScript ושל C# מספקים שיטת נוחות `createChatClient()` שמחזירה לקוח צ'אט מקומי — אין צורך בהתקנה או תצורה נפרדת של SDK OpenAI.

<details>
<summary><h3>📘 JavaScript - <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// צור ChatClient ישירות מהמודל — אין צורך בייבוא OpenAI
const chatClient = model.createChatClient();

// completeChat מחזיר אובייקט תגובה התואם ל-OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// סטרימינג משתמש בדפוס callback
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

ה-`ChatClient` תומך גם בקריאת כלים — העבר כלים כארגומנט שני:

```javascript
const response = await chatClient.completeChat(messages, tools);
```

</details>

<details>
<summary><h3>💜 C# - <code>model.GetChatClientAsync()</code></h3></summary>

```csharp
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("phi-3.5-mini", default);
if (!await model.IsCachedAsync(default))
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);

// Get a native chat client — no OpenAI NuGet package needed
var chatClient = await model.GetChatClientAsync(default);

// Use it exactly like the OpenAI ChatClient
var response = chatClient.CompleteChat("What is the golden ratio?");
Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **מתי להשתמש באיזה דפוס:**
> - **`createChatClient()`** — פרוטוטייפ מהיר, תלותות פחותות, קוד פשוט יותר
> - **OpenAI SDK** — שליטה מלאה בפרמטרים (טמפרטורה, top_p, stop tokens, וכו'), טוב יותר ליישומי ייצור

---

### תרגיל 8: וריאנטים של דגמים ובחירת חומרה

לדגמים יכולים להיות מספר **וריאנטים** מותאמים לחומרות שונות. ה-SDK בוחר אוטומטית את הווריאנט הטוב ביותר, אך ניתן גם לבדוק ולבחור ידנית.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// רשום את כל הגרסאות הזמינות
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// ערכת הפיתוח בוחרת אוטומטית את הגרסה הטובה ביותר עבור החומרה שלך
// כדי לבטל, השתמש ב-selectVariant():
// model.selectVariant(model.variants[0]);
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

```csharp
var model = await catalog.GetModelAsync("phi-3.5-mini", default);

Console.WriteLine($"Model: {model.Alias}");
Console.WriteLine($"Selected variant: {model.SelectedVariant?.Info.ModelId}");
Console.WriteLine($"All variants:");
foreach (var variant in model.Variants)
{
    Console.WriteLine($"  - {variant.Info.ModelId}");
    Console.WriteLine($"    Device: {variant.Info.Runtime?.DeviceType}");
}

// To select a specific variant:
// model.SelectVariant(model.Variants.First());
```

</details>

<details>
<summary><h3>🐍 Python</h3></summary>

בפייתון, ה-SDK בוחר אוטומטית את הווריאנט הטוב ביותר בהתאם לחומרה. השתמש ב-`get_model_info()` כדי לראות מה נבחר:

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

info = manager.get_model_info("phi-3.5-mini")
print(f"Selected model: {info.id}")
print(f"Device: {info.device_type}")
print(f"Provider: {info.execution_provider}")
```

</details>

#### דגמים עם וריאנטים NPU

לחלק מהדגמים יש וריאנטים מותאמים למעבדי Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| דגם | וריאנט NPU זמין |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **טיפ:** על חומרה התומכת ב-NPU, ה-SDK בוחר אוטומטית את הווריאנט NPU כאשר זמין. אין צורך לשנות את הקוד שלך. עבור פרויקטים ב-C# על Windows, הוסף את חבילת ה-NuGet `Microsoft.AI.Foundry.Local.WinML` כדי לאפשר את ספק הביצוע QNN — QNN מופץ כתוסף EP באמצעות WinML.

---

### תרגיל 9: שדרוגים לדגם ורענון הקטלוג

קטלוג הדגמים מתעדכן מדי תקופה. השתמש בשיטות אלה לבדיקה וליישום עדכונים.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# רענן את הקטלוג כדי לקבל את רשימת הדגמים האחרונה
manager.refresh_catalog()

# בדוק אם למודל במטמון יש גרסה חדשה יותר זמינה
if manager.is_model_upgradeable(alias):
    print(f"{alias} has a newer version available!")
    manager.upgrade_model(alias)
    print("Upgrade complete")
else:
    print(f"{alias} is up to date")
```

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "UpgradeDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// רענן את הקטלוג כדי לקבל את רשימת הדגמים העדכנית ביותר
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// הצג את כל הדגמים הזמינים לאחר הרענון
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### תרגיל 10: עבודה עם דגמי חשיבה

דגם **phi-4-mini-reasoning** כולל חשיבה בתהליך השרשרת. הוא עוטף את ההיגיון הפנימי שלו בתגי `<think>...</think>` לפני הפקת התוצאה הסופית. זה שימושי למשימות שדורשות הגיון רב-שלבי, מתמטיקה או פתרון בעיות.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning הוא כ~4.6 ג'יגה-בייט
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# המודל עוטף את החשיבה שלו בתגיות <think>...</think>
if "<think>" in content and "</think>" in content:
    think_start = content.index("<think>") + len("<think>")
    think_end = content.index("</think>")
    thinking = content[think_start:think_end].strip()
    answer = content[think_end + len("</think>"):].strip()
    print(f"Thinking: {thinking}")
    print(f"Answer: {answer}")
else:
    print(content)
```

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ReasoningDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-4-mini-reasoning");
if (!model.isCached) await model.download();
await model.load();

const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

const response = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "What is 17 × 23?" }],
});

const content = response.choices[0].message.content;

// ניתוח חשיבה של שרשרת מחשבות
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **מתי להשתמש בדגמי חשיבה:**
> - בעיות מתמטיקה והיגיון
> - משימות תכנון רב-שלבים
> - יצירת קוד מורכב
> - משימות בהן הצגת תהליך העבודה משפרת דיוק
>
> **תמורה:** דגמי החשיבה מפיקים יותר טוקנים (חלק ה-`<think>`) ואיטיים יותר. עבור שאלות ותשובות פשוטות, דגם רגיל כמו phi-3.5-mini מהיר יותר.

---

### תרגיל 11: הבנת כינויים ובחירת חומרה

כשאתה מעביר **כינוי** (כגון `phi-3.5-mini`) במקום מזהה דגם מלא, ה-SDK בוחר אוטומטית את הווריאנט הטוב ביותר עבור החומרה שלך:

| חומרה | ספק ביצוע שנבחר |
|----------|---------------------------|
| GPU של NVIDIA (CUDA) | `CUDAExecutionProvider` |
| NPU של Qualcomm | `QNNExecutionProvider` (דרך תוסף WinML) |
| NPU של Intel | `OpenVINOExecutionProvider` |
| GPU של AMD | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| כל מכשיר (גיבוי) | `CPUExecutionProvider` או `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# הכינוי מתייחס לגרסה הטובה ביותר עבור החומרה שלך
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **טיפ:** השתמש תמיד בכינויים בקוד האפליקציה שלך. כשאתה מפעיל במחשב של משתמש, ה-SDK בוחר את הווריאנט האופטימלי בזמן ריצה - CUDA על NVIDIA, QNN על Qualcomm, CPU במקום אחר.

---

### תרגיל 12: אפשרויות תצורה ל-C#

מחלקת `Configuration` בספריית C# מספקת שליטה מדויקת בזמן הריצה:

```csharp
var config = new Configuration
{
    AppName = "MyApp",
    LogLevel = Microsoft.AI.Foundry.Local.LogLevel.Information,

    // Pin a specific port (useful for debugging)
    Web = new Configuration.WebService
    {
        Urls = "http://127.0.0.1:55588"
    },

    // Custom directories
    AppDataDir = "./foundry_local_data",
    ModelCacheDir = "{AppDataDir}/model_cache",
    LogsDir = "{AppDataDir}/logs"
};
```

| מאפיין | ברירת מחדל | תיאור |
|----------|---------|-------------|
| `AppName` | (נדרש) | שם האפליקציה שלך |
| `LogLevel` | `Information` | רמת פירוט הלוגים |
| `Web.Urls` | (דינמי) | נעילת פורט ספציפי לשרת האינטרנט |
| `AppDataDir` | ברירת מחדל מערכת ההפעלה | תיקיית בסיס לנתוני אפליקציה |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | היכן מאוחסנים הדגמים |
| `LogsDir` | `{AppDataDir}/logs` | היכן כותבים את הלוגים |

---

### תרגיל 13: שימוש בדפדפן (JavaScript בלבד)

ל-SDK של JavaScript קיימת גרסה תואמת לדפדפן. בדפדפן, יש להפעיל ידנית את השירות דרך CLI ולציין את כתובת השרת:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// הפעל את השירות ידנית תחילה:
//   foundry service start
// לאחר מכן השתמש ב-URL מהפלט של שורת הפקודה
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// עיין בקטלוג
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **הגבלות בדפדפן:** גרסת הדפדפן **אינה** תומכת ב-`startWebService()`. יש לוודא שהשירות Foundry Local רץ טרם השימוש ב-SDK בדפדפן.

---

## הפניה מלאה ל-API

### Python

| קטגוריה | שיטה | תיאור |
|----------|--------|-------------|
| **אתחול** | `FoundryLocalManager(alias?, bootstrap=True)` | יצירת מנהל; אופציונלי לאתחול עם דגם |
| **שירות** | `is_service_running()` | בדיקה אם השירות רץ |
| **שירות** | `start_service()` | הפעלת השירות |
| **שירות** | `endpoint` | כתובת נקודת הקצה של ה-API |
| **שירות** | `api_key` | מפתח API |
| **קטלוג** | `list_catalog_models()` | רשימת כל הדגמים הזמינים |
| **קטלוג** | `refresh_catalog()` | רענון הקטלוג |
| **קטלוג** | `get_model_info(alias_or_model_id)` | קבלת מטא-דאטה של דגם |
| **מטמון** | `get_cache_location()` | נתיב תיקיית המטמון |
| **מטמון** | `list_cached_models()` | רשימת דגמים שהורדו |
| **דגם** | `download_model(alias_or_model_id)` | הורדת דגם |
| **דגם** | `load_model(alias_or_model_id, ttl=600)` | טעינת דגם |
| **דגם** | `unload_model(alias_or_model_id)` | פריקת דגם |
| **דגם** | `list_loaded_models()` | רשימת דגמים טעונים |
| **דגם** | `is_model_upgradeable(alias_or_model_id)` | בדוק אם יש גרסה חדשה זמינה |
| **דגם** | `upgrade_model(alias_or_model_id)` | שדרוג דגם לגרסה העדכנית ביותר |
| **שירות** | `httpx_client` | לקוח HTTPX מוכן לשימוש לפניות ישירות ל-API |

### JavaScript

| קטגוריה | שיטה | תיאור |
|----------|--------|-------------|
| **אתחול** | `FoundryLocalManager.create(options)` | אתחול singleton של ה-SDK |
| **אתחול** | `FoundryLocalManager.instance` | גישה למנהל ה-singleton |
| **שירות** | `manager.startWebService()` | הפעלת שירות האינטרנט |
| **שירות** | `manager.urls` | מערך כתובות בסיס לשירות |
| **קטלוג** | `manager.catalog` | גישה לקטלוג הדגמים |
| **קטלוג** | `catalog.getModel(alias)` | קבלת אובייקט דגם לפי כינוי (מחזיר Promise) |
| **דגם** | `model.isCached` | האם הדגם הועלה למטמון |
| **דגם** | `model.download()` | הורדת הדגם |
| **דגם** | `model.load()` | טעינת הדגם |
| **דגם** | `model.unload()` | פריקת הדגם |
| **דגם** | `model.id` | מזהה ייחודי של הדגם |
| **דגם** | `model.alias` | הכינוי של הדגם |
| **דגם** | `model.createChatClient()` | קבלת לקוח צ'אט מקומי (אין צורך ב-SDK OpenAI) |
| **דגם** | `model.createAudioClient()` | קבלת לקוח שמע לתמלול |
| **דגם** | `model.removeFromCache()` | הסרת הדגם ממטמון מקומי |
| **דגם** | `model.selectVariant(variant)` | בחירת וריאנט חומרה ספציפי |
| **דגם** | `model.variants` | מערך הווריאנטים הזמינים לדגם זה |
| **דגם** | `model.isLoaded()` | בדיקה אם הדגם טעון כרגע |
| **קטלוג** | `catalog.getModels()` | רשימת כל הדגמים הזמינים |
| **קטלוג** | `catalog.getCachedModels()` | רשימת דגמים שהורדו |
| **קטלוג** | `catalog.getLoadedModels()` | רשימת דגמים טעונים |
| **קטלוג** | `catalog.updateModels()` | רענון הקטלוג מהשירות |
| **שירות** | `manager.stopWebService()` | עצירת שירות האינטרנט Foundry Local |

### C# (v0.8.0+)

| קטגוריה | שיטה | תיאור |
|----------|--------|-------------|
| **אתחול** | `FoundryLocalManager.CreateAsync(config, logger)` | אתחול המנהל |
| **אתחול** | `FoundryLocalManager.Instance` | גישה ל-singleton |
| **קטלוג** | `manager.GetCatalogAsync()` | קבלת קטלוג |
| **קטלוג** | `catalog.ListModelsAsync()` | רשימת כל הדגמים |
| **קטלוג** | `catalog.GetModelAsync(alias)` | קבלת דגם ספציפי |
| **קטלוג** | `catalog.GetCachedModelsAsync()` | רשימת דגמים במטמון |
| **קטלוג** | `catalog.GetLoadedModelsAsync()` | רשימת דגמים טעונים |
| **דגם** | `model.DownloadAsync(progress?)` | הורדת דגם |
| **דגם** | `model.LoadAsync()` | טעינת דגם |
| **דגם** | `model.UnloadAsync()` | פריקת דגם |
| **דגם** | `model.SelectVariant(variant)` | בחירת וריאנט חומרה |
| **דגם** | `model.GetChatClientAsync()` | קבלת לקוח צ'אט מקומי |
| **דגם** | `model.GetAudioClientAsync()` | קבלת לקוח תמלול שמע |
| **דגם** | `model.GetPathAsync()` | קבלת נתיב קובץ מקומי |
| **קטלוג** | `catalog.GetModelVariantAsync(alias, variant)` | קבלת וריאנט חומרה ספציפי |
| **קטלוג** | `catalog.UpdateModelsAsync()` | רענון הקטלוג |
| **שרת** | `manager.StartWebServerAsync()` | הפעלת שרת רשת REST |
| **שרת** | `manager.StopWebServerAsync()` | עצירת שרת רשת REST |
| **תצורה** | `config.ModelCacheDir` | תיקיית המטמון |

---

## דגשים מרכזיים

| מושג | מה שלמדת |
|---------|-----------------|
| **SDK לעומת CLI** | ה-SDK מספק שליטה תכנותית - חיוני ליישומים |
| **מישור השליטה** | ה-SDK מנהל שירותים, דגמים ומטמון |
| **פורט דינמי** | תמיד השתמש ב-`manager.endpoint` (Python) או `manager.urls[0]` (JS/C#) - לעולם אל תקודד פורט ישירות |
| **כינויים** | השתמש בכינויים לבחירה אוטומטית של דגם מיטבי לחומרה |
| **התחלה מהירה** | פייתון: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **עיצוב מחדש ב-C#** | גרסה 0.8.0 ומעלה - עצמאית לחלוטין, לא נדרש CLI במחשבי המשתמש |
| **מחזור חיי המודל** | קטלוג → הורדה → טעינה → שימוש → פריקה |
| **FoundryModelInfo** | מטא-נתונים עשירים: משימה, מכשיר, גודל, רישיון, תמיכה בקריאות כלי |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) לשימוש ללא OpenAI |
| **ווריאנטים** | למודלים יש וריאנטים מותאמים לחומרה (CPU, GPU, NPU); נבחרים אוטומטית |
| **שדרוגים** | פייתון: `is_model_upgradeable()` + `upgrade_model()` לשמירת עדכניות המודלים |
| **ריענון קטלוג** | `refresh_catalog()` (פייתון) / `updateModels()` (JS) לגילוי מודלים חדשים |

---

## משאבים

| משאב | קישור |
|----------|------|
| התייחסות ל-SDK (כל שפות התכנות) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| אינטגרציה עם SDKs להסקה | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| התייחסות ל-API של C# SDK | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| דוגמאות ל-C# SDK | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| אתר Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## שלבים הבאים

המשך ל־[חלק 3: שימוש ב-SDK עם OpenAI](part3-sdk-and-apis.md) כדי לחבר את ה-SDK לספריית הלקוח של OpenAI ולבנות את אפליקציית השיחה הראשונה שלך.