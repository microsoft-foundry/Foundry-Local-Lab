![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Dio 10: Korištenje prilagođenih ili Hugging Face modela s Foundry Local

> **Cilj:** Prevesti Hugging Face model u optimizirani ONNX format koji Foundry Local zahtijeva, konfigurirati ga s chat predloškom, dodati ga u lokalni cache i pokrenuti inferenciju nad njim koristeći CLI, REST API i OpenAI SDK.

## Pregled

Foundry Local dolazi s kuriranim katalogom prethodno prevedenih modela, ali niste ograničeni na taj popis. Bilo koji transformer-bazirani jezični model dostupan na [Hugging Face](https://huggingface.co/) (ili pohranjen lokalno u PyTorch / Safetensors formatu) može se prevesti u optimizirani ONNX model i poslužiti putem Foundry Local.

Pipeline za prevođenje koristi **ONNX Runtime GenAI Model Builder**, alat na komandnoj liniji uključen u `onnxruntime-genai` paket. Model builder obavlja teška posla: preuzima izvornu težinu, pretvara ih u ONNX format, primjenjuje kvantizaciju (int4, fp16, bf16) i emitira konfiguracijske datoteke (uključujući chat predložak i tokenizator) koje Foundry Local očekuje.

U ovoj radionici ćete prevesti **Qwen/Qwen3-0.6B** s Hugging Face, registrirati ga u Foundry Local i razgovarati s njim potpuno na svom uređaju.

---

## Ciljevi učenja

Do kraja ove radionice moći ćete:

- Objasniti zašto je korisno prilagođeno prevođenje modela i kada vam može zatrebati
- Instalirati ONNX Runtime GenAI model builder
- Prevesti Hugging Face model u optimizirani ONNX format jednim naredbom
- Razumjeti ključne parametre prevođenja (execution provider, preciznost)
- Kreirati `inference_model.json` konfiguracijsku datoteku chat predloška
- Dodati prevedeni model u Foundry Local cache
- Pokrenuti inferenciju nad prilagođenim modelom koristeći CLI, REST API i OpenAI SDK

---

## Preduvjeti

| Zahtjev | Detalji |
|-------------|---------|
| **Foundry Local CLI** | Instaliran i u vašem `PATH` ([Dio 1](part1-getting-started.md)) |
| **Python 3.10+** | Potrebno za ONNX Runtime GenAI model builder |
| **pip** | Python paketni upravitelj |
| **Diskovni prostor** | Najmanje 5 GB slobodno za izvornu i prevedenu datoteku modela |
| **Hugging Face račun** | Neki modeli zahtijevaju prihvaćanje licence prije preuzimanja. Qwen3-0.6B koristi Apache 2.0 licencu i slobodno je dostupan. |

---

## Postavljanje okruženja

Prevođenje modela zahtijeva nekoliko velikih Python paketa (PyTorch, ONNX Runtime GenAI, Transformers). Kreirajte posebno virtualno okruženje kako ti paketi ne bi ometali vaš sustavni Python ili druge projekte.

```bash
# Iz korijena spremišta
python -m venv .venv
```

Aktivirajte okruženje:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Nadogradite pip da izbjegnete probleme s ovisnostima:

```bash
python -m pip install --upgrade pip
```

> **Savjet:** Ako već imate `.venv` iz ranijih radionica, možete ga ponovo koristiti. Samo pazite da je aktiviran prije nastavka.

---

## Koncept: Pipeline prevođenja

Foundry Local zahtijeva modele u ONNX formatu s ONNX Runtime GenAI konfiguracijom. Većina open-source modela na Hugging Face distribuira se kao PyTorch ili Safetensors weighti, pa je potreban korak konverzije.

![Pipeline prilagođenog prevođenja modela](../../../images/custom-model-pipeline.svg)

### Što radi Model Builder?

1. **Preuzima** izvorni model s Hugging Face (ili ga učitava s lokalne putanje).
2. **Pretvara** PyTorch / Safetensors težine u ONNX format.
3. **Kvantizira** model na manju preciznost (na primjer, int4) radi smanjenja upotrebe memorije i povećanja propusnosti.
4. **Emitira** ONNX Runtime GenAI konfiguraciju (`genai_config.json`), chat predložak (`chat_template.jinja`) i sve datoteke tokenizatora da bi Foundry Local mogao učitati i poslužiti model.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Možda ćete naići na reference na **Microsoft Olive** kao alternativni alat za optimizaciju modela. Oba alata mogu proizvoditi ONNX modele, ali služe različitim svrhama i imaju različite prednosti:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Paket** | `onnxruntime-genai` | `olive-ai` |
| **Glavna svrha** | Prevođenje i kvantizacija generativnih AI modela za ONNX Runtime GenAI inferenciju | Framework za optimizaciju modela end-to-end s podrškom za mnoge backende i hardverske ciljeve |
| **Jednostavnost korištenja** | Jedan naredbeni redak — konverzija + kvantizacija u jednom koraku | Temeljen na workflowu — konfigurabilni višepass pipeline-ovi s YAML/JSON |
| **Izlazni format** | ONNX Runtime GenAI format (spreman za Foundry Local) | Generički ONNX, ONNX Runtime GenAI ili drugi formati ovisno o workflowu |
| **Ciljani hardver** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN i još |
| **Opcije kvantizacije** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus optimizacije grafa, prilagodba po slojevima |
| **Opseg modela** | Generativni AI modeli (LLM, SLM) | Bilo koji ONNX-konvertibilan model (vizija, NLP, audio, multimodalno) |
| **Najbolje za** | Brzo prevođenje jednog modela za lokalnu inferenciju | Produkcijske pipelineove kojima treba detaljna kontrola optimizacije |
| **Ovisnosti** | Umjerene (PyTorch, Transformers, ONNX Runtime) | Veće (dodaje Olive framework, dodatne opcije po workflowu) |
| **Integracija u Foundry Local** | Direktna — izlaz odmah kompatibilan | Potrebna zastavica `--use_ort_genai` i dodatne konfiguracije |

> **Zašto ova radionica koristi Model Builder:** Za zadatak prevođenja jednog Hugging Face modela i registracije u Foundry Local, Model Builder je najjednostavniji i najsigurniji put. Proizvodi točno izlazni format koji Foundry Local očekuje u jednoj naredbi. Ako kasnije trebate napredne značajke optimizacije — poput kvantizacije svijesne točnosti, preinaka grafa ili višepass prilagodbe — Olive je moćna opcija za istraživanje. Pogledajte [Microsoft Olive dokumentaciju](https://microsoft.github.io/Olive/) za više detalja.

---

## Vježbe u radionici

### Vježba 1: Instalirajte ONNX Runtime GenAI Model Builder

Instalirajte ONNX Runtime GenAI paket koji uključuje alat za prevođenje modela:

```bash
pip install onnxruntime-genai
```

Potvrdite instalaciju provjerom dostupnosti model buildera:

```bash
python -m onnxruntime_genai.models.builder --help
```

Trebali biste vidjeti help izlaz s parametrima poput `-m` (ime modela), `-o` (putanju izlaza), `-p` (preciznost) i `-e` (execution provider).

> **Napomena:** Model builder ovisi o PyTorchu, Transformersima i nekoliko drugih paketa. Instalacija može potrajati nekoliko minuta.

---

### Vježba 2: Prevedite Qwen3-0.6B za CPU

Pokrenite sljedeću naredbu za preuzimanje Qwen3-0.6B modela s Hugging Face i prevođenje za CPU inferenciju s int4 kvantizacijom:

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

#### Što svaki parametar radi

| Parametar | Namjena | Vrijednost |
|-----------|---------|------------|
| `-m` | ID modela na Hugging Face ili lokalni direktorij | `Qwen/Qwen3-0.6B` |
| `-o` | Direktorij gdje će biti spremljeni prevedeni ONNX modeli | `models/qwen3` |
| `-p` | Preciznost kvantizacije tijekom prevođenja | `int4` |
| `-e` | ONNX Runtime execution provider (ciljani hardver) | `cpu` |
| `--extra_options hf_token=false` | Preskače Hugging Face autentikaciju (dovoljno za javne modele) | `hf_token=false` |

> **Koliko ovo traje?** Vrijeme prevođenja ovisi o vašem hardveru i veličini modela. Za Qwen3-0.6B s int4 kvantizacijom na modernom CPU-u očekujte otprilike 5 do 15 minuta. Veći modeli zahtijevaju proporcionalno više vremena.

Kada naredba završi, trebali biste vidjeti direktorij `models/qwen3` s prevedenim model datotekama. Provjerite sadržaj:

```bash
ls models/qwen3
```

Trebali biste vidjeti datoteke uključujući:
- `model.onnx` i `model.onnx.data` — prevedena težina modela
- `genai_config.json` — ONNX Runtime GenAI konfiguracija
- `chat_template.jinja` — chat predložak modela (auto-generirano)
- `tokenizer.json`, `tokenizer_config.json` — datoteke tokenizatora
- Ostale vokabularne i konfiguracijske datoteke

---

### Vježba 3: Prevedite za GPU (neobavezno)

Ako imate NVIDIA GPU s CUDA podrškom, možete prevesti GPU-optimiziranu varijantu za bržu inferenciju:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Napomena:** GPU prevođenje zahtijeva `onnxruntime-gpu` i instaliranu ispravnu CUDA verziju. Ako to nije dostupno, model builder će javiti grešku. Možete preskočiti ovu vježbu i nastaviti s CPU varijantom.

#### Referenca za hardverski specifično prevođenje

| Cilj | Execution Provider (`-e`) | Preporučena preciznost (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` ili `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` ili `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Kompromisi u preciznosti

| Preciznost | Veličina | Brzina | Kvaliteta |
|-----------|------|-------|---------|
| `fp32` | Najveća | Najsporija | Najviša točnost |
| `fp16` | Velika | Brza (GPU) | Vrlo dobra točnost |
| `int8` | Mala | Brza | Blagi gubitak točnosti |
| `int4` | Najmanja | Najbrža | Umjereni gubitak točnosti |

Za većinu lokalnog razvoja, `int4` na CPU-u pruža najbolju ravnotežu brzine i korištenja resursa. Za produkcijsku kvalitetu preporučuje se `fp16` na CUDA GPU-u.

---

### Vježba 4: Kreirajte konfiguraciju chat predloška

Model builder automatski generira `chat_template.jinja` datoteku i `genai_config.json` u izlaznom direktoriju. Međutim, Foundry Local također treba `inference_model.json` datoteku da zna kako formatirati upite za vaš model. Ta datoteka definira naziv modela i predložak prompta koji zaokružuje korisničke poruke odgovarajućim posebnim tokenima.

#### Korak 1: Pregledajte prevedeni izlaz

Ispišite sadržaj direktorija prevedenog modela:

```bash
ls models/qwen3
```

Trebali biste vidjeti datoteke poput:
- `model.onnx` i `model.onnx.data` — prevedene težine modela
- `genai_config.json` — ONNX Runtime GenAI konfiguracija (auto-generirano)
- `chat_template.jinja` — chat predložak modela (auto-generirano)
- `tokenizer.json`, `tokenizer_config.json` — datoteke tokenizatora
- Razne druge konfiguracijske i vokabularne datoteke

#### Korak 2: Generirajte datoteku inference_model.json

`inference_model.json` datoteka govori Foundry Local kako oblikovati promptove. Kreirajte Python skriptu nazvanu `generate_chat_template.py` **u korijenu repozitorija** (u istom direktoriju gdje se nalazi mapa `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Izgradite minimalan razgovor za izdvajanje predloška za chat
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

# Izgradite strukturu inference_model.json
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

Pokrenite skriptu iz korijena repozitorija:

```bash
python generate_chat_template.py
```

> **Napomena:** Paket `transformers` već je instaliran kao ovisnost `onnxruntime-genai`. Ako vidite `ImportError`, prvo pokrenite `pip install transformers`.

Skripta će proizvesti `inference_model.json` datoteku unutar `models/qwen3` direktorija. Ta datoteka kaže Foundry Localu kako zaokružiti korisnički unos odgovarajućim posebnim tokenima za Qwen3.

> **Važno:** Polje `"Name"` u `inference_model.json` (postavljeno na `qwen3-0.6b` u ovoj skripti) je **alias modela** koji ćete koristiti u svim narednim naredbama i API pozivima. Ako promijenite ovo ime, ažurirajte ga i u Vježbama 6–10 u skladu s tim.

#### Korak 3: Provjerite konfiguraciju

Otvorite `models/qwen3/inference_model.json` i provjerite da sadrži polje `Name` i objekt `PromptTemplate` s ključevima `assistant` i `prompt`. Predložak prompta treba sadržavati posebne tokene poput `<|im_start|>` i `<|im_end|>` (točni tokeni ovise o chat predlošku modela).

> **Ručno alternativno:** Ako ne želite pokretati skriptu, datoteku možete i ručno kreirati. Ključni zahtjev je da `prompt` polje sadrži puni chat predložak modela s `{Content}` kao zamjenskim mjestom za korisničku poruku.

---

### Vježba 5: Provjerite strukturu direktorija modela
Model builder stavlja sve prevedene datoteke izravno u direktorij za izlaz koji ste naveli. Provjerite izgleda li konačna struktura ispravno:

```bash
ls models/qwen3
```

Direktorij bi trebao sadržavati sljedeće datoteke:

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

> **Napomena:** Za razliku od nekih drugih alata za kompilaciju, model builder ne stvara ugniježđene poddirektorije. Sve datoteke nalaze se izravno u izlaznoj mapi, što je točno ono što Foundry Local očekuje.

---

### Vježba 6: Dodajte model u Foundry Local cache

Recite Foundry Local gdje može pronaći vaš prevedeni model dodavanjem direktorija u njegov cache:

```bash
foundry cache cd models/qwen3
```

Provjerite pojavljuje li se model u cacheu:

```bash
foundry cache ls
```

Trebali biste vidjeti vaš prilagođeni model naveden uz prethodno keširane modele (kao što su `phi-3.5-mini` ili `phi-4-mini`).

---

### Vježba 7: Pokrenite prilagođeni model putem CLI-ja

Pokrenite interaktivnu chat sesiju s vašim novoprevedenim modelom (alias `qwen3-0.6b` dolazi iz polja `Name` koje ste postavili u `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Zastavica `--verbose` prikazuje dodatne dijagnostičke informacije, što je korisno prilikom prvog testiranja prilagođenog modela. Ako se model uspješno učita, vidjet ćete interaktivni prompt. Isprobajte nekoliko poruka:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Upišite `exit` ili pritisnite `Ctrl+C` za završetak sesije.

> **Rješavanje problema:** Ako se model ne učitava, provjerite sljedeće:
> - Da je datoteka `genai_config.json` generirana od strane model buildera.
> - Da datoteka `inference_model.json` postoji i da je valjani JSON.
> - Da se ONNX model datoteke nalaze u ispravnom direktoriju.
> - Da imate dovoljno dostupne RAM memorije (Qwen3-0.6B int4 treba otprilike 1 GB).
> - Qwen3 je model za rezoniranje koji proizvodi `<think>` oznake. Ako vidite `<think>...</think>` na početku odgovora, to je normalno ponašanje. Predložak prompta u `inference_model.json` može se podesiti za suzbijanje izlaza razmišljanja.

---

### Vježba 8: Postavite upit prilagođenom modelu preko REST API-ja

Ako ste izašli iz interaktivne sesije u Vježbi 7, model možda više nije učitan. Prvo pokrenite Foundry Local servis i učitajte model:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Provjerite na kojem je portu servis pokrenut:

```bash
foundry service status
```

Zatim pošaljite zahtjev (zamijenite `5273` stvarnim portom ako je različit):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Napomena za Windows:** `curl` naredba gore koristi bash sintaksu. Na Windowsu koristite PowerShell `Invoke-RestMethod` cmdlet ispod.

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

### Vježba 9: Koristite prilagođeni model s OpenAI SDK-jem

Možete se povezati na vaš prilagođeni model koristeći točno isti OpenAI SDK kod koji ste koristili za ugrađene modele (vidi [Dio 3](part3-sdk-and-apis.md)). Jedina razlika je naziv modela.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local ne provjerava API ključeve
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
  apiKey: "foundry-local", // Foundry Local ne provjerava API ključeve
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

> **Ključna točka:** Budući da Foundry Local izlaže API kompatibilan s OpenAI-jem, svaki kod koji radi s ugrađenim modelima također radi i s vašim prilagođenim modelima. Potrebno je samo promijeniti parametar `model`.

---

### Vježba 10: Testirajte prilagođeni model s Foundry Local SDK-jem

U ranijim laboratorijskim vježbama koristili ste Foundry Local SDK za pokretanje servisa, otkrivanje endpointa i automatsko upravljanje modelima. Možete slijediti točno isti obrazac s vašim prilagođeno prevedenim modelom. SDK upravlja pokretanjem servisa i otkrivanjem endpointa, tako da vaš kod ne mora vraćati `localhost:5273` hardkodirano.

> **Napomena:** Provjerite da je Foundry Local SDK instaliran prije pokretanja ovih primjera:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Dodajte NuGet pakete `Microsoft.AI.Foundry.Local` i `OpenAI`
>
> Spremite svaku skriptu **u korijenski direktorij repozitorija** (isti direktorij kao i vaša mapa `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Korak 1: Pokrenite Foundry Local servis i učitajte prilagođeni model
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Korak 2: Provjerite predmemoriju za prilagođeni model
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Korak 3: Učitajte model u memoriju
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Korak 4: Kreirajte OpenAI klijenta koristeći SDK otkriveni endpoint
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Korak 5: Pošaljite zahtjev za streaming chat dovršenjem
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

Pokrenite ga:

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

// Korak 1: Pokrenite lokalnu uslugu Foundry
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Korak 2: Dohvatite prilagođeni model iz kataloga
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Korak 3: Učitajte model u memoriju
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Korak 4: Kreirajte OpenAI klijenta korištenjem SDK-om otkrivenog krajnjeg mjesta
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Korak 5: Pošaljite zahtjev za streaming dovršetak chata
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

Pokrenite ga:

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

> **Ključna točka:** Foundry Local SDK dinamički otkriva endpoint, stoga nikada ne hardkodirate broj porta. Ovo je preporučeni pristup za produkcijske aplikacije. Vaš prilagođeni prevedeni model radi identično kao modeli u ugrađenom katalogu preko SDK-ja.

---

## Odabir modela za kompilaciju

Qwen3-0.6B koristi se kao referentni primjer u ovom laboratoriju jer je mali, brz za kompilaciju i slobodno dostupan pod Apache 2.0 licencom. Međutim, možete kompilirati mnogo drugih modela. Evo nekoliko prijedloga:

| Model | Hugging Face ID | Parametri | Licenca | Napomene |
|-------|-----------------|------------|---------|----------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Vrlo mali, brza kompilacija, dobar za testiranje |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Bolja kvaliteta, još uvijek brza kompilacija |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Visoka kvaliteta, zahtijeva više RAM-a |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Zahtijeva prihvaćanje licence na Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Visoka kvaliteta, veće preuzimanje i dulja kompilacija |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Već u Foundry Local katalogu (korisno za usporedbu) |

> **Podsjetnik o licenci:** Uvijek provjerite licencu modela na Hugging Face prije korištenja. Neki modeli (poput Llamasa) zahtijevaju prihvaćanje ugovora o licenci i autentifikaciju putem `huggingface-cli login` prije preuzimanja.

---

## Koncepti: Kada koristiti prilagođene modele

| Scenarij | Zašto kompilirati vlastiti? |
|----------|-----------------------------|
| **Model koji vam treba nije u katalogu** | Foundry Local katalog je kuriran. Ako model koji želite nije naveden, kompilirajte ga sami. |
| **Fino podešeni modeli** | Ako ste model fino podešavali na domen-specificiranim podacima, trebate kompilirati vlastite težine. |
| **Specifični zahtjevi kvantizacije** | Možda želite preciznost ili strategiju kvantizacije koja se razlikuje od zadane u katalogu. |
| **Noviji modeli** | Kada je novi model objavljen na Hugging Face, možda još nije u Foundry Local katalogu. Kompilacijom dobivate trenutačni pristup. |
| **Istraživanje i eksperimentiranje** | Isprobavanje različitih arhitektura modela, veličina ili konfiguracija lokalno prije donošenja produkcijske odluke. |

---

## Sažetak

U ovom laboratoriju naučili ste kako:

| Korak | Što ste napravili |
|-------|-------------------|
| 1 | Instalirali ONNX Runtime GenAI model builder |
| 2 | Preveli `Qwen/Qwen3-0.6B` s Hugging Face u optimizirani ONNX model |
| 3 | Kreirali konfig datoteku `inference_model.json` s predloškom za chat |
| 4 | Dodali prevedeni model u Foundry Local cache |
| 5 | Pokrenuli interaktivnu chat sesiju s prilagođenim modelom putem CLI-ja |
| 6 | Postavili upit modelu putem REST API-ja kompatibilnog s OpenAI |
| 7 | Povezali se preko Python, JavaScript i C# koristeći OpenAI SDK |
| 8 | Testirali prilagođeni model end-to-end koristeći Foundry Local SDK |

Glavna poruka je da **bilo koji transformacijski model može raditi kroz Foundry Local** čim bude preveden u ONNX format. API kompatibilan s OpenAI-jem znači da sav vaš postojeći kod za aplikacije radi bez promjena; potrebno je samo promijeniti naziv modela.

---

## Ključne točke

| Koncept | Detalj |
|---------|--------|
| ONNX Runtime GenAI Model Builder | Pretvara Hugging Face modele u ONNX format s kvantizacijom u jednoj naredbi |
| ONNX format | Foundry Local zahtijeva ONNX modele s ONNX Runtime GenAI konfiguracijom |
| Predlošci za chat | Datoteka `inference_model.json` govori Foundry Localu kako oblikovati promptove za dati model |
| Hardverski ciljevi | Kompajlirajte za CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) ili WebGPU ovisno o hardveru |
| Kvantizacija | Niža preciznost (int4) smanjuje veličinu i poboljšava brzinu uz cijenu točnosti; fp16 zadržava visoku kvalitetu na GPU-ima |
| Kompatibilnost API-ja | Prilagođeni modeli koriste isti OpenAI-kompatibilni API kao i ugrađeni modeli |
| Foundry Local SDK | SDK automatski upravlja pokretanjem servisa, otkrivanjem endpointa i učitavanjem modela za katalog i prilagođene modele |

---

## Daljnje čitanje

| Izvor | Link |
|--------|-------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local vodič za prilagođene modele | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 model obitelj | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive dokumentacija | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Sljedeći koraci

Nastavite na [Dio 11: Pozivanje alata s lokalnim modelima](part11-tool-calling.md) kako biste naučili kako omogućiti vašim lokalnim modelima pozivanje vanjskih funkcija.

[← Dio 9: Whisper prepoznavanje glasa](part9-whisper-voice-transcription.md) | [Dio 11: Pozivanje alata →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Odricanje od odgovornosti**:
Ovaj dokument je preveden koristeći AI uslugu prijevoda [Co-op Translator](https://github.com/Azure/co-op-translator). Iako nastojimo postići točnost, imajte na umu da automatski prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na izvornom jeziku smatra se službenim izvorom. Za kritične informacije preporučuje se profesionalni ljudski prijevod. Ne snosimo odgovornost za bilo kakva nesporazuma ili pogrešne interpretacije koje proizlaze iz korištenja ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->