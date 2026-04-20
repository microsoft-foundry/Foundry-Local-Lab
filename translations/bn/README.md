<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local ওয়ার্কশপ - ডিভাইসে এআই অ্যাপস তৈরি করুন

একটি হ্যান্ডস-অন ওয়ার্কশপ যা আপনাকে আপনার নিজস্ব মেশিনে ভাষা মডেল চালাতে এবং [Foundry Local](https://foundrylocal.ai) এবং [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) ব্যবহার করে বুদ্ধিমান অ্যাপ্লিকেশন তৈরি করতে সহায়তা করবে।

> **Foundry Local কী?** Foundry Local একটি লাইটওয়েট রানটাইম যা আপনাকে সম্পূর্ণভাবে আপনার হার্ডওয়্যারে ভাষা মডেল ডাউনলোড, ম্যানেজ এবং সার্ভ করতে দেয়। এটি একটি **OpenAI-সঙ্গত API** উন্মোচিত করে যাতে যেকোন টুল বা SDK যা OpenAI কথা বলে তা সংযোগ করতে পারে - কোনো ক্লাউড অ্যাকাউন্ট প্রয়োজন নেই।

---

## শেখার উদ্দেশ্য

এই ওয়ার্কশপের শেষে আপনি সক্ষম হবেন:

| # | উদ্দেশ্য |
|---|-----------|
| 1 | Foundry Local ইনস্টল করুন এবং CLI দিয়ে মডেল ম্যানেজ করুন |
| 2 | Foundry Local SDK API মাস্টার করুন প্রোগ্রাম্যাটিক মডেল ম্যানেজমেন্টের জন্য |
| 3 | Python, JavaScript, এবং C# SDK ব্যবহার করে লোকাল ইনফারেন্স সার্ভারের সাথে সংযোগ করুন |
| 4 | একটি Retrieval-Augmented Generation (RAG) পাইপলাইন তৈরি করুন যা আপনার নিজস্ব ডেটাতে ভিত্তি করে উত্তর প্রদান করে |
| 5 | স্থায়ী নির্দেশাবলী এবং প্যারসোনাস সহ AI এজেন্ট তৈরি করুন |
| 6 | মাল্টি-এজেন্ট ওয়ার্কফ্লো পরিচালনা করুন প্রতিক্রিয়া লুপ সহ |
| 7 | একটি প্রোডাকশন ক্যাপস্টোন অ্যাপ অনুসন্ধান করুন - Zava Creative Writer |
| 8 | গোল্ডেন ডেটাসেট এবং LLM-as-judge স্কোরিং সহ মূল্যায়ন ফ্রেমওয়ার্ক তৈরি করুন |
| 9 | Whisper ব্যবহার করে অডিও ট্রান্সক্রাইব করুন - ডিভাইসে স্পিচ-টু-টেক্সট Foundry Local SDK দিয়ে |
| 10 | কাস্টম বা Hugging Face মডেল ONNX Runtime GenAI এবং Foundry Local দিয়ে কম্পাইল এবং চালান |
| 11 | টুল-কলিং প্যাটার্ন ব্যবহার করে লোকাল মডেলকে বাহ্যিক ফাংশন কল করতে সক্ষম করুন |
| 12 | রিয়েল-টাইম স্ট্রিমিং সহ Zava Creative Writer এর জন্য ব্রাউজার-ভিত্তিক UI তৈরি করুন |

---

## প্রয়োজনীয়তা

| প্রয়োজনীয়তা | বিস্তারিত |
|-------------|---------|
| **হার্ডওয়্যার** | ন্যূনতম ৮ জিবি RAM (১৬ জিবি প্রস্তাবিত); AVX2-সক্ষম CPU বা সমর্থিত GPU |
| **অপারেটিং সিস্টেম** | Windows 10/11 (x64/ARM), Windows Server 2025, অথবা macOS ১৩+ |
| **Foundry Local CLI** | Windows-এ `winget install Microsoft.FoundryLocal` অথবা macOS-এ `brew tap microsoft/foundrylocal && brew install foundrylocal` দিয়ে ইনস্টল করুন। বিস্তারিত জন্য [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) দেখুন। |
| **ভাষা রানটাইম** | **Python 3.9+** এবং/অথবা **.NET 9.0+** এবং/অথবা **Node.js 18+** |
| **Git** | এই রিপোজিটরি ক্লোন করার জন্য |

---

## শুরু করা যাক

```bash
# ১. রিপোসিটরি ক্লোন করুন
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# ২. নিশ্চিত করুন যে Foundry Local ইনস্টল করা হয়েছে
foundry model list              # উপলব্ধ মডেলগুলি তালিকাভুক্ত করুন
foundry model run phi-3.5-mini  # একটি ইন্টারেক্টিভ চ্যাট শুরু করুন

# ৩. আপনার ভাষা ট্র্যাক নির্বাচন করুন (সম্পূর্ণ সেটআপের জন্য পার্ট ২ ল্যাব দেখুন)
```

| ভাষা | দ্রুত শুরু |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ওয়ার্কশপ অংশসমূহ

### অংশ ১: Foundry Local এর সাথে শুরু করা

**ল্যাব গাইড:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local কী এবং এটি কিভাবে কাজ করে
- Windows এবং macOS এ CLI ইনস্টল করা
- মডেল অন্বেষণ - তালিকা, ডাউনলোড, রান করা
- মডেল এলিয়াস এবং ডায়নামিক পোর্ট বোঝা

---

### অংশ ২: Foundry Local SDK গভীর অধ্যয়ন

**ল্যাব গাইড:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- অ্যাপ্লিকেশন ডেভেলপমেন্টের জন্য CLI এর পরিবর্তে SDK ব্যবহারের কারণ
- Python, JavaScript, এবং C# এর জন্য সম্পূর্ণ SDK API রেফারেন্স
- সার্ভিস ম্যানেজমেন্ট, ক্যাটালগ ব্রাউজিং, মডেল লাইফসাইকেল (ডাউনলোড, লোড, আনলোড)
- দ্রুত শুরু প্যাটার্ন: Python কনস্ট্রাক্টর বুটস্ট্র্যাপ, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` মেটাডেটা, এলিয়াস, এবং হার্ডওয়্যার-ব্রতিত্ব মডেল নির্বাচন

---

### অংশ ৩: SDK এবং API

**ল্যাব গাইড:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, এবং C# থেকে Foundry Local সংযোগ করা
- Foundry Local SDK ব্যবহার করে প্রোগ্রাম্যাটিক সার্ভিস ম্যানেজমেন্ট
- OpenAI-সঙ্গত API এর মাধ্যমে স্ট্রিমিং চ্যাট কমপ্লিশন
- প্রতিটি ভাষার জন্য SDK মেথড রেফারেন্স

**কোড নমুনা:**

| ভাষা | ফাইল | বর্ণনা |
|----------|------|-------------|
| Python | `python/foundry-local.py` | বেসিক স্ট্রিমিং চ্যাট |
| C# | `csharp/BasicChat.cs` | .NET এর সাথে স্ট্রিমিং চ্যাট |
| JavaScript | `javascript/foundry-local.mjs` | Node.js দিয়ে স্ট্রিমিং চ্যাট |

---

### অংশ ৪: Retrieval-Augmented Generation (RAG)

**ল্যাব গাইড:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG কী এবং কেন গুরুত্বপূর্ণ
- একটি ইন-মেমরি নলেজ বেস তৈরি করা
- কিওয়ার্ড-ওভারল্যাপ রিট্রিভাল এবং স্কোরিং
- ভিত্তিক সিস্টেম প্রম্পট গঠন করা
- একটি সম্পূর্ণ RAG পাইপলাইন ডিভাইসে চালানো

**কোড নমুনা:**

| ভাষা | ফাইল |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### অংশ ৫: AI এজেন্ট তৈরি করা

**ল্যাব গাইড:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI এজেন্ট কী (কাঁচা LLM কল এর বিপরীতে)
- `ChatAgent` প্যাটার্ন এবং Microsoft Agent Framework
- সিস্টেম নির্দেশাবলী, প্যারসোনাস, এবং মাল্টি-টার্ন কথোপকথন
- এজেন্ট থেকে স্ট্রাকচার্ড আউটপুট (JSON)

**কোড নমুনা:**

| ভাষা | ফাইল | বর্ণনা |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework সহ সিঙ্গেল এজেন্ট |
| C# | `csharp/SingleAgent.cs` | সিঙ্গেল এজেন্ট (ChatAgent প্যাটার্ন) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | সিঙ্গেল এজেন্ট (ChatAgent প্যাটার্ন) |

---

### অংশ ৬: মাল্টি-এজেন্ট ওয়ার্কফ্লো

**ল্যাব গাইড:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- মাল্টি-এজেন্ট পাইপলাইন: রিসার্চার → লেখক → সম্পাদক
- ধারাবাহিক অর্কেস্ট্রেশন ও ফিডব্যাক লুপ
- শেয়ার্ড কনফিগারেশন এবং স্ট্রাকচার্ড হ্যান্ড-অফ
- নিজের মাল্টি-এজেন্ট ওয়ার্কফ্লো ডিজাইন করা

**কোড নমুনা:**

| ভাষা | ফাইল | বর্ণনা |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | তিন-এজেন্ট পাইপলাইন |
| C# | `csharp/MultiAgent.cs` | তিন-এজেন্ট পাইপলাইন |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | তিন-এজেন্ট পাইপলাইন |

---

### অংশ ৭: Zava Creative Writer - ক্যাপস্টোন অ্যাপ্লিকেশন

**ল্যাব গাইড:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- ৪টি বিশেষায়িত এজেন্ট সহ প্রোডাকশন-স্টাইল মাল্টি-এজেন্ট অ্যাপ
- ধারাবাহিক পাইপলাইন এবং ইভ্যালুয়েটর-চালিত ফিডব্যাক লুপ
- স্ট্রিমিং আউটপুট, প্রোডাক্ট ক্যাটালগ সার্চ, স্ট্রাকচার্ড JSON হ্যান্ড-অফ
- Python (FastAPI), JavaScript (Node.js CLI), এবং C# (.NET কনসোল) এ পূর্ণাঙ্গ ইম্প্লিমেন্টেশন

**কোড নমুনা:**

| ভাষা | ডিরেক্টরি | বর্ণনা |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI ওয়েব সার্ভিস অর্কেস্ট্রেটর সহ |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI অ্যাপ্লিকেশন |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 কনসোল অ্যাপ্লিকেশন |

---

### অংশ ৮: ইভ্যালুয়েশন-নেতৃৃত্বাধীন ডেভেলপমেন্ট

**ল্যাব গাইড:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- গোল্ডেন ডেটাসেট ব্যবহার করে AI এজেন্টের জন্য সিস্টেম্যাটিক ইভ্যালুয়েশন ফ্রেমওয়ার্ক তৈরি করা
- রুল-ভিত্তিক চেক (দৈর্ঘ্য, কিওয়ার্ড কভারেজ, নিষিদ্ধ শব্দ) + LLM-as-judge স্কোরিং
- প্রম্পট ভেরিয়ান্টগুলোর পারস্পরিক তুলনা এবং সামগ্রিক স্কোরকার্ড
- পার্ট ৭ এর Zava Editor এজেন্ট প্যাটার্নকে অফলাইন টেস্ট স্যুটে বিস্তার
- Python, JavaScript, এবং C# ট্র্যাক

**কোড নমুনা:**

| ভাষা | ফাইল | বর্ণনা |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | ইভ্যালুয়েশন ফ্রেমওয়ার্ক |
| C# | `csharp/AgentEvaluation.cs` | ইভ্যালুয়েশন ফ্রেমওয়ার্ক |
| JavaScript | `javascript/foundry-local-eval.mjs` | ইভ্যালুয়েশন ফ্রেমওয়ার্ক |

---

### অংশ ৯: Whisper দিয়ে ভয়েস ট্রান্সক্রিপশন

**ল্যাব গাইড:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- OpenAI Whisper ব্যবহার করে লোকালি স্পিচ-টু-টেক্সট ট্রান্সক্রিপশন
- প্রাইভেসি-প্রথম অডিও প্রসেসিং - অডিও কখনই আপনার ডিভাইস ছাড়ে না
- Python, JavaScript, এবং C# ট্র্যাকসমূহে `client.audio.transcriptions.create()` (Python/JS) এবং `AudioClient.TranscribeAudioAsync()` (C#) ব্যবহার করা
- হ্যান্ডস-অন অনুশীলনের জন্য Zava-থিমযুক্ত নমুনা অডিও ফাইল সহ

**কোড নমুনা:**

| ভাষা | ফাইল | বর্ণনা |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper ভয়েস ট্রান্সক্রিপশন |
| C# | `csharp/WhisperTranscription.cs` | Whisper ভয়েস ট্রান্সক্রিপশন |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper ভয়েস ট্রান্সক্রিপশন |

> **নোট:** এই ল্যাবটি প্রোগ্রাম্যাটিকভাবে Whisper মডেল ডাউনলোড এবং লোড করতে **Foundry Local SDK** ব্যবহার করে, তারপর ট্রান্সক্রিপশনের জন্য অডিও লোকাল OpenAI-সঙ্গত এন্ডপয়েন্টে পাঠায়। Whisper মডেল (`whisper`) Foundry Local ক্যাটালগে তালিকাভুক্ত এবং সম্পূর্ণরূপে ডিভাইসে চলে - কোনো ক্লাউড API কী অথবা নেটওয়ার্ক অ্যাক্সেস প্রয়োজন হয় না।

---

### অংশ ১০: কাস্টম বা Hugging Face মডেল ব্যবহার

**ল্যাব গাইড:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face মডেলগুলোকে ONNX Runtime GenAI মডেল বিল্ডার দিয়ে অপ্টিমাইজড ONNX ফর্ম্যাটে কম্পাইল করা
- হার্ডওয়্যার-নির্দিষ্ট কম্পাইলেশন (CPU, NVIDIA GPU, DirectML, WebGPU) এবং কোয়ান্টাইজেশন (int4, fp16, bf16)
- Foundry Local এর জন্য চ্যাট-টেমপ্লেট কনফিগারেশন ফাইল তৈরি করা
- Foundry Local ক্যাশে কম্পাইল্ড মডেল যোগ করা
- CLI, REST API, এবং OpenAI SDK এর মাধ্যমে কাস্টম মডেল রান করা
- রেফারেন্স উদাহরণ: Qwen/Qwen3-0.6B প্রথম থেকে শেষ পর্যন্ত কম্পাইলেশন

---

### অংশ ১১: টুল কলিং লোকাল মডেল দিয়ে

**ল্যাব গাইড:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- লোকাল মডেলকে বাহ্যিক ফাংশন কল (টুল/ফাংশন কলিং) সক্ষম করা
- OpenAI ফাংশন-কলিং ফরম্যাট ব্যবহার করে টুল স্কিমা ডিফাইন করা
- মাল্টি-টার্ন টুল-কলিং কথোপকথন প্রবাহ পরিচালনা করা
- টুল কল স্থানীয়ভাবে চালানো এবং ফলাফল মডেলে ফেরত দেওয়া
- টুল-কলিং পরিস্থিতির জন্য সঠিক মডেল নির্বাচন (Qwen 2.5, Phi-4-mini)
- SDK এর নেটিভ `ChatClient` ব্যবহার করে টুল কলিং (JavaScript)

**কোড নমুনা:**

| ভাষা | ফাইল | বর্ণনা |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | টুল কলিং সাথে ওয়েদার/পপুলেশন টুল |
| C# | `csharp/ToolCalling.cs` | .NET সহ টুল কলিং |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient দিয়ে টুল কলিং |

---

### অংশ ১২: Zava Creative Writer এর জন্য ওয়েব UI তৈরি

**ল্যাব গাইড:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer এর জন্য ব্রাউজার-ভিত্তিক ফ্রন্টএন্ড যোগ করা
- Python (FastAPI), JavaScript (Node.js HTTP), এবং C# (ASP.NET Core) থেকে শেয়ার্ড UI সার্ভ করা
- ব্রাউজারে Fetch API এবং ReadableStream দিয়ে স্ট্রিমিং NDJSON খাওয়ানো
- লাইভ এজেন্ট স্ট্যাটাস ব্যাজ এবং রিয়েল-টাইম আর্টিকেল টেক্সট স্ট্রিমিং

**কোড (শেয়ার্ড UI):**

| ফাইল | বর্ণনা |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | পেজ লেআউট |
| `zava-creative-writer-local/ui/style.css` | স্টাইলিং |
| `zava-creative-writer-local/ui/app.js` | স্ট্রিম রিডার এবং DOM আপডেট লজিক |

**ব্যাকএন্ড সংযোজন:**

| ভাষা | ফাইল | বর্ণনা |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | স্ট্যাটিক UI সার্ভ করার জন্য আপডেটেড |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | অর্কেস্ট্রেটর র‍্যাপপিং HTTP সার্ভার নতুন |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | নতুন ASP.NET Core মিনিমাল API প্রজেক্ট |

---

### অংশ ১৩: ওয়ার্কশপ সম্পন্ন
**ল্যাব গাইড:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- আপনি যেসব বিষয় ১২টি পার্টের মধ্যে তৈরি করেছেন তার সারসংক্ষেপ
- আপনার অ্যাপ্লিকেশন সম্প্রসারণের জন্য আরও ধারণা
- সম্পদ ও ডকুমেন্টেশনের লিঙ্কসমূহ

---

## প্রকল্পের কাঠামো

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

## সম্পদ

| সম্পদ | লিঙ্ক |
|----------|------|
| Foundry Local ওয়েবসাইট | [foundrylocal.ai](https://foundrylocal.ai) |
| মডেল ক্যাটালগ | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| শুরু করার গাইড | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK রেফারেন্স | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## লাইসেন্স

এই ওয়ার্কশপ উপকরণ শিক্ষামূলক উদ্দেশ্যে প্রদান করা হয়েছে।

---

**সুখী নির্মাণ! 🚀**