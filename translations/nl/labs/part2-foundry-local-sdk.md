![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Deel 2: Foundry Local SDK Diepgaande Verkenning

> **Doel:** Beheers de Foundry Local SDK om modellen, services en caching programmatisch te beheren - en begrijp waarom de SDK de aanbevolen aanpak is boven de CLI voor het bouwen van applicaties.

## Overzicht

In Deel 1 heb je de **Foundry Local CLI** gebruikt om modellen interactief te downloaden en uit te voeren. De CLI is geweldig voor verkenning, maar wanneer je echte applicaties bouwt, heb je **programmatische controle** nodig. De Foundry Local SDK geeft je dat - het beheert het **control plane** (het starten van de service, het ontdekken van modellen, downloaden, laden) zodat je applicatiecode zich kan richten op het **data plane** (prompts verzenden, reacties ontvangen).

Deze lab leert je de volledige SDK API-surface over Python, JavaScript en C#. Aan het einde begrijp je elke beschikbare methode en wanneer je deze moet gebruiken.

## Leerdoelen

Aan het einde van dit lab kun je:

- Uitleggen waarom de SDK de voorkeur heeft boven de CLI voor applicatieontwikkeling
- De Foundry Local SDK installeren voor Python, JavaScript of C#
- `FoundryLocalManager` gebruiken om de service te starten, modellen te beheren en de catalogus te raadplegen
- Modellen programmatisch weergeven, downloaden, laden en ontladen
- Modelmetadata inspecteren met `FoundryModelInfo`
- Het verschil begrijpen tussen catalogus-, cache- en geladen modellen
- De constructor bootstrap (Python) en `create()` + cataloguspatroon (JavaScript) gebruiken
- Het herontwerp van de C# SDK en de objectgeoriënteerde API begrijpen

---

## Vereisten

| Vereiste | Details |
|-------------|---------|
| **Foundry Local CLI** | Geïnstalleerd en op je `PATH` ([Deel 1](part1-getting-started.md)) |
| **Taalruntime** | **Python 3.9+** en/of **Node.js 18+** en/of **.NET 9.0+** |

---

## Concept: SDK vs CLI - Waarom de SDK gebruiken?

| Aspect | CLI (`foundry` commando) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Gebruik** | Verkenning, handmatige testen | Applicatie-integratie |
| **Servicebeheer** | Handmatig: `foundry service start` | Automatisch: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Poortontdekking** | Lezen uit CLI-uitvoer | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Model downloaden** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Foutafhandeling** | Exit-codes, stderr | Exceptions, getypeerde fouten |
| **Automatisering** | Shellscripts | Native taalintegratie |
| **Deployment** | Vereist CLI op eindgebruikersmachine | C# SDK kan self-contained zijn (geen CLI nodig) |

> **Belangrijk inzicht:** De SDK beheert de volledige levenscyclus: starten van de service, cache controleren, ontbrekende modellen downloaden, laden en de endpoint ontdekken, in slechts een paar regels code. Je applicatie hoeft CLI-uitvoer niet te parseren of subprocessen te beheren.

---

## Lab Oefeningen

### Oefening 1: Installeer de SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Controleer de installatie:

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

Controleer de installatie:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Er zijn twee NuGet-pakketten:

| Pakket | Platform | Beschrijving |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Cross-platform | Werkt op Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Alleen Windows | Voegt WinML hardwareversnelling toe; downloadt en installeert plugin execution providers (bijv. QNN voor Qualcomm NPU) |

**Windows setup:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Bewerk het `.csproj`-bestand:

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

> **Opmerking:** Op Windows is het WinML-pakket een superset die de basis SDK plus de QNN execution provider bevat. Op Linux/macOS wordt de basis SDK gebruikt. De conditionele TFM en pakketreferenties houden het project volledig cross-platform.

Maak een `nuget.config` aan in de projectroot:

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

Herstel pakketten:

```bash
dotnet restore
```

</details>

---

### Oefening 2: Start de Service en Bekijk de Catalogus

Het eerste wat elke applicatie doet is het starten van de Foundry Local service en ontdekken welke modellen beschikbaar zijn.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Maak een manager aan en start de service
manager = FoundryLocalManager()
manager.start_service()

# Toon alle modellen die beschikbaar zijn in de catalogus
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Service Beheermethoden

| Methode | Handtekening | Beschrijving |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Controleer of de service draait |
| `start_service()` | `() -> None` | Start de Foundry Local service |
| `service_uri` | `@property -> str` | De basis URI van de service |
| `endpoint` | `@property -> str` | De API endpoint (service URI + `/v1`) |
| `api_key` | `@property -> str` | API-sleutel (van omgevingsvariabele of standaard placeholder) |

#### Python SDK - Catalogus Beheermethoden

| Methode | Handtekening | Beschrijving |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Lijst van alle modellen in de catalogus |
| `refresh_catalog()` | `() -> None` | Vernieuw de catalogus vanuit de service |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Haal informatie op voor een specifiek model |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Maak een beheerder aan en start de dienst
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Blader door de catalogus
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Manager Methoden

| Methode | Handtekening | Beschrijving |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Initialiseer de SDK singleton |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Toegang tot de singleton manager |
| `manager.startWebService()` | `() => Promise<void>` | Start de Foundry Local webservice |
| `manager.urls` | `string[]` | Array van basis-URL's voor de service |

#### JavaScript SDK - Catalogus en Model Methoden

| Methode | Handtekening | Beschrijving |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Toegang tot de modelcatalogus |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Haal een model op via alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

De C# SDK v0.8.0+ gebruikt een objectgeoriënteerde architectuur met `Configuration`, `Catalog` en `Model` objecten:

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

#### C# SDK - Belangrijke Klassen

| Klasse | Doel |
|-------|---------|
| `Configuration` | Stel app-naam, logniveau, cachemap, webserver-URLs in |
| `FoundryLocalManager` | Hoofd toegangspunt - gemaakt via `CreateAsync()`, toegang via `.Instance` |
| `Catalog` | Blader, zoek en haal modellen op uit de catalogus |
| `Model` | Vertegenwoordigt een specifiek model - downloaden, laden, clients krijgen |

#### C# SDK - Manager en Catalogus Methoden

| Methode | Beschrijving |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Initialiseer de manager |
| `FoundryLocalManager.Instance` | Toegang tot de singleton manager |
| `manager.GetCatalogAsync()` | Haal de modelcatalogus op |
| `catalog.ListModelsAsync()` | Lijst van alle beschikbare modellen |
| `catalog.GetModelAsync(alias: "alias")` | Haal een specifiek model op via alias |
| `catalog.GetCachedModelsAsync()` | Lijst van gedownloade modellen |
| `catalog.GetLoadedModelsAsync()` | Lijst van momenteel geladen modellen |

> **C# Architectuur Opmerking:** De C# SDK v0.8.0+ is herontworpen om de applicatie **self-contained** te maken; het vereist de Foundry Local CLI niet op de eindgebruikersmachine. De SDK beheert modelbeheer en inferentie natively.

</details>

---

### Oefening 3: Download en Laad een Model

De SDK scheidt downloaden (naar schijf) van laden (in geheugen). Dit laat je modellen vooraf downloaden tijdens de setup en ze on-demand laden.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Optie A: Handmatige stap-voor-stap
manager = FoundryLocalManager()
manager.start_service()

# Controleer eerst de cache
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

# Optie B: Eénregelige bootstrap (aanbevolen)
# Geef alias door aan de constructor - het start de service, downloadt en laadt automatisch
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Model Beheermethoden

| Methode | Handtekening | Beschrijving |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Download een model naar de lokale cache |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Laad een model in de inferentieserver |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Ontlaad een model van de server |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Lijst van alle momenteel geladen modellen |

#### Python - Cache Beheermethoden

| Methode | Handtekening | Beschrijving |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Haal het pad van de cache directory |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Lijst van alle gedownloade modellen |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Stapsgewijze benadering
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

#### JavaScript - Model Methoden

| Methode | Handtekening | Beschrijving |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Of het model al is gedownload |
| `model.download()` | `() => Promise<void>` | Download het model naar lokale cache |
| `model.load()` | `() => Promise<void>` | Laad in inferentieserver |
| `model.unload()` | `() => Promise<void>` | Ontlaad van inferentieserver |
| `model.id` | `string` | De unieke modelidentificator |

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

#### C# - Model Methoden

| Methode | Beschrijving |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Download de geselecteerde variant |
| `model.LoadAsync()` | Laad het model in het geheugen |
| `model.UnloadAsync()` | Ontlaad het model |
| `model.SelectVariant(variant)` | Selecteer een specifieke variant (CPU/GPU/NPU) |
| `model.SelectedVariant` | De momenteel geselecteerde variant |
| `model.Variants` | Alle beschikbare varianten voor dit model |
| `model.GetPathAsync()` | Krijg het lokale bestandspad |
| `model.GetChatClientAsync()` | Krijg een native chatclient (geen OpenAI SDK nodig) |
| `model.GetAudioClientAsync()` | Krijg een audio-client voor transcriptie |

</details>

---

### Oefening 4: Inspecteer Model Metadata

Het `FoundryModelInfo` object bevat rijke metadata over elk model. Begrip van deze velden helpt je het juiste model voor je applicatie te kiezen.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Verkrijg gedetailleerde informatie over een specifiek model
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

#### FoundryModelInfo Velden

| Veld | Type | Beschrijving |
|-------|------|-------------|
| `alias` | string | Korte naam (bijv. `phi-3.5-mini`) |
| `id` | string | Unieke modelidentificator |
| `version` | string | Modelversie |
| `task` | string | `chat-completions` of `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, of NPU |
| `execution_provider` | string | Runtime backend (CUDA, CPU, QNN, WebGPU, enz.) |
| `file_size_mb` | int | Grootte op schijf in MB |
| `supports_tool_calling` | bool | Of het model functie-/tool-aanroepen ondersteunt |
| `publisher` | string | Wie het model heeft gepubliceerd |
| `license` | string | Licentienaam |
| `uri` | string | Model URI |
| `prompt_template` | dict/null | Prompt template, indien aanwezig |

---

### Oefening 5: Beheer de Model Levenscyclus

Oefen de volledige levenscyclus: lijst → downloaden → laden → gebruiken → ontladen.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Klein model voor snelle tests

manager = FoundryLocalManager()
manager.start_service()

# 1. Controleer wat er in de catalogus staat
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Controleer wat er al is gedownload
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Download een model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Controleer of het nu in de cache staat
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Laad het
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Controleer wat geladen is
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Ontlaad het
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

const alias = "qwen2.5-0.5b"; // Klein model voor snelle testen

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Haal model uit catalogus
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Download indien nodig
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Laad het
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Ontlaad het
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Oefening 6: De Quick-Start Patronen

Elke taal biedt een snelkoppeling om de service te starten en een model te laden in één oproep. Dit zijn de **aanbevolen patronen** voor de meeste toepassingen.

<details>
<summary><h3>🐍 Python - Constructor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Geef een alias door aan de constructor - het regelt alles:
# 1. Start de service als deze niet draait
# 2. Downloadt het model als het niet in de cache staat
# 3. Laadt het model in de inferentieserver
manager = FoundryLocalManager("phi-3.5-mini")

# Direct klaar voor gebruik
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

De `bootstrap` parameter (standaard `True`) bepaalt dit gedrag. Stel `bootstrap=False` in als je handmatige controle wilt:

```python
# Handmatige modus - er gebeurt niets automatisch
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catalogus</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() regelt alles:
// 1. Start de dienst
// 2. Haalt het model uit de catalogus
// 3. Downloadt indien nodig en laadt het model
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Direct klaar voor gebruik
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Catalogus</h3></summary>

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

> **C# Opmerking:** De C# SDK gebruikt `Configuration` om de app-naam, logging, cache-mappen en zelfs een specifieke webserverpoort vast te leggen. Dit maakt het de meest configureerbare van de drie SDK's.

</details>

---

### Oefening 7: De Native ChatClient (Geen OpenAI SDK Nodig)

De JavaScript en C# SDK's bieden een `createChatClient()` gemaksmethode die een native chatclient retourneert — je hoeft de OpenAI SDK niet apart te installeren of configureren.

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

// Maak een ChatClient direct van het model — geen OpenAI-import nodig
const chatClient = model.createChatClient();

// completeChat geeft een OpenAI-compatibel antwoordobject terug
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming gebruikt een callback-patroon
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

De `ChatClient` ondersteunt ook het aanroepen van tools — geef tools door als het tweede argument:

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

> **Wanneer welk patroon te gebruiken:**
> - **`createChatClient()`** — Snelle prototyping, minder afhankelijkheden, eenvoudiger code
> - **OpenAI SDK** — Volledige controle over parameters (temperatuur, top_p, stop tokens, enz.), beter voor productieapplicaties

---

### Oefening 8: Modelvarianten en Hardware Selectie

Modellen kunnen meerdere **varianten** hebben die geoptimaliseerd zijn voor verschillende hardware. De SDK selecteert automatisch de beste variant, maar je kunt ook handmatig inspecteren en kiezen.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Lijst alle beschikbare varianten
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// De SDK selecteert automatisch de beste variant voor je hardware
// Om te overschrijven, gebruik selectVariant():
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

In Python selecteert de SDK automatisch de beste variant op basis van hardware. Gebruik `get_model_info()` om te zien wat geselecteerd is:

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

#### Modellen met NPU-varianten

Sommige modellen hebben NPU-geoptimaliseerde varianten voor apparaten met Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Model | NPU-variant beschikbaar |
|-------|:-----------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Tip:** Op NPU-compatibele hardware kiest de SDK automatisch de NPU-variant indien beschikbaar. Je hoeft je code niet aan te passen. Voor C# projecten op Windows voeg je het `Microsoft.AI.Foundry.Local.WinML` NuGet-pakket toe om de QNN-executieprovider te activeren — QNN wordt geleverd als een plugin EP via WinML.

---

### Oefening 9: Modelupdates en Catalogus Vernieuwing

De modelcatalogus wordt periodiek bijgewerkt. Gebruik deze methoden om te controleren op en toe te passen van updates.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Vernieuw de catalogus om de nieuwste modellijst te krijgen
manager.refresh_catalog()

# Controleer of er een nieuwere versie beschikbaar is van een gecached model
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

// Vernieuw de catalogus om de nieuwste modellenlijst op te halen
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Toon alle beschikbare modellen na verversen
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Oefening 10: Werken met Redeneringsmodellen

Het **phi-4-mini-reasoning** model bevat keten-van-denken redenering. Het wikkelt zijn interne gedachtegang in `<think>...</think>` tags voordat het zijn definitieve antwoord produceert. Dit is nuttig voor taken die meerstapslogica, wiskunde of probleemoplossing vereisen.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-redeneren is ~4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Het model wikkelt zijn denken in <think>...</think> tags
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

// Parseer keten-van-denken gedachten
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Wanneer redeneringsmodellen te gebruiken:**
> - Wiskunde en logica problemen
> - Taken met meerstapsplanning
> - Complexe codegeneratie
> - Taken waarbij het tonen van tussenstappen de nauwkeurigheid verhoogt
>
> **Afweging:** Redeneringsmodellen produceren meer tokens (de `<think>` sectie) en zijn trager. Voor eenvoudige Q&A is een standaardmodel zoals phi-3.5-mini sneller.

---

### Oefening 11: Begrip van Aliassen en Hardware Selectie

Wanneer je een **alias** (zoals `phi-3.5-mini`) gebruikt in plaats van een volledig model-ID, selecteert de SDK automatisch de beste variant voor jouw hardware:

| Hardware | Geselecteerde uitvoeringsprovider |
|----------|----------------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (via WinML-plugin) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Elk apparaat (fallback) | `CPUExecutionProvider` of `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Het alias verwijst naar de beste variant voor JOUW hardware
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tip:** Gebruik altijd aliassen in je applicatiecode. Wanneer je uitrolt naar de machine van een gebruiker, kiest de SDK tijdens runtime de optimale variant - CUDA op NVIDIA, QNN op Qualcomm, CPU elders.

---

### Oefening 12: C# Configuratieopties

De `Configuration` klasse van de C# SDK biedt fijne controle over de runtime:

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

| Eigenschap | Standaard | Beschrijving |
|------------|-----------|--------------|
| `AppName` | (vereist) | De naam van je applicatie |
| `LogLevel` | `Information` | Logging detailniveau |
| `Web.Urls` | (dynamisch) | Koppel een specifieke poort voor de webserver |
| `AppDataDir` | OS-standaard | Basismap voor app-gegevens |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Locatie waar modellen worden opgeslagen |
| `LogsDir` | `{AppDataDir}/logs` | Locatie waar logs worden geschreven |

---

### Oefening 13: Gebruik in de Browser (Alleen JavaScript)

De JavaScript SDK bevat een browsercompatibele versie. In de browser moet je de service handmatig starten via CLI en de host-URL opgeven:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Start de service eerst handmatig:
//   foundry service start
// Gebruik dan de URL uit de CLI-uitvoer
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Blader door de catalogus
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Beperkingen in de browser:** De browserversie ondersteunt **niet** `startWebService()`. Je moet ervoor zorgen dat de Foundry Local service al draait voordat je de SDK in een browser gebruikt.

---

## Complete API Referentie

### Python

| Categorie | Methode | Beschrijving |
|-----------|---------|--------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Manager aanmaken; optioneel bootstrap met een model |
| **Service** | `is_service_running()` | Controleren of de service draait |
| **Service** | `start_service()` | Start de service |
| **Service** | `endpoint` | API eindpunt URL |
| **Service** | `api_key` | API sleutel |
| **Catalogus** | `list_catalog_models()` | Alle beschikbare modellen opsommen |
| **Catalogus** | `refresh_catalog()` | Vernieuwen van de catalogus |
| **Catalogus** | `get_model_info(alias_or_model_id)` | Model metadata ophalen |
| **Cache** | `get_cache_location()` | Cache map pad |
| **Cache** | `list_cached_models()` | Gedownloade modellen opsommen |
| **Model** | `download_model(alias_or_model_id)` | Een model downloaden |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Een model laden |
| **Model** | `unload_model(alias_or_model_id)` | Een model uitladen |
| **Model** | `list_loaded_models()` | Opgeslagen modellen opsommen |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Controleren of een nieuwere versie beschikbaar is |
| **Model** | `upgrade_model(alias_or_model_id)` | Een model upgraden naar de nieuwste versie |
| **Service** | `httpx_client` | Vooraf geconfigureerde HTTPX client voor directe API-aanroepen |

### JavaScript

| Categorie | Methode | Beschrijving |
|-----------|---------|--------------|
| **Init** | `FoundryLocalManager.create(options)` | Initialiseer de SDK singleton |
| **Init** | `FoundryLocalManager.instance` | Toegang tot de singleton manager |
| **Service** | `manager.startWebService()` | Start de webservice |
| **Service** | `manager.urls` | Array van basis-URL's voor de service |
| **Catalogus** | `manager.catalog` | Toegang tot de modelcatalogus |
| **Catalogus** | `catalog.getModel(alias)` | Krijg een modelobject via alias (retourneert Promise) |
| **Model** | `model.isCached` | Of het model is gedownload |
| **Model** | `model.download()` | Download het model |
| **Model** | `model.load()` | Laad het model |
| **Model** | `model.unload()` | Laad het model uit |
| **Model** | `model.id` | Het unieke model-ID |
| **Model** | `model.alias` | De alias van het model |
| **Model** | `model.createChatClient()` | Krijg een native chatclient (geen OpenAI SDK nodig) |
| **Model** | `model.createAudioClient()` | Krijg een audioclient voor transcriptie |
| **Model** | `model.removeFromCache()` | Verwijder het model uit de lokale cache |
| **Model** | `model.selectVariant(variant)` | Selecteer een specifieke hardwarevariant |
| **Model** | `model.variants` | Array van beschikbare varianten voor dit model |
| **Model** | `model.isLoaded()` | Controleren of het model geladen is |
| **Catalogus** | `catalog.getModels()` | Alle beschikbare modellen opsommen |
| **Catalogus** | `catalog.getCachedModels()` | Gedownloade modellen opsommen |
| **Catalogus** | `catalog.getLoadedModels()` | Momenteel geladen modellen opsommen |
| **Catalogus** | `catalog.updateModels()` | Vernieuw de catalogus vanaf de service |
| **Service** | `manager.stopWebService()` | Stop de Foundry Local webservice |

### C# (v0.8.0+)

| Categorie | Methode | Beschrijving |
|-----------|---------|--------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Initialiseer de manager |
| **Init** | `FoundryLocalManager.Instance` | Toegang tot de singleton |
| **Catalogus** | `manager.GetCatalogAsync()` | Krijg catalogus |
| **Catalogus** | `catalog.ListModelsAsync()` | Alle modellen opsommen |
| **Catalogus** | `catalog.GetModelAsync(alias)` | Specifiek model ophalen |
| **Catalogus** | `catalog.GetCachedModelsAsync()` | Gedownloade modellen opsommen |
| **Catalogus** | `catalog.GetLoadedModelsAsync()` | Geladen modellen opsommen |
| **Model** | `model.DownloadAsync(progress?)` | Een model downloaden |
| **Model** | `model.LoadAsync()` | Een model laden |
| **Model** | `model.UnloadAsync()` | Een model uitladen |
| **Model** | `model.SelectVariant(variant)` | Kies een hardwarevariant |
| **Model** | `model.GetChatClientAsync()` | Krijg native chatclient |
| **Model** | `model.GetAudioClientAsync()` | Krijg audiotranscriptieclient |
| **Model** | `model.GetPathAsync()` | Lokale bestandslocatie ophalen |
| **Catalogus** | `catalog.GetModelVariantAsync(alias, variant)` | Krijg een specifieke hardwarevariant |
| **Catalogus** | `catalog.UpdateModelsAsync()` | Vernieuw de catalogus |
| **Server** | `manager.StartWebServerAsync()` | Start de REST webserver |
| **Server** | `manager.StopWebServerAsync()` | Stop de REST webserver |
| **Config** | `config.ModelCacheDir` | Cache map |

---

## Belangrijkste Leerpunten

| Concept | Wat Je Hebt Geleerd |
|---------|---------------------|
| **SDK vs CLI** | De SDK biedt programmatologische controle - essentieel voor applicaties |
| **Control plane** | De SDK beheert diensten, modellen en caching |
| **Dynamische poorten** | Gebruik altijd `manager.endpoint` (Python) of `manager.urls[0]` (JS/C#) - nooit een vaste poort |
| **Aliassen** | Gebruik aliassen voor automatische selectie van hardware-optimale modellen |
| **Snel starten** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# herontwerp** | v0.8.0+ is zelfstandig - geen CLI nodig op eindgebruikersmachines |
| **Model levenscyclus** | Catalogus → Downloaden → Laden → Gebruiken → Ontladen |
| **FoundryModelInfo** | Rijke metadata: taak, apparaat, grootte, licentie, ondersteuning voor oproepen door tool |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) voor OpenAI-vrij gebruik |
| **Varianten** | Modellen hebben hardware-specifieke varianten (CPU, GPU, NPU); automatisch geselecteerd |
| **Updates** | Python: `is_model_upgradeable()` + `upgrade_model()` om modellen actueel te houden |
| **Catalogus verversen** | `refresh_catalog()` (Python) / `updateModels()` (JS) om nieuwe modellen te ontdekken |

---

## Bronnen

| Bron | Link |
|----------|------|
| SDK Referentie (alle talen) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integratie met inference SDK's | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API Referentie | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK Voorbeelden | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local website | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Volgende stappen

Ga verder met [Deel 3: De SDK gebruiken met OpenAI](part3-sdk-and-apis.md) om de SDK te verbinden met de OpenAI-clientbibliotheek en je eerste chat-completieapplicatie te bouwen.