![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 10: Använda anpassade eller Hugging Face-modeller med Foundry Local

> **Mål:** Kompilera en Hugging Face-modell till det optimerade ONNX-format som Foundry Local kräver, konfigurera den med en chattmall, lägg till den i den lokala cachen och kör inferens mot den med hjälp av CLI, REST API och OpenAI SDK.

## Översikt

Foundry Local levereras med en noggrant utvald katalog av förkompilerade modeller, men du är inte begränsad till den listan. Vilken transformerbaserad språkmodell som helst tillgänglig på [Hugging Face](https://huggingface.co/) (eller lagrad lokalt i PyTorch / Safetensors-format) kan kompileras till en optimerad ONNX-modell och serveras genom Foundry Local.

Kompileringspipen använder **ONNX Runtime GenAI Model Builder**, ett kommandoradsverktyg som ingår i `onnxruntime-genai`-paketet. Modellbyggaren hanterar det tunga arbetet: laddar ner källvikterna, konverterar dem till ONNX-format, applicerar kvantisering (int4, fp16, bf16), och genererar konfigurationsfilerna (inklusive chattmallen och tokeniseraren) som Foundry Local förväntar sig.

I detta laboration kommer du att kompilera **Qwen/Qwen3-0.6B** från Hugging Face, registrera den i Foundry Local och chatta med den helt på din enhet.

---

## Läromål

I slutet av denna labb kommer du att kunna:

- Förklara varför anpassad modellkompilering är användbar och när du kan behöva den
- Installera ONNX Runtime GenAI model builder
- Kompilera en Hugging Face-modell till optimerat ONNX-format med ett enda kommando
- Förstå nyckelparametrar för kompilering (exekveringsleverantör, precision)
- Skapa konfigurationsfilen `inference_model.json` för chattmallen
- Lägga till en kompilerad modell i Foundry Local-cachen
- Köra inferens mot den anpassade modellen med CLI, REST API och OpenAI SDK

---

## Förutsättningar

| Krav | Detaljer |
|-------------|---------|
| **Foundry Local CLI** | Installerad och på din `PATH` ([Del 1](part1-getting-started.md)) |
| **Python 3.10+** | Krävs av ONNX Runtime GenAI model builder |
| **pip** | Python-pakethanterare |
| **Diskutrymme** | Minst 5 GB ledigt för käll- och kompilerade modellfiler |
| **Hugging Face-konto** | Vissa modeller kräver att du accepterar en licens innan nedladdning. Qwen3-0.6B använder Apache 2.0-licensen och är fritt tillgänglig. |

---

## Miljöinställning

Modellkompilering kräver flera stora Python-paket (PyTorch, ONNX Runtime GenAI, Transformers). Skapa en dedikerad virtuell miljö så att dessa inte stör din system-Python eller andra projekt.

```bash
# Från arkivets rotpunkt
python -m venv .venv
```

Aktivera miljön:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Uppgradera pip för att undvika beroendeproblem:

```bash
python -m pip install --upgrade pip
```

> **Tips:** Om du redan har en `.venv` från tidigare lägen kan du återanvända den. Se bara till att den är aktiverad innan du fortsätter.

---

## Koncept: Kompileringspipen

Foundry Local kräver modeller i ONNX-format med konfiguration för ONNX Runtime GenAI. De flesta öppen källkodsmodeller på Hugging Face distribueras som PyTorch eller Safetensors-vikter, så en konverteringssteg behövs.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### Vad gör Model Builder?

1. **Laddar ner** källmodellen från Hugging Face (eller läser från en lokal sökväg).
2. **Konverterar** PyTorch / Safetensors-vikterna till ONNX-format.
3. **Kvantiserar** modellen till en lägre precision (t.ex. int4) för att minska minnesanvändning och öka genomströmningen.
4. **Genererar** ONNX Runtime GenAI-konfiguration (`genai_config.json`), chattmall (`chat_template.jinja`) och alla tokeniserarfiler så att Foundry Local kan ladda och servera modellen.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Du kan stöta på referenser till **Microsoft Olive** som ett alternativt verktyg för modelloptimering. Båda verktygen kan producera ONNX-modeller, men de tjänar olika syften och har olika kompromisser:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Paket** | `onnxruntime-genai` | `olive-ai` |
| **Primärt syfte** | Konvertera och kvantisera generativa AI-modeller för ONNX Runtime GenAI-inferens | Slut-till-slut modelloptimeringsramverk som stödjer många backends och hårdvarumål |
| **Användarvänlighet** | Enkel kommando — enstegs konvertering + kvantisering | Flödesbaserat — konfigurerbara flerstegs pipelines med YAML/JSON |
| **Utdataformat** | ONNX Runtime GenAI-format (klart för Foundry Local) | Generisk ONNX, ONNX Runtime GenAI, eller andra format beroende på arbetsflöde |
| **Hårdvarumål** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN med mera |
| **Kvantiseringalternativ** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus grafoptimeringar, lagervisel finjustering |
| **Modellomfång** | Generativa AI-modeller (LLM, SLM) | Alla ONNX-konverterbara modeller (vision, NLP, ljud, multimodala) |
| **Bäst för** | Snabb kompilering av enstaka modell för lokal inferens | Produktionsflöden som behöver finjusterad optimeringskontroll |
| **Beroendeavtryck** | Måttligt (PyTorch, Transformers, ONNX Runtime) | Större (lägger till Olive-ramverket, valfria tillägg per arbetsflöde) |
| **Integrering med Foundry Local** | Direkt — utdata är omedelbart kompatibel | Kräver `--use_ort_genai`-flagga och ytterligare konfiguration |

> **Varför detta labb använder Model Builder:** För uppgiften att kompilera en enstaka Hugging Face-modell och registrera den i Foundry Local är Model Builder den enklaste och mest pålitliga vägen. Den producerar det exakta utdataformat Foundry Local förväntar sig med ett enda kommando. Om du senare behöver avancerade optimeringsfunktioner — som noggrannhetsmedveten kvantisering, grafoperationer eller flerstegsfinjustering — är Olive ett kraftfullt alternativ att utforska. Se [Microsoft Olive dokumentation](https://microsoft.github.io/Olive/) för mer information.

---

## Laborationsövningar

### Övning 1: Installera ONNX Runtime GenAI Model Builder

Installera ONNX Runtime GenAI-paketet som inkluderar model builder-verktyget:

```bash
pip install onnxruntime-genai
```

Verifiera installationen genom att kontrollera att model builder är tillgänglig:

```bash
python -m onnxruntime_genai.models.builder --help
```

Du bör se hjälputdata som listar parametrar som `-m` (modellnamn), `-o` (utdataväg), `-p` (precision) och `-e` (exekveringsleverantör).

> **Notera:** Model Builder är beroende av PyTorch, Transformers och flera andra paket. Installationen kan ta några minuter.

---

### Övning 2: Kompilera Qwen3-0.6B för CPU

Kör följande kommando för att ladda ner Qwen3-0.6B från Hugging Face och kompilera den för CPU-inferens med int4-kvantisering:

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

#### Vad varje parameter gör

| Parameter | Syfte | Använt värde |
|-----------|---------|------------|
| `-m` | Hugging Face modell-ID eller lokal katalogväg | `Qwen/Qwen3-0.6B` |
| `-o` | Katalog där den kompilerade ONNX-modellen sparas | `models/qwen3` |
| `-p` | Kvantiseringsprecision som appliceras under kompilering | `int4` |
| `-e` | ONNX Runtime exekveringsleverantör (mål-hårdvara) | `cpu` |
| `--extra_options hf_token=false` | Hoppar över Hugging Face-autentisering (fungerar för publika modeller) | `hf_token=false` |

> **Hur lång tid tar det?** Kompileringstiden beror på din hårdvara och modellens storlek. För Qwen3-0.6B med int4-kvantisering på en modern CPU, räkna med ungefär 5 till 15 minuter. Större modeller tar proportionellt längre tid.

När kommandot är klart ska du se en katalog `models/qwen3` som innehåller de kompilerade modellfilerna. Verifiera utdata:

```bash
ls models/qwen3
```

Du bör se filer inklusive:
- `model.onnx` och `model.onnx.data` — de kompilerade modellvikterna
- `genai_config.json` — ONNX Runtime GenAI-konfiguration
- `chat_template.jinja` — modellens chattmall (automatgenererad)
- `tokenizer.json`, `tokenizer_config.json` — tokeniserfiler
- Andra vokabulär- och konfigurationsfiler

---

### Övning 3: Kompilera för GPU (Valfritt)

Om du har ett NVIDIA GPU med CUDA-stöd kan du kompilera en GPU-optimerad variant för snabbare inferens:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Notera:** GPU-kompilering kräver `onnxruntime-gpu` och en fungerande CUDA-installation. Om detta inte finns kommer model builder att rapportera ett fel. Du kan hoppa över denna övning och fortsätta med CPU-varianten.

#### Hårdvaruspecifik kompilering referens

| Mål | Exekveringsleverantör (`-e`) | Rekommenderad precision (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` eller `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` eller `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Precisionens kompromisser

| Precision | Storlek | Hastighet | Kvalitet |
|-----------|------|-------|---------|
| `fp32` | Störst | Långsammaste | Högst noggrannhet |
| `fp16` | Stor | Snabb (GPU) | Mycket bra noggrannhet |
| `int8` | Liten | Snabb | Lätt noggrannhetsförlust |
| `int4` | Minst | Snabbast | Måttlig noggrannhetsförlust |

För de flesta lokala utvecklingsfall ger `int4` på CPU den bästa balansen mellan hastighet och resursanvändning. För produktionskvalitet rekommenderas `fp16` på en CUDA-GPU.

---

### Övning 4: Skapa konfiguration för chattmall

Model builder genererar automatiskt en fil `chat_template.jinja` och en `genai_config.json` i utmatningskatalogen. Men Foundry Local behöver även en fil `inference_model.json` för att förstå hur man formaterar promptar för din modell. Denna fil definierar modellnamnet och promptmallen som omsluter användarmeddelanden i korrekta specialtokens.

#### Steg 1: Inspektera den kompilerade utmatningen

Lista innehållet i den kompilerade modellkatalogen:

```bash
ls models/qwen3
```

Du bör se filer såsom:
- `model.onnx` och `model.onnx.data` — de kompilerade modellvikterna
- `genai_config.json` — ONNX Runtime GenAI-konfiguration (automatgenererad)
- `chat_template.jinja` — modellens chattmall (automatgenererad)
- `tokenizer.json`, `tokenizer_config.json` — tokeniserfiler
- Flera andra konfigurations- och vokabulärfiler

#### Steg 2: Generera filen inference_model.json

Filen `inference_model.json` talar om för Foundry Local hur den ska formatera promptar. Skapa ett Python-skript som heter `generate_chat_template.py` **i rotkatalogen av ditt repo** (samma katalog som innehåller din `models/`-mapp):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Skapa en minimal konversation för att extrahera chattmallen
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

# Skapa strukturen för inference_model.json
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

Kör skriptet från rotkatalogen:

```bash
python generate_chat_template.py
```

> **Notera:** Paketet `transformers` var redan installerat som beroende till `onnxruntime-genai`. Om du ser ett `ImportError`, installera det först med `pip install transformers`.

Skriptet producerar en fil `inference_model.json` inuti katalogen `models/qwen3`. Den filen berättar för Foundry Local hur användarindata ska omslutas med rätt specialtokens för Qwen3.

> **Viktigt:** Fältet `"Name"` i `inference_model.json` (satt till `qwen3-0.6b` i skriptet) är **modellaliaset** du kommer använda i alla efterföljande kommandon och API-anrop. Om du ändrar detta namn, uppdatera modellnamnet i Övningar 6–10 därefter.

#### Steg 3: Verifiera konfigurationen

Öppna `models/qwen3/inference_model.json` och bekräfta att den innehåller ett `Name`-fält och ett `PromptTemplate`-objekt med `assistant` och `prompt`-nycklar. Promptmallen bör inkludera specialtokens såsom `<|im_start|>` och `<|im_end|>` (exakta tokens beror på modellens chattmall).

> **Manuellt alternativ:** Om du föredrar att inte köra skriptet kan du skapa filen manuellt. Det viktiga är att `prompt`-fältet innehåller modellens fullständiga chattmall med `{Content}` som platshållare för användarens meddelande.

---

### Övning 5: Verifiera modellens katalogstruktur

Modellbyggaren placerar alla kompilerade filer direkt i den utmatningskatalog du angav. Kontrollera att den slutgiltiga strukturen ser korrekt ut:

```bash
ls models/qwen3
```

Katalogen bör innehålla följande filer:

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

> **Notera:** Till skillnad från vissa andra kompilationsverktyg skapar inte modellbyggaren inbäddade undermappar. Alla filer ligger direkt i utmatningsmappen, vilket är precis vad Foundry Local förväntar sig.

---

### Övning 6: Lägg till modellen i Foundry Local Cache

Berätta för Foundry Local var den ska hitta din kompilerade modell genom att lägga till katalogen i dess cache:

```bash
foundry cache cd models/qwen3
```

Verifiera att modellen visas i cachen:

```bash
foundry cache ls
```

Du bör se din anpassade modell listad tillsammans med tidigare cachade modeller (såsom `phi-3.5-mini` eller `phi-4-mini`).

---

### Övning 7: Kör den anpassade modellen med CLI

Starta en interaktiv chatt-session med din nykompilerade modell (aliaset `qwen3-0.6b` kommer från fältet `Name` som du satte i `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Flaggan `--verbose` visar ytterligare diagnostisk information, vilket är hjälpsamt när du testar en anpassad modell för första gången. Om modellen laddas framgångsrikt kommer du att se en interaktiv prompt. Prova några meddelanden:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Skriv `exit` eller tryck på `Ctrl+C` för att avsluta sessionen.

> **Felsökning:** Om modellen inte laddas, kontrollera följande:
> - Filen `genai_config.json` genererades av modellbyggaren.
> - Filen `inference_model.json` finns och är giltig JSON.
> - ONNX-modellfilerna är i korrekt katalog.
> - Du har tillräckligt med tillgängligt RAM (Qwen3-0.6B int4 behöver ungefär 1 GB).
> - Qwen3 är en resonemangsmodell som producerar `<think>` taggar. Om du ser `<think>...</think>` före svar är detta normalt. Promptsatsen i `inference_model.json` kan justeras för att undertrycka tänkande-utdata.

---

### Övning 8: Anropa den anpassade modellen via REST API

Om du avslutade den interaktiva sessionen i Övning 7 kan modellen vara avlastad. Starta Foundry Local-tjänsten och ladda modellen först:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Kontrollera på vilken port tjänsten körs:

```bash
foundry service status
```

Skicka sedan en förfrågan (ersätt `5273` med din faktiska port om den skiljer sig):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows-notis:** `curl`-kommandot ovan använder bash-syntax. På Windows använd istället PowerShell-kommandot `Invoke-RestMethod` nedan.

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

### Övning 9: Använd den anpassade modellen med OpenAI SDK

Du kan ansluta till din anpassade modell med exakt samma OpenAI SDK-kod som du använde för de inbyggda modellerna (se [Del 3](part3-sdk-and-apis.md)). Den enda skillnaden är modellnamnet.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local validerar inte API-nycklar
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
  apiKey: "foundry-local", // Foundry Local validerar inte API-nycklar
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

> **Viktigt:** Eftersom Foundry Local exponerar en OpenAI-kompatibel API fungerar all kod som fungerar med de inbyggda modellerna även med dina anpassade modeller. Du behöver bara ändra `model` parametern.

---

### Övning 10: Testa den anpassade modellen med Foundry Local SDK

I tidigare labbar använde du Foundry Local SDK för att starta tjänsten, upptäcka endpoint och hantera modeller automatiskt. Du kan följa exakt samma mönster med din egenkompilerade modell. SDK hanterar tjänstestart och endpointupptäckt, så din kod behöver inte hårdkoda `localhost:5273`.

> **Notera:** Säkerställ att Foundry Local SDK är installerat innan du kör dessa exempel:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Lägg till NuGet-paketen `Microsoft.AI.Foundry.Local` och `OpenAI`
>
> Spara varje skriptfil **i roten av repot** (samma katalog som din `models/`-mapp).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Steg 1: Starta Foundry Local-tjänsten och ladda den anpassade modellen
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Steg 2: Kontrollera cachen för den anpassade modellen
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Steg 3: Ladda modellen i minnet
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Steg 4: Skapa en OpenAI-klient med SDK-upptäckt slutpunkt
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Steg 5: Skicka en strömmande chattkompletteringsförfrågan
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

Kör det:

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

// Steg 1: Starta Foundry Local-tjänsten
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Steg 2: Hämta den anpassade modellen från katalogen
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Steg 3: Ladda modellen i minnet
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Steg 4: Skapa en OpenAI-klient med hjälp av den av SDK upptäckta slutpunkten
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Steg 5: Skicka en streaming chattfullföljandeförfrågan
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

Kör det:

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

> **Viktigt:** Foundry Local SDK upptäcker endpoint dynamiskt, så du hårdkodar aldrig portnummer. Detta är den rekommenderade metoden för produktionsapplikationer. Din egenkompilerade modell fungerar identiskt med inbyggda katalogmodeller genom SDK.

---

## Val av modell att kompilera

Qwen3-0.6B används som referensexempel i denna labb eftersom den är liten, snabb att kompilera och fritt tillgänglig under Apache 2.0-licensen. Men du kan kompilera många andra modeller. Här är några förslag:

| Modell | Hugging Face ID | Parametrar | Licens | Noter |
|--------|-----------------|------------|--------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Mycket liten, snabb kompilering, bra för testning |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Bättre kvalitet, fortfarande snabb kompilering |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Stark kvalitet, kräver mer RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Kräver licensacceptans på Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Hög kvalitet, större nedladdning och längre kompilering |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Redan i Foundry Local-katalogen (användbar för jämförelse) |

> **Licenspåminnelse:** Kontrollera alltid modellens licens på Hugging Face innan användning. Vissa modeller (som Llama) kräver att du accepterar en licensavtal och autentiserar med `huggingface-cli login` innan nedladdning.

---

## Begrepp: När man ska använda anpassade modeller

| Scenario | Varför kompilera egen modell? |
|----------|-------------------------------|
| **En modell du behöver finns inte i katalogen** | Foundry Local-katalogen är selektiv. Om modellen du vill ha inte finns med, kompilera den själv. |
| **Finjusterade modeller** | Om du har finjusterat en modell på domänspecifika data behöver du kompilera egna vikter. |
| **Specifika kvantiseringskrav** | Du kanske vill ha en precision eller kvantiseringsstrategi som skiljer sig från katalogens standard. |
| **Nyare modellutgåvor** | När en ny modell släpps på Hugging Face kan den ännu inte finnas i Foundry Local-katalogen. Genom att själv kompilera får du omedelbar tillgång. |
| **Forskning och experimenterande** | Prova olika modellarkitekturer, storlekar eller konfigurationer lokalt innan du beslutar dig för produktion. |

---

## Sammanfattning

I denna labb lärde du dig att:

| Steg | Vad du gjorde |
|-------|--------------|
| 1 | Installerade ONNX Runtime GenAI modellbyggare |
| 2 | Kompilerade `Qwen/Qwen3-0.6B` från Hugging Face till en optimerad ONNX-modell |
| 3 | Skapade en konfigurationsfil för chattmall `inference_model.json` |
| 4 | Lade till den kompilerade modellen i Foundry Local-cache |
| 5 | Körde interaktiv chatt med den anpassade modellen via CLI |
| 6 | Anropade modellen via OpenAI-kompatibelt REST API |
| 7 | Anslöt från Python, JavaScript och C# med OpenAI SDK |
| 8 | Testade den anpassade modellen end-to-end med Foundry Local SDK |

Det viktiga är att **vilken transformerbaserad modell som helst kan köras via Foundry Local** när den har kompilerats till ONNX-format. Den OpenAI-kompatibla API:n innebär att all befintlig applikationskod fungerar utan ändringar; du behöver bara byta modellnamn.

---

## Viktiga insikter

| Begrepp | Detalj |
|---------|--------|
| ONNX Runtime GenAI Modellbyggare | Omvandlar Hugging Face-modeller till ONNX-format med kvantisering i ett enda kommando |
| ONNX-format | Foundry Local kräver ONNX-modeller med ONNX Runtime GenAI-konfiguration |
| Chattmallar | Filen `inference_model.json` talar om för Foundry Local hur prompts ska formateras för en viss modell |
| Hårdvarumål | Kompilera för CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) eller WebGPU beroende på din hårdvara |
| Kvantisering | Lägre precision (int4) minskar storlek och ökar hastighet på bekostnad av viss noggrannhet; fp16 bibehåller hög kvalitet på GPU |
| API-kompatibilitet | Anpassade modeller använder samma OpenAI-kompatibla API som inbyggda modeller |
| Foundry Local SDK | SDK hanterar tjänstestart, endpointupptäckt och modellhantering automatiskt för både katalog- och egna modeller |

---

## Vidare läsning

| Resurs | Länk |
|--------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local anpassad modell-guide | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 modelfamilj | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive dokumentation | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Nästa steg

Fortsätt till [Del 11: Tool Calling med lokala modeller](part11-tool-calling.md) för att lära dig hur du aktiverar dina lokala modeller att anropa externa funktioner.

[← Del 9: Whisper rösttranskription](part9-whisper-voice-transcription.md) | [Del 11: Tool Calling →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfriskrivning**:  
Detta dokument har översatts med hjälp av AI-översättningstjänsten [Co-op Translator](https://github.com/Azure/co-op-translator). Även om vi strävar efter noggrannhet, var vänlig observera att automatiska översättningar kan innehålla fel eller brister. Det ursprungliga dokumentet på dess modersmål bör betraktas som den auktoritativa källan. För kritisk information rekommenderas professionell mänsklig översättning. Vi ansvarar inte för några missförstånd eller feltolkningar som uppstår vid användning av denna översättning.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->