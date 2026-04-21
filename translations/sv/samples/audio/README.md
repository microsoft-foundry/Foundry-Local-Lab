# Provljudfiler för Del 7 - Whisper rösttranskription

Dessa WAV-filer genereras med `pyttsx3` (Windows SAPI5 text-till-tal) och har tema kring **Zava DIY-produkter** från Creative Writer-demot.

## Generera proverna

```bash
# Från repo-roten - kräver .venv med pyttsx3 installerat
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Provfiler

| Fil | Scenario | Längd |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Kund som frågar om **Zava ProGrip sladdlös borrmaskin** - vridmoment, batteritid, bärväska | ~15 sek |
| `zava-product-review.wav` | Kundrecension av **Zava UltraSmooth interiörfärg** - täckning, torktid, låg VOC | ~22 sek |
| `zava-support-call.wav` | Supportärende om **Zava TitanLock verktygslåda** - ersättningsnycklar, extra lådsatser | ~20 sek |
| `zava-project-planning.wav` | Gör-det-själv-person som planerar en bakgårdsdäck med **Zava EcoBoard kompositdäck** & BrightBeam-lampor | ~25 sek |
| `zava-workshop-setup.wav` | Genomgång av en komplett verkstad med **alla fem Zava-produkter** | ~32 sek |
| `zava-full-project-walkthrough.wav` | Utökad genomgång av garage-renovering med **alla Zava-produkter** (för långt ljudtest, se [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Noteringar

- WAV-filerna är **incheckade** i repoet (listade i `. För att skapa nya .wav-filer, kör skriptet ovan för att återskapa nya skript eller modifiera för att skapa nya skript.
- Skriptet använder **Microsoft David** (US English) röst vid 160 WPM för tydliga transkriptionsresultat.
- Alla scenarier refererar till produkter från [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Ansvarsfriskrivning**:
Detta dokument har översatts med hjälp av AI-översättningstjänsten [Co-op Translator](https://github.com/Azure/co-op-translator). Även om vi strävar efter noggrannhet, vänligen var medveten om att automatiska översättningar kan innehålla fel eller brister. Det ursprungliga dokumentet på dess modersmål ska betraktas som den auktoritativa källan. För kritisk information rekommenderas professionell mänsklig översättning. Vi ansvarar inte för några missförstånd eller feltolkningar som uppstår från användningen av denna översättning.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->