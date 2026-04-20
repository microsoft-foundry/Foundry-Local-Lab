# پارٹ 7 کے لیے نمونہ آڈیو فائلیں - وسپر وائس ٹرانسکرپشن

یہ WAV فائلیں `pyttsx3` (ونڈوز SAPI5 ٹیکسٹ ٹو اسپیچ) کا استعمال کرتے ہوئے بنائی گئی ہیں اور تخلیقی رائٹر ڈیمو کے **Zava DIY مصنوعات** کے موضوع پر مشتمل ہیں۔

## نمونے تیار کریں

```bash
# ریپو روٹ سے - .venv کی ضرورت ہے جس میں pyttsx3 انسٹال ہو
.venv\Scripts\Activate.ps1          # ونڈوز
python samples/audio/generate_samples.py
```

## نمونہ فائلیں

| فائل | منظرنامہ | دورانیہ |
|------|----------|----------|
| `zava-customer-inquiry.wav` | کسٹمر **Zava ProGrip Cordless Drill** کے بارے میں پوچھ رہا ہے - ٹؤرک، بیٹری کی زندگی، کیری کیس | ~15 سیکنڈ |
| `zava-product-review.wav` | کسٹمر **Zava UltraSmooth Interior Paint** کا جائزہ لے رہا ہے - کوریج، خشک ہونے کا وقت، کم VOC | ~22 سیکنڈ |
| `zava-support-call.wav` | سپورٹ کال **Zava TitanLock Tool Chest** کے بارے میں - متبادل چابیاں، اضافی دراز لائینرز | ~20 سیکنڈ |
| `zava-project-planning.wav` | DIY کرنے والا **Zava EcoBoard Composite Decking** اور BrightBeam لائٹس کے ساتھ بیک یارڈ ڈیک کی منصوبہ بندی کر رہا ہے | ~25 سیکنڈ |
| `zava-workshop-setup.wav` | مکمل ورکشاپ کا جائزہ جس میں **تمام پانچ Zava مصنوعات** استعمال ہو رہی ہیں | ~32 سیکنڈ |
| `zava-full-project-walkthrough.wav` | **تمام Zava مصنوعات** استعمال کرتے ہوئے وسیع گیراج کی تزئین و آرائش کا جائزہ (طویل آڈیو ٹیسٹنگ کے لیے، دیکھیں [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 منٹ |

## نوٹس

- WAV فائلیں ریپو میں **کمیٹ** کی گئی ہیں (فہرست `. میں موجود ہے۔ نئی .wav فائلز بنانے کے لیے اوپر دی گئی اسکرپٹ چلائیں یا نئی اسکرپٹس بنانے کے لیے ترمیم کریں۔
- اسکرپٹ **Microsoft David** (امریکی انگریزی) کی آواز کو 160 WPM پر واضح ٹرانسکرپشن کے لیے استعمال کرتا ہے۔
- تمام منظرنامے مصنوعات کا حوالہ دیتے ہیں جو [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) میں موجود ہیں۔