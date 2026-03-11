![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Part 10: Using Custom or Hugging Face Models with Foundry Local

> **Goal:** Compile a Hugging Face model into the optimised ONNX format that Foundry Local requires, configure it with a chat template, add it to the local cache, and run inference against it using the CLI, REST API, and OpenAI SDK.

## Overview

Foundry Local ships with a curated catalogue of pre-compiled models, but you are not limited to that list. Any transformer-based language model available on [Hugging Face](https://huggingface.co/) (or stored locally in PyTorch / Safetensors format) can be compiled into an optimised ONNX model and served through Foundry Local.

The compilation pipeline uses the **ONNX Runtime GenAI Model Builder**, a command-line tool included with the `onnxruntime-genai` package. The model builder handles the heavy lifting: downloading the source weights, converting them to ONNX format, applying quantisation (int4, fp16, bf16), and emitting the configuration files (including the chat template and tokeniser) that Foundry Local expects.

In this lab you will compile **Qwen/Qwen3-0.6B** from Hugging Face, register it with Foundry Local, and chat with it entirely on your device.

---

## Learning Objectives

By the end of this lab you will be able to:

- Explain why custom model compilation is useful and when you might need it
- Install the ONNX Runtime GenAI model builder
- Compile a Hugging Face model to optimised ONNX format with a single command
- Understand the key compilation parameters (execution provider, precision)
- Create the `inference_model.json` chat-template configuration file
- Add a compiled model to the Foundry Local cache
- Run inference against the custom model using the CLI, REST API, and OpenAI SDK

---

## Prerequisites

| Requirement | Details |
|-------------|---------|
| **Foundry Local CLI** | Installed and on your `PATH` ([Part 1](part1-getting-started.md)) |
| **Python 3.10+** | Required by the ONNX Runtime GenAI model builder |
| **pip** | Python package manager |
| **Disk space** | At least 5 GB free for the source and compiled model files |
| **Hugging Face account** | Some models require you to accept a licence before downloading. Qwen3-0.6B uses the Apache 2.0 licence and is freely available. |

---

## Environment Setup

Model compilation requires several large Python packages (PyTorch, ONNX Runtime GenAI, Transformers). Create a dedicated virtual environment so these do not interfere with your system Python or other projects.

```bash
# From the repository root
python -m venv .venv
```

Activate the environment:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Upgrade pip to avoid dependency resolution issues:

```bash
python -m pip install --upgrade pip
```

> **Tip:** If you already have a `.venv` from earlier labs, you can reuse it. Just make sure it is activated before continuing.

---

## Concept: The Compilation Pipeline

Foundry Local requires models in ONNX format with ONNX Runtime GenAI configuration. Most open-source models on Hugging Face are distributed as PyTorch or Safetensors weights, so a conversion step is needed.

```
Source Model           Model Builder          Compiled Output
(Hugging Face)     ──────────────────►      (Optimised ONNX)

Qwen/Qwen3-0.6B    onnxruntime_genai        models/qwen3/
  - Safetensors     ─────────────►            - model.onnx
  - tokenizer        convert +               - model.onnx.data
  - config.json       quantise               - tokenizer files
                                              - genai_config.json
                                              - chat_template.jinja
```

### What Does the Model Builder Do?

1. **Downloads** the source model from Hugging Face (or reads it from a local path).
2. **Converts** the PyTorch / Safetensors weights to ONNX format.
3. **Quantises** the model to a smaller precision (for example, int4) to reduce memory usage and improve throughput.
4. **Emits** the ONNX Runtime GenAI configuration (`genai_config.json`), the chat template (`chat_template.jinja`), and all tokeniser files so that Foundry Local can load and serve the model.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

You may encounter references to **Microsoft Olive** as an alternative tool for model optimisation. Both tools can produce ONNX models, but they serve different purposes and have different trade-offs:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Package** | `onnxruntime-genai` | `olive-ai` |
| **Primary purpose** | Convert and quantise generative AI models for ONNX Runtime GenAI inference | End-to-end model optimisation framework supporting many backends and hardware targets |
| **Ease of use** | Single command — one-step conversion + quantisation | Workflow-based — configurable multi-pass pipelines with YAML/JSON |
| **Output format** | ONNX Runtime GenAI format (ready for Foundry Local) | Generic ONNX, ONNX Runtime GenAI, or other formats depending on workflow |
| **Hardware targets** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, and more |
| **Quantisation options** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus graph optimisations, layer-wise tuning |
| **Model scope** | Generative AI models (LLMs, SLMs) | Any ONNX-convertible model (vision, NLP, audio, multimodal) |
| **Best for** | Quick single-model compilation for local inference | Production pipelines needing fine-grained optimisation control |
| **Dependency footprint** | Moderate (PyTorch, Transformers, ONNX Runtime) | Larger (adds Olive framework, optional extras per workflow) |
| **Foundry Local integration** | Direct — output is immediately compatible | Requires `--use_ort_genai` flag and additional configuration |

> **Why this lab uses the Model Builder:** For the task of compiling a single Hugging Face model and registering it with Foundry Local, the Model Builder is the simplest and most reliable path. It produces the exact output format Foundry Local expects in a single command. If you later need advanced optimisation features — such as accuracy-aware quantisation, graph surgery, or multi-pass tuning — Olive is a powerful option to explore. See the [Microsoft Olive documentation](https://microsoft.github.io/Olive/) for more details.

---

## Lab Exercises

### Exercise 1: Install the ONNX Runtime GenAI Model Builder

Install the ONNX Runtime GenAI package, which includes the model builder tool:

```bash
pip install onnxruntime-genai
```

Verify the installation by checking that the model builder is available:

```bash
python -m onnxruntime_genai.models.builder --help
```

You should see help output listing parameters such as `-m` (model name), `-o` (output path), `-p` (precision), and `-e` (execution provider).

> **Note:** The model builder depends on PyTorch, Transformers, and several other packages. The installation may take a few minutes.

---

### Exercise 2: Compile Qwen3-0.6B for CPU

Run the following command to download the Qwen3-0.6B model from Hugging Face and compile it for CPU inference with int4 quantisation:

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

#### What Each Parameter Does

| Parameter | Purpose | Value Used |
|-----------|---------|------------|
| `-m` | The Hugging Face model ID or a local directory path | `Qwen/Qwen3-0.6B` |
| `-o` | Directory where the compiled ONNX model will be saved | `models/qwen3` |
| `-p` | Quantisation precision applied during compilation | `int4` |
| `-e` | ONNX Runtime execution provider (target hardware) | `cpu` |
| `--extra_options hf_token=false` | Skips Hugging Face authentication (fine for public models) | `hf_token=false` |

> **How long does this take?** Compilation time depends on your hardware and the model size. For Qwen3-0.6B with int4 quantisation on a modern CPU, expect roughly 5 to 15 minutes. Larger models take proportionally longer.

Once the command completes you should see a `models/qwen3` directory containing the compiled model files. Verify the output:

```bash
ls models/qwen3
```

You should see files including:
- `model.onnx` and `model.onnx.data` — the compiled model weights
- `genai_config.json` — ONNX Runtime GenAI configuration
- `chat_template.jinja` — the model's chat template (auto-generated)
- `tokenizer.json`, `tokenizer_config.json` — tokeniser files
- Other vocabulary and configuration files

---

### Exercise 3: Compile for GPU (Optional)

If you have an NVIDIA GPU with CUDA support, you can compile a GPU-optimised variant for faster inference:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Note:** GPU compilation requires `onnxruntime-gpu` and a working CUDA installation. If these are not present, the model builder will report an error. You can skip this exercise and continue with the CPU variant.

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
| `fp32` | Largest | Slowest | Highest accuracy |
| `fp16` | Large | Fast (GPU) | Very good accuracy |
| `int8` | Small | Fast | Slight accuracy loss |
| `int4` | Smallest | Fastest | Moderate accuracy loss |

For most local development, `int4` on CPU provides the best balance of speed and resource usage. For production-quality output, `fp16` on a CUDA GPU is recommended.

---

### Exercise 4: Create the Chat Template Configuration

The model builder automatically generates a `chat_template.jinja` file and a `genai_config.json` file in the output directory. However, Foundry Local also needs an `inference_model.json` file to understand how to format prompts for your model. This file defines the model name and the prompt template that wraps user messages in the correct special tokens.

#### Step 1: Inspect the Compiled Output

List the contents of the compiled model directory:

```bash
ls models/qwen3
```

You should see files such as:
- `model.onnx` and `model.onnx.data` — the compiled model weights
- `genai_config.json` — ONNX Runtime GenAI configuration (auto-generated)
- `chat_template.jinja` — the model's chat template (auto-generated)
- `tokenizer.json`, `tokenizer_config.json` — tokeniser files
- Various other configuration and vocabulary files

#### Step 2: Generate the inference_model.json File

The `inference_model.json` file tells Foundry Local how to format prompts. Create a Python script called `generate_chat_template.py` **in the repository root** (the same directory that contains your `models/` folder):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Build a minimal conversation to extract the chat template
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

Run the script from the repository root:

```bash
python generate_chat_template.py
```

> **Note:** The `transformers` package was already installed as a dependency of `onnxruntime-genai`. If you see an `ImportError`, run `pip install transformers` first.

The script produces an `inference_model.json` file inside the `models/qwen3` directory. The file tells Foundry Local how to wrap user input in the correct special tokens for Qwen3.

> **Important:** The `"Name"` field in `inference_model.json` (set to `qwen3-0.6b` in this script) is the **model alias** you will use in all subsequent commands and API calls. If you change this name, update the model name in Exercises 6–10 accordingly.

#### Step 3: Verify the Configuration

Open `models/qwen3/inference_model.json` and confirm it contains a `Name` field and a `PromptTemplate` object with `assistant` and `prompt` keys. The prompt template should include special tokens such as `<|im_start|>` and `<|im_end|>` (the exact tokens depend on the model's chat template).

> **Manual alternative:** If you prefer not to run the script, you can create the file manually. The key requirement is that the `prompt` field contains the model's full chat template with `{Content}` as a placeholder for the user's message.

---

### Exercise 5: Verify the Model Directory Structure

The model builder places all compiled files directly into the output directory you specified. Verify that the final structure looks correct:

```bash
ls models/qwen3
```

The directory should contain the following files:

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

> **Note:** Unlike some other compilation tools, the model builder does not create nested subdirectories. All files sit directly in the output folder, which is exactly what Foundry Local expects.

---

### Exercise 6: Add the Model to the Foundry Local Cache

Tell Foundry Local where to find your compiled model by adding the directory to its cache:

```bash
foundry cache cd models/qwen3
```

Verify that the model appears in the cache:

```bash
foundry cache ls
```

You should see your custom model listed alongside any previously cached models (such as `phi-3.5-mini` or `phi-4-mini`).

---

### Exercise 7: Run the Custom Model with the CLI

Start an interactive chat session with your newly compiled model (the `qwen3-0.6b` alias comes from the `Name` field you set in `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

The `--verbose` flag shows additional diagnostic information, which is helpful when testing a custom model for the first time. If the model loads successfully you will see an interactive prompt. Try a few messages:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Type `exit` or press `Ctrl+C` to end the session.

> **Troubleshooting:** If the model fails to load, check the following:
> - The `genai_config.json` file was generated by the model builder.
> - The `inference_model.json` file exists and is valid JSON.
> - The ONNX model files are in the correct directory.
> - You have enough available RAM (Qwen3-0.6B int4 needs roughly 1 GB).
> - Qwen3 is a reasoning model that produces `<think>` tags. If you see `<think>...</think>` prefixed to responses, this is normal behaviour. The prompt template in `inference_model.json` can be adjusted to suppress thinking output.

---

### Exercise 8: Query the Custom Model via the REST API

If you exited the interactive session in Exercise 7, the model may no longer be loaded. Start the Foundry Local service and load the model first:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Check which port the service is running on:

```bash
foundry service status
```

Then send a request (replace `5273` with your actual port if it differs):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows note:** The `curl` command above uses bash syntax. On Windows, use the PowerShell `Invoke-RestMethod` cmdlet below instead.

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

### Exercise 9: Use the Custom Model with the OpenAI SDK

You can connect to your custom model using exactly the same OpenAI SDK code you used for the built-in models (see [Part 3](part3-sdk-and-apis.md)). The only difference is the model name.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local does not validate API keys
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
  apiKey: "foundry-local", // Foundry Local does not validate API keys
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

> **Key point:** Because Foundry Local exposes an OpenAI-compatible API, any code that works with the built-in models also works with your custom models. You only need to change the `model` parameter.

---

### Exercise 10: Test the Custom Model with the Foundry Local SDK

In earlier labs you used the Foundry Local SDK to start the service, discover the endpoint, and manage models automatically. You can follow exactly the same pattern with your custom-compiled model. The SDK handles service startup and endpoint discovery, so your code does not need to hard-code `localhost:5273`.

> **Note:** Make sure the Foundry Local SDK is installed before running these examples:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Add `Microsoft.AI.Foundry.Local` and `OpenAI` NuGet packages
>
> Save each script file **in the repository root** (the same directory as your `models/` folder).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Step 1: Start the Foundry Local service and load the custom model
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Step 2: Check the cache for the custom model
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Step 3: Load the model into memory
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Step 4: Create an OpenAI client using the SDK-discovered endpoint
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Step 5: Send a streaming chat completion request
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

Run it:

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

// Step 1: Start the Foundry Local service
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Step 2: Get the custom model from the catalog
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Step 3: Load the model into memory
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Step 4: Create an OpenAI client using the SDK-discovered endpoint
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

Run it:

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

> **Key point:** The Foundry Local SDK discovers the endpoint dynamically, so you never hard-code a port number. This is the recommended approach for production applications. Your custom-compiled model works identically to built-in catalogue models through the SDK.

---

## Choosing a Model to Compile

Qwen3-0.6B is used as the reference example in this lab because it is small, fast to compile, and freely available under the Apache 2.0 licence. However, you can compile many other models. Here are some suggestions:

| Model | Hugging Face ID | Parameters | Licence | Notes |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Very small, fast compilation, good for testing |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Better quality, still fast to compile |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Strong quality, needs more RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Requires licence acceptance on Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | High quality, larger download and longer compilation |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Already in the Foundry Local catalogue (useful for comparison) |

> **Licence reminder:** Always check the model's licence on Hugging Face before using it. Some models (such as Llama) require you to accept a licence agreement and authenticate with `huggingface-cli login` before downloading.

---

## Concepts: When to Use Custom Models

| Scenario | Why Compile Your Own? |
|----------|----------------------|
| **A model you need is not in the catalogue** | The Foundry Local catalogue is curated. If the model you want is not listed, compile it yourself. |
| **Fine-tuned models** | If you have fine-tuned a model on domain-specific data, you need to compile your own weights. |
| **Specific quantisation requirements** | You may want a precision or quantisation strategy that differs from the catalogue default. |
| **Newer model releases** | When a new model is released on Hugging Face, it may not yet be in the Foundry Local catalogue. Compiling it yourself gives you immediate access. |
| **Research and experimentation** | Trying different model architectures, sizes, or configurations locally before committing to a production choice. |

---

## Summary

In this lab you learned how to:

| Step | What You Did |
|------|-------------|
| 1 | Installed the ONNX Runtime GenAI model builder |
| 2 | Compiled `Qwen/Qwen3-0.6B` from Hugging Face into an optimised ONNX model |
| 3 | Created an `inference_model.json` chat-template configuration file |
| 4 | Added the compiled model to the Foundry Local cache |
| 5 | Ran interactive chat with the custom model via the CLI |
| 6 | Queried the model through the OpenAI-compatible REST API |
| 7 | Connected from Python, JavaScript, and C# using the OpenAI SDK |
| 8 | Tested the custom model end-to-end with the Foundry Local SDK |

The key takeaway is that **any transformer-based model can run through Foundry Local** once it has been compiled to ONNX format. The OpenAI-compatible API means that all your existing application code works without changes; you only need to swap the model name.

---

## Key Takeaways

| Concept | Detail |
|---------|--------|
| ONNX Runtime GenAI Model Builder | Converts Hugging Face models to ONNX format with quantisation in a single command |
| ONNX format | Foundry Local requires ONNX models with ONNX Runtime GenAI configuration |
| Chat templates | The `inference_model.json` file tells Foundry Local how to format prompts for a given model |
| Hardware targets | Compile for CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), or WebGPU depending on your hardware |
| Quantisation | Lower precision (int4) reduces size and improves speed at the cost of some accuracy; fp16 retains high quality on GPUs |
| API compatibility | Custom models use the same OpenAI-compatible API as built-in models |
| Foundry Local SDK | The SDK handles service startup, endpoint discovery, and model loading automatically for both catalogue and custom models |

---

## Further Reading

| Resource | Link |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local custom model guide | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 model family | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive documentation | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Workshop Complete!

Congratulations — you have completed the full Foundry Local Workshop! You have gone from installing the CLI to building chat apps, RAG pipelines, multi-agent systems, speech-to-text transcription, and compiling your own custom models — all running entirely on your device.

| Part | What You Built |
|------|---------------|
| 1 | Installed Foundry Local, explored models via CLI |
| 2 | Mastered the Foundry Local SDK API — service, catalogue, cache, model management |
| 3 | Connected from Python/JS/C# using the SDK with OpenAI |
| 4 | Built a RAG pipeline with local knowledge retrieval |
| 5 | Created AI agents with personas and structured output |
| 6 | Orchestrated multi-agent pipelines with feedback loops |
| 7 | Explored a production capstone app — the Zava Creative Writer |
| 8 | Built evaluation-led development workflows for agents |
| 9 | Transcribed audio with Whisper — speech-to-text on-device |
| 10 | Compiled and ran a custom Hugging Face model with ONNX Runtime GenAI |

Continue to [Part 11: Tool Calling with Local Models](part11-tool-calling.md) to learn how to enable your local models to call external functions.

Go back to the [workshop overview](../README.md) to review what you have covered and explore the further reading resources.

---

**Further ideas:**
- Try different models (`phi-4-mini`, `deepseek-r1-7b`) to compare quality and speed
- Build a frontend UI for the Zava Writer API (Python version)
- Create your own multi-agent application for a domain you care about
- Deploy to the cloud by swapping Foundry Local for Azure AI Foundry - same code, different endpoint
