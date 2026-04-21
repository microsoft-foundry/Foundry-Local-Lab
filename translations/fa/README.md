<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# کارگاه Foundry Local - ساخت برنامه‌های هوش مصنوعی روی دستگاه

یک کارگاه عملی برای اجرای مدل‌های زبان روی دستگاه خودتان و ساخت برنامه‌های هوشمند با [Foundry Local](https://foundrylocal.ai) و [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Foundry Local چیست؟** Foundry Local یک محیط زمان اجرا سبک است که به شما اجازه می‌دهد مدل‌های زبان را به‌طور کامل روی سخت‌افزار خود دانلود، مدیریت و ارائه دهید. این ابزار یک **رابط برنامه‌نویسی سازگار با OpenAI** را در اختیار می‌گذارد تا هر ابزار یا SDK که با OpenAI سازگار است بتواند متصل شود - بدون نیاز به حساب ابری.

---

## اهداف یادگیری

تا پایان این کارگاه شما قادر خواهید بود:

| # | هدف |
|---|-----------|
| ۱ | نصب Foundry Local و مدیریت مدل‌ها با خط فرمان |
| ۲ | تسلط بر API SDK Foundry Local برای مدیریت برنامه‌ای مدل‌ها |
| ۳ | اتصال به سرور استنتاج محلی با استفاده از SDK‌های پایتون، جاوااسکریپت و سی‌شارپ |
| ۴ | ساخت یک خط لوله تولید بنابر بازیابی (RAG) که پاسخ‌ها را در داده‌های خود شما ریشه‌دار می‌کند |
| ۵ | ایجاد عوامل هوش مصنوعی با دستورالعمل‌ها و شخصیت‌های ماندگار |
| ۶ | هماهنگ‌سازی گردش‌کار چندعامل با حلقه‌های بازخورد |
| ۷ | بررسی یک برنامه نمونه تولیدی - نویسنده خلاقانه زاو |
| ۸ | ساخت چارچوب‌های ارزیابی با داده‌های طلایی و نمره‌دهی با مدل‌های زبان بزرگ (LLM) به عنوان داور |
| ۹ | تبدیل گفتار به متن با Whisper - تبدیل گفتار به متن روی دستگاه با استفاده از SDK Foundry Local |
| ۱۰ | کامپایل و اجرای مدل‌های سفارشی یا Hugging Face با ONNX Runtime GenAI و Foundry Local |
| ۱۱ | فعال‌سازی مدل‌های محلی برای فراخوانی توابع خارجی با الگوی فراخوانی ابزار |
| ۱۲ | ساخت رابط کاربری مبتنی بر مرورگر برای نویسنده خلاقانه زاو با پخش زنده |

---

## ملزومات

| نیاز | جزئیات |
|-------------|---------|
| **سخت‌افزار** | حداقل ۸ گیگابایت رم (توصیه‌شده ۱۶ گیگابایت)؛ پردازنده پشتیبانی‌کننده AVX2 یا کارت گرافیک پشتیبانی‌شده |
| **سیستم‌عامل** | ویندوز ۱۰/۱۱ (x64/ARM)، Windows Server 2025، یا macOS ۱۳+ |
| **Foundry Local CLI** | نصب از طریق `winget install Microsoft.FoundryLocal` (ویندوز) یا `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). برای جزئیات به [راهنمای شروع](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) مراجعه کنید. |
| **محیط اجرا** | **پایتون ۳.۹+** و/یا **.NET 9.0+** و/یا **Node.js 18+** |
| **گیت** | برای کلون کردن این مخزن |

---

## شروع به کار

```bash
# ۱. مخزن را کلون کنید
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# ۲. اطمینان حاصل کنید که Foundry Local نصب شده است
foundry model list              # لیست مدل‌های قابل دسترس
foundry model run phi-3.5-mini  # شروع یک چت تعاملی

# ۳. مسیر زبان خود را انتخاب کنید (برای تنظیم کامل به آزمایشگاه قسمت ۲ مراجعه کنید)
```

| زبان | شروع سریع |
|----------|-------------|
| **پایتون** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **جاوااسکریپت** | `cd javascript && npm install && node foundry-local.mjs` |

---

## بخش‌های کارگاه

### بخش ۱: شروع به کار با Foundry Local

**راهنمای کارگاه:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local چیست و چگونه کار می‌کند
- نصب CLI روی ویندوز و macOS
- بررسی مدل‌ها - فهرست، دانلود، اجرا
- درک نام‌های مستعار مدل و پورت‌های پویا

---

### بخش ۲: بررسی عمیق SDK Foundry Local

**راهنمای کارگاه:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- چرا به جای CLI از SDK برای توسعه برنامه استفاده کنیم
- مرجع کامل API SDK برای پایتون، جاوااسکریپت، و سی‌شارپ
- مدیریت سرویس، مرور کاتالوگ، چرخه عمر مدل (دانلود، بارگذاری، آزادسازی)
- الگوهای شروع سریع: راه‌اندازی سازنده پایتون، `init()` در جاوااسکریپت، `CreateAsync()` در سی‌شارپ
- متادیتای `FoundryModelInfo`، نام‌های مستعار و انتخاب مدل بهینه سخت‌افزاری

---

### بخش ۳: SDK و API‌ها

**راهنمای کارگاه:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- اتصال به Foundry Local از طریق پایتون، جاوااسکریپت و سی‌شارپ
- استفاده از SDK برای مدیریت برنامه‌ای سرویس
- پخش زنده پاسخ‌های چت با API سازگار با OpenAI
- مرجع روش‌های SDK برای هر زبان

**نمونه کد:**

| زبان | فایل | توضیحات |
|----------|------|-------------|
| پایتون | `python/foundry-local.py` | چت پایه با پخش زنده |
| C# | `csharp/BasicChat.cs` | چت پخش زنده با .NET |
| جاوااسکریپت | `javascript/foundry-local.mjs` | چت پخش زنده با Node.js |

---

### بخش ۴: تولید بنابر بازیابی (RAG)

**راهنمای کارگاه:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG چیست و چرا اهمیت دارد
- ساخت یک پایگاه دانش در حافظه
- بازیابی با همپوشانی کلمات کلیدی و امتیازدهی
- ترکیب پرامپت‌های سیستم مبتنی بر داده
- اجرای کامل خط لوله RAG روی دستگاه

**نمونه کد:**

| زبان | فایل |
|----------|------|
| پایتون | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| جاوااسکریپت | `javascript/foundry-local-rag.mjs` |

---

### بخش ۵: ساخت عوامل هوش مصنوعی

**راهنمای کارگاه:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- عامل هوش مصنوعی چیست (در مقابل فراخوانی خام مدل زبان بزرگ)
- الگوی `ChatAgent` و Microsoft Agent Framework
- دستورالعمل‌های سیستمی، شخصیت‌ها و گفتگوهای چندمرحله‌ای
- خروجی ساختاریافته (JSON) از عوامل

**نمونه کد:**

| زبان | فایل | توضیحات |
|----------|------|-------------|
| پایتون | `python/foundry-local-with-agf.py` | عامل منفرد با Agent Framework |
| C# | `csharp/SingleAgent.cs` | عامل منفرد (الگوی ChatAgent) |
| جاوااسکریپت | `javascript/foundry-local-with-agent.mjs` | عامل منفرد (الگوی ChatAgent) |

---

### بخش ۶: گردش‌کار چندعامل

**راهنمای کارگاه:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- خط لوله‌های چندعامل: پژوهشگر → نویسنده → ویراستار
- هماهنگی ترتیبی و حلقه‌های بازخورد
- پیکربندی مشترک و تحویل‌های ساختاریافته
- طراحی گردش‌کار چندعامل خودتان

**نمونه کد:**

| زبان | فایل | توضیحات |
|----------|------|-------------|
| پایتون | `python/foundry-local-multi-agent.py` | خط لوله سه عاملی |
| C# | `csharp/MultiAgent.cs` | خط لوله سه عاملی |
| جاوااسکریپت | `javascript/foundry-local-multi-agent.mjs` | خط لوله سه عاملی |

---

### بخش ۷: نویسنده خلاقانه زاو - برنامه نمونه نهایی

**راهنمای کارگاه:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- یک برنامه چندعامل تولیدی با ۴ عامل تخصصی
- خط لوله ترتیبی با حلقه‌های بازخورد هدایت شده توسط ارزیابی‌کننده
- خروجی پخش زنده، جستجوی کاتالوگ محصول، تحویل‌های ساختاریافته JSON
- پیاده‌سازی کامل در پایتون (FastAPI)، جاوااسکریپت (Node.js CLI)، و C# (کنسول .NET)

**نمونه کد:**

| زبان | دایرکتوری | توضیحات |
|----------|-----------|-------------|
| پایتون | `zava-creative-writer-local/src/api/` | سرویس وب FastAPI با هماهنگ‌کننده |
| جاوااسکریپت | `zava-creative-writer-local/src/javascript/` | برنامه CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | برنامه کنسول .NET 9 |

---

### بخش ۸: توسعه مبتنی بر ارزیابی

**راهنمای کارگاه:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- ساخت چارچوب ارزیابی سیستماتیک برای عوامل هوش مصنوعی با استفاده از داده‌های طلایی
- بررسی‌های مبتنی بر قوانین (طول، پوشش کلمات کلیدی، اصطلاحات ممنوعه) + نمره‌دهی مدل زبان بزرگ به عنوان داور
- مقایسه رو در رو از انواع پرامپت‌ها با کارنامه نمرات تجمیعی
- توسعه الگوی عامل ویرایشگر زاو از بخش ۷ به مجموعه تست‌های آفلاین
- مسیرهای پایتون، جاوااسکریپت و C#

**نمونه کد:**

| زبان | فایل | توضیحات |
|----------|------|-------------|
| پایتون | `python/foundry-local-eval.py` | چارچوب ارزیابی |
| C# | `csharp/AgentEvaluation.cs` | چارچوب ارزیابی |
| جاوااسکریپت | `javascript/foundry-local-eval.mjs` | چارچوب ارزیابی |

---

### بخش ۹: رونویسی صدا با Whisper

**راهنمای کارگاه:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- تبدیل گفتار به متن با استفاده از OpenAI Whisper به صورت محلی
- پردازش صوتی با حفظ حریم خصوصی - صدا هرگز از دستگاه خارج نمی‌شود
- مسیرهای پایتون، جاوااسکریپت و C# با `client.audio.transcriptions.create()` (پایتون/جاوااسکریپت) و `AudioClient.TranscribeAudioAsync()` (C#)
- شامل فایل‌های نمونه صوتی با تم زاو برای تمرین عملی

**نمونه کد:**

| زبان | فایل | توضیحات |
|----------|------|-------------|
| پایتون | `python/foundry-local-whisper.py` | رونویسی صدای Whisper |
| C# | `csharp/WhisperTranscription.cs` | رونویسی صدای Whisper |
| جاوااسکریپت | `javascript/foundry-local-whisper.mjs` | رونویسی صدای Whisper |

> **توجه:** این کارگاه از **SDK Foundry Local** برای دانلود و بارگذاری برنامه‌ای مدل Whisper استفاده می‌کند، سپس صدا را به نقطه پایانی محلی سازگار با OpenAI برای رونویسی ارسال می‌کند. مدل Whisper (`whisper`) در کاتالوگ Foundry Local فهرست شده و کاملاً روی دستگاه اجرا می‌شود - نیازی به کلید API ابری یا دسترسی شبکه ندارد.

---

### بخش ۱۰: استفاده از مدل‌های سفارشی یا Hugging Face

**راهنمای کارگاه:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- کامپایل مدل‌های Hugging Face به فرمت بهینه‌شده ONNX با استفاده از ONNX Runtime GenAI مدل‌ساز
- کامپایل مخصوص سخت‌افزار (CPU، GPU انویدیا، DirectML، WebGPU) و کمیت‌سازی (int4، fp16، bf16)
- ساخت فایل‌های پیکربندی قالب چت برای Foundry Local
- افزودن مدل‌های کامپایل شده به کش Foundry Local
- اجرای مدل‌های سفارشی از طریق CLI، REST API و SDK OpenAI
- نمونه مرجع: کامپایل کامل Qwen/Qwen3-0.6B

---

### بخش ۱۱: فراخوانی ابزار با مدل‌های محلی

**راهنمای کارگاه:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- فعال کردن مدل‌های محلی برای فراخوانی توابع خارجی (فراخوانی ابزار/تابع)
- تعریف طرح ابزار با استفاده از قالب فراخوانی تابع OpenAI
- مدیریت گفتگوهای چندمرحله‌ای فراخوانی ابزار
- اجرای فراخوانی ابزار به صورت محلی و بازگرداندن نتایج به مدل
- انتخاب مدل مناسب برای سناریوهای فراخوانی ابزار (Qwen 2.5، Phi-4-mini)
- استفاده از `ChatClient` بومی SDK برای فراخوانی ابزار (جاوااسکریپت)

**نمونه کد:**

| زبان | فایل | توضیحات |
|----------|------|-------------|
| پایتون | `python/foundry-local-tool-calling.py` | فراخوانی ابزار با ابزارهای آب و هوا/جمعیت |
| C# | `csharp/ToolCalling.cs` | فراخوانی ابزار با .NET |
| جاوااسکریپت | `javascript/foundry-local-tool-calling.mjs` | فراخوانی ابزار با ChatClient |

---

### بخش ۱۲: ساخت رابط وب برای نویسنده خلاقانه زاو

**راهنمای کارگاه:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- افزودن رابط فرانت‌اند مبتنی بر مرورگر به نویسنده خلاقانه زاو
- ارائه رابط مشترک از پایتون (FastAPI)، جاوااسکریپت (سرور HTTP Node.js)، و C# (ASP.NET Core)
- استفاده از NDJSON پخش زنده در مرورگر با Fetch API و ReadableStream
- نشانگر وضعیت زنده عامل‌ها و پخش متن مقاله در زمان واقعی

**کد (رابط مشترک):**

| فایل | توضیحات |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | طرح صفحه |
| `zava-creative-writer-local/ui/style.css` | استایل‌دهی |
| `zava-creative-writer-local/ui/app.js` | خواننده جریان و منطق به‌روزرسانی DOM |

**افزودنی‌های بک‌اند:**

| زبان | فایل | توضیحات |
|----------|------|-------------|
| پایتون | `zava-creative-writer-local/src/api/main.py` | به‌روزرسانی برای ارائه UI استاتیک |
| جاوااسکریپت | `zava-creative-writer-local/src/javascript/server.mjs` | سرور HTTP جدید با پوشش هماهنگ‌کننده |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | پروژه جدید ASP.NET Core minimal API |

---

### بخش ۱۳: پایان کارگاه
**راهنمای آزمایشگاه:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- خلاصه‌ای از هر آنچه در تمام ۱۲ بخش ساخته‌اید
- ایده‌های بیشتر برای گسترش برنامه‌های شما
- لینک به منابع و مستندات

---

## ساختار پروژه

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

## منابع

| منبع | لینک |
|----------|------|
| وب‌سایت Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |
| فهرست مدل‌ها | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| گیت‌هاب Foundry Local | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| راهنمای شروع کار | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| مرجع SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| چارچوب عامل مایکروسافت | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## مجوز

این مطالب کارگاه برای اهداف آموزشی ارائه شده است.

---

**ساختن خوش بگذرد! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**سلب مسؤولیت**:  
این سند با استفاده از سرویس ترجمه ماشینی [Co-op Translator](https://github.com/Azure/co-op-translator) ترجمه شده است. در حالی که ما در تلاش برای دقت هستیم، لطفاً توجه داشته باشید که ترجمه‌های خودکار ممکن است حاوی خطاها یا نادرستی‌هایی باشند. سند اصلی به زبان بومی خود باید به عنوان منبع معتبر در نظر گرفته شود. برای اطلاعات حساس، ترجمه حرفه‌ای انسانی توصیه می‌شود. ما در قبال هرگونه سوءتفاهم یا تفسیر نادرست ناشی از استفاده از این ترجمه مسئولیتی نداریم.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->