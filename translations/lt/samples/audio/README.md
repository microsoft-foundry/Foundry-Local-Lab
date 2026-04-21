# Pavyzdiniai garso failai 7 daliai – Whisper balso transkripcija

Šie WAV failai sugeneruoti naudojant `pyttsx3` (Windows SAPI5 teksto į kalbą) ir yra susiję su **Zava DIY produktais** iš Creative Writer demonstracijos.

## Sugeneruoti pavyzdžius

```bash
# Iš repozitorijos šaknies - reikalinga .venv su įdiegta pyttsx3
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Pavyzdiniai failai

| Failas | Scenarijus | Trukmė |
|--------|------------|--------|
| `zava-customer-inquiry.wav` | Kliento užklausa apie **Zava ProGrip belaidį grąžtą** – sukimo momentas, baterijos veikimo laikas, laikymo dėklas | ~15 sek |
| `zava-product-review.wav` | Kliento atsiliepimas apie **Zava UltraSmooth vidaus dažus** – dengiamumas, džiūvimo laikas, mažas VOC kiekis | ~22 sek |
| `zava-support-call.wav` | Pagalbos skambutis dėl **Zava TitanLock įrankių spintelės** – atsarginiai raktai, papildomos stalčių kilimėliai | ~20 sek |
| `zava-project-planning.wav` | Meistras planuoja kiemo terasą su **Zava EcoBoard kompozitine terasa** ir BrightBeam apšvietimu | ~25 sek |
| `zava-workshop-setup.wav` | Išsamus darbo vietos aprašymas naudojant **visus penkis Zava produktus** | ~32 sek |
| `zava-full-project-walkthrough.wav` | Išsamus garažo remonto apžiūros įrašas naudojant **visus Zava produktus** (ilgam garso testavimui, žr. [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Pastabos

- WAV failai yra **įtraukti** į repozitoriją (išvardyti `. Norėdami sukurti naujų .wav failų, paleiskite aukščiau pateiktą skriptą, kad sugeneruotumėte naujus scenarijus, arba pakeiskite jį, kad sukurtumėte naujus scenarijus.
- Skriptas naudoja **Microsoft David** (JAV anglų kalba) balsą, kalbant 160 žodžių per minutę, kad būtų aiškūs transkripcijos rezultatai.
- Visuose scenarijuose nurodyti produktai yra iš [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės apribojimas**:  
Šis dokumentas buvo išverstas naudojant dirbtinio intelekto vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors siekiame tikslumo, atkreipkite dėmesį, kad automatiniai vertimai gali turėti klaidų ar netikslumų. Originalus dokumentas jo gimtąja kalba turėtų būti laikomas autoritetingu šaltiniu. Kritiškai svarbiai informacijai rekomenduojamas profesionalus žmogaus vertimas. Mes neatsakome už jokius nesusipratimus ar neteisingus aiškinimus, kylantčius dėl šio vertimo naudojimo.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->