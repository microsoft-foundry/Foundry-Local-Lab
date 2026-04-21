<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Εργαστήριο Foundry Local - Δημιουργία Εφαρμογών Τεχνητής Νοημοσύνης στην Συσκευή

Ένα πρακτικό εργαστήριο για την εκτέλεση γλωσσικών μοντέλων στη δική σας μηχανή και τη δημιουργία ευφυών εφαρμογών με το [Foundry Local](https://foundrylocal.ai) και το [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Τι είναι το Foundry Local;** Το Foundry Local είναι ένα ελαφρύ runtime που σας επιτρέπει να κατεβάσετε, διαχειριστείτε και να σερβίρετε γλωσσικά μοντέλα εντελώς στην υλικοτεχνική σας υποδομή. Παρέχει ένα **API συμβατό με το OpenAI** ώστε οποιοδήποτε εργαλείο ή SDK που υποστηρίζει OpenAI να μπορεί να συνδεθεί - χωρίς να απαιτείται λογαριασμός cloud.

---

## Στόχοι Εκμάθησης

Μέχρι το τέλος αυτού του εργαστηρίου θα μπορείτε να:

| # | Στόχος |
|---|-----------|
| 1 | Εγκατάσταση του Foundry Local και διαχείριση μοντέλων με το CLI |
| 2 | Κατανόηση του API του Foundry Local SDK για προγραμματιστική διαχείριση μοντέλων |
| 3 | Σύνδεση με το τοπικό inference server χρησιμοποιώντας τα SDKs για Python, JavaScript και C# |
| 4 | Δημιουργία pipeline Retrieval-Augmented Generation (RAG) που στηρίζει απαντήσεις στα δικά σας δεδομένα |
| 5 | Δημιουργία AI agents με μόνιμες οδηγίες και προσωπικότητες |
| 6 | Ορχήστρωση ροών εργασιών πολλαπλών agents με βρόχους ανατροφοδότησης |
| 7 | Εξερεύνηση μιας παραγωγικής εφαρμογής - ο Zava Creative Writer |
| 8 | Δημιουργία framework αξιολόγησης με golden datasets και βαθμολόγηση LLM-ως-κριτή |
| 9 | Μεταγραφή ήχου με Whisper - φωνή σε κείμενο στην συσκευή χρησιμοποιώντας το Foundry Local SDK |
| 10 | Μεταγλώττιση και εκτέλεση προσαρμοσμένων ή Hugging Face μοντέλων με ONNX Runtime GenAI και Foundry Local |
| 11 | Δυνατότητα τοπικών μοντέλων να καλούν εξωτερικές λειτουργίες με το πρότυπο tool-calling |
| 12 | Δημιουργία διεπαφής χρήστη browser-based για τον Zava Creative Writer με streaming σε πραγματικό χρόνο |

---

## Προαπαιτούμενα

| Απαίτηση | Λεπτομέρειες |
|-------------|---------|
| **Υλικό** | Τουλάχιστον 8 GB RAM (16 GB προτεινόμενα); CPU με AVX2 ή υποστηριζόμενη GPU |
| **Λειτουργικό Σύστημα** | Windows 10/11 (x64/ARM), Windows Server 2025 ή macOS 13+ |
| **Foundry Local CLI** | Εγκατάσταση μέσω `winget install Microsoft.FoundryLocal` (Windows) ή `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Δείτε τον [οδηγό εκκίνησης](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) για λεπτομέρειες. |
| **Runtime γλώσσας** | **Python 3.9+** και/ή **.NET 9.0+** και/ή **Node.js 18+** |
| **Git** | Για κλωνοποίηση αυτής της αποθήκης |

---

## Ξεκινώντας

```bash
# 1. Κλωνοποιήστε το αποθετήριο
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Επαληθεύστε ότι το Foundry Local είναι εγκατεστημένο
foundry model list              # Λίστα διαθέσιμων μοντέλων
foundry model run phi-3.5-mini  # Ξεκινήστε μια διαδραστική συνομιλία

# 3. Επιλέξτε τη γλωσσική διαδρομή σας (δείτε το εργαστήριο Μέρος 2 για πλήρη ρύθμιση)
```

| Γλώσσα | Γρήγορη Εκκίνηση |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Μέρη του Εργαστηρίου

### Μέρος 1: Ξεκινώντας με το Foundry Local

**Οδηγός εργαστηρίου:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Τι είναι το Foundry Local και πώς λειτουργεί
- Εγκατάσταση του CLI σε Windows και macOS
- Εξερεύνηση μοντέλων - λίστα, κατέβασμα, εκτέλεση
- Κατανόηση aliases μοντέλων και δυναμικών θυρών

---

### Μέρος 2: Βαθιά Διερεύνηση του Foundry Local SDK

**Οδηγός εργαστηρίου:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Γιατί να χρησιμοποιήσετε το SDK αντί για το CLI στην ανάπτυξη εφαρμογών
- Πλήρης αναφορά API SDK για Python, JavaScript και C#
- Διαχείριση υπηρεσίας, περιήγηση καταλόγου, κύκλος ζωής μοντέλων (κατέβασμα, φόρτωση, αποφόρτωση)
- Γρήγορα πρότυπα εκκίνησης: Python constructor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- Μεταδεδομένα `FoundryModelInfo`, aliases και επιλογή μοντέλων κατάλληλων για υλικό

---

### Μέρος 3: SDKs και APIs

**Οδηγός εργαστηρίου:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Σύνδεση με το Foundry Local από Python, JavaScript και C#
- Χρήση του Foundry Local SDK για προγραμματιστική διαχείριση της υπηρεσίας
- Streaming ολοκληρώσεων συνομιλίας μέσω του API συμβατού με OpenAI
- Αναφορά μεθόδων SDK για κάθε γλώσσα

**Παραδείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Βασικό streaming συνομιλίας |
| C# | `csharp/BasicChat.cs` | Streaming συνομιλίας με .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming συνομιλίας με Node.js |

---

### Μέρος 4: Retrieval-Augmented Generation (RAG)

**Οδηγός εργαστηρίου:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Τι είναι το RAG και γιατί έχει σημασία
- Δημιουργία στη μνήμη βάσης γνώσης
- Ανάκτηση με βάση την επικάλυψη λέξεων-κλειδιών και βαθμολόγηση
- Σύνθεση θεμελιωμένων prompts συστήματος
- Εκτέλεση πλήρους pipeline RAG στην συσκευή

**Παραδείγματα κώδικα:**

| Γλώσσα | Αρχείο |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Μέρος 5: Δημιουργία AI Agents

**Οδηγός εργαστηρίου:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Τι είναι ένας AI agent (σε σύγκριση με απλή κλήση LLM)
- Το πρότυπο `ChatAgent` και το Microsoft Agent Framework
- Οδηγίες συστήματος, προσωποποιήσεις και συνομιλίες πολλαπλών γύρων
- Δομημένη έξοδος (JSON) από τους agents

**Παραδείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Μεμονωμένος agent με Agent Framework |
| C# | `csharp/SingleAgent.cs` | Μεμονωμένος agent (πρότυπο ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Μεμονωμένος agent (πρότυπο ChatAgent) |

---

### Μέρος 6: Ροές Εργασίας Πολλαπλών Agents

**Οδηγός εργαστηρίου:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipelines πολλαπλών agents: Ερευνητής → Συγγραφέας → Επιμελητής
- Διαδοχική ορχήστρωση και βρόχοι ανατροφοδότησης
- Κοινή διαμόρφωση και δομημένες παραδόσεις
- Σχεδιασμός της δικής σας ροής εργασίας πολλαπλών agents

**Παραδείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline τριών agents |
| C# | `csharp/MultiAgent.cs` | Pipeline τριών agents |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline τριών agents |

---

### Μέρος 7: Zava Creative Writer - Εφαρμογή Capstone

**Οδηγός εργαστηρίου:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Μία παραγωγική εφαρμογή πολλαπλών agents με 4 εξειδικευμένους agents
- Διαδοχικό pipeline με βρόχους ανατροφοδότησης καθοδηγούμενους από αξιολογητή
- Streaming εξόδου, αναζήτηση καταλόγου προϊόντων, δομημένες JSON παραδόσεις
- Πλήρης υλοποίηση σε Python (FastAPI), JavaScript (Node.js CLI) και C# (.NET console)

**Παραδείγματα κώδικα:**

| Γλώσσα | Φάκελος | Περιγραφή |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI web υπηρεσία με orchestrator |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Εφαρμογή Node.js CLI |
| C# | `zava-creative-writer-local/src/csharp/` | Εφαρμογή κονσόλας .NET 9 |

---

### Μέρος 8: Ανάπτυξη με Νέες Αξιολογήσεις

**Οδηγός εργαστηρίου:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Δημιουργία συστηματικού πλαισίου αξιολόγησης για AI agents χρησιμοποιώντας golden datasets
- Έλεγχοι βάσει κανόνων (μήκος, κάλυψη λέξεων-κλειδιών, απαγορευμένοι όροι) + βαθμολόγηση LLM-ως-κριτή
- Σύγκριση παραλλαγών prompts με συγκεντρωτικές καρτέλες βαθμολογίας
- Επέκταση του προτύπου Zava Editor agent από το Μέρος 7 σε offline test suite
- Πορεία σε Python, JavaScript και C#

**Παραδείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Πλαίσιο αξιολόγησης |
| C# | `csharp/AgentEvaluation.cs` | Πλαίσιο αξιολόγησης |
| JavaScript | `javascript/foundry-local-eval.mjs` | Πλαίσιο αξιολόγησης |

---

### Μέρος 9: Μεταγραφή Φωνής με Whisper

**Οδηγός εργαστηρίου:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Φωνή σε κείμενο χρησιμοποιώντας OpenAI Whisper τοπικά
- Επεξεργασία ήχου με προτεραιότητα απορρήτου - ο ήχος δεν εγκαταλείπει ποτέ τη συσκευή σας
- Πορείες σε Python, JavaScript και C# με `client.audio.transcriptions.create()` (Python/JS) και `AudioClient.TranscribeAudioAsync()` (C#)
- Συμπεριλαμβάνει δείγματα ήχου με θέμα το Zava για πρακτική εξάσκηση

**Παραδείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Μεταγραφή φωνής Whisper |
| C# | `csharp/WhisperTranscription.cs` | Μεταγραφή φωνής Whisper |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Μεταγραφή φωνής Whisper |

> **Σημείωση:** Αυτό το εργαστήριο χρησιμοποιεί το **Foundry Local SDK** για προγραμματιστικό κατέβασμα και φόρτωση του μοντέλου Whisper, και στη συνέχεια στέλνει τον ήχο στο τοπικό API συμβατό με OpenAI για μεταγραφή. Το μοντέλο Whisper (`whisper`) είναι καταχωρημένο στον κατάλογο Foundry Local και τρέχει εξ ολοκλήρου στην συσκευή - δεν απαιτούνται API κλειδιά cloud ή πρόσβαση δικτύου.

---

### Μέρος 10: Χρήση Προσαρμοσμένων ή Hugging Face Μοντέλων

**Οδηγός εργαστηρίου:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Μεταγλώττιση μοντέλων Hugging Face σε βελτιστοποιημένη μορφή ONNX με τον ONNX Runtime GenAI builder
- Μεταγλώττιση ειδική για υλικό (CPU, NVIDIA GPU, DirectML, WebGPU) και ποσοτικοποίηση (int4, fp16, bf16)
- Δημιουργία αρχείων παραμετροποίησης chat-template για Foundry Local
- Προσθήκη μεταγλωττισμένων μοντέλων στην cache του Foundry Local
- Εκτέλεση προσαρμοσμένων μοντέλων μέσω CLI, REST API και OpenAI SDK
- Παράδειγμα αναφοράς: ολοκληρωμένη μεταγλώττιση Qwen/Qwen3-0.6B

---

### Μέρος 11: Κλήση Εργαλείων με Τοπικά Μοντέλα

**Οδηγός εργαστηρίου:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Ενεργοποίηση τοπικών μοντέλων να καλούν εξωτερικές λειτουργίες (tool/function calling)
- Ορισμός σχημάτων εργαλείων με τη μορφή κλήσης λειτουργιών OpenAI
- Διαχείριση συζήτησης με πολλούς γύρους κλήσης εργαλείων
- Εκτέλεση κλήσεων εργαλείων τοπικά και επιστροφή αποτελεσμάτων στο μοντέλο
- Επιλογή κατάλληλου μοντέλου για σενάρια tool-calling (Qwen 2.5, Phi-4-mini)
- Χρήση του εγγενούς SDK `ChatClient` για κλήση εργαλείων (JavaScript)

**Παραδείγματα κώδικα:**

| Γλώσσα | Αρχείο | Περιγραφή |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Κλήση εργαλείων με εργαλεία καιρό/πληθυσμού |
| C# | `csharp/ToolCalling.cs` | Κλήση εργαλείων με .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Κλήση εργαλείων με ChatClient |

---

### Μέρος 12: Δημιουργία Web UI για τον Zava Creative Writer

**Οδηγός εργαστηρίου:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Προσθήκη frontend browser-based στον Zava Creative Writer
- Παροχή κοινής UI από Python (FastAPI), JavaScript (Node.js HTTP) και C# (ASP.NET Core)
- Κατανάλωση streaming NDJSON στον browser με το Fetch API και ReadableStream
- Σήματα ζωντανής κατάστασης agent και streaming κειμένου άρθρου σε πραγματικό χρόνο

**Κώδικας (κοινό UI):**

| Αρχείο | Περιγραφή |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Διάταξη σελίδας |
| `zava-creative-writer-local/ui/style.css` | Στυλ |
| `zava-creative-writer-local/ui/app.js` | Λογική ανάγνωσης ροής και ενημέρωση DOM |

**Προσθήκες Backend:**

| Γλώσσα | Αρχείο | Περιγραφή |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Ενημερωμένο για παροχή στατικής UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Νέος HTTP server που περιτυλίγει τον orchestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Νέο ελάχιστο API έργο ASP.NET Core |

---

### Μέρος 13: Ολοκλήρωση Εργαστηρίου
**Οδηγός εργαστηρίου:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Περίληψη όλων όσων έχετε δημιουργήσει σε όλα τα 12 μέρη
- Επιπλέον ιδέες για διεύρυνση των εφαρμογών σας
- Συνδέσεις προς πόρους και τεκμηρίωση

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
| Ιστότοπος Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| Κατάλογος μοντέλων | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local στο GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Οδηγός εκκίνησης | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Αναφορά SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Πλαίσιο εργασίας Microsoft Agent | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Άδεια

Το υλικό αυτού του εργαστηρίου παρέχεται για εκπαιδευτικούς σκοπούς.

---

**Καλή δημιουργία! 🚀**