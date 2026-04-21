![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Μέρος 2: Βαθιά Εξέταση του Foundry Local SDK

> **Στόχος:** Κατακτήστε το Foundry Local SDK για να διαχειρίζεστε μοντέλα, υπηρεσίες και προσωρινή αποθήκευση προγραμματιστικά - και κατανοήστε γιατί το SDK είναι η προτεινόμενη προσέγγιση έναντι του CLI για τη δημιουργία εφαρμογών.

## Επισκόπηση

Στο Μέρος 1 χρησιμοποιήσατε το **Foundry Local CLI** για να κατεβάσετε και να εκτελέσετε μοντέλα διαδραστικά. Το CLI είναι ιδανικό για εξερεύνηση, αλλά όταν δημιουργείτε πραγματικές εφαρμογές χρειάζεστε **προγραμματιστικό έλεγχο**. Το Foundry Local SDK σας το παρέχει αυτό - διαχειρίζεται το **control plane** (εκκίνηση υπηρεσίας, αναζήτηση μοντέλων, λήψη, φόρτωση) ώστε ο κώδικας της εφαρμογής σας να εστιάζει στο **data plane** (αποστολή προτροπών, λήψη ολοκληρώσεων).

Αυτό το εργαστήριο σας διδάσκει το πλήρες API του SDK σε Python, JavaScript και C#. Στο τέλος θα κατανοείτε κάθε διαθέσιμη μέθοδο και πότε να τη χρησιμοποιήσετε.

## Στόχοι Μάθησης

Στο τέλος αυτού του εργαστηρίου θα μπορείτε να:

- Εξηγήσετε γιατί προτιμάται το SDK έναντι του CLI για ανάπτυξη εφαρμογών
- Εγκαταστήσετε το Foundry Local SDK για Python, JavaScript ή C#
- Χρησιμοποιήσετε το `FoundryLocalManager` για εκκίνηση υπηρεσίας, διαχείριση μοντέλων και αναζήτηση καταλόγου
- Λίστα, λήψη, φόρτωση και αποφόρτωση μοντέλων προγραμματιστικά
- Εξετάσετε μεταδεδομένα μοντέλων χρησιμοποιώντας το `FoundryModelInfo`
- Κατανοήσετε τη διαφορά μεταξύ καταλόγου, προσωρινής αποθήκευσης και φορτωμένων μοντέλων
- Χρησιμοποιήσετε τον constructor bootstrap (Python) και το πρότυπο `create()` + κατάλογος (JavaScript)
- Κατανοήσετε τον επανασχεδιασμό του C# SDK και το αντικειμενοστραφές API

---

## Προαπαιτήσεις

| Απαίτηση | Λεπτομέρειες |
|-------------|---------|
| **Foundry Local CLI** | Εγκατεστημένο και στο `PATH` σας ([Μέρος 1](part1-getting-started.md)) |
| **Περιβάλλον γλώσσας** | **Python 3.9+** και/ή **Node.js 18+** και/ή **.NET 9.0+** |

---

## Έννοια: SDK έναντι CLI - Γιατί Χρησιμοποιούμε το SDK;

| Πτυχή | CLI (εντολή `foundry`) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Περίπτωση χρήσης** | Εξερεύνηση, χειροκίνητος έλεγχος | Ενσωμάτωση σε εφαρμογές |
| **Διαχείριση υπηρεσίας** | Χειροκίνητα: `foundry service start` | Αυτόματα: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Ανακάλυψη θύρας** | Διαβάζει από έξοδο CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Λήψη μοντέλου** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Διαχείριση σφαλμάτων** | Κωδικοί εξόδου, stderr | Exceptions, τυποποιημένα σφάλματα |
| **Αυτοματοποίηση** | Shell scripts | Φυσική ενσωμάτωση γλώσσας |
| **Ανάπτυξη** | Απαιτεί CLI στον υπολογιστή χρήστη | Το C# SDK είναι αυτεξούσιο (δεν απαιτεί CLI) |

> **Βασικό συμπέρασμα:** Το SDK διαχειρίζεται ολόκληρο τον κύκλο ζωής: εκκίνηση υπηρεσίας, έλεγχος cache, λήψη ελλειπόντων μοντέλων, φόρτωση και ανακάλυψη endpoint, όλα με λίγες γραμμές κώδικα. Η εφαρμογή σας δεν χρειάζεται να αναλύει έξοδο CLI ή να διαχειρίζεται υποδιαδικασίες.

---

## Ασκήσεις Εργαστηρίου

### Άσκηση 1: Εγκαταστήστε το SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Επαληθεύστε την εγκατάσταση:

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

Επαληθεύστε την εγκατάσταση:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Υπάρχουν δύο πακέτα NuGet:

| Πακέτο | Πλατφόρμα | Περιγραφή |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Πλατφόρμα ανεξάρτητη | Λειτουργεί σε Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Μόνο Windows | Προσθέτει επιτάχυνση υλικού WinML; κατεβάζει και εγκαθιστά παρόχους εκτέλεσης plugin (π.χ. QNN για Qualcomm NPU) |

**Ρύθμιση Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Επεξεργαστείτε το αρχείο `.csproj`:

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

> **Σημείωση:** Στα Windows, το πακέτο WinML είναι υπερσύνολο που περιλαμβάνει το βασικό SDK και τον πάροχο εκτέλεσης QNN. Σε Linux/macOS χρησιμοποιείται το βασικό SDK. Η συνθήκη TFM και οι αναφορές πακέτων διατηρούν το έργο πλήρως πλατφόρμα ανεξάρτητο.

Δημιουργήστε ένα `nuget.config` στη ρίζα του έργου:

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

Ανακτήστε πακέτα:

```bash
dotnet restore
```

</details>

---

### Άσκηση 2: Εκκινήστε την Υπηρεσία και Αναζητήστε τον Κατάλογο

Το πρώτο που κάνει κάθε εφαρμογή είναι να εκκινήσει την υπηρεσία Foundry Local και να ανακαλύψει ποια μοντέλα είναι διαθέσιμα.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Δημιουργήστε έναν διαχειριστή και ξεκινήστε την υπηρεσία
manager = FoundryLocalManager()
manager.start_service()

# Λίστα με όλα τα διαθέσιμα μοντέλα στο κατάλογο
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Μέθοδοι Διαχείρισης Υπηρεσίας

| Μέθοδος | Υπογραφή | Περιγραφή |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Ελέγχει αν η υπηρεσία τρέχει |
| `start_service()` | `() -> None` | Εκκινεί την υπηρεσία Foundry Local |
| `service_uri` | `@property -> str` | Η βασική διεύθυνση URI υπηρεσίας |
| `endpoint` | `@property -> str` | Το endpoint API (διεύθυνση υπηρεσίας + `/v1`) |
| `api_key` | `@property -> str` | Κλειδί API (από περιβάλλον ή προεπιλογή) |

#### Python SDK - Μέθοδοι Διαχείρισης Καταλόγου

| Μέθοδος | Υπογραφή | Περιγραφή |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Λίστα όλων των μοντέλων του καταλόγου |
| `refresh_catalog()` | `() -> None` | Ανανεώνει τον κατάλογο από την υπηρεσία |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Λαμβάνει πληροφορίες για συγκεκριμένο μοντέλο |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Δημιουργήστε έναν διαχειριστή και ξεκινήστε την υπηρεσία
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Περιηγηθείτε στον κατάλογο
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Μέθοδοι Διαχείρισης

| Μέθοδος | Υπογραφή | Περιγραφή |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Αρχικοποίηση singleton του SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Πρόσβαση στον singleton manager |
| `manager.startWebService()` | `() => Promise<void>` | Εκκίνηση της Foundry Local web υπηρεσίας |
| `manager.urls` | `string[]` | Πίνακας βασικών URLs για την υπηρεσία |

#### JavaScript SDK - Μέθοδοι Καταλόγου και Μοντέλων

| Μέθοδος | Υπογραφή | Περιγραφή |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Πρόσβαση στον κατάλογο μοντέλων |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Λαμβάνει αντικείμενο μοντέλου με βάση ψευδώνυμο |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Το C# SDK έκδοσης v0.8.0+ χρησιμοποιεί αντικειμενοστραφή αρχιτεκτονική με αντικείμενα `Configuration`, `Catalog` και `Model`:

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

#### C# SDK - Κύριες Κλάσεις

| Κλάση | Σκοπός |
|-------|---------|
| `Configuration` | Ορισμός ονόματος εφαρμογής, επιπέδου καταγραφής, καταλόγου cache, URLs web server |
| `FoundryLocalManager` | Κύριο σημείο εισόδου - δημιουργείται μέσω `CreateAsync()`, πρόσβαση μέσω `.Instance` |
| `Catalog` | Περιήγηση, αναζήτηση και απόκτηση μοντέλων από τον κατάλογο |
| `Model` | Αντιπροσωπεύει ένα συγκεκριμένο μοντέλο - λήψη, φόρτωση, πρόσβαση πελατών |

#### C# SDK - Μέθοδοι Manager και Καταλόγου

| Μέθοδος | Περιγραφή |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Αρχικοποίηση manager |
| `FoundryLocalManager.Instance` | Πρόσβαση σε singleton manager |
| `manager.GetCatalogAsync()` | Λήψη καταλόγου μοντέλων |
| `catalog.ListModelsAsync()` | Λίστα όλων των διαθέσιμων μοντέλων |
| `catalog.GetModelAsync(alias: "alias")` | Απόκτηση συγκεκριμένου μοντέλου με ψευδώνυμο |
| `catalog.GetCachedModelsAsync()` | Λίστα ληφθέντων μοντέλων |
| `catalog.GetLoadedModelsAsync()` | Λίστα φορτωμένων μοντέλων |

> **Σημείωση Αρχιτεκτονικής C#:** Το SDK v0.8.0+ το καθιστά **αυτεξούσιο**· δεν απαιτεί το Foundry Local CLI στον υπολογιστή του χρήστη. Το SDK διαχειρίζεται τη διαχείριση μοντέλων και την παραγωγή αποτελεσμάτων εγγενώς.

</details>

---

### Άσκηση 3: Κατεβάστε και Φορτώστε ένα Μοντέλο

Το SDK διαχωρίζει την λήψη (στο δίσκο) από τη φόρτωση (στη μνήμη). Αυτό σας επιτρέπει να κατεβάζετε μοντέλα εκ των προτέρων κατά την εγκατάσταση και να τα φορτώνετε κατά ζήτηση.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Επιλογή Α: Χειροκίνητο βήμα προς βήμα
manager = FoundryLocalManager()
manager.start_service()

# Ελέγξτε πρώτα την προσωρινή μνήμη
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

# Επιλογή Β: Εντολή bootstrap σε μία γραμμή (συνιστάται)
# Περνάτε ψευδώνυμο στον κατασκευαστή - ξεκινά την υπηρεσία, κατεβάζει και φορτώνει αυτόματα
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Μέθοδοι Διαχείρισης Μοντέλου

| Μέθοδος | Υπογραφή | Περιγραφή |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Κατεβάζει μοντέλο στην τοπική cache |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Φορτώνει μοντέλο στον server παραγωγής αποτελεσμάτων |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Αποφορτώνει μοντέλο από τον server |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Λίστα όλων των φορτωμένων μοντέλων |

#### Python - Μέθοδοι Διαχείρισης Cache

| Μέθοδος | Υπογραφή | Περιγραφή |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Λαμβάνει τη διαδρομή του καταλόγου cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Λίστα όλων των κατεβασμένων μοντέλων |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Προσέγγιση βήμα προς βήμα
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

#### JavaScript - Μέθοδοι Μοντέλου

| Μέθοδος | Υπογραφή | Περιγραφή |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Αν το μοντέλο είναι ήδη κατεβασμένο |
| `model.download()` | `() => Promise<void>` | Κατεβάζει το μοντέλο στην τοπική cache |
| `model.load()` | `() => Promise<void>` | Φορτώνει στον inference server |
| `model.unload()` | `() => Promise<void>` | Αποφορτώνει από τον inference server |
| `model.id` | `string` | Μοναδικό αναγνωριστικό μοντέλου |

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

#### C# - Μέθοδοι Μοντέλου

| Μέθοδος | Περιγραφή |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Κατεβάζει την επιλεγμένη παραλλαγή |
| `model.LoadAsync()` | Φορτώνει το μοντέλο στη μνήμη |
| `model.UnloadAsync()` | Αποφορτώνει το μοντέλο |
| `model.SelectVariant(variant)` | Επιλέγει συγκεκριμένη παραλλαγή (CPU/GPU/NPU) |
| `model.SelectedVariant` | Η παρούσα επιλεγμένη παραλλαγή |
| `model.Variants` | Όλες οι διαθέσιμες παραλλαγές για το μοντέλο |
| `model.GetPathAsync()` | Λαμβάνει την τοπική διαδρομή αρχείου |
| `model.GetChatClientAsync()` | Λαμβάνει εγγενή πελάτη συνομιλίας (δεν απαιτείται OpenAI SDK) |
| `model.GetAudioClientAsync()` | Λαμβάνει πελάτη ηχητικής απομαγνητοφώνησης |

</details>

---

### Άσκηση 4: Εξετάστε τα Μεταδεδομένα του Μοντέλου

Το αντικείμενο `FoundryModelInfo` περιέχει πλούσια μεταδεδομένα για κάθε μοντέλο. Η κατανόηση αυτών των πεδίων βοηθά στην επιλογή του κατάλληλου μοντέλου για την εφαρμογή σας.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Λάβετε λεπτομερείς πληροφορίες για ένα συγκεκριμένο μοντέλο
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

#### Πεδία FoundryModelInfo

| Πεδίο | Τύπος | Περιγραφή |
|-------|------|-------------|
| `alias` | string | Σύντομο όνομα (π.χ. `phi-3.5-mini`) |
| `id` | string | Μοναδικό αναγνωριστικό μοντέλου |
| `version` | string | Έκδοση μοντέλου |
| `task` | string | `chat-completions` ή `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU ή NPU |
| `execution_provider` | string | Backend εκτέλεσης (CUDA, CPU, QNN, WebGPU κλπ.) |
| `file_size_mb` | int | Μέγεθος σε MB στον δίσκο |
| `supports_tool_calling` | bool | Αν το μοντέλο υποστηρίζει κλήση λειτουργιών/εργαλείων |
| `publisher` | string | Ποιος δημοσίευσε το μοντέλο |
| `license` | string | Όνομα άδειας |
| `uri` | string | Διεύθυνση URI μοντέλου |
| `prompt_template` | dict/null | Πρότυπο προτροπής, αν υπάρχει |

---

### Άσκηση 5: Διαχειριστείτε τον Κύκλο Ζωής του Μοντέλου

Εξασκηθείτε στον πλήρη κύκλο: λίστα → λήψη → φόρτωση → χρήση → αποφόρτωση.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Μικρό μοντέλο για γρήγορη δοκιμή

manager = FoundryLocalManager()
manager.start_service()

# 1. Ελέγξτε τι υπάρχει στον κατάλογο
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Ελέγξτε τι έχει ήδη κατεβεί
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Κατεβάστε ένα μοντέλο
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Επαληθεύστε ότι είναι τώρα στην κρυφή μνήμη
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Φορτώστε το
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Ελέγξτε τι είναι φορτωμένο
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Αποφορτώστε το
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

const alias = "qwen2.5-0.5b"; // Μικρό μοντέλο για γρήγορο δοκιμαστικό έλεγχο

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Λάβετε το μοντέλο από τον κατάλογο
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Κατεβάστε αν χρειάζεται
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Φορτώστε το
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Αποφορτώστε το
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Άσκηση 6: Τα Πρότυπα Γρήγορης Εκκίνησης

Κάθε γλώσσα παρέχει μια συντόμευση για να ξεκινήσει η υπηρεσία και να φορτωθεί ένα μοντέλο με μία κλήση. Αυτά είναι τα **συνιστώμενα πρότυπα** για τις περισσότερες εφαρμογές.

<details>
<summary><h3>🐍 Python - Constructor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Δώστε ένα ψευδώνυμο στον κατασκευαστή - αυτός χειρίζεται τα πάντα:
# 1. Ξεκινάει την υπηρεσία αν δεν τρέχει
# 2. Κατεβάζει το μοντέλο αν δεν είναι αποθηκευμένο στην κρυφή μνήμη
# 3. Φορτώνει το μοντέλο στον διακομιστή συμπερασμάτων
manager = FoundryLocalManager("phi-3.5-mini")

# Έτοιμο προς χρήση αμέσως
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Η παράμετρος `bootstrap` (προεπιλογή `True`) ελέγχει αυτή τη συμπεριφορά. Ορίστε `bootstrap=False` αν θέλετε χειροκίνητο έλεγχο:

```python
# Χειροκίνητη λειτουργία - τίποτα δεν συμβαίνει αυτόματα
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() χειρίζονται τα πάντα:
// 1. Ξεκινά την υπηρεσία
// 2. Αποκτά το μοντέλο από τον κατάλογο
// 3. Κατεβάζει αν χρειάζεται και φορτώνει το μοντέλο
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Έτοιμο προς χρήση αμέσως
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

> **Σημείωση C#:** Το SDK του C# χρησιμοποιεί το `Configuration` για να ελέγχει το όνομα της εφαρμογής, την καταγραφή, τους φακέλους cache, και ακόμη για να ορίσει μια συγκεκριμένη θύρα web server. Αυτό το καθιστά το πιο παραμετροποιήσιμο από τα τρία SDKs.

</details>

---

### Άσκηση 7: Ο Φυσικός ChatClient (Δεν Απαιτείται OpenAI SDK)

Τα SDKs για JavaScript και C# παρέχουν τη μέθοδο εξυπηρέτησης `createChatClient()` που επιστρέφει έναν φυσικό chat client — δεν χρειάζεται να εγκαταστήσετε ή να παραμετροποιήσετε ξεχωριστά το OpenAI SDK.

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

// Δημιουργήστε ένα ChatClient απευθείας από το μοντέλο — δεν χρειάζεται εισαγωγή του OpenAI
const chatClient = model.createChatClient();

// completeChat επιστρέφει ένα αντικείμενο απόκρισης συμβατό με το OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Η ροή χρησιμοποιεί ένα μοτίβο ανάκλησης (callback)
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

Ο `ChatClient` υποστηρίζει επίσης κλήση εργαλείων — περάστε εργαλεία ως δεύτερο όρισμα:

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

> **Πότε να χρησιμοποιείτε ποιο πρότυπο:**
> - **`createChatClient()`** — Γρήγορο πρωτότυπο, λιγότερες εξαρτήσεις, απλούστερος κώδικας
> - **OpenAI SDK** — Πλήρης έλεγχος παραμέτρων (θερμοκρασία, top_p, stop tokens, κλπ.), καλύτερο για παραγωγικές εφαρμογές

---

### Άσκηση 8: Παραλλαγές Μοντέλων και Επιλογή Υλικού

Τα μοντέλα μπορούν να έχουν πολλαπλές **παραλλαγές** βελτιστοποιημένες για διαφορετικό υλικό. Το SDK επιλέγει αυτόματα την καλύτερη παραλλαγή, αλλά μπορείτε επίσης να ελέγξετε και να επιλέξετε χειροκίνητα.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Καταγράψτε όλες τις διαθέσιμες παραλλαγές
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// Το SDK επιλέγει αυτόματα την καλύτερη παραλλαγή για το υλικό σας
// Για αντικατάσταση, χρησιμοποιήστε selectVariant():
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

Στην Python, το SDK επιλέγει αυτόματα την καλύτερη παραλλαγή βάσει του υλικού. Χρησιμοποιήστε `get_model_info()` για να δείτε τι επιλέχθηκε:

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

#### Μοντέλα με Παραλλαγές NPU

Μερικά μοντέλα διαθέτουν παραλλαγές βελτιστοποιημένες για NPU (Units Νευρωνικής Επεξεργασίας) σε συσκευές όπως Qualcomm Snapdragon, Intel Core Ultra:

| Μοντέλο | Διαθέσιμη Παραλλαγή NPU |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Συμβουλή:** Σε υλικό με δυνατότητα NPU, το SDK επιλέγει αυτόματα την παραλλαγή NPU όταν είναι διαθέσιμη. Δεν χρειάζεται να αλλάξετε τον κώδικά σας. Για έργα C# σε Windows, προσθέστε το πακέτο NuGet `Microsoft.AI.Foundry.Local.WinML` για να ενεργοποιήσετε τον QNN execution provider — ο QNN παρέχεται ως plugin EP μέσω WinML.

---

### Άσκηση 9: Αναβαθμίσεις Μοντέλων και Ανανεώσεις Καταλόγου

Ο κατάλογος μοντέλων ενημερώνεται περιοδικά. Χρησιμοποιήστε αυτές τις μεθόδους για να ελέγξετε και να εφαρμόσετε ενημερώσεις.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Ανανέωσε τον κατάλογο για να πάρεις την πιο πρόσφατη λίστα μοντέλων
manager.refresh_catalog()

# Έλεγξε αν ένα αποθηκευμένο μοντέλο έχει διαθέσιμη νεότερη έκδοση
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

// Ανανεώστε τον κατάλογο για να λάβετε την πιο πρόσφατη λίστα μοντέλων
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Καταγράψτε όλα τα διαθέσιμα μοντέλα μετά την ανανέωση
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Άσκηση 10: Εργασία με Μοντέλα Συλλογισμού

Το μοντέλο **phi-4-mini-reasoning** περιλαμβάνει συλλογισμό αλυσίδας σκέψης. Περιβάλλει την εσωτερική του σκέψη σε ετικέτες `<think>...</think>` πριν παράγει την τελική του απάντηση. Αυτό είναι χρήσιμο για εργασίες που απαιτούν πολύ-βηματική λογική, μαθηματικά ή επίλυση προβλημάτων.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# Το phi-4-mini-reasoning είναι περίπου 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Το μοντέλο περικλείει τη σκέψη του σε ετικέτες <think>...</think>
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

// Ανάλυση σκέψης αλυσιδωτής σύνδεσης
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Πότε να χρησιμοποιείτε μοντέλα συλλογισμού:**
> - Μαθηματικά και λογικά προβλήματα
> - Πολύ-βηματικά προγραμματιστικά καθήκοντα
> - Σύνθετη δημιουργία κώδικα
> - Εργασίες όπου η επίδειξη των βημάτων βελτιώνει την ακρίβεια
>
> **Αντίβαρο:** Τα μοντέλα συλλογισμού παράγουν περισσότερα tokens (το τμήμα `<think>`) και είναι πιο αργά. Για απλές ερωτήσεις και απαντήσεις, ένα τυπικό μοντέλο όπως το phi-3.5-mini είναι ταχύτερο.

---

### Άσκηση 11: Κατανόηση Ψευδωνύμων και Επιλογή Υλικού

Όταν περνάτε ένα **ψευδώνυμο** (όπως `phi-3.5-mini`) αντί για πλήρες αναγνωριστικό μοντέλου, το SDK επιλέγει αυτόματα την καλύτερη παραλλαγή για το υλικό σας:

| Υλικό | Επιλεγμένος Execution Provider |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (μέσω WinML plugin) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Οποιαδήποτε συσκευή (εναλλακτικά) | `CPUExecutionProvider` ή `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Το ψευδώνυμο επιλύεται στην καλύτερη παραλλαγή για τον ΕΞΟΠΛΙΣΜΟ ΣΑΣ
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Συμβουλή:** Χρησιμοποιείτε πάντα ψευδώνυμα στον κώδικα της εφαρμογής σας. Όταν αναπτύσσετε σε υπολογιστή χρήστη, το SDK επιλέγει τη βέλτιστη παραλλαγή κατά το runtime - CUDA σε NVIDIA, QNN σε Qualcomm, CPU αλλού.

---

### Άσκηση 12: Επιλογές Παραμετροποίησης C#

Η κλάση `Configuration` του C# SDK παρέχει λεπτομερή έλεγχο του χρόνου εκτέλεσης:

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

| Ιδιότητα | Προεπιλογή | Περιγραφή |
|----------|---------|-------------|
| `AppName` | (απαιτείται) | Το όνομα της εφαρμογής σας |
| `LogLevel` | `Information` | Επιπέδου καταγραφής |
| `Web.Urls` | (δυναμικό) | Ορίζει συγκεκριμένη θύρα για τον web server |
| `AppDataDir` | Προεπιλογή OS | Βασικός φάκελος δεδομένων εφαρμογής |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Όπου αποθηκεύονται τα μοντέλα |
| `LogsDir` | `{AppDataDir}/logs` | Όπου γράφονται τα αρχεία καταγραφής |

---

### Άσκηση 13: Χρήση στον Περιηγητή (Μόνο JavaScript)

Το SDK JavaScript περιλαμβάνει μια έκδοση συμβατή με περιηγητές. Στον περιηγητή πρέπει να ξεκινήσετε χειροκίνητα την υπηρεσία μέσω CLI και να ορίσετε τη διεύθυνση URL του host:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Ξεκινήστε την υπηρεσία χειροκίνητα πρώτα:
//   foundry service start
// Στη συνέχεια, χρησιμοποιήστε το URL από την έξοδο της CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Περιηγηθείτε στον κατάλογο
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Περιορισμοί περιηγητή:** Η έκδοση περιηγητή **δεν** υποστηρίζει `startWebService()`. Πρέπει να βεβαιωθείτε ότι η υπηρεσία Foundry Local εκτελείται ήδη πριν χρησιμοποιήσετε το SDK σε περιηγητή.

---

## Πλήρης Αναφορά API

### Python

| Κατηγορία | Μέθοδος | Περιγραφή |
|----------|--------|-------------|
| **Εκκίνηση** | `FoundryLocalManager(alias?, bootstrap=True)` | Δημιουργία manager· προαιρετικά εκκίνηση με μοντέλο |
| **Υπηρεσία** | `is_service_running()` | Έλεγχος εάν η υπηρεσία τρέχει |
| **Υπηρεσία** | `start_service()` | Εκκίνηση της υπηρεσίας |
| **Υπηρεσία** | `endpoint` | URL endpoint API |
| **Υπηρεσία** | `api_key` | Κλειδί API |
| **Κατάλογος** | `list_catalog_models()` | Λίστα όλων των διαθέσιμων μοντέλων |
| **Κατάλογος** | `refresh_catalog()` | Ανανέωση καταλόγου |
| **Κατάλογος** | `get_model_info(alias_or_model_id)` | Λήψη μεταδεδομένων μοντέλου |
| **Cache** | `get_cache_location()` | Διαδρομή φακέλου cache |
| **Cache** | `list_cached_models()` | Λίστα κατεβασμένων μοντέλων |
| **Μοντέλο** | `download_model(alias_or_model_id)` | Λήψη μοντέλου |
| **Μοντέλο** | `load_model(alias_or_model_id, ttl=600)` | Φόρτωση μοντέλου |
| **Μοντέλο** | `unload_model(alias_or_model_id)` | Απεγκατάσταση μοντέλου |
| **Μοντέλο** | `list_loaded_models()` | Λίστα φορτωμένων μοντέλων |
| **Μοντέλο** | `is_model_upgradeable(alias_or_model_id)` | Έλεγχος εάν υπάρχει νεότερη έκδοση |
| **Μοντέλο** | `upgrade_model(alias_or_model_id)` | Αναβάθμιση μοντέλου στην πιο πρόσφατη έκδοση |
| **Υπηρεσία** | `httpx_client` | Προρυθμισμένος HTTPX client για απευθείας κλήσεις API |

### JavaScript

| Κατηγορία | Μέθοδος | Περιγραφή |
|----------|--------|-------------|
| **Εκκίνηση** | `FoundryLocalManager.create(options)` | Αρχικοποίηση singleton SDK |
| **Εκκίνηση** | `FoundryLocalManager.instance` | Πρόσβαση σε singleton manager |
| **Υπηρεσία** | `manager.startWebService()` | Εκκίνηση web υπηρεσίας |
| **Υπηρεσία** | `manager.urls` | Πίνακας με βασικά URLs για την υπηρεσία |
| **Κατάλογος** | `manager.catalog` | Πρόσβαση στον κατάλογο μοντέλων |
| **Κατάλογος** | `catalog.getModel(alias)` | Λήψη αντικειμένου μοντέλου από ψευδώνυμο (επιστρέφει Promise) |
| **Μοντέλο** | `model.isCached` | Εάν το μοντέλο είναι κατεβασμένο |
| **Μοντέλο** | `model.download()` | Κατέβασμα μοντέλου |
| **Μοντέλο** | `model.load()` | Φόρτωση μοντέλου |
| **Μοντέλο** | `model.unload()` | Απεγκατάσταση μοντέλου |
| **Μοντέλο** | `model.id` | Μοναδικός αναγνωριστικός αριθμός μοντέλου |
| **Μοντέλο** | `model.alias` | Ψευδώνυμο μοντέλου |
| **Μοντέλο** | `model.createChatClient()` | Λήψη φυσικού chat client (χωρίς ανάγκη OpenAI SDK) |
| **Μοντέλο** | `model.createAudioClient()` | Λήψη audio client για απομαγνητοφώνηση |
| **Μοντέλο** | `model.removeFromCache()` | Αφαίρεση μοντέλου από τοπική cache |
| **Μοντέλο** | `model.selectVariant(variant)` | Επιλογή συγκεκριμένης παραλλαγής υλικού |
| **Μοντέλο** | `model.variants` | Πίνακας διαθέσιμων παραλλαγών για το μοντέλο |
| **Μοντέλο** | `model.isLoaded()` | Έλεγχος εάν το μοντέλο είναι φορτωμένο |
| **Κατάλογος** | `catalog.getModels()` | Λίστα όλων των διαθέσιμων μοντέλων |
| **Κατάλογος** | `catalog.getCachedModels()` | Λίστα κατεβασμένων μοντέλων |
| **Κατάλογος** | `catalog.getLoadedModels()` | Λίστα φορτωμένων μοντέλων |
| **Κατάλογος** | `catalog.updateModels()` | Ανανέωση καταλόγου από την υπηρεσία |
| **Υπηρεσία** | `manager.stopWebService()` | Διακοπή της Foundry Local web υπηρεσίας |

### C# (v0.8.0+)

| Κατηγορία | Μέθοδος | Περιγραφή |
|----------|--------|-------------|
| **Εκκίνηση** | `FoundryLocalManager.CreateAsync(config, logger)` | Αρχικοποίηση του manager |
| **Εκκίνηση** | `FoundryLocalManager.Instance` | Πρόσβαση στο singleton |
| **Κατάλογος** | `manager.GetCatalogAsync()` | Λήψη καταλόγου |
| **Κατάλογος** | `catalog.ListModelsAsync()` | Λίστα όλων των μοντέλων |
| **Κατάλογος** | `catalog.GetModelAsync(alias)` | Λήψη συγκεκριμένου μοντέλου |
| **Κατάλογος** | `catalog.GetCachedModelsAsync()` | Λίστα κατεβασμένων μοντέλων |
| **Κατάλογος** | `catalog.GetLoadedModelsAsync()` | Λίστα φορτωμένων μοντέλων |
| **Μοντέλο** | `model.DownloadAsync(progress?)` | Λήψη μοντέλου |
| **Μοντέλο** | `model.LoadAsync()` | Φόρτωση μοντέλου |
| **Μοντέλο** | `model.UnloadAsync()` | Απεγκατάσταση μοντέλου |
| **Μοντέλο** | `model.SelectVariant(variant)` | Επιλογή παραλλαγής υλικού |
| **Μοντέλο** | `model.GetChatClientAsync()` | Λήψη φυσικού chat client |
| **Μοντέλο** | `model.GetAudioClientAsync()` | Λήψη audio transcription client |
| **Μοντέλο** | `model.GetPathAsync()` | Λήψη τοπικής διαδρομής αρχείου |
| **Κατάλογος** | `catalog.GetModelVariantAsync(alias, variant)` | Λήψη συγκεκριμένης παραλλαγής υλικού |
| **Κατάλογος** | `catalog.UpdateModelsAsync()` | Ανανέωση καταλόγου |
| **Εξυπηρετητής** | `manager.StartWebServerAsync()` | Εκκίνηση REST web server |
| **Εξυπηρετητής** | `manager.StopWebServerAsync()` | Διακοπή REST web server |
| **Ρυθμίσεις** | `config.ModelCacheDir` | Φάκελος cache |

---

## Βασικά Συμπεράσματα

| Έννοια | Τι Μάθατε |
|---------|-----------------|
| **SDK vs CLI** | Το SDK παρέχει προγραμματιστικό έλεγχο - απαραίτητο για εφαρμογές |
| **Control plane** | Το SDK διαχειρίζεται υπηρεσίες, μοντέλα και cache |
| **Δυναμικές θύρες** | Πάντα χρησιμοποιείτε `manager.endpoint` (Python) ή `manager.urls[0]` (JS/C#) - ποτέ μη σκληροκωδικοποιημένες θύρες |
| **Ψευδώνυμα** | Χρησιμοποιείτε ψευδώνυμα για αυτόματη επιλογή βέλτιστης παραλλαγής υλικού |
| **Γρήγορη εκκίνηση** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Επανασχεδιασμός C#** | Η έκδοση v0.8.0+ είναι αυτόνομη - δεν απαιτείται CLI στους υπολογιστές των τελικών χρηστών |
| **Κύκλος ζωής μοντέλου** | Κατάλογος → Λήψη → Φόρτωση → Χρήση → Εκφόρτωση |
| **FoundryModelInfo** | Πλούσια μεταδεδομένα: εργασία, συσκευή, μέγεθος, άδεια, υποστήριξη καλούντος εργαλείου |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) για χρήση χωρίς OpenAI |
| **Παραλλαγές** | Τα μοντέλα έχουν παραλλαγές ειδικές για υλικό (CPU, GPU, NPU)· επιλέγονται αυτόματα |
| **Αναβαθμίσεις** | Python: `is_model_upgradeable()` + `upgrade_model()` για διατήρηση των μοντέλων ενημερωμένων |
| **Ανανέωση καταλόγου** | `refresh_catalog()` (Python) / `updateModels()` (JS) για ανεύρεση νέων μοντέλων |

---

## Πόροι

| Πόρος | Σύνδεσμος |
|----------|------|
| Αναφορά SDK (όλες οι γλώσσες) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Ενσωμάτωση με SDK inference | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Αναφορά API C# SDK | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| Παραδείγματα C# SDK | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Ιστότοπος Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Επόμενα βήματα

Συνεχίστε στο [Μέρος 3: Χρήση του SDK με OpenAI](part3-sdk-and-apis.md) για να συνδέσετε το SDK με τη βιβλιοθήκη πελάτη OpenAI και να δημιουργήσετε την πρώτη σας εφαρμογή συμπλήρωσης συνομιλίας.