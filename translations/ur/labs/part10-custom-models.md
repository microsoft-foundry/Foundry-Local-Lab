![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# حصہ 10: Foundry Local کے ساتھ کسٹم یا Hugging Face ماڈلز کا استعمال

> **مقصد:** ایک Hugging Face ماڈل کو ایسے بہتر کردہ ONNX فارمیٹ میں کمپائل کرنا جو Foundry Local کو درکار ہو، اسے چیٹ ٹیمپلیٹ کے ساتھ ترتیب دینا، لوکل کیشے میں شامل کرنا، اور CLI, REST API، اور OpenAI SDK کے ذریعے اس کے خلاف انفیرینس چلانا۔

## جائزہ

Foundry Local پہلے سے کمپائل شدہ منتخب ماڈلز کا کیٹلاگ فراہم کرتا ہے، لیکن آپ اس فہرست تک محدود نہیں ہیں۔ کوئی بھی ٹرانسفارمر بیسڈ لینگویج ماڈل جو [Hugging Face](https://huggingface.co/) پر دستیاب ہو (یا PyTorch / Safetensors فارمیٹ میں لوکل اسٹور کیا گیا ہو) کو بہتر کردہ ONNX ماڈل میں کمپائل کیا جا سکتا ہے اور Foundry Local کے ذریعے فراہم کیا جا سکتا ہے۔

کمپائل کرنے کا عمل **ONNX Runtime GenAI Model Builder** استعمال کرتا ہے، جو `onnxruntime-genai` پیکیج کے ساتھ شامل ایک کمانڈ لائن ٹول ہے۔ ماڈل بلڈر بھاری کام سنبھالتا ہے: ماخذ ویٹس ڈاؤنلوڈ کرنا، انہیں ONNX فارمیٹ میں تبدیل کرنا، مقداری تبدیلی (int4, fp16, bf16) لگانا، اور وہ کنفگریشن فائلز (جس میں چیٹ ٹیمپلیٹ اور ٹوکنائزر شامل ہیں) جاری کرنا جن کی Foundry Local کو ضرورت ہوتی ہے۔

اس لیب میں آپ Hugging Face سے **Qwen/Qwen3-0.6B** کو کمپائل کریں گے، اسے Foundry Local میں رجسٹر کریں گے، اور پورے طور پر اپنے ڈیوائس پر اس سے چیٹ کریں گے۔

---

## سیکھنے کے مقاصد

اس لیب کے آخر تک آپ درج ذیل کرنے کے قابل ہوں گے:

- وضاحت کریں کہ کسٹم ماڈل کمپائلیشن کب اور کیوں مفید ہے  
- ONNX Runtime GenAI ماڈل بلڈر انسٹال کرنا  
- ایک کمانڈ کے ذریعے Hugging Face ماڈل کو بہتر ONNX فارمیٹ میں کمپائل کرنا  
- کلیدی کمپائلیشن پیرامیٹرز (ایگزیکیوشن پرووائیڈر، پریسجن) سمجھنا  
- `inference_model.json` چیٹ ٹیمپلیٹ کنفگریشن فائل بنانا  
- کمپائل شدہ ماڈل کو Foundry Local کیشے میں شامل کرنا  
- CLI، REST API، اور OpenAI SDK کے ذریعے کسٹم ماڈل کے خلاف انفیرینس چلانا  

---

## پیشگی ضروریات

| ضروریات | تفصیلات |
|-------------|---------|
| **Foundry Local CLI** | انسٹال شدہ اور آپ کے `PATH` میں ([حصہ 1](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI ماڈل بلڈر کی ضرورت ہے |
| **pip** | پائتھون پیکیج مینیجر |
| **ڈسک اسپیس** | ماخذ اور کمپائل شدہ ماڈل فائلز کے لیے کم از کم 5 جی بی خالی جگہ |
| **Hugging Face اکاؤنٹ** | کچھ ماڈلز کے لیے ڈاؤن لوڈ سے پہلے لائسنس قبول کرنا ضروری ہے۔ Qwen3-0.6B Apache 2.0 لائسنس کے تحت مفت دستیاب ہے۔ |

---

## ماحول کی ترتیب

ماڈل کمپائلیشن کئی وزنی پائتھون پیکیجز (PyTorch, ONNX Runtime GenAI, Transformers) کی ضرورت ہوتی ہے۔ ایک وقف شدہ ورچوئل ماحول بنائیں تاکہ یہ آپ کے سسٹم پائتھون یا دیگر پروجیکٹس میں مداخلت نہ کریں۔

```bash
# ذخیرہ جاتی جڑ سے
python -m venv .venv
```

ماحول کو فعال کریں:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

pip کو اپ گریڈ کریں تاکہ انحصار کے مسائل سے بچا جا سکے:

```bash
python -m pip install --upgrade pip
```

> **مشورہ:** اگر آپ کے پاس پہلے کی لیبز کی `.venv` موجود ہے تو اسے دوبارہ استعمال کیا جا سکتا ہے۔ بس یقینی بنائیں کہ جاری رکھنے سے پہلے وہ فعال ہو۔

---

## تصور: کمپائلیشن پائپ لائن

Foundry Local کو ONNX فارمیٹ میں ماڈلز درکار ہوتے ہیں جن کا ONNX Runtime GenAI ترتیب ہو۔ زیادہ تر اوپن سورس ماڈلز جو Hugging Face پر ہیں وہ PyTorch یا Safetensors ویٹس کے طور پر دستیاب ہوتے ہیں، اس لیے تبدیلی کا ایک مرحلہ ضروری ہے۔

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### ماڈل بلڈر کیا کرتا ہے؟

1. Hugging Face سے ماخذ ماڈل ڈاؤنلوڈ کرتا ہے (یا لوکل راستے سے پڑھتا ہے)۔  
2. PyTorch / Safetensors ویٹس کو ONNX فارمیٹ میں تبدیل کرتا ہے۔  
3. ماڈل کو چھوٹے پریسجن میں مقداری تبدیلی دیتا ہے (مثلاً int4) تاکہ میموری کی کھپت کم ہو اور تھروپٹ بہتر ہو۔  
4. ONNX Runtime GenAI کی کنفگریشن (`genai_config.json`)، چیٹ ٹیمپلیٹ (`chat_template.jinja`)، اور تمام ٹوکنائزر فائلز جاری کرتا ہے تاکہ Foundry Local ماڈل کو لوڈ اور فراہم کر سکے۔

### ONNX Runtime GenAI Model Builder بمقابلہ Microsoft Olive

آپ کو **Microsoft Olive** کے حوالہ جات مل سکتے ہیں جو ماڈل کی بہتری کے لیے ایک متبادل آلہ ہے۔ دونوں ٹول ONNX ماڈلز تیار کر سکتے ہیں، لیکن ان کے مقاصد اور خصوصیات مختلف ہیں:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **پیکیج** | `onnxruntime-genai` | `olive-ai` |
| **اصلی مقصد** | ONNX Runtime GenAI انفیرینس کے لیے جنریٹو AI ماڈلز کو تبدیل اور مقداری بنانا | متعدد بیک اینڈز اور ہارڈویئر ٹارگٹس کی سپورٹ کے ساتھ مکمل ماڈل بہتر سازی کا فریم ورک |
| **استعمال میں آسانی** | ایک کمانڈ — سنگل اسٹیپ کنورژن + مقداری | ورک فلو بیسڈ — YAML/JSON کے ساتھ کنفیگر ایبل ملٹی پاس پائپ لائنز |
| **آؤٹ پٹ فارمیٹ** | ONNX Runtime GenAI فارمیٹ (Foundry Local کے لیے تیار) | جنرل ONNX، ONNX Runtime GenAI، یا ورک فلو کے مطابق دیگر فارمیٹس |
| **ہارڈویئر ٹارگٹس** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN، اور مزید |
| **مقداری اختیارات** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16، پلس گراف آپٹیمائزیشنز، لیئر وائز ٹیوننگ |
| **ماڈل کی رینج** | جنریٹو AI ماڈلز (LLMs, SLMs) | کوئی بھی ONNX میں تبدیل ہونے والا ماڈل (ویژن، NLP، آڈیو، ملٹی موڈل) |
| **سب سے بہتر** | لوکل انفیرینس کے لیے تیز رفتار واحد ماڈل کمپائلیشن | پروڈکشن پائپ لائنز جنہیں باریک بینی سے کنٹرول کی ضرورت ہو |
| **انحصار** | معتدل (PyTorch, Transformers, ONNX Runtime) | زیادہ (Olive فریم ورک شامل، ورک فلو کے مطابق اضافی) |
| **Foundry Local انضمام** | براہ راست — آؤٹ پٹ فوری طور پر موافق | `--use_ort_genai` فلیگ اور اضافی کنفگریشن کی ضرورت |

> **یہ لیب ماڈل بلڈر کیوں استعمال کرتی ہے؟** ایک واحد Hugging Face ماڈل کو کمپائل اور Foundry Local میں رجسٹر کرنے کے لیے ماڈل بلڈر سب سے آسان اور معتبر راستہ ہے۔ یہ وہی آؤٹ پٹ فارمیٹ تیار کرتا ہے جس کی Foundry Local کو ضرورت ہوتی ہے صرف ایک کمانڈ میں۔ اگر آپ کو بعد میں جدید بہتری کی خصوصیات چاہیے جیسے درستگی پر مبنی مقداری تبدیلی، گراف سرجری، یا ملٹی پاس ٹیوننگ، تو Olive ایک مضبوط اختیار ہے۔ مزید تفصیلات کے لیے [Microsoft Olive کی دستاویزات](https://microsoft.github.io/Olive/) دیکھیں۔

---

## لیب کے مشقیں

### مشق 1: ONNX Runtime GenAI Model Builder انسٹال کریں

ONNX Runtime GenAI پیکیج انسٹال کریں جس میں ماڈل بلڈر ٹول شامل ہے:

```bash
pip install onnxruntime-genai
```

تنصیب کی تصدیق کے لیے چیک کریں کہ ماڈل بلڈر دستیاب ہے:

```bash
python -m onnxruntime_genai.models.builder --help
```

آپ کو ہیلپ آؤٹ پٹ میں ایسے پیرامیٹرز نظر آئیں گے جیسے `-m` (ماڈل نام), `-o` (آؤٹ پٹ پاتھ), `-p` (پریسجن)، اور `-e` (ایگزیکیوشن پرووائیڈر)۔

> **نوٹ:** ماڈل بلڈر PyTorch, Transformers، اور کئی دیگر پیکیجز پر انحصار کرتا ہے۔ تنصیب میں کچھ منٹ لگ سکتے ہیں۔

---

### مشق 2: Qwen3-0.6B کو CPU کے لیے کمپائل کریں

مندرجہ ذیل کمانڈ چلائیں تاکہ Hugging Face سے Qwen3-0.6B ماڈل ڈاؤن لوڈ ہو اور int4 مقداری اصلاح کے ساتھ CPU انفیرینس کے لیے کمپائل ہو جائے:

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

#### ہر پیرامیٹر کا مطلب

| پیرامیٹر | مقصد | استعمال شدہ قدر |
|-----------|---------|------------|
| `-m` | Hugging Face ماڈل کی شناخت یا لوکل ڈائریکٹری پاتھ | `Qwen/Qwen3-0.6B` |
| `-o` | وہ فولڈر جہاں کمپائل شدہ ONNX ماڈل محفوظ ہوگا | `models/qwen3` |
| `-p` | کمپائلیشن کے دوران لگائی گئی مقداری پریسجن | `int4` |
| `-e` | ONNX Runtime ایگزیکیوشن پرووائیڈر (ہدف ہارڈویئر) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face تصدیق کو چھوڑ دیتا ہے (پبلک ماڈلز کے لیے ٹھیک ہے) | `hf_token=false` |

> **وقت کتنا لگتا ہے؟** کمپائلیشن کا وقت آپ کی ہارڈویئر اور ماڈل کے حجم پر منحصر ہے۔ Qwen3-0.6B کے ساتھ int4 پر ایک جدید CPU پر تقریباً 5 سے 15 منٹ لگتے ہیں۔ بڑے ماڈلز مزید وقت لیتے ہیں۔

کمانڈ مکمل ہونے کے بعد آپ کو `models/qwen3` فولڈر نظر آئے گا جس میں کمپائل شدہ ماڈل کی فائلز ہوں گی۔ آؤٹ پٹ کی تصدیق کریں:

```bash
ls models/qwen3
```

آپ کو درج ذیل فائلز نظر آئیں گی:
- `model.onnx` اور `model.onnx.data` — کمپائل شدہ ماڈل ویٹس  
- `genai_config.json` — ONNX Runtime GenAI کنفگریشن  
- `chat_template.jinja` — ماڈل کا چیٹ ٹیمپلیٹ (خودکار)  
- `tokenizer.json`, `tokenizer_config.json` — ٹوکنائزر فائلز  
- دیگر وکابیولری اور کنفگریشن فائلز  

---

### مشق 3: GPU کے لیے کمپائل کریں (اختیاری)

اگر آپ کے پاس NVIDIA GPU ہے جس میں CUDA سپورٹ ہے، تو آپ تیز تر انفیرینس کے لیے GPU کے لیے بہتر کردہ ورژن کمپائل کر سکتے ہیں:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **نوٹ:** GPU کمپائلیشن کے لیے `onnxruntime-gpu` اور ایک کام کرنے والی CUDA تنصیب ضروری ہے۔ اگر یہ نہ ہو، تو ماڈل بلڈر ایرر دے گا۔ آپ اس مشق کو چھوڑ کر CPU ورژن کے ساتھ جاری رکھ سکتے ہیں۔

#### ہارڈویئر مخصوص کمپائلیشن حوالہ

| ہدف | ایگزیکیوشن پرووائیڈر (`-e`) | سفارش کردہ پریسجن (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` یا `int4` |
| DirectML (ونڈوز GPU) | `dml` | `fp16` یا `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### پریسجن کے فائدے اور نقصانات

| پریسجن | حجم | رفتار | معیار |
|-----------|------|-------|---------|
| `fp32` | سب سے بڑا | سب سے سست | سب سے زیادہ درستگی |
| `fp16` | بڑا | تیز (GPU) | بہت اچھی درستگی |
| `int8` | چھوٹا | تیز | معمولی درستگی میں کمی |
| `int4` | سب سے چھوٹا | سب سے تیز | معتدل درستگی کی کمی |

زیادہ تر لوکل ڈیولپمنٹ کے لیے CPU پر `int4` رفتار اور وسائل کے استعمال کا بہترین توازن فراہم کرتا ہے۔ پروڈکشن معیار کے آؤٹ پٹ کے لیے CUDA GPU پر `fp16` تجویز کیا جاتا ہے۔

---

### مشق 4: چیٹ ٹیمپلیٹ کنفگریشن بنائیں

ماڈل بلڈر خودکار طور پر `chat_template.jinja` فائل اور `genai_config.json` فائل آؤٹ پٹ ڈائریکٹری میں بناتا ہے۔ تاہم، Foundry Local کو ایک `inference_model.json` فائل بھی چاہیے تاکہ یہ سمجھ سکے کہ آپ کے ماڈل کے لیے پرامپٹس کو کیسے فارمیٹ کیا جائے۔ یہ فائل ماڈل کا نام اور وہ پرامپٹ ٹیمپلیٹ بیان کرتی ہے جو صارف کے پیغامات کو صحیح خاص ٹوکنز کے اندر لپیٹتا ہے۔

#### قدم 1: کمپائل شدہ آؤٹ پٹ کا معائنہ کریں

کمپائل شدہ ماڈل ڈائریکٹری کا مواد دیکھیں:

```bash
ls models/qwen3
```

آپ کو فائلیں نظر آئیں گی جیسے:
- `model.onnx` اور `model.onnx.data` — کمپائل شدہ ماڈل ویٹس  
- `genai_config.json` — ONNX Runtime GenAI کنفگریشن (خودکار)  
- `chat_template.jinja` — ماڈل کا چیٹ ٹیمپلیٹ (خودکار)  
- `tokenizer.json`, `tokenizer_config.json` — ٹوکنائزر فائلز  
- مختلف دیگر کنفگریشن اور وکابیولری فائلز  

#### قدم 2: inference_model.json فائل بنائیں

`inference_model.json` فائل Foundry Local کو بتاتی ہے کہ پرامپٹس کو کیسے فارمیٹ کیا جائے۔ ایک پائتھون اسکرپٹ `generate_chat_template.py` تخلیق کریں **ریپوزٹری کی روٹ میں** (وہی ڈائریکٹری جہاں `models/` فولڈر موجود ہے):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# بات چیت کا ایک کم از کم ڈھانچہ تیار کریں تاکہ چیٹ ٹیمپلیٹ نکالا جا سکے
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

# inference_model.json کا ڈھانچہ تیار کریں
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

اسکرپٹ کو ریپوزٹری روٹ سے چلائیں:

```bash
python generate_chat_template.py
```

> **نوٹ:** `transformers` پیکیج پہلے ہی `onnxruntime-genai` کی ایک انحصاری کے طور پر انسٹال ہے۔ اگر آپ کو `ImportError` ملے تو پہلے `pip install transformers` چلائیں۔

اسکرپٹ `models/qwen3` فولڈر میں `inference_model.json` فائل بنائے گا۔ یہ فائل Foundry Local کو بتاتی ہے کہ Qwen3 کے لیے صارف کے ان پٹ کو کس طرح خاص ٹوکنز میں لپیٹا جائے۔

> **اہم:** `inference_model.json` میں `"Name"` فیلڈ (جسے اس اسکرپٹ میں `qwen3-0.6b` رکھا گیا ہے) ماڈل کا عرفی نام ہے جسے آپ تمام بعد کی کمانڈز اور API کالز میں استعمال کریں گے۔ اگر آپ اس نام کو تبدیل کرتے ہیں تو مشق 6–10 میں ماڈل کا نام بھی اپڈیٹ کریں۔

#### قدم 3: کنفگریشن کی تصدیق کریں

`models/qwen3/inference_model.json` کھولیں اور تصدیق کریں کہ اس میں `Name` فیلڈ اور `PromptTemplate` آبجیکٹ موجود ہیں جس میں `assistant` اور `prompt` کیز ہوں۔ پرامپٹ ٹیمپلیٹ میں خاص ٹوکنز جیسے `<|im_start|>` اور `<|im_end|>` شامل ہونے چاہئیں (یہ ٹوکنز ماڈل کے چیٹ ٹیمپلیٹ کے مطابق مختلف ہو سکتے ہیں)۔

> **دستی متبادل:** اگر آپ اسکرپٹ نہیں چلانا چاہتے تو یہ فائل دستی طور پر بھی بنا سکتے ہیں۔ بنیادی شرط یہ ہے کہ `prompt` فیلڈ میں ماڈل کا پورا چیٹ ٹیمپلیٹ شامل ہو `{Content}` کے ساتھ جہاں یوزر کا پیغام آئے۔

---

### مشق 5: ماڈل ڈائریکٹری کی ساخت کی تصدیق کریں
ماڈل بلڈر تمام مرتب شدہ فائلوں کو براہِ راست اس آؤٹ پٹ ڈائریکٹری میں رکھتا ہے جسے آپ نے مخصوص کیا ہے۔ تصدیق کریں کہ حتمی ساخت درست دکھائی دے رہی ہے:

```bash
ls models/qwen3
```

ڈائریکٹری میں درج ذیل فائلیں ہونی چاہئیں:

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

> **نوٹ:** بعض دوسرے کمپائلیشن ٹولز کے برخلاف، ماڈل بلڈر گہری ذیلی ڈائریکٹریز نہیں بناتا۔ تمام فائلیں براہِ راست آؤٹ پٹ فولڈر میں رہتی ہیں، جو بالکل ویسا ہی ہے جیسا Foundry Local توقع کرتا ہے۔

---

### مشق 6: ماڈل کو Foundry Local کیش میں شامل کریں

اپنے مرتب کردہ ماڈل کو تلاش کرنے کے لیے Foundry Local کو مطلع کرنے کے لیے ڈائریکٹری کو اس کے کیش میں شامل کریں:

```bash
foundry cache cd models/qwen3
```

تصدیق کریں کہ ماڈل کیش میں ظاہر ہو رہا ہے:

```bash
foundry cache ls
```

آپ کو اپنا کسٹم ماڈل پہلے سے کیش کیے گئے کسی ماڈل (جیسے `phi-3.5-mini` یا `phi-4-mini`) کے ساتھ فہرست میں نظر آئے گا۔

---

### مشق 7: CLI کے ذریعے کسٹم ماڈل چلائیں

اپنے حال ہی میں مرتب کیے گئے ماڈل کے ساتھ ایک انٹرایکٹو چیٹ سیشن شروع کریں ( `qwen3-0.6b` عرف نام `inference_model.json` کی فائل میں `Name` فیلڈ سے آتا ہے):

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` جھنڈا اضافی تشخیصی معلومات ظاہر کرتا ہے، جو پہلی بار کسٹم ماڈل کی جانچ کے دوران مددگار ہے۔ اگر ماڈل کامیابی سے لوڈ ہو جاتا ہے تو آپ کو ایک انٹرایکٹو پرامپٹ نظر آئے گا۔ چند پیغامات آزما کر دیکھیں:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

سیشن ختم کرنے کے لیے `exit` ٹائپ کریں یا `Ctrl+C` دبائیں۔

> **مسائل کا حل:** اگر ماڈل لوڈ ہونے میں ناکام ہو تو درج ذیل چیک کریں:
> - `genai_config.json` فائل ماڈل بلڈر نے بنائی ہے۔
> - `inference_model.json` فائل موجود ہے اور درست JSON ہے۔
> - ONNX ماڈل فائلیں صحیح ڈائریکٹری میں ہیں۔
> - آپ کے پاس کافی RAM دستیاب ہے (Qwen3-0.6B int4 کو تقریباً 1 GB درکار ہوتا ہے)۔
> - Qwen3 ایک استدلال ماڈل ہے جو `<think>` ٹیگز پیدا کرتا ہے۔ اگر آپ جوابات کے شروع میں `<think>...</think>` دیکھتے ہیں تو یہ معمول کا رویہ ہے۔ `inference_model.json` میں پرامپٹ ٹیمپلیٹ کو اس طرح ایڈجسٹ کیا جا سکتا ہے کہ سوچنے کا آؤٹ پٹ روکا جائے۔

---

### مشق 8: REST API کے ذریعے کسٹم ماڈل سے پوچھ گچھ کریں

اگر آپ نے مشق 7 میں انٹرایکٹو سیشن سے باہر نکل گئے ہیں تو ماڈل ممکن ہے اب لوڈ نہ ہو۔ Foundry Local سروس کو شروع کریں اور پہلے ماڈل لوڈ کریں:

```bash
foundry service start
foundry model load qwen3-0.6b
```

چیک کریں کہ سروس کس پورٹ پر چل رہی ہے:

```bash
foundry service status
```

پھر درخواست بھیجیں (اگر پورٹ مختلف ہے تو `5273` کو اپنی اصل پورٹ نمبر سے تبدیل کریں):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **ونڈوز نوٹ:** اوپر دیا گیا `curl` کمانڈ بش سینٹیکس استعمال کرتا ہے۔ ونڈوز پر اس کے بجائے PowerShell کا `Invoke-RestMethod` cmdlet استعمال کریں۔

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

### مشق 9: OpenAI SDK کے ساتھ کسٹم ماڈل استعمال کریں

آپ اپنے کسٹم ماڈل کو بالکل انہی OpenAI SDK کوڈ کے ذریعے جو بلٹ ان ماڈلز کے لیے استعمال کرتے ہیں، کنیکٹ کر سکتے ہیں (دیکھیں [حصہ 3](part3-sdk-and-apis.md))۔ فرق صرف ماڈل کے نام کا ہے۔

<details>
<summary><b>پائتھن</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # فانڈری لوکل API کیز کی تصدیق نہیں کرتا
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
<summary><b>جاوا اسکرپٹ</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // فاؤنڈری لوکل API کیز کی تصدیق نہیں کرتا
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

> **اہم نکتہ:** چونکہ Foundry Local ایک OpenAI-مطابق API فراہم کرتا ہے، جو بھی کوڈ بلٹ ان ماڈلز کے ساتھ کام کرتا ہے، وہی کوڈ آپ کے کسٹم ماڈلز کے ساتھ بھی کام کرے گا۔ آپ کو صرف `model` پیرا میٹر تبدیل کرنا ہوگا۔

---

### مشق 10: Foundry Local SDK کے ساتھ کسٹم ماڈل کی جانچ کریں

پہلے کی لیبز میں آپ نے Foundry Local SDK کا استعمال کرتے ہوئے سروس شروع کی، اینڈپوائنٹ دریافت کیا اور ماڈلز کو خودکار طریقے سے منظم کیا۔ آپ اپنے کسٹم-مرتب شدہ ماڈل کے ساتھ بالکل اسی اصول پر عمل کر سکتے ہیں۔ SDK سروس کی شروعات اور اینڈپوائنٹ دریافت خودکار طریقے سے کرتا ہے، لہٰذا آپ کے کوڈ کو `localhost:5273` کو ہارڈ کوڈ کرنے کی ضرورت نہیں۔

> **نوٹ:** ان مثالوں کو چلانے سے پہلے یقین کریں کہ Foundry Local SDK انسٹال ہے:
> - **پائتھن:** `pip install foundry-local openai`
> - **جاوا اسکرپٹ:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` اور `OpenAI` NuGet پیکیجز شامل کریں
>
> ہر اسکرپٹ فائل کو **ریپوزیٹری کی روٹ ڈائریکٹری میں** محفوظ کریں (وہی ڈائریکٹری جہاں آپ کا `models/` فولڈر ہے)۔

<details>
<summary><b>پائتھن</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# مرحلہ 1: فاؤنڈری لوکل سروس شروع کریں اور حسب ضرورت ماڈل لوڈ کریں
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# مرحلہ 2: حسب ضرورت ماڈل کے لیے کیش چیک کریں
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# مرحلہ 3: ماڈل کو میموری میں لوڈ کریں
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# مرحلہ 4: SDK سے معلوم شدہ اینڈپوائنٹ کا استعمال کرتے ہوئے OpenAI کلائنٹ بنائیں
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# مرحلہ 5: اسٹریم کرنے والی چیٹ تکمیل کی درخواست بھیجیں
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

چلائیں:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>جاوا اسکرپٹ</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// مرحلہ 1: فاؤنڈری لوکل سروس شروع کریں
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// مرحلہ 2: کیٹلاگ سے کسٹم ماڈل حاصل کریں
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// مرحلہ 3: ماڈل کو میموری میں لوڈ کریں
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// مرحلہ 4: SDK سے معلوم کردہ اینڈپوائنٹ استعمال کرتے ہوئے OpenAI کلائنٹ بنائیں
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// مرحلہ 5: اسٹریمنگ چیٹ کمپلیشن کی درخواست بھیجیں
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

چلائیں:

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

> **اہم نکتہ:** Foundry Local SDK اینڈپوائنٹ خودکار طور پر دریافت کرتا ہے، لہٰذا آپ کبھی بھی پورٹ نمبر ہارڈ کوڈ نہ کریں۔ یہ پیداواری ایپلیکیشنز کے لیے تجویز کردہ طریقہ ہے۔ آپ کا کسٹم-مرتب شدہ ماڈل SDK کے ذریعے بلٹ ان کیٹلاگ ماڈلز کی طرح کام کرتا ہے۔

---

## ماڈل کا انتخاب برائے کمپائلیشن

اس لیب میں بطور حوالہ Qwen3-0.6B استعمال کیا گیا ہے کیونکہ یہ چھوٹا ہے، تیزی سے کمپائل ہوتا ہے، اور Apache 2.0 لائسنس کے تحت مفت دستیاب ہے۔ تاہم، آپ بہت سے دوسرے ماڈلز کمپائل کر سکتے ہیں۔ یہاں کچھ تجاویز دی گئی ہیں:

| ماڈل | Hugging Face ID | پیرا میٹرز | لائسنس | نوٹس |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | بہت چھوٹا، تیز کمپائلیشن، جانچ کے لیے اچھا |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | بہتر معیار، اب بھی تیزی سے کمپائل ہوتا ہے |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | اعلی معیار، زیادہ RAM درکار ہے |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face پر لائسنس کی منظوری ضروری ہے |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | اعلی معیار، بڑی ڈاؤن لوڈ اور طویل کمپائلیشن |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | پہلے سے Foundry Local کیٹلاگ میں شامل (موازنہ کے لیے مفید) |

> **لائسنس کی یاد دہانی:** استعمال سے پہلے ہمیشہ Hugging Face پر ماڈل کے لائسنس کو چیک کریں۔ کچھ ماڈلز (جیسے Llama) کے لیے لائسنس معاہدہ قبول کرنا اور `huggingface-cli login` کے ذریعے تصدیق کرنا ضروری ہوتا ہے۔

---

## تصورات: کب کسٹم ماڈلز استعمال کریں

| منظر نامہ | اپنا ماڈل کمپائل کیوں کریں؟ |
|----------|----------------------------|
| **ایک ماڈل کیٹلاگ میں نہیں ہے** | Foundry Local کیٹلاگ مفصل ہے۔ اگر آپ کا مطلوبہ ماڈل موجود نہیں تو خود کمپائل کریں۔ |
| **فائن ٹون شدہ ماڈلز** | اگر آپ نے کوئی ماڈل مخصوص ڈومین کے ڈیٹا پر فائن ٹون کیا ہے تو اپنے وزن خود کمپائل کریں۔ |
| **خاص کوانٹائزیشن کی ضرورت** | آپ کو ایسا پرسیژن یا کوانٹائزیشن حکمت عملی چاہیے جو کیٹلاگ سے مختلف ہو۔ |
| **نئے ماڈل ریلیز** | جب Hugging Face پر نیا ماڈل آتا ہے، وہ فوراً Foundry Local کیٹلاگ میں نہیں ہوتا۔ خود کمپائل کرنے سے فوراً رسائی ملتی ہے۔ |
| **تحقیقات اور تجربات** | پیداوار کا انتخاب کرنے سے پہلے مختلف ماڈل آرکیٹیکچر، سائز یا کنفیگریشن مقامی طور پر آزمانا۔ |

---

## خلاصہ

اس لیب میں آپ نے سیکھا کہ کس طرح:

| مرحلہ | آپ نے کیا کیا |
|-------|---------------|
| 1 | ONNX Runtime GenAI ماڈل بلڈر انسٹال کیا |
| 2 | `Qwen/Qwen3-0.6B` کو Hugging Face سے لے کر ONNX ماڈل میں کمپائل کیا |
| 3 | `inference_model.json` چیٹ ٹیمپلیٹ کنفیگریشن فائل بنائی |
| 4 | کمپائل کردہ ماڈل کو Foundry Local کیش میں شامل کیا |
| 5 | CLI کے ذریعے کسٹم ماڈل کے ساتھ انٹرایکٹو چیٹ چلائی |
| 6 | OpenAI-مطابق REST API کے ذریعے ماڈل کو پوچھ گچھ کی |
| 7 | Python، JavaScript اور C# سے OpenAI SDK کے ذریعے کنیکٹ کیا |
| 8 | Foundry Local SDK کے ساتھ کسٹم ماڈل کا آخری مرحلہ تک ٹیسٹ کیا |

اہم نکتہ یہ ہے کہ **کوئی بھی ٹرانسفارمر بیسڈ ماڈل ONNX فارمیٹ میں کمپائل ہونے کے بعد Foundry Local پر چلایا جا سکتا ہے۔** OpenAI-مطابق API کا مطلب ہے کہ آپ کے موجودہ اپلیکیشن کوڈ میں کوئی تبدیلی نہیں کرنی پڑے گی؛ آپ کو صرف ماڈل کا نام تبدیل کرنا ہے۔

---

## اہم نکات

| تصور | تفصیل |
|--------|---------|
| ONNX Runtime GenAI ماڈل بلڈر | Hugging Face ماڈلز کو ایک کمانڈ میں ONNX فارمیٹ میں کوانٹائز کر کے تبدیل کرتا ہے |
| ONNX فارمیٹ | Foundry Local ONNX Runtime GenAI کنفیگریشن کے ساتھ ONNX ماڈلز کا تقاضا کرتا ہے |
| چیٹ ٹیمپلیٹس | `inference_model.json` فائل بتاتی ہے کہ دیے گئے ماڈل کے لیے prompts کیسے بنائیں |
| ہارڈ ویئر کی حدف | آپ کے ہارڈ ویئر کے مطابق CPU، NVIDIA GPU (CUDA)، DirectML (Windows GPU)، یا WebGPU کے لیے کمپائل کریں |
| کوانٹائزیشن | کم پرسیژن (int4) سائز کم کرتا اور رفتار بڑھاتا ہے مگر کچھ درستگی کی قیمت پر؛ fp16 GPU پر اعلی معیار رکھتا ہے |
| API مطابقت | کسٹم ماڈلز وہی OpenAI-مطابق API استعمال کرتے ہیں جو بلٹ ان ماڈلز کے لیے ہوتا ہے |
| Foundry Local SDK | SDK خودکار طور پر سروس شروع کرتا ہے، اینڈپوائنٹ دریافت کرتا ہے، اور کیٹلاگ و کسٹم ماڈلز دونوں کی لوڈنگ سنبھالتا ہے |

---

## مزید مطالعہ

| ذریعہ | لنک |
|--------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local کسٹم ماڈل گائیڈ | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 ماڈل فیملی | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive دستاویزات | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## اگلے اقدامات

[حصہ 11: لوکل ماڈلز کے ساتھ ٹول کالنگ](part11-tool-calling.md) پر جائیں تاکہ سیکھیں کہ آپ اپنے مقامی ماڈلز کو بیرونی فنکشن کال کرنے کے قابل کیسے بنا سکتے ہیں۔

[← حصہ 9: وِسپَر وائس ٹرانسکرپشن](part9-whisper-voice-transcription.md) | [حصہ 11: ٹول کالنگ →](part11-tool-calling.md)