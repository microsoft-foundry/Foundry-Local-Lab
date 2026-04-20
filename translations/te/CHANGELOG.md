# చేంజ్‌లాగ్ — ఫౌండ్రి లోకల్ వర్క్‌షాప్

ఈ వర్క్‌షాప్‌కు సంబంధించిన అన్ని ప్రాధాన్యమైన మార్పులు క్రింద డాక్యుమెంటెడ్ చేయబడ్డాయి.

---

## 2026-03-11 — భాగం 12 & 13, వెబ్ UI, విస్పర్ రీ రైటింగ్, WinML/QNN ఫిక్స్, మరియు ధృవీకరణ

### జతచేయబడింది
- **భాగం 12: జావా క్రియేటివ్ రైటర్ కోసం వెబ్ UI నిర్మించడం** — స్ట్రీమింగ్ NDJSON, బ్రౌజర్ `ReadableStream`, లైవ్ ఏజెంట్ స్థితి బ్యాడ్జులు, మరియు రియల్‌ టైం ఆర్టికల్ టెక్స్‌ట్ స్ట్రీమింగ్‌ను కవర్ చేసే కొత్త ప్రయోగాల గైడ్ (`labs/part12-zava-ui.md`)
- **భాగం 13: వర్క్‌షాప్ పూర్తి** — అన్ని 12 భాగాల సమీక్ష, మరిన్ని ఆలోచనలు, మరియు వనరు లింక్లతో కొత్త సారాంశ ప్రయోగం (`labs/part13-workshop-complete.md`)
- **జావా UI ఫ్రంట్ ఎండ్:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — మూడు బ్యాకెండ్స్ అందరికీ సాధారణ వెనుకభాగ HTML/CSS/JS బ్రౌజర్ ఇంటర్‌ఫేస్
- **జావాస్క్రిప్ట్ HTTP సర్వర్:** `zava-creative-writer-local/src/javascript/server.mjs` — బ్రౌజర్ ఆధారిత ప్రాప్యత కోసం ఆర్కెస్ట్రేటర్‌ని చుట్టే కొత్త Express-స్టైల్ HTTP సర్వర్
- **C# ASP.NET కోర్ బ్యాకెండ్:** `zava-creative-writer-local/src/csharp-web/Program.cs` మరియు `ZavaCreativeWriterWeb.csproj` — UI మరియు స్ట్రీమింగ్ NDJSON అందించే కొత్త కనిష్ట API ప్రాజెక్ట్
- **ఆడియో సాంపిల్ జనరేటర్:** `samples/audio/generate_samples.py` — భాగం 9 కోసం జావా థీమ్ WAV ఫైళ్లను తయారుచేసేందుకు ఆఫ్‌లైన్ TTS స్క్రిప్ట్ `pyttsx3` ఉపయోగించి
- **ఆడియో సాంపిల్:** `samples/audio/zava-full-project-walkthrough.wav` — ట్రాన్స్క్రిప్షన్ తనిఖీ కోసం కొత్త పొడవైన ఆడియో సాంపిల్
- **ధృవీకరణ స్క్రిప్ట్:** `validate-npu-workaround.ps1` — అన్ని C# సాంపిల్స్ లో NPU/QNN పనితీరు మార్పుల ధృవీకరణకు ఆటోమేటెడ్ పవర్‌షెల్ స్క్రిప్ట్
- **Mermaid డయాగ్రామ్ SVGలు:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML క్రాస్-ప్లాట్‌ఫాం మద్దతు:** మూడు C# `.csproj` ఫైళ్ళ్లో (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) ఇప్పుడు సాపేక్షమైన TFM మరియు పరస్పరం ప్రత్యేకమైన ప్యాకేజ్ సూచనలు ఉంటాయి. విండోస్ పై: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (QNN EP ప్లగిన్‌తో సుప్రసెట్). విండోస్ కాని సిస్టమ్స్ పై: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (బేస్ SDK). Zava ప్రాజెక్ట్స్‌లో హార్డ్కోడ్ చేసిన `win-arm64` RID ఆటో డిటెక్ట్‌తో మార్చబడింది. ట్రాన్సిటివ్ డిపెండెన్సీ వార్క్ అరౌండ్ ద్వారా `Microsoft.ML.OnnxRuntime.Gpu.Linux` నుండి నేటివ్ అసెట్స్ తీసివేయబడ్డాయి, ఇది బ్రోకెన్(win-arm64) రిఫరెన్స్ కలిగి ఉంది. ముందు ఉన్న try/catch NPU వార్క్ అరౌండ్ అన్ని 7 C# ఫైళ్ళ నుండి తీసివేయబడింది.

### మార్చబడింది
- **భాగం 9 (Whisper):** ముఖ్యమైన రీలో రైట్ — జావాస్క్రిప్ట్ ఇక SDK లో నిర్మితమైన `AudioClient` (`model.createAudioClient()`) ఉపయోగిస్తుంది, మాన్యువల్ ONNX Runtime ఇన్ఫరెన్స్ కాకుండా; JS/C# `AudioClient` పద్ధతి vs Python ONNX Runtime పద్ధతిని ప్రతిబింబించే ఆర్కిటెక్చర్ వివరణలు, పోలిక పట్టికలు మరియు పైప్‌లైన్ డయాగ్రామ్‌లు నవీకరించబడ్డాయి
- **భాగం 11:** నావిగేషన్ లింక్లు అప్డేట్ చేయబడ్డాయి (ఇప్పుడు భాగం 12కి సూచిస్తాయి); టూల్-కస్టమర్ ఫ్లో మరియు సీక్వెన్స్ కోసం SVG డయాగ్రామ్‌లు జోడించబడ్డాయి
- **భాగం 10:** వర్క్‌షాప్ ముగింపునకు బదులు భాగం 12 ద్వారా మార్గనిర্দেশించడం కోసం నావిగేషన్ అప్డేట్ చేయబడింది
- **పైథాన్ విస్పర్ (`foundry-local-whisper.py`):** మరిన్ని ఆడియో సాంపిళ్లు మరియు మెరుగైన లోపాల నిర్వహణతో విస్తరింపబడింది
- **జావాస్క్రిప్ట్ విస్పర్ (`foundry-local-whisper.mjs`):** మాన్యువల్ ONNX Runtime సెషన్ల స్థానంలో `model.createAudioClient()`తో `audioClient.transcribe()` ఉపయోగించి రీ రైటింగ్ చేయబడింది
- **పైథాన్ FastAPI (`zava-creative-writer-local/src/api/main.py`):** API తోపాటు స్థిర UI ఫైళ్లను సేవ్ చేయడానికి అప్డేట్ చేయబడింది
- **జావా C# కన్సోల్ (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU వార్క్ అరౌండ్ తీసివేయబడింది (ఇప్పుడు WinML ప్యాకేజ్ ద్వారా నిర్వహణ)
- **README.md:** భాగం 12 సెక్షన్ కోడ్ నమూనా పట్టికలు మరియు బ్యాకెండ్ జోడింపులతో; భాగం 13 సెక్షన్ జోడించబడింది; నేర్చుకునే లక్ష్యాలు మరియు ప్రాజెక్ట్ నిర్మాణం అప్డేట్ చేయబడ్డాయి
- **KNOWN-ISSUES.md:** పరిష్కరించబడిన ఇష్యూ #7 తీసివేయబడింది (C# SDK NPU మోడల్ యొక్క వేరియంట్ — ఇప్పుడు WinML ప్యాకేజ్ వలన నిర్వహించబడుతోంది). మిగిలిన ఇష్యూస్ నంబరింగ్ #1-#6కి మార్చబడింది. వాతావరణ వివరాలు .NET SDK 10.0.104తో నవీకరించబడ్డాయి
- **AGENTS.md:** కొత్త `zava-creative-writer-local` ఎంట్రీస్ (`ui/`, `csharp-web/`, `server.mjs`)తో ప్రాజెక్ట్ నిర్మాణం ట్రీ అప్డేట్ చేయబడింది; C# కీలక ప్యాకేజీలు మరియు సాపేక్ష TFM వివరాలు నవీకరించబడ్డాయి
- **labs/part2-foundry-local-sdk.md:** `.csproj` ఉదాహరణను పూర్తి క్రాస్-ప్లాట్‌ఫాం ప్యాటర్న్ తో సాపేక్ష TFM, పరస్పరం ప్రత్యేకమైన ప్యాకేజ్ సూచనలు, మరియు వివరణాత్మక నోట్ తో అప్డేట్ చేయబడింది

### ధృవీకరించబడింది
- మూడు C# ప్రాజెక్టులు (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) Windows ARM64 పై విజయవంతంగా బిల్ట్ అయ్యాయి
- చాట్ నమూనా (`dotnet run chat`): WinML/QNN ద్వారా `phi-3.5-mini-instruct-qnn-npu:1`గా మోడల్ లోడవుతుంది — NPU వేరియంట్ CPU fallback లేకుండా నేరుగా లోడవుతుంది
- ఏజెంట్ నమూనా (`dotnet run agent`): మల్టీ-టర్న్ సంభాషణతో పూర్తి రకంగా రన్ అవుతుంది, ఎగ్జిట్ కోడ్ 0
- Foundry Local CLI v0.8.117 మరియు SDK v0.9.0 .NET SDK 9.0.312 పై

---

## 2026-03-11 — కోడ్ ఫిక్సులు, మోడల్ క్లీనప్, Mermaid డయాగ్రామ్‌లు, మరియు ధృవీకరణ

### ఫిక్సులు
- **అన్ని 21 కోడ్ సాంపిల్స్ (7 Python, 7 JavaScript, 7 C#):** ఎగ్జిట్ పై `model.unload()` / `unload_model()` / `model.UnloadAsync()` క్లీన్-అప్ జోడించడం ద్వారా OGA మెమొరీ లీక్స్ హెచ్చరికలను పరిష్కరించడం (తెలిసి ఉన్న ఇష్యూ #4)
- **csharp/WhisperTranscription.cs:** దృఢత్వము లేని `AppContext.BaseDirectory` రిలేటివ్ పథ్‌ను `FindSamplesDirectory()`తో మార్చడం, ఇది పెద్దదిగా డైరెక్టరీలను ఎగువ నుంచి నడిచి `samples/audio`ని నమ్మకంగా కనుగొంటుంది (తెలిసి ఉన్న ఇష్యూ #7)
- **csharp/csharp.csproj:** హార్డ్కోడ్ `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>`ని ఆటో-డిటెక్ట్ fallbackతో `$(NETCoreSdkRuntimeIdentifier)` ఉపయోగించి మార్చడం, అందువల్ల `dotnet run` ఏ ప్లాట్‌ఫాం పై అయినా `-r` ఫ్లాగ్ లేకుండా పనిచేస్తుంది ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### మార్పులు
- **భాగం 8:** ASCII బాక్స్ డయాగ్రాం నుండి ప్రయోగవేత్త అభిప్రాయం ఆధారంగా లూప్‌ను తిరిగి SVG చిత్రంగా మార్చడం
- **భాగం 10:** ASCII బాణాల నుండి కంపైలేషన్ పైప్‌లైన్ డయాగ్రామ్‌ను SVG చిత్రంగా మార్చడం
- **భాగం 11:** టూల్-కాల్ ఫ్లో మరియు సీక్వెన్స్ డయాగ్రామ్‌లను SVG చిత్రాలుగా మార్చడం
- **భాగం 10:** "వర్క్‌షాప్ పూర్తి!" విభాగం భాగం 11కి మార్చబడింది (చివరి ప్రయోగం); "ಮುಂದಿನ దశలు" లింక్‌తో మార్చబడింది
- **KNOWN-ISSUES.md:** CLI v0.8.117 లో అన్ని ఇష్యూలను మరోసారి ధృవీకరణ చేయడం. పరిష్కరించినవి తీసివేయడం: OGA మెమొరీ ల Leak (క్లీన్-అప్ జోడించబడింది), విస్పర్ పాథ్ (FindSamplesDirectory), HTTP 500 సుస్టేన్‌డ్ ఇన్ఫరెన్స్ (పునరుత్పత్తి చెందకుండా, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), టూల్_చాయిస్ పరిమితులు (ఇప్పుడు `"required"` మరియు qwen2.5-0.5b లో స్పష్టమైన ఫంక్షన్ టార్గెటింగ్‌తో పనిచేస్తున్నాయి). JS విస్పర్ ఇష్యూ అనుబంధం — అన్ని ఫైళ్ళు ఖాళీ/బైనరీ అవుట్‌పుట్ ఇస్తున్నాయి (v0.9.x నుండి తగ్గింపుగా, తీవ్రత మెజార్‌గా పెంచబడింది). #4 C# RID ఆటో-డిటెక్ట్ వార్క్ అరౌండ్ మరియు [#497](https://github.com/microsoft/Foundry-Local/issues/497) లింక్ అప్డేట్ చేయబడ్డాయి. 7 తెరిచి ఉన్న ఇష్యూస్ మిగిలి ఉన్నాయి.
- **javascript/foundry-local-whisper.mjs:** క్లీన్-అప్ వేరుబుల్ పేరు సరిచేయబడింది (`whisperModel` → `model`)

### ధృవీకరించబడింది
- పైథాన్: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — క్లీన్-అప్‌తో విజయవంతంగా నడిచాయి
- జావాస్క్రిప్ట్: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — క్లీన్-అప్‌తో విజయవంతంగా నడిచాయి
- C#: `dotnet build` 0 హెచ్చరికలు, 0 లోపాలతో విజయవంతం(net9.0 టార్గెట్)
- అన్ని 7 పైథాన్ ఫైళ్లు `py_compile` సింటాక్స్ చెక్‌ను సక్సెస్‌ఫుల్‌గా పాస్ అయ్యాయి
- అన్ని 7 జావాస్క్రిప్ట్ ఫైళ్లు `node --check` సింటాక్స్ ధృవీకరణ ముగింపు

---

## 2026-03-10 — భాగం 11: టూల్ కాలింగ్, SDK API విస్తరణ, మరియు మోడల్ కవచం

### జతచేయబడింది
- **భాగం 11: లోకల్ మోడల్స్‌తో టూల్ కాలింగ్** — టూల్ స్కీమాలు, మల్టీ-టర్న్ ఫ్లో, బహుళ టూల్ కాల్స్, కస్టమ్ టూల్స్, చాట్‌క్లయింట్ టూల్ కాలింగ్, మరియు `tool_choice` కలిగి 8 ప్రయోగాలతో కొత్త గైడ్ (`labs/part11-tool-calling.md`)
- **పైథాన్ సాంపిల్:** `python/foundry-local-tool-calling.py` — OpenAI SDK ఉపయోగించి `get_weather`/`get_population` టూల్స్‌తో టూల్ కాలింగ్
- **జావాస్క్రిప్ట్ సాంపిల్:** `javascript/foundry-local-tool-calling.mjs` — SDK యొక్క స్వదేశీ `ChatClient` (`model.createChatClient()`) ఉపయోగించి టూల్ కాలింగ్
- **C# సాంపిల్:** `csharp/ToolCalling.cs` — OpenAI C# SDK తో `ChatTool.CreateFunctionTool()` ఉపయోగించి టూల్ కాలింగ్
- **భాగం 2, ప్రాక్టీస్ 7:** స్వదేశీ `ChatClient` — `model.createChatClient()` (JS) మరియు `model.GetChatClientAsync()` (C#) OpenAI SDKకి ప్రత్యామ్నాయాలు
- **భాగం 2, ప్రాక్టీస్ 8:** మోడల్ వేరియంట్లు మరియు హార్డ్వేర్ ఎంపిక — `selectVariant()`, `variants`, NPU వేరియంట్ పట్టిక (7 మోడల్స్)
- **భాగం 2, ప్రాక్టీస్ 9:** మోడల్ అప్‌గ్రేడ్లు మరియు కాటలాగ్ రీఫ్రెష్ — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **భాగం 2, ప్రాక్టీస్ 10:** రీజనింగ్ మోడల్స్ — `<think>` ట్యాగ్ పార్సింగ్ ఉదాహరణలతో `phi-4-mini-reasoning`
- **భాగం 3, ప్రాక్టీస్ 4:** OpenAI SDK ప్రత్యామ్నాయంగా `createChatClient` కోసం స్ట్రీమింగ్ కాల్‌బ్యాక్ ప్యాటర్న్ డాక్యుమెంటేషన్‌తో
- **AGENTS.md:** టూల్ కాలింగ్, ChatClient, మరియు రీజనింగ్ మోడల్స్ కోడింగ్ కన్వెన్షన్లను జోడించడం

### మార్చబడింది
- **భాగం 1:** మోడల్ కాటలాగ్ విస్తరణ — `phi-4-mini-reasoning`, `gpt-oss-20b`, `phi-4`, `qwen2.5-7b`, `qwen2.5-coder-7b`, `whisper-large-v3-turbo` జోడించబడ్డాయి
- **భాగం 2:** API సూచిక పట్టికలను విస్తరించి — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync` జోడించబడ్డాయి
- **భాగం 2:** ఎక్స్‌సర్‌సైజ్ 7-9 ని 10-13కి నంబరింగ్ మార్చి కొత్త ప్రయోగాలకు స్థలం కల్పించబడింది
- **భాగం 3:** కీలక గ్రహింపు పట్టికలో స్వదేశీ ChatClient ని చేర్చడం
- **README.md:** భాగం 11 సెక్షన్ కోడ్ నమూనా పట్టికతో; నేర్చుకునే లక్ష్యం #11 తో; ప్రాజెక్ట్ నిర్మాణం ట్రీ నవీకరణ
- **csharp/Program.cs:** CLI రౌటర్ కి `toolcall` కేస్ జతచేయబడింది మరియు సహాయ పాఠ్యాలు నవీకరించబడ్డాయి

---

## 2026-03-09 — SDK v0.9.0 అప్డేట్, బ్రిటిష్ ఇంగ్లీష్, మరియు ధృవీకరణ పాస్

### మార్పులు
- **అన్ని కోడ్ సాంపిల్స్ (Python, JavaScript, C#):** Foundry Local SDK v0.9.0 API కి అప్డేట్ — `await catalog.getModel()` (ముందు `await` ఉండకుండా ఉండేది) సరిచేయబడింది, `FoundryLocalManager` ప్రారంభ నమూనాలు నవీకరించబడ్డాయి, ఎండ్పాయింట్ డిస్కవరీ సరిచేయబడింది
- **అన్ని ల్యాబ్ గైడ్స్ (భాగాలు 1-10):** బ్రిటిష్ ఇంగ్లీష్ (colour, catalogue, optimised, మొదలైనవి)కి మార్చబడింది
- **అన్ని ల్యాబ్ గైడ్స్:** SDK కోడ్ ఉదాహరణలు v0.9.0 API వరకూ అప్డేట్ చేయబడ్డాయి
- **అన్ని ల్యాబ్ గైడ్స్:** API సూచిక పట్టికలు మరియు ఎక్స్‌సర్‌సైజ్ కోడ్ బ్లాక్స్ అప్డేట్ అయినవి
- **జావాస్క్రిప్ట్ క్రిటికల్ ఫిక్స్:** `catalog.getModel()`పై కోల్పోయిన `await` జోడించడం — ఇది మోడల్ ఆబ్జెక్ట్‌కి కాకుండా `Promise` ఇచ్చి సైలెంట్ ఫెయిల్యూర్‌లకు దారి తీస్తుంది

### ధృవీకరించడం
- అన్ని పైథాన్ నమూనాలు Foundry Local సర్వీస్ పై విజయవంతంగా నడిచాయి
- అన్ని జావాస్క్రిప్ట్ నమూనాలు విజయవంతంగా నడిచాయి (Node.js 18+)
- C# ప్రాజెక్టు .NET 9.0లో బిల్డ్ అవుతుంది మరియు నడుస్తుంది (net8.0 SDK అసెంబ్లీ నుండి ముందుకి కంపాటిబిలిటీ)
- వర్క్‌షాప్ మొత్తం 29 ఫైళ్లు మార్చబడినవి మరియు ధృవీకరించబడినవి

---

## ఫైల్ సూచిక

| ఫైల్ | చివరి అప్‌డేట్ | వివరణ |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | విస్తరించిన మోడల్ కాటలాగ్ |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | కొత్త ప్రయోగాలు 7-10, విస్తరించిన API పట్టికలు |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | కొత్త ప్రయోగం 4 (ChatClient), అప్డేట్ చేయబడిన కీలక పాఠాలు |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + బ్రిటిష్ ఇంగ్లీష్ |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + బ్రిటిష్ ఇంగ్లీష్ |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | మెర్మైడ్ డయాగ్రామ్ |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + British English |
| `labs/part10-custom-models.md` | 2026-03-11 | మెర్మైడ్ డయాగ్రామ్స్, వర్క్‌షాప్ కంప్లీట్‌ను భాగం 11కి మార్చారు |
| `labs/part11-tool-calling.md` | 2026-03-11 | కొత్త ప్రయోగశాల, మెర్మైడ్ డయాగ్రామ్స్, వర్క్‌షాప్ కంప్లీట్ విభాగం |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | కొత్త: టూల్ కాలింగ్ సాంపిల్ |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | కొత్త: టూల్ కాలింగ్ సాంపిల్ |
| `csharp/ToolCalling.cs` | 2026-03-10 | కొత్త: టూల్ కాలింగ్ సాంపిల్ |
| `csharp/Program.cs` | 2026-03-10 | `toolcall` CLI కమాండ్ జోడించబడింది |
| `README.md` | 2026-03-10 | భాగం 11, ప్రాజెక్టు నిర్మాణం |
| `AGENTS.md` | 2026-03-10 | టూల్ కాలింగ్ + చాట్ క్లయింట్ సాంప్రదాయాలు |
| `KNOWN-ISSUES.md` | 2026-03-11 | పరిష్కరించిన సమస్య #7 తొలగించబడింది, 6 తుమ్మర్శి సమస్యలు మిగిలి ఉన్నాయి |
| `csharp/csharp.csproj` | 2026-03-11 | క్రాస్-ప్లాట్‌ఫారమ్ TFM, WinML/బేస్ SDK షరతుపడిన ఆధారాలు |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | క్రాస్-ప్లాట్‌ఫారమ్ TFM, ఆటో-డిటెక్ట్ RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | క్రాస్-ప్లాట్‌ఫారమ్ TFM, ఆటో-డిటెక్ట్ RID |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU ట్రై/క్యాచ్ వర్క్ అరౌండ్ తొలగించబడింది |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU ట్రై/క్యాచ్ వర్క్ అరౌండ్ తొలగించబడింది |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU ట్రై/క్యాచ్ వర్క్ అరౌండ్ తొలగించబడింది |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU ట్రై/క్యాచ్ వర్క్ అరౌండ్ తొలగించబడింది |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU ట్రై/క్యాచ్ వర్క్ అరౌండ్ తొలగించబడింది |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | క్రాస్-ప్లాట్‌ఫారమ్ .csproj ఉదాహరణ |
| `AGENTS.md` | 2026-03-11 | C# ప్యాకేజీలు మరియు TFM వివరాలు నవీకరించబడ్డాయి |
| `CHANGELOG.md` | 2026-03-11 | ఈ ఫైలు |