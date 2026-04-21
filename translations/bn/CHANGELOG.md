# পরিবর্তন লগ — ফাউন্ড্রি লোকাল ওয়ার্কশপ

এই ওয়ার্কশপে সকল গুরুত্বপূর্ণ পরিবর্তন নিচে নিবন্ধীকৃত হয়েছে।

---

## ২০২৬-০৩-১১ — অংশ ১২ ও ১৩, ওয়েব UI, উইস্পার পুনর্লিখন, WinML/QNN সমাধান, এবং যাচাই

### যোগ করা হয়েছে
- **অংশ ১২: জাভা ক্রিয়েটিভ রাইটারের জন্য ওয়েব UI তৈরি** — নতুন ল্যাব নির্দেশিকা (`labs/part12-zava-ui.md`) যেখানে স্ট্রিমিং NDJSON, ব্রাউজার `ReadableStream`, লাইভ এজেন্ট স্ট্যাটাস ব্যাজ, এবং রিয়েল-টাইম আর্টিকেল টেক্সট স্ট্রিমিং বিষয়ক অনুশীলন রয়েছে
- **অংশ ১৩: ওয়ার্কশপ সম্পূর্ণ** — নতুন সারাংশ ল্যাব (`labs/part13-workshop-complete.md`) যেখানে ১২টি অংশের পুনরাবৃত্তি, আরও ধারণা এবং রিসোর্স লিঙ্ক প্রদান করা হয়েছে
- **জাভা UI ফ্রন্টএন্ড:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — সব তিনটি ব্যাকএন্ড দ্বারা ব্যবহৃত শেয়ার্ড ভ্যানিলা HTML/CSS/JS ব্রাউজার ইন্টারফেস
- **জাভাস্ক্রিপ্ট HTTP সার্ভার:** `zava-creative-writer-local/src/javascript/server.mjs` — ব্রাউজার-ভিত্তিক এক্সেসের জন্য অর্কেস্ট্রেটরকে র‍্যাপ করে নতুন এক্সপ্রেস-স্টাইল HTTP সার্ভার
- **C# ASP.NET Core ব্যাকএন্ড:** `zava-creative-writer-local/src/csharp-web/Program.cs` এবং `ZavaCreativeWriterWeb.csproj` — UI এবং স্ট্রিমিং NDJSON সরবরাহের জন্য নতুন মিনি মাল API প্রকল্প
- **অডিও স্যাম্পল জেনারেটর:** `samples/audio/generate_samples.py` — অফলাইন TTS স্ক্রিপ্ট যা Part 9 এর জন্য জাভা-থিমযুক্ত WAV ফাইল তৈরি করে `pyttsx3` ব্যবহার করে
- **অডিও স্যাম্পল:** `samples/audio/zava-full-project-walkthrough.wav` — ট্রান্সক্রিপশন পরীক্ষার জন্য নতুন দীর্ঘ অডিও স্যাম্পল
- **যাচাই স্ক্রিপ্ট:** `validate-npu-workaround.ps1` — সব C# নমুনার জন্য NPU/QNN ওয়ার্কঅ্যারাউন্ড যাচাই করার জন্য স্বয়ংক্রিয় PowerShell স্ক্রিপ্ট
- **মারমেড ডায়াগ্রাম SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML ক্রস-প্ল্যাটফর্ম সাপোর্ট:** সব ৩টি C# `.csproj` ফাইল (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) এখন ক্রস-প্ল্যাটফর্ম সাপোর্টের জন্য শর্তাধীন TFM এবং পরস্পর-বহিষ্কৃত প্যাকেজ রেফারেন্স ব্যবহার করে। উইন্ডোজে: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (QNN EP প্লাগইনসহ সুপারসেট)। নন-উইন্ডোজে: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (বেস SDK)। জাভা প্রকল্পগুলোর হার্ডকোডেড `win-arm64` RID অটো-ডিটেক্ট করে প্রতিস্থাপিত হয়েছে। একটি ট্রানজিটিভ ডিপেন্ডেন্সি ওয়ার্কঅ্যারাউন্ড `Microsoft.ML.OnnxRuntime.Gpu.Linux` এর নেটিভ অ্যাসেটগুলো বাদ দিয়েছে যার ভাঙা win-arm64 রেফারেন্স আছে। পূর্বের ট্রাই/ক্যাচ NPU ওয়ার্কঅ্যারাউন্ড সমস্ত ৭টি C# ফাইল থেকে সরানো হয়েছে।

### পরিবর্তনসমূহ
- **অংশ ৯ (উইস্পার):** প্রধান পুনর্লিখন — জাভাস্ক্রিপ্ট এখন SDK-এর বিল্ট-ইন `AudioClient` (`model.createAudioClient()`) ব্যবহার করে ম্যানুয়াল ONNX Runtime inference এর বদলে; JS/C# `AudioClient` পদ্ধতি বনাম পাইথন ONNX Runtime পদ্ধতির সাপেক্ষে আর্কিটেকচার বর্ণনা, তুলনা টেবিল, এবং পাইপলাইন ডায়াগ্রাম আপডেট হয়েছে
- **অংশ ১১:** নেভিগেশন লিংক আপডেট (এখন অংশ ১২-এ নির্দেশ করে); টুল-ক্যালিং ফ্লো এবং সিকোয়েন্সের রেন্ডারড SVG ডায়াগ্রাম যোগ করা হয়েছে
- **অংশ ১০:** নেভিগেশন আপডেট, ওয়ার্কশপ শেষের পরিবর্তে অংশ ১২-এ রুটিং
- **পাইথন উইস্পার (`foundry-local-whisper.py`):** অতিরিক্ত অডিও স্যাম্পল এবং উন্নত ত্রুটি হ্যান্ডলিং সহ সম্প্রসারিত
- **জাভাস্ক্রিপ্ট উইস্পার (`foundry-local-whisper.mjs`):** ম্যানুয়াল ONNX Runtime সেশনগুলোর বদলে সদর্থক `model.createAudioClient()` এবং `audioClient.transcribe()` ব্যবহার করে পুনর্লিখিত
- **পাইথন ফাস্টAPI (`zava-creative-writer-local/src/api/main.py`):** স্ট্যাটিক UI ফাইল API-এর পাশাপাশি পরিবেশন করার জন্য আপডেট
- **জাভা C# কনসোল (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU ওয়ার্কঅ্যারাউন্ড সরানো (এখন WinML প্যাকেজ দ্বারা পরিচালিত)
- **README.md:** অংশ ১২-এর কোড স্যাম্পল টেবিল এবং ব্যাকএন্ড সংযোজন সহ বিভাগ যোগ; অংশ ১৩-এর অংশ যোগ; শেখার লক্ষ্য এবং প্রকল্প কাঠামো আপডেট
- **KNOWN-ISSUES.md:** নিষ্পত্তিকৃত Issue #7 (C# SDK NPU মডেল ভ্যারিয়েন্ট – এখন WinML প্যাকেজ দ্বারা পরিচালিত) সরানো হয়েছে; বাকি ইস্যুগুলো পুনর্মূল্যায়ন করে #1–#6 নম্বরকরণ করা হয়েছে; .NET SDK 10.0.104 পরিবেশ বিবরণ আপডেট
- **AGENTS.md:** `zava-creative-writer-local` নতুন এন্ট্রিসহ প্রকল্প কাঠামো গাছ আপডেট (`ui/`, `csharp-web/`, `server.mjs`); C# প্রধান প্যাকেজ এবং শর্তাধীন TFM বিবরণ আপডেট
- **labs/part2-foundry-local-sdk.md:** `.csproj` উদাহরণ আপডেট, সম্পূর্ণ ক্রস-প্ল্যাটফর্ম প্যাটার্নসহ শর্তাধীন TFM, পরস্পর-বহিষ্কৃত প্যাকেজ রেফারেন্স এবং ব্যাখ্যামূলক টীকা দেখানো হয়েছে

### যাচাই করা হয়েছে
- সব ৩টি C# প্রকল্প (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) সফলভাবে উইন্ডোজ ARM64 এ বিল্ড হয়
- চ্যাট স্যাম্পল (`dotnet run chat`): মডেল WinML/QNN এর মাধ্যমে `phi-3.5-mini-instruct-qnn-npu:1` হিসেবে লোড হয় — CPU fallback ছাড়াই সরাসরি NPU ভ্যারিয়েন্ট লোড হয়
- এজেন্ট স্যাম্পল (`dotnet run agent`): মাল্টি-টার্ন কথোপকথন সহ সম্পূর্ণ চালিত হয়, exit code 0
- Foundry Local CLI v0.8.117 এবং SDK v0.9.0 .NET SDK 9.0.312 তে

---

## ২০২৬-০৩-১১ — কোড ফিক্স, মডেল পরিস্কার, মারমেড ডায়াগ্রাম, এবং যাচাই

### মেরামত করা হয়েছে
- **সব ২১ কোড স্যাম্পল (৭ পাইথন, ৭ জাভাস্ক্রিপ্ট, ৭ C#):** প্রস্থানকালে `model.unload()` / `unload_model()` / `model.UnloadAsync()` যোগ করে OGA মেমোরি লিক সতর্কতা দূর করা হয়েছে (জানা সমস্যা #4)
- **csharp/WhisperTranscription.cs:** ভঙ্গুর `AppContext.BaseDirectory` আপেক্ষিক পাথ বাদ দিয়ে `FindSamplesDirectory()` যা ডিরেক্টরিগুলো উর্ধ্বমুখী অনুসন্ধান করে `samples/audio` নির্ভরযোগ্যভাবে অবস্থান করে প্রতিস্থাপিত হয়েছে (জানা সমস্যা #7)
- **csharp/csharp.csproj:** হার্ডকোডেড `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` অটো-ডিটেক্ট নিরাপদ বিকল্প `$(NETCoreSdkRuntimeIdentifier)` দ্বারা প্রতিস্থাপিত হয়েছে যাতে কোনো প্ল্যাটফর্মেই `dotnet run` চলতে পারে `-r` ফ্ল্যাগ ছাড়াই ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### পরিবর্তনসমূহ
- **অংশ ৮:** অ্যাস্কি বক্স ডায়াগ্রাম থেকে রেন্ডারড SVG চিত্রে ইভ্যালুয়েশন চালানো পুনরাবৃত্তি লুপ রূপান্তরিত হয়েছে
- **অংশ ১০:** অ্যাস্কি তীর থেকে রেন্ডারড SVG চিত্রে কম্পাইলেশন পাইপলাইন ডায়াগ্রাম রূপান্তরিত হয়েছে
- **অংশ ১১:** টুল-ক্যালিং ফ্লো এবং সিকোয়েন্স ডায়াগ্রাম রেন্ডারড SVG চিত্রে রূপান্তরিত হয়েছে
- **অংশ ১০:** "ওয়ার্কশপ সম্পূর্ণ!" বিভাগ অংশ ১১-এ সরানো হয়েছে (অবশেষ ল্যাব); পরিবর্তে "পরবর্তী ধাপ" লিংক যোগ
- **KNOWN-ISSUES.md:** CLI v0.8.117 এর বিরুদ্ধে সব সমস্যা পূর্ণ পুনঃপরীক্ষণ; নিষ্পত্তিকৃত: OGA মেমোরি লিক (পরিষ্কার যুক্ত), উইস্পার পাথ (FindSamplesDirectory), HTTP 500 স্থায়ী ইনফারেন্স (পুনরুত্পাদনযোগ্য নয়, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice সীমাবদ্ধতা (এখন `"required"` এবং বিশেষ ফাংশন টার্গেট qwen2.5-0.5b এ কাজ করে)। JS উইস্পার ইস্যু আপডেট — এখন সব ফাইল ফাঁকা/বাইনারি আউটপুট রিটার্ন করে (v0.9.x থেকে রিগ্রেশন, গুরুতরতা বৃদ্ধি করা হয়েছে)। #4 C# RID অটো-ডিটেক্ট ওয়ার্কঅ্যারাউন্ড এবং [#497](https://github.com/microsoft/Foundry-Local/issues/497) লিঙ্ক আপডেট। ৭টি খোলা সমস্যা অবশিষ্ট।
- **javascript/foundry-local-whisper.mjs:** ক্লিনআপ ভেরিয়েবল নাম সংশোধন (`whisperModel` → `model`)

### যাচাই করা হয়েছে
- পাইথন: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` সফলভাবে চালানো হয়েছে ক্লিনআপ সহ
- জাভাস্ক্রিপ্ট: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` সফলভাবে চালানো হয়েছে ক্লিনআপ সহ
- C#: `dotnet build` ০ ওয়্যার্নিং, ০ এরর সহ সফল (net9.0 লক্ষ্য)
- সব ৭টি পাইথন ফাইল `py_compile` সিনট্যাক্স চেক পাশ করেছে
- সব ৭টি জাভাস্ক্রিপ্ট ফাইল `node --check` সিনট্যাক্স যাচাই পাশ করেছে

---

## ২০২৬-০৩-১০ — অংশ ১১: টুল ক্যালিং, SDK API সম্প্রসারণ, এবং মডেল কভারেজ

### যোগ করা হয়েছে
- **অংশ ১১: লোকাল মডেলসের সাথে টুল ক্যালিং** — নতুন ল্যাব নির্দেশিকা (`labs/part11-tool-calling.md`) ৮টি অনুশীলন নিয়ে টুল স্কিমা, মাল্টি-টার্ন ফ্লো, একাধিক টুল কল, কাস্টম টুল, ChatClient টুল ক্যালিং, এবং `tool_choice`
- **পাইথন স্যাম্পল:** `python/foundry-local-tool-calling.py` — OpenAI SDK ব্যবহার করে `get_weather`/`get_population` টুলগুলোর কলিং
- **জাভাস্ক্রিপ্ট স্যাম্পল:** `javascript/foundry-local-tool-calling.mjs` — SDK-র নেটিভ `ChatClient` (`model.createChatClient()`) ব্যবহার করে টুল ক্যালিং
- **C# স্যাম্পল:** `csharp/ToolCalling.cs` — OpenAI C# SDK ব্যবহার করে `ChatTool.CreateFunctionTool()` মাধ্যমে টুল ক্যালিং
- **অংশ ২, অনুশীলন ৭:** নেটিভ `ChatClient` — জাভাস্ক্রিপ্টে `model.createChatClient()`, C#-এ `model.GetChatClientAsync()` ওপেনAI SDK এর বিকল্প হিসেবে
- **অংশ ২, অনুশীলন ৮:** মডেল ভ্যারিয়েন্ট এবং হার্ডওয়্যার বাছাই — `selectVariant()`, `variants`, NPU ভ্যারিয়েন্ট টেবিল (৭টি মডেল)
- **অংশ ২, অনুশীলন ৯:** মডেল উন্নয়ন এবং ক্যাটালগ রিফ্রেশ — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **অংশ ২, অনুশীলন ১০:** রিজনিং মডেলস — `<think>` ট্যাগ বিশ্লেষণের সাথে `phi-4-mini-reasoning`
- **অংশ ৩, অনুশীলন ৪:** OpenAI SDK-র বিকল্প হিসেবে `createChatClient`, স্ট্রিমিং কলব্যাক প্যাটার্ন ডকুমেন্টেশনসহ
- **AGENTS.md:** টুল ক্যালিং, ChatClient, এবং রিজনিং মডেলসের কোডিং কনভেনশন যোগ করা হয়েছে

### পরিবর্তনসমূহ
- **অংশ ১:** মডেল ক্যাটালগ সম্প্রসারিত — phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo যোগ করা হয়েছে
- **অংশ ২:** API রেফারেন্স টেবিল সম্প্রসারিত — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync` যোগ করা হয়েছে
- **অংশ ২:** অনুশীলন ৭-৯ পুনঃনম্বরকরণ ১০-১৩ এ নতুন অনুশীলনসমুহের জন্য
- **অংশ ৩:** প্রধান শিক্ষা বিষয়ক টেবিলে নেটিভ ChatClient অন্তর্ভুক্ত করা হয়েছে
- **README.md:** অংশ ১১ কোড স্যাম্পল টেবিল, শেখার লক্ষ্য #11, এবং প্রকল্প কাঠামোর গাছ আপডেট করা হয়েছে
- **csharp/Program.cs:** CLI রাউটারে `toolcall` কেস যোগ এবং সাহায্য বার্তা আপডেট

---

## ২০২৬-০৩-০৯ — SDK v0.9.0 আপডেট, ব্রিটিশ ইংরেজি, এবং যাচাই

### পরিবর্তনসমূহ
- **সব কোড স্যাম্পল (পাইথন, জাভাস্ক্রিপ্ট, C#):** Foundry Local SDK v0.9.0 API-তে আপডেট — `await catalog.getModel()` (আগে `await` ছিল না) সংশোধন, `FoundryLocalManager` ইনিশিয়ালাইজেশন প্যাটার্ন আপডেট, এন্ডপয়েন্ট ডিসকভারি ঠিক করা হয়েছে
- **সব ল্যাব গাইড (অংশ ১-১০):** ব্রিটিশ ইংরেজিতে রূপান্তর (colour, catalogue, optimised ইত্যাদি)
- **সব ল্যাব গাইড:** SDK কোড উদাহরণ v0.9.0 API মেলানো হয়েছে
- **সব ল্যাব গাইড:** API রেফারেন্স টেবিল এবং অনুশীলন কোড ব্লক আপডেট
- **জাভাস্ক্রিপ্ট গুরুত্বপূর্ণ মেরামত:** `catalog.getModel()` এ একখানা মিসিং `await` যোগ করা হয়েছে — `Promise` রিটার্ন করছিল কিন্তু `Model` অবজেক্ট ছিল না, ফলে নীচের ধাপে নীরব ব্যর্থতা হচ্ছিল

### যাচাই করা হয়েছে
- সব পাইথন স্যাম্পল Foundry Local সার্ভিসে সফলভাবে চালানো হয়েছে
- সব জাভাস্ক্রিপ্ট স্যাম্পল সফলভাবে চালানো হয়েছে (Node.js 18+)
- C# প্রকল্প .NET 9.0-এ বিল্ড এবং চালানো হয়েছে (net8.0 SDK থেকে ফরওয়ার্ড-কম্প্যাট)
- ওয়ার্কশপ জুড়ে মোট ২৯ ফাইল পরিবর্তন ও যাচাই

---

## ফাইল সূচি

| ফাইল | সর্বশেষ আপডেট | বর্ণনা |
|------|---------------|---------|
| `labs/part1-getting-started.md` | ২০২৬-০৩-১০ | মডেল ক্যাটালগ সম্প্রসারিত |
| `labs/part2-foundry-local-sdk.md` | ২০২৬-০৩-১০ | নতুন অনুশীলন ৭-১০, API টেবিল সম্প্রসারিত |
| `labs/part3-sdk-and-apis.md` | ২০২৬-০৩-১০ | নতুন অনুশীলন ৪ (ChatClient), শিক্ষা বিষয়ক টেবিল আপডেট |
| `labs/part4-rag-fundamentals.md` | ২০২৬-০৩-০৯ | SDK v0.9.0 + ব্রিটিশ ইংরেজি |
| `labs/part5-single-agents.md` | ২০২৬-০৩-০৯ | SDK v0.9.0 + ব্রিটিশ ইংরেজি |
| `labs/part6-multi-agent-workflows.md` | ২০২৬-০৩-০৯ | SDK v0.9.0 + British English |
| `labs/part7-zava-creative-writer.md` | ২০২৬-০৩-০৯ | SDK v0.9.0 + British English |
| `labs/part8-evaluation-led-development.md` | ২০২৬-০৩-১১ | Mermaid diagram |
| `labs/part9-whisper-voice-transcription.md` | ২০২৬-০৩-০৯ | SDK v0.9.0 + British English |
| `labs/part10-custom-models.md` | ২০২৬-০৩-১১ | Mermaid diagram, Workshop Complete কে Part 11 এ স্থানান্তরিত হয়েছে |
| `labs/part11-tool-calling.md` | ২০২৬-০৩-১১ | নতুন ল্যাব, Mermaid diagrams, Workshop Complete section |
| `python/foundry-local-tool-calling.py` | ২০২৬-০৩-১০ | নতুন: tool calling নমুনা |
| `javascript/foundry-local-tool-calling.mjs` | ২০২৬-০৩-১০ | নতুন: tool calling নমুনা |
| `csharp/ToolCalling.cs` | ২০২৬-০৩-১০ | নতুন: tool calling নমুনা |
| `csharp/Program.cs` | ২০২৬-০৩-১০ | `toolcall` CLI কমান্ড যোগ করা হয়েছে |
| `README.md` | ২০২৬-০৩-১০ | Part 11, প্রকল্পের কাঠামো |
| `AGENTS.md` | ২০২৬-০৩-১০ | Tool calling + ChatClient কনভেনশন |
| `KNOWN-ISSUES.md` | ২০২৬-০৩-১১ | Issue #7 মিমাংসা করেছে, ৬টি খোলা সমস্যা রয়ে গেছে |
| `csharp/csharp.csproj` | ২০২৬-০৩-১১ | Cross-platform TFM, WinML/base SDK শর্তাধীন রেফারেন্স |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | ২০২৬-০৩-১১ | Cross-platform TFM, অটো-ডিটেক্ট RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | ২০২৬-০৩-১১ | Cross-platform TFM, অটো-ডিটেক্ট RID |
| `csharp/BasicChat.cs` | ২০২৬-০৩-১১ | NPU try/catch ওয়ার্কঅ্যারাউন্ড অপসারণ করা হয়েছে |
| `csharp/SingleAgent.cs` | ২০২৬-০৩-১১ | NPU try/catch ওয়ার্কঅ্যারাউন্ড অপসারণ করা হয়েছে |
| `csharp/MultiAgent.cs` | ২০২৬-০৩-১১ | NPU try/catch ওয়ার্কঅ্যারাউন্ড অপসারণ করা হয়েছে |
| `csharp/RagPipeline.cs` | ২০২৬-০৩-১১ | NPU try/catch ওয়ার্কঅ্যারাউন্ড অপসারণ করা হয়েছে |
| `csharp/AgentEvaluation.cs` | ২০২৬-০৩-১১ | NPU try/catch ওয়ার্কঅ্যারাউন্ড অপসারণ করা হয়েছে |
| `labs/part2-foundry-local-sdk.md` | ২০২৬-০৩-১১ | Cross-platform .csproj উদাহরণ |
| `AGENTS.md` | ২০২৬-০৩-১১ | C# প্যাকেজ এবং TFM বিস্তারিত আপডেট করা হয়েছে |
| `CHANGELOG.md` | ২০২৬-০৩-১১ | এই ফাইল |