<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local कार्यशाला - ऑन-डिवाइस AI ऐप्स बनाएं

अपनी खुद की मशीन पर भाषा मॉडल चलाने और [Foundry Local](https://foundrylocal.ai) और [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) के साथ बुद्धिमान अनुप्रयोग बनाने के लिए एक व्यावहारिक कार्यशाला।

> **Foundry Local क्या है?** Foundry Local एक हल्का रनटाइम है जो आपको भाषा मॉडल को पूरी तरह से अपने हार्डवेयर पर डाउनलोड, प्रबंधित और सर्व करने देता है। यह एक **OpenAI-समतुल्य API** प्रदान करता है ताकि कोई भी उपकरण या SDK जो OpenAI बोलता है, जुड़ सके - किसी क्लाउड खाते की आवश्यकता नहीं।

### 🌐 बहुभाषी समर्थन

#### GitHub Action के माध्यम से समर्थित (स्वचालित और सदैव अद्यतित)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](./README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **स्थानीय रूप से क्लोन करना पसंद है?**
>
> इस रिपॉजिटरी में 50+ भाषा अनुवाद शामिल हैं जो डाउनलोड आकार को काफी बढ़ाते हैं। बिना अनुवाद के क्लोन करने के लिए, sparse checkout का उपयोग करें:
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
> यह आपको तेज़ डाउनलोड के साथ कोर्स पूरा करने के लिए जरूरी हर चीज़ देगा।
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## सीखने के उद्देश्य

इस कार्यशाला के अंत तक आप सक्षम होंगे:

| # | उद्देश्य |
|---|-----------|
| 1 | Foundry Local इंस्टॉल करें और CLI के साथ मॉडलों का प्रबंधन करें |
| 2 | प्रोग्रामेटिक मॉडल प्रबंधन के लिए Foundry Local SDK API में माहिर बनें |
| 3 | Python, JavaScript, और C# SDKs का उपयोग करके लोकल इन्फेरेंस सर्वर से कनेक्ट करें |
| 4 | एक Retrieval-Augmented Generation (RAG) पाइपलाइन बनाएं जो आपके अपने डेटा में उत्तरों को आधार बनाये |
| 5 | पर्सिस्टेंट निर्देशों और पर्सोनाज़ के साथ AI एजेंट बनाएं |
| 6 | फीडबैक लूप के साथ मल्टी-एजेंट वर्कफ्लोज़ को ऑर्केस्ट्रेट करें |
| 7 | एक प्रोडक्शन कैपस्टोन ऐप का अन्वेषण करें - Zava Creative Writer |
| 8 | गोल्डन डेटासेट्स और LLM-as-judge स्कोरिंग के साथ मूल्यांकन फ्रेमवर्क बनाएं |
| 9 | Whisper के साथ ऑडियो ट्रांसक्राइब करें - Foundry Local SDK का उपयोग करके ऑन-डिवाइस स्पीच-टू-टेक्स्ट |
| 10 | ONNX Runtime GenAI और Foundry Local के साथ कस्टम या Hugging Face मॉडल को कंपाइल और रन करें |
| 11 | टूल-कॉलिंग पैटर्न के साथ लोकल मॉडल को बाहरी फ़ंक्शन कॉल करने के लिए सक्षम बनाएं |
| 12 | Zava Creative Writer के लिए रियल-टाइम स्ट्रीमिंग वाला ब्राउज़र-आधारित UI बनाएं |

---

## आवश्यकताएँ

| आवश्यकता | विवरण |
|-------------|---------|
| **हार्डवेयर** | न्यूनतम 8 GB RAM (16 GB सिफारिश), AVX2-सक्षम CPU या समर्थित GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, या macOS 13+ |
| **Foundry Local CLI** | Windows के लिए `winget install Microsoft.FoundryLocal` या macOS के लिए `brew tap microsoft/foundrylocal && brew install foundrylocal` के माध्यम से इंस्टॉल करें। विवरण के लिए [शुरुआत गाइड](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) देखें। |
| **भाषा रनटाइम** | **Python 3.9+** और/या **.NET 9.0+** और/या **Node.js 18+** |
| **Git** | इस रिपॉजिटरी को क्लोन करने के लिए |

---

## शुरू करें

```bash
# 1. रिपॉजिटरी क्लोन करें
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. पुष्टि करें कि Foundry Local इंस्टॉल है
foundry model list              # उपलब्ध मॉडल सूचीबद्ध करें
foundry model run phi-3.5-mini  # एक इंटरैक्टिव चैट शुरू करें

# 3. अपनी भाषा ट्रैक चुनें (पूर्ण सेटअप के लिए पार्ट 2 लैब देखें)
```

| भाषा | त्वरित आरंभ |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## कार्यशाला के भाग

### भाग 1: Foundry Local के साथ शुरुआत

**लैब गाइड:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local क्या है और यह कैसे काम करता है
- Windows और macOS पर CLI इंस्टॉल करना
- मॉडलों का अन्वेषण - सूचीबद्ध करना, डाउनलोड करना, चलाना
- मॉडल उपनाम और डायनामिक पोर्ट समझना

---

### भाग 2: Foundry Local SDK का गहराई से अध्ययन

**लैब गाइड:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- ऐप विकास के लिए CLI की तुलना में SDK का उपयोग क्यों करें
- Python, JavaScript, और C# के लिए पूर्ण SDK API संदर्भ
- सेवा प्रबंधन, कैटलॉग ब्राउज़िंग, मॉडल जीवनचक्र (डाउनलोड, लोड, अनलोड)
- त्वरित प्रारंभ पैटर्न: Python कंस्ट्रक्टर बूटस्ट्रैप, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` मेटाडेटा, उपनाम, और हार्डवेयर-इष्टतम मॉडल चयन

---

### भाग 3: SDKs और APIs

**लैब गाइड:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, और C# से Foundry Local से कनेक्ट करना
- Foundry Local SDK का उपयोग करके प्रोग्रामेटिक सेवा प्रबंधन
- OpenAI-समतुल्य API के माध्यम से स्ट्रीमिंग चैट पूर्णताएं
- प्रत्येक भाषा के लिए SDK विधि संदर्भ

**कोड नमूने:**

| भाषा | फ़ाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local.py` | बुनियादी स्ट्रीमिंग चैट |
| C# | `csharp/BasicChat.cs` | .NET के साथ स्ट्रीमिंग चैट |
| JavaScript | `javascript/foundry-local.mjs` | Node.js के साथ स्ट्रीमिंग चैट |

---

### भाग 4: Retrieval-Augmented Generation (RAG)

**लैब गाइड:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG क्या है और यह क्यों महत्वपूर्ण है
- एक इन-मेमोरी नॉलेज बेस बनाना
- स्कोरिंग के साथ कीवर्ड-ओवरलैप पुनर्प्राप्ति
- आधारित सिस्टम प्रॉम्प्ट्स बनाना
- डिवाइस पर पूर्ण RAG पाइपलाइन चलाना

**कोड नमूने:**

| भाषा | फ़ाइल |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### भाग 5: AI एजेंट बनाना

**लैब गाइड:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI एजेंट क्या है (एक कच्चे LLM कॉल के मुकाबले)
- `ChatAgent` पैटर्न और Microsoft Agent Framework
- सिस्टम निर्देश, पर्सोनाज, और मल्टी-टर्न बातचीत
- एजेंट से संरचित आउटपुट (JSON)

**कोड नमूने:**

| भाषा | फ़ाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework के साथ एकल एजेंट |
| C# | `csharp/SingleAgent.cs` | एकल एजेंट (ChatAgent पैटर्न) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | एकल एजेंट (ChatAgent पैटर्न) |

---

### भाग 6: मल्टी-एजेंट वर्कफ्लोज़

**लैब गाइड:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- मल्टी-एजेंट पाइपलाइन: रिसर्चर → लेखक → संपादक
- क्रमिक ऑर्केस्ट्रेशन और फीडबैक लूप
- साझा कॉन्फ़िगरेशन और संरचित हैंड-ऑफ
- अपना मल्टी-एजेंट वर्कफ्लो डिज़ाइन करना

**कोड नमूने:**

| भाषा | फ़ाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | तीन-एजेंट पाइपलाइन |
| C# | `csharp/MultiAgent.cs` | तीन-एजेंट पाइपलाइन |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | तीन-एजेंट पाइपलाइन |

---

### भाग 7: Zava Creative Writer - कैपस्टोन एप्लिकेशन

**लैब गाइड:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 विशेषज्ञ एजेंटों के साथ एक प्रोडक्शन-शैली का मल्टी-एजेंट ऐप
- मूल्यांकनकर्ता प्रमुख फीडबैक लूप के साथ क्रमिक पाइपलाइन
- स्ट्रीमिंग आउटपुट, उत्पाद कैटलॉग खोज, संरचित JSON हैंड-ऑफ
- Python (FastAPI), JavaScript (Node.js CLI), और C# (.NET कंसोल) में पूर्ण कार्यान्वयन

**कोड नमूने:**

| भाषा | निर्देशिका | विवरण |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | ऑर्केस्ट्रेटर के साथ FastAPI वेब सेवा |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI एप्लिकेशन |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 कंसोल एप्लिकेशन |

---

### भाग 8: मूल्यांकन-प्रेरित विकास

**लैब गाइड:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- गोल्डन डेटासेट्स का उपयोग करके AI एजेंट्स के लिए एक व्यवस्थित मूल्यांकन फ्रेमवर्क बनाएं
- नियम-आधारित जांचें (लंबाई, कीवर्ड कवरेज, निषिद्ध शब्द) + LLM-एज़-जज स्कोरिंग
- प्राथमिकताओं के साइड-बाय-साइड तुलनात्मक विश्लेषण के साथ समग्र स्कोरकार्ड
- भाग 7 से Zava Editor एजेंट पैटर्न को ऑफ़लाइन टेस्ट सूट में विस्तारित करता है
- Python, JavaScript, और C# ट्रैक

**कोड नमूने:**

| भाषा | फ़ाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | मूल्यांकन फ्रेमवर्क |
| C# | `csharp/AgentEvaluation.cs` | मूल्यांकन फ्रेमवर्क |
| JavaScript | `javascript/foundry-local-eval.mjs` | मूल्यांकन फ्रेमवर्क |

---

### भाग 9: Whisper के साथ वॉइस ट्रांसक्रिप्शन

**लैब गाइड:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- OpenAI Whisper का उपयोग करके स्थानीय रूप से चलने वाला स्पीच-टू-टेक्स्ट ट्रांसक्रिप्शन
- प्राइवेसी-फर्स्ट ऑडियो प्रोसेसिंग - ऑडियो कभी भी आपके डिवाइस से बाहर नहीं जाता
- Python, JavaScript, और C# ट्रैकों के साथ `client.audio.transcriptions.create()` (Python/JS) और `AudioClient.TranscribeAudioAsync()` (C#)
- हाथों-हाथ अभ्यास के लिए Zava-थीम्ड सैंपल ऑडियो फाइलें शामिल हैं

**कोड सैंपल्स:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper वॉइस ट्रांसक्रिप्शन |
| C# | `csharp/WhisperTranscription.cs` | Whisper वॉइस ट्रांसक्रिप्शन |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper वॉइस ट्रांसक्रिप्शन |

> **Note:** यह प्रयोगशाला **Foundry Local SDK** का उपयोग करके व्हिस्पर मॉडल को प्रोग्रामैटिक रूप से डाउनलोड और लोड करती है, फिर ट्रांसक्रिप्शन के लिए ऑडियो को स्थानीय OpenAI-समर्थित एंडपॉइंट पर भेजती है। Whisper मॉडल (`whisper`) Foundry Local कैटलॉग में सूचीबद्ध है और पूरी तरह से डिवाइस पर चलता है - किसी क्लाउड API की या नेटवर्क एक्सेस की आवश्यकता नहीं है।

---

### भाग 10: कस्टम या Hugging Face मॉडल्स का उपयोग

**प्रयोगशाला मार्गदर्शिका:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face मॉडल्स को ONNX Runtime GenAI मॉडल बिल्डर का उपयोग करके अनुकूलित ONNX फॉर्मेट में संकलित करना
- हार्डवेयर-विशिष्ट संकलन (CPU, NVIDIA GPU, DirectML, WebGPU) और क्वांटाइजेशन (int4, fp16, bf16)
- Foundry Local के लिए चैट-टेम्पलेट कॉन्फ़िगरेशन फाइलें बनाना
- संकलित मॉडलों को Foundry Local कैश में जोड़ना
- CLI, REST API, और OpenAI SDK के माध्यम से कस्टम मॉडल्स चलाना
- संदर्भ उदाहरण: Qwen/Qwen3-0.6B को एंड-टू-एंड संकलित करना

---

### भाग 11: स्थानीय मॉडलों के साथ टूल कॉलिंग

**प्रयोगशाला मार्गदर्शिका:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- स्थानीय मॉडलों को बाहरी फ़ंक्शंस कॉल करने में सक्षम बनाना (टूल/फ़ंक्शन कॉलिंग)
- OpenAI फ़ंक्शन-कॉलिंग फॉर्मेट का उपयोग करके टूल स्कीमाओं को परिभाषित करना
- मल्टी-टर्न टूल-कॉलिंग बातचीत प्रवाह को संभालना
- टूल कॉल स्थानीय रूप से निष्पादित करना और मॉडल को परिणाम लौटाना
- टूल-कॉलिंग परिदृश्यों के लिए सही मॉडल चुनना (Qwen 2.5, Phi-4-mini)
- SDK के नेटिव `ChatClient` का उपयोग करना टूल कॉलिंग (JavaScript)

**कोड सैंपल्स:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | मौसम/जनसंख्या टूल के साथ टूल कॉलिंग |
| C# | `csharp/ToolCalling.cs` | .NET के साथ टूल कॉलिंग |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient के साथ टूल कॉलिंग |

---

### भाग 12: Zava Creative Writer के लिए वेब UI बनाना

**प्रयोगशाला मार्गदर्शिका:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer के लिए ब्राउज़र-आधारित फ्रंट एंड जोड़ना
- Python (FastAPI), JavaScript (Node.js HTTP), और C# (ASP.NET Core) से साझा UI सर्व करना
- ब्राउज़र में Fetch API और ReadableStream के साथ स्ट्रीमिंग NDJSON को उपभोग करना
- लाइव एजेंट स्टेटस बैजेस और रियल-टाइम आर्टिकल टेक्स्ट स्ट्रीमिंग

**कोड (साझा UI):**

| File | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | पेज लेआउट |
| `zava-creative-writer-local/ui/style.css` | स्टाइलिंग |
| `zava-creative-writer-local/ui/app.js` | स्ट्रीम रीडर और DOM अपडेट लॉजिक |

**बैकएंड जोड़ियां:**

| Language | File | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | स्थैतिक UI सर्व करने के लिए अपडेट किया गया |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | आयोजक को रैप करने वाला नया HTTP सर्वर |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | नया ASP.NET Core न्यूनतम API प्रोजेक्ट |

---

### भाग 13: कार्यशाला पूर्ण

**प्रयोगशाला मार्गदर्शिका:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- आपने जिन 12 भागों में बनाया है उनका सारांश
- अपने ऐप्लिकेशन को बढ़ाने के लिए आगे के विचार
- संसाधनों और प्रलेखन के लिंक

---

## प्रोजेक्ट संरचना

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

| Resource | Link |
|----------|------|
| Foundry Local वेबसाइट | [foundrylocal.ai](https://foundrylocal.ai) |
| मॉडल कैटलॉग | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local गिटहब | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| आरंभ करने का मार्गदर्शिका | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK संदर्भ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft एजेंट फ्रेमवर्क | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## लाइसेंस

यह कार्यशाला सामग्री शैक्षिक उद्देश्यों के लिए प्रदान की गई है।

---

**शुभ निर्माण! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:  
यह दस्तावेज़ AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) का उपयोग करके अनूदित किया गया है। जबकि हम सटीकता का प्रयास करते हैं, कृपया ध्यान दें कि स्वचालित अनुवादों में त्रुटियाँ या असामान्यताएँ हो सकती हैं। मूल दस्तावेज़ अपनी मूल भाषा में आधिकारिक स्रोत माना जाना चाहिए। महत्वपूर्ण जानकारी के लिए, पेशेवर मानव अनुवाद की सिफारिश की जाती है। इस अनुवाद के उपयोग से उत्पन्न किसी भी गलतफहमी या गलत व्याख्या के लिए हम जिम्मेदार नहीं हैं।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->