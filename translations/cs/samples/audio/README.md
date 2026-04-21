# Vzorky zvukových souborů pro část 7 - Přepis hlasem pomocí Whisper

Tyto WAV soubory jsou vytvořeny pomocí `pyttsx3` (Windows SAPI5 převod textu na řeč) a jsou tématicky zaměřeny na **produkty Zava DIY** z ukázky Creative Writer.

## Generování vzorků

```bash
# Ze základního adresáře repozitáře - vyžaduje .venv s nainstalovaným pyttsx3
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Ukázkové soubory

| Soubor | Scénář | Délka |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Zákazník se ptá na **Zava ProGrip Bezdrátovou Vrtačku** - točivý moment, výdrž baterie, přepravní pouzdro | ~15 s |
| `zava-product-review.wav` | Zákazník hodnotí **Zava UltraSmooth Interiérovou Barvu** - krytí, doba schnutí, nízký obsah VOC | ~22 s |
| `zava-support-call.wav` | Podpora ohledně **Zava TitanLock Nářaďového Kufru** - náhradní klíče, vložky do zásuvek | ~20 s |
| `zava-project-planning.wav` | Hobby kutil plánuje terasu na zahradě s **Zava EcoBoard Kompozitním Terasovým Prknem** a osvětlením BrightBeam | ~25 s |
| `zava-workshop-setup.wav` | Prohlídka kompletní dílny využívající **všechny pět produktů Zava** | ~32 s |
| `zava-full-project-walkthrough.wav` | Rozšířená prohlídka renovace garáže s využitím **všech produktů Zava** (pro testování dlouhých nahrávek viz [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Poznámky

- WAV soubory jsou **uloženy** v repozitáři (vypsané v `. K vytvoření nových .wav souborů spusťte výše uvedený skript pro generování nových scénářů nebo upravte skripty pro vytvoření nových.
- Skript používá hlas **Microsoft David** (angličtina US) s rychlostí 160 slov za minutu pro jasný přepis.
- Všechny scénáře odkazují na produkty z [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).