![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# অংশ ১০: Foundry Local-এর সাথে কাস্টম বা Hugging Face মডেল ব্যবহার করা

> **লক্ষ্য:** Foundry Local-এর প্রয়োজনীয় অপটিমাইজ করা ONNX ফরম্যাটে একটি Hugging Face মডেল কম্পাইল করা, এটি একটি চ্যাট টেমপ্লেট দিয়ে কনফিগার করা, লোকাল ক্যাশে যোগ করা, এবং CLI, REST API, এবং OpenAI SDK ব্যবহার করে inference চালানো।

## ওভারভিউ

Foundry Local পূর্বে কম্পাইল করা মডেলের একটি নির্বাচিত ক্যাটালগ নিয়ে আসে, কিন্তু আপনি শুধুমাত্র ঐ তালিকাতে সীমাবদ্ধ নন। [Hugging Face](https://huggingface.co/) এ পাওয়া যেকোনো ট্রান্সফর্মার-ভিত্তিক ভাষা মডেল (অথবা স্থানীয়ভাবে PyTorch / Safetensors ফরম্যাটে সংরক্ষিত) অপটিমাইজ করা ONNX মডেলে কম্পাইল করা যায় এবং Foundry Local-এর মাধ্যমে সেবা দেওয়া যায়।

কম্পাইলেশন পাইপলাইন ব্যবহার করে **ONNX Runtime GenAI Model Builder**, যা একটি কমান্ড-লাইন টুল এবং `onnxruntime-genai` প্যাকেজের অংশ। মডেল বিল্ডার ভারী কাজগুলো করে: সোর্স ওয়েটস ডাউনলোড করা, ONNX ফরম্যাটে রূপান্তর করা, কোয়ান্টাইজেশন প্রয়োগ করা (int4, fp16, bf16), এবং Foundry Local-এর প্রয়োজনীয় কনফিগারেশন ফাইল তৈরি করা (যেমন চ্যাট টেমপ্লেট এবং টোকেনাইজার)।

এই ল্যাবে আপনি Hugging Face থেকে **Qwen/Qwen3-0.6B** কম্পাইল করবেন, Foundry Local-এ রেজিস্টার করবেন, এবং সম্পূর্ণ আপনার ডিভাইসে এর সাথে চ্যাট করবেন।

---

## শেখার উদ্দেশ্য

এই ল্যাবের শেষে আপনি সক্ষম হবেন:

- কেন কাস্টম মডেল কম্পাইলেশন দরকার এবং কখন প্রয়োজন তা ব্যাখ্যা করতে
- ONNX Runtime GenAI মডেল বিল্ডার ইনস্টল করতে
- একক কমান্ডে একটি Hugging Face মডেল অপটিমাইজ করা ONNX ফরম্যাটে কম্পাইল করতে
- মূল কম্পাইলেশন প্যারামিটারগুলো (execution provider, precision) বুঝতে
- `inference_model.json` চ্যাট-টেমপ্লেট কনফিগারেশন ফাইল তৈরি করতে
- কম্পাইল করা মডেল Foundry Local ক্যাশে যোগ করতে
- CLI, REST API, এবং OpenAI SDK ব্যবহার করে কাস্টম মডেলের উপর inference চালাতে

---

## প্রয়োজনীয়তাসমূহ

| প্রয়োজনীয়তা | বিবরণ |
|-------------|---------|
| **Foundry Local CLI** | ইনস্টল করা এবং আপনার `PATH` এ থাকা ([Part 1](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI মডেল বিল্ডারের জন্য প্রয়োজন |
| **pip** | পাইথন প্যাকেজ ম্যানেজার |
| **ডিস্ক স্পেস** | সোর্স এবং কম্পাইল করা মডেল ফাইলের জন্য কমপক্ষে ৫ জিবি ফ্রী |
| **Hugging Face অ্যাকাউন্ট** | কিছু মডেল ডাউনলোডের আগে লাইসেন্স গ্রহণ করতে হতে পারে। Qwen3-0.6B Apache 2.0 লাইসেন্স ব্যবহার করে এবং এটি ফ্রিতে পাওয়া যায়। |

---

## এনভায়রনমেন্ট সেটআপ

মডেল কম্পাইলেশন জন্য বেশ কিছু বড় পাইথন প্যাকেজ (PyTorch, ONNX Runtime GenAI, Transformers) দরকার। একটি আলাদাভাবে ভার্চুয়াল এনভায়রনমেন্ট তৈরি করুন যাতে এগুলো আপনার সিস্টেম পাইথন বা অন্য প্রকল্পের সাথে সমস্যা না করে।

```bash
# রিপোজিটরি রুট থেকে
python -m venv .venv
```

এনভায়রনমেন্ট সক্রিয় করুন:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

ডিপেনডেন্সি সমস্যার এড়াতে pip আপগ্রেড করুন:

```bash
python -m pip install --upgrade pip
```

> **টিপ:** যদি আগের ল্যাব থেকে `.venv` ইতিমধ্যে থাকে, আপনি সেটা পুনরায় ব্যবহার করতে পারেন। শুধু সেটি চালু থাকুক নিশ্চিত করুন।

---

## ধারণা: কম্পাইলেশন পাইপলাইন

Foundry Local ONNX ফরম্যাটে ONNX Runtime GenAI কনফিগারেশনসহ মডেল চায়। অধিকাংশ ওপেন সোর্স Hugging Face মডেল PyTorch বা Safetensors ওয়েটস আকারে থাকে, তাই রূপান্তর ধাপ দরকার।

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### মডেল বিল্ডার কী করে?

১. Hugging Face থেকে সোর্স মডেল ডাউনলোড করে (অথবা স্থানীয় পাথ থেকে পড়ে)।
২. PyTorch / Safetensors ওয়েটস ONNX ফরম্যাটে রূপান্তর করে।
৩. মডেল ছোট প্রিসিশনে কোয়ান্টাইজ করে (যেমন int4) মেমরি কমাতে এবং পারফরম্যান্স বাড়াতে।
৪. ONNX Runtime GenAI কনফিগারেশন (`genai_config.json`), চ্যাট টেমপ্লেট (`chat_template.jinja`), এবং সমস্ত টোকেনাইজার ফাইল তৈরি করে যাতে Foundry Local মডেল লোড ও সেবা দিতে পারে।

### ONNX Runtime GenAI Model Builder বনাম Microsoft Olive

আপনি হয়তো **Microsoft Olive** সম্পর্কেও শুনেছেন যেটি মডেল অপটিমাইজেশনের বিকল্প টুল। এই দুইটি টুল ONNX মডেল তৈরি করতে পারে, তবে ভিন্ন উদ্দেশ্য ও ফিচার রয়েছে:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **প্যাকেজ** | `onnxruntime-genai` | `olive-ai` |
| **মূল উদ্দেশ্য** | ONNX Runtime GenAI inference এর জন্য জেনারেটিভ AI মডেল রূপান্তর ও কোয়ান্টাইজেশন | এন্ড-টু-এন্ড মডেল অপটিমাইজেশন ফ্রেমওয়ার্ক, অনেক ব্যাকএন্ড ও হার্ডওয়্যার সমর্থন করে |
| **ব্যবহার সহজতা** | এক কমান্ডে — এক ধাপে রূপান্তর + কোয়ান্টাইজেশন | ওয়ার্কফলো ভিত্তিক — যামএল/জেসন কনফিগারযোগ্য মাল্টি-পাস পাইপলাইন |
| **আউটপুট ফরম্যাট** | ONNX Runtime GenAI ফরম্যাট (Foundry Local এর জন্য প্রস্তুত) | জেনেরিক ONNX, ONNX Runtime GenAI অথবা ওয়ার্কফলো অনুসারে |
| **হার্ডওয়্যার টার্গেট** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, আরও অনেক |
| **কোয়ান্টাইজেশন বিকল্প** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, গ্রাফ অপ্টিমাইজেশন, লেয়ার-বাই-লেয়ার টিউনিং |
| **মডেল স্কোপ** | জেনারেটিভ AI মডেল (LLM, SLM) | যেকোনো ONNX-রূপান্তরযোগ্য মডেল (ভিশন, NLP, অডিও, মাল্টিমোডাল) |
| **সেরা ব্যবহার** | লোকাল ইনফারেন্সের জন্য দ্রুত এক-মডেল কম্পাইলেশন | প্রোডাকশন পাইপলাইন যেখানে সূক্ষ্ম নিয়ন্ত্রণ প্রয়োজন |
| **ডিপেনডেন্সি পরিধি** | মাঝারি (PyTorch, Transformers, ONNX Runtime) | বড় (Olive ফ্রেমওয়ার্ক যোগ হয়, অতিরিক্ত অপশনাল) |
| **Foundry Local ইন্টিগ্রেশন** | সরাসরি — আউটপুট সঙ্গে সঙ্গেই সামঞ্জস্যপূর্ণ | `--use_ort_genai` ফ্ল্যাগ ও অতিরিক্ত কনফিগারেশন প্রয়োজন |

> **এই ল্যাবে মডেল বিল্ডার ব্যবহার করার কারণ:** একটি Hugging Face মডেল কম্পাইল ও Foundry Local-এ রেজিস্টার করার কাজের জন্য মডেল বিল্ডার হল সবচেয়ে সোজা ও সবচেয়ে নির্ভরযোগ্য উপায়। এটি একটি কমান্ডে Foundry Local-এ প্রয়োজনীয় সঠিক আউটপুট তৈরি করে। পরবর্তীতে যদি উন্নত অপটিমাইজেশনের প্রয়োজন হয়—যেমন অর্কুরেসি সচেতন কোয়ান্টাইজেশন, গ্রাফ সার্জারি বা মাল্টি-পাস টিউনিং—তবে Olive একটি শক্তিশালী বিকল্প। বিস্তারিত জানতে দেখুন [Microsoft Olive ডকুমেন্টেশন](https://microsoft.github.io/Olive/)।

---

## ল্যাব অনুশীলনসমূহ

### অনুশীলন ১: ONNX Runtime GenAI Model Builder ইনস্টল করুন

ONNX Runtime GenAI প্যাকেজ ইনস্টল করুন, যা মডেল বিল্ডার টুল অন্তর্ভুক্ত:

```bash
pip install onnxruntime-genai
```

ইনস্টলেশন সঠিক হয়েছে কিনা যাচাই করতে মডেল বিল্ডার উপলভ্য কিনা দেখুন:

```bash
python -m onnxruntime_genai.models.builder --help
```

আপনি দেখতে পাবেন সাহায্যের আউটপুট যেখানে প্যারামিটারসমূহ যেমন `-m` (মডেল নাম), `-o` (আউটপুট পথ), `-p` (প্রেসিশন), এবং `-e` (এক্সিকিউশন প্রোভাইডার) তালিকাভুক্ত।

> **দ্রষ্টব্য:** মডেল বিল্ডার PyTorch, Transformers, এবং আরও কিছু প্যাকেজের উপর নির্ভরশীল। ইনস্টলেশনে কয়েক মিনিট সময় লাগতে পারে।

---

### অনুশীলন ২: CPU জন্য Qwen3-0.6B কম্পাইল করুন

নিচের কমান্ডটি চালিয়ে Hugging Face থেকে Qwen3-0.6B মডেল ডাউনলোড করুন এবং CPU inference এর জন্য int4 কোয়ান্টাইজেশন সহ কম্পাইল করুন:

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

#### প্রতিটি প্যারামিটারের অর্থ

| প্যারামিটার | উদ্দেশ্য | ব্যবহারকৃত মান |
|-----------|---------|------------|
| `-m` | Hugging Face মডেল ID অথবা স্থানীয় ডিরেক্টরির পথ | `Qwen/Qwen3-0.6B` |
| `-o` | যেখানে কম্পাইল করা ONNX মডেল সংরক্ষণ করা হবে সেই ডিরেক্টরি | `models/qwen3` |
| `-p` | কম্পাইলেশনের সময় প্রয়োগকৃত কোয়ান্টাইজেশন প্রেসিশন | `int4` |
| `-e` | ONNX Runtime এক্সিকিউশন প্রোভাইডার (টার্গেট হার্ডওয়্যার) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face অ্যাথেন্টিকেশন এড়িয়ে যায় (পাবলিক মডেলের জন্য ঠিক আছে) | `hf_token=false` |

> **এটি কতক্ষণ সময় নেয়?** কম্পাইলেশনের সময় আপনার হার্ডওয়্যার এবং মডেল আকারের উপর নির্ভর করে। আধুনিক CPU তে Qwen3-0.6B এর int4 কোয়ান্টাইজেশনের জন্য সাধারণত ৫ থেকে ১৫ মিনিট লাগে। বড় মডেলগুলোর জন্য সময় প্রোপোরশনালভাবে বেশি।

কমান্ড সফল হলে `models/qwen3` ডিরেক্টরি তৈরি হবে যেখানে কম্পাইল করা মডেল ফাইল থাকবে। আউটপুট যাচাই করুন:

```bash
ls models/qwen3
```

আপনি নিম্নলিখিত ফাইল দেখতে পাবেন:
- `model.onnx` এবং `model.onnx.data` — কম্পাইল করা মডেল ওয়েটস
- `genai_config.json` — ONNX Runtime GenAI কনফিগারেশন
- `chat_template.jinja` — মডেলের চ্যাট টেমপ্লেট (অটো-জেনারেটেড)
- `tokenizer.json`, `tokenizer_config.json` — টোকেনাইজার ফাইলসমূহ
- অন্যান্য শব্দভাণ্ডার এবং কনফিগারেশন ফাইলসমূহ

---

### অনুশীলন ৩: GPU-র জন্য কম্পাইল করুন (ঐচ্ছিক)

যদি আপনার NVIDIA GPU CUDA সমর্থিত থাকে, আপনি আরো দ্রুত ইনফারেন্সের জন্য GPU-অপ্টিমাইজ্ড ভার্সন কম্পাইল করতে পারেন:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **দ্রষ্টব্য:** GPU কম্পাইলেশনের জন্য `onnxruntime-gpu` এবং কাজ করা CUDA ইনস্টলেশন দরকার। না থাকলে মডেল বিল্ডার ত্রুটি দেখাবে। আপনি এই অনুশীলন বাদ দিয়ে CPU ভার্সন দিয়ে চালিয়ে যেতে পারেন।

#### হার্ডওয়্যার-ভিত্তিক কম্পাইলেশন রেফারেন্স

| টার্গেট | এক্সিকিউশন প্রোভাইডার (`-e`) | সুপারিশকৃত প্রেসিশন (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` অথবা `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` অথবা `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### প্রেসিশনের পার্থক্য

| প্রেসিশন | আকার | গতি | গুণগত মান |
|-----------|------|-------|---------|
| `fp32` | সবচেয়ে বড় | সবচেয়ে ধীর | সর্বোচ্চ যথার্থতা |
| `fp16` | বড় | দ্রুত (GPU) | খুব ভাল যথার্থতা |
| `int8` | ছোট | দ্রুত | সোজা একটু যথার্থতা হ্রাস |
| `int4` | সবচেয়ে ছোট | সবচেয়ে দ্রুত | মাঝারি যথার্থতা কমতি |

অধিকাংশ লোকাল ডেভেলপমেন্টের জন্য CPU তে `int4` সর্বোত্তম গতিসূচক ও সম্পদ ব্যবহার ভারসাম্য দেয়। প্রোডাকশন মানের জন্য CUDA GPU এ `fp16` সুপারিশিত।

---

### অনুশীলন ৪: চ্যাট টেমপ্লেট কনফিগারেশন তৈরি করুন

মডেল বিল্ডার স্বয়ংক্রিয়ভাবে `chat_template.jinja` এবং `genai_config.json` ফাইল আউটপুট ডিরেক্টরিতে তৈরি করে। তবে Foundry Local এর জন্য `inference_model.json` ফাইলও প্রয়োজন যা জানায় কি রকম প্রম্পট মডেলে পাঠাতে হয়। এই ফাইলটি মডেলের নাম এবং প্রম্পট টেমপ্লেট সংজ্ঞায়িত করে যা ব্যবহারকারীর মেসেজ সঠিক বিশেষ টোকেনে মোড়কে রাখে।

#### ধাপ ১: কম্পাইল করা আউটপুট পরীক্ষা করুন

কম্পাইল করা মডেল ডিরেক্টরির বিষয়বস্তু তালিকা করুন:

```bash
ls models/qwen3
```

আপনি নিম্নলিখিত ফাইল দেখতে পাবেন:
- `model.onnx` এবং `model.onnx.data` — কম্পাইল মডেল ওয়েটস
- `genai_config.json` — ONNX Runtime GenAI কনফিগারেশন (অটো-জেনারেটেড)
- `chat_template.jinja` — মডেলের চ্যাট টেমপ্লেট (অটো-জেনারেটেড)
- `tokenizer.json`, `tokenizer_config.json` — টোকেনাইজার ফাইলসমূহ
- অন্যান্য বিভিন্ন কনফিগারেশন এবং শব্দভান্ডার ফাইল

#### ধাপ ২: inference_model.json ফাইল তৈরি করুন

`inference_model.json` ফাইলটি Foundry Local-কে জানায় কিভাবে প্রম্পট ফরম্যাট করতে হয়। একটি পাইথন স্ক্রিপ্ট তৈরি করুন যার নাম `generate_chat_template.py` **রিপোজিটরির রুট ডিরেক্টরতে** (যেখানে আপনার `models/` ফোল্ডার আছে):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# চ্যাট টেমপ্লেট বের করতে একটি ন্যূনতম আলাপ তৈরি করুন
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

# inference_model.json কাঠামো তৈরি করুন
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

রিপোজিটরির রুট থেকে স্ক্রিপ্ট চালান:

```bash
python generate_chat_template.py
```

> **দ্রষ্টব্য:** `transformers` প্যাকেজটি ইতিমধ্যে `onnxruntime-genai` ডিপেনডেন্সি হিসেবে ইনস্টল হয়েছে। যদি `ImportError` দেখেন, আগে `pip install transformers` চালান।

স্ক্রিপ্ট চালানোর পর `models/qwen3` ডিরেক্টরির মধ্যে `inference_model.json` ফাইল তৈরি হবে। ফাইলটি Foundry Local-কে জানায় কিভাবে Qwen3 মডেলের জন্য ব্যবহারকারীর ইনপুটকে সঠিক বিশেষ টোকেনে মোড়ক করতে হয়।

> **গুরুত্বপূর্ণ:** `inference_model.json`-এর `"Name"` ফিল্ড (স্ক্রিপ্টে `qwen3-0.6b` সেট করা হয়েছে) হল মডেলের **অ্যালিয়াস** যা পরবর্তী সকল কমান্ড এবং API কলগুলোতে ব্যবহার করবেন। যদি নাম পরিবর্তন করেন, অনুশীলন ৬–১০ এ সেই নাম আপডেট করুন।

#### ধাপ ৩: কনফিগারেশন যাচাই করুন

`models/qwen3/inference_model.json` খুলে নিশ্চিত করুন এতে একটি `Name` ফিল্ড এবং একটি `PromptTemplate` অবজেক্ট আছে যার মধ্যে `assistant` এবং `prompt` কী আছে। প্রম্পট টেমপ্লেটে `<|im_start|>` ও `<|im_end|>` মত বিশেষ টোকেনগুলো থাকা উচিত (টোকেনগুলি নির্ভর করে মডেলের চ্যাট টেমপ্লেটের উপর)।

> **ম্যানুয়াল বিকল্প:** যদি স্ক্রিপ্ট চালাতে না চান, ম্যানুয়ালি ফাইলটি তৈরি করতে পারেন। মূল শর্ত হল `prompt` ফিল্ডে মডেলের সম্পূর্ণ চ্যাট টেমপ্লেট থাকা এবং `{Content}` ব্যবহারকারীর মেসেজের প্লেসহোল্ডার হিসেবে থাকুক।

---

### অনুশীলন ৫: মডেল ডিরেক্টরির স্ট্রাকচার যাচাই করুন


মডেল বিল্ডার সমস্ত কম্পাইল করা ফাইল সরাসরি আপনার নির্দিষ্ট আউটপুট ডিরেক্টরিতে রাখে। নিশ্চিত করুন যে চূড়ান্ত কাঠামো সঠিক দেখাচ্ছে:

```bash
ls models/qwen3
```

ডিরেক্টরিতে নিম্নলিখিত ফাইলগুলো থাকা উচিত:

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

> **নোট:** কিছু অন্যান্য কম্পাইলেশন টুলের মতো নয়, মডেল বিল্ডার nested সাবডিরেক্টরি তৈরি করে না। সমস্ত ফাইল সরাসরি আউটপুট ফোল্ডারে থাকে, যা ঠিক Foundry Local এর প্রত্যাশা অনুযায়ী।

---

### অনুশীলন ৬: মডেলটিকে Foundry Local ক্যাশে যুক্ত করুন

Foundry Local কে জানান আপনার কম্পাইল করা মডেলটি কোথায় খুঁজতে হবে, ডিরেক্টরিটিকে তার ক্যাশে যুক্ত করে:

```bash
foundry cache cd models/qwen3
```

ক্যাশেতে মডেলটি আসছে কিনা যাচাই করুন:

```bash
foundry cache ls
```

আপনি আপনার কাস্টম মডেলটি আগের ক্যাশ করা মডেলগুলোর (যেমন `phi-3.5-mini` বা `phi-4-mini`) পাশাপাশি তালিকাভুক্ত দেখতে পাবেন।

---

### অনুশীলন ৭: CLI দিয়ে কাস্টম মডেল চালান

আপনার সদ্য কম্পাইল করা মডেল দিয়ে একটি ইন্টারেক্টিভ চ্যাট সেশন শুরু করুন (`qwen3-0.6b` উপনামটি `inference_model.json` এর `Name` ফিল্ড থেকে এসেছে):

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` ফ্ল্যাগ অতিরিক্ত ডায়াগনস্টিক তথ্য দেখায়, যা প্রথমবার কাস্টম মডেল পরীক্ষা করার সময় সহায়ক। মডেল সফলভাবে লোড হলে আপনি একটি ইন্টারেক্টিভ প্রম্পট দেখতে পাবেন। কিছু মেসেজ প্রেরণ করুন:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

সেশন শেষ করতে `exit` টাইপ করুন অথবা `Ctrl+C` চাপুন।

> **সমস্যা সমাধান:** যদি মডেল লোড করতে ব্যর্থ হয়, নিম্নলিখিতগুলি পরীক্ষা করুন:
> - `genai_config.json` ফাইলটি মডেল বিল্ডার দ্বারা তৈরি হয়েছে কিনা।
> - `inference_model.json` ফাইলটি আছে এবং বৈধ JSON কিনা।
> - ONNX মডেল ফাইলগুলি সঠিক ডিরেক্টরিতে আছে।
> - আপনার কাছে পর্যাপ্ত RAM আছে (Qwen3-0.6B int4 প্রায় ১ জিবি প্রয়োজন)।
> - Qwen3 একটি reasoning মডেল যা `<think>` ট্যাগ উৎপন্ন করে। যদি প্রতিক্রিয়াগুলোর সামনে `<think>...</think>` দেখা যায়, সেটি স্বাভাবিক আচরণ। `inference_model.json` ফাইলে প্রম্পট টেমপ্লেট পরিবর্তন করে চিন্তার আউটপুট কমানো যেতে পারে।

---

### অনুশীলন ৮: REST API দিয়ে কাস্টম মডেলকে প্রশ্ন করুন

আপনি অনুশীলন ৭-এ ইন্টারেক্টিভ সেশন থেকে বের হয়ে গেছেন, তাই মডেলটি হয়তো আর লোড থাকে না। প্রথমে Foundry Local সার্ভিস শুরু করুন এবং মডেলটি লোড করুন:

```bash
foundry service start
foundry model load qwen3-0.6b
```

সার্ভিস কোন পোর্টে চলছে যাচাই করুন:

```bash
foundry service status
```

তারপর একটি অনুরোধ পাঠান (যদি আপনার পোর্ট `5273` থেকে আলাদা হয় তাহলে এটিকে প্রতিস্থাপন করুন):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows নোট:** উপরের `curl` কমান্ডটি bash সিনট্যাক্স ব্যবহার করে। Windows এ PowerShell এর `Invoke-RestMethod` কমান্ড ব্যবহার করুন।

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

### অনুশীলন ৯: OpenAI SDK দিয়ে কাস্টম মডেল ব্যবহার করুন

আপনি আপনার কাস্টম মডেলের সাথে ঠিক একই OpenAI SDK কোড ব্যবহার করতে পারবেন যা আপনি বিল্ট-ইন মডেলের জন্য ব্যবহার করেছেন ([পার্ট ৩](part3-sdk-and-apis.md) দেখুন)। শুধু মডেলের নাম পরিবর্তন করবেন।

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # ফাউন্ড্রি লোকাল এপিআই কী যাচাই করে না
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
  apiKey: "foundry-local", // ফাউন্ড্রি লোকাল API কী যাচাই করে না
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

> **মূল দৃষ্টিকোণ:** Foundry Local একটি OpenAI-সঙ্গত API প্রদান করে, তাই বিল্ট-ইন মডেলগুলোর জন্য কাজ করা কোনও কোডই আপনার কাস্টম মডেলগুলোর জন্যও কাজ করবে। শুধু `model` প্যারামিটার পরিবর্তন করুন।

---

### অনুশীলন ১০: Foundry Local SDK দিয়ে কাস্টম মডেল পরীক্ষা করুন

আগের ল্যাবে আপনি Foundry Local SDK ব্যবহার করে সার্ভিস শুরু, endpoint আবিষ্কার, এবং মডেল ব্যবস্থাপনা করেছেন স্বয়ংক্রিয়ভাবে। একই প্যাটার্ন আপনার কাস্টম-কম্পাইল মডেলেও ব্যবহার করতে পারেন। SDK সার্ভিস স্টার্টআপ এবং endpoint আবিষ্কার পরিচালনা করে, তাই আপনাকে `localhost:5273` হার্ডকোড করতে হবে না।

> **নোট:** নিচের উদাহরণ চালানোর আগে Foundry Local SDK ইনস্টল করুন:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` এবং `OpenAI` NuGet প্যাকেজ যুক্ত করুন
>
> প্রতিটি স্ক্রিপ্ট ফাইল **রিপোজিটরি রুটে** সংরক্ষণ করুন (যেখানে আপনার `models/` ফোল্ডার থাকে)।

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# ধাপ ১: Foundry Local সেবা শুরু করুন এবং কাস্টম মডেল লোড করুন
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# ধাপ ২: কাস্টম মডেলের জন্য ক্যাশ চেক করুন
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# ধাপ ৩: মডেলটি মেমরিতে লোড করুন
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# ধাপ ৪: SDK দ্বারা আবিষ্কৃত এন্ডপয়েন্ট ব্যবহার করে একটি OpenAI ক্লায়েন্ট তৈরি করুন
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# ধাপ ৫: স্ট্রিমিং চ্যাট সম্পূর্ণকরণের অনুরোধ পাঠান
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

চালান:

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

// ধাপ ১: Foundry Local সার্ভিস শুরু করুন
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ধাপ ২: ক্যাটালগ থেকে কাস্টম মডেলটি পান
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// ধাপ ৩: মডেলটি মেমরিতে লোড করুন
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// ধাপ ৪: SDK-আবিষ্কৃত এন্ডপয়েন্ট ব্যবহার করে একটি OpenAI ক্লায়েন্ট তৈরি করুন
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// ধাপ ৫: একটি স্ট্রিমিং চ্যাট কমপ্লিশন অনুরোধ পাঠান
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

চালান:

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

> **মূল দৃষ্টিকোণ:** Foundry Local SDK ডাইনামিক্যালি endpoint আবিষ্কার করে, তাই আপনি কখনো পোর্ট নাম্বার হার্ডকোড করবেন না। এটি প্রোডাকশন অ্যাপ্লিকেশনের জন্য সুপারিশকৃত পদ্ধতি। আপনার কাস্টম-কম্পাইল মডেল SDK এর মাধ্যমে বিল্ট-ইন ক্যাটালগ মডেলের মতোই কাজ করে।

---

## কম্পাইলের জন্য একটি মডেল নির্বাচন করা

Qwen3-0.6B এই ল্যাবে রেফারেন্স উদাহরণ হিসেবে ব্যবহৃত হয়েছে কারণ এটি ছোট, দ্রুত কম্পাইল হয় এবং Apache 2.0 লাইসেন্সের অধীনে সহজলভ্য। তবে আপনি অন্যান্য অনেক মডেলও কম্পাইল করতে পারেন। এখানে কিছু পরামর্শ:

| মডেল | Hugging Face ID | প্যারামিটার | লাইসেন্স | নোটস |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | খুব ছোট, দ্রুত কম্পাইল, পরীক্ষার জন্য ভালো |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | ভালো মানের, তবুও দ্রুত কম্পাইল |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | শক্তিশালী মান, বেশি RAM প্রয়োজন |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face-এ লাইসেন্স গ্রহণ দরকার |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | উচ্চ মানের, বড় ডাউনলোড এবং দীর্ঘ কম্পাইল সময় |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Foundry Local ক্যাটালগে ইতোমধ্যে আছে (তুলনার জন্য দরকারী) |

> **লাইসেন্সের স্মরণ করিয়ে দেওয়া:** ব্যবহারের আগে Hugging Face-এ মডেলের লাইসেন্স চেক করুন। কিছু মডেল (যেমন Llama) ডাউনলোডের আগে লাইসেন্স সম্মতিসহ `huggingface-cli login` দিয়ে প্রমাণীকরণ প্রয়োজন।

---

## ধারণা: কখন কাস্টম মডেল ব্যবহার করবেন

| পরিস্থিতি | কেন নিজের মডেল কম্পাইল করবেন? |
|----------|-------------------------------|
| **ক্যাটালগে আপনার প্রয়োজনের মডেল নেই** | Foundry Local ক্যাটালগ নির্বাচন করা হয়। আপনি যদি আপনার মডেল না পান, নিজেই কম্পাইল করুন। |
| **ফাইন-টিউনড মডেল** | যদি আপনি ডোমেইন-নির্দিষ্ট ডেটায় মডেল ফাইন-টিউন করে থাকেন, নিজস্ব ওয়েট কম্পাইল করতে হবে। |
| **নির্দিষ্ট কোয়ান্টাইজেশন প্রয়োজনীয়তা** | আপনি এমন কোয়ান্টাইজেশন বা প্রিসিশন চাইতে পারেন যা ক্যাটালগ ডিফল্ট থেকে আলাদা। |
| **নতুন মডেল রিলিজ** | নতুন মডেল আসলে তা ক্যাটালগে নাও থাকতে পারে, নিজ হাতে কম্পাইল করলে আপনি দ্রুত অ্যাক্সেস পাবেন। |
| **গবেষণা ও পরীক্ষন** | উৎপাদনে নেওয়ার আগে বিভিন্ন আর্কিটেকচার, আকার, অথবা কনফিগারেশন লোকালেই পরীক্ষা করা। |

---

## সারাংশ

এই ল্যাবে আপনি শিখলেন কীভাবে:

| ধাপ | আপনি যা করলেন |
|------|---------------|
| ১ | ONNX Runtime GenAI মডেল বিল্ডার ইনস্টল করলেন |
| ২ | Hugging Face থেকে `Qwen/Qwen3-0.6B` কম্পাইল করে অপটিমাইজড ONNX মডেল তৈরি করলেন |
| ৩ | `inference_model.json` চ্যাট টেমপ্লেট কনফিগারেশন ফাইল তৈরি করলেন |
| ৪ | কম্পাইল করা মডেল Foundry Local ক্যাশে যুক্ত করলেন |
| ৫ | CLI দিয়ে কাস্টম মডেলের সাথে ইন্টারেক্টিভ চ্যাট চালালেন |
| ৬ | OpenAI-সঙ্গত REST API দিয়ে মডেলকে প্রশ্ন করলেন |
| ৭ | Python, JavaScript, এবং C# থেকে OpenAI SDK ব্যবহার করলেন |
| ৮ | Foundry Local SDK দিয়ে কাস্টম মডেল সম্পূর্ণরূপে পরীক্ষা করলেন |

মুখ্য ধারনাটি হল **যেকোনো ট্রান্সফরমার-ভিত্তিক মডেল ONNX ফরম্যাটে কম্পাইল করার পর Foundry Local এ চলতে পারে**। OpenAI-সঙ্গত API থাকার ফলে আপনার বিদ্যমান অ্যাপ্লিকেশন কোড বিঘ্নিত হয় না; শুধু মডেলের নাম পরিবর্তন করতে হয়।

---

## প্রধান ধাপসমূহ

| ধারণা | বিস্তারিত |
|---------|----------|
| ONNX Runtime GenAI মডেল বিল্ডার | Hugging Face মডেলকে ONNX ফরম্যাটে কোয়ান্টাইজেশন সহ এক কমান্ডে রূপান্তর করে |
| ONNX ফরম্যাট | Foundry Local ONNX Runtime GenAI কনফিগারেশনসহ ONNX মডেল প্রয়োজন |
| চ্যাট টেমপ্লেট | `inference_model.json` ফাইল Foundry Local কে বলে মডেলের জন্য কিভাবে প্রম্পট ফরম্যাট করতে হবে |
| হার্ডওয়্যার টার্গেট | CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), অথবা WebGPU অনুযায়ী কম্পাইল করতে পারবেন |
| কোয়ান্টাইজেশন | কম প্রিসিশন (int4) আকার কমায় এবং গতি বাড়ায় স্বল্প কিছু সঠিকতা খরচে; fp16 GPU তে উচ্চ মান রাখে |
| API সামঞ্জস্যতা | কাস্টম মডেলও বিল্ট-ইন মডেলের মত OpenAI-সঙ্গত API ব্যবহার করে |
| Foundry Local SDK | SDK স্বয়ংক্রিয়ভাবে সার্ভিস শুরু, endpoint আবিষ্কার এবং মডেল লোডিং সামলায় ক্যাটালগ ও কাস্টম উভয় মডেলের জন্য |

---

## আরও পড়াশোনা

| রিসোর্স | লিংক |
|----------|-------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local কাস্টম মডেল গাইড | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 মডেল পরিবার | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive ডকুমেন্টেশন | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## পরবর্তী ধাপ

আপনার লোকাল মডেলগুলোকে বাহ্যিক ফাংশন কল করার সুবিধা দিতে শিখতে [Part 11: Tool Calling with Local Models](part11-tool-calling.md) এ যান।

[← Part 9: Whisper Voice Transcription](part9-whisper-voice-transcription.md) | [Part 11: Tool Calling →](part11-tool-calling.md)