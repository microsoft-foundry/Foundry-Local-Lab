![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ഭാഗം 2: Foundry Local SDK ആഴത്തിൽ പഠിക്കുക

> **ലക്ഷ്യം:** മോഡലുകൾ, സേവനങ്ങൾ, കാഷിംഗ് പ്രോഗ്രാമാറ്റിക്കൽ രീതിയിൽ കൈകാര്യം ചെയ്യാൻ Foundry Local SDK ന熟 Master ചെയ്യുക - ആപ്ലിക്കേഷനുകൾ നിർമ്മിക്കുന്നതിന് CLIയുടെ പകരം SDK നീ കൈകാര്യം ചെയ്യേണ്ടതിന്റെ കാരണങ്ങൾ മനസ്സിലാക്കുക.

## അവലോകനം

പണിയിൽ 1ൽ നിങ്ങൾ **Foundry Local CLI** ഉപയോഗിച്ച് മോഡലുകൾ ഡൗൺലോഡ് ചെയ്ത് ഇന്ററാക്ടീവായി പ്രവർത്തിപ്പിച്ചു. CLI അന്വേഷണം നിർവഹിക്കാൻ വളരെ നല്ലതാണ്, എന്നാൽ നിങ്ങൾ യഥാർത്ഥ ആപ്ലിക്കേഷനുകൾ നിർമ്മിക്കുമ്പോൾ **പ്രോഗ്രാമാറ്റിക് നിയന്ത്രണം** ആവശ്യമാണ്. Foundry Local SDK അതിനാണ് ലഭ്യമാക്കുന്നത് - ഇത് **കൺട്രോൾ പ്ലെയിൻ** (സേവനം തുടങ്ങൽ, മോഡലുകൾ കണ്ടെത്തൽ, ഡൗൺലോഡ് ചെയ്യൽ, ലോഡ് ചെയ്യൽ) കൈകാര്യം ചെയ്യുന്നു, നിങ്ങളുടെ ആപ്ലിക്കേഷൻ കോഡ് **ഡേറ്റാ പ്ലെയിൻ**-ൽ (പ്രോംസുകൾ അയക്കൽ, പൂർണ്ണതകൾ സ്വീകരിക്കൽ) കേന്ദ്രീകരിക്കാം.

ഈ ലാബിൽ Python, JavaScript, C# എന്നിവിടെയുള്ള SDK API മുഴുവൻ പഠിപ്പിക്കും. അവസാനത്തിന് നിങ്ങൾ എല്ലാ പ്രാപ്യമായ രീതികളും അവ ഉപയോഗിക്കേണ്ട സമയവും മനസ്സിലാകും.

## പഠന ലക്ഷ്യങ്ങൾ

ഈ ലാബിന്റെ അവസാനം നിങ്ങൾക്ക് കഴിയും:

- SDK ആപ്ലിക്കേഷൻ വികസനത്തിൽ CLIക്ക് പകരം തിരഞ്ഞെടുക്കേണ്ടതിന്റെ കാരണം വിശദീകരിക്കുക
- Python, JavaScript, അല്ലെങ്കിൽ C#-ക്കായി Foundry Local SDK ഇൻസ്റ്റാൾ ചെയ്യുക
- `FoundryLocalManager` ഉപയോഗിച്ച് സേവനം തുടങ്ങുക, മോഡലുകൾ കൈകാര്യം ചെയ്യുക, കാറ്റലോഗ് ചോദിച്ചറിയുക
- പ്രോഗ്രാമാറ്റിക്കമായി മോഡലുകൾ പട്ടികപ്പെടുത്തുക, ഡൗൺലോഡ് ചെയ്യുക, ലോഡ് ചെയ്യുക, അൺലോഡ് ചെയ്യുക
- `FoundryModelInfo` ഉപയോഗിച്ച് മോഡൽ മെറ്റാഡേറ്റ പരിശോധിക്കുക
- കാറ്റലോഗ്, കാഷേ, ലോഡുചെയ്‌ത മോഡലുകൾ തമ്മിലുള്ള വ്യത്യാസം മനസ്സിലാക്കുക
- കൺസ്ട്രക്ടർ ബൂറ്റ്സ്ട്രാപ്പ് (Python) ഉം `create()` + കാറ്റലോഗ് മാതൃക (JavaScript) ഉം ഉപയോഗിക്കുക
- C# SDK പുനര്‍സംരചനയും അതിന്റെ ഒബ്ജക്ട്-ഓറിയന്റഡ് APIയും മനസ്സിലാക്കുക

---

## മുൻകരുതലുകൾ

| ആവശ്യകം | വിവരങ്ങൾ |
|-------------|---------|
| **Foundry Local CLI** | ഇൻസ്റ്റാൾ ചെയ്തിട്ടും നിങ്ങളുടെ `PATH`-ൽിരിക്കണം ([പാര്ട് 1](part1-getting-started.md)) |
| **ഭാഷാ റൺടൈം** | **Python 3.9+** അല്ലെങ്കിൽ **Node.js 18+** അല്ലെങ്കിൽ **.NET 9.0+** |

---

## ആശയം: SDK vs CLI - SDK എന്തിന് ഉപയോഗിക്കണം?

| വിചാരം | CLI (`foundry` കമാൻഡ്) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **ഉപയോഗം** | അന്വേഷണത്തിനും മാനുവൽ ടെസ്റ്റിംഗിനും | ആപ്ലിക്കേഷൻ ഇന്റഗ്രേഷനും |
| **സേവന മാനേജ്മെൻറ്** | മാനുവൽ: `foundry service start` | ഓട്ടോമാറ്റിക്: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **പോർട്ട് കണ്ടെത്തൽ** | CLI ഔട്ട്പുട്ടിൽനിന്ന് വായിക്കുക | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **മോഡൽ ഡൗൺലോഡ്** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **എറർ കൈകാര്യം** | എക്സിറ്റ് കോഡുകൾ, stderr | എക്സപ്ഷൻകൾ, ടൈപ്പ്ഡ് എററുകൾ |
| **ഓട്ടോമേഷൻ** | ഷെൽ സ്‌ക്രിപ്റ്റുകൾ | നേറ്റീവ് ഭാഷ ഇന്റഗ്രേഷൻ |
| **ഡിപ്ലോയ്മെൻറ്** | അവസാനം ഉപഭോക്താവ് മെഷീനിൽ CLI ആവശ്യം | C# SDK സ്വയം അടങ്ങിയിരിക്കുന്നു (CLI ആവശ്യമില്ല) |

> **പ്രധാന കാണിപ്പ്:** SDK മുഴുവൻ ജീവിതചക്രവും കൈകാര്യം ചെയ്യുന്നു: സേവനം തുടങ്ങൽ, കാഷേ പരിശോധിക്കൽ, മിസ്സിംഗ് മോഡലുകൾ ഡൗൺലോഡ് ചെയ്യൽ, ലോഡ് ചെയ്യൽ, എন্ড്പോയിന്റ് കണ്ടെത്തൽ, കുറച്ച് കോഡ് ലൈനുകളിൽ. നിങ്ങളുടെ ആപ്ലിക്കേഷൻ CLI ഔട്ട്പുട്ട് പാഴ്‌സാക്കേണ്ടതോ subprocesses മാനേജു ചെയ്യേണ്ടതോ ഇല്ല.

---

## ലാബ് അഭ്യാസങ്ങൾ

### അഭ്യാസം 1: SDK ഇൻസ്റ്റാൾ ചെയ്യുക

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

ഇൻസ്റ്റാളേഷൻ പരിശോധിക്കുക:

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

ഇൻസ്റ്റാളേഷൻ പരിശോധിക്കുക:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

നു​ഗെറ്റ് പാക്കേജുകൾ രണ്ട്:

| പാക്കേജ് | പ്ലാറ്റ്‌ഫോം | വിവരണം |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | ക്രോസ്-പ്ലാറ്റ്‌ഫോം | Windows, Linux, macOS-ൽ പ്രവർത്തിക്കുന്നു |
| `Microsoft.AI.Foundry.Local.WinML` | മാത്രം Windows | WinML ഹാർഡ്‌വെയർ ആക്സിലറേഷൻ; പ്ലഗിൻ എക്സിക്യൂഷൻ പ്രൊവൈഡർമാർ ഡൗൺലോഡ് ചെയ്ത് ഇൻസ്റ്റാൾ ചെയ്യുന്നു (ഉദാ.: Qualcomm NPU-ക്കുള്ള QNN) |

**Windows സജ്ജീകരണം:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` ഫയൽ എഡിറ്റ് ചെയ്യുക:

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

> **സൂചന:** Windows-ൽ WinML പാക്കേജ് ആണ് ബേസ് SDKക്കും QNN എക്സിക്യൂഷൻ പ്രൊവൈഡറിനും ഉള്ള സൂപർസെറ്റ്. Linux/macOS-ൽ ബേസ് SDK ആണ് ഉപയോഗിക്കുന്നത്. കണ്ടീഷണൽ TFM, പാക്കേജ് രേഖപ്പെടുത്തലുകൾ പ്രോജക്ട് പൂർത്തിയായി ക്രോസ്-പ്ലാറ്റ്‌ഫോം ആക്കുന്നു.

പ്രോജക്ട് റൂട്ടിൽ `nuget.config` സൃഷ്ടിക്കുക:

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

പാക്കേജുകൾ പുനഃസ്ഥാപിക്കുക:

```bash
dotnet restore
```

</details>

---

### അഭ്യാസം 2: സേവനം തുടങ്ങി കാറ്റലോഗ് പട്ടിക കാണുക

ഏതെങ്കിലും ആപ്ലിക്കേഷൻ ആദ്യം ചെയ്യുന്നത് Foundry Local സേവനം തുടങ്ങുകയും ലഭ്യമായ മോഡലുകൾ കണ്ടെത്തുകയും ചെയ്യുക ആണ്.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# ഒരു മാനേജറെ സൃഷ്ടിച്ച് സേവനം ആരംഭിക്കുക
manager = FoundryLocalManager()
manager.start_service()

# പട്ടികയിൽ ലഭ്യമായ എല്ലാ മോഡലുകളും പട്ടികപ്പെടുത്തുക
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - സേവന മാനേജ്മെന്റ് മേതഡുകൾ

| മേതഡ് | സിഗ്നേച്ചർ | വിവരണം |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | സേവനം പ്രവർത്തിക്കുന്നുണ്ടോ എന്ന് പരിശോധിക്കുക |
| `start_service()` | `() -> None` | Foundry Local സേവനം ആരംഭിക്കുക |
| `service_uri` | `@property -> str` | അടിസ്ഥാന സേവന URI |
| `endpoint` | `@property -> str` | API എൻപോയിന്റ് (സേവന URI + `/v1`) |
| `api_key` | `@property -> str` | API കീ (എൻവയോൺമെന്റിൽ നിന്ന് അല്ലെങ്കിൽ ഡിഫോൾട്ട് പ്ലേസ്ഹോൾഡർ) |

#### Python SDK - കാറ്റലോഗ് മാനേജ്മെന്റ് മേതഡുകൾ

| മേതഡ് | സിഗ്നേച്ചർ | വിവരണം |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | കാറ്റലോഗിലുള്ള എല്ലാ മോഡലുകളും പട്ടികപ്പെടുത്തുക |
| `refresh_catalog()` | `() -> None` | സേവനത്തിൽനിന്നും കാറ്റലോഗ് പുതുക്കുക |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | പ്രത്യേക മോഡലിന് വിവരങ്ങൾ എടുക്കുക |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ഒരു മാനേജര്‍ സൃഷ്ടിച്ച് സര്‍വീസ് ആരംഭിക്കുക
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// കാറ്റലോഗ് ബ്രൗസ് ചെയ്യുക
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - മാനേജർ മേതഡുകൾ

| മേതഡ് | സിഗ്നേച്ചർ | വിവരണം |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK സിംഗിൾടൺ ഇനിഷ്യലൈസ് ചെയ്യുക |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | സിംഗിൾടൺ മാനേജർ ആക്സസ് ചെയ്യുക |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local വെബ് സേവനം തുടങ്ങി |
| `manager.urls` | `string[]` | സേവനത്തിനുള്ള അടിസ്ഥാന URL-കളുടെ ആരേ |

#### JavaScript SDK - കാറ്റലോഗ് & മോഡൽ മേതഡുകൾ

| മേതഡ് | സിഗ്നേച്ചർ | വിവരണം |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | മോഡൽ കാറ്റലോഗ് ആക്സസ് ചെയ്യുക |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | അലിയാസ് ഉപയോഗിച്ച് മോഡൽ ഒബ്ജക്റ്റ് എടുക്കുക |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ ഒബ്ജക്റ്റ്-ഓറിയന്റഡ് ആർക്കിടെക്ചറുപയോഗിക്കുന്നു: `Configuration`, `Catalog`, `Model` ഒബ്ജക്റ്റുകൾ:

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

#### C# SDK - പ്രധാന ക്ലാസുകൾ

| ക്ലാസ് | ഉദ്ദേശ്യം |
|-------|---------|
| `Configuration` | ആപ്ലിക്കേഷൻ പേര്, ലോഗ് ലെവൽ, കാഷേ ഡയറക്ടরি, വെബ് സെർവർ URLs സെറ്റ് ചെയ്യുക |
| `FoundryLocalManager` | പ്രധാന എൻട്രിപോയിന്റ് - `CreateAsync()` മുഖേന സൃഷ്ടിക്കപ്പെടുന്നു, `.Instance` വഴി ആക്‌സസ് ചെയ്യപ്പെടുന്നു |
| `Catalog` | കാറ്റലോഗ് ബ്രൗസ് ചെയ്യുക, തിരയുക, മോഡലുകൾ നേടുക |
| `Model` | ഒരു പ്രത്യേക മോഡൽ പ്രതിനിധാനം ചെയ്യുന്നു - ഡൗൺലോഡ്, ലോഡ്, ക്ലയന്റുകൾ നേടുക |

#### C# SDK - മാനേജർ & കാറ്റലോഗ് മേതഡുകൾ

| മേതഡ് | വിവരണം |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | മാനേജർ ഇനിഷ്യലൈസ് ചെയ്യുക |
| `FoundryLocalManager.Instance` | സിംഗിൾടൺ മാനേജറിലേക്ക് ആക്‌സസ് |
| `manager.GetCatalogAsync()` | മോഡൽ കാറ്റലോഗ് നേടുക |
| `catalog.ListModelsAsync()` | ലഭ്യമായ എല്ലാ മോഡലുകളും പട്ടികപ്പെടുത്തുക |
| `catalog.GetModelAsync(alias: "alias")` | അലിയാസ് ഉപയോഗിച്ച് പ്രത്യേക മോഡൽ എടുക്കുക |
| `catalog.GetCachedModelsAsync()` | ഡൗൺലോഡ് ചെയ്ത മോഡലുകൾ പട്ടികപ്പെടുത്തുക |
| `catalog.GetLoadedModelsAsync()` | നിലവിൽ ലോഡുചെയ്‌ത മോഡലുകൾ പട്ടികപ്പെടുത്തുക |

> **C# ആർക്കിടെക്ചർ കുറിപ്പ്:** C# SDK v0.8.0+ പുനർനിർമിതമായ ആപ്ലിക്കേഷൻ സ്വയം അടങ്ങിയിരിക്കണം; അവസാനം ഉപഭോക്താവിന്റെ മെഷീനിൽ Foundry Local CLI ആവശ്യമായില്ല. SDK സ്വാഭാവികമായി മോഡൽ മാനേജ്മെന്റ്, ഇൻഫറൻസും കൈകാര്യം ചെയ്യുന്നു.

</details>

---

### അഭ്യാസം 3: ഒരു മോഡൽ ഡൗൺലോഡ് ചെയ്ത് ലോഡ് ചെയ്യുക

SDK ഡൗൺലോഡ് (ഡിസ്‌കിലേക്ക്) ലോഡ് ചെയ്യുന്നതിൽനിന്ന് (മെമ്മറിയിലേക്ക്) വ്യത്യസ്തമാക്കുന്നു. ഇത് സെറ്റപ് സമയത്ത് മോഡലുകൾ മുൻകൂട്ടി ഡൗൺലോഡ് ചെയ്ത് ആവശ്യമുള്ളപ്പോൾ ലോഡ് ചെയ്യാനാകും.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# ഓപ്‌ഷൻ A: മാനുവൽ ഘട്ടം ഘട്ടമായുള്ള
manager = FoundryLocalManager()
manager.start_service()

# ആദ്യം ക്യാഷെ പരിശോധിക്കുക
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

# ഓപ്‌ഷൻ B: ഒറ്റ വരി ബൂട്ട്‌സ്ട്രാപ് (പരിഗണനാ)
# കോൺസ്ട്രക്ടറിലേക്ക് അലിയാസ് പാസ്സ് ചെയ്യൂ - ഇത് സർവീസ് തുടങ്ങുന്നു, ഡൗൺലോഡ് ചെയ്യുന്നു, സ്വയം ലോഡുചെയ്യുന്നു
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - മോഡൽ മാനേജ്മെന്റ് മേതഡുകൾ

| മേതഡ് | സിഗ്നേച്ചർ | വിവരണം |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | മോഡൽ ലോക്കൽ കാഷേയ്ക്ക് ഡൗൺലോഡ് ചെയ്യുക |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | മോഡൽ ഇൻഫറൻസ് സെർവറിലേക്ക് ലോഡ് ചെയ്യുക |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | മോഡൽ സെർവറിൽനിന്ന് അൺലോഡ് ചെയ്യുക |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | നിലവിൽ ലോഡുചെയ്‌ത മോഡലുകൾ പട്ടികപ്പെടുത്തുക |

#### Python - കാഷേ മാനേജ്മെന്റ് മേതഡുകൾ

| മേതഡ് | സിഗ്നേച്ചർ | വിവരണം |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | കാഷേ ഡയറക്ടറി പാത്ത് നേടുക |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | ഡൗൺലോഡ് ചെയ്ത മോഡലുകൾ പട്ടികപ്പെടുത്തുക |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// ഘട്ടംഘട്ടമായ സമീപനം
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

#### JavaScript - മോഡൽ മേതഡുകൾ

| മേതഡ് | സിഗ്നേച്ചർ | വിവരണം |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | മോഡൽ ഇതിനകം ഡൗൺലോഡ് ചെയ്തിട്ടുണ്ടോ എന്ന് |
| `model.download()` | `() => Promise<void>` | മോഡൽ ലോക്കൽ കാഷേയ്ക്ക് ഡൗൺലോഡ് ചെയ്യുക |
| `model.load()` | `() => Promise<void>` | ഇൻഫറൻസ് സെർവറിലേക്ക് ലോഡ് ചെയ്യുക |
| `model.unload()` | `() => Promise<void>` | ഇൻഫറൻസ് സെർവറിൽനിന്ന് അൺലോഡ് ചെയ്യുക |
| `model.id` | `string` | മോഡലിന്റെ വ്യത്യസ്ത ഐഡന്റിഫയർ |

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

#### C# - മോഡൽ മേതഡുകൾ

| മേതഡ് | വിവരണം |
|--------|-------------|
| `model.DownloadAsync(progress?)` | തിരഞ്ഞെടുക്കപ്പെട്ട വേരിയന്റ് ഡൗൺലോഡ് ചെയ്യുക |
| `model.LoadAsync()` | മോഡൽ മെമ്മറിയിലേക്ക് ലോഡ് ചെയ്യുക |
| `model.UnloadAsync()` | മോഡൽ അൺലോഡ് ചെയ്യുക |
| `model.SelectVariant(variant)` | പ്രത്യേക വേരിയന്റ് തിരഞ്ഞെടുക്കുക (CPU/GPU/NPU) |
| `model.SelectedVariant` | ഇപ്പോൾ തിരഞ്ഞെടുക്കപ്പെട്ട വേരിയന്റ് |
| `model.Variants` | ഈ മോഡലിന്റെ എല്ലാ ലഭ്യമായ വേരിയന്റുകൾ |
| `model.GetPathAsync()` | ലോക്കൽ ഫയൽ പാത്ത് നേടുക |
| `model.GetChatClientAsync()` | നേറ്റീവ് ചാറ്റ് ക്ലയന്റ് ലഭിക്കുക (OpenAI SDK ആവശ്യമില്ല) |
| `model.GetAudioClientAsync()` | ട്രാൻസ്ക്ക്രിപ്ഷനിനുള്ള ഓഡിയോ ക്ലയന്റ് നേടുക |

</details>

---

### അഭ്യാസം 4: മോഡൽ മെറ്റാഡേറ്റ പരിശോധിക്കുക

`FoundryModelInfo` ഒബ്ജക്റ്റ് ഓരോ മോഡലിന്റെയും സമ്പൂർണ മെറ്റാഡേറ്റ അടക്കുന്നു. ഈ ഫീൽഡുകൾ മനസിലാക്കുന്നതിലൂടെ നിങ്ങൾക്ക് നിങ്ങളുടെ ആപ്ലിക്കേഷനു ശരിയായ മോഡൽ തിരഞ്ഞെടുക്കാൻ സാധിക്കും.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ഒരു നിർദ്ദിഷ്‌ട മോഡലിനെക്കുറിച്ച് വിശദമായ വിവരങ്ങൾ നേടുക
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

#### FoundryModelInfo ഫീൽഡുകൾ

| ഫീൽഡ് | തരം | വിവരണം |
|-------|------|-------------|
| `alias` | string | ചെറുപേരായ (ഉദാ.: `phi-3.5-mini`) |
| `id` | string | വ്യത്യസ്തമായ മോഡൽ ഐഡന്റിഫയർ |
| `version` | string | മോഡൽ പതിപ്പ് |
| `task` | string | `chat-completions` അല്ലെങ്കിൽ `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, അല്ലെങ്കിൽ NPU |
| `execution_provider` | string | റൺടൈം ബാക്ക്എൻഡ് (CUDA, CPU, QNN, WebGPU, മുതലായവ) |
| `file_size_mb` | int | MBലുള്ള ഡിസ്കിലെ വലിപ്പം |
| `supports_tool_calling` | bool | ഫംഗ്ഷൻ / ടൂൾ കാൾ ചെയ്യുന്നത് സപ്പോർട്ട് ചെയ്യുന്നുണ്ടോ എന്ന് |
| `publisher` | string | മോഡൽ പുറത്തിറക്കിയത് ആരെന്ന് |
| `license` | string | ലൈസൻസ് പേരു |
| `uri` | string | മോഡൽ URI |
| `prompt_template` | dict/null | പ്രോംപ്റ്റ് ടെമ്പ്ലേറ്റ്, എങ്കിൽ |

---

### അഭ്യാസം 5: മോഡൽ ജീവിതചക്രം കൈകാര്യം ചെയ്യുക

പൂർണ്ണ ജീവിതചക്രം പ്രാക്ടീസ് ചെയ്യുക: പട്ടിക → ഡൗൺലോഡ് → ലോഡ് → ഉപയോഗിക്കുക → അൺലോഡ്.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # വേഗത്തിൽ പരീക്ഷിക്കാൻ ചെറിയ മോഡൽ

manager = FoundryLocalManager()
manager.start_service()

# 1. കാറ്റലോഗിൽ എ什么 ഉണ്ട് എന്ന് പരിശോധിക്കുക
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. ഇതിനകം ഡൗൺലോഡ് ചെയ്തിട്ടുണ്ടോ എന്ന് പരിശോധിക്കുക
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. ഒരു മോഡൽ ഡൗൺലോഡ് ചെയ്യുക
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. അതിപ്പോൾ ക്യാഷെയിൽ ഉണ്ടെന്ന് സ്ഥിരീകരിക്കുക
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. അത് ലോഡ് ചെയ്യുക
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. എന്തെല്ലാം ലോഡ് ചെയ്‌തിട്ടുണ്ട് എന്ന് പരിശോധിക്കുക
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. അത് അൺലോഡ് ചെയ്യുക
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

const alias = "qwen2.5-0.5b"; // വേഗത്തിലുള്ള പരിശോധനയ്ക്കുള്ള ചെറിയ മോഡൽ

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. കാറ്റലോഗിൽ നിന്നുമODEൽ നേടുക
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. ആവശ്യമെങ്കിൽ ഡൗൺലോഡ് ചെയ്യുക
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. അത് ലോഡ് ചെയ്യുക
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. അത് അൺലോഡ് ചെയ്യുക
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Exercise 6: ദ്രുത ആരംഭ സൂത്രപദ്ദേശങ്ങൾ

ഓരോ ഭാഷയും സർവീസ് ആരംഭിക്കാനും ഒരു മോടൽ ലോഡ് ചെയ്യാനും ഒരു ഷോർട്‌കട്ട് നൽകുന്നു. ഇവയാണ് **ബഹുഭൂരിപക്ഷ പ്രയോഗങ്ങൾക്കുള്ള ശിപാർശ ചെയ്ത സൂത്രങ്ങൾ**.

<details>
<summary><h3>🐍 Python - കൺസ്ട്രക്ടർ ബൂട്ട്‌സ്ട്രാപ്പ്</h3></summary>

```python
from foundry_local import FoundryLocalManager

# കൺസ്ട്രക്റ്ററിന് ഒരു അലിയാസ് പാസ്സ് ചെയ്യുക - ഇത് എല്ലാം കൈകാര്യം ചെയ്യുന്നു:
# 1. സർവീസ് പ്രവർത്തനത്തിലാണ് എങ്കിൽ തവിടാതെ അതു ആരംഭിക്കുന്നു
# 2. മോദൽ കാഷേ ചെയ്തിട്ടില്ലെങ്കിൽ ഡൗൺലോഡ് ചെയ്യുന്നു
# 3. മോദൽ ഇൻഫറൻസ് സർവറിൽ ലോഡ് ചെയ്യുന്നു
manager = FoundryLocalManager("phi-3.5-mini")

# ഉടൻ ഉപയോഗിക്കാൻ തയ്യാറായി
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` പാരാമീറ്റർ (ഡീഫോൾട്ട് `True`) ഈ പെരുമാറ്റം നിയന്ത്രിക്കുന്നു. നിങ്ങൾക്ക് മാനുവൽ നിയന്ത്രണം വേണെങ്കിൽ `bootstrap=False` സെറ്റ് ചെയ്യുക:

```python
# മാനുവൽ മോഡ് - എല്ലാം സ്വയം സംഭവിക്കുന്നില്ല
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + ക്യാറ്റലോഗ്</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() എല്ലാം കൈകാര്യം ചെയ്യുന്നു:
// 1. സേവനം ആരംഭിക്കുന്നു
// 2. കാറ്റലോഗിൽ നിന്ന് മോഡൽ ലഭിക്കുന്നു
// 3. ആവശ്യമെങ്കിൽ ഡൗൺലോഡ് ചെയ്ത് മോഡൽ ലോഡ് ചെയ്യുന്നു
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// താത്കാലികമായി ഉപയോഗത്തിന് ഒരുക്കം
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + ക്യാറ്റലോഗ്</h3></summary>

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

> **C# നോട്ട്സ്:** C# SDK ആപ്പ് നാമം, ലോഗിംഗ്, കാഷെ ഡയറക്ടറികൾ എന്നിവ നിയന്ത്രിക്കാൻ `Configuration` ഉപയോഗിക്കുന്നു, കൂടാതെ ഒരു പ്രത്യേക വെബ് സെർവർ പോർട്ട് പിൻ ചെയ്യാനും സാധിക്കുന്നു. ഇത് മൂന്ന് SDK-കളിലെയും ഏറ്റവും ക്രമീകരിക്കാവുന്നതാണ്.

</details>

---

### Exercise 7: നാടിവ് ChatClient (OpenAI SDK വേണമെന്നില്ല)

JavaScript, C# SDKകൾ `createChatClient()` എന്ന സൗകര്യമുള്ള ഒരു നാടിവ് ചാറ്റ് ക്ലയന്റ് മെഥഡ് നൽകുന്നു — OpenAI SDK വേർതിരിച്ച് ഇൻസ്റ്റാൾ ചെയ്യാനും കോൺഫിഗർ ചെയ്യാനുമുള്ള ആവശ്യമില്ല.

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

// മോഡലിൽ നിന്ന് നേരിട്ട് ഒരു ChatClient സൃഷ്ടിക്കുക — OpenAI ഇംപോർട്ട് ആവശ്യമില്ല
const chatClient = model.createChatClient();

// completeChat ഒരു OpenAI-അനുകൂല പ്രതികരണ объект് നൽകുന്നു
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// സ്റ്റ്രീമിംഗ് ഒരു കോൾബാക്ക് മാതൃക ഉപയോഗിക്കുന്നു
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` ടൂൾ കോളിംഗ് പിന്തുണയ്ക്കുന്നു — രണ്ടാമത്തെ ആർഗ്യുമെന്റായി ടൂളുകൾ പാസ്സ് ചെയ്യാവുന്നത്:

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

> **ഏത് സൂത്രം ഉപയോഗിക്കണം എന്നത്:**
> - **`createChatClient()`** — ദ്രുത പ്രോട്ടോടൈപ്പിങ്, കുറച്ചുള്ള ആശ്രിതങ്ങൾ, ലളിതമായ കോഡ്
> - **OpenAI SDK** — പാരാമീറ്ററുകൾ (താപനില, top_p, സ്റ്റോപ്പ് ടോക്കണുകൾ മുതലായവ) *സമ്പൂർണ നിയന്ത്രണം*, ഉത്പാദന ആപ്ലിക്കേഷനുകൾക്ക് ഉത്തമം

---

### Exercise 8: മോഡൽ വേരിയന്റുകളും ഹാർഡ്‌വെയർ തിരഞ്ഞെടുപ്പും

മോഡലുകൾക്കിന് വിവിധ **വേരിയന്റുകൾ** ഉണ്ടാകുന്നു, വ്യത്യസ്ത ഹാർഡ്‌വെയർ ഓപ്റ്റിമൈസേഷനോടെ. SDK ഏറ്റവും മികച്ച വേരിയന്റിനെ സ്വയം തിരഞ്ഞെടുക്കുന്നു, എന്നാൽ നിങ്ങൾക്ക് ഉദ്ഘാടനം പരിശോധിച്ച് മാനുവലി തിരഞ്ഞെടുക്കാം.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// ലഭ്യമായ എല്ലാ വകഭേദങ്ങളും പട്ടികവായി കാണിക്കുക
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// നിങ്ങളുടെ ഹാർഡ്‌വെയറിന് മികച്ച വകഭേദം SDK ഓട്ടോമാറ്റിക്കായി തിരഞ്ഞെടുക്കുന്നു
// മറികടക്കാൻ, selectVariant() ഉപയോഗിക്കുക:
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

Python-ൽ, SDK ഹാർഡ്‌വെയർ അടിസ്ഥാനമാക്കി മികച്ച വേരിയന്റിനെ സ്വയം തിരഞ്ഞെടുക്കുന്നു. തിരഞ്ഞെടുക്കപ്പെട്ടത് കാണാൻ `get_model_info()` ഉപയോഗിക്കുക:

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

#### NPU വേരിയന്റുകൾ ഉള്ള മോഡലുകൾ

കെവേഷൻ പ്രോസസിംഗ് യൂണിറ്റുകൾ ഉള്ള യന്ത്രങ്ങൾക്ക് (Qualcomm Snapdragon, Intel Core Ultra) NPU-ഓപ്റ്റിമൈസ്ഡ് വേരിയന്റുകൾ ലഭ്യമാണ്:

| മോഡൽ | NPU വേരിയന്റ് ലഭ്യമാണോ |
|-------|:---------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **ടിപ്പ്:** NPU ശേഷിയുള്ള ഹാർഡ്‌വെയറിൽ SDK സ്വയമേവ NPU വേരിയന്റ് തിരഞ്ഞെടുക്കുന്നു. നിങ്ങളുടെ കോഡ് മാറ്റേണ്ടതില്ല. Windows-ൽ C# പ്രോജക്റ്റുകൾക്കായി `Microsoft.AI.Foundry.Local.WinML` NuGet പാക്കേജ് ചേർക്കുക — ഇത് WinML പ്ലഗിൻ വഴി QNN എക്സിക്യൂഷൻ പ്രൊവൈഡറെ സജീവമാക്കും.

---

### Exercise 9: മോഡൽ അപ്ഗ്രേഡുകളും ക്യാറ്റലോഗ് പുതുക്കലുകളും

മോഡൽ ക്യാറ്റലോഗ് കാലക്രമേണ അപ്ഡേറ്റ് ചെയ്യുന്നു. അപ്ഡേറ്റുകൾ പരിശോധിക്കാനും പ്രയോഗിക്കാനും താഴെയുള്ള മെഥഡുകൾ ഉപയോഗിക്കുക.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# പുതിയ മോഡൽ ലിസ്റ്റ് ലഭിക്കാൻ ക്യാറ്റലോഗ് പുതുക്കുക
manager.refresh_catalog()

# കാഷ് ചെയ്ത മോഡലിന് പുതിയ പതിപ്പ് ലഭ്യമാണോ എന്ന് പരിശോധിക്കുക
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

// ഏറ്റവും പുതിയ മോഡൽ പട്ടിക നേടുന്നതിന് കാറ്റലോഗ് പുതുക്കുക
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// പുതുക്കലിന് ശേഷം ലഭ്യമായ എല്ലാ മോഡലുകളും പട്ടികപ്പെടുത്തുക
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Exercise 10: റീസണിംഗ് മോഡലുകളുമായി ജോലി ചെയ്യുക

**phi-4-mini-reasoning** മോഡൽ ചെയിൻ-ഓഫ്-തോട്ട് റീസണിംഗ് ഉൾക്കൊള്ളുന്നു. ഇത് അന്തർഗ്ഗത ചിന്താക്കർമ്മം `<think>...</think>` ടാഗുകളിൽ പൊതിഞ്ഞ ശേഷം അന്തിമ ഉത്തരം നൽകുന്നു. ഇത് നിരവധി ഘട്ടങ്ങളുള്ള ലജിക്, ഗണിതം, പ്രശ്നപരിഹാരങ്ങൾ ആവശ്യമായ ജോലികൾക്ക് ഉപയോഗപ്രദമാണ്.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning ഏകദേശം 4.6 GB ആണ്
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# മോഡൽ അതിന്റെ ചിന്തനങ്ങൾ <think>...</think> ടാഗുകളുമായി സംരക്ഷിക്കുന്നു
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

// ചൈന്അഫ്-തോട്ട് ചിന്ത വ്യക്തമാക്കുക
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **റീസണിംഗ് മോഡലുകൾ ഉപയോഗിക്കേണ്ടപ്പോൾ:**
> - ഗണിതം, ലജിക് പ്രശ്നങ്ങൾ
> - ഒന്നിലധികം ഘട്ടങ്ങളിലുള്ള പദ്ധതികൾ
> - സങ്കീർണ്ണമായ കോഡ് ജനറേഷൻ
> - പ്രവര്‍ത്തനം കാണിക്കുന്നത് കൃത്യത മെച്ചപ്പെടുത്തുന്ന ജോലികൾ
>
> **വ്യാപാരി:** റീസണിംഗ് മോഡലുകൾ കൂടുതൽ ടോക്കണുകൾ ( `<think>` ഭാഗം) ഉത്പാദിപ്പിച്ച് മന്ദഗതിയുള്ളവയാണ്. ലളിതമായ Q&Aക്ക് phi-3.5-mini പോലൊരു സാധാരണ മോഡൽ വേഗമേറിയതാണ്.

---

### Exercise 11: അലയാസുകളും ഹാർഡ്‌വെയർ തിരഞ്ഞെടുപ്പും മനസ്സിലാക്കുക

നിങ്ങൾ **അലയാസ്** (ഉദാ: `phi-3.5-mini`) പാസ്സ് ചെയ്യുമ്പോൾ SDK നിങ്ങളുടെ ഹാർഡ്‌വെയറിന് ഉചിതമായ മികച്ച വേരിയന്റ് സ്വയം തിരഞ്ഞെടുക്കും:

| ഹാർഡ്‌വെയർ | തിരഞ്ഞെടുക്കപ്പെട്ട എക്സിക്യൂഷൻ പ്രൊവൈഡർ |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML പ്ലഗിൻ വഴി) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| ഏതൊരു ഉപകരണം (ഫോൾബാക്ക്) | `CPUExecutionProvider` അല്ലെങ്കിൽ `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ഈ ആലിയാസ് നിങ്ങളുടെ ഹാർഡ്‌വെയറിനുള്ള بہترین വെരിയന്റ് കണ്ടെത്തുന്നു
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **ടിപ്പ്:** നിങ്ങളുടെ ആപ്ലിക്കേഷൻ കോഡിൽ എപ്പോഴും അലയാസുകൾ ഉപയോഗിക്കണം. ഉപയോക്താവിന്റെ മെഷീനിലേക്കുള്ള ഡിപ്പ്ലോയ്മെന്റ് സമയത്ത് SDK റൺടൈമിൽ മികച്ച വേരിയന്റ് സ്വയം തെരഞ്ഞെടുക്കും - NVIDIA-യിൽ CUDA, Qualcomm-ലേക്ക് QNN, മറ്റു സ്ഥലങ്ങളിൽ CPU.

---

### Exercise 12: C# കോൺഫിഗറേഷൻ ഓപ്ഷനുകൾ

C# SDKയുടെ `Configuration` ക്ലാസ് റൺടൈം നിയന്ത്രണത്തിന് സൂക്ഷ്മ നിയന്ത്രണങ്ങൾ നൽകുന്നു:

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

| പ്രോപ്പർട്ടി | ഡീഫോൾട്ട് | വിവരണം |
|----------|---------|-------------|
| `AppName` | (ആവശ്യമാണ്) | നിങ്ങളുടെ ആപ്ലിക്കേഷന്റെ പേര് |
| `LogLevel` | `Information` | ലോഗിംഗ് verbosity |
| `Web.Urls` | (ഡൈനാമിക്) | വെബ് സെർവർക്ക് പ്രത്യേക പോർട്ട് പിൻ ചെയ്യുക |
| `AppDataDir` | OS ഡീഫോൾട്ട് | ആപ്പ് ഡേറ്റയുടെ അടിസ്ഥാന ഡയറക്ടറി |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | മോഡലുകൾ സൂക്ഷിക്കുന്നിടം |
| `LogsDir` | `{AppDataDir}/logs` | ലോഗുകൾ എഴുതുന്നിടം |

---

### Exercise 13: ബ്രൗസർ ഉപയോഗം (JavaScript മാത്രം)

JavaScript SDK ഒരു ബ്രൗസർ-പൊരുത്തമുള്ള പതിപ്പ് ഉൾക്കൊള്ളുന്നു. ബ്രൗസറിൽ CLI വഴിയായുള്ള സർവീസ് മാനുവലായി ആരംഭിച്ച് ഹോസ്റ്റ് URL നിർദ്ദേശിക്കണം:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// സേവനം ആദ്യമായി കൈമാറി തുടങ്ങുക:
//   foundry സേവനം ആരംഭിക്കുക
// പിന്നെ CLI ഔട്ട്പുട്ടിൽ നിന്നുള്ള URL ഉപയോഗിക്കുക
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// കാറ്റലോഗ് ബ്രൗസ് ചെയ്യുക
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **ബ്രൗസർ പരിധികൾ:** ബ്രൗസർ പതിപ്പ് `startWebService()` പിന്തുണയ്ക്കുന്നില്ല. SDK ബ്രൗസറിൽ ഉപയോഗിക്കുന്നതിന് മുൻപ് Foundry Local സർവീസ് നിലവിലുണ്ടെന്ന് ഉറപ്പുവരുത്തണം.

---

## പൂർണ്ണ API റഫറൻസ്

### Python

| വിഭാഗം | മെഥഡ് | വിവരണം |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | മാനേജർ സൃഷ്‌ടിക്കുക; ഓപ്ഷണായി മോഡൽ ബൂട്ട്‌സ്ട്രാപ്പ് ചെയ്യുന്നു |
| **Service** | `is_service_running()` | സർവീസ് പ്രവർത്തിച്ചുകൊണ്ടിരിക്കുകയാണോ പരിശോധിക്കുക |
| **Service** | `start_service()` | സർവീസ് ആരംഭിക്കുക |
| **Service** | `endpoint` | API എൻഡ്‌പോയിന്റ് URL |
| **Service** | `api_key` | API കീ |
| **Catalog** | `list_catalog_models()` | ലഭ്യമായ എല്ലാ മോഡലുകൾ പട്ടിക ചെയ്യുക |
| **Catalog** | `refresh_catalog()` | ക്യാറ്റലോഗ് പുതുക്കുക |
| **Catalog** | `get_model_info(alias_or_model_id)` | മോഡൽ മെടാഡേറ്റാ നേടുക |
| **Cache** | `get_cache_location()` | കാഷെ ഡയറക്ടറി പാത |
| **Cache** | `list_cached_models()` | ഡൗൺലോഡ് ചെയ്ത മോഡലുകൾ പട്ടിക ചെയ്യുക |
| **Model** | `download_model(alias_or_model_id)` | ഒരു മോഡൽ ഡൗൺലോഡ് ചെയ്യുക |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | ഒരു മോഡൽ ലോഡ് ചെയ്യുക |
| **Model** | `unload_model(alias_or_model_id)` | ഒരു മോഡൽ അൺലോഡ് ചെയ്യുക |
| **Model** | `list_loaded_models()` | ലോഡ് ചെയ്ത മോഡലുകളുടെ പട്ടിക |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | ഒരു പുതിയ പതിപ്പ് ലഭ്യമാണോ പരിശോധിക്കുക |
| **Model** | `upgrade_model(alias_or_model_id)` | മോഡൽ ഏറ്റവും പുതിയ പതിപ്പിലേക്ക് അപ്ഗ്രേഡ് ചെയ്യുക |
| **Service** | `httpx_client` | API കാൾസ് നേരിട്ട് ചെയ്യാനുള്ള പ്രീ-കോൺഫിഗർ ചെയ്ത HTTPX ക്ലയന്റ് |

### JavaScript

| വിഭാഗം | മെഥഡ് | വിവരണം |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | SDK സിംഗിൾട്ടൻ ആരംഭിക്കുക |
| **Init** | `FoundryLocalManager.instance` | സിംഗിൾട്ടൻ മാനേജർ ആക്‌സസ് ചെയ്യുക |
| **Service** | `manager.startWebService()` | വെബ് സർവീസ് ആരംഭിക്കുക |
| **Service** | `manager.urls` | സർവീസിന്റെ അടിസ്ഥാന URL ലിസ്റ്റ് |
| **Catalog** | `manager.catalog` | മോഡൽ ക്യാറ്റലോഗ് ആക്‌സസ് ചെയ്യുക |
| **Catalog** | `catalog.getModel(alias)` | അലയാസ് മുഖേന മോഡൽ ഒൽജികെ (പ്രോമിസ് അഭ്യർത്ഥന) |
| **Model** | `model.isCached` | മോഡൽ ഡൗൺലോഡ് ചെയ്തിട്ടുണ്ടോ എന്ന് |
| **Model** | `model.download()` | മോഡൽ ഡൗൺലോഡ് ചെയ്യുക |
| **Model** | `model.load()` | മോഡൽ ലോഡ് ചെയ്യുക |
| **Model** | `model.unload()` | മോഡൽ അൺലോഡ് ചെയ്യുക |
| **Model** | `model.id` | മോഡലിന്റെ വിശിഷ്ട ഐഡി |
| **Model** | `model.alias` | മോഡലിന്റെ അലയാസ് |
| **Model** | `model.createChatClient()` | നാടിവ് ചാറ്റ് ക്ലയന്റ് നേടുക (OpenAI SDK ആവശ്യപ്പെട്ടിട്ടില്ല) |
| **Model** | `model.createAudioClient()` | ട്രാൻസ്ക്രിപ്ഷനിനുള്ള ഓഡിയോ ക്ലയന്റ് |
| **Model** | `model.removeFromCache()` | ലോക്കൽ കാഷെയിൽ നിന്നുമു മോഡൽ നീക്കം ചെയ്യുക |
| **Model** | `model.selectVariant(variant)` | ഒരു പ്രത്യേക ഹാർഡ്‌വെയർ വേരിയന്റ് തിരഞ്ഞെടുക്കുക |
| **Model** | `model.variants` | ഈ മോഡലിനുള്ള ലഭ്യമായ വേരിയന്റുകൾ പട്ടിക |
| **Model** | `model.isLoaded()` | മോഡൽ നിലവിൽ ലോഡ് ചെയ്തിട്ടുണ്ട് എന്നറിയുക |
| **Catalog** | `catalog.getModels()` | ലഭ്യമായ എല്ലാ മോഡലുകളും പട്ടിക ചെയ്യുക |
| **Catalog** | `catalog.getCachedModels()` | ഡൗൺലോഡ് ചെയ്ത മോഡലുകൾ തിരി‍ക്കും |
| **Catalog** | `catalog.getLoadedModels()` | നിലവിൽ ലോഡ് ചെയ്ത മോഡൽ പട്ടിക |
| **Catalog** | `catalog.updateModels()` | സർവീസ് നിന്ന് ക്യാറ്റലോഗ് പുതുക്കുക |
| **Service** | `manager.stopWebService()` | Foundry Local വെബ് സർവീസ് നിർത്തുക |

### C# (v0.8.0+)

| വിഭാഗം | മെഥഡ് | വിവരണം |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | മാനേജർ ആരംഭിക്കുക |
| **Init** | `FoundryLocalManager.Instance` | സിംഗിൾട്ടൻ ആക്‌സസ് ചെയ്യുക |
| **Catalog** | `manager.GetCatalogAsync()` | ക്യാറ്റലോഗ് നേടുക |
| **Catalog** | `catalog.ListModelsAsync()` | എല്ലാ മോഡലുകളും പട്ടിക ചെയ്യുക |
| **Catalog** | `catalog.GetModelAsync(alias)` | ഒരു പ്രത്യേക മോഡൽ നേടുക |
| **Catalog** | `catalog.GetCachedModelsAsync()` | കാഷെ ചെയ്ത മോഡലുകളുടെ പട്ടിക |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | ലോഡ് ചെയ്ത മോഡലുകൾ |
| **Model** | `model.DownloadAsync(progress?)` | മോഡൽ ഡൗൺലോഡ് ചെയ്യുക |
| **Model** | `model.LoadAsync()` | മോഡൽ ലോഡ് ചെയ്യുക |
| **Model** | `model.UnloadAsync()` | മോഡൽ അൺലോഡ് ചെയ്യുക |
| **Model** | `model.SelectVariant(variant)` | ഹാർഡ്‌വെയർ വേരിയന്റ് തിരഞ്ഞെടുക്കുക |
| **Model** | `model.GetChatClientAsync()` | നാടിവ് ചാറ്റ് ക്ലയന്റ് നേടുക |
| **Model** | `model.GetAudioClientAsync()` | ഓഡിയോ ട്രാൻസ്ക്രിപ്ഷൻ ക്ലയന്റ് |
| **Model** | `model.GetPathAsync()` | ലോക്കൽ ഫയൽ പാത |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | പ്രത്യേക ഹാർഡ്‌വെയർ വേരിയന്റ് നേടുക |
| **Catalog** | `catalog.UpdateModelsAsync()` | ക്യാറ്റലോഗ് പുതുക്കുക |
| **Server** | `manager.StartWebServerAsync()` | REST വെബ് സെർവർ ആരംഭിക്കുക |
| **Server** | `manager.StopWebServerAsync()` | REST വെബ് സെർവർ നിർത്തുക |
| **Config** | `config.ModelCacheDir` | കാഷെ ഡയറക്ടറി |

---

## പ്രധാന കാര്യങ്ങൾ

| ആശയം | നിങ്ങൾ പഠിച്ചത് |
|---------|-----------------|
| **SDK vs CLI** | SDK പ്രോഗ്രാമാറ്റിക് നിയന്ത്രണം നൽകുന്നു - ആപ്ലിക്കേഷനുകൾക്ക് അനിവാര്യമാണ് |
| **നിയന്ത്രണം** | SDK സർവീസുകൾ, മോഡലുകൾ, കാഷിംഗ് നിയന്ത്രിക്കുന്നു |
| **ഡൈനാമിക് പോർട്ടുകൾ** | എല്ലായ്പ്പോഴും `manager.endpoint` (Python) അല്ലെങ്കിൽ `manager.urls[0]` (JS/C#) ഉപയോഗിക്കുക - പോർട്ട് ഹാർഡ്‌കോഡ് ചെയ്യരുത് |
| **അലയാസുകൾ** | സ്വയം ഹാർഡ്‌വെയർ-ഇതിഹാസ മോഡൽ തിരഞ്ഞെടക്കാനുള്ള അലയാസുകൾ ഉപയോഗിക്കുക |
| **ക്വിക്ക്-സ്റ്റാർട്ട്** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# പുനർഡിസൈൻ** | v0.8.0+ സ്വയംപൂരിതമാണ് - end-user യന്ത്രങ്ങളിൽ CLI ആവശ്യമായില്ല |
| **മോഡൽ ലൈഫ്‌സൈക്കിൾ** | Catalog → ഡൗൺലോഡ് → ലോഡ് → ഉപയോഗിക്കുക → അന്ലോഡ് |
| **FoundryModelInfo** | സമ്പന്നമായ മെറ്റാഡേറ്റ: ടാസ്ക്, ഡിവൈസ്, വലുപ്പം, ലൈസൻസ്, ടൂൾ വിളിക്കുന്നത് പിന്തുണക്കുന്നു |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) OpenAI-മോചന ഉപയോഗത്തിന് |
| **ഭിന്നതകൾ** | മോഡലുകൾക്ക് ഹാർഡ്‌വെയർ-നിർദ്ദിഷ്ടമായ ഭിന്നതകൾ ഉണ്ട് (CPU, GPU, NPU); സ്വയം തിരഞ്ഞെടുക്കപ്പെടുന്നു |
| **അപ്ഗ്രേഡുകൾ** | Python: `is_model_upgradeable()` + `upgrade_model()` മോഡലുകൾ പുതുക്കാൻ |
| **Catalog നവീകരണം** | `refresh_catalog()` (Python) / `updateModels()` (JS) പുതിയ മോഡലുകൾ കണ്ടെത്താൻ |

---

## സ്രോതസ്സുകൾ

| സ്രോതസ്സ് | ലിങ്ക് |
|----------|------|
| SDK റഫറൻസ് (എല്ലാ ഭാഷകളും) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| ഇൻഫറൻസ് SDK-കളുമായി സംയോജിപ്പിക്കുക | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API റഫറൻസ് | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK സാമ്പിൾ | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local വെബ്സൈറ്റ് | [foundrylocal.ai](https://foundrylocal.ai) |

---

## അടുത്ത പടികൾ

SDK-യെ OpenAI ക്ലയന്റ് ലൈബ്രറിയുമായി ബന്ധിപ്പിച്ച് നിങ്ങളുടെ ആദ്യ ചാറ്റ് പൂർത്തീകരണ അപ്ലിക്കേഷൻ നിർമ്മിക്കാൻ [ഭാഗം 3: SDK ഉപയോഗിച്ച് OpenAI](part3-sdk-and-apis.md) തുടരണം.