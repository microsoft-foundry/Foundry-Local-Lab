![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Časť 2: Hlboký ponor do Foundry Local SDK

> **Cieľ:** Ovládnuť Foundry Local SDK na programové riadenie modelov, služieb a ukladania do vyrovnávacej pamäte - a pochopiť, prečo je SDK odporúčaný prístup pred CLI pre tvorbu aplikácií.

## Prehľad

V časti 1 ste použili **Foundry Local CLI** na stiahnutie a interaktívne spúšťanie modelov. CLI je skvelé na prieskum, ale keď tvoríte skutočné aplikácie, potrebujete **programovateľnú kontrolu**. Foundry Local SDK vám to umožňuje - spravuje **riadiacu rovinu** (spúšťanie služby, objavovanie modelov, sťahovanie, nahrávanie), aby váš aplikačný kód mohol byť zameraný na **dátovú rovinu** (odosielanie promptov, prijímanie výstupov).

Táto laboratórka vás naučí celý povrch API SDK v Pythone, JavaScripte a C#. Na jej konci budete rozumieť každému dostupnému metódu a kedy ktorú použiť.

## Učiace ciele

Na konci tejto laboratórky budete vedieť:

- Vysvetliť, prečo je SDK preferované pred CLI pre vývoj aplikácií
- Nainštalovať Foundry Local SDK pre Python, JavaScript alebo C#
- Použiť `FoundryLocalManager` na spustenie služby, správu modelov a vyhľadávanie v katalógu
- Programovo vypísať, stiahnuť, nahrať a vyložiť modely
- Prezerať metadáta modelu pomocou `FoundryModelInfo`
- Rozumieť rozdielu medzi katalógom, cache a načítanými modelmi
- Použiť bootstrap konštruktor (Python) a vzor `create()` + katalóg (JavaScript)
- Pochopiť prepracovanie C# SDK a jeho objektovo orientované API

---

## Predpoklady

| Požiadavka | Podrobnosti |
|-------------|-------------|
| **Foundry Local CLI** | Nainštalované a v systémovej `PATH` ([Časť 1](part1-getting-started.md)) |
| **Bežné prostredie jazyka** | **Python 3.9+** a/alebo **Node.js 18+** a/alebo **.NET 9.0+** |

---

## Koncept: SDK vs CLI - Prečo použiť SDK?

| Aspekt | CLI (`foundry` príkaz) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Použitie** | Prieskum, manuálne testovanie | Integrácia do aplikácií |
| **Správa služby** | Manuálna: `foundry service start` | Automatická: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Objavovanie portu** | Čítanie z výstupu CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Sťahovanie modelov** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Spracovanie chýb** | Výstupné kódy, stderr | Výnimky, typované chyby |
| **Automatizácia** | Shell skripty | Natívna integrácia s jazykom |
| **Deployment** | Vyžaduje CLI na stroji používateľa | C# SDK je samostatné (bez potreby CLI) |

> **Kľúčová poznámka:** SDK riadi celý životný cyklus: spustenie služby, kontrolu cache, stiahnutie chýbajúcich modelov, ich načítanie a nájdenie endpointu, všetko v niekoľkých riadkoch kódu. Vaša aplikácia nemusí analyzovať výstup CLI ani spravovať podprocesy.

---

## Laboratórne cvičenia

### Cvičenie 1: Inštalácia SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Overte inštaláciu:

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

Overte inštaláciu:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Sú dve NuGet balíčky:

| Balíček | Platforma | Popis |
|---------|----------|-------|
| `Microsoft.AI.Foundry.Local` | Multiplatformový | Funguje na Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Iba Windows | Pridáva WinML hardvérovú akceleráciu; sťahuje a inštaluje pluginy pre poskytovateľov vykonávania (napr. QNN pre Qualcomm NPU) |

**Nastavenie pre Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Upravte `.csproj` súbor:

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

> **Poznámka:** Na Windows je WinML balíček nadmnožina, ktorá obsahuje základné SDK plus QNN poskytovateľa. Na Linux/macOS sa používa len základné SDK. Podmienkové TFM a odkazy na balíčky zabezpečujú plnú multiplatformovosť projektu.

Vytvorte `nuget.config` v koreňovom adresári projektu:

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

Obnovte balíčky:

```bash
dotnet restore
```

</details>

---

### Cvičenie 2: Spustenie služby a výpis katalógu

Prvou vecou, ktorú každá aplikácia robí, je spustenie služby Foundry Local a zistenie dostupných modelov.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Vytvorte manažéra a spustite službu
manager = FoundryLocalManager()
manager.start_service()

# Zoznam všetkých modelov dostupných v katalógu
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Metódy správy služby

| Metóda | Signatúra | Popis |
|--------|-----------|-------|
| `is_service_running()` | `() -> bool` | Skontrolovať, či je služba spustená |
| `start_service()` | `() -> None` | Spustiť Foundry Local službu |
| `service_uri` | `@property -> str` | Základná URI služby |
| `endpoint` | `@property -> str` | API endpoint (URI služby + `/v1`) |
| `api_key` | `@property -> str` | API kľúč (z prostredia alebo predvolený zástupca) |

#### Python SDK - Metódy správy katalógu

| Metóda | Signatúra | Popis |
|--------|-----------|-------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Vypísať všetky modely v katalógu |
| `refresh_catalog()` | `() -> None` | Obnoviť katalóg zo služby |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Získať informácie o konkrétnom modeli |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Vytvorte manažéra a spustite službu
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Prezerať katalóg
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Manažérske metódy

| Metóda | Signatúra | Popis |
|--------|-----------|-------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inicializovať SDK singleton |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Prístup k singleton manažérovi |
| `manager.startWebService()` | `() => Promise<void>` | Spustiť Foundry Local webovú službu |
| `manager.urls` | `string[]` | Pole základných URL služby |

#### JavaScript SDK - Metódy katalógu a modelov

| Metóda | Signatúra | Popis |
|--------|-----------|-------|
| `manager.catalog` | `Catalog` | Prístup ku katalógu modelov |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Získať model objekt podľa aliasu |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK verzie 0.8.0+ používa objektovo orientovanú architektúru s objektmi `Configuration`, `Catalog` a `Model`:

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

#### C# SDK - Kľúčové triedy

| Trieda | Účel |
|--------|-------|
| `Configuration` | Nastavenie mena aplikácie, úrovne logovania, adresára cache, URL web servera |
| `FoundryLocalManager` | Hlavný vstupný bod - vytvorený cez `CreateAsync()`, prístupný cez `.Instance` |
| `Catalog` | Prezeranie, vyhľadávanie a získavanie modelov z katalógu |
| `Model` | Reprezentuje konkrétny model - stiahnutie, načítanie, získanie klientov |

#### C# SDK - Manažérske a katalógové metódy

| Metóda | Popis |
|--------|-------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inicializácia manažéra |
| `FoundryLocalManager.Instance` | Prístup k singleton manažérovi |
| `manager.GetCatalogAsync()` | Získať katalóg modelov |
| `catalog.ListModelsAsync()` | Vypísať všetky dostupné modely |
| `catalog.GetModelAsync(alias: "alias")` | Získať konkrétny model podľa aliasu |
| `catalog.GetCachedModelsAsync()` | Vypísať stiahnuté modely |
| `catalog.GetLoadedModelsAsync()` | Vypísať momentálne načítané modely |

> **Poznámka k architektúre C#:** C# SDK v0.8.0+ je prepracované tak, že je aplikácia **celistvá**; nevyžaduje Foundry Local CLI na stroji koncového používateľa. SDK spravuje modely a inferenciu natívne.

</details>

---

### Cvičenie 3: Stiahnutie a načítanie modelu

SDK rozdeľuje sťahovanie (na disk) od načítania (do pamäti). Toto umožňuje vopred stiahnuť modely počas nastavenia a načítať ich na vyžiadanie.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Možnosť A: Manuálny krok za krokom
manager = FoundryLocalManager()
manager.start_service()

# Najskôr skontrolujte cache
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

# Možnosť B: Jednoradkový bootstrap (odporúčané)
# Predajte alias do konštruktora - automaticky spustí službu, stiahne a načíta
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Metódy správy modelov

| Metóda | Signatúra | Popis |
|--------|-----------|-------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Stiahnuť model do lokálnej cache |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Načítať model do inferenčného servera |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Vyložiť model zo servera |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Vypísať všetky momentálne načítané modely |

#### Python - Metódy správy cache

| Metóda | Signatúra | Popis |
|--------|-----------|-------|
| `get_cache_location()` | `() -> str` | Získať cestu k adresáru cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Vypísať všetky stiahnuté modely |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Postupný prístup
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

#### JavaScript - Metódy modelov

| Metóda | Signatúra | Popis |
|--------|-----------|-------|
| `model.isCached` | `boolean` | Či je model už stiahnutý |
| `model.download()` | `() => Promise<void>` | Stiahnuť model do lokálnej cache |
| `model.load()` | `() => Promise<void>` | Načítať do inferenčného servera |
| `model.unload()` | `() => Promise<void>` | Vyložiť z inferenčného servera |
| `model.id` | `string` | Unikátny identifikátor modelu |

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

#### C# - Metódy modelov

| Metóda | Popis |
|--------|-------|
| `model.DownloadAsync(progress?)` | Stiahnuť vybranú variantu |
| `model.LoadAsync()` | Načítať model do pamäte |
| `model.UnloadAsync()` | Vyložiť model |
| `model.SelectVariant(variant)` | Vybrať špecifickú variantu (CPU/GPU/NPU) |
| `model.SelectedVariant` | Aktuálne vybraná varianta |
| `model.Variants` | Všetky dostupné varianty pre model |
| `model.GetPathAsync()` | Získať lokálnu cestu k súboru |
| `model.GetChatClientAsync()` | Získať natívne chatovacieho klienta (nie je potrebný OpenAI SDK) |
| `model.GetAudioClientAsync()` | Získať audio klienta na prepis |

</details>

---

### Cvičenie 4: Prezretie metadát modelu

Objekt `FoundryModelInfo` obsahuje bohaté metadáta o každom modeli. Pochopenie týchto polí vám pomôže vybrať správny model pre vašu aplikáciu.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Získať podrobné informácie o konkrétnom modeli
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

#### Polia FoundryModelInfo

| Pole | Typ | Popis |
|-------|-----|-------|
| `alias` | string | Krátke meno (napr. `phi-3.5-mini`) |
| `id` | string | Unikátny identifikátor modelu |
| `version` | string | Verzia modelu |
| `task` | string | `chat-completions` alebo `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU alebo NPU |
| `execution_provider` | string | Runtime backend (CUDA, CPU, QNN, WebGPU, atď.) |
| `file_size_mb` | int | Veľkosť na disku v MB |
| `supports_tool_calling` | bool | Či model podporuje volanie funkcií/nástrojov |
| `publisher` | string | Publikujúci model |
| `license` | string | Názov licencie |
| `uri` | string | URI modelu |
| `prompt_template` | dict/null | Šablóna promptu, ak existuje |

---

### Cvičenie 5: Správa životného cyklu modelu

Precvičte si celý životný cyklus: vypísať → stiahnuť → načítať → použiť → vyložiť.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Malý model na rýchle testovanie

manager = FoundryLocalManager()
manager.start_service()

# 1. Skontrolujte, čo je v katalógu
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Skontrolujte, čo už je stiahnuté
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Stiahnite model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Overte, že je teraz v cache
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Načítajte ho
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Skontrolujte, čo je načítané
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Odstráňte ho z pamäte
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

const alias = "qwen2.5-0.5b"; // Malý model pre rýchle testovanie

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Získať model z katalógu
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Stiahnuť, ak je to potrebné
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Načítať ho
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Odstrániť ho z pamäte
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Cvičenie 6: Rýchle štartovacie vzory

Každý jazyk poskytuje skratku na spustenie služby a načítanie modelu v jednom volaní. Toto sú **odporúčané vzory** pre väčšinu aplikácií.

<details>
<summary><h3>🐍 Python - Konštruktor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Predajte alias konštruktéru - postará sa o všetko:
# 1. Spustí službu, ak nie je spustená
# 2. Stiahne model, ak nie je uložený v cache
# 3. Načíta model do inferenčného servera
manager = FoundryLocalManager("phi-3.5-mini")

# Ihneď pripravené na použitie
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Parameter `bootstrap` (predvolené `True`) riadi toto správanie. Nastavte `bootstrap=False`, ak chcete manuálnu kontrolu:

```python
# Manuálny režim - nič sa nedeje automaticky
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalóg</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() rieši všetko:
// 1. Spustí službu
// 2. Získa model z katalógu
// 3. V prípade potreby stiahne a načíta model
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Ihneď pripravené na použitie
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Katalóg</h3></summary>

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

> **Poznámka k C#:** SDK pre C# používa `Configuration` na ovládanie názvu aplikácie, logovania, cache adresárov a dokonca pripnutie konkrétneho portu webového servera. To robí tento SDK najkonfigurovateľnejším z troch SDK.

</details>

---

### Cvičenie 7: Natívny ChatClient (Nie je potrebný OpenAI SDK)

SDK pre JavaScript a C# poskytujú metódu `createChatClient()` pre jednoduché získanie natívneho chat klienta — nie je potrebné inštalovať alebo konfigurovať OpenAI SDK samostatne.

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

// Vytvoriť ChatClient priamo z modelu — nie je potrebný import OpenAI
const chatClient = model.createChatClient();

// completeChat vracia objekt odpovede kompatibilný s OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming používa vzor spätného volania
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` podporuje aj volanie nástrojov — odovzdajte nástroje ako druhý argument:

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

> **Kedy použiť ktorý vzor:**
> - **`createChatClient()`** — Rýchle prototypovanie, menej závislostí, jednoduchší kód
> - **OpenAI SDK** — Plná kontrola nad parametrami (teplota, top_p, stop tokeny atď.), lepšie pre produkčné aplikácie

---

### Cvičenie 8: Varianty Modelov a Výber Hardvéru

Modely môžu mať viacero **variantov** optimalizovaných pre rôzny hardvér. SDK automaticky vyberá najlepší variant, no môžete si ho preskúmať a zvoliť manuálne.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Zoznam všetkých dostupných variantov
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK automaticky vyberie najlepší variant pre váš hardvér
// Ak chcete prepísať, použite selectVariant():
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

V Pythone SDK automaticky vyberá najlepší variant na základe hardvéru. Použite `get_model_info()`, aby ste videli, čo bolo vybrané:

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

#### Modely s NPU Variantmi

Niektoré modely majú varianty optimalizované pre Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Model | Dostupný NPU Variant |
|-------|:---------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Tip:** Na hardvéri podporujúcom NPU SDK automaticky vyberie NPU variant, ak je dostupný. Nie je potrebné meniť váš kód. Pre C# projekty na Windows pridajte NuGet balík `Microsoft.AI.Foundry.Local.WinML`, ktorý povolí QNN execution provider — QNN sa dodáva ako plugin EP cez WinML.

---

### Cvičenie 9: Aktualizácie Modelov a Obnova Katalógu

Katalóg modelov sa pravidelne aktualizuje. Použite tieto metódy na kontrolu a použitie aktualizácií.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Obnovte katalóg, aby ste získali najnovší zoznam modelov
manager.refresh_catalog()

# Skontrolujte, či má uložený model k dispozícii novšiu verziu
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

// Obnovte katalóg na získanie najnovšieho zoznamu modelov
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Zoznam všetkých dostupných modelov po obnovení
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Cvičenie 10: Práca s Modelmi na Rozumovanie

Model **phi-4-mini-reasoning** zahŕňa reťazec myslenia (chain-of-thought reasoning). Obalí svoje vnútorné uvažovanie do tagov `<think>...</think>` pred tým, ako poskytne finálnu odpoveď. Je to užitočné pre úlohy vyžadujúce viackrokovú logiku, matematiku alebo riešenie problémov.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning je približne 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Model obaluje svoje uvažovanie do značiek <think>...</think>
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

// Analyzovať reťazové rozmýšľanie
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Kedy použiť modely na rozumovanie:**
> - Matematické a logické problémy
> - Úlohy s viackrokovým plánovaním
> - Generovanie zložitého kódu
> - Úlohy, kde zobrazenie postupu zvyšuje presnosť
>
> **Nevýhoda:** Modely na rozumovanie produkujú viac tokenov (časť `<think>`) a sú pomalšie. Pre jednoduché otázky a odpovede je štandardný model ako phi-3.5-mini rýchlejší.

---

### Cvičenie 11: Pochopenie Aliasov a Výber Hardvéru

Keď použijete **alias** (napr. `phi-3.5-mini`) namiesto úplného ID modelu, SDK automaticky vyberá najlepší variant podľa vášho hardvéru:

| Hardvér | Vybraný Execution Provider |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (cez WinML plugin) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Akékoľvek zariadenie (fallback) | `CPUExecutionProvider` alebo `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Alias sa prekladá na najlepšiu variantu pre VAŠE hardvér
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tip:** Vždy používajte aliasy vo vašom aplikačnom kóde. Pri nasadení na zariadenie používateľa SDK vyberie optimálny variant za behu - CUDA na NVIDIA, QNN na Qualcomm, CPU inde.

---

### Cvičenie 12: Konfiguračné Možnosti v C#

Trieda `Configuration` v C# SDK poskytuje detailnú kontrolu nad spustením:

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

| Vlastnosť | Predvolené | Popis |
|----------|-------------|-------------|
| `AppName` | (povinné) | Názov vašej aplikácie |
| `LogLevel` | `Information` | Úroveň detailov logovania |
| `Web.Urls` | (dynamické) | Pripnúť konkrétny port pre web server |
| `AppDataDir` | Predvolený OS | Základný adresár pre dáta aplikácie |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Miesto uloženia modelov |
| `LogsDir` | `{AppDataDir}/logs` | Miesto zápisu logov |

---

### Cvičenie 13: Použitie v Prehliadači (len JavaScript)

JavaScript SDK obsahuje verziu kompatibilnú s prehliadačom. V prehliadači musíte manuálne spustiť službu cez CLI a špecifikovať host URL:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Najprv manuálne spustite službu:
//   start foundry služby
// Potom použite URL z výstupu CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Prezrite si katalóg
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Obmedzenia prehliadača:** Verzia prehliadača **nepodporuje** `startWebService()`. Musíte zabezpečiť, aby služba Foundry Local bežala ešte pred použitím SDK v prehliadači.

---

## Kompletná API Referencia

### Python

| Kategória | Metóda | Popis |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Vytvorenie manažéra; voliteľne bootstrap s modelom |
| **Service** | `is_service_running()` | Skontrolovať, či služba beží |
| **Service** | `start_service()` | Spustiť službu |
| **Service** | `endpoint` | URL API endpointu |
| **Service** | `api_key` | API kľúč |
| **Catalog** | `list_catalog_models()` | Zoznam všetkých dostupných modelov |
| **Catalog** | `refresh_catalog()` | Obnoviť katalóg |
| **Catalog** | `get_model_info(alias_or_model_id)` | Získať metadata modelu |
| **Cache** | `get_cache_location()` | Cesta k adresáru cache |
| **Cache** | `list_cached_models()` | Zoznam stiahnutých modelov |
| **Model** | `download_model(alias_or_model_id)` | Stiahnuť model |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Načítať model |
| **Model** | `unload_model(alias_or_model_id)` | Uvoľniť model |
| **Model** | `list_loaded_models()` | Zoznam načítaných modelov |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Skontrolovať dostupnosť novej verzie |
| **Model** | `upgrade_model(alias_or_model_id)` | Aktualizovať model na najnovšiu verziu |
| **Service** | `httpx_client` | Predkonfigurovaný klient HTTPX na priame API volania |

### JavaScript

| Kategória | Metóda | Popis |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Inicializovať SDK singleton |
| **Init** | `FoundryLocalManager.instance` | Prístup k singleton manažérovi |
| **Service** | `manager.startWebService()` | Spustiť webovú službu |
| **Service** | `manager.urls` | Pole základných URL služieb |
| **Catalog** | `manager.catalog` | Prístup ku katalógu modelov |
| **Catalog** | `catalog.getModel(alias)` | Získať objekt modelu podľa aliasu (vracia Promise) |
| **Model** | `model.isCached` | Či je model stiahnutý |
| **Model** | `model.download()` | Stiahnuť model |
| **Model** | `model.load()` | Načítať model |
| **Model** | `model.unload()` | Uvoľniť model |
| **Model** | `model.id` | Jedinečný identifikátor modelu |
| **Model** | `model.alias` | Alias modelu |
| **Model** | `model.createChatClient()` | Získať natívneho chat klienta (nie je potrebný OpenAI SDK) |
| **Model** | `model.createAudioClient()` | Získať audio klienta na prepis |
| **Model** | `model.removeFromCache()` | Odstrániť model z lokálnej cache |
| **Model** | `model.selectVariant(variant)` | Vybrať konkrétny hardvérový variant |
| **Model** | `model.variants` | Pole dostupných variantov pre tento model |
| **Model** | `model.isLoaded()` | Skontrolovať, či je model práve načítaný |
| **Catalog** | `catalog.getModels()` | Zoznam všetkých dostupných modelov |
| **Catalog** | `catalog.getCachedModels()` | Zoznam stiahnutých modelov |
| **Catalog** | `catalog.getLoadedModels()` | Zoznam práve načítaných modelov |
| **Catalog** | `catalog.updateModels()` | Obnoviť katalóg zo služby |
| **Service** | `manager.stopWebService()` | Zastaviť webovú službu Foundry Local |

### C# (v0.8.0+)

| Kategória | Metóda | Popis |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Inicializovať manažéra |
| **Init** | `FoundryLocalManager.Instance` | Prístup k singletonu |
| **Catalog** | `manager.GetCatalogAsync()` | Získať katalóg |
| **Catalog** | `catalog.ListModelsAsync()` | Zoznam všetkých modelov |
| **Catalog** | `catalog.GetModelAsync(alias)` | Získať konkrétny model |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Zoznam modelov v cache |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Zoznam načítaných modelov |
| **Model** | `model.DownloadAsync(progress?)` | Stiahnuť model |
| **Model** | `model.LoadAsync()` | Načítať model |
| **Model** | `model.UnloadAsync()` | Uvoľniť model |
| **Model** | `model.SelectVariant(variant)` | Vybrať hardvérový variant |
| **Model** | `model.GetChatClientAsync()` | Získať natívneho chat klienta |
| **Model** | `model.GetAudioClientAsync()` | Získať audio transkripčný klient |
| **Model** | `model.GetPathAsync()` | Získať lokálnu cestu k súboru |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Získať konkrétny hardvérový variant |
| **Catalog** | `catalog.UpdateModelsAsync()` | Obnoviť katalóg |
| **Server** | `manager.StartWebServerAsync()` | Spustiť REST web server |
| **Server** | `manager.StopWebServerAsync()` | Zastaviť REST web server |
| **Config** | `config.ModelCacheDir` | Adresár cache |

---

## Kľúčové poznatky

| Koncept | Čo ste sa naučili |
|---------|-----------------|
| **SDK vs CLI** | SDK poskytuje programatickú kontrolu - nevyhnutnú pre aplikácie |
| **Riadiaca rovina** | SDK spravuje služby, modely a cache |
| **Dynamické porty** | Vždy používajte `manager.endpoint` (Python) alebo `manager.urls[0]` (JS/C#) - nikdy nepoužívajte natvrdo zadaný port |
| **Alias** | Používajte aliasy pre automatický výber optimálneho variantu podľa hardvéru |
| **Rýchly štart** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Redizajn v C#** | v0.8.0+ je samostatný - na počítačoch používateľov nie je potrebné CLI |
| **Životný cyklus modelu** | Katalóg → Stiahnutie → Načítanie → Použitie → Uvoľnenie |
| **FoundryModelInfo** | Bohaté metadata: úloha, zariadenie, veľkosť, licencia, podpora volania nástrojov |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) na použitie bez OpenAI |
| **Varianty** | Modely majú varianty špecifické pre hardvér (CPU, GPU, NPU); vyberá sa automaticky |
| **Aktualizácie** | Python: `is_model_upgradeable()` + `upgrade_model()` na udržiavanie modelov aktuálnych |
| **Obnovenie katalógu** | `refresh_catalog()` (Python) / `updateModels()` (JS) na objavenie nových modelov |

---

## Zdrojové materiály

| Zdroj | Odkaz |
|----------|------|
| Referencia SDK (všetky jazyky) | [Microsoft Learn - Referencia Foundry Local SDK](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrácia s inference SDK | [Microsoft Learn - Integrácia s Inference SDK](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Referencia API C# SDK | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| Ukážky C# SDK | [GitHub - Ukážky Foundry Local SDK](https://aka.ms/foundrylocalSDK) |
| Webová stránka Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Ďalšie kroky

Pokračujte na [Časť 3: Použitie SDK s OpenAI](part3-sdk-and-apis.md) a prepojte SDK s knižnicou klienta OpenAI a vytvorte svoju prvú aplikáciu na dokončenie chatu.