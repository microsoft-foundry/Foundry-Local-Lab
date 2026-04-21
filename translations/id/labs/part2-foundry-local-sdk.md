![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bagian 2: Pendalaman Foundry Local SDK

> **Tujuan:** Menguasai Foundry Local SDK untuk mengelola model, layanan, dan cache secara programatik - serta memahami mengapa SDK adalah pendekatan yang direkomendasikan dibandingkan CLI untuk membangun aplikasi.

## Ikhtisar

Di Bagian 1 Anda sudah menggunakan **Foundry Local CLI** untuk mengunduh dan menjalankan model secara interaktif. CLI bagus untuk eksplorasi, tetapi saat membangun aplikasi nyata Anda memerlukan **kontrol programatik**. Foundry Local SDK memberikan itu - mengelola **control plane** (memulai layanan, menemukan model, mengunduh, memuat) sehingga kode aplikasi Anda dapat fokus pada **data plane** (mengirim prompt, menerima hasil).

Lab ini mengajarkan semua permukaan API SDK dalam Python, JavaScript, dan C#. Pada akhir lab, Anda akan memahami setiap metode yang tersedia dan kapan menggunakan masing-masing.

## Tujuan Pembelajaran

Pada akhir lab ini Anda akan bisa:

- Menjelaskan mengapa SDK lebih disukai daripada CLI untuk pengembangan aplikasi
- Menginstal Foundry Local SDK untuk Python, JavaScript, atau C#
- Menggunakan `FoundryLocalManager` untuk memulai layanan, mengelola model, dan mengkueri katalog
- Mendaftar, mengunduh, memuat, dan membongkar model secara programatik
- Memeriksa metadata model menggunakan `FoundryModelInfo`
- Memahami perbedaan antara katalog, cache, dan model yang dimuat
- Menggunakan konstruktor bootstrap (Python) dan pola `create()` + katalog (JavaScript)
- Memahami desain ulang SDK C# dan API berorientasi objeknya

---

## Prasyarat

| Persyaratan | Rincian |
|-------------|---------|
| **Foundry Local CLI** | Terinstal dan ada di `PATH` Anda ([Bagian 1](part1-getting-started.md)) |
| **Runtime bahasa** | **Python 3.9+** dan/atau **Node.js 18+** dan/atau **.NET 9.0+** |

---

## Konsep: SDK vs CLI - Mengapa Menggunakan SDK?

| Aspek | CLI (perintah `foundry`) | SDK (`foundry-local-sdk`) |
|--------|---------------------------|---------------------------|
| **Penggunaan** | Eksplorasi, pengujian manual | Integrasi aplikasi |
| **Manajemen layanan** | Manual: `foundry service start` | Otomatis: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Penemuan port** | Dibaca dari output CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Pengunduhan model** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Penanganan kesalahan** | Kode keluar, stderr | Eksepsi, error bertipe |
| **Otomasi** | Skrip shell | Integrasi bahasa asli |
| **Penerapan** | Memerlukan CLI di mesin pengguna | SDK C# bisa berdiri sendiri (tanpa CLI) |

> **Wawasan utama:** SDK menangani seluruh siklus hidup: memulai layanan, memeriksa cache, mengunduh model yang belum ada, memuatnya, dan menemukan endpoint, dalam beberapa baris kode. Aplikasi Anda tidak perlu mengurai output CLI atau mengelola subproses.

---

## Latihan Lab

### Latihan 1: Instal SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Verifikasi instalasi:

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

Verifikasi instalasi:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Ada dua paket NuGet:

| Paket | Platform | Deskripsi |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Lintas platform | Bekerja di Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Khusus Windows | Menambahkan akselerasi hardware WinML; mengunduh dan menginstal penyedia eksekusi plugin (misal QNN untuk Qualcomm NPU) |

**Setup Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Edit file `.csproj`:

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

> **Catatan:** Di Windows, paket WinML adalah superset yang mencakup SDK dasar plus penyedia eksekusi QNN. Di Linux/macOS, SDK dasar yang digunakan. TFM kondisional dan referensi paket menjaga proyek tetap lintas platform sepenuhnya.

Buat `nuget.config` di root proyek:

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

Pulihkan paket:

```bash
dotnet restore
```

</details>

---

### Latihan 2: Mulai Layanan dan Daftar Katalog

Hal pertama yang dilakukan aplikasi adalah memulai layanan Foundry Local dan menemukan model yang tersedia.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Buat manajer dan mulai layanan
manager = FoundryLocalManager()
manager.start_service()

# Daftar semua model yang tersedia di katalog
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Metode Manajemen Layanan

| Metode | Tanda Tangan | Deskripsi |
|--------|--------------|-----------|
| `is_service_running()` | `() -> bool` | Periksa apakah layanan sedang berjalan |
| `start_service()` | `() -> None` | Mulai layanan Foundry Local |
| `service_uri` | `@property -> str` | URI dasar layanan |
| `endpoint` | `@property -> str` | Endpoint API (URI layanan + `/v1`) |
| `api_key` | `@property -> str` | Kunci API (dari env atau placeholder default) |

#### Python SDK - Metode Manajemen Katalog

| Metode | Tanda Tangan | Deskripsi |
|--------|--------------|-----------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Daftar semua model di katalog |
| `refresh_catalog()` | `() -> None` | Segarkan katalog dari layanan |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Dapatkan info model tertentu |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Buat manajer dan mulai layanan
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Telusuri katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Metode Manager

| Metode | Tanda Tangan | Deskripsi |
|--------|--------------|-----------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inisialisasi singleton SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Akses manager singleton |
| `manager.startWebService()` | `() => Promise<void>` | Mulai layanan web Foundry Local |
| `manager.urls` | `string[]` | Array URL dasar layanan |

#### JavaScript SDK - Metode Katalog dan Model

| Metode | Tanda Tangan | Deskripsi |
|--------|--------------|-----------|
| `manager.catalog` | `Catalog` | Akses katalog model |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Dapatkan objek model berdasarkan alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

SDK C# v0.8.0+ menggunakan arsitektur berorientasi objek dengan objek `Configuration`, `Catalog`, dan `Model`:

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

#### C# SDK - Kelas Kunci

| Kelas | Tujuan |
|-------|--------|
| `Configuration` | Set nama aplikasi, level log, direktori cache, URL server web |
| `FoundryLocalManager` | Titik masuk utama - dibuat lewat `CreateAsync()`, diakses lewat `.Instance` |
| `Catalog` | Telusuri, cari, dan dapatkan model dari katalog |
| `Model` | Mewakili model spesifik - unduh, muat, dapatkan klien |

#### C# SDK - Metode Manager dan Katalog

| Metode | Deskripsi |
|--------|-----------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inisialisasi manager |
| `FoundryLocalManager.Instance` | Akses singleton manager |
| `manager.GetCatalogAsync()` | Dapatkan katalog model |
| `catalog.ListModelsAsync()` | Daftar semua model yang tersedia |
| `catalog.GetModelAsync(alias: "alias")` | Dapatkan model spesifik berdasarkan alias |
| `catalog.GetCachedModelsAsync()` | Daftar model yang diunduh |
| `catalog.GetLoadedModelsAsync()` | Daftar model yang sedang dimuat |

> **Catatan Arsitektur C#:** SDK C# v0.8.0+ yang direvisi membuat aplikasi menjadi **mandiri**; tidak lagi membutuhkan Foundry Local CLI di mesin pengguna. SDK mengelola manajemen model dan inferensi secara native.

</details>

---

### Latihan 3: Unduh dan Muat Model

SDK memisahkan proses mengunduh (ke disk) dari memuat (ke memori). Ini memungkinkan Anda mengunduh model terlebih dahulu saat setup dan memuatnya sesuai permintaan.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Opsi A: Langkah manual satu per satu
manager = FoundryLocalManager()
manager.start_service()

# Periksa cache terlebih dahulu
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

# Opsi B: Bootstrap satu baris (disarankan)
# Lewatkan alias ke konstruktor - itu memulai layanan, mengunduh, dan memuat secara otomatis
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Metode Manajemen Model

| Metode | Tanda Tangan | Deskripsi |
|--------|--------------|-----------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Unduh model ke cache lokal |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Muat model ke server inferensi |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Bongkar model dari server |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Daftar semua model yang sedang dimuat |

#### Python - Metode Manajemen Cache

| Metode | Tanda Tangan | Deskripsi |
|--------|--------------|-----------|
| `get_cache_location()` | `() -> str` | Dapatkan path direktori cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Daftar semua model yang diunduh |

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

#### JavaScript - Metode Model

| Metode | Tanda Tangan | Deskripsi |
|--------|--------------|-----------|
| `model.isCached` | `boolean` | Apakah model sudah diunduh |
| `model.download()` | `() => Promise<void>` | Unduh model ke cache lokal |
| `model.load()` | `() => Promise<void>` | Muat ke server inferensi |
| `model.unload()` | `() => Promise<void>` | Bongkar dari server inferensi |
| `model.id` | `string` | Identifier unik model |

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

| Metode | Deskripsi |
|--------|-----------|
| `model.DownloadAsync(progress?)` | Unduh varian yang dipilih |
| `model.LoadAsync()` | Muat model ke memori |
| `model.UnloadAsync()` | Bongkar model |
| `model.SelectVariant(variant)` | Pilih varian tertentu (CPU/GPU/NPU) |
| `model.SelectedVariant` | Varian yang dipilih saat ini |
| `model.Variants` | Semua varian yang tersedia untuk model ini |
| `model.GetPathAsync()` | Dapatkan path file lokal |
| `model.GetChatClientAsync()` | Dapatkan klien chat native (tanpa perlu OpenAI SDK) |
| `model.GetAudioClientAsync()` | Dapatkan klien audio untuk transkripsi |

</details>

---

### Latihan 4: Periksa Metadata Model

Objek `FoundryModelInfo` berisi metadata kaya tentang setiap model. Memahami bidang ini membantu Anda memilih model yang tepat untuk aplikasi Anda.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Dapatkan info rinci tentang model tertentu
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

#### Bidang FoundryModelInfo

| Bidang | Tipe | Deskripsi |
|--------|-------|-----------|
| `alias` | string | Nama pendek (misal `phi-3.5-mini`) |
| `id` | string | Identifier unik model |
| `version` | string | Versi model |
| `task` | string | `chat-completions` atau `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, atau NPU |
| `execution_provider` | string | Backend runtime (CUDA, CPU, QNN, WebGPU, dll.) |
| `file_size_mb` | int | Ukuran di disk dalam MB |
| `supports_tool_calling` | bool | Apakah model mendukung pemanggilan fungsi/alat |
| `publisher` | string | Penerbit model |
| `license` | string | Nama lisensi |
| `uri` | string | URI model |
| `prompt_template` | dict/null | Template prompt, jika ada |

---

### Latihan 5: Kelola Siklus Hidup Model

Latih siklus hidup penuh: daftar → unduh → muat → gunakan → bongkar.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Model kecil untuk pengujian cepat

manager = FoundryLocalManager()
manager.start_service()

# 1. Periksa apa yang ada di katalog
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Periksa apa yang sudah diunduh
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Unduh sebuah model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Verifikasi bahwa model sudah ada di cache sekarang
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Muat model tersebut
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Periksa apa yang sudah dimuat
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Lepaskan muatan model tersebut
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

const alias = "qwen2.5-0.5b"; // Model kecil untuk pengujian cepat

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Dapatkan model dari katalog
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Unduh jika perlu
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Muat model
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Lepaskan model
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Latihan 6: Pola Memulai Cepat

Setiap bahasa menyediakan jalan pintas untuk memulai layanan dan memuat model dalam satu panggilan. Ini adalah **pola yang direkomendasikan** untuk sebagian besar aplikasi.

<details>
<summary><h3>🐍 Python - Bootstrap Konstruktor</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Berikan alias ke konstruktor - itu menangani segalanya:
# 1. Memulai layanan jika belum berjalan
# 2. Mengunduh model jika tidak ada dalam cache
# 3. Memuat model ke dalam server inferensi
manager = FoundryLocalManager("phi-3.5-mini")

# Siap digunakan segera
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Parameter `bootstrap` (default `True`) mengontrol perilaku ini. Setel `bootstrap=False` jika Anda ingin kontrol manual:

```python
# Mode manual - tidak ada yang terjadi secara otomatis
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() menangani semuanya:
// 1. Memulai layanan
// 2. Mendapatkan model dari katalog
// 3. Mengunduh jika perlu dan memuat model
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Siap digunakan segera
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

> **Catatan C#:** SDK C# menggunakan `Configuration` untuk mengontrol nama aplikasi, pencatatan, direktori cache, dan bahkan menetapkan port server web tertentu. Ini membuatnya menjadi yang paling dapat dikonfigurasi dari ketiga SDK.

</details>

---

### Latihan 7: Native ChatClient (Tidak Perlu OpenAI SDK)

SDK JavaScript dan C# menyediakan metode kenyamanan `createChatClient()` yang mengembalikan client chat native — tidak perlu menginstal atau mengonfigurasi OpenAI SDK secara terpisah.

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

// Buat ChatClient langsung dari model — tidak perlu impor OpenAI
const chatClient = model.createChatClient();

// completeChat mengembalikan objek respons yang kompatibel dengan OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming menggunakan pola callback
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` juga mendukung pemanggilan alat — berikan alat sebagai argumen kedua:

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

> **Kapan menggunakan pola mana:**
> - **`createChatClient()`** — Prototyping cepat, lebih sedikit ketergantungan, kode lebih sederhana
> - **OpenAI SDK** — Kontrol penuh atas parameter (temperature, top_p, stop tokens, dll.), lebih baik untuk aplikasi produksi

---

### Latihan 8: Varian Model dan Pemilihan Perangkat Keras

Model dapat memiliki beberapa **varian** yang dioptimalkan untuk perangkat keras yang berbeda. SDK secara otomatis memilih varian terbaik, tapi Anda juga bisa memeriksa dan memilih secara manual.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Daftar semua varian yang tersedia
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK secara otomatis memilih varian terbaik untuk perangkat keras Anda
// Untuk mengganti, gunakan selectVariant():
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

Di Python, SDK secara otomatis memilih varian terbaik berdasarkan perangkat keras. Gunakan `get_model_info()` untuk melihat apa yang dipilih:

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

Beberapa model memiliki varian yang dioptimalkan NPU untuk perangkat dengan Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Model | Varian NPU Tersedia |
|-------|:-------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Tip:** Pada perangkat keras yang mendukung NPU, SDK secara otomatis memilih varian NPU saat tersedia. Anda tidak perlu mengubah kode Anda. Untuk proyek C# di Windows, tambahkan paket NuGet `Microsoft.AI.Foundry.Local.WinML` untuk mengaktifkan penyedia eksekusi QNN — QNN disediakan sebagai plugin EP melalui WinML.

---

### Latihan 9: Peningkatan Model dan Penyegaran Katalog

Katalog model diperbarui secara berkala. Gunakan metode ini untuk memeriksa dan menerapkan pembaruan.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Muat ulang katalog untuk mendapatkan daftar model terbaru
manager.refresh_catalog()

# Periksa apakah model yang di-cache memiliki versi yang lebih baru tersedia
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

// Segarkan katalog untuk mengambil daftar model terbaru
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Daftar semua model yang tersedia setelah penyegaran
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Latihan 10: Bekerja dengan Model Penalaran

Model **phi-4-mini-reasoning** menyertakan penalaran rantai pemikiran. Model ini membungkus pemikiran internalnya di tag `<think>...</think>` sebelum menghasilkan jawaban akhirnya. Ini berguna untuk tugas yang memerlukan logika multi-langkah, matematika, atau pemecahan masalah.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning berukuran ~4,6 GB
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

// Mengurai pemikiran rantai-pemikiran
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Kapan menggunakan model penalaran:**
> - Masalah matematika dan logika
> - Tugas perencanaan multi-langkah
> - Pembuatan kode kompleks
> - Tugas di mana menampilkan proses kerja meningkatkan akurasi
>
> **Pertukaran:** Model penalaran menghasilkan lebih banyak token (bagian `<think>`) dan lebih lambat. Untuk tanya jawab sederhana, model standar seperti phi-3.5-mini lebih cepat.

---

### Latihan 11: Memahami Alias dan Pemilihan Perangkat Keras

Saat Anda melewatkan **alias** (seperti `phi-3.5-mini`) alih-alih ID model lengkap, SDK secara otomatis memilih varian terbaik untuk perangkat keras Anda:

| Perangkat Keras | Penyedia Eksekusi yang Dipilih |
|-----------------|-------------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (melalui plugin WinML) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Perangkat apa pun (fallback) | `CPUExecutionProvider` atau `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Alias menyelesaikan ke varian terbaik untuk perangkat keras ANDA
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tip:** Selalu gunakan alias dalam kode aplikasi Anda. Saat Anda menyebarkan ke mesin pengguna, SDK memilih varian optimal saat runtime - CUDA pada NVIDIA, QNN pada Qualcomm, CPU di tempat lain.

---

### Latihan 12: Opsi Konfigurasi C#

Kelas `Configuration` di SDK C# menyediakan kontrol terperinci atas runtime:

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

| Properti | Default | Deskripsi |
|----------|---------|-----------|
| `AppName` | (wajib) | Nama aplikasi Anda |
| `LogLevel` | `Information` | Verbositas pencatatan |
| `Web.Urls` | (dinamis) | Menetapkan port spesifik untuk server web |
| `AppDataDir` | Default OS | Direktori dasar untuk data aplikasi |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Tempat penyimpanan model |
| `LogsDir` | `{AppDataDir}/logs` | Tempat penulisan log |

---

### Latihan 13: Penggunaan di Browser (JavaScript Saja)

SDK JavaScript menyertakan versi yang kompatibel dengan browser. Di browser, Anda harus memulai layanan secara manual melalui CLI dan menentukan URL host:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Mulai layanan secara manual terlebih dahulu:
//   layanan foundry mulai
// Kemudian gunakan URL dari keluaran CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Telusuri katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Keterbatasan Browser:** Versi browser **tidak** mendukung `startWebService()`. Anda harus memastikan layanan Foundry Local sudah berjalan sebelum menggunakan SDK di browser.

---

## Referensi API Lengkap

### Python

| Kategori | Metode | Deskripsi |
|----------|--------|-----------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Membuat manajer; opsional bootstrap dengan model |
| **Service** | `is_service_running()` | Memeriksa apakah layanan berjalan |
| **Service** | `start_service()` | Memulai layanan |
| **Service** | `endpoint` | URL endpoint API |
| **Service** | `api_key` | Kunci API |
| **Catalog** | `list_catalog_models()` | Daftar semua model tersedia |
| **Catalog** | `refresh_catalog()` | Segarkan katalog |
| **Catalog** | `get_model_info(alias_or_model_id)` | Dapatkan metadata model |
| **Cache** | `get_cache_location()` | Jalur direktori cache |
| **Cache** | `list_cached_models()` | Daftar model yang diunduh |
| **Model** | `download_model(alias_or_model_id)` | Unduh model |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Muat model |
| **Model** | `unload_model(alias_or_model_id)` | Lepas muat model |
| **Model** | `list_loaded_models()` | Daftar model yang dimuat |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Periksa apakah versi lebih baru tersedia |
| **Model** | `upgrade_model(alias_or_model_id)` | Tingkatkan model ke versi terbaru |
| **Service** | `httpx_client` | Klien HTTPX yang telah dikonfigurasi untuk panggilan API langsung |

### JavaScript

| Kategori | Metode | Deskripsi |
|----------|--------|-----------|
| **Init** | `FoundryLocalManager.create(options)` | Inisialisasi singleton SDK |
| **Init** | `FoundryLocalManager.instance` | Mengakses manajer singleton |
| **Service** | `manager.startWebService()` | Memulai layanan web |
| **Service** | `manager.urls` | Array URL dasar untuk layanan |
| **Catalog** | `manager.catalog` | Mengakses katalog model |
| **Catalog** | `catalog.getModel(alias)` | Dapatkan objek model berdasarkan alias (mengembalikan Promise) |
| **Model** | `model.isCached` | Apakah model sudah diunduh |
| **Model** | `model.download()` | Unduh model |
| **Model** | `model.load()` | Muat model |
| **Model** | `model.unload()` | Lepas muat model |
| **Model** | `model.id` | Identifier unik model |
| **Model** | `model.alias` | Alias model |
| **Model** | `model.createChatClient()` | Dapatkan client chat native (tidak perlu OpenAI SDK) |
| **Model** | `model.createAudioClient()` | Dapatkan client audio untuk transkripsi |
| **Model** | `model.removeFromCache()` | Hapus model dari cache lokal |
| **Model** | `model.selectVariant(variant)` | Pilih varian perangkat keras tertentu |
| **Model** | `model.variants` | Array varian tersedia untuk model ini |
| **Model** | `model.isLoaded()` | Periksa apakah model sedang dimuat |
| **Catalog** | `catalog.getModels()` | Daftar semua model tersedia |
| **Catalog** | `catalog.getCachedModels()` | Daftar model yang diunduh |
| **Catalog** | `catalog.getLoadedModels()` | Daftar model yang sedang dimuat |
| **Catalog** | `catalog.updateModels()` | Segarkan katalog dari layanan |
| **Service** | `manager.stopWebService()` | Hentikan layanan web Foundry Local |

### C# (v0.8.0+)

| Kategori | Metode | Deskripsi |
|----------|--------|-----------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Inisialisasi manajer |
| **Init** | `FoundryLocalManager.Instance` | Mengakses singleton |
| **Catalog** | `manager.GetCatalogAsync()` | Dapatkan katalog |
| **Catalog** | `catalog.ListModelsAsync()` | Daftar semua model |
| **Catalog** | `catalog.GetModelAsync(alias)` | Dapatkan model spesifik |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Daftar model yang di-cache |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Daftar model yang dimuat |
| **Model** | `model.DownloadAsync(progress?)` | Unduh model |
| **Model** | `model.LoadAsync()` | Muat model |
| **Model** | `model.UnloadAsync()` | Lepas muat model |
| **Model** | `model.SelectVariant(variant)` | Pilih varian perangkat keras |
| **Model** | `model.GetChatClientAsync()` | Dapatkan client chat native |
| **Model** | `model.GetAudioClientAsync()` | Dapatkan client transkripsi audio |
| **Model** | `model.GetPathAsync()` | Dapatkan jalur file lokal |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Dapatkan varian perangkat keras spesifik |
| **Catalog** | `catalog.UpdateModelsAsync()` | Segarkan katalog |
| **Server** | `manager.StartWebServerAsync()` | Mulai server web REST |
| **Server** | `manager.StopWebServerAsync()` | Hentikan server web REST |
| **Config** | `config.ModelCacheDir` | Direktori cache |

---

## Poin Penting

| Konsep | Apa yang Anda Pelajari |
|---------|------------------------|
| **SDK vs CLI** | SDK menyediakan kontrol programatik — penting untuk aplikasi |
| **Control plane** | SDK mengelola layanan, model, dan caching |
| **Port dinamis** | Selalu gunakan `manager.endpoint` (Python) atau `manager.urls[0]` (JS/C#) — jangan hardcode port |
| **Alias** | Gunakan alias untuk pemilihan model optimal perangkat keras secara otomatis |
| **Mulai cepat** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Redesain C#** | v0.8.0+ berdiri sendiri - tidak perlu CLI di mesin pengguna akhir |
| **Siklus hidup model** | Katalog → Unduh → Muat → Gunakan → Bebaskan |
| **FoundryModelInfo** | Metadata kaya: tugas, perangkat, ukuran, lisensi, dukungan pemanggilan alat |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) untuk penggunaan tanpa OpenAI |
| **Varian** | Model memiliki varian spesifik perangkat keras (CPU, GPU, NPU); dipilih otomatis |
| **Peningkatan** | Python: `is_model_upgradeable()` + `upgrade_model()` untuk menjaga model tetap terbaru |
| **Penyegaran katalog** | `refresh_catalog()` (Python) / `updateModels()` (JS) untuk menemukan model baru |

---

## Sumber Daya

| Sumber Daya | Tautan |
|-------------|--------|
| Referensi SDK (semua bahasa) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrasi dengan SDK inferensi | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Referensi API SDK C# | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| Contoh SDK C# | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Situs web Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Langkah Selanjutnya

Lanjutkan ke [Bagian 3: Menggunakan SDK dengan OpenAI](part3-sdk-and-apis.md) untuk menghubungkan SDK dengan pustaka klien OpenAI dan membangun aplikasi chat completion pertama Anda.