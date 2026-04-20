![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Phần 2: Khám phá sâu Foundry Local SDK

> **Mục tiêu:** Thành thạo Foundry Local SDK để quản lý mô hình, dịch vụ và bộ nhớ đệm theo lập trình - và hiểu lý do tại sao SDK là cách tiếp cận được khuyến nghị thay vì CLI để xây dựng ứng dụng.

## Tổng quan

Ở Phần 1, bạn đã sử dụng **Foundry Local CLI** để tải xuống và chạy các mô hình tương tác. CLI rất tuyệt để khảo sát, nhưng khi bạn xây dựng ứng dụng thực sự, bạn cần **kiểm soát theo lập trình**. Foundry Local SDK cung cấp cho bạn điều đó - nó quản lý **control plane** (khởi chạy dịch vụ, phát hiện mô hình, tải xuống, tải mô hình) để mã ứng dụng của bạn có thể tập trung vào **data plane** (gửi đề bài, nhận kết quả hoàn thành).

Lab này sẽ dạy bạn toàn bộ API của SDK trên Python, JavaScript và C#. Cuối cùng bạn sẽ hiểu mọi phương thức có sẵn và khi nào sử dụng từng cái.

## Mục tiêu học tập

Kết thúc lab này bạn sẽ có khả năng:

- Giải thích tại sao SDK được ưu tiên hơn CLI cho phát triển ứng dụng
- Cài đặt Foundry Local SDK cho Python, JavaScript hoặc C#
- Sử dụng `FoundryLocalManager` để khởi chạy dịch vụ, quản lý mô hình và truy vấn danh mục
- Liệt kê, tải xuống, tải và gỡ tải mô hình theo lập trình
- Kiểm tra siêu dữ liệu mô hình bằng `FoundryModelInfo`
- Hiểu sự khác biệt giữa catalog, cache và các mô hình đã tải
- Sử dụng constructor bootstrap (Python) và mẫu `create()` + catalog (JavaScript)
- Hiểu thiết kế lại SDK C# và API hướng đối tượng của nó

---

## Yêu cầu trước

| Yêu cầu | Chi tiết |
|-------------|---------|
| **Foundry Local CLI** | Đã cài đặt và có trong `PATH` của bạn ([Phần 1](part1-getting-started.md)) |
| **Runtime ngôn ngữ** | **Python 3.9+** và/hoặc **Node.js 18+** và/hoặc **.NET 9.0+** |

---

## Khái niệm: SDK vs CLI - Tại sao dùng SDK?

| Khía cạnh | CLI (lệnh `foundry`) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Trường hợp sử dụng** | Khám phá, kiểm thử thủ công | Tích hợp ứng dụng |
| **Quản lý dịch vụ** | Thủ công: `foundry service start` | Tự động: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Phát hiện cổng** | Đọc từ đầu ra CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Tải mô hình** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Xử lý lỗi** | Mã thoát, stderr | Ngoại lệ, lỗi kiểu |
| **Tự động hóa** | Shell scripts | Tích hợp ngôn ngữ bản địa |
| **Triển khai** | Cần CLI trên máy người dùng cuối | SDK C# có thể tự chứa (không cần CLI) |

> **Ý chính:** SDK xử lý toàn bộ vòng đời: khởi chạy dịch vụ, kiểm tra cache, tải mô hình còn thiếu, tải mô hình, và phát hiện endpoint chỉ trong vài dòng code. Ứng dụng của bạn không cần phân tích đầu ra CLI hay quản lý tiến trình con.

---

## Bài tập Lab

### Bài tập 1: Cài đặt SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Xác minh việc cài đặt:

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

Xác minh việc cài đặt:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Có hai gói NuGet:

| Gói | Nền tảng | Mô tả |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Đa nền tảng | Hoạt động trên Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Chỉ Windows | Thêm tăng tốc phần cứng WinML; tải xuống và cài đặt nhà cung cấp thực thi plugin (ví dụ QNN cho Qualcomm NPU) |

**Cài đặt trên Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Chỉnh sửa file `.csproj`:

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

> **Lưu ý:** Trên Windows, gói WinML là phiên bản mở rộng bao gồm SDK cơ bản plus nhà cung cấp thực thi QNN. Trên Linux/macOS, sử dụng SDK cơ bản. Tham chiếu TFM và gói theo điều kiện đảm bảo dự án hoàn toàn đa nền tảng.

Tạo file `nuget.config` ở thư mục gốc dự án:

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

Phục hồi các gói:

```bash
dotnet restore
```

</details>

---

### Bài tập 2: Khởi chạy dịch vụ và liệt kê catalog

Điều đầu tiên bất kỳ ứng dụng nào làm là khởi chạy dịch vụ Foundry Local và phát hiện các mô hình có sẵn.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Tạo một trình quản lý và khởi động dịch vụ
manager = FoundryLocalManager()
manager.start_service()

# Liệt kê tất cả các mô hình có trong danh mục
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Phương thức quản lý dịch vụ

| Phương thức | Chữ ký | Mô tả |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Kiểm tra dịch vụ có đang chạy không |
| `start_service()` | `() -> None` | Khởi chạy dịch vụ Foundry Local |
| `service_uri` | `@property -> str` | URI cơ sở của dịch vụ |
| `endpoint` | `@property -> str` | Endpoint API (URI dịch vụ + `/v1`) |
| `api_key` | `@property -> str` | Khóa API (từ env hoặc giá trị mặc định) |

#### Python SDK - Phương thức quản lý catalog

| Phương thức | Chữ ký | Mô tả |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Liệt kê tất cả mô hình trong catalog |
| `refresh_catalog()` | `() -> None` | Làm mới catalog từ dịch vụ |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Lấy thông tin mô hình cụ thể |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Tạo một quản lý và khởi động dịch vụ
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Duyệt qua danh mục
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Phương thức Manager

| Phương thức | Chữ ký | Mô tả |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Khởi tạo singleton SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Truy cập manager singleton |
| `manager.startWebService()` | `() => Promise<void>` | Khởi chạy dịch vụ web Foundry Local |
| `manager.urls` | `string[]` | Mảng URL cơ sở của dịch vụ |

#### JavaScript SDK - Phương thức Catalog và Model

| Phương thức | Chữ ký | Mô tả |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Truy cập catalog mô hình |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Lấy đối tượng mô hình theo alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

SDK C# phiên bản 0.8.0+ sử dụng kiến trúc hướng đối tượng với các đối tượng `Configuration`, `Catalog`, và `Model`:

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

#### C# SDK - Các lớp chính

| Lớp | Mục đích |
|-------|---------|
| `Configuration` | Cài đặt tên app, mức log, thư mục cache, URL server web |
| `FoundryLocalManager` | Điểm vào chính - tạo qua `CreateAsync()`, truy cập qua `.Instance` |
| `Catalog` | Duyệt, tìm kiếm và lấy mô hình từ catalog |
| `Model` | Đại diện mô hình cụ thể - tải xuống, tải vào bộ nhớ, lấy client |

#### C# SDK - Phương thức Manager và Catalog

| Phương thức | Mô tả |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Khởi tạo manager |
| `FoundryLocalManager.Instance` | Truy cập manager singleton |
| `manager.GetCatalogAsync()` | Lấy catalog mô hình |
| `catalog.ListModelsAsync()` | Liệt kê tất cả mô hình có sẵn |
| `catalog.GetModelAsync(alias: "alias")` | Lấy mô hình cụ thể theo alias |
| `catalog.GetCachedModelsAsync()` | Liệt kê các mô hình đã tải xuống |
| `catalog.GetLoadedModelsAsync()` | Liệt kê các mô hình đang được load |

> **Lưu ý kiến trúc C#:** SDK C# v0.8.0+ tái thiết kế khiến ứng dụng **tự chứa**; không cần CLI Foundry Local trên máy người dùng cuối. SDK quản lý mô hình và suy luận một cách thuần native.

</details>

---

### Bài tập 3: Tải xuống và tải mô hình

SDK tách biệt việc tải xuống (lưu trên đĩa) và tải mô hình (vào bộ nhớ). Điều này cho phép bạn tải trước mô hình trong lúc thiết lập và tải khi cần.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Lựa chọn A: Thủ công từng bước một
manager = FoundryLocalManager()
manager.start_service()

# Kiểm tra bộ nhớ đệm trước
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

# Lựa chọn B: Khởi tạo một dòng (được khuyến nghị)
# Truyền bí danh vào hàm tạo - nó khởi động dịch vụ, tải xuống và nạp tự động
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Phương thức quản lý mô hình

| Phương thức | Chữ ký | Mô tả |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Tải mô hình về bộ nhớ đệm cục bộ |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Tải mô hình vào server suy luận |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Gỡ mô hình khỏi server |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Liệt kê các mô hình đang load |

#### Python - Phương thức quản lý cache

| Phương thức | Chữ ký | Mô tả |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Lấy đường dẫn thư mục cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Liệt kê các mô hình đã tải xuống |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Phương pháp từng bước
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

#### JavaScript - Phương thức mô hình

| Phương thức | Chữ ký | Mô tả |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Mô hình đã được tải xuống chưa |
| `model.download()` | `() => Promise<void>` | Tải mô hình về cache cục bộ |
| `model.load()` | `() => Promise<void>` | Tải vào server suy luận |
| `model.unload()` | `() => Promise<void>` | Gỡ khỏi server suy luận |
| `model.id` | `string` | Định danh duy nhất của mô hình |

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

#### C# - Phương thức mô hình

| Phương thức | Mô tả |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Tải biến thể được chọn |
| `model.LoadAsync()` | Tải mô hình vào bộ nhớ |
| `model.UnloadAsync()` | Gỡ mô hình |
| `model.SelectVariant(variant)` | Chọn biến thể cụ thể (CPU/GPU/NPU) |
| `model.SelectedVariant` | Biến thể đang được chọn |
| `model.Variants` | Tất cả các biến thể có sẵn của mô hình |
| `model.GetPathAsync()` | Lấy đường dẫn file cục bộ |
| `model.GetChatClientAsync()` | Lấy client chat native (không cần OpenAI SDK) |
| `model.GetAudioClientAsync()` | Lấy client audio cho phiên âm |

</details>

---

### Bài tập 4: Kiểm tra siêu dữ liệu mô hình

Đối tượng `FoundryModelInfo` chứa siêu dữ liệu phong phú về mỗi mô hình. Hiểu các trường này giúp bạn chọn mô hình phù hợp cho ứng dụng.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Lấy thông tin chi tiết về một mẫu cụ thể
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

#### Các trường của FoundryModelInfo

| Trường | Kiểu | Mô tả |
|-------|------|-------------|
| `alias` | string | Tên ngắn (ví dụ `phi-3.5-mini`) |
| `id` | string | Định danh duy nhất của mô hình |
| `version` | string | Phiên bản mô hình |
| `task` | string | `chat-completions` hoặc `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, hoặc NPU |
| `execution_provider` | string | Backend runtime (CUDA, CPU, QNN, WebGPU, v.v.) |
| `file_size_mb` | int | Kích thước trên đĩa tính bằng MB |
| `supports_tool_calling` | bool | Mô hình có hỗ trợ gọi hàm/công cụ không |
| `publisher` | string | Ai là người phát hành mô hình |
| `license` | string | Tên giấy phép |
| `uri` | string | URI của mô hình |
| `prompt_template` | dict/null | Mẫu prompt, nếu có |

---

### Bài tập 5: Quản lý vòng đời mô hình

Thực hành toàn bộ vòng đời: liệt kê → tải xuống → tải → sử dụng → gỡ tải.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Mô hình nhỏ để kiểm tra nhanh

manager = FoundryLocalManager()
manager.start_service()

# 1. Kiểm tra những gì có trong danh mục
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Kiểm tra những gì đã được tải xuống
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Tải xuống một mô hình
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Xác minh nó hiện đang ở trong bộ nhớ đệm
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Tải nó lên
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Kiểm tra những gì đã được tải
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Thoát tải nó
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

const alias = "qwen2.5-0.5b"; // Mô hình nhỏ để kiểm thử nhanh

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Lấy mô hình từ danh mục
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Tải xuống nếu cần
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Tải nó lên
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Gỡ nó ra
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Bài tập 6: Mẫu Khởi động Nhanh

Mỗi ngôn ngữ cung cấp một cách rút gọn để khởi động dịch vụ và tải mô hình trong một lần gọi. Đây là các **mẫu khuyến nghị** cho hầu hết các ứng dụng.

<details>
<summary><h3>🐍 Python - Bootstrap qua Constructor</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Truyền một bí danh vào hàm tạo - nó xử lý mọi thứ:
# 1. Khởi động dịch vụ nếu chưa chạy
# 2. Tải mô hình nếu chưa được lưu trong bộ nhớ đệm
# 3. Tải mô hình vào máy chủ suy diễn
manager = FoundryLocalManager("phi-3.5-mini")

# Sẵn sàng sử dụng ngay lập tức
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Tham số `bootstrap` (mặc định `True`) điều khiển hành vi này. Đặt `bootstrap=False` nếu bạn muốn kiểm soát thủ công:

```python
# Chế độ thủ công - không có gì xảy ra tự động
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Danh mục</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() xử lý mọi thứ:
// 1. Khởi động dịch vụ
// 2. Lấy mô hình từ danh mục
// 3. Tải xuống nếu cần và nạp mô hình
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Sẵn sàng sử dụng ngay lập tức
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Danh mục</h3></summary>

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

> **Lưu ý C#:** SDK C# sử dụng `Configuration` để kiểm soát tên ứng dụng, ghi log, thư mục cache, thậm chí cố định một cổng web server cụ thể. Điều này làm cho nó là SDK có khả năng cấu hình nhiều nhất trong ba SDK.

</details>

---

### Bài tập 7: ChatClient Gốc (Không Cần OpenAI SDK)

SDK JavaScript và C# cung cấp phương thức tiện lợi `createChatClient()` trả về một client chat gốc — không cần cài đặt hoặc cấu hình OpenAI SDK riêng biệt.

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

// Tạo một ChatClient trực tiếp từ mô hình — không cần nhập khẩu OpenAI
const chatClient = model.createChatClient();

// completeChat trả về một đối tượng phản hồi tương thích với OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Phát trực tiếp sử dụng mẫu callback
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` cũng hỗ trợ gọi công cụ — truyền các công cụ làm đối số thứ hai:

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

> **Khi nào dùng mẫu nào:**
> - **`createChatClient()`** — Phác thảo nhanh, ít phụ thuộc, mã đơn giản hơn
> - **OpenAI SDK** — Kiểm soát đầy đủ tham số (nhiệt độ, top_p, token dừng, v.v.), tốt cho ứng dụng sản xuất

---

### Bài tập 8: Biến thể Mô hình và Lựa chọn Phần cứng

Các mô hình có thể có nhiều **biến thể** tối ưu cho các phần cứng khác nhau. SDK tự động chọn biến thể tốt nhất, nhưng bạn cũng có thể kiểm tra và chọn thủ công.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Liệt kê tất cả các biến thể có sẵn
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK tự động chọn biến thể tốt nhất cho phần cứng của bạn
// Để ghi đè, sử dụng selectVariant():
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

Trong Python, SDK tự động chọn biến thể tốt nhất dựa trên phần cứng. Dùng `get_model_info()` để xem những gì đã được chọn:

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

#### Mô hình có Biến thể NPU

Một số mô hình có biến thể tối ưu NPU cho các thiết bị có Đơn vị Xử lý Thần kinh (Qualcomm Snapdragon, Intel Core Ultra):

| Mô hình | Biến thể NPU Có sẵn |
|---------|:-------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Mẹo:** Trên phần cứng hỗ trợ NPU, SDK tự động chọn biến thể NPU khi có. Bạn không cần thay đổi mã. Với dự án C# trên Windows, thêm gói NuGet `Microsoft.AI.Foundry.Local.WinML` để kích hoạt nhà cung cấp thực thi QNN — QNN được cung cấp dạng plugin EP qua WinML.

---

### Bài tập 9: Nâng cấp Mô hình và Làm mới Danh mục

Danh mục mô hình được cập nhật định kỳ. Dùng các phương thức này để kiểm tra và áp dụng cập nhật.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Làm mới danh mục để lấy danh sách mẫu mới nhất
manager.refresh_catalog()

# Kiểm tra xem mô hình được lưu trong bộ nhớ cache có phiên bản mới hơn không
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

// Làm mới danh mục để lấy danh sách mô hình mới nhất
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Liệt kê tất cả các mô hình có sẵn sau khi làm mới
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Bài tập 10: Làm việc với Mô hình Lý luận

Mô hình **phi-4-mini-reasoning** bao gồm chuỗi lý luận bước-thought. Nó bao bọc quá trình suy nghĩ nội bộ trong các thẻ `<think>...</think>` trước khi tạo câu trả lời cuối cùng. Điều này hữu ích cho các tác vụ cần logic nhiều bước, toán học, hoặc giải quyết vấn đề.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning có kích thước khoảng 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Mô hình bao bọc suy nghĩ của nó trong thẻ <think>...</think>
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

// Phân tích suy nghĩ chuỗi nghĩ
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Khi nào dùng mô hình lý luận:**
> - Vấn đề toán và logic
> - Nhiều bước lập kế hoạch
> - Sinh mã phức tạp
> - Tác vụ cần thể hiện quá trình làm việc để tăng độ chính xác
>
> **Điểm cân nhắc:** Mô hình lý luận sinh ra nhiều token hơn (phần `<think>`) và chậm hơn. Đối với câu hỏi đơn giản, mô hình tiêu chuẩn như phi-3.5-mini nhanh hơn.

---

### Bài tập 11: Hiểu về Bí danh và Lựa chọn Phần cứng

Khi bạn truyền một **bí danh** (như `phi-3.5-mini`) thay vì ID mô hình đầy đủ, SDK tự động chọn biến thể tốt nhất cho phần cứng của bạn:

| Phần cứng | Nhà cung cấp thực thi được chọn |
|-----------|-------------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (qua plugin WinML) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Bất kỳ thiết bị nào (dự phòng) | `CPUExecutionProvider` hoặc `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Bí danh phân giải thành biến thể tốt nhất cho phần cứng CỦA BẠN
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Mẹo:** Luôn dùng bí danh trong mã ứng dụng của bạn. Khi bạn triển khai trên máy người dùng, SDK sẽ chọn biến thể tối ưu khi chạy — CUDA trên NVIDIA, QNN trên Qualcomm, CPU ở nơi khác.

---

### Bài tập 12: Tùy chọn Cấu hình C#

Lớp `Configuration` trong SDK C# cung cấp kiểm soát chi tiết runtime:

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

| Thuộc tính | Mặc định | Mô tả |
|------------|----------|-------|
| `AppName` | (bắt buộc) | Tên ứng dụng của bạn |
| `LogLevel` | `Information` | Mức độ ghi log |
| `Web.Urls` | (động) | Cố định cổng cụ thể cho web server |
| `AppDataDir` | Mặc định hệ điều hành | Thư mục gốc cho dữ liệu ứng dụng |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Nơi lưu mô hình |
| `LogsDir` | `{AppDataDir}/logs` | Nơi ghi log |

---

### Bài tập 13: Sử dụng Trình duyệt (Chỉ JavaScript)

SDK JavaScript bao gồm phiên bản tương thích trình duyệt. Trong trình duyệt, bạn phải khởi động dịch vụ thủ công qua CLI và chỉ định URL host:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Khởi động dịch vụ bằng tay trước:
//   foundry service start
// Sau đó sử dụng URL từ đầu ra CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Duyệt danh mục
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Giới hạn trình duyệt:** Phiên bản trình duyệt **không** hỗ trợ `startWebService()`. Bạn phải đảm bảo dịch vụ Foundry Local đã chạy trước khi dùng SDK trong trình duyệt.

---

## Tham khảo API đầy đủ

### Python

| Danh mục | Phương thức | Mô tả |
|----------|-------------|-------|
| **Khởi tạo** | `FoundryLocalManager(alias?, bootstrap=True)` | Tạo manager; có thể bootstrap với mô hình |
| **Dịch vụ** | `is_service_running()` | Kiểm tra dịch vụ đang chạy |
| **Dịch vụ** | `start_service()` | Khởi động dịch vụ |
| **Dịch vụ** | `endpoint` | URL endpoint API |
| **Dịch vụ** | `api_key` | Khóa API |
| **Danh mục** | `list_catalog_models()` | Liệt kê tất cả mô hình có sẵn |
| **Danh mục** | `refresh_catalog()` | Làm mới danh mục |
| **Danh mục** | `get_model_info(alias_or_model_id)` | Lấy metadata mô hình |
| **Cache** | `get_cache_location()` | Đường dẫn thư mục cache |
| **Cache** | `list_cached_models()` | Liệt kê mô hình đã tải |
| **Mô hình** | `download_model(alias_or_model_id)` | Tải xuống mô hình |
| **Mô hình** | `load_model(alias_or_model_id, ttl=600)` | Tải mô hình |
| **Mô hình** | `unload_model(alias_or_model_id)` | Tháo mô hình |
| **Mô hình** | `list_loaded_models()` | Liệt kê mô hình đang tải |
| **Mô hình** | `is_model_upgradeable(alias_or_model_id)` | Kiểm tra có bản nâng cấp mới không |
| **Mô hình** | `upgrade_model(alias_or_model_id)` | Nâng cấp mô hình lên phiên bản mới nhất |
| **Dịch vụ** | `httpx_client` | HTTPX client cấu hình sẵn để gọi API trực tiếp |

### JavaScript

| Danh mục | Phương thức | Mô tả |
|----------|-------------|-------|
| **Khởi tạo** | `FoundryLocalManager.create(options)` | Khởi tạo singleton SDK |
| **Khởi tạo** | `FoundryLocalManager.instance` | Truy cập singleton manager |
| **Dịch vụ** | `manager.startWebService()` | Khởi động dịch vụ web |
| **Dịch vụ** | `manager.urls` | Mảng URL nền tảng cho dịch vụ |
| **Danh mục** | `manager.catalog` | Truy cập danh mục mô hình |
| **Danh mục** | `catalog.getModel(alias)` | Lấy đối tượng mô hình theo bí danh (trả về Promise) |
| **Mô hình** | `model.isCached` | Mô hình đã được tải xuống chưa |
| **Mô hình** | `model.download()` | Tải xuống mô hình |
| **Mô hình** | `model.load()` | Tải mô hình |
| **Mô hình** | `model.unload()` | Tháo mô hình |
| **Mô hình** | `model.id` | ID duy nhất của mô hình |
| **Mô hình** | `model.alias` | Bí danh của mô hình |
| **Mô hình** | `model.createChatClient()` | Lấy client chat gốc (không cần SDK OpenAI) |
| **Mô hình** | `model.createAudioClient()` | Lấy client âm thanh để chuyển văn bản |
| **Mô hình** | `model.removeFromCache()` | Xóa mô hình khỏi cache cục bộ |
| **Mô hình** | `model.selectVariant(variant)` | Chọn biến thể phần cứng cụ thể |
| **Mô hình** | `model.variants` | Mảng các biến thể sẵn cho mô hình |
| **Mô hình** | `model.isLoaded()` | Kiểm tra mô hình có đang được tải không |
| **Danh mục** | `catalog.getModels()` | Liệt kê tất cả mô hình có sẵn |
| **Danh mục** | `catalog.getCachedModels()` | Liệt kê mô hình đã tải xuống |
| **Danh mục** | `catalog.getLoadedModels()` | Liệt kê mô hình đang được tải |
| **Danh mục** | `catalog.updateModels()` | Làm mới danh mục từ dịch vụ |
| **Dịch vụ** | `manager.stopWebService()` | Dừng dịch vụ web Foundry Local |

### C# (v0.8.0+)

| Danh mục | Phương thức | Mô tả |
|----------|-------------|-------|
| **Khởi tạo** | `FoundryLocalManager.CreateAsync(config, logger)` | Khởi tạo manager |
| **Khởi tạo** | `FoundryLocalManager.Instance` | Truy cập singleton |
| **Danh mục** | `manager.GetCatalogAsync()` | Lấy danh mục |
| **Danh mục** | `catalog.ListModelsAsync()` | Liệt kê tất cả mô hình |
| **Danh mục** | `catalog.GetModelAsync(alias)` | Lấy một mô hình cụ thể |
| **Danh mục** | `catalog.GetCachedModelsAsync()` | Liệt kê mô hình đã cache |
| **Danh mục** | `catalog.GetLoadedModelsAsync()` | Liệt kê mô hình đang tải |
| **Mô hình** | `model.DownloadAsync(progress?)` | Tải mô hình |
| **Mô hình** | `model.LoadAsync()` | Tải mô hình |
| **Mô hình** | `model.UnloadAsync()` | Tháo mô hình |
| **Mô hình** | `model.SelectVariant(variant)` | Chọn biến thể phần cứng |
| **Mô hình** | `model.GetChatClientAsync()` | Lấy client chat gốc |
| **Mô hình** | `model.GetAudioClientAsync()` | Lấy client chuyển văn bản âm thanh |
| **Mô hình** | `model.GetPathAsync()` | Lấy đường dẫn file cục bộ |
| **Danh mục** | `catalog.GetModelVariantAsync(alias, variant)` | Lấy biến thể phần cứng cụ thể |
| **Danh mục** | `catalog.UpdateModelsAsync()` | Làm mới danh mục |
| **Server** | `manager.StartWebServerAsync()` | Khởi động REST web server |
| **Server** | `manager.StopWebServerAsync()` | Dừng REST web server |
| **Cấu hình** | `config.ModelCacheDir` | Thư mục cache |

---

## Những điểm chính cần nhớ

| Khái niệm | Điều bạn học được |
|-----------|-------------------|
| **SDK và CLI** | SDK cung cấp khả năng điều khiển qua lập trình — thiết yếu cho ứng dụng |
| **Control plane** | SDK quản lý dịch vụ, mô hình, và bộ nhớ cache |
| **Cổng động** | Luôn sử dụng `manager.endpoint` (Python) hoặc `manager.urls[0]` (JS/C#) — không bao giờ cứng mã cổng |
| **Bí danh** | Dùng bí danh để tự động chọn mô hình tối ưu cho phần cứng |
| **Bắt đầu nhanh** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Thiết kế lại C#** | v0.8.0+ là độc lập - không cần CLI trên máy người dùng cuối |
| **Vòng đời model** | Catalog → Tải xuống → Tải → Sử dụng → Tải ra |
| **FoundryModelInfo** | Siêu dữ liệu phong phú: tác vụ, thiết bị, kích thước, giấy phép, hỗ trợ gọi công cụ |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) để sử dụng không cần OpenAI |
| **Biến thể** | Model có các biến thể riêng cho phần cứng (CPU, GPU, NPU); chọn tự động |
| **Nâng cấp** | Python: `is_model_upgradeable()` + `upgrade_model()` để giữ model luôn mới nhất |
| **Làm mới Catalog** | `refresh_catalog()` (Python) / `updateModels()` (JS) để phát hiện model mới |

---

## Tài nguyên

| Tài nguyên | Liên kết |
|----------|------|
| Tham khảo SDK (tất cả ngôn ngữ) | [Microsoft Learn - Tham khảo Foundry Local SDK](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Tích hợp với SDK suy luận | [Microsoft Learn - Tích hợp SDK suy luận](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Tham khảo API C# SDK | [Tham khảo API Foundry Local C#](https://aka.ms/fl-csharp-api-ref) |
| Mẫu C# SDK | [GitHub - Mẫu Foundry Local SDK](https://aka.ms/foundrylocalSDK) |
| Trang web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Bước tiếp theo

Tiếp tục đến [Phần 3: Sử dụng SDK với OpenAI](part3-sdk-and-apis.md) để kết nối SDK với thư viện client OpenAI và xây dựng ứng dụng hoàn thành chat đầu tiên của bạn.