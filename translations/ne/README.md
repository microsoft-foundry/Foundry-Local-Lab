<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local कार्यशाला - उपकरणमा एआई अनुप्रयोगहरू निर्माण गर्नुहोस्

आफ्नो मेसिनमा भाषा मोडेलहरू चलाउन र [Foundry Local](https://foundrylocal.ai) र [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) सँग बुद्धिमान अनुप्रयोगहरू निर्माण गर्नका लागि एक व्यावहारिक कार्यशाला।

> **Foundry Local के हो?** Foundry Local एक हल्का रनटाइम हो जुन तपाईंलाई भाषा मोडेलहरू पूर्ण रूपमा आफ्नो हार्डवेयरमा डाउनलोड गर्न, व्यवस्थापन गर्न, र सेवा गर्न दिन्छ। यसले **OpenAI-समर्थित API** खोल्छ त्यसैले कुनै पनि उपकरण वा SDK जुन OpenAI बोल्छ, जडान हुन सक्छ - कुनै क्लाउड खाता आवश्यक छैन।

---

## सिकाइ लक्ष्यहरू

यस कार्यशालाको अन्त्यसम्ममा तपाईं सक्षम हुनुहुनेछ:

| # | लक्ष्य |
|---|-----------|
| 1 | Foundry Local स्थापना गर्ने र CLI सँग मोडेलहरू व्यवस्थापन गर्ने |
| 2 | Foundry Local SDK API मा प्रोग्रामैटिक मोडेल व्यवस्थापनमा दक्ष हुने |
| 3 | Python, JavaScript, र C# SDKs मार्फत स्थानीय इन्फरेन्स सर्भरसँग जडान हुने |
| 4 | आफ्नै डाटामा आधारित जवाफहरूको लागि Retrieval-Augmented Generation (RAG) पाइपलाइन बनाउने |
| 5 | स्थायी निर्देशन र पर्सोनास भएका AI एजेन्टहरू सिर्जना गर्ने |
| 6 | प्रतिकृया लूपहरूसहित बहु-एजेन्ट व्यवस्थित कार्यप्रवाहहरू आयोजना गर्ने |
| 7 | उत्पादन क्यापस्टोन अनुप्रयोग - Zava Creative Writer अन्वेषण गर्ने |
| 8 | सुनौलो डेटासेट र LLM-व्यवधायक स्कोरिङ्ग सहित मूल्यांकन फ्रेमवर्कहरू बनाउने |
| 9 | Whisper सँग अडियो ट्रान्सक्रिप्सन गर्ने - उपकरणमा भाषण-देखि-पाठ Foundry Local SDK प्रयोग गरेर |
| 10 | ONNX Runtime GenAI र Foundry Local सँग अनुकूलित वा Hugging Face मोडेलहरू कम्पाइल र चलाउने |
| 11 | स्थानीय मोडेलहरूलाई बाह्य फंक्शनहरू कल गर्न सक्षम बनाउने उपकरण-कलिङ ढाँचा प्रयोग गरेर |
| 12 | Zava Creative Writer को लागि वास्तविक-समय स्ट्रिमिङसहित ब्राउजर आधारित UI निर्माण गर्ने |

---

## पूर्वआवश्यकताहरू

| आवश्यकता | विवरण |
|-------------|---------|
| **हार्डवेयर** | न्यूनतम ८ GB RAM (१६ GB सिफारिस); AVX2 समर्थित CPU वा GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, वा macOS 13+ |
| **Foundry Local CLI** | `winget install Microsoft.FoundryLocal` (Windows) वा `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) मार्फत स्थापना गर्नुहोस्। विवरणका लागि [सुरू गर्ने गाइड](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) हेर्नुहोस्। |
| **भाषा रनटाइम** | **Python 3.9+** र/वा **.NET 9.0+** र/वा **Node.js 18+** |
| **Git** | यो रिपोजिटरी क्लोन गर्नको लागि |

---

## सुरु गर्ने तरिका

```bash
# 1. रिपोजिटरी क्लोन गर्नुहोस्
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. फाउन्ड्री लोकल स्थापना गरिएको छ कि छैन जाँच गर्नुहोस्
foundry model list              # उपलब्ध मोडेलहरूको सूची बनाउनुहोस्
foundry model run phi-3.5-mini  # अन्तरक्रियात्मक च्याट सुरु गर्नुहोस्

# 3. आफ्नो भाषा ट्र्याक छान्नुहोस् (पूर्ण सेटअपका लागि भाग २ को प्रयोगशाला हेर्नुहोस्)
```

| भाषा | छिटो सुरु |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## कार्यशालाका भागहरू

### भाग १: Foundry Local सँग शुरु गर्ने

**ल्याब गाइड:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local के हो र यसले कसरी काम गर्छ
- Windows र macOS मा CLI स्थापना
- मोडेलहरूको अन्वेषण - सूचीकरण, डाउनलोड, चलाउने
- मोडेल उपनामहरू र गतिशील पोर्टहरू बुझ्ने

---

### भाग २: Foundry Local SDK गहिराइमा

**ल्याब गाइड:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- अनुप्रयोग विकासका लागि CLI भन्दा SDK किन प्रयोग गर्ने
- Python, JavaScript, र C# को लागि पूर्ण SDK API सन्दर्भ
- सेवा व्यवस्थापन, क्याटलग ब्राउजिङ, मोडेल जीवनचक्र (डाउनलोड, लोड, अनलोड)
- छिटो सुरु गर्ने ढाँचाहरू: Python निर्माणकर्ता बुटस्ट्र्याप, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` मेटाडेटा, उपनामहरू, र हार्डवेयर-अनुकूल मोडेल चयन

---

### भाग ३: SDKs र APIs

**ल्याब गाइड:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, र C# बाट Foundry Local सँग जडान हुने
- Foundry Local SDK प्रयोग गरेर प्रोग्रामैटिक सेवा व्यवस्थापन
- OpenAI-अनुकूल API मार्फत स्ट्रिमिङ च्याट पूरा गर्ने
- प्रत्येक भाषाको SDK मेथड सन्दर्भ

**कोड नमूनाहरू:**

| भाषा | फाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local.py` | आधारभूत स्ट्रिमिङ च्याट |
| C# | `csharp/BasicChat.cs` | .NET सँग स्ट्रिमिङ च्याट |
| JavaScript | `javascript/foundry-local.mjs` | Node.js सँग स्ट्रिमिङ च्याट |

---

### भाग ४: Retrieval-Augmented Generation (RAG)

**ल्याब गाइड:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG के हो र किन महत्वपूर्ण छ
- इन-मेमोरी ज्ञान आधार निर्माण गर्ने
- कौशल-अधारित पुनर्प्राप्ति र स्कोरिङ
- आधारभूत प्रणाली प्रॉम्प्टहरू रचना गर्ने
- उपकरणमा पूर्ण RAG पाइपलाइन चलाउने

**कोड नमूनाहरू:**

| भाषा | फाइल |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### भाग ५: AI एजेन्टहरू निर्माण

**ल्याब गाइड:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI एजेन्ट के हो (सिधा LLM कलको विपरीत)
- `ChatAgent` ढाँचा र Microsoft Agent Framework
- प्रणाली निर्देशनहरू, पर्सोनासहरू, र बहु-पटक संवादहरू
- एजेन्टहरूबाट संरचित आउटपुट (JSON)

**कोड नमूनाहरू:**

| भाषा | फाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | एजेन्ट फ्रेमवर्कसहित एकल एजेन्ट |
| C# | `csharp/SingleAgent.cs` | एकल एजेन्ट (ChatAgent ढाँचा) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | एकल एजेन्ट (ChatAgent ढाँचा) |

---

### भाग ६: बहु-एजेन्ट कार्यप्रवाहहरू

**ल्याब गाइड:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- बहु-एजेन्ट पाइपलाइनहरू: अनुसन्धानकर्ता → लेखक → सम्पादक
- अनुक्रमिक आयोजना र प्रतिकृया लूपहरू
- साझा कन्फिगरेसन र संरचित हस्तान्तरणहरू
- आफ्नै बहु-एजेन्ट कार्यप्रवाह डिजाइन गर्ने

**कोड नमूनाहरू:**

| भाषा | फाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | तीन-एजेन्ट पाइपलाइन |
| C# | `csharp/MultiAgent.cs` | तीन-एजेन्ट पाइपलाइन |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | तीन-एजेन्ट पाइपलाइन |

---

### भाग ७: Zava Creative Writer - क्यापस्टोन अनुप्रयोग

**ल्याब गाइड:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- चार विशेषज्ञ एजेन्टहरूसँग एक उत्पादन-शैली बहु-एजेन्ट अनुप्रयोग
- मूल्याङ्कन-चालित प्रतिकृया लूपसहित अनुक्रमिक पाइपलाइन
- स्ट्रिमिङ आउटपुट, उत्पादन क्याटलग खोज, संरचित JSON हस्तान्तरण
- Python (FastAPI), JavaScript (Node.js CLI), र C# (.NET कन्सोल) मा पूर्ण कार्यान्वयन

**कोड नमूनाहरू:**

| भाषा | निर्देशिका | विवरण |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI वेब सेवा सहित आयोजक |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI अनुप्रयोग |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 कन्सोल अनुप्रयोग |

---

### भाग ८: मूल्यांकन-नेतृत्व विकास

**ल्याब गाइड:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- सुनौलो डेटासेटहरू प्रयोग गरेर AI एजेन्टहरूको लागि समालोचनात्मक मूल्यांकन फ्रेमवर्क बनाउने
- नियम-आधारित जाँचहरू (लम्बाइ, कीवर्ड कभरेज, प्रतिबन्धित शब्दहरू) + LLM-व्यवधायक स्कोरिङ
- प्रॉम्प्ट भेरियन्टहरूको बगल-मा- बगल तुलना र समग्र स्कोरकार्डहरू
- भाग ७ को Zava Editor एजेन्ट ढाँचालाई अफलाइन परीक्षण सूटमा विस्तार गर्ने
- Python, JavaScript, र C# ट्र्याकहरू

**कोड नमूनाहरू:**

| भाषा | फाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | मूल्यांकन फ्रेमवर्क |
| C# | `csharp/AgentEvaluation.cs` | मूल्यांकन फ्रेमवर्क |
| JavaScript | `javascript/foundry-local-eval.mjs` | मूल्यांकन फ्रेमवर्क |

---

### भाग ९: Whisper सँग आवाज ट्रान्सक्रिप्सन

**ल्याब गाइड:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- OpenAI Whisper स्थानीय रूपमा चलाएर भाषण-देखि-पाठ ट्रान्सक्रिप्सन
- गोपनीयता-प्रथमिक अडियो प्रक्रिया - अडियो कहिल्यै तपाईंको उपकरण बाहिर जान्छैन
- Python, JavaScript, र C# ट्र्याकहरू `client.audio.transcriptions.create()` (Python/JS) र `AudioClient.TranscribeAudioAsync()` (C#)
- कसरतका लागि Zava-थीममा नमूना अडियो फाइलहरू सहित

**कोड नमूनाहरू:**

| भाषा | फाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper आवाज ट्रान्सक्रिप्सन |
| C# | `csharp/WhisperTranscription.cs` | Whisper आवाज ट्रान्सक्रिप्सन |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper आवाज ट्रान्सक्रिप्सन |

> **सूचना:** यस ल्याबले **Foundry Local SDK** प्रयोग गरेर Whisper मोडेल प्रोग्रामैटिक रूपमा डाउनलोड र लोड गर्दछ, त्यसपछि ट्रान्सक्रिप्सनका लागि स्थानीय OpenAI-अनुकूल अन्तबिन्दुमा अडियो पठाउँछ। Whisper मोडेल (`whisper`) Foundry Local क्याटलगमा सूचीकृत छ र पूर्ण रूपमा उपकरणमा चल्छ - कुनै क्लाउड API की वा नेटवर्क पहुँच आवश्यक छैन।

---

### भाग १०: अनुकूलित वा Hugging Face मोडेलहरू प्रयोग

**ल्याब गाइड:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ONNX Runtime GenAI मोडेल बिल्डरसँग Hugging Face मोडेलहरूलाई अनुकूलित ONNX फारम्याटमा कम्पाइल गर्ने
- हार्डवेयर-विशेष कम्पाइल (CPU, NVIDIA GPU, DirectML, WebGPU) र क्वान्टाइजेसन (int4, fp16, bf16)
- Foundry Local का लागि च्याट-टेम्प्लेट कन्फिगरेसन फाइलहरू सिर्जना गर्ने
- कम्पाइल गरिएका मोडेलहरू Foundry Local क्याचमा थप्ने
- CLI, REST API, र OpenAI SDK मार्फत अनुकूलित मोडेलहरू चलाउने
- उदाहरण सन्दर्भ: Qwen/Qwen3-0.6B लाई अन्तदेखि-अन्तसम्म कम्पाइल गर्ने

---

### भाग ११: स्थानीय मोडेलहरूको उपकरण कल

**ल्याब गाइड:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- स्थानीय मोडेलहरूलाई बाह्य फंक्शनहरू कल गर्न सक्षम बनाउने (उपकरण/फंक्शन कलिङ)
- OpenAI फंक्शन-कलिङ ढाँचामा आधारित उपकरण स्किमाहरू परिभाषित गर्ने
- बहु-पटक उपकरण कलिङ संवाद प्रवाह व्यवस्थापन गर्ने
- उपकरण कलहरू स्थानीय रूपमा कार्यान्वयन गर्ने र मोडेललाई परिणाम फर्काउने
- उपकरण-कलिङ अवस्थाहरूका लागि उपयुक्त मोडेल छान्ने (Qwen 2.5, Phi-4-mini)
- उपकरण कलका लागि SDK को देशज `ChatClient` प्रयोग गर्ने (JavaScript)

**कोड नमूनाहरू:**

| भाषा | फाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | मौसम/जनसंख्या उपकरणहरूसँग उपकरण कल |
| C# | `csharp/ToolCalling.cs` | .NET सँग उपकरण कल |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient सँग उपकरण कल |

---

### भाग १२: Zava Creative Writer को लागि वेब UI निर्माण

**ल्याब गाइड:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer को लागि ब्राउजर-आधारित फ्रन्टएन्ड थप्ने
- Python (FastAPI), JavaScript (Node.js HTTP), र C# (ASP.NET Core) बाट साझा UI सेवा गर्ने
- Fetch API र ReadableStream सँग ब्राउजरमा स्ट्रिमिङ NDJSON उपभोग गर्ने
- प्रत्यक्ष एजेन्ट स्थिति ब्याज र वास्तविक-समय लेख टेक्स्ट स्ट्रिमिङ

**कोड (साझा UI):**

| फाइल | विवरण |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | पृष्ठ लेआउट |
| `zava-creative-writer-local/ui/style.css` | शैलीकरण |
| `zava-creative-writer-local/ui/app.js` | स्ट्रिम रिडर र DOM अपडेट तर्क |

**ब्याकएन्ड थपहरू:**

| भाषा | फाइल | विवरण |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | स्थिर UI सेवा गर्न अपडेट गरिएको |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | नयाँ HTTP सर्भर जसले आयोजकलाई प्याक गर्छ |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | नयाँ ASP.NET Core न्यूनतम API प्रोजेक्ट |

---

### भाग १३: कार्यशाला पूरा भयो
**प्रयोगशाला मार्गदर्शन:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- तपाईंले सबै १२ भागहरूमा बनाएको सबै कुरा सारांश
- तपाईंका अनुप्रयोगहरू विस्तार गर्ने थप विचारहरू
- स्रोतहरू र दस्तावेजहरूका लिङ्कहरू

---

## परियोजना संरचना

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

## स्रोतहरू

| स्रोत | लिङ्क |
|----------|------|
| Foundry Local वेबसाइट | [foundrylocal.ai](https://foundrylocal.ai) |
| मोडेल क्याटलग | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| सुरु गर्न गाइड | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK संदर्भ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft एजेन्ट फ्रेमवर्क | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## अनुमतिपत्र

यो कार्यशाला सामग्री शैक्षिक उद्देश्यका लागि प्रदान गरिएको हो।

---

**शुभ निर्माण! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:
यो दस्तावेज AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) प्रयोग गरी अनुवाद गरिएको हो। हामी शुद्धताको प्रयास गर्छौं, तर कृपया जानकार हुनुहोस् कि स्वचालित अनुवादमा त्रुटिहरू वा अशुद्धिहरू हुनसक्छ। मूल दस्तावेज यसको本भाषामा नै अधिकारिक स्रोत मानिनुपर्छ। महत्वपूर्ण जानकारीका लागि, व्यावसायिक मानव अनुवाद सिफारिस गरिन्छ। यस अनुवादको प्रयोगबाट उत्पन्न हुने कुनै पनि गलतफहमी वा गलत व्याख्याको लागि हामी जिम्मेवार छैनौं।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->