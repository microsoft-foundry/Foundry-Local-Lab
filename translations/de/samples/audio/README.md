# Beispiel-Audiodateien für Teil 7 - Whisper Sprachtranskription

Diese WAV-Dateien wurden mit `pyttsx3` (Windows SAPI5 Text-zu-Sprache) erzeugt und drehen sich um **Zava DIY-Produkte** aus der Creative Writer-Demo.

## Erstellung der Beispiele

```bash
# Vom Repo-Stamm - erfordert das .venv mit installiertem pyttsx3
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Beispiel-Dateien

| Datei | Szenario | Dauer |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Kunde fragt nach dem **Zava ProGrip Akkubohrer** - Drehmoment, Akkulaufzeit, Transportkoffer | ~15 Sek |
| `zava-product-review.wav` | Kunde bewertet die **Zava UltraSmooth Innenfarbe** - Deckkraft, Trockenzeit, niedrige VOC | ~22 Sek |
| `zava-support-call.wav` | Supportanruf zum **Zava TitanLock Werkzeugkasten** - Ersatzschlüssel, zusätzliche Schubladeneinlagen | ~20 Sek |
| `zava-project-planning.wav` | Heimwerker plant eine Terrassenanlage im Garten mit **Zava EcoBoard Verbund-Terrassendielen** & BrightBeam-Leuchten | ~25 Sek |
| `zava-workshop-setup.wav` | Rundgang durch eine komplette Werkstatt mit **allen fünf Zava-Produkten** | ~32 Sek |
| `zava-full-project-walkthrough.wav` | Erweiterter Garagenrenovierungsrundgang mit **allen Zava-Produkten** (für Langzeit-Audio-Tests siehe [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 Min |

## Hinweise

- WAV-Dateien sind im Repo **fest eingecheckt** (aufgelistet in `. Um neue .wav-Dateien zu erstellen, führen Sie das obige Skript aus, um neue Skripte zu generieren, oder passen Sie es an, um neue Skripte zu erstellen.
- Das Skript verwendet die Stimme **Microsoft David** (US-Englisch) mit 160 WPM für klare Transkriptionsergebnisse.
- Alle Szenarien beziehen sich auf Produkte aus [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).