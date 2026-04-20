![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ഭാഗം 9: വിസ്പറില്‍ നിന്നും Foundry Local ഉപയോഗിച്ച് ശബ്‌ദം ട്രാന്‍സ്‌ക്രൈബ് ചെയ്യല്‍

> **ലക്ഷ്യം:** Foundry Local വഴി ആള്‍ക്കാരുടെ ഉപകരണത്തില്‍ പണിചെയ്യുന്ന OpenAI Whisper മോഡല്‍ ഉപയോഗിച്ച് മുഴുവനും ഉപകരണത്തിലാണ് ഓഡിയോ ഫയലുകള്‍ ട്രാന്‍സ്‌ക്രൈബ് ചെയ്യുന്നത് - ക്ലൗഡ് ഒഴിവാക്കിയുള്ളത്.

## അവലോകനം

Foundry Local വെറും ടെക്സ്റ്റ് നിര്‍മ്മാണത്തേക്കല്ല; ഇത് **സ്പീച്ച്-ടു-ടെക്സ്റ്റ്** മോഡലുകളും പിന്തുണയ്ക്കുന്നു. ഈ ലാബില്‍ നിങ്ങൾ ഉപയോഗിക്കുന്നത് **OpenAI Whisper Medium** മോഡലാണ്, നിങ്ങളുടെ കമ്പ്യൂട്ടറിലെ ഓഡിയോ ഫയലുകള്‍ പൂര്‍ണമായും ട്രാന്‍സ്‌ക്രൈബ് ചെയ്യുന്നതിന്. ഇത് Zava കസ്റ്റമര്‍ സര്‍വീസ് കോള്‍ ട്രാന്‍സ്‌ക്രൈബ് ചെയ്യുന്നതിന്, ഉല്‍പ്പന്ന അവലോകന രേഖകള്‍, അല്ലെങ്കില്‍ ഓഡിയോ ഡാറ്റ ഉപകരണത്തിനപ്പുറം പോകരുതെന്ന സാഹചര്യങ്ങളിലുള്ള വര്‍ക്ക്‌ഷോപ്പ് ആസൂത്രണ സെഷനുകള്‍ക്ക് അനുയോജ്യമാണ്.

---

## പഠന ലക്ഷ്യങ്ങള്‍

ഈ ലാബ് പൂര്‍ത്തിയാക്കിയതിന് ശേഷം നിങ്ങള്‍ക്ക് സാധിക്കും:

- Whisper സ്പീച്ച്-ടു-ടെക്സ്റ്റ് മോഡലിന്റെയും അതിന്റെ കഴിവുകളുടെയും ബോധ്യപ്പെടുത്തല്‍
- Foundry Local ഉപയോഗിച്ച് Whisper മോഡല്‍ ഡൗണ്‍ലോഡ് ചെയ്ത് ഓടിക്കല്‍
- Python, JavaScript, C# ഭാഷകളില്‍ Foundry Local SDK ഉപയോഗിച്ച് ഓഡിയോ ഫയലുകള്‍ ട്രാന്‍സ്‌ക്രൈബ് ചെയ്യുക
- പൂര്‍ണമായും ഉപകരണത്തിലേ ഓടുന്ന ലളിതമായ ഒരു ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ സേവനം നിര്‍മ്മിക്കുക
- Foundry Local ന് ഉള്ള ചാറ്റ് / ടെക്സ്റ്റ് മോഡലുകളുടെയും ഓഡിയോ മോഡലുകളുടെയും വ്യത്യാസങ്ങള്‍ മനസിലാക്കുക

---

## മുന്‍വിധികള്‍

| ആവശ്യകത | വിവരങ്ങള്‍ |
|----------|-----------|
| **Foundry Local CLI** | പതിപ്പ് **0.8.101 അല്ലെങ്കില്‍ മുകളില്‍ത്തന്നെ** (Whisper മോഡലുകള്‍ 0.8.101 പതിപ്പില്‍ നിന്നുതന്നെ ലഭ്യമാണ്) |
| **ഓപ്പറേറ്റിങ് സിസ്റ്റം** | Windows 10/11 (x64 അല്ലെങ്കില്‍ ARM64) |
| **ഭാഷരuntime** | **Python 3.9+** അല്ലെങ്കില്‍ **Node.js 18+** അല്ലെങ്കില്‍ **.NET 9 SDK** ([Download .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **പൂര്‍ത്തിയാക്കിയിരിക്കുന്നത്** | [Part 1: Getting Started](part1-getting-started.md), [Part 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md), [Part 3: SDKs and APIs](part3-sdk-and-apis.md) |

> **എത്തി:** Whisper മോഡലുകള്‍ **SDK** വഴി മാത്രമേ ഡൗണ്‍ലോഡ് ചെയ്യേണ്ടതുള്ളു (CLI വഴി സപ്പോര്‍ട്ട് ചെയ്യപ്പെട്ടിട്ടില്ല). CLI(audio transcription endpoint) പിന്തുണയ്ക്കുന്നില്ല. നിങ്ങളുടെ പതിപ്പ് പരിശോധിക്കാന്‍:
> ```bash
> foundry --version
> ```

---

## ആശയം: Whisper Foundry Local നുമായി എങ്ങനെ പ്രവര്‍ത്തിക്കുന്നു

OpenAI Whisper മോഡല്‍ വ്യത്യസ്ത ഓഡിയോ ഡേറ്റാസെറ്റുകള്‍ ഉപയോഗിച്ച് പരിശീലിപ്പിച്ച പൊതുവായ സ്പീച്ച് തിരിച്ചറിയല്‍ മോഡലാണ്. Foundry Local വഴി ഓടുമ്പോള്‍:

- മോഡല്‍ **മുഴുവന്‍ CPU-യില്‍** ഓടും - GPU ആവശ്യമില്ല
- ഓഡിയോ ഒരിക്കലും നിങ്ങളുടെ ഉപകരണത്തിന് പുറത്തേക്ക് പോകയില്ല - **സമ്പൂർണ്ണ സ്വകാര്യത**
- Foundry Local SDK മോഡല്‍ ഡൗണ്‍ലോഡ് ചെയ്തു കാഷെ മാനേജ് ചെയ്യുന്നു
- **JavaScript, C#** SDK-യില്‍ നിര്‍മ്മിച്ചിട്ടുള്ള `AudioClient` ഉപയോഗിച്ച് മുഴുവന്‍ ട്രാന്‍സ്‌ക്രൈപ് പ്രക്രിയ നടത്തുന്നു — മാനുവല്‍ ONNX ക്രമീകരണം ആവശ്യമില്ല
- **Python** SDK മോഡല്‍ മാനേജ്മെന്റിനും, എൻകോഡര്‍/ഡികോഡര്‍ ONNX മോഡലുകള്‍ക്കായി നേരിട്ട് ONNX Runtime ഉപയോഗിക്കുന്നു

### പൈപ്പ്‌ലൈന്‍ എങ്ങനെ പ്രവര്‍ത്തിക്കുന്നു (JavaScript, C#) — SDK AudioClient

1. **Foundry Local SDK** Whisper മോഡല്‍ ഡൗണ്‍ലോഡ് ചെയ്ത് കാഷെ ചെയ്യുന്നു
2. `model.createAudioClient()` (JS) അല്ലെങ്കില്‍ `model.GetAudioClientAsync()` (C#) ഉപയോഗിച്ച് `AudioClient` സൃഷ്ടിക്കുന്നു
3. `audioClient.transcribe(path)` (JS) അല്ലെങ്കില്‍ `audioClient.TranscribeAudioAsync(path)` (C#) മുഴുവന്‍ പൈപ്പ്‌ലൈന്‍ കൈകാര്യം ചെയ്യുന്നു — ഓഡിയോ പ്രീപ്രോസസ്സിംഗ്, എൻകോഡര്‍, ഡികോഡര്‍, ടോകണ്‍ ഡികോഡിങ്
4. `AudioClient`-ന് ഉള്ള `settings.language` പ്രോപ്പര്‍ട്ടി (ഇംഗ്ലീഷിന് `"en"` ആയി സജ്ജമാക്കുക) വ്യക്തമായ ട്രാന്‍സ്‌ക്രിപ്ഷന് ആനുകൂല്യപ്പെടുത്തുന്നു

### പൈപ്പ്‌ലൈന്‍ എങ്ങനെ പ്രവര്‍ത്തിക്കുന്നു (Python) — ONNX Runtime

1. **Foundry Local SDK** Whisper ONNX മോഡല്‍ ഫയലുകള്‍ ഡൗണ്‍ലോഡ് ചെയ്ത് കാഷെ ചെയ്യുന്നു
2. **ഓഡിയോ പ്രീപ്രോസസ്സിംഗ്** WAV ഓഡിയോ മല് സ്പെക്ട്രോഗ്രാം (80 മല് ബിന്‍സ് x 3000 ഫ്രെയിംസ്) ആയി പരിവര്‍ത്തനം ചെയ്യുന്നു
3. **എൻകോഡര്‍** മല് സ്പെക്ട്രോഗ്രാം പ്രോസസ്സ് ചെയ്ത് ഹിഡന്‍ സ്റ്റേറ്റുകള്‍ കൂടാതെ ക്രോസ്-അറ്റൻഷൻ കീ/വാല്യൂ ടെൻസറുകള്‍ ഉണ്ട്
4. **ഡികോഡര്‍** ഓട്ടോ റഗ്രസീവ് ആയി ഒരൊറ്റ ടോകണ്‍ ഒരു തവണയായി ഉണ്ടാക്കി ടാക്‌സ്‌റ്റ് അവസാനം വരുന്ന ടോകണ്‍ വരെ ഊന്നല്‍ നടത്തുന്നു
5. **ടോക്കണൈസര്‍** പുറത്തുവന്ന ടോകണ്‍ ഐഡികള്‍ വായനാസാധ്യമായ ടെക്സ്റ്റിലാക്കി ഡികോഡ് ചെയ്യുന്നു

### Whisper മോഡല്‍ വകഭേദങ്ങള്‍

| അലീസ് | മോഡല്‍ ഐഡി | ഉപകരണം | വലുപ്പം | വിശദീകരണം |
|--------|------------|---------|---------|------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-ആക്‌സിലറേറ്റഡ് (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU സവിശേഷമായ (പല ഉപകരണങ്ങള്‍ക്കുമുദ്ധം) |

> **എത്തി:** സ്വയമേവ ജാതിമാറ്റം ചെയ്യുന്ന ചാറ്റ് മോഡലുകളുടെ വിപരീതമായി, Whisper മോഡലുകള്‍ `automatic-speech-recognition` ടാസ്കില്‍ കാറ്റഗറി ചെയ്തിരിക്കുന്നു. വിശദാംശങ്ങള്‍ കാണാന്‍ `foundry model info whisper-medium` ഉപയോഗിക്കുക.

---

## ലാബ് നഗരങ്ങള്‍

### ആസൂത്രണം 0 - സാമ്പിൾ ഓഡിയോ ഫയലുകള്‍ നേടുക

ഈ ലാബില്‍ Zava DIY ഉല്‍പ്പന്ന സ്‌നാരികമായ മുന്‍കൂര്‍ നിര്‍മ്മിച്ച WAV ഫയലുകള്‍ ഉള്‍പ്പെടുന്നു. ഇതു ഉൾപ്പെടുത്തിയിരിക്കുന്ന സ്ക്രിപ്റ്റ് ഉപയോഗിച്ച് സൃഷ്ടിക്കുക:

```bash
# റീപ്പോ റൂട്ടിൽ നിന്ന് - ആദ്യം ഒരു .venv സൃഷ്ടിച്ച് സജീവമാക്കുക
python -m venv .venv

# വിൻഡോസ് (പവർഷെൽ):
.venv\Scripts\Activate.ps1
# മാക് ഓഎസ്:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

ഇത് `samples/audio/` എന്ന ഫോള്‍ഡറില്‍ ആറു WAV ഫയലുകള്‍ സൃഷ്ടിക്കുന്നു:

| ഫയല്‍ | സന്നിവേശം |
|--------|-----------|
| `zava-customer-inquiry.wav` | **Zava ProGrip Cordless Drill** സംബന്ധിച്ച ഉപഭോക്തൃ അന്വേഷണ ചോദ്യങ്ങള്‍ |
| `zava-product-review.wav` | **Zava UltraSmooth Interior Paint** ഉല്‍പ്പന്ന അവലോകനം |
| `zava-support-call.wav` | **Zava TitanLock Tool Chest** സംബന്ധിച്ച പിന്തുണ കോള്‍ |
| `zava-project-planning.wav` | **Zava EcoBoard Composite Decking** ഉപയോഗിച്ച് DIYര്‍ തയ്യാറാക്കുന്ന ഡെക്ക് പ്ലാനിങ് |
| `zava-workshop-setup.wav` | **എല്ലാ അഞ്ച് Zava ഉല്‍പ്പന്നങ്ങളും** ഉപയോഗിച്ച് വര്‍ക്ക്‌ഷോപ്പ് സജ്ജീകരണം |
| `zava-full-project-walkthrough.wav` | **എല്ലാ Zava ഉല്‍പ്പന്നങ്ങളും** ഉപയോഗിച്ച് വിപുലമായ ഗാരേജ് പുനരുദ്ധാരണ വിശദീകരണം (~4 മിനിറ്റുകള്‍, നീണ്ട ഓഡിയോ പരിശോധനയ്ക്കായി) |

> **ടിപ്പ്:** നിങ്ങളുടെ സ്വന്തം WAV/MP3/M4A ഫയലുകളും ഉപയോഗിക്കാം, അല്ലെങ്കില്‍ Windows Voice Recorder ഉപയോഗിച്ച് സ്വയം റെക്കോര്‍ഡ് ചെയ്യാം.

---

### ആസൂത്രണം 1 - SDK ഉപയോഗിച്ച് Whisper മോഡല്‍ ഡൗണ്‍ലോഡ് ചെയ്യുക

Whisper മോഡലുകളുടെ CLI അനുപയോഗ്യത കാരണം പുതിയ Foundry Local പതിപ്പുകളില്‍, മോഡലിടം ഡൗണ്‍ലോഡ് ചെയ്യാനും ലോഡ് ചെയ്യാനുമുള്ള SDK ഉപയോഗിക്കുക. നിങ്ങളുടെ ഭാഷ തിരഞ്ഞെടുക്കുക:

<details>
<summary><b>🐍 Python</b></summary>

**SDK ഇന്‍സ്റ്റാള്‍ ചെയ്യുക:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# സേവനം ആരംഭിക്കുക
manager = FoundryLocalManager()
manager.start_service()

# കാറ്റലോഗ് വിവരങ്ങൾ പരിശോധിക്കുക
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# ഇതിനകം കാഷ് ചെയ്തിട്ടുണ്ടോ എന്ന് പരിശോധിക്കുക
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# മോഡൽ മെമ്മറിയിൽ ലോഡ് ചെയ്യുക
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py` എന്ന് സംരക്ഷിച്ച് ഓടിക്കുക:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK ഇന്‍സ്റ്റാള്‍ ചെയ്യുക:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// മാനേജർ സൃഷ്‌ടിച്ച് സേർവീസ് തുടങ്ങുക
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ക്യാറ്റലോഗിൽ നിന്നുള്ള മോഡൽ നേടുക
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

// മോഡൽ മെമ്മറിയിലേക്ക് ലോഡ് ചെയ്യുക
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs` എന്ന പേരിൽ സംരക്ഷിച്ച് ഓടിക്കുക:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK ഇന്‍സ്റ്റാള്‍ ചെയ്യുക:**
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

> **എന്തുകൊണ്ട് SDK CLIയുടെ പകരം?** Foundry Local CLI Whisper മോഡലുകള്‍ നേരിട്ട് ഡൗണ്‍ലോഡ് ചെയ്യാനോ സേവനമளിക്കാനോ സപ്പോര്‍ട്ട് ചെയ്യുന്നില്ല. SDK പ്രോഗ്രാമാറ്റിക്കായി ഓഡിയോ മോഡലുകള്‍ എളുപ്പത്തില്‍ ഡൗണ്‍ലോഡ് ചെയ്യാനും നിയന്ത്രിക്കാനും സാധിക്കുന്നു. JavaScript, C# SDKகള്‍ മോഡലിന് ഉള്ള `AudioClient` ഉപയോഗിച്ച് മുഴുവന്‍ ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ പൈപ്പ്‌ലൈന്‍ കൈകാര്യം ചെയ്യുന്നു. Python എങ്കില്‍ SDK മോഡല്‍ മാനേജ്മെന്റും ONNX Runtime നേരിട്ടുള്ള ഇന്‍ഫെറെന്‍സിനുമാണ് ഉപയോഗിക്കുന്നത്.

---

### ആസൂത്രണം 2 - Whisper SDK മനസിലാക്കുക

Whisper ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ ഭാഷനുസരിച്ച് വ്യത്യസ്ത സമീപനങ്ങള്‍ പിന്തുടരുന്നു. **JavaScript, C#** SDK-യില്‍ നിര്‍മ്മിച്ചിട്ടുള്ള `AudioClient` മുഖാന്തിരം മുഴുവന്‍ പൈപ്പ്‌ലൈന്‍ (ഓഡിയോ പ്രീപ്രോസസ്സിംഗ്, എൻകോഡര്‍, ഡികോഡര്‍, ടോകണ്‍ ഡികോഡിംഗ്) ഒരു കോള്‍ വഴി നടക്കുന്നു. **Python** SDK മോഡല്‍ മാനേജുമെന്റിനും, എൻകോഡര്‍/ഡികോഡര്‍ ONNX മോഡലുകളെ ഡൈരക്ട് ഇന്‍ഫെരിക്കുന്നതിനും ONNX Runtime ഉപയോഗിക്കുന്നു.

| ഘടകം | Python | JavaScript | C# |
|--------|---------|------------|----|
| **SDK പാക്കേജുകള്‍** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **മോഡല്‍ മാനേജ്മെന്റ്** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + കാറ്റലോഗ് |
| **പേര്റെര്‍ എക്‌സ്ട്രാക്ഷന്‍** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` കൈകാര്യം ചെയ്യുന്നു | SDK `AudioClient` കൈകാര്യം ചെയ്യുന്നു |
| **ഇന്‍ഫെറെന്‍സ്** | `ort.InferenceSession` (എൻകോഡര്‍, ഡിക്കോഡര്‍) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **ടോകണ്‍ ഡികോഡിംഗ്** | `WhisperTokenizer` | SDK `AudioClient` കൈകാര്യം ചെയ്യുന്നു | SDK `AudioClient` കൈകാര്യം ചെയ്യുന്നു |
| **ഭാഷ ക്രമീകരണം** | ഡികോഡറിലെ `forced_ids` ഉപയോഗിച്ച് സജ്ജമാക്കുക | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **ഇന്‍പുര്‍ട്ട്** | WAV ഫയല്‍ പاتھ് | WAV ഫയല്‍ പاتھ് | WAV ഫയല്‍ പാത്ത് |
| **ഔട്ട്‌പുട്ട്** | ഡികോഡുചെയ്‌ത ടെക്സ്റ്റ് സ്ട്രിംഗ് | `result.text` | `result.Text` |

> **പ്രധാനമാണ്:** `AudioClient`യില്‍ ഭാഷ സജ്ജീകരണം (ഉദാഹരണത്തിന് ഇംഗ്ലീഷിന് `"en"`) നിശ്ചയിക്കേണ്ടതാണ്. ഭാഷ സജ്ജീകരിക്കാതിരിക്കുകയെങ്കില്‍, മോഡല്‍ തെറ്റായ നിലവാരം വരുന്ന ഗാര്‍ബിള്‍ ടെക്സ്റ്റ് പരിച്ഛേദനം ചെയ്യാന്‍ ശ്രമിക്കും.

> **SDK മാതൃകകള്‍:** Python `FoundryLocalManager(alias)` ഉപയോഗിച്ച് ബൂട്ട്‌സ്റ്റ്രാപ്പ് ചെയ്ത്, ശേഷം `get_cache_location()` വഴി ONNX മോഡല്‍ കണ്ടെത്തുന്നു. JavaScript, C# SDK-ല്‍ ഉള്ള ബിൽറ്റ്-ഇൻ `AudioClient` `model.createAudioClient()` (JS) അല്ലെങ്കില്‍ `model.GetAudioClientAsync()` (C#) മുഖേന ലഭിക്കുന്നു - ഇത് മുഴുവന്‍ ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ പൈപ്പ്‌ലൈന്‍ കൈകാര്യം ചെയ്യുന്നു. കൂടുതല്‍ വിവരങ്ങള്‍ക്ക് [Part 2: Foundry Local SDK Deep Dive](part2-foundry-local-sdk.md) കാണുക.

---

### ആസൂത്രണം 3 - ലളിതമായ ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ ആപ്പ് നിര്‍മ്മിക്കുക

നിങ്ങളുടെ ഭാഷ പാത തിരഞ്ഞെടുക്കുക, ഓഡിയോ ഫയല്‍ ടാലുങ് ട്രാന്‍സ്‌ക്രൈബ് ചെയ്യുന്ന ഏറ്റവും ലളിതമായ അപ്ലിക്കേഷന്‍ നിര്‍മ്മിക്കുക.

> **സപ്പോര്‍ട്ട് ചെയ്യുന്ന ഓഡിയോ ഫോര്‍മാറ്റുകള്‍: ** WAV, MP3, M4A. മികച്ച ഫലങ്ങള്‍ക്ക് WAV ഫയലുകളും 16kHz സാമ്പിള്‍ നിരക്കും ഉപയോഗിക്കുക.

<details>
<summary><h3>Python ട്രാക്ക്</h3></summary>

#### സെറ്റ്‌അപ്പ്

```bash
cd python
python -m venv venv

# വെർച്വൽ എൻവയ്റൺമെന്റ് സജീവമാക്കുക:
# വിൻഡോസ് (പവർഷെൽ):
venv\Scripts\Activate.ps1
# മാക്‌ഒഎസ്:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ കോഡ്

`foundry-local-whisper.py` എന്ന ഫയല്‍ സൃഷ്ടിക്കുക:

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

# പടി 1: ബൂട്ട്‌സ്റ്റ്രാപ് - സർവീസ് തുടങ്ങുന്നു, ഡൗൺലോഡ് ചെയ്യുന്നു, മോഡൽ ലോഡ് ചെയ്യുന്നു
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# ക്യാഷ് ചെയ്ത ONNX മോഡൽ ഫയലുകളിലേക്കുള്ള പാത നിർമ്മിക്കുക
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# പടി 2: ONNX സെഷനുകളും ഫീച്ചർ എക്സ്ട്രാക്ടറും ലോഡ് ചെയ്യുക
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

# പടി 3: മെൽ സ്പെക്ട്രോഗ്രാം ഫീച്ചറുകൾ എക്സ്ട്രാക്റ്റ് ചെയ്യുക
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# പടി 4: എൻകോഡർ പ്രവർത്തിപ്പിക്കുക
enc_out = encoder.run(None, {"audio_features": input_features})
# ആദ്യ ഔട്ട്പുട്ട് ഹിഡൻ സ്റ്റേറ്റ്സാണ്; ബാക്കി ക്രോസ്-അറ്റൻഷൻ KV ജോഡികളാണ്
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# പടി 5: ഓട്ടോറെഗ്രസീവ് ഡികോഡിംഗ്
initial_tokens = [50258, 50259, 50359, 50363]  # സോറ്റ്, എൻ, ട്രാൻസ്‌ക്രൈബ്, നോറ്റൈംസ്റ്റാമ്പുകൾ
input_ids = np.array([initial_tokens], dtype=np.int32)

# ശൂന്യ സ്വയം-അറ്റൻഷൻ KV ക്യാഷ്
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

    if next_token == 50257:  # എഴുത്തിന്റെ അവസാനം
        break
    generated.append(next_token)

    # സ്വയം-അറ്റൻഷൻ KV ക്യാഷ് അപ്ഡേറ്റ് ചെയ്യുക
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### ഓടിക്കുക

```bash
# ഒരു സവ പ്രോഡക്ട് സീനാരിയോ ട്രാൻസ്ക്രൈബ് ചെയ്യുക
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# അല്ലെങ്കിൽ മറ്റെവിടെയെങ്കിലും ശ്രമിക്കുക:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### പ്രധാന Python പോയിന്റുകള്‍

| മേതഡ് | ഉദ്ദേശ്യം |
|---------|----------|
| `FoundryLocalManager(alias)` | ബൂട്ട്‌സ്റ്റ്രാപ്പ് ചെയ്യുക: സര്‍വീസ് ആരംഭിക്കുക, ഡൗണ്‍ലോഡ് ചെയ്യുക, മോഡല്‍ ലോഡ് ചെയ്യുക |
| `manager.get_cache_location()` | കാഷെ ചെയ്ത ONNX ഫയലുകളുടെ പാത കണ്ടെത്തുക |
| `WhisperFeatureExtractor.from_pretrained()` | മല് സ്പെക്ട്രോഗ്രാം ഫീച്ചര്‍ എക്‌സ്‌ട്രാക്ടര്‍ ലോഡ് ചെയ്യുക |
| `ort.InferenceSession()` | എൻകോഡര്‍, ഡികോഡര്‍ ONNX Runtime സെഷനുകള്‍ സൃഷ്ടിക്കുക |
| `tokenizer.decode()` | ഔട്ട്പുട്ട് ടോകണ്‍ ഐഡികള്‍ ടെക്സ്റ്റിലാക്കി മാറ്റുക |

</details>

<details>
<summary><h3>JavaScript ട്രാക്ക്</h3></summary>

#### സെറ്റ് അപ്

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ കോഡ്

`foundry-local-whisper.mjs` എന്ന ഫയല്‍ സൃഷ്ടിക്കുക:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// പടി 1: ബൂറ്റ്സ്‌ട്രാപ്പ് - മാനേജർ സൃഷ്‌ടിക്കുക, സേവനം ആരംഭിക്കുക, മോഡൽ ലോഡ് ചെയ്യുക
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

// പടി 2: ഒരു ഓഡിയോ ക്ലയന്റ് സൃഷ്‌ടിച്ച് ട്രാൻസ്ക്രൈബ് ചെയ്യുക
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// ക്ലീന്അപ്പ്
await model.unload();
```

> **എത്തി:** Foundry Local SDK മോഡലില്‍ നിന്ന് നല്‍കുന്ന `model.createAudioClient()` `AudioClient` സാധാരണയായി മുഴുവന്‍ ONNX ഇന്‍ഫറന്‍സ് പൈപ്പ്‌ലൈന്‍ കൈകാര്യം ചെയ്യുന്നു — `onnxruntime-node` ഇംപോര്‍ട് అవసരം ഇല്ല. എപ്പോഴും `audioClient.settings.language = "en"` എന്ന് സജ്ജമാക്കുന്നത് ഇംഗ്ലീഷ് ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ ശരിയായതാക്കും.

#### ഓടിക്കുക

```bash
# ഒരു Zava ഉൽപ്പന്ന സീനarios പ്രകർത്ത്‌ചെയ്യുക
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# അല്ലെങ്കിൽ മറ്റ് ശ്രമിക്കുക:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### പ്രധാന JavaScript പോയിന്റുകള്‍

| മേതഡ് | ഉദ്ദേശ്യം |
|---------|----------|
| `FoundryLocalManager.create({ appName })` | മാനേജര്‍ സിങ്കിളന്‍ സൃഷ്ടിക്കുക |
| `await catalog.getModel(alias)` | കാറ്റലോഗില്‍ നിന്നും മോഡല്‍ കണ്ടെത്തുക |
| `model.download()` / `model.load()` | Whisper മോഡല്‍ ഡൗണ്‍ലോഡ് ചെയ്യുകയും ലോഡ് ചെയ്യുകയും ചെയ്യുക |
| `model.createAudioClient()` | ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ ഓഡിയോക്ലയന്റ് സൃഷ്ടിക്കുക |
| `audioClient.settings.language = "en"` | ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ ഭാഷ സജ്ജമാക്കുക (പോസിറ്റീവ് ഫലത്തിന്)** |
| `audioClient.transcribe(path)` | ഓഡിയോ ഫയല്‍ ട്രാന്‍സ്‌ക്രൈബ് ചെയ്യുക, ഫലം `{ text, duration }` ആയി ലഭിക്കും |

</details>

<details>
<summary><h3>C# ട്രാക്ക്</h3></summary>

#### സെറ്റ് അപ്

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **എത്തി:** C# പഥകം `Microsoft.AI.Foundry.Local` പാക്കേജും `model.GetAudioClientAsync()` മുഖേന ഉറപ്പുവരുത്താവുന്ന `AudioClient` ഉപയോഗിക്കുകയും ചെയ്യുന്നു. ഇത് പൂര്‍ണമായ ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ പൈപ്പ്‌ലൈന്‍ പ്രോസസിനുള്ളില്‍ നടത്തിയതിനാല്‍ വേറെ ONNX Runtime ക്രമീകരണം ആവിശ്യപ്പെടുന്നില്ല.

#### ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ കോഡ്

`Program.cs` ഉള്ളടക്കം മാറ്റുക:

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

#### ഓടിക്കുക

```bash
# ഒരു സാവ പ്രൊഡക്ട് ജീവിതസംഭവം ട്രാൻസ്ക്രൈബ് ചെയ്യുക
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# അല്ലെങ്കിൽ മറ്റ് പരീക്ഷിക്കൂ:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### പ്രധാന C# പോയിന്റുകള്‍

| മേതഡ് | ഉദ്ദേശ്യം |
|---------|----------|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Local സജ്ജമാക്കുക |
| `catalog.GetModelAsync(alias)` | കാറ്റലോഗില്‍ നിന്നും മോഡല്‍ ലഭിക്കുക |
| `model.DownloadAsync()` | Whisper മോഡല്‍ ഡൗണ്‍ലോഡ് ചെയ്യുക |
| `model.GetAudioClientAsync()` | AudioClient ലഭിക്കുക (ChatClient അല്ല!) |
| `audioClient.Settings.Language = "en"` | ട്രാന്‍സ്‌ക്രിപ്ഷന്‍ ഭാഷ സജ്ജമാക്കുക (ശുദ്ധമായ ഫലത്തിന് നിർബന്ധം) |
| `audioClient.TranscribeAudioAsync(path)` | ഓഡിയോ ഫയല്‍ ട്രാന്‍സ്‌ക്രൈബ് ചെയ്യുക |
| `result.Text` | ട്രാന്‍സ്‌ക്രൈബ് ചെയ്ത ടെക്സ്റ്റ് |
> **C# vs Python/JS:** C# SDK `model.GetAudioClientAsync()` വഴി in-process ട്രാൻസ്ക്രിപ്ഷനിന് ബിൽറ്റ്-ഇൻ `AudioClient` നൽകുന്നു, ജാവാസ്ക്രിപ്റ്റ് SDK പോലെ. Python ക്യാഷ് ചെയ്ത എൻകോഡർ/ഡീകോഡർ മോഡലുകൾക്കെതിരെ നേരിട്ട് ഇൻഫറൻസ് നടത്താൻ ONNX Runtime ഉപയോഗിക്കുന്നു.

</details>

---

### വ്യായാമം 4 - ഒറ്റത്തവണ എല്ലാ Zava സാമ്പிளുകൾ ട്രാൻസ്ക്രൈബ് ചെയ്യുക

പ്രവർത്തനക്ഷമമായ ഒരു ട്രാൻസ്ക്രിപ്ഷൻ ആപ്പ് എങ്കിൽ, എല്ലാ അഞ്ച് Zava സാമ്പിള് ഫയലുകളും ട്രാൻസ്ക്രൈബ് ചെയ്യുകയും ഫലങ്ങൾ താരതമ്യം ചെയ്യുകയും ചെയ്യുക.

<details>
<summary><h3>Python ട്രാക്ക്</h3></summary>

പൂർണ്ണ സാമ്പിൾ `python/foundry-local-whisper.py` ഇതിനകം ബാച്ച് ട്രാൻസ്ക്രിപ്ഷൻ പിന്തുണക്‌ക്കുന്നുണ്ട്. ആർഗുമെന്റുകൾ ഇല്ലാതെ പ്രവർത്തിപ്പിച്ചാൽ, `samples/audio/` ൽ ഉപയോഗിച്ചുള്ള എല്ലാ `zava-*.wav` ഫയലുകളും ട്രാൻസ്ക്രൈബ് ചെയ്യും:

```bash
cd python
python foundry-local-whisper.py
```

സാമ്പിൾ `FoundryLocalManager(alias)` ഉപയോഗിച്ച് ബൂട്ട്സ്‌ട്രാപ്പ് ചെയ്യുന്നു, തുടർന്ന് ഓരോ ഫയലിനും എൻകോഡർ, ഡീകോഡർ ONNX സെഷനുകൾ ഓടിക്കുന്നു.

</details>

<details>
<summary><h3>JavaScript ട്രാക്ക്</h3></summary>

പൂർണ്ണ സാമ്പിൾ `javascript/foundry-local-whisper.mjs` ഇതിനകം ബാച്ച് ട്രാൻസ്ക്രിപ്ഷൻ പിന്തുണക്‌ക്കുന്നുണ്ട്. ആർഗുമെന്റുകൾ ഇല്ലാതെ പ്രവർത്തിപ്പിച്ചാൽ, `samples/audio/` ൽ ഉപയോഗിച്ചുള്ള എല്ലാ `zava-*.wav` ഫയലുകളും ട്രാൻസ്ക്രൈബ് നടത്തുന്നു:

```bash
cd javascript
node foundry-local-whisper.mjs
```

സാമ്പിൾ `FoundryLocalManager.create()`യും `catalog.getModel(alias)`ഉം ഉപയോഗിച്ച് SDK പ്രാരംഭമാക്കുന്നു, ശേഷം `AudioClient` (with `settings.language = "en"`) ഉപയോഗിച്ച് ഓരോ ഫയലും ട്രാൻസ്ക്രൈബ് ചെയ്യുന്നു.

</details>

<details>
<summary><h3>C# ട്രാക്ക്</h3></summary>

പൂർണ്ണ സാമ്പിൾ `csharp/WhisperTranscription.cs` ഇതിനകം ബാച്ച് ട്രാൻസ്ക്രിപ്ഷൻ പിന്തുണക്‌ക്കുന്നുണ്ട്. ഒരു പ്രത്യേക ഫയൽ ആർഗുമെന്റ് ഇല്ലാതെ പ്രവർത്തിച്ചാൽ, `samples/audio/` ൽ ഉള്ള എല്ലാ `zava-*.wav` ഫയലുകളും ട്രാൻസ്ക്രൈബ് ചെയ്യുന്നു:

```bash
cd csharp
dotnet run whisper
```

സാമ്പിൾ `FoundryLocalManager.CreateAsync()`ഉം SDKയുടെ `AudioClient`(with `Settings.Language = "en"`)ഉം in-process ട്രാൻസ്ക്രിപ്ഷനു വേണ്ടി ഉപയോഗിക്കുന്നു.

</details>

**കോടിയിടാൻ:** ട്രാൻസ്ക്രിപ്ഷൻ ഔട്ട്‌പുട്ട് `samples/audio/generate_samples.py`യിലെ യഥാർത്ഥ ടെക്സ്റ്റുമായി താരതമ്യം ചെയ്യുക. "Zava ProGrip" പോലുള്ള ഉൽപ്പന്ന നാമങ്ങളും "brushless motor" അല്ലെങ്കിൽ "composite decking" പോലുള്ള സാങ്കേതിക പദങ്ങളും Whisper എത്രമാത്രം കൃത്യമായി പിടിച്ച്‌വെക്കുന്നു?

---

### വ്യായാമം 5 - മുഖ്യ കോഡ് പാറ്റേണുകൾ മനസ്സിലാക്കുക

Whisper ട്രാൻസ്ക്രിപ്ഷനും ചാറ്റ് പൂർത്തീകരണങ്ങളും മൂന്ന് ഭാഷകളിലും എങ്ങനെ വ്യത്യസ്തമാണെന്ന് പഠിക്കൂ:

<details>
<summary><b>Python - ചാറ്റിൽനിന്നുള്ള മുഖ്യ വ്യത്യാസങ്ങൾ</b></summary>

```python
# ചാറ്റ് പൂർത്തിയാക്കൽ (ഭാഗങ്ങൾ 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# ഓഡിയോ ട്രാൻസ്ക്രിപ്പ് (ഈ ഭാഗം):
# OpenAI ക്ലയന്റ് yerine നേരിട്ട് ONNX റൺടൈം ഉപയോഗിക്കുന്നു
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... ഓട്ടോറെഗ്രസീവ് ഡികോഡർ ലൂപ്പ് ...
print(tokenizer.decode(generated_tokens))
```

**പ്രധാന മനസ്സിലാക്കൽ:** Chat മോഡലുകൾ OpenAI-കോംപാറ്റിബിൾ API `manager.endpoint` വഴിയാണ് ഉപയോഗിക്കുന്നത്. Whisper SDK ഉപയോഗിച്ച് ക്യാഷ് ചെയ്ത ONNX മോഡൽ ഫയലുകൾ കണ്ടെത്തി, ONNX Runtime നേരിട്ട് ഇൻഫറൻസ് നടത്തുന്നു.

</details>

<details>
<summary><b>JavaScript - ചാറ്റിൽനിന്നുള്ള മുഖ്യ വ്യത്യാസങ്ങൾ</b></summary>

```javascript
// ചാറ്റ് പൂർത്തീകരണം (ഭാഗങ്ങൾ 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// ഓഡിയോ തർജ്ജമ (ഈ ഭാഗം):
// SDK-യുടെ ഇൻബിൽറ്റ് ഓഡിയോക്ലയന്റ് ഉപയോഗിക്കുന്നു
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // മികച്ച ഫലങ്ങൾക്കായി 항상 ഭാഷ സജ്ജമാക്കുക
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**പ്രധാന മനസ്സിലാക്കൽ:** Chat മോഡലുകൾ OpenAI-കോംപാറ്റിബിൾ API `manager.urls[0] + "/v1"` വഴി ഉപയോഗിക്കുന്നു. Whisper ട്രാൻസ്ക്രിപ്ഷൻ SDK-യിലെ `AudioClient` ഉപയോഗിച്ച്, ഇത് `model.createAudioClient()` വഴി ലഭിക്കുന്നു. ഓട്ടോ-ഡിറ്റക്ഷൻ മൂലം തെറ്റായ ഔട്ട്പുട്ട് ഒഴിവാക്കാൻ `settings.language` സജ്ജമാക്കുക.

</details>

<details>
<summary><b>C# - ചാറ്റിൽനിന്നുള്ള മുഖ്യ വ്യത്യാസങ്ങൾ</b></summary>

C# മാർഗ്ഗം SDK ന്റെ ബിൽറ്റ്-ഇൻ `AudioClient` ഉപയോഗിച്ച് in-process ട്രാൻസ്ക്രിപ്ഷൻ നടത്തുന്നു:

**മോഡൽ പ്രാരംഭമാക്കൽ:**

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

**ട്രാൻസ്ക്രിപ്ഷൻ:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**പ്രധാന മനസ്സിലാക്കൽ:** C# `FoundryLocalManager.CreateAsync()` ഉപയോഗിച്ച് നേരിട്ട് `AudioClient` ലഭിക്കുന്നു — ONNX Runtime സെറ്റപ്പ് ആവശ്യമില്ല. ഓട്ടോ-ഡിറ്റക്ഷൻ മൂലം തെറ്റ് ഒഴിവാക്കാൻ `Settings.Language` സജ്ജമാക്കുക.

</details>

> **സംഗ്രഹം:** Python മോഡൽ മാനേജ്മെന്റിനും ONNX Runtime നേരിട്ടുള്ള ഇൻഫറൻസിനുമായി Foundry Local SDK ഉപയോഗിക്കുന്നു. JavaScript-നും C#-നും SDK ന്റെ ബിൽറ്റ്-ഇൻ `AudioClient` ഉപയോഗിച്ച് ലളിതമായ ട്രാൻസ്ക്രിപ്ഷൻ നടത്തുന്നു — ക്ലയന്റ് സൃഷ്ടിച്ച് ഭാഷ സജ്ജമാക്കി, പിന്നാലെ `transcribe()` / `TranscribeAudioAsync()` വിളിക്കുക. സവിശേഷമായ ഫലങ്ങൾക്ക് AudioClient ലെ language പ്രോപ്പർട്ടി എപ്പോഴും സജ്ജീകരിക്കുക.

---

### വ്യായാമം 6 - പരീക്ഷണം

നിങ്ങളുടെ അറിവ് കൂടുതൽ വിപുലീകരിക്കാൻ ഈ മാറ്റങ്ങൾ പരീക്ഷിക്കൂ:

1. **വ്യത്യസ്ഥ ശബ്ദ ഫയലുകൾ പരീക്ഷിക്കുക** - Windows Voice Recorder ഉപയോഗിച്ച് സ്വയം സംസാരിച്ച് റെക്കോർഡ് ചെയ്ത് WAV ആയി സേവ് ചെയ്ത് ട്രാൻസ്ക്രൈബ് ചെയ്യുക

2. **മോഡൽ വേരിയന്റുകൾ താരതമ്യം ചെയ്യുക** - NVIDIA GPU ഉണ്ടെങ്കിൽ CUDA വേരിയന്റ് പരീക്ഷിക്കുക:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   CPU വേരിയന്റിനുള്ളിൽ താരതമ്യം ചെയ്യുക ട്രാൻസ്ക്രിപ്ഷൻ വേഗത.

3. **ഔട്ട്‌പുട് ഫോർമാറ്റിംഗ് ചേർക്കുക** - JSON റെസ്പോൺസ് ഇതിനകം ഉൾക്കൊള്ളാം:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **REST API നിർമ്മിക്കുക** - ട്രാൻസ്ക്രിപ്ഷൻ കോഡ് ഒരു വെബ് സെർവറിൽ ഇട്ടുകൊണ്ട്:

   | ഭാഷ | ഫ്രെയിംവർക് | ഉദാഹരണം |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` with `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` with `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` with `IFormFile` |

5. **ട്രാൻസ്ക്രിപ്ഷൻ ഉപയോഗിച്ചുളള മൾട്ടി-ടേൺ** - Whisper ൽ നിന്നുള്ള ടെക്സ്റ്റ് ആദ്യം ട്രാൻസ്ക്രിപ്ഷൻ ചെയ്ത്, പിന്നെ അത് ഒരു ചാറ്റ് ഏജന്റിനു നൽകുക വിശകലനത്തിനോ സംഗ്രഹത്തിനോ.

---

## SDK ഓഡിയോ API റഫറൻസ്

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — ഒരു `AudioClient` ഇൻസ്റ്റൻസ് സൃഷ്ടിക്കുന്നു
> - `audioClient.settings.language` — ട്രാൻസ്ക്രിപ്ഷൻ ഭാഷ സജ്ജീകരിക്കുക (ഉദാ: `"en"`)
> - `audioClient.settings.temperature` — റാന്റംനസ് നിയന്ത്രിക്കുക (ഐഷണികം)
> - `audioClient.transcribe(filePath)` — ഒരു ഫയൽ ട്രാൻസ്ക്രൈബ് ചെയ്യുക, `{ text, duration }` എണ്ണം നൽകുന്നു
> - `audioClient.transcribeStreaming(filePath, callback)` — ട്രാൻസ്ക്രിപ്ഷൻ ചെങ്കുകൾ callback വഴി സ്‌ട്രീം ചെയ്യുക
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — ഒരു `OpenAIAudioClient` ഇൻസ്റ്റൻസ് സൃഷ്ടിക്കുന്നു
> - `audioClient.Settings.Language` — ട്രാൻസ്ക്രിപ്ഷൻ ഭാഷ സജ്ജീകരിക്കുക (ഉദാ: `"en"`)
> - `audioClient.Settings.Temperature` — റാന്റംനസ് നിയന്ത്രിക്കുക (ഐഷണികം)
> - `await audioClient.TranscribeAudioAsync(filePath)` — ഒരു ഫയൽ ട്രാൻസ്ക്രൈബ് ചെയ്യുക, `.Text` ഉള്ള ഒബ്ജക്റ്റ് നൽകുന്നു
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — ട്രാൻസ്ക്രിപ്ഷൻ ചെങ്കുകളുടെ `IAsyncEnumerable` നൽകുന്നു

> **ടിപ്പ്:** ട്രാൻസ്ക്രൈബ് ചെയ്യുന്നതിന് മുമ്പ് എപ്പോഴും ഭാഷ പ്രോപ്പർട്ടി സജ്ജമാക്കുക. അത് ഇല്ലെങ്കിൽ Whisper മോഡൽ ഓട്ടോ-ഡിറ്റക്ഷൻ നടത്തുന്നു, ഇത് തെറ്റായ ഔട്ട്പുട്ട് (പദംമാറ്റം ചെയ്ത ഒരു പ്രതീകം മാത്രം) പുരോഗമിക്കും.

---

## താരതമ്യം: ചാറ്റ് മോഡലുകൾ vs. Whisper

| ഘടകം | ചാറ്റ് മോഡലുകൾ (പാർട്ടുകൾ 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|--------------------------|--------------------|--------------------|
| **ടാസ്‌ക് തരം** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **ഇൻപുട്ട്** | ടെക്‌സ്‌റ്റ് മെസേജുകൾ (JSON) | ഓഡിയോ ഫയലുകൾ (WAV/MP3/M4A) | ഓഡിയോ ഫയലുകൾ (WAV/MP3/M4A) |
| **ഔട്ട്പുട്ട്** | ജനിത ടെക്സ്റ്റ് (സ്റ്റ്രീമഡ്) | ട്രാൻസ്ക്രൈബ് ചെയ്ത ടെക്സ്റ്റ് (പൂർണ്ണം) | ട്രാൻസ്ക്രൈബ് ചെയ്ത ടെക്സ്റ്റ് (പൂർണ്ണം) |
| **SDK പാക്കേജ്** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API മെത്തഡ്** | `client.chat.completions.create()` | ONNX Runtime നേരിട്ട് | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **ഭാഷ സജ്ജീകരണം** | പ്രയോഗിക്കപ്പെടുന്നില്ല | ഡീകോഡർ പ്രോംപ്റ്റ് ടോകൺ | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **സ്റ്റ്രീമിംഗ്** | ഉണ്ട് | ഇല്ല | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **സ്വകാര്യതാ ലാഭം** | കോഡ്/ഡാറ്റ ലോക്കലിൽ തുടരുന്നു | ഓഡിയോ ഡാറ്റ ലോക്കലിൽ തന്നെ തുടരുന്നു | ഓഡിയോ ഡാറ്റ ലോക്കലിൽ തന്നെ തുടരുന്നു |

---

## പ്രധാന കാര്യങ്ങള്‍

| ആശയം | നിങ്ങൾക്കാർന്നത് |
|---------|-----------------|
| **ഡിവൈസിൽ തന്നെ Whisper** | സ്പീച്ച്-ടു-ടെക്സ്റ്റ് പൂർണ്ണമായും ലോക്കലായ് പ്രവർത്തിക്കുന്നു, Zava കസ്റ്റമർ കോളുകളും ഉൽപ്പന്ന അവലോകനങ്ങളും on-device ട്രാൻസ്ക്രൈബ് ചെയ്യാൻ ഏറ്റവും ഉചിതം |
| **SDK AudioClient** | ജാവാസ്ക്രിപ്റ്റും C# SDKകളും ബിൽറ്റ്-ഇൻ `AudioClient` നൽകുന്നു, ഇതിലൂടെ ഒരു വിളിക്കലിൽ മുഴുവൻ ട്രാൻസ്ക്രിപ്ഷൻ പൈപ്പ്‌లൈൻ കൈകാര്യം ചെയ്യുന്നു |
| **ഭാഷ സജ്ജീകരണം** | എപ്പോഴും AudioClient ഭാഷ സജ്ജമാക്കുക (ഉദാ: `"en"`) — അത് ഇല്ലെങ്കിൽ ഓട്ടോ-ഡിറ്റക്ഷൻ തെറ്റായ ഔട്ട്പുട്ട് നൽകാം |
| **Python** | മോഡൽ മാനേജ്മെന്റിനും ഇതിനൊപ്പം `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` ഉപയോഗിച്ച് നേരിട്ട് ONNX ഇൻഫറൻസ് നടത്തുന്നു |
| **JavaScript** | `foundry-local-sdk` ഉപയോഗിച്ച് `model.createAudioClient()` — `settings.language` സജ്ജമാക്കി പിന്നീട് `transcribe()` വിളിക്കുന്നു |
| **C#** | `Microsoft.AI.Foundry.Local` ഉപയോഗിച്ച് `model.GetAudioClientAsync()` — `Settings.Language` സജ്ജമാക്കി ശേഷം `TranscribeAudioAsync()` വിളിക്കുന്നു |
| **സ്റ്റ്രീമിംഗ് പിന്തുണ** | JS, C# SDKകൾക്കും `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` Chunk-ചെക്ക് ഔട്ട്‌പുട്ടിനായി ഉണ്ട് |
| **CPU-ഓപ്റ്റിമൈസ്ഡ്** | CPU വേരിയന്റ് (3.05 GB) GPU ഇല്ലാത്ത Windows ഉപകരണങ്ങളിലും പ്രവർത്തിക്കും |
| **സ്വകാര്യത മുന്നിൽ** | Zava കസ്റ്റമർ ഇടപാടുകളും സ്വകാര്യ ഉൽപ്പന്ന ഡാറ്റയും ഡിവൈസിൽ തന്നെ സൂക്ഷിക്കാൻ ഉത്തമം |

---

## വിഭവങ്ങൾ

| വിഭവം | ലിങ്ക് |
|----------|------|
| Foundry Local ഡോക്‌സ് | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK റഫറൻസ് | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper മോഡൽ | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local വെബ്സൈറ്റ് | [foundrylocal.ai](https://foundrylocal.ai) |

---

## അടുത്ത ഘട്ടം

[Part 10: Using Custom or Hugging Face Models](part10-custom-models.md) എന്ന ഭാഗത്തേക്ക് തുടരുക, Hugging Face മodel ൽ നിന്നും നിങ്ങൾക്കാവശ്യമായ മോഡലുകൾ തയാറാക്കി Foundry Local ഉപയോഗിച്ച് ഓടിക്കുക.