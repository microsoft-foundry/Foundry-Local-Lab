# चेंजलॉग — फाउन्ड्री लोकल कार्यशाला

यस कार्यशालाका सबै उल्लेखनीय परिवर्तनहरू तल कागजात गरिएका छन्।

---

## 2026-03-11 — भाग 12 & 13, वेब UI, विस्पर पुनलेखन, WinML/QNN सुधार, र प्रमाणीकरण

### थपियो
- **भाग 12: Zava क्रिएटिभ लेखकको लागि वेब UI निर्माण** — नयाँ प्रयोगशाला मार्गदर्शन (`labs/part12-zava-ui.md`) जसमा स्ट्रिमिङ NDJSON, ब्राउजर `ReadableStream`, लाइभ एजेन्ट स्थिति ब्यान्डेज, र रियल टाइम लेख लेख स्ट्रिमिङका अभ्यासहरू समावेश छन्
- **भाग 13: कार्यशाला पूरा** — सबै 12 भागहरूको पुनरावलोकन, थप विचारहरू, र स्रोत लिंकहरू सहित नयाँ सारांश प्रयोगशाला (`labs/part13-workshop-complete.md`)
- **Zava UI फ्रन्ट इन्ड:** `zava-creative-writer-local/ui/index.html`, `style.css`, `app.js` — तिनै ब्याकएन्डहरूद्वारा प्रयोग गरिने साझा भ्यानिला HTML/CSS/JS ब्राउजर इन्टरफेस
- **जाभास्क्रिप्ट HTTP सर्भर:** `zava-creative-writer-local/src/javascript/server.mjs` — ब्राउजर-आधारित पहुँचका लागि ओरकेस्ट्रेटरलाई र्याप गर्ने नयाँ Express-शैली HTTP सर्भर
- **C# ASP.NET कोर ब्याकएन्ड:** `zava-creative-writer-local/src/csharp-web/Program.cs` र `ZavaCreativeWriterWeb.csproj` — UI सेवा गर्ने र स्ट्रिमिङ NDJSON प्रवाह गर्ने नयाँ न्यूनतम API परियोजना
- **अडियो नमूना जेनेरेटर:** `samples/audio/generate_samples.py` — `pyttsx3` प्रयोग गरी पार्ट 9 का लागि Zava-थिम्ड WAV फाइलहरू उत्पादन गर्ने अफ़लाइन TTS स्क्रिप्ट
- **अडियो नमूना:** `samples/audio/zava-full-project-walkthrough.wav` — ट्रान्सक्रिप्सन परीक्षणका लागि नयाँ लामो अडियो नमूना
- **प्रमाणीकरण स्क्रिप्ट:** `validate-npu-workaround.ps1` — सबै C# नमूनाहरूमा NPU/QNN workaround प्रमाणीकरण गर्न स्वचालित PowerShell स्क्रिप्ट
- **Mermaid डायग्राम SVGs:** `images/part12-architecture.svg`, `part12-message-types.svg`, `part12-streaming-sequence.svg`
- **WinML क्रस-प्लेटफर्म समर्थन:** तीनवटा C# `.csproj` फाइलहरू (`csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`, `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj`) ले अब शर्तीय TFM र पारस्परिक रूपमा एकअर्कालाई बाहिर गर्ने प्याकेज सन्दर्भहरू प्रयोग गर्छन् क्रस-प्लेटफर्म समर्थनका लागि। Windows मा: `net9.0-windows10.0.26100` TFM + `Microsoft.AI.Foundry.Local.WinML` (QNN EP प्लगइन सहितको सुपरसट)। गैर-Windows मा: `net9.0` TFM + `Microsoft.AI.Foundry.Local` (आधार SDK)। Zava परियोजनाहरूमा हार्डकोड गरिएको `win-arm64` RID स्वतः पत्ता लगाउन परिवर्तन भयो। एक ट्रान्जिटिभ निर्भरता workaround ले `Microsoft.ML.OnnxRuntime.Gpu.Linux` बाट नेटिभ एस्सेट्स हटाउँछ जसमा तोडिएको win-arm64 रेफरेन्स छ। अघिल्लो try/catch NPU workaround सबै 7 वटा C# फाइलबाट हटाइयो।

### परिवर्तनहरू
- **भाग 9 (Whisper):** ठूलो पुनलेखन — जाभास्क्रिप्टले अब SDK को बिल्ट-इन `AudioClient` (`model.createAudioClient()`) प्रयोग गर्छ म्यानुअल ONNX Runtime inference सट्टा; JS/C# `AudioClient` तरिका र Python ONNX Runtime तरिकाबीच तुलना वर्णन, तालिका, र पाइपलाइन डायग्रामहरू अपडेट गरिए
- **भाग 11:** नेविगेसन लिंकहरू अपडेट गरियो (अहिले भाग 12 तर्फ संकेत गर्दछ); उपकरण कल प्रवाह र अनुक्रमका लागि रेंडर गरिएको SVG डायग्राम थपियो
- **भाग 10:** नेविगेसन अपडेट गरियो जसले कार्यशाला अन्त्य नगरेर भाग 12 मार्फत मार्गनिर्देशन गर्छ
- **Python Whisper (`foundry-local-whisper.py`):** थप अडियो नमूनाहरू र सुधारिएको त्रुटि ह्यान्डलिङसहित विस्तार गरियो
- **JavaScript Whisper (`foundry-local-whisper.mjs`):** `model.createAudioClient()` र `audioClient.transcribe()` प्रयोग गरी पुर्नलेखन गरियो, म्यानुअल ONNX Runtime सेसनको सट्टा
- **Python FastAPI (`zava-creative-writer-local/src/api/main.py`):** API सँगै स्थिर UI फाइलहरू सेवा गर्न अपडेट गरियो
- **Zava C# कन्सोल (`zava-creative-writer-local/src/csharp/Program.cs`):** NPU workaround हटाइयो (अहिले WinML प्याकेजले ह्यान्डल गर्छ)
- **README.md:** पार्ट 12 अनुभाग कोड नमूना तालिका र ब्याकएन्ड थपहरू सहित थपियो; पार्ट 13 अनुभाग थपियो; सिकाइ उद्देश्य र परियोजना संरचना अपडेट गरियो
- **KNOWN-ISSUES.md:** समाधान गरिएका Issue #7 (C# SDK NPU Model Variant — अब WinML प्याकेजले ह्यान्डल गर्छ) हटाइयो। बाँकी समस्याहरू #1–#6 क्रमबद्ध गरियो। .NET SDK 10.0.104 सहित वातावरण विवरण अपडेट गरियो
- **AGENTS.md:** परियोजना संरचना वृक्ष नवीन `zava-creative-writer-local` प्रविष्टिहरू (`ui/`, `csharp-web/`, `server.mjs`) सहित अपडेट गरियो; C# प्रमुख प्याकेज र शर्तीय TFM विवरण अपडेट गरियो
- **labs/part2-foundry-local-sdk.md:** `.csproj` उदाहरण पूर्ण क्रस-प्लेटफर्म ढाँचासहित, शर्तीय TFM, पारस्परिक रूपमा एकअर्कालाई बहिर्गमन गर्ने प्याकेज सन्दर्भहरू, र व्याख्यात्मक नोट देखाउन अपडेट गरियो

### प्रमाणीकरण गरियो
- तीनवटै C# परियोजनाहरू (`csharp`, `ZavaCreativeWriter`, `ZavaCreativeWriterWeb`) Windows ARM64 मा सफलतापूर्वक बिल्ड भए
- च्याट नमूना (`dotnet run chat`): मोडेल WinML/QNN मार्फत `phi-3.5-mini-instruct-qnn-npu:1` रूपले लोड भयो — NPU भेरियन्ट CPU फालब्याक बिना सिधै लोड भयो
- एजेन्ट नमूना (`dotnet run agent`): बहु टर्न कुराकानीसहित अन्त्य-देखि-अन्त्य सम्म चल्यो, निकास कोड 0
- Foundry Local CLI v0.8.117 र SDK v0.9.0 .NET SDK 9.0.312 मा

---

## 2026-03-11 — कोड सुधारहरू, मोडेल सफाइ, Mermaid डायग्रामहरू, र प्रमाणीकरण

### ठीक गरियो
- **सबै 21 कोड नमूनाहरू (7 Python, 7 JavaScript, 7 C#):** `model.unload()` / `unload_model()` / `model.UnloadAsync()` लाई निकासमा थपियो ताकि OGA मेमोरी लीक चेतावनीहरू (Known Issue #4) समाधान गरियो
- **csharp/WhisperTranscription.cs:** नाजुक `AppContext.BaseDirectory` सापेक्ष पथलाई `FindSamplesDirectory()` ले प्रतिस्थापन गर्यो जसले `samples/audio` लाई भरपर्दो रूपमा फेला पार्छ (Known Issue #7)
- **csharp/csharp.csproj:** हार्डकोड गरिएको `<RuntimeIdentifier>win-arm64</RuntimeIdentifier>` लाई `$(NETCoreSdkRuntimeIdentifier)` प्रयोग गरी स्वतः पत्ता लगाउने फालब्याकले प्रतिस्थापन गरियो जसले `dotnet run` लाई कुनै प्लेटफर्ममा `-r` झण्डा बिना काम गर्न दिन्छ ([Foundry-Local#497](https://github.com/microsoft/Foundry-Local/issues/497))

### परिवर्तनहरू
- **भाग 8:** ASCII बक्स चित्रबाट रेंडर गरिएको SVG छविमा इवाल-चालित दोहोर्याउने लूप रूपान्तरण गरियो
- **भाग 10:** ASCII तीरहरूबाट कम्पाइल पाइपलाइन चित्र रेंडर गरिएको SVG छवि बनाइयो
- **भाग 11:** उपकरण कल प्रवाह र अनुक्रम डायग्रामहरू रेंडर गरिएको SVG छविमा रूपान्तरण गरियो
- **भाग 10:** "कार्यशाला पूरा!" खण्ड भाग 11 (अन्तिम प्रयोगशाला) तिर सारियो; यसको सट्टा "अर्को चरण" लिंक राखियो
- **KNOWN-ISSUES.md:** CLI v0.8.117 विरुद्ध सबै मुद्दाहरू पुन: प्रमाणीकरण गरियो। समाधान भएका हटाइए: OGA मेमोरी लीक (सफा गरियो), Whisper पथ (FindSamplesDirectory), HTTP 500 sustained inference (फर्कि दोहोर्याउन सकिएन, [#494](https://github.com/microsoft/Foundry-Local/issues/494)), टुल_च्वाइस सीमितता (अहिले `"required"` र qwen2.5-0.5b मा विशिष्ट फङ्क्शन लक्ष्यीकरणका साथ काम गर्छ)। JS Whisper मुद्दा अपडेट गरियो — अब सबै फाइलहरूले खाली/बाइनरी आउटपुट फिर्ता गर्छन् (v0.9.x बाट रिग्रेसन, गम्भीरता 'महत्वपूर्ण' बढाइयो)। #4 C# RID लाई स्वतः पत्ता लगाउँने workaround र [#497](https://github.com/microsoft/Foundry-Local/issues/497) लिंकसहित अपडेट गरियो। 7 खुला मुद्दा बाँकी छन्।
- **javascript/foundry-local-whisper.mjs:** क्लिनअप भेरिएबल नाम ठीक गरियो (`whisperModel` → `model`)

### प्रमाणीकरण गरियो
- Python: `foundry-local.py`, `foundry-local-rag.py`, `foundry-local-tool-calling.py` — क्लिनअप सहित सफलतापूर्वक चले
- जाभास्क्रिप्ट: `foundry-local.mjs`, `foundry-local-rag.mjs`, `foundry-local-tool-calling.mjs` — क्लिनअप सहित सफलतापूर्वक चले
- C#: `dotnet build` ले 0 चेतावनी, 0 त्रुटि सहित सफल भयो (net9.0 लक्ष्य)
- सबै 7 Python फाइलहरूले `py_compile` सिn्ट्याक्स जाँच पास गर्यो
- सबै 7 JavaScript फाइलहरूले `node --check` सिn्ट्याक्स मान्यकरण पास गर्यो

---

## 2026-03-10 — भाग 11: टुल कलिङ, SDK API विस्तार, र मोडेल कभरेज

### थपियो
- **भाग 11: लोकल मोडेलहरूसँग टुल कलिङ** — नयाँ प्रयोगशाला मार्गदर्शन (`labs/part11-tool-calling.md`) जसमा 8 अभ्यासहरू टुल स्कीमाहरू, बहु टर्न फ्लो, धेरै टुल कलहरू, कस्टम टुलहरू, ChatClient टुल कलिङ, र `tool_choice` समेट्छन्
- **Python नमूना:** `python/foundry-local-tool-calling.py` — OpenAI SDK प्रयोग गरी `get_weather`/`get_population` टुल कलिङ
- **जाभास्क्रिप्ट नमूना:** `javascript/foundry-local-tool-calling.mjs` — SDK को नेटिभ `ChatClient` (`model.createChatClient()`) प्रयोग गरी टुल कलिङ
- **C# नमूना:** `csharp/ToolCalling.cs` — OpenAI C# SDK सँग `ChatTool.CreateFunctionTool()` प्रयोग गरी टुल कलिङ
- **भाग 2, अभ्यास 7:** नेटिभ `ChatClient` — `model.createChatClient()` (JS) र `model.GetChatClientAsync()` (C#) खुलाAI SDK का वैकल्पिक रूपमा
- **भाग 2, अभ्यास 8:** मोडेल भेरियन्ट र हार्डवेयर चयन — `selectVariant()`, `variants`, NPU भेरियन्ट तालिका (7 मोडेलहरू)
- **भाग 2, अभ्यास 9:** मोडेल अपग्रेड र सूची नवीकरण — `is_model_upgradeable()`, `upgrade_model()`, `updateModels()`
- **भाग 2, अभ्यास 10:** तर्क मोडेलहरू — `<think>` ट्याग पार्सिङ उदाहरणहरू सहित `phi-4-mini-reasoning`
- **भाग 3, अभ्यास 4:** OpenAI SDK को वैकल्पिक रूपमा `createChatClient` र स्ट्रिमिङ कलब्याक ढाँचाको कागजात
- **AGENTS.md:** टुल कलिङ, ChatClient, र तर्क मोडेलहरूको कोडिङ सम्मेलनहरू थपियो

### परिवर्तनहरू
- **भाग 1:** मोडेल सूची विस्तार — phi-4-mini-reasoning, gpt-oss-20b, phi-4, qwen2.5-7b, qwen2.5-coder-7b, whisper-large-v3-turbo थपियो
- **भाग 2:** API संदर्भ तालिका विस्तार — `createChatClient`, `createAudioClient`, `removeFromCache`, `selectVariant`, `variants`, `isLoaded`, `stopWebService`, `is_model_upgradeable`, `upgrade_model`, `httpx_client`, `getModels`, `getCachedModels`, `getLoadedModels`, `updateModels`, `GetModelVariantAsync`, `UpdateModelsAsync` थपियो
- **भाग 2:** अभ्यास 7-9 लाई 10-13 मा पुनः क्रमबद्ध गरियो नयाँ अभ्यासहरू समायोजन गर्न
- **भाग 3:** मुख्य सिकाइ तालिकामा नेटिभ ChatClient समावेश गरियो
- **README.md:** भाग 11 अनुभाग कोड नमूना तालिका सहित थपियो; सिकाइ उद्देश्य #11 थपियो; परियोजना संरचना वृक्ष अपडेट गरियो
- **csharp/Program.cs:** CLI राउटरमा `toolcall` केस थपियो र सहयता पाठ अपडेट गरियो

---

## 2026-03-09 — SDK v0.9.0 अपडेट, ब्रिटिश अंग्रेजी, र प्रमाणीकरण पास

### परिवर्तनहरू
- **सामान्य सबै कोड नमूनाहरू (Python, JavaScript, C#):** Foundry Local SDK v0.9.0 API अनुसार अपडेट गरियो — `await catalog.getModel()` (अपूर्व `await` हरायो) सुधारियो, `FoundryLocalManager` init ढाँचाहरू अपडेट गरियो, एन्डपोइन्ट खोजी ठीक गरियो
- **सबै प्रयोगशाला मार्गदर्शनहरू (भाग 1-10):** ब्रिटिश अंग्रेजीमा रूपान्तरण (colour, catalogue, optimised आदि)
- **सबै प्रयोगशाला मार्गदर्शनहरू:** SDK कोड उदाहरणहरू v0.9.0 API सतहसँग मेल खाने गरी अपडेट गरियो
- **सबै प्रयोगशाला मार्गदर्शनहरू:** API संदर्भ तालिका र अभ्यास कोड ब्लकहरू अपडेट गरियो
- **जाभास्क्रिप्ट महत्वपूर्ण सुधार:** `catalog.getModel()` मा हराएको `await` थपियो — पहिले `Promise` फिर्ता गर्थ्यो जुनले गुप्त असफलताहरू निम्त्याउँथ्यो

### प्रमाणीकरण गरियो
- सबै Python नमूनाहरू Foundry Local सेवा विरुद्ध सफलतापूर्वक चले
- सबै जाभास्क्रिप्ट नमूनाहरू (Node.js 18+) सफलतापूर्वक चले
- C# परियोजना .NET 9.0 मा बिल्ड र चल्यो (net8.0 SDK बाट फर्वार्ड-कम्प्याट)
- कार्यशालाभरि 29 फाइलहरू परिवर्तन र प्रमाणीकरण गरियो

---

## फाइल सूची

| फाइल | अन्तिम अपडेट | विवरण |
|------|-------------|--------|
| `labs/part1-getting-started.md` | 2026-03-10 | मोडेल सूची विस्तार |
| `labs/part2-foundry-local-sdk.md` | 2026-03-10 | नयाँ अभ्यास 7-10, API तालिका विस्तार |
| `labs/part3-sdk-and-apis.md` | 2026-03-10 | नयाँ अभ्यास 4 (ChatClient), अपडेटेड सिकाइ सारांश |
| `labs/part4-rag-fundamentals.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश अंग्रेजी |
| `labs/part5-single-agents.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटिश अंग्रेजी |
| `labs/part6-multi-agent-workflows.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटीश इंग्रजी |
| `labs/part7-zava-creative-writer.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटीश इंग्रजी |
| `labs/part8-evaluation-led-development.md` | 2026-03-11 | मरमेड डायग्राम |
| `labs/part9-whisper-voice-transcription.md` | 2026-03-09 | SDK v0.9.0 + ब्रिटीश इंग्रजी |
| `labs/part10-custom-models.md` | 2026-03-11 | मरमेड डायग्राम, वर्कशप कम्प्लीट पार्ट ११ मा सारियो |
| `labs/part11-tool-calling.md` | 2026-03-11 | नयाँ ल्याब, मरमेड डायग्रामहरू, वर्कशप कम्प्लीट सेक्शन |
| `python/foundry-local-tool-calling.py` | 2026-03-10 | नयाँ: टुल कलिंग नमूना |
| `javascript/foundry-local-tool-calling.mjs` | 2026-03-10 | नयाँ: टुल कलिंग नमूना |
| `csharp/ToolCalling.cs` | 2026-03-10 | नयाँ: टुल कलिंग नमूना |
| `csharp/Program.cs` | 2026-03-10 | थपियो `toolcall` CLI कमाण्ड |
| `README.md` | 2026-03-10 | पार्ट ११, प्रोजेक्ट संरचना |
| `AGENTS.md` | 2026-03-10 | टुल कलिंग + ChatClient नियमहरू |
| `KNOWN-ISSUES.md` | 2026-03-11 | समाधान गरिएको मुद्दा #7 हटाइयो, ६ खुलेका मुद्दाहरू बाँकी छन् |
| `csharp/csharp.csproj` | 2026-03-11 | क्रस-प्ल्याटफर्म TFM, WinML/base SDK सर्तिया रेफ |
| `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj` | 2026-03-11 | क्रस-प्ल्याटफर्म TFM, RID स्वचालित पहिचान |
| `zava-creative-writer-local/src/csharp-web/ZavaCreativeWriterWeb.csproj` | 2026-03-11 | क्रस-प्ल्याटफर्म TFM, RID स्वचालित पहिचान |
| `csharp/BasicChat.cs` | 2026-03-11 | NPU try/catch समाधान हटाइयो |
| `csharp/SingleAgent.cs` | 2026-03-11 | NPU try/catch समाधान हटाइयो |
| `csharp/MultiAgent.cs` | 2026-03-11 | NPU try/catch समाधान हटाइयो |
| `csharp/RagPipeline.cs` | 2026-03-11 | NPU try/catch समाधान हटाइयो |
| `csharp/AgentEvaluation.cs` | 2026-03-11 | NPU try/catch समाधान हटाइयो |
| `labs/part2-foundry-local-sdk.md` | 2026-03-11 | क्रस-प्ल्याटफर्म .csproj उदाहरण |
| `AGENTS.md` | 2026-03-11 | अपडेट गरिएका C# प्याकेजहरू र TFM विवरणहरू |
| `CHANGELOG.md` | 2026-03-11 | यो फाइल |

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:
यस दस्तावेजलाई AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) प्रयोग गरेर अनुवाद गरिएको हो। हामी शुद्धताका लागि प्रयासरत छौं तापनि, कृपया जानकार हुनुस् कि स्वचालित अनुवादमा त्रुटिहरू वा गलतफहमी हुन सक्छन्। मूल दस्तावेज यसको मूल भाषामा आधिकारिक स्रोत मानिनु पर्नेछ। महत्वपूर्ण जानकारीको लागि व्यावसायिक मानव अनुवाद सिफारिस गरिन्छ। यस अनुवादको प्रयोगबाट उत्पन्न कुनै पनि भ्रम वा गलत अर्थ लगाउने स्थितिको लागि हामी जिम्मेवार हुने छैनौं।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->