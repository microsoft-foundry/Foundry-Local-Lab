![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Deel 10: Gebruik van aangepaste of Hugging Face-modellen met Foundry Local

> **Doel:** Compileer een Hugging Face-model naar het geoptimaliseerde ONNX-formaat dat Foundry Local vereist, configureer het met een chat-sjabloon, voeg het toe aan de lokale cache en voer inferentie uit via de CLI, REST API en OpenAI SDK.

## Overzicht

Foundry Local wordt geleverd met een samengestelde catalogus van vooraf gecompileerde modellen, maar je bent niet beperkt tot die lijst. Elk transformer-gebaseerd taalmodel dat beschikbaar is op [Hugging Face](https://huggingface.co/) (of lokaal opgeslagen in PyTorch- / Safetensors-formaat) kan worden gecompileerd naar een geoptimaliseerd ONNX-model en via Foundry Local worden bediend.

De compilatiepijplijn gebruikt de **ONNX Runtime GenAI Model Builder**, een commandoregeltool die is inbegrepen bij het `onnxruntime-genai`-pakket. De model builder verzorgt het zware werk: het downloaden van de brongewichten, het converteren naar ONNX-formaat, het toepassen van kwantisatie (int4, fp16, bf16) en het genereren van de configuratiebestanden (inclusief de chat-sjabloon en tokenizer) die Foundry Local verwacht.

In deze lab oefening compileer je **Qwen/Qwen3-0.6B** van Hugging Face, registreer je het bij Foundry Local en chat je er volledig op je apparaat mee.

---

## Leerdoelen

Aan het einde van deze lab oefening kun je:

- Uitleggen waarom het compileren van aangepaste modellen nuttig is en wanneer je dit nodig hebt
- De ONNX Runtime GenAI model builder installeren
- Een Hugging Face-model compileren naar het geoptimaliseerde ONNX-formaat met één commando
- De belangrijkste compilatieparameters begrijpen (uitvoeringsprovider, precisie)
- Het configuratiebestand `inference_model.json` voor de chat-sjabloon maken
- Een gecompileerd model toevoegen aan de Foundry Local-cache
- Inferentie uitvoeren op het aangepaste model via de CLI, REST API en OpenAI SDK

---

## Vereisten

| Vereiste | Details |
|-------------|---------|
| **Foundry Local CLI** | Geïnstalleerd en in je `PATH` ([Deel 1](part1-getting-started.md)) |
| **Python 3.10+** | Vereist door de ONNX Runtime GenAI model builder |
| **pip** | Python package manager |
| **Schijfruimte** | Minimaal 5 GB vrij voor de bron- en gecompileerde modelbestanden |
| **Hugging Face-account** | Sommige modellen vereisen dat je een licentie accepteert voor het downloaden. Qwen3-0.6B gebruikt de Apache 2.0-licentie en is vrij beschikbaar. |

---

## Omgevingssetup

Modelcompilatie vereist verschillende grote Python-pakketten (PyTorch, ONNX Runtime GenAI, Transformers). Maak een aparte virtuele omgeving aan zodat deze niet conflicteert met je systeem-Python of andere projecten.

```bash
# Vanuit de hoofddirectory van de repository
python -m venv .venv
```

Activeer de omgeving:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Werk pip bij om problemen met afhankelijkheidsresolutie te voorkomen:

```bash
python -m pip install --upgrade pip
```

> **Tip:** Als je al een `.venv` hebt van eerdere labs, kun je die hergebruiken. Zorg er gewoon voor dat deze geactiveerd is voordat je verdergaat.

---

## Concept: De compilatiepijplijn

Foundry Local vereist modellen in ONNX-formaat met ONNX Runtime GenAI-configuratie. De meeste open-source modellen op Hugging Face worden verspreid als PyTorch- of Safetensors-gewichten, dus is een conversiestap nodig.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### Wat doet de Model Builder?

1. **Downloadt** het bronmodel van Hugging Face (of leest het van een lokale locatie).
2. **Converteert** de PyTorch- / Safetensors-gewichten naar ONNX-formaat.
3. **Kwantiseert** het model naar een kleinere precisie (bijvoorbeeld int4) om geheugengebruik te verminderen en doorvoer te verbeteren.
4. **Genereert** de ONNX Runtime GenAI-configuratie (`genai_config.json`), de chat-sjabloon (`chat_template.jinja`) en alle tokenizer-bestanden zodat Foundry Local het model kan laden en bedienen.

### ONNX Runtime GenAI Model Builder versus Microsoft Olive

Je kunt verwijzingen tegenkomen naar **Microsoft Olive** als alternatief hulpmiddel voor modeloptimalisatie. Beide tools kunnen ONNX-modellen produceren, maar ze hebben verschillende doelen en afwegingen:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Pakket** | `onnxruntime-genai` | `olive-ai` |
| **Primaire doel** | Converteren en kwantiseren van generatieve AI-modellen voor ONNX Runtime GenAI-inferentie | End-to-end modeloptimalisatie-framework met ondersteuning voor veel backends en hardwaredoelen |
| **Gebruiksgemak** | Eén commando — conversie en kwantisatie in één stap | Workflow-based — configureerbare multi-pass pijplijnen met YAML/JSON |
| **Uitvoerformaat** | ONNX Runtime GenAI-formaat (klaar voor Foundry Local) | Generiek ONNX, ONNX Runtime GenAI of andere formaten afhankelijk van workflow |
| **Hardwaredoelen** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN en meer |
| **Kwantisatieopties** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus grafiekoptimalisaties, laag-voor-laag tuning |
| **Modelbereik** | Generatieve AI-modellen (LLM's, SLM's) | Elk ONNX-converteerbaar model (visie, NLP, audio, multimodaal) |
| **Beste voor** | Snelle compilatie van één model voor lokale inferentie | Productiepijplijnen die fijne optimalisatiecontrole vereisen |
| **Afhankelijkheden** | Gemiddeld (PyTorch, Transformers, ONNX Runtime) | Groter (voegt Olive-framework toe, optionele extra’s per workflow) |
| **Integratie met Foundry Local** | Direct — output is direct compatibel | Vereist `--use_ort_genai`-vlag en extra configuratie |

> **Waarom deze lab oefening de Model Builder gebruikt:** Voor de taak om één enkel Hugging Face-model te compileren en te registreren bij Foundry Local is de Model Builder het eenvoudigste en meest betrouwbare pad. Het produceert het exacte outputformaat dat Foundry Local verwacht in één commando. Als je later geavanceerde optimalisatiefuncties nodig hebt — zoals nauwkeurigheidsbewuste kwantisatie, grafieksnede, of multi-pass tuning — is Olive een krachtige optie om te verkennen. Zie de [Microsoft Olive-documentatie](https://microsoft.github.io/Olive/) voor meer details.

---

## Lab Oefeningen

### Oefening 1: Installeer de ONNX Runtime GenAI Model Builder

Installeer het ONNX Runtime GenAI-pakket, dat de model builder-tool bevat:

```bash
pip install onnxruntime-genai
```

Controleer de installatie door te verifiëren dat de model builder beschikbaar is:

```bash
python -m onnxruntime_genai.models.builder --help
```

Je zou helpinformatie moeten zien met parameters zoals `-m` (modelnaam), `-o` (uitvoermap), `-p` (precisie) en `-e` (uitvoeringsprovider).

> **Opmerking:** De model builder is afhankelijk van PyTorch, Transformers en verschillende andere pakketten. De installatie kan enkele minuten duren.

---

### Oefening 2: Compileer Qwen3-0.6B voor CPU

Voer het volgende commando uit om het Qwen3-0.6B-model van Hugging Face te downloaden en te compileren voor CPU-inferentie met int4-kwantisatie:

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

#### Betekenis van de parameters

| Parameter | Doel | Gebruikte waarde |
|-----------|---------|------------|
| `-m` | De Hugging Face-model-ID of een lokale directory | `Qwen/Qwen3-0.6B` |
| `-o` | Directory waarin het gecompileerde ONNX-model wordt opgeslagen | `models/qwen3` |
| `-p` | Kwantisatieprecisie toegepast tijdens compilatie | `int4` |
| `-e` | ONNX Runtime-uitvoeringsprovider (doelhardware) | `cpu` |
| `--extra_options hf_token=false` | Omzeilt Hugging Face-authenticatie (prima voor openbare modellen) | `hf_token=false` |

> **Hoe lang duurt dit?** De compilatietijd hangt af van je hardware en de modelgrootte. Voor Qwen3-0.6B met int4-kwantisatie op een moderne CPU reken op ongeveer 5 tot 15 minuten. Grotere modellen duren evenredig langer.

Na voltooiing van het commando zou je een map `models/qwen3` moeten zien met daarin de gecompileerde modelbestanden. Controleer de output:

```bash
ls models/qwen3
```

Je zou onder andere de volgende bestanden moeten zien:
- `model.onnx` en `model.onnx.data` — de gecompileerde modelgewichten
- `genai_config.json` — ONNX Runtime GenAI-configuratie
- `chat_template.jinja` — de chat-sjabloon van het model (automatisch gegenereerd)
- `tokenizer.json`, `tokenizer_config.json` — tokeniser-bestanden
- Andere vocabulaire- en configuratiebestanden

---

### Oefening 3: Compileer voor GPU (optioneel)

Als je een NVIDIA GPU met CUDA-ondersteuning hebt, kun je een GPU-geoptimaliseerde variant compileren voor snellere inferentie:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Opmerking:** GPU-compilatie vereist `onnxruntime-gpu` en een werkende CUDA-installatie. Als deze niet aanwezig zijn, geeft de model builder een foutmelding. Je kunt deze oefening overslaan en verdergaan met de CPU-variant.

#### Hardware-specifieke compilatieverwijzing

| Doel | Uitvoeringsprovider (`-e`) | Aanbevolen precisie (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` of `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` of `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Precisie-afwegingen

| Precisie | Grootte | Snelheid | Kwaliteit |
|-----------|------|-------|---------|
| `fp32` | Grootste | Langzaamste | Hoogste nauwkeurigheid |
| `fp16` | Groot | Snel (GPU) | Zeer goede nauwkeurigheid |
| `int8` | Klein | Snel | Enige nauwkeurigheidsverlies |
| `int4` | Kleinste | Snelste | Matig nauwkeurigheidsverlies |

Voor de meeste lokale ontwikkeling biedt `int4` op CPU de beste balans tussen snelheid en middelengebruik. Voor productie-kwaliteit output wordt `fp16` op een CUDA GPU aanbevolen.

---

### Oefening 4: Maak de chat-sjabloonconfiguratie

De model builder genereert automatisch een `chat_template.jinja`-bestand en een `genai_config.json`-bestand in de uitvoermap. Foundry Local heeft echter ook een bestand `inference_model.json` nodig om te weten hoe prompts voor jouw model geformatteerd moeten worden. Dit bestand definieert de modelnaam en de prompt-sjabloon die gebruikersberichten in de juiste speciale tokens wikkelt.

#### Stap 1: Inspecteer de gecompileerde output

Bekijk de inhoud van de directory met gecompileerde modellen:

```bash
ls models/qwen3
```

Je zou bestanden moeten zien zoals:
- `model.onnx` en `model.onnx.data` — de gecompileerde modelgewichten
- `genai_config.json` — ONNX Runtime GenAI-configuratie (automatisch gegenereerd)
- `chat_template.jinja` — de chat-sjabloon van het model (automatisch gegenereerd)
- `tokenizer.json`, `tokenizer_config.json` — tokenizer-bestanden
- Diverse andere configuratie- en vocabulairebestanden

#### Stap 2: Genereer het bestand inference_model.json

Het bestand `inference_model.json` vertelt Foundry Local hoe prompts opgemaakt moeten worden. Maak een Python-script met de naam `generate_chat_template.py` **in de root van het repository** (dezelfde directory waarin je de map `models/` hebt):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Bouw een minimale conversatie om de chattemplate te extraheren
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

# Bouw de structuur van inference_model.json op
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

Voer het script uit vanuit de root van het repository:

```bash
python generate_chat_template.py
```

> **Opmerking:** Het `transformers`-pakket is al geïnstalleerd als afhankelijkheid van `onnxruntime-genai`. Als je een `ImportError` ziet, installeer dan eerst `transformers` met `pip install transformers`.

Het script maakt een bestand `inference_model.json` aan binnen de map `models/qwen3`. Dit bestand vertelt Foundry Local hoe gebruikersinvoer correct gewikkeld moet worden in speciale tokens voor Qwen3.

> **Belangrijk:** Het `"Name"`-veld in `inference_model.json` (in dit script ingesteld op `qwen3-0.6b`) is de **modelalias** die je in alle volgende opdrachten en API-aanroepen zult gebruiken. Als je deze naam wijzigt, pas dan ook de modelnaam aan in Oefeningen 6–10.

#### Stap 3: Verifieer de configuratie

Open `models/qwen3/inference_model.json` en controleer of het een veld `Name` bevat en een `PromptTemplate`-object met de sleutels `assistant` en `prompt`. De prompt-sjabloon moet speciale tokens bevatten zoals `<|im_start|>` en `<|im_end|>` (de exacte tokens hangen af van de chat-sjabloon van het model).

> **Handmatige optie:** Als je het script niet wilt draaien, kun je het bestand ook handmatig maken. Het belangrijkste is dat het `prompt`-veld de volledige chat-sjabloon van het model bevat met `{Content}` als tijdelijke aanduiding voor het bericht van de gebruiker.

---

### Oefening 5: Verifieer de mapstructuur van het model
De modelbouwer plaatst alle gecompileerde bestanden direct in de uitvoermap die u heeft opgegeven. Controleer of de uiteindelijke structuur correct is:

```bash
ls models/qwen3
```

De map moet de volgende bestanden bevatten:

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

> **Opmerking:** In tegenstelling tot sommige andere compilatiegereedschappen maakt de modelbouwer geen geneste submappen aan. Alle bestanden staan direct in de uitvoermap, wat precies is wat Foundry Local verwacht.

---

### Oefening 6: Voeg het Model toe aan de Foundry Local Cache

Vertel aan Foundry Local waar het uw gecompileerde model kan vinden door de map aan zijn cache toe te voegen:

```bash
foundry cache cd models/qwen3
```

Controleer of het model in de cache verschijnt:

```bash
foundry cache ls
```

U zou uw aangepaste model moeten zien naast eerder gecachte modellen (zoals `phi-3.5-mini` of `phi-4-mini`).

---

### Oefening 7: Gebruik het Aangepaste Model met de CLI

Start een interactieve chatsessie met uw nieuw gecompileerde model (de `qwen3-0.6b` alias komt uit het `Name` veld dat u instelde in `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

De `--verbose` vlag toont extra diagnostische informatie, wat nuttig is bij het voor het eerst testen van een aangepast model. Als het model succesvol geladen is, ziet u een interactieve prompt. Probeer een paar berichten:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Typ `exit` of druk op `Ctrl+C` om de sessie te beëindigen.

> **Probleemoplossing:** Als het model niet wil laden, controleer dan het volgende:
> - Het `genai_config.json` bestand is gegenereerd door de modelbouwer.
> - Het `inference_model.json` bestand bestaat en is geldige JSON.
> - De ONNX modelbestanden staan in de juiste map.
> - U heeft voldoende beschikbare RAM (Qwen3-0.6B int4 heeft ongeveer 1 GB nodig).
> - Qwen3 is een redeneermodel dat `<think>` tags produceert. Als u `<think>...</think>` vóór reacties ziet, is dit normaal gedrag. Het prompt-template in `inference_model.json` kan worden aangepast om thinking output te onderdrukken.

---

### Oefening 8: Vraag het Aangepaste Model aan via de REST API

Als u de interactieve sessie in Oefening 7 heeft afgesloten, is het model mogelijk niet meer geladen. Start de Foundry Local-service en laad het model eerst:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Controleer op welke poort de service draait:

```bash
foundry service status
```

Stuur dan een verzoek (vervang `5273` door uw werkelijke poort indien anders):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows opmerking:** Het `curl` commando hierboven gebruikt bash-syntaxis. Gebruik op Windows in plaats daarvan de PowerShell `Invoke-RestMethod` cmdlet hieronder.

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

### Oefening 9: Gebruik het Aangepaste Model met de OpenAI SDK

U kunt verbinding maken met uw aangepaste model met exact dezelfde OpenAI SDK-code die u gebruikte voor de ingebouwde modellen (zie [Deel 3](part3-sdk-and-apis.md)). Het enige verschil is de modelnaam.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local valideert geen API-sleutels
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
  apiKey: "foundry-local", // Foundry Local valideert API-sleutels niet
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

> **Belangrijk punt:** Omdat Foundry Local een OpenAI-compatibele API aanbiedt, werkt alle code die met de ingebouwde modellen werkt ook met uw aangepaste modellen. U hoeft alleen de `model` parameter te wijzigen.

---

### Oefening 10: Test het Aangepaste Model met de Foundry Local SDK

In eerdere labs gebruikte u de Foundry Local SDK om de service te starten, de endpoint te ontdekken en modellen automatisch te beheren. U kunt exact hetzelfde patroon volgen met uw zelf gecompileerde model. De SDK regelt het opstarten van de service en het ontdekken van de endpoint, dus uw code hoeft geen harde `localhost:5273` poort te gebruiken.

> **Opmerking:** Zorg dat de Foundry Local SDK geïnstalleerd is voordat u deze voorbeelden uitvoert:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Voeg `Microsoft.AI.Foundry.Local` en `OpenAI` NuGet-pakketten toe
>
> Bewaar elk scriptbestand **in de hoofdmap van de repository** (dezelfde map als uw `models/` map).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Stap 1: Start de Foundry Local-service en laad het aangepaste model
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Stap 2: Controleer de cache op het aangepaste model
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Stap 3: Laad het model in het geheugen
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Stap 4: Maak een OpenAI-client aan met behulp van het door de SDK ontdekte eindpunt
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Stap 5: Verstuur een streaming chat-voltooiingsverzoek
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

Voer het uit:

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

// Stap 1: Start de Foundry Local-service
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Stap 2: Haal het aangepaste model uit de catalogus
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Stap 3: Laad het model in het geheugen
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Stap 4: Maak een OpenAI-client aan met de via de SDK ontdekte endpoint
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Stap 5: Verstuur een verzoek voor een streaming chat completion
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

Voer het uit:

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

> **Belangrijk punt:** De Foundry Local SDK ontdekt de endpoint dynamisch, dus u hoeft nooit een poortnummer hard te coderen. Dit is de aanbevolen aanpak voor productieapplicaties. Uw zelf gecompileerde model werkt identiek aan ingebouwde catalogusmodellen via de SDK.

---

## Een Model Kiezen om te Compileren

Qwen3-0.6B wordt in deze lab als voorbeeld gebruikt omdat het klein is, snel compileert en vrij beschikbaar is onder de Apache 2.0-licentie. U kunt echter veel andere modellen compileren. Hier zijn enkele suggesties:

| Model | Hugging Face ID | Parameters | Licentie | Opmerkingen |
|-------|-----------------|------------|---------|-------------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0,6B | Apache 2.0 | Zeer klein, snelle compilatie, goed voor testen |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1,7B | Apache 2.0 | Betere kwaliteit, nog steeds snel compileerbaar |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Sterke kwaliteit, vergt meer RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Vereist licentieacceptatie op Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Hoge kwaliteit, grotere download en langere compilatie |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3,8B | MIT | Al in Foundry Local-catalogus (handig voor vergelijking) |

> **Licentieherinnering:** Controleer altijd de licentie van het model op Hugging Face voordat u het gebruikt. Sommige modellen (zoals Llama) vereisen dat u een licentieovereenkomst accepteert en zich aanmeldt met `huggingface-cli login` voordat u ze downloadt.

---

## Concepten: Wanneer Gebruik je Aangepaste Modellen

| Scenario | Waarom Zelf Compileren? |
|----------|------------------------|
| **Een model dat u nodig heeft staat niet in de catalogus** | De Foundry Local catalogus is gecureerd. Als het gewenste model er niet bij staat, compileer het zelf. |
| **Fijn-afgestelde modellen** | Als u een model fijn hebt afgesteld op domeinspecifieke data, moet u uw eigen gewichten compileren. |
| **Specifieke quantisatievereisten** | U wilt misschien een precisie- of quantisatiestrategie die afwijkt van de catalogusstandaard. |
| **Nieuwere modelreleases** | Als er een nieuw model verschijnt op Hugging Face, staat het mogelijk nog niet in de Foundry Local catalogus. Zelf compileren geeft direct toegang. |
| **Onderzoek en experimentatie** | Experimenteer lokaal met verschillende modelarchitecturen, groottes of configuraties voordat u een productiekeuze maakt. |

---

## Samenvatting

In deze lab heeft u geleerd hoe u:

| Stap | Wat U Deed |
|------|------------|
| 1 | Installeerde de ONNX Runtime GenAI modelbouwer |
| 2 | Compileerde `Qwen/Qwen3-0.6B` van Hugging Face naar een geoptimaliseerd ONNX-model |
| 3 | Een `inference_model.json` chat-template configuratiebestand maakte |
| 4 | Het gecompileerde model toevoegde aan de Foundry Local-cache |
| 5 | Interactief chatte met het aangepaste model via de CLI |
| 6 | Het model bevroeg via de OpenAI-compatibele REST API |
| 7 | Verbinding maakte vanuit Python, JavaScript en C# via de OpenAI SDK |
| 8 | Het aangepaste model end-to-end testte met de Foundry Local SDK |

De belangrijkste conclusie is dat **elk transformer-gebaseerd model via Foundry Local kan draaien** zodra het naar ONNX-formaat is gecompileerd. De OpenAI-compatibele API betekent dat al uw bestaande applicatiecode zonder wijzigingen werkt; u hoeft alleen de modelnaam te wijzigen.

---

## Belangrijke Punten

| Concept | Detail |
|---------|--------|
| ONNX Runtime GenAI Model Builder | Zet Hugging Face modellen om naar ONNX-formaat met quantisatie in één commando |
| ONNX formaat | Foundry Local vereist ONNX modellen met ONNX Runtime GenAI configuratie |
| Chat-templates | Het `inference_model.json` bestand vertelt Foundry Local hoe prompts te formatteren voor een bepaald model |
| Hardwaredoelen | Compileer voor CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) of WebGPU afhankelijk van uw hardware |
| Quantisatie | Lagere precisie (int4) verkleint bestand en versnelt ten koste van enige nauwkeurigheid; fp16 behoudt hoge kwaliteit op GPU's |
| API-compatibiliteit | Aangepaste modellen gebruiken dezelfde OpenAI-compatibele API als ingebouwde modellen |
| Foundry Local SDK | De SDK regelt service-opstart, endpoint ontdekking en modelbeheer automatisch voor catalogus- en aangepaste modellen |

---

## Verdere Lectuur

| Bron | Link |
|-------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local gids voor aangepaste modellen | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 model familie | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive documentatie | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Volgende Stappen

Ga verder naar [Deel 11: Tool Calling met Lokale Modellen](part11-tool-calling.md) om te leren hoe u uw lokale modellen externe functies kunt laten aanroepen.

[← Deel 9: Whisper Stemtranscriptie](part9-whisper-voice-transcription.md) | [Deel 11: Tool Calling →](part11-tool-calling.md)