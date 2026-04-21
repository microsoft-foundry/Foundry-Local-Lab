# کوڈنگ ایجنٹ ہدایات

یہ فائل AI کوڈنگ ایجنٹس (GitHub Copilot, Copilot Workspace, Codex، وغیرہ) کے لیے اس ریپوزیٹری میں کام کرنے کے لیے سیاق و سباق فراہم کرتی ہے۔

## منصوبے کا جائزہ

یہ [Foundry Local](https://foundrylocal.ai) کے ساتھ AI ایپلیکیشنز بنانے کے لیے ایک **عملی ورکشاپ** ہے — ایک ہلکی پھلکی رن ٹائم جو زبان کے ماڈلز کو مکمل طور پر آلے پر ہی ڈاؤن لوڈ، منظم اور OpenAI-مطابق API کے ذریعے فراہم کرتا ہے۔ ورکشاپ میں مرحلہ وار لیب گائیڈز اور چلانے کے قابل کوڈ نمونے Python، JavaScript، اور C# میں شامل ہیں۔

## ریپوزیٹری کی ساخت

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

## زبان اور فریم ورک کی تفصیلات

### Python
- **مقام:** `python/`, `zava-creative-writer-local/src/api/`
- **انحصار:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **اہم پیکجز:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **کم از کم ورژن:** Python 3.9+
- **چلائیں:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **مقام:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **انحصار:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **اہم پیکجز:** `foundry-local-sdk`, `openai`
- **ماڈیول سسٹم:** ES ماڈیولز (`.mjs` فائلیں، `"type": "module"`)
- **کم از کم ورژن:** Node.js 18+
- **چلائیں:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **مقام:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **پروجیکٹ فائلز:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **اہم پیکجز:** `Microsoft.AI.Foundry.Local` (ونڈوز کے علاوہ), `Microsoft.AI.Foundry.Local.WinML` (ونڈوز — QNN EP کے ساتھ سپر سیٹ), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **مقصد:** .NET 9.0 (شرطی TFM: `net9.0-windows10.0.26100` ونڈوز کے لیے، `net9.0` دیگر جگہوں کے لیے)
- **چلائیں:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## کوڈنگ کنونشنز

### عمومی
- تمام کوڈ نمونے **خود مختار ایک فائل کے مثالیں** ہیں — کوئی مشترکہ یوٹیلیٹی لائبریریاں یا تجریدات نہیں۔
- ہر نمونہ اپنی انحصار کو انسٹال کرنے کے بعد آزادانہ طور پر چلتا ہے۔
- API کیز ہمیشہ `"foundry-local"` پر سیٹ کی جاتی ہیں — Foundry Local اسے ایک پلیسبہولڈر کے طور پر استعمال کرتا ہے۔
- بیس URLs `http://localhost:<port>/v1` استعمال کرتے ہیں — پورٹ متغیر ہوتا ہے اور رن ٹائم پر SDK کے ذریعے دریافت کیا جاتا ہے (`manager.urls[0]` جاوا اسکرپٹ میں، `manager.endpoint` پائتھون میں)۔
- Foundry Local SDK سروس کی اسٹارٹ اپ اور اینڈپوائنٹ کی دریافت کو سنبھالتا ہے؛ ہارڈ کوڈڈ پورٹس کی بجائے SDK کے پیٹرنز کو ترجیح دیں۔

### Python
- `openai` SDK کو `OpenAI(base_url=..., api_key="not-required")` کے ساتھ استعمال کریں۔
- SDK کے ذریعے سروس لائف سائیکل کے انتظام کے لیے `foundry_local` سے `FoundryLocalManager()` استعمال کریں۔
- اسٹریمنگ: `stream` آبجیکٹ پر 반복 کریں `for chunk in stream:` کے ذریعے۔
- نمونہ فائلوں میں کوئی ٹائپ اینوٹیشنز نہیں (ورکشاپ میں سیکھنے والوں کے لیے نمونے مختصر رکھیں)۔

### JavaScript
- ES ماڈیول نحو: `import ... from "..."`۔
- `OpenAI` کو `"openai"` سے اور `FoundryLocalManager` کو `"foundry-local-sdk"` سے استعمال کریں۔
- SDK کا آغاز: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`۔
- اسٹریمنگ: `for await (const chunk of stream)`۔
- ٹاپ لیول `await` ہر جگہ استعمال ہوتا ہے۔

### C#
- Nullable فعال، ضمنی usings، .NET 9۔
- SDK کے ذریعے لائف سائیکل مینیجمنٹ کے لیے `FoundryLocalManager.StartServiceAsync()` استعمال کریں۔
- اسٹریمنگ: `CompleteChatStreaming()` کے ساتھ `foreach (var update in completionUpdates)`۔
- مرکزی `csharp/Program.cs` ایک CLI روٹر ہے جو سٹیٹک `RunAsync()` میتھڈز کو کال کرتا ہے۔

### ٹول کالنگ
- صرف مخصوص ماڈلز ٹول کالنگ کو سپورٹ کرتے ہیں: **Qwen 2.5** فیملی (`qwen2.5-*`) اور **Phi-4-mini** (`phi-4-mini`)۔
- ٹول اسکیمے OpenAI فنکشن-کالنگ JSON فارمیٹ کی پیروی کرتے ہیں (`type: "function"`, `function.name`, `function.description`, `function.parameters`)۔
- مکالمہ ایک ملٹی ٹرن پیٹرن استعمال کرتا ہے: صارف → اسسٹنٹ (tool_calls) → ٹول (نتائج) → اسسٹنٹ (حتمی جواب)۔
- ٹول رزلٹ پیغامات میں `tool_call_id` ماڈل کے ٹول کال کے `id` سے میل کھانا چاہیے۔
- Python سیدھا OpenAI SDK استعمال کرتا ہے؛ JavaScript SDK کا مقامی `ChatClient` (`model.createChatClient()`) استعمال کرتا ہے؛ C# OpenAI SDK میں `ChatTool.CreateFunctionTool()` استعمال کرتا ہے۔

### ChatClient (مقامی SDK کلائنٹ)
- JavaScript: `model.createChatClient()` ایک `ChatClient` دیتا ہے جس میں `completeChat(messages, tools?)` اور `completeStreamingChat(messages, callback)` شامل ہیں۔
- C#: `model.GetChatClientAsync()` ایک معیاری `ChatClient` دیتا ہے جسے OpenAI NuGet پیکیج درآمد کیے بغیر استعمال کیا جا سکتا ہے۔
- Python کا کوئی مقامی ChatClient نہیں ہے — OpenAI SDK استعمال کریں `manager.endpoint` اور `manager.api_key` کے ساتھ۔
- **اہم:** JavaScript میں `completeStreamingChat` ایک **کال بیک پیٹرن** استعمال کرتا ہے، async iteration نہیں۔

### استدلالی ماڈلز
- `phi-4-mini-reasoning` اپنے سوچنے کو `<think>...</think>` ٹیگز میں لپیٹتا ہے حتمی جواب سے پہلے۔
- جب ضرورت ہو تو جواب سے استدلال الگ کرنے کے لیے ٹیگز کو پارس کریں۔

## لیب گائیڈز

لیب فائلیں `labs/` میں مارک ڈاؤن میں ہیں۔ وہ ایک مستقل ساخت کی پیروی کرتی ہیں:
- لوگو ہیڈر امیج
- عنوان اور مقصد کال آؤٹ
- جائزہ، سیکھنے کے مقاصد، ضروریات
- تصور کی وضاحتی سیکشنز کے ساتھ خاکے
- نمبر دار مشقیں کوڈ بلاکس اور متوقع آؤٹ پٹ کے ساتھ
- خلاصہ جدول، کلیدی نکات، مزید مطالعہ
- اگلے حصے کے لیے نیویگیشن لنک

لیب مواد میں ترمیم کرتے وقت:
- موجودہ مارک ڈاؤن فارمیٹنگ اسٹائل اور سیکشن ہیرارکی برقرار رکھیں۔
- کوڈ بلاکس میں زبان (python, javascript, csharp, bash, powershell) کی وضاحت کریں۔
- شیل کمانڈز کے لیے bash اور PowerShell دونوں ورژن فراہم کریں جب OS کا فرق ہو۔
- > **نوٹ:**، > **ٹپ:**، > **مسائل کا حل:** کال آؤٹ اسٹائل استعمال کریں۔
- جدول `| ہیڈر | ہیڈر |` پائپ فارمیٹ استعمال کرتے ہیں۔

## تعمیر اور جانچنے کے کمانڈز

| عمل | کمانڈ |
|--------|---------|
| **Python نمونے** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JavaScript نمونے** | `cd javascript && npm install && node <script>.mjs` |
| **C# نمونے** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (ویب)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (ویب)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **خاکے جنریٹ کریں** | `npx mmdc -i <input>.mmd -o <output>.svg` (مطلوبہ ہے root `npm install`) |

## بیرونی انحصار

- **Foundry Local CLI** ڈویلپر کی مشین پر انسٹال ہونا ضروری ہے (`winget install Microsoft.FoundryLocal` یا `brew install foundrylocal`)۔
- **Foundry Local سروس** لوکل چلتی ہے اور ایک OpenAI-مطابق REST API ایک متحرک پورٹ پر فراہم کرتی ہے۔
- کسی بھی نمونے کو چلانے کے لیے کلاؤڈ سروسز، API کیز، یا Azure سبسکرپشنز کی ضرورت نہیں۔
- حصہ 10 (اپنی ماڈلز) میں اضافی طور پر `onnxruntime-genai` اور Hugging Face سے ماڈل ویٹس کی ڈاؤن لوڈنگ چاہیے۔

## ایسے فائلیں جو جمع نہیں کرنی چاہئیں

`.gitignore` اکثر مندرجہ ذیل کو خارج کرتا ہے (اور اکثر کرتا ہے):
- `.venv/` — Python ورچوئل انوائرنمنٹس
- `node_modules/` — npm انحصار
- `models/` — کمپائل شدہ ONNX ماڈل آؤٹ پٹ (بڑے بائنری فائلز، حصہ 10 کی پیداوار)
- `cache_dir/` — Hugging Face ماڈل ڈاؤن لوڈ کیشے
- `.olive-cache/` — Microsoft Olive ورکنگ ڈائریکٹری
- `samples/audio/*.wav` — جنریٹ کیے گئے آڈیو نمونے (`python samples/audio/generate_samples.py` سے دوبارہ پیداکیے جاتے ہیں)
- معیاری Python تعمیر آرٹیفیکٹس (`__pycache__/`, `*.egg-info/`, `dist/` وغیرہ)

## لائسنس

MIT — دیکھیں `LICENSE`۔