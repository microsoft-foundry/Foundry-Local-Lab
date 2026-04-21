![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग 2: Foundry Local SDK सखोल अभ्यास

> **लक्ष्य:** Foundry Local SDK कसे वापरायचे हे शिकणे ज्याद्वारे मॉडेल्स, सेवा, आणि कॅशिंग प्रोग्रामॅटिक पद्धतीने व्यवस्थापित केली जाऊ शकतात - आणि SDK हे CLI पेक्षा का शिफारस केलेले आहे ते समजून घेणे.

## आढावा

पहिल्या भागात तुम्ही **Foundry Local CLI** वापरून मॉडेल्स डाउनलोड करून इंटरॅक्टिव्हली चालवले. CLI शोधासाठी छान आहे, पण जेव्हा तुम्ही खऱ्या अॅप्लिकेशन्स तयार करता तेव्हा तुम्हाला **प्रोग्रामॅटिक नियंत्रण** आवश्यक आहे. Foundry Local SDK तुम्हाला ते देते - हे **कंट्रोल प्लेन** (सेवा सुरू करणे, मॉडेल्स शोधणे, डाउनलोड करणे, लोड करणे) व्यवस्थापित करते जेणेकरून तुमचा अॅप्लिकेशन कोड **डेटा प्लेन** (प्रॉम्प्ट पाठवणे, पूर्णता प्राप्त करणे) वर लक्ष केंद्रित करू शकेल.

हा लॅब तुम्हाला Python, JavaScript, आणि C# या सर्व SDK API चे संपूर्ण अवलोकन शिकवतो. शेवटी तुम्हाला प्रत्येक पद्धत कशी वापरायची ते समजेल.

## शिक्षण उद्दिष्टे

या लॅबच्या शेवटी तुम्ही करू शकाल:

- अॅप्लिकेशन विकासासाठी SDK CLI पेक्षा का प्राधान्य आहे ते समजावून सांगा
- Python, JavaScript, किंवा C# साठी Foundry Local SDK कसे इन्स्टॉल करायचे ते शिका
- `FoundryLocalManager` वापरून सेवा सुरू करा, मॉडेल्स व्यवस्थापित करा आणि कॅटलॉग क्वेरी करा
- प्रोग्रामॅटिक पद्धतीने मॉडेल्सची यादी करा, डाउनलोड करा, लोड करा, आणि अनलोड करा
- `FoundryModelInfo` वापरून मॉडेल मेटाडेटा तपासा
- कॅटलॉग, कॅश, आणि लोड केलेल्या मॉडेल्समधील फरक समजून घ्या
- कॉन्स्ट्रक्टर बूटस्ट्रॅप (Python) आणि `create()` + कॅटलॉग पॅटर्न (JavaScript) वापरा
- C# SDK पुनर्रचना आणि त्याचा वस्तुमुखी API समजून घ्या

---

## पूर्वअटी

| आवश्यकता | तपशील |
|-------------|---------|
| **Foundry Local CLI** | इन्स्टॉल केलेले आणि तुमच्या `PATH` मध्ये ([भाग 1](part1-getting-started.md)) |
| **भाषा रनटाइम** | **Python 3.9+** आणि/किंवा **Node.js 18+** आणि/किंवा **.NET 9.0+** |

---

## संकल्पना: SDK विरुद्ध CLI - SDK का वापरायचे?

| पैलू | CLI (`foundry` कमांड) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **वापर प्रकरण** | शोध, मॅन्युअल चाचणी | अॅप्लिकेशन एकत्रीकरण |
| **सेवा व्यवस्थापन** | मॅन्युअल: `foundry service start` | ऑटोमॅटिक: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **पोर्ट शोधणे** | CLI आउटपुट मधून वाचा | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **मॉडेल डाउनलोड** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **त्रुटी हाताळणी** | Exit कोड, stderr | Exceptions, typed errors |
| **स्वयंचलन** | शेल स्क्रिप्ट्स | स्थानिक भाषा एकत्रीकरण |
| **डिप्लॉयमेंट** | अंतिम वापरकर्त्याच्या मशीनवर CLI आवश्यक | C# SDK स्वतःत पूर्ण (CLI आवश्यक नाही) |

> **महत्त्वाचा मुद्दा:** SDK संपूर्ण जीवनचक्र हाताळते: सेवा सुरू करणे, कॅश तपासणे, हरवलेले मॉडेल डाउनलोड करणे, लोड करणे आणि एंडपॉइंट शोधणे, काही ओळींच्या कोडमध्ये. तुमच्या अॅप्लिकेशनला CLI आउटपुट पार्स करावे लागत नाही किंवा उपप्रक्रियेचे व्यवस्थापन करावे लागत नाही.

---

## लॅब व्यायाम

### व्यायाम 1: SDK इन्स्टॉल करा

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

इन्स्टॉलेशन तपासा:

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

इन्स्टॉलेशन तपासा:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

दोन NuGet पॅकेजेस आहेत:

| पॅकेज | प्लॅटफॉर्म | वर्णन |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | क्रॉस-प्लॅटफॉर्म | Windows, Linux, macOS वर चालते |
| `Microsoft.AI.Foundry.Local.WinML` | फक्त Windows | WinML हार्डवेअर एक्सेलेरेशन जोडते; प्लगइन एक्झिक्यूशन प्रोव्हायडर्स (उदा. Qualcomm NPU साठी QNN) डाउनलोड व इन्स्टॉल करते |

**Windows सेटअप:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` फाइल संपादित करा:

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

> **टीप:** Windows वर, WinML पॅकेज बेस SDK व्यतिरिक्त QNN एक्झिक्यूशन प्रोव्हायडर समाविष्ट आहे. Linux/macOS वर फक्त बेस SDK वापरला जातो. सशर्त TFM आणि पॅकेज संदर्भ प्रोजेक्टला पूर्ण क्रॉस-प्लॅटफॉर्म ठेवतात.

प्रोजेक्ट रूटमध्ये `nuget.config` तयार करा:

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

पॅकेजेस पुनर्संचयित करा:

```bash
dotnet restore
```

</details>

---

### व्यायाम 2: सेवा सुरू करा आणि कॅटलॉगची यादी करा

कोणताही अॅप्लिकेशन सर्वप्रथम Foundry Local सेवा सुरू करतो आणि कोणती मॉडेल्स उपलब्ध आहेत हे शोधतो.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# मॅनेजर तयार करा आणि सेवा सुरू करा
manager = FoundryLocalManager()
manager.start_service()

# कॅटलॉगमधील सर्व उपलब्ध मॉडेल्सची यादी करा
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - सेवा व्यवस्थापन पद्धती

| पद्धत | सही | वर्णन |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | सेवा चालू आहे का ते तपासा |
| `start_service()` | `() -> None` | Foundry Local सेवा सुरू करा |
| `service_uri` | `@property -> str` | सेवा बेस URI |
| `endpoint` | `@property -> str` | API एंडपॉइंट (सेवा URI + `/v1`) |
| `api_key` | `@property -> str` | API की (पर्यावरणातून वा डिफॉल्ट प्लेसहोल्डर) |

#### Python SDK - कॅटलॉग व्यवस्थापन पद्धती

| पद्धत | सही | वर्णन |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | कॅटलॉगमधील सर्व मॉडेल्सची यादी करा |
| `refresh_catalog()` | `() -> None` | सेवेमधून कॅटलॉग ताजे करा |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | विशिष्ट मॉडेलची माहिती मिळवा |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// व्यवस्थापक तयार करा आणि सेवा सुरू करा
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// कॅटलॉग ब्राउझ करा
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - मॅनेजर पद्धती

| पद्धत | सही | वर्णन |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK सिंगलटन इनिशियलाइझ करा |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | सिंगलटन मॅनेजर मिळवा |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local वेब सेवा सुरू करा |
| `manager.urls` | `string[]` | सेवा बेस URLs चे array |

#### JavaScript SDK - कॅटलॉग आणि मॉडेल पद्धती

| पद्धत | सही | वर्णन |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | मॉडेल कॅटलॉग एक्सेस करा |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | अलियासने मॉडेल ऑब्जेक्ट मिळवा |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ वस्तुमुखी आर्किटेक्चर वापरते ज्यात `Configuration`, `Catalog`, आणि `Model` ऑब्जेक्ट्स असतात:

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

#### C# SDK - मुख्य वर्ग

| वर्ग | उद्दिष्ट |
|-------|---------|
| `Configuration` | अॅप नाव, लॉग स्तर, कॅश डायरेक्टरी, वेब सर्व्हर URLs सेट करा |
| `FoundryLocalManager` | मुख्य प्रवेशबिंदू - `CreateAsync()` द्वारा तयार, `.Instance` वापरून प्रवेश करता येते |
| `Catalog` | कॅटलॉग ब्राउझ, शोधा, आणि मॉडेल मिळवा |
| `Model` | विशिष्ट मॉडेलचे प्रतिनिधित्व करते - डाउनलोड, लोड, क्लायंट मिळवा |

#### C# SDK - मॅनेजर आणि कॅटलॉग पद्धती

| पद्धत | वर्णन |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | मॅनेजर इनिशियलाइझ करा |
| `FoundryLocalManager.Instance` | सिंगलटन मॅनेजर एक्सेस करा |
| `manager.GetCatalogAsync()` | मॉडेल कॅटलॉग मिळवा |
| `catalog.ListModelsAsync()` | सर्व उपलब्ध मॉडेल्सची यादी करा |
| `catalog.GetModelAsync(alias: "alias")` | विशिष्ट मॉडेल अलियासने मिळवा |
| `catalog.GetCachedModelsAsync()` | डाउनलोड केलेल्या मॉडेल्सची यादी |
| `catalog.GetLoadedModelsAsync()` | सध्या लोड केलेल्या मॉडेल्सची यादी |

> **C# आर्किटेक्चर टीप:** C# SDK v0.8.0+ पुनर्रचनेने अॅप्लिकेशन पूर्णपणे **स्वतंत्र** बनवले आहे; अंतिम वापरकर्त्याच्या मशीनवर Foundry Local CLI आवश्यक नाही. SDK मॉडेल व्यवस्थापन आणि इन्फरन्स स्थानिक पद्धतीने करते.

</details>

---

### व्यायाम 3: मॉडेल डाउनलोड करा आणि लोड करा

SDK डाउनलोडिंग (डिस्कवर) व लोडिंग (मेमरीमध्ये) वेगळे करते. त्यामुळे तुम्ही सेटअप दरम्यान आधीपासून मॉडेल्स डाउनलोड करू शकता आणि आवश्यकतेनुसार लोड करू शकता.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# पर्याय A: हस्तचालित टप्प्याटप्प्याने
manager = FoundryLocalManager()
manager.start_service()

# प्रथम कॅश तपासा
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

# पर्याय B: एक ओळीचा बूटस्ट्रॅप (शिफारस केलेले)
# कन्स्ट्रक्टरला अलियास पाठवा - ते सेवा सुरू करते, डाउनलोड करते आणि आपोआप लोड करते
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - मॉडेल व्यवस्थापन पद्धती

| पद्धत | सही | वर्णन |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | मॉडेल स्थानिक कॅशमध्ये डाउनलोड करा |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | इन्फरन्स सर्व्हरमध्ये मॉडेल लोड करा |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | सर्व्हरवरून मॉडेल अनलोड करा |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | सध्या लोड केलेल्या सर्व मॉडेल्सची यादी करा |

#### Python - कॅश व्यवस्थापन पद्धती

| पद्धत | सही | वर्णन |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | कॅश डायरेक्टरीचा पथ मिळवा |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | डाउनलोड केलेल्या मॉडेल्सची यादी करा |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// टप्प्याटप्प्याने पध्दत
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

#### JavaScript - मॉडेल पद्धती

| पद्धत | सही | वर्णन |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | मॉडेल आधीच डाउनलोड केले आहे का |
| `model.download()` | `() => Promise<void>` | मॉडेल स्थानिक कॅशमध्ये डाउनलोड करा |
| `model.load()` | `() => Promise<void>` | इन्फरन्स सर्व्हरमध्ये लोड करा |
| `model.unload()` | `() => Promise<void>` | इन्फरन्स सर्व्हरमधून अनलोड करा |
| `model.id` | `string` | मॉडेलची अद्वितीय ओळख |

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

#### C# - मॉडेल पद्धती

| पद्धत | वर्णन |
|--------|-------------|
| `model.DownloadAsync(progress?)` | निवडलेल्या व्हेरिएंट डाउनलोड करा |
| `model.LoadAsync()` | मॉडेल मेमरीमध्ये लोड करा |
| `model.UnloadAsync()` | मॉडेल अनलोड करा |
| `model.SelectVariant(variant)` | विशिष्ट व्हेरिएंट निवडा (CPU/GPU/NPU) |
| `model.SelectedVariant` | सध्या निवडलेला व्हेरिएंट |
| `model.Variants` | या मॉडेलसाठी सर्व उपलब्ध व्हेरिएंट्स |
| `model.GetPathAsync()` | स्थानिक फाइल पथ मिळवा |
| `model.GetChatClientAsync()` | स्थानिक चॅट क्लायंट मिळवा (OpenAI SDK आवश्यक नाही) |
| `model.GetAudioClientAsync()` | ट्रान्सक्रिप्शनसाठी ऑडिओ क्लायंट मिळवा |

</details>

---

### व्यायाम 4: मॉडेल मेटाडेटा तपासा

`FoundryModelInfo` ऑब्जेक्टमध्ये प्रत्येक मॉडेलचे समृद्ध मेटाडेटा असते. हे फील्ड्स समजल्यास तुम्हाला योग्य मॉडेल निवडायला मदत होईल.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# विशिष्ट मॉडेलसंदर्भातील सविस्तर माहिती मिळवा
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

#### FoundryModelInfo फील्ड्स

| फील्ड | प्रकार | वर्णन |
|-------|------|-------------|
| `alias` | string | छोटा नाव (उदा. `phi-3.5-mini`) |
| `id` | string | अद्वितीय मॉडेल आयडी |
| `version` | string | मॉडेल आवृत्ती |
| `task` | string | `chat-completions` किंवा `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, किंवा NPU |
| `execution_provider` | string | रनटाइम बॅकएंड (CUDA, CPU, QNN, WebGPU, इ.) |
| `file_size_mb` | int | डिस्कवर आकार MB मध्ये |
| `supports_tool_calling` | bool | मॉडेलमध्ये फंक्शन/टूल कॉलिंग सपोर्ट आहे का |
| `publisher` | string | मॉडेल प्रकाशित करणारा |
| `license` | string | परवाना नाव |
| `uri` | string | मॉडेल URI |
| `prompt_template` | dict/null | प्रॉम्प्ट टेम्प्लेट, असल्यास |

---

### व्यायाम 5: मॉडेल जीवनचक्र व्यवस्थापित करा

पूर्ण जीवनचक्राचा सराव करा: यादी → डाउनलोड → लोड → वापर → अनलोड.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # जलद चाचणीसाठी लहान मॉडेल

manager = FoundryLocalManager()
manager.start_service()

# 1. कॅटलॉगमध्ये काय आहे ते तपासा
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. आधीच काय डाउनलोड झाले आहे ते तपासा
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. एक मॉडेल डाउनलोड करा
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. आत्ता ते कॅशेमध्ये आहे का ते तपासा
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. ते लोड करा
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. काय लोड झाले आहे ते तपासा
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. ते अनलोड करा
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

const alias = "qwen2.5-0.5b"; // वेगाने चाचणीसाठी लहान मॉडेल

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. कॅटलॉगमधून मॉडेल मिळवा
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. गरज असल्यास डाउनलोड करा
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. ते लोड करा
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. ते अनलोड करा
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### व्यायाम 6: द क्विक-स्टार्ट पॅटर्न्स

प्रत्येक भाषेमध्ये सेवा सुरू करण्यासाठी आणि एका कॉलमध्ये मॉडेल लोड करण्यासाठी एक शॉर्टकट दिलेला आहे. हे बहुतेक अनुप्रयोगांसाठी **शिफारस केलेले पॅटर्न्स** आहेत.

<details>
<summary><h3>🐍 Python - कंस्ट्रक्टर बूटस्ट्रॅप</h3></summary>

```python
from foundry_local import FoundryLocalManager

# कन्स्ट्रक्टरला एक उपनाम पास करा - तो सर्व काही हाताळतो:
# 1. सेवा चालू नसेल तर ती सुरू करतो
# 2. मॉडेल कॅशमध्ये नसेल तर डाउनलोड करतो
# 3. मॉडेल इन्फरन्स सर्व्हरमध्ये लोड करतो
manager = FoundryLocalManager("phi-3.5-mini")

# ताबडतोब वापरण्यास तयार
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` पॅरामीटर (डिफॉल्ट `True`) हा वर्तन नियंत्रित करतो. जर तुम्हाला मॅन्युअल नियंत्रिण हवे असल्यास `bootstrap=False` सेट करा:

```python
# मॅन्युअल मोड - काहीही आपोआप होत नाही
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + कॅटलॉग</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() सर्वकाही हाताळतो:
// 1. सेवा सुरू करतो
// 2. कॅटलॉगमधून मॉडेल मिळवतो
// 3. आवश्यक असल्यास डाउनलोड करतो आणि मॉडेल लोड करतो
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// तत्काळ वापरण्यास तयार
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + कॅटलॉग</h3></summary>

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

> **C# टीप:** C# SDK मध्ये `Configuration` वापरून अ‍ॅपचे नाव, लॉगिंग, कॅशे संचिका, अगदी विशिष्ट वेब सर्व्हर पोर्ट पिन करणे देखील नियंत्रित करता येते. हे तीन SDK मध्ये सर्वात जास्त सानुकूलनक्षम आहे.

</details>

---

### व्यायाम 7: नेटिव्ह ChatClient (OpenAI SDK ची गरज नाही)

JavaScript आणि C# SDK `createChatClient()` या सुविधा मेथडचा वापर करून नेटिव्ह चॅट क्लायंट परत करतात — OpenAI SDK स्वतंत्रपणे इन्स्टॉल किंवा कॉन्फिगर करण्याची गरज नाही.

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

// मॉडेलमधून थेट ChatClient तयार करा — OpenAI आयात आवश्यक नाही
const chatClient = model.createChatClient();

// completeChat एक OpenAI-सुसंगत प्रतिसाद ऑब्जेक्ट परत करते
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// स्ट्रीमिंग callback पद्धत वापरते
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` साधन वापरही समर्थन करतो — दुसऱ्या आर्ग्युमेंटमध्ये साधने पास करा:

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

> **कधी कोणता पॅटर्न वापरायचा:**
> - **`createChatClient()`** — जलद प्रोटोटायपिंग, कमी अवलंबित्व, सोपा कोड
> - **OpenAI SDK** — पॅरामीटर्सवर पूर्ण नियंत्रण (तापमान, top_p, स्टॉप टोकन, इत्यादी), उत्पादन अनुप्रयोगांसाठी उत्तम

---

### व्यायाम 8: मॉडेल वेगळेपणा आणि हार्डवेअर निवड

मॉडेल्सना वेगवेगळ्या हार्डवेअरवर ऑप्टिमाइझ केलेले अनेक **पर्याय** (variants) असू शकतात. SDK सर्वोत्कृष्ट पर्याय आपोआप निवडते, पण तुम्ही तपासणी करून मॅन्युअली निवडू शकता.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// सर्व उपलब्ध प्रकारांची यादी करा
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK आपला हार्डवेअरसाठी सर्वोत्तम प्रकार स्वयंचलितपणे निवडते
// अधिलेखन करण्यासाठी, selectVariant() वापरा:
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

Python मध्ये SDK हार्डवेअरवर आधारित सर्वोत्तम पर्याय आपोआप निवडते. नेमका काय निवडले गेले ते पहाण्यासाठी `get_model_info()` वापरा:

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

#### NPU पर्यायांसह मॉडेल्स

काही मॉडेल्सना NPU-ऑप्टिमाइझ्ड पर्याय असतात, जे न्यूरल प्रोसेसिंग युनिट्ससाठी (Qualcomm Snapdragon, Intel Core Ultra) आहेत:

| मॉडेल | NPU पर्याय उपलब्ध |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **टीप:** NPU समर्थित हार्डवेअरवर SDK उपलब्ध असल्यास आपोआप NPU पर्याय निवडते. तुमचा कोड बदलण्याची गरज नाही. Windows वर C# प्रोजेक्टसाठी, QNN एक्सिक्युशन प्रदाता सक्षम करण्यासाठी `Microsoft.AI.Foundry.Local.WinML` NuGet पॅकेज जोडा — QNN हा WinML द्वारे प्लगइन EP म्हणून दिला जातो.

---

### व्यायाम 9: मॉडेल अपग्रेड्स आणि कॅटलॉग रिफ्रेश

मॉडेल कॅटलॉग नियमितपणे अपडेट केला जातो. अपडेट्स तपासण्यासाठी आणि लागू करण्यासाठी खालील पद्धती वापरा.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# नवीनतम मॉडेल यादी मिळवण्यासाठी कॅटलॉग रीफ्रेश करा
manager.refresh_catalog()

# तपासा की कॅश केलेल्या मॉडेलची नवीन आवृत्ती उपलब्ध आहे का
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

// नवीनतम मॉडेल यादी मिळवण्यासाठी कॅटलॉग रिफ्रेश करा
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// रिफ्रेश केल्यानंतर सर्व उपलब्ध मॉडेल्सची यादी करा
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### व्यायाम 10: रीझनिंग मॉडेल्ससह काम

**phi-4-mini-reasoning** मॉडेलमध्ये चेन-ऑफ-थॉट (chain-of-thought) रीझनिंग असते. ते आपला अंतर्गत विचार `<think>...</think>` टॅगमध्ये गुंडाळून शेवटी उत्तर देतात. हे मल्टि-स्टेप लॉजिक, गणित, किंवा समस्या सोडवण्याच्या कामांसाठी उपयुक्त आहे.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-мини-विचारणा सुमारे 4.6 जीबी आहे
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# मॉडेल त्याच्या विचारांना <think>...</think> टॅगमध्ये गुंडाळते
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

// विचारांची साखळी-आधारित विचारसरणी पार्स करा
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **रीझनिंग मॉडेल्स कधी वापरायचे:**
> - गणित आणि लॉजिक समस्या
> - मल्टि-स्टेप नियोजन कार्ये
> - जटिल कोड जनरेशन
> - असे कार्य जिथे काम करण्याचा दाखला अचूकता वाढवतो
>
> **व्यवहार:** रीझनिंग मॉडेल्स जास्त टोकन्स उत्पादन करतात (`<think>` विभाग) आणि हळू असतात. सोप्या Q&A साठी, phi-3.5-mini सारखे स्टँडर्ड मॉडेल जलद असते.

---

### व्यायाम 11: अॅलियसेस आणि हार्डवेअर निवड समजून घेणे

जेव्हा तुम्ही पूर्ण मॉडेल आयडीऐवजी **अॅलियस** (उदा. `phi-3.5-mini`) पास करता, तेव्हा SDK तुमच्या हार्डवेअर साठी सर्वोत्तम पर्याय आपोआप निवडते:

| हार्डवेअर | निवडलेला एक्सिक्युशन प्रदाता |
|----------|----------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML प्लगइनद्वारे) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| कोणतीही डिव्हाइस (फॉलबॅक) | `CPUExecutionProvider` किंवा `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# हे उपनाम तुमच्या हार्डवेअर साठी सर्वोत्तम पर्याय निश्चित करते
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **टीप:** अॅप्लिकेशन कोडमध्ये नेहमी अॅलियसेस वापरा. जेव्हा तुम्ही युजरच्या मशीनवर तैनात करता, SDK रनटाईमवर हार्डवेअर-अनुकूल पर्याय निवडतो - NVIDIA साठी CUDA, Qualcomm साठी QNN, इतरत्र CPU.

---

### व्यायाम 12: C# कॉन्फिगरेशन पर्याय

C# SDK ची `Configuration` क्लास रनटाईम नियंत्रित करण्यासाठी सूक्ष्म पर्याय देते:

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

| प्रॉपर्टी | डीफॉल्ट | वर्णन |
|----------|---------|--------|
| `AppName` | (आवश्यक) | तुमच्या अ‍ॅप्लिकेशनचे नाव |
| `LogLevel` | `Information` | लॉगिंग तपशील |
| `Web.Urls` | (डायनामिक) | वेब सर्व्हरसाठी विशिष्ट पोर्ट पिन करा |
| `AppDataDir` | OS डिफॉल्ट | अ‍ॅप डेटा साठी बेस डिरेक्टरी |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | ज्या ठिकाणी मॉडेल साठवले जातात |
| `LogsDir` | `{AppDataDir}/logs` | जिथे लॉग्स लिहिले जातात |

---

### व्यायाम 13: ब्राऊझर वापर (फक्त JavaScript)

JavaScript SDK मध्ये ब्राऊझर-योग्य आवृत्ती समाविष्ट आहे. ब्राऊझरमध्ये, तुम्हाला CLI द्वारे सेवा मॅन्युअली सुरू करावी लागते आणि होस्ट URL निर्दिष्ट करावा लागतो:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// सर्व्हिस प्रथम मॅन्युअली सुरू करा:
//   foundry सेवा सुरू करा
// मग CLI आउटपुटमधील URL वापरा
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// कॅटलॉग ब्राउझ करा
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **ब्राऊझर मर्यादा:** ब्राऊझर आवृत्ती `startWebService()` ला समर्थन देत नाही. तुम्हाला SDK ब्राऊझरमध्ये वापरण्यापूर्वी Foundry Local सेवा आधीच चालू आहे याची खात्री करावी लागेल.

---

## संपूर्ण API संदर्भ

### Python

| श्रेणी | पद्धत | वर्णन |
|----------|--------|----------|
| **इनीट** | `FoundryLocalManager(alias?, bootstrap=True)` | मॅनेजर तयार करा; पर्यायीपणे मॉडेलसह बूटस्ट्रॅप करा |
| **सेवा** | `is_service_running()` | सेवा चालू आहे का ते तपासा |
| **सेवा** | `start_service()` | सेवा सुरू करा |
| **सेवा** | `endpoint` | API endpoint URL |
| **सेवा** | `api_key` | API की |
| **कॅटलॉग** | `list_catalog_models()` | सर्व उपलब्ध मॉडेल्सची यादी |
| **कॅटलॉग** | `refresh_catalog()` | कॅटलॉग रीफ्रेश करा |
| **कॅटलॉग** | `get_model_info(alias_or_model_id)` | मॉडेल मेटाडेटा मिळवा |
| **कॅशे** | `get_cache_location()` | कॅशे डायरेक्टरीचा पथ |
| **कॅशे** | `list_cached_models()` | डाउनलोड केलेली मॉडेल्स यादी |
| **मॉडेल** | `download_model(alias_or_model_id)` | मॉडेल डाउनलोड करा |
| **मॉडेल** | `load_model(alias_or_model_id, ttl=600)` | मॉडेल लोड करा |
| **मॉडेल** | `unload_model(alias_or_model_id)` | मॉडेल अनलोड करा |
| **मॉडेल** | `list_loaded_models()` | लोड केलेल्या मॉडेल्सची यादी |
| **मॉडेल** | `is_model_upgradeable(alias_or_model_id)` | नवीन आवृत्ती उपलब्ध आहे का तपासा |
| **मॉडेल** | `upgrade_model(alias_or_model_id)` | मॉडेल नवीनतम आवृत्तीत सुधारित करा |
| **सेवा** | `httpx_client` | थेट API कॉलसाठी प्री-कॉन्फिगर केलेला HTTPX क्लायंट |

### JavaScript

| श्रेणी | पद्धत | वर्णन |
|----------|--------|----------|
| **इनीट** | `FoundryLocalManager.create(options)` | SDK सिंगल्टन सुरू करा |
| **इनीट** | `FoundryLocalManager.instance` | सिंगल्टन मॅनेजर ऍक्सेस करा |
| **सेवा** | `manager.startWebService()` | वेब सेवा सुरू करा |
| **सेवा** | `manager.urls` | सेवेसाठी बेस URL ची यादी |
| **कॅटलॉग** | `manager.catalog` | मॉडेल कॅटलॉग ऍक्सेस करा |
| **कॅटलॉग** | `catalog.getModel(alias)` | अॅलियसने मॉडेल ऑब्जेक्ट मिळवा (प्रॉमिस देते) |
| **मॉडेल** | `model.isCached` | मॉडेल डाउनलोड केले आहे का? |
| **मॉडेल** | `model.download()` | मॉडेल डाउनलोड करा |
| **मॉडेल** | `model.load()` | मॉडेल लोड करा |
| **मॉडेल** | `model.unload()` | मॉडेल अनलोड करा |
| **मॉडेल** | `model.id` | मॉडेलचा अद्वितीय आयडी |
| **मॉडेल** | `model.alias` | मॉडेलचा अॅलियस |
| **मॉडेल** | `model.createChatClient()` | नेटिव्ह चॅट क्लायंट मिळवा (OpenAI SDK आवश्यक नाही) |
| **मॉडेल** | `model.createAudioClient()` | ट्रान्स्क्रिप्शनसाठी ऑडिओ क्लायंट मिळवा |
| **मॉडेल** | `model.removeFromCache()` | स्थानिक कॅशेमधून मॉडेल काढा |
| **मॉडेल** | `model.selectVariant(variant)` | विशिष्ट हार्डवेअर पर्याय निवडा |
| **मॉडेल** | `model.variants` | या मॉडेलसाठी उपलब्ध पर्यायांची यादी |
| **मॉडेल** | `model.isLoaded()` | मॉडेल सध्या लोड आहे का तपासा |
| **कॅटलॉग** | `catalog.getModels()` | सर्व उपलब्ध मॉडेल्सची यादी |
| **कॅटलॉग** | `catalog.getCachedModels()` | डाउनलोड केलेल्या मॉडेल्सची यादी |
| **कॅटलॉग** | `catalog.getLoadedModels()` | सध्या लोड केलेल्या मॉडेल्सची यादी |
| **कॅटलॉग** | `catalog.updateModels()` | सेवेवरून कॅटलॉग रीफ्रेश करा |
| **सेवा** | `manager.stopWebService()` | Foundry Local वेब सेवा थांबवा |

### C# (v0.8.0+)

| श्रेणी | पद्धत | वर्णन |
|----------|--------|----------|
| **इनीट** | `FoundryLocalManager.CreateAsync(config, logger)` | मॅनेजर इनीशियलाइझ करा |
| **इनीट** | `FoundryLocalManager.Instance` | सिंगल्टन ऍक्सेस करा |
| **कॅटलॉग** | `manager.GetCatalogAsync()` | कॅटलॉग मिळवा |
| **कॅटलॉग** | `catalog.ListModelsAsync()` | सर्व मॉडेल्सची यादी |
| **कॅटलॉग** | `catalog.GetModelAsync(alias)` | विशिष्ट मॉडेल मिळवा |
| **कॅटलॉग** | `catalog.GetCachedModelsAsync()` | कॅशे केलेल्या मॉडेल्सची यादी |
| **कॅटलॉग** | `catalog.GetLoadedModelsAsync()` | लोड केलेल्या मॉडेल्सची यादी |
| **मॉडेल** | `model.DownloadAsync(progress?)` | मॉडेल डाउनलोड करा |
| **मॉडेल** | `model.LoadAsync()` | मॉडेल लोड करा |
| **मॉडेल** | `model.UnloadAsync()` | मॉडेल अनलोड करा |
| **मॉडेल** | `model.SelectVariant(variant)` | हार्डवेअर पर्याय निवडा |
| **मॉडेल** | `model.GetChatClientAsync()` | नेटिव्ह चॅट क्लायंट मिळवा |
| **मॉडेल** | `model.GetAudioClientAsync()` | ऑडिओ ट्रान्स्क्रिप्शन क्लायंट मिळवा |
| **मॉडेल** | `model.GetPathAsync()` | स्थानिक फाईल पथ मिळवा |
| **कॅटलॉग** | `catalog.GetModelVariantAsync(alias, variant)` | विशिष्ट हार्डवेअर पर्याय मिळवा |
| **कॅटलॉग** | `catalog.UpdateModelsAsync()` | कॅटलॉग रीफ्रेश करा |
| **सर्व्हर** | `manager.StartWebServerAsync()` | REST वेब सर्व्हर सुरु करा |
| **सर्व्हर** | `manager.StopWebServerAsync()` | REST वेब सर्व्हर थांबवा |
| **कॉन्फिग** | `config.ModelCacheDir` | कॅशे डायरेक्टरी |

---

## मुख्य गोष्टी

| संकल्पना | तुम्ही काय शिकलात |
|---------|-----------------|
| **SDK vs CLI** | SDK प्रोग्रामॅटिक नियंत्रण देते - अनुप्रयोगांसाठी आवश्यक |
| **कंट्रोल प्लेन** | SDK सेवा, मॉडेल्स आणि कॅशिंग व्यवस्थापित करते |
| **डायनॅमिक पोर्ट्स** | नेहमी `manager.endpoint` (Python) किंवा `manager.urls[0]` (JS/C#) वापरा - पोर्ट हार्डकोड करू नका |
| **अॅलियसेस** | हार्डवेअर-सर्वोत्तम मॉडेल निवडीसाठी अॅलियसेस वापरा |
| **त्वरित-प्रारंभ** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# पुनर्निर्मिती** | v0.8.0+ स्वयंपूर्ण आहे - एंड-यूजर मशीनवर CLI ची आवश्यकता नाही |
| **मॉडेल जीवनचक्र** | कॅटलॉग → डाउनलोड → लोड → वापर → अनलोड |
| **FoundryModelInfo** | समृद्ध मेटाडेटा: कार्य, उपकरण, आकार, परवाना, टूल कॉलिंग समर्थन |
| **ChatClient** | OpenAI-मुक्त वापरासाठी `createChatClient()` (JS) / `GetChatClientAsync()` (C#) |
| **भिन्न प्रकार** | मॉडेल्सकडे हार्डवेअर-विशिष्ट भिन्न प्रकार असतात (CPU, GPU, NPU); आपोआप निवडले जातात |
| **अपग्रेड्स** | Python: वर्तमान मॉडेल्स राखण्यासाठी `is_model_upgradeable()` + `upgrade_model()` |
| **कॅटलॉग ताजेतवाने करणे** | नवीन मॉडेल्स शोधण्यासाठी `refresh_catalog()` (Python) / `updateModels()` (JS) |

---

## संसाधने

| संसाधन | दुवा |
|----------|------|
| SDK संदर्भ (सर्व भाषा) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Inference SDKs सह एकत्रीकरण | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API संदर्भ | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK नमुने | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local संकेतस्थळ | [foundrylocal.ai](https://foundrylocal.ai) |

---

## पुढील पावले

SDK ला OpenAI क्लायंट लायब्ररीशी कनेक्ट करण्यासाठी आणि आपले पहिले चॅट पूर्णता अॅप्लिकेशन तयार करण्यासाठी [भाग 3: SDK सह OpenAI वापरणे](part3-sdk-and-apis.md) वर पुढे जा.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**गारंटी नाकारणी**:
हा दस्तऐवज AI भाषांतर सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) वापरून भाषांतरित केला आहे. आम्ही अचूकतेसाठी प्रयत्न करतो, परंतु कृपया लक्षात घ्या की स्वयंचलित भाषांतरांमध्ये त्रुटी किंवा अचूकतेची कमतरता असू शकते. मूळ दस्तऐवज त्याच्या स्थानिक भाषेत अधिकृत स्रोत मानला पाहिजे. अत्यावश्यक माहितीसाठी व्यावसायिक मानवी भाषांतर शिफारसीय आहे. या भाषांतराच्या वापरामुळे उद्भवलेल्या कोणत्याही गैरसमज किंवा चुकीच्या समजुतीसाठी आम्ही जबाबदार नाही.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->