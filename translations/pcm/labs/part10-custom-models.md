![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 10: Using Custom or Hugging Face Models with Foundry Local

> **Goal:** Compile one Hugging Face model into di optimised ONNX format wey Foundry Local need, configure am with chat template, add am to di local cache, and run inference against am using di CLI, REST API, and OpenAI SDK.

## Overview

Foundry Local dey come wit curated catalogue of pre-compiled models, but you no get limit to dat list. Any transformer-based language model wey dey available for [Hugging Face](https://huggingface.co/) (or wey dey stored locally for PyTorch / Safetensors format) fit compile into one optimised ONNX model and serve through Foundry Local.

Di compilation pipeline dey use di **ONNX Runtime GenAI Model Builder**, na command-line tool wey dey part of di `onnxruntime-genai` package. Di model builder na im dey do di heavy work: downloading di source weights, converting dem to ONNX format, applying quantisation (int4, fp16, bf16), and emitting di configuration files (include di chat template and tokeniser) wey Foundry Local dey expect.

For dis lab you go compile **Qwen/Qwen3-0.6B** from Hugging Face, register am with Foundry Local, and chat wit am completely for your device.

---

## Learning Objectives

By di end of dis lab you go fit:

- Explain why custom model compilation dey useful and wen you fit need am
- Install di ONNX Runtime GenAI model builder
- Compile one Hugging Face model to optimised ONNX format wit one single command
- Understand di key compilation parameters (execution provider, precision)
- Create di `inference_model.json` chat-template configuration file
- Add compiled model to Foundry Local cache
- Run inference again di custom model using di CLI, REST API, and OpenAI SDK

---

## Prerequisites

| Requirement | Details |
|-------------|---------|
| **Foundry Local CLI** | Installed and dey your `PATH` ([Part 1](part1-getting-started.md)) |
| **Python 3.10+** | Na im ONNX Runtime GenAI model builder want |
| **pip** | Python package manager |
| **Disk space** | At least 5 GB space free for di source and compiled model files |
| **Hugging Face account** | Some models go require say you accept licence before you fit download dem. Qwen3-0.6B dey under Apache 2.0 licence and e dey freely available. |

---

## Environment Setup

Model compilation need some big Python packages (PyTorch, ONNX Runtime GenAI, Transformers). Make you create dedicated virtual environment so dat dem no go affect your system Python or other projects.

```bash
# From di repository root
python -m venv .venv
```

Activate di environment:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Upgrade pip make you no get dependency wahala:

```bash
python -m pip install --upgrade pip
```

> **Tip:** If you don get `.venv` from earlier labs, you fit still use am. Just make sure sey e dey activated before you continue.

---

## Concept: The Compilation Pipeline

Foundry Local want models for ONNX format wit ONNX Runtime GenAI configuration. Most open-source models wey dey for Hugging Face na PyTorch or Safetensors weights dem dey distribute, so you go need convert dem.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### Wetin Di Model Builder Dey Do?

1. **Download** di source model from Hugging Face (or read am from local path).
2. **Convert** di PyTorch / Safetensors weights to ONNX format.
3. **Quantise** di model to smaller precision (for example, int4) to reduce memory use and improve throughput.
4. **Emit** di ONNX Runtime GenAI configuration (`genai_config.json`), di chat template (`chat_template.jinja`), and all tokeniser files make Foundry Local fit load and serve di model.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

You fit sabi **Microsoft Olive** as another tool for model optimisation. Both tools fit make ONNX models, but dem get different use cases and trade-offs:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Package** | `onnxruntime-genai` | `olive-ai` |
| **Primary purpose** | Convert and quantise generative AI models for ONNX Runtime GenAI inference | End-to-end model optimisation framework wey support many backends and hardware |
| **Ease of use** | One command — one-step conversion + quantisation | Workflow-based — configurable multi-pass pipelines wit YAML/JSON |
| **Output format** | ONNX Runtime GenAI format (ready for Foundry Local) | Generic ONNX, ONNX Runtime GenAI, or other formats depending on workflow |
| **Hardware targets** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, and more |
| **Quantisation options** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus graph optimisations, layer-wise tuning |
| **Model scope** | Generative AI models (LLMs, SLMs) | Any ONNX-convertible model (vision, NLP, audio, multimodal) |
| **Best for** | Quick single-model compilation for local inference | Production pipelines wey need fine-grained optimisation control |
| **Dependency footprint** | Moderate (PyTorch, Transformers, ONNX Runtime) | Larger (add Olive framework, optional extras per workflow) |
| **Foundry Local integration** | Direct — output dey immediately compatible | Need `--use_ort_genai` flag and add extra configuration |

> **Why dis lab dey use di Model Builder:** To compile one single Hugging Face model and register am with Foundry Local, di Model Builder na di simplest and most reliable way. E dey give di exact output format wey Foundry Local want with one single command. If later you need advanced optimisation features like accuracy-aware quantisation, graph surgery, or multi-pass tuning — Olive na better option. Check di [Microsoft Olive documentation](https://microsoft.github.io/Olive/) for more info.

---

## Lab Exercises

### Exercise 1: Install the ONNX Runtime GenAI Model Builder

Install di ONNX Runtime GenAI package wey get di model builder tool:

```bash
pip install onnxruntime-genai
```

Verify di installation to see if di model builder dey available:

```bash
python -m onnxruntime_genai.models.builder --help
```

You go see help output wey list parameters like `-m` (model name), `-o` (output path), `-p` (precision), and `-e` (execution provider).

> **Note:** Di model builder depend on PyTorch, Transformers, and other packages. Installation fit take small time.

---

### Exercise 2: Compile Qwen3-0.6B for CPU

Run dis command to download Qwen3-0.6B model from Hugging Face and compile am for CPU with int4 quantisation:

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

#### Wetin Each Parameter Mean

| Parameter | Purpose | Value Wey Dem Use |
|-----------|---------|------------|
| `-m` | Hugging Face model ID or local directory path | `Qwen/Qwen3-0.6B` |
| `-o` | Directory where compiled ONNX model go save | `models/qwen3` |
| `-p` | Quantisation precision wey dem apply during compilation | `int4` |
| `-e` | ONNX Runtime execution provider (hardware target) | `cpu` |
| `--extra_options hf_token=false` | Skip Hugging Face authentication (good for public models) | `hf_token=false` |

> **How long e go take?** Di compilation time depend on your hardware and di model size. For Qwen3-0.6B wit int4 quantisation on modern CPU, e fit take about 5 to 15 minutes. Big models go take longer.

After command finish, you go see `models/qwen3` directory wit compiled model files. Verify di output:

```bash
ls models/qwen3
```

You go see files like:
- `model.onnx` and `model.onnx.data` — compiled model weights
- `genai_config.json` — ONNX Runtime GenAI configuration
- `chat_template.jinja` — model chat template (auto-generated)
- `tokenizer.json`, `tokenizer_config.json` — tokeniser files
- Other vocabulary and configuration files

---

### Exercise 3: Compile for GPU (Optional)

If you get NVIDIA GPU wit CUDA support, you fit compile GPU-optimised variant for faster inference:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Note:** GPU compilation need `onnxruntime-gpu` and working CUDA installation. If dem no dey, model builder go give error. You fit skip dis exercise and continue wit CPU variant.

#### Hardware-Specific Compilation Reference

| Target | Execution Provider (`-e`) | Recommended Precision (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` or `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` or `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Precision Trade-offs

| Precision | Size | Speed | Quality |
|-----------|------|-------|---------|
| `fp32` | Biggest | Slowest | Highest accuracy |
| `fp16` | Big | Fast (GPU) | Very good accuracy |
| `int8` | Small | Fast | Small accuracy loss |
| `int4` | Smallest | Fastest | Moderate accuracy loss |

For local development, `int4` for CPU gives best balance of speed and resource use. For production-quality output, `fp16` on CUDA GPU dey recommend.

---

### Exercise 4: Create the Chat Template Configuration

Model builder dey automatically generate `chat_template.jinja` and `genai_config.json` files for output directory. But Foundry Local still need `inference_model.json` file to know how to format prompts for your model. Dis file dey define model name and prompt template wey wrap user messages with correct special tokens.

#### Step 1: Inspect Compiled Output

List contents of compiled model directory:

```bash
ls models/qwen3
```

You go see files like:
- `model.onnx` and `model.onnx.data` — compiled model weights
- `genai_config.json` — ONNX Runtime GenAI config (auto-generated)
- `chat_template.jinja` — model chat template (auto-generated)
- `tokenizer.json`, `tokenizer_config.json` — tokeniser files
- Various other config and vocabulary files

#### Step 2: Generate inference_model.json File

`inference_model.json` file dey tell Foundry Local how to format prompts. Make Python script called `generate_chat_template.py` **for di repository root** (same directory wey get your `models/` folder):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Build small small talk to commot the chat template
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

# Build the inference_model.json structure
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

Run di script from repository root:

```bash
python generate_chat_template.py
```

> **Note:** `transformers` package already install as dependency of `onnxruntime-genai`. If you get `ImportError`, run `pip install transformers` first.

Di script go produce `inference_model.json` inside `models/qwen3` directory. Di file dey tell Foundry Local how to wrap user input wit correct special tokens for Qwen3.

> **Important:** Di `"Name"` field for `inference_model.json` (wey this script set to `qwen3-0.6b`) na **model alias** wey you go dey use for all other commands and API calls. If you change dis name, update di model name for Exercises 6–10 too.

#### Step 3: Verify Configuration

Open `models/qwen3/inference_model.json` make sure sey e get `Name` field and one `PromptTemplate` object with `assistant` and `prompt` keys. Di prompt template suppose get special tokens like `<|im_start|>` and `<|im_end|>` (di correct tokens depend on model chat template).

> **Manual alternative:** If you no want run di script, you fit create di file yourself. Di main thing be say di `prompt` field suppose get di full chat template with `{Content}` as placeholder for user message.

---

### Exercise 5: Verify the Model Directory Structure
Di model builder dey put all di compiled files directly inside di output directory wey you specify. Check say di final structure dey correct:

```bash
ls models/qwen3
```

Di directory suppose get di following files:

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

> **Note:** No be like some oda compilation tools, di model builder no dey create nested subdirectories. All files just dey inside di output folder, wey na exactly wetin Foundry Local dey expect.

---

### Exercise 6: Add di Model to di Foundry Local Cache

Tell Foundry Local weh e go find your compiled model by adding di directory to im cache:

```bash
foundry cache cd models/qwen3
```

Check say di model show for di cache:

```bash
foundry cache ls
```

You go see your custom model dey listed together wit any models wey dem don put for cache before (like `phi-3.5-mini` or `phi-4-mini`).

---

### Exercise 7: Run di Custom Model wit di CLI

Start interactive chat session wit your newly compiled model (di `qwen3-0.6b` alias na from di `Name` field wey you set for `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Di `--verbose` flag dey show extra diagnostic information, wey good when you dey test custom model for di first time. If di model load well you go see interactive prompt. Try send some messages:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Type `exit` or press `Ctrl+C` to end di session.

> **Troubleshooting:** If di model no fit load, check the following:
> - Di `genai_config.json` file na model builder create am.
> - Di `inference_model.json` file dey and na valid JSON.
> - Di ONNX model files dey for di correct directory.
> - You get enough RAM (Qwen3-0.6B int4 need like 1 GB).
> - Qwen3 na reasoning model wey dey produce `<think>` tags. If you see `<think>...</think>` inside di responses, dis na normal behavior. You fit adjust di prompt template for `inference_model.json` to stop dis thinking output.

---

### Exercise 8: Query di Custom Model via di REST API

If you comot for di interactive session for Exercise 7, di model fit no dey loaded again. Start di Foundry Local service and load di model first:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Check which port di service dey run:

```bash
foundry service status
```

Then send request (change `5273` to your port if e different):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows note:** Di `curl` command wey dey top na bash syntax. For Windows, use PowerShell `Invoke-RestMethod` cmdlet below instead.

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

### Exercise 9: Use di Custom Model wit di OpenAI SDK

You fit connect to your custom model using exactly di same OpenAI SDK code wey you use for di built-in models (see [Part 3](part3-sdk-and-apis.md)). Di only difference na di model name.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local no dey check API keys
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
  apiKey: "foundry-local", // Foundry Local no dey check API keys validate
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

> **Key point:** Because Foundry Local dey expose OpenAI-compatible API, any code wey dey work wit built-in models go still work wit your custom models. You only need change di `model` parameter.

---

### Exercise 10: Test di Custom Model wit di Foundry Local SDK

For earlier labs you use di Foundry Local SDK to start di service, discover di endpoint, and manage models automatically. You fit follow di same pattern wit your custom-compiled model. Di SDK dey handle service startup and endpoint discovery, so your code no need hard-code `localhost:5273`.

> **Note:** Make sure say di Foundry Local SDK don install before you run these examples:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Add `Microsoft.AI.Foundry.Local` and `OpenAI` NuGet packages
>
> Save every script file **inside di repository root** (di same directory as your `models/` folder).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Step 1: Start di Foundry Local service and load di custom model
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Step 2: Check di cache for di custom model
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Step 3: Load di model into memory
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Step 4: Create one OpenAI client using di SDK-discovered endpoint
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Step 5: Send one streaming chat completion request
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

Run am:

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

// Step 1: Start di Foundry Local service
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Step 2: Grab di custom model from di catalog
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Step 3: Load di model enter memory
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Step 4: Make OpenAI client usin di SDK-discovered endpoint
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Step 5: Send a streaming chat completion request
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

Run am:

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

> **Key point:** Di Foundry Local SDK dey find di endpoint dynamically, so you no go hard-code port number. Dis na di recommended way for production applications. Your custom-compiled model dey work exactly like built-in catalogue models through di SDK.

---

## Choosing a Model to Compile

Qwen3-0.6B na di example model wey we use for dis lab because e small, fast to compile, and e dey free under di Apache 2.0 licence. But you fit compile many oda models too. Here be some suggestions:

| Model | Hugging Face ID | Parameters | Licence | Notes |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Very small, fast compilation, good for testing |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Better quality, still fast to compile |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Strong quality, needs more RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Need licence acceptance for Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | High quality, bigger download and longer compile time |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Already inside Foundry Local catalogue (good for comparison) |

> **Licence reminder:** Always check di model licence for Hugging Face before you use am. Some models (like Llama) require you to accept licence agreement and authenticate with `huggingface-cli login` before downloading.

---

## Concepts: When to Use Custom Models

| Scenario | Why Compile Your Own? |
|----------|----------------------|
| **If model wey you need no dey for catalogue** | Foundry Local catalogue dey curated. If di model wey you want no show, compile am yourself. |
| **Fine-tuned models** | If you do fine-tune model on domain-specific data, you need compile your own weights. |
| **Specific quantisation requirements** | You fit want precision or quantisation method wey different from catalogue default. |
| **Newer model releases** | When new model drop for Hugging Face, e fit no dey inside Foundry Local catalogue yet. If you compile am yourself, you go fit use am immediately. |
| **Research and experimentation** | Try different model architectures, sizes, or setups locally before you decide for production. |

---

## Summary

For dis lab you learn how to:

| Step | Wetin You Do |
|------|--------------|
| 1 | Install ONNX Runtime GenAI model builder |
| 2 | Compile `Qwen/Qwen3-0.6B` from Hugging Face to optimised ONNX model |
| 3 | Create `inference_model.json` chat-template configuration file |
| 4 | Add compiled model to Foundry Local cache |
| 5 | Run interactive chat wit custom model via CLI |
| 6 | Query model through OpenAI-compatible REST API |
| 7 | Connect from Python, JavaScript, and C# using OpenAI SDK |
| 8 | Test custom model end-to-end with Foundry Local SDK |

Di important thing be say **any transformer-based model fit run through Foundry Local** once e don compile to ONNX format. Di OpenAI-compatible API mean say all your current application code go work without any changes; you just need change di model name.

---

## Key Takeaways

| Concept | Detail |
|---------|--------|
| ONNX Runtime GenAI Model Builder | Converts Hugging Face models to ONNX format wit quantisation in one command |
| ONNX format | Foundry Local require ONNX models wit ONNX Runtime GenAI config |
| Chat templates | `inference_model.json` tells Foundry Local how to format prompts for model |
| Hardware targets | Compile for CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), or WebGPU depending on your hardware |
| Quantisation | Lower precision (int4) reduce size and increase speed but small drop for accuracy; fp16 maintain better quality for GPUs |
| API compatibility | Custom models use same OpenAI-compatible API as built-in models |
| Foundry Local SDK | SDK handle service startup, endpoint discovery, and model loading automatically for both catalogue and custom models |

---

## Further Reading

| Resource | Link |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local custom model guide | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 model family | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive documentation | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Next Steps

Continue to [Part 11: Tool Calling with Local Models](part11-tool-calling.md) to learn how to enable your local models to call external functions.

[← Part 9: Whisper Voice Transcription](part9-whisper-voice-transcription.md) | [Part 11: Tool Calling →](part11-tool-calling.md)