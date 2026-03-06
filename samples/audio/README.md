# Sample Audio Files for Part 7 — Whisper Voice Transcription

These WAV files are generated using `pyttsx3` (Windows SAPI5 text-to-speech) and themed around **Zava DIY products** from the Creative Writer demo.

## Generate the samples

```bash
# From the repo root — requires the .venv with pyttsx3 installed
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Sample files

| File | Scenario | Duration |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Customer asking about the **Zava ProGrip Cordless Drill** — torque, battery life, carrying case | ~15 sec |
| `zava-product-review.wav` | Customer reviewing the **Zava UltraSmooth Interior Paint** — coverage, drying time, low VOC | ~22 sec |
| `zava-support-call.wav` | Support call about the **Zava TitanLock Tool Chest** — replacement keys, extra drawer liners | ~20 sec |
| `zava-project-planning.wav` | DIYer planning a backyard deck with **Zava EcoBoard Composite Decking** & BrightBeam lights | ~25 sec |
| `zava-workshop-setup.wav` | Walkthrough of a complete workshop using **all five Zava products** | ~32 sec |

## Notes

- WAV files are **not committed** to the repo (listed in `.gitignore`). Run the script above to regenerate them.
- The script uses **Microsoft David** (US English) voice at 160 WPM for clear transcription results.
- All scenarios reference products from [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../zava-creative-writer-local/src/api/agents/writer/products.json).
