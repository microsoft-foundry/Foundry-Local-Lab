![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Časť 10: Používanie vlastných alebo Hugging Face modelov s Foundry Local

> **Cieľ:** Preložiť model z Hugging Face do optimalizovaného formátu ONNX, ktorý Foundry Local vyžaduje, nakonfigurovať ho s chat šablónou, pridať ho do lokálnej cache a vykonať inferenciu pomocou CLI, REST API a OpenAI SDK.

## Prehľad

Foundry Local obsahuje kurátorský katalóg predkompilovaných modelov, ale nie ste obmedzení iba na tento zoznam. Akýkoľvek transformerový jazykový model dostupný na [Hugging Face](https://huggingface.co/) (alebo uložený lokálne vo formáte PyTorch / Safetensors) možno preložiť do optimalizovaného ONNX modelu a naservírovať cez Foundry Local.

Prekladacia pipeline používa **ONNX Runtime GenAI Model Builder**, príkazový nástroj zahrnutý v balíku `onnxruntime-genai`. Model builder spracúva ťažké úlohy: sťahuje pôvodné váhy, konvertuje ich do ONNX formátu, aplikuje kvantizáciu (int4, fp16, bf16) a generuje konfiguračné súbory (vrátane chat šablóny a tokenizéra), ktoré Foundry Local očakáva.

V tejto laboratórii preložíte model **Qwen/Qwen3-0.6B** z Hugging Face, zaregistrujete ho vo Foundry Local a budete s ním komunikovať úplne lokálne na vašom zariadení.

---

## Ciele učenia

Na konci tejto laboratória budete vedieť:

- Vysvetliť, prečo je preklad vlastného modelu užitočný a kedy ho môžete potrebovať
- Nainštalovať ONNX Runtime GenAI model builder
- Preložiť model z Hugging Face do optimalizovaného ONNX formátu jedným príkazom
- Porozumieť kľúčovým parametrom prekladu (vykonávací provider, presnosť)
- Vytvoriť konfiguračný súbor chat šablóny `inference_model.json`
- Pridať preložený model do cache Foundry Local
- Vykonať inferenciu na vlastnom modeli pomocou CLI, REST API a OpenAI SDK

---

## Predpoklady

| Požiadavka | Detaily |
|-------------|---------|
| **Foundry Local CLI** | Nainštalovaný a v `PATH` ([Časť 1](part1-getting-started.md)) |
| **Python 3.10+** | Potrebný pre ONNX Runtime GenAI model builder |
| **pip** | Správca balíčkov pre Python |
| **Diskový priestor** | Minimálne 5 GB voľného miesta pre zdrojové a preložené súbory modelu |
| **Účet na Hugging Face** | Niektoré modely vyžadujú prijatie licencie pred stiahnutím. Qwen3-0.6B používa licenciu Apache 2.0 a je voľne dostupný. |

---

## Nastavenie prostredia

Preklad modelu vyžaduje niekoľko veľkých Python balíčkov (PyTorch, ONNX Runtime GenAI, Transformers). Vytvorte samostatné virtuálne prostredie, aby tieto balíčky nezasahovali do vášho systémového Pythonu alebo iných projektov.

```bash
# Z koreňového adresára repozitára
python -m venv .venv
```

Aktivujte prostredie:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Aktualizujte pip, aby ste predišli problémom s riešením závislostí:

```bash
python -m pip install --upgrade pip
```

> **Tip:** Ak už máte `.venv` z predchádzajúcich laboratórií, môžete ho použiť znova. Iba sa uistite, že je aktivovaný pred pokračovaním.

---

## Koncept: Prekladacia pipeline

Foundry Local vyžaduje modely vo formáte ONNX s konfiguráciou ONNX Runtime GenAI. Väčšina open-source modelov na Hugging Face je distribuovaná vo formáte PyTorch alebo Safetensors, preto je potrebný konverzný krok.

![Pipeline prekladu vlastného modelu](../../../images/custom-model-pipeline.svg)

### Čo model builder robí?

1. **Stiahne** zdrojový model z Hugging Face (alebo ho načíta z lokálnej cesty).
2. **Prekonvertuje** váhy PyTorch / Safetensors do formátu ONNX.
3. **Kvantizuje** model na menšiu presnosť (napríklad int4), aby sa znížila pamäťová náročnosť a zvýšila priepustnosť.
4. **Vygeneruje** konfiguráciu ONNX Runtime GenAI (`genai_config.json`), chat šablónu (`chat_template.jinja`) a všetky tokenizérové súbory, aby Foundry Local mohol model načítať a servírovať.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Môžete naraziť na odkazovanie na **Microsoft Olive** ako alternatívny nástroj pre optimalizáciu modelov. Obe nástroje vedia vytvoriť ONNX modely, ale slúžia odlišným účelom a majú rôzne kompromisy:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Balík** | `onnxruntime-genai` | `olive-ai` |
| **Hlavný účel** | Konvertovať a kvantizovať generatívne AI modely pre ONNX Runtime GenAI inferenciu | Komplexný rámec pre optimalizáciu modelov podporujúci mnoho backendov a hardvérových cieľov |
| **Jednoduchosť použitia** | Jediný príkaz — jednorazová konverzia + kvantizácia | Založené na workflow — konfigurovateľné viacpriechodové pipeline s YAML/JSON |
| **Výstupný formát** | ONNX Runtime GenAI formát (pripravený pre Foundry Local) | Všeobecný ONNX, ONNX Runtime GenAI, alebo iné formáty podľa workflow |
| **Hardvérové ciele** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN a ďalšie |
| **Možnosti kvantizácie** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus optimalizácie grafu, ladanie po vrstvách |
| **Rozsah modelov** | Generatívne AI modely (LLM, SLM) | Akýkoľvek ONNX-konvertovateľný model (vízia, NLP, audio, multimodálne) |
| **Najvhodnejšie pre** | Rýchly preklad jedného modelu pre lokálnu inferenciu | Produkčné pipeline vyžadujúce detailnú kontrolu optimalizácie |
| **Závislosti** | Stredné (PyTorch, Transformers, ONNX Runtime) | Väčšie (zahŕňa framework Olive, voliteľné doplnky podľa workflow) |
| **Integrácia s Foundry Local** | Priama — výstup je okamžite kompatibilný | Vyžaduje parameter `--use_ort_genai` a ďalšiu konfiguráciu |

> **Prečo táto laboratórium používa Model Builder:** Pre úlohu preloženia jedného modelu z Hugging Face a jeho registráciu vo Foundry Local je Model Builder najjednoduchšia a najspoľahlivejšia cesta. Vyrába presne taký formát, aký Foundry Local očakáva, jedným príkazom. Ak neskôr potrebujete pokročilé optimalizačné funkcie — napríklad kvantizáciu s rozpoznaním presnosti, úpravu grafov či ladanie viacerých priechodov — Olive je mocná možnosť na preskúmanie. Viac informácií nájdete v [Microsoft Olive dokumentácii](https://microsoft.github.io/Olive/).

---

## Lab cvičenia

### Cvičenie 1: Nainštalujte ONNX Runtime GenAI Model Builder

Nainštalujte balík ONNX Runtime GenAI, ktorý obsahuje nástroj na zostavenie modelu:

```bash
pip install onnxruntime-genai
```

Overte inštaláciu tým, že skontrolujete dostupnosť model buildera:

```bash
python -m onnxruntime_genai.models.builder --help
```

Mali by ste vidieť výstup nápovedy s parametrami ako `-m` (názov modelu), `-o` (výstupná cesta), `-p` (presnosť) a `-e` (vykonávací provider).

> **Poznámka:** Model builder závisí od PyTorch, Transformers a niekoľkých ďalších balíčkov. Inštalácia môže trvať niekoľko minút.

---

### Cvičenie 2: Preložte Qwen3-0.6B pre CPU

Spustite nasledujúci príkaz na stiahnutie modelu Qwen3-0.6B z Hugging Face a jeho preklad pre CPU inferenciu s int4 kvantizáciou:

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

#### Čo robí každý parameter

| Parameter | Účel | Použitá hodnota |
|-----------|---------|-----------------|
| `-m` | ID modelu na Hugging Face alebo cesta k lokálnemu adresáru | `Qwen/Qwen3-0.6B` |
| `-o` | Adresár, kde sa uloží preložený ONNX model | `models/qwen3` |
| `-p` | Presnosť kvantizácie aplikovaná pri preklade | `int4` |
| `-e` | ONNX Runtime vykonávací provider (cieľový hardvér) | `cpu` |
| `--extra_options hf_token=false` | Preskočí autentifikáciu Hugging Face (v poriadku pre verejné modely) | `hf_token=false` |

> **Ako dlho to trvá?** Doba prekladu závisí od vášho hardvéru a veľkosti modelu. Pre Qwen3-0.6B s int4 kvantizáciou na modernom CPU očakávajte približne 5 až 15 minút. Väčšie modely trvajú úmerne dlhšie.

Po dokončení príkazu by ste mali vidieť adresár `models/qwen3` obsahujúci preložené súbory modelu. Skontrolujte výstup:

```bash
ls models/qwen3
```

Mali by ste vidieť súbory vrátane:
- `model.onnx` a `model.onnx.data` — preložené váhy modelu
- `genai_config.json` — konfigurácia ONNX Runtime GenAI
- `chat_template.jinja` — chat šablóna modelu (automaticky vygenerovaná)
- `tokenizer.json`, `tokenizer_config.json` — súbory tokenizéra
- Ďalšie slovníkové a konfiguračné súbory

---

### Cvičenie 3: Preložte pre GPU (Voliteľné)

Ak máte NVIDIA GPU s podporou CUDA, môžete preložiť GPU-optimalizovanú variantu pre rýchlejšiu inferenciu:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Poznámka:** GPU preklad vyžaduje `onnxruntime-gpu` a funkčnú inštaláciu CUDA. Ak tieto nie sú prítomné, model builder oznámi chybu. Túto časť môžete vynechať a pokračovať na CPU variante.

#### Referencia pre hardware-špecifický preklad

| Cieľ | Vykonávací provider (`-e`) | Odporúčaná presnosť (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` alebo `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` alebo `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Kompromisy presnosti

| Presnosť | Veľkosť | Rýchlosť | Kvalita |
|-----------|------|-------|---------|
| `fp32` | Najväčšia | Najpomalšia | Najvyššia presnosť |
| `fp16` | Veľká | Rýchla (GPU) | Veľmi dobrá presnosť |
| `int8` | Malá | Rýchla | Mierna strata presnosti |
| `int4` | Najmenšia | Najrýchlejšia | Stredná strata presnosti |

Pre väčšinu lokálneho vývoja `int4` na CPU poskytuje najlepší pomer rýchlosti a využitia zdrojov. Pre produktnú kvalitu sa odporúča `fp16` na CUDA GPU.

---

### Cvičenie 4: Vytvorte konfiguráciu chat šablóny

Model builder automaticky vytvára súbor `chat_template.jinja` a súbor `genai_config.json` vo výstupnom adresári. Foundry Local však potrebuje aj súbor `inference_model.json`, ktorý mu povie, ako formátovať výzvy pre váš model. Tento súbor definuje názov modelu a šablónu promptu, ktorá obalí používateľské správy správnymi špeciálnymi tokenmi.

#### Krok 1: Skontrolujte preložený výstup

Zobrazte obsah adresára s preloženým modelom:

```bash
ls models/qwen3
```

Mali by ste vidieť súbory ako:
- `model.onnx` a `model.onnx.data` — preložené váhy modelu
- `genai_config.json` — konfigurácia ONNX Runtime GenAI (automaticky vygenerovaná)
- `chat_template.jinja` — chat šablóna modelu (automaticky vygenerovaná)
- `tokenizer.json`, `tokenizer_config.json` — súbory tokenizéra
- Rôzne ďalšie konfiguračné a slovníkové súbory

#### Krok 2: Vygenerujte súbor inference_model.json

Súbor `inference_model.json` povie Foundry Local, ako správne formátovať prompt. Vytvorte Python skript s názvom `generate_chat_template.py` **v koreňovom adresári repozitára** (t.j. v tom istom adresári, kde máte priečinok `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Vytvorte minimálny rozhovor na extrahovanie šablóny chatu
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

# Vytvorte štruktúru inference_model.json
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

Skript spustite z koreňového adresára repozitára:

```bash
python generate_chat_template.py
```

> **Poznámka:** Balík `transformers` bol už nainštalovaný ako závislosť `onnxruntime-genai`. Ak sa zobrazí chyba `ImportError`, spustite najprv `pip install transformers`.

Skript vygeneruje súbor `inference_model.json` v adresári `models/qwen3`. Tento súbor povie Foundry Local, ako obaliť vstup používateľa správnymi špeciálnymi tokenmi pre Qwen3.

> **Dôležité:** Pole `"Name"` v `inference_model.json` (nastavené na `qwen3-0.6b` v tomto skripte) je **alias modelu**, ktorý budete používať vo všetkých nasledujúcich príkazoch a API volaniach. Ak tento názov zmeníte, upravte názov modelu v cvičeniach 6–10.

#### Krok 3: Overte konfiguráciu

Otvorte súbor `models/qwen3/inference_model.json` a overte, že obsahuje pole `Name` a objekt `PromptTemplate` so znakmi `assistant` a `prompt`. Šablóna promptu by mala zahŕňať špeciálne tokeny ako `<|im_start|>` a `<|im_end|>` (presné tokeny závisia od chat šablóny modelu).

> **Manuálna alternatíva:** Ak skript nechcete spustiť, môžete súbor vytvoriť manuálne. Kľúčovou požiadavkou je, že pole `prompt` musí obsahovať úplnú chat šablónu modelu s `{Content}` ako zástupcom pre správu používateľa.

---

### Cvičenie 5: Overte štruktúru adresára modelu
Skladateľ modelov ukladá všetky zostavené súbory priamo do výstupného adresára, ktorý ste zadali. Overte, či konečná štruktúra vyzerá správne:

```bash
ls models/qwen3
```

Adresár by mal obsahovať nasledujúce súbory:

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

> **Poznámka:** Na rozdiel od niektorých iných nástrojov na kompiláciu, skladateľ modelov nevytvára vnorené podadresáre. Všetky súbory sa nachádzajú priamo vo výstupnej zložke, čo je presne to, čo očakáva Foundry Local.

---

### Cvičenie 6: Pridanie modelu do Foundry Local Cache

Povedzte Foundry Local, kde nájsť váš zostavený model pridaním adresára do jeho cache:

```bash
foundry cache cd models/qwen3
```

Overte, či sa model zobrazuje v cache:

```bash
foundry cache ls
```

Mali by ste vidieť váš vlastný model uvedený spolu s už predtým uloženými modelmi v cache (ako napríklad `phi-3.5-mini` alebo `phi-4-mini`).

---

### Cvičenie 7: Spustenie vlastného modelu cez CLI

Spustite interaktívnu chatovaciu reláciu so svojim novo zostaveným modelom (alias `qwen3-0.6b` pochádza z poľa `Name`, ktoré ste nastavili v `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Prepínač `--verbose` zobrazuje ďalšie diagnostické informácie, ktoré sú užitočné pri prvom testovaní vlastného modelu. Ak sa model úspešne načíta, uvidíte interaktívne výzvy. Vyskúšajte niekoľko správ:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Napíšte `exit` alebo stlačte `Ctrl+C` pre ukončenie relácie.

> **Riešenie problémov:** Ak sa model nepodarí načítať, skontrolujte nasledujúce:
> - Súbor `genai_config.json` bol vygenerovaný skladateľom modelov.
> - Súbor `inference_model.json` existuje a je platný JSON.
> - ONNX modelové súbory sú v správnom adresári.
> - Máte dostatok dostupnej RAM (Qwen3-0.6B int4 potrebuje približne 1 GB).
> - Qwen3 je model na uvažovanie, ktorý produkuje značky `<think>`. Ak vidíte odpovede predponované `<think>...</think>`, je to bežné správanie. Šablóna promptu v `inference_model.json` sa dá upraviť na potlačenie výstupu uvažovania.

---

### Cvičenie 8: Dopyt na vlastný model cez REST API

Ak ste ukončili interaktívnu reláciu v cvičení 7, model môže byť už vypnutý. Najskôr spustite službu Foundry Local a načítajte model:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Skontrolujte, na ktorom porte služba beží:

```bash
foundry service status
```

Potom odošlite požiadavku (nahradte `5273` vaším aktuálnym portom, ak sa líši):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Poznámka pre Windows:** Príkaz `curl` vyššie používa syntax bash. Na Windows použite namiesto toho cmdlet PowerShell `Invoke-RestMethod`.

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

### Cvičenie 9: Použitie vlastného modelu s OpenAI SDK

K vlastnému modelu sa môžete pripojiť pomocou presne toho istého kódu OpenAI SDK, aký ste použili pre zabudované modely (pozri [Časť 3](part3-sdk-and-apis.md)). Jediný rozdiel je v názve modelu.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local neoveruje API kľúče
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
  apiKey: "foundry-local", // Foundry Local neoveruje API kľúče
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

> **Kľúčový bod:** Pretože Foundry Local poskytuje API kompatibilné s OpenAI, akýkoľvek kód, ktorý funguje so zabudovanými modelmi, funguje aj s vašimi vlastnými modelmi. Stačí vám iba zmeniť parameter `model`.

---

### Cvičenie 10: Testovanie vlastného modelu s Foundry Local SDK

V predchádzajúcich laboch ste používali Foundry Local SDK na spustenie služby, nájdenie endpointu a automatickú správu modelov. Môžete použiť presne rovnaký vzor aj s vlastným zostaveným modelom. SDK spravuje spustenie služby a zisťovanie endpointu, takže vo svojom kóde nemusíte pevne zakódovať `localhost:5273`.

> **Poznámka:** Pred spustením týchto príkladov sa uistite, že Foundry Local SDK je nainštalované:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Pridajte NuGet balíčky `Microsoft.AI.Foundry.Local` a `OpenAI`
>
> Každý skript **uložte v koreňovom adresári repozitára** (rovnaký adresár ako vaša zložka `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Krok 1: Spustite službu Foundry Local a načítajte vlastný model
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Krok 2: Skontrolujte cache pre vlastný model
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Krok 3: Načítajte model do pamäte
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Krok 4: Vytvorte klienta OpenAI pomocou SDK nájdeného koncového bodu
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Krok 5: Odoslať požiadavku na dokončenie chatu prúdením
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

Spustite ho:

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

// Krok 1: Spustite službu Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Krok 2: Získajte vlastný model z katalógu
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Krok 3: Načítajte model do pamäti
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Krok 4: Vytvorte OpenAI klienta pomocou SDK nájdeného koncového bodu
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Krok 5: Odoslať požiadavku na dokončenie chatu so streamovaním
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

Spustite ho:

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

> **Kľúčový bod:** Foundry Local SDK dynamicky zisťuje endpoint, takže nikdy nepevným zakódujete číslo portu. Toto je odporúčaný prístup pre produkčné aplikácie. Váš vlastný zostavený model funguje rovnako ako vstavané modely v katalógu cez SDK.

---

## Výber modelu na kompiláciu

Qwen3-0.6B sa v tejto učebnej lekcii používa ako referenčný príklad, pretože je malý, rýchlo sa kompiluje a je voľne dostupný pod licenciou Apache 2.0. Môžete však kompilovať mnoho ďalších modelov. Tu sú niektoré návrhy:

| Model | Hugging Face ID | Parametre | Licencia | Poznámky |
|-------|-----------------|-----------|----------|----------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Veľmi malý, rýchla kompilácia, vhodný na testovanie |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Lepšia kvalita, stále rýchla kompilácia |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Silná kvalita, vyžaduje viac RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Vyžaduje akceptovanie licencie na Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Vysoká kvalita, väčšie sťahovanie a dlhšia kompilácia |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Už v katalógu Foundry Local (užitočné na porovnanie) |

> **Pripomienka licencie:** Vždy si pred použitím modelu skontrolujte jeho licenciu na Hugging Face. Niektoré modely (napr. Llama) vyžadujú akceptovanie licenčnej zmluvy a overenie pomocou `huggingface-cli login` pred stiahnutím.

---

## Koncepty: Kedy použiť vlastné modely

| Scenár | Prečo si kompilovať vlastný? |
|--------|------------------------------|
| **Model, ktorý potrebujete, nie je v katalógu** | Katalóg Foundry Local je kurátorský. Ak model, ktorý chcete, nie je uvedený, skompilujte si ho sami. |
| **Modely doladené na mieru (fine-tuned)** | Ak ste model doladili na doménovo špecifické dáta, potrebujete skompilovať vlastné váhy. |
| **Špecifické požiadavky na kvantizáciu** | Môžete chcieť presnosť alebo stratégiu kvantizácie, ktorá sa líši od predvoleného katalógu. |
| **Nové vydania modelov** | Keď je nový model uvoľnený na Hugging Face, nemusí byť ešte v katalógu Foundry Local. Kompilácia vám umožní okamžitý prístup. |
| **Výskum a experimentovanie** | Vyskúšanie rôznych architektúr modelov, veľkostí alebo konfigurácií lokálne pred zavedením do produkcie. |

---

## Zhrnutie

V tomto kurze ste sa naučili, ako:

| Krok | Čo ste urobili |
|------|----------------|
| 1 | Nainštalovali ONNX Runtime GenAI model builder |
| 2 | Skompilovali `Qwen/Qwen3-0.6B` z Hugging Face do optimalizovaného ONNX modelu |
| 3 | Vytvorili konfiguračný súbor šablóny chatu `inference_model.json` |
| 4 | Pridali zostavený model do Foundry Local cache |
| 5 | Spustili interaktívny chat s vlastným modelom cez CLI |
| 6 | Zadali dotazy na model cez REST API kompatibilné s OpenAI |
| 7 | Pripojili sa z Pythonu, JavaScriptu a C# pomocou OpenAI SDK |
| 8 | Otestovali vlastný model end-to-end s Foundry Local SDK |

Hlavná myšlienka je, že **akýkoľvek model založený na transformeroch môže bežať cez Foundry Local** po kompilácii do formátu ONNX. API kompatibilné s OpenAI znamená, že všetok váš existujúci aplikačný kód funguje bez zmien; stačí iba zmeniť názov modelu.

---

## Kľúčové poučenia

| Koncept | Detail |
|---------|--------|
| ONNX Runtime GenAI Model Builder | Konvertuje modely z Hugging Face do formátu ONNX s kvantizáciou jediným príkazom |
| ONNX formát | Foundry Local vyžaduje ONNX modely s konfiguráciou ONNX Runtime GenAI |
| Šablóny chatov | Súbor `inference_model.json` hovorí Foundry Local, ako formátovať prompt pre daný model |
| Cieľové hardvéry | Kompilujte pre CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) alebo WebGPU podľa dostupného hardvéru |
| Kvantizácia | Nižšia presnosť (int4) znižuje veľkosť a zrýchľuje beh na úkor presnosti; fp16 zachováva vysokú kvalitu na GPU |
| Kompatibilita API | Vlastné modely používajú rovnaké API kompatibilné s OpenAI ako zabudované modely |
| Foundry Local SDK | SDK automaticky spravuje spustenie služby, nájdenie endpointu a načítanie modelu pre katalógové aj vlastné modely |

---

## Ďalšie čítanie

| Zdroj | Odkaz |
|-------|-------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local príručka vlastných modelov | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Rodina modelov Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Dokumentácia Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Ďalšie kroky

Pokračujte k [Časti 11: Volanie nástrojov s lokálnymi modelmi](part11-tool-calling.md), kde sa naučíte, ako umožniť vašim lokálnym modelom volať externé funkcie.

[← Časť 9: Prepis hlasu Whisperom](part9-whisper-voice-transcription.md) | [Časť 11: Volanie nástrojov →](part11-tool-calling.md)