![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# အပိုင်း ၉: Whisper နှင့် Foundry Local ဖြင့် အသံစာတမ်းပြုလုပ်ခြင်း

> **ရည်ရွယ်ချက်:** Foundry Local မှတဆင့် ဒေသတွင်းတွင် လည်ပတ်နေသည့် OpenAI Whisper မော်ဒယ်ကို အသုံးပြု၍ အသံဖိုင်များကို စာတမ်းပြုလုပ်ခြင်း — ကွန်ပျူတာပေါ်မှာ အပြည့်အစုံ ဆောင်ရွက်ရန်၊ ကလောင်မလိုအပ်ပါ။

## အကျဉ်းချုပ်

Foundry Local သည် စာသားဖန်တီးခြင်း အတွက်သာမက **အသံမှ စာသားသို့** မော်ဒယ်များကိုလည်း ထောက်ခံပေးသည်။ ဤလက်တွေ့လုပ်ငန်းနှင့်တွင် သင်သည် **OpenAI Whisper Medium** မော်ဒယ်ကို သင့်စက်ပေါ်တွင် လုံးဝ အသုံးပြုကာ အသံဖိုင်များကို စာတမ်းပြုလုပ်မည် ဖြစ်သည်။ ၎င်းသည် Zava ဖောက်သည်ဝန်ဆောင်မှု ခေါ်ဆိုမှုများ၊ ထုတ်ကုန်သုံးသပ်ချက်များ သို့မဟုတ် အလုပ်ရုံဆွေးနွေးပွဲများကဲ့သို့ အသံဒေတာသည် သင့်စက်မှ မထွက်သင့်သော ရှုထောင့်များအတွက် အကောင်းဆုံးဖြစ်သည်။

---

## သင်ယူရမည့် ရည်မှန်းချက်များ

ဤလက်တွေ့လုပ်ငန်းကုန်ဆုံးစဉ်တွင် သင်သည်:

- Whisper အသံမှ စာသားသို့ မော်ဒယ်နှင့် ၎င်း၏ စွမ်းရည်များကို နားလည်နိုင်မည်
- Foundry Local အသုံးပြု၍ Whisper မော်ဒယ်ကို ဒေါင်းလုပ်ဆွဲကာ လည်ပတ်နိုင်မည်
- Foundry Local SDK ကို Python, JavaScript, C# ဖြင့် အသုံးပြုကာ အသံဖိုင်များ ကောက်နုတ်နိုင်မည်
- စက်ပေါ်တွင် လုံးဝ စစ်မှန်သော အသံစာတမ်းဝန်ဆောင်မှုလေးတစ်ခု တည်ဆောက်နိုင်မည်
- Foundry Local တွင် စကားပြော/စာသား မော်ဒယ်များနှင့် အသံမော်ဒယ်များအကြား မတူကွဲပြားချက်များကို နားလည်နိုင်မည်

---

## မတိုင်မီလိုအပ်ချက်များ

| လိုအပ်ချက် | အသေးစိတ် |
|-------------|---------|
| **Foundry Local CLI** | ဗားရှင်း **0.8.101 သို့ အထက်** (Whisper မော်ဒယ်များကို v0.8.101 မှ စတင် ရရှိနိုင်သည်) |
| **OS** | Windows 10/11 (x64 သို့မဟုတ် ARM64) |
| **ဘာသာစကား runtime** | **Python 3.9+** သို့မဟုတ် **Node.js 18+** သို့မဟုတ် **.NET 9 SDK** ([.NET ဒေါင်းလုပ်ဆွဲရန်](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **ပြီးမြောက်ထားမှု** | [အပိုင်း ၁: စတင်အသုံးပြုခြင်း](part1-getting-started.md), [အပိုင်း ၂: Foundry Local SDK နက်နဲစွာလေ့လာခြင်း](part2-foundry-local-sdk.md), နှင့် [အပိုင်း ၃: SDKs နှင့် APIs](part3-sdk-and-apis.md) |

> **မှတ်ချက်:** Whisper မော်ဒယ်များကို **SDK** မှတဆင့် ဒေါင်းလုပ်ဆွဲရမည် (CLI မှ မဟုတ်ပါ)။ CLI သည် အသံစာတမ်းပြုလုပ်မှု endpoint ကို မထောက်ပံ့ပါ။ မိမိရဲ့ ဗားရှင်းကို စစ်ဆေးရန်:
> ```bash
> foundry --version
> ```

---

## အယူအဆ: Whisper သည် Foundry Local နှင့် မည်သို့ လုပ်ဆောင်သနည်း

OpenAI Whisper မော်ဒယ်သည် အသံအမျိုးမျိုးပါသော ဒေတာအစုကြီးဖြင့် လေ့ကျင့်ထားသည့် ပုံမှန်အသံအသိအမှတ်ပြုမော်ဒယ်ဖြစ်သည်။ Foundry Local တွင် လည်ပတ်သောအခါ -

- မော်ဒယ်သည် **သင့် CPU ပေါ်တွင် လုံးဝ လည်ပတ်သည်** - GPU မလိုအပ်ပါ
- အသံသည် သင့်စက်မှ ထွက်သွားခြင်း မရှိပါ - **လုံးဝ ကိုယ်ရေးရာဇဝင်ကာကွယ်မှုရှိသည်**
- Foundry Local SDK သည် မော်ဒယ်ဒေါင်းလုပ်နှင့် cache စီမံခန့်ခွဲမှုကို စီမံပေးသည်
- **JavaScript နှင့် C#** တွင် SDK မှ built-in `AudioClient` ပါရှိကာ အသံစာတမ်းပြုလုပ်စဉ် လုံးဝကို ထိန်းချုပ်ပေးသည် — ONNX ကို မကြိုတင်သတ်မှတ်ရန် မလိုအပ်ပါ
- **Python** သည် မော်ဒယ်စီမံခန့်ခွဲမှုအတွက် SDK ကို အသုံးပြုကာ ONNX Runtime သည် encoder/decoder ONNX မော်ဒယ်များကို တိုကျိုသုံးဆောင်ခြင်း အတွက် အသုံးပြုသည်

### ထောင့်ကွက် အလုပ်လုပ်ပုံ (JavaScript နှင့် C#) — SDK AudioClient

1. **Foundry Local SDK** သည် Whisper မော်ဒယ်ကို ဒေါင်းလုပ်ဆွဲကာ cache ထားသည်
2. `model.createAudioClient()` (JS) သို့မဟုတ် `model.GetAudioClientAsync()` (C#) သည် `AudioClient` ကို ဖန်တီးသည်
3. `audioClient.transcribe(path)` (JS) သို့မဟုတ် `audioClient.TranscribeAudioAsync(path)` (C#) သည် အပြည့်အစုံ pipeline ကို လက်ခံပြီး ဆောင်ရွက်သည် — အသံအကြောင်းအရာ ကြိုတင်ပြင်ဆင်ခြင်း၊ encoder၊ decoder နှင့် token ဖော်ပြခြင်း
4. `AudioClient` တွင် စာသားကျယ်ပြန့်မှု ကိရိယာ `settings.language` (အင်္ဂလိပ်အတွက် `"en"` နှိပ်ထားသည်) ကို တိုက်ရိုက် ထုတ်ပြန်ပေးသည်

### ထောင့်ကွက် အလုပ်လုပ်ပုံ (Python) — ONNX Runtime

1. **Foundry Local SDK** သည် Whisper ONNX မော်ဒယ်ဖိုင်များကို ဒေါင်းလုပ်ဆွဲကာ cache ပြုလုပ်သည်
2. **အသံကြိုတင်ပြင်ဆင်ခြင်း** သည် WAV အသံကို mel spectrogram (80 mel bins x 3000 frames) အသွင်သို့ ပြောင်းလဲသည်
3. **Encoder** သည် mel spectrogram ကို ပြန်လည် ခွဲခြမ်းပြီး hidden states နှင့် cross-attention key/value tensors ကိုထုတ်ပေးသည်
4. **Decoder** သည် autoregressive ဖွဲ့စည်းထားကာ တစ်ခါထုတ်တစ်ကောင် token ထုတ်ပေးသည်၊ end-of-text token ထုတ်ရန်အထိ ဆက်လက်ပြုလုပ်သည်
5. **Tokeniser** သည် ထွက်ရှိလာသော token ID များကို နားလည်ရလွယ်သော စာသားအဖြစ် ပြန်လည်ဖော်ပြပေးသည်

### Whisper မော်ဒယ် မျိုးစိတ်များ

| နာမည် | မော်ဒယ် ID | စက်ပစ္စည်း | အရွယ်အစား | ဖော်ပြချက် |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-မြန်ဆန်စွာ ဆောင်ရွက်မှု (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU အတွက် စနစ်တကျကောင်းမွန်စွာ ပြင်ဆင်ထားသည် (အများဆုံးစက်များအတွက် အကြံပြု) |

> **မှတ်ချက်:** စကားပြော မော်ဒယ်များသည် မူရင်းစာရင်းတွင် ပါဝင်သော်လည်း Whisper မော်ဒယ်များသည် `automatic-speech-recognition` လုပ်ငန်းအမျိုးအစားအောက်တွင် အမျိုးအစားခွဲခြားထားသည်။ အသေးစိတ်အသိပေးရန် `foundry model info whisper-medium` ကို အသုံးပြုပါ။

---

## လက်တွေ့ လေ့ကျင့်ခန်းများ

### လေ့ကျင့်ခန်း ၀ - နမူနာ အသံဖိုင်များ ရယူခြင်း

ဤလက်တွေ့လုပ်ငန်းတွင် Zava DIY ထုတ်ကုန်အခြေအနေကို အခြေချ၍ ပြုစုထားသည့် WAV ဖိုင်များ ပါဝင်သည်။ ပါဝင်သော စာရင်းအစီအစဉ်ကို အသုံးပြုကာ ဖန်တီးပါ:

```bash
# repo root မှာ - ပထမဆုံး .venv ကို ဖန်တီးပြီး အလုပ်ဖြစ်အောင်လုပ်ပါ
python -m venv .venv

# Windows (PowerShell):
.venv\Scripts\Activate.ps1
# macOS:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

ဤသည်သည် `samples/audio/` တွင် WAV ဖိုင် ၆ ဖိုင် ဖန်တီးပေးသည် -

| ဖိုင်အမည် | နမူနာအခြေအနေ |
|------|----------|
| `zava-customer-inquiry.wav` | **Zava ProGrip Cordless Drill** အကြောင်း ဖောက်သည် မေးမြန်းခြင်း |
| `zava-product-review.wav` | **Zava UltraSmooth Interior Paint** အကြောင်းဖောက်သည် ပြန်လည်သုံးသပ်ခြင်း |
| `zava-support-call.wav` | **Zava TitanLock Tool Chest** အကြောင်း အထောက်အပံ့ခေါ်ဆိုမှု |
| `zava-project-planning.wav` | **Zava EcoBoard Composite Decking** ဖြင့် DIYer ပန်းခြံအစီအစဉ် |
| `zava-workshop-setup.wav` | **Zava ထုတ်ကုန်အားလုံး ၅ ခု** အသုံးပြုသည့် အလုပ်ရုံ လမ်းညွှန် |
| `zava-full-project-walkthrough.wav` | **Zava ထုတ်ကုန်အားလုံး** အသုံးပြုသည့် ကျယ်ပြန့်သော ဂယ်ရေး ဆက်လက်ပြုလုပ်ခြင်း (~၄ မိနစ်, အကြာရောင့်အသံ စမ်းသပ်မှုအတွက်) |

> **အကြံပြုချက်:** သင်၏ ကိုယ်ပိုင် WAV/MP3/M4A ဖိုင်များကိုလည်း အသုံးပြုနိုင်ပြီး Windows Voice Recorder ဖြင့် ကိုယ်တိုင် မှတ်တမ်းတင်နိုင်သည်။

---

### လေ့ကျင့်ခန်း ၁ - SDK ကို အသုံးပြုကာ Whisper မော်ဒယ် ဒေါင်းလုပ်ဆွဲခြင်း

Foundry Local အသစ်ဗားရှင်းများတွင် CLI သည် Whisper မော်ဒယ်တွေနဲ့ မကိုက်ညီမှုရှိသဖြင့် မော်ဒယ်ဒေါင်းလုပ်သည် **SDK** မှတဆင့် ပြုလုပ်ပါ။ သင်၏ ဘာသာစကားကို ရွေးချယ်ပါ -

<details>
<summary><b>🐍 Python</b></summary>

**SDK တပ်ဆင်ရန်:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# ဝန်ဆောင်မှုကိုစတင်ပါ
manager = FoundryLocalManager()
manager.start_service()

# ကတ်လုပ် အသေးစိတ်ကိုစစ်ဆေးပါ
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# ရှိပြီးသား ကက်ချ်ထားမှုရှိမရှိ စစ်ဆေးပါ
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# မော်ဒယ်ကို မေမလိုင်ထဲသို့ ပြည့်ထည့်ပါ
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py` အဖြစ် သိမ်းပြီး ပြေးပါ:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK တပ်ဆင်ရန်:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// မန်နေဂျာကို ဖန်တီးပြီး ဝန်ဆောင်မှုကို စတင်ပါ
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ကတ်တလော့မှ မော်ဒယ်ကို ရယူပါ
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

// မော်ဒယ်ကို မemory မှာ တင်ပါ
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs` အဖြစ် သိမ်းပြီး ပြေးပါ:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK တပ်ဆင်ရန်:**
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

> **ဘာကြောင့် CLI မဟုတ်ဘဲ SDK လဲ?** Foundry Local CLI သည် Whisper မော်ဒယ်များကို တိုက်ရိုက် ဒေါင်းလုပ်ဆွဲခြင်း သို့မဟုတ် ရောင်းချခြင်းကို ထောက်ပံ့မှုမပေးပါ။ SDK သည် အသံမော်ဒယ်များကို အစီအစဉ်အတိုင်းနှင့် ယုံကြည်စိတ်ချစွာ စီမံခန့်ခွဲရန် နည်းလမ်းပါ။ JavaScript နှင့် C# SDK တွင် built-in `AudioClient` ပါရှိကာ လုံးဝ အသံစာတမ်းပြုလုပ်မှု pipeline ကို စီမံပေးသည်။ Python သည် ONNX Runtime ကို အသုံးပြုကာ မော်ဒယ်ဖိုင်များပေါ်တွင် တိုက်ရိုက် အကဲဖြတ်သုံးသပ်မှု ပြုလုပ်သည်။

---

### လေ့ကျင့်ခန်း ၂ - Whisper SDK ကို နားလည်ခြင်း

Whisper စာတမ်းပြုလုပ်မှုသည် ဘာသာစကားအလိုက် မတူညီသော နည်းလမ်း အသုံးပြုသည်။ **JavaScript နှင့် C#** တွင် Foundry Local SDK အောက် built-in `AudioClient` ပါရှိကာ တစ်ချက်ခေါ်လိုက်ပါက (အသံကြိုတင်ပြင်ဆင်မှု, encoder, decoder, token ဖော်ထုတ်ခြင်း) စုစည်းစီမံသည်။ **Python** သည် မော်ဒယ်စီမံမှုအတွက် Foundry Local SDK ကို နှင့် encoder/decoder ONNX မော်ဒယ်များအတွက် တိုက်ရိုက် inference အတွက် ONNX Runtime ကို အသုံးပြုသည်။

| ပါဝင်ပစ္စည်း | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK ကောက်ယူမှုများ** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **မော်ဒယ်စီမံခန့်ခွဲမှု** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **လက္ခဏာထုတ်ယူမှု** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` မှ ဆောင်ရွက် | SDK `AudioClient` မှ ဆောင်ရွက် |
| **Inference** | `ort.InferenceSession` (encoder + decoder) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **Token ဖော်ထုတ်ခြင်း** | `WhisperTokenizer` | SDK `AudioClient` မှ ဆောင်ရွက် | SDK `AudioClient` မှ ဆောင်ရွက် |
| **ဘာသာစကားပြောင်းလဲမှု** | decoder tokens တွင် `forced_ids` ဖြင့် သတ်မှတ် | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **Input** | WAV ဖိုင်လမ်းကြောင်း | WAV ဖိုင်လမ်းကြောင်း | WAV ဖိုင်လမ်းကြောင်း |
| **Output** | စာသားပုံသဏ္ဍာန်ပြန်ထုတ် | `result.text` | `result.Text` |

> **အရေးကြီးချက်:** 항상 `AudioClient` တွင် (ဥပမာ `"en"` အင်္ဂလိပ်အတွက်) ဘာသာစကားကို သတ်မှတ်ပါ။ ဘာသာစကား ကို ငြိမ်ငြိမ် မဆိုက်မိလျှင် မော်ဒယ်သည် မမှန်ကန်သော စာသားများ ထုတ်ပေးနိုင်သည်။

> **SDK pattern များ:** Python သည် `FoundryLocalManager(alias)` ဖြင့် စတင်ပြီး `get_cache_location()` မှတဆင့် ONNX မော်ဒယ်ဖိုင်များ ရှာပါက JavaScript နှင့် C# သည် SDK built-in `AudioClient` ကို `model.createAudioClient()` (JS) သို့မဟုတ် `model.GetAudioClientAsync()` (C#) ဖြင့် ရယူ၍ အသံစာတမ်းပြုလုပ်မှု pipeline အားလုံးကို တာဝန်ယူသည်။ စုံလင်သော အသေးစိတ်အတွက် [အပိုင်း ၂: Foundry Local SDK နက်နဲစွာလေ့လာခြင်း](part2-foundry-local-sdk.md) ကို ကြည့်ပါ။

---

### လေ့ကျင့်ခန်း ၃ - တစ်ချက်ခေါ်သုံးသည့် အသံစာတမ်းလိပ်စာ App တည်ဆောက်ခြင်း

သင်၏ ဘာသာစကားလမ်းကြောင်းကို ရွေးချယ်ပြီး အသံဖိုင်တစ်ခုကို စာတမ်းပြုလုပ်သော အနည်းဆုံး အက်ပလီကေးရှင်းကို တည်ဆောက်ပါ။

> **ထောက်ခံသည့် အသံဖိုင်ပုံစံများ:** WAV, MP3, M4A။ အကောင်းဆုံးရလဒ်များအတွက် 16kHz နမူနာနှုန်းရှိသော WAV ဖိုင်များကို အသုံးပြုပါ။

<details>
<summary><h3>Python လမ်းကြောင်း</h3></summary>

#### သတ်မှတ်ခြင်း

```bash
cd python
python -m venv venv

# မျက်မှောက်ပတ်ဝန်းကျင်ကိုဖွင့်ပါ။
# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### စာတမ်းပြုလုပ်မှု ကုဒ်

`foundry-local-whisper.py` ဖိုင် ဖန်တီးပါ:

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

# အဆင့် ၁: Bootstrap - ဝန်ဆောင်မှု စတင် ပြုလုပ်ခြင်း၊ ဒေါင်းလုဒ်လုပ်ခြင်းနှင့် မော်ဒယ်ကို load ပြုလုပ်ခြင်း
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# ONNX မော်ဒယ်ဖိုင်များ cached ဖြစ်နေရာသို့ လမ်းကြောင်း တည်ဆောက်ခြင်း
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# အဆင့် ၂: ONNX sessions နှင့် feature extractor ကို load ပြုလုပ်ခြင်း
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

# အဆင့် ၃: mel spectrogram အင်္ဂါရပ်များ ထုတ်ယူခြင်း
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# အဆင့် ၄: encoder ကို ပြေးဆွဲခြင်း
enc_out = encoder.run(None, {"audio_features": input_features})
# ပထမဆုံး output မှာ hidden states ဖြစ်ပြီး ကျန်ရှိသောများသည် cross-attention KV ရှယ်ယာများဖြစ်သည်
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# အဆင့် ၅: Autoregressive decoding
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, transcribe, notimestamps
input_ids = np.array([initial_tokens], dtype=np.int32)

# စုစည်းထားသော self-attention KV cache ကို လွတ်ငြိမ်းစေခြင်း
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

    if next_token == 50257:  # စာသား၏အဆုံး
        break
    generated.append(next_token)

    # self-attention KV cache ကို update ပြုလုပ်ခြင်း
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### လည်ပတ်ရန်

```bash
# Zava ထုတ်ကုန်အကြောင်းအရာတစ်ခုကို မှတ်တမ်းတင်ပါ
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# ဒါမှမဟုတ် အခြားတွေကို ကြိုးစားကြည့်ပါ:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### Python အချက်အလက်များ

| နည်းလမ်း | အဓိပ္ပါယ် |
|--------|---------|
| `FoundryLocalManager(alias)` | စတင်ခြင်း: ဝန်ဆောင်မှုစတင်ခြင်း၊ ဒေါင်းလုပ်ဆွဲခြင်းနှင့် မော်ဒယ်ကို ဖွင့်ခြင်း |
| `manager.get_cache_location()` | ONNX မော်ဒယ်ဖိုင်များ ကို cache ထဲမှ ဆွဲထုတ်ရန် လမ်းကြောင်း ရယူခြင်း |
| `WhisperFeatureExtractor.from_pretrained()` | mel spectrogram ဖွင့်လှစ်ခြင်း |
| `ort.InferenceSession()` | encoder နှင့် decoder မော်ဒယ်များအတွက် ONNX Runtime session ဖန်တီးခြင်း |
| `tokenizer.decode()` | ထွက်ရှိလာသော token ID များကို စာသားသို့ ပြန်ဖြေဆိုခြင်း |

</details>

<details>
<summary><h3>JavaScript လမ်းကြောင်း</h3></summary>

#### သတ်မှတ်ခြင်း

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### စာတမ်းပြုလုပ်မှု ကုဒ်

`foundry-local-whisper.mjs` ဖိုင် ဖန်တီးပါ:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// အဆင့် ၁: Bootstrap - မန်နေဂျာ ဖန်တီးပါ, ဝန်ဆောင်မှု စတင်ပါ, နှင့် မော်ဒယ်ကို တင်ပါ
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

// အဆင့် ၂: အသံ client တစ်ခု ဖန်တီးပြီး စာ အကြောင်းအရာ ဖော်ပြပါ
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// သန့်ရှင်းရေးလုပ်ဆောင်မှု
await model.unload();
```

> **မှတ်ချက်:** Foundry Local SDK သည် `model.createAudioClient()` မှတဆင့် built-in `AudioClient` ကို ပံ့ပိုးကာ ONNX inference pipeline အားလုံးကို အလိုအလျောက် ထိန်းချုပ်ပေးပါသည် — `onnxruntime-node` ကို သီးခြားတင်ရန် မလိုအပ်ပါ။ အင်္ဂလိပ်စာတမ်းဖတ်မှု မှန်ကန်စေရန် အမြဲ `audioClient.settings.language = "en"` သတ်မှတ်ပါ။

#### လည်ပတ်ရန်

```bash
# Zava ကုန်ပစ္စည်းကြောင်းဇာတ်အိမ်တစ်ခုကို ပြန်ဖြည့်ပါ
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# ဒါမှမဟုတ် အခြားများကို ကြိုးစားပါ:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### JavaScript အချက်အလက်များ

| နည်းလမ်း | အဓိပ္ပါယ် |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | manager singleton ဖန်တီးခြင်း |
| `await catalog.getModel(alias)` | အုပ်စုမှ မော်ဒယ်ရယူခြင်း |
| `model.download()` / `model.load()` | Whisper မော်ဒယ် ဒေါင်းလုပ်ဆွဲခြင်းနှင့် ဖွင့်ခြင်း |
| `model.createAudioClient()` | အသံစာတမ်းအတွက် audio client ဖန်တီးခြင်း |
| `audioClient.settings.language = "en"` | စာတမ်းဖတ်စာဘာသာ သတ်မှတ်ခြင်း (မှန်ကန်မှုအတွက် လိုအပ်သည်) |
| `audioClient.transcribe(path)` | အသံဖိုင်ကို စာတမ်းပြုလုပ်ခြင်း၊ `{ text, duration }` ပြန်ထုတ်ပေးသည် |

</details>

<details>
<summary><h3>C# လမ်းကြောင်း</h3></summary>

#### သတ်မှတ်ခြင်း

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **မှတ်ချက်:** C# လမ်းကြောင်းသည် `Microsoft.AI.Foundry.Local` package ကို အသုံးပြုကာ built-in `AudioClient` ကို `model.GetAudioClientAsync()` ဖြင့် ရရှိရန် ပံ့ပိုးထားသည်။ ၎င်းသည် စာတမ်းပြုလုပ်မှု pipeline အားလုံးကို ပရိုဆက်တွင်းတွင် ဆောင်ရွက်သည် — ONNX Runtime ကို သီးခြား တပ်ဆင်ရန် မလိုအပ်ပါ။

#### စာတမ်းပြုလုပ်မှု ကုဒ်

`Program.cs` အကြောင်းအရာကို ပြောင်းလဲပါ:

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

#### လည်ပတ်ရန်

```bash
# Zava ထုတ်ကုန် အခြေအနေတစ်ခုကို မှတ်တမ်းတင်ပါ
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# မဟုတ်ရင် အခြားများကို စမ်းကြည့်ပါ:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### C# အချက်အလက်များ

| နည်းလမ်း | အဓိပ္ပါယ် |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Local ကို ဖွင့်လှစ်ရန်၊ configuration သတ်မှတ်မှု |
| `catalog.GetModelAsync(alias)` | အုပ်စုမှ မော်ဒယ် ရယူခြင်း |
| `model.DownloadAsync()` | Whisper မော်ဒယ် ဒေါင်းလုပ် ဆွဲခြင်း |
| `model.GetAudioClientAsync()` | AudioClient ရယူခြင်း (ChatClient မဟုတ်ပါ) |
| `audioClient.Settings.Language = "en"` | စာတမ်းဖတ် ဘာသာစကား သတ်မှတ်ခြင်း (မှန်ကန်မှုအတွက် လိုအပ်သည်) |
| `audioClient.TranscribeAudioAsync(path)` | အသံဖိုင် စာတမ်းပြုလုပ်ခြင်း |
| `result.Text` | စာတမ်းပြုလုပ်ပြီး ဝင်ရောက်သော စာသား |
> **C# နှင့် Python/JS:** C# SDK သည် JavaScript SDK နှင့် တူညီသည့် `model.GetAudioClientAsync()` မှတဆင့် in-process transcription အတွက် built-in `AudioClient` ကို ပံ့ပိုးပေးသည်။ Python သည် ONNX Runtime ကိုတိုက်ရိုက် အသုံးပြုပြီး cached encoder/decoder မော်ဒယ်များမှ inference လုပ်သည်။

</details>

---

### လေ့ကျင့်မှု 4 - Zava စမ်းသပ်မှု အားလုံးကို ကြိတ်ကြပ်ဖတ်ခြင်း

ယခု သင်တွင် လုပ်ဆောင်နိုင်သော transcription app ရှိပြီးဖြစ်သောကြောင့်၊ Zava စမ်းသပ်မှု ဖိုင်ငါးခုလုံးကို ကြိတ်ကြပ်ဖတ်ပြီး ရလဒ်များကို နှိုင်းယှဉ်ပါ။

<details>
<summary><h3>Python Track</h3></summary>

`python/foundry-local-whisper.py` စနစ်တကျ batch transcription ကိုပံ့ပိုးပြီးသားဖြစ်သည်။ argument မပါဘဲ run လိုက်ပါက `samples/audio/` တွင်ရှိသော `zava-*.wav` ဖိုင်အားလုံးကို transcription ပြုလုပ်သည်။

```bash
cd python
python foundry-local-whisper.py
```

ဒီနမူနာသည် `FoundryLocalManager(alias)` ကို bootstrap အဖြစ်သုံးပြီး၊ ဖိုင်တိုင်းအတွက် encoder နှင့် decoder ONNX session များကို ရွှေ့လျားလုပ်ဆောင်သည်။

</details>

<details>
<summary><h3>JavaScript Track</h3></summary>

`javascript/foundry-local-whisper.mjs` sample သည် batch transcription ကို support ပြီးသားဖြစ်သည်။ argument မပါချိန်တွင် `samples/audio/` ဖိုင်ထဲရှိ `zava-*.wav` ဖိုင်အားလုံးကို transcription ပြုလုပ်သည်။

```bash
cd javascript
node foundry-local-whisper.mjs
```

ဒီနမူနာသည် `FoundryLocalManager.create()` နှင့် `catalog.getModel(alias)` ကို SDK initialization အတွက်သုံးပြီး၊ ဖိုင်တိုင်းကို transcription ပြုလုပ်ရန် `AudioClient` (settings.language = "en" ဖြစ်) ကိုသုံးသည်။

</details>

<details>
<summary><h3>C# Track</h3></summary>

`csharp/WhisperTranscription.cs` sample သည် batch transcription ကို support ပြီးသားဖြစ်သည်။ file argument မပါဘဲ run လို့ရသောအချိန် `samples/audio/` တွင်ရှိသည့် `zava-*.wav` ဖိုင်အားလုံးကို transcription ပြုလုပ်သည်။

```bash
cd csharp
dotnet run whisper
```

ဒီနမူနာသည် `FoundryLocalManager.CreateAsync()` နှင့် SDK ၏ `AudioClient` (Settings.Language = "en" ဖြစ်စေ) ကို in-process transcription အတွက်အသုံးပြုသည်။

</details>

**ဘာကိုကြည့်ပါမလဲ:** `samples/audio/generate_samples.py` တွင် ပါသော မူရင်းစာသားနှင့် transcription ရလဒ်ကို နှိုင်းယှဥ်ပါ။ Whisper သည် "Zava ProGrip" ကဲ့သို့သော ထုတ်ကုန်အမည်များနှင့် "brushless motor", "composite decking" ကဲ့သို့သော နည်းပညာစကားလုံးများကို မှန်ကန်မှုကနေ သိသာစွာဖော်ပြနိုင်ပါသလား?

---

### လေ့ကျင့်မှု 5 - အရေးပါတဲ့ ကုဒ်ပုံစံများကို နားလည်ခြင်း

Python၊ JavaScript၊ C# ဘာသာလုံးသုံးခု လုံးတွင် Whisper transcription နှင့် chat completion မတူကွဲပြားချက်များကို လေ့လာပါ။

<details>
<summary><b>Python - Chat နှင့် ကွဲပြားချက်များ</b></summary>

```python
# စကားပြောပြီးစီးခြင်း (အစိတ်အပိုင်း 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# အသံစာတမ်းရေးခြင်း (ဤအစိတ်အပိုင်း):
# OpenAI client မသုံးဘဲ ONNX Runtime ကိုတိုက်ရိုက်အသုံးပြုသည်
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... autoregressive decoder loop ...
print(tokenizer.decode(generated_tokens))
```

**အဓိကသိသင့်ချက်:** Chat မော်ဒယ်များသည် OpenAI-compatible API ကို `manager.endpoint` မှတဆင့် အသုံးပြုသည်။ Whisper သည် SDK မှ ONNX မော်ဒယ်ဖိုင်ကွက်ပတ်ကို ရှာဖွေ၍ ONNX Runtime ဖြင့်တိုက်ရိုက် inference လုပ်သည်။

</details>

<details>
<summary><b>JavaScript - Chat နှင့် ကွဲပြားချက်များ</b></summary>

```javascript
// စကားပြောပြီးစီးခြင်း (အပိုင်း ၂-၆):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// အသံ အသံဖတ်ခြင်း (ဤအပိုင်း):
// SDK တွင်ပါဝင်သော AudioClient ကိုအသုံးပြုသည်
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // အကောင်းဆုံးရလဒ်များအတွက် ဘာသာစကားကို အမြဲသတ်မှတ်ပါ
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**အဓိကသိသင့်ချက်:** Chat မော်ဒယ်များသည် `manager.urls[0] + "/v1"` ဖြင့် OpenAI-compatible API ကိုသုံးပြီး၊ Whisper transcription သည် SDK ၏ `AudioClient` ကို `model.createAudioClient()` မှရရှိသည်။ garbled output ဖြစ်မှုမှ ကာကွယ်ရန် `settings.language` ကို သတ်မှတ်ပါ။

</details>

<details>
<summary><b>C# - Chat နှင့် ကွဲပြားချက်များ</b></summary>

C# သည် SDK ၏ built-in `AudioClient` ကို in-process transcription အတွက်သုံးသည်။

**မော်ဒယ် ဒါရိုက်လုပ်ခြင်း:**

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

**အဓိကသိသင့်ချက်:** C# သည် `FoundryLocalManager.CreateAsync()` ကိုသုံးပြီး `AudioClient` ကိုတိုက်ရိုက်ရယူသည် - ONNX Runtime စီစဉ်မှု မလိုအပ်။ garbled output ကာကွယ်ရန် `Settings.Language` ကို သတ်မှတ်ပါ။

</details>

> **အကျဥ်းချုပ်:** Python သည် မော်ဒယ်စီမံခန့်ခွဲမှုအတွက် Foundry Local SDK နှင့် encoder/decoder မော်ဒယ်များပေါ်တွင်တိုက်ရိုက် inference ပြုလုပ်ရန် ONNX Runtime ကိုသုံးသည်။ JavaScript နှင့် C# တို့သည် SDK ၏ built-in `AudioClient` ကိုအသုံးပြုပြီး transcription ကို ရိုးရှင်းစေသည် - client ကို ဖန်တီး၊ language ကို သတ်မှတ်၊ `transcribe()` / `TranscribeAudioAsync()` ကို ခေါ်ဆိုသည်။ transcription မှန်ကန်အောင် AudioClient ၏ language property ကို မမေ့သတ်မှတ်ပါနှင့်။

---

### လေ့ကျင့်မှု 6 - အတွေ့အကြုံများ ပြုလုပ်ခြင်း

သင်၏ အသိပညာ ပိုမိုရှင်းလင်းစေရန် အောက်ပါ ပြင်ဆင်မှုများကို စမ်းသပ်ကြည့်ပါ။

1. **အမျိုးမျိုးသော အသံဖိုင်များစမ်းပါ** - Windows Voice Recorder ဖြင့် မိမိစကားပြောမှုကို မှတ်တမ်းတင်၍ WAV အဖြစ်သိမ်းပြီး transcription ပြုလုပ်ပါ။

2. **မော်ဒယ်အမျိုးအစား မတူများကို နှိုင်းယှဉ်ပါ** - NVIDIA GPU ရှိပါက CUDA variant ကိုစမ်းသပ်ပါ။
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   CPU variant နှင့် transcription အမြန်နှုန်းကို နှိုင်းယှဉ်ပါ။

3. **ထွက်ပေါ်သော Format ကို ထည့်သွင်းပါ** - JSON response တွင် ပါဝင်နိုင်သည်။
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **REST API တည်ဆောက်ပါ** - transcription ကုဒ်ကို web server အတွင်းတွင် wrapper ပြုလုပ်ပါ။

   | ဘာသာစကား | Framework | ဥပမာ |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` နှင့် `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` နှင့် `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` နှင့် `IFormFile` |

5. **Multi-turn နှင့် transcription ပေါင်းစပ်ခြင်း** - Part 4 မှ chat agent နှင့် Whisper ကို ပေါင်းစပ်ပြီး မူလ အသံကို transcription လုပ်ပြီး နောက် text ကို agent သို့ ပေးပို့၍ ခွဲခြမ်းစိတ်ဖြာမှု သို့မဟုတ် အနှစ်ချုပ်ရေးသားမှု လုပ်ပါ။

---

## SDK Audio API ကိုးကားချက်

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — `AudioClient` instance သစ် ဖန်တီးသည်
> - `audioClient.settings.language` — transcription ဘာသာစကား သတ်မှတ်ရန် (ဥပမာ `"en"`)
> - `audioClient.settings.temperature` — randomness ထိန်းချုပ်ရန် (ရွေးချယ်စရာ)
> - `audioClient.transcribe(filePath)` — ဖိုင်စာသား ဖတ်ယူမှု, `{ text, duration }` ပြန်ပေးသည်
> - `audioClient.transcribeStreaming(filePath, callback)` — transcription chunks များကို callback ဖြင့် stream လုပ်ပေးသည်
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — `OpenAIAudioClient` instance ဖန်တီးသည်
> - `audioClient.Settings.Language` — transcription ဘာသာစကား သတ်မှတ်ရန် (ဥပမာ `"en"`)
> - `audioClient.Settings.Temperature` — randomness ထိန်းချုပ်ရန် (ရွေးချယ်စရာ)
> - `await audioClient.TranscribeAudioAsync(filePath)` — ဖိုင်ကို transcription ပြုလုပ်ပြီး `.Text` ၊ ပါသော object ကို ပြန်ပေးသည်
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — transcription chunks များကို IAsyncEnumerable အဖြစ် ပြန်ပေးသည်

> **အကြံပြုချက်:** Transcription မစလုပ်မီ ဘာသာစကား property ကို သတ်မှတ်ပါ။ မရှိပါက Whisper မော်ဒယ်သည် auto-detection လုပ်ပြီး တစ်လုံးသားအစားထိုးအက္ခရာ တစ်လုံး ထွက်နိုင်သည်။

---

## နှိုင်းယှဉ်မှု - Chat မော်ဒယ်များနှင့် Whisper

| သဘောတရား | Chat Models (အပိုင်း ၃-၇) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------|--------------------|--------------------|
| **တာဝန်အမျိုးအစား** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **အဝင်** | စာသားမက်ဆေ့ခ်ျများ (JSON) | အသံဖိုင်များ (WAV/MP3/M4A) | အသံဖိုင်များ (WAV/MP3/M4A) |
| **ထွက်ပေါ်မှု** | ထုတ်လုပ်ထားသော စာသား (streamed) | transcription စာသား (ပြီးပြည့်စုံ) | transcription စာသား (ပြီးပြည့်စုံ) |
| **SDK package** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API နည်းလမ်း** | `client.chat.completions.create()` | ONNX Runtime တိုက်ရိုက် | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **ဘာသာစကား သတ်မှတ်ချက်** | မဟုတ် | Decoder prompt tokens | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **streaming** | ရှိသည် | မရှိ | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **privacy အကျိုး** | ကုဒ်/ဒေတာ နေရာတွင်သာရှိ | အသံဒေတာ နေရာတွင်သာရှိ | အသံဒေတာ နေရာတွင်သာရှိ |

---

## အချက်အလက် အနှစ်ချုပ်

| သဘောတရား | သင် သင်ယူခဲ့သည် |
|---------|-----------------|
| **Whisper on-device** | စကားပြောမှ စာသားထုတ်သဖြင့် ပြိုင်ဘက်များနှင့် လုပ်ငန်း အကောင်းဆုံးဖြစ်စေရန် device အတွင်းhill run ဖို့ သင့်တော်သည် |
| **SDK AudioClient** | JavaScript နှင့် C# SDK များသည် built-in `AudioClient` ကို ပံ့ပိုးပြီး transcription အဆင့် လုံးဝကို တစ်ခါတည်း ကျင့်သုံးသည် |
| **ဘာသာစကား သတ်မှတ်ချက်** | အမြဲ AudioClient ဘာသာစကား (ဥပမာ `"en"`) ကို သတ်မှတ်ပါ - မရှိပါက auto-detection မှ မရိုးရှင်းသော output ပေါ်လာနိုင်သည် |
| **Python** | မော်ဒယ် စီမံခန့်ခွဲမှုအတွက် `foundry-local-sdk` + `onnxruntime` + `transformers` + `librosa` ကိုအသုံးပြုပြီး ONNX ထံတိုက်ရိုက် inference လုပ်သည် |
| **JavaScript** | `foundry-local-sdk` နှင့် `model.createAudioClient()` ကိုသုံး, `settings.language` သတ်မှတ်ပြီး `transcribe()` ကိုခေါ်သည် |
| **C#** | `Microsoft.AI.Foundry.Local` နှင့် `model.GetAudioClientAsync()` ကိုသုံး, `Settings.Language` သတ်မှတ်ပြီး `TranscribeAudioAsync()` ခေါ်သည် |
| **streaming အထောက်အပံ့** | JS နှင့် C# SDK များတွင် `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` ဖြင့် chunk များစွာ သော့ခတ်ထုတ်သည် |
| **CPU-အတြက္အကျိုးရှိသည့်** | CPU variant (3.05 GB) သည် GPU မရှိသော Windows စက်များတွင် အသုံးပြုနိုင်သည် |
| **privacy အထူးဂရုစိုက်မှု** | Zava ၏ ဖောက်သည် ဆက်သွယ်မှုများနှင့် မဟာဗျူဟာ ထုတ်ကုန် ဒေတာများကို device အတွင်းထားရန် အသင့်တော်သည် |

---

## အရင်းအမြစ်များ

| အရင်းအမြစ် | လင့်ခ် |
|----------|------|
| Foundry Local စာမျက်နှာများ | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK ကိုးကားချက် | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper မော်ဒယ် | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local ဝဘ်ဆိုက် | [foundrylocal.ai](https://foundrylocal.ai) |

---

## နောက်တတ်သောအဆင့်

[Part 10: Using Custom or Hugging Face Models](part10-custom-models.md) ကို ချိတ်ဆက်ပြီး Hugging Face မှ မော်ဒယ်များကို မိမိ၏ Foundry Local တွင် ကွန်ပိုင်လုပ်ကြည့်ပါ။

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**အကြောင်းကြားလွှာ**  
ဤစာရွက်စာတမ်းကို AI ဘာသာပြန်ဝန်ဆောင်မှုဖြစ်သည့် [Co-op Translator](https://github.com/Azure/co-op-translator) မှ အသုံးပြု၍ ဘာသာပြန်ထားပါသည်။ တိကျမှုအတွက် ကြိုးပမ်းထားသော်လည်း၊ အလိုအလျောက် ဘာသာပြန်ခြင်းများတွင် အမှားများ သို့မဟုတ် မှန်ကန်မှုမပြည့်စုံမှုများ ပါဝင်နိုင်ကြောင်း သတိပြုရန်လိုအပ်ပါသည်။ မူရင်းစာရွက်စာတမ်းကို သက်ဆိုင်ရာဘာသာစကားဖြင့် အာဏာပိုင်အရင်းအမြစ်အဖြစ် ယူဆသင့်ပါသည်။ အရေးကြီးသော အချက်အလက်များအတွက် လူအလုပ်သမား ပရော်ဖက်ရှင်နယ် ဘာသာပြန်ခြင်းကို အကြံပြုပါသည်။ ဤဘာသာပြန်ချက်ကို အသုံးပြုခြင်းမှ ဖြစ်ပေါ်လာနိုင်သည့် ထင်မြင်မှားယွင်းခြင်းများ သို့မဟုတ် အဓိပ္ပါယ်လျော့နည်းမှုများအတွက် ကျွန်ုပ်တို့သည် တာဝန်မခံပါ။
<!-- CO-OP TRANSLATOR DISCLAIMER END -->