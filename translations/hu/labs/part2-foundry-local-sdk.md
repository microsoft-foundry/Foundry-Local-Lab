![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 2. rész: Foundry Local SDK Mélyelemzés

> **Cél:** Sajátítsd el a Foundry Local SDK használatát a modellek, szolgáltatások és gyorsítótár programozott kezeléséhez - és értsd meg, hogy miért az SDK az ajánlott módszer az alkalmazások építésére a CLI helyett.

## Áttekintés

Az 1. részben a **Foundry Local CLI**-t használtad modellek letöltésére és interaktív futtatására. A CLI nagyszerű a felfedezéshez, de valódi alkalmazások készítésekor szükséged van a **programozott vezérlésre**. A Foundry Local SDK ezt biztosítja számodra – kezeli a **vezérlő síkot** (a szolgáltatás indítása, modellek felfedezése, letöltése, betöltése), így az alkalmazásod kódja a **adat síkra** fókuszálhat (promptok küldése, befejezések fogadása).

Ez a labor a teljes SDK API-t bemutatja Python, JavaScript és C# nyelven egyaránt. A végére minden elérhető metódust érteni fogsz és tudni fogod, mikor melyiket használd.

## Tanulási célok

A labor végére képes leszel:

- Elmagyarázni, hogy miért előnyösebb az SDK használata a CLI-vel szemben alkalmazásfejlesztéshez
- Telepíteni a Foundry Local SDK-t Python, JavaScript vagy C# környezetben
- Használni a `FoundryLocalManager`-t a szolgáltatás indításához, modellek kezeléséhez és a katalógus lekérdezéséhez
- Programozottan listázni, letölteni, betölteni és eltávolítani modelleket
- Megvizsgálni a modell metaadatait a `FoundryModelInfo` segítségével
- Megérteni a katalógus, a gyorsítótár és a betöltött modellek közötti különbséget
- Használni a konstruktor bootstrap-et (Python) és a `create()` + katalógus mintát (JavaScript)
- Megérteni a C# SDK újratervezését és annak objektumorientált API-ját

---

## Előfeltételek

| Követelmény | Részletek |
|-------------|-----------|
| **Foundry Local CLI** | Telepítve és elérhető az `PATH`-on ([1. rész](part1-getting-started.md)) |
| **Nyelvi futtatókörnyezet** | **Python 3.9+** és/vagy **Node.js 18+** és/vagy **.NET 9.0+** |

---

## Fogalom: SDK vs CLI - Miért használd az SDK-t?

| Szempont | CLI (`foundry` parancs) | SDK (`foundry-local-sdk`) |
|----------|-------------------------|---------------------------|
| **Használati eset** | Felfedezés, kézi tesztelés | Alkalmazás integráció |
| **Szolgáltatás kezelése** | Kézi: `foundry service start` | Automatikus: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Port felfedezés** | CLI kimenet olvasása | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Modell letöltés** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Hibakezelés** | Kilépési kódok, stderr | Kivételkezelés, típusos hibák |
| **Automatizálás** | Shell szkriptek | Natív nyelvi integráció |
| **Telepítés** | A végfelhasználó gépén CLI szükséges | C# SDK önállóan működik (CLI nem kell) |

> **Kulcsgondolat:** Az SDK kezeli az egész életciklust: a szolgáltatás indítását, a gyorsítótár ellenőrzését, a hiányzó modellek letöltését, azok betöltését és a végpont felfedezését pár sor kódban. Az alkalmazásodnak nem kell CLI kimenetet értelmeznie vagy alfolyamatokat kezelnie.

---

## Laborfeladatok

### 1. gyakorlat: SDK Telepítése

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Ellenőrizd a telepítést:

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

Ellenőrizd a telepítést:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Két NuGet csomag érhető el:

| Csomag | Platform | Leírás |
|--------|----------|--------|
| `Microsoft.AI.Foundry.Local` | Többplatformos | Működik Windows, Linux, macOS rendszereken |
| `Microsoft.AI.Foundry.Local.WinML` | Csak Windows | WinML hardveres gyorsítást ad hozzá; letölti és telepíti a plugin végrehajtókat (pl. QNN Qualcomm NPU-hoz) |

**Windows beállítása:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Szerkeszd a `.csproj` fájlt:

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

> **Megjegyzés:** Windows alatt a WinML csomag egy bővítmény, amely tartalmazza az alap SDK-t és a QNN végrehajtót. Linux/macOS alatt az alap SDK használatos. A feltételes TFM és csomag hivatkozások teljesen többplatformossá teszik a projektet.

Hozz létre egy `nuget.config` fájlt a projekt gyökerében:

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

Csomagok visszaállítása:

```bash
dotnet restore
```

</details>

---

### 2. gyakorlat: A Szolgáltatás Indítása és a Katalógus Listázása

Az első lépés minden alkalmazásnál a Foundry Local szolgáltatás indítása és a rendelkezésre álló modellek felderítése.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Hozzon létre egy menedzsert és indítsa el a szolgáltatást
manager = FoundryLocalManager()
manager.start_service()

# Sorolja fel az összes elérhető modellt a katalógusban
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Szolgáltatás kezelő metódusok

| Metódus | Aláírás | Leírás |
|---------|---------|--------|
| `is_service_running()` | `() -> bool` | Ellenőrzi, hogy a szolgáltatás fut-e |
| `start_service()` | `() -> None` | Elindítja a Foundry Local szolgáltatást |
| `service_uri` | `@property -> str` | Alapszolgáltatás URI |
| `endpoint` | `@property -> str` | API végpont (szolgáltatás URI + `/v1`) |
| `api_key` | `@property -> str` | API kulcs (környezeti változóból vagy alapértelmezett) |

#### Python SDK - Katalógus kezelő metódusok

| Metódus | Aláírás | Leírás |
|---------|---------|--------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | A katalógus összes modelljének listázása |
| `refresh_catalog()` | `() -> None` | A katalógus frissítése a szolgáltatásból |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Egy adott modell információinak lekérése |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Hozzon létre egy menedzsert és indítsa el a szolgáltatást
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Böngéssze a katalógust
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Manager metódusok

| Metódus | Aláírás | Leírás |
|---------|---------|--------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Az SDK egyedi példányának inicializálása |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Hozzáférés a singleton managerhez |
| `manager.startWebService()` | `() => Promise<void>` | Elindítja a Foundry Local webszolgáltatást |
| `manager.urls` | `string[]` | A szolgáltatás alap URL-jeinek tömbje |

#### JavaScript SDK - Katalógus és modell metódusok

| Metódus | Aláírás | Leírás |
|---------|---------|--------|
| `manager.catalog` | `Catalog` | Hozzáférés a modell katalógushoz |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Modell objektum lekérése alias alapján |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

A C# SDK v0.8.0+-tól objektumorientált architektúrát használ `Configuration`, `Catalog` és `Model` objektumokkal:

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

#### C# SDK - Kulcsosztályok

| Osztály | Funkció |
|---------|---------|
| `Configuration` | App név, naplózási szint, gyorsítótár könyvtár, webszerver URL-ek beállítása |
| `FoundryLocalManager` | Fő belépési pont -- `CreateAsync()`-val hozható létre, `.Instance`-tel érhető el |
| `Catalog` | Modell katalógus böngészése, keresés, lekérés |
| `Model` | Egy adott modell reprezentációja - letöltés, betöltés, kliensek lekérése |

#### C# SDK - Manager és katalógus metódusok

| Metódus | Leírás |
|---------|---------|
| `FoundryLocalManager.CreateAsync(config, logger)` | A menedzser inicializálása |
| `FoundryLocalManager.Instance` | Hozzáférés a singleton managerhez |
| `manager.GetCatalogAsync()` | Modell katalógus lekérése |
| `catalog.ListModelsAsync()` | Az elérhető modellek listázása |
| `catalog.GetModelAsync(alias: "alias")` | Egy adott modell lekérése alias alapján |
| `catalog.GetCachedModelsAsync()` | Letöltött modellek listázása |
| `catalog.GetLoadedModelsAsync()` | Jelenleg betöltött modellek listázása |

> **C# architektúra megjegyzés:** A C# SDK v0.8.0+-as újratervezése önálló alkalmazásokat eredményez; nem szükséges a Foundry Local CLI a végfelhasználó gépén. Az SDK natív módon kezeli a modellek kezelését és az inferenciát.

</details>

---

### 3. gyakorlat: Modell letöltése és betöltése

Az SDK elkülöníti a letöltést (lemezen) a betöltéstől (memóriába). Ez lehetővé teszi a modellek előzetes letöltését a telepítés során és betöltésüket igény szerint.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# A lehetőség: Kézi lépésről lépésre
manager = FoundryLocalManager()
manager.start_service()

# Először ellenőrizze a gyorsítótárat
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

# B lehetőség: Egy soros bootstrap (ajánlott)
# Alias átadása a konstruktorhoz - automatikusan elindítja a szolgáltatást, letölti és betölti
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Modellkezelő metódusok

| Metódus | Aláírás | Leírás |
|---------|---------|--------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Modell letöltése helyi gyorsítótárba |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Modell betöltése az inferencia szerverre |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Modell eltávolítása a szerverről |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Jelenleg betöltött modellek listázása |

#### Python - Gyorsítótár kezelő metódusok

| Metódus | Aláírás | Leírás |
|---------|---------|--------|
| `get_cache_location()` | `() -> str` | Gyorsítótár könyvtár elérési útja |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Letöltött modellek listázása |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Lépésről lépésre megközelítés
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

#### JavaScript - Modell metódusok

| Metódus | Aláírás | Leírás |
|---------|---------|--------|
| `model.isCached` | `boolean` | Vajon a modell már le van-e töltve |
| `model.download()` | `() => Promise<void>` | Modell letöltése helyi gyorsítótárba |
| `model.load()` | `() => Promise<void>` | Betöltés az inferencia szerverre |
| `model.unload()` | `() => Promise<void>` | Eltávolítás az inferencia szerverről |
| `model.id` | `string` | A modell egyedi azonosítója |

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

#### C# - Modell metódusok

| Metódus | Leírás |
|---------|---------|
| `model.DownloadAsync(progress?)` | A kiválasztott változat letöltése |
| `model.LoadAsync()` | Modell betöltése memóriába |
| `model.UnloadAsync()` | Modell eltávolítása |
| `model.SelectVariant(variant)` | Egy adott változat (CPU/GPU/NPU) kiválasztása |
| `model.SelectedVariant` | A jelenleg kiválasztott változat |
| `model.Variants` | A modell összes elérhető változata |
| `model.GetPathAsync()` | Helyi fájl elérési út lekérése |
| `model.GetChatClientAsync()` | Natív chat kliens lekérése (nem kell OpenAI SDK) |
| `model.GetAudioClientAsync()` | Audió kliens lekérése átirathoz |

</details>

---

### 4. gyakorlat: Modell metaadatok megvizsgálása

A `FoundryModelInfo` objektum gazdag metaadatokat tartalmaz minden modellről. Ezek megértése segít a megfelelő modell kiválasztásában az alkalmazásodhoz.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Részletes információk lekérése egy adott modellről
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

#### FoundryModelInfo mezők

| Mező | Típus | Leírás |
|-------|-------|----------|
| `alias` | string | Rövid név (pl. `phi-3.5-mini`) |
| `id` | string | Egyedi modellazonosító |
| `version` | string | Modell verzió |
| `task` | string | `chat-completions` vagy `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU vagy NPU |
| `execution_provider` | string | Futásidejű háttér (CUDA, CPU, QNN, WebGPU, stb.) |
| `file_size_mb` | int | Méret lemezen MB-ban |
| `supports_tool_calling` | bool | Támogatja-e a funkció/eszköz hívást a modell |
| `publisher` | string | Ki tette közzé a modellt |
| `license` | string | Licenc neve |
| `uri` | string | Modell URI |
| `prompt_template` | dict/null | Prompt sablon, ha van |

---

### 5. gyakorlat: A modell életciklusának kezelése

Gyakorold a teljes életciklust: listázás → letöltés → betöltés → használat → eltávolítás.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Kis modell gyors teszteléshez

manager = FoundryLocalManager()
manager.start_service()

# 1. Ellenőrizze, mi van a katalógusban
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Ellenőrizze, mi van már letöltve
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Töltsön le egy modellt
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Ellenőrizze, hogy most a gyorsítótárban van-e
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Töltse be
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Ellenőrizze, mi van betöltve
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Töltse le azt
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

const alias = "qwen2.5-0.5b"; // Kis modell a gyors teszteléshez

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Modell beszerzése a katalógusból
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Letöltés szükség esetén
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Betöltés
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Kivétel (memóriából)
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### 6. Gyakorlat: Gyorsindítási minták

Minden nyelv biztosít egy rövidített hívást a szolgáltatás elindításához és a modell betöltéséhez egyetlen hívásban. Ezek a **ajánlott minták** a legtöbb alkalmazáshoz.

<details>
<summary><h3>🐍 Python - Konstruktor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Adj meg egy aliaszt a konstruktornak - mindenről gondoskodik:
# 1. Elindítja a szolgáltatást, ha nem fut
# 2. Letölti a modellt, ha nincs gyorsítótárazva
# 3. Betölti a modellt az inferencia szerverbe
manager = FoundryLocalManager("phi-3.5-mini")

# Azonnal használatra kész
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

A `bootstrap` paraméter (alapértelmezett `True`) vezérli ezt a viselkedést. Állítsd `bootstrap=False`-ra, ha kézi vezérlést szeretnél:

```python
# Kézi mód - semmi sem történik automatikusan
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalógus</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() mindent kezel:
// 1. Elindítja a szolgáltatást
// 2. Lekéri a modellt a katalógusból
// 3. Letölti, ha szükséges, és betölti a modellt
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Azonnal használatra kész
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Katalógus</h3></summary>

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

> **C# megjegyzés:** A C# SDK a `Configuration` osztályt használja az alkalmazásnév, naplózás, gyorsítótár könyvtárak és akár a konkrét webszerver port beállítására. Ez teszi a három SDK közül a legkonfigurálhatóbbá.

</details>

---

### 7. Gyakorlat: Natív ChatClient (Nincs szükség OpenAI SDK-ra)

A JavaScript és C# SDK-k biztosítanak egy `createChatClient()` kényelmi metódust, amely egy natív chat klienst ad vissza — nem kell külön telepíteni vagy konfigurálni az OpenAI SDK-t.

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

// Készítsen ChatClient-et közvetlenül a modellből — nincs szükség OpenAI importálására
const chatClient = model.createChatClient();

// completeChat egy OpenAI-kompatibilis válaszobjektumot ad vissza
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// A streamelés egy visszahívásos mintát használ
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

A `ChatClient` támogatja az eszközhívásokat is — adj át eszközöket a második argumentumként:

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

> **Mikor melyik mintát használd:**
> - **`createChatClient()`** — Gyors prototípus készítés, kevesebb függőség, egyszerűbb kód
> - **OpenAI SDK** — Teljes paraméterkontroll (hőmérséklet, top_p, stop tokenek stb.), jobb termelési alkalmazásokhoz

---

### 8. Gyakorlat: Modellváltozatok és hardverválasztás

A modellek több, a különböző hardverekhez optimalizált **változattal** is rendelkezhetnek. Az SDK automatikusan kiválasztja a legjobb változatot, de te is megvizsgálhatod és manuálisan választhatsz.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Listázza az összes elérhető változatot
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// Az SDK automatikusan kiválasztja a hardveredhez legjobb változatot
// A felülíráshoz használd a selectVariant() függvényt:
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

Pythonban az SDK automatikusan kiválasztja a legjobb változatot a hardver alapján. Használd a `get_model_info()`-t, hogy lásd, mit választott ki:

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

#### NPU változattal rendelkező modellek

Néhány modell NPU-optimalizált változattal rendelkezik azokhoz az eszközökhöz, melyek Neural Processing Unittal (Qualcomm Snapdragon, Intel Core Ultra) vannak felszerelve:

| Modell | NPU változat elérhető |
|--------|:--------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Tipp:** NPU-kompatibilis hardveren az SDK automatikusan kiválasztja az NPU változatot, ha elérhető. Nem kell a kódodban változtatni. Windows alatti C# projektek esetén add hozzá a `Microsoft.AI.Foundry.Local.WinML` NuGet csomagot a QNN végrehajtó engedélyezéséhez — a QNN egy WinML-en keresztül szállított plugin végrehajtó.

---

### 9. Gyakorlat: Modellfrissítések és katalógus frissítése

A modellkatalógus időszakosan frissül. Használd ezeket a metódusokat a frissítések ellenőrzésére és alkalmazására.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Frissítse a katalógust a legújabb modelllista lekéréséhez
manager.refresh_catalog()

# Ellenőrizze, hogy egy gyorsítótárazott modellhez elérhető-e újabb verzió
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

// Frissítse a katalógust a legújabb modell lista lekéréséhez
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// A frissítés után az összes elérhető modell listázása
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### 10. Gyakorlat: Gondolkodó (reasoning) modellekkel való munka

A **phi-4-mini-reasoning** modell tartalmaz láncolt gondolkodási logikát. A belső gondolatmenetét `<think>...</think>` tagek közé csomagolja, mielőtt a végleges választ adná. Ez hasznos többlépcsős logikát, matematikai vagy problémamegoldó feladatokat igénylő helyzetekhez.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# a phi-4-mini-reasoning körülbelül 4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# A modell a gondolkodását <think>...</think> tagek közé csomagolja
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

// Láncgondolkodás elemzése
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Mikor használjunk reasoning modelleket:**
> - Matematikai és logikai feladatok
> - Többlépcsős tervezési feladatok
> - Összetett kódgenerálás
> - Feladatok, ahol a munkafolyamat bemutatása növeli a pontosságot
>
> **Ár:** A reasoning modellek több token-t (a `<think>` szakaszt) generálnak és lassabbak. Egyszerű kérdés-válasz esetén egy standard modell, mint a phi-3.5-mini gyorsabb.

---

### 11. Gyakorlat: Aliászok és hardverválasztás megértése

Ha egy **aliászt** (például `phi-3.5-mini`) adsz meg a teljes modellazonosító helyett, az SDK automatikusan kiválasztja a hardveredhez legoptimálisabb változatot:

| Hardver | Kiválasztott végrehajtó |
|---------|--------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML pluginon keresztül) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Bármilyen eszköz (tartalék) | `CPUExecutionProvider` vagy `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Az alias a legjobb változatra oldódik fel az ÖN hardveréhez
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tipp:** Mindig használj aliászokat az alkalmazáskódodban. Amikor feltelepíted a felhasználó gépére, az SDK futásidőben választja ki az optimális variánst — CUDA-t NVIDIA-n, QNN-t Qualcomm-on, máshol CPU-t.

---

### 12. Gyakorlat: C# konfigurációs opciók

A C# SDK `Configuration` osztálya részletes vezérlést nyújt a futtatási környezet felett:

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

| Tulajdonság | Alapértelmezett | Leírás |
|-------------|-----------------|--------|
| `AppName` | (kötelező) | Az alkalmazásod neve |
| `LogLevel` | `Information` | Naplózás részletessége |
| `Web.Urls` | (dinamikus) | Egy adott port rögzítése a webszervernek |
| `AppDataDir` | OS alapértelmezett | Alkalmazásadatok alapkönyvtára |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Modell tárolási helye |
| `LogsDir` | `{AppDataDir}/logs` | Naplófájlok helye |

---

### 13. Gyakorlat: Böngészőben való használat (csak JavaScript)

A JavaScript SDK tartalmaz egy böngésző-kompatibilis verziót. A böngészőből manuálisan kell elindítani a szolgáltatást a CLI-n keresztül és megadni a hoszt URL-t:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Először indítsa el kézzel a szolgáltatást:
//   foundry szolgáltatás indítása
// Ezután használja a CLI kimenetből származó URL-t
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Böngéssze a katalógust
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Böngésző korlátok:** A böngésző verzió **nem** támogatja a `startWebService()` metódust. Biztosítanod kell, hogy a Foundry Local szolgáltatás már fusson, mielőtt az SDK-t használod a böngészőben.

---

## Teljes API referencia

### Python

| Kategória | Metódus | Leírás |
|-----------|---------|--------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Menedzser létrehozása; opcionálisan bootstrappel egy modell |
| **Szolgáltatás** | `is_service_running()` | Szolgáltatás futását ellenőrzi |
| **Szolgáltatás** | `start_service()` | Szolgáltatás indítása |
| **Szolgáltatás** | `endpoint` | API végpont URL |
| **Szolgáltatás** | `api_key` | API kulcs |
| **Katalógus** | `list_catalog_models()` | Az elérhető modellek listázása |
| **Katalógus** | `refresh_catalog()` | Katalógus frissítése |
| **Katalógus** | `get_model_info(alias_or_model_id)` | Modell metaadatainak lekérése |
| **Gyorsítótár** | `get_cache_location()` | Gyorsítótár könyvtár elérési útja |
| **Gyorsítótár** | `list_cached_models()` | Letöltött modellek listája |
| **Modell** | `download_model(alias_or_model_id)` | Modell letöltése |
| **Modell** | `load_model(alias_or_model_id, ttl=600)` | Modell betöltése |
| **Modell** | `unload_model(alias_or_model_id)` | Modell elengedése |
| **Modell** | `list_loaded_models()` | Betöltött modellek listája |
| **Modell** | `is_model_upgradeable(alias_or_model_id)` | Ellenőrzi, van-e újabb verzió |
| **Modell** | `upgrade_model(alias_or_model_id)` | Modell frissítése a legújabb verzióra |
| **Szolgáltatás** | `httpx_client` | Előre konfigurált HTTPX kliens közvetlen API hívásokhoz |

### JavaScript

| Kategória | Metódus | Leírás |
|-----------|---------|--------|
| **Init** | `FoundryLocalManager.create(options)` | SDK singleton inicializálása |
| **Init** | `FoundryLocalManager.instance` | A singleton menedzser elérése |
| **Szolgáltatás** | `manager.startWebService()` | Webszolgáltatás indítása |
| **Szolgáltatás** | `manager.urls` | A szolgáltatás alap URL-jeinek tömbje |
| **Katalógus** | `manager.catalog` | Modell katalógus elérése |
| **Katalógus** | `catalog.getModel(alias)` | Modell objektum lekérése alias alapján (Promise-t ad vissza) |
| **Modell** | `model.isCached` | Megnézi, hogy le van-e töltve a modell |
| **Modell** | `model.download()` | Modell letöltése |
| **Modell** | `model.load()` | Modell betöltése |
| **Modell** | `model.unload()` | Modell elengedése |
| **Modell** | `model.id` | Modell egyedi azonosítója |
| **Modell** | `model.alias` | Modell aliásza |
| **Modell** | `model.createChatClient()` | Natív chat kliens lekérése (nem kell OpenAI SDK) |
| **Modell** | `model.createAudioClient()` | Audió átírási kliens lekérése |
| **Modell** | `model.removeFromCache()` | Modell törlése a helyi gyorsítótárból |
| **Modell** | `model.selectVariant(variant)` | Specifikus hardverváltozat kiválasztása |
| **Modell** | `model.variants` | A modell elérhető változatainak tömbje |
| **Modell** | `model.isLoaded()` | Ellenőrzi, hogy betöltött-e a modell |
| **Katalógus** | `catalog.getModels()` | Az összes elérhető modell listázása |
| **Katalógus** | `catalog.getCachedModels()` | Letöltött modellek listája |
| **Katalógus** | `catalog.getLoadedModels()` | Futó modellek listája |
| **Katalógus** | `catalog.updateModels()` | Katalógus frissítése a szolgáltatásból |
| **Szolgáltatás** | `manager.stopWebService()` | Foundry Local webszolgáltatás leállítása |

### C# (v0.8.0+)

| Kategória | Metódus | Leírás |
|-----------|---------|--------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Menedzser inicializálása |
| **Init** | `FoundryLocalManager.Instance` | Singleton elérése |
| **Katalógus** | `manager.GetCatalogAsync()` | Katalógus lekérése |
| **Katalógus** | `catalog.ListModelsAsync()` | Összes modell listázása |
| **Katalógus** | `catalog.GetModelAsync(alias)` | Egy adott modell lekérése |
| **Katalógus** | `catalog.GetCachedModelsAsync()` | Gyorsítótárban lévő modellek listája |
| **Katalógus** | `catalog.GetLoadedModelsAsync()` | Betöltött modellek listája |
| **Modell** | `model.DownloadAsync(progress?)` | Modell letöltése |
| **Modell** | `model.LoadAsync()` | Modell betöltése |
| **Modell** | `model.UnloadAsync()` | Modell elengedése |
| **Modell** | `model.SelectVariant(variant)` | Hardverváltozat kiválasztása |
| **Modell** | `model.GetChatClientAsync()` | Natív chat kliens lekérése |
| **Modell** | `model.GetAudioClientAsync()` | Audió átíró kliens lekérése |
| **Modell** | `model.GetPathAsync()` | Helyi fájl elérési út lekérése |
| **Katalógus** | `catalog.GetModelVariantAsync(alias, variant)` | Egy adott hardverváltozat lekérése |
| **Katalógus** | `catalog.UpdateModelsAsync()` | Katalógus frissítése |
| **Szerver** | `manager.StartWebServerAsync()` | REST webszolgáltatás elindítása |
| **Szerver** | `manager.StopWebServerAsync()` | REST webszolgáltatás leállítása |
| **Konfiguráció** | `config.ModelCacheDir` | Gyorsítótár könyvtára |

---

## Főbb tanulságok

| Fogalom | Amit megtanultál |
|---------|------------------|
| **SDK vs CLI** | Az SDK programozott vezérlést biztosít — elengedhetetlen az alkalmazások számára |
| **Vezérlési sík** | Az SDK kezeli a szolgáltatásokat, modelleket és gyorsítótárat |
| **Dinamikus portok** | Mindig használd a `manager.endpoint` (Python) vagy `manager.urls[0]` (JS/C#) értékeket — sose hardkódolj portot |
| **Aliászok** | Használj aliászokat az automatikus, hardverhez optimális modellválasztáshoz |
| **Gyorsindítás** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# áttervezés** | v0.8.0+ önálló - nincs szükség CLI-re a végfelhasználói gépeken |
| **Modell életciklus** | Katalógus → Letöltés → Betöltés → Használat → Kiürítés |
| **FoundryModelInfo** | Gazdag metaadatok: feladat, eszköz, méret, licenc, eszköztámogatás hívásra |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) OpenAI-mentes használathoz |
| **Változatok** | A modellek hardver-specifikus változatokkal rendelkeznek (CPU, GPU, NPU); automatikusan kiválasztva |
| **Frissítések** | Python: `is_model_upgradeable()` + `upgrade_model()` a modellek naprakészen tartásához |
| **Katalógus frissítés** | `refresh_catalog()` (Python) / `updateModels()` (JS) az új modellek felfedezéséhez |

---

## Erőforrások

| Erőforrás | Link |
|----------|------|
| SDK Hivatkozás (minden nyelv) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integráció az inference SDK-kkal | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API Hivatkozás | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK Példák | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local weboldal | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Következő lépések

Folytassa a [3. rész: SDK használata az OpenAI-vel](part3-sdk-and-apis.md) címmel, hogy összekapcsolja az SDK-t az OpenAI klienskönyvtárral, és elkészítse az első chat-kiegészítő alkalmazását.