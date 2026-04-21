# Primjeri audio datoteka za Dio 7 - Prepisivanje glasa Whisperom

Ove WAV datoteke generirane su pomoću `pyttsx3` (Windows SAPI5 pretvaranje teksta u govor) i tematizirane su oko **Zava DIY proizvoda** iz demonstracije Creative Writer.

## Generiranje primjera

```bash
# Iz korijena repozitorija - zahtijeva .venv s instaliranim pyttsx3
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Primjerne datoteke

| Datoteka | Scenarij | Trajanje |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Kupac postavlja pitanja o **Zava ProGrip bežičnoj bušilici** - moment, trajanje baterije, transportna torba | ~15 sek |
| `zava-product-review.wav` | Kupac daje recenziju za **Zava UltraSmooth unutarnju boju** - pokrivenost, vrijeme sušenja, niska emisija hlapivih organskih spojeva (VOC) | ~22 sek |
| `zava-support-call.wav` | Poziv podrške o **Zava TitanLock radnom ormaru** - zamjenski ključevi, dodatne podloge ladica | ~20 sek |
| `zava-project-planning.wav` | Uradi-sam entuzijast planira terasu na dvorištu koristeći **Zava EcoBoard kompozitnu terasu** i BrightBeam svjetla | ~25 sek |
| `zava-workshop-setup.wav` | Pregled kompletne radionice koristeći **sve pet Zava proizvoda** | ~32 sek |
| `zava-full-project-walkthrough.wav` | Detaljan pregled preuređenja garaže koristeći **sve Zava proizvode** (za testiranje dugačkog zvuka, vidi [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Napomene

- WAV datoteke su **učitane** u repo (popisane u `. Za izradu novih .wav datoteka pokrenite gornji skript za regeneriranje novih scenarija ili ga modificirajte za kreiranje novih scenarija.
- Skripta koristi **Microsoft David** glas (američki engleski) s brzinom od 160 riječ/min za jasne rezultate prepisivanja.
- Svi scenariji se odnose na proizvode iz [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Odricanje od odgovornosti**:
Ovaj dokument preveden je pomoću AI usluge prevođenja [Co-op Translator](https://github.com/Azure/co-op-translator). Iako nastojimo osigurati točnost, imajte na umu da automatski prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na izvornom jeziku trebao bi se smatrati autoritativnim izvorom. Za kritične informacije preporuča se profesionalni ljudski prijevod. Nismo odgovorni za bilo kakva nesporazumevanja ili pogrešna tumačenja koja proizlaze iz korištenja ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->