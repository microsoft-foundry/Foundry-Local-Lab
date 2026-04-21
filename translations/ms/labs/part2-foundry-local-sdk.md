![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagian 2: Pengajian Mendalam Foundry Local SDK

> **Matlamat:** Menguasai Foundry Local SDK untuk mengurus model, perkhidmatan, dan caching secara programatik - dan memahami mengapa SDK adalah pendekatan yang disyorkan berbanding CLI untuk membina aplikasi.

## Gambaran Keseluruhan

Dalam Bahagian 1 anda menggunakan **Foundry Local CLI** untuk memuat turun dan menjalankan model secara interaktif. CLI sangat baik untuk penerokaan, tetapi apabila anda membina aplikasi sebenar anda memerlukan **pengawalan secara programatik**. Foundry Local SDK memberi anda itu - ia mengurus **plane kawalan** (memulakan perkhidmatan, menemui model, memuat turun, memuatkan) supaya kod aplikasi anda boleh fokus pada **plane data** (menghantar arahan, menerima penyelesaian).

Makmal ini mengajar anda keseluruhan permukaan API SDK merentasi Python, JavaScript, dan C#. Pada penghujungnya anda akan memahami setiap kaedah yang tersedia dan bila untuk menggunakan setiap satu.

## Objektif Pembelajaran

Pada penghujung makmal ini anda akan mampu untuk:

- Menjelaskan mengapa SDK lebih digemari berbanding CLI untuk pembangunan aplikasi
- Memasang Foundry Local SDK untuk Python, JavaScript, atau C#
- Menggunakan `FoundryLocalManager` untuk memulakan perkhidmatan, mengurus model, dan membuat pertanyaan katalog
- Menyenaraikan, memuat turun, memuatkan, dan memuat keluarkan model secara programatik
- Memeriksa metadata model menggunakan `FoundryModelInfo`
- Memahami perbezaan antara katalog, cache, dan model yang dimuatkan
- Menggunakan konstruktor bootstrap (Python) dan pola `create()` + katalog (JavaScript)
- Memahami reka bentuk semula SDK C# dan API berorientasikan objeknya

---

## Prasyarat

| Keperluan | Butiran |
|-------------|---------|
| **Foundry Local CLI** | Dipasang dan dalam `PATH` anda ([Bahagian 1](part1-getting-started.md)) |
| **Runtime bahasa** | **Python 3.9+** dan/atau **Node.js 18+** dan/atau **.NET 9.0+** |

---

## Konsep: SDK vs CLI - Mengapa Gunakan SDK?

| Aspek | CLI (arahan `foundry`) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Kes penggunaan** | Penerokaan, ujian manual | Integrasi aplikasi |
| **Pengurusan perkhidmatan** | Manual: `foundry service start` | Automatik: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Penemuan port** | Baca dari output CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Muat turun model** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Pengendalian ralat** | Kod keluar, stderr | Exceptions, ralat berjenis |
| **Automasi** | Skrip shell | Integrasi bahasa asli |
| **Penghantaran** | Memerlukan CLI di mesin pengguna akhir | SDK C# boleh berdiri sendiri (tidak perlukan CLI) |

> **Wawasan utama:** SDK mengendalikan seluruh kitaran hayat: memulakan perkhidmatan, memeriksa cache, memuat turun model yang hilang, memuatkannya, dan menemui titik akhir, dalam beberapa baris kod. Aplikasi anda tidak perlu mengurai output CLI atau mengurus subproses.

---

## Latihan Makmal

### Latihan 1: Pasang SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Sahkan pemasangan:

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

Sahkan pemasangan:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Terdapat dua pakej NuGet:

| Pakej | Platform | Penerangan |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Merentas platform | Berfungsi pada Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Windows sahaja | Menambah pecutan perkakasan WinML; memuat turun dan memasang penyedia pelaksanaan plugin (contoh QNN untuk Qualcomm NPU) |

**Penetapan Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Sunting fail `.csproj`:

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

> **Nota:** Di Windows, pakej WinML adalah superset yang termasuk SDK asas serta penyedia pelaksanaan QNN. Di Linux/macOS, SDK asas digunakan sebaliknya. Referensi TFM dan pakej bersyarat memastikan projek sepenuhnya merentas platform.

Buat `nuget.config` di akar projek:

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

Pulihkan pakej:

```bash
dotnet restore
```

</details>

---

### Latihan 2: Mulakan Perkhidmatan dan Senaraikan Katalog

Perkara pertama yang dilakukan oleh mana-mana aplikasi ialah memulakan perkhidmatan Foundry Local dan menemui model yang tersedia.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Cipta pengurus dan mulakan perkhidmatan
manager = FoundryLocalManager()
manager.start_service()

# Senaraikan semua model yang tersedia dalam katalog
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Kaedah Pengurusan Perkhidmatan

| Kaedah | Tandatangan | Penerangan |
|--------|-------------|------------|
| `is_service_running()` | `() -> bool` | Periksa jika perkhidmatan sedang berjalan |
| `start_service()` | `() -> None` | Mulakan perkhidmatan Foundry Local |
| `service_uri` | `@property -> str` | URI asas perkhidmatan |
| `endpoint` | `@property -> str` | Titik akhir API (URI perkhidmatan + `/v1`) |
| `api_key` | `@property -> str` | Kunci API (dari env atau placeholder lalai) |

#### Python SDK - Kaedah Pengurusan Katalog

| Kaedah | Tandatangan | Penerangan |
|--------|-------------|------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Senaraikan semua model dalam katalog |
| `refresh_catalog()` | `() -> None` | Segarkan katalog dari perkhidmatan |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Dapatkan maklumat untuk model tertentu |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Cipta pengurus dan mulakan perkhidmatan
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Semak imbas katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Kaedah Pengurus

| Kaedah | Tandatangan | Penerangan |
|--------|-------------|------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inisialisasi singleton SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Akses pengurus singleton |
| `manager.startWebService()` | `() => Promise<void>` | Mulakan perkhidmatan web Foundry Local |
| `manager.urls` | `string[]` | Array URL asas untuk perkhidmatan |

#### JavaScript SDK - Kaedah Katalog dan Model

| Kaedah | Tandatangan | Penerangan |
|--------|-------------|------------|
| `manager.catalog` | `Catalog` | Akses katalog model |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Dapatkan objek model berdasarkan alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

SDK C# v0.8.0+ menggunakan seni bina berorientasikan objek dengan objek `Configuration`, `Catalog`, dan `Model`:

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

#### C# SDK - Kelas Utama

| Kelas | Tujuan |
|-------|---------|
| `Configuration` | Tetapkan nama aplikasi, tahap log, direktori cache, URL pelayan web |
| `FoundryLocalManager` | Titik masuk utama - dibuat melalui `CreateAsync()`, diakses melalui `.Instance` |
| `Catalog` | Layari, cari, dan dapatkan model dari katalog |
| `Model` | Mewakili model spesifik - muat turun, muat, dapatkan klien |

#### C# SDK - Kaedah Pengurus dan Katalog

| Kaedah | Penerangan |
|--------|------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inisialisasi pengurus |
| `FoundryLocalManager.Instance` | Akses pengurus singleton |
| `manager.GetCatalogAsync()` | Dapatkan katalog model |
| `catalog.ListModelsAsync()` | Senaraikan semua model tersedia |
| `catalog.GetModelAsync(alias: "alias")` | Dapatkan model tertentu berdasarkan alias |
| `catalog.GetCachedModelsAsync()` | Senaraikan model yang telah dimuat turun |
| `catalog.GetLoadedModelsAsync()` | Senaraikan model yang sedang dimuatkan |

> **Nota Seni Bina C#:** Reka bentuk semula SDK C# v0.8.0+ menjadikan aplikasi **bersendirian**; ia tidak memerlukan Foundry Local CLI di mesin pengguna akhir. SDK mengurus pengurusan model dan inferens secara asli.

</details>

---

### Latihan 3: Muat Turun dan Muat Model

SDK memisahkan muat turun (ke cakera) dari muat (ke dalam memori). Ini membolehkan anda pramuat turun model semasa penyediaan dan memuatkannya diminta.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Pilihan A: Langkah demi langkah manual
manager = FoundryLocalManager()
manager.start_service()

# Semak cache terlebih dahulu
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

# Pilihan B: Bootstrap satu baris (disyorkan)
# Berikan alias kepada konstruktor - ia memulakan servis, memuat turun, dan memuat secara automatik
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Kaedah Pengurusan Model

| Kaedah | Tandatangan | Penerangan |
|--------|-------------|------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Muat turun model ke dalam cache tempatan |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Muat model ke dalam pelayan inferens |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Nyahmuat model dari pelayan |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Senaraikan semua model yang sedang dimuat |

#### Python - Kaedah Pengurusan Cache

| Kaedah | Tandatangan | Penerangan |
|--------|-------------|------------|
| `get_cache_location()` | `() -> str` | Dapatkan laluan direktori cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Senaraikan semua model yang dimuat turun |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Pendekatan langkah demi langkah
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

#### JavaScript - Kaedah Model

| Kaedah | Tandatangan | Penerangan |
|--------|-------------|------------|
| `model.isCached` | `boolean` | Sama ada model sudah dimuat turun |
| `model.download()` | `() => Promise<void>` | Muat turun model ke cache tempatan |
| `model.load()` | `() => Promise<void>` | Muat ke dalam pelayan inferens |
| `model.unload()` | `() => Promise<void>` | Nyahmuat dari pelayan inferens |
| `model.id` | `string` | Pengecam unik model |

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

#### C# - Kaedah Model

| Kaedah | Penerangan |
|--------|------------|
| `model.DownloadAsync(progress?)` | Muat turun varian terpilih |
| `model.LoadAsync()` | Muat model ke dalam memori |
| `model.UnloadAsync()` | Nyahmuat model |
| `model.SelectVariant(variant)` | Pilih varian tertentu (CPU/GPU/NPU) |
| `model.SelectedVariant` | Varian yang dipilih kini |
| `model.Variants` | Semua varian yang tersedia untuk model ini |
| `model.GetPathAsync()` | Dapatkan laluan fail lokal |
| `model.GetChatClientAsync()` | Dapatkan klien sembang asli (tidak perlu SDK OpenAI) |
| `model.GetAudioClientAsync()` | Dapatkan klien audio untuk transkripsi |

</details>

---

### Latihan 4: Periksa Metadata Model

Objek `FoundryModelInfo` mengandungi metadata kaya tentang setiap model. Memahami medan ini membantu anda memilih model yang sesuai untuk aplikasi anda.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Dapatkan maklumat terperinci tentang model tertentu
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

#### Medan FoundryModelInfo

| Medan | Jenis | Penerangan |
|-------|-------|------------|
| `alias` | string | Nama ringkas (contoh `phi-3.5-mini`) |
| `id` | string | Pengecam unik model |
| `version` | string | Versi model |
| `task` | string | `chat-completions` atau `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, atau NPU |
| `execution_provider` | string | Backend runtime (CUDA, CPU, QNN, WebGPU, dll.) |
| `file_size_mb` | int | Saiz di cakera dalam MB |
| `supports_tool_calling` | bool | Sama ada model menyokong panggilan fungsi/alat |
| `publisher` | string | Siapa yang menerbitkan model |
| `license` | string | Nama lesen |
| `uri` | string | URI model |
| `prompt_template` | dict/null | Templat arahan, jika ada |

---

### Latihan 5: Mengurus Kitaran Hayat Model

Berlatih kitaran hayat penuh: senaraikan → muat turun → muat → guna → nyahmuat.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Model kecil untuk ujian cepat

manager = FoundryLocalManager()
manager.start_service()

# 1. Semak apa yang ada dalam katalog
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Semak apa yang sudah dimuat turun
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Muat turun model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Sahkan ia kini dalam cache
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Muatkan ia
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Semak apa yang dimuatkan
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Nyahmuat ia
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

const alias = "qwen2.5-0.5b"; // Model kecil untuk ujian cepat

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Dapatkan model dari katalog
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Muat turun jika perlu
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Muatkan ia
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Nyahmuat ia
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Latihan 6: Corak Mula Pantas

Setiap bahasa menyediakan pintasan untuk memulakan perkhidmatan dan memuatkan model dalam satu panggilan. Ini adalah **corak yang disyorkan** untuk kebanyakan aplikasi.

<details>
<summary><h3>🐍 Python - Bootstrap Konstruktor</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Hantar alias ke konstruktor - ia mengendalikan semuanya:
# 1. Memulakan perkhidmatan jika tidak berjalan
# 2. Memuat turun model jika tidak disimpan dalam cache
# 3. Memuat model ke dalam pelayan inferens
manager = FoundryLocalManager("phi-3.5-mini")

# Sedia untuk digunakan dengan segera
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Parameter `bootstrap` (lalai `True`) mengawal tingkah laku ini. Tetapkan `bootstrap=False` jika anda mahu kawalan manual:

```python
# Mod manual - tiada apa yang berlaku secara automatik
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() mengendalikan semuanya:
// 1. Memulakan perkhidmatan
// 2. Mendapatkan model dari katalog
// 3. Memuat turun jika perlu dan memuat model
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Sedia untuk digunakan dengan segera
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Katalog</h3></summary>

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

> **Nota C#:** SDK C# menggunakan `Configuration` untuk mengawal nama aplikasi, log, direktori cache, dan juga menetapkan port pelayan web tertentu. Ini menjadikannya yang paling mudah dikonfigurasi daripada ketiga-tiga SDK.

</details>

---

### Latihan 7: ChatClient Bawaan (Tiada OpenAI SDK Diperlukan)

SDK JavaScript dan C# menyediakan kaedah keselesaan `createChatClient()` yang mengembalikan klien sembang asli — tidak perlu memasang atau mengkonfigurasi SDK OpenAI secara berasingan.

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

// Cipta ChatClient terus dari model — tiada import OpenAI diperlukan
const chatClient = model.createChatClient();

// completeChat mengembalikan objek respons yang serasi dengan OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Penstriman menggunakan corak panggilan balik
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` juga menyokong panggilan alat — serahkan alat sebagai argumen kedua:

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

> **Bila hendak guna corak yang mana:**
> - **`createChatClient()`** — Prototaip cepat, kurang kebergantungan, kod lebih ringkas
> - **SDK OpenAI** — Kawalan penuh ke atas parameter (suhu, top_p, token hentian, dll.), lebih baik untuk aplikasi produksi

---

### Latihan 8: Variasi Model dan Pemilihan Perkakasan

Model boleh mempunyai pelbagai **varian** yang dioptimumkan untuk perkakasan berbeza. SDK memilih varian terbaik secara automatik, tetapi anda juga boleh memeriksa dan memilih secara manual.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Senaraikan semua varian yang tersedia
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK secara automatik memilih varian terbaik untuk perkakasan anda
// Untuk menggantikan, gunakan selectVariant():
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

Dalam Python, SDK secara automatik memilih varian terbaik berdasarkan perkakasan. Gunakan `get_model_info()` untuk melihat apa yang dipilih:

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

#### Model dengan Varian NPU

Sesetengah model mempunyai varian yang dioptimumkan untuk NPU bagi peranti dengan Unit Pemprosesan Neural (Qualcomm Snapdragon, Intel Core Ultra):

| Model | Varian NPU Tersedia |
|-------|:-------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Petua:** Pada perkakasan yang menyokong NPU, SDK secara automatik memilih varian NPU apabila tersedia. Anda tidak perlu mengubah kod anda. Untuk projek C# di Windows, tambah pakej NuGet `Microsoft.AI.Foundry.Local.WinML` untuk mengaktifkan penyedia pelaksanaan QNN — QNN dihantar sebagai plugin EP melalui WinML.

---

### Latihan 9: Kemas Kini Model dan Segarkan Katalog

Katalog model dikemas kini secara berkala. Gunakan kaedah ini untuk memeriksa dan memohon kemas kini.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Segarkan katalog untuk mendapatkan senarai model terkini
manager.refresh_catalog()

# Periksa jika model yang disimpan dalam cache mempunyai versi yang lebih baru tersedia
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

// Segarkan katalog untuk mendapatkan senarai model terkini
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Senaraikan semua model yang tersedia selepas penyegaran
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Latihan 10: Bekerja dengan Model Penalaran

Model **phi-4-mini-reasoning** termasuk penalaran berantai. Ia membungkus pemikiran dalaman dalam tag `<think>...</think>` sebelum menghasilkan jawapan akhirnya. Ini berguna untuk tugasan yang memerlukan logik berbilang langkah, matematik, atau penyelesaian masalah.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning adalah ~4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Model membungkus pemikirannya dalam tag <think>...</think>
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

// Menyemak pemikiran rantai-fikiran
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Bila hendak guna model penalaran:**
> - Masalah matematik dan logik
> - Tugasan perancangan berbilang langkah
> - Penjanaan kod yang kompleks
> - Tugasan di mana menunjukan langkah kerja meningkatkan ketepatan
>
> **Pertukaran:** Model penalaran menghasilkan lebih banyak token (bahagian `<think>`) dan lebih perlahan. Untuk soalan dan jawapan mudah, model standard seperti phi-3.5-mini lebih pantas.

---

### Latihan 11: Memahami Alias dan Pemilihan Perkakasan

Apabila anda menggunakan **alias** (seperti `phi-3.5-mini`) dan bukannya ID model penuh, SDK secara automatik memilih varian terbaik untuk perkakasan anda:

| Perkakasan | Penyedia Pelaksanaan Dipilih |
|------------|------------------------------|
| GPU NVIDIA (CUDA) | `CUDAExecutionProvider` |
| NPU Qualcomm | `QNNExecutionProvider` (melalui plugin WinML) |
| NPU Intel | `OpenVINOExecutionProvider` |
| GPU AMD | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Mana-mana peranti (fallback) | `CPUExecutionProvider` atau `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Alias menyelesaikan kepada varian terbaik untuk perkakasan ANDA
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Petua:** Sentiasa gunakan alias dalam kod aplikasi anda. Apabila anda mengedarkan kepada mesin pengguna, SDK memilih varian optimum masa nyata - CUDA pada NVIDIA, QNN pada Qualcomm, CPU di tempat lain.

---

### Latihan 12: Pilihan Konfigurasi C#

Kelas `Configuration` dalam SDK C# menyediakan kawalan terperinci ke atas runtime:

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

| Properties | Default | Penerangan |
|------------|---------|------------|
| `AppName` | (diperlukan) | Nama aplikasi anda |
| `LogLevel` | `Information` | Tahap verbositi log |
| `Web.Urls` | (dinamik) | Tetapkan port khusus untuk pelayan web |
| `AppDataDir` | Lalai OS | Direktori asas untuk data aplikasi |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Tempat model disimpan |
| `LogsDir` | `{AppDataDir}/logs` | Tempat log ditulis |

---

### Latihan 13: Penggunaan Penyemak Imbas (JavaScript Sahaja)

SDK JavaScript termasuk versi yang serasi dengan penyemak imbas. Dalam penyemak imbas, anda mesti memulakan perkhidmatan secara manual melalui CLI dan nyatakan URL hos:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Mulakan perkhidmatan secara manual terlebih dahulu:
//   perkhidmatan foundry start
// Kemudian gunakan URL dari output CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Layari katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Keterbatasan penyemak imbas:** Versi penyemak imbas **tidak** menyokong `startWebService()`. Anda mesti memastikan perkhidmatan Foundry Local sudah berjalan sebelum menggunakan SDK dalam penyemak imbas.

---

## Rujukan API Lengkap

### Python

| Kategori | Kaedah | Penerangan |
|----------|--------|------------|
| **Inisiasi** | `FoundryLocalManager(alias?, bootstrap=True)` | Buat pengurus; opsyenal buat bootstrap dengan model |
| **Perkhidmatan** | `is_service_running()` | Semak jika perkhidmatan berjalan |
| **Perkhidmatan** | `start_service()` | Mulakan perkhidmatan |
| **Perkhidmatan** | `endpoint` | URL titik akhir API |
| **Perkhidmatan** | `api_key` | Kunci API |
| **Katalog** | `list_catalog_models()` | Senaraikan semua model tersedia |
| **Katalog** | `refresh_catalog()` | Segarkan katalog |
| **Katalog** | `get_model_info(alias_or_model_id)` | Dapatkan metadata model |
| **Cache** | `get_cache_location()` | Laluan direktori cache |
| **Cache** | `list_cached_models()` | Senaraikan model yang dimuat turun |
| **Model** | `download_model(alias_or_model_id)` | Muat turun model |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Muat model |
| **Model** | `unload_model(alias_or_model_id)` | Buang muatan model |
| **Model** | `list_loaded_models()` | Senaraikan model yang dimuatkan |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Semak jika versi lebih baru tersedia |
| **Model** | `upgrade_model(alias_or_model_id)` | Naik taraf model ke versi terbaru |
| **Perkhidmatan** | `httpx_client` | Klien HTTPX yang telah dikonfigurasi untuk panggilan API terus |

### JavaScript

| Kategori | Kaedah | Penerangan |
|----------|--------|------------|
| **Inisiasi** | `FoundryLocalManager.create(options)` | Inisialisasi singleton SDK |
| **Inisiasi** | `FoundryLocalManager.instance` | Akses pengurus singleton |
| **Perkhidmatan** | `manager.startWebService()` | Mulakan perkhidmatan web |
| **Perkhidmatan** | `manager.urls` | Array URL asas untuk perkhidmatan |
| **Katalog** | `manager.catalog` | Akses katalog model |
| **Katalog** | `catalog.getModel(alias)` | Dapatkan objek model melalui alias (mengembalikan Promise) |
| **Model** | `model.isCached` | Sama ada model telah dimuat turun |
| **Model** | `model.download()` | Muat turun model |
| **Model** | `model.load()` | Muat model |
| **Model** | `model.unload()` | Buang muatan model |
| **Model** | `model.id` | Pengenal unik model |
| **Model** | `model.alias` | Alias model |
| **Model** | `model.createChatClient()` | Dapatkan klien sembang asli (tidak perlukan SDK OpenAI) |
| **Model** | `model.createAudioClient()` | Dapatkan klien audio untuk transkripsi |
| **Model** | `model.removeFromCache()` | Keluarkan model dari cache lokal |
| **Model** | `model.selectVariant(variant)` | Pilih varian perkakasan tertentu |
| **Model** | `model.variants` | Array varian tersedia untuk model ini |
| **Model** | `model.isLoaded()` | Semak jika model sedang dimuat |
| **Katalog** | `catalog.getModels()` | Senaraikan semua model tersedia |
| **Katalog** | `catalog.getCachedModels()` | Senaraikan model yang dimuat turun |
| **Katalog** | `catalog.getLoadedModels()` | Senaraikan model yang sedang dimuat |
| **Katalog** | `catalog.updateModels()` | Segarkan katalog daripada perkhidmatan |
| **Perkhidmatan** | `manager.stopWebService()` | Hentikan perkhidmatan web Foundry Local |

### C# (v0.8.0+)

| Kategori | Kaedah | Penerangan |
|----------|--------|------------|
| **Inisiasi** | `FoundryLocalManager.CreateAsync(config, logger)` | Inisialisasi pengurus |
| **Inisiasi** | `FoundryLocalManager.Instance` | Akses singleton |
| **Katalog** | `manager.GetCatalogAsync()` | Dapatkan katalog |
| **Katalog** | `catalog.ListModelsAsync()` | Senaraikan semua model |
| **Katalog** | `catalog.GetModelAsync(alias)` | Dapatkan model tertentu |
| **Katalog** | `catalog.GetCachedModelsAsync()` | Senaraikan model dalam cache |
| **Katalog** | `catalog.GetLoadedModelsAsync()` | Senaraikan model yang dimuat |
| **Model** | `model.DownloadAsync(progress?)` | Muat turun model |
| **Model** | `model.LoadAsync()` | Muat model |
| **Model** | `model.UnloadAsync()` | Buang muatan model |
| **Model** | `model.SelectVariant(variant)` | Pilih varian perkakasan |
| **Model** | `model.GetChatClientAsync()` | Dapatkan klien sembang asli |
| **Model** | `model.GetAudioClientAsync()` | Dapatkan klien transkripsi audio |
| **Model** | `model.GetPathAsync()` | Dapatkan laluan fail lokal |
| **Katalog** | `catalog.GetModelVariantAsync(alias, variant)` | Dapatkan varian perkakasan spesifik |
| **Katalog** | `catalog.UpdateModelsAsync()` | Segarkan katalog |
| **Pelayan** | `manager.StartWebServerAsync()` | Mulakan pelayan web REST |
| **Pelayan** | `manager.StopWebServerAsync()` | Hentikan pelayan web REST |
| **Konfigurasi** | `config.ModelCacheDir` | Lokasi direktori cache |

---

## Pengajaran Utama

| Konsep | Apa Yang Anda Pelajari |
|---------|-----------------------|
| **SDK vs CLI** | SDK menyediakan kawalan berprogram - penting untuk aplikasi |
| **Rangka Kawalan** | SDK mengurus perkhidmatan, model, dan caching |
| **Port Dinamik** | Sentiasa gunakan `manager.endpoint` (Python) atau `manager.urls[0]` (JS/C#) - jangan kodkan port secara tetap |
| **Alias** | Gunakan alias untuk pemilihan model optimum perkakasan secara automatik |
| **Mula-mula cepat** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Reka bentuk semula C#** | v0.8.0+ adalah berdikari - tiada CLI diperlukan pada mesin pengguna akhir |
| **Kitaran hayat model** | Katalog → Muat turun → Muat → Guna → Nyahmuat |
| **FoundryModelInfo** | Metadata kaya: tugasan, peranti, saiz, lesen, sokongan alat panggilan |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) untuk penggunaan tanpa OpenAI |
| **Variasi** | Model mempunyai variasi khusus perkakasan (CPU, GPU, NPU); dipilih secara automatik |
| **Naik taraf** | Python: `is_model_upgradeable()` + `upgrade_model()` untuk memastikan model sentiasa terkini |
| **Segarkan katalog** | `refresh_catalog()` (Python) / `updateModels()` (JS) untuk mencari model baru |

---

## Sumber

| Sumber | Pautan |
|----------|------|
| Rujukan SDK (semua bahasa) | [Microsoft Learn - Rujukan Foundry Local SDK](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrasi dengan SDK inferens | [Microsoft Learn - Integrasi SDK Inferens](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Rujukan API SDK C# | [Rujukan API Foundry Local C#](https://aka.ms/fl-csharp-api-ref) |
| Contoh SDK C# | [GitHub - Contoh Foundry Local SDK](https://aka.ms/foundrylocalSDK) |
| Laman web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Langkah Seterusnya

Teruskan ke [Bahagian 3: Menggunakan SDK dengan OpenAI](part3-sdk-and-apis.md) untuk menyambungkan SDK ke perpustakaan klien OpenAI dan membina aplikasi penyempurnaan perbualan pertama anda.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Penafian**:  
Dokumen ini telah diterjemahkan menggunakan perkhidmatan terjemahan AI [Co-op Translator](https://github.com/Azure/co-op-translator). Walaupun kami berusaha untuk ketepatan, sila ambil maklum bahawa terjemahan automatik mungkin mengandungi kesilapan atau ketidaktepatan. Dokumen asal dalam bahasa asalnya harus dianggap sebagai sumber yang sahih. Untuk maklumat penting, terjemahan profesional oleh manusia adalah disyorkan. Kami tidak bertanggungjawab atas sebarang salah faham atau tafsiran yang salah yang timbul daripada penggunaan terjemahan ini.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->