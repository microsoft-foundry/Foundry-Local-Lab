# Ukážkové zvukové súbory pre časť 7 - Prepis hlasu Whisper

Tieto WAV súbory sú generované pomocou `pyttsx3` (Windows SAPI5 syntéza reči) a sú tematicky zamerané na **produkty Zava DIY** z demo verzie Creative Writer.

## Generovanie vzoriek

```bash
# Z koreňového adresára repozitára - vyžaduje sa .venv s nainštalovaným pyttsx3
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Ukážkové súbory

| Súbor | Scenár | Dĺžka |
|-------|--------|-------|
| `zava-customer-inquiry.wav` | Zákazník sa pýta na **Zava ProGrip bezdrôtovú vŕtačku** - krútiaci moment, výdrž batérie, prenosné puzdro | ~15 sek |
| `zava-product-review.wav` | Zákazník hodnotí **Zava UltraSmooth vnútornú farbu** - krytie, doba schnutia, nízky VOC | ~22 sek |
| `zava-support-call.wav` | Podpora volajúca ohľadom **Zava TitanLock pracovnej skrinky** - náhradné kľúče, ďalšie vložky do zásuviek | ~20 sek |
| `zava-project-planning.wav` | Domáci majster plánuje záhradnú terasu s **Zava EcoBoard kompozitným terasovým materiálom** a svetlami BrightBeam | ~25 sek |
| `zava-workshop-setup.wav` | Prehliadka kompletnej dielne s použitím **všetkých piatich produktov Zava** | ~32 sek |
| `zava-full-project-walkthrough.wav` | Rozšírená prehliadka renovácie garáže s použitím **všetkých produktov Zava** (pre testovanie dlhého zvuku pozri [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Poznámky

- WAV súbory sú **začlenené** v repozitári (uvedené v `. Na vytvorenie nových .wav súborov spustite vyššie uvedený skript na regeneráciu nových skriptov alebo upravte ich pre vytvorenie nových textov.
- Skript používa hlas **Microsoft David** (americká angličtina) rýchlosťou 160 slov za minútu pre jasné výsledky prepisu.
- Všetky scenáre odkazujú na produkty z [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).