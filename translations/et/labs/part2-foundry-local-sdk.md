![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 2: Foundry Local SDK põhjalik ülevaade

> **Eesmärk:** Valdada Foundry Local SDK kasutamist mudelite, teenuste ja vahemälu programmipõhiseks haldamiseks - ning mõista, miks SDK on soovituslik lähenemine CLI asemel rakenduste ehitamiseks.

## Ülevaade

Esimeses osas kasutasite **Foundry Local CLI-d** mudelite allalaadimiseks ja interaktiivseks töötamiseks. CLI sobib hästi avastamiseks, kuid pärisrakenduste ehitamiseks on vaja **programmilist juhtimist**. Foundry Local SDK pakub seda - see haldab **juhtimistasandit** (teenuse käivitamine, mudelite leidmine, allalaadimine, laadimine), nii et teie rakenduse kood saab keskenduda **andmetasandile** (põhjuste saatmine, vastuste vastuvõtt).

See õpikäsiraamat õpetab teile SDK täielikku API-pinnakut pythoni, Javascripti ja C# keeles. Lõpuks mõistate kõiki saadaolevaid meetodeid ja millal kasutada ühte või teist.

## Õpieesmärgid

Selle labori lõpuks oskate:

- Selgitada, miks SDK-d eelistatakse CLI-le rakenduste arendamisel
- Paigaldada Foundry Local SDK Pythoni, Javascripti või C# jaoks
- Kasutada `FoundryLocalManager` teenuse käivitamiseks, mudelite haldamiseks ja kataloogi pärimiseks
- Programmeerimisviisiliselt loetleda, alla laadida, laadida ja laadida mudelit
- Kontrollida mudeli metainfot kasutades `FoundryModelInfo`
- Mõista kataloogi, vahemälu ja laetud mudelite erinevust
- Kasutada konstruktorit bootstrapiks (Python) ja `create()` + kataloogi mustrit (Javascript)
- Mõista C# SDK ümberkujundust ja objekti-orienteeritud API-d

---

## Eeltingimused

| Nõue | Detailid |
|-------------|---------|
| **Foundry Local CLI** | Paigaldatud ja teie `PATH`-is ([Osa 1](part1-getting-started.md)) |
| **Keele runtime** | **Python 3.9+** ja/või **Node.js 18+** ja/või **.NET 9.0+** |

---

## Mõiste: SDK vs CLI - miks kasutada SDK-d?

| Aspekt | CLI (`foundry` käsk) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Kasutusjuhtum** | Avastamine, käsitsi testimine | Rakendusintegratsioon |
| **Teenuse juhtimine** | Käsitsi: `foundry service start` | Automaatne: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Pordi avastamine** | Loeb CLI väljundist | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Mudeli allalaadimine** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Veahaldus** | Väljumiskoodid, stderr | Väljaanded, tüübitud vead |
| **Automatiseerimine** | Shell-skriptid | Natiivne keele integratsioon |
| **Deploy** | nõuab CLI lõppkasutaja masinas | C# SDK võib olla iseseisev (ei vaja CLI-d) |

> **Oluline tõdemus:** SDK haldab kogu elutsüklit: teenuse käivitamist, vahemälu kontrollimist, puuduolevate mudelite allalaadimist, nende laadimist ja lõpp-punkti avastamist mõne koodireaga. Teie rakendus ei pea analüüsima CLI väljundit ega haldama alamprotsesse.

---

## Labori ülesanded

### Ülesanne 1: SDK paigaldamine

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Kontrolli paigaldust:

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

Kontrolli paigaldust:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Saadaval on kaks NuGet paketti:

| Pakett | Platvorm | Kirjeldus |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Platvormideülene | Töötab Windows, Linux, macOS platvormidel |
| `Microsoft.AI.Foundry.Local.WinML` | Ainult Windows | Lisab WinML riistvarakiirenduse; laadib ja paigaldab pistiku täitja pakkujad (nt QNN Qualcomm NPU jaoks) |

**Windowsi seadmestus:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Muuda `.csproj` faili:

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

> **Märkus:** Windowsis on WinML pakett laiem, sisaldades põhiskeemi koos QNN täitja pakkujaga. Linux/macOS puhul kasutatakse baasil SDK-d. Tingimuslik TFM ja paketiviited hoiavad projekti täielikult platvormideülesena.

Loo projekti juurkausta `nuget.config`:

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

Taasta paketid:

```bash
dotnet restore
```

</details>

---

### Ülesanne 2: Teenuse käivitamine ja kataloogi loendamine

Iga rakendus esmalt käivitab Foundry Local teenuse ja avastab, millised mudelid on olemas.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Loo haldur ja alusta teenust
manager = FoundryLocalManager()
manager.start_service()

# Loetle kõik kataloogis olevad mudelid
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Teenuse haldamise meetodid

| Meetod | Signatuur | Kirjeldus |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Kontrolli, kas teenus töötab |
| `start_service()` | `() -> None` | Käivita Foundry Local teenus |
| `service_uri` | `@property -> str` | Teenuse põhi-URI |
| `endpoint` | `@property -> str` | API lõpp-punkt (teenuse URI + `/v1`) |
| `api_key` | `@property -> str` | API võti (keskkonnamuutujast või vaikekohastust) |

#### Python SDK - Kataloogi haldamise meetodid

| Meetod | Signatuur | Kirjeldus |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Loetle kõik mudelid kataloogis |
| `refresh_catalog()` | `() -> None` | Värskenda kataloogi teenusest |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Hangi info kindla mudeli kohta |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Loo haldur ja alusta teenust
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Sirvi kataloogi
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Manager'i meetodid

| Meetod | Signatuur | Kirjeldus |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK singleton'i algatamine |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Singleton manageri ligipääs |
| `manager.startWebService()` | `() => Promise<void>` | Käivita Foundry Local veebiteenus |
| `manager.urls` | `string[]` | Veebi teenuse baasil URL-ide massiiv |

#### JavaScript SDK - Kataloogi ja mudeli meetodid

| Meetod | Signatuur | Kirjeldus |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Ligipääs mudeli kataloogile |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Hangi mudel objekti alias'ega |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ kasutab objekti-orienteeritud arhitektuuri koos `Configuration`, `Catalog` ja `Model` objektidega:

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

#### C# SDK - Põhiklassid

| Klass | Eesmärk |
|-------|---------|
| `Configuration` | Määra rakenduse nimi, logitasand, vahemälu kaust, veebiserveri URLid |
| `FoundryLocalManager` | Peamine sisenemispunkt - luuakse `CreateAsync()` kaudu, ligipääs `.Instance` kaudu |
| `Catalog` | Sirvi, otsi ja saa mudelid kataloogist |
| `Model` | Esindab kindlat mudelit - allalaadimine, laadimine, klientide saamine |

#### C# SDK - Manager ja kataloogi meetodid

| Meetod | Kirjeldus |
|--------|-----------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Algatab manageri |
| `FoundryLocalManager.Instance` | Ligipääs singleton managerile |
| `manager.GetCatalogAsync()` | Saa mudeli kataloog |
| `catalog.ListModelsAsync()` | Loetle kõik kättesaadavad mudelid |
| `catalog.GetModelAsync(alias: "alias")` | Saa kindel mudel alias'iga |
| `catalog.GetCachedModelsAsync()` | Loetle allalaaditud mudelid |
| `catalog.GetLoadedModelsAsync()` | Loetle hetkel laetud mudelid |

> **C# arhitektuuri märkus:** C# SDK v0.8.0+ ümberkujundus teeb rakenduse **iseseisvaks**; see ei nõua Foundry Local CLI olemasolu lõppkasutaja masinas. SDK haldab mudeli haldust ja järeldamist natiivselt.

</details>

---

### Ülesanne 3: Mudeli allalaadimine ja laadimine

SDK eristab allalaadimise (kettale) ja laadimise (mälu) protsessid. See võimaldab mugavalt enne kasutust mudeleid alla laadida ja vajadusel laadida.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Variant A: käsitsi samm-sammult
manager = FoundryLocalManager()
manager.start_service()

# Kontrolli esmalt vahemälu
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

# Variant B: Ühe rea bootstrapi kood (soovitatav)
# Saada alias konstruktorile - see käivitab teenuse, laeb alla ja laadib automaatselt
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Mudelite haldamise meetodid

| Meetod | Signatuur | Kirjeldus |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Laadi mudel kohalikku vahemällu |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Laadi mudel järeldamisteenusesse |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Eemalda mudel teenusest |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Loetle kõik hetkel laetud mudelid |

#### Python - Vahemälu haldamise meetodid

| Meetod | Signatuur | Kirjeldus |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Saa vahemälu asukoha kataloogitee |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Loetle kõik alla laaditud mudelid |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Samm-sammult lähenemine
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

#### JavaScript - Mudeli meetodid

| Meetod | Signatuur | Kirjeldus |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Kas mudel on juba alla laaditud |
| `model.download()` | `() => Promise<void>` | Laadi mudel kohalikku vahemällu |
| `model.load()` | `() => Promise<void>` | Laadi järeldamisteenusesse |
| `model.unload()` | `() => Promise<void>` | Eemalda järeldamisteenusest |
| `model.id` | `string` | Mudeli unikaalne identifikaator |

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

#### C# - Mudeli meetodid

| Meetod | Kirjeldus |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Laadi valitud variant alla |
| `model.LoadAsync()` | Laadi mudel mällu |
| `model.UnloadAsync()` | Eemalda mudel |
| `model.SelectVariant(variant)` | Vali spetsiifiline variant (CPU/GPU/NPU) |
| `model.SelectedVariant` | Hetkel valitud variant |
| `model.Variants` | Kõik mudeli jaoks saadaval variandid |
| `model.GetPathAsync()` | Saa kohalik failitee |
| `model.GetChatClientAsync()` | Saa natiivne jutuklient (EI vaja OpenAI SDK-d) |
| `model.GetAudioClientAsync()` | Saa heli klient transkriptsiooniks |

</details>

---

### Ülesanne 4: Mudeli metainfo kontrollimine

`FoundryModelInfo` objekt sisaldab rikkalikku metainfot iga mudeli kohta. Nende väljade mõistmine aitab valida rakendusele sobiva mudeli.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Hangi üksikasjalikku teavet konkreetse mudeli kohta
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

#### FoundryModelInfo väljad

| Väli | Tüüp | Kirjeldus |
|-------|------|-------------|
| `alias` | string | Lühinimi (nt `phi-3.5-mini`) |
| `id` | string | Unikaalne mudeli identifikaator |
| `version` | string | Mudeli versioon |
| `task` | string | `chat-completions` või `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU või NPU |
| `execution_provider` | string | Käitusaegne tagakeha (CUDA, CPU, QNN, WebGPU jne) |
| `file_size_mb` | int | Kettal suurus MB-des |
| `supports_tool_calling` | bool | Kas mudel toetab funktsiooni/tööriista kutsumist |
| `publisher` | string | Mudeli avaldaja |
| `license` | string | Litsentsi nimi |
| `uri` | string | Mudeli URI |
| `prompt_template` | dict/null | Põhjuste mall, kui on olemas |

---

### Ülesanne 5: Mudeli elutsükli haldamine

Harjuta kogu elutsüklit: loetle → laadi alla → laadi → kasuta → eemalda.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Väike mudel kiireks testimiseks

manager = FoundryLocalManager()
manager.start_service()

# 1. Kontrolli, mis kataloogis on
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Kontrolli, mis on juba alla laaditud
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Laadi mudel alla
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Kontrolli, et see on nüüd vahemälus
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Laadi see
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Kontrolli, mis on laetud
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Lae see maha
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

const alias = "qwen2.5-0.5b"; // Väike mudel kiireks testimiseks

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Hangi mudel kataloogist
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Laadi alla, kui vaja
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Lae see
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Tühjenda see
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Harjutus 6: Kiire algus mustrid

Iga keel pakub otseteed teenuse käivitamiseks ja mudeli laadimiseks ühe kõnega. Need on enamikule rakendustele **soovitatavad mustrid**.

<details>
<summary><h3>🐍 Python - Konstruktor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Edastage konstruktorile alias - see haldab kõike:
# 1. Käivitab teenuse, kui see ei tööta
# 2. Laadib mudeli alla, kui see pole vahemällu salvestatud
# 3. Laeb mudeli järeldusserverisse
manager = FoundryLocalManager("phi-3.5-mini")

# Koheseks kasutamiseks valmis
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Parameeter `bootstrap` (vaikimisi `True`) kontrollib seda käitumist. Määra `bootstrap=False`, kui soovid käsitsi kontrolli:

```python
# Käsitsi režiim - midagi ei juhtu automaatselt
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Kataloog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() hõlmab kõike:
// 1. Teenuse käivitamine
// 2. Mudeli saamine kataloogist
// 3. Vajadusel allalaadimine ja mudeli laadimine
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Koheselt kasutamiseks valmis
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Kataloog</h3></summary>

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

> **C# Märkus:** C# SDK kasutab `Configuration`-i rakenduse nime, logimise, vahemälu kataloogide ning isegi konkreetse veebiserveri pordi määramiseks. See teeb sellest kolme SDK kõige konfigureeritavama variandi.

</details>

---

### Harjutus 7: Native ChatClient (OpenAI SDK-d pole vaja)

JavaScripti ja C# SDK-d pakuvad mugavusmeetodit `createChatClient()`, mis tagastab natuke vestlusklienti — eraldi OpenAI SDK paigaldust või seadistust ei ole vaja.

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

// Loo ChatClient otse mudelist — OpenAI importi pole vaja
const chatClient = model.createChatClient();

// completeChat tagastab OpenAI-ga ühilduva vastuse objekti
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Striiming kasutab tagasikutsumise ehk callback mustrit
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` toetab ka tööriistade kutsumist — edasta tööriistad teise argumendina:

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

> **Millal kasutada millist mustrit:**
> - **`createChatClient()`** — Kiire prototüüpimine, vähem sõltuvusi, lihtsam kood
> - **OpenAI SDK** — Täielik kontroll parameetrite üle (temperatuur, top_p, stop tokenid jne), sobib paremini tootmiskeskkonda

---

### Harjutus 8: Mudeli variandid ja riistvara valik

Mudelitele võivad eksisteerida mitmed **variandid**, mis on optimeeritud erinevale riistvarale. SDK valib automaatselt parima variandi, kuid saad ka ise uurida ja valida käsitsi.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Loetlege kõik saadaolevad variandid
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK valib automaatselt teie riistvarale parima variandi
// Üle kirjutamiseks kasutage selectVariant():
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

Pythonis valib SDK automaatselt parima variandi riistvara põhjal. Kasuta `get_model_info()` vaatamaks, mida valiti:

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

#### Mudelid koos NPU variantidega

Mõned mudelid pakuvad NPU-optimeeritud variante seadmetele, kus on närvivõrgu töötlemisüksused (Qualcomm Snapdragon, Intel Core Ultra):

| Mudel | NPU variant saadaval |
|-------|:--------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Näpunäide:** NPU-ga riistvaral valib SDK automaatselt, kui saadaval, NPU variandi. Koodi enda pole tarvis muuta. C# projektide puhul Windowsis lisa NuGet paketiks `Microsoft.AI.Foundry.Local.WinML`, et lubada QNN täitmistalent — QNN tuuakse pluginina EP-na WinML kaudu.

---

### Harjutus 9: Mudelite uuendused ja kataloogi värskendus

Mudelike kataloog uuendatakse perioodiliselt. Kasuta neid meetodeid uuenduste kontrollimiseks ja rakendamiseks.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Värskenda kataloogi, et saada uusim mudeliloend
manager.refresh_catalog()

# Kontrolli, kas vahemällu salvestatud mudelil on saadaval uuem versioon
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

// Värskenda kataloogi, et hankida uusim mudeliloend
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Loetle kõik saadaolevad mudelid pärast värskendamist
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Harjutus 10: Töötamine loogikamudelitega

Mudelis **phi-4-mini-reasoning** on ahela mõtlemise loogika. See ümbritseb oma sisemist mõtlemist `<think>...</think>` siltidega enne lõpliku vastuse andmist. See on kasulik ülesannete puhul, mis vajavad mitmeastmelist loogikat, matemaatikat või probleemi lahendamist.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning on ~4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Mudel ümbritseb oma mõtlemise siltidega <think>...</think>
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

// Tõlgenda järeldusketi mõtlemist
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Millal kasutada loogikamudeleid:**
> - Matemaatika ja loogikaprobleemid
> - Mitmeastmelised planeerimisülesanded
> - Kompleksne koodi genereerimine
> - Ülesanded, kus töö protsessi kuvamine parandab täpsust
>
> **Kompromiss:** Loogikamudelid genereerivad rohkem tokeneid (sealhulgas `<think>` osa) ja töötlevad aeglasemalt. Lihtsate küsimuste-vastuste jaoks on phi-3.5-mini tavaliselt kiirem.

---

### Harjutus 11: Alias’te ja riistvara valik mõistmine

Kui sa edastad **aliase** (nt `phi-3.5-mini`) täismudeli ID asemel, valib SDK automaatselt parima variandi sinu riistvarale:

| Riistvara | Valitud täitmise pakkuja |
|----------|--------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML plugin kaudu) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Mis tahes seade (varuvariandiks) | `CPUExecutionProvider` või `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Alias lahendab parima variandi TEIE riistvarale
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Näpunäide:** Kasuta alati oma rakenduse koodis aliasi. Kui rakenduse kasutaja masinasse selle paigaldad, valib SDK õigel ajal jooksvalt optimaalse variandi - CUDA NVIDIA-l, QNN Qualcommil, CPU mujal.

---

### Harjutus 12: C# konfiguratsiooni valikud

C# SDK `Configuration` klass pakub täpset kontrolli käitusaja üle:

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

| Omadus | Vaikimisi | Kirjeldus |
|--------|-----------|-----------|
| `AppName` | (nõutud) | Sinu rakenduse nimi |
| `LogLevel` | `Information` | Logimise detailsusaste |
| `Web.Urls` | (dünaamiline) | Fikseeri veebiserveri kindel port |
| `AppDataDir` | OS vaikeseade | Rakenduse andmete baaskataloog |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Koht, kuhu mudelid salvestatakse |
| `LogsDir` | `{AppDataDir}/logs` | Logsid kirjutamise kataloog |

---

### Harjutus 13: Brauseri kasutus (ainult JavaScript)

JavaScriptil on brauseriga ühilduv versioon. Brauseris pead teenuse käsitsi CLI kaudu käivitama ja määrama hosti URL-i:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Käivitage teenus esmalt käsitsi:
//   foundry service start
// Seejärel kasutage CLI väljundist leitud URL-i
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Sirvige kataloogi
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Brauseri piirangud:** Brauseri versioon EI toeta `startWebService()` meetodit. Sa pead enne SDK kasutamist brauseris veenduma, et Foundry Local teenus töötab juba.

---

## Täielik API viide

### Python

| Kategooria | Meetod | Kirjeldus |
|------------|--------|-----------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Loo haldur; vali mudeliga käivitamine |
| **Teenuse** | `is_service_running()` | Kontrolli, kas teenus töötab |
| **Teenuse** | `start_service()` | Käivita teenus |
| **Teenuse** | `endpoint` | API lõpp-punkti URL |
| **Teenuse** | `api_key` | API võti |
| **Kataloog** | `list_catalog_models()` | Listi saadaolevad mudelid |
| **Kataloog** | `refresh_catalog()` | Värskenda kataloogi |
| **Kataloog** | `get_model_info(alias_or_model_id)` | Saad mudeli metaandmed |
| **Vahemälu** | `get_cache_location()` | Vahemälu kataloogitee |
| **Vahemälu** | `list_cached_models()` | Loetelu allalaaditud mudelitest |
| **Mudeli** | `download_model(alias_or_model_id)` | Lae mudel alla |
| **Mudeli** | `load_model(alias_or_model_id, ttl=600)` | Lae mudel mällu |
| **Mudeli** | `unload_model(alias_or_model_id)` | Eemalda mudel mälust |
| **Mudeli** | `list_loaded_models()` | Loetelu laaditud mudelitest |
| **Mudeli** | `is_model_upgradeable(alias_or_model_id)` | Kontrolli uuendatavust |
| **Mudeli** | `upgrade_model(alias_or_model_id)` | Täienda mudelit uusimale versioonile |
| **Teenuse** | `httpx_client` | Eelseadistatud HTTPX klient API kutseteks |

### JavaScript

| Kategooria | Meetod | Kirjeldus |
|------------|--------|-----------|
| **Init** | `FoundryLocalManager.create(options)` | Initsialiseeri SDK singelton |
| **Init** | `FoundryLocalManager.instance` | Juurdepääs singelton haldurile |
| **Teenuse** | `manager.startWebService()` | Käivita veebi teenus |
| **Teenuse** | `manager.urls` | Teenuse baas URL-ide massiiv |
| **Kataloog** | `manager.catalog` | Ligipääs mudeli kataloogile |
| **Kataloog** | `catalog.getModel(alias)` | Saa mudel objekti alias järgi (Promise) |
| **Mudeli** | `model.isCached` | Kas mudel on alla laetud |
| **Mudeli** | `model.download()` | Lae mudel alla |
| **Mudeli** | `model.load()` | Lae mudel mällu |
| **Mudeli** | `model.unload()` | Eemalda mudel mälust |
| **Mudeli** | `model.id` | Mudeli unikaalne ID |
| **Mudeli** | `model.alias` | Mudeli alias |
| **Mudeli** | `model.createChatClient()` | Saa natuke vestlusklienti (OpenAI SDK pole vaja) |
| **Mudeli** | `model.createAudioClient()` | Saa heli transkriptsiooni klient |
| **Mudeli** | `model.removeFromCache()` | Eemalda mudel lokaalsest vahemälust |
| **Mudeli** | `model.selectVariant(variant)` | Vali kindel riistvara variant |
| **Mudeli** | `model.variants` | Mudeli saadavalolevate variantide massiiv |
| **Mudeli** | `model.isLoaded()` | Kontrolli, kas mudel on laaditud |
| **Kataloog** | `catalog.getModels()` | Loetelu kõigist mudelitest |
| **Kataloog** | `catalog.getCachedModels()` | Loetelu allalaetud mudelitest |
| **Kataloog** | `catalog.getLoadedModels()` | Loetelu praegu laaditud mudelitest |
| **Kataloog** | `catalog.updateModels()` | Värskenda kataloogi teenusest |
| **Teenuse** | `manager.stopWebService()` | Peata Foundry Local veebi teenus |

### C# (v0.8.0+)

| Kategooria | Meetod | Kirjeldus |
|------------|--------|-----------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Initsialiseeri haldur |
| **Init** | `FoundryLocalManager.Instance` | Juurdepääs singeltonile |
| **Kataloog** | `manager.GetCatalogAsync()` | Saa kataloog |
| **Kataloog** | `catalog.ListModelsAsync()` | Loetelu kõigist mudelitest |
| **Kataloog** | `catalog.GetModelAsync(alias)` | Saa konkreetne mudel |
| **Kataloog** | `catalog.GetCachedModelsAsync()` | Loetelu vahemällu salvestatud mudelitest |
| **Kataloog** | `catalog.GetLoadedModelsAsync()` | Loetelu laaditud mudelitest |
| **Mudeli** | `model.DownloadAsync(progress?)` | Lae mudel alla |
| **Mudeli** | `model.LoadAsync()` | Lae mudel mällu |
| **Mudeli** | `model.UnloadAsync()` | Eemalda mudel mälust |
| **Mudeli** | `model.SelectVariant(variant)` | Vali riistvara variant |
| **Mudeli** | `model.GetChatClientAsync()` | Saa native vestlusklient |
| **Mudeli** | `model.GetAudioClientAsync()` | Saa heli transkriptsiooni klient |
| **Mudeli** | `model.GetPathAsync()` | Saa lokaalne failitee |
| **Kataloog** | `catalog.GetModelVariantAsync(alias, variant)` | Saa konkreetne riistvaraline variant |
| **Kataloog** | `catalog.UpdateModelsAsync()` | Värskenda kataloogi |
| **Server** | `manager.StartWebServerAsync()` | Käivita REST-veebiserver |
| **Server** | `manager.StopWebServerAsync()` | Peata REST-veebiserver |
| **Konfiguratsioon** | `config.ModelCacheDir` | Vahemälu kataloog |

---

## Olulised võtmed

| Kontseptsioon | Mida Sa Õppisid |
|--------------|-----------------|
| **SDK vs CLI** | SDK pakub programmeerimislikku kontrolli - oluline rakendustele |
| **Juhtimistasand** | SDK haldab teenuseid, mudeleid ja vahemällu salvestamist |
| **Dünaamilised pordid** | Kasuta alati `manager.endpoint` (Python) või `manager.urls[0]` (JS/C#) - ära puhul nii porti koodi sisse kirjuta |
| **Alias’d** | Kasuta alias’i automaatseks riistvaraliselt optimaalse mudeli valimiseks |
| **Kiire alustamine** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# ümberkujundus** | v0.8.0+ on iseseisev - kasutajamasinates pole CLI vaja |
| **Mudelitsükkel** | Kataloog → Laadi alla → Laadi → Kasuta → Laadi maha |
| **FoundryModelInfo** | Rikkalikud metaandmed: ülesanne, seade, suurus, litsents, tööriista kutsumise tugi |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) OpenAI-vabaks kasutamiseks |
| **Variandid** | Mudelitel on riistvaraspetsiifilised variandid (CPU, GPU, NPU); valitakse automaatselt |
| **Uuendused** | Python: `is_model_upgradeable()` + `upgrade_model()` mudelite ajakohasena hoidmiseks |
| **Kataloogi värskendus** | `refresh_catalog()` (Python) / `updateModels()` (JS) uute mudelite avastamiseks |

---

## Ressursid

| Ressurss | Link |
|----------|------|
| SDK viide (kõik keeled) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integreerimine inference SDK-dega | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API viide | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK näited | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local veebisait | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Järgmised sammud

Jätka [Osa 3: SDK kasutamine OpenAI-ga](part3-sdk-and-apis.md), et ühendada SDK OpenAI klienditeegiga ja luua oma esimene vestluse lõpetamise rakendus.