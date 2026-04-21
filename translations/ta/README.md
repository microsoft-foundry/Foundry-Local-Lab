<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local பயிற்சி - சாதனத்தில் உள்ள AI செயலிகளை உருவாக்குதல்

உங்கள் சொந்த கணினியில் மொழி மாதிரிகளை இயக்கி, [Foundry Local](https://foundrylocal.ai) மற்றும் [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) ஆகியவற்றுடன் அறிவார்ந்த செயலிகளை உருவாக்க ஒருங்கிணைந்த பயிற்சி.

> **Foundry Local என்றால் என்ன?** Foundry Local என்பது உங்கள் ஹார்டுவேரில் முழுமையாக மொழி மாதிரிகளை பதிவிறக்கம் செய்து, நிர்வகித்து வழங்கும் ஒரு இலகுரக ரன்டைம் ஆகும். இது **OpenAI-இற்கு ஒத்த API**-ஐ வெளிப்படுத்துகிறது, ஆகையால் OpenAI பேசும் எந்த கருவி அல்லது SDK-வும் இணைக்கலாம் - எந்த மேக கணக்கு தேவை இல்லை.

---

## கற்றல் குறிக்கோள்கள்

இந்த பயிற்சி முடிவடைவதில் நீங்கள் செய்யக்கூடியவை:

| # | குறிக்கோள் |
|---|-----------|
| 1 | Foundry Local ஐ நிறுவி, CLI மூலம் மாதிரிகளை நிர்வகிக்கவும் |
| 2 | நிரலாக்க மாதிரி நிர்வகிப்பிற்கு Foundry Local SDK API-வை கற்றுக்கொள்ளவும் |
| 3 | Python, JavaScript மற்றும் C# SDK-களை பயன்படுத்தி உள்ளூர் கணிப்பு சேவையகத்தை இணைக்கவும் |
| 4 | உங்கள் சொந்த தரவுகளில் அடிப்படையிலான பதில்களை வழங்க RAG (Retrieval-Augmented Generation) குழாயை உருவாக்கவும் |
| 5 | நிலையான வழிமுறைகள் மற்றும் உருவங்களை கொண்ட AI முகவர்களை உருவாக்கவும் |
| 6 | பன்முக முகவர் வேலைசார்புகளை பதில் மடங்குகளுடன் ஒருங்கிணைக்கவும் |
| 7 | தயாரிப்பு உயர் செயலியில் ஆராய்ச்சி செய்யவும் - Zava Creative Writer |
| 8 | பொன்மொழிகள் மற்றும் LLM-as-judge மதிப்பீடு சுட்டுக்காட்டுக்களுடன் மதிப்பீட்டு கட்டமைப்புகளை உருவாக்கவும் |
| 9 | Foundry Local SDK பயன்படுத்தி Whisper மூலம் சாதனத்தில் இருந்து பேசுவதை உரையாக மாற்றவும் |
| 10 | ONNX Runtime GenAI மற்றும் Foundry Local மூலம் தனிப்பயன் அல்லது Hugging Face மாதிரிகளை தொகுத்து இயக்கவும் |
| 11 | உள்ளூர் மாதிரிகளை படைப்பான செயலிகளுடன் இணைக்க டூல்-காலிங் முறையை செயல்படுத்தவும் |
| 12 | நேரடி ஸ்ட்ரீமிங்குடன் Zava Creative Writer க்கான உலாவி அடிப்படையிலான UI உருவாக்கவும் |

---

## முன்தகவல்கள்

| தேவைகள் | விவரங்கள் |
|-------------|---------|
| **இயந்திரக் கருவிகள்** | குறைந்தபட்சம் 8 GB RAM (16 GB பரிந்துரைக்கப்படுகிறது); AVX2 திறன் கொண்ட CPU அல்லது ஆதரவான GPU |
| **இயங்கு முறைமை** | Windows 10/11 (x64/ARM), Windows Server 2025, அல்லது macOS 13+ |
| **Foundry Local CLI** | `winget install Microsoft.FoundryLocal` (Windows) அல்லது `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) என்ற வழிகளில் நிறுவவும். விரிவுகளுக்கு [தொடங்கும் வழிகாட்டி](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) பார்க்கவும். |
| **மொழி ரன்டைம்** | **Python 3.9+** மற்றும்/அல்லது **.NET 9.0+** மற்றும்/அல்லது **Node.js 18+** |
| **Git** | இந்த சேமிப்பகத்தை கிளோனுக்கு |

---

## தொடக்கம்

```bash
# 1. ரெப்போஸிடரியை நகலெடுக்கவும்
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Foundry Local நிறுவப்பட்டுள்ளதா என்பதை உறுதிப்படுத்தவும்
foundry model list              # கிடைக்கும் மாதிரிகளை பட்டியலிடவும்
foundry model run phi-3.5-mini  # ஒரு இடையூறு உரையாடலை துவங்கவும்

# 3. உங்கள் மொழி பாதையை தேர்வுசெய்க (முழு அமைப்புக்கு பாகம் 2 ஆய்வகம் பார்க்கவும்)
```

| மொழி | விரைவு தொடக்கம் |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## பயிற்சி பகுதிகள்

### பகுதி 1: Foundry Local கொண்டு தொடக்கம்

**பயிற்சி வழிகாட்டி:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local என்றால் என்ன மற்றும் அது எவ்வாறு செயல்படுகிறது
- Windows மற்றும் macOS-ல் CLI நிறுவல்
- மாதிரிகளை ஆராய்தல் - பட்டியலிடுதல், பதிவிறக்கம், இயக்கம்
- மாதிரி பூனைகள் மற்றும் மாறும் போர்டுகள் பற்றி புரிதல்

---

### பகுதி 2: Foundry Local SDK ஆழ்ந்த பயிற்சி

**பயிற்சி வழிகாட்டி:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- செயலி வளர்ச்சிக்கான CLI-க்கு பதிலாக SDK பயன்பாட்டின் நன்மைகள்
- Python, JavaScript, மற்றும் C#-க்கான முழுமையான SDK API குறிப்புகள்
- சேவை நிர்வகிப்பு, கலைக்கண்டு பார்வை, மாதிரி வாழ்கைப் பக்கமுறை (பதிவிறக்கம், ஏற்றுதல், அகற்றுதல்)
- விரைவு தொடக்கம் வடிவங்கள்: Python கட்டமைப்பாளர் உள்நுழைவு, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` ஊடுருவல், பூனைகள் மற்றும் ஹார்டுவேர் சாதகமான மாதிரி தேர்வு

---

### பகுதி 3: SDKகள் மற்றும் APIகள்

**பயிற்சி வழிகாட்டி:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript மற்றும் C# வழியாக Foundry Local-இன் இணைப்பு
- Foundry Local SDK-ஐ சேவையை நிரலில் நிர்வகிக்க பயன்படுத்துதல்
- OpenAI-அனுகூல API மூலம் ஸ்ட்ரீமிங் சந்திப்பு முடிப்புகள்
- ஒவ்வொரு மொழிக்கும் SDK முறையின் குறிப்பு

**குறியீடு எடுத்துக்காட்டுகள்:**

| மொழி | கோப்பு | விளக்கம் |
|----------|------|-------------|
| Python | `python/foundry-local.py` | அடிப்படை ஸ்ட்ரீமிங் சந்திப்பு |
| C# | `csharp/BasicChat.cs` | .NET உடன் ஸ்ட்ரீமிங் சந்திப்பு |
| JavaScript | `javascript/foundry-local.mjs` | Node.js உடன் ஸ்ட்ரீமிங் சந்திப்பு |

---

### பகுதி 4: Retrieval-Augmented Generation (RAG)

**பயிற்சி வழிகாட்டி:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG என்றால் என்ன மற்றும் ஏன் முக்கியம்
- நினைவகத்தில் அறிவுத்தளத்தை உருவாக்குதல்
- கேள்வி இணைக்கும் மீட்டெடுப்பில் மதிப்பீடு
- அடிப்படையிடப்பட்ட அமைப்பு ஊக்கங்கள் உருவாக்குதல்
- சாதனத்தில் முழுமையான RAG குழாய் இயக்குதல்

**குறியீடு எடுத்துக்காட்டுகள்:**

| மொழி | கோப்பு |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### பகுதி 5: AI முகவர்களை உருவாக்குதல்

**பயிற்சி வழிகாட்டி:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI முகவர் என்றால் என்ன (அது ஒரு நேரடி LLM அழைப்புக்கு மாறாக)
- `ChatAgent` வடிவமைப்பு மற்றும் Microsoft Agent Framework
- அமைப்பு வழிமுறைகள், உருவங்கள் மற்றும் பல மடங்கு உரையாடல்கள்
- முகவர்களிடமிருந்து வடிவமைக்கப்பட்ட வெளியீடு (JSON)

**குறியீடு எடுத்துக்காட்டுகள்:**

| மொழி | கோப்பு | விளக்கம் |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | முகவர்படுத்தல் உடன் ஒரே முகவர் |
| C# | `csharp/SingleAgent.cs` | ஒரே முகவர் (ChatAgent வடிவமைப்பு) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | ஒரே முகவர் (ChatAgent வடிவமைப்பு) |

---

### பகுதி 6: பன்முக முகவர் வேலைசார்புகள்

**பயிற்சி வழிகாட்டி:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- பன்முக முகவர் குழாய்கள்: ஆராய்ச்சியாளர் → எழுத்தாளர் → தொகுப்பாளர்
- ஒருங்கிணைந்த ஒழுங்குமுறை மற்றும் பதில் மடங்குகள்
- பகிர்ந்து கொள்ளப்பட்ட அமைப்பு மற்றும் வடிவமைக்கப்பட்ட ஒப்படைப்பு
- உங்கள் சொந்த பன்முக முகவர் வேலைசார்பை வடிவமைக்கவும்

**குறியீடு எடுத்துக்காட்டுகள்:**

| மொழி | கோப்பு | விளக்கம் |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | மூன்று முகவர் குழாய் |
| C# | `csharp/MultiAgent.cs` | மூன்று முகவர் குழாய் |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | மூன்று முகவர் குழாய் |

---

### பகுதி 7: Zava Creative Writer - கவசப்பு பயன்பாடு

**பயிற்சி வழிகாட்டி:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- நான்கு சிறப்பு முகவர்களுடன் தயாரிப்பு மாதிரியான பன்முக முகவர் செயலி
- மதிப்பீடு இயக்கும் பதில் மடங்குகளுடன் ஒழுங்கமைக்கப்பட்ட குழாய்
- ஸ்ட்ரீமிங் வெளியீடு, தயாரிப்பு கலைக்கட்டை தேடல், வடிவமைக்கப்பட்ட JSON ஒப்படைப்பு
- Python (FastAPI), JavaScript (Node.js CLI), மற்றும் C# (.NET கன்சோல்) என மூன்று மொழிகளில் முழுமையான செயலாக்கம்

**குறியீடு எடுத்துக்காட்டுகள்:**

| மொழி | அடைவகம் | விளக்கம் |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | ஒழுங்கமைப்பாளர் உடன் FastAPI வலை சேவை |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI செயலி |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 கன்சோல் செயலி |

---

### பகுதி 8: மதிப்பீடு வழிகாட்டிய வளர்ச்சி

**பயிற்சி வழிகாட்டி:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- பொன்மொழி தொகுப்புகளை பயன்படுத்தி AI முகவர்களுக்கான ஒழுங்கமைக்கப்பட்ட மதிப்பீட்டு கட்டமைப்பை உருவாக்கவும்
- விதிமுறை அடிப்படையிலான சரிபார்ப்புகள் (நீளம், முக்கிய சொல் வசதி, தடை terms) மற்றும் LLM-as-judge மதிப்பீடு
- பொதுவான மதிப்பீட்டு அட்டவணைகளுடன் prompt வகைகளின் சமயவிலாக்கப்பட்ட ஒப்பீடு
- பகுதி 7 இல் இருந்து Zava Editor முகவர் வடிவமைப்பை ஆஃப்லைன் சோதனை தொகுப்பாக்கம் உருவாக்குதல்
- Python, JavaScript மற்றும் C# வழிகளுக்கு அமைப்பு

**குறியீடு எடுத்துக்காட்டுகள்:**

| மொழி | கோப்பு | விளக்கம் |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | மதிப்பீட்டு கட்டமைப்பு |
| C# | `csharp/AgentEvaluation.cs` | மதிப்பீட்டு கட்டமைப்பு |
| JavaScript | `javascript/foundry-local-eval.mjs` | மதிப்பீட்டு கட்டமைப்பு |

---

### பகுதி 9: Whisper உடன் குரல் உரை மாற்றம்

**பயிற்சி வழிகாட்டி:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- உள்ளூரில் இயங்கும் OpenAI Whisper-ஐப் பயன்படுத்தி பேச்சை உரையாக மாற்றுதல்
- தனியுரிமை முதன்மை ஆக் ஆடியோ செயலாக்கம் - ஆடியோ உங்கள் சாதனத்தில் வெளியே செல்லாது
- Python, JavaScript மற்றும் C# வழிகள், `client.audio.transcriptions.create()` (Python/JS) மற்றும் `AudioClient.TranscribeAudioAsync()` (C#) பயன்படுத்தல்
- கையாள்வுக்கு Zava-வான மாதிரி ஆடியோ கோப்புகளையும் கொண்டுள்ளது

**குறியீடு எடுத்துக்காட்டுகள்:**

| மொழி | கோப்பு | விளக்கம் |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper குரல் உரை மாற்றம் |
| C# | `csharp/WhisperTranscription.cs` | Whisper குரல் உரை மாற்றம் |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper குரல் உரை மாற்றம் |

> **குறிப்பு:** இந்த பயிற்சி **Foundry Local SDK**-ஐ பயன்படுத்தி Whisper மாதிரியை நிரலாக பதிவிறக்கம் செய்து ஏற்றுகிறது, பிறகு உரைப்படுத்த உள்ளூர் OpenAI-அனுகூல இடைநிலைச் சேவையகத்திற்கு ஆடியோ அனுப்புகிறது. Whisper மாதிரி (`whisper`) Foundry Local கலைக்கட்டில் பட்டியலிடப்பட்டுள்ளது மற்றும் முழுமையாக சாதனத்தில் இயங்குகிறது - மேக API விசைகள் அல்லது இணைய அணுகல் தேவையில்லை.

---

### பகுதி 10: தனிப்பயன் அல்லது Hugging Face மாதிரிகளை பயன்படுத்துதல்

**பயிற்சி வழிகாட்டி:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ONNX Runtime GenAI மாதிரி கட்டுமானியைப் பயன்படுத்தி Hugging Face மாதிரிகளை பண்படுத்தப்பட்ட ONNX வடிவத்திற்கு தொகுத்தல்
- ஹார்டுவேருக்கு சிறப்பான தொகுப்பு (CPU, NVIDIA GPU, DirectML, WebGPU) மற்றும் அளவீடு (int4, fp16, bf16)
- Foundry Local க்கான உரையாடல் மாதிரிகள் (chat-template) கட்டமைப்பு கோப்புகள் உருவாக்குதல்
- தொகுக்கப்பட்ட மாதிரிகளை Foundry Local கேஷ்-க்கு சேர்த்தல்
- CLI, REST API மற்றும் OpenAI SDK வழியாக தனிப்பயன் மாதிரிகளை இயக்குதல்
- எடுத்து பார்க்க உதாரணம்: Qwen/Qwen3-0.6B முழுமையான தொகுப்பு

---

### பகுதி 11: உள்ளூர் மாதிரிகளுடன் டூல் அழைப்பு

**பயிற்சி வழிகாட்டி:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- உள்ளூர் மாதிரிகளுக்கு வெளியே செயல்பாடுகளை அழைக்க முடிவு செய்கிறது (டூல்/செயலி அழைப்பு)
- OpenAI செயலி அழைப்பு வடிவத்தைப் பயன்படுத்தி டூல் அமைப்புகளை வரையறுத்தல்
- பன்மடங்கு டூல் அழைப்பு உரையாடல் ஓட்டத்தை கையாள்தல்
- உள்ளூர் டூல் அழைப்புகளை செய்தல் மற்றும் முடிவுகளை மாதிரிக்கு திருப்பி வழங்குதல்
- டூல் அழைப்பு காட்சிகளுக்கு சரியான மாதிரியைத் தேர்ந்தெடுங் (Qwen 2.5, Phi-4-mini)
- டூல் அழைப்பிற்கு SDK இன் சொந்த `ChatClient`-ஐப் பயன்படுத்துதல் (JavaScript)

**குறியீடு எடுத்துக்காட்டுகள்:**

| மொழி | கோப்பு | விளக்கம் |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | வானிலை/மக்கள் தொகை டூல் அழைப்பு |
| C# | `csharp/ToolCalling.cs` | .NET உடன் டூல் அழைப்பு |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient உடன் டூல் அழைப்பு |

---

### பகுதி 12: Zava Creative Writer க்கான வலை UI உருவாக்குதல்

**பயிற்சி வழிகாட்டி:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer க்கான உலாவி அடிப்படையிலான முன்பக்கம் சேர்க்கவும்
- Python (FastAPI), JavaScript (Node.js HTTP) மற்றும் C# (ASP.NET Core) வழியாக பகிரப்பட்ட UI வழங்கல்
- Fetch API மற்றும் ReadableStream உடன் உலாவியில் ஸ்ட்ரீமிங் NDJSON நுகர்வு
- நேரடி முகவர் நிலை புரிந்துணர்வு மற்றும் நேரடி கட்டுரைய உரை ஸ்ட்ரீமிங்

**குறியீட்டு (பகிரப்பட்ட UI):**

| கோப்பு | விளக்கம் |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | பக்கம் அமைப்பு |
| `zava-creative-writer-local/ui/style.css` | வடிவமைப்பு |
| `zava-creative-writer-local/ui/app.js` | ஸ்ட்ரீம் வாசகி மற்றும் DOM புதுப்பிப்பு வகுப்பு |

**பின்னணி கூடுதல்கள்:**

| மொழி | கோப்பு | விளக்கம் |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | நிலையான UI வழங்க புதுப்பிக்கப்பட்டது |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ஒழுங்கமைப்பாளரை சுற்றிய புதிய HTTP சேவையகமாகும் |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | புதிய ASP.NET Core குறைந்தபட்ச API திட்டம் |

---

### பகுதி 13: பயிற்சி முடிந்து முழுமையானது
**லைப் வழிகாட்டி:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- நீங்கள் 12 பகுதிகளிலும் கட்டிய அனைத்திற்கான சுருக்கம்
- உங்கள் பயன்பாடுகளை விரிவுப்படுத்த மேலும் யோசனைகள்
- வளங்கள் மற்றும் ஆவணங்களை இணைப்புகள்

---

## திட்ட அமைப்பு

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

## வளங்கள்

| வளம் | இணைப்பு |
|----------|------|
| Foundry Local இணையதளம் | [foundrylocal.ai](https://foundrylocal.ai) |
| மாதிரி பட்டியல் | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| துவங்கும் வழிகாட்டி | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK குறிப்பு | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft முகவர் கட்டமைப்பு | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI விசிறி | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## உரிமம்

இந்த வேலைமுறைத் தொகுப்பு கல்விக் காரணமாக வழங்கப்படுகிறது.

---

**இனிய கட்டுமானம்! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**தவறுதலற்பாட்டுரை**:  
இந்த ஆவணம் AI மொழிபெயர்ப்பு சேவை [Co-op Translator](https://github.com/Azure/co-op-translator) பயன்படுத்தி மொழிபெயர்க்கப்பட்டது. நாங்கள் துல்லியத்திற்காக முயலுகிறோம் என்றாலும், தானியங்கி மொழிபெயர்ப்புகளில் பிழைகள் அல்லது தவறான தகவல்கள் இருக்கக்கூடும் என்பதை தயவுசெய்து கவனிக்கவும். இயல்புநிலையான மொழியில் உள்ள அசல் ஆவணம் அதிகாரப்பூர்வ மூலமாகக் கருதப்பட வேண்டும். முக்கியமான தகவல்க்காக, தொழில்முறை மனித மொழிபெயர்ப்பு பரிந்துரைக்கப்படுகிறது. இந்த மொழிபெயர்ப்பின் பயன்பாட்டால் ஏற்படும் எந்த தவறான புரிதல்கள் அல்லது தவறான விளக்கங்களுக்கு நாங்கள் பொறுப்பேற்கவில்லை.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->