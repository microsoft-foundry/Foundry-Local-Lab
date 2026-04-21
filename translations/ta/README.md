<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local பணிமனை - சாதனத்தில் AI பயன்பாடுகளை உருவாக்குதல்

உங்கள் சொந்த கணினியில் மொழி மாதிரிகளை இயக்கி [Foundry Local](https://foundrylocal.ai) மற்றும் [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) இன் உதவியுடன் புத்திசாலி பயன்பாடுகளை கட்டமைப்பதற்கான கைபோன்ற பயிற்சி வகுப்பு.

> **Foundry Local என்றால் என்ன?** Foundry Local என்பது உங்கள் கருவியில் முழுமையாக மொழி மாதிரிகளை டவுன்லோட், நிர್ವಹிக்க மற்றும் வழங்க அனுமதிக்கும் ஒரு எளிய ரன்டைம் ஆகும். இது **OpenAI-அனுகூள்படுத்தப்பட்ட API** ஐ வெளிக்கொடுத்து எந்த OpenAI பேசும் கருவி அல்லது SDKயும் இணைக்க முடியும் - கிளவுட் கணக்கு தேவையில்லை.

### 🌐 பன்மொழி ஆதரவு

#### GitHub Action மூலம் ஆதரிக்கப்பட்டது (தானாகவும் எப்போதும் புதுப்பிக்கப்பட்டதும்)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[அரபு](../ar/README.md) | [பெங்காலி](../bn/README.md) | [பல்கேரியன்](../bg/README.md) | [ புர்மீஸ் (மியன்மர்)](../my/README.md) | [செயினீஸ் (எளிமைப்படுத்திய)](../zh-CN/README.md) | [செயினீஸ் (சம்பிரதாய, ஹாங்காங்)](../zh-HK/README.md) | [செயினீஸ் (சம்பிரதாய, மாகாவ்)](../zh-MO/README.md) | [செயினீஸ் (சம்பிரதாய, தைவான்)](../zh-TW/README.md) | [குரோஷியன்](../hr/README.md) | [செக்](../cs/README.md) | [டேனிஷ்](../da/README.md) | [டச்சு](../nl/README.md) | [எஸ்டோனியன்](../et/README.md) | [பின்னிஷ்](../fi/README.md) | [பிரஞ்சு](../fr/README.md) | [ஜெர்மன்](../de/README.md) | [கிரேக்கு](../el/README.md) | [ஹீப்ரூ](../he/README.md) | [ஹிந்தி](../hi/README.md) | [ஹங்கேரியன்](../hu/README.md) | [இந்தோனேசியன்](../id/README.md) | [இத்தாலியன்](../it/README.md) | [ஜாப்பனீஸ்](../ja/README.md) | [கன்னடா](../kn/README.md) | [கமிழ்](../km/README.md) | [கொரியன்](../ko/README.md) | [லித்துவானியன்](../lt/README.md) | [மலாய்](../ms/README.md) | [மலையாளம்](../ml/README.md) | [மராத்தி](../mr/README.md) | [நேபாளி](../ne/README.md) | [நைஜீரியன் பிஜின்](../pcm/README.md) | [நார்வேஜியன்](../no/README.md) | [பேர்சியன் (ஃபார்ஸி)](../fa/README.md) | [போலிஷ்](../pl/README.md) | [போர்ச்சுகீஸ் (பிரேசில்)](../pt-BR/README.md) | [போர்ச்சுகீஸ் (போர்ச்சுகல்)](../pt-PT/README.md) | [பிஞ்ஜாபி (குருமுகி)](../pa/README.md) | [ரோமானியன்](../ro/README.md) | [ரஷியன்](../ru/README.md) | [செர்பியன் (சிரிலிக்)](../sr/README.md) | [ஸ்டோவாக்](../sk/README.md) | [ஸ்லோவேனியன்](../sl/README.md) | [ஸ்பானிஷ்](../es/README.md) | [ஸ்வாஹிலி](../sw/README.md) | [ஸ்வீடிஷ்](../sv/README.md) | [தாகாலோக் (பிலிப்பைன்ஸ்)](../tl/README.md) | [தமிழ்](./README.md) | [தெலுங்கு](../te/README.md) | [தை](../th/README.md) | [துருக்கி](../tr/README.md) | [உக்ரைனியன்](../uk/README.md) | [உருது](../ur/README.md) | [வியட்நாமீஸ்](../vi/README.md)

> **உருக்கு உள்ளூரில் கிளோன் செய்ய விரும்புகிறீர்களா?**
>
> இந்த திரட்டில் 50+ மொழி மொழிபெயர்ப்புகள் உள்ளதால் டவுன்லோட் அளவு பெரிதாகும். மொழிபெயர்ப்புகள் இல்லாமல் கிளோன் செய்ய, sparse checkout பயன்படுத்தவும்:
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
> இதனால் நீங்கள் கோர்ஸை முடிக்க தேவையான அனைத்தும் குறைந்த நேரத்தில் டவுன்லோட் செய்ய முடியும்.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## கற்றல் இலக்குகள்

இந்த பணிமனை முடிவில் நீங்கள் பின்வரும் திறன்களை பெறுவீர்கள்:

| # | இலக்கு |
|---|---------|
| 1 | Foundry Local ஐ நிறுவி CLI மூலம் மாதிரிகளை நிர்வகிக்கவும் |
| 2 | Foundry Local SDK API வை நிரலாக்க முறையில் மாதிரி நிர்வகிப்பதற்காக கற்றுக்கொள்ளவும் |
| 3 | Python, JavaScript மற்றும் C# SDK களைப் பயன்படுத்தி உள்ளூரான கணிப்பு சேவையகத்துடன் இணைக்கவும் |
| 4 | உங்கள் சொந்த தரவுகளில் தழுவப்பட்ட பதில்களை உருவாக்கும் Retrieval-Augmented Generation (RAG) குழாய்ச் சங்கிலியை உருவாக்கவும் |
| 5 | நிலையான வழிமுறைகள் மற்றும் தனிப்பட்ட குணாதிசயங்களுடன் AI முகவர்களை உருவாக்கவும் |
| 6 | கருத்துதவியுடன் பன்முக முகவர் பணிமுறைகளைக் ஒருங்கிணைக்கவும் |
| 7 | சாவா கிரியேட்டிவ் ரraiட்டர் என்ற உற்பத்தி கட்டத்தினை ஆராயவும் |
| 8 | பொற்கரைத் தரவுத்தொகுப்புகள் மற்றும் LLM-ஆகத் துல்லியமாய்க் குறியிடல் மூலம் மதிப்பீட்டு கட்டமைப்புகளை உருவாக்கவும் |
| 9 | Foundry Local SDK ஐப் பயன்படுத்தி சாதனத்தில் பேச்சிலிருந்து உரைக்கு மாற்றுவதற்கு Whisper உடன் ஒலியை மாற்றவும் |
| 10 | ONNX Runtime GenAI மற்றும் Foundry Local மூலம் தனிப்பயன் அல்லது Hugging Face மாதிரிகளை கம்பைல் செய்து இயக்கவும் |
| 11 | உள்ளூர் மாதிரிகளுக்கு வெளிப்புற செயல்பாடுகளை அளிக்கும் கருவி-கால் முறைநூலை இயக்கவும் |
| 12 | நடைமுறை ஒளிபரப்புடன் Zava Creative Writer இற்கான உலாவி அடிப்படையிலான UI ஐ உருவாக்கவும் |

---

## முன்பே தேவையானவை

| தேவைகள் | விவரங்கள் |
|---------|----------|
| **தொழில்நுட்பம்** | குறைந்தபட்சம் 8 GB RAM (16 GB பரிந்துரைக்கப்படுகிறது); AVX2 திறன் கொண்ட CPU அல்லது ஆதரவு GPU |
| **அசல் இயக்க முறைமை** | Windows 10/11 (x64/ARM), Windows Server 2025, அல்லது macOS 13+ |
| **Foundry Local CLI** | Windows இல் `winget install Microsoft.FoundryLocal` அல்லது macOS இல் `brew tap microsoft/foundrylocal && brew install foundrylocal` மூலம் நிறுவவும். விரிவுகளுக்கு [தொடக்கம் செய்யும் வழிகாட்டி](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) ஐ பார்க்கவும். |
| **மொழி ரன்டைம்** | **Python 3.9+** மற்றும்/அல்லது **.NET 9.0+** மற்றும்/அல்லது **Node.js 18+** |
| **Git** | இந்த தொகுப்பை கிளோன் செய்ய தேவையானது |

---

## தொடக்கம் செய்வது எப்படி

```bash
# 1. தொகுப்பகத்தை நகலெடு
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Foundry Local நிறுவப்பட்டுள்ளதா என்று சரிபார்க்கவும்
foundry model list              # கிடைக்கும் மாதிரிகளை பட்டியலிடு
foundry model run phi-3.5-mini  # ஒரு தொடர்பாடல் அரட்டை தொடங்கு

# 3. உங்கள் மொழி வழிமுறையை தேர்வு செய்க (முழு அமைப்புக்காக பகுதி 2 ஆய்வுக்கூடத்தைப் பார்க்கவும்)
```

| மொழி | விரைவானத் தொடக்கம் |
|-------|--------------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## பணிமனை பகுதிகள்

### பகுதி 1: Foundry Local உடன் தொடக்கம்

**கருவி கையேடு:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local என்றால் என்ன மற்றும் அது எப்படி செயல்படுகிறது
- Windows மற்றும் macOS இல் CLI ஐ நிறுவுதல்
- மாதிரிகளை ஆராய்ந்து பட்டியலிடுதல், டவுன்லோடு மற்றும் இயக்குதல்
- மாதிரி போலிகள் மற்றும் இயக்கநிலை போர்டுகளைப் புரிதல்

---

### பகுதி 2: Foundry Local SDK ஆழமாக

**கருவி கையேடு:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- பயன்பாடுகளை உருவாக்க CLI ஐ விட SDKஐ ஏன் பயன்படுத்துவது
- Python, JavaScript மற்றும் C# க்கான SDK API முழுமையான குறிப்பு
- சேவை நிர்வாகம், பட்டியல் உலாவல், மாதிரி வாழ்நாள் மேலாண்மை (டவுன்லோடு, ஏற்றுதல், இறக்குதல்)
- விரைவான தொடக்கம் முறைகள்: Python கட்டமைப்பாளர் bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` உருவாக்க மேலதிகத் தகவல்கள், போலிகள் மற்றும் சிறந்த கருவி உகந்த மாதிரி தேர்வு

---

### பகுதி 3: SDKகள் மற்றும் APIகள்

**கருவி கையேடு:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript மற்றும் C# மூலம் Foundry Local ஐ இணைத்தல்
- Foundry Local SDK ஐப் பயன்படுத்தி சேவையை நிரலாக்க முறையில் நிர்வகித்தல்
- OpenAI-அனுகூள்படுத்தப்பட்ட API மூலம் ஸ்ட்ரீமிங் உரையாடல் முடிப்புகள்
- ஒவ்வொரு மொழிக்கும் SDK முறை குறிப்பு

**குறியீடு உதாரணங்கள்:**

| மொழி | கோப்பு | விளக்கம் |
|-------|---------|----------|
| Python | `python/foundry-local.py` | அடிப்படையான ஸ்ட்ரீமிங் உரையாடல் |
| C# | `csharp/BasicChat.cs` | .NET உடன் ஸ்ட்ரீமிங் உரையாடல் |
| JavaScript | `javascript/foundry-local.mjs` | Node.js உடன் ஸ்ட்ரீமிங் உரையாடல் |

---

### பகுதி 4: Retrieval-Augmented Generation (RAG)

**கருவி கையேடு:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG என்றால் என்ன மற்றும் அது ஏன் முக்கியம்
- நினைவகத்தில் அடிப்படையான அறிவுத்தொகுப்பு உருவாக்கல்
- மதிப்பீட்டுடன் விசையொலி-திரட்டல் மீட்டெடுப்பு
- நிலையான அமைப்பு கொண்ட சிஸ்டம் ப்ராம்ப்ட்களை உருவாக்கல்
- சாதனத்தில் முழுமையான RAG குழாய்ச் சங்கிலி இயக்கல்

**குறியீடு உதாரணங்கள்:**

| மொழி | கோப்பு |
|-------|--------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### பகுதி 5: AI முகவர்களை கட்டமைத்தல்

**கருவி கையேடு:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI முகவரி என்றால் என்ன (ஒரு நேரடி LLM அழைப்பின் மாற்று)
- `ChatAgent` முறை மற்றும் Microsoft Agent Framework
- சிஸ்டம் வழிமுறைகள், குணாதிசயங்கள் மற்றும் பல-முறை உரையாடல்களும்
- முகவர்கள் வழங்கும் கட்டமைக்கப்பட்ட வெளியீடு (JSON)

**குறியீடு உதாரணங்கள்:**

| மொழி | கோப்பு | விளக்கம் |
|-------|---------|----------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework உடன் தனி முகவர் |
| C# | `csharp/SingleAgent.cs` | தனி முகவர் (ChatAgent முறை) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | தனி முகவர் (ChatAgent முறை) |

---

### பகுதி 6: பன்முக முகவர் பணிமுறைகள்

**கருவி கையேடு:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- பன்முக முகவர் குழாய்ச் சங்கிலிகள்: ஆராய்ச்சியாளர் → எழுத்தாளர் → தொகுப்பாளர்
- தொடர்ச்சியான ஒருங்கிணைவு மற்றும் கருத்து நேர்மறை காட்சிகள்
- பகிரப்பட்ட அமைப்புக்கள் மற்றும் கட்டமைக்கப்பட்ட கையளிப்புக்கள்
- உங்கள் சொந்த பன்முக முகவர் பணிமுறை வடிவமைத்து உருவாக்கல்

**குறியீடு உதாரணங்கள்:**

| மொழி | கோப்பு | விளக்கம் |
|-------|---------|----------|
| Python | `python/foundry-local-multi-agent.py` | மூன்று முகவர் குழாய் |
| C# | `csharp/MultiAgent.cs` | மூன்று முகவர் குழாய் |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | மூன்று முகவர் குழாய் |

---

### பகுதி 7: Zava Creative Writer - ஒருங்கிணைப்பு பயன்பாடு

**கருவி கையேடு:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 சிறப்பு முகவர்களுடன் உற்பத்தி வடிவ பன்முக முகவர் பயன்பாடு
- மதிப்பீட்டாளர் இயக்கும் கருத்து நேர்மறை காட்சிகளுடன் தொடர்ச்சியான குழாய்ச் சங்கிலி
- ஸ்ட்ரீமிங் வெளியீடு, பொருள் பட்டியல் தேடல், கட்டமைக்கப்பட்ட JSON கையளிப்புக்கள்
- Python (FastAPI), JavaScript (Node.js CLI) மற்றும் C# (.NET கன்சோல்) முழுமையாக அமல்படுத்தல்

**குறியீடு உதாரணங்கள்:**

| மொழி | அடைவு | விளக்கம் |
|-------|---------|----------|
| Python | `zava-creative-writer-local/src/api/` | ஒருங்கிணைப்பாளியுடன் FastAPI வலை சேவை |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI பயன்பாடு |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 கன்சோல் பயன்பாடு |

---

### பகுதி 8: மதிப்பீடு சார்ந்த மேம்பாடு

**கருவி கையேடு:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- பொற்கரைத் தரவுத்தொகுப்புகளைப் பயன்படுத்தி AI முகவர்களுக்கான முறையாக மதிப்பீட்டு கட்டமைப்பை உருவாக்குதல்
- விதிமுறை அடிப்படையிலான சோதனைகள் (நீளம், விசையொலி வ پوش்தை, தடையுடைய வார்த்தைகள்) + LLM-போன்ற மதிப்பெண் வழங்கல்
- ப்ராம்ப்ட் வேரியன்ட் களின் அருகிலான ஒப்பீடு மற்றும் மொத்த மதிப்பெண் அட்டவணைகள்
- பகுதி 7 இன் Zava Editor முகவர் முறைநூலை ஒரு ஆஃப்லைன் சோதனை தொகுப்பாக விரிவாக்கல்
- Python, JavaScript மற்றும் C# துறைகள்

**குறியீடு உதாரணங்கள்:**

| மொழி | கோப்பு | விளக்கம் |
|-------|---------|----------|
| Python | `python/foundry-local-eval.py` | மதிப்பீட்டு கட்டமைப்பு |
| C# | `csharp/AgentEvaluation.cs` | மதிப்பீட்டு கட்டமைப்பு |
| JavaScript | `javascript/foundry-local-eval.mjs` | மதிப்பீட்டு கட்டமைப்பு |

---

### பகுதி 9: விஸ்பர் மூலம் குரல் உரைமாற்றம்

**கருவி கையேடு:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- OpenAI Whisperஐ உள்ளூர் முறையில் இயக்கி பேசியதை உரையாக மாற்றுதல்
- தனியுரிமை முதன்மையான ஆடியோ செயலாக்கம் - ஆடியோ எதையும் உங்கள் சாதனத்தை விட்டு வெளியே செல்லாது
- Python, JavaScript, மற்றும் C# பாட்றுகள் `client.audio.transcriptions.create()` (Python/JS) மற்றும் `AudioClient.TranscribeAudioAsync()` (C#) ஆகியவையுடன்
- நடைமுறை பயிற்சிக்காக Zava தீமை பாணியுடைய மாதிரியான ஆடியோ கோப்புகளை கொண்டுள்ளது

**குறியீடு மாதிரிகள்:**

| மொழி | கோப்பு | விவரம் |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper குரல் உரை மாற்றம் |
| C# | `csharp/WhisperTranscription.cs` | Whisper குரல் உரை மாற்றம் |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper குரல் உரை மாற்றம் |

> **குறிப்பு:** இந்த ஆய்வு அறையில் **Foundry Local SDK** பயன்படுத்தி Whisper மாடலை நிரலாக்க முறையில் பதிவிறக்கம் செய்து ஏற்றுகிறது, பின்னர் உள்ளூர் OpenAI இணக்கமான முடிவுக்குப் படி ஆடியோ அனுப்பி உரைக்கு மாற்றுகிறது. Whisper மாடல் (`whisper`) Foundry Local பட்டியலில் உள்ளது மற்றும் முழுமையாக சாதனத்தில் இயக்கப்படுகிறது - எந்த காளாய API சாவிகள் அல்லது நெட்வொர்க் அணுகலும் தேவையில்லை.

---

### பகுதி 10: தனிப்பயன் அல்லது Hugging Face மாடல்களை பயன்படுத்துதல்

**ஆய்வு வழிகாட்டி:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face மாடல்களை மேம்படுத்திய ONNX வடிவத்திற்கு ONNX Runtime GenAI மாடல் கட்டமைப்பாளர் மூலம் தொகுத்தல்
- ஹார்ட்வேர்-விஷேஷ சுருக்கல் (CPU, NVIDIA GPU, DirectML, WebGPU) மற்றும் பண்புக் குறைப்பு (int4, fp16, bf16)
- Foundry Localக்கு உரையாடல் வார்ப்பு கட்டமைப்பு கோப்புகளை உருவாக்குதல்
- தொகுக்கப்பட்ட மாடல்களை Foundry Local மறுவடிப்பொறியில் சேர்த்தல்
- CLI, REST API மற்றும் OpenAI SDK மூலம் தனிப்பயன் மாடல்களை இயக்குதல்
- மேற்கோள் உதாரணம்: Qwen/Qwen3-0.6B முழு தொகுப்பு

---

### பகுதி 11: உள்ளூர் மாடல்களுடன் கருவி அழைப்பு

**ஆய்வு வழிகாட்டி:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- உள்ளூர் மாடல்களுக்கு வெளிப்புற செயல்பாடுகளை அழைக்க அனுமதித்தல் (கருவி/செயல்பாடு அழைப்பு)
- OpenAI செயல்பாடு அழைப்பு வடிவத்தைப் பயன்படுத்தி கருவி அட்டவணைகளை வரையறுத்தல்
- பல-முற்று கருவி அழைப்பு உரையாடல் ஓட்டத்தை கையாளுதல்
- கருவி அழைப்புகளை உள்ளூரில் இயக்கி முடிவுகளை மாடலுக்கு திருப்பி அளித்தல்
- கருவி அழைப்புக்கான சரியான மாடலைத் தேர்ந்தெடுக்குதல் (Qwen 2.5, Phi-4-mini)
- SDK இன் இயல்புநிலை `ChatClient`-ஐ கருவி அழைப்புக்கு பயன்படுத்துதல் (JavaScript)

**குறியீடு மாதிரிகள்:**

| மொழி | கோப்பு | விவரம் |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | வானிலை/மக்கள் கருவிகளுடன் கருவி அழைப்பு |
| C# | `csharp/ToolCalling.cs` | .NET உடன் கருவி அழைப்பு |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient உடன் கருவி அழைப்பு |

---

### பகுதி 12: Zava Creative Writer க்கான வலை UI உருவாக்கல்

**ஆய்வு வழிகாட்டி:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer க்கான உலாவி அடிப்படையிலான முன் முனையைச் சேர்த்தல்
- Python (FastAPI), JavaScript (Node.js HTTP), மற்றும் C# (ASP.NET Core) மூலம் பகிரப்பட்ட UI சேவை செய்தல்
- Fetch API மற்றும் ReadableStream உடன் உலாவியில் வெளியேற்றப்படும் NDJSON-ஐ பயன்படுத்துதல்
- நேரடி முகவர் நிலை பட்டைகள் மற்றும் நேரடி கட்டுரை உரை ஓட்டல்

**குறியீடு (பகிரப்பட்ட UI):**

| கோப்பு | விவரம் |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | பக்கம் அமைப்பு |
| `zava-creative-writer-local/ui/style.css` | அலங்காரம் |
| `zava-creative-writer-local/ui/app.js` | ஓட்டு வாசகர் மற்றும் DOM புதுப்பித்தல் முனைவு |

**பின்னணி கூடுதல்கள்:**

| மொழி | கோப்பு | விவரம் |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | நிலையான UI சேவைக்கு புதுப்பிக்கப்பட்டது |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ஒருங்கிணைப்பாளரை சுற்றி புதிய HTTP சேவையகம் |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | புதிய ASP.NET Core குறைந்தபட்ச API திட்டம் |

---

### பகுதி 13: பணிமனை நிறைவு

**ஆய்வு வழிகாட்டி:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- அனைத்து 12 பகுதிகளிலும் நீங்கள் கட்டிய அனைத்திற்கும் சுருக்கம்
- உங்கள் பயன்பாடுகளை விரிவுபடுத்துவதற்கான கூடுதல் யோசனைகள்
- வளங்கள் மற்றும் ஆவணங்களுக்கான இணைப்புகள்

---

## திட்ட கட்டமைப்பு

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
| மாடல் பட்டியல் | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| துவக்கம் செய்யும் வழிகாட்டி | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK குறிப்பேடு | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft முகவர் கட்டமைப்பு | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## உரிமம்

இந்த பணிமனைப் பொருள் கல்வித் பயன்பாட்டுக்காக வழங்கப்பட்டுள்ளது.

---

**மகிழ்ச்சியான கட்டுமானம்! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**தயவுசெய்து கவனிக்கவும்**:  
இந்த ஆவணம் AI மொழிபெயர்ப்பு சேவை [Co-op Translator](https://github.com/Azure/co-op-translator) மூலம் மொழிமாற்றம் செய்யப்பட்டுள்ளத. நாம் துல்லியத்திற்காக முயற்சி செய்கிறோம் என்றாலும், தானாக செய்யப்பட்ட மொழிபெயர்ப்புகளில் பிழைகள் அல்லது தவறுதல்கள் இருக்கலாம் என்பதை நண்பருங்கள். அசல் ஆவணம் அதன் மூல மொழியில் அங்கீகாரதார ஆதாரமாக கருதப்பட வேண்டும். முக்கிய தகவல்களுக்கு, தொழில்முறை மனித மொழிபெயர்ப்பு பரிந்துரைக்கப்படுகிறது. இந்த மொழிபெயர்ப்பைப் பயன்படுத்துவதில் ஏற்படும் எந்த தவறான புரிதல்கள் அல்லது தவறான பொருள் எடுத்துக்கொள்ளல்களுக்கு நாங்கள் பொறுப்பு இல்லாமல் இருப்போம்.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->