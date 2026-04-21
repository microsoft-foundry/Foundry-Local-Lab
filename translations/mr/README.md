<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local वर्कशॉप - डिव्हाइसवर AI अॅप्स तयार करा

तुमच्या स्वतःच्या मशीनवर भाषा मॉडेल चालविण्यासाठी आणि [Foundry Local](https://foundrylocal.ai) आणि [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) सह बुद्धिमान अनुप्रयोग तयार करण्यासाठी एक हँड्स-ऑन वर्कशॉप.

> **Foundry Local म्हणजे काय?** Foundry Local हा एक हलका रनटाइम आहे जो तुम्हाला भाषा मॉडेल्स पूर्णपणे तुमच्या हार्डवेअरवर डाउनलोड, व्यवस्थापित आणि सेवा देण्याची परवानगी देतो. तो एक **OpenAI-सुसंगत API** उघडतो जेणेकरून कोणतेही टूल किंवा SDK जे OpenAIशी संवाद साधते ते कनेक्ट होऊ शकते - कोणत्याही क्लाउड खात्याची गरज नाही.

### 🌐 बहुभाषी समर्थन

#### GitHub Action द्वारे समर्थित (स्वयंचलित आणि नेहमी अद्ययावत)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](./README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **स्थानिक क्लोन करणे प्राधान्य द्याल का?**
>
> या रिपॉझिटरीमध्ये 50+ भाषा भाषांतर समाविष्ट आहेत ज्यामुळे डाउनलोड आकार खूप वाढतो. भाषांतरांशिवाय क्लोन करण्यासाठी sparse checkout वापरा:
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
> यामुळे तुम्हाला कोर्स पूर्ण करण्यासाठी आवश्यक सर्वकाही मिळेल आणि डाउनलोड अधिक जलद होईल.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## शिकण्याची उद्दिष्टे

या वर्कशॉपचा शेवट पर्यंत तुम्ही सक्षम असाल:

| # | उद्दिष्ट |
|---|-----------|
| 1 | Foundry Local स्थापित करा आणि CLI वापरून मॉडेल व्यवस्थापित करा |
| 2 | प्रोग्रामॅटिक मॉडेल व्यवस्थापनासाठी Foundry Local SDK API मध्ये पारंगत व्हा |
| 3 | Python, JavaScript आणि C# SDK वापरून स्थानिक इन्फरन्स सर्व्हरशी कनेक्ट व्हा |
| 4 | Retrieval-Augmented Generation (RAG) पाईपलाइन तयार करा जी तुमच्या स्वतःच्या डेटावर आधारित उत्तरे देते |
| 5 | कायमस्वरूपी सूचना आणि व्यक्तिमत्वांसह AI एजंट तयार करा |
| 6 | फीडबॅक लूपसह मल्टी-एजंट वर्कफ्लोज व्यवस्थापित करा |
| 7 | उत्पादन स्तरावर झावा क्रिएटिव्ह राइटर अॅप एक्सप्लोर करा |
| 8 | गोल्डन डेटासेट्स आणि LLM-as-judge स्कोरिंगसह मूल्यांकन फ्रेमवर्क तयार करा |
| 9 | Whisper वापरून ऑडिओ ट्रान्सक्राइब करा - डिव्हाइसवरच भाषण-टेक्स्ट रूपांतरित करा |
| 10 | ONNX Runtime GenAI आणि Foundry Local वापरून कस्टम किंवा Hugging Face मॉडेल कंपाईल आणि चालवा |
| 11 | टूल-कॉलिंग पद्धतीने स्थानिक मॉडेल्सना बाह्य फंक्शन्स कॉल करण्यास सक्षम करा |
| 12 | झावा क्रिएटिव्ह राइटरसाठी ब्राऊजर-आधारित UI तयार करा ज्यात रिअल-टाइम स्ट्रीमिंग आहे |

---

## पूर्वअटी

| आवश्यकता | तपशील |
|-------------|---------|
| **हार्डवेअर** | किमान 8 GB RAM (शिफारस 16 GB); AVX2-सक्षम CPU किंवा समर्थित GPU |
| **ऑपरेटिंग सिस्टम** | Windows 10/11 (x64/ARM), Windows Server 2025, किंवा macOS 13+ |
| **Foundry Local CLI** | `winget install Microsoft.FoundryLocal` (Windows) किंवा `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) वापरून स्थापित करा. तपशीलासाठी [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) पहा. |
| **भाषा रनटाइम** | **Python 3.9+** आणि/किंवा **.NET 9.0+** आणि/किंवा **Node.js 18+** |
| **Git** | ही रिपॉझिटरी क्लोन करण्यासाठी |

---

## सुरुवात कशी करावी

```bash
# 1. रीपॉझिटरी क्लोन करा
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. फाउंड्री लोकल स्थापित आहे का याची पडताळणी करा
foundry model list              # उपलब्ध मॉडेल्सची यादी करा
foundry model run phi-3.5-mini  # संवादात्मक चॅट सुरू करा

# 3. तुमचा भाषा ट्रॅक निवडा (पूर्ण सेटअपसाठी भाग 2 चा प्रयोगशाळा पहा)
```

| भाषा | जलद प्रारंभ |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## वर्कशॉप भाग

### भाग 1: Foundry Local सोबत सुरुवात

**लॅब मार्गदर्शक:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local काय आहे आणि ते कसे काम करते
- Windows आणि macOS वर CLI स्थापित करणे
- मॉडेल एक्सप्लोर करणे - सूचीबद्ध करणे, डाउनलोड करणे, चालविणे
- मॉडेल उपनाम आणि डायनॅमिक पोर्ट्स समजून घेणे

---

### भाग 2: Foundry Local SDK सखोल अभ्यास

**लॅब मार्गदर्शक:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- अॅप्लिकेशन विकासासाठी CLI वर SDK का वापरावे
- Python, JavaScript आणि C# साठी पूर्ण SDK API संदर्भ
- सेवा व्यवस्थापन, कॅटलॉग ब्राऊझिंग, मॉडेल जीवनचक्र (डाउनलोड, लोड, अनलोड)
- जलद प्रारंभ पॅटर्न: Python कन्स्ट्रक्टर बूटस्ट्रॅप, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` मेटाडेटा, उपनाम आणि हार्डवेअर-उत्तम मॉडेल निवड

---

### भाग 3: SDKs आणि APIs

**लॅब मार्गदर्शक:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript आणि C# मधून Foundry Local शी कनेक्ट होणे
- Foundry Local SDK वापरून प्रोग्रामॅटिक सेवा व्यवस्थापन
- OpenAI-सुसंगत API द्वारे स्ट्रीमिंग चॅट पूर्णता
- प्रत्येक भाषेसाठी SDK पद्धत संदर्भ

**कोड नमुने:**

| भाषा | फाइल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local.py` | बेसिक स्ट्रीमिंग चॅट |
| C# | `csharp/BasicChat.cs` | .NET सह स्ट्रीमिंग चॅट |
| JavaScript | `javascript/foundry-local.mjs` | Node.js सह स्ट्रीमिंग चॅट |

---

### भाग 4: Retrieval-Augmented Generation (RAG)

**लॅब मार्गदर्शक:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG म्हणजे काय आणि त्याचे महत्त्व
- इन-मेमरी नॉलेज बेस तयार करणे
- कीवर्ड-ओव्हरलॅप रिट्रीव्हलसह स्कोरिंग
- आधारभूत सिस्टम प्रॉम्प्ट्स तयार करणे
- डिव्हाइसवर पूर्ण RAG पाईपलाइन चालविणे

**कोड नमुने:**

| भाषा | फाइल |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### भाग 5: AI एजंट तयार करणे

**लॅब मार्गदर्शक:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI एजंट म्हणजे काय (किंवा थेट LLM कॉलच्या तुलनेत)
- `ChatAgent` पॅटर्न आणि Microsoft Agent Framework
- सिस्टम सूचना, व्यक्तिमत्वे, आणि मल्टी-टर्न संभाषणे
- एजंट्सकडून संरचित आउटपुट (JSON)

**कोड नमुने:**

| भाषा | फाइल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework सह एकल एजंट |
| C# | `csharp/SingleAgent.cs` | एकल एजंट (ChatAgent पॅटर्न) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | एकल एजंट (ChatAgent पॅटर्न) |

---

### भाग 6: मल्टी-एजंट वर्कफ्लोज

**लॅब मार्गदर्शक:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- मल्टी-एजंट पाईपलाइन: संशोधक → लेखक → संपादक
- अनुक्रमिक ऑर्केस्ट्रेशन आणि फीडबॅक लूप्स
- सामायिक कॉन्फिगरेशन आणि संरचित हस्तांतरण
- स्वतःचा मल्टी-एजंट वर्कफ्लो डिझाइन करणे

**कोड नमुने:**

| भाषा | फाइल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | तीन एजंटांचा पाईपलाइन |
| C# | `csharp/MultiAgent.cs` | तीन एजंटांचा पाईपलाइन |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | तीन एजंटांचा पाईपलाइन |

---

### भाग 7: झावा क्रिएटिव्ह राइटर - कॅपस्टोन अॅप्लिकेशन

**लॅब मार्गदर्शक:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 विशेष एजंटसह उत्पादन स्तरावरील मल्टी-एजंट अॅप
- मूल्यांकन-केंद्रित फीडबॅक लूपसह अनुक्रमिक पाईपलाइन
- स्ट्रीमिंग आउटपुट, उत्पादन सूची शोध, संरचित JSON हस्तांतरण
- Python (FastAPI), JavaScript (Node.js CLI), आणि C# (.NET कन्सोल) मध्ये पूर्ण अंमलबजावणी

**कोड नमुने:**

| भाषा | निर्देशिका | वर्णन |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | ऑर्केस्ट्रेटरसह FastAPI वेब सेवा |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI अॅप्लिकेशन |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 कन्सोल अॅप्लिकेशन |

---

### भाग 8: मूल्यांकन-आधारित विकास

**लॅब मार्गदर्शक:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- गोल्डन डेटासेट्स वापरून AI एजंटसाठी व्यावस्थापन मूल्यांकन फ्रेमवर्क तयार करा
- नियम-आधारित तपासण्या (लांबी, कीवर्ड कव्हरेज, निषिद्ध शब्द) + LLM-as-judge स्कोरिंग
- प्रॉम्प्ट व्हेरिएंट्सची साइड-बाय-साइड तुलना आणि एकत्रित स्कोअरकार्ड
- भाग 7 मधील झावा एडिटर एजंट पॅटर्नला ऑफलाइन टेस्ट सूटमध्ये विस्तारित करणे
- Python, JavaScript, आणि C# ट्रॅक्स

**कोड नमुने:**

| भाषा | फाइल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | मूल्यांकन फ्रेमवर्क |
| C# | `csharp/AgentEvaluation.cs` | मूल्यांकन फ्रेमवर्क |
| JavaScript | `javascript/foundry-local-eval.mjs` | मूल्यांकन फ्रेमवर्क |

---

### भाग 9: Whisper वापरून वॉइस ट्रान्सक्रिप्शन

**लॅब मार्गदर्शक:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- OpenAI Whisper वापरून स्थानिकरित्या चालणार्‍या स्पीच-टू-टेक्स्ट ट्रान्सक्रिप्शन
- गोपनीयतेवर आधारित ऑडिओ प्रक्रिया - ऑडिओ कधीही तुमच्या डिव्हाइसवरून बाहेर जात नाही
- Python, JavaScript, आणि C# ट्रॅक्ससह `client.audio.transcriptions.create()` (Python/JS) आणि `AudioClient.TranscribeAudioAsync()` (C#)
- हँड्स-ऑन प्रॅक्टिससाठी Zava-थीम असलेले नमुना ऑडिओ फायली अंतर्भूत

**कोड नमुने:**

| भाषा | फाइल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | व्हिस्पर व्हॉइस ट्रान्सक्रिप्शन |
| C# | `csharp/WhisperTranscription.cs` | व्हिस्पर व्हॉइस ट्रान्सक्रिप्शन |
| JavaScript | `javascript/foundry-local-whisper.mjs` | व्हिस्पर व्हॉइस ट्रान्सक्रिप्शन |

> **टीप:** हा लॅब **Foundry Local SDK** वापरून व्हिस्पर मॉडेल प्रोग्रामॅटिकली डाउनलोड आणि लोड करतो, नंतर स्थानिक OpenAI-सुसंगत एंडपॉइंटवर ऑडिओ ट्रान्सक्रिप्शनसाठी पाठवतो. व्हिस्पर मॉडेल (`whisper`) Foundry Local कॅटलॉगमध्ये सूचीबद्ध आहे आणि पूर्णपणे डिव्हाइसवर चालते - कोणतेही क्लाउड API की किंवा नेटवर्क प्रवेश आवश्यक नाही.

---

### भाग १०: कस्टम किंवा हगिंग फेस मॉडेल्सचा वापर

**लॅब मार्गदर्शक:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- हगिंग फेस मॉडेल्स ONNX Runtime GenAI मॉडेल बिल्डर वापरून ऑप्टिमाइझ्ड ONNX स्वरूपात संकलित करणे
- हार्डवेअर-विशिष्ट संकलन (CPU, NVIDIA GPU, DirectML, WebGPU) आणि क्वांटायझेशन (int4, fp16, bf16)
- Foundry Local साठी चैट-टेम्प्लेट कॉन्फिगरेशन फाइल्स तयार करणे
- संकलित मॉडेल्स Foundry Local कॅशमध्ये जोडणे
- CLI, REST API, आणि OpenAI SDK द्वारे कस्टम मॉडेल्स चालवणे
- संदर्भ उदाहरण: Qwen/Qwen3-0.6B अखंड संकलन

---

### भाग ११: स्थानिक मॉडेल्ससह टूल कॉलिंग

**लॅब मार्गदर्शक:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- स्थानिक मॉडेल्सना बाह्य फंक्शन्स (टूल/फंक्शन कॉलिंग) कॉल करण्याची अनुमती देणे
- OpenAI फंक्शन-कॉलिंग फॉर्मॅट वापरून टूल स्कीम्स परिभाषित करणे
- मल्टी-टर्न टूल-कॉलिंग संभाषण प्रवाह हाताळणे
- टूल कॉल स्थानिकरित्या अंमलात आणणे आणि निकाल मॉडेलमध्ये परत करणे
- टूल-कॉलिंग परिस्थितीसाठी योग्य मॉडेल निवडणे (Qwen 2.5, Phi-4-mini)
- टूल कॉलिंगसाठी SDK च्या नेटिव्ह `ChatClient` चा वापर करणे (JavaScript)

**कोड नमुने:**

| भाषा | फाइल | वर्णन |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | हवामान/लोकसंख्या टूल्ससह टूल कॉलिंग |
| C# | `csharp/ToolCalling.cs` | .NET सह टूल कॉलिंग |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient सह टूल कॉलिंग |

---

### भाग १२: Zava क्रिएटिव लेखकासाठी वेब UI बनविणे

**लॅब मार्गदर्शक:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava क्रिएटिव लेखकासाठी ब्राउझर-आधारित फ्रंट एंड जोडणे
- Python (FastAPI), JavaScript (Node.js HTTP), आणि C# (ASP.NET Core) पासून सामायिक UI सर्व्ह करणे
- Fetch API आणि ReadableStream वापरून ब्राउझरमध्ये स्ट्रीमिंग NDJSON वापरणे
- लाइव्ह एजंट स्टेटस बॅजेस आणि रिअल-टाइम लेख टेक्स्ट स्ट्रीमिंग

**कोड (सामायिक UI):**

| फाइल | वर्णन |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | पृष्ठ लेआउट |
| `zava-creative-writer-local/ui/style.css` | स्टाइलिंग |
| `zava-creative-writer-local/ui/app.js` | स्ट्रीम रीडर आणि DOM अपडेट लॉजिक |

**बॅकएंड जोडण्याः**

| भाषा | फाइल | वर्णन |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | स्थिर UI सर्व्ह करण्यासाठी अद्ययावत |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ऑर्केस्ट्रेटर वेढणारा नवीन HTTP सर्व्हर |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | नवीन ASP.NET Core मिमिनल API प्रकल्प |

---

### भाग १३: कार्यशाळा पूर्ण

**लॅब मार्गदर्शक:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- सर्व १२ भागांमध्ये तुम्ही जे काही तयार केले त्याचा सारांश
- तुमच्या अनुप्रयोगांच्या विस्तारासाठी पुढील कल्पना
- संसाधने आणि दस्तऐवजीकरणासाठी दुवे

---

## प्रकल्प रचना

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
| सुरूवातीचा मार्गदर्शक | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK संदर्भ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft एजंट फ्रेमवर्क | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## परवाना

ही कार्यशाळा सामग्री शैक्षणिक हेतूंसाठी प्रदान केली आहे.

---

**बांधकामाचा आनंद घ्या! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:
हा दस्तऐवज AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) वापरून अनुवादित केला आहे. आम्ही अचूकतेसाठी प्रयत्नशील असले तरी, कृपया लक्षात घ्या की स्वयंचलित अनुवादांमध्ये चुका किंवा अचूकतेच्या त्रुटी असू शकतात. मूळ दस्तऐवज त्याच्या स्थानिक भाषेत अधिकृत स्रोत मानावा. महत्वाच्या माहितीकरिता व्यावसायिक मानव अनुवादाची शिफारस केली जाते. या अनुवादाच्या वापरामुळे उद्भवणाऱ्या कोणत्याही गैरसमजुती किंवा चुकीच्या अर्थलहरीसाठी आम्ही जबाबदार नाही.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->