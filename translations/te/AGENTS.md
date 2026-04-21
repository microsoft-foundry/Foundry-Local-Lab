# కోడింగ్ ఏజెంట్ సూచనలు

ఈ ఫైలు ఈ రిపోజిటరీలో పనిచేసే AI కోడింగ్ ఏజెంట్స్ (GitHub Copilot, Copilot Workspace, Codex, మరియు ఇతరులు) కోసం సందర్భాన్ని అందిస్తుంది.

## ప్రాజెక్ట్ అవలోకనం

ఈది [Foundry Local](https://foundrylocal.ai) తో AI అప్లికేషన్లు తయారు చేయడానికి **ప్రయోగాత్మక వర్క్‌షాప్** — ఇది ఒక సులభమైన రన్‌టైమ్, ఇది అంతా డివైస్‌లోనే డౌన్‌లోడ్ చేయబడిన, నిర్వహించబడిన, మరియు OpenAI-అనుగుణ API ద్వారా భాషా మోడల్స్‌ను సర్వ్ చేస్తుంది. వర్క్‌షాప్‌లో స్టెప్-బై-స్టెప్ ల్యాబ్ గైడ్లు మరియు Python, JavaScript, C# లో నడిపించగల కోడ్ నమూనాలు ఉంటే.

## రిపోజిటరీ నిర్మాణం

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

## భాష & ఫ్రేమ్‌వర్క్ వివరాలు

### Python
- **స్థానం:** `python/`, `zava-creative-writer-local/src/api/`
- **ఆధారాలు:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **కీ ప్యాకేజీలు:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **కనిష్ట వెర్షన్:** Python 3.9+
- **నడిపించు:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **స్థానం:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **ఆధారాలు:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **కీ ప్యాకేజీలు:** `foundry-local-sdk`, `openai`
- **మాడ్యూల్ సిస్టమ్:** ES మాడ్యూల్స్ (`.mjs` ఫైళ్లు, `"type": "module"`)
- **కనిష్ట వెర్షన్:** Node.js 18+
- **నడిపించు:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **స్థానం:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **ప్రాజెక్ట్ ఫైల్స్:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **కీ ప్యాకేజీలు:** `Microsoft.AI.Foundry.Local` (నాన్-విండోస్), `Microsoft.AI.Foundry.Local.WinML` (విండోస్ — QNN EPతో సుపర్‌సెట్), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **లక్ష్యం:** .NET 9.0 (షరతు TFM: విండోస్‌లో `net9.0-windows10.0.26100`, మిగిలిన చోట్ల `net9.0`)
- **నడిపించు:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## కోడింగ్ నిబంధనలు

### సాధారణ
- అన్ని కోడ్ నమూనాలు **స్వయంపూరిత ఒకే ఫైల్ ఉదాహరణలు** — ఏవైనా భాగస్వామ్య యుటిలిటీ లైబ్రరీలు లేదా అభివృద్ధులు లేవు.
- ప్రతి నమూనా దాని స్వంత ఆధారాలు ఇన్స్టాల్ చేసుకుని స్వతంత్రంగా నడుస్తుంది.
- API కీలు ఎప్పుడూ `"foundry-local"` గా సెట్ చేయబడ్డాయి — Foundry Local ఇది ప్లేస్‌హోల్డర్‌గా ఉపయోగిస్తుంది.
- బేస్ URLలు `http://localhost:<port>/v1` ఉపయోగిస్తాయి — పోర్ట్ డైనమిక్‌గా ఉంటుంది మరియు SDK ద్వారా రన్‌టైమ్‌లో కనుగొనబడుతుంది (`manager.urls[0]` జావాస్క్రిప్ట్‌లో, `manager.endpoint` పythాన్‌లో).
- Foundry Local SDK సర్వీస్ ప్రారంభాన్ని మరియు ఎండ్పాయింట్ కనుగొనలను నిర్వహిస్తుంది; SDK నమూనాలను హార్డ్-కోడెడ్ పోర్ట్స్ కంటే ప్రాధాన్యం ఇవ్వండి.

### Python
- `openai` SDK తో `OpenAI(base_url=..., api_key="not-required")` ఉపయోగించండి.
- SDK-మ్యానేజ్ చేసిన సర్వీస్ లైఫ్సైకిల్ కోసం `foundry_local` నుండి `FoundryLocalManager()` ఉపయోగించండి.
- స్ట్రీమింగ్: `for chunk in stream:` తరహాలో `stream` ఆబ్జెక్ట్ పై లూప్ చేయండి.
- నమూనా ఫైల్స్‌లో టైప్ అనోటేషన్లు ఉండవు (వర్క్‌షాప్ నేర్చుకునేవారికి సరళంగా ఉంచండి).

### JavaScript
- ES మాడ్యూల్ సింటెక్స్: `import ... from "..."`.
- `"openai"` నుండి `OpenAI` మరియు `"foundry-local-sdk"` నుండి `FoundryLocalManager` ఉపయోగించండి.
- SDK ప్రారంభ నమూనా: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- స్ట్రీమింగ్: `for await (const chunk of stream)`.
- టాప్-లెవెల్ `await` అన్ని చోట్ల ఉపయోగించబడుతుంది.

### C#
- Nullable ఎనేబుల్, ఇంప్లిసిట్ usings, .NET 9.
- SDK-మేనేజ్ చేసిన లైఫ్సైకిల్ కోసం `FoundryLocalManager.StartServiceAsync()` ఉపయోగించండి.
- స్ట్రీమింగ్: `CompleteChatStreaming()` లో `foreach (var update in completionUpdates)`.
- ప్రధాన `csharp/Program.cs` CLI రౌటర్ — స్టాటిక్ `RunAsync()` మేథడ్స్‌కు డిస్పాచ్ చేస్తుంది.

### టూల్ కాలింగ్
- కొన్ని మోడల్స్ మాత్రమే టూల్ కాలింగ్‌కు మద్దతు ఇస్తాయి: **Qwen 2.5** ఫ్యామిలీ (`qwen2.5-*`) మరియు **Phi-4-mini** (`phi-4-mini`).
- టూల్ స్కీమాలు OpenAI ఫంక్షన్-కాలింగ్ JSON ఫార్మాట్ అనుసరిస్తాయి (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- సంభాషణ బహు-తదుపరి నమూనా: యూజర్ → అసిస్టెంట్ (tool_calls) → టూల్ (ఫలితాలు) → అసిస్టెంట్ (చివరి సమాధానం).
- టూల్ ఫలిత సందేశాలలో `tool_call_id` మోడల్ టూల్ కాల్ లోని `id`తో సరిపోలాలి.
- Python నేరుగా OpenAI SDK ఉపయోగిస్తుంది; JavaScript SDK యొక్క స్వదేశీ `ChatClient` (`model.createChatClient()`); C# OpenAI SDKతో `ChatTool.CreateFunctionTool()` ఉపయోగిస్తుంది.

### ChatClient (స్వదేశీ SDK క్లయింట్)
- JavaScript: `model.createChatClient()` ఒక `ChatClient` ఇస్తుంది, ఇందులో `completeChat(messages, tools?)` మరియు `completeStreamingChat(messages, callback)` ఉన్నాయి.
- C#: `model.GetChatClientAsync()` ఒక సాధారణ `ChatClient` ఇస్తుంది, ఇది OpenAI NuGet ప్యాకేజ్ దిగుమతి చేయకుండా ఉపయోగించవచ్చు.
- Python native ChatClient లేదు — OpenAI SDK తో `manager.endpoint` మరియు `manager.api_key` ఉపయోగించండి.
- **ముఖ్యమైనది:** JavaScript `completeStreamingChat` **callback నమూనాను** ఉపయోగిస్తుంది, async iteration కాదు.

### Reasoning మోడల్స్
- `phi-4-mini-reasoning` తేలికగా `<think>...</think>` ట్యాగ్స్‌లో ఆలోచనను కవర్లుగా ప్యాక్ చేస్తుంది, తర్వాత చివరి సమాధానం.
- అవసరమైతే ఆ ట్యాగ్స్‌ని వేరు చేసి తర్కం మరియు సమాధానం విభజించండి.

## ల్యాబ్ గైడ్స్

ల్యాబ్ ఫైల్స్ `labs/` లో Markdown గా ఉంటాయి. అవి క్రమంగా ఉంటాయి:
- లోగో హెడ్డర్ చిత్రం
- శీర్షిక మరియు లక్ష్య కానుకలు
- అవలోకనం, నేర్చుకునే లక్ష్యాలు, ముందు అవసరాలు
- భావన వివరణ శీర్షికలతో మరియు డయాగ్రామ్స్‌తో
- సంఖ్యలతో ఎక్సర్‌సైజులు, కోడ్ బ్లాక్స్ మరియు అంచనా ఫలితాలు
- సారాంశ పట్టిక, ప్రధాన కొనుగోలు, మరింత చదవడం
- తదుపరి భాగానికి నావిగేషన్ లింక్

ల్యాబ్ కంటెంట్ సవరించినప్పుడు:
- ఇప్పటికే ఉన్న Markdown ఫార్మాటింగ్ శైలి మరియు విభాగాల క్రమాన్ని దృఢంగా ఉంచండి.
- కోడ్ బ్లాక్స్ భాషను సూచించాలి (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- షెల్ కమాండ్లకు OS ముఖ్యం అయితే రెండు `bash` మరియు PowerShell వేరియంట్లు ఇవ్వండి.
- `> **Note:**`, `> **Tip:**`, మరియు `> **Troubleshooting:**` కాలౌట్ శైలులు ఉపయోగించండి.
- పట్టికలు `| Header | Header |` పైపు ఫార్మాట్ లో ఉంటాయి.

## నిర్మాణ & పరీక్ష కమాండ్లు

| చర్య | కమాండ్ |
|--------|---------|
| **Python నమూనాలు** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS నమూనాలు** | `cd javascript && npm install && node <script>.mjs` |
| **C# నమూనాలు** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (వెబ్)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (వెబ్)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **డయాగ్రామ్‌లు ఉత్పత్తి చేయండి** | `npx mmdc -i <input>.mmd -o <output>.svg` (root `npm install` అవసరం) |

## బయటి ఆధారాలు

- **Foundry Local CLI** డెవలపర్ కంప్యూటర్‌లో ఇన్‌స్టాల్ చేయబడాలి (`winget install Microsoft.FoundryLocal` లేదా `brew install foundrylocal`).
- **Foundry Local సర్వీస్** స్థానికంగా నడుస్తుంది మరియు డైనమిక్ పోర్ట్‌పై OpenAI అనుగుణమైన REST API కోసం అందిస్తుంది.
- ఏ నమూనా నడపడానికి క్లౌడ్ సేవలు, API కీలు, లేదా Azure సబ్‌స్క్రిప్షన్లు అవసరం లేవు.
- భాగం 10 (కస్టమ్ మోడల్స్) కోసం అదనంగా `onnxruntime-genai` అవసరం మరియు Hugging Face నుండి మోడల్ వెయిట్‌లను డౌన్‌లోడ్ చేస్తుంది.

## కమిట్ చేయాల్సిన ఫైల్స్ కావు

`.gitignore` క్రింది ఆభాసాలను తప్పిస్తుంది (ముఖ్యంగా):
- `.venv/` — Python వర్చువల్ ఎన్విరాన్మెంట్లు
- `node_modules/` — npm ఆధారాలు
- `models/` — కంపైల్ చేసిన ONNX మోడల్ అవుట్పుట్ (పెద్ద బైనరీ ఫైల్స్, భాగం 10 ద్వారా ఉత్పన్నం కావలెను)
- `cache_dir/` — Hugging Face మోడల్ డౌన్‌లోడ్ క్యాష్
- `.olive-cache/` — Microsoft Olive వర్కింగ్ డైరెక్టరీ
- `samples/audio/*.wav` — ఉత్పత్తి చేసిన ఆడియో నమూనాలు (`python samples/audio/generate_samples.py` వలన మళ్ళీ ఉత్పత్తి)
- స్టాండర్డ్ Python బిల్డ్ ఆర్టిఫాక్ట్స్ (`__pycache__/`, `*.egg-info/`, `dist/`, మొదలైనవి)

## లైసెన్స్

MIT — `LICENSE` చూడండి.