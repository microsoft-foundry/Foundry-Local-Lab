# কোডিং এজেন্ট নির্দেশাবলী

এই ফাইলটি এই রিপোজিটরিতে কাজ করা AI কোডিং এজেন্টগুলো (GitHub Copilot, Copilot Workspace, Codex ইত্যাদি) এর জন্য প্রাসঙ্গিক তথ্য প্রদান করে।

## প্রকল্প পর্যালোচনা

এটি [Foundry Local](https://foundrylocal.ai) দিয়ে AI অ্যাপ্লিকেশন তৈরির জন্য একটি **হ্যান্ডস-অন কর্মশালা** — একটি হালকা ওজনের রানটাইম যা ওপেনএআই-অনুকূল API এর মাধ্যমে সম্পূর্ণ ডিভাইসে ভাষা মডেলগুলি ডাউনলোড, পরিচালনা এবং সার্ভ করে। কর্মশালায় ধাপে ধাপে ল্যাব গাইড এবং পাইথন, জাভাস্ক্রিপ্ট, ও C# এ রানযোগ্য কোড নমুনাগুলি অন্তর্ভুক্ত রয়েছে।

## রিপোজিটরি কাঠামো

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## ভাষা ও ফ্রেমওয়ার্ক বিস্তারিত

### পাইথন
- **অবস্থান:** `python/`, `zava-creative-writer-local/src/api/`
- **নির্ভরশীলতা:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **মূল প্যাকেজ:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **সর্বনিম্ন সংস্করণ:** Python 3.9+
- **চালানো:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### জাভাস্ক্রিপ্ট
- **অবস্থান:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **নির্ভরশীলতা:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **মূল প্যাকেজ:** `foundry-local-sdk`, `openai`
- **মডিউল সিস্টেম:** ES মডিউল (`.mjs` ফাইল, `"type": "module"`)
- **সর্বনিম্ন সংস্করণ:** Node.js 18+
- **চালানো:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **অবস্থান:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **প্রকল্প ফাইল:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **মূল প্যাকেজ:** `Microsoft.AI.Foundry.Local` (নন-উইন্ডোজ), `Microsoft.AI.Foundry.Local.WinML` (উইন্ডোজ — QNN EP সহ একটি সুপারসেট), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **টার্গেট:** .NET 9.0 (শর্তানুসারে TFM: উইন্ডোজে `net9.0-windows10.0.26100`, অন্যত্র `net9.0`)
- **চালানো:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## কোডিং রীতি

### সাধারণ
- সকল কোড নমুনা **স্ব-সম্পূর্ণ একক ফাইল উদাহরণ** — কোনো শেয়ার্ড ইউটিলিটি লাইব্রেরি বা অবস্ট্র্যাকশান নেই।
- প্রতিটি নমুনা তার নিজস্ব নির্ভরশীলতা ইন্সটল করার পরে স্বাধীনভাবে চালানো যায়।
- API কী সবসময় `"foundry-local"` এ সেট করা থাকে — Foundry Local এটি প্লেসহোল্ডার হিসেবে ব্যবহার করে।
- বেস URL হলো `http://localhost:<port>/v1` — পোর্টটি গতিশীল এবং SDK দ্বারা রানটাইমে আবিষ্কৃত হয় (`manager.urls[0]` জেএস-এ, `manager.endpoint` পাইথনে)।
- Foundry Local SDK সার্ভিস স্টার্টআপ এবং এন্ডপয়েন্ট আবিষ্কারে কাজ করে; হার্ড-কোডেড পোর্টের চেয়ে SDK প্যাটার্ন পছন্দ করুন।

### পাইথন
- `openai` SDK `OpenAI(base_url=..., api_key="not-required")` এর সাথে ব্যবহার করুন।
- SDK পরিচালিত সার্ভিস লাইফসাইকেলের জন্য `foundry_local` এর `FoundryLocalManager()` ব্যবহার করুন।
- স্ট্রিমিং: `for chunk in stream:` দিয়ে `stream` অবজেক্টে ইটারেট করুন।
- নমুনা ফাইলগুলিতে টাইপ অ্যানোটেশন নেই (ওয়ার্কশপ শিক্ষার্থীদের জন্য সংক্ষিপ্ত রাখুন)।

### জাভাস্ক্রিপ্ট
- ES মডিউল সিনট্যাক্স: `import ... from "..."`.
- `"openai"` থেকে `OpenAI` এবং `"foundry-local-sdk"` থেকে `FoundryLocalManager` ব্যবহার করুন।
- SDK ইনিট প্যাটার্ন: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- স্ট্রিমিং: `for await (const chunk of stream)`.
- টপ-লেভেল `await` সারাবর্ণে ব্যবহৃত হয়।

### C#
- Nullable সক্ষম, ইমপ্লিসিট usings, .NET 9।
- SDK-পরিচালিত লাইফসাইকেলের জন্য `FoundryLocalManager.StartServiceAsync()` ব্যবহার করুন।
- স্ট্রিমিং: `CompleteChatStreaming()` এবং `foreach (var update in completionUpdates)`।
- প্রধান `csharp/Program.cs` একটি CLI রাউটার, যা স্ট্যাটিক `RunAsync()` মেথডে ডিসপ্যাচ করে।

### টুল কলিং
- শুধুমাত্র নির্দিষ্ট মডেলগুলো টুল কলিং সাপোর্ট করে: **Qwen 2.5** পরিবার (`qwen2.5-*`) এবং **Phi-4-mini** (`phi-4-mini`)।
- টুল স্কিমাগুলো OpenAI ফাংশন-কলিং JSON ফরম্যাট অনুসরণ করে (`type: "function"`, `function.name`, `function.description`, `function.parameters`)।
- কথোপকথন একটি মাল্টি-টার্ন প্যাটার্ন অনুসরণ করে: ব্যবহারকারী → অ্যাসিস্ট্যান্ট (tool_calls) → টুল (results) → অ্যাসিস্ট্যান্ট (চূড়ান্ত উত্তর)।
- টুল ফলাফল বার্তায় `tool_call_id` অবশ্যই মডেলের টুল কল থেকে প্রাপ্ত `id` এর সাথে মিলে যেতে হবে।
- পাইথন সরাসরি OpenAI SDK ব্যবহার করে; জেএস SDK এর নেটিভ `ChatClient` (`model.createChatClient()`) ব্যবহার করে; সি# OpenAI SDK এর `ChatTool.CreateFunctionTool()` ব্যবহার করে।

### ChatClient (ন্যাটিভ SDK ক্লায়েন্ট)
- জাভাস্ক্রিপ্ট: `model.createChatClient()` একটি `ChatClient` রিটার্ন করে যার মধ্যে `completeChat(messages, tools?)` এবং `completeStreamingChat(messages, callback)` থাকে।
- সি#: `model.GetChatClientAsync()` একটি স্ট্যান্ডার্ড `ChatClient` রিটার্ন করে যা OpenAI NuGet প্যাকেজ ছাড়াই ব্যবহার করা যায়।
- পাইথনের নেটিভ ChatClient নেই — OpenAI SDK ব্যবহার করুন `manager.endpoint` এবং `manager.api_key` দিয়ে।
- **গুরুত্বপূর্ণ:** জাভাস্ক্রিপ্টের `completeStreamingChat` **কলব্যাক প্যাটার্ন** ব্যবহার করে, অ্যাসিঙ্ক ইটারেশন নয়।

### রিজনিং মডেল
- `phi-4-mini-reasoning` তার চিন্তাভাবনাকে `<think>...</think>` ট্যাগে মোড়ালো চূড়ান্ত উত্তরের আগে।
- প্রয়োজন হলে ট্যাগগুলি পার্স করে রিজনিং এবং উত্তর আলাদা করুন।

## ল্যাব গাইড

ল্যাব ফাইলগুলি `labs/` এ Markdown আকারে। এদের একটি নির্দিষ্ট কাঠামো রয়েছে:
- লোগো হেডার ইমেজ
- শিরোনাম ও লক্ষ্য কলআউট
- ওভারভিউ, শেখার উদ্দেশ্য, পূর্বশর্ত
- ধারণা ব্যাখ্যা বিভাগগুলি ডায়াগ্রামের সাথে
- নম্বর্ড এক্সারসাইজ কোড ব্লক এবং প্রত্যাশিত আউটপুট সহ
- সারাংশ টেবিল, মূল শিক্ষা, আরও পড়া
- পরবর্তী অংশের নেভিগেশন লিঙ্ক

ল্যাব কন্টেন্ট সম্পাদনার সময়:
- বিদ্যমান Markdown ফরম্যাটিং স্টাইল এবং বিভাগীয় স্তর বজায় রাখুন।
- কোড ব্লকগুলি ভাষা উল্লেখ করবে (`python`, `javascript`, `csharp`, `bash`, `powershell`)।
- যেখানেই OS ভিন্নতা প্রযোজ্য সেখানে bash ও PowerShell উভয় ভার্সন দিন।
- `> **Note:**`, `> **Tip:**`, এবং `> **Troubleshooting:**` কলআউট স্টাইল ব্যবহার করুন।
- টেবিলগুলো `| হেডার | হেডার |` পাইপ ফরম্যাটে হবে।

## বিল্ড ও টেস্ট কমান্ডসমূহ

| কার্যক্রম | কমান্ড |
|--------|---------|
| **পাইথন নমুনা** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS নমুনা** | `cd javascript && npm install && node <script>.mjs` |
| **সি# নমুনা** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava পাইথন** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (ওয়েব)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (ওয়েব)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **ডায়াগ্রাম তৈরি করুন** | `npx mmdc -i <input>.mmd -o <output>.svg` (রুটে `npm install` প্রয়োজন) |

## বাহ্যিক নির্ভরশীলতা

- **Foundry Local CLI** ডেভেলপার মেশিনে ইন্সটল থাকা আবশ্যক (`winget install Microsoft.FoundryLocal` বা `brew install foundrylocal`)।
- **Foundry Local সার্ভিস** লোকালি চলে এবং ওপেনএআই-অনুকূল REST API উন্মুক্ত করে একটি গতিশীল পোর্টে।
- কোন ক্লাউড সেবা, API কী, বা Azure সাবস্ক্রিপশন প্রয়োজন নেই যেকোনো নমুনা চালানোর জন্য।
- পার্ট ১০ (কাস্টম মডেল) থেমে `onnxruntime-genai` প্রয়োজন এবং Hugging Face থেকে মডেল ওজন ডাউনলোড করে।

## ফাইল যেগুলো কমিট করা উচিত নয়

`.gitignore` (অধিকাংশ ক্ষেত্রে) বাদ দেয়:
- `.venv/` — পাইথন ভার্চুয়াল এনভায়রনমেন্ট
- `node_modules/` — npm নির্ভরশীলতা
- `models/` — কম্পাইল্ড ONNX মডেল আউটপুট (বড় বাইনারি ফাইল, পার্ট ১০ দ্বারা তৈরি)
- `cache_dir/` — Hugging Face মডেল ডাউনলোড ক্যাশ
- `.olive-cache/` — Microsoft Olive ওয়ার্কিং ডিরেক্টরি
- `samples/audio/*.wav` — উৎপন্ন অডিও নমুনা (পুনরায় তৈরি হয় `python samples/audio/generate_samples.py` দ্বারা)
- স্ট্যান্ডার্ড পাইথন বিল্ড আর্টিফ্যাক্টস (`__pycache__/`, `*.egg-info/`, `dist/` ইত্যাদি)

## লাইসেন্স

MIT — দেখুন `LICENSE`।