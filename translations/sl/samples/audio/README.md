# Vzorec avdio datotek za 7. del - Prepis glasu Whisper

Te datoteke WAV so ustvarjene z uporabo `pyttsx3` (Windows SAPI5 pretvorba besedila v govor) in so tematsko povezane z **Zava DIY izdelki** iz demonstracije Creative Writer.

## Generiranje vzorcev

```bash
# Iz korena repozitorija - zahteva .venv z nameščenim pyttsx3
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Vzorcne datoteke

| Datoteka | Scenarij | Trajanje |
|----------|----------|----------|
| `zava-customer-inquiry.wav` | Stranka sprašuje o **Zava ProGrip brezžični vrtalnik** - navor, življenjska doba baterije, torbica za prenašanje | ~15 s |
| `zava-product-review.wav` | Stranka ocenjuje **Zava UltraSmooth notranjo barvo** - prekrivnost, čas sušenja, nizka vsebnost VOC | ~22 s |
| `zava-support-call.wav` | Klic podpore glede **Zava TitanLock orodjarne** - nadomestni ključi, dodatne podloge za predale | ~20 s |
| `zava-project-planning.wav` | Samograditelj načrtuje teraso v vrtu z **Zava EcoBoard kompozitno teraso** in svetilkami BrightBeam | ~25 s |
| `zava-workshop-setup.wav` | Pregled celotne delavnice z uporabo **vseh pet Zava izdelkov** | ~32 s |
| `zava-full-project-walkthrough.wav` | Podrobna predstavitev prenove garaže z uporabo **vseh Zava izdelkov** (za testiranje daljšega zvoka, glej [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Opombe

- Datoteke WAV so **zabeležene** v repozitoriju (navedene v `. Za izdelavo novih .wav datotek zaženite zgornjo skripto za ponovno ustvarjanje ali jo prilagodite za izdelavo novih skript.
- Skripta uporablja glas **Microsoft David** (ameriška angleščina) s hitrostjo 160 WPM za jasne rezultate prepisovanja.
- Vsi scenariji se nanašajo na izdelke iz [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Omejitev odgovornosti**:  
Ta dokument je bil preveden z uporabo AI prevajalske storitve [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za natančnost, upoštevajte, da avtomatizirani prevodi lahko vsebujejo napake ali netočnosti. Izvirni dokument v matičnem jeziku se šteje za avtoritativni vir. Za ključne informacije priporočamo strokovni človeški prevod. Za morebitna nesporazume ali napačne interpretacije, ki izhajajo iz uporabe tega prevoda, ne prevzemamo odgovornosti.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->