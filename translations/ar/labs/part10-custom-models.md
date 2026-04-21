![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# الجزء 10: استخدام نماذج مخصصة أو نماذج Hugging Face مع Foundry Local

> **الهدف:** تجميع نموذج Hugging Face إلى تنسيق ONNX المحسن الذي يتطلبه Foundry Local، تكوينه بقالب محادثة، إضافته إلى التخزين المحلي المؤقت، وتشغيل الاستدلال عليه باستخدام CLI و REST API و OpenAI SDK.

## نظرة عامة

يأتي Foundry Local مع كتالوج من النماذج المجمعة مسبقاً بعناية، ولكنك لست مقيداً بهذه القائمة فقط. يمكن لأي نموذج لغوي يعتمد على المحولات ومتوافر على [Hugging Face](https://huggingface.co/) (أو مخزن محليًا بتنسيق PyTorch / Safetensors) أن يُجمّع إلى نموذج ONNX محسن ويُقدم عبر Foundry Local.

تستخدم سلسلة التجميع أداة **ONNX Runtime GenAI Model Builder**، وهي أداة سطر أوامر مضمّنة مع حزمة `onnxruntime-genai`. تتولى أداة إنشاء النماذج المهام الصعبة: تنزيل أوزان المصدر، تحويلها إلى تنسيق ONNX، تطبيق التكميم (int4، fp16، bf16)، وإصدار ملفات التكوين (بما في ذلك قالب المحادثة والمجزئ) الذي يتوقعه Foundry Local.

في هذا المختبر، ستقوم بتجميع نموذج **Qwen/Qwen3-0.6B** من Hugging Face، تسجيله مع Foundry Local، والتحدث معه بالكامل على جهازك.

---

## أهداف التعلم

في نهاية هذا المختبر، ستكون قادرًا على:

- شرح سبب فائدة تجميع النماذج المخصصة ومتى قد تحتاج إليها  
- تثبيت ONNX Runtime GenAI model builder  
- تجميع نموذج Hugging Face إلى تنسيق ONNX محسن بأمر واحد  
- فهم معلمات التجميع الرئيسية (مقدم التنفيذ، الدقة)  
- إنشاء ملف تكوين قالب المحادثة `inference_model.json`  
- إضافة نموذج مجمّع إلى ذاكرة Foundry Local المؤقتة  
- تشغيل الاستدلال على النموذج المخصص باستخدام CLI و REST API و OpenAI SDK  

---

## المتطلبات الأساسية

| المتطلب | التفاصيل |
|-------------|---------|
| **Foundry Local CLI** | مثبت وموجود في `PATH` الخاص بك ([الجزء 1](part1-getting-started.md)) |
| **Python 3.10+** | مطلوب بواسطة ONNX Runtime GenAI model builder |
| **pip** | مدير حزم بايثون |
| **مساحة تخزين** | على الأقل 5 جيجابايت خالية لملفات المصدر والنموذج المجمع |
| **حساب Hugging Face** | بعض النماذج تتطلب قبول رخصة قبل التحميل. يستخدم Qwen3-0.6B ترخيص Apache 2.0 وهو متاح مجانًا. |

---

## إعداد البيئة

يتطلب تجميع النماذج عدة حزم بايثون ضخمة (PyTorch، ONNX Runtime GenAI، Transformers). أنشئ بيئة افتراضية مخصصة حتى لا تتداخل هذه الحزم مع بايثون النظام أو مشاريع أخرى.

```bash
# من جذر المستودع
python -m venv .venv
```
  
فعل البيئة:

**Windows (PowerShell):**  
```powershell
.venv\Scripts\Activate.ps1
```
  
**macOS / Linux:**  
```bash
source .venv/bin/activate
```
  
قم بترقية pip لتجنب مشكلات حل التبعيات:

```bash
python -m pip install --upgrade pip
```
  
> **نصيحة:** إذا كان لديك بالفعل `.venv` من المختبرات السابقة، يمكنك إعادة استخدامه. فقط تأكد من تفعيله قبل المتابعة.

---

## المفهوم: سلسلة تجميع النماذج

يتطلب Foundry Local نماذج بتنسيق ONNX مع تكوين ONNX Runtime GenAI. معظم النماذج مفتوحة المصدر على Hugging Face موزعة كأوزان PyTorch أو Safetensors، لذا هناك حاجة لخطوة تحويل.

![سلسلة تجميع نموذج مخصص](../../../images/custom-model-pipeline.svg)

### ماذا يفعل Model Builder؟

1. **ينزل** النموذج المصدر من Hugging Face (أو يقرأه من مسار محلي).  
2. **يحوّل** أوزان PyTorch / Safetensors إلى تنسيق ONNX.  
3. **يكمّم** النموذج إلى دقة أصغر (مثلاً int4) لتقليل استخدام الذاكرة وتحسين الأداء.  
4. **يصدر** تكوين ONNX Runtime GenAI (`genai_config.json`)، قالب المحادثة (`chat_template.jinja`)، وجميع ملفات المجزئ ليتمكن Foundry Local من تحميل النموذج وتقديمه.

### ONNX Runtime GenAI Model Builder مقابل Microsoft Olive

قد تلتقي بإشارات إلى **Microsoft Olive** كأداة بديلة لتحسين النماذج. كلا الأدوات يمكنها إنتاج نماذج ONNX لكنها تخدم أغراضًا مختلفة وتملك مزايا مختلفة:

|  | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **الحزمة** | `onnxruntime-genai` | `olive-ai` |
| **الغرض الأساسي** | تحويل وتكميم نماذج الذكاء الاصطناعي التوليدي لاستدلال ONNX Runtime GenAI | إطار عمل لتحسين النماذج من النهاية إلى النهاية بدعم العديد من الخلفيات والأجهزة |
| **سهولة الاستخدام** | أمر واحد — تحويل وتكميم خطوة واحدة | قائم على سير العمل — أنابيب متعددة التمريرات قابلة للتعديل مع YAML/JSON |
| **تنسيق الإخراج** | ONNX Runtime GenAI (جاهز لـ Foundry Local) | ONNX عام، ONNX Runtime GenAI، أو تنسيقات أخرى حسب سير العمل |
| **الأجهزة المستهدفة** | CPU، CUDA، DirectML، TensorRT RTX، WebGPU | CPU، CUDA، DirectML، TensorRT، Qualcomm QNN، وأكثر |
| **خيارات التكميم** | int4، int8، fp16، fp32 | int4 (AWQ، GPTQ، RTN)، int8، fp16، بالإضافة إلى تحسينات الرسم البياني، ضبط الطبقات |
| **نطاق النموذج** | نماذج الذكاء الاصطناعي التوليدي (LLMs، SLMs) | أي نموذج قابل للتحويل إلى ONNX (رؤية، معالجة اللغة الطبيعية، صوت، متعدد الوسائط) |
| **الأفضل لـ** | تجميع نموذج واحد سريع لاستدلال محلي | خطوط إنتاج تحتاج تحكمًا دقيقًا في التحسين |
| **بصمة التبعيات** | متوسطة (PyTorch، Transformers، ONNX Runtime) | أكبر (يضيف إطار Olive، إضافات اختيارية حسب سير العمل) |
| **تكامل Foundry Local** | مباشر — الإخراج متوافق فورًا | يتطلب علم `--use_ort_genai` وتكوين إضافي |

> **لماذا يستخدم هذا المختبر Model Builder:** لمهمة تجميع نموذج Hugging Face واحد وتسجيله مع Foundry Local، يعد Model Builder أبسط وأوثق مسار. ينتج تنسيق الإخراج الدقيق الذي يتوقعه Foundry Local بأمر واحد. إذا احتجت لاحقًا إلى ميزات تحسين متقدمة — مثل التكميم المرتكز على الدقة، تعديل الرسم البياني، أو ضبط متعدد التمرير — يعتبر Olive خيارًا قويًا للاستكشاف. راجع [توثيق Microsoft Olive](https://microsoft.github.io/Olive/) لمزيد من التفاصيل.

---

## تمارين المختبر

### التمرين 1: تثبيت ONNX Runtime GenAI Model Builder

قم بتثبيت حزمة ONNX Runtime GenAI التي تتضمن أداة إنشاء النموذج:

```bash
pip install onnxruntime-genai
```
  
تحقق من التثبيت عبر التأكد من توفر أداة إنشاء النموذج:

```bash
python -m onnxruntime_genai.models.builder --help
```
  
يجب أن ترى مخرجات المساعدة التي تستعرض معلمات مثل `-m` (اسم النموذج)، `-o` (مسار الإخراج)، `-p` (الدقة)، و `-e` (مقدم التنفيذ).

> **ملاحظة:** تعتمد أداة الإنشاء على PyTorch و Transformers وعدد من الحزم الأخرى. قد يستغرق التثبيت بضع دقائق.

---

### التمرين 2: تجميع Qwen3-0.6B لوحدة المعالجة المركزية CPU

شغّل الأمر التالي لتنزيل نموذج Qwen3-0.6B من Hugging Face وتجميعه لاستدلال CPU مع تكميم int4:

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
  
#### ماذا يفعل كل معلم؟

| المعلم | الغرض | القيمة المستخدمة |
|-----------|---------|------------|
| `-m` | معرف نموذج Hugging Face أو مسار مجلد محلي | `Qwen/Qwen3-0.6B` |
| `-o` | المجلد حيث سيتم حفظ نموذج ONNX المجمع | `models/qwen3` |
| `-p` | دقة التكميم المطبقة أثناء التجميع | `int4` |
| `-e` | مقدم تنفيذ ONNX Runtime (الأجهزة المستهدفة) | `cpu` |
| `--extra_options hf_token=false` | يتخطى المصادقة على Hugging Face (مناسب للنماذج العامة) | `hf_token=false` |

> **كم يستغرق هذا؟** وقت التجميع يعتمد على العتاد وحجم النموذج. لنموذج Qwen3-0.6B مع تكميم int4 على معالج حديث، توقع من 5 إلى 15 دقيقة تقريبًا. النماذج الأكبر تستغرق وقتًا أطول تناسبياً.

عند اكتمال الأمر، يجب أن ترى مجلد `models/qwen3` يحتوي على ملفات النموذج المجمع. تحقق من الإخراج:

```bash
ls models/qwen3
```
  
يجب أن ترى ملفات تشمل:  
- `model.onnx` و `model.onnx.data` — أوزان النموذج المجمعة  
- `genai_config.json` — تكوين ONNX Runtime GenAI  
- `chat_template.jinja` — قالب المحادثة الخاص بالنموذج (تم إنشاؤه أوتوماتيكيًا)  
- `tokenizer.json`، `tokenizer_config.json` — ملفات المجزئ  
- ملفات أخرى خاصة بالمفردات والتكوين

---

### التمرين 3: تجميع لوحدة معالجة الرسوميات GPU (اختياري)

إذا كان لديك وحدة معالجة رسوميات NVIDIA مع دعم CUDA، يمكنك تجميع نسخة محسنة للـ GPU لاستدلال أسرع:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```
  
> **ملاحظة:** يتطلب تجميع GPU وجود `onnxruntime-gpu` وتثبيت CUDA صحيح. إذا لم تتوفر هذه، ستظهر أداة الإنشاء خطأ. يمكنك تخطي هذا التمرين ومتابعة نسخة CPU.

#### مرجع التجميع حسب العتاد

| الهدف | مقدم التنفيذ (`-e`) | الدقة الموصى بها (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| بطاقة NVIDIA GPU | `cuda` | `fp16` أو `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` أو `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### تTrade-offs في الدقة

| الدقة | الحجم | السرعة | الجودة |
|-----------|------|-------|---------|
| `fp32` | الأكبر | الأبطأ | أعلى دقة |
| `fp16` | كبير | سريع (GPU) | دقة جيدة جداً |
| `int8` | صغير | سريع | فقدان طفيف في الدقة |
| `int4` | الأصغر | الأسرع | فقدان متوسط في الدقة |

لأغلب تطويرات المحلية، يوفر `int4` على CPU أفضل توازن بين السرعة واستخدام الموارد. للإخراج بجودة الإنتاج، يوصى بـ `fp16` على GPU CUDA.

---

### التمرين 4: إنشاء تكوين قالب المحادثة

يقوم Model Builder تلقائيًا بإنشاء ملف `chat_template.jinja` وملف `genai_config.json` في مجلد الإخراج. لكن Foundry Local يحتاج أيضًا إلى ملف `inference_model.json` لفهم كيفية تنسيق المطالبات لنموذجك. يعرّف هذا الملف اسم النموذج وقالب المطالبة الذي يحيط برسائل المستخدم بالتوكنات الخاصة الصحيحة.

#### الخطوة 1: تفقد المخرجات المجمعة

اعرض محتويات مجلد النموذج المجمّع:

```bash
ls models/qwen3
```
  
يجب أن ترى ملفات مثل:  
- `model.onnx` و `model.onnx.data` — أوزان النموذج المجمعة  
- `genai_config.json` — تكوين ONNX Runtime GenAI (تم إنشاؤه أوتوماتيكيًا)  
- `chat_template.jinja` — قالب المحادثة الخاص بالنموذج (تم إنشاؤه أوتوماتيكيًا)  
- `tokenizer.json`، `tokenizer_config.json` — ملفات المجزئ  
- ملفات أخرى مختلفة للتكوين والمفردات

#### الخطوة 2: إنشاء ملف inference_model.json

يرشد ملف `inference_model.json` Foundry Local حول كيفية تنسيق المطالبات. أنشئ سكربت بايثون يسمى `generate_chat_template.py` **في جذر المستودع** (نفس المجلد الذي يحتوى على مجلد `models/`):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# بناء محادثة بسيطة لاستخراج نموذج الدردشة
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

# بناء هيكل inference_model.json
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
  
شغل السكربت من جذر المستودع:

```bash
python generate_chat_template.py
```
  
> **ملاحظة:** تم تثبيت حزمة `transformers` مسبقًا كاعتماد لـ `onnxruntime-genai`. إذا ظهرت لك رسالة `ImportError`، شغّل أولًا `pip install transformers`.

ينتج السكربت ملف `inference_model.json` داخل مجلد `models/qwen3`. يخبر الملف Foundry Local بكيفية إحاطة مدخلات المستخدم بالتوكنات الخاصة الصحيحة لـ Qwen3.

> **مهم:** الحقل `"Name"` في `inference_model.json` (محدد إلى `qwen3-0.6b` في هذا السكربت) هو **الاسم المستعار للنموذج** الذي ستستخدمه في جميع الأوامر واستدعاءات API اللاحقة. إذا غيرت هذا الاسم، حدث اسم النموذج في تمارين 6–10 وفقًا لذلك.

#### الخطوة 3: التحقق من التكوين

افتح `models/qwen3/inference_model.json` وتأكد أنه يحتوي على حقل `Name` وكائن `PromptTemplate` يحوي مفاتيح `assistant` و `prompt`. يجب أن يتضمن قالب المطالبة توكنات خاصة مثل `<|im_start|>` و `<|im_end|>` (تعتمد التوكنات الدقيقة على قالب المحادثة للنموذج).

> **بديل يدوي:** إذا فضّلت عدم إطلاق السكربت، يمكنك إنشاء الملف يدويًا. الشرط الأساسي هو أن يحتوي حقل `prompt` على قالب المحادثة الكامل للنموذج مع `{Content}` كعنصر نائب لرسالة المستخدم.

---

### التمرين 5: التحقق من هيكل مجلد النموذج


يقوم منشئ النموذج بوضع جميع الملفات المجمعة مباشرة في الدليل الناتج الذي حددته. تحقق من أن الهيكل النهائي يبدو صحيحًا:

```bash
ls models/qwen3
```

يجب أن يحتوي الدليل على الملفات التالية:

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

> **ملاحظة:** على عكس بعض أدوات التجميع الأخرى، لا يقوم منشئ النموذج بإنشاء مجلدات فرعية متداخلة. جميع الملفات تقع مباشرة في مجلد الإخراج، وهذا بالضبط ما يتوقعه Foundry Local.

---

### التمرين 6: إضافة النموذج إلى ذاكرة Foundry Local التخزينية المؤقتة

أخبر Foundry Local أين يجد نموذجك المجمّع عن طريق إضافة الدليل إلى ذاكرته المؤقتة:

```bash
foundry cache cd models/qwen3
```

تحقق من ظهور النموذج في الذاكرة المؤقتة:

```bash
foundry cache ls
```

يجب أن ترى نموذجك المخصص مدرجًا بجانب أي نماذج مخزنة سابقًا (مثل `phi-3.5-mini` أو `phi-4-mini`).

---

### التمرين 7: تشغيل النموذج المخصص باستخدام سطر الأوامر CLI

ابدأ جلسة دردشة تفاعلية مع نموذجك الذي تم تجميعه حديثًا (الاسم المستعار `qwen3-0.6b` يأتي من حقل `Name` الذي قمت بتحديده في `inference_model.json`):

```bash
foundry model run qwen3-0.6b --verbose
```

تعرض العلامة `--verbose` معلومات تشخيص إضافية، وهذا مفيد عند اختبار نموذج مخصص لأول مرة. إذا تم تحميل النموذج بنجاح، سترى موجهًا تفاعليًا. جرب بعض الرسائل:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

اكتب `exit` أو اضغط `Ctrl+C` لإنهاء الجلسة.

> **استكشاف الأخطاء وإصلاحها:** إذا فشل النموذج في التحميل، تحقق من ما يلي:
> - تم إنشاء ملف `genai_config.json` بواسطة منشئ النموذج.
> - ملف `inference_model.json` موجود وصحيح بتنسيق JSON.
> - ملفات نموذج ONNX موجودة في الدليل الصحيح.
> - لديك ذاكرة وصول عشوائي كافية (يحتاج Qwen3-0.6B int4 إلى حوالي 1 جيجابايت).
> - Qwen3 هو نموذج استدلال ينتج علامات `<think>`. إذا رأيت `<think>...</think>` مسبوقة على الردود، فهذا سلوك طبيعي. يمكن تعديل قالب الموجه في `inference_model.json` لقمع ناتج التفكير.

---

### التمرين 8: الاستعلام من النموذج المخصص عبر API REST

إذا خرجت من الجلسة التفاعلية في التمرين 7، فقد لا يتم تحميل النموذج بعد الآن. ابدأ خدمة Foundry Local وقم بتحميل النموذج أولاً:

```bash
foundry service start
foundry model load qwen3-0.6b
```

تحقق من المنفذ الذي تعمل عليه الخدمة:

```bash
foundry service status
```

ثم أرسل طلبًا (استبدل `5273` بالمنفذ الفعلي لديك إذا كان مختلفًا):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **ملاحظة ويندوز:** يستخدم الأمر `curl` أعلاه بناء جملة bash. على ويندوز، استخدم الأمر PowerShell `Invoke-RestMethod` أدناه بدلاً منه.

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

### التمرين 9: استخدام النموذج المخصص مع OpenAI SDK

يمكنك الاتصال بنموذجك المخصص باستخدام نفس كود OpenAI SDK الذي استخدمته مع النماذج المدمجة (انظر [الجزء 3](part3-sdk-and-apis.md)). الاختلاف الوحيد هو اسم النموذج.

<details>
<summary><b>بايثون</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # لا يقوم Foundry Local بالتحقق من صحة مفاتيح API
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
<summary><b>جافا سكريبت</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // لا يقوم Foundry Local بالتحقق من صحة مفاتيح API
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
<summary><b>سي#</b></summary>

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

> **نقطة رئيسية:** لأن Foundry Local يكشف عن API متوافق مع OpenAI، أي كود يعمل مع النماذج المدمجة يعمل كذلك مع نماذجك المخصصة. فقط تحتاج لتغيير معامل `model`.

---

### التمرين 10: اختبار النموذج المخصص مع Foundry Local SDK

في المختبرات السابقة استخدمت Foundry Local SDK لبدء الخدمة واكتشاف نقطة النهاية وإدارة النماذج تلقائيًا. يمكنك اتباع نفس النمط تمامًا مع نموذجك المجمّع مخصصًا. يتولى SDK بدء الخدمة واكتشاف نقطة النهاية، لذلك لا تحتاج إلى تحديد `localhost:5273` بشكل ثابت في كودك.

> **ملاحظة:** تأكد من تثبيت Foundry Local SDK قبل تشغيل هذه الأمثلة:
> - **بايثون:** `pip install foundry-local openai`
> - **جافا سكريبت:** `npm install foundry-local-sdk openai`
> - **سي#:** أضف حزم NuGet `Microsoft.AI.Foundry.Local` و `OpenAI`
>
> احفظ كل ملف سكريبت **في جذر المستودع** (نفس الدليل الذي فيه مجلد `models/`).

<details>
<summary><b>بايثون</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# الخطوة 1: ابدأ خدمة Foundry Local وحمّل النموذج المخصص
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# الخطوة 2: تحقق من ذاكرة التخزين المؤقت للنموذج المخصص
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# الخطوة 3: حمّل النموذج في الذاكرة
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# الخطوة 4: أنشئ عميل OpenAI باستخدام نقطة النهاية التي يكتشفها SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# الخطوة 5: أرسل طلب إكمال دردشة متدفق
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

شغلها:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>جافا سكريبت</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// الخطوة 1: ابدأ خدمة Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// الخطوة 2: احصل على النموذج المخصص من الكتالوج
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// الخطوة 3: حمّل النموذج في الذاكرة
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// الخطوة 4: أنشئ عميل OpenAI باستخدام نقطة النهاية المكتشفة بواسطة SDK
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// الخطوة 5: أرسل طلب إكمال محادثة متدفقة
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

شغلها:

```bash
node foundry_sdk_custom_model.mjs
```

</details>

<details>
<summary><b>سي#</b></summary>

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

> **نقطة رئيسية:** يكتشف Foundry Local SDK نقطة النهاية ديناميكيًا، لذلك لا تضطر أبدًا لترميز رقم منفذ ثابت. هذه هي الطريقة الموصى بها للتطبيقات الإنتاجية. يعمل نموذجك المجمّع مخصصًا بنفس طريقة نماذج الكتالوج المدمجة من خلال SDK.

---

## اختيار نموذج للتجميع

يتم استخدام Qwen3-0.6B كمثال مرجعي في هذا المختبر لأنه صغير وسريع التجميع ومتاح مجانًا تحت رخصة Apache 2.0. ومع ذلك، يمكنك تجميع نماذج أخرى كثيرة. إليك بعض الاقتراحات:

| النموذج | معرف Hugging Face | المعاملات | الرخصة | ملاحظات |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | صغير جدًا، تجميع سريع، جيد للاختبار |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | جودة أفضل، لا يزال سريع التجميع |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | جودة قوية، يحتاج المزيد من الذاكرة |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | يتطلب قبول الرخصة على Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | جودة عالية، حجم تنزيل أكبر وتجميع أطول |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | موجود بالفعل في كتالوج Foundry Local (مفيد للمقارنة) |

> **تذكير الرخصة:** تأكد دائمًا من رخصة النموذج على Hugging Face قبل استخدامه. بعض النماذج (مثل Llama) تتطلب منك قبول اتفاقية ترخيص وتوثيق باستخدام `huggingface-cli login` قبل التنزيل.

---

## المفاهيم: متى تستخدم النماذج المخصصة

| السيناريو | لماذا تقوم بتجميع نموذجك الخاص؟ |
|----------|----------------------|
| **النموذج الذي تحتاجه غير موجود في الكتالوج** | كتالوج Foundry Local مختار بعناية. إذا لم يكن النموذج الذي تريده مدرجًا، قم بتجميعه بنفسك. |
| **نماذج مدربة تخصيصًا (Fine-tuned)** | إذا قمت بتخصيص نموذج على بيانات خاصة بمجال معين، تحتاج إلى تجميع الأوزان بنفسك. |
| **متطلبات محددة للتكميم** | قد ترغب بدقة أو استراتيجية تكميم تختلف عن الإعداد الافتراضي في الكتالوج. |
| **إصدارات نماذج أحدث** | عندما يصدر نموذج جديد على Hugging Face قد لا يكون موجودًا في كتالوج Foundry Local بعد. التجميع الذاتي يمنحك وصولًا فوريًا. |
| **البحث والتجارب** | تجربة معمارية نماذج مختلفة، أحجام أو تكوينات محليًا قبل اتخاذ قرار الإنتاج. |

---

## الملخص

في هذا المختبر تعلمت كيف:

| الخطوة | ما قمت به |
|------|-------------|
| 1 | تثبيت منشئ نموذج ONNX Runtime GenAI |
| 2 | تجميع `Qwen/Qwen3-0.6B` من Hugging Face إلى نموذج ONNX محسّن |
| 3 | إنشاء ملف تهيئة قالب الدردشة `inference_model.json` |
| 4 | إضافة النموذج المجمّع إلى ذاكرة Foundry Local المؤقتة |
| 5 | تشغيل دردشة تفاعلية مع النموذج المخصص عبر CLI |
| 6 | استعلام النموذج عبر API REST المتوافق مع OpenAI |
| 7 | الاتصال من بايثون وجافا سكريبت وسي# باستخدام OpenAI SDK |
| 8 | اختبار النموذج المخصص بشكل كامل باستخدام Foundry Local SDK |

النقطة الأساسية هي أن **أي نموذج معتمد على المحولات يمكن تشغيله عبر Foundry Local** بمجرد تجميعه بتنسيق ONNX. تعني واجهة برمجة التطبيقات المتوافقة مع OpenAI أن كل كود التطبيق الحالي يعمل بدون تغييرات؛ تحتاج فقط إلى استبدال اسم النموذج.

---

## النقاط الرئيسية

| المفهوم | التفاصيل |
|---------|--------|
| منشئ نموذج ONNX Runtime GenAI | يحول نماذج Hugging Face إلى تنسيق ONNX مع التكميم في أمر واحد |
| تنسيق ONNX | يتطلب Foundry Local نماذج ONNX مع تهيئة ONNX Runtime GenAI |
| قوالب الدردشة | يخبر ملف `inference_model.json` Foundry Local كيف يصيغ المطالبات لنموذج معين |
| أهداف الأجهزة | التجميع لوحدة المعالجة المركزية، بطاقة NVIDIA GPU (CUDA)، DirectML (GPU على الويندوز)، أو WebGPU حسب جهازك |
| التكميم | الدقة المنخفضة (int4) تقلل الحجم وتحسن السرعة مع بعض الخسارة في الدقة؛ fp16 تحافظ على جودة عالية على وحدات GPU |
| توافق API | تستخدم النماذج المخصصة نفس واجهة برمجة التطبيقات المتوافقة مع OpenAI كنماذج المدمجة |
| Foundry Local SDK | يتولى SDK بدء الخدمة، اكتشاف النقطة النهائية، وتحميل النماذج تلقائيًا لكل من النماذج الكتالوج والمخصصة |

---

## قراءة إضافية

| المورد | الرابط |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| دليل نموذج مخصص Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| عائلة نماذج Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| توثيق Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## الخطوات التالية

تابع إلى [الجزء 11: استدعاء الأدوات مع النماذج المحلية](part11-tool-calling.md) لتتعلم كيفية تمكين نماذجك المحلية من استدعاء الوظائف الخارجية.

[← الجزء 9: تفريغ الصوت باستخدام Whisper](part9-whisper-voice-transcription.md) | [الجزء 11: استدعاء الأدوات →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**إخلاء مسؤولية**:  
تمت ترجمة هذا المستند باستخدام خدمة الترجمة الآلية [Co-op Translator](https://github.com/Azure/co-op-translator). بينما نسعى جاهدين للدقة، يرجى العلم أن الترجمات الآلية قد تحتوي على أخطاء أو عدم دقة. يجب اعتبار المستند الأصلي بلغته الأصلية المصدر الرسمي والمعتمد. للمعلومات الهامة، يُنصح بالترجمة المهنية البشرية. نحن غير مسؤولين عن أي سوء فهم أو تفسير ناتج عن استخدام هذه الترجمة.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->