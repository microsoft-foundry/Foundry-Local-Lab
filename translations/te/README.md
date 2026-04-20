<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local కార్యశాల - ఆన్-డివైస్ AI యాప్స్ నిర్మించండి

మీ స్వంత సిస్టంను ఉపయోగించి భాషా మోడళ్ళను నడిపించడానికి మరియు [Foundry Local](https://foundrylocal.ai) మరియు [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/)తో తెలివైన అప్లికేషన్లు నిర్మించడానికి హ్యాండ్స్-ఆన్ కార్యశాల.

> **Foundry Local అంటే ఏమిటి?** Foundry Local అనేది ఒక లైట్‌వెయిట్ రంటైమ్, ఇది మీ సామర్థ్యంతో భాషా మోడల్స్‌ను పూర్తిగా డౌన్లోడ్, నిర్వహించడానికి మరియు సర్వ్ చేయడానికి అనుమతిస్తుంది. ఇది **OpenAI-అనుకూల API**ని విడుదల చేస్తుంది కాబట్టి ఏదైనా టూల్ లేదా SDK OpenAIతో మాట్లాడగలదు - క్లౌడ్ ఖాతా అవసరం లేదు.

---

## నేర్చుకోవలసిన లక్ష్యాలు

ఈ కార్యశాల చివరిలో మీరు చేయగలరు:

| # | లక్ష్యం |
|---|-----------|
| 1 | Foundry Localని ఇన్‌స్టాల్ చేసి CLIతో మోడల్స్‌ను నిర్వహించడం |
| 2 | Foundry Local SDK APIలో ప్రోగ్రామాటిక్ మోడల్ నిర్వహణకు నైపుణ్యం పొందడం |
| 3 | Python, JavaScript, మరియు C# SDK లను ఉపయోగించి లోకల్ ఇన్ఫెరెన్స్ సర్వర్‌కు కనెక్ట్ అవ్వడం |
| 4 | RAG (Retrieval-Augmented Generation) పైప్‌లైన్ నిర్మించడం, మీరు కలిగిన డేటాలో సమాధానాలను దృఢీకరించడం |
| 5 | నిరంతర సూచనలు మరియు వ్యక్తిత్వాలతో AI ఏజెంట్లను సృష్టించడం |
| 6 | ఫీడ్బాక్ లూప్‌లతో బహుఆర్ ఏజెంట్ వర్క్‌ఫ్లోలను నిర్వహించడం |
| 7 | ఉత్పత్తి కాప్‌స్టోన్ యాప్‌ను అన్వేషించడం - Zava Creative Writer |
| 8 | గోల్డెన్ డేటాసెట్‌లు మరియు LLM-జడ్జి స్కోరింగ్‌తో మూల్యాంకన ఫ్రేమ్‌వర్క్‌లను నిర్మించడం |
| 9 | Whisperతో ఆడియో ట్రాన్స్క్రిప్షన్ - Foundry Local SDK ఉపయోగించి ఆన్-డివైస్ స్పీచ్-టు-టెక్స్ట్ |
| 10 | ONNX Runtime GenAI మరియు Foundry Localతో కస్టమ్ లేదా Hugging Face మోడల్స్ కాంపైల్ చేసి నడపడం |
| 11 | లోకల్ మోడల్స్‌కు టూల్-కలింగ్ ప్యాట‌ర్న్‌తో బాహ్య ఫంక్షన్‌లను పిలవడం అవకాశం కల్గించడం |
| 12 | Zava Creative Writer కోసం బ్రౌజర్ ఆధారిత UIని రియల్-టైమ్ స్ట్రీమింగ్‌తో నిర్మించడం |

---

## అవసరమైనవి

| అవసరాలు | వివరాలు |
|-------------|---------|
| **హార్డ్వేర్** | కనీసం 8 GB RAM (16 GB సిఫార్సు); AVX2-సామర్థ్య CPU లేదా మద్దతు ఇచ్చే GPU |
| **ఆపరేటింగ్ సిస్టమ్** | Windows 10/11 (x64/ARM), Windows Server 2025, లేదా macOS 13+ |
| **Foundry Local CLI** | Windows కోసం `winget install Microsoft.FoundryLocal` లేదా macOS కోసం `brew tap microsoft/foundrylocal && brew install foundrylocal` ద్వారా ఇన్‌స్టాల్ చేయండి. వివరాలకు [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) చూడండి. |
| **భాష రన్టైమ్** | **Python 3.9+**, మరియు/లేదా **.NET 9.0+**, మరియు/లేదా **Node.js 18+** |
| **Git** | ఈ రిపాజిటరీని క్లోన్ చేసుకోవడానికి |

---

## ప్రారంభం చేయడం

```bash
# 1. రిపాజిటరీని క్లోన్ చేయండి
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Foundry Local ఇన్‌స్టాల్ అయినదీ లేదో ధృవీకరించండి
foundry model list              # అందుబాటులో ఉన్న మోడల్స్ జాబితా
foundry model run phi-3.5-mini  # ఒక ఇంటరాక్టివ్ చాట్ ప్రారంభించండి

# 3. మీ భాష ట్రాక్‌ను ఎంచుకోండి (పూర్తి సెటప్ కోసం భాగం 2 ల్యాబ్ చూడండి)
```

| భాష | త్వరిత ప్రారంభం |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## కార్యశాల భాగాలు

### భాగం 1: Foundry Localతో ప్రారంభం

**ల్యాబ్ గైడ్:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local అంటే ఏమిటి మరియు ఇది ఎలా పనిచేస్తుంది
- Windows మరియు macOSపై CLI ఇన్‌స్టాల్ చేయడం
- మోడల్స్‌ను అన్వేషించడం - జాబితా, డౌన్లోడ్, నడిపించడం
- మోడల్ అలయాసులు మరియు డైనమిక్ పోర్టుల గురించి అర్థం చేసుకోవడం

---

### భాగం 2: Foundry Local SDK లో లోతైన అవగాహన

**ల్యాబ్ గైడ్:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- అప్లికేషన్ అభివృద్ధికి CLI కన్నా SDK ఎందుకు ఉపయోగించాలి
- Python, JavaScript, మరియు C# కోసం పూర్తి SDK API సూచిక
- సర్వీస్ నిర్వహణ, కేటలాగ్ బ్రౌజింగ్, మోడల్ జీవిత చక్రం (డౌన్లోడ్, లోడ్, అన్‌లోడ్)
- త్వరితారంభ నమూనాలు: Python కన్‌స్ట్రక్టర్ బూట్‌స్ట్రాప్, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` మెటాడేటా, అలయాసులు, మరియు హార్డ్వేర్-ఎంతో తగిన మోడల్ ఎంపిక

---

### భాగం 3: SDKలు మరియు APIలు

**ల్యాబ్ గైడ్:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, మరియు C# నుండి Foundry Localకు కనెక్ట్ అవ్వడం
- Foundry Local SDK ఉపయోగించి సర్వీస్‌ను ప్రోగ్రామాటిక్‌గా నిర్వహించడం
- OpenAI-అనుకూల API ద్వారా స్ట్రీమింగ్ చాట్ కంప్లీషన్స్
- ప్రతి భాష కొరకు SDK పద్ధతుల సూచిక

**కోడ్ నమూనాలు:**

| భాష | ఫైల్ | వివరణ |
|----------|------|-------------|
| Python | `python/foundry-local.py` | ప్రాథమిక స్ట్రీమింగ్ చాట్ |
| C# | `csharp/BasicChat.cs` | .NETతో స్ట్రీమింగ్ చాట్ |
| JavaScript | `javascript/foundry-local.mjs` | Node.jsతో స్ట్రీమింగ్ చాట్ |

---

### భాగం 4: RAG (Retrieval-Augmented Generation)

**ల్యాబ్ గైడ్:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG అంటే ఏమిటి మరియు ఇది ఎందుకు ముఖ్యం
- ఇన్-మె모రీ జ్ఞాన ఆధారం నిర్మించడం
- స్కోరింగ్‌తో కీలకపదం-ఓవర్‌లాప్ రిట్రీవల్
- గ్రౌండెడ్ సిస్టమ్ ప్రమ్ప్ట్‌లను రూపొందించడం
- పూర్తి RAG పైప్‌లైన్‌ను ఆన్-డివైస్ న‌డిపించడం

**కోడ్ నమూనాలు:**

| భాష | ఫైల్ |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### భాగం 5: AI ఏజెంట్లను నిర్మించడం

**ల్యాబ్ గైడ్:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI ఏజెంట్ అంటే ఏమిటి (సాధారణ LLM కాల్‌తో పోల్చితే)
- `ChatAgent` ప్యాటర్న్ మరియు Microsoft Agent Framework
- సిస్టమ్ సూచనలు, వ్యక్తిత్వాలు, మరియు బహుమార్గ సంభాషణలు
- ఏజెంట్ల నుండి నిర్మిత అవుట్‌పుట్ (JSON)

**కోడ్ నమూనాలు:**

| భాష | ఫైల్ | వివరణ |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Frameworkతో ఒక ఏజెంట్ |
| C# | `csharp/SingleAgent.cs` | ఒక ఏజెంట్ (ChatAgent ప్యాటర్న్) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | ఒక ఏజెంట్ (ChatAgent ప్యాటర్న్) |

---

### భాగం 6: బహుఆర్ ఏజెంట్ వర్క్‌ఫ్లోలు

**ల్యాబ్ గైడ్:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- బహుఆర్ ఏజెంట్ పైప్‌లైన్లు: పరిశోధకుడు → రచయిత → ఎడిటర్
- వరుస ఆర్గనైజేషన్ మరియు ఫీడ్బ్యాక్ లూప్‌లు
- పంచుకున్న కాన్ఫిగరేషన్ మరియు నిర్మిత హ్యాండ్-ఆఫ్‌లు
- మీ స్వంత బహుఆర్ ఏజెంట్ వర్క్‌ఫ్లో రూపకల్పన

**కోడ్ నమూనాలు:**

| భాష | ఫైల్ | వివరణ |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | మూడు ఏజెంట్ పైప్‌లైన్ |
| C# | `csharp/MultiAgent.cs` | మూడు ఏజెంట్ పైప్‌లైన్ |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | మూడు ఏజెంట్ పైప్‌లైన్ |

---

### భాగం 7: Zava Creative Writer - కాప్‌స్టోన్ అప్లికేషన్

**ల్యాబ్ గైడ్:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 ప్రత్యేక ఏజెంట్లతో తయారైన ఉత్పత్తి శైలి బహుఆర్ ఏజెంట్ యాప్
- అభిప్రాయం పై నడిచే వరుస పైప్‌లైన్
- స్ట్రీమింగ్ అవుట్పుట్, ఉత్పత్తి కేటలాగ్ శోధన, నిర్మిత JSON హ్యాండ్-ఆఫ్‌లు
- Python (FastAPI), JavaScript (Node.js CLI), మరియు C# (.NET కన్సోల్)లో పూర్తి అమలు

**కోడ్ నమూనాలు:**

| భాష | డైరెక్టరీ | వివరణ |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | ఆర్కెస్ట్రేటర్‌తో FastAPI వెబ్ సర్వీస్ |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI అప్లికేషన్ |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 కన్సోల్ అప్లికేషన్ |

---

### భాగం 8: మూల్యాంకనం-ఆధారిత అభివృద్ధి

**ల్యాబ్ గైడ్:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- గోల్డెన్ డేటాసెట్‌ల ఆధారంగా AI ఏజెంట్ల కోసం పద్ధతిగత మూల్యాంకన ఫ్రేమ్‌వర్క్‌ను నిర్మించడం
- నియమ-ఆధారిత తనిఖీలు (పొడవు, కీలకపద కవర్, నిషిద్ధ పదాలు) + LLM-జడ్జి స్కోరింగ్
- ప్రాంప్ట్ వేరియంట్లతో పక్కపక్కన పోలిక మరియు సమగ్ర స్కోర్‌కార్డులు
- భాగం 7 నుండి Zava ఎడిటర్ ఏజెంట్ ప్యాటర్న్‌ను ఆఫ్‌లైన్ టెస్ట్ సూట్‌గా పెంచడం
- Python, JavaScript, మరియు C# ట్రాక్స్

**కోడ్ నమూనాలు:**

| భాష | ఫైల్ | వివరణ |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | మూల్యాంకన ఫ్రేమ్‌వర్క్ |
| C# | `csharp/AgentEvaluation.cs` | మూల్యాంకన ఫ్రేమ్‌వర్క్ |
| JavaScript | `javascript/foundry-local-eval.mjs` | మూల్యాంకన ఫ్రేమ్‌వర్క్ |

---

### భాగం 9: Whisperతో వాయిస్ ట్రాన్స్క్రిప్షన్

**ల్యాబ్ గైడ్:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- OpenAI Whisperను లోకల్‌గా నడిపించి స్పీచ్-టు-టెక్స్ట్ ట్రాన్స్క్రిప్షన్
- గోప్యతా-మొదటి ఆడియో ప్రాసెసింగ్ - ఆడియో మీ పరికరాన్ని దాటదు
- Python, JavaScript, మరియు C# ట్రాక్స్‌లో `client.audio.transcriptions.create()` (Python/JS) మరియు `AudioClient.TranscribeAudioAsync()` (C#)
- శిక్షణ కోసం Zava-థీమ్ ఉన్న నమూనా ఆడియో ఫైల్స్ కూడా ఉన్నాయి

**కోడ్ నమూనాలు:**

| భాష | ఫైల్ | వివరణ |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper వాయిస్ ట్రాన్స్క్రిప్షన్ |
| C# | `csharp/WhisperTranscription.cs` | Whisper వాయిస్ ట్రాన్స్క్రిప్షన్ |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper వాయిస్ ట్రాన్స్క్రిప్షన్ |

> **గమనిక:** ఈ ల్యాబ్ **Foundry Local SDK**ని ఉపయోగించి Whisper మోడల్‌ను ప్రోగ్రామాటిక్‌గా డౌన్లోడ్ చేసి లోడ్ చేస్తుంది, తర్వాత ఆడియోను లోకల్ OpenAI-అనుకూల ఎండ్‌పాయింట్‌కు ట్రాన్స్క్రిప్షన్ కోసం పంపుతుంది. Whisper మోడల్ (`whisper`) Foundry Local కేటలాగ్‌లో ఉంది మరియు పూర్తిగా ఆన్-డివైస్ నడుస్తుంది - క్లౌడ్ API కీలు లేదా నెట్‌వర్క్ యాక్సెస్ అవసరం లేదు.

---

### భాగం 10: కస్టమ్ లేదా Hugging Face మోడల్స్ ఉపయోగించడం

**ల్యాబ్ గైడ్:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face మోడల్స్‌ను ONNX రన్టైమ్ GenAI మోడల్ బిల్డర్ ఉపయోగించి ఆప్టిమైజ్ చేయబడిన ONNX ఫార్మాట్‌గా కాంపైల్ చేయడం
- హార్డ్వేర్-స్పెసిఫిక్ కాంపైల్ (CPU, NVIDIA GPU, DirectML, WebGPU) మరియు క్వాంటైజేషన్ (int4, fp16, bf16)
- Foundry Local కోసం చాట్ టెంప్లేట్ కాన్ఫిగరేషన్ ఫైళ్ళు సృష్టించడం
- కాంపైల్ చేసిన మోడల్స్‌ను Foundry Local క్యాష్‌లో చేర్చడం
- CLI, REST API, మరియు OpenAI SDK ద్వారా కస్టమ్ మోడల్స్ నడపడం
- ఉదాహరణకి: Qwen/Qwen3-0.6B ఎండ్-టు-ఎండ్ కాంపైలింగ్

---

### భాగం 11: లోకల్ మోడల్స్‌తో టూల్ కాలింగ్

**ల్యాబ్ గైడ్:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- లోకల్ మోడల్స్‌కు బాహ్య ఫంక్షన్లను పిలవడానికి వీలు కల్పించడం (టూల్/ఫంక్షన్ కాలింగ్)
- OpenAI ఫంక్షన్-కాలింగ్ ఫార్మాట్ ద్వారా టూల్ స్కీమాలను నిర్వచించడం
- బహుమార్గ టూల్-కాలింగ్ సంభాషణ ప్రవాహాన్ని నిర్వహించడం
- టూల్ కాల్స్‌ను లోకల్‌గా అమలు చేసి ఫలితాలను మోడల్‌కు తిరిగి ఇవ్వడం
- టూల్-కాలింగ్ పరిస్థితుల కోసం సరైన మోడల్ ఎంపిక (Qwen 2.5, Phi-4-mini)
- జవాస్క్రిప్ట్ కోసం SDK స్థానిక `ChatClient`ని ఉపయోగించడం

**కోడ్ నమూనాలు:**

| భాష | ఫైల్ | వివరణ |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | వాతావరణం/ప్రజాసంఖ్య టూల్స్‌తో టూల్ కాలింగ్ |
| C# | `csharp/ToolCalling.cs` | .NETతో టూల్ కాలింగ్ |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClientతో టూల్ కాలింగ్ |

---

### భాగం 12: Zava Creative Writer కోసం వెబ్ UI నిర్మాణం

**ల్యాబ్ గైడ్:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writerకి బ్రౌజర్ ఆధారిత ఫ్రంట్ ఎండ్ జోడించడం
- Python (FastAPI), JavaScript (Node.js HTTP), మరియు C# (ASP.NET Core) నుండి భాగస్వామ్య UI సర్వ్ చేయడం
- Fetch API మరియు ReadableStreamతో బ్రౌజర్‌లో స్ట్రీమింగ్ NDJSONను వినియోగించడం
- లైవ్ ఏజెంట్ స్థితి బ్యాడ్జ్‌లు మరియు రియల్-టైమ్ ఆర్టికల్ టెక్స్ట్ స్ట్రీమింగ్

**కోడ్ (భాగస్వామ్య UI):**

| ఫైల్ | వివరణ |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | పేజీ లేఅవుట్ |
| `zava-creative-writer-local/ui/style.css` | శైలీకరణ |
| `zava-creative-writer-local/ui/app.js` | స్ట్రీమ్ రీడర్ మరియు DOM అప్‌డేట్ లాజిక్ |

**బ్యాక్‌ఎండ్ జతలు:**

| భాష | ఫైల్ | వివరణ |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | స్థిర UIని సర్వ్ చేసేలా నవీకరించారు |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ఆర్కెస్ట్రేటర్ చుట్టూ కొత్త HTTP సర్వర్ |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | కొత్త ASP.NET Core మినిమల్ API ప్రాజెక్ట్ |

---

### భాగం 13: కార్యశాల పూర్తి
**ల్యాబ్ గైడ్:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- మీరు నిర్మించిన మొత్తం 12 భాగాల్లోని అన్ని పనుల సారాంశం
- మీ అనువర్తనాలను విస్తరించడానికి అదనపు ఆలోచనలు
- వనరులు మరియు డాక్యుమెంటేషన్ కు లింకులు

---

## ప్రాజెక్ట్ నిర్మాణం

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

## వనరులు

| వనరు | లింక్ |
|----------|------|
| ఫౌండ్రి లోకల్ వెబ్‌സైట్ | [foundrylocal.ai](https://foundrylocal.ai) |
| మోడల్ క్యాటలాగ్ | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| ఫౌండ్రి లోకల్ GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| ప్రారంభ మార్గదర్శిని | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| ఫౌండ్రి లోకల్ SDK సూచిక | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft ఏజెంట్ ఫ్రేమ్‌వర్క్ | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI విస్పర్ | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX రన్‌టైమ్ GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## లైసెన్స్

ఈ వర్క్‌షాప్ మెటీరియల్ విద్యా ప్రయోజనాల కోసం అందించబడింది.

---

**అందంగా నిర్మించండి! 🚀**