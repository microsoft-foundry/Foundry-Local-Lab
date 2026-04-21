# Sampuli za Faili za Sauti kwa Sehemu ya 7 - Uandishi wa Sauti wa Whisper

Hizi faili za WAV zimetengenezwa kwa kutumia `pyttsx3` (Windows SAPI5 matamshi ya maandishi) na zimetengenezwa kwa mada ya **bidhaa za Zava DIY** kutoka kwa onyesho la Mwandishi Mbunifu.

## Tengeneza sampuli

```bash
# Kutoka kwenye mzizi wa repo - inahitaji .venv yenye pyttsx3 imewekwa
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Faili za sampuli

| Faili | Hali | Muda |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Mteja akiuliza kuhusu **Zava ProGrip Cordless Drill** - torque, maisha ya betri, kesi ya kubeba | ~15 sek |
| `zava-product-review.wav` | Mteja akikagua **rangi ya ndani ya Zava UltraSmooth** - ufunikaji, muda wa kukausha, VOC chini | ~22 sek |
| `zava-support-call.wav` | Simu ya msaada kuhusu **Zava TitanLock Tool Chest** - funguo za kubadilisha, liners za drawer za ziada | ~20 sek |
| `zava-project-planning.wav` | Mtu anapanga mradi wa jiko la nyuma na **Zava EcoBoard Composite Decking** & taa za BrightBeam | ~25 sek |
| `zava-workshop-setup.wav` | Maelezo ya warsha kamili inayotumia **bidhaa zote tano za Zava** | ~32 sek |
| `zava-full-project-walkthrough.wav` | Maelezo marefu ya ukarabati wa gereji kwa kutumia **bidhaa zote za Zava** (kwa majaribio ya sauti ndefu, angalia [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Vidokezo

- Faili za WAV zimeshafikishwa kwenye hifadhidata (zimeorodheshwa katika `. Kwa kuunda faili mpya za .wav endesha script iliyo juu kuunda scripts mpya au badilisha kuunda scripts mpya.
- Script inatumia sauti ya **Microsoft David** (Kiingereza cha Marekani) kwa 160 WPM kwa matokeo ya uandishi wazi.
- Hali zote zinahusisha bidhaa kutoka [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Kauli ya Msamaha**:  
Hati hii imetafsiriwa kwa kutumia huduma ya utafsiri ya AI [Co-op Translator](https://github.com/Azure/co-op-translator). Ingawa tunajitahidi kwa usahihi, tafadhali fahamu kuwa tafsiri za otomatiki zinaweza kuwa na makosa au upungufu wa usahihi. Hati ya asili katika lugha yake ya asili inapaswa kuzingatiwa kama chanzo cha mamlaka. Kwa taarifa muhimu, tafsiri ya kitaalamu kwa mtu inashauriwa. Hatuwajibiki kwa kutoelewana au tafsiri potofu yoyote inayotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->