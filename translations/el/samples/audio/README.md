# Δείγματα Αρχείων Ήχου για το Μέρος 7 - Μεταγραφή Φωνής Whisper

Αυτά τα αρχεία WAV έχουν δημιουργηθεί χρησιμοποιώντας `pyttsx3` (Windows SAPI5 φωνητική σύνθεση κειμένου) και έχουν ως θέμα τα **προϊόντα Zava DIY** από το demo Creative Writer.

## Δημιουργία των δειγμάτων

```bash
# Από τη ρίζα του αποθετηρίου - απαιτεί το .venv με εγκατεστημένο το pyttsx3
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Αρχεία δειγμάτων

| Αρχείο | Σενάριο | Διάρκεια |
|--------|---------|----------|
| `zava-customer-inquiry.wav` | Πελάτης που ρωτά για το **Zava ProGrip Cordless Drill** - ροπή, διάρκεια μπαταρίας, θήκη μεταφοράς | ~15 δευτ. |
| `zava-product-review.wav` | Πελάτης που αξιολογεί το **Zava UltraSmooth Interior Paint** - κάλυψη, χρόνος στεγνώματος, χαμηλά VOC | ~22 δευτ. |
| `zava-support-call.wav` | Κλήση υποστήριξης για το **Zava TitanLock Tool Chest** - ανταλλακτικά κλειδιά, επιπλέον εσωτερικές επενδύσεις συρταριών | ~20 δευτ. |
| `zava-project-planning.wav` | DIYer που σχεδιάζει μια βεράντα στον κήπο με **Zava EcoBoard Composite Decking** & φώτα BrightBeam | ~25 δευτ. |
| `zava-workshop-setup.wav` | Ξενάγηση σε ένα πλήρες εργαστήριο χρησιμοποιώντας **και τα πέντε προϊόντα Zava** | ~32 δευτ. |
| `zava-full-project-walkthrough.wav` | Εκτενής παρουσίαση ανακαίνισης γκαράζ με χρήση **όλων των προϊόντων Zava** (για δοκιμές μεγάλης διάρκειας ήχου, δείτε [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 λεπτά |

## Σημειώσεις

- Τα αρχεία WAV είναι **δεσμευμένα** στο αποθετήριο (αναφέρονται στο . Για να δημιουργήσετε νέα αρχεία .wav τρέξτε το παραπάνω script για να επαναδημιουργήσετε νέα σενάρια ή τροποποιήστε για να δημιουργήσετε νέα σενάρια.
- Το script χρησιμοποιεί τη φωνή **Microsoft David** (US English) στα 160 λέξεις ανά λεπτό για καθαρά αποτελέσματα μεταγραφής.
- Όλα τα σενάρια αναφέρονται σε προϊόντα από το [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).