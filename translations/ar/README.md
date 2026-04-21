<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# ورشة عمل Foundry Local - بناء تطبيقات الذكاء الاصطناعي على الجهاز

ورشة عمل تطبيقية لتشغيل نماذج اللغة على جهازك الخاص وبناء تطبيقات ذكية باستخدام [Foundry Local](https://foundrylocal.ai) وإطار عمل الوكيل من [مايكروسوفت](https://learn.microsoft.com/en-us/agent-framework/).

> **ما هو Foundry Local؟** Foundry Local هو بيئة تشغيل خفيفة الوزن تتيح لك تنزيل وإدارة وتقديم نماذج اللغة بالكامل على جهازك. يوفر **واجهة برمجة تطبيقات متوافقة مع OpenAI** بحيث يمكن لأي أداة أو SDK تتحدث OpenAI الاتصال به - دون الحاجة إلى حساب سحابي.

### 🌐 دعم متعدد اللغات

#### مدعوم عبر إجراء GitHub (آلي ودائم التحديث)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[العربية](./README.md) | [البنغالية](../bn/README.md) | [البلغارية](../bg/README.md) | [البورمية (ميانمار)](../my/README.md) | [الصينية المبسطة](../zh-CN/README.md) | [الصينية التقليدية، هونغ كونغ](../zh-HK/README.md) | [الصينية التقليدية، ماكاو](../zh-MO/README.md) | [الصينية التقليدية، تايوان](../zh-TW/README.md) | [الكرواتية](../hr/README.md) | [التشيكية](../cs/README.md) | [الدنماركية](../da/README.md) | [الهولندية](../nl/README.md) | [الإستونية](../et/README.md) | [الفنلندية](../fi/README.md) | [الفرنسية](../fr/README.md) | [الألمانية](../de/README.md) | [اليونانية](../el/README.md) | [العبرية](../he/README.md) | [الهندية](../hi/README.md) | [الهنغارية](../hu/README.md) | [الإندونيسية](../id/README.md) | [الإيطالية](../it/README.md) | [اليابانية](../ja/README.md) | [الكانادية](../kn/README.md) | [الخميرية](../km/README.md) | [الكورية](../ko/README.md) | [الليتوانية](../lt/README.md) | [الماليزية](../ms/README.md) | [المالايالامية](../ml/README.md) | [الماراثية](../mr/README.md) | [النيبالية](../ne/README.md) | [النيجيرية بيدجن](../pcm/README.md) | [النرويجية](../no/README.md) | [الفارسية (الفارسية)](../fa/README.md) | [البولندية](../pl/README.md) | [البرتغالية (البرازيل)](../pt-BR/README.md) | [البرتغالية (البرتغال)](../pt-PT/README.md) | [البنجابية (غورموخي)](../pa/README.md) | [الرومانية](../ro/README.md) | [الروسية](../ru/README.md) | [الصربية (السيريلية)](../sr/README.md) | [السلوفاكية](../sk/README.md) | [السلوفينية](../sl/README.md) | [الإسبانية](../es/README.md) | [السواحيلية](../sw/README.md) | [السويدية](../sv/README.md) | [التاغالوغ (الفلبينية)](../tl/README.md) | [التاميلية](../ta/README.md) | [التيلوجو](../te/README.md) | [التايلاندية](../th/README.md) | [التركية](../tr/README.md) | [الأوكرانية](../uk/README.md) | [الأردية](../ur/README.md) | [الفيتنامية](../vi/README.md)

> **هل تفضل الاستنساخ محليًا؟**
>
> يحتوي هذا المستودع على أكثر من ٥٠ ترجمة للغات مما يزيد بشكل كبير من حجم التنزيل. للاستنساخ بدون الترجمات، استخدم السحب الجزئي:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (ويندوز):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> هذا يمنحك كل ما تحتاجه لإكمال الدورة بتنزيل أسرع بكثير.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## أهداف التعلم

بنهاية هذه الورشة ستكون قادرًا على:

| # | الهدف |
|---|-----------|
| 1 | تثبيت Foundry Local وإدارة النماذج باستخدام CLI |
| 2 | إتقان واجهة برمجة التطبيقات Foundry Local SDK لإدارة النماذج برمجيًا |
| 3 | الاتصال بخادم الاستدلال المحلي باستخدام SDK للبايثون، جافاسكريبت، وC# |
| 4 | بناء خط أنابيب انتاج معزز بالإسترجاع (RAG) يؤسس الإجابات على بياناتك الخاصة |
| 5 | إنشاء وكلاء ذكاء اصطناعي بتعليمات وشخصيات مستمرة |
| 6 | تنسيق تدفقات عمل متعددة الوكلاء بحلقات تغذية راجعة |
| 7 | استكشاف تطبيق عملي نهائي - كاتب إبداعي زافا |
| 8 | بناء أُطر تقييم باستخدام مجموعات بيانات ذهبية وتسجيل LLM كقاضٍ |
| 9 | تحويل الصوت إلى نص مع Whisper - الكلام إلى نص على الجهاز باستخدام Foundry Local SDK |
| 10 | تجميع وتشغيل نماذج مخصصة أو من Hugging Face باستخدام ONNX Runtime GenAI وFoundry Local |
| 11 | تمكين النماذج المحلية من استدعاء وظائف خارجية بنمط استدعاء الأدوات |
| 12 | بناء واجهة مستخدم متصفح لكاتب زافا الإبداعي مع البث في الوقت الفعلي |

---

## المتطلبات الأساسية

| المتطلب | التفاصيل |
|-------------|---------|
| **الأجهزة** | ذاكرة ٨ جيجابايت رام كحد أدنى (١٦ جيجابايت موصى به)؛ معالج يدعم AVX2 أو بطاقة رسومات مدعومة |
| **نظام التشغيل** | Windows 10/11 (x64/ARM)، Windows Server 2025، أو macOS 13+ |
| **Foundry Local CLI** | تثبيت عبر `winget install Microsoft.FoundryLocal` (ويندوز) أو `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). انظر [دليل البدء](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) للتفاصيل. |
| **بيئة تشغيل اللغات** | **بايثون 3.9+** و/أو **.NET 9.0+** و/أو **Node.js 18+** |
| **Git** | للاستنساخ هذا المستودع |

---

## البدء

```bash
# 1. استنساخ المستودع
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. تحقق من تثبيت Foundry Local
foundry model list              # سرد النماذج المتاحة
foundry model run phi-3.5-mini  # بدء دردشة تفاعلية

# 3. اختر مسار لغتك (انظر المختبر في الجزء 2 للإعداد الكامل)
```

| اللغة | بداية سريعة |
|----------|-------------|
| **بايثون** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **جافاسكريبت** | `cd javascript && npm install && node foundry-local.mjs` |

---

## أجزاء الورشة

### الجزء 1: البدء مع Foundry Local

**دليل المختبر:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- ما هو Foundry Local وكيف يعمل
- تثبيت CLI على ويندوز وماك
- استكشاف النماذج - العرض، التحميل، التشغيل
- فهم الأسماء المستعارة للنماذج والمنافذ الديناميكية

---

### الجزء 2: الغوص في Foundry Local SDK

**دليل المختبر:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- لماذا نستخدم SDK بدلاً من CLI لتطوير التطبيقات
- مرجع كامل لواجهة برمجة التطبيقات SDK للبايثون، جافاسكريبت، وC#
- إدارة الخدمة، تصفح الكتالوج، دورة حياة النموذج (تحميل، تحميل في الذاكرة، إلغاء التحميل)
- أنماط البداية السريعة: مُهيئ بايثون bootstrap، جافاسكريبت `init()`، C# `CreateAsync()`
- بيانات وصفية `FoundryModelInfo`، الأسماء المستعارة، واختيار النموذج الأمثل للأجهزة

---

### الجزء 3: SDKs وواجهات برمجة التطبيقات

**دليل المختبر:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- الاتصال بـ Foundry Local من بايثون، جافاسكريبت، وC#
- استخدام Foundry Local SDK لإدارة الخدمة برمجياً
- بث إكمالات الشات عبر واجهة برمجة التطبيقات المتوافقة مع OpenAI
- مرجع طرق SDK لكل لغة

**نماذج التعليمات البرمجية:**

| اللغة | الملف | الوصف |
|----------|------|-------------|
| بايثون | `python/foundry-local.py` | دردشة بث أساسية |
| C# | `csharp/BasicChat.cs` | دردشة بث مع .NET |
| جافاسكريبت | `javascript/foundry-local.mjs` | دردشة بث مع Node.js |

---

### الجزء 4: الإنتاج المعزز بالإسترجاع (RAG)

**دليل المختبر:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- ما هو RAG ولماذا هو مهم
- بناء قاعدة معرفة في الذاكرة
- استرجاع مصطلحات متداخلة مع تسجيل النقاط
- تركيب محثات نظام موثوقة
- تشغيل خط أنابيب RAG كامل على الجهاز

**نماذج التعليمات البرمجية:**

| اللغة | الملف |
|----------|------|
| بايثون | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| جافاسكريبت | `javascript/foundry-local-rag.mjs` |

---

### الجزء 5: بناء وكلاء الذكاء الاصطناعي

**دليل المختبر:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- ما هو وكيل الذكاء الاصطناعي (مقابل استدعاء LLM الخام)
- نمط `ChatAgent` وإطار عمل الوكيل من مايكروسوفت
- تعليمات النظام، الشخصيات، والمحادثات متعددة الأدوار
- الإخراج الهيكلي (JSON) من الوكلاء

**نماذج التعليمات البرمجية:**

| اللغة | الملف | الوصف |
|----------|------|-------------|
| بايثون | `python/foundry-local-with-agf.py` | وكيل مفرد مع إطار عمل الوكيل |
| C# | `csharp/SingleAgent.cs` | وكيل مفرد (نمط ChatAgent) |
| جافاسكريبت | `javascript/foundry-local-with-agent.mjs` | وكيل مفرد (نمط ChatAgent) |

---

### الجزء 6: تدفقات عمل متعددة الوكلاء

**دليل المختبر:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- خطوط أنابيب متعددة الوكلاء: الباحث → الكاتب → المحرر
- التنسيق التسلسلي وحلقات التغذية الراجعة
- التكوين المشترك والتسليمات المنظمة
- تصميم تدفق العمل متعدد الوكلاء الخاص بك

**نماذج التعليمات البرمجية:**

| اللغة | الملف | الوصف |
|----------|------|-------------|
| بايثون | `python/foundry-local-multi-agent.py` | خط أنابيب ثلاثي الوكلاء |
| C# | `csharp/MultiAgent.cs` | خط أنابيب ثلاثي الوكلاء |
| جافاسكريبت | `javascript/foundry-local-multi-agent.mjs` | خط أنابيب ثلاثي الوكلاء |

---

### الجزء 7: زافا الكاتب الإبداعي - التطبيق النهائي

**دليل المختبر:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- تطبيق متعدد الوكلاء على مستوى الإنتاج مع ٤ وكلاء متخصصين
- خط أنابيب تسلسلي مع حلقات تغذية راجعة يقودها المقيم
- إخراج مباشر، بحث في كتالوج المنتج، تسليمات JSON منظمة
- تنفيذ كامل في بايثون (FastAPI)، جافاسكريبت (CLI لـ Node.js)، وC# (وحدة تحكم .NET)

**نماذج التعليمات البرمجية:**

| اللغة | الدليل | الوصف |
|----------|-----------|-------------|
| بايثون | `zava-creative-writer-local/src/api/` | خدمة ويب FastAPI مع منسق |
| جافاسكريبت | `zava-creative-writer-local/src/javascript/` | تطبيق CLI لـ Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | تطبيق وحدة تحكم .NET 9 |

---

### الجزء 8: التطوير بقيادة التقييم

**دليل المختبر:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- بناء إطار تقييم منهجي لوكلاء الذكاء الاصطناعي باستخدام مجموعات بيانات ذهبية
- فحوصات قائمة على القواعد (الطول، تغطية الكلمات المفتاحية، المصطلحات المحظورة) + تسجيل LLM كقاضٍ
- مقارنة جنباً إلى جنب لمتغيرات المحث مع بطاقات النقاط الإجمالية
- يوسع نمط وكيل محرر زافا من الجزء ٧ إلى مجموعة اختبار غير متصلة بالإنترنت
- مسارات بايثون، جافاسكريبت، وC#

**نماذج التعليمات البرمجية:**

| اللغة | الملف | الوصف |
|----------|------|-------------|
| بايثون | `python/foundry-local-eval.py` | إطار التقييم |
| C# | `csharp/AgentEvaluation.cs` | إطار التقييم |
| جافاسكريبت | `javascript/foundry-local-eval.mjs` | إطار التقييم |

---

### الجزء 9: النسخ الصوتي بالصوت مع Whisper

**دليل المختبر:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- التفريغ الصوتي إلى نص باستخدام OpenAI Whisper الذي يعمل محليًا  
- معالجة صوتية تراعي الخصوصية - الصوت لا يغادر جهازك أبدًا  
- مسارات Python و JavaScript و C# مع `client.audio.transcriptions.create()` (Python/JS) و `AudioClient.TranscribeAudioAsync()` (C#)  
- يتضمن ملفات صوتية عينة temática زافا للتدريب العملي  

**عينات الكود:**  

| اللغة | الملف | الوصف |  
|----------|------|-------------|  
| Python | `python/foundry-local-whisper.py` | التفريغ الصوتي بواسطة Whisper |  
| C# | `csharp/WhisperTranscription.cs` | التفريغ الصوتي بواسطة Whisper |  
| JavaScript | `javascript/foundry-local-whisper.mjs` | التفريغ الصوتي بواسطة Whisper |  

> **ملاحظة:** يستخدم هذا المختبر **Foundry Local SDK** لتحميل نموذج Whisper برمجيًا، ثم يرسل الصوت إلى نقطة نهاية محلية متوافقة مع OpenAI للتفريغ. نموذج Whisper (`whisper`) مدرج في كتالوج Foundry Local ويعمل بالكامل على الجهاز - لا حاجة لمفاتيح API سحابية أو وصول للشبكة.

---

### الجزء 10: استخدام النماذج المخصصة أو نماذج Hugging Face  

**دليل المختبر:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)  

- تجميع نماذج Hugging Face إلى صيغة ONNX المحسنة باستخدام منشئ نماذج ONNX Runtime GenAI  
- تجميع مخصص للمعدات (وحدة المعالجة المركزية، وحدة معالجة الرسوميات NVIDIA، DirectML، WebGPU) والتكميم (int4, fp16, bf16)  
- إنشاء ملفات تكوين قالب الدردشة لـ Foundry Local  
- إضافة النماذج المجَمعة إلى ذاكرة Foundry Local المؤقتة  
- تشغيل النماذج المخصصة عبر CLI، REST API، وOpenAI SDK  
- مثال مرجعي: تجميع Qwen/Qwen3-0.6B بشكل شامل  

---

### الجزء 11: استدعاء الأدوات مع النماذج المحلية  

**دليل المختبر:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)  

- تمكين النماذج المحلية من استدعاء الوظائف الخارجية (استدعاء الأدوات/الوظائف)  
- تعريف مخططات الأدوات باستخدام تنسيق استدعاء الوظائف OpenAI  
- التعامل مع سير محادثة استدعاء الأدوات متعدد الخطوات  
- تنفيذ استدعاءات الأدوات محليًا وإرجاع النتائج للنموذج  
- اختيار النموذج المناسب لسيناريوهات استدعاء الأدوات (Qwen 2.5، Phi-4-mini)  
- استخدام ChatClient الأصلي في SDK لاستدعاء الأدوات (JavaScript)  

**عينات الكود:**  

| اللغة | الملف | الوصف |  
|----------|------|-------------|  
| Python | `python/foundry-local-tool-calling.py` | استدعاء أدوات الطقس/السكان |  
| C# | `csharp/ToolCalling.cs` | استدعاء الأدوات مع .NET |  
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | استدعاء الأدوات مع ChatClient |  

---

### الجزء 12: بناء واجهة ويب لـ Zava Creative Writer  

**دليل المختبر:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)  

- إضافة واجهة أمامية معتمدة على المتصفح لـ Zava Creative Writer  
- تقديم واجهة المستخدم المشتركة من Python (FastAPI)، JavaScript (Node.js HTTP)، و C# (ASP.NET Core)  
- استهلاك دفق NDJSON في المتصفح باستخدام Fetch API و ReadableStream  
- شارات حالة العميل الحي وبث نص المقالة في الوقت الفعلي  

**الكود (واجهة المستخدم المشتركة):**  

| الملف | الوصف |  
|------|-------------|  
| `zava-creative-writer-local/ui/index.html` | تخطيط الصفحة |  
| `zava-creative-writer-local/ui/style.css` | التنسيق |  
| `zava-creative-writer-local/ui/app.js` | قارئ البث ومنطق تحديث DOM |  

**الإضافات الخلفية:**  

| اللغة | الملف | الوصف |  
|----------|------|-------------|  
| Python | `zava-creative-writer-local/src/api/main.py` | محدث لخدمة واجهة المستخدم الثابتة |  
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | خادم HTTP جديد يغلف المنسق |  
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | مشروع API مختصر جديد لـ ASP.NET Core |  

---

### الجزء 13: انتهاء ورشة العمل  

**دليل المختبر:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)  

- ملخص لكل ما أنشأته عبر الأجزاء الـ 12  
- أفكار إضافية لتوسيع تطبيقاتك  
- روابط لمصادر ووثائق  

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
| إطار وكلاء Microsoft | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |  
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |  
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |  

---

## الترخيص  

هذه المواد الخاصة بورشة العمل متاحة لأغراض تعليمية.

---

**نتمنى لك بناءً موفقًا! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**إخلاء المسؤولية**:  
تمت ترجمة هذا المستند باستخدام خدمة الترجمة الآلية [Co-op Translator](https://github.com/Azure/co-op-translator). بينما نسعى لتحقيق الدقة، يرجى العلم أن الترجمات الآلية قد تحتوي على أخطاء أو عدم دقة. يجب اعتبار المستند الأصلي بلغته الأصلية المصدر الرسمي. للمعلومات الحرجة، يُنصح بالاستعانة بالترجمة البشرية المهنية. نحن غير مسؤولين عن أي سوء فهم أو تفسير ناتج عن استخدام هذه الترجمة.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->