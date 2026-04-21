![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 11: Chamada de Ferramentas com Modelos Locais

> **Objetivo:** Permitir que seu modelo local chame funções externas (ferramentas) para que ele possa recuperar dados em tempo real, realizar cálculos ou interagir com APIs — tudo rodando privativamente no seu dispositivo.

## O que é Chamada de Ferramentas?

Chamada de ferramentas (também conhecida como **chamada de função**) permite que um modelo de linguagem solicite a execução de funções que você define. Ao invés de adivinhar uma resposta, o modelo reconhece quando uma ferramenta ajudaria e retorna uma solicitação estruturada para seu código executar. Sua aplicação roda a função, envia o resultado de volta, e o modelo incorpora essa informação na resposta final.

![Tool-calling flow](../../../images/tool-calling-flow.svg)

Esse padrão é essencial para construir agentes que podem:

- **Consultar dados ao vivo** (clima, preços de ações, consultas a banco de dados)
- **Executar cálculos precisos** (matemática, conversão de unidades)
- **Executar ações** (enviar emails, criar tickets, atualizar registros)
- **Acessar sistemas privados** (APIs internas, sistemas de arquivos)

---

## Como funciona a Chamada de Ferramentas

O fluxo de chamada de ferramentas tem quatro etapas:

| Etapa | O que acontece |
|-------|----------------|
| **1. Definir ferramentas** | Você descreve as funções disponíveis usando JSON Schema — nome, descrição e parâmetros |
| **2. Modelo decide** | O modelo recebe sua mensagem mais as definições das ferramentas. Se uma ferramenta ajudar, ele retorna uma resposta com `tool_calls` ao invés de texto |
| **3. Executar localmente** | Seu código interpreta a chamada da ferramenta, executa a função e coleta o resultado |
| **4. Resposta final** | Você envia o resultado da ferramenta de volta para o modelo, que produz a resposta final |

> **Ponto-chave:** O modelo nunca executa código. Ele apenas *solicita* que uma ferramenta seja chamada. Sua aplicação decide se atende a essa solicitação — assim você mantém controle total.

---

## Quais modelos suportam chamada de ferramentas?

Nem todo modelo suporta chamada de ferramentas. No catálogo atual do Foundry Local, os seguintes modelos têm essa capacidade:

| Modelo | Tamanho | Chamada de Ferramentas |
|--------|---------|:----------------------:|
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

> **Dica:** Para este laboratório usamos o **qwen2.5-0.5b** — é pequeno (822 MB para baixar), rápido, e tem suporte confiável para chamada de ferramentas.

---

## Objetivos de Aprendizagem

Ao final deste laboratório você será capaz de:

- Explicar o padrão de chamada de ferramentas e por que é importante para agentes de IA
- Definir esquemas de ferramentas usando o formato OpenAI function-calling
- Gerenciar o fluxo de conversa multi-turno com chamadas de ferramentas
- Executar chamadas de ferramentas localmente e enviar resultados ao modelo
- Escolher o modelo certo para cenários de chamada de ferramentas

---

## Pré-requisitos

| Requisito | Detalhes |
|-----------|----------|
| **Foundry Local CLI** | Instalado e no seu `PATH` ([Parte 1](part1-getting-started.md)) |
| **Foundry Local SDK** | SDK Python, JavaScript ou C# instalado ([Parte 2](part2-foundry-local-sdk.md)) |
| **Um modelo com chamada de ferramentas** | qwen2.5-0.5b (será baixado automaticamente) |

---

## Exercícios

### Exercício 1 — Entender o Fluxo da Chamada de Ferramentas

Antes de escrever código, estude este diagrama de sequência:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**Observações chave:**

1. Você define as ferramentas antecipadamente como objetos JSON Schema
2. A resposta do modelo contém `tool_calls` ao invés de conteúdo normal
3. Cada chamada a ferramenta tem um `id` único que você deve referenciar ao retornar resultados
4. O modelo vê todas as mensagens anteriores *mais* os resultados das ferramentas ao gerar a resposta final
5. Várias chamadas de ferramentas podem acontecer numa única resposta

> **Discussão:** Por que o modelo retorna chamadas de ferramentas invés de executar funções diretamente? Que vantagens de segurança isso oferece?

---

### Exercício 2 — Definindo os Esquemas das Ferramentas

As ferramentas são definidas usando o formato padrão OpenAI para chamadas de função. Cada ferramenta precisa de:

- **`type`**: Sempre `"function"`
- **`function.name`**: Um nome descritivo da função (ex.: `get_weather`)
- **`function.description`**: Uma descrição clara — o modelo usa isso para decidir quando chamar a ferramenta
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

> **Boas práticas para descrições das ferramentas:**
> - Seja específico: "Obter o clima atual para uma cidade" é melhor do que "Obter clima"
> - Descreva os parâmetros claramente: o modelo lê essas descrições para preencher os valores corretos
> - Marque parâmetros obrigatórios vs opcionais — isso ajuda o modelo a saber o que perguntar

---

### Exercício 3 — Executar os Exemplos de Chamada de Ferramentas

Cada exemplo de linguagem define duas ferramentas (`get_weather` e `get_population`), envia uma pergunta que ativa o uso da ferramenta, executa a ferramenta localmente e envia o resultado para uma resposta final.

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

# Verifique se o modelo deseja chamar uma ferramenta
if response.choices[0].message.tool_calls:
    # Execute a ferramenta e envie o resultado de volta
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

Este exemplo usa o `ChatClient` nativo do Foundry Local SDK ao invés do SDK OpenAI, mostrando a conveniência do método `createChatClient()`:

```javascript
// Obter um ChatClient diretamente do objeto modelo
const chatClient = model.createChatClient();

// Enviar com ferramentas — ChatClient lida com o formato compatível com OpenAI
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// Verificar chamadas de ferramenta
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // Executar ferramentas e enviar os resultados de volta
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

C# usa o helper `ChatTool.CreateFunctionTool` para definir ferramentas:

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

### Exercício 4 — O Fluxo da Conversa com Chamada de Ferramentas

Entender a estrutura da mensagem é crítico. Aqui está o fluxo completo, mostrando o array `messages` em cada etapa:

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

> **Importante:** O `tool_call_id` na mensagem da ferramenta deve corresponder ao `id` da chamada da ferramenta. Assim o modelo associa os resultados às solicitações.

---

### Exercício 5 — Múltiplas Chamadas de Ferramentas

Um modelo pode solicitar várias chamadas de ferramentas numa única resposta. Tente mudar a mensagem do usuário para disparar múltiplas chamadas:

```python
# Em Python — altere a mensagem do usuário:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// Em JavaScript — altere a mensagem do usuário:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

O modelo deve retornar dois `tool_calls` — um para `get_weather` e outro para `get_population`. Seu código já lida com isso porque itera por todas as chamadas de ferramentas.

> **Experimente:** Modifique a mensagem do usuário e execute o exemplo novamente. O modelo chama ambas as ferramentas?

---

### Exercício 6 — Adicione Sua Própria Ferramenta

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
        # Em um aplicativo real, use uma biblioteca de fuso horário
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... ferramentas existentes ...
```

3. Adicione a ferramenta ao array `tools` e teste com: "Que horas são em Tóquio?"

> **Desafio:** Adicione uma ferramenta que realize um cálculo, como `convert_temperature` que converte entre Celsius e Fahrenheit. Teste com: "Converta 100°F para Celsius."

---

### Exercício 7 — Chamada de Ferramentas com ChatClient do SDK (JavaScript)

O exemplo JavaScript já usa o `ChatClient` nativo do SDK ao invés do SDK OpenAI. Isso é uma facilidade que elimina a necessidade de construir um cliente OpenAI manualmente:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient é criado diretamente a partir do objeto modelo
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat aceita ferramentas como segundo parâmetro
const response = await chatClient.completeChat(messages, tools);
```

Compare isso com a abordagem Python que usa explicitamente o SDK OpenAI:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

Ambos os padrões são válidos. O `ChatClient` é mais conveniente; o SDK OpenAI oferece acesso a toda a gama de parâmetros OpenAI.

> **Experimente:** Modifique o exemplo JavaScript para usar o SDK OpenAI em vez de `ChatClient`. Você precisará de `import OpenAI from "openai"` e construir o cliente com o endpoint em `manager.urls[0]`.

---

### Exercício 8 — Entendendo o tool_choice

O parâmetro `tool_choice` controla se o modelo *deve* usar uma ferramenta ou pode escolher livremente:

| Valor | Comportamento |
|-------|---------------|
| `"auto"` | Modelo decide se chama uma ferramenta (padrão) |
| `"none"` | Modelo não chama nenhuma ferramenta, mesmo que disponível |
| `"required"` | Modelo deve chamar pelo menos uma ferramenta |
| `{"type": "function", "function": {"name": "get_weather"}}` | Modelo deve chamar a ferramenta especificada |

Teste cada opção no exemplo Python:

```python
# Forçar o modelo a chamar get_weather
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **Nota:** Nem todas as opções de `tool_choice` podem ser suportadas por todos os modelos. Se um modelo não suportar `"required"`, pode ignorar esse ajuste e agir como `"auto"`.

---

## Armadilhas Comuns

| Problema | Solução |
|----------|---------|
| Modelo nunca chama ferramentas | Verifique se está usando um modelo com suporte à chamada de ferramentas (ex.: qwen2.5-0.5b). Confira a tabela acima. |
| Incompatibilidade de `tool_call_id` | Sempre use o `id` da resposta da chamada de ferramenta, nunca um valor fixo |
| Modelo retorna JSON malformado em `arguments` | Modelos menores às vezes produzem JSON inválido. Coloque `JSON.parse()` em try/catch |
| Modelo chama ferramenta que não existe | Adicione um handler padrão na sua função `execute_tool` |
| Loop infinito de chamadas de ferramentas | Defina um número máximo de rodadas (ex.: 5) para evitar loops intermináveis |

---

## Pontos Principais

1. **Chamada de ferramentas** permite que modelos peçam a execução de funções ao invés de adivinhar respostas
2. O modelo **nunca executa código**; sua aplicação decide o que rodar
3. Ferramentas são definidas como objetos **JSON Schema** seguindo o formato OpenAI function-calling
4. A conversa usa um padrão **multi-turno**: usuário, assistente (tool_calls), ferramenta (resultados), assistente (resposta final)
5. Sempre use um **modelo que suporte chamada de ferramentas** (Qwen 2.5, Phi-4-mini)
6. O `createChatClient()` do SDK oferece uma forma conveniente de fazer chamadas de ferramentas sem construir um cliente OpenAI

---

Continue para [Parte 12: Construindo uma UI Web para o Zava Creative Writer](part12-zava-ui.md) para adicionar uma interface baseada em navegador ao pipeline multi-agente com streaming em tempo real.

---

[← Parte 10: Modelos Personalizados](part10-custom-models.md) | [Parte 12: UI do Zava Writer →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido usando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se tradução humana profissional. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->