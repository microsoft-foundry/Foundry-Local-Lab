![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# بخش ۲: بررسی عمیق Foundry Local SDK

> **هدف:** تسلط بر Foundry Local SDK برای مدیریت مدل‌ها، سرویس‌ها و کش به صورت برنامه‌نویسی شده - و درک اینکه چرا SDK رویکرد پیشنهادی نسبت به CLI برای ساخت برنامه‌ها است.

## مرور کلی

در بخش ۱ از **Foundry Local CLI** برای دانلود و اجرای تعاملی مدل‌ها استفاده کردید. CLI برای اکتشاف عالی است، اما وقتی برنامه‌های واقعی می‌سازید، به **کنترل برنامه‌نویسی** نیاز دارید. Foundry Local SDK این امکان را به شما می‌دهد - مدیریت **صفحه کنترل** (شروع سرویس، کشف مدل‌ها، دانلود، بارگذاری) را بر عهده می‌گیرد تا کد برنامه شما بتواند بر **صفحه داده** (ارسال پرسش‌ها، دریافت تکمیل‌ها) تمرکز کند.

این آزمایشگاه کل API SDK را در زبان‌های Python، JavaScript و C# آموزش می‌دهد. در پایان هر متدی را خواهید شناخت و می‌دانید چه زمانی از هرکدام استفاده کنید.

## اهداف آموزشی

در پایان این آزمایشگاه قادر خواهید بود:

- توضیح دهید چرا SDK نسبت به CLI برای توسعه برنامه ترجیح داده می‌شود
- نصب Foundry Local SDK برای Python، JavaScript یا C#
- استفاده از `FoundryLocalManager` برای شروع سرویس، مدیریت مدل‌ها و پرسش فهرست
- لیست کردن، دانلود، بارگذاری و بارگذاری مجدد مدل‌ها به صورت برنامه‌نویسی شده
- بررسی فراداده مدل با استفاده از `FoundryModelInfo`
- درک تفاوت بین فهرست، کش و مدل‌های بارگذاری شده
- استفاده از constructor bootstrap (در Python) و الگوی `create()` + catalog (در JavaScript)
- درک بازطراحی SDK در C# و API شیءگرا

---

## پیش‌نیازها

| نیازمندی | جزئیات |
|-------------|---------|
| **Foundry Local CLI** | نصب شده و در `PATH` شما باشد ([بخش ۱](part1-getting-started.md)) |
| **محیط اجرای زبان** | **Python 3.9+** و/یا **Node.js 18+** و/یا **.NET 9.0+** |

---

## مفهوم: SDK در مقابل CLI - چرا از SDK استفاده کنیم؟

| جنبه | CLI (`foundry` command) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **موارد استفاده** | اکتشاف، تست دستی | یکپارچه‌سازی برنامه |
| **مدیریت سرویس** | دستی: `foundry service start` | خودکار: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **کشف پورت** | خواندن از خروجی CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **دانلود مدل** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **مدیریت خطا** | کدهای خروج، stderr | استثناها، خطاهای تایپ‌شده |
| **اتوماسیون** | اسکریپت‌های شل | ادغام بومی زبان برنامه‌نویسی |
| **استقرار** | نیاز به CLI روی دستگاه کاربر نهایی | SDK زبان C# خودکفا است (نیازی به CLI نیست) |

> **نکته کلیدی:** SDK کل چرخه را مدیریت می‌کند: راه‌اندازی سرویس، بررسی کش، دانلود مدل‌های مفقود، بارگذاری آن‌ها و کشف نقطه دسترسی، تنها در چند خط کد. برنامه شما نیازی به تحلیل خروجی CLI یا مدیریت زیرروندها ندارد.

---

## تمرین‌های آزمایشگاه

### تمرین ۱: نصب SDK

<details>
<summary><h3>🐍 پایتون</h3></summary>

```bash
pip install foundry-local-sdk
```

نصب را بررسی کنید:

```python
from foundry_local import FoundryLocalManager
print("SDK installed successfully")
```

</details>

<details>
<summary><h3>📘 جاوااسکریپت</h3></summary>

```bash
npm install foundry-local-sdk
```

نصب را بررسی کنید:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 سی‌شارپ</h3></summary>

دو بسته NuGet وجود دارد:

| بسته | پلتفرم | توضیحات |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | چندسکویی | روی ویندوز، لینوکس، مک او اس کار می‌کند |
| `Microsoft.AI.Foundry.Local.WinML` | فقط ویندوز | افزودن شتاب سخت‌افزاری WinML؛ بارگیری و نصب اجراکننده‌های افزونه (مثلاً QNN برای Qualcomm NPU) |

**تنظیم ویندوز:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

فایل `.csproj` را ویرایش کنید:

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

> **توجه:** در ویندوز، بسته WinML شامل SDK پایه به همراه ارائه‌دهنده اجرای QNN است. در لینوکس/مک، از SDK پایه استفاده می‌شود. ارجاعات شرطی TFM و پکیج پروژه را کاملاً چندسکویی نگه‌می‌دارد.

یک `nuget.config` در ریشه پروژه بسازید:

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

بسته‌ها را بازیابی کنید:

```bash
dotnet restore
```

</details>

---

### تمرین ۲: شروع سرویس و فهرست مدل‌ها

اولین کاری که هر برنامه انجام می‌دهد، شروع سرویس Foundry Local و کشف مدل‌های در دسترس است.

<details>
<summary><h3>🐍 پایتون</h3></summary>

```python
from foundry_local import FoundryLocalManager

# یک مدیر ایجاد کنید و سرویس را شروع کنید
manager = FoundryLocalManager()
manager.start_service()

# همه مدل‌های موجود در فهرست را لیست کنید
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### روش‌های مدیریت سرویس در Python SDK

| روش | امضا | توضیح |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | بررسی وضعیت اجرای سرویس |
| `start_service()` | `() -> None` | شروع سرویس Foundry Local |
| `service_uri` | `@property -> str` | URI پایه سرویس |
| `endpoint` | `@property -> str` | نقطه دسترسی API (URI سرویس + `/v1`) |
| `api_key` | `@property -> str` | کلید API (از محیط یا مقدار پیش‌فرض) |

#### روش‌های مدیریت فهرست در Python SDK

| روش | امضا | توضیح |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | لیست تمام مدل‌های فهرست |
| `refresh_catalog()` | `() -> None` | تازه‌سازی فهرست از سرویس |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | دریافت اطلاعات مدل خاص |

</details>

<details>
<summary><h3>📘 جاوااسکریپت</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// یک مدیر ایجاد کنید و سرویس را شروع کنید
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// مرور کاتالوگ
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### روش‌های مدیر در JavaScript SDK

| روش | امضا | توضیح |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | مقداردهی اولیه تک‌نمونه SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | دسترسی به مدیر تک‌نمونه |
| `manager.startWebService()` | `() => Promise<void>` | شروع سرویس وب Foundry Local |
| `manager.urls` | `string[]` | آرایه URL پایه‌های سرویس |

#### روش‌های فهرست و مدل در JavaScript SDK

| روش | امضا | توضیح |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | دسترسی به فهرست مدل‌ها |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | دریافت شیء مدل با نام مستعار |

</details>

<details>
<summary><h3>💜 سی‌شارپ</h3></summary>

نسخه 0.8.0+ SDK زبان C# از معماری شیءگرا با اشیاء `Configuration`، `Catalog` و `Model` استفاده می‌کند:

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

#### کلاس‌های کلیدی در C# SDK

| کلاس | هدف |
|-------|---------|
| `Configuration` | تنظیم نام برنامه، سطح لاگ، دایرکتوری کش، URLهای وب‌سرور |
| `FoundryLocalManager` | نقطه ورود اصلی - ایجاد شده با `CreateAsync()` و دسترسی از طریق `.Instance` |
| `Catalog` | مرور، جستجو و دریافت مدل‌ها از فهرست |
| `Model` | نماینده یک مدل خاص - دانلود، بارگذاری، دریافت کلاینت‌ها |

#### روش‌های مدیر و فهرست در C# SDK

| روش | توضیح |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | مقداردهی اولیه مدیر |
| `FoundryLocalManager.Instance` | دسترسی به مدیر تک‌نمونه |
| `manager.GetCatalogAsync()` | دریافت فهرست مدل‌ها |
| `catalog.ListModelsAsync()` | لیست تمام مدل‌های در دسترس |
| `catalog.GetModelAsync(alias: "alias")` | دریافت مدل خاص با نام مستعار |
| `catalog.GetCachedModelsAsync()` | لیست مدل‌های دانلود شده |
| `catalog.GetLoadedModelsAsync()` | لیست مدل‌های بارگذاری‌شده فعلی |

> **یادداشت معماری C#:** بازطراحی نسخه 0.8.0+ SDK زبان C# برنامه را **خودکفا** می‌کند؛ نیاز به CLI Foundry Local روی دستگاه کاربر ندارد. SDK مدیریت مدل و استنتاج را به صورت بومی انجام می‌دهد.

</details>

---

### تمرین ۳: دانلود و بارگذاری یک مدل

SDK دانلود (روی دیسک) را از بارگذاری (در حافظه) جدا می‌کند. این امکان را می‌دهد که مدل‌ها را در هنگام راه‌اندازی پیش‌دانلود و در صورت نیاز بارگذاری کنید.

<details>
<summary><h3>🐍 پایتون</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# گزینه الف: دستی مرحله به مرحله
manager = FoundryLocalManager()
manager.start_service()

# ابتدا کش را بررسی کنید
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

# گزینه ب: بوت‌استرپ تک‌خطی (توصیه شده)
# نام مستعار را به سازنده پاس دهید - این سرویس را شروع می‌کند، دانلود می‌کند و به طور خودکار بارگذاری می‌کند
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### روش‌های مدیریت مدل در Python

| روش | امضا | توضیح |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | دانلود مدل به کش محلی |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | بارگذاری مدل در سرور استنتاج |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | خارج کردن مدل از سرور |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | لیست مدل‌های فعلاً بارگذاری شده |

#### روش‌های مدیریت کش در Python

| روش | امضا | توضیح |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | گرفتن مسیر دایرکتوری کش |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | لیست تمام مدل‌های دانلود شده |

</details>

<details>
<summary><h3>📘 جاوااسکریپت</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// روش گام به گام
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

#### روش‌های مدل در JavaScript

| روش | امضا | توضیح |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | آیا مدل قبلاً دانلود شده است |
| `model.download()` | `() => Promise<void>` | دانلود مدل به کش محلی |
| `model.load()` | `() => Promise<void>` | بارگذاری در سرور استنتاج |
| `model.unload()` | `() => Promise<void>` | خارج کردن از سرور استنتاج |
| `model.id` | `string` | شناسه یکتا مدل |

</details>

<details>
<summary><h3>💜 سی‌شارپ</h3></summary>

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

#### روش‌های مدل در C#

| روش | توضیح |
|--------|-------------|
| `model.DownloadAsync(progress?)` | دانلود نمونه انتخاب شده |
| `model.LoadAsync()` | بارگذاری مدل در حافظه |
| `model.UnloadAsync()` | خارج کردن مدل |
| `model.SelectVariant(variant)` | انتخاب نسخه خاص (CPU/GPU/NPU) |
| `model.SelectedVariant` | نسخه انتخاب شده فعلی |
| `model.Variants` | تمام نسخه‌های موجود برای این مدل |
| `model.GetPathAsync()` | گرفتن مسیر فایل محلی |
| `model.GetChatClientAsync()` | دریافت کلاینت چت بومی (بدون نیاز به OpenAI SDK) |
| `model.GetAudioClientAsync()` | دریافت کلاینت صوتی برای تبدیل گفتار به متن |

</details>

---

### تمرین ۴: بررسی فراداده مدل

شیء `FoundryModelInfo` دارای فراداده غنی برای هر مدل است. درک این فیلدها به شما کمک می‌کند مدل مناسب برنامه خود را انتخاب کنید.

<details>
<summary><h3>🐍 پایتون</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# دریافت اطلاعات دقیق درباره یک مدل خاص
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
<summary><h3>📘 جاوااسکریپت</h3></summary>

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

#### فیلدهای FoundryModelInfo

| فیلد | نوع | توضیح |
|-------|------|-------------|
| `alias` | رشته | نام کوتاه (مثلاً `phi-3.5-mini`) |
| `id` | رشته | شناسه یکتا مدل |
| `version` | رشته | نسخه مدل |
| `task` | رشته | `chat-completions` یا `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU، GPU یا NPU |
| `execution_provider` | رشته | پردازشگر زمان اجرا (CUDA، CPU، QNN، WebGPU و غیره) |
| `file_size_mb` | عدد صحیح | اندازه روی دیسک بر حسب مگابایت |
| `supports_tool_calling` | بول | آیا مدل از فراخوانی توابع/ابزار پشتیبانی می‌کند |
| `publisher` | رشته | ناشر مدل |
| `license` | رشته | نام مجوز |
| `uri` | رشته | URI مدل |
| `prompt_template` | دیکشنری/خالی | قالب پرسش، اگر وجود داشته باشد |

---

### تمرین ۵: مدیریت چرخه عمر مدل

عملکرد کامل چرخه را تمرین کنید: لیست → دانلود → بارگذاری → استفاده → خارج کردن.

<details>
<summary><h3>🐍 پایتون</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # مدل کوچک برای آزمایش سریع

manager = FoundryLocalManager()
manager.start_service()

# ۱. بررسی محتویات کاتالوگ
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# ۲. بررسی مواردی که قبلاً دانلود شده‌اند
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# ۳. دانلود یک مدل
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# ۴. بررسی اینکه اکنون در کش قرار دارد
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# ۵. بارگذاری آن
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# ۶. بررسی موارد بارگذاری شده
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# ۷. خارج کردن آن از بارگذاری
print(f"\nUnloading {alias}...")
manager.unload_model(alias)
loaded = manager.list_loaded_models()
print(f"Loaded models after unload: {len(loaded)}")
```

</details>

<details>
<summary><h3>📘 جاوااسکریپت</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "qwen2.5-0.5b"; // مدل کوچک برای آزمایش سریع

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ۱. دریافت مدل از کاتالوگ
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// ۲. در صورت نیاز دانلود کنید
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// ۳. بارگذاری آن
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// ۴. خارج کردن آن
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### تمرین ۶: الگوهای شروع سریع

هر زبان یک میانبر برای راه‌اندازی سرویس و بارگذاری مدل در یک فراخوانی ارائه می‌دهد. این‌ها **الگوهای پیشنهادی** برای بیشتر برنامه‌ها هستند.

<details>
<summary><h3>🐍 پایتون - راه‌اندازی سازنده</h3></summary>

```python
from foundry_local import FoundryLocalManager

# یک نام مستعار به سازنده بدهید - همه چیز را مدیریت می‌کند:
# ۱. اگر سرویس در حال اجرا نباشد، آن را شروع می‌کند
# ۲. مدل را اگر در کش نباشد، دانلود می‌کند
# ۳. مدل را در سرور استنباط بارگذاری می‌کند
manager = FoundryLocalManager("phi-3.5-mini")

# آماده استفاده بلافاصله
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

پارامتر `bootstrap` (به‌طور پیش‌فرض `True`) این رفتار را کنترل می‌کند. اگر می‌خواهید کنترل دستی داشته باشید، مقدار `bootstrap=False` را تنظیم کنید:

```python
# حالت دستی - هیچ کاری به صورت خودکار انجام نمی‌شود
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 جاوااسکریپت - `create()` + فهرست</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() همه چیز را مدیریت می‌کند:
// ۱. سرویس را شروع می‌کند
// ۲. مدل را از کاتالوگ دریافت می‌کند
// ۳. در صورت نیاز دانلود و مدل را بارگذاری می‌کند
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// آماده استفاده بلافاصله
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 سی‌شارپ - `CreateAsync()` + فهرست</h3></summary>

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

> **نکته در مورد سی‌شارپ:** کیت توسعه سی‌شارپ از `Configuration` برای کنترل نام برنامه، ثبت وقایع، دایرکتوری‌های کش و حتی پین کردن پورت وب‌سرور خاص استفاده می‌کند. این باعث می‌شود که قابل تنظیم‌ترین کیت توسعه در این سه باشد.

</details>

---

### تمرین ۷: ChatClient بومی (نیازی به SDK OpenAI نیست)

کیت‌های توسعه جاوااسکریپت و سی‌شارپ روش راحت `createChatClient()` را ارائه می‌دهند که یک کلاینت چت بومی برمی‌گرداند — نیازی به نصب یا پیکربندی جداگانه SDK OpenAI نیست.

<details>
<summary><h3>📘 جاوااسکریپت - <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// ساخت یک ChatClient مستقیماً از مدل — نیازی به وارد کردن OpenAI نیست
const chatClient = model.createChatClient();

// completeChat یک شیء پاسخ سازگار با OpenAI را برمی‌گرداند
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// استریمینگ از الگوی callback استفاده می‌کند
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

کلاس `ChatClient` همچنین از فراخوانی ابزارها پشتیبانی می‌کند — ابزارها را به عنوان آرگومان دوم ارسال کنید:

```javascript
const response = await chatClient.completeChat(messages, tools);
```

</details>

<details>
<summary><h3>💜 سی‌شارپ - <code>model.GetChatClientAsync()</code></h3></summary>

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

> **زمان استفاده از هر الگو:**
> - **`createChatClient()`** — نمونه‌سازی سریع، وابستگی‌های کمتر، کد ساده‌تر
> - **کیت SDK OpenAI** — کنترل کامل روی پارامترها (دما، top_p، توقف توکن، و غیره)، بهتر برای برنامه‌های تولیدی

---

### تمرین ۸: انواع مدل و انتخاب سخت‌افزار

مدل‌ها می‌توانند چند **نوع** بهینه‌شده برای سخت‌افزارهای متفاوت داشته باشند. کیت توسعه بهترین نوع را به‌طور خودکار انتخاب می‌کند، اما شما همچنین می‌توانید آن را بررسی و به‌صورت دستی انتخاب کنید.

<details>
<summary><h3>📘 جاوااسکریپت</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// تمامی واریانت‌های موجود را لیست کن
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK به طور خودکار بهترین واریانت را برای سخت‌افزار شما انتخاب می‌کند
// برای اعمال تغییر، از selectVariant() استفاده کنید:
// model.selectVariant(model.variants[0]);
```

</details>

<details>
<summary><h3>💜 سی‌شارپ</h3></summary>

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
<summary><h3>🐍 پایتون</h3></summary>

در پایتون، کیت توسعه بهترین نوع را بر اساس سخت‌افزار به‌طور خودکار انتخاب می‌کند. از `get_model_info()` استفاده کنید تا ببینید چه چیزی انتخاب شده است:

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

#### مدل‌هایی با انواع NPU

برخی مدل‌ها نوع بهینه‌شده برای NPU دارند برای دستگاه‌هایی با واحدهای پردازش عصبی (Qualcomm Snapdragon، Intel Core Ultra):

| مدل | نوع NPU موجود است |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **نکته:** در سخت‌افزارهای مجهز به NPU، کیت توسعه به طور خودکار نوع NPU را در صورت موجود بودن انتخاب می‌کند. نیازی نیست کد خود را تغییر دهید. برای پروژه‌های سی‌شارپ در ویندوز، بسته NuGet با نام `Microsoft.AI.Foundry.Local.WinML` را اضافه کنید تا فراهم‌کننده اجرای QNN فعال شود — QNN به صورت افزونه‌ای از طریق WinML ارائه می‌شود.

---

### تمرین ۹: بروزرسانی مدل‌ها و تازه‌سازی فهرست

کاتالوگ مدل‌ها به صورت دوره‌ای به‌روزرسانی می‌شود. از این روش‌ها برای بررسی و اعمال بروزرسانی‌ها استفاده کنید.

<details>
<summary><h3>🐍 پایتون</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# برای دریافت جدیدترین فهرست مدل‌ها کاتالوگ را تازه کنید
manager.refresh_catalog()

# بررسی کنید که آیا مدل ذخیره شده نسخه جدیدتری دارد یا خیر
if manager.is_model_upgradeable(alias):
    print(f"{alias} has a newer version available!")
    manager.upgrade_model(alias)
    print("Upgrade complete")
else:
    print(f"{alias} is up to date")
```

</details>

<details>
<summary><h3>📘 جاوااسکریپت</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "UpgradeDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// تازه‌سازی کاتالوگ برای دریافت جدیدترین لیست مدل‌ها
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// نمایش تمام مدل‌های موجود پس از تازه‌سازی
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### تمرین ۱۰: کار با مدل‌های استنتاجی

مدل **phi-4-mini-reasoning** شامل استدلال زنجیره تفکر است. قبل از تولید پاسخ نهایی، فکر کردن داخلی خود را در تگ‌های `<think>...</think>` می‌پیچد. این برای وظایفی که نیاز به منطق چند مرحله‌ای، ریاضیات یا حل مسئله دارند مفید است.

<details>
<summary><h3>🐍 پایتون</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning تقریباً ۴.۶ گیگابایت است
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# مدل فکر خود را در تگ‌های <think>...</think> قالب‌بندی می‌کند
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
<summary><h3>📘 جاوااسکریپت</h3></summary>

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

// تجزیه فکر زنجیره‌ای
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **زمان استفاده از مدل‌های استنتاجی:**
> - حل مسائل ریاضی و منطقی
> - برنامه‌ریزی چند مرحله‌ای
> - تولید کد پیچیده
> - وظایفی که نمایش کار باعث افزایش دقت می‌شود
>
> **محدودیت:** مدل‌های استنتاجی توکن‌های بیشتری تولید می‌کنند (بخش `<think>`) و کندتر هستند. برای سوال و جواب ساده، مدل استانداردی مثل phi-3.5-mini سریع‌تر است.

---

### تمرین ۱۱: درک نام مستعار و انتخاب سخت‌افزار

وقتی یک **نام مستعار** (مثل `phi-3.5-mini`) به جای شناسه کامل مدل ارسال می‌کنید، کیت توسعه بهترین نوع را برای سخت‌افزار شما به‌صورت خودکار انتخاب می‌کند:

| سخت‌افزار | فراهم‌کننده اجرای انتخاب شده |
|----------|---------------------------|
| کارت گرافیک NVIDIA (CUDA) | `CUDAExecutionProvider` |
| NPU کوالکام | `QNNExecutionProvider` (از طریق افزونه WinML) |
| NPU اینتل | `OpenVINOExecutionProvider` |
| کارت گرافیک AMD | `VitisAIExecutionProvider` |
| کارت گرافیک NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| هر دستگاه دیگری (پیش‌فرض) | `CPUExecutionProvider` یا `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# نام مستعار به بهترین گونه برای سخت‌افزار شما اشاره می‌کند
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **نکته:** همیشه در کد برنامه خود از نام‌های مستعار استفاده کنید. هنگامی که روی ماشین کاربر مستقر می‌کنید، کیت توسعه به‌طور زمان اجرا بهترین نوع را انتخاب می‌کند — CUDA روی NVIDIA، QNN روی کوالکام، و CPU در سایر موارد.

---

### تمرین ۱۲: گزینه‌های پیکربندی سی‌شارپ

کلاس `Configuration` در کیت سی‌شارپ کنترل جزئی بر زمان اجرا را فراهم می‌کند:

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

| ویژگی | پیش‌فرض | توضیح |
|----------|---------|-------------|
| `AppName` | (الزامی) | نام برنامه شما |
| `LogLevel` | `Information` | شدت ثبت لاگ |
| `Web.Urls` | (پویا) | پین کردن پورت خاص برای وب سرور |
| `AppDataDir` | پیش‌فرض سیستم‌عامل | دایرکتوری پایه داده‌های برنامه |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | محل ذخیره مدل‌ها |
| `LogsDir` | `{AppDataDir}/logs` | محل نوشتن لاگ‌ها |

---

### تمرین ۱۳: استفاده در مرورگر (فقط جاوااسکریپت)

کیت جاوااسکریپت نسخه سازگار با مرورگر دارد. در مرورگر باید به‌صورت دستی سرویس را از طریق CLI راه‌اندازی کنید و URL میزبان را مشخص نمایید:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// ابتدا سرویس را به‌صورت دستی شروع کنید:
//   دستور start foundry service
// سپس از آدرس URL که در خروجی خط فرمان آمده استفاده کنید
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// کاتالوگ را مرور کنید
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **محدودیت‌های مرورگر:** نسخه مرورگر از `startWebService()` پشتیبانی نمی‌کند. شما باید اطمینان حاصل کنید که سرویس Foundry Local پیش از استفاده در مرورگر در حال اجرا است.

---

## مرجع کامل API

### پایتون

| دسته‌بندی | روش | توضیح |
|----------|--------|-------------|
| **ابتدایی** | `FoundryLocalManager(alias?, bootstrap=True)` | ایجاد مدیر؛ به‌طور اختیاری با مدل بوت‌استرپ کنید |
| **سرویس** | `is_service_running()` | بررسی اجرای سرویس |
| **سرویس** | `start_service()` | راه‌اندازی سرویس |
| **سرویس** | `endpoint` | URL نقطه پایانی API |
| **سرویس** | `api_key` | کلید API |
| **کاتالوگ** | `list_catalog_models()` | فهرست تمام مدل‌های موجود |
| **کاتالوگ** | `refresh_catalog()` | تازه‌سازی کاتالوگ |
| **کاتالوگ** | `get_model_info(alias_or_model_id)` | دریافت متادیتای مدل |
| **کش** | `get_cache_location()` | مسیر دایرکتوری کش |
| **کش** | `list_cached_models()` | فهرست مدل‌های دانلود شده |
| **مدل** | `download_model(alias_or_model_id)` | دانلود مدل |
| **مدل** | `load_model(alias_or_model_id, ttl=600)` | بارگذاری مدل |
| **مدل** | `unload_model(alias_or_model_id)` | خارج کردن مدل از بارگذاری |
| **مدل** | `list_loaded_models()` | فهرست مدل‌های بارگذاری شده |
| **مدل** | `is_model_upgradeable(alias_or_model_id)` | بررسی در دسترس بودن نسخه جدیدتری |
| **مدل** | `upgrade_model(alias_or_model_id)` | ارتقاء مدل به آخرین نسخه |
| **سرویس** | `httpx_client` | کلاینت HTTPX پیش‌پیکربندی شده برای فراخوانی مستقیم API |

### جاوااسکریپت

| دسته‌بندی | روش | توضیح |
|----------|--------|-------------|
| **ابتدایی** | `FoundryLocalManager.create(options)` | مقداردهی اولیه کیت توسعه |
| **ابتدایی** | `FoundryLocalManager.instance` | دسترسی به مدیر سینگلتون |
| **سرویس** | `manager.startWebService()` | راه‌اندازی سرویس وب |
| **سرویس** | `manager.urls` | آرایه URLهای پایه سرویس |
| **کاتالوگ** | `manager.catalog` | دسترسی به کاتالوگ مدل |
| **کاتالوگ** | `catalog.getModel(alias)` | دریافت یک مدل بر اساس نام مستعار (برگرداندن Promise) |
| **مدل** | `model.isCached` | آیا مدل دانلود شده است |
| **مدل** | `model.download()` | دانلود مدل |
| **مدل** | `model.load()` | بارگذاری مدل |
| **مدل** | `model.unload()` | خارج کردن مدل از بارگذاری |
| **مدل** | `model.id` | شناسه منحصر به فرد مدل |
| **مدل** | `model.alias` | نام مستعار مدل |
| **مدل** | `model.createChatClient()` | دریافت کلاینت چت بومی (نیاز به SDK OpenAI ندارد) |
| **مدل** | `model.createAudioClient()` | دریافت کلاینت صوتی برای رونویسی |
| **مدل** | `model.removeFromCache()` | حذف مدل از کش محلی |
| **مدل** | `model.selectVariant(variant)` | انتخاب نوع خاص سخت‌افزار |
| **مدل** | `model.variants` | آرایه انواع موجود برای این مدل |
| **مدل** | `model.isLoaded()` | بررسی وضعیت بارگذاری فعلی مدل |
| **کاتالوگ** | `catalog.getModels()` | فهرست تمام مدل‌های موجود |
| **کاتالوگ** | `catalog.getCachedModels()` | فهرست مدل‌های دانلود شده |
| **کاتالوگ** | `catalog.getLoadedModels()` | فهرست مدل‌های بارگذاری شده فعلی |
| **کاتالوگ** | `catalog.updateModels()` | تازه‌سازی کاتالوگ از سرویس |
| **سرویس** | `manager.stopWebService()` | متوقف کردن سرویس وب Foundry Local |

### سی‌شارپ (نسخه 0.8.0 و بالاتر)

| دسته‌بندی | روش | توضیح |
|----------|--------|-------------|
| **ابتدایی** | `FoundryLocalManager.CreateAsync(config, logger)` | مقداردهی اولیه مدیر |
| **ابتدایی** | `FoundryLocalManager.Instance` | دسترسی به سینگلتون |
| **کاتالوگ** | `manager.GetCatalogAsync()` | دریافت کاتالوگ |
| **کاتالوگ** | `catalog.ListModelsAsync()` | فهرست تمام مدل‌ها |
| **کاتالوگ** | `catalog.GetModelAsync(alias)` | دریافت مدل مشخص |
| **کاتالوگ** | `catalog.GetCachedModelsAsync()` | فهرست مدل‌های کش شده |
| **کاتالوگ** | `catalog.GetLoadedModelsAsync()` | فهرست مدل‌های بارگذاری شده |
| **مدل** | `model.DownloadAsync(progress?)` | دانلود مدل |
| **مدل** | `model.LoadAsync()` | بارگذاری مدل |
| **مدل** | `model.UnloadAsync()` | خارج کردن مدل از بارگذاری |
| **مدل** | `model.SelectVariant(variant)` | انتخاب نوع سخت‌افزار |
| **مدل** | `model.GetChatClientAsync()` | دریافت کلاینت چت بومی |
| **مدل** | `model.GetAudioClientAsync()` | دریافت کلاینت رونویسی صوتی |
| **مدل** | `model.GetPathAsync()` | دریافت مسیر فایل محلی |
| **کاتالوگ** | `catalog.GetModelVariantAsync(alias, variant)` | دریافت نوع سخت‌افزار مشخص |
| **کاتالوگ** | `catalog.UpdateModelsAsync()` | تازه‌سازی کاتالوگ |
| **سرور** | `manager.StartWebServerAsync()` | راه‌اندازی وب سرور REST |
| **سرور** | `manager.StopWebServerAsync()` | متوقف کردن وب سرور REST |
| **پیکربندی** | `config.ModelCacheDir` | دایرکتوری کش |

---

## نکات کلیدی

| مفهوم | آنچه آموختید |
|---------|-----------------|
| **SDK در مقابل CLI** | کیت توسعه کنترل برنامه‌نویسی فراهم می‌کند — ضروری برای برنامه‌ها |
| **صفحه کنترل** | کیت توسعه سرویس‌ها، مدل‌ها و کش را مدیریت می‌کند |
| **پورت‌های پویا** | همیشه از `manager.endpoint` (پایتون) یا `manager.urls[0]` (جاوااسکریپت/سی‌شارپ) استفاده کنید — هرگز پورت را به‌طور سخت‌کد مشخص نکنید |
| **نام‌های مستعار** | از نام‌های مستعار برای انتخاب خودکار بهترین مدل برای سخت‌افزار استفاده کنید |
| **شروع سریع** | پایتون: `FoundryLocalManager(alias)`، JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **طراحی مجدد C#** | نسخه v0.8.0+ به صورت خودکفا است - نیازی به CLI روی ماشین‌های کاربر نهایی نیست |
| **چرخه عمر مدل** | کاتالوگ → دانلود → بارگذاری → استفاده → بارگذاری مجدد |
| **FoundryModelInfo** | متادیتای غنی: کار، دستگاه، اندازه، مجوز، پشتیبانی از فراخوانی ابزار |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) برای استفاده رایگان بدون OpenAI |
| **انواع** | مدل‌ها نسخه‌های خاص سخت‌افزاری دارند (CPU، GPU، NPU)؛ به صورت خودکار انتخاب می‌شوند |
| **ارتقاء‌ها** | پایتون: `is_model_upgradeable()` + `upgrade_model()` برای به‌روزرسانی مدل‌ها |
| **تازه‌سازی کاتالوگ** | `refresh_catalog()` (پایتون) / `updateModels()` (JS) برای کشف مدل‌های جدید |

---

## منابع

| منبع | لینک |
|----------|------|
| مرجع SDK (تمام زبان‌ها) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| یکپارچه‌سازی با SDKهای استنتاج | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| مرجع API SDK سی‌شارپ | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| نمونه‌های SDK سی‌شارپ | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| وب‌سایت Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## مراحل بعدی

ادامه دهید به [بخش ۳: استفاده از SDK با OpenAI](part3-sdk-and-apis.md) برای اتصال SDK به کتابخانه کلاینت OpenAI و ساخت اولین برنامه تکمیل چت خود.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**سلب مسئولیت**:  
این سند با استفاده از سرویس ترجمه هوش مصنوعی [Co-op Translator](https://github.com/Azure/co-op-translator) ترجمه شده است. در حالی که ما در تلاش برای دقت هستیم، لطفاً آگاه باشید که ترجمه‌های خودکار ممکن است شامل خطاها یا نادرستی‌هایی باشند. سند اصلی به زبان اصلی خود باید به عنوان منبع معتبر در نظر گرفته شود. برای اطلاعات حساس، توصیه می‌شود از ترجمه حرفه‌ای انسانی استفاده کنید. ما مسئول هیچ گونه سوءتفاهم یا تفسیر نادرست ناشی از استفاده از این ترجمه نیستیم.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->