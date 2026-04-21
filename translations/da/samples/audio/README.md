# Prøve lydfiler til del 7 - Whisper stemmetransskription

Disse WAV-filer er genereret med `pyttsx3` (Windows SAPI5 tekst-til-tale) og har tema omkring **Zava DIY produkter** fra Creative Writer-demoen.

## Generer prøverne

```bash
# Fra repo-roden - kræver .venv med pyttsx3 installeret
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Prøvefiler

| Fil | Scenario | Varighed |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Kunde, der spørger om **Zava ProGrip ledningsfri boremaskine** - drejningsmoment, batterilevetid, bæretaske | ~15 sek |
| `zava-product-review.wav` | Kundeanmeldelse af **Zava UltraSmooth indvendig maling** - dækningsgrad, tørretid, lavt VOC | ~22 sek |
| `zava-support-call.wav` | Supportopkald om **Zava TitanLock værktøjskasse** - reservnøgler, ekstra skuffeforinger | ~20 sek |
| `zava-project-planning.wav` | Gør-det-selv person planlægger en baghave terrasse med **Zava EcoBoard komposit terrassebrædder** & BrightBeam lamper | ~25 sek |
| `zava-workshop-setup.wav` | Gennemgang af et komplet værksted med **alle fem Zava produkter** | ~32 sek |
| `zava-full-project-walkthrough.wav` | Udvidet gennemgang af garage-renovering med **alle Zava produkter** (til lang lyd test, se [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Noter

- WAV-filer er **committed** til repoet (listet i `. For at oprette nye .wav-filer skal du køre scriptet ovenfor for at regenerere nye scripts eller ændre for at lave nye scripts.
- Scriptet bruger **Microsoft David** (amerikansk engelsk) stemme ved 160 WPM for klare transskriptionsresultater.
- Alle scenarier refererer til produkter fra [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfraskrivelse**:  
Dette dokument er blevet oversat ved brug af AI-oversættelsestjenesten [Co-op Translator](https://github.com/Azure/co-op-translator). Selvom vi stræber efter nøjagtighed, bedes du være opmærksom på, at automatiserede oversættelser kan indeholde fejl eller unøjagtigheder. Det oprindelige dokument på dets modersmål bør betragtes som den autoritative kilde. For kritisk information anbefales professionel menneskelig oversættelse. Vi påtager os intet ansvar for misforståelser eller fejltolkninger, der måtte opstå som følge af brugen af denne oversættelse.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->