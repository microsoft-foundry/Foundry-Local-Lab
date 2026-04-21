# Узорци аудио фајлова за део 7 - Whisper гласовна транскрипција

Ови WAV фајлови су генерисани користећи `pyttsx3` (Windows SAPI5 текст у говор) и тематски су везани за **Zava DIY производе** из Creative Writer демо-а.

## Генерисање узорака

```bash
# Из корена репозиторијума - захтева .venв са инсталираним pyttsx3
.venv\Scripts\Activate.ps1          # Виндоус
python samples/audio/generate_samples.py
```

## Узорци фајлова

| Фајл | Сценарио | Трајање |
|------|----------|---------|
| `zava-customer-inquiry.wav` | Купац пита о **Zava ProGrip бежичној бушилици** - обртни моменат, трајање батерије, кутија за ношење | око 15 сек |
| `zava-product-review.wav` | Купац даје осврт на **Zava UltraSmooth унутрашњу боју** - покривеност, време сушења, ниска VOC | око 22 сек |
| `zava-support-call.wav` | Подршка позив у вези са **Zava TitanLock алатком** - резервни кључеви, додаци за фиоке | око 20 сек |
| `zava-project-planning.wav` | ДИИ корисник планира дворишну терасу са **Zava EcoBoard композитним даскама** и BrightBeam осветљењем | око 25 сек |
| `zava-workshop-setup.wav` | Преглед комплетне радионице користећи **све пет Zava производа** | око 32 сек |
| `zava-full-project-walkthrough.wav` | Дуже вођење кроз реновирање гараже користећи **све Zava производе** (за тестирање дугог звука, видети [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | око 4 мин |

## Напомене

- WAV фајлови су **смерно додати** у репозиторијум (наведени у `. За креирање нових .wav фајлова покрените скрипту изнад да бисте регенерисали нове скрипте или модификовали за нове садржаје.
- Скрипта користи **Microsoft David** (амерички енглески) глас са 160 речи у минути за јасне резултате транскрипције.
- Сви сценарији реферишу на производе из [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).