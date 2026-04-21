![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 10: Utilizar Modelos Personalizados ou Hugging Face com Foundry Local

> **Objetivo:** Compilar um modelo Hugging Face no formato ONNX otimizado que o Foundry Local requer, configurá-lo com um template de chat, adicioná-lo ao cache local e executar inferência usando CLI, API REST e SDK OpenAI.

## Visão Geral

O Foundry Local inclui um catálogo selecionado de modelos pré-compilados, mas não está limitado a essa lista. Qualquer modelo de linguagem baseado em transformadores disponível no [Hugging Face](https://huggingface.co/) (ou armazenado localmente em formato PyTorch / Safetensors) pode ser compilado num modelo ONNX otimizado e servido através do Foundry Local.

O pipeline de compilação usa o **ONNX Runtime GenAI Model Builder**, uma ferramenta de linha de comandos incluída no pacote `onnxruntime-genai`. O model builder trata do trabalho complexo: descarrega os pesos da fonte, converte para o formato ONNX, aplica quantização (int4, fp16, bf16) e gera os ficheiros de configuração (incluindo o template de chat e o tokenizador) que o Foundry Local espera.

Neste laboratório vai compilar o **Qwen/Qwen3-0.6B** do Hugging Face, registá-lo no Foundry Local e conversar com ele inteiramente no seu dispositivo.

---

## Objetivos de Aprendizagem

No final deste laboratório será capaz de:

- Explicar por que a compilação de modelos personalizados é útil e quando pode ser necessária
- Instalar o ONNX Runtime GenAI model builder
- Compilar um modelo Hugging Face para o formato ONNX otimizado com um único comando
- Compreender os principais parâmetros de compilação (execution provider, precisão)
- Criar o ficheiro de configuração `inference_model.json` para o template de chat
- Adicionar um modelo compilado ao cache do Foundry Local
- Executar inferência contra o modelo personalizado usando CLI, API REST e SDK OpenAI

---

## Pré-requisitos

| Requisito | Detalhes |
|-------------|---------|
| **Foundry Local CLI** | Instalado e no seu `PATH` ([Parte 1](part1-getting-started.md)) |
| **Python 3.10+** | Requerido pelo ONNX Runtime GenAI model builder |
| **pip** | Gestor de pacotes Python |
| **Espaço em disco** | Pelo menos 5 GB livres para os ficheiros do modelo fonte e compilado |
| **Conta Hugging Face** | Alguns modelos requerem que aceite uma licença antes do download. Qwen3-0.6B usa a licença Apache 2.0 e está disponível gratuitamente. |

---

## Configuração do Ambiente

A compilação do modelo requer vários pacotes Python grandes (PyTorch, ONNX Runtime GenAI, Transformers). Crie um ambiente virtual dedicado para que estes não interfiram com o Python do sistema ou outros projetos.

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

> **Dica:** Se já tiver uma `.venv` dos laboratórios anteriores, pode reutilizá-la. Só precisa garantir que está ativada antes de continuar.

---

## Conceito: O Pipeline de Compilação

O Foundry Local requer modelos no formato ONNX com configuração ONNX Runtime GenAI. A maioria dos modelos open-source no Hugging Face é distribuída como pesos PyTorch ou Safetensors, pelo que é necessário um passo de conversão.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### O que faz o Model Builder?

1. **Descarrega** o modelo fonte do Hugging Face (ou lê a partir de um caminho local).
2. **Converte** os pesos PyTorch / Safetensors para o formato ONNX.
3. **Quantiza** o modelo para uma precisão menor (por exemplo, int4) para reduzir o uso de memória e melhorar o throughput.
4. **Gera** a configuração do ONNX Runtime GenAI (`genai_config.json`), o template de chat (`chat_template.jinja`) e todos os ficheiros do tokenizador para que o Foundry Local possa carregar e servir o modelo.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Poderá encontrar referências ao **Microsoft Olive** como uma ferramenta alternativa para otimização de modelos. Ambas as ferramentas podem produzir modelos ONNX, mas servem propósitos diferentes e apresentam diferentes compromissos:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Pacote** | `onnxruntime-genai` | `olive-ai` |
| **Objetivo principal** | Converter e quantizar modelos de IA generativa para inferência ONNX Runtime GenAI | Framework de otimização de modelos end-to-end suportando múltiplos backends e hardware |
| **Facilidade de uso** | Comando único — conversão + quantização num só passo | Baseado em workflows — pipelines configuráveis multi-pass com YAML/JSON |
| **Formato de saída** | Formato ONNX Runtime GenAI (pronto para Foundry Local) | ONNX genérico, ONNX Runtime GenAI ou outros, dependendo do workflow |
| **Targets de hardware** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN e mais |
| **Opções de quantização** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, além de otimizações de grafo, ajuste por camadas |
| **Escopo do modelo** | Modelos de IA generativa (LLMs, SLMs) | Qualquer modelo conversível para ONNX (visão, NLP, áudio, multimodal) |
| **Indicado para** | Compilação rápida de modelo único para inferência local | Pipelines de produção precisando de controlo fino da otimização |
| **Pegada de dependências** | Moderada (PyTorch, Transformers, ONNX Runtime) | Maior (inclui o framework Olive, extras opcionais por workflow) |
| **Integração com Foundry Local** | Direta — saída é imediatamente compatível | Requer flag `--use_ort_genai` e configuração adicional |

> **Porque este laboratório usa o Model Builder:** Para a tarefa de compilar um único modelo Hugging Face e registá-lo no Foundry Local, o Model Builder é o caminho mais simples e fiável. Ele produz exatamente o formato de saída que o Foundry Local espera num único comando. Se mais tarde precisar de funcionalidades avançadas de otimização – como quantização consciente de precisão, cirurgia de grafos ou ajuste multi-pass – o Olive é uma opção poderosa para explorar. Consulte a [documentação Microsoft Olive](https://microsoft.github.io/Olive/) para mais detalhes.

---

## Exercícios do Laboratório

### Exercício 1: Instalar o ONNX Runtime GenAI Model Builder

Instale o pacote ONNX Runtime GenAI, que inclui a ferramenta model builder:

```bash
pip install onnxruntime-genai
```

Verifique a instalação confirmando que o model builder está disponível:

```bash
python -m onnxruntime_genai.models.builder --help
```

Deverá ver saída de ajuda listando parâmetros como `-m` (nome do modelo), `-o` (caminho de saída), `-p` (precisão) e `-e` (execution provider).

> **Nota:** O model builder depende de PyTorch, Transformers e vários outros pacotes. A instalação pode demorar alguns minutos.

---

### Exercício 2: Compilar Qwen3-0.6B para CPU

Execute o seguinte comando para descarregar o modelo Qwen3-0.6B do Hugging Face e compilá-lo para inferência CPU com quantização int4:

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

#### O que cada parâmetro faz

| Parâmetro | Propósito | Valor Utilizado |
|-----------|---------|------------|
| `-m` | ID do modelo Hugging Face ou caminho local para diretório | `Qwen/Qwen3-0.6B` |
| `-o` | Diretório onde o modelo ONNX compilado será guardado | `models/qwen3` |
| `-p` | Precisão da quantização aplicada durante compilação | `int4` |
| `-e` | Execution provider ONNX Runtime (hardware alvo) | `cpu` |
| `--extra_options hf_token=false` | Ignora autenticação Hugging Face (válido para modelos públicos) | `hf_token=false` |

> **Quanto tempo demora?** O tempo de compilação depende do seu hardware e do tamanho do modelo. Para Qwen3-0.6B com quantização int4 num CPU moderno, espere entre 5 a 15 minutos. Modelos maiores levam proporcionalmente mais tempo.

Após o comando terminar deve ver um diretório `models/qwen3` contendo os ficheiros do modelo compilado. Verifique a saída:

```bash
ls models/qwen3
```

Deverá ver ficheiros incluindo:
- `model.onnx` e `model.onnx.data` — pesos do modelo compilado
- `genai_config.json` — configuração ONNX Runtime GenAI
- `chat_template.jinja` — template de chat do modelo (gerado automaticamente)
- `tokenizer.json`, `tokenizer_config.json` — ficheiros do tokenizador
- Outros ficheiros de vocabulário e configuração

---

### Exercício 3: Compilar para GPU (Opcional)

Se tiver uma GPU NVIDIA com suporte CUDA, pode compilar uma variante otimizada para GPU para inferência mais rápida:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Nota:** A compilação para GPU requer `onnxruntime-gpu` e uma instalação CUDA funcional. Se estes não estiverem presentes, o model builder indicará um erro. Pode ignorar este exercício e continuar com a variante CPU.

#### Referência para compilação específica de hardware

| Alvo | Execution Provider (`-e`) | Precisão recomendada (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| GPU NVIDIA | `cuda` | `fp16` ou `int4` |
| DirectML (GPU Windows) | `dml` | `fp16` ou `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Compromissos de precisão

| Precisão | Tamanho | Velocidade | Qualidade |
|-----------|------|-------|---------|
| `fp32` | Maior | Mais lento | Maior precisão |
| `fp16` | Grande | Rápido (GPU) | Muito boa precisão |
| `int8` | Pequeno | Rápido | Perda ligeira de precisão |
| `int4` | Menor | Mais rápido | Perda moderada de precisão |

Para a maior parte do desenvolvimento local, `int4` em CPU oferece o melhor equilíbrio entre velocidade e uso de recursos. Para saída com qualidade de produção, `fp16` num GPU CUDA é recomendado.

---

### Exercício 4: Criar a Configuração do Template de Chat

O model builder gera automaticamente um ficheiro `chat_template.jinja` e um `genai_config.json` no diretório de saída. No entanto, o Foundry Local também precisa de um ficheiro `inference_model.json` para entender como formatar os prompts para o seu modelo. Este ficheiro define o nome do modelo e o template do prompt que envolve as mensagens do utilizador nos tokens especiais corretos.

#### Passo 1: Inspecionar a saída compilada

Liste o conteúdo do diretório do modelo compilado:

```bash
ls models/qwen3
```

Deverá ver ficheiros tais como:
- `model.onnx` e `model.onnx.data` — pesos do modelo compilado
- `genai_config.json` — configuração ONNX Runtime GenAI (gerado automaticamente)
- `chat_template.jinja` — template de chat do modelo (gerado automaticamente)
- `tokenizer.json`, `tokenizer_config.json` — ficheiros do tokenizador
- Vários outros ficheiros de configuração e vocabulário

#### Passo 2: Gerar o ficheiro inference_model.json

O ficheiro `inference_model.json` informa ao Foundry Local como formatar os prompts. Crie um script Python chamado `generate_chat_template.py` **na raiz do repositório** (o mesmo diretório que contém a pasta `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Construir uma conversa mínima para extrair o modelo de chat
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

# Construir a estrutura inference_model.json
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

> **Nota:** O pacote `transformers` já foi instalado como dependência do `onnxruntime-genai`. Se receber um `ImportError`, execute primeiro `pip install transformers`.

O script gera um ficheiro `inference_model.json` dentro do diretório `models/qwen3`. O ficheiro diz ao Foundry Local como envolver a entrada do utilizador nos tokens especiais corretos para o Qwen3.

> **Importante:** O campo `"Name"` no `inference_model.json` (definido para `qwen3-0.6b` neste script) é o **alias do modelo** que usará em todos os comandos e chamadas API seguintes. Se mudar este nome, atualize o nome do modelo nos Exercícios 6–10 em conformidade.

#### Passo 3: Verificar a configuração

Abra `models/qwen3/inference_model.json` e confirme que contém um campo `Name` e um objeto `PromptTemplate` com as chaves `assistant` e `prompt`. O template de prompt deve incluir tokens especiais como `<|im_start|>` e `<|im_end|>` (os tokens exatos dependem do template de chat do modelo).

> **Alternativa manual:** Se preferir não executar o script, pode criar o ficheiro manualmente. O requisito chave é que o campo `prompt` contenha o template completo de chat do modelo com `{Content}` como placeholder para a mensagem do utilizador.

---

### Exercício 5: Verificar a Estrutura do Diretório do Modelo
O construtor de modelos coloca todos os ficheiros compilados diretamente na diretoria de saída que especificou. Verifique se a estrutura final está correta:

```bash
ls models/qwen3
```

A diretoria deve conter os seguintes ficheiros:

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

> **Nota:** Ao contrário de algumas outras ferramentas de compilação, o construtor de modelos não cria subdiretórios aninhados. Todos os ficheiros ficam diretamente na pasta de saída, que é exatamente o que o Foundry Local espera.

---

### Exercício 6: Adicionar o Modelo ao Cache do Foundry Local

Diga ao Foundry Local onde encontrar o seu modelo compilado adicionando a diretoria ao seu cache:

```bash
foundry cache cd models/qwen3
```

Verifique se o modelo aparece no cache:

```bash
foundry cache ls
```

Deverá ver o seu modelo personalizado listado juntamente com quaisquer modelos previamente armazenados em cache (como `phi-3.5-mini` ou `phi-4-mini`).

---

### Exercício 7: Executar o Modelo Personalizado com a CLI

Inicie uma sessão de chat interativa com o seu modelo recém-compilado (o alias `qwen3-0.6b` vem do campo `Name` que configurou em `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

A opção `--verbose` mostra informações adicionais de diagnóstico, o que é útil ao testar um modelo personalizado pela primeira vez. Se o modelo carregar com sucesso, verá um prompt interativo. Experimente algumas mensagens:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Digite `exit` ou pressione `Ctrl+C` para terminar a sessão.

> **Resolução de problemas:** Se o modelo falhar ao carregar, verifique o seguinte:
> - O ficheiro `genai_config.json` foi gerado pelo construtor de modelos.
> - O ficheiro `inference_model.json` existe e é um JSON válido.
> - Os ficheiros do modelo ONNX estão na diretoria correta.
> - Tem RAM disponível suficiente (Qwen3-0.6B int4 necessita de aproximadamente 1 GB).
> - Qwen3 é um modelo de raciocínio que produz etiquetas `<think>`. Se vir `<think>...</think>` prefixado às respostas, este é um comportamento normal. O template do prompt em `inference_model.json` pode ser ajustado para suprimir a saída de pensamento.

---

### Exercício 8: Interrogar o Modelo Personalizado via API REST

Se saiu da sessão interativa no Exercício 7, o modelo pode já não estar carregado. Inicie o serviço Foundry Local e carregue primeiro o modelo:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Verifique em que porta o serviço está a correr:

```bash
foundry service status
```

Depois envie um pedido (substitua `5273` pela sua porta real, caso seja diferente):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Nota para Windows:** O comando `curl` acima usa sintaxe bash. No Windows, utilize em vez disso o cmdlet `Invoke-RestMethod` do PowerShell indicado abaixo.

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

### Exercício 9: Usar o Modelo Personalizado com o SDK do OpenAI

Pode ligar-se ao seu modelo personalizado usando exatamente o mesmo código do SDK OpenAI que usou para os modelos incorporados (veja [Parte 3](part3-sdk-and-apis.md)). A única diferença é o nome do modelo.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # O Foundry Local não valida as chaves API
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
  apiKey: "foundry-local", // O Foundry Local não valida chaves API
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

> **Ponto chave:** Como o Foundry Local expõe uma API compatível com OpenAI, qualquer código que funcione com os modelos incorporados também funciona com os seus modelos personalizados. Só precisa de mudar o parâmetro `model`.

---

### Exercício 10: Testar o Modelo Personalizado com o SDK do Foundry Local

Nos laboratórios anteriores usou o SDK do Foundry Local para iniciar o serviço, descobrir o endpoint e gerir modelos automaticamente. Pode seguir exatamente o mesmo padrão com o seu modelo compilado personalizado. O SDK trata da inicialização do serviço e da descoberta do endpoint, pelo que o seu código não precisa de codificar diretamente `localhost:5273`.

> **Nota:** Certifique-se de que o SDK do Foundry Local está instalado antes de executar estes exemplos:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Adicione os pacotes NuGet `Microsoft.AI.Foundry.Local` e `OpenAI`
>
> Guarde cada ficheiro de script **na raiz do repositório** (a mesma diretoria da sua pasta `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Passo 1: Iniciar o serviço Foundry Local e carregar o modelo personalizado
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Passo 2: Verificar a cache para o modelo personalizado
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Passo 3: Carregar o modelo na memória
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Passo 4: Criar um cliente OpenAI usando o endpoint descoberto pelo SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Passo 5: Enviar uma solicitação de conclusão de chat em streaming
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

// Passo 1: Iniciar o serviço Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Passo 2: Obter o modelo personalizado do catálogo
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Passo 3: Carregar o modelo na memória
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Passo 4: Criar um cliente OpenAI utilizando o endpoint descoberto pelo SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Passo 5: Enviar um pedido de conclusão de chat em streaming
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

> **Ponto chave:** O SDK do Foundry Local descobre dinamicamente o endpoint, assim nunca codifica um número de porta diretamente. Esta é a abordagem recomendada para aplicações de produção. O seu modelo compilado personalizado funciona de forma idêntica aos modelos do catálogo incorporados através do SDK.

---

## Escolha de um Modelo para Compilar

O Qwen3-0.6B é usado como exemplo de referência neste laboratório porque é pequeno, rápido de compilar e livremente disponível sob a licença Apache 2.0. No entanto, pode compilar muitos outros modelos. Aqui estão algumas sugestões:

| Modelo | ID do Hugging Face | Parâmetros | Licença | Notas |
|--------|--------------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Muito pequeno, compilação rápida, bom para testes |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Qualidade melhor, ainda rápido de compilar |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Qualidade forte, necessita mais RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Requer aceite de licença no Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Alta qualidade, download maior e compilação mais longa |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Já no catálogo Foundry Local (útil para comparação) |

> **Lembrete de licença:** Verifique sempre a licença do modelo no Hugging Face antes de o usar. Alguns modelos (como o Llama) exigem que aceite um acordo de licença e se autentique com `huggingface-cli login` antes de fazer o download.

---

## Conceitos: Quando Usar Modelos Personalizados

| Cenário | Porquê Compilar o Seu Próprio? |
|---------|-------------------------------|
| **Um modelo que precisa não está no catálogo** | O catálogo Foundry Local é curado. Se o modelo que deseja não está listado, compile-o você mesmo. |
| **Modelos ajustados (fine-tuned)** | Se afinou um modelo com dados específicos de domínio, precisa compilar os seus próprios pesos. |
| **Requisitos específicos de quantização** | Pode querer uma estratégia de precisão ou quantização diferente do padrão do catálogo. |
| **Novas versões de modelos** | Quando um modelo novo é lançado no Hugging Face, pode ainda não estar no catálogo Foundry Local. Compilá-lo dá acesso imediato. |
| **Pesquisa e experimentação** | Experimentar diferentes arquiteturas, tamanhos ou configurações localmente antes de escolher para produção. |

---

## Resumo

Neste laboratório aprendeu a:

| Passo | O que Fez |
|-------|-----------|
| 1 | Instalou o construtor de modelos ONNX Runtime GenAI |
| 2 | Compilou `Qwen/Qwen3-0.6B` do Hugging Face para um modelo ONNX optimizado |
| 3 | Criou um ficheiro de configuração de template de chat `inference_model.json` |
| 4 | Adicionou o modelo compilado ao cache do Foundry Local |
| 5 | Executou chat interativo com o modelo personalizado via CLI |
| 6 | Interrogou o modelo através da API REST compatível com OpenAI |
| 7 | Ligou-se a partir de Python, JavaScript e C# usando o SDK OpenAI |
| 8 | Testou o modelo personalizado de ponta a ponta com o SDK do Foundry Local |

A principal conclusão é que **qualquer modelo baseado em transformadores pode ser executado através do Foundry Local** assim que tiver sido compilado para o formato ONNX. A API compatível com OpenAI significa que todo o código da sua aplicação existente funciona sem alterações; só precisa de trocar o nome do modelo.

---

## Principais Conclusões

| Conceito | Detalhe |
|----------|---------|
| Construtor ONNX Runtime GenAI | Converte modelos do Hugging Face para formato ONNX com quantização num único comando |
| Formato ONNX | Foundry Local requer modelos ONNX com configuração ONNX Runtime GenAI |
| Templates de chat | O ficheiro `inference_model.json` indica ao Foundry Local como formatar prompts para um determinado modelo |
| Alvos de hardware | Compile para CPU, GPU NVIDIA (CUDA), DirectML (Windows GPU) ou WebGPU conforme o seu hardware |
| Quantização | Precisão inferior (int4) reduz tamanho e melhora velocidade à custa de alguma precisão; fp16 mantém alta qualidade em GPUs |
| Compatibilidade API | Modelos personalizados usam a mesma API compatível com OpenAI que os modelos incorporados |
| SDK do Foundry Local | O SDK trata da inicialização do serviço, descoberta de endpoints e carregamento automático de modelos tanto do catálogo como personalizados |

---

## Leitura Adicional

| Recurso | Ligação |
|---------|---------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Guia de modelos personalizados Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Família de modelos Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Documentação Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Próximos Passos

Continue para [Parte 11: Chamada de Ferramentas com Modelos Locais](part11-tool-calling.md) para aprender como permitir que os seus modelos locais chamem funções externas.

[← Parte 9: Transcrição de Voz Whisper](part9-whisper-voice-transcription.md) | [Parte 11: Chamada de Ferramentas →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Aviso Legal**:
Este documento foi traduzido utilizando o serviço de tradução por IA [Co-op Translator](https://github.com/Azure/co-op-translator). Embora nos esforcemos pela precisão, esteja ciente de que traduções automáticas podem conter erros ou imprecisões. O documento original na sua língua nativa deve ser considerado a fonte autorizada. Para informações críticas, recomenda-se tradução profissional humana. Não nos responsabilizamos por quaisquer mal-entendidos ou interpretações incorretas decorrentes do uso desta tradução.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->