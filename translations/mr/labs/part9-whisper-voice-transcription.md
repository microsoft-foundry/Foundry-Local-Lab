![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग 9: Whisper आणि Foundry Local वापरून व्हॉइस ट्रान्सक्रिप्शन

> **लक्ष्य:** Foundry Local द्वारे स्थानिकपणे चालणारे OpenAI Whisper मॉडेल वापरून ऑडिओ फाइल्सचे ट्रान्सक्रिप्शन करणे - पूर्णपणे डिव्हाइसवर, कुठलीही क्लाऊड गरज नाही.

## आढावा

Foundry Local फक्त टेक्स्ट जनरेशनसाठी नाही तर तो **स्पीच-टू-टेक्स्ट** मॉडेल्सना देखील समर्थन करतो. या लॅबमध्ये तुम्ही पूर्णपणे आपल्या मशीनवर OpenAI Whisper Medium मॉडेल वापरून ऑडिओ फाइल्सचे ट्रान्सक्रिप्शन कराल. हे अशा परिस्थितीसाठी उपयुक्त आहे जसे की Zava ग्राहक सेवा कॉल्सचे ट्रान्सक्रिप्शन, उत्पादन पुनरावलोकन रेकॉर्डिंग्ज, किंवा वर्कशॉप नियोजन सत्रे जिथे ऑडिओ डेटा आपल्या डिव्हाइसवरून बाहेर जाण्याची गरज नाही.

---

## शिकण्याची उद्दिष्टे

या लॅबचा शेवटी तुम्ही खालील गोष्टी करू शकाल:

- Whisper स्पीच-टू-टेक्स्ट मॉडेल आणि त्याच्या क्षमता समजून घेणे
- Foundry Local वापरून Whisper मॉडेल डाउनलोड करणे आणि चालवणे
- Foundry Local SDK वापरून Python, JavaScript, आणि C# मध्ये ऑडिओ फाइल्स ट्रान्सक्राईब करणे
- पूर्णपणे डिव्हाइसवर चालणारी साधी ट्रान्सक्रिप्शन सेवा तयार करणे
- Foundry Local मध्ये चॅट/टेक्स्ट मॉडेल्स आणि ऑडिओ मॉडेल्स यामधील फरक समजून घेणे

---

## आवश्यकताः

| आवश्यकता | तपशील |
|-------------|---------|
| **Foundry Local CLI** | आवृत्ती **0.8.101 किंवा त्यापुढील** (Whisper मॉडेल्स आवृत्ती v0.8.101 पासून उपलब्ध आहेत) |
| **ऑपरेटिंग सिस्टम** | Windows 10/11 (x64 किंवा ARM64) |
| **प्रोग्रामिंग भाषा रनटाइम** | **Python 3.9+** आणि/किंवा **Node.js 18+** आणि/किंवा **.NET 9 SDK** ([डाउनलोड .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **संपूर्ण केले आहे** | [भाग 1: Getting Started](part1-getting-started.md), [भाग 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md), आणि [भाग 3: SDKs and APIs](part3-sdk-and-apis.md) |

> **नोट:** Whisper मॉडेल्स SDK द्वारे डाउनलोड केले पाहिजेत (CLI द्वारे नाही). CLI ऑडिओ ट्रान्सक्रिप्शन एंडपॉइंटला समर्थन करत नाही. तुमची आवृत्ती तपासण्यासाठी:
> ```bash
> foundry --version
> ```

---

## संकल्पना: Whisper Foundry Local सह कसे काम करते

OpenAI Whisper मॉडेल हे विविध ऑडिओंच्या मोठ्या डेटासेटवर प्रशिक्षण झालेले एक सामान्य हेतूचे स्पीच रेकग्निशन मॉडेल आहे. Foundry Local मध्ये चालत असताना:

- मॉडेल पूर्णपणे **तुमच्या CPU** वर चालते - GPU ची गरज नाही
- ऑडिओ कधीही तुमच्या डिव्हाइसला सोडत नाही - **पूर्ण गोपनीयता**
- Foundry Local SDK मॉडेल डाउनलोड आणि कॅश व्यवस्थापन हाताळतो
- **JavaScript आणि C#** मध्ये SDK मध्ये अंगभूत `AudioClient` आहे, जो संपूर्ण ट्रान्सक्रिप्शन पाइपलाईन हाताळतो — कोणतीही ONNX सेटअप आवश्यक नाही
- **Python** मॉडेल व्यवस्थापनासाठी SDK वापरतो आणि एंडकोडर/डिकोडर ONNX मॉडेलसाठी थेट इनफरन्ससाठी ONNX Runtime वापरतो

### पाइपलाईन कसे काम करते (JavaScript आणि C#) — SDK AudioClient

1. **Foundry Local SDK** Whisper मॉडेल डाउनलोड करून कॅश करतं
2. `model.createAudioClient()` (JS) किंवा `model.GetAudioClientAsync()` (C#) एक `AudioClient` तयार करतं
3. `audioClient.transcribe(path)` (JS) किंवा `audioClient.TranscribeAudioAsync(path)` (C#) संपूर्ण पाइपलाईन अंतर्गत हाताळतात — ऑडिओ प्रीप्रोसेसिंग, एंडकोडर, डिकोडर, आणि टोकन डिकोडिंग
4. `AudioClient` मध्ये `settings.language` प्रॉपर्टी (इंग्रजीसाठी `"en"`) योग्य ट्रान्सक्रिप्शनसाठी निर्धारित केली जाते

### पाइपलाईन कसे काम करते (Python) — ONNX Runtime

1. **Foundry Local SDK** Whisper ONNX मॉडेल फाइल्स डाउनलोड करून कॅश करतो
2. **ऑडिओ प्रीप्रोसेसिंग** WAV ऑडिओला मेल स्पेक्ट्रोग्राम (80 मेल बिन्स x 3000 फ्रेम्स) मध्ये रूपांतरित करतो
3. **एंडकोडर** मेल स्पेक्ट्रोग्राम प्रक्रिया करून हिडन स्टेट्स आणि क्रॉस-अटेंशन की/व्हॅल्यू टेन्सर्स तयार करतो
4. **डिकोडर** ऑटोरेग्रेसीवली चालतो, एकावेळी एक टोकन जनरेट करतो जोपर्यंत तो एंड-ऑफ-टेक्स्ट टोकन परत करतो
5. **टोकनायझर** आउटपुट टोकन IDs वाचण्याजोग्या मजकुरात परत डिकोड करतो

### Whisper मॉडेलचे प्रकार

| उपनाम | मॉडेल ID | डिव्हाइस | आकार | वर्णन |
|-------|----------|----------|-------|---------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-त्वरित (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU-साठी ऑप्टिमाइझ्ड (अधिकांश डिव्हाइससाठी शिफारस केलेले) |

> **नोट:** चॅट मॉडेल्स प्रमाणे यादीत नाहीत, Whisper मॉडेल्स `automatic-speech-recognition` टास्क अंतर्गत वर्गीकृत आहेत. तपशील पाहण्यासाठी `foundry model info whisper-medium` वापरा.

---

## लॅब सराव

### सराव 0 - नमुना ऑडिओ फाइल्स मिळवा

या लॅबमध्ये Zava DIY उत्पादनांच्या प्रकरणांवर आधारित प्री-बिल्ट WAV फाइल्स आहेत. त्या समाविष्ट स्क्रिप्टने तयार करा:

```bash
# रेपो रूटमधून - प्रथम .venv तयार करा आणि सक्रिय करा
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

हे `samples/audio/` मध्ये सहा WAV फाइल्स तयार करते:

| फाइल | प्रकरण |
|------|----------|
| `zava-customer-inquiry.wav` | ग्राहक विचारत आहे **Zava ProGrip Cordless Drill** बद्दल |
| `zava-product-review.wav` | ग्राहक पुनरावलोकन करीत आहे **Zava UltraSmooth Interior Paint** चे |
| `zava-support-call.wav` | सपोर्ट कॉल आहे **Zava TitanLock Tool Chest** बद्दल |
| `zava-project-planning.wav` | DIYer डेकची योजना तयार करीत आहे **Zava EcoBoard Composite Decking** सह |
| `zava-workshop-setup.wav` | वॉकथ्रू वर्कशॉपचे जे **पाचही Zava उत्पादने** वापरत आहे |
| `zava-full-project-walkthrough.wav` | विस्तारित गॅरेज दुरुस्ती वॉकथ्रू, वापरत आहे **सर्व Zava उत्पादने** (~4 मिनिटे, लांब ऑडिओ चाचणीसाठी) |

> **टीप:** तुम्ही तुमचे स्वतःचे WAV/MP3/M4A फाइल्स देखील वापरू शकता, किंवा Windows Voice Recorder वापरून स्वतःचा रेकॉर्डिंग करू शकता.

---

### सराव 1 - SDK वापरून Whisper मॉडेल डाउनलोड करा

Whisper मॉडेल्ससाठी नवीन Foundry Local आवृत्त्यांतील CLI सुसंगततेमुळे, मॉडेल डाउनलोडसाठी आणि लोडसाठी **SDK** वापरा. तुमची भाषा निवडा:

<details>
<summary><b>🐍 Python</b></summary>

**SDK इन्स्टॉल करा:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# सेवा सुरू करा
manager = FoundryLocalManager()
manager.start_service()

# कॅटलॉग माहिती तपासा
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# आधीच कॅश केले आहे का तपासा
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# मॉडेल स्मृतीमध्ये लोड करा
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py` म्हणून जतन करा आणि चालवा:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK इन्स्टॉल करा:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// मॅनेजर तयार करा आणि सेवा सुरू करा
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// कॅटलॉगमधून मॉडेल मिळवा
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

// मॉडेल मेमरीमध्ये लोड करा
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs` म्हणून जतन करा आणि चालवा:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK इन्स्टॉल करा:**
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

> **SDK का CLI नाही?** Foundry Local CLI Whisper मॉडेल्स थेट डाउनलोड किंवा सर्व्ह करण्यास समर्थ नाही. SDK प्रोग्रामॅटिकली ऑडिओ मॉडेल्स डाउनलोड व व्यवस्थापनासाठी विश्वसनीय मार्ग प्रदान करतो. JavaScript आणि C# SDK मध्ये अंगभूत `AudioClient` असतो जो संपूर्ण ट्रान्सक्रिप्शन पाइपलाईन हाताळतो. Python थेट संचित मॉडेल फाइलवर ONNX Runtime वापरतो.

---

### सराव 2 - Whisper SDK समजून घ्या

Whisper ट्रान्सक्रिप्शन लँग्वेजनुसार वेगळ्या पद्धती वापरतो. **JavaScript आणि C#** मध्ये Foundry Local SDK मध्ये अंगभूत `AudioClient` असतो जो पूर्ण पाइपलाईन (ऑडिओ प्रीप्रोसेसिंग, एंडकोडर, डिकोडर, टोकन डिकोडिंग) एका कॉलमध्ये करते. **Python** मध्ये Foundry Local SDK मॉडेल मॅनेजमेंटसाठी आणि एंडकोडर/डिकोडर ONNX मॉडेल्ससाठी थेट इनफरन्ससाठी ONNX Runtime वापरतो.

| घटक | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK पॅकेजेस** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **मॉडेल व्यवस्थापन** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **फीचर एक्सट्रॅक्शन** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` हाताळतो | SDK `AudioClient` हाताळतो |
| **इनफरन्स** | `ort.InferenceSession` (एंडकोडर + डिकोडर) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **टोकन डिकोडिंग** | `WhisperTokenizer` | SDK `AudioClient` हाताळतो | SDK `AudioClient` हाताळतो |
| **भाषा सेटिंग** | डिकोडर टोकन्समध्ये `forced_ids` वापरा | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **इनपुट** | WAV फाइल पथ | WAV फाइल पथ | WAV फाइल पथ |
| **आउटपुट** | डिकोडेड टेक्स्ट स्ट्रिंग | `result.text` | `result.Text` |

> **महत्त्वाचे:** `AudioClient` वर नेहमी भाषा सेट करा (उदा. इंग्रजीसाठी `"en"`). भाषा सेट न केल्यास मॉडेल चुकीची आउटपुट देऊ शकतो कारण तो भाषा ऑटो-डिटेक्ट करण्याचा प्रयत्न करतो.

> **SDK पॅटर्न्स:** Python मध्ये `FoundryLocalManager(alias)` वापरून बूटस्ट्रॅप करा, नंतर `get_cache_location()` वापरून ONNX मॉडेल फाइल्स शोधा. JavaScript आणि C# मध्ये SDK चा अंगभूत `AudioClient` वापरा — `model.createAudioClient()` (JS) किंवा `model.GetAudioClientAsync()` (C#), जो संपूर्ण ट्रान्सक्रिप्शन पाइपलाईन हाताळतो. पूर्ण तपशीलांसाठी पाहा [भाग 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md).

---

### सराव 3 - साधे ट्रान्सक्रिप्शन अ‍ॅप तयार करा

तुमचा भाषा ट्रॅक निवडा आणि एक साधे अ‍ॅप तयार करा जे ऑडिओ फाइल ट्रान्सक्राइब करेल.

> **समर्थित ऑडिओ फॉरमॅट्स:** WAV, MP3, M4A. सर्वोत्तम निकालांसाठी 16kHz सॅम्पल रेट असलेले WAV फाइल्स वापरा.

<details>
<summary><h3>Python ट्रॅक</h3></summary>

#### सेटअप

```bash
cd python
python -m venv venv

# आभासी वातावरण सक्रिय करा:
# विंडोज (पॉवरशेल):
venv\Scripts\Activate.ps1
# मॅकोएस:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### ट्रान्सक्रिप्शन कोड

`foundry-local-whisper.py` नावाचा एक फाइल तयार करा:

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

# पायरी 1: बूटस्ट्रॅप - सेवा सुरू करते, डाउनलोड करते आणि मॉडेल लोड करते
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# कॅश केलेल्या ONNX मॉडेल फाइल्ससाठी मार्ग तयार करा
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# पायरी 2: ONNX सेशन्स आणि फिचर एक्स्ट्रॅक्टर लोड करा
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

# पायरी 3: मेल स्पेक्ट्रोग्राम वैशिष्ट्ये काढा
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# पायरी 4: एन्कोडर चालवा
enc_out = encoder.run(None, {"audio_features": input_features})
# पहिला आउटपुट हिडन स्टेट्स आहे; उरलेले क्रॉस-अटेंशन KV जोड्या आहेत
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# पायरी 5: ऑटोरेग्रेसीव्ह डिकोडिंग
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, ट्रान्सक्राइब, notimestamps
input_ids = np.array([initial_tokens], dtype=np.int32)

# रिकामा सेल्फ-अटेंशन KV कॅश
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

    if next_token == 50257:  # मजकुराचा शेवट
        break
    generated.append(next_token)

    # सेल्फ-अटेंशन KV कॅश अपडेट करा
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### ते चालवा

```bash
# Zava उत्पादन परिस्थिती टाइप करा
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# किंवा इतर प्रयत्न करा:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Python महत्त्वाचे मुद्दे

| पद्धत | कार्य |
|--------|---------|
| `FoundryLocalManager(alias)` | बूटस्ट्रॅप: सेवा सुरू करा, डाउनलोड करा आणि मॉडेल लोड करा |
| `manager.get_cache_location()` | कॅश केलेल्या ONNX मॉडेल फाइल्सचा मार्ग मिळवा |
| `WhisperFeatureExtractor.from_pretrained()` | मेल स्पेक्ट्रोग्राम फीचर एक्सट्रॅक्टर लोड करा |
| `ort.InferenceSession()` | एंडकोडर आणि डिकोडरसाठी ONNX Runtime सत्र तयार करा |
| `tokenizer.decode()` | आउटपुट टोकन IDs टेक्स्टमध्ये रूपांतरित करा |

</details>

<details>
<summary><h3>JavaScript ट्रॅक</h3></summary>

#### सेटअप

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### ट्रान्सक्रिप्शन कोड

`foundry-local-whisper.mjs` नावाचा फाइल तयार करा:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// टप्पा 1: बूटस्ट्रॅप - व्यवस्थापक तयार करा, सेवा सुरू करा, आणि मॉडेल लोड करा
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

// टप्पा 2: एक ऑडिओ क्लायंट तयार करा आणि ट्रांसक्राइब करा
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// स्वच्छता
await model.unload();
```

> **नोट:** Foundry Local SDK मध्ये `model.createAudioClient()` वापरून अंगभूत `AudioClient` देतो जो संपूर्ण ONNX इनफरन्स पाइपलाईन अंतर्गत हाताळतो — `onnxruntime-node` आयात करण्याची गरज नाही. नेहमीच `audioClient.settings.language = "en"` सेट करा जेणेकरून इंग्रजी ट्रान्सक्रिप्शन अचूक होईल.

#### चालवा

```bash
# झावा उत्पादन परिस्थितीचे प्रतिलेखन करा
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# किंवा इतर प्रयत्न करा:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### JavaScript महत्त्वाचे मुद्दे

| पद्धत | कार्य |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | मॅनेजर सिंगलटन तयार करा |
| `await catalog.getModel(alias)` | कॅटलॉगमधून मॉडेल मिळवा |
| `model.download()` / `model.load()` | Whisper मॉडेल डाउनलोड आणि लोड करा |
| `model.createAudioClient()` | ट्रान्सक्रिप्शनसाठी ऑडिओ क्लायंट तयार करा |
| `audioClient.settings.language = "en"` | ट्रान्सक्रिप्शन भाषा सेट करा (अचूक आउटपुटसाठी आवश्यक) |
| `audioClient.transcribe(path)` | ऑडिओ फाइल ट्रान्सक्राइब करा, परत करते `{ text, duration }` |

</details>

<details>
<summary><h3>C# ट्रॅक</h3></summary>

#### सेटअप

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **नोट:** C# ट्रॅक `Microsoft.AI.Foundry.Local` पॅकेज वापरतो, जो `model.GetAudioClientAsync()` द्वारे अंगभूत `AudioClient` पुरवतो. हे पूर्ण ट्रान्सक्रिप्शन पाइपलाईन इन-प्रोसेस हाताळतो — वेगळ्या ONNX Runtime सेटअपची गरज नाही.

#### ट्रान्सक्रिप्शन कोड

`Program.cs` चे कंटेन्ट खालील प्रमाणे बदला:

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

#### चालवा

```bash
# झावा उत्पादन परिस्थिती ट्रान्सक्राइब करा
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# किंवा इतर प्रयत्न करा:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### C# महत्त्वाचे मुद्दे

| पद्धत | कार्य |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | कॉन्फिगरेशनसह Foundry Local प्रारंभ करा |
| `catalog.GetModelAsync(alias)` | कॅटलॉगमधून मॉडेल मिळवा |
| `model.DownloadAsync()` | Whisper मॉडेल डाउनलोड करा |
| `model.GetAudioClientAsync()` | AudioClient मिळवा (ChatClient नाही!) |
| `audioClient.Settings.Language = "en"` | ट्रान्सक्रिप्शन भाषा सेट करा (अचूक आउटपुटसाठी आवश्यक) |
| `audioClient.TranscribeAudioAsync(path)` | ऑडिओ फाइल ट्रान्सक्राइब करा |
| `result.Text` | ट्रान्सक्राइब केलेला मजकूर |
> **C# vs Python/JS:** C# SDK मध्ये `model.GetAudioClientAsync()` द्वारे इन-प्रोसेस ट्रान्स्क्रिप्शनसाठी अंगभूत `AudioClient` प्रदान केले जाते, जे जावास्क्रिप्ट SDK सारखेच आहे. पायथन थेट ONNX Runtime वापरतो ज्याचा उपयोग कॅश केलेल्या एनकोडर/डिकोडर मॉडेल्सवर इनफेरन्ससाठी केला जातो.

</details>

---

### व्यायाम ४ - सर्व झवा नमुने एकत्र ट्रान्स्क्राइब करा

आता तुमच्याकडे काम करणारी ट्रान्स्क्रिप्शन अॅप आहे, सर्व पाच झवा नमुना फाइल्स ट्रान्स्क्राइब करा आणि निकालांची तुलना करा.

<details>
<summary><h3>पायथन ट्रॅक</h3></summary>

पूर्ण नमुना `python/foundry-local-whisper.py` आधीच बॅच ट्रान्स्क्रिप्शनला समर्थन देतो. बिना अर्ग्युमेंट्सशिवाय चालवल्यास, तो `samples/audio/` मधील सर्व `zava-*.wav` फाइल्स ट्रान्स्क्राइब करतो:

```bash
cd python
python foundry-local-whisper.py
```

हा नमुना `FoundryLocalManager(alias)` वापरून बूटस्ट्रॅप करतो, नंतर प्रत्येक फाइलसाठी एनकोडर आणि डिकोडर ONNX सत्र चालवतो.

</details>

<details>
<summary><h3>जावास्क्रिप्ट ट्रॅक</h3></summary>

पूर्ण नमुना `javascript/foundry-local-whisper.mjs` आधीच बॅच ट्रान्स्क्रिप्शनला समर्थन देतो. बिना अर्ग्युमेंट्सशिवाय चालवल्यास, तो `samples/audio/` मधील सर्व `zava-*.wav` फाइल्स ट्रान्स्क्राइब करतो:

```bash
cd javascript
node foundry-local-whisper.mjs
```

हा नमुना SDK प्रारंभ करण्यासाठी `FoundryLocalManager.create()` आणि `catalog.getModel(alias)` वापरतो, नंतर प्रत्येक फाइल ट्रान्स्क्राइब करण्यासाठी `AudioClient` (सेटिंग्जमध्ये `language = "en"`) वापरतो.

</details>

<details>
<summary><h3>C# ट्रॅक</h3></summary>

पूर्ण नमुना `csharp/WhisperTranscription.cs` आधीच बॅच ट्रान्स्क्रिप्शनला समर्थन देतो. विशिष्ट फाइल अर्ग्युमेंटशिवाय चालवल्यास, तो `samples/audio/` मधील सर्व `zava-*.wav` फाइल्स ट्रान्स्क्राइब करतो:

```bash
cd csharp
dotnet run whisper
```

हा नमुना in-process ट्रान्स्क्रिप्शनसाठी `FoundryLocalManager.CreateAsync()` आणि SDK चा `AudioClient` (सेटिंग्जमध्ये `Language = "en"`) वापरतो.

</details>

**काय पाहायचे:** `samples/audio/generate_samples.py` मधील मूळ मजकुराशी ट्रान्स्क्रिप्शन आउटपुटची तुलना करा. "Zava ProGrip" सारखे उत्पादन नावे आणि "brushless motor" किंवा "composite decking" सारखे तांत्रिक संज्ञा Whisper किती अचूकपणे पकडतो?

---

### व्यायाम ५ - मुख्य कोड नमुन्यांचा अभ्यास करा

सर्व तीन भाषांमधील Whisper ट्रान्स्क्रिप्शन आणि चॅट पूर्णता यातील फरक अभ्यास करा:

<details>
<summary><b>पायथन - चॅटपासून मुख्य फरक</b></summary>

```python
# चॅट पूर्णता (भाग 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# ऑडिओ ट्रान्सक्रिप्शन (हा भाग):
# OpenAI क्लायंटऐवजी थेट ONNX Runtime वापरतो
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... ऑटोरिग्रेडिव्ह डिकोडर लूप ...
print(tokenizer.decode(generated_tokens))
```

**मुख्य अंतर्दृष्टी:** चॅट मॉडेल OpenAI-समर्थित API (`manager.endpoint`) वापरतात. Whisper SDK वापरून कॅश केलेल्या ONNX मॉडेल फाइल्स शोधतो आणि नंतर थेट ONNX Runtime वापरून इनफेरन्स चालवतो.

</details>

<details>
<summary><b>जावास्क्रिप्ट - चॅटपासून मुख्य फरक</b></summary>

```javascript
// चॅट पूर्णता (भाग 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// ऑडिओ ट्रान्सक्रिप्शन (हा भाग):
// SDK मधील अंगभूत AudioClient वापरतो
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // सर्वोत्तम निकालांसाठी नेहमी भाषा सेट करा
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**मुख्य अंतर्दृष्टी:** चॅट मॉडेल OpenAI-समर्थित API `manager.urls[0] + "/v1"` वापरतात. Whisper ट्रान्स्क्रिप्शन SDK च्या `AudioClient` (`model.createAudioClient()` मधून मिळवलेले) वापरते. गोंधळट आउटपुट टाळण्यासाठी `settings.language` ठेवा.

</details>

<details>
<summary><b>C# - चॅटपासून मुख्य फरक</b></summary>

C# पद्धतीत SDK चा अंगभूत `AudioClient` वापरून in-process ट्रान्स्क्रिप्शन केले जाते:

**मॉडेल आरंभीकरण:**

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

**ट्रान्स्क्रिप्शन:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**मुख्य अंतर्दृष्टी:** C# मध्ये `FoundryLocalManager.CreateAsync()` वापरून थेट `AudioClient` मिळवतात — ONNX Runtime सेटअपची गरज नाही. गोंधळट आउटपुट टाळण्यासाठी `Settings.Language` सेट करा.

</details>

> **सारांश:** पायथन मॉडेल व्यवस्थापनासाठी Foundry Local SDK वापरतो आणि थेट एनकोडर/डिकोडर मॉडेल्सवर इनफेरन्ससाठी ONNX Runtime वापरतो. जावास्क्रिप्ट आणि C# दोन्ही SDK च्या अंगभूत `AudioClient` वापरतात, ज्यात ग्राहक तयार करा, भाषा सेट करा आणि `transcribe()` / `TranscribeAudioAsync()` कॉल करा. अचूक निकालांसाठी नेहमी `AudioClient` च्या भाषा प्रॉपर्टी सेट करा.

---

### व्यायाम ६ - प्रयोग करा

तुमच्या समजास अधिक खोल करण्यासाठी खालील बदल करा:

1. **वेगळ्या ऑडिओ फाइल्स वापरून पहा** - Windows Voice Recorder वापरून स्वतःचा व्हॉईस रेकॉर्ड करा, WAV मध्ये जतन करा आणि ट्रान्स्क्राइब करा

2. **मॉडेल व्हेरिएंट्सची तुलना करा** - तुमच्याकडे NVIDIA GPU असल्यास CUDA व्हेरिएंट वापरून पहा:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   
CPU व्हेरिएंटच्या तुलनेत ट्रान्स्क्रिप्शन वेग तपासा.

3. **आउटपुट फॉरमॅटिंग जोडा** - JSON प्रतिसादात यांचा समावेश होऊ शकतो:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```


4. **REST API तयार करा** - तुमचा ट्रान्स्क्रिप्शन कोड वेब सर्व्हरमध्ये रॅप करा:

   | भाषा | फ्रेमवर्क | उदाहरण |
   |----------|-----------|--------|
   | पायथन | FastAPI | `@app.post("/v1/audio/transcriptions")` सह `UploadFile` |
   | जावास्क्रिप्ट | Express.js | `app.post("/v1/audio/transcriptions")` सह `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` सह `IFormFile` |

5. **मल्टी-टर्न ट्रान्स्क्रिप्शनसह** - भाग ४ मधील चॅट एजंटसह Whisper संयोजित करा: आधी ऑडिओ ट्रान्स्क्राइब करा, नंतर टेक्स्ट एजंटसाठी विश्लेषण किंवा सारांशासाठी पाठवा.

---

## SDK ऑडिओ API संदर्भ

> **जावास्क्रिप्ट AudioClient:**
> - `model.createAudioClient()` — `AudioClient` उदाहरण तयार करते
> - `audioClient.settings.language` — ट्रान्स्क्रिप्शन भाषा सेट करा (उदा. `"en"`)
> - `audioClient.settings.temperature` — यादृच्छिकता नियंत्रित करा (ऐच्छिक)
> - `audioClient.transcribe(filePath)` — फाइल ट्रान्स्क्राइब करा, परत करते `{ text, duration }`
> - `audioClient.transcribeStreaming(filePath, callback)` — कॉलबॅकद्वारे ट्रान्स्क्रिप्शन तुकडे प्रवाहित करा
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — `OpenAIAudioClient` उदाहरण तयार करते
> - `audioClient.Settings.Language` — ट्रान्स्क्रिप्शन भाषा सेट करा (उदा. `"en"`)
> - `audioClient.Settings.Temperature` — यादृच्छिकता नियंत्रित करा (ऐच्छिक)
> - `await audioClient.TranscribeAudioAsync(filePath)` — फाइल ट्रान्स्क्राइब करा, `.Text` असलेल्या ऑब्जेक्ट परत करतो
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — ट्रान्स्क्रिप्शन तुकड्यांचा `IAsyncEnumerable` परत करतो

> **टीप:** ट्रान्स्क्राइब करण्यापूर्वी नेहमी भाषा प्रॉपर्टी सेट करा. यात नसेल तर Whisper मॉडेल ऑटो-डिटेक्शन करते, ज्यामुळे गोंधळट आउटपुट (मजकूराऐवजी एकल रिप्लेसमेंट कॅरेक्टर) येऊ शकतो.

---

## तुलना: चॅट मॉडेल्स विरुद्ध Whisper

| पैलू | चॅट मॉडेल्स (भाग ३-७) | Whisper - पायथन | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **कार्य प्रकार** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **इनपुट** | मजकूर संदेश (JSON) | ऑडिओ फाइल्स (WAV/MP3/M4A) | ऑडिओ फाइल्स (WAV/MP3/M4A) |
| **आउटपुट** | तयार केलेला मजकूर (स्ट्रीमिंग) | ट्रान्स्क्राइब केलेला मजकूर (पूर्ण) | ट्रान्स्क्राइब केलेला मजकूर (पूर्ण) |
| **SDK पॅकेज** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API पद्धत** | `client.chat.completions.create()` | ONNX Runtime थेट | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **भाषा सेटिंग** | लागू नाही | डिकोडर प्रॉम्प्ट टोकन्स | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **स्ट्रीमिंग** | होय | नाही | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **गोपनीयतेचा फायदा** | कोड/डेटा स्थानिक राहतो | ऑडिओ डेटा स्थानिक राहतो | ऑडिओ डेटा स्थानिक राहतो |

---

## मुख्य मुद्दे

| संकल्पना | तुम्ही काय शिकलात |
|---------|-----------------|
| **व्हिस्पर डिव्हाइसवर** | स्पीच-टू-टेक्स्ट पूर्णपणे स्थानिक चालते, झवा ग्राहक कॉल आणि उत्पादन पुनरावलोकने डिव्हाइसवर ट्रान्स्क्राइब करणे आदर्श |
| **SDK AudioClient** | जावास्क्रिप्ट आणि C# SDKs अंगभूत `AudioClient` पुरवतात जी संपूर्ण ट्रान्स्क्रिप्शन पाईपलाइन एका कॉलमध्ये हाताळते |
| **भाषा सेटिंग** | नेहमी AudioClient ची भाषा सेट करा (उदा. `"en"`) — नसेल तर ऑटो-डिटेक्शनमुळे गोंधळट आउटपुट येऊ शकतो |
| **पायथन** | मॉडेल व्यवस्थापनासाठी `foundry-local-sdk`, थेट ONNX इनफेरन्ससाठी `onnxruntime` + `transformers` + `librosa` वापरतो |
| **जावास्क्रिप्ट** | `foundry-local-sdk` सह `model.createAudioClient()` वापरतो — `settings.language` सेट करा, नंतर `transcribe()` कॉल करा |
| **C#** | `Microsoft.AI.Foundry.Local` सह `model.GetAudioClientAsync()` वापरतो — `Settings.Language` सेट करा, नंतर `TranscribeAudioAsync()` कॉल करा |
| **स्ट्रीमिंग समर्थन** | JS आणि C# SDKs `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` देखील ऑफर करतात, तुकड्याने आउटपुटसाठी |
| **CPU-ऑप्टिमाइझ्ड** | CPU व्हेरिएंट (3.05 GB) कोणत्याही Windows डिव्हाइसवर GPU शिवाय चालतो |
| **गोपनीयतेवर प्रथम** | झवा ग्राहक संवाद आणि मालकीचे उत्पादन डेटा डिव्हाइसवर ठेवण्यासाठी परिपूर्ण |

---

## संसाधने

| संसाधन | दुवा |
|----------|------|
| Foundry Local दस्तऐवज | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK संदर्भ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper मॉडेल | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local वेबसाइट | [foundrylocal.ai](https://foundrylocal.ai) |

---

## पुढील टप्पा

[भाग १०: कस्टम किंवा हगिंग फेस मॉडेल वापरणे](part10-custom-models.md) कडे पुढे जा जेथे तुम्ही हगिंग फेसचे स्वतःचे मॉडेल कॉम्पाइल करून Foundry Local वापरून चालवू शकता.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:  
हा दस्तऐवज AI भाषांतर सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) वापरून भाषांतरित केला आहे. जरी आम्ही अचूकतेसाठी प्रयत्न करीत असलो तरी, कृपया लक्षात घ्या की स्वयंचलित भाषांतरांमध्ये चुका किंवा अपूर्णता असू शकते. मूळ दस्तऐवज त्याच्या स्थानिक भाषेत अधिकृत स्रोत मानला जावा. महत्त्वाची माहिती मिळविण्यासाठी व्यावसायिक मानवी भाषांतर शिफारस केली जाते. या भाषांतराच्या वापरामुळे निर्माण होणाऱ्या कोणत्याही गैरसमजुती किंवा चुकीच्या अर्थलहरीसाठी आम्ही जबाबदार नाही.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->