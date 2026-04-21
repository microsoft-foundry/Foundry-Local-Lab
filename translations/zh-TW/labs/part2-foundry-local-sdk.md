![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 第 2 部分：Foundry Local SDK 深入解析

> **目標：** 精通 Foundry Local SDK，透過程式化方式管理模型、服務與快取，並理解為何 SDK 是建立應用程式時比 CLI 更推薦的方案。

## 概覽

在第 1 部分，你使用了 **Foundry Local CLI** 來下載及互動式執行模型。CLI 適合探索，但當你建立真正的應用程式時，需具備 <strong>程式化控制能力</strong>。Foundry Local SDK 提供了這種能力——它管理 <strong>控制平面</strong>（啟動服務、尋找模型、下載、載入），讓你的應用程式程式碼能專注在 <strong>資料平面</strong>（傳送提示詞、接收完成結果）。

本實驗將教你 Python、JavaScript 與 C# 等語言的完整 SDK API。結束時你將了解每個方法的用途與使用時機。

## 學習目標

本實驗結束後，你將能：

- 解釋為何應用程式開發推薦使用 SDK 而非 CLI
- 安裝 Foundry Local SDK（Python、JavaScript 或 C#）
- 利用 `FoundryLocalManager` 啟動服務、管理模型、查詢目錄
- 程式化列出、下載、載入與卸載模型
- 使用 `FoundryModelInfo` 檢視模型元資料
- 理解目錄、快取與載入模型的差異
- 使用建構子引導（Python）與 `create()` + 目錄模式（JavaScript）
- 理解 C# SDK 的重新設計及其物件導向 API

---

## 先決條件

| 需求 | 詳細 |
|-------------|---------|
| **Foundry Local CLI** | 已安裝且在你的 `PATH` 中 ([第 1 部分](part1-getting-started.md)) |
| <strong>語言執行環境</strong> | **Python 3.9+** 和／或 **Node.js 18+** 和／或 **.NET 9.0+** |

---

## 概念：SDK 與 CLI 比較－為何使用 SDK？

| 方面 | CLI（`foundry` 指令） | SDK（`foundry-local-sdk`） |
|--------|------------------------|--------------------------|
| <strong>使用情境</strong> | 探索、手動測試 | 應用整合 |
| <strong>服務管理</strong> | 手動：`foundry service start` | 自動：`manager.start_service()`（Python） / `manager.startWebService()`（JS/C#） |
| <strong>埠號偵測</strong> | 從 CLI 輸出讀取 | `manager.endpoint`（Python） / `manager.urls[0]`（JS/C#） |
| <strong>模型下載</strong> | `foundry model download alias` | `manager.download_model(alias)`（Python） / `model.download()`（JS/C#） |
| <strong>錯誤處理</strong> | 退出碼、標準錯誤輸出 | 例外、型別化錯誤 |
| <strong>自動化</strong> | Shell 腳本 | 原生語言整合 |
| <strong>部署</strong> | 需終端使用者安裝 CLI | C# SDK 可做到自包含（無需 CLI） |

> **關鍵洞察：** SDK 在幾行程式碼內即能完整處理生命週期：啟動服務、檢查快取、下載缺少模型、載入，並取得端點。你的應用程式無需解析 CLI 輸出或管理子程序。

---

## 實驗練習

### 練習 1：安裝 SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

驗證安裝：

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

驗證安裝：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

有兩個 NuGet 套件：

| 套件 | 平台 | 說明 |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | 跨平台 | 支援 Windows、Linux、macOS |
| `Microsoft.AI.Foundry.Local.WinML` | 僅 Windows | 增加 WinML 硬體加速；會下載並安裝外掛執行提供者（例如 Qualcomm NPU 的 QNN） |

**Windows 設定：**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

編輯 `.csproj` 檔案：

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

> **注意：** 在 Windows 上，WinML 套件是包含基本 SDK 加上 QNN 執行提供者的超集。Linux/macOS 則使用基本 SDK。條件式 TFM 和套件參考讓專案維持完全跨平台。

在專案根目錄建立 `nuget.config`：

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

還原套件：

```bash
dotnet restore
```

</details>

---

### 練習 2：啟動服務並列出目錄

任何應用程式第一步都是啟動 Foundry Local 服務並得知有哪些模型可用。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# 建立一個管理器並啟動服務
manager = FoundryLocalManager()
manager.start_service()

# 列出目錄中所有可用的模型
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - 服務管理方法

| 方法 | 簽章 | 說明 |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | 檢查服務是否正在運行 |
| `start_service()` | `() -> None` | 啟動 Foundry Local 服務 |
| `service_uri` | `@property -> str` | 服務基本 URI |
| `endpoint` | `@property -> str` | API 端點（服務 URI + `/v1`） |
| `api_key` | `@property -> str` | API 金鑰（從環境變數或預設佔位） |

#### Python SDK - 目錄管理方法

| 方法 | 簽章 | 說明 |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | 列出目錄中所有模型 |
| `refresh_catalog()` | `() -> None` | 從服務刷新目錄 |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | 獲取特定模型資訊 |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// 建立一個管理員並啟動服務
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 瀏覽目錄
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - 管理器方法

| 方法 | 簽章 | 說明 |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | 初始化 SDK 唯一實例 |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | 存取唯一管理器實例 |
| `manager.startWebService()` | `() => Promise<void>` | 啟動 Foundry Local 網路服務 |
| `manager.urls` | `string[]` | 服務基本 URL 陣列 |

#### JavaScript SDK - 目錄與模型方法

| 方法 | 簽章 | 說明 |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | 存取模型目錄 |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | 依別名取得模型物件 |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ 採用物件導向架構，包含 `Configuration`、`Catalog` 與 `Model` 物件：

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

#### C# SDK - 主要類別

| 類別 | 用途 |
|-------|---------|
| `Configuration` | 設定應用程式名稱、日誌等級、快取目錄、Web 伺服器 URL |
| `FoundryLocalManager` | 主要進入點－透過 `CreateAsync()` 建立，透過 `.Instance` 存取 |
| `Catalog` | 瀏覽、搜尋並從目錄取得模型 |
| `Model` | 代表特定模型－下載、載入、取得客戶端 |

#### C# SDK - 管理器與目錄方法

| 方法 | 說明 |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | 初始化管理器 |
| `FoundryLocalManager.Instance` | 存取單例管理器 |
| `manager.GetCatalogAsync()` | 取得模型目錄 |
| `catalog.ListModelsAsync()` | 列出所有可用模型 |
| `catalog.GetModelAsync(alias: "alias")` | 依別名取得特定模型 |
| `catalog.GetCachedModelsAsync()` | 列出已下載模型 |
| `catalog.GetLoadedModelsAsync()` | 列出目前載入模型 |

> **C# 架構說明：** C# SDK v0.8.0+ 重新設計讓應用程式成為 <strong>自包含</strong>，不需終端用戶的 Foundry Local CLI。SDK 原生管理模型與推論。

</details>

---

### 練習 3：下載並載入模型

SDK 將下載（至磁碟）與載入（進入記憶體）分開，允許預先下載模型並按需載入。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# 選項 A：手動逐步操作
manager = FoundryLocalManager()
manager.start_service()

# 先檢查快取
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

# 選項 B：一行啟動程式碼（建議使用）
# 傳遞別名到建構函式 - 它會自動啟動服務、下載並載入
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - 模型管理方法

| 方法 | 簽章 | 說明 |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | 將模型下載至本地快取 |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | 將模型載入推論伺服器 |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | 從伺服器卸載模型 |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | 列出所有目前載入模型 |

#### Python - 快取管理方法

| 方法 | 簽章 | 說明 |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | 取得快取目錄路徑 |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | 列出所有已下載模型 |

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

| 方法 | 簽章 | 說明 |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | 模型是否已下載 |
| `model.download()` | `() => Promise<void>` | 將模型下載至本地快取 |
| `model.load()` | `() => Promise<void>` | 載入推論伺服器 |
| `model.unload()` | `() => Promise<void>` | 從推論伺服器卸載 |
| `model.id` | `string` | 模型唯一標識符 |

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

| 方法 | 說明 |
|--------|-------------|
| `model.DownloadAsync(progress?)` | 下載所選變體 |
| `model.LoadAsync()` | 將模型載入記憶體 |
| `model.UnloadAsync()` | 卸載模型 |
| `model.SelectVariant(variant)` | 選擇特定變體（CPU/GPU/NPU） |
| `model.SelectedVariant` | 目前選擇的變體 |
| `model.Variants` | 該模型所有可用變體 |
| `model.GetPathAsync()` | 取得本地檔案路徑 |
| `model.GetChatClientAsync()` | 取得原生聊天客戶端（無需 OpenAI SDK） |
| `model.GetAudioClientAsync()` | 取得音訊客戶端以供轉錄 |

</details>

---

### 練習 4：檢視模型元資料

`FoundryModelInfo` 物件包含豐富的模型元資料。瞭解這些欄位，有助於為你的應用程式選擇合適模型。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# 獲取特定模型的詳細資訊
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

#### FoundryModelInfo 欄位

| 欄位 | 類型 | 說明 |
|-------|------|-------------|
| `alias` | string | 簡短名稱（例如 `phi-3.5-mini`） |
| `id` | string | 唯一模型識別碼 |
| `version` | string | 模型版本 |
| `task` | string | `chat-completions` 或 `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU、GPU 或 NPU |
| `execution_provider` | string | 執行時後端（CUDA、CPU、QNN、WebGPU 等） |
| `file_size_mb` | int | 磁碟大小（MB） |
| `supports_tool_calling` | bool | 是否支援函式／工具呼叫 |
| `publisher` | string | 發布者 |
| `license` | string | 授權名稱 |
| `uri` | string | 模型 URI |
| `prompt_template` | dict/null | 提示詞範本（若有） |

---

### 練習 5：管理模型生命週期

練習完整生命週期：列出 → 下載 → 載入 → 使用 → 卸載。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # 用於快速測試的小型模型

manager = FoundryLocalManager()
manager.start_service()

# 1. 檢查目錄中的內容
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. 檢查已下載的內容
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. 下載模型
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. 驗證它現在是否在快取中
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. 載入它
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. 檢查已載入的內容
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. 卸載它
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

const alias = "qwen2.5-0.5b"; // 用於快速測試的小型模型

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. 從目錄中取得模型
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. 如有需要，下載模型
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. 載入模型
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. 卸載模型
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### 練習 6：快速啟動模式

每種語言都提供一個捷徑，讓你能在一次呼叫中啟動服務並載入模型。這些是大多數應用程式的<strong>推薦用法</strong>。

<details>
<summary><h3>🐍 Python - 建構子啟動</h3></summary>

```python
from foundry_local import FoundryLocalManager

# 傳入別名給建構子 - 它處理所有事情：
# 1. 若服務未運行則啟動服務
# 2. 若沒有快取則下載模型
# 3. 將模型載入推理伺服器
manager = FoundryLocalManager("phi-3.5-mini")

# 立即可使用
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

參數 `bootstrap`（預設為 `True`）控管此行為。如果你想手動控制，請設定 `bootstrap=False`：

```python
# 手動模式 - 不會自動執行任何操作
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + 目錄</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() 處理所有事項：
// 1. 啟動服務
// 2. 從目錄中取得模型
// 3. 若需要則下載並載入模型
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// 立即可用
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + 目錄</h3></summary>

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

> **C# 注意事項：** C# SDK 使用 `Configuration` 來控制應用程式名稱、日誌記錄、快取目錄，甚至固定特定的 Web 伺服器埠號。這使它成為三個 SDK 中配置最細緻的。

</details>

---

### 練習 7：原生 ChatClient（不需要 OpenAI SDK）

JavaScript 和 C# SDK 提供 `createChatClient()` 方便方法，回傳原生聊天客戶端 — 不需要另外安裝或設定 OpenAI SDK。

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

// 直接從模型建立 ChatClient — 不需要導入 OpenAI
const chatClient = model.createChatClient();

// completeChat 會返回一個與 OpenAI 相容的回應物件
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// 串流使用回呼模式
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` 也支援工具呼叫 — 將工具做為第二個參數傳入：

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

> **何時使用哪種模式：**
> - **`createChatClient()`** — 快速原型開發，依賴較少，程式碼更簡單
> - **OpenAI SDK** — 對參數（溫度、top_p、停止字元等）擁有完整控制，對正式應用更佳

---

### 練習 8：模型變種與硬體選擇

模型可能有多個針對不同硬體優化的<strong>變種</strong>。SDK 會自動選擇最適合的版本，但你也可以檢視並手動選擇。

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// 列出所有可用的變體
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK 會自動為您的硬體選擇最佳變體
// 若要覆寫，請使用 selectVariant():
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

在 Python 中，SDK 會根據硬體自動選擇最佳變種。使用 `get_model_info()` 查看選擇結果：

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

#### 含 NPU 變種的模型

部分模型有針對具備神經處理單元 (Neural Processing Units，例如 Qualcomm Snapdragon、Intel Core Ultra) 優化的 NPU 變種：

| 模型 | 是否有 NPU 變種 |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **提示：** 在具備 NPU 的硬體上，SDK 會自動選擇 NPU 變種，不需更改程式碼。對於 Windows 上的 C# 專案，請加入 `Microsoft.AI.Foundry.Local.WinML` NuGet 套件啟用 QNN 執行提供者 — QNN 是透過 WinML 的外掛 EP 提供。

---

### 練習 9：模型升級與目錄更新

模型目錄會定期更新。使用這些方法來檢查並套用更新。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# 重新整理目錄以取得最新的模型清單
manager.refresh_catalog()

# 檢查快取模型是否有更新版本可用
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

// 重新整理目錄以獲取最新模型列表
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// 重新整理後列出所有可用模型
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### 練習 10：操作推理模型

**phi-4-mini-reasoning** 模型包含連貫的推理思維。它在產生最終答案前，會用 `<think>...</think>` 標籤包裹內部思考流程。這對需要多步邏輯、數學或問題解決的任務十分有用。

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning 大約是 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# 模型將其思考包裹在 <think>...</think> 標籤中
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

// 解析連鎖思考
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **何時使用推理模型：**
> - 數學與邏輯問題
> - 多步規劃任務
> - 複雜程式碼生成
> - 需要顯示演算過程以提升準確度的任務
>
> **折衷考量：** 推理模型會產生更多 tokens（`<think>` 區段），速度較慢。對於簡單問答，標準模型例如 phi-3.5-mini 較快。

---

### 練習 11：理解別名與硬體選擇

當你傳入<strong>別名</strong>（如 `phi-3.5-mini`）而非完整模型 ID 時，SDK 會自動根據你的硬體選擇最佳變種：

| 硬體 | 被選擇的執行提供者 |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (透過 WinML 外掛) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| 任何裝置（備援） | `CPUExecutionProvider` 或 `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# 該別名解析為適用於您的硬體的最佳版本
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **提示：** 應用程式代碼中務必使用別名。當部署到用戶機器時，SDK 會在執行時挑選最佳變種 — NVIDIA 選 CUDA、Qualcomm 選 QNN、其他選 CPU。

---

### 練習 12：C# 配置選項

C# SDK 的 `Configuration` 類別提供細緻的執行期控制：

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

| 屬性 | 預設值 | 說明 |
|----------|---------|-------------|
| `AppName` | （必填） | 你的應用程式名稱 |
| `LogLevel` | `Information` | 日誌詳細程度 |
| `Web.Urls` | （動態） | 固定 Web 伺服器的特定埠號 |
| `AppDataDir` | 作業系統預設 | 應用程式資料基底目錄 |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | 模型存放位置 |
| `LogsDir` | `{AppDataDir}/logs` | 日誌寫入位置 |

---

### 練習 13：瀏覽器使用（僅限 JavaScript）

JavaScript SDK 包含瀏覽器相容版本。在瀏覽器中，必須透過 CLI 手動啟動服務並指定主機 URL：

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// 先手動啟動服務：
//   foundry service start
// 然後使用 CLI 輸出的 URL
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// 瀏覽目錄
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **瀏覽器限制：** 瀏覽器版本<strong>不支援</strong> `startWebService()`，你必須確保 Foundry Local 服務已啟動後才能在瀏覽器中使用 SDK。

---

## 完整 API 參考

### Python

| 類別 | 方法 | 說明 |
|----------|--------|-------------|
| <strong>初始化</strong> | `FoundryLocalManager(alias?, bootstrap=True)` | 建立管理器；可選擇帶入模型自動啟動 |
| <strong>服務</strong> | `is_service_running()` | 檢查服務是否運行中 |
| <strong>服務</strong> | `start_service()` | 啟動服務 |
| <strong>服務</strong> | `endpoint` | API 端點 URL |
| <strong>服務</strong> | `api_key` | API 金鑰 |
| <strong>目錄</strong> | `list_catalog_models()` | 列出所有可用模型 |
| <strong>目錄</strong> | `refresh_catalog()` | 更新目錄 |
| <strong>目錄</strong> | `get_model_info(alias_or_model_id)` | 取得模型元數據 |
| <strong>快取</strong> | `get_cache_location()` | 快取目錄路徑 |
| <strong>快取</strong> | `list_cached_models()` | 列出已下載模型 |
| <strong>模型</strong> | `download_model(alias_or_model_id)` | 下載模型 |
| <strong>模型</strong> | `load_model(alias_or_model_id, ttl=600)` | 載入模型 |
| <strong>模型</strong> | `unload_model(alias_or_model_id)` | 卸載模型 |
| <strong>模型</strong> | `list_loaded_models()` | 列出已載入模型 |
| <strong>模型</strong> | `is_model_upgradeable(alias_or_model_id)` | 檢查是否有新版可用 |
| <strong>模型</strong> | `upgrade_model(alias_or_model_id)` | 將模型升級到最新版 |
| <strong>服務</strong> | `httpx_client` | 預配置用於直接 API 呼叫的 HTTPX 客戶端 |

### JavaScript

| 類別 | 方法 | 說明 |
|----------|--------|-------------|
| <strong>初始化</strong> | `FoundryLocalManager.create(options)` | 初始化 SDK 單例 |
| <strong>初始化</strong> | `FoundryLocalManager.instance` | 存取單例管理器 |
| <strong>服務</strong> | `manager.startWebService()` | 啟動 Web 服務 |
| <strong>服務</strong> | `manager.urls` | 服務基底 URL 陣列 |
| <strong>目錄</strong> | `manager.catalog` | 存取模型目錄 |
| <strong>目錄</strong> | `catalog.getModel(alias)` | 依別名取得模型物件（回傳 Promise） |
| <strong>模型</strong> | `model.isCached` | 模型是否已下載 |
| <strong>模型</strong> | `model.download()` | 下載模型 |
| <strong>模型</strong> | `model.load()` | 載入模型 |
| <strong>模型</strong> | `model.unload()` | 卸載模型 |
| <strong>模型</strong> | `model.id` | 模型唯一識別碼 |
| <strong>模型</strong> | `model.alias` | 模型別名 |
| <strong>模型</strong> | `model.createChatClient()` | 取得原生聊天客戶端（無需OpenAI SDK） |
| <strong>模型</strong> | `model.createAudioClient()` | 取得語音轉錄客戶端 |
| <strong>模型</strong> | `model.removeFromCache()` | 從本地快取移除模型 |
| <strong>模型</strong> | `model.selectVariant(variant)` | 選擇特定硬體變種 |
| <strong>模型</strong> | `model.variants` | 此模型可用變種陣列 |
| <strong>模型</strong> | `model.isLoaded()` | 檢查模型是否已載入 |
| <strong>目錄</strong> | `catalog.getModels()` | 列出所有可用模型 |
| <strong>目錄</strong> | `catalog.getCachedModels()` | 列出已下載模型 |
| <strong>目錄</strong> | `catalog.getLoadedModels()` | 列出目前載入模型 |
| <strong>目錄</strong> | `catalog.updateModels()` | 從服務刷新目錄 |
| <strong>服務</strong> | `manager.stopWebService()` | 停止 Foundry Local Web 服務 |

### C# (v0.8.0+)

| 類別 | 方法 | 說明 |
|----------|--------|-------------|
| <strong>初始化</strong> | `FoundryLocalManager.CreateAsync(config, logger)` | 初始化管理器 |
| <strong>初始化</strong> | `FoundryLocalManager.Instance` | 存取單例 |
| <strong>目錄</strong> | `manager.GetCatalogAsync()` | 取得目錄 |
| <strong>目錄</strong> | `catalog.ListModelsAsync()` | 列出所有模型 |
| <strong>目錄</strong> | `catalog.GetModelAsync(alias)` | 取得特定模型 |
| <strong>目錄</strong> | `catalog.GetCachedModelsAsync()` | 列出已快取模型 |
| <strong>目錄</strong> | `catalog.GetLoadedModelsAsync()` | 列出已載入模型 |
| <strong>模型</strong> | `model.DownloadAsync(progress?)` | 下載模型 |
| <strong>模型</strong> | `model.LoadAsync()` | 載入模型 |
| <strong>模型</strong> | `model.UnloadAsync()` | 卸載模型 |
| <strong>模型</strong> | `model.SelectVariant(variant)` | 選擇硬體變種 |
| <strong>模型</strong> | `model.GetChatClientAsync()` | 取得原生聊天客戶端 |
| <strong>模型</strong> | `model.GetAudioClientAsync()` | 取得語音轉錄客戶端 |
| <strong>模型</strong> | `model.GetPathAsync()` | 取得本地檔案路徑 |
| <strong>目錄</strong> | `catalog.GetModelVariantAsync(alias, variant)` | 取得特定硬體變種 |
| <strong>目錄</strong> | `catalog.UpdateModelsAsync()` | 更新目錄 |
| <strong>伺服器</strong> | `manager.StartWebServerAsync()` | 啟動 REST Web 伺服器 |
| <strong>伺服器</strong> | `manager.StopWebServerAsync()` | 停止 REST Web 伺服器 |
| <strong>配置</strong> | `config.ModelCacheDir` | 快取目錄 |

---

## 主要重點摘要

| 概念 | 你學到了什麼 |
|---------|-----------------|
| **SDK vs CLI** | SDK 提供程式化控制 — 是應用程式不可或缺的工具 |
| <strong>控制平面</strong> | SDK 管理服務、模型與快取 |
| <strong>動態埠號</strong> | 永遠使用 `manager.endpoint`（Python）或 `manager.urls[0]`（JS/C#）— 不要硬編埠號 |
| <strong>別名</strong> | 使用別名以啟用自動硬體最佳化模型選擇 |
| <strong>快速開始</strong> | Python: `FoundryLocalManager(alias)`，JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# 重新設計** | v0.8.0+ 為獨立版本 - 終端使用者機器無需 CLI |
| <strong>模型生命週期</strong> | 目錄 → 下載 → 載入 → 使用 → 卸載 |
| **FoundryModelInfo** | 豐富的元資料：任務、裝置、大小、授權、工具呼叫支援 |
| **ChatClient** | `createChatClient()`（JS）/ `GetChatClientAsync()`（C#）用於無 OpenAI 的使用 |
| <strong>變體</strong> | 模型有硬體專屬變體（CPU、GPU、NPU）；自動選擇 |
| <strong>升級</strong> | Python：`is_model_upgradeable()` + `upgrade_model()` 保持模型更新 |
| <strong>目錄刷新</strong> | `refresh_catalog()`（Python）/ `updateModels()`（JS）以發現新模型 |

---

## 資源

| 資源 | 連結 |
|----------|------|
| SDK 參考（所有語言） | [Microsoft Learn - Foundry Local SDK 參考](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| 與推論 SDK 集成 | [Microsoft Learn - 推論 SDK 集成](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API 參考 | [Foundry Local C# API 參考](https://aka.ms/fl-csharp-api-ref) |
| C# SDK 範例 | [GitHub - Foundry Local SDK 範例](https://aka.ms/foundrylocalSDK) |
| Foundry Local 官方網站 | [foundrylocal.ai](https://foundrylocal.ai) |

---

## 下一步

繼續前往 [第三部分：使用 SDK 與 OpenAI](part3-sdk-and-apis.md)，連接 SDK 與 OpenAI 用戶端庫，並建立您的第一個聊天完成應用程式。