# چینج لاگ — فاؤنڈری لوکل ورکشاپ

اس ورکشاپ میں تمام قابل ذکر تبدیلیاں ذیل میں دستاویزی ہیں۔

---

## 2026-03-11 — حصہ 12 اور 13، ویب یو آئی، وسپر ری رائٹ، WinML/QNN فکس، اور ویلیڈیشن

### شامل کیا گیا
- **حصہ 12: زاوہ کریئیٹو رائٹر کے لیے ویب یو آئی بنانا** — نیا لیب گائیڈ (`labs/part12-zava-ui.md`) جس میں اسٹریمینگ NDJSON، براؤزر `ReadableStream`, لائیو ایجنٹ اسٹیٹس بیجز، اور حقیقی وقت مضمون کا متن اسٹریمینگ کے مشقیں شامل ہیں  
- **حصہ 13: ورکشاپ مکمل** — نیا خلاصہ لیب (`labs/part13-workshop-complete.md`) جس میں تمام 12 حصوں کا ریکاپ، مزید خیالات، اور وسائل کے لنکس ہیں  
- **زاوہ یو آئی فرنٹ اینڈ:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — شیئرڈ ونیلا HTML/CSS/JS براؤزر انٹرفیس جو تینوں بیک اینڈز کے ذریعہ استعمال ہوتا ہے  
- **جاوا اسکرپٹ HTTP سرور:** `zava-creative-writer-local/src/javascript/server.mjs` — نیا ایکسپریس طرز HTTP سرور جو آرکیسٹریٹر کو براؤزر پر دستیاب بناتا ہے  
- **C# ASP.NET Core بیک اینڈ:** `zava-creative-writer-local/src/csharp-web/Program.cs` اور `ZavaCreativeWriterWeb.csproj` — نیا منیمم API پروجیکٹ جو یو آئی اور اسٹریمینگ NDJSON فراہم کرتا ہے  
- **آڈیو سیمپل جنریٹر:** `samples/audio/generate_samples.py` — آف لائن TTS اسکرپٹ `pyttsx3` استعمال کرتے ہوئے حصہ 9 کے لیے زاوہ تھیمڈ WAV فائلز تیار کرنے کے لیے  
- **آڈیو سیمپل:** `samples/audio/zava-full-project-walkthrough.wav` — ٹرانسکرپشن ٹیسٹنگ کے لیے نیا طویل آڈیو سیمپل  
- **ویلیڈیشن اسکرپٹ:** `validate-npu-workaround.ps1` — خودکار پاور شیل اسکرپٹ جو تمام C# سیمپلز میں NPU/QNN ورک اراؤنڈ کو ویلیڈیٹ کرتا ہے  
- **مرمیڈ ڈایاگرام SVGs:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`  
- **WinML کراس پلیٹ فارم سپورٹ:** تینوں C# `.csproj` فائلز (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) اب مشروط TFM اور ایک دوسرے کے متبادل پیکیج ریفرنسز استعمال کرتی ہیں۔ ونڈوز پر: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (ایک سوپرسٹ جو QNN EP پلگ ان شامل کرتا ہے)؛ غیر ونڈوز پر: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (بیس SDK). زاوہ پروجیکٹس میں ہارڈ کوڈڈ `win-arm64` RID کو خودکار پتہ لگانے سے بدل دیا گیا ہے۔ ایک متعدی انحصار ورک اراؤنڈ نے `Microsoft.ML.OnnxRuntime.Gpu.Linux` سے نیٹیو اثاثے خارج کیے جن میں ٹوٹی ہوئی win-arm64 ریفرنس تھی۔ پرانا try/catch NPU ورک اراؤنڈ تمام 7 C# فائلوں سے ہٹا دیا گیا۔

### تبدیل کیا گیا
- **حصہ 9 (وسپر):** بڑی ری رائٹ — جاوا اسکرپٹ اب SDK کے اندرونی `AudioClient` (`model.createAudioClient()`) استعمال کرتا ہے بجائے دستی ONNX Runtime inference کے؛ اپ ڈیٹ شدہ فن تعمیر کی تفصیلات، موازنہ جدول، اور پائپ لائن ڈایاگرامز JS/C# `AudioClient` طریقہ کار بمقابلہ Python ONNX Runtime طریقہ کار کو ظاہر کرتے ہیں  
- **حصہ 11:** نیویگیشن لنکس کو اپ ڈیٹ کیا (اب حصہ 12 کی طرف اشارہ کرتے ہیں)؛ ٹول کالنگ فلو اور سیکوئنس کے SVG ڈایاگرام شامل کیے گئے  
- **حصہ 10:** نیویگیشن کو اپ ڈیٹ کر کے ورکشاپ ختم کرنے کے بجائے حصہ 12 کے ذریعے راستہ دیا گیا  
- **Python Whisper (`foundry-local-whisper.py`):** مزید آڈیو سیمپلز کے ساتھ بڑھایا گیا اور ایرر ہینڈلنگ میں بہتری کی گئی  
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** دوبارہ لکھا گیا تاکہ `model.createAudioClient()` اور `audioClient.transcribe()` استعمال کرے بجائے دستی ONNX Runtime سیشنز کے  
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** اب API کے ساتھ ساتھ سٹیٹک UI فائلز کی فراہمی کی گئی  
- **Zava C# کنسول (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU ورک اراؤنڈ ہٹا دیا گیا (اب WinML پیکیج کے ذریعے ہینڈل ہوتا ہے)  
- **README.md:** حصہ 12 کا سیکشن کوڈ سیمپلز ٹیبلز اور بیک اینڈ اضافے کے ساتھ شامل کیا گیا؛ حصہ 13 کا سیکشن شامل کیا گیا؛ سیکھنے کے مقاصد اور پروجیکٹ ساخت کو اپ ڈیٹ کیا گیا  
- **KNOWN-ISSUES.md:** مسئلہ #7 (C# SDK NPU ماڈل ویریئنٹ – اب WinML پیکیج کے ذریعے ہینڈل ہوتا ہے) کو ہٹا دیا گیا؛ باقی مسائل کو #1 سے #6 تک پھر سے نمبر دیا گیا؛ ماحول کی تفصیلات کو .NET SDK 10.0.104 کے ساتھ اپ ڈیٹ کیا گیا  
- **AGENTS.md:** پروجیکٹ سٹرکچر ٹری میں نئے `zava-creative-writer-local` اندراجات شامل کیے (`ui/`, `csharp-web/`, `server.mjs`); C# اہم پیکیجز اور مشروط TFM کی تفصیلات اپ ڈیٹ کی گئیں  
- **labs/part2-foundry-local-sdk.md:** `.csproj` مثال کو مکمل کراس پلیٹ فارم پیٹرن کے ساتھ مشروط TFM، ایک دوسرے کے متبادل پیکیج ریفرنسز، اور وضاحتی نوٹ کے ساتھ اپ ڈیٹ کیا گیا  

### ویلیڈیٹ کیا گیا
- تینوں C# پروجیکٹس (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) کامیابی سے ونڈوز ARM64 پر بنے  
- چیٹ سیمپل (`dotnet run chat`): ماڈل `phi-3.5-mini-instruct-qnn-npu:1` کے طور پر WinML/QNN کے ذریعے لوڈ ہوتا ہے — NPU ویریئنٹ براہ راست لوڈ ہوتا ہے بغیر CPU فالو بیک کے  
- ایجنٹ سیمپل (`dotnet run agent`): ایند-ٹو-ایند ملٹی ٹرن بات چیت کے ساتھ چلتا ہے، نکلنے کا کوڈ 0 ہے  
- فاؤنڈری لوکل CLI v0.8.117 اور SDK v0.9.0 .NET SDK 9.0.312 پر  

---

## 2026-03-11 — کوڈ فکسز، ماڈل صفائی، مرمیڈ ڈایاگرامز، اور ویلیڈیشن

### درست کیا گیا
- **تمام 21 کوڈ سیمپلز (7 Python, 7 JavaScript, 7 C#):** ایگزٹ پر `model.unload()` / `unload_model()` / `model.UnloadAsync()` کلین اپ شامل کی تاکہ OGA میموری لیک وارننگز کا مسئلہ حل ہو (معروف مسئلہ #4)  
- **csharp/WhisperTranscription.cs:** نازک `AppContext.BaseDirectory` رشتہ دار راستہ کو `FindSamplesDirectory()` سے تبدیل کیا جو ڈائریکٹریز میں اوپر جاتا ہے تاکہ `samples/audio` کو معتبر طور پر پا سکے (معروف مسئلہ #7)  
- **csharp/csharp.csproj:** ہارڈ کوڈڈ `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` کو خودکار پتہ لگانے والے فال بیک `$(NETCoreSdkRuntimeIdentifier)` کے ساتھ بدل دیا گیا تاکہ `dotnet run` ہر پلیٹ فارم پر بغیر `-r` فلیگ کے کام کرے ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))  

### تبدیل کیا گیا
- **حصہ 8:** ASCII باکس ڈایاگرام سے ایوالویشن پر مبنی تکراری لوپ کو رینڈرڈ SVG تصویر میں تبدیل کیا گیا  
- **حصہ 10:** کمپائلیشن پائپ لائن ڈایاگرام کو ASCII تیر سے رینڈرڈ SVG تصویر میں تبدیل کیا گیا  
- **حصہ 11:** ٹول کالنگ فلو اور سیکوئنس ڈایاگرامز کو رینڈرڈ SVG تصاویر میں تبدیل کیا گیا  
- **حصہ 10:** "ورکشاپ مکمل!" سیکشن کو حصہ 11 (آخری لیب) میں منتقل کیا گیا؛ "اگلے اقدامات" لنک سے بدل دیا گیا  
- **KNOWN-ISSUES.md:** CLI v0.8.117 کے خلاف تمام مسائل کی مکمل دوبارہ تصدیق کی گئی؛ حل شدہ مسائل کو ہٹا دیا گیا: OGA میموری لیک (کلین اپ شامل)، وسپر راستہ (FindSamplesDirectory)، HTTP 500 مسلسل انفیرینس (نہ دہرانے والا، [#494](https://github.com/microsoft/Foundry-Local/issues/494))، tool_choice کی حدیں (اب `"required"` اور مخصوص فنکشن ٹارگٹنگ qwen2.5-0.5b پر کام کرتی ہیں)؛ JS وسپر مسئلہ اپ ڈیٹ کیا گیا — اب تمام فائلیں خالی/بائنری آؤٹ پٹ دیتی ہیں (v0.9.x سے ریگریشن، سنجیدگی میجر پر)؛ #4 C# RID کو خودکار پتہ لگانے کے ورک اراؤنڈ اور [#497](https://github.com/microsoft/Foundry-Local/issues/497) لنک کے ساتھ اپ ڈیٹ کیا گیا؛ 7 کھلے مسائل باقی ہیں۔  
- **javascript/foundry-local-whisper.mjs:** کلین اپ ویریبل کا نام درست کیا (`whisperModel` → `model`)  

### ویلیڈیٹ کیا گیا
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — کلین اپ کے ساتھ کامیابی سے چلتے ہیں  
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — کلین اپ کے ساتھ کامیابی سے چلتے ہیں  
- C#: `dotnet build` صفر وارننگز، صفر ایررز کے ساتھ کامیاب (net9.0 ٹارگٹ)  
- تمام 7 Python فائلیں `py_compile` سنتیکس چیک پاس کرتی ہیں  
- تمام 7 JavaScript فائلیں `node --check` سنتیکس ویلیڈیشن پاس کرتی ہیں  

---

## 2026-03-10 — حصہ 11: ٹول کالنگ، SDK API توسیع، اور ماڈل کوریج

### شامل کیا گیا
- **حصہ 11: لوکل ماڈلز کے ساتھ ٹول کالنگ** — نیا لیب گائیڈ (`labs/part11-tool-calling.md`) جس میں 8 مشقیں شامل ہیں جو ٹول اسکیموں، ملٹی ٹرن فلو، متعدد ٹول کالز، کسٹم ٹولز، ChatClient ٹول کالنگ، اور `tool_choice` کو کور کرتی ہیں  
- **Python سیمپل:** `python/foundry-local-tool-calling.py` — OpenAI SDK استعمال کرتے ہوئے `get_weather`/`get_population` ٹول کالنگ  
- **JavaScript سیمپل:** `javascript/foundry-local-tool-calling.mjs` — SDK کے اندرونی `ChatClient` (`model.createChatClient()`) استعمال کرتے ہوئے ٹول کالنگ  
- **C# سیمپل:** `csharp/ToolCalling.cs` — OpenAI C# SDK کے ساتھ `ChatTool.CreateFunctionTool()` استعمال کرتے ہوئے ٹول کالنگ  
- **حصہ 2، مشق 7:** اندرونی `ChatClient` — `model.createChatClient()` (JS) اور `model.GetChatClientAsync()` (C#) OpenAI SDK کے متبادل کے طور پر  
- **حصہ 2، مشق 8:** ماڈل ویریئنٹس اور ہارڈویئر کا انتخاب — `selectVariant()`, `variants`, NPU ویریئنٹ جدول (7 ماڈلز)  
- **حصہ 2، مشق 9:** ماڈل اپ گریڈز اور کیٹلاگ ریفریش — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`  
- **حصہ 2، مشق 10:** تجزیاتی ماڈلز — `<think>` ٹیگ پارسنگ کی مثالوں کے ساتھ `phi-4-mini-reasoning`  
- **حصہ 3، مشق 4:** `createChatClient` کو OpenAI SDK کے متبادل کے طور پر، سٹریمینگ کال بیک پیٹرن دستاویزی کے ساتھ شامل کیا گیا  
- **AGENTS.md:** ٹول کالنگ، ChatClient، اور ریاضتی ماڈلز کے کوڈنگ کنونشنز شامل کیے گئے  

### تبدیل کیا گیا
- **حصہ 1:** ماڈل کیٹلاگ میں توسیع — `phi-4-mini-reasoning`, `gpt-oss-20b`, `phi-4`, `qwen2.5-7b`, `qwen2.5-coder-7b`, `whisper-large-v3-turbo` شامل کیے گئے  
- **حصہ 2:** API حوالہ جاتی جدولوں کو بڑھایا گیا — شامل کیا گیا `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`  
- **حصہ 2:** مشقوں کی نمبرنگ 7-9 → 10-13 نئی مشقوں کے مطابق کی گئی  
- **حصہ 3:** کلیدی نکات کے جدول میں اندرونی ChatClient شامل کیا گیا  
- **README.md:** حصہ 11 کا سیکشن کوڈ سیمپل جدول کے ساتھ شامل کیا گیا؛ سیکھنے کا مقصد #11 شامل کیا گیا؛ پروجیکٹ کی ساخت کی ٹری اپ ڈیٹ کی گئی  
- **csharp/Program.cs:** CLI روٹر میں `toolcall` کیس شامل کیا گیا اور مدد کی تحریر اپ ڈیٹ کی گئی  

---

## 2026-03-09 — SDK v0.9.0 اپ ڈیٹ، برٹش انگلش، اور ویلیڈیشن پاس

### تبدیل کیا گیا
- **تمام کوڈ سیمپلز (Python, JavaScript, C#):** Foundry Local SDK v0.9.0 API کے مطابق اپ ڈیٹ کیے گئے — `await catalog.getModel()` درست کیا گیا (غائب `await` شامل کیا گیا)، `FoundryLocalManager` کے انیشیئلائزیشن پیٹرنز کو اپ ڈیٹ کیا گیا، اینڈ پوائنٹ ڈسکوری درست کی گئی  
- **تمام لیب گائیڈز (حصے 1-10):** برٹش انگلش میں تبدیل کیے گئے (colour، catalogue، optimised، وغیرہ)  
- **تمام لیب گائیڈز:** SDK کوڈ مثالیں v0.9.0 API کے مطابق اپ ڈیٹ کی گئیں  
- **تمام لیب گائیڈز:** API حوالہ جاتی جدولوں اور مشقوں کے کوڈ بلاکس کو اپ ڈیٹ کیا گیا  
- **JavaScript اہم فکس:** `catalog.getModel()` میں غائب `await` شامل کیا گیا — یہ پہلے `Promise` دیتا تھا، `Model` نہیں، جس کی وجہ سے خاموش خرابیاں ہوتی تھیں  

### ویلیڈیٹ کیا گیا
- تمام Python سیمپلز Foundry Local سروس پر کامیابی سے چلتے ہیں  
- تمام JavaScript سیمپلز کامیابی سے چلتے ہیں (Node.js 18+ پر)  
- C# پروجیکٹ .NET 9.0 پر بنتا اور چلتا ہے (net8.0 SDK اسمبلی سے فارورڈ کمپٹیبیلٹی)  
- ورکشاپ میں 29 فائلوں میں ترمیم اور ویلیڈیشن کی گئی  

---

## فائل انڈیکس

| فائل | آخری تازہ کاری | تفصیل |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | ماڈل کیٹلاگ میں توسیع |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | نئی مشقیں 7-10، API جدولوں میں توسیع |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | نئی مشق 4 (ChatClient)، اپ ڈیٹ شدہ نکات |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + برٹش انگلش |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + برٹش انگلش |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + برٹش انگلش |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + برٹش انگلش |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | مرمیڈ ڈایاگرام |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + برٹش انگلش |
| `labs/part10-custom-models.md` | 2026-03-11 | مرمیڈ ڈایاگرام، ورکشاپ مکمل حصہ 11 میں منتقل کیا گیا |
| `labs/part11-tool-calling.md` | 2026-03-11 | نیا لیب، مرمیڈ ڈایاگرامز، ورکشاپ مکمل سیکشن |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | نیا: ٹول کالنگ کی مثال |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | نیا: ٹول کالنگ کی مثال |
| `csharp/ToolCalling.cs` | 2026-03-10 | نیا: ٹول کالنگ کی مثال |
| `csharp/Program.cs` | 2026-03-10 | `toolcall` CLI کمانڈ شامل کی گئی |
| `README.md` | 2026-03-10 | حصہ 11، پروجیکٹ ساخت |
| `AGENTS.md` | 2026-03-10 | ٹول کالنگ + ChatClient کنونشنز |
| `KNOWN-ISSUES.md` | 2026-03-11 | حل شدہ مسئلہ #7 ہٹا دیا گیا، 6 مسائل کھلے ہیں |
| `csharp/csharp.csproj` | 2026-03-11 | کراس-پلیٹ فارم TFM، WinML/base SDK مشروط حوالہ جات |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | کراس-پلیٹ فارم TFM، خودکار RID شناخت |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | کراس-پلیٹ فارم TFM، خودکار RID شناخت |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU try/catch workaround ہٹا دیا گیا |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU try/catch workaround ہٹا دیا گیا |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU try/catch workaround ہٹا دیا گیا |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU try/catch workaround ہٹا دیا گیا |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU try/catch workaround ہٹا دیا گیا |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | کراس-پلیٹ فارم .csproj مثال |
| `AGENTS.md` | 2026-03-11 | C# پیکجز اور TFM کی تفصیلات اپ ڈیٹ کی گئیں |
| `CHANGELOG.md` | 2026-03-11 | یہ فائل |