# Sample Audio Files for Part 7 - Whisper Voice Transcription

These WAV files dem generate wit `pyttsx3` (Windows SAPI5 text-to-speech) and tema around **Zava DIY products** from di Creative Writer demo.

## Generate the samples

```bash
# From di repo root - e need di .venv wey get pyttsx3 install
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Sample files

| File | Scenario | Duration |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Customer wey dey ask about di **Zava ProGrip Cordless Drill** - torque, battery life, carrying case | ~15 sec |
| `zava-product-review.wav` | Customer wey dey review di **Zava UltraSmooth Interior Paint** - coverage, drying time, low VOC | ~22 sec |
| `zava-support-call.wav` | Support call about di **Zava TitanLock Tool Chest** - replacement keys, extra drawer liners | ~20 sec |
| `zava-project-planning.wav` | DIYer wey dey plan backyard deck wit **Zava EcoBoard Composite Decking** & BrightBeam lights | ~25 sec |
| `zava-workshop-setup.wav` | Walkthrough of complete workshop wey use **all five Zava products** | ~32 sec |
| `zava-full-project-walkthrough.wav` | Extended garage renovation walkthrough wey use **all Zava products** (for long-audio testing, see [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Notes

- WAV files dem **commit** for di repo (dem list for `. To create new .wav files, run di script above to regenerate new scripts or modify am to create new scripts.
- Di script dey use **Microsoft David** (US English) voice at 160 WPM for clear transcription results.
- All di scenarios dey reference products from [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json).