# ឧទាហរណ៍ឯកសាសម្លេងសម្រាប់ផ្នែកទី 7 - ការបម្លែងសំលេងឱ្យមានអត្ថបទ Whisper

ឯកសារ WAV នេះត្រូវបានផលិតដោយប្រើ `pyttsx3` (Windows SAPI5 ពាក្យទៅសំលេង) ហើយមានប្រធានបទជុំវិញផលិតផល **Zava DIY** ពីការបង្ហាញ Creative Writer។

## បង្កើតឧទាហរណ៍

```bash
# ពីឫសរបស់រ៉េប៊ី - តម្រូវឲ្យមាន .venv ដែលបានដំឡើង pyttsx3
.venv\Scripts\Activate.ps1          # វីនដូស
python samples/audio/generate_samples.py
```

## ឯកសារឧទាហរណ៍

| ឯកសារ | សេណារីយ៉ូ | រយៈពេល |
|------|----------|----------|
| `zava-customer-inquiry.wav` | អតិថិជនសួរអំពី **Zava ProGrip Cordless Drill** - កម្លាំងបង្វិល, អាយុកាលថ្ម, ខ្ទង់​ធ្វើ​ដំណើរ | ~15 វិនាទី |
| `zava-product-review.wav` | អតិថិជនបញ្ចេញមតិអំពី **Zava UltraSmooth Interior Paint** - ការប្រមូលផ្តុំ, ពេលស្ងួត, VOC ទាប | ~22 វិនាទី |
| `zava-support-call.wav` | ការហៅគាំទ្រអំពី **Zava TitanLock Tool Chest** - សោជំនួស, ស្រទាប់ទ្វារ​បន្ថែម | ~20 វិនាទី |
| `zava-project-planning.wav` | អ្នក DIY កំពុងរៀបចំគម្រោងបាតដែកក្រោយផ្ទះជាមួយ **Zava EcoBoard Composite Decking** និងភ្លើង BrightBeam | ~25 វិនាទី |
| `zava-workshop-setup.wav` | ដំណើរកម្សាន្តក្នុងកម្ម្រាំងធ្វើការ ពេញលេញដោយប្រើ **ផលិតផល Zava ទាំងប្រាំ** | ~32 វិនាទី |
| `zava-full-project-walkthrough.wav` | ដំណើរកម្សាន្តវាហ្គារ៉ាសផ្លាស់ប្តូរពេញលេញដោយប្រើ **ផលិតផល Zava ទាំងអស់** (សម្រាប់សាកល្បងសម្លេងវែង ទស្សនា [Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517)) | ~4 នាទី |

## កំណត់សម្គាល់

- ឯកសារ WAV ត្រូវបាន **ដាក់ក្នុង repository** (រាយក្នុង `. ដើម្បីបង្កើតឯកសារ .wav ថ្មី ចាប់ផ្តើមការរត់ script ខាងលើដើម្បីបង្កើត script ថ្មី ឬកែប្រែដើម្បីបង្កើត script ថ្មី។
- script ប្រើសម្លេង **Microsoft David** (ភាសាអង់គ្លេសសហរដ្ឋអាមេរិក) នៅ 160 WPM សម្រាប់លទ្ធផលការបម្លែងច្បាស់។
- សេណារីយ៉ូទាំងអស់យោងទៅផលិតផលពី [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json)។