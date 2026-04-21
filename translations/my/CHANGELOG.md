# Changelog — Foundry Local Workshop

အဆိုပါ workshop တွင် ထင်ရှားစွာ ပြောင်းလဲမှုများအားလုံးကို အောက်တွင် မှတ်တမ်းတင်ထားပါသည်။

---

## 2026-03-11 — အပိုင်း 12 & 13, Web UI, Whisper ပြန်ရေးခြင်း, WinML/QNN ပြုပြင်ခြင်းနှင့် အတည်ပြုခြင်း

### ထည့်သွင်းခဲ့သည်
- **အပိုင်း 12: Zava Creative Writer အတွက် Web UI တည်ဆောက်ခြင်း** — ဝင်ရောက်လေ့လာရန် အသစ် lab ညွှန်ကြားချက် (`labs/part12-zava-ui.md`) ကို NDJSON စီးမြောင်း, browser `ReadableStream`, သက်ဆိုင်ရာ ကိုယ်စားလှယ်အဆင့်အတန်းဘက်တံများနှင့် မြန်ဆန်စွာ အကြောင်းအရာစာသား စီးမြောင်းခြင်းတို့ ပါဝင်သော အမှုသင်ခန်းစာများနှင့်အတူ
- **အပိုင်း 13: Workshop အပြီးသတ်မှု** — အပိုင်း ၁၂ ဆက်တိုက်ပြီးပြီးခဲ့သည့် lab အနှစ်ချုပ် (`labs/part13-workshop-complete.md`) သို့ အကြံဉာဏ်အသစ်များနှင့် အရင်းအမြစ် လင့်ခ်များပါဝင်
- **Zava UI ရဲ့ frontend:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — ဘရောက်ဇာအတွက် vanilla HTML/CSS/JS အင်တာဖေ့စ် သုံးပြုမှု သုံး backend အားလုံးမှ
- **JavaScript HTTP server:** `zava-creative-writer-local/src/javascript/server.mjs` — ဤမှာ Express စတိုင် HTTP server အသစ် ဖြစ်ပြီး orchestrator ကို ဘရောက်ဇာမတော်တဆင့် ဝင်ရောက် အသုံးပြုခွင့်ရပေးသည်
- **C# ASP.NET Core backend:** `zava-creative-writer-local/src/csharp-web/Program.cs` နှင့် `ZavaCreativeWriterWeb.csproj` — UI နှင့် NDJSON စီးမြောင်းကို ပေးပို့သည့် လျှပ်စစ် API စီမံကိန်းအသစ်
- **အသံနမူနာ ထုတ်လုပ်သူ:** `samples/audio/generate_samples.py` — TTS script (offline) `pyttsx3` ကို အသုံးပြု၍ Part 9 အတွက် Zava ခံစားမှု WAV ဖိုင်များ ထုတ်လုပ်ရန်
- **အသံနမူနာ:** `samples/audio/zava-full-project-walkthrough.wav` — အသံနမူနာ အချိန်ပိုင်း နည်းများ သွင်းစစ်ဆေးရန် အသစ်
- **အတည်ပြုသော script:** `validate-npu-workaround.ps1` — C# နမူနာအားလုံးတွင် NPU/QNN workaround ကို အသုံးပြုပြီး အလိုအလျောက် PowerShell script ဖြင့် စစ်ဆေးခြင်း
- **Mermaid diagram SVG များ:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML cross-platform ထောက်ပံ့မှု:** C# `.csproj` ဖိုင်များ သုံးခု (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) တွင် အခြေအနေ TFM နှင့် ထိခိုက်နိုင်သော အထုပ်အညွှန်းများ မတွဲလျှောက်ဘဲ cross-platform ထောက်ပံ့မှုရှိအောင် ပြင်ဆင်ထားသည်။ Windows တွင်: `net9.0-windows10.0.26100` TFM နှင့် `Microsoft.AI.Foundry.Local.WinML` (QNN EP plugin အပါအဝင် superset) ကို အသုံးပြုသည်။ Windows မဟုတ်သော စက်များတွင်: `net9.0` TFM နှင့် `Microsoft.AI.Foundry.Local` (အခြေခံ SDK) အသုံးပြုသည်။ Zava စီမံကိန်းများတွင် hardcoded `win-arm64` RID ကို အလိုအလျောက် ရှာဖွေတင်ပြရန် ပြောင်းလဲခဲ့သည်။ `Microsoft.ML.OnnxRuntime.Gpu.Linux` ထဲမှ native အမြစ်များကို ထုတ်ပယ်ရန် transitive dependency workaround ပါဝင်ပြီး၊ win-arm64 ရွေ့မကောင်းသော reference ဖြစ်နေသော အရာကို ဖြန့်ချိကာ ဖြေရှင်း။ ယခင် try/catch NPU workaround ကို C# ဖိုင် ၇ ခုလုံးမှ ဖယ်ရှားပြီးဖြစ်သည်။

### ပြောင်းလဲခဲ့သည်
- **အပိုင်း 9 (Whisper):** အရေးကြီး ပြန်ရေးခြင်း — JavaScript တွင် SDK ရဲ့ သဘာဝ `AudioClient` (`model.createAudioClient()`) ကို အသုံးပြုပြီး ONNX Runtime inference ကို မျက်နှာကင်း စနစ်ဖြင့် အစားထိုးခဲ့ပြီး၊ အင်ဂျင်နီယာ ဖော်ပြချက်များ၊ နှိုင်းယှဉ်ဇယားများနှင့် pipeline နမူနာများကို JS/C# `AudioClient` နည်းလမ်းနှင့် Python ONNX Runtime နည်းလမ်းများကို ဆက်တိုက်ထုတ်ပြန်ခဲ့သည်။
- **အပိုင်း 11:** လမ်းညွှန်လင့်ခ်များ ပြင်ဆင်ထားပြီး (အခု အပိုင်း 12 ကိုပြသသည်) ။ tool-calling flow နှင့် sequence အတွက် SVG ဖော်ပြချက်များ ထည့်သွင်းထားသည်။
- **အပိုင်း 10:** နောက်ဆုံး workshop ဖြစ်စဉ်မှ အပိုင်း 12 သို့ လမ်းကြောင်းပြောင်းသည်။
- **Python Whisper (`foundry-local-whisper.py`):** အသံနမူနာများ အသစ်ထပ်ထည့်ပြီး အမှားစီမံချက်များ တိုးတက်စေသည်။
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** `model.createAudioClient()` နှင့် `audioClient.transcribe()` ကို အသုံးပြုရန် ပြန်ရေးပြီး ONNX Runtime session ကို ထုတ်ပယ်သည်။
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** API နှင့် static UI ဖိုင်များ တပြိုင်တည်း ပေးဆောင်ရန် ပြန်လည် ပြင်ဆင်ထားသည်။
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU workaround ကို ဖယ်ရှားပြီး (ယခု WinML package က ဆောင်ရွက်သည်)။
- **README.md:** အပိုင်း 12 အပိုင်းကို အားကောင်းစွာ နှင့် backend တွင် ထည့်သွင်းထားပြီး အပိုင်း 13 အပိုင်းကိုထည့်သွင်းထားသည်၊ သင်ယူရမည့် ရည်ရွယ်ချက်များနှင့် စီမံကိန်း ဖွဲ့စည်းမှုအသစ်များ ဖြစ်စေသည်။
- **KNOWN-ISSUES.md:** ဖြေရှင်းပြီး Issue #7 (C# SDK NPU Model Variant — ယခု WinML package က ဆောင်ရွက်) ကို ဖယ်ရှားပြီး အခြား Issue များကို #1–#6 အပြောင်းအလဲပြုလုပ်သည်။ .NET SDK 10.0.104 နှင့် ပတ်သက်၍ ပတ်ဝန်းကျင် အသေးစိတ်အချက်အလက်များ ပြန်လည်ပြုပြင်ထားသည်။
- **AGENTS.md:** စီမံကိန်း ဖွဲ့စည်းမှု အမြင့်ဆုံးထောက်ခံချက်ဖြင့် `zava-creative-writer-local` အသစ်များ (`ui/`, `csharp-web/`, `server.mjs`) ထည့်သွင်း။ C# 주요 ထုပ်ပိုးမှုများနှင့် အခြေအနေ TFM အသေးစိတ် ပြင်ဆင်ထားသည်။
- **labs/part2-foundry-local-sdk.md:** `.csproj` နမူနာကို အပြည့်အစုံ cross-platform ပုံစံနှင့် အခြေအနေ TFM, ထိခိုက်နိုင်သော ထုပ်ပိုးမှုများ၊ ရှင်းပြချက် အသေးစိတ်ဖြင့် ပြန်ပြင်ထားသည်။

### အတည်ပြုထားသည်
- C# စီမံကိန်းသုံးခု (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) အသီးသီး Windows ARM64 တွင် အောင်မြင်စွာ ဆောက်လုပ်နိုင်သည်။
- Chat နမူနာ (`dotnet run chat`): ပုံစံကို WinML/QNN ဖြင့် `phi-3.5-mini-instruct-qnn-npu:1` အဖြစ် loading ပြုလုပ်ပြီး၊ CPU fallback မပါဘဲ အနည်းဆုံး NPU variant ပြီးစီး၏။
- Agent နမူနာ (`dotnet run agent`): မပြတ်မနပ် multi-turn စကားပြောဆက်သွယ်မှုဖြင့် အဆုံးသတ်ပြီး exit code 0 ကို ထုတ်ပေးသည်။
- Foundry Local CLI v0.8.117 နှင့် SDK v0.9.0 သည် .NET SDK 9.0.312 ပေါ်တွင် အောင်မြင်စွာ လည်ပတ်ပါသည်။

---

## 2026-03-11 — ကုဒ် ပြင်ဆင်မှုများ၊ မော်ဒယ် သန့်ရှင်းမှု၊ Mermaid diagrams နှင့် အတည်ပြုခြင်း

### ပြင်ဆင်ခဲ့သည်
- **ကုဒ်နမူနာ ၂၁ ခုလုံး (Python ၇ ခု၊ JavaScript ၇ ခု၊ C# ၇ ခု):** ထွက်သွားမှုအတွက် `model.unload()` / `unload_model()` / `model.UnloadAsync()` ကို ထည့်သွင်းပြီး OGA မှတ်ဉာဏ်ထိန်းမှု ကြောင့် ဖြစ်ပေါ်သည့် သတိပေးချက်များ ဖြေရှင်းထားသည် (သိရှိပြီးသော ပြဿနာ#4)
- **csharp/WhisperTranscription.cs:** မတည်ကြည်သော `AppContext.BaseDirectory` ဆက်စပ်လမ်းကြောင်းကို `FindSamplesDirectory()` ဖြင့် ပြောင်းလဲခဲ့သည်။ သည် function သည် directory များကို ဦးတည်၍ `samples/audio` ကို တွေ့ရှိရန် လမ်းညွှန်ပါသည် (သိရှိပြီးသော ပြဿနာ #7)
- **csharp/csharp.csproj:** `win-arm64` ကို ဤနေရာတွင် hardcodedထားသော `<RuntimeIdentifier>` ကို auto-detect fallback ဖြစ်ပြီး `$(NETCoreSdkRuntimeIdentifier)` အသုံးပြုမှုဖြင့် `dotnet run` သည် platform မရွေး လုပ်ဆောင်နိုင်ရန် ပြုပြင်ခဲ့သည် ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### ပြောင်းလဲခဲ့သည်
- **အပိုင်း 8:** ASCII box diagram ဖြင့် ပြုလုပ်ထားသော eval-driven iteration loop ကို SVG ပုံတူပုံရိပ်သို့ ပြောင်းပြန်ခဲ့သည်
- **အပိုင်း 10:** ASCII မြှားပုံများဖြင့် ဖော်ပြထားသော compilation pipeline ကို SVG ပုံတူ ပုံရိပ်သို့ ပြောင်းပြန်ခဲ့သည်
- **အပိုင်း 11:** tool-calling flow နှင့် sequence diagrams အား အပြည့်အဝ SVG ပုံဖော်ပြီး ပြောင်းလဲတင်ပြခဲ့သည်
- **အပိုင်း 10:** "Workshop Complete!" အပိုင်းကို အပိုင်း 11 (နောက်ဆုံး lab) သို့ ဦးတည်ပြောင်းလဲခဲ့ပြီး "Next Steps" လင့်ခ်ဖြင့် အစားထိုးခဲ့သည်
- **KNOWN-ISSUES.md:** CLI v0.8.117 နှင့် တစ်ပြိုင်နက် ပြန်လည်စစ်ဆေးမှု ပြုလုပ်ပြီး ဖြေရှင်းပြီး ပြဿနာများျဖစ္သော OGA Memory Leak (ထိန်းသိမ်းမှု ထည့်သွင်းမှု), Whisper လမ်းကြောင်း (FindSamplesDirectory), HTTP 500 inference များ(ပြန်ဖြေရှင်းဖို့ မဖြစ်ရ), tool_choice ကန့်သတ်ချက်များ (ယခု `"required"` နှင့် function ကို ရှာဖွေရန် qwen2.5-0.5b တွင်အလုပ်လုပ်သည်) ကို ဖယ်ရှားသည်။ JS Whisper ပြဿနာကို အသစ်မွမ်းမံပြောင်းလဲပြီး ဖိုင်များအားလုံးသည် empty/binary output ထုတ်သည် (v0.9.x မှ regression ဖြစ်ပြီး severity ကို အရေးကြီးအဖြစ် တိုးမြှင့်သည်)။ C# RID #4 သည့် auto-detect workaround နှင့် [#497](https://github.com/microsoft/Foundry-Local/issues/497) ကို ထည့်သွင်းခဲ့သည်။ ဖွင့်ထားသည့် ပြဿနာ ၇ ခု ကျန်ရှိသည်။
- **javascript/foundry-local-whisper.mjs:** သန့်ရှင်းရေး မတိမ်းညွတ်သော variable name (`whisperModel` → `model`) ကို ပြင်ဆင်ခဲ့သည်။

### အတည်ပြုသည်
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — ထုတ်လုပ်မှု သန့်ရှင်းချက်နှင့်အဆင်ပြေစွာ လည်ပတ်သည်။
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — ထုတ်လုပ်မှု သန့်ရှင်းချက်နှင့်အဆင်ပြေစွာ လည်ပတ်သည်။
- C#: `dotnet build` ပြီးစီးပြီး သတိပေးချက် ၀၊ အမှား ၀ (net9.0 target)
- Python ဖိုင် ၇ ခုလုံးသည် `py_compile` syntax စစ်ဆေးမှု ဖြတ်သန်းသည်။
- JavaScript ဖိုင် ၇ ခုလုံးသည် `node --check` syntax အတည်ပြုချက် ဖြတ်သန်းသည်။

---

## 2026-03-10 — အပိုင်း 11: Tool Calling, SDK API တိုးချဲ့ခြင်းနှင့် မော်ဒယ် အကွာအဝေး

### ထည့်သွင်းခဲ့သည်
- **အပိုင်း 11: ဒေသတွင်း မော်ဒယ်များဖြင့် Tool Calling** — အသစ် lab ညွှန်ကြားချက် (`labs/part11-tool-calling.md`) တွင် tool schemas, multi-turn flow, အထပ်တလွဲ tool call များ, အထူး tool များ, ChatClient tool calling နှင့် `tool_choice` ကို အပါအဝင် ၈ ခု အမှု သင်ခန်းစာများပါဝင်သည်။
- **Python နမူနာ:** `python/foundry-local-tool-calling.py` — OpenAI SDK အသုံးပြု၍ `get_weather`/`get_population` tools ဖြင့် tool calling
- **JavaScript နမူနာ:** `javascript/foundry-local-tool-calling.mjs` — SDK native `ChatClient` (`model.createChatClient()`) အသုံးပြု၍ tool calling
- **C# နမူနာ:** `csharp/ToolCalling.cs` — OpenAI C# SDK ဖြင့် `ChatTool.CreateFunctionTool()` အသုံးပြု tool calling
- **အပိုင်း 2, အပြုအမှု 7:** Native `ChatClient` — JS တွင် `model.createChatClient()` နှင့် C# တွင် `model.GetChatClientAsync()` ကို OpenAI SDK အစား အဖြစ်သုံးခြင်း
- **အပိုင်း 2, အပြုအမှု 8:** မော်ဒယ် မျိုးစုံ နှင့် hardware ရွေးချယ်မှု — `selectVariant()`, `variants`, NPU variant ဇယား (မော်ဒယ် ၇ ခု)
- **အပိုင်း 2, အပြုအမှု 9:** မော်ဒယ် နယူးညီမှုနှင့် ကတ်တလော့ ပြန်လည် အဖြစ်သစ်ခြင်း — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **အပိုင်း 2, အပြုအမှု 10:** သံသယကြည့်ရှု မော်ဒယ်များ — `<think>` tag parsing စမ်းသပ်ခြင်းနှင့် `phi-4-mini-reasoning`
- **အပိုင်း 3, အပြုအမှု 4:** OpenAI SDK အစား `createChatClient` ကို streaming callback pattern နှင့် သတ်မှတ်ချက်၊ မှတ်စုအဖြစ် အသုံးပြုခြင်း
- **AGENTS.md:** Tool Calling, ChatClient, Reasoning Models အတွက် ကုဒ်ရေးသားမှု စံနှုန်းများ ထည့်သွင်းထားသည်။

### ပြောင်းလဲခဲ့သည်
- **အပိုင်း 1:** မော်ဒယ် ကတ်တလော့ ထပ်ဆောင်း — phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo သို့ ထည့်သွင်း။
- **အပိုင်း 2:** API ရည်ညွှန်းဇယားများ ချဲ့ထွင် — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **အပိုင်း 2:** အပြုအမှုများ 7-9 ကို 10-13 သို့ ပြောင်းလဲ ချိန်ညှိခြင်း (အသစ်ထည့်သွင်းရန်)
- **အပိုင်း 3:** အဓိက ရရှိချက်ဇယားတွင် Native ChatClient ကို ထည့်သွင်း ပြင်ဆင်ထားသည်။
- **README.md:** အပိုင်း 11 အပိုင်းနှင့် ကုဒ်နမူနာဇယား ထည့်သွင်းပြီး သင်ယူရန် ရည်ရွယ်ချက် #11 ထည့်သွင်းထားသည်။ စီမံကိန်းဖွဲ့စည်းမှု သစ်များ ပြင်ဆင်ထားသည်။
- **csharp/Program.cs:** CLI router တွင် `toolcall` အတည်ပြုထားပြီး အကူအညီ စာသား ပုံစံ ပြင်ဆင်ထားသည်။

---

## 2026-03-09 — SDK v0.9.0 အပ်ဒိတ်, ဘရစ်တိန် အင်္ဂလိပ်စကားနှင့် အတည်ပြုခြင်း

### ပြောင်းလဲခဲ့သည်
- **ကုဒ်နမူနာများအားလုံး (Python, JavaScript, C#):** Foundry Local SDK v0.9.0 API ပြောင်းလဲမှုနှင့် လိုက်ဖက်စေရန် — `await catalog.getModel()` မြောက်မှုတွင် `await` မပါရှိခြင်း ပြင်ဆင်ပြီး၊ `FoundryLocalManager` init ပုံစံများ ပြုပြင်၊ endpoint ရှာဖွေမှု ပြင်ဆင်ပြီး။
- **အခန်းသင်ကြားချက်များအားလုံး (အပိုင်း 1-10):** ဘရစ်တိန်အင်္ဂလိပ် စက်ဘာသာသို့ ပြောင်းလဲထားသည်။ (ဥပမာ: colour, catalogue, optimised စသည်)
- **အခန်းသင်ကြားချက်များအားလုံး:** SDK ကုဒ် နမူနာများ v0.9.0 API ဖြင့် ပြင်ဆင်ထားသည်။
- **အခန်းသင်ကြားချက်များအားလုံး:** API ရည်ညွှန်းဇယားများနှင့် အမှုသင်ခန်းစာ ကုဒ် blocks တွေအား ပြင်ဆင်ထားသည်။
- **JavaScript အရေးကြီး ပြင်ဆင်မှု:** `catalog.getModel()` တွင် `await` မျှော်လင့်ချက် မပါရှိသော ပြဿနာ ဖြေရှင်းထားပြီး downstream မှာ တိတ်တိတ်လေး မအောင်မြင်မှု ဖြစ်စေသော ပြဿနာဖြစ်သည်။

### အတည်ပြုသည်
- Python နမူနာအားလုံးမှာ Foundry Local service တွင် အောင်မြင်စွာ လည်ပတ်မှုရှိသည်
- JavaScript နမူနာအားလုံး Node.js 18+ ပေါ်တွင် အောင်မြင်စွာ လည်ပတ်သည်
- C# စီမံကိန်းသည် .NET 9.0 တွင် အသစ်တည်ဆောက်ပြီးလည်ပတ်သည် (net8.0 SDK assembly မှ အနာဂတ်အောက်တွင် ဖြောင့်မတ်မှုရှိ)
- ၂၉ ဖိုင် အသစ်ပြင်ပြီး workshop တစ်ခုလုံးတွင် အတည်ပြုထားသည်

---

## ဖိုင် စာရင်း

| ဖိုင် | နောက်ဆုံး ပြင်ဆင်သည့်နေ့ | ဖော်ပြချက် |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | မော်ဒယ် ကတ်တလော့ ချဲ့ထွင်ခြင်း |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | အပြုအမှုအသစ် 7-10, API ဇယား ချဲ့ထွင်ခြင်း |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | အပြုအမှုအသစ် 4 (ChatClient), ရရှိချက် ပြင်ဆင်ခြင်း |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + ဘရစ်တိန် အင်္ဂလိပ် |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + ဘရစ်တိန် အင်္ဂလိပ် |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid diagram |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid diagram, moved Workshop Complete to Part 11 |
| `labs/part11-tool-calling.md` | 2026-03-11 | New lab, Mermaid diagrams, Workshop Complete section |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | အသစ်: ကိရိယာ ဖုန်းခေါ်နမူနာ |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | အသစ်: ကိရိယာ ဖုန်းခေါ်နမူနာ |
| `csharp/ToolCalling.cs` | 2026-03-10 | အသစ်: ကိရိယာ ဖုန်းခေါ်နမူနာ |
| `csharp/Program.cs` | 2026-03-10 | `toolcall` CLI အမိန့် ထည့်သွင်းပြီးဖြစ်သည် |
| `README.md` | 2026-03-10 | အပိုင်း 11၊ စီမံကိန်းဖွဲ့စည်းမှု |
| `AGENTS.md` | 2026-03-10 | ကိရိယာ ဖုန်းခေါ် + ChatClient စည်းကမ်းများ |
| `KNOWN-ISSUES.md` | 2026-03-11 | ဖြေရှင်းပြီး Issue #7 ဖယ်ရှားပြီး၊ အမှား 6 ခု ကျန်ရှိသည် |
| `csharp/csharp.csproj` | 2026-03-11 | Cross-platform TFM, WinML/base SDK အခြေအနေအရ ဆက်စပ်မှုများ |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | Cross-platform TFM, RID ကို အလိုအလျောက် စစ်ဆေးခြင်း |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | Cross-platform TFM, RID ကို အလိုအလျောက် စစ်ဆေးခြင်း |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU try/catch workaround ဖယ်ရှားပြီးဖြစ်သည် |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU try/catch workaround ဖယ်ရှားပြီးဖြစ်သည် |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU try/catch workaround ဖယ်ရှားပြီးဖြစ်သည် |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU try/catch workaround ဖယ်ရှားပြီးဖြစ်သည် |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU try/catch workaround ဖယ်ရှားပြီးဖြစ်သည် |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | Cross-platform .csproj နမူနာ |
| `AGENTS.md` | 2026-03-11 | C# ပက်ကေ့ဂျ်များနှင့် TFM အသေးစိတ် အသစ်ပြောင်းလဲချက်များ |
| `CHANGELOG.md` | 2026-03-11 | ဒီဖိုင် |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**အသိပေးချက်**:  
ဤစာရွက်ကို AI ဘာသာပြန်ဝန်ဆောင်မှုဖြစ်သော [Co-op Translator](https://github.com/Azure/co-op-translator) သုံးပြီး ဘာသာပြန်ထားပါသည်။ ကျွန်တော်တို့သည် တိကျမှန်ကန်မှုအတွက် ကြိုးပမ်းသည်မှာ ဖြစ်သော်လည်း စက်ရုပ်ဘာသာပြန်မှုများတွင် အမှားများ သို့မဟုတ် မှားယွင်းမှုများပါဝင်နိုင်ကြောင်း သတိပြုပါရန်။ မူရင်းစာရွက်ကို သည်ဘာသာစကားဖြင့် ရေးသားထားသောဇာတိစာစောင်အဖြစ် ယုံကြည်စရာအရင်းအမြစ်အဖြစ်ယူဆရမည်ဖြစ်သည်။ အရေးကြီးသော အချက်အလက်များအတွက် လူမှုဘာသာပြန်တစ်ဦးမှ အတည်ပြုထားသော ဘာသာပြန်ခြင်းကို အကြံပြုပါသည်။ ဤဘာသာပြန်မှုအား အသုံးပြုမှုကြောင့် ဖြစ်ပေါ်လာနိုင်သော နားလည်မှုမှားယွင်းမှုများ သို့မဟုတ် မှားဖတ်ခြင်းများအတွက် ကျွန်တော်တို့မှာ တာဝန် မရှိပါ။
<!-- CO-OP TRANSLATOR DISCLAIMER END -->