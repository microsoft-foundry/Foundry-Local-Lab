![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Bahagi 10: Paggamit ng Custom o Hugging Face na Modelo sa Foundry Local

> **Layunin:** I-compile ang isang Hugging Face na modelo sa optimised na ONNX format na kinakailangan ng Foundry Local, i-configure ito gamit ang isang chat template, idagdag ito sa lokal na cache, at patakbuhin ang inference laban dito gamit ang CLI, REST API, at OpenAI SDK.

## Pangkalahatang-ideya

Nagdala ang Foundry Local ng isang piniling katalogo ng mga pre-compiled na modelo, ngunit hindi ka limitado sa listahang iyon. Anumang transformer-based na language model na available sa [Hugging Face](https://huggingface.co/) (o naka-imbak nang lokal sa PyTorch / Safetensors na format) ay maaaring i-compile sa optimised na ONNX model at maipagsilbi sa pamamagitan ng Foundry Local.

Ginagamit ng compilation pipeline ang **ONNX Runtime GenAI Model Builder**, isang command-line na tool na kasama sa `onnxruntime-genai` na package. Ang model builder ang humahawak ng mabibigat na gawain: pag-download ng source weights, pag-convert sa ONNX format, pag-aapply ng quantisation (int4, fp16, bf16), at pag-emit ng mga configuration file (kasama na ang chat template at tokeniser) na inaasahan ng Foundry Local.

Sa lab na ito, i-compile mo ang **Qwen/Qwen3-0.6B** mula sa Hugging Face, iparehistro ito sa Foundry Local, at makipag-chat dito nang buong-buo sa iyong device.

---

## Mga Layunin sa Pagkatuto

Sa pagtatapos ng lab na ito magagawa mong:

- Ipaliwanag kung bakit kapaki-pakinabang ang custom na compilation ng modelo at kailan mo ito kakailanganin
- I-install ang ONNX Runtime GenAI model builder
- I-compile ang Hugging Face na modelo sa optimised ONNX format gamit ang isang utos lang
- Maunawaan ang mga pangunahing parameter ng compilation (execution provider, precision)
- Lumikha ng `inference_model.json` na chat-template na configuration file
- Magdagdag ng na-compile na modelo sa Foundry Local cache
- Patakbuhin ang inference laban sa custom na modelo gamit ang CLI, REST API, at OpenAI SDK

---

## Mga Kinakailangan

| Kinakailangan | Detalye |
|-------------|---------|
| **Foundry Local CLI** | Na-install at nasa iyong `PATH` ([Bahagi 1](part1-getting-started.md)) |
| **Python 3.10+** | Kinakailangan ng ONNX Runtime GenAI model builder |
| **pip** | Tagapamahala ng package ng Python |
| **Disk space** | Hindi bababa sa 5 GB na libre para sa source at na-compile na mga file ng modelo |
| **Hugging Face account** | May ilang modelo na nangangailangan ng pag-accept ng lisensya bago mag-download. Ang Qwen3-0.6B ay gumagamit ng Apache 2.0 na lisensya at malayang magagamit. |

---

## Pag-setup ng Kapaligiran

Ang compilation ng modelo ay nangangailangan ng ilang malalaking package ng Python (PyTorch, ONNX Runtime GenAI, Transformers). Gumawa ng dedikadong virtual environment upang hindi makaapekto ito sa iyong system Python o iba pang proyekto.

```bash
# Mula sa ugat ng imbakan
python -m venv .venv
```

I-activate ang environment:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

I-upgrade ang pip para maiwasan ang mga isyu sa dependency resolution:

```bash
python -m pip install --upgrade pip
```

> **Tip:** Kung mayroon ka nang `.venv` mula sa mga naunang lab, maaari mo itong gamitin muli. Siguraduhing activated ito bago magpatuloy.

---

## Konsepto: Ang Compilation Pipeline

Kinakailangan ng Foundry Local ang mga modelo sa ONNX format na may ONNX Runtime GenAI configuration. Karamihan sa mga open-source na modelo sa Hugging Face ay ipinapamahagi bilang PyTorch o Safetensors weights, kaya kinakailangan ang hakbang ng conversion.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### Ano ang Ginagawa ng Model Builder?

1. **Dinadownload** ang source model mula sa Hugging Face (o binabasa mula sa lokal na path).
2. **Kinokonvert** ang PyTorch / Safetensors na mga timbang sa ONNX format.
3. **Qquantises** ang modelo sa mas maliit na precision (halimbawa, int4) para mabawasan ang paggamit ng memorya at mapabuti ang throughput.
4. **Nag-eemit** ng ONNX Runtime GenAI configuration (`genai_config.json`), ang chat template (`chat_template.jinja`), at lahat ng tokeniser files upang ma-load at maipagsilbi ng Foundry Local ang modelo.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Maaaring makarinig ka ng mga sanggunian sa **Microsoft Olive** bilang alternatibong tool para sa model optimisation. Pareho silang makakagawa ng ONNX models, ngunit may iba't ibang layunin at trade-offs:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Package** | `onnxruntime-genai` | `olive-ai` |
| **Pangunahing layunin** | I-convert at i-quantise ang generative AI models para sa ONNX Runtime GenAI inference | End-to-end na framework para sa optimisation ng modelo na sumusuporta sa maraming backends at hardware targets |
| **Kadalian ng paggamit** | Isang utos lang — one-step conversion + quantisation | Workflow-based — configurable multi-pass pipelines gamit ang YAML/JSON |
| **Format ng output** | ONNX Runtime GenAI format (handa na para sa Foundry Local) | Generic ONNX, ONNX Runtime GenAI, o iba pang format depende sa workflow |
| **Hardware targets** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, at iba pa |
| **Mga opsyon sa quantisation** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, kasama ang graph optimisations, layer-wise tuning |
| **Saklaw ng modelo** | Generative AI models (LLMs, SLMs) | Anumang ONNX-convertible na modelo (vision, NLP, audio, multimodal) |
| **Pinakamainam para sa** | Mabilis na single-model compilation para sa local inference | Production pipelines na nangangailangan ng detalyadong kontrol sa optimisation |
| **Dependency footprint** | Katamtaman (PyTorch, Transformers, ONNX Runtime) | Mas malaki (idinaragdag ang Olive framework, mga optional na extras kada workflow) |
| **Integrasyon sa Foundry Local** | Direktang compatible ang output | Nangangailangan ng `--use_ort_genai` na flag at karagdagang configuration |

> **Bakit ginagamit ng lab na ito ang Model Builder:** Para sa layunin ng pag-compile ng isang Hugging Face model at pagrerehistro nito sa Foundry Local, ang Model Builder ang pinakasimpleng at pinaka-maasahang paraan. Nagbibigay ito ng eksaktong format ng output na inaasahan ng Foundry Local sa isang utos lang. Kung kakailanganin mo ng advanced optimisation katulad ng accuracy-aware quantisation, graph surgery, o multi-pass tuning, malakas na opsyon ang Olive. Tingnan ang [Microsoft Olive documentation](https://microsoft.github.io/Olive/) para sa higit pang detalye.

---

## Mga Ehersisyo sa Lab

### Ehersisyo 1: I-install ang ONNX Runtime GenAI Model Builder

I-install ang ONNX Runtime GenAI package, na kasama ang model builder tool:

```bash
pip install onnxruntime-genai
```

Suriin ang pag-install sa pamamagitan ng pag-check na available ang model builder:

```bash
python -m onnxruntime_genai.models.builder --help
```

Dapat makita mo ang help output na naglilista ng mga parameter tulad ng `-m` (model name), `-o` (output path), `-p` (precision), at `-e` (execution provider).

> **Tandaan:** Ang model builder ay umaasa sa PyTorch, Transformers, at iba pang package. Maaaring tumagal ng ilang minuto ang pag-install.

---

### Ehersisyo 2: I-compile ang Qwen3-0.6B para sa CPU

Patakbuhin ang sumusunod na utos para i-download ang Qwen3-0.6B na modelo mula sa Hugging Face at i-compile ito para sa CPU inference gamit ang int4 quantisation:

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

#### Ano ang Ginagawa ng Bawat Parameter

| Parameter | Layunin | Halagang Ginamit |
|-----------|---------|------------|
| `-m` | Ang Hugging Face model ID o lokal na directory path | `Qwen/Qwen3-0.6B` |
| `-o` | Direktoryo kung saan ise-save ang compiled ONNX model | `models/qwen3` |
| `-p` | Precision ng quantisation na ginagamit sa pag-compile | `int4` |
| `-e` | ONNX Runtime execution provider (target hardware) | `cpu` |
| `--extra_options hf_token=false` | Iniiwasan ang Hugging Face authentication (okay para sa mga public models) | `hf_token=false` |

> **Gaano katagal ito tatagal?** Depende ang oras ng compilation sa iyong hardware at sa laki ng modelo. Para sa Qwen3-0.6B na may int4 quantisation sa makabagong CPU, asahan milya 5 hanggang 15 minuto. Mas malalaking modelo ay tatagal ng katumbas na mahaba.

Kapag natapos ang utos, dapat kang makita ng `models/qwen3` na direktoryo na naglalaman ng mga compiled na file ng modelo. Suriin ang output:

```bash
ls models/qwen3
```

Dapat makita ang mga file kabilang ang:
- `model.onnx` at `model.onnx.data` — ang compiled weights ng modelo
- `genai_config.json` — ONNX Runtime GenAI configuration
- `chat_template.jinja` — chat template ng modelo (auto-generated)
- `tokenizer.json`, `tokenizer_config.json` — mga tokeniser files
- Iba pang mga file ng bokabularyo at configuration

---

### Ehersisyo 3: I-compile para sa GPU (Opsyonal)

Kung mayroon kang NVIDIA GPU na may suporta sa CUDA, maaari kang mag-compile ng GPU-optimised na variant para sa mas mabilis na inference:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Tandaan:** Nangangailangan ang GPU compilation ng `onnxruntime-gpu` at gumaganang CUDA installation. Kapag wala ito, magrereport ng error ang model builder. Maaari mong laktawan ang ehersisyong ito at ipagpatuloy ang CPU variant.

#### Reference sa Hardware-Specific Compilation

| Target | Execution Provider (`-e`) | Recommended Precision (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` o `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` o `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Mga Trade-off sa Precision

| Precision | Laki | Bilis | Kalidad |
|-----------|------|-------|---------|
| `fp32` | Pinakamalaki | Pinakabagal | Pinakamataas na katumpakan |
| `fp16` | Malaki | Mabilis (GPU) | Napakagandang katumpakan |
| `int8` | Maliit | Mabilis | Bahagyang pagkabawas sa katumpakan |
| `int4` | Pinakamaliit | Pinakamabilis | Katamtamang pagkawala ng katumpakan |

Para sa karamihan ng local development, ang `int4` sa CPU ay nagbibigay ng pinakamainam na balanse ng bilis at paggamit ng resources. Para sa production-quality output, inirerekomenda ang `fp16` sa CUDA GPU.

---

### Ehersisyo 4: Lumikha ng Chat Template Configuration

Awtomatikong lumilikha ang model builder ng `chat_template.jinja` file at `genai_config.json` file sa output directory. Ngunit kailangan din ng Foundry Local ng `inference_model.json` file upang maunawaan kung paano i-format ang mga prompt para sa iyong modelo. Ang file na ito ay nagtatakda ng pangalan ng modelo at ang prompt template na bumabalot sa mga mensahe ng user sa tamang special tokens.

#### Hakbang 1: Suriin ang Na-compile na Output

Ilista ang laman ng directory ng na-compile na modelo:

```bash
ls models/qwen3
```

Dapat makita mo ang mga file tulad ng:
- `model.onnx` at `model.onnx.data` — compiled weights ng modelo
- `genai_config.json` — ONNX Runtime GenAI configuration (auto-generated)
- `chat_template.jinja` — template ng chat ng modelo (auto-generated)
- `tokenizer.json`, `tokenizer_config.json` — mga tokeniser files
- Iba pang configuration at mga file ng bokabularyo

#### Hakbang 2: Lumikha ng `inference_model.json` File

Sinasabi ng `inference_model.json` sa Foundry Local kung paano i-format ang mga prompt. Gumawa ng Python script na tinatawag na `generate_chat_template.py` **sa root ng repository** (sa parehong direktoryo kung saan naroroon ang iyong `models/` folder):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Bumuo ng isang minimal na usapan upang kunin ang template ng chat
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

# Bumuo ng istraktura ng inference_model.json
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

Patakbuhin ang script mula sa root ng repository:

```bash
python generate_chat_template.py
```

> **Tandaan:** Na-install na ang `transformers` package bilang dependency ng `onnxruntime-genai`. Kung makita mo ang `ImportError`, patakbuhin muna ang `pip install transformers`.

Naglalabas ang script ng `inference_model.json` file sa loob ng `models/qwen3` na direktoryo. Sinasabi ng file sa Foundry Local kung paano balutin ang input ng user sa tamang special tokens para sa Qwen3.

> **Mahalaga:** Ang `"Name"` field sa `inference_model.json` (nakaset sa `qwen3-0.6b` sa script na ito) ay ang **alias ng modelo** na gagamitin mo sa lahat ng mga sumusunod na utos at API calls. Kung babaguhin mo ang pangalang ito, i-update ang pangalan ng modelo sa Mga Ehersisyo 6–10 nang naaayon.

#### Hakbang 3: Suriin ang Configuration

Buksan ang `models/qwen3/inference_model.json` at tiyakin na mayroon itong `Name` field at isang `PromptTemplate` object na may mga key na `assistant` at `prompt`. Dapat maglaman ang prompt template ng special tokens tulad ng `<|im_start|>` at `<|im_end|>` (ang eksaktong mga token ay depende sa chat template ng modelo).

> **Manwal na alternatibo:** Kung ayaw mong patakbuhin ang script, maaari mong likhain ang file nang mano-mano. Ang mahalagang kinakailangan ay ang `prompt` field ay naglalaman ng buong chat template ng modelo na may `{Content}` bilang placeholder para sa mensahe ng user.

---

### Ehersisyo 5: Suriin ang Estruktura ng Diretoryo ng Modelo
Ang model builder ay inilalagay ang lahat ng na-kompila na mga file nang direkta sa output na direktoryo na iyong tinukoy. Suriin na ang panghuling istruktura ay tama ang pagkakaayos:

```bash
ls models/qwen3
```

Ang direktoryo ay dapat maglaman ng mga sumusunod na file:

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

> **Note:** Hindi tulad ng ibang mga compilation tool, ang model builder ay hindi gumagawa ng nested subdirectories. Lahat ng mga file ay direktang nasa loob ng output folder, na siyang eksaktong inaasahan ng Foundry Local.

---

### Pagsasanay 6: Idagdag ang Modelo sa Foundry Local Cache

Sabihan ang Foundry Local kung saan matatagpuan ang iyong na-kompila na modelo sa pamamagitan ng pagdagdag ng direktoryo sa cache nito:

```bash
foundry cache cd models/qwen3
```

Suriin na ang modelo ay lumilitaw sa cache:

```bash
foundry cache ls
```

Dapat mong makita ang iyong custom model na nakalista kasama ng anumang naunang naka-cache na mga modelo (tulad ng `phi-3.5-mini` o `phi-4-mini`).

---

### Pagsasanay 7: Patakbuhin ang Custom Model gamit ang CLI

Simulan ang isang interactive na chat session gamit ang iyong bagong na-kompila na modelo (ang alias na `qwen3-0.6b` ay mula sa `Name` field na iyong itinakda sa `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Ipinapakita ng `--verbose` flag ang karagdagang impormasyon para sa diagnosis, na kapaki-pakinabang kapag sumusubok ng custom model sa unang pagkakataon. Kung matagumpay na nag-load ang modelo, makikita mo ang isang interactive prompt. Subukan ang ilang mga mensahe:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

I-type ang `exit` o pindutin ang `Ctrl+C` upang tapusin ang session.

> **Troubleshooting:** Kung nabigo ang pag-load ng modelo, suriin ang mga sumusunod:
> - Ang `genai_config.json` file ay nalikha ng model builder.
> - Ang `inference_model.json` file ay naroroon at valid na JSON.
> - Ang ONNX model files ay nasa tamang direktoryo.
> - May sapat kang libreng RAM (ang Qwen3-0.6B int4 ay nangangailangan ng humigit-kumulang 1 GB).
> - Ang Qwen3 ay isang reasoning model na naglalabas ng `<think>` tags. Kung makakita ka ng `<think>...</think>` na naka-prefix sa mga sagot, ito ay normal na pag-uugali. Maaaring baguhin ang prompt template sa `inference_model.json` upang itigil ang pagpapakita ng thinking output.

---

### Pagsasanay 8: Mag-query ng Custom Model sa pamamagitan ng REST API

Kung nilabas mo ang interactive session sa Pagsasanay 7, maaaring hindi na naka-load ang modelo. Simulan muna ang Foundry Local service at i-load ang modelo:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Suriin kung aling port ang ginagamit ng service:

```bash
foundry service status
```

Pagkatapos magpadala ng request (palitan ang `5273` ng iyong aktwal na port kung ito ay iba):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows note:** Ang `curl` command sa itaas ay gumagamit ng bash syntax. Sa Windows, gamitin ang PowerShell `Invoke-RestMethod` cmdlet sa halip.

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

### Pagsasanay 9: Gamitin ang Custom Model gamit ang OpenAI SDK

Maaari kang kumonekta sa iyong custom model gamit ang eksaktong parehong OpenAI SDK code na ginamit mo para sa mga built-in na modelo (tingnan ang [Part 3](part3-sdk-and-apis.md)). Ang nag-iisang pagkakaiba ay ang pangalan ng modelo.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Hindi sinusuri ng Foundry Local ang mga API key
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
  apiKey: "foundry-local", // Hindi sinusuri ng Foundry Local ang mga API key
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

> **Pangunahing punto:** Dahil ang Foundry Local ay nagpapakita ng OpenAI-compatible API, anumang kodigong gumagana sa mga built-in na modelo ay gagana rin sa iyong custom models. Kailangang palitan lang ang `model` parameter.

---

### Pagsasanay 10: Subukan ang Custom Model gamit ang Foundry Local SDK

Sa mga naunang labs ginamit mo ang Foundry Local SDK upang simulan ang serbisyo, tuklasin ang endpoint, at pamahalaan ang mga modelo nang awtomatiko. Maaari mong sundan ang eksaktong parehong pattern sa iyong custom-compiled model. Hinahandle ng SDK ang pagsisimula ng serbisyo at pagtuklas ng endpoint, kaya hindi mo kailangang hard-code ang `localhost:5273` sa iyong code.

> **Note:** Siguraduhing naka-install ang Foundry Local SDK bago patakbuhin ang mga halimbawa na ito:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Magdagdag ng `Microsoft.AI.Foundry.Local` at `OpenAI` NuGet packages
>
> I-save ang bawat script file **sa root ng repository** (sa parehong direktoryo kung saan naroon ang iyong `models/` folder).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Hakbang 1: Simulan ang Foundry Local na serbisyo at i-load ang custom na modelo
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Hakbang 2: Suriin ang cache para sa custom na modelo
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Hakbang 3: I-load ang modelo sa memorya
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Hakbang 4: Gumawa ng OpenAI client gamit ang SDK-discovered na endpoint
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Hakbang 5: Magpadala ng streaming chat completion na kahilingan
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

Patakbuhin ito:

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

// Hakbang 1: Simulan ang Foundry Local na serbisyo
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Hakbang 2: Kunin ang custom na modelo mula sa katalogo
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Hakbang 3: I-load ang modelo sa memorya
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Hakbang 4: Gumawa ng OpenAI client gamit ang endpoint na natuklasan ng SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Hakbang 5: Magpadala ng streaming chat completion na kahilingan
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

Patakbuhin ito:

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

> **Pangunahing punto:** Dinidiskubre ng Foundry Local SDK ang endpoint nang dinamiko, kaya hindi mo kailangang i-hard-code ang port number. Ito ang inirerekomendang paraan para sa mga production application. Gumagana nang pareho ang iyong custom-compiled model tulad ng built-in catalogue models sa pamamagitan ng SDK.

---

## Pagpili ng Modelo para I-Compile

Ang Qwen3-0.6B ay ginamit bilang reference example sa lab na ito dahil ito ay maliit, mabilis i-compile, at libre sa ilalim ng Apache 2.0 licence. Gayunpaman, maaari kang mag-compile ng maraming ibang mga modelo. Narito ang ilang mga rekomendasyon:

| Model | Hugging Face ID | Parameters | Licence | Notes |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Napakaliit, mabilis i-compile, maganda para sa testing |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Mas magandang kalidad, mabilis pa rin i-compile |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Malakas na kalidad, nangangailangan ng mas maraming RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Nangangailangan ng pagtanggap ng lisensya sa Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Mataas ang kalidad, mas malaki ang download at mas matagal i-compile |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Nasa Foundry Local catalogue na (kapaki-pakinabang para sa paghahambing) |

> **Paalala sa Lisensya:** Laging suriin ang lisensya ng modelo sa Hugging Face bago gamitin. Ang ilang mga modelo (tulad ng Llama) ay nangangailangan na tanggapin ang isang lisensyang kasunduan at mag-authenticate gamit ang `huggingface-cli login` bago i-download.

---

## Mga Konsepto: Kailan Gagamit ng Custom Models

| Scenario | Bakit Ka Magko-compile ng Sarili? |
|----------|----------------------------------|
| **Kung ang modelong kailangan mo ay wala sa catalogue** | Ang Foundry Local catalogue ay pinamamahalaan. Kung ang modelong gusto mo ay hindi nakalista, i-compile mo ito nang sarili mo. |
| **Fine-tuned models** | Kung ikaw ay nag-fine-tune ng modelo gamit ang domain-specific na data, kailangan mong i-compile ang sarili mong weights. |
| **Mga espesipikong pangangailangan sa quantisation** | Maaari kang magkaroon ng precision o quantisation strategy na iba sa default ng catalogue. |
| **Mas bagong mga release ng modelo** | Kapag may bagong modelo na inilabas sa Hugging Face, maaaring hindi pa ito nasa Foundry Local catalogue. Ang pag-compile nito nang sarili mo ay nagbibigay ng agarang access. |
| **Pananaliksik at eksperimento** | Pagsubok ng iba't ibang arkitektura ng modelo, laki, o configuration nang lokal bago pumili ng pang-production. |

---

## Buod

Sa lab na ito natutunan mo kung paano:

| Hakbang | Ano ang Ginawa Mo |
|---------|--------------------|
| 1 | Nag-install ng ONNX Runtime GenAI model builder |
| 2 | Na-kompila ang `Qwen/Qwen3-0.6B` mula sa Hugging Face sa isang optimised ONNX model |
| 3 | Nilikha ang `inference_model.json` chat-template configuration file |
| 4 | Idinagdag ang na-kompila na modelo sa Foundry Local cache |
| 5 | Pinatakbo ang interactive chat gamit ang custom model sa pamamagitan ng CLI |
| 6 | Nag-query sa modelo gamit ang OpenAI-compatible REST API |
| 7 | Nakakonekta mula sa Python, JavaScript, at C# gamit ang OpenAI SDK |
| 8 | Nasubukan ang custom model end-to-end gamit ang Foundry Local SDK |

Ang pangunahing aral ay na **anumang transformer-based na modelo ay maaaring patakbuhin sa Foundry Local** kapag na-kompila na ito sa ONNX format. Dahil ang OpenAI-compatible API, lahat ng kasalukuyan mong code sa aplikasyon ay gagana nang walang pagbabago; papalitan mo lang ang pangalan ng modelo.

---

## Pangunahing Aral

| Konsepto | Detalye |
|----------|---------|
| ONNX Runtime GenAI Model Builder | Nagko-convert ng Hugging Face models sa ONNX format na may quantisation sa isang utos lang |
| ONNX format | Kinakailangan ng Foundry Local ang ONNX models na may ONNX Runtime GenAI configuration |
| Chat templates | Sinasabi ng `inference_model.json` file kung paano i-format ang mga prompt para sa isang partikular na modelo |
| Hardware targets | Puwedeng i-compile para sa CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), o WebGPU depende sa iyong hardware |
| Quantisation | Ang mas mababang precision (int4) ay nagpapaiksi ng laki at nagpapabilis kapalit ng konting accuracy; ang fp16 ay nagpapanatili ng mataas na kalidad sa GPUs |
| API compatibility | Gumagamit ang custom models ng parehong OpenAI-compatible API tulad ng mga built-in na modelo |
| Foundry Local SDK | Awtomatikong hinahandle ng SDK ang pagsisimula ng serbisyo, pagtuklas ng endpoint, at pag-load ng modelo para sa catalogue at custom models |

---

## Dagdag na Pagbabasa

| Resource | Link |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local custom model guide | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 model family | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive documentation | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Susunod na Mga Hakbang

Magpatuloy sa [Part 11: Tool Calling with Local Models](part11-tool-calling.md) upang matutunan kung paano paganahin ang iyong mga lokal na modelo na tumawag ng external na mga function.

[← Part 9: Whisper Voice Transcription](part9-whisper-voice-transcription.md) | [Part 11: Tool Calling →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Pagtatanggol**:  
Ang dokumentong ito ay isinalin gamit ang serbisyong AI na pagsasalin na [Co-op Translator](https://github.com/Azure/co-op-translator). Bagamat nagsusumikap kami para sa katumpakan, pakatandaan na ang awtomatikong pagsasalin ay maaaring maglaman ng mga pagkakamali o kamalian. Ang orihinal na dokumento sa orihinal nitong wika ang dapat ituring na opisyal na sanggunian. Para sa mahalagang impormasyon, inirerekomenda ang propesyonal na pagsasalin ng tao. Hindi kami mananagot sa anumang hindi pagkakaunawaan o maling interpretasyon na nagmula sa paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->