# Przykładowe pliki audio do części 7 - Transkrypcja głosu Whisper

Te pliki WAV zostały wygenerowane przy użyciu `pyttsx3` (Windows SAPI5 syntezator mowy) i są tematyką dotyczącą **produktów Zava DIY** z demonstracji Creative Writer.

## Generowanie próbek

```bash
# Z katalogu głównego repozytorium - wymaga .venv z zainstalowanym pyttsx3
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Przykładowe pliki

| Plik | Scenariusz | Czas trwania |
|------|------------|--------------|
| `zava-customer-inquiry.wav` | Klient pyta o **Zava ProGrip Cordless Drill** - moment obrotowy, żywotność baterii, etui transportowe | ~15 sek |
| `zava-product-review.wav` | Klient ocenia **Zava UltraSmooth Interior Paint** - krycie, czas schnięcia, niska zawartość VOC | ~22 sek |
| `zava-support-call.wav` | Telefon do wsparcia dotyczący **Zava TitanLock Tool Chest** - zapasowe klucze, dodatkowe wykładziny do szuflad | ~20 sek |
| `zava-project-planning.wav` | Majster planuje taras w ogrodzie z wykorzystaniem **Zava EcoBoard Composite Decking** & lamp BrightBeam | ~25 sek |
| `zava-workshop-setup.wav` | Prezentacja kompletnego warsztatu z użyciem **wszystkich pięciu produktów Zava** | ~32 sek |
| `zava-full-project-walkthrough.wav` | Rozbudowany opis remontu garażu z użyciem **wszystkich produktów Zava** (do testów z długim audio, zobacz [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Uwagi

- Pliki WAV są **zatwierdzone** w repozytorium (wymienione w `. Aby utworzyć nowe pliki .wav uruchom skrypt powyżej, aby wygenerować nowe skrypty lub zmodyfikuj aby utworzyć nowe skrypty.
- Skrypt używa głosu **Microsoft David** (angielski US) z prędkością 160 WPM dla jasnych wyników transkrypcji.
- Wszystkie scenariusze odwołują się do produktów z [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).