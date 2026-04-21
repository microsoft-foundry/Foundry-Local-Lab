# Οδηγίες για τον Κώδικα Αντιπροσώπου

Αυτό το αρχείο παρέχει το πλαίσιο για AI αντιπροσώπους κώδικα (GitHub Copilot, Copilot Workspace, Codex κ.ά.) που εργάζονται σε αυτό το αποθετήριο.

## Επισκόπηση Έργου

Αυτό είναι ένα **εργαστήριο πρακτικής** για την κατασκευή εφαρμογών AI με το [Foundry Local](https://foundrylocal.ai) — ένα ελαφρύ περιβάλλον εκτέλεσης που κατεβάζει, διαχειρίζεται και εξυπηρετεί γλωσσικά μοντέλα εντελώς στη συσκευή μέσω ενός API συμβατού με OpenAI. Το εργαστήριο περιλαμβάνει βήμα προς βήμα οδηγούς εργαστηρίου και εκτελέσιμα δείγματα κώδικα σε Python, JavaScript και C#.

## Δομή Αποθετηρίου

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## Λεπτομέρειες για Γλώσσες & Πλαίσια Εργασίας

### Python
- **Τοποθεσία:** `python/`, `zava-creative-writer-local/src/api/`
- **Εξαρτήσεις:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Κύρια πακέτα:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Ελάχιστη έκδοση:** Python 3.9+
- **Εκτέλεση:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Τοποθεσία:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Εξαρτήσεις:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Κύρια πακέτα:** `foundry-local-sdk`, `openai`
- **Σύστημα μονάδων:** ES modules (`.mjs` αρχεία, `"type": "module"`)
- **Ελάχιστη έκδοση:** Node.js 18+
- **Εκτέλεση:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Τοποθεσία:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Αρχεία έργου:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Κύρια πακέτα:** `Microsoft.AI.Foundry.Local` (μη-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — υπερσύνολο με QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Στόχος:** .NET 9.0 (συνθηματικό TFM: `net9.0-windows10.0.26100` στα Windows, `net9.0` αλλού)
- **Εκτέλεση:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Κανόνες Κωδικοποίησης

### Γενικά
- Όλα τα δείγματα κώδικα είναι **αυτοτελή παραδείγματα ενός αρχείου** — χωρίς κοινές βοηθητικές βιβλιοθήκες ή αφαιρέσεις.
- Κάθε δείγμα εκτελείται ανεξάρτητα μετά την εγκατάσταση των δικών του εξαρτήσεων.
- Τα κλειδιά API ορίζονται πάντα ως `"foundry-local"` — το Foundry Local το χρησιμοποιεί ως δείκτη θέσης.
- Οι βασικές διευθύνσεις URL χρησιμοποιούν `http://localhost:<port>/v1` — η θύρα είναι δυναμική και ανακαλύπτεται κατά το χρόνο εκτέλεσης μέσω του SDK (`manager.urls[0]` σε JS, `manager.endpoint` σε Python).
- Το Foundry Local SDK χειρίζεται την εκκίνηση της υπηρεσίας και την ανακάλυψη του endpoint· προτιμήστε τα πρότυπα του SDK από τις σταθερές θύρες.

### Python
- Χρησιμοποιήστε το SDK `openai` με `OpenAI(base_url=..., api_key="not-required")`.
- Χρησιμοποιήστε το `FoundryLocalManager()` από το `foundry_local` για τη διαχείριση του κύκλου ζωής της υπηρεσίας μέσω SDK.
- Ροή: επαναλάβετε πάνω στο αντικείμενο `stream` με `for chunk in stream:`.
- Χωρίς τύπους στις σημειώσεις δειγμάτων (κρατήστε τα δείγματα συνοπτικά για τους μαθητές του εργαστηρίου).

### JavaScript
- Σύνταξη ES module: `import ... from "..."`.
- Χρησιμοποιήστε `OpenAI` από `"openai"` και `FoundryLocalManager` από `"foundry-local-sdk"`.
- Πρότυπο αρχικοποίησης SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Ροή: `for await (const chunk of stream)`.
- Χρησιμοποιείται top-level `await`.

### C#
- Nullable ενεργό, implicit usings, .NET 9.
- Χρησιμοποιήστε `FoundryLocalManager.StartServiceAsync()` για διαχείριση κύκλου ζωής μέσω SDK.
- Ροή: `CompleteChatStreaming()` με `foreach (var update in completionUpdates)`.
- Το κύριο `csharp/Program.cs` είναι CLI router που δρομολογεί σε στατικές μεθόδους `RunAsync()`.

### Κλήση Εργαλείων
- Μόνο ορισμένα μοντέλα υποστηρίζουν κλήση εργαλείων: οικογένεια **Qwen 2.5** (`qwen2.5-*`) και **Phi-4-mini** (`phi-4-mini`).
- Τα σχήματα εργαλείων ακολουθούν τη μορφή JSON κλήσης λειτουργιών OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Η συνομιλία ακολουθεί μοτίβο πολλαπλών γύρων: χρήστης → βοηθός (tool_calls) → εργαλείο (αποτελέσματα) → βοηθός (τελική απάντηση).
- Το `tool_call_id` στα μηνύματα αποτελέσματος εργαλείου πρέπει να ταιριάζει με το `id` από την κλήση εργαλείου του μοντέλου.
- Η Python χρησιμοποιεί το OpenAI SDK απευθείας· η JavaScript χρησιμοποιεί το εγγενές `ChatClient` του SDK (`model.createChatClient()`); η C# χρησιμοποιεί το OpenAI SDK με `ChatTool.CreateFunctionTool()`.

### ChatClient (Εγγενής πελάτης SDK)
- JavaScript: `model.createChatClient()` επιστρέφει `ChatClient` με μεθόδους `completeChat(messages, tools?)` και `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` επιστρέφει τυπικό `ChatClient` που μπορεί να χρησιμοποιηθεί χωρίς εισαγωγή του πακέτου OpenAI NuGet.
- Python δεν έχει εγγενή ChatClient — χρησιμοποιεί το OpenAI SDK με `manager.endpoint` και `manager.api_key`.
- **Σημαντικό:** Το JavaScript `completeStreamingChat` χρησιμοποιεί **μοτίβο callback**, όχι ασύγχρονη επανάληψη.

### Μοντέλα Λογικής
- Το `phi-4-mini-reasoning` περικλείει τη σκέψη του σε ετικέτες `<think>...</think>` πριν από την τελική απάντηση.
- Αναλύστε τις ετικέτες για να ξεχωρίσετε τη λογική από την απάντηση όπου χρειάζεται.

## Οδηγοί Εργαστηρίων

Τα αρχεία εργαστηρίων βρίσκονται στο `labs/` σε μορφή Markdown. Ακολουθούν σταθερή δομή:
- Επικεφαλίδα με λογότυπο
- Τίτλος και δήλωση στόχου
- Επισκόπηση, Στόχοι Μάθησης, Προαπαιτούμενα
- Τμήματα εξήγησης εννοιών με διαγράμματα
- Αριθμημένες ασκήσεις με μπλοκ κώδικα και αναμενόμενη έξοδο
- Πίνακας συνοψίσεως, Σημαντικά Μαθήματα, Επεκτάσεις Ανάγνωσης
- Σύνδεσμος πλοήγησης στο επόμενο μέρος

Κατά την επεξεργασία περιεχομένου εργαστηρίου:
- Διατηρήστε το υπάρχον στυλ μορφοποίησης Markdown και την ιεραρχία τμημάτων.
- Τα μπλοκ κώδικα πρέπει να ορίζουν τη γλώσσα (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Παρέχετε εκδόσεις bash και PowerShell για εντολές shell όπου ο ρόλος του OS έχει σημασία.
- Χρησιμοποιήστε στυλ κλήσης προσοχής `> **Note:**`, `> **Tip:**` και `> **Troubleshooting:**`.
- Οι πίνακες χρησιμοποιούν τη μορφή με σωληνώσεις `| Header | Header |`.

## Εντολές Κατασκευής & Δοκιμών

| Ενέργεια | Εντολή |
|--------|---------|
| **Δείγματα Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **Δείγματα JS** | `cd javascript && npm install && node <script>.mjs` |
| **Δείγματα C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Δημιουργία διαγραμμάτων** | `npx mmdc -i <input>.mmd -o <output>.svg` (απαιτεί root `npm install`) |

## Εξωτερικές Εξαρτήσεις

- Το **Foundry Local CLI** πρέπει να είναι εγκατεστημένο στη μηχανή του προγραμματιστή (`winget install Microsoft.FoundryLocal` ή `brew install foundrylocal`).
- Η **υπηρεσία Foundry Local** τρέχει τοπικά και παρέχει ένα OpenAI-συμβατό REST API σε δυναμική θύρα.
- Δεν απαιτούνται υπηρεσίες cloud, κλειδιά API ή συνδρομές Azure για την εκτέλεση οποιουδήποτε δείγματος.
- Το Μέρος 10 (προσαρμοσμένα μοντέλα) απαιτεί επιπλέον `onnxruntime-genai` και κατεβάζει βάρη μοντέλων από το Hugging Face.

## Αρχεία που δεν πρέπει να δεσμευτούν

Το `.gitignore` πρέπει να αποκλείει (και συνήθως αποκλείει):
- `.venv/` — εικονικά περιβάλλοντα Python
- `node_modules/` — εξαρτήσεις npm
- `models/` — παραγόμενα αρχεία ONNX (μεγάλα δυαδικά αρχεία, δημιουργούνται από το Μέρος 10)
- `cache_dir/` — cache λήψης μοντέλων Hugging Face
- `.olive-cache/` — φάκελος εργασίας Microsoft Olive
- `samples/audio/*.wav` — παραγόμενα ηχητικά δείγματα (επαναδημιουργούνται με `python samples/audio/generate_samples.py`)
- Τυπικά αρχεία κατασκευής Python (`__pycache__/`, `*.egg-info/`, `dist/`, κλπ.)

## Άδεια

MIT — δείτε το αρχείο `LICENSE`.