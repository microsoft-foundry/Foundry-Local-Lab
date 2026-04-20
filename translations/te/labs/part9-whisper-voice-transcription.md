![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# భాగం 9: Whisper మరియు Foundry Local తో వాయిస్ ట్రాన్స్క్రిప్షన్

> **లక్ష్యం:** Foundry Local ద్వారా స్థానికంగా నడిచే OpenAI Whisper మోడల్ ఉపయోగించి ఆడియో ఫైళ్లను ట్రాన్స్క్రైబ్ చేయడం - పూర్తిగా పరికరంలోనే, క్లౌడ్ అవసరం లేదు.

## అవలోకనం

Foundry Local కేవలం టెక్స్ట్ ఉత్పత్తికోసం కాదు; ఇది **స్పీచ్-టు-టెక్స్ట్** మోడల్స్‌కు కూడా మద్దతు ఇస్తుంది. ఈ ల్యాబ్‌లో మీరు **OpenAI Whisper Medium** మోడల్ ఉపయోగించి ఆడియో ఫైల్స్‌ను పూర్తిగా మీ యంత్రంలో ట్రాన్స్క్రిప్ట్ చేస్తారు. ఇది Zava కస్టమర్ సర్వీస్ కాల్స్, ఉత్పత్తి సమీక్ష రికార్డింగ్స్, లేదా వర్క్‌షాప్ ప్లానింగ్ సెషన్స్ వంటి పరిస్థితులకు అనువైనది, ముఖ్యంగా ఆడియో డేటా మీ పరికరం నుంచి ఎవరికీ వెలుపలకు వెళ్లకూడదు.

---

## నేర్చుకునే లక్ష్యాలు

ఈ ల్యాబ్ చివరికి మీరు చేయగలుగుతారు:

- Whisper స్పీచ్-టు-టెక్స్ట్ మోడల్ మరియు దాని సామర్థ్యాలను అర్థం చేసుకోవడం
- Foundry Local ఉపయోగించి Whisper మోడల్ డౌన్‌లోడ్ చేసి నడపడం
- Python, JavaScript, మరియు C# లో Foundry Local SDK ద్వారా ఆడియో ఫైల్స్ ట్రాన్స్క్రైబ్ చేయడం
- పూర్తిగా పరికరం లోనే నడిచే సులభమైన ట్రాన్స్క్రిప్షన్ సర్వీసును నిర్మించడం
- Foundry Localలో చాట్/టెక్స్ట్ మోడల్స్ మరియు ఆడియో మోడల్స్ మధ్య వ్యత్యాసాలు అర్థం చేసుకోవడం

---

## ముందస్తు అవసరాలు

| అవసరం | వివరాలు |
|-------------|---------|
| **Foundry Local CLI** | వెర్షన్ **0.8.101 లేదా పైగా** (Whisper మోడల్స్ v0.8.101 నుండి అందుబాటులో ఉన్నాయి) |
| **ఏపరేటింగ్ సిస్టమ్** | Windows 10/11 (x64 లేదా ARM64) |
| **భాషా రన్నింగ్ టైమ్** | **Python 3.9+** మరియు/లేదా **Node.js 18+** మరియు/లేదా **.NET 9 SDK** ([.NET డౌన్‌లోడ్](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **పూర్తి అయినవి** | [భాగం 1: ప్రారంభం](part1-getting-started.md), [భాగం 2: Foundry Local SDK లో లోతైన అధ్యయనం](part2-foundry-local-sdk.md), మరియు [భాగం 3: SDKలు మరియు APIs](part3-sdk-and-apis.md) |

> **గమనిక:** Whisper మోడల్స్ **SDK** ద్వారా డౌన్‌లోడ్ చేయాలి (CLI ద్వారా కాదు). CLI ఆడియో ట్రాన్స్క్రిప్షన్ ఎండ్పాయింట్‌ను మద్దతు ఇవ్వదు. మీ వెర్షన్‌ను ఇలా చెక్ చేయండి:
> ```bash
> foundry --version
> ```

---

## సూత్రం: Whisper Foundry Local తో ఎలా పనిచేస్తుంది

OpenAI Whisper మోడల్ అనే అది విస్తృతంగా విభిన్న ఆడియో డేటా పై శిక్షణ పొందిన సాధారణ-పురోగమ స్పీచ్ గుర్తింపు మోడల్. Foundry Local ద్వారా నడుపుతున్నప్పుడు:

- మోడల్ పూర్తిగా **మీ CPU పై నడుస్తుంది** - GPU అవసరం లేదు
- ఆడియో మీ పరికరం నుండి ఎప్పుడూ బయటకు వెళ్లదు - **పూర్తి ప్రైవసీ**
- Foundry Local SDK మోడల్ డౌన్‌లోడ్ మరియు క్యాష్ నిర్వహణను నిర్వహిస్తుంది
- **JavaScript మరియు C#** SDKలో బిల్ట్-ఇన్ `AudioClient` ఉంటుంది, ఇది ట్రాన్స్క్రిప్షన్ మొత్తం పైప్లైన్‌ను నిర్వహిస్తుంది — ONNX సెటప్ లేకుండా
- **Python** SDK ని మోడల్ నిర్వహణకు ఉపయోగిస్తుంది మరియు ENCODER/DECODER ONNX మోడల్స్‌ మీద నేరుగా నిష్పత్తి కోసం ONNX రన్నింగ్ టైమ్ ఉపయోగిస్తాయి

### పైప్లైన్ ఎలా పనిచేస్తుంది (JavaScript మరియు C#) — SDK AudioClient

1. **Foundry Local SDK** Whisper మోడల్‌ను డౌన్‌లోడ్ చేసి క్యాష్ చేస్తుంది
2. `model.createAudioClient()` (JS) లేదా `model.GetAudioClientAsync()` (C#) ఒక `AudioClient` ను సృష్టిస్తుంది
3. `audioClient.transcribe(path)` (JS) లేదా `audioClient.TranscribeAudioAsync(path)` (C#) పూర్తి పైప్లైన్ (ఆడియో ప్రీఫ్రొసెసింగ్, ఎన్కోడర్, డికోడర్, టోక్‌న్ల డికోడింగ్) ను అంతర్గతంగా నిర్వహిస్తుంది
4. `AudioClient` లో `settings.language` ప్రాపర్టీ (ఇంగ్లీష్ కోసం `"en"` గా సెట్ చేయాలి) ఉంటుంది, ట్రాన్స్క్రిప్షన్ సవ్యంగా ఉండేందుకు గైడ్ చేస్తుంది

### పైప్లైన్ ఎలా పనిచేస్తుంది (Python) — ONNX రన్నింగ్ టైమ్

1. **Foundry Local SDK** Whisper ONNX మోడల్ ఫైళ్లను డౌన్‌లోడ్ చేసి క్యాష్ చేస్తుంది
2. **ఆడియో ప్రీఫ్రొసెసింగ్** WAV ఆడియోను మెల్ స్పెక్ట్రోగ్రామ్‌గా మార్చుతుంది (80 మెల్ బిన్‌లు x 3000 ఫ్రేములు)
3. **ఎన్కోడర్** మెల్ స్పెక్ట్రోగ్రామ్‌ను ప్రాసెస్ చేసి హిడెన్ స్టేట్స్ మరియు క్రాస్-అటెన్షన్ కీ/వెల్యూ టెన్సార్లు ఉత్పత్తి చేస్తుంది
4. **డికోడర్** ఆటోరెగ్రెసివ్‌గా ఒకటి ఒక్క టోకెన్ ఉత్పత్తి చేస్తూ చివరికి ఎండ్-ఆఫ్-టెక్స్ట్ టోకెన్‌ను ఉత్పత్తి చేస్తుంది
5. **టోకనైజర్** అవుట్‌పుట్ టోకెన్ ఐడిలను తిరిగి చదవదగిన టెక్స్ట్‌గా డికోడ్ చేస్తుంది

### Whisper మోడల్ వేరియంట్లు

| అలియాస్ | మోడల్ ID | పరికరం | పరిమాణం | వివరణ |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-అక్సిలరేటెడ్ (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU-ఆప్టిమైజ్డ్ (బహుళ పరికరాలకు సూచించబడింది) |

> **గమనిక:** చాట్ మోడల్స్ వలె డిఫాల్ట్‌గా లిస్టు కాకుండా, Whisper మోడల్స్ `automatic-speech-recognition` టాస్కు కింద వర్గీకరించబడ్డాయి. వివరాలు తెలుసుకోవడానికి `foundry model info whisper-medium` ను ఉపయోగించండి.

---

## ల్యాబ్ వ్యాయామాలు

### వ్యాయామం 0 - సాంపిల్ ఆడియో ఫైళ్లు పొందండి

ఈ ల్యాబ్‌లో Zava DIY ఉత్పత్తుల సన్నివేశాలపై ఆధారపడి ప్రీ-బిల్డ్ చేసిన WAV ఫైళ్లు ఉంటాయి. అవి క్రింది స్క్రిప్ట్ ద్వారా ఉత్పత్తి చేయండి:

```bash
# రిపో రూట్ నుండి - ముందుగా .venv ను సృష్టించి యాక్టివేట్ చేయండి
python -m venv .venv

# విండోస్ (పవర్‌షెల్):
.venv\Scripts\Activate.ps1
# మాక్ఓఎస్:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```
  
ఇది `samples/audio/` లో ఆరు WAV ఫైళ్ళను సృష్టిస్తుంది:

| ఫైలు | సన్నివేశం |
|------|----------|
| `zava-customer-inquiry.wav` | కస్టమర్ **Zava ProGrip Cordless Drill** గురించి అడుగుతారు |
| `zava-product-review.wav` | కస్టమర్ **Zava UltraSmooth Interior Paint** ను సమీక్షిస్తున్నారు |
| `zava-support-call.wav` | **Zava TitanLock Tool Chest** గురించి సపోర్ట్ కాల్ |
| `zava-project-planning.wav` | DIYer **Zava EcoBoard Composite Decking** ఉపయోగించి డెక్ ప్లాన్ చేస్తున్నారు |
| `zava-workshop-setup.wav` | **అన్ని ఐదు Zava ఉత్పత్తులను** ఉపయోగించి వర్క్‌షాప్పు పరిచయం |
| `zava-full-project-walkthrough.wav` | **అన్ని Zava ఉత్పత్తుల** పొడుగు గేరేజ్ రీనోవేషన్ పరిచయం (~4 నిమిషాలు, దీర్ఘ-ఆడియో టెస్టింగ్ కోసం) |

> **టిప్:** మీరు మీ స్వంత WAV/MP3/M4A ఫైళ్లను కూడా ఉపయోగించవచ్చు, లేదా Windows వాయిస్ రికార్డర్‌తో స్వయంగా రికార్డు చేయవచ్చు.

---

### వ్యాయామం 1 - SDK ఉపయోగించి Whisper మోడల్ డౌన్‌లోడ్ చేయండి

కొత్త Foundry Local వెర్షన్లలో CLI ద్వారా Whisper మోడల్స్ పనిచేయకపోవడంతో, మోడల్‌ను డౌన్‌లోడ్ చేసి లోడ్ చేయడానికి **SDK**ని ఉపయోగించండి. మీ భాషను ఎంచుకోండి:

<details>
<summary><b>🐍 Python</b></summary>

**SDK ఇన్‌స్టాల్ చెయండి:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# సేవను ప్రారంభించండి
manager = FoundryLocalManager()
manager.start_service()

# క్యాటలాగ్ సమాచారం తనిఖీ చేయండి
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# ఇప్పటికే క్యాష్‌లో ఉందా అని చూడండి
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# మోడల్‌ను మెమరీలో లోడ్ చేయండి
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py` గా సేవ్ చేసి నడపండి:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK ఇన్‌స్టాల్ చెయండి:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// మేనేజర్‌ను సృష్టించి సేవను ప్రారంభించండి
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// క్యాటలాగ్ నుండి మోడల్‌ను పొందండి
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.id}`);

if (model.isCached) {
  console.log("Whisper model already downloaded.");
} else {
  console.log("Downloading Whisper model (this may take several minutes)...");
  await model.download();
  console.log("Download complete.");
}

// మోడల్‌ను మెమరీలో లోడ్ చేయండి
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs` గా సేవ్ చేసి నడపండి:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK ఇన్‌స్టాల్ చెయండి:**
```bash
dotnet add package Microsoft.AI.Foundry.Local
```

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "whisper-medium";

// Start the service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);
Console.WriteLine($"Model: {model.Id}");

// Check if already cached
var isCached = await model.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine("Whisper model already downloaded.");
}
else
{
    Console.WriteLine("Downloading Whisper model (this may take several minutes)...");
    await model.DownloadAsync(null, default);
    Console.WriteLine("Download complete.");
}

// Load the model into memory
await model.LoadAsync(default);
Console.WriteLine($"Whisper model loaded: {model.Id}");
```

</details>

> **ఏ SDK? CLI కాకపోవడమూ ఎందుకు?** Foundry Local CLI గానీ నేరుగా Whisper మోడల్స్ డౌన్‌లోడ్ లేదా సర్వ్ చేయడానికి మద్దతు ఇవ్వదు. SDK ప్రోగ్రామేటిక్‌గా ఆడియో మోడల్స్‌ను డౌన్‌లోడ్ చేసి నిర్వహించడానికి విశ్వసనీయ మార్గం అందిస్తుంది. JavaScript మరియు C# SDKలు లో బిల్ట్-ఇన్ `AudioClient` కలిగి ఉన్నాయి, ఇది మొత్తం ట్రాన్స్క్రిప్షన్ పైప్లైన్‌ను నిర్వహిస్తుంది. Python ONNX రన్నింగ్ టైమ్‌ను డైరెక్ట్ ఇన్ఫెరెన్స్ కోసం మోడల్ ఫైళ్లపై వాడుతుంది.

---

### వ్యాయామం 2 - Whisper SDK అర్ధం చేసుకోండి

Whisper ట్రాన్స్క్రిప్షన్ భాష ప్రాతిపదికన వివిధ విధానాలు ఉపయోగిస్తుంది. **JavaScript మరియు C#** Foundry Local SDKలో బిల్ట్-ఇన్ `AudioClient` ను కలిగి ఉంటాయి, ఇది పూర్తిగా ట్రాన్స్క్రిప్షన్ పైప్లైన్‌ను ఒకే మినహాయింపు (ఆడియో ప్రీఫ్రొసెసింగ్, ఎన్కోడర్, డికోడర్, టోకెన్ డికోడింగ్) లో నిర్వహిస్తుంది. **Python** మోడల్ నిర్వహణ కోసం Foundry Local SDK, మరియు ఎన్కోడర్/డికోడర్ ONNX మోడల్స్‌పై నేరుగా ఇన్ఫెరెన్స్ కోసం ONNX Runtime ఉపయోగిస్తుంది.

| భాగం | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK ప్యాకేజ్లు** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **మోడల్ నిర్వహణ** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **ఫీచర్ ఎక్స్‌ట్రాక్షన్** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` ద్వారా నిర్వహించబడుతుంది | SDK `AudioClient` ద్వారా నిర్వహించబడుతుంది |
| **ఇన్ఫెరెన్స్** | `ort.InferenceSession` (ఎన్కోడర్ + డికోడర్) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **టోకెన్ డికోడింగ్** | `WhisperTokenizer` | SDK `AudioClient` ద్వారా నిర్వహించబడుతుంది | SDK `AudioClient` ద్వారా నిర్వహించబడుతుంది |
| **భాషా సెట్టింగ్** | డికోడర్ టోకెన్లలో `forced_ids` ద్వారా సెట్ చేస్తారు | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **ఇన్పుట్** | WAV ఫైల్ పాత్ | WAV ఫైల్ పాత్ | WAV ఫైల్ పాత్ |
| **అవుట్‌పుట్** | డికోడ్ చేసిన టెక్స్ట్ స్ట్రింగ్ | `result.text` | `result.Text` |

> **అత్యవసరం:** ఎప్పుడూ `AudioClient` పై భాషను సెట్ చేయండి (ఉదా: ఇంగ్లీష్ కోసం `"en"`). స్పష్టమైన భాషా సెట్టింగ్ లేకపోతే, మోడల్ ఆटो-డిటెక్ట్ చేస్తూ అర్థం కాని అవుట్‌పుట్ ఇస్తుంది.

> **SDK నమూనాలు:** Python `FoundryLocalManager(alias)` ఉపయోగించి బూట్‌స్ట్రాప్ అవుతుంది, తర్వాత `get_cache_location()`తో ONNX మోడల్ ఫైళ్లను వెతుకుతుంది. JavaScript మరియు C# SDKలో బిల్ట్-ఇన్ `AudioClient` ఉంటుంది - ఇది `model.createAudioClient()` (JS) లేదా `model.GetAudioClientAsync()` (C#) ద్వారా పొందుతారు - ఇది పూర్తి ట్రాన్స్క్రిప్షన్ పైప్లైన్‌ను నిర్వహిస్తుంది. పూర్తి వివరాలకు [భాగం 2: Foundry Local SDK లో లోతైన అధ్యయనం](part2-foundry-local-sdk.md) చూడండి.

---

### వ్యాయామం 3 - సింపుల్ ట్రాన్స్క్రిప్షన్ యాప్ నిర్మించండి

మీ ভাষా ట్రాక్ ఎంపిక చేసుకుని ఒక ఆడియో ఫైల్ ట్రాన్స్క్రైబ్ చేసే మినిమల్ యాప్ తయారుచేయండి.

> **మద్దతుతో ఉన్న ఆడియో ఫార్మాట్స్:** WAV, MP3, M4A. ఉత్తమ ఫలితాల కోసం 16kHz సాంపుల్ రేట్ ఉన్న WAV ఫైళ్ళను ఉపయోగించండి.

<details>
<summary><h3>Python ట్రాక్</h3></summary>

#### సెట్‌అప్

```bash
cd python
python -m venv venv

# వర్చువల్ వాతావరణాన్ని సక్రియం చేయండి:
# విండోస్ (పవర్‌షెల్):
venv\Scripts\Activate.ps1
# మాక్‌ఓఎస్:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### ట్రాన్స్క్రిప్షన్ కోడ్

`foundry-local-whisper.py` అనే ఫైల్ సృష్టించండి:

```python
import sys
import os
import numpy as np
import onnxruntime as ort
import librosa
from transformers import WhisperFeatureExtractor, WhisperTokenizer
from foundry_local import FoundryLocalManager

model_alias = "whisper-medium"
audio_file = sys.argv[1] if len(sys.argv) > 1 else "sample.wav"

if not os.path.exists(audio_file):
    print(f"Audio file not found: {audio_file}")
    sys.exit(1)

# దశ 1: బూట్‌స్ట్రాప్ - సర్వీస్ ప్రారంభిస్తుంది, డౌన్‌లోడ్ చేస్తుంది, మరియు మోడల్ లోడ్ చేస్తుంది
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# క్యాచ్డ్ ONNX మోడల్ ఫైల్‌లకు మార్గం నిర్మించండి
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# దశ 2: ONNX సెషన్లు మరియు ఫీచర్ ఎక్స్‌ట్రాక్టర్‌ను లోడ్ చేయండి
encoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_encoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
decoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_decoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
fe = WhisperFeatureExtractor.from_pretrained(model_dir)
tokenizer = WhisperTokenizer.from_pretrained(model_dir)

# దశ 3: మెల్ స్పెక్టోగ్రామ్ ఫీచర్లను ఎక్స్‌ట్రాక్ట్ చేయండి
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# దశ 4: ఎంకోడర్‌ను నడిపించండి
enc_out = encoder.run(None, {"audio_features": input_features})
# మొదటి అవుట్పుట్ ప్రాచీకృత స్థితులు; మిగతావి క్రాస్-అటెన్షన్ KV జంటలు
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# దశ 5: ఆటోరెగ్రెసివ్ డీకోడింగ్
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, ట్రాన్స్క్రయిబ్, నోటిస్టాంప్స్
input_ids = np.array([initial_tokens], dtype=np.int32)

# ఖాళీ స్వయం-అటెన్షన్ KV క్యాచ్
self_kv = {}
for i in range(24):
    self_kv[f"past_key_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)
    self_kv[f"past_value_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)

generated = []
for _ in range(448):
    feeds = {"input_ids": input_ids, **cross_kv, **self_kv}
    outputs = decoder.run(None, feeds)
    logits = outputs[0]
    next_token = int(np.argmax(logits[0, -1, :]))

    if next_token == 50257:  # టెక్స్ట్ ముగింపు
        break
    generated.append(next_token)

    # స్వయం-అటెన్షన్ KV క్యాచ్ నవీకరించండి
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### దాన్ని నడపండి

```bash
# Zava ఉత్పత్తి పరిస్థితిని ట్రాన్స్‌క్రైబ్ చేయండి
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# లేదా ఇతరులను ప్రయత్నించండి:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Python ముఖ్యమైన అంశాలు

| పద్ధతి | ప్రయోజనం |
|--------|---------|
| `FoundryLocalManager(alias)` | బూట్‌స్ట్రాప్: సర్వీస్ ప్రారంభం, డౌన్‌లోడ్, మోడల్ లోడ్ చేయడం |
| `manager.get_cache_location()` | క్యాష్ అయిన ONNX మోడల్ ఫైళ్లకు పాత్ పొందడం |
| `WhisperFeatureExtractor.from_pretrained()` | మెల్ స్పెక్ట్రోగ్రామ్ ఫీచర్ ఎక్స్‌ట్రాక్టర్ లోడ్ చేయడం |
| `ort.InferenceSession()` | ఎన్కోడర్ మరియు డికోడర్ కోసం ONNX రన్నింగ్ సెషన్స్ సృష్టించడం |
| `tokenizer.decode()` | అవుట్‌పుట్ టోకెన్ ఐడీలను టెక్స్ట్‌గా మార్చడం |

</details>

<details>
<summary><h3>JavaScript ట్రాక్</h3></summary>

#### సెట్‌అప్

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### ట్రాన్స్క్రిప్షన్ కోడ్

`foundry-local-whisper.mjs` అనే ఫైల్ సృష్టించండి:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// దశ 1: బూట్‌స్ట్రాప్ - మేనేజర్‌ను సృష్టించండి, సేవను ప్రారంభించండి, మరియు మోడల్‌ను లోడ్ చేయండి
console.log(`Initialising Foundry Local with model: ${modelAlias}...`);
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);

if (!model.isCached) {
  console.log("Downloading Whisper model...");
  await model.download();
}
await model.load();

// దశ 2: ఒక ఆడియో క్లయింట్‌ను సృష్టించి ట్రాన్స్‌క్రైబ్ చేయండి
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// పరిశుభ్రం చేయడం
await model.unload();
```

> **గమనిక:** Foundry Local SDK `model.createAudioClient()` ద్వారా బిల్ట్-ఇన్ `AudioClient` అందిస్తుంది, ఇది పూర్తి ONNX ఇన్ఫెరెన్స్ పైప్లైన్‌ను అంతర్గతంగా నిర్వహిస్తుంది — `onnxruntime-node` ను ఇంపోర్ట్ చేయాల్సిన అవసరం లేదు. సరిగ్గా ఇంగ్లీష్ ట్రాన్స్క్రిప్షన్ కోసం ఎప్పుడూ `audioClient.settings.language = "en"` సెట్ చేయండి.

#### దాన్ని నడపండి

```bash
# Zava ఉత్పత్తి పరిస్థితిని లిప్యంతరం చేయండి
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# లేదా ఇతరులను ప్రయత్నించండి:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### JavaScript ముఖ్యమైన అంశాలు

| పద్ధతి | ప్రయోజనం |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | మేనేజర్ సింగిల్టన్ సృష్టించడం |
| `await catalog.getModel(alias)` | క్యాటలాగ్ నుండి మోడల్ పొందడం |
| `model.download()` / `model.load()` | Whisper మోడల్‌ను డౌన్‌లోడ్ చేసి లోడ్ చేయడం |
| `model.createAudioClient()` | ట్రాన్స్క్రిప్షన్ కోసం ఆడియో క్లయింట్ సృష్టించడం |
| `audioClient.settings.language = "en"` | ట్రాన్స్క్రిప్షన్ భాష సెట్ చేయడం (ఖచ్చితమైన అవుట్‌పుట్ కోసం అవసరం) |
| `audioClient.transcribe(path)` | ఆడియో ఫైల్ ట్రాన్స్క్రైబ్ చేయడం, `{ text, duration }` వస్తుంది |

</details>

<details>
<summary><h3>C# ట్రాక్</h3></summary>

#### సెట్‌అప్

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **గమనిక:** C# ట్రాక్ `Microsoft.AI.Foundry.Local` ప్యాకేజీని ఉపయోగించి `model.GetAudioClientAsync()` ద్వారా బిల్ట్-ఇన్ `AudioClient` అందిస్తుంది. ఇది పూర్తి ట్రాన్స్క్రిప్షన్ పైప్లైన్‌ను ప్రాసెస్‌లో సిద్ధంగా నిర్వహిస్తుంది - వేరు ONNX రన్నింగ్ సెషన్ అవసరం లేదు.

#### ట్రాన్స్క్రిప్షన్ కోడ్

`Program.cs` లో కంటెంటును మార్చండి:

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// --- Configuration ---
var modelAlias = "whisper-medium";
var audioFile = args.Length > 0 ? args[0] : "sample.wav";

if (!File.Exists(audioFile))
{
    Console.WriteLine($"Audio file not found: {audioFile}");
    Console.WriteLine("Usage: dotnet run <path-to-audio-file>");
    return;
}

// --- Step 1: Initialize Foundry Local ---
Console.WriteLine("Initializing Foundry Local...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// --- Step 2: Load the Whisper model ---
Console.WriteLine($"Loading model: {modelAlias}...");
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading model...");
    await model.DownloadAsync(null, default);
}

// Load model into memory
Console.WriteLine("Loading model into memory...");
await model.LoadAsync(default);

// --- Step 3: Transcribe audio ---
Console.WriteLine($"Transcribing: {audioFile}");

var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en";

var result = await audioClient.TranscribeAudioAsync(audioFile);

Console.WriteLine("\n--- Transcription ---");
Console.WriteLine(result.Text);
Console.WriteLine("---------------------");
```

#### దాన్ని నడపండి

```bash
# Zava ఉత్పత్తి పరిస్థితిని లిప్యంతరించండి
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# లేదా ఇతరులను ప్రయత్నించండి:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### C# ముఖ్యమైన అంశాలు

| పద్ధతి | ప్రయోజనం |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Localను కాన్ఫిగరేషన్‌తో ప్రారంభించడం |
| `catalog.GetModelAsync(alias)` | క్యాటలాగ్ నుండి మోడల్ పొందడం |
| `model.DownloadAsync()` | Whisper మోడల్ డౌన్‌లోడ్ చేయడం |
| `model.GetAudioClientAsync()` | AudioClient పొందడం (ChatClient కాదు!) |
| `audioClient.Settings.Language = "en"` | ట్రాన్స్క్రిప్షన్ భాష సెట్ చేయడం (ఖచ్చితమైన అవుట్‌పుట్ కోసం అవసరం) |
| `audioClient.TranscribeAudioAsync(path)` | ఆడియో ఫైల్ ట్రాన్స్క్రైబ్ చేయడం |
| `result.Text` | ట్రాన్స్క్రైబ్ అయిన టెక్స్ట్ |


> **C# vs Python/JS:** C# SDK లో బిల్ట్-ఇన్ `AudioClient` ను `model.GetAudioClientAsync()` ద్వారా ప్రాసెస్ లోనే ట్రాన్స్క్రిప్షన్ కోసం అందిస్తుంది, ఇది JavaScript SDK లాగా పనిచేస్తుంది. Python నేరుగా ONNX Runtime ను కాష్డ్ ఎన్కోడర్/డీకోడర్ మోడల్స్ మీద ఇన్ఫెరెన్స్ కోసం ఉపయోగిస్తుంది.

</details>

---

### వ్యాయామం 4 - అన్ని Zava నమూనాలను బ్యాచ్ ట్రాన్స్క్రయిబ్ చేయండి

ఇప్పుడు మీ কাছে పనిచేవి ట్రాన్స్క్రిప్షన్ యాప్ ఉన్నప్పుడు, ఐదు Zava నమూనా ఫైళ్ళన్నీ ట్రాన్స్క్రైబ్ చేసి ఫలితాలను సరిపోల్చండి.

<details>
<summary><h3>Python Track</h3></summary>

పూర్తి నమూనా `python/foundry-local-whisper.py` ఇప్పటికే బ్యాచ్ ట్రాన్స్క్రిప్షన్ ను మద్దతిస్తోంది. ఎలాంటి బాహ్య ఆర్గ్యుమెంట్ల లేకుండా నడిపితే, ఇది `samples/audio/` లో ఉన్న అన్ని `zava-*.wav` ఫైళ్లను ట్రాన్స్క్రైబ్ చేస్తుంది:

```bash
cd python
python foundry-local-whisper.py
```

నమూనా `FoundryLocalManager(alias)` ద్వారా బూట్‌ప్రాప్ చేసుకుని, ప్రతి ఫైలు కి ఎన్కోడర్ మరియు డీకోడర్ ONNX సెషన్లను నడిపిస్తుంది.

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

పూర్తి నమూనా `javascript/foundry-local-whisper.mjs` ఇప్పటికే బ్యాచ్ ట్రాన్స్క్రిప్షన్ ను మద్దతిస్తోంది. ఎలాంటి బాహ్య ఆర్గ్యుమెంట్ల లేకుండా నడిపితే, ఇది `samples/audio/` లో ఉన్న అన్ని `zava-*.wav` ఫైళ్లను ట్రాన్స్క్రైబ్ చేస్తుంది:

```bash
cd javascript
node foundry-local-whisper.mjs
```

నమూనా `FoundryLocalManager.create()` మరియు `catalog.getModel(alias)` ను SDK ప్రారంభానికి ఉపయోగించి, తరువాత ప్రతి ఫైల్ కోసం `AudioClient` (సెట్టింగ్‌లో `language = "en"`) ఉపయోగించి ట్రాన్స్క్రైబ్ చేస్తుంది.

</details>

<details>
<summary><h3>C# Track</h3></summary>

పూర్తి నమూనా `csharp/WhisperTranscription.cs` ఇప్పటికే బ్యాచ్ ట్రాన్స్క్రిప్షన్ ను మద్దతిస్తోంది. ఏ నిర్దిష్ట ఫైల్ ఆర్గ్యుమెంట్ లేకుండా నడిపితే, ఇది `samples/audio/` లో ఉన్న అన్ని `zava-*.wav` ఫైళ్లను ట్రాన్స్క్రైబ్ చేస్తుంది:

```bash
cd csharp
dotnet run whisper
```

నమూనా `FoundryLocalManager.CreateAsync()` మరియు SDK యొక్క `AudioClient` (సెట్టింగ్స్‌లో `Language = "en"`) ను ప్రాసెస్ లోనే ట్రాన్స్క్రిప్షన్ కోసం ఉపయోగిస్తుంది.

</details>

**ఎంతో చూడండి:** ట్రాన్స్క్రిప్షన్ ఔట్‌పుట్‌ను `samples/audio/generate_samples.py` లోని మూలపాఠ్యం తో పోల్చండి. Whisper ఎలా సరిగ్గా "Zava ProGrip" వంటి ఉత్పత్తి పేర్లు మరియు "brushless motor" లేదా "composite decking" వంటి సాంకేతిక పదాలను క్యాప్చర్ చేస్తుంది?

---

### వ్యాయామం 5 - కీలక కోడ్ నమూనాలను అర్థం చేసుకోండి

మూడూ భాషల్లో ప్రతిభాగం చాటుతో పోల్చినప్పుడు Whisper ట్రాన్స్క్రిప్షన్ ఎలా వేరుగా ఉందో అధ్యయనం చేయండి:

<details>
<summary><b>Python - చాటు నుండి కీలక తేడాలు</b></summary>

```python
# చాట్ పూర్తి (భాగాలు 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# ఆడియో ట్రాన్స్క్రిప్షన్ (ఈ భాగం):
# OpenAI క్లయింట్‌కి బదులుగా నేరుగా ONNX రన్‌టైమ్‌ను ఉపయోగిస్తుంది
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... ఆటోరెగ్రెసివ్ డీకోడర్ లూప్ ...
print(tokenizer.decode(generated_tokens))
```

**కీలక అవగాహన:** చాట్ మోడల్స్ `manager.endpoint` ద్వారా OpenAI అనుకూల API ఉపయోగిస్తాయి. Whisper SDKని ఉపయోగించి కాష్డ్ ONNX మోడల్ ఫైళ్లను కనుగొని, నేరుగా ONNX Runtime తో ఇన్ఫెరెన్స్ చేస్తుంది.

</details>

<details>
<summary><b>JavaScript - చాటు నుండి కీలక తేడాలు</b></summary>

```javascript
// చాట్ కంప్లీషన్ (భాగాలు 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// ఆడియో ట్రాన్స్‌క్రిప్షన్ (ఈ భాగం):
// SDK లోని బిల్ట్-ఇన్ ఆడియోక్లయింట్‌ను ఉపయోగిస్తుంది
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // ఉత్తమ ఫలితాల కోసం భాషను ఎల్లప్పుడూ సెట్ చేయండి
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**కీలక అవగాహన:** చాట్ మోడల్స్ `manager.urls[0] + "/v1"` ద్వారా OpenAI అనుకూల API ని ఉపయోగిస్తాయి. Whisper ట్రాన్స్క్రిప్షన్ SDK లోని `AudioClient` (`model.createAudioClient()` ద్వారా పొందబడుతుంది) ను ఉపయోగిస్తుంది. ఆటో-డిటెక్షన్ నుండి తయారయ్యే అశుధ్ధ ఔట్‌పుట్ నివారించడానికి `settings.language` సెట్ చేయాలి.

</details>

<details>
<summary><b>C# - చాటు నుండి కీలక తేడాలు</b></summary>

C# పద్దతి SDKలోని బిల్ట్-ఇన్ `AudioClient` ని ప్రాసెస్ లోనే ట్రాన్స్క్రిప్షన్ కోసం ఉపయోగిస్తుంది:

**మోడల్ ప్రారంభం:**

```csharp
// 1. Create the manager with configuration
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// 2. Get model from catalog, download, and load
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("whisper-medium", default);
await model.DownloadAsync(null, default);
await model.LoadAsync(default);
```

**ట్రాన్స్క్రిప్షన్:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**కీలక అవగాహన:** C# `FoundryLocalManager.CreateAsync()` పిలుస్తుంది మరియు నేరుగా `AudioClient` పొందుతుంది — ONNX Runtime సెటప్ అవసరం లేదు. ఆటో-డిటెక్షన్ లోపం నివారించడానికి `Settings.Language` సెట్ చేయండి.

</details>

> **సారాంశం:** Python మోడల్ నిర్వహణ కోసం Foundry Local SDK మరియు ఎన్కోడర్/డీకోడర్ మోడల్స్ పై నేరుగా ఇన్ఫెరెన్స్ కోసం ONNX Runtime ఉపయోగిస్తుంది. JavaScript మరియు C# రెండూ SDK లోని బిల్ట్-ఇన్ `AudioClient` ను అనుసరించే ట్రాన్స్క్రిప్షన్ కోసం ఉపయోగిస్తాయి — క్లయింట్ సృష్టించండి, భాష సెట్ చేయండి, మరియు `transcribe()` / `TranscribeAudioAsync()` ను పిలవండి. ఖచ్చితమైన ఫలితాల కోసం ఎప్పుడూ AudioClient లో భాష సెట్ చేయండి.

---

### వ్యాయామం 6 - ప్రయోగం

మీ అర్థాన్ని పెంచుకునేందుకు ఈ మార్పులు ప్రయత్నించండి:

1. **వేరే ఆడియో ఫైళ్లను ప్రయత్నించండి** - Windows Voice Recorder వాడి మాట్లాడి రికార్డ్ చేసి WAV గా సేవ్ చేసి ట్రాన్స్క్రైబ్ చేయండి

2. **మోడల్ వేరియంట్లను సరిపోల్చండి** - మీ వద్ద NVIDIA GPU ఉంటే CUDA వేరియంట్ ప్రయత్నించండి:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   CPU వేరియంట్ తో ట్రాన్స్క్రిప్షన్ వేగాన్ని సరిపోల్చండి.

3. **ఔట్‌పుట్ ఫార్మాటింగ్ చేర్చండి** - JSON ప్రతిస్పందనలో వీటిని చేర్చవచ్చు:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **REST API నిర్మించండి** - ట్రాన్స్క్రిప్షన్ కోడ్ ను వెబ్ సర్వర్ లో ప్యాక్డ్ చేయండి:

   | భాష | ఫ్రేమ్‌వర్క్ | ఉదాహరణ |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` తో `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` తో `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` తో `IFormFile` |

5. **మల్టీ-టర్న్ తో ట్రాన్స్క్రిప్షన్** - పార్ట్ 4 లోని చాట్ ఏజెంటుతో Whisper మిళితం చేయండి: ముందుగా ఆడియో ట్రాన్స్క్రైబ్ చేయండి, తరువాత టెక్స్ట్ ను ఏజెంటుకి పంపి విశ్లేషణ లేదా సారాంశం చేయించుకోండి.

---

## SDK ఆడియో API సూచిక

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — ఒక `AudioClient` ఇన్స్టాన్స్ సృష్టించు
> - `audioClient.settings.language` — ట్రాన్స్క్రిప్షన్ భాష ను సెట్ చేయండి (ఉదా: `"en"`)
> - `audioClient.settings.temperature` — రాండమ్నెస్ నియంత్రణ (ఐచ్ఛికం)
> - `audioClient.transcribe(filePath)` — ఫైల్ ను ట్రాన్స్క్రైబ్ చేసి `{ text, duration }` రిటర్న్ చేస్తుంది
> - `audioClient.transcribeStreaming(filePath, callback)` — కాల్బాక్ ద్వారా ట్రాన్స్క్రిప్షన్ భాగాలను స్ట్రీమ్ చేస్తుంది
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — ఒక `OpenAIAudioClient` ఇన్స్టాన్స్ సృష్టిస్తుంది
> - `audioClient.Settings.Language` — ట్రాన్స్క్రిప్షన్ భాష ను సెట్ చేయండి (ఉదా: `"en"`)
> - `audioClient.Settings.Temperature` — రాండమ్నెస్ నియంత్రణ (ఐచ్ఛికం)
> - `await audioClient.TranscribeAudioAsync(filePath)` — ఫైల్ని ట్రాన్స్క్రైబ్ చేసి `.Text` కలిగిన آب్జెక్ట్ ను ఇస్తుంది
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — ట్రాన్స్క్రిప్షన్ భాగాల IAsyncEnumerable ని ఇస్తుంది

> **సూచన:** ఎప్పుడూ ట్రాన్స్క్రైబ్ చేయటానికి ముందు భాష ప్రాపర్టీని సెట్ చేయండి. లేకపోతే, Whisper మోడల్ ఆటో-డిటెక్షన్ చేస్తుంది, ఇది అశుద్ధ ఔట్‌పుట్ (సINGLE ప్రతిస్థాపన చిహ్నం) ఇవ్వవచ్చు.

---

## తులనాత్మక: చాట్ మోడల్స్ vs. Whisper

| అంశం | చాట్ మోడల్స్ (పార్ట్స్ 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **టాస్క్ రకం** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **ఇన్‌పుట్** | టెక్స్ట్ మెసేజ్లు (JSON) | ఆడియో ఫైళ్లు (WAV/MP3/M4A) | ఆడియో ఫైళ్లు (WAV/MP3/M4A) |
| **ఔట్‌పుట్** | జనరేట్ చేయబడిన టెక్స్ట్ (స్ట్రీమ్డ్) | ట్రాన్స్క్రైబ్ టెక్స్ట్ (సంపೂರ್ಣం) | ట్రాన్స్క్రైబ్ టెక్స్ట్ (సంపೂರ್ಣం) |
| **SDK ప్యాకేజ్** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API పద్ధతి** | `client.chat.completions.create()` | ONNX Runtime నేరుగా | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **భాష సెట్** | వర్తించదు | డీకోడర్ ప్రాంప్ట్ టోకెన్లు | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **స్ట్రీమింగ్** | అవును | లేదు | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **గోప్యత ప్రయోజనం** | కోడ్/డేటా స్థానికంగా ఉంటుంది | ఆడియో డేటా స్థానికంగా ఉంటుంది | ఆడియో డేటా స్థానికంగా ఉంటుంది |

---

## కీలక గమనికలు

| సూత్రం | మీరు నేర్చుకున్నది |
|---------|-----------------|
| **Whisper ఆన్-డివైస్** | భాషణం-టు-టెక్స్ట్ పూర్తిగా స్థానికంగా నడుస్తుంది, Zava కస్టమర్ కాల్స్ మరియు ఉత్పత్తి సమీక్షలను ఆన్-డివైస్ ట్రాన్స్క్రైబ్ చేయడానికి బాగుంది |
| **SDK AudioClient** | JavaScript మరియు C# SDKలు బిల్ట్-ఇన్ `AudioClient` ను అందిస్తాయి, ఇది పూర్తి ట్రాన్స్క్రిప్షన్ పైప్‌లైన్ ను ఒక్క పిలుపుతో నిర్వహిస్తుంది |
| **భాష సెట్** | ఎప్పుడూ AudioClient భాష (ఉదా: `"en"`) సెట్ చేయాలి — లేకపోతే ఆటో-డిటెక్షన్ అశుద్ధ ఔట్‌పుట్ ఇస్తుంది |
| **Python** | మోడల్ నిర్వహణకి `foundry-local-sdk`, నేరుగా ONNX ఇన్ఫెరెన్స్ కోసం `onnxruntime` + `transformers` + `librosa` ఉపయోగిస్తుంది |
| **JavaScript** | `foundry-local-sdk` లో `model.createAudioClient()` వాడి — `settings.language` సెట్ చేసి, తరువాత `transcribe()` పిలవండి |
| **C#** | `Microsoft.AI.Foundry.Local` లో `model.GetAudioClientAsync()` వాడి — `Settings.Language` సెట్ చేసి, తరువాత `TranscribeAudioAsync()` పిలవండి |
| **స్ట్రీమింగ్ మద్దతు** | JS మరియు C# SDKలు కూడా భాగాల వారీగా ఔట్‌పుట్ కోసం `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` అందిస్తాయి |
| **CPU-ఆప్టిమైజ్డ్** | CPU వేరియంట్ (3.05 GB) GPU లేని Windows పరికరాలలో కూడా పనిచేస్తుంది |
| **గోప్యత-మొదట** | Zava కస్టమ్ పరస్పర చర్యలు మరియు గోప్య ఉత్పత్తి డేటా స్థానికంగా ఉంచడానికి అనుకూలం |

---

## వనరులు

| వనరు | లింక్ |
|----------|------|
| Foundry Local డాక్స్ | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK సూచిక | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper మోడల్ | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local వెబ్‌సైట్ | [foundrylocal.ai](https://foundrylocal.ai) |

---

## తదుపరి దశ

మీరు స్వయంగా మోడల్స్ సేకరించి Foundry Local ద్వారా నడిపేందుకు [పార్ట్ 10: కస్టమ్ లేదా Hugging Face మోడల్స్ ఉపయోగించడం](part10-custom-models.md) కి కొనసాగండి.