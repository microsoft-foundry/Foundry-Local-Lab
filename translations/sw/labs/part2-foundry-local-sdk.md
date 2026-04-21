![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Sehemu ya 2: Undani wa SDK ya Foundry Local

> **Lengo:** Kuwa mtaalamu wa SDK ya Foundry Local kusimamia mifano, huduma, na kuhifadhi kwa njia ya programu - na kuelewa kwa nini SDK inashauriwa badala ya CLI kwa ajili ya kujenga programu.

## Muhtasari

Katika Sehemu ya 1 ulitumia **Foundry Local CLI** kupakua na kuendesha mifano kwa njia ya ushirikiano. CLI ni nzuri kwa uchunguzi, lakini unapo jenga programu halisi unahitaji **uduari wa programu**. SDK ya Foundry Local inakupa hilo - inasimamia **dhibiti ndege** (kuanza huduma, kugundua mifano, kupakua, kupakia) ili msimbo wa programu yako uweze kuzingatia **ndege ya data** (kutuma maelekezo, kupokea maelezo kamili).

Maabara hii inakufundisha uso wote wa API ya SDK kupitia Python, JavaScript, na C#. Mwisho wake utaelewa kila njia inayopatikana na lini kutumia kila moja.

## Malengo ya Kujifunza

Mwisho wa maabara hii utaweza:

- Eleza kwa nini SDK inapendekezwa kuliko CLI kwa maendeleo ya programu
- Sakinisha SDK ya Foundry Local kwa Python, JavaScript, au C#
- Tumia `FoundryLocalManager` kuanza huduma, kusimamia mifano, na kuulizia katalogi
- Orodhesha, pakua, pakia, na toa mifano kwa njia ya programu
- Chunguza metadata ya mfano kwa kutumia `FoundryModelInfo`
- Elewa tofauti kati ya katalogi, cache, na mifano iliyopakuliwa
- Tumia msanidi bootstrap (Python) na muundo wa `create()` + katalogi (JavaScript)
- Elewa muundo mpya wa SDK ya C# na API yake ya vitu

---

## Mahitaji

| Hitaji | Maelezo |
|-------------|---------|
| **Foundry Local CLI** | Imewekwa na iko kwenye `PATH` yako ([Sehemu ya 1](part1-getting-started.md)) |
| **Lugha ya mtiririko** | **Python 3.9+** na/au **Node.js 18+** na/au **.NET 9.0+** |

---

## Dhana: SDK dhidi ya CLI - Kwa Nini Utumie SDK?

| Kipengele | CLI (amri `foundry`) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Matumizi** | Uchunguzi, majaribio ya mikono | Muunganisho wa programu |
| **Usimamizi wa huduma** | Mikono: `foundry service start` | Kiotomatiki: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Ugunduzi wa bandari** | Soma kutoka matokeo ya CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Kupakua mfano** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Usimamizi wa makosa** | Msimbo wa kutoka, stderr | Isimoshi, makosa yenye aina |
| **Uendeshaji wa mfumo** | Skripti za shell | Muunganisho wa lugha asilia |
| **Ueneaji** | Inahitaji CLI kwenye mashine ya mtumiaji mwisho | SDK ya C# inaweza kujitegemea (haina hitaji la CLI) |

> **Ufahamu muhimu:** SDK inasimamia mzunguko mzima wa maisha: kuanza huduma, kuangalia cache, kupakua mifano isiyopatikana, kuipakia, na kugundua endpoint, kwa mistari michache ya msimbo. Programu yako haihitaji kuchambua matokeo ya CLI au kusimamia michakato ndogo.

---

## Mazoezi ya Maabara

### Zoëzi 1: Sakinisha SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Thibitisha usakinishaji:

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

Thibitisha usakinishaji:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Kuna vifurushi viwili vya NuGet:

| Kifurushi | Jukwaa | Maelezo |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Msalaba-jukwaa | Hufanya kazi Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Windows pekee | Huongeza kasi ya vifaa vya WinML; hupakua na kusanidua wasambazaji wa utekelezaji wa plugin (mfano QNN kwa Qualcomm NPU) |

**Mipangilio ya Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Hariri faili ya `.csproj`:

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

> **Kumbuka:** Kwa Windows, kifurushi cha WinML ni sehemu kamili inayojumuisha SDK ya msingi pamoja na msambazaji wa utekelezaji wa QNN. Kwa Linux/macOS, SDK ya msingi hutumika badala yake. TFM ya masharti na marejeleo ya vifurushi huweka mradi kuwa msalaba-jukwaa kikamilifu.

Tengeneza `nuget.config` kwenye mzizi wa mradi:

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

Rudisha vifurushi:

```bash
dotnet restore
```

</details>

---

### Zoëzi 2: Anzisha Huduma na Orodhesha Katalogi

Kitu cha kwanza ambacho programu yoyote hufanya ni kuanzisha huduma ya Foundry Local na kugundua mifano inayopatikana.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Unda meneja na anza huduma
manager = FoundryLocalManager()
manager.start_service()

# Orodhesha modeli zote zinazopatikana kwenye katalogi
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Njia za Usimamizi wa Huduma

| Njia | Sahihi | Maelezo |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Angalia ikiwa huduma inaendeshwa |
| `start_service()` | `() -> None` | Anzisha huduma ya Foundry Local |
| `service_uri` | `@property -> str` | URI ya msaada wa huduma |
| `endpoint` | `@property -> str` | Endpoint ya API (URI ya huduma + `/v1`) |
| `api_key` | `@property -> str` | Funguo ya API (kutoka kwenye mazingira au kielekezi cha chaguo-msingi) |

#### Python SDK - Njia za Usimamizi wa Katalogi

| Njia | Sahihi | Maelezo |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Orodhesha mifano yote katika katalogi |
| `refresh_catalog()` | `() -> None` | Sasisha katalogi kutoka huduma |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=false) -> FoundryModelInfo \| None` | Pata taarifa za mfano maalum |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Unda meneja na anza huduma
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Vinjari orodha ya bidhaa
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Njia za Meneja

| Njia | Sahihi | Maelezo |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Anzisha singleton ya SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Pata meneja singleton |
| `manager.startWebService()` | `() => Promise<void>` | Anzisha huduma ya wavuti ya Foundry Local |
| `manager.urls` | `string[]` | Orodha ya URL za msingi kwa huduma |

#### JavaScript SDK - Njia za Katalogi na Mfano

| Njia | Sahihi | Maelezo |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Pata katalogi ya mfano |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Pata kitu cha mfano kwa jina la utani |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

SDK ya C# toleo v0.8.0+ inatumia usanifu wa vitu wenye vitu kama `Configuration`, `Catalog`, na `Model`:

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

#### C# SDK - Madarasa Muhimu

| Darasa | Kusudi |
|-------|---------|
| `Configuration` | Weka jina la app, kiwango cha kumbukumbu, saraka ya cache, URL za server ya wavuti |
| `FoundryLocalManager` | Sehemu kuu ya kuingia - inaundwa kupitia `CreateAsync()`, inapatikana kupitia `.Instance` |
| `Catalog` | Vinjari, tafuta, na pata mifano kutoka katalogi |
| `Model` | Inaonyesha mfano maalum - pakua, pakia, pata wateja |

#### C# SDK - Njia za Meneja na Katalogi

| Njia | Maelezo |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Anzisha meneja |
| `FoundryLocalManager.Instance` | Pata meneja singleton |
| `manager.GetCatalogAsync()` | Pata katalogi ya mfano |
| `catalog.ListModelsAsync()` | Orodhesha mifano yote inayopatikana |
| `catalog.GetModelAsync(alias: "alias")` | Pata mfano maalum kwa jina la utani |
| `catalog.GetCachedModelsAsync()` | Orodhesha mifano zilizopakuliwa |
| `catalog.GetLoadedModelsAsync()` | Orodhesha mifano zilizopakiwa sasa |

> **Kumbuka Muundo wa C#:** SDK ya C# toleo v0.8.0+ hurekebisha ili programu iwe **hujiendesha yenyewe**; haihitaji CLI ya Foundry Local kwenye mashine ya mtumiaji mwisho. SDK inasimamia usimamizi wa mfano na utoaji wa taarifa kiasili.

</details>

---

### Zoëzi 3: Pakua na Pakia Mfano

SDK inatenganisha kupakua (kwenye diski) na kupakia (ndani ya kumbukumbu). Hii inakuwezesha kupakua mifano kabla wakati wa usanidi na kuipakia unapotaka.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Chaguo A: Hatua kwa hatua kwa mkono
manager = FoundryLocalManager()
manager.start_service()

# Angalia cache kwanza
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

# Chaguo B: Bootstrapping ya mstari mmoja (inayopendekezwa)
# Pita jina la utani kwenye muanzishaji - huanzisha huduma, inapakua, na inapakia moja kwa moja
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Njia za Usimamizi wa Mfano

| Njia | Sahihi | Maelezo |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Pakua mfano hadi cache ya eneo la ndani |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Paka mfano kwenye server ya maelezo kamili |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Toa mfano kutoka server |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Orodhesha mifano yote inayopakiwa sasa |

#### Python - Njia za Usimamizi wa Cache

| Njia | Sahihi | Maelezo |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Pata njia ya saraka ya cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Orodhesha mifano yote iliyopakuliwa |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Njia hatua kwa hatua
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

#### JavaScript - Njia za Mfano

| Njia | Sahihi | Maelezo |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Ikiwa mfano tayari umepakuliwa |
| `model.download()` | `() => Promise<void>` | Pakua mfano hadi cache ya ndani |
| `model.load()` | `() => Promise<void>` | Paka kwenye server ya maelezo kamili |
| `model.unload()` | `() => Promise<void>` | Toa kutoka server ya maelezo kamili |
| `model.id` | `string` | Kitambulisho cha kipekee cha mfano |

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

#### C# - Njia za Mfano

| Njia | Maelezo |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Pakua tofauti iliyochaguliwa |
| `model.LoadAsync()` | Paka mfano ndani ya kumbukumbu |
| `model.UnloadAsync()` | Toa mfano |
| `model.SelectVariant(variant)` | Chagua toleo la mfano (CPU/GPU/NPU) |
| `model.SelectedVariant` | Toleo la sasa lililochaguliwa |
| `model.Variants` | Tofauti zote zinazo patikana za mfano huu |
| `model.GetPathAsync()` | Pata njia ya faili la eneo la ndani |
| `model.GetChatClientAsync()` | Pata mteja wa mazungumzo wa asili (haina hitaji la SDK ya OpenAI) |
| `model.GetAudioClientAsync()` | Pata mteja wa sauti kwa uandishi wa maandishi |

</details>

---

### Zoëzi 4: Chunguza Metadata ya Mfano

Kitu cha `FoundryModelInfo` kina metadata tajiri kuhusu kila mfano. Kuelewa maeneo haya kunakusaidia kuchagua mfano sahihi kwa programu yako.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Pata maelezo ya kina kuhusu mfano maalum
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

#### Maeneo ya FoundryModelInfo

| Sehemu | Aina | Maelezo |
|-------|------|-------------|
| `alias` | string | Jina fupi (mfano `phi-3.5-mini`) |
| `id` | string | Kitambulisho cha kipekee cha mfano |
| `version` | string | Toleo la mfano |
| `task` | string | `chat-completions` au `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, au NPU |
| `execution_provider` | string | Muunganisho wa wakati wa utekelezaji (CUDA, CPU, QNN, WebGPU, n.k.) |
| `file_size_mb` | int | Ukubwa kwenye diski kwa MB |
| `supports_tool_calling` | bool | Ikiwa mfano unasaidia kuita kazi/vifaa |
| `publisher` | string | Aliyetangaza mfano |
| `license` | string | Jina la leseni |
| `uri` | string | URI ya mfano |
| `prompt_template` | dict/null | Kiolezo cha mwito, ikiwa ipo |

---

### Zoëzi 5: Simamia Mzunguko wa Maisha wa Mfano

Mazoezi ya mzunguko mzima wa maisha: orodha → pakua → paka → tumia → toa.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Mfano mdogo kwa majaribio ya haraka

manager = FoundryLocalManager()
manager.start_service()

# 1. Angalia kile kilicho katika katalogi
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Angalia kile kilicho tayari kupakuliwa
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Pakua mfano
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Thibitisha kuwa sasa iko kwenye cache
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Ipakue
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Angalia kile kilicho pakuliwa
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Iondoshe
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

const alias = "qwen2.5-0.5b"; // Mfano mdogo kwa majaribio ya haraka

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Pata mfano kutoka kwenye katalogi
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Pakua ikiwa inahitajika
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Upakie
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Uondoe pakiti yake
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Zo упражения 6: Mifumo ya Kuanzia Haraka

Kila lugha hutoa njia ya mkato kuanzisha huduma na kupakia mfano katika wito mmoja. Hizi ni **mifumo iliyo pendekezwa** kwa matumizi mengi ya programu.

<details>
<summary><h3>🐍 Python - Kuanzisha kwa Constructor</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Pitia jina mbadala kwa constructor - inashughulikia kila kitu:
# 1. Inaanzisha huduma ikiwa haijawashwa
# 2. Inapakua mfano ikiwa haujohifadhiwa
# 3. Inapakia mfano kwenye seva ya uchambuzi
manager = FoundryLocalManager("phi-3.5-mini")

# Tayari kutumika mara moja
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Kipengele `bootstrap` (chaguo-msingi `True`) hudhibiti tabia hii. Weka `bootstrap=False` ikiwa unataka udhibiti wa mikono:

```python
# Mode ya mwongozo - hakuna kinachotokea kiotomatiki
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalogi</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() hushughulikia kila kitu:
// 1. Huanzisha huduma
// 2. Hupata mfano kutoka kwenye katalogi
// 3. Hushusha ikiwa inahitajika na huingiza mfano
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Tayari kutumiwa mara moja
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Katalogi</h3></summary>

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

> **Kumbuka C#:** SDK ya C# inatumia `Configuration` kudhibiti jina la programu, uandikishaji wa kumbukumbu, folda za cache, na hata kuweka bandari maalum ya seva ya wavuti. Hii inafanya iwe yenye uwezo zaidi wa kubadilika kati ya SDK tatu.

</details>

---

### Zo упражения 7: ChatClient Asili (Hakuna SDK ya OpenAI Inahitajika)

SDK za JavaScript na C# hutoa njia rahisi ya `createChatClient()` inayorejesha mteja wa mazungumzo wa asili — hakuna haja ya kusakinisha au kubadilisha SDK ya OpenAI tofauti.

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

// Tengeneza ChatClient moja kwa moja kutoka kwa modeli — hakuna haja ya kuingiza OpenAI
const chatClient = model.createChatClient();

// completeChat hurudisha kitu cha majibu kinacholingana na OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming inatumia muundo wa callback
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` pia inaunga mkono kuitwa zana — pita zana kama hoja ya pili:

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

> **Lini kutumia mfumo gani:**
> - **`createChatClient()`** — Kubuni haraka, utegemezi mdogo, msimbo rahisi
> - **SDK ya OpenAI** — Udhibiti kamili wa vigezo (joto, top_p, tokeni za kusimama, n.k.), bora kwa matumizi ya uzalishaji

---

### Zo упражения 8: Tofauti za Mfano na Uteuzi wa Vifaa

Modeli zinaweza kuwa na **tofauti** kadhaa zilizo boreshwa kwa vifaa tofauti. SDK huchagua toleo bora kiotomatiki, lakini pia unaweza kuangalia na kuchagua kwa mikono.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Orodhesha aina zote zinazopatikana
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK hujichagua moja kwa moja aina bora kwa vifaa vyako
// Ili kubadilisha, tumia selectVariant():
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

Katika Python, SDK huchagua toleo bora kulingana na vifaa kiotomatiki. Tumia `get_model_info()` kuona kilichochaguliwa:

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

#### Modeli Zaine Tofauti za NPU

Modeli zingine zina toleo lililoborezwa kwa NPU kwa vifaa vyenye Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Mfano | Tofauti ya NPU Inapatikana |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Kidokezo:** Kwenye vifaa vya NPU, SDK huchagua toleo la NPU kiotomatiki wakati linapatikana. Huna haja ya kubadilisha msimbo wako. Kwa miradi ya C# kwenye Windows, ongeza kifurushi cha NuGet `Microsoft.AI.Foundry.Local.WinML` kuwezesha msambazaji wa utekelezaji wa QNN — QNN hutolewa kama plugin EP kupitia WinML.

---

### Zo упражения 9: Kuboresha Modeli na Kusasisha Katalogi

Katalogi ya modeli husasishwa mara kwa mara. Tumia njia hizi kuangalia na kutekeleza sasisho.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Sasisha orodha kupata orodha ya modeli za hivi karibuni
manager.refresh_catalog()

# Angalia kama modeli iliyohifadhiwa ina toleo jipya zaidi linalopatikana
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

// Sasisha katalogi kupakua orodha ya modeli za hivi karibuni
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Orodhesha modeli zote zinazopatikana baada ya kusasisha
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Zo упражения 10: Kufanya kazi na Modeli za Ufikiri

Mfano **phi-4-mini-reasoning** una kufikiri kwa mnyororo wa mawazo (chain-of-thought). Hujifunga kwa alama `<think>...</think>` kabla ya kutoa jibu la mwisho. Hii ni muhimu kwa kazi zinazohitaji mantiki za hatua nyingi, hisabati, au kutatua matatizo.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning ni takriban 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Mfano huweka mawazo yake ndani ya lebo za <think>...</think>
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

// Tafsiri fikiria mnyororo wa mawazo
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Lini kutumia modeli za ufikiri:**
> - Matatizo ya hisabati na mantiki
> - Kazi za upangaji wa hatua nyingi
> - Uundaji wa msimbo mgumu
> - Kazi ambapo kuonyesha kazi kunaboresha usahihi
>
> **Kifurushi:** Modeli za ufikiri hutengeneza tokeni zaidi (sehemu ya `<think>`) na huchukua muda mrefu. Kwa maswali rahisi, mfano wa kawaida kama phi-3.5-mini ni wa kasi zaidi.

---

### Zo упражения 11: Kuelewa Majina Mbadala na Uteuzi wa Vifaa

Unapopita **jina mbadala** (kama `phi-3.5-mini`) badala ya kitambulisho kamili cha mfano, SDK huchagua toleo bora kwa vifaa vyako kiotomatiki:

| Vifaa | Msambazaji wa Utekelezaji Ulioteuliwa |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (kupitia plugin ya WinML) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Kifaa chochote (mara ya dharura) | `CPUExecutionProvider` au `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Jina la utani linaelekeza kwa toleo bora zaidi kwa vifaa VYAKO
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Kidokezo:** Tumia majina mbadala kila wakati kwenye msimbo wa programu yako. Unapoweka kwenye mashine ya mtumiaji, SDK huchagua toleo bora kwa wakati wa utekelezaji - CUDA kwa NVIDIA, QNN kwa Qualcomm, CPU mahali pengine.

---

### Zo упражения 12: Chaguzi za Usanidi wa C#

Darasa la `Configuration` la SDK ya C# hutoa udhibiti wa kina wa wakati wa utekelezaji:

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

| Mali | Chaguo-msingi | Maelezo |
|----------|---------|-------------|
| `AppName` | (inahitajika) | Jina la programu yako |
| `LogLevel` | `Information` | Kiwango cha uandikishaji kumbukumbu |
| `Web.Urls` | (kinachobadilika) | Weka bandari maalum kwa seva ya wavuti |
| `AppDataDir` | Kawaida ya OS | Saraka msingi kwa data ya programu |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Mahali ambapo modeli huhifadhiwa |
| `LogsDir` | `{AppDataDir}/logs` | Mahali ambapo kumbukumbu huandikwa |

---

### Zo упражения 13: Matumizi ya Kivinjari (JavaScript Pekee)

SDK ya JavaScript ina toleo linalofaa kivinjari. Kwenye kivinjari, lazima uanze huduma kwa mkono kupitia CLI na uainishe URL ya mwenyeji:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Anzisha huduma kwa mkono kwanza:
//   anza huduma ya foundry
// Kisha tumia URL kutoka kwa matokeo ya CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Vinjari kwenye katalogi
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Mabingwa ya kivinjari:** Toa la kivinjari haliiungi mkono `startWebService()`. Lazima uhakikishe Huduma ya Foundry Local inakimbia kabla ya kutumia SDK kwenye kivinjari.

---

## Marejeleo Kamili ya API

### Python

| Kundi | Njia | Maelezo |
|----------|--------|-------------|
| **Anzishi** | `FoundryLocalManager(alias?, bootstrap=True)` | Tengeneza meneja; hiari anzisha kwa mfano |
| **Huduma** | `is_service_running()` | Angalia kama huduma inaendeshwa |
| **Huduma** | `start_service()` | Anzisha huduma |
| **Huduma** | `endpoint` | URL ya mwisho wa API |
| **Huduma** | `api_key` | Funguo ya API |
| **Katalogi** | `list_catalog_models()` | Orodhesha modeli zote zinazopatikana |
| **Katalogi** | `refresh_catalog()` | Sasisha katalogi |
| **Katalogi** | `get_model_info(alias_or_model_id)` | Pata metadata ya mfano |
| **Cache** | `get_cache_location()` | Njia ya folda ya cache |
| **Cache** | `list_cached_models()` | Orodhesha modeli zilizopakuliwa |
| **Mfano** | `download_model(alias_or_model_id)` | Pakua mfano |
| **Mfano** | `load_model(alias_or_model_id, ttl=600)` | Pakia mfano |
| **Mfano** | `unload_model(alias_or_model_id)` | Ondoa mzigo wa mfano |
| **Mfano** | `list_loaded_models()` | Orodhesha modeli zilizo pakizwa |
| **Mfano** | `is_model_upgradeable(alias_or_model_id)` | Angalia kama toleo jipya linapatikana |
| **Mfano** | `upgrade_model(alias_or_model_id)` | Boresha mfano hadi toleo jipya |
| **Huduma** | `httpx_client` | Mteja wa HTTPX uliotayarishwa kwa wito za moja kwa moja API |

### JavaScript

| Kundi | Njia | Maelezo |
|----------|--------|-------------|
| **Anzishi** | `FoundryLocalManager.create(options)` | Anzisha SDK singleton |
| **Anzishi** | `FoundryLocalManager.instance` | Pata meneja singleton |
| **Huduma** | `manager.startWebService()` | Anzisha huduma ya wavuti |
| **Huduma** | `manager.urls` | Msururu wa URL za msingi za huduma |
| **Katalogi** | `manager.catalog` | Pata katalogi ya modeli |
| **Katalogi** | `catalog.getModel(alias)` | Pata kitu cha mfano kwa jina mbadala (hurudisha Ahadi) |
| **Mfano** | `model.isCached` | Je, mfano umeshapakuliwa? |
| **Mfano** | `model.download()` | Pakua mfano |
| **Mfano** | `model.load()` | Pakia mfano |
| **Mfano** | `model.unload()` | Ondoa mzigo wa mfano |
| **Mfano** | `model.id` | Kitambulisho cha kipekee cha mfano |
| **Mfano** | `model.alias` | Jina mbadala la mfano |
| **Mfano** | `model.createChatClient()` | Pata mteja wa mazungumzo wa asili (hakuna SDK ya OpenAI inahitajika) |
| **Mfano** | `model.createAudioClient()` | Pata mteja wa sauti kwa uandishi wa maneno |
| **Mfano** | `model.removeFromCache()` | Ondoa mfano kutoka cache ya ndani |
| **Mfano** | `model.selectVariant(variant)` | Chagua toleo la vifaa maalum |
| **Mfano** | `model.variants` | Msururu wa toleo linapatikana kwa mfano huu |
| **Mfano** | `model.isLoaded()` | Angalia ikiwa mfano umejazwa sasa |
| **Katalogi** | `catalog.getModels()` | Orodhesha modeli zote zinazopatikana |
| **Katalogi** | `catalog.getCachedModels()` | Orodhesha modeli zilizopakuliwa |
| **Katalogi** | `catalog.getLoadedModels()` | Orodhesha modeli zilizo pakizwa sasa |
| **Katalogi** | `catalog.updateModels()` | Sasisha katalogi kutoka huduma |
| **Huduma** | `manager.stopWebService()` | Zima huduma ya wavuti ya Foundry Local |

### C# (v0.8.0+)

| Kundi | Njia | Maelezo |
|----------|--------|-------------|
| **Anzishi** | `FoundryLocalManager.CreateAsync(config, logger)` | Anzisha meneja |
| **Anzishi** | `FoundryLocalManager.Instance` | Pata singleton |
| **Katalogi** | `manager.GetCatalogAsync()` | Pata katalogi |
| **Katalogi** | `catalog.ListModelsAsync()` | Orodhesha modeli zote |
| **Katalogi** | `catalog.GetModelAsync(alias)` | Pata mfano maalum |
| **Katalogi** | `catalog.GetCachedModelsAsync()` | Orodhesha modeli zilizopakuliwa |
| **Katalogi** | `catalog.GetLoadedModelsAsync()` | Orodhesha modeli zilizopakuliwa |
| **Mfano** | `model.DownloadAsync(progress?)` | Pakua mfano |
| **Mfano** | `model.LoadAsync()` | Pakia mfano |
| **Mfano** | `model.UnloadAsync()` | Ondoa mzigo wa mfano |
| **Mfano** | `model.SelectVariant(variant)` | Chagua toleo la vifaa |
| **Mfano** | `model.GetChatClientAsync()` | Pata mteja wa mazungumzo asilia |
| **Mfano** | `model.GetAudioClientAsync()` | Pata mteja wa uandishi wa sauti |
| **Mfano** | `model.GetPathAsync()` | Pata njia ya faili ya eneo la ndani |
| **Katalogi** | `catalog.GetModelVariantAsync(alias, variant)` | Pata toleo maalum la vifaa |
| **Katalogi** | `catalog.UpdateModelsAsync()` | Sasisha katalogi |
| **Seva** | `manager.StartWebServerAsync()` | Anzisha seva ya wavuti ya REST |
| **Seva** | `manager.StopWebServerAsync()` | Zima seva ya wavuti ya REST |
| **Usanidi** | `config.ModelCacheDir` | Folda ya cache |

---

## Muhimu Kuzingatia

| Dhana | Uliyojifunza |
|---------|-----------------|
| **SDK dhidi ya CLI** | SDK hutoa udhibiti wa programu - muhimu kwa programu |
| **Ndege ya udhibiti** | SDK inadhibiti huduma, modeli, na cache |
| **Bandari za mabadiliko** | Tumia kila wakati `manager.endpoint` (Python) au `manager.urls[0]` (JS/C#) - usiweka bandari thabiti |
| **Majina mbadala** | Tumia majina mbadala kwa kuchagua toleo bora kwa vifaa kiotomatiki |
| **Kuanzia kwa haraka** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Ubunifu upya wa C#** | v0.8.0+ ni kujitegemea - hakuna CLI inahitajika kwenye mashine za watumiaji wa mwisho |
| **Mzunguko wa maisha ya Mfano** | Katalogi → Pakua → Pakuzi → Tumia → Toa mzigo |
| **FoundryModelInfo** | Metadata tajiri: kazi, kifaa, ukubwa, leseni, msaada wa zana inayoita |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) kwa matumizi bila OpenAI |
| **Aina** | Mifano ina aina maalum za vifaa (CPU, GPU, NPU); huchaguliwa moja kwa moja |
| **Maboresha** | Python: `is_model_upgradeable()` + `upgrade_model()` kuweka mifano ikiwa ya kisasa |
| **Kusasisha katalogi** | `refresh_catalog()` (Python) / `updateModels()` (JS) kugundua mifano mipya |

---

## Rasilimali

| Rasilimali | Kiungo |
|----------|------|
| Marejeleo ya SDK (lugha zote) | [Microsoft Learn - Marejeleo ya SDK ya Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Unganisha na SDK za uchambuzi | [Microsoft Learn - Muunganisho wa SDK za Uchambuzi](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Marejeleo ya API ya C# SDK | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| Sampuli za C# SDK | [GitHub - Sampuli za Foundry Local SDK](https://aka.ms/foundrylocalSDK) |
| Tovuti ya Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Hatua Zifuatazo

Endelea na [Sehemu ya 3: Kutumia SDK na OpenAI](part3-sdk-and-apis.md) kuunganisha SDK na maktaba ya mteja wa OpenAI na kujenga programu yako ya kwanza ya kukamilisha mazungumzo.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Kang’amuzi**:
Hati hii imetafsiriwa kwa kutumia huduma ya tafsiri ya AI [Co-op Translator](https://github.com/Azure/co-op-translator). Ingawa tunajitahidi kwa usahihi, tafadhali fahamu kuwa tafsiri za kiotomatiki zinaweza kuwa na makosa au ukosefu wa usahihi. Hati ya asili katika lugha yake ya asili inapaswa kuzingatiwa kama chanzo cha mamlaka. Kwa taarifa muhimu, tafsiri ya mtaalamu wa binadamu inapendekezwa. Hatutawajibika kwa kutoelewana au tafsiri isiyo sahihi inayotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->