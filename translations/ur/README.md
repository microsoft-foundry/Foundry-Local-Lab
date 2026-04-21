<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# فاؤنڈری لوکل ورکشاپ - آن ڈیوائس AI ایپس بنائیں

اپنے مشین پر زبان کے ماڈلز چلانے اور [Foundry Local](https://foundrylocal.ai) اور [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) کے ساتھ ذہین ایپلیکیشنز بنانے کے لیے ایک عملی ورکشاپ۔

> **Foundry Local کیا ہے؟** فاؤنڈری لوکل ایک ہلکا پھلکا رن ٹائم ہے جو آپ کو اپنے ہارڈویئر پر زبان کے ماڈلز مکمل طور پر ڈاؤن لوڈ، منظم اور پیش کرنے دیتا ہے۔ یہ ایک **OpenAI-مطابق API** فراہم کرتا ہے تاکہ کوئی بھی ٹول یا SDK جو OpenAI کے ساتھ بات کرتا ہے، کنیکٹ کر سکے - کوئی کلاؤڈ اکاؤنٹ ضروری نہیں۔

### 🌐 کثیراللسانی حمایت

#### GitHub ایکشن کے ذریعے معاونت حاصل (خودکار اور ہمیشہ تازہ ترین)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](./README.md) | [Vietnamese](../vi/README.md)

> **مقامی طور پر کلون کرنا پسند کریں؟**
>
> اس ذخیرے میں 50+ زبانوں کے ترجمے شامل ہیں جو ڈاؤن لوڈ سائز کو نمایاں طور پر بڑھاتے ہیں۔ ترجمے کے بغیر کلون کرنے کے لئے sparse checkout استعمال کریں:
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
> یہ آپ کو تیز تر ڈاؤن لوڈ کے ساتھ کورس مکمل کرنے کے لیے ہر چیز فراہم کرتا ہے۔
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## سیکھنے کے مقاصد

اس ورکشاپ کے اختتام تک آپ قابل ہوں گے:

| # | مقصد |
|---|-----------|
| 1 | Foundry Local انسٹال کریں اور CLI کے ذریعے ماڈلز کا انتظام کریں |
| 2 | پروگراماتی ماڈل مینجمنٹ کے لیے Foundry Local SDK API میں مہارت حاصل کریں |
| 3 | Python، JavaScript، اور C# SDKs کے ذریعے لوکل انفرنس سرور سے کنیکٹ کریں |
| 4 | Retrieval-Augmented Generation (RAG) پائپ لائن بنائیں جو جوابوں کی بنیاد آپ کے اپنے ڈیٹا پر رکھتی ہو |
| 5 | مستقل ہدایات اور شخصیات کے ساتھ AI ایجنٹ بنائیں |
| 6 | فیڈبیک لوپس کے ساتھ ملٹی ایجنٹ ورک فلو کی ترتیب دیں |
| 7 | پروڈکشن کیپ اسٹون ایپ - Zava Creative Writer کو دریافت کریں |
| 8 | گولڈن ڈیٹا سیٹس اور LLM-as-judge اسکورنگ کے ساتھ ایویلیوایشن فریم ورکس بنائیں |
| 9 | Whisper کے ذریعے آڈیو کی ٹرانسکرپشن کریں - فاؤنڈری لوکل SDK کے تحت آن ڈیوائس اسپیچ ٹو ٹیکسٹ |
| 10 | ONNX Runtime GenAI اور Foundry Local کے ساتھ کسٹم یا Hugging Face ماڈلز کو کمپائل اور چلائیں |
| 11 | ٹول-کالنگ پیٹرن کے ساتھ لوکل ماڈلز کو خارجی فنکشنز کال کرنے کے قابل بنائیں |
| 12 | Zava Creative Writer کے لیے براوزر-بیسڈ UI بنائیں جس میں ریئل ٹائم سٹریمنگ ہو |

---

## پیشگی ضروریات

| ضرورت | تفصیلات |
|-------------|---------|
| **ہارڈویئر** | کم از کم 8 جی بی RAM (16 جی بی تجویز کردہ)؛ AVX2-قابل CPU یا حمایت یافتہ GPU |
| **آپریٹنگ سسٹم** | Windows 10/11 (x64/ARM)، Windows Server 2025، یا macOS 13+ |
| **Foundry Local CLI** | Windows پر `winget install Microsoft.FoundryLocal` یا macOS پر `brew tap microsoft/foundrylocal && brew install foundrylocal` کے ذریعے انسٹال کریں۔ تفصیلات کے لیے [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) دیکھیں۔ |
| **زبان رن ٹائم** | **Python 3.9+** اور/یا **.NET 9.0+** اور/یا **Node.js 18+** |
| **Git** | اس ذخیرے کو کلون کرنے کے لیے |

---

## شروع کریں

```bash
# 1. مخزن کی نقل بنائیں
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. یہ تصدیق کریں کہ Foundry Local انسٹال ہے
foundry model list              # دستیاب ماڈلز کی فہرست بنائیں
foundry model run phi-3.5-mini  # ایک تعاملی چیٹ شروع کریں

# 3. اپنی زبان کا ٹریک منتخب کریں (مکمل ترتیبات کے لیے حصہ 2 کی لیب دیکھیں)
```

| زبان | جلد آغاز |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ورکشاپ کے حصے

### حصہ 1: Foundry Local کے ساتھ شروع کریں

**لیب گائیڈ:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local کیا ہے اور یہ کیسے کام کرتا ہے
- Windows اور macOS پر CLI انسٹال کرنا
- ماڈلز کا جائزہ لینا - فہرست، ڈاؤن لوڈ، چلانا
- ماڈل علیاسز اور ڈائنامک پورٹس کو سمجھنا

---

### حصہ 2: Foundry Local SDK کی تفصیلی جانچ

**لیب گائیڈ:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- ایپلیکیشن ڈیولپمنٹ کے لیے CLI کی بجائے SDK کیوں استعمال کریں
- Python، JavaScript، اور C# کے لیے مکمل SDK API ریفرنس
- سروس مینجمنٹ، کیٹلاگ براؤزنگ، ماڈل لائف سائیکل (ڈاؤن لوڈ، لوڈ، ان لوڈ)
- جلد آغاز کے پیٹرنز: Python کنسٹرکٹر بوٹ سٹراپ، JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` میٹا ڈیٹا، علیاسز، اور ہارڈویئر-مثالی ماڈل کا انتخاب

---

### حصہ 3: SDKs اور APIs

**لیب گائیڈ:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python، JavaScript، اور C# سے Foundry Local سے کنیکٹ ہونا
- Foundry Local SDK کے ذریعے پروگراماتی طور پر سروس کا انتظام کرنا
- OpenAI-مطابق API کے ذریعے سٹریمنگ چیٹ کمپلیشنز
- ہر زبان کے لیے SDK میتھڈ ریفرنس

**کوڈ نمونے:**

| زبان | فائل | وضاحت |
|----------|------|-------------|
| Python | `python/foundry-local.py` | بنیادی سٹریمنگ چیٹ |
| C# | `csharp/BasicChat.cs` | .NET کے ساتھ سٹریمنگ چیٹ |
| JavaScript | `javascript/foundry-local.mjs` | Node.js کے ساتھ سٹریمنگ چیٹ |

---

### حصہ 4: Retrieval-Augmented Generation (RAG)

**لیب گائیڈ:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG کیا ہے اور کیوں اہم ہے
- ان-میموری نالج بیس بنانا
- اسکورنگ کے ساتھ کی ورڈ-اوورلیپ رٹریوال
- گراؤنڈڈ سسٹم پرامپٹس مرتب کرنا
- آن ڈیوائس مکمل RAG پائپ لائن چلانا

**کوڈ نمونے:**

| زبان | فائل |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### حصہ 5: AI ایجنٹس بنانا

**لیب گائیڈ:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI ایجنٹ کیا ہے (رو ای ایل ایل ایم کال کے مقابلے میں)
- `ChatAgent` پیٹرن اور Microsoft Agent Framework
- سسٹم ہدایات، شخصیات، اور ملٹی ٹرن گفتگو
- ایجنٹس سے منظم شدہ آؤٹ پٹ (JSON)

**کوڈ نمونے:**

| زبان | فائل | وضاحت |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | ایجنٹ فریم ورک کے ساتھ ایک ایجنٹ |
| C# | `csharp/SingleAgent.cs` | ایک ایجنٹ (ChatAgent پیٹرن) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | ایک ایجنٹ (ChatAgent پیٹرن) |

---

### حصہ 6: ملٹی ایجنٹ ورک فلو

**لیب گائیڈ:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- ملٹی ایجنٹ پائپ لائنز: ریسرچر → رائٹر → ایڈیٹر
- تسلسلی رابطہ کاری اور فیڈبیک لوپس
- مشترکہ کنفیگریشن اور منظم ہینڈ آف
- اپنا ملٹی ایجنٹ ورک فلو ڈیزائن کریں

**کوڈ نمونے:**

| زبان | فائل | وضاحت |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | تین ایجنٹس کی پائپ لائن |
| C# | `csharp/MultiAgent.cs` | تین ایجنٹس کی پائپ لائن |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | تین ایجنٹس کی پائپ لائن |

---

### حصہ 7: Zava Creative Writer - کیپ اسٹون ایپلیکیشن

**لیب گائیڈ:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- چار ماہر ایجنٹس کے ساتھ پروڈکشن طرز کی ملٹی ایجنٹ ایپ
- تسلسلی پائپ لائن جس میں ایویلیوایٹر سے باہر آنے والے فیڈبیک لوپس شامل ہیں
- سٹریمنگ آؤٹ پٹ، پروڈکٹ کیٹلاگ سرچ، ساختہ JSON ہینڈ آف
- Python (FastAPI)، JavaScript (Node.js CLI)، اور C# (.NET کنسول) میں مکمل نفاذ

**کوڈ نمونے:**

| زبان | ڈائریکٹری | وضاحت |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI ویب سروس کے ساتھ آرکیسٹریٹر |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI ایپلیکیشن |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 کنسول ایپلیکیشن |

---

### حصہ 8: ایویلیوایشن-لیڈ ڈیولپمنٹ

**لیب گائیڈ:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- گولڈن ڈیٹا سیٹس کے ذریعے AI ایجنٹس کے لیے ایک منظم جائزہ فریم ورک بنائیں
- قاعدہ بنیاد پر چیکس (لمبائی، کی ورڈ کورج، ممنوعہ الفاظ) + LLM-as-judge اسکورنگ
- پرامپٹ کی قسموں کا ایک ساتھ موازنہ اور مجموعی اسکورکارڈز
- حصہ 7 کے Zava Editor ایجنٹ پیٹرن کو آف لائن ٹیسٹ سوئٹ میں توسیع دیتا ہے
- Python، JavaScript، اور C# کے تراکز

**کوڈ نمونے:**

| زبان | فائل | وضاحت |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | ایویلیوایشن فریم ورک |
| C# | `csharp/AgentEvaluation.cs` | ایویلیوایشن فریم ورک |
| JavaScript | `javascript/foundry-local-eval.mjs` | ایویلیوایشن فریم ورک |

---

### حصہ 9: Whisper کے ساتھ وائس ٹرانسکرپشن

**لیب گائیڈ:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- OpenAI Whisper کو مقامی طور پر چلانے والی تقریر سے متن میں منتقلی
- پرائیویسی کو اولین ترجیح دینے والی آڈیو پراسیسنگ - آڈیو کبھی بھی آپ کے آلے کو نہیں چھوڑتی
- Python، JavaScript، اور C# ٹریکس کے لیے `client.audio.transcriptions.create()` (Python/JS) اور `AudioClient.TranscribeAudioAsync()` (C#)
- ہاتھوں سے مشق کے لیے Zava تھیم والے نمونہ آڈیو فائلز شامل ہیں

**کوڈ کے نمونے:**

| زبان | فائل | وضاحت |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper آواز کی تحریر بندی |
| C# | `csharp/WhisperTranscription.cs` | Whisper آواز کی تحریر بندی |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper آواز کی تحریر بندی |

> **نوٹ:** یہ لیب **Foundry Local SDK** استعمال کرتی ہے تاکہ پروگرام کے ذریعے Whisper ماڈل ڈاؤن لوڈ اور لوڈ کیا جائے، پھر آڈیو کو تحریر کے لیے مقامی OpenAI-مطابق اینڈپوائنٹ پر بھیجا جائے۔ Whisper ماڈل (`whisper`) Foundry Local کیٹلاگ میں شامل ہے اور مکمل طور پر آلہ پر چلتا ہے - کسی کلاؤڈ API کیز یا نیٹ ورک کے بغیر۔

---

### حصہ 10: کسٹم یا Hugging Face ماڈلز کا استعمال

**لیب گائیڈ:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face ماڈلز کو ONNX رن ٹائم GenAI ماڈل بلڈر کی مدد سے بہتر ONNX فارمیٹ میں مرتب کرنا
- مخصوص ہارڈویئر کے لیے کمپائلیشن (CPU، NVIDIA GPU، DirectML، WebGPU) اور کوانٹائزیشن (int4, fp16, bf16)
- Foundry Local کے لیے چیٹ-ٹیمپلیٹ کنفیگریشن فائلز تیار کرنا
- کمپائل شدہ ماڈلز کو Foundry Local کیش میں شامل کرنا
- CLI، REST API، اور OpenAI SDK کے ذریعے کسٹم ماڈلز چلانا
- حوالہ کے طور پر: Qwen/Qwen3-0.6B کو ابتدا سے انتہا تک مرتب کرنا

---

### حصہ 11: مقامی ماڈلز کے ذریعے ٹول کالنگ

**لیب گائیڈ:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- مقامی ماڈلز کو بیرونی فنکشن کالز کی اجازت دینا (ٹول/فنکشن کالنگ)
- ٹول سکیموں کی تعریف OpenAI فنکشن-کالنگ فارمیٹ کے مطابق کرنا
- کثیر دورانیہ ٹول کالنگ مکالمے کا انتظام کرنا
- ٹول کالز کو مقامی طور پر چلانا اور ماڈل کو نتائج واپس بھیجنا
- ٹول کالنگ کے منظرناموں کے لیے مناسب ماڈل کا انتخاب (Qwen 2.5, Phi-4-mini)
- SDK کی مقامی `ChatClient` کو استعمال کرتے ہوئے ٹول کالنگ کرنا (JavaScript)

**کوڈ کے نمونے:**

| زبان | فائل | وضاحت |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | موسمی/آبادی کے ٹولز کے ساتھ ٹول کالنگ |
| C# | `csharp/ToolCalling.cs` | .NET کے ساتھ ٹول کالنگ |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient کے ساتھ ٹول کالنگ |

---

### حصہ 12: Zava تخلیقی رائٹر کے لیے ویب UI بنانا

**لیب گائیڈ:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava تخلیقی رائٹر کے لیے براؤزر پر مبنی فرنٹ اینڈ شامل کرنا
- Python (FastAPI)، JavaScript (Node.js HTTP)، اور C# (ASP.NET Core) سے مشترکہ UI کی سروس فراہم کرنا
- براؤزر میں Fetch API اور ReadableStream کے ذریعے NDJSON کی اسٹریم استعمال کرنا
- لائیو ایجنٹ اسٹیٹس بیجز اور حقیقی وقت میں مضمون کی اسٹریمنگ

**کوڈ (مشترکہ UI):**

| فائل | وضاحت |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | صفحہ کا لے آؤٹ |
| `zava-creative-writer-local/ui/style.css` | اسٹائلنگ |
| `zava-creative-writer-local/ui/app.js` | اسٹریم ریڈر اور DOM اپڈیٹ منطق |

**بیک اینڈ میں اضافے:**

| زبان | فائل | وضاحت |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | سٹیٹک UI کی سروس کے لیے اپڈیٹ کیا گیا |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | آراستہ کار کے لیے نیا HTTP سرور |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | نیا ASP.NET Core مِنیمل API پروجیکٹ |

---

### حصہ 13: ورکشاپ مکمل

**لیب گائیڈ:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- آپ نے 12 حصوں میں جو کچھ تعمیر کیا اس کا خلاصہ
- آپ کی ایپلیکیشنز کو مزید توسیع دینے کے آئیڈیاز
- وسائل اور دستاویزات کے روابط

---

## پروجیکٹ کا ڈھانچہ

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
| Foundry Local ویب سائٹ | [foundrylocal.ai](https://foundrylocal.ai) |
| ماڈل کیٹلاگ | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| شروع کرنے کی رہنمائی | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK ریفرنس | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft ایجنٹ فریم ورک | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## لائسنس

یہ ورکشاپ مواد تعلیمی مقاصد کے لیے فراہم کیا گیا ہے۔

---

**خوشی سے تعمیر کریں! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**انہتمامی دستبرداری**:  
یہ دستاویز AI ترجمہ خدمت [Co-op Translator](https://github.com/Azure/co-op-translator) کے ذریعے ترجمہ کی گئی ہے۔ اگرچہ ہم درستگی کے لیے کوشاں ہیں، براہ کرم نوٹ کریں کہ خودکار ترجمے میں غلطیاں یا کمی بیشی ہو سکتی ہے۔ اصل دستاویز اپنی مادری زبان میں مستند اور معتبر ماخذ سمجھا جائے گا۔ اہم معلومات کے لیے پیشہ ور انسانی ترجمہ کی سفارش کی جاتی ہے۔ اس ترجمے کے استعمال سے پیدا ہونے والی کسی بھی غلط فہمی یا غلط تشریح کی وجہ سے ہم ذمہ دار نہیں ہیں۔
<!-- CO-OP TRANSLATOR DISCLAIMER END -->