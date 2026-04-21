![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 10: Bruke egendefinerte eller Hugging Face-modeller med Foundry Local

> **Mål:** Kompiler en Hugging Face-modell til det optimaliserte ONNX-formatet som Foundry Local krever, konfigurer den med en chat-mal, legg den til i lokal hurtigbuffer, og kjør inferens mot den ved bruk av CLI, REST API og OpenAI SDK.

## Oversikt

Foundry Local leveres med en kuratert katalog av forhåndskompilerte modeller, men du er ikke begrenset til denne listen. Enhver transformer-basert språkmodell tilgjengelig på [Hugging Face](https://huggingface.co/) (eller lagret lokalt i PyTorch / Safetensors-format) kan kompileres til en optimalisert ONNX-modell og serveres gjennom Foundry Local.

Kompileringspipen bruker **ONNX Runtime GenAI Model Builder**, et kommandolinjeverktøy inkludert i `onnxruntime-genai`-pakken. Modellbyggeren tar seg av det tunge arbeidet: laster ned kildevektene, konverterer dem til ONNX-format, anvender kvantisering (int4, fp16, bf16), og genererer konfigurasjonsfilene (inkludert chat-mal og tokenizer) som Foundry Local forventer.

I denne labben skal du kompilere **Qwen/Qwen3-0.6B** fra Hugging Face, registrere den med Foundry Local, og chatte med den helt på din egen enhet.

---

## Læringsmål

Etter denne labben skal du kunne:

- Forklare hvorfor egendefinert modellkompilering er nyttig og når du kan trenge det
- Installere ONNX Runtime GenAI model builder
- Kompilere en Hugging Face-modell til optimert ONNX-format med en enkelt kommando
- Forstå nøkkelparametrene for kompilering (execution provider, presisjon)
- Lage `inference_model.json` chat-mal konfigurasjonsfil
- Legge til en kompilert modell i Foundry Local sin cache
- Kjøre inferens mot den egendefinerte modellen ved bruk av CLI, REST API, og OpenAI SDK

---

## Forutsetninger

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Installert og i din `PATH` ([Del 1](part1-getting-started.md)) |
| **Python 3.10+** | Kreves av ONNX Runtime GenAI model builder |
| **pip** | Python-pakkehåndterer |
| **Diskplass** | Minst 5 GB ledig for kilde- og kompilerte modelfiler |
| **Hugging Face-konto** | Noen modeller krever at du godtar en lisens før nedlasting. Qwen3-0.6B bruker Apache 2.0-lisens og er fritt tilgjengelig. |

---

## Miljøoppsett

Modellkompilering krever flere store Python-pakker (PyTorch, ONNX Runtime GenAI, Transformers). Opprett et dedikert virtuelt miljø slik at disse ikke forstyrrer systemets Python eller andre prosjekter.

```bash
# Fra depotets rot
python -m venv .venv
```

Aktiver miljøet:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Oppgrader pip for å unngå avhengighetsløsningsproblemer:

```bash
python -m pip install --upgrade pip
```

> **Tips:** Hvis du allerede har en `.venv` fra tidligere labber, kan du gjenbruke den. Bare sørg for at den er aktivert før du fortsetter.

---

## Konsept: Kompileringspipen

Foundry Local krever modeller i ONNX-format med ONNX Runtime GenAI-konfigurasjon. De fleste åpen-kilde-modeller på Hugging Face distribueres som PyTorch- eller Safetensors-vekter, så et konverteringstrinn er nødvendig.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### Hva gjør modellbyggeren?

1. **Laster ned** kildemodellen fra Hugging Face (eller leser den fra en lokal sti).
2. **Konverterer** PyTorch / Safetensors-vektene til ONNX-format.
3. **Kvantisering** av modellen til lavere presisjon (for eksempel int4) for å redusere minnebruk og forbedre gjennomstrømning.
4. **Genererer** ONNX Runtime GenAI-konfigurasjonen (`genai_config.json`), chat-malen (`chat_template.jinja`), og alle tokenizer-filene slik at Foundry Local kan laste og serve modellen.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Du kan støte på referanser til **Microsoft Olive** som et alternativt verktøy for modelloptimalisering. Begge verktøy kan produsere ONNX-modeller, men de har ulike formål og kompromisser:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Pakkenavn** | `onnxruntime-genai` | `olive-ai` |
| **Hovedformål** | Konvertere og kvantisere generative AI-modeller for ONNX Runtime GenAI inferens | End-to-end modelloptimaliseringsrammeverk som støtter mange bakender og maskinvaremål |
| **Brukervennlighet** | Enkelt kommando – ett-trinns konvertering + kvantisering | Arbeidsflyt-basert – konfigurerbare flertrinnspipelines med YAML/JSON |
| **Output-format** | ONNX Runtime GenAI-format (klart for Foundry Local) | Generisk ONNX, ONNX Runtime GenAI, eller andre formater avhengig av arbeidsflyt |
| **Maskinvaremål** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, og flere |
| **Kvantisering-opsjoner** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, pluss grafoptimalisering, lagvis tuning |
| **Modellomfang** | Generative AI-modeller (LLMs, SLMs) | Enhver ONNX-konverterbar modell (visjon, NLP, lyd, multimodal) |
| **Best til** | Rask kompilerig av enkeltmodell for lokal inferens | Produksjonspipelines som trenger finjustert optimaliseringskontroll |
| **Avhengighetsavtrykk** | Moderat (PyTorch, Transformers, ONNX Runtime) | Større (inkluderer Olive-rammeverk, valgfrie tillegg per arbeidsflyt) |
| **Integrasjon med Foundry Local** | Direkte – output er umiddelbart kompatibelt | Krever `--use_ort_genai`-flag og ekstra konfigurasjon |

> **Hvorfor denne labben bruker Model Builder:** For oppgaven med å kompilere en enkelt Hugging Face-modell og registrere den med Foundry Local, er Model Builder den enkleste og mest pålitelige veien. Den produserer nøyaktig det output-formatet Foundry Local forventer med en enkelt kommando. Hvis du senere trenger avanserte optimaliseringsfunksjoner – som presisjonsbevisst kvantisering, grafoperasjoner eller flertrinnsjustering – er Olive et kraftig alternativ å utforske. Se [Microsoft Olive-dokumentasjonen](https://microsoft.github.io/Olive/) for flere detaljer.

---

## Labøvelser

### Øvelse 1: Installer ONNX Runtime GenAI Model Builder

Installer ONNX Runtime GenAI-pakken, som inkluderer model builder-verktøyet:

```bash
pip install onnxruntime-genai
```

Bekreft installasjonen ved å sjekke at model builder er tilgjengelig:

```bash
python -m onnxruntime_genai.models.builder --help
```

Du bør se hjelpetekst som viser parametere som `-m` (modellenavn), `-o` (output-sti), `-p` (presisjon), og `-e` (execution provider).

> **Merk:** Model Builder er avhengig av PyTorch, Transformers og flere andre pakker. Installasjonen kan ta noen minutter.

---

### Øvelse 2: Kompiler Qwen3-0.6B for CPU

Kjør følgende kommando for å laste ned Qwen3-0.6B-modellen fra Hugging Face og kompilere den for CPU-inferens med int4-kvantisering:

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

#### Hva hver parameter gjør

| Parameter | Formål | Verdi brukt |
|-----------|---------|-------------|
| `-m` | Hugging Face-modell ID eller en lokal katalogsti | `Qwen/Qwen3-0.6B` |
| `-o` | Katalog hvor den kompilerte ONNX-modellen lagres | `models/qwen3` |
| `-p` | Kvantiseringspresisjonen som brukes under kompilering | `int4` |
| `-e` | ONNX Runtime execution provider (målmaskinvare) | `cpu` |
| `--extra_options hf_token=false` | Hopper over Hugging Face-autentisering (fint for offentlige modeller) | `hf_token=false` |

> **Hvor lang tid tar dette?** Kompileringstid avhenger av maskinvaren din og modellens størrelse. For Qwen3-0.6B med int4-kvantisering på en moderne CPU, forvent rundt 5 til 15 minutter. Større modeller tar proporsjonalt lengre tid.

Når kommandoen er ferdig, skal du se en `models/qwen3`-mappe som inneholder de kompilerte modelfilene. Sjekk output:

```bash
ls models/qwen3
```

Du bør se filer inkludert:
- `model.onnx` og `model.onnx.data` — de kompilerte modellvektene
- `genai_config.json` — ONNX Runtime GenAI-konfigurasjon
- `chat_template.jinja` — modellens chat-mal (automatisk generert)
- `tokenizer.json`, `tokenizer_config.json` — tokenizer-filer
- Andre vokabular- og konfigurasjonsfiler

---

### Øvelse 3: Kompiler for GPU (valgfritt)

Hvis du har en NVIDIA GPU med CUDA-støtte, kan du kompilere en GPU-optimalisert variant for raskere inferens:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Merk:** GPU-kompilering krever `onnxruntime-gpu` og en fungerende CUDA-installasjon. Hvis disse ikke er tilstede, vil model builder rapportere en feil. Du kan hoppe over denne øvelsen og fortsette med CPU-varianten.

#### Referanse for maskinvarespesifikk kompilering

| Mål | Execution Provider (`-e`) | Anbefalt presisjon (`-p`) |
|--------|-------------------------|---------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` eller `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` eller `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Presisjonsavveininger

| Presisjon | Størrelse | Hastighet | Kvalitet |
|-----------|-----------|-----------|----------|
| `fp32` | Størst | Sakte | Høyest nøyaktighet |
| `fp16` | Stor | Rask (GPU) | Svært god nøyaktighet |
| `int8` | Liten | Rask | Litt nøyaktighetstap |
| `int4` | Minst | Raskest | Moderat nøyaktighetstap |

For de fleste lokale utviklingsscenarioer gir `int4` på CPU den beste balansen mellom hastighet og ressursbruk. For produksjonskvalitet anbefales `fp16` på CUDA GPU.

---

### Øvelse 4: Opprett chat-mal konfigurasjonen

Model Builder genererer automatisk en `chat_template.jinja`-fil og en `genai_config.json`-fil i output-katalogen. Men Foundry Local trenger også en `inference_model.json`-fil for å forstå hvordan den skal formatere prompts for modellen din. Denne filen definerer modellnavnet og promptmalen som pakker brukerbeskjeder inn i riktige spesialtegn.

#### Steg 1: Inspiser det kompilerte output

Lister innholdet i den kompilerte modellmappen:

```bash
ls models/qwen3
```

Du bør se filer som:
- `model.onnx` og `model.onnx.data` — kompilerte modellvekter
- `genai_config.json` — ONNX Runtime GenAI-konfigurasjon (automatisk generert)
- `chat_template.jinja` — modellens chat-mal (automatisk generert)
- `tokenizer.json`, `tokenizer_config.json` — tokenizer-filer
- Flere andre konfigurasjons- og vokabularfiler

#### Steg 2: Generer inference_model.json-filen

`inference_model.json`-filen forteller Foundry Local hvordan den skal formatere prompts. Lag et Python-skript kalt `generate_chat_template.py` **i rotmappen til repositoriet** (samme mappe som inneholder `models/`-mappen):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Bygg en minimal samtale for å hente ut chat-mal
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

# Bygg strukturen for inference_model.json
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

Kjør skriptet fra repositoriets rotmappe:

```bash
python generate_chat_template.py
```

> **Merk:** `transformers`-pakken var allerede installert som en avhengighet av `onnxruntime-genai`. Hvis du får en `ImportError`, kjør `pip install transformers` først.

Skriptet lager en `inference_model.json`-fil inne i `models/qwen3`-mappen. Filen forteller Foundry Local hvordan den skal pakke brukerens input inn i riktige spesialtegn for Qwen3.

> **Viktig:** `"Name"`-feltet i `inference_model.json` (satt til `qwen3-0.6b` i dette skriptet) er **modellaliaset** du vil bruke i alle påfølgende kommandoer og API-kall. Hvis du endrer dette navnet, må du oppdatere modellnavnet i Øvelsene 6–10 tilsvarende.

#### Steg 3: Verifiser konfigurasjonen

Åpne `models/qwen3/inference_model.json` og sjekk at den inneholder et `Name`-felt og et `PromptTemplate`-objekt med `assistant` og `prompt` nøkler. Prompt-malen bør inkludere spesialtegn som `<|im_start|>` og `<|im_end|>` (de eksakte tegnene avhenger av modellens chat-mal).

> **Manuelt alternativ:** Hvis du foretrekker å ikke kjøre skriptet, kan du lage filen manuelt. Det viktigste kravet er at `prompt`-feltet inneholder modellens fulle chat-mal med `{Content}` som plassholder for brukerens melding.

---

### Øvelse 5: Verifiser modellens mappestruktur
Modellbyggeren plasserer alle kompilerte filer direkte i utdata-katalogen du spesifiserte. Bekreft at den endelige strukturen ser riktig ut:

```bash
ls models/qwen3
```

Katalogen bør inneholde følgende filer:

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

> **Merk:** I motsetning til noen andre kompilasjonsverktøy, lager ikke modellbyggeren nestede undermapper. Alle filer ligger direkte i utdata-mappen, noe som er akkurat hva Foundry Local forventer.

---

### Øvelse 6: Legg til modellen i Foundry Local-cache

Fortell Foundry Local hvor den finner den kompilerte modellen ved å legge til katalogen i cachen:

```bash
foundry cache cd models/qwen3
```

Bekreft at modellen vises i cachen:

```bash
foundry cache ls
```

Du bør se din tilpassede modell listet sammen med eventuelle tidligere cachelagrede modeller (som `phi-3.5-mini` eller `phi-4-mini`).

---

### Øvelse 7: Kjør den tilpassede modellen med CLI

Start en interaktiv chat-økt med din nylig kompilerte modell (aliaset `qwen3-0.6b` kommer fra `Name`-feltet du satte i `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose`-flagget viser ekstra diagnostisk informasjon, som er nyttig når du tester en tilpasset modell for første gang. Hvis modellen lastes inn vellykket, vil du se en interaktiv ledetekst. Prøv noen meldinger:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Skriv `exit` eller trykk `Ctrl+C` for å avslutte økten.

> **Feilsøking:** Hvis modellen ikke lastes, sjekk følgende:
> - At `genai_config.json`-filen er generert av modellbyggeren.
> - At `inference_model.json`-filen eksisterer og er gyldig JSON.
> - At ONNX-modellfilene er i riktig katalog.
> - At du har nok tilgjengelig RAM (Qwen3-0.6B int4 trenger omtrent 1 GB).
> - Qwen3 er en resonneringsmodell som produserer `<think>`-tagger. Hvis du ser `<think>...</think>` prefikset på svar, er dette normal oppførsel. Prompmalen i `inference_model.json` kan justeres for å undertrykke tenkeutskrift.

---

### Øvelse 8: Spørr den tilpassede modellen via REST API

Hvis du avsluttet den interaktive økten i øvelse 7, kan det hende modellen ikke lenger er lastet. Start Foundry Local-tjenesten og last inn modellen først:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Sjekk hvilken port tjenesten kjører på:

```bash
foundry service status
```

Deretter sender du en forespørsel (erstatt `5273` med din faktiske port hvis den er annerledes):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows-merknad:** `curl`-kommandoen ovenfor bruker bash-syntaks. På Windows, bruk PowerShell `Invoke-RestMethod`-cmdlet i stedet.

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

### Øvelse 9: Bruk den tilpassede modellen med OpenAI SDK

Du kan koble til din tilpassede modell ved å bruke nøyaktig samme OpenAI SDK-kode som du brukte for innebygde modeller (se [Del 3](part3-sdk-and-apis.md)). Den eneste forskjellen er modellnavnet.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local validerer ikke API-nøkler
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
  apiKey: "foundry-local", // Foundry Local validerer ikke API-nøkler
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

> **Nøkkelpunk:** Fordi Foundry Local eksponerer et OpenAI-kompatibelt API, fungerer all kode som virker med innebygde modeller også med dine tilpassede modeller. Du trenger bare å endre `model`-parameteren.

---

### Øvelse 10: Test den tilpassede modellen med Foundry Local SDK

I tidligere øvelser brukte du Foundry Local SDK for å starte tjenesten, oppdage endepunktet og håndtere modeller automatisk. Du kan følge nøyaktig samme mønster med din egendefinerte, kompilert modell. SDK-en håndterer oppstart av tjenesten og oppdagelse av endepunkt, så koden din trenger ikke hardkode `localhost:5273`.

> **Merk:** Sørg for at Foundry Local SDK er installert før du kjører disse eksemplene:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Legg til NuGet-pakkene `Microsoft.AI.Foundry.Local` og `OpenAI`
>
> Lagre hver skriptfil **i rotmappen til depotet** (samme katalog som `models/`-mappen din).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Trinn 1: Start Foundry Local-tjenesten og last inn den tilpassede modellen
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Trinn 2: Sjekk cachen for den tilpassede modellen
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Trinn 3: Last modellen inn i minnet
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Trinn 4: Opprett en OpenAI-klient ved å bruke SDK-opprettet endepunkt
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Trinn 5: Send en streaming chat fullføringsforespørsel
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

Kjør den:

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

// Trinn 1: Start Foundry Local-tjenesten
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Trinn 2: Hent den tilpassede modellen fra katalogen
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Trinn 3: Last modellen inn i minnet
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Trinn 4: Opprett en OpenAI-klient ved å bruke SDK-opptakspunktet
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Trinn 5: Send en strømmet chat fullføringsforespørsel
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

Kjør den:

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

> **Nøkkelpunk:** Foundry Local SDK oppdager endepunktet dynamisk, så du hardkoder aldri et portnummer. Dette er den anbefalte tilnærmingen for produksjonsapplikasjoner. Din egendefinerte, kompilerte modell fungerer identisk med innebygde katalogmodeller via SDK-en.

---

## Valg av modell for kompilering

Qwen3-0.6B brukes som referanseeksempel i dette laboratoriet fordi den er liten, rask å kompilere og fritt tilgjengelig under Apache 2.0-lisensen. Du kan imidlertid kompilere mange andre modeller. Her er noen forslag:

| Modell | Hugging Face ID | Parametere | Lisens | Notater |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Veldig liten, rask kompilering, god for testing |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Bedre kvalitet, fortsatt rask kompilering |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Sterk kvalitet, krever mer RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Krever lisensgodkjenning på Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Høy kvalitet, større nedlasting og lengre kompilering |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Allerede i Foundry Local-katalogen (nyttig for sammenligning) |

> **Lisenspåminnelse:** Sjekk alltid modellens lisens på Hugging Face før bruk. Noen modeller (som Llama) krever at du godtar en lisensavtale og autentiserer med `huggingface-cli login` før nedlasting.

---

## Konsepter: Når bruke tilpassede modeller

| Scenario | Hvorfor kompilere selv? |
|----------|-------------------------|
| **En modell du trenger finnes ikke i katalogen** | Foundry Local-katalogen er kuratert. Hvis modellen du ønsker ikke er listet, kompiler den selv. |
| **Finjusterte modeller** | Hvis du har finjustert en modell på domenespesifikke data, må du kompilere dine egne vekter. |
| **Spesifikke kvantiseringsbehov** | Du kan ønske en presisjon eller kvantiseringsstrategi som avviker fra katalogens standard. |
| **Nyere modellutgivelser** | Når en ny modell slippes på Hugging Face, kan det hende den ennå ikke er i Foundry Local-katalogen. Kompilering gir umiddelbar tilgang. |
| **Forskning og eksperimentering** | Prøver ulike modellarkitekturer, størrelser eller konfigurasjoner lokalt før du tar et produksjonsvalg. |

---

## Oppsummering

I dette laboratoriet lærte du hvordan du:

| Steg | Hva du gjorde |
|------|---------------|
| 1 | Installerte ONNX Runtime GenAI modellbyggeren |
| 2 | Kompilerte `Qwen/Qwen3-0.6B` fra Hugging Face til en optimalisert ONNX-modell |
| 3 | Opprettet en `inference_model.json` chat-mal konfigurasjonsfil |
| 4 | La den kompilerte modellen til Foundry Local-cache |
| 5 | Kjørte interaktiv chat med den tilpassede modellen via CLI |
| 6 | Spurte modellen gjennom OpenAI-kompatibelt REST API |
| 7 | Koblede til fra Python, JavaScript og C# ved bruk av OpenAI SDK |
| 8 | Testet den tilpassede modellen ende-til-ende med Foundry Local SDK |

Hovedpoenget er at **enhver transformer-basert modell kan kjøres gjennom Foundry Local** når den er kompilert til ONNX-format. Det OpenAI-kompatible API-et betyr at all eksisterende applikasjonskode fungerer uten endringer; du trenger bare å bytte modellnavn.

---

## Viktige punkter

| Konsept | Detalj |
|---------|--------|
| ONNX Runtime GenAI Model Builder | Konverterer Hugging Face-modeller til ONNX-format med kvantisering i en enkelt kommando |
| ONNX-format | Foundry Local krever ONNX-modeller med ONNX Runtime GenAI-konfigurasjon |
| Chat-maler | `inference_model.json`-filen forteller Foundry Local hvordan man formaterer meldinger for en gitt modell |
| Maskinvaremål | Kompiler for CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) eller WebGPU avhengig av maskinvaren din |
| Kvantisering | Lavere presisjon (int4) reduserer størrelse og forbedrer hastighet på bekostning av noe nøyaktighet; fp16 beholder høy kvalitet på GPU-er |
| API-kompatibilitet | Tilpassede modeller bruker samme OpenAI-kompatible API som innebygde modeller |
| Foundry Local SDK | SDK-en håndterer tjenestestart, endepunktoppdagelse og modelllasting automatisk for både katalog- og tilpassede modeller |

---

## Videre lesning

| Ressurs | Lenke |
|---------|--------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local tilpasset modellguide | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3-modellfamilie | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive-dokumentasjon | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Neste steg

Fortsett til [Del 11: Verktøyskalling med lokale modeller](part11-tool-calling.md) for å lære hvordan du aktiverer lokale modeller til å kalle eksterne funksjoner.

[← Del 9: Whisper stemmetranskripsjon](part9-whisper-voice-transcription.md) | [Del 11: Verktøyskalling →](part11-tool-calling.md)