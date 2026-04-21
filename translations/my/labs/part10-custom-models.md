![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# အပိုင်း ၁၀: Foundry Local နှင့်အတူ Custom သို့မဟုတ် Hugging Face မော်ဒယ်များ အသုံးပြုခြင်း

> **ရည်မှန်းချက်:** Foundry Local လိုအပ်သည့် အမြင့်တင်ပြီး ONNX ပုံစံသို့ Hugging Face မော်ဒယ်တစ်ခုကို compile ပြုလုပ်၍၊ chat နမူနာဖြင့်ပြင်ဆင်၍၊ ဒေသဆိုင်ရာ cache ထဲသို့ထည့်၍ CLI၊ REST API နှင့် OpenAI SDK အသုံးပြု၍ inference လုပ်ခြင်း။

## အနှစ်ချုပ်

Foundry Local သည် အသေးစိတ်ရွေးချယ်ပြီးပြင်ဆင်ထားသော pre-compiled မော်ဒယ်များကို ထည့်သွင်းပေးထားပေမယ့် ထိုစာရင်းတွင် သင်ကန့်သတ်ထားသည့် မဟုတ်ပါ။ [Hugging Face](https://huggingface.co/) တွင် ရနိုင်သည့် (သို့မဟုတ် ဒေသတွင် PyTorch / Safetensors ပုံစံဖြင့် သိမ်းဆည်းထားသော) transformer အခြေပြု ဘာသာစကားမော်ဒယ် မည်သည့် မော်ဒယ်ကိုမဆို optimised ONNX မော်ဒယ်အဖြစ် compile ပြုလုပ်ကာ Foundry Local မှတဆင့် ဝန်ဆောင်မှုပေးနိုင်ပါသည်။

ဒီ compilation လုပ်ငန်းစဉ်သည် `onnxruntime-genai` package တွင်ပါဝင်သော **ONNX Runtime GenAI Model Builder** ဆိုသည့် command-line ကိရိယာကို အသုံးပြုသည်။ မော်ဒယ်တည်ဆောက်သူသည် အလေးပေးဆောင်ရွက်ချက်များကို ကိုင်တွယ်ပေးသည်။ မှတ်တမ်းအလေးချိန်များကို ဒေါင်းလုပ်လုပ်ခြင်း၊ ONNX ပုံစံသို့ပြောင်းပြန်ခြင်း၊ quantisation (int4, fp16, bf16) ကိုအသုံးပြုခြင်းနှင့် Foundry Local မျှော်လင့်သည့် ဖိုင်များ (chat template နှင့် tokeniser အပါအဝင်) များ ထုတ်ပေးသည်။

ယခု lab တွင် Hugging Faceမှ **Qwen/Qwen3-0.6B** ကို compile ပြုလုပ်ကာ Foundry Local တွင် မှတ်ပုံတင်ပြီး သင်၏စက်ထဲတွင် တစ်ပြိုင်နက်တွင် ချက်ချင်း စကားပြောဆက်သွယ်နိုင်ပါမည်။

---

## သင်ယူရမည့်ရည်မှန်းချက်များ

ဤ lab အပြီးတွင် တက်ကြွစွာ လုပ်ဆောင်နိုင်မည့်အရာများမှာ -

- Custom မော်ဒယ် compile ပြုလုပ်ခြင်း၏ အကြောင်းပြချက်နှင့် ဘယ်အခါလိုအပ်ကြောင်း ရှင်းပြနိုင်ခြင်း
- ONNX Runtime GenAI model builder ကို တပ်ဆင်နိုင်ခြင်း
- Hugging Face မော်ဒယ်တစ်ခုကို တစ်ချက် command ဖြင့် optimised ONNX ပုံစံသို့ compile ပြုလုပ်နိုင်ခြင်း
- အခြေခံ compilation parameters များကို နားလည်ခြင်း (execution provider, precision)
- `inference_model.json` chat-template configuration ဖိုင်ကို ဖန်တီးနိုင်ခြင်း
- Compiled မော်ဒယ်တစ်ခုကို Foundry Local cache ထဲသို့ ထည့်နိုင်ခြင်း
- CLI၊ REST API နှင့် OpenAI SDK အသုံးပြု၍ custom မော်ဒယ်ကို inference ပြုလုပ်နိုင်ခြင်း

---

## မတိုင်မီလိုအပ်ချက်များ

| လိုအပ်ချက် | အသေးစိတ် |
|-------------|---------|
| **Foundry Local CLI** | ထည့်သွင်းပြီး တပ်ဆင်ထားပြီး `PATH` တွင်ပါရှိရန် ([အပိုင်း ၁](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI model builder အတွက် လိုအပ်သည် |
| **pip** | Python package မန်နေဂျာ |
| **ဒစ်စ့ခ် ဆိုင်ရာ အခမဲ့ နေရာ** | ရင်းမြစ်နှင့် compile ပြုလုပ်ထားသော မော်ဒယ်ဖိုင်များအတွက် အနည်းဆုံး ၅ GB အခမဲ့နေရာ |
| **Hugging Face အကောင့်** | မော်ဒယ်အချို့ကို ဒေါင်းလုပ်မရယူမီ လိုင်စင်ကိုလက်ခံရန် တောင်းဆိုခြင်း ရှိသည်။ Qwen3-0.6B သည် Apache 2.0 လိုင်စင်ကို အသုံးပြုပြီး အခမဲ့ရရှိနိုင်သည်။ |

---

## ပတ်ဝန်းကျင် ပြင်ဆင်ခြင်း

မော်ဒယ် compile ပြုလုပ်ရန် PyTorch, ONNX Runtime GenAI, Transformers စသည့် Python package များ အကြီးစား လိုအပ်ပါသည်။ သင့်စက်တွင် အသုံးမပြုနိုင်သော လိုအပ်ချက်များနှင့် ထိတွေ့မှု မရှိစေရန် အသီးသီး virtual environment တစ်ခုဖန်တီးပါ။

```bash
# ကိုင်တွယ်ရာတိုက် မိတ်ဆက်နေရာမှ
python -m venv .venv
```
  
ပတ်ဝန်းကျင်ကို ဖွင့်ပါ -

**Windows (PowerShell):**  
```powershell
.venv\Scripts\Activate.ps1
```
  
**macOS / Linux:**  
```bash
source .venv/bin/activate
```
  
pip ကို အဆင့်မြှင့်တင်ပါ၊ dependency ပြဿနာများရှောင်ရှားရန် -

```bash
python -m pip install --upgrade pip
```
  
> **အကြံပြုချက်:** မိတ်ဆွေမှာ ယခင် lab များမှ `.venv` ရှိပြီးသားဖြစ်ပါက ထပ်မံအသုံးပြုနိုင်သည်။ ဆက်လက်လုပ်ဆောင်မည်ဆိုပါက ပတ်ဝန်းကျင်ကို ဖွင့်ထားရန် သေချာစေရန်။

---

## ထောက်ခံချက်: Compilation Pipeline

Foundry Local သည် ONNX ပုံစံနှင့် ONNX Runtime GenAI configuration ရှိသည့် မော်ဒယ်များကို လိုအပ်သည်။ Hugging Face ပေါ်ရှိ အများလူသုံး open-source မော်ဒယ်များ ပုံမှန်အားဖြင့် PyTorch သို့မဟုတ် Safetensors အလေးချိန်အဖြစ် သတ်မှတ်ထားသည်။ ထို့ကြောင့် ရွေ့ပြောင်းရေးခြင်း လုပ်ငန်းစဉ်လိုအပ်သည်။

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### မော်ဒယ်တည်ဆောက်သူသည် ဘာများ လုပ်ပါသနည်း?

1. Hugging Face မှ ရင်းမြစ် မော်ဒယ်ကို ဒေါင်းလုပ်ဆွဲသည် (သို့မဟုတ် ဒေသတွင်ရှိသော ဖိုင်မှ ဖတ်သည်)။
2. PyTorch / Safetensors အလေးချိန်များကို ONNX ပုံစံသို့ ပြောင်းလဲသည်။
3. မော်ဒယ်ကို သိုလှောင်စမည့် memory များ လျော့ပါးစေရန်၊ အလျင်မြန်ဆုံးဖြစ်အောင် နည်းနည်းလားမှုရှိသော Precision (ဥပမာ int4) ဖြင့် quantise ပြုလုပ်သည်။
4. ONNX Runtime GenAI configuration (`genai_config.json`), chat template (`chat_template.jinja`), နှင့် tokeniser ဖိုင်များထုတ်ပေးသည်။ ဤအရာသည် Foundry Local တွင် မော်ဒယ်ကို load လုပ်နိုင်ရန် လိုအပ်ပါသည်။

### ONNX Runtime GenAI Model Builder နှင့် Microsoft Olive နှိုင်းယှဉ်ခြင်း

**Microsoft Olive** သည် မော်ဒယ်အဆင့်မြှင့်တင်ခြင်းအတွက် တခြားကိရိယာတစ်ခုအဖြစ် ရှိနိုင်သည်ကို တွေ့နေရမည်။ နှစ်ခုစလုံးသည် ONNX မော်ဒယ်များထုတ်ပေးနိုင်သော်လည်း ရည်ရွယ်ချက်၊ အသုံးပြုမှုနှင့် trade-off များကွာခြားသည်။

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **Package** | `onnxruntime-genai` | `olive-ai` |
| **ရည်ရွယ်ချက်အဓိက** | ONNX Runtime GenAI inference အတွက် generative AI မော်ဒယ်များကို ပြောင်းလဲနှင့် quantise ပြုလုပ်ခြင်း | အမျိုးမျိုးသော backends နှင့် hardware များအတွက် end-to-end မော်ဒယ် optimisation framework |
| **အသုံးပြုရလွယ်ကူမှု** | တစ်ချက် command ဖြင့် ပြောင်းလဲခြင်း + quantisation | workflow အခြေပြုနှစ်ကြိမ်ကျော် လုပ်ဆောင်မှုများကို YAML/JSON ဖြင့်ပြင်ဆင်နိုင်ခြင်း |
| **ထွက်ရှိပုံစံ** | ONNX Runtime GenAI ပုံစံ (Foundry Local အတွက် အသင့်ဖြစ်) | ဓါတ်ပုံများတကြိမ် - Generic ONNX, ONNX Runtime GenAI သို့မဟုတ် workflow အလိုက် အခြားပုံစံများ |
| **Hardware ပံ့ပိုးမှုများ** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN နှင့် အခြားများ |
| **Quantisation ရွေးချယ်စရာ** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16 နှင့် graph optimisations, layer-wise tuning |
| **မော်ဒယ် ကွက်တိ** | Generative AI မော်ဒယ်များ (LLMs, SLMs) | ONNX သို့ပြောင်းနိုင်သည့် မည်သည့် မော်ဒယ်မျိုးမဆို (vision, NLP, audio, multimodal) |
| **အကောင်းဆုံးအသုံးပြုမှု** | ဒေသဆိုင်ရာ inference အတွက် မော်ဒယ် တစ်ခုချင်း ရိုးရှင်းမြန်ဆန်သော compilation | ကြီးမားသည့် fine-grained optimisation လိုအပ်သော production pipeline များအတွက် |
| **လိုအပ်ခြင်းများ** | အလတ်စား (PyTorch, Transformers, ONNX Runtime) | ပိုကြီး (Olive framework နှင့် workflow အလိုက် optional extras ထပ်ဖြည့်) |
| **Foundry Local နှင့် ပေါင်းစပ်မှု** | တိုက်ရိုက် — output ကို ချက်ချင်းအသုံးပြုနိုင် | `--use_ort_genai` flag နှင့် configuration ထပ်လိုအပ်သည် |

> **ယခု lab တွင် Model Builder အသုံးပြုသော အကြောင်းရင်း:** Hugging Face မော်ဒယ် တစ်ခုကို compile ပြုလုပ်ကာ Foundry Local တွင် မှတ်ပုံတင်ရန်အတွက် Model Builder သည် အဆင့်ရိုးလှပြီးအာမခံရရှိသော နည်းလမ်းတစ်ခုဖြစ်သည်။ တစ်ချက် command ဖြင့် Foundry Local လိုအပ်သော output ကို ထုတ်ပေးနိုင်သည်။ နောက်ပိုင်းတွင် ကျွမ်းကျင်သော ကောင်းမွန်သော optimisation နည်းလမ်းများလိုအပ်ပါက (တိကျမှုအသိပေး quantisation, graph surgery, multi-pass tuning စသဖြင့်) Olive သည် စွမ်းရည်မြင့်သော ရွေးချယ်စရာဖြစ်ပါသည်။ အကြောင်းအရာလွန်များအတွက် [Microsoft Olive စာတမ်း](https://microsoft.github.io/Olive/) ကို ကြည့်ပါ။

---

## လက်တွေ့ လေ့ကျင့်ခန်းများ

### လေ့ကျင့်ခန်း ၁: ONNX Runtime GenAI Model Builder ထည့်သွင်းခြင်း

Model builder ကိရိယာပါသော ONNX Runtime GenAI package ကို ထည့်သွင်းပါ -

```bash
pip install onnxruntime-genai
```
  
ထည့်သွင်းပြီးသော model builder ရှိမှုကို စစ်ဆေးရန် -

```bash
python -m onnxruntime_genai.models.builder --help
```
  
`-m` (model name), `-o` (output path), `-p` (precision), `-e` (execution provider) စသည့် parameters များကို ဖော်ပြသော ကူညီချက် output ကို တွေ့ရမည်။

> **မှတ်ချက်:** model builder သည် PyTorch, Transformers နှင့် အခြားပက်ကေ့ဂျ်များအပေါ် မှီခိုသည်။ ထည့်သွင်းမှုသည် အချိန်အနည်းငယ်ယူနိုင်သည်။

---

### လေ့ကျင့်ခန်း ၂: CPU အတွက် Qwen3-0.6B Compile ပြုလုပ်ခြင်း

Hugging Face မှ Qwen3-0.6B မော်ဒယ်ကို ဒေါင်းလုပ်ဆွဲကာ int4 quantisation ဖြင့် CPU inference အတွက် compile ပြုလုပ်ရန် အောက်ပါ command ကို အသုံးပြုပါ -

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
  
#### အချက်လက္ ပြောကြားချက်များ

| အချက် | ရည်ရွယ်ချက် | အသုံးပြုသော တန်ဖိုး |
|-----------|---------|------------|
| `-m` | Hugging Face မော်ဒယ် ID သို့ ဒေသထဲ ဒိုင်ရက်ထရီလမ်းကြောင်း | `Qwen/Qwen3-0.6B` |
| `-o` | Compile ပြုလုပ်ထားသော ONNX မော်ဒယ် သိမ်းဆည်းမည့် ဒိုင်ရက်ထရီ | `models/qwen3` |
| `-p` | Compilation အတွင်း အသုံးပြုမည့် quantisation precision | `int4` |
| `-e` | ONNX Runtime execution provider (hardware ရည်ရွယ်ချက်) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face အတည်ပြုချက် ဖြတ်တောက်ခြင်း (public မော်ဒယ်များတွင်အဆင်ပြေ) | `hf_token=false` |

> **ဘယ်လောက်ကြာပါသလဲ?** compilation အချိန်သည် hardware နှင့် မော်ဒယ်အရွယ်အစား အပေါ် မူတည်သည်။ Qwen3-0.6B ဘက် int4 quantisation ဖြင့် စက်သစ်တစ်လုံးတွင် ၅ မှ ၁၅ မိနစ်တန်ဖိုးရှိပါသည်။ ကြီးမားသော မော်ဒယ်များသည် အချိန်ပိုတတ်သည်။

command ပြီးဆုံးပြီးပါက `models/qwen3` ဒိုင်ရက်ထရီအတွင်း compile ပြီး မော်ဒယ်ဖိုင်များ တွေ့ရှိရမည်။ output ကို စစ်ဆေးပါ -

```bash
ls models/qwen3
```
  
ပါဝင်နိုင်သည့် ဖိုင်များမှာ -  
- `model.onnx` နှင့် `model.onnx.data` — compile ပြီး အလေးချိန်ဖိုင်များ  
- `genai_config.json` — ONNX Runtime GenAI configuration  
- `chat_template.jinja` — မော်ဒယ်၏ chat template (auto-generated)  
- `tokenizer.json`, `tokenizer_config.json` — tokeniser ဖိုင်များ  
- အခြား vocabulary နှင့် configuration ဖိုင်များ

---

### လေ့ကျင့်ခန်း ၃: GPU အတွက် Compile (ရွေးချယ်စရာ)

NVIDIA GPU နှင့် CUDA ထောက်ပံ့မှုရှိပါက မြန်ဆန်သော inference အတွက် GPU အတွက် optimised variant ကို compile ပြုလုပ်နိုင်သည် -

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```
  
> **မှတ်ချက်:** GPU compile တွင် `onnxruntime-gpu` နှင့် CUDA installation မှန်ကန်မှု လိုအပ်သည်။ မရှိပါက model builder သည် error ပြမည်။ CPU variant နှင့် ဆက်လက်လုပ်ဆောင်နိုင်သည်။

#### Hardware အလိုက် Compilation ကို ညွှန်ကြားချက်

| ရည်ရွယ်ချက် | Execution Provider (`-e`) | အကြံပြု Precision (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` သို့ `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` သို့ `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### Precision ၏ အကျိုးကျေးဇူးနှင့် ဆိုးကျိုးများ

| Precision | အရွယ်အစား | အရှိန် | အရည်အချင်း |
|-----------|------|-------|---------|
| `fp32` | အကြီးဆုံး | အာမခံနိမ့်ဆုံး | အတိအကျအမြင့်ဆုံး |
| `fp16` | ကြီးမား | အမြန် (GPU) | အရည်အချင်း ကောင်းမွန် |
| `int8` | အသေး | အမြန် | အနည်းငယ် accuracy လျော့နည်းမှု |
| `int4` | အလွန်အသေး | အရှိန်အမြန်ဆုံး | အလယ်အလတ် accuracy လျော့နည်းမှု |

ဒေသစံတော်ချိန် တည်ဆောက်မှုများအတွက် `int4` CPU သည် အရှိန်နှင့် resource အသုံးပြုမှုတွင် ပိုမိုထိရောက်သည်။ ထုတ်လုပ်မှုအဆင့် output အတွက် GPU `fp16` ကို အကြံပြုသည်။

---

### လေ့ကျင့်ခန်း ၄: Chat Template Configuration ဖန်တီးခြင်း

Model builder သည် output directory တွင် `chat_template.jinja` နှင့် `genai_config.json` ဖိုင်များကို မော်ဒယ်အလိုက် ကိုယ်တိုင်း generate ပြုလုပ်ပေးသည်။ သို့သော် Foundry Local သည် prompt များကို ဘယ်လို format ပြုလုပ်မည်ကို သိရန် `inference_model.json` ဖိုင်ကိုလည်း လိုအပ်သည်။ ဤဖိုင်သည် မော်ဒယ်နာမည်နှင့် အသုံးပြုသူ၏ စကား မက်ဆေ့ကို အထူး token များဖြင့် ဖုံးအုပ်သည့် prompt template ကို သတ်မှတ်သည်။

#### အဆင့် ၁: Compile ပြီး Output အကြောင်းကြားချက်ကြည့်ရှုခြင်း

Compile ပြုလုပ်သော မော်ဒယ် ဒိုင်ရက်ထရီ အတွင်းဖိုင်များ စာရင်းပေး -

```bash
ls models/qwen3
```
  
ဖိုင်တွင်ရနိုင်သည့်အရာများ -  
- `model.onnx` နှင့် `model.onnx.data` — compile ပြီး မော်ဒယ်အလေးချိန်များ  
- `genai_config.json` — ONNX Runtime GenAI configuration (auto-generated)  
- `chat_template.jinja` — မော်ဒယ် chat template (auto-generated)  
- `tokenizer.json`, `tokenizer_config.json` — tokeniser ဖိုင်များ  
- အခြား configuration နှင့် vocabulary ဖိုင်များ

#### အဆင့် ၂: inference_model.json ဖိုင် တည်ဆောက်ခြင်း

`inference_model.json` ဖိုင်သည် Foundry Local တွင် prompt များကို ဘယ်လိုဖော်မျိုး ကျေရာ သေချာညဏ်ပေးသည်။ `models/` ဖိုင်းဒါ အတွင်းရှိ ရှိသူတို့ ကြားက repository ရှေ့လှမ်းမှု root တွင် `generate_chat_template.py` လို့ခေါ် Python script တစ်ခု ဖန်တီးပါ -

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# စကားပြောပုံစံကို ခွဲထုတ်ရန် အနိမ့်ဆုံး စကားပြောတစ်ခု တည်ဆောက်ပါ
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

# inference_model.json ဖွဲ့စည်းမှုကို တည်ဆောက်ပါ
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
  
repository root မှ script ကို run ပါ -

```bash
python generate_chat_template.py
```
  
> **မှတ်ချက်:** `transformers` package သည် `onnxruntime-genai` ထည့်သွင်းမှုအတွင်း ရှိပြီးသားဖြစ်သည်။ `ImportError` တွေ့ပါက `pip install transformers` ကို run ပါ။

script က `models/qwen3` directory အတွင်း `inference_model.json` ဖိုင်ကို ထုတ်ပေးမည်ဖြစ်သည်။ ၎င်းသည် Foundry Local အတွက် Qwen3 မော်ဒယ်အတွက် အသုံးပြုသူ input ကို အထူး token များဖြင့် ဖုံးအုပ်ရန် သတ်မှတ်ချက်များ ပါဝင်သည်။

> **အရေးကြီးသည်:** `inference_model.json` တွင် `"Name"` ကွက်လပ် (ဤ script တွင် `qwen3-0.6b` ဟု သတ်မှတ်ထားသည်) သည် နောက်ထပ် command များနှင့် API ခေါ်ဆိုမှုများတွင် သုံးမည့် မော်ဒယ်အမည် alias ဖြစ်သည်။ အမည်ပြောင်းလဲပါက လေ့ကျင့်ခန်း ၆ မှ ၁၀ အထိ မော်ဒယ်နာမည်ကို လိုက်ဖက် အပ်ဒိတ်ပြုလုပ်ပါ။

#### အဆင့် ၃: Configuration စစ်ဆေးခြင်း

`models/qwen3/inference_model.json` ကို ဖွင့်ကြည့်၍ `Name` အကွက်နှင့် `PromptTemplate` object ကို `assistant` နှင့် `prompt` အချက်အလက်များ ပါရှိသည်ကို သေချာစေရန်စစ်ပါ။ prompt template တွင် `<|im_start|>` နှင့် `<|im_end|>` ကဲ့သို့သော အထူး token များ ပါဝင်သင့်သည် (model ၏ chat template အပေါ်မှ မူတည်သည်)။

> **လက်စွဲနည်း အစားထိုး:** script မသုံးချင်ပါက လက်ဖြင့် ဖိုင်ကိုဖန်တီးနိုင်သည်။ အဓိကလိုအပ်ချက်မှာ `prompt` ကွက်တွင် မော်ဒယ်၏ စကား ပြောခြင်း template အား `{Content}` အဖြစ် အသုံးပြုသူ၏ မက်ဆေ့ကို အစားထိုးထား၍ တင်သွင်းထားရမည် ဖြစ်သည်။

---

### လေ့ကျင့်ခန်း ၅: မော်ဒယ် ဒိုင်ရက်ထရီ ဖွဲ့စည်းပုံ စစ်ဆေးခြင်း
model builder သည် သင် ဖော်ပြထားသော output directory ထဲသို့ compiled files အားလုံးကို တိုက်ရိုက် ထည့်သွင်းသည်။ နောက်ဆုံး အမှန်တကယ် ဖြစ်ပေါ်ထားသော ဖွဲ့စည်းမှုမှာ မှန်ကန်ကြောင်း အတည်ပြုပါ။

```bash
ls models/qwen3
```

directory တွင် အောက်ပါ ဖိုင်များ ပါဝင်သင့်သည်။

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

> **မှတ်ချက်။** အခြား compilation tools တချို့နှင့် မတူဘဲ၊ model builder သည် nested subdirectories မဖြစ်ပေါ်စေပါဘူး။ ဖိုင်အားလုံးကို output ဖိုလ်ဒါထဲမှာ တိုက်ရိုက် တည်ရှိစေပြီး၊ ၎င်းသည် Foundry Local ရဲ့ အကောင်းဆုံး မျှော်လင့်ချက်ဖြစ်သည်။

---

### မြင်သာလုပ်ငန်း ၆: Model ကို Foundry Local Cache ထဲသို့ ထည့်ရန်

သင့် compiled model ကို ရှာဖွေရန် Foundry Local သို့ directory ကို ၎င်း၏ cache ထဲထည့်ပါ။

```bash
foundry cache cd models/qwen3
```

model သည် cache တွင် ဖော်ပြနေကြောင်း အတည်ပြုပါ။

```bash
foundry cache ls
```

သင့် custom model ကို နှစ်ခါ မတိုင်မီ cache ထဲရှိနေသော မော်ဒယ်များ (ဥပမာ `phi-3.5-mini` သို့မဟုတ် `phi-4-mini`) နှင့်အတူ စာရင်းပြထားသင့်သည်။

---

### မြင်သာလုပ်ငန်း ၇: CLI ဖြင့် Custom Model ကို ဝင်ရောက်အသုံးပြုရန်

သင်ပြုလုပ်ပြီးသော compiled model ဖြင့် အပြန်အလှန် စကားပြော session တစ်ခုကို စတင်ပါ ( `qwen3-0.6b` alias သည် `inference_model.json` ထဲရှိ `Name` အကွက်မှ ဖြစ်သည်)။

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` flag သည် အပို diagnostic အချက်အလက်များကို ပြသကာ custom model ကို စစချင်းစမ်းသပ်ရာတွင် အထောက်အကူဖြစ်စေသည်။ model မှအောင်မြင်စွာ တင်သွင်းပါက ရှ interactive prompt ကိုမြင်ရမည်။ စာတိုများ စပါးကြံကြည့်ပါ။

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

`exit` ဟူသော စကားလုံးကို ရိုက်ထည့်ပါ သို့မဟုတ် `Ctrl+C` နှိပ်၍ session ကို ပြီးဆုံးပါ။

> **ပြဿနာဖြေရှင်းမှု။** Model တင်မရပါက အောက်ပါအချက်များကို စစ်ဆေးပါ။
> - `genai_config.json` ဖိုင်ကို model builder မှ ထုတ်လုပ်ထားပါသလား။
> - `inference_model.json` ဖိုင်တည်ရှိပြီး မှန်ကန်သော JSON ဖြစ်ပါသလား။
> - ONNX model ဖိုင်များသည် မှန်ကန်သော directory တွင် ရှိပါသလား။
> - ရနိုင်သော RAM လုံလောက်ပါသလား (Qwen3-0.6B int4 သည် လှမ်းနီးပါး 1 GB လိုအပ်သည်)။
> - Qwen3 သည် reasoning model တစ်ခုဖြစ်ပြီး `<think>` tags ထုတ်ပေးသည်။ `<think>...</think>` ကို မှတ်ချက်ခေါင်းစဉ်များ မည်သို့ဖြစ်လာသည်ကို တွေ့ရပါက ၎င်းသည် ပုံမှန်အပြုအမူဖြစ်သည်။ `inference_model.json` ထဲရှိ prompt template ကို အသုံးပြုပြင်၍ ထွက်ရှိသော အတွေးများကို ဖျောက်ပိတ်နိုင်သည်။

---

### မြင်သာလုပ်ငန်း ၈: REST API မှာ Custom Model ကို မေးမြန်းသုံးစွဲရန်

မြင်သာလုပ်ငန်း ၇ တွင် session ကို ပြီးစီးပြီးပါက model ကို ပြန်တင်ထားမှု မရှိနိုင်ပါဘူး။ Foundry Local service ကို စတင်ပြီး model ကို ပထမဦးစွာ ထည့်သွင်းပါ။

```bash
foundry service start
foundry model load qwen3-0.6b
```

service သည် ဘယ် port ပေါ်မှာ ပြေးနေသည်ကို စစ်ဆေးပါ။

```bash
foundry service status
```

ထို့နောက် request တစ်ခု ပို့ပါ (သင့် actual port ကို `5273` နှင့် မတူပါက အစားထိုးပါ)။

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows မှတ်ချက်။** အထက်ပါ `curl` command သည် bash စာတမ်းဖြစ်သည်။ Windows တွင်တော့ PowerShell ၏ `Invoke-RestMethod` cmdlet ကို အောက်ပါအတိုင်း အသုံးပြုပါ။

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

### မြင်သာလုပ်ငန်း ၉: OpenAI SDK ဖြင့် Custom Model ကို အသုံးပြုရန်

သင်သည် built-in models များအတွက် အသုံးပြုပြီးသား OpenAI SDK ကို တူညီပုံစံနဲ့ သင့် custom model နှင့် ချိတ်ဆက်နိုင်သည် (ကြည့်ရှုရန် [Part 3](part3-sdk-and-apis.md))။ ကွဲပြားချက်မှာ model name ပဲ ဖြစ်သည်။

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry Local သည် API key များကို အတည်ပြုခြင်း မပြုလုပ်ပါ။
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
  apiKey: "foundry-local", // Foundry Local သည် API keys မမှန်ကန်မှုကို စစ်ဆေးမှုမပြုလုပ်ပါ။
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

> **အဓိက အချက်။** Foundry Local သည် OpenAI-ကိုက်ညီသော API ကို ပေးသည့်အတွက် built-in models များနှင့်အတူ အလုပ်လုပ်သော code များသည် သင့် custom models များဖြင့်လည်း အလုပ်လုပ်ပါမည်။ `model` parameter ကိုသာ ပြောင်းလဲရန် လိုသည်။

---

### မြင်သာလုပ်ငန်း ၁၀: Foundry Local SDK ဖြင့် Custom Model ကို စမ်းသပ်ရန်

အစောပိုင်း lab များတွင် Foundry Local SDK ကို အသုံးပြုပြီး service စတင်ခြင်း၊ endpoint ရှာဖွေရေးနှင့် model များကို automatic စီမံခန့်ခွဲခြင်း ပြုလုပ်ခဲ့သည်။ သင့် custom-compiled model ကိုလည်း တူညီသော ပုံစံနဲ့ လိုက်နာနိုင်သည်။ SDK သည် service စတင်ခြင်းနှင့် endpoint ရှာဖွေရေးကို ကိုယ်တိုင် ဆောင်ရွက်သည်၊ ထို့ကြောင့် သင့် code တွင် `localhost:5273` ကို hard-code လုပ်ရန် မလိုပါ။

> **မှတ်ချက်။** ဤဥပမာများကို ပြေးရန် မတိုင်မှီ Foundry Local SDK ကို ထည့်သွင်းထားကြောင်း သေချာပါစေ။
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` နှင့် `OpenAI` NuGet packages များထည့်သွင်းခြင်း
>
> script ဖိုင်တိုင်းကို **repository အပေါ်ဆုံး directory** (သင့် `models/` ဖိုလ်ဒါနှင့်တူ directory) တွင် သိမ်းပါ။

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# အဆင့် ၁: Foundry Local ဝန်ဆောင်မှုကို စတင်ပြီး စိတ်ကြိုက်မော်ဒယ်ကို အခြားလူငယ်များဖြင့် ပြန်လည်တင်ပါ
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# အဆင့် ၂: စိတ်ကြိုက်မော်ဒယ်အတွက် cache ကို စစ်ဆေးပါ
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# အဆင့် ၃: မော်ဒယ်ကို မှတ်ဉာဏ်ထဲသို့တင်ပါ
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# အဆင့် ၄: SDK ဖြင့် ရှာဖွေတွေ့ရှိထားသော endpoint ကို သုံးပြီး OpenAI client ကို ဖန်တီးပါ
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# အဆင့် ၅: streaming chat completion တောင်းဆိုချက်တစ်ခုကို ပို့ပါ
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

ပြေးရန် -

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

// အဆင့် ၁: Foundry Local ဝန်ဆောင်မှုကို စတင်ပါ
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// အဆင့် ၂: စာရင်းမှ စိတ်ကြိုက် မော်ဒယ်ကို ရယူပါ
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// အဆင့် ၃: မော်ဒယ်ကို မှတ်ဉာဏ်ထဲသို့ တွဲသွင်းပါ
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// အဆင့် ၄: SDK ရှာဖွေတွေ့ရှိသည့် endpoint ကို အသုံးပြုပြီး OpenAI client ကို ဖန်တီးပါ
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// အဆင့် ၅: Streaming chat completion ဆက်တင် မေးမြန်းမှု တင်ပို့ပါ
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

ပြေးရန် -

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

> **အဓိက အချက်။** Foundry Local SDK သည် endpoint ကို dynamic အနေဖြင့် ရှာဖွေသည်၊ ထို့ကြောင့် port number ကို ဆက်တိုက် hard-code မလုပ်ပါ။ ၎င်းတွင် အမြဲအကောင်းဆုံးရှိသော production applications ရည်ရွယ်ချက်ဖြစ်သည်။ သင့် custom-compiled model သည် built-in catalogue မော်ဒယ်များနှင့် တူညီစွာ SDK ဖြင့် အလုပ်လုပ်ပါသည်။

---

## Compile လုပ်ရန် Model ရွေးချယ်ခြင်း

ဤ lab တွင် Qwen3-0.6B ကို အရည်အသွေးကောင်း၊ compile လျင်မြန်၊ Apache 2.0 အောက်တွင် လွတ်လပ်စွာ ရရှိနိုင်သော နမူနာအဖြစ် အသုံးပြုသည်။ သို့သော် နောက် models များစွာကိုလည်း compile လုပ်နိုင်သည်။ အောက်တွင် အကြံပြုချက်များ ပါရှိပါသည်။

| Model | Hugging Face ID | Parameters | Licence | မှတ်ချက်များ |
|-------|-----------------|------------|---------|--------------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | အလွန်သေးငယ်ပြီး compile လျင်မြန်၊ စမ်းသပ်ရန်သင့်တော်သည် |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | အရည်အသွေးပိုကောင်း၊ compile လျင်မြန်သည် |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | အရည်အသွေးสูงပြင်း၊ RAM ပိုလိုအပ်သည် |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face တွင် licence လက်ခံရယူရန် လိုအပ်သည် |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | အရည်အသွေးမြင့်၊ အရေအတွက်ကြီးပြီး download နှင့် compile ပိုကြာသည် |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Foundry Local catalogue တွင် ရှိပြီး (နှိုင်းယှဉ်ရာတွင် အသုံးဝင်သည်) |

> **Licence မှတ်ချက်။** အသုံးပြုမည့် Model ၏ Licence ကို Hugging Face မှာ အမြဲစစ်ဆေးပါ။ အချို့ models (Llama ကဲ့သို့) မှ licence သဘောတူချက် လက်ခံရန် နှင့် `huggingface-cli login` ဖြင့် အတည်ပြုရပါမည်။

---

## သတိပြုရန် အကြောင်းအရာများ: Custom Models ကို ဘယ်အချိန် အသုံးပြုမည်နည်း

| နည်းလမ်း | မည်သည့်အကြောင်းပြချက်နဲ့ အလိုက် Compile လုပ်ရမည်နည်း |
|----------|-------------------------------------------|
| **လိုအပ်သည့် model သည် catalogue တွင် မရှိပါက** | Foundry Local catalogue သည် စီစဉ်ပြီးဖြစ်ပါသည်။ သင်လိုချင်သော model ထည့်မှာ မပါပါက တစ်ကိုယ်တော် compile လုပ်ပါ။ |
| **Fine-tuned models များ** | Domain-specific ဒေတာပေါ်တွင် fine-tune ပြုလုပ်ထားသည့် model များမှာ ကိုယ်ပိုင် weights များ compile လုပ်ရန် လိုအပ်သည်။ |
| **အထူး quantisation လိုအပ်ချက်များ** | catalogue ပုံမှန် strategy မတူညီသော precision သို့မဟုတ် quantisation လုပ်မှုလိုလားလျှင် ကိုယ်ပိုင် Compile လုပ်ပါ။ |
| **နောက်ဆုံးထွက် model များ** | Hugging Face တွင် model အသစ် ထွက်ရှိလျှင် Foundry Local catalogue မူလတွင် မပါနိုင်သေးပါ။ ကိုယ်ပိုင် compile လုပ်ခြင်းအားဖြင့် ချက်ချင်းရရှိနိုင်သည်။ |
| **သုတေသနနှင့် စမ်းသပ်ခြင်းများ** | မတူညီသော architecture, အရွယ်အစား သို့မဟုတ် configurations များကို locally စမ်းသပ်သုံးစွဲချင်ပါက။ |

---

## အနှစ်ချုပ်

ဤ lab တွင် သင်သည် -

| အဆင့် | သင်လုပ်ဆောင်ခဲ့သည်များ |
|--------|-----------------------|
| 1 | ONNX Runtime GenAI model builder ကို တပ်ဆင်ခဲ့သည် |
| 2 | Hugging Face ကနေ `Qwen/Qwen3-0.6B` ကို optimized ONNX model ထဲသို့ compiled ပြုလုပ်ခဲ့သည် |
| 3 | `inference_model.json` chat-template configuration file ကို ဖန်တီးခဲ့သည် |
| 4 | compiled model ကို Foundry Local cache ထဲ ထည့်သွင်းခဲ့သည် |
| 5 | CLI ဖြင့် custom model နှင့် အပြန်အလှန် စကားပြောပွဲ ပြုလုပ်ခဲ့သည် |
| 6 | OpenAI-compatible REST API မှတဆင့် model ကို မေးမြန်းခဲ့သည် |
| 7 | Python, JavaScript, C# မှ OpenAI SDK ဖြင့် ချိတ်ဆက်ခဲ့သည် |
| 8 | Foundry Local SDK ဖြင့် custom model ကို end-to-end စမ်းသပ်ခဲ့သည် |

အဓိက သင်ယူချက်မှာ **transformer-based models အားလုံးကို Foundry Local မှ ONNX format ပြောင်းပြီး အဆင်ပြေစွာ run နိင်သည်**။ OpenAI-compatible API ဖြစ်သောကြောင့် သင်၏ ရှိပြီးသား application code မပြောင်းလဲဘဲ ရပါသည်။ model name တစ်ခုတည်း ပြောင်းလဲရုံဖြစ်သည်။

---

## အဓိက သင်ယူချက်များ

| အကြောင်းအရာ | အသေးစိတ် |
|--------------|-----------|
| ONNX Runtime GenAI Model Builder | Hugging Face models များကို ONNX format သို့ ပြောင်းပြီး quantisation အပြင်တစ်ခါပြုလုပ်သည် |
| ONNX format | Foundry Local သည် ONNX Runtime GenAI configuration ပါသော ONNX models လိုအပ်သည် |
| Chat templates | `inference_model.json` ဖိုင်က Foundry Local ကို မည်သို့ prompts ဖော်စပ်ရမည်ကို ရှင်းပြသည် |
| Hardware targets | သင်၏ hardware အမျိုးအစားအလိုက် CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU) သို့မဟုတ် WebGPU တို့အတွက် compile ပြုလုပ်နိုင်သည် |
| Quantisation | precision နည်း (int4) သည် အရွယ်အစား လျော့ပေါ့ကာ မြန်ဆန်စေသော်လည်း တိကျမှုနည်းသည်။ fp16 သည် GPU များပေါ်တွင် အရည်အသွေးမြင့်စေသည် |
| API compatibility | Custom models များသည် built-in models များနှင့် တူညီသော OpenAI-compatible API ကို အသုံးပြုသည် |
| Foundry Local SDK | SDK သည် service စတင်ခြင်း၊ endpoint ရှာဖွေရေးနှင့် model loading ကို catalogue နှင့် custom models အတွက် အလိုအလျောက် ဆောင်ရွက်ပေးသည် |

---

## နောက်ထပ် ဖတ်ရှုရန်

| အရင်းအမြစ် | Link |
|-------------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local custom model guide | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 model family | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive documentation | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## နောက်တစ်ဆင့်များ

သင်၏ local models များကို ပြင်ပ function များကို ခေါ်ဆိုနိုင်ရန် [Part 11: Tool Calling with Local Models](part11-tool-calling.md) မှ ဆက်လက်လေ့လာပါ။

[← Part 9: Whisper Voice Transcription](part9-whisper-voice-transcription.md) | [Part 11: Tool Calling →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**မကြာခဏ မှတ်စု**  
ဤစာတမ်းကို AI ဘာသာပြန်၀န်ဆောင်မှု [Co-op Translator](https://github.com/Azure/co-op-translator) ဖြင့် ဘာသာပြန်ထားပါသည်။ တိကျမှန်ကန်မှုအတွက် ကြိုးစားပေမယ့် အလိုအလျောက် ဘာသာပြန်ချက်များတွင် အမှားများ သို့မဟုတ် မှားယွင်းမှုများ ပါဝင်နိုင်ကြောင်း သတိပြုရန် လိုအပ်ပါသည်။ မူရင်းစာတမ်းကို ၎င်း၏ မူလဘာသာဖြင့် အတိုင်ပင်ခံ အရင်းအမြစ်အဖြစ် ယူဆရန်လိုအပ်ပါသည်။ အရေးကြီးသတင်းအချက်အလက်များအတွက် လူ့ဘာသာပြန်ပညာရှင်လုပ်ငန်းတာဝန်ခံဖြင့် ဘာသာပြန်ခြင်းကို အကြံပြုပါသည်။ ဤဘာသာပြန်ချက်ကို အသုံးပြုမှုကြောင့် ဖြစ်ပေါ်နိုင်သည့် အနားလွဲချက်များ သို့မဟုတ် မှားယွင်းဖတ်ရှုမှုများအတွက် ကျွန်ုပ်တို့သည် တာဝန်မရှိပါ။
<!-- CO-OP TRANSLATOR DISCLAIMER END -->