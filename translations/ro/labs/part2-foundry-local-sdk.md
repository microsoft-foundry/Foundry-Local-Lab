![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partea 2: Explorare Detaliată a Foundry Local SDK

> **Obiectiv:** Stăpânește Foundry Local SDK pentru a gestiona modele, servicii și caching programatic - și înțelege de ce SDK este metoda recomandată față de CLI pentru construirea aplicațiilor.

## Prezentare Generală

În Partea 1 ai folosit **Foundry Local CLI** pentru a descărca și rula modele interactiv. CLI este excelent pentru explorare, însă când construiești aplicații reale ai nevoie de **control programatic**. Foundry Local SDK îți oferă asta - gestionează **planul de control** (pornirea serviciului, descoperirea modelelor, descărcarea, încărcarea) astfel încât codul aplicației tale să se concentreze pe **planul de date** (trimiterea prompturilor, primirea completărilor).

Acest laborator te învață întreaga API SDK pentru Python, JavaScript și C#. La final vei înțelege fiecare metodă disponibilă și când să folosești fiecare.

## Obiective de Învățare

Până la finalul acestui laborator vei putea:

- Explica de ce SDK este preferat față de CLI pentru dezvoltarea aplicațiilor
- Instala Foundry Local SDK pentru Python, JavaScript sau C#
- Folosi `FoundryLocalManager` pentru a porni serviciul, gestiona modelele și interoga catalogul
- Lista, descărca, încărca și descărca modele programatic
- Inspecta metadata modelelor folosind `FoundryModelInfo`
- Înțelege diferența între catalog, cache și modele încărcate
- Folosi constructorul bootstrap (Python) și patarea `create()` + catalog (JavaScript)
- Înțelege redesign-ul SDK C# și API-ul său orientat pe obiecte

---

## Precondiții

| Cerință | Detalii |
|-------------|---------|
| **Foundry Local CLI** | Instalată și pe `PATH` ([Partea 1](part1-getting-started.md)) |
| **Runtime limbaj** | **Python 3.9+** și/sau **Node.js 18+** și/sau **.NET 9.0+** |

---

## Concept: SDK vs CLI - De ce să folosești SDK-ul?

| Aspect | CLI (comanda `foundry`) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Caz de utilizare** | Explorare, testare manuală | Integrare aplicație |
| **Management serviciu** | Manual: `foundry service start` | Automat: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Descoperire port** | Citit din output CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Descărcare model** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Tratament erori** | Coduri de ieșire, stderr | Excepții, erori tipizate |
| **Automatizare** | Scripturi shell | Integrare nativă în limbaj |
| **Dezvoltare** | Necesită CLI pe mașina utilizatorului | SDK C# poate fi auto-inclusiv (fără CLI) |

> **Observație-cheie:** SDK gestionează întregul ciclu de viață: pornirea serviciului, verificarea cache-ului, descărcarea modelelor lipsă, încărcarea lor și descoperirea endpoint-ului, în câteva linii de cod. Aplicația ta nu trebuie să parseze output CLI sau să gestioneze subprocesses.

---

## Exerciții de Laborator

### Exercițiul 1: Instalarea SDK-ului

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Verifică instalarea:

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

Verifică instalarea:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Există două pachete NuGet:

| Pachet | Platformă | Descriere |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Cross-platform | Funcționează pe Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Doar Windows | Adaugă accelerare hardware WinML; descarcă și instalează furnizori de execuție plugin (ex. QNN pentru Qualcomm NPU) |

**Configurare Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Editează fișierul `.csproj`:

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

> **Notă:** Pe Windows, pachetul WinML este un superset care include SDK-ul de bază plus furnizorul de execuție QNN. Pe Linux/macOS se folosește SDK-ul de bază. TFM condiționat și referințele păstrează proiectul complet cross-platform.

Creează un `nuget.config` în rădăcina proiectului:

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

Restaurează pachetele:

```bash
dotnet restore
```

</details>

---

### Exercițiul 2: Pornește Serviciul și Listează Catalogul

Primul lucru pe care-l face orice aplicație este să pornească serviciul Foundry Local și să descopere ce modele sunt disponibile.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Creează un manager și pornește serviciul
manager = FoundryLocalManager()
manager.start_service()

# Listează toate modelele disponibile în catalog
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Metode pentru Managementul Serviciului

| Metodă | Semnătură | Descriere |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Verifică dacă serviciul este pornit |
| `start_service()` | `() -> None` | Pornește serviciul Foundry Local |
| `service_uri` | `@property -> str` | URI-ul de bază al serviciului |
| `endpoint` | `@property -> str` | Endpoint-ul API (URI serviciu + `/v1`) |
| `api_key` | `@property -> str` | Cheia API (din env sau placeholder implicit) |

#### Python SDK - Metode pentru Managementul Catalogului

| Metodă | Semnătură | Descriere |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Listează toate modelele din catalog |
| `refresh_catalog()` | `() -> None` | Reîmprospătează catalogul de la serviciu |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Obține informații pentru un model specific |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Creează un manager și pornește serviciul
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Răsfoiește catalogul
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Metode Manager

| Metodă | Semnătură | Descriere |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inițializează singleton-ul SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Accesează managerul singleton |
| `manager.startWebService()` | `() => Promise<void>` | Pornește serviciul web Foundry Local |
| `manager.urls` | `string[]` | Array cu URL-urile de bază ale serviciului |

#### JavaScript SDK - Metode Catalog și Model

| Metodă | Semnătură | Descriere |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Accesează catalogul de modele |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Obține un obiect model după alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

SDK-ul C# v0.8.0+ folosește o arhitectură orientată pe obiecte cu obiecte `Configuration`, `Catalog` și `Model`:

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

#### C# SDK - Clase Cheie

| Clasă | Scop |
|-------|---------|
| `Configuration` | Setează numele aplicației, nivelul de log, directorul cache, URL-urile serverului web |
| `FoundryLocalManager` | Punctul principal de intrare - creat via `CreateAsync()`, accesat via `.Instance` |
| `Catalog` | Răsfoiește, caută și obține modelele din catalog |
| `Model` | Reprezintă un model specific - descarcă, încarcă, obține clienți |

#### C# SDK - Metode Manager și Catalog

| Metodă | Descriere |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inițializează managerul |
| `FoundryLocalManager.Instance` | Accesează managerul singleton |
| `manager.GetCatalogAsync()` | Obține catalogul de modele |
| `catalog.ListModelsAsync()` | Listează toate modelele disponibile |
| `catalog.GetModelAsync(alias: "alias")` | Obține un model specific după alias |
| `catalog.GetCachedModelsAsync()` | Listează modelele descărcate |
| `catalog.GetLoadedModelsAsync()` | Listează modelele încărcate în prezent |

> **Notă Arhitectură C#:** Redesign-ul SDK C# v0.8.0+ face aplicația **auto-inclusivă**; nu necesită Foundry Local CLI pe mașina utilizatorului. SDK-ul gestionează nativ managementul modelelor și inferența.

</details>

---

### Exercițiul 3: Descarcă și Încarcă un Model

SDK-ul separă descărcarea (pe disc) de încărcarea (în memorie). Acest lucru permite să descarci modelele în avans în timpul configurării și să le încarci la cerere.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Opțiunea A: Pas cu pas manual
manager = FoundryLocalManager()
manager.start_service()

# Verifică mai întâi cache-ul
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

# Opțiunea B: Bootstrap într-o singură linie (recomandat)
# Transmite un alias constructorului - acesta pornește serviciul, descarcă și încarcă automat
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Metode de Management al Modelelor

| Metodă | Semnătură | Descriere |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Descarcă un model în cache local |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Încarcă un model în serverul de inferență |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Descărcă un model de pe server |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Listează toate modelele încarcate în prezent |

#### Python - Metode de Management Cache

| Metodă | Semnătură | Descriere |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Obține calea directorului cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Listează toate modelele descărcate |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Abordare pas cu pas
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

#### JavaScript - Metode Model

| Metodă | Semnătură | Descriere |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Dacă modelul este deja descărcat |
| `model.download()` | `() => Promise<void>` | Descarcă modelul în cache local |
| `model.load()` | `() => Promise<void>` | Încarcă în serverul de inferență |
| `model.unload()` | `() => Promise<void>` | Descărcă de pe serverul de inferență |
| `model.id` | `string` | Identificator unic al modelului |

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

#### C# - Metode Model

| Metodă | Descriere |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Descarcă varianta selectată |
| `model.LoadAsync()` | Încarcă modelul în memorie |
| `model.UnloadAsync()` | Descărcă modelul |
| `model.SelectVariant(variant)` | Selectează o variantă specifică (CPU/GPU/NPU) |
| `model.SelectedVariant` | Varianta selectată în prezent |
| `model.Variants` | Toate variantele disponibile pentru acest model |
| `model.GetPathAsync()` | Obține calea fișierului local |
| `model.GetChatClientAsync()` | Obține un client chat nativ (fără SDK OpenAI) |
| `model.GetAudioClientAsync()` | Obține un client audio pentru transcriere |

</details>

---

### Exercițiul 4: Inspectează Metadata Model

Obiectul `FoundryModelInfo` conține metadata detaliată despre fiecare model. Înțelegerea acestor câmpuri te ajută să alegi modelul potrivit pentru aplicația ta.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Obține informații detaliate despre un model specific
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

#### Câmpuri FoundryModelInfo

| Câmp | Tip | Descriere |
|-------|------|-------------|
| `alias` | string | Nume scurt (ex. `phi-3.5-mini`) |
| `id` | string | Identificator unic al modelului |
| `version` | string | Versiunea modelului |
| `task` | string | `chat-completions` sau `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU sau NPU |
| `execution_provider` | string | Backend runtime (CUDA, CPU, QNN, WebGPU etc.) |
| `file_size_mb` | int | Dimensiunea pe disc în MB |
| `supports_tool_calling` | bool | Dacă modelul suportă apeluri de funcții/unelte |
| `publisher` | string | Cine a publicat modelul |
| `license` | string | Numele licenței |
| `uri` | string | URI model |
| `prompt_template` | dict/null | Șablon prompt, dacă există |

---

### Exercițiul 5: Gestionează Ciclu de Viață al Modelului

Exersează ciclul de viață complet: listare → descărcare → încărcare → utilizare → descărcare.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Model mic pentru testare rapidă

manager = FoundryLocalManager()
manager.start_service()

# 1. Verifică ce este în catalog
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Verifică ce este deja descărcat
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Descarcă un model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Verifică dacă este acum în cache
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Încarcă-l
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Verifică ce este încărcat
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Descărcă-l
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

const alias = "qwen2.5-0.5b"; // Model mic pentru testare rapidă

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Obține modelul din catalog
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Descarcă dacă este nevoie
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Încarcă-l
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Descărcă-l
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Exercițiul 6: Modelele Quick-Start

Fiecare limbaj oferă o comandă rapidă pentru a porni serviciul și a încărca un model într-un singur apel. Acestea sunt **modelele recomandate** pentru majoritatea aplicațiilor.

<details>
<summary><h3>🐍 Python - Constructor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Transmite un alias constructorului - se ocupă de tot:
# 1. Pornește serviciul dacă nu este în funcțiune
# 2. Descarcă modelul dacă nu este memorat în cache
# 3. Încarcă modelul în serverul de inferență
manager = FoundryLocalManager("phi-3.5-mini")

# Gata de utilizare imediat
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Parametrul `bootstrap` (implicit `True`) controlează acest comportament. Setează `bootstrap=False` dacă dorești control manual:

```python
# Mod manual - nimic nu se întâmplă automat
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() gestionează totul:
// 1. Pornește serviciul
// 2. Obține modelul din catalog
// 3. Descarcă dacă este necesar și încarcă modelul
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Gata de utilizare imediat
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Catalog</h3></summary>

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

> **Notă C#:** SDK-ul C# folosește `Configuration` pentru a controla numele aplicației, jurnalizarea, directoarele cache și chiar pentru a fixa un port specific al serverului web. Acest lucru îl face cel mai configurabil dintre cele trei SDK-uri.

</details>

---

### Exercițiul 7: ChatClient-ul nativ (Nu este necesar SDK OpenAI)

SDK-urile JavaScript și C# oferă o metodă convenabilă `createChatClient()` care returnează un client chat nativ — nu este nevoie să instalezi sau să configurezi separat SDK-ul OpenAI.

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

// Creează un ChatClient direct din model — nu este nevoie de import OpenAI
const chatClient = model.createChatClient();

// completeChat returnează un obiect de răspuns compatibil cu OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming-ul folosește un model de apel invers (callback)
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` suportă și apelarea instrumentelor — transmite instrumentele ca al doilea argument:

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

> **Când să folosești care model:**
> - **`createChatClient()`** — Prototipare rapidă, mai puține dependențe, cod mai simplu
> - **SDK OpenAI** — Control complet asupra parametrilor (temperature, top_p, stop tokens etc.), mai potrivit pentru aplicații de producție

---

### Exercițiul 8: Variante de modele și selecția hardware

Modelele pot avea multiple **variante** optimizate pentru hardware diferit. SDK-ul selectează automat cea mai bună variantă, dar poți și inspecta și alege manual.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Listează toate variantele disponibile
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK-ul selectează automat cea mai bună variantă pentru hardware-ul tău
// Pentru a suprascrie, folosește selectVariant():
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

În Python, SDK-ul selectează automat cea mai bună variantă în funcție de hardware. Folosește `get_model_info()` pentru a vedea ce a fost selectat:

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

#### Modele cu variante NPU

Unele modele au variante optimizate NPU pentru dispozitive cu Unități de Procesare Neurală (Qualcomm Snapdragon, Intel Core Ultra):

| Model | Variantă NPU Disponibilă |
|-------|:------------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Sfat:** Pe hardware compatibil NPU, SDK-ul selectează automat varianta NPU dacă este disponibilă. Nu trebuie să modifici codul. Pentru proiectele C# pe Windows, adaugă pachetul NuGet `Microsoft.AI.Foundry.Local.WinML` pentru a activa furnizorul de execuție QNN — QNN este oferit ca plugin EP prin WinML.

---

### Exercițiul 9: Upgrade-uri de model și reîmprospătarea catalogului

Catalogul de modele este actualizat periodic. Folosește aceste metode pentru a verifica și aplica actualizări.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Reîmprospătează catalogul pentru a obține cea mai recentă listă de modele
manager.refresh_catalog()

# Verifică dacă un model în cache are o versiune mai nouă disponibilă
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

// Reîmprospătează catalogul pentru a prelua cea mai recentă listă de modele
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Listează toate modelele disponibile după reîmprospătare
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Exercițiul 10: Lucrul cu modele de raționament

Modelul **phi-4-mini-reasoning** include raționament prin lanțul gândirii. Își învelește gândirea internă în tag-uri `<think>...</think>` înainte de a produce răspunsul final. Acest lucru este util pentru sarcini care necesită logică în mai mulți pași, matematică sau rezolvare de probleme.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning are aproximativ 4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Modelul își înfășoară gândirea în etichete <think>...</think>
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

// Analizează gândirea în lanț
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Când să folosești modele de raționament:**
> - Probleme de matematică și logică
> - Sarcini de planificare în mai mulți pași
> - Generare complexă de cod
> - Sarcini unde afișarea modului de lucru îmbunătățește acuratețea
>
> **Compromis:** Modelele de raționament produc mai mulți tokeni (secțiunea `<think>`) și sunt mai lente. Pentru întrebări simple, un model standard precum phi-3.5-mini este mai rapid.

---

### Exercițiul 11: Înțelegerea aliasurilor și selecția hardware

Când transmiți un **alias** (ca `phi-3.5-mini`) în loc de un ID complet de model, SDK-ul selectează automat cea mai bună variantă pentru hardware-ul tău:

| Hardware | Furnizorul de execuție selectat |
|----------|-------------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (prin plugin WinML) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Orice dispozitiv (fallback) | `CPUExecutionProvider` sau `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Aliasul se rezolvă la cea mai bună variantă pentru hardware-ul DUMNEAVOASTRĂ
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Sfat:** Folosește întotdeauna aliasuri în codul aplicației. Când implementezi pe mașina unui utilizator, SDK-ul alege varianta optimă la rulare — CUDA pe NVIDIA, QNN pe Qualcomm, CPU în rest.

---

### Exercițiul 12: Opțiuni de configurare C#

Clasa `Configuration` din SDK-ul C# oferă control detaliat asupra runtime-ului:

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

| Proprietate | Implicit | Descriere |
|-------------|----------|-----------|
| `AppName` | (obligatoriu) | Numele aplicației tale |
| `LogLevel` | `Information` | Nivelul de verbositate al jurnalelor |
| `Web.Urls` | (dinamic) | Fixează un port specific pentru serverul web |
| `AppDataDir` | Implicit OS | Directorul de bază pentru datele aplicației |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Unde sunt stocate modelele |
| `LogsDir` | `{AppDataDir}/logs` | Unde sunt scrise jurnalele |

---

### Exercițiul 13: Utilizare în browser (doar JavaScript)

SDK-ul JavaScript include o versiune compatibilă browserului. În browser, trebuie să pornești manual serviciul prin CLI și să specifici URL-ul gazdei:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Porniți serviciul manual mai întâi:
//   foundry service start
// Apoi folosiți URL-ul din ieșirea CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Răsfoiți catalogul
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Limitări browser:** Versiunea pentru browser **nu** suportă `startWebService()`. Trebuie să te asiguri că serviciul Foundry Local este deja pornit înainte de a folosi SDK-ul în browser.

---

## Referință completă API

### Python

| Categorie | Metodă | Descriere |
|----------|--------|-----------|
| **Inițializare** | `FoundryLocalManager(alias?, bootstrap=True)` | Creează manager; opțional pornește cu un model |
| **Serviciu** | `is_service_running()` | Verifică dacă serviciul rulează |
| **Serviciu** | `start_service()` | Pornește serviciul |
| **Serviciu** | `endpoint` | URL-ul endpoint API |
| **Serviciu** | `api_key` | Cheia API |
| **Catalog** | `list_catalog_models()` | Listează toate modelele disponibile |
| **Catalog** | `refresh_catalog()` | Reîmprospătează catalogul |
| **Catalog** | `get_model_info(alias_or_model_id)` | Obține metadatele modelului |
| **Cache** | `get_cache_location()` | Calea directorului cache |
| **Cache** | `list_cached_models()` | Listează modelele descărcate |
| **Model** | `download_model(alias_or_model_id)` | Descarcă un model |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Încarcă un model |
| **Model** | `unload_model(alias_or_model_id)` | Descărcă un model |
| **Model** | `list_loaded_models()` | Listează modelele încărcate |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Verifică dacă există o versiune mai nouă |
| **Model** | `upgrade_model(alias_or_model_id)` | Actualizează modelul la ultima versiune |
| **Serviciu** | `httpx_client` | Client HTTPX preconfigurat pentru apeluri API directe |

### JavaScript

| Categorie | Metodă | Descriere |
|----------|--------|-----------|
| **Inițializare** | `FoundryLocalManager.create(options)` | Inițializează singletonul SDK |
| **Inițializare** | `FoundryLocalManager.instance` | Accesează managerul singleton |
| **Serviciu** | `manager.startWebService()` | Pornește serviciul web |
| **Serviciu** | `manager.urls` | Array de URL-uri de bază pentru serviciu |
| **Catalog** | `manager.catalog` | Accesează catalogul de modele |
| **Catalog** | `catalog.getModel(alias)` | Obține un obiect model după alias (returnează Promise) |
| **Model** | `model.isCached` | Indică dacă modelul este descărcat |
| **Model** | `model.download()` | Descarcă modelul |
| **Model** | `model.load()` | Încarcă modelul |
| **Model** | `model.unload()` | Descărcă modelul |
| **Model** | `model.id` | Identificatorul unic al modelului |
| **Model** | `model.alias` | Aliasul modelului |
| **Model** | `model.createChatClient()` | Obține un client chat nativ (fără SDK OpenAI) |
| **Model** | `model.createAudioClient()` | Obține un client audio pentru transcriere |
| **Model** | `model.removeFromCache()` | Elimină modelul din cache-ul local |
| **Model** | `model.selectVariant(variant)` | Selectează o variantă hardware specifică |
| **Model** | `model.variants` | Array cu variante disponibile pentru model |
| **Model** | `model.isLoaded()` | Verifică dacă modelul este încărcat |
| **Catalog** | `catalog.getModels()` | Listează toate modelele disponibile |
| **Catalog** | `catalog.getCachedModels()` | Listează modelele descărcate |
| **Catalog** | `catalog.getLoadedModels()` | Listează modelele încărcate în prezent |
| **Catalog** | `catalog.updateModels()` | Reîmprospătează catalogul de la serviciu |
| **Serviciu** | `manager.stopWebService()` | Oprește serviciul web Foundry Local |

### C# (v0.8.0+)

| Categorie | Metodă | Descriere |
|----------|--------|-----------|
| **Inițializare** | `FoundryLocalManager.CreateAsync(config, logger)` | Inițializează managerul |
| **Inițializare** | `FoundryLocalManager.Instance` | Accesează singletonul |
| **Catalog** | `manager.GetCatalogAsync()` | Obține catalogul |
| **Catalog** | `catalog.ListModelsAsync()` | Listează toate modelele |
| **Catalog** | `catalog.GetModelAsync(alias)` | Obține un model specific |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Listează modelele din cache |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Listează modelele încărcate |
| **Model** | `model.DownloadAsync(progress?)` | Descarcă un model |
| **Model** | `model.LoadAsync()` | Încarcă un model |
| **Model** | `model.UnloadAsync()` | Descărcă un model |
| **Model** | `model.SelectVariant(variant)` | Alege o variantă hardware |
| **Model** | `model.GetChatClientAsync()` | Obține client chat nativ |
| **Model** | `model.GetAudioClientAsync()` | Obține client audio pentru transcriere |
| **Model** | `model.GetPathAsync()` | Obține calea locală a fișierului |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Obține o variantă hardware specifică |
| **Catalog** | `catalog.UpdateModelsAsync()` | Reîmprospătează catalogul |
| **Server** | `manager.StartWebServerAsync()` | Pornește serverul web REST |
| **Server** | `manager.StopWebServerAsync()` | Oprește serverul web REST |
| **Config** | `config.ModelCacheDir` | Directorul cache |

---

## Puncte cheie

| Concept | Ce ai învățat |
|---------|---------------|
| **SDK vs CLI** | SDK-ul oferă control programatic - esențial pentru aplicații |
| **Plan de control** | SDK-ul gestionează servicii, modele și cache |
| **Porturi dinamice** | Folosește întotdeauna `manager.endpoint` (Python) sau `manager.urls[0]` (JS/C#) - nu fixa porturi |
| **Aliasuri** | Folosește aliasuri pentru selecția automată optimă de hardware |
| **Început rapid** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Redesenare C#** | v0.8.0+ este auto-conținut - nu este nevoie de CLI pe calculatoarele utilizatorilor finali |
| **Ciclul de viață al modelului** | Catalog → Descărcare → Încărcare → Utilizare → Descărcare |
| **FoundryModelInfo** | Metadate bogate: sarcină, dispozitiv, dimensiune, licență, suport pentru apelarea instrumentelor |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) pentru utilizare fără OpenAI |
| **Variante** | Modelele au variante specifice hardware-ului (CPU, GPU, NPU); selectate automat |
| **Actualizări** | Python: `is_model_upgradeable()` + `upgrade_model()` pentru a menține modelele actualizate |
| **Reîmprospătare catalog** | `refresh_catalog()` (Python) / `updateModels()` (JS) pentru a descoperi modele noi |

---

## Resurse

| Resursă | Link |
|----------|------|
| Referință SDK (toate limbile) | [Microsoft Learn - Referință SDK Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrare cu SDK-uri de inferență | [Microsoft Learn - Integrare SDK de inferență](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Referință API C# SDK | [Referință API C# Foundry Local](https://aka.ms/fl-csharp-api-ref) |
| Exemple C# SDK | [GitHub - Exemple SDK Foundry Local](https://aka.ms/foundrylocalSDK) |
| Site Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Pașii următori

Continuați cu [Partea 3: Folosirea SDK-ului cu OpenAI](part3-sdk-and-apis.md) pentru a conecta SDK-ul la biblioteca client OpenAI și a construi prima aplicație de completare a chat-ului.