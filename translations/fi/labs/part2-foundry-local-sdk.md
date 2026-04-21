![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 2: Foundry Local SDK Syventävä Käsittely

> **Tavoite:** Hallitse Foundry Local SDK:tä mallien, palveluiden ja välimuistin ohjelmalliseen hallintaan - ja ymmärrä, miksi SDK on suositeltu tapa verrattuna CLI:hin sovellusten rakentamisessa.

## Yleiskatsaus

Osa 1:ssä käytit **Foundry Local CLI** -työkalua ladataksesi ja ajaaksesi malleja interaktiivisesti. CLI on erinomainen tutkimiseen, mutta oikeiden sovellusten rakentamisessa tarvitset **ohjelmallista hallintaa**. Foundry Local SDK tarjoaa tämän - se hallinnoi **ohjaustasoa** (palvelun käynnistys, mallien löytäminen, lataaminen, kuormaus) jotta sovelluskoodisi voi keskittyä **datatasoon** (kehotteiden lähettäminen, täydentymien vastaanottaminen).

Tämä harjoitus opettaa sinulle koko SDK:n API-pinnan Pythonissa, JavaScriptissä ja C#:ssa. Lopuksi ymmärrät jokaisen käytettävissä olevan metodin ja milloin kutakin tulee käyttää.

## Oppimistavoitteet

Tämän harjoituksen jälkeen osaat:

- Selittää, miksi SDK on parempi kuin CLI sovelluskehityksessä
- Asentaa Foundry Local SDK:n Pythonille, JavaScriptille tai C#:lle
- Käyttää `FoundryLocalManager`ia palvelun käynnistämiseen, mallien hallintaan ja katalogin kyselyyn
- Luetteloida, ladata, kuormata ja purkaa malleja ohjelmallisesti
- Tutkia mallin metatietoja `FoundryModelInfo`lla
- Ymmärtää erot katalogin, välimuistin ja ladattujen mallien välillä
- Käyttää konstruktori-bootstrapia (Python) ja `create()` + katalogi -mallia (JavaScript)
- Ymmärtää C# SDK:n uudelleensuunnittelun ja sen olio-ohjatun API:n

---

## Esivaatimukset

| Vaatimus | Tiedot |
|----------|--------|
| **Foundry Local CLI** | Asennettuna ja polulla (`PATH`) ([Osa 1](part1-getting-started.md)) |
| **Kieliajoalusta** | **Python 3.9+** ja/tai **Node.js 18+** ja/tai **.NET 9.0+** |

---

## Konsepti: SDK vs CLI - Miksi Käyttää SDK:ta?

| Ominaisuus | CLI (`foundry` komento) | SDK (`foundry-local-sdk`) |
|------------|-------------------------|---------------------------|
| **Käyttötarkoitus** | Tutkiminen, manuaalinen testaus | Sovelluksen integrointi |
| **Palvelun hallinta** | Manuaalinen: `foundry service start` | Automaattinen: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Portin löytäminen** | Luetaan CLI:n tulosteesta | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Mallin lataus** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Virheiden käsittely** | Poistumiskoodit, stderr | Poikkeukset, tyypitetyt virheet |
| **Automaatio** | Shell-skriptit | Natiivi kielten integraatio |
| **Jakeluprosessi** | CLI tarvitaan loppukäyttäjän koneella | C# SDK voi olla itsenäinen (ei CLI:tä tarvita) |

> **Keskeinen havainto:** SDK hoitaa koko elinkaaren: palvelun käynnistyksen, välimuistin tarkistuksen, puuttuvien mallien latauksen, niiden kuormauksen ja päätepisteen löytämisen muutamassa koodirivissä. Sovelluksesi ei tarvitse jäsentää CLI:n tulostetta tai hallita aliohjelmia.

---

## Labraharkat

### Harjoitus 1: SDK:n Asennus

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Varmista asennus:

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

Varmista asennus:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Näitä on kaksi NuGet-pakettia:

| Paketti | Alusta | Kuvaus |
|---------|---------|---------|
| `Microsoft.AI.Foundry.Local` | Monialustainen | Toimii Windowsissa, Linuxissa, macOS:ssä |
| `Microsoft.AI.Foundry.Local.WinML` | Vain Windows | Lisää WinML-laitteistokiihdytyksen; lataa ja asennus plug-in -suorittajat (esim. QNN Qualcomm NPU:lle) |

**Windowsin asetus:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Muokkaa `.csproj` -tiedostoa:

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

> **Huom:** Windowsissa WinML-paketti on laajennettu paketti, johon sisältyy perus-SDK sekä QNN-suorittaja. Linux/macOS-käytössä käytetään perus-SDK:ta. Ehdolliset TFM- ja pakettiviitteet pitävät projektin täysin monialustaisena.

Luo `nuget.config` projektin juureen:

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

Palauta paketit:

```bash
dotnet restore
```

</details>

---

### Harjoitus 2: Käynnistä Palvelu ja Listaa Katalogi

Mikä tahansa sovellus aloittaa Foundry Local -palvelun käynnistämisellä ja saatavilla olevien mallien löytymisellä.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Luo hallinnoija ja käynnistä palvelu
manager = FoundryLocalManager()
manager.start_service()

# Listaa kaikki katalogissa saatavilla olevat mallit
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Palvelun Hallintametodit

| Metodi | Signatuuri | Kuvaus |
|--------|------------|--------|
| `is_service_running()` | `() -> bool` | Tarkistaa, onko palvelu käynnissä |
| `start_service()` | `() -> None` | Käynnistää Foundry Local -palvelun |
| `service_uri` | `@property -> str` | Palvelun perus-URI |
| `endpoint` | `@property -> str` | API-päätepiste (palvelun URI + `/v1`) |
| `api_key` | `@property -> str` | API-avain (ympäristöstä tai oletustilasta) |

#### Python SDK - Katalogin Hallintametodit

| Metodi | Signatuuri | Kuvaus |
|--------|------------|--------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Listaa kaikki mallikatalogin mallit |
| `refresh_catalog()` | `() -> None` | Päivittää katalogin palvelusta |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Hakee tiedot tietystä mallista |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Luo hallinnoija ja käynnistä palvelu
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Selaa luetteloa
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Manager-metodit

| Metodi | Signatuuri | Kuvaus |
|--------|------------|--------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Alustaa SDK:n singleton-instanssin |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Pääsy singleton-manageriin |
| `manager.startWebService()` | `() => Promise<void>` | Käynnistää Foundry Local -web-palvelun |
| `manager.urls` | `string[]` | Palvelun perus-URL-osoitteiden taulukko |

#### JavaScript SDK - Katalogi- ja Malleihin liittyvät metodit

| Metodi | Signatuuri | Kuvaus |
|--------|------------|--------|
| `manager.catalog` | `Catalog` | Pääsy mallikatalogiin |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Hakee mallin aliaksella |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ käyttää olio-ohjaista arkkitehtuuria `Configuration`-, `Catalog`- ja `Model`-olioilla:

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

#### C# SDK - Keskeiset Luokat

| Luokka | Tarkoitus |
|--------|-----------|
| `Configuration` | Aseta sovelluksen nimi, lokitaso, välimuistin hakemisto, web-palvelimen URL:t |
| `FoundryLocalManager` | Pääsisäänkäynti - luotu `CreateAsync()`-metodilla, käytettävissä `.Instance`-ominaisuudella |
| `Catalog` | Selaa, hae ja hae malleja katalogista |
| `Model` | Edustaa tiettyä mallia - lataa, kuormaa, saa asiakkaat |

#### C# SDK - Manager- ja Katalogimetodit

| Metodi | Kuvaus |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Alusta manager |
| `FoundryLocalManager.Instance` | Pääsy singleton-manageriin |
| `manager.GetCatalogAsync()` | Hakee mallikatalogin |
| `catalog.ListModelsAsync()` | Listaa kaikki saatavilla olevat mallit |
| `catalog.GetModelAsync(alias: "alias")` | Hakee tietyn mallin aliaksella |
| `catalog.GetCachedModelsAsync()` | Listaa ladatut mallit |
| `catalog.GetLoadedModelsAsync()` | Listaa tällä hetkellä ladatut mallit |

> **C# Arkkitehtuurimuistiinpano:** C# SDK v0.8.0+ uudelleensuunnittelu tekee sovelluksesta **itsenäisen**; se ei vaadi Foundry Local CLI:tä loppukäyttäjän koneella. SDK hoitaa mallien hallinnan ja inferenssin natiivisti.

</details>

---

### Harjoitus 3: Lataa ja Kuormaa Malli

SDK erottaa lataamisen (levylle) ja kuormauksen (muistiin). Tämä mahdollistaa mallien ennakkoon lataamisen asennuksen aikana ja kuormauksen tarpeen mukaan.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Vaihtoehto A: Manuaalinen vaihe vaiheelta
manager = FoundryLocalManager()
manager.start_service()

# Tarkista välimuisti ensin
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

# Vaihtoehto B: Yksirivinen käynnistys (suositeltu)
# Anna alias konstruktorille - se käynnistää palvelun, lataa ja lataa automaattisesti
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Mallien Hallintametodit

| Metodi | Signatuuri | Kuvaus |
|--------|------------|--------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Lataa malli paikalliseen välimuistiin |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Kuormaa malli inferenssipalvelimelle |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Purkaa malli palvelimelta |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Listaa kaikki ladatut mallit |

#### Python - Välimuistin Hallintametodit

| Metodi | Signatuuri | Kuvaus |
|--------|------------|--------|
| `get_cache_location()` | `() -> str` | Saat välimuistikansion polun |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Listaa kaikki ladatut mallit |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Askeltainen lähestymistapa
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

#### JavaScript - Mallimetodit

| Metodi | Signatuuri | Kuvaus |
|--------|------------|--------|
| `model.isCached` | `boolean` | Onko malli jo ladattu |
| `model.download()` | `() => Promise<void>` | Lataa malli paikalliseen välimuistiin |
| `model.load()` | `() => Promise<void>` | Kuormaa inferenssipalvelimelle |
| `model.unload()` | `() => Promise<void>` | Purkaa inferenssipalvelimelta |
| `model.id` | `string` | Mallin yksilöllinen tunniste |

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

#### C# - Mallimetodit

| Metodi | Kuvaus |
|--------|---------|
| `model.DownloadAsync(progress?)` | Lataa valitun variaation |
| `model.LoadAsync()` | Kuormaa malli muistiin |
| `model.UnloadAsync()` | Purkaa malli |
| `model.SelectVariant(variant)` | Valitse tietty variaatio (CPU/GPU/NPU) |
| `model.SelectedVariant` | Tällä hetkellä valittu variaatio |
| `model.Variants` | Kaikki saatavilla olevat variaatiot tälle mallille |
| `model.GetPathAsync()` | Hanki paikallinen tiedostopolku |
| `model.GetChatClientAsync()` | Hanki natiivi chat-asiakas (ei OpenAI SDK:ta vaadita) |
| `model.GetAudioClientAsync()` | Hanki ääniasiakas transkriptioon |

</details>

---

### Harjoitus 4: Tutki Mallin Metatietoa

`FoundryModelInfo`-objekti sisältää laajat metatiedot jokaisesta mallista. Näiden kenttien ymmärtäminen auttaa valitsemaan oikean mallin sovelluksellesi.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Hanki yksityiskohtaiset tiedot tietystä mallista
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

#### FoundryModelInfo -kentät

| Kenttä | Tyyppi | Kuvaus |
|--------|--------|--------|
| `alias` | string | Lyhyt nimi (esim. `phi-3.5-mini`) |
| `id` | string | Mallin yksilöllinen tunniste |
| `version` | string | Mallin versio |
| `task` | string | `chat-completions` tai `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU tai NPU |
| `execution_provider` | string | Ajonaikaisen taustan tyyppi (CUDA, CPU, QNN, WebGPU jne.) |
| `file_size_mb` | int | Koko levyllä megatavuina |
| `supports_tool_calling` | bool | Tukeeko malli toimintojen/työkalujen kutsumista |
| `publisher` | string | Kuka julkaisi mallin |
| `license` | string | Lisenssin nimi |
| `uri` | string | Mallin URI |
| `prompt_template` | dict/null | Kehotepohja, jos sellainen on |

---

### Harjoitus 5: Hallitse Mallin Elinkaarta

Harjoittele koko elinkaari: listaa → lataa → kuormaa → käytä → pura.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Pieni malli nopeaan testaukseen

manager = FoundryLocalManager()
manager.start_service()

# 1. Tarkista, mitä on luettelossa
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Tarkista, mitä on jo ladattu
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Lataa malli
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Varmista, että se on nyt välimuistissa
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Lataa se
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Tarkista, mitä on ladattu
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Poista lataus
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

const alias = "qwen2.5-0.5b"; // Pieni malli nopeaa testausta varten

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Hanki malli katalogista
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Lataa tarvittaessa
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Lataa se
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Purkaa se
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Harjoitus 6: Pika-aloitusmallit

Jokainen kieli tarjoaa pikanäppäimen palvelun käynnistämiseen ja mallin lataamiseen yhdellä kutsulla. Nämä ovat **suositellut mallit** useimmille sovelluksille.

<details>
<summary><h3>🐍 Python - Konstruktorin Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Anna alias konstruktorille - se hoitaa kaiken:
# 1. Käynnistää palvelun, jos se ei ole käynnissä
# 2. Lataa mallin, jos sitä ei ole välimuistissa
# 3. Lataa mallin päättelypalvelimelle
manager = FoundryLocalManager("phi-3.5-mini")

# Välittömästi käyttövalmis
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap`-parametri (oletus `True`) ohjaa tätä käyttäytymistä. Aseta `bootstrap=False`, jos haluat hallita manuaalisesti:

```python
# Manuaalitila - mitään ei tapahdu automaattisesti
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Luettelo</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() hoitaa kaiken:
// 1. Käynnistää palvelun
// 2. Hakee mallin luettelosta
// 3. Lataa tarvittaessa ja lataa mallin
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Välittömästi käyttövalmis
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Luettelo</h3></summary>

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

> **C# Huomautus:** C# SDK käyttää `Configuration`-luokkaa sovelluksen nimen, lokituksen, välimuistihakemiston ja jopa tietyn verkkopalvelinportin lukitsemiseen. Tämä tekee siitä kolmesta SDK:sta monipuolisimman.

</details>

---

### Harjoitus 7: Natiivinen ChatClient (Ei OpenAI SDK:ta Tarvittu)

JavaScript- ja C#-SDK:t tarjoavat `createChatClient()` kätevyyden, joka palauttaa natiivin chat-asiakkaan — ei tarvitse asentaa tai määrittää OpenAI SDK:ta erikseen.

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

// Luo ChatClient suoraan mallista — OpenAI-importtia ei tarvita
const chatClient = model.createChatClient();

// completeChat palauttaa OpenAI-yhteensopivan vastausolion
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Striimaus käyttää callback-kuviota
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` tukee myös työkalujen kutsumista — lähetä työkalut toisena argumenttina:

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

> **Milloin käyttää mitäkin mallia:**
> - **`createChatClient()`** — Nopea prototypointi, vähemmän riippuvuuksia, yksinkertaisempi koodi
> - **OpenAI SDK** — Täysi hallinta parametreihin (lämpötila, top_p, stop-tokenit jne.), parempi tuotantosovelluksille

---

### Harjoitus 8: Mallin Variantit ja Laitteistovalinta

Mallilla voi olla useita **variantteja**, jotka on optimoitu erilaisille laitteistoille. SDK valitsee automaattisesti parhaan variantin, mutta voit myös tarkistaa ja valita manuaalisesti.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Listaa kaikki saatavilla olevat variantit
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK valitsee automaattisesti parhaan variantin laitteistollesi
// Ylikirjoittamiseksi käytä selectVariant()-funktiota:
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

Pythonissa SDK valitsee automaattisesti parhaan variantin laitteiston perusteella. Käytä `get_model_info()` nähdäksesi mikä valittiin:

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

#### Mallit, joissa on NPU-variantit

Joissakin malleissa on NPU-optimoidut variantit laitteille, joissa on Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Malli | NPU-variantti saatavilla |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Vinkki:** NPU-tuen omaavilla laitteilla SDK valitsee automaattisesti NPU-variantin, kun se on saatavilla. Sinun ei tarvitse muuttaa koodiasi. Windowsille C#-projekteissa lisää `Microsoft.AI.Foundry.Local.WinML` NuGet-paketti mahdollistamaan QNN-ajon tarjoajan — QNN toimitetaan WinML-pluginina.

---

### Harjoitus 9: Mallipäivitykset ja Luettelon Päivitys

Malliluettelo päivitetään säännöllisesti. Käytä näitä metodeja tarkistaaksesi ja soveltaaksesi päivityksiä.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Päivitä katalogi saadaksesi viimeisimmän mallilistan
manager.refresh_catalog()

# Tarkista, onko välimuistissa olevasta mallista saatavilla uudempi versio
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

// Päivitä hakemisto saadaksesi uusimman mallilistan
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Listaa kaikki saatavilla olevat mallit päivityksen jälkeen
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Harjoitus 10: Työskentely Päättelymalleilla

**phi-4-mini-reasoning**-malli sisältää ketjutetun päättelyn. Se käärii sisäisen ajattelunsa `<think>...</think>`-tageihin ennen lopullisen vastauksen tuottamista. Tämä on hyödyllistä tehtäviin, jotka vaativat monivaiheista logiikkaa, matematiikkaa tai ongelmanratkaisua.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-päättely on noin 4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Malli käärii ajattelunsa <think>...</think> -tageihin
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

// Jäsennä ketjumaista ajattelua
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Milloin käyttää päättelymalleja:**
> - Matematiikan ja logiikan ongelmat
> - Monivaiheiset suunnittelutehtävät
> - Monimutkainen koodin generointi
> - Tehtävät, joissa laskentatavan näyttäminen parantaa tarkkuutta
>
> **Kompromissi:** Päättelymallit tuottavat enemmän tokeneita ( `<think>`-osio) ja ovat hitaampia. Yksinkertaisiin kysymys-vastaus-tehtäviin tavallinen malli kuten phi-3.5-mini on nopeampi.

---

### Harjoitus 11: Alias-nimien ja Laitteiston Valinnan Ymmärtäminen

Kun annat **alias-nimen** (kuten `phi-3.5-mini`) täyden mallin tunnisteen sijaan, SDK valitsee automaattisesti parhaan variantin laitteistollesi:

| Laitteisto | Valittu Ajon Tarjoaja |
|------------|----------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML-pluginin kautta) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Mikä tahansa laite (varajärjestelmä) | `CPUExecutionProvider` tai `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Alias ratkaisee parhaan variantin laitteistollesi
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Vinkki:** Käytä aina alias-nimiä sovelluskoodissasi. Kun otat sovelluksen käyttöön käyttäjän koneella, SDK valitsee parhaimman variantin ajon aikana - CUDA NVIDIA:lle, QNN Qualcommille, CPU muille.

---

### Harjoitus 12: C# Konfigurointivaihtoehdot

C# SDK:n `Configuration`-luokka tarjoaa tarkkaa hallintaa ajon aikana:

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

| Ominaisuus | Oletus | Kuvaus |
|------------|--------|--------|
| `AppName` | (vaaditaan) | Sovelluksesi nimi |
| `LogLevel` | `Information` | Lokituksen tarkkuus |
| `Web.Urls` | (dynaaminen) | Kiinnitä tietty portti verkkopalvelimelle |
| `AppDataDir` | Käyttöjärjestelmän oletus | Sovellustiedostojen pääkansio |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Missä mallit säilytetään |
| `LogsDir` | `{AppDataDir}/logs` | Missä lokit tallennetaan |

---

### Harjoitus 13: Selainkäyttö (Vain JavaScript)

JavaScript SDK sisältää selainyhteensopivan version. Selaimessa sinun täytyy käynnistää palvelu manuaalisesti CLI:n kautta ja määrittää isäntä-URL:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Käynnistä palvelu ensin manuaalisesti:
//   foundry service start
// Käytä sitten CLI-tulosteesta saatavaa URL-osoitetta
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Selaa luetteloa
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Selaimen rajoitukset:** Selainversio EI tue `startWebService()`-metodia. Sinun on varmistettava, että Foundry Local -palvelu on jo käynnissä ennen SDK:n käyttämistä selaimessa.

---

## Täydellinen API-viite

### Python

| Kategoria | Metodi | Kuvaus |
|-----------|--------|--------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Luo manager; valinnainen mallin bootstrap |
| **Palvelu** | `is_service_running()` | Tarkista, onko palvelu käynnissä |
| **Palvelu** | `start_service()` | Käynnistä palvelu |
| **Palvelu** | `endpoint` | API-päättämis-URL |
| **Palvelu** | `api_key` | API-avain |
| **Luettelo** | `list_catalog_models()` | Listaa kaikki saatavilla olevat mallit |
| **Luettelo** | `refresh_catalog()` | Päivitä luettelo |
| **Luettelo** | `get_model_info(alias_or_model_id)` | Hanki mallin metatiedot |
| **Välimuisti** | `get_cache_location()` | Välimuistihakemiston polku |
| **Välimuisti** | `list_cached_models()` | Listaa ladatut mallit |
| **Malli** | `download_model(alias_or_model_id)` | Lataa malli |
| **Malli** | `load_model(alias_or_model_id, ttl=600)` | Lataa malli käyttöön |
| **Malli** | `unload_model(alias_or_model_id)` | Vapauta malli |
| **Malli** | `list_loaded_models()` | Listaa ladatut mallit |
| **Malli** | `is_model_upgradeable(alias_or_model_id)` | Tarkista, onko päivitys saatavilla |
| **Malli** | `upgrade_model(alias_or_model_id)` | Päivitä malli uusimpaan versioon |
| **Palvelu** | `httpx_client` | Esikonfiguroitu HTTPX-asiakas suoriin API-kutsuihin |

### JavaScript

| Kategoria | Metodi | Kuvaus |
|-----------|--------|--------|
| **Init** | `FoundryLocalManager.create(options)` | Alusta SDK-singe |
| **Init** | `FoundryLocalManager.instance` | Pääsy singleteen |
| **Palvelu** | `manager.startWebService()` | Käynnistä verkkopalvelu |
| **Palvelu** | `manager.urls` | Palvelun perusosoitteiden lista |
| **Luettelo** | `manager.catalog` | Pääsy malliluetteloon |
| **Luettelo** | `catalog.getModel(alias)` | Hanki malli alias-nimellä (palauttaa Promise) |
| **Malli** | `model.isCached` | Onko malli ladattu |
| **Malli** | `model.download()` | Lataa malli |
| **Malli** | `model.load()` | Lataa malli käyttöön |
| **Malli** | `model.unload()` | Vapauta malli |
| **Malli** | `model.id` | Mallin yksilöllinen tunniste |
| **Malli** | `model.alias` | Mallin alias |
| **Malli** | `model.createChatClient()` | Hanki natiivi chat-asiakas (ei OpenAI SDK:ta) |
| **Malli** | `model.createAudioClient()` | Hanki ääni-asiakas transkriptioon |
| **Malli** | `model.removeFromCache()` | Poista malli paikallisesta välimuistista |
| **Malli** | `model.selectVariant(variant)` | Valitse tietty laitteistovariantti |
| **Malli** | `model.variants` | Saatavilla olevien varianttien lista |
| **Malli** | `model.isLoaded()` | Onko malli ladattu |
| **Luettelo** | `catalog.getModels()` | Listaa kaikki mallit |
| **Luettelo** | `catalog.getCachedModels()` | Listaa ladatut mallit |
| **Luettelo** | `catalog.getLoadedModels()` | Listaa ladatut mallit |
| **Luettelo** | `catalog.updateModels()` | Päivitä luettelo palvelusta |
| **Palvelu** | `manager.stopWebService()` | Pysäytä Foundry Local -verkkopalvelu |

### C# (v0.8.0+)

| Kategoria | Metodi | Kuvaus |
|-----------|--------|--------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Alusta manager |
| **Init** | `FoundryLocalManager.Instance` | Pääsy singleteen |
| **Luettelo** | `manager.GetCatalogAsync()` | Hanki luettelo |
| **Luettelo** | `catalog.ListModelsAsync()` | Listaa kaikki mallit |
| **Luettelo** | `catalog.GetModelAsync(alias)` | Hanki tietty malli |
| **Luettelo** | `catalog.GetCachedModelsAsync()` | Listaa ladatut mallit |
| **Luettelo** | `catalog.GetLoadedModelsAsync()` | Listaa ladatut mallit |
| **Malli** | `model.DownloadAsync(progress?)` | Lataa malli |
| **Malli** | `model.LoadAsync()` | Lataa malli käyttöön |
| **Malli** | `model.UnloadAsync()` | Vapauta malli |
| **Malli** | `model.SelectVariant(variant)` | Valitse laitteistovariantti |
| **Malli** | `model.GetChatClientAsync()` | Hanki natiivi chat-asiakas |
| **Malli** | `model.GetAudioClientAsync()` | Hanki audio-transkriptioasiakas |
| **Malli** | `model.GetPathAsync()` | Hanki paikallinen tiedostopolku |
| **Luettelo** | `catalog.GetModelVariantAsync(alias, variant)` | Hanki tietty laitteistovariantti |
| **Luettelo** | `catalog.UpdateModelsAsync()` | Päivitä luettelo |
| **Palvelin** | `manager.StartWebServerAsync()` | Käynnistä REST-verkkopalvelin |
| **Palvelin** | `manager.StopWebServerAsync()` | Pysäytä REST-verkkopalvelin |
| **Konfiguraatio** | `config.ModelCacheDir` | Välimuistihakemisto |

---

## Tärkeimmät Opit

| Käsite | Mitä opit |
|---------|-----------|
| **SDK vs CLI** | SDK tarjoaa ohjelmallisen hallinnan - välttämätön sovelluksille |
| **Ohjauskerros** | SDK hallinnoi palveluita, malleja ja välimuistia |
| **Dynaamiset portit** | Käytä aina `manager.endpoint` (Python) tai `manager.urls[0]` (JS/C#) - älä kovakoodaa porttia |
| **Alias-nimet** | Käytä alias-nimiä automaattiseen laitteistolle optimoituun mallivalintaan |
| **Pika-aloitus** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# uudelleensuunnittelu** | v0.8.0+ on itsenäinen - ei CLI:tä loppukäyttäjän koneilla |
| **Mallin elinkaari** | Luettelo → Lataa → Lataa muistiin → Käytä → Vapauta muisti |
| **FoundryModelInfo** | Runsaasti metatietoja: tehtävä, laite, koko, lisenssi, työkalun kutsun tuki |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) OpenAI-vapaan käytön tukena |
| **Variantit** | Mallit sisältävät laitteistokohtaisia versioita (CPU, GPU, NPU); valinta automaattinen |
| **Päivitykset** | Python: `is_model_upgradeable()` + `upgrade_model()` mallien ajan tasalla pitämiseen |
| **Luettelon päivitys** | `refresh_catalog()` (Python) / `updateModels()` (JS) uusien mallien löytämiseen |

---

## Resurssit

| Resurssi | Linkki |
|----------|--------|
| SDK viite (kaikki kielet) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrointi inference SDK:hin | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API viite | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK näytteet | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local -sivusto | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Seuraavat askeleet

Jatka kohtaan [Osa 3: SDK:n käyttäminen OpenAI:n kanssa](part3-sdk-and-apis.md) yhdistääksesi SDK:n OpenAI-asiakas kirjastoon ja rakentaaksesi ensimmäisen keskustelun täydennissovelluksesi.