![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 2: Exploração Profunda do Foundry Local SDK

> **Objetivo:** Dominar o Foundry Local SDK para gerir modelos, serviços e caching programaticamente - e compreender por que o SDK é a abordagem recomendada em relação ao CLI para construir aplicações.

## Visão Geral

Na Parte 1 usou o **Foundry Local CLI** para descarregar e executar modelos interativamente. O CLI é ótimo para exploração, mas quando constrói aplicações reais precisa de **controlo programático**. O Foundry Local SDK dá-lhe isso - gere o **plano de controlo** (arrancar o serviço, descobrir modelos, descarregar, carregar) para que o código da sua aplicação se possa focar no **plano de dados** (enviar prompts, receber conclusões).

Este laboratório ensina a API completa do SDK em Python, JavaScript, e C#. No final compreenderá todos os métodos disponíveis e quando usar cada um.

## Objetivos de Aprendizagem

No final deste laboratório será capaz de:

- Explicar por que o SDK é preferido em relação ao CLI para desenvolvimento de aplicações
- Instalar o Foundry Local SDK para Python, JavaScript, ou C#
- Usar `FoundryLocalManager` para arrancar o serviço, gerir modelos, e consultar o catálogo
- Listar, descarregar, carregar e descarregar modelos programaticamente
- Inspecionar metadados do modelo usando `FoundryModelInfo`
- Compreender a diferença entre catálogo, cache, e modelos carregados
- Usar o constructor bootstrap (Python) e o padrão `create()` + catálogo (JavaScript)
- Compreender o redesenho do SDK C# e a sua API orientada a objetos

---

## Pré-requisitos

| Requisito | Detalhes |
|-------------|---------|
| **Foundry Local CLI** | Instalado e no seu `PATH` ([Parte 1](part1-getting-started.md)) |
| **Ambiente de execução da linguagem** | **Python 3.9+** e/ou **Node.js 18+** e/ou **.NET 9.0+** |

---

## Conceito: SDK vs CLI - Por Que Usar o SDK?

| Aspeto | CLI (comando `foundry`) | SDK (`foundry-local-sdk`) |
|--------|-------------------------|---------------------------|
| **Caso de uso** | Exploração, testes manuais | Integração em aplicações |
| **Gestão do serviço** | Manual: `foundry service start` | Automática: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Descoberta de porta** | Lida a partir da saída do CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Descarregar modelo** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Tratamento de erros** | Códigos de saída, stderr | Exceções, erros tipados |
| **Automação** | Scripts shell | Integração nativa na linguagem |
| **Deploy** | Requer CLI na máquina do utilizador final | O SDK C# pode ser autocontido (sem CLI necessário) |

> **Insight chave:** O SDK gere todo o ciclo de vida: arrancar o serviço, verificar o cache, descarregar modelos em falta, carregá-los, e descobrir o endpoint, com poucas linhas de código. A sua aplicação não precisa de analisar a saída do CLI nem gerir processos filhos.

---

## Exercícios do Laboratório

### Exercício 1: Instalar o SDK

<details>
<summary><h3>🐍 Python</h3></summary>

```bash
pip install foundry-local-sdk
```

Verifique a instalação:

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

Verifique a instalação:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
console.log("SDK installed successfully");
```

</details>

<details>
<summary><h3>💜 C#</h3></summary>

Existem dois pacotes NuGet:

| Pacote | Plataforma | Descrição |
|---------|------------|-----------|
| `Microsoft.AI.Foundry.Local` | Multiplataforma | Funciona em Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Apenas Windows | Adiciona aceleração de hardware WinML; descarrega e instala providers de execução para plugins (p.ex. QNN para Qualcomm NPU) |

**Configuração para Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Edite o ficheiro `.csproj`:

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

> **Nota:** No Windows, o pacote WinML é um superset que inclui o SDK base mais o provider de execução QNN. No Linux/macOS, usa-se o SDK base. O TFM condicional e referências a pacotes mantêm o projeto totalmente multiplataforma.

Crie um `nuget.config` na raiz do projeto:

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

Restaure os pacotes:

```bash
dotnet restore
```

</details>

---

### Exercício 2: Arrancar o Serviço e Listar o Catálogo

A primeira coisa que qualquer aplicação faz é arrancar o serviço Foundry Local e descobrir que modelos estão disponíveis.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Criar um gestor e iniciar o serviço
manager = FoundryLocalManager()
manager.start_service()

# Listar todos os modelos disponíveis no catálogo
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### SDK Python - Métodos de Gestão do Serviço

| Método | Assinatura | Descrição |
|--------|------------|-----------|
| `is_service_running()` | `() -> bool` | Verifica se o serviço está a correr |
| `start_service()` | `() -> None` | Arranca o serviço Foundry Local |
| `service_uri` | `@property -> str` | URI base do serviço |
| `endpoint` | `@property -> str` | Endpoint da API (URI do serviço + `/v1`) |
| `api_key` | `@property -> str` | Chave API (do ambiente ou placeholder padrão) |

#### SDK Python - Métodos de Gestão do Catálogo

| Método | Assinatura | Descrição |
|--------|------------|-----------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Lista todos os modelos do catálogo |
| `refresh_catalog()` | `() -> None` | Atualiza o catálogo a partir do serviço |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Obtém informação de um modelo específico |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Crie um gestor e inicie o serviço
FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Navegue pelo catálogo
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");
console.log(`Model alias: ${model.alias}`);
console.log(`Model ID:    ${model.id}`);
console.log(`Cached:      ${model.isCached}`);
```

#### SDK JavaScript - Métodos do Manager

| Método | Assinatura | Descrição |
|--------|------------|-----------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inicializa o singleton do SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Acede ao manager singleton |
| `manager.startWebService()` | `() => Promise<void>` | Arranca o serviço web Foundry Local |
| `manager.urls` | `string[]` | Array de URLs base para o serviço |

#### SDK JavaScript - Métodos de Catálogo e Modelo

| Método | Assinatura | Descrição |
|--------|------------|-----------|
| `manager.catalog` | `Catalog` | Acede ao catálogo de modelos |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Obtém objeto modelo por alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

O SDK C# v0.8.0+ usa uma arquitetura orientada a objetos com objetos `Configuration`, `Catalog`, e `Model`:

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

#### SDK C# - Classes Principais

| Classe | Propósito |
|--------|-----------|
| `Configuration` | Define nome da app, nível de log, diretório de cache, URLs do servidor web |
| `FoundryLocalManager` | Ponto de entrada principal - criado via `CreateAsync()`, acedido via `.Instance` |
| `Catalog` | Navegar, pesquisar e obter modelos do catálogo |
| `Model` | Representa um modelo específico - descarregar, carregar, obter clientes |

#### SDK C# - Métodos do Manager e Catálogo

| Método | Descrição |
|--------|-----------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inicializa o manager |
| `FoundryLocalManager.Instance` | Acede ao manager singleton |
| `manager.GetCatalogAsync()` | Obtém o catálogo de modelos |
| `catalog.ListModelsAsync()` | Lista todos os modelos disponíveis |
| `catalog.GetModelAsync(alias: "alias")` | Obtém um modelo específico por alias |
| `catalog.GetCachedModelsAsync()` | Lista os modelos descarregados |
| `catalog.GetLoadedModelsAsync()` | Lista os modelos atualmente carregados |

> **Nota sobre a arquitetura C#:** O redesenho do SDK C# v0.8.0+ torna a aplicação **autocontida**; não requer o Foundry Local CLI na máquina do utilizador final. O SDK gere a gestão do modelo e inferência nativamente.

</details>

---

### Exercício 3: Descarregar e Carregar um Modelo

O SDK separa a descarregamento (para disco) do carregamento (para memória). Isto permite pré-descarregar modelos durante a configuração e carregá-los quando necessário.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Opção A: Passo a passo manual
manager = FoundryLocalManager()
manager.start_service()

# Verificar cache primeiro
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

# Opção B: Bootstrap numa linha (recomendado)
# Passar alias ao construtor - isto inicia o serviço, descarrega e carrega automaticamente
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Métodos de Gestão de Modelo

| Método | Assinatura | Descrição |
|--------|------------|-----------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Descarrega um modelo para o cache local |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Carrega um modelo no servidor de inferência |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Descarrega um modelo do servidor |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Lista todos os modelos atualmente carregados |

#### Python - Métodos de Gestão de Cache

| Método | Assinatura | Descrição |
|--------|------------|-----------|
| `get_cache_location()` | `() -> str` | Obtém o caminho do diretório de cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Lista todos os modelos descarregados |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Abordagem passo a passo
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

#### JavaScript - Métodos do Modelo

| Método | Assinatura | Descrição |
|--------|------------|-----------|
| `model.isCached` | `boolean` | Indica se o modelo já está descarregado |
| `model.download()` | `() => Promise<void>` | Descarrega o modelo para o cache local |
| `model.load()` | `() => Promise<void>` | Carrega no servidor de inferência |
| `model.unload()` | `() => Promise<void>` | Descarrega do servidor de inferência |
| `model.id` | `string` | Identificador único do modelo |

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

#### C# - Métodos do Modelo

| Método | Descrição |
|--------|-----------|
| `model.DownloadAsync(progress?)` | Descarrega a variante selecionada |
| `model.LoadAsync()` | Carrega o modelo para a memória |
| `model.UnloadAsync()` | Descarrega o modelo |
| `model.SelectVariant(variant)` | Seleciona uma variante específica (CPU/GPU/NPU) |
| `model.SelectedVariant` | A variante atualmente selecionada |
| `model.Variants` | Todas as variantes disponíveis para este modelo |
| `model.GetPathAsync()` | Obtém o caminho local do ficheiro |
| `model.GetChatClientAsync()` | Obtém um cliente de chat nativo (sem necessidade do OpenAI SDK) |
| `model.GetAudioClientAsync()` | Obtém um cliente de áudio para transcrição |

</details>

---

### Exercício 4: Inspecionar Metadados do Modelo

O objeto `FoundryModelInfo` contém metadados ricos sobre cada modelo. Compreender estes campos ajuda a escolher o modelo certo para a sua aplicação.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# Obter informações detalhadas sobre um modelo específico
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

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `alias` | string | Nome curto (p.ex. `phi-3.5-mini`) |
| `id` | string | Identificador único do modelo |
| `version` | string | Versão do modelo |
| `task` | string | `chat-completions` ou `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU, ou NPU |
| `execution_provider` | string | Backend de runtime (CUDA, CPU, QNN, WebGPU, etc.) |
| `file_size_mb` | int | Tamanho em disco em MB |
| `supports_tool_calling` | bool | Indica se o modelo suporta chamada de funções/ferramentas |
| `publisher` | string | Quem publicou o modelo |
| `license` | string | Nome da licença |
| `uri` | string | URI do modelo |
| `prompt_template` | dict/null | Template do prompt, se existir |

---

### Exercício 5: Gerir o Ciclo de Vida do Modelo

Pratique o ciclo completo: listar → descarregar → carregar → usar → descarregar.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Modelo pequeno para testes rápidos

manager = FoundryLocalManager()
manager.start_service()

# 1. Verificar o que está no catálogo
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Verificar o que já está descarregado
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Descarregar um modelo
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Verificar se está agora na cache
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Carregá-lo
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Verificar o que está carregado
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Descarregá-lo
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

const alias = "qwen2.5-0.5b"; // Modelo pequeno para testes rápidos

FoundryLocalManager.create({ appName: "SDKDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 1. Obter modelo do catálogo
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.alias} (${model.id})`);
console.log(`Cached: ${model.isCached}`);

// 2. Descarregar se necessário
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Carregar
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Descarregar
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Exercício 6: Padrões de Arranque Rápido

Cada linguagem fornece um atalho para iniciar o serviço e carregar um modelo numa só chamada. Estes são os **padrões recomendados** para a maioria das aplicações.

<details>
<summary><h3>🐍 Python - Inicialização no Construtor</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Passe um alias para o construtor - ele trata de tudo:
# 1. Inicia o serviço se não estiver a correr
# 2. Descarrega o modelo se não estiver em cache
# 3. Carrega o modelo no servidor de inferência
manager = FoundryLocalManager("phi-3.5-mini")

# Pronto a usar imediatamente
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

O parâmetro `bootstrap` (por padrão `True`) controla este comportamento. Defina `bootstrap=False` se quiser controlo manual:

```python
# Modo manual - nada acontece automaticamente
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catálogo</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() tratam de tudo:
// 1. Inicia o serviço
// 2. Obtém o modelo do catálogo
// 3. Descarrega se necessário e carrega o modelo
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Pronto a usar imediatamente
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

> **Nota C#:** O SDK C# usa `Configuration` para controlar o nome da app, registo, diretórios de cache e até fixar uma porta específica do servidor web. Isto torna-o o mais configurável dos três SDKs.

</details>

---

### Exercício 7: O ChatClient Nativo (Não é Preciso o SDK OpenAI)

Os SDKs JavaScript e C# fornecem um método de conveniência `createChatClient()` que devolve um cliente de chat nativo — não é necessário instalar ou configurar o SDK OpenAI separadamente.

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

// Criar um ChatClient diretamente a partir do modelo — sem necessidade de importar OpenAI
const chatClient = model.createChatClient();

// completeChat retorna um objeto resposta compatível com OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// A transmissão utiliza um padrão de callback
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

O `ChatClient` também suporta chamadas a ferramentas — passe as ferramentas como segundo argumento:

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

> **Quando usar cada padrão:**
> - **`createChatClient()`** — Prototipagem rápida, menos dependências, código mais simples
> - **SDK OpenAI** — Controlo completo sobre parâmetros (temperatura, top_p, tokens de paragem, etc.), melhor para produção

---

### Exercício 8: Variantes de Modelos e Seleção de Hardware

Os modelos podem ter múltiplas **variantes** otimizadas para hardware diferente. O SDK seleciona a melhor variante automaticamente, mas também pode inspecionar e escolher manualmente.

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

FoundryLocalManager.create({ appName: "VariantDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");

// Liste todas as variantes disponíveis
console.log(`Model: ${model.alias}`);
console.log(`Variants: ${model.variants.length}`);
for (const variant of model.variants) {
  console.log(`  - ${variant.modelId}`);
  console.log(`    Device: ${variant.deviceType}, Provider: ${variant.executionProvider}`);
}

// O SDK seleciona automaticamente a melhor variante para o seu hardware
// Para substituir, use selectVariant():
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

No Python, o SDK seleciona automaticamente a melhor variante com base no hardware. Utilize `get_model_info()` para ver o que foi selecionado:

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

#### Modelos com Variantes NPU

Alguns modelos têm variantes otimizadas para NPU (Unidades de Processamento Neural) para dispositivos com Qualcomm Snapdragon, Intel Core Ultra:

| Modelo | Variante NPU Disponível |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Dica:** Em hardware com capacidade NPU, o SDK seleciona automaticamente a variante NPU quando disponível. Não precisa de alterar o seu código. Para projetos C# no Windows, adicione o pacote NuGet `Microsoft.AI.Foundry.Local.WinML` para ativar o provider de execução QNN — o QNN é entregue como um plugin EP via WinML.

---

### Exercício 9: Atualizações de Modelos e Atualização do Catálogo

O catálogo de modelos é atualizado periodicamente. Utilize estes métodos para verificar e aplicar atualizações.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

alias = "phi-3.5-mini"

# Atualize o catálogo para obter a lista mais recente de modelos
manager.refresh_catalog()

# Verifique se um modelo em cache tem uma versão mais recente disponível
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

// Atualizar o catálogo para obter a lista mais recente de modelos
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Listar todos os modelos disponíveis após a atualização
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Exercício 10: Trabalhar com Modelos de Raciocínio

O modelo **phi-4-mini-reasoning** inclui raciocínio em cadeia de pensamento. Ele envolve o seu pensamento interno em etiquetas `<think>...</think>` antes de produzir a resposta final. Isto é útil para tarefas que requerem lógica multi-etapa, matemática ou resolução de problemas.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
import openai
from foundry_local import FoundryLocalManager

# phi-4-mini-reasoning tem cerca de 4,6 GB
manager = FoundryLocalManager("phi-4-mini-reasoning")

client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
model_id = manager.get_model_info("phi-4-mini-reasoning").id

response = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "What is 17 × 23?"}],
)

content = response.choices[0].message.content

# O modelo envolve o seu pensamento em etiquetas <think>...</think>
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

// Analisar o raciocínio em cadeia
const thinkMatch = content.match(/<think>([\s\S]*?)<\/think>/);
if (thinkMatch) {
  console.log(`Thinking: ${thinkMatch[1].trim()}`);
  console.log(`Answer: ${content.replace(/<think>[\s\S]*?<\/think>/, "").trim()}`);
} else {
  console.log(content);
}
```

</details>

> **Quando usar modelos de raciocínio:**
> - Problemas de matemática e lógica
> - Tarefas de planeamento multi-etapa
> - Geração complexa de código
> - Tarefas onde mostrar o raciocínio melhora a precisão
>
> **Compromisso:** Modelos de raciocínio produzem mais tokens (a secção `<think>`) e são mais lentos. Para perguntas e respostas simples, um modelo standard como o phi-3.5-mini é mais rápido.

---

### Exercício 11: Entender Alias e Seleção de Hardware

Quando passar um **alias** (como `phi-3.5-mini`) em vez de um ID completo do modelo, o SDK seleciona automaticamente a melhor variante para o seu hardware:

| Hardware | Provider de Execução Selecionado |
|----------|---------------------------|
| GPU NVIDIA (CUDA) | `CUDAExecutionProvider` |
| NPU Qualcomm | `QNNExecutionProvider` (via plugin WinML) |
| NPU Intel | `OpenVINOExecutionProvider` |
| GPU AMD | `VitisAIExecutionProvider` |
| NVIDIA RTX | `NvTensorRTRTXExecutionProvider` |
| Qualquer dispositivo (fallback) | `CPUExecutionProvider` ou `WebGpuExecutionProvider` |

```python
from foundry_local import FoundryLocalManager

manager = FoundryLocalManager()
manager.start_service()

# O alias resolve para a melhor variante para O SEU hardware
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Dica:** Use sempre aliases no seu código de aplicação. Quando fizer deploy na máquina do utilizador, o SDK seleciona a variante ótima em tempo de execução — CUDA em NVIDIA, QNN em Qualcomm, CPU noutros casos.

---

### Exercício 12: Opções de Configuração C#

A classe `Configuration` do SDK C# fornece controlo detalhado sobre o runtime:

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

| Propriedade | Padrão | Descrição |
|------------|---------|-------------|
| `AppName` | (obrigatório) | Nome da sua aplicação |
| `LogLevel` | `Information` | Verbosidade do registo |
| `Web.Urls` | (dinâmico) | Fixar uma porta específica para o servidor web |
| `AppDataDir` | Padrão do SO | Diretório base dos dados da app |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Onde os modelos são armazenados |
| `LogsDir` | `{AppDataDir}/logs` | Onde os registos são escritos |

---

### Exercício 13: Uso no Browser (Só JavaScript)

O SDK JavaScript inclui uma versão compatível com browser. No browser, deve iniciar manualmente o serviço via CLI e especificar a URL do host:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Inicie o serviço manualmente primeiro:
//   foundry service start
// Depois, use o URL da saída do CLI
FoundryLocalManager.create({ appName: "BrowserDemo" });
const manager = FoundryLocalManager.instance;

// Navegue pelo catálogo
const catalog = manager.catalog;
const model = await catalog.getModel("phi-3.5-mini");

if (!model.isCached) {
  await model.download();
}
await model.load();
```

> **Limitações do browser:** A versão para browser **não** suporta `startWebService()`. Deve garantir que o serviço Foundry Local já está em execução antes de usar o SDK no browser.

---

## Referência Completa da API

### Python

| Categoria | Método | Descrição |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Criar manager; opcionalmente fazer bootstrap com um modelo |
| **Serviço** | `is_service_running()` | Verificar se o serviço está a correr |
| **Serviço** | `start_service()` | Iniciar o serviço |
| **Serviço** | `endpoint` | URL do endpoint da API |
| **Serviço** | `api_key` | Chave da API |
| **Catálogo** | `list_catalog_models()` | Listar todos os modelos disponíveis |
| **Catálogo** | `refresh_catalog()` | Atualizar o catálogo |
| **Catálogo** | `get_model_info(alias_or_model_id)` | Obter metadados do modelo |
| **Cache** | `get_cache_location()` | Caminho do diretório de cache |
| **Cache** | `list_cached_models()` | Listar modelos descarregados |
| **Modelo** | `download_model(alias_or_model_id)` | Descarregar um modelo |
| **Modelo** | `load_model(alias_or_model_id, ttl=600)` | Carregar um modelo |
| **Modelo** | `unload_model(alias_or_model_id)` | Descarregar um modelo |
| **Modelo** | `list_loaded_models()` | Listar modelos carregados |
| **Modelo** | `is_model_upgradeable(alias_or_model_id)` | Verificar se existe uma versão nova disponível |
| **Modelo** | `upgrade_model(alias_or_model_id)` | Atualizar um modelo para a versão mais recente |
| **Serviço** | `httpx_client` | Cliente HTTPX pré-configurado para chamadas diretas à API |

### JavaScript

| Categoria | Método | Descrição |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Inicializar o singleton do SDK |
| **Init** | `FoundryLocalManager.instance` | Aceder ao manager singleton |
| **Serviço** | `manager.startWebService()` | Iniciar o serviço web |
| **Serviço** | `manager.urls` | Array de URLs base para o serviço |
| **Catálogo** | `manager.catalog` | Aceder ao catálogo de modelos |
| **Catálogo** | `catalog.getModel(alias)` | Obter um objeto de modelo por alias (devolve Promise) |
| **Modelo** | `model.isCached` | Indica se o modelo está descarregado |
| **Modelo** | `model.download()` | Descarregar o modelo |
| **Modelo** | `model.load()` | Carregar o modelo |
| **Modelo** | `model.unload()` | Descarregar o modelo |
| **Modelo** | `model.id` | Identificador único do modelo |
| **Modelo** | `model.alias` | Alias do modelo |
| **Modelo** | `model.createChatClient()` | Obter um cliente de chat nativo (sem necessidade do SDK OpenAI) |
| **Modelo** | `model.createAudioClient()` | Obter um cliente de áudio para transcrição |
| **Modelo** | `model.removeFromCache()` | Remover o modelo do cache local |
| **Modelo** | `model.selectVariant(variant)` | Selecionar uma variante de hardware específica |
| **Modelo** | `model.variants` | Array com variantes disponíveis para este modelo |
| **Modelo** | `model.isLoaded()` | Verificar se o modelo está atualmente carregado |
| **Catálogo** | `catalog.getModels()` | Listar todos os modelos disponíveis |
| **Catálogo** | `catalog.getCachedModels()` | Listar modelos descarregados |
| **Catálogo** | `catalog.getLoadedModels()` | Listar modelos carregados atualmente |
| **Catálogo** | `catalog.updateModels()` | Atualizar o catálogo a partir do serviço |
| **Serviço** | `manager.stopWebService()` | Parar o serviço web Foundry Local |

### C# (v0.8.0+)

| Categoria | Método | Descrição |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Inicializar o manager |
| **Init** | `FoundryLocalManager.Instance` | Aceder ao singleton |
| **Catálogo** | `manager.GetCatalogAsync()` | Obter catálogo |
| **Catálogo** | `catalog.ListModelsAsync()` | Listar todos os modelos |
| **Catálogo** | `catalog.GetModelAsync(alias)` | Obter um modelo específico |
| **Catálogo** | `catalog.GetCachedModelsAsync()` | Listar modelos em cache |
| **Catálogo** | `catalog.GetLoadedModelsAsync()` | Listar modelos carregados |
| **Modelo** | `model.DownloadAsync(progress?)` | Descarregar um modelo |
| **Modelo** | `model.LoadAsync()` | Carregar um modelo |
| **Modelo** | `model.UnloadAsync()` | Descarregar um modelo |
| **Modelo** | `model.SelectVariant(variant)` | Escolher uma variante de hardware |
| **Modelo** | `model.GetChatClientAsync()` | Obter cliente de chat nativo |
| **Modelo** | `model.GetAudioClientAsync()` | Obter cliente de transcrição áudio |
| **Modelo** | `model.GetPathAsync()` | Obter caminho local do ficheiro |
| **Catálogo** | `catalog.GetModelVariantAsync(alias, variant)` | Obter uma variante específica de hardware |
| **Catálogo** | `catalog.UpdateModelsAsync()` | Atualizar o catálogo |
| **Servidor** | `manager.StartWebServerAsync()` | Iniciar o servidor web REST |
| **Servidor** | `manager.StopWebServerAsync()` | Parar o servidor web REST |
| **Config** | `config.ModelCacheDir` | Diretório de cache |

---

## Principais Conclusões

| Conceito | O que Aprendeu |
|---------|-----------------|
| **SDK vs CLI** | O SDK fornece controlo programático - essencial para aplicações |
| **Plano de controlo** | O SDK gere serviços, modelos e cache |
| **Portas dinâmicas** | Use sempre `manager.endpoint` (Python) ou `manager.urls[0]` (JS/C#) — nunca fixe uma porta no código |
| **Aliases** | Use aliases para seleção automática de modelo otimizado para hardware |
| **Início rápido** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Redesign C#** | v0.8.0+ é autónomo – não necessita de CLI nas máquinas dos utilizadores finais |
| **Ciclo de vida do modelo** | Catálogo → Transferir → Carregar → Usar → Descarregar |
| **FoundryModelInfo** | Metadados ricos: tarefa, dispositivo, tamanho, licença, suporte a ferramentas de chamada |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) para utilização sem OpenAI |
| **Variantes** | Os modelos têm variantes específicas de hardware (CPU, GPU, NPU); selecionadas automaticamente |
| **Atualizações** | Python: `is_model_upgradeable()` + `upgrade_model()` para manter os modelos atualizados |
| **Atualização do catálogo** | `refresh_catalog()` (Python) / `updateModels()` (JS) para descobrir novos modelos |

---

## Recursos

| Recurso | Ligação |
|----------|------|
| Referência do SDK (todas as linguagens) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integrar com SDKs de inferência | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Referência API do SDK C# | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| Exemplos do SDK C# | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Website Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Próximos passos

Continue para [Parte 3: Usar o SDK com OpenAI](part3-sdk-and-apis.md) para ligar o SDK à biblioteca cliente OpenAI e construir a sua primeira aplicação de conclusão de chat.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido utilizando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, por favor esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original, na sua língua nativa, deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se a tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes da utilização desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->