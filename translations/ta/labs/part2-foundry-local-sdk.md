![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# பகுதி 2: Foundry Local SDK ஆழமான பகுப்பாய்வு

> **நோக்கம்:** Foundry Local SDK ஐ நிரலைக் கொண்டு மாதிரிகள், சேவைகள் மற்றும் கேச்சை நிர்வகிப்பதில் திறமை பெறுதல் - மற்றும் செயலிகளை உருவாக்க CLIஐவிட SDKஐ பரிந்துரைக்கும் காரணத்தை புரிந்து கொள்வது.

## கண்ணோட்டம்

பகுதி 1ல் நீங்கள் **Foundry Local CLI**யை பயன்படுத்தி மாதிரிகளை பதிவிறக்கம் செய்து தொடர்பாடிய முறையில் இயங்க வைத்தீர்கள். CLI ஆராய்ச்சிக்கு சிறந்தது, ஆனால் நீங்கள் உண்மையான செயலிகளை உருவாக்கும்போது ** நிரலைக் கொண்டு இயக்குதல்** தேவை. Foundry Local SDK இதை வழங்குகிறது - இது **கட்டுப்பாட்டு தளம்** (சேவை துவக்கம், மாதிரிகள் கண்டுபிடித்தல், பதிவிறக்கம், ஏற்றல்) நிர்வகிக்கிறது, உங்கள் செயலி குறியீடு **தரவு தளத்தில்** (கோரிக்கைகள் அனுப்புதல், முடிவுகள் பெறுதல்) கவனம் செலுத்தலாம்.

இந்த பயிற்சி Python, JavaScript மற்றும் C#ல் முழு SDK API பரப்பை கற்பிக்கிறது. கடைசியில் நீங்கள் அனைத்து முறைகளையும் புரிந்து கொண்டு எப்போது எதனை பயன்படுத்துவது என்பதை அறிந்துகொள்வீர்கள்.

## கற்றல் நோக்கங்கள்

இந்த பயிற்சியின் இறுதியில் நீங்கள் செய்யக்கூடியவை:

- SDK இதை CLIவுக்கு மேலாக செயலி மேம்பாட்டில் ஏன் முன்னுரிமை அளிக்கப்படுகிறது என்று விளக்கம்
- Python, JavaScript அல்லது C#க்கு Foundry Local SDK நிறுவுதல்
- சேவை துவக்க, மாதிரிகளை நிர்வகிக்கும் மற்றும் தொகுப்பை விசாரிப்பதில் `FoundryLocalManager` ஐப் பயன்படுத்துதல்
- நிரலைக் கொண்டு மாதிரிகளை பட்டியலிடுதல், பதிவிறக்கம் செய்தல், ஏற்றுதல் மற்றும் வெளியேற்றுதல்
- `FoundryModelInfo` பயன் படுத்தி மாதிரி மெட்டாடேட்டாவை ஆய்வு செய்தல்
- தொகுப்பு, கேசு, ஏற்றப்பட்ட மாதிரிகளுக்கு இடையேயான வேறுபாடுகளை புரிதல்
- கட்டமைப்பாளர்.bootstrap (Python) மற்றும் `create()` + தொகுப்பு வடிவத்தை (JavaScript) பயன்படுத்துதல்
- C# SDK மறுநகர்ந்து அதன் பொருள் சார்ந்த APIஐ புரிந்து கொள்வது

---

## தேவைகள்

| தேவையானது | விவரங்கள் |
|-------------|---------|
| **Foundry Local CLI** | நிறுவப்பட்டு உங்கள் `PATH`இல் இருக்க வேண்டும் ([பகுதி 1](part1-getting-started.md)) |
| **மொழி ரன்டைம்** | **Python 3.9+** மற்றும்/அல்லது **Node.js 18+** மற்றும்/அல்லது **.NET 9.0+** |

---

## கருத்து: SDK vs CLI - SDKஐ ஏன் பயன்படுத்த வேண்டும்?

| அம்சம் | CLI (`foundry` கமாண்ட்) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **பயன்பாடு** | ஆராய்ச்சி, கையேடு சோதனை | செயலி ஒருங்கிணைப்பு |
| **சேவை நிர்வகம்** | கையேடு: `foundry service start` | தானாக: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **போர்ட் கண்டுபிடிப்பு** | CLI வெளியீட்டில் இருந்து வாசித்தல் | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **மாதிரி பதிவிறக்கம்** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **பிழை கையாளல்** | வெளியேற்றிய குறியீடுகள், stderr | விதிகள், வகைப்படுத்தப்பட்ட பிழைகள் |
| **தானாக இயங்குதல்** | ஷெல் ஸ்கிரிப்டுகள் | இயல்புமொழி ஒருங்கிணைப்பு |
| **பதிப்பு** | இறுதி பயனர் இயந்திரத்தில் CLI தேவை | C# SDK சுயாதீனம் (CLI தேவையில்லை) |

> **முக்கிய கண்ணோட்டம்:** SDK சேவையை துவக்குதல், கேசைக் சரிபார்த்தல், காணாமல் போன மாதிரிகளை பதிவிறக்கம் செய்தல், ஏற்றுதல் மற்றும் உடன் சேவை முகவரியை கண்டுபிடித்தல் போன்ற முழு வாழ்க்கைச்சுழற்சியையும் சில வரி குறியீடு கொண்டு கையாள்கிறது. உங்கள் செயலி CLI வெளியீடுகளை பகுப்பாய்வு செய்யவோ சாதகமாக subprocess களை நிர்வகிக்கவோ தேவையில்லை.

---

## பயிற்சி பயிற்சிகள்

### பயிற்சி 1: SDK நிறுவுதல்

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```
  
நிறுவலை சரிபார்க்கவும்:

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
  
நிறுவலை சரிபார்க்கவும்:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

இரு NuGet தொகுதிகள் உள்ளன:

| தொகுதி | தளம் | விளக்கம் |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | பன்முக தளம் | Windows, Linux, macOSக்கு வேலை செய்கிறது |
| `Microsoft.AI.Foundry.Local.WinML` | Windows மட்டும் | WinML இயந்திர வேகமைப்பை சேர்க்கிறது; இணைப்புக் செயலிகளையும் பதிவிறக்குகிறது (உதா: Qualcomm NPUக்கு QNN) |

**Windows அமைப்பு:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```
  
`.csproj` கோப்பை திருத்தவும்:

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
  
> **க் கவனிப்பு:** Windows இல், WinML தொகுதி SDK அடிப்படையை கூட எடுத்துக் கொண்டுள்ளது மற்றும் QNN செயலி வழங்குனரை கொண்டுள்ளது. Linux/macOS இல் அடிப்படை SDK பயன்படுத்தப்படுகிறது. பகுப்பாய்வு TFM மற்றும் தொகுதி குறிப்பு திட்டத்தை பன்முக தளமாக வைத்திருக்க உதவும்.

திட்ட மூலத்தில் `nuget.config` உருவாக்கவும்:

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
  
தொகுதிகளை மீட்டெடுக்கவும்:

```bash
dotnet restore
```

</details>

---

### பயிற்சி 2: சேவை துவக்கம் மற்றும் தொகுப்பு பட்டியல்

ஒவ்வொரு செயலியும் முதலில் Foundry Local சேவையை துவக்கி எத்தனை மாதிரிகள் உள்ளன என்பதை கண்டுபிடிக்கிறது.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# ஒரு மேலாளரை உருவாக்கி சேவையை ஆரம்பிக்கவும்
manager = FoundryLocalManager()
manager.start_service()

# காட்சியகத்தில் உள்ள அனைத்து மாடல்களையும் பட்டியலிடவும்
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```
  
#### Python SDK - சேவை நிர்வாக முறைகள்

| முறை | கையொப்பம் | விளக்கம் |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | சேவை இயங்கி வருகிறதா என்பதைச் சரிபார்க்கவும் |
| `start_service()` | `() -> None` | Foundry Local சேவையை துவங்கு |
| `service_uri` | `@property -> str` | அடிப்படை சேவை URI |
| `endpoint` | `@property -> str` | API முகவரி (சேவை URI + `/v1`) |
| `api_key` | `@property -> str` | API விசை (சூழல்variablesஎல்லாம் இருந்து அல்லது இயல்புநிலை) |

#### Python SDK - தொகுப்பு நிர்வாக முறைகள்

| முறை | கையொப்பம் | விளக்கம் |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | தொகுப்பில் உள்ள அனைத்து மாதிரிகளையும் பட்டியலிடுக |
| `refresh_catalog()` | `() -> None` | சேவையிலிருந்து தொகுப்பை புதுப்பிக்கவும் |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | குறிப்பிட்ட மாதிரியின் தகவலைப் பெறுக |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ஒரு மேலாளரை உருவாக்கி சேவையை துவங்கவும்
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// பட்டியலை உலாவவும்
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```
  
#### JavaScript SDK - மேலாளர் முறைகள்

| முறை | கையொப்பம் | விளக்கம் |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK ஒற்றை உருப்படியை துவக்கவும் |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | ஒற்றை உருப்படியை அணுகவும் |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local வலை சேவையை துவங்கவும் |
| `manager.urls` | `string[]` | சேவை அடிப்படை URLகளின் வரிசை |

#### JavaScript SDK - தொகுப்பு மற்றும் மாதிரி முறைகள்

| முறை | கையொப்பம் | விளக்கம் |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | மாதிரி தொகுப்பை அணுகவும் |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | குறிப்பிட்ட அ்லைஸ் மூலம் மாதிரியைப் பெறுக |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK பதிப்பு 0.8.0+ `Configuration`, `Catalog`, மற்றும் `Model` பொருட்களைப் பயன்படுத்தி பொருள் மையமான கட்டமைப்பைப் பயன்படுத்துகிறது:

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
  
#### C# SDK - முக்கிய வகுப்புகள்

| வகுப்பு | நோக்கம் |
|-------|---------|
| `Configuration` | செயலி பெயர், பதிவு நிலை, கேச் அடைவை, வலை சேவை URLகளை அமைக்கவும் |
| `FoundryLocalManager` | முக்கிய நுழைவு - `CreateAsync()` மூலம் உருவாக்கம், `.Instance` மூலம் அணுகல் |
| `Catalog` | தொகுப்பில் இருந்து மாதிரிகளை உலாவல், தேடல் மற்றும் பெறுதல் |
| `Model` | குறிப்பிட்ட மாதிரியை பிரதிநிதித்துவம் செய்கிறது - பதிவிறக்கம், ஏற்றுதல், கிளையண்டுகளைப் பெறுதல் |

#### C# SDK - மேலாளரும் தொகுப்பும் முறைகள்

| முறை | விளக்கம் |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | மேலாளரை தொடங்குக |
| `FoundryLocalManager.Instance` | ஒற்றை மேலாளரை அணுகுக |
| `manager.GetCatalogAsync()` | மாதிரி தொகுப்பைப் பெறுக |
| `catalog.ListModelsAsync()` | அனைத்து கிடைக்கும் மாதிரிகளையும் பட்டியலிடுக |
| `catalog.GetModelAsync(alias: "alias")` | குறிப்பிட்ட அலைஸ்டின் மூலம் மாதிரியைப் பெறுக |
| `catalog.GetCachedModelsAsync()` | பதிவிறக்கம் செய்யப்பட்ட மாதிரிகளின் பட்டியல் |
| `catalog.GetLoadedModelsAsync()` | தற்போது ஏற்றப்பட்ட மாதிரிகளின் பட்டியல் |

> **C# கட்டமைப்பு குறிப்பு:** C# SDK பதிப்பு 0.8.0+ செயலியை **சுயமேற்பணியாளராக** மாற்றுகிறது; இறுதி பயனர் இயந்திரத்தில் Foundry Local CLI தேவை இல்லை. SDK மாதிரி நிர்வகிப்பு மற்றும் முன்னறிவிப்பை இயல்பாகக் கையாள்கிறது.

</details>

---

### பயிற்சி 3: மாதிரியை பதிவிறக்கம் செய்து ஏற்றுதல்

SDK, பதிவிறக்கம் செய்தல் (வட்டிவடிவில்) மற்றும் ஏற்றுதல் (நினைவகத்தில்) ஆகியவற்றை பிரிக்கிறது. இதனால் நீங்கள் முன்பே மாதிரிகளை setup நேரத்தில் பதிவிறக்கம் செய்து தேவையான சமயத்தில் ஏற்ற முடியும்.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# விருப்பம் A: கைமுறை படி படியாக
manager = FoundryLocalManager()
manager.start_service()

# முதலில் கேச் சரிபார்க்கவும்
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

# விருப்பம் B: ஒரே வரி புட்டூஸ்ட் (பரிந்துரைக்கப்பட்டது)
# ஆலியாஸ்-ஐ கட்டியமைப்பாளருக்கு வழங்கவும் - இது சேவையை தொடங்கி, தரவிறக்கம் செய்து, தானாக ஏற்றுகிறது
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```
  
#### Python - மாதிரி நிர்வாக முறைகள்

| முறை | கையொப்பம் | விளக்கம் |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | ஒரு மாதிரியை உள்ளக கேசுக்கு பதிவிறக்கம் செய்தல் |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | மாதிரியை கணிப்பு சேவையகத்தில் ஏற்றுதல் |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | சேவையிடத்தில் இருந்து மாதிரியை வெளியேற்றல் |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | தற்போது ஏற்றப்பட்ட மாதிரிகளைக் பட்டியலிடுக |

#### Python - கேச் நிர்வாக முறைகள்

| முறை | கையொப்பம் | விளக்கம் |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | கேச் அடைவு பாதையைப் பெறுக |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | அனைத்து பதிவிறக்கம் செய்யப்பட்ட மாதிரிகளைக் பட்டியலிடுக |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// படி படியாக அணுகுமுறை
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
  
#### JavaScript - மாதிரி முறைகள்

| முறை | கையொப்பம் | விளக்கம் |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | மாதிரி ஏற்கனவே பதிவிறக்கம் செய்யப்பட்டதா |
| `model.download()` | `() => Promise<void>` | மாதிரியை உள்ளக கேஷ் க்கு பதிவிறக்கம் செய்தல் |
| `model.load()` | `() => Promise<void>` | கணிப்பு சேவையகத்தில் ஏற்றுதல் |
| `model.unload()` | `() => Promise<void>` | கணிப்பு சேவையகத்தில் இருந்து வெளியேற்றல் |
| `model.id` | `string` | மாதிரியின் தனித்துவ அடையாளம் |

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
  
#### C# - மாதிரி முறைகள்

| முறை | விளக்கம் |
|--------|-------------|
| `model.DownloadAsync(progress?)` | தேர்ந்தெடுக்கப்பட்ட மாறுபாட்டைப் பதிவிறக்கம் செய்க |
| `model.LoadAsync()` | மாதிரியை நினைவகத்தில் ஏற்றுக |
| `model.UnloadAsync()` | மாதிரியை வெளியேறு |
| `model.SelectVariant(variant)` | குறிப்பிட்ட மாறுபாட்டை தேர்ந்தெடு (CPU/GPU/NPU) |
| `model.SelectedVariant` | தற்போதைய தேர்ந்தெடுக்கப்பட்ட மாறுபாடு |
| `model.Variants` | இந்த மாதிரிக்கான அனைத்து கிடைக்கும் மாறுபாடுகள் |
| `model.GetPathAsync()` | உள்ளக கோப்பு பாதையைப் பெறுக |
| `model.GetChatClientAsync()` | இயல்பு உரையாடல் கிளையண்டை பெறுக (OpenAI SDK தேவையில்லை) |
| `model.GetAudioClientAsync()` | பேச்சு மாற்றுமுறை கிளையண்டை பெறுக |

</details>

---

### பயிற்சி 4: மாதிரி மெட்டாடேட்டாவைப் பார்வையிடுக

`FoundryModelInfo` பொருள் ஒவ்வொரு மாதிரிக்கும் விரிவான மெட்டாடேட்டாவைக் கொண்டுள்ளது. இந்த புலங்களைப் புரிந்துகொள்வது உங்கள் செயலிக்கு சரியான மாதிரியை தேர்வு செய்ய உதவும்.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ஒரு குறிப்பிட்ட மாதிரி பற்றி விரிவான தகவலைப் பெறுக
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

#### FoundryModelInfo புலங்கள்

| புலம் | வகை | விளக்கம் |
|-------|------|-------------|
| `alias` | string | குறுகிய பெயர் (உதா: `phi-3.5-mini`) |
| `id` | string | தனிச்சிறப்பு மாதிரி அடையாளம் |
| `version` | string | மாதிரி பதிப்பு |
| `task` | string | `chat-completions` அல்லது `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, அல்லது NPU |
| `execution_provider` | string | இயக்கம் பின்னணி (CUDA, CPU, QNN, WebGPU, மேலும்) |
| `file_size_mb` | int | கோப்பு அளவு தொகுதியில் (MB) |
| `supports_tool_calling` | bool | செயலி/கருவி அழைப்பை ஆதரிப்பதா? |
| `publisher` | string | மாதிரி வெளியிட்டவர் |
| `license` | string | உரிமம் பெயர் |
| `uri` | string | மாதிரி URI |
| `prompt_template` | dict/null | ஏதேனும் இருந்தால், கோட்டு கைதரம் |

---

### பயிற்சி 5: மாதிரி வாழ்க்கைச்சுழற்சியை நிர்வகிக்கவும்

முழு வாழ்க்கைச்சுழற்சியைப் பயிற்சி செய்யவும்: பட்டியலிடு → பதிவிறக்கு → ஏற்று → பயன்படுத்த → வெளியேறு.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # விரைவு சோதனக்காக சிறிய மாதிரி

manager = FoundryLocalManager()
manager.start_service()

# 1. பட்டியலில் என்ன உள்ளது கண்டறியவும்
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. ஏற்கனவே பதிவிறக்கம் செய்யப்பட்டதை சோதிக்கவும்
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. ஒரு மாதிரியை பதிவிறக்கம் செய்யவும்
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. அது இப்போது காட்சேவில் உள்ளதா என உறுதிப்படுத்தவும்
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. அதைப் படையுங்கள்
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. ஏற்றப்பட்டதை சோதிக்கவும்
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. அதை அகற்றவும்
print(f"\nUnloading {alias}...")
manager.unload_model(alias)
loaded = manager.list_loaded_models()
print(f"Loaded models after unload: {len(loaded)}")
```
  
</details>

<details>
<summary><h3>📘 ஜாவாஸ்கிரிப்ட்</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "qwen2.5-0.5b"; // வேகமான சோதனைக்கான சிறியது மாதிரி

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. பட்டியலிலிருந்து மாதிரியை பெறு
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. தேவைப்பட்டால் பதிவிறக்கு
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. அதை ஏற்று
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. அதனை இறக்க
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### பயிற்சி 6: விரைவான தொடக்கம் முறைமைகள்

ஒவ்வொரு மொழியும் ஒரு அழைப்பில் சேவையை ஆரம்பித்து ஒரு மாதிரியை ஏற்ற ஒரு குறுகிய பாதையை வழங்குகிறது. இவை பெரும்பான்மையான பயன்பாடுகளுக்கு **பரிந்துரைக்கப்பட்ட முறைமைகள்** ஆகும்.

<details>
<summary><h3>🐍 பைதான் - கட்டமைப்பாளர் பூட்ஸ்ட்ராப்</h3></summary>

```python
from foundry_local import FoundryLocalManager

# கட்டமைப்பாளருக்கு ஒரு மாறுபெயரை அளிக்கவும் - இது அனைத்தையும் கையாள்கிறது:
# 1. சேவை இயக்கப்படவில்லை என்றால் துவங்குகிறது
# 2. காட்சிப்படுத்தப்படவில்லை என்றால் மாதிரியை பதிவிறக்கம் செய்க
# 3. மாதிரியை முடிவெடும் சேவையகத்தில் ஏற்றுகிறது
manager = FoundryLocalManager("phi-3.5-mini")

# உடனடியாக பயன்படுத்த தயாராக உள்ளது
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` அளவுரு (இயல்புநிலை `True`) இந்த நடத்தையை கட்டுப்படுத்துகிறது. கைமுறை கட்டுப்பாட்டை விரும்பினால் `bootstrap=False` என அமைக்கவும்:

```python
# கைமுறை முறையில் - எதுவும் தானாக நடைபெறாது
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 ஜாவாஸ்கிரிப்ட் - `create()` + பட்டியல்</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() எல்லாவற்றையும் கையாள்கிறது:
// 1. சேவையைத் தொடங்குகிறது
// 2. அடுக்குமுறையிலிருந்து மாதிரியை பெறுகிறது
// 3. தேவையாயின் பதிவிறக்கம் செய்து மாதிரியை ஏற்றுகிறது
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// உடனடியாக பயன்படுத்த தயாராக உள்ளது
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + பட்டியல்</h3></summary>

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

> **C# குறிப்பு:** C# SDK பயன்பாட்டின் பெயர், பதிவு, கேஷ் கோப்புறைகள் மற்றும் குறிப்பிட்ட வெப் சர்வர் போர்ட்டை பின்சுட்டுவதற்காக `Configuration` ஐப் பலவகைகளிலும் கட்டுப்படுத்த பயன்படுத்துகிறது. இது மூன்று SDK-களில் மிகவும் அமைப்பிைமானதாகும்.

</details>

---

### பயிற்சி 7: சொந்த ChatClient (OpenAI SDK தேவையில்லை)

ஜாவாஸ்கிரிப்ட் மற்றும் C# SDKகள் ஒரு பொதுவான சொந்தச் சொடுகருத்துக் கிளையண்டை அளிக்கும் `createChatClient()` வசதியை வழங்குகின்றன — OpenAI SDK தனியே நிறுவலோ அல்லது அமைப்போ தேவையில்லை.

<details>
<summary><h3>📘 ஜாவாஸ்கிரிப்ட் - <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// மாடலில் இருந்து நேரடியாக ChatClient உருவாக்கவும் — OpenAI இறக்குமதி தேவையில்லை
const chatClient = model.createChatClient();

// completeChat ஒரு OpenAI-போன்ற பதில் பொருளை வழங்குகிறது
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// ஸ்ட்ரீமிங் கால்‌بேக் முறையை பயன்படுத்துகிறது
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` கூடுதலாக கருவிகளைக் கூற்று ஆதரிக்கிறது — இரண்டாவது அளவுருவாக கருவிகளை அனுப்பவும்:

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

> **எப்போது எந்த முறைமையைப் பயன்படுத்த வேண்டும்:**
> - **`createChatClient()`** — விரைவு மாதிரிப் பரிசோதனை, குறைந்த சார்பு, எளிய குறியீடு
> - **OpenAI SDK** — அளவுருக்கள் (வெப்பநிலை, top_p, நிறுத்தக் குறிகள், போன்றவை) முழுமையான கட்டுப்பாடு, தயாரிப்பு பயன்பாடுகளுக்கு சிறந்தவை

---

### பயிற்சி 8: மாதிரி வகைகள் மற்றும் ஹார்ட்வேயர் தேர்வு

மாதிரிகள் பல **வகைகள்** கொண்டிருக்கும், அவை வெவ்வேறு ஹார்ட்வேருக்கு சிறப்பாக மையமாக்கப்பட்டவை. SDK தானாக சிறந்த வகையைத் தேர்வு செய்கிறது, மேலும் நீங்கள் நேர்மறையாகப் பார்க்கவும் தேர்வு செய்யவும் முடியும்.

<details>
<summary><h3>📘 ஜாவாஸ்கிரிப்ட்</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// அனைத்து கிடைக்கும் வகைகளையும் பட்டியலிடு
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// உங்கள் ஹார்ட்வேர்‌க்கு ஏற்ற சிறந்த வகையை SDK தானாக தேர்வு செய்கிறது
// மாற்ற விரும்பினால், selectVariant() பயன்படுத்தவும்:
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
<summary><h3>🐍 பைதான்</h3></summary>

பைதானில், SDK ஹார்ட்வேரின் அடிப்படையில் தானாக சிறந்த வகையைத் தேர்வு செய்கிறது. `get_model_info()` பயன்படுத்தி தேர்ந்தெடுக்கப்பட்டதைப் பார்க்கவும்:

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

#### NPU வகைகள் கொண்ட மாதிரிகள்

சில மாதிரிகள் Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra) கொண்ட சாதனங்களுக்கு NPU-அமைப்பான வகைகளை கொண்டுள்ளன:

| மாதிரி | NPU வகை உள்ளது |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **அறிவுரை:** NPU-சாதனத்தில் SDK தானாகவே NPU வகையை தேர்வு செய்யும். நீங்கள் உங்கள் குறியீடுகளை மாற்ற தேவையில்லை. Windows இல் C# திட்டங்களுக்கு, QNN செயல்பாட்டு வழங்குநரை இயக்கு `Microsoft.AI.Foundry.Local.WinML` NuGet பாக்கேஜ் சேர்க்கவும் — QNN WinML வழியாக ஒரு சிறப்பி EP ஆக வழங்கப்படுகின்றது.

---

### பயிற்சி 9: மாதிரி மேம்படுத்தல்கள் மற்றும் பட்டியல் புதுப்பிப்பு

மாதிரி பட்டியல் காலத்திற்கு மேற்படி புதுப்பிக்கப்படும். புதுப்பிப்புகளை சரிபார்க்கவும் மற்றும் நடைமுறைப்படுத்தவும் இந்த வழிமுறைகளைப் பயன்படுத்தவும்.

<details>
<summary><h3>🐍 பைதான்</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# சமீபத்திய மாதிரி பட்டியலைப் பெற அடைகையைக் புதுப்பிக்கவும்
manager.refresh_catalog()

# சேமிக்கப்பட்ட மாதிரியில் புதிய பதிப்பு கிடைக்குமா என்று சரிபார்க்கவும்
if manager.is_model_upgradeable(alias):
    print(f"{alias} has a newer version available!")
    manager.upgrade_model(alias)
    print("Upgrade complete")
else:
    print(f"{alias} is up to date")
```

</details>

<details>
<summary><h3>📘 ஜாவாஸ்கிரிப்ட்</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "UpgradeDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// சமீபத்திய மாதிரி பட்டியலை பெறக் காட்சிப்படுத்தவும்
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// புதுப்பிக்கப்பட்ட பிறகு அனைத்து கிடைக்கும் மாதிரிகளையும் பட்டியலிடவும்
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### பயிற்சி 10: காரணம் மாதிரிகளுடன் வேலை செய்வது

**phi-4-mini-reasoning** மாதிரி சங்கிலித் தந்தை காரணத் திறன் கொண்டது. அதன் உட்புற சிந்தனையை `<think>...</think>` குறிச்சொற்களில் சுற்றி இறுதி பதிலை வழங்குகிறது. இது பல படி காரணித்திறன், கணக்கு அல்லது பிரச்சனை தீர்க்கும் பணிகளுக்கு பயனுள்ளதாகும்.

<details>
<summary><h3>🐍 பைதான்</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning என்பது சுமார் 4.6 GB ஆகும்
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# மாதிரி அதன் சிந்தனையை <think>...</think> குறிச்சொற்களில் இடுக்கிறது
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
<summary><h3>📘 ஜாவாஸ்கிரிப்ட்</h3></summary>

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

// கோர்டு-சிந்தனை சிந்தனையை பகுப்பாய்வு செய்க
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **எப்போது காரணித் திறனுடைய மாதிரிகளைப் பயன்படுத்துவது:**
> - கணித மற்றும் தர்க்கப் பிரச்சனைகள்
> - பல படி திட்டமிடல் பணிகள்
> - சிக்கலான குறியீடு உருவாக்கல்
> - பணியின் விளக்கத்தால் கூர்மையான பயிற்சி மேம்படுத்தப்படுகிறது
>
> **தள்ளுபடி:** காரணித் திறன் மாதிரிகள் அதிகமான குறியீடுகளை (குறிப்பாக `<think>` பகுதி) உருவாக்குகின்றன மற்றும் மெதுவாக இருக்கின்றன. எளிய கேள்வி-பதில் செயலுக்கு phi-3.5-mini மாதிரி விரைவாக இருக்கும்.

---

### பயிற்சி 11: பெயர்ப்பெயரையும் ஹார்ட்வேரும் தேர்வு செய்வது

நீங்கள் முழு மாதிரி ஐடியின் பதிலாக **பெயர்ப்பெயர்** (எ.கா., `phi-3.5-mini`) வழங்கும் போது SDK தானாக உங்கள் ஹார்ட்வேருக்கேற்ற சிறந்த வகையைத் தேர்வு செய்கிறது:

| ஹார்ட்வேர் | தேர்ந்தெடுக்கப்பட்ட செயல்படுத்தும் வழங்குநர் |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML சிறப்பி வழியாக) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| எந்த சாதனம் (இயக்கு வழி) | `CPUExecutionProvider` அல்லது `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# அந்த அலைஸ் உங்களுடைய ஹார்டுவேருக்கான சிறந்த வகையை தீர்க்கிறது
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **அறிவுரை:** உங்கள் பயன்பாட்டுக் குறியீட்டில் எப்போதும் பெயர்ப்பெயர்களைப் பயன்படுத்தவும். பயனரின் கணினியில் செயல்படுத்தும் போது, SDK இயங்கு நேரத்தில் சிறந்த வகையைத் தேர்வு செய்கிறது - NVIDIA இல் CUDA, Qualcomm இல் QNN, மற்றவை CPU.

---

### பயிற்சி 12: C# அமைப்பு விருப்பங்கள்

C# SDK இன் `Configuration` வகுப்பு இயக்க நேரத்தில் மெல்லிய கட்டுப்பாட்டை வழங்குகிறது:

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

| சொத்து | இயல்புநிலை | விளக்கம் |
|----------|---------|-------------|
| `AppName` | (தேவையானது) | உங்கள் பயன்பாட்டின் பெயர் |
| `LogLevel` | `Information` | பதிவு விவரக்குறிப்பு |
| `Web.Urls` | (மாறுமானது) | வெப் சர்வருக்காக குறிப்பிட்ட போர்ட்டை பின்சுட்டவும் |
| `AppDataDir` | OS இயல்புநிலை | பயன்பாட்டு தரவு அடிப்படை கோப்புறை |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | மாதிரிகள் சேமிக்கப்படும் இடம் |
| `LogsDir` | `{AppDataDir}/logs` | பதிவுகள் எழுதப்படும் இடம் |

---

### பயிற்சி 13: உலாவி பயன்பாடு (ஜாவாஸ்கிரிப்ட் மட்டும்)

ஜாவாஸ்கிரிப்ட் SDK உலாவிக்கான பொருத்தமான பதிப்பையும் உள்ளடக்கியது. உலாவியில், நீங்கள் கைமுறை CLI வழியாக சேவையை துவங்கி ஹோஸ்ட் URL ஐ குறிப்பிட வேண்டும்:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// முதலில் சேவையை கையேடாக துவங்கவும்:
//   foundry சேவை துவக்கு
// பின்னர் CLI வெளியீடிலிருந்து URL ஐ பயன்படுத்தவும்
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// காடலை உலாவவும்
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **உலாவி கட்டுப்பாடுகள்:** உலாவி பதிப்பு `startWebService()` ஐ ஆதரிக்கவில்லை. உலாவியில் SDK ஐப் பயன்படுத்தும் முன் Foundry Local சேவை ஏற்கனவே இயங்கிவிருக்க வேண்டும்.

---

## முழுமையான API குறிப்பு

### பைதான்

| வகை | முறை | விளக்கம் |
|----------|--------|-------------|
| **துவக்கம்** | `FoundryLocalManager(alias?, bootstrap=True)` | மேலாளரை உருவாக்கவும்; விருப்பமாய் மாதிரிக்கு பூட்ஸ்ட்ராப் செய்யவும் |
| **சேவை** | `is_service_running()` | சேவை இயங்குகிறதா என சரிபார்க்கவும் |
| **சேவை** | `start_service()` | சேவையை துவங்கவும் |
| **சேவை** | `endpoint` | API முடிவு URL |
| **சேவை** | `api_key` | API விசை |
| **பட்டியல்** | `list_catalog_models()` | உள்ள அனைத்து மாதிரிகளையும் பட்டியலிடவும் |
| **பட்டியல்** | `refresh_catalog()` | பட்டியலை புதுப்பிக்கவும் |
| **பட்டியல்** | `get_model_info(alias_or_model_id)` | மாதிரி தகவல்களைப் பெறவும் |
| **கேஷ்** | `get_cache_location()` | கேஷ் கோப்புறை பாதை |
| **கேஷ்** | `list_cached_models()` | பதிவிறக்கம் செய்யப்பட்ட மாதிரிகள் பட்டியல் |
| **மாதிரி** | `download_model(alias_or_model_id)` | மாதிரியை பதிவிறக்கம் செய்க |
| **மாதிரி** | `load_model(alias_or_model_id, ttl=600)` | மாதிரியை ஏற்றவும் |
| **மாதிரி** | `unload_model(alias_or_model_id)` | மாதிரியை ஏற்றலை எழுப்பவும் |
| **மாதிரி** | `list_loaded_models()` | ஏற்றப்பட்ட மாதிரிகள் பட்டியல் |
| **மாதிரி** | `is_model_upgradeable(alias_or_model_id)` | புதிய பதிப்பு உள்ளதா என சரி பார்க |
| **மாதிரி** | `upgrade_model(alias_or_model_id)` | மாதிரியை புதிய பதிப்பிற்குக் மேம்படுத்து |
| **சேவை** | `httpx_client` | நேரடி API அழைக்க முன்னமைக்கப்பட்ட HTTPX கிளையண்ட் |

### ஜாவாஸ்கிரிப்ட்

| வகை | முறை | விளக்கம் |
|----------|--------|-------------|
| **துவக்கம்** | `FoundryLocalManager.create(options)` | SDK ஒற்றை நகலை ஆரம்பிக்கவு |
| **துவக்கம்** | `FoundryLocalManager.instance` | ஒற்றை நகலை அணுகுக |
| **சேவை** | `manager.startWebService()` | வெப் சேவையை துவங்கவும் |
| **சேவை** | `manager.urls` | சேவைக்கான அடிப்படை URL-களின் பட்டியல் |
| **பட்டியல்** | `manager.catalog` | மாதிரி பட்டியலை அணுகவும் |
| **பட்டியல்** | `catalog.getModel(alias)` | பெயர்பெயரின் மூலம் மாதிரி பொருளைப் பெறவும் (வாக்குறுதி திருப்பும்) |
| **மாதிரி** | `model.isCached` | மாதிரி பதிவிறக்கம் செய்யப்பட்டதா என்பதை |
| **மாதிரி** | `model.download()` | மாதிரி பதிவிறக்கம் செய்யவும் |
| **மாதிரி** | `model.load()` | மாதிரியை ஏற்கவும் |
| **மாதிரி** | `model.unload()` | மாதிரியை ஏற்றலை செய்து விடவும் |
| **மாதிரி** | `model.id` | மாதிரியின் தனித்துவ அடையாளம் |
| **மாதிரி** | `model.alias` | மாதிரியின் பெயர்பெயர் |
| **மாதிரி** | `model.createChatClient()` | சொந்தச் சொடுகருத்துக் கிளையண்டைப் பெற (OpenAI SDK தேவையில்லை) |
| **மாதிரி** | `model.createAudioClient()` | ஒலி உருமாற்றக் கிளையண்டைப் பெறவும் |
| **மாதிரி** | `model.removeFromCache()` | மாதிரியை உள்ளக கேஷ் இலிருந்து அகற்று |
| **மாதிரி** | `model.selectVariant(variant)` | குறிப்பிட்ட ஹார்ட்வேர்க் வகையைத் தேர்வு செய்க |
| **மாதிரி** | `model.variants` | இந்த மாதிரிக்கான கிடைக்கும் வகைகளின் வரிசை |
| **மாதிரி** | `model.isLoaded()` | மாதிரி தற்போது ஏற்றப்பட்டதா என சரிபார்க்கவும் |
| **பட்டியல்** | `catalog.getModels()` | உள்ள அனைத்து மாதிரிகளையும் பட்டியலிடவும் |
| **பட்டியல்** | `catalog.getCachedModels()` | பதிவிறக்கம் செய்யப்பட்ட மாதிரிகள் பட்டியல் |
| **பட்டியல்** | `catalog.getLoadedModels()` | தற்போது ஏற்றப்பட்ட மாதிரிகள் பட்டியல் |
| **பட்டியல்** | `catalog.updateModels()` | சேவையிலிருந்து பட்டியலை புதுப்பி் |
| **சேவை** | `manager.stopWebService()` | Foundry Local வெப் சேவையை நிறுத்தவும் |

### C# (v0.8.0+)

| வகை | முறை | விளக்கம் |
|----------|--------|-------------|
| **துவக்கம்** | `FoundryLocalManager.CreateAsync(config, logger)` | மேலாளரை ஆரம்பிக்கவும் |
| **துவக்கம்** | `FoundryLocalManager.Instance` | ஒற்றை நகலை அணுகவும் |
| **பட்டியல்** | `manager.GetCatalogAsync()` | பட்டியலைப் பெறு |
| **பட்டியல்** | `catalog.ListModelsAsync()` | அனைத்து மாதிரிகளையும் பட்டியலிடவும் |
| **பட்டியல்** | `catalog.GetModelAsync(alias)` | குறிப்பிட்ட மாதிரியைப் பெறவும் |
| **பட்டியல்** | `catalog.GetCachedModelsAsync()` | கேஷ் செய்யப்பட்ட மாதிரிகள் பட்டியல் |
| **பட்டியல்** | `catalog.GetLoadedModelsAsync()` | ஏற்றப்பட்ட மாதிரிகள் பட்டியல் |
| **மாதிரி** | `model.DownloadAsync(progress?)` | மாதிரியை பதிவிறக்கம் செய்யவும் |
| **மாதிரி** | `model.LoadAsync()` | மாதிரியை ஏற்றவும் |
| **மாதிரி** | `model.UnloadAsync()` | மாதிரியை ஏற்றலை செய்யவும் |
| **மாதிரி** | `model.SelectVariant(variant)` | ஹார்ட்வேர் வகையைத் தேர்வு செய்க |
| **மாதிரி** | `model.GetChatClientAsync()` | சொந்தச் சொடுகருத்துக் கிளையண்டைப் பெறவும் |
| **மாதிரி** | `model.GetAudioClientAsync()` | ஒலி உருமாற்ற கிளையண்டைப் பெறவும் |
| **மாதிரி** | `model.GetPathAsync()` | உள்ளக கோப்புறை பாதையைப் பெறவும் |
| **பட்டியல்** | `catalog.GetModelVariantAsync(alias, variant)` | குறிப்பிட்ட ஹார்ட்வேயர் வகையைப் பெறவும் |
| **பட்டியல்** | `catalog.UpdateModelsAsync()` | பட்டியலை புதுப்பிக்கவும் |
| **சேவை** | `manager.StartWebServerAsync()` | REST வெப் சர்வரை துவங்கவும் |
| **சேவை** | `manager.StopWebServerAsync()` | REST வெப் சர்வரை நிறுத்தவும் |
| **அமைப்பு** | `config.ModelCacheDir` | கேஷ் கோப்புறை |

---

## முக்கிய எடுத்துக்கொள்கைகள்

| கருத்து | நீங்கள் கற்றுக்கொண்டது |
|---------|-----------------|
| **SDK மற்றும் CLI** | SDK திட்டமிட்ட கட்டுப்பாட்டை வழங்குகிறது – பயன்பாடுகளுக்கு அவசியம் |
| **கட்டுப்பாட்டு விமானம்** | SDK சேவைகள், மாதிரிகள், மற்றும் கேஷிங்கை நிர்வகிக்கிறது |
| **சீரற்ற போர்ட்டுகள்** | எப்போதும் `manager.endpoint` (பைதான்) அல்லது `manager.urls[0]` (JS/C#) பயன்படுத்தவும் - போர்ட்டை கடுமையாக நிர்ணயிக்க வேண்டாம் |
| **பெயர்ப்பெயர்கள்** | தானாக ஹார்ட்வேருக்கு உகந்த மாதிரியை தேர்ந்தெடுக்க பெயர்ப்பெயர்களைப் பயன்படுத்தவும் |
| **விரைவு ஸ்டார்ட்** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# மறுசீரமைப்பு** | v0.8.0+ தன்னிலை உடையது - பயனர் இயந்திரங்களில் CLI தேவையில்லை |
| **மாதிரி வாழ்க்கைச்சுழற்சி** | படிவியல் → பதிவிறக்கு → ஏற்றுகொள் → பயன்படுத்து → இறக்கு |
| **FoundryModelInfo** | செழுமையான மெட்டா தரவு: வேலை, சாதனம், அளவு, உரிமம், கருவி அழைப்பு ஆதாரம் |
| **ChatClient** | OpenAI-இலவச பயன்பாட்டிற்கு `createChatClient()` (JS) / `GetChatClientAsync()` (C#) |
| **வகைகள்** | மாதிரிகளுக்கு இயந்திர விசேஷமான வகைகள் உள்ளன (CPU, GPU, NPU); தானாக தேர்வு செய்யப்படும் |
| **மேம்பாடுகள்** | Python: தற்போதைய மாதிரிகளை பராமரிக்க `is_model_upgradeable()` + `upgrade_model()` |
| **படிவியல் புதுப்பிப்பு** | புதிய மாதிரிகள் கண்டுபிடிக்க `refresh_catalog()` (Python) / `updateModels()` (JS) |

---

## வளங்கள்

| வளம் | இணைப்பு |
|----------|------|
| SDK குறிப்ப reference (அனைத்து மொழிகளும்) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| inference SDK-களுடன் ஒருங்கிணைப்பு | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API குறிப்ப reference | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK மாதிரிகள் | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local இணையத்தளம் | [foundrylocal.ai](https://foundrylocal.ai) |

---

## அடுத்த படிகள்

SDK-ஐ OpenAI மூலக்கூறுடன் இணைத்து உங்கள் முதல் உரையாடல் முடிப்பு பயன்பாட்டை உருவாக்க [பகுதி 3: OpenAI உடன் SDK பயன்படுத்தல்](part3-sdk-and-apis.md) தொடரவும்.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**உறுதிப்பத்திரம்**:
இந்த ஆவணம் AI மொழிபெயர்ப்பு சேவையான [Co-op Translator](https://github.com/Azure/co-op-translator) மூலம் மொழிபெயர்க்கப்பட்டுள்ளது. நாங்கள் துல்லியத்திற்காக முயற்சி செய்கிறோம் என்றாலும், தானாக செய்யப்பட்ட மொழிபெயர்ப்புகளில் பிழைகள் அல்லது தவறுகள் இருக்கக்கூடும் என்பதை கவனிக்கவும். இதன் தாய்மொழியில் உள்ள அசல் ஆவணம் அதிகாரபூர்வமான மூலமாக கருதப்பட வேண்டும். முக்கியமான தகவல்களுக்கு, தொழில்முறை மனித மொழிபெயர்ப்பு பரிந்துரைக்கப்படுகிறது. இந்த மொழிபெயர்ப்பின் பயன்பாட்டால் ஏற்படும் எந்த误解ங்களுக்கும் அல்லது தவறான விளக்கங்களுக்கும் நாங்கள் பொறுப்பேற்கமுடியாது.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->