![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 3: Usando o Foundry Local SDK com OpenAI

## Visão Geral

Na Parte 1 você usou o Foundry Local CLI para executar modelos interativamente. Na Parte 2 você explorou toda a superfície da API do SDK. Agora você aprenderá a **integrar o Foundry Local em suas aplicações** usando o SDK e a API compatível com OpenAI.

O Foundry Local fornece SDKs para três linguagens. Escolha a que você se sentir mais confortável - os conceitos são idênticos nas três.

## Objetivos de Aprendizagem

Ao final deste laboratório você será capaz de:

- Instalar o Foundry Local SDK para sua linguagem (Python, JavaScript ou C#)
- Inicializar `FoundryLocalManager` para iniciar o serviço, verificar o cache, baixar e carregar um modelo
- Conectar ao modelo local usando o SDK OpenAI
- Enviar chat completions e lidar com respostas em streaming
- Entender a arquitetura de porta dinâmica

---

## Pré-requisitos

Complete primeiro [Parte 1: Começando com Foundry Local](part1-getting-started.md) e [Parte 2: Explorando o Foundry Local SDK](part2-foundry-local-sdk.md).

Instale **um** dos seguintes runtimes de linguagem:
- **Python 3.9+** - [python.org/downloads](https://www.python.org/downloads/)
- **Node.js 18+** - [nodejs.org](https://nodejs.org/)
- **.NET 9.0+** - [dot.net/download](https://dotnet.microsoft.com/download)

---

## Conceito: Como o SDK Funciona

O Foundry Local SDK gerencia o **plano de controle** (iniciar o serviço, baixar modelos), enquanto o SDK OpenAI lida com o **plano de dados** (enviar prompts, receber completions).

![SDK Architecture](../../../translated_images/pt-BR/part3-sdk-architecture.9925e51f20a277e5.webp)

---

## Exercícios do Lab

### Exercício 1: Configuração do Ambiente

<details>
<summary><b>🐍 Python</b></summary>

```bash
cd python
python -m venv venv

# Ative o ambiente virtual:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# Windows (Prompt de Comando):
venv\Scripts\activate.bat
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

O `requirements.txt` instala:
- `foundry-local-sdk` - O Foundry Local SDK (importado como `foundry_local`)
- `openai` - O SDK Python da OpenAI
- `agent-framework` - Microsoft Agent Framework (usado em partes posteriores)

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

```bash
cd javascript
npm install
```

O `package.json` instala:
- `foundry-local-sdk` - O Foundry Local SDK
- `openai` - O SDK OpenAI para Node.js

</details>

<details>
<summary><b>💜 C#</b></summary>

```bash
cd csharp
dotnet restore
dotnet build
```

O `csharp.csproj` usa:
- `Microsoft.AI.Foundry.Local` - O Foundry Local SDK (NuGet)
- `OpenAI` - O SDK OpenAI para C# (NuGet)

> **Estrutura do projeto:** O projeto C# usa um roteador de linha de comando em `Program.cs` que despacha para arquivos de exemplo separados. Execute `dotnet run chat` (ou apenas `dotnet run`) para esta parte. Outras partes usam `dotnet run rag`, `dotnet run agent` e `dotnet run multi`.

</details>

---

### Exercício 2: Chat Completion Básico

Abra o exemplo básico de chat para sua linguagem e examine o código. Cada script segue o mesmo padrão de três passos:

1. **Iniciar o serviço** - `FoundryLocalManager` inicia o runtime Foundry Local
2. **Baixar e carregar o modelo** - verificar o cache, baixar se necessário e carregar na memória
3. **Criar um cliente OpenAI** - conectar ao endpoint local e enviar um chat completion em streaming

<details>
<summary><b>🐍 Python - <code>python/foundry-local.py</code></b></summary>

```python
import sys
import openai
from foundry_local import FoundryLocalManager

alias = "phi-3.5-mini"

# Passo 1: Crie um FoundryLocalManager e inicie o serviço
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Passo 2: Verifique se o modelo já está baixado
cached = manager.list_cached_models()
catalog_info = manager.get_model_info(alias)
is_cached = any(m.id == catalog_info.id for m in cached) if catalog_info else False

if is_cached:
    print(f"Model already downloaded: {alias}")
else:
    print(f"Downloading model: {alias} (this may take several minutes)...")
    manager.download_model(alias)
    print(f"Download complete: {alias}")

# Passo 3: Carregue o modelo na memória
print(f"Loading model: {alias}...")
manager.load_model(alias)

# Crie um cliente OpenAI apontando para o serviço Foundry LOCAL
client = openai.OpenAI(
    base_url=manager.endpoint,   # Porta dinâmica - nunca codifique fixamente!
    api_key=manager.api_key
)

# Gere uma conclusão de chat em streaming
stream = client.chat.completions.create(
    model=manager.get_model_info(alias).id,
    messages=[{"role": "user", "content": "What is the golden ratio?"}],
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

**Execute:**
```bash
python foundry-local.py
```

</details>

<details>
<summary><b>📘 JavaScript - <code>javascript/foundry-local.mjs</code></b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

// Passo 1: Inicie o serviço Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "FoundryLocalWorkshop" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Passo 2: Verifique se o modelo já está baixado
const catalog = manager.catalog;
const model = await catalog.getModel(alias);

if (model.isCached) {
  console.log(`Model already downloaded: ${alias}`);
} else {
  console.log(`Downloading model: ${alias} (this may take several minutes)...`);
  await model.download();
  console.log(`Download complete: ${alias}`);
}

// Passo 3: Carregue o modelo na memória
console.log(`Loading model: ${alias}...`);
await model.load();
console.log(`Model loaded: ${model.id}`);

// Crie um cliente OpenAI apontando para o serviço Foundry LOCAL
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",   // Porta dinâmica - nunca codifique de forma fixa!
  apiKey: "foundry-local",
});

// Gere uma conclusão de chat em streaming
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "What is the golden ratio?" }],
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

**Execute:**
```bash
node foundry-local.mjs
```

</details>

<details>
<summary><b>💜 C# - <code>csharp/BasicChat.cs</code></b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var alias = "phi-3.5-mini";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);

// Step 3: Check if the model is already downloaded
var isCached = await model.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine($"Model already downloaded: {alias}");
}
else
{
    Console.WriteLine($"Downloading model: {alias} (this may take several minutes)...");
    await model.DownloadAsync(null, default);
    Console.WriteLine($"Download complete: {alias}");
}

// Step 4: Load the model into memory
Console.WriteLine($"Loading model: {alias}...");
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");
Console.WriteLine($"Endpoint: {manager.Urls[0]}");

// Create OpenAI client pointing to the LOCAL Foundry service
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls[0] + "/v1")  // Dynamic port - never hardcode!
});

var chatClient = client.GetChatClient(model.Id);

// Stream a chat completion
var completionUpdates = chatClient.CompleteChatStreaming("What is the golden ratio?");

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

**Execute:**
```bash
dotnet run chat
```

</details>

---

### Exercício 3: Experimente com Prompts

Quando seu exemplo básico rodar, tente modificar o código:

1. **Altere a mensagem do usuário** - experimente diferentes perguntas
2. **Adicione um prompt de sistema** - dê uma persona ao modelo
3. **Desative o streaming** - defina `stream=False` e imprima a resposta completa de uma vez só
4. **Tente outro modelo** - troque o alias de `phi-3.5-mini` para outro modelo da lista com `foundry model list`

<details>
<summary><b>🐍 Python</b></summary>

```python
# Adicione um prompt do sistema - dê ao modelo uma persona:
stream = client.chat.completions.create(
    model=manager.get_model_info(alias).id,
    messages=[
        {"role": "system", "content": "You are a pirate. Answer everything in pirate speak."},
        {"role": "user", "content": "What is the golden ratio?"}
    ],
    stream=True,
)

# Ou desligue a transmissão:
response = client.chat.completions.create(
    model=manager.get_model_info(alias).id,
    messages=[{"role": "user", "content": "What is the golden ratio?"}],
    stream=False,
)
print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

```javascript
// Adicione um prompt de sistema - dê ao modelo uma persona:
const stream = await client.chat.completions.create({
  model: modelInfo.id,
  messages: [
    { role: "system", content: "You are a pirate. Answer everything in pirate speak." },
    { role: "user", content: "What is the golden ratio?" },
  ],
  stream: true,
});

// Ou desligue o streaming:
const response = await client.chat.completions.create({
  model: modelInfo.id,
  messages: [{ role: "user", content: "What is the golden ratio?" }],
  stream: false,
});
console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>💜 C#</b></summary>

```csharp
// Add a system prompt - give the model a persona:
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a pirate. Answer everything in pirate speak."),
        new UserChatMessage("What is the golden ratio?")
    }
);

// Or turn off streaming:
var response = chatClient.CompleteChat("What is the golden ratio?");
Console.WriteLine(response.Value.Content[0].Text);
```

</details>

---

### Referência de Métodos do SDK

<details>
<summary><b>🐍 Métodos do SDK Python</b></summary>

| Método | Propósito |
|--------|-----------|
| `FoundryLocalManager()` | Cria uma instância do gerenciador |
| `manager.start_service()` | Inicia o serviço Foundry Local |
| `manager.list_cached_models()` | Lista modelos baixados no dispositivo |
| `manager.get_model_info(alias)` | Obtém ID e metadados do modelo |
| `manager.download_model(alias, progress_callback=fn)` | Baixa modelo com callback opcional para progresso |
| `manager.load_model(alias)` | Carrega o modelo na memória |
| `manager.endpoint` | Obtém a URL do endpoint dinâmico |
| `manager.api_key` | Obtém a chave API (placeholder para local) |

</details>

<details>
<summary><b>📘 Métodos do SDK JavaScript</b></summary>

| Método | Propósito |
|--------|-----------|
| `FoundryLocalManager.create({ appName })` | Cria instância do gerenciador |
| `FoundryLocalManager.instance` | Acessa o gerenciador singleton |
| `await manager.startWebService()` | Inicia o serviço Foundry Local |
| `await manager.catalog.getModel(alias)` | Obtém modelo do catálogo |
| `model.isCached` | Verifica se o modelo já está baixado |
| `await model.download()` | Baixa um modelo |
| `await model.load()` | Carrega o modelo na memória |
| `model.id` | Obtém o ID do modelo para chamadas OpenAI API |
| `manager.urls[0] + "/v1"` | Obtém a URL do endpoint dinâmico |
| `"foundry-local"` | Chave API (placeholder para local) |

</details>

<details>
<summary><b>💜 Métodos do SDK C#</b></summary>

| Método | Propósito |
|--------|-----------|
| `FoundryLocalManager.CreateAsync(config)` | Cria e inicializa o gerenciador |
| `manager.StartWebServiceAsync()` | Inicia o serviço web Foundry Local |
| `manager.GetCatalogAsync()` | Obtém o catálogo de modelos |
| `catalog.ListModelsAsync()` | Lista todos os modelos disponíveis |
| `catalog.GetModelAsync(alias)` | Obtém modelo específico pelo alias |
| `model.IsCachedAsync()` | Verifica se o modelo está baixado |
| `model.DownloadAsync()` | Baixa um modelo |
| `model.LoadAsync()` | Carrega o modelo na memória |
| `manager.Urls[0]` | Obtém a URL do endpoint dinâmico |
| `new ApiKeyCredential("foundry-local")` | Credencial da chave API para local |

</details>

---

### Exercício 4: Usando o ChatClient Nativo (Alternativa ao SDK OpenAI)

Nos Exercícios 2 e 3 você usou o SDK OpenAI para chat completions. Os SDKs JavaScript e C# também fornecem um **ChatClient nativo** que elimina a necessidade do SDK OpenAI completamente.

<details>
<summary><b>📘 JavaScript - <code>model.createChatClient()</code></b></summary>

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "phi-3.5-mini";

FoundryLocalManager.create({ appName: "ChatClientDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const model = await manager.catalog.getModel(alias);
if (!model.isCached) await model.download();
await model.load();

// Não é necessário importar OpenAI — obtenha um cliente diretamente do modelo
const chatClient = model.createChatClient();

// Completação não streaming
const response = await chatClient.completeChat([
  { role: "system", content: "You are a pirate. Answer everything in pirate speak." },
  { role: "user", content: "What is the golden ratio?" }
]);
console.log(response.choices[0].message.content);

// Completação streaming (usa um padrão de callback)
await chatClient.completeStreamingChat(
  [{ role: "user", content: "What is the golden ratio?" }],
  (chunk) => {
    if (chunk.choices?.[0]?.delta?.content) {
      process.stdout.write(chunk.choices[0].delta.content);
    }
  }
);
console.log();
```

> **Nota:** O método `completeStreamingChat()` do ChatClient usa um padrão de **callback**, não um iterador async. Passe uma função como segundo argumento.

</details>

<details>
<summary><b>💜 C# - <code>model.GetChatClientAsync()</code></b></summary>

```csharp
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("phi-3.5-mini", default);
if (!await model.IsCachedAsync(default))
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);

// No OpenAI NuGet needed — get a client directly from the model
var chatClient = await model.GetChatClientAsync(default);

// Use it like a standard OpenAI ChatClient
var response = chatClient.CompleteChat("What is the golden ratio?");
Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **Quando usar qual:**
> | Abordagem | Melhor para |
> |----------|--------------|
> | SDK OpenAI | Controle completo de parâmetros, apps em produção, código OpenAI existente |
> | ChatClient Nativo | Protótipos rápidos, menos dependências, configuração simples |

---

## Principais Lições

| Conceito | O que Você Aprendeu |
|----------|---------------------|
| Plano de controle | O Foundry Local SDK gerencia iniciar o serviço e carregar modelos |
| Plano de dados | O SDK OpenAI gerencia chat completions e streaming |
| Portas dinâmicas | Sempre use o SDK para descobrir o endpoint; nunca codifique URLs diretamente |
| Multilíngue | O mesmo padrão de código funciona em Python, JavaScript e C# |
| Compatibilidade OpenAI | Compatibilidade total com API OpenAI significa que códigos OpenAI existentes funcionam com poucas alterações |
| ChatClient nativo | `createChatClient()` (JS) / `GetChatClientAsync()` (C#) oferecem alternativa ao SDK OpenAI |

---

## Próximos Passos

Continue para [Parte 4: Construindo uma Aplicação RAG](part4-rag-fundamentals.md) para aprender a construir uma pipeline de Retrieval-Augmented Generation rodando inteiramente no seu dispositivo.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido utilizando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações errôneas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->