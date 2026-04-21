![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 2 dalis: Foundry Local SDK giluminis tyrimas

> **Tikslas:** Įvaldyti Foundry Local SDK, kad programiškai valdytumėte modelius, paslaugas ir talpyklą - ir suprastumėte, kodėl SDK yra rekomenduojamas požiūris, o ne CLI, kuriant programas.

## Apžvalga

1 dalyje jūs naudojote **Foundry Local CLI**, kad atsisiųstumėte ir paleistumėte modelius interaktyviai. CLI puikiai tinka tyrinėjimui, bet kuriant tikras programas reikia **programinio valdymo**. Foundry Local SDK tai suteikia – jis valdo **valdymo lygmenį** (paleidžia paslaugą, atranda modelius, atsisiunčia, įkelia), kad jūsų programos kodas galėtų koncentruotis ties **duomenų lygmeniu** (siunčia užklausas, gauna atsakymus).

Šis užsiėmimas supažindina jus su visa SDK API galimybių apimtimi Python, JavaScript ir C# kalbomis. Pabaigoje jūs suprasite kiekvieną metodą ir kada jį naudoti.

## Mokymosi tikslai

Užsiėmimo pabaigoje gebėsite:

- Paaiškinti, kodėl SDK yra pageidaujama priemonė, palyginti su CLI, kuriant programas
- Įdiegti Foundry Local SDK Python, JavaScript arba C# kalboms
- Naudoti `FoundryLocalManager` paslaugos paleidimui, modelių valdymui ir katalogo užklausoms
- Programiškai išvardinti, atsisiųsti, įkelti ir iškrauti modelius
- Peržiūrėti modelių metaduomenis naudojant `FoundryModelInfo`
- Suprasti skirtumą tarp katalogo, talpyklos ir įkeltų modelių
- Naudoti konstruktoriaus bootstrap (Python) ir `create()` + katalogo modelį (JavaScript)
- Suprasti C# SDK pertvarką ir jos objektinį API

---

## Prieš sąlygos

| Reikalavimas | Detalės |
|-------------|---------|
| **Foundry Local CLI** | Įdiegta ir įtraukta į jūsų `PATH` ([1 dalis](part1-getting-started.md)) |
| **Programavimo aplinka** | **Python 3.9+** ir/arba **Node.js 18+** ir/arba **.NET 9.0+** |

---

## Koncepcija: SDK vs CLI - Kodėl naudoti SDK?

| Aspektas | CLI (`foundry` komanda) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Panaudojimas** | Tyrinėjimui, rankiniam testavimui | Programų integracijai |
| **Paslaugos valdymas** | Rankinis: `foundry service start` | Automatinis: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Prievado atradimas** | Skaitomas iš CLI išvesties | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Modelio atsisiuntimas** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Klaidų valdymas** | Išeigos kodai, stderr | Išimtys, tipizuotos klaidos |
| **Automatizavimas** | Šell skriptai | Natūralus kalbos integravimas |
| **Diegimas** | Reikia CLI vartotojo mašinoje | C# SDK gali būti savarankiškas (nereikia CLI) |

> **Svarbi įžvalga:** SDK valdo visą gyvavimo ciklą: paslaugos paleidimą, talpyklos tikrinimą, trūkstamų modelių atsisiuntimą, įkėlimą ir galutinio taško atradimą, vos per kelias kodo eilutes. Jūsų aplikacijai nereikia analizuoti CLI išvesties ar valdyti subprocess'us.

---

## Užduotys

### Užduotis 1: Įdiegti SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Patikrinkite diegimą:

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

Patikrinkite diegimą:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Yra du NuGet paketai:

| Paketas | Platforma | Aprašymas |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Daugiaplatformis | Veikia Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Tik Windows | Prideda WinML aparatūros spartinimą; atsisiunčia ir įdiegia vykdymo teikėjus (pvz. QNN Qualcomm NPU) |

**Windows sąranka:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Redaguokite `.csproj` failą:

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

> **Pastaba:** Windows sistemoje WinML paketas yra platus rinkinys, apimantis bazinį SDK ir QNN vykdymo teikėją. Linux/macOS naudojamas bazinis SDK. Sąlyginių TFM ir paketo nuorodų dėka projektas yra pilnai tarpplatforminis.

Sukurkite `nuget.config` projekto šaknyje:

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

Atnaujinkite paketus:

```bash
dotnet restore
```

</details>

---

### Užduotis 2: Paleisti paslaugą ir išvardinti katalogą

Pagrindinis pirmas žingsnis bet kuriai programai yra paleisti Foundry Local paslaugą ir sužinoti, kokie modeliai yra prieinami.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Sukurkite tvarkyklę ir paleiskite paslaugą
manager = FoundryLocalManager()
manager.start_service()

# Išvardinkite visus kataloge esančius modelius
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Paslaugos valdymo metodai

| Metodas | Parašas | Aprašymas |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Patikrina, ar paslauga veikia |
| `start_service()` | `() -> None` | Paleidžia Foundry Local paslaugą |
| `service_uri` | `@property -> str` | Pagrindinis paslaugos URI |
| `endpoint` | `@property -> str` | API galinis taškas (paslaugos URI + `/v1`) |
| `api_key` | `@property -> str` | API raktas (iš aplinkos arba numatytojo) |

#### Python SDK - Katalogo valdymo metodai

| Metodas | Parašas | Aprašymas |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Išvardina visus katalogo modelius |
| `refresh_catalog()` | `() -> None` | Atnaujina katalogą paslaugoje |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Gauk konkretaus modelio informaciją |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Sukurkite vadybininką ir paleiskite paslaugą
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Naršykite katalogą
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Valdytojo metodai

| Metodas | Parašas | Aprašymas |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inicijuoja SDK vienintelį egzempliorių |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Prieiga prie vienintelio valdytojo |
| `manager.startWebService()` | `() => Promise<void>` | Paleidžia Foundry Local žiniatinklio paslaugą |
| `manager.urls` | `string[]` | Paslaugos bazinių URL masyvas |

#### JavaScript SDK - Katalogo ir modelių metodai

| Metodas | Parašas | Aprašymas |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Prieiga prie modelių katalogo |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Gauk modelio objektą pagal alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK 0.8.0+ versija yra objektinio programavimo architektūra su `Configuration`, `Catalog` ir `Model` objektais:

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

#### C# SDK - Pagrindinės klasės

| Klasė | Paskirtis |
|-------|---------|
| `Configuration` | Nustato programos pavadinimą, žurnalų lygį, talpyklos katalogą, žiniatinklio serverio URL |
| `FoundryLocalManager` | Pagrindinis įėjimo taškas - sukuriamas per `CreateAsync()`, pasiekiamas per `.Instance` |
| `Catalog` | Naršyti, ieškoti ir gauti modelius kataloge |
| `Model` | Atstovauja konkretų modelį - atsisiųsti, įkelti, gauti klientus |

#### C# SDK - Valdytojo ir katalogo metodai

| Metodas | Aprašymas |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inicijuoja valdytoją |
| `FoundryLocalManager.Instance` | Prieiga prie vienintelio valdytojo |
| `manager.GetCatalogAsync()` | Gauti modelių katalogą |
| `catalog.ListModelsAsync()` | Išvardinti visus prieinamus modelius |
| `catalog.GetModelAsync(alias: "alias")` | Gauti konkretų modelį pagal alias |
| `catalog.GetCachedModelsAsync()` | Išvardinti atsisiųstus modelius |
| `catalog.GetLoadedModelsAsync()` | Išvardinti šiuo metu įkeltus modelius |

> **C# architektūros pastaba:** C# SDK 0.8.0+ pertvarka leidžia programą padaryti **savistove**; ji nereikalauja Foundry Local CLI vartotojo mašinoje. SDK valdo modelių tvarkymą ir prognozavimą gimtąja kalba.

</details>

---

### Užduotis 3: Atsisiųsti ir įkelti modelį

SDK atskiria atsisiuntimą (į diską) nuo įkėlimo (į atmintį). Tai leidžia iš anksto atsisiųsti modelius diegimo metu ir įkelti juos pagal poreikį.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Parinktis A: Rankinis žingsnis po žingsnio
manager = FoundryLocalManager()
manager.start_service()

# Pirmiausia patikrinkite talpyklą
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

# Parinktis B: Vienos eilutės bootstrap (rekomenduojama)
# Perdavimo slapyvardis konstruktoriui - jis automatiškai paleidžia paslaugą, atsisiunčia ir įkelia
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Modelių valdymo metodai

| Metodas | Parašas | Aprašymas |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Atsisiųsti modelį į vietinę talpyklą |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Įkelti modelį į prognozavimo serverį |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Iškrauti modelį iš serverio |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Išvardinti visus šiuo metu įkeltus modelius |

#### Python - Talpyklos valdymo metodai

| Metodas | Parašas | Aprašymas |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Gauti talpyklos katalogo kelią |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Išvardinti visus atsisiųstus modelius |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Žingsnis po žingsnio metodas
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

#### JavaScript - Modelių metodai

| Metodas | Parašas | Aprašymas |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Ar modelis jau yra atsisiųstas |
| `model.download()` | `() => Promise<void>` | Atsisiųsti modelį į vietinę talpyklą |
| `model.load()` | `() => Promise<void>` | Įkelti į prognozavimo serverį |
| `model.unload()` | `() => Promise<void>` | Iškrauti iš prognozavimo serverio |
| `model.id` | `string` | Unikalus modelio identifikatorius |

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

#### C# - Modelių metodai

| Metodas | Aprašymas |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Atsisiųsti pasirinktą variantą |
| `model.LoadAsync()` | Įkelti modelį į atmintį |
| `model.UnloadAsync()` | Iškrauti modelį |
| `model.SelectVariant(variant)` | Pasirinkti konkretų variantą (CPU/GPU/NPU) |
| `model.SelectedVariant` | Dabartinis pasirinktas variantas |
| `model.Variants` | Visi šio modelio variantai |
| `model.GetPathAsync()` | Gauti vietinį failo kelią |
| `model.GetChatClientAsync()` | Gauti gimtąjį pokalbio klientą (nereikia OpenAI SDK) |
| `model.GetAudioClientAsync()` | Gauti garso klientą transkripcijai |

</details>

---

### Užduotis 4: Peržiūrėti modelio metaduomenis

`FoundryModelInfo` objektas talpina daug ausų metaduomenų apie kiekvieną modelį. Šių laukų supratimas padės pasirinkti tinkamą modelį jūsų programai.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Gauti išsamią informaciją apie konkretų modelį
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

#### FoundryModelInfo laukai

| Laukas | Tipas | Aprašymas |
|-------|------|-------------|
| `alias` | string | Trumpas pavadinimas (pvz., `phi-3.5-mini`) |
| `id` | string | Unikalus modelio identifikatorius |
| `version` | string | Modelio versija |
| `task` | string | `chat-completions` arba `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU arba NPU |
| `execution_provider` | string | Vykdymo pagrindas (CUDA, CPU, QNN, WebGPU ir kt.) |
| `file_size_mb` | int | Dydis diske MB |
| `supports_tool_calling` | bool | Ar modelis palaiko funkcijų/įrankių kvietimą |
| `publisher` | string | Kas paskelbė modelį |
| `license` | string | Licencijos pavadinimas |
| `uri` | string | Modelio URI |
| `prompt_template` | dict/null | Užklausos šablonas, jei yra |

---

### Užduotis 5: Valdyti modelio gyvavimo ciklą

Praktikuokite pilną gyvavimo ciklą: išvardinti → atsisiųsti → įkelti → naudoti → iškrauti.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Mažas modelis greitam patikrinimui

manager = FoundryLocalManager()
manager.start_service()

# 1. Patikrinkite, kas yra kataloge
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Patikrinkite, kas jau atsisiųsta
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Atsisiųskite modelį
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Patikrinkite, ar jis dabar yra talpykloje
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Įkelkite jį
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Patikrinkite, kas įkelta
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Iškraukite jį
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

const alias = "qwen2.5-0.5b"; // Nedidelis modelis greitam testavimui

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Gaukite modelį iš katalogo
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Atsisiųskite, jei reikia
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Įkelkite jį
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Iškraukite jį
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### 6 užduotis: Greito paleidimo šablonai

Kiekviena kalba suteikia trumpą būdą pradėti paslaugą ir vienu kvietimu įkelti modelį. Tai yra **rekomenduojami šablonai** daugumai programų.

<details>
<summary><h3>🐍 Python – Konstruktoriaus paleidimas</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Paduokite pseudonimą konstruktoriui – jis tvarko viską:
# 1. Paleidžia paslaugą, jei ji neveikia
# 2. Atsisiunčia modelį, jei jis nėra talpykloje
# 3. Įkelia modelį į spėjimo serverį
manager = FoundryLocalManager("phi-3.5-mini")

# Iš karto paruošta naudoti
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Parametras `bootstrap` (numatytoji reikšmė `True`) kontroliuoja šį elgesį. Nustatykite `bootstrap=False`, jei norite valdyti rankiniu būdu:

```python
# Rankinis režimas - niekas nevyksta automatiškai
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript – `create()` + Katalogas</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() atlieka viską:
// 1. Paleidžia paslaugą
// 2. Gautas modelis iš katalogo
// 3. Parsisiunčia, jei reikia, ir įkelia modelį
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Iš karto paruošta naudoti
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# – `CreateAsync()` + Katalogas</h3></summary>

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

> **C# pastaba:** C# SDK naudoja `Configuration`, kad valdytų programėlės pavadinimą, žurnalų rašymą, talpyklos katalogus ir net fiksuotų konkretų tinklo serverio prievadą. Tai daro jį labiausiai konfigūruojamu iš trijų SDK.

</details>

---

### 7 užduotis: Native ChatClient (OpenAI SDK nereikia)

JavaScript ir C# SDK suteikia `createChatClient()` patogumo metodą, kuris grąžina vietinį pokalbių klientą – nereikia atskirai įdiegti ar konfigūruoti OpenAI SDK.

<details>
<summary><h3>📘 JavaScript – <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Sukurkite ChatClient tiesiogiai iš modelio — importuoti OpenAI nereikia
const chatClient = model.createChatClient();

// completeChat grąžina OpenAI suderinamą atsakymo objektą
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Srautinė transliacija naudoja atgalinio kvietimo šabloną
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` taip pat palaiko įrankių kvietimą – perduokite įrankius kaip antrą argumentą:

```javascript
const response = await chatClient.completeChat(messages, tools);
```

</details>

<details>
<summary><h3>💜 C# – <code>model.GetChatClientAsync()</code></h3></summary>

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

> **Kada naudoti kurį šabloną:**
> - **`createChatClient()`** – Greitam prototipų kūrimui, mažiau priklausomybių, paprastesnis kodas
> - **OpenAI SDK** – Pilnas parametrų valdymas (temperatūra, top_p, stop token'ai ir kt.), geriau produkcijos programoms

---

### 8 užduotis: Modelių variantai ir aparatinės įrangos parinkimas

Modeliai gali turėti kelis **variantus**, optimizuotus skirtingai aparatinei įrangai. SDK automatiškai parenka geriausią variantą, bet jūs galite taip pat patikrinti ir pasirinkti rankiniu būdu.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Išvardinkite visas galimas variacijas
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK automatiškai parenka geriausią variaciją jūsų aparatūrai
// Norėdami perrašyti, naudokite selectVariant():
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

Python SDK automatiškai parenka geriausią variantą pagal aparatūrą. Naudokite `get_model_info()`, kad sužinotumėte, kuris buvo parinktasis:

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

#### Modeliai su NPU variantais

Kai kurie modeliai turi NPU optimizuotus variantus įrenginiams su Neuronalinių Apdorojimo Vienetų (Qualcomm Snapdragon, Intel Core Ultra):

| Modelis | Yra NPU variantas |
|---------|:-----------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Patarimas:** Aparatūroje, kur yra NPU, SDK automatiškai parenka NPU variantą, jei jis yra. Jums nereikia keisti kodo. C# projektams Windows aplinkoje pridėkite NuGet paketą `Microsoft.AI.Foundry.Local.WinML`, kad įgalintumėte QNN vykdymo tiekėją – QNN tiekiamas kaip WinML papildinys.

---

### 9 užduotis: Modelių atnaujinimai ir katalogo atnaujinimas

Modelių katalogas atnaujinamas periodiškai. Naudokite šiuos metodus atnaujinimams patikrinti ir pritaikyti.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Atnaujinkite katalogą, kad gautumėte naujausią modelių sąrašą
manager.refresh_catalog()

# Patikrinkite, ar talpykloje esantis modelis turi naujesnę versiją
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

// Atnaujinkite katalogą, kad gautumėte naujausią modelių sąrašą
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Išvardinkite visus galimus modelius po atnaujinimo
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### 10 užduotis: Darbas su mąstymo modeliais

**phi-4-mini-reasoning** modelis apima grandininio mąstymo logiką. Jis viduje aptraukia mąstymą `<think>...</think>` žymomis prieš pateikdamas galutinį atsakymą. Tai naudinga užduotims, kurios reikalauja kelių žingsnių logikos, matematikos ar problemų sprendimo.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning yra apie 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Modelis apgaubia savo mintis <think>...</think> žymomis
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

// Analizuoti grandininį mąstymą
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Kada naudoti mąstymo modelius:**
> - Matematikos ir logikos užduotims
> - Kelių žingsnių planavimo užduotims
> - Sudėtingam kodo generavimui
> - Užduotims, kur rodant sprendimo eigą gerėja tikslumas
>
> **Komproomisai:** Mąstymo modeliai sukuria daugiau tokenų ( `<think>` sekciją) ir veikia lėčiau. Paprastiems klausimams ir atsakymams standartinis modelis, pvz., phi-3.5-mini, yra greitesnis.

---

### 11 užduotis: Aliaso ir aparatinės įrangos parinkimo supratimas

Kai perduodate **aliase'ą** (pvz., `phi-3.5-mini`) vietoj pilno modelio ID, SDK automatiškai parenka geriausią variantą pagal jūsų aparatūrą:

| Aparatinė įranga | Pasirinktas vykdymo tiekėjas |
|------------------|-----------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (per WinML papildinį) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Bet koks įrenginys (alternatyva) | `CPUExecutionProvider` arba `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Slapyvardis atitinka geriausią variantą JŪSŲ aparatinei įrangai
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Patarimas:** Visada naudokite alias’us programos kode. Diegiant vartotojo įrenginyje, SDK paleidimo metu pasirenka optimalų variantą — CUDA NVIDIA įrenginiuose, QNN Qualcomm, CPU kitur.

---

### 12 užduotis: C# konfigūracijos parinktys

C# SDK klasė `Configuration` suteikia smulkų valdymą vykdymo metu:

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

| Savybė | Numatytoji reikšmė | Aprašymas |
|----------|---------|-------------|
| `AppName` | (privaloma) | Jūsų programos pavadinimas |
| `LogLevel` | `Information` | Žurnalų detalių lygis |
| `Web.Urls` | (dinamiška) | Fiksuokite konkretų serverio prievadą |
| `AppDataDir` | OS numatytoji | Pagrindinis programos duomenų katalogas |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Kur saugomi modeliai |
| `LogsDir` | `{AppDataDir}/logs` | Kur rašomi žurnalai |

---

### 13 užduotis: Naršyklės naudojimas (tik JavaScript)

JavaScript SDK yra suderinama su naršykle. Naršyklėje paslaugą reikia paleisti per CLI ir nurodyti šeimininko URL:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Pirmiausia paleiskite paslaugą rankiniu būdu:
//   foundry service start
// Tada naudokite URL iš CLI išvesties
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Naršykite katalogą
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Naršyklės apribojimai:** Naršyklės versija **nepalaiko** `startWebService()`. Prieš naudojant SDK naršyklėje, Foundry Local paslauga turi jau veikti.

---

## Pilnas API referatas

### Python

| Kategorija | Metodas | Aprašymas |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Sukuria valdytoją; neprivalomai paleidžia su modeliu |
| **Paslauga** | `is_service_running()` | Patikrina, ar paslauga veikia |
| **Paslauga** | `start_service()` | Paleidžia paslaugą |
| **Paslauga** | `endpoint` | API galinio taško URL |
| **Paslauga** | `api_key` | API raktas |
| **Katalogas** | `list_catalog_models()` | Išvardina visus galimus modelius |
| **Katalogas** | `refresh_catalog()` | Atnaujina katalogą |
| **Katalogas** | `get_model_info(alias_or_model_id)` | Gauti modelio metaduomenis |
| **Talpykla** | `get_cache_location()` | Talpyklos katalogo kelias |
| **Talpykla** | `list_cached_models()` | Išvardinti atsisiųstus modelius |
| **Modelis** | `download_model(alias_or_model_id)` | Atsisiųsti modelį |
| **Modelis** | `load_model(alias_or_model_id, ttl=600)` | Įkelti modelį |
| **Modelis** | `unload_model(alias_or_model_id)` | Iškrauti modelį |
| **Modelis** | `list_loaded_models()` | Išvardinti įkeltus modelius |
| **Modelis** | `is_model_upgradeable(alias_or_model_id)` | Patikrinti, ar yra naujesnė versija |
| **Modelis** | `upgrade_model(alias_or_model_id)` | Atidugeti modelį naujausia versija |
| **Paslauga** | `httpx_client` | Iš anksto konfigūruotas HTTPX klientas tiesioginiams API kvietimams |

### JavaScript

| Kategorija | Metodas | Aprašymas |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Inicijuoja SDK vienetą |
| **Init** | `FoundryLocalManager.instance` | Prieiga prie vieneto valdytojo |
| **Paslauga** | `manager.startWebService()` | Paleidžia tinklo paslaugą |
| **Paslauga** | `manager.urls` | Paslaugos pagrindinių URL masyvas |
| **Katalogas** | `manager.catalog` | Prieiga prie modelių katalogo |
| **Katalogas** | `catalog.getModel(alias)` | Gauti modelio objektą pagal alias (grąžina pažadą) |
| **Modelis** | `model.isCached` | Ar modelis jau atsisiųstas |
| **Modelis** | `model.download()` | Atsisiųsti modelį |
| **Modelis** | `model.load()` | Įkelti modelį |
| **Modelis** | `model.unload()` | Iškrauti modelį |
| **Modelis** | `model.id` | Modelio unikalus identifikatorius |
| **Modelis** | `model.alias` | Modelio alias |
| **Modelis** | `model.createChatClient()` | Gauti vietinį pokalbių klientą (nereikia OpenAI SDK) |
| **Modelis** | `model.createAudioClient()` | Gauti garso klientą transkripcijai |
| **Modelis** | `model.removeFromCache()` | Pašalinti modelį iš vietinės talpyklos |
| **Modelis** | `model.selectVariant(variant)` | Pasirinkti konkretų aparatūros variantą |
| **Modelis** | `model.variants` | Galimų modelio variantų masyvas |
| **Modelis** | `model.isLoaded()` | Patikrinti, ar modelis yra šiuo metu įkeltas |
| **Katalogas** | `catalog.getModels()` | Išvardinti visus galimus modelius |
| **Katalogas** | `catalog.getCachedModels()` | Išvardinti atsisiųstus modelius |
| **Katalogas** | `catalog.getLoadedModels()` | Išvardinti šiuo metu įkeltus modelius |
| **Katalogas** | `catalog.updateModels()` | Atnaujinti katalogą iš paslaugos |
| **Paslauga** | `manager.stopWebService()` | Sustabdyti Foundry Local tinklo paslaugą |

### C# (v0.8.0+)

| Kategorija | Metodas | Aprašymas |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Inicijuoti valdytoją |
| **Init** | `FoundryLocalManager.Instance` | Prieiga prie vieneto |
| **Katalogas** | `manager.GetCatalogAsync()` | Gauti katalogą |
| **Katalogas** | `catalog.ListModelsAsync()` | Išvardinti visus modelius |
| **Katalogas** | `catalog.GetModelAsync(alias)` | Gauti konkretų modelį |
| **Katalogas** | `catalog.GetCachedModelsAsync()` | Išvardinti atsisiųstus modelius |
| **Katalogas** | `catalog.GetLoadedModelsAsync()` | Išvardinti įkeltus modelius |
| **Modelis** | `model.DownloadAsync(progress?)` | Atsisiųsti modelį |
| **Modelis** | `model.LoadAsync()` | Įkelti modelį |
| **Modelis** | `model.UnloadAsync()` | Iškrauti modelį |
| **Modelis** | `model.SelectVariant(variant)` | Pasirinkti aparatūros variantą |
| **Modelis** | `model.GetChatClientAsync()` | Gauti vietinį pokalbių klientą |
| **Modelis** | `model.GetAudioClientAsync()` | Gauti garso transkripcijos klientą |
| **Modelis** | `model.GetPathAsync()` | Gauti vietinį failo kelią |
| **Katalogas** | `catalog.GetModelVariantAsync(alias, variant)` | Gauti konkretų aparatūros variantą |
| **Katalogas** | `catalog.UpdateModelsAsync()` | Atnaujinti katalogą |
| **Serveris** | `manager.StartWebServerAsync()` | Paleisti REST tinklo serverį |
| **Serveris** | `manager.StopWebServerAsync()` | Sustabdyti REST tinklo serverį |
| **Konfigūracija** | `config.ModelCacheDir` | Talpyklos katalogas |

---

## Pagrindiniai pastebėjimai

| Sąvoka | Ko išmokote |
|---------|--------------|
| **SDK vs CLI** | SDK suteikia programinį valdymą – esminis programoms |
| **Valdymo sritis** | SDK valdo paslaugas, modelius ir talpyklą |
| **Dinaminiai prievadai** | Visada naudokite `manager.endpoint` (Python) arba `manager.urls[0]` (JS/C#) – niekada nekoduokite prievado |
| **Alias’ai** | Naudokite alias’us automatiškai aparatūrai optimaliam modelio pasirinkimui |
| **Greitas pradėjimas** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# pertvarkymas** | v0.8.0+ yra savarankiškas - galutinių vartotojų kompiuteriuose nereikia CLI |
| **Modelio gyvavimo ciklas** | Katalogas → Atsisiuntimas → Įkėlimas → Naudojimas → Iškėlimas |
| **FoundryModelInfo** | Išsamūs metaduomenys: užduotis, įrenginys, dydis, licencija, įrankio kvietimo palaikymas |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) skirtas OpenAI nemokamam naudojimui |
| **Variantai** | Modeliai turi aparatūros specifinių variantų (CPU, GPU, NPU); parenkama automatiškai |
| **Atnaujinimai** | Python: `is_model_upgradeable()` + `upgrade_model()` modelių aktualizavimui |
| **Katalogo atnaujinimas** | `refresh_catalog()` (Python) / `updateModels()` (JS) naujiems modeliams atrasti |

---

## Ištekliai

| Išteklius | Nuoroda |
|----------|------|
| SDK nuoroda (visos kalbos) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Inference SDK integravimas | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API nuoroda | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK pavyzdžiai | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local svetainė | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Tolimesni veiksmai

Tęskite į [3 dalį: SDK naudojimas su OpenAI](part3-sdk-and-apis.md), kad prijungtumėte SDK prie OpenAI kliento bibliotekos ir sukurtumėte savo pirmąją pokalbių užbaigimo programą.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės apribojimas**:
Šis dokumentas buvo išverstas naudojant AI vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors siekiame tikslumo, prašome atkreipti dėmesį, kad automatizuoti vertimai gali turėti klaidų ar netikslumų. Originalus dokumentas gimtąja kalba turi būti laikomas pagrindiniu šaltiniu. Kritinei informacijai rekomenduojamas profesionalus žmogiškas vertimas. Mes neatsakome už bet kokius nesusipratimus ar neteisingus aiškinimus, kilusius dėl šio vertimo naudojimo.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->