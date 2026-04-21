![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# အပိုင်း ၂: Foundry Local SDK ပို၍ စူးစမ်းလေ့လာခြင်း

> **ရည်ရွယ်ချက်:** Foundry Local SDK ကိုကျွမ်းကျင်စွာ အသုံးပြု၍ မော်ဒယ်များ၊ ဝန်ဆောင်မှုများနှင့် ကက်ရှ်များကို စီမံခန့်ခွဲရန် - နှင့် SDK ကို CLI ထက် တိုက်ရိုက်လမ်းကြောင်းအဖြစ် ဆောက်လုပ်မှုများတွင် ဘာကြောင့် မှတ်ချက်အာဏာရှိသည်ကို နားလည်ခြင်း။

## အနှစ်ချုပ်

အစပိုင်း ၁ တွင် သင်သည် **Foundry Local CLI** ကို အသုံးပြု၍ မော်ဒယ်များကို ဒေါင်းလုပ်ဆွဲခြင်းနှင့် နိုင်ငံလက်ခံ Run ခြင်း ပြုလုပ်ခဲ့သည်။ CLI သည် ရှာဖွေရေးအတွက် အထူးကောင်းမွန်သော်လည်း စိတ်ကူးရင်း အခြေခံ၍ အမှန်တကယ် ဆော့ဖ်ဝဲတွေ ဆောက်လုပ်သောအခါ **ပရိုဂရမ်မတ် လမ်းညွှန်မှု** လိုအပ်သည်။ Foundry Local SDK သည် ထိုအရာကို သင့်အားပေးသည် - ၎င်းသည် **control plane** ကို စီမံကိန်းပြု (ဝန်ဆောင်မှု စတင်ခြင်း၊ မော်ဒယ် ရှာဖွေခြင်း၊ ဒေါင်းလုပ်ဆွဲခြင်း၊ loading) သွား၍ သင့် application code သည် **data plane** (prompt များ ပို့ခြင်း၊ ဖြေကြားမှုများ လက်ခံခြင်း) မှာအာရုံစိုက်နိုင်သည်။

ဤလက်တွေ့သင်ခန်းစာသည် Python, JavaScript နှင့် C# အတိုင်း SDK API ပြည့်စုံကို သင်ကြားပေးမည်ဖြစ်သည်။ အဆုံးတွင် သင်သည် တိုက်ရိုက် အသုံးပြုနိုင်သော နည်းလမ်းအားလုံးနဲ့ ဘယ်အခါသုံးရမည်ကို နားလည်ပါမည်။

## သင်ယူရမည့် ရည်ရွယ်ချက်များ

ဤလက်တွေ့သင်ခန်းစာ၏ အဆုံးတွင် သင်အောက်ပါလုပ်ဆောင်နိုင်မည်ဖြစ်သည်-

- အကာအရံခြေ CLI ထက် SDK ကို ဘာကြောင့် ရွေးချယ်သင့်ကြောင်းရှင်းပြနိင်ရန်
- Python, JavaScript သို့မဟုတ် C# အတွက် Foundry Local SDK ထည့်သွင်းရန်
- `FoundryLocalManager` ကို အသုံးပြု၍ ဝန်ဆောင်မှု စတင်ခြင်း၊ မော်ဒယ်များ စီမံခြင်းနှင့် ကတ်တလော့ ကို ရှာဖွေရန်
- မော်ဒယ်များကို စာရင်းပြုစုခြင်း၊ ဒေါင်းလုပ်ဆွဲခြင်း၊ load နှင့် unload ကို ပရိုဂရမ်ဖြင့် ထိန်းချုပ်နိုင်ရန်
- `FoundryModelInfo` ကို အသုံးပြု၍ မော်ဒယ် metadata ကို စစ်ဆေးနားလည်ရန်
- ကတ်တလော့၊ ကက်ရှ်နှင့် load လုပ်ထားသော မော်ဒယ်များ၏ ကွာခြားချက်ကို နားလည်ရန်
- Constructor bootstrap (Python) နှင့် `create()` + ကတ်တလော့ ပုံစံ (JavaScript) ကို အသုံးပြုတတ်ရန်
- C# SDK ပြန်လည် ဒီဇိုင်းနှင့် ၎င်း၏ အရာဝတ္ထုပြု API ကို နားလည်ရန်

---

## မပြင်ဆင်မီ လိုအပ်ချက်များ

| လိုအပ်ချက် | အသေးစိတ် |
|-------------|---------|
| **Foundry Local CLI** | သင့် PATH တွင် ထည့်သွင်းပြီး ([Part 1](part1-getting-started.md)) |
| **ဘာသာစကား runtime** | **Python 3.9+** နှင့်/သို့မဟုတ် **Node.js 18+** နှင့်/သို့မဟုတ် **.NET 9.0+** |

---

## အကြောင်းအရာ: SDK vs CLI - SDK ကို ဘာကြောင့်သုံးရမလဲ?

| အချက် | CLI (`foundry` command) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **အသုံးပြုမှုအမျိုးအစား** | ရှာဖွေခြင်း၊ လက်စွဲ စမ်းသပ်ခြင်း | အပလီကေးရှင်း ထည့်သွင်းခြင်း |
| **ဝန်ဆောင်မှု စီမံခန့်ခွဲမှု** | လက်အောက်တွင် `foundry service start` | အလိုအလျောက် `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **ပေါ့တ်ဖွင့်ရှာဖွေခြင်း** | CLI output မှ ဖတ်ရန် | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **မော်ဒယ် ဒေါင်းလုပ်ဆွဲခြင်း** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **အမှားကိုင်တွယ်ခြင်း** | ထွက်ခွာကုဒ်များ၊ stderr | Exceptions, အမျိုးမျိုးသောအမှားများ |
| **အလိုအလျောက် smart automation** | Shell script များ | မူလ ဘာသာစကား ထည့်သွင်းမှု |
| **တပ်ဆင်ခြင်း** | user machine တွင် CLI လိုအပ်သည် | C# SDK သည် self-contained (CLI လိုအပ်မှု မရှိ) |

> **အချက်သတိထားရန်:** SDK သည် ဝန်ဆောင်မှု စတင်ခြင်း၊ cache စစ်ဆေးခြင်း၊ မရှိသေးသော မော်ဒယ်များ ဒေါင်းလုပ်ဆွဲခြင်း၊ load လုပ်ခြင်းနှင့် endpoint ရှာဖွေခြင်းတို့အား အနည်းငယ်သော ကုဒ်လိုင်းများအတွင်း ပြုလုပ်ပေးသည်။ သင့် အပလီကေးရှင်းသည် CLI output ကို parse လုပ်ရန် သို့မဟုတ် subprocess များထိန်းချုပ်ရန် မလိုအပ်ပါ။

---

## လက်တွေ့လေ့ကျင့်ခန်းများ

### လေ့ကျင့်ခန်း ၁: SDK ထည့်သွင်းခြင်း

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

ထည့်သွင်းမှု မှန်ကန်မှု ရယူရန်:

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

ထည့်သွင်းမှု မှန်ကန်မှု ရယူရန်:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

NuGet package နှစ်ခု ရှိသည်-

| Package | ပလက်ဖောင်း | အသေးစိတ် |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Cross-platform | Windows, Linux, macOS တွင် အလုပ်လုပ်သည် |
| `Microsoft.AI.Foundry.Local.WinML` | Windows အတွက်သာ | WinML hardware အရှိဆုံးမြှင့်တင်မှု; plugin execution providers များ (ဥပမာ Qualcomm NPU အတွက် QNN) ကို ဒေါင်းလုပ်ဆွဲပြုလုပ်သည် |

**Windows ပြင်ဆင်မှု:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` ဖိုင်ကို ပြင်ဆင်ပါ-

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

> **မှတ်ချက်:** Windows တွင် WinML package သည် base SDK နှင့် QNN execution provider ကို ပါဝင်သည့် superset ဖြစ်သည်။ Linux/macOS တွင် base SDK ကိုအသုံးပြုသည်။ TFM နှင့် package reference များ သတ်မှတ်၍ စာရင်းကိုယ်တွေ့ရာ ပလက်ဖောင်း ရိုးရာများအားလုံးကို ဆောင်ရွက်နိုင်ရန်။

Project root တွင် `nuget.config` ဖိုင်ဖန်တီးပါ-

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

Packages ပြန်လည်ထည့်သွင်းခြင်း-

```bash
dotnet restore
```

</details>

---

### လေ့ကျင့်ခန်း ၂: ဝန်ဆောင်မှု စတင်၍ ကတ်တလော့ စာရင်းကြည့်ခြင်း

မည်သည့် အပလီကေးရှင်းမဆို ပထမဆုံး အလုပ်ဆောင်ရာမှာ Foundry Local ဝန်ဆောင်မှု စတင်ပြီး မော်ဒယ်လ်များ ရှိ/မရှိ စစ်ဆေးသည်။

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# မန်နေဂျာတစ်ခုဖန်တီးပြီး စတင်ဝန်ဆောင်မှုကို စတင်ပါ
manager = FoundryLocalManager()
manager.start_service()

# ကတ်တလော့ရှိ မော်ဒယ်အားလုံးကို စာရင်းပြုစုပါ
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - ဝန်ဆောင်မှု စီမံခန့်ခွဲမှု နည်းလမ်းများ

| နည်းလမ်း | Signature | အသေးစိတ် |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | ဝန်ဆောင်မှု လည်ပတ်နေ/မနေ စစ်ဆေးခြင်း |
| `start_service()` | `() -> None` | Foundry Local ဝန်ဆောင်မှု စတင်ခြင်း |
| `service_uri` | `@property -> str` | ဝန်ဆောင်မှု၏ အခြေခံ URI |
| `endpoint` | `@property -> str` | API endpoint (service URI + `/v1`) |
| `api_key` | `@property -> str` | API key (env ထဲမှ သို့မဟုတ် ဇယား နေရာချထားမှု) |

#### Python SDK - ကတ်တလော့ စီမံခန့်ခွဲမှု နည်းလမ်းများ

| နည်းလမ်း | Signature | အသေးစိတ် |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | ကတ်တလော့ ထဲမော်ဒယ်များ စာရင်းပြုစုခြင်း |
| `refresh_catalog()` | `() -> None` | ဝန်ဆောင်မှုမှ ကတ်တလော့ အသစ်ရယူခြင်း |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | မော်ဒယ် တစ်ခု ရှာဖွေရန် |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// မန်နေဂျာတစ်ဦး ဖန်တီးပြီး ဝန်ဆောင်မှုကို စတင်ပါ
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// စာရင်းဇယားကို ကြည့်ရှုပါ
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Manager နည်းလမ်းများ

| နည်းလမ်း | Signature | အသေးစိတ် |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK singleton ကို စတင်ဖြည့်စွက်ခြင်း |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | singleton manager ကို ဝင်ရောက်အသုံးပြုခြင်း |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local web service ကို စတင်ခြင်း |
| `manager.urls` | `string[]` | ဝန်ဆောင်မှု၏ အခြေခံ URL များ စုစည်းမှု |

#### JavaScript SDK - ကတ်တလော့နှင့် မော်ဒယ် နည်းလမ်းများ

| နည်းလမ်း | Signature | အသေးစိတ် |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | မော်ဒယ် ကတ်တလော့ကို ဝင်ရောက်လေ့လာခြင်း |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | ဂုဏ်အမည်ဖြင့် မော်ဒယ် တစ်ခု ရယူခြင်း |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ သည် object-oriented အထောက်အထားဖြင့် `Configuration`, `Catalog` နှင့် `Model` အရာဝတ္ထုများကို အသုံးပြုသည်-

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

#### C# SDK - အဓိက တန်းတူများ

| တန်းတူ | ရည်ရွယ်ချက် |
|-------|---------|
| `Configuration` | app အမည်၊ log အဆင့်၊ cache ဒီရက်্টရီ၊ web ဆာဗာ URL များ သတ်မှတ်ရန် |
| `FoundryLocalManager` | အဓိက ဝင်ကြောင်း - `CreateAsync()` ဖြင့် ဖန်တီးပြီး `.Instance` ဖြင့် ဝင်ရောက်မည် |
| `Catalog` | ကတ်တလော့ မော်ဒယ်များ စူးစမ်းရှာဖွေခြင်း |
| `Model` | တိကျသော မော်ဒယ် တစ်ခုကို ကိုယ်စားပြု - ဒေါင်းလုပ်ဆွဲ၊ load၊ client ရယူခြင်း |

#### C# SDK - Manager နှင့် ကတ်တလော့ နည်းလမ်းများ

| နည်းလမ်း | အသေးစိတ် |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | manager စတင်ဖန်တီးခြင်း |
| `FoundryLocalManager.Instance` | singleton manager ကို ဝင်ရောက်အသုံးပြုခြင်း |
| `manager.GetCatalogAsync()` | မော်ဒယ် ကတ်တလော့ကို ရယူခြင်း |
| `catalog.ListModelsAsync()` | ရနိုင်သော မော်ဒယ်များ စာရင်းပြုစုခြင်း |
| `catalog.GetModelAsync(alias: "alias")` | တိကျသော အမည်ဖြင့် မော်ဒယ် ရယူခြင်း |
| `catalog.GetCachedModelsAsync()` | ဒေါင်းလုပ်ဆွဲထားသော မော်ဒယ်များ စာရင်း |
| `catalog.GetLoadedModelsAsync()` | လောလောဆယ် load ယူထားသော မော်ဒယ်များ စာရင်း |

> **C# ဒီဇိုင်း မှတ်ချက်:** C# SDK v0.8.0+ ပြန်လည် ဒီဇိုင်းသည် အပလီကေးရှင်းကို self-contained ဖြစ်စေသည်; end-user machine တွင် Foundry Local CLI မလိုအပ်ပါ။ SDK သည် မော်ဒယ် စီမံခန့်ခွဲမှုနှင့် inference ကို နိုင်ငံလက်ခံ native ဖြစ်စေရန် တာဝန်ယူသည်။

</details>

---

### လေ့ကျင့်ခန်း ၃: မော်ဒယ် ဒေါင်းလုပ်ဆွဲပြီး Load ခ်ရန်

SDK သည် ဒေါင်းလုပ်ခြင်း (disk ပေါ်သိုလှောင်ခြင်း) နှင့် load ချခြင်း (memory သို့) ကို ခွဲခြားထားသည်။ ၎င်းက မော်ဒယ်များကို စတင်သုံးစွဲစဉ်တွင် ဒေါင်းလုပ်ဆွဲထားပြီး နောက်မှလိုအပ်ချက်အလိုက် load လုပ်နိုင်ရေး အဆင်ပြေသည်။

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# ရွေးချယ်မှု A: လက်ဖြင့် လှုပ်ရှားမှု အဆင့်-အဆင့်
manager = FoundryLocalManager()
manager.start_service()

# ပထမဦးစွာ ကက်ရှ်စစ်ဆေးပါ
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

# ရွေးချယ်မှု B: တစ်ကြောင်းနဲ့ bootstrap (အကြံပြု)
# alias ကို constructor ထဲ ပေးပို့ပါ - ၎င်းသည် ဝန်ဆောင်မှုကို စတင်ပြီး ဒေါင်းလုပ်လုပ်ကာ တင်သွင်းပေးပါသည်
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - မော်ဒယ် စီမံခန့်ခွဲမှု နည်းလမ်းများ

| နည်းလမ်း | Signature | အသေးစိတ် |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | မော်ဒယ်ကို ဒေသခံ cache သို့ ဒေါင်းလုပ်ဆွဲရန် |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | မော်ဒယ်ကို inference server တွင် load ကာ အသုံးပြုရန် |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | inference server မှ မော်ဒယ်ကို unload ရန် |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | ယခု load ရှိသော မော်ဒယ်များ စာရင်း |

#### Python - Cache စီမံခန့်ခွဲမှု နည်းလမ်းများ

| နည်းလမ်း | Signature | အသေးစိတ် |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | cache directory ရဲ့ path ရယူရန် |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | ဒေါင်းလုပ်ထားသော မော်ဒယ်များ စာရင်း |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// လိုက်လျောညီထွေပြီးအဆင့်ဆင့်လုပ်ဆောင်မည့်နည်းလမ်း
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

#### JavaScript - မော်ဒယ် နည်းလမ်းများ

| နည်းလမ်း | Signature | အသေးစိတ် |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | မော်ဒယ် ဒေါင်းလုပ်ဆွဲပြီး/မဆွဲရသေး ကို ပြသခြင်း |
| `model.download()` | `() => Promise<void>` | ဒေသခံ cache သို့ မော်ဒယ်ကို ဒေါင်းလုပ်ဆွဲခြင်း |
| `model.load()` | `() => Promise<void>` | inference server တွင် load ချခြင်း |
| `model.unload()` | `() => Promise<void>` | inference server မှ unload ချခြင်း |
| `model.id` | `string` | မော်ဒယ်၏ ထူးခြားရှိ unique ID |

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

#### C# - မော်ဒယ် နည်းလမ်းများ

| နည်းလမ်း | အသေးစိတ် |
|--------|-------------|
| `model.DownloadAsync(progress?)` | ရွေးချယ်ထားသော variant ကို ဒေါင်းလုပ်ဆွဲခြင်း |
| `model.LoadAsync()` | မော်ဒယ်ကို memory ထဲ load ချခြင်း |
| `model.UnloadAsync()` | မော်ဒယ်ကို unload ချခြင်း |
| `model.SelectVariant(variant)` | တိကျသည့် variant (CPU/GPU/NPU) ရွေးချယ်ခြင်း |
| `model.SelectedVariant` | လောလောဆယ် ရွေးထားသော variant |
| `model.Variants` | မော်ဒယ်အတွက် ရနိုင်သော variant အားလုံး |
| `model.GetPathAsync()` | ဒေသခံ ဖိုင်လမ်းကြောင်း ရယူခြင်း |
| `model.GetChatClientAsync()` | native chat client ရယူခြင်း (OpenAI SDK မလိုအပ်) |
| `model.GetAudioClientAsync()` | transcription အတွက် audio client ရယူခြင်း |

</details>

---

### လေ့ကျင့်ခန်း ၄: မော်ဒယ် Metadata စစ်ဆေးခြင်း

`FoundryModelInfo` အရာဝတ္ထုတွင် မော်ဒယ် တစ်ခုစီ ဆိုင်ရာ metadata မြင့်မားစွာ ပါဝင်သည်။ ဤကွက်လပ်များကို နားလည်ခြင်းဖြင့် သင့် application အတွက် သင့်တော်သော မော်ဒယ်ကို ရွေးချယ်နိုင်သည်။

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# တိတိကျကျ မော်ဒယ်တစ်ခုအကြောင်း အချက်အလက်အသေးစိတ်ရယူပါ
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

#### FoundryModelInfo ကွက်လပ်များ

| ကွက် | အမျိုးအစား | အသေးစိတ် |
|-------|------|-------------|
| `alias` | string | ဂုဏ်အမည်တို (ဥပမာ `phi-3.5-mini`) |
| `id` | string | မော်ဒယ်၏ ထူးခြားသော ID |
| `version` | string | မော်ဒယ် ဗားရှင်း |
| `task` | string | `chat-completions` သို့မဟုတ် `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU သို့မဟုတ် NPU |
| `execution_provider` | string | Runtime backend (CUDA, CPU, QNN, WebGPU စသည်) |
| `file_size_mb` | int | ဖိုင်၏ အရွယ်အစား (MB) |
| `supports_tool_calling` | bool | မော်ဒယ်မှ function/tool calling ကို ထောက်ပံ့သည်/မထောက်ပံ့သည် |
| `publisher` | string | မော်ဒယ်ကို ထုတ်ဝေသူ |
| `license` | string | အသုံးပြုခွင့် နာမည် |
| `uri` | string | မော်ဒယ် URI |
| `prompt_template` | dict/null | Prompt ဖော်စပ်ပုံ (ရှိပါက) |

---

### လေ့ကျင့်ခန်း ၅: မော်ဒယ် Life Cycle ကို စီမံခန့်ခွဲခြင်း

လုံးဝ life cycle ကို လေ့ကျင့်ပါ- စာရင်းပြု → ဒေါင်းလုပ်ဆွဲ → load → အသုံးပြု → unload။

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # အမြန်စမ်းသပ်ရန် အသေး מודယ်

manager = FoundryLocalManager()
manager.start_service()

# 1. ဒေတာစာရင်းတွင် ဘာရှိလဲ စစ်ဆေးပါ
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. ရှိပြီးသားဒေတာများကို စစ်ဆေးပါ
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. မော်ဒယ်တစ်ခုဒေါင်းလုပ်လုပ်ပါ
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. ယခုဖိုင်သိုလှောင်ထားမှုတွင် တည်ရှိကြောင်း အတည်ပြုပါ
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. အသုံးပြုဖို့ ထည့်ပါ
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. မည်သည်များထည့်ထားကြောင်း စစ်ဆေးပါ
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. ဖယ်ရှားပါ
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

const alias = "qwen2.5-0.5b"; // လျင်မြန်စွာ စမ်းသပ်ရန် အသေးစား မော်ဒယ်

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. ကတ်တလောမှ မော်ဒယ် ရယူပါ
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. လိုအပ်ပါက ဒေါင်းလုပ်လုပ်ပါ
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. မော်ဒယ်ကို ဖွင့်ပါ
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. မော်ဒယ်ကို ပိတ်ပါ
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### အတွေးလေ့ကျင့်ခန်း ၆: အမြန်အစပြု နမူနာများ

အချို့သော ဘာသာစကားတိုင်းသည် တစ်ခေါက်ခေါ်ပြီး ဝန်ဆောင်မှု စတင်ခြင်းနှင့် ကိုယ်စားလှယ် မော်ဒယ်တင်ခြင်းကို ရရှိစေမည့် လမ်းညွှန်တိုcutsရွိသည်။ သည်မော်ဒယ်များသည် အများဆုံး အပလီကေးရှင်းများအတွက် **အကြံပြုထားသော နမူနာများ** ဖြစ်သည်။

<details>
<summary><h3>🐍 Python - Constructor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# constructor အတွက် alias တစ်ခု ပေးပါ - အားလုံးကို ကိုင်တွယ်ပေးသည်။
# ၁။ အလုပ်မလုပ်ဆဲဆို စနစ်ကို စတင်ပါ။
# ၂။ cache မရှိသေးလျှင် model ကို ဒေါင်းလုပ်လုပ်ပါ။
# ၃။ model ကို inference server ထဲသို့ ဖွင့်ပါ။
manager = FoundryLocalManager("phi-3.5-mini")

# ချက်ချင်း အသုံးပြုနိုင်သည်။
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` ပါရာမီတာ (ပုံမှန် `True`) သည် ဤအပြုအမူကို ထိန်းချုပ်သည်။ အလိုတော်ဖြင့် ထိန်းချုပ်လိုလျှင် `bootstrap=False` ဟု သတ်မှတ်ပါ။

```python
# လက်ဖြည့်စနစ် - ဘာမှအလိုအလျောက်မဖြစ်ပါ။
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() က အရာအားလုံးကို ကြီးကြပ်ပါသည်။
// 1. ဝန်ဆောင်မှုကို စတင်သည်
// 2. စာရင်းမှာ မော်ဒယ်ကို ရယူသည်
// 3. လိုအပ်ပါက ဒေါင်းလုတ်လုပ်ပြီး မော်ဒယ်ကို မောင်းထည့်သည်
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// ချက်ချင်း အသုံးပြုနိုင်သည်
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

> **C# မှတ်ချက်။** C# SDK သည် app နာမည်၊ logging၊ cache ဒိုင်ရေးတရိုရီများနှင့် သီးသန့် web server port ပြင်ဆင်ခြင်းအတွက် `Configuration` ကို သုံးသည်။ ၎င်းကြောင့် သုံး SDK များထဲမှ အလွန် ထိန်းချုပ်နိုင်သည်။

</details>

---

### အတွေးလေ့ကျင့်ခန်း ၇: ကိုယ်ပိုင် ChatClient (OpenAI SDK မလိုအပ်)

JavaScript နှင့် C# SDK များတွင် `createChatClient()` ဆိုသော မိတ်ဆက်လာသောနည်းလမ်းရှိပြီး native chat client ကို ပြန်ပေးသည်။ OpenAI SDK ကို သီးခြား တပ်ဆင်ရန် မလိုအပ်ပါ။

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

// မော်ဒယ်မှတိုက်ရိုက် ChatClient ကိုဖန်တီးပါ — OpenAI import လိုအပ်မှုမရှိပါ
const chatClient = model.createChatClient();

// completeChat သည် OpenAI- အညီ response object ကို 반환ပေးပါသည်
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming သည် callback ပုံစံကို အသုံးပြုသည်
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` သည် tool ခေါ်ဆိုမှုကိုလည်း ပံ့ပိုးသည် — tool များကို ဒုတိယအကြောင်းအရာအဖြစ် ပေးပို့နိုင်သည်။

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

> **ဘယ် pattern ကို အသုံးပြုမလဲ:**
> - **`createChatClient()`** — အမြန် prototype ပြုလုပ်မှု၊ လိုအပ်ချက်နည်းသဖြင့်၊ code ရိုးရှင်းသည့်နည်း
> - **OpenAI SDK** — parameter များကိုပြည့်စုံ ထိန်းချုပ်နိုင်မှု (temperature, top_p, stop token များ), ထုတ်လုပ်မှုအတွက်ပိုသင့်တော်သည်

---

### အတွေးလေ့ကျင့်ခန်း ၈: မော်ဒယ် မတူညီမှုများနှင့် ဟာ့ဒ်ဝဲ ရွေးချယ်မှု

မော်ဒယ်များတွင် ဟာ့ဒ်ဝဲ အမျိုးအစားအလိုက် Optimise လုပ်ထားသည့် **မတူကွဲပြားသည့်** အမျိုးအစားများရှိသည်။ SDK သည် အကောင်းဆုံး မတူကွဲပြားမှုကို အလိုအလျောက်ရွေးချယ်ပေးသော်လည်း သင်ကိုယ်တိုင်လည်း ကြည့်ရှုရွေးချယ်နိုင်ပါသည်။

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// အသုံးပြုနိုင်သော မျိုးကွဲအားလုံးကို စာရင်းပြုစုပါ
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK သည် သင့် hardware အတွက် အကောင်းဆုံး မျိုးကွဲကို အလိုအလျောက် ရွေးချယ်ပေးသည်
// အစားထိုးရန် selectVariant() ကို အသုံးပြုပါ
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

Python တွင် SDK သည် ဟာ့ဒ်ဝဲအလိုက် အကောင်းဆုံး မတူကွဲပြားမှုကို အလိုအလျောက်ရွေးချယ်ပေးသည်။ ရွေးချယ်ထားသော မော်ဒယ်အချက်အလက်ကို `get_model_info()` ဖြင့် ကြည့်ရှုနိုင်ပါသည်။

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

#### NPU မတူကွဲပြားမှု ပါသော မော်ဒယ်များ

နေရန် ပစ္စည်းတစ်ချို့တွင် Neural Processing Unit (Qualcomm Snapdragon, Intel Core Ultra) Optimise လုပ်ထားသည့် NPU မတူကွဲပြားမှုများပါရှိသည်။

| Model | NPU Variant Available |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **အကြံပြုချက်:** NPU ပိုင်ဆိုင်သည့် ဟာ့ဒ်ဝဲတွင် SDK သည် အလိုအလျောက် NPU မတူကွဲပြားမှုကို ရွေးချယ်ပေးသည်။ သင်၏ code ကို ပြောင်းရန် မလိုအပ်ပါ။ Windows ဖြင့် C# လုပ်ငန်းစဉ်များအတွက် `Microsoft.AI.Foundry.Local.WinML` NuGet package ကို ထည့်သွင်းပါက QNN execution provider အသုံးပြုနိုင်မည်၊ QNN သည် WinML မှ ပလပ်အင် EP အဖြစ်ပေးပို့သည်။

---

### အတွေးလေ့ကျင့်ခန်း ၉: မော်ဒယ်မြှင့်တင်ခြင်းနှင့် Catalog အသစ်လွှတ်ခြင်း

မော်ဒယ်ဒေတာဘေ့စ်ကို ကြာကြာ ကြိမ်ပြုလုပ် အသစ်တင်ပေးသည်။ အောက်ပါ နည်းလမ်းများဖြင့် အပ်ဒိတ်ရှိမရှိ စစ်ဆေးပြီး အသုံးပြုနိုင်သည်။

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# နောက်ဆုံးပေါ် မော်ဒယ်စာရင်းရရန် စာရင်းကို ထပ်မံဖြည့်မည်
manager.refresh_catalog()

# သိမ်းဆည်းထားသော မော်ဒယ်တွင် ပိုမိုအသစ်သော ဗားရှင်းရှိမရှိ စစ်ဆေးပါ
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

// နောက်ဆုံးထွက်မော်ဒယ်စာရင်းကို ရယူရန် စာရင်းကို ပြန်လည်သစ်တမ်းဆွဲပါ
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// ပြန်လည်သစ်တမ်းဆွဲပြီးနောက် အားလုံးရနိုင်သော မော်ဒယ်များကို စာရင်းပြပါ
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### အတွေးလေ့ကျင့်ခန်း ၁၀: သဘောပေါက်မှုမော်ဒယ်များနှင့် အလုပ်လုပ်ခြင်း

**phi-4-mini-reasoning** မော်ဒယ်တွင် စဉ်းစားသော မြှောက်တင်ချက်များ ပါဝင်သည်။ ၎င်းသည် ၎င်း၏ ပို့စ်အဖြေ ပေးမီ `<think>...</think>` tag များကို အတွင်းခံ ရေးဆွဲထားသည်။ ဒုတိယအဆင့် မှန်ကန်မှုများ လိုအပ်သော လုပ်ငန်းများ (သင်္ချာ၊ အချက်အလက်အပေါ် စဉ်းစားမှု) အတွက် အသုံးဝင်သည်။

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning သည် ประมาณ ၄.၆ GB ဖြစ်သည်
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# မော်ဒယ်သည် ၎င်း၏စဉ်းစားမှုကို <think>...</think> တက်များဖြင့် ထုပ်ပိုးထားသည်
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

// စဉ်ဆက်မပြတ် စဉ်းစားမှုပုံစံကို ပေါင်းကွဲထားသည်
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **ဘယ်တော့သဘောပေါက်မှုမော်ဒယ်အသုံးပြုမလဲ:**
> - သင်္ချာနှင့် နှုတ်ချမြင်။
> - အဆင့်များစွာရှိသော စီစဉ်မှု လုပ်ငန်းများ။
> - ဖန်တီးမှု အဆင့်မြင့် code များ။
> - လုပ်ဆောင်မှု ပြသခြင်း ဖြင့် တိကျမှုတိုးမြှင့်မှု။
>
> **တရားမျှတမှုချက်** သဘောပေါက်မှုမော်ဒယ်များသည် token များ (ထည့်သွင်းသော `<think>` အပိုင်း) ပိုထုတ်ဖော်ပြီး နှေးကွေး သောကြောင့်၊ ရိုးရိုး Q&A အတွက် phi-3.5-mini က ပိုမြန်သည်။

---

### အတွေးလေ့ကျင့်ခန်း ၁၁: Alias နှင့် ဟာ့ဒ်ဝဲ ရွေးချယ်မှုကို နားလည်ခြင်း

သင်သည် **alias** (ဥပမာ `phi-3.5-mini`) ကို တိကျတဲ့ မော်ဒယ် ID မဟုတ်ပဲ ပေးပို့သည်ဆိုပါက SDK သည် သင်၏ ဟာ့ဒ်ဝဲအတွက် အကောင်းဆုံး မတူကွဲပြားမှုကို အလိုအလျောက် ရွေးချယ်ပေးသည်။

| Hardware | ရွေးချယ်ထားသော Execution Provider |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML ပလပ်အင်ဖြင့်) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| မည်သည့်ပစ္စည်း (fallback) | `CPUExecutionProvider` သို့မဟုတ် `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ဤအောက်ဖော်ပြပါ အဲလိပ်အမည်သည် သင့်ကွန်ပျူတာပစ္စည်းအတွက် အကောင်းဆုံး ဗားရှင်းကို ဖြေရှင်းပေးသည်။
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **အကြံပြုချက်:** အချိန်တိုင်း alias များကို သင်၏ အပလီကေးရှင်း code တွင် အသုံးပြုပါ။ အသုံးပြုသူ့စက်တွင် ထည့်သွင်းထားသောအခါ SDK သည် runtime တွင် အကောင်းဆုံး မတူကွဲပြားမှုကို ရွေးချယ်ပေးသည် - NVIDIA တွင် CUDA, Qualcomm တွင် QNN, အခြားတွင် CPU အသုံးပြုသည်။

---

### အတွေးလေ့ကျင့်ခန်း ၁၂: C# Configuration ရွေးချယ်စရာများ

C# SDK ၏ `Configuration` class သည် runtime ကို တိကျစွာ ထိန်းချုပ်နိုင်ရန် အတွက် ပါဝင်သည်။

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

| ပိုင်ဆိုင်မှု | ပုံမှန်တန်ဖိုး | ဖော်ပြချက် |
|----------|---------|-------------|
| `AppName` | (လိုအပ်သည်) | သင့်အပလီကေးရှင်း၏ နာမည် |
| `LogLevel` | `Information` | Logging အဆင့် |
| `Web.Urls` | (dynamic) | web server အတွက် သီးသန့် port သတ်မှတ်ခြင်း |
| `AppDataDir` | OS ပုံမှန် | app data အခြေခံ ဒိုင်ရေးတရိုရီ |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | မော်ဒယ်များ သိမ်းဆည်းရာနေရာ |
| `LogsDir` | `{AppDataDir}/logs` | logs ဖိုင်ထားရာနေရာ |

---

### အတွေးလေ့ကျင့်ခန်း ၁၃: Browser အသုံးပြုမှု (JavaScript အတွက်သာ)

JavaScript SDK တွင် browser အထောက်အကူပြု ဗားရှင်း ပါဝင်သည်။ browser တွင် သင်သည် CLI ဖြင့် ဝန်ဆောင်မှုစတင်ပြီး host URL ကို သတ်မှတ်ရမည်။

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// ပထမဦးစွာ ဝန်ဆောင်မှုကို လက်ဖြင့် စတင်ပါ:
//   foundry service start
// ထိုနောက် CLI output မှ URL ကို အသုံးပြုပါ
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// ကတ်တလော့ကို စူးစမ်းကြည့်ရှုပါ
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Browser ကန့်သတ်ချက်များ:** browser ဗားရှင်းတွင် `startWebService()` ကို မထောက်ပံ့ပါ။ SDK ကို browser တွင် အသုံးပြုမည့်အခါ Foundry Local service ကို ပြီးသားတင်ထားထားရန် လိုအပ်သည်။

---

## API ပြည့်စုံ ရည်ညွှန်းချက်

### Python

| အမျိုးအစား | နည်းလမ်း | ဖော်ပြချက် |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | မန်နေဂျာ ဖန်တီးရန်; အလိုအလျောက် မော်ဒယ်နှင့် Bootstrap လုပ်စေသည် |
| **Service** | `is_service_running()` | ဝန်ဆောင်မှု ပြေးနေခြင်းစစ်ဆေးရန် |
| **Service** | `start_service()` | ဝန်ဆောင်မှု စတင်ရန် |
| **Service** | `endpoint` | API endpoint URL |
| **Service** | `api_key` | API key |
| **Catalog** | `list_catalog_models()` | ရရှိနိုင်သော မော်ဒယ်များ စာရင်းထုတ်ရန် |
| **Catalog** | `refresh_catalog()` | ကတာလော့အသစ် ပြုလုပ်ရန် |
| **Catalog** | `get_model_info(alias_or_model_id)` | မော်ဒယ် metadata ရယူရန် |
| **Cache** | `get_cache_location()` | cache directory လမ်းကြောင်း |
| **Cache** | `list_cached_models()` | ဒေါင်းလုတ် ထားသော မော်ဒယ်များ စာရင်းထုတ်ရန် |
| **Model** | `download_model(alias_or_model_id)` | မော်ဒယ် ဒေါင်းလုတ်လုပ်ရန် |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | မော်ဒယ် တင်ရန် |
| **Model** | `unload_model(alias_or_model_id)` | မော်ဒယ် ခွဲထုတ်ရန် |
| **Model** | `list_loaded_models()` | တင်ထားသော မော်ဒယ်များ စာရင်း |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | မော်ဒယ် အဆင့်မြှင့်ရနိုင်မှု စစ်ဆေးရန် |
| **Model** | `upgrade_model(alias_or_model_id)` | မော်ဒယ် အဆင့်မြှင့်ရန် |
| **Service** | `httpx_client` | တိုက်ရိုက် API ခေါ်သုံးရန် ဆက်တင် HTTPX client |

### JavaScript

| အမျိုးအစား | နည်းလမ်း | ဖော်ပြချက် |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | SDK singleton စတင်ဖန်တီးရန် |
| **Init** | `FoundryLocalManager.instance` | singleton မန်နေဂျာ ရယူရန် |
| **Service** | `manager.startWebService()` | web service စတင်ရန် |
| **Service** | `manager.urls` | ဝန်ဆောင်မှု အခြေခံ URL များစာရင်း |
| **Catalog** | `manager.catalog` | မော်ဒယ် ကတာလော့ |
| **Catalog** | `catalog.getModel(alias)` | alias ဖြင့် မော်ဒယ် object ရယူရန် (Promise return) |
| **Model** | `model.isCached` | မော်ဒယ် ဒေါင်းလုတ်ပြီးဖြစ်ခြင်း |
| **Model** | `model.download()` | မော်ဒယ် ဒေါင်းလုတ်ရန် |
| **Model** | `model.load()` | မော်ဒယ် တင်ရန် |
| **Model** | `model.unload()` | မော်ဒယ် ခွဲထုတ်ရန် |
| **Model** | `model.id` | မော်ဒယ် တိကျသည့် ID |
| **Model** | `model.alias` | မော်ဒယ် alias |
| **Model** | `model.createChatClient()` | native chat client ရယူရန် (OpenAI SDK မလိုအပ်) |
| **Model** | `model.createAudioClient()` | အသံမှတ်တမ်း client ရယူရန် |
| **Model** | `model.removeFromCache()` | မော်ဒယ်ကို ဒေသတွင်း cache မှ ဖယ်ရှားရန် |
| **Model** | `model.selectVariant(variant)` | ဟာ့ဒ်ဝဲ ဗားရှင်းရွေးချယ်ရန် |
| **Model** | `model.variants` | မော်ဒယ်ရှိ မတူကွဲပြားမှုများ စာရင်း |
| **Model** | `model.isLoaded()` | မော်ဒယ် မိတ်ဆက်ပြီး ဖွင့်ထားခြင်း ရှိ/မရှိ စစ်ဆေးရန် |
| **Catalog** | `catalog.getModels()` | ရရှိနိုင်သော မော်ဒယ်များ စာရင်းထုတ်ရန် |
| **Catalog** | `catalog.getCachedModels()` | ဒေါင်းလုတ်ထားသော မော်ဒယ်များ စာရင်း |
| **Catalog** | `catalog.getLoadedModels()` | တင်ထားသော မော်ဒယ်များ စာရင်း |
| **Catalog** | `catalog.updateModels()` | ဝန်ဆောင်မှုမှ ကတာလော့ အပ်ဒိတ်လုပ်ရန် |
| **Service** | `manager.stopWebService()` | Foundry Local web service ရပ်တန့်ရန် |

### C# (v0.8.0+)

| အမျိုးအစား | နည်းလမ်း | ဖော်ပြချက် |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | မန်နေဂျာ စတင်ဖန်တီးရန် |
| **Init** | `FoundryLocalManager.Instance` | singleton ရယူရန် |
| **Catalog** | `manager.GetCatalogAsync()` | ကတာလော့ ရယူရန် |
| **Catalog** | `catalog.ListModelsAsync()` | မော်ဒယ်များ စာရင်းထုတ်ရန် |
| **Catalog** | `catalog.GetModelAsync(alias)` | သီးသန့် မော်ဒယ် ရယူရန် |
| **Catalog** | `catalog.GetCachedModelsAsync()` | ဒေါင်းလုတ်ပြီး မော်ဒယ်များ စာရင်း |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | တင်ထားသော မော်ဒယ်များ စာရင်း |
| **Model** | `model.DownloadAsync(progress?)` | မော်ဒယ် ဒေါင်းလုတ်ရန် |
| **Model** | `model.LoadAsync()` | မော်ဒယ် တင်ရန် |
| **Model** | `model.UnloadAsync()` | မော်ဒယ် ခွဲထုတ်ရန် |
| **Model** | `model.SelectVariant(variant)` | ဟာ့ဒ်ဝဲ ဗားရှင်း ရွေးချယ်ရန် |
| **Model** | `model.GetChatClientAsync()` | native chat client ရယူရန် |
| **Model** | `model.GetAudioClientAsync()` | အသံမှတ်တမ်း client ရယူရန် |
| **Model** | `model.GetPathAsync()` | ဒေသထွက် ဖိုင်လမ်းကြောင်း ရယူရန် |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | ဟာ့ဒ်ဝဲ မတူကွဲပြားမှု တိကျ ရယူရန် |
| **Catalog** | `catalog.UpdateModelsAsync()` | ကတာလော့ အသစ်ပြုလုပ်ရန် |
| **Server** | `manager.StartWebServerAsync()` | REST web server စတင်ရန် |
| **Server** | `manager.StopWebServerAsync()` | REST web server ရပ်တန့်ရန် |
| **Config** | `config.ModelCacheDir` | cache directory |

---

## အဓိက သင်ယူချက်များ

| အယူအဆ | သင်ယူထားသော အကြောင်းအရာ |
|---------|-----------------|
| **SDK နှင့် CLI** | SDK သည် အပလီကေးရှင်းများအတွက် အလိုအလျောက် ထိန်းချုပ်မှု ပေးသည် |
| **ထိန်းချုပ်မှု လမ်းကြောင်း** | SDK သည် ဝန်ဆောင်မှုများ၊ မော်ဒယ်များ၊ cache များကို စီမံသည် |
| **Dynamic ports** | အမြဲ `manager.endpoint` (Python) ဒါမှမဟုတ် `manager.urls[0]` (JS/C#) ကို အသုံးပြုပါ — port ကို စာကြည့်တိုက်ထည့်သွင်းခြင်း မလုပ်ပါနှင့် |
| **Aliases** | alias များကို အသုံးပြုသောအခါ ဟာ့ဒ်ဝဲအလိုက် အကောင်းဆုံး မော်ဒယ် ကို အလိုအလျောက် ရွေးချယ်ပေးသည် |
| **အမြန်စတင်ခြင်း** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# ပြန်လည်ဒီဇိုင်းဆွဲခြင်း** | v0.8.0+ သည် ကိုယ်ပိုင်ဖြစ်ပြီး အပြီးသုံးစက်များတွင် CLI မလိုအပ်ပါ |
| **မော်ဒယ်ရုပ်သဏ္ဍန်သက်တမ်း** | Catalog → ဒေါင်းလုဒ် → သွင်းခြင်း → အသုံးပြုခြင်း → ဖြုတ်ခြင်း |
| **FoundryModelInfo** | စိုက်ထုတ်သည့်အချက်အလက်များစွာပါဝင်သည် - အလုပ်, စက်ပစ္စည်း, အရွယ်, လိုင်စင်, ကိရိယာခေါ်ဆိုမှုအထောက်အပံ့ |
| **ChatClient** | OpenAI အခမဲ့သုံးနိုင်ရန် `createChatClient()` (JS) / `GetChatClientAsync()` (C#) |
| **ဗားရှင်းများ** | မော်ဒယ်များတွင် hardware-specific ဗားရှင်းများ ရှိသည် (CPU, GPU, NPU); အလိုအလျောက်ရွေးချယ်သည် |
| **တိုးတက်မှုများ** | Python: မော်ဒယ်များကို မကြာခဏထားရန် `is_model_upgradeable()` + `upgrade_model()` |
| **Catalog ပြန်လည်လှည့်မည်** | `refresh_catalog()` (Python) / `updateModels()` (JS) သစ်မော်ဒယ်များ ရှာဖွေခြင်း |

---

## အရင်းအမြစ်များ

| အရင်းအမြစ် | လင့်ခ် |
|-------------|---------|
| SDK ရည်ညွှန်းစာတမ်း (ဘာသာစကားအားလုံး) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| inference SDK များနှင့် ပေါင်းစည်းခြင်း | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API ရည်ညွှန်းစာတမ်း | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK နမူနာများ | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local ဝက်ဘ်ဆိုက် | [foundrylocal.ai](https://foundrylocal.ai) |

---

## နောက်ထပ်အဆင့်များ

SDK ကို OpenAI အတွက် client library နှင့် ချိတ်ဆက်၍ သင့်ရဲ့ ပထမဆုံး chat completion application တည်ဆောက်ရန် [အပိုင်း ၃: SDK နှင့် APIs အသုံးပြုခြင်း](part3-sdk-and-apis.md) သို့ ဆက်လက်လေ့လာပါ။

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ပယ်ချမှတ်ချက်**:  
ဤစာတမ်းကို AI ဘာသာပြန်ဝန်ဆောင်မှုဖြစ်သော [Co-op Translator](https://github.com/Azure/co-op-translator) ဖြင့် ဘာသာပြန်ထားပါသည်။ ကျွန်ုပ်တို့သည် တိကျမှန်ကန်မှုအတွက် ကြိုးစားပြီးဖြစ်သော်လည်း အလိုအလျောက် ဘာသာပြန်ချက်များတွင် အမှားများ သို့မဟုတ် မှားယွင်းချက်များ ပါဝင်နိုင်ကြောင်း သိရှိပေးပါရန် မေတ္တာရပ်ခံအပ်ပါသည်။ မူရင်းစာတမ်းကို ၎င်း၏ ကိုယ်ပိုင်ဘာသာစကားဖြင့် တရားဝင် အောက်ခံဌာနအဖြစ် မှတ်ယူရပါမည်။ အရေးကြီးသော သတင်းအချက်အလက်များအတွက် လက်ရှိကျွမ်းကျင်သော လူ့ဘာသာပြန်ကိုအသုံးပြုရန် သတိပေးအပ်ပါသည်။ ဤဘာသာပြန်ခြင်းကို အသုံးပြုမှုကြောင့် ဖြစ်ပေါ်လာနိုင်သည့် အလွဲအတားများ သို့မဟုတ် အဓိပ္ပာယ်မမှန်မှားယူမှုများအတွက် ကျွန်ုပ်တို့သည် တာဝန်မခံပါ။
<!-- CO-OP TRANSLATOR DISCLAIMER END -->