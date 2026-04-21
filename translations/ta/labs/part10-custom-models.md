![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# பகுதி 10: Foundry Local உடன் தனிப்பயன் அல்லது Hugging Face மாடல்களை பயன்படுத்துவது

> **கோல்:** Foundry Local தேவையான மேம்படுத்தப்பட்ட ONNX வடிவத்தில் Hugging Face மாடலை தொகுக்க, அதனை உரையாடல் வார்ப்புருவுடன் கட்டமைக்க, உள்ளூர் கேச்-க்கு சேர்க்க, மற்றும் CLI, REST API, மற்றும் OpenAI SDK பயன்படுத்தி அதிலால் inference இயக்க.

## கண்ணோட்டம்

Foundry Local முன்னிறுத்திய முன்-தொகுக்கப்பட்ட மாடல்கள் கொண்ட ஒரு தொகுப்புடன் வருகிறது, ஆனால் நீங்கள் அந்த பட்டியலுக்குள் மட்டுமே இருக்க வேண்டும் என்ற கட்டாயம் இல்லை. [Hugging Face](https://huggingface.co/)இல் கிடைக்கும் எந்த трансформர்-அடிப்படையிலான மொழி மாடலும் (அல்லது PyTorch / Safetensors வடிவத்தில் உள்ளூர் சேமிக்கப்பட்டவை) மேம்படுத்தப்பட்ட ONNX மாடலாக தொகுக்கப்பட்டு Foundry Local வழியாக சேவையாக்கப்படலாம்.

தொகுப்பு குழாய் **ONNX Runtime GenAI Model Builder** என்ற கட்டளைக் கருவியைப் பயன்படுத்துகிறது, இது `onnxruntime-genai` தொகுப்புடன் சேர்க்கப்பட்டுள்ளது. மாடல் பில்டர் மிகப்பெரிய வேலையை மேற்கொள்கிறது: மூலம் எடைகளை பதிவிறக்கம் செய்தல், onnx வடிவத்திற்கு மாற்றல், அளவுரு குறைப்பு (int4, fp16, bf16) மற்றும் Foundry Local எதிர்பார்க்கும் கட்டமைப்பு கோப்புகளை (மொழியாடல் வார்ப்புருவும் tokeniserவும் உட்பட) வெளியீடு செய்தல்.

இந்த பயிற்சியில் நீங்கள் Hugging Face இலிருந்து **Qwen/Qwen3-0.6B** மodelன் தொகுத்து, அதை Foundry Local உடன் பதிவு செய்து, உங்கள் சாதனத்திலேயே முற்றிலும் உரையாடலாற்றல் செய்யலாம்.

---

## கற்றல் இலக்குகள்

இந்த பயிற்சிக்குப் பிறகு நீங்கள் செய்யக்கூடியவை:

- தனிப்பயன் மாடல் தொகுப்பு ஏன் பயனுள்ளதாக உள்ளது மற்றும் எப்போது தேவையாக இருக்கும் என்பதை விளக்கம் செய்தல்  
- ONNX Runtime GenAI மாடல் பில்டர் நிறுவல்  
- ஒரு கட்டளையுடன் Hugging Face மாடலை மேம்படுத்தப்பட்ட ONNX வடிவத்தில் தொகுக்க  
- முக்கிய தொகுப்பு அளவுருக்களை (நடைமுறை வழங்குநர், துல்லியம்) புரிந்து கொள்வது  
- `inference_model.json` உரையாடல் வார்ப்புரு கட்டமைப்பு கோப்பை உருவாக்குதல்  
- தொகுக்கப்பட்ட மாடலை Foundry Local கேச்சில் சேர்த்தல்  
- CLI, REST API மற்றும் OpenAI SDK மூலம் தனிப்பயன் மாடல் மீது inference இயக்கல்  

---

## முன் தேவைகள்

| தேவை | விவரங்கள் |
|-------------|---------|
| **Foundry Local CLI** | நிறுவப்பட்டு உங்கள் `PATH` இல் இருக்க வேண்டும் ([பகுதி 1](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI மாடல் பில்டருக்குத் தேவை |
| **pip** | Python தொகுப்பு நிர்வாகி |
| **கடிகாரம் இடம்** | மூலம் மற்றும் தொகுக்கப்பட்ட மாடல் கோப்புகளுக்கு குறைந்தது 5 GB இலவசம் |
| **Hugging Face கணக்கு** | சில மாடல்களுக்கு பதிவிறக்குவதற்கு முன் உரிமம் ஒப்புதல் தேவைப்படும். Qwen3-0.6B Apache 2.0 உரிமை கொண்டது மற்றும் இலவசமாகக் கிடைக்கும். |

---

## சூழல் அமைப்பு

மாடல் தொகுப்பிற்கு சில பெரிய Python தொகுப்புகள் (PyTorch, ONNX Runtime GenAI, Transformers) தேவை. இவை உங்கள் கணினியில் நிறுவப்பட்ட Python அல்லது பிற திட்டங்களுடன் மோத வேண்டாமென தனி வி.இ.டி. பயன்படுத்தவும்.

```bash
# சேமிப்பகத்தின் மூலத்திலிருந்து
python -m venv .venv
```

சூழலை இயக்கு:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

மேலதிகமான பாகுபாடு பிரச்சனைகளைத் தவிர்க்க pip மேம்படுத்தவும்:

```bash
python -m pip install --upgrade pip
```

> **அறிவு:** முந்தைய பயிற்சிகளின் `.venv` இருந்தால் அதை மீண்டும் பயன்படுத்தலாம். ஆனால் தொடர்ச்சியாக அதை இயக்கு என்பதை உறுதி செய்யவும்.

---

## கருத்து: தொகுப்பு குழாய்

Foundry Local ONNX வடிவமைப்பில் ONNX Runtime GenAI கட்டமைப்புடன் மாடல்களை தேவைப்படுகிறது. Hugging Face இல் உள்ள பெரும்பாலான திறந்த வினியோகம் மாடல்கள் PyTorch அல்லது Safetensors எடைகள் வடிவில் பரவலாக உள்ளதால் மாற்றுவழி கடமை.

![தனிப்பயன் மாடல் தொகுப்பு குழாய்](../../../images/custom-model-pipeline.svg)

### மாடல் பில்டர் என்ன செய்கிறது?

1. Hugging Face இலிருந்து மூலம் மாடலைப் பதிவிறக்கம் செய்கிறது (அல்லது உள்ளூர் பாதையில் இருந்து வாசிக்கிறது).
2. PyTorch / Safetensors எடைகளை ONNX வடிவிற்கு மாற்றுகிறது.
3. நினைவகம் பயன்பாடு குறைப்பதற்கும், செயல்திறனை மேம்படுத்த int4 போன்ற குறைவான துல்லியத்துக்கு அளவு குறைக்கிறது.
4. **genai_config.json**, **chat_template.jinja**, மற்றும் அனைத்து tokeniser கோப்புகளுடன் ONNX Runtime GenAI கட்டமைப்பை வெளியீடு செய்கிறது எனவே Foundry Local அந்த மாடலை ஏற்றவும் சேவையாற்றவும் முடியும்.

### ONNX Runtime GenAI Model Builder மற்றும் Microsoft Olive ஒப்பீடு

நீங்கள் மாடல் மேம்பாட்டிற்கான மாற்று கருவியான **Microsoft Olive** குறித்துறையைக் காணலாம். இரண்டு கருவிகளும் ONNX மாடல்களை உருவாக்கக்கூடியவை, ஆனால் அவர்கள் வித்தியாசமான நோக்கங்களுடன் மற்றும் வீழ்ச்சிகளுடன் செயல்படுகின்றனர்:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **பொதி** | `onnxruntime-genai` | `olive-ai` |
| **முக்கிய நோக்கம்** | ONNX Runtime GenAI inferenceக்கான உருவாக்கும் மற்றும் அளவுருக்கருவான AI மாடலை மாற்றும் | பல பின்தளங்கள் மற்றும் ஹார்ட்வேர்கள் குறித்த விருப்பத்துடன் மாடல் மேம்பாடு முழுமையான கட்டமைப்பு |
| **எளிமை** | ஒரே கட்டளையில் — ஒரே கட்டத்தில் மாற்றல் மற்றும் அளவுரு குறைப்பு | பணிச்சூழல் சார்ந்த — YAML/JSON உடன் பல கட்ட அட்டவணைகள் இருக்கக்கூடியது |
| **வெளியீட்டு வடிவம்** | ONNX Runtime GenAI வடிவம் (Foundry Localக்கான தயார்) | பொதுவான ONNX, ONNX Runtime GenAI அல்லது பணிசுழற்சி பொறுத்து வேறு வடிவங்கள் |
| **ஹார்ட்வேர்கள் இலக்கு** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN மற்றும் மேலும் பல |
| **அளவுரு குறைப்பு விருப்பங்கள்** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, மற்றும் மகர்கருத்திருத்தங்கள், படி வாரியான சரிசெய்தல் |
| **மாடல் வரம்பு** | உருவாக்கும் AI மாடல்கள் (LLMs, SLMs) | எதுவும் ONNX-க்கு மாற்றக்கூடிய மாடல் (காட்சி, இயற்கை மொழி, ஒலி, பலரீதியுடன்) |
| **சிறந்த பயன்படுத்தல்** | உள்ளூர் inferenceக்கான விரைவான தனி மாடல் தொகுப்பு | உற்பத்தி குழாய்பாதையில் நுட்ப மேம்பாட்டு கட்டுப்பாடு தேவையானவை |
| **பொறுமாற்றம் இருப்பு** | நடுத்தரம் (PyTorch, Transformers, ONNX Runtime) | பெரியது (Olive கட்டமைப்பு, விருப்ப மேலதிகங்கள்) |
| **Foundry Local ஒருங்கிணைப்பு** | நேரடி — வெளியீடு உடனடி பொருந்தும் | `--use_ort_genai` ப்ராம்ப்ட் மற்றும் மேலதிக கட்டமைப்பு தேவை |

> **இந்த பயிற்சியில் Model Builder ஏன் பயன்படுத்துகிறது:** ஒரு Hugging Face மாடலை மட்டும் தொகுத்து Foundry Local உடன் பதிவு செய்வதற்கான பணிக்கு Model Builder எளிதாகவும் நம்பகமாகவும் உள்ளது. அது Foundry Local எதிர்பார்க்கும் சரியான வெளியீட்டை ஒரே கட்டளையில் உருவாக்கும். பின்னர் நீங்கள் அதிக விரிவான மேம்பாட்டு அம்சங்கள் (துல்லிய அறிந்து கூர்மையான அளவுரு குறைப்பு, கிராப் அறுவை சிகிச்சை, பன்முறை சரிசெய்தல்) தேவைப்பட்டால் Olive ஒரு வலுவான தேர்வாகும். கூடுதல் விவரங்களுக்கு [Microsoft Olive ஆவணம்](https://microsoft.github.io/Olive/) ஐ பார்க்கவும்.

---

## பயிற்சி செயல்கள்

### பயிற்சி 1: ONNX Runtime GenAI Model Builder நிறுவுதல்

மாடல் பில்டர் கருவியை உள்ளடக்கிய ONNX Runtime GenAI தொகுப்பை நிறுவவும்:

```bash
pip install onnxruntime-genai
```

மாடல் பில்டர் கிடைப்பது சரிபார்க்க உதவியைப் பார்வையிடவும்:

```bash
python -m onnxruntime_genai.models.builder --help
```

உங்களுக்கு `-m` (மாடல் பெயர்), `-o` (வெளியீட்டுப் பாதை), `-p` (துல்லியம்), மற்றும் `-e` (நடைமுறை வழங்குநர்) போன்ற அளவுருக்கள் கொண்ட உதவி விளைவை காணலாம்.

> **குறிப்பு:** மாடல் பில்டர் PyTorch, Transformers மற்றும் மேலும் சில தொகுப்புகளுக்கு பொறுப்பானது. நிறுவுதல் சில நிமிடங்கள் ஆகலாம்.

---

### பயிற்சி 2: CPUக்கான Qwen3-0.6B தொகுக்க

கீழ்க்காணும் கட்டளையை இயக்கி Hugging Face இலிருந்து Qwen3-0.6B மாடலை பதிவிறக்கம் செய்து CPU inferenceக்கு int4 அளவுரு குறைப்புடன் தொகுக்கவும்:

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

#### ஒவ்வொரு அளவுருவின் விளக்கம்

| அளவுரு | நோக்கம் | பயன்படுத்திய மதிப்பு |
|-----------|---------|------------|
| `-m` | Hugging Face மாடல் ஐடி அல்லது உள்ளூர் அடைவு பாதை | `Qwen/Qwen3-0.6B` |
| `-o` | தொகுக்கப்பட்ட ONNX மாடல் சேமிக்கப்படும் அடைவு | `models/qwen3` |
| `-p` | தொகுப்பின் போது பயன்படுத்தப்படும் துல்லியம் | `int4` |
| `-e` | ONNX Runtime நடைமுறை வழங்குநர் (இயந்திரத்தின் இலக்கு ஹார்ட்வேரு) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face அங்கீகாரத்தை தவிர்க்க (பொது மாடல்களுக்கு சரி) | `hf_token=false` |

> **இது எவ்வளவு நேரம் எடுக்கும்?** தொகுப்பு நேரம் உங்களுடைய ஹார்ட்வேரு மற்றும் மாடல் அளவைக் குறிக்கும். புதிய CPU இல் Qwen3-0.6Bக்கு int4 அளவுருருடன் சுமார் 5 முதல் 15 நிமிடங்கள். பெரிய மாடல்கள் அதிக நேரம் எடுக்கும்.

கட்டளை முடிந்தவுடன் `models/qwen3` என்ற அடைவில் தொகுக்கப்பட்ட மாடல் கோப்புகளை காணலாம். வெளியீட்டை சரிபார்க்கவும்:

```bash
ls models/qwen3
```

கீழ்காணும் கோப்புகள் உள்ளன:
- `model.onnx` மற்றும் `model.onnx.data` — தொகுக்கப்பட்ட மாடல் எடைகள்
- `genai_config.json` — ONNX Runtime GenAI கட்டமைப்பு
- `chat_template.jinja` — மாடல் உரையாடல் வார்ப்புரு (தானாக உருவாக்கப்பட்டது)
- `tokenizer.json`, `tokenizer_config.json` — டோக்கனேஷன் கோப்புகள்
- மற்ற சொற்பொருள் மற்றும் கட்டமைப்பு கோப்புகள்

---

### பயிற்சி 3: GPUக்கான தொகுப்பாக்கம் ( விருப்பமானது )

நீங்கள் CUDA ஆதரவு கொண்ட NVIDIA GPU வைத்திருந்தால், விரைவான inferenceக்காக GPU-க்கு ஏற்ற மாறுபாட்டை தொகுக்கலாம்:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **குறிப்பு:** GPU தொகுப்பு `onnxruntime-gpu` மற்றும் செயல்படும் CUDA நிறுவலில் தேவை. அவை இல்லாவிட்டால் மாடல் பில்டர் பிழை தெரிவிக்கும். இந்தப் பயிற்சியை தவிர்த்து CPU மாறுபாட்டுடன் தொடரலாம்.

#### ஹார்ட்வேருக்கேற்ற தொகுப்பு குறிப்புகள்

| இலக்கு | நடைமுறை வழங்குநர் (`-e`) | பரிந்துரைக்கப்பட்ட துல்லியம் (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` அல்லது `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` அல்லது `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### துல்லியம் தொடர்பான ஒப்பீடு

| துல்லியம் | அளவு | வேகம் | தரம் |
|-----------|------|-------|---------|
| `fp32` | மிகப்பெரியது | மிகவும் மெதுவானது | மிக துல்லியமாகும் |
| `fp16` | பெரியது | விரைவானது (GPU) | நல்ல துல்லியம் |
| `int8` | சிறியது | வேகமானது | சிறிது துல்லியம் இழப்பு |
| `int4` | மிக சிறியது | மிகவும் வேகமானது | நடுத்தர துல்லியம் இழப்பு |

உள்ளூர் மேம்பாட்டுக்கு CPU இல் `int4` வேகம் மற்றும் வள பயன்பாட்டிற்கு சிறந்த சமநிலை வழங்கும். உற்பத்தித் தரத்திற்கு CUDA GPU இல் `fp16` பரிந்துரைக்கப்படுகிறது.

---

### பயிற்சி 4: உரையாடல் வார்ப்புரு கட்டமைப்பு உருவாக்குதல்

மாடல் பில்டர் தானாகவே வெளியீடு அடைவில் `chat_template.jinja` மற்றும் `genai_config.json` கோப்புகளை உருவாக்கும். இருப்பினும், Foundry Local க்கு `inference_model.json` கோப்பும் தேவை, இது உங்கள் மாடலுக்கான உரையாடல் விசைகளை எப்படி வடிவமைப்பது என்பதை அறிமுகப்படுத்தும். இந்த கோப்பு மாடல் பெயர் மற்றும் உரையாடல் கட்டமைப்பை வரையறு, இது பயனாளர் தகவல்களை சரியான சிறப்பு டோக்கன்களில் கட்டி ஏற்றுகொள்ள உதவும்.

#### படி 1: தொகுக்கப்பட்ட வெளியீட்டை பார்வையிடுதல்

தொகுக்கப்பட்ட மாடல் அடைவை பட்டியலிடவும்:

```bash
ls models/qwen3
```

கீழ்காணும் கோப்புகள் காணப்படலாம்:
- `model.onnx` மற்றும் `model.onnx.data` — தொகுக்கப்பட்ட மாடல் எடைகள்
- `genai_config.json` — ONNX Runtime GenAI கட்டமைப்பு (தானாக உருவாக்கப்பட்டது)
- `chat_template.jinja` — மாடலின் உரையாடல் வார்ப்புரு (தானாக உருவாக்கப்பட்டது)
- `tokenizer.json`, `tokenizer_config.json` — டோக்கனேஷன் கோப்புகள் 
- பல்வேறு கட்டமைப்பு மற்றும் சொற்பொருள் கோப்புகள்

#### படி 2: inference_model.json கோப்பு உருவாக்குதல்

`inference_model.json` கோப்பு Foundry Localக்கு உங்களுக்கு உரையாடலை எப்படி வடிவமைக்க வேண்டும் எனக் கூறுகிறது. நீங்கள் `models/` அடைவை கொண்டிருக்கும் **கையெழுத்திச் சுழற்சி அடைவிலேயே** `generate_chat_template.py` என்கிற Python ஸ்கிரிப்ட்டை உருவாக்கவும்:

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# உரையாடல் ஸ்தம்பிக்க ஒரு குறைந்தபட்ச உரையாடலை உருவாக்கவும்
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

# inference_model.json கட்டமைப்பை உருவாக்கவும்
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

ஸ்கிரிப்ட்டை கையெழுத்திச் சுழற்சி அடைவிலிருந்து இயக்கவும்:

```bash
python generate_chat_template.py
```

> **குறிப்பு:** `transformers` தொகுப்பு `onnxruntime-genai` உடனான சார்பு. நீங்கள் `ImportError` ஐ காணினால் `pip install transformers` என இயக்குங்கள்.

இந்த ஸ்கிரிப்ட் `models/qwen3` அடைவுக்குள் `inference_model.json` கோப்பை உருவாக்கும். இது Foundry Localக்கு Qwen3க்கு உரிய சிறப்பு டோக்கன்களில் பயனாளர் உள்ளீட்டை எப்படி சுற்றும்என்பதை கூறும்.

> **முக்கியம்:** `inference_model.json`இல் உள்ள `"Name"` புலம் (இந்த ஸ்கிரிப்ட்டில் `qwen3-0.6b`) அடுத்த கட்டளைகள் மற்றும் API அழைப்புகளில் நீங்கள் பயன்படுத்தும் **மாடல் மறுபெயர்** ஆகும். இதை மாற்றினால் பயிற்சிகள் 6–10இல் மாடல் பெயரை புதுப்பிக்கவும்.

#### படி 3: கட்டமைப்பை சரிபார்த்து காண்க

`models/qwen3/inference_model.json` ஐத் திறந்து பின்வருமாறு காணவும்: `Name` புலமும் `PromptTemplate` பொருளும் அதன் கீழ் `assistant` மற்றும் `prompt` விசைகளை உள்ளடக்கியிருக்க வேண்டும். prompt வார்ப்புரு `<|im_start|>` மற்றும் `<|im_end|>` போன்ற சிறப்பு டோக்கன்களை உள்ளடக்கியிருக்க வேண்டும் (சரியான டோக்கன்கள் மாடல் உரையாடல் வார்ப்புருவின் அடிப்படையில் மாறும்).

> **கைப்ப/manual மாற்று:** நீங்கள் ஸ்கிரிப்ட் இயக்க விரும்பவில்லை என்றால், கோப்பை கையால் உருவாக்கலாம். முக்கியமாக `prompt` புலம் மாடலின் முழு உரையாடல் வார்ப்புருவையும் `{Content}` எனும் பயனர் செய்தியின் இடமாற்று தன்மை கொண்ட தோற்றத்தையும் கொண்டிருக்க வேண்டும்.

---

### பயிற்சி 5: மாடல் அடைவு கட்டமைப்பை சரிபார்க்கவும்


மாதிரி கட்டமைப்பாளர் உங்கள் குறிப்பிட்ட வெளியீட்டு கோப்புறையில் அனைத்து தொகுக்கப்பட்ட கோப்புகளையும் நேரடியாக வைக்கிறார். இறுதி அமைப்பு சரியானதாக இருக்கிறது என்பதனை சரிபார்க்கவும்:

```bash
ls models/qwen3
```

கோப்புறை பின்வரும் கோப்புகளை கொண்டிருக்க வேண்டும்:

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

> **குறிப்பு:** சில பிற தொகுப்பு கருவிகளுக்கு மாறாக, மாதிரி கட்டமைப்பாளர் உள்-கோப்புறைகள் உருவாக்குவதில்லை. அனைத்து கோப்புகளும் நேரடியாக வெளியீட்டு கோப்புறையில் அமைகின்றன, இது Foundry Local எதிர்பார்க்கும் துல்லியமான அமைப்பாகும்.

---

### பயிற்சி 6: மாதிரியை Foundry Local கேஷில் சேர்க்கவும்

உங்கள் தொகுக்கப்பட்ட மாதிரியை எங்கு கண்டுபிடிக்க வேண்டும் என்பதைக் குறிப்பிட்டு Foundry Local இன் கேஷில் அந்த கோப்புறையை சேர்க்கவும்:

```bash
foundry cache cd models/qwen3
```

மாதிரி கேஷில் தோன்றுகிறதா என்று சரிபார்க்கவும்:

```bash
foundry cache ls
```

நீங்கள் உங்கள் தனிப்பயன் மாதிரியை முன்பிருந்த கேஷிடப்பட்ட மாதிரிகளுடன் (எ.கா., `phi-3.5-mini` அல்லது `phi-4-mini`) வரிசைப்படுத்தப்பட்டுள்ளது என்று காண்பீர்கள்.

---

### பயிற்சி 7: CLI மூலம் தனிப்பயன் மாதிரியை இயக்கவும்

புதிய தொகுக்கப்பட்ட மாதிரியுடன் (உதாரணமாக, `qwen3-0.6b` என்பது `inference_model.json` இல் நீங்கள் அமைத்துள்ள `Name` புலத்திலிருந்து வரும்) ஒரு குறைகருத்து உரையாடல் அமர்வைத் தொடங்கவும்:

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` கொடி கூடுதல் பாதிக்கப்பட்ட தகவல்களை காட்டுகிறது, இது முதலாம் முறையாக தனிப்பயன் மாதிரியை சோதிக்கும் போது உதவியாக இருக்கும். மாதிரி வெற்றிகரமாக ஏற்றப்பட்டால், நீங்கள் ஒரு இடைமுகக் குறிப்பை காணலாம். சில செய்திகள் அனுப்பி பாருங்கள்:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

அமர்வை முடிக்க `exit` என்று தட்டச்சு செய்யவும் அல்லது `Ctrl+C` அழுத்தவும்.

> **பிரச்சனைகள் தீர்க்க:** மாதிரி ஏற்ற முடியாவிட்டால், பின்வரும் விஷயங்களை சரிபார்க்கவும்:
> - `genai_config.json` கோப்பு மாதிரி கட்டமைப்பாளர் மூலம் உருவாக்கப்பட்டதா.
> - `inference_model.json` கோப்பு உள்ளதா மற்றும் செல்லுபடியான JSON ஆக உள்ளது.
> - ONNX மாதிரி கோப்புகள் சரியான கோப்புறையில் உள்ளன.
> - உங்களுக்கு போதுமான RAM உள்ளது (Qwen3-0.6B int4 சுமார் 1 GB தேவை).
> - Qwen3 என்பது `<think>` குறிச்சொற்களைப் உற்பத்தி செய்யும் காரணமயமான மாதிரி. பதில்களுக்கு முன்னில் `<think>...</think>` என்று தோன்றினால், அது சாதாரண நடைமுறையாகும். `inference_model.json` இல் உள்ள கூறிக் கட்டமைப்பை உள்ளடக்க விலக்கு செய்ய மாற்றலாம்.

---

### பயிற்சி 8: REST API மூலம் தனிப்பயன் மாதிரியை கேள்வி செய்யவும்

பயிற்சி 7 இல் நீங்கள் இடைமுக அமர்வை நிறுத்தியிருந்தால், மாதிரி ஏற்றப்படாமலும் இருக்கலாம். முதலில் Foundry Local சேவையை துவங்கி மாதிரியை ஏற்று கொள்ளவும்:

```bash
foundry service start
foundry model load qwen3-0.6b
```

சேவை எந்த போர்ட்டில் இயங்குகிறது என்று சரிபார்க்கவும்:

```bash
foundry service status
```

பிறகு கோரிக்கை அனுப்பவும் (`5273` என்பதை உங்கள் போர்ட் எண்ணுக்கு மாற்றவும், அது வேறுவாக இருந்தால்):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows குறிப்பு:** மேலே உள்ள `curl` கட்டளை bash விரிவாக்கத்தைப் பயன்படுத்துகிறது. Windows-ல் PowerShell இன் `Invoke-RestMethod` கட்டளையைப் பயன்படுத்தவும்.

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

### பயிற்சி 9: OpenAI SDK உடன் தனிப்பயன் மாதிரியை பயன்படுத்தவும்

உங்கள் தனிப்பயன் மாதிரியை உள்ளமைப்பு மாதிரிகளுக்கு நீங்கள் பயன்படுத்திய OpenAI SDK குறியீட்டுடன் அதேபோல் இணைக்கலாம் ([பகுதி 3](part3-sdk-and-apis.md) பார்க்கவும்).唯ஒரு மாறுதலாக மாதிரியின் பெயர் மட்டும்.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # ஃபாண்ட்ரி லோகல் API விசைகளை சரிபார்க்காது
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
  apiKey: "foundry-local", // Foundry லோக்கல் API முக்கியங்களை சரிபார்ப்பதில்லை
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

> **முக்கிய புள்ளி:** Foundry Local ஒரு OpenAI-பொருந்தும் API ஐ வெளிப்படுத்துவதால், உள்ளமைப்பு மாதிரிகளுடன் வேலைசெய்யும் எந்த குறியீடும் உங்கள் தனிப்பயன் மாதிரிகளுடன் வேலை செய்யும். உங்கள் தேவையானது `model` பராமரிப்பை மாற்றுவதே.

---

### பயிற்சி 10: Foundry Local SDK உடன் தனிப்பயன் மாதிரியை சோதிக்கவும்

முன்னர் நீங்கள் Foundry Local SDK ஐ பயன்படுத்தி சேவையை துவக்கி, இறுதிச்சூட்டைக் கண்டுபிடித்து மற்றும் மாதிரிகளை தானாகவே நிர்வகித்தீர்கள். இதே முறையை உங்கள் தனிப்பயன் தொகுக்கப்பட்ட மாதிரியுடனும் பின்பற்றலாம். SDK சேவை துவக்கம் மற்றும் இறுதிச்சூட்டைக் கண்டுபிடிப்பை கையாளும், ஆகவே உங்கள் குறியீட்டில் `localhost:5273` என்ற போர்ட் எண்ணை நேரடியாக குறிப்பிட தேவையில்லை.

> **குறிப்பு:** கீழ்காணும் உதாரணங்களை இயக்குவதற்கு முன் Foundry Local SDK நிறுவப்பட்டிருப்பதை உறுதி கொள்ளவும்:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` மற்றும் `OpenAI` NuGet தொகுப்புகளைச் சேர்க்கவும்
>
> ஒவ்வொரு ஸ்கிரிப்ட்பைலில் **மூல நிரப்பிக்கோப்பின் அடிப்பகுதியில்** சேமிக்கவும் (உங்கள் `models/` கோப்புறையோடு ஒரே கோப்புறை).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# படி 1: Foundry Local சேவையை தொடங்கி தனிப்பயன் மாதிரியை ஏற்று கொள்ளுங்கள்
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# படி 2: தனிப்பயன் மாதிரிக்கான கேஷையை சரிபார்க்கவும்
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# படி 3: மாதிரியை நினைவகத்தில் ஏற்றவும்
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# படி 4: SDK-யால் கண்டுபிடிக்கப்பட்ட இடைமுகமைக் கொண்டு OpenAI கிளையண்டைப் பயன்படுத்தி உருவாக்கவும்
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# படி 5: ஒற்றைப் பிரவேச உரையாடல் நிறைவு கோரிக்கையை அனுப்பவும்
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

இதை இயக்கவும்:

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

// படி 1: Foundry உள்ளூர் சேவையைத் தொடங்கு
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// படி 2: பட்டியலிலிருந்து தனிப்பயன் மாதிரியைப் பெறுக
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// படி 3: நினைவகத்தில் மாதிரியை ஏற்றுக
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// படி 4: SDK-கண்டுபிடிக்கப்பட்ட முடிவுச்சுட்டியைப் பயன்படுத்தி OpenAI கிளையன்டை உருவாக்குக
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// படி 5: ஓர் ஸ்ட்ரீமிங் அரட்டையுடன் முடிப்பு கோரிக்கையை அனுப்புக
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

இதை இயக்கவும்:

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

> **முக்கிய புள்ளி:** Foundry Local SDK இறுதிச்சூட்டைக் கண்டுபிடிப்பை தானாகக் கையாளும், எனவே நீங்கள் ஒருபோதும் போர்ட் எண்ணை நேரடியாக குறிப்பிட வேண்டாம். இது உற்பத்தி பயன்பாடுகளுக்கான பரிந்துரைக்கப்பட்ட நடைமுறை. உங்கள் தனிப்பயன் தொகுக்கப்பட்ட மாதிரி SDK மூலம் உள்ளமைப்பு பட்டியலிலுள்ள மாதிரிகளுபோல் முழுமையாக வேலை செய்கிறது.

---

## தொகுக்க ஒரு மாதிரியை தேர்ந்தெடுப்பது

Qwen3-0.6B இந்த ஆய்வரங்கத்தில் விளக்க உதாரணமாக பயன்படுத்தப்பட்டது, ஏனெனில் இது சிறியది, விரைவில் தொகுக்கப்படுகிறது, மற்றும் Apache 2.0 உரிமத்தில் இலவசமாக கிடைக்கிறது. இருப்பினும், நீங்கள் பல மாதிரிகளை தொகுக்கலாம். சில பரிந்துரைகள் இங்கே:

| மாதிரி | Hugging Face ஐடி | பராமரிப்புகள் | உரிமம் | குறிப்பு |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | மிகவும் சிறியது, விரைவான தொகுப்பு, சோதனைக்குப் பொருத்தமானது |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | சிறந்த தரம், இன்னும் விரைவில் தொகுக்கக்கூடியது |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | வலுவான தரம், அதிக RAM தேவைப்படுகிறது |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face இல் உரிமம் ஒப்புதலை தேவைப்படுத்துகிறது |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | உயர்தர, பெரிய பதிவிறக்கம் மற்றும் நீண்ட தொகுப்பு நேரம் |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | ஏற்கனவே Foundry Local பட்டியலில் உள்ளது (ஒப்பிட உகந்தது) |

> **உரிமம் நினைவூட்டல்:** அதை பயன்படுத்த மாற்றுகிற முன் Hugging Face இல் மாதிரியின் உரிமத்தை எப்போதும் சரிபார்க்கவும். சில மாதிரிகள் (எ.கா., Llama) உரிம ஒப்பந்தத்தை ஏற்றுக்கொள்ளவும் மற்றும் `huggingface-cli login` மூலம் அங்கீகரிக்கவும் வேண்டும்.

---

## கருத்துக்கள்: தனிப்பயன் மாதிரிகளை எப்போது பயன்படுத்த வேண்டும்

| நிலை | நீங்கள் ஏன் உங்கள் சொந்த மாதிரியை தொகுக்க வேண்டும்? |
|----------|----------------------|
| **கட்டுமான பட்டியலில் இல்லாத ஒரு மாதிரி உங்களுக்கு தேவை** | Foundry Local கட்டுமான பட்டியல் அனைத்து மாதிரிகளையும் கவனமாக தேர்ந்தெடுக்கிறது. நீங்கள் விரும்பும் மாதிரி பட்டியலில் இல்லையெனில், அதை உங்கள் சொந்தமாக தொகுக்கவும். |
| **நுணுக்கமாக மேம்படுத்தப்பட்ட மாதிரிகள்** | துறைக்கேற்ப தரவுகளில் நுணுக்கமாக மாற்றியமைப்புகள் varsa, உங்கள் சொந்த எடை கோப்புகளை தொகுக்க வேண்டும். |
| **குறிப்பிட்ட அளவு குறித்த தேவைகள்** | நீங்கள் தொகுப்பில் இல்லை எனினும் ஒரு துல்லியமான அளவு அல்லது குறிக்கும் திட்டத்தை விரும்பலாம். |
| **புதிய மாதிரி வெளியீடுகள்** | புதிய மாதிரி Hugging Face இல் வெளியிடப்பட்டால், அது Foundry Local பட்டியலில் இன்னும் சேர்க்கப்படாமலும் இருக்கலாம். நேரடியாக தொகுக்காப்பு விரைவான அணுகலை உங்களுக்கு தரும். |
| **ஆராய்ச்சி மற்றும் பரிசோதனை** | முன்கூரியுதவி தயாரிப்புக்கு முன் மாடல் கட்டமைப்புகள், அளவுகள் அல்லது உள்ளமைவுகளை உள்ளூர் மட்டத்தில் முயற்சி செய்தல். |

---

## சுருக்கம்

இந்த ஆய்வரங்கத்தில் நீங்கள் கற்றுக் கொண்டது:

| படி | நீங்கள் செய்தது |
|------|-------------|
| 1 | ONNX Runtime GenAI மாதிரி கட்டமைப்பாளரை நிறுவி |
| 2 | Hugging Face இல் இருந்து `Qwen/Qwen3-0.6B` ஐ தொகுத்து மேம்படுத்திய ONNX மாதிரியில் மாற்றி |
| 3 | `inference_model.json` உரையாடல் மாதிரி கட்டமைப்பு கோப்பை உருவாக்கி |
| 4 | தொகுக்கப்பட்ட மாதிரியை Foundry Local கேஷில் சேர்த்து |
| 5 | CLI மூலம் தனிப்பயன் மாதிரியுடன் உரையாடலும் |
| 6 | OpenAI பொருந்தும் REST API மூலம் மாதிரியை கேள்வி செய்தல் |
| 7 | Python, JavaScript மற்றும் C# பயன்படுத்தி OpenAI SDK மூலம் இணைப்பு செய்தல் |
| 8 | Foundry Local SDK மூலம் தனிப்பயன் மாதிரியை முழுமையாக சோதித்தல் |

முக்கியமானது: **ஏதேனும் மாற்றி அமைக்கப்பட்ட மாதிரி Foundry Local மூலம் ONNX வடிவத்தில் தொகுக்கப்பட்ட பிறகு இயக்க முடியும்**. OpenAI பொருந்தும் API என்பது உங்கள் உள்ளமைவு வகைப் பயன்பாட்டு குறியீடு எந்த மாற்றமுமின்றி இயங்கும் என்பதை அர்த்தம் கொண்டது; மாதிரியின் பெயரையும் மாற்றுவது மட்டும் போதும்.

---

## முக்கியமான கருத்துக்கள்

| கருத்து | விவரம் |
|---------|--------|
| ONNX Runtime GenAI Model Builder | Hugging Face மாதிரிகளை ONNX வடிவுக்கு ஒரே கட்டளையில் குறிக்கும் ஊடாக மாற்றுகிறது |
| ONNX வடிவம் | Foundry Local ONNX Runtime GenAI கட்டமைப்போடு ONNX மாதிரிகளை தேவைப்படுத்துகிறது |
| உரையாடல் மாதிரிகள் | `inference_model.json` கோப்பு ஒருவித மாதிரிக்கான முன்மொழிவுகளை Foundry Localக்கு சொல்லும் |
| உபகரண இலக்கு | CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), அல்லது WebGPU போன்ற உங்கள் உபகரணங்களுக்கு ஏற்ப தொகுக்கலாம் |
| குறித்தல் | குறைந்த துல்லியம் (int4) அளவு குறைக்கவும் வேகம் அதிகரிக்கவும் உதவுகிறது; fp16 உயர் தரத்தை பெறுகிறது GPUகளில் |
| API பொருந்துமை | தனிப்பயன் மாதிரிகள் உள்ளமைவு மாதிரிகள் பயன்படுத்தும் அதே OpenAI பொருந்தும் APIஐ பயன்படுத்துகின்றன |
| Foundry Local SDK | SDK சேவை துவக்கம், இறுதிச்சூட்டைக் கண்டுபிடிப்பு மற்றும் மாதிரி ஏற்றலை தானாக கையாள்கிறது, என்றமும் பட்டியலிலும் தனிப்பயனிலும் |

---

## மேலதிக வாசிப்பு

| வளம் | இணைப்பு |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local தனிப்பயன் மாதிரி வழிகாட்டி | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 மாதிரி குடும்பம் | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive ஆவணம் | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## அடுத்த படிகள்

[பகுதி 11: உள்ளூர் மாதிரிகள் மூலம் கருவி அழைப்புகள்](part11-tool-calling.md) தொடங்குங்கள், உங்கள் உள்ளூர் மாதிரிகள் வெளியே துறை செயல்பாடுகளை அழைக்க எப்படி உரிமம் பெறுவது என்று கற்றுக் கொள்ள.

[← பகுதி 9: விசை குரல் மாற்றம்](part9-whisper-voice-transcription.md) | [பகுதி 11: கருவி அழைப்புகள் →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**தயவு செய்த குறிப்பு**:  
இந்த ஆவணம் [Co-op Translator](https://github.com/Azure/co-op-translator) என்ற AI மொழிபெயர்ப்புச் சேவையை பயன்படுத்தி மொழிபெயர்க்கப்பட்டுள்ளது. நாம் சரியான மொழிபெயர்ப்பிற்காக முயற்சி செய்கிறோம் என்றாலும், தானாகச் செய்யப்படும் மொழிபெயர்ப்புகளில் பிழைகள் அல்லது தவறுதல்களிருக்கும் வாய்ப்பு உள்ளது என்பதை நினைவில் கொள்ளவும். மூல ஆவணம் அதன் இயல்பான மொழியில் அங்கீகாரம் பெற்ற ஆதாரமாக கருதப்பட வேண்டும். முக்கியமான தகவல்களுக்கு, தொழில்நுட்பமான மனித மொழிபெயர்ப்பு பரிந்துரைக்கப்படுகிறது. இந்த மொழிபெயர்ப்பைப் பயன்படுத்துவதால் தோன்றும் எந்தவொரு தவறான புரிதலுக்கும் நாங்கள் பொறுப்பேற்கமாட்டோம்.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->