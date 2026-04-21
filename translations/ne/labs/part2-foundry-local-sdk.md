![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग २: Foundry Local SDK गहिरो अध्ययन

> **लक्ष्य:** Foundry Local SDK मा निपुण हुनुहोस् ताकि मोडेलहरू, सेवाहरू, र क्यासिङ प्रोग्रामिङ्ग रूपले व्यवस्थापन गर्न सकियोस् - र बुझ्नुहोस् किन SDK ले CLI भन्दा एप्लिकेशन निर्माणका लागि सिफारिस गरिएको दृष्टिकोण हो।

## अवलोकन

भाग १ मा तपाईँले **Foundry Local CLI** प्रयोग गरेर मोडेलहरू डाउनलोड र अन्तरक्रियात्मक रूपमा चलाउनुभयो। CLI अन्वेषणको लागि राम्रो छ, तर जब तपाईँले वास्तविक एपहरू निर्माण गर्नुहुन्छ तपाईँलाई **प्रोग्रामिङ्ग नियन्त्रण** आवश्यक छ। Foundry Local SDK ले त्यो दिन्छ - यसले **नियन्त्रण विमान** (सेवा सुरु गर्नु, मोडेल पत्ता लगाउनु, डाउनलोड गर्नु, लोड गर्नु) व्यवस्थापन गर्छ ताकि तपाईँको एप्लिकेशन कोडले **डेटा विमान** (प्रॉम्प्ट पठाउने, पूर्णताहरू प्राप्त गर्ने) मा ध्यान केन्द्रित गर्न सकोस्।

यस अभ्यासले तपाईँलाई Python, JavaScript, र C# मा सम्पूर्ण SDK API सतह सिकाउँछ। अन्त्यसम्म तपाईँले प्रत्येक उपलब्ध विधि बुझ्न र कहिले प्रयोग गर्ने थाहा पाउनु हुनेछ।

## सिकाइ लक्ष्यहरू

यस अभ्यासको अन्त्यमा तपाईँ गर्न सक्नुहुनेछ:

- किन SDK लाई CLI भन्दा एप विकासमा प्राथमिकता दिइन्छ व्याख्या गर्न
- Python, JavaScript, वा C# का लागि Foundry Local SDK स्थापना गर्न
- `FoundryLocalManager` प्रयोग गरेर सेवा सुरु गर्नु, मोडेलहरू व्यवस्थापन गर्नु, र क्याटलग सोध्नु
- प्रोग्रामिङ्गमै मोडेलहरू सूचीबद्ध, डाउनलोड, लोड, र अनलोड गर्नु
- `FoundryModelInfo` प्रयोग गरेर मोडेल मेटाडाटा निरीक्षण गर्नु
- क्याटलग, क्यास, र लोड गरिएको मोडेलहरूको भिन्नता बुझ्नु
- कन्स्ट्रक्टर बुटस्ट्र्याप (Python) र `create()` + क्याटलग ढाँचा (JavaScript) प्रयोग गर्नु
- C# SDK पुनःडिजाइन र यसको वस्तु-उन्मुख API बुझ्नु

---

## आवश्यकताहरू

| आवश्यकता | विवरण |
|-------------|---------|
| **Foundry Local CLI** | स्थापना गरिएको र तपाईँको `PATH` मा रहेको ([भाग १](part1-getting-started.md)) |
| **प्रोग्रामिङ्ग भाषा रनटाइम** | **Python 3.9+** र/वा **Node.js 18+** र/वा **.NET 9.0+** |

---

## अवधारणा: SDK बनाम CLI - किन SDK प्रयोग गर्ने?

| पक्ष | CLI (`foundry` कमाण्ड) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **प्रयोगको मामला** | अन्वेषण, म्यानुअल परीक्षण | एप्लिकेशन एकीकरण |
| **सेवा व्यवस्थापन** | म्यानुअल: `foundry service start` | स्वचालित: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **पोर्ट पत्ता लगाउने** | CLI आउटपुटबाट पढ्ने | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **मोडेल डाउनलोड** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **त्रुटि व्यवस्थापन** | निकास कोड, stderr | अपवादहरू, टाइप गरिएका त्रुटिहरू |
| **स्वचालन** | Shell स्क्रिप्टहरू | स्वदेशी भाषा एकीकरण |
| **डिप्लॉयमेन्ट** | अन्त-प्रयोगकर्ता मेसिनमा CLI आवश्यक | C# SDK स्व-समावेशी हुनसक्छ (CLI आवश्यक छैन) |

> **मुख्य तथ्य:** SDK सम्पूर्ण जीवनचक्र व्यवस्थापन गर्छ: सेवा सुरु गर्ने, क्यास जाँच्ने, हराएका मोडेलहरू डाउनलोड गर्ने, तिनीहरू लोड गर्ने, र अन्तबिन्दु पत्ता लगाउने, केही पंक्तिमा। तपाईँको एपलाई CLI आउटपुट पार्स गर्न वा उपप्रक्रियाहरू व्यवस्थापन गर्न आवश्यक छैन।

---

## अभ्यासहरू

### अभ्यास १: SDK स्थापना गर्नुहोस्

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

स्थापना जाँच गर्नुहोस्:

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

स्थापना जाँच गर्नुहोस्:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

दुई NuGet प्याकेजहरू छन्:

| प्याकेज | प्लेटफार्म | विवरण |
|---------|------------|--------|
| `Microsoft.AI.Foundry.Local` | क्रस-प्लेटफार्म | Windows, Linux, macOS मा काम गर्छ |
| `Microsoft.AI.Foundry.Local.WinML` | Windows मात्र | WinML हार्डवेयर एक्सेलेरेशन थप्छ; प्लगइन कार्यान्वयन प्रदायकहरू (जस्तै Qualcomm NPU को लागि QNN) डाउनलोड र स्थापना गर्छ |

**Windows सेटअप:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` फाइल सम्पादन गर्नुहोस्:

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

> **टिप्पणी:** Windows मा WinML प्याकेज आधार SDK सहित QNN execution provider समेत हुन्छ। Linux/macOS मा आधार SDK प्रयोग गरिन्छ। सशर्त TFM र प्याकेज सन्दर्भहरूले परियोजनालाई पूर्ण क्रस-प्लेटफार्म बनाइराख्छ।

परियोजना मूलमा `nuget.config` सिर्जना गर्नुहोस्:

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

प्याकेजहरू पुन:स्थापना गर्नुहोस्:

```bash
dotnet restore
```

</details>

---

### अभ्यास २: सेवा सुरु गर्नुहोस् र क्याटलग सूचीबद्ध गर्नुहोस्

कुनै पनि एपले सबैभन्दा पहिला Foundry Local सेवा सुरु गर्छ र उपलब्ध मोडेलहरू पत्ता लगाउँछ।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# म्यानेजर बनाउनुहोस् र सेवा सुरु गर्नुहोस्
manager = FoundryLocalManager()
manager.start_service()

# सूचीकृत सबै मोडेलहरू देखाउनुहोस्
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - सेवा व्यवस्थापन विधिहरू

| विधि | हस्ताक्षर | विवरण |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | सेवा चलिरहेको छ कि छैन जाँच गर्नुहोस् |
| `start_service()` | `() -> None` | Foundry Local सेवा सुरु गर्नुहोस् |
| `service_uri` | `@property -> str` | आधार सेवा URI |
| `endpoint` | `@property -> str` | API अन्तबिन्दु (सेवा URI + `/v1`) |
| `api_key` | `@property -> str` | API कुञ्जी (env बाट वा पूर्वनिर्धारित प्लेसहोल्डर) |

#### Python SDK - क्याटलग व्यवस्थापन विधिहरू

| विधि | हस्ताक्षर | विवरण |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | क्याटलगमा सबै मोडेलहरूको सूची दिनुहोस् |
| `refresh_catalog()` | `() -> None` | सेवाबाट क्याटलग ताजा गर्नुहोस् |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | विशेष मोडेलको जानकारी लिनुहोस् |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// म्यानेजर सिर्जना गर्नुहोस् र सेवा सुरु गर्नुहोस्
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// क्याटलग ब्राउज गर्नुहोस्
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - व्यवस्थापक विधिहरू

| विधि | हस्ताक्षर | विवरण |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK एकल उदाहरण आरम्भ गर्नुहोस् |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | एकल व्यवस्थापक पहुँचाउनुहोस् |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local वेब सेवा सुरु गर्नुहोस् |
| `manager.urls` | `string[]` | सेवाका आधार URL हरूको एरे |

#### JavaScript SDK - क्याटलग र मोडेल विधिहरू

| विधि | हस्ताक्षर | विवरण |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | मोडेल क्याटलग पहुँच |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | उपनामबाट मोडेल वस्तु प्राप्त गर्नुहोस् |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ ले वस्तु-उन्मुख वास्तुकलाको प्रयोग गर्छ `Configuration`, `Catalog`, र `Model` वस्तुहरूसँग:

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

#### C# SDK - प्रमुख कक्षाहरू

| कक्षा | उद्देश्य |
|-------|----------|
| `Configuration` | एप नाम, लग स्तर, क्यास डाइरेक्टरी, वेब सर्भर URL सेट गर्नुहोस् |
| `FoundryLocalManager` | मुख्य प्रवेश बिन्दु - `CreateAsync()` बाट सिर्जना गरिएको, `.Instance` मार्फत पहुँच |
| `Catalog` | क्याटलगबाट ब्राउज, खोज, र मोडेल प्राप्त गर्नुहोस् |
| `Model` | विशिष्ट मोडेल प्रतिनिधित्व - डाउनलोड, लोड, ग्राहकहरू प्राप्त गर्नुहोस् |

#### C# SDK - व्यवस्थापक र क्याटलग विधिहरू

| विधि | विवरण |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | व्यवस्थापक आरम्भ गर्नुहोस् |
| `FoundryLocalManager.Instance` | एकल व्यवस्थापक पहुँचाउनुहोस् |
| `manager.GetCatalogAsync()` | मोडेल क्याटलग प्राप्त गर्नुहोस् |
| `catalog.ListModelsAsync()` | सबै उपलब्ध मोडेल सूची गर्नुहोस् |
| `catalog.GetModelAsync(alias: "alias")` | उपनामबाट विशेष मोडेल प्राप्त गर्नुहोस् |
| `catalog.GetCachedModelsAsync()` | डाउनलोड गरिएका मोडेलहरूको सूची |
| `catalog.GetLoadedModelsAsync()` | हाल लोड गरिएको मोडेलहरूको सूची |

> **C# वास्तुकला नोट:** C# SDK v0.8.0+ पुनःडिजाइनले एप्लिकेशनलाई **स्व-समावेशी** बनाउँछ; यसलाई अन्त प्रयोगकर्ताको मेसिनमा Foundry Local CLI आवश्यक पर्दैन। SDK स्वदेशी रूपमा मोडेल व्यवस्थापन र अनुमान गर्ने कार्य गर्छ।

</details>

---

### अभ्यास ३: मोडेल डाउनलोड र लोड गर्नुहोस्

SDK ले डाउनलोड (डिस्कमा) र लोड (मेमोरीमा) छुट्याउँछ। यसले तपाईँलाई सेटअप समयमा पूर्व-डाउनलोड गर्न र माग ब्यापारमा लोड गर्न अनुमति दिन्छ।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# विकल्प A: म्यानुअल चरण-चरण
manager = FoundryLocalManager()
manager.start_service()

# पहिले क्यास जाँच गर्नुहोस्
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

# विकल्प B: एक लाइनर बुटस्ट्र्याप (सिफारिस गरिन्छ)
# कन्स्ट्रक्टरलाई उपनाम पास गर्नुहोस् - यसले सेवा सुरु गर्छ, डाउनलोड गर्छ, र स्वचालित रूपमा लोड गर्छ
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - मोडेल व्यवस्थापन विधिहरू

| विधि | हस्ताक्षर | विवरण |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | स्थानीय क्यासमा मोडेल डाउनलोड गर्नुहोस् |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | मोडेललाई अनुमान सर्भरमा लोड गर्नुहोस् |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | सर्भरबाट मोडेल अनलोड गर्नुहोस् |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | हाल लोड गरिएको सबै मोडेलहरूको सूची दिनुहोस् |

#### Python - क्यास व्यवस्थापन विधिहरू

| विधि | हस्ताक्षर | विवरण |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | क्यास डाइरेक्टरीको पथ प्राप्त गर्नुहोस् |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | सबै डाउनलोड गरिएका मोडेलहरूको सूची दिनुहोस् |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// चरण-दर-चरण दृष्टिकोण
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

#### JavaScript - मोडेल विधिहरू

| विधि | हस्ताक्षर | विवरण |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | मोडेल पहिल्यै डाउनलोड गरिएको छ कि छैन |
| `model.download()` | `() => Promise<void>` | मोडेल स्थानीय क्यासमा डाउनलोड गर्नुहोस् |
| `model.load()` | `() => Promise<void>` | अनुमान सर्भरमा लोड गर्नुहोस् |
| `model.unload()` | `() => Promise<void>` | अनुमान सर्भरबाट अनलोड गर्नुहोस् |
| `model.id` | `string` | मोडेलको अनन्य परिचयपत्र |

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

#### C# - मोडेल विधिहरू

| विधि | विवरण |
|--------|-------------|
| `model.DownloadAsync(progress?)` | चयन गरिएको भेरियन्ट डाउनलोड गर्नुहोस् |
| `model.LoadAsync()` | मोडेललाई मेमोरीमा लोड गर्नुहोस् |
| `model.UnloadAsync()` | मोडेल अनलोड गर्नुहोस् |
| `model.SelectVariant(variant)` | विशेष भेरियन्ट चयन गर्नुहोस् (CPU/GPU/NPU) |
| `model.SelectedVariant` | हाल चयन गरिएको भेरियन्ट |
| `model.Variants` | यस मोडेलका सबै उपलब्ध भेरियन्टहरू |
| `model.GetPathAsync()` | स्थानीय फाइल पथ प्राप्त गर्नुहोस् |
| `model.GetChatClientAsync()` | स्वदेशी च्याट क्लाइन्ट प्राप्त गर्नुहोस् (OpenAI SDK आवश्यक छैन) |
| `model.GetAudioClientAsync()` | ट्रान्सक्रिप्सनका लागि अडियो क्लाइन्ट प्राप्त गर्नुहोस् |

</details>

---

### अभ्यास ४: मोडेल मेटाडाटा निरीक्षण गर्नुहोस्

`FoundryModelInfo` वस्तुमा प्रत्येक मोडेलको समृद्ध मेटाडाटा हुन्छ। यी क्षेत्रहरू बुझ्नाले तपाईँलाई तपाईँको एपका लागि सही मोडेल छनोट गर्न मद्दत गर्छ।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# एक विशिष्ट मोडेलको विस्तृत जानकारी प्राप्त गर्नुहोस्
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

#### FoundryModelInfo क्षेत्रहरू

| क्षेत्र | प्रकार | विवरण |
|-------|--------|-------------|
| `alias` | string | छोटो नाम (जस्तै `phi-3.5-mini`) |
| `id` | string | अनन्य मोडेल परिचयपत्र |
| `version` | string | मोडेल संस्करण |
| `task` | string | `chat-completions` वा `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, वा NPU |
| `execution_provider` | string | रनटाइम ब्याकएण्ड (CUDA, CPU, QNN, WebGPU, आदि) |
| `file_size_mb` | int | डिस्कमा आकार एमबीमा |
| `supports_tool_calling` | bool | मोडेलले फङ्सन/टुल कलिंग समर्थन गर्छ कि गर्दैन |
| `publisher` | string | मोडेल प्रकाशक |
| `license` | string | लाइसेन्स नाम |
| `uri` | string | मोडेल URI |
| `prompt_template` | dict/null | प्रॉम्प्ट टेम्प्लेट, यदि कुनै छ भने |

---

### अभ्यास ५: मोडेल जीवनचक्र व्यवस्थापन गर्नुहोस्

पूरा जीवनचक्र अभ्यास गर्नुहोस्: सूची → डाउनलोड → लोड → प्रयोग → अनलोड।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # छिटो परीक्षणको लागि सानो मोडेल

manager = FoundryLocalManager()
manager.start_service()

# 1. क्याटलगमा के छ जाँच गर्नुहोस्
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. के पहिले नै डाउनलोड गरिएको छ जाँच्नुहोस्
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. एउटा मोडेल डाउनलोड गर्नुहोस्
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. अब यो क्यासमा छ कि छैन जाँच्नुहोस्
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. यसलाई लोड गर्नुहोस्
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. के लोड गरिएको छ जाँच्नुहोस्
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. यसलाई अनलोड गर्नुहोस्
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

const alias = "qwen2.5-0.5b"; // छिटो परीक्षणको लागि सानो मोडेल

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// १. क्याटलगबाट मोडेल प्राप्त गर्नुहोस्
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// २. आवश्यक परेमा डाउनलोड गर्नुहोस्
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// ३. यसलाई लोड गर्नुहोस्
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// ४. यसलाई अनलोड गर्नुहोस्
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### अभ्यास ६: क्विक-स्टार्ट प्याटनहरू

हरेक भाषा एक कलमा सेवा सुरु गर्ने र मोडेल लोड गर्ने छोटो बाटो प्रदान गर्छ। यी धेरै एप्लिकेसनहरूको लागि **सिफारिस गरिएका प्याटनहरू** हुन्।

<details>
<summary><h3>🐍 Python - कन्स्ट्रक्टर बुटस्ट्र्याप</h3></summary>

```python
from foundry_local import FoundryLocalManager

# कन्स्ट्रक्टरमा एउटा उपनाम पास गर्नुहोस् - यसले सबै केही ह्यान्डल गर्छ:
# 1. सेवा चलिरहेको छैन भने सुरु गर्छ
# 2. मोडेल क्यासमा छैन भने डाउनलोड गर्छ
# 3. मोडेललाई इन्फेरेन्स सर्भरमा लोड गर्छ
manager = FoundryLocalManager("phi-3.5-mini")

# तुरुन्तै प्रयोग गर्न तयार
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` प्यारामिटर (पूर्वनिर्धारित `True`) यस व्यवहारलाई नियन्त्रण गर्छ। यदि तपाईं म्यानुअल नियन्त्रण चाहनु हुन्छ भने `bootstrap=False` सेट गर्नुहोस्:

```python
# म्यानुअल मोड - केही पनि स्वचालित रूपमा हुँदैन
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + क्याटलग</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() ले सबै कुरा ह्यान्डल गर्दछ:
// 1. सेवा सुरु गर्दछ
// 2. catalog बाट मोडेल प्राप्त गर्दछ
// 3. आवश्यकता अनुसार डाउनलोड गर्दछ र मोडेल लोड गर्दछ
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// तुरुन्तै प्रयोगको लागि तयार
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + क्याटलग</h3></summary>

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

> **C# नोट:** C# SDK ले एप नाम, लगिङ, क्याच डाइरेक्टोरीहरू, र निश्चित वेब सर्भर पोर्ट पिन गर्न `Configuration` प्रयोग गर्छ। यसले यसलाई तीनवटा SDK मध्ये सबैभन्दा धेरै कन्फिगरेबल बनाउँछ।

</details>

---

### अभ्यास ७: नेटिभ ChatClient (OpenAI SDK आवश्यक छैन)

JavaScript र C# SDK हरूले `createChatClient()` सुविधाजनक मेथड दिन्छन् जुन नेटिभ च्याट क्लाइन्ट फर्काउँछ—OpenAI SDK अलगै इन्स्टल वा कन्फिगर गर्न आवश्यक छैन।

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

// मोडेलबाट सिधै ChatClient सिर्जना गर्नुहोस् — OpenAI आयात आवश्यक छैन
const chatClient = model.createChatClient();

// completeChat ले OpenAI-अनुकूल प्रतिक्रिया वस्तु फिर्ता गर्दछ
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// स्ट्रिमिङले कलब्याक ढाँचा प्रयोग गर्दछ
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` ले टुल कलिङ पनि समर्थन गर्छ—दोस्रो आर्गुमेन्टको रूपमा टुलहरू पास गर्नुहोस्:

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

> **कुन प्याटन कहिले प्रयोग गर्ने:**
> - **`createChatClient()`** — छिटो प्रोटोटाइपिङ, कम निर्भरता, सरल कोड
> - **OpenAI SDK** — प्यारामिटरहरूमा पूर्ण नियन्त्रण (तापक्रम, top_p, स्टप टोकनहरू आदि), उत्पादन एप्लिकेसनका लागि उपयुक्त

---

### अभ्यास ८: मोडेल भेरियन्टहरू र हार्डवेयर छनोट

मोडेलहरूले विभिन्न हार्डवेयरका लागि अप्टिमाइज गरिएका धेरै **भेरियन्टहरू** हुन सक्छन्। SDK ले सर्वश्रेष्ठ भेरियन्ट स्वतः चयन गर्छ, तर तपाईंले पनि मनपरि हेर्न र छनोट गर्न सक्नुहुन्छ।

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// सबै उपलब्ध भेरियन्टहरू सूचीबद्ध गर्नुहोस्
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK ले स्वचालित रूपमा तपाईंको हार्डवेयरका लागि उत्तम भेरियन्ट चयन गर्छ
// ओभरराइड गर्न, selectVariant() प्रयोग गर्नुहोस्:
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

Python मा SDK ले हार्डवेयरको आधारमा सर्वश्रेष्ठ भेरियन्ट स्वतः चयन गर्छ। `get_model_info()` प्रयोग गरेर चयन के भयो हेर्नुहोस्:

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

#### NPU भेरियन्टहरू भएका मोडेलहरू

केही मोडेलहरू Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra) संग उपकरणहरूको लागि NPU-अप्टिमाइज्ड भेरियन्टहरू राख्छन्:

| मोडेल | NPU भेरियन्ट उपलब्ध |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **टिप:** NPU-सक्षम हार्डवेयरमा, SDK ले उपलब्ध हुँदा NPU भेरियन्ट स्वचालित रूपमा चयन गर्छ। तपाईंले तपाईंको कोड परिवर्तन गर्नु पर्दैन। Windows मा C# प्रोजेक्टहरूको लागि, QNN इन्जेक्सन प्रोभाइडर सक्षम गर्न `Microsoft.AI.Foundry.Local.WinML` NuGet प्याकेज थप्नुहोस्—QNN WinML मार्फत प्लगइन EP को रूपमा वितरण हुन्छ।

---

### अभ्यास ९: मोडेल अपग्रेडहरू र क्याटलग रिफ्रेश

मोडेल क्याटलग समय समयमा अपडेट हुन्छ। अपडेटहरू जाँच्न र लागू गर्न यी मेथडहरू प्रयोग गर्नुहोस्।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# नवीनतम मोडेल सूची प्राप्त गर्न क्याटलग पुनः ताजा पार्नुहोस्
manager.refresh_catalog()

# जाँच गर्नुहोस् कि के cached मोडेलको नयाँ संस्करण उपलब्ध छ कि छैन
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

// नयाँ मोडेल सूची प्राप्त गर्न क्याटालग रीफ्रेस गर्नुहोस्
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// रीफ्रेस पछि सबै उपलब्ध मोडेलहरू सूचीबद्ध गर्नुहोस्
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### अभ्यास १०: रिजनिङ मोडेलहरूसँग काम

**phi-4-mini-reasoning** मोडेलले chain-of-thought reasoning समावेश गर्छ। यसको अन्तरिक सोच `<think>...</think>` ट्यागहरू भित्र राख्छ अन्तिम उत्तर दिन अघि। यो बहु-चरण तर्क, गणित, वा समस्या समाधान गर्ने कामहरूका लागि उपयोगी छ।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-मिनी-कारण लगभग ४.६ जीबी हो
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# मोडेल यसको सोचलाई <think>...</think> ट्यागहरूमा लपेट्छ
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

// विचारश्रृंखला सोचलाई विश्लेषण गर्नुहोस्
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **कहिले रिजनिङ मोडेलहरू प्रयोग गर्ने:**
> - गणित र तर्कसम्बन्धी समस्याहरू
> - बहु-चरण योजना बनाउने कार्यहरू
> - जटिल कोड निर्माण
> - काम देखाउँदा सटीकता बढ्ने कार्यहरू
>
> **व्यापार:** रिजनिङ मोडेलहरूले बढी टोकन (उदाहरण `<think>` सेक्सन) उत्पादन गर्छन् र ढिलो हुन्छन्। साधारण प्रश्नोत्तरका लागि phi-3.5-mini जस्तो मानक मोडेल छिटो हुन्छ।

---

### अभ्यास ११: एलियसहरू र हार्डवेयर चयन बुझ्न

जब तपाईं पूर्ण मोडेल ID को सट्टा **एलियस** (जस्तै `phi-3.5-mini`) पास गर्नुहुन्छ, SDK ले तपाईंको हार्डवेयरका लागि सबैभन्दा उपयुक्त भेरियन्ट स्वचालित रूपमा चयन गर्छ:

| हार्डवेयर | चयन गरिएको इग्जिक्युसन प्रोभाइडर |
|----------|-------------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML प्लगइन मार्फत) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| कुनै पनि उपकरण (फल्याब्याक) | `CPUExecutionProvider` वा `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# उपनाम तपाईंको हार्डवेयरको लागि सबैभन्दा उत्तम भेरियन्टमा समाधान हुन्छ
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **टिप:** सधैं तपाईंको एप्लिकेसन कोडमा एलियसहरू प्रयोग गर्नुहोस्। जब तपाईं प्रयोगकर्ताको मेसिनमा तैनाथ गर्नुहुन्छ, SDK ले रनटाइममा उपयुक्त भेरियन्ट चयन गर्छ—CUDA NVIDIA मा, QNN Qualcomm मा, अरू ठाउँमा CPU।

---

### अभ्यास १२: C# कन्फिगरेसन विकल्पहरू

C# SDK को `Configuration` क्लासले रनटाइममा सूक्ष्म नियन्त्रण दिन्छ:

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

| सम्पत्ति | पूर्वनिर्धारित | विवरण |
|----------|--------------|----------|
| `AppName` | (आवश्यक) | तपाईंको एप्लिकेसनको नाम |
| `LogLevel` | `Information` | लगिङको स्तर |
| `Web.Urls` | (डायनामिक) | वेब सर्भरको लागि विशिष्ट पोर्ट पिन गर्नुहोस् |
| `AppDataDir` | OS डिफल्ट | एप डाटा लागि आधार डाइरेक्टोरी |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | मोडेलहरू कहाँ भण्डारण छन् |
| `LogsDir` | `{AppDataDir}/logs` | लगहरू कहाँ लेखिन्छन् |

---

### अभ्यास १३: ब्राउजर प्रयोग (केवल JavaScript)

JavaScript SDK मा ब्राउजर-संग अनुकूल संस्करण छ। ब्राउजरमा तपाईंले सेवा म्यानुअली CLI मार्फत सुरु गर्न र होस्ट URL निर्दिष्ट गर्नुपर्छ:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// सेवा म्यानुअली सुरु गर्नुहोस्:
//   foundry सेवा सुरु गर्नुहोस्
// त्यसपछि CLI आउटपुटबाट URL प्रयोग गर्नुहोस्
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// क्याटलग ब्राउज गर्नुहोस्
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **ब्राउजर सीमाहरू:** ब्राउजर संस्करणले `startWebService()` समर्थन गर्दैन। SDK ब्राउजरमा प्रयोग गर्नुअघि Foundry Local सेवा पहिले नै चलिरहेको हुनुपर्छ।

---

## पूर्ण API सन्दर्भ

### Python

| वर्गीकरण | मेथड | विवरण |
|----------|--------|-------------|
| **इनिट** | `FoundryLocalManager(alias?, bootstrap=True)` | म्यानेजर सिर्जना गर्नुहोस्; विकल्प रूपमा मोडेलसँग बुटस्ट्र्याप गर्नुहोस् |
| **सेवा** | `is_service_running()` | सेवा चलिरहेको छ कि छैन जाँच्नुहोस् |
| **सेवा** | `start_service()` | सेवा सुरु गर्नुहोस् |
| **सेवा** | `endpoint` | API एण्डपोइन्ट URL |
| **सेवा** | `api_key` | API कुञ्जी |
| **क्याटलग** | `list_catalog_models()` | सबै उपलब्ध मोडेलहरूको सूची |
| **क्याटलग** | `refresh_catalog()` | क्याटलग रिफ्रेश गर्नुहोस् |
| **क्याटलग** | `get_model_info(alias_or_model_id)` | मोडेल मेटाडाटा प्राप्त गर्नुहोस् |
| **क्याच** | `get_cache_location()` | क्याच डाइरेक्टोरी स्थान |
| **क्याच** | `list_cached_models()` | डाउनलोड भएका मोडेलहरूको सूची |
| **मोडेल** | `download_model(alias_or_model_id)` | मोडेल डाउनलोड गर्नुहोस् |
| **मोडेल** | `load_model(alias_or_model_id, ttl=600)` | मोडेल लोड गर्नुहोस् |
| **मोडेल** | `unload_model(alias_or_model_id)` | मोडेल अनलोड गर्नुहोस् |
| **मोडेल** | `list_loaded_models()` | लोड गरिएका मोडेलहरूको सूची |
| **मोडेल** | `is_model_upgradeable(alias_or_model_id)` | नयाँ संस्करण उपलब्ध छ कि छैन जाँच्नुहोस् |
| **मोडेल** | `upgrade_model(alias_or_model_id)` | मोडेललाई नयाँतम संस्करणमा अपग्रेड गर्नुहोस् |
| **सेवा** | `httpx_client` | प्रत्यक्ष API कलहरूको लागि प्रि-कन्फिगर्ड HTTPX क्लाइन्ट |

### JavaScript

| वर्गीकरण | मेथड | विवरण |
|----------|--------|-------------|
| **इनिट** | `FoundryLocalManager.create(options)` | SDK सिंगलटन सुरु गर्नुहोस् |
| **इनिट** | `FoundryLocalManager.instance` | सिंगलटन म्यानेजर पहुँच गर्नुहोस् |
| **सेवा** | `manager.startWebService()` | वेब सेवा सुरु गर्नुहोस् |
| **सेवा** | `manager.urls` | सेवाका लागि आधार URL हरूको एरे |
| **क्याटलग** | `manager.catalog` | मोडेल क्याटलग पहुँच गर्नुहोस् |
| **क्याटलग** | `catalog.getModel(alias)` | एलियस द्वारा मोडेल वस्तु प्राप्त गर्नुहोस् (प्रोमिस फर्काउँछ) |
| **मोडेल** | `model.isCached` | मोडेल डाउनलोड गरिएको हो कि होइन |
| **मोडेल** | `model.download()` | मोडेल डाउनलोड गर्नुहोस् |
| **मोडेल** | `model.load()` | मोडेल लोड गर्नुहोस् |
| **मोडेल** | `model.unload()` | मोडेल अनलोड गर्नुहोस् |
| **मोडेल** | `model.id` | मोडेलको अनन्य पहिचान |
| **मोडेल** | `model.alias` | मोडेलको एलियस |
| **मोडेल** | `model.createChatClient()` | नेटिभ च्याट क्लाइन्ट प्राप्त गर्नुहोस् (OpenAI SDK आवश्यक छैन) |
| **मोडेल** | `model.createAudioClient()` | ट्रान्सक्रिप्शनका लागि अडियो क्लाइन्ट प्राप्त गर्नुहोस् |
| **मोडेल** | `model.removeFromCache()` | मोडेललाई स्थानीय क्याचबाट हटाउनुहोस् |
| **मोडेल** | `model.selectVariant(variant)` | निश्चित हार्डवेयर भेरियन्ट चयन गर्नुहोस् |
| **मोडेल** | `model.variants` | यो मोडेलका उपलब्ध भेरियन्टहरूको एरे |
| **मोडेल** | `model.isLoaded()` | मोडेल हाल लोड गरिएको छ कि छैन जाँच्नुहोस् |
| **क्याटलग** | `catalog.getModels()` | सबै उपलब्ध मोडेलहरूको सूची |
| **क्याटलग** | `catalog.getCachedModels()` | डाउनलोड गरिएका मोडेलहरूको सूची |
| **क्याटलग** | `catalog.getLoadedModels()` | हाल लोड गरिएका मोडेलहरूको सूची |
| **क्याटलग** | `catalog.updateModels()` | सेवाबाट क्याटलग रिफ्रेश गर्नुहोस् |
| **सेवा** | `manager.stopWebService()` | Foundry Local वेब सेवा रोकिन्छ |

### C# (v0.8.0+)

| वर्गीकरण | मेथड | विवरण |
|----------|--------|-------------|
| **इनिट** | `FoundryLocalManager.CreateAsync(config, logger)` | म्यानेजर सुरु गर्नुहोस् |
| **इनिट** | `FoundryLocalManager.Instance` | सिंगलटन पहुँच गर्नुहोस् |
| **क्याटलग** | `manager.GetCatalogAsync()` | क्याटलग प्राप्त गर्नुहोस् |
| **क्याटलग** | `catalog.ListModelsAsync()` | सबै मोडेलहरूको सूची |
| **क्याटलग** | `catalog.GetModelAsync(alias)` | कुनै विशिष्ट मोडेल प्राप्त गर्नुहोस् |
| **क्याटलग** | `catalog.GetCachedModelsAsync()` | क्याच गरिएका मोडेलहरूको सूची |
| **क्याटलग** | `catalog.GetLoadedModelsAsync()` | लोड गरिएका मोडेलहरूको सूची |
| **मोडेल** | `model.DownloadAsync(progress?)` | मोडेल डाउनलोड गर्नुहोस् |
| **मोडेल** | `model.LoadAsync()` | मोडेल लोड गर्नुहोस् |
| **मोडेल** | `model.UnloadAsync()` | मोडेल अनलोड गर्नुहोस् |
| **मोडेल** | `model.SelectVariant(variant)` | हार्डवेयर भेरियन्ट चयन गर्नुहोस् |
| **मोडेल** | `model.GetChatClientAsync()` | नेटिभ च्याट क्लाइन्ट प्राप्त गर्नुहोस् |
| **मोडेल** | `model.GetAudioClientAsync()` | अडियो ट्रान्सक्रिप्सन क्लाइन्ट प्राप्त गर्नुहोस् |
| **मोडेल** | `model.GetPathAsync()` | स्थानीय फाइल पथ प्राप्त गर्नुहोस् |
| **क्याटलग** | `catalog.GetModelVariantAsync(alias, variant)` | विशेष हार्डवेयर भेरियन्ट प्राप्त गर्नुहोस् |
| **क्याटलग** | `catalog.UpdateModelsAsync()` | क्याटलग रिफ्रेश गर्नुहोस् |
| **सर्भर** | `manager.StartWebServerAsync()` | REST वेब सर्भर सुरु गर्नुहोस् |
| **सर्भर** | `manager.StopWebServerAsync()` | REST वेब सर्भर रोक्नुहोस् |
| **कन्फिग** | `config.ModelCacheDir` | क्याच डाइरेक्टोरी |

---

## मुख्य कुरा याद गर्ने

| अवधारणा | तपाईंले के सिक्नुभयो |
|---------|-----------------|
| **SDK vs CLI** | SDK ले प्रोग्राम्याटिक नियन्त्रण दिन्छ - एप्लिकेसनहरूका लागि आवश्यक |
| **कन्ट्रोल प्लेन** | SDK सेवाहरू, मोडेलहरू, र क्याचिंग व्यवस्थापन गर्छ |
| **डायनामिक पोर्टहरू** | सदैव `manager.endpoint` (Python) वा `manager.urls[0]` (JS/C#) प्रयोग गर्नुहोस् - पोर्ट कहिल्यै हार्डकोड नगर्नुहोस् |
| **एलियसहरू** | स्वचालित हार्डवेयर-अनुकूल मोडेल चयनका लागि एलियसहरू प्रयोग गर्नुहोस् |
| **छिटो सुरूवात** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# पुनःडिजाइन** | v0.8.0+ स्व-सम्पूर्ण छ - अन्तिम प्रयोगकर्ताका कम्प्युटरमा CLI आवश्यक छैन |
| **मोडेल जीवनचक्र** | सूची → डाउनलोड → लोड → प्रयोग → अनलोड |
| **FoundryModelInfo** | समृद्ध मेटाडाटा: कार्य, उपकरण, आकार, लाइसेन्स, उपकरण कल समर्थन |
| **ChatClient** | OpenAI-मुक्त प्रयोगका लागि `createChatClient()` (JS) / `GetChatClientAsync()` (C#) |
| **भेरियन्टहरू** | मोडेलहरूसँग हार्डवेयर-विशिष्ट भेरियन्टहरू हुन्छन् (CPU, GPU, NPU); स्वतः चयन गरिन्छ |
| **अップग्रेडहरू** | Python: `is_model_upgradeable()` + `upgrade_model()` ले मोडेलहरू अद्यावधिक राख्छ |
| **सूची नवीकरण** | नयाँ मोडेलहरू पत्ता लगाउन `refresh_catalog()` (Python) / `updateModels()` (JS) |

---

## स्रोतहरू

| स्रोत | लिंक |
|----------|------|
| SDK सन्दर्भ (सबै भाषा) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| अनुमान SDK सँग समेकन गर्नुहोस् | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API सन्दर्भ | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK नमूनाहरू | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local वेबसाइट | [foundrylocal.ai](https://foundrylocal.ai) |

---

## भोलिको चरणहरू

SDK लाई OpenAI क्लाइन्ट पुस्तकालयसँग जडान गर्न र तपाईंको पहिलो च्याट पूर्णता एप्लिकेशन बनाउन [भाग 3: OpenAI सँग SDK प्रयोग गर्दै](part3-sdk-and-apis.md) मा जानुहोस्।

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:
यो दस्तावेज AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) प्रयोग गरेर अनुवाद गरिएको हो। हामी शुद्धताका लागि प्रयासरत छौं, तर कृपया जानकार हुनुहोस् कि स्वचालित अनुवादहरूमा त्रुटिहरू वा गलत जानकारी हुन सक्छ। मौलिक दस्तावेज यसको मूल भाषामा आधिकारिक स्रोतको रूपमा मानिनु पर्छ। महत्वपूर्ण जानकारीका लागि, व्यावसायिक मानव अनुवाद सिफारिस गरिन्छ। यो अनुवादको प्रयोगबाट उत्पन्न कुनै पनि गलतफहमी वा गलत व्याख्याका लागि हामी जिम्मेवार हौंन।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->