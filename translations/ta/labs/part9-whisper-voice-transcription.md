![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# பகுதி 9: Whisper மற்றும் Foundry Local உடன் குரல் உரைமாற்றம்

> **இலக்கு:** Foundry Local மூலம் உள்ளூரில் ஓடுகிறது OpenAI Whisper மாடலைப் பயன்படுத்தி ஒலிக்கோப்புகளை உரைமாற்றம் செய்யவும் - முழுமையாக சாதனத்தில், எவ்வித மேக சேவை தேவையில்லை.

## பார்வை概ம்

Foundry Local என்பது வெறும் உரை உருவாக்கத்துக்காக அல்ல- அது **மொழி முதல் உரை** மாதிரிகளையும் ஆதரிக்கிறது. இந்த பயிற்சித்திட்டத்தில் நீங்கள் **OpenAI Whisper Medium** மாடலை உங்களுக்கு உரிய சாதனத்தில் முழுமையாக ஓட்டி ஒலிக்கோப்புகளை உரைமாற்றம் செய்வீர்கள். இது Zava வாடிக்கையாளர் சேவை அழைப்புகள், பொருள் மதிப்பீடு பதிவுகள், அல்லது பாடசாலை திட்டமிடல் அமர்வுகளுக்கான ஒலி தரவு உங்கள் சாதனத்தை விட்டு வெளியே செல்லக் கூடாத சூழல்களுக்கு மிகவும் பொருத்தமானது.


---

## கற்றல் இலக்குகள்

இந்த பயிற்சித் தொகுப்பை முடிக்க நீங்கள் கீழ்காணும் திறன்களை அடைய முடியும்:

- Whisper மொழி முதல் உரை மாதிரியின் செயல்முறை மற்றும் திறன்களைப் புரிந்துகொள்  
- Foundry Local பயன்படுத்தி Whisper மாதிரியை பதிவிறக்கம் செய்து இயக்கு  
- Python, JavaScript மற்றும் C# ஆகிய மொழிகளில் Foundry Local SDK மூலம் ஒலிக்கோப்புகளை உரைமாற்று  
- முழுமையாக சாதனத்தில் இயங்கும் ஒரு எளிய உரைமாற்ற சேவையை உருவாக்கு  
- Foundry Local இல் உரையாடல்/உரை மாதிரிகள் மற்றும் ஒலி மாதிரிகளுக்கு இடையே உள்ள வேறுபாடுகளை புரிந்துகொள்  

---

## தேவைகள்

| தேவைகள் | விவரங்கள் |
|-------------|---------|
| **Foundry Local CLI** | பதிப்பு **0.8.101 அல்லது அதற்கு மேலானது** (Whisper மாதிரிகள் 0.8.101 முதல் கிடைக்கின்றன) |
| **OS** | Windows 10/11 (x64 அல்லது ARM64) |
| **மொழி இயங்குநிலை** | **Python 3.9+** மற்றும்/ அல்லது **Node.js 18+** மற்றும்/ அல்லது **.NET 9 SDK** ([.NET பதிவிறக்கம்](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **முடிக்கப்பட்டது** | [பகுதி 1: தொடக்கம்](part1-getting-started.md), [பகுதி 2: Foundry Local SDK ஆழமான ஆய்வு](part2-foundry-local-sdk.md), மற்றும் [பகுதி 3: SDK மற்றும் APIகள்](part3-sdk-and-apis.md) |

> **குறிப்பு:** Whisper மாதிரிகள் **SDK** மூலம் பதிவிறக்கம் செய்யப்பட வேண்டும் (CLI மூலம் அல்ல). CLI ஒலி உரைமாற்ற சேவை இயங்குதளத்துக்குப் பாதுகாப்பு இல்லை. உங்கள் பதிப்பை பின்வருமாறு சரிபார்க்கவும்:
> ```bash
> foundry --version
> ```

---

## கருத்து: Whisper Foundry Local உடன் எப்படி செயல்படுகிறது

OpenAI Whisper மாதிரி என்பது பலவகை ஒலிக் குறிப்புக்கள் அடங்கிய பெரிய தொகுப்பில் பயிற்றப்பட்ட பொதுவான பயன்பாட்டுக்கான குரல் அங்கீகார மாதிரி ஆகும். Foundry Local வழியாக ஓடும் போதே:

- இந்த மாதிரி **முழுமையாக உங்கள் CPU லேயே ஓடும்** - GPU தேவையில்லை  
- ஒலி உங்கள் சாதனத்தை விட்டு வெளியே செல்லாது - **முழுமையான தனியுரிமை**  
- Foundry Local SDK மாதிரி பதிவிறக்கம் மற்றும் கேச் நிர்வகிப்பை கையாளும்  
- **JavaScript மற்றும் C#** SDK இல் உள்ள கட்டமைக்கப்பட்ட `AudioClient` முழு உரைமாற்ற குழாய்தணையை கையாளும் — தனியாக ONNX அமைப்பு தேவையில்லை  
- **Python** SDK யைப் பயன்படுத்தி மாதிரி நிர்வகிப்பிற்கும், ONNX Runtime மூலம் குறுக்குவழி/வெளியீட்டு ONNX மாதிரிகள் மீது நேரடி முன்னுணர்வு செய்யும்  

### குழாய்துறை செயல்முறை (JavaScript மற்றும் C#) — SDK AudioClient

1. **Foundry Local SDK** Whisper மாதிரியை பதிவிறக்கம் செய்து கேஷ் செய்கிறது  
2. `model.createAudioClient()` (JS) அல்லது `model.GetAudioClientAsync()` (C#) மூலம் `AudioClient` உருவாக்கப்படுகிறது  
3. `audioClient.transcribe(path)` (JS) அல்லது `audioClient.TranscribeAudioAsync(path)` (C#) முழு குழாய்துறையை உள்ளடக்கி கையாள்கிறது — ஒலி முன்செயலாக்கம், குறுக்குவழியை செயல்படுத்தல், வெளியீட்டு வடிகட்டி மற்றும் டோக்கன் உரையாக்கம்  
4. `AudioClient` இல் உள்ள `settings.language` (ஆங்கிலத்திற்கு `"en"`) சர்வதேச உரைமாற்றத்தை வழிசெய்கிறது  

### குழாய்துறை செயல்முறை (Python) — ONNX Runtime

1. **Foundry Local SDK** Whisper ONNX மாதிரி கோப்புகளை பதிவிறக்கம் செய்து கேஷ் செய்கிறது  
2. **ஒலி முன்செயலாக்கம்** WAV ஒலியைக் கொண்டு 80 மேல் பின்கள் மற்றும் 3000 ஃப்ரேம்கள் கொண்ட மெல் ஸ்பெக்டோகிராமாக மாற்றுகிறது  
3. **குறுக்குவழி செயலி** மெல் ஸ்பெக்டோகிராமை செயலாக்கி மறைப்புச் நிலைகள் மற்றும் குறுக்கு-கவனியம் விசைகள்/மதிப்புகளைக் கொண்ட டென்சர்களை உருவாக்குகிறது  
4. **வெளியீடு வடிகட்டி** தானாக இயக்கப்படுகிறான், ஒரு கீற்று ஒரே நேரத்தில் உருவாக்கி, உரையின் முடிவைக் குறிப்பிடும் டோக்கனை வரையிறுத்தும் வரை  
5. **டோக்கனீசர்** வெளியீட்டு டோக்கன் ஐடியோக்களை திருப்பி வாசிக்கக் கூடிய உரையாக மாற்றுகிறது  

### Whisper மாதிரி வகைகள்

| மாற்றுப்பெயர் | மாதிரி ஐடி | சாதனம் | அளவு | விளக்கம் |
|--------------|------------|--------|-------|----------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU வளைத்த(CUDA) விரைவு |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU சிறப்பாக முன்னோக்கி (பல சாதனங்களுக்கு பரிந்துரைக்கப்படுகிறது) |

> **குறிப்பு:** உரையாடல் மாதிரிகள் இயல்பாக பட்டியலிடுவதற்கு மாறாக, Whisper மாதிரிகள் `automatic-speech-recognition` பணிக்கட்டத்தில் வகைப்படுத்தப்பட்டுள்ளன. விவரங்களுக்கு `foundry model info whisper-medium` பயன்படுத்தவும்.

---

## பயிற்சி பயிற்சிகள்

### பயிற்சி 0 - மாதிரி ஒலி கோப்புகளைப் பெறுக

இந்த பயிற்சியில் முன்கூட்டியே உருவாக்கப்பட்ட WAV கோப்புகள் உள்ளன, அவை Zava DIY தயாரிப்பு நிலைகளுக்கு அடிப்படையாகும். இணைக்கப்பட்ட ஸ்கிரிப்ட்டை இயக்குங்கள்:

```bash
# ரெப்போ ரூட் இலிருந்து - முதலில் ஒரு .venv உருவாக்கி செயல்படுத்தவும்
python -m venv .venv

# விண்டோஸ் (பவர்‌ஷெல்):
.venv\Scripts\Activate.ps1
# மேக்ஓஎஸ்:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

இது `samples/audio/` இல் ஆறு WAV கோப்புகளை உருவாக்கும்:

| கோப்பு | சூழல் |
|--------|---------|
| `zava-customer-inquiry.wav` | **Zava ProGrip Cordless Drill** பற்றி வாடிக்கையாளர் கேள்வி |
| `zava-product-review.wav` | **Zava UltraSmooth Internal Paint** பற்றிய வாடிக்கையாளர் மதிப்பாய்வு |
| `zava-support-call.wav` | **Zava TitanLock Tool Chest** பற்றிய ஆதரவு அழைப்பு |
| `zava-project-planning.wav` | **Zava EcoBoard Composite Decking** கொண்டு டெக் திட்டமிடல் |
| `zava-workshop-setup.wav` | **அனைத்து ஐந்து Zava தயாரிப்புகளும்** உடன் ஒருங்கிணைந்த வேலைப்பாடல் அறை |
| `zava-full-project-walkthrough.wav` | அனைத்து Zava தயாரிப்புகளும் உடன் நீண்ட கால கேரேஜ் புதுப்பிப்பு அறிமுகம் (~4 நிமிடங்கள், நீண்ட ஒலி சோதனைக்கு) |

> **உத்தரம்:** நீங்கள் உங்கள் சொந்த WAV/MP3/M4A கோப்புகளைக் கூட பயன்படுத்தலாம், அல்லது Windows Voice Recorder மூலம் பதிவு செய்யலாம்.

---

### பயிற்சி 1 - SDK பயன்படுத்தி Whisper மாதிரி பதிவிறக்குதல்

நவீன Foundry Local பதிப்புகளில் Whisper மாதிரிகளுடன் CLI பொருந்தாமை காரணமாக, மாதிரியை பதிவிறக்கம் மற்றும் ஏற்ற SDK பயன்படுத்தவும். உங்கள் மொழியை தேர்வுசெய்க:

<details>
<summary><b>🐍 Python</b></summary>

**SDK நிறுவுக:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# சேவையை துவங்குக
manager = FoundryLocalManager()
manager.start_service()

# பட்டியல் தகவலைச் சரிபார்க்கவும்
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# ஏற்கனவே கேஷ் செய்யபட்டதா என்று பொருட்படுத்துக
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# மாதிரியை நினைவகத்தில் ஏற்றுக
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py` என்ற பெயரிடுங்கள் மற்றும் இயக்கவும்:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK நிறுவுக:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// மேலாளரை உருவாக்கி சேவையைத் தொடங்கு
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// பட்டியலிலிருந்து மாதிரியைப் பெறுக
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

// மாதிரியை நினைவகத்தில் ஏற்றுக
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs` எனக் சேமித்து இயக்கவும்:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK நிறுவுக:**
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

> **ஏன் CLI அல்ல SDK?** Foundry Local CLI Whisper மாதிரிகளை நேரடியாக பதிவிறக்கம் செய்யவோ வழங்கவோ முடியாது. SDK க்கு பயன்படுத்தி ஒலி மாதிரிகளை நிர்வகிப்பதால் மிகவும் நம்பகமான முறையாகும். JavaScript மற்றும் C# SDKகள் கட்டமைக்கப்பட்ட `AudioClient` உடன் வரும், இது முழு உரைமாற்ற குழாய்துறையை கையாள்கிறது. Python ஆனால் ONNX Runtime மூலம் கேஷ் செய்யப்பட்ட மாதிரிகளுக்கு நேரடி இறுக்கத்தைச் செய்கிறது.

---

### பயிற்சி 2 - Whisper SDK ஐப் புரிந்துகொள்

Whisper உரைமாற்றம் மொழி அடிப்படையில் மாறுபடும் அணுகுமுறைகளைப் பயன்படுத்துகிறது. **JavaScript மற்றும் C#** கட்டமைக்கப்பட்ட `AudioClient` முழு குழாய்துறையை (ஒலி முன்னெடுத்தல், குறுக்குவழி, வெளியீடு வடிகட்டி, டோக்கன் உரை) ஒரே முறை அழைப்பில் கையாள்கிறது. **Python** Foundry Local SDK மூலம் மாதிரி நிர்வகிப்பதற்கும், ONNX Runtime மூலமான குறுக்குவழி/வெளியீடு ONNX மாதிரிகளுக்கு நேரடி இறுக்கத்திற்கு பயன்படுத்துகிறது.

| கூறு | Python | JavaScript | C# |
|--------|--------|------------|----|
| **SDK தொகுப்புகள்** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **மாதிரி நிர்வகம்** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **வசதி எடுக்கும்** | `WhisperFeatureExtractor` + `librosa` | SDK இல் `AudioClient` கையாள்கிறது | SDK இல் `AudioClient` கையாள்கிறது |
| **ஈடுபாட்டுக் காட்சி** | `ort.InferenceSession` (செயல்படுத்தி + வடிகட்டி) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **டோக்கன் உரை** | `WhisperTokenizer` | SDK இல் `AudioClient` கையாள்கிறது | SDK இல் `AudioClient` கையாள்கிறது |
| **மொழி அமைப்பு** | வடிகட்டி டோக்கன்களில் `forced_ids` மூலம் | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **உள்ளீடு** | WAV கோப்பின் பாதை | WAV கோப்பின் பாதை | WAV கோப்பின் பாதை |
| **வெளியீடு** | உரையாக மறுவிளக்கம் செய்த தொடர் | `result.text` | `result.Text` |

> **முக்கியம்:** எப்போதும் `AudioClient` இல் மொழி அமைக்கவும் (ஆங்கிலத்திற்கு `"en"`). மொழி அமைக்காமல் விட்டால், மாதிரி தானாக மொழியை கண்டறிய முடியவில்லை எனவும், தவறான வெளிப்பாடு ஏற்படலாம்.

> **SDK மாதிரிகள்:** Pythonல் `FoundryLocalManager(alias)` மூலம் துவங்கி பின்னர் `get_cache_location()` மூலம் ONNX மாதிரி கோப்புகளை கண்டுபிடிக்கிறது. JavaScript மற்றும் C# SDK இல் உள்ள `AudioClient` — `model.createAudioClient()` (JS) அல்லது `model.GetAudioClientAsync()` (C#) மூலம் பெறப்படும் — முழு உரைமாற்ற குழாய்துறையை கையாள்கிறது. முழுமையான விவரங்களுக்கு [பகுதி 2: Foundry Local SDK ஆழமான ஆய்வு](part2-foundry-local-sdk.md) பார்க்கவும்.

---

### பயிற்சி 3 - எளிய உரைமாற்ற பயன்பாட்டை உருவாக்குக

உங்கள் மொழியை தேர்வு செய்து ஒரு குறைந்தபட்ச பயன்பாட்டை உருவாக்கி ஒரு ஒலிக்கோப்பை உரைமாற்றம் செய்யவும்.

> **ஆதரிக்கப்படும் ஒலி வடிவங்கள்:** WAV, MP3, M4A. சிறந்த முடிவுக்காக 16kHz மாதிரிக் குறியீடு கொண்ட WAV கோப்புகளைப் பயன்படுத்தவும்.

<details>
<summary><h3>Python மொழி பாதை</h3></summary>

#### தொடக்கம்

```bash
cd python
python -m venv venv

# கற்பனை சூழலை செயல்படுத்துக:
# விண்டோச்கள் (PowerShell):
venv\Scripts\Activate.ps1
# மாக்ஓஎஸ்:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### உரைமாற்றக் குறியீடு

`foundry-local-whisper.py` என்ற கோப்பை உருவாக்குக:

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

# படி 1: பூட்ஸ்டிராப் - சேவையை துவக்குகிறது, பதிவிறக்குகிறது மற்றும் மாடலை ஏற்றுகிறது
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# கெஷ் செய்யப்பட்ட ONNX மாடல் கோப்புகளுக்கான பாதையை கட்டமைக்கவும்
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# படி 2: ONNX அத்தியாயங்களை மற்றும் அம்ச எடுப்பாரை ஏற்று
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

# படி 3: மெல் ஸ்பெக்ட்ரோகிராம் அம்சங்களை எடுக்கவும்
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# படி 4: குறியாக்ப்பு இயந்திரத்தை இயக்கவும்
enc_out = encoder.run(None, {"audio_features": input_features})
# முதல் வெளியீடு மறைந்த நிலைகள்; மீதமுள்ளவை குறுக்கு-கவன KV ஜோடிகள்
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# படி 5: தன்னே தானாக குறியாக்கம் செய்யும் பிரோசஸ்
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, எழுத்துரு மாற்றம், நேர அடையாளங்கள் இல்லை
input_ids = np.array([initial_tokens], dtype=np.int32)

# வெற்று தன்னிகர்வு KV கெஷ்
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

    if next_token == 50257:  # உரையின் முடிவு
        break
    generated.append(next_token)

    # தன்னிகர்வு KV கெஷை புதுப்பிக்கவும்
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### இயக்குக

```bash
# ஒரு Zava தயாரிப்பு காட்சி பதிவேடு செய்க
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# அல்லது மற்றவற்றை முயற்சிக்கவும்:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### முக்கிய Python விருதுகள்

| முறை | நோக்கம் |
|--------|---------|
| `FoundryLocalManager(alias)` | துவக்கம்: சேவை துவக்கல், பதிவிறக்கம் மற்றும் மாதிரி ஏற்றுதல் |
| `manager.get_cache_location()` | கேஷ் செய்யப்பட்ட ONNX மாதிரி கோப்புகளின் பாதையை பெறுதல் |
| `WhisperFeatureExtractor.from_pretrained()` | மெல் ஸ்பெக்டோகிராமின் அம்ச எடுப்பதற்கான கருவி |
| `ort.InferenceSession()` | குறுக்குவழி மற்றும் வடிகட்டி ONNX ரன்னிங் சேஷன்கள் உருவாக்குதல் |
| `tokenizer.decode()` | வெளியீட்டு டோக்கன் ஐடியை உரையை மாற்றுதல் |

</details>

<details>
<summary><h3>JavaScript மொழி பாதை</h3></summary>

#### தொடக்கம்

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### உரைமாற்றக் குறியீடு

`foundry-local-whisper.mjs` என்ற கோப்பை உருவாக்குக:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// படி 1: பூட்ஸ்ட்ராப் - மென்பொருள் மேலாளரை உருவாக்கவும், சேவையை துவக்கவும், மற்றும் மாதிரியை ஏற்றவும்
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

// படி 2: ஒரு ஒலி கிளையண்டை உருவாக்கவும் மற்றும் மாற்றச்செய்க
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// சுத்தம் செய்யவும்
await model.unload();
```

> **குறிப்பு:** Foundry Local SDK இல் உள்ள `model.createAudioClient()` மூலம் கட்டமைக்கப்பட்ட `AudioClient` உள்ளது, இது முழு ONNX இறுக்கக் குழாய்துறையை உள்ளடக்கமே செய்கிறது — `onnxruntime-node` இறக்குமதி தேவையில்லை. சரியான ஆங்கில உரைமாற்றத்திற்காக எப்போதும் `audioClient.settings.language = "en"` என்பதை அமைக்கவும்.

#### இயக்குக

```bash
# ஒரு Zava தயாரிப்பு காட்சியை பிரதானிக்கவும்
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# அல்லது மற்றவற்றைப் பயன்படுத்த முயற்சிக்கவும்:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### முக்கிய JavaScript விருதுகள்

| முறை | நோக்கம் |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | மேலாளர் ஒற்றைக்கோவை உருவாக்குதல் |
| `await catalog.getModel(alias)` | வகைப்பட்டியலிலிருந்து மாதிரி பெறுதல் |
| `model.download()` / `model.load()` | Whisper மாதிரியை பதிவிறக்கி ஏற்றுதல் |
| `model.createAudioClient()` | உரைமாற்றத்திற்கு AudioClient உருவாக்குதல் |
| `audioClient.settings.language = "en"` | உரைமாற்ற மொழி அமைத்தல் (சரியான வெளியீட்டுக்கு அவசியம்) |
| `audioClient.transcribe(path)` | ஒலி கோப்பை உரைமாற்றுகிறான், `{ text, duration }` வழங்குகிறது |

</details>

<details>
<summary><h3>C# மொழி பாதை</h3></summary>

#### தொடக்கம்

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **குறிப்பு:** C# பாதை `Microsoft.AI.Foundry.Local` தொகுப்பைப் பயன்படுத்துகிறது, இது `model.GetAudioClientAsync()` மூலம் கட்டமைக்கப்பட்ட `AudioClient` ஐ வழங்குகிறது. இது ஒரு தனி ONNX Runtime அமைப்பு தேவை இல்லாமல் முழு உரைமாற்ற குழாய்துறையையும் நிர்வகிக்கிறது.

#### உரைமாற்றக் குறியீடு

`Program.cs` உள்ளடக்கத்தை மாற்றுக:

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

#### இயக்குக

```bash
# ஒரு Zava தயாரிப்பு பகுப்பாய்வை எழுதி வையுங்கள்
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# அல்லது மற்றவற்றை முயற்சிக்கவும்:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### முக்கிய C# விருதுகள்

| முறை | நோக்கம் |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Local ஐ கட்டமைப்புடன் தொடங்குக |
| `catalog.GetModelAsync(alias)` | வகைப் பட்டியலிலிருந்து மாதிரி பெறுக |
| `model.DownloadAsync()` | Whisper மாதிரியை பதிவிறக்கவும் |
| `model.GetAudioClientAsync()` | AudioClient ஐ பெறுக (ChatClient அல்ல!) |
| `audioClient.Settings.Language = "en"` | உரைமாற்ற மொழி அமைப்புக் க்கான கட்டளை (சரியான வெளியீட்டுக்கு தேவை) |
| `audioClient.TranscribeAudioAsync(path)` | ஒலி கோப்பை உரைமாற்றும் |
| `result.Text` | உரைமாற்றப்பட்ட உரை |
> **C# vs Python/JS:** C# SDK என்பது `model.GetAudioClientAsync()` மூலம் in-process transcription க்கான இயல்பான `AudioClient` ஐ வழங்குகிறது, இது JavaScript SDK போலவே உள்ளது. Python நேரடியாக ONNX Runtime ஐ பயன்படுத்துகிறது கையிருத்து செய்யப்பட்ட encoder/decoder மாதிரிகள் மீது inference செய்ய.

</details>

---

### Exercise 4 - எல்லா Zava மாதிரிகளையும் தொகுப்பாக படிமொழி மாற்று செய்யவும்

இப்போது உங்களுக்கு செயல்படும் transcription செயலி உள்ளது, ஐந்து Zava மாதிரி கோப்புகளையும் தொகுப்பாக படிமொழி மாற்று செய்து முடிவுகளை ஒப்பிடுங்கள்.

<details>
<summary><h3>Python Track</h3></summary>

முழு மாதிரி `python/foundry-local-whisper.py` தொகுப்பாக transcription ஐ ஆதரிக்கின்றது. எதுவும் அளவுரு இல்லாமல் இயக்கும்போது, அது `samples/audio/` உள்ள அனைத்து `zava-*.wav` கோப்புகளையும் transcription செய்கிறது:

```bash
cd python
python foundry-local-whisper.py
```

இந்த மாதிரி `FoundryLocalManager(alias)` ஐ शुरूசெய்து, அடுத்தடுத்து ஒவ்வொரு கோப்புக்கும் encoder மற்றும் decoder ONNX அமர்வுகளை இயக்குகிறது.

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

முழு மாதிரி `javascript/foundry-local-whisper.mjs` தொகுப்பாக transcription ஐ ஆதரிக்கின்றது. எதுவும் அளவுரு இல்லாமல் இயக்கும்போது, அது `samples/audio/` உள்ள அனைத்து `zava-*.wav` கோப்புகளையும் transcription செய்கிறது:

```bash
cd javascript
node foundry-local-whisper.mjs
```

இந்த மாதிரி `FoundryLocalManager.create()` மற்றும் `catalog.getModel(alias)` ஐ SDK ஐ ஆரம்பிக்க பயன்படுத்துகிறது, பின்னர் ஒவ்வொரு கோப்புக்கும் `AudioClient` (`settings.language = "en"`) ஐ பயன்படுத்தி transcription செய்கிறது.

</details>

<details>
<summary><h3>C# Track</h3></summary>

முழு மாதிரி `csharp/WhisperTranscription.cs` தொகுப்பாக transcription ஐ ஆதரிக்கின்றது. குறிப்பிட்ட கோப்பு அளவுரு இல்லாமல் இயக்கும்போது, அது `samples/audio/` உள்ள அனைத்து `zava-*.wav` கோப்புகளையும் transcription செய்கிறது:

```bash
cd csharp
dotnet run whisper
```

இந்த மாதிரி `FoundryLocalManager.CreateAsync()` மற்றும் SDK இன் `AudioClient` (`Settings.Language = "en"`) ஐ in-process transcription க்காக பயன்படுத்துகிறது.

</details>

**எதை கவனிக்க வேண்டும்:** `samples/audio/generate_samples.py` உள்ள அசல் உரையுடன் transcription வெளியீட்டை ஒப்பிடுங்கள். Whisper "Zava ProGrip" போன்ற தயாரிப்பு பெயர்களையும் "brushless motor" அல்லது "composite decking" போன்ற தொழில்நுட்ப சொற்களையும் எவ்வாறு துல்லியமாக பிடிக்கிறது?

---

### Exercise 5 - முக்கிய குறியீட்டு முறை மற்றும் விதிகள் புரிந்துகொள்ளவும்

Whisper transcription மற்றும் chat completions மைய மொழிகளில் எவ்வாறு வேறுபடுகின்றன என்பதைப் படியுங்கள்:

<details>
<summary><b>Python - Chat இற்கு மாறுபட்ட முக்கிய அம்சங்கள்</b></summary>

```python
# உரையாடல் முடிவு (பகுதிகள் 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# ஒலி உரைமாற்றம் (இந்த பகுதி):
# OpenAI கிளையண்ட் பதிலாக நேரடியாக ONNX Runtime பயன்படுத்துகிறது
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... சுயமரியாதை குறியிடுதல் வட்டம் ...
print(tokenizer.decode(generated_tokens))
```

**முக்கிய விளக்கம்:** Chat மாதிரிகள் OpenAI பொருந்தக்கூடிய API ஐ `manager.endpoint` மூலம் பயன்படுத்துகின்றன. Whisper நேரடியாக ONNX Runtime ஐ பயன்படுத்தி கையிருத்து செய்த ONNX மாதிரி கோப்புகளுக்கு inference செய்கிறது.

</details>

<details>
<summary><b>JavaScript - Chat இற்கு மாறுபட்ட முக்கிய அம்சங்கள்</b></summary>

```javascript
// உரையாடல் நிறைவு (பகுதி 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// ஒலி உரைமாற்றம் (இந்த பகுதி):
// SDKவில் உள்ள AudioClient ஐ பயன்படுத்துகிறது
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // சிறந்த முடிவுகளுக்காக எப்போதும் மொழியை அமைக்கவும்
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**முக்கிய விளக்கம்:** Chat மாதிரிகள் OpenAI பொருந்தக்கூடிய API ஐ `manager.urls[0] + "/v1"` மூலம் பயன்படுத்துகின்றன. Whisper transcription SDK இன் `AudioClient` (`model.createAudioClient()` மூலம் பெறப்படும்) ஐ பயன்படுத்துகிறது. `settings.language` ஐ அமைத்தல் auto-detection மூலம் தவறான வெளியீட்டைத் தவிர்க்க உதவும்.

</details>

<details>
<summary><b>C# - Chat இற்கு மாறுபட்ட முக்கிய அம்சங்கள்</b></summary>

C# முறையில் SDK உடன் வரும் `AudioClient` ஐ in-process transcription க்காக பயன்படுத்துகிறது:

**மாதிரி ஆரம்பித்தல்:**

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

**Transcription:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**முக்கிய விளக்கம்:** C# இல் `FoundryLocalManager.CreateAsync()` மற்றும் நேரடியாக `AudioClient` பெறப்படுகிறது — ONNX Runtime அமைப்புக்கு அவசியமில்லை. தவறான auto-detection வெளியீட்டைத் தவிர்க்க `Settings.Language` அமைக்க வேண்டும்.

</details>

> **சுருக்கம்:** Python மாதிரி மேலாண்மைக்கு Foundry Local SDK மற்றும் ONNX Runtime ஐ நேரடியாக பயன்படுத்துகிறது. JavaScript மற்றும் C# இரண்டும் SDK இன் இயல்பான `AudioClient` ஐ பயன்படுத்தி transcription செய்கின்றன — client உருவாக்கி, மொழியை அமைத்து, `transcribe()` / `TranscribeAudioAsync()` ஐ அழைக்க வேண்டும். AudioClient இல் எப்போதும் மொழி அமைப்பை சீராகத் தேர்வு செய்யுங்கள்.

---

### Exercise 6 - பரிசோதனை செய்யவும்

உங்கள் புரிதலை அதிகரிக்க இந்த மாற்றங்களை முயற்சிக்கவும்:

1. **வேறுபட்ட ஆடியோ கோப்புகளைப் பயன்படுத்துக** - Windows Voice Recorder இல் பேசுவதை பதிவு செய்து WAV ஆக சேமித்து அதைப் படிமொழி மாற்று செய்யவும்

2. **மாதிரி மாறுபாடுகளை ஒப்பிடுக** - NVIDIA GPU இருந்தால் CUDA மாறுபாட்டை முயற்சிக்கவும்:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   CPU மாறுபாட்டுடன் transcription வேகத்தை ஒப்பிட்டு பாருங்கள்.

3. **வெளியீட்டு வடிவமைப்பைச் சேர்க்கவும்** - JSON பதில் இதைப் போன்றவை அடங்கும்:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **REST API உருவாக்குக** - உங்கள் transcription குறியீட்டைக் கோப்பு வலை சேவையகமாக கட்டமைக்கவும்:

   | மொழி | கட்டமைப்பு | உதாரணம் |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` உடன் `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` உடன் `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` உடன் `IFormFile` |

5. **ஒரே நேரத்தில் பல சுற்று உரையாடல் transcription** - மொழிவழங்கலை முதலில் Whisper மூலம் தடிமல் செய்து, பிறகு பகுப்பு அல்லது சுருக்கத்துக்காக chat agent ஐ பயன்படுத்தலாம்.

---

## SDK ஆடியோ API குறிப்பு

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — ஒரு `AudioClient` உருவாக்குகிறது
> - `audioClient.settings.language` — படிமொழி மொழியைக் குறிக்க (எ.கா., `"en"`)
> - `audioClient.settings.temperature` — சீரற்ற தன்மையை கட்டுப்பாடு செய்ய (சோதனை)
> - `audioClient.transcribe(filePath)` — கோப்பை மொழி மாற்று செய்கிறது, `{ text, duration }` வழங்குகிறது
> - `audioClient.transcribeStreaming(filePath, callback)` — callback மூலம் படிமொழி பகுதியான பாகங்களை சீராக வழங்குகிறது
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — ஒரு `OpenAIAudioClient` உருவாக்குகிறது
> - `audioClient.Settings.Language` — படிமொழி மொழி அமைக்க (எ.கா., `"en"`)
> - `audioClient.Settings.Temperature` — சீரற்ற தன்மையை கட்டுப்பாடு செய்ய (சோதனை)
> - `await audioClient.TranscribeAudioAsync(filePath)` — கோப்பை மொழி மாற்று செய்கிறது, `.Text` உடன் பொருள் வழங்குகிறது
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — transcription பகுதி தொகுதிகள் கொண்ட `IAsyncEnumerable` ஐ வழங்குகிறது

> **குறிப்பு:** மொழி பண்பை எப்போதும் அமைக்க வேண்டும். இல்லாவிட்டால், Whisper மாதிரி தானாக கண்டறிதலை முயன்றுவிடும், அதனால் பிழையான அல்லது வேறு எழுத்து முறை வெளிச்சம் செய்யலாம்.

---

## ஒப்பீடு: Chat மாதிரிகள் மற்றும் Whisper

| அம்சம் | Chat மாதிரிகள் (பகுதிகள் 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **பணி வகை** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **உள்ளீடு** | உரை செய்திகள் (JSON) | ஆடியோ கோப்புகள் (WAV/MP3/M4A) | ஆடியோ கோப்புகள் (WAV/MP3/M4A) |
| **வெளியீடு** | உருவாக்கப்பட்ட உரை (தொடர் வெளியீடு) | முழுமையான transcription உரை | முழுமையான transcription உரை |
| **SDK தொகுப்பு** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API முறை** | `client.chat.completions.create()` | ONNX Runtime நேரடி | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **மொழி அமைப்பு** | பொருந்தாது | Decoder prompt tokens | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **தொடர்ச்சி வெளியீடு** | ஆம் | இல்லை | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **தனியுரிமை நன்மை** | குறியீடு/தரவு உள்ளூர் | ஆடியோ தரவு உள்ளூர் | ஆடியோ தரவு உள்ளூர் |

---

## முக்கிய அம்சங்கள்

| கருத்து | நீங்கள் கற்றுக் கொண்டது |
|---------|-----------------|
| **Whisper on-device** | உரையாடலைப் பூரணமாக உள்ளூரேயே இயக்கு, Zava வாடிக்கையாளர் அழைப்புகள் மற்றும் தயாரிப்பு விமர்சனங்களுக்கான சிறந்தது |
| **SDK AudioClient** | JavaScript மற்றும் C# SDK கள் முழுமையான transcription வழிமுறையை ஒரே அழைப்பில் கையாளும் இயல்பான `AudioClient` ஐ வழங்குகின்றன |
| **மொழி அமைப்பு** | AudioClient இல் எப்போதும் மொழி அமைக்க வேண்டும் (எ.கா., `"en"`) - இல்லாவிட்டால் auto-detection மூலம் தவறான வெளியீடு வரலாம் |
| **Python** | மாதிரி மேலாண்மைக்காக `foundry-local-sdk` + `onnxruntime` + `transformers` மற்றும் `librosa` களை நேரடியாக ONNX inference க்காகப் பயன்படுத்துகிறது |
| **JavaScript** | `foundry-local-sdk` உடன் `model.createAudioClient()` வழியாக பயன்படுத்துகிறது — `settings.language` அமைத்து, பிறகு `transcribe()` அழைக்கிறது |
| **C#** | `Microsoft.AI.Foundry.Local` உடன் `model.GetAudioClientAsync()` — `Settings.Language` அமைத்து, பிறகு `TranscribeAudioAsync()` அழைக்கிறது |
| **தொடர்ச்சித் தரம் ஆதரவு** | JS மற்றும் C# SDK கள் chunk-ஆக வெளியீடு பெற `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` வழங்குகின்றன |
| **CPU-சேர்வானது** | CPU மாறுபாடு (3.05 GB) எந்த Windows சாதனத்திலும் GPU இல்லாமல் செயல்படும் |
| **தனியுரிமை முதன்மை** | Zava வாடிக்கையாளர் தகவல் மற்றும் சொத்து உற்பத்தி தரவை உள்ளூரேயே பாதுகாப்பதற்கான சிறந்த தீர்வு |

---

## ஆதாரங்கள்

| ஆதாரம் | இணைப்பு |
|----------|------|
| Foundry Local பதிவுகள் | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK குறிப்பு | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper மாதிரி | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local இணையதளம் | [foundrylocal.ai](https://foundrylocal.ai) |

---

## அடுத்தடி

தொடர்ந்து [பகுதி 10: தனிப்பயன் அல்லது Hugging Face மாதிரிகளைப் பயன்படுத்துதல்](part10-custom-models.md) எனும் பகுதியில் உங்கள் சொந்த மாதிரிகளை Hugging Face இலிருந்து உருவாக்கி Foundry Local வழியாக இயக்கவும்.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**தயாரிப்பு**:  
இந்த ஆவணம் AI மொழிபெயர்ப்பு சேவை [Co-op Translator](https://github.com/Azure/co-op-translator) பயன்படுத்தி மொழிபெயர்க்கப்பட்டுள்ளது. நாங்கள் துல்லியத்துக்கு முயற்சித்தாலும், தானாக செய்யப்பட்ட மொழிபெயர்ப்புகளில் பிழைகள் அல்லது தவறுகள் இருக்கக்கூடும் என்பதை தயவுசெய்து கவனிக்கவும். மொழிப்பூர்வ ஆவணம் அதன் சொந்த மொழியில் அங்கீகாரம் பெற்ற மூலமாக கருதப்பட வேண்டும். முக்கியமான தகவல்களுக்கு, தொழில்துறை மனித மொழிபெயர்ப்பு பரிந்துரைக்கப்படுகிறது. இந்த மொழிபெயர்ப்பின் பயன்பாட்டில் ஏற்பட்ட எந்தவொரு புரிதல் தவறுகளுக்கும் அல்லது தவறான விளக்கங்களுக்கு நாங்கள் பொறுப்பு பெற்றிருக்கமாட்டோம்.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->