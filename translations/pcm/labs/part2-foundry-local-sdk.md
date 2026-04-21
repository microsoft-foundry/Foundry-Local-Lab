![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 2: Foundry Local SDK Deep Dive

> **Goal:** Master di Foundry Local SDK to manage models, services, an caching programmatically - an understand why di SDK na di recommended way pass CLI for build applications.

## Overview

For Part 1, you use di **Foundry Local CLI** to download an run models interactively. Di CLI good for exploration, but wen you dey build correct applications you need **programmatic control**. Di Foundry Local SDK go give you dat - e dey manage di **control plane** (start di service, discover models, download, load) so your application code fit focus on di **data plane** (send prompts, receive completions).

Dis lab dey teach you di complete SDK API surface for Python, JavaScript, an C#. By di end you go sabi every method wey dey an wen to use each one.

## Learning Objectives

By di end of dis lab you go fit:

- Explain why di SDK dey preferred pass CLI for application development
- Install di Foundry Local SDK for Python, JavaScript, or C#
- Use `FoundryLocalManager` start di service, manage models, an query di catalog
- List, download, load, an unload models programmatically
- Inspect model metadata using `FoundryModelInfo`
- Understand di difference between catalog, cache, an loaded models
- Use di constructor bootstrap (Python) an `create()` + catalog pattern (JavaScript)
- Understand di C# SDK redesign an its object-oriented API

---

## Prerequisites

| Requirement | Details |
|-------------|---------|
| **Foundry Local CLI** | Installed an dey your `PATH` ([Part 1](part1-getting-started.md)) |
| **Language runtime** | **Python 3.9+** an/or **Node.js 18+** an/or **.NET 9.0+** |

---

## Concept: SDK vs CLI - Why Use di SDK?

| Aspect | CLI (`foundry` command) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Use case** | Exploration, manual testing | Application integration |
| **Service management** | Manual: `foundry service start` | Automatic: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Port discovery** | Read from CLI output | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Model download** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Error handling** | Exit codes, stderr | Exceptions, typed errors |
| **Automation** | Shell scripts | Native language integration |
| **Deployment** | Dem need CLI for end-user machine | C# SDK fit be self-contained (no CLI needed) |

> **Key insight:** Di SDK dey handle di whole lifecycle: start di service, check di cache, download missing models, load dem, an discover di endpoint, all inside few lines code. Your application no need parse CLI output or manage subprocess.

---

## Lab Exercises

### Exercise 1: Install di SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Verify di installation:

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

Verify di installation:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Dem get two NuGet packages:

| Package | Platform | Description |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Cross-platform | Dey work for Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Windows only | Adds WinML hardware acceleration; dey download an install plugin execution providers (e.g. QNN for Qualcomm NPU) |

**Windows setup:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Edit di `.csproj` file:

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

> **Note:** For Windows, di WinML package na superset wey get di base SDK plus di QNN execution provider. For Linux/macOS, di base SDK na im dem dey use. Di conditional TFM an package references dey keep di project fully cross-platform.

Create `nuget.config` for di project root:

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

Restore packages:

```bash
dotnet restore
```

</details>

---

### Exercise 2: Start di Service an List di Catalog

Di first thing wey any application go do na to start di Foundry Local service an find out wetin models dey available.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Make manager and start di service
manager = FoundryLocalManager()
manager.start_service()

# List all di models wey dey dia catalog
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Service Management Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Check if service dey run |
| `start_service()` | `() -> None` | Start di Foundry Local service |
| `service_uri` | `@property -> str` | Base service URI |
| `endpoint` | `@property -> str` | Di API endpoint (service URI + `/v1`) |
| `api_key` | `@property -> str` | API key (from env or default placeholder) |

#### Python SDK - Catalog Management Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | List all models for di catalog |
| `refresh_catalog()` | `() -> None` | Refresh di catalog from di service |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Get info for one specific model |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Make manager and start di service
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Browse di katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Manager Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Initialise di SDK singleton |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Access di singleton manager |
| `manager.startWebService()` | `() => Promise<void>` | Start di Foundry Local web service |
| `manager.urls` | `string[]` | Array of base URLs for di service |

#### JavaScript SDK - Catalog and Model Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Access di model catalog |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Get model object by alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Di C# SDK v0.8.0+ dey use object-oriented architecture with `Configuration`, `Catalog`, an `Model` objects:

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

#### C# SDK - Key Classes

| Class | Purpose |
|-------|---------|
| `Configuration` | Set app name, log level, cache dir, web server URLs |
| `FoundryLocalManager` | Main entry point - created via `CreateAsync()`, accessed via `.Instance` |
| `Catalog` | Browse, search, an get models from di catalog |
| `Model` | Represent one specific model - download, load, get clients |

#### C# SDK - Manager and Catalog Methods

| Method | Description |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Initialise di manager |
| `FoundryLocalManager.Instance` | Access di singleton manager |
| `manager.GetCatalogAsync()` | Get di model catalog |
| `catalog.ListModelsAsync()` | List all available models |
| `catalog.GetModelAsync(alias: "alias")` | Get one specific model by alias |
| `catalog.GetCachedModelsAsync()` | List downloaded models |
| `catalog.GetLoadedModelsAsync()` | List models wey dem load currently |

> **C# Architecture Note:** Di C# SDK v0.8.0+ redesign make di application **self-contained**; e no need Foundry Local CLI for di end-user machine. Di SDK dey manage model management an inference natively.

</details>

---

### Exercise 3: Download an Load One Model

Di SDK separate downloading (to disk) from loading (inside memory). Dis one go allow you pre-download models wen you dey setup an load dem wen you need.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Option A: Manually do am step by step
manager = FoundryLocalManager()
manager.start_service()

# Make you check di cache first
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

# Option B: One-liner bootstrap (wey we recommend)
# Pass alias gɛt constructor - e go start di service, download, and load am for you automatically
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Model Management Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Download model go local cache |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Load model into di inference server |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Unload model from di server |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | List all models wey currently loaded |

#### Python - Cache Management Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Get di cache directory path |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | List all models wey don download before |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Step-by-step way to do am
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

#### JavaScript - Model Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Whether model don download before |
| `model.download()` | `() => Promise<void>` | Download di model go local cache |
| `model.load()` | `() => Promise<void>` | Load into di inference server |
| `model.unload()` | `() => Promise<void>` | Unload from inference server |
| `model.id` | `string` | Di model unique identifier |

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

#### C# - Model Methods

| Method | Description |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Download di selected variant |
| `model.LoadAsync()` | Load di model inside memory |
| `model.UnloadAsync()` | Unload di model |
| `model.SelectVariant(variant)` | Select one specific variant (CPU/GPU/NPU) |
| `model.SelectedVariant` | Di variant wey current dem select |
| `model.Variants` | All variants wey available for dis model |
| `model.GetPathAsync()` | Get di local file path |
| `model.GetChatClientAsync()` | Get native chat client (no OpenAI SDK needed) |
| `model.GetAudioClientAsync()` | Get audio client for transcription |

</details>

---

### Exercise 4: Inspect Model Metadata

Di `FoundryModelInfo` object get rich metadata about every model. If you sabi dis fields well, e go help you choose di right model for your application.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Make you get detailed info about one kind model
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

#### FoundryModelInfo Fields

| Field | Type | Description |
|-------|------|-------------|
| `alias` | string | Short name (example: `phi-3.5-mini`) |
| `id` | string | Unique model identifier |
| `version` | string | Model version |
| `task` | string | `chat-completions` or `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, or NPU |
| `execution_provider` | string | Runtime backend (CUDA, CPU, QNN, WebGPU, etc.) |
| `file_size_mb` | int | Size for disk inside MB |
| `supports_tool_calling` | bool | Whether di model fit support function/tool calling |
| `publisher` | string | Who publish di model |
| `license` | string | License name |
| `uri` | string | Model URI |
| `prompt_template` | dict/null | Prompt template, if e dey |

---

### Exercise 5: Manage Model Lifecycle

Practice di full lifecycle: list → download → load → use → unload.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Small model for quick testing

manager = FoundryLocalManager()
manager.start_service()

# 1. Check wetin dey inside the catalog
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Check wetin don already download
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Download one model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Confirm say e dey for the cache now
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Load am
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Check wetin don load
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Unload am
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

const alias = "qwen2.5-0.5b"; // Smol model for fast testing

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Fetch model from catalog
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Download if e necessary
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Load am
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Unload am
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Exercise 6: The Quick-Start Patterns

Each language dey provide shortcut to start di service and load model for one call. Dis na di **recommended patterns** for most apps.

<details>
<summary><h3>🐍 Python - Constructor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Pass one alias go the constructor - e go handle everytin:
# 1. E go start the service if e no dey run
# 2. E go download the model if e no dey for cache
# 3. E go load the model enter the inference server
manager = FoundryLocalManager("phi-3.5-mini")

# E ready to use sharp sharp
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Di `bootstrap` parameter (wey default na `True`) dey control dis behaviour. Set `bootstrap=False` if you want make you dey control manually:

```python
# Manual mode - nɔtin go happen by yɑs yɑs
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() de handle everything:
// 1. E go start the service
// 2. E go get di model from di catalog
// 3. E go download if e need and load di model
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Ready to use sharp sharp
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

> **C# Note:** Di C# SDK dey use `Configuration` control app name, logging, cache folders, and e even fit pin one specific web server port. Dis one make am di most configurable for di three SDKs dem.

</details>

---

### Exercise 7: The Native ChatClient (No OpenAI SDK Needed)

JavaScript and C# SDK dem get one `createChatClient()` ease-of-use method wey dey give native chat client — you no need install or configure OpenAI SDK separately.

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

// Mak ChatClient sharp sharp from di model — no need to carry OpenAI come
const chatClient = model.createChatClient();

// completeChat dey return response wey OpenAI fit understand
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming dey use callback style
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

Di `ChatClient` still dey support tool calling — pass tools as di second argument:

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

> **When to use which pattern:**
> - **`createChatClient()`** — Quick prototyping, less dependencies, simpler code
> - **OpenAI SDK** — Full control over parameters (temperature, top_p, stop tokens, etc.), better for production apps

---

### Exercise 8: Model Variants and Hardware Selection

Models fit get multiple **variants** wey dem optimize for different hardware. SDK go pick di best variant automatically, but you fit also check and select manually.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// List all di available kinds
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// Di SDK go automatically choose di best kind for your hardware
// To change am, use selectVariant():
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

For Python, di SDK dey automatically select di best variant based on hardware. Use `get_model_info()` make you see wetin e select:

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

#### Models wit NPU Variants

Some models get NPU-optimized variants for devices wey get Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Model | NPU Variant Available |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Tip:** For NPU-capable hardware, di SDK go automatically select di NPU variant if e dey available. You no need change your code. For C# projects wey dey Windows, add `Microsoft.AI.Foundry.Local.WinML` NuGet package to enable QNN execution provider — QNN dey deliver as plugin EP through WinML.

---

### Exercise 9: Model Upgrades and Catalog Refresh

Model catalog dey update from time to time. Use these methods to check and apply updates.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Refresh di catalog make you fit get di latest model list
manager.refresh_catalog()

# Check if di cached model get newer version wey dey available
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

// Make e fresh di catalog make e bring di latest model list
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Show all di models wey dey after e don refresh
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Exercise 10: Working with Reasoning Models

Di **phi-4-mini-reasoning** model include chain-of-thought reasoning. E dey wrap e internal thinking inside `<think>...</think>` tags before e produce di final answer. Dis one good for tasks wey need multi-step logic, maths, or problem-solving.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning na about 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Di model dey put im tok insaid <think>...</think> tags
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

// Make sense for how person dey reason gbege by gbege
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **When to use reasoning models:**
> - Maths and logic wahala
> - Multi-step planning tasks
> - Complex code generation
> - Tasks wey showing work dey improve accuracy
>
> **Trade-off:** Reasoning models dey produce more tokens (inside `<think>` section) and dem dey slower. For simple Q&A, standard model like phi-3.5-mini dey faster.

---

### Exercise 11: Understanding Aliases and Hardware Selection

When you pass **alias** (like `phi-3.5-mini`) instead of full model ID, SDK go automatically select di best variant for your hardware:

| Hardware | Selected Execution Provider |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (through WinML plugin) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Any device (fallback) | `CPUExecutionProvider` or `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Di alias go resolve to di best variant wey fit YOUR hardware
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tip:** Always use aliases inside your app code. When you deploy to user machine, SDK go pick di optimal variant at runtime - CUDA for NVIDIA, QNN for Qualcomm, CPU for other places.

---

### Exercise 12: C# Configuration Options

Di C# SDK `Configuration` class dey provide fine control over runtime:

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

| Property | Default | Description |
|----------|---------|-------------|
| `AppName` | (required) | Your app name |
| `LogLevel` | `Information` | Logging verbosity |
| `Web.Urls` | (dynamic) | Pin one specific port for web server |
| `AppDataDir` | OS default | Base directory for app data |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Place wey models dey stored |
| `LogsDir` | `{AppDataDir}/logs` | Place wey log files dey saved |

---

### Exercise 13: Browser Usage (JavaScript Only)

JavaScript SDK get browser-compatible version. For browser, you go need start di service manually via CLI and specify di host URL:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Start di service by hand first:
//   foundry service start
// Den use di URL wey come out from di CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Browse di catalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Browser limitations:** Di browser version no dey support `startWebService()`. You must make sure Foundry Local service dey already run before you use SDK for browser.

---

## Complete API Reference

### Python

| Category | Method | Description |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Create manager; optionally bootstrap with one model |
| **Service** | `is_service_running()` | Check if service dey run |
| **Service** | `start_service()` | Start di service |
| **Service** | `endpoint` | API endpoint URL |
| **Service** | `api_key` | API key |
| **Catalog** | `list_catalog_models()` | List all available models |
| **Catalog** | `refresh_catalog()` | Refresh di catalog |
| **Catalog** | `get_model_info(alias_or_model_id)` | Get model metadata |
| **Cache** | `get_cache_location()` | Cache directory path |
| **Cache** | `list_cached_models()` | List downloaded models dem |
| **Model** | `download_model(alias_or_model_id)` | Download model |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Load model |
| **Model** | `unload_model(alias_or_model_id)` | Unload model |
| **Model** | `list_loaded_models()` | List loaded models |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Check if newer version dey |
| **Model** | `upgrade_model(alias_or_model_id)` | Upgrade model to latest version |
| **Service** | `httpx_client` | Pre-configured HTTPX client for direct API calls |

### JavaScript

| Category | Method | Description |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Initialise di SDK singleton |
| **Init** | `FoundryLocalManager.instance` | Access di singleton manager |
| **Service** | `manager.startWebService()` | Start di web service |
| **Service** | `manager.urls` | Array of base URLs for di service |
| **Catalog** | `manager.catalog` | Access di model catalog |
| **Catalog** | `catalog.getModel(alias)` | Get model object by alias (returns Promise) |
| **Model** | `model.isCached` | Whether model don download |
| **Model** | `model.download()` | Download model |
| **Model** | `model.load()` | Load model |
| **Model** | `model.unload()` | Unload model |
| **Model** | `model.id` | Model unique identifier |
| **Model** | `model.alias` | Model alias |
| **Model** | `model.createChatClient()` | Get native chat client (no OpenAI SDK needed) |
| **Model** | `model.createAudioClient()` | Get audio client for transcription |
| **Model** | `model.removeFromCache()` | Remove model from local cache |
| **Model** | `model.selectVariant(variant)` | Select specific hardware variant |
| **Model** | `model.variants` | Array of variants available for dis model |
| **Model** | `model.isLoaded()` | Check if model dey loaded now |
| **Catalog** | `catalog.getModels()` | List all available models |
| **Catalog** | `catalog.getCachedModels()` | List downloaded models |
| **Catalog** | `catalog.getLoadedModels()` | List loaded models |
| **Catalog** | `catalog.updateModels()` | Refresh catalog from service |
| **Service** | `manager.stopWebService()` | Stop Foundry Local web service |

### C# (v0.8.0+)

| Category | Method | Description |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Initialise manager |
| **Init** | `FoundryLocalManager.Instance` | Access singleton |
| **Catalog** | `manager.GetCatalogAsync()` | Get catalog |
| **Catalog** | `catalog.ListModelsAsync()` | List all models |
| **Catalog** | `catalog.GetModelAsync(alias)` | Get specific model |
| **Catalog** | `catalog.GetCachedModelsAsync()` | List cached models |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | List loaded models |
| **Model** | `model.DownloadAsync(progress?)` | Download model |
| **Model** | `model.LoadAsync()` | Load model |
| **Model** | `model.UnloadAsync()` | Unload model |
| **Model** | `model.SelectVariant(variant)` | Choose hardware variant |
| **Model** | `model.GetChatClientAsync()` | Get native chat client |
| **Model** | `model.GetAudioClientAsync()` | Get audio transcription client |
| **Model** | `model.GetPathAsync()` | Get local file path |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Get specific hardware variant |
| **Catalog** | `catalog.UpdateModelsAsync()` | Refresh catalog |
| **Server** | `manager.StartWebServerAsync()` | Start REST web server |
| **Server** | `manager.StopWebServerAsync()` | Stop REST web server |
| **Config** | `config.ModelCacheDir` | Cache directory |

---

## Key Takeaways

| Concept | Wetin You Learn |
|---------|-----------------|
| **SDK vs CLI** | SDK dey provide programmatic control - important for apps |
| **Control plane** | SDK dey manage services, models, and caching |
| **Dynamic ports** | Always use `manager.endpoint` (Python) or `manager.urls[0]` (JS/C#) - no hardcode port |
| **Aliases** | Use aliases for automatic hardware-optimal model selection |
| **Quick-start** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# redesign** | v0.8.0+ na standalone - no CLI wey user machine need |
| **Model lifecycle** | Catalog → Download → Load → Use → Unload |
| **FoundryModelInfo** | Correct metadata: task, device, size, license, tool wey fit call am |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) for OpenAI free use |
| **Variants** | Models get hardware-specific variants (CPU, GPU, NPU); e dey select automatically |
| **Upgrades** | Python: `is_model_upgradeable()` + `upgrade_model()` to keep models fresh |
| **Catalog refresh** | `refresh_catalog()` (Python) / `updateModels()` (JS) to find new models |

---

## Resources

| Resource | Link |
|----------|------|
| SDK Reference (all languages) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrate with inference SDKs | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API Reference | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK Samples | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local website | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Next Steps

Continue to [Part 3: Using the SDK with OpenAI](part3-sdk-and-apis.md) to connect the SDK to the OpenAI client library and build your first chat completion application.