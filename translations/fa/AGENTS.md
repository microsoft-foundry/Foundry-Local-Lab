# دستورالعمل‌های عامل کدنویسی

این فایل زمینه را برای عامل‌های کدنویسی هوش مصنوعی (GitHub Copilot، Copilot Workspace، Codex و غیره) که در این مخزن کار می‌کنند، فراهم می‌کند.

## مرور کلی پروژه

این یک **کارگاه عملی** برای ساخت برنامه‌های هوش مصنوعی با [Foundry Local](https://foundrylocal.ai) است — یک محیط اجرایی سبک که مدل‌های زبانی را به‌طور کامل روی دستگاه بارگیری، مدیریت و ارائه می‌دهد از طریق یک API سازگار با OpenAI. این کارگاه شامل راهنماهای آزمایشگاهی مرحله‌به‌مرحله و نمونه‌های قابل اجرا در پایتون، جاوااسکریپت و C# است.

## ساختار مخزن

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

## جزئیات زبان و فریم‌ورک

### پایتون
- **موقعیت:** `python/`، `zava-creative-writer-local/src/api/`
- **وابستگی‌ها:** `python/requirements.txt`، `zava-creative-writer-local/src/api/requirements.txt`
- **بسته‌های کلیدی:** `foundry-local-sdk`، `openai`، `agent-framework-foundry-local`، `fastapi`، `uvicorn`
- **نسخه حداقل:** پایتون 3.9+
- **اجرابا:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### جاوااسکریپت
- **موقعیت:** `javascript/`، `zava-creative-writer-local/src/javascript/`
- **وابستگی‌ها:** `javascript/package.json`، `zava-creative-writer-local/src/javascript/package.json`
- **بسته‌های کلیدی:** `foundry-local-sdk`، `openai`
- **سیستم ماژول:** ماژول‌های ES (فایل‌های `.mjs`، `"type": "module"`)
- **نسخه حداقل:** Node.js 18+
- **اجرابا:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **موقعیت:** `csharp/`، `zava-creative-writer-local/src/csharp/`
- **فایل‌های پروژه:** `csharp/csharp.csproj`، `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **بسته‌های کلیدی:** `Microsoft.AI.Foundry.Local` (غیرویندوز)، `Microsoft.AI.Foundry.Local.WinML` (ویندوز — مجموعهٔ کامل با QNN EP)، `OpenAI`، `Microsoft.Agents.AI.OpenAI`
- **هدف:** .NET 9.0 (TFM شرطی: `net9.0-windows10.0.26100` روی ویندوز، `net9.0` در بقیه)
- **اجرابا:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## کنوانسیون‌های کدنویسی

### کلی
- تمام نمونه‌های کد، **مثال‌های تک‌فایل مستقل** هستند — بدون کتابخانه‌های ابزاری یا انتزاعات مشترک.
- هر نمونه پس از نصب وابستگی‌های خودش به طور مستقل اجرا می‌شود.
- کلیدهای API همیشه روی `"foundry-local"` تنظیم شده‌اند — Foundry Local این را به عنوان جایگزین استفاده می‌کند.
- آدرس‌های پایه از `http://localhost:<port>/v1` استفاده می‌کنند — پورت پویاست و در زمان اجرا از طریق SDK کشف می‌شود (`manager.urls[0]` در JS، `manager.endpoint` در پایتون).
- SDK Foundry Local مدیریت راه‌اندازی سرویس و کشف نقطه انتهایی را انجام می‌دهد؛ الگوهای SDK را ترجیح دهید به جای پورت‌های سخت‌کد شده.

### پایتون
- از SDK `openai` با `OpenAI(base_url=..., api_key="not-required")` استفاده کنید.
- از `FoundryLocalManager()` در `foundry_local` برای چرخه زندگی مدیریت‌شده SDK استفاده کنید.
- برای استریم: روی شیء `stream` با `for chunk in stream:` تکرار کنید.
- در فایل‌های نمونه هیچ نوعی را علامت نزنید (نمونه‌ها را ساده برای یادگیرندگان کارگاه نگه دارید).

### جاوااسکریپت
- نحو ماژول ES: `import ... from "..."`.
- از `OpenAI` از `"openai"` و `FoundryLocalManager` از `"foundry-local-sdk"` استفاده کنید.
- الگوی مقداردهی اولیه SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- استریم: `for await (const chunk of stream)`.
- از `await` در سطح بالا به طور کامل استفاده می‌شود.

### C#
- فعال بودن Nullable، usings ضمنی، .NET 9.
- از `FoundryLocalManager.StartServiceAsync()` برای چرخه زندگی مدیریت شده SDK استفاده کنید.
- استریم: `CompleteChatStreaming()` با `foreach (var update in completionUpdates)`.
- `csharp/Program.cs` اصلی یک مسیریاب CLI است که به متدهای ایستا `RunAsync()` ارسال می‌کند.

### فراخوانی ابزار
- فقط برخی مدل‌ها از فراخوانی ابزار پشتیبانی می‌کنند: خانواده **Qwen 2.5** (`qwen2.5-*`) و **Phi-4-mini** (`phi-4-mini`).
- طرح‌های ابزار از قالب JSON فراخوانی تابع OpenAI پیروی می‌کنند (`type: "function"`، `function.name`، `function.description`، `function.parameters`).
- مکالمه از الگوی چندگامی استفاده می‌کند: کاربر → دستیار (tool_calls) → ابزار (نتایج) → دستیار (پاسخ نهایی).
- `tool_call_id` در پیام‌های نتایج ابزار باید با `id` فراخوانی ابزار مدل مطابقت داشته باشد.
- پایتون از SDK OpenAI به طور مستقیم استفاده می‌کند؛ جاوااسکریپت از `ChatClient` بومی SDK (`model.createChatClient()`); C# از SDK OpenAI با `ChatTool.CreateFunctionTool()` بهره می‌برد.

### ChatClient (مشتری بومی SDK)
- جاوااسکریپت: `model.createChatClient()` یک `ChatClient` برمی‌گرداند با `completeChat(messages, tools?)` و `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` یک `ChatClient` استاندارد برمی‌گرداند که بدون وارد کردن بسته NuGet OpenAI می‌توان استفاده کرد.
- پایتون مشتری Native ChatClient ندارد — از SDK OpenAI با `manager.endpoint` و `manager.api_key` استفاده کنید.
- **مهم:** در جاوااسکریپت، `completeStreamingChat` از الگوی **کال‌بک** استفاده می‌کند، نه تکرار async.

### مدل‌های استدلال
- `phi-4-mini-reasoning` تفکر خود را قبل از پاسخ نهایی در برچسب‌های `<think>...</think>` جای می‌دهد.
- در صورت نیاز برچسب‌ها را تجزیه کنید تا استدلال از پاسخ جدا شود.

## راهنماهای آزمایشگاهی

فایل‌های آزمایشگاه در `labs/` به صورت Markdown هستند. آنها ساختار ثابتی دارند:
- تصویر سربرگ لوگو
- عنوان و هدف مورد استفاده
- مرور کلی، اهداف یادگیری، پیش‌نیازها
- بخش‌های توضیح مفاهیم با نمودارها
- تمرین‌های شماره دار با بلوک‌های کد و خروجی مورد انتظار
- جدول خلاصه، نکات کلیدی، مطالعه بیشتر
- لینک ناوبری به بخش بعدی

هنگام ویرایش محتوای آزمایشگاه:
- سبک قالب‌بندی Markdown موجود و سلسله مراتب بخش‌ها را حفظ کنید.
- بلوک‌های کد باید زبان را مشخص کنند (`python`، `javascript`، `csharp`، `bash`، `powershell`).
- برای دستورات شل که سیستم عامل مهم است، هر دو نسخه bash و PowerShell را ارائه دهید.
- از سبک‌های یادداشت `> **Note:**`، `> **Tip:**`، و `> **Troubleshooting:**` استفاده کنید.
- جداول با فرمت لوله‌ای `| Header | Header |` هستند.

## دستورات ساخت و تست

| اقدام | دستور |
|--------|---------|
| **نمونه‌های پایتون** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **نمونه‌های JS** | `cd javascript && npm install && node <script>.mjs` |
| **نمونه‌های C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava پایتون** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (وب)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (وب)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **خط فرمان Foundry Local** | `foundry model list`، `foundry model run <model>`، `foundry service status` |
| **ایجاد نمودارها** | `npx mmdc -i <input>.mmd -o <output>.svg` (نیازمند نصب npm به صورت ریشه) |

## وابستگی‌های خارجی

- **خط فرمان Foundry Local** باید روی ماشین توسعه‌دهنده نصب شده باشد (`winget install Microsoft.FoundryLocal` یا `brew install foundrylocal`).
- **سرویس Foundry Local** به صورت محلی اجرا می‌شود و یک API REST سازگار با OpenAI را روی پورتی پویا ارائه می‌دهد.
- هیچ سرویس ابری، کلید API، یا اشتراک Azure برای اجرای هیچ نمونه‌ای لازم نیست.
- بخش ۱۰ (مدل‌های سفارشی) به علاوه نیازمند `onnxruntime-genai` و بارگیری وزن مدل‌ها از Hugging Face است.

## فایل‌هایی که نباید کامیت شوند

فایل `.gitignore` باید (و برای بیشتر موارد این کار را می‌کند) موارد زیر را حذف کند:
- `.venv/` — محیط‌های مجازی پایتون
- `node_modules/` — وابستگی‌های npm
- `models/` — خروجی مدل ONNX کامپایل شده (فایل‌های باینری حجیم، ایجاد شده توسط بخش ۱۰)
- `cache_dir/` — کش بارگیری مدل Hugging Face
- `.olive-cache/` — دایرکتوری کاری Microsoft Olive
- `samples/audio/*.wav` — نمونه‌های صوتی تولید شده (تولید شده دوباره از طریق `python samples/audio/generate_samples.py`)
- آثار ساخت استاندارد پایتون (`__pycache__/`، `*.egg-info/`، `dist/` و غیره)

## مجوز

MIT — لطفاً فایل `LICENSE` را ببینید.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**سلب مسئولیت**:  
این سند با استفاده از خدمات ترجمه هوش مصنوعی [Co-op Translator](https://github.com/Azure/co-op-translator) ترجمه شده است. در حالی که ما برای دقت تلاش می‌کنیم، لطفاً توجه داشته باشید که ترجمه‌های خودکار ممکن است حاوی خطاها یا نادرستی‌هایی باشند. سند اصلی به زبان بومی آن باید به عنوان منبع معتبر در نظر گرفته شود. برای اطلاعات حیاتی، توصیه می‌شود از ترجمه حرفه‌ای انسانی استفاده شود. ما مسئول هیچ گونه سوءتفاهم یا برداشت نادرست ناشی از استفاده از این ترجمه نیستیم.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->