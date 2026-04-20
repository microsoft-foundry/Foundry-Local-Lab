# कोडिंग एजेंट निर्देश

यह फ़ाइल इस रिपॉज़िटरी में काम करने वाले AI कोडिंग एजेंट्स (GitHub Copilot, Copilot Workspace, Codex, आदि) के लिए संदर्भ प्रदान करती है।

## प्रोजेक्ट अवलोकन

यह [Foundry Local](https://foundrylocal.ai) के साथ AI अनुप्रयोग बनाने के लिए एक **व्यावहारिक कार्यशाला** है — एक हल्का रनटाइम जो पूरी तरह से ऑन-डिवाइस भाषा मॉडल को डाउनलोड, प्रबंधित और सेवा प्रदान करता है, एक OpenAI-संगत API के माध्यम से। कार्यशाला में चरण-दर-चरण लैब गाइड और पायथन, जावास्क्रिप्ट और C# में चलाने योग्य कोड नमूने शामिल हैं।

## रिपॉज़िटरी संरचना

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

## भाषा एवं फ्रेमवर्क विवरण

### पायथन
- **स्थान:** `python/`, `zava-creative-writer-local/src/api/`
- **निर्भरता:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **प्रमुख पैकेज:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **न्यूनतम संस्करण:** Python 3.9+
- **चलाने का तरीका:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### जावास्क्रिप्ट
- **स्थान:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **निर्भरता:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **प्रमुख पैकेज:** `foundry-local-sdk`, `openai`
- **मॉड्यूल सिस्टम:** ES मॉड्यूल्स (`.mjs` फाइलें, `"type": "module"`)
- **न्यूनतम संस्करण:** Node.js 18+
- **चलाने का तरीका:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **स्थान:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **प्रोजेक्ट फाइलें:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **प्रमुख पैकेज:** `Microsoft.AI.Foundry.Local` (non-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — QNN EP सहित), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **लक्षित प्लेटफ़ॉर्म:** .NET 9.0 (शर्तीय TFM: `net9.0-windows10.0.26100` Windows पर, अन्यथा `net9.0`)
- **चलाने का तरीका:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## कोडिंग कन्वेंशन्स

### सामान्य
- सभी कोड नमूने **स्वतंत्र एक-फाइल उदाहरण** हैं — कोई साझा उपयोगिता लाइब्रेरी या अमूर्तता नहीं।
- प्रत्येक नमूना अपने निर्भरता स्थापित करने के बाद स्वतंत्र रूप से चलता है।
- API कुंजी हमेशा `"foundry-local"` सेट की जाती है — Foundry Local इसके रूप में प्लेसहोल्डर उपयोग करता है।
- बेस URL `http://localhost:<port>/v1` का उपयोग करते हैं — पोर्ट गतिशील है और SDK के ज़रिए रनटाइम में खोजा जाता है (`manager.urls[0]` JS में, `manager.endpoint` पायथन में)।
- Foundry Local SDK सेवा स्टार्टअप और एंडपॉइंट खोज को संभालता है; हार्ड-कोडेड पोर्ट की तुलना में SDK पैटर्न को प्राथमिकता दें।

### पायथन
- `openai` SDK का उपयोग करें `OpenAI(base_url=..., api_key="not-required")` के साथ।
- SDK-प्रबंधित सेवा लाइफसाइकल के लिए `foundry_local` से `FoundryLocalManager()` का उपयोग करें।
- स्ट्रीमिंग: `stream` ऑब्जेक्ट के ऊपर `for chunk in stream:` लूप का उपयोग करें।
- नमूना फ़ाइलों में कोई टाइप एनोटेशन नहीं (कार्यशाला शिक्षार्थियों के लिए संक्षिप्त रखें)।

### जावास्क्रिप्ट
- ES मॉड्यूल सिंटैक्स: `import ... from "..."`.
- `"openai"` से `OpenAI` और `"foundry-local-sdk"` से `FoundryLocalManager` का उपयोग करें।
- SDK इनिशियलाइज़ पैटर्न: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`।
- स्ट्रीमिंग: `for await (const chunk of stream)`।
- शीर्ष-स्तरीय `await` पूरे कोड में उपयोग होता है।

### C#
- Nullable सक्षम, इम्प्लिसिट usings, .NET 9।
- SDK-प्रबंधित लाइफसाइकल के लिए `FoundryLocalManager.StartServiceAsync()` का उपयोग करें।
- स्ट्रीमिंग: `CompleteChatStreaming()` के साथ `foreach (var update in completionUpdates)`।
- मुख्य `csharp/Program.cs` CLI राउटर है जो स्थैतिक `RunAsync()` विधियों को डिस्पैच करता है।

### टूल कॉलिंग
- केवल कुछ मॉडल टूल कॉलिंग का समर्थन करते हैं: **Qwen 2.5** परिवार (`qwen2.5-*`) और **Phi-4-mini** (`phi-4-mini`)।
- टूल स्कीमा OpenAI फ़ंक्शन-कॉलिंग JSON प्रारूप का पालन करते हैं (`type: "function"`, `function.name`, `function.description`, `function.parameters`)।
- वार्तालाप एक बहु-राउंड पैटर्न का उपयोग करता है: उपयोगकर्ता → सहायक (tool_calls) → टूल (परिणाम) → सहायक (अंतिम उत्तर)।
- टूल परिणाम संदेशों में `tool_call_id` को मॉडल के टूल कॉल के `id` से मेल खाना चाहिए।
- पायथन सीधे OpenAI SDK का उपयोग करता है; जावास्क्रिप्ट SDK के नेटिव `ChatClient` का (`model.createChatClient()`) उपयोग करता है; C# OpenAI SDK के साथ `ChatTool.CreateFunctionTool()` उपयोग करता है।

### ChatClient (नेटिव SDK क्लाइंट)
- जावास्क्रिप्ट: `model.createChatClient()` एक `ChatClient` लौटाता है जिसमें `completeChat(messages, tools?)` और `completeStreamingChat(messages, callback)` होता है।
- C#: `model.GetChatClientAsync()` एक मानक `ChatClient` लौटाता है जिसका उपयोग OpenAI NuGet पैकेज के बिना किया जा सकता है।
- पायथन में नेटिव ChatClient नहीं है — `manager.endpoint` और `manager.api_key` के साथ OpenAI SDK का उपयोग करें।
- **महत्वपूर्ण:** जावास्क्रिप्ट `completeStreamingChat` एक **कॉलबैक पैटर्न** का उपयोग करता है, न कि async iteration।

### तर्क-मॉडल
- `phi-4-mini-reasoning` अंतिम उत्तर से पहले `<think>...</think>` टैग में सोच को संलग्न करता है।
- आवश्यकता होने पर उत्तर से तर्क अलग करने के लिए टैग पार्स करें।

## लैब गाइड्स

लैब फाइलें `labs/` में Markdown में हैं। वे एक सुसंगत संरचना का पालन करती हैं:
- लोगो हेडर छवि
- शीर्षक और लक्ष्य कॉलआउट
- अवलोकन, सीखने के उद्देश्य, पूर्वापेक्षाएँ
- अवधारणा व्याख्या अनुभाग आरेखों के साथ
- कोड ब्लॉक्स और अपेक्षित आउटपुट वाले अनुक्रमित अभ्यास
- सारांश तालिका, मुख्य निष्कर्ष, आगे पढ़ाई
- अगले भाग के लिए नेविगेशन लिंक

लैब सामग्री संपादित करते समय:
- मौजूदा Markdown फॉर्मेटिंग शैली और अनुभाग पदानुक्रम बनाए रखें।
- कोड ब्लॉकों में भाषा निर्दिष्ट करें (`python`, `javascript`, `csharp`, `bash`, `powershell`)।
- शेल कमांड के लिए bash और PowerShell दोनों वेरिएंट प्रदान करें जहाँ OS महत्वपूर्ण हो।
- `> **Note:**`, `> **Tip:**`, और `> **Troubleshooting:**` कॉलआउट शैलियाँ उपयोग करें।
- तालिकाएं `| हेडर | हेडर |` पाइप फ़ॉर्मेट में बनाएं।

## बिल्ड और टेस्ट कमांड्स

| क्रिया | कमांड |
|--------|---------|
| **पायथन नमूने** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS नमूने** | `cd javascript && npm install && node <script>.mjs` |
| **C# नमूने** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava पायथन** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (वेब)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (वेब)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **आरेख बनाने के लिए** | `npx mmdc -i <input>.mmd -o <output>.svg` (रूट पर `npm install` आवश्यक) |

## बाहरी निर्भरताएँ

- **Foundry Local CLI** डेवलपर की मशीन पर स्थापित होना चाहिए (`winget install Microsoft.FoundryLocal` या `brew install foundrylocal`)।
- **Foundry Local सेवा** स्थानीय रूप से चलती है और डायनेमिक पोर्ट पर OpenAI-संगत REST API प्रदर्शित करती है।
- किसी भी नमूने को चलाने के लिए क्लाउड सेवाएं, API कुंजी या Azure सदस्यता आवश्यक नहीं हैं।
- भाग 10 (कस्टम मॉडल) के लिए अतिरिक्त रूप से `onnxruntime-genai` और Hugging Face से मॉडल वज़न डाउनलोड करने की आवश्यकता होती है।

## वे फाइलें जिन्हें कमिट नहीं करना चाहिए

`.gitignore` इसमें शामिल करने चाहिए (और अधिकांश के लिए करता है):
- `.venv/` — पायथन वर्चुअल एनवायरनमेंट्स
- `node_modules/` — npm निर्भरता
- `models/` — संकलित ONNX मॉडल आउटपुट (बड़े बाइनरी फाइलें, भाग 10 द्वारा उत्पन्न)
- `cache_dir/` — Hugging Face मॉडल डाउनलोड कैश
- `.olive-cache/` — माइक्रोसॉफ्ट ऑलिव वर्किंग डायरेक्टरी
- `samples/audio/*.wav` — उत्पन्न ऑडियो नमूने (`python samples/audio/generate_samples.py` के जरिए पुन: उत्पन्न)
- मानक पायथन बिल्ड आर्टिफेक्ट्स (`__pycache__/`, `*.egg-info/`, `dist/`, आदि)

## लाइसेंस

MIT — देखें `LICENSE`।