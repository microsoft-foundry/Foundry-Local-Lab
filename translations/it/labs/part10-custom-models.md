![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Parte 10: Utilizzo di modelli personalizzati o Hugging Face con Foundry Local

> **Obiettivo:** Compilare un modello Hugging Face nel formato ONNX ottimizzato richiesto da Foundry Local, configurarlo con un modello di chat, aggiungerlo alla cache locale ed eseguire inferenza tramite CLI, REST API e OpenAI SDK.

## Panoramica

Foundry Local viene fornito con un catalogo curato di modelli precompilati, ma non sei limitato a quella lista. Qualsiasi modello linguistico basato su transformer disponibile su [Hugging Face](https://huggingface.co/) (o memorizzato localmente in formato PyTorch / Safetensors) può essere compilato in un modello ONNX ottimizzato e servito tramite Foundry Local.

La pipeline di compilazione utilizza **ONNX Runtime GenAI Model Builder**, uno strumento a riga di comando incluso nel pacchetto `onnxruntime-genai`. Il model builder gestisce il lavoro principale: scaricamento dei pesi sorgente, conversione in formato ONNX, applicazione della quantizzazione (int4, fp16, bf16) ed emissione dei file di configurazione (inclusi il modello di chat e il tokenizzatore) che Foundry Local si aspetta.

In questo laboratorio compilerai **Qwen/Qwen3-0.6B** da Hugging Face, lo registrerai con Foundry Local e dialogherai con esso interamente sul tuo dispositivo.

---

## Obiettivi di apprendimento

Al termine di questo laboratorio sarai in grado di:

- Spiegare perché è utile compilare modelli personalizzati e quando potrebbe servire
- Installare ONNX Runtime GenAI model builder
- Compilare un modello Hugging Face in formato ONNX ottimizzato con un singolo comando
- Comprendere i parametri chiave di compilazione (execution provider, precisione)
- Creare il file di configurazione `inference_model.json` del modello di chat
- Aggiungere un modello compilato alla cache di Foundry Local
- Eseguire inferenza sul modello personalizzato utilizzando CLI, REST API e OpenAI SDK

---

## Prerequisiti

| Requisito | Dettagli |
|-------------|---------|
| **Foundry Local CLI** | Installato e nella tua `PATH` ([Parte 1](part1-getting-started.md)) |
| **Python 3.10+** | Richiesto dal ONNX Runtime GenAI model builder |
| **pip** | Gestore pacchetti Python |
| **Spazio su disco** | Almeno 5 GB liberi per i file del modello sorgente e compilato |
| **Account Hugging Face** | Alcuni modelli richiedono di accettare una licenza prima di scaricarli. Qwen3-0.6B usa la licenza Apache 2.0 ed è liberamente disponibile. |

---

## Configurazione dell’ambiente

La compilazione del modello richiede diversi pacchetti Python di grandi dimensioni (PyTorch, ONNX Runtime GenAI, Transformers). Crea un ambiente virtuale dedicato affinché non interferiscano con il tuo Python di sistema o altri progetti.

```bash
# Dalla radice del repository
python -m venv .venv
```

Attiva l’ambiente:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Aggiorna pip per evitare problemi di risoluzione delle dipendenze:

```bash
python -m pip install --upgrade pip
```

> **Suggerimento:** Se hai già un `.venv` dai laboratori precedenti, puoi riutilizzarlo. Assicurati solo che sia attivato prima di continuare.

---

## Concetto: La pipeline di compilazione

Foundry Local richiede modelli in formato ONNX con configurazione ONNX Runtime GenAI. La maggior parte dei modelli open source su Hugging Face viene distribuita come pesi PyTorch o Safetensors, quindi è necessaria una fase di conversione.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### Cosa fa il Model Builder?

1. **Scarica** il modello sorgente da Hugging Face (o lo legge da un percorso locale).
2. **Converte** i pesi PyTorch / Safetensors in formato ONNX.
3. **Quantizza** il modello a una precisione inferiore (ad esempio, int4) per ridurre l’uso di memoria e migliorare la velocità.
4. **Emette** la configurazione ONNX Runtime GenAI (`genai_config.json`), il modello di chat (`chat_template.jinja`) e tutti i file del tokenizzatore così che Foundry Local possa caricare e servire il modello.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Potresti incontrare riferimenti a **Microsoft Olive** come strumento alternativo per l’ottimizzazione dei modelli. Entrambi gli strumenti possono produrre modelli ONNX, ma hanno obiettivi diversi e differenti compromessi:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Pacchetto** | `onnxruntime-genai` | `olive-ai` |
| **Scopo principale** | Convertire e quantizzare modelli AI generativi per inferenza ONNX Runtime GenAI | Framework di ottimizzazione end-to-end per modelli con molti backend e target hardware |
| **Facilità d’uso** | Comando singolo — conversione + quantizzazione in un solo passaggio | Basato su flussi di lavoro — pipeline multi-step configurabili con YAML/JSON |
| **Formato output** | Formato ONNX Runtime GenAI (pronto per Foundry Local) | ONNX generico, ONNX Runtime GenAI o altri formati a seconda del workflow |
| **Target hardware** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN e altro |
| **Opzioni di quantizzazione** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, più ottimizzazioni grafiche e tuning layer-wise |
| **Ambito dei modelli** | Modelli AI generativi (LLM, SLM) | Qualsiasi modello convertibile in ONNX (vision, NLP, audio, multimodale) |
| **Ideale per** | Compilazione rapida di un solo modello per inferenza locale | Pipeline di produzione con controllo fine dell’ottimizzazione |
| **Dipendenze** | Moderate (PyTorch, Transformers, ONNX Runtime) | Maggiori (aggiunge framework Olive, opzionali per workflow) |
| **Integrazione Foundry Local** | Diretta — output immediatamente compatibile | Richiede flag `--use_ort_genai` e configurazioni aggiuntive |

> **Perché questo laboratorio usa Model Builder:** Per compilare un singolo modello Hugging Face e registrarlo in Foundry Local, Model Builder è il percorso più semplice e affidabile. Produce l’output esatto che Foundry Local si aspetta con un solo comando. Se in seguito ti servono funzioni di ottimizzazione avanzate — come quantizzazione consapevole della precisione, modifica grafi o tuning multipasso — Olive è uno strumento potente da esplorare. Consulta la [documentazione Microsoft Olive](https://microsoft.github.io/Olive/) per maggiori dettagli.

---

## Esercizi del laboratorio

### Esercizio 1: Installare ONNX Runtime GenAI Model Builder

Installa il pacchetto ONNX Runtime GenAI, che include lo strumento model builder:

```bash
pip install onnxruntime-genai
```

Verifica l’installazione controllando che il model builder sia disponibile:

```bash
python -m onnxruntime_genai.models.builder --help
```

Dovresti vedere l’output di help con parametri come `-m` (nome modello), `-o` (percorso output), `-p` (precisione) e `-e` (execution provider).

> **Nota:** Il model builder dipende da PyTorch, Transformers e altri pacchetti. L’installazione può richiedere qualche minuto.

---

### Esercizio 2: Compilare Qwen3-0.6B per CPU

Esegui il comando seguente per scaricare il modello Qwen3-0.6B da Hugging Face e compilarlo per inferenza CPU con quantizzazione int4:

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

#### Cosa fa ciascun parametro

| Parametro | Scopo | Valore utilizzato |
|-----------|---------|------------|
| `-m` | ID modello Hugging Face o percorso directory locale | `Qwen/Qwen3-0.6B` |
| `-o` | Directory dove salvare il modello ONNX compilato | `models/qwen3` |
| `-p` | Precisione della quantizzazione applicata durante la compilazione | `int4` |
| `-e` | Provider di esecuzione ONNX Runtime (hardware target) | `cpu` |
| `--extra_options hf_token=false` | Salta l’autenticazione Hugging Face (valido per modelli pubblici) | `hf_token=false` |

> **Quanto tempo richiede?** La durata della compilazione dipende dall’hardware e dalla dimensione del modello. Per Qwen3-0.6B con quantizzazione int4 su una CPU moderna, aspettati circa 5-15 minuti. Modelli più grandi impiegano proporzionalmente di più.

Al termine del comando dovresti vedere una directory `models/qwen3` contenente i file del modello compilato. Verifica l’output:

```bash
ls models/qwen3
```

Dovresti vedere file inclusi:
- `model.onnx` e `model.onnx.data` — pesi del modello compilato
- `genai_config.json` — configurazione ONNX Runtime GenAI
- `chat_template.jinja` — modello di chat (generato automaticamente)
- `tokenizer.json`, `tokenizer_config.json` — file del tokenizzatore
- Altri file di vocabolario e configurazione

---

### Esercizio 3: Compilare per GPU (Opzionale)

Se disponi di una GPU NVIDIA con supporto CUDA, puoi compilare una variante ottimizzata per GPU per inferenze più rapide:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Nota:** La compilazione per GPU richiede `onnxruntime-gpu` e un’installazione CUDA funzionante. Se questi non sono presenti, il model builder segnalerà un errore. Puoi saltare questo esercizio e procedere con la variante CPU.

#### Riferimento compilazione specifica per hardware

| Target | Execution Provider (`-e`) | Precisione raccomandata (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| GPU NVIDIA | `cuda` | `fp16` o `int4` |
| DirectML (GPU Windows) | `dml` | `fp16` o `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Compromessi di precisione

| Precisione | Dimensione | Velocità | Qualità |
|-----------|------|-------|---------|
| `fp32` | Grande | Più lenta | Massima precisione |
| `fp16` | Media | Veloce (GPU) | Precisione molto buona |
| `int8` | Piccola | Veloce | Leggera perdita di precisione |
| `int4` | Minima | Più veloce | Perdita moderata di precisione |

Per lo sviluppo locale, `int4` su CPU offre il miglior compromesso tra velocità e risorse. Per output di qualità produzione si consiglia `fp16` su GPU CUDA.

---

### Esercizio 4: Creare il file di configurazione del modello di chat

Il model builder genera automaticamente un file `chat_template.jinja` e un file `genai_config.json` nella directory di output. Tuttavia, Foundry Local richiede anche un file `inference_model.json` per capire come formattare i prompt per il tuo modello. Questo file definisce il nome del modello e il template di prompt che incapsula i messaggi utente nei token speciali corretti.

#### Passo 1: Ispeziona l’output compilato

Elenca il contenuto della directory del modello compilato:

```bash
ls models/qwen3
```

Dovresti vedere file quali:
- `model.onnx` e `model.onnx.data` — pesi del modello compilato
- `genai_config.json` — configurazione ONNX Runtime GenAI (generata automaticamente)
- `chat_template.jinja` — modello di chat del modello (generato automaticamente)
- `tokenizer.json`, `tokenizer_config.json` — file del tokenizzatore
- Vari altri file di configurazione e vocabolario

#### Passo 2: Genera il file inference_model.json

Il file `inference_model.json` indica a Foundry Local come formattare i prompt. Crea uno script Python chiamato `generate_chat_template.py` **nella root del repository** (la stessa directory che contiene la cartella `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Costruisci una conversazione minimale per estrarre il modello di chat
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

# Costruisci la struttura inference_model.json
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

Esegui lo script dalla root del repository:

```bash
python generate_chat_template.py
```

> **Nota:** Il pacchetto `transformers` è già stato installato come dipendenza di `onnxruntime-genai`. Se vedi un `ImportError`, esegui prima `pip install transformers`.

Lo script produce un file `inference_model.json` dentro la directory `models/qwen3`. Il file indica a Foundry Local come incapsulare l’input utente nei token speciali corretti per Qwen3.

> **Importante:** Il campo `"Name"` in `inference_model.json` (impostato su `qwen3-0.6b` in questo script) è l'**alias del modello** che userai in tutti i comandi e chiamate API successive. Se cambi questo nome, aggiorna il nome modello negli Esercizi 6–10 di conseguenza.

#### Passo 3: Verifica la configurazione

Apri `models/qwen3/inference_model.json` e conferma che contenga un campo `Name` e un oggetto `PromptTemplate` con chiavi `assistant` e `prompt`. Il modello di prompt dovrebbe includere token speciali come `<|im_start|>` e `<|im_end|>` (i token esatti dipendono dal modello di chat).

> **Alternativa manuale:** Se preferisci non eseguire lo script, puoi creare il file manualmente. L’unico requisito chiave è che il campo `prompt` contenga il modello di chat completo del modello con `{Content}` come segnaposto per il messaggio dell’utente.

---

### Esercizio 5: Verifica la struttura della directory del modello

Il compilatore del modello posiziona tutti i file compilati direttamente nella directory di output che hai specificato. Verifica che la struttura finale sia corretta:

```bash
ls models/qwen3
```

La directory dovrebbe contenere i seguenti file:

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

> **Nota:** A differenza di alcuni altri strumenti di compilazione, il compilatore del modello non crea sottodirectory nidificate. Tutti i file si trovano direttamente nella cartella di output, che è esattamente ciò che Foundry Local si aspetta.

---

### Esercizio 6: Aggiungi il Modello alla Cache di Foundry Local

Indica a Foundry Local dove trovare il tuo modello compilato aggiungendo la directory alla sua cache:

```bash
foundry cache cd models/qwen3
```

Verifica che il modello compaia nella cache:

```bash
foundry cache ls
```

Dovresti vedere il tuo modello personalizzato elencato insieme agli eventuali modelli precedentemente presenti nella cache (come `phi-3.5-mini` o `phi-4-mini`).

---

### Esercizio 7: Esegui il Modello Personalizzato con la CLI

Avvia una sessione di chat interattiva con il tuo modello appena compilato (l'alias `qwen3-0.6b` deriva dal campo `Name` che hai impostato in `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Il flag `--verbose` mostra informazioni diagnostiche aggiuntive, utili quando si testa un modello personalizzato per la prima volta. Se il modello viene caricato con successo vedrai un prompt interattivo. Prova qualche messaggio:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Digita `exit` oppure premi `Ctrl+C` per terminare la sessione.

> **Risoluzione problemi:** Se il modello non si carica, verifica quanto segue:
> - Il file `genai_config.json` è stato generato dal compilatore del modello.
> - Il file `inference_model.json` esiste ed è un JSON valido.
> - I file del modello ONNX si trovano nella directory corretta.
> - Hai abbastanza RAM disponibile (Qwen3-0.6B int4 necessita di circa 1 GB).
> - Qwen3 è un modello di ragionamento che produce tag `<think>`. Se vedi risposte precedute da `<think>...</think>`, è un comportamento normale. Il template del prompt in `inference_model.json` può essere modificato per sopprimere l'output del ragionamento.

---

### Esercizio 8: Interroga il Modello Personalizzato tramite REST API

Se hai chiuso la sessione interattiva nell'Esercizio 7, il modello potrebbe non essere più caricato. Avvia il servizio Foundry Local e carica prima il modello:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Controlla su quale porta sta girando il servizio:

```bash
foundry service status
```

Poi invia una richiesta (sostituisci `5273` con la tua porta effettiva, se diversa):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Nota Windows:** Il comando `curl` sopra usa la sintassi bash. Su Windows, usa invece il cmdlet PowerShell `Invoke-RestMethod` qui sotto.

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

### Esercizio 9: Usa il Modello Personalizzato con l’OpenAI SDK

Puoi connetterti al tuo modello personalizzato usando esattamente lo stesso codice OpenAI SDK che usi per i modelli integrati (vedi [Parte 3](part3-sdk-and-apis.md)). L’unica differenza è il nome del modello.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local non convalida le chiavi API
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
  apiKey: "foundry-local", // Foundry Local non convalida le chiavi API
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

> **Punto chiave:** Poiché Foundry Local espone un’API compatibile OpenAI, qualsiasi codice che funziona con i modelli integrati funziona anche con i tuoi modelli personalizzati. Devi solo modificare il parametro `model`.

---

### Esercizio 10: Testa il Modello Personalizzato con il Foundry Local SDK

Nei laboratori precedenti hai utilizzato il Foundry Local SDK per avviare il servizio, scoprire l’endpoint e gestire i modelli automaticamente. Puoi seguire esattamente lo stesso schema con il tuo modello compilato personalizzato. Lo SDK gestisce l’avvio del servizio e la scoperta dell’endpoint, quindi il tuo codice non ha bisogno di codificare a mano `localhost:5273`.

> **Nota:** Assicurati che il Foundry Local SDK sia installato prima di eseguire questi esempi:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Aggiungi i pacchetti NuGet `Microsoft.AI.Foundry.Local` e `OpenAI`
>
> Salva ogni file script **nella radice del repository** (nella stessa directory della cartella `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Passo 1: Avvia il servizio Foundry Local e carica il modello personalizzato
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Passo 2: Controlla la cache per il modello personalizzato
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Passo 3: Carica il modello in memoria
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Passo 4: Crea un client OpenAI usando l'endpoint trovato dall’SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Passo 5: Invia una richiesta di completamento chat in streaming
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

Eseguilo:

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

// Passo 1: Avvia il servizio Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Passo 2: Ottieni il modello personalizzato dal catalogo
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Passo 3: Carica il modello in memoria
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Passo 4: Crea un client OpenAI usando l'endpoint scoperto dall'SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Passo 5: Invia una richiesta di completamento chat in streaming
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

Eseguilo:

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

> **Punto chiave:** Il Foundry Local SDK scopre l’endpoint dinamicamente, quindi non devi mai codificare a mano un numero di porta. Questo è l’approccio consigliato per applicazioni in produzione. Il tuo modello compilato personalizzato funziona esattamente come i modelli di catalogo integrati tramite lo SDK.

---

## Scelta di un Modello da Compilare

Qwen3-0.6B è usato come esempio di riferimento in questo laboratorio perché è piccolo, veloce da compilare e liberamente disponibile sotto licenza Apache 2.0. Tuttavia, puoi compilare molti altri modelli. Ecco alcuni suggerimenti:

| Modello | ID Hugging Face | Parametri | Licenza | Note |
|---------|-----------------|-----------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Molto piccolo, compilazione veloce, ottimo per test |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Qualità migliore, ancora veloce da compilare |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Ottima qualità, richiede più RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Richiede accettazione della licenza su Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Alta qualità, download più grande e compilazione più lunga |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Già nel catalogo Foundry Local (utile per confronto) |

> **Promemoria licenza:** Controlla sempre la licenza del modello su Hugging Face prima di usarlo. Alcuni modelli (come Llama) richiedono di accettare un accordo di licenza e autenticarsi con `huggingface-cli login` prima del download.

---

## Concetti: Quando Usare Modelli Personalizzati

| Scenario | Perché Compilare da Solo? |
|----------|---------------------------|
| **Un modello necessario non è nel catalogo** | Il catalogo di Foundry Local è curato. Se il modello che desideri non è elencato, compilalo tu stesso. |
| **Modelli fine-tuned** | Se hai perfezionato un modello su dati specifici di dominio, devi compilare i tuoi pesi. |
| **Requisiti specifici di quantizzazione** | Potresti voler una precisione o una strategia di quantizzazione diversa da quella predefinita nel catalogo. |
| **Nuove versioni di modelli** | Quando viene rilasciato un nuovo modello su Hugging Face, potrebbe non essere ancora nel catalogo Foundry Local. Compilarlo ti dà accesso immediato. |
| **Ricerca e sperimentazione** | Provare diverse architetture, dimensioni o configurazioni di modelli localmente prima di scegliere un’opzione di produzione. |

---

## Riepilogo

In questo laboratorio hai imparato a:

| Passo | Cosa Hai Fatto |
|-------|----------------|
| 1 | Installato il compilatore di modelli ONNX Runtime GenAI |
| 2 | Compilato `Qwen/Qwen3-0.6B` da Hugging Face in un modello ONNX ottimizzato |
| 3 | Creato un file di configurazione template chat `inference_model.json` |
| 4 | Aggiunto il modello compilato alla cache di Foundry Local |
| 5 | Eseguito chat interattiva con il modello personalizzato tramite CLI |
| 6 | Interrogato il modello tramite REST API compatibile OpenAI |
| 7 | Connesso da Python, JavaScript e C# usando l’OpenAI SDK |
| 8 | Testato il modello personalizzato end-to-end con il Foundry Local SDK |

La cosa importante è che **qualsiasi modello basato su transformer può essere eseguito tramite Foundry Local** una volta compilato in formato ONNX. L’API compatibile OpenAI significa che tutto il codice applicativo esistente funziona senza modifiche; devi solo cambiare il nome del modello.

---

## Punti Chiave

| Concetto | Dettaglio |
|----------|-----------|
| ONNX Runtime GenAI Model Builder | Converte modelli Hugging Face in formato ONNX con quantizzazione in un unico comando |
| Formato ONNX | Foundry Local richiede modelli ONNX con configurazione ONNX Runtime GenAI |
| Template chat | Il file `inference_model.json` dice a Foundry Local come formattare i prompt per un dato modello |
| Destinazioni hardware | Compila per CPU, GPU NVIDIA (CUDA), DirectML (Windows GPU) o WebGPU a seconda dell’hardware |
| Quantizzazione | Precisione inferiore (int4) riduce dimensioni e migliora velocità a scapito di precisione; fp16 mantiene alta qualità su GPU |
| Compatibilità API | Modelli personalizzati usano la stessa API compatibile OpenAI dei modelli integrati |
| Foundry Local SDK | Lo SDK gestisce avvio servizio, scoperta endpoint e caricamento modelli automaticamente sia per modelli di catalogo che personalizzati |

---

## Ulteriori Letture

| Risorsa | Link |
|---------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Guida modelli personalizzati Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Famiglia modelli Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Documentazione Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Prossimi Passi

Continua con [Parte 11: Chiamata a Strumenti con Modelli Locali](part11-tool-calling.md) per imparare come abilitare i tuoi modelli locali a chiamare funzioni esterne.

[← Parte 9: Trascrizione Vocale Whisper](part9-whisper-voice-transcription.md) | [Parte 11: Chiamata a Strumenti →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Disclaimer**:  
Questo documento è stato tradotto utilizzando il servizio di traduzione AI [Co-op Translator](https://github.com/Azure/co-op-translator). Sebbene ci impegniamo per l’accuratezza, si prega di notare che le traduzioni automatiche possono contenere errori o imprecisioni. Il documento originale nella sua lingua natale deve essere considerato la fonte autorevole. Per informazioni critiche, si raccomanda una traduzione professionale umana. Non siamo responsabili per eventuali incomprensioni o interpretazioni errate derivanti dall’uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->