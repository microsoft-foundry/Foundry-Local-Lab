![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Partea 10: Utilizarea modelelor personalizate sau Hugging Face cu Foundry Local

> **Obiectiv:** Compilarea unui model Hugging Face în formatul ONNX optimizat pe care îl cere Foundry Local, configurarea acestuia cu un șablon de chat, adăugarea sa în cache-ul local și rularea inferenței folosind CLI, REST API și OpenAI SDK.

## Prezentare generală

Foundry Local vine cu un catalog selectat de modele pre-compilate, dar nu ești limitat la această listă. Orice model de limbaj bazat pe transformere disponibil pe [Hugging Face](https://huggingface.co/) (sau stocat local în format PyTorch / Safetensors) poate fi compilat într-un model ONNX optimizat și servit prin Foundry Local.

Pipeline-ul de compilare folosește **ONNX Runtime GenAI Model Builder**, un unelte CLI inclus în pachetul `onnxruntime-genai`. Model builder-ul se ocupă de toate etapele grele: descarcă greutățile sursă, le convertește în format ONNX, aplică cuantizarea (int4, fp16, bf16) și generează fișierele de configurare (inclusiv șablonul de chat și tokenizerul) pe care le așteaptă Foundry Local.

În acest laborator vei compila **Qwen/Qwen3-0.6B** de pe Hugging Face, îl vei înregistra în Foundry Local și vei putea purta un chat complet local pe dispozitivul tău.

---

## Obiective de învățare

La finalul acestui laborator vei putea să:

- Explici de ce compilarea unui model personalizat este utilă și când ai nevoie de ea
- Instalezi ONNX Runtime GenAI model builder
- Compilezi un model Hugging Face în format ONNX optimizat cu o singură comandă
- Înțelegi parametrii principali de compilare (execution provider, precizie)
- Creezi fișierul de configurare `inference_model.json` pentru șablonul de chat
- Adaugi un model compilat în cache-ul Foundry Local
- Rulezi inferențe pe modelul personalizat folosind CLI, REST API și OpenAI SDK

---

## Cerințe

| Cerință | Detalii |
|-------------|---------|
| **Foundry Local CLI** | Instalată și în calea ta `PATH` ([Partea 1](part1-getting-started.md)) |
| **Python 3.10+** | Necesare pentru ONNX Runtime GenAI model builder |
| **pip** | Manager de pachete Python |
| **Spațiu pe disc** | Cel puțin 5 GB pentru fișierele sursă și cele compilate |
| **Cont Hugging Face** | Unele modele necesită acceptarea unei licențe înainte de descărcare. Qwen3-0.6B folosește licența Apache 2.0 și este disponibil gratuit. |

---

## Configurarea mediului

Compilarea modelelor necesită mai multe pachete Python mari (PyTorch, ONNX Runtime GenAI, Transformers). Creează un mediu virtual dedicat pentru a nu interfera cu Python-ul sistemului sau alte proiecte.

```bash
# Din rădăcina depozitului
python -m venv .venv
```

Activează mediul:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Actualizează pip pentru a evita problemele de rezolvare a dependențelor:

```bash
python -m pip install --upgrade pip
```

> **Sfat:** Dacă ai deja un `.venv` din laboratoare anterioare, îl poți reutiliza. Asigură-te doar că este activat înainte să continui.

---

## Concept: Pipeline-ul de compilare

Foundry Local necesită modele în format ONNX cu configurare ONNX Runtime GenAI. Majoritatea modelelor open-source pe Hugging Face sunt distribuite ca greutăți PyTorch sau Safetensors, deci este nevoie de o etapă de conversie.

![Pipeline de compilare al modelului personalizat](../../../images/custom-model-pipeline.svg)

### Ce face Model Builder-ul?

1. **Descarcă** modelul sursă de pe Hugging Face (sau îl citește de pe un path local).
2. **Convertește** greutățile PyTorch / Safetensors în format ONNX.
3. **Cuantizează** modelul la o precizie mai mică (de exemplu, int4) ca să reducă consumul de memorie și să crească rata de procesare.
4. **Generează** fișierele de configurare ONNX Runtime GenAI (`genai_config.json`), șablonul de chat (`chat_template.jinja`) și toate fișierele tokenizerului pentru ca Foundry Local să încarce și să servească modelul.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Poți întâlni referințe la **Microsoft Olive** ca o unealtă alternativă pentru optimizarea modelelor. Ambele pot produce modele ONNX, dar au scopuri diferite și compromisuri diferite:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Pachet** | `onnxruntime-genai` | `olive-ai` |
| **Scop principal** | Conversia și cuantizarea modelelor AI generative pentru ONNX Runtime GenAI | Cadru de optimizare complet pentru modele cu suport pentru multe backend-uri și hardware |
| **Ușurință în utilizare** | O singură comandă — conversie + cuantizare într-un singur pas | Workflow-uri configurabile cu multiple etape și YAML/JSON |
| **Format output** | Format ONNX Runtime GenAI (pregătit pentru Foundry Local) | ONNX generic, ONNX Runtime GenAI sau alte formate în funcție de workflow |
| **Ținte hardware** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN și altele |
| **Opțiuni cuantizare** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, plus optimizări grafice, ajustări layer-wise |
| **Scop modele** | Modele AI generative (LLMs, SLMs) | Orice model convertibil ONNX (vision, NLP, audio, multimodal) |
| **Cel mai potrivit pentru** | Compilare rapidă a unui singur model pentru inferență locală | Pipeline-uri de producție cu control avansat al optimizării |
| **Dependențe** | Moderate (PyTorch, Transformers, ONNX Runtime) | Mai mari (include framework Olive și opționale pe workflow) |
| **Integrare Foundry Local** | Directă — output-ul este imediat compatibil | Necesită flag-ul `--use_ort_genai` și configurare suplimentară |

> **De ce folosim Model Builder-ul în acest laborator:** Pentru sarcina de a compila un model Hugging Face și de a-l înregistra în Foundry Local, Model Builder este cea mai simplă și fiabilă metodă. Produce exact formatul de output cerut de Foundry Local într-o singură comandă. Dacă vei avea nevoie mai târziu de optimizări avansate — cum ar fi cuantizare conștientă de precizie, modificări grafice sau reglare multi-pas — Olive este o opțiune puternică de explorat. Vezi [documentația Microsoft Olive](https://microsoft.github.io/Olive/) pentru mai multe detalii.

---

## Exerciții în laborator

### Exercițiul 1: Instalează ONNX Runtime GenAI Model Builder

Instalează pachetul ONNX Runtime GenAI, care include unealta model builder:

```bash
pip install onnxruntime-genai
```

Verifică instalarea căutând model builder-ul disponibil:

```bash
python -m onnxruntime_genai.models.builder --help
```

Ar trebui să vezi un output de ajutor care listează parametri precum `-m` (nume model), `-o` (cale output), `-p` (precizie) și `-e` (execution provider).

> **Notă:** Model builder-ul depinde de PyTorch, Transformers și câteva alte pachete. Instalarea poate dura câteva minute.

---

### Exercițiul 2: Compilează Qwen3-0.6B pentru CPU

Rulează comanda următoare pentru a descărca modelul Qwen3-0.6B de pe Hugging Face și a-l compila pentru inferență CPU cu cuantizare int4:

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

#### Ce face fiecare parametru

| Parametru | Scop | Valoare utilizată |
|-----------|---------|------------|
| `-m` | ID model Hugging Face sau cale locală | `Qwen/Qwen3-0.6B` |
| `-o` | Directorul unde vor fi salvate fișierele ONNX compilate | `models/qwen3` |
| `-p` | Precizia cuantizării aplicate la compilare | `int4` |
| `-e` | Execution provider pentru ONNX Runtime (hardware țintă) | `cpu` |
| `--extra_options hf_token=false` | Ocolește autentificarea Hugging Face (ok pentru modele publice) | `hf_token=false` |

> **Cât durează?** Timpul de compilare depinde de hardware și dimensiunea modelului. Pentru Qwen3-0.6B cu cuantizare int4 pe CPU modern, așteaptă-te la 5–15 minute. Modelele mai mari durează proporțional mai mult.

După finalizarea comenzii ar trebui să vezi un director `models/qwen3` care conține fișierele compilate. Verifică output-ul:

```bash
ls models/qwen3
```

Ar trebui să vezi fișiere precum:
- `model.onnx` și `model.onnx.data` — greutățile modelului compilat
- `genai_config.json` — configurația ONNX Runtime GenAI
- `chat_template.jinja` — șablonul de chat al modelului (generat automat)
- `tokenizer.json`, `tokenizer_config.json` — fișierele tokenizerului
- Alte fișiere de vocabular și configurație

---

### Exercițiul 3: Compilează pentru GPU (Opțional)

Dacă ai o placă NVIDIA cu suport CUDA poți compila o variantă optimizată pentru GPU pentru inferență mai rapidă:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Notă:** Compilarea GPU necesită `onnxruntime-gpu` și o instalare CUDA funcțională. Dacă acestea lipsesc, model builder va raporta o eroare. Poți sări acest exercițiu și să continui cu varianta CPU.

#### Referință compilare specifică hardware

| Țintă | Execution Provider (`-e`) | Precizia recomandată (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` sau `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` sau `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Compromisuri în precizie

| Precizie | Dimensiune | Viteză | Calitate |
|-----------|------------|---------|----------|
| `fp32` | Cea mai mare | Cea mai lentă | Precizie maximă |
| `fp16` | Mare | Rapid (GPU) | Precizie foarte bună |
| `int8` | Mică | Rapidă | Ușoară pierdere de precizie |
| `int4` | Cea mai mică | Cea mai rapidă | Pierdere moderată de precizie |

Pentru dezvoltare locală de obicei `int4` pe CPU oferă cel mai bun echilibru între viteză și consumul de resurse. Pentru output de calitate producție, se recomandă `fp16` pe GPU CUDA.

---

### Exercițiul 4: Crearea configurației șablonului de chat

Model builder generează automat un fișier `chat_template.jinja` și un `genai_config.json` în directorul de output. Totuși, Foundry Local mai are nevoie și de un fișier `inference_model.json` pentru a înțelege cum să formateze prompturile pentru modelul tău. Acest fișier definește numele modelului și șablonul de prompt care învelește mesajele utilizatorului în tokenii speciali corecți.

#### Pasul 1: Inspectează output-ul compilat

Listează conținutul directorului modelului compilat:

```bash
ls models/qwen3
```

Ar trebui să vezi fișiere precum:
- `model.onnx` și `model.onnx.data` — greutățile compilate ale modelului
- `genai_config.json` — configurația ONNX Runtime GenAI (generată automat)
- `chat_template.jinja` — șablonul de chat al modelului (generat automat)
- `tokenizer.json`, `tokenizer_config.json` — fișierele tokenizerului
- Alte fișiere de vocabular și configurare

#### Pasul 2: Generează fișierul inference_model.json

Fișierul `inference_model.json` spune Foundry Local cum să formateze prompturile. Creează un script Python numit `generate_chat_template.py` **în rădăcina depozitului** (același director în care se află folderul `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Construiți o conversație minimală pentru a extrage șablonul de chat
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

# Construiți structura inference_model.json
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

Rulează scriptul din rădăcina depozitului:

```bash
python generate_chat_template.py
```

> **Notă:** Pachetul `transformers` este deja instalat ca dependență a lui `onnxruntime-genai`. Dacă primești o eroare `ImportError`, instalează-l manual cu `pip install transformers`.

Scriptul va produce un fișier `inference_model.json` în interiorul directorului `models/qwen3`. Acest fișier spune Foundry Local cum să încapsuleze inputul utilizatorului cu tokenii speciali corecți pentru Qwen3.

> **Important:** Câmpul `"Name"` din `inference_model.json` (setat aici la `qwen3-0.6b`) este **aliasul modelului** pe care îl vei folosi în toate comenzile și apelurile API ulterioare. Dacă schimbi acest nume, actualizează-l în exercițiile 6–10 corespunzător.

#### Pasul 3: Verifică configurația

Deschide `models/qwen3/inference_model.json` și confirmă că are un câmp `Name` și un obiect `PromptTemplate` cu cheile `assistant` și `prompt`. Șablonul de prompt ar trebui să includă tokenii speciali precum `<|im_start|>` și `<|im_end|>` (tokenii exacți depind de șablonul de chat al modelului).

> **Alternativ manual:** Dacă preferi să nu rulezi scriptul, poți crea fișierul manual. Cerința cheie este ca câmpul `prompt` să conțină șablonul complet de chat al modelului cu `{Content}` ca substituent pentru mesajul utilizatorului.

---

### Exercițiul 5: Verifică structura directorului modelului


Constructorul de modele plasează toate fișierele compilate direct în directorul de ieșire pe care l-ai specificat. Verifică dacă structura finală arată corect:

```bash
ls models/qwen3
```

Directorul ar trebui să conțină următoarele fișiere:

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

> **Notă:** Spre deosebire de alte unelte de compilare, constructorul de modele nu creează subdirectoare imbricate. Toate fișierele stau direct în folderul de ieșire, ceea ce este exact așteptat de Foundry Local.

---

### Exercițiul 6: Adaugă Modelul în Cache-ul Foundry Local

Spune lui Foundry Local unde să găsească modelul tău compilat, adăugând directorul în cache-ul său:

```bash
foundry cache cd models/qwen3
```

Verifică dacă modelul apare în cache:

```bash
foundry cache ls
```

Ar trebui să vezi modelul tău personalizat listat alături de oricare modele preexistente în cache (cum ar fi `phi-3.5-mini` sau `phi-4-mini`).

---

### Exercițiul 7: Rulează Modelul Personalizat cu CLI

Pornește o sesiune interactivă de chat cu modelul tău recent compilat (aliasul `qwen3-0.6b` vine din câmpul `Name` pe care l-ai setat în `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Flagul `--verbose` afișează informații suplimentare de diagnosticare, utile mai ales la testarea pentru prima dată a unui model personalizat. Dacă modelul se încarcă cu succes, vei vedea un prompt interactiv. Încearcă câteva mesaje:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Tastează `exit` sau apasă `Ctrl+C` pentru a încheia sesiunea.

> **Rezolvare probleme:** Dacă modelul nu se încarcă, verifică următoarele:
> - Fișierul `genai_config.json` a fost generat de constructorul de modele.
> - Fișierul `inference_model.json` există și este JSON valid.
> - Fișierele modelului ONNX se află în directorul corect.
> - Ai suficientă memorie RAM disponibilă (Qwen3-0.6B int4 necesită aproximativ 1 GB).
> - Qwen3 este un model de raționament care produce tag-uri `<think>`. Dacă vezi `<think>...</think>` la începutul răspunsurilor, este comportament normal. Șablonul prompt din `inference_model.json` poate fi ajustat pentru a suprima ieșirea de tip „thinking”.

---

### Exercițiul 8: Interoghează Modelul Personalizat prin API-ul REST

Dacă ai ieșit din sesiunea interactivă în Exercițiul 7, modelul s-ar putea să nu fie încărcat. Pornește mai întâi serviciul Foundry Local și încarcă modelul:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Verifică pe ce port rulează serviciul:

```bash
foundry service status
```

Apoi trimite o cerere (înlocuiește `5273` cu portul tău dacă este diferit):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Notă Windows:** Comanda `curl` de mai sus folosește sintaxa bash. Pe Windows, folosește comanda PowerShell `Invoke-RestMethod` de mai jos.

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

### Exercițiul 9: Folosește Modelul Personalizat cu SDK-ul OpenAI

Poți conecta la modelul tău personalizat folosind exact același cod din SDK-ul OpenAI pe care l-ai folosit pentru modelele încorporate (vezi [Partea 3](part3-sdk-and-apis.md)). Singura diferență este numele modelului.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local nu validează cheile API
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
  apiKey: "foundry-local", // Foundry Local nu validează cheile API
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

> **Punct cheie:** Deoarece Foundry Local expune o API compatibilă OpenAI, orice cod care funcționează cu modelele încorporate funcționează și cu modelele tale personalizate. Trebuie doar să schimbi parametrul `model`.

---

### Exercițiul 10: Testează Modelul Personalizat cu Foundry Local SDK

În laboratoarele anterioare ai folosit Foundry Local SDK pentru a porni serviciul, a descoperi endpoint-ul și a gestiona modelele automat. Poți urma exact același tipar și cu modelul compilat de tine. SDK-ul gestionează pornirea serviciului și descoperirea endpoint-ului, deci codul tău nu trebuie să hard-codeze `localhost:5273`.

> **Notă:** Asigură-te că Foundry Local SDK este instalat înainte de a rula aceste exemple:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Adaugă pachetele NuGet `Microsoft.AI.Foundry.Local` și `OpenAI`
>
> Salvează fiecare fișier script **în rădăcina depozitului** (același director cu folderul tău `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Pasul 1: Porniți serviciul Foundry Local și încărcați modelul personalizat
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Pasul 2: Verificați memoria cache pentru modelul personalizat
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Pasul 3: Încărcați modelul în memorie
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Pasul 4: Creați un client OpenAI folosind endpoint-ul descoperit de SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Pasul 5: Trimiteți o cerere de completare chat în flux continuu
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

Rulează-l:

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

// Pasul 1: Porniți serviciul Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Pasul 2: Obțineți modelul personalizat din catalog
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Pasul 3: Încărcați modelul în memorie
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Pasul 4: Creați un client OpenAI folosind punctul final descoperit de SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Pasul 5: Trimiteți o cerere de completare chat în flux
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

Rulează-l:

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

> **Punct cheie:** Foundry Local SDK descoperă endpoint-ul dinamic, deci nu hard-codezi niciodată un număr de port. Aceasta este metoda recomandată pentru aplicații în producție. Modelul tău compilat personalizat funcționează identic cu modelele din catalog prin SDK.

---

## Alegerea unui Model de Compilat

Qwen3-0.6B este folosit ca exemplu de referință în acest laborator deoarece este mic, se compilează rapid și este disponibil liber sub licența Apache 2.0. Totuși, poți compila multe alte modele. Iată câteva sugestii:

| Model | Hugging Face ID | Parametri | Licență | Note |
|-------|-----------------|-----------|---------|------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Foarte mic, compilare rapidă, bun pentru testare |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Calitate mai bună, încă rapid la compilare |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Calitate puternică, necesită mai multă memorie RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Necesită acceptarea licenței pe Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Calitate înaltă, descărcare mai mare și compilare mai lungă |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Deja în catalogul Foundry Local (util pentru comparații) |

> **Reamintire licență:** Verifică întotdeauna licența modelului pe Hugging Face înainte de a-l folosi. Unele modele (precum Llama) necesită să accepți un acord de licență și să te autentifici cu `huggingface-cli login` înainte de descărcare.

---

## Concepte: Când Să Folosești Modele Personalizate

| Scenariu | De ce să-ți compilezi propriul model? |
|----------|---------------------------------------|
| **Un model de care ai nevoie nu este în catalog** | Catalogul Foundry Local este curationat. Dacă modelul dorit nu este listat, compilează-l tu însuți. |
| **Modele fine-tunate** | Dacă ai finetunat un model pe date specifice domeniului, trebuie să compilezi propriile greutăți. |
| **Cerințe specifice de cuantizare** | Poți dori o precizie sau o strategie de cuantizare care diferă de opțiunea implicită din catalog. |
| **Lansări noi de modele** | Când apare un model nou pe Hugging Face, poate să nu fie încă în catalogul Foundry Local. Compilându-l singur ai acces imediat. |
| **Cercetare și experimentare** | Încearcă arhitecturi, dimensiuni sau configurații diferite local, înainte de o alegere pentru producție. |

---

## Rezumat

În acest laborator ai învățat să:

| Pas | Ce ai făcut |
|------|------------|
| 1 | Ai instalat constructorul de modele ONNX Runtime GenAI |
| 2 | Ai compilat `Qwen/Qwen3-0.6B` de pe Hugging Face în model ONNX optimizat |
| 3 | Ai creat un fișier de configurare `inference_model.json` cu șablon pentru chat |
| 4 | Ai adăugat modelul compilat în cache-ul Foundry Local |
| 5 | Ai rulat o conversație interactivă cu modelul personalizat prin CLI |
| 6 | Ai interogat modelul prin API-ul REST compatibil OpenAI |
| 7 | Te-ai conectat din Python, JavaScript și C# folosind SDK-ul OpenAI |
| 8 | Ai testat modelul personalizat end-to-end cu Foundry Local SDK |

Ideea principală este că **orice model bazat pe transformere poate rula prin Foundry Local** odată ce este compilat în format ONNX. API-ul compatibil OpenAI înseamnă că tot codul tău de aplicație existent funcționează fără modificări; trebuie doar să schimbi numele modelului.

---

## Aspecte-cheie

| Concept | Detaliu |
|---------|---------|
| Constructorul de modele ONNX Runtime GenAI | Convertește modelele Hugging Face în format ONNX cu cuantizare într-o singură comandă |
| Formatul ONNX | Foundry Local necesită modele ONNX cu configurație ONNX Runtime GenAI |
| Șabloane pentru chat | Fișierul `inference_model.json` spune Foundry Local cum să formateze prompturile pentru un model dat |
| Ținte hardware | Compilează pentru CPU, GPU NVIDIA (CUDA), DirectML (GPU Windows) sau WebGPU în funcție de hardware |
| Cuantizare | Precizie mai mică (int4) reduce dimensiunea și crește viteza în detrimentul puțină acuratețe; fp16 păstrează calitate înaltă pe GPU-uri |
| Compatibilitate API | Modelele personalizate folosesc aceeași API compatibilă OpenAI ca modelele încorporate |
| Foundry Local SDK | SDK-ul gestionează pornirea serviciului, descoperirea endpoint-ului și încărcarea modelelor automat pentru ambele tipuri |

---

## Lecturi suplimentare

| Resursă | Link |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Ghid modele personalizate Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Familia de modele Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Documentație Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Pașii următori

Continuă cu [Partea 11: Apelarea Instrumentelor cu Modele Locale](part11-tool-calling.md) pentru a învăța cum să permiți modelelor tale locale să apeleze funcții externe.

[← Partea 9: Transcriere vocală Whisper](part9-whisper-voice-transcription.md) | [Partea 11: Apelarea Instrumentelor →](part11-tool-calling.md)