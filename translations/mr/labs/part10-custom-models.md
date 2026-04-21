![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# भाग 10: Foundry Local सह कस्टम किंवा Hugging Face मॉडेल्स वापरणे

> **उद्देश:** Foundry Local ला आवश्यक असलेला ऑप्टिमाइझ्ड ONNX फॉरमॅटमध्ये Hugging Face मॉडेल कंपाइल करणे, त्यास चाट टेम्पलेटसह कॉन्फिगर करणे, स्थानिक कॅशेमध्ये जोडणे, आणि CLI, REST API, आणि OpenAI SDK वापरून त्याच्या विरुद्ध इन्फरन्स चालवणे.

## आढावा

Foundry Local पूर्व-तयार केलेल्या मॉडेल्सच्या कॅटलॉगसह येतो, पण तुम्ही फक्त त्या यादीपुरते मर्यादित नाहीत. [Hugging Face](https://huggingface.co/) वर उपलब्ध असलेले कोणतेही ट्रान्सफॉर्मर-आधारित भाषा मॉडेल (किंवा PyTorch / Safetensors फॉरमॅटमध्ये स्थानिकरित्या संग्रहित) ऑप्टिमाइझ्ड ONNX मॉडेलमध्ये कंपाइल करून Foundry Local द्वारे सर्व्ह करू शकता.

कंपाइलेशन पाईपलाइन **ONNX Runtime GenAI Model Builder** वापरते, जे `onnxruntime-genai` पॅकेजमध्ये समाविष्ट असलेले एक कमांड-लाइन टूल आहे. हा मॉडेल बिल्डर मुख्य काम करतो: स्त्रोत वेट्स डाउनलोड करणे, त्यांना ONNX फॉरमॅटमध्ये रूपांतरित करणे, क्वांटायझेशन (int4, fp16, bf16) लागू करणे, आणि Foundry Local ला अपेक्षित असलेले कॉन्फिगरेशन फाइल्स (चाट टेम्पलेट आणि टोकनायझर यांसह) तयार करणे.

या लॅबमध्ये तुम्ही Hugging Face मधील **Qwen/Qwen3-0.6B** कंपाइल कराल, तो Foundry Local सह नोंदणी कराल, आणि पूर्णपणे तुमच्या डिव्हाइसवर त्याच्याशी चाट कराल.

---

## शिक्षण उद्दिष्टे

या लॅबच्या शेवटी तुम्ही सक्षम असाल:

- कस्टम मॉडेल कंपाइलिंग का उपयुक्त आहे आणि कधी ते आवश्यक असू शकते हे समजावून सांगणे
- ONNX Runtime GenAI मॉडेल बिल्डर इन्स्टॉल करणे
- एका आदेशाने Hugging Face मॉडेल ऑप्टिमाइझ्ड ONNX फॉरमॅटमध्ये कंपाइल करणे
- मुख्य कंपाइलेशन पॅरामीटर्स (एक्झिक्युशन प्रोव्हायडर, प्रिसिजन) समजून घेणे
- `inference_model.json` चाट-टेम्पलेट कॉन्फिगरेशन फाइल तयार करणे
- कंपाइल केलेला मॉडेल Foundry Local कॅशेमध्ये जोडणे
- CLI, REST API, आणि OpenAI SDK वापरून कस्टम मॉडेलच्या विरुद्ध इन्फरन्स चालवणे

---

## पूर्वतयारी

| आवश्यकता | तपशील |
|-------------|---------|
| **Foundry Local CLI** | इन्स्टॉल केलेले आणि तुमच्या `PATH` मध्ये ([भाग 1](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI मॉडेल बिल्डरसाठी आवश्यक |
| **pip** | Python पॅकेज व्यवस्थापक |
| **डिस्क स्पेस** | स्त्रोत आणि कंपाइल केलेल्या मॉडेल फाइल्ससाठी किमान 5 GB मोकळे |
| **Hugging Face खाते** | काही मॉडेल्स डाउनलोड करण्यापूर्वी परवाना स्वीकारणे आवश्यक असते. Qwen3-0.6B Apache 2.0 परवाना वापरतो आणि मोकळा आहे. |

---

## पर्यावरण सेटअप

मॉडेल कंपाइलेशनसाठी अनेक मोठ्या Python पॅकेजेसची आवश्यकता असते (PyTorch, ONNX Runtime GenAI, Transformers). तुमच्या सिस्टम Python किंवा इतर प्रकल्पांसोबत अडचण होऊ नये म्हणून एक वेगळे व्हर्च्युअल एन्व्हायर्नमेंट तयार करा.

```bash
# संग्रहालय मुळापासून
python -m venv .venv
```

एन्व्हायर्नमेंट सक्रिय करा:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

डिपेंडेंसी रिझोल्यूशन समस्या टाळण्यासाठी pip अपग्रेड करा:

```bash
python -m pip install --upgrade pip
```

> **टीप:** जर तुमच्याकडे आधीच आधीच्या लॅबमधून `.venv` असेल, तर तुम्ही ते पुन्हा वापरू शकता. फक्त ते पुढे सुरू करण्यापूर्वी सक्रिय करा याची खात्री करा.

---

## संकल्पना: कंपाइलेशन पाईपलाइन

Foundry Local ONNX फॉरमॅटमध्ये आणि ONNX Runtime GenAI कॉन्फिगरेशनसह मॉडेल्सची आवश्यकता असते. बहुतेक ओपन-सोर्स मॉडेल्स Hugging Face वर PyTorch किंवा Safetensors वेट्स म्हणून वितरित होतात, त्यामुळे रूपांतरित करण्याचा एक टप्पा आवश्यक आहे.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### मॉडेल बिल्डर काय करतो?

1. Hugging Face कडून स्त्रोत मॉडेल डाउनलोड करतो (किंवा स्थानिक पथातून वाचतो).
2. PyTorch / Safetensors वेट्स ONNX फॉरमॅटमध्ये रूपांतरित करतो.
3. मॉडेलला कमी प्रिसिजनमध्ये क्वांटाइज करतो (उदा., int4) ज्याने मेमरी वापर कमी होतो आणि थ्रूपुट वाढतो.
4. ONNX Runtime GenAI कॉन्फिगरेशन (`genai_config.json`), चाट टेम्पलेट (`chat_template.jinja`), आणि सर्व टोकनायझर फाइल्स तयार करतो, ज्यामुळे Foundry Local मॉडेल लोड करू आणि सर्व्ह करू शकते.

### ONNX Runtime GenAI Model Builder आणि Microsoft Olive

तुम्हाला मॉडेल ऑप्टिमायझेशनसाठी पर्यायी टूल म्हणून **Microsoft Olive** चा उल्लेख दिसू शकतो. दोन्ही टूल्स ONNX मॉडेल्स तयार करू शकतात, पण त्यांचे उपयोग वेगळे आहेत आणि त्यांचे फरक खालीलप्रमाणे आहेत:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **पॅकेज** | `onnxruntime-genai` | `olive-ai` |
| **प्राथमिक उद्देश** | ONNX Runtime GenAI इन्फरन्ससाठी जनरेटिव AI मॉडेल्सचे रूपांतर आणि क्वांटायझेशन | बरीच बॅकएंड्स आणि हार्डवेअर टार्गेटसाठी  एंड-टू-एंड मॉडेल ऑप्टिमायझेशन फ्रेमवर्क |
| **सोपे वापर** | एकाधिक आदेशांशिवाय एकच कमांड - रूपांतर आणि क्वांटायझेशन | वर्कफ्लो आधारित - YAML/JSON सह सानुकूल करता येणारे मल्टि-पास पाईपलाइन |
| **आउटपुट फॉरमॅट** | ONNX Runtime GenAI फॉरमॅट (Foundry Local साठी तत्पर) | जनरिक ONNX, ONNX Runtime GenAI, किंवा वर्कफ्लोवर अवलंबून इतर फॉरमॅट |
| **हार्डवेअर टार्गेट्स** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, इतर |
| **क्वांटायझेशन पर्याय** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, ग्राफ ऑप्टिमायझेशन, लेयर-वार ट्यूनिंग |
| **मॉडेल कव्हरेज** | जनरेटिव AI मॉडेल्स (LLMs, SLMs) | कोणतेही ONNX-रुपांतर करता येणारे मॉडेल (व्हिजन, NLP, ऑडिओ, मल्टीमोडल) |
| **सर्वोत्तम वापरासाठी** | स्थानिक इन्फरन्ससाठी जलद सिंगल-मॉडेल कंपाइलेशन | उत्पादन पाइपलाइनसाठी सूक्ष्म ऑप्टिमायझेशन नियंत्रण |
| **डिपेंडेंसी प्रभाव** | मध्यम (PyTorch, Transformers, ONNX Runtime) | मोठा (Olive फ्रेमवर्क, वैकल्पिक एक्स्ट्रा प्रत्येक वर्कफ्लोसाठी) |
| **Foundry Local समाकलन** | थेट — आउटपुट ताबडतोब सुसंगत | `--use_ort_genai` फ्लॅग आणि अतिरिक्त कॉन्फिगरेशन आवश्यक |

> **हे लॅब मॉडेल बिल्डर का वापरते:** एकाच Hugging Face मॉडेलला कंपाइल करून Foundry Local सह नोंदणी करण्याच्या कामासाठी, मॉडेल बिल्डर सर्वात सोपा आणि विश्वसनीय मार्ग आहे. तो Foundry Local ला अपेक्षित तीच नक्की आउटपुट फॉरमॅट एका कमांडमध्ये तयार करतो. नंतर जर तुम्हाला अचूकता-आधारित क्वांटायझेशन, ग्राफ सर्जरी, किंवा मल्टि-पास ट्यूनिंग सारख्या प्रगत ऑप्टिमायझेशन वैशिष्ट्यांची आवश्यकता भासली, तर Olive तपासणे फायदेशीर ठरेल. अधिक माहितीसाठी [Microsoft Olive दस्तऐवज](https://microsoft.github.io/Olive/) पहा.

---

## लॅब सराव

### सराव 1: ONNX Runtime GenAI Model Builder इन्स्टॉल करा

ONNX Runtime GenAI पॅकेज इन्स्टॉल करा, ज्यात मॉडेल बिल्डर टूल समाविष्ट आहे:

```bash
pip install onnxruntime-genai
```

मॉडेल बिल्डर उपलब्ध असल्याची पुष्टी करा:

```bash
python -m onnxruntime_genai.models.builder --help
```

तुम्हाला मदत आउटपुट दिसेल ज्यात `-m` (मॉडेल नाव), `-o` (आउटपुट पथ), `-p` (प्रिसिजन), आणि `-e` (एक्झिक्युशन प्रोव्हायडर) यांसारखे पॅरामीटर्स असतील.

> **टीप:** मॉडेल बिल्डरला PyTorch, Transformers आणि इतर अनेक पॅकेजेसची आवश्यकता आहे. इन्स्टॉलेशनला काही मिनिटे लागू शकतात.

---

### सराव 2: CPU साठी Qwen3-0.6B कंपाइल करा

Hugging Face कडून Qwen3-0.6B मॉडेल डाउनलोड करा आणि CPU इन्फरन्ससाठी int4 क्वांटायझेशनसह कंपाइल करण्यासाठी खालील आदेश चालवा:

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

#### प्रत्येक पॅरामीटर काय करतो

| पॅरामीटर | उद्देश | वापरलेले मूल्य |
|-----------|---------|------------|
| `-m` | Hugging Face मॉडेल आयडी किंवा स्थानिक डायरेक्टरी पथ | `Qwen/Qwen3-0.6B` |
| `-o` | जिथे कंपाइल केलेला ONNX मॉडेल सेव्ह होईल ती डायरेक्टरी | `models/qwen3` |
| `-p` | कंपाइलेशन दरम्यान लागू केलेला क्वांटायझेशन प्रिसिजन | `int4` |
| `-e` | ONNX Runtime एक्झिक्युशन प्रोव्हायडर (टार्गेट हार्डवेअर) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face प्रमाणीकरण वगळते (सार्वजनिक मॉडेल्ससाठी ठीक) | `hf_token=false` |

> **हा प्रक्रिया किती वेळ लागते?** कंपाइलेशनचा वेळ तुमच्या हार्डवेअर आणि मॉडेलच्या आकारावर अवलंबून असतो. एक आधुनिक CPU वापरताना Qwen3-0.6B साठी int4 क्वांटायझेशन सह साधारणत: ५ ते १५ मिनिटे लागतात. मोठ्या मॉडेलला आणखी जास्त वेळ लागतो.

हा आदेश पूर्ण झाल्यावर `models/qwen3` नावाची डायरेक्टरी तयार होईल ज्यात कंपाइल केलेल्या मॉडेल फाइल्स असतील. आउटपुट पुष्टी करा:

```bash
ls models/qwen3
```

तुम्हाला खालील फाइल्स दिसतील:
- `model.onnx` आणि `model.onnx.data` — कंपाइल केलेल्या मॉडेल वेट्स
- `genai_config.json` — ONNX Runtime GenAI कॉन्फिगरेशन
- `chat_template.jinja` — मॉडेलचे चाट टेम्पलेट (स्वतः तयार झालेले)
- `tokenizer.json`, `tokenizer_config.json` — टोकनायझर फाइल्स
- इतर शब्दसंग्रह आणि कॉन्फिगरेशन फाइल्स

---

### सराव 3: GPU साठी कंपाइल करा (ऐच्छिक)

तुमच्याकडे CUDA सपोर्टसह NVIDIA GPU असल्यास, जलद इन्फरन्ससाठी GPU-ऑप्टिमाइझ्ड व्हेरिअंट कंपाइल करू शकता:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **टीप:** GPU कंपाइलेशनसाठी `onnxruntime-gpu` आणि कार्यरत CUDA इन्स्टॉलेशन आवश्यक आहे. जर हे उपलब्ध नसल्यास, मॉडेल बिल्डर त्रुटी दाखवेल. तुम्ही हा सराव वगळू शकता आणि CPU व्हेरिअंटसह पुढे जाऊ शकता.

#### हार्डवेअर-विशिष्ट कंपाइलेशन संदर्भ

| टार्गेट | एक्झिक्युशन प्रोव्हायडर (`-e`) | शिफारसीय प्रिसिजन (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` किंवा `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` किंवा `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### प्रिसिजनचे फायदे आणि तोटे

| प्रिसिजन | आकार | गती | गुणवत्ता |
|-----------|------|-------|---------|
| `fp32` | सर्वात मोठा | सर्वात मंद | सर्वाधिक अचूकता |
| `fp16` | मोठा | जलद (GPU) | खूप चांगली अचूकता |
| `int8` | लहान | जलद | थोडीशी अचूकता कमी |
| `int4` | सर्वात लहान | सर्वात जलद | मध्यम प्रमाणात अचूकता कमी |

जास्तीतजास्त स्थानिक विकासासाठी CPU वर `int4` हे गती आणि संसाधन वापर यांचे सर्वोत्तम संतुलन देते. उत्पादन-गुणवत्तेसाठी CUDA GPU वर `fp16` शिफारस केली जाते.

---

### सराव 4: चाट टेम्पलेट कॉन्फिगरेशन तयार करा

मॉडेल बिल्डर आउटपुट डायरेक्टरीमध्ये आपोआप `chat_template.jinja` आणि `genai_config.json` फाइल निर्मिती करतो. पण Foundry Local ला `inference_model.json` फाइल देखील आवश्यक आहे ज्यायोगे मॉडेलसाठी प्रॉम्प्ट कसा फॉरमॅट करायचा हे समजते. ही फाइल मॉडेलचे नाव आणि वापरकर्त्याच्या संदेशांभोवती योग्य विशेष टोकन्स लपेटण्यासाठी प्रॉम्प्ट टेम्पलेट परिभाषित करते.

#### टप्पा 1: कंपाइल केलेल्या आउटपुटची तपासणी करा

कंपाइल केलेल्या मॉडेल डायरेक्टरीच्या सामग्रीची यादी करा:

```bash
ls models/qwen3
```

तुम्हाला खालील फाइल्स दिसतील:
- `model.onnx` आणि `model.onnx.data` — कंपाइल केलेले मॉडेल वेट्स
- `genai_config.json` — ONNX Runtime GenAI कॉन्फिगरेशन (स्वतः तयार झालेले)
- `chat_template.jinja` — मॉडेलचे चाट टेम्पलेट (स्वतः तयार झालेले)
- `tokenizer.json`, `tokenizer_config.json` — टोकनायझर फाइल्स
- विविध इतर कॉन्फिगरेशन आणि शब्दसंग्रह फाइल्स

#### टप्पा 2: inference_model.json फाइल तयार करा

`inference_model.json` फाइल Foundry Local ला सांगते की प्रॉम्प्ट कसा फॉरमॅट करायचा. एका Python स्क्रिप्टचे नाव `generate_chat_template.py` **रिपॉझिटरीच्या मूळ फोल्डरमध्ये** तयार करा (ज्याच्यात तुमचा `models/` फोल्डर आहे):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# चॅट टेम्पलेट काढण्यासाठी कमीतकमी संभाषण तयार करा
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

# inference_model.json ची रचना तयार करा
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

स्क्रिप्ट रिपॉझिटरीच्या मूळ फोल्डरमधून चालवा:

```bash
python generate_chat_template.py
```

> **टीप:** `onnxruntime-genai` च्या डिपेंडेंसी म्हणून `transformers` आधीच इन्स्टॉल झाले आहे. जर तुम्हाला `ImportError` दिसला तर प्रथम `pip install transformers` चालवा.

स्क्रिप्ट `models/qwen3` डायरेक्टरीमध्ये `inference_model.json` फाइल तयार करते. ही फाइल Foundry Local ला Qwen3 साठी वापरकर्ता इनपुट योग्य खास टोकन्समध्ये कशी गुंडाळायची ते सांगते.

> **महत्त्वाचे:** `inference_model.json` मधील `"Name"` फील्ड (या स्क्रिप्टमध्ये `qwen3-0.6b` म्हणून सेट केलेले) हा तुम्ही पुढील सर्व आदेश आणि API कॉल्समध्ये वापरणार असलेला **मॉडेल उपनाम** आहे. जर तुम्ही हे नाव बदलले, तर सराव 6–10 मध्ये मॉडेल नाव तसह अद्यतन करा.

#### टप्पा 3: कॉन्फिगरेशन तपासा

`models/qwen3/inference_model.json` उघडा आणि ते `Name` फील्ड आणि `PromptTemplate` ऑब्जेक्ट (ज्यात `assistant` आणि `prompt` कीज असतील) आहे याची पुष्टी करा. प्रॉम्प्ट टेम्पलेटमध्ये `<|im_start|>` आणि `<|im_end|>` सारखे खास टोकन्स असतील (हे टोकन्स मॉडेलच्या चाट टेम्पलेटवर अवलंबून वेगळे असू शकतात).

> **मॅन्युअल पर्याय:** जर तुम्हाला स्क्रिप्ट चालवायची इच्छा नसेल तर तुम्ही फाइल स्वतःही तयार करू शकता. मुख्य गरज अशी आहे की `prompt` फील्डमध्ये मॉडेलचा पूर्ण चाट टेम्पलेट `{Content}` या प्लेसेहोल्डरसह वापरकर्त्याच्या संदेशासाठी असावा.

---

### सराव 5: मॉडेल डायरेक्टरीची रचना तपासा
मॉडेल बिल्डर सर्व संकलित फाइल्स तुम्ही निर्दिष्ट केलेल्या आउटपुट डिरेक्टरीमध्ये थेट ठेवतो. अंतिम रचना योग्य दिसते का ते तपासा:

```bash
ls models/qwen3
```

डिरेक्टरीमध्ये खालील फाइल्स असाव्यात:

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

> **टीप:** काही इतर संकलन साधनांपासून वेगळे, मॉडेल बिल्डर नेस्टेड सबडिरेक्टरी तयार करत नाही. सर्व फाइल्स थेट आउटपुट फोल्डरमध्ये असतात, जे Foundry Local नेमके अपेक्षा करतो.

---

### व्यायाम 6: Foundry Local कॅशेमध्ये मॉडेल जोडा

Foundry Local ला तुमचा संकलित मॉडेल कुठे आहे ते सांगा, डिरेक्टरी कॅशेमध्ये जोडून:

```bash
foundry cache cd models/qwen3
```

कॅशेमध्ये मॉडेल दिसते का ते तपासा:

```bash
foundry cache ls
```

तुमचा कस्टम मॉडेल आधीच्या कॅशेमध्ये असलेल्या कोणत्याही मॉडेल्स (उदा. `phi-3.5-mini` किंवा `phi-4-mini`) सोबत सूचीबद्ध दिसेल.

---

### व्यायाम 7: CLI वापरून कस्टम मॉडेल चालवा

तुमच्या नुकत्याच संकलित केलेल्या मॉडेलसह ( `qwen3-0.6b` हा `inference_model.json` मधील `Name` फील्डमधून आलेला उपनाव आहे) संवादात्मक चॅट सत्र सुरू करा:

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` ध्वज अतिरिक्त निदान माहिती दाखवतो, जो प्रथमच कस्टम मॉडेल तपासताना उपयोगी ठरतो. जर मॉडेल यशस्वीपणे लोड झाले तर तुम्हाला संवादात्मक प्रॉम्प्ट दिसेल. काही संदेश टाकून पाहा:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

सत्र संपवण्यासाठी `exit` टाइप करा किंवा `Ctrl+C` दाबा.

> **समस्या निवारण:** जर मॉडेल लोड करणे अयशस्वी झाले, तर पुढील तपासा:
> - `genai_config.json` फाइल मॉडेल बिल्डरने निर्मित आहे का.
> - `inference_model.json` फाइल अस्तित्वात आहे आणि वैध JSON आहे का.
> - ONNX मॉडेल फाइल्स योग्य डिरेक्टरीत आहेत का.
> - तुमच्याकडे पुरेशी RAM उपलब्ध आहे का (Qwen3-0.6B int4 सुमारे 1 GB आवश्यक आहे).
> - Qwen3 हा एक reasoning मॉडेल आहे जो `<think>` टॅग निर्माण करतो. जर प्रतिक्रिया `<think>...</think>` सह सुरू होत असल्यास, हा सामान्य व्यवहार आहे. `inference_model.json` मधील प्रॉम्प्ट टेम्पलेट विचार व्हिज्युअल आउटपुट थांबविण्यासाठी समायोजित केला जाऊ शकतो.

---

### व्यायाम 8: REST API द्वारे कस्टम मॉडेलला क्वेरी करा

जर तुम्ही व्यायाम 7 मध्ये संवाद सत्रातून बाहेर पडले असाल, तर मॉडेल कदाचित आता लोड नसेल. Foundry Local सेवा सुरू करा आणि प्रथम मॉडेल लोड करा:

```bash
foundry service start
foundry model load qwen3-0.6b
```

सेवा कोणत्या पोर्टवर चालू आहे ते तपासा:

```bash
foundry service status
```

मग विनंती पाठवा (जर वेगळा पोर्ट असेल तर `5273` बदलून तुमचा वास्तविक पोर्ट वापरा):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows टीप:** वरच्या `curl` आदेशासाठी bash सिंटॅक्स वापरला आहे. Windows वर PowerShell `Invoke-RestMethod` कमांडला खाली वापरा.

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

### व्यायाम 9: OpenAI SDK सह कस्टम मॉडेल वापरा

तुम्ही तुमचा कस्टम मॉडेल बिल्ट-इन मॉडेल्ससाठी वापरलेल्या OpenAI SDK कोडसारखाच कनेक्ट करू शकता (पहा [भाग 3](part3-sdk-and-apis.md)). फक्त मॉडेल नाव वेगळे आहे.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # फाउंड्री लोकल API कीजची पडताळणी करत नाही
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
  apiKey: "foundry-local", // फाउंड्री लोकल API कीसची पडताळणी करत नाही
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

> **महत्वाचा मुद्दा:** Foundry Local OpenAI-संगत API उघडते, त्यामुळे बिल्ट-इन मॉडेल्ससह काम करणारा कोणताही कोड तुमच्या कस्टम मॉडेल्ससह देखील काम करतो. फक्त `model` पॅरामीटर बदलावे लागते.

---

### व्यायाम 10: Foundry Local SDK सह कस्टम मॉडेल तपासा

पूर्वीच्या प्रयोगशाळांमध्ये तुम्ही Foundry Local SDK वापरून सेवा सुरू केली, एंडपॉइंट शोधले आणि मॉडेल्स स्वयंचलितपणे व्यवस्थापित केल्या. तुम्ही कस्टम-compiled मॉडेलसह अगदी त्याच पद्धतीने पुढे जा शकता. SDK सेवा सुरू करणे आणि एंडपॉइंट शोधणे हँडल करते, त्यामुळे तुमच्या कोडमध्ये `localhost:5273` हार्ड-कोड करणे आवश्यक नाही.

> **टीप:** पुढील उदाहरणे चालवण्याआधी Foundry Local SDK इन्स्टॉल आहे याची खात्री करा:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` आणि `OpenAI` NuGet पॅकेजेस जोडा
>
> प्रत्येक स्क्रिप्ट फाइल **रेपॉझिटरी रूटमध्ये** जतन करा (तुमच्या `models/` फोल्डरच्या सारखीच डिरेक्टरी).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# टप्पा 1: Foundry Local सेवा सुरू करा आणि कस्टम मॉडेल लोड करा
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# टप्पा 2: कस्टम मॉडेलसाठी कॅश तपासा
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# टप्पा 3: मॉडेल मेमरीमध्ये लोड करा
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# टप्पा 4: SDK-आढळलेल्या एंडपॉइंटचा वापर करून OpenAI क्लायंट तयार करा
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# टप्पा 5: स्ट्रिमिंग चॅट पूर्णता विनंती पाठवा
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

चला चालवा:

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

// चरण 1: Foundry Local सेवा सुरू करा
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// चरण 2: कॅटलॉगमधून सानुकूल मॉडेल मिळवा
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// चरण 3: मॉडेल मेमरीमध्ये लोड करा
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// चरण 4: SDK-आढळलेल्या एंडपॉइंट वापरून OpenAI क्लायंट तयार करा
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// चरण 5: स्ट्रीमिंग चॅट पूर्णतेची विनंती पाठवा
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

चला चालवा:

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

> **महत्त्वाचा मुद्दा:** Foundry Local SDK एंडपॉइंट गतिशीलरित्या शोधतो, त्यामुळे तुम्ही कधीही पोर्ट नंबर हार्ड-कोड करत नाही. हे उत्पादन अनुप्रयोगांसाठी शिफारस केलेले प्रमाण आहे. तुमचा कस्टम-compiled मॉडेल SDK द्वारे बिल्ट-इन कॅटलॉग मॉडेल्ससारखे कार्य करतो.

---

## संकलनासाठी मॉडेलची निवड

Qwen3-0.6B हा संदर्भ उदाहरण म्हणून वापरला आहे कारण तो लहान, जलद संकलन करण्यास योग्य आणि Apache 2.0 परवान्याअंतर्गत मुक्त आहे. परंतु, तुम्ही अनेक इतर मॉडेल्स संकलित करू शकता. काही सूचना खाली दिल्या आहेत:

| मॉडेल | Hugging Face आयडी | पॅरामीटर्स | परवाना | टीपा |
|-------|------------------|-------------|--------|------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | खूप लहान, जलद संकलन, चाचणीसाठी चांगले |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | अधिक दर्जेदार, अजूनही जलद संकलन |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | मजबूत गुणवत्ता, अधिक RAM आवश्यक |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face वर परवाना स्वीकारणे आवश्यक |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | उच्च दर्जा, मोठा डाउनलोड आणि जास्त संकलन वेळ |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Foundry Local कॅटलॉगमध्ये आधीच आहे (तुलनेसाठी उपयुक्त) |

> **परवाना स्मरणपत्र:** वापरण्यापूर्वी Hugging Face वर मॉडेलचा परवाना तपासा. काही मॉडेल्स (जसे Llama) साठी परवाना करार स्वीकारणे आणि `huggingface-cli login` वापरून प्रमाणीकरण करणे आवश्यक आहे.

---

## संकल्पना: कधी कस्टम मॉडेल वापरावे

| परिस्थिती | तुमच्या स्वतःच्या मॉडेलचे संकलन का करावे? |
|----------|-----------------------------------------|
| **तुम्हाला हवा असलेला मॉडेल कॅटलॉगमध्ये नाही** | Foundry Local कॅटलॉग क्युरेटेड आहे. जर हवा असलेला मॉडेल यादीत नसेल तर स्वतः संकलित करा. |
| **फाईन-ट्यून केलेले मॉडेल्स** | जर तुम्ही विशिष्ट डोमेन डेटावर मॉडेल फाईन-ट्यून केले असेल तर स्वतःचे वजन संकलित करणे आवश्यक आहे. |
| **विशिष्ट क्वांटायझेशन गरजा** | तुम्हाला कॅटलॉगच्या डीफॉल्टपेक्षा वेगळा प्रिसिजन किंवा क्वांटायझेशन धोरण हवे असल्यास. |
| **नवीन मॉडेल रिलीजेस** | Hugging Face वर नवीन मॉडेल आले तर ते लगेच कॅटलॉगमध्ये असू नसेल. स्वतः संकलन केल्यास त्वरित प्रवेश मिळतो. |
| **संशोधन आणि प्रयोग** | उत्पादन निवडीपूर्वी विविध मॉडेल आर्किटेक्चर, आकार किंवा कॉन्फिगरेशन लोकली तपासणे. |

---

## सारांश

याप्रयोगशाळेत तुम्ही काय शिकलात:

| पाऊल | तुम्ही काय केले |
|-------|--------------|
| 1 | ONNX Runtime GenAI मॉडेल बिल्डर इन्स्टॉल केले |
| 2 | Hugging Face वरून `Qwen/Qwen3-0.6B` चे ऑप्टिमाइझ्ड ONNX मॉडेलमध्ये संकलन केले |
| 3 | `inference_model.json` चॅट-टेम्पलेट कॉन्फिगरेशन फाइल तयार केली |
| 4 | संकलित मॉडेल Foundry Local कॅशेमध्ये जोडले |
| 5 | CLI द्वारे कस्टम मॉडेलसह संवादात्मक चॅट चालवला |
| 6 | OpenAI-संगत REST API द्वारे मॉडेलला क्वेरी केली |
| 7 | Python, JavaScript, आणि C# मध्ये OpenAI SDK वापरून कनेक्ट केले |
| 8 | Foundry Local SDK सह कस्टम मॉडेल पूर्णपणे तपासले |

महत्त्वाचा मुद्दा म्हणजे **कोणतेही ट्रान्सफॉर्मर-आधारित मॉडेल ONNX स्वरूपात संकलित केल्यावर Foundry Local द्वारे चालू शकते**. OpenAI-संगत API मुळे तुमचा विद्यमान अॅप्लिकेशन कोड बदल न करता कार्य करते; फक्त मॉडेल नाव बदलणे आवश्यक आहे.

---

## महत्त्वाच्या मुद्द्यांचा आढावा

| संकल्पना | तपशील |
|---------|--------|
| ONNX Runtime GenAI मॉडेल बिल्डर | Hugging Face मॉडेल्सला ONNX स्वरूपात, क्वांटायझेशनसह एका आदेशात रूपांतरित करतो |
| ONNX स्वरूप | Foundry Local ONNX Runtime GenAI कॉन्फिगरेशनसह ONNX मॉडेल्सची गरज असते |
| चॅट टेम्पलेट्स | `inference_model.json` फाइल Foundry Local ला मॉडेलसाठी प्रॉम्प्ट कसे तयार करायचे ते सांगते |
| हार्डवेअर टार्गेट्स | CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), किंवा WebGPU साठी संकलन करा, तुमच्या हार्डवेअरनुसार |
| क्वांटायझेशन | कमी प्रिसिजन (int4) आकार कमी करतो आणि गती वाढवतो, काही अचूकतेच्या तोट्याने; fp16 GPU वर उच्च गुणवत्ता टिकवतो |
| API सुसंगतता | कस्टम मॉडेल्स बिल्ट-इन मॉडेल्ससारखेच OpenAI-संगत API वापरतात |
| Foundry Local SDK | SDK सेवा सुरू करणे, एंडपॉइंट शोधणे, आणि मॉडेल लोडिंग स्वयंचलितपणे हाताळतो, कॅटलॉग तसेच कस्टम मॉडेल्ससाठी |

---

## पुढील वाचनासाठी

| स्रोत | दुवा |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local कस्टम मॉडेल मार्गदर्शक | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 मॉडेल कुटुंब | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive दस्तऐवज | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## पुढील पाऊल

[भाग 11: लोकल मॉडेल्ससह टूल कॉलिंग](part11-tool-calling.md) येथे सुरू ठेवा, जेणेकरून तुमचे स्थानिक मॉडेल्स बाह्य फंक्शन्स कॉल करू शकतील.

[← भाग 9: व्हिस्पर व्हॉइस ट्रान्सक्रिप्शन](part9-whisper-voice-transcription.md) | [भाग 11: टूल कॉलिंग →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**अस्वीकरण**:
हा दस्तऐवज AI अनुवाद सेवा [Co-op Translator](https://github.com/Azure/co-op-translator) वापरून अनुवादित केला आहे. आम्ही अचूकतेसाठी प्रयत्न करतो, तरी कृपया लक्षात ठेवा की स्वयंचलित अनुवादात त्रुटी किंवा अचूकतेची चूक असू शकते. मूळ दस्तऐवज त्याच्या मूलभूत भाषेत अधिकृत स्रोत मानला पाहिजे. महत्त्वाची माहिती साठी, व्यावसायिक मानवी अनुवादशास्त्रज्ञाची शिफारस केली जाते. या अनुवादाच्या वापरामुळे उद्भवणाऱ्या कोणत्याही गैरसमज किंवा चुकीच्या अर्थलाभासाठी आम्हाला जबाबदार धरू नये.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->