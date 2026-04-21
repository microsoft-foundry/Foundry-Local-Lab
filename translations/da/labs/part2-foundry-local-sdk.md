![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 2: Foundry Local SDK Dybt Dyk

> **Mål:** Mestre Foundry Local SDK til at administrere modeller, tjenester og caching programmatisk - og forstå hvorfor SDK’en er den anbefalede metode frem for CLI til at bygge applikationer.

## Oversigt

I Del 1 brugte du **Foundry Local CLI** til at downloade og køre modeller interaktivt. CLI er fantastisk til udforskning, men når du bygger rigtige applikationer, har du brug for **programmatisk styring**. Foundry Local SDK giver dig det - det håndterer **kontrolplanet** (starter tjenesten, opdager modeller, downloader, loader) så din applikationskode kan fokusere på **dataplanet** (afsender prompts, modtager færdiggørelser).

Dette kursus lærer dig hele SDK’ens API-flade på tværs af Python, JavaScript og C#. Når du er færdig, forstår du hver metode tilgængelig og hvornår du skal bruge den.

## Læringsmål

Når du er færdig med dette kursus, kan du:

- Forklare hvorfor SDK er foretrukket frem for CLI til applikationsudvikling
- Installere Foundry Local SDK til Python, JavaScript eller C#
- Bruge `FoundryLocalManager` til at starte tjenesten, administrere modeller og forespørge kataloget
- Liste, downloade, loade og afloade modeller programmatisk
- Inspicere modelmetadata med `FoundryModelInfo`
- Forstå forskellen mellem katalog, cache og loadede modeller
- Bruge konstruktør-bootstrap (Python) og `create()` + katalog-mønsteret (JavaScript)
- Forstå C# SDK-redesignet og dets objektorienterede API

---

## Forudsætninger

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Installeret og på din `PATH` ([Del 1](part1-getting-started.md)) |
| **Sprogruntime** | **Python 3.9+** og/eller **Node.js 18+** og/eller **.NET 9.0+** |

---

## Koncept: SDK vs CLI - Hvorfor Bruge SDK?

| Aspekt | CLI (`foundry` kommando) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Brugsscenarie** | Udforskning, manuel test | Applikationsintegration |
| **Tjeneste administration** | Manuel: `foundry service start` | Automatisk: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Port-opdagelse** | Læs fra CLI-output | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Model-download** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Fejlhåndtering** | Exit-koder, stderr | Undtagelser, typede fejl |
| **Automatisering** | Shell-scripts | Indbygget sprogintegration |
| **Deployment** | Kræver CLI på slutbrugermaskine | C# SDK kan være selvstændig (ingen CLI nødvendig) |

> **Nøgleindsigt:** SDK håndterer hele livscyklussen: starter tjenesten, tjekker cachen, downloader manglende modeller, loader dem og opdager endpoint, alt sammen med få linjer kode. Din applikation behøver ikke at parse CLI-output eller styre subprocesser.

---

## Lab Øvelser

### Øvelse 1: Installer SDK’en

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Bekræft installationen:

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

Bekræft installationen:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Der er to NuGet-pakker:

| Pakke | Platform | Beskrivelse |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Tværplatform | Fungerer på Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Kun Windows | Tilføjer WinML hardwareacceleration; downloader og installerer plugin execution providers (f.eks. QNN til Qualcomm NPU) |

**Windows opsætning:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Rediger `.csproj`-filen:

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

> **Bemærk:** På Windows er WinML-pakken et superset, der inkluderer grundlæggende SDK plus QNN execution provider. På Linux/macOS bruges grundlæggende SDK i stedet. De betingede TFM og pakkehenvisninger holder projektet fuldt tværplatform.

Opret en `nuget.config` i projektrod:

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

Gendan pakker:

```bash
dotnet restore
```

</details>

---

### Øvelse 2: Start tjenesten og list kataloget

Det første enhver applikation gør, er at starte Foundry Local tjenesten og opdage hvilke modeller der er tilgængelige.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Opret en manager og start tjenesten
manager = FoundryLocalManager()
manager.start_service()

# List alle modeller tilgængelige i kataloget
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Tjeneste Management Metoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Tjek om tjenesten kører |
| `start_service()` | `() -> None` | Start Foundry Local tjenesten |
| `service_uri` | `@property -> str` | Basis tjeneste-URI |
| `endpoint` | `@property -> str` | API-endpoint (service URI + `/v1`) |
| `api_key` | `@property -> str` | API-nøgle (fra miljø eller standardpladsholder) |

#### Python SDK - Katalog Management Metoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | List alle modeller i kataloget |
| `refresh_catalog()` | `() -> None` | Opdater kataloget fra tjenesten |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Hent info om en specifik model |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Opret en manager og start tjenesten
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Gennemse kataloget
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Manager Metoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Initialiser SDK singleton |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Tilgå singleton manageren |
| `manager.startWebService()` | `() => Promise<void>` | Start Foundry Local webtjenesten |
| `manager.urls` | `string[]` | Array af basis URLs til tjenesten |

#### JavaScript SDK - Katalog og Model Metoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Tilgå modelkataloget |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Hent en modelobjekt via alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ bruger et objektorienteret design med `Configuration`, `Catalog` og `Model` objekter:

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

#### C# SDK - Centrale Klasser

| Klasse | Formål |
|-------|---------|
| `Configuration` | Sæt appnavn, log-niveau, cache-mappe, webserver-URLs |
| `FoundryLocalManager` | Hovedindgangspunkt - oprettes via `CreateAsync()`, tilgås via `.Instance` |
| `Catalog` | Browse, søg, og hent modeller fra kataloget |
| `Model` | Repræsenterer en specifik model - download, load, hent clients |

#### C# SDK - Manager og Katalog Metoder

| Metode | Beskrivelse |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Initialiser manageren |
| `FoundryLocalManager.Instance` | Tilgå singleton manageren |
| `manager.GetCatalogAsync()` | Hent modelkataloget |
| `catalog.ListModelsAsync()` | List alle tilgængelige modeller |
| `catalog.GetModelAsync(alias: "alias")` | Hent en specifik model via alias |
| `catalog.GetCachedModelsAsync()` | List downloadede modeller |
| `catalog.GetLoadedModelsAsync()` | List aktuelt loadede modeller |

> **C# Arkitektur Note:** C# SDK v0.8.0+ redesign gør applikationen **selvstændig;** den kræver ikke Foundry Local CLI på slutbrugermaskinen. SDK håndterer modelstyring og inferens nativen.

</details>

---

### Øvelse 3: Download og Load en Model

SDK adskiller download (til disk) fra load (i hukommelsen). Det giver dig mulighed for at pre-downloade modeller under opsætning og loade dem efter behov.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Mulighed A: Manuel trin-for-trin
manager = FoundryLocalManager()
manager.start_service()

# Tjek cache først
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

# Mulighed B: Én-linje bootstrap (anbefales)
# Giv alias til konstruktøren - den starter tjenesten, downloader og indlæser automatisk
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Model Management Metoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Download en model til lokal cache |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Load en model i inferens-serveren |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Afload en model fra serveren |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | List alle aktuelt loadede modeller |

#### Python - Cache Management Metoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Hent cache-mappelokation |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | List alle downloadede modeller |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Trin-for-trin tilgang
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

#### JavaScript - Model Metoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Om modellen allerede er downloadet |
| `model.download()` | `() => Promise<void>` | Download modellen til lokal cache |
| `model.load()` | `() => Promise<void>` | Load ind i inferens-serveren |
| `model.unload()` | `() => Promise<void>` | Afload fra inferens-serveren |
| `model.id` | `string` | Modellens unikke identifikator |

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

#### C# - Model Metoder

| Metode | Beskrivelse |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Download den valgte variant |
| `model.LoadAsync()` | Load modellen ind i hukommelsen |
| `model.UnloadAsync()` | Afload modellen |
| `model.SelectVariant(variant)` | Vælg en specifik variant (CPU/GPU/NPU) |
| `model.SelectedVariant` | Den aktuelt valgte variant |
| `model.Variants` | Alle tilgængelige varianter for modellen |
| `model.GetPathAsync()` | Hent den lokale filsti |
| `model.GetChatClientAsync()` | Få en native chat-klient (ingen OpenAI SDK nødvendig) |
| `model.GetAudioClientAsync()` | Få en audioklient til transskription |

</details>

---

### Øvelse 4: Inspicér Modelmetadata

`FoundryModelInfo` objektet indeholder detaljeret metadata om hver model. At forstå disse felter hjælper dig med at vælge den rigtige model til din applikation.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Få detaljerede oplysninger om en specifik model
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

#### FoundryModelInfo Felter

| Felt | Type | Beskrivelse |
|-------|------|-------------|
| `alias` | string | Kort navn (fx `phi-3.5-mini`) |
| `id` | string | Unik modelidentifikator |
| `version` | string | Modelversion |
| `task` | string | `chat-completions` eller `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU eller NPU |
| `execution_provider` | string | Runtime backend (CUDA, CPU, QNN, WebGPU etc.) |
| `file_size_mb` | int | Størrelse på disk i MB |
| `supports_tool_calling` | bool | Om modellen understøtter funktions-/værktøjskald |
| `publisher` | string | Hvem der har publiceret modellen |
| `license` | string | Licensnavn |
| `uri` | string | Model-URI |
| `prompt_template` | dict/null | Prompt-skabelon, hvis nogen |

---

### Øvelse 5: Administrér Modellers Livscyklus

Øv dig i hele livscyklussen: list → download → load → brug → unload.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Lille model til hurtig test

manager = FoundryLocalManager()
manager.start_service()

# 1. Tjek hvad der er i kataloget
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Tjek hvad der allerede er downloadet
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Download en model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Bekræft at den nu er i cachen
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Indlæs den
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Tjek hvad der er indlæst
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Afslut den
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

const alias = "qwen2.5-0.5b"; // Lille model til hurtig test

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Hent model fra katalog
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Download hvis nødvendigt
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Indlæs den
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Aflæs den
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Øvelse 6: Hurtigstartsmønstrene

Hvert sprog tilbyder en genvej til at starte tjenesten og indlæse en model i et enkelt kald. Disse er de **anbefalede mønstre** for de fleste applikationer.

<details>
<summary><h3>🐍 Python - Constructor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Giv et alias til konstruktøren - det håndterer alt:
# 1. Starter tjenesten, hvis den ikke kører
# 2. Downloader modellen, hvis den ikke er cachet
# 3. Indlæser modellen i inferensserveren
manager = FoundryLocalManager("phi-3.5-mini")

# Klar til brug med det samme
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap`-parameteren (standard `True`) styrer denne adfærd. Sæt `bootstrap=False`, hvis du vil have manuel kontrol:

```python
# Manuel tilstand - intet sker automatisk
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() håndterer alt:
// 1. Starter tjenesten
// 2. Henter modellen fra katalogen
// 3. Downloader om nødvendigt og indlæser modellen
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Klar til brug med det samme
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

> **C# Note:** C# SDK’en bruger `Configuration` til at styre app-navn, logning, cache-mapper og endda fastlåse en bestemt webserver-port. Det gør den mest konfigurerbare af de tre SDK’er.

</details>

---

### Øvelse 7: Den Native ChatClient (Ingen OpenAI SDK Nødvendig)

JavaScript- og C#-SDK’erne tilbyder en `createChatClient()` hjælpefunktion, der returnerer en native chatclient — ingen grund til at installere eller konfigurere OpenAI SDK’en separat.

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

// Opret en ChatClient direkte fra modellen — ingen OpenAI-import nødvendig
const chatClient = model.createChatClient();

// completeChat returnerer et OpenAI-kompatibelt svarobjekt
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming bruger et callback-mønster
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` understøtter også kald til værktøjer — send værktøjer som andet argument:

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

> **Hvornår man bruger hvilket mønster:**
> - **`createChatClient()`** — Hurtig prototyping, færre afhængigheder, enklere kode
> - **OpenAI SDK** — Fuld kontrol over parametre (temperatur, top_p, stop tokens osv.), bedre til produktionsapplikationer

---

### Øvelse 8: Modelvarianter og Hardwarevalg

Modeller kan have flere **varianter** optimeret til forskellig hardware. SDK’en vælger automatisk den bedste variant, men du kan også inspicere og vælge manuelt.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// List alle tilgængelige varianter
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK'en vælger automatisk den bedste variant til dit hardware
// For at tilsidesætte, brug selectVariant():
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

I Python vælger SDK’en automatisk den bedste variant baseret på hardwaren. Brug `get_model_info()` for at se, hvad der blev valgt:

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

#### Modeller med NPU-varianter

Nogle modeller har NPU-optimerede varianter til enheder med Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Model | NPU Variant Tilgængelig |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Tip:** På NPU-kompatibel hardware vælger SDK’en automatisk NPU-varianten, når den er tilgængelig. Du behøver ikke ændre din kode. For C#-projekter på Windows, tilføj `Microsoft.AI.Foundry.Local.WinML` NuGet-pakken for at aktivere QNN eksekveringsudbyderen — QNN leveres som en plugin EP gennem WinML.

---

### Øvelse 9: Modelopgraderinger og Katalogopdatering

Modelkataloget opdateres jævnligt. Brug disse metoder til at tjekke for og anvende opdateringer.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Opdater kataloget for at få den nyeste model liste
manager.refresh_catalog()

# Tjek om en cached model har en nyere version tilgængelig
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

// Opdater kataloget for at hente den nyeste model liste
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// List alle tilgængelige modeller efter opdatering
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Øvelse 10: Arbejde med Reasoning-Model

**phi-4-mini-reasoning**-modellen inkluderer ræsonnering med kæde-af-tanker. Den indhyller sin interne tankeproces i `<think>...</think>` tags før den producerer sit endelige svar. Dette er nyttigt til opgaver, der kræver flere logiske trin, matematik eller problemløsning.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning er ~4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Modellen pakker sin tænkning ind i <think>...</think> tags
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

// Analyser kæde-af-tankegang
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Hvornår man bruger reasoning-modeller:**
> - Matematik- og logiske problemer
> - Flertrins planlægningsopgaver
> - Komplekse kodegenereringer
> - Opgaver, hvor arbejdsvisning forbedrer nøjagtigheden
>
> **Trade-off:** Reasoning-modeller producerer flere tokens ( `<think>` sektionen) og er langsommere. Til simple Q&A-opgaver er en standardmodel som phi-3.5-mini hurtigere.

---

### Øvelse 11: Forståelse af Aliasser og Hardwarevalg

Når du bruger et **alias** (som `phi-3.5-mini`) i stedet for et komplet model-ID, vælger SDK’en automatisk den bedste variant til din hardware:

| Hardware | Valgt Eksekveringsudbyder |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (via WinML plugin) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Enhver enhed (fallback) | `CPUExecutionProvider` eller `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Aliaset løser til den bedste variant til DIN hardware
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tip:** Brug altid aliasser i din applikationskode. Når du deployer til en brugers maskine, vælger SDK’en den optimale variant ved runtime - CUDA på NVIDIA, QNN på Qualcomm, CPU andetsteds.

---

### Øvelse 12: C# Konfigurationsmuligheder

C# SDK’ens `Configuration`-klasse giver finmasket kontrol over runtime:

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

| Egenskab | Standard | Beskrivelse |
|----------|----------|-------------|
| `AppName` | (pålægges) | Din applikations navn |
| `LogLevel` | `Information` | Logningslogik |
| `Web.Urls` | (dynamisk) | Fastlås en bestemt port for webserveren |
| `AppDataDir` | OS standard | Basismappe for appdata |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Hvor modeller gemmes |
| `LogsDir` | `{AppDataDir}/logs` | Hvor logs skrives |

---

### Øvelse 13: Browserbrug (Kun JavaScript)

JavaScript SDK’en inkluderer en browserkompatibel version. I browseren skal du manuelt starte tjenesten via CLI og angive host-URL:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Start tjenesten manuelt først:
//   foundry service start
// Brug derefter URL'en fra CLI-outputtet
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Gennemse kataloget
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Browserbegrænsninger:** Browserversionen understøtter **ikke** `startWebService()`. Du skal sikre dig, at Foundry Local-tjenesten allerede kører, inden du bruger SDK’en i en browser.

---

## Fuldstændig API Reference

### Python

| Kategori | Metode | Beskrivelse |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Opret manager; valgfrit bootstrap med model |
| **Service** | `is_service_running()` | Tjek om tjenesten kører |
| **Service** | `start_service()` | Start tjenesten |
| **Service** | `endpoint` | API-endpoint URL |
| **Service** | `api_key` | API-nøgle |
| **Catalog** | `list_catalog_models()` | Liste alle tilgængelige modeller |
| **Catalog** | `refresh_catalog()` | Opfrisk kataloget |
| **Catalog** | `get_model_info(alias_or_model_id)` | Hent modelmetadata |
| **Cache** | `get_cache_location()` | Cache-mappesti |
| **Cache** | `list_cached_models()` | Liste downloadede modeller |
| **Model** | `download_model(alias_or_model_id)` | Download en model |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Indlæs en model |
| **Model** | `unload_model(alias_or_model_id)` | Aflyt en model |
| **Model** | `list_loaded_models()` | Liste indlæste modeller |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Tjek om nyere version er tilgængelig |
| **Model** | `upgrade_model(alias_or_model_id)` | Opgrader en model til nyeste version |
| **Service** | `httpx_client` | Forudkonfigureret HTTPX-klient til direkte API-kald |

### JavaScript

| Kategori | Metode | Beskrivelse |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Initialiser SDK singleton |
| **Init** | `FoundryLocalManager.instance` | Adgang til singleton manager |
| **Service** | `manager.startWebService()` | Start webtjenesten |
| **Service** | `manager.urls` | Array af base-URL’er for tjenesten |
| **Catalog** | `manager.catalog` | Adgang til modelkatalog |
| **Catalog** | `catalog.getModel(alias)` | Hent modelobjekt via alias (returnerer Promise) |
| **Model** | `model.isCached` | Om modellen er downloadet |
| **Model** | `model.download()` | Download modellen |
| **Model** | `model.load()` | Indlæs modellen |
| **Model** | `model.unload()` | Aflyt modellen |
| **Model** | `model.id` | Modellens unikke ID |
| **Model** | `model.alias` | Modellens alias |
| **Model** | `model.createChatClient()` | Få en native chatclient (ingen OpenAI SDK krævet) |
| **Model** | `model.createAudioClient()` | Få en audioclient til transkription |
| **Model** | `model.removeFromCache()` | Fjern modellen fra lokal cache |
| **Model** | `model.selectVariant(variant)` | Vælg en specifik hardwarevariant |
| **Model** | `model.variants` | Array af tilgængelige varianter for denne model |
| **Model** | `model.isLoaded()` | Tjek om modellen er indlæst |
| **Catalog** | `catalog.getModels()` | List alle tilgængelige modeller |
| **Catalog** | `catalog.getCachedModels()` | List downloadede modeller |
| **Catalog** | `catalog.getLoadedModels()` | List aktuelt indlæste modeller |
| **Catalog** | `catalog.updateModels()` | Opfrisk kataloget fra tjenesten |
| **Service** | `manager.stopWebService()` | Stop Foundry Local webtjenesten |

### C# (v0.8.0+)

| Kategori | Metode | Beskrivelse |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Initialiser manageren |
| **Init** | `FoundryLocalManager.Instance` | Adgang til singleton |
| **Catalog** | `manager.GetCatalogAsync()` | Hent katalog |
| **Catalog** | `catalog.ListModelsAsync()` | Liste alle modeller |
| **Catalog** | `catalog.GetModelAsync(alias)` | Hent en specifik model |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Liste cached modeller |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Liste indlæste modeller |
| **Model** | `model.DownloadAsync(progress?)` | Download en model |
| **Model** | `model.LoadAsync()` | Indlæs en model |
| **Model** | `model.UnloadAsync()` | Aflyt en model |
| **Model** | `model.SelectVariant(variant)` | Vælg hardwarevariant |
| **Model** | `model.GetChatClientAsync()` | Få native chatclient |
| **Model** | `model.GetAudioClientAsync()` | Få audiotranskriberingsclient |
| **Model** | `model.GetPathAsync()` | Få lokal filsti |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Hent en specifik hardwarevariant |
| **Catalog** | `catalog.UpdateModelsAsync()` | Opfrisk katalog |
| **Server** | `manager.StartWebServerAsync()` | Start REST-webserver |
| **Server** | `manager.StopWebServerAsync()` | Stop REST-webserver |
| **Config** | `config.ModelCacheDir` | Cache-mappe |

---

## Vigtige Pointer

| Koncept | Hvad du lærte |
|---------|-----------------|
| **SDK vs CLI** | SDK’en giver programmatisk kontrol - essentielt for applikationer |
| **Kontrolplan** | SDK’en styrer tjenester, modeller og caching |
| **Dynamiske porte** | Brug altid `manager.endpoint` (Python) eller `manager.urls[0]` (JS/C#) - aldrig hardkod en port |
| **Aliasser** | Brug aliasser for automatisk hardware-optimal modelvalg |
| **Hurtig start** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# redesign** | v0.8.0+ er selvstændig - ingen CLI nødvendig på slutbrugermaskiner |
| **Model livscyklus** | Katalog → Download → Indlæs → Brug → Frigiv |
| **FoundryModelInfo** | Omfattende metadata: opgave, enhed, størrelse, licens, værktøj der kalder support |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) til brug uden OpenAI |
| **Varianter** | Modeller har hardware-specifikke varianter (CPU, GPU, NPU); vælges automatisk |
| **Opgraderinger** | Python: `is_model_upgradeable()` + `upgrade_model()` for at holde modeller opdaterede |
| **Katalogopdatering** | `refresh_catalog()` (Python) / `updateModels()` (JS) for at finde nye modeller |

---

## Ressourcer

| Ressource | Link |
|----------|------|
| SDK Reference (alle sprog) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integration med inference SDK'er | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API Reference | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK Eksempler | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local hjemmeside | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Næste Skridt

Fortsæt til [Del 3: Brug af SDK med OpenAI](part3-sdk-and-apis.md) for at forbinde SDK'en til OpenAI klientbiblioteket og opbygge din første chatafslutningsapplikation.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:
Dette dokument er blevet oversat ved hjælp af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi bestræber os på nøjagtighed, bedes du være opmærksom på, at automatiserede oversættelser kan indeholde fejl eller unøjagtigheder. Det oprindelige dokument på dets modersmål bør betragtes som den autoritative kilde. For kritisk information anbefales professionel menneskelig oversættelse. Vi er ikke ansvarlige for eventuelle misforståelser eller fejltolkninger, der opstår som følge af brugen af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->