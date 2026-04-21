![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 2: Poglobljen pregled Foundry Local SDK

> **Cilj:** Obvladati Foundry Local SDK za upravljanje modelov, storitev in začasnega shranjevanja programatično – ter razumeti, zakaj je SDK priporočljiv pristop pred CLI za gradnjo aplikacij.

## Pregled

V Delu 1 ste uporabljali **Foundry Local CLI** za prenos in interaktivno uporabo modelov. CLI je odličen za raziskovanje, vendar pa za izdelavo resničnih aplikacij potrebujete **programatičen nadzor**. Foundry Local SDK vam to omogoča – upravlja **nadzorno ravnino** (zagon storitve, iskanje modelov, prenos, nalaganje), da se lahko vaša aplikacijska koda osredotoči na **podatkovno ravnino** (pošiljanje pozivov, prejemanje zaključkov).

Ta laboratorij vam predstavi celotno površino API-ja SDK-ja v Pythonu, JavaScriptu in C#. Na koncu boste razumeli vsak razpoložljiv način in kdaj ga uporabiti.

## Cilji učenja

Na koncu tega laboratorija boste znali:

- Razložiti, zakaj je SDK prednosten pred CLI za razvoj aplikacij
- Namestiti Foundry Local SDK za Python, JavaScript ali C#
- Uporabiti `FoundryLocalManager` za zagon storitve, upravljanje modelov in poizvedovanje po katalogu
- Programatično seznanjati, prenašati, nalagati in odlagati modele
- Pregledovati metapodatke modelov z `FoundryModelInfo`
- Razumeti razliko med katalogom, predpomnilnikom in naloženimi modeli
- Uporabiti konstruktor bootstrap (Python) in vzorec `create()` + katalog (JavaScript)
- Razumeti prenovo SDK-ja za C# in njegov objektno usmerjen API

---

## Predpogoji

| Zahteva | Podrobnosti |
|---------|-------------|
| **Foundry Local CLI** | Nameščen in na vaši poti `PATH` ([Del 1](part1-getting-started.md)) |
| **Okolje izvajanja jezika** | **Python 3.9+** in/ali **Node.js 18+** in/ali **.NET 9.0+** |

---

## Koncept: SDK vs CLI – Zakaj uporabiti SDK?

| Vidik | CLI (`foundry` ukaz) | SDK (`foundry-local-sdk`) |
|-------|----------------------|---------------------------|
| **Uporaba** | Raziskovanje, ročno testiranje | Integracija aplikacije |
| **Upravljanje storitve** | Ročno: `foundry service start` | Avtomatsko: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Iskanje vrat** | Branje iz izhoda CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Prenos modelov** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Ravnanje z napakami** | Izhodne kode, stderr | Izjeme, tipizirane napake |
| **Avtomatizacija** | Shell skripte | Avtomatizacija v domačem jeziku |
| **Namestitev** | Zahteva CLI na končnem računalniku | C# SDK je samostojen (CLI ni potreben) |

> **Ključni vtis:** SDK upravlja celoten življenjski cikel: zagon storitve, preverjanje predpomnilnika, prenos manjkajočih modelov, njihovo nalaganje in iskanje končne točke, vse v nekaj vrsticah kode. Vaša aplikacija ne potrebuje razčlenjevanja izhoda CLI ali upravljanja podprocesov.

---

## Laboratorijske vaje

### Vaja 1: Namestitev SDK-ja

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Preverite namestitev:

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

Preverite namestitev:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Na voljo sta dva NuGet paketa:

| Paket | Platforma | Opis |
|-------|-----------|-------|
| `Microsoft.AI.Foundry.Local` | Večplatformni | Deluje na Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Samo Windows | Dodaja strojno pohitritev WinML; prenese in namesti izvajalne vtičnike (npr. QNN za Qualcomm NPU) |

**Nastavitev za Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Uredite datoteko `.csproj`:

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

> **Opomba:** Na Windows je paket WinML superskupina, ki vključuje osnovni SDK in QNN izvajalnega ponudnika. Na Linuxu/macOS se uporablja osnovni SDK. Pogojni TFM in reference paketov omogočajo popolno večplatformnost projekta.

Ustvarite `nuget.config` v korenu projekta:

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

Obnovite pakete:

```bash
dotnet restore
```

</details>

---

### Vaja 2: Zagon storitve in seznam kataloga

Prva stvar, ki jo aplikacija naredi, je zagon Foundry Local storitve in ugotavljanje razpoložljivih modelov.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Ustvarite upravitelja in zaženite storitev
manager = FoundryLocalManager()
manager.start_service()

# Naštejte vse modele, ki so na voljo v katalogu
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Metode za upravljanje storitve

| Metoda | Podpis | Opis |
|--------|--------|------|
| `is_service_running()` | `() -> bool` | Preveri, ali teče storitev |
| `start_service()` | `() -> None` | Zažene Foundry Local storitev |
| `service_uri` | `@property -> str` | Osnovni URI storitve |
| `endpoint` | `@property -> str` | API končna točka (service URI + `/v1`) |
| `api_key` | `@property -> str` | API ključ (iz okolja ali privzeti nadomestek) |

#### Python SDK - Metode za upravljanje kataloga

| Metoda | Podpis | Opis |
|--------|--------|------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Prikaže vse modele iz kataloga |
| `refresh_catalog()` | `() -> None` | Osveži katalog iz storitve |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Pridobi podatke za določen model |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Ustvarite upravitelja in zaženite storitev
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Brskajte po katalogu
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Metode upravitelja

| Metoda | Podpis | Opis |
|--------|--------|------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inicializira singleton SDK-ja |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Dostop do singleton upravitelja |
| `manager.startWebService()` | `() => Promise<void>` | Zažene Foundry Local spletno storitev |
| `manager.urls` | `string[]` | Polje osnovnih URL-jev storitve |

#### JavaScript SDK - Metode za katalog in modele

| Metoda | Podpis | Opis |
|--------|--------|------|
| `manager.catalog` | `Catalog` | Dostop do kataloga modelov |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Pridobi model po aliasu |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ uporablja objektno usmerjeno arhitekturo s predmeti `Configuration`, `Catalog` in `Model`:

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

#### C# SDK - Ključne razrede

| Razred | Namen |
|--------|-------|
| `Configuration` | Nastavi ime aplikacije, raven dnevnika, mapo za predpomnilnik, spletne URL-je strežnika |
| `FoundryLocalManager` | Glavna vstopna točka – ustvarjen prek `CreateAsync()`, dostopen prek `.Instance` |
| `Catalog` | Brskanje, iskanje in pridobivanje modelov iz kataloga |
| `Model` | Predstavlja določen model – prenos, nalaganje, pridobivanje klientov |

#### C# SDK - Metode upravitelja in kataloga

| Metoda | Opis |
|--------|------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inicializira upravitelja |
| `FoundryLocalManager.Instance` | Dostop do singleton upravitelja |
| `manager.GetCatalogAsync()` | Pridobi katalog modelov |
| `catalog.ListModelsAsync()` | Seznam vseh razpoložljivih modelov |
| `catalog.GetModelAsync(alias: "alias")` | Pridobi določen model po aliasu |
| `catalog.GetCachedModelsAsync()` | Seznam prenesenih modelov |
| `catalog.GetLoadedModelsAsync()` | Seznam trenutno naloženih modelov |

> **Opomba glede arhitekture C#:** Prenova C# SDK v0.8.0+ naredi aplikacijo **samostojno**; ni potrebe po Foundry Local CLI na uporabniškem računalniku. SDK nativno upravlja modele in inferenco.

</details>

---

### Vaja 3: Prenos in nalaganje modela

SDK ločuje prenos (na disk) od nalaganja (v pomnilnik). Tako lahko modele predhodno prenesete med namestitvijo in jih naložite po potrebi.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Možnost A: Ročni korak za korakom
manager = FoundryLocalManager()
manager.start_service()

# Najprej preverite predpomnilnik
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

# Možnost B: Enovrstični bootstrap (priporočeno)
# Posredujte alias konstruktorju - ta samodejno zažene storitev, prenese in naloži
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Metode za upravljanje modelov

| Metoda | Podpis | Opis |
|--------|--------|------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Prenese model v lokalni predpomnilnik |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Naloži model v strežnik za inferenco |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Odloži model iz strežnika |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Prikaže seznam trenutno naloženih modelov |

#### Python - Metode za upravljanje predpomnilnika

| Metoda | Podpis | Opis |
|--------|--------|------|
| `get_cache_location()` | `() -> str` | Pridobi pot do mape predpomnilnika |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Prikaže seznam vseh prenesenih modelov |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Pristop korak za korakom
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

#### JavaScript - Metode modelov

| Metoda | Podpis | Opis |
|--------|--------|------|
| `model.isCached` | `boolean` | Ali je model že prenesen |
| `model.download()` | `() => Promise<void>` | Prenese model v lokalni predpomnilnik |
| `model.load()` | `() => Promise<void>` | Naloži v strežnik za inferenco |
| `model.unload()` | `() => Promise<void>` | Odloži iz strežnika za inferenco |
| `model.id` | `string` | Enolični identifikator modela |

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

#### C# - Metode modelov

| Metoda | Opis |
|--------|------|
| `model.DownloadAsync(progress?)` | Prenese izbrano varianto |
| `model.LoadAsync()` | Naloži model v pomnilnik |
| `model.UnloadAsync()` | Odloži model |
| `model.SelectVariant(variant)` | Izbere določeno varianto (CPU/GPU/NPU) |
| `model.SelectedVariant` | Trenutno izbrana varianta |
| `model.Variants` | Vse razpoložljive variante za ta model |
| `model.GetPathAsync()` | Pridobi lokalno datotečno pot |
| `model.GetChatClientAsync()` | Pridobi nativni klepetalni klient (ni potreben OpenAI SDK) |
| `model.GetAudioClientAsync()` | Pridobi audio klient za transkripcijo |

</details>

---

### Vaja 4: Pregled metapodatkov modela

Objekt `FoundryModelInfo` vsebuje bogate metapodatke o vsakem modelu. Razumevanje teh polj pomaga izbrati pravi model za vašo aplikacijo.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Pridobite podrobne informacije o določenem modelu
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

#### Polja FoundryModelInfo

| Polje | Tip | Opis |
|-------|-----|------|
| `alias` | string | Kratko ime (npr. `phi-3.5-mini`) |
| `id` | string | Enolični identifikator modela |
| `version` | string | Verzija modela |
| `task` | string | `chat-completions` ali `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU ali NPU |
| `execution_provider` | string | Izvajalno okolje (CUDA, CPU, QNN, WebGPU, itd.) |
| `file_size_mb` | int | Velikost na disku v MB |
| `supports_tool_calling` | bool | Ali model podpira klic funkcij/orodij |
| `publisher` | string | Izdajatelj modela |
| `license` | string | Ime licence |
| `uri` | string | URI modela |
| `prompt_template` | dict/null | Predloga poziva, če obstaja |

---

### Vaja 5: Upravljanje življenjskega cikla modela

Vadite celoten življenjski cikel: seznam → prenos → nalaganje → uporaba → odlaganje.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Majhen model za hitro testiranje

manager = FoundryLocalManager()
manager.start_service()

# 1. Preveri, kaj je v katalogu
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Preveri, kaj je že preneseno
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Prenesi model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Preveri, ali je zdaj v predpomnilniku
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Naloži ga
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Preveri, kaj je naloženo
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Izprazni ga
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

const alias = "qwen2.5-0.5b"; // Majhen model za hitro testiranje

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Pridobite model iz kataloga
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Prenesite, če je potrebno
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Naložite ga
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Izpraznite ga
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Vaja 6: Vzorec za hiter začetek

Vsak jezik ponuja bližnjico za zagon storitve in nalaganje modela v enem klicu. To so **priporočeni vzorci** za večino aplikacij.

<details>
<summary><h3>🐍 Python - Konstruktor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Posredujte vzdevek konstruktorju - vse uredi:
# 1. Zažene storitev, če ni zagnana
# 2. Prenese model, če ni shranjen v predpomnilniku
# 3. Naloži model v strežnik za sklepanje
manager = FoundryLocalManager("phi-3.5-mini")

# Takojšnje uporabo pripravljen
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Parameter `bootstrap` (privzeto `True`) nadzoruje to vedenje. Nastavite `bootstrap=False`, če želite ročni nadzor:

```python
# Ročni način - nič se ne zgodi samodejno
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() obvladuje vse:
// 1. Zažene storitev
// 2. Pridobi model iz kataloga
// 3. Po potrebi prenese in naloži model
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Takoj pripravljen za uporabo
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

> **Opomba za C#:** C# SDK uporablja `Configuration` za nadzor imena aplikacije, beleženja, predpomnilniških imenikov in celo določanje specifične spletne vratnice. To ga naredi najbolj nastavljivega med tremi SDK-ji.

</details>

---

### Vaja 7: Native ChatClient (Ni potrebnega OpenAI SDK)

JavaScript in C# SDK-ja nudita priročno metodo `createChatClient()`, ki vrne native klepetalni odjemalec — ni potrebe po posebni namestitvi ali konfiguraciji OpenAI SDK.

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

// Ustvari ChatClient neposredno iz modela — brez potrebe po uvozu OpenAI
const chatClient = model.createChatClient();

// completeChat vrne objekt odgovora, združljiv z OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Pretakanje uporablja vzorec klica nazaj
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` podpira tudi klic orodij — kot drugi argument predajte orodja:

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

> **Kdaj uporabiti kateri vzorec:**
> - **`createChatClient()`** — Hitra prototipizacija, manj odvisnosti, preprostejša koda
> - **OpenAI SDK** — Poln nadzor nad parametri (temperature, top_p, stop tokeni itd.), bolj primerno za produkcijske aplikacije

---

### Vaja 8: Modelske variante in izbira strojne opreme

Modeli imajo lahko več **variant**, optimiziranih za različne strojne opreme. SDK samodejno izbere najboljšo varianto, vendar lahko tudi ročno preverite in izberete.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Naštejte vse razpoložljive variante
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK samodejno izbere najboljšo varianto za vašo strojno opremo
// Za preklic uporabite selectVariant():
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

V Pythonu SDK samodejno izbere najboljšo varianto glede na strojno opremo. Uporabite `get_model_info()` za ogled izbrane:

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

#### Modeli z NPU variantami

Nekateri modeli imajo NPU-optimirane variante za naprave z Nevronskimi procesorskimi enotami (Qualcomm Snapdragon, Intel Core Ultra):

| Model | NPU varianta na voljo |
|-------|:---------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Namig:** Na strojni opremi z NPU SDK samodejno izbere NPU varianto, ko je na voljo. Ni vam treba spreminjati kode. Za C# projekte na Windows dodajte NuGet paket `Microsoft.AI.Foundry.Local.WinML`, da omogočite izvajalca QNN — QNN je dostavljen kot vtičnik EP prek WinML.

---

### Vaja 9: Nadgradnje modelov in osvežitev kataloga

Katalog modelov se periodično posodablja. Uporabite te metode za preverjanje in uporabo posodobitev.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Osvežite katalog, da dobite najnovejši seznam modelov
manager.refresh_catalog()

# Preverite, ali ima predpomnjeni model na voljo novejšo različico
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

// Osvežite katalog, da pridobite najnovejši seznam modelov
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Naštejte vse razpoložljive modele po osvežitvi
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Vaja 10: Delo z modeli za sklepanje

Model **phi-4-mini-reasoning** vključuje sklepanjsko razmišljanje. Notranje misli ovije v oznake `<think>...</think>` preden poda končni odgovor. To je uporabno za naloge, ki zahtevajo večstopenjsko logiko, matematiko ali reševanje problemov.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-razmišljanje je ~4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Model ovije svoje razmišljanje v oznake <think>...</think>
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

// Analiza mišljenja verige misli
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Kdaj uporabljati modele za sklepanje:**
> - Matematika in logični problemi
> - Naloge z večstopenjskim načrtovanjem
> - Kompleksno generiranje kode
> - Naloge, kjer prikaz postopka izboljša natančnost
>
> **Kompromis:** Modeli za sklepanje proizvedejo več žetonov (odsek `<think>`) in so počasnejši. Za preprosto vprašanje-odgovor je standardni model, kot je phi-3.5-mini, hitrejši.

---

### Vaja 11: Razumevanje vzdevkov in izbira strojne opreme

Ko podate **vzdevek** (kot `phi-3.5-mini`) namesto polnega ID modela, SDK samodejno izbere najboljšo varianto za vašo strojno opremo:

| Strojna oprema | Izbrani izvajalec izvajanja |
|----------------|-----------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (prek WinML vtičnika) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Poljubna naprava (rezerva) | `CPUExecutionProvider` ali `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Nadomestek se preslika na najboljšo različico za VAŠO strojno opremo
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Namig:** Vedno uporabljajte vzdevke v svoji aplikacijski kodi. Ko nameščate na uporabnikov računalnik, SDK ob zagonu izbere optimalno varianto – CUDA na NVIDIA, QNN na Qualcomm, CPU drugod.

---

### Vaja 12: Možnosti konfiguracije za C#

Razred `Configuration` v C# SDK ponuja natančen nadzor nad izvajanjem:

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

| Lastnost | Privzeto | Opis |
|----------|---------|-------|
| `AppName` | (zahtevano) | Ime vaše aplikacije |
| `LogLevel` | `Information` | Raven beleženja |
| `Web.Urls` | (dinamično) | Določi specifično vratnico za spletni strežnik |
| `AppDataDir` | Privzeto OS | Osnovni imenik za podatke aplikacije |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Kje so shranjeni modeli |
| `LogsDir` | `{AppDataDir}/logs` | Kje se zapisujejo dnevniški zapisi |

---

### Vaja 13: Uporaba v brskalniku (samo JavaScript)

JavaScript SDK vključuje različico združljivo z brskalnikom. V brskalniku morate storitev ročno zagnati prek CLI in določiti URL gostitelja:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Najprej ročno zaženite storitev:
//   foundry service start
// Nato uporabite URL iz izhoda CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Brskajte po katalogu
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Omejitve v brskalniku:** Brskalniška različica **ne podpira** `startWebService()`. Zagotoviti morate, da je storitev Foundry Local že zagnana, preden uporabljate SDK v brskalniku.

---

## Popoln API referenčni pregled

### Python

| Kategorija | Metoda | Opis |
|------------|--------|------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Ustvari upravitelja; po želji bootstrap z modelom |
| **Storitev** | `is_service_running()` | Preveri, ali storitev teče |
| **Storitev** | `start_service()` | Zaženi storitev |
| **Storitev** | `endpoint` | URL API končne točke |
| **Storitev** | `api_key` | API ključ |
| **Katalog** | `list_catalog_models()` | Navedba vseh razpoložljivih modelov |
| **Katalog** | `refresh_catalog()` | Osveži katalog |
| **Katalog** | `get_model_info(alias_or_model_id)` | Pridobi metapodatke o modelu |
| **Predpomnilnik** | `get_cache_location()` | Pot do predpomnilniškega imenika |
| **Predpomnilnik** | `list_cached_models()` | Navedba prenesenih modelov |
| **Model** | `download_model(alias_or_model_id)` | Prenesi model |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Naloži model |
| **Model** | `unload_model(alias_or_model_id)` | Izloči model iz pomnilnika |
| **Model** | `list_loaded_models()` | Navedba naloženih modelov |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Preveri, ali je na voljo novejša verzija |
| **Model** | `upgrade_model(alias_or_model_id)` | Nadgradi model na najnovejšo verzijo |
| **Storitev** | `httpx_client` | Prednastavljen HTTPX odjemalec za neposredne API klice |

### JavaScript

| Kategorija | Metoda | Opis |
|------------|--------|------|
| **Init** | `FoundryLocalManager.create(options)` | Inicializiraj SDK singlton |
| **Init** | `FoundryLocalManager.instance` | Dostop do singlton upravitelja |
| **Storitev** | `manager.startWebService()` | Zaženi spletno storitev |
| **Storitev** | `manager.urls` | Polje osnovnih URL-jev za storitev |
| **Katalog** | `manager.catalog` | Dostop do kataloga modelov |
| **Katalog** | `catalog.getModel(alias)` | Pridobi model po vzdevku (vrne Promise) |
| **Model** | `model.isCached` | Ali je model prenesen |
| **Model** | `model.download()` | Prenesi model |
| **Model** | `model.load()` | Naloži model |
| **Model** | `model.unload()` | Izloči model iz pomnilnika |
| **Model** | `model.id` | Edinstveni identifikator modela |
| **Model** | `model.alias` | Vzdevek modela |
| **Model** | `model.createChatClient()` | Pridobi native klepetalni odjemalec (brez OpenAI SDK) |
| **Model** | `model.createAudioClient()` | Pridobi avdio odjemalec za prepise |
| **Model** | `model.removeFromCache()` | Odstrani model iz lokalnega predpomnilnika |
| **Model** | `model.selectVariant(variant)` | Izberi določeno strojno varianto |
| **Model** | `model.variants` | Polje razpoložljivih variant za ta model |
| **Model** | `model.isLoaded()` | Preveri, ali je model naložen |
| **Katalog** | `catalog.getModels()` | Navedba vseh razpoložljivih modelov |
| **Katalog** | `catalog.getCachedModels()` | Navedba prenesenih modelov |
| **Katalog** | `catalog.getLoadedModels()` | Navedba trenutno naloženih modelov |
| **Katalog** | `catalog.updateModels()` | Osveži katalog iz storitve |
| **Storitev** | `manager.stopWebService()` | Ustavi pointerno spletno storitev Foundry Local |

### C# (v0.8.0+)

| Kategorija | Metoda | Opis |
|------------|--------|------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Inicializiraj upravitelja |
| **Init** | `FoundryLocalManager.Instance` | Dostop do singltona |
| **Katalog** | `manager.GetCatalogAsync()` | Pridobi katalog |
| **Katalog** | `catalog.ListModelsAsync()` | Navedba vseh modelov |
| **Katalog** | `catalog.GetModelAsync(alias)` | Pridobi specifičen model |
| **Katalog** | `catalog.GetCachedModelsAsync()` | Navedba predpomnjenih modelov |
| **Katalog** | `catalog.GetLoadedModelsAsync()` | Navedba naloženih modelov |
| **Model** | `model.DownloadAsync(progress?)` | Prenesi model |
| **Model** | `model.LoadAsync()` | Naloži model |
| **Model** | `model.UnloadAsync()` | Izloči model iz pomnilnika |
| **Model** | `model.SelectVariant(variant)` | Izberi strojno varianto |
| **Model** | `model.GetChatClientAsync()` | Pridobi native klepetalni odjemalec |
| **Model** | `model.GetAudioClientAsync()` | Pridobi avdio odjemalec za prepise |
| **Model** | `model.GetPathAsync()` | Pridobi lokalno pot do datoteke |
| **Katalog** | `catalog.GetModelVariantAsync(alias, variant)` | Pridobi specifično strojno varianto |
| **Katalog** | `catalog.UpdateModelsAsync()` | Osveži katalog |
| **Strežnik** | `manager.StartWebServerAsync()` | Zaženi REST spletni strežnik |
| **Strežnik** | `manager.StopWebServerAsync()` | Ustavi REST spletni strežnik |
| **Konfiguracija** | `config.ModelCacheDir` | Imenik predpomnilnika |

---

## Ključne ugotovitve

| Koncept | Naučili ste se |
|---------|----------------|
| **SDK vs CLI** | SDK omogoča programski nadzor – ključno za aplikacije |
| **Nadzorna ravnina** | SDK upravlja storitve, modele in predpomnilnik |
| **Dinamične vratnice** | Vedno uporabljajte `manager.endpoint` (Python) ali `manager.urls[0]` (JS/C#) – nikoli ne kodirajte vratnic trdo |
| **Vzdevki** | Uporabljajte vzdevke za samodejno optimalno izbiro strojne opreme |
| **Hiter začetek** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Prenova C#** | v0.8.0+ je samostojen - za končne uporabnike ni potreben CLI |
| **Življenjski cikel modela** | Katalog → Prenos → Naloži → Uporabi → Razveljavi nalaganje |
| **FoundryModelInfo** | Bogati metapodatki: naloga, naprava, velikost, licenca, podpora orodjem, ki kliče |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) za uporabo brez OpenAI |
| **Variante** | Modeli imajo strojno-specifične variante (CPU, GPU, NPU); izbrane samodejno |
| **Nadgradnje** | Python: `is_model_upgradeable()` + `upgrade_model()` za ohranjanje modelov ažurnih |
| **Osvežitev kataloga** | `refresh_catalog()` (Python) / `updateModels()` (JS) za odkrivanje novih modelov |

---

## Viri

| Vir | Povezava |
|----------|------|
| SDK Referenca (vsi jeziki) | [Microsoft Learn - Foundry Local SDK Referenca](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integracija z inferenčnimi SDK-ji | [Microsoft Learn - Integracija z inferenčnimi SDK-ji](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API Referenca | [Foundry Local C# API Referenca](https://aka.ms/fl-csharp-api-ref) |
| C# SDK Primeri | [GitHub - Foundry Local SDK Primeri](https://aka.ms/foundrylocalSDK) |
| Spletna stran Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Naslednji koraki

Nadaljujte na [Del 3: Uporaba SDK z OpenAI](part3-sdk-and-apis.md), da povežete SDK s knjižnico OpenAI odjemalca in zgradite svojo prvo aplikacijo za dokončanje klepeta.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Izjava o omejitvi odgovornosti**:
Ta dokument je bil preveden z uporabo AI prevajalske storitve [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za natančnost, upoštevajte, da avtomatizirani prevodi lahko vsebujejo napake ali netočnosti. Izvirni dokument v njegovi izvorni jezikovni različici velja za uradni vir. Za kritične informacije priporočamo strokovni človeški prevod. Nismo odgovorni za morebitna nesporazume ali napačne interpretacije, ki izhajajo iz uporabe tega prevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->