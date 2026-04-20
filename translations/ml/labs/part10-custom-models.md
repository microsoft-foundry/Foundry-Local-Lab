![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ഭാഗം 10: Foundry Local-ലൂടെ കസ്റ്റം അല്ലെങ്കിൽ Hugging Face മോഡലുകൾ ഉപയോഗിക്കുക

> **ലക്ഷ്യം:** Foundry Local ആവശ്യപ്പെടുന്ന ഓപ്‌റ്റിമൈസ്ഡ് ONNX ഫോർമാറ്റിലേക്ക് Hugging Face മോഡൽ കമ്പൈൽ ചെയ്യുക, ഒരു ചാറ്റ് ടെമ്പ്ലേറ്റ് ഉപയോഗിച്ച് കോൺഫിഗർ ചെയ്യുക, ലോക്കൽ കാഷെയിലേക്ക് ചേർക്കുക, പിന്നീട് CLI, REST API, OpenAI SDK എന്നിവ ഉപയോഗിച്ച് അതിൽ inference നടത്തുക.

## അവലോകനം

Foundry Local പ്രീ-കമ്പൈൽ ചെയ്ത മോഡലുകളുടെ ക്രൂരേറ്റഡ് കാറ്റലോഗുമായി എത്തുന്നു, എന്നാൽ നിങ്ങൾ ആ പട്ടികവുമായിൽ മാത്രം പരിമിതപ്പെടുത്തിയിട്ടില്ല. [Hugging Face](https://huggingface.co/) ൽ ലഭ്യമായ ഏതൊരു ട്രാൻസ്ഫോർമർ അടിസ്ഥാനമാക്കിയുള്ള ഭാഷ മോഡലും (അഥവാ PyTorch / Safetensors ഫോർമാറ്റിൽ ലോക്കലായി സേവ് ചെയ്തിട്ടുള്ളത്) ഓപ്‌റ്റിമൈസ്ഡ് ONNX മോഡലായി കമ്പൈൽ ചെയ്ത് Foundry Local വഴി സർവ് ചെയ്‌തേകാം.

കമ്പൈലേഷൻ പൈപ്പ്‌ലൈൻ **ONNX Runtime GenAI Model Builder** എന്ന കമാൻഡ് ലൈൻ ടൂൾ ഉപയോഗിക്കുന്നു, ഇത് `onnxruntime-genai` പാക്കേജിനൊപ്പം ഉൾപ്പെടുത്തിയിട്ടുണ്ട്. മോഡൽ ബിൽഡർ താഴെ പറയുന്ന ഏറെ വലുതായ കാര്യമെല്ലാം കൈകാര്യം ചെയ്യുന്നു: സോഴ്‌സ് വെയ്റ്റുകൾ ഡൗൺലോഡ് ചെയ്യൽ, അവ ONNX ഫോർമാറ്റിലേക്ക് പരിവർത്തനം ചെയ്യൽ, ക്വാണ്ടൈസേഷൻ (int4, fp16, bf16) പ്രയോഗിക്കൽ, കൂടാതെ Foundry Local പ്രതീക്ഷിക്കുന്ന കോൺഫിഗറേഷൻ ഫയലുകൾ (ചാറ്റ് ടെമ്പ്ലേറ്റ്, ടോക്കിനൈസർ ഉൾപ്പെടെ) പുറത്താക്കൽ.

ഈ ലാബിൽ നിങ്ങൾ Hugging Face-ൽ നിന്ന് **Qwen/Qwen3-0.6B** മോഡൽ കമ്പൈൽ ചെയ്ത് Foundry Local-ൽ രജിസ്റ്റർ ചെയ്ത് നിങ്ങളുടെ യന്ത്രത്തിൽ മുഴുവനായും ചാറ്റ് ചെയ്യണമെന്നതാണ്.

---

## പഠന ലക്ഷ്യങ്ങൾ

ഈ ലാബ് പൂര്‍ത്തിയായപ്പോൾ നിങ്ങൾക്ക് സാധിക്കേണ്ടത്:

- കസ്റ്റം മോഡൽ കമ്പൈലേഷൻ എപ്പോൾ എങ്ങനെ ഉപകാരപ്രദമാണെന്ന് വിശദീകരിക്കാൻ
- ONNX Runtime GenAI മോഡൽ ബിൽഡർ ഇൻസ്റ്റാൾ ചെയ്യാൻ
- ഒരു Hugging Face മോഡൽ ഒറ്റ കമാൻഡിൽ ഓപ്‌റ്റിമൈസ്ഡ് ONNX ഫോർമാറ്റിലേക്ക് കമ്പൈൽ ചെയ്യാൻ
- മുഖ്യ കമ്പൈലേഷൻ പാരാമീറ്ററുകൾ (എക്സിക്യുഷൻ പ്രൊവൈഡർ, പ്രിസിഷൻ) മനസ്സിലാക്കാൻ
- `inference_model.json` ചാറ്റ് ടെമ്പ്ലേറ്റ് കോൺഫിഗറേഷൻ ഫയൽ സൃഷ്ടിക്കാൻ
- കമ്പൈൽ ചെയ്ത മോഡൽ Foundry Local കാഷെയിലേക്ക് ചേർക്കാൻ
- CLI, REST API, OpenAI SDK എന്നിവ ഉപയോഗിച്ച് കസ്റ്റം മോഡലിൽ inference നടത്താൻ

---

## മുൻകൂർ ആവശ്യങ്ങൾ

| ആവശ്യകതി | വിശദാംശങ്ങൾ |
|-------------|---------|
| **Foundry Local CLI** | ഇൻസ്റ്റാൾ ചെയ്ത് നിങ്ങളുടെ `PATH`-ൽ ഉണ്ടായിരിക്കണം ([ഭാഗം 1](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI മോഡൽ ബിൽഡർക്കായി ആവശ്യമാണ് |
| **pip** | Python പാക്കേജ് മാനേജർ |
| **ഡിസ്‌ക് സ്‌പെയ്‌സ്** | സോഴ്‌സ് ഫയലുകൾക്കും കമ്പൈൽ ചെയ്ത മോഡലിനും കുറഞ്ഞത് 5 GB സൗജന്യ സ്ഥലം |
| **Hugging Face അക്കൗണ്ട്** | ചില മോഡലുകൾ ഡൗൺലോഡ് ചെയ്യുന്നതിന് മുമ്പ് ലൈസൻസ് അംഗീകരണം ആവശ്യമാണ്. Qwen3-0.6B Apache 2.0 ലൈസൻസ് ഉപയോഗിക്കുകയും സൗജന്യമായി ലഭ്യമാകുകയും ചെയ്യുന്നു. |

---

## പരിസ്ഥിതി ക്രമീകരണം

മോഡൽ കമ്പൈലേഷൻക്ക് വലിയ Python പാക്കേജുകൾ (PyTorch, ONNX Runtime GenAI, Transformers) ആവശ്യമാണ്. നിങ്ങളുടെ സിസ്റ്റം Python-ിയും മറ്റ് പ്രോജക്ടുകളും ബാധിക്കാതിരിക്കാൻ വേർതിരിച്ച വേർച്വൽ എൻവയോൺമെന്റ് സൃഷ്ടിക്കുക.

```bash
# റിപോസിറ്ററി റൂട്ട് നിന്നും
python -m venv .venv
```

എൻവയോൺമെന്റ് സജീവമാക്കുക:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

പിപ് അപ്‌ഗ്രേഡുചെയ്യുക, ഡിപ്പെൻഡൻസി പരിഹാര പ്രശ്നങ്ങൾ ഒഴിവാക്കാൻ:

```bash
python -m pip install --upgrade pip
```

> **സൂചനം:** മുമ്പത്തെ ലാബുകളിൽ നിങ്ങൾക്ക് `.venv` ഉണ്ടെങ്കിൽ, അത് വീണ്ടും ഉപയോഗിക്കാം. തുടരുന്നതിന് മുൻപ് അത് സജീവമാക്കാൻ ശ്രദ്ധിക്കുക.

---

## ആശയം: കമ്പൈലേഷൻ പൈപ്പ്‌ലાઈન

Foundry Local ONNX ഫോർമാറ്റിലുള്ള മോഡലുകൾ, ONNX Runtime GenAI കോൺഫിഗറേഷനുമായ ഓണാക്കിയ മോഡലുകൾ ആവശ്യപ്പെടുന്നു. Hugging Face-ലെ പല ഓപ്പൺ-സോഴ്‌സ് മോഡലുകളും PyTorch അല്ലെങ്കിൽ Safetensors വെയ്റ്റുകളായി വിതരണം ചെയ്യുന്നു, അതിനാൽ മാറ്റം ചെയ്യാനുള്ള ഘട്ടം ആവശ്യമാണ്.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### മോഡൽ ബിൽഡർ എന്ത് ചെയ്യുന്നു?

1. Hugging Face മുതൽ സോഴ്‌സ് മോഡൽ ഡൗൺലോഡ് ചെയ്യുന്നു (അഥവാ ലോക്കൽ പാത്തിൽ നിന്നും വായിക്കുന്നു).
2. PyTorch / Safetensors വെയ്റ്റുകൾ ONNX ഫോർമാറ്റിലേക്കു പരിവർത്തനം ചെയ്യുന്നു.
3. മെമ്മറി ഉപയോഗവും ത്രൂപ്പുട്ടും മെച്ചപ്പെടുത്താൻ മോഡൽ ചെറിയ പ്രിസിഷനിലേക്ക് (ഉദാ., int4) ക്വാണ്ടൈസേഷൻ ചെയ്യുന്നു.
4. ONNX Runtime GenAI കോൺഫിഗറേഷൻ (`genai_config.json`), ചാറ്റ് ടെംപ്ലേറ്റ് (`chat_template.jinja`), ടോക്കിനൈസർ ഫയലുകൾ എന്നിവ ബഹുമുഖമായി പുറത്തെടുക്കുന്നു, അതിലൂടെ Foundry Local മോഡൽ ലോഡ് ചെയ്ത് സർവ് ചെയ്യാൻ സാധിക്കും.

### ONNX Runtime GenAI Model Builder വേഴ്സസ് Microsoft Olive

മോഡൽ ഓപ്റ്റിമൈസేషన్ ടൂളായി **Microsoft Olive**-യുടെ പരാമർശം കണ്ടെത്താം. രണ്ടു ടൂളുകളും ONNX മോഡലുകൾ നിർമ്മിക്കാം, എന്നാൽ വ്യത്യസ്ത ലക്ഷ്യങ്ങൾക്കും വ്യാപ്തിക്ക് ഉണ്ട്:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **പാക്കേജ്** | `onnxruntime-genai` | `olive-ai` |
| **പ്രധാന ലക്ഷ്യം** | ONNX Runtime GenAI inference-ന് ജനറേറ്റീവ് AI മോഡലുകൾ പരിവർത്തനം ചെയ്ത് ക്വാണ്ടൈസ് ചെയ്യുക | നിരവധി ബാക്ക്‌എൻഡുകളും ഹാർഡ്‌വെയർ ടാർഗെറ്റുകളുമായി ചേരുന്ന End-to-end മോഡൽ ഒപ്റ്റിമൈസേഷൻ ഫ്രെയിംവർക്ക് |
| **ഉപയോഗ എളുപ്പം** | ഒറ്റ കമാൻഡ് — ഏക ഘട്ടം പരിവർത്തനം + ക്വാണ്ടൈസേഷൻ | വർക്ക്ഫ്ലോ അടിസ്ഥാനമാക്കി — YAML/JSON ഉപയോഗിച്ച് കോൺഫിഗറബിള്‍ മൾട്ടി-പാസ് പൈപ്പ്‌ലൈൻകൾ |
| **ഔട്ട്പുട്ട് ഫോർമാറ്റ്** | ONNX Runtime GenAI ഫോർമാറ്റ് (Foundry Local-ന് തൊടുത്തു തയാറായത്) | ജനറിക്ക് ONNX, ONNX Runtime GenAI, അല്ലെങ്കിൽ ഓൺ അനുസരിച്ച് വ്യത്യസ്ത ഫോർമാറ്റുകൾ |
| **ഹാർഡ്‌വെയർ ലക്ഷ്യങ്ങൾ** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN മുതലായവ |
| **ക്വാണ്ടൈസേഷൻ ഓപ്ഷനുകൾ** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, ഗ്രാഫ് ഓപ്റ്റിമൈസേഷൻ, ലെയർ-വൈസ് ട്യൂണിംഗ് |
| **മോഡൽ വ്യാപ്തി** | ജനറേറ്റീവ് AI മോഡലുകൾ (LLMs, SLMs) | ഏതൊരു ONNX-കൺവേർട്ടബിൾ മോഡലും (വിഷൻ, NLP, ഓഡിയോ, മൾട്ടി മോഡൽ) |
| **ഉത്തരവാദിത്തം** | നിഘണ്ടുവിദ്യയായ സെറ്റ്-മോഡൽ കമ്പൈലേഷൻ | ഫൈൻ-ഗ്രെയിൻഡ് ഒപ്റ്റിമൈസേഷൻ നിയന്ത്രണം ആവശ്യമായ പ്രൊഡക്ഷൻ പൈപ്പ്‌ലൈനുകൾ |
| **ഡിപ്പെൻഡൻസി ഫുട്‌പ്രിന്റ്** | മധ്യമം (PyTorch, Transformers, ONNX Runtime) | വലിയത് (Olive ഫ്രെയിംവർക്ക്, ആവശ്യത്തിനായുള്ള അധിക പാക്കേജുകൾ) |
| **Foundry Local ഇന്റഗ്രേഷൻ** | നേരിട്ടുള്ളത് — ഔട്ട്പുട്ട് ഉടൻ توافقിക്കുന്നു | `--use_ort_genai` ഫ്ലാഗും അധിക കോൺഫിഗറേഷൻ ആവശ്യമാണ് |

> **ഈ ലാബിൽ മോഡൽ ബിൽഡർ ഉപയോഗിക്കുന്നത്:**
> ഒരു Hugging Face മോഡൽ ഒറ്റയ്ക്ക് കമ്പൈൽ ചെയ്ത് Foundry Local-ലേക്ക് രജിസ്റ്റർ ചെയ്യുന്നതിനായി മോഡൽ ബിൽഡർ ഏറ്റവും ലളിതവും വിശ്വസനീയവുമായ മാർഗമാണ്. Foundry Local പ്രതീക്ഷിക്കുന്ന കൃത്യ ഫോർമാറ്റ് ഇത് ഉൽപ്പാദിപ്പിക്കും. പിന്നീട് നിങ്ങൾക്ക് ആക്യുറസി അറിയുന്നതിനുള്ള ക്വാണ്ടൈസേഷൻ, ഗ്രാഫ് സർജറി, മൾട്ടി-പാസ് ട്യൂണിംഗ് പോലുള്ള ആഡ്‌വാൻസ്ഡ് ഓപ്റ്റിമൈസേഷൻ ഫീച്ചറുകൾ ആവശ്യമെങ്കിൽ Olive പരീക്ഷിക്കാം. കൂടുതൽ വിവരങ്ങൾക്ക് [Microsoft Olive ഡോക്യുമെന്റേഷൻ](https://microsoft.github.io/Olive/) കാണുക.

---

## ലാബ് അഭ്യാസങ്ങൾ

### അഭ്യാസം 1: ONNX Runtime GenAI Model Builder ഇൻസ്റ്റാൾ ചെയ്യുക

മോഡൽ ബിൽഡർ ടൂൾ ഉൾപ്പെടുന്ന ONNX Runtime GenAI പാക്കേജ് ഇൻസ്റ്റാൾ ചെയ്യുക:

```bash
pip install onnxruntime-genai
```

ഇൻസ്റ്റാളേഷൻ ശരിയാണെന്ന് ഉറപ്പാക്കാൻ മോഡൽ ബിൽഡർ ലഭ്യമാണോ പരിശോധിക്കുക:

```bash
python -m onnxruntime_genai.models.builder --help
```

`-m` (മോഡൽ നാമം), `-o` (ഔട്ട്പുട്ട് പാത്ത്), `-p` (პრДиси lion), `-e` (എക്സിക്യൂഷൻ പ്രൊവൈഡർ) പോലുള്ള പാരാമീറ്ററുകൾ കാണിക്കുന്ന സഹായ ഔട്ട്പുട്ട് ലഭിക്കും.

> **അറിയിപ്പ്:** മോഡൽ ബിൽഡർ PyTorch, Transformers, മറ്റ് പാക്കേജുകൾ എന്നിവ ആശ്രയിക്കുന്നു. ഇൻസ്റ്റാളേഷൻ കുറച്ച് മിനിറ്റുകൾക്കിടയിൽ നടക്കും.

---

### അഭ്യാസം 2: Qwen3-0.6B CPU വിന് വേണ്ടി കമ്പൈൽ ചെയ്യുക

Hugging Face-ൽ നിന്നുള്ള Qwen3-0.6B മോഡൽ ഡൗൺലോഡ് ചെയ്ത് CPU ഇൻഫറൻസ് വേണ്ടി int4 ക്വാണ്ടൈസേഷനോടൊപ്പം കമ്പൈൽ ചെയ്യുക:

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

#### ഓരോ പാരാമീറ്ററിന്റെയും പ്രവർത്തനം

| പാരാമീറ്റർ | ഉദ്യേശം | ഉപയോക്തിരിക്കുന്ന മൂല്യം |
|-----------|---------|------------|
| `-m` | Hugging Face മോഡൽ ഐഡി അല്ലെങ്കിൽ ലോക്കൽ ഡയറക്ടറി പാത്ത് | `Qwen/Qwen3-0.6B` |
| `-o` | കമ്പൈൽ ചെയ്ത ONNX മോഡൽ സൂക്ഷിക്കുന്ന ഡയറക്ടറി | `models/qwen3` |
| `-p` | കമ്പൈലേഷൻ സമയമുണ്ടാകുന്ന ക്വാണ്ടൈസേഷൻ ആസ്പ്രിഷൻ | `int4` |
| `-e` | ONNX Runtime എക്സിക്യൂഷൻ പ്രൊവൈഡർ (ലക്ഷ്യം ഹാർഡ്‌വെയർ) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face പ്രാമാണീകരണം ഒഴിവാക്കുന്നു (പബ്ലിക് മോഡലുകൾക്ക് ചൂകാവുന്ന കൂട്ടായി) | `hf_token=false` |

> **എത്ര നേരം എടുക്കും?**
> നിങ്ങളുടെ ഹാർഡ്‌വെയർ, മോഡൽ വലിപ്പം എന്നിവ അനുസരിച്ച് സമയമാകും. Qwen3-0.6B int4 ക്വാണ്ടൈസേഷനോടെ ആധുനിക CPUയിൽ ഏകദേശം 5 മുതൽ 15 മിനിറ്റ് വരെ കാണാം. വലുതായ മോഡലുകൾ അനുപാതമായി കുറച്ച് കൂടുതൽ സമയമെടുക്കും.

കമാൻഡ് പൂർത്തിയാക്കുമ്പോൾ `models/qwen3` എന്ന ഡയറക്ടറി കമ്പൈൽ ചെയ്ത മോഡൽ ഫയലുകൾ അടങ്ങിയതായി കാണണം. ഔട്ട്പുട്ട് പരിശോധിക്കുക:

```bash
ls models/qwen3
```

തുടർന്ന് കാണുന്ന ഫയലുകൾ ഉണ്ടായിരിക്കണം:
- `model.onnx` և `model.onnx.data` — കമ്പൈൽ ചെയ്ത മോഡൽ വെയ്റ്റുകൾ
- `genai_config.json` — ONNX Runtime GenAI കോൺഫിഗറേഷൻ
- `chat_template.jinja` — മോഡലിന്റെ ചാറ്റ് ടെംപ്ലേറ്റ് (സ്വയം സൃഷ്ടിച്ച)
- `tokenizer.json`, `tokenizer_config.json` — ടോക്കിനൈസർ ഫയലുകൾ
- മറ്റ് വോകാബുലറി, കോൺഫിഗറേഷൻ ഫയലുകൾ

---

### അഭ്യാസം 3: GPU-വിന് വേണ്ടി കമ്പൈൽ ചെയ്യുക (ഐച്ഛികം)

CUDA പിന്തുണയുള്ള NVIDIA GPU ഉണ്ടെങ്കിൽ, വേഗതകുറഞ്ഞ ഇൻഫറൻസിന് GPU-ഓപ്റ്റിമൈസ്ഡ് വേർഷൻ കമ്പൈൽ ചെയ്യാം:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **അറിയിപ്പ്:** GPU കമ്പൈലേഷനു `onnxruntime-gpu`യുടെ ഇൻസ്റ്റാളേഷനും CUDA പ്രവർത്തനക്ഷമമായ ഇൻസ്റ്റാളേഷനും ആവശ്യമാണ്. ഇവ ഇല്ലെങ്കിൽ മോഡൽ ബിൽഡർ പിഴവ് റിപ്പോർട്ട് ചെയ്യും. ഈ അഭ്യസം ഒഴിവാക്കി CPU വേർഷൻ തുടരണം.

#### ഹാർഡ്‌വെയർ-സ്പെസിഫിക് കമ്പൈലേഷൻ റഫറൻസ്

| ടാർഗെറ്റ് | എക്സിക്യൂഷൻ പ്രൊവൈഡർ (`-e`) | ശിപാർശ ചെയ്‌ത പ്രിസിഷൻ (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` അല്ലെങ്കിൽ `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` അല്ലെങ്കിൽ `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### പ്രിസിഷൻ ട്രേഡ്-ഓഫ്

| പ്രിസിഷൻ | വലിപ്പം | വേഗം | ഗുണനം |
|-----------|------|-------|---------|
| `fp32` | ഏറ്റവും വലുത് | ഏറ്റവും താഴ്ന്ന വേഗം | ഏറ്റവും കൂടുതൽ കൃത്യത |
| `fp16` | വലി | വേഗം (GPU) | വളരെ നല്ല കൃത്യത |
| `int8` | ചെറുത് | വേഗമുള്ളത് | ചെറുതായ കൃത്യത നഷ്ടം |
| `int4` | ഏറ്റവും ചെറുത് | ഏറ്റവും വേഗം | മിതമായ കൃത്യത നഷ്ടം |

ബഹുഭാഗം ലോക്കൽ ഡെവലപ്പ്മെന്റിനായി CPU-യിൽ `int4` മികച്ച വേഗതയും സ്രോതസ്സുകൾ കുറഞ്ഞ ഉപയോഗവും നൽകുന്നു. പ്രൊഡക്ഷൻ ഗുണനിലവാരത്തിനായി CUDA GPU-ൽ `fp16` ശിപാർശ ചെയ്യുന്നു.

---

### അഭ്യാസം 4: ചാറ്റ് ടെമ്പ്ലേറ്റ് കോൺഫിഗറേഷൻ സൃഷ്ടിക്കുക

മോഡൽ ബിൽഡർ ഔട്ട്പുട്ട് ഡയറക്ടറിയിൽ സ്വയം `chat_template.jinja` ഫയൽ, `genai_config.json` ഫയൽ സൃഷ്ടിക്കുന്നു. എന്നാൽ Foundry Localയ്ക്ക് മോഡലിന് പ്രാംപ്റ്റുകൾ എങ്ങനെ ഫോർമാറ്റ് ചെയ്യണമെന്ന് അറിയാൻ `inference_model.json` ഫയലും വേണം. ഇത് മോഡൽ നാമവും ഉപയോക്തൃ സന്ദേശങ്ങൾ ശരിയായ സ്പെഷ്യൽ ടോക്കണുകളാൽ ഘടിപ്പിക്കുന്ന പ്രാംപ്റ്റ് ടെംപ്ലേറ്റ് വിശദീകരിക്കുകയും ചെയ്യുന്നു.

#### ഘട്ടം 1: കമ്പൈൽ ചെയ്‌തിരിക്കുന്ന ഔട്ട്പുട്ട് പരിശോധിക്കുക

കമ്പൈൽ ചെയ്ത മോഡൽ ഡയറക്ടറി ഉള്ളടക്കം ലിസ്റ്റുചെയ്യുക:

```bash
ls models/qwen3
```

നിങ്ങൾക്ക് ഇത്തരത്തിലുള്ള ഫയലുകൾ കാണാം:
- `model.onnx` և `model.onnx.data` — കമ്പൈൽ ചെയ്ത മോഡൽ വെയ്റ്റുകൾ
- `genai_config.json` — ONNX Runtime GenAI കോൺഫിഗറേഷൻ (സ്വയം സൃഷ്ടിച്ചത്)
- `chat_template.jinja` — മോഡലിന്റെ ചാറ്റ് ടെംപ്ലേറ്റ് (സ്വയം സൃഷ്ടിച്ചത്)
- `tokenizer.json`, `tokenizer_config.json` — ടോക്കിനൈസർ ഫയലുകൾ
- വിവിധ മറ്റ് കോൺഫിഗറേഷൻ, വോകാബുലറി ഫയലുകൾ

#### ഘട്ടം 2: inference_model.json ഫയൽ ജനറേറ്റ് ചെയ്യുക

`inference_model.json` ഫയൽ Foundry Local-ന് പ്രാംപ്റ്റുകൾ എങ്ങനെ ഫോർമാറ്റ് ചെയ്യണമെന്നു പറയുന്നു. **റിപ്പോസിറ്ററി റൂട്ട്** (നിങ്ങളുടെ `models/` ഫോൾഡർ ഉണ്ടായിരിക്കുന്ന അതേ ഡയറക്ടറി)യിൽ `generate_chat_template.py` എന്ന Python സ്ക്രിപ്റ്റ് സൃഷ്ടിക്കുക:

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# ചാറ്റ് ടെംപ്ലേറ്റ് എടുക്കാൻ കുറഞ്ഞ conversation സൃഷ്ടിക്കുക
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

# inference_model.json ഘടന സൃഷ്ടിക്കുക
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

സ്ക്രിപ്റ്റ് റൺ ചെയ്യുക റിപ്പോസിറ്ററി റൂട്ട്-ൽ നിന്ന്:

```bash
python generate_chat_template.py
```

> **അറിയിപ്പ്:** `transformers` പാക്കേജ് `onnxruntime-genai`ന്റെ ആശ്രിതമായി മുൻപ് തന്നെ ഇൻസ്റ്റാൾ ചെയ്തിട്ടുണ്ട്. `ImportError` വരുകയാണെങ്കിൽ ആദ്യം `pip install transformers` റൺ ചെയ്യുക.

സ്ക്രിപ്റ്റ് `models/qwen3` ഡയറക്ടറിയിൽ `inference_model.json` സൃഷ്ടിക്കും. ഈ ഫയൽ Qwen3-നായി ഉപയോക്തൃ ഇൻപുട്ട് ശരിയായ സ്പെഷ്യൽ ടോക്കണുകളാൽ എങ്ങനെ ചുറ്റി പൊതിയണമെന്ന് Foundry Local-ന് പറയുന്നു.

> **അത്യാവശ്യമാണ്:** `inference_model.json`യിലെ `"Name"` ഫീൽഡ് (ഈ സ്ക്രിപ്റ്റിൽ `qwen3-0.6b` ആയി സെറ്റ് ചെയ്യപ്പെട്ടിരിക്കുന്നു) നിങ്ങൾ പിന്നീട് നടത്തන കമാൻഡുകൾക്കും API വിളികൾക്കും ഉപയോഗിക്കുന്ന **മോഡൽ അലിയസ്** ആണ്. നിങ്ങൾ ഈ പേര് മാറ്റിയാൽ അഭ്യാസങ്ങൾ 6 മുതൽ 10 വരെയുള്ള മോഡൽ നാമം അനുയായി അപ്‌ഡേറ്റ് ചെയ്യുക.

#### ഘട്ടം 3: കോൺഫിഗറേഷൻ പരിശോധന

`models/qwen3/inference_model.json` തുറന്ന് പരിശോധിക്കുക. അതിൽ `Name` ഫീൽഡ് മാത്രമല്ല `PromptTemplate` ഒബ്ജെക്റ്റിൽ `assistant` ഒപ്പം `prompt` കീകൾ ഉണ്ടാകണം. പ്രാംപ്റ്റ് ടെംപ്ലേറ്റിൽ `<|im_start|>` ഉം `<|im_end|>` ഉം പോലുള്ള പ്രത്യേക ടോക്കണുകൾ ഉൾപ്പെടണം (ടോക്കണുകൾ മോഡലിന്റെ ചാറ്റ് ടെംപ്ലേറ്റ് അനുസരിച്ച് വ്യത്യാസപ്പെടും).

> **മാനുവൽ ഓപ്ഷൻ:** സ്ക്രിപ്റ്റ് റൺ ചെയ്യാതിരിക്കുന്നുവെങ്കിൽ, ഫോൾഡർ സൃഷ്ടിച്ച് ഫയൽ നിങ്ങൾ സ്വന്തമായി സൃഷ്ടിക്കാം. പ്രധാനമായും, `prompt` ഫീൽഡ് മോഡലിന്റെ മുഴുവനായ ചാറ്റ് ടെംപ്ലേറ്റ് `{Content}` എന്ന പ്ലേസ്‌ഹോൾഡറോടുകൂടി ഉപയോക്തൃ സന്ദേശത്തിനായി ഉണ്ടാകണം.

---

### അഭ്യാസം 5: മോഡൽ ഡയറക്ടറി ഘടന പരിശോധിക്കുക
model builder നിങ്ങളുടെ निर्दिष्ट आउटपുട്ട് ഡയറക്ടറിയിൽ സങ്കലിപ്പിച്ച എല്ലാ ഫയലുകളും നേരിട്ട് വയ്ക്കുന്നു. അന്തിമ ഘടന ശരിയെന്ന് ഉറപ്പാക്കുക:

```bash
ls models/qwen3
```

ഡയർക്ടറിയിൽ താഴെ പറയുന്ന ഫയലുകൾ ഉണ്ടാകണം:

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

> **കുറിപ്പ്:** മറ്റു ചില സംയോജന ഉപകരണങ്ങളുമായി താരതമ്യം ചെയ്‌താൽ, model builder സബ് ഡയറക്ടറികൾ സൃഷ്‌ടിക്കുന്നില്ല. എല്ലാ ഫയലുകളും നേരിട്ട് പുറംഫോൾഡറിട്ടായിരിക്കും, അത് Foundry Local ആകാന് ആവശ്യമായതും ആണ്.

---

### Exercise 6: ഫോണ്ടറി ലോക്കൽ കാഷിലായി മോഡൽ ചേർക്കുക

നിങ്ങളുടെ സങ്കലിപ്പിച്ച മോഡലിനെ ഫൈളുകൾ ഫണ്ടറി ലോക്കലിനു് എവിടെ കണ്ടെത്താമെന്ന് അറിയിക്കുന്നതിന് ഡയറക്ടറി കാഷിലിൽ ചേർക്കുക:

```bash
foundry cache cd models/qwen3
```

മോഡൽ കാഷിലിൽ കാണപ്പെടുന്നുവെന്ന് സ്ഥിരീകരിക്കുക:

```bash
foundry cache ls
```

നിങ്ങളുടെ കസ്റ്റം മോഡൽ മുൻകൂർ കാഷിലിൽ ഉണ്ടായിരുന്ന മോഡലുകളുമായി (ഉദാഹരണത്തിന് `phi-3.5-mini` അല്ലെങ്കിൽ `phi-4-mini`) തുല്യപെട്ടതായി കാണും.

---

### Exercise 7: കസ്റ്റം മോഡൽ CLI ഉപയോഗിച്ച് అమർത്തുക

നിങ്ങളുടെ പുതിയത് സങ്കലിപ്പിച്ച മോഡലോടൊപ്പം ഇന്ററാക്ടീവ് ചാറ്റ് സെഷൻ ആരംഭിക്കുക (`qwen3-0.6b` നു് ങ്ങള് നൽകിയ `inference_model.json` ഫയലിലെ `Name` ഫീൽഡിലെ പേരാണ്):

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` ഫ്ലാഗ് അധിക ഡയഗ്നോസ്റ്റിക് വിവരങ്ങൾ കാണിക്കുന്നു, ഇത് കസ്റ്റം മോഡൽ ആദ്യമായി പരീക്ഷിക്കുമ്പോൾ സഹായിക്കുന്നു. മോഡൽ വിജയകരമായി ലോഡ് ചെയ്താൽ ഇന്ററാക്ടീവ് പ്രോമ്പ്റ്റ് കാണിക്കും. ചില സന്ദേശങ്ങൾ ശ്രമിക്കുക:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

സെഷൻ അവസാനിപ്പിക്കാൻ `exit` ടൈപ്പ് ചെയ്യുക അല്ലെങ്കിൽ `Ctrl+C` അമർത്തുക.

> **പരിഹാര നിർദ്ദേശങ്ങൾ:** മോഡൽ ലോഡ് ചെയ്യാൻ തകരാറുണ്ടെങ്കിൽ താഴെ പരിശോധിക്കുക:
> - `genai_config.json` ഫയൽ model builder സൃഷ്ടിച്ചതാണോ എന്ന് പരിശോധിക്കുക.
> - `inference_model.json` ഫയൽ ഉള്ളതും സാധുവായ JSON ആണെന്നും ഉറപ്പാക്കുക.
> - ONNX മോഡൽ ഫയലുകൾ ശരിയായ ഡയറക്ടറിയിലാണ്.
> - മതിയായ RAM ഉണ്ട് (Qwen3-0.6B int4 ഏകദേശം 1 GB ആവശ്യമാണ്).
> - Qwen3 ഒരു reasoning മോഡലാണ്, `<think>` ടാഗുകൾ പ്രൊഡ്യൂസ് ചെയ്യുന്നു. `<think>...</think>` ഫ്രീഫിക്‌സും പ്രതിപാദിക്കുന്ന മറുപടികൾ കാണുന്നത് സാധാരണമാണ്. `inference_model.json` ലെ പ്രോംപ്‌ട് ടെംപ്ലേറ്റ് മാറ്റിคิดം കാണിക്കൽ ഒഴിവാക്കാം.

---

### Exercise 8: REST API വഴി കസ്റ്റം മോഡലിന് ചോദിക്കുക

Exercise 7-ൽ ഇന്ററാക്ടീവ് സെഷൻ കഴിഞ്ഞെങ്കിൽ മോഡൽ ലോഡ് ചെയ്തിരിക്കില്ല. ആദ്യം Foundry Local സർവീസ് ആരംഭിച്ച് മോഡൽ ലോഡ് ചെയ്യുക:

```bash
foundry service start
foundry model load qwen3-0.6b
```

സർവീസ് ഏത് പോർട്ടിൽ പ്രവർത്തിക്കുന്നുവെന്ന് പരിശോധിക്കുക:

```bash
foundry service status
```

പിറകെ ഒരു അഭ്യർത്ഥന അയയ്ക്കുക (`5273` നിങ്ങളുടെ പോർടുമായി ഭിന്നമായിരുന്നെങ്കിൽ അതിന്റെ സ്ഥാനത്ത് മാറ്റുക):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows കുറിപ്പ്:** മുകളിൽ കാണുന്ന `curl` കമാൻഡ് ബാഷ് സിങ്ക്‌സ് ഉപയോഗിക്കുന്നു. Windows-ൽ പവർശെൽ `Invoke-RestMethod` കംഡ്ലറ്റ് താഴെക്കൊടുത്തതാണ്.

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

### Exercise 9: OpenAI SDK ഉപയോഗിച്ച് കസ്റ്റം മോഡൽ ഉപയോഗിക്കുക

നിങ്ങൾ ഉൾപ്പെടുത്തിയിരിക്കുന്ന മോഡലുകൾക്കായി ഉപയോഗിച്ച OpenAI SDK കോഡ് കൃത്യമായും കസ്റ്റം മോഡലിനും ഉപയോഗിക്കാം (കാണുക [ഭാഗം 3](part3-sdk-and-apis.md)). വ്യത്യാസം മാത്രം മോഡൽ നാമമാണ്.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # ഫൗണ്ട്‌റി ലോക്കൽ API കീകൾ പരിശോധിക്കുന്നില്ല
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
  apiKey: "foundry-local", // ഫൗണ്ട്രി ലോക്കൽ API കീകൾ സാധുവാക്കി പരിശോധിക്കുന്നില്ല
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

> **പ്രധാന മുദ്രാവാക്യം:** Foundry Local OpenAI-സമേതമായ API ഉം പുറപ്പെടുവിക്കുന്നതിനാൽ ഉൾപ്പെടുത്തിയ മോഡലുകൾക്കൊപ്പം പ്രവർത്തിക്കുന്ന ഏതൊരു കോഡും കസ്റ്റം മോഡലുകളോടും പ്രവർത്തിക്കും. നിങ്ങൾക്ക് ചെലവ് മാറ്റേണ്ടത് `model` പാരാമീറ്ററാണ്.

---

### Exercise 10: Foundry Local SDK ഉപയോഗിച്ച് കസ്റ്റം മോഡൽ പരിശോധന

മുന്‌പുള്ള ലബ്ബുകളിൽ Foundry Local SDK ഉപയോഗിച്ച് സർവീസ് തുടക്കം, എൻഡ്പോയിന്റ് കണ്ടെത്തൽ, മോഡൽ മാനേജ്മെന്റ് ഓട്ടോമാറ്റിക് ആയി ചെയ്തിട്ടുണ്ട്. നിങ്ങൾക്ക് സങ്കലിപ്പിച്ച മോഡലുമായി ഉടനെ അതേ മാതൃക പിന്തുടരാം. SDK സർവീസ് ആരംഭിക്കുകയും എൻഡ്പോയിന്റ് കണ്ടെത്തുകയും ചെയ്യും, അതിനാല് `localhost:5273` ഹാർഡ്-കോടിങ്ങ് ആവശ്യമില്ല.

> **കുറിപ്പ്:** ഈ ഉദാഹരണങ്ങൾ പ്രവർത്തിപ്പിക്കുന്നതിന് മുമ്പ് Foundry Local SDK ഇൻസ്റ്റാൾ ചെയ്തിട്ടുണ്ടെന്ന് ഉറപ്പാക്കുക:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` ഉം `OpenAI` NuGet പാക്കേജുകളും ചേർക്കുക
>
> ഓരോ സ്ക്രിപ്റ്റ് ഫയൽ **റെപ്പോസിറ്ററി റൂട്ടിൽ** (നിങ്ങളുടെ `models/` ഫോൾഡറിന്റെ ബദലായി) സൂക്ഷിക്കുക.

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# പടികടി 1: ഫൗണ്ടറി ലോക്കൽ സേവനം ആരംഭിച്ച് കസ്റ്റം മോഡൽ ലോഡ് ചെയ്യുക
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# പടികടി 2: കസ്റ്റം മോഡലിനായി കാഷെ പരിശോധിക്കുക
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# പടികടി 3: മോഡൽ മെമ്മറിയിലേക്ക് ലോഡ് ചെയ്യുക
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# പടികടി 4: SDK- കണ്ടെത്തിയ എൻഡ്‌പോയിന്റ് ഉപയോഗിച്ച് OpenAI ക്ലയന്റ് സൃഷ്ടിക്കുക
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# പടികടി 5: ഒരു സ്ട്രീമിംഗ് ചാറ്റ് പൂർത്തീകരണ അഭ്യർത്ഥന അയയ്ക്കുക
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

അത് പ്രവർത്തിപ്പിക്കുക:

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

// ഘട്ടം 1: ഫൗണ്ട്രി ലോക്കൽ സേവനം ആരംഭിക്കുക
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ഘട്ടം 2: കാറ്റലോഗിൽ നിന്നു കസ്റ്റം മോഡൽ നേടുക
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// ഘട്ടം 3: മോഡൽ മെമ്മറിയിൽ പാഴ്‌സുചെയ്യുക
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// ഘട്ടം 4: SDK കണ്ടെത്തിയ എൻഡ്‌പോയിന്റ് ഉപയോഗിച്ച് OpenAI ക്ലയന്റ് സൃഷ്ടിക്കുക
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// ഘട്ടം 5: സ്റ്റ്രീമിംഗ് ചാറ്റ് പൂർത്തീകരണ അഭ്യര്‍ത്ഥന അയയ്ക്കുക
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

അത് പ്രവർത്തിപ്പിക്കുക:

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

> **പ്രധാന മുദ്രാവാക്യം:** Foundry Local SDK എൻഡ്പോയിന്റ് ഡൈനമിക് ആയി കണ്ടെത്തുന്നു, അതുകൊണ്ട് നിങ്ങൾ ഒരിക്കലും പോർട്ട് നമ്പർ ഹാർഡ്-കോടിങ് ചെയ്യേണ്ടതില്ല. ഇത് ഉത്പാദന അപ്ലിക്കേഷനുകളിലേക്കുള്ള ശിപാർശ ചെയ്‌ത രീതി ആണ്. നിങ്ങളുടെ കസ്റ്റം സങ്കലിപ്പിച്ച മോഡൽ SDK വഴിയുള്ള ഉൾപ്പെടുത്തിയ കാറ്റലോഗ് മോഡലുകളെപ്പോലെ പ്രവർത്തിക്കും.

---

## സങ്കലിപ്പിക്കാനുള്ള മോഡലുകൾ തിരഞ്ഞെടുക്കൽ

ഈ ലബ്ബിൽ റഫറൻസ് എക്സാമ്പിളായി Qwen3-0.6B എടുത്തതാണ്. ഇത് ചെറിയത്, സങ്കലിപ്പിക്കാൻ വേഗതയുള്ളതും, ഉപയോഗിക്കാൻ സൗജന്യമായ Apache 2.0 ലൈസൻസിന് കീഴിലുള്ളതും ആണ്. എന്നാൽ, നിങ്ങൾക്ക് അനേകം മോഡലുകൾ സങ്കലിപ്പിക്കാമല്ലോ. ചില നിർദ്ദേശങ്ങൾ താഴെ:

| മോഡൽ | Hugging Face ID | പാരാമീറ്ററുകൾ | ലൈസൻസ് | കുറിപ്പുകൾ |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | വളരെ ചെറിയത്, വേഗം സങ്കലിപ്പിക്കാം, പരീക്ഷണങ്ങൾക്ക് നല്ലത് |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | മികച്ച ഗുണമേന്മ, ഇപ്പോഴും വേഗത്തിൽ സങ്കലിപ്പിക്കാം |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | ശക്തമായ ഗുണമേന്മ, ഊംറമുള്ള RAM ആവശ്യമാണ് |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face-ൽ ലൈസൻസ് അംഗീകാരം വേണം |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | ഉയർന്ന ഗുണമേന്മ, വലിയ ഡൗൺലോഡ്, നീണ്ട സങ്കലനം |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Foundry Local കാറ്റലോഗിൽ ഇതിനകം ഉണ്ട് (തമോലനത്തിനായി ഉപകാരപ്രദം) |

> **ലൈസൻസ് ഓർമ്മപ്പെടുത്തൽ:** അതുപോലെ മോഡൽ ഉപയോഗിക്കാൻ മുമ്പ് Hugging Face-ൽ മോഡലിന്റെ ലൈസൻസ് പരിശോധിക്കുക. ചില മോഡലുകൾ (ഉദാ: Llama) ലൈസൻസ് ഷർത്ത് അംഗീകരിക്കുകയും `huggingface-cli login` ഉപയോഗിച്ച് ഓഥന്റിക്കേറ്റ് ചെയ്‌തെടുക്കണമെന്നു് ആവശ്യപ്പെടുന്നു.

---

## ആശയങ്ങൾ: കസ്റ്റം മോഡലുകൾ എപ്പോഴ kullanmidt использовать

| സാഹചര്യം | എങ്കിൽ നിങ്ങൾ സ്വന്തമായി സങ്കലിപ്പിക്കേണ്ടതുണ്ട്? |
|----------|-----------------------------------|
| **നിങ്ങൾക്ക് വേണ്ട മോഡൽ കാറ്റലോഗിൽ ഇല്ല** | Foundry Local കാറ്റലോഗ് നിരീക്ഷിച്ചിരിക്കുന്നു. വേണ്ട മോഡൽ ലിസ്റ്റിൽ ഇല്ലെങ്കിൽ, സ്വയം സങ്കലിപ്പിക്കുക. |
| **Fine-tuned മോഡലുകൾ** | നിങ്ങൾ ഡൊമെയിൻ-സ്പെസിഫിക് ഡാറ്റയിൽ fine-tune ചെയ്‌തിട്ടുണ്ടെങ്കിൽ നിങ്ങളുടെ സ്വന്തം വെയ്റ്റുകൾ സങ്കലിപ്പിക്കേണ്ടതാണ്. |
| **നിർദിഷ്ട ക്വാണ്ടൈസേഷൻ ആവശ്യകതകൾ** | കാറ്റലോഗ് ഡിഫോൾട്ട് പരിഹാരത്തിൽ നിന്നു വ്യത്യസ്തമായ prescision അല്ലെങ്കിൽ quantisation രീതി ആഗ്രഹിക്കുന്നു എങ്കിൽ. |
| **പുതിയ മോഡൽ റിലീസുകൾ** | Hugging Face-ൽ പുതിയ മോഡൽ പുറത്തിറങ്ങുമ്പോൾ അത് Foundry Local കാറ്റലോഗിൽ ഉണ്ടായിരിക്ക nemusശേഷുക. തൽക്ഷണമേല്പ്പാടായി സങ്കലിപ്പിക്കുന്നത് ആക്‌സസ് നൽകും. |
| **ഗവേഷണവും പരീക്ഷണവും** | വിവിധ മോഡൽ ഘടനകൾ, വലുപ്പങ്ങൾ, കോൺഫിഗറേഷനുകൾ പ്രാദേശികമായി പരീക്ഷിച്ച് ഉത്പാദന തീരുമാനങ്ങൾക്കു മുൻപ് പരിശോധിക്കുക. |

---

## സർവ്വ സമാപനം

ഈ ലബ്ബിൽ നിങ്ങൾ പഠിച്ചത്:

| ഘട്ടം | നിങ്ങൾ ചെയ്തതു് |
|------|--------------|
| 1 | ONNX Runtime GenAI model builder ഇൻസ്റ്റാൾ ചെയ്തു |
| 2 | Hugging Face-ൽ നിന്ന് `Qwen/Qwen3-0.6B` സങ്കലിപ്പിച്ച് optimized ONNX മോഡലായി മാറ്റി |
| 3 | `inference_model.json` ചാറ്റ്-ടെംപ്ലേറ്റ് കോൺഫിഗറേഷൻ ഫയൽ സൃഷ്ടിച്ചു |
| 4 | സങ്കലിപ്പിച്ച മോഡൽ Foundry Local കാഷിലിൽ ചേർത്തു |
| 5 | CLI വഴി കസ്റ്റം മോഡലിൽ ഇന്ററാക്ടീവ് ചാറ്റ് നടത്തിയത് |
| 6 | OpenAI-സമേതമായ REST API വഴി മോഡലിനെ ചോദിച്ചു |
| 7 | Python, JavaScript, C# OpenAI SDK ഉപയോഗിച്ച് കണക്റ്റുള്ളത് |
| 8 | Foundry Local SDK വഴി കസ്റ്റം മോഡൽ ടെസ്റ്റ് ചെയ്തു |

പ്രധാനം: **ഏതൊരു ട്രാൻസ്ഫോർമർ അടിസ്ഥാനമാക്കിയ മോഡലും ONNX ഫോർമാറ്റിലേക്ക് സങ്കലിപ്പിച്ചിട്ടുണ്ടെങ്കിൽ Foundry Local-ൽ പ്രവർത്തിക്കും.** OpenAI-സമേതമായ API ക്ക് എല്ലാ നിലവിലുള്ള ആപ്ലിക്കേഷൻ കോഡും മാറ്റം കൂടാതെ പ്രവർത്തിക്കും; നിങ്ങൾക്ക് വെറും മോഡൽ നാമം മാറ്റേണ്ടതുണ്ട്.

---

## മുഖ്യ പാഠങ്ങൾ

| ആശയം | വിശദാംശം |
|---------|-------|
| ONNX Runtime GenAI Model Builder | Hugging Face മോഡലുകൾ ONNX ഫോർമാറ്റിലേക്ക് ക്വാണ്ടൈസേഷനൊപ്പം ഒന്നായി മാറ്റുന്നു |
| ONNX ഫോർമാറ്റ് | Foundry Local ONNX മോഡലുകളും ONNX Runtime GenAI കോൺഫിഗറേഷനും ആവശ്യപ്പെടുന്നു |
| ചാറ്റ് ടെംപ്ലേറ്റുകൾ | `inference_model.json` ഫയൽ Foundry Local-നു് പ്രോംപ്റ്റുകൾ എങ്ങനെയാണ് രൂപകൽപ്പന ചെയ്യേണ്ടതെന്ന് പറയുന്നു |
| ഹാർഡ്‌വെയർ ലക്ഷ്യങ്ങൾ | CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), WebGPU പോലുള്ള ഹാർഡ്‌വെയർ അടിസ്ഥാനത്തിൽ സങ്കലനം ചെയ്യാം |
| ക്വാണ്ടൈസേഷൻ | താഴ്ന്ന പ്രവർത്തനക്ഷമത (int4) വലുപ്പം കുറയ്ക്കുകയും വേഗത വർദ്ധിപ്പിക്കുകയും ചെയ്യുംfp16 GPU-കളിൽ ഉയർന്ന ഗുണമേന്മ നിലനിർത്തുന്നു |
| API സൗഹൃദം | കസ്റ്റം മോഡലുകൾ ഉൾപ്പെടുത്തി മോഡലുകളുമായി ഒരു പോലെ OpenAI എൻറഫേസ് ഉപയോഗിക്കുന്നു |
| Foundry Local SDK | SDK സർവീസ് തുടക്കം, എൻഡ്പോയിന്റ് കണ്ടെത്തൽ, മോഡൽ ലോഡ് ചെയ്യൽ ഓട്ടോമാറ്റിക് ആയി കൈകാര്യം ചെയ്യുന്നു കാറ്റലോഗും കസ്റ്റമും മോഡലുകൾക്കും|

---

## കൂടുതൽ വായനാമേഖലകൾ

| സ്രോതസ്സ് | ലിങ്ക് |
|----------|-------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local കസ്റ്റം മോഡൽ ഗൈഡ് | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 മോഡൽ കുടുംബം | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive ഡോക്യുമെന്റേഷൻ | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## അടുത്ത ഘട്ടങ്ങൾ

[Part 11: Tool Calling with Local Models](part11-tool-calling.md) ലേക്ക് തുടരുക, നിങ്ങളുടെ പ്രാദേശിക മോഡലുകൾക്ക് ബാഹ്യ ഫംഗ്ഷനുകൾ വിളിക്കാൻ എങ്ങനെ അനുവദിക്കാം എന്നത് പഠിക്കാം.

[← Part 9: Whisper Voice Transcription](part9-whisper-voice-transcription.md) | [Part 11: Tool Calling →](part11-tool-calling.md)