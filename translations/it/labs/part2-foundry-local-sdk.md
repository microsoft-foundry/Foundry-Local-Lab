![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 2: Approfondimento sul Foundry Local SDK

> **Obiettivo:** Padroneggiare il Foundry Local SDK per gestire modelli, servizi e caching in modo programmatico - e capire perché l’SDK è l'approccio consigliato rispetto alla CLI per costruire applicazioni.

## Panoramica

Nella Parte 1 hai utilizzato il **Foundry Local CLI** per scaricare e far girare i modelli in modo interattivo. La CLI è ottima per l’esplorazione, ma quando costruisci applicazioni reali hai bisogno di un **controllo programmatico**. Il Foundry Local SDK ti offre proprio questo - gestisce il **control plane** (avvio del servizio, scoperta dei modelli, download, caricamento) così il codice della tua applicazione può concentrarsi sul **data plane** (invio prompt, ricezione completamenti).

Questo laboratorio ti insegna l’intera API dello SDK in Python, JavaScript e C#. Alla fine capirai ogni metodo disponibile e quando usarlo.

## Obiettivi di Apprendimento

Al termine di questo laboratorio sarai in grado di:

- Spiegare perché l’SDK è preferito rispetto alla CLI per lo sviluppo applicativo
- Installare il Foundry Local SDK per Python, JavaScript o C#
- Usare `FoundryLocalManager` per avviare il servizio, gestire modelli e interrogare il catalogo
- Elencare, scaricare, caricare e scaricare modelli in modo programmatico
- Ispezionare i metadati del modello utilizzando `FoundryModelInfo`
- Comprendere la differenza tra modelli catalogati, in cache e caricati
- Usare il bootstrap del costruttore (Python) e il pattern `create()` + catalogo (JavaScript)
- Capire il redesign dello SDK C# e la sua API orientata agli oggetti

---

## Prerequisiti

| Requisito | Dettagli |
|-----------|----------|
| **Foundry Local CLI** | Installato e nel tuo `PATH` ([Parte 1](part1-getting-started.md)) |
| **Runtime linguaggio** | **Python 3.9+** e/o **Node.js 18+** e/o **.NET 9.0+** |

---

## Concetto: SDK vs CLI - Perché usare l’SDK?

| Aspetto | CLI (comando `foundry`) | SDK (`foundry-local-sdk`) |
|---------|-------------------------|---------------------------|
| **Caso d’uso** | Esplorazione, test manuale | Integrazione applicativa |
| **Gestione del servizio** | Manuale: `foundry service start` | Automatica: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Scoperta della porta** | Lettura dall’output CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Download modello** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Gestione errori** | Codici di uscita, stderr | Eccezioni, errori tipizzati |
| **Automazione** | Script shell | Integrazione nativa nel linguaggio |
| **Deployment** | Richiede CLI sulla macchina utente | SDK C# può essere standalone (senza CLI) |

> **Insight chiave:** L’SDK gestisce l’intero ciclo di vita: avvia il servizio, controlla la cache, scarica i modelli mancanti, li carica e scopre l’endpoint, in poche righe di codice. La tua applicazione non deve parsare output della CLI o gestire sottoprocessi.

---

## Esercizi di laboratorio

### Esercizio 1: Installare l’SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Verifica l’installazione:

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

Verifica l’installazione:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Ci sono due pacchetti NuGet:

| Pacchetto | Piattaforma | Descrizione |
|-----------|-------------|-------------|
| `Microsoft.AI.Foundry.Local` | Cross-platform | Funziona su Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Solo Windows | Aggiunge accelerazione hardware WinML; scarica e installa plugin per esecuzione (es. QNN per Qualcomm NPU) |

**Setup Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Modifica il file `.csproj`:

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

> **Nota:** Su Windows, il pacchetto WinML è un superset che include l’SDK base più il provider di esecuzione QNN. Su Linux/macOS si usa l’SDK base. La condizione TFM e i riferimenti al pacchetto rendono il progetto completamente cross-platform.

Crea un `nuget.config` nella radice del progetto:

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

Ripristina i pacchetti:

```bash
dotnet restore
```

</details>

---

### Esercizio 2: Avviare il Servizio e Elencare il Catalogo

La prima cosa che fa un’applicazione è avviare il servizio Foundry Local e scoprire quali modelli sono disponibili.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Crea un gestore e avvia il servizio
manager = FoundryLocalManager()
manager.start_service()

# Elenca tutti i modelli disponibili nel catalogo
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Metodi di gestione del servizio

| Metodo | Firma | Descrizione |
|--------|-------|-------------|
| `is_service_running()` | `() -> bool` | Controlla se il servizio è in esecuzione |
| `start_service()` | `() -> None` | Avvia il servizio Foundry Local |
| `service_uri` | `@property -> str` | URI base del servizio |
| `endpoint` | `@property -> str` | Endpoint API (URI servizio + `/v1`) |
| `api_key` | `@property -> str` | Chiave API (da env o valore di default) |

#### Python SDK - Metodi di gestione catalogo

| Metodo | Firma | Descrizione |
|--------|-------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Elenca tutti i modelli nel catalogo |
| `refresh_catalog()` | `() -> None` | Aggiorna il catalogo dal servizio |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Ottiene info per un modello specifico |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Crea un manager e avvia il servizio
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Sfoglia il catalogo
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Metodi del Manager

| Metodo | Firma | Descrizione |
|--------|-------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inizializza il singleton dello SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Accesso al manager singleton |
| `manager.startWebService()` | `() => Promise<void>` | Avvia il servizio web Foundry Local |
| `manager.urls` | `string[]` | Array degli URL base del servizio |

#### JavaScript SDK - Metodi del catalogo e modelli

| Metodo | Firma | Descrizione |
|--------|-------|-------------|
| `manager.catalog` | `Catalog` | Accede al catalogo modelli |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Ottiene un oggetto modello tramite alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Il C# SDK v0.8.0+ usa un’architettura orientata agli oggetti con `Configuration`, `Catalog` e `Model`:

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

#### C# SDK - Classi chiave

| Classe | Scopo |
|--------|-------|
| `Configuration` | Imposta nome app, livello log, directory cache, URL del server web |
| `FoundryLocalManager` | Entry point principale - creato tramite `CreateAsync()`, accessibile via `.Instance` |
| `Catalog` | Naviga, cerca e ottiene modelli dal catalogo |
| `Model` | Rappresenta un modello specifico - scarica, carica, ottiene client |

#### C# SDK - Metodi di Manager e Catalogo

| Metodo | Descrizione |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inizializza il manager |
| `FoundryLocalManager.Instance` | Accesso al manager singleton |
| `manager.GetCatalogAsync()` | Ottiene il catalogo modelli |
| `catalog.ListModelsAsync()` | Elenca tutti i modelli disponibili |
| `catalog.GetModelAsync(alias: "alias")` | Ottiene un modello specifico per alias |
| `catalog.GetCachedModelsAsync()` | Elenca modelli scaricati |
| `catalog.GetLoadedModelsAsync()` | Elenca modelli attualmente caricati |

> **Nota architetturale C#:** Il redesign dello SDK v0.8.0+ rende l’applicazione **autosufficiente**; non richiede il Foundry Local CLI sulla macchina dell’utente finale. Lo SDK gestisce nativamente la gestione modello e l’inferenza.

</details>

---

### Esercizio 3: Scaricare e Caricare un Modello

Lo SDK separa il download (su disco) dal caricamento (in memoria). Questo ti permette di pre-scaricare modelli in fase di setup e caricarli su richiesta.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Opzione A: Passo manuale passo dopo passo
manager = FoundryLocalManager()
manager.start_service()

# Controlla prima la cache
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

# Opzione B: Bootstrap in una riga (consigliato)
# Passa l'alias al costruttore - avvia il servizio, scarica e carica automaticamente
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Metodi di gestione modello

| Metodo | Firma | Descrizione |
|--------|-------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Scarica un modello nella cache locale |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Carica un modello nel server di inferenza |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Scarica un modello dal server |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Elenca tutti i modelli attualmente caricati |

#### Python - Metodi di gestione cache

| Metodo | Firma | Descrizione |
|--------|-------|-------------|
| `get_cache_location()` | `() -> str` | Ottiene il percorso della directory cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Elenca tutti i modelli scaricati |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Approccio passo-passo
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

#### JavaScript - Metodi modello

| Metodo | Firma | Descrizione |
|--------|-------|-------------|
| `model.isCached` | `boolean` | Indica se il modello è già scaricato |
| `model.download()` | `() => Promise<void>` | Scarica il modello nella cache locale |
| `model.load()` | `() => Promise<void>` | Carica nel server di inferenza |
| `model.unload()` | `() => Promise<void>` | Scarica dal server di inferenza |
| `model.id` | `string` | Identificativo univoco del modello |

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

#### C# - Metodi modello

| Metodo | Descrizione |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Scarica la variante selezionata |
| `model.LoadAsync()` | Carica il modello in memoria |
| `model.UnloadAsync()` | Scarica il modello |
| `model.SelectVariant(variant)` | Seleziona una variante specifica (CPU/GPU/NPU) |
| `model.SelectedVariant` | La variante attualmente selezionata |
| `model.Variants` | Tutte le varianti disponibili per questo modello |
| `model.GetPathAsync()` | Ottiene il percorso locale del file |
| `model.GetChatClientAsync()` | Ottiene un client chat nativo (senza OpenAI SDK) |
| `model.GetAudioClientAsync()` | Ottiene un client audio per trascrizione |

</details>

---

### Esercizio 4: Ispezionare i Metadati del Modello

L’oggetto `FoundryModelInfo` contiene metadati ricchi su ogni modello. Capire questi campi ti aiuta a scegliere il modello giusto per la tua applicazione.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Ottieni informazioni dettagliate su un modello specifico
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

#### Campi di FoundryModelInfo

| Campo | Tipo | Descrizione |
|-------|------|-------------|
| `alias` | stringa | Nome breve (es. `phi-3.5-mini`) |
| `id` | stringa | Identificativo univoco del modello |
| `version` | stringa | Versione del modello |
| `task` | stringa | `chat-completions` o `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU o NPU |
| `execution_provider` | stringa | Backend runtime (CUDA, CPU, QNN, WebGPU, ecc.) |
| `file_size_mb` | int | Dimensione su disco in MB |
| `supports_tool_calling` | bool | Se il modello supporta funzioni/chiamate a tool |
| `publisher` | stringa | Chi ha pubblicato il modello |
| `license` | stringa | Nome della licenza |
| `uri` | stringa | URI del modello |
| `prompt_template` | dict/null | Template del prompt, se presente |

---

### Esercizio 5: Gestire il Ciclo di Vita del Modello

Esercitati con il ciclo completo: elenco → download → carica → usa → scarica.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Modello piccolo per test rapidi

manager = FoundryLocalManager()
manager.start_service()

# 1. Controlla cosa c'è nel catalogo
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Controlla cosa è già scaricato
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Scarica un modello
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Verifica che sia ora nella cache
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Caricalo
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Controlla cosa è caricato
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Scaricalo
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

const alias = "qwen2.5-0.5b"; // Modello piccolo per test veloci

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Prendere il modello dal catalogo
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Scaricare se necessario
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Caricarlo
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Scaricarlo
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Esercizio 6: I Modelli Quick-Start

Ogni linguaggio fornisce una scorciatoia per avviare il servizio e caricare un modello in una sola chiamata. Questi sono i **modelli consigliati** per la maggior parte delle applicazioni.

<details>
<summary><h3>🐍 Python - Constructor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Passa un alias al costruttore - gestisce tutto:
# 1. Avvia il servizio se non è in esecuzione
# 2. Scarica il modello se non è in cache
# 3. Carica il modello nel server di inferenza
manager = FoundryLocalManager("phi-3.5-mini")

# Pronto all'uso immediatamente
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Il parametro `bootstrap` (predefinito `True`) controlla questo comportamento. Imposta `bootstrap=False` se desideri il controllo manuale:

```python
# Modalità manuale - nulla accade automaticamente
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() gestisce tutto:
// 1. Avvia il servizio
// 2. Ottiene il modello dal catalogo
// 3. Scarica se necessario e carica il modello
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Pronto per l'uso immediatamente
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

> **Nota C#:** L’SDK C# utilizza `Configuration` per controllare il nome dell’app, il logging, le directory cache e persino per fissare una porta specifica del server web. Questo lo rende il più configurabile dei tre SDK.

</details>

---

### Esercizio 7: Il ChatClient Nativo (Non Serve l’OpenAI SDK)

Gli SDK JavaScript e C# forniscono un metodo comodo `createChatClient()` che restituisce un client chat nativo — non è necessario installare o configurare separatamente l’OpenAI SDK.

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

// Crea un ChatClient direttamente dal modello — nessuna importazione OpenAI necessaria
const chatClient = model.createChatClient();

// completeChat restituisce un oggetto di risposta compatibile con OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Lo streaming utilizza un modello di callback
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

Il `ChatClient` supporta anche le chiamate agli strumenti — passa gli strumenti come secondo argomento:

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

> **Quando usare quale modello:**
> - **`createChatClient()`** — Prototipazione rapida, meno dipendenze, codice più semplice
> - **OpenAI SDK** — Controllo completo sui parametri (temperature, top_p, token di stop, ecc.), migliore per applicazioni in produzione

---

### Esercizio 8: Varianti dei Modelli e Selezione Hardware

I modelli possono avere diverse **varianti** ottimizzate per hardware differenti. L’SDK seleziona automaticamente la variante migliore, ma puoi anche ispezionare e scegliere manualmente.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Elenca tutte le varianti disponibili
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// L'SDK seleziona automaticamente la migliore variante per il tuo hardware
// Per sovrascrivere, usa selectVariant():
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

In Python, l’SDK seleziona automaticamente la variante migliore in base all’hardware. Usa `get_model_info()` per vedere qual è stata selezionata:

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

#### Modelli con Varianti NPU

Alcuni modelli hanno varianti ottimizzate per NPU (Neural Processing Units) in dispositivi come Qualcomm Snapdragon, Intel Core Ultra:

| Modello | Variante NPU Disponibile |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Suggerimento:** Su hardware con NPU, l’SDK seleziona automaticamente la variante NPU quando disponibile. Non è necessario modificare il codice. Per progetti C# su Windows, aggiungi il pacchetto NuGet `Microsoft.AI.Foundry.Local.WinML` per abilitare il provider di esecuzione QNN — QNN è fornito come plugin EP tramite WinML.

---

### Esercizio 9: Aggiornamenti del Modello e Aggiornamento del Catalogo

Il catalogo dei modelli viene aggiornato periodicamente. Usa questi metodi per controllare e applicare gli aggiornamenti.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Aggiorna il catalogo per ottenere l'ultima lista dei modelli
manager.refresh_catalog()

# Verifica se un modello memorizzato nella cache ha una versione più recente disponibile
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

// Aggiorna il catalogo per ottenere l'ultima lista dei modelli
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Elenca tutti i modelli disponibili dopo l'aggiornamento
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Esercizio 10: Lavorare con i Modelli di Ragionamento

Il modello **phi-4-mini-reasoning** include ragionamento a catena di pensiero (chain-of-thought). Incapsula il suo ragionamento interno in tag `<think>...</think>` prima di produrre la risposta finale. Questo è utile per compiti che richiedono logica multi-step, matematica o problem solving.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning è circa 4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Il modello incapsula il suo pensiero nei tag <think>...</think>
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

// Analizza il pensiero a catena
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Quando usare modelli di ragionamento:**
> - Problemi di matematica e logica
> - Compiti di pianificazione a più passaggi
> - Generazione codice complessa
> - Attività in cui mostrare il processo migliora la precisione
>
> **Contro:** I modelli di ragionamento producono più token (la sezione `<think>`) e sono più lenti. Per Q&A semplici, un modello standard come phi-3.5-mini è più veloce.

---

### Esercizio 11: Comprendere Alias e Selezione Hardware

Quando passi un **alias** (come `phi-3.5-mini`) invece di un ID modello completo, l’SDK seleziona automaticamente la variante migliore per il tuo hardware:

| Hardware | Provider di Esecuzione Selezionato |
|----------|---------------------------|
| GPU NVIDIA (CUDA) | `CUDAExecutionProvider` |
| NPU Qualcomm | `QNNExecutionProvider` (tramite plugin WinML) |
| NPU Intel | `OpenVINOExecutionProvider` |
| GPU AMD | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Qualsiasi dispositivo (fallback) | `CPUExecutionProvider` o `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# L'alias risolve nella variante migliore per IL TUO hardware
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Suggerimento:** Usa sempre gli alias nel codice della tua applicazione. Quando distribuisci sulla macchina di un utente, l’SDK sceglie la variante ottimale al runtime - CUDA su NVIDIA, QNN su Qualcomm, CPU altrove.

---

### Esercizio 12: Opzioni di Configurazione C#

La classe `Configuration` dell’SDK C# offre un controllo granulare sul runtime:

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

| Proprietà | Predefinito | Descrizione |
|----------|---------|-------------|
| `AppName` | (richiesto) | Nome della tua applicazione |
| `LogLevel` | `Information` | Verbosità del logging |
| `Web.Urls` | (dinamico) | Fissa una porta specifica per il server web |
| `AppDataDir` | Default OS | Directory base per i dati dell’app |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Dove sono archiviati i modelli |
| `LogsDir` | `{AppDataDir}/logs` | Dove vengono scritti i log |

---

### Esercizio 13: Uso nel Browser (Solo JavaScript)

L’SDK JavaScript include una versione compatibile con il browser. Nel browser, devi avviare manualmente il servizio via CLI e specificare l’URL host:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Avviare il servizio manualmente prima:
//   foundry service start
// Quindi utilizzare l'URL dall'output della CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Navigare nel catalogo
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Limitazioni del browser:** La versione browser **non** supporta `startWebService()`. Devi assicurarti che il servizio Foundry Local sia già in esecuzione prima di usare l’SDK nel browser.

---

## Riferimento Completo API

### Python

| Categoria | Metodo | Descrizione |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Crea il manager; opzionalmente fai bootstrap con un modello |
| **Service** | `is_service_running()` | Verifica se il servizio è in esecuzione |
| **Service** | `start_service()` | Avvia il servizio |
| **Service** | `endpoint` | URL endpoint API |
| **Service** | `api_key` | Chiave API |
| **Catalog** | `list_catalog_models()` | Elenca tutti i modelli disponibili |
| **Catalog** | `refresh_catalog()` | Aggiorna il catalogo |
| **Catalog** | `get_model_info(alias_or_model_id)` | Ottieni metadati del modello |
| **Cache** | `get_cache_location()` | Percorso directory cache |
| **Cache** | `list_cached_models()` | Elenca modelli scaricati |
| **Model** | `download_model(alias_or_model_id)` | Scarica un modello |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Carica un modello |
| **Model** | `unload_model(alias_or_model_id)` | Scarica un modello dalla memoria |
| **Model** | `list_loaded_models()` | Elenca modelli caricati |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Verifica se è disponibile una versione più nuova |
| **Model** | `upgrade_model(alias_or_model_id)` | Aggiorna un modello all’ultima versione |
| **Service** | `httpx_client` | Client HTTPX preconfigurato per chiamate API dirette |

### JavaScript

| Categoria | Metodo | Descrizione |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Inizializza il singleton SDK |
| **Init** | `FoundryLocalManager.instance` | Accesso al manager singleton |
| **Service** | `manager.startWebService()` | Avvia il servizio web |
| **Service** | `manager.urls` | Array di URL base del servizio |
| **Catalog** | `manager.catalog` | Accesso al catalogo modelli |
| **Catalog** | `catalog.getModel(alias)` | Ottieni un oggetto modello per alias (ritorna Promise) |
| **Model** | `model.isCached` | Indica se il modello è scaricato |
| **Model** | `model.download()` | Scarica il modello |
| **Model** | `model.load()` | Carica il modello |
| **Model** | `model.unload()` | Scarica il modello dalla memoria |
| **Model** | `model.id` | Identificatore univoco del modello |
| **Model** | `model.alias` | Alias del modello |
| **Model** | `model.createChatClient()` | Ottieni un client chat nativo (non serve OpenAI SDK) |
| **Model** | `model.createAudioClient()` | Ottieni un client audio per trascrizione |
| **Model** | `model.removeFromCache()` | Rimuovi il modello dalla cache locale |
| **Model** | `model.selectVariant(variant)` | Seleziona una variante hardware specifica |
| **Model** | `model.variants` | Array delle varianti disponibili per questo modello |
| **Model** | `model.isLoaded()` | Verifica se il modello è attualmente caricato |
| **Catalog** | `catalog.getModels()` | Elenca tutti i modelli disponibili |
| **Catalog** | `catalog.getCachedModels()` | Elenca i modelli scaricati |
| **Catalog** | `catalog.getLoadedModels()` | Elenca i modelli attualmente caricati |
| **Catalog** | `catalog.updateModels()` | Aggiorna il catalogo dal servizio |
| **Service** | `manager.stopWebService()` | Ferma il servizio web Foundry Local |

### C# (v0.8.0+)

| Categoria | Metodo | Descrizione |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Inizializza il manager |
| **Init** | `FoundryLocalManager.Instance` | Accesso al singleton |
| **Catalog** | `manager.GetCatalogAsync()` | Ottieni il catalogo |
| **Catalog** | `catalog.ListModelsAsync()` | Elenca tutti i modelli |
| **Catalog** | `catalog.GetModelAsync(alias)` | Ottieni un modello specifico |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Elenca i modelli in cache |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Elenca i modelli caricati |
| **Model** | `model.DownloadAsync(progress?)` | Scarica un modello |
| **Model** | `model.LoadAsync()` | Carica un modello |
| **Model** | `model.UnloadAsync()` | Scarica un modello dalla memoria |
| **Model** | `model.SelectVariant(variant)` | Scegli una variante hardware |
| **Model** | `model.GetChatClientAsync()` | Ottieni client chat nativo |
| **Model** | `model.GetAudioClientAsync()` | Ottieni client audio per trascrizione |
| **Model** | `model.GetPathAsync()` | Ottieni percorso file locale |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Ottieni una variante hardware specifica |
| **Catalog** | `catalog.UpdateModelsAsync()` | Aggiorna il catalogo |
| **Server** | `manager.StartWebServerAsync()` | Avvia il server web REST |
| **Server** | `manager.StopWebServerAsync()` | Ferma il server web REST |
| **Config** | `config.ModelCacheDir` | Directory cache |

---

## Punti Chiave

| Concetto | Cosa Hai Imparato |
|---------|-----------------|
| **SDK vs CLI** | L’SDK offre controllo programmatico – essenziale per le applicazioni |
| **Piano di controllo** | L’SDK gestisce servizi, modelli e caching |
| **Porte dinamiche** | Usa sempre `manager.endpoint` (Python) o `manager.urls[0]` (JS/C#) – mai una porta hardcoded |
| **Alias** | Usa alias per la selezione automatica della variante hardware ottimale |
| **Avvio rapido** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Ridisegno C#** | La versione 0.8.0+ è autonoma - non serve CLI sui computer degli utenti finali |
| **Ciclo di vita del modello** | Catalogo → Download → Caricamento → Uso → Scaricamento |
| **FoundryModelInfo** | Metadati ricchi: task, dispositivo, dimensione, licenza, supporto per richiamo da tool |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) per utilizzo senza OpenAI |
| **Varianti** | I modelli hanno varianti specifiche per hardware (CPU, GPU, NPU); selezionate automaticamente |
| **Aggiornamenti** | Python: `is_model_upgradeable()` + `upgrade_model()` per mantenere i modelli aggiornati |
| **Aggiornamento catalogo** | `refresh_catalog()` (Python) / `updateModels()` (JS) per scoprire nuovi modelli |

---

## Risorse

| Risorsa | Link |
|----------|------|
| Riferimento SDK (tutte le lingue) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrazione con SDK di inferenza | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Riferimento API SDK C# | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| Esempi SDK C# | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Sito di Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Passi successivi

Continua con [Parte 3: Uso dello SDK con OpenAI](part3-sdk-and-apis.md) per collegare lo SDK alla libreria client OpenAI e costruire la tua prima applicazione di completamento di chat.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:  
Questo documento è stato tradotto utilizzando il servizio di traduzione AI [Co-op Translator](https://github.com/Azure/co-op-translator). Pur impegnandoci per garantire l'accuratezza, si prega di notare che le traduzioni automatiche possono contenere errori o inesattezze. Il documento originale nella sua lingua madre deve essere considerato la fonte autorevole. Per informazioni critiche, si consiglia una traduzione professionale effettuata da un umano. Non siamo responsabili per eventuali fraintendimenti o interpretazioni errate derivanti dall'uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->