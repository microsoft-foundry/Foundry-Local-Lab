<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local వర్క్‌షాప్ - ఆన్-డివైస్ AI యాప్‌ల నిర్మాణం

మీ స్వంత మెషిన్‌లో భాషా మోడల్స్ నడిపించి [Foundry Local](https://foundrylocal.ai) మరియు [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) తో తెలివైన అనువర్తనాలను నిర్మించడానికి ఒక వైపు ప్రాక్టికల్ వర్క్‌షాప్.

> **Foundry Local ఏంటి?** Foundry Local అనేది ఒక లైట్‌వెయిట్ రన్‌టైమ్, ఇది మీ హార్డ్వేర్ మీద మొత్తం భాషా మోడల్స్ డౌన్లోడ్ చేయడానికి, నిర్వహించడానికి మరియు సర్వ్ చేయడానికి అనుమతిస్తుంది. ఇది **OpenAI-అనుకూల API**ని అందిస్తుంది అందుకే OpenAI మాట్లాడే ఏ టూల్ లేదా SDK అయినా కనెక్ట్ కావచ్చు - మేఘ ఖాతా అవసరం లేదు.

### 🌐 బహుభాషా మద్దతు

#### GitHub చర్య ద్వారా మద్దతు (స్వంతం మరియు ఎప్పుడూ నవీకరించబడుతుంది)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[అరబ్బీ](../ar/README.md) | [బెంగాలీ](../bn/README.md) | [బల్గేరియన్](../bg/README.md) | [బర్మీస్ (మయన్మార్)](../my/README.md) | [చైనీస్ (సాదాసీదా)](../zh-CN/README.md) | [చైనీస్ (పారంపర్య, హాంకాంగ్)](../zh-HK/README.md) | [చైనీస్ (పారంపర్య, మకావు)](../zh-MO/README.md) | [చైనీస్ (పారంపర్య, తైవాన్)](../zh-TW/README.md) | [క్రొయేటియన్](../hr/README.md) | [చెక్](../cs/README.md) | [డానిష్](../da/README.md) | [డచ్](../nl/README.md) | [ఎస్తోనియన్](../et/README.md) | [ఫిన్నిష్](../fi/README.md) | [ఫ్రెంచ్](../fr/README.md) | [జర్మన్](../de/README.md) | [గ్రీకు](../el/README.md) | [హిబ్రూ](../he/README.md) | [హిందీ](../hi/README.md) | [హంగేరియన్](../hu/README.md) | [ఇండోనేషియన్](../id/README.md) | [ఇటాలియన్](../it/README.md) | [జాపనీస్](../ja/README.md) | [కన్నడ](../kn/README.md) | [ఖ్మేర్](../km/README.md) | [కొరియన్](../ko/README.md) | [లిథువేనియన్](../lt/README.md) | [మలయ్](../ms/README.md) | [మలయాళం](../ml/README.md) | [మరాఠీ](../mr/README.md) | [నేపాలీ](../ne/README.md) | [నైజీరియన్ పిజిన్](../pcm/README.md) | [నార్వేజియన్](../no/README.md) | [పర్శియన్ (ఫార్సీ)](../fa/README.md) | [పోలిష్](../pl/README.md) | [పోర్చుగీస్ (బ్రెజిల్)](../pt-BR/README.md) | [పోర్చుగీస్ (పోర్చుగల్)](../pt-PT/README.md) | [పంజాబీ (గుర్ముఖి)](../pa/README.md) | [రోమేనియన్](../ro/README.md) | [రష్యన్](../ru/README.md) | [సెర్బియన్ (సిరిలిక్)](../sr/README.md) | [స్లోవాక్](../sk/README.md) | [స్లోవేనియన్](../sl/README.md) | [స్పానిష్](../es/README.md) | [స్వాహిలీ](../sw/README.md) | [స్వీడిష్](../sv/README.md) | [టగాలోగ్ (ఫిలిపినో)](../tl/README.md) | [తమిళ్](../ta/README.md) | [తెలుగు](./README.md) | [థాయి](../th/README.md) | [టర్కిష్](../tr/README.md) | [ఉక్రెయిన్](../uk/README.md) | [ఉర్దూ](../ur/README.md) | [వియత్నామీస్](../vi/README.md)

> **స్థానికంగా క్లోన్ చేయడం ఇష్టం?**
>
> ఈ రిపాజిటరీలో 50+ భాషా అనువాదాలు ఉన్నాయి, ఇది డౌన్లోడ్ పరిమాణాన్ని గణనీయంగా పెంచుతుంది. అనువాదాలు లేకుండా క్లోన్ చేయడానికి స్పార్స్ చెక్‌ఔట్ ఉపయోగించండి:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> ఇది కోర్స్ పూర్తిగా చేయడానికి అవసరమైన ప్రతిదీ మీకు చాలా వేగంగా డౌన్లోడ్ చేస్తుంది.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## అభ్యాస లక్ష్యాలు

ఈ వర్క్‌షాప్ చివరికి మీరు చేయగలరు:

| # | లక్ష్యం |
|---|-----------|
| 1 | Foundry Local ని ఇన్‌స్టాల్ చేసి CLI తో మోడల్స్ నిర్వహించాలి |
| 2 | Foundry Local SDK API నేర్చుకుని ప్రోగ్రామాటిక్ మోడల్ నిర్వహణలో ప్రావీణ్యం సాధించాలి |
| 3 | Python, JavaScript, మరియు C# SDKలతో లోకల్ ఇన్ఫరెన్స్ సర్వర్‌కు కనెక్ట్ కావాలి |
| 4 | మీ స్వంత డేటా ఆధారంగా జవాబులను ఆధారంగా ఉంచే Retrieval-Augmented Generation (RAG) పైప్‌లైన్ నిర్మించాలి |
| 5 | నిరంతర సూచనలు మరియు వ్యక్తిత్వాలతో AI ఏజెంట్లను సృష్టించాలి |
| 6 | రెప్రొగ్యామింగ్ ఫీడ్బ్యాక్ లూప్‌లతో బహుఎజెంట్ వర్క్‌ఫ్లోలను ఏర్పాటు చేయాలి |
| 7 | ప్రొడక్షన్ క్యాప్స్టోన్ యాప్‌ని అన్వేషించాలి - Zava సృజనాత్మక రచయిత |
| 8 | గోల్డెన్ డేటాసెట్లతో మరియు LLM-అజాడ్జ్ స్కోరింగ్‌తో అంచనా ఫ్రేమ్‌వర్క్‌లు నిర్మించాలి |
| 9 | Foundry Local SDK ఉపయోగించి ఆన్-డివైస్ స్పీచ్-టు-టెక్స్ట్ కోసం విస్పర్‌తో ఆటో డయిజెస్ట్ చేయాలి |
| 10 | ONNX Runtime GenAI మరియు Foundry Local తో కస్టమ్ లేదా Hugging Face మోడల్స్ కంపైల్ చేసి నడపాలి |
| 11 | టూల్-కలింగ్ ప్యాటర్నుతో లోకల్ మోడల్స్‌కు బాహ్య ఫంక్షన్లు కాల్ చేయడానికి సవాలు ఇవ్వాలి |
| 12 | రియల్-టైమ్ స్ట్రిమింగ్‌తో Zava సృజనాత్మక రచయిత కొరకు బ్రౌజర్-ఆధారిత UI నిర్మించాలి |

---

## అవసరాలు

| అవసరం | వివరాలు |
|-------------|---------|
| **హార్డ్వేర్** | కనీసం 8 GB RAM (16 GB సిఫార్సు), AVX2-సమర్థ CPU లేదా మద్ధతు ఉన్న GPU |
| **ఆపరేటింగ్ సిస్టమ్** | Windows 10/11 (x64/ARM), Windows Server 2025, లేదా macOS 13+ |
| **Foundry Local CLI** | Windows కోసం `winget install Microsoft.FoundryLocal` లేదా macOS కోసం `brew tap microsoft/foundrylocal && brew install foundrylocal` ద్వారా ఇన్‌స్టాల్ చేయండి. వివరాలకు [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) చూడండి. |
| **భాషా రన్‌టైమ్** | **Python 3.9+** మరియు/లేదా **.NET 9.0+** మరియు/లేదా **Node.js 18+** |
| **Git** | ఈ రిపాజిటరీను క్లోన్ చేయడానికి |

---

## ప్రారంభించడం

```bash
# 1. రిపోజిటరీని క్లోన్ చేయండి
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Foundry Local ఇన్స్టాల్ అయినదీ లేదో తనివీ చేయండి
foundry model list              # అందుబాటులో ఉన్న మోడళ్లను జాబితా చేయండి
foundry model run phi-3.5-mini  # ఒక పరస్పర చర్య చాట్ ప్రారంభించండి

# 3. మీ భాషా ట్రాక్‌ని ఎంచుకోండి (పూర్తి సెటప్ కోసం పార్ట్ 2 లాబ్ చూడండి)
```

| భాష | త్వరితగతిన ప్రారంభం |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## వర్క్‌షాప్ భాగాలు

### భాగం 1: Foundry Local తో ప్రారంభం

**ల్యాబ్ గైడ్:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local అంటే ఏమిటి మరియు ఇది ఎలా పని చేస్తుంది
- Windows మరియు macOS పై CLI ఇన్‌స్టాల్ చేయడం
- మోడల్స్ అన్వేషణ - జాబితా, డౌన్లోడ్, నడిపించడం
- మోడల్ అలియాసులు మరియు డైనమిక్ పోర్ట్లు అర్థం చేసుకోవడం

---

### భాగం 2: Foundry Local SDK లో లోతైన అవగాహన

**ల్యాబ్ గైడ్:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- అప్లికేషన్ డెవలప్‌మెంట్ కోసం CLI కంటే SDKను ఎందుకు ఉపయోగించాలి
- Python, JavaScript, C#కి SDK API సంపూర్ణ సూత్రాలు
- సర్వీస్ నిర్వహణ, కాటలాగ్ బ్రౌజింగ్, మోడల్ లైఫ్ సైకిల్ (డౌన్లోడ్, లోడ్, అన్‌లోడ్)
- త్వరిత అలవాటు నమూనాలు: Python కన్‌స్ట్రక్టర్ బూట్‌స్ట్రాప్, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` మెటాడేటా, అలియాసులు మరియు హార్డ్వేర్-ఆప్టిమల్ మోడల్ ఎంపిక

---

### భాగం 3: SDKలు మరియు APIలు

**ల్యాబ్ గైడ్:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, C# నుండి Foundry Local కు కనెక్ట్ కావడం
- Foundry Local SDK ని ఉపయోగించి సర్వీస్ ప్రోగ్రామాటిక్ గా నిర్వహించడం
- OpenAI-అనుకూల API ద్వారా స్ట్రీమింగ్ చాట్ పూర్తి
- ప్రతి భాషకి SDK పద్ధతి సూచిక

**కోడు నమూనాలు:**

| భాష | ఫైల్ | వివరణ |
|----------|------|-------------|
| Python | `python/foundry-local.py` | ప్రాథమిక స్ట్రీమింగ్ చాట్ |
| C# | `csharp/BasicChat.cs` | .NET తో స్ట్రీమింగ్ చాట్ |
| JavaScript | `javascript/foundry-local.mjs` | Node.js తో స్ట్రీమింగ్ చాట్ |

---

### భాగం 4: Retrieval-Augmented Generation (RAG)

**ల్యాబ్ గైడ్:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG అంటే ఏమిటి మరియు దాని ప్రాముఖ్యం
- ఒక ఇన్-మెమరీ నాలెడ్జ్ బేస్ కట్టడం
- స్కోరింగ్‌తో కీవర్డ్ ఓవర్‌లాప్ రిట్రీవల్
- గ్రౌండెడ్ సిస్టమ్ ప్రాంప్ట్స్ తీయడం
- ఆన్-డివైస్ పూర్తి RAG పైప్‌లైన్ నడిపించడం

**కోడు నమూనాలు:**

| భాష | ఫైల్ |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### భాగం 5: AI ఏజెంట్లు నిర్మించడం

**ల్యాబ్ గైడ్:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI ఏజెంట్ అంటే ఏమిటి (సాధారణ LLM కాల్ కు వ్యతిరేకంగా)
- `ChatAgent` నమూనా మరియు Microsoft Agent Framework
- సిస్టమ్ సూచనలు, వ్యక్తిత్వాలు, బహుం-మార్గ సంభాషణలు
- ఏజెంట్ల నుండి సాంప్రదాయబద్ధమైన అవుట్‌పుట్ (JSON)

**కోడు నమూనాలు:**

| భాష | ఫైల్ | వివరణ |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | ఏజెంట్ ఫ్రేమ్‌వర్క్ తో ఒక ఏజెంట్ |
| C# | `csharp/SingleAgent.cs` | ఒక ఏజెంట్ (ChatAgent నమూనా) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | ఒక ఏజెంట్ (ChatAgent నమూనా) |

---

### భాగం 6: బహుఎజెంట్ వర్క్‌ఫ్లోలు

**ల్యాబ్ గైడ్:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- బహుఎజెంట్ పైప్‌లైన్లు: పరిశోధక → రచయిత → సంపాదకుడు
- క్రమపద్ధతిగా నిర్వహణ మరియు ఫీడ్బ్యాక్ లూప్‌లు
- పంచుకున్న ఆకృతి మరియు నిర్మాణాత్మక హ్యాండ్-ఆఫ్స్
- మీ స్వంత బహుఎజెంట్ వర్క్‌ఫ్లో రూపకల్పన

**కోడు నమూనాలు:**

| భాష | ఫైల్ | వివరణ |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | మూడు ఏజెంట్ల పైప్‌లైన్ |
| C# | `csharp/MultiAgent.cs` | మూడు ఏజెంట్ల పైప్‌లైన్ |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | మూడు ఏజెంట్ల పైప్‌లైన్ |

---

### భాగం 7: Zava సృజనాత్మక రచయిత - క్యాప్స్టోన్ అనువర్తనం

**ల్యాబ్ గైడ్:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- నాలుగు ప్రత్యేక ఏజెంట్లతో ప్రొడక్షన్-శैली బహుఎజెంట్ యాప్
- క్రమపద్ధతిలో ఎవాల్యుయేటర్ మార్గదర్శకమైన ఫీడ్బ్యాక్ లూప్‌లు
- స్ట్రీమింగ్ అవుట్‌పుట్, ఉత్పత్తి కాటాలాగ్ శోధన, నిర్మాణాత్మక JSON హ్యాండ్-ఆఫ్స్
- Python (FastAPI), JavaScript (Node.js CLI), మరియు C# (.NET కన్సోల్)లో పూర్తి అమలు

**కోడు నమూనాలు:**

| భాష | డైరెక్టరీ | వివరణ |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | ఆర్కెస్ట్‌రేటర్ తో FastAPI వెబ్ సర్వీస్ |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI అనువర్తనం |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 కన్సోల్ అనువర్తనం |

---

### భాగం 8: అంచనా-ఆధారిత అభివృద్ధి

**ల్యాబ్ గైడ్:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- గోల్డెన్ డేటాసెట్‌లతో AI ఏజెంట్లకు ఒక వ్యూహాత్మక అంచనా ఫ్రేమ్‌వర్క్ నిర్మించండి
- నియమాల ఆధారిత తనిఖీలు (పొడవు, కీవర్డ్ కవరేజ్, నిషేధిత పదాలు) + LLM-గా న్యాయమూర్తి స్కోరింగ్
- ప్రాంప్ట్ వేరియంట్‌ల పక్కన పక్కన పోలికతో సమగ్ర స్కోర్‌కార్డులు
- భాగం 7 ల నుండి Zava ఎడిటర్ ఏజెంట్ నమూనాను ఆఫ్‌లైన్ పరీక్షా సూట్‌గా విస్తరించు
- Python, JavaScript, మరియు C# ట్రాక్స్

**కోడు నమూనాలు:**

| భాష | ఫైల్ | వివరణ |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | అంచనా ఫ్రేమ్‌వర్క్ |
| C# | `csharp/AgentEvaluation.cs` | అంచనా ఫ్రేమ్‌వర్క్ |
| JavaScript | `javascript/foundry-local-eval.mjs` | అంచనా ఫ్రేమ్‌వర్క్ |

---

### భాగం 9: విస్పర్‌తో వాయిస్ ట్రాన్స్‌క్రిప్షన్

**ల్యాబ్ గైడ్:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- లోకల్‌గా నడుస్తున్న OpenAI Whisper ఉపయోగించి స్పీచ్-టు-టెక్స్ట్ ట్రాన్స్క్రిప్షన్  
- గోప్యతా-ముందస్తు ఆడియో ప్రాసెసింగ్ - ఆడియో మీరు ఉపయోగిస్తున్న పరికరం నుండి ఎప్పుడూ బయలుదేరదు  
- Python, JavaScript, మరియు C# ట్రాక్స్ `client.audio.transcriptions.create()` (Python/JS) మరియు `AudioClient.TranscribeAudioAsync()` (C#) తో  
- ప్రాక్టిస్ కోసం Zava-థీమ్ సాంపిల్ ఆడియో ఫైల్స్ అందుబాటులో ఉన్నాయి  

**కోడ్ నమూనాలు:**  

| భాష | ఫైల్ | వివరణ |  
|----------|------|-------------|  
| Python | `python/foundry-local-whisper.py` | విస్పర్ వాయిస్ ట్రాన్స్క్రిప్షన్ |  
| C# | `csharp/WhisperTranscription.cs` | విస్పర్ వాయిస్ ట్రాన్స్క్రిప్షన్ |  
| JavaScript | `javascript/foundry-local-whisper.mjs` | విస్పర్ వాయిస్ ట్రాన్స్క్రిప్షన్ |  

> **గమనిక:** ఈ ప్రయోగశాలలో **Foundry Local SDK** ను ఉపయోగించి ప్రోగ్రామేటిక్‌గా విస్పర్ మోడల్ డౌన్‌లోడ్ చేసి లోడ్ చేస్తుంది, ఆపై ఆడియోను స్థానిక OpenAI-అనుకూల ఎండ్పాయింట్‌కు పంపి ట్రాన్స్క్రిప్షన్ చేస్తుంది. విస్పర్ మోడల్ (`whisper`) Foundry Local క్యాటలాగ్లో జాబితా చేయబడింది మరియు పూర్తిగా పరికరంలోనే నడుస్తుంది - క్లౌడ్ API కీలు లేకపోతో, నెట్‌వర్క్ యాక్సెస్ అవసరం లేదు.  

---

### భాగం 10: కస్టమ్ లేదా Hugging Face మోడల్స్ ఉపయోగింపు  

**ప్రయోగశాల గైడ్:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)  

- Hugging Face మోడల్స్‌ని ONNX Runtime GenAI మోడల్ బిల్డర్ ఉపయోగించి ఆప్టిమైజ్ చేసిన ONNX ఫార్మాట్‌కు కంపైల్ చేయడం  
- హార్డ్వేర్-ప్రత్యేక కంపైల్ (CPU, NVIDIA GPU, DirectML, WebGPU) మరియు క్వాంటైజేషన్ (int4, fp16, bf16)  
- Foundry Local కోసం చాట్-టెంప్లేట్ కాన్ఫిగరేషన్ ఫైళ్ళను సృష్టించడం  
- కంపైల్ చేసిన మోడల్స్‌ను Foundry Local కాచ్‌లో చేర్చడం  
- CLI, REST API, మరియు OpenAI SDK ద్వారా కస్టమ్ మోడల్స్ నడపడం  
- ఉదాహరణరూపం: Qwen/Qwen3-0.6B మొత్తం కంపైల్ చేయడం  

---

### భాగం 11: స్థానిక మోడల్స్‌తో టూల్ కాలింగ్  

**ప్రయోగశాల గైడ్:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)  

- స్థానిక మోడల్స్ ఎగ్జటర్నల్ ఫంక్షన్లను పిలవడానికి అనుమతించడం (టూల్/ఫంక్షన్ కాలింగ్)  
- OpenAI ఫంక్షన్-కాలింగ్ ఫార్మాట్ ఉపయోగించి టూల్ స్కీమాలు నిర్వచించండి  
- బహుళ-తిరుగుడు టూల్-కాలింగ్ సంభాషణ ప్రవాహాన్ని నిర్వహించండి  
- స్థానికంగా టూల్ కాల్స్ అమలు చేసి ఫలితాలను మోడల్‌కు తిరిగి పంపండి  
- టూల్-కాలింగ్ సందర్భాలకు సరైన మోడల్ ఎంచుకోండి (Qwen 2.5, Phi-4-mini)  
- SDK లో మాతృక `ChatClient` ఉపయోగించి టూల్ కాలింగ్ చేయండి (JavaScript)  

**కోడ్ నమూనాలు:**  

| భాష | ఫైల్ | వివరణ |  
|----------|------|-------------|  
| Python | `python/foundry-local-tool-calling.py` | వాతావరణం/జనాభా టూల్స్ తో టూల్ కాలింగ్ |  
| C# | `csharp/ToolCalling.cs` | .NET తో టూల్ కాలింగ్ |  
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient తో టూల్ కాలింగ్ |  

---

### భాగం 12: Zava క్రియేటివ్ రైటర్ కోసం వెబ్ UI నిర్మాణం  

**ప్రయోగశాల గైడ్:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)  

- Zava క్రియేటివ్ రైటర్‌కు బ్రౌజర్-ఆధారిత ఫ్రంట్ ఎండ్ చేర్చండి  
- Python (FastAPI), JavaScript (Node.js HTTP), మరియు C# (ASP.NET Core) నుండి షేర్ చేసిన UI ను సర్వ్ చేయండి  
- Fetch API మరియు ReadableStream తో బ్రౌజర్‌లో స్ట్రీమింగ్ NDJSON‌ను వినియోగించండి  
- లైవ్ ఏజెంట్ స్థితి బ్యాడ్జెస్ మరియు రియల్-టైం ఆర్టికల్ టెక్స్ట్ స్ట్రీమింగ్  

**కోడ్ (షేర్ చేసిన UI):**  

| ఫైల్ | వివరణ |  
|------|-------------|  
| `zava-creative-writer-local/ui/index.html` | పేజీ లేఅవుట్ |  
| `zava-creative-writer-local/ui/style.css` | స్టైలింగ్ |  
| `zava-creative-writer-local/ui/app.js` | స్ట్రీమ్ రీడర్ మరియు DOM అప్డేట్ లాజిక్ |  

**బ్యాక్ ఎండ్ చేర్పింపులు:**  

| భాష | ఫైల్ | వివరణ |  
|----------|------|-------------|  
| Python | `zava-creative-writer-local/src/api/main.py` | స్టాటిక్ UI సర్వ్ చేయడానికి నవీకరణ చేసినది |  
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ఆర్కిస్ట్రేటర్‌ను చుట్టి కొత్త HTTP సర్వర్ |  
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | కొత్త ASP.NET Core మినిమల్ API ప్రాజెక్టు |  

---

### భాగం 13: వర్క్‌షాప్ పూర్తయింది  

**ప్రయోగశాల గైడ్:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)  

- మీరు 12 భాగాల మొత్తం నిర్మించిన దాని సమీక్ష  
- మీ అప్లికేషన్లను మరింత విస్తరించే ఆలోచనలు  
- వనరులు మరియు డాక్యుమెంటేషన్లకు లింకులు  

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
| Foundry Local వెబ్‌సైట్ | [foundrylocal.ai](https://foundrylocal.ai) |  
| మోడల్ క్యాటలాగ్ | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |  
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |  
| ప్రారంభించుకోవడానికి గైడ్ | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |  
| Foundry Local SDK సూచిక | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |  
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |  
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |  
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |  

---

## లైసెన్స్  

ఈ వర్క్‌షాప్ మెటీరియల్ విద్యార్థుల ప్రయోజనాలకు అందించబడింది.  

---

**సంతోషంగా నిర్మించండి! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**్డిస్క్లెయిమర్**:  
ఈ డాక్యుమెంట్‌ను AI అనువాద సేవ [Co-op Translator](https://github.com/Azure/co-op-translator) ఉపయోగించి అనువదించబడింది. మేము ఖచ్చితత్వానికి ప్రయత్నిస్తున్నప్పటికీ, ఆటోమేటెడ్ అనువాదాల్లో పొరపాట్లు లేదా తప్పుడు వివరాలు ఉండవచ్చు. ప్రామాణికమైన మూలం native భాషలో ఉన్న ప్రాథమిక డాక్యుమెంట్‌ని పరిగణించాలి. ముఖ్యమైన సమాచారం కొరకు, ప్రొఫెషనల్ హ్యూమన్ అనువాదాన్ని సిఫారసు చేస్తున్నాము. ఈ అనువాదం వలన సంభవించే ఏవైనా అపార్థాలు లేదా తప్పైన వివరణలకు మేము బాధ్యత వహించము.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->