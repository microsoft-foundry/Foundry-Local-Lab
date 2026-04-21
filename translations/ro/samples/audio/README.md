# Fișiere audio de probă pentru Partea 7 - Transcriere vocală Whisper

Aceste fișiere WAV sunt generate folosind `pyttsx3` (text-to-speech Windows SAPI5) și au ca temă **produsele Zava DIY** din demo-ul Creative Writer.

## Generarea probelor

```bash
# Din rădăcina repo - necesită .venv cu pyttsx3 instalat
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Fișiere de probă

| Fișier | Scenariu | Durată |
|--------|----------|---------|
| `zava-customer-inquiry.wav` | Client care întreabă despre **Zava ProGrip Cordless Drill** - cuplu, durata bateriei, geantă de transport | ~15 sec |
| `zava-product-review.wav` | Client care evaluează **Zava UltraSmooth Interior Paint** - acoperire, timp de uscare, VOC redus | ~22 sec |
| `zava-support-call.wav` | Apel de suport pentru **Zava TitanLock Tool Chest** - chei de înlocuire, căptușeli suplimentare pentru sertare | ~20 sec |
| `zava-project-planning.wav` | Amator DIY planificând o terasă în curte cu **Zava EcoBoard Composite Decking** și lumini BrightBeam | ~25 sec |
| `zava-workshop-setup.wav` | Prezentare a unui atelier complet folosind **toate cele cinci produse Zava** | ~32 sec |
| `zava-full-project-walkthrough.wav` | Prezentare extinsă a renovării garajului folosind **toate produsele Zava** (pentru testare cu audio lung, vezi [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Note

- Fișierele WAV sunt **incorporate** în repo (listate în `. Pentru a crea fișiere .wav noi, rulați scriptul de mai sus pentru a regenera scripturi noi sau modificați pentru a crea scripturi noi.
- Scriptul folosește vocea **Microsoft David** (engleză SUA) la 160 WPM pentru rezultate clare la transcriere.
- Toate scenariile fac referire la produse din [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).