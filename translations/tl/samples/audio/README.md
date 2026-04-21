# Mga Halimbawang Audio File para sa Bahagi 7 - Whisper Voice Transcription

Ang mga WAV file na ito ay nilikha gamit ang `pyttsx3` (Windows SAPI5 text-to-speech) at may temang tungkol sa **Zava DIY products** mula sa Creative Writer demo.

## Pagbuo ng mga sample

```bash
# Mula sa ugat ng repo - nangangailangan ng .venv na may naka-install na pyttsx3
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Mga sample na file

| File | Scenario | Tagal |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Customer na nagtatanong tungkol sa **Zava ProGrip Cordless Drill** - torque, buhay ng baterya, carrying case | ~15 seg |
| `zava-product-review.wav` | Customer na nagre-review ng **Zava UltraSmooth Interior Paint** - coverage, oras ng pagpapatuyo, mababang VOC | ~22 seg |
| `zava-support-call.wav` | Tawag sa suporta tungkol sa **Zava TitanLock Tool Chest** - pamalit na mga susi, dagdag na drawer liners | ~20 seg |
| `zava-project-planning.wav` | DIYer na nagpaplano ng backyard deck gamit ang **Zava EcoBoard Composite Decking** at BrightBeam lights | ~25 seg |
| `zava-workshop-setup.wav` | Paglalahad ng isang kompletong workshop gamit ang **lahat ng limang Zava products** | ~32 seg |
| `zava-full-project-walkthrough.wav` | Pinalawig na walkthrough ng pag-renovate ng garahe gamit ang **lahat ng Zava products** (para sa long-audio testing, tingnan ang [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Tala

- Ang mga WAV file ay **nakakabit** sa repo (naka-lista sa `. Upang makalikha ng mga bagong .wav file, patakbuhin ang script sa itaas upang muling likhain ang mga script o i-modify upang gumawa ng bagong mga script.
- Gumagamit ang script ng boses na **Microsoft David** (US English) sa 160 WPM para sa malinaw na resulta ng transcription.
- Ang lahat ng mga senaryo ay tumutukoy sa mga produkto mula sa [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Paunawa**:  
Ang dokumentong ito ay isinalin gamit ang AI translation service na [Co-op Translator](https://github.com/Azure/co-op-translator). Bagama't nagsusumikap kami para sa katumpakan, pakitandaan na ang mga automated na pagsasalin ay maaaring maglaman ng mga pagkakamali o kamalian. Ang orihinal na dokumento sa orihinal nitong wika ang dapat ituring na opisyal na sanggunian. Para sa mahahalagang impormasyon, inirerekomenda ang propesyonal na pagsasalin ng tao. Hindi kami mananagot para sa anumang hindi pagkakaunawaan o maling interpretasyon na maaaring magmula sa paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->