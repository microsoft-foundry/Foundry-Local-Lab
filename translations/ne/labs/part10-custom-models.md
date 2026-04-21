![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग १०: Foundry Local सँग कस्टम वा Hugging Face मोडेलहरू प्रयोग गर्ने

> **लक्ष्य:** Hugging Face मोडेललाई Foundry Local ले आवश्यक पर्ने अनुकूलित ONNX ढाँचामा कम्पाइल गर्ने, यसलाई च्याट टेम्प्लेटसँग कन्फिगर गर्ने, लोकल क्यासमा थप्ने, र CLI, REST API, र OpenAI SDK प्रयोग गरेर यसमा इन्फरेन्स चलाउने।

## अवलोकन

Foundry Local पूर्व-कम्पाइल गरिएको मोडेलहरूको एक छनोट सूची सहित आउँछ, तर तपाईं त्यो सूचीमा मात्र सीमित हुनुहुन्न। [Hugging Face](https://huggingface.co/) मा उपलब्ध कुनै पनि ट्रान्सफर्मर-आधारित भाषा मोडेल (वा स्थानीय रूपमा PyTorch / Safetensors ढाँचामा संग्रहित) लाई अनुकूलित ONNX मोडेलमा कम्पाइल गरी Foundry Local मार्फत सेवा दिन सकिन्छ।

कम्पाइलेशन पाइपलाइनले **ONNX Runtime GenAI Model Builder** प्रयोग गर्छ, जुन `onnxruntime-genai` प्याकेजसँग समावेश गरिएको कमाण्ड-लाइन उपकरण हो। मोडेल बिल्डरले मुख्य काम गर्छ: स्रोत तौलहरू डाउनलोड गर्ने, तिनीहरूलाई ONNX ढाँचामा रूपान्तरण गर्ने, क्वान्टाइजेसन (int4, fp16, bf16) लागू गर्ने, र Foundry Local ले अपेक्षा गर्ने कन्फिगरेसन फाइलहरू (च्याट टेम्प्लेट र टोकनाइजर सहित) उत्पादन गर्ने।

यस प्रयोगशालामा तपाईंले Hugging Face बाट **Qwen/Qwen3-0.6B** कम्पाइल गर्ने, यसलाई Foundry Local मा दर्ता गर्ने, र पूर्ण रूपमा आफ्नो उपकरणमा यससँग च्याट गर्ने।

---

## सिकाइका उद्देश्यहरू

यस प्रयोगशालाको अन्त्यसम्ममा तपाईं सक्षम हुनुहुनेछ:

- कस्टम मोडेल कम्पाइल किन उपयोगी छ र कहिले आवश्यक पर्छ भनेर व्याख्या गर्न
- ONNX Runtime GenAI मोडेल बिल्डर स्थापना गर्न
- एक कमाण्डमार्फत Hugging Face मोडेललाई अनुकूलित ONNX ढाँचामा कम्पाइल गर्न
- मुख्य कम्पाइलेशन प्यारामिटरहरू (एक्जिक्युसन प्रोभाइडर, प्रिसिजन) बुझ्न
- `inference_model.json` च्याट-टेम्प्लेट कन्फिगरेसन फाइल सिर्जना गर्न
- कम्पाइल गरिएको मोडेल Foundry Local क्यासमा थप्न
- CLI, REST API, र OpenAI SDK प्रयोग गरी कस्टम मोडेलमा इन्फरेन्स चलाउन

---

## पूर्वशर्तहरू

| आवश्यकताहरू | विवरण |
|-------------|---------|
| **Foundry Local CLI** | स्थापना गरिएको र तपाईँको `PATH` मा ([भाग १](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI मोडेल बिल्डरलाई आवश्यक |
| **pip** | Python प्याकेज मेनेजर |
| **डिस्क स्पेस** | स्रोत र कम्पाइल गरिएको मोडेल फाइलहरूको लागि कम्तीमा ५ GB खाली |
| **Hugging Face खाता** | केही मोडेलहरू डाउनलोड गर्नुअघि लाइसेन्स स्वीकार्न आवश्यक पर्छ। Qwen3-0.6B ले Apache 2.0 लाइसेन्स प्रयोग गर्छ र स्वतन्त्र रूपमा उपलब्ध छ। |

---

## वातावरण तयार पार्ने

मोडेल कम्पाइल गर्नका लागि केही ठूलो Python प्याकेजहरू (PyTorch, ONNX Runtime GenAI, Transformers) आवश्यक पर्छन्। यीले तपाईँको सिस्टम Python वा अन्य परियोजनाहरूसँग समस्या नपरोस् भनेर समर्पित भर्चुअल वातावरण बनाउनुहोस्।

```bash
# रिपोजिटरीको मूलबाट
python -m venv .venv
```

वातावरण सक्रिय पार्नुहोस्:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

पिप अपग्रेड गर्नुहोस् ताकि निर्भरता समाधान समस्याहरू नआओस्:

```bash
python -m pip install --upgrade pip
```

> **सुझाव:** यदि तपाईँसँग पहिलाका प्रयोगशालाहरूबाट `.venv` छ भने पुन: प्रयोग गर्न सक्नुहुन्छ। मात्र यसलाई सक्रिय पार्न निश्चित हुनुहोस्।

---

## अवधारणा: कम्पाइलेशन पाइपलाइन

Foundry Local लाई ONNX ढाँचामा ONNX Runtime GenAI कन्फिगरेसनसहित मोडेलहरू आवश्यक पर्छ। धेरै खुला स्रोत मोडेलहरू Hugging Face मा PyTorch वा Safetensors तौलका रूपमा वितरण हुन्छन्, त्यसैले रूपान्तरण चरण चाहिन्छ।

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### मोडेल बिल्डर के गर्छ?

१. Hugging Face बाट स्रोत मोडेल डाउनलोड गर्छ (वा स्थानीय पथबाट पढ्छ)।
२. PyTorch / Safetensors तौलहरूलाई ONNX ढाँचामा रूपान्तरण गर्छ।
३. मोडेललाई कम प्रिसिजनमा (जस्तै int4) क्वान्टाइज गरेर मेमोरी प्रयोग घटाउँछ र थ्रुपुट सुधार गर्छ।
४. ONNX Runtime GenAI कन्फिगरेसन (`genai_config.json`), च्याट टेम्प्लेट (`chat_template.jinja`), र सबै टोकनाइजर फाइलहरू उत्पादन गर्छ ताकि Foundry Local मोडेल लोड र सेवा गर्नसकून्।

### ONNX Runtime GenAI Model Builder र Microsoft Olive

मोडेल अनुकूलनका लागि वैकल्पिक उपकरणको रूपमा **Microsoft Olive** को उल्लेख भेट्न सकिन्छ। दुबै उपकरणले ONNX मोडेल उत्पादन गर्न सक्छन्, तर तिनीहरूको उद्देश्य र ट्रेड-अफ फरक छन्:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **प्याकेज** | `onnxruntime-genai` | `olive-ai` |
| **प्राथमिक उद्देश्य** | ONNX Runtime GenAI इन्फरेन्सका लागि जेनेरेटिभ AI मोडेल रूपान्तरण र क्वान्टाइजेसन | थुप्रै ब्याकएन्ड र हार्डवेयर लक्ष्यहरू समर्थन गर्ने मोडेल अनुकूलन फ्रेमवर्क |
| **प्रयोग सहजता** | एकल कमाण्ड — एकचरण रूपान्तरण + क्वान्टाइजेसन | वर्कफ्लो आधारित — YAML/JSON सहित कन्फिगरेसन लागू गर्ने प्रवाहहरू |
| **आउटपुट ढाँचा** | ONNX Runtime GenAI ढाँचा (Foundry Local का लागि तयार) | जनरल ONNX, ONNX Runtime GenAI, वा वर्कफ्लो अनुसार अन्य ढाँचाहरू |
| **हार्डवेयर लक्ष्यहरू** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, आदि |
| **क्वान्टाइजेसन विकल्पहरू** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, प्लस ग्राफ अनुकूलन, तह-स्तरीय ट्यूनिङ |
| **मोडेल दायरा** | जेनेरेटिभ AI मोडेलहरू (LLMs, SLMs) | कुनै पनि ONNX-रूपान्तरणयोग्य मोडेल (भिजन, NLP, ऑडियो, मल्टिमोडल) |
| **उपयुक्त** | स्थानीय इन्फरेन्सका लागि छिटो एक-मोडेल कम्पाइल | उत्पादन पाइपलाइनहरू जसलाई सूक्ष्म अनुकूलन चाहिन्छ |
| **निर्भरता** | मध्यम (PyTorch, Transformers, ONNX Runtime) | ठूलो (Olive फ्रेमवर्क थप्छ, वैकल्पिक वर्कफ्लो अनुसार) |
| **Foundry Local समाकलन** | सिधा — आउटपुट तुरुन्त उपयुक्त हुन्छ | `--use_ort_genai` झण्डा र अतिरिक्त कन्फिगर आवश्यकता |

> **किन यो प्रयोगशालाले मोडेल बिल्डर प्रयोग गर्छ:** एक Hugging Face मोडेल कम्पाइल गरी Foundry Local मा दर्ता गर्ने कामका लागि मोडेल बिल्डर सबैभन्दा सरल र भरपर्दो बाटो हो। यसले एकै कमाण्डमा Foundry Local ले चाहेको ठ्याक्कै आउटपुट ढाँचा उत्पादन गर्छ। पछि यदि तपाईंलाई उन्नत अनुकूलन विकल्पहरू जस्तै सटीकता-जागरुक क्वान्टाइजेसन, ग्राफ सर्जरी, वा बहु-पास ट्यूनिङ आवश्यक परे Microsoft Olive पत्ता लगाउन सकिन्छ। थप जानकारीका लागि [Microsoft Olive कागजात](https://microsoft.github.io/Olive/) हेर्नुहोस्।

---

## प्रयोगशाला अभ्यासहरू

### अभ्यास १: ONNX Runtime GenAI Model Builder स्थापना गर्नुहोस्

ONNX Runtime GenAI प्याकेज स्थापना गर्नुहोस्, जसले मोडेल बिल्डर उपकरण समावेश गर्दछ:

```bash
pip install onnxruntime-genai
```

सेटअप सफल छ कि छैन परीक्षण गर्न मोडेल बिल्डर उपलब्ध छ कि छैन जाँच्नुहोस्:

```bash
python -m onnxruntime_genai.models.builder --help
```

तपाईंले `-m` (मोडेल नाम), `-o` (आउटपुट पथ), `-p` (प्रिसिजन), र `-e` (एक्जिक्युसन प्रोभाइडर) जस्ता प्यारामिटरहरू सूचीबद्ध हेल्प आउटपुट देख्नु पर्नेछ।

> **नोट:** मोडेल बिल्डरले PyTorch, Transformers, र अन्य प्याकेजहरूमा निर्भर गर्दछ। स्थापना केही मिनेट लिन सक्छ।

---

### अभ्यास २: CPU को लागि Qwen3-0.6B कम्पाइल गर्नुहोस्

तलको कमाण्ड चलाएर Hugging Face बाट Qwen3-0.6B मोडेल डाउनलोड गरी CPU इन्फरेन्सका लागि int4 क्वान्टाइजेसनसहित कम्पाइल गर्नुहोस्:

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

#### प्रत्येक प्यारामिटरको कार्य

| प्यारामिटर | उद्देश्य | प्रयोग गरिएको मान |
|-----------|---------|------------------|
| `-m` | Hugging Face मोडेल ID वा स्थानीय डाइरेक्टरी पथ | `Qwen/Qwen3-0.6B` |
| `-o` | कम्पाइल गरिएको ONNX मोडेल सुरक्षित गर्ने डाइरेक्टरी | `models/qwen3` |
| `-p` | कम्पाइल गर्दा लागू गरिएको क्वान्टाइजेसन प्रिसिजन | `int4` |
| `-e` | ONNX Runtime एक्जिक्युसन प्रोभाइडर (लक्ष्य हार्डवेयर) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face प्रमाणिकरण छोड्ने (सार्वजनिक मोडेलका लागि ठीक) | `hf_token=false` |

> **कति समय लाग्छ?** कम्पाइल समय तपाईंको हार्डवेयर र मोडेल आकारमा निर्भर हुन्छ। Qwen3-0.6B मा int4 क्वान्टाइजेसनसहित आधुनिक CPU मा करिब ५ देखि १५ मिनेट। ठूला मोडेलहरू अधिक समय लाग्छ।

एकपटक कमाण्ड पूरा भएपछि तपाईंले `models/qwen3` नामक डाइरेक्टरी देख्नु पर्नेछ जसमा कम्पाइल गरिएको मोडेल फाइलहरू छन्। आउटपुट प्रमाणित गर्नुहोस्:

```bash
ls models/qwen3
```

तपाईंले यस्ता फाइलहरू देख्नु पर्नेछ:
- `model.onnx` र `model.onnx.data` — कम्पाइल गरिएको मोडेल तौलहरू
- `genai_config.json` — ONNX Runtime GenAI कन्फिगरेसन
- `chat_template.jinja` — मोडेलको च्याट टेम्प्लेट (स्वतः उत्पन्न)
- `tokenizer.json`, `tokenizer_config.json` — टोकनाइजर फाइलहरू
- अन्य शब्दावली र कन्फिगरेसन फाइलहरू

---

### अभ्यास ३: GPU को लागि कम्पाइल गर्नुहोस् (ऐच्छिक)

यदि तपाईं सँग CUDA समर्थन सहित NVIDIA GPU छ भने छिटो इन्फरेन्सका लागि GPU-अनुकूलित संस्करण कम्पाइल गर्न सक्नुहुन्छ:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **नोट:** GPU कम्पाइलेशनको लागि `onnxruntime-gpu` र CUDA को काम गर्ने स्थापना आवश्यक छ। यदि यी छैनन् भने मोडेल बिल्डरले त्रुटि देखाउनेछ। तपाईं यो अभ्यास छोडेर CPU संस्करणसहित जारी राख्न सक्नुहुन्छ।

#### हार्डवेयर-विशिष्ट कम्पाइल सन्दर्भ

| लक्ष्य | एक्जिक्युसन प्रोभाइडर (`-e`) | सिफारिस गरिएको प्रिसिजन (`-p`) |
|--------|-----------------------------|--------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` वा `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` वा `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### प्रिसिजन व्यापार

| प्रिसिजन | आकार | गति | गुणस्तर |
|-----------|-------|-----|---------|
| `fp32` | सबैभन्दा ठूलो | सबैभन्दा ढिलो | सबैभन्दा उच्च सटीकता |
| `fp16` | ठूलो | छिटो (GPU) | धेरै राम्रो सटीकता |
| `int8` | सानो | छिटो | थोरै सटीकता घाटा |
| `int4` | सबैभन्दा सानो | सबैभन्दा छिटो | मध्यम सटीकता घाटा |

धेरैजसो स्थानीय विकासका लागि CPU मा `int4` सबैभन्दा राम्रो सन्तुलन हो। उत्पादन-गुणस्तरको नतिजाका लागि CUDA GPU मा `fp16` सिफारिस गरिन्छ।

---

### अभ्यास ४: च्याट टेम्प्लेट कन्फिगरेसन सिर्जना गर्नुहोस्

मोडेल बिल्डरले स्वतः `chat_template.jinja` फाइल र आउटपुट डाइरेक्टरीमा `genai_config.json` फाइल बनाउँछ। तर Foundry Local लाई सही ढंगले प्रयोगकर्ताका सन्देशहरूलाई विशेष टोकनहरूमा र्याप गर्ने तरिका बुझ्न `inference_model.json` फाइल चाहिन्छ। यसले मोडेल नाम र प्रॉम्प्ट टेम्प्लेट परिभाषित गर्छ।

#### चरण १: कम्पाइल गरिएको आउटपुट निरीक्षण गर्नुहोस्

कम्पाइल गरिएको मोडेल डाइरेक्टरीको सामग्री सूचीबद्ध गर्नुहोस्:

```bash
ls models/qwen3
```

तपाईंले यस्ता फाइलहरू देख्नु पर्नेछ:
- `model.onnx` र `model.onnx.data` — कम्पाइल मोडेल तौलहरू
- `genai_config.json` — ONNX Runtime GenAI कन्फिगरेसन (स्वतः उत्पन्न)
- `chat_template.jinja` — मोडेलको च्याट टेम्प्लेट (स्वतः उत्पन्न)
- `tokenizer.json`, `tokenizer_config.json` — टोकनाइजर फाइलहरू
- विभिन्न अन्य कन्फिगरेसन र शब्दावली फाइलहरू

#### चरण २: inference_model.json फाइल निर्माण गर्नुहोस्

`inference_model.json` फाइलले Foundry Local लाई प्रॉम्प्टहरू कसरी ढाँचा गर्ने भनेर बताउँछ। यसका लागि `generate_chat_template.py` नामक Python स्क्रिप्ट **रिपोजिटरी रूटमा** तयार गर्नुहोस् (उही डाइरेक्टरी जहाँ तपाईंको `models/` फोल्डर छ):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# कुराकानी ढाँचा निकाल्नका लागि न्यूनतम कुराकानी बनाएँ
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

# inference_model.json संरचना बनाएँ
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

रिपोजिटरी रूटबाट स्क्रिप्ट चलाउनुहोस्:

```bash
python generate_chat_template.py
```

> **नोट:** `transformers` प्याकेज पहिले नै `onnxruntime-genai` को निर्भरता रूपमा स्थापना गरिएको थियो। यदि तपाईंलाई `ImportError` देखिन्छ भने, पहिले `pip install transformers` चलाउनुहोस्।

यो स्क्रिप्टले `models/qwen3` भित्र `inference_model.json` फाइल उत्पादन गर्दछ। यसले Foundry Local लाई बताउँछ कि Qwen3 का लागि प्रयोगकर्ताको इनपुटलाई सही विशेष टोकनमा कसरी र्याप गर्ने।

> **महत्त्वपूर्ण:** `inference_model.json` को `"Name"` फिल्ड (यस स्क्रिप्टमा `qwen3-0.6b` सेट गरिएको) तपाईंले सबै पछिल्ला कमाण्ड र API कलहरूमा प्रयोग गर्ने **मोडेल उपनाम** हो। यदि तपाईंले यो नाम परिवर्तन गर्नुभयो भने अभ्यास ६–१० मा मोडेल नाम अपडेट गर्नुहोस्।

#### चरण ३: कन्फिगरेसन प्रमाणित गर्नुहोस्

`models/qwen3/inference_model.json` खोल्नुहोस् र पक्का गर्नुहोस् कि यसमा `Name` फिल्ड र `assistant` र `prompt` कुञ्जीहरू सहित `PromptTemplate` वस्तु छ। प्रॉम्प्ट टेम्प्लेटमा `<|im_start|>` र `<|im_end|>` जस्ता विशेष टोकनहरू हुनु पर्छ (ठ्याक्कै टोकनहरू मोडेलको च्याट टेम्प्लेट अनुसार फरक पर्न सक्छ)।

> **म्यानुअल विकल्प:** यदि स्क्रिप्ट चलाउन चाहनुहुन्न भने, फाइल आफैं सिर्जना गर्न सक्नुहुन्छ। मुख्य आवश्यकता भनेको `prompt` फिल्डमा मोडेलको पूरै च्याट टेम्प्लेट `{Content}` स्टेटमेन्ट साथ प्रयोगकर्ताको सन्देशका लागि प्लेसहोल्डर हुनुपर्छ।

---

### अभ्यास ५: मोडेल डाइरेक्टरी संरचना प्रमाणित गर्नुहोस्
मोडेल बिल्डरले सबै कम्पाइल गरिएका फाइलहरू तपाईंले निर्दिष्ट गरेको आउटपुट डाइरेक्टरीमा सीधै राख्छ। अन्तिम संरचना सही देखिन्छ कि छैन भनेर जाँच गर्नुहोस्:

```bash
ls models/qwen3
```

डाइरेक्टरीमा निम्न फाइलहरू हुनु पर्नेछ:

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

> **नोट:** केही अन्य कम्पाइल उपकरणहरू जस्तै, मोडेल बिल्डरले नेस्टेड सबडाइरेक्टरीहरू सिर्जना गर्दैन। सबै फाइलहरू सिधै आउटपुट फोल्डरमा हुन्छन्, जुन Foundry Local ले अपेक्षा गरेको ठीक त्यही हो।

---

### अभ्यास ६: Foundry Local Cache मा मोडेल थप्नुहोस्

तपाईंले कम्पाइल गरेको मोडेल कहाँ छ भनेर Foundry Local लाई बताउनुहोस्, त्यसको क्यासमा डाइरेक्टरी थपेर:

```bash
foundry cache cd models/qwen3
```

क्यासमा मोडेल देखिन्छ कि भनेर जाँच्नुहोस्:

```bash
foundry cache ls
```

तपाईंको कस्टम मोडेल पहिले क्यास गरिएको मोडेलहरू (जस्तै `phi-3.5-mini` वा `phi-4-mini`) सँगै सूचीबद्ध भएको देख्नु पर्नेछ।

---

### अभ्यास ७: CLI सँग कस्टम मोडेल चलाउनुहोस्

भर्खर कम्पाइल गरिएको मोडेलसँग अन्तरक्रियात्मक चैट सत्र सुरु गर्नुहोस् (`qwen3-0.6b` उपनाम `inference_model.json` फाइलमा तपाईंले सेट गरेको `Name` फिल्डबाट आएको हो):

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` फ्ल्यागले थप डाइग्नोस्टिक जानकारी देखाउँछ, जुन पहिलो पटक कस्टम मोडेल परीक्षण गर्दा उपयोगी हुन्छ। मोडेल सफलतापूर्वक लोड भयो भने अन्तरक्रियात्मक प्रॉम्प्ट देखिनेछ। केही सन्देशहरू प्रयास गर्नुहोस्:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

सत्र अन्त्य गर्न `exit` टाइप गर्नुहोस् वा `Ctrl+C` थिच्नुहोस्।

> **समस्या समाधान:** यदि मोडेल लोड हुन असफल भयो भने, तलका कुराहरू जाँच्नुहोस्:
> - `genai_config.json` फाइल मोडेल बिल्डरले सिर्जना गरेको छ।
> - `inference_model.json` फाइल छ र मान्य JSON हो।
> - ONNX मोडेल फाइलहरू सही डाइरेक्टरीमा छन्।
> - तपाईंको RAM पर्याप्त छ (Qwen3-0.6B int4 करिब 1 GB चाहिन्छ)।
> - Qwen3 एक reasoning मोडेल हो जसले `<think>` ट्यागहरू उत्पादन गर्छ। यदि प्रतिक्रियामा `<think>...</think>` देखिन्छ भने यो सामान्य व्यवहार हो। `inference_model.json` मा रहेको प्रॉम्प्ट टेम्प्लेटलाई समायोजन गरेर सोच्ने आउटपुटलाई दबाउन सकिन्छ।

---

### अभ्यास ८: REST API मार्फत कस्टम मोडेल क्वेरी गर्नुहोस्

यदि तपाईंले अभ्यास ७ मा अन्तरक्रियात्मक सत्र बन्द गर्नुभयो भने, मोडेल अब लोड नहुन सक्छ। पहिले Foundry Local सेवा सुरु गरेर मोडेल लोड गर्नुहोस्:

```bash
foundry service start
foundry model load qwen3-0.6b
```

सेवा कुन पोर्टमा चलिरहेको छ भनेर जाँच्नुहोस्:

```bash
foundry service status
```

पछाडि अनुरोध पठाउनुहोस् (यदि फरक पोर्ट छ भने `5273` साट्नुहोस्):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows नोट:** माथिको `curl` आदेश bash सिन्ट्याक्स प्रयोग गर्छ। Windows मा, तलको PowerShell `Invoke-RestMethod` कमाण्ड प्रयोग गर्नुहोस्।

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

### अभ्यास ९: OpenAI SDK सँग कस्टम मोडेल प्रयोग गर्नुहोस्

तपाईंले बनाइएका बिल्ट-इन मोडेलहरूमा जस्तै OpenAI SDK कोड प्रयोग गरेर कस्टम मोडेलमा जडान गर्न सक्नुहुन्छ (हेर्नुहोस् [भाग ३](part3-sdk-and-apis.md))। भिन्नता केवल मोडेल नाम हो।

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local ले API कुञ्जीहरू प्रमाणित गर्दैन
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
  apiKey: "foundry-local", // फन्ड्री लोकल एपीआई कुञ्जीहरू प्रमाणित गर्दैन
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

> **मुख्य कुरा:** Foundry Local ले OpenAI अनुकुल API सार्वजनिक गरेकोले, बिल्ट-इन मोडेलहरूसँग काम गर्ने कुनै पनि कोड तपाईंका कस्टम मोडेलहरूसँग पनि काम गर्छ। तपाईंले मात्र `model` प्यारामिटर परिवर्तन गर्नुपर्ने हुन्छ।

---

### अभ्यास १०: Foundry Local SDK सँग कस्टम मोडेल परीक्षण गर्नुहोस्

पहिलाका प्रयोगशालाहरूमा तपाईंले Foundry Local SDK उपयोग गरेर सेवा सुरु गर्ने, अन्त बिन्दु पत्ता लगाउने, र मोडेलहरू स्वचालित रूपमा व्यवस्थापन गर्ने काम गर्नुभएको थियो। तपाईंले आफ्नै कस्टम कम्पाइल गरिएको मोडेलमा पनि ठीक त्यही विधि अनुसरण गर्न सक्नुहुन्छ। SDK सेवा सुरूवात र अन्त बिन्दु पत्ता लगाउने काम गर्छ, त्यसैले तपाईंले `localhost:5273` कडाइका साथ लेख्न आवश्यक छैन।

> **नोट:** यी उदाहरणहरू चलाउनु अघि Foundry Local SDK इन्स्टल गरिएको छ कि छैन जाँच गर्नुहोस्:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` र `OpenAI` NuGet प्याकेजहरू थप्नुहोस्
>
> प्रत्येक स्क्रिप्ट फाइल **रिपोजिटरी रुटमा** बचत गर्नुहोस् (तपाईंको `models/` फोल्डरसँगै रहेको डाइरेक्टरी)।

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# चरण 1: फाउन्ड्री लोकल सेवा सुरू गर्नुहोस् र कस्टम मोडेल लोड गर्नुहोस्
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# चरण 2: कस्टम मोडेलको लागि क्याच जाँच गर्नुहोस्
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# चरण 3: मोडेललाई मेमोरीमा लोड गर्नुहोस्
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# चरण 4: SDK द्वारा पत्ता लागेको एन्डपोइन्ट प्रयोग गरेर OpenAI क्लाइन्ट सिर्जना गर्नुहोस्
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# चरण 5: स्ट्रिमिङ च्याट पूरा गर्ने अनुरोध पठाउनुहोस्
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

चलाउनुहोस्:

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

// चरण 1: फाउन्ड्री स्थानीय सेवा सुरू गर्नुहोस्
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// चरण 2: क्याटलगबाट अनुकूलित मोडल प्राप्त गर्नुहोस्
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// चरण 3: मोडललाई मेमोरीमा लोड गर्नुहोस्
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// चरण 4: SDK-आविष्कृत अन्तबिन्दु प्रयोग गरेर OpenAI क्लाइन्ट सिर्जना गर्नुहोस्
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// चरण 5: स्ट्रिमिङ च्याट कम्प्लिशन अनुरोध पठाउनुहोस्
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

चलाउनुहोस्:

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

> **मुख्य कुरा:** Foundry Local SDK ले अन्त बिन्दु गतिशील रूपमा पत्ता लगाउँछ, त्यसैले तपाईंले कहिल्यै पनि पोर्ट नम्बर कडाइका साथ लेख्नुपर्दैन। यो उत्पादन अनुप्रयोगका लागि सिफारिस गरिएको तरिका हो। तपाईंको कस्टम कम्पाइल गरिएको मोडेल SDK मार्फत बिल्ट-इन क्याटालग मोडेलहरूसँग उस्तै प्रकारले काम गर्छ।

---

## कम्पाइल गर्न मोडेल छनोट

Qwen3-0.6B यस प्रयोगशालामा सन्दर्भ उदाहरणको रूपमा प्रयोग गरिएको छ किनकि यो सानो छ, छिटो कम्पाइल हुन्छ, र Apache 2.0 लाइसेन्स अन्तर्गत स्वतन्त्र रूपमा उपलब्ध छ। यद्यपि, तपाईंले धेरै अन्य मोडेलहरू पनि कम्पाइल गर्न सक्नुहुन्छ। यहाँ केही सुझावहरू छन्:

| मोडेल | Hugging Face ID | प्यारामिटरहरू | लाइसेन्स | टिप्पणीहरू |
|-------|-----------------|---------------|---------|------------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | धेरै सानो, छिटो कम्पाइल हुन्छ, परीक्षणका लागि राम्रो |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | राम्रो गुणस्तर, अझै छिटो कम्पाइल हुन्छ |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | उच्च गुणस्तर, बढी RAM चाहिन्छ |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face मा लाइसेन्स स्वीकार गर्नुपर्ने हुन्छ |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | उच्च गुणस्तर, ठूलो डाउनलोड र लामो कम्पाइल समय |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | पहिले नै Foundry Local क्याटालगमा छ (तुलनाका लागि उपयोगी) |

> **लाइसेन्स सम्झना:** कुनै पनि मोडेल प्रयोग गर्नु अघि Hugging Face मा मोडेलको लाइसेन्स जाँच्नुहोस्। केही मोडेलहरू (जस्तै Llama) को लागि लाइसेन्स सम्झौता स्वीकार गर्न र `huggingface-cli login` प्रयोग गरेर प्रमाणिकरण गर्न आवश्यक पर्छ।

---

## अवधारणाहरू: कहिलेदेखि कस्टम मोडेल प्रयोग गर्ने

| अवस्था | किन आफ्नो कम्पाइल गर्ने? |
|----------|--------------------------|
| **कुनै आवश्यक मोडेल क्याटालगमा छैन** | Foundry Local क्याटालग क्युरेट गरिएको हो। यदि तपाईंलाई चाहिएको मोडेल सूचीमा छैन भने, आफैं कम्पाइल गर्नुहोस्। |
| **फाइन्-ट्युन गरिएको मोडेलहरू** | तपाईंले डोमेन-विशिष्ट डाटामा मोडेल फाइन् ट्युन गर्नुभएको छ भने, तपाईंले आफ्नै वेट्स कम्पाइल गर्नुपर्ने हुन्छ। |
| **विशिष्ट क्वान्टाइजेशन आवश्यकताहरू** | तपाईंलाई क्याटालग डिफल्ट भन्दा फरक प्रिसिजन वा क्वान्टाइजेशन रणनीति चाहिन सक्छ। |
| **नयाँ मोडेल रिलिजहरू** | नयाँ मोडेल Hugging Face मा रिलिज हुने बेला Foundry Local मा नहुन सक्छ। आफैं कम्पाइल गर्दा तुरुन्त पहुँच पाउनुहुन्छ। |
| **अनुसन्धान र प्रयोग** | उत्पादन छनौट अघि विभिन्न मोडेल आर्किटेक्चर, साइज वा कन्फिगरेसनहरू स्थानीय रूपमा परीक्षण गर्न। |

---

## सारांश

यस प्रयोगशालामा तपाईंले सिक्नुभयो कि कसरी:

| चरण | तपाईंले के गर्नुभयो |
|------|-------------------|
| 1 | ONNX Runtime GenAI मोडेल बिल्डर इन्स्टल गर्नुभयो |
| 2 | `Qwen/Qwen3-0.6B` लाई Hugging Face बाट ONNX मोडेलमा रूपान्तरण गर्नुभयो |
| 3 | `inference_model.json` च्याट-टेम्प्लेट कन्फिग फाइल तयार गर्नुभयो |
| 4 | कम्पाइल गरिएको मोडेल Foundry Local क्यासमा थप्नुभयो |
| 5 | CLI मार्फत अन्तरक्रियात्मक च्याट सञ्चालन गर्नुभयो |
| 6 | OpenAI अनुकुल REST API बाट मोडेललाई क्वेरी गर्नुभयो |
| 7 | Python, JavaScript, र C# बाट OpenAI SDK प्रयोग गरेर कनेक्ट गर्नुभयो |
| 8 | Foundry Local SDK सँग कस्टम मोडेलको अन्त देखि अन्त परीक्षण गर्नुभयो |

मुख्य कुरा यो हो कि **कुनै पनि ट्रान्सफर्मर-आधारित मोडेललाई ONNX ढाँचामा कम्पाइल गरेपछि Foundry Local मार्फत चलाउन सकिन्छ**। OpenAI अनुकुल API ले तपाईंको वर्तमान एप्लिकेशन कोडमा कुनै परिवर्तन नगरी काम गर्छ; तपाईंले मात्र मोडेल नाम परिवर्तन गर्नुपर्छ।

---

## मुख्य बुँदाहरू

| अवधारणा | विवरण |
|---------|--------|
| ONNX Runtime GenAI मोडेल बिल्डर | Hugging Face मोडेलहरूलाई ONNX ढाँचामा क्वान्टाइजेसनसहित एकै कमाण्डमा रूपान्तरण गर्दछ |
| ONNX ढाँचा | Foundry Local ले ONNX Runtime GenAI कन्फिगरेसनसहित ONNX मोडेलहरू आवश्यक पर्दछ |
| च्याट टेम्प्लेटहरू | `inference_model.json` फाइलले Foundry Local लाई कसरी प्रॉम्प्टहरू तयार गर्ने भनेर बताउँछ |
| हार्डवेयर टार्गेटहरू | CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), वा WebGPU का लागि कम्पाइल गर्न सकिन्छ |
| क्वान्टाइजेसन | कम प्रिसिजन (int4) ले साइज घटाउँछ र गति बढाउँछ तर केहि सटीकता घट्छ; fp16 GPU मा उच्च गुणस्तर कायम राख्छ |
| API अनुकूलता | कस्टम मोडेलहरूले बिल्ट-इन मोडेलहरूको जस्तै OpenAI अनुकुल API प्रयोग गर्छन् |
| Foundry Local SDK | सेवा सुरु गर्ने, अन्त बिन्दु पत्ता लगाउने र मोडेल लोड गर्ने काम SDK ले आफैं गर्छ, क्याटालग र कस्टम मोडेल दुबैको लागि |

---

## थप अध्ययन सामग्री

| स्रोत | लिङ्क |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local कस्टम मोडेल मार्गदर्शन | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 मोडेल परिवार | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive प्रलेखन | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## आगामी चरणहरू

[भाग ११: लोकल मोडेलहरूसँग टुल कलिङ](part11-tool-calling.md) मा जानुहोस् र सिक्नुहोस् कसरी तपाईंका लोकल मोडेलहरूले बाह्य फङ्सनहरू बोलाउन सकून्।

[← भाग ९: व्हिस्पर भ्वाइस ट्रान्सक्रिप्सन](part9-whisper-voice-transcription.md) | [भाग ११: टुल कलिङ →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:  
यस दस्तावेजलाई AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) प्रयोग गरी अनुवाद गरिएको हो। हामी सटिकताका लागि प्रयत्न गर्छौं भने पनि, कृपया ध्यान दिनुहोस् कि स्वचालित अनुवादमा त्रुटि वा असङ्गतिहरू हुन सक्छन्। मूल दस्तावेज यसको स्वदेशी भाषामा अधिकारिक स्रोत मान्नुपर्छ। महत्वपूर्ण जानकारीको लागि, व्यावसायिक मानव अनुवाद सिफारिस गरिन्छ। यो अनुवादको प्रयोगबाट उत्पन्न कुनै पनि गलतफहमी वा गलत व्याख्याको लागि हामी जिम्मेवार छैनौं।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->