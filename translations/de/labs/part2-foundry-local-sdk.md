![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Teil 2: Foundry Local SDK Deep Dive

> **Ziel:** Beherrsche das Foundry Local SDK, um Modelle, Dienste und Caching programmatisch zu verwalten – und verstehe, warum das SDK der empfohlene Ansatz gegenüber der CLI zum Erstellen von Anwendungen ist.

## Überblick

In Teil 1 hast du die **Foundry Local CLI** verwendet, um Modelle interaktiv herunterzuladen und auszuführen. Die CLI ist großartig zum Erkunden, aber wenn du echte Anwendungen entwickelst, benötigst du **programmatische Kontrolle**. Das Foundry Local SDK bietet dir genau das – es verwaltet die **Kontrollebene** (Starten des Dienstes, Entdecken von Modellen, Herunterladen, Laden), sodass dein Anwendungscode sich auf die **Datenebene** konzentrieren kann (Senden von Eingaben, Empfangen von Ausgaben).

Dieses Lab zeigt dir die gesamte SDK-API-Oberfläche in Python, JavaScript und C#. Am Ende wirst du jede verfügbare Methode verstehen und wissen, wann du welche anwenden solltest.

## Lernziele

Am Ende dieses Labs wirst du in der Lage sein:

- Erklären, warum das SDK der CLI für die Anwendungsentwicklung vorgezogen wird
- Das Foundry Local SDK für Python, JavaScript oder C# zu installieren
- `FoundryLocalManager` zu verwenden, um den Dienst zu starten, Modelle zu verwalten und den Katalog abzufragen
- Modelle programmatisch aufzulisten, herunterzuladen, zu laden und zu entladen
- Modellmetadaten mit `FoundryModelInfo` zu inspizieren
- Den Unterschied zwischen Katalog-, Cache- und geladenen Modellen zu verstehen
- Den Konstruktor-Bootstrap (Python) sowie das `create()` + Katalog-Muster (JavaScript) zu verwenden
- Das Redesign des C# SDK und seine objektorientierte API zu verstehen

---

## Voraussetzungen

| Voraussetzung | Details |
|---------------|---------|
| **Foundry Local CLI** | Installiert und im `PATH` ([Teil 1](part1-getting-started.md)) |
| **Programmiersprache** | **Python 3.9+** und/oder **Node.js 18+** und/oder **.NET 9.0+** |

---

## Konzept: SDK vs CLI – Warum das SDK verwenden?

| Aspekt | CLI (`foundry` Befehl) | SDK (`foundry-local-sdk`) |
|--------|------------------------|----------------------------|
| **Anwendungsfall** | Erkundung, manuelles Testen | Anwendungsintegration |
| **Dienstverwaltung** | Manuell: `foundry service start` | Automatisch: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Port-Erkennung** | Aus CLI-Ausgabe lesen | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Modell-Download** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Fehlerbehandlung** | Exit-Codes, stderr | Ausnahmen, typisierte Fehler |
| **Automatisierung** | Shell-Skripte | Native Sprachintegration |
| **Bereitstellung** | CLI auf Endnutzer-Maschine erforderlich | C# SDK kann eigenständig sein (kein CLI notwendig) |

> **Schlüssel-Insight:** Das SDK verwaltet den kompletten Lebenszyklus: Starten des Dienstes, Cache prüfen, fehlende Modelle herunterladen, laden und den Endpunkt entdecken – mit nur wenigen Codezeilen. Deine Anwendung muss die CLI-Ausgabe nicht parsen oder Subprozesse verwalten.

---

## Laborübungen

### Übung 1: SDK installieren

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Installationsüberprüfung:

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

Installationsüberprüfung:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Es gibt zwei NuGet-Pakete:

| Paket | Plattform | Beschreibung |
|-------|-----------|--------------|
| `Microsoft.AI.Foundry.Local` | Plattformübergreifend | Funktioniert auf Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Nur Windows | Fügt WinML Hardwarebeschleunigung hinzu; installiert Plugin-Ausführungsanbieter (z.B. QNN für Qualcomm NPU) |

**Windows-Setup:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Bearbeite die `.csproj`-Datei:

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

> **Hinweis:** Unter Windows ist das WinML-Paket ein Superset, das das Basissdk plus QNN-Ausführungsanbieter enthält. Unter Linux/macOS wird das Basissdk verwendet. Die bedingten TFM- und Paketverweise halten das Projekt vollständig plattformübergreifend.

Erstelle eine `nuget.config` im Projektstamm:

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

Pakete wiederherstellen:

```bash
dotnet restore
```

</details>

---

### Übung 2: Dienst starten und Katalog auflisten

Das Erste, was jede Anwendung tut, ist den Foundry Local Dienst zu starten und zu erkunden, welche Modelle verfügbar sind.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Erstellen Sie einen Manager und starten Sie den Dienst
manager = FoundryLocalManager()
manager.start_service()

# Listen Sie alle im Katalog verfügbaren Modelle auf
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### Python SDK – Methoden zur Dienstverwaltung

| Methode | Signatur | Beschreibung |
|---------|----------|--------------|
| `is_service_running()` | `() -> bool` | Prüfen, ob der Dienst läuft |
| `start_service()` | `() -> None` | Den Foundry Local Dienst starten |
| `service_uri` | `@property -> str` | Basis-URI des Dienstes |
| `endpoint` | `@property -> str` | API-Endpunkt (Dienst-URI + `/v1`) |
| `api_key` | `@property -> str` | API-Schlüssel (aus Umgebungsvariablen oder Platzhalter) |

#### Python SDK – Methoden zur Katalogverwaltung

| Methode | Signatur | Beschreibung |
|---------|----------|--------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Alle Modelle im Katalog auflisten |
| `refresh_catalog()` | `() -> None` | Katalog vom Dienst aktualisieren |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Infos zu einem bestimmten Modell abrufen |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Erstellen Sie einen Manager und starten Sie den Dienst
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Durchsuchen Sie den Katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### JavaScript SDK – Methoden des Managers

| Methode | Signatur | Beschreibung |
|---------|----------|--------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | SDK-Singleton initialisieren |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Zugriff auf das Singleton-Manager-Objekt |
| `manager.startWebService()` | `() => Promise<void>` | Foundry Local Webdienst starten |
| `manager.urls` | `string[]` | Array von Basis-URLs für den Dienst |

#### JavaScript SDK – Katalog- und Modellmethoden

| Methode | Signatur | Beschreibung |
|---------|----------|--------------|
| `manager.catalog` | `Catalog` | Zugriff auf den Modellkatalog |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Modellobjekt per Alias abrufen |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Das C# SDK v0.8.0+ verwendet eine objektorientierte Architektur mit `Configuration`, `Catalog` und `Model` Objekten:

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

#### C# SDK – Wichtige Klassen

| Klasse | Zweck |
|--------|-------|
| `Configuration` | App-Name, Log-Level, Cache-Verzeichnis, Webserver-URLs setzen |
| `FoundryLocalManager` | Haupteinstiegspunkt – erzeugt via `CreateAsync()`, zugreifbar über `.Instance` |
| `Catalog` | Modelle durchsuchen, suchen und abrufen |
| `Model` | Repräsentiert ein spezifisches Modell – herunterladen, laden, Clients abrufen |

#### C# SDK – Methoden des Managers und Katalogs

| Methode | Beschreibung |
|---------|--------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Manager initialisieren |
| `FoundryLocalManager.Instance` | Zugriff auf das Singleton-Managerobjekt |
| `manager.GetCatalogAsync()` | Modellkatalog abrufen |
| `catalog.ListModelsAsync()` | Alle verfügbaren Modelle auflisten |
| `catalog.GetModelAsync(alias: "alias")` | Ein spezifisches Modell per Alias abrufen |
| `catalog.GetCachedModelsAsync()` | Heruntergeladene Modelle auflisten |
| `catalog.GetLoadedModelsAsync()` | Derzeit geladene Modelle auflisten |

> **C# Architektur-Hinweis:** Das Redesign des C# SDK v0.8.0+ macht die Anwendung **eigenständig**; sie benötigt die Foundry Local CLI auf der Endnutzer-Maschine nicht. Das SDK verwaltet Modellverwaltung und Inferenz nativ.

</details>

---

### Übung 3: Modell herunterladen und laden

Das SDK trennt Herunterladen (auf Festplatte) und Laden (in den Speicher). So kannst du Modelle bei der Einrichtung vorab herunterladen und bei Bedarf laden.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Option A: Manueller Schritt-für-Schritt
manager = FoundryLocalManager()
manager.start_service()

# Zuerst Cache überprüfen
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

# Option B: Einzeiler-bootstrap (empfohlen)
# Alias an den Konstruktor übergeben - es startet den Dienst, lädt herunter und lädt automatisch
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python – Methoden zur Modellverwaltung

| Methode | Signatur | Beschreibung |
|---------|----------|--------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Ein Modell in den lokalen Cache herunterladen |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Ein Modell in den Inferenzserver laden |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Ein Modell vom Server entladen |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Alle aktuell geladenen Modelle auflisten |

#### Python – Methoden zur Cache-Verwaltung

| Methode | Signatur | Beschreibung |
|---------|----------|--------------|
| `get_cache_location()` | `() -> str` | Cache-Verzeichnis-Pfad abrufen |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Alle heruntergeladenen Modelle auflisten |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Schritt-für-Schritt-Ansatz
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

#### JavaScript – Modellmethoden

| Methode | Signatur | Beschreibung |
|---------|----------|--------------|
| `model.isCached` | `boolean` | Ob das Modell bereits heruntergeladen ist |
| `model.download()` | `() => Promise<void>` | Modell in den lokalen Cache herunterladen |
| `model.load()` | `() => Promise<void>` | Ins Inferenz-Backend laden |
| `model.unload()` | `() => Promise<void>` | Aus dem Inferenz-Backend entladen |
| `model.id` | `string` | Die eindeutige Modell-ID |

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

#### C# – Modellmethoden

| Methode | Beschreibung |
|---------|--------------|
| `model.DownloadAsync(progress?)` | Ausgewählte Variante herunterladen |
| `model.LoadAsync()` | Modell in den Speicher laden |
| `model.UnloadAsync()` | Modell entladen |
| `model.SelectVariant(variant)` | Eine bestimmte Variante auswählen (CPU/GPU/NPU) |
| `model.SelectedVariant` | Aktuell ausgewählte Variante |
| `model.Variants` | Alle verfügbaren Varianten für dieses Modell |
| `model.GetPathAsync()` | Lokalen Dateipfad abrufen |
| `model.GetChatClientAsync()` | Nativen Chat-Client abrufen (kein OpenAI-SDK notwendig) |
| `model.GetAudioClientAsync()` | Audioclient für Transkription abrufen |

</details>

---

### Übung 4: Modellmetadaten inspizieren

Das `FoundryModelInfo`-Objekt enthält umfangreiche Metadaten zu jedem Modell. Das Verständnis dieser Felder hilft dir, das passende Modell für deine Anwendung auszuwählen.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Holen Sie sich detaillierte Informationen über ein bestimmtes Modell
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

#### FoundryModelInfo Felder

| Feld | Typ | Beschreibung |
|-------|-----|--------------|
| `alias` | string | Kurzname (z.B. `phi-3.5-mini`) |
| `id` | string | Eindeutige Modell-ID |
| `version` | string | Modellversion |
| `task` | string | `chat-completions` oder `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU oder NPU |
| `execution_provider` | string | Laufzeit-Backend (CUDA, CPU, QNN, WebGPU usw.) |
| `file_size_mb` | int | Größe auf der Festplatte in MB |
| `supports_tool_calling` | bool | Ob das Modell Funktions-/Werkzeugaufrufe unterstützt |
| `publisher` | string | Wer das Modell veröffentlicht hat |
| `license` | string | Lizenzname |
| `uri` | string | Modell-URI |
| `prompt_template` | dict/null | Prompt-Vorlage, falls vorhanden |

---

### Übung 5: Modelllebenszyklus verwalten

Übe den kompletten Lebenszyklus: auflisten → herunterladen → laden → nutzen → entladen.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Kleines Modell für schnelle Tests

manager = FoundryLocalManager()
manager.start_service()

# 1. Überprüfen, was im Katalog ist
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Überprüfen, was bereits heruntergeladen wurde
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Ein Modell herunterladen
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Überprüfen, dass es jetzt im Cache ist
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Laden
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Überprüfen, was geladen ist
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Entladen
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

const alias = "qwen2.5-0.5b"; // Kleines Modell für schnelle Tests

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Modell aus dem Katalog holen
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Bei Bedarf herunterladen
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Laden
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Entladen
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Übung 6: Die Quick-Start-Muster

Jede Sprache bietet eine Abkürzung, um den Dienst zu starten und ein Modell in einem Aufruf zu laden. Dies sind die **empfohlenen Muster** für die meisten Anwendungen.

<details>
<summary><h3>🐍 Python - Konstruktor-Bootstrap</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Übergeben Sie dem Konstruktor einen Alias - er kümmert sich um alles:
# 1. Startet den Dienst, falls er nicht läuft
# 2. Lädt das Modell herunter, falls es nicht zwischengespeichert ist
# 3. Lädt das Modell in den Inferenzserver
manager = FoundryLocalManager("phi-3.5-mini")

# Sofort einsatzbereit
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Der Parameter `bootstrap` (Standard `True`) steuert dieses Verhalten. Setze `bootstrap=False`, wenn du manuelle Kontrolle möchtest:

```python
# Manueller Modus - es passiert nichts automatisch
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Katalog</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() erledigt alles:
// 1. Startet den Dienst
// 2. Holt das Modell aus dem Katalog
// 3. Lädt herunter, wenn nötig, und lädt das Modell
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Sofort einsatzbereit
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

> **C# Hinweis:** Das C# SDK verwendet `Configuration`, um den App-Namen, Logging, Cache-Verzeichnisse und sogar einen bestimmten Webserver-Port festzulegen. Dadurch ist es am anpassungsfähigsten von den drei SDKs.

</details>

---

### Übung 7: Der native ChatClient (Kein OpenAI SDK erforderlich)

Die JavaScript- und C#-SDKs bieten eine `createChatClient()`-Bequemlichkeitsmethode, die einen nativen Chat-Client zurückgibt – keine separate Installation oder Konfiguration des OpenAI SDK nötig.

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

// Erstellen Sie einen ChatClient direkt aus dem Modell — kein OpenAI-Import erforderlich
const chatClient = model.createChatClient();

// completeChat gibt ein OpenAI-kompatibles Antwortobjekt zurück
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Streaming verwendet ein Rückruffunktionsmuster
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

Der `ChatClient` unterstützt auch das Aufrufen von Werkzeugen – übergebe Werkzeuge als zweiten Parameter:

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

> **Wann welches Muster verwenden:**
> - **`createChatClient()`** — Schnelles Prototyping, weniger Abhängigkeiten, einfacherer Code
> - **OpenAI SDK** — Volle Kontrolle über Parameter (Temperatur, top_p, Stop Tokens, etc.), besser für Produktionsanwendungen

---

### Übung 8: Modellvarianten und Hardwareauswahl

Modelle können mehrere **Varianten** haben, die für unterschiedliche Hardware optimiert sind. Das SDK wählt automatisch die beste Variante aus, du kannst aber auch manuell prüfen und wählen.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Alle verfügbaren Varianten auflisten
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// Das SDK wählt automatisch die beste Variante für Ihre Hardware aus
// Um dies zu überschreiben, verwenden Sie selectVariant():
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

In Python wählt das SDK automatisch die beste Variante basierend auf der Hardware aus. Benutze `get_model_info()`, um zu sehen, was ausgewählt wurde:

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

#### Modelle mit NPU-Varianten

Einige Modelle haben NPU-optimierte Varianten für Geräte mit Neural Processing Units (Qualcomm Snapdragon, Intel Core Ultra):

| Modell | NPU-Variante verfügbar |
|--------|:----------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Tipp:** Bei NPU-fähiger Hardware wählt das SDK automatisch die NPU-Variante, wenn verfügbar. Du musst deinen Code nicht ändern. Für C#-Projekte unter Windows füge das `Microsoft.AI.Foundry.Local.WinML` NuGet-Paket hinzu, um den QNN-Ausführungsanbieter zu aktivieren — QNN wird als Plugin-EP über WinML bereitgestellt.

---

### Übung 9: Modell-Updates und Katalogaktualisierung

Der Modellkatalog wird regelmäßig aktualisiert. Nutze diese Methoden, um Updates zu prüfen und anzuwenden.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Aktualisieren Sie den Katalog, um die neueste Modellliste zu erhalten
manager.refresh_catalog()

# Überprüfen Sie, ob ein zwischengespeichertes Modell eine neuere Version verfügbar hat
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

// Aktualisieren Sie den Katalog, um die neueste Modellauswahl abzurufen
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Zeigen Sie alle verfügbaren Modelle nach der Aktualisierung an
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Übung 10: Arbeiten mit Reasoning-Modellen

Das **phi-4-mini-reasoning** Modell beinhaltet Chain-of-Thought Reasoning. Es umschließt sein internes Nachdenken mit `<think>...</think>` Tags, bevor es die finale Antwort liefert. Das ist nützlich für Aufgaben, die mehrstufige Logik, Mathematik oder Problemlösung erfordern.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning ist ~4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Das Modell verpackt sein Denken in <think>...</think>-Tags
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

// Kette-von-Gedanken-Denken analysieren
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Wann Reasoning-Modelle verwenden:**
> - Mathe- und Logikprobleme
> - Mehrstufige Planungsaufgaben
> - Komplexe Codegenerierung
> - Aufgaben, bei denen das Zeigen der Arbeitsweise die Genauigkeit erhöht
>
> **Trade-off:** Reasoning-Modelle erzeugen mehr Tokens (der `<think>`-Abschnitt) und sind langsamer. Für einfache Q&A ist ein Standardmodell wie phi-3.5-mini schneller.

---

### Übung 11: Alias-Verständnis und Hardware-Auswahl

Wenn du einen **Alias** (wie `phi-3.5-mini`) anstelle einer vollständigen Modell-ID übergibst, wählt das SDK automatisch die beste Variante passend zu deiner Hardware:

| Hardware | Ausgewählter Ausführungsanbieter |
|----------|----------------------------------|
| NVIDIA GPU (CUDA) | `CUDAExecutionProvider` |
| Qualcomm NPU | `QNNExecutionProvider` (über WinML-Plugin) |
| Intel NPU | `OpenVINOExecutionProvider` |
| AMD GPU | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Beliebiges Gerät (Fallback) | `CPUExecutionProvider` oder `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Der Alias wird auf die beste Variante für IHRE Hardware aufgelöst
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Tipp:** Verwende immer Aliase in deinem Anwendungscode. Beim Deployment auf dem Nutzergerät wählt das SDK zur Laufzeit die optimale Variante – CUDA auf NVIDIA, QNN auf Qualcomm, CPU sonst.

---

### Übung 12: C# Konfigurationsoptionen

Die `Configuration`-Klasse des C# SDKs bietet feinkörnige Steuerung zur Laufzeit:

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

| Eigenschaft | Standard | Beschreibung |
|-------------|----------|--------------|
| `AppName` | (erforderlich) | Name deiner Anwendung |
| `LogLevel` | `Information` | Logging-Detailgrad |
| `Web.Urls` | (dynamisch) | Einen bestimmten Port für den Webserver festlegen |
| `AppDataDir` | OS-Standard | Basisverzeichnis für Anwendungsdaten |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Speicherort der Modelle |
| `LogsDir` | `{AppDataDir}/logs` | Speicherort der Logs |

---

### Übung 13: Browser-Nutzung (nur JavaScript)

Das JavaScript SDK enthält eine browser-kompatible Version. Im Browser musst du den Dienst manuell per CLI starten und die Host-URL angeben:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Starten Sie den Dienst zuerst manuell:
//   foundry service start
// Verwenden Sie dann die URL aus der CLI-Ausgabe
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Durchsuchen Sie den Katalog
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Browser-Einschränkungen:** Die Browserversion unterstützt **nicht** `startWebService()`. Du musst sicherstellen, dass der Foundry Local-Dienst bereits läuft, bevor du das SDK im Browser benutzt.

---

## Komplette API-Referenz

### Python

| Kategorie | Methode | Beschreibung |
|-----------|---------|--------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Manager erstellen; optional mit Modell bootstrappen |
| **Service** | `is_service_running()` | Prüfen, ob Dienst läuft |
| **Service** | `start_service()` | Dienst starten |
| **Service** | `endpoint` | API-Endpunkt-URL |
| **Service** | `api_key` | API-Schlüssel |
| **Katalog** | `list_catalog_models()` | Alle verfügbaren Modelle auflisten |
| **Katalog** | `refresh_catalog()` | Katalog aktualisieren |
| **Katalog** | `get_model_info(alias_or_model_id)` | Metadaten des Modells abrufen |
| **Cache** | `get_cache_location()` | Verzeichnis des Cache |
| **Cache** | `list_cached_models()` | Heruntergeladene Modelle auflisten |
| **Modell** | `download_model(alias_or_model_id)` | Ein Modell herunterladen |
| **Modell** | `load_model(alias_or_model_id, ttl=600)` | Ein Modell laden |
| **Modell** | `unload_model(alias_or_model_id)` | Ein Modell entladen |
| **Modell** | `list_loaded_models()` | Geladene Modelle auflisten |
| **Modell** | `is_model_upgradeable(alias_or_model_id)` | Prüfen, ob ein neueres Update verfügbar ist |
| **Modell** | `upgrade_model(alias_or_model_id)` | Modell auf neueste Version aktualisieren |
| **Service** | `httpx_client` | Vorgefertigter HTTPX-Client für direkte API-Aufrufe |

### JavaScript

| Kategorie | Methode | Beschreibung |
|-----------|---------|--------------|
| **Init** | `FoundryLocalManager.create(options)` | Ein SDK-Singleton initialisieren |
| **Init** | `FoundryLocalManager.instance` | Zugriff auf Singleton-Manager |
| **Service** | `manager.startWebService()` | Webdienst starten |
| **Service** | `manager.urls` | Array mit Basis-URLs des Dienstes |
| **Katalog** | `manager.catalog` | Zugriff auf Modellkatalog |
| **Katalog** | `catalog.getModel(alias)` | Modellobjekt per Alias abrufen (gibt Promise zurück) |
| **Modell** | `model.isCached` | Ob das Modell heruntergeladen ist |
| **Modell** | `model.download()` | Modell herunterladen |
| **Modell** | `model.load()` | Modell laden |
| **Modell** | `model.unload()` | Modell entladen |
| **Modell** | `model.id` | Einzigartige Modell-ID |
| **Modell** | `model.alias` | Alias des Modells |
| **Modell** | `model.createChatClient()` | Nativen Chat-Client holen (kein OpenAI SDK nötig) |
| **Modell** | `model.createAudioClient()` | Audio-Client für Transkription holen |
| **Modell** | `model.removeFromCache()` | Modell aus lokalem Cache entfernen |
| **Modell** | `model.selectVariant(variant)` | Spezifische Hardwarevariante auswählen |
| **Modell** | `model.variants` | Array verfügbarer Varianten für dieses Modell |
| **Modell** | `model.isLoaded()` | Prüfen, ob Modell aktuell geladen ist |
| **Katalog** | `catalog.getModels()` | Alle verfügbaren Modelle auflisten |
| **Katalog** | `catalog.getCachedModels()` | Heruntergeladene Modelle auflisten |
| **Katalog** | `catalog.getLoadedModels()` | Aktuell geladene Modelle auflisten |
| **Katalog** | `catalog.updateModels()` | Katalog vom Dienst aktualisieren |
| **Service** | `manager.stopWebService()` | Foundry Local Webservice stoppen |

### C# (v0.8.0+)

| Kategorie | Methode | Beschreibung |
|-----------|---------|--------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Manager initialisieren |
| **Init** | `FoundryLocalManager.Instance` | Zugriff auf Singleton |
| **Katalog** | `manager.GetCatalogAsync()` | Katalog abrufen |
| **Katalog** | `catalog.ListModelsAsync()` | Alle Modelle auflisten |
| **Katalog** | `catalog.GetModelAsync(alias)` | Ein bestimmtes Modell abrufen |
| **Katalog** | `catalog.GetCachedModelsAsync()` | Heruntergeladene Modelle auflisten |
| **Katalog** | `catalog.GetLoadedModelsAsync()` | Geladene Modelle auflisten |
| **Modell** | `model.DownloadAsync(progress?)` | Modell herunterladen |
| **Modell** | `model.LoadAsync()` | Modell laden |
| **Modell** | `model.UnloadAsync()` | Modell entladen |
| **Modell** | `model.SelectVariant(variant)` | Hardwarevariante wählen |
| **Modell** | `model.GetChatClientAsync()` | Nativen Chat-Client holen |
| **Modell** | `model.GetAudioClientAsync()` | Audio-Transkriptionsclient holen |
| **Modell** | `model.GetPathAsync()` | Lokalen Dateipfad holen |
| **Katalog** | `catalog.GetModelVariantAsync(alias, variant)` | Spezifische Hardwarevariante abrufen |
| **Katalog** | `catalog.UpdateModelsAsync()` | Katalog aktualisieren |
| **Server** | `manager.StartWebServerAsync()` | REST-Webserver starten |
| **Server** | `manager.StopWebServerAsync()` | REST-Webserver stoppen |
| **Config** | `config.ModelCacheDir` | Cache-Verzeichnis |

---

## Wichtigste Erkenntnisse

| Konzept | Was du gelernt hast |
|---------|--------------------|
| **SDK vs CLI** | Das SDK bietet programmatische Steuerung – essenziell für Anwendungen |
| **Control plane** | Das SDK verwaltet Services, Modelle und Caches |
| **Dynamische Ports** | Nutze immer `manager.endpoint` (Python) oder `manager.urls[0]` (JS/C#) – niemals Port hart codieren |
| **Aliase** | Verwende Aliase für automatische hardwareoptimale Modellauswahl |
| **Schnellstart** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **C# Neugestaltung** | v0.8.0+ ist eigenständig - keine CLI auf Endbenutzergeräten erforderlich |
| **Modelllebenszyklus** | Katalog → Herunterladen → Laden → Verwenden → Entladen |
| **FoundryModelInfo** | Umfangreiche Metadaten: Aufgabe, Gerät, Größe, Lizenz, Unterstützung für aufrufende Tools |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) für OpenAI-freie Nutzung |
| **Varianten** | Modelle haben hardware-spezifische Varianten (CPU, GPU, NPU); automatisch ausgewählt |
| **Upgrades** | Python: `is_model_upgradeable()` + `upgrade_model()` um Modelle aktuell zu halten |
| **Katalog aktualisieren** | `refresh_catalog()` (Python) / `updateModels()` (JS) um neue Modelle zu entdecken |

---

## Ressourcen

| Ressource | Link |
|----------|------|
| SDK-Referenz (alle Sprachen) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integration mit Inference-SDKs | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| C# SDK API-Referenz | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| C# SDK Beispiele | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Foundry Local Website | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Nächste Schritte

Fahren Sie fort mit [Teil 3: Verwendung des SDK mit OpenAI](part3-sdk-and-apis.md), um das SDK mit der OpenAI-Clientbibliothek zu verbinden und Ihre erste Chat Completion-Anwendung zu erstellen.