![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# Μέρος 10: Χρήση Προσαρμοσμένων ή Hugging Face Μοντέλων με το Foundry Local

> **Στόχος:** Συγκεντρώστε ένα μοντέλο Hugging Face σε βελτιστοποιημένη μορφή ONNX που απαιτεί το Foundry Local, διαμορφώστε το με ένα πρότυπο συνομιλίας, προσθέστε το στην τοπική προσωρινή μνήμη και εκτελέστε συμπερασμό μέσω της CLI, REST API και OpenAI SDK.

## Επισκόπηση

Το Foundry Local συνοδεύεται από έναν επιμελημένο κατάλογο προ-συγκεντρωμένων μοντέλων, αλλά δεν περιορίζεστε σε αυτή τη λίστα. Οποιοδήποτε γλωσσικό μοντέλο βασισμένο σε transformer που είναι διαθέσιμο στο [Hugging Face](https://huggingface.co/) (ή αποθηκευμένο τοπικά σε μορφή PyTorch / Safetensors) μπορεί να μετατραπεί σε βελτιστοποιημένο μοντέλο ONNX και να υπηρετηθεί μέσω του Foundry Local.

Η διαδικασία συγκέντρωσης χρησιμοποιεί το **ONNX Runtime GenAI Model Builder**, ένα εργαλείο γραμμής εντολών που περιλαμβάνεται στο πακέτο `onnxruntime-genai`. Ο δημιουργός μοντέλων αναλαμβάνει το βασικό έργο: κατεβάζει τα αρχικά βάρη, τα μετατρέπει σε μορφή ONNX, εφαρμόζει ποσοτικοποίηση (int4, fp16, bf16) και παράγει τα αρχεία διαμόρφωσης (περιλαμβανομένου του προτύπου συνομιλίας και του tokenizer) που απαιτεί το Foundry Local.

Σε αυτό το εργαστήριο θα συγκεντρώσετε το **Qwen/Qwen3-0.6B** από το Hugging Face, θα το καταχωρήσετε στο Foundry Local και θα συνομιλήσετε μαζί του εξ ολοκλήρου στη συσκευή σας.

---

## Μαθησιακοί Στόχοι

Με το τέλος αυτού του εργαστηρίου θα μπορείτε να:

- Εξηγήσετε γιατί είναι χρήσιμη η προσαρμοσμένη συγκέντρωση μοντέλου και πότε μπορεί να τη χρειαστείτε
- Εγκαταστήσετε το ONNX Runtime GenAI model builder
- Συγκεντρώσετε ένα μοντέλο Hugging Face σε βελτιστοποιημένη μορφή ONNX με μία εντολή
- Κατανοήσετε τις βασικές παραμέτρους συγκέντρωσης (εκτελεστής, ακρίβεια)
- Δημιουργήσετε το αρχείο διαμόρφωσης προτύπου συνομιλίας `inference_model.json`
- Προσθέσετε ένα συγκεντρωμένο μοντέλο στην τοπική προσωρινή μνήμη Foundry Local
- Εκτελέσετε συμπερασμό με το προσαρμοσμένο μοντέλο μέσω CLI, REST API και OpenAI SDK

---

## Προαπαιτούμενα

| Απαίτηση | Λεπτομέρειες |
|-------------|---------|
| **Foundry Local CLI** | Εγκατεστημένο και στο `PATH` σας ([Μέρος 1](part1-getting-started.md)) |
| **Python 3.10+** | Απαραίτητο για το ONNX Runtime GenAI model builder |
| **pip** | Διαχειριστής πακέτων Python |
| **Χώρος δίσκου** | Τουλάχιστον 5 GB ελεύθερα για τα αρχεία μοντέλου πηγής και συγκεντρωμένου μοντέλου |
| **Λογαριασμός Hugging Face** | Ορισμένα μοντέλα απαιτούν αποδοχή αδείας πριν τη λήψη. Το Qwen3-0.6B χρησιμοποιεί την άδεια Apache 2.0 και είναι διαθέσιμο ελεύθερα. |

---

## Ρύθμιση Περιβάλλοντος

Η συγκέντρωση μοντέλων απαιτεί αρκετά μεγάλα πακέτα Python (PyTorch, ONNX Runtime GenAI, Transformers). Δημιουργήστε ένα αφιερωμένο εικονικό περιβάλλον ώστε αυτά να μην επηρεάζουν την Python του συστήματός σας ή άλλα έργα.

```bash
# Από τη ρίζα του αποθετηρίου
python -m venv .venv
```

Ενεργοποιήστε το περιβάλλον:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

Αναβαθμίστε το pip για να αποφύγετε προβλήματα επίλυσης εξαρτήσεων:

```bash
python -m pip install --upgrade pip
```

> **Συμβουλή:** Αν ήδη διαθέτετε ένα `.venv` από προηγούμενα εργαστήρια, μπορείτε να το χρησιμοποιήσετε ξανά. Απλώς βεβαιωθείτε ότι είναι ενεργοποιημένο πριν συνεχίσετε.

---

## Εννοιολογικό Πλαίσιο: Η Διαδικασία Συγκέντρωσης

Το Foundry Local απαιτεί μοντέλα σε μορφή ONNX με διαμόρφωση ONNX Runtime GenAI. Τα περισσότερα μοντέλα ανοικτού κώδικα στο Hugging Face διανέμονται ως βάρη PyTorch ή Safetensors, οπότε απαιτείται βήμα μετατροπής.

![Διαδικασία προσαρμοσμένης συγκέντρωσης μοντέλου](../../../images/custom-model-pipeline.svg)

### Τι Κάνει ο Δημιουργός Μοντέλων;

1. **Κατεβάζει** το αρχικό μοντέλο από το Hugging Face (ή το διαβάζει από τοπική διαδρομή).
2. **Μετατρέπει** τα βάρη PyTorch / Safetensors σε μορφή ONNX.
3. **Ποσοτικοποιεί** το μοντέλο σε μικρότερη ακρίβεια (π.χ. int4) για μείωση χρήσης μνήμης και βελτίωση απόδοσης.
4. **Παράγει** τη διαμόρφωση ONNX Runtime GenAI (`genai_config.json`), το πρότυπο συνομιλίας (`chat_template.jinja`) και όλα τα αρχεία tokenizer ώστε το Foundry Local να μπορεί να φορτώσει και να υπηρετήσει το μοντέλο.

### ONNX Runtime GenAI Model Builder vs Microsoft Olive

Μπορεί να συναντήσετε αναφορές στο **Microsoft Olive** ως εναλλακτικό εργαλείο βελτιστοποίησης μοντέλων. Και τα δύο εργαλεία μπορούν να παράγουν μοντέλα ONNX, αλλά έχουν διαφορετικούς σκοπούς και ιδιαιτερότητες:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Πακέτο** | `onnxruntime-genai` | `olive-ai` |
| **Κύριος σκοπός** | Μετατροπή και ποσοτικοποίηση γεννητικών AI μοντέλων για ONNX Runtime GenAI συμπερασμό | Ολοκληρωμένο πλαίσιο βελτιστοποίησης μοντέλων πολλαπλών βημάτων με υποστήριξη για πολλούς backends και στόχους υλικού |
| **Ευκολία χρήσης** | Εντολή μιας βήματος — μετατροπή + ποσοτικοποίηση | Ροές εργασίας — παραμετροποιήσιμες πολυβηματικές διαδικασίες με YAML/JSON |
| **Μορφή εξόδου** | Μορφή ONNX Runtime GenAI (έτοιμη για Foundry Local) | Γενική ONNX, ONNX Runtime GenAI ή άλλες, ανάλογα με τη ροή εργασίας |
| **Στόχοι υλικού** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, κ.ά. |
| **Επιλογές ποσοτικοποίησης** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, συν βελτιώσεις γράφων, ρύθμιση επιπέδου στρώματος |
| **Εύρος μοντέλων** | Γεννητικά μοντέλα AI (LLMs, SLMs) | Οποιοδήποτε μοντέλο μετατρέψιμο σε ONNX (όραση, NLP, ήχος, πολυτροπικό) |
| **Καλύτερο για** | Γρήγορη συγκέντρωση μοντέλου για τοπικό συμπερασμό | Παραγωγικές ροές με απαιτήσεις λεπτομερούς ελέγχου βελτιστοποίησης |
| **Εξαρτήσεις** | Μέτριες (PyTorch, Transformers, ONNX Runtime) | Μεγαλύτερες (περιλαμβάνει Olive framework, προαιρετικά extras ανά ροή εργασίας) |
| **Ενσωμάτωση Foundry Local** | Άμεση — συμβατή αμέσως | Απαιτεί την παράμετρο `--use_ort_genai` και επιπλέον διαμόρφωση |

> **Γιατί αυτό το εργαστήριο χρησιμοποιεί το Model Builder:** Για το έργο συγκέντρωσης ενός μοντέλου Hugging Face και καταχώρησής του στο Foundry Local, ο Model Builder είναι η απλούστερη και πιο αξιόπιστη επιλογή. Παράγει ακριβώς τη μορφή εξόδου που απαιτεί το Foundry Local με μία εντολή. Αν στο μέλλον χρειαστείτε προηγμένες λειτουργίες βελτιστοποίησης — όπως ποσοτικοποίηση με επίγνωση ακρίβειας, χειρισμό γράφων ή πολλαπλή ρύθμιση — το Olive είναι μια ισχυρή επιλογή για διερεύνηση. Δείτε την [τεκμηρίωση Microsoft Olive](https://microsoft.github.io/Olive/) για περισσότερες λεπτομέρειες.

---

## Ασκήσεις Εργαστηρίου

### Άσκηση 1: Εγκατάσταση του ONNX Runtime GenAI Model Builder

Εγκαταστήστε το πακέτο ONNX Runtime GenAI που περιλαμβάνει το εργαλείο δημιουργίας μοντέλων:

```bash
pip install onnxruntime-genai
```

Επαληθεύστε την εγκατάσταση ελέγχοντας ότι το εργαλείο δημιουργίας είναι διαθέσιμο:

```bash
python -m onnxruntime_genai.models.builder --help
```

Θα πρέπει να δείτε βοήθεια που απαριθμεί παραμέτρους όπως `-m` (όνομα μοντέλου), `-o` (διαδρομή εξόδου), `-p` (ακρίβεια), και `-e` (εκτελεστής).

> **Σημείωση:** Ο δημιουργός μοντέλων εξαρτάται από τα PyTorch, Transformers και άλλα πακέτα. Η εγκατάσταση μπορεί να διαρκέσει μερικά λεπτά.

---

### Άσκηση 2: Συγκέντρωση του Qwen3-0.6B για CPU

Εκτελέστε την ακόλουθη εντολή για να κατεβάσετε το μοντέλο Qwen3-0.6B από το Hugging Face και να το συγκεντρώσετε για συμπερασμό CPU με ποσοτικοποίηση int4:

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

#### Τι Κάνει Κάθε Παράμετρος

| Παράμετρος | Σκοπός | Χρησιμοποιούμενη Τιμή |
|-----------|---------|------------|
| `-m` | Το ID μοντέλου Hugging Face ή τοπική διαδρομή φακέλου | `Qwen/Qwen3-0.6B` |
| `-o` | Φάκελος όπου θα αποθηκευτεί το συγκεντρωμένο μοντέλο ONNX | `models/qwen3` |
| `-p` | Ακρίβεια ποσοτικοποίησης που εφαρμόστηκε κατά τη συγκέντρωση | `int4` |
| `-e` | Εκτελεστής ONNX Runtime (στοχευμένο υλικό) | `cpu` |
| `--extra_options hf_token=false` | Παράλειψη αυθεντικοποίησης Hugging Face (καλό για δημόσια μοντέλα) | `hf_token=false` |

> **Πόσος χρόνος χρειάζεται;** Ο χρόνος συγκέντρωσης εξαρτάται από το υλικό σας και το μέγεθος μοντέλου. Για το Qwen3-0.6B με int4 ποσοτικοποίηση σε σύγχρονη CPU, υπολογίστε περίπου 5 έως 15 λεπτά. Τα μεγαλύτερα μοντέλα χρειάζονται αναλογικά περισσότερο χρόνο.

Όταν ολοκληρωθεί η εντολή, θα πρέπει να δείτε έναν φάκελο `models/qwen3` που περιέχει τα συγκεντρωμένα αρχεία μοντέλου. Επαληθεύστε το αποτέλεσμα:

```bash
ls models/qwen3
```

Θα πρέπει να δείτε αρχεία όπως:
- `model.onnx` και `model.onnx.data` — τα βάρη του συγκεντρωμένου μοντέλου
- `genai_config.json` — διαμόρφωση ONNX Runtime GenAI
- `chat_template.jinja` — το πρότυπο συνομιλίας του μοντέλου (αυτόματα δημιουργημένο)
- `tokenizer.json`, `tokenizer_config.json` — αρχεία tokenizer
- Άλλα λεξιλογικά και αρχεία διαμόρφωσης

---

### Άσκηση 3: Συγκέντρωση για GPU (Προαιρετικό)

Αν διαθέτετε κάρτα NVIDIA GPU με υποστήριξη CUDA, μπορείτε να συγκεντρώσετε μια παραλλαγή βελτιστοποιημένη για GPU για γρηγορότερο συμπερασμό:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **Σημείωση:** Η συγκέντρωση GPU απαιτεί το `onnxruntime-gpu` και εγκατεστημένη σωστά CUDA. Αν αυτά λείπουν, ο δημιουργός μοντέλων θα εμφανίσει σφάλμα. Μπορείτε να παραλείψετε αυτή την άσκηση και να συνεχίσετε με την παραλλαγή CPU.

#### Αναφορά Εκτέλεσης ανά Υλικό

| Στόχος | Εκτελεστής (`-e`) | Συνιστώμενη ακρίβεια (`-p`) |
|--------|-------------------|-----------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` ή `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` ή `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Αντισταθμίσεις ακρίβειας

| Ακρίβεια | Μέγεθος | Ταχύτητα | Ποιότητα |
|-----------|------|-------|---------|
| `fp32` | Μεγαλύτερη | Πιο αργή | Υψηλότερη ακρίβεια |
| `fp16` | Μεγάλη | Γρήγορη (GPU) | Πολύ καλή ακρίβεια |
| `int8` | Μικρή | Γρήγορη | Μικρή απώλεια ακρίβειας |
| `int4` | Πιο μικρή | Πιο γρήγορη | Μέτρια απώλεια ακρίβειας |

Για την ανάπτυξη τοπικά, το `int4` σε CPU παρέχει την καλύτερη ισορροπία ταχύτητας και χρήσης πόρων. Για παραγωγική ποιότητα, συνιστάται το `fp16` σε CUDA GPU.

---

### Άσκηση 4: Δημιουργία της Διαμόρφωσης Προτύπου Συνομιλίας

Ο δημιουργός μοντέλων δημιουργεί αυτόματα το αρχείο `chat_template.jinja` και το `genai_config.json` στον φάκελο εξόδου. Ωστόσο, το Foundry Local χρειάζεται επίσης ένα αρχείο `inference_model.json` για να κατανοήσει πώς να μορφοποιεί τα προτροπές για το μοντέλο σας. Αυτό το αρχείο ορίζει το όνομα μοντέλου και το πρότυπο προτροπής που περικλείει τα μηνύματα των χρηστών με τους σωστούς ειδικούς χαρακτήρες.

#### Βήμα 1: Επιθεώρηση της Συγκεντρωμένης Έξοδου

Καταγράψτε τα περιεχόμενα του φακέλου συγκεντρωμένου μοντέλου:

```bash
ls models/qwen3
```

Θα πρέπει να δείτε αρχεία όπως:
- `model.onnx` και `model.onnx.data` — τα βάρη του συγκεντρωμένου μοντέλου
- `genai_config.json` — διαμόρφωση ONNX Runtime GenAI (αυτόματα δημιουργημένη)
- `chat_template.jinja` — το πρότυπο συνομιλίας του μοντέλου (αυτόματα δημιουργημένο)
- `tokenizer.json`, `tokenizer_config.json` — αρχεία tokenizer
- Διάφορα άλλα αρχεία διαμόρφωσης και λεξιλογίου

#### Βήμα 2: Δημιουργία του Αρχείου inference_model.json

Το αρχείο `inference_model.json` λέει στο Foundry Local πώς να μορφοποιεί τις προτροπές. Δημιουργήστε ένα σενάριο Python με όνομα `generate_chat_template.py` **στη ρίζα του αποθετηρίου** (τον ίδιο φάκελο που περιέχει το `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# Δημιουργήστε μια ελάχιστη συνομιλία για να εξαγάγετε το πρότυπο συνομιλίας
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

# Δημιουργήστε τη δομή inference_model.json
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

Εκτελέστε το σενάριο από τη ρίζα του αποθετηρίου:

```bash
python generate_chat_template.py
```

> **Σημείωση:** Το πακέτο `transformers` είχε ήδη εγκατασταθεί ως εξάρτηση του `onnxruntime-genai`. Αν δείτε σφάλμα `ImportError`, εκτελέστε πρώτα `pip install transformers`.

Το σενάριο παράγει ένα αρχείο `inference_model.json` μέσα στον φάκελο `models/qwen3`. Αυτό το αρχείο λέει στο Foundry Local πώς να περικλείει την είσοδο χρήστη με τους σωστούς ειδικούς χαρακτήρες για το Qwen3.

> **Σημαντικό:** Το πεδίο `"Name"` στο `inference_model.json` (ορισμένο σε `qwen3-0.6b` σε αυτό το σενάριο) είναι το **ψευδώνυμο μοντέλου** που θα χρησιμοποιείτε σε όλες τις επόμενες εντολές και κλήσεις API. Αν αλλάξετε αυτό το όνομα, ενημερώστε το όνομα μοντέλου στις Ασκήσεις 6–10 ανάλογα.

#### Βήμα 3: Επαλήθευση της Διαμόρφωσης

Ανοίξτε το `models/qwen3/inference_model.json` και επιβεβαιώστε ότι περιέχει πεδίο `Name` και αντικείμενο `PromptTemplate` με κλειδιά `assistant` και `prompt`. Το πρότυπο προτροπής θα πρέπει να περιλαμβάνει ειδικούς χαρακτήρες όπως `<|im_start|>` και `<|im_end|>` (οι ακριβείς χαρακτήρες εξαρτώνται από το πρότυπο συνομιλίας του μοντέλου).

> **Εναλλακτική χειροκίνητη μέθοδος:** Αν δεν θέλετε να εκτελέσετε το σενάριο, μπορείτε να δημιουργήσετε το αρχείο χειροκίνητα. Η βασική απαίτηση είναι ότι το πεδίο `prompt` να περιέχει ολόκληρο το πρότυπο συνομιλίας του μοντέλου με το `{Content}` ως θέση για το μήνυμα του χρήστη.

---

### Άσκηση 5: Επαλήθευση της Δομής Φακέλου Μοντέλου
Ο κατασκευαστής μοντέλου τοποθετεί όλα τα μεταγλωττισμένα αρχεία απευθείας στον κατάλογο εξόδου που ορίσατε. Επαληθεύστε ότι η τελική δομή φαίνεται σωστή:

```bash
ls models/qwen3
```

Ο κατάλογος θα πρέπει να περιέχει τα ακόλουθα αρχεία:

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

> **Σημείωση:** Σε αντίθεση με κάποια άλλα εργαλεία μεταγλώττισης, ο κατασκευαστής μοντέλων δεν δημιουργεί εμφωλευμένους υποφακέλους. Όλα τα αρχεία βρίσκονται απευθείας στο φάκελο εξόδου, που ακριβώς είναι αυτό που περιμένει το Foundry Local.

---

### Άσκηση 6: Προσθήκη του Μοντέλου στην Cache του Foundry Local

Πείτε στο Foundry Local πού να βρει το μεταγλωττισμένο σας μοντέλο προσθέτοντας τον κατάλογο στην cache του:

```bash
foundry cache cd models/qwen3
```

Επαληθεύστε ότι το μοντέλο εμφανίζεται στην cache:

```bash
foundry cache ls
```

Θα πρέπει να δείτε το προσαρμοσμένο μοντέλο σας στη λίστα μαζί με οποιαδήποτε προηγούμενα αποθηκευμένα μοντέλα (όπως `phi-3.5-mini` ή `phi-4-mini`).

---

### Άσκηση 7: Εκτέλεση του Προσαρμοσμένου Μοντέλου μέσω CLI

Ξεκινήστε μια διαδραστική συνεδρία συνομιλίας με το πρόσφατα μεταγλωττισμένο μοντέλο σας (το ψευδώνυμο `qwen3-0.6b` προέρχεται από το πεδίο `Name` που ορίσατε στο `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

Η σημαία `--verbose` εμφανίζει επιπλέον διαγνωστικές πληροφορίες, κάτι που βοηθά στη δοκιμή ενός προσαρμοσμένου μοντέλου για πρώτη φορά. Αν το μοντέλο φορτωθεί επιτυχώς, θα δείτε ένα διαδραστικό περιβάλλον. Δοκιμάστε μερικά μηνύματα:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

Πληκτρολογήστε `exit` ή πατήστε `Ctrl+C` για να τερματίσετε τη συνεδρία.

> **Επίλυση Προβλημάτων:** Αν το μοντέλο δεν φορτώνει, ελέγξτε τα εξής:
> - Το αρχείο `genai_config.json` δημιουργήθηκε από τον κατασκευαστή μοντέλου.
> - Το αρχείο `inference_model.json` υπάρχει και είναι έγκυρο JSON.
> - Τα αρχεία μοντέλου ONNX βρίσκονται στον σωστό κατάλογο.
> - Διαθέτετε επαρκή μνήμη RAM (το Qwen3-0.6B int4 χρειάζεται περίπου 1 GB).
> - Το Qwen3 είναι μοντέλο συλλογιστικής που παράγει ετικέτες `<think>`. Αν βλέπετε `<think>...</think>` στην αρχή των απαντήσεων, αυτή είναι φυσιολογική συμπεριφορά. Το πρότυπο προτροπής στο `inference_model.json` μπορεί να προσαρμοστεί για να καταστείλει την έξοδο σκέψης.

---

### Άσκηση 8: Ερώτημα στο Προσαρμοσμένο Μοντέλο μέσω REST API

Αν τερματίσατε τη διαδραστική συνεδρία στην Άσκηση 7, το μοντέλο ενδέχεται να μην είναι πλέον φορτωμένο. Ξεκινήστε πρώτα την υπηρεσία Foundry Local και φορτώστε το μοντέλο:

```bash
foundry service start
foundry model load qwen3-0.6b
```

Ελέγξτε σε ποια θύρα εκτελείται η υπηρεσία:

```bash
foundry service status
```

Μετά αποστείλετε ένα αίτημα (αντικαταστήστε το `5273` με την πραγματική θύρα αν διαφέρει):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Σημείωση για Windows:** Η εντολή `curl` παραπάνω χρησιμοποιεί σύνταξη bash. Σε Windows, χρησιμοποιήστε το cmdlet PowerShell `Invoke-RestMethod` παρακάτω.

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

### Άσκηση 9: Χρήση του Προσαρμοσμένου Μοντέλου με το OpenAI SDK

Μπορείτε να συνδεθείτε με το προσαρμοσμένο σας μοντέλο χρησιμοποιώντας ακριβώς τον ίδιο κώδικα OpenAI SDK που χρησιμοποιήσατε για τα ενσωματωμένα μοντέλα (δείτε [Μέρος 3](part3-sdk-and-apis.md)). Η μόνη διαφορά είναι το όνομα του μοντέλου.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Το Foundry Local δεν επικυρώνει τα κλειδιά API
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
  apiKey: "foundry-local", // Το Foundry Local δεν επικυρώνει τα API κλειδιά
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

> **Κύριο σημείο:** Επειδή το Foundry Local εκθέτει ένα API συμβατό με OpenAI, οποιοσδήποτε κώδικας λειτουργεί με τα ενσωματωμένα μοντέλα λειτουργεί και με τα προσαρμοσμένα μοντέλα σας. Αρκεί να αλλάξετε την παράμετρο `model`.

---

### Άσκηση 10: Δοκιμή του Προσαρμοσμένου Μοντέλου με το Foundry Local SDK

Σε προηγούμενα εργαστήρια χρησιμοποιήσατε το Foundry Local SDK για να ξεκινήσετε την υπηρεσία, να εντοπίσετε το endpoint και να διαχειριστείτε τα μοντέλα αυτόματα. Μπορείτε να ακολουθήσετε ακριβώς το ίδιο μοτίβο με το δικό σας προσαρμοσμένο μεταγλωττισμένο μοντέλο. Το SDK χειρίζεται την εκκίνηση υπηρεσίας και την ανακάλυψη του endpoint, έτσι ο κώδικάς σας δεν χρειάζεται να σκληροκωδικοποιεί το `localhost:5273`.

> **Σημείωση:** Βεβαιωθείτε ότι το Foundry Local SDK είναι εγκατεστημένο πριν τρέξετε αυτά τα παραδείγματα:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** Προσθέστε τα πακέτα NuGet `Microsoft.AI.Foundry.Local` και `OpenAI`
>
> Αποθηκεύστε κάθε αρχείο σεναρίου **στην ρίζα του αποθετηρίου** (ίδιος κατάλογος με το φάκελο `models/`).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# Βήμα 1: Εκκινήστε την υπηρεσία Foundry Local και φορτώστε το προσαρμοσμένο μοντέλο
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# Βήμα 2: Ελέγξτε την προσωρινή μνήμη για το προσαρμοσμένο μοντέλο
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# Βήμα 3: Φορτώστε το μοντέλο στη μνήμη
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# Βήμα 4: Δημιουργήστε έναν πελάτη OpenAI χρησιμοποιώντας το σημείο τερματισμού που ανακάλυψε το SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# Βήμα 5: Στείλτε ένα αίτημα ολοκλήρωσης συνομιλίας ροής
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

Τρέξτε το:

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

// Βήμα 1: Ξεκινήστε την υπηρεσία Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// Βήμα 2: Λάβετε το προσαρμοσμένο μοντέλο από τον κατάλογο
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// Βήμα 3: Φορτώστε το μοντέλο στη μνήμη
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// Βήμα 4: Δημιουργήστε έναν πελάτη OpenAI χρησιμοποιώντας το σημείο τερματισμού που ανακαλύφθηκε από το SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// Βήμα 5: Στείλτε ένα αίτημα ολοκλήρωσης συνομιλίας ροής
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

Τρέξτε το:

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

> **Κύριο σημείο:** Το Foundry Local SDK ανακαλύπτει δυναμικά το endpoint, έτσι ποτέ δεν σκληροκωδικοποιείτε αριθμό θύρας. Αυτή είναι η προτεινόμενη προσέγγιση για παραγωγικές εφαρμογές. Το προσαρμοσμένο σας μεταγλωττισμένο μοντέλο λειτουργεί με τον ίδιο τρόπο όπως τα ενσωματωμένα μοντέλα του καταλόγου μέσω του SDK.

---

## Επιλογή Μοντέλου για Μεταγλώττιση

Το Qwen3-0.6B χρησιμοποιείται ως παράδειγμα αναφοράς σε αυτό το εργαστήριο επειδή είναι μικρό, γρήγορο στη μεταγλώττιση και διαθέσιμο ελεύθερα υπό την άδεια Apache 2.0. Ωστόσο, μπορείτε να μεταγλωττίσετε πολλά άλλα μοντέλα. Ακολουθούν μερικές προτάσεις:

| Μοντέλο | Hugging Face ID | Παράμετροι | Άδεια | Σημειώσεις |
|---------|-----------------|------------|-------|------------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | Πολύ μικρό, γρήγορη μεταγλώττιση, καλό για δοκιμές |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | Καλύτερη ποιότητα, ακόμα γρήγορο στη μεταγλώττιση |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | Ισχυρή ποιότητα, χρειάζεται περισσότερη RAM |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Απαιτεί αποδοχή άδειας στο Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | Υψηλή ποιότητα, μεγαλύτερο κατέβασμα και πιο αργή μεταγλώττιση |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Ήδη στον κατάλογο Foundry Local (χρήσιμο για σύγκριση) |

> **Υπενθύμιση Άδειας:** Ελέγξτε πάντα την άδεια του μοντέλου στο Hugging Face πριν τη χρήση. Κάποια μοντέλα (όπως το Llama) απαιτούν να αποδεχτείτε συμφωνία άδειας και να κάνετε πιστοποίηση με `huggingface-cli login` πριν το κατέβασμα.

---

## Έννοιες: Πότε να Χρησιμοποιήσετε Προσαρμοσμένα Μοντέλα

| Σενάριο | Γιατί να Μεταγλωττίσετε το δικό σας; |
|---------|--------------------------------------|
| **Ένα μοντέλο που χρειάζεστε δεν υπάρχει στον κατάλογο** | Ο κατάλογος Foundry Local είναι επιμελημένος. Αν το μοντέλο που θέλετε δεν είναι καταχωρημένο, μεταγλωττίστε το μόνοι σας. |
| **Μοντέλα με λεπτομερή εκπαίδευση (fine-tuned models)** | Αν έχετε εκπαιδεύσει ένα μοντέλο σε δεδομένα συγκεκριμένου τομέα, χρειάζεστε να μεταγλωττίσετε τα δικά σας βάρη. |
| **Ειδικές απαιτήσεις ποσοτικοποίησης (quantisation)** | Μπορεί να θέλετε ακρίβεια ή στρατηγική ποσοτικοποίησης διαφορετική από την προεπιλογή του καταλόγου. |
| **Νεότερες κυκλοφορίες μοντέλων** | Όταν κυκλοφορήσει νέο μοντέλο στο Hugging Face, ενδέχεται να μην υπάρχει αμέσως στον κατάλογο Foundry Local. Με το να το μεταγλωττίσετε μόνοι σας έχετε άμεση πρόσβαση. |
| **Έρευνα και πειραματισμός** | Δοκιμάζοντας διαφορετικές αρχιτεκτονικές μοντέλων, μεγέθη ή ρυθμίσεις τοπικά πριν αποφασίσετε για παραγωγική χρήση. |

---

## Περίληψη

Σε αυτό το εργαστήριο μάθατε πώς να:

| Βήμα | Τι Κάνατε |
|-------|------------|
| 1 | Εγκαταστήσατε τον κατασκευαστή μοντέλων ONNX Runtime GenAI |
| 2 | Μεταγλωττίσατε το `Qwen/Qwen3-0.6B` από το Hugging Face σε βελτιστοποιημένο μοντέλο ONNX |
| 3 | Δημιουργήσατε το αρχείο ρύθμισης προτύπου συνομιλίας `inference_model.json` |
| 4 | Προσθέσατε το μεταγλωττισμένο μοντέλο στην cache του Foundry Local |
| 5 | Εκτελέσατε διαδραστική συνομιλία με το προσαρμοσμένο μοντέλο μέσω CLI |
| 6 | Κάνατε ερώτημα στο μοντέλο μέσω του OpenAI συμβατού REST API |
| 7 | Συνδεθήκατε από Python, JavaScript και C# με το OpenAI SDK |
| 8 | Δοκιμάσατε το προσαρμοσμένο μοντέλο από άκρο σε άκρο με το Foundry Local SDK |

Το βασικό συμπέρασμα είναι ότι **οποιοδήποτε μοντέλο βασισμένο σε transformer μπορεί να τρέξει μέσω Foundry Local** μόλις μεταγλωττιστεί σε μορφή ONNX. Το OpenAI-συμβατό API σημαίνει ότι όλος ο υπάρχων κώδικας εφαρμογής σας λειτουργεί χωρίς αλλαγές· αρκεί απλώς να αλλάξετε το όνομα του μοντέλου.

---

## Βασικά Σημεία

| Έννοια | Λεπτομέρεια |
|--------|-------------|
| ONNX Runtime GenAI Model Builder | Μετατρέπει μοντέλα Hugging Face σε ONNX με ποσοτικοποίηση σε μια εντολή |
| Μορφή ONNX | Το Foundry Local απαιτεί μοντέλα ONNX με διαμόρφωση ONNX Runtime GenAI |
| Πρότυπα συνομιλίας | Το αρχείο `inference_model.json` λέει στο Foundry Local πώς να μορφοποιεί τις προτροπές για συγκεκριμένο μοντέλο |
| Υλικό-στόχοι | Μεταγλώττιση για CPU, NVIDIA GPU (CUDA), DirectML (GPU Windows) ή WebGPU ανάλογα με το υλικό σας |
| Ποσοτικοποίηση | Χαμηλότερη ακρίβεια (int4) μειώνει το μέγεθος και βελτιώνει την ταχύτητα με κόστος λίγη ακρίβεια· fp16 διατηρεί υψηλή ποιότητα σε GPUs |
| Συμβατότητα API | Τα προσαρμοσμένα μοντέλα χρησιμοποιούν το ίδιο συμβατό με OpenAI API όπως τα ενσωματωμένα μοντέλα |
| Foundry Local SDK | Το SDK χειρίζεται αυτόματα την εκκίνηση υπηρεσίας, ανακάλυψη endpoint και φόρτωση μοντέλων για κατάλογο και προσαρμοσμένα μοντέλα |

---

## Επιπλέον Ανάγνωση

| Πόρος | Σύνδεσμος |
|--------|-----------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Οδηγός προσαρμοσμένων μοντέλων Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Οικογένεια μοντέλων Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Τεκμηρίωση Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## Επόμενα Βήματα

Συνεχίστε στο [Μέρος 11: Κλήση Εργαλείων με Τοπικά Μοντέλα](part11-tool-calling.md) για να μάθετε πώς να επιτρέπετε στα τοπικά μοντέλα σας να καλούν εξωτερικές λειτουργίες.

[← Μέρος 9: Μεταγραφή Φωνής Whisper](part9-whisper-voice-transcription.md) | [Μέρος 11: Κλήση Εργαλείων →](part11-tool-calling.md)