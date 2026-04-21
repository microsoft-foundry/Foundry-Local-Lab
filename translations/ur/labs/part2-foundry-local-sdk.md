![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# حصہ 2: Foundry Local SDK کی گہرائی میں جانچ

> **مقصد:** Foundry Local SDK میں مہارت حاصل کریں تاکہ پروگرام کے ذریعے ماڈلز، خدمات، اور کیشنگ کو مینج کیا جا سکے - اور سمجھیں کہ ایپلیکیشنز بنانے کے لیے SDK کو CLI پر کیوں ترجیح دی جاتی ہے۔

## جائزہ

حصہ 1 میں آپ نے **Foundry Local CLI** کا استعمال کرتے ہوئے ماڈلز کو ڈاؤن لوڈ اور انٹرایکٹو طریقے سے چلایا۔ CLI دریافت کے لیے بہترین ہے، لیکن جب آپ حقیقی ایپلیکیشنز بناتے ہیں تو آپ کو **پروگراماتی کنٹرول** کی ضرورت ہوتی ہے۔ Foundry Local SDK آپ کو یہ فراہم کرتا ہے - یہ **کنٹرول پلین** کو مینج کرتا ہے (سروس شروع کرنا، ماڈلز دریافت کرنا، ڈاؤن لوڈ کرنا، لوڈ کرنا) تاکہ آپ کا ایپلیکیشن کوڈ **ڈیٹا پلین** پر (پرومپٹس بھیجنا، کمپلیشنز وصول کرنا) توجہ دے سکے۔

یہ لیب آپ کو Python، JavaScript، اور C# کے ذریعے مکمل SDK API سطح سکھاتی ہے۔ آخر تک آپ ہر میتھڈ کو سمجھ جائیں گے اور کب کون سا استعمال کرنا ہے۔

## سیکھنے کے مقاصد

اس لیب کے آخر تک آپ قابل ہوں گے:

- وضاحت کریں کہ ایپلیکیشن ڈویلپمنٹ کے لیے SDK کو CLI پر کیوں ترجیح دی جاتی ہے
- Python، JavaScript، یا C# کے لیے Foundry Local SDK انسٹال کریں
- `FoundryLocalManager` کا استعمال کر کے سروس شروع کریں، ماڈلز مینیج کریں، اور کیٹلاگ سے سوال کریں
- پروگرام کے ذریعے ماڈلز کی فہرست بنائیں، ڈاؤن لوڈ کریں، لوڈ کریں، اور ان لوڈ کریں
- `FoundryModelInfo` کا استعمال کرتے ہوئے ماڈل کے میٹا ڈیٹا کا معائنہ کریں
- کیٹلاگ، کیش، اور لوڈ شدہ ماڈلز کے درمیان فرق سمجھیں
- کنسٹرکٹر بوسٹرپ (Python) اور `create()` + کیٹلاگ پیٹرن (JavaScript) کا استعمال کریں
- C# SDK کی دوبارہ ڈیزائن اور اس کی آبجیکٹ اورینٹڈ API کو سمجھیں

---

## ضروریات

| ضرورت | تفصیلات |
|-------------|---------|
| **Foundry Local CLI** | انسٹال شدہ اور آپ کے `PATH` پر ([حصہ 1](part1-getting-started.md)) |
| **لینگویج رن ٹائم** | **Python 3.9+** اور/یا **Node.js 18+** اور/یا **.NET 9.0+** |

---

## تصور: SDK بمقابلہ CLI - SDK کیوں استعمال کریں؟

| پہلو | CLI (`foundry` کمانڈ) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **استعمال کا معاملہ** | دریافت، دستی ٹیسٹنگ | ایپلیکیشن انٹیگریشن |
| **سروس مینجمنٹ** | دستی: `foundry service start` | خودکار: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **پورٹ دریافت** | CLI آؤٹ پُٹ سے پڑھنا | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **ماڈل ڈاؤن لوڈ** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **ایرر ہینڈلنگ** | ایگزٹ کوڈز، stderr | Exceptions، ٹائپ کیے ہوئے ایررز |
| **خود کاری** | شیل اسکرپٹس | مقامی زبان میں انٹیگریشن |
| **ڈیپلائمنٹ** | اینڈ یوزر مشین پر CLI چاہیے | C# SDK خودمختار ہو سکتا ہے (CLI کی ضرورت نہیں) |

> **اہم بصیرت:** SDK پوری لائف سائیکل کو ہینڈل کرتا ہے: سروس شروع کرنا، کیش چیک کرنا، مفقود ماڈلز ڈاؤن لوڈ کرنا، لوڈ کرنا، اور اینڈ پوائنٹ دریافت کرنا، چند لائنوں میں۔ آپ کے ایپلیکیشن کو CLI آؤٹ پٹ پارس کرنے یا سب پراسیسز کو مینیج کرنے کی ضرورت نہیں۔

---

## لیب مشقیں

### مشق 1: SDK انسٹال کریں

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

تنصیب کی تصدیق کریں:

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

تنصیب کی تصدیق کریں:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

دو NuGet پیکیجز ہیں:

| پیکیج | پلیٹ فارم | وضاحت |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | کراس پلیٹ فارم | Windows, Linux, macOS پر کام کرتا ہے |
| `Microsoft.AI.Foundry.Local.WinML` | صرف Windows | WinML ہارڈویئر ایکسیلریشن شامل کرتا ہے؛ پلگ ان ایکزیکیوشن پرووائیڈرز (جیسے Qualcomm NPU کے لیے QNN) ڈاؤن لوڈ اور انسٹال کرتا ہے |

**ونڈوز سیٹ اپ:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` فائل ایڈٹ کریں:

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

> **نوٹ:** ونڈوز پر، WinML پیکیج ایک سپر سیٹ ہے جو بیس SDK کے ساتھ QNN ایکزیکیوشن پرووائیڈر بھی شامل کرتا ہے۔ Linux/macOS پر بیس SDK استعمال ہوتا ہے۔ شرطی TFM اور پیکیج ریفرنسز پروجیکٹ کو مکمل کراس پلیٹ فارم رکھتے ہیں۔

پروجیکٹ کے روٹ میں `nuget.config` بنائیں:

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

پیکیجز ریسٹور کریں:

```bash
dotnet restore
```

</details>

---

### مشق 2: سروس شروع کریں اور کیٹلاگ کی فہرست نکالیں

کوئی بھی ایپلیکیشن سب سے پہلے Foundry Local سروس شروع کرتی ہے اور معلوم کرتی ہے کہ کون سے ماڈلز دستیاب ہیں۔

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# مینیجر بنائیں اور سروس شروع کریں
manager = FoundryLocalManager()
manager.start_service()

# کیٹلاگ میں دستیاب تمام ماڈلز کی فہرست بنائیں
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - سروس مینجمنٹ کے طریقے

| طریقہ | دستخط | وضاحت |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | چیک کریں کہ سروس چل رہی ہے یا نہیں |
| `start_service()` | `() -> None` | Foundry Local سروس شروع کریں |
| `service_uri` | `@property -> str` | بنیادی سروس یو آر آئی |
| `endpoint` | `@property -> str` | API اینڈ پوائنٹ (سروس URI + `/v1`) |
| `api_key` | `@property -> str` | API کلید (ماحول یا ڈیفالٹ پلیس ہولڈر سے) |

#### Python SDK - کیٹلاگ مینجمنٹ کے طریقے

| طریقہ | دستخط | وضاحت |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | کیٹلاگ میں تمام ماڈلز کی فہرست بنائیں |
| `refresh_catalog()` | `() -> None` | سروس سے کیٹلاگ کو ریفریش کریں |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | ایک خاص ماڈل کی معلومات حاصل کریں |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// مینیجر بنائیں اور سروس شروع کریں
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// کیٹلاگ کو براؤز کریں
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - مینیجر کے طریقے

| طریقہ | دستخط | وضاحت |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK سنگلٹن کو initialise کریں |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | سنگلٹن مینیجر تک رسائی |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local ویب سروس شروع کریں |
| `manager.urls` | `string[]` | سروس کے بنیادی URLs کی فہرست |

#### JavaScript SDK - کیٹلاگ اور ماڈل کے طریقے

| طریقہ | دستخط | وضاحت |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | ماڈل کیٹلاگ تک رسائی |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | alias کے ذریعے ماڈل آبجیکٹ حاصل کریں |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK ورژن 0.8.0+ آبجیکٹ اورینٹڈ آرکیٹیکچر استعمال کرتا ہے جس میں `Configuration`, `Catalog`, اور `Model` آبجیکٹ شامل ہیں:

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

#### C# SDK - کلیدی کلاسیں

| کلاس | مقصد |
|-------|---------|
| `Configuration` | ایپ نام، لاگ لیول، کیش ڈائریکٹری، ویب سرور URLs سیٹ کریں |
| `FoundryLocalManager` | مین انٹری پوائنٹ - `CreateAsync()` کے ذریعے بنائیں، `.Instance` سے رسائی حاصل کریں |
| `Catalog` | کیٹلاگ براؤز، تلاش، اور ماڈلز حاصل کریں |
| `Model` | ایک خاص ماڈل کی نمائندگی - ڈاؤن لوڈ، لوڈ، کلائنٹس حاصل کریں |

#### C# SDK - مینیجر اور کیٹلاگ کے طریقے

| طریقہ | وضاحت |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | مینیجر کو initialise کریں |
| `FoundryLocalManager.Instance` | سنگلٹن مینیجر تک رسائی |
| `manager.GetCatalogAsync()` | ماڈل کیٹلاگ حاصل کریں |
| `catalog.ListModelsAsync()` | تمام دستیاب ماڈلز کی فہرست بنائیں |
| `catalog.GetModelAsync(alias: "alias")` | alias کے ذریعے مخصوص ماڈل حاصل کریں |
| `catalog.GetCachedModelsAsync()` | ڈاؤن لوڈ شدہ ماڈلز کی فہرست بنائیں |
| `catalog.GetLoadedModelsAsync()` | اس وقت لوڈ شدہ ماڈلز کی فہرست بنائیں |

> **C# آرکیٹیکچر نوٹ:** C# SDK ورژن 0.8.0+ دوبارہ ڈیزائن سے ایپلیکیشن **خودمختار** ہو جاتی ہے؛ اسے اینڈ یوزر کی مشین پر Foundry Local CLI کی ضرورت نہیں ہوتی۔ SDK ماڈل مینجمنٹ اور انفیرینس کو مقامی طور پر سنبھالتا ہے۔

</details>

---

### مشق 3: ماڈل ڈاؤن لوڈ اور لوڈ کریں

SDK ڈاؤن لوڈنگ (ڈسک پر) اور لوڈنگ (میموری میں) کو الگ کرتا ہے۔ اس سے آپ سیٹ اپ کے دوران ماڈلز پری ڈاؤن لوڈ کر سکتے ہیں اور ضرورت پر لوڈ کر سکتے ہیں۔

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# آپشن A: دستی قدم بہ قدم
manager = FoundryLocalManager()
manager.start_service()

# پہلے کیش چیک کریں
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

# آپشن B: ایک لائنر بوٹ اسٹریپ (تجویز کردہ)
# کنسٹرکٹر کو علیاس پاس کریں - یہ سروس شروع کرتا ہے، ڈاؤن لوڈ کرتا ہے، اور خود بخود لوڈ کرتا ہے
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - ماڈل مینجمنٹ کے طریقے

| طریقہ | دستخط | وضاحت |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | ماڈل کو مقامی کیش میں ڈاؤن لوڈ کریں |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | ماڈل کو انفیرینس سرور میں لوڈ کریں |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | ماڈل کو سرور سے ان لوڈ کریں |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | تمام اس وقت لوڈ شدہ ماڈلز کی فہرست بنائیں |

#### Python - کیش مینجمنٹ کے طریقے

| طریقہ | دستخط | وضاحت |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | کیش ڈائریکٹری کا پتہ حاصل کریں |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | تمام ڈاؤن لوڈ شدہ ماڈلز کی فہرست بنائیں |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// مرحلہ بہ مرحلہ طریقہ کار
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

#### JavaScript - ماڈل کے طریقے

| طریقہ | دستخط | وضاحت |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | آیا ماڈل پہلے سے ڈاؤن لوڈ ہے یا نہیں |
| `model.download()` | `() => Promise<void>` | ماڈل کو مقامی کیش میں ڈاؤن لوڈ کریں |
| `model.load()` | `() => Promise<void>` | انفیرینس سرور میں لوڈ کریں |
| `model.unload()` | `() => Promise<void>` | انفیرینس سرور سے ان لوڈ کریں |
| `model.id` | `string` | ماڈل کا یونیک شناخت کنندہ |

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

#### C# - ماڈل کے طریقے

| طریقہ | وضاحت |
|--------|-------------|
| `model.DownloadAsync(progress?)` | منتخب ورژن ڈاؤن لوڈ کریں |
| `model.LoadAsync()` | ماڈل کو میموری میں لوڈ کریں |
| `model.UnloadAsync()` | ماڈل کو ان لوڈ کریں |
| `model.SelectVariant(variant)` | مخصوص ورژن منتخب کریں (CPU/GPU/NPU) |
| `model.SelectedVariant` | اس وقت منتخب شدہ ورژن |
| `model.Variants` | اس ماڈل کے تمام دستیاب ورژنز |
| `model.GetPathAsync()` | مقامی فائل کا راستہ حاصل کریں |
| `model.GetChatClientAsync()` | نیٹو چیٹ کلائنٹ حاصل کریں (OpenAI SDK کی ضرورت نہیں) |
| `model.GetAudioClientAsync()` | ٹرانسکرپشن کے لیے آڈیو کلائنٹ حاصل کریں |

</details>

---

### مشق 4: ماڈل میٹا ڈیٹا کا معائنہ کریں

`FoundryModelInfo` آبجیکٹ ہر ماڈل کے بارے میں مکمل میٹا ڈیٹا فراہم کرتا ہے۔ ان فیلڈز کو سمجھنا آپ کی ایپلیکیشن کے لیے صحیح ماڈل منتخب کرنے میں مدد دیتا ہے۔

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# کسی خاص ماڈل کے بارے میں تفصیلی معلومات حاصل کریں
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

#### FoundryModelInfo فیلڈز

| فیلڈ | قسم | وضاحت |
|-------|------|-------------|
| `alias` | string | مختصر نام (مثلاً `phi-3.5-mini`) |
| `id` | string | منفرد ماڈل شناخت کنندہ |
| `version` | string | ماڈل ورژن |
| `task` | string | `chat-completions` یا `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU، GPU، یا NPU |
| `execution_provider` | string | رن ٹائم بیکینڈ (CUDA، CPU، QNN، WebGPU، وغیرہ) |
| `file_size_mb` | int | ڈسک پر سائز میگا بائٹس میں |
| `supports_tool_calling` | bool | آیا ماڈل فنکشن/ٹول کالنگ کی حمایت کرتا ہے |
| `publisher` | string | کس نے ماڈل شائع کیا |
| `license` | string | لائیسنس کا نام |
| `uri` | string | ماڈل یو آر آئی |
| `prompt_template` | dict/null | اگر ہو تو پرومپٹ ٹیمپلیٹ |

---

### مشق 5: ماڈل لائف سائیکل مینج کریں

پورے لائف سائیکل کی مشق کریں: فہرست → ڈاؤن لوڈ → لوڈ → استعمال → ان لوڈ۔

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # چھوٹا ماڈل تیز جانچ کے لیے

manager = FoundryLocalManager()
manager.start_service()

# 1. کیٹلاگ میں کیا ہے چیک کریں
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. جو پہلے سے ڈاؤن لوڈ ہو چکا ہے چیک کریں
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. ایک ماڈل ڈاؤن لوڈ کریں
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. تصدیق کریں کہ یہ اب کیش میں ہے
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. اسے لوڈ کریں
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. جو لوڈ ہوا ہے وہ چیک کریں
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. اسے ان لوڈ کریں
print(f"\nUnloading {alias}...")
manager.unload_model(alias)
loaded = manager.list_loaded_models()
print(f"Loaded models after unload: {len(loaded)}")
```

</details>

<details>
<summary><h3>📘 جاوااسکرپٹ</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "qwen2.5-0.5b"; // چھوٹا ماڈل فوری جانچ کے لیے

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. ماڈل کیٹلاگ سے حاصل کریں
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. اگر ضرورت ہو تو ڈاؤن لوڈ کریں
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. اسے لوڈ کریں
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. اسے ان لوڈ کریں
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### مشق 6: فوری آغاز کے نمونے

ہر زبان ایک ایسی شارٹ کٹ فراہم کرتی ہے جو خدمت شروع کرنے اور ایک ماڈل کو ایک کال میں لوڈ کرنے کے لیے ہوتی ہے۔ یہ زیادہ تر ایپلیکیشنز کے لیے **تجویز کردہ نمونے** ہیں۔

<details>
<summary><h3>🐍 پائتھون - کنسٹرکٹر بوٹ اسٹریپ</h3></summary>

```python
from foundry_local import FoundryLocalManager

# کنسٹرکٹر کو ایک عرفی نام دیں - یہ سب کچھ سنبھال لیتا ہے:
# 1. اگر سروس چل نہیں رہی تو اسے شروع کرتا ہے
# 2. اگر ماڈل کیش نہیں ہوا تو اسے ڈاؤن لوڈ کرتا ہے
# 3. ماڈل کو انفرنس سرور میں لوڈ کرتا ہے
manager = FoundryLocalManager("phi-3.5-mini")

# فوری طور پر استعمال کے لیے تیار
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` پیرامیٹر (ڈیفالٹ `True`) اس رویے کو کنٹرول کرتا ہے۔ اگر آپ دستی کنٹرول چاہتے ہیں تو `bootstrap=False` سیٹ کریں:

```python
# دستی وضع - کچھ خود بخود نہیں ہوتا
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 جاوااسکرپٹ - `create()` + کیٹلاگ</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() سب کچھ سنبھالتے ہیں:
// 1. سروس شروع کرتا ہے
// 2. ماڈل کو کیٹلاگ سے حاصل کرتا ہے
// 3. اگر ضرورت ہو تو ڈاؤن لوڈ کرتا ہے اور ماڈل کو لوڈ کرتا ہے
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// فوری استعمال کے لیے تیار ہے
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + کیٹلاگ</h3></summary>

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

> **C# نوٹ:** C# SDK `Configuration` استعمال کرتا ہے ایپ کا نام، لاگنگ، کیش ڈائریکٹریز، اور ایک مخصوص ویب سرور پورٹ پن کرنے کے لیے۔ یہ تینوں SDKs میں سب سے زیادہ قابل تخصیص بناتا ہے۔

</details>

---

### مشق 7: نیٹو ChatClient (کسی OpenAI SDK کی ضرورت نہیں)

جاوااسکرپٹ اور C# SDKs ایک `createChatClient()` سہولت طریقہ فراہم کرتے ہیں جو ایک نیٹو چیٹ کلائنٹ واپس کرتا ہے — OpenAI SDK کو الگ سے انسٹال یا کنفیگر کرنے کی ضرورت نہیں۔

<details>
<summary><h3>📘 جاوااسکرپٹ - <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// ماڈل سے براہ راست ایک ChatClient بنائیں — کوئی OpenAI درآمد کی ضرورت نہیں
const chatClient = model.createChatClient();

// completeChat ایک OpenAI-مطابق جواب آبجیکٹ واپس کرتا ہے
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// اسٹریم کرنے کے لیے کال بیک پیٹرن استعمال کرتا ہے
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` ٹول کالنگ کو بھی سپورٹ کرتا ہے — دوسرا آرگیومنٹ کے طور پر ٹولز پاس کریں:

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

> **کون سا نمونہ کب استعمال کریں:**
> - **`createChatClient()`** — فوری پروٹوٹائپنگ، کم انحصار، آسان کوڈ
> - **OpenAI SDK** — پیرامیٹرز (temperature, top_p, stop tokens وغیرہ) پر مکمل کنٹرول، پروڈکشن اپلیکیشنز کے لیے بہتر

---

### مشق 8: ماڈل کی اقسام اور ہارڈویئر کا انتخاب

ماڈلز کے کئی **اقسام** ہوسکتے ہیں جو مختلف ہارڈویئر کے لیے بہتر کی گئی ہوں۔ SDK خودکار طور پر بہترین قسم منتخب کرتا ہے، لیکن آپ دستی طور پر بھی معائنہ اور انتخاب کرسکتے ہیں۔

<details>
<summary><h3>📘 جاوااسکرپٹ</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// دستیاب تمام اقسام کی فہرست بنائیں
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// ایس ڈی کے خود بخود آپ کے ہارڈویئر کے لئے بہترین قسم منتخب کرتا ہے
// اوور رائیڈ کرنے کے لئے، selectVariant() استعمال کریں:
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
<summary><h3>🐍 پائتھون</h3></summary>

پائتھون میں، SDK خودکار طور پر ہارڈویئر کی بنیاد پر بہترین قسم منتخب کرتا ہے۔ منتخب شدہ معلوم کرنے کے لیے `get_model_info()` استعمال کریں:

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

#### NPU اقسام کے ساتھ ماڈلز

کچھ ماڈلز NPU-مخصوص اقسام رکھتے ہیں جو نیورل پروسیسنگ یونٹس (Qualcomm Snapdragon, Intel Core Ultra) والے ڈیوائسز کے لیے بہتر بنائی گئی ہیں:

| ماڈل | NPU قسم دستیاب ہے |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **مشورہ:** NPU-قابل ہارڈویئر پر، SDK دستیاب ہونے پر خودکار طور پر NPU قسم منتخب کرتا ہے۔ آپ کو اپنے کوڈ میں تبدیلی کرنے کی ضرورت نہیں۔ ونڈوز پر C# پروجیکٹس کے لیے `Microsoft.AI.Foundry.Local.WinML` NuGet پیکیج شامل کریں تاکہ QNN execution provider فعال ہو سکے — QNN WinML کے ذریعے ایک پلگ ان EP کے طور پر مہیا کیا جاتا ہے۔

---

### مشق 9: ماڈل اپ گریڈز اور کیٹلاگ ریفریش

ماڈل کیٹلاگ وقتاً فوقتاً اپ ڈیٹ ہوتی ہے۔ اپ ڈیٹس چیک کرنے اور لاگو کرنے کے لیے یہ طریقے استعمال کریں۔

<details>
<summary><h3>🐍 پائتھون</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# تازہ ترین ماڈل فہرست حاصل کرنے کے لیے کیٹلاگ کو ریفریش کریں
manager.refresh_catalog()

# چیک کریں کہ آیا کیش شدہ ماڈل کا کوئی نیا ورژن دستیاب ہے یا نہیں
if manager.is_model_upgradeable(alias):
    print(f"{alias} has a newer version available!")
    manager.upgrade_model(alias)
    print("Upgrade complete")
else:
    print(f"{alias} is up to date")
```

</details>

<details>
<summary><h3>📘 جاوااسکرپٹ</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "UpgradeDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// تازہ ترین ماڈل فہرست حاصل کرنے کے لیے کیٹلاگ کو ریفریش کریں
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// ریفریش کے بعد تمام دستیاب ماڈلز کی فہرست بنائیں
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### مشق 10: ریئزننگ ماڈلز کے ساتھ کام کرنا

**phi-4-mini-reasoning** ماڈل چین آف تھوٹ ریئزننگ شامل کرتا ہے۔ یہ اپنے اندرونی سوچ کو `<think>...</think>` ٹیگز میں لپیٹ کر فائنل جواب دیتا ہے۔ یہ ایسے کاموں کے لیے مفید ہے جنہیں کثیر-مرحلہ منطق، ریاضی، یا مسئلہ حل کرنے کی ضرورت ہو۔

<details>
<summary><h3>🐍 پائتھون</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning تقریباً 4.6 جی بی ہے
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# ماڈل اپنی سوچ کو <think>...</think> ٹیگز میں لپیٹتا ہے
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
<summary><h3>📘 جاوااسکرپٹ</h3></summary>

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

// سوچ کے سلسلے کی تجزیہ کریں
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **ریئزننگ ماڈلز کب استعمال کریں:**
> - ریاضی اور منطقی مسائل
> - کثیر مرحلہ منصوبہ بندی کے کام
> - پیچیدہ کوڈ جنریشن
> - وہ کام جہاں عمل دکھانے سے درستگی بہتر ہو جائے
>
> **ٹریڈ آف:** ریئزننگ ماڈلز زیادہ ٹوکنز (مثلاً `<think>` سیکشن) پیدا کرتے ہیں اور سست ہوتے ہیں۔ آسان سوال و جواب کے لیے، جیسا کہ phi-3.5-mini، تیز تر ہے۔

---

### مشق 11: عرفیات اور ہارڈویئر انتخاب کو سمجھنا

جب آپ ایک **عرفی نام** (جیسے `phi-3.5-mini`) پورے ماڈل ID کے بجائے پاس کرتے ہیں تو SDK خودکار طور پر آپ کے ہارڈویئر کے لیے بہترین قسم منتخب کرتا ہے:

| ہارڈویئر | منتخب شدہ execution provider |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML پلگ ان کے ذریعے) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| کوئی بھی ڈیوائس (فیل بیک) | `CPUExecutionProvider` یا `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ایلیاس آپ کے ہارڈ ویئر کے لیے بہترین متبادل حل کرتا ہے
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **مشورہ:** ہمیشہ اپنے ایپ کوڈ میں عرفیات استعمال کریں۔ جب آپ صارف کے کمپیوٹر پر ڈیپلائی کرتے ہیں، SDK رن ٹائم پر بہترین قسم منتخب کرتا ہے — NVIDIA پر CUDA، Qualcomm پر QNN، اور دیگر جگہوں پر CPU۔

---

### مشق 12: C# کنفیگریشن اختیارات

C# SDK کا `Configuration` کلاس رن ٹائم کے بارے میں باریک بینی سے کنٹرول فراہم کرتا ہے:

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

| پراپرٹی | ڈیفالٹ | وضاحت |
|----------|---------|-------------|
| `AppName` | (ضروری) | آپ کی ایپلیکیشن کا نام |
| `LogLevel` | `Information` | لاگنگ کی تفصیل |
| `Web.Urls` | (متحرک) | ویب سرور کے لیے مخصوص پورٹ پن کرنا |
| `AppDataDir` | OS کا ڈیفالٹ | ایپ ڈیٹا کے لیے بنیادی ڈائریکٹری |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | ماڈلز کہاں ذخیرہ ہوں |
| `LogsDir` | `{AppDataDir}/logs` | لاگز کہاں لکھے جائیں |

---

### مشق 13: براؤزر میں استعمال (صرف جاوااسکرپٹ)

جاوااسکرپٹ SDK ایک براؤزر-مطابقت ورژن شامل کرتا ہے۔ براؤزر میں، آپ کو CLI کے ذریعے خدمت دستی طور پر شروع کرنی ہوگی اور میزبان URL مخصوص کرنا ہوگا:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// پہلے دستی طور پر سروس شروع کریں:
//   foundry service start
// پھر CLI آؤٹ پٹ سے URL استعمال کریں
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// کیٹلاگ براؤز کریں
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **براؤزر کی حدود:** براؤزر ورژن `startWebService()` کی حمایت نہیں کرتا۔ آپ کو یہ یقینی بنانا ہوگا کہ Foundry Local سروس پہلے سے چل رہی ہو تب ہی SDK براؤزر میں استعمال کریں۔

---

## مکمل API حوالہ

### پائتھون

| زمرہ | طریقہ | وضاحت |
|----------|--------|-------------|
| **ابتدائیہ** | `FoundryLocalManager(alias?, bootstrap=True)` | مینیجر بنائیں؛ اختیاری طور پر ماڈل کے ساتھ بوٹ اسٹریپ کریں |
| **سروس** | `is_service_running()` | چیک کریں کہ خدمت چل رہی ہے یا نہیں |
| **سروس** | `start_service()` | خدمت شروع کریں |
| **سروس** | `endpoint` | API endpoint کا URL |
| **سروس** | `api_key` | API کی|
| **کیٹلاگ** | `list_catalog_models()` | تمام دستیاب ماڈلز کی فہرست بنائیں |
| **کیٹلاگ** | `refresh_catalog()` | کیٹلاگ کو تازہ کریں |
| **کیٹلاگ** | `get_model_info(alias_or_model_id)` | ماڈل میٹاڈیٹا حاصل کریں |
| **کیش** | `get_cache_location()` | کیش ڈائریکٹری کا راستہ |
| **کیش** | `list_cached_models()` | ڈاؤن لوڈ شدہ ماڈلز کی فہرست |
| **ماڈل** | `download_model(alias_or_model_id)` | ماڈل ڈاؤن لوڈ کریں |
| **ماڈل** | `load_model(alias_or_model_id, ttl=600)` | ماڈل لوڈ کریں |
| **ماڈل** | `unload_model(alias_or_model_id)` | ماڈل ان لوڈ کریں |
| **ماڈل** | `list_loaded_models()` | لوڈ کیے گئے ماڈلز کی فہرست |
| **ماڈل** | `is_model_upgradeable(alias_or_model_id)` | چیک کریں کہ نیا ورژن دستیاب ہے یا نہیں |
| **ماڈل** | `upgrade_model(alias_or_model_id)` | ماڈل کو تازہ ترین ورژن میں اپ گریڈ کریں |
| **سروس** | `httpx_client` | براہ راست API کالز کے لیے پہلے سے کنفیگر کردہ HTTPX کلائنٹ |

### جاوااسکرپٹ

| زمرہ | طریقہ | وضاحت |
|----------|--------|-------------|
| **ابتدائیہ** | `FoundryLocalManager.create(options)` | SDK singleton کو initialise کریں |
| **ابتدائیہ** | `FoundryLocalManager.instance` | singleton مینیجر تک رسائی |
| **سروس** | `manager.startWebService()` | ویب سروس شروع کریں |
| **سروس** | `manager.urls` | سروس کے بیس URLs کا ارے |
| **کیٹلاگ** | `manager.catalog` | ماڈل کیٹلاگ تک رسائی |
| **کیٹلاگ** | `catalog.getModel(alias)` | عرف کے ذریعے ماڈل آبجیکٹ حاصل کریں (Promise لوٹاتا ہے) |
| **ماڈل** | `model.isCached` | ماڈل ڈاؤن لوڈ ہوا ہے یا نہیں |
| **ماڈل** | `model.download()` | ماڈل ڈاؤن لوڈ کریں |
| **ماڈل** | `model.load()` | ماڈل لوڈ کریں |
| **ماڈل** | `model.unload()` | ماڈل ان لوڈ کریں |
| **ماڈل** | `model.id` | ماڈل کا منفرد شناخت کار |
| **ماڈل** | `model.alias` | ماڈل کا عرفی نام |
| **ماڈل** | `model.createChatClient()` | نیٹو چیٹ کلائنٹ حاصل کریں (کوئی OpenAI SDK نہیں چاہیے) |
| **ماڈل** | `model.createAudioClient()` | آڈیو بولاوت کلائنٹ حاصل کریں |
| **ماڈل** | `model.removeFromCache()` | لوکل کیش سے ماڈل ہٹائیں |
| **ماڈل** | `model.selectVariant(variant)` | مخصوص ہارڈویئر قسم منتخب کریں |
| **ماڈل** | `model.variants` | اس ماڈل کے دستیاب اقسام کا ارے |
| **ماڈل** | `model.isLoaded()` | چیک کریں کہ ماڈل لوڈ ہے یا نہیں |
| **کیٹلاگ** | `catalog.getModels()` | تمام دستیاب ماڈلز کی فہرست |
| **کیٹلاگ** | `catalog.getCachedModels()` | ڈاؤن لوڈ شدہ ماڈلز کی فہرست |
| **کیٹلاگ** | `catalog.getLoadedModels()` | اس وقت لوڈ کیے گئے ماڈلز کی فہرست |
| **کیٹلاگ** | `catalog.updateModels()` | سروس سے کیٹلاگ کو تازہ کریں |
| **سروس** | `manager.stopWebService()` | Foundry Local ویب سروس روکیں |

### C# (v0.8.0+)

| زمرہ | طریقہ | وضاحت |
|----------|--------|-------------|
| **ابتدائیہ** | `FoundryLocalManager.CreateAsync(config, logger)` | مینیجر کو initialise کریں |
| **ابتدائیہ** | `FoundryLocalManager.Instance` | singleton تک رسائی |
| **کیٹلاگ** | `manager.GetCatalogAsync()` | کیٹلاگ حاصل کریں |
| **کیٹلاگ** | `catalog.ListModelsAsync()` | تمام ماڈلز کی فہرست |
| **کیٹلاگ** | `catalog.GetModelAsync(alias)` | مخصوص ماڈل حاصل کریں |
| **کیٹلاگ** | `catalog.GetCachedModelsAsync()` | کیشڈ ماڈلز کی فہرست |
| **کیٹلاگ** | `catalog.GetLoadedModelsAsync()` | لوڈ شدہ ماڈلز کی فہرست |
| **ماڈل** | `model.DownloadAsync(progress?)` | ماڈل ڈاؤن لوڈ کریں |
| **ماڈل** | `model.LoadAsync()` | ماڈل لوڈ کریں |
| **ماڈل** | `model.UnloadAsync()` | ماڈل ان لوڈ کریں |
| **ماڈل** | `model.SelectVariant(variant)` | ہارڈویئر کی قسم منتخب کریں |
| **ماڈل** | `model.GetChatClientAsync()` | نیٹو چیٹ کلائنٹ حاصل کریں |
| **ماڈل** | `model.GetAudioClientAsync()` | آڈیو ٹرانسکرپشن کلائنٹ حاصل کریں |
| **ماڈل** | `model.GetPathAsync()` | لوکل فائل کا راستہ حاصل کریں |
| **کیٹلاگ** | `catalog.GetModelVariantAsync(alias, variant)` | مخصوص ہارڈویئر قسم حاصل کریں |
| **کیٹلاگ** | `catalog.UpdateModelsAsync()` | کیٹلاگ کو تازہ کریں |
| **سرور** | `manager.StartWebServerAsync()` | REST ویب سرور شروع کریں |
| **سرور** | `manager.StopWebServerAsync()` | REST ویب سرور بند کریں |
| **کنفیگریشن** | `config.ModelCacheDir` | کیش ڈائریکٹری |

---

## اہم نکات

| تصور | آپ نے کیا سیکھا |
|---------|-----------------|
| **SDK بمقابلہ CLI** | SDK پروگراماتی کنٹرول فراہم کرتا ہے - ایپلیکیشنز کے لیے ضروری |
| **کنٹرول پلین** | SDK خدمات، ماڈلز، اور کیشنگ کو منظم کرتا ہے |
| **متحرک پورٹس** | ہمیشہ `manager.endpoint` (پائتھون) یا `manager.urls[0]` (JS/C#) استعمال کریں - پورٹ ہارڈ کوڈ نہ کریں |
| **عرفیات** | خودکار ہارڈویئر کے لیے بہترین ماڈل انتخاب کے لیے عرفیات استعمال کریں |
| **فوری آغاز** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# دوبارہ ڈیزائن** | v0.8.0+ خود مختار ہے - اینڈ یوزر مشینوں پر CLI کی ضرورت نہیں |
| **ماڈل کا لائف سائیکل** | کیٹلاگ → ڈاؤن لوڈ → لوڈ → استعمال → ان لوڈ |
| **FoundryModelInfo** | مکمل میٹا ڈیٹا: ٹاسک، ڈیوائس، سائز، لائسنس، ٹول کالنگ سپورٹ |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) بغیر OpenAI استعمال کے لیے |
| **ورائئنٹس** | ماڈل کے ہارڈ ویئر مخصوص ورائئنٹس ہوتے ہیں (CPU, GPU, NPU); خودکار طور پر منتخب ہوتے ہیں |
| **اپ گریڈز** | Python: `is_model_upgradeable()` + `upgrade_model()` ماڈلز کو تازہ رکھنے کے لیے |
| **کیٹلاگ ریفریش** | `refresh_catalog()` (Python) / `updateModels()` (JS) نئے ماڈلز دریافت کرنے کے لیے |

---

## وسائل

| وسیلہ | لنک |
|----------|------|
| SDK حوالہ (تمام زبانیں) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| انفیرینس SDKs کے ساتھ انٹیگریٹ کریں | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API حوالہ | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK نمونے | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local ویب سائٹ | [foundrylocal.ai](https://foundrylocal.ai) |

---

## اگلے اقدامات

[حصہ 3: SDK کو OpenAI کے ساتھ استعمال کرنا](part3-sdk-and-apis.md) جاری رکھیں تاکہ SDK کو OpenAI کلائنٹ لائبریری سے منسلک کریں اور اپنی پہلی چیٹ کمپلیشن ایپلیکیشن بنائیں۔