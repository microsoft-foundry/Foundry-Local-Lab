![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ផ្នែកទី 2: ការដល់ជ្រៅលើ Foundry Local SDK

> **គោលដៅ:** ជំនាញលើ Foundry Local SDK ដើម្បីគ្រប់គ្រងម៉ូឌែល ខ្នងសេវា និងការផ្ទុកឡើងវិញជាផែនការជំនួយកម្មវិធី - និងយល់ដឹងពីមូលហេតុដែល SDK គឺជាវិធីសាស្រ្តដែលបានផ្ដល់អាទិភាពលើ CLI សម្រាប់ការសង់កម្មវិធី។

## ទិដ្ឋភាពទូទៅ

នៅផ្នែក 1 អ្នកបានប្រើប្រាស់ **Foundry Local CLI** ដើម្បីទាញយក និងរត់ម៉ូឌែលប្រតិបត្តិការយ៉ាងអន្តរជាតិក្នុងចំណោម។ CLI គឺល្អសម្រាប់ការស្វែងរក ប៉ុន្តេលើពេលដែលអ្នកសង់កម្មវិធីពិតប្រាកដ អ្នកត្រូវការត្រួតពិនិត្យដោយកម្មវិធី។ Foundry Local SDK ផ្ដល់អ្វីនោះឲ្យអ្នក - វាគ្រប់គ្រង **ផ្ទាល់គ្រប់គ្រង** (ចាប់ផ្តើមសេវា ស្វែងរកម៉ូឌែល ទាញយក ផ្ទុក) ដូច្នេះកូដកម្មវិធីរបស់អ្នកអាចផ្ដោតលើ **ផ្ទាល់ទិន្នន័យ** (ផ្ញើសាងសំណុំ ស្វែងយកការបញ្ចប់)។

មន្ទីរពិសោធន៍នេះបង្រៀនអ្នកលើផ្ទៃ API SDK ពេញលេញគ្រប់ផ្នែក Python, JavaScript, និង C#។ នៅចុងបញ្ចប់ អ្នកនឹងយល់ពីវិធីសាស្រ្តគ្រប់យ៉ាងដែលអាចប្រើ និងពេលវេលាត្រូវប្រើ។

## គោលដៅការសិក្សា

នៅចុងបញ្ចប់នៃមន្ទីរពិសោធន៍នេះ អ្នកនឹងអាច៖

- ពន្យល់ថាហេតុអ្វី SDK ကိုផ្ដល់អាទិភាពលើ CLI សម្រាប់ការអភិវឌ្ឍកម្មវិធី
- ដំឡើង Foundry Local SDK សម្រាប់ Python, JavaScript, ឬ C#
- ប្រើ `FoundryLocalManager` ដើម្បីចាប់ផ្តើមសេវា គ្រប់គ្រងម៉ូឌែល និងសួរសំណួរអំពីកាតាឡុក
- រាយបញ្ជី ទាញយក ផ្ទុក និងដកយកម៉ូឌែលជាកម្មវិធី
- ពិនិត្យមetadata ម៉ូឌែលប្រើ `FoundryModelInfo`
- យល់ពីភាពខុសគ្នារវាងកាតាឡុក ការផ្ទុកឡើងវិញ និងម៉ូឌែលដែលត្រូវបានផ្ទុក
- ប្រើ constructor bootstrap (Python) និង `create()` + រូបមន្តកាតាឡុក (JavaScript)
- យល់ពីការរចនាវិញរបស់ SDK C# និង API៣ផ្នែកផ្អែកលើវត្ថុ

---

## លក្ខខណ្ឌជាមុន

| តម្រូវការ | សេចក្តីលម្អិត |
|-------------|---------|
| **Foundry Local CLI** | បានដំឡើង និងមាននៅលើ `PATH` របស់អ្នក ([Part 1](part1-getting-started.md)) |
| **បរិវេណភាសា** | **Python 3.9+** និង/ឬ **Node.js 18+** និង/ឬ **.NET 9.0+** |

---

## ការស្វែងយល់ៈ SDK ប្រៀបធៀប CLI - ហេតុអ្វីគួរប្រើ SDK?

| មានផងដែ | CLI (ពាក្យបញ្ជា `foundry`) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **ករណីប្រើប្រាស់** | ការស្វែងរក ការធ្វើតេស្តដោយដៃ | ការរួមបញ្ចូលកម្មវិធី |
| **ការគ្រប់គ្រងសេវា** | ដោយដៃ៖ `foundry service start` | ដោយស្វ័យចលនា៖ `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **ការស្វែងរកច្រកទ្វារ** | អានពីលទ្ធផល CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **ការទាញយកម៉ូឌែល** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **ការដោះស្រាយកំហុស** | លេខកូដចាកចេញ, stderr | ករណីចម្លងកំហុស, កំហុសបែប Typed |
| **ស្វ័យប្រវត្តិការ** | ស្គ្រីប shell | លាយបញ្ចូលជាភាសាដើម |
| **ការបង្ហោះ** | ត្រូវការមាន CLI លើម៉ាស៊ីនអ្នកប្រើផ្នែកចុងក្រោយ | SDK C# អាចហើយធ្វើរបស់ខ្លួន (មិនចាំបាច់ CLI) |

> **ចំណុចសំខាន់:** SDK គ្រប់គ្រងជីវវរណៈពេញលេញ៖ ចាប់ផ្តើមសេវា ពិនិត្យកាច ទាញយកម៉ូឌែលដែលខ្វះ បញ្ចូលវា និងស្វែងរកច្រកទ្វារ ក្នុងបន្ទាត់កូដខ្លីៗ។ កម្មវិធីរបស់អ្នកមិនចាំបាច់ផ្គូរផ្គងលទ្ធផល CLI ឬគ្រប់គ្រង subprocesses ទេ។

---

## មំនួញមន្ទីរពិសោធន៍

### មំនួញទី 1: ដំឡើង SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

ផ្ទៀងផ្ទាត់ការដំឡើង៖

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

ផ្ទៀងផ្ទាត់ការដំឡើង៖

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

មានកញ្ចប់ NuGet ពីរចម្បង៖

| កញ្ចប់ | វេទិកា | ការពិពណ៌នា |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | វេទិកា​ឆ្លាស់កាត់ | ដំណើរការលើ Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Windows តែប៉ុណ្ណោះ | បន្ថែមការបង្កបង្កើនប្រសិទ្ធភាពមេកាស WinML; ទាញយក និងដំឡើងអ្នកផ្តល់សេវាកម្មកម្មវិធីបន្ថែម (ឧ. QNN សម្រាប់ Qualcomm NPU) |

**ការកំណត់ Windows៖**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

កែសម្រួលឯកសារ `.csproj`៖

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

> **ចំណាំ៖** លើ Windows កញ្ចប់ WinML គឺជាកញ្ចប់ដ៏ពេញលេញរួមបញ្ចូល SDK មូលដ្ឋានសម្រាប់អ្នកផ្តល់សេវាកម្ម QNN។ លើ Linux/macOS ធ្វើយន្តកម្មSDK មូលដ្ឋាន ជំនួស។ TFM និងយោងកញ្ចប់លក្ខណៈលក្ខណៈគោលកំណត់ធ្វើឲ្យគម្រោងអាចដំណើរការឆ្លាស់កាត់វេទិកា។

បង្កើត `nuget.config` នៅឫសគម្រោង៖

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

ប្តូរកញ្ចប់៖

```bash
dotnet restore
```

</details>

---

### មំនួញទី 2: ចាប់ផ្តើមសេវា និងរាយត្រីកាតាឡុក

រឿងដំបូងដែលកម្មវិធីណាមួយធ្វើ គឺចាប់ផ្តើមសេវា Foundry Local និងស្វែងរកថាម៉ូឌែលអ្វីខ្លះមានស្រាប់។

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# បង្កើតអ្នកគ្រប់គ្រង និងចាប់ផ្តើមសេវាកម្ម
manager = FoundryLocalManager()
manager.start_service()

# បញ្ជីម៉ូដែលទាំងអស់ដែលមាននៅក្នុងកាតាឡុក
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - វិធីសាស្រ្តគ្រប់គ្រងសេវា

| វិធីសាស្រ្ត | រូបមន្ត | ការពិពណ៌នា |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | ពិនិត្យមើលថាសេវាកម្មមានរត់មែនទេ |
| `start_service()` | `() -> None` | ចាប់ផ្តើមសេវា Foundry Local |
| `service_uri` | `@property -> str` | អាសយដ្ឋាន URI សេវាកម្មគ្រឹះ |
| `endpoint` | `@property -> str` | ច្រក API (service URI + `/v1`) |
| `api_key` | `@property -> str` | ពាក្យកូនសោ API (ពីបរិស្ថាន ឬចាំបាច់តម្លើងតំណរ) |

#### Python SDK - វិធីសាស្រ្តគ្រប់គ្រងកាតាឡុក

| វិធីសាស្រ្ត | រូបមន្ត | ការពិពណ៌នា |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | រាយបញ្ជីម៉ូឌែលទាំងអស់ក្នុងកាតាឡុក |
| `refresh_catalog()` | `() -> None` | ធ្វើឡើងវិញកាតាឡុកពីសេវាកម្ម |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=false) -> FoundryModelInfo \| None` | ទទួលបានព័ត៌មានម៉ូឌែលតាមកូដឫឈ្មោះ |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// បង្កើតអ្នកគ្រប់គ្រង និងចាប់ផ្តើមសេវាកម្ម
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ស្វែងរកក្នុងកាតាឡុក
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - វិធីសាស្រ្តគ្រប់គ្រង Manager

| វិធីសាស្រ្ត | រូបមន្ត | ការពិពណ៌នា |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | ចាប់ផ្តើម SDK singleton |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | ចូលដំណើរការប្រភេទ singleton |
| `manager.startWebService()` | `() => Promise<void>` | ចាប់ផ្តើមសេវា Web Foundry Local |
| `manager.urls` | `string[]` | អារ៉េ URL គ្រឹះសម្រាប់សេវា |

#### JavaScript SDK - កាតាឡុក និងម៉ូឌែលវិធីសាស្រ្ត

| វិធីសាស្រ្ត | រូបមន្ត | ការពិពណ៌នា |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | ចូលដំណើរការកាតាឡុកម៉ូឌែល |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | ទទួលបានវត្ថុម៉ូឌែលតាមឈ្មោះ |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

SDK C# កំណែ v0.8.0+ ប្រើរចនាសម្ព័ន្ធផ្អែកលើវត្ថុ ជាមួយវត្ថុ `Configuration`, `Catalog`, និង `Model`៖

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

#### C# SDK - ថ្នាក់សំខាន់ៗ

| ថ្នាក់ | គោលបំណង |
|-------|---------|
| `Configuration` | កំណត់ឈ្មោះកម្មវិធី, កម្រិតកំណត់ហេតុ, ធាតុកីចាំទុក, URL របស់ម៉ាស៊ីនបម្រើវេប |
| `FoundryLocalManager` | ចំណុចចូលដំណើរ - បង្កើតតាម `CreateAsync()`, ចូលដំណើរការតាម `.Instance` |
| `Catalog` | ស្វែងរក ស្វែងយក និងទទួលម៉ូឌែលពីកាតាឡុក |
| `Model` | តំណាងម៉ូឌែលជាក់លាក់ - ទាញយក, ផ្ទុក, ទទួលអ្នកអតិថិជន |

#### C# SDK - វិធីសាស្រ្តគ្រប់គ្រង Manager និង Catalog

| វិធីសាស្រ្ត | ការពិពណ៌នា |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | ចាប់ផ្តើម manager |
| `FoundryLocalManager.Instance` | ចូលដំណើរការកម្មវិធីយូនីក |
| `manager.GetCatalogAsync()` | ទទួលបានកាតាឡុកម៉ូឌែល |
| `catalog.ListModelsAsync()` | រាយបញ្ជីម៉ូឌែលទាំងអស់ដែលមានស្រាប់ |
| `catalog.GetModelAsync(alias: "alias")` | ទទួលម៉ូឌែលដោយឈ្មោះតែប៉ុណ្ណោះ |
| `catalog.GetCachedModelsAsync()` | រាយបញ្ជីម៉ូឌែលដែលបានទាញយក |
| `catalog.GetLoadedModelsAsync()` | រាយបញ្ជីម៉ូឌែលដែលបានផ្ទុកទីពេលនេះ |

> **ចំណាំរចនាសម្ព័ន្ធ C#:** SDK C# កំណែ v0.8.0+ រចនាឡើងឱ្យកម្មវិធីមាន **ភាពថ្នាក់ខ្លួន**; មិនចាំបាច់មាន CLI Foundry Local នៅលើម៉ាស៊ីនអ្នកប្រើ។ SDK គ្រប់គ្រងម៉ូឌែល និងការប៉ាន់ប្រមាណដោយស្វ័យប្រវត្តិ។

</details>

---

### មំនួញទី 3: ទាញយក និងផ្ទុកម៉ូឌែល

SDK បំបែកការទាញយក (ទៅផ្ទាំងទិន្នន័យ) ពីការផ្ទុក (ចូលទៅក្នុងមេម៉ូរី)។ វាឲ្យអ្នកអាចទាញយកម៉ូឌែលជាមុននៅពេលដំឡើង ហើយផ្ទុកវាដោយតម្រូវការ។

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# ជម្រើស A: ជំហានដីហាតបុគ្គល
manager = FoundryLocalManager()
manager.start_service()

# ពិនិត្យមើលផ្ទុកឆែកជាមុន
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

# ជម្រើស B: ការតំឡើងមួយបន្ទាត់ (អនុញ្ញាត)
# ផ្តល់ឈ្មោះរូបរាងទៅឧបករណ៍កសាង - វាចាប់ផ្តើមសេវាកម្ម, រក្សាទាញយក, និងផ្ទុកដោយស្វ័យប្រវត្តិ
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - វិធីសាស្រ្តគ្រប់គ្រងម៉ូឌែល

| វិធីសាស្រ្ត | រូបមន្ត | ការពិពណ៌នា |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | ទាញយកម៉ូឌែលទៅកន្លែងផ្ទុកក្នុងតំបន់ |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | ផ្ទុកម៉ូឌែលចូលម៉ាស៊ីនបញ្ជាប៉ាន់ប្រមាណ |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | ដកម៉ូឌែលចេញពីម៉ាស៊ីនបញ្ជា |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | រាយបញ្ជីម៉ូឌែលដែលបានផ្ទុកទាំងអស់ |

#### Python - វិធីសាស្រ្តគ្រប់គ្រង Cache

| វិធីសាស្រ្ត | រូបមន្ត | ការពិពណ៌នា |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | ទទួលបានផ្លូវថ្នល់ឃ្លាំងកខ្វក់ |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | រាយបញ្ជីម៉ូឌែលដែលបានទាញយកទាំងអស់ |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// វិធីសាស្រ្តជំហាន់-ជំហាន់
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

#### JavaScript - វិធីសាស្រ្តម៉ូឌែល

| វិធីសាស្រ្ត | រូបមន្ត | ការពិពណ៌នា |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | តើម៉ូឌែលបានទាញយករួចហើយឬនៅ |
| `model.download()` | `() => Promise<void>` | ទាញយកម៉ូឌែលទៅកន្លែងផ្ទុកក្នុងតំបន់ |
| `model.load()` | `() => Promise<void>` | ផ្ទុកទៅម៉ាស៊ីនបញ្ជាប៉ាន់ប្រមាណ |
| `model.unload()` | `() => Promise<void>` | ដកចេញពីម៉ាស៊ីនបញ្ជាប៉ាន់ប្រមាណ |
| `model.id` | `string` | អត្តសញ្ញាណម៉ូឌែល |

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

#### C# - វិធីសាស្រ្តម៉ូឌែល

| វិធីសាស្រ្ត | ការពិពណ៌នា |
|--------|-------------|
| `model.DownloadAsync(progress?)` | ទាញយកវ៉ារីយ៉ង់ដែលបានជ្រើសរើស |
| `model.LoadAsync()` | ផ្ទុកម៉ូឌែលចូលទៅក្នុងម៉ាស៊ីនមេម៉ូរី |
| `model.UnloadAsync()` | ដកម៉ូឌែលចេញ |
| `model.SelectVariant(variant)` | ជ្រើសរើសវ៉ារីយ៉ង់ជាក់លាក់ (CPU/GPU/NPU) |
| `model.SelectedVariant` | វ៉ារីយ៉ង់ដែលបានជ្រើសរើសបច្ចុប្បន្ន |
| `model.Variants` | វ៉ារីយ៉ង់ទាំងអស់ដែលអាចប្រើបានសម្រាប់ម៉ូឌែលនេះ |
| `model.GetPathAsync()` | ទទួលបានផ្លូវឯកសារកន្លែងផ្ទុក |
| `model.GetChatClientAsync()` | ទទួលបានអ្នកអតិថិជនសន្ទនា ដើមទាំងអស់ (មិនចាំបាច់ OpenAI SDK) |
| `model.GetAudioClientAsync()` | ទទួលបានអ្នកអតិថិជនសំលេង សម្រាប់ការបកប្រែ |

</details>

---

### មំនួញទី 4: ពិនិត្យមetadata ម៉ូឌែល

វត្ថុ `FoundryModelInfo` មានមetadata ធំបំផុតអំពីម៉ូឌែលនីមួយៗ។ ការយល់ដឹងដល់វាលទាំងនេះជួយអោយអ្នកជ្រើសរើសម៉ូឌែលត្រឹមត្រូវសម្រាប់កម្មវិធីរបស់អ្នក។

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ទទួលបានព័ត៌មានលម្អិតអំពីគំរូជាក់លាក់មួយ
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

#### វាល FoundryModelInfo

| វាល | ប្រភេទ | ការពិពណ៌នា |
|-------|------|-------------|
| `alias` | string | ឈ្មោះខ្លី (ឧ. `phi-3.5-mini`) |
| `id` | string | អត្តសញ្ញាណម៉ូឌែលឯកត្ត |
| `version` | string | កំណែម៉ូឌែល |
| `task` | string | `chat-completions` ឬ `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, ឬ NPU |
| `execution_provider` | string | បច្ចេកវិទ្យាសំណង់ក្រោយ (CUDA, CPU, QNN, WebGPU, លើកន្លែង) |
| `file_size_mb` | int | ទំហំឯកសារលើឌីសភារម៉ែត្រ |
| `supports_tool_calling` | bool | វ៉ៃឲ្យម៉ូឌែលគាំទ្រហៅមុខងារ/ឧបករណ៍ |
| `publisher` | string | អ្នកផ្សព្វផ្សាយម៉ូឌែល |
| `license` | string | ឈ្មោះអាជ្ញាប័ណ្ណ |
| `uri` | string | អាសយដ្ឋានម៉ូឌែល URI |
| `prompt_template` | dict/null | រូបមន្តសំណួរ ប្រសិនបើមាន |

---

### មំនួញទី 5: គ្រប់គ្រងជីវវរណៈម៉ូឌែល

ហាត់ដំណើរការជីវវរណៈពេញលេញ៖ រាយបញ្ជី → ទាញយក → ផ្ទុក → ប្រើ → ដកចេញ។

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # ម៉ូដែលតូចសម្រាប់តេស្តលឿន

manager = FoundryLocalManager()
manager.start_service()

# ១. ពិនិត្យមើលអ្វីដែលមានក្នុងសៀវភៅបញ្ជី
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# ២. ពិនិត្យមើលអ្វីដែលបានទាញយករួចហើយ
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# ៣. ទាញយកម៉ូដែលមួយ
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# ៤. ផ្ទៀងផ្ទាត់ថាវា​នៅក្នុងផ្ទុកជាបច្ចុប្បន្ន
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# ៥. បង្ហាញវា
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# ៦. ពិនិត្យមើលអ្វីដែលបានបង្ហាញ
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# ៧. ធ្វើអោយវាពុំត្រូវបានបង្ហាញទៀត
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

const alias = "qwen2.5-0.5b"; // ម៉ូដែលតូចសម្រាប់ការធ្វើតេស្តរហ័ស

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. ទទួលម៉ូដែលពីកាតាឡុក
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. ទាញយកបើចាំបាច់
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. ផ្ទុកវា
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. បញ្ចេញវា
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### លំហាត់ 6៖ គំរូចាប់ផ្តើមលឿន

ភាសាដែលមាននារូបភាពផ្ដល់ជូននូវផ្លូវកាត់សំរាប់ចាប់ផ្តើមសេវាកម្ម និងផ្ទុកម៉ូដែលក្នុងការហៅមួយដង។ នេះគឺជាគំរូ **ផ្ដល់អនុសាសន៍** សម្រាប់កម្មវិធីភាគច្រើន។

<details>
<summary><h3>🐍 Python - កម្មវិធីបើក Constructor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# ផ្ញើឈ្មោះយោបល់ទៅម៉ាស៊ីនបង្កើត - វាដំណើរការពីរបៀបគ្រប់យ៉ាង:
# 1. ចាប់ផ្តើមសេវាកម្មបើមិនកំពុងរត់
# 2. ទាញយកម៉ូដែលបើមិនមានក្នុងផ្ទុកផង
# 3. ដំណើរការម៉ូដែលទៅក្នុងម៉ាស៊ីនបំពេញការប្រាក់
manager = FoundryLocalManager("phi-3.5-mini")

# រៀបចំប្រើបានភ្លាមៗ
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

ប៉ារ៉ាម៉ែត្រ `bootstrap` (លំនាំដើម `True`) គ្រប់គ្រងឥរិយាបថនេះ។ កំណត់ `bootstrap=False` ប្រសិនបើអ្នកចង់គ្រប់គ្រងដោយដៃ៖

```python
# របៀបមនុស្សបញ្ជា - គ្មានអ្វីកើតឡើងដោយស្វ័យប្រវត្តិ
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + ទិញបញ្ជីគំរូ Catalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() ដំណើរការប្រព័ន្ធទាំងមូល៖
// 1. ចាប់ផ្តើមសេវាកម្ម
// 2. ទទួលយកម៉ូដែលពីកាតាឡុក
// 3. ទាញយកបើចាំបាច់ និងដាក់បញ្ចូលម៉ូដែល
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// រួចរាល់សម្រាប់ប្រើបានភ្លាមៗ
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + ទិញបញ្ជីគំរូ Catalog</h3></summary>

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

> **ចំណាំ C#:** SDK C# ប្រើ `Configuration` ដើម្បីគ្រប់គ្រងឈ្មោះកម្មវិធី ការចេញកំណត់ហេតុ កន្លែងផ្ទុក cache និងកំណត់ច្រកបណ្តាញសំរាប់ម៉ាស៊ីនបម្រើតែមួយ។ វាជាឧបករណ៍ដែលអាចកំណត់បំផុតក្នុងចំណោម SDK ទាំងបី។

</details>

---

### លំហាត់ 7៖ ChatClient ដើមដំណើរការ (មិនចាំបាច់មាន OpenAI SDK)

SDK JavaScript និង C# ផ្ដល់ជូនវិធីសាស្ត្រ `createChatClient()` ដែលផ្ដល់ ChatClient ដើម​ដំណើរការ — មិនចាំបាច់ដំឡើងឬកំណត់កម្មវិធី OpenAI SDK ផ្សេងទៀត។

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

// បង្កើត ChatClient ពីម៉ូដែលដោយផ្ទាល់ — មិនត្រូវការនាំចូល OpenAI ទេ
const chatClient = model.createChatClient();

// completeChat ផ្តល់ត្រឡប់វត្ថុចម្លើយដែលផ្គូរផ្គងជាមួយ OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// ការចាក់ផ្សាយប្រើលំនាំហៅត្រឡប់ក្ដី
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` ក៏គាំទ្រការហៅឧបករណ៍ផងដែរ — ផ្ដល់ឧបករណ៍ជាអាគុយម៉ង់ទីពីរ៖

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

> **ពេលណាត្រូវប្រើគំរូណាមួយ៖**
> - **`createChatClient()`** — ការផលិតគំរូលឿន, ការពន្យល់តិច, កូដសាមញ្ញ
> - **OpenAI SDK** — គ្រប់គ្រងពេញលេញលើប៉ារ៉ាម៉ែត្រ (សីតុណ្ហភាព, top_p, ចុងសញ្ញា stop tokens ល។) ល្អសម្រាប់កម្មវិធីផលិតកម្ម

---

### លំហាត់ 8៖ រូបមន្តម៉ូដែល និងជម្រើសឧបករណ៍ផ្គត់ផ្គង់

ម៉ូដែលអាចមាន **វ៉ារីអង់** ពហុភាព ដែលបំលែងបានសម្រាប់ខ្សែប្រភេទឧបករណ៍ផ្សេងៗ។ SDK ជ្រើសវ៉ារីអង់ល្អបំផុតដោយស្វ័យប្រវត្តិ ប៉ុន្តែអ្នកអាចពិនិត្យនិងជ្រើសដោយដៃផង។

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// បញ្ជីរាបរាងទាំងអស់ដែលមាន
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK ជ្រើសរើសរាបរាងល្អបំផុតសម្រាប់ឧបករណ៍របស់អ្នក secaraoto
// ដើម្បីលើសកំណត់, ប្រើ selectVariant():
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

នៅក្នុង Python, SDK ជ្រើសវ៉ារីអង់ល្អបំផុតដោយស្វ័យប្រវត្តិប្រៀបធៀបតាមហාර්ដវែរដែលមាន។ ប្រើ `get_model_info()` ដើម្បីមើលអ្វីដែលបានជ្រើស៖

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

#### ម៉ូដែលជាមួយវ៉ារីអង់ NPU

ម៉ូដែលខ្លះមានវ៉ារីអង់ NPU ដែលបំលែងសាកសមសម្រាប់ឧបករណ៍ដែលមាន Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra)៖

| ម៉ូដែល | មានវ៉ារីអង់ NPU រួច |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **គន្លឹះ៖** នៅលើឧបករណ៍ដែលគាំទ្រ NPU, SDK ជ្រើសវ៉ារីអង់ NPU ដោយស្វ័យប្រវត្តិក្នុងករណីមាន។ អ្នកមិនចាំបាច់ផ្លាស់ប្ដូរកូដឡើយ។ សម្រាប់គម្រោង C# នៅលើ Windows បន្ថែមកញ្ចប់ `Microsoft.AI.Foundry.Local.WinML` NuGet ដើម្បីអាចដំណើរការ QNN execution provider — QNN ផ្តល់ជូនជា plugin EP តាមរយៈ WinML ។

---

### លំហាត់ 9៖ ការបន្ថែមម៉ូដែល និងបច្ចុប្បន្នភាពទិញបញ្ជីគំរូ

បញ្ជីម៉ូដែលត្រូវបានបង្កើតឡើងជាប្រចាំ។ ប្រើវិធីសាស្ត្រទាំងនេះដើម្បីពិនិត្យនិងអនុវត្តបច្ចុប្បន្នភាព។

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# ព្យួរសំលុះបញ្ជីដើម្បីទទួលបានបញ្ជីម៉ូដែលចុងក្រោយបំផុត
manager.refresh_catalog()

# ពិនិត្យមើលថាតើម៉ូដែលបានរក្សាទុកមានកំណែថ្មីជាងទេឬអត់
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

// បន្ទាន់សម័យតារាងដើម្បីទាញយកបញ្ជីម៉ូដែលថ្មីៗបំផុត
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// បញ្ជីម៉ូដែលដែលអាចប្រើបានទាំងអស់បісляការបន្ទាន់សម័យ
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### លំហាត់ 10៖ ដំណើរការជាមួយម៉ូដែល Reasoning

ម៉ូដែល **phi-4-mini-reasoning** មានគំនិតចងខ្សែ — វាចងក្នុងស្ទីល `<think>...</think>` មុនពេលលទ្ធផលចុងក្រោយ។ វាប្រយោជន៍សម្រាប់ភារកិច្ចដែលចាំបាច់ផ្នែកគំនិតច្រើនជំហាន គណិតវិទ្យា ឬដោះស្រាយបញ្ហា។

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning គឺប្រហែល 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# ម៉ូឌែលនេះជំរុញការគិតរបស់វាតាមស្លាក <think>...</think>
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

// វិភាគការគិតជា​សន្លឹក​ខ្សែ​សង្វាក់
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **ពេលណាត្រូវប្រើម៉ូដែល reasoning៖**
> - បញ្ហាគណិតវិទ្យា និងគំនិត
> - មុខងារសម្រាប់បាក់គំនិតជាច្រើនជំហាន
> - កូដស្មុគស្មាញ
> - ភារកិច្ចដែលការបង្ហាញដំណើរការជួយបង្កើនភាពត្រឹមត្រូវ
>
> **ការជួញដូរ៖** ម៉ូដែល reasoning បែងចែកដំណើរការបានច្រើនកូដ (ផ្នែក `<think>` ) និងយឺតជាង។ សម្រាប់សំណួរឆ្លើយតបដោយសាមញ្ញ ម៉ូដែលស្តង់ដារ ដូចជា phi-3.5-mini លឿនជាង។

---

### លំហាត់ 11៖ ការយល់ដឹងអំពីឈ្មោះបន្លំ Aliases និងជម្រើសឧបករណ៍

ពេលដែលអ្នកផ្ដល់ **alias** (ដូចជា `phi-3.5-mini`) ជំនួស ID ម៉ូដែលពេញលេញ SDK ជ្រើសកំណែវ៉ារីអង់ល្អបំផុតសម្រាប់ឧបករណ៍របស់អ្នក៖

| ឧបករណ៍ | កម្មវិធីដែលបានជ្រើសជាកម្មវិធីអនុវត្ត |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (តាមរយៈ plugin WinML) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| ឧបករណ៍ណាមួយ (fallback) | `CPUExecutionProvider` ឬ `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ឈ្មោះអថេរត្រូវបានដោះស្រាយទៅជាប្រភេទល្អបំផុតសម្រាប់រ៉ឺម៉ារបស់អ្នក
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **គន្លឹះ៖** ប្រើ aliases ទៀងទាត់នៅក្នុងកូដកម្មវិធីរបស់អ្នក។ ពេលដែលអ្នកចាត់ចែងទៅកាន់ម៉ាស៊ីនអ្នកប្រើ SDK ជ្រើសវ៉ារីអង់ល្អបំផុតនៅពេលរត់ — CUDA នៅលើ NVIDIA, QNN នៅលើ Qualcomm, CPU នៅកន្លែងផ្សេងៗ។

---

### លំហាត់ 12៖ ជម្រើសកំណត់ C# Configuration

ថ្នាក់ `Configuration` នៅក្នុង SDK C# ផ្ដល់នូវការគ្រប់គ្រងលម្អិតលើ runtime៖

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

| គន្លង | លំនាំដើម | សង្ខេប |
|----------|---------|-------------|
| `AppName` | (ចាំបាច់) | ឈ្មោះកម្មវិធីរបស់អ្នក |
| `LogLevel` | `Information` | កម្រិតការចេញកំណត់ហេតុ |
| `Web.Urls` | (ឌីណាមិច) | កំណត់ច្រកមួយឲ្យម៉ាស៊ីនបម្រើវេប |
| `AppDataDir` | លំនាំដើម OS | ថតបណ្តុំព័ត៌មានកម្មវិធី |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | ទីតាំងផ្ទុកម៉ូដែល |
| `LogsDir` | `{AppDataDir}/logs` | ទីតាំងផ្ទុកកំណត់ហេតុ |

---

### លំហាត់ 13៖ ការប្រើប្រាស់កម្មវិធី Web Browser (សម្រាប់ JavaScript តែប៉ុណ្ណោះ)

SDK JavaScript មានកំណែដែលគាំទ្រកម្មវិធីរុករក។ ក្នុងកម្មវិធីរុករក អ្នកត្រូវចាប់ផ្តើមសេវាកម្មតាម CLI ដោយដាក់ URL ម៉ាស៊ីនផ្ដល់សេវា៖

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// ចាប់ផ្ដើមសេវាកម្មដោយដៃសិន៖
//   ចាប់ផ្ដើមសេវាកម្ម foundry
// បន្ទាប់មកប្រើ URL ពីលទ្ធផល CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// ឆែកមើលបញ្ជីទំនិញ
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **កំណត់ត្រារុករក៖** កំណែរុករកមិនគាំទ្រការហៅ `startWebService()` ទេ។ អ្នកត្រូវប្រាកដថាសេវា Foundry Local បានដំណើរការមុនពេលប្រើ SDK នៅក្នុងកម្មវិធីរុករក។

---

## ឯកសារយោង API ពេញលេញ

### Python

| ប្រភេទ | វិធីសាស្ត្រ | សង្ខេប |
|----------|--------|-------------|
| **ចាប់ផ្តើម** | `FoundryLocalManager(alias?, bootstrap=True)` | បង្កើតអ្នកគ្រប់គ្រង; ជាជម្រើស bootstrap ជាមួយម៉ូដែល |
| **សេវា** | `is_service_running()` | ពិនិត្យថាសេវាកម្មកំពុងដំណើរការ |
| **សេវា** | `start_service()` | ចាប់ផ្តើមសេវាកម្ម |
| **សេវា** | `endpoint` | URL ចុងកំណត់ API |
| **សេវា** | `api_key` | សោ API |
| **ទិញបញ្ជីគំរូ** | `list_catalog_models()` | បញ្ជីម៉ូដែលទាំងអស់មាន |
| **ទិញបញ្ជីគំរូ** | `refresh_catalog()` | បច្ចុប្បន្នភាពបញ្ជីគំរូ |
| **ទិញបញ្ជីគំរូ** | `get_model_info(alias_or_model_id)` | ទទួលព័ត៌មានម៉ូដែល |
| **Cache** | `get_cache_location()` | បញ្ជីទីតាំងផ្ទុក cache |
| **Cache** | `list_cached_models()` | បញ្ជីម៉ូដែលបានទាញយក |
| **ម៉ូដែល** | `download_model(alias_or_model_id)` | ទាញយកម៉ូដែល |
| **ម៉ូដែល** | `load_model(alias_or_model_id, ttl=600)` | ផ្ទុកម៉ូដែល |
| **ម៉ូដែល** | `unload_model(alias_or_model_id)` | ដោះបង្ហាញម៉ូដែល |
| **ម៉ូដែល** | `list_loaded_models()` | បញ្ជីម៉ូដែលដែលបានផ្ទុក |
| **ម៉ូដែល** | `is_model_upgradeable(alias_or_model_id)` | ពិនិត្យថាមានកំណែថ្មីអាចបន្ថែមបានទេ |
| **ម៉ូដែល** | `upgrade_model(alias_or_model_id)` | បន្ថែមកំណែថ្មីសម្រាប់ម៉ូដែល |
| **សេវា** | `httpx_client` | អ្នកអតិថិជន HTTPX បង្កើតរួចសម្រាប់ហៅ API តូច |

### JavaScript

| ប្រភេទ | វិធីសាស្ត្រ | សង្ខេប |
|----------|--------|-------------|
| **ចាប់ផ្តើម** | `FoundryLocalManager.create(options)` | ចាប់ផ្តើម SDK ដូចជា singleton |
| **ចាប់ផ្តើម** | `FoundryLocalManager.instance` | ចូលទៅអ្នកគ្រប់គ្រង singleton |
| **សេវា** | `manager.startWebService()` | ចាប់ផ្តើមសេវាកម្មវេប |
| **សេវា** | `manager.urls` | អារេ URL មូលដ្ឋានសម្រាប់សេវាកម្ម |
| **ទិញបញ្ជីគំរូ** | `manager.catalog` | ចូលទៅក្នុងបញ្ជីម៉ូដែល |
| **ទិញបញ្ជីគំរូ** | `catalog.getModel(alias)` | ទទួលម៉ូដែលតាម alias (ត្រឡប់ Promise) |
| **ម៉ូដែល** | `model.isCached` | ពិនិត្យថាម៉ូដែលបានទាញយកហើយឬទេ |
| **ម៉ូដែល** | `model.download()` | ទាញយកម៉ូដែល |
| **ម៉ូដែល** | `model.load()` | ផ្ទុកម៉ូដែល |
| **ម៉ូដែល** | `model.unload()` | ដោះបង្ហាញម៉ូដែល |
| **ម៉ូដែល** | `model.id` | អត្ថាធិប្បាយម៉ូដែលតែមួយ |
| **ម៉ូដែល** | `model.alias` | ឈ្មោះបន្លំម៉ូដែល |
| **ម៉ូដែល** | `model.createChatClient()` | ទទួល chat client ដើមដំណើរការ (មិនចាំបាច់ OpenAI SDK) |
| **ម៉ូដែល** | `model.createAudioClient()` | ទទួល client សំរាប់បំលែងសំឡេង |
| **ម៉ូដែល** | `model.removeFromCache()` | លុបម៉ូដែលក្នុង cache ទីតាំងស្រុក |
| **ម៉ូដែល** | `model.selectVariant(variant)` | ជ្រើសរើសវ៉ារីអង់ឧបករណ៍ |
| **ម៉ូដែល** | `model.variants` | អារេវ៉ារីអង់ដែលមានសម្រាប់ម៉ូដែលនេះ |
| **ម៉ូដែល** | `model.isLoaded()` | ពិនិត្យថាម៉ូដែលបានផ្ទុកហើយឬទេ |
| **ទិញបញ្ជីគំរូ** | `catalog.getModels()` | បញ្ជីម៉ូដែលទាំងអស់មាន |
| **ទិញបញ្ជីគំរូ** | `catalog.getCachedModels()` | បញ្ជីម៉ូដែលបានទាញយក |
| **ទិញបញ្ជីគំរូ** | `catalog.getLoadedModels()` | បញ្ជីម៉ូដែលដែលបានផ្ទុក |
| **ទិញបញ្ជីគំរូ** | `catalog.updateModels()` | បច្ចុប្បន្នភាពបញ្ជីពីសេវាកម្ម |
| **សេវា** | `manager.stopWebService()` | បញ្ឈប់សេវាកម្ម Foundry Local វេប |

### C# (v0.8.0+)

| ប្រភេទ | វិធីសាស្ត្រ | សង្ខេប |
|----------|--------|-------------|
| **ចាប់ផ្តើម** | `FoundryLocalManager.CreateAsync(config, logger)` | ចាប់ផ្តើមអ្នកគ្រប់គ្រង |
| **ចាប់ផ្តើម** | `FoundryLocalManager.Instance` | ចូលទៅ singleton |
| **ទិញបញ្ជីគំរូ** | `manager.GetCatalogAsync()` | ទទួលបញ្ជីគំរូ |
| **ទិញបញ្ជីគំរូ** | `catalog.ListModelsAsync()` | បញ្ជីម៉ូដែលទាំងអស់មាន |
| **ទិញបញ្ជីគំរូ** | `catalog.GetModelAsync(alias)` | ទទួលម៉ូដែលជាក់លាក់ |
| **ទិញបញ្ជីគំរូ** | `catalog.GetCachedModelsAsync()` | បញ្ជីម៉ូដែលបានទាញយក |
| **ទិញបញ្ជីគំរូ** | `catalog.GetLoadedModelsAsync()` | បញ្ជីម៉ូដែលដែលបានផ្ទុក |
| **ម៉ូដែល** | `model.DownloadAsync(progress?)` | ទាញយកម៉ូដែល |
| **ម៉ូដែល** | `model.LoadAsync()` | ផ្ទុកម៉ូដែល |
| **ម៉ូដែល** | `model.UnloadAsync()` | ដោះបង្ហាញម៉ូដែល |
| **ម៉ូដែល** | `model.SelectVariant(variant)` | ជ្រើសវ៉ារីអង់ឧបករណ៍ |
| **ម៉ូដែល** | `model.GetChatClientAsync()` | ទទួល chat client ដើមដំណើរការ |
| **ម៉ូដែល** | `model.GetAudioClientAsync()` | ទទួល client transcription សំឡេង |
| **ម៉ូដែល** | `model.GetPathAsync()` | ទទួលផ្លូវទៅឯកសារស្រុក |
| **ទិញបញ្ជីគំរូ** | `catalog.GetModelVariantAsync(alias, variant)` | ទទួលវ៉ារីអង់ឧបករណ៍ជាក់លាក់ |
| **ទិញបញ្ជីគំរូ** | `catalog.UpdateModelsAsync()` | បច្ចុប្បន្នភាពបញ្ជីគំរូ |
| **ម៉ាស៊ីនបម្រើ** | `manager.StartWebServerAsync()` | ចាប់ផ្តើមម៉ាស៊ីនបម្រើវេប REST |
| **ម៉ាស៊ីនបម្រើ** | `manager.StopWebServerAsync()` | បញ្ឈប់ម៉ាស៊ីនបម្រើវេប REST |
| **កំណត់ការ** | `config.ModelCacheDir` | ទីតាំង cache |

---

## ចំណាំ​សំខាន់ៗ

| ទ្រឹស្តី | អ្វីដែលអ្នកបានរៀន |
|---------|-----------------|
| **SDK ទល់នឹង CLI** | SDK ផ្ដល់ការគ្រប់គ្រងកម្មវិធី — ចាំបាច់សម្រាប់កម្មវិធី |
| **គ្រប់គ្រងបេសកកម្ម** | SDK គ្រប់គ្រងសេវា ម៉ូដែល និង cache |
| **ច្រកឌីណាមិច** | ប្រើ `manager.endpoint` (Python) ឬ `manager.urls[0]` (JS/C#) តែងតែ មិនគួរផ្តល់លេខច្រកដោយដៃ |
| **ឈ្មោះបន្លំ aliases** | ប្រើ aliases សម្រាប់ជ្រើសវ៉ារីអង់ម៉ូដែលដ៏មានប្រសិទ្ធភាពអ្វីដែលឧបករណ៍មាន |
| **ចាប់ផ្តើមរហ័ស** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **ការរចនា​ឡើងវិញ C#** | កំណែ v0.8.0+ មានការ​ផ្ទុក​ក្នុង​ខ្លួនឯង - មិនចាំបាច់មាន CLI លើគ្រឿងកុំព្យូទ័រអ្នកប្រើ​ប្រាស់ចុងក្រោយ |
| **មូលដ្ឋានជីវិតម៉ូដែល** | កាតាឡុក → ទាញយក → បញ្ចូល → ប្រើប្រាស់ → បញ្ចេញ |
| **FoundryModelInfo** | ទិន្នន័យប្រភេទធំនៅលើ: ការងារ, ឧបករណ៍, ទំហំ, អាជ្ញាបណ្ណ, ការអនុគ្រឿងឧបករណ៍ហៅ |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) សម្រាប់ប្រើប្រាស់ដោយគ្មាន OpenAI |
| **ប្រភេទខុសគ្នា** | ម៉ូដែលមានប្រភេទខុសគ្នាដែលជាប់ hardware (CPU, GPU, NPU); ជ្រើសរើសដោយស្វ័យប្រវត្តិ |
| **ធ្វើឱ្យប្រសើរឡើង** | Python: `is_model_upgradeable()` + `upgrade_model()` ដើម្បីរក្សាម៉ូដែលឱ្យទាន់សម័យ |
| **ធ្វើ​ថ្មី​កាតាឡុក** | `refresh_catalog()` (Python) / `updateModels()` (JS) ដើម្បី​ស្វែងរកម៉ូដែល​ថ្មីៗ |

---

## អ្នកធ្វើវិធី

| អ្នកធ្វើវិធី | តំណ |
|--------------|------|
| ឯកសារយោង SDK (ភាសាទាំងអស់) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| បញ្ចូលជាមួយ SDK សន្និដ្ឋាន | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| ឯកសារយោង API C# SDK | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| គំរូ C# SDK | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| គេហទំព័រ Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## ជំហានបន្ទាប់

បន្តទៅ [ផ្នែក 3៖ ការប្រើ SDK ជាមួយ OpenAI](part3-sdk-and-apis.md) ដើម្បីភ្ជាប់ SDK ទៅបណ្ណាល័យ OpenAI client និងសាងសង់កម្មវិធីបញ្ចប់ផ្ញើសារជាញឹកញាប់ដំបូងរបស់អ្នក។