# Näyteäänitiedostot osaan 7 - Whisper-äänen transkriptio

Nämä WAV-tiedostot on luotu käyttämällä `pyttsx3`-kirjastoa (Windows SAPI5 -tekstistä puheeksi) ja ne liittyvät **Zava DIY -tuotteisiin** Creative Writer -demosta.

## Luo näytteet

```bash
# Repo juuresta - vaatii .venv:n, johon on asennettu pyttsx3
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Näytetiedostot

| Tiedosto | Tilanne | Kesto |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Asiakas kyselee **Zava ProGrip Cordless Drill** -ruuvinvääntimestä – vääntömomentti, akun kesto, kantokotelo | ~15 s |
| `zava-product-review.wav` | Asiakas arvostelee **Zava UltraSmooth Interior Paint** -sisämaalin – peittävyys, kuivumisaika, vähäpäästöisyys | ~22 s |
| `zava-support-call.wav` | Tukipuhelu koskien **Zava TitanLock Tool Chest** -työkalulaatikkoa – varmuusavaimet, lisälaatikon vuoraukset | ~20 s |
| `zava-project-planning.wav` | Tee-se-itse-harrastaja suunnittelee takapihan terassia käyttäen **Zava EcoBoard Composite Decking** -materiaalin ja BrightBeam-valaisimet | ~25 s |
| `zava-workshop-setup.wav` | Kattava esittely työpajasta, jossa käytetään **kaikkia viittä Zava-tuotetta** | ~32 s |
| `zava-full-project-walkthrough.wav` | Pitkä garage-remontin esittely käyttäen **kaikkia Zava-tuotteita** (pitkien äänitteiden testaukseen, katso [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Huomautukset

- WAV-tiedostot on **sitoutettu** repositorioon (lista on tiedostossa`). Uusien .wav-tiedostojen luomiseksi suorita yllä oleva skripti uudelleen tai muokkaa sitä luomaan uusia skriptejä.
- Skripti käyttää **Microsoft David** (US English) -ääntä nopeudella 160 wpm varmistaakseen selkeän transkription.
- Kaikki tilanteet viittaavat tuotteisiin tiedostossa [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).