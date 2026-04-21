![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# بخش ۱۰: استفاده از مدل‌های سفارشی یا Hugging Face با Foundry Local

> **هدف:** کامپایل یک مدل Hugging Face به فرمت بهینه شده ONNX که Foundry Local نیاز دارد، پیکربندی آن با قالب چت، افزودن آن به کش محلی، و اجرای استنتاج با استفاده از CLI، REST API و OpenAI SDK.

## مرور کلی

Foundry Local با یک کاتالوگ curated از مدل‌های پیش‌کامپایل شده عرضه می‌شود، اما شما محدود به آن لیست نیستید. هر مدل زبانی مبتنی بر ترنسفورمر که در [Hugging Face](https://huggingface.co/) موجود باشد (یا به‌صورت محلی در فرمت PyTorch / Safetensors ذخیره شده باشد) می‌تواند به یک مدل ONNX بهینه شده کامپایل شده و از طریق Foundry Local سرو شود.

خط لوله کامپایل از **ONNX Runtime GenAI Model Builder** استفاده می‌کند، یک ابزار خط فرمان که در بسته `onnxruntime-genai` گنجانده شده است. مدل بیلدر کارهای سنگین را بر عهده دارد: دانلود وزن‌های منبع، تبدیل آن‌ها به فرمت ONNX، اعمال کوانتیزه کردن (int4، fp16، bf16) و تولید فایل‌های پیکربندی (شامل قالب چت و توکنایزر) که Foundry Local انتظار دارد.

در این آزمایشگاه شما مدل **Qwen/Qwen3-0.6B** را از Hugging Face کامپایل، با Foundry Local ثبت، و کاملاً روی دستگاه خود با آن چت خواهید کرد.

---

## اهداف یادگیری

تا پایان این آزمایشگاه قادر خواهید بود:

- توضیح دهید چرا کامپایل مدل سفارشی مفید است و چه زمانی ممکن است به آن نیاز داشته باشید
- نصب مدل بیلدر ONNX Runtime GenAI
- کامپایل یک مدل Hugging Face به فرمت بهینه شده ONNX با یک دستور واحد
- درک پارامترهای کلیدی کامپایل (اجرای فراهم‌کننده، دقت)
- ایجاد فایل پیکربندی قالب چت `inference_model.json`
- افزودن یک مدل کامپایل شده به کش Foundry Local
- اجرای استنتاج از مدل سفارشی با استفاده از CLI، REST API، و OpenAI SDK

---

## پیش‌نیازها

| الزام | جزئیات |
|-------------|---------|
| **Foundry Local CLI** | نصب شده و در مسیر `PATH` شما ([بخش ۱](part1-getting-started.md)) |
| **Python 3.10+** | مورد نیاز برای مدل بیلدر ONNX Runtime GenAI |
| **pip** | مدیر بسته پایتون |
| **فضای دیسک** | حداقل ۵ گیگابایت فضای آزاد برای فایل‌های مدل منبع و کامپایل شده |
| **حساب کاربری Hugging Face** | برخی مدل‌ها نیاز دارند قبل از دانلود لایسنس قبول شود. Qwen3-0.6B از لایسنس Apache 2.0 استفاده می‌کند و به‌صورت آزاد در دسترس است. |

---

## راه‌اندازی محیط

کامپایل مدل به چند بسته بزرگ پایتون (PyTorch، ONNX Runtime GenAI، Transformers) نیاز دارد. یک محیط مجازی جداگانه ایجاد کنید تا این‌ها با پایتون سیستم یا پروژه‌های دیگر شما تداخل نداشته باشند.

```bash
# از ریشه مخزن
python -m venv .venv
```

فعال‌سازی محیط:

**ویندوز (PowerShell):**  
```powershell
.venv\Scripts\Activate.ps1
```
  
**macOS / لینوکس:**  
```bash
source .venv/bin/activate
```
  
برای جلوگیری از مشکلات حل وابستگی، pip را به‌روز کنید:

```bash
python -m pip install --upgrade pip
```
  
> **نکته:** اگر قبلاً از آزمایشگاه‌های قبل `.venv` دارید، می‌توانید آن را دوباره استفاده کنید. فقط مطمئن شوید قبل از ادامه فعال باشد.

---

## مفهوم: خط لوله کامپایل

Foundry Local نیاز دارد مدل‌ها در فرمت ONNX با پیکربندی ONNX Runtime GenAI باشند. اکثر مدل‌های متن‌باز Hugging Face به صورت وزن‌های PyTorch یا Safetensors منتشر می‌شوند، بنابراین یک مرحله تبدیل لازم است.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### مدل بیلدر چه کاری انجام می‌دهد؟

۱. مدل منبع را از Hugging Face دانلود می‌کند (یا از مسیر محلی می‌خواند).  
۲. وزن‌های PyTorch / Safetensors را به فرمت ONNX تبدیل می‌کند.  
۳. مدل را به دقت کوچکتر (مثلاً int4) کوانتیزه می‌کند تا مصرف حافظه کاهش یافته و توان عملیاتی افزایش یابد.  
۴. پیکربندی ONNX Runtime GenAI (`genai_config.json`)، قالب چت (`chat_template.jinja`) و تمام فایل‌های توکنایزر را تولید می‌کند به طوری که Foundry Local بتواند مدل را بارگذاری و سرو کند.

### ONNX Runtime GenAI Model Builder در مقابل Microsoft Olive

ممکن است به **Microsoft Olive** به عنوان ابزاری جایگزین برای بهینه‌سازی مدل اشاره شود. هر دو ابزار می‌توانند مدل‌های ONNX تولید کنند ولی اهداف و مزایای متفاوتی دارند:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **بسته** | `onnxruntime-genai` | `olive-ai` |
| **هدف اصلی** | تبدیل و کوانتیزه کردن مدل‌های هوش مصنوعی مولد برای استنتاج ONNX Runtime GenAI | چارچوب بهینه‌سازی مدل انتها به انتها با پشتیبانی از بک‌اندها و سخت‌افزارهای متعدد |
| **سهولت استفاده** | دستور واحد—تبدیل و کوانتیزه کردن یک مرحله‌ای | مبتنی بر جریان کاری—خط‌لوله‌های چندمرحله‌ای قابل پیکربندی با YAML/JSON |
| **فرمت خروجی** | فرمت ONNX Runtime GenAI (آماده برای Foundry Local) | ONNX عمومی، ONNX Runtime GenAI یا سایر فرمت‌ها بسته به جریان کاری |
| **سخت‌افزار هدف** | CPU، CUDA، DirectML، TensorRT RTX، WebGPU | CPU، CUDA، DirectML، TensorRT، Qualcomm QNN و بیشتر |
| **گزینه‌های کوانتیزه کردن** | int4، int8، fp16، fp32 | int4 (AWQ، GPTQ، RTN)، int8، fp16، همچنین بهینه‌سازی گراف و تنظیم لایه‌ای |
| **دامنه مدل** | مدل‌های هوش مصنوعی مولد (LLMها، SLMها) | هر مدل قابل تبدیل به ONNX (بینایی، NLP، صوت، چندرسانه‌ای) |
| **بهترین کاربرد** | کامپایل سریع مدل منفرد برای استنتاج محلی | خط‌لوله‌های تولیدی نیازمند کنترل دقیق بهینه‌سازی |
| **ابعاد وابستگی** | متوسط (PyTorch، Transformers، ONNX Runtime) | بزرگ‌تر (افزودن چارچوب Olive و موارد اختیاری هر جریان کاری) |
| **ادغام با Foundry Local** | مستقیم — خروجی به‌سرعت قابل استفاده است | نیازمند فلگ `--use_ort_genai` و پیکربندی اضافی |

> **چرا این آزمایشگاه از مدل بیلدر استفاده می‌کند:** برای کار کامپایل یک مدل Hugging Face و ثبت آن با Foundry Local، مدل بیلدر ساده‌ترین و قابل اطمینان‌ترین مسیر است. خروجی دقیقاً فرمت مورد انتظار Foundry Local را با یک دستور تولید می‌کند. اگر بعداً نیاز به ویژگی‌های پیشرفته بهینه‌سازی مانند کوانتیزه کردن دقیق‌تر، جراحی گراف، یا تنظیم چندمرحله‌ای داشتید، Olive گزینه قدرتمندی برای بررسی است. برای جزئیات بیشتر به [مستندات Microsoft Olive](https://microsoft.github.io/Olive/) مراجعه کنید.

---

## تمرین‌های آزمایشگاه

### تمرین ۱: نصب مدل بیلدر ONNX Runtime GenAI

بسته ONNX Runtime GenAI که ابزار مدل بیلدر را شامل می‌شود نصب کنید:

```bash
pip install onnxruntime-genai
```
  
نصب را با بررسی در دسترس بودن مدل بیلدر تأیید کنید:

```bash
python -m onnxruntime_genai.models.builder --help
```
  
باید خروجی راهنمایی شامل پارامترهایی مانند `-m` (نام مدل)، `-o` (مسیر خروجی)، `-p` (دقت) و `-e` (فراهم‌کننده اجرایی) مشاهده کنید.

> **توجه:** مدل بیلدر به PyTorch، Transformers و چند بسته دیگر وابسته است. نصب ممکن است چند دقیقه طول بکشد.

---

### تمرین ۲: کامپایل Qwen3-0.6B برای CPU

این دستور را اجرا کنید تا مدل Qwen3-0.6B را از Hugging Face دانلود و برای استنتاج CPU با کوانتیزه کردن int4 کامپایل کند:

**macOS / لینوکس:**  
```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3 \
    -p int4 \
    -e cpu \
    --extra_options hf_token=false
```
  
**ویندوز (PowerShell):**  
```powershell
python -m onnxruntime_genai.models.builder `
    -m Qwen/Qwen3-0.6B `
    -o models/qwen3 `
    -p int4 `
    -e cpu `
    --extra_options hf_token=false
```
  
#### معنی هر پارامتر

| پارامتر | هدف | مقدار استفاده‌شده |
|-----------|---------|------------|
| `-m` | شناسه مدل Hugging Face یا مسیر دایرکتوری محلی | `Qwen/Qwen3-0.6B` |
| `-o` | دایرکتوری که مدل ONNX کامپایل شده در آن ذخیره می‌شود | `models/qwen3` |
| `-p` | دقت کوانتیزه‌شده اعمال‌شده در زمان کامپایل | `int4` |
| `-e` | فراهم‌کننده اجرای ONNX Runtime (سخت‌افزار هدف) | `cpu` |
| `--extra_options hf_token=false` | رد کردن احراز هویت Hugging Face (برای مدل‌های عمومی خوب است) | `hf_token=false` |

> **چه مدت طول می‌کشد؟** زمان کامپایل به سخت‌افزار و اندازه مدل بستگی دارد. برای Qwen3-0.6B با کوانتیزه کردن int4 روی یک CPU مدرن، حدود ۵ تا ۱۵ دقیقه طول می‌کشد. مدل‌های بزرگ‌تر زمان بیشتری نیاز دارند.

بعد از پایان دستور باید دایرکتوری `models/qwen3` شامل فایل‌های مدل کامپایل شده را مشاهده کنید. خروجی را بررسی کنید:

```bash
ls models/qwen3
```
  
باید فایل‌هایی مانند این‌ها را ببینید:  
- `model.onnx` و `model.onnx.data` — وزن‌های مدل کامپایل شده  
- `genai_config.json` — پیکربندی ONNX Runtime GenAI  
- `chat_template.jinja` — قالب چت مدل (تولید خودکار)  
- `tokenizer.json`، `tokenizer_config.json` — فایل‌های توکنایزر  
- سایر فایل‌های واژگان و پیکربندی

---

### تمرین ۳: کامپایل برای GPU (اختیاری)

اگر GPU انویدیا با پشتیبانی CUDA دارید، می‌توانید نسخه‌ای بهینه شده برای GPU را برای استنتاج سریع‌تر کامپایل کنید:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```
  
> **توجه:** کامپایل GPU نیازمند `onnxruntime-gpu` و نصب درست CUDA است. اگر این‌ها موجود نباشند، مدل بیلدر خطا گزارش می‌دهد. می‌توانید این تمرین را رد کرده و با نسخه CPU ادامه دهید.

#### مرجع کامپایل اختصاصی سخت‌افزار

| هدف | فراهم‌کننده اجرا (`-e`) | دقت پیشنهادی (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| GPU انویدیا | `cuda` | `fp16` یا `int4` |
| DirectML (ویندوز GPU) | `dml` | `fp16` یا `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### مزایا و معایب دقت‌ها

| دقت | اندازه | سرعت | کیفیت |
|-----------|------|-------|---------|
| `fp32` | بزرگ‌ترین | کندترین | بیشترین دقت |
| `fp16` | بزرگ | سریع (GPU) | دقت بسیار خوب |
| `int8` | کوچک | سریع | کمی کاهش دقت |
| `int4` | کوچک‌ترین | سریع‌ترین | کاهش دقت متوسط |

برای بیشتر توسعه‌های محلی، `int4` روی CPU بهترین تعادل سرعت و مصرف منابع را دارد. برای خروجی با کیفیت تولید، `fp16` روی GPU CUDA توصیه می‌شود.

---

### تمرین ۴: ایجاد پیکربندی قالب چت

مدل بیلدر به‌طور خودکار فایل `chat_template.jinja` و `genai_config.json` را در دایرکتوری خروجی تولید می‌کند. با این حال، Foundry Local همچنین به فایل `inference_model.json` نیاز دارد تا بفهمد چگونه پرامپت‌ها را برای مدل شما قالب‌بندی کند. این فایل نام مدل و قالب پرامپت را که پیام‌های کاربر را با توکن‌های خاص صحیح می‌پیچد تعریف می‌کند.

#### مرحله ۱: بررسی خروجی کامپایل شده

محتویات دایرکتوری مدل کامپایل شده را فهرست کنید:

```bash
ls models/qwen3
```
  
باید فایل‌هایی مانند زیر دیده شود:  
- `model.onnx` و `model.onnx.data` — وزن‌های مدل کامپایل شده  
- `genai_config.json` — پیکربندی ONNX Runtime GenAI (تولید خودکار)  
- `chat_template.jinja` — قالب چت مدل (تولید خودکار)  
- `tokenizer.json`، `tokenizer_config.json` — فایل‌های توکنایزر  
- دیگر فایل‌های پیکربندی و واژگان  

#### مرحله ۲: تولید فایل inference_model.json

فایل `inference_model.json` به Foundry Local می‌گوید چگونه پرامپت‌ها را قالب‌بندی کند. یک اسکریپت پایتون به نام `generate_chat_template.py` **در ریشه مخزن** (همان دایرکتوری حاوی پوشه `models/`) ایجاد کنید:

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# ساخت یک مکالمه حداقلی برای استخراج قالب گفتگو
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

# ساختار inference_model.json را بسازید
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
  
اسکریپت را از ریشه مخزن اجرا کنید:

```bash
python generate_chat_template.py
```
  
> **توجه:** بسته `transformers` قبلاً به عنوان وابستگی `onnxruntime-genai` نصب شده است. اگر خطای `ImportError` مشاهده کردید، ابتدا `pip install transformers` را اجرا کنید.

این اسکریپت یک فایل `inference_model.json` در دایرکتوری `models/qwen3` تولید می‌کند. این فایل به Foundry Local می‌گوید چگونه ورودی کاربر را با توکن‌های خاص صحیح برای Qwen3 بپیچد.

> **مهم:** فیلد `"Name"` در `inference_model.json` (که در این اسکریپت برابر با `qwen3-0.6b` تنظیم شده) **نام مستعار مدل** است که در تمام دستورها و فراخوانی‌های API بعدی استفاده می‌کنید. اگر این نام را تغییر دهید، نام مدل در تمرین‌های ۶ تا ۱۰ را نیز به‌روزرسانی کنید.

#### مرحله ۳: بررسی پیکربندی

فایل `models/qwen3/inference_model.json` را باز کنید و تأیید کنید شامل فیلد `Name` و شیء `PromptTemplate` با کلیدهای `assistant` و `prompt` باشد. قالب پرامپت باید توکن‌های خاصی مانند `<|im_start|>` و `<|im_end|>` را شامل شود (توکن‌های دقیق بستگی به قالب چت مدل دارد).

> **گزینه دستی:** اگر نمی‌خواهید اسکریپت را اجرا کنید، می‌توانید فایل را به‌صورت دستی ایجاد کنید. شرط اصلی این است که فیلد `prompt` شامل قالب کامل چت مدل با `{Content}` به عنوان محل جایگزین پیام کاربر باشد.

---

### تمرین ۵: بررسی ساختار دایرکتوری مدل


سازنده مدل همه فایل‌های کامپایل‌شده را مستقیماً در پوشه خروجی‌ای که مشخص کرده‌اید قرار می‌دهد. بررسی کنید ساختار نهایی درست به نظر برسد:

```bash
ls models/qwen3
```

این پوشه باید فایل‌های زیر را شامل شود:

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

> **توجه:** بر خلاف برخی ابزارهای کامپایل دیگر، سازنده مدل زیرپوشه‌های تو در تو ایجاد نمی‌کند. همه فایل‌ها مستقیماً در پوشه خروجی قرار دارند، که دقیقاً همان چیزی است که Foundry Local انتظار دارد.

---

### تمرین ۶: افزودن مدل به کش Foundry Local

به Foundry Local بگویید مدل کامپایل‌شده شما را از کجا پیدا کند، با افزودن پوشه به کش آن:

```bash
foundry cache cd models/qwen3
```

بررسی کنید که مدل در کش ظاهر شده است:

```bash
foundry cache ls
```

باید مدل سفارشی خود را در کنار هر مدل کش شده قبلی (مانند `phi-3.5-mini` یا `phi-4-mini`) مشاهده کنید.

---

### تمرین ۷: اجرای مدل سفارشی با CLI

یک جلسه چت تعاملی با مدل تازه کامپایل‌شده خود آغاز کنید (اسم مستعار `qwen3-0.6b` از فیلد `Name` در فایل `inference_model.json` آمده است):

```bash
foundry model run qwen3-0.6b --verbose
```

فلگ `--verbose` اطلاعات تشخیصی بیشتری نشان می‌دهد که هنگام آزمایش مدل سفارشی برای اولین بار مفید است. اگر مدل با موفقیت بارگذاری شود، یک پرامپت تعاملی خواهید دید. چند پیام امتحان کنید:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

برای پایان جلسه، عبارت `exit` را تایپ کنید یا کلیدهای `Ctrl+C` را فشار دهید.

> **عیب‌یابی:** اگر مدل بارگذاری نشد، موارد زیر را بررسی کنید:
> - فایل `genai_config.json` توسط سازنده مدل تولید شده است.
> - فایل `inference_model.json` وجود دارد و JSON معتبر است.
> - فایل‌های مدل ONNX در مسیر درست قرار دارند.
> - حافظه RAM کافی دارید (مدل Qwen3-0.6B int4 تقریباً ۱ گیگابایت نیاز دارد).
> - Qwen3 یک مدل استدلالی است که تگ‌های `<think>` تولید می‌کند. اگر پاسخ‌ها با `<think>...</think>` شروع شوند، این رفتار طبیعی است. قالب پرامپت در `inference_model.json` را می‌توان تنظیم کرد تا خروجی فکر کردن را سرکوب کند.

---

### تمرین ۸: پرس‌وجو از مدل سفارشی از طریق REST API

اگر در تمرین ۷ از جلسه تعاملی خارج شده‌اید، ممکن است مدل دیگر بارگذاری نشده باشد. ابتدا سرویس Foundry Local را راه‌اندازی و مدل را بارگیری کنید:

```bash
foundry service start
foundry model load qwen3-0.6b
```

بررسی کنید سرویس روی کدام پورت اجرا می‌شود:

```bash
foundry service status
```

سپس درخواست ارسال کنید (اگر پورت شما متفاوت است، `5273` را جایگزین کنید):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **نکته ویندوز:** دستور `curl` بالا از سینتکس bash استفاده می‌کند. در ویندوز به جای آن از cmdlet `Invoke-RestMethod` در PowerShell استفاده کنید.

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

### تمرین ۹: استفاده از مدل سفارشی با SDK اوپن‌ای‌آی

می‌توانید به مدل سفارشی خود با همان کدهای SDK اوپن‌ای‌آی که برای مدل‌های داخلی استفاده کردید متصل شوید (به [بخش ۳](part3-sdk-and-apis.md) مراجعه کنید). تنها تفاوت نام مدل است.

<details>
<summary><b>پایتون</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # فاندری لوکال کلیدهای API را اعتبارسنجی نمی‌کند
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
<summary><b>جاوااسکریپت</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // Foundry Local کلیدهای API را اعتبارسنجی نمی‌کند
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

> **نکته کلیدی:** از آنجا که Foundry Local یک API سازگار با اوپن‌ای‌آی ارائه می‌دهد، هر کدی که با مدل‌های داخلی کار می‌کند، با مدل‌های سفارشی شما نیز کار می‌کند. فقط کافی است پارامتر `model` را تغییر دهید.

---

### تمرین ۱۰: آزمایش مدل سفارشی با Foundry Local SDK

در آزمایشگاه‌های قبلی از Foundry Local SDK برای راه‌اندازی سرویس، کشف نقطه پایانی و مدیریت مدل‌ها به‌طور خودکار استفاده کردید. می‌توانید دقیقاً همان الگو را با مدل سفارشی خود دنبال کنید. SDK راه‌اندازی سرویس و کشف نقطه پایانی را انجام می‌دهد، پس نیازی نیست در کد خود `localhost:5273` را به‌صورت سخت‌کد قرار دهید.

> **توجه:** قبل از اجرای این مثال‌ها مطمئن شوید Foundry Local SDK نصب شده است:
> - **پایتون:** `pip install foundry-local openai`
> - **جاوااسکریپت:** `npm install foundry-local-sdk openai`
> - **C#:** بسته‌های NuGet `Microsoft.AI.Foundry.Local` و `OpenAI` را اضافه کنید
>
> هر فایل اسکریپت را در **ریشه مخزن** ذخیره کنید (همان پوشه‌ای که فولدر `models/` شما قرار دارد).

<details>
<summary><b>پایتون</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# مرحله ۱: سرویس محلی Foundry را راه‌اندازی کنید و مدل سفارشی را بارگذاری کنید
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# مرحله ۲: کش را برای مدل سفارشی بررسی کنید
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# مرحله ۳: مدل را در حافظه بارگذاری کنید
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# مرحله ۴: یک کلاینت OpenAI با استفاده از نقطه پایانی کشف شده توسط SDK ایجاد کنید
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# مرحله ۵: ارسال درخواست تکمیل چت به صورت استریمینگ
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

اجرایش کنید:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>جاوااسکریپت</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// مرحله ۱: سرویس Foundry Local را شروع کنید
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// مرحله ۲: مدل سفارشی را از کاتالوگ دریافت کنید
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// مرحله ۳: مدل را در حافظه بارگذاری کنید
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// مرحله ۴: یک کلاینت OpenAI با استفاده از نقطه انتهایی کشف شده توسط SDK ایجاد کنید
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// مرحله ۵: درخواست تکمیل گفتگو به صورت استریم را ارسال کنید
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

اجرایش کنید:

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

> **نکته کلیدی:** Foundry Local SDK نقطه پایانی را به‌طور پویا کشف می‌کند و هرگز شماره پورت را به‌صورت سخت‌کد ذخیره نمی‌کنید. این رویکرد توصیه‌شده برای برنامه‌های تولیدی است. مدل سفارشی شما به‌طور یکسان مانند مدل‌های فهرست‌شده از طریق SDK کار می‌کند.

---

## انتخاب مدلی برای کامپایل

مدل Qwen3-0.6B به‌عنوان نمونه در این آزمایشگاه استفاده شده چون کوچک، سریع برای کامپایل و تحت مجوز Apache 2.0 به‌صورت رایگان در دسترس است. با این حال می‌توانید مدل‌های زیادی را کامپایل کنید. در اینجا چند پیشنهاد آمده است:

| مدل | شناسه Hugging Face | پارامترها | مجوز | توضیحات |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | ۰.۶ میلیارد | Apache 2.0 | بسیار کوچک، کامپایل سریع، مناسب آزمایش |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | ۱.۷ میلیارد | Apache 2.0 | کیفیت بهتر، هنوز سریع برای کامپایل |
| Qwen3-4B | `Qwen/Qwen3-4B` | ۴ میلیارد | Apache 2.0 | کیفیت قوی، نیاز به RAM بیشتر |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | ۱ میلیارد | Llama 3.2 | نیازمند قبول مجوز در Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | ۷ میلیارد | Apache 2.0 | کیفیت بالا، دانلود بزرگتر و زمان کامپایل طولانی‌تر |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | ۳.۸ میلیارد | MIT | قبلاً در فهرست Foundry Local موجود است (مفید برای مقایسه) |

> **تذکر مجوز:** همیشه پیش از استفاده، مجوز مدل در Hugging Face را بررسی کنید. برخی مدل‌ها (مانند Llama) نیازمند پذیرش قرارداد مجوز و احراز هویت با `huggingface-cli login` قبل از دانلود هستند.

---

## مفاهیم: کی از مدل‌های سفارشی استفاده کنیم

| موقعیت | چرا مدل خود را کامپایل کنیم؟ |
|----------|------------------------------|
| **مدلی که نیاز دارید در فهرست نیست** | فهرست Foundry Local گزیده است. اگر مدل دلخواه در فهرست نیست، خودتان آن را کامپایل کنید. |
| **مدل‌های فاین‌تیون‌شده** | اگر مدلی را روی داده‌های تخصصی تنظیم کرده‌اید، نیاز به کامپایل وزن‌های خود دارید. |
| **نیازهای خاص کمی‌سازی** | ممکن است بخواهید استراتژی دقت یا کمی‌سازی متفاوت از حالت پیش‌فرض فهرست اعمال کنید. |
| **نسخه‌های جدید مدل‌ها** | وقتی مدل جدیدی در Hugging Face منتشر می‌شود، ممکن است هنوز در فهرست Foundry Local نباشد. با کامپایل شخصی، دسترسی فوری دارید. |
| **پژوهش و آزمایش** | آزمایش معماری‌ها، اندازه‌ها یا پیکربندی‌های متفاوت روی سیستم محلی قبل از انتخاب نهایی برای تولید. |

---

## خلاصه

در این آزمایشگاه یاد گرفتید چگونه:

| مرحله | کاری که انجام دادید |
|------|-------------|
| ۱ | نصب سازنده مدل ONNX Runtime GenAI |
| ۲ | کامپایل `Qwen/Qwen3-0.6B` از Hugging Face به مدل بهینه ONNX |
| ۳ | ایجاد فایل پیکربندی قالب چت `inference_model.json` |
| ۴ | افزودن مدل کامپایل‌شده به کش Foundry Local |
| ۵ | اجرای چت تعاملی با مدل سفارشی از طریق CLI |
| ۶ | پرس‌وجو از مدل از طریق REST API سازگار با اوپن‌ای‌آی |
| ۷ | اتصال از پایتون، جاوااسکریپت و C# با استفاده از SDK اوپن‌ای‌آی |
| ۸ | تست مدل سفارشی از ابتدا تا انتها با Foundry Local SDK |

نکته مهم این است که **هر مدل مبتنی بر ترنسفورمر می‌تواند پس از کامپایل به فرمت ONNX از طریق Foundry Local اجرا شود.** API سازگار با اوپن‌ای‌آی به این معناست که همه کدهای برنامه شما بدون تغییر کار می‌کنند؛ فقط کافی است نام مدل را عوض کنید.

---

## نکات کلیدی

| مفهوم | جزئیات |
|---------|--------|
| سازنده مدل ONNX Runtime GenAI | مدل‌های Hugging Face را با کمی‌سازی در یک دستور به فرمت ONNX تبدیل می‌کند |
| فرمت ONNX | Foundry Local به مدل‌های ONNX با پیکربندی ONNX Runtime GenAI نیاز دارد |
| قالب‌های چت | فایل `inference_model.json` به Foundry Local می‌گوید چگونه پرامپت‌ها را برای مدل خاص قالب‌بندی کند |
| اهداف سخت‌افزاری | کامپایل برای CPU، GPU انویدیا (CUDA)، DirectML (ویندوز GPU) یا WebGPU بسته به سخت‌افزار شما |
| کمی‌سازی | دقت پایین‌تر (int4) اندازه را کاهش و سرعت را بهبود می‌بخشد با از دست دادن مقداری دقت؛ fp16 کیفیت بالا را روی GPU حفظ می‌کند |
| سازگاری API | مدل‌های سفارشی از همان API سازگار با اوپن‌ای‌آی که مدل‌های داخلی استفاده می‌کنند بهره می‌برند |
| Foundry Local SDK | SDK راه‌اندازی سرویس، کشف نقطه پایانی و بارگذاری مدل را به‌طور خودکار برای هر دو مدل فهرست‌شده و سفارشی مدیریت می‌کند |

---

## خواندن‌های بیشتر

| منبع | لینک |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| راهنمای مدل سفارشی Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| خانواده مدل Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| مستندات Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## مراحل بعدی

ادامه دهید به [بخش ۱۱: فراخوانی ابزار با مدل‌های محلی](part11-tool-calling.md) برای یادگیری نحوه فعال‌سازی مدل‌های محلی برای فراخوانی توابع خارجی.

[← بخش ۹: تبدیل گفتار Whisper](part9-whisper-voice-transcription.md) | [بخش ۱۱: فراخوانی ابزار →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**توضیح مسئولیت**:
این سند با استفاده از سرویس ترجمه هوش مصنوعی [Co-op Translator](https://github.com/Azure/co-op-translator) ترجمه شده است. در حالی که ما برای دقت تلاش می‌کنیم، لطفاً آگاه باشید که ترجمه‌های خودکار ممکن است دارای خطا یا نادرستی باشند. سند اصلی به زبان مبدأ باید به عنوان منبع معتبر در نظر گرفته شود. برای اطلاعات حیاتی، ترجمه حرفه‌ای انسانی توصیه می‌شود. ما مسئول هیچ گونه سوء تفاهم یا تفسیر نادرست ناشی از استفاده از این ترجمه نیستیم.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->