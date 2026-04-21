![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Μέρος 9: Μεταγραφή Φωνής με Whisper και Foundry Local

> **Στόχος:** Χρησιμοποιήστε το μοντέλο OpenAI Whisper που τρέχει τοπικά μέσω Foundry Local για να μεταγράψετε αρχεία ήχου - ολοκληρωτικά στη συσκευή, χωρίς να απαιτείται σύννεφο.

## Επισκόπηση

Το Foundry Local δεν είναι μόνο για δημιουργία κειμένου· υποστηρίζει επίσης μοντέλα **μετατροπής ομιλίας σε κείμενο**. Σε αυτό το εργαστήριο θα χρησιμοποιήσετε το μοντέλο **OpenAI Whisper Medium** για να μεταγράψετε αρχεία ήχου εντελώς στον υπολογιστή σας. Αυτό είναι ιδανικό για σενάρια όπως η μεταγραφή κλήσεων εξυπηρέτησης πελατών Zava, η ηχογράφηση αξιολογήσεων προϊόντων ή οι συνεδρίες προγραμματισμού εργαστηρίων όπου τα δεδομένα ήχου δεν πρέπει ποτέ να φύγουν από τη συσκευή σας.


---

## Μαθησιακοί Στόχοι

Μέχρι το τέλος αυτού του εργαστηρίου θα μπορείτε να:

- Κατανοήσετε το μοντέλο ομιλίας-σε-κείμενο Whisper και τις δυνατότητές του
- Κατεβάσετε και εκτελέσετε το μοντέλο Whisper χρησιμοποιώντας το Foundry Local
- Μεταγράψετε αρχεία ήχου χρησιμοποιώντας το Foundry Local SDK σε Python, JavaScript και C#
- Δημιουργήσετε μια απλή υπηρεσία μεταγραφής που τρέχει εξολοκλήρου στη συσκευή
- Κατανοήσετε τις διαφορές μεταξύ μοντέλων συνομιλίας/κειμένου και μοντέλων ήχου στο Foundry Local

---

## Προαπαιτούμενα

| Απαίτηση | Λεπτομέρειες |
|-------------|---------|
| **Foundry Local CLI** | Έκδοση **0.8.101 ή νεότερη** (τα μοντέλα Whisper είναι διαθέσιμα από την έκδοση 0.8.101 και μετά) |
| **ΛΣ** | Windows 10/11 (x64 ή ARM64) |
| **Περιβάλλον γλώσσας** | **Python 3.9+** και/ή **Node.js 18+** και/ή **.NET 9 SDK** ([Κατέβασμα .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **Ολοκληρωμένα** | [Μέρος 1: Ξεκινώντας](part1-getting-started.md), [Μέρος 2: Foundry Local SDK Βαθιά Εξέταση](part2-foundry-local-sdk.md), και [Μέρος 3: SDK και APIs](part3-sdk-and-apis.md) |

> **Σημείωση:** Τα μοντέλα Whisper πρέπει να κατεβάζονται μέσω του **SDK** (όχι το CLI). Το CLI δεν υποστηρίζει το endpoint μεταγραφής ήχου. Ελέγξτε την έκδοσή σας με:
> ```bash
> foundry --version
> ```

---

## Έννοια: Πώς Λειτουργεί το Whisper με το Foundry Local

Το μοντέλο OpenAI Whisper είναι ένα μοντέλο γενικής χρήσης αναγνώρισης ομιλίας, εκπαιδευμένο σε ένα μεγάλο σύνολο δεδομένων με ποικίλο ήχο. Όταν εκτελείται μέσω του Foundry Local:

- Το μοντέλο εκτελείται **αποκλειστικά στην CPU σας** - δεν απαιτείται GPU
- Ο ήχος δεν φεύγει ποτέ από τη συσκευή σας - **πλήρης ιδιωτικότητα**
- Το Foundry Local SDK χειρίζεται τη λήψη και τη διαχείριση της προσωρινής μνήμης του μοντέλου
- **JavaScript και C#** παρέχουν ενσωματωμένο `AudioClient` στο SDK που χειρίζεται ολόκληρη τη διαδικασία μεταγραφής — δεν χρειάζεται χειροκίνητη ρύθμιση ONNX
- **Python** χρησιμοποιεί το SDK για τη διαχείριση μοντέλου και το ONNX Runtime για άμεση επεξεργασία των μοντέλων encoder/decoder ONNX

### Πώς Λειτουργεί η Οροφή (JavaScript και C#) — SDK AudioClient

1. Το **Foundry Local SDK** κατεβάζει και αποθηκεύει στην προσωρινή μνήμη το μοντέλο Whisper
2. `model.createAudioClient()` (JS) ή `model.GetAudioClientAsync()` (C#) δημιουργεί έναν `AudioClient`
3. `audioClient.transcribe(path)` (JS) ή `audioClient.TranscribeAudioAsync(path)` (C#) χειρίζεται εσωτερικά ολόκληρη τη διαδικασία — προεπεξεργασία ήχου, encoder, decoder, και αποκωδικοποίηση token
4. Ο `AudioClient` εκθέτει ιδιότητα `settings.language` (ορισμένη στο `"en"` για Αγγλικά) για σωστή καθοδήγηση της μεταγραφής

### Πώς Λειτουργεί η Οροφή (Python) — ONNX Runtime

1. Το **Foundry Local SDK** κατεβάζει και αποθηκεύει στην προσωρινή μνήμη τα αρχεία μοντέλου Whisper ONNX
2. Η **προεπεξεργασία ήχου** μετατρέπει τον ήχο WAV σε μελωδικό φάσμα (mel spectrogram) (80 mel bins x 3000 frames)
3. Ο **Encoder** επεξεργάζεται το mel spectrogram και παράγει κρυφές καταστάσεις και tensors κλειδιών/τιμών cross-attention
4. Ο **Decoder** τρέχει αυτοπαραγωγικά, δημιουργώντας από ένα token κάθε φορά μέχρι να παραγάγει token τέλους κειμένου
5. Ο **Tokeniser** αποκωδικοποιεί τους IDs των tokens πίσω σε αναγνώσιμο κείμενο

### Παραλλαγές Μοντέλου Whisper

| Ψευδώνυμο | Αναγνωριστικό Μοντέλου | Συσκευή | Μέγεθος | Περιγραφή |
|-----------|-----------------------|---------|---------|-----------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | Επιταχυνόμενη GPU (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | Βελτιστοποιημένη για CPU (συνιστάται για τα περισσότερα μηχανήματα) |

> **Σημείωση:** Σε αντίθεση με τα μοντέλα συνομιλίας που εμφανίζονται από προεπιλογή, τα μοντέλα Whisper κατατάσσονται στην εργασία `automatic-speech-recognition`. Χρησιμοποιήστε `foundry model info whisper-medium` για λεπτομέρειες.

---

## Ασκήσεις Εργαστηρίου

### Άσκηση 0 - Απόκτηση Δείγματος Αρχείων Ήχου

Αυτό το εργαστήριο περιλαμβάνει προεγκατεστημένα αρχεία WAV βασισμένα σε σενάρια προϊόντων Zava DIY. Δημιουργήστε τα με το συμπεριλαμβανόμενο script:

```bash
# Από τη ρίζα του αποθετηρίου - δημιουργήστε και ενεργοποιήστε πρώτα ένα .venv
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

Αυτό δημιουργεί έξι αρχεία WAV στο `samples/audio/`:

| Αρχείο | Σενάριο |
|--------|---------|
| `zava-customer-inquiry.wav` | Πελάτης ρωτά για το **Zava ProGrip Cordless Drill** |
| `zava-product-review.wav` | Πελάτης αξιολογεί την **Zava UltraSmooth Εσωτερική Βαφή** |
| `zava-support-call.wav` | Κλήση υποστήριξης για το **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | Χρήστης σχεδιάζει ένα κατάστρωμα με **Zava EcoBoard Composite Decking** |
| `zava-workshop-setup.wav` | Περιήγηση σε εργαστήριο με **όλα τα πέντε προϊόντα Zava** |
| `zava-full-project-walkthrough.wav` | Εκτενής περιήγηση ανακαίνισης γκαράζ με **όλα τα προϊόντα Zava** (~4 λεπτά, για δοκιμές μεγάλου ήχου) |

> **Συμβουλή:** Μπορείτε επίσης να χρησιμοποιήσετε τα δικά σας αρχεία WAV/MP3/M4A ή να κάνετε ηχογράφηση με το Windows Voice Recorder.

---

### Άσκηση 1 - Κατέβασμα του Μοντέλου Whisper με το SDK

Λόγω ασυμβατότητας του CLI με τα μοντέλα Whisper σε νεότερες εκδόσεις Foundry Local, χρησιμοποιήστε το **SDK** για το κατέβασμα και φόρτωση του μοντέλου. Επιλέξτε τη γλώσσα σας:

<details>
<summary><b>🐍 Python</b></summary>

**Εγκατάσταση SDK:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# Ξεκινήστε την υπηρεσία
manager = FoundryLocalManager()
manager.start_service()

# Ελέγξτε τις πληροφορίες του καταλόγου
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# Ελέγξτε αν έχει ήδη αποθηκευτεί στην προσωρινή μνήμη
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# Φορτώστε το μοντέλο στη μνήμη
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

Αποθηκεύστε ως `download_whisper.py` και τρέξτε:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**Εγκατάσταση SDK:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// Δημιουργήστε διαχειριστή και ξεκινήστε την υπηρεσία
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Λάβετε το μοντέλο από τον κατάλογο
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.id}`);

if (model.isCached) {
  console.log("Whisper model already downloaded.");
} else {
  console.log("Downloading Whisper model (this may take several minutes)...");
  await model.download();
  console.log("Download complete.");
}

// Φορτώστε το μοντέλο στη μνήμη
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

Αποθηκεύστε ως `download-whisper.mjs` και τρέξτε:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**Εγκατάσταση SDK:**
```bash
dotnet add package Microsoft.AI.Foundry.Local
```

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "whisper-medium";

// Start the service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);
Console.WriteLine($"Model: {model.Id}");

// Check if already cached
var isCached = await model.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine("Whisper model already downloaded.");
}
else
{
    Console.WriteLine("Downloading Whisper model (this may take several minutes)...");
    await model.DownloadAsync(null, default);
    Console.WriteLine("Download complete.");
}

// Load the model into memory
await model.LoadAsync(default);
Console.WriteLine($"Whisper model loaded: {model.Id}");
```

</details>

> **Γιατί SDK αντί CLI;** Το Foundry Local CLI δεν υποστηρίζει απευθείας λήψη ή διαχείριση μοντέλων Whisper. Το SDK προσφέρει έναν αξιόπιστο τρόπο για να κατεβάσετε και να διαχειριστείτε μοντέλα ήχου προγραμματιστικά. Τα SDK JavaScript και C# περιλαμβάνουν ενσωματωμένο `AudioClient` που χειρίζεται ολόκληρη τη διαδικασία μεταγραφής. Η Python χρησιμοποιεί το ONNX Runtime για άμεση επεξεργασία έναντι των αποθηκευμένων μοντέλων.

---

### Άσκηση 2 - Κατανόηση του Whisper SDK

Η μεταγραφή με το Whisper χρησιμοποιεί διαφορετικές προσεγγίσεις ανά γλώσσα. Οι **JavaScript και C#** παρέχουν ενσωματωμένο `AudioClient` στο Foundry Local SDK που χειρίζεται όλη τη διαδικασία (προεπεξεργασία ήχου, encoder, decoder, αποκωδικοποίηση token) με μία ενιαία κλήση. Η **Python** χρησιμοποιεί το Foundry Local SDK για διαχείριση μοντέλου και ONNX Runtime για άμεση επεξεργασία των μοντέλων encoder/decoder ONNX.

| Συστατικό | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK πακέτα** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **Διαχείριση μοντέλου** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + κατάλογος |
| **Εξαγωγή χαρακτηριστικών** | `WhisperFeatureExtractor` + `librosa` | Χειρίζεται το SDK `AudioClient` | Χειρίζεται το SDK `AudioClient` |
| **Εκτίμηση** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Αποκωδικοποίηση token** | `WhisperTokenizer` | Χειρίζεται το SDK `AudioClient` | Χειρίζεται το SDK `AudioClient` |
| **Ρύθμιση γλώσσας** | Ορίζεται μέσω `forced_ids` στα tokens του decoder | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Είσοδος** | Μονοπάτι αρχείου WAV | Μονοπάτι αρχείου WAV | Μονοπάτι αρχείου WAV |
| **Έξοδος** | Κείμενο μεταγραφής | `result.text` | `result.Text` |

> **Σημαντικό:** Πάντα ορίστε τη γλώσσα στο `AudioClient` (π.χ. `"en"` για Αγγλικά). Χωρίς ρητό ορισμό γλώσσας, το μοντέλο μπορεί να παράγει ασαφές κείμενο προσπαθώντας να ανιχνεύσει αυτόματα τη γλώσσα.

> **Πρότυπα SDK:** Η Python χρησιμοποιεί `FoundryLocalManager(alias)` για αρχικοποίηση και μετά `get_cache_location()` για να βρει τα αρχεία μοντέλου ONNX. Η JavaScript και η C# χρησιμοποιούν τον ενσωματωμένο `AudioClient` — μέσω `model.createAudioClient()` (JS) ή `model.GetAudioClientAsync()` (C#) — που χειρίζεται ολόκληρη την οροφή. Δείτε [Μέρος 2: Foundry Local SDK Βαθιά Εξέταση](part2-foundry-local-sdk.md) για πλήρεις λεπτομέρειες.

---

### Άσκηση 3 - Δημιουργία Απλής Εφαρμογής Μεταγραφής

Επιλέξτε τη γλώσσα σας και δημιουργήστε μια μίνιμαλ εφαρμογή που μεταγράφει ένα αρχείο ήχου.

> **Υποστηριζόμενα φορμά ήχου:** WAV, MP3, M4A. Για καλύτερα αποτελέσματα, χρησιμοποιήστε αρχεία WAV με δείγμα 16kHz.

<details>
<summary><h3>Τραμ Python</h3></summary>

#### Ρύθμιση

```bash
cd python
python -m venv venv

# Ενεργοποιήστε το εικονικό περιβάλλον:
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### Κώδικας Μεταγραφής

Δημιουργήστε ένα αρχείο `foundry-local-whisper.py`:

```python
import sys
import os
import numpy as np
import onnxruntime as ort
import librosa
from transformers import WhisperFeatureExtractor, WhisperTokenizer
from foundry_local import FoundryLocalManager

model_alias = "whisper-medium"
audio_file = sys.argv[1] if len(sys.argv) > 1 else "sample.wav"

if not os.path.exists(audio_file):
    print(f"Audio file not found: {audio_file}")
    sys.exit(1)

# Βήμα 1: Bootstrap - ξεκινά την υπηρεσία, κατεβάζει και φορτώνει το μοντέλο
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# Δημιουργία διαδρομής προς τα αποθηκευμένα αρχεία μοντέλου ONNX
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# Βήμα 2: Φόρτωση συνεδριών ONNX και εξαγωγέα χαρακτηριστικών
encoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_encoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
decoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_decoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
fe = WhisperFeatureExtractor.from_pretrained(model_dir)
tokenizer = WhisperTokenizer.from_pretrained(model_dir)

# Βήμα 3: Εξαγωγή χαρακτηριστικών μελωδικού φασματογραφήματος
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# Βήμα 4: Εκτέλεση κωδικοποιητή
enc_out = encoder.run(None, {"audio_features": input_features})
# Η πρώτη έξοδος είναι κρυφές καταστάσεις· οι υπόλοιπες είναι ζεύγη KV διασταυρωμένης προσοχής
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# Βήμα 5: Αυτοπαραγωγικός αποκωδικοποιητής
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, μεταγραφή, χωρίς χρονικές σημάνσεις
input_ids = np.array([initial_tokens], dtype=np.int32)

# Άδειο κρυφό αποθηκευτικό της αυτό-προσοχής KV
self_kv = {}
for i in range(24):
    self_kv[f"past_key_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)
    self_kv[f"past_value_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)

generated = []
for _ in range(448):
    feeds = {"input_ids": input_ids, **cross_kv, **self_kv}
    outputs = decoder.run(None, feeds)
    logits = outputs[0]
    next_token = int(np.argmax(logits[0, -1, :]))

    if next_token == 50257:  # τέλος κειμένου
        break
    generated.append(next_token)

    # Ενημέρωση κρυφού αποθηκευτικού της αυτό-προσοχής KV
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### Εκτέλεση

```bash
# Μεταγράψτε ένα σενάριο προϊόντος Zava
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# Ή δοκιμάστε άλλα:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Κύρια Σημεία Python

| Μέθοδος | Σκοπός |
|---------|---------|
| `FoundryLocalManager(alias)` | Εκκίνηση: εκκίνηση υπηρεσίας, κατέβασμα και φόρτωση μοντέλου |
| `manager.get_cache_location()` | Λήψη μονοπατιού για τα αρχεία ONNX μοντέλου στην cache |
| `WhisperFeatureExtractor.from_pretrained()` | Φόρτωση του εναγωγού mel spectrogram |
| `ort.InferenceSession()` | Δημιουργία συνεδριών ONNX Runtime για encoder και decoder |
| `tokenizer.decode()` | Μετατροπή των IDs των output tokens σε κείμενο |

</details>

<details>
<summary><h3>Τραμ JavaScript</h3></summary>

#### Ρύθμιση

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### Κώδικας Μεταγραφής

Δημιουργήστε ένα αρχείο `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// Βήμα 1: Εκκίνηση - δημιουργία διαχειριστή, εκκίνηση υπηρεσίας και φόρτωση του μοντέλου
console.log(`Initialising Foundry Local with model: ${modelAlias}...`);
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);

if (!model.isCached) {
  console.log("Downloading Whisper model...");
  await model.download();
}
await model.load();

// Βήμα 2: Δημιουργία πελάτη ήχου και απομαγνητοφώνηση
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// Καθαρισμός
await model.unload();
```

> **Σημείωση:** Το Foundry Local SDK παρέχει ενσωματωμένο `AudioClient` μέσω `model.createAudioClient()` που χειρίζεται εσωτερικά ολόκληρη την οροφή ONNX — δεν χρειάζεται εισαγωγή `onnxruntime-node`. Πάντα ορίστε `audioClient.settings.language = "en"` για ακριβή μεταγραφή στα Αγγλικά.

#### Εκτέλεση

```bash
# Μεταγράψτε ένα σενάριο προϊόντος Zava
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# Ή δοκιμάστε άλλα:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### Κύρια Σημεία JavaScript

| Μέθοδος | Σκοπός |
|---------|---------|
| `FoundryLocalManager.create({ appName })` | Δημιουργία singleton διαχειριστή |
| `await catalog.getModel(alias)` | Λήψη μοντέλου από τον κατάλογο |
| `model.download()` / `model.load()` | Λήψη και φόρτωση του μοντέλου Whisper |
| `model.createAudioClient()` | Δημιουργία audio client για μεταγραφή |
| `audioClient.settings.language = "en"` | Ρύθμιση γλώσσας μεταγραφής (απαραίτητο για ακρίβεια) |
| `audioClient.transcribe(path)` | Μεταγραφή αρχείου ήχου, επιστρέφει `{ text, duration }` |

</details>

<details>
<summary><h3>Τραμ C#</h3></summary>

#### Ρύθμιση

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **Σημείωση:** Η διαδρομή C# χρησιμοποιεί το πακέτο `Microsoft.AI.Foundry.Local` που παρέχει ενσωματωμένο `AudioClient` μέσω `model.GetAudioClientAsync()`. Χειρίζεται ολόκληρη τη διαδικασία μεταγραφής στην ίδια διαδικασία — δεν απαιτείται ξεχωριστή ρύθμιση ONNX Runtime.

#### Κώδικας Μεταγραφής

Αντικαταστήστε το περιεχόμενο του `Program.cs`:

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// --- Configuration ---
var modelAlias = "whisper-medium";
var audioFile = args.Length > 0 ? args[0] : "sample.wav";

if (!File.Exists(audioFile))
{
    Console.WriteLine($"Audio file not found: {audioFile}");
    Console.WriteLine("Usage: dotnet run <path-to-audio-file>");
    return;
}

// --- Step 1: Initialize Foundry Local ---
Console.WriteLine("Initializing Foundry Local...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// --- Step 2: Load the Whisper model ---
Console.WriteLine($"Loading model: {modelAlias}...");
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading model...");
    await model.DownloadAsync(null, default);
}

// Load model into memory
Console.WriteLine("Loading model into memory...");
await model.LoadAsync(default);

// --- Step 3: Transcribe audio ---
Console.WriteLine($"Transcribing: {audioFile}");

var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en";

var result = await audioClient.TranscribeAudioAsync(audioFile);

Console.WriteLine("\n--- Transcription ---");
Console.WriteLine(result.Text);
Console.WriteLine("---------------------");
```

#### Εκτέλεση

```bash
# Μεταγράψτε ένα σενάριο προϊόντος Zava
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# Ή δοκιμάστε άλλα:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### Κύρια Σημεία C#

| Μέθοδος | Σκοπός |
|---------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Αρχικοποίηση Foundry Local με ρύθμιση |
| `catalog.GetModelAsync(alias)` | Λήψη μοντέλου από κατάλογο |
| `model.DownloadAsync()` | Λήψη του μοντέλου Whisper |
| `model.GetAudioClientAsync()` | Λήψη AudioClient (όχι ChatClient!) |
| `audioClient.Settings.Language = "en"` | Ρύθμιση γλώσσας μεταγραφής (απαραίτητο για ακρίβεια) |
| `audioClient.TranscribeAudioAsync(path)` | Μεταγραφή αρχείου ήχου |
| `result.Text` | Το μεταγραμμένο κείμενο |
> **C# vs Python/JS:** Το C# SDK παρέχει έναν ενσωματωμένο `AudioClient` για μεταγραφή εντός της διαδικασίας μέσω `model.GetAudioClientAsync()`, παρόμοια με το JavaScript SDK. Η Python χρησιμοποιεί απευθείας το ONNX Runtime για την εκτέλεση προβλέψεων στα αποθηκευμένα μοντέλα encoder/decoder.

</details>

---

### Άσκηση 4 - Ομαδική Μεταγραφή Όλων των Δειγμάτων Zava

Τώρα που έχετε μια λειτουργική εφαρμογή μεταγραφής, μεταγράψτε όλα τα πέντε δείγματα αρχείων Zava και συγκρίνετε τα αποτελέσματα.

<details>
<summary><h3>Python Track</h3></summary>

Το πλήρες δείγμα `python/foundry-local-whisper.py` υποστηρίζει ήδη ομαδική μεταγραφή. Όταν εκτελείται χωρίς ορίσματα, μεταγράφει όλα τα αρχεία `zava-*.wav` στο `samples/audio/`:

```bash
cd python
python foundry-local-whisper.py
```

Το δείγμα χρησιμοποιεί `FoundryLocalManager(alias)` για εκκίνηση, στη συνέχεια εκτελεί τις συνεδρίες encoder και decoder ONNX για κάθε αρχείο.

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

Το πλήρες δείγμα `javascript/foundry-local-whisper.mjs` υποστηρίζει ήδη ομαδική μεταγραφή. Όταν εκτελείται χωρίς ορίσματα, μεταγράφει όλα τα αρχεία `zava-*.wav` στο `samples/audio/`:

```bash
cd javascript
node foundry-local-whisper.mjs
```

Το δείγμα χρησιμοποιεί `FoundryLocalManager.create()` και `catalog.getModel(alias)` για την αρχικοποίηση του SDK, στη συνέχεια χρησιμοποιεί τον `AudioClient` (με `settings.language = "en"`) για να μεταγράψει κάθε αρχείο.

</details>

<details>
<summary><h3>C# Track</h3></summary>

Το πλήρες δείγμα `csharp/WhisperTranscription.cs` υποστηρίζει ήδη ομαδική μεταγραφή. Όταν εκτελείται χωρίς συγκεκριμένο όρισμα αρχείου, μεταγράφει όλα τα αρχεία `zava-*.wav` στο `samples/audio/`:

```bash
cd csharp
dotnet run whisper
```

Το δείγμα χρησιμοποιεί `FoundryLocalManager.CreateAsync()` και τον `AudioClient` του SDK (με `Settings.Language = "en"`) για μεταγραφή εντός της διαδικασίας.

</details>

**Τι να προσέξετε:** Συγκρίνετε το αποτέλεσμα της μεταγραφής με το αρχικό κείμενο στο `samples/audio/generate_samples.py`. Πόσο ακριβώς καταγράφει το Whisper ονόματα προϊόντων όπως το "Zava ProGrip" και τεχνικούς όρους όπως "brushless motor" ή "composite decking";

---

### Άσκηση 5 - Κατανόηση Κύριων Προτύπων Κώδικα

Μελετήστε πώς η μεταγραφή Whisper διαφέρει από τις συμπληρώσεις συνομιλίας στις τρεις γλώσσες:

<details>
<summary><b>Python - Κύριες Διαφορές από τη Συνομιλία</b></summary>

```python
# Ολοκλήρωση συνομιλίας (Μέρη 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# Μεταγραφή ήχου (Αυτό το μέρος):
# Χρησιμοποιεί άμεσα το ONNX Runtime αντί για τον πελάτη OpenAI
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... βρόχος αποκωδικοποιητή αυτοπαλινδρόμησης ...
print(tokenizer.decode(generated_tokens))
```

**Βασικό εύρημα:** Τα μοντέλα συνομιλίας χρησιμοποιούν το API συμβατό με OpenAI μέσω του `manager.endpoint`. Το Whisper χρησιμοποιεί το SDK για τον εντοπισμό των αποθηκευμένων αρχείων μοντέλων ONNX και στη συνέχεια τρέχει απευθείας την εκτέλεση με ONNX Runtime.

</details>

<details>
<summary><b>JavaScript - Κύριες Διαφορές από τη Συνομιλία</b></summary>

```javascript
// Συμπλήρωση συνομιλίας (Μέρη 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// Μεταγραφή ήχου (Αυτό το Μέρος):
// Χρησιμοποιεί τον ενσωματωμένο AudioClient του SDK
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // Ρυθμίστε πάντα τη γλώσσα για τα καλύτερα αποτελέσματα
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**Βασικό εύρημα:** Τα μοντέλα συνομιλίας χρησιμοποιούν το API συμβατό με OpenAI μέσω `manager.urls[0] + "/v1"`. Η μεταγραφή Whisper χρησιμοποιεί τον `AudioClient` του SDK, ο οποίος λαμβάνεται από `model.createAudioClient()`. Ορίστε τη γλώσσα στα `settings.language` για να αποφύγετε κατεστραμμένη έξοδο από αυτόματη ανίχνευση.

</details>

<details>
<summary><b>C# - Κύριες Διαφορές από τη Συνομιλία</b></summary>

Η προσέγγιση C# χρησιμοποιεί τον ενσωματωμένο `AudioClient` του SDK για μεταγραφή εντός της διαδικασίας:

**Αρχικοποίηση μοντέλου:**

```csharp
// 1. Create the manager with configuration
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// 2. Get model from catalog, download, and load
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("whisper-medium", default);
await model.DownloadAsync(null, default);
await model.LoadAsync(default);
```

**Μεταγραφή:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**Βασικό εύρημα:** Η C# χρησιμοποιεί `FoundryLocalManager.CreateAsync()` και λαμβάνει απευθείας `AudioClient` — δεν απαιτείται ρύθμιση του ONNX Runtime. Ορίστε `Settings.Language` για να αποφύγετε κατεστραμμένη έξοδο από αυτόματη ανίχνευση.

</details>

> **Περίληψη:** Η Python χρησιμοποιεί το Foundry Local SDK για διαχείριση μοντέλων και το ONNX Runtime για απευθείας εκτέλεση στα μοντέλα encoder/decoder. Το JavaScript και το C# χρησιμοποιούν τον ενσωματωμένο `AudioClient` του SDK για απλοποιημένη μεταγραφή — δημιουργήστε τον πελάτη, ορίστε τη γλώσσα και καλέστε `transcribe()` / `TranscribeAudioAsync()`. Πάντα ορίστε την ιδιότητα γλώσσας στον AudioClient για ακριβή αποτελέσματα.

---

### Άσκηση 6 - Πειραματισμός

Δοκιμάστε αυτές τις τροποποιήσεις για να εμβαθύνετε στην κατανόησή σας:

1. **Δοκιμάστε διαφορετικά αρχεία ήχου** - ηχογραφήστε τον εαυτό σας χρησιμοποιώντας το Windows Voice Recorder, αποθηκεύστε ως WAV και μεταγράψτε το

2. **Συγκρίνετε παραλλαγές μοντέλων** - αν έχετε NVIDIA GPU, δοκιμάστε την παραλλαγή CUDA:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   Συγκρίνετε την ταχύτητα μεταγραφής με την παραλλαγή CPU.

3. **Προσθέστε μορφοποίηση εξόδου** - η απάντηση JSON μπορεί να περιλαμβάνει:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **Δημιουργήστε ένα REST API** - ενσωματώστε τον κώδικα μεταγραφής σε έναν web server:

   | Γλώσσα | Πλαίσιο | Παράδειγμα |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` με `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` με `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` με `IFormFile` |

5. **Πολλαπλός γύρος με μεταγραφή** - συνδυάστε το Whisper με έναν πράκτορα συνομιλίας από το Μέρος 4: μεταγράψτε πρώτα τον ήχο, στη συνέχεια περάστε το κείμενο στον πράκτορα για ανάλυση ή σύνοψη.

---

## Αναφορά Audio API SDK

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — δημιουργεί μια παρουσία `AudioClient`
> - `audioClient.settings.language` — ορίζει τη γλώσσα μεταγραφής (π.χ. `"en"`)
> - `audioClient.settings.temperature` — ελέγχει την τυχαιότητα (προαιρετικό)
> - `audioClient.transcribe(filePath)` — μεταγράφει ένα αρχείο, επιστρέφει `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — μεταφέρει τμήματα μεταγραφής μέσω callback
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — δημιουργεί μια παρουσία `OpenAIAudioClient`
> - `audioClient.Settings.Language` — ορίζει τη γλώσσα μεταγραφής (π.χ. `"en"`)
> - `audioClient.Settings.Temperature` — ελέγχει την τυχαιότητα (προαιρετικό)
> - `await audioClient.TranscribeAudioAsync(filePath)` — μεταγράφει ένα αρχείο, επιστρέφει αντικείμενο με `.Text`
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — επιστρέφει `IAsyncEnumerable` με τμήματα μεταγραφής

> **Συμβουλή:** Πάντα ορίστε την ιδιότητα γλώσσας πριν από τη μεταγραφή. Χωρίς αυτήν, το μοντέλο Whisper επιχειρεί αυτόματη ανίχνευση, που μπορεί να παράγει κατεστραμμένη έξοδο (ένα μεμονωμένο σύμβολο αντικατάστασης αντί κειμένου).

---

## Σύγκριση: Μοντέλα Συνομιλίας vs. Whisper

| Χαρακτηριστικό | Μοντέλα Συνομιλίας (Μέρη 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **Τύπος εργασίας** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **Εισαγωγή** | Κείμενο μηνυμάτων (JSON) | Αρχεία ήχου (WAV/MP3/M4A) | Αρχεία ήχου (WAV/MP3/M4A) |
| **Έξοδος** | Παραγόμενο κείμενο (σε ροή) | Μεταγραμμένο κείμενο (ολοκληρωμένο) | Μεταγραμμένο κείμενο (ολοκληρωμένο) |
| **Πακέτο SDK** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **Μέθοδος API** | `client.chat.completions.create()` | ONNX Runtime απευθείας | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **Ρύθμιση γλώσσας** | Μη διαθέσιμη | Decoder prompt tokens | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **Streaming** | Ναι | Όχι | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **Πλεονέκτημα ιδιωτικότητας** | Κώδικας/δεδομένα μένουν τοπικά | Τα δεδομένα ήχου μένουν τοπικά | Τα δεδομένα ήχου μένουν τοπικά |

---

## Βασικά Συμπεράσματα

| Έννοια | Τι Μάθατε |
|---------|-----------------|
| **Whisper στο συσκευή** | Η μετατροπή ομιλίας σε κείμενο τρέχει πλήρως τοπικά, ιδανικό για μεταγραφή κλήσεων πελατών Zava και κριτικές προϊόντων στη συσκευή |
| **SDK AudioClient** | Τα SDK JavaScript και C# παρέχουν ενσωματωμένο `AudioClient` που διαχειρίζεται όλη την αλυσίδα μεταγραφής με μία κλήση |
| **Ρύθμιση γλώσσας** | Πάντα ορίστε τη γλώσσα στον AudioClient (π.χ. `"en"`) — αλλιώς η αυτόματη ανίχνευση μπορεί να δώσει κατεστραμμένη έξοδο |
| **Python** | Χρησιμοποιεί `foundry-local-sdk` για διαχείριση μοντέλων + `onnxruntime` + `transformers` + `librosa` για απευθείας ONNX πρόβλεψη |
| **JavaScript** | Χρησιμοποιεί `foundry-local-sdk` με `model.createAudioClient()` — ορίστε `settings.language`, μετά καλέστε `transcribe()` |
| **C#** | Χρησιμοποιεί `Microsoft.AI.Foundry.Local` με `model.GetAudioClientAsync()` — ορίστε `Settings.Language`, μετά καλέστε `TranscribeAudioAsync()` |
| **Υποστήριξη streaming** | Τα SDK JS και C# προσφέρουν επίσης `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` για τμηματική έξοδο |
| **Βελτιστοποιημένο για CPU** | Η παραλλαγή CPU (3.05 GB) λειτουργεί σε οποιαδήποτε συσκευή Windows χωρίς GPU |
| **Προστασία ιδιωτικότητας** | Τέλειο για διατήρηση των συνομιλιών πελατών Zava και ιδιόκτητων δεδομένων προϊόντων τοπικά |

---

## Πόροι

| Πόρος | Σύνδεσμος |
|----------|------|
| Τεκμηρίωση Foundry Local | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Αναφορά SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper μοντέλο | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Ιστότοπος Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

## Επόμενο Βήμα

Συνεχίστε στο [Μέρος 10: Χρήση Προσαρμοσμένων ή Hugging Face Μοντέλων](part10-custom-models.md) για να δημιουργήσετε τα δικά σας μοντέλα από το Hugging Face και να τα τρέξετε μέσω του Foundry Local.