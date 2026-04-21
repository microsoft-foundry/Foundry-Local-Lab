![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Deo 10: Korišćenje prilagođenih ili Hugging Face modela sa Foundry Local

> **Cilj:** Sastaviti Hugging Face model u optimizovani ONNX format koji Foundry Local zahteva, konfigurisati ga sa šablonom za ćaskanje, dodati ga u lokalni keš i izvršiti inferenciju korišćenjem CLI-ja, REST API-ja i OpenAI SDK-a.

## Pregled

Foundry Local dolazi sa kuriranim katalogom prethodno sastavljenih modela, ali niste ograničeni samo na tu listu. Bilo koji model zasnovan na transformatorima dostupan na [Hugging Face](https://huggingface.co/) (ili lokalno uskladišten u PyTorch / Safetensors formatu) može biti sastavljen u optimizovani ONNX model i poslužen preko Foundry Local.

Cevovod za sastavljanje koristi **ONNX Runtime GenAI Model Builder**, komandni alat uključen u `onnxruntime-genai` paket. Model builder obavlja glavni posao: preuzima izvorne težine, konvertuje ih u ONNX format, primenjuje kvantizaciju (int4, fp16, bf16) i emituje konfiguracione fajlove (uključujući šablon za ćaskanje i tokenizator) koje Foundry Local očekuje.

U ovoj vežbi ćete sastaviti **Qwen/Qwen3-0.6B** sa Hugging Face, registrovati ga u Foundry Local i ćaskati s njim potpuno na vašem uređaju.

---

## Ciljevi učenja

Na kraju ove vežbe moći ćete da:

- Objasnite zašto je korisno sastavljati prilagođene modele i kada vam je potrebno
- Instalirate ONNX Runtime GenAI model builder
- Sastavite Hugging Face model u optimizovani ONNX format jednim komandnim redom
- Razumete ključne parametre sastavljanja (izvršni provajder, preciznost)
- Kreirate konfiguracioni fajl `inference_model.json` za šablon ćaskanja
- Dodate sastavljeni model u Foundry Local keš
- Izvršite inferenciju nad prilagođenim modelom koristeći CLI, REST API i OpenAI SDK

---

## Preduslovi

| Zahtev | Detalji |
|-------------|---------|
| **Foundry Local CLI** | Instaliran i dostupan u vašem `PATH` ([Deo 1](part1-getting-started.md)) |
| **Python 3.10+** | Neophodan za ONNX Runtime GenAI model builder |
| **pip** | Python upravljač paketima |
| **Disk prostora** | Najmanje 5 GB slobodnog za izvore i sastavljene fajlove modela |
| **Hugging Face nalog** | Neki modeli zahtevaju prihvatanje licence pre preuzimanja. Qwen3-0.6B koristi Apache 2.0 licencu i slobodno je dostupan. |

---

## Podešavanje okruženja

Za sastavljanje modela potrebni su veliki Python paketi (PyTorch, ONNX Runtime GenAI, Transformers). Napravite posvećeno virtuelno okruženje kako bi ovi paketi bili izolovani od vašeg sistemskog Pythona ili drugih projekata.

```bash
# Из корена репозиторијума
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

Nadogradite pip da biste izbegli probleme sa zavisnostima:

```bash
python -m pip install --upgrade pip
```

> **Savet:** Ako već imate `.venv` iz ranijih vežbi, možete ga ponovo koristiti. Samo se uverite da je aktiviran pre nego što nastavite.

---

## Koncept: Cevovod za sastavljanje

Foundry Local zahteva modele u ONNX formatu sa ONNX Runtime GenAI konfiguracijom. Većina open-source modela na Hugging Face distribuirana su kao PyTorch ili Safetensors težine, tako da je neophodna konverzija.

![Cevovod za sastavljanje prilagođenog modela](../../../images/custom-model-pipeline.svg)

### Šta radi Model Builder?

1. **Preuzima** izvorni model sa Hugging Face (ili učitava sa lokalnog puta).
2. **Konvertuje** PyTorch / Safetensors težine u ONNX format.
3. **Kvantizuje** model na manju preciznost (npr. int4) radi smanjenja potrošnje memorije i povećanja propusnosti.
4. **Emituje** ONNX Runtime GenAI konfiguraciju (`genai_config.json`), šablon za ćaskanje (`chat_template.jinja`), i sve fajlove tokenizatora da bi Foundry Local mogao da učita i posluži model.

### ONNX Runtime GenAI Model Builder naspram Microsoft Olive

Možda ste upoznati sa **Microsoft Olive** kao alternativnim alatom za optimizaciju modela. Ova dva alata mogu proizvoditi ONNX modele, ali služe različitim svrhama i imaju različite kompromise:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Paket** | `onnxruntime-genai` | `olive-ai` |
| **Primarna svrha** | Konvertovanje i kvantizacija generativnih AI modela za ONNX Runtime GenAI inferenciju | Okvir za optimizaciju modela od početka do kraja sa podrškom za brojne backend-ove i hardverske ciljeve |
| **Jednostavnost korišćenja** | Jedan komandni red — konverzija + kvantizacija u jednom koraku | Radni tok baziran — konfigurabilni višepasovni cevovodi sa YAML/JSON |
| **Format izlaza** | ONNX Runtime GenAI format (spreman za Foundry Local) | Generički ONNX, ONNX Runtime GenAI ili drugi formati u zavisnosti od radnog toka |
| **Target hardvera** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN i drugi |
| **Opcije kvantizacije** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus optimizacije grafa, podešavanje po sloju |
| **Obuhvat modela** | Generativni AI modeli (LLM, SLM) | Bilo koji model konvertibilan u ONNX (vizuelni, NLP, audio, multimodalni) |
| **Najbolje za** | Brzo sastavljanje pojedinačnih modela za lokalnu inferenciju | Produkcioni cevovodi kojima je potrebna detaljna kontrola optimizacije |
| **Zavisnosti** | Umerene (PyTorch, Transformers, ONNX Runtime) | Veće (dodaje Olive okvir, opciono dodatke po radnom toku) |
| **Integracija sa Foundry Local** | Direktna — izlaz odmah kompatibilan | Zahteva `--use_ort_genai` zastavicu i dodatnu konfiguraciju |

> **Zašto ova vežba koristi Model Builder:** Za zadatak sastavljanja jednog Hugging Face modela i registraciju u Foundry Local, Model Builder je najjednostavniji i najpouzdaniji put. On proizvodi tačan format izlaza koji Foundry Local očekuje u jednoj komandi. Ako vam kasnije budu potrebne napredne opcije optimizacije — poput kvantizacije sa očuvanjem tačnosti, intervencija na grafu, ili višepasovnog podešavanja — Olive je moćna opcija za istraživanje. Pogledajte [Microsoft Olive dokumentaciju](https://microsoft.github.io/Olive/) za više detalja.

---

## Laboratorijske vežbe

### Vežba 1: Instalirajte ONNX Runtime GenAI Model Builder

Instalirajte ONNX Runtime GenAI paket koji uključuje alat za sastavljanje modela:

```bash
pip install onnxruntime-genai
```

Proverite instalaciju tako što ćete se uveriti da je alat za sastavljanje dostupан:

```bash
python -m onnxruntime_genai.models.builder --help
```

Trebalo bi da vidite pomoćni ispis sa parametrima kao što su `-m` (ime modela), `-o` (putanja izlaza), `-p` (preciznost) i `-e` (izvršni provajder).

> **Napomena:** Model builder zavisi od PyTorch, Transformers i nekoliko drugih paketa. Instalacija može potrajati nekoliko minuta.

---

### Vežba 2: Sastavite Qwen3-0.6B za CPU

Pokrenite sledeću komandu da preuzmete Qwen3-0.6B model sa Hugging Face i sastavite ga za CPU inferenciju sa int4 kvantizacijom:

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

#### Šta svaki parametar radi

| Parametar | Svrha | Korisćena vrednost |
|-----------|---------|------------|
| `-m` | ID modela sa Hugging Face ili lokalni direktorijum | `Qwen/Qwen3-0.6B` |
| `-o` | Direktorijum u koji će biti sačuvan sastavljeni ONNX model | `models/qwen3` |
| `-p` | Preciznost kvantizacije korišćena pri sastavljanju | `int4` |
| `-e` | Izvršni provajder ONNX Runtime-a (ciljni hardver) | `cpu` |
| `--extra_options hf_token=false` | Preskače autentifikaciju na Hugging Face (pogodno za javne modele) | `hf_token=false` |

> **Koliko traje ovaj proces?** Vreme sastavljanja zavisi od vašeg hardvera i veličine modela. Za Qwen3-0.6B sa int4 kvantizacijom na modernom CPU očekujte otprilike 5 do 15 minuta. Veći modeli traju proporcionalno duže.

Po završetku komande trebalo bi da vidite direktorijum `models/qwen3` koji sadrži sastavljene fajlove modela. Proverite izlaz:

```bash
ls models/qwen3
```

Trebalo bi da vidite fajlove kao što su:
- `model.onnx` i `model.onnx.data` — sastavljene težine modela
- `genai_config.json` — ONNX Runtime GenAI konfiguracija
- `chat_template.jinja` — šablon za ćaskanje modela (automatski generisan)
- `tokenizer.json`, `tokenizer_config.json` — fajlovi tokenizatora
- Ostale datoteke rečnika i konfiguracije

---

### Vežba 3: Sastavite za GPU (opciono)

Ako imate NVIDIA GPU sa CUDA podrškom, možete sastaviti GPU-optimizovanu varijantu za bržu inferenciju:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Napomena:** Sastavljanje za GPU zahteva `onnxruntime-gpu` i ispravnu CUDA instalaciju. Ako nisu prisutni, model builder će prijaviti grešku. Možete preskočiti ovu vežbu i nastaviti sa varijantom za CPU.

#### Referenca za specifično hardversko sastavljanje

| Cilj | Izvršni provajder (`-e`) | Preporučena preciznost (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` ili `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` ili `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Kompromisi u preciznosti

| Preciznost | Veličina | Brzina | Kvalitet |
|-----------|------|-------|---------|
| `fp32` | Najveća | Najsporija | Najveća tačnost |
| `fp16` | Velika | Brza (GPU) | Veoma dobra tačnost |
| `int8` | Mala | Brza | Malo smanjenje tačnosti |
| `int4` | Najmanja | Najbrža | Umereno smanjenje tačnosti |

Za većinu lokalnog razvoja, `int4` na CPU pruža najbolju ravnotežu brzine i potrošnje resursa. Za produkcijski kvalitet, preporučuje se `fp16` na CUDA GPU.

---

### Vežba 4: Kreirajte konfiguraciju šablona ćaskanja

Model builder automatski generiše fajl `chat_template.jinja` i `genai_config.json` u izlaznom direktorijumu. Međutim, Foundry Local takođe zahteva fajl `inference_model.json` da zna kako da formatira upite za vaš model. Ovaj fajl definiše ime modela i šablon prompta koji omotava korisničke poruke u odgovarajuće specijalne tokene.

#### Korak 1: Pregledajte sastavljeni izlaz

Navedite sadržaj direktorijuma sa sastavljenim modelom:

```bash
ls models/qwen3
```

Trebalo bi da vidite fajlove kao:
- `model.onnx` i `model.onnx.data` — sastavljene težine modela
- `genai_config.json` — ONNX Runtime GenAI konfiguracija (automatski generisana)
- `chat_template.jinja` — šablon za ćaskanje modela (automatski generisan)
- `tokenizer.json`, `tokenizer_config.json` — fajlovi tokenizatora
- Razne druge konfiguracione i rečničke datoteke

#### Korak 2: Generišite fajl inference_model.json

Fajl `inference_model.json` govori Foundry Local kako da formatira upite. Napravite Python skriptu pod nazivom `generate_chat_template.py` **u korenu repozitorijuma** (u istom direktorijumu gde se nalazi folder `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Направите минималан разговор за извлачење шаблона ћаскања
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

# Направите структуру inference_model.json
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

Pokrenite skriptu iz korena repozitorijuma:

```bash
python generate_chat_template.py
```

> **Napomena:** Paket `transformers` je već instaliran kao zavisnost `onnxruntime-genai`. Ako dobijete `ImportError`, prvo pokrenite `pip install transformers`.

Skripta proizvodi fajl `inference_model.json` unutar direktorijuma `models/qwen3`. Taj fajl govori Foundry Local kako da omota korisnički unos u ispravne specijalne tokene za Qwen3.

> **Važno:** Polje `"Name"` u `inference_model.json` (podešeno na `qwen3-0.6b` u ovoj skripti) je **alias modela** koji ćete koristiti u svim narednim komandama i API pozivima. Ako promenite ovo ime, ažurirajte naziv modela u Vežbama 6–10 u skladu sa tim.

#### Korak 3: Proverite konfiguraciju

Otvorite `models/qwen3/inference_model.json` i potvrdite da sadrži polje `Name` i objekat `PromptTemplate` sa ključevima `assistant` i `prompt`. Šablon prompta treba da uključuje specijalne tokene poput `<|im_start|>` i `<|im_end|>` (tačni tokeni zavise od šablona ćaskanja modela).

> **Ručno alternativno rešenje:** Ako ne želite da pokrećete skriptu, fajl možete napraviti i ručno. Ključni zahtev je da polje `prompt` sadrži kompletan šablon ćaskanja modela sa `{Content}` kao rezervisanom oznakom za korisničku poruku.

---

### Vežba 5: Proverite strukturu direktorijuma modela
Изграђивач модела смешта све састављене датотеке директно у излазни директоријум који сте навели. Проверите да ли коначна структура изгледа исправно:

```bash
ls models/qwen3
```

Директоријум треба да садржи следеће датотеке:

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

> **Напомена:** За разлику од неких других алата за компилацију, изграђивач модела не креира уметнуте поддиректоријуме. Све датотеке су смештене директно у излазни фолдер, што је управо оно што Foundry Local очекује.

---

### Вежба 6: Додајте модел у кеш Foundry Local

Обавестите Foundry Local где да пронађе ваш састављени модел додавањем директоријума у његов кеш:

```bash
foundry cache cd models/qwen3
```

Проверите да ли се модел појављује у кешу:

```bash
foundry cache ls
```

Требало би да видите ваш прилагођени модел наведен поред модела које сте раније сачували у кешу (као што су `phi-3.5-mini` или `phi-4-mini`).

---

### Вежба 7: Покрените прилагођени модел помоћу CLI

Покрените интерактивну ћаскање са вашим ново састављеним моделом (алијас `qwen3-0.6b` долази из поља `Name` које сте поставили у `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Опција `--verbose` приказује додатне дијагностичке информације, што је корисно приликом првог тестирања прилагођеног модела. Ако се модел успешно учита, видећете интерактивни промпт. Испробајте неколико порука:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Укуцајте `exit` или притисните `Ctrl+C` да завршите сесију.

> **Решавање проблема:** Ако се модел не учита, проверите следеће:
> - Да ли је датотека `genai_config.json` генерисана од стране изграђивача модела.
> - Да ли датотека `inference_model.json` постоји и да ли је важећи JSON.
> - Да ли се ONNX датотеке модела налазе у исправном директоријуму.
> - Да ли имате довољно расположиве RAM меморије (Qwen3-0.6B int4 захтева око 1 GB).
> - Qwen3 је модел за размишљање који производи `<think>` тагове. Ако видите одговоре са `<think>...</think>` на почетку, то је нормално понашање. Образац промпта у `inference_model.json` може се прилагодити да потисне излаз размишљања.

---

### Вежба 8: Питајте прилагођени модел преко REST API-ja

Ако сте напустили интерактивну сесију у Вежби 7, модел можда више није учитан. Прво покрените Foundry Local услугу и учитајте модел:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Проверите на ком порту услуга ради:

```bash
foundry service status
```

Затим пошаљите захтев (замените `5273` вашим стварним портом ако је другачији):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Напомена за Windows:** Команда `curl` изнад користи bash синтаксу. На Windows-у уместо тога користите PowerShell cmdlet `Invoke-RestMethod`.

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

### Вежба 9: Користите прилагођени модел са OpenAI SDK

Можете се повезати на ваш прилагођени модел користећи тачно исти OpenAI SDK код који сте користили за уграђене моделе (погледајте [Део 3](part3-sdk-and-apis.md)). Једина разлика је име модела.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local не верификује API кључеве
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
  apiKey: "foundry-local", // Foundry Local не валидира API кључеве
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

> **Кључна напомена:** Пошто Foundry Local излаже OpenAI компатибилан API, сваки код који ради са уграђеним моделима ради и са вашим прилагођеним моделима. Потребно је само да промените параметар `model`.

---

### Вежба 10: Тестирајте прилагођени модел са Foundry Local SDK

У ранијим лабораторијама користили сте Foundry Local SDK да бисте покренули услугу, пронашли крајњу тачку и управљали моделима аутоматски. Можете следити тачно исти образац и са вашим прилагођеним састављеним моделом. SDK управља покретањем услуге и откривањем крајње тачке, па ваш код не мора да убацује `localhost:5273` директно.

> **Напомена:** Уверите се да је Foundry Local SDK инсталиран пре покретања ових примера:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Додајте NuGet пакете `Microsoft.AI.Foundry.Local` и `OpenAI`
>
> Сачувајте сваки скрипт **у корен репозиторијума** (у истом фолдеру где вам је `models/` фасцикла).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Корак 1: Покрените Foundry Local услугу и учитајте прилагођени модел
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Корак 2: Проверите кеш за прилагођени модел
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Корак 3: Учитајте модел у меморију
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Корак 4: Креирајте OpenAI клијента користећи SDK-ом пронађену крајњу тачку
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Корак 5: Пошаљите захтев за стриминг завршетак разговора
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

Покрените га:

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

// Корак 1: Покрените Foundry Local услугу
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Корак 2: Преузмите прилагођени модел из каталога
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Корак 3: Учитајте модел у меморију
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Корак 4: Креирајте OpenAI клијента користећи SDK откривену крајњу тачку
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Корак 5: Пошаљите захтев за стримовање завршетка чат-а
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

Покрените га:

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

> **Кључна напомена:** Foundry Local SDK динамички открива крајњу тачку, тако да никада не користите хардкодирани број порта. Ово је препоручени приступ за производне апликације. Ваш прилагођени састављени модел функционише идентично као уграђени модели из каталога преко SDK.

---

## Избор модела за компилацију

Qwen3-0.6B се користи као референтни пример у овој лабораторији јер је мали, брзо се компајлира и слободно је доступан под Apache 2.0 лиценцом. Међутим, можете састављати и многе друге моделе. Ево неколико предлога:

| Модел | Hugging Face ID | Параметри | Лиценца | Напомене |
|-------|-----------------|------------|---------|----------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Врло мали, брза компилација, добар за тестирање |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Бољи квалитет, још увек брза компилација |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Снажан квалитет, треба више RAM-а |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Захтева прихватање лиценце на Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Висок квалитет, већа преузимања и дужа компилација |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Већ у Foundry Local каталогу (корисно за поређење) |

> **Подсећање о лиценци:** Увек проверите лиценцу модела на Hugging Face пре коришћења. Неки модели (као што је Llama) захтевају да прихватите уговор о лиценци и аутентификујете се помоћу `huggingface-cli login` пре преузимања.

---

## Концепти: Када користити прилагођене моделе

| Сценарио | Зашто компајлирати свој? |
|----------|--------------------------|
| **Модел који вам треба није у каталогу** | Foundry Local каталог је куриран. Ако није наведено онај модел који желите, компајлирајте га сами. |
| **Фино подешени модели** | Ако сте фино подесили модел на податке специфичне за домен, потребно је да компајлирате своје тежине. |
| **Специфични захтеви за квантизацију** | Можда желите прецизнију или другачију стратегију квантизације од подразумеване у каталогу. |
| **Новија издања модела** | Када је нови модел објављен на Hugging Face, можда још није у Foundry Local каталогу. Компилирање омогућава тренутни приступ. |
| **Истраживање и експериментисање** | Испробавање различитих архитектура модела, величина или конфигурација локално пре привржавања производном избору. |

---

## Резиме

У овој лабораторији сте научили како да:

| Корак | Шта сте урадили |
|-------|-----------------|
| 1 | Инсталирали ONNX Runtime GenAI изграђивач модела |
| 2 | Саставили `Qwen/Qwen3-0.6B` са Hugging Face у оптимизовани ONNX модел |
| 3 | Креирали конфигурациони фајл шаблона ћаскања `inference_model.json` |
| 4 | Додали састављени модел у кеш Foundry Local |
| 5 | Покренули интерактивно ћаскање са прилагођеним моделом преко CLI |
| 6 | Питали модел преко OpenAI компатибилног REST API-ја |
| 7 | Повезали се из Pythona, JavaScripta и C# користећи OpenAI SDK |
| 8 | Тестирали прилагођени модел од почетка до краја са Foundry Local SDK |

Главна поука је да **сваки трансформер модел може радити преко Foundry Local** када је састављен у ONNX формат. OpenAI-компатибилан API значи да ваш постојећи код ради без измена; потребно је само да промените име модела.

---

## Кључне поуке

| Концепт | Детаљ |
|---------|--------|
| ONNX Runtime GenAI изграђивач модела | Претвара Hugging Face моделe у ONNX формат са квантизацијом у једној команди |
| ONNX формат | Foundry Local захтева ONNX моделе са ONNX Runtime GenAI конфигурацијом |
| Шаблони ћаскања | Фајл `inference_model.json` говори Foundry Local-у како да форматира промптове за одређени модел |
| Хардверске мета | Компилирајте за CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) или WebGPU у зависности од вашег хардвера |
| Квантизација | Нижа прецизност (int4) смањује величину и повећава брзину по цену неке тачности; fp16 задржава висок квалитет на GPU-овима |
| Компатибилност API-ја | Прилагођени модели користе исти OpenAI-компатибилан API као и уграђени модели |
| Foundry Local SDK | SDK аутоматски управља покретањем услуге, откривањем крајње тачке и учитавањем модела за каталог и прилагођене моделе |

---

## Додатно читање

| Ресурс | Линк |
|--------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local водич за прилагођене моделе | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 породично дрво модела | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive документација | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Следећи кораци

Наставите на [Део 11: Позив алата са локалним моделима](part11-tool-calling.md) да бисте научили како да омогућите вашим локалним моделима да позивају спољне функције.

[← Део 9: Whisper препис говора](part9-whisper-voice-transcription.md) | [Део 11: Позив алата →](part11-tool-calling.md)