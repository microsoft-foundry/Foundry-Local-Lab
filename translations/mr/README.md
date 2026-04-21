<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local कार्यशाळा - डिव्हाइसवर AI अॅप्स तयार करा

आपल्या स्वत:च्या मशीनीवर भाषा मॉडेल चालवण्यासाठी आणि [Foundry Local](https://foundrylocal.ai) आणि [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) सह बुद्धिमान अनुप्रयोग तयार करण्यासाठी एक हॅण्ड्स-ऑन कार्यशाळा.

> **Foundry Local म्हणजे काय?** Foundry Local हा एक हलका रनटाइम आहे जो आपल्याला भाषा मॉडेल पूर्णपणे आपल्या हार्डवेअरवर डाउनलोड, व्यवस्थापित आणि सर्व्ह करण्यास अनुमती देतो. हे एक **OpenAI-सुसंगत API** उघडतो, जेणेकरून OpenAI बोलणारे कोणतेही साधन किंवा SDK कनेक्ट करू शकतात - कोणत्याही क्लाउड खात्याची आवश्यकता नाही.

---

## शिकण्याचे उद्दिष्टे

या कार्यशाळेच्या शेवटी आपण सक्षम असाल:

| # | उद्दिष्ट |
|---|-----------|
| 1 | Foundry Local इंस्टॉल करा आणि CLI द्वारे मॉडेल व्यवस्थापित करा |
| 2 | Foundry Local SDK API प्रोग्रामॅटिक मॉडेल व्यवस्थापनासाठी पारंगत व्हा |
| 3 | Python, JavaScript, आणि C# SDK वापरून स्थानिक इंफरन्स सर्व्हरशी कनेक्ट करा |
| 4 | Retrieval-Augmented Generation (RAG) पाइपलाइन तयार करा जी आपल्या स्वतःच्या डेटावर आधारित उत्तरे देते |
| 5 | सातत्यपूर्ण सूचना आणि व्यक्तिमत्त्वांसह AI एजंट तयार करा |
| 6 | फीडबॅक लूप्ससह मल्टी-एजंट वर्कफ्लोजचे आयोजन करा |
| 7 | उत्पादन स्तरावरच्या कॅपस्टोन अॅप - Zava Creative Writer चा अन्वेषण करा |
| 8 | सुवर्ण डेटा संच आणि LLM-एज-जज स्कोअरिंगसह मूल्यांकन फ्रेमवर्क तयार करा |
| 9 | Whisper वापरून ऑडिओ ट्रांसक्राईब करा - Foundry Local SDK सह डिव्हाइसवर भाषण-ते-टेक्स्ट |
| 10 | ONNX Runtime GenAI आणि Foundry Local सह कस्टम किंवा Hugging Face मॉडेल्स संकलित करा आणि चालवा |
| 11 | टूल-कॉलिंग पॅटर्नसह स्थानिक मॉडेलना बाह्य फंक्शन्स कॉल करण्याची परवानगी द्या |
| 12 | रिअल-टाइम स्ट्रीमिंगसह Zava Creative Writer साठी ब्राउझर-आधारित UI तयार करा |

---

## पूर्वअटी

| आवश्यकता | तपशील |
|-------------|---------|
| **हार्डवेअर** | कमीतकमी 8 GB RAM (शिफारस 16 GB); AVX2-सक्षम CPU किंवा समर्थित GPU |
| **ऑपरेटिंग सिस्टम** | Windows 10/11 (x64/ARM), Windows Server 2025, किंवा macOS 13+ |
| **Foundry Local CLI** | Windows साठी `winget install Microsoft.FoundryLocal` किंवा macOS साठी `brew tap microsoft/foundrylocal && brew install foundrylocal` वापरून इंस्टॉल करा. सविस्तर माहितीसाठी [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) पहा. |
| **भाषा रनटाइम** | **Python 3.9+** आणि/किंवा **.NET 9.0+** आणि/किंवा **Node.js 18+** |
| **Git** | या रिपॉझिटरी क्लोन करण्यासाठी |

---

## प्रारंभ करणे

```bash
# 1. रेपॉझिटरी क्लोन करा
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Foundry लोकल स्थापित आहे का तपासा
foundry model list              # उपलब्ध मॉडेल्सची यादी करा
foundry model run phi-3.5-mini  # इंटरएक्टिव्ह चॅट सुरू करा

# 3. आपली भाषा ट्रॅक निवडा (पूर्ण सेटअपसाठी Part 2 lab पहा)
```

| भाषा | जलद प्रारंभ |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## कार्यशाळेचे भाग

### भाग 1: Foundry Local सह प्रारंभ करणे

**लॅब मार्गदर्शक:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local काय आहे आणि ते कसे कार्य करते
- Windows आणि macOS वर CLI इन्स्टॉल करणे
- मॉडेल्सचा अभ्यास करणे - यादी तयार करणे, डाउनलोड करणे, चालवणे
- मॉडेल उपनाम आणि डायनॅमिक पोर्ट्स समजून घेणे

---

### भाग 2: Foundry Local SDK सखोल अभ्यास

**लॅब मार्गदर्शक:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- अॅप्लिकेशन विकसनासाठी CLI च्या तुलनेत SDK का वापरावे
- Python, JavaScript, आणि C# साठी संपूर्ण SDK API संदर्भ
- सेवा व्यवस्थापन, कॅटलॉग ब्राउझिंग, मॉडेल लाइफसायकल (डाउनलोड, लोड, अनलोड)
- जलद प्रारंभ नमुने: Python कॉन्स्ट्रक्टर बूटस्ट्रॅप, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` मेटाडेटा, उपनाम, आणि हार्डवेअर-अनुकूल मॉडेल निवड

---

### भाग 3: SDK आणि API

**लॅब मार्गदर्शक:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, आणि C# मधून Foundry Local शी कनेक्ट होणे
- Foundry Local SDK वापरून प्रोग्रामॅटिक स्वरूपात सेवा व्यवस्थापित करणे
- OpenAI-सुसंगत API द्वारे स्ट्रीमिंग चॅट पूर्णता
- प्रत्येक भाषेसाठी SDK पद्धत संदर्भ

**कोड नमुने:**

| भाषा | फाईल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local.py` | मूलभूत स्ट्रीमिंग चॅट |
| C# | `csharp/BasicChat.cs` | .NET सह स्ट्रीमिंग चॅट |
| JavaScript | `javascript/foundry-local.mjs` | Node.js सह स्ट्रीमिंग चॅट |

---

### भाग 4: Retrieval-Augmented Generation (RAG)

**लॅब मार्गदर्शक:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG म्हणजे काय आणि ते का महत्त्वाचे आहे
- इन-मेमरी नॉलेज बेस तयार करणे
- कीवर्ड-अवरलॅप मिळवणे आणि स्कोअरिंग
- आधारभूत सिस्‍टम प्रॉम्प्ट्स तयार करणे
- डिव्हाइसवर पूर्ण RAG पाइपलाइन चालवणे

**कोड नमुने:**

| भाषा | फाईल |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### भाग 5: AI एजंट तयार करणे

**लॅब मार्गदर्शक:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI एजंट म्हणजे काय (साध्या LLM कॉलच्या विरुद्ध)
- `ChatAgent` पॅटर्न आणि Microsoft Agent Framework
- सिस्‍टम सूचना, व्यक्तिमत्त्वे, आणि मल्टी-टर्न संभाषणे
- एजंटकडून संरचित आउटपुट (JSON)

**कोड नमुने:**

| भाषा | फाईल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework सह एकल एजंट |
| C# | `csharp/SingleAgent.cs` | एकल एजंट (ChatAgent पॅटर्न) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | एकल एजंट (ChatAgent पॅटर्न) |

---

### भाग 6: मल्टी-एजंट वर्कफ्लोज

**लॅब मार्गदर्शक:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- मल्टी-एजंट पाइपलाइन: संशोधक → लेखक → संपादक
- सिक्वेन्शियल ऑर्केस्ट्रेशन आणि फीडबॅक लूप्स
- सामायिक कॉन्फिगरेशन आणि संरचित हस्तांतरण
- आपला स्वतःचा मल्टी-एजंट वर्कफ्लो डिझाइन करा

**कोड नमुने:**

| भाषा | फाईल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | तीन एजंटांची पाइपलाइन |
| C# | `csharp/MultiAgent.cs` | तीन एजंटांची पाइपलाइन |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | तीन एजंटांची पाइपलाइन |

---

### भाग 7: Zava Creative Writer - कॅपस्टोन अनुप्रयोग

**लॅब मार्गदर्शक:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- ४ विशेष एजंटांसह उत्पादन-शैलीची मल्टी-एजंट अॅप्लिकेशन
- सिक्वेन्शियल पाइपलाइन सोबत मूल्यांकन-चालित फीडबॅक लूप्स
- स्ट्रीमिंग आउटपुट, उत्पादन कॅटलॉग शोध, संरचित JSON हस्तांतरण
- पूर्ण अंमलबजावणी Python (FastAPI), JavaScript (Node.js CLI), आणि C# (.NET कन्सोल) मध्ये

**कोड नमुने:**

| भाषा | डिरेक्टरी | वर्णन |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | ऑर्केस्ट्रेटरसह FastAPI वेब सेवा |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI अॅप्लिकेशन |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 कन्सोल अनुप्रयोग |

---

### भाग 8: मूल्यांकन-आधारित विकास

**लॅब मार्गदर्शक:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- सुवर्ण डेटा संच वापरून AI एजंटसाठी एक प्रणालीबद्ध मूल्यांकन फ्रेमवर्क तयार करा
- नियम-आधारित तपासणी (लांबी, कीवर्ड कव्हरेज, मनाई केलेले शब्द) + LLM-एज-न्यायाधीश स्कोअरिंग
- प्रॉम्प्ट प्रकारांची साइड-बाय-साइड तुलना आणि एकत्रित स्कोरकार्ड्स
- भाग 7 मधील Zava Editor एजंट पॅटर्नला ऑफलाइन चाचणी सुईटमध्ये विस्तारित करा
- Python, JavaScript, आणि C# ट्रॅक्स

**कोड नमुने:**

| भाषा | फाईल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | मूल्यांकन फ्रेमवर्क |
| C# | `csharp/AgentEvaluation.cs` | मूल्यांकन फ्रेमवर्क |
| JavaScript | `javascript/foundry-local-eval.mjs` | मूल्यांकन फ्रेमवर्क |

---

### भाग 9: Whisper सह व्हॉइस ट्रांसक्रिप्शन

**लॅब मार्गदर्शक:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- स्थानिक चालणारे OpenAI Whisper वापरून स्पीच-टू-टेक्स्ट ट्रांसक्रिप्शन
- गोपनीयतेवर प्राथमिकता देणारी ऑडिओ प्रक्रिया - ऑडिओ कधीही आपल्या डिव्हाइसबाहेर जात नाही
- Python, JavaScript, आणि C# ट्रॅक्स सह `client.audio.transcriptions.create()` (Python/JS) आणि `AudioClient.TranscribeAudioAsync()` (C#)
- हॅण्ड्स-ऑन सरावासाठी Zava-थीम असलेले नमुना ऑडिओ फाइल्स समाविष्ट

**कोड नमुने:**

| भाषा | फाईल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper व्हॉइस ट्रांसक्रिप्शन |
| C# | `csharp/WhisperTranscription.cs` | Whisper व्हॉइस ट्रांसक्रिप्शन |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper व्हॉइस ट्रांसक्रिप्शन |

> **टीप:** या लॅबमध्ये प्रोग्रामॅटिकली Whisper मॉडेल डाउनलोड आणि लोड करण्यासाठी **Foundry Local SDK** वापरले जाते, नंतर ट्रांसक्रिप्शनसाठी ऑडिओ स्थानिक OpenAI-सुसंगत एंडपॉइंटला पाठवले जाते. Whisper मॉडेल (`whisper`) Foundry Local कॅटलॉगमध्ये सूचीबद्ध आहे आणि पूर्णपणे डिव्हाइसवर चालते - कोणत्याही क्लाउड API की किंवा नेटवर्क प्रवेशाची आवश्यकता नाही.

---

### भाग 10: कस्टम किंवा Hugging Face मॉडेल्स वापरणे

**लॅब मार्गदर्शक:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ONNX Runtime GenAI मॉडेल बिल्डर वापरून Hugging Face मॉडेल्सला ऑप्टिमाइझ्ड ONNX फॉरमॅटमध्ये संकलित करणे
- हार्डवेअर-विशिष्ट संकलन (CPU, NVIDIA GPU, DirectML, WebGPU) आणि क्वांटायझेशन (int4, fp16, bf16)
- Foundry Local साठी चॅट-टेम्पलेट कॉन्फिगरेशन फाइल्स तयार करणे
- संकलित मॉडेल्स Foundry Local कॅशेमध्ये जोडणे
- CLI, REST API, आणि OpenAI SDK द्वारे कस्टम मॉडेल्स चालवणे
- संदर्भ उदाहरण: Qwen/Qwen3-0.6B संकलन सुरुवातीपासून शेवटपर्यंत

---

### भाग 11: स्थानिक मॉडेल्ससह टूल कॉलिंग

**लॅब मार्गदर्शक:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- स्थानिक मॉडेल्सना बाह्य फंक्शन्स कॉल करण्यास सक्षम करणे (टूल/फंक्शन कॉलिंग)
- OpenAI फंक्शन-कॉलिंग फॉरमॅट वापरून टूल स्कीमास 정의 करणे
- मल्टी-टर्न टूल-कॉलिंग संभाषण प्रवाह हाताळणे
- स्थानिकपणे टूल कॉल्ज एक्झिक्यूट करणे आणि निकाल मॉडेलला परत देणे
- टूल-कॉलिंग परिस्थितीसाठी योग्य मॉडेल निवडणे (Qwen 2.5, Phi-4-mini)
- टूल कॉलिंगसाठी SDK च्या नेटिव्ह `ChatClient` चा वापर (JavaScript)

**कोड नमुने:**

| भाषा | फाईल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | हवामान/लोकसंख्या टूल्ससह टूल कॉलिंग |
| C# | `csharp/ToolCalling.cs` | .NET सह टूल कॉलिंग |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient सह टूल कॉलिंग |

---

### भाग 12: Zava Creative Writer साठी वेब UI तयार करणे

**लॅब मार्गदर्शक:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer साठी ब्राउझर-आधारित फ्रंटएन्ड जोडा
- Python (FastAPI), JavaScript (Node.js HTTP), आणि C# (ASP.NET Core) मधून सामायिक UI सर्व्ह करा
- ब्राउझरमध्ये Fetch API आणि ReadableStream वापरून स्ट्रीमिंग NDJSON वापरा
- लाईव्ह एजंट स्थिती बॅजेस आणि रिअल-टाइम लेखाची मजकूर स्ट्रीमिंग

**कोड (सामायिक UI):**

| फाईल | वर्णन |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | पृष्ठ लेआउट |
| `zava-creative-writer-local/ui/style.css` | स्टाइलिंग |
| `zava-creative-writer-local/ui/app.js` | स्ट्रीम रीडर आणि DOM अद्यतन लॉजिक |

**बॅकएन्ड जोडणी:**

| भाषा | फाईल | वर्णन |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | स्थिर UI सर्व्ह करण्यासाठी अद्यतनित |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ऑर्केस्ट्रेटरचा HTTP सर्व्हर नवीन रॅपिंग |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | नवीन ASP.NET Core मिनिमल API प्रोजेक्ट |

---

### भाग 13: कार्यशाळा पूर्ण
**प्रयोगशाळा मार्गदर्शक:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- आपण 12 भागांमध्ये बांधलेले सर्व काही याचे सारांश
- आपल्या अनुप्रयोगांचा विस्तार करण्यासाठी पुढील कल्पना
- संसाधने आणि दस्तऐवजीकरणासाठी दुवे

---

## प्रकल्प संरचना

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

## संसाधने

| संसाधन | दुवा |
|----------|------|
| Foundry Local वेबसाइट | [foundrylocal.ai](https://foundrylocal.ai) |
| मॉडेल कॅटलॉग | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| सुरुवात मार्गदर्शक | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK संदर्भ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## परवाना

हे कार्यशाळा साहित्य शैक्षणिक उद्देशासाठी प्रदान केले आहे.

---

**खूप छान बांधा! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:
हा दस्तऐवज AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) वापरून अनुवादित केला आहे. आम्ही अचूकतेसाठी प्रयत्नशील असलो तरी, कृपया लक्षात ठेवा की स्वयंचलित अनुवादांमध्ये चुका किंवा असत्यता असू शकतात. मूळ दस्तऐवज त्याच्या मूळ भाषेत अधिकृत स्रोत मानला पाहिजे. महत्त्वाच्या माहितीसाठी व्यावसायिक मानवी अनुवाद शिफारस केला जातो. या अनुवादाच्या वापरामुळे उद्भवणार्‍या कोणत्याही गैरसमजुती किंवा चुकीच्या अर्थलावासाठी आम्ही जबाबदार नाही.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->