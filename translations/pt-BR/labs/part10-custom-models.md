![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 10: Usando Modelos Personalizados ou Hugging Face com Foundry Local

> **Objetivo:** Compilar um modelo Hugging Face no formato ONNX otimizado que o Foundry Local requer, configurá-lo com um template de chat, adicioná-lo ao cache local e executar inferência usando o CLI, REST API e OpenAI SDK.

## Visão Geral

O Foundry Local é entregue com um catálogo selecionado de modelos pré-compilados, mas você não está limitado a essa lista. Qualquer modelo de linguagem baseado em transformadores disponível no [Hugging Face](https://huggingface.co/) (ou armazenado localmente nos formatos PyTorch / Safetensors) pode ser compilado em um modelo ONNX otimizado e servido através do Foundry Local.

O pipeline de compilação usa o **ONNX Runtime GenAI Model Builder**, uma ferramenta de linha de comando incluída no pacote `onnxruntime-genai`. O construtor de modelos faz o trabalho pesado: baixa os pesos fonte, converte-os para o formato ONNX, aplica quantização (int4, fp16, bf16) e gera os arquivos de configuração (incluindo o template de chat e tokenizador) que o Foundry Local espera.

Neste laboratório, você irá compilar **Qwen/Qwen3-0.6B** do Hugging Face, registrá-lo no Foundry Local e conversar com ele inteiramente no seu dispositivo.

---

## Objetivos de Aprendizagem

Ao final deste laboratório você será capaz de:

- Explicar por que a compilação de modelos personalizados é útil e quando pode ser necessária
- Instalar o construtor de modelos ONNX Runtime GenAI
- Compilar um modelo Hugging Face para o formato ONNX otimizado com um único comando
- Entender os principais parâmetros de compilação (provedor de execução, precisão)
- Criar o arquivo de configuração `inference_model.json` do template de chat
- Adicionar um modelo compilado ao cache do Foundry Local
- Executar inferência no modelo personalizado usando o CLI, REST API e OpenAI SDK

---

## Pré-requisitos

| Requisito | Detalhes |
|-------------|---------|
| **Foundry Local CLI** | Instalado e incluído no seu `PATH` ([Parte 1](part1-getting-started.md)) |
| **Python 3.10+** | Requerido pelo construtor de modelos ONNX Runtime GenAI |
| **pip** | Gerenciador de pacotes Python |
| **Espaço em disco** | Pelo menos 5 GB livres para os arquivos fonte e compilados do modelo |
| **Conta no Hugging Face** | Alguns modelos exigem que você aceite uma licença antes do download. Qwen3-0.6B usa a licença Apache 2.0 e está disponível gratuitamente. |

---

## Configuração do Ambiente

A compilação do modelo requer vários pacotes Python grandes (PyTorch, ONNX Runtime GenAI, Transformers). Crie um ambiente virtual dedicado para evitar interferência com seu Python do sistema ou outros projetos.

```bash
# A partir da raiz do repositório
python -m venv .venv
```

Ative o ambiente:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Atualize o pip para evitar problemas de resolução de dependências:

```bash
python -m pip install --upgrade pip
```

> **Dica:** Se você já tem um `.venv` dos laboratórios anteriores, pode reutilizá-lo. Apenas certifique-se de que ele está ativado antes de continuar.

---

## Conceito: O Pipeline de Compilação

O Foundry Local requer modelos no formato ONNX com configuração ONNX Runtime GenAI. A maioria dos modelos open-source no Hugging Face são distribuídos como pesos PyTorch ou Safetensors, então é necessário um passo de conversão.

![Pipeline de compilação de modelo personalizado](../../../images/custom-model-pipeline.svg)

### O Que o Construtor de Modelos Faz?

1. **Baixa** o modelo fonte do Hugging Face (ou o lê de um caminho local).
2. **Converte** os pesos PyTorch / Safetensors em formato ONNX.
3. **Quantiza** o modelo para uma precisão menor (por exemplo, int4) para reduzir o uso de memória e melhorar o desempenho.
4. **Gera** a configuração ONNX Runtime GenAI (`genai_config.json`), o template de chat (`chat_template.jinja`) e todos os arquivos do tokenizador para que o Foundry Local possa carregar e servir o modelo.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Você pode encontrar referências à **Microsoft Olive** como uma ferramenta alternativa para otimização de modelos. Ambas podem produzir modelos ONNX, mas servem propósitos diferentes e têm compensações distintas:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Pacote** | `onnxruntime-genai` | `olive-ai` |
| **Propósito principal** | Converter e quantizar modelos de IA generativa para inferência ONNX Runtime GenAI | Framework de otimização de modelo end-to-end, suportando múltiplos backends e hardwares |
| **Facilidade de uso** | Comando único — conversão + quantização em uma etapa | Baseado em fluxo de trabalho — pipelines configuráveis multi-pass com YAML/JSON |
| **Formato de saída** | Formato ONNX Runtime GenAI (pronto para Foundry Local) | ONNX genérico, ONNX Runtime GenAI, ou outros conforme o fluxo de trabalho |
| **Alvo hardware** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, e mais |
| **Opções de quantização** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, além de otimizações de grafo e ajuste por camada |
| **Escopo do modelo** | Modelos de IA generativa (LLMs, SLMs) | Qualquer modelo convertível para ONNX (visão, NLP, áudio, multimodal) |
| **Ideal para** | Compilação rápida de um único modelo para inferência local | Pipelines de produção com controle fino de otimização |
| **Dependências** | Moderadas (PyTorch, Transformers, ONNX Runtime) | Maiores (inclui framework Olive, extras opcionais por fluxo) |
| **Integração com Foundry Local** | Direta — saída é imediatamente compatível | Requer flag `--use_ort_genai` e configuração adicional |

> **Por que este laboratório usa o Model Builder:** Para a tarefa de compilar um único modelo Hugging Face e registrá-lo no Foundry Local, o Model Builder é a maneira mais simples e confiável. Ele produz o formato de saída exato que o Foundry Local espera em um único comando. Se você precisar de recursos avançados de otimização — como quantização consciente da acurácia, manipulação de grafos, ou ajuste multi-pass — Olive é uma poderosa opção a explorar. Veja a [documentação Microsoft Olive](https://microsoft.github.io/Olive/) para mais detalhes.

---

## Exercícios do Laboratório

### Exercício 1: Instalar o ONNX Runtime GenAI Model Builder

Instale o pacote ONNX Runtime GenAI, que inclui a ferramenta construtora de modelos:

```bash
pip install onnxruntime-genai
```

Verifique a instalação checando se o construtor de modelos está disponível:

```bash
python -m onnxruntime_genai.models.builder --help
```

Você deverá ver uma saída de ajuda listando parâmetros como `-m` (nome do modelo), `-o` (caminho de saída), `-p` (precisão) e `-e` (provedor de execução).

> **Nota:** O construtor depende de PyTorch, Transformers e outros pacotes. A instalação pode levar alguns minutos.

---

### Exercício 2: Compilar Qwen3-0.6B para CPU

Execute o comando abaixo para baixar o modelo Qwen3-0.6B do Hugging Face e compilá-lo para inferência CPU com quantização int4:

**macOS / Linux:**
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```

**Windows (PowerShell):**
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```

#### O Que Cada Parâmetro Faz

| Parâmetro | Propósito | Valor Usado |
|-----------|---------|------------|
| `-m` | ID do modelo Hugging Face ou caminho para diretório local | `Qwen/Qwen3-0.6B` |
| `-o` | Diretório onde o modelo ONNX compilado será salvo | `models/qwen3` |
| `-p` | Precisão de quantização aplicada durante a compilação | `int4` |
| `-e` | Provedor de execução ONNX Runtime (hardware alvo) | `cpu` |
| `--extra_options hf_token=false` | Pulaj autenticação Hugging Face (ok para modelos públicos) | `hf_token=false` |

> **Quanto tempo leva?** O tempo de compilação depende do seu hardware e do tamanho do modelo. Para Qwen3-0.6B com quantização int4 em um CPU moderno, espere entre 5 a 15 minutos. Modelos maiores levam proporcionalmente mais tempo.

Quando o comando terminar, você deverá ver um diretório `models/qwen3` contendo os arquivos do modelo compilado. Verifique a saída:

```bash
ls models/qwen3
```

Você deve ver arquivos incluindo:
- `model.onnx` e `model.onnx.data` — os pesos compilados do modelo
- `genai_config.json` — configuração ONNX Runtime GenAI
- `chat_template.jinja` — o template de chat do modelo (auto-gerado)
- `tokenizer.json`, `tokenizer_config.json` — arquivos do tokenizador
- Outros arquivos de vocabulário e configuração

---

### Exercício 3: Compilar para GPU (Opcional)

Se você tem uma GPU NVIDIA com suporte CUDA, pode compilar uma variante otimizada para GPU para inferência mais rápida:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Nota:** A compilação para GPU requer `onnxruntime-gpu` e uma instalação CUDA funcionando. Se não estiverem presentes, o construtor reportará erro. Você pode pular este exercício e continuar com a variante CPU.

#### Referência de Compilação por Hardware

| Alvo | Provedor de Execução (`-e`) | Precisão Recomendada (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` ou `int4` |
| DirectML (GPU Windows) | `dml` | `fp16` ou `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Compromissos de Precisão

| Precisão | Tamanho | Velocidade | Qualidade |
|-----------|------|-------|---------|
| `fp32` | Maior | Mais lenta | Maior precisão |
| `fp16` | Grande | Rápida (GPU) | Muito boa precisão |
| `int8` | Pequeno | Rápida | Pequena perda de precisão |
| `int4` | Menor | Mais rápida | Perda moderada de precisão |

Para a maior parte do desenvolvimento local, `int4` em CPU oferece o melhor equilíbrio entre velocidade e uso de recursos. Para qualidade de produção, `fp16` em uma GPU CUDA é recomendado.

---

### Exercício 4: Criar a Configuração do Template de Chat

O construtor gera automaticamente um arquivo `chat_template.jinja` e um `genai_config.json` no diretório de saída. Porém, o Foundry Local também precisa de um arquivo `inference_model.json` para entender como formatar os prompts para seu modelo. Esse arquivo define o nome do modelo e o template de prompt que envolve as mensagens do usuário nos tokens especiais corretos.

#### Passo 1: Inspecionar a Saída Compilada

Liste o conteúdo do diretório do modelo compilado:

```bash
ls models/qwen3
```

Você deverá ver arquivos como:
- `model.onnx` e `model.onnx.data` — pesos compilados do modelo
- `genai_config.json` — configuração ONNX Runtime GenAI (auto-gerada)
- `chat_template.jinja` — template de chat do modelo (auto-gerado)
- `tokenizer.json`, `tokenizer_config.json` — arquivos do tokenizador
- Vários outros arquivos de configuração e vocabulário

#### Passo 2: Gerar o Arquivo inference_model.json

O arquivo `inference_model.json` informa ao Foundry Local como formatar os prompts. Crie um script Python chamado `generate_chat_template.py` **na raiz do repositório** (o mesmo diretório que contém sua pasta `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Construa uma conversa mínima para extrair o modelo de chat
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# Construa a estrutura do inference_model.json
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```

Execute o script a partir da raiz do repositório:

```bash
python generate_chat_template.py
```

> **Nota:** O pacote `transformers` já foi instalado como dependência do `onnxruntime-genai`. Se ver um `ImportError`, execute `pip install transformers` antes.

O script cria o arquivo `inference_model.json` dentro do diretório `models/qwen3`. Esse arquivo instruirá o Foundry Local a envolver a entrada do usuário nos tokens especiais corretos para o Qwen3.

> **Importante:** O campo `"Name"` em `inference_model.json` (definido como `qwen3-0.6b` neste script) é o **alias do modelo** que você usará em todos os comandos e chamadas API subsequentes. Se alterar esse nome, atualize o nome do modelo nos Exercícios 6–10 conforme necessário.

#### Passo 3: Verificar a Configuração

Abra `models/qwen3/inference_model.json` e confirme que contém um campo `Name` e um objeto `PromptTemplate` com as chaves `assistant` e `prompt`. O template do prompt deve incluir tokens especiais como `<|im_start|>` e `<|im_end|>` (os tokens exatos dependem do template de chat do modelo).

> **Alternativa manual:** Caso prefira não executar o script, pode criar o arquivo manualmente. O requisito principal é que o campo `prompt` contenha o template completo de chat do modelo com `{Content}` como espaço reservado para a mensagem do usuário.

---

### Exercício 5: Verificar a Estrutura do Diretório do Modelo
O construtor de modelos coloca todos os arquivos compilados diretamente no diretório de saída que você especificou. Verifique se a estrutura final está correta:

```bash
ls models/qwen3
```

O diretório deve conter os seguintes arquivos:

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **Nota:** Diferentemente de algumas outras ferramentas de compilação, o construtor de modelos não cria subdiretórios aninhados. Todos os arquivos ficam diretamente na pasta de saída, que é exatamente o que o Foundry Local espera.

---

### Exercício 6: Adicione o Modelo ao Cache do Foundry Local

Diga ao Foundry Local onde encontrar seu modelo compilado adicionando o diretório ao seu cache:

```bash
foundry cache cd models/qwen3
```

Verifique se o modelo aparece no cache:

```bash
foundry cache ls
```

Você deve ver seu modelo personalizado listado junto com quaisquer modelos previamente armazenados no cache (como `phi-3.5-mini` ou `phi-4-mini`).

---

### Exercício 7: Execute o Modelo Personalizado com o CLI

Inicie uma sessão de chat interativa com seu modelo recém-compilado (o alias `qwen3-0.6b` vem do campo `Name` que você definiu em `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

A flag `--verbose` mostra informações adicionais de diagnóstico, o que é útil ao testar um modelo personalizado pela primeira vez. Se o modelo for carregado com sucesso, você verá um prompt interativo. Experimente algumas mensagens:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Digite `exit` ou pressione `Ctrl+C` para encerrar a sessão.

> **Solução de problemas:** Se o modelo não carregar, verifique o seguinte:
> - O arquivo `genai_config.json` foi gerado pelo construtor de modelos.
> - O arquivo `inference_model.json` existe e é um JSON válido.
> - Os arquivos do modelo ONNX estão no diretório correto.
> - Você tem RAM disponível suficiente (Qwen3-0.6B int4 precisa de aproximadamente 1 GB).
> - Qwen3 é um modelo de raciocínio que produz tags `<think>`. Se você vir `<think>...</think>` prefixado às respostas, isso é um comportamento normal. O template do prompt em `inference_model.json` pode ser ajustado para suprimir a saída do pensamento.

---

### Exercício 8: Consulte o Modelo Personalizado via API REST

Se você saiu da sessão interativa no Exercício 7, o modelo pode não estar mais carregado. Primeiro, inicie o serviço Foundry Local e carregue o modelo:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Verifique em qual porta o serviço está rodando:

```bash
foundry service status
```

Então envie uma requisição (substitua `5273` pela sua porta real, se for diferente):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Nota para Windows:** O comando `curl` acima usa sintaxe bash. No Windows, use o cmdlet `Invoke-RestMethod` do PowerShell abaixo.

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### Exercício 9: Use o Modelo Personalizado com o SDK OpenAI

Você pode conectar-se ao seu modelo personalizado usando exatamente o mesmo código SDK OpenAI que usou para os modelos embutidos (veja [Parte 3](part3-sdk-and-apis.md)). A única diferença é o nome do modelo.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # O Foundry Local não valida chaves de API
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local não valida chaves de API
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **Ponto chave:** Como o Foundry Local expõe uma API compatível com OpenAI, qualquer código que funciona com os modelos embutidos também funciona com seus modelos personalizados. Você só precisa alterar o parâmetro `model`.

---

### Exercício 10: Teste o Modelo Personalizado com o SDK Foundry Local

Nos laboratórios anteriores você usou o SDK Foundry Local para iniciar o serviço, descobrir o endpoint e gerenciar modelos automaticamente. Você pode seguir exatamente o mesmo padrão com seu modelo compilado personalizado. O SDK cuida da inicialização do serviço e da descoberta do endpoint, então seu código não precisa codificar a porta `localhost:5273`.

> **Nota:** Certifique-se de que o SDK Foundry Local está instalado antes de executar estes exemplos:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Adicione os pacotes NuGet `Microsoft.AI.Foundry.Local` e `OpenAI`
>
> Salve cada arquivo script **na raiz do repositório** (no mesmo diretório da sua pasta `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Passo 1: Inicie o serviço Foundry Local e carregue o modelo personalizado
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Passo 2: Verifique o cache para o modelo personalizado
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Passo 3: Carregue o modelo na memória
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Passo 4: Crie um cliente OpenAI usando o endpoint descoberto pelo SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Passo 5: Envie uma solicitação de conclusão de chat em streaming
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

Execute-o:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// Etapa 1: Inicie o serviço Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Etapa 2: Obtenha o modelo personalizado do catálogo
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Etapa 3: Carregue o modelo na memória
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Etapa 4: Crie um cliente OpenAI usando o endpoint descoberto pelo SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Etapa 5: Envie uma solicitação de conclusão de chat em streaming
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

Execute-o:

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **Ponto chave:** O SDK Foundry Local descobre o endpoint dinamicamente, então você nunca codifica um número de porta manualmente. Esta é a abordagem recomendada para aplicações em produção. Seu modelo compilado personalizado funciona de forma idêntica aos modelos do catálogo embutido por meio do SDK.

---

## Escolhendo um Modelo para Compilar

Qwen3-0.6B é usado como exemplo de referência neste laboratório porque é pequeno, rápido para compilar e disponibilizado gratuitamente sob a licença Apache 2.0. No entanto, você pode compilar muitos outros modelos. Aqui estão algumas sugestões:

| Modelo | ID do Hugging Face | Parâmetros | Licença | Observações |
|--------|--------------------|------------|---------|-------------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Muito pequeno, compilação rápida, bom para testes |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Qualidade melhor, ainda rápido para compilar |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Alta qualidade, necessita mais RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Exige aceitação de licença no Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Alta qualidade, download maior e compilação mais longa |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Já está no catálogo Foundry Local (útil para comparação) |

> **Lembrete de licença:** Sempre verifique a licença do modelo no Hugging Face antes de usá-lo. Alguns modelos (como Llama) exigem que você aceite um acordo de licença e autentique com `huggingface-cli login` antes de baixar.

---

## Conceitos: Quando Usar Modelos Personalizados

| Cenário | Por que Compilar Seu Próprio? |
|---------|-------------------------------|
| **O modelo que você precisa não está no catálogo** | O catálogo Foundry Local é curado. Se o modelo desejado não está listado, compile você mesmo. |
| **Modelos afinados (fine-tuned)** | Se você afinou um modelo com dados específicos de domínio, precisa compilar seus próprios pesos. |
| **Requisitos específicos de quantização** | Você pode querer uma precisão ou estratégia de quantização diferente do padrão do catálogo. |
| **Lançamentos de modelos mais recentes** | Quando um novo modelo é lançado no Hugging Face, pode não estar ainda no catálogo Foundry Local. Compilá-lo você mesmo dá acesso imediato. |
| **Pesquisa e experimentação** | Testar diferentes arquiteturas, tamanhos ou configurações localmente antes de escolher uma opção para produção. |

---

## Resumo

Neste laboratório você aprendeu a:

| Etapa | O que Você Fez |
|-------|----------------|
| 1 | Instalou o construtor de modelos ONNX Runtime GenAI |
| 2 | Compilou `Qwen/Qwen3-0.6B` do Hugging Face em um modelo ONNX otimizado |
| 3 | Criou um arquivo de configuração de template de chat `inference_model.json` |
| 4 | Adicionou o modelo compilado ao cache do Foundry Local |
| 5 | Executou uma conversa interativa com o modelo personalizado via CLI |
| 6 | Consultou o modelo pela API REST compatível com OpenAI |
| 7 | Conectou-se a partir de Python, JavaScript e C# usando o SDK OpenAI |
| 8 | Testou o modelo personalizado de ponta a ponta com o SDK Foundry Local |

A principal conclusão é que **qualquer modelo baseado em transformador pode rodar via Foundry Local** uma vez que tenha sido compilado para o formato ONNX. A API compatível com OpenAI significa que todo seu código de aplicação existente funciona sem alterações; só é necessário trocar o nome do modelo.

---

## Principais Lições

| Conceito | Detalhe |
|----------|---------|
| Construtor ONNX Runtime GenAI | Converte modelos Hugging Face para ONNX com quantização em um único comando |
| Formato ONNX | Foundry Local requer modelos ONNX com configuração ONNX Runtime GenAI |
| Templates de chat | O arquivo `inference_model.json` informa ao Foundry Local como formatar prompts para um modelo específico |
| Destinos de hardware | Compile para CPU, GPU NVIDIA (CUDA), DirectML (GPU no Windows) ou WebGPU dependendo do seu hardware |
| Quantização | Precisão inferior (int4) reduz tamanho e melhora velocidade com custo em precisão; fp16 mantém alta qualidade em GPUs |
| Compatibilidade de API | Modelos personalizados usam a mesma API compatível com OpenAI dos modelos embutidos |
| SDK Foundry Local | O SDK lida automaticamente com inicialização do serviço, descoberta de endpoint e carregamento de modelos tanto do catálogo quanto personalizados |

---

## Leituras Complementares

| Recurso | Link |
|---------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Guia de modelos personalizados Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Família de modelos Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Documentação Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Próximos Passos

Continue para [Parte 11: Chamada de Ferramentas com Modelos Locais](part11-tool-calling.md) para aprender como habilitar seus modelos locais a chamar funções externas.

[← Parte 9: Transcrição de Voz Whisper](part9-whisper-voice-transcription.md) | [Parte 11: Chamada de Ferramentas →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:  
Este documento foi traduzido usando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos para garantir a precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original em seu idioma nativo deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se tradução profissional feita por um humano. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->