![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# భాగం 2: Foundry Local SDK లో లోతైన అవగాహన

> **గోల్ఛెయ్యడం:** ప్రోగ్రామాటిక్‌గా మోడల్లు, సర్వీసులు మరియు కాష్చె నిర్వహించడానికి Foundry Local SDK లో పూర్తి నైపుణ్యం పొందడం - మరియు CLI కంటే SDK ని ఎందుకు సిఫారసు చేస్తారో అర్థం చేసుకోవడం.

## అవలోకనం

భాగం 1 లో మీరు **Foundry Local CLI** ఉపయోగించి మోడల్లు డౌన్‌లోడ్ చేసి ఇంటరాక్టివ్‌గా నడిపించాల్సింది. CLI అన్వేషణ కోసం చక్కగా ఉండగా, అసలు అప్లికేషన్లు నిర్మించేటప్పుడు మీరు **ప్రోగ్రామాటిక్ నియంత్రణ** అవసరం అవుతుంది. Foundry Local SDK మీకు ఇదే ఇస్తుంది - ఇది **కంట్రోల్ ప్లేన్** (సర్వీసు ప్రారంభం, మోడల్లు కనుగొనడం, డౌన్‌లోడ్, లోడ్)ను నిర్వహిస్తుంది అందుకని మీ అప్లికేషన్ కోడ్ **డేటా ప్లేన్** (ప్రాంప్ట్స్ పంపడం, పూర్తి ఫలితాలు అందుకోవడం)పైన కేంద్రీకృతం అవుతుంది.

ఈ ప్రయోగశాల Python, JavaScript, మరియు C# పరిధిలో పూర్తి SDK API ని నేర్పిస్తుంది. చివరికి మీరు ప్రతి మేతడ్ ను అర్థం చేసుకొని ఎప్పుడు ఎలా వాడాలో తెలుసుకుంటారు.

## అధ్యయన లక్ష్యాలు

ఈ ప్రయోగశాల ముగింపులో మీరు:

- అప్లికేషన్ అభివృద్ధికి CLI పై SDK ఎందుకు ప్రాధాన్యం ఇవ్వబడిందో వివరిస్తారు
- Python, JavaScript, లేదా C# కోసం Foundry Local SDK ఎలా ఇన్‌స్టాల్ చేయాలో తెలుసుకుంటారు
- `FoundryLocalManager` తో సర్వీసు ప్రారంభించడం, మోడల్లు నిర్వహించడం, కాటలాగ్ ను క్వెరీ చేయడం
- ప్రోగ్రామాటిక్‌గా మోడల్లు జాబితా చేయడం, డౌన్‌లోడ్ చేయడం, లోడ్ చేయడం, అన్‌లోడ్ చేయడం
- `FoundryModelInfo` ఉపయోగించి మోడల్ మెటాడేటాను పరిశీలించడం
- కాటలాగ్, కాష్చె, మరియు లోడ్ అయిన మోడల్స్ మధ్య తేడాను అర్థం చేసుకోవడం
- కన్స్ట్రక్టర్ బూట్‌స్ట్రాప్ (Python) మరియు `create()` + కాటలాగ్ నమూనాను (JavaScript) వినియోగించడం
- C# SDK మళ్లింపు రూపకల్పన మరియు దాని ఆబ్జెక్ట్-ఒరియెంటెడ్ API ని అర్థం చేసుకోవడం

---

## ముందస్తు తగింపు

| అవసరం | వివరాలు |
|-------------|---------|
| **Foundry Local CLI** | ఇన్‌స్టాల్ చేసి PATH లో ఉంచడం ([భాగం 1](part1-getting-started.md)) |
| **భాష రంటైమ్** | **Python 3.9+** మరియు/లేదా **Node.js 18+** మరియు/లేదా **.NET 9.0+** |

---

## కల్పన: SDK vs CLI - SDK ని ఎందుకు ఉపయోగిస్తారు?

| అంశం | CLI (`foundry` కమాండ్) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **వినియోగ సందర్భం** | అన్వేషణ, మాన్యువల్ పరీక్ష | అప్లికేషన్ సమన్వయం |
| **సర్వీస్ నిర్వహణ** | మాన్యువల్: `foundry service start` | ఆటోమేటిక్: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **పోర్ట్ కనుగొనటం** | CLI అవుట్పుట్ నుండి చదవడం | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **మోడల్ డౌన్‌లోడ్** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **లోప నిర్వహణ** | ఎగ్జిట్ కోడ్స్, stderr | ఎక్సెప్షన్స్, టైప్ చేసిన లోపాలు |
| **ఆటోమేషన్** | షెల్ స్క్రిప్ట్స్ | స్థానిక భాష సమన్వయం |
| **డిప్లాయ్‌మెంట్** | ఎండ్-యూజర్ మషిన్ లో CLI అవసరం | C# SDK స్వీయ-సంపూర్ణం (CLI అవసరం లేదు) |

> **ముఖ్య అవగాహన:** SDK పూర్తి జీవితచక్రం నిర్వహణను చేస్తుంది: సర్వీసు ప్రారంభం, కాష్చె తనిఖీ, మోడల్లు డౌన్‌లోడ్ చేయడం, లోడ్ చేయడం, మరియు ఎండ్‌పాయింట్ కనుగొనటం - కొన్ని కోడ్ లైన్స్ లో. మీ అప్లికేషన్ CLI అవుట్పుట్ పార్స్ చేయాల్సిన లేదా సబ్రోసెస్‌లు నిర్వహించాల్సిన అవసరం లేదు.

---

## ప్రయోగాలు

### ప్రయోగం 1: SDK ఇన్‌స్టాల్ చేయండి

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

ఇన్‌స్టలేషన్ నిర్ధారించుకోండి:

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

ఇన్‌స్టలేషన్ నిర్ధారించుకోండి:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

రెండు NuGet ప్యాకేజీలున్నాయి:

| ప్యాకేజీ | ప్లాట్ఫారమ్ | వివరణ |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | క్రాస్-ప్లాట్ఫారమ్ | విండోస్, లినక్స్, macOS పై పనిచేస్తుంది |
| `Microsoft.AI.Foundry.Local.WinML` | విండోస్ మాత్రమే | WinML హార్డ్‌వేర్ యాక్సిలరేషన్ ని జోడిస్తుంది; ప్లగిన్ ఎగ్జిక్యూషన్ ప్రొవైడర్లను డౌన్‌లోడ్ చేసి ఇన్స్టాల్ చేస్తుంది (ఉదా: Qualcomm NPU కోసం QNN) |

**విండోస్ సెటప్:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` ఫైల్ ను ఎడిట్ చేయండి:

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

> **గమనిక:** విండోస్‌లో, WinML ప్యాకేజీ బేస్ SDK ని మరియు QNN ఎగ్జిక్యూషన్ ప్రొవైడర్ ని కలిగి ఉన్న సూపర్‌సెట్. లినక్స్/macOS పై బేస్ SDK ఉపయోగించబడుతుంది. కండిషనల్ TFM మరియు ప్యాకేజీ రిఫరెన్సులు ప్రాజెక్ట్‌ను పూర్తిగా క్రాస్-प్లాట్ఫారమ్‌గా ఉంచతాయి.

ప్రాజెక్ట్ రూట్ లో `nuget.config` సృష్టించండి:

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

ప్యాకేజీలు రీస్టోర్ చేయండి:

```bash
dotnet restore
```

</details>

---

### ప్రయోగం 2: సర్వీస్‌ను ప్రారంభించి కాటలాగ్ జాబితా చేయండి

ఏ అప్లికేషన్ మొదటగా చేసే పని Foundry Local సర్వీస్ ప్రారంభించి అందుబాటులో ఉన్న మోడల్లు కనుగొనటం.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# మేనేజర్‌ను సృష్టించి సేవను ప్రారంభించండి
manager = FoundryLocalManager()
manager.start_service()

# క్యాటలాగ్‌లో అందుబాటులో ఉన్న అన్ని మోడళ్లను జాబితా చేయండి
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - సర్వీస్ నిర్వహణ మేతడ్స్

| మేతడ్ | సిగ్నేచర్ | వివరణ |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | సర్వీస్ నడుస్తుందా కాదా చెక్ చేయు |
| `start_service()` | `() -> None` | Foundry Local సర్వీస్ ప్రారంభించు |
| `service_uri` | `@property -> str` | బేస్ సర్వీస్ URI |
| `endpoint` | `@property -> str` | API ఎండ్‌పాయింట్ (సర్వీస్ URI + `/v1`) |
| `api_key` | `@property -> str` | API కీ (పరిసరాల నుండి లేదా డిఫాల్ట్ ప్లేస్‌హోల్డర్) |

#### Python SDK - కాటలాగ్ నిర్వహణ మేతడ్స్

| మేతడ్ | సిగ్నేచర్ | వివరణ |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | కాటలాగ్ లోని అన్ని మోడల్స్ జాబితా చేయండి |
| `refresh_catalog()` | `() -> None` | సర్వీస్ నుండి కాటలాగ్ రిఫ్రెష్ చేయండి |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=false) -> FoundryModelInfo \| None` | ఒక నిర్దిష్ట మోడల్ సమాచారం అందుకోండి |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// మేనేజర్‌ని సృష్టించి సేవ ప్రారంభించండి
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// క్యాటలాగ్‌ను బ్రౌజ్ చేయండి
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - మేనేజర్ మేతడ్స్

| మేతడ్ | సిగ్నేచర్ | వివరణ |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK సింగిల్టన్ ని ప్రథమీకరించు |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | సింగిల్టన్ మేనేజర్ యాక్సెస్ చేయండి |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local వెబ్ సర్వీస్ ప్రారంభించండి |
| `manager.urls` | `string[]` | సర్వీస్ కోసం బేస్ URLs సంగ్రహం |

#### JavaScript SDK - కాటలాగ్ మరియు మోడల్ మేతడ్స్

| మేతడ్ | సిగ్నేచర్ | వివరణ |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | మోడల్ కాటలాగ్ యాక్సెస్ చేయండి |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | అలియాస్ ద్వారా మోడల్ ఆబ్జెక్ట్ పొందండి |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ ఆబ్జెక్ట్-ఒరియెంటెడ్ ఆర్కిటెక్చర్ ఉపయోగిస్తుంది `Configuration`, `Catalog`, మరియు `Model` ఆబ్జెక్టులతో:

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

#### C# SDK - కీలక క్లాసులు

| క్లాస్ | ఉద్దేశ్యం |
|-------|---------|
| `Configuration` | అప్లికేషన్ పేరు, లాగ్ స్థాయి, కాష్చే డైరెక్టరీ, వెబ్ సర్వర్ URLs సెట్ చేయడం |
| `FoundryLocalManager` | ప్రధాన ప్రవేశపు పాయింట్ - `CreateAsync()`తో సృష్టించబడి `.Instance` ద్వారా యాక్సెస్ చేయబడుతుంది |
| `Catalog` | కాటలాగ్ బ్రౌజ్ చేయుట, శోధించుట, మోడల్స్ పొందుట |
| `Model` | ఒక నిర్దిష్ట మోడల్ ಪ್ರತినిధ్యం - డౌన్‌లోడ్, లోడ్, క్లయింట్లు పొందుట |

#### C# SDK - మేనేజర్ మరియు కాటలాగ్ మేతడ్స్

| మేతడ్ | వివరణ |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | మేనేజర్ ప్రారంభించండి |
| `FoundryLocalManager.Instance` | సింగిల్టన్ మేనేజర్ యాక్సెస్ చేయండి |
| `manager.GetCatalogAsync()` | మోడల్ కాటలాగ్ పొందండి |
| `catalog.ListModelsAsync()` | అందుబాటులో ఉన్న అన్ని మోడల్స్ జాబితా చేయండి |
| `catalog.GetModelAsync(alias: "alias")` | ఒక నిర్దిష్ట అలియాస్ వున్న మోడల్ పొందండి |
| `catalog.GetCachedModelsAsync()` | డౌన్‌లోడ్ అయిన మోడల్స్ జాబితా చేయండి |
| `catalog.GetLoadedModelsAsync()` | ప్రస్తుతంగా లోడ్ చేసిన మోడల్స్ జాబితా చేయండి |

> **C# ఆర్కిటెక్చర్ గమనిక:** C# SDK v0.8.0+ రూపకల్పన అప్లికేషన్‌ను **స్వీయ-సంపూర్ణం**గా మార్చింది; ఇది ఎండ్-యూజర్’s మషీన్ లో Foundry Local CLI అవసరం లేకుండా ఉంటుంది. SDK స్వంతంగా మోడల్ నిర్వహణ మరియు ఇన్‌ఫరెన్స్ నిర్వహిస్తది.

</details>

---

### ప్రయోగం 3: మోడల్ డౌన్‌లోడ్ చేసి లోడ్ చేయండి

SDK డౌన్‌లోడ్ (డిస్క్‌కు) మరియు లోడ్ (మెమరీకి)ని వేరుగా నిర్వహిస్తుంది. ఇది మీరు సెటప్ సమయంలో ప్రీ-డౌన్‌లోడ్ చేసి, అవసరానికి అనుగుణంగా లోడ్ చేసుకునేందుకు అనుమతిస్తుంది.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# ఎంపిక A: మాన్యువల్ దశలవారీగా
manager = FoundryLocalManager()
manager.start_service()

# ముందుగా క్యాషేను తనిఖీ చేయండి
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

# ఎంపిక B: ఒక లైనర్ బూట్‌స్ట్రాప్పింగ్ (సిఫారసు చేయబడింది)
# కన్స్ట్రక్టర్‌కు అలియాస్‌ను పాస్ చేయండి - ఇది సర్వీసును ప్రారంభించి, డౌన్‌లోడ్ చేసి, స్వయంచాలకంగా లోడ్ చేస్తుంది
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - మోడల్ నిర్వహణ మేతడ్స్

| మేతడ్ | సిగ్నేచర్ | వివరణ |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | మోడల్ ను లోకల్ కాష్చెలో డౌన్‌లోడ్ చేయండి |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | మోడల్ ను ఇన్ఫరెన్స్ సర్వర్ లో లోడ్ చేయండి |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | సర్వర్ నుండి మోడల్ ను అన్‌లోడ్ చేయండి |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | ప్రస్తుతంగా లోడ్ అయిన మోడల్స్ జాబితా చేయండి |

#### Python - కాష్చె నిర్వహణ మేతడ్స్

| మేతడ్ | సిగ్నేచర్ | వివరణ |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | కాష్చె డైరెక్టరీ పొజిషన్ పొందండి |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | డౌన్‌లోడ్ అయిన మోడల్స్ జాబితా చేయండి |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// దశలవారీగా విధానం
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

#### JavaScript - మోడల్ మేతడ్స్

| మేతడ్ | సిగ్నేచర్ | వివరణ |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | మోడల్ ఇప్పటికే డౌన్‌లోడ్ అయిందా అని |
| `model.download()` | `() => Promise<void>` | మోడల్ ను లోకల్ కాష్చెలో డౌన్‌లోడ్ చేయండి |
| `model.load()` | `() => Promise<void>` | ఇన్ఫరెన్స్ సర్వర్ లో లోడ్ చేయండి |
| `model.unload()` | `() => Promise<void>` | ఇన్ఫరెన్స్ సర్వర్ నుండి అన్‌లోడ్ చేయండి |
| `model.id` | `string` | మోడల్ వియుక్త గుర్తింపు సంఖ్య |

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

#### C# - మోడల్ మేతడ్స్

| మేతడ్ | వివరణ |
|--------|-------------|
| `model.DownloadAsync(progress?)` | ఎంచుకున్న వేరియంట్ను డౌన్‌లోడ్ చేయండి |
| `model.LoadAsync()` | మోడల్ ను మెమరీలో లోడ్ చేయండి |
| `model.UnloadAsync()` | మోడల్ ను అన్‌లోడ్ చేయండి |
| `model.SelectVariant(variant)` | ఒక ప్రత్యేక వేరియంట్ (CPU/GPU/NPU) ఎంచుకోండి |
| `model.SelectedVariant` | ప్రస్తుతం ఎంచుకున్న వేరియంట్ |
| `model.Variants` | ఈ మోడల్ కి అందుబాటులో ఉన్న అన్ని వేరియంట్లు |
| `model.GetPathAsync()` | లోకల్ ఫైల్ మార్గం పొందండి |
| `model.GetChatClientAsync()` | స్థానిక చాట్ క్లయింట్ పొందండి (OpenAI SDK అవసరం లేదు) |
| `model.GetAudioClientAsync()` | ట్రాన్స్క్రిప్షన్ కోసం ఆడియో క్లయింట్ పొందండి |

</details>

---

### ప్రయోగం 4: మోడల్ మెటాడేటాను పరిశీలించండి

`FoundryModelInfo` ఆబ్జెక్టులో ప్రతి మోడల్ గురించి సంపన్నమైన మెటాడేటా ఉంటుంది. ఈ ఫీల్డ్స్ అర్థం చేసుకోవడం మీ అప్లికేషన్ కు సరైన మోడల్ ఎంచుకోవటానికి సహాయపడుతుంది.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ఒక నిర్దిష్ట మోడల్ గురించి విపులమైన సమాచారం పొందండి
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

#### FoundryModelInfo ఫీల్డ్స్

| ఫీల్డ్ | రకం | వివరాలు |
|-------|------|-------------|
| `alias` | string | చిన్న పేరు (ఉదా: `phi-3.5-mini`) |
| `id` | string | ప్రత్యేక మోడల్ గుర్తింపు సంఖ్య |
| `version` | string | మోడల్ సంస్కరణ |
| `task` | string | `chat-completions` లేదా `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, లేదా NPU |
| `execution_provider` | string | రన్‌టైమ్ బ్యాక్‌ఎండ్ (CUDA, CPU, QNN, WebGPU, మొదలైనవి) |
| `file_size_mb` | int | డిస్క్ పై MBలలో పరిమాణం |
| `supports_tool_calling` | bool | మోడల్ ఫంక్షన్/టూల్ కాలింగ్ ని మద్దతు ఇస్తుందా |
| `publisher` | string | మోడల్ ప్రచురించినవారు |
| `license` | string | లైసెన్స్ పేరు |
| `uri` | string | మోడల్ URI |
| `prompt_template` | dict/null | ప్రాంప్ట్ టెంప్లేట్, ఉంటే |

---

### ప్రయోగం 5: మోడల్ జీవితచక్రం నిర్వహణ

పూర్తి జీవితచక్రాన్ని ప్రాక్టీస్ చేయండి: జాబితా చేయండి → డౌన్‌లోడ్ చేయండి → లోడ్ చేయండి → వాడండి → అన్‌లోడ్ చేయండి.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # త్వరిత పరీక్ష కోసం చిన్న మోడల్

manager = FoundryLocalManager()
manager.start_service()

# 1. క్యాటలాగ్‌లో ఏం ఉన్నదో తనిఖీ చేయండి
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. ఇప్పటికే డౌన్‌లోడ్ చేసినది ఏమిటి చూడండి
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. ఒక మోడల్‌ను డౌన్లోడ్ చేయండి
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. ఇప్పుడు అది క్యాషేని లో ఉందో ధృవీకరించండి
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. దానిని లోడ్ చేయండి
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. ఏం లోడ్ అయిందో తనిఖీ చేయండి
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. దానిని అన్‌లోడ్ చేయండి
print(f"\nUnloading {alias}...")
manager.unload_model(alias)
loaded = manager.list_loaded_models()
print(f"Loaded models after unload: {len(loaded)}")
```

</details>
<summary><h3>📘 జావాస్క్రిప్ట్</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "qwen2.5-0.5b"; // వేగవంతమైన పరీక్ష కోసం చిన్న మోడల్

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. క్యాటలాగ్ నుండి మోడల్ పొందండి
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. అవసరమైతే 다운로드 చేయండి
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. దాన్ని లోడ్ చేయండి
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. దాన్ని అన్‌లోడ్ చేయండి
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### వ్యాయామం 6: క్విక్-స్టార్ట్ ప్యాటర్న్లు

ప్రతి భాష ఒకే కాల్‌లో సర్వీస్‌ను ప్రారంభించి, మోడల్‌ను లోడ్ చేయడానికి షార్ట్‌కట్ అందిస్తుంది. ఇవి ఎక్కువ మొత్తంలో అప్లికేషన్ల కోసం **శిఫార్సు చేయబడిన నమూనాలు**.

<details>
<summary><h3>🐍 పైథాన్ - కన్‌స్ట్రక్టర్ బూట్‌స్ట్రాప్</h3></summary>

```python
from foundry_local import FoundryLocalManager

# కన్స్ట్రక్టర్ కి ఒక అలియాస్ ఇవ్వండి - ఇది అన్నింటినీ నిర్వహిస్తుంది:
# 1. సర్వీస్ పనిచేస్తున్నట్లయితే ప్రారంభిస్తుంది
# 2. మోడల్ కాష్ లో లేకుంటే డౌన్ లోడ్ చేస్తుంది
# 3. మోడల్ ని ఇన్ఫరెన్స్ సర్వర్ కి లోడ్ చేస్తుంది
manager = FoundryLocalManager("phi-3.5-mini")

# అప్పుడు వెంటనే ఉపయోగించడానికి సిద్ధంగా ఉంటుంది
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` పరామితి (డిఫాల్ట్ `True`) ఈ ప్రవర్తనను నియంత్రిస్తుంది. మాన్యువల్ నియంత్రణ కావాలంటే `bootstrap=False` సెట్ చేయండి:

```python
# మాన్యువల్ మోడ్ - ఏమి ఆటోమాటిక్‌గా జరగదు
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 జావాస్క్రిప్ట్ - `create()` + క్యాటలాగ్</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() అన్నీ నిర్వహిస్తాయి:
// 1. సర్వీస్ ప్రారంభిస్తుంది
// 2. క్యాటలాగ్ నుండి మోడల్ పొందుతుంది
// 3. అవసరమైతే డౌన్‌లోడ్ చేసి మోడల్ لوడ్ చేస్తుంది
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// తక్షణంగా ఉపయోగించడానికి సిద్ధంగా ఉంది
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + క్యాటలాగ్</h3></summary>

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

> **C# గమనిక:** C# SDK యాప్ పేరు, లాగింగ్, కాషే డైరెక్టరీలు, మరియు స్పెసిఫిక్ వెబ్ సర్వర్ పోర్ట్‌ను పిన్ చేయడానికి `Configuration` ఉపయోగిస్తుంది. ఇది మూడు SDKలలో అత్యంత కస్టమైజ్ చేయదగినది.

</details>

---

### వ్యాయామం 7: నేటివ్ ChatClient (అంతర్గతంగా OpenAI SDK అవసరం లేదు)

జావాస్క్రిప్ట్ మరియు C# SDKలు ఒక `createChatClient()` సౌకర్యవంతమైన విధానాన్ని అందిస్తాయి, ఇది ఓపెన్ ఏఐ SDK ని వేరుగా ఇన్స్టాల్ లేదా కాన్ఫిగర్ చేయకుండా నేటివ్ చాట్ క్లయింట్‌ని ఇచ్చేస్తుంది.

<details>
<summary><h3>📘 జావాస్క్రిప్ట్ - <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// మోడల్ నుండి తక్షణమే ChatClient సృష్టించండి — OpenAI ఇంపోర్ట్ అవసరం లేదు
const chatClient = model.createChatClient();

// completeChat OpenAI-తో అనుకూలమైన స్పందన వస్తువును 반환ిస్తుంది
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// స్ట్రీమింగ్ కాల్‌బ్యాక్ ప్యాటర్న్ ఉపయోగిస్తుంది
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` టूल్స్ కాలింగ్‌ను కూడా మద్దతు ఇస్తుంది — రెండవ ఆర్గుమెంట్‌గా టూల్స్ పంపండి:

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

> **எప్పుడు எどன pattern వాడాలి:**
> - **`createChatClient()`** — త్వరిత ప్రోటోటైపింగ్, తక్కువ డిపెండెన్సులు, సులభమైన కోడ్
> - **OpenAI SDK** — పర్యవేక్షణ పై పూర్తి నియంత్రణ (తాపం, top_p, స్టాప్ టోకెన్లు మొదలైనవి), ఉత్పత్తి అప్లికేషన్లకు ఉత్తమం

---

### వ్యాయామం 8: మోడల్ వేరియంట్లు మరియు హార్డ్వేర్ ఎంపిక

మోడల్స్‌లో వేర్వేరు హార్డ్వేర్ కోసం అనుకూలీకరించిన ఎన్నో **వేరియంట్లు** ఉండవచ్చు. SDK తనతోనే బాగా పనిచేసే వేరియంట్‌ని ఆటోమేటిక్‌గా ఎంచుకుంటుంది, కానీ మీరు కూడా మాన్యువల్‌గా పరిశీలించి ఎంచుకోవచ్చు.

<details>
<summary><h3>📘 జావాస్క్రిప్ట్</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// అందుబాటులో ఉన్న అన్ని వేరియంట్లను జాబితా చేయండి
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// మీ హార్డ్వేర్‌కు ఉత్తమ వేరియంట్‌ను SDK ఆటోమేటిక్‌గా ఎంచుకుంటుంది
// తప్పించుకోవడానికి, selectVariant()ను ఉపయోగించండి:
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
<summary><h3>🐍 పైథాన్</h3></summary>

పైథాన్‌లో, SDK హార్డ్వేర్ ఆధారంగా ఉత్తమ వేరియంట్‌ని ఆటోమేటిక్‌గా ఎంచుకుంటుంది. ఎంచుకున్నది చూసేందుకు `get_model_info()` ఉపయోగించండి:

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

#### NPU వేరియంట్లు ఉన్న మోడల్స్

కొన్ని మోడల్స్ న్యూరల్ ప్రాసెసింగ్ యూనిట్లు (Qualcomm Snapdragon, Intel Core Ultra) కలిగిన పరికరాల కోసం NPU కి అనుకూలమైన వేరియంట్లు కలిగి ఉంటాయి:

| మోడల్ | NPU వేరియంట్ అందుబాటు |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **సలహా:** NPU సామర్థ్యం ఉన్న హార్డ్వేర్‌లో, SDK అందుబాటులో ఉన్నప్పుడు ఆటోమేటిక్‌గా NPU వేరియంట్‌ని ఎంచుకుంటుంది. మీ కోడ్ మార్చాల్సిన అవసరం లేదు. Windows C# ప్రాజెక్టుల్లో, QNN ఎగ్జిక్యూషన్ ప్రొవైడర్ ఎనేబుల్ చేయడానికి `Microsoft.AI.Foundry.Local.WinML` NuGet ప్యాకేజీని జోడించండి — QNN WinML ద్వారా ప్లగిన్ EP గా సరఫరా చేయబడుతుంది.

---

### వ్యాయామం 9: మోడల్ అప్గ్రేడ్‌లు మరియు క్యాటలాగ్ రిఫ్రెష్

మోడల్ క్యాటలాగ్ పీరియాడిక్‌గా నవీకరించబడుతుంది. నవీకరణలను తనిఖీ చేయడానికి మరియు వర్తింపజేయడానికి ఈ విధానాలను ఉపయోగించండి.

<details>
<summary><h3>🐍 పైథాన్</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# తాజా మోడల్ జాబితాను పొందడానికి కాటలాగ్‌ను రిఫ్రెష్ చేయండి
manager.refresh_catalog()

# క్యాష్ చేసిన మోడల్‌కు కొత్త వెర్షన్ లభ్యమవుతుందో లేదో తనిఖీ చేయండి
if manager.is_model_upgradeable(alias):
    print(f"{alias} has a newer version available!")
    manager.upgrade_model(alias)
    print("Upgrade complete")
else:
    print(f"{alias} is up to date")
```

</details>

<details>
<summary><h3>📘 జావాస్క్రిప్ట్</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "UpgradeDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// తాజా మోడల్ జాబితా పొందడానికి క్యాటలాగ్‌ని రిఫ్రెష్ చేయండి
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// రిఫ్రెష్ తరువాత అందుబాటులో ఉన్న అన్ని మోడల్స్‌ను జాబితా చేయండి
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### వ్యాయామం 10: రీజనింగ్ మోడల్స్‌తో పని

**phi-4-mini-reasoning** మోడల్ చైన్-ఆఫ్-థాట్ రీజనింగ్‌ను కలిగి ఉంటుంది. ఇది తుది సమాధానం ఇవ్వడం ముందే, `<think>...</think>` ట్యాగ్లలో అంతర్గత ఆలోచనను చుట్టబడుతుంది. ఇది బహుళ-దశ లాజిక్, గణితం లేదా సమస్య పరిష్కారం అవసరమైన పనులకు ఉపయోగకరం.

<details>
<summary><h3>🐍 పైథాన్</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning సుమారు 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# మోడల్ తన ఆలోచనలను <think>...</think> ట్యాగ్‌లలో చుట్టుకొంటే ఉంది
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
<summary><h3>📘 జావాస్క్రిప్ట్</h3></summary>

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

// చైన్-ఆఫ్-థాట్త్ ఆలోచనను పర్సు చేయండి
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **ఎప్పుడు రీజనింగ్ మోడల్స్ వాడాలి:**
> - గణితం మరియు లాజిక్ సమస్యలు
> - బహుళ దశల ప్రణాళికా పనులు
> - సంక్లిష్ట కోడ్ జనరేషన్
> - పని చూపించడం ఖచ్చితత్వం పెంచే పనులు
>
> **వ్యత్యాసం:** రీజనింగ్ మోడల్స్ ఎక్కువ టోకెన్లు (<think> భాగం) ఉత్పత్తి చేస్తాయి మరియు మందగి ఉంటాయి. సాధారణ ప్రశ్న-జవాబులకు, phi-3.5-mini వంటి స్టాండర్డ్ మోడల్ వేగంగా ఉంటుంది.

---

### వ్యాయామం 11: అలియాసులు మరియు హార్డ్వేర్ ఎంపిక అర్థం చేసుకోవడం

మీరు పూర్తి మోడల్ ID కంటే **అలియాస్** (`phi-3.5-mini` లాంటిది) ప్రాసెస్ చేసినపుడు, SDK మీ హార్డ్వేర్‌కు ఉత్తమ వేరియంట్‌ని ఆటోమేటిక్‌గా ఎంచుకుంటుంది:

| హార్డ్వేర్ | ఎంచుకున్న ఎగ్జిక్యూషన్ ప్రొవైడర్ |
|------------|---------------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML ప్లగిన్ ద్వారా) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| ఏదైనా పరికరం (Fallback) | `CPUExecutionProvider` లేదా `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# ఆ అలియాస్ మీ హార్డ్వేర్ కోసం ఉత్తమ వేరియంట్‌ను పరిష్కరించుతుంది
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **సలహా:** మీ అప్లికేషన్ కోడ్‌లో ఎల్లప్పుడూ అలియాసులను ఉపయోగించండి. యూజర్ మెషీన్‌కి డిప్లోయ్ చేసినప్పుడు, SDK సమయానుసారం ఉత్తమ వేరియంట్‌ని ఎంచుకుంటుంది - NVIDIAకి CUDA, Qualcommకి QNN, మిగిలినవాటికి CPU.

---

### వ్యాయామం 12: C# కాన్ఫిగరేషన్ ఎంపికలు

C# SDK `Configuration` క్లాస్ రన్‌టైమ్‌పై ఫైన్-గ్రెయిన్ నియంత్రణని అందిస్తుంది:

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

| ప్రాపర్టీ | డిఫాల్ట్ | వివరణ |
|----------|----------|---------|
| `AppName` | (అవసరం) | మీ యాప్ పేరు |
| `LogLevel` | `Information` | లాగింగ్ verbosity |
| `Web.Urls` | (డైనమిక్) | వెబ్ సర్వర్‌కు స్పెసిఫిక్ పోర్ట్‌ను పిన్ చేయండి |
| `AppDataDir` | OS డిఫాల్ట్ | యాప్ డేటా కోసం బేస్ డైరెక్టరీ |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | మోడల్స్ నిల్వ చేసే చోటు |
| `LogsDir` | `{AppDataDir}/logs` | లాగ్‌లు రాయబడే చోటు |

---

### వ్యాయామం 13: బ్రౌజర్ వాడకం (జావాస్క్రిప్ట్ మాత్రమే)

జావాస్క్రిప్ట్ SDK బ్రౌజర్-కంపాటిబుల్ వెర్షన్‌ను అందిస్తుంది. బ్రౌజర్‌లో, మీరు CLI ద్వారా మాన్యువల్‌గా సర్వీస్ ప్రారంభించి హోస్ట్ URL ని నిర్దేశించాలి:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// మొదటే సర్వీసును మాన్యువల్‌గా ప్రారంభించండి:
//   foundry service start
// తరువాత CLI అవుట్‌పుట్ నుండి URL ను ఉపయోగించండి
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// క్యాటలాగ్ ను బ్రౌజ్ చేయండి
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **బ్రౌజర్ పరిమితులు:** బ్రౌజర్ వెర్షన్ `startWebService()` ని మద్దతు ఇవ్వదు. బ్రౌజర్‌లో SDK వాడేముందు Foundry Local సర్వీస్ ఇప్పటికే రన్ అవ్వాల్సి ఉంటుంది.

---

## పూర్తి API సూచిక

### పైథాన్

| వర్గం | పద్ధతి | వివరణ |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | మేనేజర్ సృష్టించండి; ఐచ్ఛికంగా మోడల్‌తో బూట్‌స్ట్రాప్ చేయండి |
| **సర్వీస్** | `is_service_running()` | సర్వీస్ నడుస్తుందా కాదా తెలుసుకోండి |
| **సర్వీస్** | `start_service()` | సర్వీస్ ప్రారంభించండి |
| **సర్వీస్** | `endpoint` | API ఎండ్‌పాయింట్ URL |
| **సర్వీస్** | `api_key` | API కీ |
| **క్యాటలాగ్** | `list_catalog_models()` | అందుబాటులో ఉన్న అన్ని మోడల్స్ జాబితా |
| **క్యాటలాగ్** | `refresh_catalog()` | క్యాటలాగ్ రిఫ్రెష్ చేయండి |
| **క్యాటలాగ్** | `get_model_info(alias_or_model_id)` | మోడల్ మెటాడేటా పొందండి |
| **కాషే** | `get_cache_location()` | కాషే డైరెక్టరీ మార్గం |
| **కాషే** | `list_cached_models()` | డౌన్‌లోడ్ చేసిన మోడల్స్ జాబితా |
| **మోడల్** | `download_model(alias_or_model_id)` | మోడల్ డౌన్‌లోడ్ చేయండి |
| **మోడల్** | `load_model(alias_or_model_id, ttl=600)` | మోడల్ లోడ్ చేయండి |
| **మోడల్** | `unload_model(alias_or_model_id)` | మోడల్ అన్‌లోడ్ చేయండి |
| **మోడల్** | `list_loaded_models()` | లోడ్ అయిన మోడల్స్ జాబితా |
| **మోడల్** | `is_model_upgradeable(alias_or_model_id)` | కొత్త వెర్షన్ అందుబాటులో ఉందా చూడండి |
| **మోడల్** | `upgrade_model(alias_or_model_id)` | మోడల్ని తాజా వెర్షన్‌గా అప్గ్రేడ్ చేయండి |
| **సర్వీస్** | `httpx_client` | నేరుగా API కాల్స్‌కు ప్రీ-కాన్ఫిగర్ HTTPX క్లయింట్ |

### జావాస్క్రిప్ట్

| వర్గం | పద్ధతి | వివరణ |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | SDK సింగిల్టన్‌ను ప్రారంభించండి |
| **Init** | `FoundryLocalManager.instance` | సింగిల్టన్ మేనేజర్‌కి యాక్సెస్ |
| **సర్వీస్** | `manager.startWebService()` | వెబ్ సర్వీస్ ప్రారంభించండి |
| **సర్వీస్** | `manager.urls` | సర్వీస్‌ కోసం బేస్ URLs అర్రే |
| **క్యాటలాగ్** | `manager.catalog` | మోడల్ క్యాటలాగ్ యాక్సెస్ చేయండి |
| **క్యాటలాగ్** | `catalog.getModel(alias)` | అలియాస్ ద్వారా మోడల్ వస్తువు పొందండి (ప్రామిస్ రిటర్న్) |
| **మోడల్** | `model.isCached` | మోడల్ డౌన్‌లోడ్ అయిందా తెలుసుకోండి |
| **మోడల్** | `model.download()` | మోడల్ డౌన్‌లోడ్ చేయండి |
| **మోడల్** | `model.load()` | మోడల్ లోడ్ చేయండి |
| **మోడల్** | `model.unload()` | మోడల్ అన్‌లోడ్ చేయండి |
| **మోడల్** | `model.id` | మోడల్ యొక్క యునిక ఐడీ |
| **మోడల్** | `model.alias` | మోడల్ అలియాస్ |
| **మోడల్** | `model.createChatClient()` | నేటివ్ చాట్ క్లయింట్ పొందండి (OpenAI SDK అవసరం లేదు) |
| **మోడల్** | `model.createAudioClient()` | ట్రాన్స్క్రిప్షన్ కోసం ఆడియో క్లయింట్ పొందండి |
| **మోడల్** | `model.removeFromCache()` | స్థానిక కాషే నుంచి మోడల్ తొలగించండి |
| **మోడల్** | `model.selectVariant(variant)` | ఒక ప్రత్యేక హార్డ్వేర్ వేరియంట్ ఎంచుకోండి |
| **మోడల్** | `model.variants` | ఈ మోడల్‌కు అందుబాటు వేరియంట్ల అర్రే |
| **మోడల్** | `model.isLoaded()` | మోడల్ ప్రస్తుతం లోడ్ అయిందా తెలుసుకోండి |
| **క్యాటలాగ్** | `catalog.getModels()` | అందుబాటులో ఉన్న మోడల్స్ జాబితా |
| **క్యాటలాగ్** | `catalog.getCachedModels()` | డౌన్‌లోడ్ చేసిన మోడల్స్ జాబితా |
| **క్యాటలాగ్** | `catalog.getLoadedModels()` | ప్రస్తుతం లోడ్ అయిన మోడల్స్ జాబితా |
| **క్యాటలాగ్** | `catalog.updateModels()` | సర్వీస్ నుండి క్యాటలాగ్ రిఫ్రెష్ చేయండి |
| **సర్వీస్** | `manager.stopWebService()` | Foundry Local వెబ్ సర్వీస్ ఆపండి |

### C# (v0.8.0+)

| వర్గం | పద్ధతి | వివరణ |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | మేనేజర్ ని ఆరంభించండి |
| **Init** | `FoundryLocalManager.Instance` | సింగిల్టన్ యాక్సెస్ |
| **క్యాటలాగ్** | `manager.GetCatalogAsync()` | క్యాటలాగ్ పొందండి |
| **క్యాటలాగ్** | `catalog.ListModelsAsync()` | అన్ని మోడల్స్ జాబితా |
| **క్యాటలాగ్** | `catalog.GetModelAsync(alias)` | ఒక నిర్దిష్ట మోడల్ పొందండి |
| **క్యాటలాగ్** | `catalog.GetCachedModelsAsync()` | కాషే మోడల్స్ జాబితా |
| **క్యాటలాగ్** | `catalog.GetLoadedModelsAsync()` | లోడ్ అయిన మోడల్స్ జాబితా |
| **మోడల్** | `model.DownloadAsync(progress?)` | మోడల్ డౌన్‌లోడ్ చేయండి |
| **మోడల్** | `model.LoadAsync()` | మోడల్ లోడ్ చేయండి |
| **మోడల్** | `model.UnloadAsync()` | మోడల్ అన్‌లోడ్ చేయండి |
| **మోడల్** | `model.SelectVariant(variant)` | హార్డ్వేర్ వేరియంట్ ఎంచుకోండి |
| **మోడల్** | `model.GetChatClientAsync()` | నేటివ్ చాట్ క్లయింట్ పొందండి |
| **మోడల్** | `model.GetAudioClientAsync()` | ఆడియో ట్రాన్స్క్రిప్షన్ క్లయింట్ పొందండి |
| **మోడల్** | `model.GetPathAsync()` | స్థానిక ఫైల్ మార్గం పొందండి |
| **క్యాటలాగ్** | `catalog.GetModelVariantAsync(alias, variant)` | నిర్దిష్ట హార్డ్వేర్ వేరియంట్ పొందండి |
| **క్యాటలాగ్** | `catalog.UpdateModelsAsync()` | క్యాటలాగ్ రిఫ్రెష్ చేయండి |
| **సర్వర్** | `manager.StartWebServerAsync()` | REST వెబ్ సర్వర్ ప్రారంభించండి |
| **సర్వర్** | `manager.StopWebServerAsync()` | REST వెబ్ సర్వర్ ఆపండి |
| **కాన్ఫిగ్** | `config.ModelCacheDir` | కాషే డైరెక్టరీ |

---

## ముఖ్య విషయాలు

| కాన్సెప్టు | మీరు నేర్చుకున్నది |
|---------|-----------------|
| **SDK vs CLI** | SDK ప్రోగ్రామాటిక్ నియంత్రణ అందిస్తుంది - అప్లికేషన్ల కోసం అత్యవసరం |
| **కంట్రోల్ ప్లేన్** | SDK సర్వీసులు, మోడల్స్, కాషే మేనేజ్ చేస్తుంది |
| **డైనమిక్ పోర్ట్స్** | ఎప్పుడూ `manager.endpoint` (పైథాన్) లేదా `manager.urls[0]` (JS/C#) ఉపయోగించండి - పోర్ట్ హార్డ్కోడింగ్ చేయవద్దు |
| **అలియాసులు** | ఆటోమేటిక్ హార్డ్వేర్-ఆప్టిమల్ మోడల్ ఎంపిక కోసం అలియాసులు ఉపయోగించండి |
| **అత్యంత త్వరిత ప్రారంభం** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# మళ్లీ రూపకల్పన** | v0.8.0+ స్వయం-సంపూర్ణం - చివరి వినియోగదారు మెషీన్లపై CLI అవసరం లేదు |
| **మోడల్ జీవనచక్రం** | క్యాటలాగ్ → డౌన్‌లోడ్ → లోడ్ → ఉపయోగించు → అన్లోడ్ |
| **FoundryModelInfo** | సంపన్న మెటాడేటా: టాస్క్, డివైస్, పరిమాణం, లైసెన్స్, టూల్ కాలింగ్ మద్దతు |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) ఓపెన్‌ఏఐ-వెనుకవుండే వినియోగం కోసం |
| **వేరియంట్లు** | మోడల్స్ కు హార్డ్వేర్-ప్రత్యేక వేరియంట్లు ఉండాయి (CPU, GPU, NPU); ఆటోమేటిక్‌గా ఎంచుకోబడతాయి |
| **అప్‌గ్రేడ్‌లు** | Python: `is_model_upgradeable()` + `upgrade_model()` మోడల్స్‌ను తాజాకరించడానికి |
| **క్యాటలాగ్ రిఫ్రెష్** | `refresh_catalog()` (Python) / `updateModels()` (JS) కొత్త మోడల్స్ కనుగొనటానికి |

---

## వనరులు

| వనరు | లింక్ |
|----------|------|
| SDK సూచిక (అన్ని భాషలు) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| ఇన్‌ఫరెన్స్ SDKలు ఒకచోట చేర్చడం | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API సూచిక | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK నమూనాలు | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local వెబ్‌సైట్ | [foundrylocal.ai](https://foundrylocal.ai) |

---

## తర్వాతి దశలు

SDKని ఓపెన్‌ఏఐ క్లయింట్ లైబ్రరీతో లింక్ చేసి మీ మొదటి చాట్ కంప్లీషన్ అప్లికేషన్‌ను రూపొందించేందుకు [Part 3: Using the SDK with OpenAI](part3-sdk-and-apis.md)కి కొనసాగించండి.