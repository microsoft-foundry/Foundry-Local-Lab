# कोडिङ एजेन्ट निर्देशनहरू

यस फाइलले यस रिपोजिटरीमा काम गर्ने AI कोडिङ एजेन्टहरू (GitHub Copilot, Copilot Workspace, Codex, आदि) का लागि सन्दर्भ प्रदान गर्दछ।

## परियोजना अवलोकन

यो [Foundry Local](https://foundrylocal.ai) सँग AI अनुप्रयोगहरू बनाउनको लागि **हातेमालो कार्यशाला** हो — एक हल्का रनटाइम जसले भाषा मोडेलहरू पूर्ण रूपमा डिभाइसमा एक OpenAI-अनुकूल API मार्फत डाउनलोड, व्यवस्थापन, र सेवा गर्दछ। कार्यशालामा Python, JavaScript, र C# मा चरण-द्वारा-चरण प्रयोगशाला गाइडहरू र चलाउन मिल्ने कोड नमूनाहरू समावेश छन्।

## रिपोजिटरी संरचना

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

## भाषा र फ्रेमवर्क विवरणहरू

### Python
- **स्थान:** `python/`, `zava-creative-writer-local/src/api/`
- **आश्रितहरू:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **मुख्य प्याकेजहरू:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **कम्तीमा संस्करण:** Python 3.9+
- **चलाउने तरिका:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **स्थान:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **आश्रितहरू:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **मुख्य प्याकेजहरू:** `foundry-local-sdk`, `openai`
- **मोड्युल प्रणाली:** ES मोड्युलहरू (`.mjs` फाइलहरू, `"type": "module"`)
- **कम्तीमा संस्करण:** Node.js 18+
- **चलाउने तरिका:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **स्थान:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **परियोजना फाइलहरू:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **मुख्य प्याकेजहरू:** `Microsoft.AI.Foundry.Local` (नन-विन्डोज), `Microsoft.AI.Foundry.Local.WinML` (विन्डोज — QNN EP सहित), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **लक्ष्य:** .NET 9.0 (सशर्त TFM: विन्डोजमा `net9.0-windows10.0.26100`, अन्यत्र `net9.0`)
- **चलाउने तरिका:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## कोडिङ परम्पराहरू

### सामान्य
- सबै कोड नमूनाहरू **आत्मनिर्भर एकल-फाइल उदाहरणहरू** हुन् — साझा युटिलिटी पुस्तकालय वा सारहरू छैनन्।
- प्रत्येक नमूना आफ्नै निर्भरताहरू इन्स्टल गरेपछि स्वतन्त्र रूपमा चल्छ।
- API कुञ्जीहरू सधैं `"foundry-local"` मा सेट हुन्छन् — Foundry Local ले यसलाई प्लेसहोल्डरको रूपमा प्रयोग गर्छ।
- बेस URL हरू `http://localhost:<port>/v1` प्रयोग गर्छन् — पोर्ट गतिशील हुन्छ र रनटाइममा SDK मार्फत पत्ता लगाइन्छ (`manager.urls[0]` JS मा, `manager.endpoint` Python मा)।
- Foundry Local SDK ले सेवा सुरु र एन्डपोइन्ट पत्ता लगाउने काम ह्यान्डल गर्छ; हाम्रा हार्ड-कोडेड पोर्टहरूको सट्टामा SDK ढाँचाहरू प्राथमिकता दिनुहोस्।

### Python
- `OpenAI(base_url=..., api_key="not-required")` सहित `openai` SDK प्रयोग गर्नुहोस्।
- SDK-व्यवस्थापन गरिएको सेवा जीवनचक्रको लागि `foundry_local` बाट `FoundryLocalManager()` प्रयोग गर्नुहोस्।
- स्ट्रिमिङ: `stream` वस्तुमा `for chunk in stream:` मार्फत पुनरावृत्ति गर्नुहोस्।
- नमूना फाइलहरूमा प्रकार एनोटेशनहरू छैनन् (कार्यशाला सिक्नेहरूका लागि नमूनाहरू संक्षिप्त राख्नुहोस्)।

### JavaScript
- ES मोड्युल सिन्ट्याक्स: `import ... from "..."`।
- `openai` बाट `OpenAI` र `foundry-local-sdk` बाट `FoundryLocalManager` प्रयोग गर्नुहोस्।
- SDK सुरु गर्ने तरिका: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`।
- स्ट्रिमिङ: `for await (const chunk of stream)`।
- शीर्ष-स्तरीय `await` सधैं प्रयोग हुन्छ।

### C#
- Nullable सक्षम, implicit usings, .NET 9।
- SDK-व्यवस्थापन गरिएको जीवनचक्रको लागि `FoundryLocalManager.StartServiceAsync()` प्रयोग गर्नुहोस्।
- स्ट्रिमिङ: `CompleteChatStreaming()` मा `foreach (var update in completionUpdates)`।
- मुख्य `csharp/Program.cs` CLI राउटर हो जसले स्थिर `RunAsync()` विधिहरूमा डिस्प्याच गर्छ।

### टुल कलिङ
- केवल निश्चित मोडेलहरू टुल कलिङ समर्थन गर्छन्: **Qwen 2.5** परिवार (`qwen2.5-*`) र **Phi-4-mini** (`phi-4-mini`)।
- टुल स्कीमाहरू OpenAI फंक्शन-कलिङ JSON ढाँचामा छन् (`type: "function"`, `function.name`, `function.description`, `function.parameters`)।
- संवाद बहु-टर्न ढाँचामा हुन्छ: प्रयोगकर्ता → सहायक (टुल_कल) → टुल (परिणामहरू) → सहायक (अन्तिम उत्तर)।
- टुल परिणाम सन्देशहरूमा `tool_call_id` मोडेलको टुल कलबाट आएको `id` सँग मिल्नुपर्छ।
- Python ले OpenAI SDK सीधा प्रयोग गर्छ; JavaScript SDK को नेटिभ `ChatClient` (`model.createChatClient()`); C# OpenAI SDK सँग `ChatTool.CreateFunctionTool()` प्रयोग गर्छ।

### ChatClient (नेटिभ SDK क्लाइंट)
- JavaScript: `model.createChatClient()` ले `ChatClient` फिर्ता गर्छ जसमा `completeChat(messages, tools?)` र `completeStreamingChat(messages, callback)` हुन्छ।
- C#: `model.GetChatClientAsync()` ले मानक `ChatClient` फिर्ता गर्छ जुन OpenAI NuGet प्याकेज आयात नगरी पनि प्रयोग गर्न सकिन्छ।
- Python सँग नेटिभ ChatClient छैन — `manager.endpoint` र `manager.api_key` सहित OpenAI SDK प्रयोग गर्नुहोस्।
- **महत्त्वपूर्ण:** JavaScript को `completeStreamingChat` **कॉलब्याक ढाँचा** प्रयोग गर्छ, async iteration होइन।

### तर्क मोडेलहरू
- `phi-4-mini-reasoning` ले अन्तिम उत्तर अघि `<think>...</think>` ट्यागहरूमा आफ्नो सोचलाई समेट्छ।
- आवश्यक भएमा ट्यागहरू पार्स गरेर तर्कलाई उत्तरबाट अलग गर्नुहोस्।

## प्रयोगशाला गाइडहरू

प्रयोगशाला फाइलहरू `labs/` मा Markdown रूपमा छन्। तिनीहरूले समान संरचना पालना गर्दछन्:
- लोगो हेडर छवि
- शीर्षक र लक्ष्य कॉलआउट
- अवलोकन, सिकाइ उद्देश्यहरू, पूर्वापेक्षाहरू
- परिकल्पना व्याख्या खण्डहरू र आरेखहरू
- क्रमबद्ध अभ्यासहरू कोड ब्लकहरू र अपेक्षित आउटपुटसहित
- सारांश तालिका, मुख्य सिकाइहरू, थप पढाइ
- अर्को भागमा नेभिगेसन लिंक

प्रयोगशाला सामग्री सम्पादन गर्दा:
- विद्यमान Markdown ढाँचाको शैली र खण्ड संरचनालाई कायम राख्नुहोस्।
- कोड ब्लकहरू भाषामा निर्दिष्ट हुनुपर्छ (`python`, `javascript`, `csharp`, `bash`, `powershell`)।
- शेल आदेशहरूका लागि OS अनुसार दुबै bash र PowerShell भेरिएन्टहरू प्रदान गर्नुहोस्।
- `> **Note:**`, `> **Tip:**`, र `> **Troubleshooting:**` कलआउट शैलीहरू प्रयोग गर्नुहोस्।
- तालिकाहरू `| Header | Header |` पाइप ढाँचामा हुन्छन्।

## निर्माण र परीक्षण आदेशहरू

| कार्य | आदेश |
|--------|---------|
| **Python नमूनाहरू** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS नमूनाहरू** | `cd javascript && npm install && node <script>.mjs` |
| **C# नमूनाहरू** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (वेब)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (वेब)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **आरेखहरू उत्पन्न गर्नुहोस्** | `npx mmdc -i <input>.mmd -o <output>.svg` (मूल `npm install` आवश्यक) |

## बाह्य आश्रितहरू

- **Foundry Local CLI** विकासकर्ताको मेसिनमा इन्स्टल हुनुपर्छ (`winget install Microsoft.FoundryLocal` वा `brew install foundrylocal`)।
- **Foundry Local सेवा** स्थानीय रूपमा चल्छ र गतिशील पोर्टमा OpenAI-अनुकूल REST API खुला गर्दछ।
- कुनै पनि नमूना चलाउन क्लाउड सेवाहरू, API कुञ्जीहरू वा Azure सदस्यताहरू आवश्यक पर्दैन।
- भाग 10 (अनुकूलन मोडेलहरू) का लागि `onnxruntime-genai` आवश्यक छ र Hugging Face बाट मोडेल तौलहरू डाउनलोड गर्दछ।

## फाइलहरू जुन कमिट गर्नु हुँदैन

`.gitignore` ले (र अधिकांश अवस्थामा गर्छ):
- `.venv/` — Python भर्चुअल वातावरणहरू
- `node_modules/` — npm आश्रितहरू
- `models/` — कम्पाइल गरिएका ONNX मोडेल आउटपुट (ठूला द्वि-आधारित फाइलहरू, भाग 10 द्वारा उत्पन्न)
- `cache_dir/` — Hugging Face मोडेल डाउनलोड क्यासे
- `.olive-cache/` — Microsoft Olive कार्य डाइरेक्टरी
- `samples/audio/*.wav` — उत्पन्न गरिएको अडियो नमूनाहरू (`python samples/audio/generate_samples.py` मार्फत पुनःउत्पन्न)
- मानक Python निर्माण सामग्रीहरू (`__pycache__/`, `*.egg-info/`, `dist/`, आदि)

## अनुज्ञप्ति

MIT — `LICENSE` हेर्नुहोस्।

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:
यस दस्तावेजलाई AI अनुवाद सेवाको माध्यमबाट अनुवाद गरिएको छ [Co-op Translator](https://github.com/Azure/co-op-translator)। हामी शुद्धताको प्रयास गर्छौं, तर कृपया ध्यान दिनुहोस् कि स्वचालित अनुवादमा त्रुटिहरू वा असत्यताहरू हुन सक्छन्। मूल दस्तावेज यसको मूल भाषामा नै आधिकारिक स्रोत मानिन्छ। महत्वपूर्ण जानकारीको लागि व्यावसायिक मानिस द्वारा गरिएको अनुवाद सिफारिस गरिन्छ। यस अनुवादको प्रयोगबाट उत्पन्न हुने कुनै पनि गलतफहमी वा गलत व्याख्याहरूको लागि हामी जिम्मेवार छैनौं।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->