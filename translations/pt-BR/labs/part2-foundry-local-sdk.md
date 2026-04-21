![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 2: Imersão Profunda no Foundry Local SDK

> **Objetivo:** Dominar o Foundry Local SDK para gerenciar modelos, serviços e cache programaticamente - e compreender por que o SDK é a abordagem recomendada em vez da CLI para construir aplicações.

## Visão Geral

Na Parte 1 você usou o **Foundry Local CLI** para baixar e executar modelos de forma interativa. A CLI é ótima para exploração, mas quando você constrói aplicações reais, precisa de **controle programático**. O Foundry Local SDK oferece isso - ele gerencia o **plano de controle** (iniciar o serviço, descobrir modelos, baixar, carregar) para que o código da sua aplicação possa focar no **plano de dados** (enviar prompts, receber respostas).

Este laboratório ensina toda a API do SDK nas linguagens Python, JavaScript e C#. Ao final, você compreenderá cada método disponível e quando usar cada um.

## Objetivos de Aprendizagem

Ao final deste laboratório, você poderá:

- Explicar por que o SDK é preferido em relação à CLI para desenvolvimento de aplicações
- Instalar o Foundry Local SDK para Python, JavaScript ou C#
- Usar `FoundryLocalManager` para iniciar o serviço, gerenciar modelos e consultar o catálogo
- Listar, baixar, carregar e descarregar modelos programaticamente
- Inspecionar metadados de modelos usando `FoundryModelInfo`
- Entender a diferença entre catálogo, cache e modelos carregados
- Usar o bootstrap do construtor (Python) e o padrão `create()` + catálogo (JavaScript)
- Entender o redesign do SDK C# e sua API orientada a objetos

---

## Pré-requisitos

| Requisito | Detalhes |
|-------------|---------|
| **Foundry Local CLI** | Instalado e no seu `PATH` ([Parte 1](part1-getting-started.md)) |
| **Ambiente de execução** | **Python 3.9+** e/ou **Node.js 18+** e/ou **.NET 9.0+** |

---

## Conceito: SDK vs CLI - Por Que Usar o SDK?

| Aspecto | CLI (comando `foundry`) | SDK (`foundry-local-sdk`) |
|--------|-------------------------|--------------------------|
| **Caso de uso** | Exploração, testes manuais | Integração em aplicações |
| **Gerenciamento do serviço** | Manual: `foundry service start` | Automático: `manager.start_service()` (Python) / `manager.startWebService()` (JS/C#) |
| **Descoberta de porta** | Lê saída da CLI | `manager.endpoint` (Python) / `manager.urls[0]` (JS/C#) |
| **Download do modelo** | `foundry model download alias` | `manager.download_model(alias)` (Python) / `model.download()` (JS/C#) |
| **Tratamento de erros** | Códigos de saída, stderr | Exceções, erros tipados |
| **Automação** | Scripts shell | Integração nativa na linguagem |
| **Implantação** | Requer CLI na máquina do usuário final | SDK C# pode ser autocontido (sem necessidade da CLI) |

> **Insight chave:** O SDK gerencia todo o ciclo de vida: iniciar o serviço, verificar o cache, baixar modelos faltantes, carregá-los e descobrir o endpoint, com poucas linhas de código. Sua aplicação não precisa interpretar saída da CLI nem gerir subprocessos.

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
|---------|----------|-------------|
| `Microsoft.AI.Foundry.Local` | Multiplataforma | Funciona em Windows, Linux, macOS |
| `Microsoft.AI.Foundry.Local.WinML` | Apenas Windows | Adiciona aceleração de hardware WinML; baixa e instala provedores de execução do plugin (ex: QNN para Qualcomm NPU) |

**Configuração Windows:**

```bash
dotnet new console -n foundry-sdk-demo
cd foundry-sdk-demo
```

Edite o arquivo `.csproj`:

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

> **Nota:** No Windows, o pacote WinML é um superset que inclui o SDK base mais o provedor de execução QNN. No Linux/macOS, usa-se o SDK base. As configurações condicionais de TFM e referências mantêm o projeto totalmente multiplataforma.

Crie um `nuget.config` no raiz do projeto:

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

### Exercício 2: Iniciar o Serviço e Listar o Catálogo

A primeira coisa que qualquer aplicação faz é iniciar o serviço Foundry Local e descobrir quais modelos estão disponíveis.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Crie um gerente e inicie o serviço
manager = FoundryLocalManager()
manager.start_service()

# Liste todos os modelos disponíveis no catálogo
catalog = manager.list_catalog_models()
print(f"Models available in catalog: {len(catalog)}")

for model in catalog:
    print(f"  - {model.alias} ({model.id})")
    print(f"    Task: {model.task}, Size: {model.file_size_mb} MB")
    print(f"    Device: {model.device_type}, Provider: {model.publisher}")
```

#### SDK Python - Métodos de Gerenciamento de Serviço

| Método | Assinatura | Descrição |
|--------|-----------|-------------|
| `is_service_running()` | `() -> bool` | Verifica se o serviço está rodando |
| `start_service()` | `() -> None` | Inicia o serviço Foundry Local |
| `service_uri` | `@property -> str` | URI base do serviço |
| `endpoint` | `@property -> str` | Endpoint da API (URI do serviço + `/v1`) |
| `api_key` | `@property -> str` | Chave da API (do ambiente ou placeholder padrão) |

#### SDK Python - Métodos de Gerenciamento de Catálogo

| Método | Assinatura | Descrição |
|--------|-----------|-------------|
| `list_catalog_models()` | `() -> list[FoundryModelInfo]` | Lista todos os modelos no catálogo |
| `refresh_catalog()` | `() -> None` | Atualiza o catálogo a partir do serviço |
| `get_model_info()` | `(alias_or_model_id: str, raise_on_not_found=False) -> FoundryModelInfo \| None` | Obtém informações de um modelo específico |

</details>

<details>
<summary><h3>📘 JavaScript</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// Crie um gerente e inicie o serviço
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
|--------|-----------|-------------|
| `FoundryLocalManager.create()` | `(options: { appName: string }) => void` | Inicializa o singleton do SDK |
| `FoundryLocalManager.instance` | `FoundryLocalManager` | Acessa o manager singleton |
| `manager.startWebService()` | `() => Promise<void>` | Inicia o serviço web Foundry Local |
| `manager.urls` | `string[]` | Array de URLs base do serviço |

#### SDK JavaScript - Métodos do Catálogo e Modelos

| Método | Assinatura | Descrição |
|--------|-----------|-------------|
| `manager.catalog` | `Catalog` | Acessa o catálogo de modelos |
| `catalog.getModel()` | `(alias: string) => Promise<Model>` | Obtém um objeto modelo pelo alias |

</details>

<details>
<summary><h3>💜 C#</h3></summary>

O SDK C# versão 0.8.0+ usa arquitetura orientada a objetos com os objetos `Configuration`, `Catalog` e `Model`:

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
|-------|---------|
| `Configuration` | Define nome da aplicação, nível de logs, diretório de cache, URLs do servidor web |
| `FoundryLocalManager` | Ponto de entrada principal - criado via `CreateAsync()`, acessado via `.Instance` |
| `Catalog` | Navega, pesquisa e obtém modelos do catálogo |
| `Model` | Representa um modelo específico - baixar, carregar, obter clientes |

#### SDK C# - Métodos do Manager e Catálogo

| Método | Descrição |
|--------|-------------|
| `FoundryLocalManager.CreateAsync(config, logger)` | Inicializa o manager |
| `FoundryLocalManager.Instance` | Acessa o manager singleton |
| `manager.GetCatalogAsync()` | Obtém o catálogo de modelos |
| `catalog.ListModelsAsync()` | Lista todos os modelos disponíveis |
| `catalog.GetModelAsync(alias: "alias")` | Obtém um modelo específico pelo alias |
| `catalog.GetCachedModelsAsync()` | Lista modelos baixados |
| `catalog.GetLoadedModelsAsync()` | Lista modelos atualmente carregados |

> **Nota de Arquitetura C#:** O redesign do SDK C# v0.8.0+ torna a aplicação **autossuficiente**; não requer o Foundry Local CLI na máquina do usuário final. O SDK gerencia modelos e inferência nativamente.

</details>

---

### Exercício 3: Baixar e Carregar um Modelo

O SDK separa o download (para disco) do carregamento (na memória). Isso permite pré-baixar modelos durante a configuração e carregá-los sob demanda.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Opção A: Passo a passo manual
manager = FoundryLocalManager()
manager.start_service()

# Verifique o cache primeiro
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

# Opção B: Bootstrap de uma linha (recomendado)
# Passe o alias para o construtor - ele inicia o serviço, faz o download e carrega automaticamente
manager = FoundryLocalManager(alias)
print(f"Ready! Endpoint: {manager.endpoint}")
```

#### Python - Métodos de Gerenciamento de Modelos

| Método | Assinatura | Descrição |
|--------|-----------|-------------|
| `download_model()` | `(alias_or_model_id, token=None, force=False) -> FoundryModelInfo` | Baixa um modelo para o cache local |
| `load_model()` | `(alias_or_model_id, ttl=600) -> FoundryModelInfo` | Carrega um modelo no servidor de inferência |
| `unload_model()` | `(alias_or_model_id, force=False) -> None` | Descarrega um modelo do servidor |
| `list_loaded_models()` | `() -> list[FoundryModelInfo]` | Lista todos os modelos atualmente carregados |

#### Python - Métodos de Gerenciamento do Cache

| Método | Assinatura | Descrição |
|--------|-----------|-------------|
| `get_cache_location()` | `() -> str` | Obtém o caminho do diretório de cache |
| `list_cached_models()` | `() -> list[FoundryModelInfo]` | Lista todos os modelos baixados |

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

#### JavaScript - Métodos de Modelo

| Método | Assinatura | Descrição |
|--------|-----------|-------------|
| `model.isCached` | `boolean` | Se o modelo já está baixado |
| `model.download()` | `() => Promise<void>` | Baixa o modelo para cache local |
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

#### C# - Métodos de Modelo

| Método | Descrição |
|--------|-------------|
| `model.DownloadAsync(progress?)` | Baixa a variante selecionada |
| `model.LoadAsync()` | Carrega o modelo na memória |
| `model.UnloadAsync()` | Descarrega o modelo |
| `model.SelectVariant(variant)` | Seleciona uma variante específica (CPU/GPU/NPU) |
| `model.SelectedVariant` | Variante atualmente selecionada |
| `model.Variants` | Todas as variantes disponíveis para este modelo |
| `model.GetPathAsync()` | Obtém o caminho local do arquivo |
| `model.GetChatClientAsync()` | Obtém um cliente de chat nativo (sem precisar do OpenAI SDK) |
| `model.GetAudioClientAsync()` | Obtém um cliente de áudio para transcrição |

</details>

---

### Exercício 4: Inspecionar Metadados do Modelo

O objeto `FoundryModelInfo` contém metadados ricos sobre cada modelo. Entender esses campos ajuda a escolher o modelo certo para sua aplicação.

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

#### Campos do FoundryModelInfo

| Campo | Tipo | Descrição |
|-------|------|-------------|
| `alias` | string | Nome curto (ex: `phi-3.5-mini`) |
| `id` | string | Identificador único do modelo |
| `version` | string | Versão do modelo |
| `task` | string | `chat-completions` ou `automatic-speech-recognition` |
| `device_type` | DeviceType | CPU, GPU ou NPU |
| `execution_provider` | string | Backend de execução (CUDA, CPU, QNN, WebGPU, etc.) |
| `file_size_mb` | int | Tamanho no disco em MB |
| `supports_tool_calling` | bool | Se o modelo suporta chamadas de função/ferramenta |
| `publisher` | string | Quem publicou o modelo |
| `license` | string | Nome da licença |
| `uri` | string | URI do modelo |
| `prompt_template` | dict/null | Template de prompt, se houver |

---

### Exercício 5: Gerenciar o Ciclo de Vida do Modelo

Pratique o ciclo completo: listar → baixar → carregar → usar → descarregar.

<details>
<summary><h3>🐍 Python</h3></summary>

```python
from foundry_local import FoundryLocalManager

alias = "qwen2.5-0.5b"  # Modelo pequeno para testes rápidos

manager = FoundryLocalManager()
manager.start_service()

# 1. Verifique o que está no catálogo
catalog = manager.list_catalog_models()
print(f"Catalog: {len(catalog)} models")

# 2. Verifique o que já foi baixado
cached = manager.list_cached_models()
print(f"Cached: {len(cached)} models")
for m in cached:
    print(f"  - {m.alias} ({m.file_size_mb} MB)")

# 3. Baixe um modelo
print(f"\nDownloading {alias}...")
manager.download_model(alias)
print("Download complete")

# 4. Verifique se está no cache agora
cached = manager.list_cached_models()
print(f"Cached after download: {len(cached)} models")

# 5. Carregue-o
print(f"\nLoading {alias}...")
loaded_info = manager.load_model(alias)
print(f"Loaded: {loaded_info.id}")

# 6. Verifique o que está carregado
loaded = manager.list_loaded_models()
print(f"\nLoaded models: {len(loaded)}")
for m in loaded:
    print(f"  - {m.alias} ({m.id})")

# 7. Descarregue-o
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

// 2. Baixar se necessário
if (!model.isCached) {
  console.log(`\nDownloading ${alias}...`);
  await model.download();
  console.log("Download complete");
}

// 3. Carregá-lo
console.log(`\nLoading ${alias}...`);
await model.load();
console.log(`Loaded: ${model.id}`);

// 4. Descarregá-lo
console.log(`\nUnloading ${alias}...`);
await model.unload();
console.log("Unloaded");
```

</details>

---

### Exercício 6: Os Padrões de Inicialização Rápida

Cada linguagem fornece um atalho para iniciar o serviço e carregar um modelo em uma única chamada. Estes são os **padrões recomendados** para a maioria das aplicações.

<details>
<summary><h3>🐍 Python - Inicialização pelo Construtor</h3></summary>

```python
from foundry_local import FoundryLocalManager

# Passe um alias para o construtor - ele cuida de tudo:
# 1. Inicia o serviço se não estiver em execução
# 2. Baixa o modelo se não estiver em cache
# 3. Carrega o modelo no servidor de inferência
manager = FoundryLocalManager("phi-3.5-mini")

# Pronto para usar imediatamente
print(f"Endpoint: {manager.endpoint}")
print(f"Model ID: {manager.get_model_info('phi-3.5-mini').id}")
```

O parâmetro `bootstrap` (padrão `True`) controla esse comportamento. Defina `bootstrap=False` se quiser controle manual:

```python
# Modo manual - nada acontece automaticamente
manager = FoundryLocalManager(bootstrap=False)
```

</details>

<details>
<summary><h3>📘 JavaScript - `create()` + Catálogo</h3></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// create() + startWebService() + catalog.getModel() lida com tudo:
// 1. Inicia o serviço
// 2. Obtém o modelo do catálogo
// 3. Baixa se necessário e carrega o modelo
FoundryLocalManager.create({ appName: "QuickStart" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel("phi-3.5-mini");
if (!model.isCached) await model.download();
await model.load();

// Pronto para usar imediatamente
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

> **Nota C#:** O SDK C# usa `Configuration` para controlar nome do app, logging, diretórios de cache e até fixar uma porta específica do servidor web. Isso o torna o mais configurável dos três SDKs.

</details>

---

### Exercício 7: O ChatClient Nativo (Sem Necessidade do SDK OpenAI)

Os SDKs JavaScript e C# fornecem um método de conveniência `createChatClient()` que retorna um cliente de chat nativo — não há necessidade de instalar ou configurar o SDK OpenAI separadamente.

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

// Crie um ChatClient diretamente do modelo — nenhuma importação do OpenAI é necessária
const chatClient = model.createChatClient();

// completeChat retorna um objeto de resposta compatível com OpenAI
const response = await chatClient.completeChat([
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// A transmissão usa um padrão de callback
await chatClient.completeStreamingChat(
  [{ role: "user", content: "Explain quantum computing briefly." }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
```

O `ChatClient` também suporta chamada de ferramentas — passe as ferramentas como segundo argumento:

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

> **Quando usar qual padrão:**
> - **`createChatClient()`** — Protótipos rápidos, menos dependências, código mais simples
> - **SDK OpenAI** — Controle completo sobre parâmetros (temperatura, top_p, tokens de parada, etc.), melhor para aplicações de produção

---

### Exercício 8: Variantes de Modelos e Seleção de Hardware

Modelos podem ter múltiplas **variantes** otimizadas para diferentes hardwares. O SDK seleciona a melhor variante automaticamente, mas você também pode inspecionar e escolher manualmente.

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

// O SDK seleciona automaticamente a melhor variante para seu hardware
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

Em Python, o SDK seleciona automaticamente a melhor variante com base no hardware. Use `get_model_info()` para ver o que foi selecionado:

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

Alguns modelos têm variantes otimizadas para NPU (Unidades de Processamento Neural) em dispositivos (Qualcomm Snapdragon, Intel Core Ultra):

| Modelo | Variante NPU Disponível |
|-------|:---:|
| phi-3.5-mini | ✅ |
| phi-3-mini-128k | ✅ |
| phi-3-mini-4k | ✅ |
| deepseek-r1-14b | ✅ |
| deepseek-r1-7b | ✅ |
| qwen2.5-1.5b | ✅ |
| qwen2.5-7b | ✅ |

> **Dica:** Em hardware com suporte a NPU, o SDK seleciona automaticamente a variante NPU quando disponível. Você não precisa alterar seu código. Para projetos C# no Windows, adicione o pacote NuGet `Microsoft.AI.Foundry.Local.WinML` para habilitar o provedor de execução QNN — QNN é entregue como um plugin EP via WinML.

---

### Exercício 9: Atualizações de Modelos e Atualização do Catálogo

O catálogo de modelos é atualizado periodicamente. Use estes métodos para verificar e aplicar atualizações.

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

// Atualize o catálogo para buscar a lista mais recente de modelos
await manager.catalog.updateModels();
console.log("Catalog refreshed");

// Liste todos os modelos disponíveis após a atualização
const models = await manager.catalog.getModels();
console.log(`${models.length} models available`);
```

</details>

---

### Exercício 10: Trabalhando com Modelos de Raciocínio

O modelo **phi-4-mini-reasoning** inclui raciocínio em cadeia de pensamento. Ele envolve seu pensamento interno com tags `<think>...</think>` antes de produzir a resposta final. Isso é útil para tarefas que exigem lógica em múltiplas etapas, matemática ou resolução de problemas.

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

# O modelo envolve seu raciocínio em tags <think>...</think>
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

// Analisar pensamento em cadeia
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
> - Tarefas de planejamento em múltiplas etapas
> - Geração complexa de código
> - Tarefas onde mostrar o raciocínio melhora a precisão
>
> **Compromisso:** Modelos de raciocínio geram mais tokens (a seção `<think>`) e são mais lentos. Para perguntas e respostas simples, um modelo padrão como phi-3.5-mini é mais rápido.

---

### Exercício 11: Entendendo Aliases e Seleção de Hardware

Quando você passa um **alias** (como `phi-3.5-mini`) em vez do ID completo do modelo, o SDK seleciona automaticamente a melhor variante para seu hardware:

| Hardware | Provedor de Execução Selecionado |
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

# O alias resolve para a melhor variante para SEU hardware
info = manager.get_model_info("phi-3.5-mini")
print(f"Selected variant: {info.id}")
print(f"Execution provider: {info.execution_provider}")
print(f"Device type: {info.device_type}")
```

> **Dica:** Sempre use aliases em seu código de aplicativo. Quando você implantar na máquina do usuário, o SDK escolhe a variante ótima em tempo de execução - CUDA em NVIDIA, QNN na Qualcomm, CPU no restante.

---

### Exercício 12: Opções de Configuração C#

A classe `Configuration` do SDK C# fornece controle detalhado sobre o runtime:

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
|----------|---------|-------------|
| `AppName` | (obrigatório) | Nome do seu aplicativo |
| `LogLevel` | `Information` | Verbosidade do log |
| `Web.Urls` | (dinâmico) | Fixar uma porta específica para o servidor web |
| `AppDataDir` | Padrão do SO | Diretório base para dados do app |
| `ModelCacheDir` | `{AppDataDir}/model_cache` | Onde os modelos são armazenados |
| `LogsDir` | `{AppDataDir}/logs` | Onde os logs são escritos |

---

### Exercício 13: Uso no Navegador (Apenas JavaScript)

O SDK JavaScript inclui uma versão compatível com navegador. No navegador, você deve iniciar manualmente o serviço via CLI e especificar a URL do host:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk/browser";

// Inicie o serviço manualmente primeiro:
//   foundry service start
// Em seguida, use a URL da saída do CLI
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

> **Limitações do navegador:** A versão para navegador **não** suporta `startWebService()`. Você deve garantir que o serviço Foundry Local já esteja em execução antes de usar o SDK no navegador.

---

## Referência Completa da API

### Python

| Categoria | Método | Descrição |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager(alias?, bootstrap=True)` | Cria o gerente; opcionalmente inicializa com um modelo |
| **Service** | `is_service_running()` | Verifica se o serviço está rodando |
| **Service** | `start_service()` | Inicia o serviço |
| **Service** | `endpoint` | URL do endpoint da API |
| **Service** | `api_key` | Chave API |
| **Catalog** | `list_catalog_models()` | Lista todos os modelos disponíveis |
| **Catalog** | `refresh_catalog()` | Atualiza o catálogo |
| **Catalog** | `get_model_info(alias_or_model_id)` | Obtém metadados do modelo |
| **Cache** | `get_cache_location()` | Caminho do diretório de cache |
| **Cache** | `list_cached_models()` | Lista modelos baixados |
| **Model** | `download_model(alias_or_model_id)` | Baixa um modelo |
| **Model** | `load_model(alias_or_model_id, ttl=600)` | Carrega um modelo |
| **Model** | `unload_model(alias_or_model_id)` | Descarrega um modelo |
| **Model** | `list_loaded_models()` | Lista modelos carregados |
| **Model** | `is_model_upgradeable(alias_or_model_id)` | Verifica se há versão mais recente |
| **Model** | `upgrade_model(alias_or_model_id)` | Atualiza o modelo para a versão mais recente |
| **Service** | `httpx_client` | Cliente HTTPX pré-configurado para chamadas diretas da API |

### JavaScript

| Categoria | Método | Descrição |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.create(options)` | Inicializa a instância singleton do SDK |
| **Init** | `FoundryLocalManager.instance` | Acessa o gerenciador singleton |
| **Service** | `manager.startWebService()` | Inicia o serviço web |
| **Service** | `manager.urls` | Array das URLs base do serviço |
| **Catalog** | `manager.catalog` | Acessa o catálogo de modelos |
| **Catalog** | `catalog.getModel(alias)` | Obtém um objeto de modelo pelo alias (retorna Promise) |
| **Model** | `model.isCached` | Se o modelo está baixado |
| **Model** | `model.download()` | Baixa o modelo |
| **Model** | `model.load()` | Carrega o modelo |
| **Model** | `model.unload()` | Descarrega o modelo |
| **Model** | `model.id` | Identificador único do modelo |
| **Model** | `model.alias` | Alias do modelo |
| **Model** | `model.createChatClient()` | Obtém um cliente de chat nativo (sem necessidade do SDK OpenAI) |
| **Model** | `model.createAudioClient()` | Obtém um cliente de áudio para transcrição |
| **Model** | `model.removeFromCache()` | Remove o modelo do cache local |
| **Model** | `model.selectVariant(variant)` | Seleciona uma variante de hardware específica |
| **Model** | `model.variants` | Array de variantes disponíveis para este modelo |
| **Model** | `model.isLoaded()` | Verifica se o modelo está carregado |
| **Catalog** | `catalog.getModels()` | Lista todos os modelos disponíveis |
| **Catalog** | `catalog.getCachedModels()` | Lista modelos baixados |
| **Catalog** | `catalog.getLoadedModels()` | Lista modelos carregados atualmente |
| **Catalog** | `catalog.updateModels()` | Atualiza o catálogo a partir do serviço |
| **Service** | `manager.stopWebService()` | Para o serviço web Foundry Local |

### C# (v0.8.0+)

| Categoria | Método | Descrição |
|----------|--------|-------------|
| **Init** | `FoundryLocalManager.CreateAsync(config, logger)` | Inicializa o gerenciador |
| **Init** | `FoundryLocalManager.Instance` | Acessa a instância singleton |
| **Catalog** | `manager.GetCatalogAsync()` | Obtém o catálogo |
| **Catalog** | `catalog.ListModelsAsync()` | Lista todos os modelos |
| **Catalog** | `catalog.GetModelAsync(alias)` | Obtém um modelo específico |
| **Catalog** | `catalog.GetCachedModelsAsync()` | Lista modelos em cache |
| **Catalog** | `catalog.GetLoadedModelsAsync()` | Lista modelos carregados |
| **Model** | `model.DownloadAsync(progress?)` | Baixa um modelo |
| **Model** | `model.LoadAsync()` | Carrega um modelo |
| **Model** | `model.UnloadAsync()` | Descarrega um modelo |
| **Model** | `model.SelectVariant(variant)` | Escolhe uma variante de hardware |
| **Model** | `model.GetChatClientAsync()` | Obtém cliente de chat nativo |
| **Model** | `model.GetAudioClientAsync()` | Obtém cliente de transcrição de áudio |
| **Model** | `model.GetPathAsync()` | Obtém caminho local do arquivo |
| **Catalog** | `catalog.GetModelVariantAsync(alias, variant)` | Obtém uma variante de hardware específica |
| **Catalog** | `catalog.UpdateModelsAsync()` | Atualiza o catálogo |
| **Server** | `manager.StartWebServerAsync()` | Inicia o servidor web REST |
| **Server** | `manager.StopWebServerAsync()` | Para o servidor web REST |
| **Config** | `config.ModelCacheDir` | Diretório de cache |

---

## Principais Lições

| Conceito | O Que Você Aprendeu |
|---------|-----------------|
| **SDK vs CLI** | O SDK fornece controle programático - essencial para aplicativos |
| **Plano de controle** | O SDK gerencia serviços, modelos e caching |
| **Portas dinâmicas** | Sempre use `manager.endpoint` (Python) ou `manager.urls[0]` (JS/C#) - nunca codifique a porta |
| **Aliases** | Use aliases para seleção automática do modelo otimizado para hardware |
| **Início rápido** | Python: `FoundryLocalManager(alias)`, JS: `FoundryLocalManager.create()` + `await catalog.getModel(alias)` |
| **Redesign C#** | v0.8.0+ é autônomo - não requer CLI nas máquinas dos usuários finais |
| **Ciclo de vida do modelo** | Catálogo → Download → Carregar → Usar → Descarregar |
| **FoundryModelInfo** | Metadados ricos: tarefa, dispositivo, tamanho, licença, suporte a chamada de ferramenta |
| **ChatClient** | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) para uso gratuito do OpenAI |
| **Variantes** | Modelos possuem variantes específicas para hardware (CPU, GPU, NPU); selecionadas automaticamente |
| **Atualizações** | Python: `is_model_upgradeable()` + `upgrade_model()` para manter modelos atualizados |
| **Atualização do catálogo** | `refresh_catalog()` (Python) / `updateModels()` (JS) para descobrir novos modelos |

---

## Recursos

| Recurso | Link |
|----------|------|
| Referência SDK (todas as linguagens) | [Microsoft Learn - Foundry Local SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Integração com SDKs de inferência | [Microsoft Learn - Inference SDK Integration](https://learn.microsoft.com/en-us/azure/foundry-local/how-to/how-to-integrate-with-inference-sdks) |
| Referência da API C# SDK | [Foundry Local C# API Reference](https://aka.ms/fl-csharp-api-ref) |
| Exemplos C# SDK | [GitHub - Foundry Local SDK Samples](https://aka.ms/foundrylocalSDK) |
| Site Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Próximos passos

Continue para [Parte 3: Usando o SDK com OpenAI](part3-sdk-and-apis.md) para conectar o SDK à biblioteca cliente do OpenAI e construir seu primeiro aplicativo de conclusão de chat.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:
Este documento foi traduzido usando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se tradução profissional humana. Não nos responsabilizamos por quaisquer equívocos ou interpretações errôneas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->