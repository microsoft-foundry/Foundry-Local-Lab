![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagi 2: Malalim na Pagsisid sa Foundry Local SDK

> **Layunin:** Maging bihasa sa Foundry Local SDK upang pamahalaan ang mga modelo, serbisyo, at caching nang programmatic - at maunawaan kung bakit ang SDK ang inirerekomendang pamamaraan kaysa sa CLI para sa paggawa ng mga aplikasyon.

## Pangkalahatang-ideya

Sa Bahagi 1 ginamit mo ang **Foundry Local CLI** upang mag-download at magpatakbo ng mga modelo nang interaktibo. Maganda ang CLI para sa pagsasaliksik, ngunit kapag gumagawa ka ng totoong aplikasyon kailangan mo ng **programmatic control**. Ibinibigay ng Foundry Local SDK iyon - pinamamahalaan nito ang **control plane** (pagsisimula ng serbisyo, pagtuklas ng mga modelo, pag-download, pag-load) kaya't ang iyong application code ay makakapagtuon sa **data plane** (pagpapadala ng mga prompt, pagtanggap ng mga completion).

Itinuturo ng lab na ito ang buong SDK API surface sa Python, JavaScript, at C#. Sa dulo ay mauunawaan mo ang bawat pamamaraan at kung kailan gagamitin ang bawat isa.

## Mga Layunin sa Pagkatuto

Sa pagtatapos ng lab na ito ay magagawa mong:

- Ipaliwanag kung bakit mas pinipili ang SDK kumpara sa CLI para sa pagbuo ng aplikasyon
- Mag-install ng Foundry Local SDK para sa Python, JavaScript, o C#
- Gamitin ang `FoundryLocalManager` upang simulan ang serbisyo, pamahalaan ang mga modelo, at tanungin ang katalogo
- Ilahad, i-download, i-load, at i-unload ang mga modelo nang programmatic
- Siyasatin ang metadata ng modelo gamit ang `FoundryModelInfo`
- Unawain ang pagkakaiba ng katalogo, cache, at mga na-load na modelo
- Gamitin ang constructor bootstrap (Python) at `create()` + catalog pattern (JavaScript)
- Unawain ang redesign ng C# SDK at ang object-oriented na API nito

---

## Mga Kinakailangan

| Kinakailangan | Mga Detalye |
|-------------|---------|
| **Foundry Local CLI** | Na-install at nasa iyong `PATH` ([Bahagi 1](part1-getting-started.md)) |
| **Language runtime** | **Python 3.9+** at/o **Node.js 18+** at/o **.NET 9.0+** |

---

## Konsepto: SDK kontra CLI - Bakit Gamitin ang SDK?

| Aspeto | CLI (`foundry` command) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Gamit** | Pagsasaliksik, manual na pagsusuri | Integrasyon ng aplikasyon |
| **Pamamahala ng serbisyo** | Mano-mano: `foundry service start` | Awtomatik: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Pagtuklas ng port** | Basahin mula sa output ng CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Pag-download ng modelo** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Pag-handle ng error** | Mga exit code, stderr | Mga exceptions, typed na error |
| **Awtomasyon** | Shell scripts | Integrasyon sa native na wika |
| **Deployment** | Nangangailangan ng CLI sa makina ng end-user | Ang C# SDK ay maaaring self-contained (hindi kailangan ng CLI) |

> **Mahalagang pananaw:** Pinamamahalaan ng SDK ang buong lifecycle: pagsisimula ng serbisyo, pag-check ng cache, pag-download ng kulang na mga modelo, pag-load sa mga ito, at pagtuklas ng endpoint, sa ilang linya lang ng code. Hindi kailangan ng iyong aplikasyon na i-parse ang output ng CLI o pamahalaan ang mga subprocess.

---

## Mga Ehersisyo sa Lab

### Ehersisyo 1: I-install ang SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Patunayan ang pag-install:

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

Patunayan ang pag-install:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

May dalawang NuGet packages:

| Package | Platform | Paglalarawan |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Cross-platform | Gumagana sa Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Windows lang | Nagdaragdag ng WinML hardware acceleration; nagda-download at nag-iinstall ng plugin execution providers (hal. QNN para sa Qualcomm NPU) |

**Setup sa Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

I-edit ang `.csproj` file:

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

> **Tandaan:** Sa Windows, ang WinML package ay superset na kinabibilangan ng base SDK pati na ang QNN execution provider. Sa Linux/macOS, ang base SDK ang ginagamit. Pinananatili ng conditional TFM at package references na lubos na cross-platform ang proyekto.

Gumawa ng `nuget.config` sa root ng proyekto:

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

I-restore ang mga package:

```bash
dotnet restore
```

</details>

---

### Ehersisyo 2: Simulan ang Serbisyo at Ilahad ang Katalogo

Ang unang bagay na ginagawa ng anumang aplikasyon ay simulan ang Foundry Local service at alamin kung ano ang mga available na modelo.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Gumawa ng manager at simulan ang serbisyo
manager = FoundryLocalManager()
manager.start_service()

# Ilahad ang lahat ng modelo na available sa katalogo
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Mga Pamamaraan sa Pamamahala ng Serbisyo

| Pamamaraan | Pirma | Paglalarawan |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Tiyakin kung tumatakbo ang serbisyo |
| `start_service()` | `() -> None` | Simulan ang Foundry Local service |
| `service_uri` | `@property -> str` | Base service URI |
| `endpoint` | `@property -> str` | API endpoint (service URI + `/v1`) |
| `api_key` | `@property -> str` | API key (mula sa env o default na placeholder) |

#### Python SDK - Mga Pamamaraan sa Pamamahala ng Katalogo

| Pamamaraan | Pirma | Paglalarawan |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Ilahad lahat ng modelo sa katalogo |
| `refresh_catalog()` | `() -> None` | I-refresh ang katalogo mula sa serbisyo |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=false) -> FoundryModelInfo \| None` | Kunin ang impormasyon para sa isang partikular na modelo |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Gumawa ng manager at simulan ang serbisyo
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Mag-browse sa katalogo
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Mga Pamamaraan ng Manager

| Pamamaraan | Pirma | Paglalarawan |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Simulan ang SDK singleton |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Access sa singleton manager |
| `manager.startWebService()` | `() => Promise<void>` | Simulan ang Foundry Local web service |
| `manager.urls` | `string[]` | Array ng mga base URLs para sa serbisyo |

#### JavaScript SDK - Mga Pamamaraan ng Katalogo at Modelo

| Pamamaraan | Pirma | Paglalarawan |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Access sa model catalog |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Kunin ang isang model object ayon sa alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Gumagamit ang C# SDK v0.8.0+ ng object-oriented na arkitektura kasama ang `Configuration`, `Catalog`, at `Model` na mga object:

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

#### C# SDK - Mga Pangunahing Klase

| Klase | Layunin |
|-------|---------|
| `Configuration` | Itakda ang app name, log level, cache dir, web server URLs |
| `FoundryLocalManager` | Pangunahing entry point - nililikha gamit ang `CreateAsync()`, naa-access gamit ang `.Instance` |
| `Catalog` | Mag-browse, maghanap, at kumuha ng mga modelo mula sa katalogo |
| `Model` | Kumakatawan sa isang partikular na modelo - pag-download, pag-load, pagkuha ng mga kliyente |

#### C# SDK - Mga Pamamaraan ng Manager at Catalog

| Pamamaraan | Paglalarawan |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Simulan ang manager |
| `FoundryLocalManager.Instance` | Access sa singleton na manager |
| `manager.GetCatalogAsync()` | Kunin ang model catalog |
| `catalog.ListModelsAsync()` | Ilahad lahat ng available na modelo |
| `catalog.GetModelAsync(alias: "alias")` | Kunin ang partikular na modelo ayon sa alias |
| `catalog.GetCachedModelsAsync()` | Ilahad ang mga na-download na modelo |
| `catalog.GetLoadedModelsAsync()` | Ilahad ang kasalukuyang mga na-load na modelo |

> **Tala sa Arkitektura ng C#:** Ginagawa ng redesign ng C# SDK v0.8.0+ ang aplikasyon na **self-contained**; hindi nito kailangan ang Foundry Local CLI sa makina ng end-user. Ang SDK ang humahawak ng pamamahala ng modelo at inference nang native.

</details>

---

### Ehersisyo 3: Mag-download at Mag-load ng Modelo

Ipinaghiwalay ng SDK ang pag-download (sa disk) mula sa pag-load (sa memory). Pinapayagan nito na ma-pre-download ang mga modelo sa panahon ng setup at i-load ang mga iyon kapag kinakailangan.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Opsyon A: Manwal na hakbang-hakbang
manager = FoundryLocalManager()
manager.start_service()

# Suriin muna ang cache
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

# Opsyon B: Isang linya ng bootstrap (inirerekomenda)
# Ipasok ang alias sa constructor - ito ay nagsisimula ng serbisyo, nagda-download, at naglo-load nang awtomatiko
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Mga Pamamaraan sa Pamamahala ng Modelo

| Pamamaraan | Pirma | Paglalarawan |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | I-download ang modelo papunta sa lokal na cache |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | I-load ang modelo sa inference server |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | I-unload ang modelo mula sa server |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Ilahad ang lahat ng kasalukuyang na-load na mga modelo |

#### Python - Mga Pamamaraan sa Pamamahala ng Cache

| Pamamaraan | Pirma | Paglalarawan |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Kunin ang lokasyon ng cache directory |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Ilahad lahat ng na-download na mga modelo |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Hakbang-hakbang na pamamaraan
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

#### JavaScript - Mga Pamamaraan ng Modelo

| Pamamaraan | Pirma | Paglalarawan |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Kung ang modelo ay na-download na |
| `model.download()` | `() => Promise<void>` | I-download ang modelo sa lokal na cache |
| `model.load()` | `() => Promise<void>` | I-load sa inference server |
| `model.unload()` | `() => Promise<void>` | I-unload mula sa inference server |
| `model.id` | `string` | Natatanging identifier ng modelo |

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

#### C# - Mga Pamamaraan ng Modelo

| Pamamaraan | Paglalarawan |
|--------|-------------|
| `model.DownloadAsync(progress?)` | I-download ang napiling variant |
| `model.LoadAsync()` | I-load ang modelo sa memory |
| `model.UnloadAsync()` | I-unload ang modelo |
| `model.SelectVariant(variant)` | Piliin ang isang tiyak na variant (CPU/GPU/NPU) |
| `model.SelectedVariant` | Ang kasalukuyang napiling variant |
| `model.Variants` | Lahat ng available na variant para sa modelong ito |
| `model.GetPathAsync()` | Kunin ang lokal na path ng file |
| `model.GetChatClientAsync()` | Kunin ang native chat client (hindi kailangan ang OpenAI SDK) |
| `model.GetAudioClientAsync()` | Kunin ang audio client para sa transcription |

</details>

---

### Ehersisyo 4: Siyasatin ang Metadata ng Modelo

Naglalaman ang `FoundryModelInfo` object ng mayamang metadata tungkol sa bawat modelo. Ang pag-unawa sa mga patlang na ito ay tumutulong sa pagpili ng tamang modelo para sa iyong aplikasyon.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Kumuha ng detalyadong impormasyon tungkol sa isang partikular na modelo
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

#### Mga Patlang ng FoundryModelInfo

| Patlang | Uri | Paglalarawan |
|-------|------|-------------|
| `alias` | string | Maikling pangalan (hal. `phi-3.5-mini`) |
| `id` | string | Natatanging identifier ng modelo |
| `version` | string | Bersyon ng modelo |
| `task` | string | `chat-completions` o `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, o NPU |
| `execution_provider` | string | Runtime backend (CUDA, CPU, QNN, WebGPU, atbp.) |
| `file_size_mb` | int | Laki sa disk na MB |
| `supports_tool_calling` | bool | Kung sinusuportahan ng modelo ang pagtawag ng function/tool |
| `publisher` | string | Sino ang naglathala ng modelo |
| `license` | string | Pangalan ng lisensya |
| `uri` | string | URI ng modelo |
| `prompt_template` | dict/null | Prompt template, kung ano man |

---

### Ehersisyo 5: Pamahalaan ang Lifecycle ng Modelo

Sanayin ang buong lifecycle: listahan → i-download → i-load → gamitin → i-unload.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Maliit na modelo para sa mabilisang pagsubok

manager = FoundryLocalManager()
manager.start_service()

# 1. Suriin kung ano ang nasa katalogo
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Suriin kung ano ang na-download na
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Mag-download ng modelo
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Tiyakin na ito ay nasa cache na ngayon
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. I-load ito
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Suriin kung ano ang naka-load
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. I-unload ito
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

const alias = "qwen2.5-0.5b"; // Maliit na modelo para sa mabilis na pagsubok

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Kunin ang modelo mula sa katalogo
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. I-download kung kinakailangan
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. I-load ito
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. I-unload ito
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Exercise 6: The Quick-Start Patterns

Bawat wika ay nagbibigay ng shortcut para simulan ang serbisyo at i-load ang modelo sa isang tawag. Ito ang **mga inirerekomendang pattern** para sa karamihan ng mga aplikasyon.

<details>
<summary><h3>🐍 Python - Constructor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Ibigay ang alias sa constructor - siya ang bahala sa lahat:
# 1. Sinisimulan ang serbisyo kung hindi pa ito tumatakbo
# 2. Dina-download ang modelo kung hindi pa naka-cache
# 3. Ini-load ang modelo sa inference server
manager = FoundryLocalManager("phi-3.5-mini")

# Handa nang gamitin agad
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Ang `bootstrap` parameter (default `True`) ay kumokontrol sa asal na ito. Itakda ang `bootstrap=False` kung nais mong magkaroon ng manual na kontrol:

```python
# Manwal na mode - walang nangyayari nang awtomatiko
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() ang humahawak ng lahat:
// 1. Sinasimulan ang serbisyo
// 2. Kinukuha ang modelo mula sa katalogo
// 3. Dina-download kung kailangan at iniloload ang modelo
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Handa nang gamitin kaagad
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

> **Tanda sa C#:** Ginagamit ng C# SDK ang `Configuration` upang makontrol ang pangalan ng app, pag-log, mga direktoryo ng cache, at maging ang pag-pin ng partikular na web server port. Ginagawa nitong pinakamaraming configurability sa tatlong SDK.

</details>

---

### Exercise 7: The Native ChatClient (Walang Kailangan na OpenAI SDK)

Ang JavaScript at C# SDK ay nagbibigay ng `createChatClient()` method na bumabalik ng native chat client — hindi na kailangang i-install o i-configure nang hiwalay ang OpenAI SDK.

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

// Lumikha ng ChatClient direkta mula sa modelo — hindi kailangan ang OpenAI import
const chatClient = model.createChatClient();

// Ang completeChat ay nagbabalik ng tugon na bagay na tugma sa OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Ang streaming ay gumagamit ng pattern na callback
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

Sinusuportahan din ng `ChatClient` ang pagtawag ng mga tool — ipasa ang mga tool bilang pangalawang argumento:

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

> **Kailan gagamitin ang bawat pattern:**
> - **`createChatClient()`** — Mabilis na prototyping, mas kaunting dependencies, mas simple ang code
> - **OpenAI SDK** — Ganap na kontrol sa mga parameter (temperature, top_p, stop tokens, atbp.), mas angkop para sa mga production na aplikasyon

---

### Exercise 8: Mga Variant ng Modelo at Pagpili ng Hardware

Ang mga modelo ay maaaring magkaroon ng maraming **variant** na in-optimize para sa iba't ibang hardware. Awtomatikong pinipili ng SDK ang pinakamainam na variant, ngunit maaari mo ring siyasatin at piliin ito nang manual.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Ilahad ang lahat ng mga available na variant
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// Awtomatikong pinipili ng SDK ang pinakamahusay na variant para sa iyong hardware
// Upang i-override, gamitin ang selectVariant():
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

Sa Python, awtomatikong pinipili ng SDK ang pinakamainam na variant batay sa hardware. Gamitin ang `get_model_info()` upang makita kung ano ang napili:

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

#### Mga Modelo na may NPU Variants

Ang ilang mga modelo ay may mga variant na na-optimize para sa NPU (Neural Processing Units) tulad ng Qualcomm Snapdragon, Intel Core Ultra:

| Modelo | May NPU Variant |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Tip:** Sa hardware na may kakayahan sa NPU, awtomatikong pinipili ng SDK ang NPU variant kung ito ay available. Hindi mo na kailangang baguhin ang iyong code. Para sa mga C# na proyekto sa Windows, idagdag ang `Microsoft.AI.Foundry.Local.WinML` NuGet package upang paganahin ang QNN execution provider — ang QNN ay ibinibigay bilang plugin EP sa pamamagitan ng WinML.

---

### Exercise 9: Mga Pag-upgrade ng Modelo at Pag-refresh ng Catalog

Ang katalogo ng modelo ay ina-update nang pana-panahon. Gamitin ang mga pamamaraang ito upang suriin at ilapat ang mga update.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# I-refresh ang katalogo upang makuha ang pinakabagong listahan ng mga modelo
manager.refresh_catalog()

# Suriin kung ang naka-cache na modelo ay may mas bagong bersyon na magagamit
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

// I-refresh ang katalogo upang kunin ang pinakabagong listahan ng modelo
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Ilista ang lahat ng magagamit na mga modelo pagkatapos i-refresh
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Exercise 10: Paggamit ng Reasoning Models

Ang **phi-4-mini-reasoning** na modelo ay may kasamang chain-of-thought reasoning. Binabalot nito ang internal na pag-iisip sa mga tag na `<think>...</think>` bago maglabas ng pangwakas na sagot. Ito ay kapaki-pakinabang para sa mga gawain na nangangailangan ng multi-step na lohika, matematika, o paglutas ng problema.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning ay ~4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Ang modelo ay bumabalot ng pag-iisip nito sa mga tag na <think>...</think>
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

// I-parse ang chain-of-thought na pag-iisip
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Kailan gagamitin ang mga reasoning models:**
> - Mga problema sa matematika at lohika
> - Multi-step na mga gawain sa pagpaplano
> - Komplikadong pagbuo ng code
> - Mga gawain kung saan nakakatulong ang pagpapakita ng proseso upang mapabuti ang katumpakan
>
> **Trade-off:** Gumagawa ang reasoning models ng mas maraming tokens (ang seksyon na `<think>`) at mas mabagal. Para sa simpleng Q&A, mas mabilis ang isang karaniwang modelo tulad ng phi-3.5-mini.

---

### Exercise 11: Pag-unawa sa Aliases at Pagpili ng Hardware

Kapag nagbigay ka ng isang **alias** (tulad ng `phi-3.5-mini`) sa halip na buong model ID, awtomatikong pinipili ng SDK ang pinakamahusay na variant para sa iyong hardware:

| Hardware | Piniling Execution Provider |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (sa pamamagitan ng WinML plugin) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Anumang device (fallback) | `CPUExecutionProvider` o `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Ang alyas ay nagreresolba sa pinakamagandang variant para sa IYONG hardware
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tip:** Palaging gamitin ang mga aliases sa iyong application code. Kapag dineploy sa makina ng user, pipili ang SDK ng pinakaangkop na variant sa oras ng pagsasagawa — CUDA sa NVIDIA, QNN sa Qualcomm, CPU sa iba pa.

---

### Exercise 12: Mga Opsyon sa Configuration ng C#

Ang `Configuration` class ng C# SDK ay nagbibigay ng detalyadong kontrol sa runtime:

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

| Property | Default | Paglalarawan |
|----------|---------|-------------|
| `AppName` | (kailangan) | Pangalan ng iyong aplikasyon |
| `LogLevel` | `Information` | Antas ng detalye ng logging |
| `Web.Urls` | (dynamic) | Ipinapako ang isang partikular na port para sa web server |
| `AppDataDir` | Default ng OS | Base na direktoryo para sa app data |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Kung saan iniimbak ang mga modelo |
| `LogsDir` | `{AppDataDir}/logs` | Kung saan isinusulat ang mga log |

---

### Exercise 13: Paggamit sa Browser (JavaScript Lang)

Isinasama ng JavaScript SDK ang isang bersyon na compatible sa browser. Sa browser, kailangang mano-manong simulan ang serbisyo sa CLI at tukuyin ang host URL:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Simulan muna ang serbisyo nang manu-mano:
//   foundry service start
// Pagkatapos gamitin ang URL mula sa output ng CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// I-browse ang katalogo
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Mga limitasyon sa browser:** Ang bersyon sa browser ay **hindi** sumusuporta sa `startWebService()`. Kailangan mong tiyakin na ang Foundry Local service ay tumatakbo na bago gamitin ang SDK sa browser.

---

## Kumpletong API Reference

### Python

| Kategorya | Metodo | Paglalarawan |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Gumawa ng manager; opsyonal na i-bootstrap gamit ang modelo |
| **Service** | `is_service_running()` | Suriin kung tumatakbo ang serbisyo |
| **Service** | `start_service()` | Simulan ang serbisyo |
| **Service** | `endpoint` | URL ng API endpoint |
| **Service** | `api_key` | API key |
| **Catalog** | `list_catalog_models()` | Ilan ang lahat ng available na modelo |
| **Catalog** | `refresh_catalog()` | I-refresh ang katalogo |
| **Catalog** | `get_model_info(alias_or_model_id)` | Kunin ang metadata ng modelo |
| **Cache** | `get_cache_location()` | Path ng direktoryo ng cache |
| **Cache** | `list_cached_models()` | Ilan ang mga downloaded na modelo |
| **Model** | `download_model(alias_or_model_id)` | I-download ang modelo |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Ilagay ang modelo sa memorya |
| **Model** | `unload_model(alias_or_model_id)` | Alisin ang modelo mula sa memorya |
| **Model** | `list_loaded_models()` | Ilan ang mga loaded na modelo |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Suriin kung may bagong bersyon |
| **Model** | `upgrade_model(alias_or_model_id)` | I-upgrade ang modelo sa pinakabagong bersyon |
| **Service** | `httpx_client` | Pre-configured HTTPX client para diretso sa API tawag |

### JavaScript

| Kategorya | Metodo | Paglalarawan |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | I-initialize ang singleton ng SDK |
| **Init** | `FoundryLocalManager.instance` | Ma-access ang singleton manager |
| **Service** | `manager.startWebService()` | Simulan ang web service |
| **Service** | `manager.urls` | Array ng base URLs para sa serbisyo |
| **Catalog** | `manager.catalog` | I-access ang model catalog |
| **Catalog** | `catalog.getModel(alias)` | Kunin ang isang modelo ayon sa alias (nagbabalik ng Promise) |
| **Model** | `model.isCached` | Kung na-download na ang modelo |
| **Model** | `model.download()` | I-download ang modelo |
| **Model** | `model.load()` | I-load ang modelo |
| **Model** | `model.unload()` | I-unload ang modelo |
| **Model** | `model.id` | Natatanging identifier ng modelo |
| **Model** | `model.alias` | Alias ng modelo |
| **Model** | `model.createChatClient()` | Kumuha ng native chat client (walang kailangang OpenAI SDK) |
| **Model** | `model.createAudioClient()` | Kumuha ng audio client para sa transcription |
| **Model** | `model.removeFromCache()` | Alisin ang modelo mula sa lokal na cache |
| **Model** | `model.selectVariant(variant)` | Piliin ang partikular na hardware variant |
| **Model** | `model.variants` | Array ng mga available na variant para sa modelong ito |
| **Model** | `model.isLoaded()` | Suriin kung ang modelo ay naka-load na |
| **Catalog** | `catalog.getModels()` | Ilan ang lahat ng available na modelo |
| **Catalog** | `catalog.getCachedModels()` | Listahan ng mga na-download na modelo |
| **Catalog** | `catalog.getLoadedModels()` | Listahan ng mga kasalukuyang naka-load na modelo |
| **Catalog** | `catalog.updateModels()` | I-refresh ang katalogo mula sa serbisyo |
| **Service** | `manager.stopWebService()` | Itigil ang Foundry Local web service |

### C# (v0.8.0+)

| Kategorya | Metodo | Paglalarawan |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | I-initialize ang manager |
| **Init** | `FoundryLocalManager.Instance` | Ma-access ang singleton |
| **Catalog** | `manager.GetCatalogAsync()` | Kunin ang katalogo |
| **Catalog** | `catalog.ListModelsAsync()` | Listahan ng lahat ng modelo |
| **Catalog** | `catalog.GetModelAsync(alias)` | Kunin ang partikular na modelo |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Listahan ng mga cached na modelo |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Listahan ng mga loaded na modelo |
| **Model** | `model.DownloadAsync(progress?)` | I-download ang modelo |
| **Model** | `model.LoadAsync()` | I-load ang modelo |
| **Model** | `model.UnloadAsync()` | I-unload ang modelo |
| **Model** | `model.SelectVariant(variant)` | Piliin ang hardware variant |
| **Model** | `model.GetChatClientAsync()` | Kumuha ng native chat client |
| **Model** | `model.GetAudioClientAsync()` | Kumuha ng audio transcription client |
| **Model** | `model.GetPathAsync()` | Kumuha ng lokal na path ng file |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Kunin ang partikular na hardware variant |
| **Catalog** | `catalog.UpdateModelsAsync()` | I-refresh ang katalogo |
| **Server** | `manager.StartWebServerAsync()` | Simulan ang REST web server |
| **Server** | `manager.StopWebServerAsync()` | Itigil ang REST web server |
| **Config** | `config.ModelCacheDir` | Direktoryo ng cache |

---

## Mahahalagang Punto

| Konsepto | Iyong Natutunan |
|---------|-----------------|
| **SDK vs CLI** | Nagbibigay ang SDK ng programmatic kontrol - mahalaga para sa mga aplikasyon |
| **Control plane** | Pinamamahalaan ng SDK ang mga serbisyo, modelo, at caching |
| **Dynamic ports** | Palaging gamitin ang `manager.endpoint` (Python) o `manager.urls[0]` (JS/C#) - huwag hardcode ang port |
| **Aliases** | Gumamit ng mga aliases para sa awtomatikong pagpili ng hardware-optimal na modelo |
| **Mabilis na pagsisimula** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Pag-redesign sa C#** | v0.8.0+ ay nakapag-iisa - walang kailangan na CLI sa mga end-user na makina |
| **Buhay ng modelo** | Catalog → I-download → I-load → Gamitin → I-unload |
| **FoundryModelInfo** | Masaganang metadata: gawain, device, sukat, lisensya, suporta sa pagtawag ng tool |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) para sa OpenAI-free na paggamit |
| **Mga variant** | Ang mga modelo ay may hardware-specific na variant (CPU, GPU, NPU); awtomatikong pinipili |
| **Mga upgrade** | Python: `is_model_upgradeable()` + `upgrade_model()` para panatilihing bago ang mga modelo |
| **Pag-refresh ng catalog** | `refresh_catalog()` (Python) / `updateModels()` (JS) para matuklasan ang mga bagong modelo |

---

## Mga Mapagkukunan

| Mapagkukunan | Link |
|----------|------|
| SDK Reference (lahat ng wika) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Pagsasama gamit ang inference SDKs | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Sanggunian ng API ng C# SDK | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| Mga Halimbawa ng C# SDK | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Website ng Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Mga Susunod na Hakbang

Magpatuloy sa [Part 3: Using the SDK with OpenAI](part3-sdk-and-apis.md) upang ikonekta ang SDK sa OpenAI client library at bumuo ng iyong unang aplikasyon ng chat completion.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Paunawa**:  
Ang dokumentong ito ay isinalin gamit ang AI translation service na [Co-op Translator](https://github.com/Azure/co-op-translator). Bagaman nagsusumikap kami para sa katumpakan, mangyaring tandaan na ang mga awtomatikong pagsasalin ay maaaring maglaman ng mga pagkakamali o kawastuhan. Ang orihinal na dokumento sa orihinal nitong wika ang dapat ituring na pangunahing sanggunian. Para sa mga mahahalagang impormasyon, inirerekomenda ang propesyonal na pagsasalin ng tao. Hindi kami mananagot sa anumang hindi pagkakaunawaan o maling interpretasyon na maaaring magmula sa paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->