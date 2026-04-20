![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ភាគទី ១០៖ ការប្រើម៉ូដែល Custom ឬ Hugging Face ជាមួយ Foundry Local

> **គោលបំណង៖** បង្កើតម៉ូដែល Hugging Face ទៅជា ONNX ដែលបានបង្រួមរួច ដែល Foundry Local តម្រូវការ កំណត់រចនាសម្ព័ន្ធវាជាមួយទំរង់សន្ទនា ចម្លងវាទៅកាន់ស្តុកទិន្នន័យនៅក្នុងកន្លែង និងដំណើរការសំណួរតាមរយៈ CLI, REST API និង OpenAI SDK ។

## សង្ខេប

Foundry Local ផ្តល់ជាមួយបញ្ជីម៉ូដែលដែលបានបបួលរួច ក៏ប៉ុន្តែមិនមានកំណត់ត្រឹមតែបញ្ជីនោះទេ។ ម៉ូដែលភាសាដែលមានមូលដ្ឋានលើ Transformer ដែលមាននៅលើ [Hugging Face](https://huggingface.co/) (ឬផ្ទុកក្នុងទ្រង់ទ្រាយ PyTorch / Safetensors លើកុំព្យូទ័រផ្ទាល់) អាចត្រូវបានបង្កើតជា ONNX ដែលបានបង្រួមរួចហើយ និងបម្រើតាមរយៈ Foundry Local។

ប៉ាយវេលធំនឹងប្រើសំភារៈ **ONNX Runtime GenAI Model Builder** ជា​ឧបករណ៍កំណត់បន្ទាត់ការកម្មង់ ដែលមានក្នុងកញ្ចប់ `onnxruntime-genai` ។ អ្នកបង្កើតម៉ូដែលនឹងដោះស្រាយការងារដ៏ធំបំផុត៖ ទាញយកទម្ងន់ម៉ូដែល ដំណើរការបម្លែងទៅទ្រង់ទ្រាយ ONNX អនុវត្ត Quantisation (int4, fp16, bf16) ហើយបញ្ចេញឯកសារតំរូវការរួមមានទំរង់សន្ទនា និង tokeniser ដែល Foundry Local ប្រើ។

នៅក្នុងមន្ទីរពិសោធន៍នេះ អ្នកនឹងបង្កើតម៉ូដែល **Qwen/Qwen3-0.6B** ពី Hugging Face កត់ត្រាវាជាមួយ Foundry Local ហើយពិភាក្សាជាមួយវាពេញលេញលើឧបករណ៍របស់អ្នក។

---

## គោលបំណងការសិក្សា

នៅចុងបញ្ចប់នៃមន្ទីរពិសោធន៍នេះ អ្នកនឹងអាច៖

- ពន្យល់ពីមូលហេតុដែលការបង្កើតម៉ូដែលផ្ទាល់ខ្លួនមានប្រយោជន៍ និងពេលណាអ្នកត្រូវការវា
- តំឡើងកម្មវិធី ONNX Runtime GenAI model builder
- បង្កើតម៉ូដែល Hugging Face ទៅទ្រង់ទ្រាយ ONNX ដែលបានបង្រួមរួច ជាមួយពាក្យបញ្ជា​តែមួយ
- យល់ដឹងពីប៉ារ៉ាម៉ែត្រសំខាន់ៗក្នុងការបង្កើត (execution provider, precision)
- បង្កើតឯកសារ `inference_model.json` ដែលជាទំរង់សន្ទនា
- បន្ថែមម៉ូដែលដែលបានបង្កើតពិសេសទៅស្តុកទិន្នន័យ Foundry Local
- ដំណើរការសំណួរពីម៉ូដែលជាមួយ CLI, REST API និង OpenAI SDK

---

## លក្ខខ័ណ្ឌតម្រូវការ

| តម្រូវការ | ព័ត៌មានលម្អិត |
|-------------|---------|
| **Foundry Local CLI** | ត្រូវបានដំឡើងហើយមាននៅលើ `PATH` របស់អ្នក ([ភាគ​ ១](part1-getting-started.md)) |
| **Python 3.10+** | តម្រូវដោយ ONNX Runtime GenAI model builder |
| **pip** | អ្នកគ្រប់គ្រងកញ្ចប់ Python |
| **ទំហំទិន្នន័យឌីស** | យ៉ាងហោចណាស់ ៥ GB សម្រាប់ទិន្នន័យដើម និងម៉ូដែលដែលបានបង្កើត |
| **គណនី Hugging Face** | ម៉ូដែលមួយចំនួនត្រូវការអោយទទួលយកអាជ្ញាប័ណ្ណមុនទាញយក។ Qwen3-0.6B ប្រើអាជ្ញាប័ណ្ណ Apache 2.0 និងអាចប្រើប្រាស់ដាច់ដោយឡែក។ |

---

## ការតំឡើងបរិស្ថាន

ការបង្កើតម៉ូដែលត្រូវការកញ្ចប់ Python ដ៏ធំនានា (PyTorch, ONNX Runtime GenAI, Transformers)។ បង្កើតបរិស្ថានច្បាស់លាស់ដើម្បីជៀសវាងបញ្ហានៅក្នុង Python ប្រព័ន្ធរបស់អ្នក ឬគម្រោងផ្សេងៗ។

```bash
# ពីដើមហាង​រក្សាទុក​ឯកសារ
python -m venv .venv
```

បើកបរិស្ថាន៖

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

ធ្វើបច្ចុប្បន្នភាព pip ដើម្បីជៀសវាងបញ្ហាខ្សែការពារ៖

```bash
python -m pip install --upgrade pip
```

> **កំណត់ចំណាំ៖** ប្រសិនបើអ្នកមាន `.venv` ពីមុន នៅក្នុងមន្ទីរពិសោធន៍កន្លងមក អ្នកអាចប្រើវាឡើងវិញ។ តែបញ្ជាក់ថាវាត្រូវបានដំណើរការឡើងវិញមុនបន្ត។

---

## គំនិត៖ ប៉ាយវេលការបង្កើត

Foundry Local ត្រូវការម៉ូដែលក្នុងទ្រង់ទ្រាយ ONNX ជាមួយការកំណត់រចនាសម្ព័ន្ធ ONNX Runtime GenAI។ ម៉ូដែលភាគច្រើននៅលើ Hugging Face ត្រូវបានផ្ដល់ជាទំរង់ទម្ងន់ PyTorch ឬ Safetensors ដូច្នេះត្រូវការការបម្លែង។

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### អ្វីដែល Model Builder ធ្វើ?

1. **ទាញយក** ម៉ូដែលដើមពី Hugging Face (ឬអានពីផ្លូវទីតាំងក្នុងកុំព្យូទ័រ)។
2. **បម្លែង** ទម្ងន់ PyTorch / Safetensors ទៅ ONNX។
3. **បង្ហាប់** ម៉ូដែលឲ្យមានតូចជាង (ដូចជា int4) ដើម្បីកាត់បន្ថយការប្រើអង្គចងចាំ និងបង្កើនល្បឿន។  
4. **បញ្ចេញ** កំណត់រចនាសម្ព័ន្ធ ONNX Runtime GenAI (`genai_config.json`), ទំរង់សន្ទនា (`chat_template.jinja`), និងឯកសារទាំងអស់នៃtokeniser ដើម្បី Foundry Local អាចផ្ទុក និងបម្រើម៉ូដែល។

### ONNX Runtime GenAI Model Builder និង Microsoft Olive

អ្នកអាចជួបឯកសារឆ្ពោះទៅ **Microsoft Olive** ជាឧបករណ៍ជំនួសសម្រាប់បង្រួមម៉ូដែល។ ឧបករណ៍ទាំងពីរនេះអាចផលិតម៉ូដែល ONNX តែមានគោលបំណងនិងការប្រើប្រាស់ខុសគ្នា៖

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **កញ្ចប់** | `onnxruntime-genai` | `olive-ai` |
| **គោលបំណងចម្បង** | បម្លែងនិងបង្ហាប់ម៉ូដែល AI បង្កើតសម្រាប់ ONNX Runtime GenAI | សំណុំប្រព័ន្ធបង្រួមម៉ូដែលពីដើមដល់ចុង គាំទ្រតំបន់ក្រោយ និងគោលដៅរាប់រៀល |
| **ងាយស្រួលប្រើ** | ការបញ្ជាលើកតែមួយ — បម្លែង និងបង្ហាប់ជាមួយគ្នា | គ្រប់គ្រងវដ្តការងារ — ប៉ាយវេលច្រើនជាជំហានជាមួយ YAML/JSON |
| **ទ្រង់ទ្រាយចេញ** | ONNX Runtime GenAI (សម្រាប់ Foundry Local) | ONNX ទូទៅ, ONNX Runtime GenAI, ឬទ្រង់ទ្រាយផ្សេងទៀតតាមលំដាប់ការងារ |
| **គោលដៅបរិមាណកុំព្យូទ័រ** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, និងផ្សេងៗ |
| **ជម្រើសបង្ហាប់** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, ព្រមទាំងបង្កើតក្រាប, ការកែតម្រូវក្រាបជាក្រឡា  |
| **វិសាលភាពម៉ូដែល** | ម៉ូដែល AI បង្កើត (LLMs, SLMs) | ម៉ូដែលដែលអាចបម្លែងទៅ ONNX (ចុងភៅ, NLP, សំឡេង, មេមូលទី) |
| **សាកសមសម្រាប់** | ការបង្កើតម៉ូដែលតែមួយសម្រាប់តេស្តលើកុំព្យូទ័រផ្ទាល់ | បម្រែបម្រួលកម្រិតខ្នាតម៉ូដែលសម្រាប់ផលិតកម្ម |
| **ការពាក់ព័ន្ធថាមពល** | មធ្យម (PyTorch, Transformers, ONNX Runtime) | ធំជាង (បន្ថែមប្រែកម្មវិធី Olive, ជម្រើសបន្ថែមតាមវដ្ត) |
| **ការចំរូងជាមួយ Foundry Local** | ត្រូវបានគ្រប់គ្រងដោយផ្ទាល់ — លទ្ធផលតូចជាមួយ Foundry Local | ត្រូវការទង់ភ្ជាប់ `--use_ort_genai` និងកំណត់រចនាសម្ព័ន្ធបន្ថែម |

> **ហេតុអ្វីបានជាមន្ទីរពិសោធន៍នេះប្រើ Model Builder:** សម្រាប់បញ្ហាបង្កើតម៉ូដែល Hugging Face តែមួយ និងចុះបញ្ជីវាជាមួយ Foundry Local, Model Builder គឺជាវិធីងាយស្រួល និងទុកចិត្តបានបំផុត។ វាបង្ហាញលទ្ធផលដូចដែល Foundry Local រង់ចាំក្នុងមួយពាក្យបញ្ជា។ ប្រសិនបើអ្នកត្រូវការជម្រើសបង្រួមខ្ពស់ដូចជា quantisation បានៅត្រឹមត្រូវ, ការកែសម្រួលក្រាប, ឬការតំឡើងវដ្តហ្វាល — Olive ជាជម្រើសខ្លាំង។ សូមមើលឯកសាររបស់ [Microsoft Olive](https://microsoft.github.io/Olive/) សម្រាប់ព័ត៌មានលម្អិត។

---

## ផ្នែកហ្វឹកហាត់មន្ទីរពិសោធន៍

### អនុវត្តន៍ ១៖ តំឡើង ONNX Runtime GenAI Model Builder

តំឡើងកញ្ចប់ ONNX Runtime GenAI ដែលរួមមានកម្មវិធីបង្កើតម៉ូដែល៖

```bash
pip install onnxruntime-genai
```

ផ្ទៀងផ្ទាត់ការតំឡើងដោយពិនិត្យមើលថាកម្មវិធីបង្កើតម៉ូដែលអាចប្រើបាន៖

```bash
python -m onnxruntime_genai.models.builder --help
```

អ្នកគួរតែឃើញលទ្ធផលជំនួយដែលបង្ហាញប៉ារ៉ាម៉ែត្រ ដូចជា `-m` (ឈ្មោះម៉ូដែល), `-o` (ផ្លូវចេញ), `-p` (ភាពត្រឹមត្រូវ), និង `-e` (execution provider)។

> **កំណត់ចំណាំ៖** កម្មវិធីបង្កើតម៉ូដែលពឹងផ្អែកលើ PyTorch, Transformers, និងកញ្ចប់ផ្សេងៗច្រើនទៀត។ ការតំឡើងអាចចំណាយពេលប៉ុន្មាននាទី។

---

### អនុវត្តន៍ ២៖ បង្កើត Qwen3-0.6B សម្រាប់ CPU

ដំណើរការពាក្យបញ្ជារខាងក្រោមដើម្បីទាញយកម៉ូដែល Qwen3-0.6B ពី Hugging Face និងបង្កើតវាសម្រាប់ CPU ជាមួយ Quantisation ដែលជ int4៖

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

#### ប៉ារ៉ាម៉ែត្រពីរ

| ប៉ារ៉ាម៉ែត្រ | គោលបំណង | តម្លៃ |
|-----------|---------|------------|
| `-m` | ID ម៉ូដែល Hugging Face ឬផ្លូវទីតាំងក្នុងកុំព្យូទ័រ | `Qwen/Qwen3-0.6B` |
| `-o` | ឃ្លាំបង្ហាញម៉ូដែល ONNX ដែលបានបង្កើត | `models/qwen3` |
| `-p` | ភាពត្រឹមត្រូវរបស់ quantisation | `int4` |
| `-e` | execution provider របស់ ONNX Runtime (ផ្នែករ៉ូបូទ័រ) | `cpu` |
| `--extra_options hf_token=false` | លែងការផ្ទៀងផ្ទាត់ Hugging Face (ល្អសម្រាប់ម៉ូដែលសាធារណៈ) | `hf_token=false` |

> **តើចំណាយពេលប៉ុន្មាន?** ពេលវេលាកំណត់បង្កើតអាស្រ័យលើគ្រឿងម៉ាស៊ីន និងទំហំម៉ូដែល។ សម្រាប់ Qwen3-0.6B ជាមួយ quantisation int4 លើ CPU សម័យថ្មី គួរតែប្រហែល ៥ ទៅ ១៥ នាទី។ ម៉ូដែលធំៗចំណាយពេលយូរ។

បន្ទាប់ពីពាក្យបញ្ជាចប់ អ្នកគួរមើលឃើញថាបានមានថត `models/qwen3` ដែលមានឯកសារម៉ូដែលបានបង្កើត។ ផ្ទៀងផ្ទាត់លទ្ធផល៖

```bash
ls models/qwen3
```

អ្នកគួរតែឃើញឯកសារដូចជា៖
- `model.onnx` និង `model.onnx.data` — ទម្ងន់ម៉ូដែលដែលបានបង្កើត
- `genai_config.json` — កំណត់រចនាសម្ព័ន្ធ ONNX Runtime GenAI
- `chat_template.jinja` — ទំរង់សន្ទនារបស់ម៉ូដែល (បង្កើតដោយស្វ័យប្រវត្តិ)
- `tokenizer.json`, `tokenizer_config.json` — ឯកសារសម្រាប់ tokeniser
- ឯកសារពាក្យ និងកំណត់រចនាសម្ព័ន្ធផ្សេងៗ

---

### អនុវត្តន៍ ៣៖ បង្កើតសម្រាប់ GPU (ជាជម្រើស)

បើអ្នកមានអ៊ិនធឺហ្ស៊ីត GPU NVIDIA ដែលគាំទ្រដោយ CUDA អ្នកអាចបង្កើតម៉ូដែលដែលបង្រួមសម្រាប់ GPU ដើម្បីមានល្បឿនបង្ហាញលទ្ធផលលឿនជាង៖

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **កំណត់ចំណាំ៖** ការបង្កើតម៉ូដែលសម្រាប់ GPU ត្រូវការកញ្ចប់ `onnxruntime-gpu` និងការតំឡើង CUDA ដំណើរការបាន។ ប្រសិនបើមិនមាន វានឹងរាយការណ៍កំហុស។ អ្នកអាចលើកលែងមិនធ្វើការហ្វឹកហាត់នេះហើយបន្តជាមួយវ៉ារីយ៉ង់ CPU។

#### តារាងយោងកំណត់សំរាប់សមាសភាគរឹង

| គោលដៅ | execution provider (`-e`) | ភាពត្រឹមត្រូវដែលបានផ្ដល់អនុសាសន៍ (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| GPU NVIDIA | `cuda` | `fp16` ឬ `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` ឬ `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### ការពិពណ៌នាភាពត្រឹមត្រូវ

| ភាពត្រឹមត្រូវ | ទំហំ | ល្បឿន | គុណភាព |
|-----------|------|-------|---------|
| `fp32` | ធំបំផុត | ខ្សោយបំផុត | ត្រឹមត្រូវខ្ពស់បំផុត |
| `fp16` | ធំ | លឿន (GPU) | ត្រឹមត្រូវល្អណាស់ |
| `int8` | តូច | លឿន | បាត់បង់ត្រឹមត្រូវស្រាល |
| `int4` | តូចបំផុត | លឿនបំផុត | បាត់បង់ត្រឹមត្រូវមធ្យម |

សម្រាប់ការអភិវឌ្ឍក្នុងផ្ទៃ ការប្រើ `int4` លើ CPU ផ្តល់ការសមតុល្យល្អបំផុតរវាងល្បឿន និងប្រើប្រាស់ធនធាន។ សម្រាប់លទ្ធផលគុណភាពផលិតកម្ម អ្នកត្រូវប្រើ `fp16` លើ CUDA GPU។

---

### អនុវត្តន៍ ៤៖ បង្កើតកំណត់រចនាសម្ព័ន្ធទំរង់សន្ទនា

កម្មវិធីបង្កើតម៉ូដែលបង្កើតឯកសារ `chat_template.jinja` និង `genai_config.json` នៅក្នុងថតលទ្ធផលដោយស្វ័យប្រវត្តិ។ ប៉ុន្តែ Foundry Local ត្រូវការឯកសារ `inference_model.json` ផងដើម្បីយល់ពីរបៀបបម្រុងសំណើសម្រាប់ម៉ូដែលរបស់អ្នក។ ឯកសារនេះកំណត់ឈ្មោះម៉ូដែល និងទំរង់សំណើដែលរួមបញ្ចូលសារ​អ្នកប្រើជាមួយនឹងសញ្ញាពិសេសត្រឹមត្រូវ។

#### ជំហាន ១៖ ពិនិត្យលទ្ធផលម៉ូដែលដែលបានបង្កើត

បញ្ជីមើលថាតើនៅក្នុងថតម៉ូដែលប្រកបដោយភាពបង្កើតមានអ្វីខ្លះ៖

```bash
ls models/qwen3
```

អ្នកគួរតែឃើញឯកសារដូចជា៖
- `model.onnx` និង `model.onnx.data` — ទម្ងន់ម៉ូដែលដែលបានបង្កើត
- `genai_config.json` — កំណត់រចនាសម្ព័ន្ធ ONNX Runtime GenAI (បង្កើតដោយស្វ័យប្រវត្តិ)
- `chat_template.jinja` — ទំរង់សន្ទនារបស់ម៉ូដែល (បង្កើតដោយស្វ័យប្រវត្តិ)
- `tokenizer.json`, `tokenizer_config.json` — ឯកសារសម្រាប់ tokeniser
- ឯកសារកំណត់រចនាសម្ព័ន្ធ និងពាក្យផ្សេងៗ

#### ជំហាន ២៖ បង្កើតឯកសារ inference_model.json

ឯកសារ `inference_model.json` នេះប្រាប់ Foundry Local ឲ្យដឹងពីរបៀបបម្រុងសំណើ។ បង្កើត script Python ឈ្មោះ `generate_chat_template.py` **នៅក្នុងថតដើម ប្រព័ន្ធគ្រប់គ្រង** (ថតដដែល ដែលមានថត `models/`)៖

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# សង់សំណុំសន្ទនាថ្មីតិចតួចដើម្បីដកទុកសំណង់សន្ទនាចេញ
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

# សង់រចនាសម្ព័ន្ធ inference_model.json
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

ដំណើរការរសាយ script ខាងលើពីថតដើម៖

```bash
python generate_chat_template.py
```

> **កំណត់ចំណាំ៖** កញ្ចប់ `transformers` ត្រូវបានដំឡើងរួចជាការពឹងផ្អែកលើ `onnxruntime-genai`។ ប្រសិនបើមានកំហុស `ImportError`, សូម​ដំឡើង `pip install transformers` មុន។

script នេះផលិតឯកសារ `inference_model.json` នៅក្នុងថត `models/qwen3`។ ឯកសារនេះប្រាប់ Foundry Local របៀបដើម្បីបម្រុងសារ​អ្នកប្រើជាមួយសញ្ញាពិសេសត្រឹមត្រូវសម្រាប់ Qwen3។

> **សំខាន់៖** ផ្ទៃ `"Name"` ក្នុង `inference_model.json` (ត្រូវបានកំណត់ជា `qwen3-0.6b` នៅក្នុង script នេះ) គឺជាឈ្មោះដំណាក់កាលម៉ូដែលដែលអ្នកនឹងប្រើក្នុងពាក្យបញ្ជា ឬការហៅ API ទាំងអស់ បើចង់ផ្លាស់ប្តូរឈ្មោះនេះ សូមផ្លាស់ប្តូរឈ្មោះម៉ូដែលក្នុងអនុវត្តន៍ ៦ ដល់ ១០ ផងដែរ។

#### ជំហាន ៣៖ ផ្ទៀងផ្ទាត់កំណត់រចនាសម្ព័ន្ធ

បើកឯកសារ `models/qwen3/inference_model.json` និងបញ្ចាក់ថាមានផ្ទៃ `Name` និងអក្សរវ័ត្ត `PromptTemplate` ដែលមានកូនសោ `assistant` និង `prompt`។ ទំរង់សន្ទនាគួរតែមានសញ្ញាពិសេសដូចជា `<|im_start|>` និង `<|im_end|>` (សញ្ញានេះអាស្រ័យលើទំរង់សន្ទនារបស់ម៉ូដែល)។

> **ជម្រើសដៃ៖** ប្រសិនបើមិនចង់ដំណើរការ script អ្នកអាចបង្កើតឯកសារនេះដោយដៃ។ ស្នាមមួយចម្បងគឺថា ផ្ទៃ `prompt` គួរតែ​មានទំរង់សន្ទនាពេញលេញរបស់ម៉ូដែលជាមួយ `{Content}` ជាកន្លែងគេបញ្ចូលសារអ្នកប្រើ។

---

### អនុవត្តន៍ ៥៖ ផ្ទៀងផ្ទាត់រចនាសម្ព័ន្ធថតម៉ូដែល
អ្នកបង្កើតម៉ូដែលដាក់ឯកសារបង្ហាប់ទាំងអស់ចូលទៅក្នុងថតបង្ហាញដែលអ្នកបានបញ្ជាក់ផ្ទាល់។ សូមផ្ទៀងផ្ទាត់ថា រចនាសម្ព័ន្ធចុងក្រោយមើលទៅត្រឹមត្រូវ៖

```bash
ls models/qwen3
```

ថតគួរតែមានឯកសារទាំងនេះ៖

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

> **សម្គាល់:** ខុសពីឧបករណ៍បង្ហាប់ខ្លះៗផ្សេងទៀត អ្នកបង្កើតម៉ូដែលមិនបង្កើតថតរងទេ។ ឯកសារទាំងអស់ត្រូវបញ្ចេញផ្ទាល់ក្នុងថតបង្ហាញ ដូច្នេះវាជាអ្វីដែល Foundry Local កំពុងរង់ចាំ។

---

### ការធ្វើលំហាត់ 6៖ បន្ថែមម៉ូដែលទៅរកម៉ាស្សាទុក Foundry Local

ប្រាប់ Foundry Local ថាត្រូវស្វែងរកម៉ូដែលដែលបានបង្ហាប់របស់អ្នកដោយបន្ថែមថតទៅក្នុងម៉ាស្សាទុករបស់វា៖

```bash
foundry cache cd models/qwen3
```

ផ្ទៀងផ្ទាត់ថាម៉ូដែលបានបង្ហាញក្នុងម៉ាស្សាទុក៖

```bash
foundry cache ls
```

អ្នកគួរមើលឃើញម៉ូដែលផ្ទាល់ខ្លួនរបស់អ្នកបញ្ចូលក្នុងបញ្ចីជាមួយនឹងម៉ូដែលដែលបានផ្ទុកក្នុងម៉ាស្សាទុកមុន (ដូចជា `phi-3.5-mini` ឬ `phi-4-mini`)។

---

### ការធ្វើលំហាត់ 7៖ បើកម៉ូដែលផ្ទាល់ខ្លួនជាមួយ CLI

ចាប់ផ្តើមសមាសភាសារជជែកអន្តរកម្មជាមួយម៉ូដែលដែលអ្នកទើបបានបង្ហាប់ (ឈ្មោះ `qwen3-0.6b` មកពីវាល `Name` ដែលអ្នកបានកំណត់ក្នុង `inference_model.json`)៖

```bash
foundry model run qwen3-0.6b --verbose
```

ប៊្លក់ `--verbose` បង្ហាញព័ត៌មានបន្ថែមសម្រាប់វាយតម្លៃ ដែលមានប្រយោជន៍នៅពេលសាកល្បងម៉ូដែលផ្ទាល់ខ្លួនក្នុងដំណាក់កាលដំបូង។ បើម៉ូដែលបានបើកបានជោគជ័យ អ្នកនឹងឃើញបំពង់បញ្ចូលពាក្យសុំជជែក។ សាកល្បងសារចំនួនមួយ៖

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

វាយ `exit` ឬ ចុច `Ctrl+C` ដើម្បីបញ្ចប់សមាសភាសា។

> **ការដោះស្រាយបញ្ហា៖** ប្រសិនបើម៉ូដែលបរាជ័យក្នុងការបើក សូមពិនិត្យចំណុចដូចខាងក្រោម៖
> - ឯកសារ `genai_config.json` ត្រូវបានបង្កើតដោយអ្នកបង្កើតម៉ូដែល។
> - ឯកសារ `inference_model.json` មានស្រាប់ និងជា JSON ត្រឹមត្រូវ។
> - ឯកសារម៉ូដែល ONNX ស្ថិតនៅក្នុងថតត្រឹមត្រូវ។
> - អ្នកមាន RAM គ្រប់គ្រាន់ (Qwen3-0.6B int4 ត្រូវការខ្ទង់ 1 GB)។
> - Qwen3 គឺជា ម៉ូដែលអាថ៌កំបាំង ដែលបង្កើត ដេក `<think>`។ ប្រសើរអ្នកឃើញ `<think>...</think>` នៅដើមទិន្នផល វាជាប្រព័ន្ធធម្មតា។ អ្នកអាចកែសម្រួលព្រិយមួយក្នុង `inference_model.json` ដើម្បីបិទប្រាប់គំនិត។

---

### ការធ្វើលំហាត់ 8៖ សួរលទ្ធផលម៉ូដែលផ្ទាល់ខ្លួនតាមរយៈ REST API

បើអ្នកបានចាកចេញពីសមាសភាសាអន្តរកម្មនៅក្នុងលំហាត់ 7, ម៉ូដែលអាចមិនបង្ហាញឡើងទៀត។ ចាប់ផ្តើមសេវាកម្ម Foundry Local ហើយប загрузить ម៉ូដែលជាមុន៖

```bash
foundry service start
foundry model load qwen3-0.6b
```

សូមពិនិត្យមើលថាតើសេវាកម្មកំពុងរត់នៅលើច្រកដែលម្តេច៖

```bash
foundry service status
```

បន្ទាប់មកផ្ញើសំណើ (ប្តូរ `5273` ជាច្រកពិតរបស់អ្នកបើវាផ្សេង)៖

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **សម្គាល់ Windows:** ពាក្យបញ្ជា `curl` ខាងលើប្រើបែបបទ bash។ នៅលើ Windows សូមប្រើកម្មវិធី PowerShell `Invoke-RestMethod` ខាងក្រោមជំនួស។

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

### ការធ្វើលំហាត់ 9៖ ប្រើម៉ូដែលផ្ទាល់ខ្លួនជាមួយ OpenAI SDK

អ្នកអាចភ្ជាប់ទៅម៉ូដែលផ្ទាល់ខ្លួនដោយប្រើកូដ OpenAI SDK ដដែលនឹងដែលអ្នកបានប្រើសម្រាប់ម៉ូដែល built-in (មើល [ផ្នែក 3](part3-sdk-and-apis.md))។ ភាពខុសគ្នាដែលត្រូវមានគឺឈ្មោះម៉ូដែលតែប៉ុណ្ណោះ។

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # Foundry មូលដ្ឋានមិនផ្ទៀងផ្ទាត់កូដ API​ឡើយ
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
  apiKey: "foundry-local", // Foundry Local មិនផ្ទៀងផ្ទាត់ ក្តារ API ទេ
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

> **ចំណុចសំខាន់៖** ព្រោះ Foundry Local បង្ហាញ API ដែលផ្គូរផ្គងនឹង OpenAI សូមកូដណាមួយដែលដំណើរការជាមួយម៉ូដែល built-in ក៏ដំណើរការជាមួយម៉ូដែលផ្ទាល់ខ្លួនរបស់អ្នកដែរ។ អ្នកគ្រាន់តែប្ដូរពារាម៉ែត្រ `model` តែប៉ុណ្ណោះ។

---

### ការធ្វើលំហាត់ 10៖ សាកល្បងម៉ូដែលផ្ទាល់ខ្លួនជាមួយ Foundry Local SDK

ក្នុងមន្ទីរពិសោធន៍មុន អ្នកបានប្រើ Foundry Local SDK ដើម្បីចាប់ផ្តើមសេវាកម្ម ស្វែងរកចំណុចចេញ និងគ្រប់គ្រងម៉ូដែលដោយស្វ័យប្រវត្តិ។ អ្នកអាចអនុវត្តន៍និរន្តរភាពដដែលជាមួយម៉ូដែលបង្ហាប់ផ្ទាល់ខ្លួន។ SDK នឹងដោះសោចការចាប់ផ្តើមសេវា និងស្វែងរកចំណុចចេញ ដូច្នេះកូដរបស់អ្នកមិនចាំបាច់កូដកំរិត `localhost:5273` ។

> **សម្គាល់:** សូមប្រាកដថា Foundry Local SDK ត្រូវបានដំឡើង មុនពេលដំណើរការឧទាហរណ៍ទាំងនេះ៖
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** បន្ថែមកញ្ចប់ `Microsoft.AI.Foundry.Local` និង `OpenAI` NuGet
>
> រក្សាទុករាល់ឯកសារស្គ្រីប **នៅក្នុងឫសីផតថលឯកសារ** (ថតដូចគ្នាជាមួយថត `models/`)។

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# ជំហានទី ១: ចាប់ផ្តើមសេវាកម្ម Foundry Local និងផ្ទុកម៉ូដែលផ្ទាល់ខ្លួន
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# ជំហានទី ២: ពិនិត្យមើលឃ្លាំងសម្រាប់ម៉ូដែលផ្ទាល់ខ្លួន
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# ជំហានទី ៣: ផ្ទុកម៉ូដែលចូលក្នុងចងចាំ
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# ជំហានទី ៤: បង្កើតអតិថិជន OpenAI ប្រើចំណុចផ្លូវដែលរកឃើញដោយ SDK
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# ជំហានទី ៥: ផ្ញើសំណើបញ្ចប់ការសន្ទនាប្រភេទចរាចរណ៍
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

ប្រតិបត្តិវា៖

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

// ជំហានទី 1: ចាប់ផ្តើមសេវាកម្ម Foundry Local
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ជំហានទី 2: ទាញយកម៉ូដែលផ្ទាល់ខ្លួនពីកាតាឡុក
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// ជំហានទី 3: ដាក់ទិន្នន័យម៉ូដែលទៅក្នុងមេម៉ូរី
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// ជំហានទី 4: បង្កើតអតិថិជន OpenAI ដោយប្រើចំណុចបញ្ចប់ដែល SDK បានរកឃើញ
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// ជំហានទី 5: ផ្ញើសំណើបញ្ចប់ការជជែកតាមស្ទ្រីម
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

ប្រតិបត្តិវា៖

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

> **ចំណុចសំខាន់៖** Foundry Local SDK ស្វែងរកចំណុចចេញដោយឥតគិតថ្លៃ ដូច្នេះអ្នកមិនចាំបាច់កូដច្រកណាមួយឡើងវិញទេ។ វាដំណើរការល្អសម្រាប់កម្មវិធីផលិតកម្ម។ ម៉ូដែលផ្ទាល់ខ្លួនរបស់អ្នកដំណើរការដូចម៉ូដែលក្នុងបណ្ណាល័យតាម SDK។

---

## ជ្រើសរើសម៉ូដែលសម្រាប់បង្ហាប់

Qwen3-0.6B ត្រូវបានប្រើជាគំរូយោងនៅក្នុងមន្ទីរពិសោធន៍នេះ ព្រោះវាមានទំហំតូច លឿនក្នុងការបង្ហាប់ ហើយអាចប្រើបានដោយសេរីក្រោមអាជ្ញាបណ្ណ Apache 2.0។ ទោះយ៉ាងណា អ្នកអាចបង្ហាប់ម៉ូដែលផ្សេងទៀតបានៗ។ នេះជាការផ្តល់អនុសាសន៍៖

| ម៉ូដែល | Hugging Face ID | ប៉ារ៉ាម៉ែត្រ | អាជ្ញាបណ្ណ | សម្គាល់ |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | តូច មានល្បឿនលឿន គួរឲ្យប្រើសាកល្បង |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | គុណភាពល្អប្រសើរ បន្ទាប់តែរហ័សក្នុងការបង្ហាប់ |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | គុណភាពខ្លាំង តម្រូវការជាង RAM ច្រើន |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | តម្រូវការអភិមោទ្នអាជ្ញាបណ្ណលើ Hugging Face |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | គុណភាពខ្ពស់ ទំហំធំ និងការបង្ហាប់យូរ |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | មានរួចក្នុងបណ្ណាល័យ Foundry Local (មានប្រយោជន៍សម្រាប់ប្រៀបធៀប) |

> **កំណត់ចំណាំអាជ្ញាបណ្ណ:** សូមពិនិត្យអាជ្ញាបណ្ណម៉ូដែលនៅលើ Hugging Face មុនប្រើ។ ម៉ូដែលខ្លះ (ដូចជា Llama) តម្រូវឲ្យអ្នកទទួលយកអាជ្ញាបណ្ណ និងចូលប្រើដោយ `huggingface-cli login` មុនទាញយក។

---

## គំនិត៖ ពេលណាចាំបាច់ប្រើម៉ូដែលផ្ទាល់ខ្លួន

| ស្ថានការ | មូលហេតុដែលចាំបាច់បង្ហាប់ខ្លួនឯង? |
|----------|----------------------|
| **ម៉ូដែលដែលអ្នកត្រូវការមិនមានក្នុងបណ្ណាល័យ** | បណ្ណាល័យ Foundry Local បានរៀបចំជាមុត។ ប្រសិនបើម៉ូដែលដែលអ្នកចង់មិនត្រូវបានរាប់បញ្ចូល សូមបង្ហាប់ខ្លួនឯង។ |
| **ម៉ូដែលដែលបានធ្វើ fine-tune** | ប្រសិនបើអ្នកធ្វើ fine-tune ម៉ូដែលលើទិន្នន័យជាក់លាក់ អ្នកត្រូវបង្ហាប់ទម្ងន់ផ្ទាល់ខ្លួន។ |
| **តម្រូវការតុល្យភាពជាក់លាក់** | អ្នកឲ្យការតុល្យភាព ឬយុទ្ធសាស្ត្រតុល្យភាពខុសពីលំនាំដើម។ |
| **ម៉ូដែលថ្មីៗដែលបានចេញផ្សាយ** | ពេលម៉ូដែលថ្មីចេញផ្សាយលើ Hugging Face យ៉ាងហោចណាស់វាអាចមិនមាននៅក្នុងបណ្ណាល័យ Foundry Local តែម្ដង។ ការបង្ហាប់ដោយខ្លួនឯងធ្វើឲ្យអ្នកបានចូលសម្រាប់ភ្លាមៗ។ |
| **ការស្រាវជ្រាវ និងការសាកល្បង** | ល្បែងតាមផែនបន្ទាត់ម៉ូដែលទំហំ ផ្នែកផ្សេងៗ ឬការរចនា មុនបញ្ចប់ជាជម្រើសផលិតកម្ម។ |

---

## សេចក្ដីសង្ខេប

នៅក្នុងមន្ទីរពិសោធន៍នេះ អ្នកបានរៀនពីរបៀប៖

| ជំហាន | អ្វីដែលអ្នកបានធ្វើ |
|------|-------------|
| 1 | ដំឡើង ONNX Runtime GenAI model builder |
| 2 | បង្ហាប់ `Qwen/Qwen3-0.6B` ពី Hugging Face ទៅជា ONNX ដែលបានអុបទ៊ីម៉ៃស៍ |
| 3 | បង្កើតឯកសារ `inference_model.json` ជា chat-template |
| 4 | បន្ថែមម៉ូដែលបង្ហាប់ទៅម៉ាស្សាទុក Foundry Local |
| 5 | រត់សមាសភាសាជជែកជាមួយម៉ូដែលផ្ទាល់ខ្លួនតាម CLI |
| 6 | សួរព័ត៌មានម៉ូដែលតាម REST API ដែលផ្គូរផ្គង OpenAI |
| 7 | ភ្ជាប់ពី Python, JavaScript និង C# ដោយប្រើ OpenAI SDK |
| 8 | សាកល្បងម៉ូដែលផ្ទាល់ខ្លួនពេញលេញជាមួយ Foundry Local SDK |

ចំណុចសំខាន់គឺ **ម៉ូដែលផ្អែកលើ transformer ណាមួយអាចបើកប្រាស់តាម Foundry Local** បន្ទាប់ពីវាត្រូវបានបង្ហាប់ទៅជា ONNX។ API ផ្គូរផ្គង OpenAI សមត្ថភាពធ្វើឲ្យកូដកម្មវិធីដែលមានស្រាប់ដំណើរការជាទូទៅដោយគ្មានការផ្លាស់ប្ដូរ។ អ្នកគ្រាន់តែប្ដូរឈ្មោះម៉ូដែល។

---

## ចំណុចសំខាន់ៗ

| គំនិត | ព័ត៌មានលម្អិត |
|---------|--------|
| ONNX Runtime GenAI Model Builder | ផ្លាស់ប្ដូរ ម៉ូដែល Hugging Face ទៅ ONNX ជាមួយការតុល្យភាព ក្នុងមួយពាក្យបញ្ជា |
| ទ្រង់ទ្រាយ ONNX | Foundry Local តម្រូវម៉ូដែល ONNX ដែលមានកំណត់ការកំណត់ ONNX Runtime GenAI |
| ឧទាហរណ៍ chat | ឯកសារ `inference_model.json` ប្រាប់ Foundry Local របៀបរៀបចំ prompt សម្រាប់ម៉ូដែលមួយ |
| គោលដៅធាតុផ្សេងៗ | បង្ហាប់សម្រាប់ CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), ឬ WebGPU ដោយផ្អែកលើឧបករណ៍របស់អ្នក |
| តុល្យភាព | ការតុល្យភាពភាពទាប (int4) កាត់បន្ថយទំហំ និងលឿន តែបង្កគុណភាពខ្លះ; fp16 រក្សាគុណភាពខ្ពស់លើ GPU |
| តុល្យភាព API | ម៉ូដែលផ្ទាល់ខ្លួនប្រើ API ផ្គូរផ្គង OpenAI ដដែលនឹងម៉ូដែល built-in |
| Foundry Local SDK | SDK គ្រប់គ្រងការចាប់ផ្តើម ស្វែងរកចំណុច ចេញ និងបើកម៉ូដែលដោយស្វ័យប្រវត្តិ សម្រាប់បណ្ណាល័យ និងម៉ូដែលផ្ទាល់ខ្លួន |

---

## អានបន្ថែម

| ប្រភព | តំណភ្ជាប់ |
|----------|------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| មគ្គុទេសក៍ម៉ូដែលផ្ទាល់ខ្លួន Foundry Local | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| គ្រួសារម៉ូដែល Qwen3 | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| ឯកសារ Olive | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## ជំហានបន្ទាប់

បន្តទៅ [ផ្នែក 11៖ ការហៅឧបករណ៍ជាមួយម៉ូដែលក្នុងស្រុក](part11-tool-calling.md) ដើម្បីរៀនពីរបៀបអនុញ្ញាតឲ្យម៉ូដែលក្នុងស្រុកហៅមុខងារផ្សេងទៀត។

[← ផ្នែក 9៖ ការបម្លែងសម្លេង Whisper](part9-whisper-voice-transcription.md) | [ផ្នែក 11៖ ការហៅឧបករណ៍ →](part11-tool-calling.md)