![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 11: Chamadas de Ferramentas com Modelos Locais

> **Objetivo:** Permitir que o seu modelo local faça chamadas a funções externas (ferramentas) para que possa obter dados em tempo real, realizar cálculos ou interagir com APIs — tudo a correr de forma privada no seu dispositivo.

## O Que É a Chamada de Ferramentas?

A chamada de ferramentas (também conhecida como **chamada de funções**) permite a um modelo de linguagem solicitar a execução de funções que você define. Em vez de adivinhar uma resposta, o modelo reconhece quando uma ferramenta seria útil e retorna um pedido estruturado para que o seu código execute. A sua aplicação executa a função, envia o resultado de volta e o modelo incorpora essa informação na resposta final.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Este padrão é essencial para construir agentes que podem:

- **Consultar dados ao vivo** (previsão do tempo, cotações de ações, consultas a bases de dados)
- **Realizar cálculos precisos** (matemática, conversões de unidades)
- **Executar ações** (enviar emails, criar tickets, atualizar registos)
- **Aceder a sistemas privados** (APIs internas, sistemas de ficheiros)

---

## Como Funciona a Chamada de Ferramentas

O fluxo de chamada de ferramentas tem quatro etapas:

| Etapa | O Que Acontece |
|-------|----------------|
| **1. Definir ferramentas** | Você descreve as funções disponíveis usando JSON Schema — nome, descrição e parâmetros |
| **2. Modelo decide** | O modelo recebe a sua mensagem e as definições das ferramentas. Se uma ferramenta ajudar, retorna uma resposta `tool_calls` em vez de uma resposta em texto |
| **3. Executar localmente** | O seu código analisa a chamada da ferramenta, executa a função e obtém o resultado |
| **4. Resposta final** | Você envia o resultado da ferramenta de volta para o modelo, que produz a resposta final |

> **Ponto-chave:** O modelo nunca executa código. Só *pede* que uma ferramenta seja chamada. A sua aplicação decide se aceita esse pedido — isto mantém você no controlo total.

---

## Quais Modelos Suportam Chamada de Ferramentas?

Nem todos os modelos suportam a chamada de ferramentas. No catálogo atual do Foundry Local, os seguintes modelos têm a funcionalidade de chamada de ferramentas:

| Modelo | Tamanho | Chamada de Ferramentas |
|--------|---------|:---------------------:|
| qwen2.5-0.5b | 822 MB | ✅ |
| qwen2.5-1.5b | 1.8 GB | ✅ |
| qwen2.5-7b | 6.3 GB | ✅ |
| qwen2.5-14b | 11.3 GB | ✅ |
| qwen2.5-coder-0.5b | 822 MB | ✅ |
| qwen2.5-coder-1.5b | 1.8 GB | ✅ |
| qwen2.5-coder-7b | 6.3 GB | ✅ |
| qwen2.5-coder-14b | 11.3 GB | ✅ |
| phi-4-mini | 4.6 GB | ✅ |
| phi-3.5-mini | 2.6 GB | ❌ |
| phi-4 | 10.4 GB | ❌ |

> **Dica:** Para este laboratório usamos o **qwen2.5-0.5b** — é pequeno (822 MB de download), rápido e tem suporte fiável para chamadas de ferramentas.

---

## Objetivos de Aprendizagem

No final deste laboratório, será capaz de:

- Explicar o padrão de chamada de ferramentas e por que é importante para agentes de IA
- Definir esquemas de ferramentas usando o formato OpenAI function-calling
- Gerir o fluxo de conversação com múltiplas interações para chamada de ferramentas
- Executar chamadas de ferramentas localmente e devolver resultados ao modelo
- Escolher o modelo apropriado para cenários de chamadas de ferramentas

---

## Pré-requisitos

| Requisito | Detalhes |
|-----------|----------|
| **Foundry Local CLI** | Instalado e disponível no seu `PATH` ([Parte 1](part1-getting-started.md)) |
| **Foundry Local SDK** | SDK Python, JavaScript ou C# instalado ([Parte 2](part2-foundry-local-sdk.md)) |
| **Um modelo de chamada de ferramentas** | qwen2.5-0.5b (será descarregado automaticamente) |

---

## Exercícios

### Exercício 1 — Compreender o Fluxo de Chamada de Ferramentas

Antes de escrever código, estude este diagrama de sequência:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Observações chave:**

1. Você define as ferramentas antecipadamente como objetos JSON Schema  
2. A resposta do modelo contém `tool_calls` em vez de conteúdo normal  
3. Cada chamada de ferramenta tem um `id` único que deve usar ao devolver resultados  
4. O modelo vê todas as mensagens anteriores *mais* os resultados das ferramentas ao gerar a resposta final  
5. Várias chamadas de ferramentas podem acontecer numa única resposta  

> **Discussão:** Por que o modelo devolve chamadas de ferramentas em vez de executar funções diretamente? Que vantagens de segurança isto proporciona?

---

### Exercício 2 — Definir Esquemas de Ferramentas

As ferramentas são definidas usando o formato padrão do OpenAI para chamadas de funções. Cada ferramenta necessita de:

- **`type`**: Sempre `"function"`
- **`function.name`**: Um nome descritivo da função (ex: `get_weather`)
- **`function.description`**: Uma descrição clara — que o modelo usa para decidir quando chamar a ferramenta
- **`function.parameters`**: Um objeto JSON Schema que descreve os argumentos esperados

```json
{
  "type": "function",
  "function": {
    "name": "get_weather",
    "description": "Get the current weather for a given city",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. London"
        }
      },
      "required": ["city"]
    }
  }
}
```

> **Boas práticas para descrições de ferramentas:**
> - Seja específico: "Obter o tempo atual para uma cidade dada" é melhor que "Obter tempo"
> - Descreva os parâmetros claramente: o modelo lê estas descrições para preencher os valores corretos
> - Indique parâmetros obrigatórios vs opcionais — isso ajuda o modelo a decidir o que pedir

---

### Exercício 3 — Executar os Exemplos de Chamadas de Ferramentas

Cada exemplo de linguagem define duas ferramentas (`get_weather` e `get_population`), envia uma questão que desencadeia o uso de ferramentas, executa localmente a ferramenta e devolve o resultado para a resposta final.

<details>
<summary><strong>🐍 Python</strong></summary>

**Pré-requisitos:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / Linux:
source venv/bin/activate

pip install -r requirements.txt
```

**Executar:**
```bash
python foundry-local-tool-calling.py
```

**Saída esperada:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Análise do código** (`python/foundry-local-tool-calling.py`):

```python
# Define ferramentas como uma lista de esquemas de função
tools = [
    {
        "type": "function",
        "function": {
            "name": "get_weather",
            "description": "Get the current weather for a given city",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {"type": "string", "description": "The city name"}
                },
                "required": ["city"]
            }
        }
    }
]

# Enviar com ferramentas — o modelo pode retornar chamadas_de_ferramenta em vez de conteúdo
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# Verificar se o modelo quer chamar uma ferramenta
if response.choices[0].message.tool_calls:
    # Executar a ferramenta e enviar o resultado de volta
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**Pré-requisitos:**
```bash
cd javascript
npm install
```

**Executar:**
```bash
node foundry-local-tool-calling.mjs
```

**Saída esperada:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Análise do código** (`javascript/foundry-local-tool-calling.mjs`):

Este exemplo usa o `ChatClient` nativo do SDK Foundry Local em vez do SDK OpenAI, demonstrando o método conveniente `createChatClient()`:

```javascript
// Obtenha um ChatClient diretamente do objeto do modelo
const chatClient = model.createChatClient();

// Envie com ferramentas — o ChatClient gere o formato compatível com OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Verificar chamadas de ferramentas
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Executar ferramentas e enviar resultados de volta
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**Pré-requisitos:**
```bash
cd csharp
dotnet restore
```

**Executar:**
```bash
dotnet run toolcall
```

**Saída esperada:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**Análise do código** (`csharp/ToolCalling.cs`):

O C# usa o auxiliar `ChatTool.CreateFunctionTool` para definir ferramentas:

```csharp
ChatTool getWeatherTool = ChatTool.CreateFunctionTool(
    functionName: "get_weather",
    functionDescription: "Get the current weather for a given city",
    functionParameters: BinaryData.FromString("""
    {
        "type": "object",
        "properties": {
            "city": { "type": "string", "description": "The city name" }
        },
        "required": ["city"]
    }
    """));

var options = new ChatCompletionOptions();
options.Tools.Add(getWeatherTool);

// Check FinishReason to see if tools were called
if (completion.Value.FinishReason == ChatFinishReason.ToolCalls)
{
    // Execute tools and send results back
    ...
}
```

</details>

---

### Exercício 4 — O Fluxo de Conversação de Chamada de Ferramentas

Compreender a estrutura da mensagem é crucial. Aqui está o fluxo completo, mostrando o array `messages` em cada etapa:

**Etapa 1 — Pedido inicial:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**Etapa 2 — Modelo responde com tool_calls (não conteúdo):**
```json
{
  "role": "assistant",
  "tool_calls": [
    {
      "id": "call_abc123",
      "type": "function",
      "function": {
        "name": "get_weather",
        "arguments": "{\"city\": \"London\"}"
      }
    }
  ]
}
```

**Etapa 3 — Você adiciona a mensagem do assistente E o resultado da ferramenta:**
```json
[
  {"role": "system", "content": "..."},
  {"role": "user", "content": "What is the weather like in London?"},
  {"role": "assistant", "tool_calls": [...]},
  {
    "role": "tool",
    "tool_call_id": "call_abc123",
    "content": "{\"city\": \"London\", \"temperature\": \"18°C\", \"condition\": \"Partly cloudy\"}"
  }
]
```

**Etapa 4 — Modelo produz a resposta final usando o resultado da ferramenta.**

> **Importante:** O `tool_call_id` na mensagem da ferramenta deve corresponder ao `id` da chamada da ferramenta. É assim que o modelo associa resultados aos pedidos.

---

### Exercício 5 — Múltiplas Chamadas de Ferramentas

Um modelo pode pedir várias chamadas de ferramentas numa única resposta. Experimente alterar a mensagem do utilizador para desencadear múltiplas chamadas:

```python
# Em Python — alterar a mensagem do utilizador:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// Em JavaScript — altere a mensagem do utilizador:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

O modelo deverá devolver duas `tool_calls` — uma para `get_weather` e outra para `get_population`. O seu código já trata disso porque itera em todas as chamadas de ferramentas.

> **Experimente:** Modifique a mensagem do utilizador e corra o exemplo novamente. O modelo chama ambas as ferramentas?

---

### Exercício 6 — Adicionar a Sua Própria Ferramenta

Estenda um dos exemplos com uma nova ferramenta. Por exemplo, adicione uma ferramenta `get_time`:

1. Defina o esquema da ferramenta:
```json
{
  "type": "function",
  "function": {
    "name": "get_time",
    "description": "Get the current time in a given city's timezone",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. Tokyo"
        }
      },
      "required": ["city"]
    }
  }
}
```

2. Adicione a lógica de execução:
```python
# Python
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # Numa aplicação real, utilize uma biblioteca de fusos horários
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... ferramentas existentes ...
```

3. Adicione a ferramenta ao array `tools` e teste com: "Que horas são em Tokyo?"

> **Desafio:** Adicione uma ferramenta que faça um cálculo, como `convert_temperature` que converte entre Celsius e Fahrenheit. Teste com: "Converter 100°F para Celsius."

---

### Exercício 7 — Chamada de Ferramentas com o ChatClient do SDK (JavaScript)

O exemplo em JavaScript já usa o `ChatClient` nativo do SDK em vez do SDK OpenAI. Isto é uma funcionalidade de conveniência que elimina a necessidade de construir um cliente OpenAI por si:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient é criado diretamente a partir do objeto modelo
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat aceita ferramentas como um segundo parâmetro
const response = await chatClient.completeChat(messages, tools);
```

Compare com a abordagem Python que usa explicitamente o SDK OpenAI:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Ambos os padrões são válidos. O `ChatClient` é mais conveniente; o SDK OpenAI dá-lhe acesso à gama completa de parâmetros OpenAI.

> **Experimente:** Modifique o exemplo em JavaScript para usar o SDK OpenAI em vez do `ChatClient`. Vai precisar de `import OpenAI from "openai"` e construir o cliente com o endpoint de `manager.urls[0]`.

---

### Exercício 8 — Compreender tool_choice

O parâmetro `tool_choice` controla se o modelo *deve* usar uma ferramenta ou pode escolher livremente:

| Valor | Comportamento |
|-------|--------------|
| `"auto"` | O modelo decide se chama uma ferramenta (padrão) |
| `"none"` | O modelo não chama ferramentas, mesmo que existam |
| `"required"` | O modelo deve chamar pelo menos uma ferramenta |
| `{"type": "function", "function": {"name": "get_weather"}}` | O modelo deve chamar a ferramenta especificada |

Experimente cada opção no exemplo Python:

```python
# Forçar o modelo a chamar get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Nota:** Nem todas as opções de `tool_choice` podem ser suportadas por todos os modelos. Se um modelo não suportar `"required"`, pode ignorar a configuração e comportar-se como `"auto"`.

---

## Armadilhas Comuns

| Problema | Solução |
|----------|---------|
| Modelo nunca chama ferramentas | Certifique-se que está a usar um modelo com suporte para chamadas de ferramentas (ex: qwen2.5-0.5b). Veja a tabela acima. |
| `tool_call_id` não coincide | Use sempre o `id` da resposta da chamada da ferramenta, não um valor fixo |
| Modelo devolve JSON malformado em `arguments` | Modelos pequenos podem gerar JSON inválido. Envolva `JSON.parse()` em try/catch |
| Modelo chama uma ferramenta que não existe | Adicione um manipulador default na sua função `execute_tool` |
| Loop infinito de chamadas de ferramentas | Defina um número máximo de rondas (ex: 5) para evitar loops sem fim |

---

## Pontos-Chave

1. **Chamada de ferramentas** permite que modelos solicitem execução de funções em vez de adivinhar respostas  
2. O modelo **nunca executa código**; a sua aplicação decide o que executar  
3. As ferramentas são definidas como objetos **JSON Schema** seguindo o formato OpenAI function-calling  
4. A conversa usa um **padrão multi-turn**: utilizador, depois assistente (tool_calls), depois ferramenta (resultados), depois assistente (resposta final)  
5. Use sempre um **modelo que suporte chamada de ferramentas** (Qwen 2.5, Phi-4-mini)  
6. O método `createChatClient()` do SDK fornece uma forma conveniente de fazer pedidos de chamadas de ferramentas sem construir um cliente OpenAI  

---

Prossiga para [Parte 12: Construir uma Interface Web para o Zava Creative Writer](part12-zava-ui.md) para adicionar uma interface browser ao pipeline multi-agente com streaming em tempo real.

---

[← Parte 10: Modelos Personalizados](part10-custom-models.md) | [Parte 12: Interface do Zava Writer →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:
Este documento foi traduzido utilizando o serviço de tradução automática [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, por favor tenha em atenção que traduções automáticas podem conter erros ou imprecisões. O documento original na sua língua nativa deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se a tradução profissional por humanos. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações erradas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->