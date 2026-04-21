![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग 2: Foundry Local SDK गहराई से अध्ययन

> **लक्ष्य:** Foundry Local SDK में महारत हासिल करें ताकि मॉडल, सेवाओं और कैशिंग को प्रोग्रामैटिक रूप से प्रबंधित किया जा सके - और समझें कि SDK क्यों CLI की तुलना में एप्लिकेशन बनाने के लिए अनुशंसित तरीका है।

## अवलोकन

भाग 1 में आपने **Foundry Local CLI** का उपयोग करके इंटरएक्टिव रूप से मॉडल डाउनलोड और चलाए। CLI खोज के लिए अच्छा है, लेकिन जब आप असली एप्लिकेशन बनाते हैं तो आपको **प्रोग्रामैटिक नियंत्रण** चाहिए। Foundry Local SDK आपको यह देता है - यह **कंट्रोल प्लेन** (सेवा शुरू करना, मॉडल खोजना, डाउनलोड करना, लोड करना) प्रबंधित करता है ताकि आपका एप्लिकेशन कोड **डेटा प्लेन** (प्रॉम्प्ट भेजना, पूर्णताएँ प्राप्त करना) पर ध्यान केंद्रित कर सके।

यह प्रयोगशाला आपको Python, JavaScript, और C# के बीच SDK API के पूर्ण सतह को सिखाती है। अंत तक आप हर उपलब्ध विधि को समझेंगे और कब कौन सी उपयोग करनी है।

## शिक्षण उद्देश्य

इस प्रयोगशाला के अंत तक आप सक्षम होंगे:

- समझाना कि एप्लिकेशन विकास के लिए SDK CLI से बेहतर क्यों है
- Python, JavaScript, या C# के लिए Foundry Local SDK स्थापित करना
- `FoundryLocalManager` का उपयोग करके सेवा शुरू करना, मॉडल प्रबंधित करना, और कैटलॉग को क्वेरी करना
- प्रोग्रामैटिक रूप से मॉडल सूचीबद्ध करना, डाउनलोड करना, लोड करना, और अनलोड करना
- `FoundryModelInfo` का उपयोग करके मॉडल मेटाडेटा निरीक्षण करना
- कैटलॉग, कैश, और लोड किए गए मॉडलों के बीच अंतर समझना
- कंस्ट्रक्टर बूटस्ट्रैप (Python) और `create()` + कैटलॉग पैटर्न (JavaScript) का उपयोग करना
- C# SDK पुनःडिज़ाइन और उसकी ऑब्जेक्ट-ओरिएंटेड API समझना

---

## पूर्वापेक्षाएँ

| आवश्यकता | विवरण |
|-------------|---------|
| **Foundry Local CLI** | स्थापित और आपके `PATH` पर ([भाग 1](part1-getting-started.md)) |
| **भाषा रनटाइम** | **Python 3.9+** और/या **Node.js 18+** और/या **.NET 9.0+** |

---

## अवधारणा: SDK बनाम CLI - SDK क्यों उपयोग करें?

| पहलू | CLI (`foundry` कमांड) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **उपयोग केस** | अन्वेषण, मैनुअल परीक्षण | एप्लिकेशन एकीकरण |
| **सेवा प्रबंधन** | मैनुअल: `foundry service start` | स्वचालित: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **पोर्ट खोज** | CLI आउटपुट से पढ़ें | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **मॉडल डाउनलोड** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **त्रुटि प्रबंधन** | एग्जिट कोड, stderr | अपवाद, टाइप किए गए त्रुटियाँ |
| **स्वचालन** | शेल स्क्रिप्ट | मूल भाषा एकीकरण |
| **डिप्लॉयमेंट** | अंत-उपयोगकर्ता मशीन पर CLI आवश्यक | C# SDK स्वयं-संपूर्ण हो सकता है (CLI की आवश्यकता नहीं) |

> **मुख्य अंतर्दृष्टि:** SDK संपूर्ण जीवनचक्र को संभालता है: सेवा शुरू करना, कैश जांचना, लापता मॉडल डाउनलोड करना, उन्हें लोड करना, और एंडपॉइंट खोजना, कुछ लाइनों कोड में। आपके एप्लिकेशन को CLI आउटपुट पार्स करने या subprocesses को प्रबंधित करने की जरूरत नहीं है।

---

## प्रयोगशाला अभ्यास

### अभ्यास 1: SDK स्थापित करें

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

स्थापना सत्यापित करें:

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

स्थापना सत्यापित करें:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

दो NuGet पैकेज हैं:

| पैकेज | प्लेटफ़ॉर्म | विवरण |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | क्रॉस-प्लेटफ़ॉर्म | विंडोज, लिनक्स, मैकओएस पर काम करता है |
| `Microsoft.AI.Foundry.Local.WinML` | केवल विंडोज | WinML हार्डवेयर एक्सेलेरेशन जोड़ता है; प्लगइन निष्पादन प्रदाता (जैसे Qualcomm NPU के लिए QNN) डाउनलोड और इंस्टॉल करता है |

**विंडोज सेटअप:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` फ़ाइल संपादित करें:

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

> **नोट:** विंडोज पर, WinML पैकेज एक सुपरसैट है जिसमें बेस SDK और QNN execution provider शामिल हैं। लिनक्स/मैकओएस पर, बेस SDK का उपयोग किया जाता है। कंडिशनल TFM और पैकेज संदर्भ प्रोजेक्ट को पूरी तरह क्रॉस-प्लेटफ़ॉर्म रखते हैं।

प्रोजेक्ट रूट में `nuget.config` बनाएँ:

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

पैकेज पुनर्स्थापित करें:

```bash
dotnet restore
```

</details>

---

### अभ्यास 2: सेवा शुरू करें और कैटलॉग सूचीबद्ध करें

कोई भी एप्लिकेशन सबसे पहले Foundry Local सेवा शुरू करता है और उपलब्ध मॉडलों को खोजता है।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# एक मैनेजर बनाएं और सेवा प्रारंभ करें
manager = FoundryLocalManager()
manager.start_service()

# कैटलॉग में उपलब्ध सभी मॉडल सूचीबद्ध करें
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - सेवा प्रबंधन विधियाँ

| विधि | सिग्नेचर | विवरण |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | जांचें कि सेवा चल रही है या नहीं |
| `start_service()` | `() -> None` | Foundry Local सेवा शुरू करें |
| `service_uri` | `@property -> str` | आधार सेवा URI |
| `endpoint` | `@property -> str` | API एंडपॉइंट (सेवा URI + `/v1`) |
| `api_key` | `@property -> str` | API कुंजी (env या डिफ़ॉल्ट प्लेसहोल्डर से) |

#### Python SDK - कैटलॉग प्रबंधन विधियाँ

| विधि | सिग्नेचर | विवरण |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | कैटलॉग में सभी मॉडल सूचीबद्ध करें |
| `refresh_catalog()` | `() -> None` | सेवा से कैटलॉग ताज़ा करें |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | किसी विशिष्ट मॉडल की जानकारी प्राप्त करें |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// एक प्रबंधक बनाएं और सेवा शुरू करें
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// कैटलॉग ब्राउज़ करें
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - मैनेजर विधियाँ

| विधि | सिग्नेचर | विवरण |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK सिंगलटन शुरू करें |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | सिंगलटन मैनेजर तक पहुंचें |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local वेब सेवा शुरू करें |
| `manager.urls` | `string[]` | सेवा के लिए बेस URL की सूची |

#### JavaScript SDK - कैटलॉग और मॉडल विधियाँ

| विधि | सिग्नेचर | विवरण |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | मॉडल कैटलॉग तक पहुंचें |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | उपनाम द्वारा मॉडल ऑब्जेक्ट प्राप्त करें |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ ऑब्जेक्ट-ओरिएंटेड आर्किटेक्चर का उपयोग करता है जिसमें `Configuration`, `Catalog`, और `Model` ऑब्जेक्ट शामिल हैं:

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

#### C# SDK - प्रमुख क्लासेस

| क्लास | उद्देश्य |
|-------|---------|
| `Configuration` | ऐप नाम, लॉग स्तर, कैश डायरेक्टरी, वेब सर्वर URL सेट करें |
| `FoundryLocalManager` | मुख्य एंट्री पॉइंट - `CreateAsync()` से बनाया गया, `.Instance` के माध्यम से एक्सेस |
| `Catalog` | कैटलॉग से मॉडल ब्राउज, सर्च और प्राप्त करें |
| `Model` | एक विशिष्ट मॉडल का प्रतिनिधित्व करता है - डाउनलोड, लोड, क्लाइंट प्राप्त करें |

#### C# SDK - मैनेजर और कैटलॉग विधियाँ

| विधि | विवरण |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | मैनेजर शुरू करें |
| `FoundryLocalManager.Instance` | सिंगलटन मैनेजर तक पहुंचिए |
| `manager.GetCatalogAsync()` | मॉडल कैटलॉग प्राप्त करें |
| `catalog.ListModelsAsync()` | सभी उपलब्ध मॉडल सूचीबद्ध करें |
| `catalog.GetModelAsync(alias: "alias")` | उपनाम द्वारा विशिष्ट मॉडल प्राप्त करें |
| `catalog.GetCachedModelsAsync()` | डाउनलोड किए गए मॉडल सूचीबद्ध करें |
| `catalog.GetLoadedModelsAsync()` | वर्तमान में लोड किए गए मॉडल सूचीबद्ध करें |

> **C# आर्किटेक्चर नोट:** C# SDK v0.8.0+ का पुनःडिज़ाइन एप्लिकेशन को **स्वयं-संपूर्ण** बनाता है; इसे उपयोगकर्ता मशीन पर Foundry Local CLI की आवश्यकता नहीं है। SDK मॉडल प्रबंधन और अनुमान को मूल रूप से संभालता है।

</details>

---

### अभ्यास 3: मॉडल डाउनलोड और लोड करें

SDK डाउनलोडिंग (डिस्क पर) को लोडिंग (मेमोरी में) से अलग करता है। इससे आप पूर्व-सेटअप के दौरान मॉडल प्री-डाउनलोड कर सकते हैं और मांग पर लोड कर सकते हैं।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# विकल्प A: मैनुअल चरण-दर-चरण
manager = FoundryLocalManager()
manager.start_service()

# पहले कैश जांचें
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

# विकल्प B: वन-लाइनर बूटस्ट्रैप (अनुशंसित)
# कंस्ट्रक्टर को उपनाम पास करें - यह सेवा शुरू करता है, डाउनलोड करता है, और स्वचालित रूप से लोड करता है
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - मॉडल प्रबंधन विधियाँ

| विधि | सिग्नेचर | विवरण |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | मॉडल को स्थानीय कैश में डाउनलोड करें |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | मॉडल को अनुमान सर्वर में लोड करें |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | सर्वर से मॉडल अनलोड करें |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | वर्तमान में लोड किए गए सभी मॉडल सूचीबद्ध करें |

#### Python - कैश प्रबंधन विधियाँ

| विधि | सिग्नेचर | विवरण |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | कैश डायरेक्टरी पथ प्राप्त करें |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | डाउनलोड किए गए सभी मॉडल सूचीबद्ध करें |

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

#### JavaScript - मॉडल विधियाँ

| विधि | सिग्नेचर | विवरण |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | क्या मॉडल पहले से डाउनलोड है |
| `model.download()` | `() => Promise<void>` | मॉडल को स्थानीय कैश में डाउनलोड करें |
| `model.load()` | `() => Promise<void>` | अनुमान सर्वर में लोड करें |
| `model.unload()` | `() => Promise<void>` | अनुमान सर्वर से अनलोड करें |
| `model.id` | `string` | मॉडल की अद्वितीय पहचान |

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

#### C# - मॉडल विधियाँ

| विधि | विवरण |
|--------|-------------|
| `model.DownloadAsync(progress?)` | चयनित वेरिएंट डाउनलोड करें |
| `model.LoadAsync()` | मॉडल को मेमोरी में लोड करें |
| `model.UnloadAsync()` | मॉडल को अनलोड करें |
| `model.SelectVariant(variant)` | विशिष्ट वेरिएंट चुनें (CPU/GPU/NPU) |
| `model.SelectedVariant` | वर्तमान में चयनित वेरिएंट |
| `model.Variants` | इस मॉडल के लिए सभी उपलब्ध वेरिएंट |
| `model.GetPathAsync()` | स्थानीय फ़ाइल पथ प्राप्त करें |
| `model.GetChatClientAsync()` | मूल चैट क्लाइंट प्राप्त करें (OpenAI SDK आवश्यक नहीं) |
| `model.GetAudioClientAsync()` | ट्रांसक्रिप्शन के लिए ऑडियो क्लाइंट प्राप्त करें |

</details>

---

### अभ्यास 4: मॉडल मेटाडेटा निरीक्षण करें

`FoundryModelInfo` ऑब्जेक्ट में प्रत्येक मॉडल के बारे में समृद्ध मेटाडेटा होता है। इन क्षेत्रों को समझना आपको आपके एप्लिकेशन के लिए सही मॉडल चुनने में मदद करता है।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# किसी विशेष मॉडल के बारे में विस्तृत जानकारी प्राप्त करें
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

#### FoundryModelInfo क्षेत्र

| क्षेत्र | प्रकार | विवरण |
|-------|------|-------------|
| `alias` | string | संक्षिप्त नाम (उदाहरण के लिए `phi-3.5-mini`) |
| `id` | string | अद्वितीय मॉडल पहचानकर्ता |
| `version` | string | मॉडल संस्करण |
| `task` | string | `chat-completions` या `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, या NPU |
| `execution_provider` | string | रनटाइम बैकेंड (CUDA, CPU, QNN, WebGPU, आदि) |
| `file_size_mb` | int | डिस्क पर आकार (MB में) |
| `supports_tool_calling` | bool | क्या मॉडल फ़ंक्शन/टूल कॉलिंग का समर्थन करता है |
| `publisher` | string | किसने मॉडल प्रकाशित किया |
| `license` | string | लाइसेंस नाम |
| `uri` | string | मॉडल URI |
| `prompt_template` | dict/null | प्रॉम्प्ट टेम्पलेट, यदि कोई हो |

---

### अभ्यास 5: मॉडल जीवनचक्र प्रबंधन

पूरे जीवनचक्र का अभ्यास करें: सूचीबद्ध करें → डाउनलोड करें → लोड करें → उपयोग करें → अनलोड करें।

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # त्वरित परीक्षण के लिए छोटा मॉडल

manager = FoundryLocalManager()
manager.start_service()

# 1. जांचें कि कैटलॉग में क्या है
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. जांचें कि क्या पहले से डाउनलोड किया गया है
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. एक मॉडल डाउनलोड करें
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. पुष्टि करें कि यह अब कैश में है
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. इसे लोड करें
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. जांचें कि क्या लोड किया गया है
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. इसे अनलोड करें
print(f"\nUnloading {alias}...")
manager.unload_model(alias)
loaded = manager.list_loaded_models()
print(f"Loaded models after unload: {len(loaded)}")
```

</details>

<details>
<summary><h3>📘 जावास्क्रिप्ट</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "qwen2.5-0.5b"; // त्वरित परीक्षण के लिए छोटा मॉडल

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. कैटलॉग से मॉडल प्राप्त करें
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. यदि आवश्यक हो तो डाउनलोड करें
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. इसे लोड करें
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. इसे अनलोड करें
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### अभ्यास 6: त्वरित-प्रारंभ पैटर्न्स

प्रत्येक भाषा सेवा को शुरू करने और एक कॉल में मॉडल लोड करने के लिए शॉर्टकट प्रदान करती है। ये अधिकांश अनुप्रयोगों के लिए **अनुशंसित पैटर्न** हैं।

<details>
<summary><h3>🐍 पाइथन - कंस्ट्रक्टर बूटस्ट्रैप</h3></summary>

```python
from foundry_local import FoundryLocalManager

# कंस्ट्रक्टर को एक उपनाम पास करें - यह सब कुछ संभालता है:
# 1. यदि सेवा चल नहीं रही है तो इसे शुरू करता है
# 2. यदि मॉडल कैश में नहीं है तो इसे डाउनलोड करता है
# 3. मॉडल को इन्फेरेंस सर्वर में लोड करता है
manager = FoundryLocalManager("phi-3.5-mini")

# तुरंत उपयोग के लिए तैयार
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` पैरामीटर (डिफ़ॉल्ट `True`) इस व्यवहार को नियंत्रित करता है। यदि आप मैनुअल नियंत्रण चाहते हैं तो `bootstrap=False` सेट करें:

```python
# मैनुअल मोड - कुछ भी स्वचालित रूप से नहीं होता है
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 जावास्क्रिप्ट - `create()` + कैटलॉग</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() सब कुछ संभालता है:
// 1. सेवा शुरू करता है
// 2. कैटलॉग से मॉडल प्राप्त करता है
// 3. आवश्यकता होने पर डाउनलोड करता है और मॉडल लोड करता है
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// तुरंत उपयोग के लिए तैयार
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + कैटलॉग</h3></summary>

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

> **C# नोट:** C# SDK `Configuration` का उपयोग करता है ऐप नाम, लॉगिंग, कैश डायरेक्टरीज़, और यहां तक कि एक विशिष्ट वेब सर्वर पोर्ट पिन करने के लिए। यह इसे तीनों SDK में सबसे कॉन्फ़िगरेबल बनाता है।

</details>

---

### अभ्यास 7: नेटिव ChatClient (कोई OpenAI SDK आवश्यक नहीं)

जावास्क्रिप्ट और C# SDK एक `createChatClient()` सुविधा विधि प्रदान करते हैं जो एक नेटिव चैट क्लाइंट लौटाती है — अलग से OpenAI SDK स्थापित या कॉन्फ़िगर करने की आवश्यकता नहीं है।

<details>
<summary><h3>📘 जावास्क्रिप्ट - <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// मॉडल से सीधे ChatClient बनाएं — OpenAI इम्पोर्ट की जरूरत नहीं
const chatClient = model.createChatClient();

// completeChat एक OpenAI-अनुकूल प्रतिक्रिया वस्तु लौटाता है
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// स्ट्रीमिंग में callback पैटर्न का उपयोग होता है
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` टूल कॉलिंग का भी समर्थन करता है — टूल्स को दूसरे आर्गुमेंट के रूप में पास करें:

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

> **कौन सा पैटर्न कब इस्तेमाल करें:**
> - **`createChatClient()`** — त्वरित प्रोटोटाइपिंग, कम डिपेंडेंसीज़, सरल कोड
> - **OpenAI SDK** — पैरामीटर (temperature, top_p, stop tokens, आदि) पर पूर्ण नियंत्रण, उत्पादन अनुप्रयोगों के लिए बेहतर

---

### अभ्यास 8: मॉडल वेरिएंट और हार्डवेयर चयन

मॉडल के कई **वेरिएंट** हो सकते हैं जो विभिन्न हार्डवेयर के लिए अनुकूलित होते हैं। SDK स्वचालित रूप से सर्वश्रेष्ठ वेरिएंट चुनता है, लेकिन आप मैन्युअल रूप से भी निरीक्षण और चयन कर सकते हैं।

<details>
<summary><h3>📘 जावास्क्रिप्ट</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// सभी उपलब्ध वेरिएंट सूचीबद्ध करें
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK स्वचालित रूप से आपके हार्डवेयर के लिए सर्वोत्तम वेरिएंट का चयन करता है
// ओवरराइड करने के लिए, selectVariant() का उपयोग करें:
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
<summary><h3>🐍 पाइथन</h3></summary>

पाइथन में, SDK हार्डवेयर के आधार पर स्वचालित रूप से सर्वश्रेष्ठ वेरिएंट चुनता है। यह देखने के लिए `get_model_info()` का उपयोग करें कि क्या चुना गया था:

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

#### NPU वेरिएंट वाले मॉडल

कुछ मॉडल NPU-ऑप्टिमाइज्ड वेरिएंट के साथ आते हैं, जो Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra) वाले डिवाइस के लिए हैं:

| मॉडल | NPU वेरिएंट उपलब्ध |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **टिप:** NPU समर्थित हार्डवेयर पर, SDK उपलब्ध होने पर स्वचालित रूप से NPU वेरिएंट चुनता है। आपको कोड को बदलने की आवश्यकता नहीं है। Windows पर C# प्रोजेक्ट्स के लिए, `Microsoft.AI.Foundry.Local.WinML` NuGet पैकेज जोड़ें ताकि QNN execution provider सक्षम हो सके — QNN WinML के माध्यम से एक प्लगइन EP के रूप में वितरित किया जाता है।

---

### अभ्यास 9: मॉडल अपडेट और कैटलॉग रिफ्रेश

मॉडल कैटलॉग समय-समय पर अपडेट होता है। अपडेट्स की जांच करने और लागू करने के लिए इन विधियों का उपयोग करें।

<details>
<summary><h3>🐍 पाइथन</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# नवीनतम मॉडल सूची प्राप्त करने के लिए कैटलॉग को ताज़ा करें
manager.refresh_catalog()

# जांचें कि क्या किसी कैश्ड मॉडल का नया संस्करण उपलब्ध है
if manager.is_model_upgradeable(alias):
    print(f"{alias} has a newer version available!")
    manager.upgrade_model(alias)
    print("Upgrade complete")
else:
    print(f"{alias} is up to date")
```

</details>

<details>
<summary><h3>📘 जावास्क्रिप्ट</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "UpgradeDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// नवीनतम मॉडल सूची प्राप्त करने के लिए कैटलॉग को ताज़ा करें
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// ताज़ा करने के बाद सभी उपलब्ध मॉडल सूचीबद्ध करें
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### अभ्यास 10: रीज़निंग मॉडल के साथ काम करना

**phi-4-mini-reasoning** मॉडल में चेन-ऑफ-थॉट रीज़निंग शामिल है। यह अंतिम उत्तर प्रदान करने से पहले अपनी आंतरिक सोच को `<think>...</think>` टैग्स में लपेटता है। यह उन कार्यों के लिए उपयोगी है जिनमें बहु-चरण तर्क, गणित, या समस्या-समाधान शामिल हैं।

<details>
<summary><h3>🐍 पाइथन</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning लगभग 4.6 जीबी है
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# मॉडल अपने विचारों को <think>...</think> टैग्स में लपेटता है
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
<summary><h3>📘 जावास्क्रिप्ट</h3></summary>

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

// सोच की श्रृंखला को पार्स करें
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **रिजनिंग मॉडल कब उपयोग करें:**
> - गणित और तर्क समस्याएँ
> - बहु-चरण योजना कार्य
> - जटिल कोड जनरेशन
> - कार्य जहां कार्य दिखाने से सटीकता बढ़ती है
>
> **ट्रेड-ऑफ:** रीज़निंग मॉडल अधिक टोकन (जैसे `<think>` सेक्शन) उत्पन्न करते हैं और धीमे होते हैं। सरल प्रश्नोत्तरों के लिए, phi-3.5-mini जैसे सामान्य मॉडल तेज़ होते हैं।

---

### अभ्यास 11: उपनाम और हार्डवेयर चयन को समझना

जब आप एक **उपनाम** (जैसे `phi-3.5-mini`) पास करते हैं न कि पूर्ण मॉडल आईडी, तो SDK स्वचालित रूप से आपके हार्डवेयर के लिए सर्वश्रेष्ठ वेरिएंट चुनता है:

| हार्डवेयर | चयनित Execution Provider |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML प्लगइन के माध्यम से) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| कोई भी डिवाइस (फ़ैलबैक) | `CPUExecutionProvider` या `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# यह उपनाम आपके हार्डवेयर के लिए सबसे उपयुक्त संस्करण को हल करता है
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **टिप:** अपने एप्लिकेशन कोड में हमेशा उपनामों का उपयोग करें। जब आप किसी उपयोगकर्ता के मशीन पर तैनात करते हैं, तो SDK रनटाइम पर इष्टतम वेरिएंट चुनता है - NVIDIA पर CUDA, Qualcomm पर QNN, अन्य जगहों पर CPU।

---

### अभ्यास 12: C# कॉन्फ़िगरेशन विकल्प

C# SDK के `Configuration` क्लास रनटाइम पर सूक्ष्म नियंत्रण प्रदान करते हैं:

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

| गुण | डिफ़ॉल्ट | विवरण |
|----------|---------|-------------|
| `AppName` | (आवश्यक) | आपके एप्लिकेशन का नाम |
| `LogLevel` | `Information` | लॉगिंग की परिचालना स्तर |
| `Web.Urls` | (डायनेमिक) | वेब सर्वर के लिए एक विशिष्ट पोर्ट पिन करें |
| `AppDataDir` | OS डिफ़ॉल्ट | ऐप डेटा के लिए बेस डायरेक्टरी |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | जहां मॉडल स्टोर होते हैं |
| `LogsDir` | `{AppDataDir}/logs` | जहां लॉग लिखे जाते हैं |

---

### अभ्यास 13: ब्राउज़र उपयोग (केवल जावास्क्रिप्ट)

जावास्क्रिप्ट SDK में एक ब्राउज़र- अनुकूल संस्करण शामिल है। ब्राउज़र में, आपको CLI के माध्यम से मैन्युअल रूप से सेवा शुरू करनी होगी और होस्ट URL निर्दिष्ट करना होगा:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// सेवा को मैन्युअल रूप से पहले शुरू करें:
//   foundry सेवा शुरू करें
// फिर CLI आउटपुट से URL का उपयोग करें
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// श्रेणी सूची ब्राउज़ करें
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **ब्राउज़र सीमाएँ:** ब्राउज़र संस्करण `startWebService()` को सपोर्ट नहीं करता। SDK का उपयोग करने से पहले सुनिश्चित करें कि Foundry Local सेवा पहले से चल रही हो।

---

## पूर्ण API संदर्भ

### पाइथन

| श्रेणी | विधि | विवरण |
|----------|--------|-------------|
| **इनिशियलाइजेशन** | `FoundryLocalManager(alias?, bootstrap=True)` | मैनेजर बनाएं; ऑप्शनल रूप से मॉडल के साथ बूटस्ट्रैप करें |
| **सेवा** | `is_service_running()` | जांचें कि सेवा चल रही है या नहीं |
| **सेवा** | `start_service()` | सेवा शुरू करें |
| **सेवा** | `endpoint` | API एंडपॉइंट URL |
| **सेवा** | `api_key` | API कुंजी |
| **कैटलॉग** | `list_catalog_models()` | उपलब्ध सभी मॉडल सूचीबद्ध करें |
| **कैटलॉग** | `refresh_catalog()` | कैटलॉग रिफ्रेश करें |
| **कैटलॉग** | `get_model_info(alias_or_model_id)` | मॉडल मेटाडेटा प्राप्त करें |
| **कैश** | `get_cache_location()` | कैश डायरेक्टरी पथ |
| **कैश** | `list_cached_models()` | डाउनलोड किए गए मॉडल सूचीबद्ध करें |
| **मॉडल** | `download_model(alias_or_model_id)` | मॉडल डाउनलोड करें |
| **मॉडल** | `load_model(alias_or_model_id, ttl=600)` | मॉडल लोड करें |
| **मॉडल** | `unload_model(alias_or_model_id)` | मॉडल अनलोड करें |
| **मॉडल** | `list_loaded_models()` | लोड किए गए मॉडल सूचीबद्ध करें |
| **मॉडल** | `is_model_upgradeable(alias_or_model_id)` | जांचें कि नया संस्करण उपलब्ध है या नहीं |
| **मॉडल** | `upgrade_model(alias_or_model_id)` | मॉडल को नवीनतम संस्करण में अपग्रेड करें |
| **सेवा** | `httpx_client` | डायरेक्ट API कॉल के लिए प्री-कॉन्फ़िगर HTTPX क्लाइंट |

### जावास्क्रिप्ट

| श्रेणी | विधि | विवरण |
|----------|--------|-------------|
| **इनिशियलाइजेशन** | `FoundryLocalManager.create(options)` | SDK सिंगिलटन इनिशियलाइज़ करें |
| **इनिशियलाइजेशन** | `FoundryLocalManager.instance` | सिंगिलटन मैनेजर एक्सेस करें |
| **सेवा** | `manager.startWebService()` | वेब सेवा शुरू करें |
| **सेवा** | `manager.urls` | सेवा के बेस URL की सूची |
| **कैटलॉग** | `manager.catalog` | मॉडल कैटलॉग एक्सेस करें |
| **कैटलॉग** | `catalog.getModel(alias)` | उपनाम द्वारा मॉडल ऑब्जेक्ट प्राप्त करें (प्रॉमिस लौटाता है) |
| **मॉडल** | `model.isCached` | क्या मॉडल डाउनलोड हो चुका है |
| **मॉडल** | `model.download()` | मॉडल डाउनलोड करें |
| **मॉडल** | `model.load()` | मॉडल लोड करें |
| **मॉडल** | `model.unload()` | मॉडल अनलोड करें |
| **मॉडल** | `model.id` | मॉडल की यूनिक आईडी |
| **मॉडल** | `model.alias` | मॉडल का उपनाम |
| **मॉडल** | `model.createChatClient()` | नेटिव चैट क्लाइंट प्राप्त करें (कोई OpenAI SDK आवश्यक नहीं) |
| **मॉडल** | `model.createAudioClient()` | ट्रांस्क्रिप्शन के लिए ऑडियो क्लाइंट प्राप्त करें |
| **मॉडल** | `model.removeFromCache()` | लोकल कैश से मॉडल हटाएं |
| **मॉडल** | `model.selectVariant(variant)` | एक विशिष्ट हार्डवेयर वेरिएंट चुनें |
| **मॉडल** | `model.variants` | इस मॉडल के लिए उपलब्ध वेरिएंट्स की सूची |
| **मॉडल** | `model.isLoaded()` | जांचें कि मॉडल लोड है या नहीं |
| **कैटलॉग** | `catalog.getModels()` | उपलब्ध सभी मॉडल सूचीबद्ध करें |
| **कैटलॉग** | `catalog.getCachedModels()` | डाउनलोड किए गए मॉडल सूचीबद्ध करें |
| **कैटलॉग** | `catalog.getLoadedModels()` | वर्तमान में लोड किए गए मॉडल सूचीबद्ध करें |
| **कैटलॉग** | `catalog.updateModels()` | सेवा से कैटलॉग रिफ्रेश करें |
| **सेवा** | `manager.stopWebService()` | Foundry Local वेब सेवा बंद करें |

### C# (v0.8.0+)

| श्रेणी | विधि | विवरण |
|----------|--------|-------------|
| **इनिशियलाइजेशन** | `FoundryLocalManager.CreateAsync(config, logger)` | मैनेजर इनिशियलाइज़ करें |
| **इनिशियलाइजेशन** | `FoundryLocalManager.Instance` | सिंगिलटन एक्सेस करें |
| **कैटलॉग** | `manager.GetCatalogAsync()` | कैटलॉग प्राप्त करें |
| **कैटलॉग** | `catalog.ListModelsAsync()` | सभी मॉडल सूचीबद्ध करें |
| **कैटलॉग** | `catalog.GetModelAsync(alias)` | एक विशिष्ट मॉडल प्राप्त करें |
| **कैटलॉग** | `catalog.GetCachedModelsAsync()` | कैश किए गए मॉडल सूचीबद्ध करें |
| **कैटलॉग** | `catalog.GetLoadedModelsAsync()` | लोड किए गए मॉडल सूचीबद्ध करें |
| **मॉडल** | `model.DownloadAsync(progress?)` | मॉडल डाउनलोड करें |
| **मॉडल** | `model.LoadAsync()` | मॉडल लोड करें |
| **मॉडल** | `model.UnloadAsync()` | मॉडल अनलोड करें |
| **मॉडल** | `model.SelectVariant(variant)` | हार्डवेयर वेरिएंट चुनें |
| **मॉडल** | `model.GetChatClientAsync()` | नेटिव चैट क्लाइंट प्राप्त करें |
| **मॉडल** | `model.GetAudioClientAsync()` | ऑडियो ट्रांसक्रिप्शन क्लाइंट प्राप्त करें |
| **मॉडल** | `model.GetPathAsync()` | लोकल फाइल पथ प्राप्त करें |
| **कैटलॉग** | `catalog.GetModelVariantAsync(alias, variant)` | एक विशिष्ट हार्डवेयर वेरिएंट प्राप्त करें |
| **कैटलॉग** | `catalog.UpdateModelsAsync()` | कैटलॉग रिफ्रेश करें |
| **सर्वर** | `manager.StartWebServerAsync()` | REST वेब सर्वर शुरू करें |
| **सर्वर** | `manager.StopWebServerAsync()` | REST वेब सर्वर बंद करें |
| **कॉन्फ़िग** | `config.ModelCacheDir` | कैश डायरेक्टरी |

---

## प्रमुख निष्कर्ष

| अवधारणा | आपने क्या सीखा |
|---------|-----------------|
| **SDK बनाम CLI** | SDK प्रोग्रामैटिक नियंत्रण प्रदान करता है - अनुप्रयोगों के लिए आवश्यक |
| **कंट्रोल प्लेन** | SDK सेवाओं, मॉडलों, और कैशिंग का प्रबंधन करता है |
| **डायनेमिक पोर्ट** | हमेशा `manager.endpoint` (पाइथन) या `manager.urls[0]` (JS/C#) का उपयोग करें - कभी पोर्ट हार्डकोड न करें |
| **उपनाम** | हार्डवेयर-इष्टतम मॉडल चयन के लिए उपनामों का उपयोग करें |
| **त्वरित प्रारंभ** | पाइथन: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# पुनः डिज़ाइन** | v0.8.0+ स्व-निहित है - अंतिम उपयोगकर्ता मशीनों पर कोई CLI आवश्यक नहीं है |
| **मॉडल जीवनचक्र** | कैटलॉग → डाउनलोड → लोड → उपयोग → अनलोड |
| **FoundryModelInfo** | समृद्ध मेटाडेटा: कार्य, उपकरण, आकार, लाइसेंस, टूल कॉलिंग समर्थन |
| **ChatClient** | ओपनएआई-मुक्त उपयोग के लिए `createChatClient()` (JS) / `GetChatClientAsync()` (C#) |
| **वेरिएंट्स** | मॉडलों के पास हार्डवेयर-विशिष्ट वेरिएंट्स होते हैं (CPU, GPU, NPU); स्वचालित रूप से चयनित |
| **अपग्रेड्स** | पाइथन: वर्तमान मॉडल बनाए रखने के लिए `is_model_upgradeable()` + `upgrade_model()` |
| **कैटलॉग ताज़ा करें** | नए मॉडल खोजने के लिए `refresh_catalog()` (Python) / `updateModels()` (JS) |

---

## संसाधन

| संसाधन | लिंक |
|----------|------|
| SDK संदर्भ (सभी भाषाएँ) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| इन्फरेंस SDKs के साथ इंटीग्रेट करें | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API संदर्भ | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK नमूने | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local वेबसाइट | [foundrylocal.ai](https://foundrylocal.ai) |

---

## अगले कदम

SDK को OpenAI क्लाइंट लाइब्रेरी से कनेक्ट करने और अपनी पहली चैट पूर्णता एप्लिकेशन बनाने के लिए [भाग 3: OpenAI के साथ SDK का उपयोग](part3-sdk-and-apis.md) पर जाएं।