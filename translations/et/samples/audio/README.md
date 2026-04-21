# Näidismuusikafailid osa 7 jaoks - Whisper hääle transkriptsioon

Need WAV-failid on genereeritud kasutades `pyttsx3` (Windows SAPI5 tekst kõneks) ja nende teemaks on **Zava DIY tooted** Creative Writer demonstratsioonist.

## Näidiste genereerimine

```bash
# Repo juurest - nõuab .venv-i, kus on paigaldatud pyttsx3
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Näidist failid

| Fail | Stsenaarium | Kestus |
|------|-------------|--------|
| `zava-customer-inquiry.wav` | Klient küsib **Zava ProGrip juhtmevaba puurmasina** kohta - pöördemoment, aku kestvus, kandekott | ~15 sek |
| `zava-product-review.wav` | Klient arvustab **Zava UltraSmooth sisevärvi** - katvus, kuivamisaeg, madal VOC | ~22 sek |
| `zava-support-call.wav` | Tugikõne **Zava TitanLock tööriistakasti** kohta - asendusvõtmed, lisasahtli vooderdised | ~20 sek |
| `zava-project-planning.wav` | Käsitööline planeerib tagaaia terrassi koos **Zava EcoBoard komposiitterrassi ja BrightBeami valgustitega** | ~25 sek |
| `zava-workshop-setup.wav` | Üksikasjalik töökojaseadistuse ülevaade kasutades **kõiki viit Zava toodet** | ~32 sek |
| `zava-full-project-walkthrough.wav` | Pika garaaži renoveerimise ülevaade kasutades **kõiki Zava tooteid** (pika audio testimiseks vaata [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Märkused

- WAV-failid on **repo lisatud** (loetletud failis `. Käivita ülaltoodud skript uute .wav failide loomiseks või muuda skripti uute stsenaariumite loomiseks.
- Skript kasutab **Microsoft David** (USA inglise keel) häält kiirusega 160 sõna minutis, et saavutada selged transkriptsiooni tulemused.
- Kõik stsenaariumid viitavad toodetele failist [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).