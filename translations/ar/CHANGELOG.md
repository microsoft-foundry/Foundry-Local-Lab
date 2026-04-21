# سجل التغييرات — ورشة العمل المحلية Foundry

تُوثَّق جميع التغييرات المهمة لهذه الورشة أدناه.

---

## 2026-03-11 — الجزء 12 و 13، واجهة ويب، إعادة كتابة Whisper، إصلاح WinML/QNN، والتحقق

### أُضيف
- **الجزء 12: بناء واجهة ويب لكاتب Zava الإبداعي** — دليل مختبر جديد (`labs/part12-zava-ui.md`) مع تمارين تغطي بث NDJSON، `ReadableStream` في المستعرض، شارات حالة الوكيل الحي، وبث نص المقال في الوقت الحقيقي
- **الجزء 13: إكمال الورشة** — مختبر ملخص جديد (`labs/part13-workshop-complete.md`) مع استعراض لجميع الأجزاء الـ 12، وأفكار إضافية، وروابط المصادر
- **واجهة Zava الأمامية:** `zava-creative-writer-local/ui/index.html`، `style.css`، `app.js` — واجهة متصفح HTML/CSS/JS مشتركة مستهلكة من قبل جميع البَاك إند الثلاثة
- **خادم HTTP جافا سكريبت:** `zava-creative-writer-local/src/javascript/server.mjs` — خادم HTTP جديد على نمط Express يغلف المُنسق للوصول عبر المتصفح
- **خلفية C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` و `ZavaCreativeWriterWeb.csproj` — مشروع API مصغر جديد يقدم الواجهة وبث NDJSON
- **مولد عينات الصوت:** `samples/audio/generate_samples.py` — سكريبت TTS دون اتصال باستخدام `pyttsx3` لتوليد ملفات WAV ذات طابع Zava للجزء 9
- **عينة صوت:** `samples/audio/zava-full-project-walkthrough.wav` — عينة صوت أطول جديدة لاختبار النسخ
- **سكريبت التحقق:** `validate-npu-workaround.ps1` — سكريبت PowerShell آلي للتحقق من حل NPU/QNN عبر جميع عينات C#
- **مخططات Mermaid على شكل SVG:** `images/part12-architecture.svg`، `part12-message-types.svg`، `part12-streaming-sequence.svg`
- **دعم WinML عبر المنصات:** جميع ملفات `.csproj` الثلاثة لـ C# (`csharp/csharp.csproj`، `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`، `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) تستخدم الآن TFM شرطي ومراجع حزم حصرية متبادلة لدعم عبر المنصات. على ويندوز: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (تشمل QNN EP plugin). على غير ويندوز: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK الأساسي). تم استبدال RID الثابت `win-arm64` في مشاريع Zava بالكشف التلقائي. حل اعتماد انتقالي يستبعد الأصول الأصلية لـ `Microsoft.ML.OnnxRuntime.Gpu.Linux` التي تتضمن مرجع win-arm64 مكسور. تمت إزالة حل try/catch السابق لـ NPU من جميع ملفات C# السبعة.

### تم التغيير
- **الجزء 9 (Whisper):** إعادة كتابة كبيرة — يستخدم جافا سكريبت الآن `AudioClient` المدمج في SDK (`model.createAudioClient()`) بدلاً من استدلال ONNX Runtime اليدوي؛ تحديث أوصاف الهيكلية، جداول المقارنة، ومخططات خط الأنابيب لتعكس نهج JS/C# `AudioClient` مقابل نهج Python ONNX Runtime
- **الجزء 11:** تحديث روابط التنقل (تشير الآن إلى الجزء 12)؛ إضافة مخططات SVG مرسومة لسير استدعاء الأدوات والتتابع
- **الجزء 10:** تحديث التنقل ليُمرر عبر الجزء 12 بدلاً من إنهاء الورشة
- **Whisper بايثون (`foundry-local-whisper.py`):** توسيع مع عينات صوت إضافية وتحسين التعامل مع الأخطاء
- **Whisper جافا سكريبت (`foundry-local-whisper.mjs`):** إعادة كتابة لاستخدام `model.createAudioClient()` مع `audioClient.transcribe()` بدلاً من جلسات ONNX Runtime اليدوية
- **FastAPI بايثون (`zava-creative-writer-local/src/api/main.py`):** تحديث لخدمة ملفات UI الثابتة إلى جانب API
- **كونسول Zava C# (`zava-creative-writer-local/src/csharp/Program.cs`):** إزالة حل NPU (يتم التعامل معه الآن بواسطة حزمة WinML)
- **README.md:** إضافة قسم الجزء 12 مع جداول عينات الكود والإضافات في الباك إند؛ إضافة قسم الجزء 13؛ تحديث أهداف التعلم وهيكل المشروع
- **KNOWN-ISSUES.md:** إزالة المشكلة رقم 7 التي تم حلها (متغير نموذج NPU SDK لـ C# — يتم التعامل معه الآن بواسطة حزمة WinML). إعادة ترقيم المشكلات المتبقية إلى #1–#6. تحديث تفاصيل البيئة مع .NET SDK 10.0.104
- **AGENTS.md:** تحديث شجرة هيكل المشروع مع إدخالات جديدة لـ `zava-creative-writer-local` (`ui/`، `csharp-web/`، `server.mjs`)؛ تحديث حزم C# الرئيسية وتفاصيل TFM الشرطية
- **labs/part2-foundry-local-sdk.md:** تحديث مثال `.csproj` لعرض نمط متعدد المنصات كامل مع TFM شرطي، مراجع حزم حصرية، وملاحظة شرح

### تم التحقق منه
- بناء جميع مشاريع C# الثلاثة (`csharp`، `ZavaCreativeWriter`، `ZavaCreativeWriterWeb`) بنجاح على Windows ARM64
- عينة الدردشة (`dotnet run chat`): تحميل نموذج `phi-3.5-mini-instruct-qnn-npu:1` عبر WinML/QNN — تحميل متغير NPU مباشرة بدون الرجوع إلى CPU
- عينة الوكيل (`dotnet run agent`): تنفيذ شامل مع محادثة متعددة الأدوار، رمز خروج 0
- Foundry Local CLI v0.8.117 و SDK v0.9.0 على .NET SDK 9.0.312

---

## 2026-03-11 — إصلاحات الكود، تنظيف النماذج، مخططات Mermaid، والتحقق

### أُصلح
- **جميع العينات البرمجية الـ 21 (7 بايثون، 7 جافا سكريبت، 7 C#):** إضافة `model.unload()` / `unload_model()` / `model.UnloadAsync()` لتنظيف النماذج عند الخروج لحل تحذيرات تسرب الذاكرة في OGA (المشكلة المعروفة #4)
- **csharp/WhisperTranscription.cs:** استبدال مسار `AppContext.BaseDirectory` الهش بمسار `FindSamplesDirectory()` الذي يصعد المجلدات لتحديد موقع `samples/audio` بشكل موثوق (المشكلة المعروفة #7)
- **csharp/csharp.csproj:** استبدال `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` الثابت بالكشف التلقائي باستخدام `$(NETCoreSdkRuntimeIdentifier)` بحيث يعمل `dotnet run` على أي منصة بدون علم `-r` ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### تم التغيير
- **الجزء 8:** تحويل حلقة التكرار القائمة على التقييم من مخطط ASCII إلى صورة SVG مرسومة
- **الجزء 10:** تحويل مخطط خط تجميع الترميز من أسهم ASCII إلى صورة SVG مرسومة
- **الجزء 11:** تحويل مخططات سير استدعاء الأدوات والتتابع إلى صور SVG مرسومة
- **الجزء 10:** نقل قسم "تم إكمال الورشة!" إلى الجزء 11 (المختبر النهائي)؛ استبداله برابط "الخطوات التالية"
- **KNOWN-ISSUES.md:** إعادة تحقق كاملة لجميع المشكلات ضد CLI v0.8.117. إزالة المشكلات المحلولة: تسرب ذاكرة OGA (تمت إضافة التنظيف)، مسار Whisper (FindSamplesDirectory)، استدلال HTTP 500 مستمر (غير قابل لإعادة الإنتاج، [#494](https://github.com/microsoft/Foundry-Local/issues/494))، قيود tool_choice (يعمل الآن مع `"required"` واستهداف دالة محددة في qwen2.5-0.5b). تحديث مشكلة Whisper جافا سكريبت — جميع الملفات تُرجع إخراج فارغ/ثنائي الآن (تراجع من v0.9.x، وشدة أعلى إلى كبيرة). تحديث #4 RID في C# مع حل الكشف التلقائي ورابط [#497](https://github.com/microsoft/Foundry-Local/issues/497). تبقى 7 مشكلات مفتوحة.
- **javascript/foundry-local-whisper.mjs:** إصلاح اسم متغير التنظيف (`whisperModel` → `model`)

### تم التحقق منه
- بايثون: تشغيل `foundry-local.py`، `foundry-local-rag.py`، `foundry-local-tool-calling.py` بنجاح مع التنظيف
- جافا سكريبت: تشغيل `foundry-local.mjs`، `foundry-local-rag.mjs`، `foundry-local-tool-calling.mjs` بنجاح مع التنظيف
- C#: نجاح `dotnet build` بدون تحذيرات أو أخطاء (الهدف net9.0)
- اجتياز فحص تركيب `py_compile` لجميع ملفات بايثون السبعة
- اجتياز فحص تركيب `node --check` لجميع ملفات جافا سكريبت السبعة

---

## 2026-03-10 — الجزء 11: استدعاء الأدوات، توسيع API SDK، وتغطية النماذج

### أُضيف
- **الجزء 11: استدعاء الأدوات مع النماذج المحلية** — دليل مختبر جديد (`labs/part11-tool-calling.md`) مع 8 تمارين تغطي مخططات الأدوات، تدفق متعدد الأدوار، استدعاءات أدوات متعددة، أدوات مخصصة، استدعاء أدوات ChatClient، و `tool_choice`
- **عينة بايثون:** `python/foundry-local-tool-calling.py` — استدعاء الأدوات مع `get_weather`/`get_population` باستخدام OpenAI SDK
- **عينة جافا سكريبت:** `javascript/foundry-local-tool-calling.mjs` — استدعاء الأدوات باستخدام `ChatClient` المحلي لـ SDK (`model.createChatClient()`)
- **عينة C#:** `csharp/ToolCalling.cs` — استدعاء الأدوات باستخدام `ChatTool.CreateFunctionTool()` عبر OpenAI C# SDK
- **الجزء 2، التمرين 7:** ChatClient أصلي — `model.createChatClient()` (JS) و `model.GetChatClientAsync()` (C#) كبدائل لـ OpenAI SDK
- **الجزء 2، التمرين 8:** متغيرات النماذج واختيار الأجهزة — `selectVariant()`, `variants`, جدول متغير NPU (7 نماذج)
- **الجزء 2، التمرين 9:** ترقيات النماذج وتجديد الكتالوج — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **الجزء 2، التمرين 10:** نماذج الاستدلال — `phi-4-mini-reasoning` مع أمثلة تحليل الوسم `<think>`
- **الجزء 3، التمرين 4:** `createChatClient` كبديل لـ OpenAI SDK، مع توثيق نمط رد الاتصال المتدفق
- **AGENTS.md:** إضافة قواعد كتابة استدعاء الأدوات، ChatClient، ونماذج الاستدلال

### تم التغيير
- **الجزء 1:** توسيع كتالوج النماذج — إضافة phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo
- **الجزء 2:** توسيع جداول مرجع API — إضافة `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **الجزء 2:** إعادة ترقيم التمارين 7-9 → 10-13 لاستيعاب التمارين الجديدة
- **الجزء 3:** تحديث جدول الاستنتاجات الرئيسية ليشمل ChatClient الأصلي
- **README.md:** إضافة قسم الجزء 11 مع جدول عينات الكود؛ إضافة هدف تعلم رقم 11؛ تحديث شجرة هيكل المشروع
- **csharp/Program.cs:** إضافة حالة `toolcall` إلى موجه CLI وتحديث نص المساعدة

---

## 2026-03-09 — تحديث SDK v0.9.0، الإنجليزية البريطانية، وجولة تحقق

### تم التغيير
- **جميع عينات الكود (بايثون، جافا سكريبت، C#):** تحديث إلى Foundry Local SDK v0.9.0 API — إصلاح `await catalog.getModel()` (كان ينقصه `await`)، تحديث أنماط تهيئة `FoundryLocalManager`، إصلاح اكتشاف نقطة النهاية
- **جميع أدلة المختبر (الأجزاء 1-10):** التحويل إلى الإنجليزية البريطانية (colour، catalogue، optimised، إلخ)
- **جميع أدلة المختبر:** تحديث أمثلة الكود للطابق API v0.9.0
- **جميع أدلة المختبر:** تحديث جداول مرجع API وكتل كود التمارين
- **إصلاح حرج في جافا سكريبت:** إضافة `await` المفقود على `catalog.getModel()` — أعاد `Promise` وليس كائن `Model`، مما أدى إلى فشل صامت لاحقاً

### تم التحقق منه
- تشغيل جميع عينات بايثون بنجاح ضد خدمة Foundry Local
- تشغيل جميع عينات جافا سكريبت بنجاح (Node.js 18+)
- بناء وتشغيل مشروع C# على .NET 9.0 (توافق أمامي من تجميع net8.0 SDK)
- تعديل والتحقق من 29 ملفًا عبر الورشة

---

## فهرس الملفات

| الملف | آخر تحديث | الوصف |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | توسيع كتالوج النماذج |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | تمارين جديدة 7-10، توسيع جداول API |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | تمرين جديد 4 (ChatClient)، تحديث الاستنتاجات |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + الإنجليزية البريطانية |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + الإنجليزية البريطانية |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + الإنجليزية البريطانية |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + الإنجليزية البريطانية |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | مخطط Mermaid |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + الإنجليزية البريطانية |
| `labs/part10-custom-models.md` | 2026-03-11 | مخطط Mermaid، تم نقل "ورشة العمل اكتملت" إلى الجزء 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | مختبر جديد، مخططات Mermaid، قسم ورشة العمل اكتملت |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | جديد: نموذج استدعاء الأداة |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | جديد: نموذج استدعاء الأداة |
| `csharp/ToolCalling.cs` | 2026-03-10 | جديد: نموذج استدعاء الأداة |
| `csharp/Program.cs` | 2026-03-10 | أُضيف أمر CLI `toolcall` |
| `README.md` | 2026-03-10 | الجزء 11، هيكل المشروع |
| `AGENTS.md` | 2026-03-10 | استدعاء الأدوات + اتّفاقيات ChatClient |
| `KNOWN-ISSUES.md` | 2026-03-11 | تمت إزالة المشكلة رقم 7 المحلولة، تبقى 6 مشاكل مفتوحة |
| `csharp/csharp.csproj` | 2026-03-11 | TFM عبر المنصات، مراجع WinML/base SDK حسب الشرط |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | TFM عبر المنصات، اكتشاف RID تلقائياً |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | TFM عبر المنصات، اكتشاف RID تلقائياً |
| `csharp/BasicChat.cs` | 2026-03-11 | تمت إزالة حل مشكلة try/catch لـ NPU |
| `csharp/SingleAgent.cs` | 2026-03-11 | تمت إزالة حل مشكلة try/catch لـ NPU |
| `csharp/MultiAgent.cs` | 2026-03-11 | تمت إزالة حل مشكلة try/catch لـ NPU |
| `csharp/RagPipeline.cs` | 2026-03-11 | تمت إزالة حل مشكلة try/catch لـ NPU |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | تمت إزالة حل مشكلة try/catch لـ NPU |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | مثال ملف .csproj عبر المنصات |
| `AGENTS.md` | 2026-03-11 | تحديث تفاصيل حزم C# و TFM |
| `CHANGELOG.md` | 2026-03-11 | هذا الملف |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**إخلاء المسؤولية**:  
تمت ترجمة هذا المستند باستخدام خدمة الترجمة الآلية [Co-op Translator](https://github.com/Azure/co-op-translator). بينما نسعى جاهدين للدقة، يرجى العلم أن الترجمات الآلية قد تحتوي على أخطاء أو عدم دقة. يجب اعتبار المستند الأصلي بلغته الأصلية هو المصدر المعتمد. بالنسبة للمعلومات الحيوية، يُنصح بالرجوع إلى ترجمة بشرية محترفة. نحن غير مسؤولين عن أي سوء فهم أو تفسيرات خاطئة تنشأ عن استخدام هذه الترجمة.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->