# Coding Agent ညွှန်ကြားချက်များ

ဤဖိုင်သည် ဤ repository တွင် အလုပ်လုပ်သော AI coding agents (GitHub Copilot, Copilot Workspace, Codex စသဖြင့်) အတွက် context ကို ပံ့ပိုးပေးသည်။

## Project အနှုတ်ချုပ်

ဤသည်မှာ [Foundry Local](https://foundrylocal.ai) ဖြင့် AI applications များ တည်ဆောက်ခြင်းအတွက် **လက်တွေ့လုပ်ငန်းသင်တန်း** တစ်ခုဖြစ်ပြီး — OpenAI-compatible API မှတဆင့် device အတွင်းရှိ language models များကို တပြိုင်နက် download, စီမံခန့်ခွဲပြီး ဝန်ဆောင်မှုပေးသည့် lightweight runtime ဖြစ်သည်။ လက်တွေ့လုပ်ငန်းသင်တန်းတွင် Python, JavaScript, နှင့် C# မှ အဆင့်ဆင့် lab လမ်းညွှန်များနှင့် အလုပ်လုပ်နိုင်သည့် code နမူနာများ ပါဝင်သည်။

## Repository ဖွဲ့စည်းမှု

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

## ဘာသာစကားနှင့် Framework အသေးစိတ်

### Python
- **တည်နေရာ။** `python/`, `zava-creative-writer-local/src/api/`
- **လိုအပ်ချက်များ။** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **အဓိက package များ။** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **အနိမ့်ဆုံး ဗားရှင်း။** Python 3.9+
- **ပြေးရန်။** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **တည်နေရာ။** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **လိုအပ်ချက်များ။** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **အဓိက package များ။** `foundry-local-sdk`, `openai`
- **Module စနစ်။** ES modules (`.mjs` ဖိုင်များ၊ `"type": "module"`)
- **အနိမ့်ဆုံး ဗားရှင်း။** Node.js 18+
- **ပြေးရန်။** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **တည်နေရာ။** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Project ဖိုင်များ။** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **အဓိက package များ။** `Microsoft.AI.Foundry.Local` (non-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — QNN EP နှင့်အတူ superset), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Target။** .NET 9.0 (conditional TFM: Windows တွင် `net9.0-windows10.0.26100`, အခြားနေရာတွင် `net9.0`)
- **ပြေးရန်။** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Coding စံနှုန်းများ

### အထွေထွေ
- စာတမ်းတွင် ဖော်ပြထားသည့် code နမူနာအားလုံးမှာ **တစ်ဖိုင်တည်း အပြည့်အစုံပါဝင်သည့် နမူနာများဖြစ်သည်** — မျှဝေသုံးသော utility libraries သို့မဟုတ် abstraction မရှိပါ။
- နမူနာတိုင်းကို ကိုယ်ပိုင်လိုအပ်ချက်များ ထည့်သွင်းပြီး လွတ်လပ်စွာ ပြေးနိုင်ပါသည်။
- API keys များသည် အမြဲ `"foundry-local"` သတ်မှတ်ထားသည် — Foundry Local မှ placeholder အဖြစ် အသုံးပြုသည်။
- Base URLs သည် `http://localhost:<port>/v1` ကို အသုံးပြုသည် — port သည် runtime တွင် SDK (JavaScript တွင် `manager.urls[0]`, Python တွင် `manager.endpoint`) မှ dynamic ဖော်ထုတ်သည်။
- Foundry Local SDK သည် ဝန်ဆောင်မှု စတင်ခြင်းနှင့် endpoint ရှာဖွေမှုကို ကိုင်တွယ်ပေးသောကြောင့် ရိုးရိုး port hard-coded အစား SDK ပုံစံများကို မျှော်လင့်သည်။

### Python
- `openai` SDK ကို `OpenAI(base_url=..., api_key="not-required")` အဖြစ် အသုံးပြုပါ။
- SDK စီမံခန့်ခွဲမှုအတွက် `foundry_local` မှ `FoundryLocalManager()` ကို အသုံးပြုပါ။
- Streaming အတွက် `stream` object ကို `for chunk in stream:` ဖြင့် iterate လုပ်ပါ။
- နမူနာဖိုင်များ၌ type annotations မပါဝင်ပါ (လေ့ကျင့်သူများအတွက် နမူနာများကို တစ်ချက်ချင်း စာတိုဆုံးထားရန်)။

### JavaScript
- ES module စာတမ်းလမ်းစဉ်: `import ... from "..."`.
- `"openai"` မှ `OpenAI` နှင့် `"foundry-local-sdk"` မှ `FoundryLocalManager` ကို အသုံးပြုပါ။
- SDK စတင်မှုပုံစံ: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- အထက်ဆုံး `await` ကို ပုံမှန်အသုံးပြုသည်။

### C#
- Nullable enabled, implicit usings, .NET 9 စနစ်ကို အသုံးပြုသည်။
- SDK စီမံခန့်ခွဲမှုအတွက် `FoundryLocalManager.StartServiceAsync()` ကို အသုံးပြုပါ။
- Streaming: `CompleteChatStreaming()` နှင့် `foreach (var update in completionUpdates)`.
- `csharp/Program.cs` သည် CLI ရှာဖွေရေး router ဖြစ်ပြီး static `RunAsync()` မက်သောငျများသို့ ပေးပို့သည်။

### Tool Calling
- အချို့ဘဲ model များသာ tool calling ကို ထောက်ပံ့သည် — **Qwen 2.5** မိသားစု (`qwen2.5-*`) နှင့် **Phi-4-mini** (`phi-4-mini`)။
- Tool schema များသည် OpenAI function-calling JSON ပုံစံ (`type: "function"`, `function.name`, `function.description`, `function.parameters`) ကိုလိုက်နာသည်။
- စကားပြောပြောဆိုမှုမှာ multi-turn ပုံစံဖြစ်သည် — user → assistant (tool_calls) → tool (results) → assistant (နောက်ဆုံးဖြေ)။
- Tool result message များရှိ `tool_call_id` သည် model ၏ tool call မှာရှိသော `id` နှင့် ကိုက်ညီရမည်။
- Python သည် OpenAI SDK ကို တိုက်ရိုက်အသုံးပြုသည်၊ JavaScript သည် SDK ရဲ့ native `ChatClient` (`model.createChatClient()`) ကို အသုံးပြုသည်၊ C# သည် OpenAI SDK ဖြင့် `ChatTool.CreateFunctionTool()` ကို အသုံးပြုသည်။

### ChatClient (Native SDK Client)
- JavaScript: `model.createChatClient()` သည် `completeChat(messages, tools?)` နှင့် `completeStreamingChat(messages, callback)` ပါရှိသည့် `ChatClient` ကို return ပြန်သည်။
- C#: `model.GetChatClientAsync()` သည် OpenAI NuGet package တင်စရာမလိုသော စံ `ChatClient` ကို return ပြန်သည်။
- Python တွင် native ChatClient မရှိပါ — `manager.endpoint` နှင့် `manager.api_key` ဖြင့် OpenAI SDK ကို အသုံးပြုပါ။
- **အရေးကြီးချက်။** JavaScript ၏ `completeStreamingChat` သည် async iteration မဟုတ်ပဲ callback pattern အသုံးပြုသည်။

### Reasoning Models
- `phi-4-mini-reasoning` သည် နောက်ဆုံးဖြေရှင်းချက်မှာ `<think>...</think>` tag များအတွင်းတွင် စဉ်းစားချက်အပိုင်းကို ချိတ်ဆက်သည်။
- လိုအပ်ပါက tag များကို parsed ပြီး စဉ်းစားချက်နှင့် ဖြေရှင်းချက်ကို ကြားကွာစိတ်ကြားနိုင်သည်။

## Lab လမ်းညွှန်များ

Lab ဖိုင်များသည် `labs/` တွင် Markdown အနေနှင့် ရှိသည်။ အောက်ပါဖွဲ့စည်းမှုကို လိုက်နာသည်။
- ရုပ်ပုံအထူးအမှတ်အသား header ပုံတစ်ပုံ
- ခေါင်းစဉ်နှင့် ရည်မှန်းချက် callout
- အကျဉ်းချုပ်၊ သင်ယူမည့်ရည်မှန်းချက်များ၊ ကြိုတင်လိုအပ်ချက်များ
- အယူအဆရှင်းလင်းရေးအပိုင်းများနှင့် နမူနာပုံနှိပ်ချက်များ
- နံပါတ်စီအလုပ်လေ့ကျင့်ခန်းများ၊ ကုဒ်ကွက်များနှင့် မျှော်မှန်းထားသော output များ
- အကျဥ်းချုပ်ဇယား၊ အဓိကယူဆချက်များ၊ နောက်ထပ်ဖတ်ရှုသင့်မှုများ
- နောက်ပိုင်းအစိတ်အပိုင်းသို့ သွားမည့် navigation လင့်ခ်

Lab အချက်အလက်များကို ပြင်ဆင်ရာတွင်-
- ရှိပြီးသား Markdown ဖော်မှုပုံစံနှင့် အပိုင်းစဉ်ကို ထားရှိရန်
- Code blocks များတွင် ဘာသာစကားကို (`python`, `javascript`, `csharp`, `bash`, `powershell`) ထည့်သွင်းရန်
- OS မူတည်သော shell command များအတွက် bash နှင့် PowerShell နှစ်မျိုးလုံး ပေးသွင်းရန်
- `> **Note:**`, `> **Tip:**`, `> **Troubleshooting:**` ပြောကြားချက်ပုံစံများကို အသုံးပြုရန်
- ဇယားများသည် `| Header | Header |` pipe format အသုံးပြုသည်

## Build & Test အမိန့်များ

| လုပ်ဆောင်ချက် | အမိန့် |
|--------|---------|
| **Python နမူနာများ** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS နမူနာများ** | `cd javascript && npm install && node <script>.mjs` |
| **C# နမူနာများ** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Diagram ပြုလုပ်ရန်** | `npx mmdc -i <input>.mmd -o <output>.svg` (root `npm install` လိုအပ်သည်) |

## ပြင်ပလိုအပ်ချက်များ

- **Foundry Local CLI** သည် developer ၏ machine တွင် တပ်ဆင်ထားရမည် (`winget install Microsoft.FoundryLocal` သို့မဟုတ် `brew install foundrylocal`)။
- **Foundry Local ဝန်ဆောင်မှု** သည် ဒေသန္တရတွင် run ပြီး OpenAI-compatible REST API ကို dynamic port မှတဆင့် ထုတ်လွှင့်သည်။
- မည်သည့်နမူနာကိုမဆို run ဖို့ လိုအပ်သည့် cloud ဝန်ဆောင်မှု၊ API keys သို့မဟုတ် Azure subscription မလိုအပ်ပါ။
- အပိုင်း 10 (custom models) မှာ `onnxruntime-genai` သွင်းထားရန်နှင့် Hugging Face မှ မော်ဒယ် ဝိတ်ကို download ရမည်။

## Commit မပြုလုပ်သင့်သော ဖိုင်များ

`.gitignore` သည် (အများစုအနေဖြင့် သိမ်းဆည်းပေးထားသည့်) ဖိုင်များကို ထည့်သွင်းသည်-
- `.venv/` — Python virtual environments
- `node_modules/` — npm dependencies
- `models/` — compiled ONNX model output (အကြီးစား binary ဖိုင်များ၊ အပိုင်း 10 မှ generated)
- `cache_dir/` — Hugging Face မော်ဒယ် download cache
- `.olive-cache/` — Microsoft Olive work directory
- `samples/audio/*.wav` — ဖန်တီး-generated အသံနမူနာများ (`python samples/audio/generate_samples.py` ဖြင့် regen)
- သာမန် Python build artifacts (`__pycache__/`, `*.egg-info/`, `dist/`, စသည်)

## license

MIT — `LICENSE` ကို ကြည့်ပါ။

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**အသိပေးချက်**  
ဤစာတမ်းကို AI ဘာသာပြန်မှု ဝန်ဆောင်မှု [Co-op Translator](https://github.com/Azure/co-op-translator) ဖြင့် ဘာသာပြန်ထားပါသည်။ ကျွန်ုပ်တို့သည် တိကျမှုအတွက် ကြိုးပမ်းနေသော်လည်း အလိုအလျောက် ဘာသာပြန်မှုတွင် အမှားများ သို့မဟုတ် မှားယွင်းချက်များ ပါဝင်နိုင်ကြောင်း ကျေးဇူးပြု၍ သတိပြုပါ။ မူရင်းစာတမ်းကို ၎င်း၏ မူလဘာသာဖြင့်သာ ယုံကြည်စိတ်ချရသော အရင်းခံအချက်အလက်ဟု ထည့်သွင်းစဉ်းစားသင့်ပါသည်။ အရေးကြီးသော အချက်အလက်များအတွက်ပရော်ဖက်ရှင်နယ် လူ့ဘာသာပြန်ခြင်းကို အကြံပြုလိုပါသည်။ ဤဘာသာပြန်မှုကို သုံးစွဲခြင်းကြောင့် ဖြစ်ပေါ်လာနိုင်သည့် နားလည်မှားယွင်းမှုများ သို့မဟုတ် မတိကျမှုများအတွက် ကျွန်ုပ်တို့ တာဝန်မရှိပါ။
<!-- CO-OP TRANSLATOR DISCLAIMER END -->