![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partie 2 : Approfondissement du SDK Foundry Local

> **Objectif :** Maîtriser le SDK Foundry Local pour gérer les modèles, services, et le cache de manière programmatique – et comprendre pourquoi le SDK est l’approche recommandée plutôt que le CLI pour construire des applications.

## Vue d’ensemble

Dans la Partie 1, vous avez utilisé le **Foundry Local CLI** pour télécharger et exécuter des modèles de manière interactive. Le CLI est idéal pour l’exploration, mais pour construire de vraies applications vous avez besoin d’un **contrôle programmatique**. Le SDK Foundry Local vous offre cela – il gère le **plan de contrôle** (démarrage du service, découverte des modèles, téléchargement, chargement) afin que votre code applicatif puisse se concentrer sur le **plan de données** (envoi de prompts, réception de complétions).

Ce laboratoire vous enseigne l’ensemble de l’API SDK en Python, JavaScript, et C#. À la fin, vous comprendrez chaque méthode disponible et quand utiliser chacune.

## Objectifs d’apprentissage

À la fin de ce laboratoire, vous serez capable de :

- Expliquer pourquoi le SDK est préféré au CLI pour le développement d’applications
- Installer le SDK Foundry Local pour Python, JavaScript, ou C#
- Utiliser `FoundryLocalManager` pour démarrer le service, gérer les modèles, et interroger le catalogue
- Lister, télécharger, charger, et décharger des modèles de façon programmatique
- Inspecter les métadonnées des modèles via `FoundryModelInfo`
- Comprendre la différence entre catalogue, cache, et modèles chargés
- Utiliser le bootstrap constructeur (Python) et le pattern `create()` + catalogue (JavaScript)
- Comprendre la refonte du SDK C# et son API orientée objet

---

## Prérequis

| Exigence | Détails |
|----------|---------|
| **Foundry Local CLI** | Installé et dans votre `PATH` ([Partie 1](part1-getting-started.md)) |
| **Runtime langage** | **Python 3.9+** et/ou **Node.js 18+** et/ou **.NET 9.0+** |

---

## Concept : SDK vs CLI – Pourquoi utiliser le SDK ?

| Aspect | CLI (commande `foundry`) | SDK (`foundry-local-sdk`) |
|--------|--------------------------|---------------------------|
| **Cas d’utilisation** | Exploration, tests manuels | Intégration d’applications |
| **Gestion du service** | Manuelle : `foundry service start` | Automatique : `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Découverte du port** | Lecture de la sortie CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Téléchargement du modèle** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Gestion des erreurs** | Codes de sortie, stderr | Exceptions, erreurs typées |
| **Automatisation** | Scripts shell | Intégration native au langage |
| **Déploiement** | Nécessite le CLI sur la machine utilisateur | Le SDK C# peut être autonome (pas besoin de CLI) |

> **Insight clé :** Le SDK gère tout le cycle de vie : démarrage du service, vérification du cache, téléchargement des modèles manquants, chargement, et découverte du point d’accès, en quelques lignes de code. Votre application n’a pas besoin de parser la sortie CLI ni de gérer des sous-processus.

---

## Exercices du laboratoire

### Exercice 1 : Installer le SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Vérifiez l’installation :

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

Vérifiez l’installation :

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Il y a deux packages NuGet :

| Package | Plateforme | Description |
|---------|------------|-------------|
| `Microsoft.AI.Foundry.Local` | Multi-plateforme | Fonctionne sur Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Windows uniquement | Ajoute l’accélération matérielle WinML ; télécharge et installe des fournisseurs d’exécution plugins (ex. QNN pour Qualcomm NPU) |

**Configuration Windows :**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Modifiez le fichier `.csproj` :

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

> **Note :** Sur Windows, le package WinML est un sur-ensemble incluant le SDK de base plus le fournisseur d’exécution QNN. Sur Linux/macOS, c’est le SDK de base qui est utilisé. Le TFM conditionnel et les références de package gardent le projet entièrement multi-plateforme.

Créez un `nuget.config` à la racine du projet :

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

Restaurez les packages :

```bash
dotnet restore
```

</details>

---

### Exercice 2 : Démarrer le Service et Lister le Catalogue

La première étape d’une application est de démarrer le service Foundry Local et découvrir quels modèles sont disponibles.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Créez un gestionnaire et démarrez le service
manager = FoundryLocalManager()
manager.start_service()

# Listez tous les modèles disponibles dans le catalogue
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### SDK Python – Méthodes de gestion du service

| Méthode | Signature | Description |
|---------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Vérifie si le service est en cours d’exécution |
| `start_service()` | `() -> None` | Démarre le service Foundry Local |
| `service_uri` | `@property -> str` | URI base du service |
| `endpoint` | `@property -> str` | Point d’accès API (URI service + `/v1`) |
| `api_key` | `@property -> str` | Clé API (depuis l’environnement ou valeur par défaut) |

#### SDK Python – Méthodes de gestion du catalogue

| Méthode | Signature | Description |
|---------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Liste tous les modèles du catalogue |
| `refresh_catalog()` | `() -> None` | Rafraîchit le catalogue via le service |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Obtient les infos pour un modèle spécifique |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Créez un gestionnaire et démarrez le service
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Parcourez le catalogue
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### SDK JavaScript – Méthodes du manager

| Méthode | Signature | Description |
|---------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Initialise le singleton SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Accède au manager singleton |
| `manager.startWebService()` | `() => Promise<void>` | Démarre le service web Foundry Local |
| `manager.urls` | `string[]` | Tableau des URLs de base pour le service |

#### SDK JavaScript – Méthodes catalogue et modèle

| Méthode | Signature | Description |
|---------|-----------|-------------|
| `manager.catalog` | `Catalog` | Accès au catalogue de modèles |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Obtient un objet modèle par alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Le SDK C# v0.8.0+ utilise une architecture orientée objet avec les objets `Configuration`, `Catalog`, et `Model` :

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

#### SDK C# – Classes clés

| Classe | But |
|--------|-----|
| `Configuration` | Configure nom d’app, niveau de log, dossier cache, URLs serveur web |
| `FoundryLocalManager` | Point d’entrée principal – créé via `CreateAsync()`, accessible via `.Instance` |
| `Catalog` | Navigue, recherche, et obtient des modèles depuis le catalogue |
| `Model` | Représente un modèle spécifique – téléchargement, chargement, accès client |

#### SDK C# – Méthodes du manager et catalogue

| Méthode | Description |
|---------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Initialise le manager |
| `FoundryLocalManager.Instance` | Accède au manager singleton |
| `manager.GetCatalogAsync()` | Obtient le catalogue des modèles |
| `catalog.ListModelsAsync()` | Liste tous les modèles disponibles |
| `catalog.GetModelAsync(alias: "alias")` | Obtient un modèle spécifique par alias |
| `catalog.GetCachedModelsAsync()` | Liste les modèles téléchargés |
| `catalog.GetLoadedModelsAsync()` | Liste les modèles chargés actuellement |

> **Note architecture C# :** La refonte v0.8.0+ rend l’application **autonome** ; elle ne requiert pas le Foundry Local CLI sur la machine utilisateur. Le SDK gère nativement la gestion de modèle et l’inférence.

</details>

---

### Exercice 3 : Télécharger et Charger un Modèle

Le SDK sépare le téléchargement (vers disque) du chargement (en mémoire). Cela permet de pré-télécharger les modèles lors de la configuration et de les charger à la demande.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Option A : Étape manuelle par étape
manager = FoundryLocalManager()
manager.start_service()

# Vérifier d'abord le cache
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

# Option B : Bootstrap en une seule ligne (recommandé)
# Passez l'alias au constructeur - il démarre le service, télécharge et charge automatiquement
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python – Méthodes de gestion de modèles

| Méthode | Signature | Description |
|---------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Télécharge un modèle dans le cache local |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Charge un modèle dans le serveur d’inférence |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Décharge un modèle du serveur |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Liste tous les modèles chargés actuellement |

#### Python – Méthodes de gestion du cache

| Méthode | Signature | Description |
|---------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Obtient le chemin du dossier cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Liste les modèles téléchargés |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Approche étape par étape
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

#### JavaScript – Méthodes modèle

| Méthode | Signature | Description |
|---------|-----------|-------------|
| `model.isCached` | `boolean` | Indique si le modèle est déjà téléchargé |
| `model.download()` | `() => Promise<void>` | Télécharge le modèle dans le cache local |
| `model.load()` | `() => Promise<void>` | Charge dans le serveur d’inférence |
| `model.unload()` | `() => Promise<void>` | Décharge du serveur d’inférence |
| `model.id` | `string` | Identifiant unique du modèle |

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

#### C# – Méthodes modèle

| Méthode | Description |
|---------|-------------|
| `model.DownloadAsync(progress?)` | Télécharge la variante sélectionnée |
| `model.LoadAsync()` | Charge le modèle en mémoire |
| `model.UnloadAsync()` | Décharge le modèle |
| `model.SelectVariant(variant)` | Sélectionne une variante spécifique (CPU/GPU/NPU) |
| `model.SelectedVariant` | Variante actuellement sélectionnée |
| `model.Variants` | Toutes les variantes disponibles pour ce modèle |
| `model.GetPathAsync()` | Obtient le chemin local du fichier |
| `model.GetChatClientAsync()` | Obtient un client chat natif (pas besoin du SDK OpenAI) |
| `model.GetAudioClientAsync()` | Obtient un client audio pour la transcription |

</details>

---

### Exercice 4 : Inspecter les Métadonnées du Modèle

L’objet `FoundryModelInfo` contient des métadonnées riches pour chaque modèle. Comprendre ces champs vous aide à choisir le modèle adapté à votre application.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Obtenir des informations détaillées sur un modèle spécifique
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

#### Champs FoundryModelInfo

| Champ | Type | Description |
|-------|------|-------------|
| `alias` | string | Nom court (ex. `phi-3.5-mini`) |
| `id` | string | Identifiant unique du modèle |
| `version` | string | Version du modèle |
| `task` | string | `chat-completions` ou `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, ou NPU |
| `execution_provider` | string | Backend d’exécution (CUDA, CPU, QNN, WebGPU, etc.) |
| `file_size_mb` | int | Taille disque en MB |
| `supports_tool_calling` | bool | Le modèle supporte-t-il l’appel de fonctions/outils |
| `publisher` | string | Auteur/publicateur du modèle |
| `license` | string | Nom de la licence |
| `uri` | string | URI du modèle |
| `prompt_template` | dict/null | Modèle de prompt, si présent |

---

### Exercice 5 : Gérer le Cycle de Vie du Modèle

Pratiquez le cycle complet : lister → télécharger → charger → utiliser → décharger.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Petit modèle pour des tests rapides

manager = FoundryLocalManager()
manager.start_service()

# 1. Vérifier ce qui est dans le catalogue
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Vérifier ce qui est déjà téléchargé
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Télécharger un modèle
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Vérifier qu'il est maintenant dans le cache
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Le charger
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Vérifier ce qui est chargé
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Le décharger
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

const alias = "qwen2.5-0.5b"; // Petit modèle pour des tests rapides

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Obtenir le modèle depuis le catalogue
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Télécharger si nécessaire
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Le charger
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Le décharger
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Exercice 6 : Les modèles de démarrage rapide

Chaque langage offre un raccourci pour démarrer le service et charger un modèle en un seul appel. Ce sont les **modèles recommandés** pour la plupart des applications.

<details>
<summary><h3>🐍 Python - Initialisation par constructeur</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Passez un alias au constructeur - il gère tout :
# 1. Démarre le service s'il ne fonctionne pas
# 2. Télécharge le modèle s'il n'est pas mis en cache
# 3. Charge le modèle dans le serveur d'inférence
manager = FoundryLocalManager("phi-3.5-mini")

# Prêt à être utilisé immédiatement
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

Le paramètre `bootstrap` (par défaut `True`) contrôle ce comportement. Passez à `bootstrap=False` si vous souhaitez un contrôle manuel :

```python
# Mode manuel - rien ne se passe automatiquement
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catalogue</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() gère tout :
// 1. Démarre le service
// 2. Récupère le modèle du catalogue
// 3. Télécharge si nécessaire et charge le modèle
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Prêt à l'emploi immédiatement
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Catalogue</h3></summary>

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

> **Note C# :** Le SDK C# utilise `Configuration` pour contrôler le nom de l'application, les journaux, les répertoires de cache, et même pour fixer un port spécifique du serveur web. Cela en fait le SDK le plus configurable des trois.

</details>

---

### Exercice 7 : Le ChatClient natif (Pas besoin du SDK OpenAI)

Les SDK JavaScript et C# fournissent une méthode de commodité `createChatClient()` qui retourne un client de chat natif — pas besoin d'installer ou configurer séparément le SDK OpenAI.

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

// Créez un ChatClient directement à partir du modèle — aucune importation OpenAI nécessaire
const chatClient = model.createChatClient();

// completeChat renvoie un objet de réponse compatible avec OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Le streaming utilise un modèle de rappel
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

Le `ChatClient` supporte aussi l’appel d’outils — passez les outils en second argument :

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

> **Quand utiliser quel modèle :**
> - **`createChatClient()`** — Prototypage rapide, moins de dépendances, code plus simple
> - **SDK OpenAI** — Contrôle complet des paramètres (température, top_p, tokens d'arrêt, etc.), mieux pour les applications en production

---

### Exercice 8 : Variantes du modèle et choix du matériel

Les modèles peuvent avoir plusieurs **variantes** optimisées pour différents matériels. Le SDK sélectionne automatiquement la meilleure variante, mais vous pouvez aussi inspecter et choisir manuellement.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Lister toutes les variantes disponibles
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// Le SDK sélectionne automatiquement la meilleure variante pour votre matériel
// Pour remplacer, utilisez selectVariant():
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

En Python, le SDK sélectionne automatiquement la meilleure variante selon le matériel. Utilisez `get_model_info()` pour voir ce qui a été sélectionné :

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

#### Modèles avec variantes NPU

Certains modèles ont des variantes optimisées pour des unités de traitement neuronal (NPU) (Qualcomm Snapdragon, Intel Core Ultra) :

| Modèle | Variante NPU disponible |
|--------|:-----------------------:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Astuce :** Sur un matériel doté d’une NPU, le SDK sélectionne automatiquement la variante NPU quand elle est disponible. Vous n'avez pas besoin de modifier votre code. Pour les projets C# sur Windows, ajoutez le package NuGet `Microsoft.AI.Foundry.Local.WinML` pour activer le fournisseur d’exécution QNN — QNN est livré comme plugin EP via WinML.

---

### Exercice 9 : Mises à jour des modèles et rafraîchissement du catalogue

Le catalogue des modèles est mis à jour périodiquement. Utilisez ces méthodes pour vérifier et appliquer les mises à jour.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Actualisez le catalogue pour obtenir la liste des modèles la plus récente
manager.refresh_catalog()

# Vérifiez si un modèle mis en cache a une version plus récente disponible
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

// Actualiser le catalogue pour récupérer la dernière liste de modèles
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Lister tous les modèles disponibles après actualisation
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Exercice 10 : Travailler avec des modèles de raisonnement

Le modèle **phi-4-mini-reasoning** inclut un raisonnement en chaîne de pensée. Il enveloppe sa réflexion interne dans des balises `<think>...</think>` avant de produire sa réponse finale. C'est utile pour des tâches nécessitant une logique à plusieurs étapes, des maths, ou la résolution de problèmes.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning fait ~4,6 Go
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# Le modèle encadre sa réflexion avec des balises <think>...</think>
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

// Analyser la réflexion en chaîne
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Quand utiliser les modèles de raisonnement :**
> - Problèmes de maths et de logique
> - Tâches de planification à plusieurs étapes
> - Génération de code complexe
> - Tâches où montrer le raisonnement améliore la précision
>
> **Inconvénient :** Les modèles de raisonnement produisent plus de tokens (la section `<think>`) et sont plus lents. Pour les Q&R simples, un modèle standard comme phi-3.5-mini est plus rapide.

---

### Exercice 11 : Comprendre les alias et la sélection du matériel

Lorsque vous fournissez un **alias** (comme `phi-3.5-mini`) plutôt qu’un ID complet de modèle, le SDK sélectionne automatiquement la meilleure variante pour votre matériel :

| Matériel | Fournisseur d’exécution sélectionné |
|----------|------------------------------------|
| GPU NVIDIA (CUDA) | `CUDAExecutionProvider` |
| NPU Qualcomm | `QNNExecutionProvider` (via plugin WinML) |
| NPU Intel | `OpenVINOExecutionProvider` |
| GPU AMD | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Tout autre appareil (secours) | `CPUExecutionProvider` ou `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# L'alias correspond à la meilleure variante pour VOTRE matériel
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Astuce :** Utilisez toujours des alias dans votre code d’application. Lorsque vous déployez sur la machine d’un utilisateur, le SDK choisit la variante optimale au lancement — CUDA sur NVIDIA, QNN sur Qualcomm, CPU ailleurs.

---

### Exercice 12 : Options de configuration C#

La classe `Configuration` du SDK C# offre un contrôle fin sur le runtime :

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

| Propriété | Par défaut | Description |
|-----------|------------|-------------|
| `AppName` | (obligatoire) | Le nom de votre application |
| `LogLevel` | `Information` | Niveau de verbosité des logs |
| `Web.Urls` | (dynamique) | Fixer un port spécifique pour le serveur web |
| `AppDataDir` | Par défaut OS | Répertoire de base pour les données d’application |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Où les modèles sont stockés |
| `LogsDir` | `{AppDataDir}/logs` | Où les logs sont écrits |

---

### Exercice 13 : Utilisation dans le navigateur (JavaScript uniquement)

Le SDK JavaScript inclut une version compatible navigateur. Dans le navigateur, vous devez démarrer le service manuellement via le CLI et spécifier l’URL hôte :

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Démarrez d'abord le service manuellement :
//   foundry service start
// Ensuite, utilisez l'URL fournie par la sortie CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Parcourez le catalogue
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Limites dans le navigateur :** La version navigateur ne supporte **pas** `startWebService()`. Vous devez vous assurer que le service Foundry Local est déjà lancé avant d’utiliser le SDK dans un navigateur.

---

## Référence API complète

### Python

| Catégorie | Méthode | Description |
|-----------|---------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Créer un manager ; optionnellement démarrer avec un modèle |
| **Service** | `is_service_running()` | Vérifier si le service est en cours |
| **Service** | `start_service()` | Démarrer le service |
| **Service** | `endpoint` | URL de point d’accès API |
| **Service** | `api_key` | Clé API |
| **Catalogue** | `list_catalog_models()` | Lister tous les modèles disponibles |
| **Catalogue** | `refresh_catalog()` | Rafraîchir le catalogue |
| **Catalogue** | `get_model_info(alias_or_model_id)` | Obtenir les métadonnées du modèle |
| **Cache** | `get_cache_location()` | Chemin du répertoire cache |
| **Cache** | `list_cached_models()` | Lister les modèles téléchargés |
| **Modèle** | `download_model(alias_or_model_id)` | Télécharger un modèle |
| **Modèle** | `load_model(alias_or_model_id, ttl=600)` | Charger un modèle |
| **Modèle** | `unload_model(alias_or_model_id)` | Décharger un modèle |
| **Modèle** | `list_loaded_models()` | Lister les modèles chargés |
| **Modèle** | `is_model_upgradeable(alias_or_model_id)` | Vérifier si une version plus récente est disponible |
| **Modèle** | `upgrade_model(alias_or_model_id)` | Mettre à jour un modèle à la dernière version |
| **Service** | `httpx_client` | Client HTTPX préconfiguré pour appels API directs |

### JavaScript

| Catégorie | Méthode | Description |
|-----------|---------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Initialiser l’instance SDK |
| **Init** | `FoundryLocalManager.instance` | Accéder à l’instance singleton |
| **Service** | `manager.startWebService()` | Démarrer le service web |
| **Service** | `manager.urls` | Tableau des URLs de base du service |
| **Catalogue** | `manager.catalog` | Accéder au catalogue de modèles |
| **Catalogue** | `catalog.getModel(alias)` | Obtenir un objet modèle par alias (retourne une Promise) |
| **Modèle** | `model.isCached` | Indique si le modèle est téléchargé |
| **Modèle** | `model.download()` | Télécharger le modèle |
| **Modèle** | `model.load()` | Charger le modèle |
| **Modèle** | `model.unload()` | Décharger le modèle |
| **Modèle** | `model.id` | Identifiant unique du modèle |
| **Modèle** | `model.alias` | Alias du modèle |
| **Modèle** | `model.createChatClient()` | Obtenir un client de chat natif (pas besoin du SDK OpenAI) |
| **Modèle** | `model.createAudioClient()` | Obtenir un client audio pour transcription |
| **Modèle** | `model.removeFromCache()` | Supprimer le modèle du cache local |
| **Modèle** | `model.selectVariant(variant)` | Sélectionner une variante matérielle spécifique |
| **Modèle** | `model.variants` | Tableau des variantes disponibles pour ce modèle |
| **Modèle** | `model.isLoaded()` | Vérifier si le modèle est chargé |
| **Catalogue** | `catalog.getModels()` | Lister tous les modèles disponibles |
| **Catalogue** | `catalog.getCachedModels()` | Lister les modèles téléchargés |
| **Catalogue** | `catalog.getLoadedModels()` | Lister les modèles chargés actuellement |
| **Catalogue** | `catalog.updateModels()` | Rafraîchir le catalogue depuis le service |
| **Service** | `manager.stopWebService()` | Arrêter le service web Foundry Local |

### C# (v0.8.0+)

| Catégorie | Méthode | Description |
|-----------|---------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Initialiser le manager |
| **Init** | `FoundryLocalManager.Instance` | Accéder au singleton |
| **Catalogue** | `manager.GetCatalogAsync()` | Obtenir le catalogue |
| **Catalogue** | `catalog.ListModelsAsync()` | Lister tous les modèles |
| **Catalogue** | `catalog.GetModelAsync(alias)` | Obtenir un modèle spécifique |
| **Catalogue** | `catalog.GetCachedModelsAsync()` | Lister les modèles en cache |
| **Catalogue** | `catalog.GetLoadedModelsAsync()` | Lister les modèles chargés |
| **Modèle** | `model.DownloadAsync(progress?)` | Télécharger un modèle |
| **Modèle** | `model.LoadAsync()` | Charger un modèle |
| **Modèle** | `model.UnloadAsync()` | Décharger un modèle |
| **Modèle** | `model.SelectVariant(variant)` | Choisir une variante matérielle |
| **Modèle** | `model.GetChatClientAsync()` | Obtenir un client de chat natif |
| **Modèle** | `model.GetAudioClientAsync()` | Obtenir un client de transcription audio |
| **Modèle** | `model.GetPathAsync()` | Obtenir le chemin local du fichier |
| **Catalogue** | `catalog.GetModelVariantAsync(alias, variant)` | Obtenir une variante matérielle spécifique |
| **Catalogue** | `catalog.UpdateModelsAsync()` | Rafraîchir le catalogue |
| **Serveur** | `manager.StartWebServerAsync()` | Démarrer le serveur REST |
| **Serveur** | `manager.StopWebServerAsync()` | Arrêter le serveur REST |
| **Config** | `config.ModelCacheDir` | Répertoire cache |

---

## Points clés à retenir

| Concept | Ce que vous avez appris |
|---------|------------------------|
| **SDK vs CLI** | Le SDK offre un contrôle programmatique — essentiel pour les applications |
| **Plan de contrôle** | Le SDK gère les services, modèles, et caches |
| **Ports dynamiques** | Utilisez toujours `manager.endpoint` (Python) ou `manager.urls[0]` (JS/C#) — ne hardcodez jamais un port |
| **Alias** | Utilisez des alias pour la sélection automatique du modèle optimale selon le matériel |
| **Démarrage rapide** | Python : `FoundryLocalManager(alias)`, JS : `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Refonte C#** | v0.8.0+ est autonome - aucun CLI requis sur les machines des utilisateurs finaux |
| **Cycle de vie du modèle** | Catalogue → Téléchargement → Chargement → Utilisation → Déchargement |
| **FoundryModelInfo** | Métadonnées enrichies : tâche, appareil, taille, licence, prise en charge de l'appel d'outil |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) pour une utilisation sans OpenAI |
| **Variantes** | Les modèles ont des variantes spécifiques au matériel (CPU, GPU, NPU) ; sélectionnées automatiquement |
| **Mises à niveau** | Python : `is_model_upgradeable()` + `upgrade_model()` pour maintenir les modèles à jour |
| **Actualisation du catalogue** | `refresh_catalog()` (Python) / `updateModels()` (JS) pour découvrir de nouveaux modèles |

---

## Ressources

| Ressource | Lien |
|----------|------|
| Référence SDK (toutes langues) | [Microsoft Learn - Référence SDK Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Intégration avec les SDKs d'inférence | [Microsoft Learn - Intégration SDK d'inférence](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Référence API SDK C# | [Référence API Foundry Local C#](https://aka.ms/fl-csharp-api-ref) |
| Exemples SDK C# | [GitHub - Exemples SDK Foundry Local](https://aka.ms/foundrylocalSDK) |
| Site Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Étapes suivantes

Continuez avec [Partie 3 : Utiliser le SDK avec OpenAI](part3-sdk-and-apis.md) pour connecter le SDK à la bibliothèque cliente OpenAI et créer votre première application de complétion de chat.