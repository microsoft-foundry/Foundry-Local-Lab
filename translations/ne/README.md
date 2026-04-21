<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local कार्यशाला - उपकरणमा AI अनुप्रयोगहरू बनाउनुहोस्

आफ्नो कम्प्युटरमा भाषा मोडेलहरू चलाउन र [Foundry Local](https://foundrylocal.ai) र [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) सँग सुझबुझपूर्ण अनुप्रयोगहरू निर्माण गर्ने व्यावहारिक कार्यशाला।

> **Foundry Local के हो?** Foundry Local एक हल्का रनटाइम हो जसले तपाईंको हार्डवेयरमा सम्पूर्ण रूपमा भाषा मोडेलहरू डाउनलोड, व्यवस्थापन, र सेवा गर्न दिन्छ। यसले **OpenAI-अनुकूल API** प्रकट गर्दछ जसले OpenAI sँग कुरा गर्ने कुनै पनि उपकरण वा SDK लाई जडान गर्न दिन्छ - कुनै क्लाउड खाता आवश्यक छैन।

### 🌐 बहुभाषिक समर्थन

#### GitHub क्रियाकलापमार्फत समर्थित (स्वचालित र सधैं अपडेट)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](./README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **स्थानीय रूपमा क्लोन गर्न मन छ?**
>
> यो रिपोजिटरीमा ५०+ भाषा अनुवादहरू समावेश छन् जसले डाउनलोड आकार उल्लेखनीय रूपमा बढाउँछ। अनुवाद बिना क्लोन गर्न sparse checkout प्रयोग गर्नुहोस्:
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
> यसले तपाईंलाई छिटो डाउनलोडको साथ कोर्स पूरा गर्न आवश्यक सबै थोक दिन्छ।
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## सिकाइका उद्देश्यहरू

यस कार्यशालाको अन्त्यसम्म तपाईं सक्षम हुनुहुनेछ:

| # | उद्देश्य |
|---|----------|
| 1 | Foundry Local स्थापना गर्न र CLI बाट मोडेलहरू व्यवस्थापन गर्न |
| 2 | Foundry Local SDK API कदर गरेर कार्यक्रमगत मोडेल व्यवस्थापनमा दक्ष बन्न |
| 3 | Python, JavaScript, र C# SDK मार्फत स्थानीय इनफरेन्स सर्भरसँग जडान हुन |
| 4 | आफ्नै डाटामा आधारित उत्तरहरूको लागि Retrieval-Augmented Generation (RAG) पाइपलाइन निर्माण गर्न |
| 5 | स्थायी निर्देशन र व्यक्तित्वहरू सहित AI एजेन्टहरू सिर्जना गर्न |
| 6 | फिडब्याक लूपहरू सहित बहु-एजेन्ट कार्यप्रवाहहरू सञ्चालक गर्न |
| 7 | उत्पादन स्तरको capstone एप्लिकेसन - Zava Creative Writer अन्वेषण गर्न |
| 8 | सुनौलो डेटासेट र LLM-एज-निर्णायक स्कोरिङ प्रयोग गरेर मूल्यांकन ढाँचा निर्माण गर्न |
| 9 | Whisper प्रयोग गरेर अडियो ट्रान्सक्रिप्ट गर्न - उपकरणमा बोलेकोलाई टेक्स्टमा रुपान्तरण (speech-to-text) |
| 10 | ONNX Runtime GenAI र Foundry Local सँग कस्टम वा Hugging Face मोडेल कम्पाइल र चलाउन |
| 11 | टुल-कलिङ ढाँचामा स्थानीय मोडेलहरूले बाह्य फंक्शनहरू कल गर्न सक्षम बनाउन |
| 12 | Zava Creative Writer लेख्न ब्राउजर-आधारित UI बनाउनुहोस् साथमा वास्तविक-समय स्ट्रिमिङ |

---

## आवश्यकताहरू

| आवश्यकताहरू | विवरणहरू |
|-------------|-----------|
| **हार्डवेयर** | कम्तीमा ८ GB RAM (१६ GB सिफारिस गरिएको); AVX2 समर्थित CPU वा समर्थित GPU |
| **अपरेटिङ सिस्टम** | Windows 10/11 (x64/ARM), Windows Server 2025, वा macOS 13+ |
| **Foundry Local CLI** | Windows मा `winget install Microsoft.FoundryLocal` वा macOS मा `brew tap microsoft/foundrylocal && brew install foundrylocal` बाट स्थापना गर्नुहोस्। विस्तृत जानकारीको लागि [सुरु गर्न मार्गदर्शन](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) हेर्नुहोस्। |
| **भाषा रनटाइम** | **Python 3.9+** र/वा **.NET 9.0+** र/वा **Node.js 18+** |
| **Git** | यो रिपोजिटरी क्लोन गर्न |

---

## सुरु गर्ने तरिका

```bash
# 1. रिपोजिटरी क्लोन गर्नुहोस्
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. फाउन्ड्री लोकल स्थापना गरिएको छ कि छैन जाँच गर्नुहोस्
foundry model list              # उपलब्ध मोडेलहरू सूचीबद्ध गर्नुहोस्
foundry model run phi-3.5-mini  # एक अन्तरक्रियात्मक च्याट सुरु गर्नुहोस्

# 3. आफ्नो भाषा ट्रयाक छनोट गर्नुहोस् (पूर्ण सेटअपका लागि भाग २ प्रयोगशाला हेर्नुहोस्)
```

| भाषा | छिटो सुरु गर्नुहोस् |
|-------|---------------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## कार्यशालाका भागहरू

### भाग १: Foundry Local सँग परिचय

**प्रयोगशाला मार्गदर्शन:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local के हो र यसले कसरी काम गर्छ
- Windows र macOS मा CLI स्थापना गर्दै
- मोडेलहरू अन्वेषण गर्दै - सूची, डाउनलोड, चलाउने
- मोडेल उपनाम र गतिशील पोर्टहरू बुझ्ने

---

### भाग २: Foundry Local SDK गहिराइमा

**प्रयोगशाला मार्गदर्शन:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- अनुप्रयोग विकासका लागि CLI भन्दा SDK किन प्रयोग गर्ने
- Python, JavaScript, र C# का लागि पूर्ण SDK API सन्दर्भ
- सेवा व्यवस्थापन, क्याटलग ब्राउजिंग, मोडेल जीवनचक्र (डाउनलोड, लोड, अनलोड)
- छिटो सुरु गर्ने ढाँचाहरू: Python कन्स्ट्रक्टर बुटस्ट्र्याप, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` मेटाडाटा, उपनामहरू, र हार्डवेयर उपयुक्त मोडेल छनोट

---

### भाग ३: SDKs र APIहरू

**प्रयोगशाला मार्गदर्शन:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, र C# बाट Foundry Local सँग जडान हुनु
- सेवा कार्यक्रमगत रूपमा व्यवस्थापन गर्न Foundry Local SDK प्रयोग गर्दै
- OpenAI-अनुकूल API मार्फत स्ट्रीमिंग च्याट कम्प्लिशन्स
- प्रत्येक भाषाका लागि SDK विधि सन्दर्भ

**कोड नमूना:**

| भाषा | फाइल | विवरण |
|-------|-------|---------|
| Python | `python/foundry-local.py` | आधारभूत स्ट्रिमिङ च्याट |
| C# | `csharp/BasicChat.cs` | .NET सहित स्ट्रिमिङ च्याट |
| JavaScript | `javascript/foundry-local.mjs` | Node.js सँग स्ट्रिमिङ च्याट |

---

### भाग ४: Retrieval-Augmented Generation (RAG)

**प्रयोगशाला मार्गदर्शन:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG के हो र किन महत्व राख्छ
- इन-मेमोरी ज्ञान आधार निर्माण गर्ने
- कीवर्ड-अवलोकन पुनःप्राप्ति स्कोरिङसहित
- आधारित प्रणाली प्रॉम्प्टहरू संकलन गर्ने
- उपकरणमा पूर्ण RAG पाइपलाइन चलाउने

**कोड नमूना:**

| भाषा | फाइल |
|-------|--------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### भाग ५: AI एजेन्टहरू बनाउँदै

**प्रयोगशाला मार्गदर्शन:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI एजेन्ट के हो (सिधा LLM कल भन्दा)
- `ChatAgent` ढाँचा र Microsoft Agent Framework
- प्रणाली निर्देशनहरू, व्यक्तित्वहरू, र बहु-टर्न कुराकानीहरू
- एजेन्टहरूबाट संरचित आउटपुट (JSON)

**कोड नमूना:**

| भाषा | फाइल | विवरण |
|-------|-------|---------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework सहित एकल एजेन्ट |
| C# | `csharp/SingleAgent.cs` | एकल एजेन्ट (`ChatAgent` ढाँचा) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | एकल एजेन्ट (`ChatAgent` ढाँचा) |

---

### भाग ६: बहु-एजेन्ट कार्यप्रवाह

**प्रयोगशाला मार्गदर्शन:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- बहु-एजेन्ट पाइपलाइनहरू: अनुसन्धानकर्ता → लेखक → सम्पादक
- अनुक्रमिक सञ्चालन र फिडब्याक लूपहरू
- साझा संरचना र संरचित हस्तान्तरणहरू
- आफ्नै बहु-एजेन्ट कार्यप्रवाह डिजाइन गर्ने

**कोड नमूना:**

| भाषा | फाइल | विवरण |
|-------|-------|---------|
| Python | `python/foundry-local-multi-agent.py` | तीन-एजेन्ट पाइपलाइन |
| C# | `csharp/MultiAgent.cs` | तीन-एजेन्ट पाइपलाइन |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | तीन-एजेन्ट पाइपलाइन |

---

### भाग ७: Zava Creative Writer - क्यापस्टोन अनुप्रयोग

**प्रयोगशाला मार्गदर्शन:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- ४ विशेष एजेन्टहरू सहित उत्पादन-शैलीको बहु-एजेन्ट एप
- समीक्षा-चालित फिडब्याक लूपहरू सहित अनुक्रमिक पाइपलाइन
- स्ट्रिमिङ आउटपुट, उत्पादन क्याटलग खोज, संरचित JSON हस्तान्तरणहरू
- Python (FastAPI), JavaScript (Node.js CLI), र C# (.NET कन्सोल) मा पूर्ण कार्यान्वयन

**कोड नमूना:**

| भाषा | डिरेक्टरी | विवरण |
|-------|------------|---------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI वेब सेवा र आयोजक सहित |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI अनुप्रयोग |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 कन्सोल अनुप्रयोग |

---

### भाग ८: मूल्यांकन-नेतृत्व विकास

**प्रयोगशाला मार्गदर्शन:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- सुनौलो डेटासेटसँग AI एजेन्टहरूको लागि एक प्रणालीगत मूल्यांकन ढाँचा निर्माण
- नियम-आधारित जाँचहरू (लम्बाइ, कीवर्ड कवरेज, निषेधित शब्दहरू) + LLM-एज-निर्णायक स्कोरिङ
- प्रॉम्प्ट भेरियन्टहरूको साइड-बाय-साइड तुलना समेकित स्कोरकार्डसहित
- भाग ७ को Zava Editor एजेन्ट ढाँचालाई अफलाइन परीक्षण सूटमा विस्तार गरेको
- Python, JavaScript, र C# ट्रयाकहरू

**कोड नमूना:**

| भाषा | फाइल | विवरण |
|-------|-------|---------|
| Python | `python/foundry-local-eval.py` | मूल्यांकन ढाँचा |
| C# | `csharp/AgentEvaluation.cs` | मूल्यांकन ढाँचा |
| JavaScript | `javascript/foundry-local-eval.mjs` | मूल्यांकन ढाँचा |

---

### भाग ९: Whisper प्रयोग गरेर स्वर ट्रान्सक्रिप्शन

**प्रयोगशाला मार्गदर्शन:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- OpenAI Whisper प्रयोग गरेर स्थानिय रूपमा चलिरहेको स्पीच-देखि-पाठ ट्रान्सक्रिप्सन
- गोपनीयताको प्राथमिकता रहेको अडियो प्रक्रिया - अडियो कहिल्यै तपाईँको उपकरणबाट बाहिर जान्दैन
- Python, JavaScript, र C# ट्र्याकहरू `client.audio.transcriptions.create()` (Python/JS) र `AudioClient.TranscribeAudioAsync()` (C#) सँग
- हात्ती अभ्यासको लागि Zava थीम गरिएको नमूना अडियो फाइलहरू सहित

**कोड नमूना:**

| भाषा | फाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper भ्वाइस ट्रान्सक्रिप्सन |
| C# | `csharp/WhisperTranscription.cs` | Whisper भ्वाइस ट्रान्सक्रिप्सन |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper भ्वाइस ट्रान्सक्रिप्सन |

> **Note:** यो ल्याब **Foundry Local SDK** प्रयोग गरी प्रोग्रामेटिक रूपमा Whisper मोडेल डाउनलोड र लोड गर्छ, त्यसपछि अडियोलाई स्थानिय OpenAI-मिल्दो अन्त बिन्दुमा ट्रान्सक्रिप्सनको लागि पठाउँछ। Whisper मोडेल (`whisper`) Foundry Local कैटलॉगमा सूचीबद्ध छ र पूर्णतः उपकरणमै चल्छ - कुनै क्लाउड API किज वा नेटवर्क पहुँच आवश्यक छैन।

---

### भाग 10: कस्टम वा Hugging Face मोडेलहरू प्रयोग गर्दै

**ल्याब गाइड:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Hugging Face मोडेलहरूलाई ONNX Runtime GenAI मोडेल बिल्डर प्रयोग गरी अनुकूलित ONNX फारम्याटमा कम्पाइल गर्ने
- हार्डवेयर-विशेष कम्पाइल (CPU, NVIDIA GPU, DirectML, WebGPU) र क्वान्टाइजेसन (int4, fp16, bf16)
- Foundry Local का लागि च्याट-टेम्प्लेट कन्फिगरेसन फाइलहरू सिर्जना गर्ने
- कम्पाइल गरिएका मोडेलहरूलाई Foundry Local क्यासमा थप्ने
- CLI, REST API, र OpenAI SDK मार्फत कस्टम मोडेलहरू चलाउने
- सन्दर्भ उदाहरण: Qwen/Qwen3-0.6B को सम्पूर्ण कम्पाइलिङ

---

### भाग 11: स्थानिय मोडेलहरूसँग टूल कलिंग

**ल्याब गाइड:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- स्थानिय मोडेलहरूलाई बाह्य फंक्शनहरू (टूल/फंक्शन कलिंग) कल गर्न सक्षम पार्ने
- OpenAI फंक्शन-कलिंग फार्म्याट प्रयोग गरी टूल स्किमाहरू परिभाषित गर्ने
- बहु-टर्न टूल-कलिंग संवाद प्रवाह सम्हाल्ने
- स्थानिय रूपमा टूल कलहरू कार्यान्वयन गरी नतिजा मोडेलमा फिर्ता गर्ने
- टूल-कलिंग परिस्थितिहरूको लागि उपयुक्त मोडेल चयन गर्ने (Qwen 2.5, Phi-4-mini)
- SDK को देशज `ChatClient` प्रयोग गरी टूल कलिंग गर्ने (JavaScript)

**कोड नमूना:**

| भाषा | फाइल | विवरण |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | मौसम/जनसंख्या टूलहरूसँग टूल कलिंग |
| C# | `csharp/ToolCalling.cs` | .NET सँग टूल कलिंग |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient सँग टूल कलिंग |

---

### भाग 12: Zava Creative Writer को लागि वेब UI निर्माण

**ल्याब गाइड:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer लाई ब्राउजर-आधारित फ्रन्टएन्ड थप्ने
- Python (FastAPI), JavaScript (Node.js HTTP), र C# (ASP.NET Core) बाट साझा UI सर्भ गर्ने
- ब्राउजरमा Fetch API र ReadableStream प्रयोग गरी स्ट्रीमिङ NDJSON उपभोग गर्ने
- लाइव एजेन्ट स्टाटस बैजहरू र वास्तविक समयमा लेखको पाठ स्ट्रीमिङ

**कोड (साझा UI):**

| फाइल | विवरण |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | पृष्ठ लेआउट |
| `zava-creative-writer-local/ui/style.css` | शैलीकरण |
| `zava-creative-writer-local/ui/app.js` | स्ट्रिम रिसीभर र DOM अपडेट लॉजिक |

**ब्याकएन्ड थपहरू:**

| भाषा | फाइल | विवरण |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | स्थिर UI सेवा गर्न अपडेट गरिएको |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | नयाँ HTTP सर्भर जो अर्चेस्ट्रेटरलाई र्याप गर्छ |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | नयाँ ASP.NET Core न्यूनतम API प्रोजेक्ट |

---

### भाग 13: कार्यशाला पूरा

**ल्याब गाइड:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- तपाईंले सबै १२ भागहरूमा निर्माण गरेको कुरा सारांश
- तपाईंका अनुप्रयोगहरू विस्तार गर्ने थप विचारहरू
- स्रोत र कागजातहरूका लिंकहरू

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

## स्रोतहरू

| स्रोत | लिंक |
|----------|------|
| Foundry Local वेबसाइट | [foundrylocal.ai](https://foundrylocal.ai) |
| मोडेल कैटलॉग | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| सुरु गर्ने गाइड | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK सन्दर्भ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft एजेन्ट फ्रेमवर्क | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## लाइसेन्स

यो कार्यशाला सामग्री शैक्षिक प्रयोजनका लागि प्रदान गरिएको हो।

---

**सफल निर्माण! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:
यस कागजातलाई AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) प्रयोग गरेर अनुवाद गरिएको हो। हामी शुद्धताको प्रयास गर्छौं, तर कृपया जान्नुहोस् कि स्वतः अनुवादहरूमा त्रुटि वा असत्यता हुन सक्छ। मूल कागजात यसको मातृ भाषामा आधिकारिक स्रोत मानिनुपर्छ। महत्वपूर्ण जानकारीका लागि व्यावसायिक मानव अनुवाद सिफारिस गरिन्छ। यस अनुवादको प्रयोगबाट उत्पन्न कुनै पनि गलतफहमी वा गलत व्याख्याका लागि हामी जिम्मेवार छैनौं।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->