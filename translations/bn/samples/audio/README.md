# পার্ট ৭-এর জন্য নমুনা অডিও ফাইল - হুইস্পার ভয়েস ট্রান্সক্রিপশন

এই WAV ফাইলগুলি `pyttsx3` (Windows SAPI5 টেক্সট-টু-স্পিচ) ব্যবহার করে তৈরি করা হয়েছে এবং Creative Writer ডেমোর **Zava DIY পণ্য** সংক্রান্ত।

## নমুনাগুলো তৈরি করুন

```bash
# রিপো রুট থেকে - .venv দরকার pyttsx3 ইন্সটল করা
.venv\Scripts\Activate.ps1          # উইন্ডোজ
python samples/audio/generate_samples.py
```

## নমুনা ফাইলসমূহ

| ফাইল | দৃশ্যপট | সময়কাল |
|------|----------|----------|
| `zava-customer-inquiry.wav` | গ্রাহক **Zava ProGrip Cordless Drill** সম্পর্কে জিজ্ঞাসা করছেন - টর্ক, ব্যাটারি লাইফ, ক্যারিং কেস | ~১৫ সেকেন্ড |
| `zava-product-review.wav` | গ্রাহক **Zava UltraSmooth Interior Paint** পর্যালোচনা করছেন - কভারেজ, শুকানোর সময়, কম VOC | ~২২ সেকেন্ড |
| `zava-support-call.wav` | সাপোর্ট কল **Zava TitanLock Tool Chest** সম্পর্কে - রিপ্লেসমেন্ট চাবি, অতিরিক্ত ড্রয়ার লাইনার | ~২০ সেকেন্ড |
| `zava-project-planning.wav` | DIY ব্যবহারকারী ফরসা দানা প্ল্যান করছেন Backyard Deck এর জন্য **Zava EcoBoard Composite Decking** & BrightBeam লাইটস | ~২৫ সেকেন্ড |
| `zava-workshop-setup.wav` | সম্পূর্ণ ওয়ার্কশপের ওয়াকথ্রু, ব্যবহার করে **সব পাঁচটি Zava পণ্য** | ~৩২ সেকেন্ড |
| `zava-full-project-walkthrough.wav` | দীর্ঘকালীন অডিও পরীক্ষার জন্য **সব Zava পণ্য** ব্যবহার করে গ্যারেজ রেনোভেশনের বিস্তৃত ওয়াকথ্রু ([Foundry-Local#517](https://github.com/microsoft/Foundry-Local/issues/517) দেখুন) | ~৪ মিনিট |

## নোটসমূহ

- WAV ফাইলগুলো রিপোজিটরিতে **কমিট করা হয়েছে** (তালিকাভুক্ত `. নতুন .wav ফাইল তৈরির জন্য উপরের স্ক্রিপ্ট চালিয়ে নতুন স্ক্রিপ্ট তৈরি করা বা পরিবর্তন করুন।
- স্ক্রিপ্টে **Microsoft David** (US English) ভয়েস ১৬০ WPM এ ব্যবহার করা হয় সুস্পষ্ট ট্রান্সক্রিপশনের জন্য।
- সকল দৃশ্যপটের রেফারেন্স রয়েছে [`zava-creative-writer-local/src/api/agents/writer/products.json`](../../../../zava-creative-writer-local/src/api/agents/writer/products.json) থেকে নেওয়া পণ্যের।