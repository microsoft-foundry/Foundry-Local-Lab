# Voorbeeld Audiobestanden voor Deel 7 - Whisper Spraaktranscriptie

Deze WAV-bestanden zijn gegenereerd met `pyttsx3` (Windows SAPI5 tekst-naar-spraak) en hebben als thema **Zava DIY-producten** uit de Creative Writer-demo.

## Genereer de voorbeelden

```bash
# Vanuit de root van de repo - vereist de .venv met pyttsx3 geïnstalleerd
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Voorbeeldbestanden

| Bestand | Scenario | Duur |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Klant vraagt naar de **Zava ProGrip Draadloze Boormachine** - koppel, batterijduur, draagtas | ~15 sec |
| `zava-product-review.wav` | Klant beoordeelt de **Zava UltraSmooth Interieurverf** - dekking, droogtijd, lage VOC | ~22 sec |
| `zava-support-call.wav` | Ondersteuningsoproep over de **Zava TitanLock Gereedschapskist** - vervangende sleutels, extra ladevoeringen | ~20 sec |
| `zava-project-planning.wav` | Doe-het-zelver plant een achtertuinterras met **Zava EcoBoard Composite Decking** & BrightBeam-verlichting | ~25 sec |
| `zava-workshop-setup.wav` | Rondleiding door een complete werkplaats met **alle vijf Zava-producten** | ~32 sec |
| `zava-full-project-walkthrough.wav` | Uitgebreide rondleiding door een garage-renovatie met **alle Zava-producten** (voor lange-audio testen, zie [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Notities

- WAV-bestanden zijn **toegevoegd** aan de repository (vermeld in `. Om nieuwe .wav-bestanden te maken, voer het bovenstaande script uit om nieuwe scripts te genereren of pas aan om nieuwe scripts te maken.
- Het script gebruikt de stem van **Microsoft David** (US Engels) op 160 WPM voor duidelijke transcriptieresultaten.
- Alle scenario's verwijzen naar producten uit [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).