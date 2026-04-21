# कोडिंग एजंट सूचना

हा फाइल या रेपॉजिटरीमध्ये कार्यरत AI कोडिंग एजंटसाठी (GitHub Copilot, Copilot Workspace, Codex, इ.) संदर्भ प्रदान करतो.

## प्रोजेक्ट विहंगावलोकन

हा [Foundry Local](https://foundrylocal.ai) सह AI अॅप्लिकेशन्स तयार करण्यासाठी एक **प्रायोगिक कार्यशाळा** आहे — एक हलके वजनाचे रनटाइम जे संपूर्णपणे डिव्हाइसवर भाषिक मॉडेल्स डाउनलोड, व्यवस्थापित आणि OpenAI-सुसंगत API द्वारे सेवा देते. कार्यशाळेमध्ये पायथन, जावास्क्रिप्ट आणि C# मध्ये टप्प्याटप्प्याने लॅब मार्गदर्शक आणि चालवता येणारे कोड नमुने आहेत.

## रेपॉजिटरी रचना

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## भाषा आणि फ्रेमवर्क तपशील

### पायथन
- **स्थान:** `python/`, `zava-creative-writer-local/src/api/`
- **अवलंबित्व:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **मुख्य पॅकेजेस:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **किमान आवृत्ती:** Python 3.9+
- **चालवा:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### जावास्क्रिप्ट
- **स्थान:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **अवलंबित्व:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **मुख्य पॅकेजेस:** `foundry-local-sdk`, `openai`
- **मॉड्यूल प्रणाली:** ES मॉड्यूल (`.mjs` फाइल्स, `"type": "module"`)
- **किमान आवृत्ती:** Node.js 18+
- **चालवा:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **स्थान:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **प्रोजेक्ट फाइल्स:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **मुख्य पॅकेजेस:** `Microsoft.AI.Foundry.Local` (नॉन-विंडोज), `Microsoft.AI.Foundry.Local.WinML` (विंडोज — QNN EP सह सुपरसॅट), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **लक्ष्य:** .NET 9.0 (सशर्त TFM: विंडोजवर `net9.0-windows10.0.26100`, इतरत्र `net9.0`)
- **चालवा:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## कोडिंग नियम

### सामान्य
- सर्व कोड नमुने **स्वतःमध्ये संपूर्ण असलेले एकल फाइलचे उदाहरण** आहेत — कोणतीही सामायिक युटिलिटी लायब्ररी किंवा अमूर्तता नाही.
- प्रत्येक नमुना स्वतंत्रपणे चालतो त्याचे स्वतंत्र अवलंबित्व स्थापित केल्यानंतर.
- API किज नेहमी `"foundry-local"` असा सेट असतात — Foundry Local हा एक प्लेसहोल्डर म्हणून वापरतो.
- बेस URL `http://localhost:<port>/v1` वापरतात — पोर्ट डायनामिक असून SDK द्वारे रनटाइममध्ये शोधला जातो (`manager.urls[0]` JS मध्ये, `manager.endpoint` पायथनमध्ये).
- Foundry Local SDK सेवा सुरू आणि एंडपॉइंट शोधण्याचे व्यवस्थापन करतो; हार्डकोडेड पोर्ट्सच्या तुलनेत SDK पॅटर्नस प्राधान्य द्या.

### पायथन
- `openai` SDK वापरा `OpenAI(base_url=..., api_key="not-required")` सह.
- SDK-व्यवस्थापित सेवा जीवनचक्रासाठी `foundry_local` मधील `FoundryLocalManager()` वापरा.
- स्ट्रिमिंग: `stream` ऑब्जेक्टवर `for chunk in stream:` वापरून पुनरावृत्ती करा.
- नमुना फाइल्समध्ये टाईप अ‍ॅनोटेशन नाहीत (कार्यशाळेतील शिकणाऱ्यांसाठी नमुने संक्षिप्त ठेवा).

### जावास्क्रिप्ट
- ES मॉड्यूल सिंटॅक्स: `import ... from "..."`.
- `"openai"` मधून `OpenAI` आणि `"foundry-local-sdk"` मधून `FoundryLocalManager` वापरा.
- SDK सुरूवात नमुना: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- स्ट्रिमिंग: `for await (const chunk of stream)`
- टॉप-लेव्हल `await` सर्वत्र वापरले जाते.

### C#
- Nullable सक्षम, implicit usings, .NET 9.
- SDK-व्यवस्थापित जीवनचक्रसाठी `FoundryLocalManager.StartServiceAsync()` वापरा.
- स्ट्रिमिंग: `CompleteChatStreaming()` सह `foreach (var update in completionUpdates)`.
- मुख्य `csharp/Program.cs` हे CLI राउटर आहे जे स्थिर `RunAsync()` पद्धतींना कॉल करते.

### टूल कॉलिंग
- केवळ काही मॉडेल्स टूल कॉलिंगला समर्थन देतात: **Qwen 2.5** कुटुंब (`qwen2.5-*`) आणि **Phi-4-mini** (`phi-4-mini`).
- टूल स्कीमास OpenAI फंक्शन-कॉलिंग JSON फॉर्मॅटचा वापर करतात (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- संभाषण मल्टी-टर्न पॅटर्न वापरतो: user → assistant (tool_calls) → tool (results) → assistant (final answer).
- टूल निकाल संदेशांतील `tool_call_id` मॉडेलच्या टूल कॉलमधील `id` शी जुळायला हवा.
- पायथन थेट OpenAI SDK वापरतो; जावास्क्रिप्ट SDK चा देशी `ChatClient` (`model.createChatClient()`); C# OpenAI SDK सह `ChatTool.CreateFunctionTool()` वापरतो.

### ChatClient (देशी SDK क्लायंट)
- जावास्क्रिप्ट: `model.createChatClient()` चे परतावा `ChatClient` ज्यात `completeChat(messages, tools?)` आणि `completeStreamingChat(messages, callback)` असते.
- C#: `model.GetChatClientAsync()` एक सामान्य `ChatClient` देतो ज्याचा वापर OpenAI NuGet पॅकेज आयात न करता होतो.
- पायथनमध्ये देशी ChatClient नाही — OpenAI SDK वापरा `manager.endpoint` आणि `manager.api_key` सह.
- **महत्वाचे:** जावास्क्रिप्ट `completeStreamingChat` **कॉलबॅक पॅटर्न** वापरतो, async पुनरावृत्ती नाही.

### विचार करणारे मॉडेल्स
- `phi-4-mini-reasoning` अंतिम उत्तराआधी `<think>...</think>` टॅग्जमध्ये त्याचे विचार लपेटतो.
- उत्तरापासून वेगळे करण्यासाठी टॅग्ज पार्स करा.

## लॅब मार्गदर्शक

लॅब फाईल्स `labs/` मध्ये मार्कडाउन म्हणून असतात. त्यांची संरचना एकसारखी आहे:
- लोगो हेडर प्रतिमा
- शीर्षक आणि उद्दिष्ट कॉलआउट
- विहंगावलोकन, शिका उद्दिष्टे, पूर्वअट्स
- संकल्पना समजावणारे विभाग आणि आरेखे
- क्रमांकित व्यायाम, कोड ब्लॉक्स आणि अपेक्षित आउटपुट
- सारांश तक्ता, मुख्य बिंदू, पुढील वाचन
- पुढील भागावर नेण्यासाठी लिंक

लॅब सामग्री संपादित करताना:
- पूर्वीचा मार्कडाउन स्वरूप आणि विभाग रचना जपावी.
- कोड ब्लॉकमध्ये भाषेचा उल्लेख करावा (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- शेल कमांडसाठी OS आवश्यक असल्यास bash आणि PowerShell दोन्ही प्रकार द्यावेत.
- `> **Note:**`, `> **Tip:**`, आणि `> **Troubleshooting:**` कॉलआउट शैली वापरा.
- तक्ते `| Header | Header |` पाइप फॉरमॅटचे पालन करतील.

## बिल्ड आणि चाचणी आदेश

| क्रिया | आदेश |
|--------|---------|
| **पायथन नमुने** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS नमुने** | `cd javascript && npm install && node <script>.mjs` |
| **C# नमुने** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **आरेख तयार करा** | `npx mmdc -i <input>.mmd -o <output>.svg` (मुळात `npm install` आवश्यक) |

## बाह्य अवलंबित्वे

- **Foundry Local CLI** विकसकाच्या संगणकावर स्थापित असणे आवश्यक (`winget install Microsoft.FoundryLocal` किंवा `brew install foundrylocal`).
- **Foundry Local सेवा** स्थानिकपणे चालते व डायनामिक पोर्टवर OpenAI-सुसंगत REST API उपलब्ध करून देते.
- कोणतीही क्लाउड सेवा, API कीज किंवा Azure सदस्यता कोणताही नमुना चालवण्यासाठी आवश्यक नाही.
- भाग 10 (कस्टम मॉडेल्स) साठी `onnxruntime-genai` आणि Hugging Face वरून मॉडेल एकत्र डाउनलोड करणे आवश्यक आहे.

## जे फायली कमिट करू नयेत

`.gitignore` ने खालील गोष्टी वगळाव्यात (आणि बहुतेक वेळा वगळतो):
- `.venv/` — पायथन वर्चुअल एन्व्हायर्नमेंट्स
- `node_modules/` — npm अवलंबित्वे
- `models/` — संकलित ONNX मॉडेल आउटपुट (मोठ्या बायनरी फाइल्स, भाग 10 मध्ये तयार)
- `cache_dir/` — Hugging Face मॉडेल डाउनलोड कॅश
- `.olive-cache/` — Microsoft Olive काम करणारी डायरेक्टरी
- `samples/audio/*.wav` — तयार केलेली ऑडिओ नमुने (`python samples/audio/generate_samples.py` मार्फत पुन्हा तयार होतात)
- मानक पायथन बिल्ड आर्टिफॅक्ट्स (`__pycache__/`, `*.egg-info/`, `dist/`, इत्यादी)

## अनुज्ञप्ती

MIT — पहा `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:  
हा दस्तऐवज AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) वापरून भाषांतरित केला आहे. जरी आम्ही अचूकतेसाठी प्रयत्न करत असलो तरी, कृपया लक्षात ठेवा की स्वयंचलित अनुवादांमध्ये चुका किंवा गैरसमज असू शकतात. मूळ दस्तऐवज त्याच्या स्थानिक भाषेत अधिकृत स्रोत मानला जावा. महत्त्वाच्या माहितीसाठी व्यावसायिक मानवी अनुवाद शिफारसीय आहे. या अनुवादाच्या वापरातून झालेल्या कोणत्याही गैरसमज वा चुकीसाठी आम्ही जबाबदार नाही आहोत.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->