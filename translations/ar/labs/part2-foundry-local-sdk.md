![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# الجزء الثاني: الغوص العميق في Foundry Local SDK

> **الهدف:** إتقان Foundry Local SDK لإدارة النماذج، والخدمات، والتخزين المؤقت برمجيًا - وفهم لماذا تُعد SDK النهج المُوصى به بدلاً من CLI لبناء التطبيقات.

## نظرة عامة

في الجزء الأول استخدمت **Foundry Local CLI** لتنزيل وتشغيل النماذج تفاعليًا. CLI رائع للاستكشاف، لكن عند بناء تطبيقات حقيقية تحتاج إلى **التحكم البرمجي**. توفر لك Foundry Local SDK ذلك - فهي تدير **مستوى التحكم** (بدء الخدمة، اكتشاف النماذج، التنزيل، التحميل) حتى يركّز كود التطبيق الخاص بك على **مستوى البيانات** (إرسال المطالبات، استقبال الإكمالات).

تعلمك هذه المختبرات كامل واجهة برمجة التطبيقات (API) للـ SDK عبر Python و JavaScript و C#. بنهاية المختبر ستفهم كل طريقة متاحة ومتى تستخدم كل واحدة.

## أهداف التعلم

بنهاية هذا المختبر ستكون قادرًا على:

- شرح سبب تفضيل SDK على CLI لتطوير التطبيقات
- تثبيت Foundry Local SDK لـ Python أو JavaScript أو C#
- استخدام `FoundryLocalManager` لبدء الخدمة، إدارة النماذج، والاستعلام من الكتالوج
- سرد، تنزيل، تحميل، وإلغاء تحميل النماذج برمجيًا
- تفقد بيانات التعريف للنماذج باستخدام `FoundryModelInfo`
- فهم الفرق بين النماذج في الكتالوج، التخزين المؤقت، والنماذج المحملة
- استخدام منشئ التهيئة (البوتستراب) في Python ونمط `create()` + الكتالوج في JavaScript
- فهم إعادة تصميم SDK في C# وواجهته البرمجية القائمة على الكائنات

---

## المتطلبات الأساسية

| المتطلب | التفاصيل |
|-------------|---------|
| **Foundry Local CLI** | مثبت ومُضاف إلى `PATH` الخاص بك ([الجزء الأول](part1-getting-started.md)) |
| **بيئة تشغيل اللغة** | **Python 3.9+** و/أو **Node.js 18+** و/أو **.NET 9.0+** |

---

## المفهوم: SDK مقابل CLI - لماذا نستخدم SDK؟

| الجانب | CLI (أمر `foundry`) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **حالة الاستخدام** | استكشاف، اختبارات يدوية | تكامل التطبيقات |
| **إدارة الخدمة** | يدوي: `foundry service start` | تلقائي: `manager.start_service()` (بايثون) / `manager.startWebService()` (جافاسكريبت/سي شارب) |
| **اكتشاف المنفذ** | يُقرأ من مخرجات CLI | `manager.endpoint` (بايثون) / `manager.urls[0]` (جافاسكريبت/سي شارب) |
| **تنزيل النموذج** | `foundry model download alias` | `manager.download_model(alias)` (بايثون) / `model.download()` (جافاسكريبت/سي شارب) |
| **معالجة الأخطاء** | رموز الخروج، stderr | استثناءات، أخطاء نوعية |
| **الأتمتة** | سكريبتات شل | تكامل لغة برمجة أصلي |
| **النشر** | يتطلب CLI على جهاز المستخدم النهائي | SDK في C# يمكن أن يكون مستقلًا (بدون CLI) |

> **الرؤية الأساسية:** يدير SDK دورة الحياة الكاملة: بدء الخدمة، التحقق من التخزين المؤقت، تنزيل النماذج المفقودة، تحميلها، واكتشاف نقطة النهاية، كل ذلك بسطور قليلة. لا يحتاج تطبيقك لتحليل مخرجات CLI أو إدارة العمليات الفرعية.

---

## تمارين المختبر

### التمرين 1: تثبيت SDK

<details>
<summary><h3>🐍 بايثون</h3></summary>

```bash
pip install foundry-local-sdk
```

تحقق من التثبيت:

```python
from foundry_local import FoundryLocalManager
print("SDK installed successfully")
```

</details>

<details>
<summary><h3>📘 جافاسكريبت</h3></summary>

```bash
npm install foundry-local-sdk
```

تحقق من التثبيت:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 سي شارب</h3></summary>

هناك حزمتان في NuGet:

| الحزمة | النظام الأساسي | الوصف |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | متعدد الأنظمة | يعمل على ويندوز، لينكس، ماك |
| `Microsoft.AI.Foundry.Local.WinML` | ويندوز فقط | يضيف تسريع الأجهزة لـ WinML؛ يقوم بتنزيل وتثبيت موفري تنفيذ الإضافات (مثل QNN لمعالج Qualcomm NPU) |

**إعداد ويندوز:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

حرر ملف `.csproj`:

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

> **ملاحظة:** في ويندوز، حزمة WinML هي امتداد يشمل SDK الأساسي بالإضافة إلى موفر تنفيذ QNN. في لينكس/ماك، يتم استخدام SDK الأساسي فقط. الإشارات الشرطية لإطار العمل والحزم تجعل المشروع متعدد الأنظمة بالكامل.

أنشئ ملف `nuget.config` في جذر المشروع:

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

استعادة الحزم:

```bash
dotnet restore
```

</details>

---

### التمرين 2: بدء الخدمة وسرد الكتالوج

أول شيء تقوم به أي تطبيق هو بدء خدمة Foundry Local واكتشاف النماذج المتوفرة.

<details>
<summary><h3>🐍 بايثون</h3></summary>

```python
from foundry_local import FoundryLocalManager

# إنشاء مدير وبدء الخدمة
manager = FoundryLocalManager()
manager.start_service()

# سرد جميع النماذج المتاحة في الكتالوج
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - طرق إدارة الخدمة

| الطريقة | التوقيع | الوصف |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | تحقق إذا كانت الخدمة تعمل |
| `start_service()` | `() -> None` | بدء خدمة Foundry Local |
| `service_uri` | `@property -> str` | عنوان URI الأساسي للخدمة |
| `endpoint` | `@property -> str` | نقطة نهاية API (عنوان URI للخدمة + `/v1`) |
| `api_key` | `@property -> str` | مفتاح API (من البيئة أو القيم الافتراضية) |

#### Python SDK - طرق إدارة الكتالوج

| الطريقة | التوقيع | الوصف |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | سرد جميع النماذج في الكتالوج |
| `refresh_catalog()` | `() -> None` | تحديث الكتالوج من الخدمة |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | الحصول على معلومات نموذج محدد |

</details>

<details>
<summary><h3>📘 جافاسكريبت</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// إنشاء مدير وبدء الخدمة
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// تصفح الكتالوج
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - طرق المدير

| الطريقة | التوقيع | الوصف |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | تهيئة نموذج الـ SDK المفرد |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | الوصول إلى المدير المفرد |
| `manager.startWebService()` | `() => Promise<void>` | بدء خدمة نهاية الويب Foundry Local |
| `manager.urls` | `string[]` | مصفوفة عناوين URL الأساسية للخدمة |

#### JavaScript SDK - طرق الكتالوج والنموذج

| الطريقة | التوقيع | الوصف |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | الوصول إلى كتالوج النماذج |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | الحصول على كائن نموذج حسب الاسم المستعار |

</details>

<details>
<summary><h3>💜 سي شارب</h3></summary>

تستخدم C# SDK v0.8.0+ هندسة قائمة على الكائنات مع كائنات `Configuration` و `Catalog` و `Model`:

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

#### C# SDK - الفئات الرئيسية

| الفئة | الغرض |
|-------|---------|
| `Configuration` | ضبط اسم التطبيق، مستوى السجل، دليل التخزين المؤقت، عناوين خادم الويب |
| `FoundryLocalManager` | نقطة الدخول الرئيسية - تنشأ عبر `CreateAsync()`، وتصل عبر `.Instance` |
| `Catalog` | تصفح، بحث، والحصول على النماذج من الكتالوج |
| `Model` | تمثل نموذج محدد - تنزيل، تحميل، الحصول على عملاء |

#### C# SDK - طرق المدير والكتالوج

| الطريقة | الوصف |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | تهيئة المدير |
| `FoundryLocalManager.Instance` | الوصول إلى المدير المفرد |
| `manager.GetCatalogAsync()` | الحصول على كتالوج النماذج |
| `catalog.ListModelsAsync()` | سرد جميع النماذج المتاحة |
| `catalog.GetModelAsync(alias: "alias")` | الحصول على نموذج معين بالاسم المستعار |
| `catalog.GetCachedModelsAsync()` | سرد النماذج التي تم تنزيلها |
| `catalog.GetLoadedModelsAsync()` | سرد النماذج المحملة حاليًا |

> **ملاحظة هندسة C#:** إعادة تصميم C# SDK v0.8.0+ يجعل التطبيق **مكتفيًا ذاتيًا**؛ لا يتطلب Foundry Local CLI على جهاز المستخدم النهائي. يدير SDK إدارة النماذج والاستدلال أصليًا.

</details>

---

### التمرين 3: تنزيل وتحميل نموذج

يفصل SDK بين التنزيل (إلى القرص) والتحميل (إلى الذاكرة). هذا يسمح بتنزيل النماذج مسبقًا خلال الإعداد وتحميلها عند الطلب.

<details>
<summary><h3>🐍 بايثون</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# الخيار أ: خطوة بخطوة يدوية
manager = FoundryLocalManager()
manager.start_service()

# تحقق من ذاكرة التخزين المؤقت أولاً
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

# الخيار ب: تمهيد بسطر واحد (مُوصى به)
# مرر الاسم المستعار إلى المُنشئ - يبدأ الخدمة، ينزل، ويحمل تلقائيًا
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - طرق إدارة النماذج

| الطريقة | التوقيع | الوصف |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | تنزيل نموذج إلى التخزين المؤقت المحلي |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | تحميل نموذج إلى خادم الاستدلال |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | إلغاء تحميل نموذج من الخادم |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | سرد جميع النماذج المحملة حاليًا |

#### Python - طرق إدارة التخزين المؤقت

| الطريقة | التوقيع | الوصف |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | الحصول على مسار مجلد التخزين المؤقت |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | سرد كل النماذج التي تم تنزيلها |

</details>

<details>
<summary><h3>📘 جافاسكريبت</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// نهج خطوة بخطوة
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

#### JavaScript - طرق النماذج

| الطريقة | التوقيع | الوصف |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | سواء كان النموذج تم تنزيله مسبقًا |
| `model.download()` | `() => Promise<void>` | تنزيل النموذج إلى التخزين المؤقت المحلي |
| `model.load()` | `() => Promise<void>` | تحميل للنموذج إلى خادم الاستدلال |
| `model.unload()` | `() => Promise<void>` | إلغاء تحميل النموذج من خادم الاستدلال |
| `model.id` | `string` | المعرف الفريد للنموذج |

</details>

<details>
<summary><h3>💜 سي شارب</h3></summary>

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

#### C# - طرق النماذج

| الطريقة | الوصف |
|--------|-------------|
| `model.DownloadAsync(progress?)` | تنزيل المتغير المحدد |
| `model.LoadAsync()` | تحميل النموذج في الذاكرة |
| `model.UnloadAsync()` | إلغاء تحميل النموذج |
| `model.SelectVariant(variant)` | اختيار متغير معين (CPU/GPU/NPU) |
| `model.SelectedVariant` | المتغير المُختار حاليًا |
| `model.Variants` | كل المتغيرات المتاحة لهذا النموذج |
| `model.GetPathAsync()` | الحصول على مسار الملف المحلي |
| `model.GetChatClientAsync()` | الحصول على عميل دردشة أصلي (بدون الحاجة لـ OpenAI SDK) |
| `model.GetAudioClientAsync()` | الحصول على عميل صوتي للنص الصوتي |

</details>

---

### التمرين 4: تفقد بيانات تعريف النموذج

يحتوي كائن `FoundryModelInfo` على بيانات تعريف غنية لكل نموذج. فهم هذه الحقول يساعدك في اختيار النموذج المناسب لتطبيقك.

<details>
<summary><h3>🐍 بايثون</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# الحصول على معلومات مفصلة حول نموذج معين
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
<summary><h3>📘 جافاسكريبت</h3></summary>

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

#### حقول FoundryModelInfo

| الحقل | النوع | الوصف |
|-------|------|-------------|
| `alias` | string | الاسم المختصر (مثلاً `phi-3.5-mini`) |
| `id` | string | معرف النموذج الفريد |
| `version` | string | نسخة النموذج |
| `task` | string | `chat-completions` أو `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU، GPU، أو NPU |
| `execution_provider` | string | البيئة المساندة للتشغيل (CUDA، CPU، QNN، WebGPU، إلخ) |
| `file_size_mb` | int | الحجم على القرص بالميغابايت |
| `supports_tool_calling` | bool | هل يدعم النموذج استدعاء الوظائف/الأدوات |
| `publisher` | string | من نشر النموذج |
| `license` | string | اسم الرخصة |
| `uri` | string | URI الخاص بالنموذج |
| `prompt_template` | dict/null | قالب المطالبة، إن وجد |

---

### التمرين 5: إدارة دورة حياة النموذج

مارس دورة الحياة الكاملة: سرد → تنزيل → تحميل → استخدام → إلغاء تحميل.

<details>
<summary><h3>🐍 بايثون</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # نموذج صغير للاختبار السريع

manager = FoundryLocalManager()
manager.start_service()

# 1. تحقق مما هو موجود في الكتالوج
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. تحقق مما تم تنزيله بالفعل
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. تنزيل نموذج
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. تأكد من وجوده الآن في ذاكرة التخزين المؤقت
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. تحميله
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. تحقق مما تم تحميله
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. إلغاء تحميله
print(f"\nUnloading {alias}...")
manager.unload_model(alias)
loaded = manager.list_loaded_models()
print(f"Loaded models after unload: {len(loaded)}")
```

</details>

<details>
<summary><h3>📘 جافا سكريبت</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "qwen2.5-0.5b"; // نموذج صغير للاختبار السريع

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. احصل على النموذج من الكتالوج
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. حمّل إذا لزم الأمر
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. قم بتحميله
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. قم بإلغاء تحميله
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### التمرين 6: أنماط البدء السريع

تقدّم كل لغة اختصارًا لبدء الخدمة وتحميل نموذج في مكالمة واحدة. هذه هي **الأنماط الموصى بها** لمعظم التطبيقات.

<details>
<summary><h3>🐍 بايثون - تمهيد المُنشئ</h3></summary>

```python
from foundry_local import FoundryLocalManager

# مرر اسم مستعار إلى المُنشئ - هو يتولى كل شيء:
# 1. يبدأ الخدمة إذا لم تكن تعمل
# 2. ينزل النموذج إذا لم يكن مخزنًا مؤقتًا
# 3. يحمل النموذج في خادم الاستدلال
manager = FoundryLocalManager("phi-3.5-mini")

# جاهز للاستخدام فورًا
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

تتحكم باراميتر `bootstrap` (الافتراضي `True`) في هذا السلوك. عيّن `bootstrap=False` إذا كنت تريد التحكم اليدوي:

```python
# الوضع اليدوي - لا يحدث شيء تلقائيًا
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 جافا سكريبت - `create()` + كتالوج</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() يتعاملون مع كل شيء:
// 1. يبدأ الخدمة
// 2. يحصل على النموذج من الكتالوج
// 3. ينزل النموذج إذا لزم الأمر ويحمله
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// جاهز للاستخدام فوراً
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + كتالوج</h3></summary>

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

> **ملاحظة C#:** يستخدم SDK الخاص بـ C# `Configuration` للتحكم في اسم التطبيق، وتسجيل الأحداث، ومجلدات التخزين المؤقت، وحتى تعيين منفذ ويب سيرفر معين. هذا يجعله الأكثر قابلية للتكوين بين SDK الثلاثة.

</details>

---

### التمرين 7: عميل الدردشة الأصلي (لا حاجة لـ OpenAI SDK)

توفّر SDK لجافا سكريبت وC# طريقة سهولة `createChatClient()` التي تعيد عميل دردشة أصلي — لا حاجة لتثبيت أو تكوين OpenAI SDK بشكل منفصل.

<details>
<summary><h3>📘 جافا سكريبت - <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// إنشاء عميل دردشة مباشرة من النموذج — لا حاجة لاستيراد OpenAI
const chatClient = model.createChatClient();

// تُرجع completeChat كائن استجابة متوافق مع OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// البث يستخدم نمط استدعاء رد النداء
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

يدعم `ChatClient` أيضًا استدعاء الأدوات — مرّر الأدوات كوسيط ثاني:

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

> **متى تستخدم أي نمط:**
> - **`createChatClient()`** — النمذجة السريعة، عدد أقل من التبعية، كود أبسط
> - **OpenAI SDK** — تحكم كامل في البراميترات (درجة الحرارة، top_p، رموز التوقف، إلخ)، أفضل لتطبيقات الإنتاج

---

### التمرين 8: متغيرات النماذج واختيار العتاد

يمكن أن يكون للنماذج عدة **متغيرات** مُحسّنة لمعدات مختلفة. يختار SDK المتغير الأمثل تلقائيًا، ويمكنك أيضًا الفحص والاختيار يدويًا.

<details>
<summary><h3>📘 جافا سكريبت</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// قائمة بجميع المتغيرات المتاحة
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// يقوم SDK باختيار أفضل متغير تلقائيًا لجهازك
// للتجاوز، استخدم selectVariant():
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
<summary><h3>🐍 بايثون</h3></summary>

في بايثون، يختار SDK تلقائيًا المتغير الأمثل بناءً على العتاد. استخدم `get_model_info()` لمعرفة ما تم اختياره:

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

#### نماذج ذات متغيرات NPU

بعض النماذج لها متغيرات محسّنة لوحدات المعالجة العصبية (Qualcomm Snapdragon، Intel Core Ultra):

| النموذج | متغير NPU متاح |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **نصيحة:** على العتاد القادر على NPU، يختار SDK تلقائيًا متغير NPU عندما يكون متاحًا. لا تحتاج إلى تغيير الكود الخاص بك. لمشاريع C# على ويندوز، أضف حزمة NuGet `Microsoft.AI.Foundry.Local.WinML` لتمكين مزود تنفيذ QNN — يتم تقديم QNN كمكون إضافي EP عبر WinML.

---

### التمرين 9: ترقية النماذج وتحديث الكتالوج

يتم تحديث كتالوج النماذج بانتظام. استخدم هذه الطرق للتحقق من التحديثات وتطبيقها.

<details>
<summary><h3>🐍 بايثون</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# تحديث الكتالوج للحصول على أحدث قائمة للنماذج
manager.refresh_catalog()

# تحقق مما إذا كان للنموذج المخزن مؤقتًا إصدار أحدث متاح
if manager.is_model_upgradeable(alias):
    print(f"{alias} has a newer version available!")
    manager.upgrade_model(alias)
    print("Upgrade complete")
else:
    print(f"{alias} is up to date")
```

</details>

<details>
<summary><h3>📘 جافا سكريبت</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "UpgradeDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// تحديث الكتالوج لجلب أحدث قائمة بالنماذج
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// عرض جميع النماذج المتاحة بعد التحديث
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### التمرين 10: العمل مع نماذج التفكير

يتضمن نموذج **phi-4-mini-reasoning** سلسلة تفكير متسلسلة. يلف تفكيره الداخلي بعلامات `<think>...</think>` قبل إنتاج الإجابة النهائية. هذا مفيد للمهام التي تتطلب منطق متعدد الخطوات، أو رياضيات، أو حل مشاكل.

<details>
<summary><h3>🐍 بايثون</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning هو ~4.6 جيجابايت
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# النموذج يضع تفكيره داخل وسم <think>...</think>
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
<summary><h3>📘 جافا سكريبت</h3></summary>

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

// تحليل التفكير المتسلسل
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **متى تستخدم نماذج التفكير:**
> - مسائل رياضيات ومنطق
> - مهام التخطيط متعددة الخطوات
> - توليد كود معقد
> - مهام حيث يعزز عرض العمل الدقة
>
> **المقايضة:** نماذج التفكير تنتج عددًا أكبر من الرموز (قسم `<think>`) وهي أبطأ. للنصوص البسيطة والسؤال والجواب، يكون نموذج مثل phi-3.5-mini أسرع.

---

### التمرين 11: فهم الأسماء المستعارة واختيار العتاد

عندما تمرر **اسمًا مستعارًا** (مثل `phi-3.5-mini`) بدلاً من معرف نموذج كامل، يختار SDK تلقائيًا أفضل متغير لعتادك:

| العتاد | مزود التنفيذ المختار |
|----------|---------------------------|
| GPU من NVIDIA (CUDA) | `CUDAExecutionProvider` |
| NPU من Qualcomm | `QNNExecutionProvider` (عبر إضافة WinML) |
| NPU من Intel | `OpenVINOExecutionProvider` |
| GPU من AMD | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| أي جهاز (الاحتياطي) | `CPUExecutionProvider` أو `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# الاسم المستعار يحل إلى أفضل نسخة لمعداتك
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **نصيحة:** استخدم دائمًا الأسماء المستعارة في كود تطبيقك. عند النشر على جهاز المستخدم، يختار SDK المتغير الأمثل في وقت التشغيل - CUDA على NVIDIA، QNN على Qualcomm، CPU في أماكن أخرى.

---

### التمرين 12: خيارات التهيئة في C#

توفر فئة `Configuration` لـ SDK الخاص بـ C# تحكمًا دقيقًا في وقت التشغيل:

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

| الخاصية | الافتراضي | الوصف |
|----------|---------|-------------|
| `AppName` | (مطلوب) | اسم تطبيقك |
| `LogLevel` | `Information` | مستوى تسجيل الأحداث |
| `Web.Urls` | (ديناميكي) | تحديد منفذ ويب سيرفر معين |
| `AppDataDir` | افتراضي نظام التشغيل | المجلد الأساسي لبيانات التطبيق |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | موقع تخزين النماذج |
| `LogsDir` | `{AppDataDir}/logs` | موقع ملفات السجلات |

---

### التمرين 13: الاستخدام في المتصفح (جافا سكريبت فقط)

يشتمل SDK لجافا سكريبت على نسخة متوافقة مع المتصفح. في المتصفح، يجب بدء الخدمة يدويًا عبر CLI وتحديد عنوان URL للمضيف:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// ابدأ الخدمة يدويًا أولاً:
//   بدء خدمة الفاوندرى
// ثم استخدم الرابط من مخرجات سطر الأوامر
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// تصفح الكتالوج
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **قيود المتصفح:** نسخة المتصفح لا تدعم `startWebService()`. يجب التأكد من تشغيل خدمة Foundry Local مسبقًا قبل استخدام SDK في المتصفح.

---

## المرجع الكامل لواجهة البرمجة (API)

### بايثون

| الفئة | الطريقة | الوصف |
|----------|--------|-------------|
| **التهيئة** | `FoundryLocalManager(alias?, bootstrap=True)` | إنشاء مدير؛ اختيارياً تهيئة مع نموذج |
| **الخدمة** | `is_service_running()` | تحقق مما إذا كانت الخدمة تعمل |
| **الخدمة** | `start_service()` | بدء الخدمة |
| **الخدمة** | `endpoint` | عنوان نقطة نهاية API |
| **الخدمة** | `api_key` | مفتاح API |
| **الكتالوج** | `list_catalog_models()` | سرد جميع النماذج المتاحة |
| **الكتالوج** | `refresh_catalog()` | تحديث الكتالوج |
| **الكتالوج** | `get_model_info(alias_or_model_id)` | الحصول على بيانات النموذج |
| **التخزين المؤقت** | `get_cache_location()` | مسار مجلد التخزين المؤقت |
| **التخزين المؤقت** | `list_cached_models()` | سرد النماذج المحفوظة محليًا |
| **النموذج** | `download_model(alias_or_model_id)` | تحميل نموذج |
| **النموذج** | `load_model(alias_or_model_id, ttl=600)` | تحميل نموذج |
| **النموذج** | `unload_model(alias_or_model_id)` | إلغاء تحميل نموذج |
| **النموذج** | `list_loaded_models()` | سرد النماذج المحملة |
| **النموذج** | `is_model_upgradeable(alias_or_model_id)` | التحقق مما إذا كان هناك إصدار أحدث متاح |
| **النموذج** | `upgrade_model(alias_or_model_id)` | ترقية النموذج لأحدث إصدار |
| **الخدمة** | `httpx_client` | عميل HTTPX مُهيأ مسبقًا للاتصالات المباشرة مع API |

### جافا سكريبت

| الفئة | الطريقة | الوصف |
|----------|--------|-------------|
| **التهيئة** | `FoundryLocalManager.create(options)` | تهيئة وحدة SDK المفردة |
| **التهيئة** | `FoundryLocalManager.instance` | الوصول إلى مدير الوحدة المفردة |
| **الخدمة** | `manager.startWebService()` | بدء خدمة الويب |
| **الخدمة** | `manager.urls` | مصفوفة عناوين URL الأساسية للخدمة |
| **الكتالوج** | `manager.catalog` | الوصول إلى كتالوج النماذج |
| **الكتالوج** | `catalog.getModel(alias)` | الحصول على كائن نموذج بواسطة الاسم المستعار (يعيد Promise) |
| **النموذج** | `model.isCached` | ما إذا كان النموذج محمّل محليًا |
| **النموذج** | `model.download()` | تحميل النموذج |
| **النموذج** | `model.load()` | تحميل النموذج |
| **النموذج** | `model.unload()` | إلغاء تحميل النموذج |
| **النموذج** | `model.id` | معرف النموذج الفريد |
| **النموذج** | `model.alias` | الاسم المستعار للنموذج |
| **النموذج** | `model.createChatClient()` | الحصول على عميل دردشة أصلي (بدون الحاجة لـ OpenAI SDK) |
| **النموذج** | `model.createAudioClient()` | الحصول على عميل صوتي للتحويل إلى نص |
| **النموذج** | `model.removeFromCache()` | إزالة النموذج من التخزين المؤقت المحلي |
| **النموذج** | `model.selectVariant(variant)` | اختيار متغير عتاد معين |
| **النموذج** | `model.variants` | قائمة المتغيرات المتاحة لهذا النموذج |
| **النموذج** | `model.isLoaded()` | التحقق مما إذا كان النموذج محمّلًا حاليًا |
| **الكتالوج** | `catalog.getModels()` | سرد جميع النماذج المتاحة |
| **الكتالوج** | `catalog.getCachedModels()` | سرد النماذج المحفوظة |
| **الكتالوج** | `catalog.getLoadedModels()` | سرد النماذج المحملة حاليًا |
| **الكتالوج** | `catalog.updateModels()` | تحديث الكتالوج من الخدمة |
| **الخدمة** | `manager.stopWebService()` | إيقاف خدمة الويب Foundry Local |

### C# (v0.8.0+)

| الفئة | الطريقة | الوصف |
|----------|--------|-------------|
| **التهيئة** | `FoundryLocalManager.CreateAsync(config, logger)` | تهيئة المدير |
| **التهيئة** | `FoundryLocalManager.Instance` | الوصول إلى الوحدة المفردة |
| **الكتالوج** | `manager.GetCatalogAsync()` | الحصول على الكتالوج |
| **الكتالوج** | `catalog.ListModelsAsync()` | سرد جميع النماذج |
| **الكتالوج** | `catalog.GetModelAsync(alias)` | الحصول على نموذج محدد |
| **الكتالوج** | `catalog.GetCachedModelsAsync()` | سرد النماذج المخزنة محليًا |
| **الكتالوج** | `catalog.GetLoadedModelsAsync()` | سرد النماذج المحملة |
| **النموذج** | `model.DownloadAsync(progress?)` | تحميل نموذج |
| **النموذج** | `model.LoadAsync()` | تحميل نموذج |
| **النموذج** | `model.UnloadAsync()` | إلغاء تحميل نموذج |
| **النموذج** | `model.SelectVariant(variant)` | اختيار متغير عتاد |
| **النموذج** | `model.GetChatClientAsync()` | الحصول على عميل دردشة أصلي |
| **النموذج** | `model.GetAudioClientAsync()` | الحصول على عميل التفريغ الصوتي |
| **النموذج** | `model.GetPathAsync()` | الحصول على مسار الملف المحلي |
| **الكتالوج** | `catalog.GetModelVariantAsync(alias, variant)` | الحصول على متغير عتاد محدد |
| **الكتالوج** | `catalog.UpdateModelsAsync()` | تحديث الكتالوج |
| **الخادم** | `manager.StartWebServerAsync()` | بدء خادم الويب REST |
| **الخادم** | `manager.StopWebServerAsync()` | إيقاف خادم الويب REST |
| **التكوين** | `config.ModelCacheDir` | مجلد التخزين المؤقت |

---

## النقاط الرئيسية للمراجعة

| المفهوم | ما تعلمته |
|---------|-----------------|
| **SDK مقابل CLI** | يوفر SDK التحكم البرمجي - ضروري للتطبيقات |
| **طائرة التحكم** | يدير SDK الخدمات، النماذج، والتخزين المؤقت |
| **المنافذ الديناميكية** | استخدم دائمًا `manager.endpoint` (بايثون) أو `manager.urls[0]` (جافا سكريبت / C#) — لا تقم بتثبيت منفذ ثابت |
| **الأسماء المستعارة** | استخدم الأسماء المستعارة للاختيار التلقائي لأفضل نموذج متوافق مع العتاد |
| **البدء السريع** | بايثون: `FoundryLocalManager(alias)`, جافا سكريبت: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **إعادة تصميم C#** | الإصدار 0.8.0+ مستقل - لا حاجة لواجهة سطر الأوامر على أجهزة المستخدم النهائي |
| **دورة حياة النموذج** | الكتالوج → التنزيل → التحميل → الاستخدام → التفريغ |
| **FoundryModelInfo** | بيانات وصفية غنية: المهمة، الجهاز، الحجم، الترخيص، دعم أداة الاستدعاء |
| **ChatClient** | `createChatClient()` (جافا سكريبت) / `GetChatClientAsync()` (C#) للاستخدام بدون OpenAI |
| **الأنواع المتعددة** | للنماذج أنواع متعددة خاصة بالأجهزة (CPU، GPU، NPU)؛ اختيار تلقائي |
| **الترقيات** | بايثون: `is_model_upgradeable()` + `upgrade_model()` للحفاظ على تحديث النماذج |
| **تحديث الكتالوج** | `refresh_catalog()` (بايثون) / `updateModels()` (جافا سكريبت) لاكتشاف نماذج جديدة |

---

## الموارد

| المورد | الرابط |
|----------|------|
| مرجع SDK (جميع اللغات) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| التكامل مع SDKs الاستدلال | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| مرجع واجهة برمجة تطبيقات C# SDK | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| عينات C# SDK | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| موقع Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## الخطوات التالية

تابع إلى [الجزء 3: استخدام SDK مع OpenAI](part3-sdk-and-apis.md) لربط SDK بمكتبة عميل OpenAI وبناء أول تطبيق إكمال محادثة لك.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**إخلاء المسؤولية**:  
تمت ترجمة هذا المستند باستخدام خدمة الترجمة الآلية [Co-op Translator](https://github.com/Azure/co-op-translator). بينما نسعى جاهدين للدقة، يرجى العلم أن الترجمات الآلية قد تحتوي على أخطاء أو inaccuracies. يجب اعتبار الوثيقة الأصلية بلغتها الأصلية المصدر الموثوق به. بالنسبة للمعلومات الحرجة، يُنصح بالاعتماد على الترجمة المهنية البشرية. نحن غير مسؤولين عن أي سوء فهم أو تفسير خاطئ ناتج عن استخدام هذه الترجمة.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->