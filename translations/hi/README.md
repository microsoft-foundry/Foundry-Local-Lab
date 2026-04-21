<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local कार्यशाला - डिवाइस पर AI ऐप बनाएं

अपनी मशीन पर भाषा मॉडल चलाने और [Foundry Local](https://foundrylocal.ai) तथा [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) के साथ बुद्धिमान एप्लिकेशन बनाने के लिए एक व्यावहारिक कार्यशाला।

> **Foundry Local क्या है?** Foundry Local एक हल्का रनटाइम है जो आपको पूरी तरह से अपने हार्डवेयर पर भाषा मॉडल डाउनलोड, प्रबंधित और सेवा करने देता है। यह एक **OpenAI-समर्थित API** प्रदान करता है ताकि कोई भी उपकरण या SDK जो OpenAI से बात करता है जुड़ सके - कोई क्लाउड खाता आवश्यक नहीं।

---

## सीखने के उद्देश्य

इस कार्यशाला के अंत तक आप सक्षम होंगे:

| # | उद्देश्य |
|---|-----------|
| 1 | Foundry Local स्थापित करना और CLI के साथ मॉडल प्रबंधित करना |
| 2 | Foundry Local SDK API का प्रोग्रामैटिक मॉडल प्रबंधन के लिए मास्टर बनना |
| 3 | पायथन, जावास्क्रिप्ट, और C# SDK के द्वारा स्थानीय इन्फरेन्स सर्वर से कनेक्ट करना |
| 4 | Retrieval-Augmented Generation (RAG) पाइपलाइन बनाना जो आपके अपने डेटा में उत्तर आधारित हो |
| 5 | कायम रहने वाले निर्देशों और व्यक्तित्वों के साथ AI एजेंट बनाना |
| 6 | फीडबैक लूप्स के साथ मल्टी-एजेंट वर्कफ़्लोज़ का समन्वय करना |
| 7 | प्रोडक्शन कैपस्टोन ऐप - Zava Creative Writer का अन्वेषण करना |
| 8 | गोल्डन डेटासेट्स और LLM-एज-जज स्कोरिंग के साथ मूल्यांकन फ्रेमवर्क बनाना |
| 9 | Whisper के साथ ऑडियो ट्रांसक्रिप्शन - डिवाइस पर स्पीच-टू-टेक्स्ट निर्मित करना Foundry Local SDK का उपयोग कर |
| 10 | ONNX Runtime GenAI और Foundry Local के साथ कस्टम या Hugging Face मॉडल संकलित और चलाना |
| 11 | टूल-कॉलिंग पैटर्न के साथ स्थानीय मॉडलों को बाहरी फंक्शन कॉल करने के लिए सक्षम करना |
| 12 | Zava Creative Writer के लिए रियल-टाइम स्ट्रीमिंग के साथ ब्राउज़र-बेस्ड UI बनाना |

---

## पूर्वापेक्षाएँ

| आवश्यकता | विवरण |
|-------------|---------|
| **हार्डवेयर** | न्यूनतम 8 GB RAM (16 GB अनुशंसित); AVX2-सक्षम CPU या समर्थित GPU |
| **ऑपरेटिंग सिस्टम** | Windows 10/11 (x64/ARM), Windows Server 2025, या macOS 13+ |
| **Foundry Local CLI** | `winget install Microsoft.FoundryLocal` (Windows) या `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) के जरिए इंस्टॉल करें। विवरण के लिए [शुरुआत गाइड](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) देखें। |
| **भाषा रनटाइम** | **Python 3.9+** और/या **.NET 9.0+** और/या **Node.js 18+** |
| **Git** | इस रिपॉजिटरी को क्लोन करने के लिए |

---

## शुरू करना

```bash
# 1. रिपॉजिटरी को क्लोन करें
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. पुष्टि करें कि Foundry Local इंस्टॉल है
foundry model list              # उपलब्ध मॉडल्स की सूची बनाएं
foundry model run phi-3.5-mini  # एक इंटरैक्टिव चैट शुरू करें

# 3. अपनी भाषा ट्रैक चुनें (पूर्ण सेटअप के लिए पार्ट 2 लैब देखें)
```

| भाषा | त्वरित प्रारंभ |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## कार्यशाला के भाग

### भाग 1: Foundry Local के साथ शुरूआत

**लैब गाइड:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local क्या है और यह कैसे काम करता है
- Windows और macOS पर CLI स्थापित करना
- मॉडल अन्वेषण - सूचीबद्ध करना, डाउनलोड करना, चलाना
- मॉडल उपनाम और डायनेमिक पोर्ट को समझना

---

### भाग 2: Foundry Local SDK डिप डाइव

**लैब गाइड:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- एप्लिकेशन विकास के लिए CLI के बजाय SDK क्यों उपयोग करें
- Python, JavaScript, और C# के लिए पूर्ण SDK API संदर्भ
- सेवा प्रबंधन, कैटलॉग ब्राउज़िंग, मॉडल जीवनचक्र (डाउनलोड, लोड, अनलोड)
- त्वरित प्रारंभ पैटर्न: Python कंस्ट्रक्टर बूटस्ट्रैप, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` मेटाडेटा, उपनाम, और हार्डवेयर-अनुकूल मॉडल चयन

---

### भाग 3: SDKs और APIs

**लैब गाइड:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, और C# से Foundry Local से कनेक्ट करना
- Foundry Local SDK का उपयोग कर प्रोग्रामैटिक रूप से सेवा प्रबंधित करना
- OpenAI-समर्थित API के माध्यम से स्ट्रीमिंग चैट पूर्णताएं
- प्रत्येक भाषा के लिए SDK मेथड संदर्भ

**कोड उदाहरण:**

| भाषा | फ़ाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local.py` | बुनियादी स्ट्रीमिंग चैट |
| C# | `csharp/BasicChat.cs` | .NET के साथ स्ट्रीमिंग चैट |
| JavaScript | `javascript/foundry-local.mjs` | Node.js के साथ स्ट्रीमिंग चैट |

---

### भाग 4: Retrieval-Augmented Generation (RAG)

**लैब गाइड:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG क्या है और यह क्यों महत्वपूर्ण है
- इन-मेमोरी नॉलेज बेस बनाना
- स्कोरिंग के साथ कीवर्ड-ओवरलैप रिट्रीवल
- आधारभूत सिस्टम प्रॉम्प्ट्स का संयोजन
- डिवाइस पर पूरी RAG पाइपलाइन चलाना

**कोड उदाहरण:**

| भाषा | फ़ाइल |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### भाग 5: AI एजेंट बनाना

**लैब गाइड:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI एजेंट क्या है (साधारण LLM कॉल के मुकाबले)
- `ChatAgent` पैटर्न और Microsoft Agent Framework
- सिस्टम निर्देश, व्यक्तित्व, और बहु-टर्न वार्तालाप
- एजेंट से संरचित आउटपुट (JSON)

**कोड उदाहरण:**

| भाषा | फ़ाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework के साथ एकल एजेंट |
| C# | `csharp/SingleAgent.cs` | एकल एजेंट (ChatAgent पैटर्न) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | एकल एजेंट (ChatAgent पैटर्न) |

---

### भाग 6: मल्टी-एजेंट वर्कफ़्लोज़

**लैब गाइड:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- मल्टी-एजेंट पाइपलाइन: रिसर्चर → लेखक → संपादक
- अनुक्रमिक समन्वय और फीडबैक लूप्स
- साझा कॉन्फ़िगरेशन और संरचित हैंड-ऑफ़
- अपनी खुद की मल्टी-एजेंट वर्कफ़्लो डिजाइन करना

**कोड उदाहरण:**

| भाषा | फ़ाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | तीन-एजेंट पाइपलाइन |
| C# | `csharp/MultiAgent.cs` | तीन-एजेंट पाइपलाइन |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | तीन-एजेंट पाइपलाइन |

---

### भाग 7: Zava Creative Writer - कैपस्टोन एप्लिकेशन

**लैब गाइड:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 विशेषज्ञ एजेंटों के साथ प्रोडक्शन-शैली मल्टी-एजेंट ऐप
- क्रमानुसार पाइपलाइन जिसमें मूल्यांकनकर्ता निर्देशित फीडबैक लूप्स हैं
- स्ट्रीमिंग आउटपुट, उत्पाद कैटलॉग खोज, संरचित JSON हैंड-ऑफ़
- Python (FastAPI), JavaScript (Node.js CLI), और C# (.NET कॉन्सोल) में पूर्ण कार्यान्वयन

**कोड उदाहरण:**

| भाषा | डायरेक्टरी | विवरण |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | Orchestrator के साथ FastAPI वेब सेवा |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI एप्लिकेशन |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 कॉन्सोल एप्लिकेशन |

---

### भाग 8: मूल्यांकन-आधारित विकास

**लैब गाइड:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- गोल्डन डेटासेट्स का उपयोग करके AI एजेंट के लिए व्यवस्थित मूल्यांकन फ्रेमवर्क बनाना
- नियम आधारित जांच (लंबाई, कीवर्ड कवरेज, निषिद्ध शब्द) + LLM-एज-जज स्कोरिंग
- प्रॉम्प्ट वेरिएंट्स की साइड-बाय-साइड तुलना के साथ समग्र स्कोरकार्ड
- भाग 7 के Zava Editor एजेंट पैटर्न को ऑफलाइन परीक्षण सूट में विस्तारित करना
- Python, JavaScript, और C# ट्रैक्स

**कोड उदाहरण:**

| भाषा | फ़ाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | मूल्यांकन फ्रेमवर्क |
| C# | `csharp/AgentEvaluation.cs` | मूल्यांकन फ्रेमवर्क |
| JavaScript | `javascript/foundry-local-eval.mjs` | मूल्यांकन फ्रेमवर्क |

---

### भाग 9: Whisper के साथ आवाज़ ट्रांसक्रिप्शन

**लैब गाइड:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- स्थानीय रूप से चल रहे OpenAI Whisper का उपयोग कर स्पीच-टू-टेक्स्ट ट्रांसक्रिप्शन
- प्राइवेसी-प्रथम ऑडियो प्रोसेसिंग - ऑडियो कभी भी आपके डिवाइस से बाहर नहीं जाता
- Python, JavaScript, और C# ट्रैक्स `client.audio.transcriptions.create()` (Python/JS) और `AudioClient.TranscribeAudioAsync()` (C#) के साथ
- हाथों-हाथ अभ्यास के लिए Zava-थीम वाले नमूना ऑडियो फाइल शामिल

**कोड उदाहरण:**

| भाषा | फ़ाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper आवाज़ ट्रांसक्रिप्शन |
| C# | `csharp/WhisperTranscription.cs` | Whisper आवाज़ ट्रांसक्रिप्शन |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper आवाज़ ट्रांसक्रिप्शन |

> **ध्यान दें:** यह लैब **Foundry Local SDK** का उपयोग करके प्रोग्रामैटिक रूप से Whisper मॉडल डाउनलोड और लोड करती है, फिर ट्रांसक्रिप्शन के लिए ऑडियो को स्थानीय OpenAI-समर्थित एंडपॉइंट पर भेजती है। Whisper मॉडल (`whisper`) Foundry Local कैटलॉग में सूचीबद्ध है और पूरी तरह डिवाइस पर चलता है - कोई क्लाउड API कुंजियाँ या नेटवर्क एक्सेस आवश्यक नहीं।

---

### भाग 10: कस्टम या Hugging Face मॉडल का उपयोग

**लैब गाइड:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ONNX Runtime GenAI मॉडल बिल्डर का उपयोग कर Hugging Face मॉडलों को अनुकूलित ONNX फॉर्मेट में संकलित करना
- हार्डवेयर-विशिष्ट संकलन (CPU, NVIDIA GPU, DirectML, WebGPU) और क्वांटाइजेशन (int4, fp16, bf16)
- Foundry Local के लिए चैट-टेम्प्लेट कॉन्फ़िगरेशन फ़ाइलें बनाना
- संकलित मॉडलों को Foundry Local कैश में जोड़ना
- CLI, REST API, और OpenAI SDK के माध्यम से कस्टम मॉडल चलाना
- संदर्भ उदाहरण: Qwen/Qwen3-0.6B का एंड-टू-एंड संकलन

---

### भाग 11: स्थानीय मॉडलों के साथ टूल कॉलिंग

**लैब गाइड:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- स्थानीय मॉडलों को बाहरी फंक्शन कॉल (टूल/फंक्शन कॉलिंग) सक्षम करना
- OpenAI फंक्शन-कॉलिंग फॉर्मेट का उपयोग कर टूल स्कीमा परिभाषित करना
- मल्टी-टर्न टूल-कॉलिंग वार्तालाप फ्लो को संभालना
- स्थानीय टूल कॉल्स को निष्पादित करना और मॉडल को परिणाम वापस करना
- टूल-कॉलिंग परिदृश्यों के लिए सही मॉडल चुनना (Qwen 2.5, Phi-4-mini)
- टूल कॉलिंग के लिए SDK के नेटिव `ChatClient` का उपयोग (JavaScript)

**कोड उदाहरण:**

| भाषा | फ़ाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | मौसम/जनसंख्या टूल्स के साथ टूल कॉलिंग |
| C# | `csharp/ToolCalling.cs` | .NET के साथ टूल कॉलिंग |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient के साथ टूल कॉलिंग |

---

### भाग 12: Zava Creative Writer के लिए वेब UI बनाना

**लैब गाइड:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer के लिए ब्राउज़र-आधारित फ्रंट एंड जोड़ना
- Python (FastAPI), JavaScript (Node.js HTTP), और C# (ASP.NET Core) से साझा UI सर्व करना
- Fetch API और ReadableStream के साथ ब्राउज़र में स्ट्रीमिंग NDJSON को उपभोग करना
- लाइव एजेंट स्टेटस बैज और रियल-टाइम आर्टिकल टेक्स्ट स्ट्रीमिंग

**कोड (साझा UI):**

| फ़ाइल | विवरण |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | पेज लेआउट |
| `zava-creative-writer-local/ui/style.css` | स्टाइलिंग |
| `zava-creative-writer-local/ui/app.js` | स्ट्रीम रीडर और DOM अपडेट लॉजिक |

**बैकएंड एडिशन्स:**

| भाषा | फ़ाइल | विवरण |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | स्थैतिक UI सर्व करने के लिए अपडेट किया गया |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | नया HTTP सर्वर जो orchestrator को रैप करता है |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | नया ASP.NET Core मिनिमल API प्रोजेक्ट |

---

### भाग 13: कार्यशाला पूर्ण
**प्रयोगशाला मार्गदर्शिका:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- आपने सभी 12 भागों में जो कुछ भी बनाया उसका सारांश
- अपने अनुप्रयोगों का विस्तार करने के लिए आगे के विचार
- संसाधनों और प्रलेखन के लिंक

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

## संसाधन

| संसाधन | लिंक |
|----------|------|
| Foundry Local वेबसाइट | [foundrylocal.ai](https://foundrylocal.ai) |
| मॉडल कैटलॉग | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| आरंभ करने का मार्गदर्शन | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK संदर्भ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft एजेंट फ्रेमवर्क | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## लाइसेंस

यह कार्यशाला सामग्री शैक्षिक उद्देश्यों के लिए प्रदान की गई है।

---

**खुशी से निर्माण करें! 🚀**