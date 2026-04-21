![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Sehemu ya 10: Kutumia Mifano ya Custom au Hugging Face na Foundry Local

> **Lengo:** Kukusanya mfano wa Hugging Face katika fomati iliyoboreshwa ya ONNX ambayo Foundry Local inahitaji, kuuisanidi na kiolezo cha mazungumzo, kuiongeza katika cache ya mahali hapa, na kuendesha utambuzi dhidi yake kwa kutumia CLI, REST API, na OpenAI SDK.

## Muhtasari

Foundry Local huja na orodha iliyochaguliwa ya mifano iliyokusanywa kabla, lakini hauwezi kufungiwa kwa orodha hiyo tu. Mfano wowote wa lugha unaotumia transformer unaopatikana kwenye [Hugging Face](https://huggingface.co/) (au kuhifadhiwa kwa njia ya PyTorch / Safetensors ndani ya mashine yako) unaweza kukusanywa kuwa mfano wa ONNX ulioboreshwa na kutumika kupitia Foundry Local.

Mtiririko wa ukusanyaji hutumia **ONNX Runtime GenAI Model Builder**, chombo cha mstari wa amri kilicho katika kifurushi cha `onnxruntime-genai`. Mjenzi wa mfano huchukua jukumu kubwa: kupakua uzito wa chanzo, kuyabadilisha kuwa fomati ya ONNX, kutekeleza upunguzaji wa ukubwa (int4, fp16, bf16), na kutoa faili za usanidi (ikiwemo kiolezo cha mazungumzo na tokeniser) ambacho Foundry Local inatarajia.

Katika maabara hii utakusanya **Qwen/Qwen3-0.6B** kutoka Hugging Face, kujiandikisha nayo katika Foundry Local, na kuzungumza nayo kabisa katika kifaa chako.

---

## Malengo ya Kujifunza

Mwisho wa maabara hii utaweza:

- Eleza kwa nini ukusanyaji wa mfano maalum ni muhimu na lini unaweza kuhitaji
- Sakinisha mjenzi wa mfano wa ONNX Runtime GenAI
- Kusanya mfano wa Hugging Face kwa fomati ya ONNX iliyoboreshwa kwa amri moja
- Elewa vigezo muhimu vya ukusanyaji (mtoa utekelezaji, usahihi)
- Tengeneza faili la usanidi wa kiolezo cha mazungumzo `inference_model.json`
- Ongeza mfano uliokusanywa katika cache ya Foundry Local
- Endesha utambuzi dhidi ya mfano maalum kwa kutumia CLI, REST API, na OpenAI SDK

---

## Mahitaji

| Mahitaji | Maelezo |
|-------------|---------|
| **Foundry Local CLI** | Imewekwa na iko kwenye `PATH` yako ([Sehemu 1](part1-getting-started.md)) |
| **Python 3.10+** | Inahitajika na mjenzi wa mfano wa ONNX Runtime GenAI |
| **pip** | Meneja wa vifurushi vya Python |
| **Nafasi ya Diski** | Angalau GB 5 huru kwa faili za chanzo na mfano uliokusanywa |
| **Akaunti ya Hugging Face** | Baadhi ya mifano inahitaji kukubali leseni kabla ya kupakua. Qwen3-0.6B inatumia leseni ya Apache 2.0 na inapatikana bure. |

---

## Kusanidi Mazingira

Ukusanyaji wa mfano unahitaji vifurushi vikubwa vya Python (PyTorch, ONNX Runtime GenAI, Transformers). Tengeneza mazingira ya kidijitali ili visiingilie Python ya mfumo wako au miradi mingine.

```bash
# Kutoka kwenye mzizi wa hazina
python -m venv .venv
```

Washa mazingira:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Boresha pip ili kuepuka matatizo ya utekelezaji wa utegemezi:

```bash
python -m pip install --upgrade pip
```

> **Ushauri:** Ikiwa tayari una `.venv` kutoka maabara za awali, unaweza kuitumia tena. Hakikisha imewashwa kabla ya kuendelea.

---

## Dhana: Mtiririko wa Ukusanyaji

Foundry Local inahitaji mifano katika fomati ya ONNX yenye usanidi wa ONNX Runtime GenAI. Mifano mingi ya chanzo huria kwenye Hugging Face huenezwa kama uzito wa PyTorch au Safetensors, hivyo hatua ya kubadilisha inahitajika.

![Mtiririko wa ukusanyaji wa mfano maalum](../../../images/custom-model-pipeline.svg)

### Mjenzi wa Mfano Hufanya Nini?

1. **Hupakua** mfano wa chanzo kutoka Hugging Face (au kusoma kutoka njia ya ndani).
2. **Hubadilisha** uzito wa PyTorch / Safetensors kuwa fomati ya ONNX.
3. **Hunyonya ukubwa** wa mfano hadi usahihi mdogo (mfano, int4) kupunguza matumizi ya kumbukumbu na kuongeza ufanisi.
4. **Hutoa** usanidi wa ONNX Runtime GenAI (`genai_config.json`), kiolezo cha mazungumzo (`chat_template.jinja`), na faili zote za tokeniser ili Foundry Local iweze kupakia na kuhudumia mfano.

### ONNX Runtime GenAI Model Builder dhidi ya Microsoft Olive

Unaweza kukutana na marejeleo ya **Microsoft Olive** kama chombo mbadala cha kuboresha mfano. Vyete vyote vinaweza kutoa mifano ya ONNX, lakini vinahudumia madhumuni tofauti na vina upungufu tofauti:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Kifurushi** | `onnxruntime-genai` | `olive-ai` |
| **Madhumuni ya msingi** | Badilisha na nyonya mifano ya AI ya uzalishaji kwa ONNX Runtime GenAI | Mfumo wa kuboresha modeli mwishoni-mwisho unaounga mkono mabweni mengi na malengo ya vifaa |
| **Urahisi wa matumizi** | Amri moja — uongofu wa hatua moja + nyonyaji | Mchakato wa kazi unaoelekezwa — mitiririko ya vitu vinavyoweza kusanidiwa na YAML/JSON |
| **Fomati ya towe** | Fomati ya ONNX Runtime GenAI (tayari kwa Foundry Local) | ONNX ya jumla, ONNX Runtime GenAI, au fomati nyingine kulingana na mchakato |
| **Malengo ya vifaa** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, na zaidi |
| **Chaguzi za nyonyaji** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, pamoja na uboreshaji wa grafu, usahihishaji kwa tabaka |
| **Eneo la mfano** | Mifano ya AI ya uzalishaji (LLMs, SLMs) | Mfano wowote unaoweza kubadilishwa kuwa ONNX (maono, NLP, sauti, multimodal) |
| **Bora kwa** | Ukusanyaji wa haraka wa mfano mmoja kwa utambuzi wa ndani | Mifumo ya uzalishaji inayohitaji udhibiti wa kina wa uboreshaji |
| **Footprint ya utegemezi** | Wastani (PyTorch, Transformers, ONNX Runtime) | Kubwa zaidi (huongeza mfumo wa Olive, vitu vya ziada kulingana na mchakato) |
| **Uunganisho wa Foundry Local** | Moja kwa moja — towe ni sambamba mara moja | Inahitaji bendera `--use_ort_genai` na usanidi wa ziada |

> **Kwa nini maabara hii inatumia Mjenzi wa Mfano:** Kwa kazi ya ukusanyaji wa mfano mmoja wa Hugging Face na kujiandikisha nayo katika Foundry Local, Mjenzi wa Mfano ni njia rahisi na ya kuaminika zaidi. Hutengeneza fomati halisi ya towe ambayo Foundry Local inaihitaji katika amri moja. Ikiwa baadaye unahitaji sifa za hali ya juu za uboreshaji — kama upunguzaji unaojali usahihi, upasuaji wa grafu, au usahihishaji wa mara nyingi — Olive ni chaguo kubwa la kuchunguza. Angalia [nyaraka za Microsoft Olive](https://microsoft.github.io/Olive/) kwa maelezo zaidi.

---

## Mazoezi ya Maabara

### Zoeezi 1: Sakinisha ONNX Runtime GenAI Model Builder

Sakinisha kifurushi cha ONNX Runtime GenAI, ambacho kinajumuisha chombo cha mjenzi wa mfano:

```bash
pip install onnxruntime-genai
```

Thibitisha usakinishaji kwa kuangalia kama mjenzi wa mfano upo:

```bash
python -m onnxruntime_genai.models.builder --help
```

Unapaswa kuona msaada unaoorodhesha vigezo kama `-m` (jina la mfano), `-o` (njia ya towe), `-p` (usahihi), na `-e` (mtoa utekelezaji).

> **Kumbuka:** Mjenzi wa mfano unategemea PyTorch, Transformers, na vifurushi vingine kadhaa. Usakinishaji unaweza kuchukua dakika kadhaa.

---

### Zoeezi 2: Kusanya Qwen3-0.6B kwa CPU

Endesha amri ifuatayo kupakua mfano wa Qwen3-0.6B kutoka Hugging Face na kuusanya kwa utambuzi wa CPU wenye nyonyaji int4:

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

#### Kila Kigezo Kinatumikaje

| Kigezo | Kusudi | Thamani Iliyotumika |
|-----------|---------|------------|
| `-m` | Kitambulisho cha mfano wa Hugging Face au njia ya saraka ya ndani | `Qwen/Qwen3-0.6B` |
| `-o` | Saraka ambapo mfano wa ONNX uliokusanywa utawekwa | `models/qwen3` |
| `-p` | Usahihi wa nyonyaji ulio tumika wakati wa ukusanyaji | `int4` |
| `-e` | Mtoa utekelezaji wa ONNX Runtime (lengo la vifaa) | `cpu` |
| `--extra_options hf_token=false` | Hupita uthibitishaji wa Hugging Face (sawa kwa mifano ya umma) | `hf_token=false` |

> **Inachukua muda gani?** Muda wa ukusanyaji hutegemea vifaa vyako na ukubwa wa mfano. Kwa Qwen3-0.6B na nyonyaji int4 kwenye CPU ya kisasa, tarajia dakika 5 hadi 15. Mifano mikubwa huchukua muda zaidi kulingana na ukubwa.

Mara amri inapokamilika unapaswa kuona saraka ya `models/qwen3` yenye faili zilizokusanywa. Thibitisha towe:

```bash
ls models/qwen3
```

Unapaswa kuona faili zifuatazo:
- `model.onnx` na `model.onnx.data` — uzito wa mfano uliokusanywa
- `genai_config.json` — usanidi wa ONNX Runtime GenAI
- `chat_template.jinja` — kiolezo cha mazungumzo cha mfano (kimeundwa moja kwa moja)
- `tokenizer.json`, `tokenizer_config.json` — faili za tokeniser
- Faili zingine za msamiati na usanidi

---

### Zoeezi 3: Kusanya kwa GPU (Hiari)

Ikiwa una GPU ya NVIDIA yenye msaada wa CUDA, unaweza kusanya toleo lililo boreshwa kwa GPU kwa utambuzi wa haraka zaidi:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Kumbuka:** Ukusanyaji kwa GPU unahitaji `onnxruntime-gpu` na usakinishaji wa CUDA unaofanya kazi. Ikiwa hizi hazipo, mjenzi wa mfano utaonyesha kosa. Unaweza kuruka zoezi hili na kuendelea na toleo la CPU.

#### Marejeleo ya Ukusanyaji Kulingana na Vifaa

| Lengo | Mtoa Utekelezaji (`-e`) | Usahihi Unaopendekezwa (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` au `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` au `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Kubadilika kwa Usahihi

| Usahihi | Ukubwa | Kasi | Ubora |
|-----------|------|-------|---------|
| `fp32` | Kubwa zaidi | Polepole zaidi | Usahihi mkubwa |
| `fp16` | Kubwa | Haraka (GPU) | Usahihi mzuri sana |
| `int8` | Ndogo | Haraka | Kupoteza usahihi kidogo |
| `int4` | Ndogo zaidi | Haraka sana | Kupoteza usahihi wastani |

Kwa maendeleo mengi ya ndani, `int4` kwenye CPU hutoa usawa bora kati ya kasi na matumizi ya rasilimali. Kwa towe la ubora wa uzalishaji, `fp16` kwenye GPU ya CUDA inapendekezwa.

---

### Zoeezi 4: Tengeneza Usanidi wa Kiolezo cha Mazungumzo

Mjenzi wa mfano hutengeneza moja kwa moja faili ya `chat_template.jinja` na `genai_config.json` katika saraka ya towe. Hata hivyo, Foundry Local pia inahitaji faili ya `inference_model.json` kuelewa jinsi ya kuweka maelekezo (prompts) kwa mfano wako. Faili hii inaelezea jina la mfano na kiolezo cha maelekezo kinachozunguka ujumbe wa mtumiaji na tokeni maalum sahihi.

#### Hatua 1: Chunguza Tokeo la Mfano Uliokusanywa

Orodhesha yaliyomo katika saraka ya mfano uliokusanywa:

```bash
ls models/qwen3
```

Unapaswa kuona faili kama:
- `model.onnx` na `model.onnx.data` — uzito wa mfano uliokusanywa
- `genai_config.json` — usanidi wa ONNX Runtime GenAI (uliundwa moja kwa moja)
- `chat_template.jinja` — kiolezo cha mazungumzo cha mfano (uliundwa moja kwa moja)
- `tokenizer.json`, `tokenizer_config.json` — faili za tokeniser
- Faili zingine za usanidi na msamiati

#### Hatua 2: Tengeneza Faili ya inference_model.json

Faili ya `inference_model.json` huaeleza Foundry Local jinsi ya kuweka maelekezo. Tengeneza script ya Python iitwayo `generate_chat_template.py` **katika mizizi ya hifadhi ya mradi** (saraka ile ile inayotoa `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Jenga mazungumzo ya chini kabisa ili kupata kiolezo cha mazungumzo
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

# Jenga muundo wa inference_model.json
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

Endesha script kutoka mizizi ya hifadhi:

```bash
python generate_chat_template.py
```

> **Kumbuka:** Kifurushi cha `transformers` tayari kilikuwa kimesakinishwa kama tegemezi la `onnxruntime-genai`. Ikiwa utaona `ImportError`, endesha `pip install transformers` kwanza.

Script hutengeneza faili ya `inference_model.json` ndani ya saraka ya `models/qwen3`. Faili hii huaeleza Foundry Local jinsi ya kuweka maelekezo ya kuzunguka maelezo ya mtumiaji kwa tokeni maalum sahihi za Qwen3.

> **Muhimu:** Sehemu ya `"Name"` katika `inference_model.json` (iliyo seti kwa `qwen3-0.6b` katika script hii) ni **jina la mfano** utakayotumia katika amri na API zote zitakazofuata. Ukibadilisha jina hili, sasisha jina la mfano katika Mazoezi 6–10 mtawalia.

#### Hatua 3: Thibitisha Usanidi

Fungua `models/qwen3/inference_model.json` na uthibitishe ina sehemu ya `Name` na kitu cha `PromptTemplate` chenye funguo `assistant` na `prompt`. Kiolezo cha maelekezo kinapaswa kujumuisha tokeni maalum kama `<|im_start|>` na `<|im_end|>` (tokeni halisi hutegemea kiolezo cha mazungumzo cha mfano).

> **Mbali ya kiutu:** Ikiwa haupaswi kuendesha script, unaweza kutengeneza faili kwa mikono. Sharti muhimu ni kuwa sehemu ya `prompt` ina vizuri kiolezo kamili cha mazungumzo cha mfano pamoja na `{Content}` kama nafasi ya ujumbe wa mtumiaji.

---

### Zoeezi 5: Thibitisha Muundo wa Saraka ya Mfano
Mjenzi wa modeli huweka faili zote zilizokusanywa moja kwa moja katika saraka ya pato uliyobainisha. Thibitisha kwamba muundo wa mwisho unaonekana sawa:

```bash
ls models/qwen3
```

Saraka hiyo inapaswa kuwa na faili zifuatazo:

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

> **Kumbuka:** Tofauti na baadhi ya zana nyingine za kukusanya, mjenzi wa modeli haufanyi saraka zilizojengwa chini ndani. Faili zote ziko moja kwa moja kwenye folda ya pato, ambayo ndiyo hasa Foundry Local inavyotarajia.

---

### Zoefzoe 6: Ongeza Modeli kwenye Cache ya Foundry Local

Mweleze Foundry Local wapi ya kupata modeli yako iliyokusanywa kwa kuongeza saraka kwenye cache yake:

```bash
foundry cache cd models/qwen3
```

Thibitisha kuwa modeli inaonekana kwenye cache:

```bash
foundry cache ls
```

Unapaswa kuona modeli yako ya kibinafsi ikielezwa pamoja na modeli zilizohifadhiwa hapo awali kwenye cache (kama `phi-3.5-mini` au `phi-4-mini`).

---

### Zoefzoe 7: Endesha Modeli ya Kibinafsi Kwa CLI

Anza kikao cha mazungumzo hai na modeli yako mpya iliyokusanywa (jina la `qwen3-0.6b` linatokana na sehemu ya `Name` uliyoweka katika faili `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Bendera `--verbose` inaonyesha taarifa za ziada za uchunguzi, ambazo ni msaada wakati wa kujaribu modeli ya kibinafsi kwa mara ya kwanza. Ikiwa modeli itapakia kwa mafanikio utaona mwaliko wa mazungumzo hai. Jaribu ujumbe kadhaa:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Andika `exit` au bonyeza `Ctrl+C` kumaliza kikao.

> **Kutatua matatizo:** Ikiwa modeli haitapakia, angalia yafuatayo:
> - Faili `genai_config.json` imelipiwa na mjenzi wa modeli.
> - Faili `inference_model.json` ipo na ni JSON halali.
> - Faili za modeli ya ONNX ziko kwenye saraka sahihi.
> - Una RAM ya kutosha (Qwen3-0.6B int4 inahitaji takriban 1 GB).
> - Qwen3 ni modeli ya kufikiri inayotoa vitambulisho vya `<think>`. Ikiwa unaona `<think>...</think>` ikitangulia majibu, ni tabia ya kawaida. Kiolezo cha mwaliko katika `inference_model.json` kinaweza kubadilishwa ili kuzima pato la fikira.

---

### Zoefzoe 8: Uliza Modeli ya Kibinafsi Kupitia REST API

Ikiwa uliamua kutoka kikao cha mazungumzo cha Zoefzoe 7, modeli inaweza isipochaguliwa tena. Anza huduma ya Foundry Local na pakia modeli kwanza:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Angalia ni bandari gani huduma inaendesha:

```bash
foundry service status
```

Kisha tuma ombi (badilisha `5273` na nambari ya bandari halisi kama ni tofauti):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Kumbuka Windows:** Amri ya `curl` hapo juu inatumia sintaksia ya bash. Kwenye Windows, tumia cmdlet ya PowerShell `Invoke-RestMethod` kama ifuatavyo.

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

### Zoefzoe 9: Tumia Modeli ya Kibinafsi na SDK ya OpenAI

Unaweza kuunganisha na modeli yako ya kibinafsi ukitumia nambari ile ile ya SDK ya OpenAI uliyotumia kwa modeli zilizojengewa ndani (angaliza [Sehemu ya 3](part3-sdk-and-apis.md)). Tofauti pekee ni jina la modeli.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local hahakiki funguo za API
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
  apiKey: "foundry-local", // Foundry Local haithibitishi funguo za API
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

> **Jambo muhimu:** Kwa sababu Foundry Local ina API inayolingana na OpenAI, nambari yoyote inayoendana na modeli za ndani pia hufanya kazi na modeli zako za kibinafsi. Unahitaji kubadilisha tu parameter ya `model`.

---

### Zoefzoe 10: Jaribu Modeli ya Kibinafsi na SDK ya Foundry Local

Katika maabara za awali ulitumia SDK ya Foundry Local kuanzisha huduma, kugundua endpoint, na kusimamia modeli moja kwa moja. Unaweza kufuata muundo huo huo kwa modeli yako iliyokusanywa mwenyewe. SDK husimamia anza huduma na ugunduzi wa endpoint, hivyo huna haja ya kuweka `localhost:5273` kwa mkono katika nambari yako.

> **Kumbuka:** Hakikisha SDK ya Foundry Local imewekwa kabla ya kuendesha mifano hii:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Ongeza vifurushi vya NuGet `Microsoft.AI.Foundry.Local` na `OpenAI`
>
> Hifadhi kila faili la script **katika mzizi wa hazina** (saraka ile ile na folda yako ya `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Hatua ya 1: Anzisha huduma ya Foundry Local na pakiwa modeli maalum
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Hatua ya 2: Angalia cache kwa ajili ya modeli maalum
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Hatua ya 3: Pakia modeli kwenye kumbukumbu
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Hatua ya 4: Tengeneza mteja wa OpenAI ukitumia kiungo kilichogunduliwa na SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Hatua ya 5: Tuma ombi la kukamilisha mazungumzo la mtiririko
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

Endesha:

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

// Hatua ya 1: Anzisha huduma ya Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Hatua ya 2: Pata mfano maalum kutoka kwenye katalogi
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Hatua ya 3: Pakia mfano ndani ya kumbukumbu
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Hatua ya 4: Unda mteja wa OpenAI ukitumia kiungo kilichogunduliwa na SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Hatua ya 5: Tuma ombi la mazungumzo ya mtiririko ya moja kwa moja
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

Endesha:

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

> **Jambo muhimu:** SDK ya Foundry Local hugundua endpoint kwa njia ya kiotomatiki, hivyo huhitaji kuweka nambari ya bandari kwa mkono. Hii ndiyo njia inayopendekezwa kwa matumizi ya maombi ya uzalishaji. Modeli yako iliyokusanywa inafanya kazi sawa na modeli za katalogi zilizojengewa ndani kupitia SDK.

---

## Kuchagua Modeli ya Kukusanya

Qwen3-0.6B imetumika kama mfano wa rejea katika maabara hii kwa sababu ni ndogo, inakukusanya haraka, na inapatikana bure chini ya leseni ya Apache 2.0. Hata hivyo, unaweza kukusanya modeli nyingi nyingine. Hapa kuna mapendekezo:

| Modeli | Hugging Face ID | Vigezo | Leseni | Maelezo |
|-------|-----------------|------------|---------|---------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Ndogo sana, kukusanya haraka, nzuri kwa majaribio |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Ubora bora, bado inakusanywa haraka |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Ubora thabiti, inahitaji RAM zaidi |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Inahitaji kukubali leseni kwenye Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Ubora wa juu, upakuaji mkubwa na muda mrefu wa kukusanya |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Tayari iko katika katalogi ya Foundry Local (inayosaidia kulinganisha) |

> **Kumbusho la leseni:** Daima angalia leseni ya modeli kwenye Hugging Face kabla ya kuitumia. Baadhi ya modeli (kama Llama) zinahitaji kukubali makubaliano ya leseni na uthibitisho kwa kutumia `huggingface-cli login` kabla ya kupakua.

---

## Dhana: Lini Utumie Modeli za Kibinafsi

| Hali | Kwa Nini Kukusanya Yako? |
|----------|---------------------------|
| **Modeli unayohitaji haipo kwenye katalogi** | Katalogi ya Foundry Local imeandaliwa. Ikiwa modeli unayotaka haipo, jisukumzie kuikusanya mwenyewe. |
| **Modeli zilizo elekezwa vyema (fine-tuned)** | Ikiwa umeelekeza modeli kwa data maalum ya kikoa, unahitaji kukusanya uzito wako mwenyewe. |
| **Mahitaji maalum ya kuquantise** | Unaweza kutaka usahihi au mkakati wa kuquantise tofauti na chaguo msimbo wa katalogi. |
| **Toleo jipya la modeli** | Wakati modeli mpya inapotolewa Hugging Face, huenda haijapokewa bado kwenye katalogi ya Foundry Local. Kukusanya yenyewe kunakupa upatikanaji wa papo hapo. |
| **Utafiti na majaribio** | Kujaribu miundo tofauti ya modeli, ukubwa, au usanidi hapo eneo kabla ya kuamua kwa matumizi ya uzalishaji. |

---

## Muhtasari

Katika maabara hii umejifunza jinsi ya:

| Hatua | Ulifanya Nini |
|------|-------------|
| 1 | Kusakinisha mjenzi wa modeli wa ONNX Runtime GenAI |
| 2 | Kukusanya `Qwen/Qwen3-0.6B` kutoka Hugging Face kuwa modeli iliyo optimized katika muundo wa ONNX |
| 3 | Kuunda faili la usanidi la chat-template `inference_model.json` |
| 4 | Kuweka modeli iliyokusanywa kwenye cache ya Foundry Local |
| 5 | Kuendesha mazungumzo hai na modeli ya kibinafsi kupitia CLI |
| 6 | Kuuliza modeli kupitia API ya REST inayoendana na OpenAI |
| 7 | Kuungana kutoka Python, JavaScript, na C# ukitumia SDK ya OpenAI |
| 8 | Kupima modeli ya kibinafsi kutoka mwanzo hadi mwisho kwa kutumia SDK ya Foundry Local |

Jambo kuu ni kwamba **moduli yoyote inayotegemea transformeri inaweza kuendeshwa kupitia Foundry Local** mara tu imekusanywa kwa muundo wa ONNX. API inayoendana na OpenAI inamaanisha nambari yako yote ya programu inafanya kazi bila mabadiliko; unahitaji tu kubadilisha jina la modeli.

---

## Muhimu Kutambua

| Dhana | Maelezo |
|---------|---------|
| Mjenzi wa Modeli wa ONNX Runtime GenAI | Hubadilisha modeli za Hugging Face kuwa muundo wa ONNX pamoja na kuquantise kwa amri moja |
| Muundo wa ONNX | Foundry Local inahitaji modeli za ONNX zilizo na usanidi wa ONNX Runtime GenAI |
| Violezo vya mazungumzo | Faili `inference_model.json` huelezea Foundry Local jinsi ya kuunda mihadhara kwa modeli fulani |
| Malengo ya vifaa | Kikusanya kwa ajili ya CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), au WebGPU kulingana na vifaa vyako |
| Kuquantise | Usahihi wa chini (int4) unapunguza ukubwa na kuongeza kasi kwa gharama ya usahihi fulani; fp16 hubakisha ubora wa juu kwenye GPU |
| Ulinganifu wa API | Modeli za kibinafsi hutumia API ile ile inayolingana na OpenAI kama modeli za ndani |
| SDK ya Foundry Local | SDK husimamia kuanzisha huduma, kugundua endpoint, na kupakia modeli kiotomatiki kwa katalogi na modeli za kibinafsi |

---

## Kusoma Zaidi

| Rasilimali | Kiungo |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Mwongozo wa modeli za kibinafsi wa Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Familia ya modeli ya Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Nyaraka za Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Hatua Zifuatazo

Endelea kwa [Sehemu 11: Kuitisha Zana na Modeli za Ndani](part11-tool-calling.md) kujifunza jinsi ya kuwezesha modeli zako za ndani kuita kazi za nje.

[← Sehemu 9: Uandishi wa Sauti wa Whisper](part9-whisper-voice-transcription.md) | [Sehemu 11: Kuitisha Zana →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Kivunja Hati**:  
Hati hii imefasiriwa kwa kutumia huduma ya tafsiri ya AI [Co-op Translator](https://github.com/Azure/co-op-translator). Ingawa tunajitahidi kuwa sahihi, tafadhali fahamu kwamba tafsiri za moja kwa moja zinaweza kuwa na makosa au ukosefu wa usahihi. Hati ya asili katika lugha yake ya asili inapaswa kuchukuliwa kama chanzo cha mamlaka. Kwa taarifa muhimu, tafsiri ya mtaalamu wa binadamu inashauriwa. Hatubeba dhamana kwa maelewano mabaya au tafsiri zisizo sahihi zinazotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->