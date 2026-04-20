# परिवर्तन सूची — Foundry लोकल कार्यशाला

इस कार्यशाला के सभी महत्वपूर्ण परिवर्तनों को नीचे दस्तावेजीकृत किया गया है।

---

## 2026-03-11 — भाग 12 और 13, वेब UI, Whisper पुनर्लेखन, WinML/QNN सुधार, और सत्यापन

### जोड़ा गया
- **भाग 12: Zava क्रिएटिव राइटर के लिए वेब UI बनाना** — नई प्रयोगशाला गाइड (`labs/part12-zava-ui.md`) जिसमें स्ट्रीमिंग NDJSON, ब्राउज़र `ReadableStream`, लाइव एजेंट स्टेटस बैज, और रीयल-टाइम लेख टेक्स्ट स्ट्रीमिंग को कवर करने वाले अभ्यास
- **भाग 13: कार्यशाला पूर्ण** — नया सारांश प्रयोगशाला (`labs/part13-workshop-complete.md`) जिसमें सभी 12 भागों का पुनरावलोकन, आगे के विचार, और संसाधन लिंक शामिल हैं
- **Zava UI फ्रंट एंड:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — साझा किया गया वेनिला HTML/CSS/JS ब्राउज़र इंटरफ़ेस जो तीनों बैकेंड द्वारा उपयोग किया जाता है
- **JavaScript HTTP सर्वर:** `zava-creative-writer-local/src/javascript/server.mjs` — नया Express-शैली HTTP सर्वर जो ब्राउज़र-आधारित एक्सेस के लिए ऑर्केस्ट्रेटर को रैप करता है
- **C# ASP.NET Core बैकेंड:** `zava-creative-writer-local/src/csharp-web/Program.cs` और `ZavaCreativeWriterWeb.csproj` — नया न्यूनतम API प्रोजेक्ट जो UI और स्ट्रीमिंग NDJSON प्रदान करता है
- **ऑडियो नमूना जनरेटर:** `samples/audio/generate_samples.py` — ऑफलाइन TTS स्क्रिप्ट जो Part 9 के लिए Zava-थीम वाले WAV फाइल जनरेट करती है
- **ऑडियो नमूना:** `samples/audio/zava-full-project-walkthrough.wav` — ट्रांसक्रिप्शन परीक्षण के लिए नया लंबा ऑडियो नमूना
- **सत्यापन स्क्रिप्ट:** `validate-npu-workaround.ps1` — सभी C# नमूनों में NPU/QNN वर्कअराउंड को सत्यापित करने के लिए स्वचालित पावरशेल स्क्रिप्ट
- **Mermaid डायग्राम SVG:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML क्रॉस-प्लेटफ़ॉर्म समर्थन:** सभी 3 C# `.csproj` फाइलें (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) अब क्रॉस-प्लेटफ़ॉर्म समर्थन के लिए सशर्त TFM और पारस्परिक रूप से अनन्य पैकेज संदर्भों का उपयोग करती हैं। विंडोज़ पर: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (जिसमें QNN EP प्लगइन शामिल है)। विंडोज़ के बाहर: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (बेस SDK)। Zava प्रोजेक्ट्स में हार्डकोडेड `win-arm64` RID को ऑटो-डिटेक्ट से बदला गया। एक ट्रांजिटिव निर्भरता वर्कअराउंड ने `Microsoft.ML.OnnxRuntime.Gpu.Linux` से नेटिव अस्सेट्स को बाहर रखा, जिसमें टूटी हुई win-arm64 संदर्भ थी। सभी 7 C# फाइलों से पूर्व का try/catch NPU वर्कअराउंड हटा दिया गया है।

### बदला गया
- **भाग 9 (Whisper):** प्रमुख पुनर्लेखन — JavaScript अब मैनुअल ONNX रनटाइम अनुमान के बजाय SDK के बिल्ट-इन `AudioClient` (`model.createAudioClient()`) का उपयोग करता है; JS/C# `AudioClient` दृष्टिकोण बनाम Python ONNX Runtime दृष्टिकोण को दर्शाने के लिए वास्तुकला विवरण, तुलना तालिकाएँ और पाइपलाइन डायग्राम अपडेट किए गए
- **भाग 11:** नेविगेशन लिंक अपडेट किए गए (अब भाग 12 की ओर इशारा करता है); टूल-कॉलिंग फ्लो और अनुक्रम के लिए रेंडर्ड SVG डायग्राम जोड़े गए
- **भाग 10:** नेविगेशन को अपडेट कर भाग 12 के माध्यम से रूट किया गया है बजाय कार्यशाला समाप्त करने के
- **Python Whisper (`foundry-local-whisper.py`):** अतिरिक्त ऑडियो नमूनों के साथ विस्तारित किया गया और त्रुटि हैंडलिंग में सुधार किया गया
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** मैनुअल ONNX Runtime सत्रों के बजाय `model.createAudioClient()` और `audioClient.transcribe()` के साथ पुनर्लेखन किया गया
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** API के साथ-साथ स्थैतिक UI फाइलों को सेवा देने के लिए अपडेट किया गया
- **Zava C# कंसोल (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU वर्कअराउंड हटाया गया (अब WinML पैकेज द्वारा संभाला जाता है)
- **README.md:** कोड नमूना तालिकाओं और बैकेंड परिवर्धन के साथ भाग 12 अनुभाग जोड़ा गया; भाग 13 अनुभाग जोड़ा गया; सीखने के उद्देश्य और प्रोजेक्ट संरचना अपडेट की गई
- **KNOWN-ISSUES.md:** हल किया गया मुद्दा #7 (C# SDK NPU मॉडल वेरिएंट — अब WinML पैकेज द्वारा संभाला जाता है) हटाया गया। शेष मुद्दों को #1 से #6 तक पुनः क्रमांकित किया गया। पर्यावरण विवरण को .NET SDK 10.0.104 के साथ अपडेट किया गया
- **AGENTS.md:** प्रोजेक्ट संरचना पेड़ नया `zava-creative-writer-local` एंट्रीज़ (`ui/`, `csharp-web/`, `server.mjs`) के साथ अपडेट किया गया; C# मुख्य पैकेज और सशर्त TFM विवरण अपडेट किए गए
- **labs/part2-foundry-local-sdk.md:** `.csproj` उदाहरण को पूर्ण क्रॉस-प्लेटफ़ॉर्म पैटर्न के साथ सशर्त TFM, पारस्परिक रूप से अनन्य पैकेज संदर्भों, और व्याख्यात्मक नोट दिखाने के लिए अपडेट किया गया

### सत्यापित
- तीनों C# प्रोजेक्ट सफलतापूर्वक Windows ARM64 पर बनते हैं (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`)
- चैट नमूना (`dotnet run chat`): मॉडल WinML/QNN के माध्यम से `phi-3.5-mini-instruct-qnn-npu:1` के रूप में लोड होता है — NPU वेरिएंट सीधे लोड होता है बिना CPU फेलबैक के
- एजेंट नमूना (`dotnet run agent`): मल्टी-टर्न बातचीत के साथ एंड-टू-एंड चलता है, निकास कोड 0
- Foundry Local CLI v0.8.117 और SDK v0.9.0 .NET SDK 9.0.312 पर

---

## 2026-03-11 — कोड फिक्स, मॉडल क्लीनअप, Mermaid डायग्राम, और सत्यापन

### फिक्स किया गया
- **सभी 21 कोड नमूने (7 Python, 7 JavaScript, 7 C#):** `model.unload()` / `unload_model()` / `model.UnloadAsync()` क्लीनअप को एग्जिट पर जोड़ा गया ताकि OGA मेमोरी लीक चेतावनियां (#4 ज्ञात समस्या) समाप्त हो सकें
- **csharp/WhisperTranscription.cs:** नाज़ुक `AppContext.BaseDirectory` सापेक्ष पाथ को `FindSamplesDirectory()` से बदला गया जो विश्वसनीय रूप से `samples/audio` खोजने के लिए डायरेक्टरीज़ में ऊपर चलता है (#7 ज्ञात समस्या)
- **csharp/csharp.csproj:** हार्डकोडेड `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` को `$(NETCoreSdkRuntimeIdentifier)` के साथ ऑटो-डिटेक्ट फ़ैल-बैक से बदला गया ताकि `dotnet run` किसी भी प्लेटफ़ॉर्म पर बिना `-r` फ्लैग के काम करे ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### बदला गया
- **भाग 8:** ASCII बॉक्स आरेख से रेंडर्ड SVG छवि में इवाल-ड्रिवन पुनरावृत्ति लूप बदला गया
- **भाग 10:** ASCII तीरों से रेंडर्ड SVG में संकलन पाइपलाइन डायग्राम बदला गया
- **भाग 11:** टूल-कॉलिंग फ्लो और अनुक्रम डायग्राम को रेंडर्ड SVG चित्रों में बदला गया
- **भाग 10:** "कार्यशाला पूर्ण!" अनुभाग को भाग 11 (अंतिम प्रयोगशाला) में स्थानांतरित किया गया; इसे "अगले कदम" लिंक से बदला गया
- **KNOWN-ISSUES.md:** सभी मुद्दों का पूर्ण पुनः सत्यापन CLI v0.8.117 के खिलाफ किया गया। हल किए गए: OGA मेमोरी लीक (क्लीनअप जोड़ा गया), Whisper पाथ (FindSamplesDirectory), HTTP 500 स्थायी अनुमान (पुनरुत्पादित नहीं, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), tool_choice सीमाएं (अब "required" और विशिष्ट फ़ंक्शन लक्षित करने के साथ qwen2.5-0.5b पर काम करती हैं)। JS Whisper समस्या अपडेट की गई — अब सभी फाइलें खाली/बाइनरी आउटपुट देती हैं (v0.9.x से रिग्रेशन, गंभीरता बढ़ा कर मुख्य कर दी गई)। #4 C# RID का ऑटो-डिटेक्ट वर्कअराउंड और [#497](https://github.com/microsoft/Foundry-Local/issues/497) लिंक जोड़ा गया। 7 खुले मुद्दे बाकी हैं।
- **javascript/foundry-local-whisper.mjs:** क्लीनअप वैरिएबल नाम ठीक किया (`whisperModel` → `model`)

### सत्यापित
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — क्लीनअप के साथ सफलतापूर्वक चलते हैं
- JavaScript: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — क्लीनअप के साथ सफलतापूर्वक चलते हैं
- C#: `dotnet build` 0 चेतावनी, 0 त्रुटि के साथ सफल (net9.0 लक्ष्य)
- सभी 7 Python फाइलें `py_compile` सिंटैक्स जांच पास करती हैं
- सभी 7 JavaScript फाइलें `node --check` सिंटैक्स मान्यता पास करती हैं

---

## 2026-03-10 — भाग 11: टूल कॉलिंग, SDK API विस्तार, और मॉडल कवरेज

### जोड़ा गया
- **भाग 11: स्थानीय मॉडलों के साथ टूल कॉलिंग** — नई प्रयोगशाला गाइड (`labs/part11-tool-calling.md`) जिसमें 8 अभ्यास शामिल हैं जो टूल स्कीमा, मल्टी-टर्न फ्लो, कई टूल कॉल, कस्टम टूल, ChatClient टूल कॉलिंग, और `tool_choice` को कवर करते हैं
- **Python नमूना:** `python/foundry-local-tool-calling.py` — OpenAI SDK का उपयोग करते हुए `get_weather`/`get_population` टूल कॉलिंग
- **JavaScript नमूना:** `javascript/foundry-local-tool-calling.mjs` — SDK के मूल `ChatClient` (`model.createChatClient()`) का उपयोग करते हुए टूल कॉलिंग
- **C# नमूना:** `csharp/ToolCalling.cs` — OpenAI C# SDK के साथ `ChatTool.CreateFunctionTool()` का उपयोग करते हुए टूल कॉलिंग
- **भाग 2, अभ्यास 7:** मूल `ChatClient` — `model.createChatClient()` (JS) और `model.GetChatClientAsync()` (C#) OpenAI SDK के विकल्प के रूप में
- **भाग 2, अभ्यास 8:** मॉडल वेरिएंट और हार्डवेयर चयन — `selectVariant()`, `variants`, NPU वेरिएंट तालिका (7 मॉडल)
- **भाग 2, अभ्यास 9:** मॉडल उन्नयन और कैटलॉग ताज़ा करना — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **भाग 2, अभ्यास 10:** तर्क मॉडल — `<think>` टैग पार्सिंग उदाहरणों के साथ `phi-4-mini-reasoning`
- **भाग 3, अभ्यास 4:** OpenAI SDK के विकल्प के रूप में `createChatClient`, स्ट्रीमिंग कॉलबैक पैटर्न दस्तावेज़ीकरण के साथ
- **AGENTS.md:** टूल कॉलिंग, ChatClient, और तर्क मॉडल कोडिंग कन्वेंशंस जोड़े गए

### बदला गया
- **भाग 1:** मॉडल कैटलॉग विस्तार — phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo जोड़े गए
- **भाग 2:** API संदर्भ तालिकाएँ विस्तृत की गईं — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync` जोड़े गए
- **भाग 2:** अभ्यास 7-9 से 10-13 तक पुनः क्रमांकित किए गए ताकि नए अभ्यास शामिल किए जा सकें
- **भाग 3:** मुख्य निष्कर्ष तालिका को मूल ChatClient शामिल करने के लिए अपडेट किया गया
- **README.md:** भाग 11 अनुभाग कोड नमूना तालिका के साथ जोड़ा गया; सीखने का उद्देश्य #11 जोड़ा गया; प्रोजेक्ट संरचना पेड़ अपडेट किया गया
- **csharp/Program.cs:** CLI राउटर में `toolcall` केस जोड़ा गया और मदद टेक्स्ट अपडेट किया गया

---

## 2026-03-09 — SDK v0.9.0 अपडेट, ब्रिटिश अंग्रेज़ी, और सत्यापन पास

### बदला गया
- **सभी कोड नमूने (Python, JavaScript, C#):** Foundry Local SDK v0.9.0 API को अपडेट किया गया — `await catalog.getModel()` (जिसमें `await` गायब था) ठीक किया गया, `FoundryLocalManager` इनिशियलाइज़ेशन पैटर्न अपडेट किए गए, एंडपॉइंट डिस्कवरी फिक्स की गई
- **सभी प्रयोगशाला गाइड (भाग 1-10):** ब्रिटिश अंग्रेज़ी में परिवर्तित किया गया (colour, catalogue, optimised, आदि)
- **सभी प्रयोगशाला गाइड:** SDK कोड उदाहरणों को v0.9.0 API सरफेस से मेल करने के लिए अपडेट किया गया
- **सभी प्रयोगशाला गाइड:** API संदर्भ तालिकाएँ और अभ्यास कोड ब्लॉक अपडेट किए गए
- **JavaScript महत्वपूर्ण फिक्स:** `catalog.getModel()` पर गायब `await` जोड़ा गया — जो `Promise` लौटाता था, न कि `Model` ऑब्जेक्ट, जिससे बंद दरवाज़े के नीचे चुपचाप असफलताएं हो रही थीं

### सत्यापित
- सभी Python नमूने Foundry Local सेवा के खिलाफ सफलतापूर्वक चलते हैं
- सभी JavaScript नमूने सफलतापूर्वक चलते हैं (Node.js 18+)
- C# प्रोजेक्ट .NET 9.0 पर बनता और चलता है (net8.0 SDK असेंबली से फॉवार्ड-कंपेट)
- कार्यशाला में 29 फाइलें संशोधित और सत्यापित की गईं

---

## फाइल सूची

| फाइल | अंतिम अपडेट | विवरण |
|------|-------------|-------------|
| `labs/part1-getting-started.md` | 2026-03-10 | विस्तृत मॉडल कैटलॉग |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | नए अभ्यास 7-10, विस्तृत API तालिकाएँ |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | नया अभ्यास 4 (ChatClient), अपडेट किए गए निष्कर्ष |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश अंग्रेज़ी |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश अंग्रेज़ी |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश अंग्रेज़ी |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश अंग्रेज़ी |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | मर्मेड आरेख |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश अंग्रेज़ी |
| `labs/part10-custom-models.md` | 2026-03-11 | मर्मेड आरेख, वर्कशॉप पूर्ण को भाग 11 में स्थानांतरित किया गया |
| `labs/part11-tool-calling.md` | 2026-03-11 | नया लैब, मर्मेड आरेख, वर्कशॉप पूर्ण अनुभाग |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | नया: उपकरण कॉलिंग उदाहरण |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | नया: उपकरण कॉलिंग उदाहरण |
| `csharp/ToolCalling.cs` | 2026-03-10 | नया: उपकरण कॉलिंग उदाहरण |
| `csharp/Program.cs` | 2026-03-10 | `toolcall` CLI कमांड जोड़ा गया |
| `README.md` | 2026-03-10 | भाग 11, परियोजना संरचना |
| `AGENTS.md` | 2026-03-10 | उपकरण कॉलिंग + ChatClient सम्मेलन |
| `KNOWN-ISSUES.md` | 2026-03-11 | हल किए गए Issue #7 को हटाया गया, 6 खुले मुद्दे बचे हैं |
| `csharp/csharp.csproj` | 2026-03-11 | क्रॉस-प्लेटफ़ॉर्म TFM, WinML/बेस SDK शर्तीय संदर्भ |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | क्रॉस-प्लेटफ़ॉर्म TFM, ऑटो-डिटेक्ट RID |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | क्रॉस-प्लेटफ़ॉर्म TFM, ऑटो-डिटेक्ट RID |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU ट्राय/कैच वर्कअराउंड हटाया गया |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU ट्राय/कैच वर्कअराउंड हटाया गया |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU ट्राय/कैच वर्कअराउंड हटाया गया |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU ट्राय/कैच वर्कअराउंड हटाया गया |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU ट्राय/कैच वर्कअराउंड हटाया गया |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | क्रॉस-प्लेटफ़ॉर्म .csproj उदाहरण |
| `AGENTS.md` | 2026-03-11 | C# पैकेज और TFM विवरण अपडेट किए गए |
| `CHANGELOG.md` | 2026-03-11 | यह फ़ाइल |