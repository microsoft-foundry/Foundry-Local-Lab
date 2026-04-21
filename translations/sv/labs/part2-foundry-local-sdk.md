![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 2: Foundry Local SDK Djupdykning

> **Mål:** Bemästra Foundry Local SDK för att programmatiskt hantera modeller, tjänster och cache – och förstå varför SDK är det rekommenderade tillvägagångssättet framför CLI när du bygger applikationer.

## Översikt

I Del 1 använde du **Foundry Local CLI** för att ladda ner och köra modeller interaktivt. CLI är utmärkt för utforskning, men när du bygger riktiga applikationer behöver du **programmatisk kontroll**. Foundry Local SDK ger dig det – det hanterar **kontrollplanet** (starta tjänst, upptäcka modeller, ladda ner, ladda) så att din applikationskod kan fokusera på **dataplanet** (skicka promptar, ta emot resultat).

Denna labb lär dig hela SDK:s API-ytan över Python, JavaScript och C#. I slutet kommer du förstå alla tillgängliga metoder och när du ska använda dem.

## Lärandemål

I slutet av denna labb ska du kunna:

- Förklara varför SDK föredras över CLI för applikationsutveckling
- Installera Foundry Local SDK för Python, JavaScript eller C#
- Använda `FoundryLocalManager` för att starta tjänsten, hantera modeller och fråga katalogen
- Lista, ladda ner, ladda och avlasta modeller programmässigt
- Inspektera modellmetadata med `FoundryModelInfo`
- Förstå skillnaden mellan katalog-, cache- och laddade modeller
- Använda konstruktörs-bootstrap (Python) och `create()` + katalog-mönster (JavaScript)
- Förstå C# SDK:s redesign och dess objektorienterade API

---

## Förutsättningar

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Installerad och i din `PATH` ([Del 1](part1-getting-started.md)) |
| **Språkruntime** | **Python 3.9+** och/eller **Node.js 18+** och/eller **.NET 9.0+** |

---

## Koncept: SDK vs CLI – Varför använda SDK?

| Aspekt | CLI (`foundry` kommando) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Användningsområde** | Utforskning, manuell testning | Applikationsintegration |
| **Tjänstehantering** | Manuell: `foundry service start` | Automatisk: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Portupptäckt** | Läs från CLI-utdata | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Modell-nedladdning** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Felhante­ring** | Exit-koder, stderr | Undantag, typade fel |
| **Automatisering** | Shellskript | Native språk­integration |
| **Driftsättning** | Kräver CLI på slutanvändarens dator | C# SDK kan vara självförsörjande (ingen CLI behövs) |

> **Viktig insikt:** SDK hanterar hela livscykeln: starta tjänsten, kolla cachen, ladda ner saknade modeller, ladda dem och hitta endpoint på några kodrader. Din applikation behöver inte tolka CLI-utdata eller hantera underprocesser.

---

## Labbövningar

### Övning 1: Installera SDK:n

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Verifiera installationen:

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

Verifiera installationen:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Det finns två NuGet-paket:

| Paket | Plattform | Beskrivning |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Plattformoberoende | Fungerar på Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Endast Windows | Lägger till WinML-hårdvaruacceleration; laddar ner och installerar plugin-exekveringsleverantörer (t.ex. QNN för Qualcomm NPU) |

**Windows-inställning:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Redigera `.csproj`-filen:

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

> **Notera:** På Windows är WinML-paketet ett superset som inkluderar bas-SDK plus QNN-exekveringsleverantören. På Linux/macOS används bas-SDK istället. De konditionella TFM- och paketreferenserna gör projektet fullt plattformsoberoende.

Skapa en `nuget.config` i projektets rotmapp:

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

Återställ paket:

```bash
dotnet restore
```

</details>

---

### Övning 2: Starta tjänsten och lista katalogen

Det första en applikation gör är att starta Foundry Local-tjänsten och upptäcka vilka modeller som finns tillgängliga.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Skapa en manager och starta tjänsten
manager = FoundryLocalManager()
manager.start_service()

# Lista alla modeller som finns tillgängliga i katalogen
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Metoder för tjänstehantering

| Metod | Signatur | Beskrivning |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Kontrollera om tjänsten körs |
| `start_service()` | `() -> None` | Starta Foundry Local-tjänsten |
| `service_uri` | `@property -> str` | Bas-URI för tjänsten |
| `endpoint` | `@property -> str` | API-endpoint (tjänst-URI + `/v1`) |
| `api_key` | `@property -> str` | API-nyckel (från miljö eller standardplatshållare) |

#### Python SDK - Metoder för kataloghantering

| Metod | Signatur | Beskrivning |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Lista alla modeller i katalogen |
| `refresh_catalog()` | `() -> None` | Uppdatera katalogen från tjänsten |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Hämta info för specifik modell |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Skapa en manager och starta tjänsten
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Bläddra i katalogen
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Manager Metoder

| Metod | Signatur | Beskrivning |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Initialisera SDK-singleton |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Åtkomst till singleton-manager |
| `manager.startWebService()` | `() => Promise<void>` | Starta Foundry Local webb­tjänst |
| `manager.urls` | `string[]` | Array med bas-URL:er för tjänsten |

#### JavaScript SDK - Katalog- och Modellmetoder

| Metod | Signatur | Beskrivning |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Åtkomst till modellkatalogen |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Hämta modellobjekt via alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ använder en objektorienterad arkitektur med objekt för `Configuration`, `Catalog` och `Model`:

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

#### C# SDK - Viktiga klasser

| Klass | Syfte |
|-------|---------|
| `Configuration` | Ange appnamn, loggningsnivå, cachedir, webbtjänst-URL:er |
| `FoundryLocalManager` | Huvudåtkomstpunkt - skapas via `CreateAsync()`, nås via `.Instance` |
| `Catalog` | Bläddra, sök och hämta modeller från katalogen |
| `Model` | Representerar en specifik modell - ladda ner, ladda, få klienter |

#### C# SDK - Manager- och Katalogmetoder

| Metod | Beskrivning |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Initialisera manager |
| `FoundryLocalManager.Instance` | Åtkomst till singleton-manager |
| `manager.GetCatalogAsync()` | Hämta modellkatalog |
| `catalog.ListModelsAsync()` | Lista alla tillgängliga modeller |
| `catalog.GetModelAsync(alias: "alias")` | Hämta specifik modell via alias |
| `catalog.GetCachedModelsAsync()` | Lista nedladdade modeller |
| `catalog.GetLoadedModelsAsync()` | Lista aktuellt laddade modeller |

> **Notis om C# Arkitektur:** C# SDK v0.8.0+ redesign gör applikationen **självförsörjande**; den kräver inte Foundry Local CLI på slutanvändarens dator. SDK hanterar modellhantering och inferens nativt.

</details>

---

### Övning 3: Ladda ner och ladda en modell

SDK separerar nedladdning (till disk) från laddning (i minnet). Det låter dig förladda modeller under setup och ladda dem vid behov.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Alternativ A: Manuell steg-för-steg
manager = FoundryLocalManager()
manager.start_service()

# Kontrollera cache först
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

# Alternativ B: Enkelrads bootstrap (rekommenderas)
# Skicka alias till konstruktorn - det startar tjänsten, laddar ner och laddar automatiskt
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Modellhanteringsmetoder

| Metod | Signatur | Beskrivning |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Ladda ner en modell till lokal cache |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Ladda en modell i inferenstjänsten |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Avlasta en modell från tjänsten |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Lista alla för närvarande laddade modeller |

#### Python - Cachehanteringsmetoder

| Metod | Signatur | Beskrivning |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Hämta cache-katalogens sökväg |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Lista alla nedladdade modeller |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Steg-för-steg tillvägagångssätt
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

#### JavaScript - Modellmetoder

| Metod | Signatur | Beskrivning |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Om modellen redan är nedladdad |
| `model.download()` | `() => Promise<void>` | Ladda ner modellen till lokal cache |
| `model.load()` | `() => Promise<void>` | Ladda in i inferenstjänsten |
| `model.unload()` | `() => Promise<void>` | Avlasta från inferenstjänsten |
| `model.id` | `string` | Modellens unika identifierare |

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

#### C# - Modellmetoder

| Metod | Beskrivning |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Ladda ner vald variant |
| `model.LoadAsync()` | Ladda modellen till minnet |
| `model.UnloadAsync()` | Avlasta modellen |
| `model.SelectVariant(variant)` | Välj specifik variant (CPU/GPU/NPU) |
| `model.SelectedVariant` | Den för närvarande valda varianten |
| `model.Variants` | Alla tillgängliga varianter för denna modell |
| `model.GetPathAsync()` | Hämta lokal filväg |
| `model.GetChatClientAsync()` | Hämta en native chattklient (ingen OpenAI SDK behövs) |
| `model.GetAudioClientAsync()` | Hämta en ljudklient för transkribering |

</details>

---

### Övning 4: Inspektera modellmetadata

`FoundryModelInfo`-objektet innehåller rik metadata om varje modell. Att förstå dessa fält hjälper dig välja rätt modell för din applikation.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Få detaljerad information om en specifik modell
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

#### Fält i FoundryModelInfo

| Fält | Typ | Beskrivning |
|-------|------|-------------|
| `alias` | string | Kortnamn (t.ex. `phi-3.5-mini`) |
| `id` | string | Unikt modell-id |
| `version` | string | Modellversion |
| `task` | string | `chat-completions` eller `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, eller NPU |
| `execution_provider` | string | Runtime-backend (CUDA, CPU, QNN, WebGPU etc.) |
| `file_size_mb` | int | Storlek på disk i MB |
| `supports_tool_calling` | bool | Om modellen stödjer funktions-/verktygsanrop |
| `publisher` | string | Vem som publicerat modellen |
| `license` | string | Licensnamn |
| `uri` | string | Modell-URI |
| `prompt_template` | dict/null | Promptmall, om någon |

---

### Övning 5: Hantera modellens livscykel

Öva på hela livscykeln: lista → ladda ner → ladda → använd → avlasta.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Liten modell för snabb testning

manager = FoundryLocalManager()
manager.start_service()

# 1. Kontrollera vad som finns i katalogen
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Kontrollera vad som redan är nedladdat
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Ladda ner en modell
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Verifiera att den nu finns i cacheminnet
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Ladda den
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Kontrollera vad som är laddat
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Ladda ur den
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

const alias = "qwen2.5-0.5b"; // Liten modell för snabb testning

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Hämta modell från katalogen
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Ladda ner om det behövs
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Ladda in den
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Ladda ur den
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Övning 6: Snabbstartsmönstren

Varje språk erbjuder en genväg för att starta tjänsten och ladda en modell i ett enda anrop. Dessa är de **rekommenderade mönstren** för de flesta applikationer.

<details>
<summary><h3>🐍 Python - Konstruktor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Ge ett alias till konstruktorn - det hanterar allt:
# 1. Startar tjänsten om den inte körs
# 2. Laddar ner modellen om den inte finns i cache
# 3. Laddar modellen till inferenstjänsten
manager = FoundryLocalManager("phi-3.5-mini")

# Klar att användas omedelbart
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Parametern `bootstrap` (standard `True`) styr detta beteende. Sätt `bootstrap=False` om du vill ha manuell kontroll:

```python
# Manuellt läge - inget händer automatiskt
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() hanterar allt:
// 1. Startar tjänsten
// 2. Hämtar modellen från katalogen
// 3. Laddar ner vid behov och laddar modellen
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Redo att användas omedelbart
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Katalog</h3></summary>

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

> **C# Notis:** C# SDK använder `Configuration` för att styra appnamn, loggning, cachekataloger och även fästa en specifik webserverport. Detta gör den mest konfigurerbara av de tre SDK:erna.

</details>

---

### Övning 7: Den Nativa ChatClient (Ingen OpenAI SDK Behövs)

JavaScript och C# SDK:erna erbjuder en `createChatClient()` bekvämlighetsmetod som returnerar en native chattklient — inget behov av att installera eller konfigurera OpenAI SDK separat.

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

// Skapa en ChatClient direkt från modellen — ingen OpenAI-import behövs
const chatClient = model.createChatClient();

// completeChat returnerar ett OpenAI-kompatibelt svarobjekt
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming använder ett callback-mönster
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` stöder även verktygsanrop — skicka verktyg som andra argument:

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

> **När man ska använda vilket mönster:**
> - **`createChatClient()`** — Snabb prototypning, färre beroenden, enklare kod
> - **OpenAI SDK** — Full kontroll över parametrar (temperature, top_p, stop tokens, etc.), bättre för produktionsapplikationer

---

### Övning 8: Modellvarianter och Hårdvaruval

Modeller kan ha flera **varianter** optimerade för olika hårdvara. SDK:n väljer automatiskt den bästa varianten, men du kan även inspektera och välja manuellt.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Lista alla tillgängliga varianter
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK:et väljer automatiskt den bästa varianten för din hårdvara
// För att åsidosätta, använd selectVariant():
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

I Python väljer SDK automatiskt den bästa varianten baserat på hårdvara. Använd `get_model_info()` för att se vad som valdes:

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

Vissa modeller har NPU-optimerade varianter för enheter med Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Modell | NPU-variant tillgänglig |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Tips:** På NPU-kompatibel hårdvara väljer SDK automatiskt NPU-varianten när den finns. Du behöver inte ändra din kod. För C#-projekt på Windows, lägg till `Microsoft.AI.Foundry.Local.WinML` NuGet-paketet för att aktivera QNN-exekveringsleverantören — QNN levereras som en plugin EP via WinML.

---

### Övning 9: Modelluppgraderingar och Kataloguppdatering

Modellkatalogen uppdateras periodiskt. Använd dessa metoder för att kontrollera och tillämpa uppdateringar.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Uppdatera katalogen för att få den senaste modellistan
manager.refresh_catalog()

# Kontrollera om en cachad modell har en nyare version tillgänglig
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

// Uppdatera katalogen för att hämta den senaste modellistan
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Lista alla tillgängliga modeller efter uppdatering
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Övning 10: Arbeta med Resonemangsmodeller

Modellen **phi-4-mini-reasoning** inkluderar kedjetänkande (chain-of-thought) resonemang. Den omsluter sitt interna tänkande i `<think>...</think>`-taggar innan den producerar sitt slutgiltiga svar. Detta är användbart för uppgifter som kräver flerstegslogik, matematik eller problemlösning.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning är ~4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Modellen kapslar in sitt tänkande i <think>...</think>-taggar
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

// Tolka kedjetänkande
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **När man ska använda resonemangsmodeller:**
> - Matematik- och logikproblem
> - Flerstegsplaneringsuppgifter
> - Komplex kodgenerering
> - Uppgifter där redovisning av arbetet förbättrar noggrannhet
>
> **Avvägning:** Resonemangsmodeller producerar fler tokens (avsnittet `<think>`) och är långsammare. För enkla Q&A är en standardmodell som phi-3.5-mini snabbare.

---

### Övning 11: Förstå Aliaser och Hårdvaruval

När du anger en **alias** (som `phi-3.5-mini`) istället för ett fullständigt modell-ID, väljer SDK automatiskt den bästa varianten för din hårdvara:

| Hårdvara | Vald exekveringsleverantör |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (via WinML-plugin) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Vilken enhet som helst (fallback) | `CPUExecutionProvider` eller `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Aliaset löser till den bästa varianten för DIN hårdvara
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tips:** Använd alltid alias i din applikationskod. När du distribuerar till en användares maskin väljer SDK den optimala varianten vid körning - CUDA på NVIDIA, QNN på Qualcomm, CPU annars.

---

### Övning 12: C# Konfigurationsalternativ

C# SDK:s `Configuration`-klass ger finmald kontroll över runtime:

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

| Egenskap | Standard | Beskrivning |
|----------|---------|-------------|
| `AppName` | (obligatorisk) | Ditt applikationsnamn |
| `LogLevel` | `Information` | Loggnivå |
| `Web.Urls` | (dynamisk) | Fäst en specifik port för webservern |
| `AppDataDir` | OS-standard | Bas katalog för appdata |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Där modeller sparas |
| `LogsDir` | `{AppDataDir}/logs` | Där loggar skrivs |

---

### Övning 13: Browseranvändning (Endast JavaScript)

JavaScript SDK innehåller en webbläsarkompatibel version. I webbläsaren måste du manuellt starta tjänsten via CLI och ange värd-URL:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Starta tjänsten manuellt först:
//   foundry service start
// Använd sedan URL:en från CLI-utdata
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Bläddra i katalogen
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Browserbegränsningar:** Webbläsarversionen stöder **inte** `startWebService()`. Du måste säkerställa att Foundry Local-tjänsten redan körs innan du använder SDK i en webbläsare.

---

## Komplett API-referens

### Python

| Kategori | Metod | Beskrivning |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Skapa manager; valfritt bootstrap med modell |
| **Tjänst** | `is_service_running()` | Kontrollera om tjänsten körs |
| **Tjänst** | `start_service()` | Starta tjänsten |
| **Tjänst** | `endpoint` | API-endpoint URL |
| **Tjänst** | `api_key` | API-nyckel |
| **Katalog** | `list_catalog_models()` | Lista alla tillgängliga modeller |
| **Katalog** | `refresh_catalog()` | Uppdatera katalogen |
| **Katalog** | `get_model_info(alias_or_model_id)` | Hämta metadata för modell |
| **Cache** | `get_cache_location()` | Cachekatalogens sökväg |
| **Cache** | `list_cached_models()` | Lista nedladdade modeller |
| **Modell** | `download_model(alias_or_model_id)` | Ladda ned en modell |
| **Modell** | `load_model(alias_or_model_id, ttl=600)` | Ladda en modell |
| **Modell** | `unload_model(alias_or_model_id)` | Koppla bort en modell |
| **Modell** | `list_loaded_models()` | Lista laddade modeller |
| **Modell** | `is_model_upgradeable(alias_or_model_id)` | Kontrollera om en nyare version finns |
| **Modell** | `upgrade_model(alias_or_model_id)` | Uppgradera en modell till senaste version |
| **Tjänst** | `httpx_client` | Förkonfigurerad HTTPX-klient för direkta API-anrop |

### JavaScript

| Kategori | Metod | Beskrivning |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Initiera SDK singleton |
| **Init** | `FoundryLocalManager.instance` | Åtkomst till singleton manager |
| **Tjänst** | `manager.startWebService()` | Starta webbtjänsten |
| **Tjänst** | `manager.urls` | Array med bas-URL:er för tjänsten |
| **Katalog** | `manager.catalog` | Åtkomst till modellkatalogen |
| **Katalog** | `catalog.getModel(alias)` | Hämta en modellobjekt via alias (returnerar Promise) |
| **Modell** | `model.isCached` | Om modellen är nedladdad |
| **Modell** | `model.download()` | Ladda ner modellen |
| **Modell** | `model.load()` | Ladda modellen |
| **Modell** | `model.unload()` | Koppla bort modellen |
| **Modell** | `model.id` | Modellens unika identifierare |
| **Modell** | `model.alias` | Modellens alias |
| **Modell** | `model.createChatClient()` | Hämta native chatklient (ingen OpenAI SDK behövs) |
| **Modell** | `model.createAudioClient()` | Hämta ljudklient för transkribering |
| **Modell** | `model.removeFromCache()` | Ta bort modellen från lokal cache |
| **Modell** | `model.selectVariant(variant)` | Välj specifik hårdvaruvariant |
| **Modell** | `model.variants` | Array med tillgängliga varianter för denna modell |
| **Modell** | `model.isLoaded()` | Kontrollera om modellen är laddad |
| **Katalog** | `catalog.getModels()` | Lista alla tillgängliga modeller |
| **Katalog** | `catalog.getCachedModels()` | Lista nedladdade modeller |
| **Katalog** | `catalog.getLoadedModels()` | Lista aktuellt laddade modeller |
| **Katalog** | `catalog.updateModels()` | Uppdatera katalogen från tjänsten |
| **Tjänst** | `manager.stopWebService()` | Stoppa Foundry Local webbtjänst |

### C# (v0.8.0+)

| Kategori | Metod | Beskrivning |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Initiera managern |
| **Init** | `FoundryLocalManager.Instance` | Åtkomst till singleton |
| **Katalog** | `manager.GetCatalogAsync()` | Hämta katalog |
| **Katalog** | `catalog.ListModelsAsync()` | Lista alla modeller |
| **Katalog** | `catalog.GetModelAsync(alias)` | Hämta specifik modell |
| **Katalog** | `catalog.GetCachedModelsAsync()` | Lista cachelagrade modeller |
| **Katalog** | `catalog.GetLoadedModelsAsync()` | Lista laddade modeller |
| **Modell** | `model.DownloadAsync(progress?)` | Ladda ner en modell |
| **Modell** | `model.LoadAsync()` | Ladda en modell |
| **Modell** | `model.UnloadAsync()` | Koppla bort en modell |
| **Modell** | `model.SelectVariant(variant)` | Välj hårdvaruvariant |
| **Modell** | `model.GetChatClientAsync()` | Hämta native chatklient |
| **Modell** | `model.GetAudioClientAsync()` | Hämta ljudtranskriptionsklient |
| **Modell** | `model.GetPathAsync()` | Hämta lokal filsökväg |
| **Katalog** | `catalog.GetModelVariantAsync(alias, variant)` | Hämta specifik hårdvaruvariant |
| **Katalog** | `catalog.UpdateModelsAsync()` | Uppdatera katalog |
| **Server** | `manager.StartWebServerAsync()` | Starta REST-webbserver |
| **Server** | `manager.StopWebServerAsync()` | Stoppa REST-webbserver |
| **Konfig** | `config.ModelCacheDir` | Cachekatalog |

---

## Viktiga Insikter

| Koncept | Vad Du Lärde Dig |
|---------|-----------------|
| **SDK vs CLI** | SDK erbjuder programmatisk kontroll - nödvändigt för applikationer |
| **Kontrollplan** | SDK hanterar tjänster, modeller och caching |
| **Dynamiska portar** | Använd alltid `manager.endpoint` (Python) eller `manager.urls[0]` (JS/C#) – hårdkoda aldrig port |
| **Aliaser** | Använd alias för automatisk hårdvaruoptimal modellval |
| **Snabbstart** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# omdesign** | v0.8.0+ är självständigt - ingen CLI behövs på slutanvändarmaskiner |
| **Modellens livscykel** | Katalog → Ladda ner → Ladda → Använd → Avlasta |
| **FoundryModelInfo** | Rik metadata: uppgift, enhet, storlek, licens, verktygsanropsstöd |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) för OpenAI-fritt användande |
| **Varianter** | Modeller har hårdvaruspecifika varianter (CPU, GPU, NPU); väljs automatiskt |
| **Uppgraderingar** | Python: `is_model_upgradeable()` + `upgrade_model()` för att hålla modeller aktuella |
| **Uppdatering av katalog** | `refresh_catalog()` (Python) / `updateModels()` (JS) för att upptäcka nya modeller |

---

## Resurser

| Resurs | Länk |
|----------|------|
| SDK-referens (alla språk) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrera med inferens-SDK:er | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API-referens | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK-exempel | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local webbplats | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Nästa steg

Fortsätt till [Del 3: Använda SDK med OpenAI](part3-sdk-and-apis.md) för att koppla SDK:en till OpenAI-klientbiblioteket och skapa din första applikation för chattkomplettering.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfriskrivning**:  
Detta dokument har översatts med hjälp av AI-översättningstjänsten [Co-op Translator](https://github.com/Azure/co-op-translator). Även om vi strävar efter noggrannhet, vänligen notera att automatiska översättningar kan innehålla fel eller brister. Det ursprungliga dokumentet på dess modersmål ska betraktas som den auktoritativa källan. För viktig information rekommenderas professionell mänsklig översättning. Vi ansvarar inte för några missförstånd eller feltolkningar som uppstår från användningen av denna översättning.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->