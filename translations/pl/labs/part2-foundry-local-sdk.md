![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Część 2: Dogłębne poznanie Foundry Local SDK

> **Cel:** Opanować Foundry Local SDK do programowego zarządzania modelami, usługami i pamięcią podręczną – oraz zrozumieć, dlaczego SDK jest zalecanym podejściem w porównaniu do CLI przy tworzeniu aplikacji.

## Przegląd

W części 1 używałeś **Foundry Local CLI** do pobierania i interaktywnego uruchamiania modeli. CLI jest świetne do eksploracji, ale gdy tworzysz rzeczywiste aplikacje, potrzebujesz **programowej kontroli**. Foundry Local SDK daje Ci to – zarządza **planem kontrolnym** (uruchamianie usługi, wykrywanie modeli, pobieranie, ładowanie), aby Twój kod aplikacji mógł się skoncentrować na **planie danych** (wysyłanie promptów, odbiór wyników).

Ten kurs uczy pełnego API SDK w Pythonie, JavaScript i C#. Na końcu zrozumiesz każdą dostępną metodę i kiedy jej używać.

## Cele nauki

Na zakończenie tego kursu będziesz umiał:

- Wyjaśnić, dlaczego SDK jest preferowane nad CLI do tworzenia aplikacji
- Zainstalować Foundry Local SDK dla Pythona, JavaScriptu lub C#
- Używać `FoundryLocalManager` do uruchamiania usługi, zarządzania modelami i zapytań katalogu
- Programowo wylistować, pobrać, załadować i odładować modele
- Zbadać metadane modelu korzystając z `FoundryModelInfo`
- Rozumieć różnice między katalogiem, pamięcią podręczną i załadowanymi modelami
- Używać konstruktora bootstrap (Python) i wzorca `create()` + katalog (JavaScript)
- Zrozumieć redesign SDK w C# i jego obiektowe API

---

## Wymagania wstępne

| Wymaganie | Szczegóły |
|-------------|---------|
| **Foundry Local CLI** | Zainstalowane i w Twojej zmiennej `PATH` ([Część 1](part1-getting-started.md)) |
| **Środowisko uruchomieniowe języka** | **Python 3.9+** i/lub **Node.js 18+** i/lub **.NET 9.0+** |

---

## Koncepcja: SDK vs CLI – Dlaczego korzystać z SDK?

| Aspekt | CLI (polecenie `foundry`) | SDK (`foundry-local-sdk`) |
|--------|---------------------------|---------------------------|
| **Przypadek użycia** | Eksploracja, testowanie ręczne | Integracja aplikacji |
| **Zarządzanie usługą** | Ręczne: `foundry service start` | Automatyczne: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Wykrywanie portu** | Odczyt z wyjścia CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Pobieranie modelu** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Obsługa błędów** | Kody wyjścia, stderr | Wyjątki, typowane błędy |
| **Automatyzacja** | Skrypty powłoki | Natywna integracja języka |
| **Wdrażanie** | Wymaga CLI na komputerze użytkownika końcowego | SDK C# może być samodzielne (nie wymaga CLI) |

> **Kluczowa wskazówka:** SDK obsługuje cały cykl życia: uruchamianie usługi, sprawdzanie pamięci podręcznej, pobieranie brakujących modeli, ich ładowanie i wykrywanie punktu końcowego w kilku liniach kodu. Twoja aplikacja nie musi analizować wyjścia CLI ani zarządzać procesami potomnymi.

---

## Ćwiczenia laboratoryjne

### Ćwiczenie 1: Zainstaluj SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Zweryfikuj instalację:

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

Zweryfikuj instalację:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Są dwie paczki NuGet:

| Paczka | Platforma | Opis |
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Wieloplatformowa | Działa na Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Tylko Windows | Dodaje akcelerację sprzętową WinML; pobiera i instaluje wykonawcze wtyczki (np. QNN dla Qualcomm NPU) |

**Konfiguracja Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Edytuj plik `.csproj`:

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

> **Uwaga:** Na Windows paczka WinML to superset, który zawiera podstawowe SDK plus wykonawcę QNN. Na Linux/macOS używane jest podstawowe SDK. Warunkowe TFM i referencje do pakietów zapewniają pełną wieloplatformowość projektu.

Utwórz `nuget.config` w katalogu głównym projektu:

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

Przywróć pakiety:

```bash
dotnet restore
```

</details>

---

### Ćwiczenie 2: Uruchom usługę i wylistuj katalog

Pierwszą rzeczą, jaką robi każda aplikacja, jest uruchomienie usługi Foundry Local i odkrycie dostępnych modeli.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Utwórz menedżera i uruchom usługę
manager = FoundryLocalManager()
manager.start_service()

# Wyświetl wszystkie modele dostępne w katalogu
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK – Metody zarządzania usługą

| Metoda | Sygnatura | Opis |
|--------|-----------|------|
| `is_service_running()` | `() -> bool` | Sprawdza, czy usługa działa |
| `start_service()` | `() -> None` | Uruchamia usługę Foundry Local |
| `service_uri` | `@property -> str` | Bazowy URI usługi |
| `endpoint` | `@property -> str` | Punkt końcowy API (URI usługi + `/v1`) |
| `api_key` | `@property -> str` | Klucz API (z env lub domyślny placeholder) |

#### Python SDK – Metody zarządzania katalogiem

| Metoda | Sygnatura | Opis |
|--------|-----------|------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Wypisuje wszystkie modele w katalogu |
| `refresh_catalog()` | `() -> None` | Odświeża katalog z usługi |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Pobiera informacje o konkretnym modelu |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Utwórz menedżera i uruchom usługę
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Przeglądaj katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK – Metody managera

| Metoda | Sygnatura | Opis |
|--------|-----------|------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inicjalizuje singleton SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Dostęp do singletona managera |
| `manager.startWebService()` | `() => Promise<void>` | Uruchamia lokalną usługę web Foundry |
| `manager.urls` | `string[]` | Tablica bazowych URL usług |

#### JavaScript SDK – Metody katalogu i modelu

| Metoda | Sygnatura | Opis |
|--------|-----------|------|
| `manager.catalog` | `Catalog` | Dostęp do katalogu modeli |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Pobiera obiekt modelu po aliasie |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

SDK C# v0.8.0+ używa architektury obiektowej z obiektami `Configuration`, `Catalog` i `Model`:

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

#### C# SDK – Kluczowe klasy

| Klasa | Cel |
|-------|-----|
| `Configuration` | Ustawia nazwę aplikacji, poziom logów, katalog cache, URL web serwera |
| `FoundryLocalManager` | Główne wejście – tworzone przez `CreateAsync()`, dostęp przez `.Instance` |
| `Catalog` | Przeglądanie, wyszukiwanie i pobieranie modeli z katalogu |
| `Model` | Reprezentuje konkretny model – pobieranie, ładowanie, dostęp do klientów |

#### C# SDK – Metody managera i katalogu

| Metoda | Opis |
|--------|------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inicjalizuje managera |
| `FoundryLocalManager.Instance` | Dostęp do singletona managera |
| `manager.GetCatalogAsync()` | Pobiera katalog modeli |
| `catalog.ListModelsAsync()` | Wypisuje wszystkie dostępne modele |
| `catalog.GetModelAsync(alias: "alias")` | Pobiera konkretny model po aliasie |
| `catalog.GetCachedModelsAsync()` | Wypisuje pobrane modele |
| `catalog.GetLoadedModelsAsync()` | Wypisuje aktualnie załadowane modele |

> **Uwaga architektoniczna C#:** SDK C# v0.8.0+ jest projektowane jako **samowystarczalne**; nie wymaga Foundry Local CLI na komputerze użytkownika. SDK obsługuje zarządzanie modelami i inferencję natywnie.

</details>

---

### Ćwiczenie 3: Pobierz i załaduj model

SDK rozdziela pobieranie (na dysk) od ładowania (do pamięci). Pozwala to wstępnie pobrać modele podczas konfiguracji i ładować je na żądanie.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Opcja A: Ręczny krok po kroku
manager = FoundryLocalManager()
manager.start_service()

# Najpierw sprawdź pamięć podręczną
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

# Opcja B: Jednolinijkowy bootstrap (zalecany)
# Przekaż alias do konstruktora - automatycznie uruchamia usługę, pobiera i ładuje
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python – Metody zarządzania modelem

| Metoda | Sygnatura | Opis |
|--------|-----------|------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Pobierz model do lokalnej pamięci podręcznej |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Załaduj model do serwera inferencyjnego |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Odładuj model z serwera |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Wypisz wszystkie aktualnie załadowane modele |

#### Python – Metody zarządzania cache

| Metoda | Sygnatura | Opis |
|--------|-----------|------|
| `get_cache_location()` | `() -> str` | Pobierz ścieżkę katalogu cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Wypisz wszystkie pobrane modele |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Podejście krok po kroku
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

#### JavaScript – Metody modelu

| Metoda | Sygnatura | Opis |
|--------|-----------|------|
| `model.isCached` | `boolean` | Czy model jest już pobrany |
| `model.download()` | `() => Promise<void>` | Pobierz model do pamięci podręcznej |
| `model.load()` | `() => Promise<void>` | Załaduj do serwera inferencyjnego |
| `model.unload()` | `() => Promise<void>` | Odładuj z serwera inferencyjnego |
| `model.id` | `string` | Unikalny identyfikator modelu |

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

#### C# – Metody modelu

| Metoda | Opis |
|--------|------|
| `model.DownloadAsync(progress?)` | Pobierz wybraną wariantę |
| `model.LoadAsync()` | Załaduj model do pamięci |
| `model.UnloadAsync()` | Odładuj model |
| `model.SelectVariant(variant)` | Wybierz konkretną wariantę (CPU/GPU/NPU) |
| `model.SelectedVariant` | Aktualnie wybrana warianta |
| `model.Variants` | Wszystkie dostępne warianty tego modelu |
| `model.GetPathAsync()` | Pobierz lokalną ścieżkę pliku |
| `model.GetChatClientAsync()` | Pobierz natywnego klienta czatu (nie jest potrzebne SDK OpenAI) |
| `model.GetAudioClientAsync()` | Pobierz klienta audio do transkrypcji |

</details>

---

### Ćwiczenie 4: Zbadaj metadane modelu

Obiekt `FoundryModelInfo` zawiera bogate metadane o każdym modelu. Zrozumienie tych pól pomaga wybrać właściwy model dla Twojej aplikacji.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Pobierz szczegółowe informacje o konkretnym modelu
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

#### Pola FoundryModelInfo

| Pole | Typ | Opis |
|-------|------|------|
| `alias` | string | Krótka nazwa (np. `phi-3.5-mini`) |
| `id` | string | Unikalny identyfikator modelu |
| `version` | string | Wersja modelu |
| `task` | string | `chat-completions` lub `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU albo NPU |
| `execution_provider` | string | Backend wykonawczy (CUDA, CPU, QNN, WebGPU itd.) |
| `file_size_mb` | int | Rozmiar na dysku w MB |
| `supports_tool_calling` | bool | Czy model obsługuje wywołania funkcji/narzędzi |
| `publisher` | string | Kto wydał model |
| `license` | string | Nazwa licencji |
| `uri` | string | URI modelu |
| `prompt_template` | dict/null | Szablon promptu, jeśli istnieje |

---

### Ćwiczenie 5: Zarządzaj cyklem życia modelu

Przećwicz cały cykl życia: listuj → pobierz → załaduj → używaj → odładuj.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Mały model do szybkiego testowania

manager = FoundryLocalManager()
manager.start_service()

# 1. Sprawdź, co jest w katalogu
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Sprawdź, co jest już pobrane
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Pobierz model
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Zweryfikuj, że jest teraz w pamięci podręcznej
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Załaduj go
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Sprawdź, co jest załadowane
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Zwolnij go
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

const alias = "qwen2.5-0.5b"; // Mały model do szybkiego testowania

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Pobierz model z katalogu
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Pobierz, jeśli to konieczne
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Załaduj go
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Zwolnij go
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Ćwiczenie 6: Wzorce szybkiego startu

Każdy język oferuje skrót do uruchomienia usługi i załadowania modelu w jednym wywołaniu. Są to **zalecane wzorce** dla większości aplikacji.

<details>
<summary><h3>🐍 Python - Bootstrap konstruktora</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Przekaż alias do konstruktora - on zajmuje się wszystkim:
# 1. Uruchamia usługę, jeśli nie jest aktywna
# 2. Pobiera model, jeśli nie jest w pamięci podręcznej
# 3. Ładuje model do serwera inferencji
manager = FoundryLocalManager("phi-3.5-mini")

# Gotowy do natychmiastowego użycia
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Parametr `bootstrap` (domyślnie `True`) kontroluje to zachowanie. Ustaw `bootstrap=False`, jeśli chcesz kontrolować to ręcznie:

```python
# Tryb ręczny - nic się nie dzieje automatycznie
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() obsługuje wszystko:
// 1. Uruchamia usługę
// 2. Pobiera model z katalogu
// 3. Pobiera, jeśli to potrzebne, i ładuje model
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Gotowy do natychmiastowego użycia
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

> **Uwaga C#:** SDK w C# używa `Configuration` do kontrolowania nazwy aplikacji, logowania, katalogów podręcznych, a nawet przypinania konkretnego portu serwera sieciowego. Czyni to SDK najbardziej konfigurowalnym spośród trzech SDK.

</details>

---

### Ćwiczenie 7: Natywny ChatClient (bez potrzeby OpenAI SDK)

SDK JavaScript i C# oferują metodę wygody `createChatClient()`, która zwraca natywnego klienta czatu — nie ma potrzeby instalowania ani konfigurowania osobno OpenAI SDK.

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

// Utwórz ChatClient bezpośrednio z modelu — nie jest wymagany import OpenAI
const chatClient = model.createChatClient();

// completeChat zwraca obiekt odpowiedzi zgodny z OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Strumieniowanie wykorzystuje wzorzec wywołania zwrotnego
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

`ChatClient` obsługuje również wywoływanie narzędzi — przekaż narzędzia jako drugi argument:

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

> **Kiedy używać którego wzorca:**
> - **`createChatClient()`** — szybkie prototypowanie, mniej zależności, prostszy kod
> - **OpenAI SDK** — pełna kontrola nad parametrami (temperature, top_p, tokeny stopujące itd.), lepsze dla aplikacji produkcyjnych

---

### Ćwiczenie 8: Warianty modeli i wybór sprzętu

Modele mogą mieć wiele **wariantów** zoptymalizowanych pod różny sprzęt. SDK wybiera najlepszy wariant automatycznie, ale możesz też samodzielnie sprawdzić i wybrać.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Wypisz wszystkie dostępne warianty
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// SDK automatycznie wybiera najlepszy wariant dla Twojego sprzętu
// Aby nadpisać, użyj selectVariant():
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

W Pythonie SDK automatycznie wybiera najlepszy wariant bazując na sprzęcie. Użyj `get_model_info()`, aby zobaczyć, co zostało wybrane:

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

#### Modele z wariantami NPU

Niektóre modele mają warianty zoptymalizowane pod jednostki przetwarzania neuronowego (NPU) dla urządzeń z Neural Processing Unit (Qualcomm Snapdragon, Intel Core Ultra):

| Model | Wariant NPU dostępny |
|-------|:--------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Wskazówka:** Na sprzęcie obsługującym NPU SDK automatycznie wybiera wariant NPU, gdy jest dostępny. Nie musisz zmieniać swojego kodu. W projektach C# na Windows dodaj pakiet NuGet `Microsoft.AI.Foundry.Local.WinML`, aby włączyć dostawcę wykonywania QNN — QNN jest dostarczany jako wtyczka EP przez WinML.

---

### Ćwiczenie 9: Aktualizacje modeli i odświeżanie katalogu

Katalog modeli jest aktualizowany okresowo. Używaj tych metod, aby sprawdzić i zastosować aktualizacje.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Odśwież katalog, aby uzyskać najnowszą listę modeli
manager.refresh_catalog()

# Sprawdź, czy dostępna jest nowsza wersja przechowywanego w pamięci podręcznej modelu
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

// Odśwież katalog, aby pobrać najnowszą listę modeli
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Wyświetl wszystkie dostępne modele po odświeżeniu
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Ćwiczenie 10: Praca z modelami rozumującymi

Model **phi-4-mini-reasoning** zawiera łańcuch myślenia (chain-of-thought). Owinie swoje wewnętrzne rozumowanie w tagi `<think>...</think>` zanim wygeneruje ostateczną odpowiedź. To przydatne do zadań wymagających wieloetapowej logiki, matematyki lub rozwiązywania problemów.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning zajmuje około 4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Model otacza swoje myślenie tagami <think>...</think>
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

// Parsuj rozumowanie łańcuchowe
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Kiedy używać modeli rozumujących:**
> - Problemy matematyczne i logiczne
> - Zadania planowania wieloetapowego
> - Generowanie złożonego kodu
> - Zadania, gdzie pokazanie toku rozumowania zwiększa dokładność
>
> **Wada:** Modele rozumujące generują więcej tokenów (sekcja `<think>`) i są wolniejsze. Do prostych pytań i odpowiedzi szybciej sprawdzi się standardowy model jak phi-3.5-mini.

---

### Ćwiczenie 11: Zrozumienie aliasów i wyboru sprzętu

Przekazując **alias** (np. `phi-3.5-mini`) zamiast pełnego ID modelu, SDK automatycznie wybiera najlepszy wariant dla twojego sprzętu:

| Sprzęt | Wybrany dostawca wykonawczy |
|---------|-----------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (poprzez wtyczkę WinML) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Dowolne urządzenie (fallback) | `CPUExecutionProvider` lub `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Alias rozwiązuje się do najlepszej warianty dla TWOJEGO sprzętu
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Wskazówka:** Zawsze używaj aliasów w swoim kodzie aplikacji. Gdy wdrażasz na maszynie użytkownika, SDK wybierze optymalny wariant w czasie działania — CUDA na NVIDIA, QNN na Qualcomm, CPU gdzie indziej.

---

### Ćwiczenie 12: Opcje konfiguracji C#

Klasa `Configuration` w SDK C# oferuje szczegółową kontrolę nad środowiskiem wykonawczym:

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

| Właściwość | Domyślna | Opis |
|------------|----------|------|
| `AppName` | (wymagane) | Nazwa twojej aplikacji |
| `LogLevel` | `Information` | Szczegółowość logowania |
| `Web.Urls` | (dynamiczny) | Przypiniecie konkretnego portu dla serwera web |
| `AppDataDir` | Domyślne OS | Podstawowy katalog danych aplikacji |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Gdzie przechowywane są modele |
| `LogsDir` | `{AppDataDir}/logs` | Gdzie zapisywane są logi |

---

### Ćwiczenie 13: Użycie w przeglądarce (tylko JavaScript)

SDK JavaScript zawiera wersję kompatybilną z przeglądarką. W przeglądarce musisz ręcznie uruchomić usługę przez CLI i podać adres URL hosta:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Najpierw ręcznie uruchom usługę:
//   foundry service start
// Następnie użyj URL z wyjścia CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Przeglądaj katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Ograniczenia przeglądarki:** Wersja przeglądarkowa nie obsługuje `startWebService()`. Musisz upewnić się, że usługa Foundry Local jest już uruchomiona przed użyciem SDK w przeglądarce.

---

## Pełna Referencja API

### Python

| Kategoria | Metoda | Opis |
|----------|--------|-------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Utwórz managera; opcjonalnie bootstrapuj z modelem |
| **Service** | `is_service_running()` | Sprawdź, czy usługa działa |
| **Service** | `start_service()` | Uruchom usługę |
| **Service** | `endpoint` | URL punktu końcowego API |
| **Service** | `api_key` | Klucz API |
| **Catalog** | `list_catalog_models()` | Wypisz wszystkie dostępne modele |
| **Catalog** | `refresh_catalog()` | Odśwież katalog |
| **Catalog** | `get_model_info(alias_or_model_id)` | Pobierz metadane modelu |
| **Cache** | `get_cache_location()` | Ścieżka katalogu podręcznego |
| **Cache** | `list_cached_models()` | Wypisz pobrane modele |
| **Model** | `download_model(alias_or_model_id)` | Pobierz model |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Załaduj model |
| **Model** | `unload_model(alias_or_model_id)` | Zwolnij model |
| **Model** | `list_loaded_models()` | Wypisz załadowane modele |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Sprawdź, czy dostępna jest nowsza wersja |
| **Model** | `upgrade_model(alias_or_model_id)` | Zaktualizuj model do najnowszej wersji |
| **Service** | `httpx_client` | Wstępnie skonfigurowany klient HTTPX do bezpośrednich wywołań API |

### JavaScript

| Kategoria | Metoda | Opis |
|----------|--------|------|
| **Init** | `FoundryLocalManager.create(options)` | Inicjalizuj singleton SDK |
| **Init** | `FoundryLocalManager.instance` | Dostęp do singletona managera |
| **Service** | `manager.startWebService()` | Uruchom usługę webową |
| **Service** | `manager.urls` | Tablica bazowych URL usług |
| **Catalog** | `manager.catalog` | Dostęp do katalogu modeli |
| **Catalog** | `catalog.getModel(alias)` | Pobierz obiekt modelu po aliasie (zwraca Promise) |
| **Model** | `model.isCached` | Czy model jest pobrany |
| **Model** | `model.download()` | Pobierz model |
| **Model** | `model.load()` | Załaduj model |
| **Model** | `model.unload()` | Zwolnij model |
| **Model** | `model.id` | Unikalny identyfikator modelu |
| **Model** | `model.alias` | Alias modelu |
| **Model** | `model.createChatClient()` | Pobierz natywnego klienta czatu (nie wymaga OpenAI SDK) |
| **Model** | `model.createAudioClient()` | Pobierz klienta do transkrypcji audio |
| **Model** | `model.removeFromCache()` | Usuń model z lokalnego cache’u |
| **Model** | `model.selectVariant(variant)` | Wybierz konkretny wariant sprzętowy |
| **Model** | `model.variants` | Tablica dostępnych wariantów modelu |
| **Model** | `model.isLoaded()` | Sprawdź, czy model jest aktualnie załadowany |
| **Catalog** | `catalog.getModels()` | Wypisz wszystkie dostępne modele |
| **Catalog** | `catalog.getCachedModels()` | Wypisz pobrane modele |
| **Catalog** | `catalog.getLoadedModels()` | Wypisz aktualnie załadowane modele |
| **Catalog** | `catalog.updateModels()` | Odśwież katalog z usługi |
| **Service** | `manager.stopWebService()` | Zatrzymaj usługę sieciową Foundry Local |

### C# (v0.8.0+)

| Kategoria | Metoda | Opis |
|----------|--------|------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Inicjalizuj managera |
| **Init** | `FoundryLocalManager.Instance` | Dostęp do singletona |
| **Catalog** | `manager.GetCatalogAsync()` | Pobierz katalog |
| **Catalog** | `catalog.ListModelsAsync()` | Wypisz wszystkie modele |
| **Catalog** | `catalog.GetModelAsync(alias)` | Pobierz konkretny model |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Wypisz pobrane modele |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Wypisz załadowane modele |
| **Model** | `model.DownloadAsync(progress?)` | Pobierz model |
| **Model** | `model.LoadAsync()` | Załaduj model |
| **Model** | `model.UnloadAsync()` | Zwolnij model |
| **Model** | `model.SelectVariant(variant)` | Wybierz wariant sprzętowy |
| **Model** | `model.GetChatClientAsync()` | Pobierz natywnego klienta czatu |
| **Model** | `model.GetAudioClientAsync()` | Pobierz klienta transkrypcji audio |
| **Model** | `model.GetPathAsync()` | Pobierz lokalną ścieżkę pliku |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Pobierz konkretny wariant sprzętowy |
| **Catalog** | `catalog.UpdateModelsAsync()` | Odśwież katalog |
| **Server** | `manager.StartWebServerAsync()` | Uruchom RESTowy serwer webowy |
| **Server** | `manager.StopWebServerAsync()` | Zatrzymaj RESTowy serwer webowy |
| **Config** | `config.ModelCacheDir` | Katalog cache’u |

---

## Kluczowe wnioski

| Pojęcie | Czego się nauczyłeś |
|---------|---------------------|
| **SDK vs CLI** | SDK zapewnia kontrolę programistyczną — niezbędną w aplikacjach |
| **Płaszczyzna kontroli** | SDK zarządza usługami, modelami i cache’em |
| **Porty dynamiczne** | Zawsze używaj `manager.endpoint` (Python) albo `manager.urls[0]` (JS/C#) — nigdy nie koduj portu na sztywno |
| **Aliasy** | Używaj aliasów, aby automatycznie wybierać optymalny sprzętowo wariant modelu |
| **Szybki start** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Przebudowa C#** | Wersja 0.8.0+ jest samodzielna - nie wymaga CLI na maszynach użytkowników końcowych |
| **Cykl życia modelu** | Katalog → Pobierz → Załaduj → Użyj → Zwolnij |
| **FoundryModelInfo** | Obszerne metadane: zadanie, urządzenie, rozmiar, licencja, wsparcie wywoływania narzędzi |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) do użycia bez OpenAI |
| **Warianty** | Modele mają specyficzne dla sprzętu warianty (CPU, GPU, NPU); wybierane automatycznie |
| **Aktualizacje** | Python: `is_model_upgradeable()` + `upgrade_model()` do utrzymania modeli na bieżąco |
| **Odświeżanie katalogu** | `refresh_catalog()` (Python) / `updateModels()` (JS) do wykrywania nowych modeli |

---

## Zasoby

| Zasób | Link |
|----------|------|
| Referencje SDK (wszystkie języki) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integracja z SDK inferencyjnym | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Referencje API C# SDK | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| Przykłady C# SDK | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Strona Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Kolejne kroki

Kontynuuj w [Część 3: Używanie SDK z OpenAI](part3-sdk-and-apis.md), aby połączyć SDK z biblioteką klienta OpenAI i zbudować swoją pierwszą aplikację do uzupełniania czatu.