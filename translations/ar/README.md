<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="فاوندري لوكال" width="280" />
</p>

# ورشة فاوندري لوكال - بناء تطبيقات الذكاء الصناعي على الجهاز

ورشة عملية لتشغيل نماذج اللغة على جهازك الخاص وبناء تطبيقات ذكية باستخدام [فاوندري لوكال](https://foundrylocal.ai) و [إطار عمل مايكروسوفت للوكيل](https://learn.microsoft.com/en-us/agent-framework/).

> **ما هو فاوندري لوكال؟** فاوندري لوكال هو بيئة تشغيل خفيفة تمكنك من تنزيل وإدارة وتقديم نماذج اللغة بالكامل على جهازك. يوفر واجهة برمجة تطبيقات متوافقة مع OpenAI بحيث يمكن لأي أداة أو SDK يدعم OpenAI الاتصال به - لا حاجة لحساب سحابي.

---

## الأهداف التعليمية

بحلول نهاية هذه الورشة ستكون قادرًا على:

| # | الهدف |
|---|-----------|
| 1 | تثبيت فاوندري لوكال وإدارة النماذج باستخدام واجهة الأوامر |
| 2 | إتقان واجهة SDK لفاوندري لوكال لإدارة النماذج برمجيًا |
| 3 | الاتصال بخادم الاستدلال المحلي باستخدام SDKات بايثون وجافاسكربت وسي شارب |
| 4 | بناء خط أنابيب توليد معزز بالاسترجاع (RAG) يؤسس الإجابات على بياناتك الخاصة |
| 5 | إنشاء وكلاء ذكاء اصطناعي مع تعليمات وشخصيات دائمة |
| 6 | تنظيم سير عمل متعدد الوكلاء مع حلقات تغذية راجعة |
| 7 | استكشاف تطبيق إنتاجي نهائي - كاتب إبداعي زافا |
| 8 | بناء أطر تقييم باستخدام مجموعات بيانات ذهبية ودرجات LLM كقاضي |
| 9 | نسخ الصوت إلى نص باستخدام Whisper - تحويل الكلام إلى نص على الجهاز باستخدام SDK فاوندري لوكال |
| 10 | تجميع وتشغيل نماذج مخصصة أو Hugging Face باستخدام ONNX Runtime GenAI وفاوندري لوكال |
| 11 | تمكين النماذج المحلية من استدعاء الدوال الخارجية بنمط استدعاء الأدوات |
| 12 | بناء واجهة مستخدم قائمة على المتصفح لزاڤا كريتيف رايتر مع بث مباشر في الوقت الحقيقي |

---

## المتطلبات المسبقة

| المتطلب | التفاصيل |
|-------------|---------|
| **الأجهزة** | ذاكرة وصول عشوائي 8 جيجابايت كحد أدنى (16 جيجابايت مفضلة)؛ معالج يدعم AVX2 أو بطاقة رسومات مدعومة |
| **نظام التشغيل** | ويندوز 10/11 (x64/ARM)، ويندوز سيرفر 2025، أو macOS 13+ |
| **واجهة فاوندري لوكال** | التثبيت عبر `winget install Microsoft.FoundryLocal` (ويندوز) أو `brew tap microsoft/foundrylocal && brew install foundrylocal` (ماك). انظر [دليل البدء](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) للمزيد. |
| **بيئة اللغة** | **بايثون 3.9+** و/أو **.NET 9.0+** و/أو **Node.js 18+** |
| **Git** | لاستنساخ هذا المستودع |

---

## البدء

```bash
# 1. استنساخ المستودع
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. التحقق من تثبيت Foundry Local
foundry model list              # قائمة النماذج المتاحة
foundry model run phi-3.5-mini  # بدء دردشة تفاعلية

# 3. اختر مسار اللغة الخاص بك (راجع الجزء 2 المختبر لإعداد كامل)
```

| اللغة | بدء سريع |
|----------|-------------|
| **بايثون** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **سي شارب** | `cd csharp && dotnet run` |
| **جافاسكربت** | `cd javascript && npm install && node foundry-local.mjs` |

---

## أجزاء الورشة

### الجزء 1: البدء مع فاوندري لوكال

**دليل المعمل:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- ما هو فاوندري لوكال وكيف يعمل
- تثبيت واجهة الأوامر على ويندوز وماك
- استكشاف النماذج - القوائم، التنزيل، التشغيل
- فهم أسماء النماذج المستعارة والمنافذ الديناميكية

---

### الجزء 2: الغوص العميق في SDK فاوندري لوكال

**دليل المعمل:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- لماذا استخدام SDK بدلاً من CLI لتطوير التطبيقات
- مرجع كامل لواجهة SDK لبايثون وجافاسكربت وسي شارب
- إدارة الخدمة، تصفح الكتالوج، دورة حياة النموذج (تنزيل، تحميل، إلغاء تحميل)
- أنماط البدء السريع: مُهيئ بايثون، `init()` لجافاسكربت، `CreateAsync()` لسي شارب
- بيانات التعريف `FoundryModelInfo`، الأسماء المستعارة، واختيار النموذج الأمثل للأجهزة

---

### الجزء 3: SDK وواجهات برمجة التطبيقات

**دليل المعمل:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- الاتصال بفاوندري لوكال من بايثون وجافاسكربت وسي شارب
- استخدام SDK لإدارة الخدمة برمجيًا
- بث إكمالات الدردشة عبر واجهة برمجة تطبيقات متوافقة مع OpenAI
- مرجع طرق SDK لكل لغة

**أمثلة برمجية:**

| اللغة | الملف | الوصف |
|----------|------|-------------|
| بايثون | `python/foundry-local.py` | دردشة بث أساسية |
| سي شارب | `csharp/BasicChat.cs` | دردشة بث مع .NET |
| جافاسكربت | `javascript/foundry-local.mjs` | دردشة بث مع Node.js |

---

### الجزء 4: التوليد المعزز بالاسترجاع (RAG)

**دليل المعمل:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- ما هو RAG ولماذا هو مهم
- بناء قاعدة معرفة في الذاكرة
- استرجاع يتقاطع كلمات مفتاحية مع تسجيل
- تكوين مطالبات نظام مؤسَّسة
- تشغيل خط RAG كامل على الجهاز

**أمثلة برمجية:**

| اللغة | الملف |
|----------|------|
| بايثون | `python/foundry-local-rag.py` |
| سي شارب | `csharp/RagPipeline.cs` |
| جافاسكربت | `javascript/foundry-local-rag.mjs` |

---

### الجزء 5: بناء وكلاء الذكاء الاصطناعي

**دليل المعمل:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- ما هو وكيل الذكاء الاصطناعي (مقارنة باستدعاء نموذج لغة خام)
- نمط `ChatAgent` وإطار عمل مايكروسوفت للوكيل
- تعليمات النظام، الشخصيات، والمحاورات متعددة الأدوار
- مخرجات منظمة (JSON) من الوكلاء

**أمثلة برمجية:**

| اللغة | الملف | الوصف |
|----------|------|-------------|
| بايثون | `python/foundry-local-with-agf.py` | وكيل واحد مع إطار العمل |
| سي شارب | `csharp/SingleAgent.cs` | وكيل واحد (نمط ChatAgent) |
| جافاسكربت | `javascript/foundry-local-with-agent.mjs` | وكيل واحد (نمط ChatAgent) |

---

### الجزء 6: سير عمل متعدد الوكلاء

**دليل المعمل:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- خطوط أنابيب متعددة الوكلاء: الباحث → الكاتب → المحرر
- تنظيم تسلسلي وحلقات تغذية راجعة
- إعدادات مشتركة وتسليمات منظمة
- تصميم سير عمل متعدد الوكلاء خاص بك

**أمثلة برمجية:**

| اللغة | الملف | الوصف |
|----------|------|-------------|
| بايثون | `python/foundry-local-multi-agent.py` | خط أنابيب بثلاثة وكلاء |
| سي شارب | `csharp/MultiAgent.cs` | خط أنابيب بثلاثة وكلاء |
| جافاسكربت | `javascript/foundry-local-multi-agent.mjs` | خط أنابيب بثلاثة وكلاء |

---

### الجزء 7: زافا كريتيف رايتر - تطبيق التتويج

**دليل المعمل:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- تطبيق متعدد الوكلاء بأسلوب الإنتاج مع 4 وكلاء متخصصين
- خط أنابيب تسلسلي مع حلقات تغذية راجعة مدفوعة بالتقييم
- بث مباشر، بحث في كتالوج المنتجات، تسليمات منظمة JSON
- تنفيذ كامل في بايثون (FastAPI)، جافاسكربت (Node.js CLI)، وسي شارب (.NET لوحي)

**أمثلة برمجية:**

| اللغة | الدليل | الوصف |
|----------|-----------|-------------|
| بايثون | `zava-creative-writer-local/src/api/` | خدمة ويب FastAPI مع منظّم |
| جافاسكربت | `zava-creative-writer-local/src/javascript/` | تطبيق CLI لـ Node.js |
| سي شارب | `zava-creative-writer-local/src/csharp/` | تطبيق لوحي .NET 9 |

---

### الجزء 8: التطوير بقيادة التقييم

**دليل المعمل:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- بناء إطار تقييم منهجي لوكلاء الذكاء الاصطناعي باستخدام مجموعات بيانات ذهبية
- فحوصات قائمة على قواعد (الطول، تغطية الكلمات المفتاحية، المصطلحات المحظورة) + درجات LLM كقاضي
- مقارنة جنباً إلى جنب بين أنواع المطالبات ببطاقات درجات مجمعة
- تمديد نمط وكيل المحرر من الجزء 7 إلى مجموعة اختبارات غير متصلة بالشبكة
- مسارات بايثون، جافاسكربت، وسي شارب

**أمثلة برمجية:**

| اللغة | الملف | الوصف |
|----------|------|-------------|
| بايثون | `python/foundry-local-eval.py` | إطار التقييم |
| سي شارب | `csharp/AgentEvaluation.cs` | إطار التقييم |
| جافاسكربت | `javascript/foundry-local-eval.mjs` | إطار التقييم |

---

### الجزء 9: نسخ الصوت باستخدام Whisper

**دليل المعمل:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- نسخ الكلام إلى نص باستخدام OpenAI Whisper يعمل محليًا
- معالجة صوتية تراعي الخصوصية - الصوت لا يغادر جهازك
- مسارات بايثون، جافاسكربت، وسي شارب مع `client.audio.transcriptions.create()` (بايثون/جافاسكربت) و `AudioClient.TranscribeAudioAsync()` (سي شارب)
- يشمل ملفات صوتية نموذجية بموضوع زافا للتدريب العملي

**أمثلة برمجية:**

| اللغة | الملف | الوصف |
|----------|------|-------------|
| بايثون | `python/foundry-local-whisper.py` | نسخ صوت Whisper |
| سي شارب | `csharp/WhisperTranscription.cs` | نسخ صوت Whisper |
| جافاسكربت | `javascript/foundry-local-whisper.mjs` | نسخ صوت Whisper |

> **ملاحظة:** تستخدم هذه المختبرات **SDK فاوندري لوكال** لتنزيل وتحميل نموذج Whisper برمجيًا، ثم ترسل الصوت إلى نقطة نهاية متوافقة مع OpenAI محليًا للنسخ. نموذج Whisper (`whisper`) مدرج في كتالوج فاوندري لوكال ويعمل بالكامل على الجهاز - لا حاجة لمفاتيح API سحابية أو وصول شبكة.

---

### الجزء 10: استخدام نماذج مخصصة أو Hugging Face

**دليل المعمل:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- تجميع نماذج Hugging Face إلى صيغة ONNX محسنة باستخدام منشئ نماذج ONNX Runtime GenAI
- تجميع مخصص للأجهزة (CPU، بطاقة NVIDIA، DirectML، WebGPU) والكمية (int4، fp16، bf16)
- إنشاء ملفات تكوين قوالب الدردشة لـ فاوندري لوكال
- إضافة النماذج المجمعة إلى ذاكرة التخزين المؤقت لـ فاوندري لوكال
- تشغيل النماذج المخصصة عبر CLI، REST API، و OpenAI SDK
- مثال مرجعي: تجميع Qwen/Qwen3-0.6B من البداية للنهاية

---

### الجزء 11: استدعاء الأدوات مع النماذج المحلية

**دليل المعمل:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- تمكين النماذج المحلية من استدعاء دوال خارجية (استدعاء الأدوات/الدوال)
- تعريف مخططات الأدوات باستخدام صيغة استدعاء الدالة الخاصة بـ OpenAI
- التعامل مع تدفق المحادثة متعدد الأدوار لاستدعاء الأدوات
- تنفيذ استدعاءات الأدوات محليًا وإرجاع النتائج إلى النموذج
- اختيار النموذج المناسب لسيناريوهات استدعاء الأدوات (Qwen 2.5، Phi-4-mini)
- استخدام `ChatClient` الأصلي لـ SDK لاستدعاء الأدوات (جافاسكربت)

**أمثلة برمجية:**

| اللغة | الملف | الوصف |
|----------|------|-------------|
| بايثون | `python/foundry-local-tool-calling.py` | استدعاء الأدوات مع أدوات الطقس/السكان |
| سي شارب | `csharp/ToolCalling.cs` | استدعاء الأدوات مع .NET |
| جافاسكربت | `javascript/foundry-local-tool-calling.mjs` | استدعاء الأدوات مع ChatClient |

---

### الجزء 12: بناء واجهة ويب لكاتب زافا الإبداعي

**دليل المعمل:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- إضافة واجهة أمامية متصفح لكاتب زافا الإبداعي
- تقديم واجهة المستخدم المشتركة من بايثون (FastAPI)، جافاسكربت (Node.js HTTP)، وسي شارب (ASP.NET Core)
- استهلاك بث NDJSON في المتصفح باستخدام Fetch API و ReadableStream
- شارات حالة الوكيل المباشرة وبث نص المقال في الوقت الحقيقي

**الكود (واجهة المستخدم المشتركة):**

| الملف | الوصف |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | تخطيط الصفحة |
| `zava-creative-writer-local/ui/style.css` | تنسيق التصميم |
| `zava-creative-writer-local/ui/app.js` | قارئ البث ومنطق تحديث DOM |

**إضافات الباك إند:**

| اللغة | الملف | الوصف |
|----------|------|-------------|
| بايثون | `zava-creative-writer-local/src/api/main.py` | تحديث لتقديم واجهة المستخدم الثابتة |
| جافاسكربت | `zava-creative-writer-local/src/javascript/server.mjs` | سيرفر HTTP جديد يلف المنظم |
| سي شارب | `zava-creative-writer-local/src/csharp-web/Program.cs` | مشروع API مصغر جديد لـ ASP.NET Core |

---

### الجزء 13: إتمام الورشة
**دليل المختبر:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- ملخص لكل ما قمت ببنائه عبر جميع الأجزاء الـ 12
- أفكار إضافية لتوسيع تطبيقاتك
- روابط للموارد والتوثيق

---

## هيكل المشروع

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

## الموارد

| المورد | الرابط |
|----------|------|
| موقع Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| كتالوج النماذج | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local على GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| دليل البدء | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| مرجع Foundry Local SDK | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| إطار عمل Microsoft Agent | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## الترخيص

هذه المواد الخاصة بالورشة مقدمة لأغراض تعليمية.

---

**بناء موفق! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**إخلاء المسؤولية**:  
تمت ترجمة هذا المستند باستخدام خدمة الترجمة الآلية [Co-op Translator](https://github.com/Azure/co-op-translator). بينما نسعى جاهدين لتحقيق الدقة، يرجى العلم أن الترجمات الآلية قد تحتوي على أخطاء أو عدم دقة. يجب اعتبار الوثيقة الأصلية بلغتها الأصلية المصدر الرسمي والموثوق. للمعلومات الحرجة، يُنصح بالترجمة المهنية البشرية. نحن غير مسؤولين عن أي سوء فهم أو تفسيرات خاطئة تنتج عن استخدام هذه الترجمة.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->