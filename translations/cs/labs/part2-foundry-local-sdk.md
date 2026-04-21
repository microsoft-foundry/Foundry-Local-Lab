![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Část 2: Hluboký průzkum Foundry Local SDK

> **Cíl:** Ovládnout Foundry Local SDK pro programové řízení modelů, služeb a cachování – a pochopit, proč je SDK doporučovaný přístup oproti CLI pro vývoj aplikací.

## Přehled

V Části 1 jste použili **Foundry Local CLI** ke stažení a interaktivnímu spuštění modelů. CLI je skvělé pro průzkum, ale když vytváříte skutečné aplikace, potřebujete **programovou kontrolu**. Foundry Local SDK vám ji poskytuje – spravuje **řídicí rovinu** (spouštění služby, hledání modelů, stahování, načítání), aby váš aplikační kód mohl řešit **datovou rovinu** (odesílání promptů, přijímání dokončení).

Tento lab vás naučí plný SDK API povrch napříč Pythonem, JavaScriptem a C#. Na konci budete rozumět každé dostupné metodě a kdy ji použít.

## Cíle učení

Na konci tohoto labu budete schopni:

- Vysvětlit, proč je SDK preferované před CLI pro vývoj aplikací
- Nainstalovat Foundry Local SDK pro Python, JavaScript nebo C#
- Použít `FoundryLocalManager` pro spuštění služby, správu modelů a dotazování katalogu
- Programově vypsat, stáhnout, načíst a odložit modely
- Prověřit metadata modelu pomocí `FoundryModelInfo`
- Pochopit rozdíl mezi katalogem, cache a načtenými modely
- Použít konstruktor bootstrap (Python) a vzor `create()` + katalog (JavaScript)
- Pochopit redesign C# SDK a jeho objektově orientované API

---

## Požadavky

| Požadavek | Detaily |
|-----------|---------|
| **Foundry Local CLI** | Nainstalováno a dostupné na vaší `PATH` ([Část 1](part1-getting-started.md)) |
| **Prostředí jazyka** | **Python 3.9+** a/nebo **Node.js 18+** a/nebo **.NET 9.0+** |

---

## Koncept: SDK vs CLI - Proč používat SDK?

| Aspekt | CLI (`foundry` příkaz) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Použití** | Průzkum, manuální testování | Integrace do aplikace |
| **Správa služby** | Manuálně: `foundry service start` | Automaticky: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Nalezení portu** | Čtení z výstupu CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Stažení modelu** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Zpracování chyb** | Exit kódy, stderr | Výjimky, typované chyby |
| **Automatizace** | Skripty shellu | Nativní integrace v jazyce |
| **Nasazení** | Vyžaduje CLI na stroji uživatele | C# SDK může být samostatné (bez potřeby CLI) |

> **Klíčová poznámka:** SDK zvládá celý životní cyklus: spuštění služby, kontrolu cache, stahování chybějících modelů, jejich načtení a zjištění endpointu – to vše v několika řádcích kódu. Vaše aplikace nemusí parsovat výstupy CLI nebo spravovat podprocesy.

---

## Laboratorní cvičení

### Cvičení 1: Instalace SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Ověřte instalaci:

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

Ověřte instalaci:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Existují dva NuGet balíčky:

| Balíček | Platforma | Popis |
|---------|-----------|-------|
| `Microsoft.AI.Foundry.Local` | Víceplatforemní | Funguje na Windows, Linuxu, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Pouze Windows | Přidává WinML hardwarovou akceleraci; stahuje a instaluje pluginy pro vykonávání (např. QNN pro Qualcomm NPU) |

**Nastavení Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Upravte soubor `.csproj`:

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

> **Poznámka:** Na Windows je WinML balíček nadstavbou, která zahrnuje základní SDK plus QNN vykonávací provider. Na Linuxu/macOS se používá základní SDK. Podmíněné TFM a reference balíčků zajišťují plnou multiplatformnost projektu.

Vytvořte `nuget.config` v kořeni projektu:

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

### Cvičení 2: Spuštění služby a výpis katalogu

Nejprve jakákoli aplikace spouští Foundry Local službu a zjišťuje, jaké modely jsou k dispozici.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Vytvořte správce a spusťte službu
manager = FoundryLocalManager()
manager.start_service()

# Vyjmenujte všechny modely dostupné v katalogu
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Metody správy služby

| Metoda | Signatura | Popis |
|--------|-----------|-------|
| `is_service_running()` | `() -> bool` | Ověří, zda služba běží |
| `start_service()` | `() -> None` | Spustí Foundry Local službu |
| `service_uri` | `@property -> str` | Základní URI služby |
| `endpoint` | `@property -> str` | API endpoint (URI služby + `/v1`) |
| `api_key` | `@property -> str` | API klíč (ze proměnných prostředí nebo výchozí placeholder) |

#### Python SDK - Metody správy katalogu

| Metoda | Signatura | Popis |
|--------|-----------|-------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Výpis všech modelů v katalogu |
| `refresh_catalog()` | `() -> None` | Aktualizace katalogu ze služby |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Získání informací o konkrétním modelu |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Vytvořte správce a spusťte službu
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Prohlédněte si katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Metody manažera

| Metoda | Signatura | Popis |
|--------|-----------|-------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inicializace SDK singletonu |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Přístup k singleton manažeru |
| `manager.startWebService()` | `() => Promise<void>` | Spuštění Foundry Local webové služby |
| `manager.urls` | `string[]` | Pole základních URL služeb |

#### JavaScript SDK - Metody katalogu a modelu

| Metoda | Signatura | Popis |
|--------|-----------|-------|
| `manager.catalog` | `Catalog` | Přístup ke katalogu modelů |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Získání objektu modelu podle aliasu |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK ve verzi v0.8.0+ používá objektově orientovanou architekturu s objekty `Configuration`, `Catalog` a `Model`:

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

#### C# SDK - Klíčové třídy

| Třída | Účel |
|-------|-------|
| `Configuration` | Nastavení jména aplikace, logovací úrovně, cache adresáře, URL web serveru |
| `FoundryLocalManager` | Hlavní vstupní bod – vytváří se přes `CreateAsync()`, přístup přes `.Instance` |
| `Catalog` | Procházení, hledání a získávání modelů z katalogu |
| `Model` | Reprezentuje konkrétní model – stahování, načítání, získání klientů |

#### C# SDK - Metody manažera a katalogu

| Metoda | Popis |
|--------|-------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inicializace manažera |
| `FoundryLocalManager.Instance` | Přístup k singleton manažeru |
| `manager.GetCatalogAsync()` | Získání katalogu modelů |
| `catalog.ListModelsAsync()` | Výpis všech dostupných modelů |
| `catalog.GetModelAsync(alias: "alias")` | Získání modelu podle aliasu |
| `catalog.GetCachedModelsAsync()` | Výpis stažených modelů |
| `catalog.GetLoadedModelsAsync()` | Výpis aktuálně načtených modelů |

> **C# Architektonická poznámka:** Redesign C# SDK ve verzi v0.8.0+ umožňuje, že aplikace je **samostatná**; nevyžaduje Foundry Local CLI na koncovém zařízení. SDK nativně spravuje modely a inference.

</details>

---

### Cvičení 3: Stažení a načtení modelu

SDK odděluje stahování (na disk) od načtení (do paměti). To umožňuje předběžně stáhnout modely při nastavení a načíst je podle potřeby.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Možnost A: Manuální krok za krokem
manager = FoundryLocalManager()
manager.start_service()

# Nejprve zkontrolujte cache
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

# Možnost B: Jednořádkový bootstrap (doporučeno)
# Předat alias konstruktoru - automaticky spustí službu, stáhne a načte
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Metody správy modelu

| Metoda | Signatura | Popis |
|--------|-----------|-------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Stáhne model do lokální cache |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Načte model do inference serveru |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Odloží model ze serveru |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Výpis všech aktuálně načtených modelů |

#### Python - Metody správy cache

| Metoda | Signatura | Popis |
|--------|-----------|-------|
| `get_cache_location()` | `() -> str` | Cesta k adresáři cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Výpis všech stažených modelů |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Postupný přístup
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

#### JavaScript - Metody modelu

| Metoda | Signatura | Popis |
|--------|-----------|-------|
| `model.isCached` | `boolean` | Jestli je model již stažený |
| `model.download()` | `() => Promise<void>` | Stáhne model do lokální cache |
| `model.load()` | `() => Promise<void>` | Načte do inference serveru |
| `model.unload()` | `() => Promise<void>` | Odloží ze inference serveru |
| `model.id` | `string` | Jedinečný identifikátor modelu |

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

#### C# - Metody modelu

| Metoda | Popis |
|--------|-------|
| `model.DownloadAsync(progress?)` | Stáhne vybranou variantu |
| `model.LoadAsync()` | Načte model do paměti |
| `model.UnloadAsync()` | Odloží model |
| `model.SelectVariant(variant)` | Vybere konkrétní variantu (CPU/GPU/NPU) |
| `model.SelectedVariant` | Aktuálně vybraná varianta |
| `model.Variants` | Všechny dostupné varianty modelu |
| `model.GetPathAsync()` | Získá lokální cestu k souboru |
| `model.GetChatClientAsync()` | Získá nativního chat klienta (bez potřeby OpenAI SDK) |
| `model.GetAudioClientAsync()` | Získá audio klienta pro přepis |

</details>

---

### Cvičení 4: Prohlédnutí metadat modelu

Objekt `FoundryModelInfo` obsahuje podrobné metadata o každém modelu. Pochopení těchto polí pomůže vybrat správný model pro vaši aplikaci.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Získejte podrobné informace o konkrétním modelu
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

#### Polí FoundryModelInfo

| Pole | Typ | Popis |
|-------|-----|-------|
| `alias` | string | Krátký název (např. `phi-3.5-mini`) |
| `id` | string | Jedinečný identifikátor modelu |
| `version` | string | Verze modelu |
| `task` | string | `chat-completions` nebo `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU nebo NPU |
| `execution_provider` | string | Runtime backend (CUDA, CPU, QNN, WebGPU, atd.) |
| `file_size_mb` | int | Velikost na disku v MB |
| `supports_tool_calling` | bool | Jestli model podporuje volání funkcí/nástrojů |
| `publisher` | string | Kdo model publikoval |
| `license` | string | Jméno licence |
| `uri` | string | URI modelu |
| `prompt_template` | dict/null | Vzorek promptu, pokud existuje |

---

### Cvičení 5: Správa životního cyklu modelu

Procvičte si celý životní cyklus: vypiš → stáhni → načti → použij → odlož.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Malý model pro rychlé testování

manager = FoundryLocalManager()
manager.start_service()

# 1. Zkontrolujte, co je v katalogu
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Zkontrolujte, co je již staženo
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Stáhněte model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Ověřte, že je nyní v mezipaměti
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Načtěte ho
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Zkontrolujte, co je načteno
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Odstraňte ho z paměti
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

const alias = "qwen2.5-0.5b"; // Malý model pro rychlé testování

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Získejte model z katalogu
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Stáhněte, pokud je to potřeba
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Načtěte ho
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Uvolněte ho
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Cvičení 6: Rychlé startovací vzorce

Každý jazyk poskytuje zkratku pro spuštění služby a načtení modelu jedním voláním. Toto jsou **doporučené vzorce** pro většinu aplikací.

<details>
<summary><h3>🐍 Python - Bootstrap konstruktoru</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Předat alias konstruktoru - vše se vyřídí:
# 1. Spustí službu, pokud neběží
# 2. Stáhne model, pokud není v cache
# 3. Nahraje model do inference serveru
manager = FoundryLocalManager("phi-3.5-mini")

# Okamžitě připraveno k použití
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Parametr `bootstrap` (výchozí `True`) řídí toto chování. Nastavte `bootstrap=False`, pokud chcete manuální kontrolu:

```python
# Manuální režim - nic se nedeje automaticky
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() řeší vše:
// 1. Spustí službu
// 2. Získá model z katalogu
// 3. Stáhne, pokud je potřeba, a načte model
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Ihned připraven k použití
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

> **Poznámka k C#:** SDK pro C# používá `Configuration` pro nastavení názvu aplikace, logování, cache adresářů a dokonce pro zafixování konkrétního portu webového serveru. Díky tomu je nejkonfigurovatelnější ze všech tří SDK.

</details>

---

### Cvičení 7: Nativní ChatClient (Není potřeba OpenAI SDK)

SDK pro JavaScript a C# poskytují pohodlnou metodu `createChatClient()`, která vrací nativního chat klienta — není potřeba samostatné instalace nebo konfigurace OpenAI SDK.

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

// Vytvořte ChatClient přímo z modelu — není potřeba importovat OpenAI
const chatClient = model.createChatClient();

// completeChat vrací objekt odpovědi kompatibilní s OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming používá vzor callbacku
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` také podporuje volání nástrojů — předávejte nástroje jako druhý argument:

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

> **Kdy použít který vzorec:**
> - **`createChatClient()`** — Rychlé prototypování, méně závislostí, jednodušší kód
> - **OpenAI SDK** — Plná kontrola nad parametry (teplota, top_p, stop tokeny atd.), vhodnější pro produkční aplikace

---

### Cvičení 8: Varianty modelů a výběr hardwaru

Modely mohou mít více **variant** optimalizovaných pro různé hardwarové konfigurace. SDK automaticky vybírá nejlepší variantu, ale můžete si ji také sami prohlédnout a vybrat.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Vypsat všechny dostupné varianty
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK automaticky vybere nejlepší variantu pro váš hardware
// Pro přepsání použijte selectVariant():
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

V Pythonu SDK automaticky vybírá nejlepší variantu podle hardware. Pomocí `get_model_info()` zjistíte, co bylo vybráno:

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

#### Modely s NPU variantami

Některé modely mají NPU-optimalizované varianty pro zařízení s Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Model | Dostupná NPU varianta |
|-------|:---------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Tip:** Na zařízeních s NPU SDK automaticky vybírá NPU variantu, pokud je dostupná. Nemusíte měnit svůj kód. Pro C# projekty na Windows přidejte NuGet balíček `Microsoft.AI.Foundry.Local.WinML`, který umožní použití QNN execution providera — QNN je dodáván jako plugin EP přes WinML.

---

### Cvičení 9: Aktualizace modelů a obnovení katalogu

Katalog modelů je pravidelně aktualizován. Použijte tyto metody pro kontrolu a aplikaci aktualizací.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Aktualizujte katalog, abyste získali nejnovější seznam modelů
manager.refresh_catalog()

# Zkontrolujte, zda má uložený model novější dostupnou verzi
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

// Aktualizujte katalog pro získání nejnovějšího seznamu modelů
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Vypsat všechny dostupné modely po aktualizaci
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Cvičení 10: Práce s modely pro deduktivní uvažování

Model **phi-4-mini-reasoning** zahrnuje logické uvažování pomocí řetězce myšlení. Svůj interní proces myšlení obaluje do značek `<think>...</think>` před vyprodukováním konečné odpovědi. To je užitečné pro úkoly vyžadující vícekrokovou logiku, matematiku nebo řešení problémů.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning je ~4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Model obaluje své myšlení do značek <think>...</think>
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

// Analyzovat myšlení řetězce myšlenek
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Kdy použít modely pro uvažování:**
> - Matematické a logické problémy
> - Vícekrokové plánovací úkoly
> - Generování složitějšího kódu
> - Úkoly, kde zobrazení postupu zvyšuje přesnost
>
> **Kompromis:** Modely pro uvažování generují více tokenů (část `<think>`) a jsou pomalejší. Pro jednoduché otázky a odpovědi je rychlejší standardní model jako phi-3.5-mini.

---

### Cvičení 11: Pochopení aliasů a výběr hardwaru

Když předáte **alias** (např. `phi-3.5-mini`) místo plného ID modelu, SDK automaticky vybere nejlepší variantu pro váš hardware:

| Hardware | Vybraný execution provider |
|----------|----------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (přes WinML plugin) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Jakékoli zařízení (fallback) | `CPUExecutionProvider` nebo `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Alias odkazuje na nejlepší variantu pro VAŠE hardware
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tip:** Vždy používejte aliasy ve vašem aplikačním kódu. Při nasazení na uživatelovo zařízení SDK vybere optimální variantu za běhu — CUDA na NVIDIA, QNN na Qualcomm, CPU jinde.

---

### Cvičení 12: Možnosti konfigurace v C#

Třída `Configuration` v C# SDK poskytuje detailní kontrolu nad během:

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

| Vlastnost | Výchozí hodnota | Popis |
|----------|---------------|-------|
| `AppName` | (povinné) | Název vaší aplikace |
| `LogLevel` | `Information` | Úroveň logování |
| `Web.Urls` | (dynamické) | Fixace konkrétního portu webového serveru |
| `AppDataDir` | výchozí OS | Základní adresář pro data aplikace |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Umístění uložených modelů |
| `LogsDir` | `{AppDataDir}/logs` | Kam se ukládají logy |

---

### Cvičení 13: Použití v prohlížeči (pouze JavaScript)

JavaScript SDK obsahuje verzi kompatibilní s prohlížečem. V prohlížeči musíte službu spustit ručně přes CLI a zadat URL hostitele:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Nejprve ručně spusťte službu:
//   foundry service start
// Poté použijte URL z výstupu CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Procházejte katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Omezení prohlížeče:** Verze prohlížeče **nepodporuje** `startWebService()`. Musíte zajistit, že služba Foundry Local běží před použitím SDK v prohlížeči.

---

## Kompletní API Reference

### Python

| Kategorie | Metoda | Popis |
|----------|--------|-------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Vytvoření správce; volitelně bootstrap s modelem |
| **Service** | `is_service_running()` | Kontrola běhu služby |
| **Service** | `start_service()` | Spuštění služby |
| **Service** | `endpoint` | URL API endpointu |
| **Service** | `api_key` | API klíč |
| **Catalog** | `list_catalog_models()` | Výpis všech dostupných modelů |
| **Catalog** | `refresh_catalog()` | Obnovení katalogu |
| **Catalog** | `get_model_info(alias_or_model_id)` | Získání metadat modelu |
| **Cache** | `get_cache_location()` | Cesta ke cache adresáři |
| **Cache** | `list_cached_models()` | Výpis stažených modelů |
| **Model** | `download_model(alias_or_model_id)` | Stažení modelu |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Načtení modelu |
| **Model** | `unload_model(alias_or_model_id)` | Uvolnění modelu |
| **Model** | `list_loaded_models()` | Výpis načtených modelů |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Kontrola, jestli je k dispozici novější verze |
| **Model** | `upgrade_model(alias_or_model_id)` | Upgrade modelu na nejnovější verzi |
| **Service** | `httpx_client` | Přednastavený HTTPX klient pro přímá API volání |

### JavaScript

| Kategorie | Metoda | Popis |
|----------|--------|-------|
| **Init** | `FoundryLocalManager.create(options)` | Inicializace SDK singletonu |
| **Init** | `FoundryLocalManager.instance` | Přístup k singleton správci |
| **Service** | `manager.startWebService()` | Spuštění webové služby |
| **Service** | `manager.urls` | Pole základních URL služby |
| **Catalog** | `manager.catalog` | Přístup ke katalogu modelů |
| **Catalog** | `catalog.getModel(alias)` | Získání modelu podle aliasu (vrací Promise) |
| **Model** | `model.isCached` | Zda je model stažen |
| **Model** | `model.download()` | Stažení modelu |
| **Model** | `model.load()` | Načtení modelu |
| **Model** | `model.unload()` | Uvolnění modelu |
| **Model** | `model.id` | Unikátní identifikátor modelu |
| **Model** | `model.alias` | Alias modelu |
| **Model** | `model.createChatClient()` | Získání nativního chat klienta (bez potřeby OpenAI SDK) |
| **Model** | `model.createAudioClient()` | Získání audio klienta pro přepis |
| **Model** | `model.removeFromCache()` | Odstranění modelu z lokální cache |
| **Model** | `model.selectVariant(variant)` | Výběr konkrétní hardwarové varianty |
| **Model** | `model.variants` | Pole dostupných variant modelu |
| **Model** | `model.isLoaded()` | Kontrola, zda je model aktuálně načtený |
| **Catalog** | `catalog.getModels()` | Výpis všech dostupných modelů |
| **Catalog** | `catalog.getCachedModels()` | Výpis stažených modelů |
| **Catalog** | `catalog.getLoadedModels()` | Výpis aktuálně načtených modelů |
| **Catalog** | `catalog.updateModels()` | Obnovení katalogu ze služby |
| **Service** | `manager.stopWebService()` | Zastavení Foundry Local webové služby |

### C# (verze 0.8.0+)

| Kategorie | Metoda | Popis |
|----------|--------|-------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Inicializace správce |
| **Init** | `FoundryLocalManager.Instance` | Přístup ke singletonu |
| **Catalog** | `manager.GetCatalogAsync()` | Získání katalogu |
| **Catalog** | `catalog.ListModelsAsync()` | Výpis všech modelů |
| **Catalog** | `catalog.GetModelAsync(alias)` | Získání konkrétního modelu |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Výpis stažených modelů |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Výpis načtených modelů |
| **Model** | `model.DownloadAsync(progress?)` | Stažení modelu |
| **Model** | `model.LoadAsync()` | Načtení modelu |
| **Model** | `model.UnloadAsync()` | Uvolnění modelu |
| **Model** | `model.SelectVariant(variant)` | Výběr hardwarové varianty |
| **Model** | `model.GetChatClientAsync()` | Získání nativního chat klienta |
| **Model** | `model.GetAudioClientAsync()` | Získání audio transkripčního klienta |
| **Model** | `model.GetPathAsync()` | Získání lokální cesty k souboru |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Získání konkrétní hardwarové varianty |
| **Catalog** | `catalog.UpdateModelsAsync()` | Obnovení katalogu |
| **Server** | `manager.StartWebServerAsync()` | Spuštění REST web serveru |
| **Server** | `manager.StopWebServerAsync()` | Zastavení REST web serveru |
| **Config** | `config.ModelCacheDir` | Adresář cache |

---

## Klíčové poznatky

| Koncept | Co jste se naučili |
|---------|--------------------|
| **SDK vs CLI** | SDK poskytuje programatickou kontrolu — nezbytné pro aplikace |
| **Řídicí rovina** | SDK spravuje služby, modely a cache |
| **Dynamické porty** | Vždy používejte `manager.endpoint` (Python) nebo `manager.urls[0]` (JS/C#) — nikdy nezadávejte port tvrdě do kódu |
| **Alias** | Používejte aliasy pro automatický výběr hardwarově optimálního modelu |
| **Rychlý start** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Přepracování C#** | v0.8.0+ je soběstačné - nepotřebuje CLI na zařízeních uživatelů |
| **Životní cyklus modelu** | Katalog → Stažení → Načtení → Použití → Uvolnění |
| **FoundryModelInfo** | Bohatá metadata: úkol, zařízení, velikost, licence, podpora nástroje volajícího |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) pro použití bez OpenAI |
| **Varianty** | Modely mají hardwarově specifické varianty (CPU, GPU, NPU); automaticky vybrané |
| **Aktualizace** | Python: `is_model_upgradeable()` + `upgrade_model()` pro udržení modelů aktuálních |
| **Obnovení katalogu** | `refresh_catalog()` (Python) / `updateModels()` (JS) pro zjištění nových modelů |

---

## Zdroje

| Zdroj | Odkaz |
|----------|------|
| SDK Reference (všechny jazyky) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrace s inference SDK | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API Reference | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK Ukázky | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Další kroky

Pokračujte na [Část 3: Použití SDK s OpenAI](part3-sdk-and-apis.md) pro připojení SDK k OpenAI klientské knihovně a vytvoření vaší první aplikace pro dokončení chatu.