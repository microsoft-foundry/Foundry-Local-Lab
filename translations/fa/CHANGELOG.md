# فهرست تغییرات — کارگاه محلی Foundry

تمام تغییرات قابل توجه این کارگاه در زیر مستند شده است.

---

## 2026-03-11 — بخش ۱۲ و ۱۳، رابط وب، بازنویسی Whisper، رفع WinML/QNN و اعتبارسنجی

### اضافه شده
- **بخش ۱۲: ساخت رابط وب برای نویسنده خلاق Zava** — راهنمای آزمایشگاه جدید (`labs/part12-zava-ui.md`) با تمرین‌هایی که پوشش‌دهندهٔ استریمینگ NDJSON، `ReadableStream` مرورگر، نشان‌های وضعیت زنده‌ی عامل و استریمینگ متن مقاله در زمان واقعی هستند
- **بخش ۱۳: تکمیل کارگاه** — آزمایشگاه خلاصه جدید (`labs/part13-workshop-complete.md`) با مرور تمام ۱۲ بخش، ایده‌های بیشتر و لینک منابع
- **رابط جلویی Zava UI:** `zava-creative-writer-local/ui/index.html`، `style.css`، `app.js` — رابط مرورگر مشترک ونیلا HTML/CSS/JS که توسط هر سه بک‌اند مصرف می‌شود
- **سرور HTTP جاوااسکریپت:** `zava-creative-writer-local/src/javascript/server.mjs` — سرور HTTP سبکی به سبک Express که اورکستریتور را برای دسترسی مرورگر می‌پیچد
- **بک‌اند C# ASP.NET Core:** `zava-creative-writer-local/src/csharp-web/Program.cs` و `ZavaCreativeWriterWeb.csproj` — پروژه API مینیمال جدید که UI و استریمینگ NDJSON را ارائه می‌دهد
- **تولیدکننده نمونه صوتی:** `samples/audio/generate_samples.py` — اسکریپت TTS آفلاین با استفاده از `pyttsx3` برای تولید فایل‌های WAV با تم Zava برای بخش ۹
- **نمونه صوتی:** `samples/audio/zava-full-project-walkthrough.wav` — نمونه صوتی طولانی‌تر جدید برای آزمایش رونویسی
- **اسکریپت اعتبارسنجی:** `validate-npu-workaround.ps1` — اسکریپت خودکار PowerShell برای اعتبارسنجی راه‌حل موقت NPU/QNN در تمام نمونه‌های C#
- **شکل‌های نمودار Mermaid به صورت SVG:** `images/part12-architecture.svg`، `part12-message-types.svg`، `part12-streaming-sequence.svg`
- **پشتیبانی چندسکویی WinML:** هر ۳ فایل `.csproj` زبان C# (`csharp/csharp.csproj`، `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`، `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) اکنون از TFM شرطی و مراجع بسته انحصاری متقابل برای پشتیبانی چندسکویی استفاده می‌کنند. در ویندوز: TFM `net9.0-windows10.0.26100` + `Microsoft.AI.Foundry.Local.WinML` (سوپرست که شامل پلاگین QNN EP است). در غیر ویندوز: TFM `net9.0` + `Microsoft.AI.Foundry.Local` (SDK پایه). RID سخت‌کد شده `win-arm64` در پروژه‌های Zava با تشخیص خودکار جایگزین شده است. راه‌حل موقتی وابستگی ترانزیتی دارایی‌های بومی `Microsoft.ML.OnnxRuntime.Gpu.Linux` را که اشاره شکسته به win-arm64 دارد، حذف می‌کند. راه‌حل موقت قبلی با try/catch برای NPU از تمام ۷ فایل C# حذف شده است.

### تغییر یافته
- **بخش ۹ (Whisper):** بازنویسی عمده — جاوااسکریپت اکنون از `AudioClient` ساخته شده در SDK (`model.createAudioClient()`) استفاده می‌کند به جای استنتاج دستی ONNX Runtime؛ توصیف‌های معماری، جداول مقایسه و نمودارهای خط لوله برای بازتاب رویکرد JS/C# `AudioClient` در مقابل رویکرد Python ONNX Runtime به‌روزرسانی شدند
- **بخش ۱۱:** پیوندهای ناوبری به‌روزرسانی شدند (اکنون به بخش ۱۲ اشاره می‌کند)؛ نمودارهای SVG رندر شده برای جریان فراخوانی ابزار و ترتیب اضافه شدند
- **بخش ۱۰:** ناوبری به گونه‌ای به‌روزرسانی شده که مسیر از طریق بخش ۱۲ باشد به جای پایان یافتن کارگاه
- **Whisper پایتون (`foundry-local-whisper.py`):** با نمونه‌های صوتی اضافی و بهبود مدیریت خطا گسترش یافته است
- **Whisper جاوااسکریپت (`foundry-local-whisper.mjs`):** بازنویسی شده تا از `model.createAudioClient()` با `audioClient.transcribe()` به جای جلسات دستی ONNX Runtime استفاده کند
- **FastAPI پایتون (`zava-creative-writer-local/src/api/main.py`):** به‌روزرسانی شده تا فایل‌های UI استاتیک را همراه با API ارائه دهد
- **کنسول Zava C# (`zava-creative-writer-local/src/csharp/Program.cs`):** راه‌حل موقت NPU حذف شد (اکنون توسط بسته WinML مدیریت می‌شود)
- **README.md:** بخش بخش ۱۲ با جداول نمونه کد و افزودنی‌های بک‌اند اضافه شد؛ بخش بخش ۱۳ افزوده شد؛ اهداف یادگیری و ساختار پروژه به‌روزرسانی شدند
- **KNOWN-ISSUES.md:** مسئله حل شده شماره ۷ حذف شد (نوع مدل NPU در SDK C# اکنون توسط بسته WinML مدیریت می‌شود). شماره‌گذاری مسائل باقی‌مانده به #۱–#۶ تغییر یافت. جزییات محیط با .NET SDK 10.0.104 به‌روزرسانی شد
- **AGENTS.md:** ساختار درخت پروژه با ورودی‌های جدید `zava-creative-writer-local` (`ui/`، `csharp-web/`، `server.mjs`) به‌روزرسانی شد؛ بسته‌های کلیدی C# و جزئیات TFM شرطی تنظیم شدند
- **labs/part2-foundry-local-sdk.md:** مثال `.csproj` به‌روزرسانی شد تا الگوی کامل چند سکویی با TFM شرطی، مراجع بسته انحصاری و یادداشت توضیحی نشان داده شود

### اعتبارسنجی شده
- هر ۳ پروژه C# (`csharp`، `ZavaCreativeWriter`، `ZavaCreativeWriterWeb`) با موفقیت روی ویندوز ARM64 ساخته شدند
- نمونه چت (`dotnet run chat`): مدل به عنوان `phi-3.5-mini-instruct-qnn-npu:1` از طریق WinML/QNN بارگذاری می‌شود — نسخه NPU مستقیماً بارگذاری شده بدون افت به CPU
- نمونه عامل (`dotnet run agent`): اجرای کامل با گفتگوی چند نوبتی، کد خروج ۰
- Foundry Local CLI نسخه v0.8.117 و SDK نسخه v0.9.0 روی .NET SDK 9.0.312

---

## 2026-03-11 — رفع اشکالات کد، پاکسازی مدل، نمودارهای Mermaid و اعتبارسنجی

### رفع شده
- **هر ۲۱ نمونه کد (۷ پایتون، ۷ جاوااسکریپت، ۷ C#):** فراخوانی `model.unload()` / `unload_model()` / `model.UnloadAsync()` هنگام خروج اضافه شد تا هشدارهای نشت حافظه OGA رفع شود (مسئله شناخته شده شماره ۴)
- **csharp/WhisperTranscription.cs:** مسیر نسبی شکننده `AppContext.BaseDirectory` با `FindSamplesDirectory()` جایگزین شد که به صورت مطمئن درخت دایرکتوری‌ها را برای یافتن `samples/audio` پیمایش می‌کند (مسئله شناخته شده شماره ۷)
- **csharp/csharp.csproj:** `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` سخت‌کد شده با تشخیص خودکار جایگزین شد که از `$(NETCoreSdkRuntimeIdentifier)` استفاده کند تا فرمان `dotnet run` روی هر سکویی بدون پرچم `-r` کار کند ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### تغییر یافته
- **بخش ۸:** حلقه تکرار مبتنی بر ارزیابی از نمودار جعبه‌ای ASCII به تصویر رندر شده SVG تبدیل شد
- **بخش ۱۰:** نمودار خط لوله کامپایل از پیکان‌های ASCII به تصویر رندر شده SVG تبدیل شد
- **بخش ۱۱:** نمودارهای جریان فراخوان ابزار و ترتیب به تصاویر SVG رندر شده تبدیل شدند
- **بخش ۱۰:** بخش "کارگاه کامل!" به بخش ۱۱ (آخرین آزمایشگاه) منتقل شد؛ با لینک "گام‌های بعدی" جایگزین شد
- **KNOWN-ISSUES.md:** اعتبارسنجی کامل تمام مسائل علیه CLI v0.8.117 انجام شد. موارد حل شده حذف شدند: نشت حافظه OGA (پاکسازی اضافه شده)، مسیر Whisper (FindSamplesDirectory)، خطای HTTP 500 در استنتاج مداوم (تکرار نشد، [#494](https://github.com/microsoft/Foundry-Local/issues/494))، محدودیت‌های tool_choice (اکنون با `"required"` و هدف‌گیری تابع خاص روی qwen2.5-0.5b کار می‌کند). مسئله Whisper جاوااسکریپت به‌روزرسانی شده — اکنون تمام فایل‌ها خروجی خالی/باینری برمی‌گردانند (افت از نسخه v0.9.x، شدت به Major افزایش یافته است). RID C# شماره ۴ با راه‌حل اتوماتیک و پیوند [#497](https://github.com/microsoft/Foundry-Local/issues/497) به‌روزرسانی شد. ۷ مسئله باز باقی مانده‌اند.
- **javascript/foundry-local-whisper.mjs:** نام متغیر پاکسازی اصلاح شد (`whisperModel` → `model`)

### اعتبارسنجی شده
- پایتون: `foundry-local.py`، `foundry-local-rag.py`، `foundry-local-tool-calling.py` — با پاکسازی با موفقیت اجرا شدند
- جاوااسکریپت: `foundry-local.mjs`، `foundry-local-rag.mjs`، `foundry-local-tool-calling.mjs` — با پاکسازی با موفقیت اجرا شدند
- C#: ساخت `dotnet build` بدون هشدار و خطا (هدف net9.0) موفقیت‌آمیز بود
- تمام ۷ فایل پایتون از نظر syntax با `py_compile` پاس شدند
- تمام ۷ فایل جاوااسکریپت از نظر syntax توسط `node --check` تایید شدند

---

## 2026-03-10 — بخش ۱۱: فراخوانی ابزار، گسترش API SDK و پوشش مدل

### اضافه شده
- **بخش ۱۱: فراخوانی ابزار با مدل‌های محلی** — راهنمای آزمایشگاه جدید (`labs/part11-tool-calling.md`) با ۸ تمرین که شمای ابزار، جریان چند نوبتی، چندین فراخوان ابزار، ابزارهای سفارشی، فراخوانی ابزار ChatClient و `tool_choice` را پوشش می‌دهند
- **نمونه پایتون:** `python/foundry-local-tool-calling.py` — فراخوانی ابزار با ابزارهای `get_weather`/`get_population` با استفاده از SDK OpenAI
- **نمونه جاوااسکریپت:** `javascript/foundry-local-tool-calling.mjs` — فراخوانی ابزار با استفاده از `ChatClient` بومی SDK (`model.createChatClient()`)
- **نمونه C#:** `csharp/ToolCalling.cs` — فراخوانی ابزار با استفاده از `ChatTool.CreateFunctionTool()` همراه با SDK OpenAI زبان C#
- **بخش ۲، تمرین ۷:** ChatClient بومی — `model.createChatClient()` (JS) و `model.GetChatClientAsync()` (C#) به عنوان جایگزین‌هایی برای SDK OpenAI
- **بخش ۲، تمرین ۸:** انواع مدل و انتخاب سخت‌افزار — `selectVariant()`، `variants`، جدول نسخه NPU (۷ مدل)
- **بخش ۲، تمرین ۹:** ارتقاهای مدل و تازه‌سازی کاتالوگ — `is_model_upgradeable()`، `upgrade_model()`، `updateModels()`
- **بخش ۲، تمرین ۱۰:** مدل‌های استدلال — `phi-4-mini-reasoning` همراه با مثال‌های تجزیه برچسب `<think>`
- **بخش ۳، تمرین ۴:** `createChatClient` به عنوان جایگزین SDK OpenAI، با مستندات الگوی بازخورد استریمینگ
- **AGENTS.md:** قواعد کدگذاری فراخوانی ابزار، ChatClient و مدل‌های استدلال اضافه شدند

### تغییر یافته
- **بخش ۱:** کاتالوگ مدل توسعه یافته — اضافه شدن phi-4-mini-reasoning، gpt-oss-20b، phi-4، qwen2.5-7b، qwen2.5-coder-7b، whisper-large-v3-turbo
- **بخش ۲:** جداول مرجع API گسترش یافته — افزودن `createChatClient`، `createAudioClient`، `removeFromCache`، `selectVariant`، `variants`، `isLoaded`، `stopWebService`، `is_model_upgradeable`، `upgrade_model`، `httpx_client`، `getModels`، `getCachedModels`، `getLoadedModels`، `updateModels`، `GetModelVariantAsync`، `UpdateModelsAsync`
- **بخش ۲:** شماره تمرین‌های ۷ تا ۹ به ۱۰ تا ۱۳ تغییر یافت تا تمرین‌های جدید جا بیفتند
- **بخش ۳:** جدول نکات کلیدی برای گنجاندن ChatClient بومی به‌روزرسانی شد
- **README.md:** بخش ۱۱ با جدول نمونه‌های کد اضافه شد؛ هدف یادگیری شماره ۱۱ اضافه شد؛ ساختار پروژه به‌روزرسانی شد
- **csharp/Program.cs:** مورد `toolcall` به روتر CLI اضافه شد و متن راهنما به‌روزرسانی شد

---

## 2026-03-09 — به روز رسانی SDK v0.9.0، انگلیسی بریتانیایی و گذر اعتبارسنجی

### تغییر یافته
- **تمام نمونه‌های کد (پایتون، جاوااسکریپت، C#):** به API نسخه Foundry Local SDK v0.9.0 به‌روزرسانی شدند — رفع اشکال `await catalog.getModel()` (که `await` نداشت)، الگوهای راه‌اندازی `FoundryLocalManager` به‌روزرسانی شدند، کشف endpoint اصلاح شد
- **تمام راهنماهای آزمایشگاه (بخش‌های ۱-۱۰):** به انگلیسی بریتانیایی تبدیل شدند (colour، catalogue، optimised و غیره)
- **تمام راهنماهای آزمایشگاه:** نمونه‌های کد SDK به‌روزرسانی شدند تا با API v0.9.0 مطابقت داشته باشند
- **تمام راهنماهای آزمایشگاه:** جداول مرجع API و بلوک‌های کد تمرین به‌روزرسانی شدند
- **رفع بحرانی در جاوااسکریپت:** اضافه شدن `await` گمشده روی `catalog.getModel()` — که پیش از این یک Promise بازمی‌گرداند نه یک شیء Model و باعث خطاهای خاموش در مراحل بعدی می‌شد

### اعتبارسنجی شده
- همه نمونه‌های پایتون با موفقیت در برابر سرویس Foundry Local اجرا شدند
- همه نمونه‌های جاوااسکریپت با موفقیت اجرا شدند (Node.js نسخه ۱۸+)
- پروژه C# ساخته شده و روی .NET 9.0 اجرا می‌شود (سازگاری رو به جلو از اسمبلی SDK net8.0)
- ۲۹ فایل در کل کارگاه اصلاح و اعتبارسنجی شدند

---

## فهرست فایل‌ها

| فایل | آخرین به‌روزرسانی | توضیحات |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | کاتالوگ مدل توسعه یافته |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | تمرین‌های جدید ۷–۱۰، جداول API گسترش یافته |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | تمرین جدید ۴ (ChatClient)، نکات کلیدی به‌روزرسانی شده |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK نسخه 0.9.0 + انگلیسی بریتانیایی |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK نسخه 0.9.0 + انگلیسی بریتانیایی |
| `labs/part6-multi-agent-workflows.md` | ۲۰۲۶-۰۳-۰۹ | SDK نسخه ۰.۹.۰ + انگلیسی بریتانیایی |
| `labs/part7-zava-creative-writer.md` | ۲۰۲۶-۰۳-۰۹ | SDK نسخه ۰.۹.۰ + انگلیسی بریتانیایی |
| `labs/part8-evaluation-led-development.md` | ۲۰۲۶-۰۳-۱۱ | نمودار علامت‌گذاری Mermaid |
| `labs/part9-whisper-voice-transcription.md` | ۲۰۲۶-۰۳-۰۹ | SDK نسخه ۰.۹.۰ + انگلیسی بریتانیایی |
| `labs/part10-custom-models.md` | ۲۰۲۶-۰۳-۱۱ | نمودار علامت‌گذاری Mermaid، انتقال بخش اتمام کارگاه به قسمت ۱۱ |
| `labs/part11-tool-calling.md` | ۲۰۲۶-۰۳-۱۱ | آزمایشگاه جدید، نمودارهای Mermaid، بخش اتمام کارگاه |
| `python/foundry-local-tool-calling.py` | ۲۰۲۶-۰۳-۱۰ | جدید: نمونه فراخوانی ابزار |
| `javascript/foundry-local-tool-calling.mjs` | ۲۰۲۶-۰۳-۱۰ | جدید: نمونه فراخوانی ابزار |
| `csharp/ToolCalling.cs` | ۲۰۲۶-۰۳-۱۰ | جدید: نمونه فراخوانی ابزار |
| `csharp/Program.cs` | ۲۰۲۶-۰۳-۱۰ | اضافه شده دستور خط فرمان `toolcall` |
| `README.md` | ۲۰۲۶-۰۳-۱۰ | قسمت ۱۱، ساختار پروژه |
| `AGENTS.md` | ۲۰۲۶-۰۳-۱۰ | فراخوانی ابزار + قراردادهای ChatClient |
| `KNOWN-ISSUES.md` | ۲۰۲۶-۰۳-۱۱ | حذف مشکل رفع شده شماره ۷، باقی ماندن ۶ مشکل باز |
| `csharp/csharp.csproj` | ۲۰۲۶-۰۳-۱۱ | چندسکویی TFM، منابع شرطی WinML/base SDK |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | ۲۰۲۶-۰۳-۱۱ | چندسکویی TFM، شناسایی خودکار RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | ۲۰۲۶-۰۳-۱۱ | چندسکویی TFM، شناسایی خودکار RID |
| `csharp/BasicChat.cs` | ۲۰۲۶-۰۳-۱۱ | حذف راه‌حل جایگزین try/catch برای NPU |
| `csharp/SingleAgent.cs` | ۲۰۲۶-۰۳-۱۱ | حذف راه‌حل جایگزین try/catch برای NPU |
| `csharp/MultiAgent.cs` | ۲۰۲۶-۰۳-۱۱ | حذف راه‌حل جایگزین try/catch برای NPU |
| `csharp/RagPipeline.cs` | ۲۰۲۶-۰۳-۱۱ | حذف راه‌حل جایگزین try/catch برای NPU |
| `csharp/AgentEvaluation.cs` | ۲۰۲۶-۰۳-۱۱ | حذف راه‌حل جایگزین try/catch برای NPU |
| `labs/part2-foundry-local-sdk.md` | ۲۰۲۶-۰۳-۱۱ | نمونه .csproj چندسکویی |
| `AGENTS.md` | ۲۰۲۶-۰۳-۱۱ | به‌روزرسانی بسته‌های C# و جزئیات TFM |
| `CHANGELOG.md` | ۲۰۲۶-۰۳-۱۱ | این فایل |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**سلب مسئولیت**:  
این سند با استفاده از سرویس ترجمه هوش مصنوعی [Co-op Translator](https://github.com/Azure/co-op-translator) ترجمه شده است. در حالی که ما برای دقت تلاش می‌کنیم، لطفاً توجه داشته باشید که ترجمه‌های خودکار ممکن است شامل خطاها یا نادرستی‌هایی باشند. سند اصلی به زبان مادری خود به‌عنوان منبع معتبر تلقی شود. برای اطلاعات حیاتی، ترجمه حرفه‌ای انسانی توصیه می‌شود. ما مسئول هیچ گونه سوتفاهم یا تفسیر نادرست ناشی از استفاده از این ترجمه نیستیم.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->