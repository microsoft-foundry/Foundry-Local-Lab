![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 10 dalis: Pasirinktinių arba Hugging Face modelių naudojimas su Foundry Local

> **Tikslas:** Sukompiliuoti Hugging Face modelį į optimizuotą ONNX formatą, kurio reikalauja Foundry Local, sukonfigūruoti jį su pokalbių šablonu, pridėti į vietinį kešą ir vykdyti spėjimą naudojant CLI, REST API ir OpenAI SDK.

## Apžvalga

Foundry Local yra komplektuojamas su parinktais iš anksto sudėtais modelių katalogais, tačiau nesate ribojami tik tuo sąrašu. Bet koks transformatorių pagrindu veikiantis kalbos modelis, prieinamas [Hugging Face](https://huggingface.co/) arba saugomas lokaliai PyTorch / Safetensors formatu, gali būti paverstas į optimizuotą ONNX modelį ir aptarnaujamas per Foundry Local.

Kompiliavimo procesas naudoja **ONNX Runtime GenAI Model Builder**, komandų eilutės įrankį, įtrauktą į `onnxruntime-genai` paketą. Modelių kūrėjas atlieka pagrindinius darbus: atsisiunčia šaltinio svorius, konvertuoja juos į ONNX formatą, taiko kiekybinimą (int4, fp16, bf16) ir generuoja konfigūracijos failus (įskaitant pokalbių šabloną ir žodžių skaidytuvą), kurių laukia Foundry Local.

Šiame laboratorijos darbe sukursite **Qwen/Qwen3-0.6B** iš Hugging Face, užregistruosite jį Foundry Local ir bendrausite su juo visiškai savo įrenginyje.

---

## Mokymosi tikslai

Darbo pabaigoje gebėsite:

- Paaiškinti, kodėl naudinga kompliuoti pasirinktinius modelius ir kada tai gali prireikti
- Įdiegti ONNX Runtime GenAI modelių kūrėją
- Vienu komandos raginimu paversti Hugging Face modelį į optimizuotą ONNX formatą
- Suprasti pagrindinius kompliavimo parametrus (vykdymo tiekėją, tikslumą)
- Sukurti `inference_model.json` pokalbių šablono konfigūracijos failą
- Pridėti sukurtą modelį į Foundry Local kešą
- Vykdyti spėjimą naudojant pasirinktinius modelius per CLI, REST API ir OpenAI SDK

---

## Reikalavimai

| Reikalavimas | Detalės |
|-------------|---------|
| **Foundry Local CLI** | Įdiegta ir yra jūsų `PATH` aplinkoje ([1 dalis](part1-getting-started.md)) |
| **Python 3.10+** | Būtinas ONNX Runtime GenAI modelių kūrėjui |
| **pip** | Python paketų valdymo įrankis |
| **Disko vieta** | Bent 5 GB laisvos vietos šaltinio ir sukurtų modelių failams |
| **Hugging Face paskyra** | Kai kurie modeliai reikalauja sutikti su licencija prieš atsisiunčiant. Qwen3-0.6B turi Apache 2.0 licenciją ir yra laisvai pasiekiamas. |

---

## Aplinkos paruošimas

Modelių kompiuliavimui reikia kelių didelių Python paketų (PyTorch, ONNX Runtime GenAI, Transformers). Sukurkite atskirą virtualią aplinką, kad jie netrukdytų jūsų sistemos Python ar kitiems projektams.

```bash
# Iš saugyklos šaknies
python -m venv .venv
```

Aktyvuokite aplinką:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Atnaujinkite pip, kad išvengtumėte priklausomybių konfliktų:

```bash
python -m pip install --upgrade pip
```

> **Patarimas:** Jei turite `.venv` iš ankstesnių laboratorijų, galite jį naudoti pakartotinai. Tik įsitikinkite, kad jis aktyvuotas prieš tęsdami.

---

## Koncepcija: Kompiliavimo procesas

Foundry Local reikalauja modelių ONNX formatu su ONNX Runtime GenAI konfigūracija. Dauguma atviro kodo modelių Hugging Face pateikiami kaip PyTorch arba Safetensors svoriai, todėl reikalinga konvertavimo dalis.

![Pasirinktinio modelio kompiliavimo procesas](../../../images/custom-model-pipeline.svg)

### Ką atlieka Modelių kūrėjas?

1. **Atsisiunčia** šaltinio modelį iš Hugging Face (arba skaito jį iš vietinio katalogo).
2. **Konvertuoja** PyTorch / Safetensors svorius į ONNX formatą.
3. **Atlieką kiekybinimą** iki mažesnio tikslumo (pvz., int4), kad sumažintų atminties naudojimą ir pagerintų pralaidumą.
4. **Sukuria** ONNX Runtime GenAI konfigūraciją (`genai_config.json`), pokalbių šabloną (`chat_template.jinja`) ir visus žodžių skaidytuvo failus, kad Foundry Local galėtų užkrauti ir aptarnauti modelį.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Galite sutikti nuorodų į **Microsoft Olive** kaip alternatyvų įrankį modelių optimizavimui. Abu įrankiai sukuria ONNX modelius, bet skirti skirtingoms reikmėms ir turi skirtingas kompromisų savybes:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Paketas** | `onnxruntime-genai` | `olive-ai` |
| **Pagrindinis tikslas** | Konvertuoti ir kiekybinti generatyvinius DI modelius ONNX Runtime GenAI spėjimui | Pilnas modelių optimizavimo procesas, palaikantis daug atskirų sistemų ir aparatūros |
| **Naudojimo paprastumas** | Viena komanda — vieno žingsnio konvertavimas + kiekybinimas | Darbo srauto sistema — kelių etapų procesai konfigūruojami YAML/JSON |
| **Išvesties formatas** | ONNX Runtime GenAI formatas (paruoštas Foundry Local) | Bendras ONNX, ONNX Runtime GenAI arba kiti formatai priklausomai nuo darbo srauto |
| **Aparatūros palaikymas** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN ir kiti |
| **Kiekybinimo galimybės** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plius grafų optimizacijos, sluoksnio lygiu reguliavimas |
| **Modelių aprėptis** | Generatyviniai DI modeliai (LLM, SLM) | Bet koks modelis, kurį galima paversti į ONNX (vizija, NLP, garsas, multimodaliniai) |
| **Geriausia paskirtis** | Greitam vietiniam vieno modelio kompiliavimui | Produkcijos linijoms, kurioms reikalinga detali optimizavimo kontrolė |
| **Priklausomybių apimtis** | Vidutinė (PyTorch, Transformers, ONNX Runtime) | Didesnė (prideda Olive sistemą, priedus pagal darbo srautą) |
| **Foundry Local integracija** | Tiesioginė — išvestis iš karto suderinama | Reikia žymos `--use_ort_genai` ir papildomos konfigūracijos |

> **Kodėl šiame darbe naudojamas Modelių kūrėjas:** Jei norite sukompiliuoti vieną Hugging Face modelį ir užregistruoti jį Foundry Local, šis įrankis yra paprasčiausias ir patikimiausias. Jis vienu komandos raginimu sukuria reikiamą formatą. Jei vėliau reikės pažangesnių optimizacijos funkcijų – pavyzdžiui, tikslaus tikslumo kiekybinimo, grafų intervencijų ar kelių etapų reguliavimo – Olive yra galinga alternatyva. Plačiau žiūrėkite [Microsoft Olive dokumentacijoje](https://microsoft.github.io/Olive/).

---

## Laboratoriniai pratimai

### 1 pratimas: Įdiekite ONNX Runtime GenAI Model Builder

Įdiekite ONNX Runtime GenAI paketą, kuris apjungia modelių kūrėją:

```bash
pip install onnxruntime-genai
```

Patikrinkite, ar įrankis įdiegtas ir pasiekiamas:

```bash
python -m onnxruntime_genai.models.builder --help
```

Turėtumėte pamatyti pagalbos tekstą, kuriame yra parametrai kaip `-m` (modelio pavadinimas), `-o` (išvesties vieta), `-p` (tikslumas), `-e` (vykdymo tiekėjas).

> **Pastaba:** Modelių kūrėjas priklauso nuo PyTorch, Transformers ir kitų paketų. Įdiegimas gali užtrukti kelias minutes.

---

### 2 pratimas: Sukompiliuokite Qwen3-0.6B CPU

Vykdykite šią komandą, norėdami atsisiųsti Qwen3-0.6B modelį iš Hugging Face ir sukompiliuoti jį CPU spėjimui su int4 kiekybinimu:

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

#### Ką reiškia kiekvienas parametras

| Parametras | Paskirtis | Naudojama reikšmė |
|-----------|-----------|-------------------|
| `-m` | Hugging Face modelio ID arba vietinio katalogo kelias | `Qwen/Qwen3-0.6B` |
| `-o` | Katalogas, kur bus saugomi sukompiliuoti ONNX modelio failai | `models/qwen3` |
| `-p` | Kompiliavimo metu taikomas kiekybinimo tikslumas | `int4` |
| `-e` | ONNX Runtime vykdymo tiekėjas (tikslinei aparatūrai) | `cpu` |
| `--extra_options hf_token=false` | Praleidžia Hugging Face autentifikaciją (tinka viešiems modeliams) | `hf_token=false` |

> **Kiek užtrunka?** Kompiliavimo laikas priklauso nuo jūsų aparatūros ir modelio dydžio. Dėl Qwen3-0.6B su int4 kiekybinimu moderniame CPU tai trunka apie 5–15 minučių. Didesni modeliai užtrunka proporcingai ilgiau.

Pabaigus komandą matysite `models/qwen3` katalogą su sukompiliuotais modelio failais. Patikrinkite išvestį:

```bash
ls models/qwen3
```

Turėtumėte matyti failus:
- `model.onnx` ir `model.onnx.data` — sukaupti modelio svoriai
- `genai_config.json` — ONNX Runtime GenAI konfigūracija
- `chat_template.jinja` — modelio pokalbių šablonas (automatiškai sugeneruotas)
- `tokenizer.json`, `tokenizer_config.json` — žodžių skaidytuvo failai
- Kiti žodynų ir konfigūraciniai failai

---

### 3 pratimas: Kompiliavimas GPU (pasirinktinai)

Jei turite NVIDIA GPU su CUDA palaikymu, galite sukompiliuoti GPU optimizuotą versiją spartesniam spėjimui:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Pastaba:** GPU kompiliavimui reikalingas `onnxruntime-gpu` paketas ir tinkama CUDA įrenginio aplinka. Jei jų nėra, modelių kūrėjas praneš apie klaidą. Galite praleisti šį pratimą ir tęsti su CPU versija.

#### Aparatūros specifika

| Tikslas | Vykdymo tiekėjas (`-e`) | Rekomenduojamas tikslumas (`-p`) |
|--------|---------------------------|----------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` arba `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` arba `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Tikslumo kompromisai

| Tikslumas | Dydis | Greitis | Kokybė |
|-----------|-------|---------|---------|
| `fp32` | Didžiausias | Lėčiausias | Aukščiausias tikslumas |
| `fp16` | Didelis | Greitas (GPU) | Labai geras tikslumas |
| `int8` | Mažas | Greitas | Šiek tiek nukritęs tikslumas |
| `int4` | Mažiausias | Greičiausias | Vidutinis tikslumo praradimas |

Daugeliui vietinės plėtros atvejų `int4` CPU suteikia geriausią greičio ir resursų balansą. Produkcijai rekomenduojama `fp16` CUDA GPU.

---

### 4 pratimas: Sukurkite pokalbių šablono konfigūraciją

Modelių kūrėjas automatiškai generuoja `chat_template.jinja` failą ir `genai_config.json` kataloge sukompiliuotame kelyje. Tačiau Foundry Local taip pat reikia `inference_model.json` failo, kuris nusako, kaip formuoti vartotojo užklausas jūsų modeliui. Šis failas apibrėžia modelio pavadinimą ir užklausos šabloną, kuris apgaubia vartotojo žinutę tinkamais specialiais ženklais.

#### 1 žingsnis: Peržiūrėkite sukompiliuotą išvestį

Išvardinkite sukompiliuoto modelio katalogo turinį:

```bash
ls models/qwen3
```

Turėtumėte matyti failus, tokius kaip:
- `model.onnx` ir `model.onnx.data` — sukaupti modelio svoriai
- `genai_config.json` — ONNX Runtime GenAI konfigūracija (automatiškai sukurta)
- `chat_template.jinja` — modelio pokalbių šablonas (automatiškai sukurta)
- `tokenizer.json`, `tokenizer_config.json` — žodžių skaidytuvo failai
- Įvairūs kiti konfigūracijos ir žodyno failai

#### 2 žingsnis: Generuokite inference_model.json failą

`inference_model.json` failas nurodo Foundry Local, kaip formuoti užklausas. Sukurkite Python skriptą pavadinimu `generate_chat_template.py` **dešiniajame šakninio katalogo lygyje** (ten, kur yra jūsų `models/` katalogas):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Sukurkite minimalų pokalbį, kad išgautumėte pokalbio šabloną
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

# Sukurkite inference_model.json struktūrą
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

Vykdykite skriptą iš šakninio katalogo:

```bash
python generate_chat_template.py
```

> **Pastaba:** `transformers` paketas jau įdiegtas su `onnxruntime-genai`. Jei gaunate `ImportError`, paleiskite `pip install transformers`.

Skriptas sugeneruos `inference_model.json` failą kataloge `models/qwen3`. Šis failas nurodys Foundry Local, kaip apgaubti vartotojo įvestį teisingais specialiais ženklais Qwen3 modeliui.

> **Svarbu:** Laukas `"Name"` `inference_model.json` faile (šiuo skriptu nustatytas kaip `qwen3-0.6b`) yra **modelio slapyvardis**, kurį naudosite visuose tolimesniuose komandose ir API kvietimuose. Jei jį pakeisite, atitinkamai atnaujinkite modelio pavadinimą pratimų 6–10 nurodymuose.

#### 3 žingsnis: Patikrinkite konfigūraciją

Atidarykite `models/qwen3/inference_model.json` ir įsitikinkite, kad jame yra laukas `Name` ir objektas `PromptTemplate` su `assistant` ir `prompt` raktiniais žodžiais. Užklausos šablonas turėtų būti su specialiais ženklais kaip `<|im_start|>` ir `<|im_end|>` (tikslūs ženklai priklauso nuo modelio pokalbių šablono).

> **Vietinis sprendimas:** Jei nenorite paleisti skripto, galite sukurti failą rankiniu būdu. Pagrindinis reikalavimas – `prompt` laukas turi apimti visą modelio pokalbių šabloną, kuriame `{Content}` yra vartotojo žinutės vieta.

---

### 5 pratimas: Patikrinkite modelio katalogo struktūrą


Modelio kūrėjas deda visus sukompiliuotus failus tiesiai į nurodytą išvesties katalogą. Patikrinkite, ar galutinė struktūra atrodo teisinga:

```bash
ls models/qwen3
```

Katalogas turėtų turėti šiuos failus:

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

> **Pastaba:** Skirtingai nei kai kurie kiti kompiliavimo įrankiai, modelio kūrėjas nesukuria sudėtinių poaplankių. Visi failai yra tiesiogiai įdėti į išvesties aplanką, o taip tiksliai tikisi Foundry Local.

---

### Pratimai 6: Pridėkite modelį į Foundry Local kešą

Pasakykite Foundry Local, kur rasti jūsų sukompiliuotą modelį, pridėdami katalogą į jo kešą:

```bash
foundry cache cd models/qwen3
```

Patikrinkite, ar modelis atsirado keše:

```bash
foundry cache ls
```

Turėtumėte matyti savo pasirinktą modelį kartu su anksčiau kešuotais modeliais (pvz., `phi-3.5-mini` arba `phi-4-mini`).

---

### Pratimai 7: Paleiskite pasirinktinį modelį naudojant CLI

Pradėkite interaktyvų pokalbio seansą su naujai sukompiliuotu modeliu (aliasas `qwen3-0.6b` kyla iš `Name` lauko, kurį nustatėte `inference_model.json` faile):

```bash
foundry model run qwen3-0.6b --verbose
```

Žymeklis `--verbose` rodo papildomą diagnostinę informaciją, kuri naudinga pirmą kartą testuojant pasirinktą modelį. Jei modelis įkeliamas sėkmingai, pamatysite interaktyvų raginimą. Išbandykite kelis pranešimus:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Įveskite `exit` arba paspauskite `Ctrl+C`, kad baigtumėte sesiją.

> **Trikčių šalinimas:** Jei modelis nepavyksta įkelti, patikrinkite šiuos dalykus:
> - ar `genai_config.json` failas buvo sugeneruotas modelio kūrėjo;
> - ar `inference_model.json` failas egzistuoja ir yra galiojantis JSON;
> - ar ONNX modelio failai yra tinkamame kataloge;
> - ar turite pakankamai laisvos RAM (Qwen3-0.6B int4 reikia maždaug 1 GB);
> - Qwen3 yra loginis modelis, kuris generuoja `<think>` žymes. Jei prie atsakymų matote `<think>...</think>`, tai yra normalu. Galite koreguoti prašymo šabloną `inference_model.json`, kad slėptumėte mąstymo išvestį.

---

### Pratimai 8: Užklauskite pasirinktą modelį per REST API

Jei baigėte interaktyvią sesiją 7 pratime, modelis gali būti nebeįkeltas. Pirmiausia paleiskite Foundry Local tarnybą ir įkelkite modelį:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Patikrinkite, kokiu prievadu veikia paslauga:

```bash
foundry service status
```

Tada siųskite užklausą (pakeiskite `5273` į savo faktinį prievadą, jei jis skiriasi):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows pastaba:** Aukščiau parodyta `curl` komanda naudoja bash sintaksę. Windows sistemoje vietoje jos naudokite PowerShell `Invoke-RestMethod` komandą žemiau.

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

### Pratimai 9: Naudokite pasirinktą modelį su OpenAI SDK

Galite prisijungti prie savo pasirinkto modelio naudodami tą pačią OpenAI SDK kodą, kurį naudojote integruotiems modeliams (žr. [3 dalį](part3-sdk-and-apis.md)). Vienintelis skirtumas yra modelio pavadinimas.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local nepatvirtina API raktų
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
  apiKey: "foundry-local", // Foundry Local nepatikrina API raktų
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

> **Pagrindinė mintis:** Kadangi Foundry Local atveria OpenAI suderinamą API, bet kuris kodas, veikiantis su integruotais modeliais, taip pat veikia su jūsų pasirinktais modeliais. Reikia tik pakeisti `model` parametrą.

---

### Pratimai 10: Išbandykite pasirinktą modelį su Foundry Local SDK

Ankstesniuose užsiėmimuose naudojote Foundry Local SDK, kad paleistumėte paslaugą, surastumėte galinį tašką ir valdytumėte modelius automatiškai. Galite vadovautis ta pačia schema su savo sukompiliuotu modeliu. SDK valdys paslaugos paleidimą ir galinio taško atradimą, todėl jūsų kodui nereikia užkoduoti `localhost:5273`.

> **Pastaba:** Įsitikinkite, kad Foundry Local SDK yra įdiegtas prieš paleidžiant šiuos pavyzdžius:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Pridėkite `Microsoft.AI.Foundry.Local` ir `OpenAI` NuGet paketus
>
> Išsaugokite kiekvieną skripto failą **repozitorijos šakniniame kataloge** (tame pačiame kataloge kaip jūsų `models/` aplankas).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# 1 žingsnis: Paleiskite Foundry Local paslaugą ir įkelkite pasirinktą modelį
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# 2 žingsnis: Patikrinkite talpyklą dėl pasirinkto modelio
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# 3 žingsnis: Įkelkite modelį į atmintį
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# 4 žingsnis: Sukurkite OpenAI klientą naudodami SDK aptiktą galinį tašką
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# 5 žingsnis: Išsiųskite srautinį pokalbio užbaigimo užklausą
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

Paleiskite:

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

// 1 žingsnis: Paleiskite Foundry Local paslaugą
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 2 žingsnis: Gaukite individualų modelį iš katalogo
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// 3 žingsnis: Įkelkite modelį į atmintį
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// 4 žingsnis: Sukurkite OpenAI klientą naudodami SDK aptiktą galinį tašką
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// 5 žingsnis: Siųskite srautinių pokalbių užbaigimo užklausą
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

Paleiskite:

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

> **Pagrindinė mintis:** Foundry Local SDK dinamiškai suranda galinį tašką, todėl niekada nereikia užkoduoti prievado numerio. Tai rekomenduojamas būdas gamybos programėlėms. Jūsų pasirinktas modelis veikia lygiai taip pat kaip ir kataloge esantys modeliai per SDK.

---

## Pasirinkite modelį kompiliavimui

Qwen3-0.6B šiame laboratoriniame darbe naudojamas kaip pavyzdys, nes jis yra mažas, greitai suprantamas ir laisvai prieinamas pagal Apache 2.0 licenciją. Tačiau galite kompiliuoti ir kitus modelius. Štai keletas pasiūlymų:

| Modelis | Hugging Face ID | Parametrai | Licencija | Pastabos |
|---------|-----------------|------------|-----------|----------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Labai mažas, greitas kompiliavimas, tinkamas testavimui |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Geresnė kokybė, vis dar greitai kompiliuojama |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Aukšta kokybė, reikia daugiau RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Reikia licencijos patvirtinimo Hugging Face platformoje |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Aukšta kokybė, didesnis atsisiuntimas ir ilgiau trunkantis kompiliavimas |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Jau yra Foundry Local kataloge (naudinga palyginimui) |

> **Licencijos priminimas:** Visada prieš naudojimąsi patikrinkite modelio licenciją Hugging Face. Kai kurie modeliai (pvz., Llama) reikalauja priimti licencijos sutartį ir autentifikuotis su `huggingface-cli login` prieš atsisiunčiant.

---

## Sąvokos: Kada naudoti pasirinktinius modelius

| Scenarijus | Kodėl kompiliuoti savo? |
|------------|-------------------------|
| **Modelio, kurio reikia, nėra kataloge** | Foundry Local katalogas yra kuriruojamas. Jei modelio nėra sąraše, sukompiliuokite jį patys. |
| **Tiksliniai modeliai** | Jei atlikote modelio pritaikymą konkrečiam domenui, turite sukompiliuoti savo svorius. |
| **Specifiniai kvantizavimo reikalavimai** | Gali prireikti kitokios tikslumo ar kvantizavimo strategijos nei numatytoji. |
| **Naujesni modelių leidimai** | Kai naujas modelis pasirodo Hugging Face, jis gali dar nebūti Foundry Local kataloge. Sukompiliavus patiems, gaunate prieigą iš karto. |
| **Tyrimai ir eksperimentai** | Bandydami skirtingas modelių architektūras, dydžius ar konfigūracijas vietoje prieš galutinį pasirinkimą gamyboje. |

---

## Santrauka

Šiame laboratoriniame darbe išmokote:

| Žingsnis | Ką padarėte |
|----------|-------------|
| 1 | Įdiegėte ONNX Runtime GenAI modelio kūrėją |
| 2 | Sukompiliavote `Qwen/Qwen3-0.6B` iš Hugging Face į optimizuotą ONNX modelį |
| 3 | Sukūrėte `inference_model.json` pokalbio šablono konfigūraciją |
| 4 | Pridėjote sukompiliuotą modelį į Foundry Local kešą |
| 5 | Paleidote interaktyvų pokalbį su pasirinktiniu modeliu per CLI |
| 6 | Užklausėte modelį per OpenAI suderinamą REST API |
| 7 | Prisijungėte naudojant Python, JavaScript ir C# su OpenAI SDK |
| 8 | Išbandėte pasirinktą modelį visapusiškai su Foundry Local SDK |

Svarbiausia mintis yra tai, kad **bet kuris transformerių pagrindu sukurtas modelis gali veikti per Foundry Local**, kai jis yra sukompiliuotas į ONNX formatą. OpenAI suderinama API reiškia, kad visas jūsų esamas programų kodas veiks be pakeitimų; tereikia pakeisti modelio pavadinimą.

---

## Pagrindinės mintys

| Sąvoka | Detalė |
|--------|--------|
| ONNX Runtime GenAI modelio kūrėjas | Konvertuoja Hugging Face modelius į ONNX formatą su kvantizacija vienu komandos paleidimu |
| ONNX formatas | Foundry Local reikalauja ONNX modelių su ONNX Runtime GenAI konfigūracija |
| Pokalbių šablonai | `inference_model.json` failas nurodo Foundry Local, kaip formatuoti užklausas konkrečiam modeliui |
| Techninė įranga | Kompiliuokite CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) arba WebGPU pagal savo techninę įrangą |
| Kvantizacija | Mažesnis tikslumas (int4) sumažina dydį ir pagreitina veikimą, tačiau šiek tiek sumažina tikslumą; fp16 išlaiko aukštą kokybę GPU aplinkoje |
| API suderinamumas | Pasirinktiniai modeliai naudoja tą pačią OpenAI suderinamą API kaip ir integruoti modeliai |
| Foundry Local SDK | SDK automatiškai tvarko paslaugos paleidimą, galinio taško paiešką ir modelių krovimą tiek katalogui, tiek pasirinktiems modeliams |

---

## Tolimesnė literatūra

| Šaltinis | Nuoroda |
|----------|---------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local pasirinktinio modelio vadovas | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 modelių šeima | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive dokumentacija | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Tolimesni žingsniai

Tęskite prie [11 dalies: Įrankių kvietimas naudojant vietinius modelius](part11-tool-calling.md), kad sužinotumėte, kaip įgalinti vietinius modelius kviesti išorines funkcijas.

[← 9 dalis: Whisper balso transkripcija](part9-whisper-voice-transcription.md) | [11 dalis: Įrankių kvietimas →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės apribojimas**:
Šis dokumentas buvo išverstas naudojant AI vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors stengiamės užtikrinti tikslumą, prašome atkreipti dėmesį, kad automatiniai vertimai gali turėti klaidų ar netikslumų. Pradiniu šaltiniu laikomas originalus dokumentas gimtąja kalba. Kritinės informacijos atveju rekomenduojama pasitelkti profesionalų žmogaus vertimą. Mes neatsakome už bet kokius nesusipratimus ar klaidingus aiškinimus, kilusius dėl šio vertimo naudojimo.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->