![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第二部分：Foundry Local SDK 深入解析

> **目标：** 精通 Foundry Local SDK，以编程方式管理模型、服务和缓存——并理解为什么 SDK 比 CLI 更适合构建应用程序。

## 概述

在第一部分中，您使用了 **Foundry Local CLI** 来下载和交互式运行模型。CLI 非常适合探索，但在构建真实应用程序时，您需要 <strong>编程控制</strong>。Foundry Local SDK 能满足这一需求——它管理 <strong>控制平面</strong>（启动服务、发现模型、下载、加载），让您的应用代码专注于 <strong>数据平面</strong>（发送提示、接收完成结果）。

本实验将教授您 Python、JavaScript 和 C# 中的完整 SDK API。完成后，您将理解所有可用方法及其适用场景。

## 学习目标

完成本实验后，您将能够：

- 说明为何应用开发中推荐使用 SDK 而非 CLI
- 安装适用于 Python、JavaScript 或 C# 的 Foundry Local SDK
- 使用 `FoundryLocalManager` 启动服务、管理模型、查询目录
- 编程方式列出、下载、加载和卸载模型
- 使用 `FoundryModelInfo` 检查模型元数据
- 理解目录、缓存和加载模型的区别
- 使用构造函数引导（Python）和 `create()` + 目录模式（JavaScript）
- 理解 C# SDK 的重设计及其面向对象 API

---

## 前提条件

| 要求 | 详情 |
|-------------|---------|
| **Foundry Local CLI** | 已安装并配置在您的 `PATH` 中 ([第1部分](part1-getting-started.md)) |
| <strong>语言运行时</strong> | **Python 3.9+** 和/或 **Node.js 18+** 和/或 **.NET 9.0+** |

---

## 概念：SDK vs CLI - 为什么使用 SDK？

| 方面 | CLI (`foundry` 命令) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| <strong>适用场景</strong> | 探索、手动测试 | 应用集成 |
| <strong>服务管理</strong> | 手动：`foundry service start` | 自动：`manager.start_service()`（Python）/ `manager.startWebService()`（JS/C#） |
| <strong>端口发现</strong> | 从 CLI 输出中读取 | `manager.endpoint`（Python）/ `manager.urls[0]`（JS/C#） |
| <strong>模型下载</strong> | `foundry model download alias` | `manager.download_model(alias)`（Python）/ `model.download()`（JS/C#） |
| <strong>错误处理</strong> | 退出码、标准错误输出 | 异常、类型化错误 |
| <strong>自动化</strong> | Shell 脚本 | 原生语言集成 |
| <strong>部署</strong> | 需要在终端用户机器上安装 CLI | C# SDK 可自包含（无需 CLI） |

> **关键见解：** SDK 使用少量代码即可处理整个生命周期：启动服务、检查缓存、下载缺失模型、加载模型、发现端点。您的应用无需解析 CLI 输出或管理子进程。

---

## 实验练习

### 练习 1：安装 SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

验证安装：

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

验证安装：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

有两个 NuGet 包：

| 包 | 平台 | 说明 |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | 跨平台 | 支持 Windows、Linux、macOS |
| `Microsoft.AI.Foundry.Local.WinML` | 仅限 Windows | 增加 WinML 硬件加速；下载并安装插件执行提供程序（如 Qualcomm NPU 的 QNN） |

**Windows 设置：**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

编辑 `.csproj` 文件：

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

> **注意：** 在 Windows 上，WinML 包是基础 SDK 加上 QNN 执行提供程序的超集。在 Linux/macOS 上使用基础 SDK。条件 TFM 和包引用保持项目完全跨平台。

在项目根目录创建 `nuget.config`：

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

还原包：

```bash
dotnet restore
```

</details>

---

### 练习 2：启动服务并列出目录

任何应用的第一步是启动 Foundry Local 服务并发现可用模型。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# 创建一个管理器并启动服务
manager = FoundryLocalManager()
manager.start_service()

# 列出目录中所有可用的模型
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - 服务管理方法

| 方法 | 签名 | 说明 |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | 检查服务是否正在运行 |
| `start_service()` | `() -> None` | 启动 Foundry Local 服务 |
| `service_uri` | `@property -> str` | 服务基础 URI |
| `endpoint` | `@property -> str` | API 端点（服务 URI + `/v1`） |
| `api_key` | `@property -> str` | API 密钥（来自环境变量或默认占位符） |

#### Python SDK - 目录管理方法

| 方法 | 签名 | 说明 |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | 列出目录中所有模型 |
| `refresh_catalog()` | `() -> None` | 从服务刷新目录 |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | 获取特定模型信息 |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// 创建一个管理器并启动服务
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 浏览目录
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Manager 方法

| 方法 | 签名 | 说明 |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | 初始化 SDK 单例 |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | 访问单例管理器 |
| `manager.startWebService()` | `() => Promise<void>` | 启动 Foundry Local Web 服务 |
| `manager.urls` | `string[]` | 服务基础 URL 数组 |

#### JavaScript SDK - 目录和模型方法

| 方法 | 签名 | 说明 |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | 访问模型目录 |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | 按别名获取模型对象 |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ 使用面向对象架构，包含 `Configuration`、`Catalog` 和 `Model` 对象：

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

#### C# SDK - 关键类

| 类 | 作用 |
|-------|---------|
| `Configuration` | 设置应用名、日志级别、缓存目录、Web 服务器 URL |
| `FoundryLocalManager` | 主要入口——通过 `CreateAsync()` 创建，通过 `.Instance` 访问 |
| `Catalog` | 浏览、搜索并获取目录中的模型 |
| `Model` | 表示具体模型——下载、加载、获取客户端 |

#### C# SDK - Manager 和 Catalog 方法

| 方法 | 说明 |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | 初始化管理器 |
| `FoundryLocalManager.Instance` | 访问单例管理器 |
| `manager.GetCatalogAsync()` | 获取模型目录 |
| `catalog.ListModelsAsync()` | 列出所有可用模型 |
| `catalog.GetModelAsync(alias: "alias")` | 按别名获取特定模型 |
| `catalog.GetCachedModelsAsync()` | 列出已下载模型 |
| `catalog.GetLoadedModelsAsync()` | 列出当前已加载模型 |

> **C# 架构说明：** C# SDK v0.8.0+ 的重设计使应用<strong>自包含</strong>；它不依赖于终端用户机器上的 Foundry Local CLI。SDK 原生处理模型管理和推理。

</details>

---

### 练习 3：下载并加载模型

SDK 将下载（保存到磁盘）和加载（加载到内存）分开，这允许您在设置阶段预先下载模型，按需加载。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# 选项A：手动逐步操作
manager = FoundryLocalManager()
manager.start_service()

# 首先检查缓存
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

# 选项B：一行命令引导（推荐）
# 将别名传递给构造函数 - 它会自动启动服务、下载并加载
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - 模型管理方法

| 方法 | 签名 | 说明 |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | 下载模型到本地缓存 |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | 将模型加载至推理服务器 |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | 从服务器卸载模型 |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | 列出当前所有已加载模型 |

#### Python - 缓存管理方法

| 方法 | 签名 | 说明 |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | 获取缓存目录路径 |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | 列出所有已下载模型 |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// 逐步方法
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

#### JavaScript - 模型方法

| 方法 | 签名 | 说明 |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | 模型是否已下载 |
| `model.download()` | `() => Promise<void>` | 下载模型到本地缓存 |
| `model.load()` | `() => Promise<void>` | 加载到推理服务器 |
| `model.unload()` | `() => Promise<void>` | 从推理服务器卸载 |
| `model.id` | `string` | 模型唯一标识符 |

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

#### C# - 模型方法

| 方法 | 说明 |
|--------|-------------|
| `model.DownloadAsync(progress?)` | 下载选定变体 |
| `model.LoadAsync()` | 加载模型到内存 |
| `model.UnloadAsync()` | 卸载模型 |
| `model.SelectVariant(variant)` | 选择特定变体（CPU/GPU/NPU） |
| `model.SelectedVariant` | 当前选定的变体 |
| `model.Variants` | 此模型所有可用变体 |
| `model.GetPathAsync()` | 获取本地文件路径 |
| `model.GetChatClientAsync()` | 获取本地聊天客户端（无需 OpenAI SDK） |
| `model.GetAudioClientAsync()` | 获取音频客户端，支持转录 |

</details>

---

### 练习 4：检查模型元数据

`FoundryModelInfo` 对象包含丰富的模型元数据。理解这些字段有助于您为应用选择合适的模型。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# 获取特定模型的详细信息
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

#### FoundryModelInfo 字段

| 字段 | 类型 | 说明 |
|-------|------|-------------|
| `alias` | string | 简短名称（如 `phi-3.5-mini`） |
| `id` | string | 模型唯一标识符 |
| `version` | string | 模型版本 |
| `task` | string | `chat-completions` 或 `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU、GPU 或 NPU |
| `execution_provider` | string | 运行时后端（CUDA、CPU、QNN、WebGPU 等） |
| `file_size_mb` | int | 磁盘占用大小，单位 MB |
| `supports_tool_calling` | bool | 是否支持函数/工具调用 |
| `publisher` | string | 发布者 |
| `license` | string | 许可证名称 |
| `uri` | string | 模型 URI |
| `prompt_template` | dict/null | 提示模板（如果有） |

---

### 练习 5：管理模型生命周期

练习完整生命周期：列出 → 下载 → 加载 → 使用 → 卸载。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # 用于快速测试的小模型

manager = FoundryLocalManager()
manager.start_service()

# 1. 检查目录中的内容
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. 检查已下载的内容
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. 下载一个模型
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. 验证它现在是否在缓存中
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. 加载它
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. 检查已加载的内容
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. 卸载它
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

const alias = "qwen2.5-0.5b"; // 小模型用于快速测试

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. 从目录中获取模型
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. 如有需要，下载模型
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. 加载模型
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. 卸载模型
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### 练习 6：快速启动模式

每种语言都提供了一种快捷方式，可以在一次调用中启动服务并加载模型。这些是大多数应用程序的<strong>推荐模式</strong>。

<details>
<summary><h3>🐍 Python - 构造函数引导</h3></summary>

```python
from foundry_local import FoundryLocalManager

# 传递一个别名给构造函数 - 它处理所有事情：
# 1. 如果服务未运行，则启动服务
# 2. 如果模型未缓存，则下载模型
# 3. 将模型加载到推理服务器中
manager = FoundryLocalManager("phi-3.5-mini")

# 立即可用
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` 参数（默认值 `True`）控制此行为。如果您想手动控制，请设置 `bootstrap=False`：

```python
# 手动模式 - 不会自动发生任何事情
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + 目录</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() 处理所有事情：
// 1. 启动服务
// 2. 从目录中获取模型
// 3. 如有需要，下载并加载模型
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// 即刻可用
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + 目录</h3></summary>

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

> **C# 注意：** C# SDK 使用 `Configuration` 来控制应用名称、日志记录、缓存目录，甚至固定特定的 Web 服务器端口。这使其成为三种 SDK 中配置最灵活的。

</details>

---

### 练习 7：原生 ChatClient（无需 OpenAI SDK）

JavaScript 和 C# SDK 提供了 `createChatClient()` 方便方法，返回原生聊天客户端 — 无需单独安装或配置 OpenAI SDK。

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

// 直接从模型创建 ChatClient — 无需导入 OpenAI
const chatClient = model.createChatClient();

// completeChat 返回一个兼容 OpenAI 的响应对象
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// 流式传输使用回调模式
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` 还支持工具调用 — 将工具作为第二个参数传入：

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

> **何时使用哪种模式：**
> - **`createChatClient()`** — 快速原型开发，依赖更少，代码更简单
> - **OpenAI SDK** — 对参数（温度，top_p，停止标记等）有完全控制，更适用于生产环境

---

### 练习 8：模型变体和硬件选择

模型可以有多个针对不同硬件优化的<strong>变体</strong>。SDK 会自动选择最佳变体，但你也可以查看并手动选择。

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// 列出所有可用的变体
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK 会自动为您的硬件选择最佳变体
// 如需覆盖，请使用 selectVariant():
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

在 Python 中，SDK 会基于硬件自动选择最佳变体。使用 `get_model_info()` 来查看被选中了哪个：

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

#### 具有 NPU 变体的模型

一些模型有针对带有神经处理单元（Qualcomm Snapdragon，Intel Core Ultra）设备的 NPU 优化变体：

| 模型 | 是否有 NPU 变体 |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **提示：** 在支持 NPU 的硬件上，SDK 会自动选择 NPU 变体，无需更改代码。对于 Windows 上的 C# 项目，请添加 `Microsoft.AI.Foundry.Local.WinML` NuGet 包以启用 QNN 执行提供程序 — QNN 通过 WinML 作为插件 EP 提供。

---

### 练习 9：模型升级与目录刷新

模型目录会定期更新。使用以下方法检查并应用更新。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# 刷新目录以获取最新的模型列表
manager.refresh_catalog()

# 检查缓存的模型是否有可用的新版本
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

// 刷新目录以获取最新模型列表
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// 刷新后列出所有可用模型
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### 练习 10：使用推理模型

**phi-4-mini-reasoning** 模型包含链式思维推理。在生成最终答案之前，会将内部思考包裹在 `<think>...</think>` 标签中。这对于需要多步逻辑、数学或问题解决的任务非常有用。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning 大约是 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# 该模型将其思考内容包裹在 <think>...</think> 标签中
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

// 解析链式思维
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **何时使用推理模型：**
> - 数学和逻辑问题
> - 多步规划任务
> - 复杂代码生成
> - 需要展示推理过程以提高准确性的任务
>
> **权衡：** 推理模型产生更多的 token（即 `<think>` 部分），且速度较慢。对于简单问答，标准模型如 phi-3.5-mini 更快。

---

### 练习 11：理解别名与硬件选择

当你传入一个<strong>别名</strong>（如 `phi-3.5-mini`）而不是完整的模型 ID，SDK 会自动为你的硬件选择最佳变体：

| 硬件 | 选择的执行提供程序 |
|----------|---------------------------|
| NVIDIA GPU（CUDA） | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider`（通过 WinML 插件） |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| 任意设备（后备） | `CPUExecutionProvider` 或 `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# 此别名解析为适合您硬件的最佳变体
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **提示：** 在你的应用代码中始终使用别名。当部署到用户机器时，SDK 会在运行时选择最佳变体——NVIDIA 上使用 CUDA，Qualcomm 上使用 QNN，其他则使用 CPU。

---

### 练习 12：C# 配置选项

C# SDK 的 `Configuration` 类提供了细粒度运行时控制：

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

| 属性 | 默认值 | 描述 |
|----------|---------|-------------|
| `AppName` | （必填） | 你的应用名称 |
| `LogLevel` | `Information` | 日志详细级别 |
| `Web.Urls` | （动态） | 固定 Web 服务器端口 |
| `AppDataDir` | 操作系统默认 | 应用数据的基础目录 |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | 模型存储位置 |
| `LogsDir` | `{AppDataDir}/logs` | 日志文件存放位置 |

---

### 练习 13：浏览器使用（仅限 JavaScript）

JavaScript SDK 包含浏览器兼容版本。在浏览器中，你必须通过 CLI 手动启动服务，并指定主机 URL：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// 首先手动启动服务：
//   foundry service start
// 然后使用 CLI 输出中的 URL
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// 浏览目录
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **浏览器限制：** 浏览器版本<strong>不支持</strong> `startWebService()`。你必须确保 Foundry Local 服务已经运行，然后才能在浏览器中使用 SDK。

---

## 完整 API 参考

### Python

| 分类 | 方法 | 描述 |
|----------|--------|-------------|
| <strong>初始化</strong> | `FoundryLocalManager(alias?, bootstrap=True)` | 创建管理器；可选择引导加载模型 |
| <strong>服务</strong> | `is_service_running()` | 检查服务是否运行 |
| <strong>服务</strong> | `start_service()` | 启动服务 |
| <strong>服务</strong> | `endpoint` | API 端点 URL |
| <strong>服务</strong> | `api_key` | API 密钥 |
| <strong>目录</strong> | `list_catalog_models()` | 列出所有可用模型 |
| <strong>目录</strong> | `refresh_catalog()` | 刷新目录 |
| <strong>目录</strong> | `get_model_info(alias_or_model_id)` | 获取模型元数据 |
| <strong>缓存</strong> | `get_cache_location()` | 缓存目录路径 |
| <strong>缓存</strong> | `list_cached_models()` | 列出已下载模型 |
| <strong>模型</strong> | `download_model(alias_or_model_id)` | 下载模型 |
| <strong>模型</strong> | `load_model(alias_or_model_id, ttl=600)` | 加载模型 |
| <strong>模型</strong> | `unload_model(alias_or_model_id)` | 卸载模型 |
| <strong>模型</strong> | `list_loaded_models()` | 列出已加载模型 |
| <strong>模型</strong> | `is_model_upgradeable(alias_or_model_id)` | 检查是否有可升级版本 |
| <strong>模型</strong> | `upgrade_model(alias_or_model_id)` | 升级模型至最新版本 |
| <strong>服务</strong> | `httpx_client` | 预配置的 HTTPX 客户端，用于直接 API 调用 |

### JavaScript

| 分类 | 方法 | 描述 |
|----------|--------|-------------|
| <strong>初始化</strong> | `FoundryLocalManager.create(options)` | 初始化 SDK 单例 |
| <strong>初始化</strong> | `FoundryLocalManager.instance` | 访问单例管理器 |
| <strong>服务</strong> | `manager.startWebService()` | 启动 Web 服务 |
| <strong>服务</strong> | `manager.urls` | 服务的基础 URL 数组 |
| <strong>目录</strong> | `manager.catalog` | 访问模型目录 |
| <strong>目录</strong> | `catalog.getModel(alias)` | 通过别名获取模型对象（返回 Promise） |
| <strong>模型</strong> | `model.isCached` | 模型是否已下载 |
| <strong>模型</strong> | `model.download()` | 下载模型 |
| <strong>模型</strong> | `model.load()` | 加载模型 |
| <strong>模型</strong> | `model.unload()` | 卸载模型 |
| <strong>模型</strong> | `model.id` | 模型唯一标识符 |
| <strong>模型</strong> | `model.alias` | 模型别名 |
| <strong>模型</strong> | `model.createChatClient()` | 获取原生聊天客户端（无需 OpenAI SDK） |
| <strong>模型</strong> | `model.createAudioClient()` | 获取音频转录客户端 |
| <strong>模型</strong> | `model.removeFromCache()` | 从本地缓存移除模型 |
| <strong>模型</strong> | `model.selectVariant(variant)` | 选择特定硬件变体 |
| <strong>模型</strong> | `model.variants` | 此模型的可用变体数组 |
| <strong>模型</strong> | `model.isLoaded()` | 检查模型是否已加载 |
| <strong>目录</strong> | `catalog.getModels()` | 列出所有可用模型 |
| <strong>目录</strong> | `catalog.getCachedModels()` | 列出已下载模型 |
| <strong>目录</strong> | `catalog.getLoadedModels()` | 列出已加载模型 |
| <strong>目录</strong> | `catalog.updateModels()` | 从服务刷新目录 |
| <strong>服务</strong> | `manager.stopWebService()` | 停止 Foundry Local Web 服务 |

### C# (v0.8.0+)

| 分类 | 方法 | 描述 |
|----------|--------|-------------|
| <strong>初始化</strong> | `FoundryLocalManager.CreateAsync(config, logger)` | 初始化管理器 |
| <strong>初始化</strong> | `FoundryLocalManager.Instance` | 访问单例 |
| <strong>目录</strong> | `manager.GetCatalogAsync()` | 获取目录 |
| <strong>目录</strong> | `catalog.ListModelsAsync()` | 列出所有模型 |
| <strong>目录</strong> | `catalog.GetModelAsync(alias)` | 获取特定模型 |
| <strong>目录</strong> | `catalog.GetCachedModelsAsync()` | 列出缓存模型 |
| <strong>目录</strong> | `catalog.GetLoadedModelsAsync()` | 列出已加载模型 |
| <strong>模型</strong> | `model.DownloadAsync(progress?)` | 下载模型 |
| <strong>模型</strong> | `model.LoadAsync()` | 加载模型 |
| <strong>模型</strong> | `model.UnloadAsync()` | 卸载模型 |
| <strong>模型</strong> | `model.SelectVariant(variant)` | 选择硬件变体 |
| <strong>模型</strong> | `model.GetChatClientAsync()` | 获取原生聊天客户端 |
| <strong>模型</strong> | `model.GetAudioClientAsync()` | 获取音频转录客户端 |
| <strong>模型</strong> | `model.GetPathAsync()` | 获取本地文件路径 |
| <strong>目录</strong> | `catalog.GetModelVariantAsync(alias, variant)` | 获取特定硬件变体 |
| <strong>目录</strong> | `catalog.UpdateModelsAsync()` | 刷新目录 |
| <strong>服务</strong> | `manager.StartWebServerAsync()` | 启动 REST Web 服务器 |
| <strong>服务</strong> | `manager.StopWebServerAsync()` | 停止 REST Web 服务器 |
| <strong>配置</strong> | `config.ModelCacheDir` | 缓存目录 |

---

## 关键要点

| 概念 | 你学到了什么 |
|---------|-----------------|
| **SDK 与 CLI** | SDK 提供程序化控制——应用程序不可或缺 |
| <strong>控制平面</strong> | SDK 管理服务、模型和缓存 |
| <strong>动态端口</strong> | 始终使用 `manager.endpoint`（Python）或 `manager.urls[0]`（JS/C#），绝不硬编码端口 |
| <strong>别名</strong> | 使用别名以自动选择硬件最优的模型 |
| <strong>快速开始</strong> | Python: `FoundryLocalManager(alias)`，JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# 重设计** | v0.8.0+ 是自包含的 - 终端用户机器无需 CLI |
| <strong>模型生命周期</strong> | Catalog → 下载 → 加载 → 使用 → 卸载 |
| **FoundryModelInfo** | 丰富的元数据：任务、设备、大小、许可、工具调用支持 |
| **ChatClient** | `createChatClient()`（JS）/ `GetChatClientAsync()`（C#）用于无 OpenAI 方式使用 |
| <strong>变体</strong> | 模型有硬件特定变体（CPU、GPU、NPU）；自动选择 |
| <strong>升级</strong> | Python: `is_model_upgradeable()` + `upgrade_model()` 用于保持模型更新 |
| <strong>目录刷新</strong> | `refresh_catalog()`（Python）/ `updateModels()`（JS） 用于发现新模型 |

---

## 资源

| 资源 | 链接 |
|----------|------|
| SDK 参考（所有语言） | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| 与推理 SDK 集成 | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API 参考 | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK 示例 | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local 网站 | [foundrylocal.ai](https://foundrylocal.ai) |

---

## 下一步

继续阅读[第3部分：使用带 OpenAI 的 SDK](part3-sdk-and-apis.md)，将 SDK 连接到 OpenAI 客户端库，构建您的第一个聊天完成应用。