![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bölüm 2: Foundry Local SDK Derinlemesine İnceleme

> **Amaç:** Modelleri, servisleri ve önbelleği programlı olarak yönetmek için Foundry Local SDK'yı ustalıkla kullanmak - ve SDK'nın uygulama geliştirmek için neden CLI yerine önerilen yaklaşım olduğunu anlamak.

## Genel Bakış

Bölüm 1'de **Foundry Local CLI** kullanarak modelleri indirip etkileşimli şekilde çalıştırdınız. CLI keşif için çok iyidir, ancak gerçek uygulamalar geliştirdiğinizde **programatik kontrol** gerekir. Foundry Local SDK size bunu sağlar - **kontrol düzlemini** yönetir (servisi başlatma, modelleri keşfetme, indirme, yükleme) böylece uygulama kodunuz **veri düzlemi**ne (istek gönderme, tamamlamaları alma) odaklanabilir.

Bu laboratuvar Python, JavaScript ve C# üzerinden tam SDK API yüzeyini öğretir. Sonunda hangi yöntemin ne zaman kullanılacağını ve mevcut tüm metodları anlayacaksınız.

## Öğrenme Hedefleri

Bu laboratuvarın sonunda şunları yapabileceksiniz:

- Uygulama geliştirme için CLI yerine SDK'nın neden tercih edildiğini açıklamak
- Python, JavaScript veya C# için Foundry Local SDK'yı kurmak
- `FoundryLocalManager` kullanarak servisi başlatmak, modelleri yönetmek ve kataloğu sorgulamak
- Modelleri programlı olarak listelemek, indirmek, yüklemek ve boşaltmak
- `FoundryModelInfo` kullanarak model meta verilerini incelemek
- Katalog, önbellek ve yüklü modeller arasındaki farkı anlamak
- Yapıcı bootstrap (Python) ve `create()` + katalog deseni (JavaScript) kullanmak
- C# SDK yeniden tasarımını ve onun nesne yönelimli API'sini anlamak

---

## Ön Koşullar

| Gereksinim | Detaylar |
|-------------|---------|
| **Foundry Local CLI** | Kurulu ve `PATH` üzerinde ([Bölüm 1](part1-getting-started.md)) |
| **Programlama ortamı** | **Python 3.9+** ve/veya **Node.js 18+** ve/veya **.NET 9.0+** |

---

## Kavram: SDK vs CLI - Neden SDK Kullanılır?

| Özellik | CLI (`foundry` komutu) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Kullanım alanı** | Keşif, manuel test | Uygulama entegrasyonu |
| **Servis yönetimi** | Manuel: `foundry service start` | Otomatik: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Port keşfi** | CLI çıktısından oku | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Model indirme** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Hata yönetimi** | Çıkış kodları, stderr | İstisnalar, tipli hatalar |
| **Otomasyon** | Kabuk scriptleri | Yerel dil entegrasyonu |
| **Dağıtım** | Son kullanıcı makinasında CLI gerekli | C# SDK kendi içinde tamamlayıcı (CLI gerekmez) |

> **Temel çıkarım:** SDK tüm yaşam döngüsünü birkaç satır kodla yönetir: servisi başlatma, önbelleği kontrol etme, eksik modelleri indirme, yükleme ve uç noktayı keşfetme. Uygulamanız CLI çıktısını ayrıştırmak veya alt süreçleri yönetmek zorunda kalmaz.

---

## Laboratuvar Egzersizleri

### Egzersiz 1: SDK'yı Kurun

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Kurulumu doğrulayın:

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

Kurulumu doğrulayın:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

İki NuGet paketi bulunmaktadır:

| Paket | Platform | Açıklama |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Platformlar arası | Windows, Linux, macOS üzerinde çalışır |
| `Microsoft.AI.Foundry.Local.WinML` | Sadece Windows | WinML donanım hızlandırması ekler; eklenti yürütme sağlayıcılarını indirir ve kurar (örnek: Qualcomm NPU için QNN) |

**Windows kurulumu:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

`.csproj` dosyasını düzenleyin:

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

> **Not:** Windows'ta WinML paketi temel SDK'ya ek olarak QNN yürütme sağlayıcısını içerir. Linux/macOS'ta temel SDK kullanılır. Koşullu hedef framework ve paket referansları projeyi tamamen platformlar arası kılar.

Proje kökünde `nuget.config` oluşturun:

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

Paketleri geri yükleyin:

```bash
dotnet restore
```

</details>

---

### Egzersiz 2: Servisi Başlatın ve Kataloğu Listeleyin

Her uygulamanın yaptığı ilk iş Foundry Local servisini başlatmak ve hangi modellerin mevcut olduğunu keşfetmektir.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Bir yönetici oluşturun ve servisi başlatın
manager = FoundryLocalManager()
manager.start_service()

# Katalogda mevcut tüm modelleri listeleyin
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Servis Yönetimi Metodları

| Metod | İmza | Açıklama |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Servisin çalışıp çalışmadığını kontrol et |
| `start_service()` | `() -> None` | Foundry Local servisini başlat |
| `service_uri` | `@property -> str` | Temel servis URI'si |
| `endpoint` | `@property -> str` | API uç noktası (servis URI + `/v1`) |
| `api_key` | `@property -> str` | API anahtarı (env veya varsayılan yer tutucu) |

#### Python SDK - Katalog Yönetimi Metodları

| Metod | İmza | Açıklama |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Kataloğundaki tüm modelleri listele |
| `refresh_catalog()` | `() -> None` | Kataloğu servisten yenile |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Belirli bir modelin bilgilerini al |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Bir yönetici oluşturun ve servisi başlatın
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Kataloğu gözden geçirin
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Yönetici Metodları

| Metod | İmza | Açıklama |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK tekil örneğini başlat |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Tekil yöneticiye erişim |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local web servisini başlat |
| `manager.urls` | `string[]` | Servisin temel URL dizisi |

#### JavaScript SDK - Katalog ve Model Metodları

| Metod | İmza | Açıklama |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Model kataloğuna erişim |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Bir takma ada göre model nesnesi al |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK v0.8.0+ nesne yönelimli mimari kullanır: `Configuration`, `Catalog` ve `Model` nesneleri:

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

#### C# SDK - Temel Sınıflar

| Sınıf | Amaç |
|-------|---------|
| `Configuration` | Uygulama adı, günlük seviyesi, önbellek dizini, web sunucu URL'leri ayarı |
| `FoundryLocalManager` | Ana giriş noktası - `CreateAsync()` ile oluşturulur, `.Instance` ile erişilir |
| `Catalog` | Katalogda gezinme, arama ve modelleri alma |
| `Model` | Belirli bir modeli temsil eder - indir, yükle, istemcileri al |

#### C# SDK - Yönetici ve Katalog Metodları

| Metod | Açıklama |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Yöneticiyi başlatır |
| `FoundryLocalManager.Instance` | Tekil yöneticiye erişim |
| `manager.GetCatalogAsync()` | Model kataloğunu al |
| `catalog.ListModelsAsync()` | Kullanılabilir tüm modelleri listele |
| `catalog.GetModelAsync(alias: "alias")` | Belirli bir modeli takma ada göre al |
| `catalog.GetCachedModelsAsync()` | İndirilen modelleri listele |
| `catalog.GetLoadedModelsAsync()` | Şu anda yüklü modelleri listele |

> **C# Mimari Notu:** C# SDK v0.8.0+ yeniden tasarımı uygulamayı **kendi kendine yeten** hale getirir; son kullanıcı makinesinde Foundry Local CLI gerekmez. SDK model yönetimini ve çıkarımı yerel olarak yapar.

</details>

---

### Egzersiz 3: Bir Model İndirin ve Yükleyin

SDK indirmeyi (diske) ve yüklemeyi (belleğe) ayırır. Böylece modelleri kurulum sırasında önceden indirip, isteğe bağlı yükleyebilirsiniz.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Seçenek A: El ile adım adım
manager = FoundryLocalManager()
manager.start_service()

# Önce önbelleği kontrol et
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

# Seçenek B: Tek satırlık bootstrap (önerilir)
# Yapıcıya alias geçir - servis başlar, indirir ve otomatik olarak yükler
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Model Yönetimi Metodları

| Metod | İmza | Açıklama |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Yerel önbelleğe model indir |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | İstemd dışı sunucuya modeli yükle |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Modeli sunucudan boşalt |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Halihazırda yüklü modelleri listele |

#### Python - Önbellek Yönetimi Metodları

| Metod | İmza | Açıklama |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Önbellek dizini yolunu al |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | İndirilen tüm modelleri listele |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Adım adım yaklaşım
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

#### JavaScript - Model Metodları

| Metod | İmza | Açıklama |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Modelin önceden indirilip indirilmediği |
| `model.download()` | `() => Promise<void>` | Modeli yerel önbelleğe indir |
| `model.load()` | `() => Promise<void>` | İstemd dışı sunucuya yükle |
| `model.unload()` | `() => Promise<void>` | İstemd dışı sunucudan boşalt |
| `model.id` | `string` | Modelin benzersiz tanımlayıcısı |

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

#### C# - Model Metodları

| Metod | Açıklama |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Seçilen varyantı indir |
| `model.LoadAsync()` | Modeli belleğe yükle |
| `model.UnloadAsync()` | Modeli boşalt |
| `model.SelectVariant(variant)` | Belirli bir varyantı seç (CPU/GPU/NPU) |
| `model.SelectedVariant` | Şu anda seçili varyant |
| `model.Variants` | Bu model için mevcut tüm varyantlar |
| `model.GetPathAsync()` | Yerel dosya yolunu al |
| `model.GetChatClientAsync()` | Yerel sohbet istemcisi al (OpenAI SDK gerekmez) |
| `model.GetAudioClientAsync()` | Transkripsiyon için ses istemcisi al |

</details>

---

### Egzersiz 4: Model Meta Verilerini İnceleyin

`FoundryModelInfo` nesnesi her model hakkında zengin meta veri içerir. Bu alanları anlamak uygulamanız için doğru modeli seçmenize yardımcı olur.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Belirli bir model hakkında detaylı bilgi alın
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

#### FoundryModelInfo Alanları

| Alan | Tür | Açıklama |
|-------|------|-------------|
| `alias` | string | Kısa isim (ör. `phi-3.5-mini`) |
| `id` | string | Benzersiz model kimliği |
| `version` | string | Model versiyonu |
| `task` | string | `chat-completions` veya `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, veya NPU |
| `execution_provider` | string | Çalışma zamanı altyapısı (CUDA, CPU, QNN, WebGPU, vb.) |
| `file_size_mb` | int | Diskte MB cinsinden boyut |
| `supports_tool_calling` | bool | Modelin fonksiyon/araç çağırmayı destekleyip desteklemediği |
| `publisher` | string | Modeli yayınlayan |
| `license` | string | Lisans adı |
| `uri` | string | Model URI'si |
| `prompt_template` | dict/null | Prompt şablonu, varsa |

---

### Egzersiz 5: Model Yaşam Döngüsünü Yönetin

Tam yaşam döngüsünü uygulayın: listele → indir → yükle → kullan → boşalt.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Hızlı test için küçük model

manager = FoundryLocalManager()
manager.start_service()

# 1. Kataloğda ne olduğunu kontrol et
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Zaten indirilenleri kontrol et
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Bir modeli indir
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Şimdi önbellekte olduğunu doğrula
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Onu yükle
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Nelerin yüklendiğini kontrol et
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Onu boşalt
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

const alias = "qwen2.5-0.5b"; // Hızlı test için küçük model

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Modeli katalogdan al
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Gerekirse indir
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Yükle
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Yüklemeyi kaldır
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Egzersiz 6: Hızlı Başlangıç Desenleri

Her dil, hizmeti başlatmak ve bir modeli tek bir çağrıda yüklemek için bir kısayol sağlar. Bunlar çoğu uygulama için **önerilen desenlerdir**.

<details>
<summary><h3>🐍 Python - Kurucu Başlatıcı</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Yapıcıya bir takma ad verin - her şeyi halleder:
# 1. Hizmet çalışmıyorsa başlatır
# 2. Model önbelleğe alınmamışsa indirir
# 3. Modeli çıkarım sunucusuna yükler
manager = FoundryLocalManager("phi-3.5-mini")

# Hemen kullanıma hazır
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

`bootstrap` parametresi (varsayılan `True`), bu davranışı kontrol eder. Manuel kontrol istiyorsanız `bootstrap=False` olarak ayarlayın:

```python
# Manuel mod - hiçbir şey otomatik olarak gerçekleşmez
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() her şeyi halleder:
// 1. Servisi başlatır
// 2. Katalogdan modeli alır
// 3. Gerekirse indirir ve modeli yükler
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Hemen kullanıma hazır
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

> **C# Notu:** C# SDK, uygulama adı, günlükleme, önbellek dizinleri ve hatta belirli bir web sunucu portunu sabitlemeyi kontrol etmek için `Configuration` kullanır. Bu, üç SDK arasında en yapılandırılabilir olanıdır.

</details>

---

### Egzersiz 7: Yerel ChatClient (Ayrı OpenAI SDK'sı Gerekmez)

JavaScript ve C# SDK'ları, OpenAI SDK'sını ayrı kurmaya veya yapılandırmaya gerek kalmadan yerel bir sohbet istemcisi döndüren `createChatClient()` kolaylık metodunu sağlar.

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

// Modelden doğrudan bir ChatClient oluşturun — OpenAI içe aktarımına gerek yok
const chatClient = model.createChatClient();

// completeChat, OpenAI ile uyumlu bir yanıt nesnesi döner
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Akış, bir geri arama deseni kullanır
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` ayrıca araç çağrısını da destekler — araçları ikinci argüman olarak iletin:

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

> **Hangi deseni ne zaman kullanmalı:**
> - **`createChatClient()`** — Hızlı prototipleme, daha az bağımlılık, daha basit kod
> - **OpenAI SDK** — Tam parametre kontrolü (sıcaklık, top_p, durdurma tokenları vb.), üretim uygulamaları için daha iyi

---

### Egzersiz 8: Model Varyantları ve Donanım Seçimi

Modeller farklı donanımlar için optimize edilmiş birden fazla **varyant** içerebilir. SDK en iyi varyantı otomatik olarak seçer, ancak siz elle inceleyip seçebilirsiniz.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Tüm mevcut varyantları listele
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK, donanımınız için en iyi varyantı otomatik olarak seçer
// Geçersiz kılmak için selectVariant() kullanın:
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

Python'da SDK donanıma göre en iyi varyantı otomatik seçer. Seçilen varyantı görmek için `get_model_info()` kullanın:

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

#### NPU Varyantlarına Sahip Modeller

Bazı modeller, Sinir İşleme Birimleri (Qualcomm Snapdragon, Intel Core Ultra) içeren cihazlar için NPU-optimise varyantlara sahiptir:

| Model | NPU Varyantı Mevcut |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **İpucu:** NPU destekli donanımlarda, SDK mevcutsa otomatik olarak NPU varyantını seçer. Kodunuzu değiştirmenize gerek yoktur. Windows için C# projelerinde, QNN yürütme sağlayıcısını etkinleştirmek için `Microsoft.AI.Foundry.Local.WinML` NuGet paketini ekleyin — QNN, WinML üzerinden eklenti EP olarak sunulur.

---

### Egzersiz 9: Model Yükseltmeleri ve Katalog Yenileme

Model kataloğu periyodik olarak güncellenir. Güncellemeleri kontrol etmek ve uygulamak için bu yöntemleri kullanın.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# En son model listesi için kataloğu yenileyin
manager.refresh_catalog()

# Önbelleğe alınmış bir modelin daha yeni bir sürümü olup olmadığını kontrol edin
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

// En son model listesini almak için kataloğu yenile
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Yenileme sonrası tüm mevcut modelleri listele
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Egzersiz 10: Akıl Yürütme Modelleriyle Çalışmak

**phi-4-mini-reasoning** modeli, düşünce zinciri akıl yürütmesini içerir. Son cevabını üretmeden önce iç düşüncesini `<think>...</think>` etiketleri içinde sarar. Bu, çok adımlı mantık, matematik veya problem çözme gerektiren görevler için faydalıdır.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning yaklaşık 4,6 GB'dir
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Model düşüncelerini <think>...</think> etiketleri içinde sarar
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

// Düşünce zinciri mantığını çözümle
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Akıl yürütme modelleri ne zaman kullanılmalı:**
> - Matematik ve mantık problemleri
> - Çok adımlı planlama görevleri
> - Karmaşık kod oluşturma
> - Çalışmayı göstermek doğruluğu artıran görevler
>
> **Takas:** Akıl yürütme modelleri daha fazla token üretir (`<think>` bölümü) ve daha yavaştır. Basit soru-cevap için phi-3.5-mini gibi standart modeller daha hızlıdır.

---

### Egzersiz 11: Takma Adları ve Donanım Seçimini Anlamak

Bir tam model kimliği yerine bir **takma ad** (örneğin `phi-3.5-mini`) ilettiğinizde, SDK donanımınız için en iyi varyantı otomatik olarak seçer:

| Donanım | Seçilen Yürütme Sağlayıcısı |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (WinML eklentisiyle) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Herhangi bir cihaz (yedek) | `CPUExecutionProvider` veya `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Takma ad DONANIMINIZ için en iyi varyanta çözülür
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **İpucu:** Uygulama kodunuzda her zaman takma adları kullanın. Bir kullanıcının makinesine dağıttığınızda, SDK çalışma zamanında en uygun varyantı seçer — NVIDIA için CUDA, Qualcomm için QNN, diğerleri için CPU.

---

### Egzersiz 12: C# Konfigürasyon Seçenekleri

C# SDK'nın `Configuration` sınıfı çalışma zamanını ince ayar kontrolü sağlar:

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

| Özellik | Varsayılan | Açıklama |
|----------|---------|-------------|
| `AppName` | (gerekli) | Uygulamanızın adı |
| `LogLevel` | `Information` | Günlükleme ayrıntı seviyesi |
| `Web.Urls` | (dinamik) | Web sunucusu için belirli bir port sabitleme |
| `AppDataDir` | OS varsayılanı | Uygulama verileri için temel dizin |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Modellerin depolandığı yer |
| `LogsDir` | `{AppDataDir}/logs` | Günlüklerin yazıldığı yer |

---

### Egzersiz 13: Tarayıcı Kullanımı (Sadece JavaScript)

JavaScript SDK'sı tarayıcı uyumlu bir sürüm içerir. Tarayıcıda, hizmeti CLI ile manuel başlatmalı ve ana bilgisayar URL'sini belirtmelisiniz:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Servisi önce manuel olarak başlatın:
//   foundry service start
// Daha sonra CLI çıktısındaki URL'yi kullanın
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Kataloğa göz atın
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Tarayıcı sınırlamaları:** Tarayıcı sürümü `startWebService()` metodunu desteklemez. SDK'yı tarayıcıda kullanmadan önce Foundry Local hizmetinin zaten çalışıyor olması gerekir.

---

## Tam API Referansı

### Python

| Kategori | Metod | Açıklama |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Yönetici oluşturur; isteğe bağlı olarak bir model ile başlatır |
| **Hizmet** | `is_service_running()` | Hizmetin çalışıp çalışmadığını kontrol eder |
| **Hizmet** | `start_service()` | Hizmeti başlatır |
| **Hizmet** | `endpoint` | API uç nokta URL'si |
| **Hizmet** | `api_key` | API anahtarı |
| **Katalog** | `list_catalog_models()` | Mevcut tüm modelleri listeler |
| **Katalog** | `refresh_catalog()` | Kataloğu yeniler |
| **Katalog** | `get_model_info(alias_or_model_id)` | Model meta verisini alır |
| **Önbellek** | `get_cache_location()` | Önbellek dizin yolu |
| **Önbellek** | `list_cached_models()` | İndirilen modelleri listeler |
| **Model** | `download_model(alias_or_model_id)` | Bir modeli indirir |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Bir modeli yükler |
| **Model** | `unload_model(alias_or_model_id)` | Bir modeli boşaltır |
| **Model** | `list_loaded_models()` | Yüklü modelleri listeler |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Daha yeni bir sürümün mevcut olup olmadığını kontrol eder |
| **Model** | `upgrade_model(alias_or_model_id)` | Modeli en son sürüme yükseltir |
| **Hizmet** | `httpx_client` | Doğrudan API çağrıları için ön yapılandırılmış HTTPX istemcisi |

### JavaScript

| Kategori | Metod | Açıklama |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | SDK singleton'ını başlatır |
| **Init** | `FoundryLocalManager.instance` | Singleton yöneticisine erişim |
| **Hizmet** | `manager.startWebService()` | Web hizmetini başlatır |
| **Hizmet** | `manager.urls` | Hizmet için temel URL dizisi |
| **Katalog** | `manager.catalog` | Model kataloğuna erişim |
| **Katalog** | `catalog.getModel(alias)` | Takma ada göre model nesnesi alır (Promise döner) |
| **Model** | `model.isCached` | Modelin indirilip indirilmediği |
| **Model** | `model.download()` | Modeli indirir |
| **Model** | `model.load()` | Modeli yükler |
| **Model** | `model.unload()` | Modeli boşaltır |
| **Model** | `model.id` | Modelin benzersiz tanımlayıcısı |
| **Model** | `model.alias` | Modelin takma adı |
| **Model** | `model.createChatClient()` | Yerel chat istemcisi alır (OpenAI SDK gerekmez) |
| **Model** | `model.createAudioClient()` | Transkripsiyon için ses istemcisi alır |
| **Model** | `model.removeFromCache()` | Modeli yerel önbellekten kaldırır |
| **Model** | `model.selectVariant(variant)` | Belirli bir donanım varyantını seçer |
| **Model** | `model.variants` | Bu model için mevcut varyantlar dizisi |
| **Model** | `model.isLoaded()` | Modelin şu anda yüklü olup olmadığını kontrol eder |
| **Katalog** | `catalog.getModels()` | Mevcut tüm modelleri listeler |
| **Katalog** | `catalog.getCachedModels()` | İndirilen modelleri listeler |
| **Katalog** | `catalog.getLoadedModels()` | Şu anda yüklü modelleri listeler |
| **Katalog** | `catalog.updateModels()` | Hizmetten kataloğu yeniler |
| **Hizmet** | `manager.stopWebService()` | Foundry Local web hizmetini durdurur |

### C# (v0.8.0+)

| Kategori | Metod | Açıklama |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Yöneticiyi başlatır |
| **Init** | `FoundryLocalManager.Instance` | Singleton'a erişim |
| **Katalog** | `manager.GetCatalogAsync()` | Kataloğu alır |
| **Katalog** | `catalog.ListModelsAsync()` | Tüm modelleri listeler |
| **Katalog** | `catalog.GetModelAsync(alias)` | Belirli bir modeli alır |
| **Katalog** | `catalog.GetCachedModelsAsync()` | Önbelleğe alınan modelleri listeler |
| **Katalog** | `catalog.GetLoadedModelsAsync()` | Yüklü modelleri listeler |
| **Model** | `model.DownloadAsync(progress?)` | Model indirir |
| **Model** | `model.LoadAsync()` | Model yükler |
| **Model** | `model.UnloadAsync()` | Model boşaltır |
| **Model** | `model.SelectVariant(variant)` | Donanım varyantı seçer |
| **Model** | `model.GetChatClientAsync()` | Yerel chat istemcisi alır |
| **Model** | `model.GetAudioClientAsync()` | Ses transkripsiyon istemcisi alır |
| **Model** | `model.GetPathAsync()` | Yerel dosya yolunu alır |
| **Katalog** | `catalog.GetModelVariantAsync(alias, variant)` | Belirli bir donanım varyantını alır |
| **Katalog** | `catalog.UpdateModelsAsync()` | Kataloğu yeniler |
| **Sunucu** | `manager.StartWebServerAsync()` | REST web sunucusunu başlatır |
| **Sunucu** | `manager.StopWebServerAsync()` | REST web sunucusunu durdurur |
| **Konfig** | `config.ModelCacheDir` | Önbellek dizini |

---

## Temel Çıkarımlar

| Kavram | Öğrendikleriniz |
|---------|-----------------|
| **SDK vs CLI** | SDK, uygulamalar için programatik kontrol sağlar |
| **Kontrol düzlemi** | SDK, hizmetleri, modelleri ve önbellekleme işlemlerini yönetir |
| **Dinamik portlar** | Portu asla sabitlemeyin, daima `manager.endpoint` (Python) veya `manager.urls[0]` (JS/C#) kullanın |
| **Takma adlar** | Otomatik donanım-optimal model seçimi için takma adlar kullanın |
| **Hızlı başlangıç** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# yeniden tasarımı** | v0.8.0+ kendi kendine yetiyor - son kullanıcı makinelerinde CLI gerekli değil |
| **Model yaşam döngüsü** | Katalog → İndir → Yükle → Kullan → Boşalt |
| **FoundryModelInfo** | Zengin meta veriler: görev, cihaz, boyut, lisans, araç çağrı desteği |
| **ChatClient** | OpenAI ücretsiz kullanımı için `createChatClient()` (JS) / `GetChatClientAsync()` (C#) |
| **Varyantlar** | Modellerin donanıma özel varyantları var (CPU, GPU, NPU); otomatik seçilir |
| **Yükseltmeler** | Python: modelleri güncel tutmak için `is_model_upgradeable()` + `upgrade_model()` |
| **Katalog yenileme** | Yeni modelleri keşfetmek için `refresh_catalog()` (Python) / `updateModels()` (JS) |

---

## Kaynaklar

| Kaynak | Bağlantı |
|----------|------|
| SDK Referansı (tüm diller) | [Microsoft Learn - Foundry Local SDK Referansı](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Çıkarım SDK'ları ile entegrasyon | [Microsoft Learn - Çıkarım SDK Entegrasyonu](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API Referansı | [Foundry Local C# API Referansı](https://aka.ms/fl-csharp-api-ref) |
| C# SDK Örnekleri | [GitHub - Foundry Local SDK Örnekleri](https://aka.ms/foundrylocalSDK) |
| Foundry Local web sitesi | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Sonraki Adımlar

SDK'yı OpenAI istemci kitaplığına bağlamak ve ilk sohbet tamamlama uygulamanızı oluşturmak için [Bölüm 3: OpenAI ile SDK Kullanımı](part3-sdk-and-apis.md) bölümüne devam edin.