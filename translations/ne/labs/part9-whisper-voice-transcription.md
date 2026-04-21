![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग ९: Whisper र Foundry Local सँग आवाज ट्रान्सक्रिप्सन

> **लक्ष्य:** Foundry Local मार्फत स्थानीय रूपमा चल्ने OpenAI Whisper मोडेल प्रयोग गरेर अडियो फाइलहरू ट्रान्सक्राइब गर्नुहोस् - पूर्ण रूपमा यन्त्रमा, कुनै क्लाउड आवश्यक छैन।

## अवलोकन

Foundry Local केवल पाठ उत्पादनका लागि होइन्; यसले **स्पीच-टू-टेक्स्ट** मोडेलहरू पनि समर्थन गर्छ। यस ल्याबमा तपाईंले **OpenAI Whisper Medium** मोडेल प्रयोग गरेर अडियो फाइलहरू पूर्ण रूपमा तपाईंको मेसिनमा ट्रान्सक्राइब गर्नुहुनेछ। यो यस्तो अवस्थाहरूका लागि आदर्श छ जहाँ जस्तै Zava ग्राहक सेवा कलहरू, उत्पादन समीक्षा रेकर्डिङहरू, वा कार्यशाला योजना सत्रहरू जहाँ अडियो डाटा कहिल्यै तपाईंको यन्त्रबाट बाहिर जानु हुँदैन।

---

## सिकाइ लक्ष्यहरू

यस ल्याबको अन्त्यसम्म तपाईं सक्षम हुनु हुनेछ:

- Whisper स्पीच-टू-टेक्स्ट मोडेल र यसको क्षमता बुझ्न
- Foundry Local प्रयोग गरेर Whisper मोडेल डाउनलोड र चलाउन
- Foundry Local SDK प्रयोग गरी Python, JavaScript, र C# मा अडियो फाइलहरू ट्रान्सक्राइब गर्न
- पूर्ण रूपमा यन्त्रमा चल्ने एक सरल ट्रान्सक्रिप्सन सेवा बनाउने
- Foundry Local मा च्याट/टेक्स्ट मोडेल र अडियो मोडेल बीचको फरक बुझ्न

---

## पूर्वशर्तहरू

| आवश्यकताहरू | विवरणहरू |
|-------------|---------|
| **Foundry Local CLI** | संस्करण **0.8.101 वा माथि** (Whisper मोडेलहरू v0.8.101 देखि उपलब्ध) |
| **अपरेटिङ सिस्टम** | Windows 10/11 (x64 वा ARM64) |
| **प्रोग्रामिङ भाषा रनटाइम** | **Python 3.9+** र/वा **Node.js 18+** र/वा **.NET 9 SDK** ([Download .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **सम्पूर्ण गरिसक्नु भएको** | [भाग १: सुरुवात गर्दै](part1-getting-started.md), [भाग २: Foundry Local SDK गहिरो अध्ययन](part2-foundry-local-sdk.md), र [भाग ३: SDK र APIs](part3-sdk-and-apis.md) |

> **सूचना:** Whisper मोडेलहरू मात्र **SDK** मार्फत डाउनलोड गर्नुपर्छ (CLI द्वारा होइन)। CLI मा अडियो ट्रान्सक्रिप्सन इन्डपोइन्ट समर्थन छैन। आफ्नो संस्करण जाँच्न:
> ```bash
> foundry --version
> ```

---

## अवधारणा: Whisper कसरी Foundry Local सँग काम गर्छ

OpenAI Whisper मोडेल एउटा बहुउद्देश्यीय स्पीच रिकग्निसन मोडेल हो जुन ठूलो विविध अडियो डेटासेटमा तालिम पाएको छ। Foundry Local मार्फत चलाउँदा:

- मोडेल ** पूर्ण रूपमा तपाईंको CPU मा चल्छ** - कुनै GPU आवश्यक छैन
- अडियो कहिल्यै तपाईंको यन्त्र छोड्दैन - **पूर्ण गोपनीयता**
- Foundry Local SDK मोडेल डाउनलोड र क्यास प्रबंधन गर्छ
- **JavaScript र C#** मा SDK भित्रै `AudioClient` छ जसले सम्पूर्ण ट्रान्सक्रिप्सन पाइपलाइनलाई ह्यान्डल गर्छ — कुनै म्यानुअल ONNX सेटअप आवश्यक छैन
- **Python** ले SDK प्रयोग गरेर मोडल प्रबंधन गर्छ र ONNX Runtime ले encoder/decoder ONNX मोडेलहरूमाथि सीधा inference गर्छ

### पाइपलाइन कसरी चल्छ (JavaScript र C#) — SDK AudioClient

1. **Foundry Local SDK** ले Whisper मोडेल डाउनलोड र क्यास गर्छ
2. `model.createAudioClient()` (JS) वा `model.GetAudioClientAsync()` (C#) ले `AudioClient` बनाउँछ
3. `audioClient.transcribe(path)` (JS) वा `audioClient.TranscribeAudioAsync(path)` (C#) ले सम्पूर्ण पाइपलाइन भित्रै ह्यान्डल गर्छ — अडियो पूर्वप्रसंस्करण, encoder, decoder, र टोकन डिकोडिङ
4. `AudioClient` मा `settings.language` प्रॉपर्टी छ (अंग्रेजीको लागि `"en"` सेट गर्नुहोस्) जसले सही ट्रान्सक्रिप्सनमा मार्गदर्शन गर्छ

### पाइपलाइन कसरी चल्छ (Python) — ONNX Runtime

1. **Foundry Local SDK** ले Whisper ONNX मोडेल फाइलहरू डाउनलोड र क्यास गर्छ
2. **अडियो पूर्वप्रसंस्करण** WAV अडियोलाई मेल स्पेक्ट्रोग्राम (८० मेल बिन × ३००० फ्रेम) मा रूपान्तरण गर्छ
3. **Encoder** मेल स्पेक्ट्रोग्राम प्रक्रिया गर्छ र हिडेन स्टेटहरू साथै क्रस-अटेन्सन कुञ्जी/मान टेन्सरहरू उत्पादन गर्छ
4. **Decoder** autoregressive ढंगले चल्छ, एक पटकमा एक टोकन उत्पादन गर्दै जबसम्म अन्त्य-टेक्स्ट टोकन नआउँछ
5. **Tokeniser** आउटपुट टोकन ID लाई पढ्न मिल्ने पाठमा डिकोड गर्छ

### Whisper मोडेल भेरियन्टहरू

| उपनाम | मोडेल ID | यन्त्र | आकार | विवरण |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | १.५३ GB | GPU-गतिशील (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | ३.०५ GB | CPU-उपयुक्त (अधिकांश यन्त्रहरूको लागि सिफारिस गरिएको) |

> **सूचना:** डिफ़ॉल्ट रूपमा च्याट मोडलहरू जस्तै सूचीबद्ध नभई, Whisper मोडेलहरू `automatic-speech-recognition` कार्य अन्तर्गत वर्गीकरण गरिएका छन्। विवरण हेर्न `foundry model info whisper-medium` प्रयोग गर्नुहोस्।

---

## ल्याब अभ्यासहरू

### अभ्यास ० - नमूना अडियो फाइलहरू प्राप्त गर्नुहोस्

यो ल्याबमा Zava DIY उत्पादन परिस्थितिमा आधारित पहिले नै बनाइएका WAV फाइलहरू समावेश छन्। समावेश गरिएको स्क्रिप्ट चलाएर तिनीहरू सिर्जना गर्नुहोस्:

```bash
# रिपो रूटबाट - पहिले .venv सिर्जना र सक्रिय गर्नुहोस्
python -m venv .venv

# विन्डोज (पावरशेल):
.venv\Scripts\Activate.ps1
# म्याकओएस:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

यसले `samples/audio/` मा छवटा WAV फाइलहरू सिर्जना गर्छ:

| फाइल | परिस्थिति |
|------|----------|
| `zava-customer-inquiry.wav` | ग्राहक Zava ProGrip Cordless Drill सम्बन्धमा प्रश्न गर्दै छन् |
| `zava-product-review.wav` | ग्राहक Zava UltraSmooth Interior Paint समीक्षा गर्दै छन् |
| `zava-support-call.wav` | Zava TitanLock Tool Chest सम्बन्धी समर्थन कल |
| `zava-project-planning.wav` | DIYer ले Zava EcoBoard Composite Decking प्रयोग गरी डेक योजना गर्दै |
| `zava-workshop-setup.wav` | सबै पाँच Zava उत्पादनहरू प्रयोग गरेर कार्यशाला अवलोकन |
| `zava-full-project-walkthrough.wav` | सबै Zava उत्पादनहरूको लामो (~४ मिनेट) प्रयोग गरी ग्यारेज पुनर्निर्माण अवलोकन |

> **सुझाव:** तपाईंले आफ्नै WAV/MP3/M4A फाइलहरू पनि प्रयोग गर्न सक्नुहुन्छ, वा Windows Voice Recorder बाट आफैं रेकर्ड गर्न सक्नुहुन्छ।

---

### अभ्यास १ - SDK प्रयोग गरी Whisper मोडेल डाउनलोड गर्नुहोस्

Foundry Local को नयाँ संस्करणहरूमा CLI र Whisper मोडेलको असंगतताको कारण, मोडेल डाउनलोड र लोड गर्न **SDK** प्रयोग गर्नुहोस्। आफ्नो भाषा छान्नुहोस्:

<details>
<summary><b>🐍 Python</b></summary>

**SDK स्थापना गर्नुहोस्:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# सेवा सुरु गर्नुहोस्
manager = FoundryLocalManager()
manager.start_service()

# सूची जानकारी जाँच गर्नुहोस्
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# पहिले नै क्यास गरिएको छ कि छैन जाँच गर्नुहोस्
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# मोडेललाई स्मृतिमा लोड गर्नुहोस्
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py` नामले सुरक्षित गरेर चलाउनुहोस्:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK स्थापना गर्नुहोस्:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// मेनेजर सिर्जना गर्नुहोस् र सेवा सुरु गर्नुहोस्
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// क्याटलगबाट मोडेल प्राप्त गर्नुहोस्
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

// मोडेललाई मेमोरीमा लोड गर्नुहोस्
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs` नामले सुरक्षित गरेर चलाउनुहोस्:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK स्थापना गर्नुहोस्:**
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

> **किन SDK CLI होइन?** Foundry Local CLI ले Whisper मोडेलहरू सिधै डाउनलोड वा सेवा गर्न समर्थन गर्दैन। SDK ले प्रोग्राम्याटिक रुपमा अडियो मोडेलहरू डाउनलोड र व्यवस्थापन गर्ने भरपर्दो तरिका प्रदान गर्दछ। JavaScript र C# SDKs मा बिल्ट-इन `AudioClient` हुन्छ जसले सम्पूर्ण ट्रान्सक्रिप्सन पाइपलाइन ह्यान्डल गर्छ। Python ले ONNX Runtime प्रयोग गरेर क्यास गरिएको मोडेल फाइलहरूमाथि सीधा inference गर्छ।

---

### अभ्यास २ - Whisper SDK बुझ्नुहोस्

Whisper ट्रान्सक्रिप्सन भाषाअनुसार फरक तरिकाले काम गर्छ। **JavaScript र C#** मा Foundry Local SDK भित्र बिल्ट-इन `AudioClient` हुन्छ जसले सम्पूर्ण पाइपलाइन (अडियो पूर्वप्रसंस्करण, encoder, decoder, टोकन डिकोडिङ) एकल विधि कलमा ह्यान्डल गर्छ। **Python** ले Foundry Local SDK प्रयोग गरेर मोडेल व्यवस्थापन गर्छ र ONNX Runtime प्रयोग गरेर encoder/decoder ONNX मोडेलहरूमा सीधा inference गर्छ।

| कम्पोनेन्ट | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK प्याकेजहरू** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **मोडेल व्यवस्थापन** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **विशेषता निष्कर्षण** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` ले ह्यान्डल गर्छ | SDK `AudioClient` ले ह्यान्डल गर्छ |
| **Inference** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **टोकन डिकोडिङ** | `WhisperTokenizer` | SDK `AudioClient` ले ह्यान्डल गर्छ | SDK `AudioClient` ले ह्यान्डल गर्छ |
| **भाषा सेटिङ** | decoder टोकनमा `forced_ids` मार्फत सेट | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **इनपुट** | WAV फाइल पथ | WAV फाइल पथ | WAV फाइल पथ |
| **आउटपुट** | डिकोड गरिएको टेक्स्ट स्ट्रिङ | `result.text` | `result.Text` |

> **महत्वपूर्ण:** `AudioClient` मा भाषाबाट स्पष्ट सेटिङ गर्नुहोस् (उदाहरणका लागि अंग्रेजीको लागि `"en"`). स्पष्टीकरण बिना मोडेल गलत वा खराब ट्रान्सक्रिप्सन उत्पादन गर्न सक्छ किनभने यो स्वचालित भाषा पहिचान गर्ने प्रयास गर्दछ।

> **SDK ढाँचाहरू:** Python ले `FoundryLocalManager(alias)` प्रयोग गरेर सुरु गर्छ, त्यसपछि ONNX मोडेल फाइलहरूको स्थान भेट्टाउन `get_cache_location()` चलाउँछ। JavaScript र C# मा SDK को बिल्ट-इन `AudioClient` हुन्छ — जसलाई `model.createAudioClient()` (JS) वा `model.GetAudioClientAsync()` (C#) बाट प्राप्त गर्न सकिन्छ — जुन सम्पूर्ण ट्रान्सक्रिप्सन पाइपलाइन ह्यान्डल गर्छ। पुरा जानकारीका लागि [भाग २: Foundry Local SDK गहिरो अध्ययन](part2-foundry-local-sdk.md) हेर्नुहोस्।

---

### अभ्यास ३ - एक सरल ट्रान्सक्रिप्सन एप बनाउनुहोस्

आफ्नो भाषा ट्रयाक छान्नुस् र एउटा न्यूनतम अनुप्रयोग बनाउनुहोस् जुन अडियो फाइल ट्रान्सक्राइब गर्छ।

> **समर्थित अडियो फारम्याटहरू:** WAV, MP3, M4A। सबैभन्दा राम्रो परिणामका लागि १६kHz नमूना दर भएका WAV फाइलहरू प्रयोग गर्नुहोस्।

<details>
<summary><h3>Python ट्रयाक</h3></summary>

#### सेटअप

```bash
cd python
python -m venv venv

# भर्चुअल वातावरण सक्रिय गर्नुहोस्:
# विन्डोज (पावरशेल):
venv\Scripts\Activate.ps1
# म्याकओएस:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### ट्रान्सक्रिप्सन कोड

`foundry-local-whisper.py` नामको फाइल बनाउनुहोस्:

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

# चरण 1: बुटस्ट्र्याप - सेवा सुरु गर्छ, डाउनलोड गर्छ, र मोडेल लोड गर्छ
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# क्याच गरिएको ONNX मोडेल फाइलहरूको मार्ग बनाउँछ
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# चरण 2: ONNX सेसनहरू र फिचर एक्स्ट्राक्टर लोड गर्नुहोस्
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

# चरण 3: मेल स्पेक्ट्रोग्राम फिचरहरू निकाल्नुहोस्
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# चरण 4: एन्कोडर चलाउनुहोस्
enc_out = encoder.run(None, {"audio_features": input_features})
# पहिलो आउटपुट लुकेको अवस्थाहरू हुन्; बाँकी क्रस-अटेन्सन KV जोडीहरू हुन्
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# चरण 5: अटोरिग्रेसिभ डिकोडिंग
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, ट्रान्सक्राइब, नोटाइमस्ट्याम्प्स
input_ids = np.array([initial_tokens], dtype=np.int32)

# खाली सेल्फ-अटेन्सन KV क्याच
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

    if next_token == 50257:  # पाठको अन्त्य
        break
    generated.append(next_token)

    # सेल्फ-अटेन्सन KV क्याच अपडेट गर्नुहोस्
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### चलाउनुहोस्

```bash
# एउटा Zava उत्पादन परिदृश्य ट्रान्सक्राइब गर्नुहोस्
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# वा अरू प्रयास गर्नुहोस्:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### मुख्य Python बुँदाहरू

| मेथड | उद्देश्य |
|--------|---------|
| `FoundryLocalManager(alias)` | सुरुआत: सेवा सुरु गर्नु, मोडेल डाउनलोड र लोड गर्नु |
| `manager.get_cache_location()` | क्यास गरिएको ONNX मोडेल फाइलहरूको पथ खोज्नुहोस् |
| `WhisperFeatureExtractor.from_pretrained()` | मेल स्पेक्ट्रोग्राम फिचर एक्स्ट्र्याक्टर लोड गर्नुहोस् |
| `ort.InferenceSession()` | Encoder र Decoder का लागि ONNX Runtime सेसनहरू सिर्जना गर्नुहोस् |
| `tokenizer.decode()` | आउटपुट टोकन ID लाई टेक्स्टमा रूपान्तरण गर्नुहोस् |

</details>

<details>
<summary><h3>JavaScript ट्रयाक</h3></summary>

#### सेटअप

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### ट्रान्सक्रिप्सन कोड

`foundry-local-whisper.mjs` नामको फाइल बनाउनुहोस्:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// चरण १: बुटस्ट्र्याप - प्रबन्धक सिर्जना गर्नुहोस्, सेवा सुरु गर्नुहोस्, र मोडेल लोड गर्नुहोस्
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

// चरण २: एक अडियो क्लाइेन्ट सिर्जना गर्नुहोस् र ट्रान्सक्राइब गर्नुहोस्
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// सफाइ गर्नुहोस्
await model.unload();
```

> **सूचना:** Foundry Local SDK ले `model.createAudioClient()` मार्फत बिल्ट-इन `AudioClient` प्रदान गर्छ जुन सम्पूर्ण ONNX inference पाइपलाइन भित्रै ह्यान्डल गर्छ — कुनै `onnxruntime-node` इम्पोर्ट आवश्यक छैन। अंग्रेजी ट्रान्सक्रिप्सनको लागि `audioClient.settings.language = "en"` सदैव सेट गर्नुहोस्।

#### चलाउनुहोस्

```bash
# एउटा Zava उत्पादन परिदृश्य ट्रान्सक्राइब गर्नुहोस्
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# वा अन्य प्रयास गर्नुहोस्:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### मुख्य JavaScript बुँदाहरू

| मेथड | उद्देश्य |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | व्यवस्थापक singleton सिर्जना गर्नुहोस् |
| `await catalog.getModel(alias)` | क्याटलगबाट मोडेल प्राप्त गर्नुहोस् |
| `model.download()` / `model.load()` | Whisper मोडेल डाउनलोड र लोड गर्नुहोस् |
| `model.createAudioClient()` | ट्रान्सक्रिप्सनको लागि अडियो क्लाइन्ट सिर्जना गर्नुहोस् |
| `audioClient.settings.language = "en"` | ट्रान्सक्रिप्सन भाषाको सेटिङ गर्नुहोस् (सही परिणामको लागि आवश्यक) |
| `audioClient.transcribe(path)` | अडियो फाइल ट्रान्सक्राइब गर्नुहोस्, `{ text, duration }` फर्काउँछ |

</details>

<details>
<summary><h3>C# ट्रयाक</h3></summary>

#### सेटअप

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **सूचना:** C# ट्रयाकले `Microsoft.AI.Foundry.Local` प्याकेज प्रयोग गर्छ जसले `model.GetAudioClientAsync()` मार्फत बिल्ट-इन `AudioClient` प्रदान गर्छ। यसले सम्पूर्ण ट्रान्सक्रिप्सन पाइपलाइनलाई इन-प्रोसेस ह्यान्डल गर्छ — अलग ONNX Runtime सेटअप आवश्यक छैन।

#### ट्रान्सक्रिप्सन कोड

`Program.cs` फाइलको सामग्री यसरी प्रतिस्थापन गर्नुहोस्:

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

#### चलाउनुहोस्

```bash
# Zava उत्पादन परिदृश्य ट्रान्सक्राइब गर्नुहोस्
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# वा अन्य प्रयास गर्नुहोस्:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### मुख्य C# बुँदाहरू

| मेथड | उद्देश्य |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Local कन्फिगरेसनसहित सुरु गर्नुहोस् |
| `catalog.GetModelAsync(alias)` | क्याटलगबाट मोडेल प्राप्त गर्नुहोस् |
| `model.DownloadAsync()` | Whisper मोडेल डाउनलोड गर्नुहोस् |
| `model.GetAudioClientAsync()` | AudioClient प्राप्त गर्नुहोस् (ChatClient होइन!) |
| `audioClient.Settings.Language = "en"` | ट्रान्सक्रिप्सन भाषा सेट गर्नुहोस् (सही परिणामको लागि आवश्यक) |
| `audioClient.TranscribeAudioAsync(path)` | अडियो फाइल ट्रान्सक्राइब गर्नुहोस् |
| `result.Text` | ट्रान्सक्राइब गरिएको पाठ |
> **C# बनाम Python/JS:** C# SDK ले इन-प्रोसेस ट्रान्सक्रिप्शनको लागि बिल्ट-इन `AudioClient` प्रदान गर्दछ जुन `model.GetAudioClientAsync()` मार्फत उपलब्ध हुन्छ, जुन JavaScript SDK सँग समान छ। Python सिधै क्यास गरिएको एन्कोडर/डिकोडर मोडेलहरूमा इन्फरेन्सको लागि ONNX Runtime प्रयोग गर्छ।

</details>

---

### अभ्यास ४ - सबै Zava नमूना फाइलहरू ब्याचमा ट्रान्सक्राइब गर्नुहोस्

अब तपाईं सँग एउटा काम गर्ने ट्रान्सक्रिप्शन एप छ, सबै पाँचवटा Zava नमूना फाइलहरू ट्रान्सक्राइब गर्नुहोस् र नतिजाहरू तुलना गर्नुहोस्।

<details>
<summary><h3>Python ट्र्याक</h3></summary>

पूर्ण नमूना `python/foundry-local-whisper.py` पहिले नै ब्याच ट्रान्सक्रिप्शनलाई समर्थन गर्छ। यसलाई बिना कुनै आर्गुमेन्ट चलाएको बेला, यो `samples/audio/` भित्र सबै `zava-*.wav` फाइलहरू ट्रान्सक्राइब गर्छ:

```bash
cd python
python foundry-local-whisper.py
```

नमूनाले `FoundryLocalManager(alias)` प्रयोग गरेर बुटस्ट्र्याप गर्छ, त्यसपछि प्रत्येक फाइलको लागि एन्कोडर र डिकोडर ONNX सेसनहरू चलाउँछ।

</details>

<details>
<summary><h3>JavaScript ट्र्याक</h3></summary>

पूर्ण नमूना `javascript/foundry-local-whisper.mjs` पहिले नै ब्याच ट्रान्सक्रिप्शनलाई समर्थन गर्छ। यसलाई बिना कुनै आर्गुमेन्ट चलाएको बेला, यो `samples/audio/` भित्र सबै `zava-*.wav` फाइलहरू ट्रान्सक्राइब गर्छ:

```bash
cd javascript
node foundry-local-whisper.mjs
```

नमूनाले SDK सुरु गर्न `FoundryLocalManager.create()` र `catalog.getModel(alias)` प्रयोग गर्छ, त्यसपछि प्रत्येक फाइल ट्रान्सक्राइब गर्न `AudioClient` (सेटिङ्स.language = "en") प्रयोग गर्छ।

</details>

<details>
<summary><h3>C# ट्र्याक</h3></summary>

पूर्ण नमूना `csharp/WhisperTranscription.cs` पहिले नै ब्याच ट्रान्सक्रिप्शनलाई समर्थन गर्छ। कुनै विशिष्ट फाइल आर्गुमेन्ट बिना चलाइएको बेला, यसले `samples/audio/` भित्र सबै `zava-*.wav` फाइलहरू ट्रान्सक्राइब गर्छ:

```bash
cd csharp
dotnet run whisper
```

नमूनाले `FoundryLocalManager.CreateAsync()` र SDK को `AudioClient` (सेटिङ्स.Language = "en") प्रयोग गर्छ इन-प्रोसेस ट्रान्सक्रिप्शनको लागि।

</details>

**के खोज्ने:** ट्रान्सक्रिप्शन परिणामहरूलाई `samples/audio/generate_samples.py` भित्रको मूल पाठसँग तुलना गर्नुहोस्। Whisper कति सटीक रूपमा "Zava ProGrip" जस्ता उत्पादन नामहरू र "brushless motor" वा "composite decking" जस्ता प्राविधिक शब्दहरू समातेको छ?

---

### अभ्यास ५ - प्रमुख कोड ढाँचाहरू बुझ्नुहोस्

तीनवटै भाषामा Whisper ट्रान्सक्रिप्शन कसरी च्याट पूर्णताहरूबाट फरक छ अध्ययन गर्नुहोस्:

<details>
<summary><b>Python - च्याटसँगको मुख्य फरकहरू</b></summary>

```python
# च्याट पूरा गर्ने (भागहरू २-६):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# अडियो ट्रान्सक्रिप्सन (यो भाग):
# OpenAI क्लाइन्टको सट्टा ONNX Runtime सिधै प्रयोग गर्दछ
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... स्वचालित अग्रसरक डिकोडर लूप ...
print(tokenizer.decode(generated_tokens))
```

**मुख्य बुझाइ:** च्याट मोडेलहरू OpenAI-संगत API `manager.endpoint` मार्फत प्रयोग गर्छन्। Whisper ले SDK प्रयोग गरेर क्यास गरिएको ONNX मोडेल फाइलहरू पत्ता लगाउँछ, त्यसपछि सिधै ONNX Runtime मार्फत इन्फरेन्स चलाउँछ।

</details>

<details>
<summary><b>JavaScript - च्याटसँगको मुख्य फरकहरू</b></summary>

```javascript
// च्याट पूर्ति (भागहरू २-६):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// अडियो ट्रान्सक्रिप्शन (यो भाग):
// SDK को बिल्ट-इन AudioClient प्रयोग गर्छ
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // सँधै सबैभन्दा राम्रो नतिजाका लागि भाषा सेट गर्नुहोस्
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**मुख्य बुझाइ:** च्याट मोडेलहरू OpenAI-सँगत API `manager.urls[0] + "/v1"` मार्फत प्रयोग गर्छन्। Whisper ट्रान्सक्रिप्शनले SDK को `AudioClient` प्रयोग गर्छ, जसलाई `model.createAudioClient()` बाट प्राप्त गरिन्छ। स्वतः पत्ता लगाउने समस्याबाट जोगिन `settings.language` सेट गर्नुहोस्।

</details>

<details>
<summary><b>C# - च्याटसँगको मुख्य फरकहरू</b></summary>

C# दृष्टिकोणले SDK को बिल्ट-इन `AudioClient` प्रयोग गर्छ इन-प्रोसेस ट्रान्सक्रिप्शनको लागि:

**मो्डेल प्रारम्भ:**

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

**ट्रान्सक्रिप्शन:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**मुख्य बुझाइ:** C# ले `FoundryLocalManager.CreateAsync()` प्रयोग गर्छ र `AudioClient` सिधै प्राप्त गर्छ — ONNX Runtime सेटअप आवश्यक छैन। स्वतः पत्ता लगाउने समस्याबाट जोगिन `Settings.Language` सेट गर्नुहोस्।

</details>

> **सारांश:** Python ले मोडेल व्यवस्थापनका लागि Foundry Local SDK र एन्कोडर/डिकोडर मोडेलहरूमा सिधा इन्फरेन्सका लागि ONNX Runtime प्रयोग गर्छ। JavaScript र C# दुबै SDK को बिल्ट-इन `AudioClient` प्रयोग गर्छन् जसले ट्रान्सक्रिप्शनलाई सजिलो बनाउँछ — क्लाइन्ट सिर्जना गर्नुहोस्, भाषा सेट गर्नुहोस्, र `transcribe()` / `TranscribeAudioAsync()` कल गर्नुहोस्। सधैं AudioClient को भाषा गुण सेट गर्नुहोस् सटीक नतिजाका लागि।

---

### अभ्यास ६ - प्रयोग गर्नुहोस्

तपाईंको बुझाइ गहिरो बनाउन यी संशोधनहरू प्रयास गर्नुहोस्:

1. **विभिन्न अडियो फाइलहरू प्रयास गर्नुहोस्** - Windows Voice Recorder प्रयोग गरेर आफैंलाई बोल्दाजोधन गरेको अडियो रेकर्ड गर्नुहोस्, WAV मा सुरक्षित गर्नुहोस्, र ट्रान्सक्राइब गर्नुहोस्।

2. **मोडेल भेरियन्टहरू तुलना गर्नुहोस्** - यदि तपाईंसँग NVIDIA GPU छ भने CUDA भेरियन्ट प्रयास गर्नुहोस्:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   ट्रान्सक्रिप्शन गति CPU भेरियन्टसँग तुलना गर्नुहोस्।

3. **आउटपुट स्वरूप थप गर्नुहोस्** - JSON प्रतिक्रिया समावेश गर्न सक्छ:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **REST API तयार गर्नुहोस्** - तपाईंको ट्रान्सक्रिप्शन कोडलाई वेब सर्भरभित्र राख्नुहोस्:

   | भाषा | फ्रेमवर्क | उदाहरण |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` सँग `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` सँग `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` सँग `IFormFile` |

5. **ट्रान्सक्रिप्शनसहित मल्टि-टर्न** - भाग ४ को च्याट एजेन्टसँग Whisper संयोजन गर्नुहोस्: अडियो पहिलो ट्रान्सक्राइब गर्नुहोस्, त्यसपछि पाठलाई एजेन्टलाई विश्लेषण वा संक्षेपणको लागि पास गर्नुहोस्।

---

## SDK अडियो API सन्दर्भ

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — एउटा `AudioClient` इन्स्ट्यान्स सिर्जना गर्छ
> - `audioClient.settings.language` — ट्रान्सक्रिप्शन भाषालाई सेट गर्नुहोस् (जस्तै `"en"`)
> - `audioClient.settings.temperature` — अनियमितता नियन्त्रण (ऐच्छिक)
> - `audioClient.transcribe(filePath)` — फाइल ट्रान्सक्राइब गर्छ, `{ text, duration }` फर्काउँछ
> - `audioClient.transcribeStreaming(filePath, callback)` — कलब्याकमार्फत स्ट्रीम ट्रान्सक्रिप्शन खण्डहरू पठाउँछ
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — एउटा `OpenAIAudioClient` इन्स्ट्यान्स सिर्जना गर्छ
> - `audioClient.Settings.Language` — ट्रान्सक्रिप्शन भाषा सेट गर्नुहोस् (जस्तै `"en"`)
> - `audioClient.Settings.Temperature` — अनियमितता नियन्त्रण (ऐच्छिक)
> - `await audioClient.TranscribeAudioAsync(filePath)` — फाइल ट्रान्सक्राइब गर्छ, `.Text` सहितको वस्तु फर्काउँछ
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — ट्रान्सक्रिप्शन खण्डहरूको `IAsyncEnumerable` फर्काउँछ

> **सूचना:** ट्रान्सक्राइब गर्नुअघि सधैं भाषा गुण सेट गर्नुहोस्। यदि यो नराखियो भने, Whisper मोडेलले स्वतः पत्ता लगाउने प्रयास गर्छ जसले गडबडिएको आउटपुट (टेक्स्टको सट्टा एउटा प्रतिस्थापना चरित्र) उत्पादन गर्न सक्छ।

---

## तुलना: च्याट मोडेलहरू बनाम Whisper

| पक्ष | च्याट मोडेलहरू (भाग ३-७) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **कार्य प्रकार** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **इनपुट** | टेक्स्ट सन्देशहरू (JSON) | अडियो फाइलहरू (WAV/MP3/M4A) | अडियो फाइलहरू (WAV/MP3/M4A) |
| **आउटपुट** | उत्पन्न टेक्स्ट (स्ट्रीम गरिएको) | ट्रान्सक्राइब गरिएको टेक्स्ट (पूरा) | ट्रान्सक्राइब गरिएको टेक्स्ट (पूरा) |
| **SDK प्याकेज** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API तरिका** | `client.chat.completions.create()` | ONNX Runtime सिधा | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **भाषा सेटिङ्ग** | छैन | डिकोडर प्रम्प्ट टोकनहरू | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **स्ट्रीमिङ** | हुन्छ | हुँदैन | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **गोपनीयता लाभ** | कोड/डेटा स्थानीयमै रहन्छ | अडियो डेटा स्थानीयमै रहन्छ | अडियो डेटा स्थानीयमै रहन्छ |

---

## प्रमुख सिकाइहरू

| अवधारणा | तपाईंले के सिक्नुभयो |
|---------|-----------------|
| **Whisper अन-डिभाइस** | स्पीच-टु-टेक्स्ट पूर्ण रूपमा स्थानीय रूपमा चल्छ, Zava ग्राहक कल र उत्पादन समीक्षा अन-डिभाइस ट्रान्सक्राइब गर्न उपयुक्त |
| **SDK AudioClient** | JavaScript र C# SDKs ले बिल्ट-इन `AudioClient` दिन्छ जुन एकै पटकमा पूर्ण ट्रान्सक्रिप्शन पाइपलाइन व्यवस्थापन गर्छ |
| **भाषा सेटिङ्ग** | सधैं AudioClient भाषा सेट गर्नुहोस् (जस्तै `"en"`) — यदि नराखियो भने स्वतः पत्ता लगाउनेले गडबडिएको आउटपुट दिन सक्छ |
| **Python** | मोडेल व्यवस्थापनका लागि `foundry-local-sdk` + ONNX सिधा इन्फरेन्सका लागि `onnxruntime` + `transformers` + `librosa` प्रयोग गर्छ |
| **JavaScript** | `foundry-local-sdk` प्रयोग गरेर `model.createAudioClient()` — `settings.language` सेट गर्नुहोस्, अनि `transcribe()` कल गर्नुहोस् |
| **C#** | `Microsoft.AI.Foundry.Local` प्रयोग गरेर `model.GetAudioClientAsync()` — `Settings.Language` सेट गर्नुहोस्, अनि `TranscribeAudioAsync()` कल गर्नुहोस् |
| **स्ट्रीमिंग समर्थन** | JS र C# SDKs ले `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` पनि प्रदान गर्छन् खण्ड-द्वारा-खण्ड आउटपुटका लागि |
| **CPU-अनुकूलित** | CPU भेरियन्ट (3.05 GB) बिना GPU कुनै पनि Windows उपकरणमा काम गर्छ |
| **गोपनीयता-प्रथम** | Zava ग्राहक अन्तरक्रियाहरू र उत्पादन डाटा अन-डिभाइस राख्नका लागि उत्कृष्ट |

---

## स्रोतहरू

| स्रोत | लिंक |
|----------|------|
| Foundry Local कागजातहरू | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK सन्दर्भ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper मोडेल | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local वेबसाइट | [foundrylocal.ai](https://foundrylocal.ai) |

---

## अर्को चरण

आफ्नो मोडेलहरू Hugging Face बाट कम्पाइल गरेर Foundry Local मार्फत चलाउन [भाग १०: कस्टम वा Hugging Face मोडेलहरू प्रयोग गर्दै](part10-custom-models.md) मा जारी राख्नुहोस्।

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:  
यो दस्तावेज AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) प्रयोग गरी अनुवाद गरिएको हो। हामी सटीकता तर्फ प्रयासरत छौं भने स्वतः अनुवादहरूमा त्रुटि वा अशुद्धता हुनसक्छन् भनी कृपया सचेत हुनुहोस्। मूल भाषा मा रहेको दस्तावेज नै प्रामाणिक स्रोत मान्नुपर्छ। महत्वपूर्ण जानकारीको लागि व्यावसायिक मानव अनुवाद सिफारिस गरिन्छ। यस अनुवाद प्रयोग गर्दा उत्पन्न कुनै पनि गलतफहमी वा गलत व्याख्याका लागि हामी जिम्मेवार होइनौं।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->