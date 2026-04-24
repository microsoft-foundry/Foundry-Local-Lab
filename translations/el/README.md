<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Εργαστήριο Foundry Local - Δημιουργία AI Εφαρμογών Τοπικά

Ένα πρακτικό εργαστήριο για την εκτέλεση γλωσσικών μοντέλων στη δική σας συσκευή και την κατασκευή έξυπνων εφαρμογών με το [Foundry Local](https://foundrylocal.ai) και το [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Τι είναι το Foundry Local;** Το Foundry Local είναι ένα ελαφρύ περιβάλλον εκτέλεσης που σας επιτρέπει να κατεβάζετε, διαχειρίζεστε και παρέχετε γλωσσικά μοντέλα αποκλειστικά στο δικό σας υλικό. Παρέχει ένα **API συμβατό με OpenAI** ώστε οποιοδήποτε εργαλείο ή SDK που υποστηρίζει OpenAI να μπορεί να συνδεθεί – χωρίς να απαιτείται λογαριασμός cloud.

### 🌐 Υποστήριξη Πολλών Γλωσσών

#### Υποστηρίζεται μέσω GitHub Action (Αυτοματοποιημένο & Πάντα Ενημερωμένο)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Αραβικά](../ar/README.md) | [Μπενγκάλι](../bn/README.md) | [Βουλγαρικά](../bg/README.md) | [Βιρμανικά (Μυανμάρ)](../my/README.md) | [Κινέζικα (Απλοποιημένα)](../zh-CN/README.md) | [Κινέζικα (Παραδοσιακά, Χονγκ Κονγκ)](../zh-HK/README.md) | [Κινέζικα (Παραδοσιακά, Μακάο)](../zh-MO/README.md) | [Κινέζικα (Παραδοσιακά, Ταϊβάν)](../zh-TW/README.md) | [Κροατικά](../hr/README.md) | [Τσεχικά](../cs/README.md) | [Δανέζικα](../da/README.md) | [Ολλανδικά](../nl/README.md) | [Εσθονικά](../et/README.md) | [Φινλανδικά](../fi/README.md) | [Γαλλικά](../fr/README.md) | [Γερμανικά](../de/README.md) | [Ελληνικά](./README.md) | [Εβραϊκά](../he/README.md) | [Χίντι](../hi/README.md) | [Ουγγρικά](../hu/README.md) | [Ινδονησιακά](../id/README.md) | [Ιταλικά](../it/README.md) | [Ιαπωνικά](../ja/README.md) | [Κανάντα](../kn/README.md) | [Χμερ](../km/README.md) | [Κορεατικά](../ko/README.md) | [Λιθουανικά](../lt/README.md) | [Μαλαισιανά](../ms/README.md) | [Μαλαγιαλάμ](../ml/README.md) | [Μαράθι](../mr/README.md) | [Νεπάλι](../ne/README.md) | [Νιγηριανό Πίτζιν](../pcm/README.md) | [Νορβηγικά](../no/README.md) | [Περσικά (Φαρσί)](../fa/README.md) | [Πολωνικά](../pl/README.md) | [Πορτογαλικά (Βραζιλία)](../pt-BR/README.md) | [Πορτογαλικά (Πορτογαλία)](../pt-PT/README.md) | [Πουντζάμπι (Γκουρμούκι)](../pa/README.md) | [Ρουμανικά](../ro/README.md) | [Ρωσικά](../ru/README.md) | [Σερβικά (Κυριλλικά)](../sr/README.md) | [Σλοβακικά](../sk/README.md) | [Σλοβενικά](../sl/README.md) | [Ισπανικά](../es/README.md) | [Σουαχίλι](../sw/README.md) | [Σουηδικά](../sv/README.md) | [Tagalog (Φιλιππινέζικα)](../tl/README.md) | [Ταμίλ](../ta/README.md) | [Τελούγκου](../te/README.md) | [Ταϊλανδικά](../th/README.md) | [Τουρκικά](../tr/README.md) | [Ουκρανικά](../uk/README.md) | [Ουρντού](../ur/README.md) | [Βιετναμέζικα](../vi/README.md)

> **Προτιμάτε τοπικό κλωνοποίηση;**
>
> Αυτό το αποθετήριο περιλαμβάνει πάνω από 50 μεταφράσεις γλωσσών που αυξάνουν σημαντικά το μέγεθος λήψης. Για να κλωνοποιήσετε χωρίς τις μεταφράσεις, χρησιμοποιήστε sparse checkout:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> Αυτό σας παρέχει ό,τι χρειάζεστε για να ολοκληρώσετε το μάθημα με πολύ πιο γρήγορη λήψη.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Στόχοι Μάθησης

Μέχρι το τέλος αυτού του εργαστηρίου θα μπορείτε να:

| # | Στόχος |
|---|---------|
| 1 | Εγκαταστήσετε το Foundry Local και να διαχειριστείτε μοντέλα με το CLI |
| 2 | Κατακτήσετε το API του Foundry Local SDK για προγραμματιστική διαχείριση μοντέλων |
| 3 | Συνδεθείτε με τον τοπικό server επεξεργασίας χρησιμοποιώντας τα SDKs Python, JavaScript, και C# |
| 4 | Δημιουργήσετε μια αλυσίδα Retrieval-Augmented Generation (RAG) που βασίζει τις απαντήσεις σε δικά σας δεδομένα |
| 5 | Δημιουργήσετε AI agents με μόνιμες οδηγίες και προσωπικότητες |
| 6 | Οργανώσετε ροές εργασιών με πολλούς agents και μηχανισμούς ανάδρασης |
| 7 | Εξερευνήσετε μια παραγωγική εφαρμογή δείκτη - το Zava Creative Writer |
| 8 | Χτίσετε πλαίσια αξιολόγησης με χρυσά datasets και βαθμολόγηση LLM-as-judge |
| 9 | Μεταγράψετε ήχο με το Whisper - ομιλία σε κείμενο στη συσκευή μέσω του Foundry Local SDK |
| 10 | Συγκολλήσετε και εκτελέσετε προσαρμοσμένα ή Hugging Face μοντέλα με ONNX Runtime GenAI και Foundry Local |
| 11 | Δυναμώσετε τις τοπικές κλήσεις μοντέλων να καλούν εξωτερικές λειτουργίες με pattern κλήσης εργαλείων |
| 12 | Δημιουργήσετε περιβάλλον διεπαφής browser για τον Zava Creative Writer με ροή σε πραγματικό χρόνο |

---

## Προαπαιτούμενα

| Απαίτηση | Λεπτομέρειες |
|----------|--------------|
| **Υλικό** | Ελάχιστο 8 GB RAM (συνιστάται 16 GB); CPU με υποστήριξη AVX2 ή συμβατή GPU |
| **ΛΣ** | Windows 10/11 (x64/ARM), Windows Server 2025 ή macOS 13+ |
| **Foundry Local CLI** | Εγκατάσταση μέσω `winget install Microsoft.FoundryLocal` (Windows) ή `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Δείτε τον [οδηγό εκκίνησης](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) για λεπτομέρειες. |
| **Γλώσσα εκτέλεσης** | **Python 3.9+** και/ή **.NET 9.0+** και/ή **Node.js 18+** |
| **Git** | Για το κλωνοποίηση του αποθετηρίου |

---

## Έναρξη

```bash
# 1. Κλωνοποιήστε το αποθετήριο
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Επαληθεύστε ότι το Foundry Local είναι εγκατεστημένο
foundry model list              # Καταγράψτε τα διαθέσιμα μοντέλα
foundry model run phi-3.5-mini  # Ξεκινήστε μια διαδραστική συνομιλία

# 3. Επιλέξτε τη γλωσσική σας διαδρομή (δείτε το εργαστήριο Μέρος 2 για πλήρη ρύθμιση)
```

| Γλώσσα | Γρήγορη Εκκίνηση |
|--------|------------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Μέρη Εργαστηρίου

### Μέρος 1: Ξεκινώντας με Foundry Local

**Οδηγός εργαστηρίου:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Τι είναι το Foundry Local και πώς λειτουργεί
- Εγκατάσταση του CLI σε Windows και macOS
- Εξερεύνηση μοντέλων - λίστα, λήψη, εκτέλεση
- Κατανόηση ψευδωνύμων μοντέλων και δυναμικών θυρών

---

### Μέρος 2: Βαθιά Εμβάθυνση στο Foundry Local SDK

**Οδηγός εργαστηρίου:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Γιατί να χρησιμοποιήσετε το SDK αντί του CLI για ανάπτυξη εφαρμογών
- Πλήρης αναφορά API SDK για Python, JavaScript και C#
- Διαχείριση υπηρεσίας, περιήγηση καταλόγου, κύκλος ζωής μοντέλων (λήψη, φόρτωση, απόρριψη)
- Γρήγορα μοτίβα εκκίνησης: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` μεταδεδομένα, ψευδώνυμα, και επιλογή μοντέλου βέλτιστη για υλικό

---

### Μέρος 3: SDKs και APIs

**Οδηγός εργαστηρίου:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Σύνδεση στο Foundry Local από Python, JavaScript και C#
- Χρήση του Foundry Local SDK για προγραμματική διαχείριση υπηρεσίας
- Ροή συνομιλίας μέσω API συμβατού με OpenAI
- Αναφορά μεθόδων SDK για κάθε γλώσσα

**Δείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|--------|---------|-----------|
| Python | `python/foundry-local.py` | Βασική ροή συνομιλίας |
| C# | `csharp/BasicChat.cs` | Ροή συνομιλίας με .NET |
| JavaScript | `javascript/foundry-local.mjs` | Ροή συνομιλίας με Node.js |

---

### Μέρος 4: Retrieval-Augmented Generation (RAG)

**Οδηγός εργαστηρίου:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Τι είναι το RAG και γιατί έχει σημασία
- Δημιουργία βάσης γνώσεων στη μνήμη
- Ανάκτηση με επικαλυπτόμενες λέξεις-κλειδιά και βαθμολόγηση
- Σύνθεση τεκμηριωμένων συστημικών οδηγιών
- Εκτέλεση μιας ολοκληρωμένης αλυσίδας RAG στη συσκευή

**Δείγματα κώδικα:**

| Γλώσσα | Αρχείο |
|--------|--------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Μέρος 5: Δημιουργία AI Agents

**Οδηγός εργαστηρίου:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Τι είναι ο AI agent (σε σχέση με απλή κλήση LLM)
- Το μοτίβο `ChatAgent` και το Microsoft Agent Framework
- Σύστημα οδηγιών, προσωπικότητες, και πολυδιάλογες συνομιλίες
- Δομημένη έξοδος (JSON) από agents

**Δείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|--------|--------|-----------|
| Python | `python/foundry-local-with-agf.py` | Μονός agent με Agent Framework |
| C# | `csharp/SingleAgent.cs` | Μονός agent (μοτίβο ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Μονός agent (μοτίβο ChatAgent) |

---

### Μέρος 6: Ροές Εργασίας Πολυ-Agents

**Οδηγός εργαστηρίου:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Αλυσίδες με πολλούς agents: Ερευνητής → Συγγραφέας → Επιμελητής
- Αλληλουχία ορχήστρωσης και μηχανισμοί ανάδρασης
- Κοινή διαμόρφωση και δομημένες παραδόσεις
- Σχεδιασμός δικής σας ροής εργασίας πολλών agents

**Δείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|--------|--------|-----------|
| Python | `python/foundry-local-multi-agent.py` | Αλυσίδα τριών agents |
| C# | `csharp/MultiAgent.cs` | Αλυσίδα τριών agents |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Αλυσίδα τριών agents |

---

### Μέρος 7: Zava Creative Writer - Εφαρμογή Capstone

**Οδηγός εργαστηρίου:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Μια παραγωγική εφαρμογή πολλών agents με 4 εξειδικευμένους agents
- Αλληλουχία με μηχανισμούς ανάδρασης υπό την καθοδήγηση αξιολογητών
- Ροή εξόδου, αναζήτηση σε κατάλογο προϊόντων, δομημένες παραδόσεις JSON
- Πλήρης υλοποίηση σε Python (FastAPI), JavaScript (Node.js CLI) και C# (.NET κονσόλα)

**Δείγματα κώδικα:**

| Γλώσσα | Φάκελος | Περιγραφή |
|--------|---------|-----------|
| Python | `zava-creative-writer-local/src/api/` | Υπηρεσία web FastAPI με ορχηστρωτή |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Εφαρμογή CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | Εφαρμογή κονσόλας .NET 9 |

---

### Μέρος 8: Ανάπτυξη με Καθοδήγηση Αξιολόγησης

**Οδηγός εργαστηρίου:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Δημιουργία συστηματικού πλαισίου αξιολόγησης για AI agents με χρυσά datasets
- Έλεγχοι βασισμένοι σε κανόνες (μήκος, κάλυψη λέξεων-κλειδιών, απαγορευμένοι όροι) + βαθμολόγηση LLM-as-judge
- Παράθεση παραλλαγών προτροπών με συνολικούς πίνακες βαθμολογίας
- Επεκτείνει το μοτίβο Zava Editor agent από το Μέρος 7 σε offline σετ δοκιμών
- Πίστες Python, JavaScript και C#

**Δείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|--------|--------|-----------|
| Python | `python/foundry-local-eval.py` | Πλαίσιο αξιολόγησης |
| C# | `csharp/AgentEvaluation.cs` | Πλαίσιο αξιολόγησης |
| JavaScript | `javascript/foundry-local-eval.mjs` | Πλαίσιο αξιολόγησης |

---

### Μέρος 9: Μεταγραφή Φωνής με Whisper

**Οδηγός εργαστηρίου:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Μετατροπή ομιλίας σε κείμενο με χρήση OpenAI Whisper που εκτελείται τοπικά  
- Επεξεργασία ήχου με προτεραιότητα στην ιδιωτικότητα - ο ήχος δεν αφήνει ποτέ τη συσκευή σας  
- Ανάπτυξη σε Python, JavaScript και C# με `client.audio.transcriptions.create()` (Python/JS) και `AudioClient.TranscribeAudioAsync()` (C#)  
- Περιλαμβάνονται δείγματα ήχου θεματικά με το Zava για πρακτική εξάσκηση  

**Δείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Μεταγραφή ομιλίας Whisper |
| C# | `csharp/WhisperTranscription.cs` | Μεταγραφή ομιλίας Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Μεταγραφή ομιλίας Whisper |

> **Σημείωση:** Αυτό το εργαστήριο χρησιμοποιεί το **Foundry Local SDK** για προγραμματιστική λήψη και φόρτωση του μοντέλου Whisper, στη συνέχεια στέλνει ήχο στο τοπικό endpoint συμβατό με OpenAI για μεταγραφή. Το μοντέλο Whisper (`whisper`) είναι καταχωρημένο στον κατάλογο Foundry Local και εκτελείται εξ ολοκλήρου στη συσκευή - δεν απαιτούνται κλειδιά API σύννεφου ή πρόσβαση δικτύου.

---

### Μέρος 10: Χρήση Προσαρμοσμένων ή Hugging Face Μοντέλων

**Οδηγός εργαστηρίου:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Μεταγλώττιση μοντέλων Hugging Face σε βελτιστοποιημένη μορφή ONNX με χρήση του ONNX Runtime GenAI model builder  
- Μεταγλώττιση ειδική για υλικό (CPU, NVIDIA GPU, DirectML, WebGPU) και κβαντισμός (int4, fp16, bf16)  
- Δημιουργία αρχείων ρύθμισης προτύπου συνομιλίας για Foundry Local  
- Προσθήκη μεταγλωττισμένων μοντέλων στην cache του Foundry Local  
- Εκτέλεση προσαρμοσμένων μοντέλων μέσω CLI, REST API και OpenAI SDK  
- Αναφορά παραδείγματος: ολική μεταγλώττιση Qwen/Qwen3-0.6B

---

### Μέρος 11: Κλήση Εργαλείων με Τοπικά Μοντέλα

**Οδηγός εργαστηρίου:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Ενεργοποίηση των τοπικών μοντέλων να καλούν εξωτερικές συναρτήσεις (κλήση εργαλείων/συναρτήσεων)  
- Ορισμός σχημάτων εργαλείων με τη μορφή κλήσης συναρτήσεων OpenAI  
- Διαχείριση της ροής πολύστροφου διαλόγου κλήσης εργαλείων  
- Εκτέλεση κλήσεων εργαλείων τοπικά και επιστροφή αποτελεσμάτων στο μοντέλο  
- Επιλογή κατάλληλου μοντέλου για σενάρια κλήσης εργαλείων (Qwen 2.5, Phi-4-mini)  
- Χρήση του εγγενούς `ChatClient` του SDK για κλήση εργαλείων (JavaScript)

**Δείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Κλήση εργαλείων με εργαλεία καιρού/πληθυσμού |
| C# | `csharp/ToolCalling.cs` | Κλήση εργαλείων με .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Κλήση εργαλείων με ChatClient |

---

### Μέρος 12: Δημιουργία Web UI για τον Zava Creative Writer

**Οδηγός εργαστηρίου:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Προσθήκη περιβάλλοντος χρήστη browser-based για τον Zava Creative Writer  
- Εξυπηρέτηση του κοινόχρηστου UI από Python (FastAPI), JavaScript (Node.js HTTP), και C# (ASP.NET Core)  
- Κατανάλωση streaming NDJSON στον browser με Fetch API και ReadableStream  
- Σήματα κατάστασης ζωντανού πράκτορα και ζωντανή ροή κειμένου άρθρου

**Κώδικας (κοινό UI):**

| Αρχείο | Περιγραφή |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Διάταξη σελίδας |
| `zava-creative-writer-local/ui/style.css` | Στυλ |
| `zava-creative-writer-local/ui/app.js` | Λογική ανάγνωσης ροής και ενημέρωσης DOM |

**Προσθήκες backend:**

| Γλώσσα | Αρχείο | Περιγραφή |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Ενημερώσεις για εξυπηρέτηση στατικού UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Νέος HTTP server που συσκευάζει τον orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Νέο πρωτοποριακό έργο ASP.NET Core minimal API |

---

### Μέρος 13: Ολοκλήρωση Εργαστηρίου

**Οδηγός εργαστηρίου:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Περίληψη όλων όσων έχετε δημιουργήσει σε όλα τα 12 μέρη  
- Επιπλέον ιδέες για επέκταση των εφαρμογών σας  
- Συνδέσμοι προς πόρους και τεκμηρίωση

---

## Δομή Έργου

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## Πόροι

| Πόρος | Σύνδεσμος |
|----------|------|
| Ιστοσελίδα Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Κατάλογος μοντέλων | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Οδηγός έναρξης | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Αναφορά Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Άδεια

Αυτό το υλικό εργαστηρίου παρέχεται για εκπαιδευτικούς σκοπούς.

---

**Καλή δημιουργία! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Αποποίηση ευθυνών**:  
Αυτό το έγγραφο έχει μεταφραστεί χρησιμοποιώντας την υπηρεσία μετάφρασης με τεχνητή νοημοσύνη [Co-op Translator](https://github.com/Azure/co-op-translator). Παρόλο που επιδιώκουμε την ακρίβεια, παρακαλούμε να έχετε υπόψη ότι οι αυτοματοποιημένες μεταφράσεις μπορεί να περιέχουν λάθη ή ανακρίβειες. Το πρωτότυπο έγγραφο στη μητρική του γλώσσα πρέπει να θεωρείται η επίσημη πηγή. Για κρίσιμες πληροφορίες, συνιστάται επαγγελματική ανθρώπινη μετάφραση. Δεν φέρουμε ευθύνη για τυχόν παρερμηνείες ή λανθασμένες ερμηνείες που προκύπτουν από τη χρήση αυτής της μετάφρασης.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->