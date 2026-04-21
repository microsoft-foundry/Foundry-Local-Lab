![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 10: Brug af tilpassede eller Hugging Face-modeller med Foundry Local

> **Mål:** Kompilér en Hugging Face-model til det optimerede ONNX-format, som Foundry Local kræver, konfigurer den med en chat-skabelon, tilføj den til den lokale cache, og kør inferens mod den ved hjælp af CLI, REST API og OpenAI SDK.

## Oversigt

Foundry Local leveres med et kurateret katalog af forkompilerede modeller, men du er ikke begrænset til denne liste. Enhver transformer-baseret sprogmodel tilgængelig på [Hugging Face](https://huggingface.co/) (eller gemt lokalt i PyTorch / Safetensors-format) kan kompilieres til en optimeret ONNX-model og serveres gennem Foundry Local.

Kompileringspipeline bruger **ONNX Runtime GenAI Model Builder**, et kommandolinjeværktøj inkluderet i `onnxruntime-genai`-pakken. Model builderen håndterer det tunge arbejde: downloader kildevægtene, konverterer dem til ONNX-format, anvender kvantisering (int4, fp16, bf16), og udsteder konfigurationsfilerne (inklusive chat-skabelon og tokenizer), som Foundry Local forventer.

I dette laboratorium vil du kompilere **Qwen/Qwen3-0.6B** fra Hugging Face, registrere den med Foundry Local og chatte med den helt på din enhed.

---

## Læringsmål

Når du har gennemført dette laboratorium, vil du kunne:

- Forklare hvorfor tilpasset modelkompilering er nyttigt, og hvornår det kan være nødvendigt
- Installere ONNX Runtime GenAI model builder
- Kompilere en Hugging Face-model til optimeret ONNX-format med én enkelt kommando
- Forstå de vigtigste kompilationsparametre (udførelsesudbyder, præcision)
- Oprette `inference_model.json` chat-skabelonskonfigurationsfilen
- Tilføje en kompileret model til Foundry Locals cache
- Køre inferens mod den tilpassede model ved hjælp af CLI, REST API og OpenAI SDK

---

## Forudsætninger

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Installeret og på din `PATH` ([Del 1](part1-getting-started.md)) |
| **Python 3.10+** | Påkrævet af ONNX Runtime GenAI model builder |
| **pip** | Python pakkehåndtering |
| **Diskplads** | Mindst 5 GB ledig til kilde- og kompilerede modelfiler |
| **Hugging Face konto** | Nogle modeller kræver du accepterer en licens før download. Qwen3-0.6B bruger Apache 2.0-licensen og er frit tilgængelig. |

---

## Miljøopsætning

Modelkompilering kræver flere store Python-pakker (PyTorch, ONNX Runtime GenAI, Transformers). Opret et dedikeret virtuelt miljø, så disse ikke forstyrrer dit system-Python eller andre projekter.

```bash
# Fra lagerets rod
python -m venv .venv
```

Aktivér miljøet:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Opgrader pip for at undgå afhængighedsløsningsproblemer:

```bash
python -m pip install --upgrade pip
```

> **Tip:** Hvis du allerede har et `.venv` fra tidligere laboratorium, kan du genbruge det. Sørg blot for, at det er aktiveret, før du fortsætter.

---

## Koncept: Kompileringspipeline

Foundry Local kræver modeller i ONNX-format med ONNX Runtime GenAI-konfiguration. De fleste open source-modeller på Hugging Face distribueres som PyTorch- eller Safetensors-vægte, så et konverteringstrin er nødvendigt.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### Hvad gør Model Builder?

1. **Downloader** kilde-modellen fra Hugging Face (eller læser den fra en lokal sti).
2. **Konverterer** PyTorch / Safetensors-vægte til ONNX-format.
3. **Kvantisere** modellen til en mindre præcision (for eksempel int4) for at reducere hukommelsesforbrug og forbedre gennemløb.
4. **Udsteder** ONNX Runtime GenAI-konfigurationen (`genai_config.json`), chat-skabelonen (`chat_template.jinja`) og alle tokenizer-filer, så Foundry Local kan indlæse og servere modellen.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Du kan støde på referencer til **Microsoft Olive** som et alternativt værktøj til modeloptimering. Begge værktøjer kan producere ONNX-modeller, men de tjener forskellige formål og har forskellige kompromiser:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Pakkke** | `onnxruntime-genai` | `olive-ai` |
| **Primært formål** | Konvertere og kvantisere generative AI-modeller til ONNX Runtime GenAI inferens | End-to-end modeloptimeringsframework med understøttelse af mange backends og hardwaremål |
| **Brugervenlighed** | Enkel kommando — ét-trins konvertering + kvantisering | Workflow-baseret — konfigurerbare multi-pass pipelines med YAML/JSON |
| **Outputformat** | ONNX Runtime GenAI-format (klart til Foundry Local) | Generisk ONNX, ONNX Runtime GenAI, eller andre formater afhængig af workflow |
| **Hardwaremål** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN med mere |
| **Kvantiseringsmuligheder** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16 plus grafoptimeringer, lagvise tuning |
| **Modelomfang** | Generative AI-modeller (LLM, SLM) | Enhver ONNX-konvertibel model (vision, NLP, audio, multimodal) |
| **Bedst til** | Hurtig enkeltmodel-kompilering til lokal inferens | Produktionspipelines der kræver finmasket optimeringskontrol |
| **Afhængighedsaftryk** | Moderat (PyTorch, Transformers, ONNX Runtime) | Større (tilføjer Olive-framework, valgfrie ekstramoduler pr. workflow) |
| **Foundry Local integration** | Direkte — output er straks kompatibelt | Kræver `--use_ort_genai` flag og yderligere konfiguration |

> **Hvorfor dette laboratorium bruger Model Builder:** Til opgaven med at kompilere en enkelt Hugging Face-model og registrere den med Foundry Local er Model Builder den enkleste og mest pålidelige løsning. Den producerer det nøjagtige outputformat, Foundry Local forventer, med en enkelt kommando. Hvis du senere har behov for avancerede optimeringsfunktioner — såsom præcision-aware kvantisering, grafoperationer eller multi-pass tuning — er Olive en kraftfuld mulighed. Se [Microsoft Olive dokumentationen](https://microsoft.github.io/Olive/) for flere detaljer.

---

## Laboratorieøvelser

### Øvelse 1: Installer ONNX Runtime GenAI Model Builder

Installer ONNX Runtime GenAI-pakken, som inkluderer model builder-værktøjet:

```bash
pip install onnxruntime-genai
```

Verificér installationen ved at tjekke, at model builder er tilgængelig:

```bash
python -m onnxruntime_genai.models.builder --help
```

Du bør se hjælpeoutput, der viser parametre såsom `-m` (modelnavn), `-o` (outputsti), `-p` (præcision), og `-e` (udførelsesudbyder).

> **Bemærk:** Model builderen afhænger af PyTorch, Transformers og flere andre pakker. Installation kan tage nogle minutter.

---

### Øvelse 2: Kompilér Qwen3-0.6B til CPU

Kør følgende kommando for at downloade Qwen3-0.6B modellen fra Hugging Face og kompilere den til CPU-inferens med int4 kvantisering:

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

#### Hvad hvert parameter gør

| Parameter | Formål | Anvendt værdi |
|-----------|---------|---------------|
| `-m` | Hugging Face-model-ID eller lokal mappe | `Qwen/Qwen3-0.6B` |
| `-o` | Mappe hvor den kompilerede ONNX-model gemmes | `models/qwen3` |
| `-p` | Kvantiseringspræcision anvendt under kompilering | `int4` |
| `-e` | ONNX Runtime udførelsesudbyder (målhardware) | `cpu` |
| `--extra_options hf_token=false` | Spring Hugging Face godkendelse over (fint for offentlige modeller) | `hf_token=false` |

> **Hvor lang tid tager det?** Kompilationstiden afhænger af dit hardware og modelstørrelse. For Qwen3-0.6B med int4 kvantisering på en moderne CPU, forvent ca. 5 til 15 minutter. Større modeller tager proportionelt længere.

Når kommandoen er fuldført, bør du se en `models/qwen3` mappe med de kompilerede modelfiler. Verificér output:

```bash
ls models/qwen3
```

Du bør se filer såsom:
- `model.onnx` og `model.onnx.data` — de kompilerede modelvægte
- `genai_config.json` — ONNX Runtime GenAI-konfiguration
- `chat_template.jinja` — modellens chat-skabelon (auto-genereret)
- `tokenizer.json`, `tokenizer_config.json` — tokenizer-filer
- Andre vokabular- og konfigurationsfiler

---

### Øvelse 3: Kompilér til GPU (Valgfrit)

Hvis du har et NVIDIA GPU med CUDA-understøttelse, kan du kompilere en GPU-optimeret variant for hurtigere inferens:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Bemærk:** GPU-kompilering kræver `onnxruntime-gpu` og en fungerende CUDA-installation. Hvis disse ikke er til stede, rapporterer model builderen en fejl. Du kan springe denne øvelse over og fortsætte med CPU-varianten.

#### Hardware-specifik kompilationsreference

| Mål | Udførelsesudbyder (`-e`) | Anbefalet præcision (`-p`) |
|--------|---------------------------|----------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` eller `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` eller `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Præcisionsafvejninger

| Præcision | Størrelse | Hastighed | Kvalitet |
|-----------|-----------|-----------|----------|
| `fp32` | Størst | Langsommest | Højeste nøjagtighed |
| `fp16` | Stor | Hurtig (GPU) | Meget god nøjagtighed |
| `int8` | Lille | Hurtig | Lidt nøjagtighedstab |
| `int4` | Mindst | Hurtigst | Moderat nøjagtighedstab |

For de fleste lokale udviklinger giver `int4` på CPU den bedste balance mellem hastighed og ressourceforbrug. Til produktion anbefales `fp16` på en CUDA-GPU.

---

### Øvelse 4: Opret chat-skabelonskonfiguration

Model builder genererer automatisk en `chat_template.jinja` fil og en `genai_config.json` fil i outputmappen. Foundry Local har dog også brug for en `inference_model.json` fil for at forstå, hvordan prompts skal formateres til din model. Denne fil definerer modelnavnet og promptskabelonen, der omslutter brugernes beskeder i de korrekte specielle tokens.

#### Trin 1: Inspicér den kompilerede output

List indholdet i den kompilerede modelmappe:

```bash
ls models/qwen3
```

Du bør se filer såsom:
- `model.onnx` og `model.onnx.data` — de kompilerede modelvægte
- `genai_config.json` — ONNX Runtime GenAI-konfiguration (auto-genereret)
- `chat_template.jinja` — modellens chat-skabelon (auto-genereret)
- `tokenizer.json`, `tokenizer_config.json` — tokenizer-filer
- Forskellige andre konfigurations- og vokabularfiler

#### Trin 2: Generér filen inference_model.json

`inference_model.json` fortæller Foundry Local, hvordan den skal formatere prompts. Opret et Python-script kaldet `generate_chat_template.py` **i repositories rodkatalog** (samme mappe som indeholder din `models/`-mappe):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Byg en minimal samtale for at udtrække chat-skabelonen
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

# Byg strukturen for inference_model.json
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

Kør scriptet fra repositories rodkatalog:

```bash
python generate_chat_template.py
```

> **Bemærk:** `transformers`-pakken var allerede installeret som afhængighed til `onnxruntime-genai`. Hvis du får en `ImportError`, kør `pip install transformers` først.

Scriptet producerer en `inference_model.json` fil inde i `models/qwen3` mappen. Filen fortæller Foundry Local, hvordan brugerinput skal omsluttes med de korrekte specielle tokens for Qwen3.

> **Vigtigt:** `"Name"`-feltet i `inference_model.json` (sat til `qwen3-0.6b` i dette script) er **modelaliaset**, du skal bruge i alle efterfølgende kommandoer og API-kald. Hvis du ændrer dette navn, skal du også opdatere modelnavnet i Øvelser 6–10.

#### Trin 3: Verificér konfigurationen

Åbn `models/qwen3/inference_model.json` og bekræft, at den indeholder et `Name`-felt og et `PromptTemplate`-objekt med `assistant` og `prompt` nøgler. Promptskabelonen skal inkludere specielle tokens som `<|im_start|>` og `<|im_end|>` (de eksakte tokens afhænger af modellens chat-skabelon).

> **Manuel alternativ:** Hvis du foretrækker ikke at køre scriptet, kan du oprette filen manuelt. Det vigtigste krav er, at `prompt`-feltet indeholder modellens fulde chat-skabelon med `{Content}` som pladsholder for brugerens besked.

---

### Øvelse 5: Verificér modelmappestrukturen
Modelbyggeren placerer alle kompilerede filer direkte i den outputmappe, du har angivet. Bekræft, at den endelige struktur ser korrekt ud:

```bash
ls models/qwen3
```

Mappen bør indeholde følgende filer:

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

> **Note:** I modsætning til nogle andre kompilerværktøjer opretter modelbyggeren ikke indlejrede undermapper. Alle filer ligger direkte i outputmappen, hvilket er præcis, hvad Foundry Local forventer.

---

### Øvelse 6: Tilføj Modellen til Foundry Local Cache

Fortæl Foundry Local, hvor den kan finde din kompilerede model ved at tilføje mappen til dens cache:

```bash
foundry cache cd models/qwen3
```

Bekræft at modellen vises i cachen:

```bash
foundry cache ls
```

Du bør se din brugerdefinerede model opført sammen med eventuelle tidligere cached modeller (såsom `phi-3.5-mini` eller `phi-4-mini`).

---

### Øvelse 7: Kør den Brugerdefinerede Model med CLI

Start en interaktiv chat-session med din nykompilerede model (aliasset `qwen3-0.6b` kommer fra feltet `Name`, som du satte i `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Flaget `--verbose` viser yderligere diagnostiske oplysninger, hvilket er nyttigt, når du tester en brugerdefineret model første gang. Hvis modellen indlæses korrekt, vil du se en interaktiv prompt. Prøv nogle beskeder:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Skriv `exit` eller tryk på `Ctrl+C` for at afslutte sessionen.

> **Fejlfinding:** Hvis modellen ikke indlæses, så tjek følgende:
> - Filen `genai_config.json` blev genereret af modelbyggeren.
> - Filen `inference_model.json` findes og er gyldig JSON.
> - ONNX-modelfilerne ligger i den korrekte mappe.
> - Du har tilstrækkelig ledig RAM (Qwen3-0.6B int4 kræver cirka 1 GB).
> - Qwen3 er en ræsonneringsmodel, der producerer `<think>` tags. Hvis du ser `<think>...</think>` foranstillet svarene, er det normal opførsel. Promptskabelonen i `inference_model.json` kan justeres for at undertrykke tænkeoutput.

---

### Øvelse 8: Spørg den Brugerdefinerede Model via REST API

Hvis du afsluttede den interaktive session i Øvelse 7, kan modellen muligvis ikke længere være indlæst. Start Foundry Local-tjenesten og indlæs modellen først:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Tjek hvilken port tjenesten kører på:

```bash
foundry service status
```

Send derefter en forespørgsel (erstat `5273` med din aktuelle port, hvis den er forskellig):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows-note:** `curl`-kommandoen ovenfor bruger bash-syntaks. På Windows skal du i stedet bruge PowerShell-cmdleten `Invoke-RestMethod` nedenfor.

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

### Øvelse 9: Brug den Brugerdefinerede Model med OpenAI SDK

Du kan forbinde til din brugerdefinerede model ved at bruge præcis den samme OpenAI SDK-kode, som du brugte til de indbyggede modeller (se [Del 3](part3-sdk-and-apis.md)). Den eneste forskel er modelnavnet.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local validerer ikke API-nøgler
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
  apiKey: "foundry-local", // Foundry Local validerer ikke API-nøgler
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

> **Nøglepunkt:** Fordi Foundry Local udstiller en OpenAI-kompatibel API, fungerer enhver kode, der virker med de indbyggede modeller, også med dine brugerdefinerede modeller. Du skal kun ændre parameteren `model`.

---

### Øvelse 10: Test den Brugerdefinerede Model med Foundry Local SDK

I tidligere labs brugte du Foundry Local SDK til at starte tjenesten, finde endpoint og administrere modeller automatisk. Du kan følge præcis samme mønster med din brugerdefineret-kompilerede model. SDK’en håndterer tjenestestart og endpoint-opdagelse, så din kode behøver ikke hårdkode `localhost:5273`.

> **Note:** Sørg for at Foundry Local SDK er installeret før du kører disse eksempler:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Tilføj NuGet-pakkerne `Microsoft.AI.Foundry.Local` og `OpenAI`
>
> Gem hver scriptfil **i rodmappen af dit repository** (samme mappe som din `models/`-folder).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Trin 1: Start Foundry Local-tjenesten og indlæs den brugerdefinerede model
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Trin 2: Tjek cachen for den brugerdefinerede model
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Trin 3: Indlæs modellen i hukommelsen
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Trin 4: Opret en OpenAI-klient ved hjælp af SDK-opdagede slutpunkt
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Trin 5: Send en streaming chat-fuldførelsesanmodning
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

Kør den:

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

// Trin 1: Start Foundry Local-tjenesten
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Trin 2: Hent den tilpassede model fra katalogen
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Trin 3: Indlæs modellen i hukommelsen
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Trin 4: Opret en OpenAI-klient ved hjælp af SDK-opdagede endpoint
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Trin 5: Send en streaming chat fuldførelsesanmodning
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

Kør den:

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

> **Nøglepunkt:** Foundry Local SDK opdager endpoint dynamisk, så du hårdkoder aldrig et portnummer. Dette er den anbefalede tilgang til produktionsapplikationer. Din brugerdefineret-kompilerede model fungerer identisk med indbyggede katalogmodeller via SDK’en.

---

## Valg af Model til Kompilering

Qwen3-0.6B bruges som referenceeksempel i dette lab, fordi den er lille, hurtig at kompilere og frit tilgængelig under Apache 2.0-licensen. Du kan dog kompilere mange andre modeller. Her er nogle forslag:

| Model | Hugging Face ID | Parametre | Licens | Noter |
|-------|-----------------|-----------|--------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Meget lille, hurtig kompilering, god til test |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Bedre kvalitet, stadig hurtig at kompilere |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Høj kvalitet, kræver mere RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Kræver accept af licens på Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Høj kvalitet, større download og længere kompilering |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Allerede i Foundry Local kataloget (nyttig til sammenligning) |

> **Licenspåmindelse:** Husk altid at tjekke modellens licens på Hugging Face, før du bruger den. Nogle modeller (som Llama) kræver, at du accepterer en licensaftale og logger ind med `huggingface-cli login` før download.

---

## Begreber: Hvornår Skal du Bruge Brugerdefinerede Modeller

| Scenario | Hvorfor Kompilere Selv? |
|----------|-------------------------|
| **En model, du har brug for, findes ikke i kataloget** | Foundry Local-kataloget er kurateret. Hvis den ønskede model ikke findes, skal du kompilere den selv. |
| **Finjusterede modeller** | Hvis du har finjusteret en model på domænespecifikke data, skal du kompilere dine egne vægte. |
| **Specifikke kvantiseringskrav** | Du vil måske bruge en præcision eller kvantiseringsstrategi, der adskiller sig fra katalogets standard. |
| **Nyere modeludgivelser** | Når en ny model udgives på Hugging Face, er den måske ikke endnu i Foundry Local-kataloget. Ved selv at kompilere får du øjeblikkelig adgang. |
| **Forskning og eksperimentering** | Prøv forskellige modelarkitekturer, størrelser eller konfigurationer lokalt, før du beslutter produktionen. |

---

## Resumé

I dette lab lærte du at:

| Trin | Hvad du gjorde |
|------|----------------|
| 1 | Installerede ONNX Runtime GenAI modelbyggeren |
| 2 | Kompilerede `Qwen/Qwen3-0.6B` fra Hugging Face til en optimeret ONNX-model |
| 3 | Oprettede en `inference_model.json` chat-skabelon-konfigurationsfil |
| 4 | Tilføjede den kompilerede model til Foundry Local cachen |
| 5 | Kørte interaktiv chat med den brugerdefinerede model via CLI |
| 6 | Spurgte modellen gennem OpenAI-kompatibel REST API |
| 7 | Forbandt fra Python, JavaScript og C# ved brug af OpenAI SDK |
| 8 | Testede den brugerdefinerede model end-to-end med Foundry Local SDK |

Det vigtigste er, at **enhver transformerbaseret model kan køre gennem Foundry Local**, når den er kompileret til ONNX-format. OpenAI-kompatibel API betyder, at al din eksisterende applikationskode fungerer uden ændringer; du skal kun skifte modelnavnet.

---

## Vigtige Pointer

| Begreb | Detalje |
|--------|---------|
| ONNX Runtime GenAI Model Builder | Konverterer Hugging Face modeller til ONNX format med kvantisering i en enkelt kommando |
| ONNX format | Foundry Local kræver ONNX modeller med ONNX Runtime GenAI konfiguration |
| Chat-skabeloner | Filen `inference_model.json` fortæller Foundry Local, hvordan den skal formatere prompt til en given model |
| Hardwaremål | Kompiler til CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) eller WebGPU afhængigt af din hardware |
| Kvantisering | Lavere præcision (int4) reducerer størrelse og forbedrer hastighed på bekostning af noget nøjagtighed; fp16 bevarer høj kvalitet på GPU’er |
| API-kompatibilitet | Brugerdefinerede modeller bruger samme OpenAI-kompatible API som indbyggede modeller |
| Foundry Local SDK | SDK’en håndterer tjenestestart, endpoint-opdagelse og modelloading automatisk for både katalog- og brugerdefinerede modeller |

---

## Yderligere Læsning

| Ressource | Link |
|-----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local brugerdefineret modelguide | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 modelfamilie | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive-dokumentation | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Næste Skridt

Fortsæt til [Del 11: Tool Calling med lokale modeller](part11-tool-calling.md) for at lære, hvordan du gør dine lokale modeller i stand til at kalde eksterne funktioner.

[← Del 9: Whisper Tale-til-tekst](part9-whisper-voice-transcription.md) | [Del 11: Tool Calling →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokument er blevet oversat ved hjælp af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi bestræber os på nøjagtighed, skal du være opmærksom på, at automatiserede oversættelser kan indeholde fejl eller unøjagtigheder. Det oprindelige dokument på dets modersmål bør betragtes som den autoritative kilde. For kritisk information anbefales professionel menneskelig oversættelse. Vi påtager os intet ansvar for misforståelser eller fejltolkninger, der opstår som følge af brugen af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->