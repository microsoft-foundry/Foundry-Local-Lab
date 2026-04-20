![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 10: Kohandatud või Hugging Face mudelite kasutamine Foundry Localiga

> **Eesmärk:** Kompileerida Hugging Face mudel optimeeritud ONNX formaati, mida Foundry Local nõuab, seadistada see vestlusemplaadiga, lisada see kohalikku vahemällu ja käivitada selle peal järeldus CLI, REST API ja OpenAI SDK abil.

## Ülevaade

Foundry Local tarnitakse hoolikalt valitud eelkompileeritud mudelite kataloogiga, kuid see nimekiri ei piira sind. Igasugune transformeritel põhinev keelemudel, mis on saadaval [Hugging Face'is](https://huggingface.co/) (või salvestatud lokaalselt PyTorch / Safetensors formaadis), saab kompileerida optimeeritud ONNX mudeliks ja pakkuda Foundry Locali kaudu.

Kompileerimistorustik kasutab **ONNX Runtime GenAI Model Builderit**, käsureatööriista, mis kuulub `onnxruntime-genai` paketti. Mudeliehitaja tegeleb raske tööga: allalaadib lähtekaalud, konverteerib need ONNX formaati, rakendab kvantiseerimist (int4, fp16, bf16) ja genereerib konfiguratsioonifailid (sh vestlusemple ja tokenisaatori), mida Foundry Local ootab.

Selles laboris kompileerite **Qwen/Qwen3-0.6B** Hugging Face'ist, registreerite selle Foundry Localiga ja vestlete selle mudeliga täielikult oma seadmes.

---

## Õpieesmärgid

Selle labori lõpuks oskad:

- Selgitada, miks on kasulik kompileerida kohandatud mudel ja millal seda võib vaja minna
- Paigaldada ONNX Runtime GenAI mudeliehitaja
- Kompileerida Hugging Face mudel optimeeritud ONNX formaati ühe käsuga
- Mõista peamisi kompileerimisparameetreid (täitmistalitaja, täpsus)
- Luua `inference_model.json` vestlusemplaadi konfiguratsioonifail
- Lisada kompileeritud mudel Foundry Locali vahemällu
- Käivitada järeldus kohandatud mudeli peal kasutades CLI-d, REST API-t ja OpenAI SDK-d

---

## Eeltingimused

| Nõue | Detailid |
|-------------|---------|
| **Foundry Local CLI** | Paigaldatud ja `PATH`-is ([Osa 1](part1-getting-started.md)) |
| **Python 3.10+** | Nõutav ONNX Runtime GenAI mudeliehitaja jaoks |
| **pip** | Python pakihaldur |
| **Kettaruum** | Vähemalt 5 GB vaba lähte- ja kompileeritud mudelifailide jaoks |
| **Hugging Face konto** | Mõned mudelid nõuavad litsentsi aktsepteerimist enne allalaadimist. Qwen3-0.6B kasutab Apache 2.0 litsentsi ja on vabalt saadaval. |

---

## Keskkonna seadistus

Mudelite kompileerimiseks on vaja mitut suurt Python paketti (PyTorch, ONNX Runtime GenAI, Transformers). Loo spetsiaalne virtuaalne keskkond, et need ei segaks sinu süsteemi Pythonit ega teisi projekte.

```bash
# Hoidla juurest
python -m venv .venv
```

Aktiveeri keskkond:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Uuenda pip'i, et vältida sõltuvuste lahendamise probleeme:

```bash
python -m pip install --upgrade pip
```

> **Vihje:** Kui sul on varasematest laboritest juba olemas `.venv`, võid seda uuesti kasutada. Lihtsalt veendu, et see oleks aktiveeritud enne jätkamist.

---

## Kontseptsioon: kompileerimistorustik

Foundry Local nõuab ONNX formaadis mudeleid koos ONNX Runtime GenAI konfiguratsiooniga. Enamus avatud lähtekoodiga mudeleid Hugging Face'is on PyTorch või Safetensors kaaludena, mistõttu on vajalik konverteerimisetapp.

![Kohandatud mudeli kompileerimise torustik](../../../images/custom-model-pipeline.svg)

### Mida teeb mudeliehitaja?

1. **Laeb alla** algse mudeli Hugging Face'ist (või loeb selle lokaalsest kaustast).
2. **Konverteerib** PyTorch / Safetensors kaalud ONNX formaati.
3. **Kvantiseerib** mudeli väiksema täpsusega (näiteks int4), et vähendada mälukasutust ja parandada läbilaskevõimet.
4. **Genereerib** ONNX Runtime GenAI konfiguratsiooni (`genai_config.json`), vestlusemplaadi (`chat_template.jinja`) ja kõik tokenisaatori failid, et Foundry Local saaks mudelit laadida ja pakkuda.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Võid kohtuda mainetega **Microsoft Olive'ist** kui alternatiivsest tööriistast mudelite optimeerimiseks. Mõlemad tööriistad suudavad toota ONNX mudeleid, kuid neil on erinevad eesmärgid ja kompromissid:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Pakk** | `onnxruntime-genai` | `olive-ai` |
| **Peamine eesmärk** | GenAI mudelite konverteerimine ja kvantiseerimine ONNX Runtime GenAI järelduseks | Lõpp-lõpuni mudelite optimeerimise raamistik, toetab palju tagapõhjasid ja riistvarasihtmärke |
| **Kasutusmugavus** | Ühekordne käsk — üheastmeline konvertimine + kvantiseerimine | Töökäigu-põhine — konfigureeritavad mitmeastmelised torustikud YAML/JSON-ga |
| **Väljundvorming** | ONNX Runtime GenAI formaat (valmis Foundry Localile) | Generiline ONNX, ONNX Runtime GenAI või muud vormid vastavalt töövoole |
| **Riistvarasihtmärgid** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN jpt |
| **Kvantiseerimisvõimalused** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, pluss graafi optimeerimised, kihipõhine häälestus |
| **Mudelimaaulatus** | Generatiivsed AI mudelid (LLM-id, SLM-id) | Igasugused ONNX-ist muudetavad mudelid (nägemine, NLP, heli, multimodaalne) |
| **Parim kasutus** | Kiire ühe mudeli kompileerimine kohalikuks järelduseks | Tootmistorustikud, mis vajavad peenhäälestust optimeerimise üle |
| **Sõltuvuste sisaldus** | Mõõdukas (PyTorch, Transformers, ONNX Runtime) | Suurem (lisab Olive raamistiku, valikulised täiendused töövoolu järgi) |
| **Foundry Local integratsioon** | Otsene — väljund sobib kohe | Vajab `--use_ort_genai` lippu ja lisakonfiguratsiooni |

> **Miks see labor kasutab Model Builderit:** Ühe Hugging Face mudeli kompileerimiseks ja Foundry Localiga registreerimiseks on Model Builder lihtsaim ja usaldusväärseim tee. See toodab ühe käsuga täpselt selle väljundvorming, mida Foundry Local ootab. Kui hiljem on vaja täpsemaid optimeerimisvõimalusi — nagu täpsust arvestav kvantiseerimine, graafikirurgia või mitmeastmeline häälestus — on Olive võimas lahendus uurimiseks. Vaata [Microsoft Olive dokumentatsiooni](https://microsoft.github.io/Olive/) lisainfo saamiseks.

---

## Labori ülesanded

### Ülesanne 1: Paigalda ONNX Runtime GenAI Model Builder

Paigalda ONNX Runtime GenAI pakk, mis sisaldab mudeliehitaja tööriista:

```bash
pip install onnxruntime-genai
```

Kontrolli paigaldust, veendumaks, et mudeliehitaja on saadaval:

```bash
python -m onnxruntime_genai.models.builder --help
```

Peaksid nägema abiväljundit, mis loetleb parameetreid nagu `-m` (mudeli nimi), `-o` (väljundi tee), `-p` (täpsus) ja `-e` (täitmistalitaja).

> **Märkus:** Mudeliehitaja sõltub PyTorchist, Transformersist ja mitmest muust paketist. Paigaldus võib võtta paar minutit.

---

### Ülesanne 2: Kompileeri Qwen3-0.6B CPU jaoks

Käivita järgmine käsklus, et laadida Qwen3-0.6B mudel Hugging Face'ist alla ja kompileerida see CPU järelduseks koos int4 kvantiseerimisega:

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

#### Iga parameetri tähendus

| Parameeter | Eesmärk | Kasutatud väärtus |
|-----------|---------|------------|
| `-m` | Hugging Face mudeli ID või kohalik kataloogi tee | `Qwen/Qwen3-0.6B` |
| `-o` | Kataloog, kuhu salvestatakse kompileeritud ONNX mudel | `models/qwen3` |
| `-p` | Kompileerimisel rakendatav kvantiseerimistäpsus | `int4` |
| `-e` | ONNX Runtime täitmistalitaja (riistvarasihtmärk) | `cpu` |
| `--extra_options hf_token=false` | Jätab vahele Hugging Face autentimise (sobib avalike mudelite jaoks) | `hf_token=false` |

> **Kui kaua see võtab?** Kompileerimise aeg sõltub sinu riistvarast ja mudeli suurusest. Qwen3-0.6B puhul int4 kvantiseerimisega kaasaegsel CPU-l võtab see ligikaudu 5 kuni 15 minutit. Suuremad mudelid vajavad proportsionaalselt kauem.

Kui käsklus on lõppenud, peaksid nägema `models/qwen3` kataloogi, mis sisaldab kompileeritud mudelifailide komplekti. Kontrolli väljundit:

```bash
ls models/qwen3
```

Peaksid nägema faile nagu:
- `model.onnx` ja `model.onnx.data` — kompileeritud mudeli kaalukirjed
- `genai_config.json` — ONNX Runtime GenAI konfiguratsioon
- `chat_template.jinja` — mudeli vestlusemplaat (automaatselt loodud)
- `tokenizer.json`, `tokenizer_config.json` — tokenisaatori failid
- Teised sõnavara- ja konfiguratsioonifailid

---

### Ülesanne 3: Kompileerimine GPU jaoks (vabatahtlik)

Kui sul on NVIDIA GPU CUDA toe jaoks, võid kompileerida GPU-optimeeritud variandi kiiremaks järelduseks:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Märkus:** GPU kompileerimiseks on vaja `onnxruntime-gpu` ja toimivat CUDA installi. Kui need puuduvad, annab mudeliehitaja vea. Sa võid selle ülesande vahele jätta ja jätkata CPU variandiga.

#### Riistvaraspetsiifilised kompileerimisviited

| Sihtmärk | Täitmistalitaja (`-e`) | Soovitatav täpsus (`-p`) |
|----------|------------------------|--------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` või `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` või `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Täpsuse kompromissid

| Täpsus | Suurus | Kiirus | Kvaliteet |
|---------|---------|---------|-----------|
| `fp32` | Kõige suurem | Kõige aeglasem | Kõrgeim täpsus |
| `fp16` | Suur | Kiire (GPU peal) | Väga hea täpsus |
| `int8` | Väike | Kiire | Mõõdukas täpsuskadu |
| `int4` | Kõige väiksem | Kõige kiirem | Mõõdukas täpsuskadu |

Enamikus kohalikus arenduses pakub CPU peal `int4` parimat tasakaalu kiiruse ja ressursikasutuse vahel. Tootmiskvaliteedi väljundi jaoks soovitatakse CUDA GPU peal `fp16`.

---

### Ülesanne 4: Loo vestlusemplaadi konfiguratsioon

Mudeliehitaja genereerib automaatselt `chat_template.jinja` faili ja `genai_config.json` faili väljundkataloogi. Kuid Foundry Local vajab lisaks ka `inference_model.json` faili, et mõista, kuidas vormistada mudeli prompt’e. See fail määratleb mudeli nime ja prompti malli, mis ümbritseb kasutajate sõnumeid õigete erimärkidega.

#### Samm 1: Uuri kompileeritud väljundit

Loetle kompileeritud mudeli kataloogi sisu:

```bash
ls models/qwen3
```

Peaksid nägema faile nagu:
- `model.onnx` ja `model.onnx.data` — kompileeritud mudeli kaalukirjed
- `genai_config.json` — ONNX Runtime GenAI konfiguratsioon (automaatselt genereeritud)
- `chat_template.jinja` — mudeli vestlusemplaat (automaatselt loodud)
- `tokenizer.json`, `tokenizer_config.json` — tokenisaatori failid
- Mitmesugused muud konfiguratsiooni- ja sõnavarafailid

#### Samm 2: Genereeri inference_model.json fail

`inference_model.json` fail ütleb Foundry Localile, kuidas vormindada prompt’e. Loo Python skript nimega `generate_chat_template.py` **repositooriumi juurkausta** (samasse kataloogi, kus on `models/` kaust):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Koosta minimaalne vestlus vestlusmalli väljavõtmiseks
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

# Koosta inference_model.json struktuur
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

Käivita skript repositooriumi juurest:

```bash
python generate_chat_template.py
```

> **Märkus:** `transformers` paket oli juba paigaldatud `onnxruntime-genai` sõltuvusena. Kui saad `ImportError` tõrke, käivita esmalt `pip install transformers`.

Skript genereerib `inference_model.json` faili kataloogi `models/qwen3`. Fail ütleb Foundry Localile, kuidas kasutaja sisestust õigesti spetsiaalsete erimärkidega (tokenitega) ümbritseda Qwen3 puhul.

> **Oluline:** Väli `"Name"` failis `inference_model.json` (mille skript seab väärtuseks `qwen3-0.6b`) on **mudeli alias**, mida kasutad järgmistes käsklustes ja API päringutes. Kui muudad seda nime, uuenda mudeli nimi ülesannetes 6 kuni 10 vastavalt.

#### Samm 3: Kontrolli konfiguratsiooni

Ava fail `models/qwen3/inference_model.json` ja veendu, et seal on välja `Name` ja objekt `PromptTemplate` võtmetega `assistant` ja `prompt`. Prompti mall peaks sisaldama erimärke nagu `<|im_start|>` ja `<|im_end|>` (täpsed tokenid sõltuvad mudeli vestlusemplaadist).

> **Käsitsi alternatiiv:** Kui sa ei soovi skripti kasutada, saad faili luua käsitsi. Peamine nõue on, et `prompt` väli sisaldaks mudeli täielikku vestlusemplaati, kus `{Content}` on kohatäide kasutaja sõnumi jaoks.

---

### Ülesanne 5: Kontrolli mudeli kataloogi struktuuri
Mudeli koostaja asetab kõik kompileeritud failid otse teie määratud väljundkausta. Veenduge, et lõplik struktuur näeb välja õige:

```bash
ls models/qwen3
```

Kaust peaks sisaldama järgmisi faile:

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

> **Märkus:** Erinevalt mõnest teisest kompileerimistööriistast ei loo mudeli koostaja pesastatud alamkaustu. Kõik failid asuvad otse väljundkaustas, mis on täpselt see, mida Foundry Local ootab.

---

### Harjutus 6: Lisa mudel Foundry Local vahemällu

Ütle Foundry Localile, kust leida teie kompileeritud mudelit, lisades kausta vahemällu:

```bash
foundry cache cd models/qwen3
```

Veenduge, et mudel ilmub vahemälus:

```bash
foundry cache ls
```

Peate nägema oma kohandatud mudelit teiste varasemate vahemällu salvestatud mudelite kõrval (näiteks `phi-3.5-mini` või `phi-4-mini`).

---

### Harjutus 7: Käivita kohandatud mudel CLI-ga

Alustage interaktiivset vestlusseanssi oma äsja kompileeritud mudeliga (`qwen3-0.6b` alias pärineb `Name` väljalt, mille määrasite failis `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Lipp `--verbose` kuvab täiendavat diagnostilist teavet, mis on kasulik kohandatud mudeli esmakordselt testimisel. Kui mudel laeb edukalt, näete interaktiivset käsureaprompti. Proovige mõnda sõnumit:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Käsu lõpetamiseks tippige `exit` või vajutage `Ctrl+C`.

> **Veaotsing:** Kui mudel ei lae, kontrollige järgmist:
> - Faili `genai_config.json` genereeris mudeli koostaja.
> - Fail `inference_model.json` eksisteerib ja on kehtiv JSON.
> - ONNX mudelifailid asuvad õiges kaustas.
> - Teil on piisavalt vaba RAM-i (Qwen3-0.6B int4 vajab umbes 1 GB).
> - Qwen3 on mõtlemismudel, mis toodab `<think>` silte. Kui vastustes on eesliitena `<think>...</think>`, on see normaalne käitumine. Failis `inference_model.json` olevat prompti(mall) saab kohandada, et mõtlemistulundit mitte kuvada.

---

### Harjutus 8: Esita päring kohandatud mudelile REST API kaudu

Kui lahkusite interaktiivsest seansist harjutuses 7, võib mudel enam laetud olla. Käivitage esmalt Foundry Local teenus ja laadige mudel:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Kontrollige, millisel pordil teenus töötab:

```bash
foundry service status
```

Saada seejärel päring (asendage `5273` vastavalt oma portiga, kui see erineb):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windowsi märkus:** Ülaltoodud `curl` käsk kasutab bash süntaksit. Windowsis kasutage selle asemel PowerShelli käsu `Invoke-RestMethod`.

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

### Harjutus 9: Kasuta kohandatud mudelit OpenAI SDK-ga

Võite oma kohandatud mudeliga ühendada täpselt sama OpenAI SDK koodi abil, mida kasutasite sisseehitatud mudelite jaoks (vt [osa 3](part3-sdk-and-apis.md)). Ainuke erinevus on mudeli nimi.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local ei kontrolli API võtmeid
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
  apiKey: "foundry-local", // Foundry Local ei valideeri API võtmeid
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

> **Oluline:** Kuna Foundry Local pakub OpenAI-kompatible API, töötab mis tahes kood, mis sobib sisseehitatud mudelitega, ka teie kohandatud mudelitega. Peate ainult vahetama `model` parameetri väärtust.

---

### Harjutus 10: Testi kohandatud mudelit Foundry Local SDK-ga

Varasemates laborites kasutasite Foundry Local SDK-d teenuse käivitamiseks, lõpp-punkti avastamiseks ja mudelite haldamiseks automaatselt. Samasugust mustrit saate kasutada ka oma kohandatud kompileeritud mudeliga. SDK haldab teenuse käivitust ja lõpp-punkti otsimist, nii et teie kood ei pea `localhost:5273` porti kõvasti kodeerima.

> **Märkus:** Veenduge, et Foundry Local SDK on enne näidiskoodide käivitamist paigaldatud:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Lisa NuGet paketid `Microsoft.AI.Foundry.Local` ja `OpenAI`
>
> Salvestage iga skriptifail **hoidla juurkausta** (sama kausta, kus on teie `models/` kaust).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Samm 1: Käivita Foundry Local teenus ja laadi kohandatud mudel
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Samm 2: Kontrolli vahemälu kohandatud mudeli jaoks
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Samm 3: Laadi mudel mällu
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Samm 4: Loo OpenAI klient, kasutades SDK poolt leitud otspunkti
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Samm 5: Saada voogedastusega vestluse lõpetamise päring
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

Käivitage:

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

// Samm 1: Käivita Foundry Local teenus
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Samm 2: Hangi kohandatud mudel kataloogist
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Samm 3: Laadi mudel mällu
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Samm 4: Loo OpenAI klient kasutades SDK abil leitud lõpp-punkti
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Samm 5: Saada voogedastuse vestlusvalmimise päring
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

Käivitage:

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

> **Oluline:** Foundry Local SDK leiab lõpp-punkti dünaamiliselt, seega ei kodeerita kunagi pordinumbrit käsitsi. See on soovitatav lähenemine tootmiskeskkonna rakenduste jaoks. Teie kohandatud kompileeritud mudel toimib täpselt samamoodi nagu sisseehitatud kataloogi mudelid SDK kaudu.

---

## Mudeli valik kompileerimiseks

Qwen3-0.6B on selles laboris kasutatud näidismudel, kuna see on väike, kiiresti kompileeritav ja vabalt saadaval Apache 2.0 litsentsi alusel. Kuid kompileerida saab palju teisi mudeleid. Siin on mõned soovitused:

| Mudel | Hugging Face ID | Parameetrid | Litsents | Märkused |
|-------|-----------------|-------------|----------|----------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Väga väike, kiire kompileerimine, hea testimiseks |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Parem kvaliteet, ikka kiire kompileerida |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Kõrge kvaliteet, vajab rohkem RAM-i |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Nõuab litsentsi aktsepteerimist Hugging Face'is |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Kõrge kvaliteet, suurem allalaadimine ja pikem kompileerimine |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Juba Foundry Local kataloogis (kasulik võrdluseks) |

> **Litsentsi meeldetuletus:** Kontrollige alati mudeli litsentsi Hugging Face'is enne selle kasutamist. Mõned mudelid (näiteks Llama) nõuavad litsentsilepingu aktsepteerimist ja autentimist `huggingface-cli login` abil enne allalaadimist.

---

## Mõisted: millal kasutada kohandatud mudeleid

| Stsenaarium | Miks ise kompileerida? |
|-------------|-----------------------|
| **Mudelit kataloogis pole** | Foundry Local kataloog on kureeritud. Kui teie soovitud mudelit kataloogis ei ole, kompileerige see ise. |
| **Peenhäälestatud mudelid** | Kui olete mudelit spetsiaalse domeeni andmete peal peenhäälestanud, peate kompileerima oma kaalud ise. |
| **Erilised kvantiseerimisnõuded** | Võite soovida täpsus- või kvantiseerimisstrateegiat, mis erineb kataloogi vaikeseadetest. |
| **Uuemad mudeli väljaanded** | Kui Hugging Face'is ilmub uus mudel, ei pruugi see veel Foundry Local kataloogis olla. Oma mudeli kompileerimine annab kohe juurdepääsu. |
| **Uuringud ja eksperimendid** | Katsetage erinevaid mudeli arhitektuure, suurusi või konfiguratsioone lokaalselt enne tootmiskasutusele asumist. |

---

## Kokkuvõte

Selles laboris õppisite, kuidas:

| Samm | Mida tegite |
|------|-------------|
| 1 | Paigaldasite ONNX Runtime GenAI mudeli koostaja |
| 2 | Kompileerisite mudeli `Qwen/Qwen3-0.6B` Hugging Face'ist optimeeritud ONNX mudeliks |
| 3 | Lõite faili `inference_model.json` vestlusmustriga konfiguratsiooni jaoks |
| 4 | Lisasite kompileeritud mudeli Foundry Local vahemällu |
| 5 | Käivitasite kohandatud mudeli CLI kaudu interaktiivse vestluse |
| 6 | Esitasite mudelile päringu OpenAI-kompatible REST API kaudu |
| 7 | Ühendusite Pythonist, JavaScriptist ja C#-st OpenAI SDK abil |
| 8 | Testisite kohandatud mudelit täielikult Foundry Local SDK-ga |

Oluline on see, et **iga transformer-põhine mudel saab läbi Foundry Local käivitada**, kui see on kompileeritud ONNX formaati. OpenAI-kompatible API tähendab, et kogu teie olemasolev rakenduskood töötab muutusteta; peate vaid mudeli nime vahetama.

---

## Peamised järeldused

| Mõiste | Üksikasjad |
|--------|------------|
| ONNX Runtime GenAI mudeli koostaja | Konverteerib Hugging Face mudelid ONNX formaati, lisades kvantiseerimise ühe käsuga |
| ONNX formaat | Foundry Local nõuab ONNX mudeleid koos ONNX Runtime GenAI konfiguratsiooniga |
| Vestlusmallid | Fail `inference_model.json` ütleb Foundry Localile, kuidas formatteerida antud mudeli küsimused |
| Riistvarasihtmärgid | Kompileerige CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) või WebGPU jaoks sõltuvalt riistvarast |
| Kvantiseerimine | Madalam täpsus (int4) vähendab suurust ja kiirendab, mõningate täpsustuskuludega; fp16 säilitab kõrge kvaliteedi GPU-del |
| API ühilduvus | Kohandatud mudelid kasutavad sama OpenAI-kompatible API-d nagu sisseehitatud mudelid |
| Foundry Local SDK | SDK haldab teenuse käivitust, lõpp-punkti leidmist ja mudelite laadimist automaatselt nii kataloogi kui ka kohandatud mudelite jaoks |

---

## Lisalugemine

| Ressurss | Link |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local kohandatud mudeli juhend | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 mudelifirma | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive dokumentatsioon | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Järgmised sammud

Jätkake [Osa 11: Tööriistade kutsumine kohalike mudelitega](part11-tool-calling.md) ja õppige, kuidas lubada oma kohalikud mudelid välisfunktsioonide käivitamist.

[← Osa 9: Whisperi kõnetuvastus](part9-whisper-voice-transcription.md) | [Osa 11: Tööriistade kutsumine →](part11-tool-calling.md)