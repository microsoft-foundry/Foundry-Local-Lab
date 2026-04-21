![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# অংশ ২: Foundry Local SDK গভীর বিশ্লেষণ

> **লক্ষ্য:** Foundry Local SDK দক্ষতা অর্জন করুন যাতে মডেল, সার্ভিস এবং ক্যাশ প্রোগ্রাম্যাটিক্যালি নিয়ন্ত্রণ করতে পারেন - এবং বুঝতে পারেন কেন SDK CLI এর তুলনায় অ্যাপ্লিকেশন তৈরি করার জন্য সুপারিশকৃত পদ্ধতি।

## ওভারভিউ

অংশ ১ এ আপনি **Foundry Local CLI** ব্যবহার করে ইন্টারেক্টিভভাবে মডেল ডাউনলোড এবং চালিয়েছেন। CLI অনুসন্ধানের জন্য চমৎকার, কিন্তু যখন আপনি বাস্তব অ্যাপ্লিকেশন তৈরি করবেন তখন আপনাকে **প্রোগ্রাম্যাটিক নিয়ন্ত্রণ** প্রয়োজন। Foundry Local SDK আপনাকে তা দেয় - এটি **কন্ট্রোল প্লেন** পরিচালনা করে (সার্ভিস শুরু করা, মডেল আবিষ্কার, ডাউনলোড, লোড) যাতে আপনার অ্যাপ্লিকেশন কোড **ডেটা প্লেন** (প্রম্পট পাঠানো, আউটপুট গ্রহণ) উপর ফোকাস করতে পারে।

এই ল্যাবটি পাইটন, জাভাস্ক্রিপ্ট, এবং C#-এর SDK API সম্পূর্ণ শেখায়। শেষে আপনি প্রতিটি পদ্ধতি বুঝতে পারবেন এবং কখন কোনটি ব্যবহার করবেন তা জানবেন।

## শেখার উদ্দেশ্য

এই ল্যাব শেষ করার পর আপনি পারবেন:

- ব্যাখ্যা করতে কেন SDK CLI এর চেয়ে অ্যাপ্লিকেশন ডেভেলপমেন্টে প্রাধান্য পায়
- Foundry Local SDK ইনস্টল করতে পাইটন, জাভাস্ক্রিপ্ট বা C# এর জন্য
- `FoundryLocalManager` ব্যবহার করে সার্ভিস শুরু করা, মডেল পরিচালনা এবং ক্যাটালগ অনুসন্ধান
- প্রোগ্রাম্যাটিকভাবে মডেল তালিকা দেখানো, ডাউনলোড, লোড এবং আনলোড করা
- `FoundryModelInfo` দিয়ে মডেল মেটাডেটা পরিদর্শন করা
- ক্যাটালগ, ক্যাশ এবং লোডেড মডেল এর পার্থক্য বোঝা
- কনস্ট্রাক্টর বুটস্ট্র্যাপ (পাইটন) এবং `create()` + ক্যাটালগ প্যাটার্ন (জাভাস্ক্রিপ্ট) ব্যবহার
- C# SDK পুনঃনকশা এবং এর অবজেক্ট-ওরিয়েন্টেড API বোঝা

---

## প্রয়োজনীয়তা

| প্রয়োজনীয়তা | বিবরণ |
|-------------|---------|
| **Foundry Local CLI** | ইনস্টল করা এবং আপনার `PATH` এ আছে ([অংশ ১](part1-getting-started.md)) |
| **ল্যাঙ্গুয়েজ রানটাইম** | **Python 3.9+**, এবং/অথবা **Node.js 18+**, এবং/অথবা **.NET 9.0+** |

---

## ধারণা: SDK বনাম CLI - কেন SDK ব্যবহার করবেন?

| দিক | CLI (`foundry` কমান্ড) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **ব্যবহার ক্ষেত্র** | অনুসন্ধান, ম্যানুয়াল টেস্টিং | অ্যাপ্লিকেশন ইন্টিগ্রেশন |
| **সার্ভিস ব্যবস্থাপনা** | ম্যানুয়াল: `foundry service start` | স্বয়ংক্রিয়: `manager.start_service()` (পাইটন) / `manager.startWebService()` (JS/C#) |
| **পোর্ট আবিষ্কার** | CLI আউটপুট থেকে পড়া | `manager.endpoint` (পাইটন) / `manager.urls[0]` (JS/C#) |
| **মডেল ডাউনলোড** | `foundry model download alias` | `manager.download_model(alias)` (পাইটন) / `model.download()` (JS/C#) |
| **ত্রুটি হ্যান্ডলিং** | এক্সিট কোড, stderr | এক্সসেপশন, টাইপড এরর |
| **স্বয়ংক্রিয়তা** | শেল স্ক্রিপ্ট | স্থানীয় ভাষার ইন্টিগ্রেশন |
| **ডিপলয়মেন্ট** | শেষ ব্যবহারকারীর মেশিনে CLI প্রয়োজন | C# SDK স্ব-সম্পূর্ণ (CLI প্রয়োজন নেই) |

> **মূল অন্তর্দৃষ্টি:** SDK পুরো লাইফসাইকেল পরিচালনা করে: সার্ভিস শুরু, ক্যাশ পরীক্ষা, অনুপস্থিত মডেল ডাউনলোড, লোড, এবং এন্ডপয়েন্ট আবিষ্কার, কয়েক লাইনের কোডে। আপনার অ্যাপ্লিকেশনকে CLI আউটপুট পার্স বা সাবপ্রসেস বন্ধ করতে হবে না।

---

## ল্যাব অনুশীলন

### অনুশীলন ১: SDK ইনস্টল করুন

<details>
<summary><h3>🐍 পাইটন</h3></summary>

```bash
pip install foundry-local-sdk
```

ইনস্টলেশন যাচাই করুন:

```python
from foundry_local import FoundryLocalManager
print("SDK installed successfully")
```

</details>

<details>
<summary><h3>📘 জাভাস্ক্রিপ্ট</h3></summary>

```bash
npm install foundry-local-sdk
```

ইনস্টলেশন যাচাই করুন:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

দুটি NuGet প্যাকেজ আছে:

| প্যাকেজ | প্ল্যাটফর্ম | বিবরণ |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | ক্রস-প্ল্যাটফর্ম | উইন্ডোজ, লিনাক্স, ম্যাকওএস এ কাজ করে |
| `Microsoft.AI.Foundry.Local.WinML` | শুধুমাত্র উইন্ডোজ | WinML হার্ডওয়্যার ত্বরান্বিতকরণ যোগ করে; প্লাগইন এক্সিকিউশন প্রোভাইডার (যেমন Qualcomm NPU এর জন্য QNN) ডাউনলোড ও ইনস্টল করে |

**উইন্ডোজ সেটআপ:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` ফাইল সম্পাদনা করুন:

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

> **দ্রষ্টব্য:** উইন্ডোজে WinML প্যাকেজ হলো একটি সুপাসেট যা বেস SDK এবং QNN এক্সিকিউশন প্রোভাইডার অন্তর্ভুক্ত করে। লিনাক্স/ম্যাকএ বেস SDK ব্যবহৃত হয়। শর্তযুক্ত TFM এবং প্যাকেজ রেফারেন্স প্রজেক্টকে সম্পূর্ণ ক্রস-প্ল্যাটফর্ম রাখে।

প্রজেক্ট রুটে `nuget.config` তৈরি করুন:

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

প্যাকেজ রিস্টোর করুন:

```bash
dotnet restore
```

</details>

---

### অনুশীলন ২: সার্ভিস শুরু করুন এবং ক্যাটালগ তালিকা দেখুন

প্রতিটি অ্যাপ্লিকেশন প্রথমে Foundry Local সার্ভিস শুরু করে এবং উপলব্ধ মডেলগুলি আবিষ্কার করে।

<details>
<summary><h3>🐍 পাইটন</h3></summary>

```python
from foundry_local import FoundryLocalManager

# ম্যানেজার তৈরি করুন এবং সেবা শুরু করুন
manager = FoundryLocalManager()
manager.start_service()

# ক্যাটালগে উপলব্ধ সব মডেলের তালিকা করুন
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### পাইটন SDK - সার্ভিস ব্যবস্থাপনা পদ্ধতি

| পদ্ধতি | স্বাক্ষর | বিবরণ |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | সার্ভিস চলছে কিনা যাচাই করুন |
| `start_service()` | `() -> None` | Foundry Local সার্ভিস শুরু করুন |
| `service_uri` | `@property -> str` | বেস সার্ভিস URI |
| `endpoint` | `@property -> str` | API এন্ডপয়েন্ট (সার্ভিস URI + `/v1`) |
| `api_key` | `@property -> str` | API কী (env বা ডিফল্ট প্লেসহোল্ডার থেকে) |

#### পাইটন SDK - ক্যাটালগ ব্যবস্থাপনা পদ্ধতি

| পদ্ধতি | স্বাক্ষর | বিবরণ |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | ক্যাটালগের সকল মডেল তালিকা করুন |
| `refresh_catalog()` | `() -> None` | সার্ভিস থেকে ক্যাটালগ রিফ্রেশ করুন |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | নির্দিষ্ট মডেল এর তথ্য পান |

</details>

<details>
<summary><h3>📘 জাভাস্ক্রিপ্ট</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// একটি ম্যানেজার তৈরি করুন এবং সার্ভিস শুরু করুন
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ক্যাটালগ ব্রাউজ করুন
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### জাভাস্ক্রিপ্ট SDK - ম্যানেজার পদ্ধতি

| পদ্ধতি | স্বাক্ষর | বিবরণ |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK সিঙ্গেলটন ইনিশিয়ালাইজ করুন |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | সিঙ্গেলটন ম্যানেজার অ্যাক্সেস |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local ওয়েব সার্ভিস শুরু করুন |
| `manager.urls` | `string[]` | সার্ভিসের বেস URL গুলোর অ্যারে |

#### জাভাস্ক্রিপ্ট SDK - ক্যাটালগ এবং মডেল পদ্ধতি

| পদ্ধতি | স্বাক্ষর | বিবরণ |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | মডেল ক্যাটালগ অ্যাক্সেস |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | একটি মডেল অবজেক্ট আলিয়াস দিয়ে পান |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ অবজেক্ট-ওরিয়েন্টেড আর্কিটেকচার ব্যবহার করে `Configuration`, `Catalog`, এবং `Model` অবজেক্ট:

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

#### C# SDK - কী ক্লাসসমূহ

| ক্লাস | উদ্দেশ্য |
|-------|---------|
| `Configuration` | অ্যাপ নাম, লগ লেভেল, ক্যাশ ডিরেক্টরি, ওয়েব সার্ভার URL সেট করুন |
| `FoundryLocalManager` | মূল এন্ট্রি পয়েন্ট - `CreateAsync()` দ্বারা তৈরি, `.Instance` দিয়ে অ্যাক্সেস |
| `Catalog` | ক্যাটালগ ব্রাউজিং, সার্চ, এবং মডেল সংগ্রহ |
| `Model` | নির্দিষ্ট মডেল প্রতিনিধিত্ব করে - ডাউনলোড, লোড, ক্লায়েন্ট পান |

#### C# SDK - ম্যানেজার এবং ক্যাটালগ পদ্ধতি

| পদ্ধতি | বিবরণ |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | ম্যানেজার ইনিশিয়ালাইজ করুন |
| `FoundryLocalManager.Instance` | সিঙ্গেলটন ম্যানেজার অ্যাক্সেস |
| `manager.GetCatalogAsync()` | মডেল ক্যাটালগ পান |
| `catalog.ListModelsAsync()` | সমস্ত উপলব্ধ মডেল তালিকা করুন |
| `catalog.GetModelAsync(alias: "alias")` | নির্দিষ্ট মডেল আলিয়াস দিয়ে পান |
| `catalog.GetCachedModelsAsync()` | ডাউনলোডকৃত মডেল তালিকা করুন |
| `catalog.GetLoadedModelsAsync()` | বর্তমানে লোড করা মডেল তালিকা করুন |

> **C# আর্কিটেকচার টীকা:** C# SDK v0.8.0+ পুনঃনকশা অ্যাপ্লিকেশনকে **স্বনির্ভর** করে তোলে; এটি শেষ ব্যবহারকারীর মেশিনে Foundry Local CLI প্রয়োজন করে না। SDK নিজে মডেল ম্যানেজমেন্ট এবং ইনফারেন্স পরিচালনা করে।

</details>

---

### অনুশীলন ৩: মডেল ডাউনলোড এবং লোড করুন

SDK ডাউনলোড (ডিস্কে) এবং লোড (মেমরিতে) আলাদা করে। এটা আপনাকে সেটআপের সময় পূর্বে মডেল ডাউনলোড এবং প্রয়োজনমতো লোড করতে সাহায্য করে।

<details>
<summary><h3>🐍 পাইটন</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# অপশন এ: ম্যানুয়াল ধাপে ধাপে
manager = FoundryLocalManager()
manager.start_service()

# আগে ক্যাশ চেক করুন
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

# অপশন বি: এক লাইন বুটস্ট্র্যাপ (প্রস্তাবিত)
# কনস্ট্রাক্টরকে এলিয়াস পাঠান - এটি পরিষেবা শুরু করে, ডাউনলোড করে এবং স্বয়ংক্রিয়ভাবে লোড করে
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### পাইটন - মডেল ব্যবস্থাপনা পদ্ধতি

| পদ্ধতি | স্বাক্ষর | বিবরণ |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | একটি মডেল লোকাল ক্যাশে ডাউনলোড করুন |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | মেমরিতে মডেল লোড করুন ইনফারেন্স সার্ভারে |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | সার্ভার থেকে মডেল আনলোড করুন |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | বর্তমানে লোডকৃত মডেলের তালিকা দেখুন |

#### পাইটন - ক্যাশ ব্যবস্থাপনা পদ্ধতি

| পদ্ধতি | স্বাক্ষর | বিবরণ |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | ক্যাশ ডিরেক্টরি পাথ পান |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | ডাউনলোডকৃত সকল মডেলের তালিকা দেখুন |

</details>

<details>
<summary><h3>📘 জাভাস্ক্রিপ্ট</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// পর্যায়ক্রমিক পদ্ধতি
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

#### জাভাস্ক্রিপ্ট - মডেল পদ্ধতি

| পদ্ধতি | স্বাক্ষর | বিবরণ |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | মডেল ইতোমধ্যেই ডাউনলোড হয়েছে কিনা |
| `model.download()` | `() => Promise<void>` | মডেল লোকাল ক্যাশে ডাউনলোড করুন |
| `model.load()` | `() => Promise<void>` | ইনফারেন্স সার্ভারে লোড করুন |
| `model.unload()` | `() => Promise<void>` | ইনফারেন্স সার্ভার থেকে আনলোড করুন |
| `model.id` | `string` | মডেলের ইউনিক আইডি |

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

#### C# - মডেল পদ্ধতি

| পদ্ধতি | বিবরণ |
|--------|-------------|
| `model.DownloadAsync(progress?)` | নির্বাচিত ভেরিয়েন্ট ডাউনলোড করুন |
| `model.LoadAsync()` | মেমরিতে মডেল লোড করুন |
| `model.UnloadAsync()` | মডেল আনলোড করুন |
| `model.SelectVariant(variant)` | নির্দিষ্ট ভেরিয়েন্ট নির্বাচন করুন (CPU/GPU/NPU) |
| `model.SelectedVariant` | বর্তমানে নির্বাচিত ভেরিয়েন্ট |
| `model.Variants` | মডেলের সমস্ত উপলব্ধ ভেরিয়েন্ট |
| `model.GetPathAsync()` | লোকাল ফাইল পাথ পান |
| `model.GetChatClientAsync()` | নেটিভ চ্যাট ক্লায়েন্ট পান (OpenAI SDK প্রয়োজন নেই) |
| `model.GetAudioClientAsync()` | ট্রান্সক্রিপশনের জন্য অডিও ক্লায়েন্ট পান |

</details>

---

### অনুশীলন ৪: মডেল মেটাডেটা পরিদর্শন করুন

`FoundryModelInfo` অবজেক্টে প্রতিটি মডেলের সম্পন্ন মেটাডেটা থাকে। এই ক্ষেত্রগুলো বোঝা সাহায্য করে আপনার অ্যাপ্লিকেশনের জন্য সঠিক মডেল নির্বাচন করতে।

<details>
<summary><h3>🐍 পাইটন</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# একটি নির্দিষ্ট মডেল সম্পর্কে বিস্তারিত তথ্য পান
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
<summary><h3>📘 জাভাস্ক্রিপ্ট</h3></summary>

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

#### FoundryModelInfo ক্ষেত্র

| ক্ষেত্র | ধরন | বিবরণ |
|-------|------|-------------|
| `alias` | string | সংক্ষিপ্ত নাম (যেমন `phi-3.5-mini`) |
| `id` | string | ইউনিক মডেল আইডি |
| `version` | string | মডেল সংস্করণ |
| `task` | string | `chat-completions` বা `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, অথবা NPU |
| `execution_provider` | string | রানটাইম ব্যাকএন্ড (CUDA, CPU, QNN, WebGPU ইত্যাদি) |
| `file_size_mb` | int | ডিস্কে সাইজ (MB) |
| `supports_tool_calling` | bool | মডেল ফাংশন/টুল কলিং সাপোর্ট করে কিনা |
| `publisher` | string | মডেল প্রকাশক |
| `license` | string | লাইসেন্স নাম |
| `uri` | string | মডেল URI |
| `prompt_template` | dict/null | প্রম্পট টেমপ্লেট, যদি থাকে |

---

### অনুশীলন ৫: মডেল লাইফসাইকেল পরিচালনা করুন

পূর্ণ লাইফসাইকেল অনুশীলন করুন: তালিকা → ডাউনলোড → লোড → ব্যবহার → আনলোড।

<details>
<summary><h3>🐍 পাইটন</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # দ্রুত পরীক্ষার জন্য ছোট মডেল

manager = FoundryLocalManager()
manager.start_service()

# ১. ক্যাটালগে কী আছে তা পরীক্ষা করুন
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# ২. কী ইতিমধ্যে ডাউনলোড হয়েছে তা পরীক্ষা করুন
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# ৩. একটি মডেল ডাউনলোড করুন
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# ৪. এটি এখন ক্যাশে আছে কিনা যাচাই করুন
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# ৫. এটি লোড করুন
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# ৬. কী লোড হয়েছে তা পরীক্ষা করুন
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# ৭. এটি আনলোড করুন
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

const alias = "qwen2.5-0.5b"; // দ্রুত পরীক্ষার জন্য ছোট মডেল

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. ক্যাটালগ থেকে মডেল নিন
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. প্রয়োজনে ডাউনলোড করুন
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. এটি লোড করুন
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. এটি আনলোড করুন
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Exercise 6: The Quick-Start Patterns

প্রতিটি ভাষা একটি শর্টকাট সরবরাহ করে যা এক কলেই সার্ভিস শুরু এবং একটি মডেল লোড করে। এই গুলো বেশিরভাগ অ্যাপ্লিকেশনের জন্য **সুপারিশকৃত প্যাটার্ন**।

<details>
<summary><h3>🐍 Python - Constructor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# কনস্ট্রাক্টরে একটি অলিয়াস পাস করুন - এটি সবকিছু পরিচালনা করে:
# 1. সার্ভিস চলমান না থাকলে শুরু করে
# 2. মডেল ক্যাশ না থাকলে ডাউনলোড করে
# 3. মডেলটি ইনফারেন্স সার্ভারে লোড করে
manager = FoundryLocalManager("phi-3.5-mini")

# সাথে সাথে ব্যবহারের জন্য প্রস্তুত
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` প্যারামিটার (ডিফল্ট `True`) এই আচরণ নিয়ন্ত্রণ করে। যদি আপনি ম্যানুয়াল নিয়ন্ত্রণ চান, তবে `bootstrap=False` সেট করুন:

```python
# ম্যানুয়াল মোড - কিছুই স্বয়ংক্রিয়ভাবে ঘটে না
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() সবকিছু পরিচালনা করে:
// 1. সার্ভিস শুরু করে
// 2. ক্যাটালগ থেকে মডেল নেয়
// 3. প্রয়োজনে ডাউনলোড করে এবং মডেল লোড করে
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// অবিলম্বে ব্যবহার করার জন্য প্রস্তুত
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Catalog</h3></summary>

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

> **C# নোট:** C# SDK `Configuration` ব্যবহার করে অ্যাপের নাম, লগিং, ক্যাশ ডিরেক্টরি এবং এমনকি একটি নির্দিষ্ট ওয়েব সার্ভার পোর্টও নিয়ন্ত্রণ করে। এটি এই তিন SDK’র মধ্যে সবচেয়ে কনফিগারযোগ্য।

</details>

---

### Exercise 7: The Native ChatClient (No OpenAI SDK Needed)

JavaScript এবং C# SDK গুলো একটি `createChatClient()` সুবিধাজনক মেথড দেয় যা একটি নেটিভ চ্যাট ক্লায়েন্ট রিটার্ন করে — OpenAI SDK আলাদাভাবে ইনস্টল বা কনফিগার করার প্রয়োজন নেই।

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

// মডেল থেকে সরাসরি একটি ChatClient তৈরি করুন — কোনো OpenAI ইম্পোর্টের প্রয়োজন নেই
const chatClient = model.createChatClient();

// completeChat একটি OpenAI-সঙ্গত সাড়া অবজেক্ট ফেরত দেয়
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// স্ট্রিমিং একটি কলব্যাক প্যাটার্ন ব্যবহার করে
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` টুল কলিংও সাপোর্ট করে — টুলগুলো দ্বিতীয় আর্গুমেন্ট হিসেবে পাস করুন:

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

> **কখন কোন প্যাটার্ন ব্যবহার করবেন:**
> - **`createChatClient()`** — দ্রুত প্রোটোটাইপিং, কম ডিপেনডেন্সি, সহজ কোড
> - **OpenAI SDK** — প্যারামিটারগুলোর পূর্ণ নিয়ন্ত্রণ (তাপমাত্রা, top_p, স্টপ টোকেন ইত্যাদি), প্রডাকশনের জন্য ভালো

---

### Exercise 8: Model Variants and Hardware Selection

মডেলগুলোর একাধিক **ভারিয়েন্ট** থাকতে পারে যা বিভিন্ন হার্ডওয়্যার জন্য অপ্টিমাইজ করা হয়। SDK স্বয়ংক্রিয়ভাবে সেরা ভারিয়েন্ট নির্বাচন করে, কিন্তু আপনি নিজেও ইন্সপেক্ট করে ম্যানুয়ালি চয়ন করতে পারেন।

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// সমস্ত উপলভ্য ভেরিয়েন্টগুলি তালিকা করুন
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK স্বয়ংক্রিয়ভাবে আপনার হার্ডওয়্যার এর জন্য সেরা ভেরিয়েন্ট নির্বাচন করে
// ওভাররাইড করতে, selectVariant() ব্যবহার করুন:
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

Python এ SDK স্বয়ংক্রিয়ভাবে হার্ডওয়্যার অনুসারে সেরা ভারিয়েন্ট নির্বাচন করে। আপনি `get_model_info()` ব্যবহার করে যা সিলেক্ট হয়েছে দেখতে পারেন:

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

#### NPU ভারিয়েন্ট সহ মডেলসমূহ

কয়েকটি মডেল NPU-অপ্টিমাইজড ভারিয়েন্ট সহ যার জন্য Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra) ব্যবহার হয়:

| মডেল | NPU ভারিয়েন্ট উপলব্ধ |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **টিপ:** NPU-সমর্থিত হার্ডওয়্যারে, SDK স্বয়ংক্রিয়ভাবে যখন উপলব্ধ তখন NPU ভারিয়েন্ট বেছে নেয়। আপনাকে আপনার কোড পরিবর্তন করতে হবে না। Windows-এ C# প্রকল্পের জন্য `Microsoft.AI.Foundry.Local.WinML` NuGet প্যাকেজ যুক্ত করুন যাতে QNN এক্সিকিউশন প্রোভাইডার সক্রিয় হয় — QNN একটি WinML প্লাগইন EP হিসেবে সরবরাহ করা হয়।

---

### Exercise 9: Model Upgrades and Catalog Refresh

মডেল ক্যাটালগ সময়ে সময়ে আপডেট হয়। আপডেট পরীক্ষা ও প্রয়োগ করতে নিচের মেথডগুলো ব্যবহার করুন।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# সর্বশেষ মডেল তালিকা পাওয়ার জন্য ক্যাটালগ রিফ্রেশ করুন
manager.refresh_catalog()

# পরীক্ষা করুন একটি ক্যাশ করা মডেল কি নতুন সংস্করণ উপলব্ধ আছে কিনা
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

// সর্বশেষ মডেল তালিকা আনতে ক্যাটালগ রিফ্রেশ করুন
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// রিফ্রেশের পর সমস্ত উপলব্ধ মডেল তালিকা করুন
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Exercise 10: Working with Reasoning Models

**phi-4-mini-reasoning** মডেল চেইন-অফ-থট রিজনিং অন্তর্ভুক্ত করে। এটি এর অভ্যন্তরীণ চিন্তাভাবনাকে `<think>...</think>` ট্যাগে মোড়ানো অবস্থায় ফাইনাল উত্তর তৈরি করে। মাল্টি-স্টেপ লজিক, গণিত, বা সমস্যা সমাধানের কাজের জন্য এটি উপকারী।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning প্রায় ৪.৬ জিবি
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# মডেলটি তার চিন্তাভাবনাকে <think>...</think> ট্যাগে মোড়ানো হয়
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

// চিন্তার শৃঙ্খলা বিশ্লেষণ করুন
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **রিজনিং মডেল কখন ব্যবহার করবেন:**
> - গণিত ও লজিক্যাল সমস্যা সমাধানে
> - মাল্টি-স্টেপ পরিকল্পনার কাজে
> - জটিল কোড জেনারেশনে
> - কাজ যেখানে কাজের প্রক্রিয়া দেখানো সঠিকতা বাড়ায়
>
> **ট্রেড-অফ:** রিজনিং মডেল বেশি টোকেন (যেমন `<think>` অংশ) তৈরি করে এবং ধীরগতির। সাধারণ প্রশ্ন-উত্তরের জন্য যেমন phi-3.5-mini মডেল দ্রুত।

---

### Exercise 11: Understanding Aliases and Hardware Selection

আপনি যখন একটি **অ্যালিয়াস** (যেমন `phi-3.5-mini`) দেয়ার পরিবর্তে একটি পূর্ণ মডেল আইডি না দিয়ে, SDK স্বয়ংক্রিয়ভাবে আপনার হার্ডওয়্যার অনুযায়ী সেরা ভারিয়েন্ট নির্বাচন করে:

| হার্ডওয়্যার | নির্বাচিত এক্সিকিউশন প্রোভাইডার |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML প্লাগইন মাধ্যমে) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| যেকোনো ডিভাইস (ফলব্যাক) | `CPUExecutionProvider` বা `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# এলিয়াসটি আপনার হার্ডওয়্যারের জন্য সেরা ভেরিয়েন্টে সমাধান করে
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **টিপ:** আপনার অ্যাপ্লিকেশন কোডে সর্বদা অ্যালিয়াস ব্যবহার করুন। ব্যবহারকারীর মেশিনে ডিপ্লয় করার সময়, SDK রানটাইমে সর্বোত্তম ভারিয়েন্ট বেছে নেয় - NVIDIA-তে CUDA, Qualcomm-এ QNN, অন্যত্র CPU।

---

### Exercise 12: C# Configuration Options

C# SDK এর `Configuration` ক্লাস রানটাইম নিয়ন্ত্রণের জন্য সূক্ষ্ম নিয়ন্ত্রণ সরবরাহ করে:

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

| প্রপার্টি | ডিফল্ট | বর্ণনা |
|----------|---------|-------------|
| `AppName` | (প্রয়োজনীয়) | আপনার অ্যাপের নাম |
| `LogLevel` | `Information` | লগিংয়ের Verbosity |
| `Web.Urls` | (ডায়নামিক) | ওয়েব সার্ভারের জন্য নির্দিষ্ট একটি পোর্ট পিন করা |
| `AppDataDir` | OS ডিফল্ট | অ্যাপ ডেটার বেস ডিরেক্টরি |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | যেখানে মডেলগুলো সংরক্ষিত থাকে |
| `LogsDir` | `{AppDataDir}/logs` | যেখানে লগ লিখিত হয় |

---

### Exercise 13: Browser Usage (JavaScript Only)

JavaScript SDK-তে একটি ব্রাউজার-সমর্থিত সংস্করণ আছে। ব্রাউজারে, আপনাকে CLI দিয়ে সার্ভিস ম্যানুয়ালি শুরু করতে হবে এবং হোস্ট URL নির্দিষ্ট করতে হবে:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// প্রথমে সার্ভিসটি ম্যানুয়ালি শুরু করুন:
//   foundry সার্ভিস শুরু করুন
// তারপর CLI আউটপুট থেকে URL ব্যবহার করুন
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// ক্যাটালগ ব্রাউজ করুন
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **ব্রাউজার সীমাবদ্ধতা:** ব্রাউজার ভার্সন `startWebService()` সমর্থন করে না। Foundry Local সার্ভিস পূর্বেই চালু থাকতে হবে ব্রাউজারে SDK ব্যবহার করার আগে।

---

## Complete API Reference

### Python

| ক্যাটাগরি | মেথড | বর্ণনা |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | ম্যানেজার তৈরি; ঐচ্ছিকভাবে একটি মডেল দিয়ে বুটস্ট্র্যাপ |
| **Service** | `is_service_running()` | সার্ভিস চলছে কিনা চেক করুন |
| **Service** | `start_service()` | সার্ভিস শুরু করুন |
| **Service** | `endpoint` | API এন্ডপয়েন্ট URL |
| **Service** | `api_key` | API কী |
| **Catalog** | `list_catalog_models()` | সব উপলব্ধ মডেল তালিকা করুন |
| **Catalog** | `refresh_catalog()` | ক্যাটালগ রিফ্রেশ করুন |
| **Catalog** | `get_model_info(alias_or_model_id)` | মডেলের মেটাডেটা পান |
| **Cache** | `get_cache_location()` | ক্যাশ ডিরেক্টরির পাথ |
| **Cache** | `list_cached_models()` | ডাউনলোড করা মডেল তালিকা করুন |
| **Model** | `download_model(alias_or_model_id)` | একটি মডেল ডাউনলোড করুন |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | একটি মডেল লোড করুন |
| **Model** | `unload_model(alias_or_model_id)` | একটি মডেল আনলোড করুন |
| **Model** | `list_loaded_models()` | Loaded মডেল তালিকা করুন |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | দেখুন নতুন ভার্সন আছে কিনা |
| **Model** | `upgrade_model(alias_or_model_id)` | মডেল সর্বশেষ ভার্সনে আপগ্রেড করুন |
| **Service** | `httpx_client` | সরাসরি API কলের জন্য প্রাক-কনফিগার্ড HTTPX ক্লায়েন্ট |

### JavaScript

| ক্যাটাগরি | মেথড | বর্ণনা |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | SDK সিঙ্গলটন ইনিশিয়ালাইজ করুন |
| **Init** | `FoundryLocalManager.instance` | সিঙ্গলটন ম্যানেজার অ্যাক্সেস করুন |
| **Service** | `manager.startWebService()` | ওয়েব সার্ভিস শুরু করুন |
| **Service** | `manager.urls` | সার্ভিসের বেস URL এর অ্যারে |
| **Catalog** | `manager.catalog` | মডেল ক্যাটালগ অ্যাক্সেস করুন |
| **Catalog** | `catalog.getModel(alias)` | অ্যালিয়াস দ্বারা মডেল অবজেক্ট পান (Promise রিটার্ন করে) |
| **Model** | `model.isCached` | মডেল ডাউনলোড হয়েছে কিনা |
| **Model** | `model.download()` | মডেল ডাউনলোড করুন |
| **Model** | `model.load()` | মডেল লোড করুন |
| **Model** | `model.unload()` | মডেল আনলোড করুন |
| **Model** | `model.id` | মডেলের ইউনিক আইডি |
| **Model** | `model.alias` | মডেলের অ্যালিয়াস |
| **Model** | `model.createChatClient()` | নেটিভ চ্যাট ক্লায়েন্ট পান (OpenAI SDK প্রয়োজন নেই) |
| **Model** | `model.createAudioClient()` | ট্রান্সক্রিপশনের জন্য অডিও ক্লায়েন্ট পান |
| **Model** | `model.removeFromCache()` | লোকাল ক্যাশ থেকে মডেল সরান |
| **Model** | `model.selectVariant(variant)` | নির্দিষ্ট হার্ডওয়্যার ভারিয়েন্ট নির্বাচন করুন |
| **Model** | `model.variants` | এই মডেলটির উপলব্ধ ভারিয়েন্টগুলোর অ্যারে |
| **Model** | `model.isLoaded()` | মডেল বর্তমানে লোড করা আছে কিনা চেক করুন |
| **Catalog** | `catalog.getModels()` | সব উপলব্ধ মডেল তালিকা করুন |
| **Catalog** | `catalog.getCachedModels()` | ডাউনলোড করা মডেল তালিকা করুন |
| **Catalog** | `catalog.getLoadedModels()` | বর্তমানে লোডকৃত মডেল তালিকা করুন |
| **Catalog** | `catalog.updateModels()` | সার্ভিস থেকে ক্যাটালগ রিফ্রেশ করুন |
| **Service** | `manager.stopWebService()` | Foundry Local ওয়েব সার্ভিস বন্ধ করুন |

### C# (v0.8.0+)

| ক্যাটাগরি | মেথড | বর্ণনা |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | ম্যানেজার ইনিশিয়ালাইজ করুন |
| **Init** | `FoundryLocalManager.Instance` | সিঙ্গলটন অ্যাক্সেস করুন |
| **Catalog** | `manager.GetCatalogAsync()` | ক্যাটালগ পান |
| **Catalog** | `catalog.ListModelsAsync()` | সব মডেল তালিকা করুন |
| **Catalog** | `catalog.GetModelAsync(alias)` | একটি নির্দিষ্ট মডেল পান |
| **Catalog** | `catalog.GetCachedModelsAsync()` | ক্যাশ করা মডেল তালিকা করুন |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | লোডকৃত মডেল তালিকা করুন |
| **Model** | `model.DownloadAsync(progress?)` | একটি মডেল ডাউনলোড করুন |
| **Model** | `model.LoadAsync()` | মডেল লোড করুন |
| **Model** | `model.UnloadAsync()` | মডেল আনলোড করুন |
| **Model** | `model.SelectVariant(variant)` | হার্ডওয়্যার ভারিয়েন্ট বেছে নিন |
| **Model** | `model.GetChatClientAsync()` | নেটিভ চ্যাট ক্লায়েন্ট পান |
| **Model** | `model.GetAudioClientAsync()` | অডিও ট্রান্সক্রিপশন ক্লায়েন্ট পান |
| **Model** | `model.GetPathAsync()` | লোকাল ফাইল পাথ পান |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | নির্দিষ্ট হার্ডওয়্যার ভারিয়েন্ট পান |
| **Catalog** | `catalog.UpdateModelsAsync()` | ক্যাটালগ রিফ্রেশ করুন |
| **Server** | `manager.StartWebServerAsync()` | REST ওয়েব সার্ভার শুরু করুন |
| **Server** | `manager.StopWebServerAsync()` | REST ওয়েব সার্ভার বন্ধ করুন |
| **Config** | `config.ModelCacheDir` | ক্যাশ ডিরেক্টরি |

---

## Key Takeaways

| ধারণা | আপনি যা শিখলেন |
|---------|-----------------|
| **SDK vs CLI** | SDK প্রোগ্রাম্যাটিক নিয়ন্ত্রণ দেয় - অ্যাপ্লিকেশন জন্য অপরিহার্য |
| **Control plane** | SDK সার্ভিস, মডেল, ও ক্যাশ ম্যানেজ করে |
| **Dynamic ports** | সবসময় `manager.endpoint` (Python) বা `manager.urls[0]` (JS/C#) ব্যবহার করুন - পোর্ট হার্ডকোড করবেন না |
| **Aliases** | অ্যালিয়াস ব্যবহার করুন স্বয়ংক্রিয় হার্ডওয়্যার-সেরা মডেল নির্বাচন জন্য |
| **দ্রুত শুরু** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# পুনর্নির্মাণ** | v0.8.0+ স্বয়ংসম্পূর্ণ - এন্ড-ইউজার যন্ত্রে CLI দরকার নেই |
| **মডেল জীবনচক্র** | ক্যাটালগ → ডাউনলোড → লোড → ব্যবহার → আনলোড |
| **FoundryModelInfo** | সমৃদ্ধ মেটাডেটা: টাস্ক, ডিভাইস, আকার, লাইসেন্স, টুল কলিং সাপোর্ট |
| **ChatClient** | OpenAI-মুক্ত ব্যবহারের জন্য `createChatClient()` (JS) / `GetChatClientAsync()` (C#) |
| **ভেরিয়েন্টস** | মডেলগুলির হার্ডওয়্যার-নির্দিষ্ট ভেরিয়েন্টস থাকে (CPU, GPU, NPU); স্বয়ংক্রিয়ভাবে নির্বাচিত |
| **আপগ্রেডস** | Python: মডেল আপ-টু-ডেট রাখার জন্য `is_model_upgradeable()` + `upgrade_model()` |
| **ক্যাটালগ রিফ্রেশ** | নতুন মডেল খুঁজে পেতে `refresh_catalog()` (Python) / `updateModels()` (JS) |

---

## রিসোর্সসমূহ

| রিসোর্স | লিঙ্ক |
|----------|------|
| SDK রেফারেন্স (সব ভাষা) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| ইনফারেন্স SDK এর সাথে একত্রিত করুন | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API রেফারেন্স | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK স্যাম্পলস | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local ওয়েবসাইট | [foundrylocal.ai](https://foundrylocal.ai) |

---

## পরবর্তী ধাপসমূহ

SDK কে OpenAI ক্লায়েন্ট লাইব্রেরির সাথে সংযুক্ত করতে এবং আপনার প্রথম চ্যাট সম্পন্ন করার অ্যাপ্লিকেশন তৈরির জন্য [Part 3: Using the SDK with OpenAI](part3-sdk-and-apis.md) এ যেতে থাকুন।