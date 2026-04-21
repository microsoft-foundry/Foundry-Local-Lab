# மாற்றங்களுக்கான பட்டியல் — Foundry உள்ளூர் பணிமனை

இந்த பணிமனையின் அனைத்து குறிப்பிடத்தகுந்த மாற்றங்களும் கீழே பதிவுசெய்யப்பட்டுள்ளன.

---

## 2026-03-11 — பகுதி 12 & 13, வலை UI, விஸ்பர் மறுபடம், WinML/QNN திருத்தம் மற்றும் சரிபார்ப்பு

### சேர்க்கப்பட்டது
- **பகுதி 12: Zava Creative Writer க்கான வலை UI உருவாக்குதல்** — புதிய பயிற்சி வழிகாட்டி (`labs/part12-zava-ui.md`) அதன் உட்பட்ட பயிற்சிகளுடன், NDJSON நேரடியாக ஓட்டம், உலாவி `ReadableStream`, நேரடி முகவர் நிலை சார்புக்கள் மற்றும் நேரடி கட்டுரை உரை ஓற்றம்
- **பகுதி 13: பணிமனை முடிந்தது** — புதிய சுருக்க பயிற்சி (`labs/part13-workshop-complete.md`) அனைத்து 12 பகுதிகளின் மீள்பார்வை, மேலதிக யோசனைகள் மற்றும் வள இணைப்புகள்
- **Zava UI முன்னணி:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — மூன்று பின்கையால் எட்டு வந்த பசும்பான HTML/CSS/JS உலாவி இடைமுகம்
- **JavaScript HTTP சேவையகம்:** `zava-creative-writer-local/src/javascript/server.mjs` — இலங்கை இயக்குநர் பாதுகூற்றான பிரவுசர் அடிப்படையிலான அணுகலுக்கு புதிய Express-பாணி HTTP சேவையகம்
- **C# ASP.NET கோர் பின்கை:** `zava-creative-writer-local/src/csharp-web/Program.cs` மற்றும் `ZavaCreativeWriterWeb.csproj` — UI மற்றும் ஸ்ட்ரீமிங் NDJSON க்காக சிறிய API திட்டம்
- **ஒலி மாதிரி உருவாக்கி:** `samples/audio/generate_samples.py` — ஆஃஃ லைன் TTS ஸ்கிரிப்ட் `pyttsx3` பயன்படுத்தி பகுதி 9 க்கு Zava கருப்பொருள் WAV கோப்புகளை உருவாக்குகிறது
- **ஒலி மாதிரி:** `samples/audio/zava-full-project-walkthrough.wav` — வடிவமைப்புப் சோதனைக்கு புதிய நீளம் அதிகமான ஒலி மாதிரி
- **சரிபார்ப்பு ஸ்கிரிப்ட்:** `validate-npu-workaround.ps1` — அனைத்து C# மாதிரிகளிலும் NPU/QNN வழிமுறையை சரிபார்க்கும் தானாக இயங்கும் PowerShell ஸ்கிரிப்ட்
- **Mermaid வரைபட SVG-கள்:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML குறுக்கு மேகளின் ஆதரவு:** அனைத்து 3 C# `.csproj` கோப்புகள் (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) இப்போது தொடர்பான TFM மற்றும் ஒரே நேரத்தில் இயற்கையாக இணைப்பட்ட प्यாகேஜ் குறிகளுக்கான ஆதரவை பயன்படுத்துகின்றன. விண்டோஸில்: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (QNN EP பொதிகை சேர்க்கை). விண்டோஸுக்கே அல்லாதது: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (அடி SDK). Zava திட்டங்களில் நிரந்தரமாக இருந்த `win-arm64` RID தானாக கண்டுபிடிப்பிற்கு மாற்றப்பட்டது. ஒரு தொற்று சார்பு வழிமுறை native சொத்துகளை `Microsoft.ML.OnnxRuntime.Gpu.Linux` இலிருந்து விலக்கினது, இது பராமரிப்பு பிழை கொண்ட win-arm64 குறிப்பை கொண்டது. கடந்த try/catch NPU வழிமுறை அனைத்து 7 C# கோப்புகளிலிருந்தும் நீக்கப்பட்டது.

### மாற்றப்பட்டது
- **பகுதி 9 (விஸ்பர்):** பெரிய மறுபடம் — JavaScript இப்போது SDK இன் உட்பிரவேசமான `AudioClient` (`model.createAudioClient()`) பயன்படுத்துகிறது; பழைய ONNX Runtime செயல் முடிவாளத்தை மாற்றியது; JS/C# `AudioClient` அணுகல் மற்றும் Python ONNX Runtime அணுகல் மாற்றங்கள் விளக்கப்பட்டுள்ளன
- **பகுதி 11:** நெவிகேஷன் இணைப்புகள் புதுப்பிக்கப்பட்டன (தற்போது பகுதி 12 க்கு குறிக்கின்றன); கருவி அழைப்பு ஓட்டம் மற்றும் தொடர் SVG வரைபடங்கள் சேர்க்கப்பட்டன
- **பகுதி 10:** வழிசெலுத்தல் புதுப்பிக்கப்பட்டு பணிமனை முடிதல் இடம் தவிர்த்து பகுதி 12 வழியாக வழியேற்றப்பெற்றுள்ளது
- **Python Whisper (`foundry-local-whisper.py`):** கூடுதல் ஒலி மாதிரிகள் மற்றும் மேம்பட்ட பிழை கையாளல் சேர்க்கப்பட்டன
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** பழைய ONNX Runtime அமர்வுகள் இல்லாமல், `model.createAudioClient()` மற்றும் `audioClient.transcribe()` பயன்படுத்தி மறுபடியும் எழுதப்பட்டது
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** API உடன் static UI கோப்புகளும் வழங்கப்படுவதற்காக புதுப்பிக்கப்பட்டது
- **Zava C# console (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU வழிமுறை அகற்றப்பட்டது (இப்போது WinML பேக்கேஜால் கையாளப்படுகிறது)
- **README.md:** பகுதி 12 பகுதிக்கான குறியீட்டு அட்டவணைகள் மற்றும் பின்கை சேர்க்கைகள்; பகுதி 13 பகுதிகள்; கற்றல் குறிக்கோள்கள் மற்றும் திட்ட அமைப்பு புதுப்பிக்கப்பட்டது
- **KNOWN-ISSUES.md:** தீர்வடைந்த #7 (C# SDK NPU மாதிரி வகை) நீக்கப்பட்டது (இப்போது WinML பேக்கேஜால் கையாளப்படுகிறது). மீதமுள்ள பிரச்சினைகள் #1–#6 என்ற எண்ணிக்கையுடன் புதுப்பிக்கப்பட்டன. .NET SDK 10.0.104 உடன் சுற்றுச் சூழல் விவரங்கள் புதுப்பிக்கப்பட்டன
- **AGENTS.md:** புதிய `zava-creative-writer-local` உள்ளமைப்புகள் (`ui/`, `csharp-web/`, `server.mjs`) மற்றும் C# முக்கிய பேக்கேஜ்கள் மற்றும் தொடர்பான TFM விவரங்கள் சேர்க்கப்பட்டன
- **labs/part2-foundry-local-sdk.md:** `.csproj` எடுத்துக்காட்டு புதுப்பிக்கப்பட்டது; முழு குறுக்கு மேடையுடன் தொடர்பான TFM, ஒரே நேரத்தில் இணைக்கப்பட முடியாத பேக்கேஜ் குறிகள் மற்றும் விளக்க குறிப்புகள் சேர்க்கப்பட்டன

### சரிபார்க்கப்பட்டது
- அனைத்து 3 C# திட்டங்களும் (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) விண்டோஸில் ARM64 யில் வெற்றிகரமாக கட்டமைக்கப்பட்டன
- உரையாடல் மாதிரி (`dotnet run chat`): WinML/QNN வழியாக `phi-3.5-mini-instruct-qnn-npu:1` ஆக ஏற்றப்பட்டது — NPU வகை நேரடியாக ஏற்றப்பட்டது CPUFallback இல்லாமல்
- முகவர் மாதிரி (`dotnet run agent`): பன்முறை உரையாடல் முழுமையாக இயங்கி, வெளியேறும் குறியீடு 0
- Foundry உள்ளூர் CLI v0.8.117 மற்றும் SDK v0.9.0 ஐ .NET SDK 9.0.312 இல் பயன்படுத்தி சோதனை செய்யப்பட்டன

---

## 2026-03-11 — குறியீட்டு திருத்தங்கள், மாதிரி சுத்தம், Mermaid வரைபடங்கள் மற்றும் சரிபார்ப்பு

### திருத்தப்பட்டது
- **அனைத்து 21 குறியீட்டு மாதிரிகள் (7 Python, 7 JavaScript, 7 C#):** வெளியேறும் பொழுது `model.unload()` / `unload_model()` / `model.UnloadAsync()` இடுகைச் சுத்தம் சேர்த்து OGA நினைவக கசிவு எச்சரிக்கை (அறியப்பட்ட பிரச்சினை #4) தீர்க்கப்பட்டது
- **csharp/WhisperTranscription.cs:** பொறுப்பற்ற `AppContext.BaseDirectory` சார்பான பாதை மாற்றப்பட்டு `FindSamplesDirectory()` பயன்பாட்டால் `samples/audio` மூலாதாரம் வெற்றிகரமாக கண்டுபிடிக்கும் செயல்பாட்டிடமொழி மாற்றப்பட்டது (அறியப்பட்ட பிரச்சினை #7)
- **csharp/csharp.csproj:** கடுமையாக நிர்ணயிக்கப்பட்ட `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` இடம் `$(NETCoreSdkRuntimeIdentifier)` தானாக கண்டுபிடிப்பு மூலம் மாற்றப்பட்டு எந்த மேடையிலும் `dotnet run` `-r` கொடுப்புக்கொள் இல்லாமல் இயங்கும் ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### மாற்றப்பட்டது
- **பகுதி 8:** மதிப்பாய்வு இயக்கத்தைக் கண்ட நேர்முகம் ASCII பெட்டி வரைபடத்திலிருந்து SVG படமாக மாற்றப்பட்டது
- **பகுதி 10:** தொகுத்தல் குழாய் வரைபடம் ASCII அம்புகளுக்கு பதிலாக உருவாக்கிய SVG படமாக மாற்றப்பட்டது
- **பகுதி 11:** கருவி அழைக்கும் ஓட்டம் மற்றும் தொடர் வரைபடங்கள் SVG உருவாக்கப்படுள்ளன
- **பகுதி 10:** "பணிமனை முடிந்தது!" பகுதி 11 ஆக இடம் மாற்றப்பட்டது; அதற்குப் பதிலாக "அடுத்த படிகள்" இணைப்பு சேர்க்கப்பட்டது
- **KNOWN-ISSUES.md:** CLI v0.8.117 இற்கு முழு மறுசரிபார்ப்பு. தீர்க்கப்பட்டவை நீக்கப்பட்டன: OGA Memory Leak (சுத்தம் சேர்க்கப்பட்டது), Whisper பாதை (FindSamplesDirectory), HTTP 500 நிலைத்த இன்பரன்சிங் (மீள்பரிசோதிக்க முடியாதது, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice கட்டுப்பாடுகள் (இப்போது `"required"` மற்றும் qwen2.5-0.5b முறைப்படி குறிப்பிட்ட செயல்பாட்டுடன் வேலை செய்கிறது). JS Whisper பிரச்சனை புதுப்பிப்பு — இப்போது அனைத்து கோப்புகளும் காலியான/இரட்டைப் பெறுமதி தருகின்றன (v0.9.x இன் பின் வரலமைப்பு, முக்கியச் சீர்திருத்தம்). #4 C# RID தானாக கண்டுபிடிப்பும் [#497](https://github.com/microsoft/Foundry-Local/issues/497) இணைப்பும் புதுப்பிக்கப்பட்டன. 7 திறந்த பிரச்சினைகள் மீதமுள்ளன.
- **javascript/foundry-local-whisper.mjs:** சுத்தம் பெயர் திருத்தம் செய்தது (`whisperModel` → `model`)

### சரிபார்க்கப்பட்டது
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — சுத்தம் உடன் வெற்றிகரமாக இயங்குகின்றன
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — சுத்தம் உடன் வெற்றிகரமாக இயங்குகின்றன
- C#: `dotnet build` எச்சரிக்கை 0, தவறு 0 உடன் வெற்றி (net9.0 இலக்கு)
- அனைத்து 7 Python கோப்புகளும் `py_compile` வாக்கியச் சரிபார்ப்பை கடந்துள்ளன
- அனைத்து 7 JavaScript கோப்புகளும் `node --check` இயல்பான சோதனையை கடந்துள்ளன

---

## 2026-03-10 — பகுதி 11: கருவி அழைப்பு, SDK API விரிவாக்கம் மற்றும் மாதிரி வரம்புகள்

### சேர்க்கப்பட்டது
- **பகுதி 11: உள்ளூர் மாதிரிகளுடன் கருவி அழைப்புகள்** — புதிய பயிற்சி வழிகாட்டி (`labs/part11-tool-calling.md`) 8 பயிற்சிகளுடன், கருவி பட்டிகள், பன்முறை உரையாடல் ஓட்டம், பல கருவி அழைப்புகள், தனிப்பயன் கருவிகள், ChatClient கருவி அழைப்பு மற்றும் `tool_choice`
- **Python மாதிரி:** `python/foundry-local-tool-calling.py` — OpenAI SDK உடன் `get_weather`/`get_population` கருவிகள் அழைப்பு
- **JavaScript மாதிரி:** `javascript/foundry-local-tool-calling.mjs` — SDK இன் உள்ளூர் `ChatClient` (`model.createChatClient()`) பயன்படுத்தி கருவி அழைப்பு
- **C# மாதிரி:** `csharp/ToolCalling.cs` — OpenAI C# SDK உடன் `ChatTool.CreateFunctionTool()` பயன்படுத்தி கருவி அழைப்பு
- **பகுதி 2, பயிற்சி 7:** உள்ளூர் `ChatClient` — `model.createChatClient()` (JS) மற்றும் `model.GetChatClientAsync()` (C#), OpenAI SDKக்கு மாற்றாக
- **பகுதி 2, பயிற்சி 8:** மாதிரி வகைகள் மற்றும் ஹார்ட்வேர் தேர்வு — `selectVariant()`, `variants`, NPU வகை அட்டவணை (7 மாதிரிகள்)
- **பகுதி 2, பயிற்சி 9:** மாதிரி மேம்பாடுகள் மற்றும் பட்டியல் புதுப்பிப்பு — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **பகுதி 2, பயிற்சி 10:** காரண அளிக்கும் மாதிரிகள் — `<think>` குறிச்சொல் பகுப்பாய்வு எடுத்துக்காட்டு உடன் `phi-4-mini-reasoning`
- **பகுதி 3, பயிற்சி 4:** `createChatClient` OpenAI SDKக்கு மாற்றாக, ஸ்ட்ரீமிங் பின்னுரையைக் கையாளும் எடுத்துக்காட்டு
- **AGENTS.md:** கருவி அழைப்பு, ChatClient மற்றும் காரண மாதிரிகள் குறியீட்டு நடைமுறைகள் சேர்க்கப்பட்டன

### மாற்றப்பட்டது
- **பகுதி 1:** மாதிரி பட்டியலில் விரிவாக்கம் — `phi-4-mini-reasoning`, `gpt-oss-20b`, `phi-4`, `qwen2.5-7b`, `qwen2.5-coder-7b`, `whisper-large-v3-turbo` சேர்க்கப்பட்டன
- **பகுதி 2:** API குறித்த அட்டவணைகள் விரிவாக்கம் — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync`
- **பகுதி 2:** பயிற்சிகள் 7-9 மறுஎண்கப்பட்டது 10-13 ஆக புதிய பயிற்சிகளுக்கு இடம் அளித்தது
- **பகுதி 3:** முக்கிய எடுத்துக்காட்டுகளில் உள்ளூர் ChatClient சேர்க்கப்பட்டது
- **README.md:** பகுதி 11 குறியீட்டு அட்டவணை மற்றும் கற்றல் குறிக்கோள் #11 சேர்க்கப்பட்டன; திட்ட அமைப்பு புதுப்பிக்கப்பட்டது
- **csharp/Program.cs:** CLI வழி இயக்கியில் `toolcall` வழக்கு சேர்க்கப்பட்டு உதவி உரை புதுப்பிக்கப்பட்டது

---

## 2026-03-09 — SDK v0.9.0 புதுப்பிப்பு, பிரிட்டிஷ் ஆங்கிலம் மற்றும் சரிபார்ப்பு

### மாற்றப்பட்டது
- **அனைத்து குறியீட்டு மாதிரிகள் (Python, JavaScript, C#):** Foundry Local SDK v0.9.0 APIக்கு புதுப்பிக்கப்பட்டன — `await catalog.getModel()` சரி செய்யப்பட்டது (`await` இல்லாமல் இருந்தது), `FoundryLocalManager` தொடக்க மாதிரிகள், இறுதிக் கண்டுபிடிப்பு திருத்தம் செய்யப்பட்டன
- **அனைத்து பயிற்சி வழிகாட்டிகள் (பகுதி 1-10):** பிரிட்டிஷ் ஆங்கிலம் (colour, catalogue, optimised முதலியன) க்கு மாற்றப்பட்டது
- **அனைத்து பயிற்சி வழிகாட்டிகள்:** v0.9.0 API இணக்கமான குறியீட்டு எடுத்துக்காட்டுகள் புதுப்பிக்கப்பட்டன
- **JavaScript முக்கிய திருத்தம்:** `catalog.getModel()` இல் `await` தவிர்ந்தது சேர்க்கப்பட்டது — அது முன்பு Promise ஆக இருந்தது, மாதிரியாக இல்லை, அமைதியான தவறுகளை உருவாக்கியது பின்னர்

### சரிபார்க்கப்பட்டது
- அனைத்து Python மாதிரிகளும் Foundry உள்ளூர் சேவையுடன் வெற்றிகரமாக இயங்குகின்றன
- அனைத்து JavaScript மாதிரிகளும் வெற்றிகரமாக இயங்குகின்றன (Node.js 18+)
- C# திட்டம் .NET 9.0 இல் கட்டமைக்கப்படுகிறது மற்றும் இயங்குகிறது (net8.0 SDK இருந்து முன்னேற்றம்)
- பணிமனை முழுவதும் 29 கோப்புகள் மாற்றப்பட்டு சரிபார்க்கப்பட்டது

---

## கோப்பு குறியீட்டு பட்டியல்

| கோப்பு | கடைசியாகப் புதுப்பிக்கப்பட்டது | விளக்கம் |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | மாதிரி பட்டியலில் விரிவாக்கம் |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | புதிய பயிற்சிகள் 7-10, விரிவாக்கப்பட்ட API அட்டவணைகள் |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | புதிய பயிற்சி 4 (ChatClient), எடுத்துக்காட்டுகள் மேம்படுத்தப்பட்டது |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + பிரிட்டிஷ் ஆங்கிலம் |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + பிரிட்டிஷ் ஆங்கிலம் |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + பிரிட்டிஷ் ஆங்கிலம் |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + பிரிட்டிஷ் ஆங்கிலம் |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | Mermaid வரைபடம் |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + பிரிட்டிஷ் ஆங்கிலம் |
| `labs/part10-custom-models.md` | 2026-03-11 | Mermaid வரைபடம், வேலைநிறுத்தம் முடிந்தது பகுதிக்கு 11க்கு மாற்றப்பட்டது |
| `labs/part11-tool-calling.md` | 2026-03-11 | புதிய ஆய்வு, Mermaid வரைபடங்கள், வேலைநிறுத்தம் முடிந்தது பிரிவு |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | புதியது: கருவி அழைப்பு மாதிரி |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | புதியது: கருவி அழைப்பு மாதிரி |
| `csharp/ToolCalling.cs` | 2026-03-10 | புதியது: கருவி அழைப்பு மாதிரி |
| `csharp/Program.cs` | 2026-03-10 | `toolcall` CLI கட்டளையை சேர்த்தது |
| `README.md` | 2026-03-10 | பகுதி 11, திட்ட அமைப்பு |
| `AGENTS.md` | 2026-03-10 | கருவி அழைப்பு + ChatClient மரபுகள் |
| `KNOWN-ISSUES.md` | 2026-03-11 | தீர்க்கப்பட்ட பிரச்சனை #7 நீக்கப்பட்டது, 6 திறந்த பிரச்சனைகள் உள்ளன |
| `csharp/csharp.csproj` | 2026-03-11 | பலதரப்பட்ட TFM, WinML/அடிப்படை SDK நிபந்தனை குறிப்புகள் |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | பலதரப்பட்ட TFM, தானாக RID கண்டறிதல் |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | பலதரப்பட்ட TFM, தானாக RID கண்டறிதல் |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU முயற்சி/பிடி வேலைச் சுற்றுவழியை நீக்கப்பட்டது |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU முயற்சி/பிடி வேலைச் சுற்றுவழியை நீக்கப்பட்டது |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU முயற்சி/பிடி வேலைச் சுற்றுவழியை நீக்கப்பட்டது |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU முயற்சி/பிடி வேலைச் சுற்றுவழியை நீக்கப்பட்டது |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU முயற்சி/பிடி வேலைச் சுற்றுவழியை நீக்கப்பட்டது |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | பலதரப்பட்ட .csproj எடுத்துக்காட்டு |
| `AGENTS.md` | 2026-03-11 | C# தொகுப்புகள் மற்றும் TFM விவரங்கள் புதுப்பிக்கப்பட்டது |
| `CHANGELOG.md` | 2026-03-11 | இந்த கோப்பு |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**தயாரிப்பு**:
இந்த ஆவணம் AI மொழிபெயர்ப்பு சேவை [Co-op Translator](https://github.com/Azure/co-op-translator) பயன்படுத்தி மொழிபெயர்க்கப்பட்டுள்ளது. நாங்கள் துல்லியத்திற்காக முயற்சிக்கும் போதிலும், தானியங்கி மொழிபெயர்ப்புகளில் பிழைகள் அல்லது தவறுகள் இருக்கக்கூடும் என்பதை தயவிருங்கள். அதன் சொந்த மொழியில் உள்ள அசல் ஆவணம் அதிகாரப்பூர்வமாகக் கருதப்பட வேண்டும். முக்கியமான தகவலுக்காக, தொழில்முறை மனித மொழிபெயர்ப்பு பரிந்துரைக்கப்படுகிறது. இந்த மொழிபெயர்ப்பின் பயன்பாட்டிலிருந்து ஏற்பட்ட எந்தவொரு தவறான புரிதல்களுக்கும் அல்லது தவறான விளக்கங்களுக்கும் நாங்கள் பொறுப்பில்லாதவர்கள்.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->