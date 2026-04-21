# ਭਾਗ 7 ਲਈ ਨਮੂਨਾ ਆਡੀਓ ਫਾਈਲਾਂ - ਵਿਸਪਰ ਵੌਇਸ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ

ਇਹ WAV ਫਾਈਲਾਂ `pyttsx3` (Windows SAPI5 ਟੈਕਸਟ-ਟੂ-ਸਪੀਚ) ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਬਣਾਈਆਂ ਗਈਆਂ ਹਨ ਅਤੇ Creative Writer ਡੈਮੋ ਦੇ **Zava DIY ਉਤਪਾਦਾਂ** ਦੀ ਥੀਮ ਵਾਲੀਆਂ ਹਨ।

## ਨਮੂਨੇ ਤਿਆਰ ਕਰੋ

```bash
# ਰੈਪੋ ਰੂਟ ਤੋਂ - .venv ਦੀ ਲੋੜ ਹੈ ਜਿਸ ਵਿੱਚ pyttsx3 ਇੰਸਟਾਲ ਹੋਈ ਹੋਵੇ
.venv\Scripts\Activate.ps1          # ਵਿੰਡੋਜ਼
python samples/audio/generate_samples.py
```

## ਨਮੂਨਾ ਫਾਈਲਾਂ

| ਫਾਈਲ | ਸਿਨਾਰੀਓ | ਸਮਾਂ ਅਵਧੀ |
|-------|----------|------------|
| `zava-customer-inquiry.wav` | ਗਾਹਕ **Zava ProGrip Cordless Drill** - ਟਾਰਕ, ਬੈਟਰੀ ਦੀ ਆਯੁ, ਕੈਰੀਇੰਗ ਕੇਸ ਬਾਰੇ ਪੁੱਛ ਰਿਹਾ ਹੈ | ~15 ਸਕਿੰਟ |
| `zava-product-review.wav` | ਗਾਹਕ **Zava UltraSmooth Interior Paint** - ਕਵਰੇਜ, ਸੁੱਕਣ ਦਾ ਸਮਾਂ, ਘੱਟ VOC ਦੀ ਸਮੀਖਿਆ ਕਰ ਰਿਹਾ ਹੈ | ~22 ਸਕਿੰਟ |
| `zava-support-call.wav` | ਸਹਾਇਤਾ ਕਾਲ **Zava TitanLock Tool Chest** - ਬਦਲੀ ਵਾਲੀਆਂ ਕੁੰਜੀਆਂ, ਵਾਧੂ ਡਰਾਅਰ ਲਾਈਨਰ ਲਈ | ~20 ਸਕਿੰਟ |
| `zava-project-planning.wav` | ਡੀ ਆਈ ਵਾਈ ਵਿਅਕਤੀ ਪਿੱਛੇ ਆੰਗਣੀ ਡੈਕ ਪਲਾਨ ਕਰ ਰਿਹਾ ਹੈ ਜਿਸ ਵਿੱਚ **Zava EcoBoard Composite Decking** ਅਤੇ BrightBeam ਲਾਈਟਸ ਹਨ | ~25 ਸਕਿੰਟ |
| `zava-workshop-setup.wav` | ਪੂਰੇ ਵਰਕਸ਼ਾਪ ਦਾ ਵਾਕਥਰੂ ਜਿਸ ਵਿੱਚ **ਸਾਰੇ ਪੰਜ Zava ਉਤਪਾਦ** ਵਰਤੇ ਗਏ ਹਨ | ~32 ਸਕਿੰਟ |
| `zava-full-project-walkthrough.wav` | ਵੱਡੇ ਆਡੀਓ ਟੈਸਟ ਲਈ ਲੰਮਾ ਗੈਰੇਜ ਨਵੀਨੀਕਰਨ ਵਾਕਥਰੂ ਜਿਸ ਵਿੱਚ **ਸਾਰੇ Zava ਉਤਪਾਦ** ਸ਼ਾਮਲ ਹਨ (ਵੇਖੋ [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 ਮਿੰਟ |

## ਨੋਟਸ

- WAV ਫਾਈਲਾਂ ਰਿਪੋ ਵਿੱਚ **ਕਮਿੱਟ ਕੀਤੀਆਂ** ਗਈਆਂ ਹਨ (ਸੂਚੀ `. ਵਿੱਚ ਦਿੱਤੀ ਗਈ ਹੈ। ਨਵੀਆਂ .wav ਫਾਈਲਾਂ ਬਣਾਉਣ ਲਈ ਉੱਪਰ ਦਿੱਤਾ ਸਕ੍ਰਿਪਟ ਚਲਾਓ ਜਾਂ ਨਵੇਂ ਸਕ੍ਰਿਪਟ ਬਣਾਉਣ ਲਈ ਸੋਧ ਕਰੋ।
- ਸਕ੍ਰਿਪਟ ਸਾਫ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਨਤੀਜੇ ਲਈ 160 WPM ਤੇ **Microsoft David** (US English) ਦੀ ਆਵਾਜ਼ ਦੀ ਵਰਤੋਂ ਕਰਦਾ ਹੈ।
- ਸਾਰੇ ਸਿਨਾਰੀਓਜ਼ [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) ਵਿੱਚ ਦਿੱਤੇ ਉਤਪਾਦਾਂ ਨੂੰ ਨੂੰ ਸੰਬੋਧਿਤ ਕਰਦੇ ਹਨ।

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ਇਸਤਿਆਰ**:  
ਇਹ ਦਸਤਾਵੇਜ਼ AI ਅਨੁਵਾਦ ਸੇਵਾ [Co-op Translator](https://github.com/Azure/co-op-translator) ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਅਨੁਵਾਦ ਕੀਤਾ ਗਿਆ ਹੈ। ਜਦੋਂ ਕਿ ਅਸੀਂ ਸਹੀਤਾ ਲਈ ਯਤਨ ਕਰਦੇ ਹਾਂ, ਕਿਰਪਾ ਕਰਕੇ ਧਿਆਨ ਵਿੱਚ ਰੱਖੋ ਕਿ ਸਵਯੰਚਾਲਿਤ ਅਨੁਵਾਦਾਂ ਵਿੱਚ ਗਲਤੀਆਂ ਜਾਂ ਅਣਸਹੀਤੀਆਂ ਹੋ ਸਕਦੀਆਂ ਹਨ। ਮੂਲ ਦਸਤਾਵੇਜ਼ ਆਪਣੀ ਕੂੜੀ ਭਾਸ਼ਾ ਵਿੱਚ ਹੀ ਮੂਲ ਸਰੋਤ ਮੰਨਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ। ਮਹੱਤਵਪੂਰਨ ਜਾਣਕਾਰੀ ਲਈ, ਪੇਸ਼ੇਵਰ ਮਨੁੱਖੀ ਅਨੁਵਾਦ ਦੀ ਸਿਫਾਰਸ਼ ਕੀਤੀ ਜਾਂਦੀ ਹੈ। ਅਸੀਂ ਇਸ ਅਨੁਵਾਦ ਦੀ ਵਰਤੋਂ ਨਾਲ ਪੈਦਾ ਹੋਣ ਵਾਲੀਆਂ ਕਿਸੇ ਵੀ ਗਲਤਫਹਿਮੀਆਂ ਜਾਂ ਗਲਤ ਵਿਆਖਿਆਵਾਂ ਲਈ ਜ਼ਿੰਮੇਵਾਰ ਨਹੀਂ ਹਾਂ।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->