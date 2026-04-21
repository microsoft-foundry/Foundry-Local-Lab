![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Часть 2: Глубокое изучение Foundry Local SDK

> **Цель:** Освоить Foundry Local SDK для программного управления моделями, сервисами и кэшированием — и понять, почему SDK является рекомендуемым подходом по сравнению с CLI для создания приложений.

## Обзор

В Части 1 вы использовали **Foundry Local CLI** для загрузки и интерактивной работы с моделями. CLI отлично подходит для исследования, но при создании реальных приложений необходим **программный контроль**. Foundry Local SDK предоставляет именно это — он управляет **контрольной плоскостью** (запуск сервиса, обнаружение моделей, загрузка, выгрузка), чтобы код вашего приложения мог сосредоточиться на **данной плоскости** (отправка подсказок, получение результатов).

В этой лабораторной работе вы изучите полный набор API SDK на Python, JavaScript и C#. К концу вы поймёте каждый доступный метод и когда его следует использовать.

## Цели обучения

К концу лабораторной работы вы сможете:

- Объяснить, почему SDK предпочтительнее CLI для разработки приложений
- Установить Foundry Local SDK для Python, JavaScript или C#
- Использовать `FoundryLocalManager` для запуска сервиса, управления моделями и запроса каталога
- Программно перечислять, скачивать, загружать и выгружать модели
- Исследовать метаданные модели с помощью `FoundryModelInfo`
- Понимать разницу между каталогом, кэшем и загруженными моделями
- Использовать конструктор bootstrap (Python) и паттерн `create()` + каталог (JavaScript)
- Понимать редизайн C# SDK и его объектно-ориентированный API

---

## Требования

| Требование | Детали |
|------------|---------|
| **Foundry Local CLI** | Установлен и доступен в `PATH` ([Часть 1](part1-getting-started.md)) |
| **Среда выполнения** | **Python 3.9+** и/или **Node.js 18+** и/или **.NET 9.0+** |

---

## Концепция: SDK против CLI — Почему использовать SDK?

| Аспект | CLI (команда `foundry`) | SDK (`foundry-local-sdk`) |
|--------|-------------------------|---------------------------|
| **Сценарий использования** | Исследование, ручное тестирование | Интеграция в приложение |
| **Управление сервисом** | Вручную: `foundry service start` | Автоматически: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Обнаружение порта** | Чтение из вывода CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Загрузка модели** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Обработка ошибок** | Коды выхода, stderr | Исключения, типизированные ошибки |
| **Автоматизация** | Shell-скрипты | Интеграция на языке программирования |
| **Развертывание** | Требует CLI на машине пользователя | C# SDK может быть автономным (CLI не требуется) |

> **Ключевое понимание:** SDK управляет полным циклом жизни: запуск сервиса, проверка кэша, загрузка отсутствующих моделей, их загрузка в память и обнаружение конечной точки API — всё в нескольких строках кода. Вашему приложению не нужно разбирать вывод CLI или управлять подпроцессами.

---

## Лабораторные упражнения

### Упражнение 1: Установка SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Проверьте установку:

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

Проверьте установку:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Существует два пакета NuGet:

| Пакет | Платформа | Описание |
|--------|-----------|----------|
| `Microsoft.AI.Foundry.Local` | Кроссплатформенный | Работает на Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Только Windows | Добавляет апаратное ускорение WinML; загружает и устанавливает провайдеры исполнения плагинов (например, QNN для Qualcomm NPU) |

**Настройка для Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Отредактируйте файл `.csproj`:

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

> **Примечание:** На Windows пакет WinML — это надмножество, которое включает базовый SDK и провайдер исполнения QNN. На Linux/macOS используется базовый SDK. Условные TFM и ссылки на пакеты обеспечивают кроссплатформенность проекта.

Создайте `nuget.config` в корне проекта:

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

Восстановите пакеты:

```bash
dotnet restore
```

</details>

---

### Упражнение 2: Запуск сервиса и перечисление каталога

Первое, что делает любое приложение — это запуск Foundry Local сервиса и обнаружение доступных моделей.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Создайте менеджера и запустите службу
manager = FoundryLocalManager()
manager.start_service()

# Выведите список всех моделей, доступных в каталоге
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Методы управления сервисом в Python SDK

| Метод | Сигнатура | Описание |
|-------|-----------|----------|
| `is_service_running()` | `() -> bool` | Проверяет, запущен ли сервис |
| `start_service()` | `() -> None` | Запускает Foundry Local сервис |
| `service_uri` | `@property -> str` | Базовый URI сервиса |
| `endpoint` | `@property -> str` | Конечная точка API (service URI + `/v1`) |
| `api_key` | `@property -> str` | Ключ API (из окружения или заглушка по умолчанию) |

#### Методы управления каталогом в Python SDK

| Метод | Сигнатура | Описание |
|-------|-----------|----------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Перечислить все модели из каталога |
| `refresh_catalog()` | `() -> None` | Обновить каталог из сервиса |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Получить информацию о конкретной модели |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Создайте менеджера и запустите сервис
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Просмотрите каталог
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### Методы менеджера в JavaScript SDK

| Метод | Сигнатура | Описание |
|--------|-----------|----------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Инициализация синглтона SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Доступ к синглтону менеджера |
| `manager.startWebService()` | `() => Promise<void>` | Запуск веб-сервиса Foundry Local |
| `manager.urls` | `string[]` | Массив базовых URL сервиса |

#### Методы каталога и моделей в JavaScript SDK

| Метод | Сигнатура | Описание |
|--------|-----------|----------|
| `manager.catalog` | `Catalog` | Доступ к каталогу моделей |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Получить объект модели по псевдониму |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

C# SDK версии v0.8.0+ использует объектно-ориентированную архитектуру с объектами `Configuration`, `Catalog` и `Model`:

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

#### Ключевые классы C# SDK

| Класс | Назначение |
|-------|------------|
| `Configuration` | Установка имени приложения, уровня логирования, директории кэша, URL веб-сервера |
| `FoundryLocalManager` | Главная точка входа — создаётся через `CreateAsync()`, доступ через `.Instance` |
| `Catalog` | Просмотр, поиск и получение моделей из каталога |
| `Model` | Представляет конкретную модель — загрузка, выгрузка, получение клиентов |

#### Методы менеджера и каталога C# SDK

| Метод | Описание |
|--------|----------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Инициализация менеджера |
| `FoundryLocalManager.Instance` | Доступ к синглтону менеджера |
| `manager.GetCatalogAsync()` | Получение каталога моделей |
| `catalog.ListModelsAsync()` | Перечисление всех доступных моделей |
| `catalog.GetModelAsync(alias: "alias")` | Получение конкретной модели по псевдониму |
| `catalog.GetCachedModelsAsync()` | Перечисление загруженных моделей |
| `catalog.GetLoadedModelsAsync()` | Перечисление текущих загруженных в память моделей |

> **Примечание по архитектуре C#:** Редизайн C# SDK v0.8.0+ делает приложение **самодостаточным**; он не требует Foundry Local CLI на машине конечного пользователя. SDK самостоятельно управляет моделями и инференсом.

</details>

---

### Упражнение 3: Загрузка и активация модели

SDK разделяет загрузку (на диск) и активацию (в память). Это позволяет предварительно загрузить модели во время настройки и активировать их по требованию.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Вариант A: Ручной поэтапный процесс
manager = FoundryLocalManager()
manager.start_service()

# Сначала проверьте кэш
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

# Вариант B: Однострочный bootstrap (рекомендуется)
# Передайте псевдоним в конструктор — он запускает сервис, загружает и подгружает автоматически
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Методы управления моделями в Python

| Метод | Сигнатура | Описание |
|--------|-----------|----------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Скачивает модель в локальный кэш |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Загружает модель в сервер инференса |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Выгружает модель из сервера |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Перечисляет все загруженные модели |

#### Методы управления кэшем в Python

| Метод | Сигнатура | Описание |
|--------|-----------|----------|
| `get_cache_location()` | `() -> str` | Получить путь к директории кэша |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Перечислить все загруженные модели |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Поэтапный подход
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

#### Методы моделей в JavaScript

| Метод | Сигнатура | Описание |
|--------|-----------|----------|
| `model.isCached` | `boolean` | Модель уже загружена на диск |
| `model.download()` | `() => Promise<void>` | Скачивает модель в локальный кэш |
| `model.load()` | `() => Promise<void>` | Загружает модель на сервер инференса |
| `model.unload()` | `() => Promise<void>` | Выгружает модель с сервера инференса |
| `model.id` | `string` | Уникальный идентификатор модели |

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

#### Методы моделей в C#

| Метод | Описание |
|--------|----------|
| `model.DownloadAsync(progress?)` | Скачивание выбранного варианта модели |
| `model.LoadAsync()` | Загрузка модели в память |
| `model.UnloadAsync()` | Выгрузка модели |
| `model.SelectVariant(variant)` | Выбор конкретного варианта (CPU/GPU/NPU) |
| `model.SelectedVariant` | Текущий выбранный вариант |
| `model.Variants` | Все доступные варианты этой модели |
| `model.GetPathAsync()` | Получить локальный путь к файлу |
| `model.GetChatClientAsync()` | Получить нативного клиента для чата (OpenAI SDK не требуется) |
| `model.GetAudioClientAsync()` | Получить аудиоклиента для транскрипции |

</details>

---

### Упражнение 4: Исследование метаданных модели

Объект `FoundryModelInfo` содержит богатые метаданные о каждой модели. Понимание этих полей помогает выбрать правильную модель для вашего приложения.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Получить подробную информацию о конкретной модели
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

| Поле | Тип | Описание |
|-------|------|----------|
| `alias` | string | Краткое имя (например, `phi-3.5-mini`) |
| `id` | string | Уникальный идентификатор модели |
| `version` | string | Версия модели |
| `task` | string | `chat-completions` или `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU или NPU |
| `execution_provider` | string | Бэкенд выполнения (CUDA, CPU, QNN, WebGPU и др.) |
| `file_size_mb` | int | Размер на диске в МБ |
| `supports_tool_calling` | bool | Поддерживает ли модель вызов функций/инструментов |
| `publisher` | string | Издатель модели |
| `license` | string | Лицензия |
| `uri` | string | URI модели |
| `prompt_template` | dict/null | Шаблон подсказки, если есть |

---

### Упражнение 5: Управление жизненным циклом модели

Практикуйтесь с полным циклом: перечисление → загрузка → активация → использование → выгрузка.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Маленькая модель для быстрого тестирования

manager = FoundryLocalManager()
manager.start_service()

# 1. Проверьте, что находится в каталоге
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Проверьте, что уже загружено
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Загрузите модель
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Убедитесь, что она теперь в кэше
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Загрузите ее
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Проверьте, что загружено
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Вынесите ее из памяти
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

const alias = "qwen2.5-0.5b"; // Маленькая модель для быстрого тестирования

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Получить модель из каталога
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Скачать при необходимости
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Загрузить её
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Выгрузить её
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Упражнение 6: Шаблоны Быстрого Старта

Каждый язык предоставляет ярлык для запуска сервиса и загрузки модели одним вызовом. Это **рекомендуемые шаблоны** для большинства приложений.

<details>
<summary><h3>🐍 Python - Инициализация через Конструктор</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Передайте псевдоним конструктору - он сделает всё:
# 1. Запускает сервис, если он не запущен
# 2. Загружает модель, если она не в кэше
# 3. Загружает модель на сервер вывода
manager = FoundryLocalManager("phi-3.5-mini")

# Готов к использованию сразу же
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Параметр `bootstrap` (по умолчанию `True`) контролирует это поведение. Установите `bootstrap=False`, если хотите управлять вручную:

```python
# Ручной режим - ничего не происходит автоматически
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Каталог</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() обрабатывают всё:
// 1. Запускает сервис
// 2. Получает модель из каталога
// 3. Загружает модель при необходимости и инициализирует её
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Готово к использованию сразу
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

> **Примечание по C#:** SDK для C# использует `Configuration` для управления именем приложения, логированием, каталогами кэша и даже закрепления конкретного порта веб-сервера. Это делает его самым настраиваемым из трех SDK.

</details>

---

### Упражнение 7: Встроенный ChatClient (Не нужен OpenAI SDK)

SDK для JavaScript и C# предоставляет удобный метод `createChatClient()`, который возвращает нативный чат-клиент — установка или настройка OpenAI SDK не требуется.

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

// Создайте ChatClient непосредственно из модели — импорт OpenAI не требуется
const chatClient = model.createChatClient();

// completeChat возвращает объект ответа, совместимый с OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Потоковая передача использует шаблон обратного вызова
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` также поддерживает вызов инструментов — передавайте инструменты вторым аргументом:

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

> **Когда использовать тот или иной шаблон:**
> - **`createChatClient()`** — Быстрая прототипизация, меньше зависимостей, проще код
> - **OpenAI SDK** — Полный контроль параметров (temperature, top_p, стоп-токены и др.), лучше для продакшен-приложений

---

### Упражнение 8: Варианты Моделей и Выбор Аппаратного Обеспечения

У моделей могут быть несколько **вариантов**, оптимизированных под разное железо. SDK выбирает лучший вариант автоматически, но вы можете просмотреть и выбрать вручную.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Перечислите все доступные варианты
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK автоматически выбирает лучший вариант для вашего оборудования
// Чтобы переопределить, используйте selectVariant():
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

В Python SDK автоматически выбирает лучший вариант на основе железа. Используйте `get_model_info()`, чтобы увидеть, что выбрано:

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

#### Модели с Вариантами для NPU

Некоторые модели имеют варианты, оптимизированные под устройства с нейронными процессорными блоками (Qualcomm Snapdragon, Intel Core Ultra):

| Модель | Вариант для NPU доступен |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Совет:** На оборудовании с NPU SDK автоматически выбирает вариант для NPU, если он доступен. Вам не нужно менять код. Для проектов C# на Windows добавьте пакет NuGet `Microsoft.AI.Foundry.Local.WinML`, чтобы включить провайдер исполнения QNN — QNN поставляется как плагин EP через WinML.

---

### Упражнение 9: Обновления Моделей и Обновление Каталога

Каталог моделей обновляется периодически. Используйте эти методы для проверки и применения обновлений.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Обновить каталог, чтобы получить последний список моделей
manager.refresh_catalog()

# Проверить, есть ли у кэшированной модели более новая версия
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

// Обновить каталог, чтобы получить список последних моделей
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Показать все доступные модели после обновления
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Упражнение 10: Работа с Моделями для Рассуждения

Модель **phi-4-mini-reasoning** включает рассуждения с цепочкой мыслей. Она оборачивает свой внутренний процесс в теги `<think>...</think>` перед выдачей окончательного ответа. Это полезно для задач, требующих многошаговой логики, математики или решения проблем.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning занимает ~4.6 ГБ
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Модель оборачивает своё мышление в теги <think>...</think>
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

// Анализ пошагового мышления
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Когда использовать модели рассуждения:**
> - Математические и логические задачи
> - Многошаговое планирование
> - Сложная генерация кода
> - Задачи, где показ правильных рассуждений улучшает точность
>
> **Компромисс:** Модели рассуждения генерируют больше токенов (секция `<think>`) и работают медленнее. Для простых вопросов и ответов стандартная модель вроде phi-3.5-mini быстрее.

---

### Упражнение 11: Понимание Алиасов и Выбор Аппаратного Обеспечения

Если вы передаёте **алиас** (например, `phi-3.5-mini`) вместо полного ID модели, SDK автоматически выбирает лучший вариант для вашего железа:

| Аппаратное обеспечение | Выбранный провайдер исполнения |
|------------------------|------------------------------|
| NVIDIA GPU (CUDA)       | `CUDAExecutionProvider`       |
| Qualcomm NPU           | `QNNExecutionProvider` (через WinML плагин) |
| Intel NPU              | `OpenVINOExecutionProvider`   |
| AMD GPU                | `VitisAIExecutionProvider`    |
| NVIDIA RTX             | `NvTensorRTRTXExecutionProvider` |
| Любое устройство (резерв) | `CPUExecutionProvider` или `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Псевдоним разрешается в лучший вариант для ВАШЕГО оборудования
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Совет:** Всегда используйте алиасы в коде приложения. При развёртывании на машинах пользователей SDK выбирает оптимальный вариант в рантайме – CUDA для NVIDIA, QNN для Qualcomm, CPU для остальных.

---

### Упражнение 12: Опции Конфигурации C#

Класс `Configuration` в SDK для C# предоставляет тонкую настройку работы:

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

| Свойство | По умолчанию | Описание |
|----------|--------------|----------|
| `AppName` | (обязательно) | Имя вашего приложения |
| `LogLevel` | `Information` | Уровень детализации логов |
| `Web.Urls` | (динамически) | Закрепить конкретный порт для веб-сервера |
| `AppDataDir` | По умолчанию ОС | Базовый каталог данных приложения |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Где хранятся модели |
| `LogsDir` | `{AppDataDir}/logs` | Куда записываются логи |

---

### Упражнение 13: Использование в Браузере (только JavaScript)

JavaScript SDK включает браузерную версию. В браузере нужно вручную запускать сервис через CLI и указывать URL хоста:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Сначала запустите службу вручную:
//   foundry service start
// Затем используйте URL из вывода CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Просмотрите каталог
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Ограничения браузера:** Версия для браузера **не поддерживает** `startWebService()`. Вы должны убедиться, что сервис Foundry Local уже запущен перед использованием SDK в браузере.

---

## Полный Справочник API

### Python

| Категория | Метод | Описание |
|----------|--------|----------|
| **Инициализация** | `FoundryLocalManager(alias?, bootstrap=True)` | Создать менеджер; опционально запустить с моделью |
| **Сервис** | `is_service_running()` | Проверить, запущен ли сервис |
| **Сервис** | `start_service()` | Запустить сервис |
| **Сервис** | `endpoint` | URL API endpoint |
| **Сервис** | `api_key` | Ключ API |
| **Каталог** | `list_catalog_models()` | Список всех доступных моделей |
| **Каталог** | `refresh_catalog()` | Обновить каталог |
| **Каталог** | `get_model_info(alias_or_model_id)` | Получить метаданные модели |
| **Кэш** | `get_cache_location()` | Путь к директории кэша |
| **Кэш** | `list_cached_models()` | Список скачанных моделей |
| **Модель** | `download_model(alias_or_model_id)` | Скачать модель |
| **Модель** | `load_model(alias_or_model_id, ttl=600)` | Загрузить модель |
| **Модель** | `unload_model(alias_or_model_id)` | Выгрузить модель |
| **Модель** | `list_loaded_models()` | Список загруженных моделей |
| **Модель** | `is_model_upgradeable(alias_or_model_id)` | Проверить доступность обновления |
| **Модель** | `upgrade_model(alias_or_model_id)` | Обновить модель до последней версии |
| **Сервис** | `httpx_client` | Преднастроенный HTTPX клиент для непосредственных вызовов API |

### JavaScript

| Категория | Метод | Описание |
|----------|--------|----------|
| **Инициализация** | `FoundryLocalManager.create(options)` | Инициализировать синглтон SDK |
| **Инициализация** | `FoundryLocalManager.instance` | Доступ к синглтону менеджера |
| **Сервис** | `manager.startWebService()` | Запустить веб-сервис |
| **Сервис** | `manager.urls` | Массив базовых URL сервиса |
| **Каталог** | `manager.catalog` | Доступ к каталогу моделей |
| **Каталог** | `catalog.getModel(alias)` | Получить объект модели по алиасу (возвращает Promise) |
| **Модель** | `model.isCached` | Модель загружена в кэш? |
| **Модель** | `model.download()` | Скачать модель |
| **Модель** | `model.load()` | Загрузить модель |
| **Модель** | `model.unload()` | Выгрузить модель |
| **Модель** | `model.id` | Уникальный идентификатор модели |
| **Модель** | `model.alias` | Алиас модели |
| **Модель** | `model.createChatClient()` | Получить нативный чат-клиент (без OpenAI SDK) |
| **Модель** | `model.createAudioClient()` | Получить аудио-клиент для транскрипции |
| **Модель** | `model.removeFromCache()` | Удалить модель из локального кэша |
| **Модель** | `model.selectVariant(variant)` | Выбрать конкретный аппаратный вариант |
| **Модель** | `model.variants` | Массив доступных вариантов модели |
| **Модель** | `model.isLoaded()` | Проверить, загружена ли модель сейчас |
| **Каталог** | `catalog.getModels()` | Список всех доступных моделей |
| **Каталог** | `catalog.getCachedModels()` | Список скачанных моделей |
| **Каталог** | `catalog.getLoadedModels()` | Список загруженных моделей |
| **Каталог** | `catalog.updateModels()` | Обновить каталог с сервиса |
| **Сервис** | `manager.stopWebService()` | Остановить веб-сервис Foundry Local |

### C# (v0.8.0+)

| Категория | Метод | Описание |
|----------|--------|----------|
| **Инициализация** | `FoundryLocalManager.CreateAsync(config, logger)` | Инициализировать менеджер |
| **Инициализация** | `FoundryLocalManager.Instance` | Доступ к синглтону |
| **Каталог** | `manager.GetCatalogAsync()` | Получить каталог |
| **Каталог** | `catalog.ListModelsAsync()` | Список всех моделей |
| **Каталог** | `catalog.GetModelAsync(alias)` | Получить конкретную модель |
| **Каталог** | `catalog.GetCachedModelsAsync()` | Список моделей в кэше |
| **Каталог** | `catalog.GetLoadedModelsAsync()` | Список загруженных моделей |
| **Модель** | `model.DownloadAsync(progress?)` | Скачать модель |
| **Модель** | `model.LoadAsync()` | Загрузить модель |
| **Модель** | `model.UnloadAsync()` | Выгрузить модель |
| **Модель** | `model.SelectVariant(variant)` | Выбрать аппаратный вариант |
| **Модель** | `model.GetChatClientAsync()` | Получить нативный чат-клиент |
| **Модель** | `model.GetAudioClientAsync()` | Получить аудиоклиента транскрипции |
| **Модель** | `model.GetPathAsync()` | Получить локальный путь к файлу |
| **Каталог** | `catalog.GetModelVariantAsync(alias, variant)` | Получить конкретный аппаратный вариант |
| **Каталог** | `catalog.UpdateModelsAsync()` | Обновить каталог |
| **Сервер** | `manager.StartWebServerAsync()` | Запустить REST веб-сервер |
| **Сервер** | `manager.StopWebServerAsync()` | Остановить REST веб-сервер |
| **Конфигурация** | `config.ModelCacheDir` | Каталог кэша |

---

## Основные Выводы

| Концепция | Чему вы научились |
|---------|------------------|
| **SDK vs CLI** | SDK обеспечивает программное управление — важно для приложений |
| **Плоскость управления** | SDK управляет сервисами, моделями и кэшированием |
| **Динамические порты** | Всегда используйте `manager.endpoint` (Python) или `manager.urls[0]` (JS/C#) — никогда не хардкодьте порт |
| **Алиасы** | Используйте алиасы для автоматического выбора оптимальной модели под аппаратное обеспечение |
| **Быстрый старт** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Редизайн C#** | Версия 0.8.0+ автономна — CLI не требуется на машинах конечных пользователей |
| **Жизненный цикл модели** | Каталог → Загрузка → Инициализация → Использование → Выгрузка |
| **FoundryModelInfo** | Обширные метаданные: задача, устройство, размер, лицензия, поддержка вызова инструмента |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) для использования без OpenAI |
| **Варианты** | Модели имеют аппаратно-специфичные варианты (CPU, GPU, NPU); выбор происходит автоматически |
| **Обновления** | Python: `is_model_upgradeable()` + `upgrade_model()` для поддержания моделей в актуальном состоянии |
| **Обновление каталога** | `refresh_catalog()` (Python) / `updateModels()` (JS) для обнаружения новых моделей |

---

## Ресурсы

| Ресурс | Ссылка |
|----------|------|
| Справочник SDK (все языки) | [Microsoft Learn - Справочник по Foundry Local SDK](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Интеграция с SDK для вывода | [Microsoft Learn - Интеграция с Inference SDK](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Справочник API C# SDK | [Справочник Foundry Local C# API](https://aka.ms/fl-csharp-api-ref) |
| Примеры C# SDK | [GitHub - Примеры Foundry Local SDK](https://aka.ms/foundrylocalSDK) |
| Вебсайт Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Следующие шаги

Продолжите с [Часть 3: Использование SDK с OpenAI](part3-sdk-and-apis.md), чтобы подключить SDK к библиотеке клиента OpenAI и создать ваше первое приложение для завершения чата.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Отказ от ответственности**:  
Этот документ был переведен с помощью сервиса автоматического перевода [Co-op Translator](https://github.com/Azure/co-op-translator). Несмотря на стремление к точности, просим учитывать, что автоматический перевод может содержать ошибки или неточности. Оригинальный документ на его родном языке следует считать авторитетным источником. Для критически важной информации рекомендуется профессиональный перевод человеком. Мы не несем ответственности за любые недоразумения или искажения смысла, возникшие в результате использования данного перевода.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->