![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# 10. rész: Egyedi vagy Hugging Face modellek használata a Foundry Local-lal

> **Cél:** Egy Hugging Face modellt lefordítani a Foundry Local által igényelt optimalizált ONNX formátumba, konfigurálni egy chat sablonnal, hozzáadni a helyi gyorsítótárhoz, és futtatni rajta lekérdezést a CLI, REST API és OpenAI SDK segítségével.

## Áttekintés

A Foundry Local egy előre összeállított modellekből álló válogatott katalógust szállít, de nem kell korlátozódnia erre a listára. Bármely, Hugging Face-en elérhető трансформер alapú nyelvi modellt (vagy helyben tárolt PyTorch / Safetensors formátumban) át lehet alakítani optimalizált ONNX modellé, és kiszolgálni a Foundry Local segítségével.

A fordítási folyamat a **ONNX Runtime GenAI Model Builder**-t használja, amely parancssori eszköz, és az `onnxruntime-genai` csomag része. A model builder elvégzi a nehéz munkát: letölti a kiindulási súlyokat, konvertálja azokat ONNX formátumba, alkalmazza a kvantálást (int4, fp16, bf16), és előállítja a konfigurációs fájlokat (beleértve a chat sablont és a tokenizert), amelyeket a Foundry Local elvár.

Ebben a gyakorlatban a Hugging Face-ről a **Qwen/Qwen3-0.6B** modellt fordítod le, regisztrálod a Foundry Localhoz, és teljes mértékben saját eszközödön folytatsz vele beszélgetést.

---

## Tanulási célok

A gyakorlat végére tudni fogod:

- Megmagyarázni, miért hasznos az egyedi modell fordítása és mikor lehet rá szükség
- Telepíteni az ONNX Runtime GenAI model buildert
- Egy Hugging Face modellt egyetlen paranccsal optimalizált ONNX formátumba fordítani
- Megérteni a fontosabb fordítási paramétereket (végrehajtó szolgáltató, pontosság)
- Létrehozni az `inference_model.json` chat-sablon konfigurációs fájlt
- Egy lefordított modellt hozzáadni a Foundry Local gyorsítótárához
- Lekérdezést futtatni az egyedi modell ellen a CLI, REST API és OpenAI SDK segítségével

---

## Előfeltételek

| Követelmény | Részletek |
|-------------|-----------|
| **Foundry Local CLI** | Telepítve és elérhető a `PATH` változódban ([1. rész](part1-getting-started.md)) |
| **Python 3.10+** | Szükséges az ONNX Runtime GenAI model builderhez |
| **pip** | Python csomagkezelő |
| **Lemezhely** | Legalább 5 GB szabad hely a forrás- és lefordított modell fájlok számára |
| **Hugging Face fiók** | Egyes modellekhez licenc elfogadása szükséges letöltés előtt. A Qwen3-0.6B Apache 2.0 licenc alatt szabadon elérhető. |

---

## Környezet beállítása

A modell fordításhoz több nagy Python csomagra van szükség (PyTorch, ONNX Runtime GenAI, Transformers). Hozz létre egy külön virtuális környezetet, hogy ezek ne zavarják az operációs rendszer Pythonját vagy más projekteket.

```bash
# A tároló gyökerétől
python -m venv .venv
```

A környezet aktiválása:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Frissítsd a pip-et, hogy elkerüld a függőségkezelési problémákat:

```bash
python -m pip install --upgrade pip
```

> **Tipp:** Ha korábbi gyakorlatokból már van `.venv` környezeted, újra felhasználhatod. Csak ügyelj arra, hogy folytatás előtt aktiválva legyen.

---

## Fogalom: A fordítási folyamat

A Foundry Local ONNX formátumú modelleket vár ONNX Runtime GenAI konfigurációval. A legtöbb nyílt forráskódú Hugging Face modell PyTorch vagy Safetensors súlyok formájában érhető el, ezért szükséges egy átalakító lépés.

![Egyedi modell fordítási folyamata](../../../images/custom-model-pipeline.svg)

### Mit csinál a Model Builder?

1. **Letölti** a forrás modellt a Hugging Face-ről (vagy egy helyi elérési útról olvassa be).
2. **Átalakítja** a PyTorch / Safetensors súlyokat ONNX formátumba.
3. **Kvantálja** a modellt kisebb pontosságra (például int4), hogy csökkentse a memóriahasználatot és növelje a teljesítményt.
4. **Generálja** az ONNX Runtime GenAI konfigurációt (`genai_config.json`), a chat sablont (`chat_template.jinja`) és az összes tokenizáló fájlt, hogy a Foundry Local betölthesse és kiszolgálhassa a modellt.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Találkozhatsz a **Microsoft Olive** eszközzel is, amely alternatív megoldás a modell optimalizálására. Mindkét eszköz tud ONNX modelleket készíteni, de különböző célokra és kompromisszumokkal.

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Csomag** | `onnxruntime-genai` | `olive-ai` |
| **Elsődleges cél** | Generatív MI modellek konvertálása és kvantálása ONNX Runtime GenAI futtatáshoz | Végponttól végpontig optimalizáló keretrendszer sok backend és hardver támogatással |
| **Használat egyszerűsége** | Egy parancs — egy lépéses konvertálás + kvantálás | Munkafolyamat-alapú — konfigurálható több lépéses pipeline YAML/JSON formátumban |
| **Kimeneti formátum** | ONNX Runtime GenAI formátum (kész a Foundry Localhoz) | Általános ONNX, ONNX Runtime GenAI vagy más formátum a munkafolyamattól függően |
| **Hardver támogatás** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN és mások |
| **Kvantálási opciók** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, továbbá gráf optimalizációk és rétegek hangolása |
| **Modelltípus** | Generatív MI modellek (LLM-ek, SLM-ek) | Bármely ONNX-re konvertálható modell (látás, NLP, hang, multimodális) |
| **Leginkább alkalmas** | Gyors egyedi modell fordítás helyi futtatáshoz | Termelési környezetekhez, finomhangolási kontrollal |
| **Függőségek** | Mérsékelt (PyTorch, Transformers, ONNX Runtime) | Nagyobb (beleértve az Olive keretrendszert és opcionális kiegészítőket) |
| **Foundry Local integráció** | Közvetlen — azonnal kompatibilis kimenet | `--use_ort_genai` kapcsoló és további konfiguráció szükséges |

> **Miért használja ez a gyakorlat a Model Buildert?** Egyetlen Hugging Face modell lefordítására és Foundry Localhoz regisztrálására a Model Builder a legegyszerűbb és legmegbízhatóbb megoldás. Egy parancsban előállítja a Foundry Local által elvárt pontos kimenetet. Ha később összetettebb optimalizálásra lesz szükség — például pontosságtudatos kvantálás, gráf szerkesztés vagy többlépcsős hangolás — az Olive hatékony eszköz lehet. Részletekért lásd a [Microsoft Olive dokumentációját](https://microsoft.github.io/Olive/).

---

## Gyakorlati feladatok

### 1. feladat: Az ONNX Runtime GenAI Model Builder telepítése

Telepítsd az ONNX Runtime GenAI csomagot, amely tartalmazza a model builder eszközt:

```bash
pip install onnxruntime-genai
```

Ellenőrizd a telepítést azzal, hogy megnézed, elérhető-e a model builder:

```bash
python -m onnxruntime_genai.models.builder --help
```

A segítségkérésnek tartalmaznia kell olyan paramétereket, mint `-m` (modell neve), `-o` (kimeneti útvonal), `-p` (pontosság) és `-e` (végrehajtó szolgáltató).

> **Megjegyzés:** A model builder függ a PyTorch-tól, Transformers-től és több más csomagtól. A telepítés eltarthat néhány percig.

---

### 2. feladat: Qwen3-0.6B CPU-ra fordítása

Futtasd az alábbi parancsot a Qwen3-0.6B modell letöltésére a Hugging Face-ről, és fordítsd le CPU-s futtatáshoz int4 kvantálással:

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

#### Mit csinál az egyes paraméter?

| Paraméter | Cél | Használt érték |
|-----------|-----|----------------|
| `-m` | A Hugging Face modell azonosítója vagy helyi mappa elérési útja | `Qwen/Qwen3-0.6B` |
| `-o` | Könyvtár, ahová a lefordított ONNX modell kerül mentésre | `models/qwen3` |
| `-p` | A fordításkor alkalmazott kvantálási pontosság | `int4` |
| `-e` | ONNX Runtime végrehajtó szolgáltató (céleszköz) | `cpu` |
| `--extra_options hf_token=false` | Kihagyja a Hugging Face hitelesítést (nyilvános modellekhez megfelelő) | `hf_token=false` |

> **Mennyi időbe telik ez?** A fordítási idő a hardvertől és a modell méretétől függ. Qwen3-0.6B int4 kvantálással egy modern CPU-n nagyjából 5-15 perc között van. Nagyobb modellek arányosan hosszabb időt vesznek igénybe.

A parancs futása után létrejön egy `models/qwen3` mappa, amely tartalmazza a lefordított modell fájlokat. Ellenőrizd a kimenetet:

```bash
ls models/qwen3
```

Látnod kell olyan fájlokat, mint:
- `model.onnx` és `model.onnx.data` — a lefordított modell súlyai
- `genai_config.json` — ONNX Runtime GenAI konfiguráció
- `chat_template.jinja` — a modell chat sablonja (automatikusan generált)
- `tokenizer.json`, `tokenizer_config.json` — tokenizáló fájlok
- Egyéb szókincs és konfigurációs fájlok

---

### 3. feladat: GPU-s fordítás (opcionális)

Ha van NVIDIA GPU-d CUDA támogatással, készíthetsz GPU-ra optimalizált változatot gyorsabb futtatáshoz:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Megjegyzés:** GPU-s fordításhoz szükség van az `onnxruntime-gpu` csomagra és működő CUDA telepítésre. Ezek nélkül a model builder hibát jelez. Ezt a gyakorlatot át is ugorhatod, és folytathatod a CPU változattal.

#### Hardver-specifikus fordítási hivatkozás

| Céleszköz | Végrehajtó szolgáltató (`-e`) | Ajánlott pontosság (`-p`) |
|-----------|-------------------------------|---------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` vagy `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` vagy `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Pontosság kompromisszumok

| Pontosság | Méret | Sebesség | Minőség |
|-----------|-------|----------|---------|
| `fp32` | Legnagyobb | Leglassabb | Legmagasabb pontosság |
| `fp16` | Nagy | Gyors (GPU) | Nagyon jó pontosság |
| `int8` | Kicsi | Gyors | Kisebb pontosságvesztés |
| `int4` | Legkisebb | Leggyorsabb | Mérsékelt pontosságvesztés |

A legtöbb helyi fejlesztéshez a CPU-n futó `int4` nyújtja a legjobb sebesség és erőforrás-használat egyensúlyt. Termelési minőségű kimenethez CUDA-s GPU-n ajánlott a `fp16`.

---

### 4. feladat: Chat sablon konfiguráció létrehozása

A model builder automatikusan előállít egy `chat_template.jinja` fájlt és egy `genai_config.json` fájlt a kimeneti mappában. A Foundry Localnak azonban szüksége van egy `inference_model.json` fájlra is, amely megmondja, hogyan formázza a promptokat a modellhez. Ebben a fájlban szerepel a modell neve és az a prompt sablon, amely megfelelő speciális tokenekkel körülveszi a felhasználói üzenetet.

#### 1. lépés: Ellenőrizd a lefordított kimenetet

Listázd a lefordított modell mappájának tartalmát:

```bash
ls models/qwen3
```

Látnod kell fájlokat, mint:
- `model.onnx` és `model.onnx.data` — a lefordított modell súlyai
- `genai_config.json` — ONNX Runtime GenAI konfiguráció (automatikusan generált)
- `chat_template.jinja` — a modell chat sablonja (automatikusan generált)
- `tokenizer.json`, `tokenizer_config.json` — tokenizáló fájlok
- Több egyéb konfigurációs és szókincsfájl

#### 2. lépés: Generáld le az inference_model.json fájlt

Az `inference_model.json` fájl megmondja a Foundry Localnak, hogyan formázza a promptokat. Hozz létre egy Python szkriptet `generate_chat_template.py` néven **a tároló gyökérkönyvtárában** (ugyanott, ahol a `models/` mappa található):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Építs egy minimális beszélgetést a chat sablon kinyeréséhez
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

# Építsd fel az inference_model.json struktúrát
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

Futtasd a szkriptet a tároló gyökérkönyvtárából:

```bash
python generate_chat_template.py
```

> **Megjegyzés:** A `transformers` csomag az `onnxruntime-genai` telepítés részeként már jelen van. Ha import hibát tapasztalsz, futtasd először a `pip install transformers` parancsot.

A szkript előállítja az `inference_model.json` fájlt a `models/qwen3` mappában. Ez a fájl megmondja a Foundry Localnak, hogyan csomagolja a felhasználói bemenetet a megfelelő speciális tokenek közé a Qwen3 modellhez.

> **Fontos:** Az `inference_model.json`-ben a `"Name"` mező (ebben a szkriptben `qwen3-0.6b`) a **modell alias**, amelyet a további parancsokban és API hívásokban használsz. Ha megváltoztatod a nevet, a 6–10. gyakorlatban szintén frissítened kell.

#### 3. lépés: Ellenőrizd a konfigurációt

Nyisd meg a `models/qwen3/inference_model.json` fájlt, és győződj meg róla, hogy tartalmaz egy `Name` mezőt, illetve egy `PromptTemplate` objektumot `assistant` és `prompt` kulcsokkal. A prompt sablonnak tartalmaznia kell speciális tokeneket, például `<|im_start|>` és `<|im_end|>` (a pontos tokenek a modell chat sablonjától függnek).

> **Kézi alternatíva:** Ha nem szeretnéd futtatni a szkriptet, kézzel is létrehozhatod a fájlt. A kulcs, hogy a `prompt` mezőben legyen a modell teljes chat sablonja, ahol a `{Content}` helyőrző jelöli a felhasználói üzenetet.

---

### 5. feladat: Ellenőrizd a modell könyvtárszerkezetét
A modellépítő az összes lefordított fájlt közvetlenül a megadott kimeneti könyvtárba helyezi. Ellenőrizze, hogy a végső struktúra helyesnek tűnik-e:

```bash
ls models/qwen3
```

A könyvtárnak a következő fájlokat kell tartalmaznia:

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

> **Megjegyzés:** Ellentétben néhány más fordítóeszközzel, a modellépítő nem hoz létre beágyazott alkönyvtárakat. Minden fájl közvetlenül a kimeneti mappában található, ami pontosan az, amit a Foundry Local elvár.

---

### 6. Gyakorlat: Add hozzá a modellt a Foundry Local gyorsítótárához

Mondd meg a Foundry Localnak, hol találja a lefordított modelledet azáltal, hogy hozzáadod a könyvtárat a gyorsítótárához:

```bash
foundry cache cd models/qwen3
```

Ellenőrizd, hogy a modell megjelenik-e a gyorsítótárban:

```bash
foundry cache ls
```

A saját modellnek a korábban gyorsítótárazott modellekkel együtt kell megjelennie (például `phi-3.5-mini` vagy `phi-4-mini`).

---

### 7. Gyakorlat: Futtasd a saját modellt a CLI segítségével

Indíts egy interaktív csevegési munkamenetet az újonnan lefordított modellel (a `qwen3-0.6b` alias a `Name` mezőből származik, amit az `inference_model.json`-ban állítottál be):

```bash
foundry model run qwen3-0.6b --verbose
```

A `--verbose` kapcsoló további diagnosztikai információkat mutat, ami hasznos, ha először teszteled a saját modellt. Ha a modell sikeresen betöltődik, egy interaktív prompt jelenik meg. Próbálj meg néhány üzenetet:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Írd be az `exit` parancsot vagy nyomj `Ctrl+C`-t a munkamenet befejezéséhez.

> **Hibaelhárítás:** Ha a modell nem töltődik be, ellenőrizd a következőket:
> - A `genai_config.json` fájlt a modellépítő generálta.
> - Az `inference_model.json` fájl létezik és érvényes JSON.
> - Az ONNX modellfájlok a megfelelő könyvtárban vannak.
> - Van elegendő szabad RAM (a Qwen3-0.6B int4 körülbelül 1 GB-ot igényel).
> - A Qwen3 egy érvelő modell, amely `<think>` tageket generál. Ha válaszok előtt `<think>...</think>` tagokat látsz, ez normális viselkedés. Az `inference_model.json` prompt sablonja módosítható a gondolkodás kimenet elnyomásához.

---

### 8. Gyakorlat: Kérdezd le a saját modellt a REST API-n keresztül

Ha az interaktív munkamenetet befejezted a 7. gyakorlatban, elképzelhető, hogy a modell már nincs betöltve. Indítsd el a Foundry Local szolgáltatást, és töltsd be először a modellt:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Ellenőrizd, melyik porton fut a szolgáltatás:

```bash
foundry service status
```

Ezután küldj egy kérés (cseréld le a `5273` értéket a tényleges portodra, ha eltér):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows megjegyzés:** A fenti `curl` parancs bash szintaxist használ. Windows alatt a PowerShell `Invoke-RestMethod` cmdletjét használd helyette.

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

### 9. Gyakorlat: Használd a saját modellt az OpenAI SDK-val

Ugyanolyan OpenAI SDK kóddal kapcsolódhatsz a saját modelledhez, mint amit a beépített modellekhez használsz (lásd [3. rész](part3-sdk-and-apis.md)). Az egyetlen különbség a modell neve.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # A Foundry Local nem ellenőrzi az API kulcsokat
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
  apiKey: "foundry-local", // A Foundry Local nem ellenőrzi az API kulcsokat
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

> **Fontos megjegyzés:** Mivel a Foundry Local OpenAI-kompatibilis API-t kínál, bármely kód, amely a beépített modellekkel működik, a saját modellekkel is működik. Csak a `model` paramétert kell megváltoztatni.

---

### 10. Gyakorlat: Teszteld a saját modellt a Foundry Local SDK-val

Korábbi laborokban a Foundry Local SDK-t használtad a szolgáltatás indítására, végpont felfedezésére és modellek kezelésére automatikusan. Pontosan ugyanígy követheted ezt a mintát a saját lefordított modelleddel is. Az SDK kezeli a szolgáltatás indítását és a végpont felfedezését, így kódodnak nem kell keménykódolnia a `localhost:5273` címet.

> **Megjegyzés:** Győződj meg róla, hogy a Foundry Local SDK telepítve van, mielőtt futtatod ezeket a példákat:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Add hozzá a `Microsoft.AI.Foundry.Local` és `OpenAI` NuGet csomagokat
>
> Mentsd el az egyes script fájlokat **a tárhely gyökérkönyvtárába** (ugyanabba a könyvtárba, ahol a `models/` mappa van).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# 1. lépés: Indítsa el a Foundry Local szolgáltatást, és töltse be az egyedi modellt
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# 2. lépés: Ellenőrizze a gyorsítótárat az egyedi modellhez
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# 3. lépés: Töltse be a modellt a memóriába
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# 4. lépés: Hozzon létre egy OpenAI klienst az SDK által felfedezett végpont használatával
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# 5. lépés: Küldjön egy streaming chat befejezési kérést
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

Futtasd:

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

// 1. lépés: Indítsa el a Foundry Local szolgáltatást
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// 2. lépés: Szerezze be az egyedi modellt a katalógusból
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// 3. lépés: Töltse be a modellt a memóriába
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// 4. lépés: Hozzon létre egy OpenAI klienst az SDK által felfedezett végpont használatával
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// 5. lépés: Küldjön streaming chat befejezési kérelmet
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

Futtasd:

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

> **Fontos megjegyzés:** A Foundry Local SDK dinamikusan fedezi fel a végpontot, így sosem kell keménykódolnod portszámot. Ez a javasolt megközelítés éles alkalmazásokhoz. A saját lefordított modelled ugyanúgy működik a beépített katalógus modellekhez hasonlóan az SDK-n keresztül.

---

## Modell kiválasztása fordításhoz

A Qwen3-0.6B szolgál referenciaként ebben a laborban, mert kicsi, gyorsan fordítható és az Apache 2.0 licenc alatt szabadon elérhető. Azonban sok más modellt is fordíthatsz. Íme néhány javaslat:

| Modell | Hugging Face ID | Paraméterek | Licenc | Megjegyzések |
|-------|-----------------|------------|---------|--------------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Nagyon kicsi, gyors kompíláció, jó teszteléshez |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Jobb minőség, még mindig gyors fordítás |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Erős minőség, több RAM szükséges |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Licenc elfogadást igényel a Hugging Face-en |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Magas minőség, nagyobb letöltés és hosszabb fordítás |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Már a Foundry Local katalógusban (jó összehasonlításhoz) |

> **Licenc emlékeztető:** Mindig ellenőrizd a modell licencét a Hugging Face-en használat előtt. Egyes modellek (például Llama) megkövetelik a licenc elfogadását és a `huggingface-cli login` parancs használatával való hitelesítést letöltés előtt.

---

## Fogalmak: Mikor használjunk egyéni modelleket?

| Eset | Miért fordítsd le magad? |
|----------|-----------------------|
| **A szükséges modell nincs a katalógusban** | A Foundry Local katalógus válogatott. Ha a kívánt modell nincs a listán, fordítsd le magad. |
| **Finomhangolt modellek** | Ha egy modellt témaspecifikus adatokon hangoltál be, a saját súlyaidat kell lefordítanod. |
| **Specifikus kvantálási követelmények** | Lehet, hogy más pontosságot vagy kvantálási stratégiát szeretnél, mint ami a katalógus alapértelmezettje. |
| **Újabb modellik megjelenése** | Amikor új modellt adnak ki a Hugging Face-en, az még nem biztos, hogy benne van a Foundry Local katalógusban. Ha magad fordítod le, azonnali hozzáférésed lesz. |
| **Kutatás és kísérletezés** | Különböző modellarchitektúrákat, méreteket vagy konfigurációkat próbálhatsz ki helyben, mielőtt döntesz a gyártásról. |

---

## Összefoglalás

Ebben a laborban megtanultad, hogyan kell:

| Lépés | Mit csináltál |
|-------|---------------|
| 1 | Telepítetted az ONNX Runtime GenAI modellépítőt |
| 2 | Lefordítottad a `Qwen/Qwen3-0.6B` modellt a Hugging Face-ről optimalizált ONNX modellre |
| 3 | Létrehoztál egy `inference_model.json` chat-mintázat konfigurációs fájlt |
| 4 | Hozzáadtad a lefordított modellt a Foundry Local gyorsítótárához |
| 5 | Interaktív csevegést futtattál a saját modellel CLI-n keresztül |
| 6 | Lekérdezted a modellt az OpenAI-kompatibilis REST API-n keresztül |
| 7 | Kapcsolódtál Pythonból, JavaScriptből és C#-ból az OpenAI SDK-val |
| 8 | Végigtesztelted a saját modellt a Foundry Local SDK-val |

A legfontosabb, hogy **bármilyen transzformer alapú modell futtatható a Foundry Localon**, ha ONNX formátumba lett fordítva. Az OpenAI-kompatibilis API azt jelenti, hogy a meglévő alkalmazásod kódja változtatás nélkül működik; csak a modell nevét kell kicserélned.

---

## Főbb tanulságok

| Fogalom | Részletek |
|---------|-----------|
| ONNX Runtime GenAI Modellépítő | Egyetlen parancsban konvertálja a Hugging Face modelleket ONNX formátumra kvantálással együtt |
| ONNX formátum | A Foundry Local ONNX modelleket igényel ONNX Runtime GenAI konfigurációval |
| Csevegő sablonok | Az `inference_model.json` fájl megmondja a Foundry Localnak, hogyan formázzon promptokat egy adott modellhez |
| Hardver célpontok | Fordíts CPU-ra, NVIDIA GPU-ra (CUDA), DirectML-re (Windows GPU) vagy WebGPU-ra a hardveredtől függően |
| Kvantálás | Az alacsonyabb pontosság (int4) csökkenti a méretet és gyorsít a pontosság némileg csökkenésével; fp16 magas minőséget tart meg GPU-kon |
| API kompatibilitás | A saját modellek ugyanazt az OpenAI-kompatibilis API-t használják, mint a beépített modellek |
| Foundry Local SDK | Az SDK automatikusan kezeli a szolgáltatás indítását, végpont felfedezését és modell betöltést katalógus vagy egyéni modelleknél |

---

## További olvasmányok

| Forrás | Link |
|--------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local egyéni modell útmutató | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 modellcsalád | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive dokumentáció | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Következő lépések

Folytasd a [11. rész: Eszközhívás helyi modellekkel](part11-tool-calling.md) megismerésével, hogy megtudd, hogyan engedélyezd a lokális modellek számára a külső függvényhívásokat.

[← 9. rész: Whisper hangátírás](part9-whisper-voice-transcription.md) | [11. rész: Eszközhívás →](part11-tool-calling.md)