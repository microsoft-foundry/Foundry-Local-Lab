![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग 10: Foundry Local के साथ कस्टम या Hugging Face मॉडल का उपयोग करना

> **लक्ष्य:** Hugging Face मॉडल को Foundry Local के लिए आवश्यक अनुकूलित ONNX फॉर्मेट में संकलित करना, इसे चैट टेम्पलेट के साथ कॉन्फ़िगर करना, स्थानीय कैश में जोड़ना, और CLI, REST API, और OpenAI SDK का उपयोग करके इसके खिलाफ इंफरेंस चलाना।

## अवलोकन

Foundry Local पूर्व-संकलित मॉडल के एक क्यूरेटेड कैटलॉग के साथ आता है, लेकिन आप उस सूची तक सीमित नहीं हैं। Hugging Face पर उपलब्ध कोई भी ट्रांसफॉर्मर-आधारित भाषा मॉडल (या स्थानीय रूप से PyTorch / Safetensors फॉर्मेट में स्टोर किया गया) अनुकूलित ONNX मॉडल में संकलित किया जा सकता है और Foundry Local के माध्यम से सेवा प्रदान की जा सकती है।

संकलन पाइपलाइन **ONNX Runtime GenAI Model Builder** का उपयोग करती है, जो `onnxruntime-genai` पैकेज के साथ शामिल एक कमांड-लाइन टूल है। मॉडल बिल्डर डाउनलोडिंग, ONNX फॉर्मेट में कन्वर्टिंग, क्वांटाइजेशन (int4, fp16, bf16) लागू करने, और कॉन्फ़िगरेशन फाइलें (चैट टेम्पलेट और टोकनाइज़र सहित) उत्पन्न करने का कार्य संभालता है जिनकी Foundry Local को आवश्यकता होती है।

इस लैब में आप Hugging Face से **Qwen/Qwen3-0.6B** को संकलित करेंगे, इसे Foundry Local के साथ पंजीकृत करेंगे, और पूरी तरह से अपने डिवाइस पर इसके साथ चैट करेंगे।

---

## सीखने के उद्देश्य

इस लैब के अंत तक आप सक्षम होंगे:

- समझें कि कस्टम मॉडल संकलन क्यों उपयोगी है और कब इसकी आवश्यकता हो सकती है
- ONNX Runtime GenAI मॉडल बिल्डर स्थापित करें
- एक ही कमांड से Hugging Face मॉडल को अनुकूलित ONNX फॉर्मेट में संकलित करना
- मुख्य संकलन पैरामीटर (execution provider, precision) को समझना
- `inference_model.json` चैट-टेम्पलेट कॉन्फ़िगरेशन फाइल बनाना
- संकलित मॉडल को Foundry Local कैश में जोड़ना
- CLI, REST API, और OpenAI SDK का उपयोग करके कस्टम मॉडल के खिलाफ इनफेरेंस चलाना

---

## पूर्वापेक्षाएँ

| आवश्यकता | विवरण |
|-------------|---------|
| **Foundry Local CLI** | इंस्टॉल किया गया और आपके `PATH` में ([भाग 1](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI मॉडल बिल्डर के लिए आवश्यक |
| **pip** | Python पैकेज मैनेजर |
| **डिस्क स्पेस** | स्रोत और संकलित मॉडल फाइलों के लिए कम से कम 5 GB खाली |
| **Hugging Face खाता** | कुछ मॉडल डाउनलोड करने से पहले लाइसेंस स्वीकार करना आवश्यक है। Qwen3-0.6B Apache 2.0 लाइसेंस का उपयोग करता है और यह मुफ्त में उपलब्ध है। |

---

## पर्यावरण सेटअप

मॉडल संकलन के लिए कई बड़े Python पैकेज (PyTorch, ONNX Runtime GenAI, Transformers) की आवश्यकता होती है। इसलिए एक समर्पित वर्चुअल एनवायरनमेंट बनाएं ताकि ये आपके सिस्टम Python या अन्य प्रोजेक्ट्स के साथ हस्तक्षेप न करें।

```bash
# रिपॉजिटरी रूट से
python -m venv .venv
```

एनवायरनमेंट सक्रिय करें:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

पिप को अपडेट करें ताकि निर्भरता समाधान की समस्याएं न हों:

```bash
python -m pip install --upgrade pip
```

> **टिप:** यदि आपके पास पहले से `.venv` है जो पिछली लैब्स से है, तो आप इसे पुनः उपयोग कर सकते हैं। बस सुनिश्चित करें कि यह सक्रिय है जब आप आगे बढ़ें।

---

## अवधारणा: संकलन पाइपलाइन

Foundry Local को ONNX फॉर्मेट और ONNX Runtime GenAI कॉन्फ़िगरेशन में मॉडल चाहिए। अधिकांश ओपन-सोर्स मॉडल Hugging Face पर PyTorch या Safetensors वज़न के रूप में वितरित किए जाते हैं, इसलिए कन्वर्ज़न चरण आवश्यक होता है।

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### मॉडल बिल्डर क्या करता है?

1. Hugging Face से स्रोत मॉडल **डाउनलोड** करता है (या यह स्थानीय पथ से पढ़ता है)।
2. PyTorch / Safetensors वज़न को ONNX फॉर्मेट में **कन्वर्ट** करता है।
3. मेमोरी उपयोग कम करने और थ्रूपुट बढ़ाने के लिए मॉडल को छोटी प्रिसिशन (जैसे int4) में **क्वांटाइज** करता है।
4. ONNX Runtime GenAI कॉन्फ़िगरेशन (`genai_config.json`), चैट टेम्पलेट (`chat_template.jinja`), और सभी टोकनाइज़र फाइलें जेनरेट करता है ताकि Foundry Local मॉडल को लोड और सेवा प्रदान कर सके।

### ONNX Runtime GenAI Model Builder बनाम Microsoft Olive

आपको **Microsoft Olive** नामक एक वैकल्पिक उपकरण के संदर्भ मिल सकते हैं जो मॉडल ऑप्टिमाइजेशन के लिए है। दोनों उपकरण ONNX मॉडल बना सकते हैं, लेकिन वे विभिन्न उपयोग के लिए हैं और उनके दांव-पेंच अलग हैं:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **पैकेज** | `onnxruntime-genai` | `olive-ai` |
| **मुख्य उद्देश्य** | ONNX Runtime GenAI इंफरेंस के लिए जेनरेटिव AI मॉडलों को कन्वर्ट और क्वांटाइज करना | कई बैकेंड और हार्डवेयर टारगेट्स का समर्थन करने वाला एंड-टू-एंड मॉडल ऑप्टिमाइजेशन फ्रेमवर्क |
| **उपयोग में सरलता** | एक ही कमांड — एक-स्टेप कन्वर्ज़न + क्वांटाइजेशन | वर्कफ़्लो-आधारित — YAML/JSON से कॉन्फ़िगर करने योग्य मल्टी-पास पाइपलाइन |
| **आउटपुट फॉर्मेट** | ONNX Runtime GenAI फॉर्मेट (Foundry Local के लिए तैयार) | वर्कफ़्लो पर निर्भर करते हुए Generic ONNX, ONNX Runtime GenAI, या अन्य फॉर्मेट |
| **हार्डवेयर टारगेट्स** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, आदि |
| **क्वांटाइजेशन विकल्प** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, साथ में ग्राफ ऑप्टिमाइजेशन, लेयर-वाइज ट्यूनिंग |
| **मॉडल स्कोप** | जेनरेटिव AI मॉडल (LLMs, SLMs) | कोई भी ONNX-कन्वर्टेबल मॉडल (विजन, NLP, ऑडियो, मल्टीमॉडल) |
| **सबसे अच्छा उपयोग** | स्थानीय इंफरेंस के लिए त्वरित एकल-मॉडल संकलन | उत्पादन पाइपलाइन जिसमें सूक्ष्म ऑप्टिमाइजेशन नियंत्रण चाहिए |
| **निर्भरता का पदचिह्न** | मध्यम (PyTorch, Transformers, ONNX Runtime) | बड़ा (Olive फ्रेमवर्क, और वर्कफ़्लो पर वैकल्पिक एक्स्ट्रा आदि) |
| **Foundry Local एकीकरण** | सीधे — आउटपुट तुरंत संगत | `--use_ort_genai` फ़्लैग और अतिरिक्त कॉन्फ़िगरेशन की आवश्यकता |

> **यह लैब मॉडल बिल्डर क्यों उपयोग करती है:** एक Hugging Face मॉडल को संकलित करना और इसे Foundry Local के साथ पंजीकृत करना सबसे सरल और विश्वसनीय रास्ता मॉडल बिल्डर है। यह एक कमांड में Foundry Local को आवश्यक सटीक आउटपुट फॉर्मेट उत्पन्न करता है। यदि भविष्य में आपको उन्नत ऑप्टिमाइजेशन की आवश्यकता हो — जैसे कि सटीकता-जागरूक क्वांटाइजेशन, ग्राफ सर्जरी, या मल्टी-पास ट्यूनिंग — तो Olive एक शक्तिशाली विकल्प है। अधिक जानकारी के लिए [Microsoft Olive दस्तावेज़](https://microsoft.github.io/Olive/) देखें।

---

## लैब अभ्यास

### अभ्यास 1: ONNX Runtime GenAI Model Builder स्थापित करें

ONNX Runtime GenAI पैकेज स्थापित करें, जिसमें मॉडल बिल्डर टूल शामिल है:

```bash
pip install onnxruntime-genai
```

स्थापना सत्यापित करें कि मॉडल बिल्डर उपलब्ध है:

```bash
python -m onnxruntime_genai.models.builder --help
```

आपको मदद आउटपुट में पैरामीटर जैसे `-m` (मॉडल नाम), `-o` (आउटपुट पथ), `-p` (प्रिसिशन), और `-e` (एग्जीक्यूशन प्रोवाइडर) दिखाई देंगे।

> **नोट:** मॉडल बिल्डर PyTorch, Transformers, और कई अन्य पैकेजों पर निर्भर करता है। इंस्टॉलेशन में कुछ मिनट लग सकते हैं।

---

### अभ्यास 2: CPU के लिए Qwen3-0.6B संकलित करें

नीचे दिया गया कमांड Hugging Face से Qwen3-0.6B मॉडल डाउनलोड करता है और int4 क्वांटाइजेशन के साथ CPU इंफरेंस के लिए संकलित करता है:

**macOS / Linux:**
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```

**Windows (PowerShell):**
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```

#### प्रत्येक पैरामीटर क्या करता है

| पैरामीटर | उद्देश्य | प्रयुक्त मान |
|-----------|---------|------------|
| `-m` | Hugging Face मॉडल ID या स्थानीय डायरेक्टरी पथ | `Qwen/Qwen3-0.6B` |
| `-o` | वह डायरेक्टरी जहाँ संकलित ONNX मॉडल सहेजा जाएगा | `models/qwen3` |
| `-p` | संकलन के दौरान लागू क्वांटाइजेशन प्रिसिशन | `int4` |
| `-e` | ONNX Runtime एग्जीक्यूशन प्रोवाइडर (टारगेट हार्डवेयर) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face प्रमाणीकरण को छोड़ देता है (सार्वजनिक मॉडल के लिए ठीक) | `hf_token=false` |

> **इसमें कितना समय लगता है?** संकलन का समय आपके हार्डवेयर और मॉडल के आकार पर निर्भर करता है। Qwen3-0.6B के लिए int4 क्वांटाइजेशन के साथ आधुनिक CPU पर लगभग 5 से 15 मिनट लग सकते हैं। बड़े मॉडल के लिए यह समय अनुपातिक रूप से अधिक होगा।

जब कमांड पूरी हो जाए, तो आपको `models/qwen3` नामक डायरेक्टरी दिखनी चाहिए जिसमें संकलित मॉडल फाइलें हों। आउटपुट सत्यापित करें:

```bash
ls models/qwen3
```

आपको निम्नलिखित फाइलें दिखनी चाहिए:
- `model.onnx` और `model.onnx.data` — संकलित मॉडल वज़न
- `genai_config.json` — ONNX Runtime GenAI कॉन्फिगरेशन
- `chat_template.jinja` — मॉडल का चैट टेम्पलेट (स्वत: उत्पन्न)
- `tokenizer.json`, `tokenizer_config.json` — टोकनाइज़र फाइलें
- अन्य शब्दावली और कॉन्फिगरेशन फाइलें

---

### अभ्यास 3: GPU के लिए संकलित करें (वैकल्पिक)

यदि आपके पास CUDA समर्थन वाला NVIDIA GPU है, तो आप तेज़ इंफरेंस के लिए GPU-ऑप्टिमाइज्ड वेरिएंट संकलित कर सकते हैं:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **नोट:** GPU संकलन के लिए `onnxruntime-gpu` और कार्यशील CUDA इंस्टॉलेशन आवश्यक है। यदि ये नहीं हैं, तो मॉडल बिल्डर एक त्रुटि रिपोर्ट करेगा। आप यह अभ्यास छोड़कर CPU वेरिएंट के साथ आगे बढ़ सकते हैं।

#### हार्डवेयर-विशिष्ट संकलन संदर्भ

| लक्ष्य | एग्जीक्यूशन प्रोवाइडर (`-e`) | अनुशंसित प्रिसिशन (`-p`) |
|--------|-------------------------------|----------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` या `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` या `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### प्रिसिशन ट्रेड-ऑफ़

| प्रिसिशन | आकार | गति | गुणवत्ता |
|-----------|------|-------|---------|
| `fp32` | सबसे बड़ा | सबसे धीमा | सर्वोच्च सटीकता |
| `fp16` | बड़ा | तेज (GPU) | बहुत अच्छी सटीकता |
| `int8` | छोटा | तेज | थोड़ी सटीकता हानि |
| `int4` | सबसे छोटा | सबसे तेज़ | मध्यम सटीकता हानि |

अधिकांश स्थानीय विकास के लिए, CPU पर `int4` सर्वोत्तम गति और संसाधन उपयोग का संतुलन प्रदान करता है। उत्पादन गुणवत्ता के आउटपुट के लिए, CUDA GPU पर `fp16` अनुशंसित है।

---

### अभ्यास 4: चैट टेम्पलेट कॉन्फ़िगरेशन बनाएं

मॉडल बिल्डर आउटपुट डायरेक्टरी में स्वतः ही `chat_template.jinja` और `genai_config.json` फाइलें बनाता है। हालांकि, Foundry Local को यह समझने के लिए `inference_model.json` फाइल की भी आवश्यकता होती है कि आपके मॉडल के लिए प्रॉम्प्ट्स को कैसे फॉर्मेट किया जाए। यह फाइल मॉडल का नाम और ऐसा प्रॉम्प्ट टेम्पलेट परिभाषित करती है जो उपयोगकर्ता संदेशों को सही विशेष टोकन्स में लपेटता है।

#### चरण 1: संकलित आउटपुट देखें

संकलित मॉडल डायरेक्टरी की सामग्री सूचीबद्ध करें:

```bash
ls models/qwen3
```

आपको निम्न फाइलें दिखनी चाहिए:
- `model.onnx` और `model.onnx.data` — संकलित मॉडल वज़न
- `genai_config.json` — ONNX Runtime GenAI कॉन्फिगरेशन (स्वत: उत्पन्न)
- `chat_template.jinja` — मॉडल का चैट टेम्पलेट (स्वत: उत्पन्न)
- `tokenizer.json`, `tokenizer_config.json` — टोकनाइज़र फाइलें
- विभिन्न अन्य कॉन्फ़िगरेशन और शब्दावली फाइलें

#### चरण 2: inference_model.json फाइल जनरेट करें

`inference_model.json` फाइल Foundry Local को बताती है कि प्रॉम्प्ट कैसे फॉर्मेट करना है। एक Python स्क्रिप्ट `generate_chat_template.py` बनाएं **रिपॉजिटरी रूट में** (उसी डायरेक्टरी में जहां आपका `models/` फोल्डर है):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# चैट टेम्पलेट निकालने के लिए एक न्यूनतम बातचीत बनाएं
messages = [
    {"role": "system", "content": "{Content}"},
    {"role": "user", "content": "{Content}"},
]

prompt_template = tokenizer.apply_chat_template(
    messages,
    tokenize=False,
    add_generation_prompt=True,
    enable_thinking=False,
)

# inference_model.json संरचना बनाएं
inference_model = {
    "Name": "qwen3-0.6b",
    "PromptTemplate": {
        "assistant": "{Content}",
        "prompt": prompt_template,
    },
}

output_path = f"{MODEL_PATH}/inference_model.json"
with open(output_path, "w", encoding="utf-8") as f:
    json.dump(inference_model, f, indent=2, ensure_ascii=False)

print(f"Chat template written to {output_path}")
print(json.dumps(inference_model, indent=2))
```

रिपॉजिटरी रूट से स्क्रिप्ट चलाएं:

```bash
python generate_chat_template.py
```

> **नोट:** `transformers` पैकेज पहले से ही `onnxruntime-genai` की डिपेंडेंसी के रूप में इंस्टॉल हो चुका है। यदि आपको `ImportError` दिखाई दे, तो पहले `pip install transformers` चलाएं।

यह स्क्रिप्ट `models/qwen3` डायरेक्टरी के अंदर `inference_model.json` फाइल बनाता है। यह फाइल Foundry Local को बताती है कि Qwen3 के लिए उपयोगकर्ता इनपुट को सही विशेष टोकन में कैसे लपेटना है।

> **महत्वपूर्ण:** `inference_model.json` में `"Name"` फ़ील्ड (इस स्क्रिप्ट में `qwen3-0.6b` पर सेट) वह **मॉडल एलियास** है जिसे आप सभी भविष्य के कमांड और API कॉल में उपयोग करेंगे। यदि आप इस नाम को बदलते हैं, तो अभ्यास 6–10 में मॉडल नाम को भी अपडेट करें।

#### चरण 3: कॉन्फ़िगरेशन सत्यापित करें

`models/qwen3/inference_model.json` खोलें और पुष्टि करें कि इसमें एक `Name` फील्ड और एक `PromptTemplate` ऑब्जेक्ट है जिसमें `assistant` और `prompt` कीज़ हैं। प्रॉम्प्ट टेम्पलेट में विशेष टोकन्स जैसे `<|im_start|>` और `<|im_end|>` शामिल होने चाहिए (सटीक टोकन्स मॉडल के चैट टेम्पलेट पर निर्भर करते हैं)।

> **मैन्युअल विकल्प:** यदि आप स्क्रिप्ट चलाना नहीं चाहते, तो आप फाइल मैन्युअली भी बना सकते हैं। मुख्य आवश्यकता यह है कि `prompt` फ़ील्ड में मॉडल का पूरा चैट टेम्पलेट `{Content}` के साथ उपयोगकर्ता संदेश के लिए प्लेसहोल्डर के रूप में शामिल हो।

---

### अभ्यास 5: मॉडल डायरेक्टरी संरचना सत्यापित करें

मодель बिल्डर आपके द्वारा निर्दिष्ट आउटपुट डायरेक्टरी में सभी कम्पाइल की गई फाइलें सीधे रखता है। सत्यापित करें कि अंतिम संरचना सही दिखती है:

```bash
ls models/qwen3
```

डायरेक्टरी में निम्नलिखित फाइलें होनी चाहिए:

```
models/
  qwen3/
    model.onnx
    model.onnx.data
    tokenizer.json
    tokenizer_config.json
    genai_config.json
    chat_template.jinja
    inference_model.json      (created in Exercise 4)
    vocab.json
    merges.txt
    special_tokens_map.json
    added_tokens.json
```

> **नोट:** कुछ अन्य संकलन उपकरणों के विपरीत, मॉडल बिल्डर नेस्टेड सबडायरेक्टरीज़ नहीं बनाता। सभी फाइलें सीधे आउटपुट फोल्डर में रहती हैं, जो कि बिल्कुल वही है जिसकी Foundry Local उम्मीद करता है।

---

### अभ्यास 6: मॉडल को Foundry Local कैश में जोड़ें

Foundry Local को बताएं कि आपकी कम्पाइल की गई मॉडल कहां मिलेगी, डायरेक्टरी को इसकी कैश में जोड़कर:

```bash
foundry cache cd models/qwen3
```

सत्यापित करें कि मॉडल कैश में दिखाई दे रही है:

```bash
foundry cache ls
```

आप अपनी कस्टम मॉडल को किसी भी पहले से कैश की गई मॉडल (जैसे `phi-3.5-mini` या `phi-4-mini`) के साथ सूचीबद्ध देखेंगे।

---

### अभ्यास 7: CLI के साथ कस्टम मॉडल चलाएं

अपने नव-निर्मित मॉडल के साथ इंटरैक्टिव चैट सेशन शुरू करें (the `qwen3-0.6b` उपनाम `inference_model.json` में आपके द्वारा सेट किए गए `Name` फ़ील्ड से आता है):

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` फ्लैग अतिरिक्त डायग्नोस्टिक जानकारी दिखाता है, जो पहली बार कस्टम मॉडल का परीक्षण करते समय मददगार होती है। यदि मॉडल सफलतापूर्वक लोड होता है तो आपको एक इंटरैक्टिव प्रॉम्प्ट दिखेगा। कुछ संदेश आज़माएं:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

सेशन समाप्त करने के लिए `exit` टाइप करें या `Ctrl+C` दबाएं।

> **समस्या निवारण:** यदि मॉडल लोड करने में विफल होता है, तो निम्नलिखित जांचें:
> - `genai_config.json` फ़ाइल मॉडल बिल्डर द्वारा बनाई गई है।
> - `inference_model.json` फ़ाइल मौजूद है और वैध JSON है।
> - ONNX मॉडल फाइलें सही डायरेक्टरी में हैं।
> - आपके पास पर्याप्त उपलब्ध RAM है (Qwen3-0.6B int4 के लिए लगभग 1 GB की आवश्यकता है)।
> - Qwen3 एक reasoning मॉडल है जो `<think>` टैग्स उत्पन्न करता है। यदि आप प्रतिक्रिया के साथ `<think>...</think>` देख रहे हैं, तो यह सामान्य व्यवहार है। `inference_model.json` में प्रॉम्प्ट टेम्पलेट को इस सोच आउटपुट को दबाने के लिए समायोजित किया जा सकता है।

---

### अभ्यास 8: REST API के माध्यम से कस्टम मॉडल से प्रश्न पूछें

यदि आपने अभ्यास 7 में इंटरैक्टिव सेशन छोड़ दिया है, तो मॉडल अब लोड नहीं हो सकता। पहले Foundry Local सेवा शुरू करें और मॉडल लोड करें:

```bash
foundry service start
foundry model load qwen3-0.6b
```

जांचें कि सेवा किस पोर्ट पर चल रही है:

```bash
foundry service status
```

फिर एक अनुरोध भेजें (यदि आपका पोर्ट अलग है तो `5273` को अपने वास्तविक पोर्ट से बदलें):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows नोट:** ऊपर दिया गया `curl` कमांड bash सिंटैक्स का उपयोग करता है। Windows पर, इसके बजाय PowerShell `Invoke-RestMethod` cmdlet का उपयोग करें।

**PowerShell:**

```powershell
$body = @{
    model = "qwen3-0.6b"
    messages = @(
        @{ role = "user"; content = "What are three interesting facts about honeybees?" }
    )
    temperature = 0.7
    max_tokens = 200
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5273/v1/chat/completions" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### अभ्यास 9: OpenAI SDK के साथ कस्टम मॉडल का उपयोग करें

आप बिल्कुल उसी OpenAI SDK कोड का उपयोग करके अपने कस्टम मॉडल से कनेक्ट कर सकते हैं जैसा कि आप बिल्ट-इन मॉडल्स के लिए करते हैं (देखें [भाग 3](part3-sdk-and-apis.md))। केवल फर्क मॉडल का नाम है।

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local API कुंजियों को सत्यापित नहीं करता है
)

response = client.chat.completions.create(
    model="qwen3-0.6b",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
)

print(response.choices[0].message.content)
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // फाउंड्री लोकल API चाबियों को सत्यापित नहीं करता है
});

const response = await client.chat.completions.create({
  model: "qwen3-0.6b",
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
});

console.log(response.choices[0].message.content);
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using OpenAI;
using OpenAI.Chat;

var client = new ChatClient(
    model: "qwen3-0.6b",
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:5273/v1"),
    });

var response = await client.CompleteChatAsync(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

Console.WriteLine(response.Value.Content[0].Text);
```

</details>

> **मुख्य बात:** चूंकि Foundry Local एक OpenAI-संगत API प्रदान करता है, इसलिए बिल्ट-इन मॉडल्स के साथ काम करने वाला कोई भी कोड आपके कस्टम मॉडल्स के साथ भी काम करता है। आपको केवल `model` पैरामीटर बदलना होता है।

---

### अभ्यास 10: Foundry Local SDK के साथ कस्टम मॉडल का परीक्षण करें

पहले के लैब में आपने Foundry Local SDK का उपयोग सेवा शुरू करने, एंडपॉइंट खोजने और मॉडलों का प्रबंधन करने के लिए किया था। आप अपने कस्टम-कम्पाइल किए गए मॉडल के साथ बिल्कुल समान पैटर्न का अनुसरण कर सकते हैं। SDK सेवा स्टार्टअप और एंडपॉइंट खोज को संभालता है, इसलिए आपका कोड `localhost:5273` को हार्ड-कोड करने की आवश्यकता नहीं है।

> **नोट:** इन उदाहरणों को चलाने से पहले सुनिश्चित करें कि Foundry Local SDK इंस्टॉल किया गया है:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` और `OpenAI` NuGet पैकेज जोड़ें
>
> प्रत्येक स्क्रिप्ट फाइल **रिपॉजिटरी रूट** में सेव करें (उसी डायरेक्टरी जहाँ आपका `models/` फ़ोल्डर है)।

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# चरण 1: फाउंड्री लोकल सेवा शुरू करें और कस्टम मॉडल लोड करें
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# चरण 2: कस्टम मॉडल के लिए कैश जांचें
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# चरण 3: मॉडल को मेमोरी में लोड करें
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# चरण 4: SDK-द्वारा खोजे गए एंडपॉइंट का उपयोग करके एक OpenAI क्लाइंट बनाएँ
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# चरण 5: स्ट्रीमिंग चैट पूर्णता अनुरोध भेजें
print("\n--- Model Response ---")
stream = client.chat.completions.create(
    model=model_alias,
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "Explain how bees make honey, in three sentences."},
    ],
    temperature=0.7,
    max_tokens=200,
    stream=True,
)

for chunk in stream:
    if chunk.choices[0].delta.content is not None:
        print(chunk.choices[0].delta.content, end="", flush=True)
print()
```

इसे चलाएं:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>JavaScript</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// चरण 1: फाउंड्री लोकल सेवा शुरू करें
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// चरण 2: कैटलॉग से कस्टम मॉडल प्राप्त करें
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// चरण 3: मॉडल को मेमोरी में लोड करें
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// चरण 4: SDK द्वारा खोजे गए एंडपॉइंट का उपयोग करके एक OpenAI क्लाइंट बनाएं
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// चरण 5: एक स्ट्रीमिंग चैट कंप्लीशन अनुरोध भेजें
console.log("\n--- Model Response ---");
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [
    { role: "system", content: "You are a helpful assistant." },
    { role: "user", content: "Explain how bees make honey, in three sentences." },
  ],
  temperature: 0.7,
  max_tokens: 200,
  stream: true,
});

for await (const chunk of stream) {
  if (chunk.choices[0]?.delta?.content) {
    process.stdout.write(chunk.choices[0].delta.content);
  }
}
console.log();
```

इसे चलाएं:

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>C#</b></summary>

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

var modelAlias = "qwen3-0.6b";

// Step 1: Start the Foundry Local service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "CustomModelDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Step 2: Get the custom model from the catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Step 3: Download if needed and load the model into memory
Console.WriteLine($"Loading model: {modelAlias}...");
var isCached = await model.IsCachedAsync(default);
if (!isCached)
    await model.DownloadAsync(null, default);
await model.LoadAsync(default);
Console.WriteLine($"Loaded model: {model.Id}");

// Step 4: Create an OpenAI client
var key = new ApiKeyCredential("foundry-local");
var client = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = new Uri(manager.Urls.First()),
});

var chatClient = client.GetChatClient(model.Id);

// Step 5: Stream a chat completion response
Console.WriteLine("\n--- Model Response ---");
var completionUpdates = chatClient.CompleteChatStreaming(
    new ChatMessage[]
    {
        new SystemChatMessage("You are a helpful assistant."),
        new UserChatMessage("Explain how bees make honey, in three sentences."),
    },
    new ChatCompletionOptions
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 200,
    });

foreach (var update in completionUpdates)
{
    if (update.ContentUpdate.Count > 0)
    {
        Console.Write(update.ContentUpdate[0].Text);
    }
}
Console.WriteLine();
```

</details>

> **मुख्य बात:** Foundry Local SDK एंडपॉइंट को डायनामिकली खोजता है, इसलिए आप कभी भी पोर्ट नंबर हार्ड-कोड नहीं करते। यह प्रोडक्शन एप्लिकेशन के लिए अनुशंसित तरीका है। आपका कस्टम-कम्पाइल किया गया मॉडल SDK के माध्यम से बिल्ट-इन कैटलॉग मॉडलों की तरह ही काम करता है।

---

## कंपाइल करने के लिए मॉडल चुनना

Qwen3-0.6B इस लैब में संदर्भ उदाहरण के रूप में उपयोग किया गया है क्योंकि यह छोटा, तेज़ कम्पाइल होता है, और Apache 2.0 लाइसेंस के तहत मुफ्त उपलब्ध है। हालाँकि, आप कई अन्य मॉडल्स भी कॉम्पाइल कर सकते हैं। यहाँ कुछ सुझाव हैं:

| मॉडल | Hugging Face ID | पैरामीटर | लाइसेंस | नोट्स |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | बहुत छोटा, तेज़ कम्पाइल, परीक्षण के लिए अच्छा |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | बेहतर गुणवत्ता, फिर भी तेज़ कम्पाइल |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | मजबूत गुणवत्ता, अधिक RAM की आवश्यकता |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face पर लाइसेंस स्वीकार करना आवश्यक |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | उच्च गुणवत्ता, बड़ा डाउनलोड और लंबा कम्पाइल समय |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | पहले से ही Foundry Local कैटलॉग में (तुलना के लिए उपयोगी) |

> **लाइसेंस रिमाइंडर:** उपयोग करने से पहले हमेशा मॉडल का लाइसेंस Hugging Face पर जांचें। कुछ मॉडल (जैसे Llama) के लिए आपको लाइसेंस समझौते को स्वीकार करना होगा और `huggingface-cli login` के साथ प्रमाणीकरण करना होगा।

---

## अवधारणाएँ: कस्टम मॉडल कब उपयोग करें

| परिदृश्य | क्यों खुद कॉम्पाइल करें? |
|----------|--------------------------|
| **जो मॉडल आपको चाहिए वह कैटलॉग में नहीं है** | Foundry Local कैटलॉग क्यूरेटेड है। यदि आपकी जरूरी मॉडल सूची में नहीं है तो खुद कॉम्पाइल करें। |
| **फाइन-ट्यून मॉडल्स** | यदि आपने डोमेन-विशिष्ट डेटा पर मॉडल को फाइन-ट्यून किया है, तो आपको अपने वज़न खुद कॉम्पाइल करने होंगे। |
| **विशिष्ट क्वांटाइज़ेशन आवश्यकताएँ** | आप कोई ऐसी प्रिसिजन या क्वांटाइज़ेशन रणनीति चाहते हैं जो कैटलॉग डिफ़ॉल्ट से भिन्न हो। |
| **नए मॉडल रिलीज** | जब Hugging Face पर नया मॉडल जारी होता है, तो वह अभी Foundry Local कैटलॉग में नहीं हो सकता। खुद कॉम्पाइल करने से तुरंत पहुंच मिलती है। |
| **अनुसंधान और प्रयोग** | उत्पादन विकल्प चुनने से पहले स्थानीय रूप से विभिन्न मॉडल आर्किटेक्चर, आकार, या विन्यास आज़माना। |

---

## सारांश

इस लैब में आपने सीखा कि कैसे:

| चरण | आपने क्या किया |
|------|----------------|
| 1 | ONNX Runtime GenAI मॉडल बिल्डर इंस्टॉल किया |
| 2 | Hugging Face से `Qwen/Qwen3-0.6B` को ऑप्टिमाइज़्ड ONNX मॉडल में कम्पाइल किया |
| 3 | एक `inference_model.json` चैट-टेम्पलेट कॉन्फ़िगरेशन फाइल बनाया |
| 4 | कम्पाइल की गई मॉडल को Foundry Local कैश में जोड़ा |
| 5 | CLI के माध्यम से कस्टम मॉडल के साथ इंटरैक्टिव चैट चलाया |
| 6 | OpenAI-संगत REST API के जरिये मॉडल से प्रश्न पूछा |
| 7 | Python, JavaScript, और C# से OpenAI SDK का उपयोग कर कनेक्ट किया |
| 8 | Foundry Local SDK के साथ कस्टम मॉडल का एंड-टू-एंड परीक्षण किया |

मुख्य बात यह है कि **किसी भी ट्रांसफॉर्मर-आधारित मॉडल को ONNX प्रारूप में कम्पाइल करने के बाद Foundry Local के जरिए चलाया जा सकता है**। OpenAI-संगत API का अर्थ है कि आपका मौजूदा एप्लिकेशन कोड बिना बदलाव के ही काम करता है; आपको केवल मॉडल नाम बदलना होता है।

---

## मुख्य निष्कर्ष

| अवधारणा | विवरण |
|----------|--------|
| ONNX Runtime GenAI मॉडल बिल्डर | Hugging Face मॉडलों को ONNX प्रारूप में क्वांटाइज़ेशन के साथ एकल कमांड में परिवर्तित करता है |
| ONNX प्रारूप | Foundry Local को ONNX Runtime GenAI कॉन्फ़िगरेशन वाली ONNX मॉडल्स की आवश्यकता होती है |
| चैट टेम्पलेट्स | `inference_model.json` फ़ाइल Foundry Local को बताती है कि किसी मॉडल के लिए प्रॉम्प्ट कैसे फ़ॉर्मेट करें |
| हार्डवेयर लक्ष्य | CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), या WebGPU के लिए कम्पाइल करें, आपके हार्डवेयर के अनुसार |
| क्वांटाइज़ेशन | कम प्रिसिजन (int4) आकार कम करता है और गति बढ़ाता है कुछ शुद्धता की कीमत पर; fp16 GPU पर उच्च गुणवत्ता बनाए रखता है |
| API संगतता | कस्टम मॉडल बिल्ट-इन मॉडल्स के समान OpenAI-संगत API का उपयोग करते हैं |
| Foundry Local SDK | SDK सेवा स्टार्टअप, एंडपॉइंट खोज, और मॉडल लोडिंग को स्वचालित रूप से संभालता है, दोनों कैटलॉग और कस्टम मॉडलों के लिए |

---

## आगे पढ़ें

| संसाधन | लिंक |
|---------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local कस्टम मॉडल गाइड | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 मॉडल परिवार | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive प्रलेखन | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## अगले कदम

स्थानीय मॉडलों को बाहरी फ़ंक्शंस कॉल करने में सक्षम बनाने के लिए [भाग 11: टूल कॉलिंग विथ लोकल मॉडल्स](part11-tool-calling.md) पर जारी रखें।

[← भाग 9: व्हिस्पर वॉइस ट्रांस्क्रिप्शन](part9-whisper-voice-transcription.md) | [भाग 11: टूल कॉलिंग →](part11-tool-calling.md)