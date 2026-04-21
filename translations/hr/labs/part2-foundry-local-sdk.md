![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Dio 2: Dubinska analiza Foundry Local SDK-a

> **Cilj:** Ovladati Foundry Local SDK-om za upravljanje modelima, uslugama i keširanjem programatski - i razumjeti zašto je SDK preporučeni pristup u odnosu na CLI za izradu aplikacija.

## Pregled

U Dijelu 1 koristili ste **Foundry Local CLI** za preuzimanje i interaktivno pokretanje modela. CLI je odličan za istraživanje, ali kada razvijate prave aplikacije, potrebna vam je **programatska kontrola**. Foundry Local SDK to omogućuje - upravlja **kontrolnom ravninom** (pokretanje usluge, otkrivanje modela, preuzimanje, učitavanje) kako bi se vaš aplikacijski kod mogao usredotočiti na **podatkovnu ravninu** (slanje upita, primanje dovršenih rezultata).

Ova radionica vas uči cijelom API-ju SDK-a preko Pythona, JavaScripta i C#. Do kraja ćete razumjeti svaku raspoloživu metodu i kada je koristiti.

## Ciljevi učenja

Do kraja ove radionice moći ćete:

- Objasniti zašto je SDK poželjniji od CLI-ja za razvoj aplikacija
- Instalirati Foundry Local SDK za Python, JavaScript ili C#
- Koristiti `FoundryLocalManager` za pokretanje usluge, upravljanje modelima i ispitivanje kataloga
- Programatski nabrajati, preuzimati, učitavati i isključivati modele
- Pregledavati metapodatke modela pomoću `FoundryModelInfo`
- Razumjeti razliku između kataloga, keša i učitanih modela
- Koristiti konstruktor bootstrap (Python) i `create()` + katalog obrazac (JavaScript)
- Razumjeti C# SDK redizajn i njegov objektno orijentirani API

---

## Preduvjeti

| Zahtjev | Detalji |
|-------------|---------|
| **Foundry Local CLI** | Instaliran i na vašem `PATH`-u ([Dio 1](part1-getting-started.md)) |
| **Runtime jezik** | **Python 3.9+** i/ili **Node.js 18+** i/ili **.NET 9.0+** |

---

## Koncept: SDK vs CLI - Zašto koristiti SDK?

| Aspekt | CLI (`foundry` naredba) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Slučaj upotrebe** | Istraživanje, ručno testiranje | Integracija u aplikaciju |
| **Upravljanje uslugom** | Ručno: `foundry service start` | Automatski: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Otkriće porta** | Čitanje iz izlaza CLI-ja | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Preuzimanje modela** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Rukovanje greškama** | Izlazni kodovi, stderr | Izuzeci, tipizirane greške |
| **Automatizacija** | Shell skripte | Integracija u izvornom jeziku |
| **Implementacija** | Zahtijeva CLI na stroju korisnika | C# SDK može biti samostalan (nije potreban CLI) |

> **Ključna spoznaja:** SDK rukuje cijelim životnim ciklusom: pokretanjem usluge, provjerom keša, preuzimanjem nedostajućih modela, njihovim učitavanjem i otkrivanjem krajnje točke, u nekoliko redaka koda. Vaša aplikacija ne mora parsirati izlaz CLI-ja niti upravljati pomoćnim procesima.

---

## Vježbe u radionici

### Vježba 1: Instalacija SDK-a

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Provjerite instalaciju:

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

Provjerite instalaciju:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Postoje dva NuGet paketa:

| Paket | Platforma | Opis |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Višeplatformski | Radi na Windowsu, Linuxu i macOS-u |
| `Microsoft.AI.Foundry.Local.WinML` | Samo Windows | Dodaje WinML hardversku akceleraciju; preuzima i instalira provajdere izvršenja plugina (npr. QNN za Qualcomm NPU) |

**Postavljanje za Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Uredite `.csproj` datoteku:

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

> **Napomena:** Na Windowsu je WinML paket superskup koji uključuje osnovni SDK plus QNN provajder izvršenja. Na Linuxu/macOS-u koristi se osnovni SDK. Uvjetne TFM i reference paketa održavaju projekt potpuno višeplatformskim.

Kreirajte `nuget.config` u rootu projekta:

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

### Vježba 2: Pokrenite uslugu i nabrojite katalog

Prva stvar koju aplikacija radi je pokretanje Foundry Local usluge i otkrivanje dostupnih modela.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Kreirajte upravitelja i pokrenite uslugu
manager = FoundryLocalManager()
manager.start_service()

# Nabrojite sve modele dostupne u katalogu
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Metode za upravljanje uslugom

| Metoda | Potpis | Opis |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Provjeri je li usluga pokrenuta |
| `start_service()` | `() -> None` | Pokreni Foundry Local uslugu |
| `service_uri` | `@property -> str` | Osnovni URI usluge |
| `endpoint` | `@property -> str` | API krajnja točka (URI usluge + `/v1`) |
| `api_key` | `@property -> str` | API ključ (iz env ili zadani placeholder) |

#### Python SDK - Metode za upravljanje katalogom

| Metoda | Potpis | Opis |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Navedite sve modele u katalogu |
| `refresh_catalog()` | `() -> None` | Osvježi katalog iz usluge |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Dohvati informacije za određeni model |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Kreirajte upravitelja i pokrenite uslugu
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Pregledajte katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Metode managera

| Metoda | Potpis | Opis |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inicijaliziraj SDK singleton |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Pristupi singleton manageru |
| `manager.startWebService()` | `() => Promise<void>` | Pokreni Foundry Local web uslugu |
| `manager.urls` | `string[]` | Niz osnovnih URL-ova za uslugu |

#### JavaScript SDK - Metode za katalog i modele

| Metoda | Potpis | Opis |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Pristupi katalogu modela |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Dohvati objekt modela prema aliasu |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ koristi objektno orijentiranu arhitekturu sa objektima `Configuration`, `Catalog` i `Model`:

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

#### C# SDK - Ključne klase

| Klasa | Svrha |
|-------|---------|
| `Configuration` | Postavi naziv aplikacije, razinu zapisa, direktorij keša, URL-ove web servera |
| `FoundryLocalManager` | Glavna ulazna točka - kreirana preko `CreateAsync()`, pristup preko `.Instance` |
| `Catalog` | Pregled, pretraživanje i dohvat modela iz kataloga |
| `Model` | Predstavlja specifični model - preuzimanje, učitavanje, dohvat klijenata |

#### C# SDK - Metode managera i kataloga

| Metoda | Opis |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inicijaliziraj manager |
| `FoundryLocalManager.Instance` | Pristupi singleton manageru |
| `manager.GetCatalogAsync()` | Dohvati katalog modela |
| `catalog.ListModelsAsync()` | Nabroji sve dostupne modele |
| `catalog.GetModelAsync(alias: "alias")` | Dohvati specifični model prema aliasu |
| `catalog.GetCachedModelsAsync()` | Nabroji preuzete modele |
| `catalog.GetLoadedModelsAsync()` | Nabroji trenutno učitane modele |

> **Napomena o arhitekturi C#-a:** Redizajn C# SDK-a v0.8.0+ čini aplikaciju **samostalnom**; nije potreban Foundry Local CLI na korisnikovom stroju. SDK upravlja modelima i izvođenjem inferencija nativno.

</details>

---

### Vježba 3: Preuzimanje i učitavanje modela

SDK odvaja preuzimanje (na disk) od učitavanja (u memoriju). To vam omogućuje da modele unaprijed preuzmete tijekom podešavanja i učitavate ih po potrebi.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Opcija A: Ručni korak po korak
manager = FoundryLocalManager()
manager.start_service()

# Prvo provjerite predmemoriju
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

# Opcija B: Jednoredni bootstrap (preporučeno)
# Proslijedite alias konstruktoru - on automatski pokreće uslugu, preuzima i učitava
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Metode za upravljanje modelima

| Metoda | Potpis | Opis |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Preuzmi model u lokalni keš |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Učitaj model u inferencijski server |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Isključi model sa servera |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Nabroji sve trenutno učitane modele |

#### Python - Metode za upravljanje kešom

| Metoda | Potpis | Opis |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Dohvati put do direktorija keša |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Nabroji sve preuzete modele |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Pristup korak po korak
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

#### JavaScript - Metode modela

| Metoda | Potpis | Opis |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Je li model već preuzet |
| `model.download()` | `() => Promise<void>` | Preuzmi model u lokalni keš |
| `model.load()` | `() => Promise<void>` | Učitaj u inferencijski server |
| `model.unload()` | `() => Promise<void>` | Isključi iz inferencijskog servera |
| `model.id` | `string` | Jedinstveni identifikator modela |

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

#### C# - Metode modela

| Metoda | Opis |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Preuzmi odabranu varijantu |
| `model.LoadAsync()` | Učitaj model u memoriju |
| `model.UnloadAsync()` | Isključi model |
| `model.SelectVariant(variant)` | Odaberi specifičnu varijantu (CPU/GPU/NPU) |
| `model.SelectedVariant` | Trenutno odabrana varijanta |
| `model.Variants` | Sve dostupne varijante za ovaj model |
| `model.GetPathAsync()` | Dohvati lokalnu putanju datoteke |
| `model.GetChatClientAsync()` | Dohvati nativnog chat klijenta (nije potrebna OpenAI SDK) |
| `model.GetAudioClientAsync()` | Dohvati audio klijenta za transkripciju |

</details>

---

### Vježba 4: Ispitajte metapodatke modela

Objekt `FoundryModelInfo` sadrži bogate metapodatke o svakom modelu. Razumijevanje ovih polja pomaže vam da odaberete pravi model za vašu aplikaciju.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Dobijte detaljne informacije o određenom modelu
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
|-------|------|-------------|
| `alias` | string | Kratko ime (npr. `phi-3.5-mini`) |
| `id` | string | Jedinstveni identifikator modela |
| `version` | string | Verzija modela |
| `task` | string | `chat-completions` ili `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU ili NPU |
| `execution_provider` | string | Runtime backend (CUDA, CPU, QNN, WebGPU itd.) |
| `file_size_mb` | int | Veličina na disku u MB |
| `supports_tool_calling` | bool | Podržava li model pozivanje funkcija/alata |
| `publisher` | string | Tko je objavio model |
| `license` | string | Naziv licence |
| `uri` | string | URI modela |
| `prompt_template` | dict/null | Predložak prompta, ako postoji |

---

### Vježba 5: Upravljanje životnim ciklusom modela

Vježbajte cijeli životni ciklus: nabrajanje → preuzimanje → učitavanje → korištenje → isključivanje.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Mali model za brzo testiranje

manager = FoundryLocalManager()
manager.start_service()

# 1. Provjerite što se nalazi u katalogu
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Provjerite što je već preuzeto
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Preuzmite model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Provjerite je li sada u predmemoriji
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Učitajte ga
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Provjerite što je učitano
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Ispraznite ga
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

const alias = "qwen2.5-0.5b"; // Mali model za brzo testiranje

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Dohvati model iz kataloga
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Preuzmi ako je potrebno
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Učitaj ga
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Isprazni ga
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Vježba 6: Obrasci za Brzi Početak

Svaki jezik pruža prečac za pokretanje usluge i učitavanje modela u jednom pozivu. Ovo su **preporučeni obrasci** za većinu aplikacija.

<details>
<summary><h3>🐍 Python - Konstruktor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Proslijedite alias konstruktoru - on se pobrine za sve:
# 1. Pokreće servis ako nije pokrenut
# 2. Preuzima model ako nije u cacheu
# 3. Učitava model u serverski sustav za inferencu
manager = FoundryLocalManager("phi-3.5-mini")

# Odmah spremno za korištenje
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Parametar `bootstrap` (zadano `True`) kontrolira ovo ponašanje. Postavite `bootstrap=False` ako želite ručnu kontrolu:

```python
# Ručni način - ništa se ne događa automatski
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() rješavaju sve:
// 1. Pokreće servis
// 2. Dohvaća model iz kataloga
// 3. Preuzima ako je potrebno i učitava model
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Spreman za korištenje odmah
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

> **Napomena za C#:** C# SDK koristi `Configuration` za upravljanje nazivom aplikacije, zapisivanjem, direktorijima predmemorije, pa čak i za pinanje određenog porta web poslužitelja. To ga čini najsvestranijim od tri SDK-a.

</details>

---

### Vježba 7: Izvorni ChatClient (nije potreban OpenAI SDK)

JavaScript i C# SDK-ovi pružaju metodu `createChatClient()` koja vraća izvorni chat klijent — nije potrebno zasebno instalirati ili konfigurirati OpenAI SDK.

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

// Kreirajte ChatClient izravno iz modela — nije potreban uvoz OpenAI
const chatClient = model.createChatClient();

// completeChat vraća odgovor kompatibilan s OpenAI-jem
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming koristi obrazac poziva povratne funkcije
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` također podržava pozivanje alata — proslijedite alate kao drugi argument:

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

> **Kada koristiti koji obrazac:**
> - **`createChatClient()`** — Brzo prototipiranje, manje ovisnosti, jednostavniji kod
> - **OpenAI SDK** — Potpuna kontrola nad parametrima (temperatura, top_p, stop tokeni itd.), bolje za produkcijske aplikacije

---

### Vježba 8: Varijante Modela i Odabir Hardvera

Modeli mogu imati više **varijanti** optimiziranih za različiti hardver. SDK automatski odabire najbolju varijantu, ali ih možete i ručno pregledati i odabrati.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Nabrojite sve dostupne varijante
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK automatski odabire najbolju varijantu za vaš hardver
// Za preglasavanje, upotrijebite selectVariant():
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

U Pythonu, SDK automatski odabire najbolju varijantu na temelju hardvera. Koristite `get_model_info()` da vidite što je odabrano:

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

#### Modeli s NPU Varijantama

Neki modeli imaju varijante optimizirane za NPU uređaje (Qualcomm Snapdragon, Intel Core Ultra):

| Model | NPU varijanta dostupna |
|-------|:----------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Savjet:** Na hardveru s podrškom za NPU, SDK automatski odabire NPU varijantu kad je dostupna. Ne morate mijenjati svoj kod. Za C# projekte na Windowsu, dodajte NuGet paket `Microsoft.AI.Foundry.Local.WinML` kako biste omogućili QNN izvršni pružatelj — QNN se isporučuje kao dodatak EP putem WinML.

---

### Vježba 9: Nadogradnje Modela i Osvježavanje Kataloga

Katalog modela se periodično ažurira. Koristite ove metode za provjeru i primjenu nadogradnji.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Osvježi katalog da dobiješ najnoviji popis modela
manager.refresh_catalog()

# Provjeri ima li predmemorirani model dostupnu noviju verziju
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

// Osvježi katalog za dohvaćanje najnovije liste modela
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Popis svih dostupnih modela nakon osvježenja
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Vježba 10: Rad s Modelima za Razmišljanje

Model **phi-4-mini-reasoning** uključuje lančanu logiku razmišljanja. Omata svoje unutarnje razmišljanje u oznake `<think>...</think>` prije nego proizvede konačni odgovor. Ovo je korisno za zadatke koji zahtijevaju višestupanjsko logičko razmišljanje, matematiku ili rješavanje problema.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning zauzima oko 4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Model obavija svoje razmišljanje u oznake <think>...</think>
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

// Parsiranje razmišljanja u lancu
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Kada koristiti modele za razmišljanje:**
> - Problemi iz matematike i logike
> - Zadaci višestupanjskog planiranja
> - Kompleksna generacija koda
> - Zadaci gdje je prikaz rada od pomoći za preciznost
>
> **Kompenzacija:** Modeli za razmišljanje proizvode više tokena (dio `<think>`) i sporiji su. Za jednostavna pitanja i odgovore standardni model poput phi-3.5-mini je brži.

---

### Vježba 11: Razumijevanje Alias-a i Odabira Hardvera

Kada proslijedite **alias** (npr. `phi-3.5-mini`) umjesto punog ID-a modela, SDK automatski odabire najbolju varijantu za vaš hardver:

| Hardver | Odabrani izvršni pružatelj |
|----------|----------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (putem WinML dodatka) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Bilo koji uređaj (fallback) | `CPUExecutionProvider` ili `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Alijas se preslikava na najbolju varijantu za VAŠ hardver
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Savjet:** Uvijek koristite alias-e u svom aplikacijskom kodu. Kada implementirate na korisničkom računalu, SDK odabire optimalnu varijantu u runtime-u - CUDA za NVIDIA, QNN za Qualcomm, CPU drugdje.

---

### Vježba 12: Mogućnosti Konfiguracije u C#

C# SDK klasa `Configuration` pruža detaljnu kontrolu nad runtime-om:

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

| Svojstvo | Zadano | Opis |
|----------|---------|------|
| `AppName` | (obavezno) | Naziv vaše aplikacije |
| `LogLevel` | `Information` | Razina detalja logiranja |
| `Web.Urls` | (dinamički) | Pinanje određenog porta za web poslužitelj |
| `AppDataDir` | Zadan od OS-a | Osnovni direktorij za podatke aplikacije |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Lokacija spremanja modela |
| `LogsDir` | `{AppDataDir}/logs` | Lokacija zapisivanja logova |

---

### Vježba 13: Korištenje u Pregledniku (samo JavaScript)

JavaScript SDK uključuje verziju kompatibilnu s preglednikom. U pregledniku morate ručno pokrenuti uslugu putem CLI i navesti URL domaćina:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Prvo ručno pokrenite uslugu:
//   foundry service start
// Zatim koristite URL iz CLI izlaza
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Pregledajte katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Ograničenja preglednika:** Verzija za preglednik **ne podržava** `startWebService()`. Morate osigurati da je Foundry Local usluga već pokrenuta prije korištenja SDK-a u pregledniku.

---

## Kompletan API Reference

### Python

| Kategorija | Metoda | Opis |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Kreiraj manager; opcionalno bootstrap s modelom |
| **Service** | `is_service_running()` | Provjerava radi li usluga |
| **Service** | `start_service()` | Pokreni uslugu |
| **Service** | `endpoint` | URL API krajnje točke |
| **Service** | `api_key` | API ključ |
| **Catalog** | `list_catalog_models()` | Prikaz svih dostupnih modela |
| **Catalog** | `refresh_catalog()` | Osvježi katalog |
| **Catalog** | `get_model_info(alias_or_model_id)` | Dohvati podatke o modelu |
| **Cache** | `get_cache_location()` | Put do direktorija predmemorije |
| **Cache** | `list_cached_models()` | Popis preuzetih modela |
| **Model** | `download_model(alias_or_model_id)` | Preuzmi model |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Učitaj model |
| **Model** | `unload_model(alias_or_model_id)` | Isprazni model iz memorije |
| **Model** | `list_loaded_models()` | Popis trenutno učitanih modela |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Provjeri ima li novije verzije modela |
| **Model** | `upgrade_model(alias_or_model_id)` | Nadogradi model na najnoviju verziju |
| **Service** | `httpx_client` | Prekonfigurirani HTTPX klijent za direktne API pozive |

### JavaScript

| Kategorija | Metoda | Opis |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Inicijaliziraj SDK singleton |
| **Init** | `FoundryLocalManager.instance` | Pristup singleton manageru |
| **Service** | `manager.startWebService()` | Pokreni web uslugu |
| **Service** | `manager.urls` | Niz osnovnih URL-ova za uslugu |
| **Catalog** | `manager.catalog` | Pristup katalogu modela |
| **Catalog** | `catalog.getModel(alias)` | Dohvati model objekt po aliasu (vraća Promise) |
| **Model** | `model.isCached` | Je li model preuzet |
| **Model** | `model.download()` | Preuzmi model |
| **Model** | `model.load()` | Učitaj model |
| **Model** | `model.unload()` | Isprazni model iz memorije |
| **Model** | `model.id` | Jedinstveni identifikator modela |
| **Model** | `model.alias` | Alias modela |
| **Model** | `model.createChatClient()` | Dohvati izvorni chat klijent (nije potreban OpenAI SDK) |
| **Model** | `model.createAudioClient()` | Dohvati audio klijent za transkripciju |
| **Model** | `model.removeFromCache()` | Ukloni model iz lokalne predmemorije |
| **Model** | `model.selectVariant(variant)` | Odaberi specifičnu hardversku varijantu |
| **Model** | `model.variants` | Niz dostupnih varijanti za ovaj model |
| **Model** | `model.isLoaded()` | Provjeri je li model trenutno učitan |
| **Catalog** | `catalog.getModels()` | Prikaži sve dostupne modele |
| **Catalog** | `catalog.getCachedModels()` | Prikaži preuzete modele |
| **Catalog** | `catalog.getLoadedModels()` | Prikaži trenutno učitane modele |
| **Catalog** | `catalog.updateModels()` | Osvježi katalog sa servisa |
| **Service** | `manager.stopWebService()` | Zaustavi Foundry Local web uslugu |

### C# (v0.8.0+)

| Kategorija | Metoda | Opis |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Inicijaliziraj manager |
| **Init** | `FoundryLocalManager.Instance` | Pristup singletonu |
| **Catalog** | `manager.GetCatalogAsync()` | Dohvati katalog |
| **Catalog** | `catalog.ListModelsAsync()` | Prikaži sve modele |
| **Catalog** | `catalog.GetModelAsync(alias)` | Dohvati specifičan model |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Prikaži predmemorirane modele |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Prikaži učitane modele |
| **Model** | `model.DownloadAsync(progress?)` | Preuzmi model |
| **Model** | `model.LoadAsync()` | Učitaj model |
| **Model** | `model.UnloadAsync()` | Isprazni model iz memorije |
| **Model** | `model.SelectVariant(variant)` | Odaberi hardversku varijantu |
| **Model** | `model.GetChatClientAsync()` | Dohvati izvorni chat klijent |
| **Model** | `model.GetAudioClientAsync()` | Dohvati audio klijent za transkripciju |
| **Model** | `model.GetPathAsync()` | Dohvati lokalnu putanju datoteke |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Dohvati specifičnu hardversku varijantu |
| **Catalog** | `catalog.UpdateModelsAsync()` | Osvježi katalog |
| **Server** | `manager.StartWebServerAsync()` | Pokreni REST web poslužitelj |
| **Server** | `manager.StopWebServerAsync()` | Zaustavi REST web poslužitelj |
| **Config** | `config.ModelCacheDir` | Direktorij predmemorije |

---

## Ključne Zabilješke

| Pojam | Ono što ste naučili |
|---------|-----------------|
| **SDK vs CLI** | SDK pruža programatsku kontrolu - ključnu za aplikacije |
| **Kontrolna ravnina** | SDK upravlja uslugama, modelima i predmemorijom |
| **Dinamički portovi** | Uvijek koristite `manager.endpoint` (Python) ili `manager.urls[0]` (JS/C#) - nikada ne hardkodirajte port |
| **Alias-i** | Koristite alias-e za automatski odabir optimalne varijante za hardver |
| **Brzi početak** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Redizajn u C#** | v0.8.0+ je samostalan - nije potreban CLI na korisničkim računalima |
| **Životni ciklus modela** | Katalog → Preuzimanje → Učitavanje → Korištenje → Isprazniti |
| **FoundryModelInfo** | Bogati metapodaci: zadatak, uređaj, veličina, licenca, podrška za pozivanje alata |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) za korištenje bez OpenAI |
| **Varijante** | Modeli imaju varijante specifične za hardver (CPU, GPU, NPU); automatski se odabiru |
| **Nadogradnje** | Python: `is_model_upgradeable()` + `upgrade_model()` za održavanje modela ažuriranim |
| **Osvježavanje kataloga** | `refresh_catalog()` (Python) / `updateModels()` (JS) za otkrivanje novih modela |

---

## Resursi

| Resurs | Veza |
|----------|------|
| SDK referenca (svi jezici) | [Microsoft Learn - Foundry Local SDK referenca](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integracija s inference SDK-ovima | [Microsoft Learn - Integracija s Inference SDK-ovima](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API referenca | [Foundry Local C# API referenca](https://aka.ms/fl-csharp-api-ref) |
| C# SDK Primjeri | [GitHub - Foundry Local SDK primjeri](https://aka.ms/foundrylocalSDK) |
| Foundry Local web stranica | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Sljedeći koraci

Nastavite na [Dio 3: Korištenje SDK-a s OpenAI](part3-sdk-and-apis.md) za povezivanje SDK-a s OpenAI klijentskom knjižnicom i izgradnju vaše prve aplikacije za chat dovršenje.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Izjava o odricanju od odgovornosti**:  
Ovaj je dokument preveden korištenjem AI usluge za prevođenje [Co-op Translator](https://github.com/Azure/co-op-translator). Iako težimo točnosti, imajte na umu da automatizirani prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na izvornom jeziku smatra se autoritativnim izvorom. Za kritične informacije preporučuje se profesionalni ljudski prijevod. Ne snosimo odgovornost za bilo kakva nesporazuma ili pogrešne interpretacije koje proizlaze iz korištenja ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->