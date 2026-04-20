![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Osa 10: Mukautettujen tai Hugging Face -mallien käyttäminen Foundry Localin kanssa

> **Tavoite:** Käännä Hugging Face -malli Foundry Localin vaatimaan optimoituun ONNX-muotoon, määritä se chat-mallipohjalla, lisää se paikalliseen välimuistiin ja suorita johtopäätöksiä sitä vastaan käyttämällä komentoriviä, REST-rajapintaa ja OpenAI SDK:ta.

## Yleiskatsaus

Foundry Local toimitetaan valikoitujen esikäännettyjen mallien luettelon kanssa, mutta et ole rajoittunut siihen listaan. Mikä tahansa Hugging Facen [https://huggingface.co/](https://huggingface.co/) kautta saatavilla oleva (tai paikallisesti PyTorch- tai Safetensors-muodossa tallennettu) transformeri-pohjainen kielimalli voidaan kääntää optimoiduksi ONNX-malliksi ja tarjota Foundry Localin kautta.

Käännösputki käyttää **ONNX Runtime GenAI Model Builder** -työkalua, joka on komentorivityökalu ja sisältyy `onnxruntime-genai`-pakettiin. Mallirakentaja hoitaa raskaan työn: lähdepainojen latauksen, niiden muuntamisen ONNX-muotoon, kvantisoinnin soveltamisen (int4, fp16, bf16) ja konfiguraatiotiedostojen (mukaan lukien chat-mallipohja ja tokenisoija) tuottamisen, joita Foundry Local odottaa.

Tässä harjoituksessa käännät **Qwen/Qwen3-0.6B** Hugging Facesta, rekisteröit sen Foundry Localiin ja keskustelet kokonaan omalla laitteellasi.

---

## Oppimistavoitteet

Tämän harjoituksen lopussa osaat:

- Selittää, miksi mukautettu mallin kääntäminen on hyödyllistä ja milloin sitä saatetaan tarvita
- Asentaa ONNX Runtime GenAI mallirakentajan
- Kääntää Hugging Face -mallin optimoituun ONNX-muotoon yhdellä komennolla
- Ymmärtää tärkeät käännösparametrit (suoritusympäristö, tarkkuus)
- Luoda `inference_model.json` chat-mallipohjan konfiguraatiotiedoston
- Lisätä käännetty malli Foundry Localin välimuistiin
- Suorittaa johtopäätöksiä mukautettua mallia vasten käyttämällä CLI:tä, REST-rajapintaa ja OpenAI SDK:ta

---

## Vaatimukset

| Vaatimus | Tiedot |
|-------------|---------|
| **Foundry Local CLI** | Asennettuna ja mukana `PATH`-ympäristömuuttujassa ([Osa 1](part1-getting-started.md)) |
| **Python 3.10+** | Tarvitaan ONNX Runtime GenAI mallirakentajalle |
| **pip** | Python-pakettien hallintaohjelma |
| **Levytilaa** | Vähintään 5 Gt vapaata lähde- ja käännettyjen mallien tiedostoille |
| **Hugging Face -tili** | Joihinkin malleihin täytyy hyväksyä lisenssi ennen latausta. Qwen3-0.6B käyttää Apache 2.0 -lisenssiä ja on vapaasti saatavilla. |

---

## Ympäristön asennus

Mallin kääntäminen tarvitsee useita suuria Python-paketteja (PyTorch, ONNX Runtime GenAI, Transformers). Luo erillinen virtuaaliympäristö, jotta nämä eivät häiritse järjestelmä-Pythoniasi tai muita projekteja.

```bash
# Varaston juuresta
python -m venv .venv
```

Aktivoi ympäristö:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Päivitä pip riippuvuuksien ratkaisun ongelmien välttämiseksi:

```bash
python -m pip install --upgrade pip
```

> **Vinkki:** Jos sinulla on jo aiemmista harjoituksista `.venv`, voit käyttää sitä uudelleen. Varmista vain, että se on aktivoitu ennen jatkamista.

---

## Käsite: Käännösputki

Foundry Local vaatii mallit ONNX-muodossa ONNX Runtime GenAI -konfiguraatiolla. Useimmat avoimen lähdekoodin mallit Hugging Facessa ovat jaettuina PyTorch- tai Safetensors-painoina, joten tarvitaan muuntovaihe.

![Mukautetun mallin käännösputki](../../../images/custom-model-pipeline.svg)

### Mitä Mallirakentaja Tekee?

1. **Lataa** lähdemalli Hugging Facesta (tai lukee sen paikallisesta sijainnista).
2. **Muuttaa** PyTorch- tai Safetensors-painot ONNX-muotoon.
3. **Kvantisoi** mallin pienempään tarkkuuteen (esim. int4) vähentääkseen muistinkulutusta ja parantaakseen läpimenoaikaa.
4. **Tuottaa** ONNX Runtime GenAI -konfiguraation (`genai_config.json`), chat-mallipohjan (`chat_template.jinja`) ja kaikki tokenisoijatiedostot, jotta Foundry Local voi ladata ja tarjota mallin.

### ONNX Runtime GenAI Model Builder ja Microsoft Olive

Saatat törmätä viitteisiin **Microsoft Olive** -työkalusta, joka on toinen vaihtoehto mallin optimointiin. Molemmat työkalut voivat tuottaa ONNX-malleja, mutta ne palvelevat eri tarkoituksia ja tukevat erilaisia ominaisuuksia:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Paketti** | `onnxruntime-genai` | `olive-ai` |
| **Pääasiallinen tarkoitus** | Muuntaa ja kvantisoida generatiivisia AI-malleja ONNX Runtime GenAI inferenssiä varten | Kokonaisvaltainen mallin optimointikehys monille backend- ja laitteistokohteille |
| **Helppous** | Yksi komento — yhdellä askelmalla muuntaminen ja kvantisointi | Työnkulkuun perustuva — konfiguroitavat monivaiheiset YAML/JSON-putket |
| **Tulostusmuoto** | ONNX Runtime GenAI -muoto (valmis Foundry Localille) | Yleinen ONNX, ONNX Runtime GenAI tai muut workflow-riippuvaiset muodot |
| **Laitteistotuki** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN ja muita |
| **Kvantisointivaihtoehdot** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, sekä graafin optimoinnit, kerroskohtainen hienosäätö |
| **Mallilajit** | Generatiiviset AI-mallit (LLM:t, SLM:t) | Mikä tahansa ONNX:ksi muunnettavissa oleva malli (näkö, NLP, ääni, multimodaalinen) |
| **Parhaiten soveltuva** | Nopeaan yhden mallin kääntämiseen paikalliseen inferenssiin | Tuotantoputket, jotka tarvitsevat hienojakoista optimointia |
| **Riippuvuuksien jalanjälki** | Kohtalainen (PyTorch, Transformers, ONNX Runtime) | Suurempi (sisältää Olive-kehyksen, valinnaiset lisät workflow’n mukaan) |
| **Foundry Local -integraatio** | Suora — tuloste on heti yhteensopiva | Tarvitsee `--use_ort_genai`-lipun ja lisäkonfiguraation |

> **Miksi tässä harjoituksessa käytetään Mallirakentajaa:** Yhden Hugging Face -mallin kääntäminen ja Foundry Localiin rekisteröinti on helpointa ja luotettavinta tehdä Mallirakentajalla. Se tuottaa tarkasti juuri oikean lähdösmuodon yhdellä komennolla. Jos tarvitset myöhemmin edistyneempiä optimointiominaisuuksia kuten tarkkuustietoista kvantisointia, graafileikkauksia tai monivaiheista hienosäätöä, Olive on hyvä työkalu. Katso lisää [Microsoft Oliven dokumentaatiosta](https://microsoft.github.io/Olive/).

---

## Harjoitukset

### Harjoitus 1: Asenna ONNX Runtime GenAI Model Builder

Asenna ONNX Runtime GenAI -paketti, joka sisältää mallirakentajan työkalun:

```bash
pip install onnxruntime-genai
```

Tarkista asennus varmistaaksesi, että mallirakentaja on käytettävissä:

```bash
python -m onnxruntime_genai.models.builder --help
```

Näet avustustulosteessa parametrit kuten `-m` (mallin nimi), `-o` (tulostuskansio), `-p` (tarkkuus) ja `-e` (suoritusympäristö).

> **Huom:** Mallirakentaja riippuu PyTorchista, Transformersista ja useista muista paketeista. Asennus voi kestää muutaman minuutin.

---

### Harjoitus 2: Käännä Qwen3-0.6B CPU:lle

Suorita seuraava komento ladataksesi Qwen3-0.6B -mallin Hugging Facesta ja käännä se CPU-inferenssille int4-kvantisoinnilla:

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

#### Parametrien selitykset

| Parametri | Tarkoitus | Käytetty arvo |
|-----------|---------|------------|
| `-m` | Hugging Face -mallin tunnus tai paikallinen hakemisto | `Qwen/Qwen3-0.6B` |
| `-o` | Hakemisto, johon käännetty ONNX-malli tallennetaan | `models/qwen3` |
| `-p` | Kvantisointitarkkuus käännöksen aikana | `int4` |
| `-e` | ONNX Runtime -suoritusympäristö (kohdealusta) | `cpu` |
| `--extra_options hf_token=false` | Ohittaa Hugging Face -autentikoinnin (sopii julkisille malleille) | `hf_token=false` |

> **Kuinka kauan tämä kestää?** Käännösaika riippuu laitteistosta ja mallin koosta. Qwen3-0.6B:n int4-kvantisointi nykyaikaisella CPU:lla vie noin 5–15 minuuttia. Suuremmat mallit vievät suhteessa enemmän aikaa.

Kun komento on suoritettu, sinun pitäisi nähdä `models/qwen3` -hakemisto käännettyjen mallin tiedostojen kanssa. Tarkista tuloste:

```bash
ls models/qwen3
```

Näet muun muassa seuraavat tiedostot:
- `model.onnx` ja `model.onnx.data` — käännetyt mallipainot
- `genai_config.json` — ONNX Runtime GenAI -konfiguraatio
- `chat_template.jinja` — mallin chat-mallipohja (automaattisesti luotu)
- `tokenizer.json`, `tokenizer_config.json` — tokenisoijatiedostot
- Muita sanasto- ja konfiguraatiotiedostoja

---

### Harjoitus 3: Käännä GPU:lle (Valinnainen)

Jos sinulla on NVIDIA GPU CUDA-tuella, voit kääntää GPU-optimoidun version nopeampaan inferenssiin:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Huom:** GPU-käännös vaatii `onnxruntime-gpu`-paketin ja toimivan CUDA-asennuksen. Jos niitä ei ole, mallirakentaja antaa virheilmoituksen. Voit ohittaa tämän harjoituksen ja jatkaa CPU-version kanssa.

#### Laitteistokohtaiset käännösparametrit

| Kohde | Suoritusympäristö (`-e`) | Suositeltu tarkkuus (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` tai `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` tai `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Tarkkuuden kompromissit

| Tarkkuus | Koko | Nopeus | Laatu |
|-----------|------|-------|---------|
| `fp32` | Suurin | Hitain | Paras tarkkuus |
| `fp16` | Suuri | Nopea (GPU) | Erittäin hyvä tarkkuus |
| `int8` | Pieni | Nopea | Pieni tarkkuuden menetys |
| `int4` | Pienin | Nopein | Kohtalainen tarkkuuden menetys |

Useimpiin paikalliseen kehitykseen `int4` CPU:lla tarjoaa parhaan nopeuden ja resurssitehokkuuden tasapainon. Tuotantotason laatuun suositellaan `fp16`-tarkkuutta CUDA-GPU:lla.

---

### Harjoitus 4: Luo Chat-mallipohjan konfiguraatio

Mallirakentaja luo automaattisesti `chat_template.jinja` ja `genai_config.json` -tiedostot tulostuskansioon. Kuitenkin Foundry Local tarvitsee myös `inference_model.json` -tiedoston ymmärtääkseen, miten promptit muotoillaan malliasi varten. Tämä tiedosto määrittelee mallin nimen ja kehotepohjan, joka käärii käyttäjän viestit oikeilla erikoistokenilla.

#### Vaihe 1: Tarkastele käännettyä tulosta

Listaa käännetyn mallin hakemiston sisältö:

```bash
ls models/qwen3
```

Näet tiedostoja kuten:
- `model.onnx` ja `model.onnx.data` — käännetyt mallipainot
- `genai_config.json` — ONNX Runtime GenAI -konfiguraatio (automaattisesti luotu)
- `chat_template.jinja` — mallin chat-mallipohja (automaattisesti luotu)
- `tokenizer.json`, `tokenizer_config.json` — tokenisoijatiedostot
- Useita muita konfiguraatio- ja sanasto-tiedostoja

#### Vaihe 2: Luo inference_model.json -tiedosto

`inference_model.json` ohjaa Foundry Localia muotoilemaan promptit oikein. Luo Python-skripti nimeltä `generate_chat_template.py` **repositoriopuun juureen** (samaan kansioon, jossa on `models/`-kansiosi):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Luo minimaalinen keskustelu chat-mallipohjan purkamiseksi
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

# Luo inference_model.json -rakenne
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

Suorita skripti repositoriopuun juuressa:

```bash
python generate_chat_template.py
```

> **Huom:** `transformers`-paketti on jo asennettu `onnxruntime-genai`-riippuvuutena. Jos saat tuontivirheen (`ImportError`), asenna se komennolla `pip install transformers` ensin.

Skripti tuottaa `inference_model.json` -tiedoston `models/qwen3` -kansioon. Tämä tiedosto kertoo Foundry Localille, miten käyttäjän syöte kääritään oikeilla erikoistokeneilla Qwen3-mallia varten.

> **Tärkeää:** `inference_model.json` -tiedoston `"Name"`-kenttä (tässä skriptissä asetettu arvoon `qwen3-0.6b`) on **malleille käyttämäsi aliaksen nimi** kaikissa myöhemmissä komennoissa ja API-kutsuissa. Jos vaihdat tätä nimeä, päivitä myös harjoitusten 6–10 mallin nimi vastaavasti.

#### Vaihe 3: Tarkista konfiguraatio

Avaa `models/qwen3/inference_model.json` ja varmista, että siellä on `Name`-kenttä ja `PromptTemplate`-objekti, jossa on `assistant`- ja `prompt`-avaimet. Kehotepohjan tulee sisältää erikoistokenit, kuten `<|im_start|>` ja `<|im_end|>` (tarkat tokenit riippuvat mallin chat-mallipohjasta).

> **Manuaalinen vaihtoehto:** Jos et halua suorittaa skriptiä, voit luoda tiedoston manuaalisesti. Tärkeää on, että `prompt`-kenttä sisältää mallin täydellisen chat-mallipohjan `{Content}`-paikalla käyttäjän viestille.

---

### Harjoitus 5: Tarkista mallihakemiston rakenne


Mallin rakentaja sijoittaa kaikki käännetyt tiedostot suoraan määrittämääsi tulostuskansioon. Varmista, että lopullinen rakenne näyttää oikealta:

```bash
ls models/qwen3
```

Kansion tulisi sisältää seuraavat tiedostot:

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

> **Huom:** Toisin kuin jotkut muut käännöstyökalut, mallin rakentaja ei luo sisäkkäisiä alikansioita. Kaikki tiedostot sijaitsevat suoraan tulostuskansiossa, mikä on juuri se, mitä Foundry Local odottaa.

---

### Harjoitus 6: Lisää malli Foundry Local -välimuistiin

Kerro Foundry Localille, mistä löydät käännetyn mallisi lisäämällä hakemisto välimuistiin:

```bash
foundry cache cd models/qwen3
```

Varmista, että malli näkyy välimuistissa:

```bash
foundry cache ls
```

Näet mukautetun mallisi listattuna yhdessä aiemmin välimuistiin tallennettujen mallien (kuten `phi-3.5-mini` tai `phi-4-mini`) kanssa.

---

### Harjoitus 7: Suorita mukautettu malli komentoriviltä

Aloita vuorovaikutteinen keskusteluistunto juuri käännetyn mallisi kanssa (aliaksena `qwen3-0.6b`, joka tulee `inference_model.json` tiedostosi `Name`-kentästä):

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose`-lippu näyttää lisädiagnostiikkaa, mikä on hyödyllistä, kun testailet mukautettua mallia ensimmäistä kertaa. Jos malli latautuu onnistuneesti, näet vuorovaikutteisen kehotteen. Kokeile muutamia viestejä:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Kirjoita `exit` tai paina `Ctrl+C` lopettaaksesi istunnon.

> **Vianmääritys:** Jos malli ei lataudu, tarkista seuraavat seikat:
> - Mallin rakentajan generoima `genai_config.json` tiedosto on olemassa.
> - `inference_model.json` tiedosto on olemassa ja kelvollinen JSON.
> - ONNX-mallitiedostot ovat oikeassa hakemistossa.
> - Saatavilla on riittävästi RAM-muistia (Qwen3-0.6B int4 tarvitsee noin 1 GB).
> - Qwen3 on päättelymalli, joka tuottaa `<think>` tageja. Jos vastauksissa näkyy `<think>...</think>`-elementtejä, tämä on normaalia käytöstä. Kehoitemalli `inference_model.json` tiedostossa voidaan säätää ajatusulostulon poistamiseksi käytöstä.

---

### Harjoitus 8: Kysy mukautetulta mallilta REST-rajapinnan kautta

Jos poistuit harjoituksessa 7 vuorovaikutteisesta istunnosta, malli ei ehkä ole enää ladattuna. Käynnistä ensin Foundry Local -palvelu ja lataa malli:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Tarkista, millä portilla palvelu toimii:

```bash
foundry service status
```

Lähetä sitten pyyntö (korvaa `5273` omalla porttisi, jos se on eri):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows-muistutus:** Yllä oleva `curl`-komento käyttää bash-syntaksia. Windowsilla käytä sen sijaan PowerShellin `Invoke-RestMethod` cmdlet-komentoa alla.

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

### Harjoitus 9: Käytä mukautettua mallia OpenAI SDK:lla

Voit yhdistää mukautettuun malliisi käyttämällä täsmälleen samaa OpenAI SDK -koodia, jota käytit sisäänrakennettujen mallien kanssa (katso [Osa 3](part3-sdk-and-apis.md)). Ainoa ero on mallin nimi.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local ei vahvista API-avaimia
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
  apiKey: "foundry-local", // Foundry Local ei validoi API-avaimia
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

> **Avainasia:** Koska Foundry Local tarjoaa OpenAI-yhteensopivan API:n, kaikki koodi, joka toimii sisäänrakennettujen mallien kanssa, toimii myös mukautettujen mallien kanssa. Sinun tarvitsee vain vaihtaa `model`-parametrin arvo.

---

### Harjoitus 10: Testaa mukautettua mallia Foundry Local SDK:lla

Aikaisemmissa harjoituksissa käytit Foundry Local SDK:ta palvelun käynnistämiseen, päätepisteen löytämiseen ja mallien hallintaan automaattisesti. Voit seurata täysin samaa mallia myös mukautetun käännetyn mallisi kanssa. SDK huolehtii palvelun käynnistyksestä ja päätepisteen etsinnästä, joten koodissasi ei tarvitse kovakoodata `localhost:5273`.

> **Huom:** Varmista, että Foundry Local SDK on asennettu ennen näiden esimerkkien suorittamista:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Lisää `Microsoft.AI.Foundry.Local` ja `OpenAI` NuGet-paketit
>
> Tallenna jokainen skriptitiedosto **varaston juurihakemistoon** (samaan kansioon kuin `models/`-hakemisto).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Vaihe 1: Käynnistä Foundry Local -palvelu ja lataa mukautettu malli
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Vaihe 2: Tarkista välimuisti mukautetun mallin osalta
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Vaihe 3: Lataa malli muistiin
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Vaihe 4: Luo OpenAI-asiakas SDK:n löytämää päätepistettä käyttäen
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Vaihe 5: Lähetä suoratoistava keskustelun täydennyspyyntö
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

Suorita se:

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

// Vaihe 1: Käynnistä Foundry Local -palvelu
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Vaihe 2: Hae mukautettu malli katalogista
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Vaihe 3: Lataa malli muistiin
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Vaihe 4: Luo OpenAI-asiakas SDK:n löytämällä päätepisteellä
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Vaihe 5: Lähetä suoratoistettu chat-täydennyspyyntö
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

Suorita se:

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

> **Avainasia:** Foundry Local SDK löytää päätepisteen dynaamisesti, joten sinun ei koskaan tarvitse kovakoodata porttinumeroa. Tämä on suositeltava käytäntö tuotantosovelluksiin. Mukautettu käännetty mallisi toimii identtisesti sisäänrakennettujen katalogimallien kanssa SDK:n kautta.

---

## Mallin valinta käännettäväksi

Qwen3-0.6B toimii tämän harjoituksen viitemallina, koska se on pieni, nopeasti käännettävä ja vapaasti saatavilla Apache 2.0 -lisenssillä. Voit kuitenkin käsitellä monia muita malleja. Tässä muutamia ehdotuksia:

| Malli | Hugging Face ID | Parametrit | Lisenssi | Huomiot |
|-------|-----------------|------------|---------|---------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Erittäin pieni, nopea käännös, hyvä testaukseen |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Parempi laatu, silti nopea käännös |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Voimakas laatu, tarvitsee enemmän RAM-muistia |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Vaatii lisenssin hyväksynnän Hugging Facessa |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Korkea laatu, suurempi lataus ja pidempi käännös |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Jo Foundry Local -katalogissa (hyödyllinen vertailuun) |

> **Lisenssimuistutus:** Tarkista aina mallin lisenssi Hugging Facessa ennen käyttöä. Jotkin mallit (kuten Llama) vaativat lisenssisopimuksen hyväksymisen ja kirjautumisen `huggingface-cli login` kautta ennen latausta.

---

## Käsitteet: Milloin käyttää mukautettuja malleja

| Tilanne | Miksi kääntää oma malli? |
|----------|--------------------------|
| **Mallia ei ole katalogissa** | Foundry Localin katalogi on kuratoitu. Jos haluamaasi mallia ei ole listattu, käännä se itse. |
| **Hienosäädetyt mallit** | Jos olet hienosäätänyt mallia domain-spesifisellä datalla, sinun täytyy kääntää omat painosi. |
| **Erityiset kvantisointivaatimukset** | Saatat haluta tarkkuus- tai kvantisointistrategian, joka poikkeaa katalogin oletuksesta. |
| **Uudemmat mallijulkaisut** | Kun uusi malli julkaistaan Hugging Facessa, sitä ei ehkä ole vielä Foundry Localin katalogissa. Kääntäminen itse antaa sinulle välittömän pääsyn. |
| **Tutkimus ja kokeilu** | Malliarkkitehtuurien, kokojen tai konfiguraatioiden kokeilu paikallisesti ennen tuotantovalintaa. |

---

## Yhteenveto

Tässä harjoituksessa opit tekemään seuraavaa:

| Vaihe | Mitä teit |
|-------|-----------|
| 1 | Asensit ONNX Runtime GenAI mallin rakentajan |
| 2 | Käänsit `Qwen/Qwen3-0.6B` Hugging Facesta optimoiduksi ONNX-malliksi |
| 3 | Loit `inference_model.json` -keskustelumallipohjatiedoston |
| 4 | Lisäsit käännetyn mallin Foundry Localin välimuistiin |
| 5 | Suoritit vuorovaikutteisen chatin mukautetulla mallilla komentoriviltä |
| 6 | Kyselit mallia OpenAI-yhteensopivan REST-rajapinnan kautta |
| 7 | Yhdistit malliin Pythonilla, JavaScripillä ja C#:lla OpenAI SDK:n avulla |
| 8 | Testasit mukautettua mallia kokonaisvaltaisesti Foundry Local SDK:lla |

Tärkein asia on, että **mikä tahansa transformer-pohjainen malli voi toimia Foundry Localissa**, kun se on käännetty ONNX-muotoon. OpenAI-yhteensopiva API tarkoittaa, että olemassa oleva sovelluskoodisi toimii ilman muutoksia; sinun tarvitsee vain vaihtaa mallin nimi.

---

## Keskeiset opit

| Käsite | Yksityiskohta |
|---------|--------------|
| ONNX Runtime GenAI Mallinrakentaja | Muuntaa Hugging Face -mallit ONNX-muotoon ja kvantisoi yhdellä komennolla |
| ONNX-muoto | Foundry Local vaatii ONNX-malleja ONNX Runtime GenAI -konfiguraation kanssa |
| Keskustelumallit | `inference_model.json` kertoo Foundry Localille, miten kehote formataan tietylle mallille |
| Laitteistotavoitteet | Käännä CPU:lle, NVIDIA GPU:lle (CUDA), DirectML:lle (Windows GPU) tai WebGPU:lle laitteistosi mukaan |
| Kvantisointi | Alhaisempi tarkkuus (int4) pienentää kokoa ja nopeuttaa toiminta, mutta madaltaa hieman tarkkuutta; fp16 säilyttää korkean laadun GPU:illa |
| API-yhteensopivuus | Mukautetut mallit käyttävät samaa OpenAI-yhteensopivaa API:a kuin sisäänrakennetut mallit |
| Foundry Local SDK | SDK hoitaa palvelun käynnistyksen, päätepisteen etsinnän ja mallin latauksen automaattisesti sekä katalogi- että mukautetuille malleille |

---

## Lisälukemista

| Resurssi | Linkki |
|----------|---------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local mukautetun mallin opas | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 malliperhe | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive-dokumentaatio | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Seuraavat askeleet

Jatka [Osa 11: Työkalukutsu paikallisten mallien kanssa](part11-tool-calling.md) oppiaksesi, miten paikalliset mallisi voivat kutsua ulkoisia toimintoja.

[← Osa 9: Whisper-äänentunnistus](part9-whisper-voice-transcription.md) | [Osa 11: Työkalukutsu →](part11-tool-calling.md)