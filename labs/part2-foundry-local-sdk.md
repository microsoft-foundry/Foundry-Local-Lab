![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 2: Foundry Local SDK Deep Dive

> **Goal:** Master the Foundry Local SDK to manage models, services, and caching programmatically - and understand why the SDK is the recommended approach over the CLI for building applications.

## Overview

In Part 1 you used the **Foundry Local CLI** to download and run models interactively. The CLI is great for exploration, but when you build real applications you need **programmatic control**. The Foundry Local SDK gives you that - it manages the **control plane** (starting the service, discovering models, downloading, loading) so your application code can focus on the **data plane** (sending prompts, receiving completions).

This lab teaches you the full SDK API surface across Python, JavaScript, and C#. By the end you will understand every method available and when to use each one.

## Learning Objectives

By the end of this lab you will be able to:

- Explain why the SDK is preferred over the CLI for application development
- Install the Foundry Local SDK for Python, JavaScript, or C#
- Use `FoundryLocalManager` to start the service, manage models, and query the catalog
- List, download, load, and unload models programmatically
- Inspect model metadata using `FoundryModelInfo`
- Understand the difference between catalog, cache, and loaded models
- Use the `init()` convenience method (JavaScript) and constructor bootstrap (Python)
- Understand the C# SDK redesign and its object-oriented API

---

## Prerequisites

| Requirement | Details |
|-------------|---------|
| **Foundry Local CLI** | Installed and on your `PATH` ([Part 1](part1-getting-started.md)) |
| **Language runtime** | **Python 3.9+** and/or **Node.js 18+** and/or **.NET 9.0+** |

---

## Concept: SDK vs CLI - Why Use the SDK?

| Aspect | CLI (`foundry` command) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Use case** | Exploration, manual testing | Application integration |
| **Service management** | Manual: `foundry service start` | Automatic: `manager.start_service()` |
| **Port discovery** | Read from CLI output | `manager.endpoint` property |
| **Model download** | `foundry model download alias` | `manager.download_model(alias)` with progress callbacks |
| **Error handling** | Exit codes, stderr | Exceptions, typed errors |
| **Automation** | Shell scripts | Native language integration |
| **Deployment** | Requires CLI on end-user machine | C# SDK can be self-contained (no CLI needed) |

> **Key insight:** The SDK handles the entire lifecycle: starting the service, checking the cache, downloading missing models, loading them, and discovering the endpoint, in a few lines of code. Your application does not need to parse CLI output or manage subprocesses.

---

## Lab Exercises

### Exercise 1: Install the SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Verify the installation:

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

Verify the installation:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

There are two NuGet packages:

| Package | Platform | Description |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local.WinML` | Windows only | Uses Windows Machine Learning (WinML) for hardware acceleration |
| `Microsoft.AI.Foundry.Local` | Cross-platform | Works on Windows, Linux, macOS |

**Windows setup:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Edit the `.csproj` file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0-windows10.0.26100</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <WindowsAppSDKSelfContained>false</WindowsAppSDKSelfContained>
    <WindowsPackageType>None</WindowsPackageType>
    <EnableCoreMrtTooling>false</EnableCoreMrtTooling>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.AI.Foundry.Local.WinML" Version="0.8.2.1" />
    <PackageReference Include="Microsoft.Extensions.Logging" Version="9.0.10" />
  </ItemGroup>
</Project>
```

Create a `nuget.config` in the project root:

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

### Exercise 2: Start the Service and List the Catalog

The first thing any application does is start the Foundry Local service and discover what models are available.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Create a manager and start the service
manager = FoundryLocalManager()
manager.start_service()

# List all models available in the catalog
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
| `is_service_running()` | `() -> bool` | Check if the service is running |
| `start_service()` | `() -> None` | Start the Foundry Local service |
| `service_uri` | `@property -> str` | The base service URI |
| `endpoint` | `@property -> str` | The API endpoint (service URI + `/v1`) |
| `api_key` | `@property -> str` | API key (from env or default placeholder) |

#### Python SDK - Catalog Management Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | List all models in the catalog |
| `refresh_catalog()` | `() -> None` | Refresh the catalog from the service |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Get info for a specific model |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Create a manager and start the service
const manager = new FoundryLocalManager();
await manager.startService();

// List all models available in the catalog
const catalog = await manager.listCatalogModels();
console.log(`Models available in catalog: ${catalog.length}`);

for (const model of catalog) {
  console.log(`  - ${model.alias} (${model.id})`);
  console.log(`    Task: ${model.task}, Size: ${model.fileSizeMb} MB`);
}
```

#### JavaScript SDK - Service Management Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `init()` | `(aliasOrModelId?: string) => Promise<FoundryModelInfo \| void>` | Initialize SDK and optionally load a model |
| `isServiceRunning()` | `() => Promise<boolean>` | Check if the service is running |
| `startService()` | `() => Promise<void>` | Start the Foundry Local service |
| `serviceUrl` | `string` | The base URL of the service |
| `endpoint` | `string` | The API endpoint (serviceUrl + `/v1`) |
| `apiKey` | `string` | API key (placeholder for local) |

#### JavaScript SDK - Catalog Management Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `listCatalogModels()` | `() => Promise<FoundryModelInfo[]>` | List all models in the catalog |
| `refreshCatalog()` | `() => Promise<void>` | Refresh the catalog |
| `getModelInfo()` | `(aliasOrModelId: string, throwOnNotFound = false) => Promise<FoundryModelInfo \| null>` | Get info for a specific model |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

The C# SDK v0.8.0+ uses an object-oriented architecture with `Configuration`, `Catalog`, and `Model` objects:

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
| `Catalog` | Browse, search, and get models from the catalog |
| `Model` | Represents a specific model - download, load, get clients |

#### C# SDK - Manager and Catalog Methods

| Method | Description |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Initialize the manager |
| `FoundryLocalManager.Instance` | Access the singleton manager |
| `manager.GetCatalogAsync()` | Get the model catalog |
| `catalog.ListModelsAsync()` | List all available models |
| `catalog.GetModelAsync(alias: "alias")` | Get a specific model by alias |
| `catalog.GetCachedModelsAsync()` | List downloaded models |
| `catalog.GetLoadedModelsAsync()` | List currently loaded models |

> **C# Architecture Note:** The C# SDK v0.8.0+ redesign makes the application **self-contained**; it does not require the Foundry Local CLI on the end-user's machine. The SDK handles model management and inference natively.

</details>

---

### Exercise 3: Download and Load a Model

The SDK separates downloading (to disk) from loading (into memory). This lets you pre-download models during setup and load them on demand.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Option A: Manual step-by-step
manager = FoundryLocalManager()
manager.start_service()

# Check cache first
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

# Option B: One-liner bootstrap (recommended)
# Pass alias to constructor - it starts the service, downloads, and loads automatically
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Model Management Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Download a model to local cache |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Load a model into the inference server |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Unload a model from the server |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | List all currently loaded models |

#### Python - Cache Management Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Get the cache directory path |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | List all downloaded models |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Option A: Manual step-by-step
const manager = new FoundryLocalManager();
await manager.startService();

const cached = await manager.listCachedModels();
const catalogInfo = await manager.getModelInfo(alias);
const isAlreadyCached = cached.some((m) => m.id === catalogInfo?.id);

if (!isAlreadyCached) {
  console.log(`Downloading ${alias}...`);
  await manager.downloadModel(alias, undefined, false, (progress) => {
    process.stdout.write(`\rDownloading: ${progress.toFixed(1)}%`);
    if (progress >= 100) process.stdout.write("\n");
  });
}

console.log(`Loading ${alias}...`);
const modelInfo = await manager.loadModel(alias);
console.log(`Loaded: ${modelInfo.id}`);
console.log(`Endpoint: ${manager.endpoint}`);

// Option B: One-liner init (recommended)
// init() starts the service, downloads, and loads in one call
const manager2 = new FoundryLocalManager();
const model = await manager2.init(alias);
console.log(`Ready! Model: ${model.id}, Endpoint: ${manager2.endpoint}`);
```

#### JavaScript - Model Management Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `downloadModel()` | `(aliasOrModelId, token?, force?, onProgress?) => Promise<FoundryModelInfo>` | Download with optional progress callback |
| `loadModel()` | `(aliasOrModelId, ttl?) => Promise<FoundryModelInfo>` | Load into inference server |
| `unloadModel()` | `(aliasOrModelId, force?) => Promise<void>` | Unload from inference server |
| `listLoadedModels()` | `() => Promise<FoundryModelInfo[]>` | List all loaded models |

#### JavaScript - Cache Management Methods

| Method | Signature | Description |
|--------|-----------|-------------|
| `getCacheLocation()` | `() => Promise<string>` | Get the cache directory path |
| `listCachedModels()` | `() => Promise<FoundryModelInfo[]>` | List all downloaded models |

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
| `model.DownloadAsync(progress?)` | Download the selected variant |
| `model.LoadAsync()` | Load the model into memory |
| `model.UnloadAsync()` | Unload the model |
| `model.SelectVariant(variant)` | Select a specific variant (CPU/GPU/NPU) |
| `model.SelectedVariant` | The currently selected variant |
| `model.Variants` | All available variants for this model |
| `model.GetPathAsync()` | Get the local file path |
| `model.GetChatClientAsync()` | Get a native chat client (no OpenAI SDK needed) |
| `model.GetAudioClientAsync()` | Get an audio client for transcription |

</details>

---

### Exercise 4: Inspect Model Metadata

The `FoundryModelInfo` object contains rich metadata about each model. Understanding these fields helps you choose the right model for your application.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Get detailed info about a specific model
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

const manager = new FoundryLocalManager();
await manager.startService();

const info = await manager.getModelInfo("phi-3.5-mini");
if (info) {
  console.log(`Alias:              ${info.alias}`);
  console.log(`Model ID:           ${info.id}`);
  console.log(`Version:            ${info.version}`);
  console.log(`Task:               ${info.task}`);
  console.log(`Device Type:        ${info.deviceType}`);
  console.log(`Execution Provider: ${info.executionProvider}`);
  console.log(`File Size (MB):     ${info.fileSizeMb}`);
  console.log(`Publisher:          ${info.publisher}`);
  console.log(`License:            ${info.license}`);
  console.log(`Tool Calling:       ${info.supportsToolCalling}`);
}
```

</details>

#### FoundryModelInfo Fields

| Field | Type | Description |
|-------|------|-------------|
| `alias` | string | Short name (e.g. `phi-3.5-mini`) |
| `id` | string | Unique model identifier |
| `version` | string | Model version |
| `task` | string | `chat-completions` or `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, or NPU |
| `execution_provider` | string | Runtime backend (CUDA, CPU, QNN, WebGPU, etc.) |
| `file_size_mb` | int | Size on disk in MB |
| `supports_tool_calling` | bool | Whether the model supports function/tool calling |
| `publisher` | string | Who published the model |
| `license` | string | License name |
| `uri` | string | Model URI |
| `prompt_template` | dict/null | Prompt template, if any |

---

### Exercise 5: Manage Model Lifecycle

Practice the full lifecycle: list → download → load → use → unload.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Small model for quick testing

manager = FoundryLocalManager()
manager.start_service()

# 1. Check what is in the catalog
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Check what is already downloaded
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Download a model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Verify it is in the cache now
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Load it
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Check what is loaded
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Unload it
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

const alias = "qwen2.5-0.5b"; // Small model for quick testing

const manager = new FoundryLocalManager();
await manager.startService();

// 1. Check catalog
const catalog = await manager.listCatalogModels();
console.log(`Catalog: ${catalog.length} models`);

// 2. Check cache
let cached = await manager.listCachedModels();
console.log(`Cached: ${cached.length} models`);

// 3. Download
console.log(`\nDownloading ${alias}...`);
await manager.downloadModel(alias, undefined, false, (progress) => {
  process.stdout.write(`\r  Progress: ${progress.toFixed(1)}%`);
  if (progress >= 100) process.stdout.write("\n");
});

// 4. Verify cache
cached = await manager.listCachedModels();
console.log(`Cached after download: ${cached.length} models`);

// 5. Load
console.log(`\nLoading ${alias}...`);
const modelInfo = await manager.loadModel(alias);
console.log(`Loaded: ${modelInfo.id}`);

// 6. Check loaded
let loaded = await manager.listLoadedModels();
console.log(`\nLoaded models: ${loaded.length}`);

// 7. Unload
console.log(`\nUnloading ${alias}...`);
await manager.unloadModel(alias);
loaded = await manager.listLoadedModels();
console.log(`Loaded after unload: ${loaded.length}`);
```

</details>

---

### Exercise 6: The Quick-Start Patterns

Each language provides a shortcut to start the service and load a model in one call. These are the **recommended patterns** for most applications.

<details>
<summary><h3>🐍 Python - Constructor Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Pass an alias to the constructor - it handles everything:
# 1. Starts the service if not running
# 2. Downloads the model if not cached
# 3. Loads the model into the inference server
manager = FoundryLocalManager("phi-3.5-mini")

# Ready to use immediately
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

The `bootstrap` parameter (default `True`) controls this behaviour. Set `bootstrap=False` if you want manual control:

```python
# Manual mode - nothing happens automatically
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `init()` Method</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const manager = new FoundryLocalManager();

// init() handles everything:
// 1. Starts the service if not running
// 2. Downloads the model if not cached
// 3. Loads the model into the inference server
const modelInfo = await manager.init("phi-3.5-mini");

// Ready to use immediately
console.log(`Endpoint: ${manager.endpoint}`);
console.log(`Model ID: ${modelInfo.id}`);
```

You can also call `init()` without a model to just start the service:

```javascript
const manager = new FoundryLocalManager();
await manager.init(); // Starts service only, no model loaded
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

> **C# Note:** The C# SDK uses `Configuration` to control app name, logging, cache directories, and even pin a specific web server port. This makes it the most configurable of the three SDKs.

</details>

---

### Exercise 7: Understanding Aliases and Hardware Selection

When you pass an **alias** (like `phi-3.5-mini`) instead of a full model ID, the SDK automatically selects the best variant for your hardware:

| Hardware | Selected Execution Provider |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Any device (fallback) | `CPUExecutionProvider` or `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# The alias resolves to the best variant for YOUR hardware
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tip:** Always use aliases in your application code. When you deploy to a user's machine, the SDK picks the optimal variant at runtime - CUDA on NVIDIA, QNN on Qualcomm, CPU elsewhere.

---

### Exercise 8: C# Configuration Options

The C# SDK's `Configuration` class provides fine-grained control over the runtime:

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
| `AppName` | (required) | Your application's name |
| `LogLevel` | `Information` | Logging verbosity |
| `Web.Urls` | (dynamic) | Pin a specific port for the web server |
| `AppDataDir` | OS default | Base directory for app data |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Where models are stored |
| `LogsDir` | `{AppDataDir}/logs` | Where logs are written |

---

### Exercise 9: Browser Usage (JavaScript Only)

The JavaScript SDK includes a browser-compatible version. In the browser, you must manually start the service via CLI and specify the host URL:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Start the service manually first:
//   foundry service start
// Then use the URL from the CLI output
const host = "http://localhost:5273";

const manager = new FoundryLocalManager({ host });

// These methods work in the browser:
const catalog = await manager.listCatalogModels();
await manager.downloadModel("phi-3.5-mini");
await manager.loadModel("phi-3.5-mini");

const cached = await manager.listCachedModels();
const loaded = await manager.listLoadedModels();
```

> **Browser limitations:** The browser version does **not** support `init()`, `isServiceRunning()`, or `startService()`. You must ensure the Foundry Local service is already running before using the SDK in a browser.

---

## Complete API Reference

### Python

| Category | Method | Description |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Create manager; optionally bootstrap with a model |
| **Service** | `is_service_running()` | Check if service is running |
| **Service** | `start_service()` | Start the service |
| **Service** | `endpoint` | API endpoint URL |
| **Service** | `api_key` | API key |
| **Catalog** | `list_catalog_models()` | List all available models |
| **Catalog** | `refresh_catalog()` | Refresh the catalog |
| **Catalog** | `get_model_info(alias_or_model_id)` | Get model metadata |
| **Cache** | `get_cache_location()` | Cache directory path |
| **Cache** | `list_cached_models()` | List downloaded models |
| **Model** | `download_model(alias_or_model_id)` | Download a model |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Load a model |
| **Model** | `unload_model(alias_or_model_id)` | Unload a model |
| **Model** | `list_loaded_models()` | List loaded models |

### JavaScript

| Category | Method | Description |
|----------|--------|-------------|
| **Init** | `new FoundryLocalManager(options?)` | Create manager |
| **Init** | `init(aliasOrModelId?)` | Start service + optionally load a model |
| **Service** | `isServiceRunning()` | Check if service is running |
| **Service** | `startService()` | Start the service |
| **Service** | `endpoint` | API endpoint URL |
| **Service** | `apiKey` | API key |
| **Catalog** | `listCatalogModels()` | List all available models |
| **Catalog** | `refreshCatalog()` | Refresh the catalog |
| **Catalog** | `getModelInfo(aliasOrModelId)` | Get model metadata |
| **Cache** | `getCacheLocation()` | Cache directory path |
| **Cache** | `listCachedModels()` | List downloaded models |
| **Model** | `downloadModel(aliasOrModelId, ...)` | Download with progress callback |
| **Model** | `loadModel(aliasOrModelId, ttl?)` | Load a model |
| **Model** | `unloadModel(aliasOrModelId)` | Unload a model |
| **Model** | `listLoadedModels()` | List loaded models |

### C# (v0.8.0+)

| Category | Method | Description |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Initialize the manager |
| **Init** | `FoundryLocalManager.Instance` | Access the singleton |
| **Catalog** | `manager.GetCatalogAsync()` | Get catalog |
| **Catalog** | `catalog.ListModelsAsync()` | List all models |
| **Catalog** | `catalog.GetModelAsync(alias)` | Get a specific model |
| **Catalog** | `catalog.GetCachedModelsAsync()` | List cached models |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | List loaded models |
| **Model** | `model.DownloadAsync(progress?)` | Download a model |
| **Model** | `model.LoadAsync()` | Load a model |
| **Model** | `model.UnloadAsync()` | Unload a model |
| **Model** | `model.SelectVariant(variant)` | Choose a hardware variant |
| **Model** | `model.GetChatClientAsync()` | Get native chat client |
| **Model** | `model.GetAudioClientAsync()` | Get audio transcription client |
| **Model** | `model.GetPathAsync()` | Get local file path |
| **Server** | `manager.StartWebServerAsync()` | Start the REST web server |
| **Server** | `manager.StopWebServerAsync()` | Stop the REST web server |
| **Config** | `config.ModelCacheDir` | Cache directory |

---

## Key Takeaways

| Concept | What You Learned |
|---------|-----------------|
| **SDK vs CLI** | The SDK provides programmatic control - essential for applications |
| **Control plane** | The SDK manages services, models, and caching |
| **Dynamic ports** | Always use `manager.endpoint` - never hardcode a port |
| **Aliases** | Use aliases for automatic hardware-optimal model selection |
| **Quick-start** | Python: `FoundryLocalManager(alias)`, JS: `manager.init(alias)` |
| **C# redesign** | v0.8.0+ is self-contained - no CLI needed on end-user machines |
| **Model lifecycle** | Catalog → Download → Load → Use → Unload |
| **FoundryModelInfo** | Rich metadata: task, device, size, license, tool calling support |

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
