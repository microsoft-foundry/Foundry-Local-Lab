# Sample Audio Files for Part 7 - Whisper Voice Transcription

These WAV files are generated using `pyttsx3` (Windows SAPI5 text-to-speech) and themed around **Zava DIY products** from the Creative Writer demo.

## Generate the samples

```bash
# repo ရှေ့ဆုံးနေရာမှ - pyttsx3 တပ်ဆင်ထားသော .venv လိုအပ်သည်
.venv\Scripts\Activate.ps1          # Windows
python samples/audio/generate_samples.py
```

## Sample files

| File | Scenario | Duration |
|------|----------|----------|
| `zava-customer-inquiry.wav` | Customer asking about the **Zava ProGrip Cordless Drill** - torque, battery life, carrying case | ~15 sec |
| `zava-product-review.wav` | Customer reviewing the **Zava UltraSmooth Interior Paint** - coverage, drying time, low VOC | ~22 sec |
| `zava-support-call.wav` | Support call about the **Zava TitanLock Tool Chest** - replacement keys, extra drawer liners | ~20 sec |
| `zava-project-planning.wav` | DIYer planning a backyard deck with **Zava EcoBoard Composite Decking** & BrightBeam lights | ~25 sec |
| `zava-workshop-setup.wav` | Walkthrough of a complete workshop using **all five Zava products** | ~32 sec |
| `zava-full-project-walkthrough.wav` | Extended garage renovation walkthrough using **all Zava products** (for long-audio testing, see [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 min |

## Notes

- WAV ဖိုင်များကို repository တွင် **ထည့်သွင်းထားပြီး** (. To create new .wav files run the script above to regenerate new scripts or modify to create new scripts.
- ဒီ script မှာ သော့သော့ အသံသွင်းခြင်းအတွက် **Microsoft David** (အမေရိကန် အင်္ဂလိပ်) အသံကို 160 WPM ဖြင့် အသုံးပြုထားသည်။
- scenarios တစ်ခုချင်းစီသည် [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) မှ ထုတ်ကုန်များနှင့် သက်ဆိုင်သည်။

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**သတိပေးချက်**:  
ဤစာတမ်းကို AI ဘာသာပြန်ဝန်ဆောင်မှု [Co-op Translator](https://github.com/Azure/co-op-translator) သုံးပြီး ဘာသာပြန်ထားသည်။ ကျနော်တို့မှန်ကန်မှုအတွက် ကြိုးပမ်းလျက်ရှိသော်လည်း အလိုအလျှောက်ဘာသာပြန်မှုတွင် အမှားများ သို့မဟုတ် မှားယွင်းချက်များ ဖြစ်ပေါ်နိုင်ပါသည်။ မူလစာတမ်းကို မိမိဘာသာဖြင့် အတည်ပြုအောင်ကြည့်ရှုသင့်ပါသည်။ အရေးကြီးသည့် အချက်အလက်များအတွက် လူမူကြိုက်သော လူ‌သမား ဘာသာပြန်ခြင်းကို သုံးရန် အကြံပြုပါသည်။ ဤဘာသာပြန်မှု မူရင်းကို အသုံးပြုခြင်းကြောင့် ဖြစ်ပေါ်သော နားလည်မှုအမှားများအတွက် ကျနော်တို့ တာဝန်မခံပါ။
<!-- CO-OP TRANSLATOR DISCLAIMER END -->