# Minta Hangfájlok a 7. Részhez – Whisper Hangfelismerés

Ezeket a WAV fájlokat a `pyttsx3` (Windows SAPI5 szöveg-beszéddé alakítás) segítségével generálták, és a Creative Writer demó **Zava DIY termékei** témájára épülnek.

## Minták generálása

```bash
# A repo gyökérből - szükséges a .venv pyttsx3 telepítéssel
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Minta fájlok

| Fájl | Forgatókönyv | Időtartam |
|------|--------------|-----------|
| `zava-customer-inquiry.wav` | Ügyfél érdeklődik a **Zava ProGrip Akkumulátoros Fúró** iránt – nyomaték, akkumulátor élettartam, hordtáska | kb. 15 mp |
| `zava-product-review.wav` | Ügyfél véleményezi a **Zava UltraSmooth Belső Falfestéket** – fedés, száradási idő, alacsony VOC tartalom | kb. 22 mp |
| `zava-support-call.wav` | Támogatói hívás a **Zava TitanLock Szerszámosládához** – pótkulcsok, extra fiókbélés | kb. 20 mp |
| `zava-project-planning.wav` | Barkácsoló hátsó terasz tervezése a **Zava EcoBoard Kompozit Teraszelemeivel** & BrightBeam világítással | kb. 25 mp |
| `zava-workshop-setup.wav` | Végigvezetés egy teljes műhelyen, amely **mind az öt Zava terméket** alkalmazza | kb. 32 mp |
| `zava-full-project-walkthrough.wav` | Hosszabb garázsfelújítás bemutatása, amely **minden Zava terméket** használ (hosszú hanganyagos teszthez lásd: [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | kb. 4 perc |

## Megjegyzések

- A WAV fájlok **el vannak helyezve** a repóban (felsorolva `. A .wav fájlok újragenerálásához futtassa a fenti szkriptet új szkriptek létrehozásához vagy módosításhoz.
- A szkript a **Microsoft David** (US English) hangját használja 160 WPM sebességgel a tiszta átírás érdekében.
- Minden forgatókönyv a [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) fájlban szereplő termékekre hivatkozik.