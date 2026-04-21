<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local ورکشاپ - ڈیوائس پر AI ایپس بنائیں

ایک عملی ورکشاپ جس میں آپ اپنی مشین پر لینگویج ماڈلز چلانا اور [Foundry Local](https://foundrylocal.ai) اور [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) کے ساتھ ذہین ایپلیکیشنز بنانا سیکھیں گے۔

> **Foundry Local کیا ہے؟** Foundry Local ایک ہلکا پھلکا رن ٹائم ہے جو آپ کو زبان کے ماڈلز مکمل طور پر اپنے ہارڈویئر پر ڈاؤن لوڈ، مینیج اور سرو کرنے دیتا ہے۔ یہ ایک **OpenAI-مطابق API** فراہم کرتا ہے جس سے کوئی بھی ٹول یا SDK جو OpenAI کی بات کرتا ہو جڑ سکتا ہے - کسی کلاؤڈ اکاؤنٹ کی ضرورت نہیں۔

---

## سیکھنے کے مقاصد

اس ورکشاپ کے اختتام تک آپ کر سکیں گے:

| # | مقصد |
|---|-----------|
| 1 | Foundry Local انسٹال کریں اور CLI سے ماڈلز کا انتظام کریں |
| 2 | پروگراماتی ماڈل مینجمنٹ کے لیے Foundry Local SDK API میں مہارت حاصل کریں |
| 3 | Python، JavaScript، اور C# SDKs کے ذریعے لوکل انفرنس سرور سے رابطہ قائم کریں |
| 4 | Retrieval-Augmented Generation (RAG) پائپ لائن بنائیں جو جوابات کو آپ کے اپنے ڈیٹا سے منسلک کرے |
| 5 | مستقل ہدایات اور شخصیات کے ساتھ AI ایجنٹس بنائیں |
| 6 | فیڈبیک لوپس کے ساتھ ملٹی-ایجنٹ ورک فلو کو منظم کریں |
| 7 | ایک پروڈکشن کیپ اسٹون ایپ - Zava Creative Writer دریافت کریں |
| 8 | سنہری ڈیٹاسیٹس اور LLM-as-judge اسکورنگ کے ساتھ ایویلیوایشن فریم ورک بنائیں |
| 9 | Foundry Local SDK کے ذریعے Whisper کا استعمال کرتے ہوئے آڈیو ٹرانسکرائب کریں - ڈیوائس پر اسپِیچ ٹو ٹیکسٹ |
| 10 | ONNX Runtime GenAI اور Foundry Local کے ساتھ کسٹم یا Hugging Face ماڈلز کمپائل اور چلائیں |
| 11 | ٹول کالنگ پیٹرن کے ذریعے لوکل ماڈلز کو بیرونی فنکشنز کال کرنے کے قابل بنائیں |
| 12 | حقیقی وقت اسٹریمنگ کے ساتھ Zava Creative Writer کے لیے براؤزر بیسڈ UI بنائیں |

---

## ضروریات

| ضرورت | تفصیلات |
|-------------|---------|
| **ہارڈویئر** | کم از کم 8 GB RAM (16 GB سفارش کی جاتی ہے)؛ AVX2-قابل CPU یا سپورٹڈ GPU |
| **OS** | Windows 10/11 (x64/ARM)، Windows Server 2025، یا macOS 13+ |
| **Foundry Local CLI** | Windows پر `winget install Microsoft.FoundryLocal` یا macOS پر `brew tap microsoft/foundrylocal && brew install foundrylocal` کے ذریعے انسٹال کریں۔ تفصیلات کے لیے [شروع کرنے کی رہنمائی](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) دیکھیں۔ |
| **لینگویج رن ٹائم** | **Python 3.9+** اور/یا **.NET 9.0+** اور/یا **Node.js 18+** |
| **Git** | اس ریپوزیٹری کو کلون کرنے کے لیے |

---

## شروع کریں

```bash
# 1۔ مخزن کو کلون کریں
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2۔ تصدیق کریں کہ Foundry Local انسٹال ہے
foundry model list              # دستیاب ماڈلز کی فہرست بنائیں
foundry model run phi-3.5-mini  # ایک تعاملی چیٹ شروع کریں

# 3۔ اپنی زبان کا ٹریک منتخب کریں (مکمل سیٹ اپ کے لیے پارٹ 2 لیب دیکھیں)
```

| زبان | جلد آغاز |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ورکشاپ کے حصے

### حصہ 1: Foundry Local کے ساتھ آغاز

**لیب گائیڈ:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local کیا ہے اور یہ کیسے کام کرتا ہے
- Windows اور macOS پر CLI کی تنصیب
- ماڈلز کی تلاش - فہرست بنانا، ڈاؤن لوڈ کرنا، چلانا
- ماڈل عرفیات اور ڈائنامک پورٹس کو سمجھنا

---

### حصہ 2: Foundry Local SDK کی گہرائی میں جائزہ

**لیب گائیڈ:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- ایپلیکیشن ڈیولپمنٹ کے لیے CLI کے بجائے SDK کیوں استعمال کریں
- Python، JavaScript، اور C# کے لیے مکمل SDK API حوالہ
- سروس مینجمنٹ، کیٹلاگ براؤزنگ، ماڈل کا لائف سائیکل (ڈاؤن لوڈ، لوڈ، انلوڈ)
- جلد شروع کرنے کے نمونے: Python کنسٹرکٹر بُوٹ اسٹر‍یپ، JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` میٹا ڈیٹا، عرفیات، اور ہارڈویئر-مناسب ماڈل کا انتخاب

---

### حصہ 3: SDKs اور APIs

**لیب گائیڈ:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python، JavaScript، اور C# سے Foundry Local سے رابطہ
- Foundry Local SDK کا استعمال کرتے ہوئے سروس کو پروگراماتی طور پر مینیج کرنا
- OpenAI-مطابق API کے ذریعے اسٹریمنگ چیٹ کمپلیشنز
- ہر زبان کے لیے SDK میتھڈ ریفرنس

**کوڈ مثالیں:**

| زبان | فائل | تفصیل |
|----------|------|-------------|
| Python | `python/foundry-local.py` | بنیادی اسٹریمنگ چیٹ |
| C# | `csharp/BasicChat.cs` | .NET کے ساتھ اسٹریمنگ چیٹ |
| JavaScript | `javascript/foundry-local.mjs` | Node.js کے ساتھ اسٹریمنگ چیٹ |

---

### حصہ 4: Retrieval-Augmented Generation (RAG)

**لیب گائیڈ:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG کیا ہے اور کیوں اہم ہے
- ان میموری نالج بیس بنانا
- کی ورڈ-اوورلیپ ریٹریول کے ساتھ اسکورنگ
- گراؤنڈڈ سسٹم پرامپٹس کی ترکیب
- ڈیوائس پر مکمل RAG پائپ لائن چلانا

**کوڈ مثالیں:**

| زبان | فائل |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### حصہ 5: AI ایجنٹس کی تعمیر

**لیب گائیڈ:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI ایجنٹ کیا ہے (ایک خام LLM کال کے مقابلے میں)
- `ChatAgent` پیٹرن اور Microsoft Agent Framework
- سسٹم ہدایات، شخصیات، اور کثیر نکاتی مکالمے
- ایجنٹس سے منظم آؤٹ پٹ (JSON)

**کوڈ مثالیں:**

| زبان | فائل | تفصیل |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework کے ساتھ واحد ایجنٹ |
| C# | `csharp/SingleAgent.cs` | واحد ایجنٹ (ChatAgent پیٹرن) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | واحد ایجنٹ (ChatAgent پیٹرن) |

---

### حصہ 6: ملٹی-ایجنٹ ورک فلوز

**لیب گائیڈ:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- ملٹی ایجنٹ پائپ لائنز: ریسرچر → رائٹر → ایڈیٹر
- ترتیبی تنظیم اور فیڈبیک لوپس
- مشترکہ کنفیگریشن اور منظم ہینڈ آفز
- اپنا ملٹی-ایجنٹ ورک فلو ڈیزائن کرنا

**کوڈ مثالیں:**

| زبان | فائل | تفصیل |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | تین ایجنٹ پائپ لائن |
| C# | `csharp/MultiAgent.cs` | تین ایجنٹ پائپ لائن |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | تین ایجنٹ پائپ لائن |

---

### حصہ 7: Zava Creative Writer - کیپ اسٹون ایپلیکیشن

**لیب گائیڈ:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 خاص ایجنٹوں کے ساتھ پروڈکشن طرز کی ملٹی-ایجنٹ ایپ
- ترتیبی پائپ لائن اور ایویلیویٹر کی طرف سے چلائے جانے والے فیڈبیک لوپس
- اسٹریمنگ آؤٹ پٹ، پروڈکٹ کیٹلاگ تلاش، منظم JSON ہینڈ آفز
- Python (FastAPI)، JavaScript (Node.js CLI)، اور C# (.NET کنسول) میں مکمل نفاذ

**کوڈ مثالیں:**

| زبان | ڈائریکٹری | تفصیل |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI ویب سروس اور آرکیسٹریٹر |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI ایپلیکیشن |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 کنسول ایپلیکیشن |

---

### حصہ 8: ایویلیویشن-مبنی ترقی

**لیب گائیڈ:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- سنہری ڈیٹاسیٹس کے ساتھ AI ایجنٹس کے لیے منظم ایویلیویشن فریم ورک بنائیں
- اصول پر مبنی چیکس (لمبائی، کی ورڈ کوریج، ممنوعہ الفاظ) + LLM-as-judge اسکورنگ
- پرامپٹ ویرینٹس کی سائڈ بائی سائڈ موازنہ اور مجموعی اسکورکارڈز
- حصہ 7 کے Zava Editor ایجنٹ پیٹرن کو آف لائن ٹیسٹ سوئٹ میں توسیع دینا
- Python، JavaScript، اور C# کے ٹریکس

**کوڈ مثالیں:**

| زبان | فائل | تفصیل |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | ایویلیویشن فریم ورک |
| C# | `csharp/AgentEvaluation.cs` | ایویلیویشن فریم ورک |
| JavaScript | `javascript/foundry-local-eval.mjs` | ایویلیویشن فریم ورک |

---

### حصہ 9: Whisper کے ساتھ وائس ٹرانسکرپشن

**لیب گائیڈ:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- OpenAI Whisper کا استعمال کرتے ہوئے مقامی طور پر چلنے والا سپیچ ٹو ٹیکسٹ ٹرانسکرپشن
- پرائیویسی کو اولین ترجیح دیتے ہوئے آڈیو پروسیسنگ - آڈیو کبھی بھی آپ کے ڈیوائس سے باہر نہیں جاتا
- Python، JavaScript، اور C# ٹریکس، `client.audio.transcriptions.create()` (Python/JS) اور `AudioClient.TranscribeAudioAsync()` (C#)
- عملی مشق کے لیے Zava تھیم والے سیمپل آڈیو فائلز شامل

**کوڈ مثالیں:**

| زبان | فائل | تفصیل |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper وائس ٹرانسکرپشن |
| C# | `csharp/WhisperTranscription.cs` | Whisper وائس ٹرانسکرپشن |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper وائس ٹرانسکرپشن |

> **نوٹ:** یہ لیب **Foundry Local SDK** کا استعمال کرتی ہے تاکہ پروگراماتی طور پر Whisper ماڈل ڈاؤن لوڈ اور لوڈ کیا جا سکے، پھر ٹرانسکرپشن کے لیے آڈیو کو لوکل OpenAI-مطابق اینڈپوائنٹ پر بھیجتی ہے۔ Whisper ماڈل (`whisper`) Foundry Local کیٹلاگ میں شامل ہے اور مکمل طور پر ڈیوائس پر چلتا ہے - کسی کلاؤڈ API کیز یا نیٹ ورک رسائی کی ضرورت نہیں۔

---

### حصہ 10: کسٹم یا Hugging Face ماڈلز کا استعمال

**لیب گائیڈ:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ONNX Runtime GenAI ماڈل بلڈر کے ذریعے Hugging Face ماڈلز کو بہتر ONNX فارمیٹ میں کمپائل کرنا
- ہارڈویئر مخصوص کمپائلیشن (CPU, NVIDIA GPU, DirectML, WebGPU) اور کوانٹائزیشن (int4، fp16، bf16)
- Foundry Local کے لیے چیٹ ٹیمپلیٹ کنفیگریشن فائلیں بنانا
- کمپائل کردہ ماڈلز کو Foundry Local کیشے میں شامل کرنا
- CLI، REST API، اور OpenAI SDK کے ذریعے کسٹم ماڈلز چلانا
- حوالہ مثال: Qwen/Qwen3-0.6B کا مکمل کمپائلیشن

---

### حصہ 11: لوکل ماڈلز کے ساتھ ٹول کالنگ

**لیب گائیڈ:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- لوکل ماڈلز کو بیرونی فنکشنز کال کرنے کے قابل بنائیں (ٹول/فنکشن کالنگ)
- OpenAI فنکشن کالنگ فارمیٹ استعمال کرتے ہوئے ٹول اسکیمز کی تعریف کریں
- کثیر نکاتی ٹول کالنگ مکالمے کے بہاؤ کو سنبھالیں
- لوکل طور پر ٹول کالز انجام دیں اور ماڈل کو نتائج واپس کریں
- ٹول کالنگ حالات کے لیے مناسب ماڈل کا انتخاب کریں (Qwen 2.5, Phi-4-mini)
- ٹول کالنگ کے لیے SDK کا مقامی `ChatClient` استعمال کریں (JavaScript)

**کوڈ مثالیں:**

| زبان | فائل | تفصیل |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | موسم/آبادی کے ٹولز کے ساتھ ٹول کالنگ |
| C# | `csharp/ToolCalling.cs` | .NET کے ساتھ ٹول کالنگ |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient کے ساتھ ٹول کالنگ |

---

### حصہ 12: Zava Creative Writer کے لیے ویب UI کی تعمیر

**لیب گائیڈ:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer کے لیے براؤزر بیسڈ فرنٹ اینڈ شامل کریں
- Python (FastAPI)، JavaScript (Node.js HTTP)، اور C# (ASP.NET Core) سے مشترکہ UI کو سرو کریں
- Fetch API اور ReadableStream کے ساتھ براؤزر میں اسٹریمنگ NDJSON کو استعمال کریں
- لائیو ایجنٹ کی حالت کے بیجز اور حقیقی وقت میں آرٹیکل ٹیکسٹ اسٹریمنگ

**کوڈ (مشترکہ UI):**

| فائل | تفصیل |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | صفحہ کا خاکہ |
| `zava-creative-writer-local/ui/style.css` | اسٹائلنگ |
| `zava-creative-writer-local/ui/app.js` | اسٹریمنگ ریڈر اور DOM اپ ڈیٹ لاجک |

**بیک اینڈ میں اضافے:**

| زبان | فائل | تفصیل |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | اسٹاٹک UI کو سرو کرنے کے لیے اپڈیٹ کیا گیا |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | آرکیسٹریٹر کو لپیٹتے ہوئے نیا HTTP سرور |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | نیا ASP.NET Core منیمال API پروجیکٹ |

---

### حصہ 13: ورکشاپ مکمل
**لیب گائیڈ:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- آپ نے جو کچھ تمام 12 حصوں میں بنایا ہے اس کا خلاصہ
- آپ کی ایپلی کیشنز کو بڑھانے کے مزید خیالات
- وسائل اور دستاویزات کے لنکس

---

## پروجیکٹ کی ساخت

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

## وسائل

| وسیلہ | لنک |
|----------|------|
| فاؤنڈری لوکل ویب سائٹ | [foundrylocal.ai](https://foundrylocal.ai) |
| ماڈل کیٹلاگ | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| فاؤنڈری لوکل گٹ ہب | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| شروع کرنے کی گائیڈ | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| فاؤنڈری لوکل SDK حوالہ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| مائیکروسافٹ ایجنٹ فریم ورک | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| اوپن اے آئی وسپر | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX رن ٹائم GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## لائسنس

یہ ورکشاپ مواد تعلیمی مقاصد کے لیے فراہم کیا گیا ہے۔

---

**خوش رہیں اور بناتے رہیں! 🚀**