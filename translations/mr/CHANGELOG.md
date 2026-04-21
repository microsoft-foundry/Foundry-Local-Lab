# चेंजलॉग — फाउंड्री लोकल वर्कशॉप

या वर्कशॉपमधील सर्व महत्वाची बदल खाली नमूद आहेत.

---

## 2026-03-11 — भाग 12 & 13, वेब UI, व्हिस्पर रीरायट, WinML/QNN फिक्स, आणि सत्यापन

### जोडले
- **भाग 12: झावा क्रिएटिव्ह लेखकासाठी वेब UI तयार करणे** — नवीन लॅब गाइड (`labs/part12-zava-ui.md`) ज्यात स्ट्रिमिंग NDJSON, ब्राउझर `ReadableStream`, लाईव्ह एजंट स्टेटस बॅजेस, आणि रिअल-टाइम लेखन मजकूर स्ट्रिमिंग या व्यायामांचा समावेश
- **भाग 13: वर्कशॉप संपूर्ण** — नवीन सारांश लॅब (`labs/part13-workshop-complete.md`) ज्यात १२ भागांचा पुनरावलोकन, पुढील कल्पना, आणि संसाधन दुवे आहेत
- **झावा UI फ्रंटएन्ड:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — सर्व तीन बॅकएंडद्वारे वापरले जाणारे शेअर्ड व्हॅनिला HTML/CSS/JS ब्राउझर इंटरफेस
- **जावास्क्रिप्ट HTTP सर्व्हर:** `zava-creative-writer-local/src/javascript/server.mjs` — ब्राउझर-आधारित प्रवेशासाठी ऑर्केस्ट्रेटरला झाकणारा नवीन Express-शैलीचा HTTP सर्व्हर
- **C# ASP.NET कोर बॅकएन्ड:** `zava-creative-writer-local/src/csharp-web/Program.cs` आणि `ZavaCreativeWriterWeb.csproj` — UI आणि स्ट्रिमिंग NDJSON सेवा देणारा नवीन मिमिमल API प्रकल्प
- **ऑडिओ सॅम्पल जनरेटर:** `samples/audio/generate_samples.py` — भाग 9 साठी झावा थीमवरील WAV फाइल्स तयार करण्यासाठी `pyttsx3` वापरून ऑफलाइन TTS स्क्रिप्ट
- **ऑडिओ सॅम्पल:** `samples/audio/zava-full-project-walkthrough.wav` — भाष्य चाचणीसाठी नवीन लांब ऑडिओ सॅम्पल
- **सत्यापन स्क्रिप्ट:** `validate-npu-workaround.ps1` — सर्व C# सॅम्पल्समध्ये NPU/QNN वर्कअराउंडचे ऑटोमेटेड पॉवरशेल स्क्रिप्ट वापरून सत्यापन
- **मर्मिड आकृती SVGs:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML क्रॉस-प्लॅटफॉर्म सपोर्ट:** सर्व 3 C# `.csproj` फाइल्स (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) आता क्रॉस-प्लॅटफॉर्म सपोर्टसाठी कंडिशनल TFM आणि परस्पर वगळल्या जाणार्‍या पॅकेज संदर्भ वापरतात. विंडोजवर: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (ज्यात QNN EP प्लगइन समाविष्ट आहे). नॉन-विंडोजवर: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (बेस SDK). झावा प्रकल्पांमधील हार्डकोड केलेला `win-arm64` RID ऑटो-डिटेक्टने बदलला गेला आहे. एका ट्रान्झिटिव्ह डिपेंडन्सी वर्कअराउंडने `Microsoft.ML.OnnxRuntime.Gpu.Linux` येथून स्थानिक मालमत्ता वगळल्या आहेत ज्याचा `win-arm64` संदर्भ तुटलेला आहे. मागील try/catch NPU वर्कअराउंड सर्व 7 C# फाइल्समधून काढून टाकले गेले.

### बदलले
- **भाग 9 (व्हिस्पर):** मोठा रीरायट — जावास्क्रिप्टने आता मॅन्युअल ONNX Runtime इन्फरन्सऐवजी SDK चा अंगभूत `AudioClient` (`model.createAudioClient()`) वापरला आहे; JS/C# `AudioClient` पद्धत विरुद्ध Python ONNX Runtime पद्धती यांच्या संदर्भातील आर्किटेक्चर वर्णन, तुलना तक्ते, आणि पाइपलाईन आकृत्या अपडेट केल्या
- **भाग 11:** नेव्हिगेशन दुवे अपडेट केले (आता भाग 12 कडे निर्देशित); टूल कॉलिंग फ्लो आणि सिक्वेन्ससाठी SVG आकृत्या अडकविल्या
- **भाग 10:** नेव्हिगेशन आता भाग 12 कडे चालवते वर्कशॉप समाप्तीऐवजी
- **Python Whisper (`foundry-local-whisper.py`):** अतिरिक्त ऑडिओ सॅम्पल्स आणि सुधारित त्रुटी हाताळणीसह वाढवले
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** मॅन्युअल ONNX Runtime सेशन्सऐवजी `model.createAudioClient()` वापरून `audioClient.transcribe()` ने रीरायट केले
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** API सोबत स्थिर UI फाइल्स सर्व्ह करू शकण्या करता अपडेट केले
- **Zava C# कन्सोल (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU वर्कअराउंड काढून टाकले (आता WinML पॅकेजकडून हाताळले जाते)
- **README.md:** भाग 12 विभाग कोड सॅम्पल तक्ते आणि बॅकएन्ड जोडण्यांसह जोडला; भाग 13 विभाग जोडा; शिक्षण उद्दिष्टे आणि प्रकल्प संरचना अपडेट केली
- **KNOWN-ISSUES.md:** निराकरण झालेले समस्यांक #7 काढले (C# SDK NPU मॉडेल व्हेरिएंट - आता WinML पॅकेजकडून हाताळले); उर्वरित समस्या #1–#6 असा पुनर्मुखवटा करा; .NET SDK 10.0.104 सह वातावरण तपशील अपडेट केला
- **AGENTS.md:** नवीन `zava-creative-writer-local` एंट्रीज (`ui/`, `csharp-web/`, `server.mjs`) सह प्रकल्प संरचना वृक्ष अपडेट; C# मुख्य पॅकेजेस व कंडिशनल TFM तपशील अद्ययावत केले
- **labs/part2-foundry-local-sdk.md:** कंडिशनल TFM, परस्पर वगळलेले पॅकेज संदर्भ, आणि स्पष्टीकरणात्मक नोटसह संपूर्ण क्रॉस-प्लॅटफॉर्म पॅटर्न दाखविण्यास `.csproj` उदाहरण अपडेट केले

### सत्यापित
- सर्व 3 C# प्रकल्प (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) विंडोज ARM64 वर यशस्वीपणे बिल्ड झाले
- चॅट सॅम्पल (`dotnet run chat`): WinML/QNN मार्फत `phi-3.5-mini-instruct-qnn-npu:1` मॉडेल लोड होतो — NPU व्हेरिएंट CPU फॉलबॅकशिवाय थेट लोड होते
- एजंट सॅम्पल (`dotnet run agent`): मल्टी-टर्न संभाषणासह एंड-टू-एंड चालतो, बाहेर पडण्याचा कोड 0
- Foundry Local CLI v0.8.117 आणि SDK v0.9.0 .NET SDK 9.0.312 वर

---

## 2026-03-11 — कोड फिक्सेस, मॉडेल क्लीनअप, मर्मिड डायग्रॅम्स, आणि सत्यापन

### दुरुस्त केले
- **सर्व 21 कोड सॅम्पल्स (7 Python, 7 JavaScript, 7 C#):** बाहेर पडताना `model.unload()` / `unload_model()` / `model.UnloadAsync()` क्लीनअप जोडले जेणेकरून OGA मेमरी लीक वॉर्निंग्ज (तज्ञ समस्या #4) दूर होतात
- **csharp/WhisperTranscription.cs:** सापडणारा अपाय होणारा `AppContext.BaseDirectory` रिलेटिव्ह पाथ काढून टाकून `FindSamplesDirectory()` ने बदलले जे `samples/audio` विश्वासार्हरित्या शोधते (तज्ञ समस्या #7)
- **csharp/csharp.csproj:** हार्डकोड `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` काढून टाकले आणि `$(NETCoreSdkRuntimeIdentifier)` वापरून ऑटो-डिटेक्ट फॉलबॅक दिला त्यामुळे `dotnet run` कोणत्याही प्लॅटफॉर्मवर `-r` फ्लॅगशिवाय काम करतो ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### बदलले
- **भाग 8:** इव्हाल-चालित पुनरावृत्ती लूप ASCII बॉक्स आकृतीऐवजी रेंडर्ड SVG प्रतिमेमध्ये परिवर्तित केला
- **भाग 10:** संकलन पाइपलाइन आकृती ASCII बाणांऐवजी रेंडर्ड SVG प्रतिमेमध्ये परिवर्तित
- **भाग 11:** टूल कॉलिंग फ्लो आणि सिक्वेन्स डायग्राम्स रेंडर्ड SVG प्रतिमांमध्ये रूपांतरित
- **भाग 10:** "वर्कशॉप संपूर्ण!" विभाग भाग 11 (शेवटचा लॅब) कडे हलविला; त्याऐवजी "पुढचे पावले" दुवा ठेवला
- **KNOWN-ISSUES.md:** CLI v0.8.117 विरुद्ध सर्व समस्या पूर्णपणे पुनसत्यापन; निराकरण झालेल्या गोष्टी काढल्या: OGA मेमरी लीक (क्लीनअप जोडले), Whisper पाथ (FindSamplesDirectory), HTTP 500 टिकाऊ इन्फरन्स (पुन्हा तयार केला नाही, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), टूल_चॉइस मर्यादा (आता `"required"` आणि विशिष्ट फंक्शन लक्ष्यांसाठी qwen2.5-0.5b वर काम करते). JS Whisper समस्या अद्ययावत केली — आता सर्व फाइल्स रिक्त/बायनरी आउटपुट परत करतात (v0.9.x मधील रिग्रेशन, गंभीरता मोठी). #4 C# RID ऑटो-डिटेक्ट वर्कअराउंडसह अद्ययावत आणि [#497](https://github.com/microsoft/Foundry-Local/issues/497) दुवा. 7 उर्वरित खुल्या समस्या.
- **javascript/foundry-local-whisper.mjs:** क्लीनअप व्हेरीएबल नाव दुरुस्त केले (`whisperModel` → `model`)

### सत्यापित
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — क्लीनअपसह यशस्वीपणे चालले
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — क्लीनअपसह यशस्वीपणे चालले
- C#: `dotnet build` 0 चेतावणी, 0 त्रुटींसह यशस्वी (net9.0 लक्ष्य)
- सर्व 7 Python फाइल्स `py_compile` सिंटॅक्स चाचणी उत्तीर्ण
- सर्व 7 JavaScript फाइल्स `node --check` सिंटॅक्स प्रमाणीकरण उत्तीर्ण

---

## 2026-03-10 — भाग 11: टूल कॉलिंग, SDK API विस्तार, आणि मॉडेल कव्हरेज

### जोडले
- **भाग 11: लोकल मॉडेलसह टूल कॉलिंग** — नवीन लॅब गाइड (`labs/part11-tool-calling.md`) ज्यात 8 व्यायामांचा समावेश आहे ज्यात टूल स्कीमा, मल्टी-टर्न फ्लो, अनेक टूल कॉल्स, कस्टम टूल्स, ChatClient टूल कॉलिंग, आणि `tool_choice`
- **Python सॅम्पल:** `python/foundry-local-tool-calling.py` — OpenAI SDK वापरून `get_weather`/`get_population` टूल कॉलिंग
- **JavaScript सॅम्पल:** `javascript/foundry-local-tool-calling.mjs` — SDK च्या नैसर्गिक `ChatClient` (`model.createChatClient()`) वापरून टूल कॉलिंग
- **C# सॅम्पल:** `csharp/ToolCalling.cs` — OpenAI C# SDK वापरून `ChatTool.CreateFunctionTool()` द्वारा टूल कॉलिंग
- **भाग 2, व्यायाम 7:** नैसर्गिक `ChatClient` — `model.createChatClient()` (JS) आणि `model.GetChatClientAsync()` (C#), OpenAI SDK च्या पर्यायांप्रमाणे
- **भाग 2, व्यायाम 8:** मॉडेल व्हेरिएंट आणि हार्डवेअर निवड — `selectVariant()`, `variants`, NPU व्हेरिएंट तक्ता (7 मॉडेल्स)
- **भाग 2, व्यायाम 9:** मॉडेल अपग्रेड आणि कॅटलॉग रिफ्रेश — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **भाग 2, व्यायाम 10:** रिझनिंग मॉडेल्स — `<think>` टॅग पार्सिंग उदाहरणांसह `phi-4-mini-reasoning`
- **भाग 3, व्यायाम 4:** OpenAI SDK चा पर्याय म्हणून `createChatClient`, स्ट्रिमिंग कॉलबॅक पॅटर्न दस्तऐवजीकरणासह
- **AGENTS.md:** टूल कॉलिंग, ChatClient, आणि रिझनिंग मॉडेल्ससाठी कोडिंग कन्व्हेन्शन्स जोडले

### बदलले
- **भाग 1:** मॉडेल कॅटलॉग वाढवले — `phi-4-mini-reasoning`, `gpt-oss-20b`, `phi-4`, `qwen2.5-7b`, `qwen2.5-coder-7b`, `whisper-large-v3-turbo` जोडले
- **भाग 2:** API रेफरन्स तक्ते वाढवले — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync` जोडले
- **भाग 2:** व्यायाम क्रमांक 7-9 → 10-13 नवे व्यायाम समायोजित करण्यासाठी पुनर्मुखवटा
- **भाग 3:** की टेकअवे टेबलमध्ये नैसर्गिक ChatClient समाविष्ट केला
- **README.md:** भाग 11 विभाग कोड सॅम्पल टेबलसह जोडला; शिक्षण उद्दिष्ट #11 जोडले; प्रकल्प संरचना वृक्ष अद्ययावत केला
- **csharp/Program.cs:** CLI राउटरमध्ये `toolcall` केस जोडला आणि मदत मजकूर अद्ययावत केला

---

## 2026-03-09 — SDK v0.9.0 अपडेट, ब्रिटिश इंग्रजी, आणि सत्यापन

### बदलले
- **सर्व कोड सॅम्पल्स (Python, JavaScript, C#):** Foundry Local SDK v0.9.0 API ला अपडेट केले — `await catalog.getModel()` (पूर्वी `await` गायब होते) दुरुस्त केले, `FoundryLocalManager` प्रारंभ पॅटर्न्स अद्ययावत केले, एन्डपीओईंट शोध दुरुस्त केला
- **सर्व लॅब गाइड्स (भाग 1-10):** ब्रिटीश इंग्रजीत रूपांतरित (colour, catalogue, optimised आदि)
- **सर्व लॅब गाइड्स:** SDK कोड उदाहरणे v0.9.0 API सर्फेसशी जुळवून घेतली
- **सर्व लॅब गाइड्स:** API रेफरन्स तक्ता आणि व्यायाम कोड ब्लॉक्स अपडेट केले
- **जावास्क्रिप्ट गंभीर दुरुस्ती:** `catalog.getModel()` वर हरवलेले `await` जोडले — `Promise` परत करत होते `Model` ऑब्जेक्ट नाही, कारण downstream साइलेंट फेल्युअर्स

### सत्यापित
- सर्व Python सॅम्पल्स Foundry Local सेवा विरुद्ध यशस्वीपणे चालले
- सर्व JavaScript सॅम्पल्स यशस्वीपणे (Node.js 18+)
- C# प्रकल्प .NET 9.0 वर बनतो आणि चालतो (net8.0 SDK असेंब्लीसाठी फॉरवर्ड-कंपॅट)
- वर्कशॉपमध्ये 29 फायली बदलल्या आणि सत्यापित केल्या

---

## फायली सूची

| फायली | शेवटचा अद्ययावत | वर्णन |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | मॉडेल कॅटलॉग विस्तारले |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | नवीन व्यायाम 7-10, API तक्ता विस्तारले |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | नवीन व्यायाम 4 (ChatClient), टेकअवे अपडेट |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश इंग्रजी |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश इंग्रजी |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश इंग्रजी |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश इंग्रजी |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | मर्मिड डायग्राम |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश इंग्रजी |
| `labs/part10-custom-models.md` | 2026-03-11 | मर्मिड डायग्राम, Workshop Complete भाग 11 कडे हलवले |
| `labs/part11-tool-calling.md` | 2026-03-11 | नवीन लॅब, मर्मिड डायग्राम, Workshop Complete विभाग |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | नवीन: टूल कॉलिंग नमुना |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | नवीन: टूल कॉलिंग नमुना |
| `csharp/ToolCalling.cs` | 2026-03-10 | नवीन: टूल कॉलिंग नमुना |
| `csharp/Program.cs` | 2026-03-10 | `toolcall` CLI कमांड जोडले |
| `README.md` | 2026-03-10 | भाग 11, प्रकल्प संरचना |
| `AGENTS.md` | 2026-03-10 | टूल कॉलिंग + ChatClient नियम |
| `KNOWN-ISSUES.md` | 2026-03-11 | सोडवलेले Issue #7 काढले, 6 उघडे मुद्दे शिल्लक |
| `csharp/csharp.csproj` | 2026-03-11 | क्रॉस-प्लॅटफॉर्म TFM, WinML/base SDK सशर्त संदर्भ |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | क्रॉस-प्लॅटफॉर्म TFM, RID आपोआप शोधा |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | क्रॉस-प्लॅटफॉर्म TFM, RID आपोआप शोधा |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU try/catch workaround काढले |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU try/catch workaround काढले |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU try/catch workaround काढले |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU try/catch workaround काढले |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU try/catch workaround काढले |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | क्रॉस-प्लॅटफॉर्म .csproj उदाहरण |
| `AGENTS.md` | 2026-03-11 | C# पॅकेजेस आणि TFM तपशील अद्ययावत केले |
| `CHANGELOG.md` | 2026-03-11 | हा फाइल |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:  
हा दस्तऐवज AI भाषांतर सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) वापरून भाषांतरित केला आहे. आम्ही अचूकतेसाठी प्रयत्नशील आहोत, तरी कृपया लक्षात ठेवा की स्वयंचलित भाषांतरांमध्ये चुका किंवा अचूकतेची कमतरता असू शकते. मूळ दस्तऐवज त्याच्या मूळ भाषेत अधिकारप्राप्त स्त्रोत मानला जाणारा आहे. महत्त्वपूर्ण माहितीसाठी, व्यावसायिक मानवी भाषांतराची शिफारस केली जाते. या भाषांतराच्या वापरातून उद्भवलेल्या कोणत्याही गैरसमजुती किंवा चुकीच्या अर्थलागी आम्ही जबाबदार नाही.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->