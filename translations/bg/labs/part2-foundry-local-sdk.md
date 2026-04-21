![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Част 2: Задълбочено разглеждане на Foundry Local SDK

> **Цел:** Овладейте Foundry Local SDK за управление на модели, услуги и кеширане програмно - и разберете защо SDK е препоръчителния подход пред CLI при изграждането на приложения.

## Преглед

В Част 1 използвахте **Foundry Local CLI**, за да изтегляте и стартирате модели интерактивно. CLI е страхотен за изследване, но когато създавате реални приложения, имате нужда от **програмно управление**. Foundry Local SDK ви дава това – той управлява **контролния слой** (стартиране на услугата, откриване на модели, изтегляне, зареждане), за да може кода на вашето приложение да се фокусира върху **данните** (изпращане на заявки, получаване на отговори).

Този лабораторен урок ви учи на пълния API на SDK за Python, JavaScript и C#. До края ще разбирате всякакъв достъпен метод и кога да използвате всеки.

## Цели на обучението

Към края на този лабораторен урок ще можете да:

- Обясните защо SDK е предпочитан пред CLI за разработка на приложения
- Инсталирате Foundry Local SDK за Python, JavaScript или C#
- Използвате `FoundryLocalManager`, за да стартирате услугата, управлявате модели и правите запитвания към каталога
- Изброявате, изтегляте, зареждате и разтоварвате модели програмно
- Преглеждате метаданни за модели с `FoundryModelInfo`
- Разбирате разликата между каталог, кеш и заредени модели
- Използвате конструкторския bootstrap (Python) и шаблона `create()` + каталог (JavaScript)
- Разбирате преработения C# SDK и неговия обектно-ориентиран API

---

## Предварителни условия

| Изискване | Подробности |
|-------------|---------|
| **Foundry Local CLI** | Инсталиран и в `PATH` ([Част 1](part1-getting-started.md)) |
| **Среда за програмен език** | **Python 3.9+** и/или **Node.js 18+** и/или **.NET 9.0+** |

---

## Концепция: SDK срещу CLI - Защо да използваме SDK?

| Аспект | CLI (`foundry` команда) | SDK (`foundry-local-sdk`) |
|--------|------------------------|--------------------------|
| **Случай на употреба** | Изследване, ръчно тестване | Интеграция в приложение |
| **Управление на услугата** | Ръчно: `foundry service start` | Автоматично: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Откриване на порт** | Чете се от CLI изход | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Изтегляне на модел** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Обработка на грешки** | Кодове за изход, stderr | Изключения, типизирани грешки |
| **Автоматизация** | Shell скриптове | Интеграция с родния език |
| **Деплоймент** | Изисква CLI на машината на потребителя | C# SDK може да е самостоятелен (без CLI) |

> **Основен извод:** SDK управлява целия жизнен цикъл: стартиране на услугата, проверка на кеша, изтегляне на липсващи модели, зареждане и откриване на крайна точка с няколко реда код. Вашето приложение не трябва да парсва изхода на CLI или да управлява подпроцеси.

---

## Лабораторни упражнения

### Упражнение 1: Инсталиране на SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Потвърдете инсталацията:

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

Потвърдете инсталацията:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Има два NuGet пакета:

| Пакет | Платформа | Описание |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Крос-платформен | Работи на Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Само Windows | Добавя WinML хардуерно ускорение; изтегля и инсталира изпълнителни провайдъри (например QNN за Qualcomm NPU) |

**Настройка за Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Редактирайте `.csproj` файла:

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

> **Забележка:** В Windows пакетът WinML е супермножество, което включва базовия SDK плюс QNN изпълнителен провайдър. В Linux/macOS се използва базовият SDK. Условните таргетирани фреймуъркове и референции на пакети правят проекта напълно крос-платформен.

Създайте `nuget.config` в корена на проекта:

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

Възстановете пакетите:

```bash
dotnet restore
```

</details>

---

### Упражнение 2: Стартиране на услугата и изброяване на каталога

Първото нещо, което всяко приложение прави, е да стартира Foundry Local услугата и да открие наличните модели.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Създайте мениджър и стартирайте услугата
manager = FoundryLocalManager()
manager.start_service()

# Изброяване на всички налични модели в каталога
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Методи за управление на услугата

| Метод | Сигнатура | Описание |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Проверява дали услугата работи |
| `start_service()` | `() -> None` | Стартира Foundry Local услугата |
| `service_uri` | `@property -> str` | Базовият URI на услугата |
| `endpoint` | `@property -> str` | API крайна точка (service URI + `/v1`) |
| `api_key` | `@property -> str` | API ключ (от среда или по подразбиране) |

#### Python SDK - Методи за управление на каталога

| Метод | Сигнатура | Описание |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Изброява всички модели в каталога |
| `refresh_catalog()` | `() -> None` | Обновява каталога от услугата |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Връща информация за конкретен модел |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Създайте мениджър и стартирайте услугата
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Разгледайте каталога
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Методи на Manager

| Метод | Сигнатура | Описание |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Инициализира сингълтъна на SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Достъп до сингълтън мениджъра |
| `manager.startWebService()` | `() => Promise<void>` | Стартира Foundry Local уеб услугата |
| `manager.urls` | `string[]` | Масив с базови URL-та на услугата |

#### JavaScript SDK - Методи на каталога и моделите

| Метод | Сигнатура | Описание |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Достъп до каталог модела |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Получаване на модел обект по псевдоним |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK версия 0.8.0+ използва обектно-ориентирана архитектура с обекти `Configuration`, `Catalog` и `Model`:

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

#### C# SDK - Основни класове

| Клас | Цел |
|-------|---------|
| `Configuration` | Задава име на приложението, ниво на лог, директория за кеш, URL-та на уеб сървър |
| `FoundryLocalManager` | Основната входна точка - създава се чрез `CreateAsync()`, достъпва се чрез `.Instance` |
| `Catalog` | Разглеждане, търсене и получаване на модели от каталога |
| `Model` | Представлява конкретен модел - изтегляне, зареждане, получаване на клиенти |

#### C# SDK - Методи на Manager и Catalog

| Метод | Описание |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Инициализация на мениджъра |
| `FoundryLocalManager.Instance` | Достъп до сингълтън мениджъра |
| `manager.GetCatalogAsync()` | Получаване на каталога с модели |
| `catalog.ListModelsAsync()` | Изброяване на всички налични модели |
| `catalog.GetModelAsync(alias: "alias")` | Получаване на конкретен модел по псевдоним |
| `catalog.GetCachedModelsAsync()` | Изброяване на изтеглени модели |
| `catalog.GetLoadedModelsAsync()` | Изброяване на заредени в момента модели |

> **Забележка за архитектурата на C#:** C# SDK версия 0.8.0+ е преработен да бъде **самостоятелен**; не изисква Foundry Local CLI на машината на крайния потребител. SDK управлява модели и извеждане нативно.

</details>

---

### Упражнение 3: Изтегляне и зареждане на модел

SDK разделя изтеглянето (на диск) от зареждането (в памет). Това ви позволява да изтеглите модели предварително при настройка и да ги зареждате при нужда.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Вариант A: Ръчно стъпка по стъпка
manager = FoundryLocalManager()
manager.start_service()

# Проверете кеша първо
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

# Вариант B: Еднореден bootstrap (препоръчително)
# Предайте псевдоним на конструктора - той стартира услугата, изтегля и зарежда автоматично
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Методи за управление на модели

| Метод | Сигнатура | Описание |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Изтегля модел в локален кеш |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Зарежда модел в сървъра за предсказания |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Разтоварва модел от сървъра |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Изброява всички заредени модели в момента |

#### Python - Методи за управление на кеша

| Метод | Сигнатура | Описание |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Получава пътя към директорията за кеш |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Изброява всички изтеглени модели |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Подход стъпка по стъпка
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

#### JavaScript - Методи на моделите

| Метод | Сигнатура | Описание |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Дали моделът вече е изтеглен |
| `model.download()` | `() => Promise<void>` | Изтегля модела в локален кеш |
| `model.load()` | `() => Promise<void>` | Зарежда в сървъра за предсказания |
| `model.unload()` | `() => Promise<void>` | Разтоварва от сървъра за предсказания |
| `model.id` | `string` | Уникалният идентификатор на модела |

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

#### C# - Методи на моделите

| Метод | Описание |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Изтегля избраната вариация |
| `model.LoadAsync()` | Зарежда модела в памет |
| `model.UnloadAsync()` | Разтоварва модела |
| `model.SelectVariant(variant)` | Избира конкретна вариация (CPU/GPU/NPU) |
| `model.SelectedVariant` | Активна избрана вариация |
| `model.Variants` | Всички налични вариации за този модел |
| `model.GetPathAsync()` | Получава локалния път към файла |
| `model.GetChatClientAsync()` | Получава нативен чат клиент (без нужда от OpenAI SDK) |
| `model.GetAudioClientAsync()` | Получава аудио клиент за транскрипция |

</details>

---

### Упражнение 4: Преглед на метаданните на модела

Обектът `FoundryModelInfo` съдържа богати метаданни за всеки модел. Разбирането на тези полета ви помага да изберете подходящия модел за вашето приложение.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Вземете подробна информация за конкретен модел
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

#### Полета на FoundryModelInfo

| Поле | Тип | Описание |
|-------|------|-------------|
| `alias` | string | Кратко име (например `phi-3.5-mini`) |
| `id` | string | Уникален идентификатор на модела |
| `version` | string | Версия на модела |
| `task` | string | `chat-completions` или `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU или NPU |
| `execution_provider` | string | Изпълнителна среда (CUDA, CPU, QNN, WebGPU и др.) |
| `file_size_mb` | int | Размер на диска в MB |
| `supports_tool_calling` | bool | Дали моделът поддържа извикване на функции/инструменти |
| `publisher` | string | Кой е публикувал модела |
| `license` | string | Лиценз |
| `uri` | string | URI на модела |
| `prompt_template` | dict/null | Шаблон на запитването, ако има такъв |

---

### Упражнение 5: Управление на жизнения цикъл на модела

Практикувайте пълния жизнен цикъл: изброяване → изтегляне → зареждане → използване → разтоварване.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Малък модел за бързо тестване

manager = FoundryLocalManager()
manager.start_service()

# 1. Проверете какво има в каталога
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Проверете какво вече е изтеглено
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Изтеглете модел
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Проверете, че сега е в кеша
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Заредете го
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Проверете какво е заредено
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Разтоварете го
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

const alias = "qwen2.5-0.5b"; // Малък модел за бързо тестване

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Вземете модел от каталога
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Изтеглете при необходимост
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Заредете го
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Разтоварете го
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Упражнение 6: Шаблони за бърз старт

Всякакъв език предоставя shortcut за стартиране на услугата и зареждане на модел с един повик. Това са **препоръчаните шаблони** за повечето приложения.

<details>
<summary><h3>🐍 Python - Конструктор Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Подайте псевдоним на конструктора - той се грижи за всичко:
# 1. Стартира услугата, ако не работи
# 2. Изтегля модела, ако не е кеширан
# 3. Зарежда модела в сървъра за извод
manager = FoundryLocalManager("phi-3.5-mini")

# Готов за използване веднага
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Параметърът `bootstrap` (по подразбиране `True`) контролира това поведение. Задайте `bootstrap=False`, ако искате да управлявате ръчно:

```python
# Ръчен режим - нищо не се случва автоматично
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Каталог</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() обработва всичко:
// 1. Стартира услугата
// 2. Взима модела от каталога
// 3. Изтегля при нужда и зарежда модела
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Готов за използване веднага
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

> **Бележка за C#:** SDK за C# използва `Configuration` за контрол на името на приложението, логването, директориите за кеш и дори за закрепване на конкретен порт за уеб сървър. Това го прави най-конфигурируемият от трите SDK.

</details>

---

### Упражнение 7: Родният ChatClient (Не е нужен OpenAI SDK)

SDK-тата за JavaScript и C# предоставят удобен метод `createChatClient()`, който връща роден чат клиент — не е необходимо отделно инсталиране или конфигурация на OpenAI SDK.

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

// Създайте ChatClient директно от модела — не е необходим импорт на OpenAI
const chatClient = model.createChatClient();

// completeChat връща обект за отговор, съвместим с OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Поточното предаване използва шаблон с обратни повиквания
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` също поддържа извикване на инструменти — предайте инструментите като втори аргумент:

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

> **Кога да използвате кой шаблон:**
> - **`createChatClient()`** — Бърз прототип, по-малко зависимости, по-прост код
> - **OpenAI SDK** — Пълен контрол над параметрите (температура, top_p, стоп токени и др.), по-подходящ за продукционни приложения

---

### Упражнение 8: Варианти на модели и избор на хардуер

Моделите може да имат няколко **варианта**, оптимизирани за различен хардуер. SDK автоматично избира най-добрия вариант, но можете и ръчно да инспектирате и изберете.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Изброяване на всички налични варианти
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK автоматично избира най-добрия вариант за вашия хардуер
// За да презапишете, използвайте selectVariant():
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

В Python SDK автоматично избира най-добрия вариант според хардуера. Използвайте `get_model_info()` за да видите какъв е избран:

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

#### Модели с NPU варианти

Някои модели имат варианти, оптимизирани за NPU устройства (Qualcomm Snapdragon, Intel Core Ultra):

| Модел | Има ли NPU вариант |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Съвет:** На хардуер с NPU, SDK автоматично избира NPU варианта, когато е наличен. Не е нужно да променяте кода си. За проекти на C# под Windows добавете NuGet пакета `Microsoft.AI.Foundry.Local.WinML`, за да активирате QNN изпълнителния доставчик — QNN се доставя като плъгин EP през WinML.

---

### Упражнение 9: Актуализации на моделите и обновяване на каталога

Каталогът на моделите се обновява периодично. Използвайте тези методи, за да проверите и приложите актуализации.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Обновете каталога, за да получите най-новия списък с модели
manager.refresh_catalog()

# Проверете дали има налична по-нова версия на кеширания модел
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

// Опресняване на каталога за извличане на най-новия списък с модели
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Избройте всички налични модели след опресняване
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Упражнение 10: Работа с модели за разсъждение

Моделът **phi-4-mini-reasoning** включва chain-of-thought разсъждаване. Той опакова вътрешното си мислене в `<think>...</think>` тагове преди да даде крайния отговор. Това е полезно за задачи, изискващи многостъпкова логика, математика или решаване на задачи.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning е около 4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Моделът обгръща мисленето си в тагове <think>...</think>
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

// Анализиране на мисловния процес стъпка по стъпка
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Кога да използвате модели за разсъждение:**
> - Математически и логически задачи
> - Многостъпкови планировъчни задачи
> - Генериране на сложен код
> - Задачи, където показването на работата подобрява точността
>
> **Компромис:** Моделите за разсъждение генерират повече токени (секцията `<think>`) и са по-бавни. За прости въпроси/отговори стандартен модел като phi-3.5-mini е по-бърз.

---

### Упражнение 11: Разбиране на псевдонимите и избор на хардуер

Когато подадете **псевдоним** (като `phi-3.5-mini`) вместо пълен идентификатор на модел, SDK автоматично избира най-добрия вариант за вашия хардуер:

| Хардуер | Избран изпълнителен доставчик |
|----------|-----------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (чрез WinML плъгин) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Всяко устройство (резервен вариант) | `CPUExecutionProvider` или `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Псевдонимът се разрешава до най-добрата версия за ВАШИЯ хардуер
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Съвет:** Винаги използвайте псевдоними в кода на вашето приложение. При стартиране на машината на потребителя, SDK избира оптималния вариант на изпълнение - CUDA при NVIDIA, QNN при Qualcomm, CPU другаде.

---

### Упражнение 12: Опции за конфигурация в C#

Класът `Configuration` в C# SDK предоставя детайлен контрол върху runtime:

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

| Свойство | По подразбиране | Описание |
|----------|-----------------|----------|
| `AppName` | (задължително) | Името на вашето приложение |
| `LogLevel` | `Information` | Ниво на детайлност на логове |
| `Web.Urls` | (динамично) | Закрепване на конкретен порт за уеб сървъра |
| `AppDataDir` | По подразбиране на ОС | Базова директория за данни на приложението |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Къде се съхраняват моделите |
| `LogsDir` | `{AppDataDir}/logs` | Къде се записват логовете |

---

### Упражнение 13: Използване в браузър (само JavaScript)

JavaScript SDK включва версия съвместима с браузъри. В браузъра трябва ръчно да стартирате услугата чрез CLI и да зададете URL на хоста:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Стартирайте услугата ръчно първо:
//   foundry service start
// След това използвайте URL адреса от изхода на CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Разгледайте каталога
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Ограничения в браузър:** Версията за браузър **не поддържа** `startWebService()`. Трябва да се уверите, че Foundry Local услугата вече работи преди да използвате SDK в браузър.

---

## Пълен API справочник

### Python

| Категория | Метод | Описание |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Създаване на мениджър; опционално старт с модел |
| **Service** | `is_service_running()` | Проверка дали услугата работи |
| **Service** | `start_service()` | Стартиране на услугата |
| **Service** | `endpoint` | URL за API крайна точка |
| **Service** | `api_key` | API ключ |
| **Catalog** | `list_catalog_models()` | Списък на всички налични модели |
| **Catalog** | `refresh_catalog()` | Обновяване на каталога |
| **Catalog** | `get_model_info(alias_or_model_id)` | Вземане на метаданни за модел |
| **Cache** | `get_cache_location()` | Път до кеш директория |
| **Cache** | `list_cached_models()` | Списък на изтеглени модели |
| **Model** | `download_model(alias_or_model_id)` | Изтегляне на модел |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Зареждане на модел |
| **Model** | `unload_model(alias_or_model_id)` | Разтоварване на модел |
| **Model** | `list_loaded_models()` | Списък на заредени модели |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Проверка дали има по-нова версия |
| **Model** | `upgrade_model(alias_or_model_id)` | Актуализация на модел до най-нова версия |
| **Service** | `httpx_client` | Преднастройки за HTTPX клиент за директни API повици |

### JavaScript

| Категория | Метод | Описание |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Инициализиране на SDK сингълтон |
| **Init** | `FoundryLocalManager.instance` | Достъп до сингълтон мениджър |
| **Service** | `manager.startWebService()` | Стартиране на уеб услугата |
| **Service** | `manager.urls` | Масив с базови URL адреси на услугата |
| **Catalog** | `manager.catalog` | Достъп до каталог на модели |
| **Catalog** | `catalog.getModel(alias)` | Вземане на модел по псевдоним (връща Promise) |
| **Model** | `model.isCached` | Моделът изтеглен ли е |
| **Model** | `model.download()` | Изтегляне на модел |
| **Model** | `model.load()` | Зареждане на модел |
| **Model** | `model.unload()` | Разтоварване на модел |
| **Model** | `model.id` | Уникален идентификатор на модел |
| **Model** | `model.alias` | Псевдоним на модела |
| **Model** | `model.createChatClient()` | Вземане на роден чат клиент (без нужда от OpenAI SDK) |
| **Model** | `model.createAudioClient()` | Вземане на аудио клиент за транскрипция |
| **Model** | `model.removeFromCache()` | Премахване от локалния кеш |
| **Model** | `model.selectVariant(variant)` | Избор на конкретен хардуерен вариант |
| **Model** | `model.variants` | Масив от налични варианти на модела |
| **Model** | `model.isLoaded()` | Проверка дали моделът е зареден |
| **Catalog** | `catalog.getModels()` | Списък на всички налични модели |
| **Catalog** | `catalog.getCachedModels()` | Списък на изтеглени модели |
| **Catalog** | `catalog.getLoadedModels()` | Списък на заредени модели |
| **Catalog** | `catalog.updateModels()` | Обновяване на каталога от услугата |
| **Service** | `manager.stopWebService()` | Спиране на Foundry Local уеб услугата |

### C# (v0.8.0+)

| Категория | Метод | Описание |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Инициализация на мениджъра |
| **Init** | `FoundryLocalManager.Instance` | Достъп до сингълтона |
| **Catalog** | `manager.GetCatalogAsync()` | Вземане на каталог |
| **Catalog** | `catalog.ListModelsAsync()` | Списък на всички модели |
| **Catalog** | `catalog.GetModelAsync(alias)` | Вземане на конкретен модел |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Списък на кеширани модели |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Списък на заредени модели |
| **Model** | `model.DownloadAsync(progress?)` | Изтегляне на модел |
| **Model** | `model.LoadAsync()` | Зареждане на модел |
| **Model** | `model.UnloadAsync()` | Разтоварване на модел |
| **Model** | `model.SelectVariant(variant)` | Избор на хардуерен вариант |
| **Model** | `model.GetChatClientAsync()` | Вземане на роден чат клиент |
| **Model** | `model.GetAudioClientAsync()` | Вземане на аудио транскрипционен клиент |
| **Model** | `model.GetPathAsync()` | Вземане на локален път до файл |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Вземане на конкретен хардуерен вариант |
| **Catalog** | `catalog.UpdateModelsAsync()` | Обновяване на каталога |
| **Server** | `manager.StartWebServerAsync()` | Стартиране на REST уеб сървър |
| **Server** | `manager.StopWebServerAsync()` | Спиране на REST уеб сървър |
| **Config** | `config.ModelCacheDir` | Директория за кеша |

---

## Основни изводи

| Концепция | Какво научихте |
|---------|-----------------|
| **SDK срещу CLI** | SDK осигурява програмно управление - важно за приложения |
| **Control plane** | SDK управлява услуги, модели и кеширане |
| **Динамични портове** | Винаги използвайте `manager.endpoint` (Python) или `manager.urls[0]` (JS/C#) - никога не жорсткодвайте порт |
| **Псевдоними** | Използвайте псевдоними за автоматичен избор на хардуерно оптимален модел |
| **Бърз старт** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Преработка на C#** | v0.8.0+ е самостоятелен - не се нуждае от CLI на потребителските машини |
| **Жизнен цикъл на модела** | Каталог → Изтегляне → Зареждане → Използване → Разтоварване |
| **FoundryModelInfo** | Богати метаданни: задача, устройство, размер, лиценз, поддръжка на повикване от инструменти |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) за използване без OpenAI |
| **Варианти** | Моделите имат хардуерно специфични варианти (CPU, GPU, NPU); избира се автоматично |
| **Актуализации** | Python: `is_model_upgradeable()` + `upgrade_model()` за поддържане на моделите актуални |
| **Обновяване на каталога** | `refresh_catalog()` (Python) / `updateModels()` (JS) за откриване на нови модели |

---

## Ресурси

| Ресурс | Връзка |
|----------|------|
| Референтна документация на SDK (всички езици) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Интеграция с inference SDK | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Референтна документация на C# SDK API | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| Примери за C# SDK | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Уебсайт на Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Следващи стъпки

Продължете към [Част 3: Използване на SDK с OpenAI](part3-sdk-and-apis.md), за да свържете SDK с клиентската библиотека на OpenAI и да създадете първото си приложение за чат завършване.