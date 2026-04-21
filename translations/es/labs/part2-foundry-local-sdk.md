![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 2: Profundización en Foundry Local SDK

> **Objetivo:** Dominar Foundry Local SDK para gestionar modelos, servicios y caché de forma programática, y entender por qué el SDK es el enfoque recomendado frente a la CLI para construir aplicaciones.

## Resumen

En la Parte 1 usaste la **Foundry Local CLI** para descargar y ejecutar modelos de forma interactiva. La CLI es ideal para exploración, pero cuando construyes aplicaciones reales necesitas **control programático**. Foundry Local SDK te ofrece eso: gestiona el **plano de control** (iniciar el servicio, descubrir modelos, descargar, cargar) para que el código de tu aplicación pueda enfocarse en el **plano de datos** (enviar indicaciones, recibir completaciones).

Este laboratorio te enseña toda la superficie de API del SDK para Python, JavaScript y C#. Al final entenderás cada método disponible y cuándo usar cada uno.

## Objetivos de Aprendizaje

Al final de este laboratorio serás capaz de:

- Explicar por qué el SDK es preferido sobre la CLI para desarrollo de aplicaciones
- Instalar Foundry Local SDK para Python, JavaScript o C#
- Usar `FoundryLocalManager` para iniciar el servicio, gestionar modelos y consultar el catálogo
- Listar, descargar, cargar y descargar modelos de forma programática
- Inspeccionar metadatos de modelos usando `FoundryModelInfo`
- Entender la diferencia entre catálogo, caché y modelos cargados
- Usar el constructor bootstrap (Python) y el patrón `create()` + catálogo (JavaScript)
- Comprender el rediseño del SDK en C# y su API orientada a objetos

---

## Prerrequisitos

| Requisito | Detalles |
|-----------|----------|
| **Foundry Local CLI** | Instalado y en tu `PATH` ([Parte 1](part1-getting-started.md)) |
| **Entorno de ejecución** | **Python 3.9+** y/o **Node.js 18+** y/o **.NET 9.0+** |

---

## Concepto: SDK vs CLI - ¿Por qué usar el SDK?

| Aspecto | CLI (comando `foundry`) | SDK (`foundry-local-sdk`) |
|---------|-------------------------|---------------------------|
| **Caso de uso** | Exploración, pruebas manuales | Integración en aplicaciones |
| **Gestión del servicio** | Manual: `foundry service start` | Automático: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Descubrimiento de puerto** | Se lee de la salida CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Descarga de modelo** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Manejo de errores** | Códigos de salida, stderr | Excepciones, errores tipados |
| **Automatización** | Scripts de shell | Integración nativa en el lenguaje |
| **Despliegue** | Requiere CLI en máquina final | El SDK en C# puede ser independiente (no requiere CLI) |

> **Insight clave:** El SDK maneja todo el ciclo de vida: iniciar el servicio, verificar caché, descargar modelos faltantes, cargarlos y descubrir el endpoint, en pocas líneas. Tu aplicación no necesita parsear salida CLI ni gestionar subprocesos.

---

## Ejercicios del Laboratorio

### Ejercicio 1: Instalar el SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Verifica la instalación:

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

Verifica la instalación:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Hay dos paquetes NuGet:

| Paquete | Plataforma | Descripción |
|---------|------------|-------------|
| `Microsoft.AI.Foundry.Local` | Multiplataforma | Funciona en Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Solo Windows | Añade aceleración por hardware WinML; descarga e instala proveedores de ejecución de plugins (ej. QNN para Qualcomm NPU) |

**Configuración en Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Edita el archivo `.csproj`:

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

> **Nota:** En Windows, el paquete WinML es un superconjunto que incluye el SDK base más el proveedor de ejecución QNN. En Linux/macOS se usa sólo el SDK base. El TFM condicional y referencias de paquete mantienen el proyecto completamente multiplataforma.

Crea un `nuget.config` en la raíz del proyecto:

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

Restaura los paquetes:

```bash
dotnet restore
```

</details>

---

### Ejercicio 2: Iniciar el Servicio y Listar el Catálogo

Lo primero que hace cualquier aplicación es iniciar el servicio Foundry Local y descubrir qué modelos están disponibles.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Crear un gestor y iniciar el servicio
manager = FoundryLocalManager()
manager.start_service()

# Listar todos los modelos disponibles en el catálogo
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### SDK de Python - Métodos de gestión del servicio

| Método | Firma | Descripción |
|--------|-------|-------------|
| `is_service_running()` | `() -> bool` | Verifica si el servicio está corriendo |
| `start_service()` | `() -> None` | Inicia el servicio Foundry Local |
| `service_uri` | `@property -> str` | URI base del servicio |
| `endpoint` | `@property -> str` | Endpoint de la API (URI del servicio + `/v1`) |
| `api_key` | `@property -> str` | Clave API (de entorno o valor por defecto) |

#### SDK de Python - Métodos de gestión del catálogo

| Método | Firma | Descripción |
|--------|-------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Lista todos los modelos en el catálogo |
| `refresh_catalog()` | `() -> None` | Actualiza el catálogo desde el servicio |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Obtiene info de un modelo específico |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Crear un gestor e iniciar el servicio
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Explorar el catálogo
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### SDK de JavaScript - Métodos del Manager

| Método | Firma | Descripción |
|--------|-------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inicializa el singleton del SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Accede al manager singleton |
| `manager.startWebService()` | `() => Promise<void>` | Inicia el servicio web Foundry Local |
| `manager.urls` | `string[]` | Array de URLs base del servicio |

#### SDK de JavaScript - Métodos de catálogo y modelo

| Método | Firma | Descripción |
|--------|-------|-------------|
| `manager.catalog` | `Catalog` | Accede al catálogo de modelos |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Obtiene un objeto modelo por alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

El SDK C# v0.8.0+ usa arquitectura orientada a objetos con objetos `Configuration`, `Catalog` y `Model`:

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

#### SDK C# - Clases clave

| Clase | Propósito |
|-------|-----------|
| `Configuration` | Configura nombre app, nivel log, directorio caché, URLs servidor web |
| `FoundryLocalManager` | Punto de entrada principal - creado vía `CreateAsync()`, accedido vía `.Instance` |
| `Catalog` | Navega, busca y obtiene modelos del catálogo |
| `Model` | Representa un modelo específico - descarga, carga, obtiene clientes |

#### SDK C# - Métodos del Manager y Catálogo

| Método | Descripción |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inicializa el manager |
| `FoundryLocalManager.Instance` | Accede al manager singleton |
| `manager.GetCatalogAsync()` | Obtiene el catálogo de modelos |
| `catalog.ListModelsAsync()` | Lista todos los modelos disponibles |
| `catalog.GetModelAsync(alias: "alias")` | Obtiene un modelo específico por alias |
| `catalog.GetCachedModelsAsync()` | Lista modelos descargados |
| `catalog.GetLoadedModelsAsync()` | Lista modelos cargados actualmente |

> **Nota sobre arquitectura C#:** El rediseño del SDK v0.8.0+ hace que la aplicación sea **autocontenida**; no requiere Foundry Local CLI en la máquina final. El SDK gestiona modelos e inferencia nativamente.

</details>

---

### Ejercicio 3: Descargar y Cargar un Modelo

El SDK separa la descarga (a disco) de la carga (en memoria). Esto te permite predescargar modelos durante la configuración y cargarlos bajo demanda.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Opción A: Paso a paso manual
manager = FoundryLocalManager()
manager.start_service()

# Verificar caché primero
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

# Opción B: Inicialización en una sola línea (recomendado)
# Pasar alias al constructor - inicia el servicio, descarga y carga automáticamente
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Métodos de gestión de modelo

| Método | Firma | Descripción |
|--------|-------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Descarga un modelo en caché local |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Carga un modelo en el servidor de inferencia |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Descarga un modelo del servidor |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Lista todos los modelos cargados actualmente |

#### Python - Métodos de gestión de caché

| Método | Firma | Descripción |
|--------|-------|-------------|
| `get_cache_location()` | `() -> str` | Obtiene la ruta del directorio de caché |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Lista todos los modelos descargados |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Enfoque paso a paso
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

#### JavaScript - Métodos de modelo

| Método | Firma | Descripción |
|--------|-------|-------------|
| `model.isCached` | `boolean` | Indica si el modelo está ya descargado |
| `model.download()` | `() => Promise<void>` | Descarga el modelo a caché local |
| `model.load()` | `() => Promise<void>` | Carga en el servidor de inferencia |
| `model.unload()` | `() => Promise<void>` | Descarga del servidor de inferencia |
| `model.id` | `string` | Identificador único del modelo |

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

#### C# - Métodos de modelo

| Método | Descripción |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Descarga la variante seleccionada |
| `model.LoadAsync()` | Carga el modelo en memoria |
| `model.UnloadAsync()` | Descarga el modelo |
| `model.SelectVariant(variant)` | Selecciona una variante específica (CPU/GPU/NPU) |
| `model.SelectedVariant` | La variante seleccionada actualmente |
| `model.Variants` | Todas las variantes disponibles para este modelo |
| `model.GetPathAsync()` | Obtiene la ruta local del archivo |
| `model.GetChatClientAsync()` | Obtiene un cliente de chat nativo (no necesita SDK OpenAI) |
| `model.GetAudioClientAsync()` | Obtiene un cliente de audio para transcripción |

</details>

---

### Ejercicio 4: Inspeccionar Metadatos del Modelo

El objeto `FoundryModelInfo` contiene metadatos enriquecidos de cada modelo. Entender estos campos te ayuda a elegir el modelo adecuado para tu aplicación.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Obtener información detallada sobre un modelo específico
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

#### Campos de FoundryModelInfo

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `alias` | string | Nombre corto (ej. `phi-3.5-mini`) |
| `id` | string | Identificador único de modelo |
| `version` | string | Versión del modelo |
| `task` | string | `chat-completions` o `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, o NPU |
| `execution_provider` | string | Backend de ejecución (CUDA, CPU, QNN, WebGPU, etc.) |
| `file_size_mb` | int | Tamaño en disco en MB |
| `supports_tool_calling` | bool | Si el modelo soporta llamadas a funciones/herramientas |
| `publisher` | string | Quién publicó el modelo |
| `license` | string | Nombre de la licencia |
| `uri` | string | URI del modelo |
| `prompt_template` | dict/null | Plantilla de prompt, si existe |

---

### Ejercicio 5: Gestionar Ciclo de Vida del Modelo

Practica el ciclo completo: listar → descargar → cargar → usar → descargar.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Modelo pequeño para pruebas rápidas

manager = FoundryLocalManager()
manager.start_service()

# 1. Ver qué hay en el catálogo
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Ver qué ya está descargado
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Descargar un modelo
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Verificar que ahora está en la caché
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Cargarlo
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Ver qué está cargado
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Descargarlo (desactivar)
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

const alias = "qwen2.5-0.5b"; // Modelo pequeño para pruebas rápidas

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Obtener el modelo del catálogo
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Descargar si es necesario
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Cargarlo
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Descargarlo
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Ejercicio 6: Los Patrones de Inicio Rápido

Cada lenguaje proporciona un atajo para iniciar el servicio y cargar un modelo en una sola llamada. Estos son los **patrones recomendados** para la mayoría de las aplicaciones.

<details>
<summary><h3>🐍 Python - Bootstrap del Constructor</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Pasa un alias al constructor - maneja todo:
# 1. Inicia el servicio si no está en ejecución
# 2. Descarga el modelo si no está en caché
# 3. Carga el modelo en el servidor de inferencia
manager = FoundryLocalManager("phi-3.5-mini")

# Listo para usar de inmediato
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

El parámetro `bootstrap` (por defecto `True`) controla este comportamiento. Establece `bootstrap=False` si quieres control manual:

```python
# Modo manual - nada sucede automáticamente
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catálogo</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() manejan todo:
// 1. Inicia el servicio
// 2. Obtiene el modelo del catálogo
// 3. Descarga si es necesario y carga el modelo
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Listo para usar de inmediato
console.log(`Endpoint: ${manager.urls[0]}/v1`);
console.log(`Model ID: ${model.id}`);
```

</details>

<details>
<summary><h3>💜 C# - `CreateAsync()` + Catálogo</h3></summary>

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

> **Nota C#:** El SDK de C# usa `Configuration` para controlar el nombre de la app, el registro, directorios de caché e incluso fijar un puerto específico para el servidor web. Esto lo convierte en el más configurable de los tres SDK.

</details>

---

### Ejercicio 7: El ChatClient Nativo (No se Necesita OpenAI SDK)

Los SDK de JavaScript y C# proporcionan un método de conveniencia `createChatClient()` que retorna un cliente de chat nativo — no es necesario instalar o configurar el OpenAI SDK por separado.

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

// Crear un ChatClient directamente desde el modelo — no se necesita importar OpenAI
const chatClient = model.createChatClient();

// completeChat devuelve un objeto de respuesta compatible con OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// La transmisión utiliza un patrón de devolución de llamada
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

El `ChatClient` también soporta llamadas a herramientas — pasa las herramientas como segundo argumento:

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

> **Cuándo usar cada patrón:**
> - **`createChatClient()`** — Prototipado rápido, menos dependencias, código más simple
> - **OpenAI SDK** — Control total sobre parámetros (temperatura, top_p, tokens de parada, etc.), mejor para aplicaciones en producción

---

### Ejercicio 8: Variantes del Modelo y Selección de Hardware

Los modelos pueden tener múltiples **variantes** optimizadas para distintos hardware. El SDK selecciona la mejor variante automáticamente, pero también puedes inspeccionarlas y elegir manualmente.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Lista todas las variantes disponibles
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// El SDK selecciona automáticamente la mejor variante para tu hardware
// Para anular, usa selectVariant():
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

En Python, el SDK selecciona automáticamente la mejor variante según el hardware. Usa `get_model_info()` para ver cuál se seleccionó:

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

#### Modelos con Variantes NPU

Algunos modelos tienen variantes optimizadas para NPU en dispositivos con Unidades de Procesamiento Neural (Qualcomm Snapdragon, Intel Core Ultra):

| Modelo | Variante NPU Disponible |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Consejo:** En hardware con NPU, el SDK selecciona automáticamente la variante NPU cuando está disponible. No necesitas cambiar tu código. Para proyectos C# en Windows, agrega el paquete NuGet `Microsoft.AI.Foundry.Local.WinML` para habilitar el proveedor de ejecución QNN — QNN se entrega como un plugin EP a través de WinML.

---

### Ejercicio 9: Actualizaciones de Modelo y Refresco del Catálogo

El catálogo de modelos se actualiza periódicamente. Usa estos métodos para comprobar y aplicar actualizaciones.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Actualizar el catálogo para obtener la lista más reciente de modelos
manager.refresh_catalog()

# Verificar si un modelo en caché tiene una versión más nueva disponible
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

// Actualice el catálogo para obtener la lista más reciente de modelos
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Liste todos los modelos disponibles después de la actualización
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Ejercicio 10: Trabajando con Modelos de Razonamiento

El modelo **phi-4-mini-reasoning** incluye razonamiento tipo cadena de pensamiento. Envuelve su pensamiento interno en etiquetas `<think>...</think>` antes de producir su respuesta final. Esto es útil para tareas que requieren lógica multinivel, matemáticas o resolución de problemas.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning es ~4.6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# El modelo envuelve su pensamiento en etiquetas <think>...</think>
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

// Analizar el pensamiento en cadena
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Cuándo usar modelos de razonamiento:**
> - Problemas de matemáticas y lógica
> - Tareas de planificación en múltiples pasos
> - Generación compleja de código
> - Tareas donde mostrar el proceso mejora la precisión
>
> **Compensación:** Los modelos de razonamiento producen más tokens (la sección `<think>`) y son más lentos. Para preguntas y respuestas simples, un modelo estándar como phi-3.5-mini es más rápido.

---

### Ejercicio 11: Entendiendo Alias y Selección de Hardware

Cuando pasas un **alias** (como `phi-3.5-mini`) en lugar de un ID completo, el SDK selecciona automáticamente la mejor variante para tu hardware:

| Hardware | Proveedor de Ejecución Seleccionado |
|----------|---------------------------|
| GPU NVIDIA (CUDA) | `CUDAExecutionProvider` |
| NPU Qualcomm | `QNNExecutionProvider` (a través del plugin WinML) |
| NPU Intel | `OpenVINOExecutionProvider` |
| GPU AMD | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Cualquier dispositivo (fallback) | `CPUExecutionProvider` o `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# El alias se resuelve a la mejor variante para TU hardware
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Consejo:** Usa siempre alias en tu código de aplicación. Cuando despliegues en la máquina de un usuario, el SDK elige la variante óptima en tiempo de ejecución - CUDA en NVIDIA, QNN en Qualcomm, CPU en otros.

---

### Ejercicio 12: Opciones de Configuración en C#

La clase `Configuration` del SDK C# proporciona control granular sobre el runtime:

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

| Propiedad | Valor Predeterminado | Descripción |
|----------|---------|-------------|
| `AppName` | (requerido) | Nombre de tu aplicación |
| `LogLevel` | `Information` | Verbosidad del registro |
| `Web.Urls` | (dinámico) | Fijar un puerto específico para el servidor web |
| `AppDataDir` | Por defecto del OS | Directorio base para datos de la app |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Donde se almacenan los modelos |
| `LogsDir` | `{AppDataDir}/logs` | Donde se guardan los registros |

---

### Ejercicio 13: Uso en Navegador (Solo JavaScript)

El SDK de JavaScript incluye una versión compatible con navegador. En el navegador, debes iniciar manualmente el servicio vía CLI y especificar la URL del host:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Inicie el servicio manualmente primero:
//   foundry service start
// Luego use la URL del resultado de la CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Navegue por el catálogo
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Limitaciones del navegador:** La versión para navegador **no** soporta `startWebService()`. Debes asegurarte de que el servicio Foundry Local ya esté ejecutándose antes de usar el SDK en un navegador.

---

## Referencia Completa de la API

### Python

| Categoría | Método | Descripción |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Crear manager; opcionalmente bootstrap con un modelo |
| **Service** | `is_service_running()` | Verificar si el servicio está corriendo |
| **Service** | `start_service()` | Iniciar el servicio |
| **Service** | `endpoint` | URL del endpoint API |
| **Service** | `api_key` | Clave API |
| **Catalog** | `list_catalog_models()` | Listar todos los modelos disponibles |
| **Catalog** | `refresh_catalog()` | Refrescar el catálogo |
| **Catalog** | `get_model_info(alias_or_model_id)` | Obtener metadata del modelo |
| **Cache** | `get_cache_location()` | Ruta del directorio de caché |
| **Cache** | `list_cached_models()` | Listar modelos descargados |
| **Model** | `download_model(alias_or_model_id)` | Descargar un modelo |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Cargar un modelo |
| **Model** | `unload_model(alias_or_model_id)` | Descargar un modelo (liberar) |
| **Model** | `list_loaded_models()` | Listar modelos cargados |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Verificar si hay versión más nueva disponible |
| **Model** | `upgrade_model(alias_or_model_id)` | Actualizar un modelo a la última versión |
| **Service** | `httpx_client` | Cliente HTTPX preconfigurado para llamadas API directas |

### JavaScript

| Categoría | Método | Descripción |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Inicializar el singleton del SDK |
| **Init** | `FoundryLocalManager.instance` | Acceder al manager singleton |
| **Service** | `manager.startWebService()` | Iniciar el servicio web |
| **Service** | `manager.urls` | Array con URL base del servicio |
| **Catalog** | `manager.catalog` | Acceder al catálogo de modelos |
| **Catalog** | `catalog.getModel(alias)` | Obtener un modelo por alias (devuelve Promise) |
| **Model** | `model.isCached` | Si el modelo está descargado |
| **Model** | `model.download()` | Descargar el modelo |
| **Model** | `model.load()` | Cargar el modelo |
| **Model** | `model.unload()` | Descargar (liberar) el modelo |
| **Model** | `model.id` | Identificador único del modelo |
| **Model** | `model.alias` | Alias del modelo |
| **Model** | `model.createChatClient()` | Obtener un cliente de chat nativo (sin OpenAI SDK) |
| **Model** | `model.createAudioClient()` | Obtener cliente de audio para transcripción |
| **Model** | `model.removeFromCache()` | Eliminar el modelo de la caché local |
| **Model** | `model.selectVariant(variant)` | Seleccionar variante de hardware específica |
| **Model** | `model.variants` | Array de variantes disponibles para este modelo |
| **Model** | `model.isLoaded()` | Verificar si el modelo está cargado |
| **Catalog** | `catalog.getModels()` | Listar todos los modelos disponibles |
| **Catalog** | `catalog.getCachedModels()` | Listar modelos descargados |
| **Catalog** | `catalog.getLoadedModels()` | Listar modelos actualmente cargados |
| **Catalog** | `catalog.updateModels()` | Refrescar catálogo desde el servicio |
| **Service** | `manager.stopWebService()` | Detener el servicio web Foundry Local |

### C# (v0.8.0+)

| Categoría | Método | Descripción |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Inicializar el manager |
| **Init** | `FoundryLocalManager.Instance` | Acceder al singleton |
| **Catalog** | `manager.GetCatalogAsync()` | Obtener catálogo |
| **Catalog** | `catalog.ListModelsAsync()` | Listar todos los modelos |
| **Catalog** | `catalog.GetModelAsync(alias)` | Obtener modelo específico |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Listar modelos en caché |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Listar modelos cargados |
| **Model** | `model.DownloadAsync(progress?)` | Descargar un modelo |
| **Model** | `model.LoadAsync()` | Cargar un modelo |
| **Model** | `model.UnloadAsync()` | Descargar un modelo |
| **Model** | `model.SelectVariant(variant)` | Elegir variante de hardware |
| **Model** | `model.GetChatClientAsync()` | Obtener cliente de chat nativo |
| **Model** | `model.GetAudioClientAsync()` | Obtener cliente para transcripción de audio |
| **Model** | `model.GetPathAsync()` | Obtener ruta local del archivo |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Obtener variante específica de hardware |
| **Catalog** | `catalog.UpdateModelsAsync()` | Refrescar catálogo |
| **Server** | `manager.StartWebServerAsync()` | Iniciar servidor web REST |
| **Server** | `manager.StopWebServerAsync()` | Detener servidor web REST |
| **Config** | `config.ModelCacheDir` | Directorio de caché |

---

## Puntos Clave

| Concepto | Lo que Aprendiste |
|---------|-----------------|
| **SDK vs CLI** | El SDK provee control programático - esencial para aplicaciones |
| **Plano de control** | El SDK maneja servicios, modelos y cachés |
| **Puertos dinámicos** | Usa siempre `manager.endpoint` (Python) o `manager.urls[0]` (JS/C#) — nunca fijes un puerto estático |
| **Alias** | Usa alias para selección automática de modelo óptimo según hardware |
| **Inicio rápido** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Rediseño en C#** | v0.8.0+ es autónomo - no se necesita CLI en los equipos de los usuarios finales |
| **Ciclo de vida del modelo** | Catálogo → Descargar → Cargar → Usar → Descargar |
| **FoundryModelInfo** | Metadatos enriquecidos: tarea, dispositivo, tamaño, licencia, soporte de llamada de herramienta |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) para uso sin OpenAI |
| **Variantes** | Los modelos tienen variantes específicas de hardware (CPU, GPU, NPU); seleccionadas automáticamente |
| **Actualizaciones** | Python: `is_model_upgradeable()` + `upgrade_model()` para mantener los modelos actualizados |
| **Actualización del catálogo** | `refresh_catalog()` (Python) / `updateModels()` (JS) para descubrir nuevos modelos |

---

## Recursos

| Recurso | Enlace |
|----------|------|
| Referencia del SDK (todos los lenguajes) | [Microsoft Learn - Referencia SDK Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrar con SDKs de inferencia | [Microsoft Learn - Integración con SDKs de Inferencia](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Referencia de API del SDK C# | [Referencia API C# Foundry Local](https://aka.ms/fl-csharp-api-ref) |
| Muestras del SDK en C# | [GitHub - Muestras SDK Foundry Local](https://aka.ms/foundrylocalSDK) |
| Sitio web de Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Próximos pasos

Continúa con [Parte 3: Usar el SDK con OpenAI](part3-sdk-and-apis.md) para conectar el SDK con la biblioteca cliente de OpenAI y crear tu primera aplicación de completado de chat.