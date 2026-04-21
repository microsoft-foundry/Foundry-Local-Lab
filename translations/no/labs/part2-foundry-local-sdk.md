![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 2: Foundry Local SDK Dypdykk

> **Mål:** Mestre Foundry Local SDK for å administrere modeller, tjenester og caching programmert - og forstå hvorfor SDK-en er den anbefalte tilnærmingen fremfor CLI for å bygge applikasjoner.

## Oversikt

I Del 1 brukte du **Foundry Local CLI** for å laste ned og kjøre modeller interaktivt. CLI-en er flott for utforskning, men når du bygger ekte applikasjoner trenger du **programmatisk kontroll**. Foundry Local SDK gir deg dette - den styrer **kontrollplanet** (starter tjenesten, oppdager modeller, laster ned, laster) slik at applikasjonskoden din kan fokusere på **dataplanet** (sende prompts, motta fullføringer).

Dette laboratoriet lærer deg hele SDK API-overflaten på tvers av Python, JavaScript og C#. På slutten vil du forstå hver metode tilgjengelig og når du skal bruke hver.

## Læringsmål

På slutten av dette laboratoriet vil du kunne:

- Forklare hvorfor SDK foretrekkes fremfor CLI for applikasjonsutvikling
- Installere Foundry Local SDK for Python, JavaScript eller C#
- Bruke `FoundryLocalManager` for å starte tjenesten, administrere modeller og søke i katalogen
- Liste, laste ned, laste og avlaste modeller programmatisk
- Inspisere modellmetadata ved bruk av `FoundryModelInfo`
- Forstå forskjellen mellom katalog, cache og lastede modeller
- Bruke konstruktør bootstrap (Python) og `create()` + katalogmønster (JavaScript)
- Forstå C# SDK redesign og dets objektorienterte API

---

## Forutsetninger

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Installert og på din `PATH` ([Del 1](part1-getting-started.md)) |
| **Språkruntime** | **Python 3.9+** og/eller **Node.js 18+** og/eller **.NET 9.0+** |

---

## Konsept: SDK vs CLI - Hvorfor bruke SDK?

| Aspekt | CLI (`foundry` kommando) | SDK (`foundry-local-sdk`) |
|--------|--------------------------|--------------------------|
| **Brukstilfelle** | Utforskning, manuell testing | Applikasjonsintegrasjon |
| **Tjenestehåndtering** | Manuell: `foundry service start` | Automatisk: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Portoppdagelse** | Leses fra CLI-utdata | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Modellnedlasting** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Feilhåndtering** | Avslutningskoder, stderr | Unntak, typede feil |
| **Automatisering** | Shell-skript | Native språk-integrasjon |
| **Distribusjon** | Krever CLI på sluttbrukers maskin | C# SDK kan være selvstendig (ingen CLI nødvendig) |

> **Nøkkelfunn:** SDK-en håndterer hele livssyklusen: starter tjenesten, sjekker cachen, laster ned manglende modeller, laster dem inn, og oppdager endepunktet, med få kodelinjer. Applikasjonen din trenger ikke å tolke CLI-utdata eller håndtere underprosesser.

---

## Laboratorieøvelser

### Øvelse 1: Installer SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Verifiser installasjonen:

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

Verifiser installasjonen:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Det finnes to NuGet-pakker:

| Pakke | Plattform | Beskrivelse |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Kryssplattform | Fungerer på Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Kun Windows | Legger til WinML maskinvareakselerasjon; laster ned og installerer plugin utføringsleverandører (f.eks. QNN for Qualcomm NPU) |

**Windows-oppsett:**

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

> **Merk:** På Windows er WinML-pakken et oversett som inkluderer basis-SDK pluss QNN utføringsleverandør. På Linux/macOS brukes basis-SDK i stedet. Den betingede TFM og pakke-referansene gjør prosjektet fullt kryssplattform.

Opprett en `nuget.config` i prosjektroten:

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

Gjenopprett pakker:

```bash
dotnet restore
```

</details>

---

### Øvelse 2: Start tjenesten og list katalogen

Det første en applikasjon gjør er å starte Foundry Local-tjenesten og oppdage hvilke modeller som er tilgjengelige.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Opprett en manager og start tjenesten
manager = FoundryLocalManager()
manager.start_service()

# List opp alle modeller tilgjengelig i katalogen
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Tjenestehåndteringsmetoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Sjekk om tjenesten kjører |
| `start_service()` | `() -> None` | Start Foundry Local-tjenesten |
| `service_uri` | `@property -> str` | Grunn-URI for tjenesten |
| `endpoint` | `@property -> str` | API-endepunkt (tjeneste URI + `/v1`) |
| `api_key` | `@property -> str` | API-nøkkel (fra miljø eller standardplassholder) |

#### Python SDK - Kataloghåndteringsmetoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | List alle modeller i katalogen |
| `refresh_catalog()` | `() -> None` | Oppdater katalogen fra tjenesten |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Hent info for en spesifikk modell |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Opprett en administrator og start tjenesten
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Bla gjennom katalogen
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Manager-metoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Initialiser SDK singleton |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Tilgang til singleton manager |
| `manager.startWebService()` | `() => Promise<void>` | Start Foundry Local webtjeneste |
| `manager.urls` | `string[]` | Array av grunnleggende URL-er for tjenesten |

#### JavaScript SDK - Katalog- og modellmetoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Tilgang til modellkatalogen |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Hent en modellobjekt via alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK versjon 0.8.0+ bruker en objektorientert arkitektur med `Configuration`, `Catalog` og `Model` objekter:

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

#### C# SDK - Nøkkelklasser

| Klasse | Formål |
|-------|---------|
| `Configuration` | Sett app navn, loggnivå, cache-mappe, webserver URL-er |
| `FoundryLocalManager` | Hovedinngangspunkt - opprettes via `CreateAsync()`, aksesseres via `.Instance` |
| `Catalog` | Bla i, søk og hent modeller fra katalogen |
| `Model` | Representerer en spesifikk modell - last ned, last, få klienter |

#### C# SDK - Manager- og katalogmetoder

| Metode | Beskrivelse |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Initialiser manager |
| `FoundryLocalManager.Instance` | Tilgang til singleton manager |
| `manager.GetCatalogAsync()` | Hent modellkatalog |
| `catalog.ListModelsAsync()` | List alle tilgjengelige modeller |
| `catalog.GetModelAsync(alias: "alias")` | Hent en spesifikk modell etter alias |
| `catalog.GetCachedModelsAsync()` | List nedlastede modeller |
| `catalog.GetLoadedModelsAsync()`` | List for øyeblikket lastede modeller |

> **C# Arkitekturmerknad:** C# SDK versjon 0.8.0+ redesign gjør applikasjonen **selvstendig**; den krever ikke Foundry Local CLI på sluttbrukers maskin. SDK håndterer modell-administrasjon og inferens nativt.

</details>

---

### Øvelse 3: Last ned og last en modell

SDK-en skiller på nedlasting (til disk) fra lasting (inn i minnet). Dette lar deg forhånds-nedlaste modeller under oppsett og laste dem ved behov.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Alternativ A: Manuell trinn for trinn
manager = FoundryLocalManager()
manager.start_service()

# Sjekk hurtigbuffer først
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

# Alternativ B: Én-linje bootstrap (anbefalt)
# Send alias til konstruktør - det starter tjenesten, laster ned og laster automatisk
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Modellhåndteringsmetoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Last ned en modell til lokal cache |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Last inn en modell i inferansetjener |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Avlast en modell fra tjenesten |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | List alle modeller som er lastet inn |

#### Python - Cachehåndteringsmetoder

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Hent cache-mappebanen |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | List alle nedlastede modeller |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Trinn-for-trinn tilnærming
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

| Metode | Signatur | Beskrivelse |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Om modellen allerede er lastet ned |
| `model.download()` | `() => Promise<void>` | Last ned modellen til lokal cache |
| `model.load()` | `() => Promise<void>` | Last inn i inferansetjener |
| `model.unload()` | `() => Promise<void>` | Avlast fra inferansetjener |
| `model.id` | `string` | Modellens unike identifikator |

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

| Metode | Beskrivelse |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Last ned valgt variant |
| `model.LoadAsync()` | Last inn modell i minnet |
| `model.UnloadAsync()` | Avlast modellen |
| `model.SelectVariant(variant)` | Velg en spesifikk variant (CPU/GPU/NPU) |
| `model.SelectedVariant` | Den for øyeblikket valgte varianten |
| `model.Variants` | Alle tilgjengelige varianter for denne modellen |
| `model.GetPathAsync()` | Hent lokal filbane |
| `model.GetChatClientAsync()` | Hent en native chat-klient (ingen OpenAI SDK nødvendig) |
| `model.GetAudioClientAsync()` | Hent en lydklient for transkripsjon |

</details>

---

### Øvelse 4: Inspiser modellmetadata

`FoundryModelInfo`-objektet inneholder rik metadata om hver modell. Forståelse av disse feltene hjelper deg å velge riktig modell for applikasjonen din.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Få detaljert informasjon om en spesifikk modell
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
| `alias` | string | Kortnavn (f.eks. `phi-3.5-mini`) |
| `id` | string | Unik modell-identifikator |
| `version` | string | Modellversjon |
| `task` | string | `chat-completions` eller `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU eller NPU |
| `execution_provider` | string | Kjøretids-backend (CUDA, CPU, QNN, WebGPU, osv.) |
| `file_size_mb` | int | Størrelse på disk i MB |
| `supports_tool_calling` | bool | Om modellen støtter funksjons-/verktøy-kall |
| `publisher` | string | Hvem som publiserte modellen |
| `license` | string | Lisensnavn |
| `uri` | string | Modell-URI |
| `prompt_template` | dict/null | Prompt-mal, om noen |

---

### Øvelse 5: Administrer modellens livssyklus

Øv på hel livssyklus: list → last ned → last → bruk → avlast.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Liten modell for rask testing

manager = FoundryLocalManager()
manager.start_service()

# 1. Sjekk hva som er i katalogen
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Sjekk hva som allerede er lastet ned
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Last ned en modell
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Bekreft at den nå er i hurtigbufferet
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Last den inn
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Sjekk hva som er lastet inn
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Last den ut
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

const alias = "qwen2.5-0.5b"; // Liten modell for rask testing

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Hent modell fra katalog
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Last ned om nødvendig
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Last den inn
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Last den ut
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Øvelse 6: Hurtigstartsmønstrene

Hvert språk tilbyr en snarvei for å starte tjenesten og laste inn en modell i ett kall. Dette er de **anbefalte mønstrene** for de fleste applikasjoner.

<details>
<summary><h3>🐍 Python - Konstruktøroversikt</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Send et alias til konstruktøren - det ordner alt:
# 1. Starter tjenesten hvis den ikke kjører
# 2. Laster ned modellen hvis den ikke er bufret
# 3. Laster modellen inn i inferensserveren
manager = FoundryLocalManager("phi-3.5-mini")

# Klar til bruk umiddelbart
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap`-parameteren (standard `True`) styrer denne oppførselen. Sett `bootstrap=False` hvis du ønsker manuell kontroll:

```python
# Manuell modus - ingenting skjer automatisk
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() håndterer alt:
// 1. Starter tjenesten
// 2. Henter modellen fra katalogen
// 3. Laster ned om nødvendig og laster inn modellen
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Klar til bruk umiddelbart
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

> **C# Merk:** C# SDK bruker `Configuration` for å styre app-navn, logging, cache-kataloger og til og med feste en spesifikk webserver-port. Dette gjør den til den mest konfigurerbare av de tre SDK-ene.

</details>

---

### Øvelse 7: Den native ChatClient (Ingen OpenAI SDK nødvendig)

JavaScript- og C#-SDK-ene tilbyr en `createChatClient()` hjelpefunksjon som returnerer en native chat-klient — du trenger ikke installere eller konfigurere OpenAI SDK separat.

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

// Opprett en ChatClient direkte fra modellen — ingen OpenAI-import nødvendig
const chatClient = model.createChatClient();

// completeChat returnerer et OpenAI-kompatibelt responsobjekt
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Strømming bruker et tilbakekallingsmønster
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` støtter også verktøysanrop — send verktøy som andre argument:

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

> **Når du skal bruke hvilket mønster:**
> - **`createChatClient()`** — Rask prototyping, færre avhengigheter, enklere kode
> - **OpenAI SDK** — Full kontroll over parametere (temperatur, top_p, stopp-tokens osv.), bedre for produksjonsapplikasjoner

---

### Øvelse 8: Modellvarianter og maskinvareseleksjon

Modeller kan ha flere **varianter** optimalisert for forskjellig maskinvare. SDK-en velger automatisk den beste varianten, men du kan også inspisere og velge manuelt.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// List opp alle tilgjengelige varianter
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK-en velger automatisk den beste varianten for maskinvaren din
// For å overstyre, bruk selectVariant():
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

I Python velger SDK automatisk den beste varianten basert på maskinvaren. Bruk `get_model_info()` for å se hva som ble valgt:

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

Noen modeller har NPU-optimaliserte varianter for enheter med Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Modell | NPU-variant tilgjengelig |
|--------|:------------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Tips:** På maskinvare med NPU støttes automatisk NPU-varianten av SDK-en når den er tilgjengelig. Du trenger ikke endre koden din. For C#-prosjekter på Windows, legg til NuGet-pakken `Microsoft.AI.Foundry.Local.WinML` for å aktivere QNN kjøreleverandør — QNN leveres som en plugin EP via WinML.

---

### Øvelse 9: Modelloppgraderinger og katalogoppdatering

Modellkatalogen oppdateres periodisk. Bruk disse metodene for å sjekke etter og anvende oppdateringer.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Oppdater katalogen for å få den nyeste modellisten
manager.refresh_catalog()

# Sjekk om en bufret modell har en nyere versjon tilgjengelig
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

// Oppdater katalogen for å hente den siste modelllisten
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// List opp alle tilgjengelige modeller etter oppdatering
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Øvelse 10: Jobbe med resonneringsmodeller

**phi-4-mini-reasoning**-modellen inkluderer kjede-av-tanker resonnering. Den pakker den interne tankegangen i `<think>...</think>`-tagger før den gir sitt endelige svar. Dette er nyttig for oppgaver som krever flertrinnslogikk, matematikk eller problemløsning.

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

# Modellen pakker inn sin tenking i <think>...</think>-tagger
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

// Tolke tankerekke-tenkning
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Når du bør bruke resonneringsmodeller:**
> - Matematikk- og logikkproblemer
> - Flertrinns planleggingsoppgaver
> - Kompleks kodegenerering
> - Oppgaver hvor å vise fremgangsmåter forbedrer nøyaktighet
>
> **Avveining:** Resonneringsmodeller produserer flere tokens (dvs. `<think>`-seksjonen) og er tregere. For enkel spørsmål-og-svar er en standardmodell som phi-3.5-mini raskere.

---

### Øvelse 11: Forstå aliaser og maskinvareseleksjon

Når du angir et **alias** (som `phi-3.5-mini`) i stedet for full modell-ID, velger SDK automatisk den beste varianten for maskinvaren din:

| Maskinvare | Valgt kjøreleverandør |
|------------|-----------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (via WinML-plugin) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Enhver enhet (tilbakefall) | `CPUExecutionProvider` eller `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Aliaset løses opp til den beste varianten for DIN maskinvare
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tips:** Bruk alltid aliaser i applikasjonskoden din. Når du distribuerer til en brukers maskin, velger SDK den optimale varianten ved kjøretid - CUDA på NVIDIA, QNN på Qualcomm, CPU ellers.

---

### Øvelse 12: Konfigurasjonsmuligheter i C#

C# SDK-ens `Configuration`-klasse gir finmasket kontroll over kjøretiden:

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

| Egenskap | Standard | Beskrivelse |
|----------|----------|-------------|
| `AppName` | (påkrevd) | Applikasjonens navn |
| `LogLevel` | `Information` | Loggnivå |
| `Web.Urls` | (dynamisk) | Fest en spesifikk port for webserveren |
| `AppDataDir` | OS-standard | Basiskatalog for appdata |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Hvor modeller lagres |
| `LogsDir` | `{AppDataDir}/logs` | Hvor logger skrives |

---

### Øvelse 13: Bruk i nettleser (kun JavaScript)

JavaScript SDK-en inkluderer en nettleserkompatibel versjon. I nettleseren må du manuelt starte tjenesten via CLI og spesifisere vert-URL:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Start tjenesten manuelt først:
//   foundry service start
// Bruk deretter URL-en fra CLI-utdataene
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Bla gjennom katalogen
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Nettleserbegrensninger:** Nettleserversjonen støtter **ikke** `startWebService()`. Du må sørge for at Foundry Local-tjenesten allerede kjører før du bruker SDK i en nettleser.

---

## Komplett API-referanse

### Python

| Kategori | Metode | Beskrivelse |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Opprett manager; alternativt bootstrap med en modell |
| **Tjeneste** | `is_service_running()` | Sjekk om tjenesten kjører |
| **Tjeneste** | `start_service()` | Start tjenesten |
| **Tjeneste** | `endpoint` | URL til API-endepunkt |
| **Tjeneste** | `api_key` | API-nøkkel |
| **Katalog** | `list_catalog_models()` | List opp alle tilgjengelige modeller |
| **Katalog** | `refresh_catalog()` | Oppdater katalogen |
| **Katalog** | `get_model_info(alias_or_model_id)` | Hent modellmetadata |
| **Cache** | `get_cache_location()` | Katalogbane for cache |
| **Cache** | `list_cached_models()` | List nedlastede modeller |
| **Modell** | `download_model(alias_or_model_id)` | Last ned en modell |
| **Modell** | `load_model(alias_or_model_id, ttl=600)` | Last inn en modell |
| **Modell** | `unload_model(alias_or_model_id)` | Frigjør en modell |
| **Modell** | `list_loaded_models()` | List innlastede modeller |
| **Modell** | `is_model_upgradeable(alias_or_model_id)` | Sjekk om nyere versjon finnes |
| **Modell** | `upgrade_model(alias_or_model_id)` | Oppgrader modell til siste versjon |
| **Tjeneste** | `httpx_client` | Ferdigkonfigurert HTTPX-klient for direkte API-kall |

### JavaScript

| Kategori | Metode | Beskrivelse |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Initialiser SDK-en som singleton |
| **Init** | `FoundryLocalManager.instance` | Tilgang til singleton manager |
| **Tjeneste** | `manager.startWebService()` | Start webtjenesten |
| **Tjeneste** | `manager.urls` | Array med base-URL-er for tjenesten |
| **Katalog** | `manager.catalog` | Tilgang til modellkatalogen |
| **Katalog** | `catalog.getModel(alias)` | Hent modellobjekt etter alias (returnerer Promise) |
| **Modell** | `model.isCached` | Om modellen er lastet ned |
| **Modell** | `model.download()` | Last ned modellen |
| **Modell** | `model.load()` | Last inn modellen |
| **Modell** | `model.unload()` | Frigjør modellen |
| **Modell** | `model.id` | Modellens unike ID |
| **Modell** | `model.alias` | Modellens alias |
| **Modell** | `model.createChatClient()` | Hent native chat-klient (ingen OpenAI SDK nødvendig) |
| **Modell** | `model.createAudioClient()` | Hent lydklient for transkripsjon |
| **Modell** | `model.removeFromCache()` | Fjern modellen fra lokal cache |
| **Modell** | `model.selectVariant(variant)` | Velg spesifikk maskinvarevariant |
| **Modell** | `model.variants` | Array med tilgjengelige varianter for modellen |
| **Modell** | `model.isLoaded()` | Sjekk om modellen er lastet inn |
| **Katalog** | `catalog.getModels()` | List opp alle tilgjengelige modeller |
| **Katalog** | `catalog.getCachedModels()` | List nedlastede modeller |
| **Katalog** | `catalog.getLoadedModels()` | List modeller som er lastet inn |
| **Katalog** | `catalog.updateModels()` | Oppdater katalog fra tjenesten |
| **Tjeneste** | `manager.stopWebService()` | Stopp Foundry Local webtjeneste |

### C# (v0.8.0+)

| Kategori | Metode | Beskrivelse |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Initialiser manager |
| **Init** | `FoundryLocalManager.Instance` | Tilgang til singleton |
| **Katalog** | `manager.GetCatalogAsync()` | Hent katalog |
| **Katalog** | `catalog.ListModelsAsync()` | List alle modeller |
| **Katalog** | `catalog.GetModelAsync(alias)` | Hent en spesifikk modell |
| **Katalog** | `catalog.GetCachedModelsAsync()` | List nedlastede modeller |
| **Katalog** | `catalog.GetLoadedModelsAsync()` | List lastede modeller |
| **Modell** | `model.DownloadAsync(progress?)` | Last ned en modell |
| **Modell** | `model.LoadAsync()` | Last inn en modell |
| **Modell** | `model.UnloadAsync()` | Frigjør en modell |
| **Modell** | `model.SelectVariant(variant)` | Velg maskinvarevariant |
| **Modell** | `model.GetChatClientAsync()` | Hent native chat-klient |
| **Modell** | `model.GetAudioClientAsync()` | Hent lydtranskripsjonsklient |
| **Modell** | `model.GetPathAsync()` | Hent lokal filbane |
| **Katalog** | `catalog.GetModelVariantAsync(alias, variant)` | Hent spesifikk maskinvarevariant |
| **Katalog** | `catalog.UpdateModelsAsync()` | Oppdater katalog |
| **Server** | `manager.StartWebServerAsync()` | Start REST-webserver |
| **Server** | `manager.StopWebServerAsync()` | Stopp REST-webserver |
| **Config** | `config.ModelCacheDir` | Cache-katalog |

---

## Viktige punkter

| Konsept | Hva du lærte |
|---------|--------------|
| **SDK vs CLI** | SDK gir programmatisk kontroll - essensielt for applikasjoner |
| **Kontrollplan** | SDK styrer tjenester, modeller og caching |
| **Dynamiske porter** | Bruk alltid `manager.endpoint` (Python) eller `manager.urls[0]` (JS/C#) – aldri hardkod port |
| **Alias** | Bruk alias for automatisk maskinvareoptimalisert modellvalg |
| **Rask start** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# redesign** | v0.8.0+ er selvstendig - krever ikke CLI på sluttbrukermaskiner |
| **Modell livssyklus** | Katalog → Last ned → Last inn → Bruk → Last ut |
| **FoundryModelInfo** | Rik metadata: oppgave, enhet, størrelse, lisens, verktøystøtte for kall |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) for OpenAI-fri bruk |
| **Varianter** | Modeller har maskinvare-spesifikke varianter (CPU, GPU, NPU); velges automatisk |
| **Oppgraderinger** | Python: `is_model_upgradeable()` + `upgrade_model()` for å holde modeller oppdaterte |
| **Katalogoppdatering** | `refresh_catalog()` (Python) / `updateModels()` (JS) for å oppdage nye modeller |

---

## Ressurser

| Ressurs | Lenke |
|----------|------|
| SDK-referanse (alle språk) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrer med inferens SDKer | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API-referanse | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK-eksempler | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local-nettside | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Neste steg

Fortsett til [Del 3: Bruke SDK med OpenAI](part3-sdk-and-apis.md) for å koble SDK til OpenAI-klientbiblioteket og bygge din første chat fullføringsapplikasjon.