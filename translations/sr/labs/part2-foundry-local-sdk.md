![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Део 2: Детаљан преглед Foundry Local SDK-а

> **Циљ:** Савладати Foundry Local SDK за управљање моделима, сервисима и кеширањем програмски - и разумети зашто је SDK препоручени приступ у односу на CLI за израду апликација.

## Преглед

У Делу 1 сте користили **Foundry Local CLI** за преузимање и интерактивно покретање модела. CLI је одличан за истраживање, али када правите праве апликације потребна вам је **програмска контрола**. Foundry Local SDK вам то пружа - управља **control plane** (покретање сервиса, откривање модела, преузимање, учитавање) тако да ваш апликацијски код може да се фокусира на **data plane** (слање упита, пријем комплетирања).

Овај лабораторијски рад вас учи комплетном SDK API-ју на Питону, ЈаваСкрипту и C#. До краја ћете разумети сваки доступан метод и када се користи који.

## Циљеви учења

До краја овог лабораторијског рада моћи ћете да:

- Објасните зашто се SDK препоручује уместо CLI за развој апликација
- Инсталирате Foundry Local SDK за Python, JavaScript или C#
- Користите `FoundryLocalManager` за покретање сервиса, управљање моделима и упите к каталогу
- Програмски набројите, преузмете, учитате и ослободите моделе
- Испитате метаподатке модела користећи `FoundryModelInfo`
- Разумете разлику између каталога, кеша и учитаних модела
- Користите конструктор bootstrap (Python) и `create()` + образац каталога (JavaScript)
- Разумете реструктурирани C# SDK и његов објектно-орјентисани API

---

## Претходни услови

| Захтев | Детаљи |
|-------------|---------|
| **Foundry Local CLI** | Инсталиран и у вашем `PATH`-у ([Део 1](part1-getting-started.md)) |
| **Извршна околина језика** | **Python 3.9+** и/или **Node.js 18+** и/или **.NET 9.0+** |

---

## Концепт: SDK против CLI - Зашто користити SDK?

| Аспект | CLI (`foundry` команда) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Намена** | Истраживање, ручно тестирање | Интеграција апликација |
| **Управљање сервисом** | Ручно: `foundry service start` | Аутоматски: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Откривање порта** | Чита се из CLI излаза | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Преузимање модела** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Обрада грешака** | Кодови излаза, stderr | Изузеци, типизоване грешке |
| **Аутоматизација** | Shell скрипте | Нативна интеграција језика |
| **Деплојмент** | Потребан CLI на корисничкој машини | C# SDK може бити самосталан (без потребе за CLI) |

> **Кључни увид:** SDK управља целим животним циклом: покретање сервиса, провера кеша, преузимање недостајућих модела, њихово учитавање и откривање endpoint-а, сва то у неколико линија кода. Ваша апликација не мора да парсира CLI излаз или управља потпроцесима.

---

## Лабораторијски задаци

### Задатак 1: Инсталирајте SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Потврдите инсталацију:

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

Потврдите инсталацију:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Постоје два NuGet пакета:

| Пакет | Платформа | Опис |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Мултиплатформски | Ради на Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Само Windows | Додаје WinML хардверску акцелерацију; преузима и инсталира провајдере извршења плагина (нпр. QNN за Qualcomm NPU) |

**Постављање на Windows-у:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Измените `.csproj` фајл:

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

> **Напомена:** На Windows-у, WinML пакет је супермножина која садржи основни SDK плус QNN провајдер извршења. На Linux/macOS се користи основни SDK. Условне TFM и референце пакета чине пројекат потпуно мултиплатформским.

Креирајте `nuget.config` у корену пројекта:

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

Вратите пакете:

```bash
dotnet restore
```

</details>

---

### Задатак 2: Покрените сервис и набројте каталог

Прва ствар коју апликација ради је покретање Foundry Local сервиса и откривање доступних модела.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Креирај менаџера и покрени услугу
manager = FoundryLocalManager()
manager.start_service()

# Наведи све моделе доступне у каталогу
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Методе за управљање сервисом

| Метод | Сигнатура | Опис |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Провера да ли сервис ради |
| `start_service()` | `() -> None` | Покрени Foundry Local сервис |
| `service_uri` | `@property -> str` | Основни URI сервиса |
| `endpoint` | `@property -> str` | API endpoint (URI сервиса + `/v1`) |
| `api_key` | `@property -> str` | API кључ (из окруженја или подразумевани плејсхолдер) |

#### Python SDK - Методе за управљање каталогом

| Метод | Сигнатура | Опис |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Наброј све моделе у каталогу |
| `refresh_catalog()` | `() -> None` | Освежи каталог са сервиса |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Добиј информације о одређеном моделу |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Креирај менаџера и покрени сервис
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Прелистај каталог
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Методи менаџера

| Метод | Сигнатура | Опис |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Иницијализуј SDK синглтон |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Приступ синглтону менаџера |
| `manager.startWebService()` | `() => Promise<void>` | Покрени Foundry Local веб сервис |
| `manager.urls` | `string[]` | Низ основних URL-ова за сервис |

#### JavaScript SDK - Методи каталога и модела

| Метод | Сигнатура | Опис |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Приступ каталогу модела |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Добиј модел по алјасу |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK верзије 0.8.0+ користи објектно-оријентисану архитектуру са објектима `Configuration`, `Catalog` и `Model`:

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

#### C# SDK - Кључне класе

| Класа | Намењеност |
|-------|---------|
| `Configuration` | Постављање имена апликације, нивоа логовања, директоријума кеша, URL-ова веб сервера |
| `FoundryLocalManager` | Главна улазна тачка - креирана путем `CreateAsync()`, приступа се преко `.Instance` |
| `Catalog` | Преглед, претрага и добијање модела из каталога |
| `Model` | Представља одређени модел - преузми, учитај, добиј клијенте |

#### C# SDK - Методи менаџера и каталога

| Метод | Опис |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Иницијализује менаџера |
| `FoundryLocalManager.Instance` | Приступ синглтону менаџера |
| `manager.GetCatalogAsync()` | Добија каталог модела |
| `catalog.ListModelsAsync()` | Набраја све доступне моделе |
| `catalog.GetModelAsync(alias: "alias")` | Добија одређени модел по алјасу |
| `catalog.GetCachedModelsAsync()` | Набраја преузете моделе |
| `catalog.GetLoadedModelsAsync()` | Набраја тренутно учитане моделе |

> **Напомена о C# архитектури:** Rеструктурирани C# SDK v0.8.0+ чини апликацију **самосталном**; не захтева Foundry Local CLI на корисничкој машини. SDK нативно управља моделима и извођењем.

</details>

---

### Задатак 3: Преузмите и учитајте модел

SDK раздваја преузимање (на диск) од учитавања (у меморију). Ово вам омогућава да претходно преузмете моделе током подешавања и учитате их по потреби.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Опција А: Ручни корак по корак
manager = FoundryLocalManager()
manager.start_service()

# Прво провери кеш
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

# Опција Б: Једноредни bootstrap (препоручено)
# Прошли алијас конструктору - он аутоматски покреће сервис, преузима и учитава
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Методе управљања моделима

| Метод | Сигнатура | Опис |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Преузми модел у локални кеш |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Учитај модел у inferenc сервер |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Ослободи модел са сервера |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Набраја све тренутно учитане моделе |

#### Python - Методе управљања кешом

| Метод | Сигнатура | Опис |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Добије пут директоријума кеша |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Набраја све преузете моделе |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Приступ корак по корак
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

#### JavaScript - Методи модела

| Метод | Сигнатура | Опис |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Да ли је модел већ преузет |
| `model.download()` | `() => Promise<void>` | Преузми модел у локални кеш |
| `model.load()` | `() => Promise<void>` | Учитај у inferenc сервер |
| `model.unload()` | `() => Promise<void>` | Ослободи са inferenc сервера |
| `model.id` | `string` | Јединствени идентификатор модела |

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

#### C# - Методи модела

| Метод | Опис |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Преузима изабрану варијанту |
| `model.LoadAsync()` | Учитава модел у меморију |
| `model.UnloadAsync()` | Ослобађа модел |
| `model.SelectVariant(variant)` | Изабери одређену варијанту (CPU/GPU/NPU) |
| `model.SelectedVariant` | Тренутно изабрана варијанта |
| `model.Variants` | Све доступне варијанте за овај модел |
| `model.GetPathAsync()` | Добија локални пут до фајла |
| `model.GetChatClientAsync()` | Добија нативни чат клијент (без потребе за OpenAI SDK) |
| `model.GetAudioClientAsync()` | Добија аудио клијента за транскрипцију |

</details>

---

### Задатак 4: Испитајте метаподатке модела

Објекат `FoundryModelInfo` садржи богате метаподатке о сваком моделу. Разумевање ових поља помаже да изаберете прави модел за вашу апликацију.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Добити детаљне информације о одређеном моделу
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

#### Поља FoundryModelInfo

| Поље | Тип | Опис |
|-------|------|-------------|
| `alias` | string | Кратко име (нпр. `phi-3.5-mini`) |
| `id` | string | Јединствени идентификатор модела |
| `version` | string | Верзија модела |
| `task` | string | `chat-completions` или `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU или NPU |
| `execution_provider` | string | Runtime backend (CUDA, CPU, QNN, WebGPU, итд.) |
| `file_size_mb` | int | Величина на диску у MB |
| `supports_tool_calling` | bool | Да ли модел подржава позивање функција/алата |
| `publisher` | string | Ко је издавач модела |
| `license` | string | Назив лиценце |
| `uri` | string | URI модела |
| `prompt_template` | dict/null | Шаблон упита, ако постоји |

---

### Задатак 5: Управљајте животним циклусом модела

Вежбајте комплетан животни циклус: наброј → преузми → учитај → користи → ослободи.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Мали модел за брзо тестирање

manager = FoundryLocalManager()
manager.start_service()

# 1. Проверите шта је у каталогу
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Проверите шта је већ преузето
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Преузмите модел
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Потврдите да је сада у кешу
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Учитајте га
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Проверите шта је учитано
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Испразните га
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

const alias = "qwen2.5-0.5b"; // Мали модел за брзо тестирање

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Преузмите модел из каталога
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Преузмите ако је потребно
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Учитајте га
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Испразните га
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Вежба 6: Обрасци за брзи почетак

Сваки језик пружа пречицу за покретање сервиса и учитавање модела у једном позиву. Ово су **препоручени обрасци** за већину апликација.

<details>
<summary><h3>🐍 Python - Конструкторско покретање</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Пренесите алијас конструктору - он се брине о свему:
# 1. Покреће сервис ако није покренут
# 2. Преузима модел ако није кеширан
# 3. Учитава модел у inference сервер
manager = FoundryLocalManager("phi-3.5-mini")

# Спреман за употребу одмах
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Параметар `bootstrap` (подразумевано `True`) контролише ово понашање. Подесите `bootstrap=False` ако желите ручну контролу:

```python
# Ручни режим - ништа се не дешава аутоматски
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Каталог</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() обављају све:
// 1. Покреће сервис
// 2. Добија модел из каталога
// 3. Преузима ако је потребно и учитава модел
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Спремно за коришћење одмах
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Каталог</h3></summary>

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

> **Напомена за C#:** C# SDK користи `Configuration` за контролу имена апликације, логовања, кеширања директоријума, и чак за фиксирање одређеног порта веб сервера. Ово га чини најконфигурибилнијим од ова три SDK-а.

</details>

---

### Вежба 7: Нативни ChatClient (Није потребан OpenAI SDK)

JavaScript и C# SDK пружају метод погодности `createChatClient()` који враћа нативног chat клијента — није потребна посебна инсталација или конфигурација OpenAI SDK-а.

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

// Креирајте ChatClient директно из модела — није потребан увоз OpenAI
const chatClient = model.createChatClient();

// completeChat враћа објекат одговора компатибилан са OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Стреаминг користи образац повратног позива
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` такође подржава позивање алата — проследите алате као други аргумент:

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

> **Када користити који образац:**
> - **`createChatClient()`** — Брзо прототиповање, мање зависности, једноставнији код
> - **OpenAI SDK** — Потпуна контрола параметара (температура, top_p, стоп токени итд.), боље за продукцијске апликације

---

### Вежба 8: Варијанте модела и одабир хардвера

Модели могу имати више **варијанти** оптимизованих за различити хардвер. SDK аутоматски бира најбољу варијанту, али такође можете прегледати и ручно изабрати.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Наведи све доступне варијанте
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK аутоматски бира најбољу варијанту за ваш хардвер
// Да бисте променили, користите selectVariant():
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

У Python-у, SDK аутоматски бира најбољу варијанту на основу хардвера. Користите `get_model_info()` да видите шта је изабрано:

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

#### Модели са NPU варијантама

Неки модели имају NPU-оптимизоване варијанте за уређаје са неуронским процесорским јединицама (Qualcomm Snapdragon, Intel Core Ultra):

| Модел | NPU варијанта доступна |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Савет:** На хардверу који подржава NPU, SDK аутоматски бира NPU варијанту кад је доступна. Не морате мењати свој код. За C# пројекте на Windows-у додате `Microsoft.AI.Foundry.Local.WinML` NuGet пакет да бисте омогућили QNN извршног провајдера — QNN се испоручује као EP додатак преко WinML.

---

### Вежба 9: Надоградње модела и освежавање каталога

Каталог модела се периодично ажурира. Користите ове методе да проверите и примените исправке.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Освежите каталог да бисте добили најновију листу модела
manager.refresh_catalog()

# Проверите да ли кеширани модел има новију верзију доступну
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

// Освежи каталог да би се учитала најновија листа модела
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Прикажи све расположиве моделе након освежавања
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Вежба 10: Рад са моделима за размишљање

**phi-4-mini-reasoning** модел укључује ланац размишљања. Обавија своје унутрашње размишљање у `<think>...</think>` тагове пре него што пружи коначни одговор. Ово је корисно за задатке који захтевају више корака логике, математике или решавања проблема.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning је ~4.6 ГБ
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Модел свој размишљање обавија у <think>...</think> ознаке
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

// Анализа ланца размишљања
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Када користити моделе за размишљање:**
> - Задаци из математике и логике
> - Задаци са више корака планирања
> - Комплексна генерација кода
> - Задаци где приказивање рада побољшава прецизност
>
> **Компромис:** Модели за размишљање производе више токена (секција `<think>`) и спорији су. За једноставна питања и одговоре, стандардни модел као phi-3.5-mini је бржи.

---

### Вежба 11: Разумевање алијаса и одабир хардвера

Када проследите **алијас** (нпр. `phi-3.5-mini`) уместо пуног ID-а модела, SDK аутоматски бира најбољу варијанту за ваш хардвер:

| Хардвер | Изабрани извршни провајдер |
|----------|---------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (кроз WinML додатак) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Било који уређај (фалбек) | `CPUExecutionProvider` или `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Псеудоним се прекида на најбољу варијанту за ВАШ хардвер
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Савет:** Увек користите алијасе у свом апликацијском коду. Када деплојујете на кориснички рачунар, SDK бира оптималну варијанту приликом извршавања - CUDA на NVIDIA, QNN на Qualcomm, CPU где год је потребно.

---

### Вежба 12: Опције конфигурације у C#

C# SDK класа `Configuration` пружа фино подешавање радног времена:

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

| Својство | Подразумевано | Опис |
|----------|---------|-------------|
| `AppName` | (обавезно) | Име ваше апликације |
| `LogLevel` | `Information` | Ниво логовања |
| `Web.Urls` | (динамично) | Фиксирање одређеног порта за веб сервер |
| `AppDataDir` | Подразумевано за OS | Основни директоријум за податке апликације |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Место где се чувају модели |
| `LogsDir` | `{AppDataDir}/logs` | Место за уписивање логова |

---

### Вежба 13: Коришћење у претраживачу (само JavaScript)

JavaScript SDK укључује верзију компатибилну са прегледачем. У прегледачу морате ручно покренути сервис преко CLI и одредити URL хоста:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Ручно покрените сервис прво:
//   foundry service start
// Затим користите URL из CLI излаза
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Прегледајте каталог
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Ограничења у прегледачу:** Верзија за прегледач не подржава `startWebService()`. Морате осигурати да Foundry Local сервис већ ради пре коришћења SDK-а у прегледачу.

---

## Комплетна референца API-ја

### Python

| Категорија | Метод | Опис |
|----------|--------|-------------|
| **Иницијализација** | `FoundryLocalManager(alias?, bootstrap=True)` | Креирање менаџера; опционално са учитавањем модела |
| **Сервис** | `is_service_running()` | Провера да ли сервис ради |
| **Сервис** | `start_service()` | Покрени сервис |
| **Сервис** | `endpoint` | URL API крајње тачке |
| **Сервис** | `api_key` | API кључ |
| **Каталог** | `list_catalog_models()` | Листа свих доступних модела |
| **Каталог** | `refresh_catalog()` | Освежи каталог |
| **Каталог** | `get_model_info(alias_or_model_id)` | Добити метаподатке модела |
| **Кеш** | `get_cache_location()` | Путања до кеш директоријума |
| **Кеш** | `list_cached_models()` | Листа преузетих модела |
| **Модел** | `download_model(alias_or_model_id)` | Преузимање модела |
| **Модел** | `load_model(alias_or_model_id, ttl=600)` | Учитавање модела |
| **Модел** | `unload_model(alias_or_model_id)` | Ослобађање модела |
| **Модел** | `list_loaded_models()` | Листа учитаних модела |
| **Модел** | `is_model_upgradeable(alias_or_model_id)` | Провера да ли је доступна новија верзија |
| **Модел** | `upgrade_model(alias_or_model_id)` | Надоградња модела на најновију верзију |
| **Сервис** | `httpx_client` | Предконфигурисани HTTPX клијент за директне API позиве |

### JavaScript

| Категорија | Метод | Опис |
|----------|--------|-------------|
| **Иницијализација** | `FoundryLocalManager.create(options)` | Иницијализација SDK синглтон објекта |
| **Иницијализација** | `FoundryLocalManager.instance` | Приступ синглтон менаџеру |
| **Сервис** | `manager.startWebService()` | Покренути веб сервис |
| **Сервис** | `manager.urls` | Низ основних URL-ова сервиса |
| **Каталог** | `manager.catalog` | Приступ каталогу модела |
| **Каталог** | `catalog.getModel(alias)` | Узми модел по алијасу (враћа Promise) |
| **Модел** | `model.isCached` | Да ли је модел преузет |
| **Модел** | `model.download()` | Преузми модел |
| **Модел** | `model.load()` | Учитај модел |
| **Модел** | `model.unload()` | Ослободи модел |
| **Модел** | `model.id` | Јединствени идентификатор модела |
| **Модел** | `model.alias` | Алијас модела |
| **Модел** | `model.createChatClient()` | Добити нативни chat клијент (без OpenAI SDK-а) |
| **Модел** | `model.createAudioClient()` | Добити аудио клијента за транскрипцију |
| **Модел** | `model.removeFromCache()` | Уклонити модел из локалног кеша |
| **Модел** | `model.selectVariant(variant)` | Изабрати одређену хардверску варијанту |
| **Модел** | `model.variants` | Низ доступних варијанти за овај модел |
| **Модел** | `model.isLoaded()` | Проверити да ли је модел тренутно учитан |
| **Каталог** | `catalog.getModels()` | Листа свих доступних модела |
| **Каталог** | `catalog.getCachedModels()` | Листа преузетих модела |
| **Каталог** | `catalog.getLoadedModels()` | Листа тренутно учитаних модела |
| **Каталог** | `catalog.updateModels()` | Освежи каталог са сервиса |
| **Сервис** | `manager.stopWebService()` | Зауставити Foundry Local веб сервис |

### C# (v0.8.0+)

| Категорија | Метод | Опис |
|----------|--------|-------------|
| **Иницијализација** | `FoundryLocalManager.CreateAsync(config, logger)` | Иницијализација менаџера |
| **Иницијализација** | `FoundryLocalManager.Instance` | Приступ синглтону |
| **Каталог** | `manager.GetCatalogAsync()` | Преузимање каталога |
| **Каталог** | `catalog.ListModelsAsync()` | Листа свих модела |
| **Каталог** | `catalog.GetModelAsync(alias)` | Преузимање конкретног модела |
| **Каталог** | `catalog.GetCachedModelsAsync()` | Листа кешираних модела |
| **Каталог** | `catalog.GetLoadedModelsAsync()` | Листа учитаних модела |
| **Модел** | `model.DownloadAsync(progress?)` | Преузимање модела |
| **Модел** | `model.LoadAsync()` | Учитавање модела |
| **Модел** | `model.UnloadAsync()` | Ослобађање модела |
| **Модел** | `model.SelectVariant(variant)` | Избор хардверске варијанте |
| **Модел** | `model.GetChatClientAsync()` | Добити нативни chat клијент |
| **Модел** | `model.GetAudioClientAsync()` | Добити аудио транскрипцијски клијент |
| **Модел** | `model.GetPathAsync()` | Локална путања до фајла |
| **Каталог** | `catalog.GetModelVariantAsync(alias, variant)` | Преузимање одређене хардверске варијанте |
| **Каталог** | `catalog.UpdateModelsAsync()` | Освежавање каталога |
| **Сервер** | `manager.StartWebServerAsync()` | Покретање REST веб сервера |
| **Сервер** | `manager.StopWebServerAsync()` | Заустављање REST веб сервера |
| **Конфиг** | `config.ModelCacheDir` | Директоријум кеша |

---

## Кључне напомене

| Концепт | Шта сте научили |
|---------|-----------------|
| **SDK у односу на CLI** | SDK пружа програмску контролу - од суштинског значаја за апликације |
| **Контролна равнина** | SDK управља сервисима, моделима и кеширањем |
| **Динамички портови** | Увек користите `manager.endpoint` (Python) или `manager.urls[0]` (JS/C#) — никад не кодирате порт статички |
| **Алијаси** | Користите алијасе за аутоматски избор најоптималније варијанте за хардвер |
| **Брзи почетак** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Редизајн за C#** | в0.8.0+ је самосталан - није потребан CLI на крајњим корисничким уређајима |
| **Животни циклус модела** | Каталог → Преузимање → Учитавање → Коришћење → Искључивање |
| **FoundryModelInfo** | Богати метаподаци: задатак, уређај, величина, лиценца, подршка за позивање алата |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) за коришћење без OpenAI |
| **Варијанте** | Модели имају варијанте специфичне за хардвер (CPU, GPU, NPU); аутоматски се бирају |
| **Надоградње** | Python: `is_model_upgradeable()` + `upgrade_model()` за одржавање модела ажурним |
| **Освежавање каталога** | `refresh_catalog()` (Python) / `updateModels()` (JS) за откривање нових модела |

---

## Ресурси

| Ресурс | Линк |
|----------|------|
| SDK Референца (сви језици) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Интеграција са inference SDK-овима | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API Референца | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK Примери | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local вебсајт | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Следећи кораци

Наставите на [Део 3: Коришћење SDK са OpenAI](part3-sdk-and-apis.md) да бисте повезали SDK са OpenAI клијент библиотеком и направили своју прву апликацију за комплетирање ћаскања.