<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local কর্মশালা - ডিভাইস-এআই অ্যাপ তৈরি করুন

নিজের মেশিনে ভাষা মডেল চালানোর জন্য এবং [Foundry Local](https://foundrylocal.ai) এবং [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) এর সাথে বুদ্ধিমান অ্যাপ্লিকেশন তৈরি করার জন্য একটি হাতেকলমে কর্মশালা।

> **Foundry Local কি?** Foundry Local একটি হালকা ওজনের রানটাইম যা আপনাকে আপনার হার্ডওয়্যারে সম্পূর্ণ ভাষা মডেল ডাউনলোড, পরিচালনা এবং সার্ভ করার সুযোগ দেয়। এটি একটি **OpenAI-সমর্থিত API** প্রকাশ করে যাতে OpenAI কথা বলার যেকোনো টুল বা SDK সংযোগ করতে পারে - কোন ক্লাউড অ্যাকাউন্টের প্রয়োজন নেই।

### 🌐 বহু-ভাষা সমর্থন

#### GitHub Action এর মাধ্যমে সমর্থিত (স্বয়ংক্রিয় ও সর্বদা আপ-টু-ডেট)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](./README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **স্থানীয়ভাবে ক্লোন করতে চান?**
>
> এই রিপোজিটরিতে ৫০+ ভাষার অনুবাদ রয়েছে যা ডাউনলোড আকার অনেক বড় করে তোলে। অনুবাদ ছাড়া ক্লোন করার জন্য sparse checkout ব্যবহার করুন:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> এটি আপনাকে কোর্স সম্পন্ন করার জন্য প্রয়োজনীয় সবকিছু অনেক দ্রুত ডাউনলোডের মাধ্যমে দেয়।
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## শেখার উদ্দেশ্যসমূহ

এই কর্মশালার শেষে আপনি করতে পারবেন:

| # | উদ্দেশ্য |
|---|-----------|
| 1 | Foundry Local ইনস্টল করা এবং CLI দিয়ে মডেলগুলি পরিচালনা করা |
| 2 | প্রোগ্রাম্যাটিক মডেল ব্যবস্থাপনার জন্য Foundry Local SDK API দক্ষতার সাথে ব্যবহার করা |
| 3 | Python, JavaScript, এবং C# SDK গুলোর মাধ্যমে লোকাল ইনফারেন্স সার্ভারের সাথে সংযোগ স্থাপন করা |
| 4 | আপনার নিজস্ব তথ্য ভিত্তিতে উত্তর প্রদান করার জন্য Retrieval-Augmented Generation (RAG) পাইপলাইন তৈরি করা |
| 5 | স্থায়ী নির্দেশনা এবং পার্সোনাস সহ AI এজেন্ট তৈরি করা |
| 6 | ফিডব্যাক লুপ সহ বহু-এজেন্ট ওয়ার্কফ্লো পরিচালনা করা |
| 7 | একটি প্রোডাকশন ক্যাপস্টোন অ্যাপ - Zava Creative Writer অন্বেষণ করা |
| 8 | গোল্ডেন ডেটাসেট এবং LLM-as-judge স্কোরিং সহ মূল্যায়ন কাঠামো তৈরি করা |
| 9 | Whisper দিয়ে অডিও ট্রান্সক্রিপশন - Foundry Local SDK ব্যবহার করে ডিভাইস-এ স্পিচ-টু-টেক্সট |
| 10 | ONNX Runtime GenAI এবং Foundry Local দিয়ে কাস্টম বা Hugging Face মডেল কম্পাইল ও চালানো |
| 11 | টুল-কলিং প্যাটার্নের মাধ্যমে লোকাল মডেলকে বাহ্যিক ফাংশন কল করার সক্ষমতা দেওয়া |
| 12 | বাস্তব সময় স্ট্রিমিং সহ Zava Creative Writer এর জন্য ব্রাউজার-ভিত্তিক UI তৈরি করা |

---

## পূর্বশর্তসমূহ

| প্রয়োজনীয়তা | বিবরণ |
|-------------|---------|
| **হার্ডওয়্যার** | কমপক্ষে ৮ GB RAM (১৬ GB সুপারিশ করা হয়); AVX2-সক্ষম CPU অথবা একটি সমর্থিত GPU |
| **অপারেটিং সিস্টেম** | Windows 10/11 (x64/ARM), Windows Server 2025, অথবা macOS 13+ |
| **Foundry Local CLI** | Windows-এর জন্য `winget install Microsoft.FoundryLocal` অথবা macOS-এর জন্য `brew tap microsoft/foundrylocal && brew install foundrylocal` কমান্ড দিয়ে ইনস্টল করুন। বিস্তারিত দেখুন [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started)। |
| **ভাষা রানটাইম** | **Python 3.9+** এবং/অথবা **.NET ৯.০+** এবং/অথবা **Node.js ১৮+** |
| **Git** | এই রিপোজিটরি ক্লোন করার জন্য |

---

## শুরু করা যাক

```bash
# 1. রিপোজিটরি ক্লোন করুন
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. নিশ্চিত করুন যে ফাউন্ড্রি লোকাল ইনস্টল করা আছে
foundry model list              # উপলব্ধ মডেলগুলি তালিকা করুন
foundry model run phi-3.5-mini  # একটি ইন্টারেক্টিভ চ্যাট শুরু করুন

# 3. আপনার ভাষা ট্র্যাক নির্বাচন করুন (পূর্ন সেটআপের জন্য পার্ট 2 ল্যাব দেখুন)
```

| ভাষা | দ্রুত শুরু করুন |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## কর্মশালার অংশসমূহ

### অংশ ১: Foundry Local দিয়ে শুরু করা

**ল্যাব গাইড:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local কি এবং এটি কীভাবে কাজ করে
- Windows এবং macOS-এ CLI ইনস্টল করা
- মডেলগুলি অন্বেষণ করা - তালিকা, ডাউনলোড, চালানো
- মডেল এলিয়াস এবং ডায়নামিক পোর্ট বোঝা

---

### অংশ ২: Foundry Local SDK গভীরতর জানাক

**ল্যাব গাইড:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- অ্যাপ্লিকেশন ডেভেলপমেন্টে CLI এর বদলে SDK কেন ব্যবহার করবেন
- Python, JavaScript, এবং C# এর জন্য সম্পূর্ণ SDK API রেফারেন্স
- সার্ভিস ম্যানেজমেন্ট, ক্যাটালগ ব্রাউজিং, মডেল লাইফসাইকেল (ডাউনলোড, লোড, আনলোড)
- দ্রুত শুরু প্যাটার্ন: Python কন্সট্রাক্টর বুটস্ট্রাপ, JavaScript এর `init()`, C# এর `CreateAsync()`
- `FoundryModelInfo` মেটাডেটা, এলিয়াস এবং হার্ডওয়্যার-সর্বোত্তম মডেল নির্বাচন

---

### অংশ ৩: SDK এবং API গুলি

**ল্যাব গাইড:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, এবং C# থেকে Foundry Local এ সংযোগ করা
- Foundry Local SDK ব্যবহার করে সার্ভিস প্রোগ্রাম্যাটিক্যালি পরিচালনা করা
- OpenAI-কম্প্যাটিবল API দিয়ে স্ট্রিমিং চ্যাট কমপ্লিশন
- প্রতিটি ভাষার SDK মেথড রেফারেন্স

**কোড নমুনা:**

| ভাষা | ফাইল | বর্ণনা |
|----------|------|-------------|
| Python | `python/foundry-local.py` | বেসিক স্ট্রিমিং চ্যাট |
| C# | `csharp/BasicChat.cs` | .NET এর সাথে স্ট্রিমিং চ্যাট |
| JavaScript | `javascript/foundry-local.mjs` | Node.js এর সাথে স্ট্রিমিং চ্যাট |

---

### অংশ ৪: Retrieval-Augmented Generation (RAG)

**ল্যাব গাইড:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG কি এবং কেন তা গুরুত্বপূর্ণ
- ইন-মেমোরি জ্ঞানভিত্তিক ডাটাবেস তৈরি করা
- স্কোরিং সহ কীওয়ার্ড-ওভারল্যাপ রিট্রিভাল
- গ্রাউন্ডেড সিস্টেম প্রম্পট কম্পোজ করা
- ডিভাইসে সম্পূর্ণ RAG পাইপলাইন চালানো

**কোড নমুনা:**

| ভাষা | ফাইল |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### অংশ ৫: AI এজেন্ট তৈরি

**ল্যাব গাইড:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- একটি AI এজেন্ট কি (কাঁচা LLM কলের তুলনায়)
- `ChatAgent` প্যাটার্ন এবং Microsoft Agent Framework
- সিস্টেম নির্দেশনা, পার্সোনাস, ও বহু-বার্তা কথোপকথন
- এজেন্ট থেকে কাঠামোবদ্ধ আউটপুট (JSON)

**কোড নমুনা:**

| ভাষা | ফাইল | বর্ণনা |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework সহ একক এজেন্ট |
| C# | `csharp/SingleAgent.cs` | একক এজেন্ট (ChatAgent প্যাটার্ন) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | একক এজেন্ট (ChatAgent প্যাটার্ন) |

---

### অংশ ৬: বহু-এজেন্ট ওয়ার্কফ্লো

**ল্যাব গাইড:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- বহু-এজেন্ট পাইপলাইন: গবেষক → লেখক → সম্পাদক
- ধারাবাহিক নির্দেশনা ও ফিডব্যাক লুপ
- শেয়ার করা কনফিগারেশন এবং কাঠামোবদ্ধ হ্যান্ড-অফ
- আপনার নিজস্ব বহু-এজেন্ট ওয়ার্কফ্লো ডিজাইন করা

**কোড নমুনা:**

| ভাষা | ফাইল | বর্ণনা |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | তিন-এজেন্ট পাইপলাইন |
| C# | `csharp/MultiAgent.cs` | তিন-এজেন্ট পাইপলাইন |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | তিন-এজেন্ট পাইপলাইন |

---

### অংশ ৭: Zava Creative Writer - ক্যাপস্টোন অ্যাপ্লিকেশন

**ল্যাব গাইড:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- ৪টি বিশেষায়িত এজেন্ট সহ প্রোডাকশন-স্টাইল বহু-এজেন্ট অ্যাপ
- ধারাবাহিক পাইপলাইন এবং মূল্যায়ক দ্বারা পরিচালিত ফিডব্যাক লুপ
- স্ট্রিমিং আউটপুট, পণ্য ক্যাটালগ অনুসন্ধান, কাঠামোবদ্ধ JSON হ্যান্ড-অফ
- Python (FastAPI), JavaScript (Node.js CLI), এবং C# (.NET কনসোল) এ সম্পূর্ণ বাস্তবায়ন

**কোড নমুনা:**

| ভাষা | ডিরেক্টরি | বর্ণনা |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI ওয়েব সার্ভিস সহ অর্থনীতি পরিচালক |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI অ্যাপ্লিকেশন |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 কনসোল অ্যাপ্লিকেশন |

---

### অংশ ৮: মূল্যায়ন-নির্ভর উন্নয়ন

**ল্যাব গাইড:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- গোল্ডেন ডেটাসেট ব্যবহার করে AI এজেন্টের জন্য একটি পদ্ধতিগত মূল্যায়ন কাঠামো তৈরি করুন
- নিয়ম-ভিত্তিক চেক (দৈর্ঘ্য, কীওয়ার্ড কাভারেজ, নিষিদ্ধ শব্দ) + LLM-as-judge স্কোরিং
- প্রম্পট ভেরিয়েন্টের পার্শ্বাত পার্শ্ব তুলনা ও সমষ্টিগত স্কোরকার্ড
- অংশ ৭ থেকে Zava Editor এজেন্ট প্যাটার্নকে অফলাইন টেস্ট স্যুটে প্রসারিত করে
- Python, JavaScript, এবং C# ট্র্যাক

**কোড নমুনা:**

| ভাষা | ফাইল | বর্ণনা |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | মূল্যায়ন কাঠামো |
| C# | `csharp/AgentEvaluation.cs` | মূল্যায়ন কাঠামো |
| JavaScript | `javascript/foundry-local-eval.mjs` | মূল্যায়ন কাঠামো |

---

### অংশ ৯: Whisper সহ ভয়েস ট্রান্সক্রিপশন

**ল্যাব গাইড:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- OpenAI Whisper ব্যবহার করে স্থানীয়ভাবে স্পিচ-টু-টেক্সট ট্রান্সক্রিপশন
- প্রাইভেসি-ফার্স্ট অডিও প্রসেসিং - অডিও কখনোই আপনার ডিভাইস ছাড়ে না
- Python, JavaScript, এবং C# ট্র্যাকসমূহ `client.audio.transcriptions.create()` (Python/JS) এবং `AudioClient.TranscribeAudioAsync()` (C#) এর মাধ্যমে
- হাতে কলমে অভ্যাসের জন্য Zava-থিমযুক্ত স্যাম্পল অডিও ফাইল অন্তর্ভুক্ত

**কোড নমুনা:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper ভয়েস ট্রান্সক্রিপশন |
| C# | `csharp/WhisperTranscription.cs` | Whisper ভয়েস ট্রান্সক্রিপশন |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper ভয়েস ট্রান্সক্রিপশন |

> **Note:** এই ল্যাবটি **Foundry Local SDK** ব্যবহার করে প্রোগ্রাম্যাটিক্যালি Whisper মডেল ডাউনলোড ও লোড করে, তারপর ট্রান্সক্রিপশনের জন্য অডিও স্থানীয় OpenAI-সঙ্গতিপূর্ণ এন্ডপয়েন্টে পাঠায়। Whisper মডেল (`whisper`) Foundry Local ক্যাটালগে তালিকাভুক্ত এবং সম্পূর্ণভাবে ডিভাইসে চলে - কোন ক্লাউড API কী বা নেটওয়ার্ক অ্যাক্সেস প্রয়োজন হয় না।

---

### পার্ট ১০: কাস্টম বা Hugging Face মডেল ব্যবহার

**ল্যাব গাইড:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face মডেলসমূহকে ONNX Runtime GenAI মডেল বিল্ডার ব্যবহার করে অপ্টিমাইজড ONNX ফরম্যাটে কম্পাইল করা
- হার্ডওয়্যার-নির্দিষ্ট কম্পাইলেশন (CPU, NVIDIA GPU, DirectML, WebGPU) এবং কোয়ান্টাইজেশন (int4, fp16, bf16)
- Foundry Local এর জন্য চ্যাট-টেমপ্লেট কনফিগারেশন ফাইল তৈরি করা
- কম্পাইল করা মডেলগুলো Foundry Local ক্যাশে যোগ করা
- কাস্টম মডেল CLI, REST API ও OpenAI SDK মাধ্যমে চালানো
- রেফারেন্স উদাহরণ: Qwen/Qwen3-0.6B সম্পূর্ণ কম্পাইলেশন

---

### পার্ট ১১: লোকাল মডেল দিয়ে টুল কলিং

**ল্যাব গাইড:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- লোকাল মডেলগুলোকে বাহ্যিক ফাংশন কল (টুল/ফাংশন কলিং) সক্ষম করা
- OpenAI ফাংশন-কলিং ফরম্যাট ব্যবহার করে টুল স্কিমা সংজ্ঞায়িত করা
- মাল্টি-টার্ন টুল-কলিং কথোপকথন প্রবাহ পরিচালনা করা
- টুল কলগুলো লোকালি এক্সিকিউট করে ফলাফল মডেলে ফেরত পাঠানো
- টুল-কলিং পরিস্থিতির জন্য সঠিক মডেল নির্বাচন (Qwen 2.5, Phi-4-mini)
- SDK এর নেটিভ `ChatClient` ব্যবহার করে টুল কলিং (JavaScript)

**কোড নমুনা:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | আবহাওয়া/জনসংখ্যা টুল দিয়ে টুল কলিং |
| C# | `csharp/ToolCalling.cs` | .NET দিয়ে টুল কলিং |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient দিয়ে টুল কলিং |

---

### পার্ট ১২: Zava ক্রিয়েটিভ রাইটারের জন্য ওয়েব UI তৈরি

**ল্যাব গাইড:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava ক্রিয়েটিভ রাইটারের জন্য ব্রাউজার-ভিত্তিক ফ্রন্টএন্ড যোগ করা
- Python (FastAPI), JavaScript (Node.js HTTP) এবং C# (ASP.NET Core) থেকে শেয়ার্ড UI সার্ভ করা
- ব্রাউজারে Fetch API এবং ReadableStream দিয়ে স্ট্রিমিং NDJSON গ্রহণ করা
- লাইভ এজেন্ট স্ট্যাটাস ব্যাজ এবং রিয়েল-টাইম আর্টিকেল টেক্সট স্ট্রিমিং

**কোড (শেয়ার্ড UI):**

| File | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | পেজ লে-আউট |
| `zava-creative-writer-local/ui/style.css` | স্টাইলিং |
| `zava-creative-writer-local/ui/app.js` | স্ট্রিম রিডার ও DOM আপডেট লজিক |

**ব্যাকএন্ড সংযোজন:**

| Language | File | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | স্ট্যাটিক UI সার্ভ করার জন্য আপডেটেড |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | নতুন HTTP সার্ভার যা অর্কেস্ট্রেটরকে র‍্যাপ করে |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | নতুন ASP.NET Core মিনিমাল API প্রজেক্ট |

---

### পার্ট ১৩: ওয়ার্কশপ সমাপ্ত

**ল্যাব গাইড:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- আপনি যা কিছু ১২টি পার্ট জুড়ে নির্মাণ করেছেন তার সারাংশ
- আপনার অ্যাপ্লিকেশন সম্প্রসারণের জন্য অতিরিক্ত ধারণা
- রিসোর্স ও ডকুমেন্টেশনের লিঙ্কসমূহ

---

## প্রজেক্ট স্ট্রাকচার

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## রিসোর্সসমূহ

| Resource | Link |
|----------|------|
| Foundry Local ওয়েবসাইট | [foundrylocal.ai](https://foundrylocal.ai) |
| মডেল ক্যাটালগ | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| শুরু করার গাইড | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK রেফারেন্স | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent ফ্রেমওয়ার্ক | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## লাইসেন্স

এই ওয়ার্কশপ উপকরণ শিক্ষা উদ্দেশ্যে প্রদান করা হয়েছে।

---

**আপনি সফলভাবে তৈরি করুন! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**স্বীকৃতি**:  
এই নথিটি AI অনুবাদ সেবা [Co-op Translator](https://github.com/Azure/co-op-translator) ব্যবহার করে অনূদিত হয়েছে। আমরা যথাসাধ্য সঠিকতার জন্য চেষ্টা করি, তবে দয়া করে মনে রাখবেন যে স্বয়ংক্রিয় অনুবাদে ভুল বা অসংগতি থাকতে পারে। মূল নথি তার নিজস্ব ভাষায় প্রামাণিক উৎস হিসাবে বিবেচিত হওয়া উচিত। গুরুত্বপূর্ণ তথ্যের জন্য পেশাদার মানব অনুবাদের পরামর্শ দেওয়া হয়। এই অনুবাদের ব্যবহার থেকে যে কোনো ভুল বোঝাবুঝি বা ভুল ব্যাখ্যার জন্য আমরা দায়বদ্ধ নই।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->