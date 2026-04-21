![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 5: Construindo Agentes de IA com o Agent Framework

> **Objetivo:** Construa seu primeiro agente de IA com instruções persistentes e uma persona definida, alimentado por um modelo local através do Foundry Local.

## O Que É um Agente de IA?

Um agente de IA envolve um modelo de linguagem com **instruções do sistema** que definem seu comportamento, personalidade e restrições. Diferente de uma única chamada de conclusão de chat, um agente fornece:

- **Persona** - uma identidade consistente ("Você é um revisor de código prestativo")
- **Memória** - histórico de conversas nas interações
- **Especialização** - comportamento focado conduzido por instruções bem elaboradas

![ChatAgent Pattern](../../../translated_images/pt-BR/part5-agent-pattern.36289d1421169525.webp)

---

## O Microsoft Agent Framework

O **Microsoft Agent Framework** (AGF) oferece uma abstração padrão de agente que funciona com diferentes backends de modelo. Neste workshop, o combinamos com o Foundry Local para que tudo funcione em sua máquina - sem necessidade de nuvem.

| Conceito | Descrição |
|---------|-------------|
| `FoundryLocalClient` | Python: gerencia o início do serviço, download/carregamento do modelo e cria agentes |
| `client.as_agent()` | Python: cria um agente a partir do cliente Foundry Local |
| `AsAIAgent()` | C#: método de extensão em `ChatClient` - cria um `AIAgent` |
| `instructions` | Prompt do sistema que molda o comportamento do agente |
| `name` | Rótulo legível, útil em cenários com múltiplos agentes |
| `agent.run(prompt)` / `RunAsync()` | Envia uma mensagem de usuário e retorna a resposta do agente |

> **Nota:** O Agent Framework possui SDKs para Python e .NET. Para JavaScript, implementamos uma classe leve `ChatAgent` que espelha o mesmo padrão usando diretamente o SDK OpenAI.

---

## Exercícios

### Exercício 1 - Entenda o Padrão do Agente

Antes de escrever código, estude os componentes principais de um agente:

1. **Cliente do modelo** - conecta-se à API compatível com OpenAI do Foundry Local
2. **Instruções do sistema** - o prompt da "personalidade"
3. **Loop de execução** - envia entrada do usuário, recebe saída

> **Pense nisso:** Como as instruções do sistema diferem de uma mensagem regular do usuário? O que acontece se você as modificar?

---

### Exercício 2 - Execute o Exemplo de Agente Único

<details>
<summary><strong>🐍 Python</strong></summary>

**Pré-requisitos:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install -r requirements.txt
```

**Executar:**
```bash
python foundry-local-with-agf.py
```

**Passeio pelo código** (`python/foundry-local-with-agf.py`):

```python
import asyncio
from agent_framework_foundry_local import FoundryLocalClient

async def main():
    alias = "phi-4-mini"

    # FoundryLocalClient gerencia o início do serviço, download do modelo e carregamento
    client = FoundryLocalClient(model_id=alias)
    print(f"Client Model ID: {client.model_id}")

    # Crie um agente com instruções do sistema
    agent = client.as_agent(
        name="Joker",
        instructions="You are good at telling jokes.",
    )

    # Não transmissível: obtenha a resposta completa de uma só vez
    result = await agent.run("Tell me a joke about a pirate.")
    print(f"Agent: {result}")

    # Transmissível: obtenha os resultados conforme são gerados
    async for chunk in agent.run("Tell me another joke.", stream=True):
        if chunk.text:
            print(chunk.text, end="", flush=True)

asyncio.run(main())
```

**Pontos importantes:**
- `FoundryLocalClient(model_id=alias)` gerencia início do serviço, download e carregamento do modelo em uma única etapa
- `client.as_agent()` cria um agente com instruções do sistema e um nome
- `agent.run()` suporta modos não-streaming e streaming
- Instale via `pip install agent-framework-foundry-local --pre`

</details>

<details>
<summary><strong>📦 JavaScript</strong></summary>

**Pré-requisitos:**
```bash
cd javascript
npm install
```

**Executar:**
```bash
node foundry-local-with-agent.mjs
```

**Passeio pelo código** (`javascript/foundry-local-with-agent.mjs`):

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

class ChatAgent {
  constructor({ client, modelId, instructions, name }) {
    this.client = client;
    this.modelId = modelId;
    this.instructions = instructions;
    this.name = name;
    this.history = [];
  }

  async run(userMessage) {
    const messages = [
      { role: "system", content: this.instructions },
      ...this.history,
      { role: "user", content: userMessage },
    ];
    const response = await this.client.chat.completions.create({
      model: this.modelId,
      messages,
    });
    const assistantMessage = response.choices[0].message.content;

    // Mantenha o histórico da conversa para interações de múltiplas etapas
    this.history.push({ role: "user", content: userMessage });
    this.history.push({ role: "assistant", content: assistantMessage });
    return { text: assistantMessage };
  }
}

async function main() {
  FoundryLocalManager.create({ appName: "FoundryLocalWorkshop" });
  const manager = FoundryLocalManager.instance;
  await manager.startWebService();

  const catalog = manager.catalog;
  const model = await catalog.getModel("phi-3.5-mini");
  if (!model.isCached) {
    console.log("Downloading model: phi-3.5-mini...");
    await model.download();
  }
  await model.load();

  const client = new OpenAI({
    baseURL: manager.urls[0] + "/v1",
    apiKey: "foundry-local",
  });

  const agent = new ChatAgent({
    client,
    modelId: model.id,
    instructions: "You are good at telling jokes.",
    name: "Joker",
  });

  const result = await agent.run("Tell me a joke about a pirate.");
  console.log(result.text);
}

main();
```

**Pontos importantes:**
- JavaScript constroi sua própria classe `ChatAgent` espelhando o padrão AGF do Python
- `this.history` armazena as interações da conversa para suporte multi-turno
- Chamadas explícitas: `startWebService()` → verificação de cache → `model.download()` → `model.load()` fornecem total visibilidade

</details>

<details>
<summary><strong>💜 C#</strong></summary>

**Pré-requisitos:**
```bash
cd csharp
dotnet restore
```

**Executar:**
```bash
dotnet run agent
```

**Passeio pelo código** (`csharp/SingleAgent.cs`):

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Agents.AI;
using OpenAI;
using System.ClientModel;

// 1. Start Foundry Local and load a model
var alias = "phi-3.5-mini";
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);

var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine($"Downloading model: {alias}...");
    await model.DownloadAsync(null, default);
}
await model.LoadAsync(default);

var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls[0] + "/v1")
});

// 2. Create an AIAgent using the Agent Framework extension method
AIAgent joker = client
    .GetChatClient(model.Id)
    .AsAIAgent(
        instructions: "You are good at telling jokes. Keep your jokes short and family-friendly.",
        name: "Joker"
    );

// 3. Run the agent (non-streaming)
var response = await joker.RunAsync("Tell me a joke about a pirate.");
Console.WriteLine($"Joker: {response}");

// 4. Run with streaming
await foreach (var update in joker.RunStreamingAsync("Tell me another joke."))
{
    Console.Write(update);
}
```

**Pontos importantes:**
- `AsAIAgent()` é um método de extensão do `Microsoft.Agents.AI.OpenAI` - não é necessário criar uma classe `ChatAgent` personalizada
- `RunAsync()` retorna a resposta completa; `RunStreamingAsync()` transmite token por token
- Instale via `dotnet add package Microsoft.Agents.AI.OpenAI --version 1.0.0-rc3`

</details>

---

### Exercício 3 - Mude a Persona

Modifique as `instructions` do agente para criar uma persona diferente. Experimente cada uma e observe como a saída muda:

| Persona | Instruções |
|---------|-------------|
| Revisor de Código | `"Você é um revisor de código especialista. Forneça feedback construtivo focado em legibilidade, desempenho e correção."` |
| Guia de Viagem | `"Você é um guia de viagem amigável. Dê recomendações personalizadas sobre destinos, atividades e culinária local."` |
| Tutor Socrático | `"Você é um tutor socrático. Nunca dê respostas diretas - em vez disso, oriente o aluno com perguntas reflexivas."` |
| Escritor Técnico | `"Você é um escritor técnico. Explique conceitos de forma clara e concisa. Use exemplos. Evite jargões."` |

**Experimente:**
1. Escolha uma persona da tabela acima
2. Substitua a string `instructions` no código
3. Ajuste o prompt do usuário para combinar (ex.: peça ao revisor de código para revisar uma função)
4. Execute o exemplo novamente e compare a saída

> **Dica:** A qualidade de um agente depende fortemente das instruções. Instruções específicas e bem estruturadas produzem resultados melhores do que as vagas.

---

### Exercício 4 - Adicione Conversa Multi-Turno

Estenda o exemplo para suportar um loop de chat multi-turno para que você possa ter uma conversa de ida e volta com o agente.

<details>
<summary><strong>🐍 Python - loop multi-turno</strong></summary>

```python
import asyncio
from agent_framework_foundry_local import FoundryLocalClient

async def main():
    client = FoundryLocalClient(model_id="phi-4-mini")

    agent = client.as_agent(
        name="Assistant",
        instructions="You are a helpful assistant.",
    )

    print("Chat with the agent (type 'quit' to exit):\n")
    while True:
        user_input = input("You: ")
        if user_input.strip().lower() in ("quit", "exit"):
            break
        result = await agent.run(user_input)
        print(f"Agent: {result}\n")

asyncio.run(main())
```

</details>

<details>
<summary><strong>📦 JavaScript - loop multi-turno</strong></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";
import * as readline from "node:readline/promises";

// (reutilizar a classe ChatAgent do Exercício 2)

async function main() {
  FoundryLocalManager.create({ appName: "FoundryLocalWorkshop" });
  const manager = FoundryLocalManager.instance;
  await manager.startWebService();

  const catalog = manager.catalog;
  const model = await catalog.getModel("phi-3.5-mini");
  if (!model.isCached) {
    console.log("Downloading model: phi-3.5-mini...");
    await model.download();
  }
  await model.load();

  const client = new OpenAI({
    baseURL: manager.urls[0] + "/v1",
    apiKey: "foundry-local",
  });

  const agent = new ChatAgent({
    client,
    modelId: model.id,
    instructions: "You are a helpful assistant.",
    name: "Assistant",
  });

  const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout,
  });

  console.log("Chat with the agent (type 'quit' to exit):\n");
  while (true) {
    const userInput = await rl.question("You: ");
    if (["quit", "exit"].includes(userInput.trim().toLowerCase())) break;
    const result = await agent.run(userInput);
    console.log(`Agent: ${result.text}\n`);
  }
  rl.close();
}

main();
```

</details>

<details>
<summary><strong>💜 C# - loop multi-turno</strong></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Agents.AI;
using OpenAI;
using System.ClientModel;

var alias = "phi-3.5-mini";
var config = new Configuration
{
    AppName = "FoundryLocalSamples",
    Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
};
await FoundryLocalManager.CreateAsync(config, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);

var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine($"Downloading model: {alias}...");
    await model.DownloadAsync(null, default);
}
await model.LoadAsync(default);

var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls[0] + "/v1")
});

AIAgent agent = client
    .GetChatClient(model.Id)
    .AsAIAgent(
        instructions: "You are a helpful assistant.",
        name: "Assistant"
    );

Console.WriteLine("Chat with the agent (type 'quit' to exit):\n");
while (true)
{
    Console.Write("You: ");
    var userInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userInput) ||
        userInput.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
        userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    var result = await agent.RunAsync(userInput);
    Console.WriteLine($"Agent: {result}\n");
}
```

</details>

Note como o agente lembra as interações anteriores - faça uma pergunta de acompanhamento e veja o contexto ser mantido.

---

### Exercício 5 - Saída Estruturada

Instrua o agente para sempre responder em um formato específico (ex.: JSON) e analise o resultado:

<details>
<summary><strong>🐍 Python - saída JSON</strong></summary>

```python
import asyncio
import json
from agent_framework_foundry_local import FoundryLocalClient

async def main():
    client = FoundryLocalClient(model_id="phi-4-mini")

    agent = client.as_agent(
        name="SentimentAnalyzer",
        instructions=(
            "You are a sentiment analysis agent. "
            "For every user message, respond ONLY with valid JSON in this format: "
            '{"sentiment": "positive|negative|neutral", "confidence": 0.0-1.0, "summary": "brief reason"}'
        ),
    )

    result = await agent.run("I absolutely loved the new restaurant downtown!")
    print("Raw:", result)

    try:
        parsed = json.loads(str(result))
        print(f"Sentiment: {parsed['sentiment']} (confidence: {parsed['confidence']})")
    except json.JSONDecodeError:
        print("Agent did not return valid JSON - try refining the instructions.")

asyncio.run(main())
```

</details>

<details>
<summary><strong>💜 C# - saída JSON</strong></summary>

```csharp
using System.Text.Json;

AIAgent analyzer = chatClient.AsAIAgent(
    name: "SentimentAnalyzer",
    instructions:
        "You are a sentiment analysis agent. " +
        "For every user message, respond ONLY with valid JSON in this format: " +
        "{\"sentiment\": \"positive|negative|neutral\", \"confidence\": 0.0-1.0, \"summary\": \"brief reason\"}"
);

var response = await analyzer.RunAsync("I absolutely loved the new restaurant downtown!");
Console.WriteLine($"Raw: {response}");

try
{
    var parsed = JsonSerializer.Deserialize<JsonElement>(response.ToString());
    Console.WriteLine($"Sentiment: {parsed.GetProperty("sentiment")} " +
                      $"(confidence: {parsed.GetProperty("confidence")})");
}
catch (JsonException)
{
    Console.WriteLine("Agent did not return valid JSON - try refining the instructions.");
}
```

</details>

> **Nota:** Modelos locais pequenos podem não produzir JSON perfeitamente válido sempre. Você pode melhorar a confiabilidade incluindo um exemplo nas instruções e sendo muito explícito sobre o formato esperado.

---

## Principais Aprendizados

| Conceito | O Que Você Aprendeu |
|---------|-----------------|
| Agente vs. chamada direta ao LLM | Um agente envolve um modelo com instruções e memória |
| Instruções do sistema | A alavanca mais importante para controlar o comportamento do agente |
| Conversa multi-turno | Agentes podem manter contexto através de múltiplas interações de usuário |
| Saída estruturada | Instruções podem impor formato de saída (JSON, markdown, etc.) |
| Execução local | Tudo roda no dispositivo via Foundry Local - sem necessidade de nuvem |

---

## Próximos Passos

Em **[Parte 6: Fluxos de Trabalho com Multi-Agentes](part6-multi-agent-workflows.md)**, você combinará múltiplos agentes em uma pipeline coordenada onde cada agente tem um papel especializado.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:
Este documento foi traduzido utilizando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos para garantir a precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original, em seu idioma nativo, deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se tradução humana profissional. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->