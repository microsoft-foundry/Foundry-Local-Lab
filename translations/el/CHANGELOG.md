# Αρχείο αλλαγών — Foundry Local Workshop

Όλες οι αξιοσημείωτες αλλαγές σε αυτό το εργαστήριο τεκμηριώνονται παρακάτω.

---

## 2026-03-11 — Μέρος 12 & 13, Web UI, Ανασυγκρότηση Whisper, Διόρθωση WinML/QNN, και Επικύρωση

### Προστέθηκαν
- **Μέρος 12: Δημιουργία Web UI για τον Zava Creative Writer** — νέος οδηγός εργαστηρίου (`labs/part12-zava-ui.md`) με ασκήσεις που καλύπτουν streaming NDJSON, browser `ReadableStream`, σήματα κατάστασης ζωντανού agent, και streaming κειμένου άρθρου σε πραγματικό χρόνο
- **Μέρος 13: Ολοκλήρωση εργαστηρίου** — νέο συνοπτικό εργαστήριο (`labs/part13-workshop-complete.md`) με ανακεφαλαίωση όλων των 12 μερών, επιπλέον ιδέες, και συνδέσμους πόρων
- **Εμπρόσθιο μέρος Zava UI:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — κοινή αρχική HTML/CSS/JS διεπαφή browser που χρησιμοποιείται από όλα τα τρία backend
- **Διακομιστής HTTP JavaScript:** `zava-creative-writer-local/src/javascript/server.mjs` — νέος Express-style HTTP server που περιτυλίγει τον orchestrator για πρόσβαση μέσω browser
- **Backend C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` και `ZavaCreativeWriterWeb.csproj` — νέο ελάχιστο API project που εξυπηρετεί το UI και streaming NDJSON
- **Γεννήτρια δειγμάτων ήχου:** `samples/audio/generate_samples.py` — offline TTS σενάριο με χρήση `pyttsx3` για παραγωγή WAV αρχείων θεματολογίας Zava για Μέρος 9
- **Δείγμα ήχου:** `samples/audio/zava-full-project-walkthrough.wav` — νέο μακρύτερο δείγμα ήχου για δοκιμές απομαγνητοφώνησης
- **Σενάριο επικύρωσης:** `validate-npu-workaround.ps1` — αυτοματοποιημένο PowerShell script για επικύρωση της λύσης παράκαμψης NPU/QNN σε όλα τα δείγματα C#
- **Διαγράμματα Mermaid σε SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **Διαπολιτισμική υποστήριξη WinML:** Όλα τα 3 αρχεία C# `.csproj` (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) χρησιμοποιούν πλέον συνθήκες TFM και αμοιβαία αποκλειόμενες αναφορές πακέτων για υποστήριξη πολλαπλών πλατφορμών. Στα Windows: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (υποσύνολο που περιλαμβάνει το πρόσθετο QNN EP). Σε μη-Windows: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (βασικό SDK). Ο σκληροκωδικοποιημένος RID `win-arm64` στα έργα Zava αντικαταστάθηκε με αυτόματη ανίχνευση. Μία μεταβατική παράκαμψη εξαιρεί εγγενή assets από το `Microsoft.ML.OnnxRuntime.Gpu.Linux` που είχε ελαττωματική αναφορά για win-arm64. Η προηγούμενη προσπάθεια/παγίδα παράκαμψης NPU έχει αφαιρεθεί από όλα τα 7 αρχεία C#.

### Αλλάχθηκαν
- **Μέρος 9 (Whisper):** Εκτενής ανασυγκρότηση — το JavaScript πλέον χρησιμοποιεί το ενσωματωμένο `AudioClient` του SDK (`model.createAudioClient()`) αντί για χειροκίνητη ONNX Runtime εκτέλεση· ενημερωμένες περιγραφές αρχιτεκτονικής, πίνακες σύγκρισης και διαγράμματα ροής για τη μέθοδο JS/C# `AudioClient` έναντι της μεθόδου Python ONNX Runtime
- **Μέρος 11:** Ενημερωμένοι σύνδεσμοι πλοήγησης (ανακατευθύνουν τώρα στο Μέρος 12)· προστέθηκαν αποδοσμένα διαγράμματα SVG για τη ροή κλήσεων εργαλείων και τη σειρά
- **Μέρος 10:** Ενημερωμένη πλοήγηση που οδηγεί μέσω Μέρος 12 αντί να τερματίζει το εργαστήριο
- **Python Whisper (`foundry-local-whisper.py`):** Επεκτάθηκε με επιπλέον δείγματα ήχου και βελτιωμένη διαχείριση σφαλμάτων
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** Ανασυγκροτήθηκε για να χρησιμοποιεί `model.createAudioClient()` με `audioClient.transcribe()` αντί για χειροκίνητες ONNX Runtime συνεδρίες
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** Ενημερώθηκε για να εξυπηρετεί στατικά αρχεία UI παράλληλα με το API
- **Zava C# κονσόλα (`zava-creative-writer-local/src/csharp/Program.cs`):** Αφαιρέθηκε η παράκαμψη NPU (αντιμετωπίζεται τώρα από το πακέτο WinML)
- **README.md:** Προστέθηκε ενότητα Μέρος 12 με πίνακες δειγμάτων κώδικα και προσθήκες backend· προστέθηκε ενότητα Μέρος 13· ενημερώθηκαν οι μαθησιακοί στόχοι και η δομή έργου
- **KNOWN-ISSUES.md:** Αφαιρέθηκε το λυμένο Θέμα #7 (C# SDK NPU Model Variant — τώρα αντιμετωπίζεται με το πακέτο WinML). Ξανααρίθμηση των υπολοίπων θεμάτων σε #1–#6. Ενημέρωση λεπτομερειών περιβάλλοντος με .NET SDK 10.0.104
- **AGENTS.md:** Ενημέρωση δομής έργου με νέες καταχωρήσεις `zava-creative-writer-local` (`ui/`, `csharp-web/`, `server.mjs`); ενημέρωση βασικών πακέτων C# και λεπτομερειών conditional TFM
- **labs/part2-foundry-local-sdk.md:** Ενημερωμένο παράδειγμα `.csproj` με πλήρες μοτίβο cross-platform, conditional TFM, αμοιβαία αποκλειόμενες αναφορές πακέτων και επεξηγηματική υποσημείωση

### Επικυρώθηκαν
- Όλα τα 3 έργα C# (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) χτίζονται επιτυχώς σε Windows ARM64
- Παράδειγμα chat (`dotnet run chat`): το μοντέλο φορτώνεται ως `phi-3.5-mini-instruct-qnn-npu:1` μέσω WinML/QNN — η παραλλαγή NPU φορτώνεται απευθείας χωρίς πτώση σε CPU
- Παράδειγμα agent (`dotnet run agent`): εκτελείται end-to-end με συνομιλία πολλαπλών γύρων, κωδικός εξόδου 0
- Foundry Local CLI v0.8.117 και SDK v0.9.0 σε .NET SDK 9.0.312

---

## 2026-03-11 — Διορθώσεις Κώδικα, Καθαρισμός Μοντέλων, Διαγράμματα Mermaid, και Επικύρωση

### Διορθώσεις
- **Όλα τα 21 δείγματα κώδικα (7 Python, 7 JavaScript, 7 C#):** Προσθήκη `model.unload()` / `unload_model()` / `model.UnloadAsync()` καθαρισμού κατά την έξοδο για επίλυση προειδοποιήσεων διαρροής μνήμης OGA (Γνωστό Ζήτημα #4)
- **csharp/WhisperTranscription.cs:** Αντικατάσταση εύθραυστου σχετικού μονοπατιού `AppContext.BaseDirectory` με `FindSamplesDirectory()` που περπατάει τους καταλόγους προς τα πάνω για αξιόπιστη εύρεση `samples/audio` (Γνωστό Ζήτημα #7)
- **csharp/csharp.csproj:** Αντικατάσταση της σκληροκωδικοποιημένης τιμής `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` με αυτόματη ανίχνευση πτώσης μέσω `$(NETCoreSdkRuntimeIdentifier)` ώστε το `dotnet run` να λειτουργεί σε όλες τις πλατφόρμες χωρίς την παράμετρο `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### Αλλαγές
- **Μέρος 8:** Μετατροπή βρόχου επαναληπτικής αξιολόγησης από διάγραμμα ASCII κουτιού σε αποδοσμένη εικόνα SVG
- **Μέρος 10:** Μετατροπή διαγράμματος pipeline μεταγλώττισης από βέλη ASCII σε αποδοσμένη εικόνα SVG
- **Μέρος 11:** Μετατροπή διαγραμμάτων ροής κλήσης εργαλείων και ακολουθίας σε αποδοσμένες εικόνες SVG
- **Μέρος 10:** Μετακίνηση ενότητας "Workshop Complete!" στο Μέρος 11 (τελικό εργαστήριο); αντικατάσταση με σύνδεσμο "Επόμενα Βήματα"
- **KNOWN-ISSUES.md:** Πλήρης επανεπιβεβαίωση όλων των θεμάτων σε CLI v0.8.117. Αφαίρεση λύσεων: διαρροή μνήμης OGA (προστέθηκε καθαρισμός), διαδρομή Whisper (FindSamplesDirectory), HTTP 500 συνεχιζόμενη εκτέλεση (μη αναπαραγώγιμη, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), περιορισμοί tool_choice (τώρα λειτουργεί με "required" και συγκεκριμένη στόχευση λειτουργιών στο qwen2.5-0.5b). Ενημερώθηκε ζήτημα JS Whisper — τώρα όλα τα αρχεία επιστρέφουν κενή/δυαδική έξοδο (υποβάθμιση από v0.9.x, αυξημένη σοβαρότητα σε Μεγάλο). Ενημερώθηκε το RID C# #4 με παράκαμψη αυτόματης ανίχνευσης και σύνδεσμος [#497](https://github.com/microsoft/Foundry-Local/issues/497). Παραμένουν 7 ανοικτά θέματα.
- **javascript/foundry-local-whisper.mjs:** Διορθώθηκε το όνομα μεταβλητής καθαρισμού (`whisperModel` → `model`)

### Επικυρώθηκαν
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — εκτελούνται επιτυχώς με καθαρισμό
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — εκτελούνται επιτυχώς με καθαρισμό
- C#: `dotnet build` πετυχαίνει με 0 προειδοποιήσεις, 0 σφάλματα (στόχος net9.0)
- Όλα τα 7 αρχεία Python περνούν `py_compile` έλεγχο σύνταξης
- Όλα τα 7 αρχεία JavaScript περνούν έλεγχο σύνταξης με `node --check`

---

## 2026-03-10 — Μέρος 11: Κλήση Εργαλείων, Επέκταση SDK API, και Κάλυψη Μοντέλων

### Προστέθηκαν
- **Μέρος 11: Κλήση Εργαλείων με Τοπικά Μοντέλα** — νέος οδηγός εργαστηρίου (`labs/part11-tool-calling.md`) με 8 ασκήσεις που καλύπτουν σχήματα εργαλείων, ροή πολλαπλών γύρων, πολλαπλές κλήσεις εργαλείων, προσαρμοσμένα εργαλεία, κλήση εργαλείων μέσω ChatClient, και `tool_choice`
- **Δείγμα Python:** `python/foundry-local-tool-calling.py` — κλήση εργαλείων `get_weather`/`get_population` με χρήση OpenAI SDK
- **Δείγμα JavaScript:** `javascript/foundry-local-tool-calling.mjs` — κλήση εργαλείων με το εγγενές `ChatClient` του SDK (`model.createChatClient()`)
- **Δείγμα C#:** `csharp/ToolCalling.cs` — κλήση εργαλείων με `ChatTool.CreateFunctionTool()` και το OpenAI C# SDK
- **Μέρος 2, Άσκηση 7:** Εμφυτευμένο `ChatClient` — `model.createChatClient()` (JS) και `model.GetChatClientAsync()` (C#) ως εναλλακτικές στο OpenAI SDK
- **Μέρος 2, Άσκηση 8:** Παραλλαγές μοντέλων και επιλογή υλικού — `selectVariant()`, `variants`, πίνακας παραλλαγών NPU (7 μοντέλα)
- **Μέρος 2, Άσκηση 9:** Αναβαθμίσεις μοντέλου και ανανέωση καταλόγου — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **Μέρος 2, Άσκηση 10:** Μοντέλα συλλογισμού — `phi-4-mini-reasoning` με παραδείγματα ανάλυσης ετικέτας `<think>`
- **Μέρος 3, Άσκηση 4:** `createChatClient` ως εναλλακτική στο OpenAI SDK, με τεκμηρίωση προτύπου stream callback
- **AGENTS.md:** Προστέθηκαν συμβάσεις κωδικοποίησης για Κλήση Εργαλείων, ChatClient, και Μοντέλα Συλλογισμού

### Αλλάχθηκαν
- **Μέρος 1:** Επεκτάθηκε ο κατάλογος μοντέλων — προσθήκη phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **Μέρος 2:** Ενημερωμένοι πίνακες αναφοράς API — προσθήκη `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **Μέρος 2:** Επανααρίθμηση ασκήσεων 7-9 → 10-13 για προσθήκη νέων ασκήσεων
- **Μέρος 3:** Ενημερωμένος πίνακας σημαντικών σημείων για να περιλαμβάνει το εγγενές ChatClient
- **README.md:** Προστέθηκε ενότητα Μέρος 11 με πίνακα δειγμάτων κώδικα; προστέθηκε μαθησιακός στόχος #11; ενημερώθηκε δενδρική δομή έργου
- **csharp/Program.cs:** Προστέθηκε περίπτωση `toolcall` στο router CLI και ενημερώθηκε το κείμενο βοήθειας

---

## 2026-03-09 — Ενημέρωση SDK v0.9.0, Βρετανικά Αγγλικά, και Δοκιμή Επικύρωσης

### Αλλάχθηκαν
- **Όλα τα δείγματα κώδικα (Python, JavaScript, C#):** Ενημερώθηκαν στη Foundry Local SDK v0.9.0 API — διορθώθηκε `await catalog.getModel()` (έλειπε το `await`), ενημέρωση προτύπων αρχικοποίησης FoundryLocalManager, διόρθωση ανίχνευσης endpoint
- **Όλοι οι οδηγοί εργαστηρίου (Μέρη 1-10):** Μεταφρασμένοι σε βρετανικά αγγλικά (colour, catalogue, optimised, κ.λπ.)
- **Όλοι οι οδηγοί εργαστηρίου:** Ενημερωμένα παραδείγματα SDK για να ταιριάζουν με την επιφάνεια API v0.9.0
- **Όλοι οι οδηγοί εργαστηρίου:** Ενημερωμένοι πίνακες αναφοράς API και μπλοκ κώδικα ασκήσεων
- **Κρίσιμη διόρθωση JavaScript:** Προσθήκη του ελλείποντος `await` σε `catalog.getModel()` — επέστρεφε `Promise`, όχι αντικείμενο `Model`, προκαλώντας σιωπηλές αποτυχίες

### Επικυρώθηκαν
- Όλα τα δείγματα Python τρέχουν επιτυχώς με την υπηρεσία Foundry Local
- Όλα τα δείγματα JavaScript τρέχουν επιτυχώς (Node.js 18+)
- Το έργο C# χτίζεται και τρέχει σε .NET 9.0 (προσαρμογή από net8.0 SDK assembly)
- Τροποποιήθηκαν και επικυρώθηκαν 29 αρχεία σε όλο το εργαστήριο

---

## Ευρετήριο Αρχείων

| Αρχείο | Τελευταία Ενημέρωση | Περιγραφή |
|--------|---------------------|-----------|
| `labs/part1-getting-started.md` | 2026-03-10 | Επεκταμένος κατάλογος μοντέλων |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | Νέες ασκήσεις 7-10, επεκταμένοι πίνακες API |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | Νέα Άσκηση 4 (ChatClient), ενημερωμένα συμπεράσματα |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + Βρετανικά Αγγλικά |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + Βρετανικά Αγγλικά |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid διάγραμμα |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid διάγραμμα, μεταφέρθηκε το Workshop Complete στο Μέρος 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | Νέο εργαστήριο, Mermaid διαγράμματα, ενότητα Workshop Complete |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | Νέο: παράδειγμα κλήσης εργαλείου |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | Νέο: παράδειγμα κλήσης εργαλείου |
| `csharp/ToolCalling.cs` | 2026-03-10 | Νέο: παράδειγμα κλήσης εργαλείου |
| `csharp/Program.cs` | 2026-03-10 | Προστέθηκε εντολή CLI `toolcall` |
| `README.md` | 2026-03-10 | Μέρος 11, δομή έργου |
| `AGENTS.md` | 2026-03-10 | Κλήση εργαλείων + Συμβάσεις ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | Αφαιρέθηκε το επιλυμένο Πρόβλημα #7, παραμένουν 6 ανοιχτά προβλήματα |
| `csharp/csharp.csproj` | 2026-03-11 | Cross-platform TFM, WinML/base SDK προϋποθέσεις υπό όρους |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Cross-platform TFM, αυτόματη ανίχνευση RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Cross-platform TFM, αυτόματη ανίχνευση RID |
| `csharp/BasicChat.cs` | 2026-03-11 | Αφαιρέθηκε η παράκαμψη try/catch NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | Αφαιρέθηκε η παράκαμψη try/catch NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | Αφαιρέθηκε η παράκαμψη try/catch NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | Αφαιρέθηκε η παράκαμψη try/catch NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | Αφαιρέθηκε η παράκαμψη try/catch NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Παράδειγμα .csproj για cross-platform |
| `AGENTS.md` | 2026-03-11 | Ενημερωμένα πακέτα C# και λεπτομέρειες TFM |
| `CHANGELOG.md` | 2026-03-11 | Αυτό το αρχείο |