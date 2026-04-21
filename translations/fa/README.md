<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# کارگاه Foundry Local - ساخت برنامه‌های هوش مصنوعی روی دستگاه

یک کارگاه عملی برای اجرای مدل‌های زبانی روی دستگاه خودتان و ساخت برنامه‌های هوشمند با استفاده از [Foundry Local](https://foundrylocal.ai) و [چارچوب عامل مایکروسافت](https://learn.microsoft.com/en-us/agent-framework/).

> **Foundry Local چیست؟** Foundry Local یک محیط اجرایی سبک است که به شما امکان دانلود، مدیریت و ارائه مدل‌های زبانی را کاملاً روی سخت‌افزار خودتان می‌دهد. این سرویس یک **API سازگار با OpenAI** ارائه می‌کند تا هر ابزار یا SDK که با OpenAI صحبت می‌کند بتواند متصل شود - بدون نیاز به حساب ابری.

### 🌐 پشتیبانی چندزبانه

#### پشتیبانی از طریق GitHub Action (خودکار و همیشه به‌روز)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](./README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **ترجیح می‌دهید به‌صورت محلی کلون کنید؟**
>
> این مخزن شامل ترجمه‌های بیش از ۵۰ زبان است که حجم دانلود را به‌طور قابل توجهی افزایش می‌دهد. برای کلون بدون ترجمه‌ها، از sparse checkout استفاده کنید:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (ویندوز):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> این روش همه چیز مورد نیاز شما برای تکمیل دوره را با دانلود بسیار سریع‌تر فراهم می‌کند.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## اهداف یادگیری

تا پایان این کارگاه قادر خواهید بود:

| # | هدف |
|---|-----------|
| 1 | نصب Foundry Local و مدیریت مدل‌ها با CLI |
| 2 | تسلط بر API SDK Foundry Local برای مدیریت برنامه‌نویسی مدل‌ها |
| 3 | اتصال به سرور استنتاج محلی با استفاده از SDKهای Python، JavaScript، و C# |
| 4 | ساخت یک خط لوله تولید تقویت‌شده با بازیابی (RAG) که پاسخ‌ها را در داده‌های خودتان مستند می‌کند |
| 5 | ایجاد عوامل هوش مصنوعی با دستورالعمل‌ها و شخصیت‌های پایدار |
| 6 | تنظیم گردش کار چندعاملی با حلقه‌های بازخورد |
| 7 | بررسی یک اپلیکیشن نهایی تولید - نویسنده خلاق Zava |
| 8 | ساخت چهارچوب‌های ارزیابی با داده‌های طلایی و امتیازدهی LLM به‌عنوان قاضی |
| 9 | تبدیل صدا به متن با Whisper - گفتار به متن در دستگاه با استفاده از SDK Foundry Local |
| 10 | کامپایل و اجرای مدل‌های سفارشی یا Hugging Face با ONNX Runtime GenAI و Foundry Local |
| 11 | فعال‌سازی مدل‌های محلی برای فراخوانی توابع خارجی با الگوی فراخوانی ابزار |
| 12 | ساخت رابط کاربری مبتنی بر مرورگر برای نویسنده خلاق Zava با پخش زنده زمان واقعی |

---

## پیش‌نیازها

| نیازمندی | جزئیات |
|-------------|---------|
| **سخت‌افزار** | حداقل ۸ گیگابایت رم (توصیه شده ۱۶ گیگابایت)؛ پردازنده پشتیبانی‌کننده AVX2 یا GPU پشتیبانی‌شده |
| **سیستم عامل** | ویندوز ۱۰/۱۱ (x64/ARM)، Windows Server 2025، یا مک‌او‌اس ۱۳+ |
| **CLI Foundry Local** | نصب از طریق `winget install Microsoft.FoundryLocal` (ویندوز) یا `brew tap microsoft/foundrylocal && brew install foundrylocal` (مک‌او‌اس). راهنمای [شروع کار](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) را ببینید. |
| **زمان اجرا زبان** | **Python 3.9+** و/یا **.NET 9.0+** و/یا **Node.js 18+** |
| **Git** | برای کلون کردن این مخزن |

---

## شروع کار

```bash
# ۱. مخزن را کلون کنید
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# ۲. بررسی کنید که Foundry Local نصب شده باشد
foundry model list              # فهرست مدل‌های موجود
foundry model run phi-3.5-mini  # شروع یک گفتگوی تعاملی

# ۳. مسیر زبان خود را انتخاب کنید (برای تنظیم کامل به آزمایشگاه بخش ۲ مراجعه کنید)
```

| زبان | شروع سریع |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## بخش‌های کارگاه

### بخش ۱: شروع کار با Foundry Local

**راهنمای آزمایشگاه:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local چیست و چگونه کار می‌کند
- نصب CLI در ویندوز و مک‌او‌اس
- بررسی مدل‌ها - فهرست، دانلود، اجرا
- درک نام‌های مستعار مدل و پورت‌های دینامیک

---

### بخش ۲: معرفی عمیق SDK Foundry Local

**راهنمای آزمایشگاه:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- چرا برای توسعه برنامه از SDK به جای CLI استفاده کنیم
- مرجع کامل API SDK برای Python، JavaScript و C#
- مدیریت سرویس، مرور فهرست، چرخه عمر مدل (دانلود، بارگذاری، لغو بارگذاری)
- الگوهای شروع سریع: بوت‌استرپ سازنده Python، `init()` در JavaScript، `CreateAsync()` در C#
- اطلاعات `FoundryModelInfo`، نام‌های مستعار و انتخاب مدل بهینه برای سخت‌افزار

---

### بخش ۳: SDKها و APIها

**راهنمای آزمایشگاه:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- اتصال به Foundry Local از Python، JavaScript و C#
- استفاده از SDK Foundry Local برای مدیریت برنامه‌نویسی سرویس
- پخش مکالمات چت از طریق API سازگار با OpenAI
- مرجع متدهای SDK برای هر زبان

**نمونه کدها:**

| زبان | فایل | توضیح |
|----------|------|-------------|
| Python | `python/foundry-local.py` | چت پایه با پخش زنده |
| C# | `csharp/BasicChat.cs` | چت پخش زنده با .NET |
| JavaScript | `javascript/foundry-local.mjs` | چت پخش زنده با Node.js |

---

### بخش ۴: تولید تقویت‌شده با بازیابی (RAG)

**راهنمای آزمایشگاه:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG چیست و چرا اهمیت دارد
- ساخت پایگاه دانش در حافظه
- بازیابی با هم‌پوشانی کلیدواژه‌ها و امتیازدهی
- ترکیب درخواست‌های سیستمی مستند
- اجرای کامل خط لوله RAG روی دستگاه

**نمونه کدها:**

| زبان | فایل |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### بخش ۵: ساخت عوامل هوش مصنوعی

**راهنمای آزمایشگاه:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- عوامل هوش مصنوعی چیستند (مقایسه با فراخوانی مستقیم LLM)
- الگوی `ChatAgent` و چارچوب عامل مایکروسافت
- دستورالعمل‌های سیستم، شخصیت‌ها و گفتگوهای چنددور
- خروجی ساختاریافته (JSON) از عوامل

**نمونه کدها:**

| زبان | فایل | توضیح |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | عامل تکی با چارچوب عامل |
| C# | `csharp/SingleAgent.cs` | عامل تکی (الگوی ChatAgent) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | عامل تکی (الگوی ChatAgent) |

---

### بخش ۶: گردش کار چندعاملی

**راهنمای آزمایشگاه:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- خطوط لوله چندعاملی: پژوهشگر → نویسنده → ویرایشگر
- ارکستراسیون ترتیبی و حلقه‌های بازخورد
- پیکربندی مشترک و تحویل ساختاریافته
- طراحی گردش کار چندعاملی خودتان

**نمونه کدها:**

| زبان | فایل | توضیح |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | خط لوله سه عاملی |
| C# | `csharp/MultiAgent.cs` | خط لوله سه عاملی |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | خط لوله سه عاملی |

---

### بخش ۷: نویسنده خلاق Zava - اپلیکیشن نهایی

**راهنمای آزمایشگاه:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- یک اپ تولیدی چندعاملی با ۴ عامل تخصصی
- خط لوله ترتیبی با حلقه‌های بازخورد هدایت‌شده توسط ارزیاب
- خروجی پخش زنده، جستجوی کاتالوگ محصول، تحویل ساختاریافته JSON
- پیاده‌سازی کامل در Python (FastAPI)، JavaScript (Node.js CLI)، و C# (.NET کنسول)

**نمونه کدها:**

| زبان | دایرکتوری | توضیح |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | سرویس وب FastAPI با ارکستر |
| JavaScript | `zava-creative-writer-local/src/javascript/` | اپلیکیشن CLI Node.js |
| C# | `zava-creative-writer-local/src/csharp/` | اپلیکیشن کنسول .NET 9 |

---

### بخش ۸: توسعه مبتنی بر ارزیابی

**راهنمای آزمایشگاه:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- ساخت چارچوب ارزیابی سیستماتیک برای عوامل هوش مصنوعی با استفاده از داده‌های طلایی
- بررسی‌های مبتنی بر قاعده (طول، پوشش کلیدواژه، اصطلاحات ممنوعه) + امتیازدهی LLM به‌عنوان قاضی
- مقایسه کنار هم نسخه‌های درخواست با کارت‌های امتیاز تجمیعی
- گسترش الگوی عامل ویرایشگر Zava از بخش ۷ به مجموعه آزمایش‌های آفلاین
- مسیرهای Python، JavaScript و C#

**نمونه کدها:**

| زبان | فایل | توضیح |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | چارچوب ارزیابی |
| C# | `csharp/AgentEvaluation.cs` | چارچوب ارزیابی |
| JavaScript | `javascript/foundry-local-eval.mjs` | چارچوب ارزیابی |

---

### بخش ۹: تبدیل صوت به متن با Whisper

**راهنمای آزمایشگاه:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- رونویسی گفتار به متن با استفاده از OpenAI Whisper که به صورت محلی اجرا می‌شود  
- پردازش صوتی با اولویت حفظ حریم خصوصی – صدا هرگز دستگاه شما را ترک نمی‌کند  
- مسیرهای پایتون، جاوااسکریپت و C# با `client.audio.transcriptions.create()` (پایتون/JS) و `AudioClient.TranscribeAudioAsync()` (C#)  
- شامل فایل‌های نمونه صوتی با تم Zava برای تمرین عملی  

**نمونه‌های کد:**  

| زبان | فایل | توضیحات |  
|----------|------|-------------|  
| Python | `python/foundry-local-whisper.py` | رونویسی صوتی Whisper |  
| C# | `csharp/WhisperTranscription.cs` | رونویسی صوتی Whisper |  
| JavaScript | `javascript/foundry-local-whisper.mjs` | رونویسی صوتی Whisper |  

> **توجه:** این آزمایشگاه از **Foundry Local SDK** برای دانلود و بارگذاری برنامه‌ای مدل Whisper استفاده می‌کند، سپس صدا را به نقطه پایانی سازگار با OpenAI محلی برای رونویسی ارسال می‌کند. مدل Whisper (`whisper`) در فهرست Foundry Local فهرست شده و کاملاً روی دستگاه اجرا می‌شود – کلید API ابری یا دسترسی به شبکه نیاز ندارد.  

---  

### بخش ۱۰: استفاده از مدل‌های سفارشی یا Hugging Face  

**راهنمای آزمایشگاه:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)  

- کامپایل مدل‌های Hugging Face به فرمت بهینه ONNX با استفاده از سازنده مدل ONNX Runtime GenAI  
- کامپایل سخت‌افزاری مخصوص (CPU، کارت گرافیک NVIDIA، DirectML، WebGPU) و کمّی‌سازی (int4، fp16، bf16)  
- ایجاد فایل‌های پیکربندی قالب گفتگوی Foundry Local  
- افزودن مدل‌های کامپایل‌شده به حافظه کش Foundry Local  
- اجرای مدل‌های سفارشی از طریق CLI، REST API، و OpenAI SDK  
- نمونه مرجع: کامپایل کامل Qwen/Qwen3-0.6B  

---  

### بخش ۱۱: فراخوانی ابزار با مدل‌های محلی  

**راهنمای آزمایشگاه:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)  

- فعال‌سازی مدل‌های محلی برای فراخوانی توابع خارجی (فراخوانی ابزار/تابع)  
- تعریف طرح‌بندی ابزارها با استفاده از قالب فراخوانی تابع OpenAI  
- مدیریت جریان مکالمه چندمرحله‌ای فراخوانی ابزار  
- اجرای فراخوانی ابزار به صورت محلی و بازگرداندن نتایج به مدل  
- انتخاب مدل مناسب برای سناریوهای فراخوانی ابزار (Qwen 2.5، Phi-4-mini)  
- استفاده از `ChatClient` بومی SDK برای فراخوانی ابزار (جاوااسکریپت)  

**نمونه‌های کد:**  

| زبان | فایل | توضیحات |  
|----------|------|-------------|  
| Python | `python/foundry-local-tool-calling.py` | فراخوانی ابزار با ابزارهای هواشناسی/جمعیت |  
| C# | `csharp/ToolCalling.cs` | فراخوانی ابزار با .NET |  
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | فراخوانی ابزار با ChatClient |  

---  

### بخش ۱۲: ساخت رابط کاربری وب برای نویسنده خلاق Zava  

**راهنمای آزمایشگاه:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)  

- افزودن یک رابط کاربری مرورگری به نویسنده خلاق Zava  
- ارائه رابط کاربری مشترک از طریق پایتون (FastAPI)، جاوااسکریپت (Node.js HTTP)، و C# (ASP.NET Core)  
- مصرف NDJSON جریان‌یافته در مرورگر با Fetch API و ReadableStream  
- نشان‌گر وضعیت عامل زنده و پخش زنده متن مقاله  

**کد (رابط کاربری مشترک):**  

| فایل | توضیحات |  
|------|-------------|  
| `zava-creative-writer-local/ui/index.html` | طرح صفحه |  
| `zava-creative-writer-local/ui/style.css` | استایل‌دهی |  
| `zava-creative-writer-local/ui/app.js` | خواننده جریان و منطق به‌روزرسانی DOM |  

**افزودنی‌های بک‌اند:**  

| زبان | فایل | توضیحات |  
|----------|------|-------------|  
| Python | `zava-creative-writer-local/src/api/main.py` | به‌روزرسانی برای ارائه UI ایستا |  
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | سرور HTTP جدید که هماهنگ‌کننده را پوشش می‌دهد |  
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | پروژه جدید API مینیمال ASP.NET Core |  

---  

### بخش ۱۳: پایان کارگاه  

**راهنمای آزمایشگاه:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)  

- خلاصه‌ای از همه چیزهایی که در ۱۲ بخش ساخته‌اید  
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
| راهنمای شروع | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |  
| مرجع SDK Foundry Local | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |  
| چارچوب Agent مایکروسافت | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |  
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |  
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |  

---  

## مجوز  

این مطالب کارگاه برای اهداف آموزشی ارائه شده است.  

---  

**ساختن خوش بگذرد! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**سلب مسئولیت**:  
این سند با استفاده از خدمت ترجمه هوش مصنوعی [Co-op Translator](https://github.com/Azure/co-op-translator) ترجمه شده است. در حالی که ما در تلاش برای دقت هستیم، لطفاً توجه داشته باشید که ترجمه‌های خودکار ممکن است شامل خطاها یا نادرستی‌هایی باشند. سند اصلی به زبان بومی آن باید به عنوان منبع معتبر در نظر گرفته شود. برای اطلاعات حیاتی، استفاده از ترجمه حرفه‌ای انسانی توصیه می‌شود. ما در قبال هرگونه سوءتفاهم یا برداشت نادرست ناشی از استفاده این ترجمه مسئولیتی نمی‌پذیریم.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->