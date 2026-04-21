# கோடிங் முகவர் வழிமுறைகள்

இந்த கோப்பு இந்த களஞ்சியத்தில் வேலை செய்கின்ற AI கோடிங் முகவர்களுக்கு (GitHub Copilot, Copilot Workspace, Codex, மற்றவை) சூழ்நிலையை வழங்குகிறது.

## திட்டம் கண்ணோட்டம்

இது [Foundry Local](https://foundrylocal.ai) உடன் AI பயன்பாடுகளை உருவாக்குவதற்கான **கைமுறை பணிமனை** — இது மென்மையான ஓட்டுநர் ஆகும், இது மொழி மாதிரிகளை முற்றிலும் சாதனத்தில் OpenAI இணக்கமான API மூலம் பதிவிறக்கம் செய்து, நிர்வகித்து மற்றும் வழங்குகிறது. பணிமனை படி படியாக ஆய்வுக் கோவை வழிகாட்டல்கள் மற்றும் Python, JavaScript மற்றும் C# இல் இயங்கக்கூடிய கோட் எடுத்துக்காட்டுக்களை கொண்டுள்ளது.

## களஞ்சிய கட்டமைப்பு

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

## மொழி & கட்டமைப்பு விவரங்கள்

### Python
- **இடம்:** `python/`, `zava-creative-writer-local/src/api/`
- **சார்புள்ளவை:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **முக்கிய தொகுதிகள்:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **குறைந்த பதிப்பு:** Python 3.9+
- **இயக்கு:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **இடம்:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **சார்புள்ளவை:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **முக்கிய தொகுதிகள்:** `foundry-local-sdk`, `openai`
- **மொடியூல் அமைப்பு:** ES மொடியூல்கள் (`.mjs` கோப்புகள், `"type": "module"`)
- **குறைந்த பதிப்பு:** Node.js 18+
- **இயக்கு:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **இடம்:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **திட்டக் கோப்புகள்:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **முக்கிய தொகுதிகள்:** `Microsoft.AI.Foundry.Local` (நான்-விண்டோஸ்), `Microsoft.AI.Foundry.Local.WinML` (விண்டோஸ் — QNN EP உடன் மேலமைப்பு), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **இலக்கு:** .NET 9.0 (நிபந்தனை TFM: `net9.0-windows10.0.26100` விண்டோஸில், `net9.0` வேறு இடங்களில்)
- **இயக்கு:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## கோடிங் பழக்கவழக்கம்

### பொதுவாக
- அனைத்து கோடு எடுத்துக்காட்டுகளும் **தனித்தனி ஒரே கோப்பு எடுத்துக்காட்டுகள்** — பகிரப்பட்ட உதவி நூலகங்கள் அல்லது செருகல்கள் இல்லை.
- ஒவ்வொரு எடுத்துக்காட்டும் அதன் சார்புள்ளவைகளை நிறுவிய பின்னர் தனித்தனியாக இயங்கும்.
- API முக்கியங்கள் எப்போதும் `"foundry-local"` என்பதுதான் — Foundry Local இதைப்.placeholder ஆக பயன்படுத்துகிறது.
- அடிப்படை URL கள் `http://localhost:<port>/v1` என்பவையாக இருக்கும் — போர்ட் இயக்கநிலையில் SDK மூலம் கண்டுபிடிக்கப்படும் (`manager.urls[0]` ஜெஎஸில், `manager.endpoint` பைதானில்).
- Foundry Local SDK சேவை துவக்க மற்றும் முடிவுப் புள்ளி கண்டுபிடிப்பை கையாள்கிறது; கடின-கோடிடப்பட்ட போர்ட்களை விட SDK முறைகளை முன்னுரிமை தாருங்கள்.

### Python
- `openai` SDK ஐ `OpenAI(base_url=..., api_key="not-required")` உடன் பயன்படுத்தவும்.
- SDK நிர்வகிக்கப்படும் சேவை ஆயுளுக்கு `foundry_local` இல் இருந்து `FoundryLocalManager()` ஐ பயன்படுத்தவும்.
- ஸ்ட்ரீமிங்: `stream` பொருளை `for chunk in stream:` என வைத்து இடைவெளிக் கோடுகள் இடைவழித்தல்.
- எடுத்துக்காட்டு கோப்புகளில் எந்தவொரு வகை குறியீடுகளும் இல்லை (பணிமனை கற்றுக் கொள்வோருக்கு எடுத்துக்காட்டுகளை குறிப்பாகச் சுருக்கமாக வைத்தல்).

### JavaScript
- ES மொடியூல் சொற்களுடன்: `import ... from "..."`.
- `"openai"` இலிருந்து `OpenAI` மற்றும் `"foundry-local-sdk"` இலிருந்து `FoundryLocalManager` ஐ பயன்படுத்தவும்.
- SDK துவக்க முறை: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- ஸ்ட்ரீமிங்: `for await (const chunk of stream)`.
- மேல்நிலை `await` முழுவதும் பயன்படுத்தப்படுகிறது.

### C#
- Nullable செயல்படுத்தப்பட்டது, தவிர் உட்படுத்தல்கள், .NET 9.
- SDK நிர்வகிக்கப்படும் ஆயுளுக்கு `FoundryLocalManager.StartServiceAsync()` பயன்படுத்தவும்.
- ஸ்ட்ரீமிங்: `CompleteChatStreaming()` உடன் `foreach (var update in completionUpdates)`.
- பிரதான `csharp/Program.cs` என்பது CLI வழிமாற்றி ஆகும், நிலையான `RunAsync()` முறைகளுக்கு அனுப்புகிறது.

### கருவி அழைப்புகள்
- சில மாதிரிகள் மட்டும் கருவி அழைப்புகளை ஆதரிக்கின்றன: **Qwen 2.5** குடும்பம் (`qwen2.5-*`) மற்றும் **Phi-4-mini** (`phi-4-mini`).
- கருவி வடிவங்கள் OpenAI செயல்பாடு அழைப்புக்கான JSON வடிவமைப்பை பின்பற்றும் (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- உரையாடல் பல-முறை முறைபாட்டை பயன்படுத்துகிறது: பயனர் → உதவியாளர் (tool_calls) → கருவி (முடிவுகள்) → உதவியாளர் (இறுதியான பதில்).
- கருவி முடிவு செய்திகளில் உள்ள `tool_call_id` மாதிரியின் கருவி அழைப்பில் இருந்த `id` உடன் பொருந்த வேண்டும்.
- Python நேரடியாக OpenAI SDK ஐ பயன்படுத்துகிறது; JavaScript SDK இயல்பான `ChatClient` (`model.createChatClient()`) ஐ பயன்படுத்துகிறது; C# OpenAI SDK உடன் `ChatTool.CreateFunctionTool()` ஐ பயன்படுத்துகிறது.

### ChatClient (இயல்புநிலைய SDK கிளைண்ட்)
- JavaScript: `model.createChatClient()` என்பது `completeChat(messages, tools?)` மற்றும் `completeStreamingChat(messages, callback)` உடைய `ChatClient` ஐ மீட்டளிக்கிறது.
- C#: `model.GetChatClientAsync()` ஒரு சாதாரண `ChatClient` ஐ மீட்டளிக்கிறது, இது OpenAI NuGet தொகுப்பை இறக்குமதி செய்யாமல் பயன்படுத்தலாம்.
- Python இல் நேரடி ChatClient இல்லை — OpenAI SDK ஐ `manager.endpoint` மற்றும் `manager.api_key` உடன் பயன்படுத்தவும்.
- **முக்கியம்:** JavaScript `completeStreamingChat` ஒரு **கால் பேக் முறையை**ப் பயன்படுத்துகிறது, asynchronous இடைமாற்றம் அல்ல.

### காரணமிடும் மாதிரிகள்
- `phi-4-mini-reasoning` தனது சிந்தனையை இறுதி விடை வழங்குவதற்கு முன்பாக `<think>...</think>` குறிச்சொற்களுக்குள் கட்டுகிறது.
- தேவைப்பட்டால், பதிலிலிருந்து காரணமிடுதலை பிரிப்பதற்காக குறிச்சொற்களை பகுப்பாய்வு செய்யவும்.

## ஆய்வு வழிகாட்டல்கள்

ஆய்வு கோப்புகள் `labs/` இல் Markdown ஆக உள்ளன. அவை ஒரே மாதிரியில் அமைந்துள்ளன:
- லோகோ தலைப்பு படம்
- தலைப்பு மற்றும் குறிக்கோள் அழைப்பு
- கண்ணோட்டம், கற்றல் நோக்குகள், முன்னதிகாரங்கள்
- கண்டுபிடிப்பு விளக்க பகுதிகள் மற்றும் விளக்கப்படங்கள்
- எண்ணிக்கை செய்யப்பட்ட பயிற்சிகள், கோட் தொகுதிகள் மற்றும் எதிர்பார்க்கும் வெளியீடு
- சுருக்க அட்டவணை, முக்கிய எடுத்துக்காட்டுகள், மேலதிக வாசிப்பு
- அடுத்த பகுதியுக்கான வழிசெலுத்தல் இணைப்பு

ஆய்வு உள்ளடக்கத்தை தொகுத்தல் செய்யும்போது:
- உள்ளமைந்த Markdown வடிவமைப்பு மற்றும் பிரிவு தொடர் தன்மையை பராமரி.
- கோட் தொகுதிகள் மொழியை குறிப்பிட வேண்டும் (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- OS தொடர்புடைய ஷெல் கட்டளைகளுக்கு bash மற்றும் PowerShell இரு வகைகளையும் வழங்கவும்.
- `> **Note:**`, `> **Tip:**`, மற்றும் `> **Troubleshooting:**` அழைப்பு வகைகள் பயன்படுத்தவும்.
- அட்டவணைகள் `| தலைப்பு | தலைப்பு |` பைப் வடிவில் இருக்க வேண்டும்.

## கட்டமைப்பு & சோதனை கட்டளைகள்

| செயல் | கட்டளை |
|--------|---------|
| **Python எடுத்துக்காட்டுகள்** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS எடுத்துக்காட்டுகள்** | `cd javascript && npm install && node <script>.mjs` |
| **C# எடுத்துக்காட்டுகள்** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (வலை)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (வலை)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **விளக்கப்படங்கள் உருவாக்குவது** | `npx mmdc -i <input>.mmd -o <output>.svg` (root `npm install` தேவை) |

## வெளிப்புற சார்புகள்

- **Foundry Local CLI** டெவலப்பர் இயந்திரத்தில் நிறுவப்பட்டிருக்க வேண்டும் (`winget install Microsoft.FoundryLocal` அல்லது `brew install foundrylocal`).
- **Foundry Local சேவை** உள்ளூரே இயங்கி, இயக்கநிலை போர்டில் OpenAI-இன் REST API ஐ வெளிக்காட்டுகிறது.
- எந்த உதாரணத்திலும் கிள라우ட் சேவைகள், API முக்கியங்கள் அல்லது Azure சந்தாதாரப்பதிவுகள் தேவையில்லை.
- பாகம் 10 (செயல்முறை மாதிரிகள்) கூடுதலாக `onnxruntime-genai` மற்றும் Hugging Face இல் இருந்து மாதிரி எடை பதிவிறக்கத்தை தேவைப்படுத்துகிறது.

## ஒப்படைக்கக்கூடாத கோப்புகள்

`.gitignore` பெரும்பாலும் தவிர்க்கிறது (அதற்காக):
- `.venv/` — Python மெய்நிகர் சூழல்கள்
- `node_modules/` — npm சார்புகள்
- `models/` — தொகுக்கப்பட்ட ONNX மாதிரி வெளியீடு (பெரிய இருமுக கோப்புகள், பாகம் 10 உருவாக்கியது)
- `cache_dir/` — Hugging Face மாதிரி பதிவிறக்கம் கேஷ்
- `.olive-cache/` — Microsoft Olive பணிமனை அடைவை
- `samples/audio/*.wav` — உருவாக்கப்பட்ட ஆடியோ எடுத்துக்காட்டுகள் (`python samples/audio/generate_samples.py` மூலம் மீண்டும் உருவாக்கப்படும்)
- நிலையான Python கட்டுமான கலைப்பொருட்கள் (`__pycache__/`, `*.egg-info/`, `dist/` மற்றும் பிற)

## உரிமம்

MIT — `LICENSE` ஐ பார்க்கவும்.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**விதிமுறை அறிவிப்பு**:  
இந்த ஆவணம் [Co-op Translator](https://github.com/Azure/co-op-translator) என்ற செயற்கை நுண்ணறிவு மொழிபெயர்ப்பு சேவையை பயன்படுத்தி மொழிபெயர்க்கப்பட்டுள்ளது. நாங்கள் துல்லியத்திற்குக் கடமைபடுத்தினாலும், தானாக செய்யப்படும் மொழிபெயர்ப்புகளில் பிழைகள் அல்லது தவறுகள் இருக்கக்கூடும் என்பதை தயவுசெய்து கருத்தில் கொள்ளவும். அசல் ஆவணம் அதன் மொழியில் அதிகாரப்பூர்வமான ஆதாரமாக கருதப்பட வேண்டும். முக்கியமான தகவல்களுக்கு தொழில்முறை மனித மொழிபெயர்ப்பு பரிந்துரைக்கப்படுகிறது. இந்த மொழிபெயர்ப்பின் பயன்பாட்டால் ஏற்பட்ட எந்த தவறான புரிதல்கள் அல்லது தவறான விரிவாக்கங்களுக்காக நாங்கள் பொறுப்பு ஏற்கமாட்டோம்.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->