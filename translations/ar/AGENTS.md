# تعليمات وكيل الترميز

يوفر هذا الملف سياقًا لوكلاء الترميز بالذكاء الاصطناعي (GitHub Copilot، Copilot Workspace، Codex، إلخ) العاملين في هذا المستودع.

## نظرة عامة على المشروع

هذا ورشة عمل **تطبيقية** لبناء تطبيقات الذكاء الاصطناعي باستخدام [Foundry Local](https://foundrylocal.ai) — بيئة تشغيل خفيفة الوزن تقوم بتنزيل وإدارة وتقديم نماذج اللغة بالكامل على الجهاز عبر واجهة برمجة تطبيقات متوافقة مع OpenAI. تتضمن الورشة أدلة مختبر خطوة بخطوة وأمثلة قابلة للتنفيذ بلغة بايثون وجافا سكريبت وC#.

## هيكل المستودع

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

## تفاصيل اللغة والإطار

### بايثون
- **الموقع:** `python/`، `zava-creative-writer-local/src/api/`
- **التبعيات:** `python/requirements.txt`، `zava-creative-writer-local/src/api/requirements.txt`
- **الحزم الأساسية:** `foundry-local-sdk`، `openai`، `agent-framework-foundry-local`، `fastapi`، `uvicorn`
- **الإصدار الأدنى:** بايثون 3.9+
- **التشغيل:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### جافا سكريبت
- **الموقع:** `javascript/`، `zava-creative-writer-local/src/javascript/`
- **التبعيات:** `javascript/package.json`، `zava-creative-writer-local/src/javascript/package.json`
- **الحزم الأساسية:** `foundry-local-sdk`، `openai`
- **نظام الموديول:** ES modules (`.mjs` ملفات، `"type": "module"`)
- **الإصدار الأدنى:** Node.js 18+
- **التشغيل:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **الموقع:** `csharp/`، `zava-creative-writer-local/src/csharp/`
- **ملفات المشروع:** `csharp/csharp.csproj`، `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **الحزم الأساسية:** `Microsoft.AI.Foundry.Local` (غير ويندوز)، `Microsoft.AI.Foundry.Local.WinML` (ويندوز — مجموعة شاملة مع QNN EP)، `OpenAI`، `Microsoft.Agents.AI.OpenAI`
- **الهدف:** .NET 9.0 (TFM شرطي: `net9.0-windows10.0.26100` على ويندوز، `net9.0` في الأماكن الأخرى)
- **التشغيل:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## اتفاقيات الترميز

### عام
- كل أمثلة الشيفرة هي **أمثلة ذات ملف واحد مستقل** — لا توجد مكتبات مساعدة مشتركة أو تجريدات.
- يعمل كل مثال بشكل مستقل بعد تثبيت تبعياته الخاصة.
- تُضبط مفاتيح API دائمًا على `"foundry-local"` — يستخدم Foundry Local هذا كعنصر نائب.
- تستخدم عناوين URL الأساسية `http://localhost:<port>/v1` — المنفذ ديناميكي ويُكتشف زمن التشغيل عبر SDK (`manager.urls[0]` في جافا سكريبت، `manager.endpoint` في بايثون).
- يدير Foundry Local SDK بدء الخدمة واكتشاف نقاط النهاية؛ يُفضل نماذج SDK بدلًا من المنافذ المحددة الصلبة.

### بايثون
- استخدم SDK `openai` مع `OpenAI(base_url=..., api_key="not-required")`.
- استخدم `FoundryLocalManager()` من `foundry_local` لإدارة دورة حياة الخدمة عبر SDK.
- البث: التكرار على كائن `stream` بواسطة `for chunk in stream:`.
- لا توجد تعليقات نوع في ملفات الأمثلة (للحفاظ على الاختصار لمتعلمي الورشة).

### جافا سكريبت
- صياغة موديول ES: `import ... from "..."`.
- استخدم `OpenAI` من `"openai"` و`FoundryLocalManager` من `"foundry-local-sdk"`.
- نمط تهيئة SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- البث: `for await (const chunk of stream)`.
- يستخدم await على المستوى الأعلى طوال الوقت.

### C#
- تمكين القابلية للإلغاء، الاستيرادات الضمنية، .NET 9.
- استخدم `FoundryLocalManager.StartServiceAsync()` لإدارة دورة الحياة عبر SDK.
- البث: `CompleteChatStreaming()` مع `foreach (var update in completionUpdates)`.
- `csharp/Program.cs` الرئيسي هو موزع CLI يوجه المناداة إلى طرق ثابتة `RunAsync()`.

### استدعاء الأدوات
- فقط بعض النماذج تدعم استدعاء الأدوات: عائلة **Qwen 2.5** (`qwen2.5-*`) و **Phi-4-mini** (`phi-4-mini`).
- تتبع مخططات الأدوات تنسيق استدعاء الدوال JSON الخاص بـ OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- تستخدم المحادثة نمطًا متعدد الأدوار: المستخدم → المساعد (tool_calls) → الأداة (النتائج) → المساعد (الإجابة النهائية).
- يجب أن يطابق `tool_call_id` في رسائل نتائج الأداة معرف `id` من استدعاء الأداة الخاص بالنموذج.
- تستخدم بايثون SDK مباشرة من OpenAI؛ تستخدم جافا سكريبت `ChatClient` المدمج بالـ SDK (`model.createChatClient()`); ويستخدم C# SDK OpenAI مع `ChatTool.CreateFunctionTool()`.

### ChatClient (عميل SDK الأصلي)
- جافا سكريبت: `model.createChatClient()` تُرجع `ChatClient` مع `completeChat(messages, tools?)` و`completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` تُرجع `ChatClient` قياسي يمكن استخدامه بدون استيراد حزمة OpenAI NuGet.
- بايثون لا تمتلك ChatClient أصلي — استخدم SDK OpenAI مع `manager.endpoint` و`manager.api_key`.
- **مهم:** تستخدم جافا سكريبت `completeStreamingChat` نمط رد الاتصال callback، وليس التكرار الأسيني async iteration.

### نماذج التفكير
- يغلف `phi-4-mini-reasoning` تفكيره ضمن وسم `<think>...</think>` قبل الإجابة النهائية.
- قم بتحليل الوسوم لفصل التفكير عن الإجابة عند الحاجة.

## أدلة المختبر

توجد ملفات المختبر في `labs/` بصيغة Markdown. تتبع هيكلًا موحدًا:
- صورة شعار في الرأس
- عنوان ونص الهدف
- نظرة عامة، أهداف التعلم، المتطلبات المسبقة
- أقسام شرح المفاهيم مع مخططات
- تمارين مرقمة مع كتل الشيفرة والناتج المتوقع
- جدول ملخص، النقاط الرئيسية، قراءات إضافية
- رابط تنقّل إلى الجزء التالي

عند تعديل محتوى المختبر:
- حافظ على نمط التنسيق الحالي في Markdown وتسلسل الأقسام.
- يجب تحديد لغة كتل الكود (`python`، `javascript`، `csharp`، `bash`، `powershell`).
- قدّم نسخًا لكل من bash و PowerShell لأوامر الصدفة إذ يهم نظام التشغيل.
- استخدم أنماط النداء: `> **Note:**`، `> **Tip:**`، و`> **Troubleshooting:**`.
- تستخدم الجداول صيغة الأنابيب `| Header | Header |`.

## أوامر البناء والاختبار

| الإجراء | الأمر |
|--------|---------|
| **عينات بايثون** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **عينات جافا سكريبت** | `cd javascript && npm install && node <script>.mjs` |
| **عينات C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **زافا بايثون** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **زافا جافا سكريبت** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **زافا جافا سكريبت (ويب)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **زافا C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **زافا C# (ويب)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **CLI الخاص بـ Foundry Local** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **إنشاء مخططات** | `npx mmdc -i <input>.mmd -o <output>.svg` (يتطلب تثبيت npm بما في ذلك root) |

## التبعيات الخارجية

- يجب تثبيت **Foundry Local CLI** على جهاز المطور (`winget install Microsoft.FoundryLocal` أو `brew install foundrylocal`).
- خدمة **Foundry Local** تعمل محليًا وتعرض واجهة برمجة REST متوافقة مع OpenAI على منفذ ديناميكي.
- لا حاجة إلى خدمات سحابية أو مفاتيح API أو اشتراكات Azure لتشغيل أي مثال.
- الجزء 10 (النماذج المخصصة) يتطلب أيضًا `onnxruntime-genai` وتنزيل أوزان النموذج من Hugging Face.

## الملفات التي يجب ألا يتم الالتزام بها في Git

يجب أن يستبعد `.gitignore` (ويستبعد في الغالب):
- `.venv/` — بيئات افتراضية لبايثون
- `node_modules/` — تبعيات npm
- `models/` — ملفات نموذج ONNX المجمعة (ملفات ثنائية كبيرة، مولدة بواسطة الجزء 10)
- `cache_dir/` — كاش تنزيل نموذج Hugging Face
- `.olive-cache/` — مجلد عمل Microsoft Olive
- `samples/audio/*.wav` — عينات صوت تم توليدها (يتم إعادة توليدها عبر `python samples/audio/generate_samples.py`)
- مخرجات بناء بايثون القياسية (`__pycache__/`، `*.egg-info/`، `dist/`، إلخ)

## الترخيص

MIT — انظر `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**إخلاء المسؤولية**:  
تمت ترجمة هذا المستند باستخدام خدمة الترجمة الآلية [Co-op Translator](https://github.com/Azure/co-op-translator). بينما نسعى لتحقيق الدقة، يرجى العلم أن الترجمات الآلية قد تحتوي على أخطاء أو عدم دقة. ينبغي اعتبار المستند الأصلي بلغته الأصلية المصدر الموثوق به. للمعلومات الحساسة، يوصى بالترجمة المهنية البشرية. نحن غير مسؤولين عن أي سوء فهم أو تفسير ناتج عن استخدام هذه الترجمة.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->