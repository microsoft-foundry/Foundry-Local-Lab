![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग 9: व्हिस्पर और Foundry Local के साथ वॉइस ट्रांसक्रिप्शन

> **लक्ष्य:** Foundry Local के माध्यम से स्थानीय रूप से चल रहे OpenAI Whisper मॉडल का उपयोग करके ऑडियो फ़ाइलों का ट्रांसक्रिप्शन करें - पूरी तरह से डिवाइस पर, किसी क्लाउड की आवश्यकता नहीं।

## अवलोकन

Foundry Local केवल टेक्स्ट जनरेशन के लिए नहीं है; यह **स्पीच-टू-टेक्स्ट** मॉडलों को भी समर्थन करता है। इस लैब में आप पूरी तरह से अपने कंप्यूटर पर ऑडियो फ़ाइलों का ट्रांसक्रिप्शन करने के लिए **OpenAI Whisper Medium** मॉडल का उपयोग करेंगे। यह उन परिदृश्यों के लिए आदर्श है जैसे ज़ावा कस्टमर सर्विस कॉल्स, प्रोडक्ट रिव्यू रिकॉर्डिंग्स, या वर्कशॉप प्लानिंग सेशन जहाँ ऑडियो डेटा कभी भी आपके डिवाइस से बाहर नहीं जाना चाहिए।

---

## सीखने के उद्देश्य

इस लैब के अंत तक आप सक्षम होंगे:

- Whisper स्पीच-टू-टेक्स्ट मॉडल और इसकी क्षमताओं को समझना
- Foundry Local का उपयोग करके Whisper मॉडल डाउनलोड और चलाना
- Foundry Local SDK में Python, JavaScript, और C# का उपयोग करके ऑडियो फ़ाइलों का ट्रांसक्रिप्शन करना
- एक सरल ट्रांसक्रिप्शन सेवा बनाना जो पूरी तरह डिवाइस पर चले
- Foundry Local में चैट/टेक्स्ट मॉडल और ऑडियो मॉडल के बीच अंतर को समझना

---

## पूर्वापेक्षाएँ

| आवश्यकता | विवरण |
|-------------|---------|
| **Foundry Local CLI** | संस्करण **0.8.101 या उससे ऊपर** (Whisper मॉडल v0.8.101 से उपलब्ध हैं) |
| **OS** | Windows 10/11 (x64 या ARM64) |
| **भाषा रनटाइम** | **Python 3.9+** और/या **Node.js 18+** और/या **.NET 9 SDK** ([.NET डाउनलोड करें](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **पूरा किया हुआ** | [भाग 1: शुरूआत](part1-getting-started.md), [भाग 2: Foundry Local SDK डीप डायव](part2-foundry-local-sdk.md), और [भाग 3: SDKs और APIs](part3-sdk-and-apis.md) |

> **नोट:** Whisper मॉडल केवल **SDK** के माध्यम से डाउनलोड किए जा सकते हैं (CLI नहीं)। CLI ऑडियो ट्रांसक्रिप्शन एंडपॉइंट का समर्थन नहीं करता। अपनी संस्करण जांचने के लिए:
> ```bash
> foundry --version
> ```

---

## अवधारणा: Whisper कैसे Foundry Local के साथ काम करता है

OpenAI Whisper मॉडल एक सामान्य प्रयोजन स्पीच रिकग्निशन मॉडल है जिसे विविध ऑडियो के बड़े डेटासेट पर प्रशिक्षित किया गया है। जब इसे Foundry Local के माध्यम से चलाया जाता है:

- मॉडल **पूरी तरह से आपके CPU पर चलता है** - GPU की आवश्यकता नहीं
- ऑडियो कभी आपके डिवाइस से बाहर नहीं जाता - **पूर्ण गोपनीयता**
- Foundry Local SDK मॉडल डाउनलोड और कैश प्रबंधन संभालता है
- **JavaScript और C#** SDK में एक बिल्ट-इन `AudioClient` प्रदान करते हैं जो पूरी ट्रांसक्रिप्शन पाइपलाइन को संभालता है — किसी मैनुअल ONNX सेटअप की आवश्यकता नहीं
- **Python** मॉडल प्रबंधन के लिए SDK और ONNX Runtime का उपयोग करता है जो एन्कोडर/डिकोडर ONNX मॉडलों पर सीधे अनुमान लगाता है

### पाइपलाइन कैसे काम करती है (JavaScript और C#) — SDK AudioClient

1. **Foundry Local SDK** Whisper मॉडल डाउनलोड और कैश करता है
2. `model.createAudioClient()` (JS) या `model.GetAudioClientAsync()` (C#) एक `AudioClient` बनाता है
3. `audioClient.transcribe(path)` (JS) या `audioClient.TranscribeAudioAsync(path)` (C#) पूरी पाइपलाइन अंदर ही नियंत्रित करता है — ऑडियो प्रीप्रोसेसिंग, एन्कोडर, डिकोडर, और टोकन डिकोडिंग
4. `AudioClient` एक `settings.language` प्रॉपर्टी प्रदान करता है (सटीक ट्रांसक्रिप्शन के लिए `"en"` हिंदी में सेट करें)

### पाइपलाइन कैसे काम करती है (Python) — ONNX Runtime

1. **Foundry Local SDK** Whisper ONNX मॉडल फ़ाइलें डाउनलोड एवं कैश करता है
2. **ऑडियो प्रीप्रोसेसिंग** WAV ऑडियो को मेल स्पेक्ट्रोग्राम (80 मेल बिन x 3000 फ्रेम) में बदलता है
3. **एन्कोडर** मेल स्पेक्ट्रोग्राम को संसाधित करके हिडन स्टेट्स और क्रॉस-अटेंशन की/वैल्यू टेन्सर्स बनाता है
4. **डिकोडर** ऑटोरिग्रैसिवली चलता है, एक-एक टोकन जनरेट करता है जब तक यह एंड-ऑफ़-टेक्स्ट टोकन नहीं देता
5. **टोकनाइजर** आउटपुट टोकन IDs को पढ़ने योग्य टेक्स्ट में डिकोड करता है

### Whisper मॉडल वेरिएंट्स

| उपनाम | मॉडल ID | डिवाइस | आकार | विवरण |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-त्वरित (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU-उन्नत (अधिकांश उपकरणों के लिए अनुशंसित) |

> **नोट:** चैट मॉडल की तरह डिफ़ॉल्ट सूचीबद्ध न होकर, Whisper मॉडल `automatic-speech-recognition` टास्क के अंतर्गत वर्गीकृत हैं। विवरण देखने के लिए `foundry model info whisper-medium` का उपयोग करें।

---

## लैब अभ्यास

### अभ्यास 0 - सैंपल ऑडियो फ़ाइलें प्राप्त करें

यह लैब ज़ावा DIY उत्पाद परिदृश्यों पर आधारित प्री-बिल्ट WAV फ़ाइलें शामिल करती है। इन्हें निम्न स्क्रिप्ट से बनाएं:

```bash
# रिपॉज़िटरी रूट से - सबसे पहले एक .venv बनाएं और सक्रिय करें
python -m venv .venv

# विंडोज़ (पावरशेल):
.venv\Scripts\Activate.ps1
# मैकओएस:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

यह `samples/audio/` में छह WAV फाइलें बनाता है:

| फ़ाइल | परिदृश्य |
|------|----------|
| `zava-customer-inquiry.wav` | ग्राहक द्वारा **Zava ProGrip Cordless Drill** के बारे में पूछताछ |
| `zava-product-review.wav` | ग्राहक द्वारा **Zava UltraSmooth Interior Paint** की समीक्षा |
| `zava-support-call.wav` | समर्थन कॉल **Zava TitanLock Tool Chest** के बारे में |
| `zava-project-planning.wav` | DIYर द्वारा **Zava EcoBoard Composite Decking** के साथ डेक की योजना |
| `zava-workshop-setup.wav` | **सभी पांच Zava उत्पादों** का उपयोग करते हुए कार्यशाला का वॉकथ्रू |
| `zava-full-project-walkthrough.wav` | **सभी Zava उत्पादों** के साथ विस्तृत गैराज नवीकरण वॉकथ्रू (~4 मिनट, लंबी ऑडियो जांच के लिए) |

> **टिप:** आप अपने खुद के WAV/MP3/M4A फाइलें भी उपयोग कर सकते हैं, या Windows Voice Recorder से रिकॉर्ड कर सकते हैं।

---

### अभ्यास 1 - SDK का उपयोग करके Whisper मॉडल डाउनलोड करें

Whisper मॉडलों के साथ नई Foundry Local संस्करणों में CLI असंगतता के कारण, मॉडल डाउनलोड और लोड करने के लिए **SDK** का उपयोग करें। अपनी भाषा चुनें:

<details>
<summary><b>🐍 Python</b></summary>

**SDK इंस्टॉल करें:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# सेवा शुरू करें
manager = FoundryLocalManager()
manager.start_service()

# सूची जानकारी जांचें
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# जांचें कि क्या पहले से कैश किया गया है
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# मॉडल को मेमोरी में लोड करें
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py` के रूप में सेव करें और चलाएं:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK इंस्टॉल करें:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// प्रबंधक बनाएं और सेवा शुरू करें
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// सूची से मॉडल प्राप्त करें
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

// मॉडल को मेमोरी में लोड करें
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs` के रूप में सेव करें और चलाएं:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK इंस्टॉल करें:**
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

> **क्यों SDK CLI के बजाय?** Foundry Local CLI Whisper मॉडल को सीधे डाउनलोड या सर्व करने का समर्थन नहीं करता। SDK एक भरोसेमंद तरीका है प्रोग्रामेटिक रूप से ऑडियो मॉडल डाउनलोड और प्रबंधित करने का। JavaScript और C# SDK में बिल्ट-इन `AudioClient` है जो पूरी ट्रांसक्रिप्शन पाइपलाइन को संभालता है। Python ONNX Runtime का उपयोग करता है जो मॉडल फ़ाइलों के प्रति सीधे अनुमान लगाता है।

---

### अभ्यास 2 - Whisper SDK को समझें

Whisper ट्रांसक्रिप्शन भाषा के आधार पर अलग-अलग दृष्टिकोणों का उपयोग करता है। **JavaScript और C#** में Foundry Local SDK का बिल्ट-इन `AudioClient` होता है जो पूरी पाइपलाइन (ऑडियो प्रीप्रोसेसिंग, एन्कोडर, डिकोडर, टोकन डिकोडिंग) एक ही विधि कॉल में संभालता है। **Python** मॉडल प्रबंधन के लिए Foundry Local SDK और सीधे एन्कोडर/डिकोडर ONNX मॉडलों के लिए ONNX Runtime का उपयोग करता है।

| घटक | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK पैकेज** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **मॉडल प्रबंधन** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **विशेषता निष्कर्षण** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` द्वारा हैंडल किया गया | SDK `AudioClient` द्वारा हैंडल किया गया |
| **इन्फरेंस** | `ort.InferenceSession` (एन्कोडर + डिकोडर) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **टोकन डिकोडिंग** | `WhisperTokenizer` | SDK `AudioClient` द्वारा हैंडल किया गया | SDK `AudioClient` द्वारा हैंडल किया गया |
| **भाषा सेटिंग** | डिकोडर टोकन में `forced_ids` द्वारा सेट | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **इनपुट** | WAV फ़ाइल पथ | WAV फ़ाइल पथ | WAV फ़ाइल पथ |
| **आउटपुट** | डिकोडेड टेक्स्ट स्ट्रिंग | `result.text` | `result.Text` |

> **महत्वपूर्ण:** हमेशा `AudioClient` पर भाषा सेट करें (जैसे अंग्रेज़ी के लिए `"en"`)। बिना स्पष्ट भाषा सेटिंग के, मॉडल गलत आउटपुट दे सकता है क्योंकि यह भाषा का ऑटो-डिटेक्शन करता है।

> **SDK पैटर्न:** Python `FoundryLocalManager(alias)` का उपयोग करता है, फिर ONNX मॉडल फ़ाइलों के लिए `get_cache_location()` का उपयोग करता है। JavaScript और C# SDK के बिल्ट-इन `AudioClient` का उपयोग करते हैं — इसे `model.createAudioClient()` (JS) या `model.GetAudioClientAsync()` (C#) के माध्यम से प्राप्त किया जाता है — जो पूरी ट्रांसक्रिप्शन पाइपलाइन को संभालता है। पूरी जानकारी के लिए देखें [भाग 2: Foundry Local SDK डीप डायव](part2-foundry-local-sdk.md)।

---

### अभ्यास 3 - एक सरल ट्रांसक्रिप्शन ऐप बनाएं

अपनी पसंदीदा भाषा ट्रैक चुनें और एक न्यूनतम एप्लिकेशन बनाएं जो ऑडियो फ़ाइल का ट्रांसक्रिप्शन करे।

> **समर्थित ऑडियो फॉर्मेट:** WAV, MP3, M4A। सर्वोत्तम परिणामों के लिए 16kHz सैंपल रेट वाली WAV फाइलों का उपयोग करें।

<details>
<summary><h3>Python ट्रैक</h3></summary>

#### सेटअप

```bash
cd python
python -m venv venv

# वर्चुअल वातावरण को सक्रिय करें:
# विंडोज़ (पावरशेल):
venv\Scripts\Activate.ps1
# मैकओएस:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### ट्रांसक्रिप्शन कोड

`foundry-local-whisper.py` नामक फ़ाइल बनाएं:

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

# चरण 1: बूटस्ट्रैप - सेवा शुरू करता है, डाउनलोड करता है, और मॉडल लोड करता है
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# कैश किए गए ONNX मॉडल फ़ाइलों का पथ बनाएं
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# चरण 2: ONNX सत्र और फीचर एक्सट्रैक्टर लोड करें
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

# चरण 3: मेल स्पेक्ट्रोग्राम फीचर्स निकालें
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# चरण 4: एन्कोडर चलाएं
enc_out = encoder.run(None, {"audio_features": input_features})
# पहला आउटपुट छिपे हुए राज्यों का होता है; बाकी क्रॉस-अटेंशन KV जोड़े होते हैं
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# चरण 5: ऑटोरेग्रेसीव डिकोडिंग
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, ट्रांसक्राइब, नो टाइमस्टैम्प्स
input_ids = np.array([initial_tokens], dtype=np.int32)

# खाली सेल्फ-अटेंशन KV कैश
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

    if next_token == 50257:  # टेक्स्ट का अंत
        break
    generated.append(next_token)

    # सेल्फ-अटेंशन KV कैश अपडेट करें
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### इसे चलाएं

```bash
# एक ज़ावा उत्पाद परिदृश्य ट्रांसक्राइब करें
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# या अन्य प्रयास करें:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### प्रमुख Python बिंदु

| विधि | उद्देश्य |
|--------|---------|
| `FoundryLocalManager(alias)` | बूटस्ट्रैप: सेवा शुरू करें, मॉडल डाउनलोड और लोड करें |
| `manager.get_cache_location()` | कैश किए गए ONNX मॉडल फ़ाइलों का पथ प्राप्त करें |
| `WhisperFeatureExtractor.from_pretrained()` | मेल स्पेक्ट्रोग्राम फीचर एक्स्ट्रेक्टर लोड करें |
| `ort.InferenceSession()` | एन्कोडर और डिकोडर के लिए ONNX रनटाइम सत्र बनाएं |
| `tokenizer.decode()` | आउटपुट टोकन IDs को वापस टेक्स्ट में कनवर्ट करें |

</details>

<details>
<summary><h3>JavaScript ट्रैक</h3></summary>

#### सेटअप

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### ट्रांसक्रिप्शन कोड

`foundry-local-whisper.mjs` नामक फ़ाइल बनाएं:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// चरण 1: बूटस्ट्रैप - मैनेजर बनाएं, सेवा प्रारंभ करें, और मॉडल लोड करें
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

// चरण 2: एक ऑडियो क्लाइंट बनाएं और ट्रांसक्राइब करें
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// सफाई करें
await model.unload();
```

> **नोट:** Foundry Local SDK मॉडल के माध्यम से एक बिल्ट-इन `AudioClient` प्रदान करता है (`model.createAudioClient()`) जो पूरी ONNX इन्फरेंस पाइपलाइन को अंदर ही संभालता है — `onnxruntime-node` आयात की आवश्यकता नहीं। हमेशा `audioClient.settings.language = "en"` सेट करें ताकि अंग्रेज़ी ट्रांसक्रिप्शन सटीक हो।

#### इसे चलाएं

```bash
# एक ज़ावा उत्पाद परिदृश्य ट्रांसक्राइब करें
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# या अन्य प्रयास करें:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### प्रमुख JavaScript बिंदु

| विधि | उद्देश्य |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | मैनेजर सिंगलटन बनाएं |
| `await catalog.getModel(alias)` | कैटलॉग से मॉडल प्राप्त करें |
| `model.download()` / `model.load()` | Whisper मॉडल डाउनलोड और लोड करें |
| `model.createAudioClient()` | ट्रांसक्रिप्शन के लिए ऑडियो क्लाइंट बनाएं |
| `audioClient.settings.language = "en"` | ट्रांसक्रिप्शन भाषा सेट करें (सटीक आउटपुट के लिए आवश्यक) |
| `audioClient.transcribe(path)` | ऑडियो फ़ाइल का ट्रांसक्राइब करें, `{ text, duration }` लौटाता है |

</details>

<details>
<summary><h3>C# ट्रैक</h3></summary>

#### सेटअप

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **नोट:** C# ट्रैक में `Microsoft.AI.Foundry.Local` पैकेज उपयोग किया गया है जो `model.GetAudioClientAsync()` के माध्यम से बिल्ट-इन `AudioClient` प्रदान करता है। यह पूरी ट्रांसक्रिप्शन पाइपलाइन इन-प्रोसेस संभालता है — अलग ONNX Runtime सेटअप की जरूरत नहीं।

#### ट्रांसक्रिप्शन कोड

`Program.cs` की सामग्री को बदलें:

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

#### इसे चलाएं

```bash
# एक ज़ावा उत्पाद परिदृश्य को ट्रांसक्राइब करें
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# या अन्य प्रयास करें:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### प्रमुख C# बिंदु

| विधि | उद्देश्य |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | कॉन्फ़िगरेशन के साथ Foundry Local इनिशियलाइज़ करें |
| `catalog.GetModelAsync(alias)` | कैटलॉग से मॉडल प्राप्त करें |
| `model.DownloadAsync()` | Whisper मॉडल डाउनलोड करें |
| `model.GetAudioClientAsync()` | AudioClient प्राप्त करें (ChatClient नहीं!) |
| `audioClient.Settings.Language = "en"` | ट्रांसक्रिप्शन भाषा सेट करें (सटीक परिणाम के लिए आवश्यक) |
| `audioClient.TranscribeAudioAsync(path)` | ऑडियो फ़ाइल का ट्रांसक्रिप्शन करें |
| `result.Text` | ट्रांसक्राइब किया हुआ टेक्स्ट |
> **C# बनाम Python/JS:** C# SDK एक बिल्ट-इन `AudioClient` प्रदान करता है जो JavaScript SDK के समान `model.GetAudioClientAsync()` के माध्यम से इन-प्रोसेस ट्रांसक्रिप्शन के लिए होता है। Python सीधे ONNX Runtime का उपयोग करता है जो कैश किए गए एन्कोडर/डिकोडर मॉडलों के खिलाफ इन्फेरेंस करता है।

</details>

---

### अभ्यास 4 - सभी Zava नमूना फ़ाइलों का बैच ट्रांसक्राइब करें

अब जब आपके पास एक कार्यशील ट्रांसक्रिप्शन ऐप है, तो सभी पांच Zava नमूना फ़ाइलों का ट्रांसक्राइब करें और परिणामों की तुलना करें।

<details>
<summary><h3>पायथन ट्रैक</h3></summary>

पूर्ण नमूना `python/foundry-local-whisper.py` पहले से ही बैच ट्रांसक्रिप्शन का समर्थन करता है। बिना तर्कों के चलाने पर, यह `samples/audio/` में सभी `zava-*.wav` फाइलों का ट्रांसक्राइब करता है:

```bash
cd python
python foundry-local-whisper.py
```

नमूना `FoundryLocalManager(alias)` का उपयोग बूटस्ट्रैप करने के लिए करता है, इसके बाद प्रत्येक फाइल के लिए एन्कोडर और डिकोडर ONNX सत्र चलाता है।

</details>

<details>
<summary><h3>जावास्क्रिप्ट ट्रैक</h3></summary>

पूर्ण नमूना `javascript/foundry-local-whisper.mjs` पहले से ही बैच ट्रांसक्रिप्शन का समर्थन करता है। बिना तर्कों के चलाने पर, यह `samples/audio/` में सभी `zava-*.wav` फाइलों का ट्रांसक्राइब करता है:

```bash
cd javascript
node foundry-local-whisper.mjs
```

नमूना `FoundryLocalManager.create()` और `catalog.getModel(alias)` का उपयोग SDK को आरंभ करने के लिए करता है, फिर `AudioClient` (जिसमें `settings.language = "en"` है) का उपयोग प्रत्येक फाइल का ट्रांसक्राइब करने के लिए करता है।

</details>

<details>
<summary><h3>C# ट्रैक</h3></summary>

पूर्ण नमूना `csharp/WhisperTranscription.cs` पहले से ही बैच ट्रांसक्रिप्शन का समर्थन करता है। बिना किसी विशिष्ट फ़ाइल तर्क के चलाने पर, यह `samples/audio/` में सभी `zava-*.wav` फाइलों का ट्रांसक्राइब करता है:

```bash
cd csharp
dotnet run whisper
```

नमूना `FoundryLocalManager.CreateAsync()` और SDK के `AudioClient` (जिसमें `Settings.Language = "en"` है) का उपयोग इन-प्रोसेस ट्रांसक्रिप्शन के लिए करता है।

</details>

**ध्यान देने योग्य बात:** `samples/audio/generate_samples.py` में मौलिक पाठ के विरुद्ध ट्रांसक्रिप्शन आउटपुट की तुलना करें। Whisper "Zava ProGrip" जैसे उत्पाद नामों और "brushless motor" या "composite decking" जैसे तकनीकी शब्दों को कितनी सटीकता से पकड़ता है?

---

### अभ्यास 5 - प्रमुख कोड पैटर्न समझें

तीनों भाषाओं में Whisper ट्रांसक्रिप्शन चैट पूर्णता से कैसे भिन्न है, इसे अध्ययन करें:

<details>
<summary><b>Python - चैट से प्रमुख अंतर</b></summary>

```python
# चैट पूर्णता (भाग 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# ऑडियो ट्रांसक्रिप्शन (यह भाग):
# OpenAI क्लाइंट के बजाय सीधे ONNX रनटाइम का उपयोग करता है
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ...autoregressive डिकोडर लूप...
print(tokenizer.decode(generated_tokens))
```

**मुख्य अंतर्दृष्टि:** चैट मॉडल OpenAI-संगत API का उपयोग `manager.endpoint` के माध्यम से करते हैं। Whisper SDK का उपयोग कर कैश किए गए ONNX मॉडल फ़ाइलों का पता लगाता है, फिर ONNX Runtime के साथ सीधे इन्फेरेंस करता है।

</details>

<details>
<summary><b>JavaScript - चैट से प्रमुख अंतर</b></summary>

```javascript
// चैट पूर्णता (भाग 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// ऑडियो ट्रांसक्रिप्शन (यह भाग):
// SDK के इन-बिल्ट AudioClient का उपयोग करता है
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // सर्वोत्तम परिणामों के लिए हमेशा भाषा सेट करें
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**मुख्य अंतर्दृष्टि:** चैट मॉडल OpenAI-संगत API का उपयोग `manager.urls[0] + "/v1"` के माध्यम से करते हैं। Whisper ट्रांसक्रिप्शन SDK के `AudioClient` का उपयोग करता है, जिसे `model.createAudioClient()` से प्राप्त किया जाता है। गड़बड़ आउटपुट से बचने के लिए `settings.language` सेट करें।

</details>

<details>
<summary><b>C# - चैट से प्रमुख अंतर</b></summary>

C# दृष्टिकोण SDK के बिल्ट-इन `AudioClient` का उपयोग करता है इन-प्रोसेस ट्रांसक्रिप्शन के लिए:

**मॉडल इनिशियलाइज़ेशन:**

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

**ट्रांसक्रिप्शन:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**मुख्य अंतर्दृष्टि:** C# `FoundryLocalManager.CreateAsync()` का उपयोग करता है और सीधे `AudioClient` प्राप्त करता है — ONNX Runtime सेटअप की जरूरत नहीं। गड़बड़ आउटपुट से बचने के लिए `Settings.Language` सेट करें।

</details>

> **सारांश:** Python मॉडल प्रबंधन के लिए Foundry Local SDK और एन्कोडर/डिकोडर मॉडलों के खिलाफ सीधे इन्फ़ेरेंस के लिए ONNX Runtime का उपयोग करता है। JavaScript और C# दोनों SDK के बिल्ट-इन `AudioClient` का उपयोग करते हैं जो एक ही कॉल में पूर्ण ट्रांसक्रिप्शन पाइपलाइन को संभालता है — क्लाइंट बनाएं, भाषा सेट करें, और `transcribe()` / `TranscribeAudioAsync()` कॉल करें। सटीक परिणामों के लिए हमेशा AudioClient पर भाषा प्रॉपर्टी सेट करें।

---

### अभ्यास 6 - प्रयोग करें

अपनी समझ को गहरा करने के लिए इन संशोधनों को आजमाएं:

1. **विभिन्न ऑडियो फ़ाइलें आज़माएं** - Windows Voice Recorder का उपयोग करके खुद की आवाज़ रिकॉर्ड करें, WAV में सहेजें, और उसे ट्रांसक्राइब करें

2. **मॉडल संस्करणों की तुलना करें** - यदि आपके पास NVIDIA GPU है, तो CUDA संस्करण आज़माएं:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   ट्रांसक्रिप्शन गति की तुलना CPU संस्करण से करें।

3. **आउटपुट फॉर्मेटिंग जोड़ें** - JSON प्रतिक्रिया में शामिल हो सकता है:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **REST API बनाएं** - अपने ट्रांसक्रिप्शन कोड को वेब सर्वर में लपेटें:

   | भाषा | फ्रेमवर्क | उदाहरण |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` के साथ `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` के साथ `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` के साथ `IFormFile` |

5. **मल्टी-टर्न ट्रांसक्रिप्शन** - पार्ट 4 से किसी चैट एजेंट के साथ Whisper को संयोजित करें: पहले ऑडियो ट्रांसक्राइब करें, फिर टेक्स्ट को विश्लेषण या सारांश के लिए एजेंट को भेजें।

---

## SDK ऑडियो API संदर्भ

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — एक `AudioClient` उदाहरण बनाता है
> - `audioClient.settings.language` — ट्रांसक्रिप्शन भाषा सेट करें (जैसे `"en"`)
> - `audioClient.settings.temperature` — यादृच्छिकता नियंत्रित करें (वैकल्पिक)
> - `audioClient.transcribe(filePath)` — फ़ाइल ट्रांसक्राइब करता है, `{ text, duration }` लौटाता है
> - `audioClient.transcribeStreaming(filePath, callback)` — कॉलबैक के माध्यम से ट्रांसक्रिप्शन टुकड़े स्ट्रीम करता है
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — एक `OpenAIAudioClient` उदाहरण बनाता है
> - `audioClient.Settings.Language` — ट्रांसक्रिप्शन भाषा सेट करें (जैसे `"en"`)
> - `audioClient.Settings.Temperature` — यादृच्छिकता नियंत्रित करें (वैकल्पिक)
> - `await audioClient.TranscribeAudioAsync(filePath)` — फ़ाइल ट्रांसक्राइब करता है, `.Text` वाली वस्तु लौटाता है
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — ट्रांसक्रिप्शन टुकड़ों का `IAsyncEnumerable` लौटाता है

> **टिप:** ट्रांसक्राइब करने से पहले हमेशा भाषा प्रॉपर्टी सेट करें। इसके बिना, Whisper मॉडल ऑटो-डिटेक्शन करने का प्रयास करता है, जो गड़बड़ आउटपुट (टेक्स्ट के बजाय एकल रिप्लेसमेंट कैरेक्टर) उत्पन्न कर सकता है।

---

## तुलना: चैट मॉडल बनाम Whisper

| पहलू | चैट मॉडल (भाग 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **कार्य प्रकार** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **इनपुट** | टेक्स्ट संदेश (JSON) | ऑडियो फ़ाइलें (WAV/MP3/M4A) | ऑडियो फ़ाइलें (WAV/MP3/M4A) |
| **आउटपुट** | उत्पन्न टेक्स्ट (स्ट्रीम्ड) | ट्रांसक्राइब किया हुआ टेक्स्ट (पूर्ण) | ट्रांसक्राइब किया हुआ टेक्स्ट (पूर्ण) |
| **SDK पैकेज** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API विधि** | `client.chat.completions.create()` | ONNX Runtime डायरेक्ट | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **भाषा सेटिंग** | लागू नहीं | डिकोडर प्रॉम्प्ट टोकन | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **स्ट्रीमिंग** | हाँ | नहीं | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **गोपनीयता लाभ** | कोड/डेटा स्थानीय रहता है | ऑडियो डेटा स्थानीय रहता है | ऑडियो डेटा स्थानीय रहता है |

---

## प्रमुख निष्कर्ष

| अवधारणा | आपने क्या सीखा |
|---------|-----------------|
| **डिवाइस पर Whisper** | स्पीच-टू-टेक्स्ट पूरी तरह स्थानीय रूप से चलता है, जो Zava ग्राहक कॉल और उत्पाद समीक्षाओं का डिवाइस पर ट्रांसक्रिप्शन के लिए आदर्श है |
| **SDK AudioClient** | JavaScript और C# SDK बिल्ट-इन `AudioClient` प्रदान करते हैं जो एक कॉल में पूरी ट्रांसक्रिप्शन पाइपलाइन संभालता है |
| **भाषा सेटिंग** | हमेशा AudioClient की भाषा सेट करें (जैसे `"en"`) — इसके बिना, ऑटो-डिटेक्शन गड़बड़ आउटपुट दे सकता है |
| **Python** | मॉडल प्रबंधन के लिए `foundry-local-sdk` + सीधे ONNX इन्फेरेंस के लिए `onnxruntime` + `transformers` + `librosa` का उपयोग करता है |
| **JavaScript** | `foundry-local-sdk` का उपयोग करता है और `model.createAudioClient()` — `settings.language` सेट करें, फिर `transcribe()` कॉल करें |
| **C#** | `Microsoft.AI.Foundry.Local` का उपयोग करता है और `model.GetAudioClientAsync()` — `Settings.Language` सेट करें, फिर `TranscribeAudioAsync()` कॉल करें |
| **स्ट्रीमिंग सपोर्ट** | JS और C# SDKs `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` भी ऑफर करते हैं जो टुकड़ों में आउटपुट प्रदान करते हैं |
| **CPU-ऑप्टिमाइज़्ड** | CPU संस्करण (3.05 GB) किसी भी Windows डिवाइस पर GPU के बिना चलता है |
| **गोपनीयता प्राथमिकता** | Zava ग्राहक इंटरैक्शन और स्वामित्व वाले उत्पाद डेटा को डिवाइस पर ही रखने के लिए बिल्कुल उपयुक्त |

---

## संसाधन

| संसाधन | लिंक |
|----------|------|
| Foundry Local दस्तावेज़ | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK संदर्भ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper मॉडल | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local वेबसाइट | [foundrylocal.ai](https://foundrylocal.ai) |

---

## अगला कदम

आगे बढ़ें [भाग 10: कस्टम या Hugging Face मॉडल का उपयोग](part10-custom-models.md) से, जहाँ आप Hugging Face से अपने स्वयं के मॉडल संकलित कर सकते हैं और उन्हें Foundry Local के माध्यम से चला सकते हैं।