![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Del 10: Uporaba lastnih ali Hugging Face modelov z Foundry Local

> **Cilj:** Sestaviti Hugging Face model v optimizirano obliko ONNX, ki jo zahteva Foundry Local, ga konfigurirati s predlogo za klepet, dodati v lokalni predpomnilnik in zagnati sklepanje z uporabo CLI, REST API in OpenAI SDK.

## Pregled

Foundry Local vsebuje skrbno izbran katalog predhodno sestavljenih modelov, vendar niste omejeni na ta seznam. Vsak jezikovni model, ki temelji na transformatorjih in je na voljo na [Hugging Face](https://huggingface.co/) (ali shranjen lokalno v PyTorch / Safetensors formatu), je mogoče sestaviti v optimiziran ONNX model in ga postreči preko Foundry Local.

Cevovod za sestavljanje uporablja **ONNX Runtime GenAI Model Builder**, orodje ukazne vrstice, ki je vključeno v paket `onnxruntime-genai`. Graditelj modelov opravi težja opravila: prenos izvornih uteži, pretvorbo v ONNX format, uporabo kvantizacije (int4, fp16, bf16) in izdelavo konfiguracijskih datotek (vključno s predlogo za klepet in tokenizatorjem), ki jih pričakuje Foundry Local.

V tej vadnici boste sestavili **Qwen/Qwen3-0.6B** s Hugging Face, ga registrirali v Foundry Local in se z njim pogovarjali povsem na vaši napravi.

---

## Cilji učenja

Do konca te vadnice boste znali:

- Pojasniti, zakaj je koristno sestavljanje lastnih modelov in kdaj ga potrebujete
- Namestiti ONNX Runtime GenAI model builder
- S sestavljanjem z enim ukazom pretvoriti Hugging Face model v optimiziran ONNX format
- Razumeti ključne parametre sestavljanja (exec provider, natančnost)
- Ustvariti `inference_model.json` datoteko konfiguracije predloge za klepet
- Dodati sestavljen model v predpomnilnik Foundry Local
- Zagnati sklepanje na lastnem modelu z uporabo CLI, REST API in OpenAI SDK

---

## Zahteve

| Zahteva | Podrobnosti |
|-------------|---------|
| **Foundry Local CLI** | Nameščen in dostopen v vaši `PATH` ([Del 1](part1-getting-started.md)) |
| **Python 3.10+** | Zahtevan za ONNX Runtime GenAI model builder |
| **pip** | Upravljalnik paketov za Python |
| **Diskovni prostor** | Vsaj 5 GB prostega za izvorne in sestavljene modelne datoteke |
| **Hugging Face račun** | Nekateri modeli zahtevajo sprejem licence pred prenosom. Qwen3-0.6B uporablja licenco Apache 2.0 in je brezplačno dostopen. |

---

## Nastavitev okolja

Sestavljanje modelov zahteva več velikih Python paketov (PyTorch, ONNX Runtime GenAI, Transformers). Ustvarite namensko virtualno okolje, da ne bodo motili vašega sistemskega Pythona ali drugih projektov.

```bash
# Iz korena repozitorija
python -m venv .venv
```

Aktivirajte okolje:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Nadgradite pip, da preprečite težave z odvisnostmi:

```bash
python -m pip install --upgrade pip
```

> **Namig:** Če imate `.venv` iz prejšnjih vaj, ga lahko uporabite ponovno. Samo poskrbite, da je aktiviran, preden nadaljujete.

---

## Koncept: cevovod za sestavljanje

Foundry Local zahteva modele v ONNX formatu z ONNX Runtime GenAI konfiguracijo. Večina odprtokodnih modelov na Hugging Face je distribuirana kot PyTorch ali Safetensors uteži, zato je potrebna pretvorba.

![Cevovod za sestavljanje lastnega modela](../../../images/custom-model-pipeline.svg)

### Kaj počne Model Builder?

1. **Prenese** izvorni model s Hugging Face (ali ga prebere s lokalne poti).
2. **Pretvori** PyTorch / Safetensors uteži v ONNX format.
3. **Kvantizira** model na nižjo natančnost (na primer int4) za manjšo porabo pomnilnika in boljšo prepustnost.
4. **Ustvari** ONNX Runtime GenAI konfiguracijo (`genai_config.json`), predlogo za klepet (`chat_template.jinja`) in vse datoteke tokenizatorja, da lahko Foundry Local naloži in postreže model.

### ONNX Runtime GenAI Model Builder v primerjavi z Microsoft Olive

Lahko naletite na sklice na **Microsoft Olive** kot alternativno orodje za optimizacijo modelov. Obe orodji lahko proizvedeta ONNX modele, vendar služita različnim namenom in imata različne kompromise:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Paket** | `onnxruntime-genai` | `olive-ai` |
| **Primarni namen** | Pretvorba in kvantizacija generativnih AI modelov za ONNX Runtime GenAI sklepanje | Okvir za optimizacijo modelov od začetka do konca, podpira več backendov in ciljev strojne opreme |
| **Enostavnost uporabe** | En ukaz — enostopenjska pretvorba + kvantizacija | Delovne poteze — konfigurabilni večkrožni cevovodi z YAML/JSON |
| **Izhodni format** | ONNX Runtime GenAI format (pripravljen za Foundry Local) | Splošni ONNX, ONNX Runtime GenAI ali drugi formati odvisno od delovne poti |
| **Cilji strojne opreme** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN in več |
| **Možnosti kvantizacije** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, poleg tega optimizacije grafov, prilagoditve plasti |
| **Obseg modelov** | Generativni AI modeli (LLM, SLM) | Katerekoli modele, pretvorljive v ONNX (vid, NLP, avdio, multimodalni) |
| **Najbolj primerno** | Hitro sestavljanje posameznih modelov za lokalno sklepanje | Produkcijski cevovodi, ki potrebujejo fino nastavitev optimizacije |
| **Poraba odvisnosti** | Zmerna (PyTorch, Transformers, ONNX Runtime) | Večja (vključuje Olive okvir, dodatke po potrebi) |
| **Integracija v Foundry Local** | Neposredno — izhod je takoj združljiv | Zahteva zastavico `--use_ort_genai` in dodatno konfiguracijo |

> **Zakaj ta laboratorij uporablja Model Builder:** Za nalogo sestavljanja posameznega Hugging Face modela in registracijo v Foundry Local je Model Builder najpreprostejša in najustreznejša pot. Proizvede točen izhodni format, ki ga Foundry Local pričakuje, z enim ukazom. Če boste pozneje potrebovali napredne funkcije optimizacije — kot so natančnostno zavedna kvantizacija, grafične spremembe ali večkrožno nastavljanje — je Olive zmogljiva alternativa. Podrobnosti najdete v [Microsoft Olive dokumentaciji](https://microsoft.github.io/Olive/).

---

## Laboratorijske vaje

### Vaja 1: Namestite ONNX Runtime GenAI Model Builder

Namestite paket ONNX Runtime GenAI, ki vključuje orodje za sestavljanje modelov:

```bash
pip install onnxruntime-genai
```

Preverite namestitev tako, da potrdite, da je model builder na voljo:

```bash
python -m onnxruntime_genai.models.builder --help
```

Videli bi morali izpis pomoči z vnosnimi parametri, kot so `-m` (ime modela), `-o` (izhodna pot), `-p` (natančnost) in `-e` (izvajalni ponudnik).

> **Opomba:** Model builder je odvisen od PyTorch, Transformers in več drugih paketov. Namestitev lahko traja nekaj minut.

---

### Vaja 2: Sestavite Qwen3-0.6B za CPU

Zaženite naslednji ukaz, da prenesete Qwen3-0.6B model s Hugging Face in ga sestavite za sklepanje na CPU z int4 kvantizacijo:

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

#### Kaj počne vsak parameter

| Parameter | Namen | Uporabljena vrednost |
|-----------|---------|------------|
| `-m` | ID modela na Hugging Face ali lokalna pot do direktorija | `Qwen/Qwen3-0.6B` |
| `-o` | Direktorij, kamor bo shranjen sestavljeni ONNX model | `models/qwen3` |
| `-p` | Natančnost kvantizacije, uporabljena med sestavljanjem | `int4` |
| `-e` | Izvajalni ponudnik za ONNX Runtime (ciljna strojna oprema) | `cpu` |
| `--extra_options hf_token=false` | Preskoči avtorizacijo Hugging Face (primerno za javne modele) | `hf_token=false` |

> **Koliko časa traja?** Čas sestavljanja je odvisen od vaše strojne opreme in velikosti modela. Za Qwen3-0.6B s kvantizacijo int4 na sodobnem CPU pričakujte približno 5 do 15 minut. Večji modeli vzamejo sorazmerno več časa.

Po končanem ukazu boste v direktoriju `models/qwen3` našli sestavljene datoteke modela. Preverite izhod:

```bash
ls models/qwen3
```

Videti bi morali datoteke, med drugim:
- `model.onnx` in `model.onnx.data` — sestavljene uteži modela
- `genai_config.json` — konfiguracija ONNX Runtime GenAI
- `chat_template.jinja` — predloga klepeta modela (samodejno ustvarjena)
- `tokenizer.json`, `tokenizer_config.json` — datoteke tokenizatorja
- Druge besediščne in konfiguracijske datoteke

---

### Vaja 3: Sestavljanje za GPU (neobvezno)

Če imate NVIDIA GPU s podporo za CUDA, lahko sestavite GPU-optimizirano različico za hitrejše sklepanje:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Opomba:** Sestavljanje za GPU zahteva `onnxruntime-gpu` in delujočo namestitev CUDA. Če teh komponent ni, bo graditelj modelov prijavil napako. To vajo lahko preskočite in nadaljujete z CPU različico.

#### Referenca za strojno-specifično sestavljanje

| Cilj | Izvajalni ponudnik (`-e`) | Priporočena natančnost (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` ali `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` ali `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Kompromisi natančnosti

| Natančnost | Velikost | Hitrost | Kakovost |
|-----------|------|-------|---------|
| `fp32` | Največja | Najpočasnejša | Najvišja natančnost |
| `fp16` | Velika | Hitro (GPU) | Zelo dobra natančnost |
| `int8` | Majhna | Hitro | Neznatna izguba natančnosti |
| `int4` | Najmanjša | Najhitrejša | Zmerna izguba natančnosti |

Za večino lokalnega razvoja `int4` na CPU ponuja najboljše ravnovesje med hitrostjo in porabo virov. Za produkcijsko kakovost je priporočljiv `fp16` na CUDA GPU.

---

### Vaja 4: Ustvarite konfiguracijo predloge za klepet

Model builder samodejno ustvari datoteko `chat_template.jinja` in `genai_config.json` v izhodnem direktoriju. Vendar pa Foundry Local potrebuje tudi datoteko `inference_model.json`, da razume, kako oblikovati pozive za vaš model. Ta datoteka določa ime modela in predlogo poziva, ki ovije uporabnikova sporočila v pravilne posebne oznake.

#### 1. korak: Preverite sestavljeni izhod

Izpišite vsebino direktorija z modelom:

```bash
ls models/qwen3
```

Videti bi morali datoteke, kot so:
- `model.onnx` in `model.onnx.data` — sestavljene uteži modela
- `genai_config.json` — konfiguracija ONNX Runtime GenAI (samodejno ustvarjena)
- `chat_template.jinja` — predloga klepeta modela (samodejno ustvarjena)
- `tokenizer.json`, `tokenizer_config.json` — datoteke tokenizatorja
- Različne druge konfiguracijske in besediščne datoteke

#### 2. korak: Ustvarite datoteko inference_model.json

Datoteka `inference_model.json` sporoča Foundry Local, kako oblikovati pozive. Ustvarite Python skripto z imenom `generate_chat_template.py` **v korenskem imeniku repozitorija** (isti imenik, kjer je vaša mapa `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Zgradi minimalen pogovor za pridobitev predloge klepeta
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

# Zgradi strukturo inference_model.json
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

Zaženite skripto iz korenskega imenika repozitorija:

```bash
python generate_chat_template.py
```

> **Opomba:** Paket `transformers` je bil že nameščen kot odvisnost `onnxruntime-genai`. Če prejmete `ImportError`, najprej zaženite `pip install transformers`.

Skripta ustvari datoteko `inference_model.json` v mapi `models/qwen3`. Datoteka Foundry Local-u sporoči, kako vključiti uporabniški vhod v pravilne posebne oznake za Qwen3.

> **Pomembno:** Polje `"Name"` v `inference_model.json` (nastavljeno na `qwen3-0.6b` v tej skripti) je **psevdonim modela**, ki ga boste uporabljali v vseh nadaljnjih ukazih in API klicih. Če spremenite to ime, posodobite ime modela v vajah od 6 do 10 skladno s tem.

#### 3. korak: Preverite konfiguracijo

Odprite `models/qwen3/inference_model.json` in potrdite, da vsebuje polje `Name` in objekt `PromptTemplate` s ključema `assistant` in `prompt`. Predloga poziva mora vsebovati posebne oznake, kot sta `<|im_start|>` in `<|im_end|>` (točne oznake so odvisne od predloge klepeta modela).

> **Ročna alternativa:** Če ne želite zagnati skripte, lahko datoteko ustvarite ročno. Ključni pogoj je, da `prompt` polje vsebuje celotno predlogo klepeta modela z `{Content}` kot zamenjavo za uporabnikovo sporočilo.

---

### Vaja 5: Preverite strukturo imenika modela
Graditelj modelov postavi vse prevedene datoteke neposredno v izhodni imenik, ki ste ga določili. Preverite, ali je končna struktura videti pravilno:

```bash
ls models/qwen3
```

V imeniku naj bodo naslednje datoteke:

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

> **Opomba:** Za razliko od nekaterih drugih orodij za prevajanje graditelj modelov ne ustvarja gnezdenih podimenikov. Vse datoteke so neposredno v izhodni mapi, kar je tudi točno tisto, kar pričakuje Foundry Local.

---

### Vaja 6: Dodajte model v predpomnilnik Foundry Local

Povejte Foundry Local, kje najde vaš prevedeni model, tako da direktorij dodate v njegov predpomnilnik:

```bash
foundry cache cd models/qwen3
```

Preverite, ali se model pojavi v predpomnilniku:

```bash
foundry cache ls
```

Vaš prilagojeni model bi moral biti naveden skupaj z drugimi že predhodno predpomnjenimi modeli (kot so `phi-3.5-mini` ali `phi-4-mini`).

---

### Vaja 7: Zaženite prilagojeni model prek CLI

Začnite interaktivno klepetalno sejo z vašim pravkar prevedenim modelom (alias `qwen3-0.6b` izhaja iz polja `Name`, ki ste ga nastavili v `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Zastavica `--verbose` prikazuje dodatne diagnostične informacije, kar je koristno pri prvem testiranju prilagojenega modela. Če se model uspešno naloži, boste videli interaktivni poziv. Poskusite nekaj sporočil:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Za konec seje vnesite `exit` ali pritisnite `Ctrl+C`.

> **Reševanje težav:** Če se model ne naloži, preverite naslednje:
> - Datoteka `genai_config.json` je bila ustvarjena z graditeljem modelov.
> - Datoteka `inference_model.json` obstaja in je veljaven JSON.
> - ONNX modelne datoteke so v pravilnem imeniku.
> - Imate dovolj razpoložljivega RAM-a (Qwen3-0.6B int4 potrebuje približno 1 GB).
> - Qwen3 je model za sklepanja, ki generira oznake `<think>`. Če vidite `<think>...</think>` pred odzivi, je to normalno vedenje. Predloga poziva v `inference_model.json` se lahko prilagodi za potlačitev izpisa razmišljanja.

---

### Vaja 8: Poizvedovanje po prilagojenem modelu prek REST API-ja

Če ste v vaji 7 izstopili iz interaktivne seje, verjetno model ni več naložen. Najprej zaženite storitev Foundry Local in naložite model:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Preverite, na katerem vratih storitev teče:

```bash
foundry service status
```

Nato pošljite zahtevo (zamenjajte `5273` z dejansko številko vrat, če se razlikuje):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Opomba za Windows:** Ukaz `curl` zgoraj uporablja bash sintakso. Na Windows uporabite PowerShell ukaz `Invoke-RestMethod`, kot je spodaj.

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

### Vaja 9: Uporaba prilagojenega modela z OpenAI SDK

Vaš prilagojeni model lahko povežete z isto kodo OpenAI SDK, kot ste jo uporabljali za vgrajene modele (glejte [Del 3](part3-sdk-and-apis.md)). Edina razlika je ime modela.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local ne preverja API ključev
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
  apiKey: "foundry-local", // Foundry Local ne preverja API ključev
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

> **Ključna točka:** Ker Foundry Local omogoča OpenAI-združljiv API, katerakoli koda, ki deluje z vgrajenimi modeli, deluje tudi z vašimi prilagojenimi modeli. Potrebno je samo spremeniti parameter `model`.

---

### Vaja 10: Testirajte prilagojeni model z Foundry Local SDK

V prejšnjih laboratorijih ste uporabili Foundry Local SDK za zagon storitve, odkrivanje končne točke in upravljanje modelov samodejno. Enako shemo lahko uporabite s svojim ročno prevedenim modelom. SDK poskrbi za zagon storitve in odkrivanje končne točke, zato vaša koda nima potrebe po trdo kodiranju `localhost:5273`.

> **Opomba:** Preden zaženete te primere, poskrbite, da imate nameščen Foundry Local SDK:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Dodajte NuGet paketa `Microsoft.AI.Foundry.Local` in `OpenAI`
>
> Vsak skript shranite **v korenski imenik repozitorija** (isti imenik kot vaša mapa `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# 1. korak: Zaženite storitev Foundry Local in naložite prilagojeni model
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# 2. korak: Preverite predpomnilnik za prilagojeni model
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# 3. korak: Naložite model v pomnilnik
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# 4. korak: Ustvarite OpenAI odjemalca z uporabo prek SDK odkritega končnega točke
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# 5. korak: Pošljite zahtevo za dokončanje klepeta v pretoku
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

Zaženite ga:

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

// Korak 1: Zaženite storitev Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Korak 2: Pridobite po meri model iz kataloga
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Korak 3: Naložite model v pomnilnik
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Korak 4: Ustvarite OpenAI odjemalca z uporabo SDK-odkritega končnega mesta
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Korak 5: Pošljite zahtevo za pretakanje klepetalne dokončave
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

Zaženite ga:

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

> **Ključna točka:** Foundry Local SDK dinamično odkrije končno točko, zato nikoli ne hardkodirate številke vrat. To je priporočeni pristop za proizvodne aplikacije. Vaš ročno prevedeni model deluje enako kot vgrajeni katalogni modeli prek SDK.

---

## Izbira modela za prevajanje

Qwen3-0.6B je v tem laboratoriju uporabljen kot referenčni primer, ker je majhen, hitro se prevaja in je na voljo pod licenco Apache 2.0. Vendar lahko prevedete še mnogo drugih modelov. Tukaj je nekaj predlogov:

| Model | ID na Hugging Face | Število parametrov | Licenca | Opombe |
|-------|--------------------|--------------------|---------|--------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Zelo majhen, hitra prevajalnost, dober za testiranje |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Boljša kakovost, še vedno hitro se prevaja |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Močna kakovost, potrebuje več RAM-a |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Zahteva sprejem licence na Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Visoka kakovost, večji prenos in daljši prevod |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Že v katalogu Foundry Local (koristno za primerjavo) |

> **Opomnik glede licence:** Vedno preverite licenco modela na Hugging Face pred uporabo. Nekateri modeli (kot Llama) zahtevajo, da sprejmete licenčni sporazum in se overite z `huggingface-cli login` pred prenosom.

---

## Koncepti: Kdaj uporabljati prilagojene modele

| Scenarij | Zakaj prevajati svoj model? |
|----------|-----------------------------|
| **Model, ki ga potrebujete, ni v katalogu** | Katalog Foundry Local je urejen. Če model, ki ga želite, ni v seznamu, ga prevedite sami. |
| **Fino prilagojeni modeli** | Če ste model fino prilagodili na podatkih specifičnih za domeno, morate prevesti svoje uteži. |
| **Specifične zahteve glede kvantizacije** | Morda želite natančnost ali strategijo kvantizacije, ki se razlikuje od privzete v katalogu. |
| **Novejše izdaje modelov** | Ko je nov model objavljen na Hugging Face, morda še ni v katalogu Foundry Local. Prevajanje vam takoj omogoči dostop. |
| **Raziskave in eksperimentiranje** | Preizkušanje različnih arhitektur modelov, velikosti ali konfiguracij lokalno pred izbiro za produkcijo. |

---

## Povzetek

V tem laboratoriju ste se naučili, kako:

| Korak | Kaj ste naredili |
|-------|------------------|
| 1 | Namestili ONNX Runtime GenAI graditelj modelov |
| 2 | Prevedli `Qwen/Qwen3-0.6B` s Hugging Face v optimiziran ONNX model |
| 3 | Ustvarili konfiguracijsko datoteko čat-predloge `inference_model.json` |
| 4 | Dodali prevedeni model v predpomnilnik Foundry Local |
| 5 | Zaželi interaktivni klepet s prilagojenim modelom prek ukazne vrstice |
| 6 | Poizvedovali model prek OpenAI združljivega REST API-ja |
| 7 | Se povezali iz Pythona, JavaScripta in C# z OpenAI SDK |
| 8 | Testirali prilagojeni model s Foundry Local SDK end-to-end |

Ključni zaključek je, da **lahko vsak model z arhitekturo transformerja teče preko Foundry Local**, ko je preveden v ONNX format. OpenAI združljiv API pomeni, da vaša obstoječa aplikacijska koda deluje brez sprememb; potrebna je samo menjava imena modela.

---

## Ključne ugotovitve

| Koncept | Podrobnosti |
|---------|-------------|
| ONNX Runtime GenAI graditelj modelov | Pretvori modele Hugging Face v ONNX format s kvantizacijo z enim ukazom |
| ONNX format | Foundry Local zahteva ONNX modele z ONNX Runtime GenAI konfiguracijo |
| Čat predloge | Datoteka `inference_model.json` pove Foundry Local, kako oblikovati pozive za dani model |
| Ciljne naprave | Prevajajte za CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) ali WebGPU glede na vašo strojno opremo |
| Kvantizacija | Nižja natančnost (int4) zmanjša velikost in poveča hitrost na račun nekaj natančnosti; fp16 ohranja visoko kakovost na GPU-jih |
| Združljivost API | Prilagojeni modeli uporabljajo isti OpenAI združljiv API kot vgrajeni modeli |
| Foundry Local SDK | SDK samodejno upravlja z zagonom storitve, odkrivanjem končne točke in nalaganjem modelov tako za katalog kot za prilagojene modele |

---

## Dodatno branje

| Vir | Povezava |
|------|----------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local vodič za prilagojene modele | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Družina modelov Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Dokumentacija Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Naslednji koraki

Nadaljujte na [Del 11: Klicanje orodij s lokalnimi modeli](part11-tool-calling.md) in se naučite, kako omogočiti, da vaši lokalni modeli kličejo zunanje funkcije.

[← Del 9: Prepis glasu Whisper](part9-whisper-voice-transcription.md) | [Del 11: Klicanje orodij →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Omejitev odgovornosti**:  
Ta dokument je bil preveden z uporabo AI prevajalske storitve [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za natančnost, vas prosimo, da upoštevate, da avtomatizirani prevodi lahko vsebujejo napake ali netočnosti. Izvirni dokument v materinem jeziku naj se šteje za avtoritativni vir. Za ključne informacije priporočamo strokoven človeški prevod. Ne odgovarjamo za morebitna nesporazume ali napačne interpretacije, ki izhajajo iz uporabe tega prevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->