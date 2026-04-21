![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Частина 2: Глибокий розбір Foundry Local SDK

> **Мета:** Опанувати Foundry Local SDK для програмного керування моделями, сервісами та кешуванням — і зрозуміти, чому SDK є рекомендованим підходом замість CLI для створення застосунків.

## Огляд

У Частині 1 ви використовували **Foundry Local CLI** для завантаження та інтерактивного запуску моделей. CLI чудово підходить для досліджень, але коли ви створюєте реальні застосунки, потрібен **програмний контроль**. Foundry Local SDK надає це — він керує **контрольним рівнем** (запуск служби, виявлення моделей, завантаження, ідентифікація) так, щоб код вашого застосунку міг зосередитись на **рівні даних** (відправка запитів, отримання результатів).

Ця лабораторна робота навчить вас повному API SDK на Python, JavaScript та C#. Наприкінці ви розумітимете кожний доступний метод і коли його використовувати.

## Навчальні цілі

Наприкінці цієї лабораторної роботи ви зможете:

- Пояснити, чому SDK переважає CLI для розробки застосунків
- Встановити Foundry Local SDK для Python, JavaScript або C#
- Використовувати `FoundryLocalManager` для запуску служби, керування моделями та запиту каталогу
- Програмно перелічувати, завантажувати, завантажувати в пам’ять і розвантажувати моделі
- Перевіряти метадані моделей за допомогою `FoundryModelInfo`
- Розуміти різницю між каталогом, кешем і завантаженими моделями
- Використовувати конструктор bootstrap (Python) та патерн `create()` + каталог (JavaScript)
- Розуміти новий дизайн SDK для C# та його орієнтований на об’єкти API

---

## Передумови

| Вимога | Деталі |
|-------------|---------|
| **Foundry Local CLI** | Встановлено і є в `PATH` ([Частина 1](part1-getting-started.md)) |
| **Рuntime мови** | **Python 3.9+** та/або **Node.js 18+** та/або **.NET 9.0+** |

---

## Концепція: SDK vs CLI — Чому використовувати SDK?

| Аспект | CLI (команда `foundry`) | SDK (`foundry-local-sdk`) |
|--------|-------------------------|--------------------------|
| **Використання** | Дослідження, ручне тестування | Інтеграція у застосунок |
| **Керування сервісом** | Вручну: `foundry service start` | Автоматично: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Виявлення порту** | Зчитування з виводу CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Завантаження моделі** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Обробка помилок** | Код виходу, stderr | Винятки, типізовані помилки |
| **Автоматизація** | Скрипти shell | Інтеграція в рідну мову |
| **Розгортання** | Потрібен CLI на машині кінцевого користувача | SDK для C# може бути самодостатнім (CLI не потрібен) |

> **Ключовий висновок:** SDK управляє повним життєвим циклом: запуск служби, перевірка кешу, завантаження відсутніх моделей, їх завантаження та виявлення кінцевої точки — усе це за кілька рядків коду. Вашому застосунку не потрібно парсити вивід CLI або керувати підпроцесами.

---

## Лабораторні вправи

### Вправа 1: Встановити SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Перевірте встановлення:

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

Перевірте встановлення:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Існує два пакети NuGet:

| Пакет | Платформа | Опис |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Кросплатформний | Працює на Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Лише Windows | Додає апаратне прискорення WinML; завантажує і встановлює провайдери виконання плагінів (наприклад, QNN для Qualcomm NPU) |

**Налаштування для Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Відредагуйте файл `.csproj`:

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

> **Примітка:** На Windows пакет WinML являє собою надмножину, що включає базовий SDK і провайдера виконання QNN. На Linux/macOS використовується базовий SDK. Умовні TFM та посилання на пакети забезпечують повну кросплатформеність проекту.

Створіть `nuget.config` у корені проекту:

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

Відновіть пакети:

```bash
dotnet restore
```

</details>

---

### Вправа 2: Запустити службу та отримати каталог

Перш за все будь-який застосунок запускає службу Foundry Local і дізнається, які моделі доступні.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Створіть менеджера та запустіть службу
manager = FoundryLocalManager()
manager.start_service()

# Перелічіть усі моделі, доступні в каталозі
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK - Методи керування службою

| Метод | Підпис | Опис |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Перевірити, чи служба запущена |
| `start_service()` | `() -> None` | Запустити службу Foundry Local |
| `service_uri` | `@property -> str` | Базова URI служби |
| `endpoint` | `@property -> str` | API кінцева точка (service URI + `/v1`) |
| `api_key` | `@property -> str` | Ключ API (з оточення або стандартний заповнювач) |

#### Python SDK - Методи керування каталогом

| Метод | Підпис | Опис |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Перелік усіх моделей у каталозі |
| `refresh_catalog()` | `() -> None` | Оновити каталог із служби |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Отримати інформацію про конкретну модель |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Створіть менеджера та запустіть службу
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Перегляньте каталог
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK - Методи менеджера

| Метод | Підпис | Опис |
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Ініціалізувати синглтон SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Доступ до синглтона менеджера |
| `manager.startWebService()` | `() => Promise<void>` | Запустити веб-службу Foundry Local |
| `manager.urls` | `string[]` | Масив базових URL служби |

#### JavaScript SDK - Методи каталогу і моделей

| Метод | Підпис | Опис |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Доступ до каталогу моделей |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Отримати об’єкт моделі за псевдонімом |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK версії 0.8.0+ використовує об’єктно-орієнтовану архітектуру з об’єктами `Configuration`, `Catalog` і `Model`:

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

#### C# SDK - Ключові класи

| Клас | Призначення |
|-------|---------|
| `Configuration` | Встановлює ім’я застосунку, рівень логування, директорію кешу, URL веб-сервера |
| `FoundryLocalManager` | Основна точка входу — створюється через `CreateAsync()`, доступна через `.Instance` |
| `Catalog` | Перегляд, пошук і отримання моделей із каталогу |
| `Model` | Представляє конкретну модель — завантаження, завантаження в пам’ять, отримання клієнтів |

#### C# SDK - Методи менеджера та каталогу

| Метод | Опис |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Ініціалізація менеджера |
| `FoundryLocalManager.Instance` | Доступ до синглтона менеджера |
| `manager.GetCatalogAsync()` | Отримати каталог моделей |
| `catalog.ListModelsAsync()` | Перелік усіх доступних моделей |
| `catalog.GetModelAsync(alias: "alias")` | Отримати конкретну модель за псевдонімом |
| `catalog.GetCachedModelsAsync()` | Перелік завантажених моделей |
| `catalog.GetLoadedModelsAsync()` | Перелік зараз завантажених у пам’ять моделей |

> **Примітка про архітектуру C#:** Переробка C# SDK v0.8.0+ робить застосунок **самодостатнім**; він не вимагає CLI Foundry Local на машині кінцевого користувача. SDK нативно керує моделями та інференсом.

</details>

---

### Вправа 3: Завантажити й завантажити модель у пам’ять

SDK розділяє завантаження (на диск) і завантаження (у пам’ять). Це дозволяє попередньо завантажувати моделі на етапі налаштування і завантажувати їх за потребою.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Варіант A: Покрокове виконання вручну
manager = FoundryLocalManager()
manager.start_service()

# Спочатку перевірте кеш
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

# Варіант B: Однорядковий bootstrap (рекомендовано)
# Передайте псевдонім у конструктор - він автоматично запускає сервіс, завантажує та завантажує дані
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Методи керування моделями

| Метод | Підпис | Опис |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Завантажити модель у локальний кеш |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Завантажити модель у сервер інференсу |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Розвантажити модель із сервера |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Перелік усіх завантажених зараз моделей |

#### Python - Методи керування кешем

| Метод | Підпис | Опис |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Отримати шлях до директорії кешу |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Перелік усіх завантажених моделей |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Покроковий підхід
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

#### JavaScript - Методи моделей

| Метод | Підпис | Опис |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Чи модель уже завантажена |
| `model.download()` | `() => Promise<void>` | Завантажити модель у локальний кеш |
| `model.load()` | `() => Promise<void>` | Завантажити в сервер інференсу |
| `model.unload()` | `() => Promise<void>` | Розвантажити із сервера |
| `model.id` | `string` | Унікальний ідентифікатор моделі |

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

#### C# - Методи моделей

| Метод | Опис |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Завантажити обрану варіанту |
| `model.LoadAsync()` | Завантажити модель у пам’ять |
| `model.UnloadAsync()` | Розвантажити модель |
| `model.SelectVariant(variant)` | Обрати конкретний варіант (CPU/GPU/NPU) |
| `model.SelectedVariant` | Поточний обраний варіант |
| `model.Variants` | Усі доступні варіанти цієї моделі |
| `model.GetPathAsync()` | Отримати локальний шлях до файлу |
| `model.GetChatClientAsync()` | Отримати нативний чат-клієнт (OpenAI SDK не потрібен) |
| `model.GetAudioClientAsync()` | Отримати аудіоклієнт для транскрипції |

</details>

---

### Вправа 4: Перегляд метаданих моделі

Об’єкт `FoundryModelInfo` містить багаті метадані про кожну модель. Розуміння цих полів допоможе обрати правильну модель для вашого застосунку.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Отримати детальну інформацію про конкретну модель
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

#### Поля FoundryModelInfo

| Поле | Тип | Опис |
|-------|------|-------------|
| `alias` | string | Коротка назва (наприклад, `phi-3.5-mini`) |
| `id` | string | Унікальний ідентифікатор моделі |
| `version` | string | Версія моделі |
| `task` | string | `chat-completions` або `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU або NPU |
| `execution_provider` | string | Рентайм-бекенд (CUDA, CPU, QNN, WebGPU тощо) |
| `file_size_mb` | int | Розмір на диску у мегабайтах |
| `supports_tool_calling` | bool | Чи підтримує модель виклик функцій/інструментів |
| `publisher` | string | Хто опублікував модель |
| `license` | string | Назва ліцензії |
| `uri` | string | URI моделі |
| `prompt_template` | dict/null | Шаблон підказки, якщо є |

---

### Вправа 5: Керування життєвим циклом моделі

Попрактикуйтеся у повному життєвому циклі: перелік → завантаження → завантаження в пам’ять → використання → розвантаження.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Маленька модель для швидкого тестування

manager = FoundryLocalManager()
manager.start_service()

# 1. Перевірте, що знаходиться в каталозі
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Перевірте, що вже завантажено
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Завантажте модель
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Переконайтеся, що вона тепер у кеші
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Завантажте її
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Перевірте, що завантажено
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Вивантажте її
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

const alias = "qwen2.5-0.5b"; // Маленька модель для швидкого тестування

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Отримати модель із каталогу
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Завантажити, якщо потрібно
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Завантажити її
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Вивантажити її
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Вправа 6: Патерни швидкого запуску

Кожна мова надає скорочення для запуску сервісу та завантаження моделі в одному виклику. Це **рекомендовані патерни** для більшості застосунків.

<details>
<summary><h3>🐍 Python — Ініціалізація через конструктор</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Передайте псевдонім у конструктор - він усе обробить:
# 1. Запускає службу, якщо вона не працює
# 2. Завантажує модель, якщо вона не кешована
# 3. Завантажує модель у сервер висновків
manager = FoundryLocalManager("phi-3.5-mini")

# Готово до використання відразу
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Параметр `bootstrap` (за замовчуванням `True`) керує цією поведінкою. Встановіть `bootstrap=False`, якщо хочете керувати вручну:

```python
# Ручний режим - нічого не відбувається автоматично
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript — `create()` + Каталог</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() опрацьовують все:
// 1. Запускає сервіс
// 2. Отримує модель з каталогу
// 3. Завантажує, якщо потрібно, і завантажує модель
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Готовий до використання негайно
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# — `CreateAsync()` + Каталог</h3></summary>

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

> **Примітка C#:** C# SDK використовує `Configuration` для керування назвою застосунку, логуванням, директоріями кешу та навіть фіксації конкретного порту вебсерверу. Це робить його найбільш конфігурованим серед трьох SDK.

</details>

---

### Вправа 7: Нативний ChatClient (OpenAI SDK не потрібен)

JavaScript та C# SDK надають зручний метод `createChatClient()`, який повертає нативного чат-клієнта — без необхідності окремо встановлювати чи конфігурувати OpenAI SDK.

<details>
<summary><h3>📘 JavaScript — <code>model.createChatClient()</code></h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Створити ChatClient безпосередньо з моделі — імпорт OpenAI не потрібен
const chatClient = model.createChatClient();

// completeChat повертає об’єкт відповіді, сумісний з OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Потокова передача використовує патерн зворотного виклику
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` також підтримує виклик інструментів — передавайте інструменти як другий аргумент:

```javascript
const response = await chatClient.completeChat(messages, tools);
```

</details>

<details>
<summary><h3>💜 C# — <code>model.GetChatClientAsync()</code></h3></summary>

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

> **Коли використовувати який патерн:**
> - **`createChatClient()`** — швидке прототипування, менше залежностей, простіший код
> - **OpenAI SDK** — повний контроль над параметрами (температура, top_p, стоп-токени тощо), краще для production-застосунків

---

### Вправа 8: Варіанти моделей та вибір апаратного забезпечення

Моделі можуть мати кілька **варіантів**, оптимізованих під різне апаратне забезпечення. SDK автоматично обирає найкращий варіант, але ви також можете переглянути й обрати вручну.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Перерахуйте всі доступні варіанти
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK автоматично вибирає найкращий варіант для вашого обладнання
// Щоб перевизначити, використовуйте selectVariant():
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

У Python SDK автоматично вибирає найкращий варіант залежно від апаратного забезпечення. Використовуйте `get_model_info()`, щоб побачити вибраний варіант:

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

#### Моделі з варіантами для NPU

Деякі моделі мають варіанти, оптимізовані під Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Модель | Варіант NPU доступний |
|--------|:---------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Порада:** На обладнанні з підтримкою NPU SDK автоматично вибирає NPU-варіант, якщо він доступний. Вам не потрібно змінювати код. Для проєктів на C# у Windows додайте пакет NuGet `Microsoft.AI.Foundry.Local.WinML` для увімкнення провайдера виконання QNN — QNN постачається як плагін EP через WinML.

---

### Вправа 9: Оновлення моделей та оновлення каталогу

Каталог моделей оновлюється періодично. Використовуйте ці методи, щоб перевіряти та застосовувати оновлення.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Оновіть каталог, щоб отримати останній список моделей
manager.refresh_catalog()

# Перевірте, чи є у кешованої моделі доступна новіша версія
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

// Оновіть каталог, щоб отримати останній список моделей
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Перелічіть усі доступні моделі після оновлення
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Вправа 10: Робота з моделями для логічного міркування

Модель **phi-4-mini-reasoning** містить ланцюжок логічного міркування. Вона обгортає внутрішнє мислення у теги `<think>...</think>` перед тим, як видасть остаточну відповідь. Це корисно для завдань, що потребують багатокрокової логіки, математики або розв'язання задач.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning займає приблизно 4.6 ГБ
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Модель обгортає свої думки в теги <think>...</think>
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

// Аналіз роздумів ланцюгом
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Коли використовувати моделі для міркування:**
> - Математичні та логічні задачі
> - Багатокрокове планування завдань
> - Генерація складного коду
> - Завдання, де показування робіт підвищує точність
>
> **Компроміс:** Моделі для міркування генерують більше токенів (сегмент `<think>`) і працюють повільніше. Для простих запитань і відповідей стандартна модель, як phi-3.5-mini, працює швидше.

---

### Вправа 11: Розуміння псевдонімів (аліасів) і вибір апаратного забезпечення

Коли ви передаєте **аліас** (наприклад, `phi-3.5-mini`) замість повного ID моделі, SDK автоматично обирає найкращий варіант для вашого обладнання:

| Апаратне забезпечення | Обраний виконавчий провайдер |
|-----------------------|------------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (через WinML плагін) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Будь-який пристрій (запасний варіант) | `CPUExecutionProvider` або `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Псевдонім відповідає найкращому варіанту для ВАШОГО обладнання
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Порада:** Завжди використовуйте аліаси у вашому коді застосунку. При розгортанні на комп’ютері користувача SDK вибирає оптимальний варіант під час виконання — CUDA для NVIDIA, QNN для Qualcomm, CPU в інших випадках.

---

### Вправа 12: Опції конфігурації в C#

Клас `Configuration` в C# SDK надає детальний контроль над часом виконання:

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

| Властивість | Значення за замовчуванням | Опис |
|-------------|---------------------------|------|
| `AppName` | (обов’язкове) | Назва вашого застосунку |
| `LogLevel` | `Information` | Рівень деталізації логів |
| `Web.Urls` | (динамічний) | Фіксація конкретного порту для вебсерверу |
| `AppDataDir` | За замовчуванням ОС | Базова директорія для даних застосунку |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Де зберігаються моделі |
| `LogsDir` | `{AppDataDir}/logs` | Де пишуться логи |

---

### Вправа 13: Використання у браузері (тільки JavaScript)

JavaScript SDK включає версію, сумісну з браузером. У браузері сервіс потрібно запускати вручну через CLI та вказувати URL хоста:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Спочатку запустіть службу вручну:
//   foundry service start
// Потім використайте URL з виводу CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Перегляньте каталог
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Обмеження браузера:** Версія для браузера **не підтримує** `startWebService()`. Ви повинні впевнитися, що сервіс Foundry Local уже запущений, перш ніж використовувати SDK у браузері.

---

## Повний довідник API

### Python

| Категорія | Метод | Опис |
|-----------|-------|------|
| **Ініціалізація** | `FoundryLocalManager(alias?, bootstrap=True)` | Створити менеджера; опційно ініціалізувати з моделлю |
| **Сервіс** | `is_service_running()` | Перевірити, чи сервіс запущений |
| **Сервіс** | `start_service()` | Запустити сервіс |
| **Сервіс** | `endpoint` | URL кінцевої точки API |
| **Сервіс** | `api_key` | API ключ |
| **Каталог** | `list_catalog_models()` | Перелік усіх доступних моделей |
| **Каталог** | `refresh_catalog()` | Оновити каталог |
| **Каталог** | `get_model_info(alias_or_model_id)` | Отримати метадані моделі |
| **Кеш** | `get_cache_location()` | Шлях до директорії кешу |
| **Кеш** | `list_cached_models()` | Перелік завантажених моделей |
| **Модель** | `download_model(alias_or_model_id)` | Завантажити модель |
| **Модель** | `load_model(alias_or_model_id, ttl=600)` | Завантажити модель у пам’ять |
| **Модель** | `unload_model(alias_or_model_id)` | Вивантажити модель |
| **Модель** | `list_loaded_models()` | Перелік завантажених моделей |
| **Модель** | `is_model_upgradeable(alias_or_model_id)` | Перевірити наявність новішої версії |
| **Модель** | `upgrade_model(alias_or_model_id)` | Оновити модель до останньої версії |
| **Сервіс** | `httpx_client` | Попередньо налаштований HTTPX клієнт для прямих викликів API |

### JavaScript

| Категорія | Метод | Опис |
|-----------|-------|------|
| **Ініціалізація** | `FoundryLocalManager.create(options)` | Ініціалізувати синглтон SDK |
| **Ініціалізація** | `FoundryLocalManager.instance` | Доступ до синглтона менеджера |
| **Сервіс** | `manager.startWebService()` | Запустити вебсервіс |
| **Сервіс** | `manager.urls` | Масив базових URL сервісу |
| **Каталог** | `manager.catalog` | Доступ до каталогу моделей |
| **Каталог** | `catalog.getModel(alias)` | Отримати об’єкт моделі за аліасом (повертає Promise) |
| **Модель** | `model.isCached` | Чи модель завантажена локально |
| **Модель** | `model.download()` | Завантажити модель |
| **Модель** | `model.load()` | Завантажити модель у пам’ять |
| **Модель** | `model.unload()` | Вивантажити модель |
| **Модель** | `model.id` | Унікальний ідентифікатор моделі |
| **Модель** | `model.alias` | Аліас моделі |
| **Модель** | `model.createChatClient()` | Отримати нативного чат-клієнта (OpenAI SDK не потрібен) |
| **Модель** | `model.createAudioClient()` | Отримати аудіоклієнта для транскрипції |
| **Модель** | `model.removeFromCache()` | Видалити модель з локального кешу |
| **Модель** | `model.selectVariant(variant)` | Обрати конкретний апаратний варіант |
| **Модель** | `model.variants` | Масив доступних варіантів для цієї моделі |
| **Модель** | `model.isLoaded()` | Перевірити, чи модель завантажена |
| **Каталог** | `catalog.getModels()` | Перелік усіх доступних моделей |
| **Каталог** | `catalog.getCachedModels()` | Перелік завантажених моделей |
| **Каталог** | `catalog.getLoadedModels()` | Перелік моделей, завантажених у пам’ять |
| **Каталог** | `catalog.updateModels()` | Оновити каталог із сервісу |
| **Сервіс** | `manager.stopWebService()` | Зупинити вебсервіс Foundry Local |

### C# (v0.8.0+)

| Категорія | Метод | Опис |
|-----------|-------|------|
| **Ініціалізація** | `FoundryLocalManager.CreateAsync(config, logger)` | Ініціалізувати менеджера |
| **Ініціалізація** | `FoundryLocalManager.Instance` | Доступ до синглтона |
| **Каталог** | `manager.GetCatalogAsync()` | Отримати каталог |
| **Каталог** | `catalog.ListModelsAsync()` | Перелік усіх моделей |
| **Каталог** | `catalog.GetModelAsync(alias)` | Отримати конкретну модель |
| **Каталог** | `catalog.GetCachedModelsAsync()` | Перелік моделей у кеші |
| **Каталог** | `catalog.GetLoadedModelsAsync()` | Перелік завантажених моделей |
| **Модель** | `model.DownloadAsync(progress?)` | Завантажити модель |
| **Модель** | `model.LoadAsync()` | Завантажити модель у пам’ять |
| **Модель** | `model.UnloadAsync()` | Вивантажити модель |
| **Модель** | `model.SelectVariant(variant)` | Обрати апаратний варіант |
| **Модель** | `model.GetChatClientAsync()` | Отримати нативного чат-клієнта |
| **Модель** | `model.GetAudioClientAsync()` | Отримати аудіоклієнта для транскрипції |
| **Модель** | `model.GetPathAsync()` | Отримати локальний шлях до файлу |
| **Каталог** | `catalog.GetModelVariantAsync(alias, variant)` | Отримати конкретний апаратний варіант |
| **Каталог** | `catalog.UpdateModelsAsync()` | Оновити каталог |
| **Сервер** | `manager.StartWebServerAsync()` | Запустити REST вебсервер |
| **Сервер** | `manager.StopWebServerAsync()` | Зупинити REST вебсервер |
| **Конфіг** | `config.ModelCacheDir` | Директорія кешу |

---

## Основні висновки

| Концепція | Що ви дізналися |
|-----------|-----------------|
| **SDK проти CLI** | SDK надає програмне керування — необхідне для застосунків |
| **Контрольна площина** | SDK керує сервісами, моделями та кешем |
| **Динамічні порти** | Завжди використовуйте `manager.endpoint` (Python) або `manager.urls[0]` (JS/C#) — ніколи не прописуйте порт жорстко |
| **Аліаси** | Використовуйте аліаси для автоматичного вибору оптимального варіанта апаратного забезпечення |
| **Швидкий старт** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Переробка C#** | версія 0.8.0+ є автономною - CLI не потрібен на машинах кінцевих користувачів |
| **Життєвий цикл моделі** | Каталог → Завантаження → Завантаження в пам’ять → Використання → Вивантаження |
| **FoundryModelInfo** | Детальні метадані: завдання, пристрій, розмір, ліцензія, підтримка виклику інструментів |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) для використання без OpenAI |
| **Варіанти** | Моделі мають апаратно-специфічні варіанти (CPU, GPU, NPU); вибираються автоматично |
| **Оновлення** | Python: `is_model_upgradeable()` + `upgrade_model()` для підтримки моделей у актуальному стані |
| **Оновлення каталогу** | `refresh_catalog()` (Python) / `updateModels()` (JS) для пошуку нових моделей |

---

## Ресурси

| Ресурс | Посилання |
|----------|------|
| Посилання на SDK (всі мови) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Інтеграція з inference SDK | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Посилання на C# SDK API | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| Приклади C# SDK | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Вебсайт Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Наступні кроки

Продовжуйте до [Частина 3: Використання SDK з OpenAI](part3-sdk-and-apis.md), щоб підключити SDK до бібліотеки клієнта OpenAI і створити ваш перший застосунок для завершення чату.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Відмова від відповідальності**:
Цей документ було перекладено за допомогою сервісу автоматичного перекладу [Co-op Translator](https://github.com/Azure/co-op-translator). Хоча ми прагнемо до точності, будь ласка, майте на увазі, що автоматичні переклади можуть містити помилки або неточності. Оригінальний документ рідною мовою слід вважати авторитетним джерелом. Для критичної інформації рекомендується професійний людський переклад. Ми не несемо відповідальності за будь-які непорозуміння або неправильні тлумачення, що виникли внаслідок використання цього перекладу.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->