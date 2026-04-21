![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ਭਾਗ 10: Foundry Local ਨਾਲ ਕਸਟਮ ਜਾਂ Hugging Face ਮਾਡਲਾਂ ਦੀ ਵਰਤੋਂ

> **ਲਕੜੀ:** Hugging Face ਮਾਡਲ ਨੂੰ Foundry Local ਲਈ ਲੋੜੀਂਦੇ ਅਦੁੱਤੀਕਰਨ ਵਾਲੇ ONNX ਫਾਰਮੈਟ ਵਿੱਚ ਕਾਮਪਾਇਲ ਕਰੋ, ਇਹਨਾਂ ਨੂੰ ਚੈਟ ਟੈਮਪਲੇਟ ਨਾਲ ਸੰਰਚਿਤ ਕਰੋ, ਲੋਕਲ ਕੈਸ਼ ਵਿੱਚ ਸ਼ਾਮਿਲ ਕਰੋ, ਅਤੇ CLI, REST API ਅਤੇ OpenAI SDK ਦੀ ਵਰਤੋਂ ਨਾਲ ਇਸਦੇ ਖਿਲਾਫ ਇੰਫਰੈਂਸ ਚਲਾਓ।

## ਝਲਕ

Foundry Local ਪ੍ਰੀ-ਕੰਪਾਇਲ ਕੀਤੇ ਮਾਡਲਾਂ ਦੀ ਇੱਕ ਚੁਣੀ ਹੋਈ ਕੈਟਾਲੌਗ ਨਾਲ ਆਉਂਦਾ ਹੈ, ਪਰ ਤੁਸੀਂ ਇਸ ਸੂਚੀ ਤੱਕ ਸੀਮਤ ਨਹੀਂ ਹੋ। ਕਿਸੇ ਵੀ ਟ੍ਰਾਂਸਫਾਰਮਰ-ਅਧਾਰਿਤ ਭਾਸ਼ਾ ਮਾਡਲ ਜੋ [Hugging Face](https://huggingface.co/) 'ਤੇ ਉਪਲਬਧ ਹੈ (ਜਾਂ PyTorch / Safetensors ਫਾਰਮੈਟ ਵਿੱਚ ਲੋਕਲ ਸਟੋਰ ਕੀਤਾ ਹੋਇਆ) ਉਸਨੂੰ ਅਦੁੱਤੀਕਤ ONNX ਮਾਡਲ ਵਿੱਚ ਕਾਮਪਾਇਲ ਕੀਤਾ ਜਾ ਸਕਦਾ ਹੈ ਅਤੇ Foundry Local ਰਾਹੀਂ ਸਰਵ ਕੀਤਾ ਜਾ ਸਕਦਾ ਹੈ।

ਕੰਪਾਇਲੇਸ਼ਨ ਪਾਈਪਲਾਈਨ **ONNX Runtime GenAI Model Builder** ਦਾ ਉਪਯੋਗ ਕਰਦੀ ਹੈ, ਜੋ `onnxruntime-genai` ਪੈਕੇਜ ਨਾਲ ਆਉਣ ਵਾਲਾ ਕਮਾਂਡ-ਲਾਈਨ ਟੂਲ ਹੈ। ਮਾਡਲ ਬਿਲਡਰ ਭਾਰੀ ਕੰਮ ਸੰਭਾਲਦਾ ਹੈ: ਸਰੋਤ ਵਜਨ ਡਾਊਨਲੋਡ ਕਰਨਾ, ਓਨਰੂਪ ਵਿੱਚ ਬਦਲਣਾ, ਕੁਐਂਟੀਜ਼ੇਸ਼ਨ (int4, fp16, bf16) ਲਾਗੂ ਕਰਨਾ ਅਤੇ Foundry Local ਵੱਲੋਂ ਉਮੀਦ ਕੀਤੀਆਂ ਸੰਰਚਨਾ ਫਾਈਲਾਂ (ਜਿਵੇਂ ਚੈਟ ਟੈਮਪਲੇਟ ਅਤੇ ਟੋਕਨਾਈਜ਼ਰ) ਜਾਰੀ ਕਰਨਾ।

ਇਸ ਲੈਬ ਵਿੱਚ ਤੁਸੀਂ Hugging Face ਤੋਂ **Qwen/Qwen3-0.6B** ਕਾਮਪਾਇਲ ਕਰੋਗੇ, Foundry Local ਨਾਲ ਰਜਿਸਟਰ ਕਰੋਗੇ ਅਤੇ ਪੂਰੀ ਤਰ੍ਹਾਂ ਆਪਣੇ ਉਪਕਰਨ ਤੇ ਇਸਦੇ ਨਾਲ ਗੱਲਬਾਤ ਕਰੋਗੇ।

---

## ਸਿੱਖਣ ਦੇ ਉਦੇਸ਼

ਇਸ ਲੈਬ ਦੇ ਅੰਤ ਵਿੱਚ ਤੁਸੀਂ ਕਰ ਸਕੋਗੇ:

- ਸਮਝਾਓ ਕਿ ਕਸਟਮ ਮਾਡਲ ਕਾਮਪਾਇਲੇਸ਼ਨ ਕਿਉਂ ਲਾਭਦਾਇਕ ਹੈ ਅਤੇ ਤੁਹਾਨੂੰ ਕਦੋਂ ਲੋੜ ਪੈਂਦੀ ਹੈ
- ONNX Runtime GenAI ਮਾਡਲ ਬਿਲਡਰ ਨੂੰ ਇੰਸਟਾਲ ਕਰੋ
- Hugging Face ਮਾਡਲ ਨੂੰ ਇੱਕ ਕਮਾਂਡ ਨਾਲ ਅਦੁੱਤੀਕਤ ONNX ਫਾਰਮੈਟ ਵਿੱਚ ਕਾਮਪਾਇਲ ਕਰੋ
- ਮੁੱਖ ਕਾਮਪਾਇਲੇਸ਼ਨ ਪੈਰਾਮੀਟਰਾਂ (ਐਕਜ਼ੈਕਿਊਸ਼ਨ ਪ੍ਰੋਵਾਈਡਰ, ਪ੍ਰਿਸੀਸ਼ਨ) ਨੂੰ ਸਮਝੋ
- `inference_model.json` ਚੈਟ-ਟੈਮਪਲੇਟ ਸੰਰਚਨਾ ਫਾਈਲ ਬਣਾਓ
- ਇੱਕ ਕਾਮਪਾਇਲ ਮਾਡਲ ਨੂੰ Foundry Local ਕੈਸ਼ ਵਿੱਚ ਜੋੜੋ
- CLI, REST API ਅਤੇ OpenAI SDK ਦੀ ਵਰਤੋਂ ਨਾਲ ਕਸਟਮ ਮਾਡਲ 'ਤੇ ਇੰਫਰੈਂਸ ਚਲਾਓ

---

## ਪਹਿਲਾਂ ਤੋਂ ਲੋੜੀਂਦੀਆਂ ਚੀਜ਼ਾਂ

| ਲੋੜ | ਵੇਰਵਾ |
|-------------|---------|
| **Foundry Local CLI** | ਇੰਸਟਾਲ ਅਤੇ ਤੁਹਾਡੇ `PATH` 'ਚ ([ਭਾਗ 1](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI ਮਾਡਲ ਬਿਲਡਰ ਲਈ ਲੋੜੀਂਦਾ |
| **pip** | Python ਪੈਕੇਜ ਮੈਨੇਜਰ |
| **ਡਿਸਕ ਸਪੇਸ** | ਸਰੋਤ ਅਤੇ ਕਾਮਪਾਇਲ ਮਾਡਲ ਫਾਈਲਾਂ ਲਈ ਘੱਟੋ-ਘੱਟ 5 GB ਖਾਲੀ ਸਥਾਨ |
| **Hugging Face ਖਾਤਾ** | ਕੁਝ ਮਾਡਲਾਂ ਨੂੰ ਡਾਊਨਲੋਡ ਕਰਨ ਤੋਂ ਪਹਿਲਾਂ ਲਾਇਸੈਂਸ ਸਵੀਕਾਰ ਕਰਨ ਦੀ ਲੋੜ ਹੁੰਦੀ ਹੈ। Qwen3-0.6B Apache 2.0 ਲਾਇਸੈਂਸ ਨਾਲ ਮੁਫ਼ਤ ਉਪਲਬਧ ਹੈ। |

---

## ਵਾਤਾਵਰਣ ਸੈਟਅੱਪ

ਮਾਡਲ ਕਾਮਪਾਇਲੇਸ਼ਨ ਲਈ ਕਈ ਵੱਡੇ Python ਪੈਕੇਜ (PyTorch, ONNX Runtime GenAI, Transformers) ਲੋੜੀਂਦੇ ਹਨ। ਇੱਕ ਸਮਰਪਿਤ ਵਰਚੁਅਲ ਵਾਤਾਵਰਣ ਬਣਾਓ ਤਾਂ ਜੋ ਇਹ ਤੁਹਾਡੇ ਸਿਸਟਮ Python ਜਾਂ ਹੋਰ ਪ੍ਰਾਜੈਕਟਾਂ ਵਿੱਚ ਹਸਤਕਸ਼ੇਪ ਨਾ ਕਰਣ।

```bash
# ਰਿਪੋਜ਼ਿਟਰੀ ਦੀ ਜੜ੍ਹ ਤੋਂ
python -m venv .venv
```
  
ਵਾਤਾਵਰਨ ਚਾਲੂ ਕਰੋ:

**Windows (PowerShell):**  
```powershell
.venv\Scripts\Activate.ps1
```
  
**macOS / Linux:**  
```bash
source .venv/bin/activate
```
  
pip ਅਪਡੇਟ ਕਰੋ ਤਾਂ ਜੋ ਡਿਪੇਂਡੈਂਸੀ ਹੱਲ ਕਰਨ ਦੀ ਸਮੱਸਿਆ ਨਾ ਆਵੇ:

```bash
python -m pip install --upgrade pip
```
  
> **ਸੁਝਾਅ:** ਜੇ ਤੁਹਾਡੇ ਕੋਲ ਪਹਿਲਾਂ ਦੇ ਲੈਬਾਂ ਤੋਂ `.venv` ਹੈ, ਤਾਂ ਤੁਸੀਂ ਇਸਨੂੰ ਮੁੜ ਵਰਤ ਸਕਦੇ ਹੋ। ਸਿਰਫ ਇਹ ਯਕੀਨੀ ਬਣਾਓ ਕਿ ਅਗਲੇ ਕਦਮ ਤੋਂ ਪਹਿਲਾਂ ਇਹ ਐਕਟੀਵੇਟ ਹੈ।

---

## ਧਾਰਣਾ: ਕਾਮਪਾਇਲੇਸ਼ਨ ਪਾਈਪਲਾਈਨ

Foundry Local ਨੂੰ ONNX ਰੂਪ ਵਿੱਚ ਮਾਡਲਾਂ ਦੀ ਲੋੜ ਹੈ ਜੋ ONNX Runtime GenAI ਸੰਰਚਨਾ ਦੇ ਨਾਲ ਹੋਵੀ। ਜ਼ਿਆਦਾਤਰ ਖੁਲ੍ਹੇ ਸਰੋਤ ਮਾਡਲ Hugging Face 'ਤੇ PyTorch ਜਾਂ Safetensors ਵਜ਼ਨਾਂ ਵਿੱਚ ਵੰਡੇ ਜਾਂਦੇ ਹਨ, ਇਸ ਲਈ ਉਹਨਾਂ ਨੂੰ ਬਦਲਣ ਦੀ ਲੋੜ ਹੁੰਦੀ ਹੈ।

![ਕਸਟਮ ਮਾਡਲ ਕਾਮਪਾਇਲੇਸ਼ਨ ਪਾਈਪਲਾਈਨ](../../../images/custom-model-pipeline.svg)

### ਮਾਡਲ ਬਿਲਡਰ ਕੀ ਕਰਦਾ ਹੈ?

1. Hugging Face ਤੋਂ ਸਰੋਤ ਮਾਡਲ ਡਾਊਨਲੋਡ ਕਰਦਾ ਹੈ (ਜਾਂ ਇਹਨਾਂ ਨੂੰ ਲੋਕਲ ਪਾਥ ਤੋਂ ਪੜ੍ਹਦਾ ਹੈ)।  
2. PyTorch / Safetensors ਵਜ਼ਨਾਂ ਨੂੰ ONNX ਫਾਰਮੈਟ ਵਿੱਚ ਬਦਲਦਾ ਹੈ।  
3. ਮਾਡਲ ਨੂੰ ਛੋਟੇ ਪ੍ਰਿਸੀਸ਼ਨ (ਜਿਵੇਂ int4) ਵਿੱਚ ਕੁਐਂਟੀਜ਼ ਕਰਦਾ ਹੈ ਤਾ ਕਿ ਮੈਮੋਰੀ ਖਪਤ ਘੱਟ ਹੋਵੇ ਅਤੇ ਪ੍ਰਦਰਸ਼ਨ ਵਧੇ।  
4. ONNX Runtime GenAI ਸੰਰਚਨਾ (`genai_config.json`), ਚੈਟ ਟੈਮਪਲੇਟ (`chat_template.jinja`) ਅਤੇ ਸਾਰੇ ਟੋਕਨਾਈਜ਼ਰ ਫਾਈਲਾਂ ਨਿਕਾਲਦਾ ਹੈ ਤਾਂ ਜੋ Foundry Local ਮਾਡਲ ਨੂੰ ਲੋਡ ਅਤੇ ਸਰਵ ਕਰ ਸਕੇ।  

### ONNX Runtime GenAI Model Builder ਤੇ Microsoft Olive ਵਿੱਚ ਫ਼ਰਕ

ਤੁਸੀਂ ਕਈ ਵਾਰੀ **Microsoft Olive** ਨੂੰ ਮਾਡਲ ਅਪਟੀਮਾਈਜ਼ੇਸ਼ਨ ਲਈ ਇੱਕ ਵਿਕਲਪ ਟੂਲ ਵਜੋਂ ਵੇਖ ਸਕਦੇ ਹੋ। ਦੋਹਾਂ ਟੂਲ ONNX ਮਾਡਲ ਤਿਆਰ ਕਰ ਸਕਦੇ ਹਨ, ਪਰ ਉਹਨਾਂ ਦੇ ਵਰਤੇ ਜਾਨ ਵਾਲੇ ਮਕਸਦ ਅਤੇ ਲਾਭ-ਨੁਕਸਾਨ ਵਿੱਚ ਅੰਤਰ ਹੁੰਦਾ ਹੈ:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **ਪੈਕੇਜ** | `onnxruntime-genai` | `olive-ai` |
| **ਮੁੱਖ ਮਕਸਦ** | ONNX Runtime GenAI ਲਈ ਜਨਰੇਟਿਵ AI ਮਾਡਲ ਬਦਲਣਾ ਅਤੇ ਕੁਐਂਟੀਜ਼ ਕਰਨਾ | ਕਈ ਬੈਕਐਂਡ ਅਤੇ ਹਾਰਡਵੇਅਰ ਲਈ ਅਖੀਰ-ਤਕ ਮਾਡਲ ਅਪਟੀਮਾਈਜ਼ੇਸ਼ਨ ਫਰੇਮਵਰਕ |
| **ਆਸਾਨੀ** | ਇਕ ਕਮਾਂਡ — ਇੱਕ ਕਦਮ ਵਿੱਚ ਬਦਲਾਅ ਅਤੇ ਕੁਐਂਟੀਜ਼ੇਸ਼ਨ | ਵਰਕਫਲੋਅ-ਅਧਾਰਿਤ — YAML/JSON ਦੇ ਨਾਲ ਬਹੁ-ਪਾਸ ਪਾਈਪਲਾਈਨਾਂ |
| **ਆਉਟਪੁਟ ਫਾਰਮੈਟ** | ONNX Runtime GenAI ਫਾਰਮੈਟ (Foundry Local ਲਈ ਤਿਆਰ) | ਜੈਨੇਰਿਕ ONNX, ONNX Runtime GenAI, ਜਾਂ ਹੋਰ ਵਰਕਫਲੋਅ ਤੇ ਨਿਰਭਰ |
| **ਹਾਰਡਵੇਅਰ ਟਾਰਗੇਟ** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN, ਆਦਿ |
| **ਕੁਐਂਟੀਜ਼ੇਸ਼ਨ ਵਿਕਲਪ** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16, ਨਾਲ ਗਰਾਫ ਅਪਟੀਮਾਈਜ਼ੇਸ਼ਨ, ਲੇਅਰ-ਵਾਈਜ਼ ਟਿūਨਿੰਗ |
| **ਮਾਡਲ ਵਿਆਪਕਤਾ** | ਜਨਰੇਟਿਵ AI ਮਾਡਲ (LLMs, SLMs) | ਕੋਈ ਵੀ ONNX ਵਿੱਚ ਬਦਲਾ ਜਾ ਸਕਣ ਵਾਲਾ ਮਾਡਲ (ਦ੍ਰਿਸ਼ਟੀ, NLP, ਆਡੀਓ, ਮਲਟੀਮੋਡਲ) |
| **ਕਿਸ ਲਈ ਵਧੀਆ** | ਸਥਾਨਕ ਇੰਫਰੈਂਸ ਲਈ ਤੇਜ਼ ਅਤੇ ਸੋਧੀ ਕਾਮਪਾਇਲੇਸ਼ਨ | ਉਤਪਾਦਨ ਪਾਈਪਲਾਈਨਾਂ ਜਿਨ੍ਹਾਂ ਵਿੱਚ ਬਰੀਕੀ ਨਾਲ ਅਪਟੀਮਾਈਜ਼ੇਸ਼ਨ ਦੀ ਲੋੜ ਹੈ |
| **ਡਿਪੇਂਡੈਂਸੀ** | ਮੋਡਰੇਟ (PyTorch, Transformers, ONNX Runtime) | ਵੱਡੀਆਂ (Olive ਫਰੇਮਵਰਕ ਅਤੇ ਵਿਕਲਪਿਕ ਐਕਸਟਰਾ ਜੋੜਦਾ ਹੈ) |
| **Foundry Local ਇੰਟੀਗ੍ਰੇਸ਼ਨ** | ਸਿੱਧਾ — ਤੁਰੰਤ ਕੰਮ ਕਰਨ ਵਾਲਾ ਫਾਰਮੈਟ | `--use_ort_genai` ਫਲੈਗ ਅਤੇ ਵਾਧੂ ਸੰਰਚਨਾ ਲੋੜੀਂਦੀ |

> **ਇਸ ਲੈਬ ਵਿੱਚ ਮਾਡਲ ਬਿਲਡਰ ਕਿਉਂ ਵਰਤਿਆ ਜਾਂਦਾ ਹੈ:** ਇੱਕ Hugging Face ਮਾਡਲ ਇੱਕ ਵਾਰੀ ਕਾਮਪਾਇਲ ਕਰਨ ਅਤੇ Foundry Local ਨਾਲ ਰਜਿਸਟਰ ਕਰਨ ਲਈ ਮਾਡਲ ਬਿਲਡਰ ਸਭ ਤੋਂ ਸਾਦਾ ਅਤੇ ਭਰੋਸੇਮੰਦ ਵਿਕਲਪ ਹੈ। ਇਹ ਇੱਕ ਕਮਾਂਡ ਵਿੱਚ ਬਿਲਕੁਲ ਉਹੀ ਫਾਰਮੈਟ ਨਿਕਾਲਦਾ ਹੈ ਜੋ Foundry Local ਨੂੰ ਚਾਹੀਦਾ ਹੈ। ਜੇ ਤੁਹਾਨੂੰ ਵਧੇਰੇ ਸੰਵਿਦਨਸ਼ੀਲ ਅਪਟੀਮਾਈਜ਼ੇਸ਼ਨ ਜਿਵੇਂ ਕਿ ਸਹੀਅਤ-ਜਾਣੂ ਕੁਐਂਟੀਜ਼ੇਸ਼ਨ, ਗਰਾਫ ਸਰਜਰੀ ਜਾਂ ਬਹੁ-ਪਾਸ ਟਿūਨਿੰਗ ਦੀ ਲੋੜ ਹੋਵੇ, ਤਾਂ Olive ਇੱਕ ਸ਼ਕਤੀਸ਼ਾਲੀ ਚੋਣ ਹੈ। ਵਿਸਥਾਰ ਲਈ [Microsoft Olive ਡੌਕਯੂਮੈਂਟੇਸ਼ਨ](https://microsoft.github.io/Olive/) ਵੇਖੋ।

---

## ਲੈਬ ਅਭਿਆਸ

### ਅਭਿਆਸ 1: ONNX Runtime GenAI Model Builder ਇੰਸਟਾਲ ਕਰੋ

ONNX Runtime GenAI ਪੈਕੇਜ ਇੰਸਟਾਲ ਕਰੋ ਜਿਸ ਵਿੱਚ ਮਾਡਲ ਬਿਲਡਰ ਟੂਲ ਸ਼ਾਮਿਲ ਹੈ:

```bash
pip install onnxruntime-genai
```
  
ਇੰਸਟਾਲੇਸ਼ਨ ਦੀ ਪੁਸ਼ਟੀ ਕਰਨ ਲਈ ਦੇਖੋ ਕਿ ਮਾਡਲ ਬਿਲਡਰ ਉਪਲਬਧ ਹੈ:

```bash
python -m onnxruntime_genai.models.builder --help
```
  
ਤੁਹਾਨੂੰ ਹੈਲਪ ਆਉਟਪੁੱਟ ਵਿੱਚ ਕਿਸੇ ਪੈਰਾਮੀਟਰ ਜਿਵੇਂ `-m` (ਮਾਡਲ ਨਾਮ), `-o` (ਆਉਟਪੁੱਟ ਪਾਥ), `-p` (ਪ੍ਰਿਸੀਸ਼ਨ), ਅਤੇ `-e` (ਐਕਜ਼ੈਕਿਊਸ਼ਨ ਪ੍ਰੋਵਾਈਡਰ) ਦੀ ਜਾਣਕਾਰੀ ਮਿਲਣੀ ਚਾਹੀਦੀ ਹੈ।

> **ਨੋਟ:** ਮਾਡਲ ਬਿਲਡਰ PyTorch, Transformers ਅਤੇ ਕੁਝ ਹੋਰ ਪੈਕੇਜਾਂ 'ਤੇ ਨਿਰਭਰ ਕਰਦਾ ਹੈ। ਇੰਸਟਾਲੇਸ਼ਨ ਨੂੰ ਕੁਝ ਮਿੰਟ ਲੱਗ ਸਕਦੇ ਹਨ।

---

### ਅਭਿਆਸ 2: CPU ਲਈ Qwen3-0.6B ਕਾਮਪਾਇਲ ਕਰੋ

ਹੇਠਾਂ ਦਿੱਤਾ ਕਮਾਂਡ ਚਲਾਓ ਤਾਂ ਜੋ Qwen3-0.6B ਮਾਡਲ Hugging Face ਤੋਂ ਡਾਊਨਲੋਡ ਹੋਕੇ CPU ਇੰਫਰੈਂਸ ਲਈ int4 ਕੁਐਂਟੀਜ਼ੇਸ਼ਨ ਨਾਲ ਕਾਮਪਾਇਲ ਹੋ ਜਾਵੇ:

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
  
#### ਹਰ ਪੈਰਾਮੀਟਰ ਦੀ ਵਿਵਰਣਾ

| ਪੈਰਾਮੀਟਰ | ਮਕਸਦ | ਵਰਤੀ ਗਈ ਮੁੱਲ |
|-----------|---------|------------|
| `-m` | Hugging Face ਮਾਡਲ ID ਜਾਂ ਲੋਕਲ ਡਾਇਰੈਕਟਰੀ ਪਾਥ | `Qwen/Qwen3-0.6B` |
| `-o` | ਡਾਇਰੈਕਟਰੀ ਜਿੱਥੇ ਕਾਮਪਾਇਲ ਕੀਤੇ ਮਾਡਲ ਸੇਵ ਹੋਣਗੇ | `models/qwen3` |
| `-p` | ਕਾਮਪਾਇਲੇਸ਼ਨ ਦੌਰਾਨ ਲਾਗੂ ਕੀਤੀ ਗਈ ਕੁਐਂਟੀਜ਼ੇਸ਼ਨ ਪ੍ਰਿਸੀਸ਼ਨ | `int4` |
| `-e` | ONNX Runtime ਐਕਜ਼ੈਕਿਊਸ਼ਨ ਪ੍ਰੋਵਾਈਡਰ (ਟਾਰਗੇਟ ਹਾਰਡਵੇਅਰ) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face ਅਥੈਂਟੀਕੇਸ਼ਨ ਛੱਡਦਾ ਹੈ (ਪਬਲਿਕ ਮਾਡਲ ਲਈ ਠੀਕ) | `hf_token=false` |

> **ਇਸ ਵਿੱਚ ਕਿੰਨਾ ਸਮਾਂ ਲੱਗਦਾ ਹੈ?** ਕਾਮਪਾਇਲੇਸ਼ਨ ਦਾ ਸਮਾਂ ਤੁਹਾਡੇ ਹਾਰਡਵੇਅਰ ਅਤੇ ਮਾਡਲ ਦੇ ਆਕਾਰ 'ਤੇ ਨਿਰਭਰ ਕਰਦਾ ਹੈ। Qwen3-0.6B ਲਈ int4 ਕੁਐਂਟੀਜ਼ੇਸ਼ਨ ਨਾਲ ਆਧੁਨਿਕ CPU 'ਤੇ ਲਗਭਗ 5 ਤੋਂ 15 ਮਿੰਟ। ਵੱਡੇ ਮਾਡਲਾਂ ਨੂੰ ਬਹੁਤ ਵੱਧ ਸਮਾਂ ਲੱਗਦਾ ਹੈ।

ਜਦੋਂ ਕਮਾਂਡ ਪੂਰੀ ਹੋ ਜਾਵੇ ਤਾਂ ਤੁਹਾਨੂੰ ਕਾਮਪਾਇਲ ਕੀਤੇ ਮਾਡਲ ਫਾਈਲਾਂ ਵਾਲੀ `models/qwen3` ਡਾਇਰੈਕਟਰੀ ਮਿਲਣੀ ਚਾਹੀਦੀ ਹੈ। ਆਉਟਪੁੱਟ ਦੀ ਜਾਂਚ ਕਰੋ:

```bash
ls models/qwen3
```
  
ਆਪਣੇ ਕੋਲ ਇਹਨਾਂ ਵਿੱਚੋਂ ਕੁਝ ਫਾਈਲਾਂ ਹੋਣੀਆਂ ਚਾਹੀਦੀਆਂ ਹਨ:  
- `model.onnx` ਅਤੇ `model.onnx.data` — ਕਾਮਪਾਇਲ ਕੀਤੇ ਮਾਡਲ ਵਜ਼ਨ  
- `genai_config.json` — ONNX Runtime GenAI ਸੰਰਚਨਾ  
- `chat_template.jinja` — ਮਾਡਲ ਦਾ ਚੈਟ ਟੈਮਪਲੇਟ (ਆਟੋ-ਜਨਰੇਟਡ)  
- `tokenizer.json`, `tokenizer_config.json` — ਟੋਕਨਾਈਜ਼ਰ ਫਾਈਲਾਂ  
- ਹੋਰ ਵੋਕੈਬੂਲੇਰੀ ਅਤੇ ਸੰਰਚਨਾ ਫਾਈਲਾਂ

---

### ਅਭਿਆਸ 3: GPU ਲਈ ਕਾਮਪਾਇਲ ਕਰੋ (ਵਿਕਲਪਿਕ)

ਜੇ ਤੁਹਾਡੇ ਕੋਲ CUDA ਸਹਿਯੋਗ ਵਾਲੀ NVIDIA GPU ਹੈ, ਤਾਂ ਤੁਸੀਂ ਤੇਜ਼ ਇੰਫਰੈਂਸ ਲਈ GPU ਅਨੁਕੂਲ ਵਰਜਨ ਕਾਮਪਾਇਲ ਕਰ ਸਕਦੇ ਹੋ:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```
  
> **ਨੋਟ:** GPU ਕਾਮਪਾਇਲੇਸ਼ਨ ਲਈ `onnxruntime-gpu` ਅਤੇ ਸਹੀ ਤਰੀਕੇ ਨਾਲ CUDA ਇੰਸਟਾਲੇਸ਼ਨ ਲਾਜ਼ਮੀ ਹੈ। ਜੇ ਇਹ ਮੌਜੂਦ ਨਹੀਂ, ਤਾਂ ਮਾਡਲ ਬਿਲਡਰ ਐਰਰ ਦਿਖਾਏਗਾ। ਤੁਸੀਂ ਇਹ ਅਭਿਆਸ ਛੱਡ ਕੇ CPU ਵਰਜਨ ਨਾਲ ਜਾਰੀ ਰੱਖ ਸਕਦੇ ਹੋ।

#### ਹਾਰਡਵੇਅਰ-ਨਿਰਧਾਰਿਤ ਕਾਮਪਾਇਲੇਸ਼ਨ ਸੂਚੀ

| ਟਾਰਗੇਟ | ਐਕਜ਼ੈਕਿਊਸ਼ਨ ਪ੍ਰੋਵਾਈਡਰ (`-e`) | ਸਿਫਾਰਸ਼ ਕੀਤੀ ਪ੍ਰਿਸੀਸ਼ਨ (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` ਜਾਂ `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` ਜਾਂ `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### ਪ੍ਰਿਸੀਸ਼ਨ ਦੇ ਫਾਇਦੇ-ਨੁਕਸਾਨ

| ਪ੍ਰਿਸੀਸ਼ਨ | ਆਕਾਰ | ਗਤੀ | ਕੁਆਲਿਟੀ |
|-----------|------|-------|---------|
| `fp32` | ਸਭ ਤੋਂ ਵੱਡਾ | ਸਭ ਤੋਂ ਧੀਮਾ | ਸਭ ਤੋਂ ਵਧੀਆ ਸਹੀਅਤ |
| `fp16` | ਵੱਡਾ | ਤੇਜ਼ (GPU) | ਬਹੁਤ ਚੰਗੀ ਸਹੀਅਤ |
| `int8` | ਛੋਟਾ | ਤੇਜ਼ | ਹਲਕੀ ਸਹੀਅਤ ਘਾਟ |
| `int4` | ਸਭ ਤੋਂ ਛੋਟਾ | ਸਭ ਤੋਂ ਤੇਜ਼ | ਦਰਮਿਆਨੀ ਸਹੀਅਤ ਘਾਟ |

ਆਮ ਤੌਰ ਤੇ ਸਥਾਨਕ ਵਿਕਾਸ ਲਈ CPU ਤੇ `int4` ਸਭ ਤੋਂ ਵਧੀਆ ਗਤੀ ਅਤੇ ਸਰੋਤ ਖਪਤ ਦਾ ਸੰਤੁਲਨ ਦਿੰਦਾ ਹੈ। ਉਤਪਾਦਨ-ਗੁਣਵੱਤਾ ਲਈ CUDA GPU 'ਤੇ `fp16` ਸਿਫਾਰਸ਼ੀਯ ਹੈ।

---

### ਅਭਿਆਸ 4: ਚੈਟ ਟੈਮਪਲੇਟ ਸੰਰਚਨਾ ਬਣਾਓ

ਮਾਡਲ ਬਿਲਡਰ ਆਟੋਮੈਟਿਕ ਤੌਰ ਤੇ ਆਉਟਪੁੱਟ ਡਾਇਰੈਕਟਰੀ ਵਿੱਚ `chat_template.jinja` ਅਤੇ `genai_config.json` ਬਣਾਉਂਦਾ ਹੈ। ਪਰ Foundry Local ਨੂੰ ਇੱਕ `inference_model.json` ਫਾਈਲ ਦੀ ਵੀ ਲੋੜ ਹੁੰਦੀ ਹੈ ਜਿਸ ਵਿੱਚ ਦੱਸਿਆ ਜਾਂਦਾ ਹੈ ਕਿ ਮਾਡਲ ਲਈ ਪ੍ਰੋਮਪਟ ਕਿਸ ਤਰ੍ਹਾਂ ਬਣਾਉਣਾ ਹੈ। ਇਹ ਫਾਈਲ ਮਾਡਲ ਦਾ ਨਾਮ ਅਤੇ ਪ੍ਰੋਮਪਟ ਟੈਮਪਲੇਟ ਪਰਿਭਾਸ਼ਿਤ ਕਰਦੀ ਹੈ ਜੋ ਯੂਜ਼ਰ ਦੇ ਸੁਨੇਹਿਆਂ ਨੂੰ ਸਹੀ ਖਾਸ ਟੋਕਨ ਵਿੱਚ ਲਪੇਟਦਾ ਹੈ।

#### ਕਦਮ 1: ਕਾਮਪਾਇਲ ਆਉਟਪੁੱਟ ਦੀ ਜਾਂਚ ਕਰੋ

ਕਾਮਪਾਇਲ ਕੀਤੇ ਮਾਡਲ ਫੋਲਡਰ ਦੇ ਸਮੱਗਰੀ ਲਿਸਟ ਕਰੋ:

```bash
ls models/qwen3
```
  
ਤੁਹਾਨੂੰ ਇਹਨਾਂ ਵਿੱਚੋਂ ਫਾਈਲਾਂ ਮਿਲਣਗੀਆਂ:  
- `model.onnx` ਅਤੇ `model.onnx.data` — ਕਾਮਪਾਇਲ ਮਾਡਲ ਵਜ਼ਨ  
- `genai_config.json` — ONNX Runtime GenAI ਸੰਰਚਨਾ (ਆਟੋ-ਜਨਰੇਟਡ)  
- `chat_template.jinja` — ਮਾਡਲ ਦਾ ਚੈਟ ਟੈਮਪਲੇਟ (ਆਟੋ-ਜਨਰੇਟਡ)  
- `tokenizer.json`, `tokenizer_config.json` — ਟੋਕਨਾਈਜ਼ਰ ਫਾਈਲਾਂ  
- ਹੋਰ ਵੱਖ-ਵੱਖ ਸੰਰਚਨਾ ਅਤੇ ਸ਼ਬਦ-ਸੰਗ੍ਰਹਿ ਫਾਈਲਾਂ

#### ਕਦਮ 2: inference_model.json ਫਾਈਲ ਬਣਾਓ

`inference_model.json` ਫਾਈਲ Foundry Local ਨੂੰ ਦੱਸਦੀ ਹੈ ਕਿ ਪ੍ਰੋਮਪਟ ਕਿਸ ਤਰ੍ਹਾਂ ਬਣਾਉਣਾ ਹੈ। ਇੱਕ Python ਸਕ੍ਰਿਪਟ `generate_chat_template.py` ਬਣਾਓ **ਰਿਪੋਜ਼ਟਰੀ ਦੀ ਜੜ ਵਿੱਚ** (ਉਹੀ ਡਾਇਰੈਕਟਰੀ ਜਿੱਥੇ `models/` ਫੋਲਡਰ ਹੈ):

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# ਗੱਲਬਾਤ ਦਾ ਇੱਕ ਸਭ ਤੋਂ ਘੱਟ ਸੰਵਾਦ ਬਣਾਓ ਤਾਂ ਜੋ ਚੈਟ ਟੈਂਪਲੇਟ ਪ੍ਰਾਪਤ ਕੀਤਾ ਜਾ ਸਕੇ
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

# inference_model.json ਸਟਰੱਕਚਰ ਬਣਾਓ
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
  
ਰਿਪੋਜ਼ਟਰੀ ਰੂਟ ਤੋਂ ਸਕ੍ਰਿਪਟ ਚਲਾਓ:

```bash
python generate_chat_template.py
```
  
> **ਨੋਟ:** `transformers` ਪੈਕੇਜ `onnxruntime-genai` ਦੀ ਡਿਪੇਂਡੈਂਸੀ ਵਜੋਂ ਪਹਿਲਾਂ ਹੀ ਇੰਸਟਾਲ ਹੈ। ਜੇ `ImportError` ਆਵੇ ਤਾਂ ਪਹਿਲਾਂ `pip install transformers` ਚਲਾਓ।

ਸਕ੍ਰਿਪਟ `models/qwen3` ਡਾਇਰੈਕਟਰੀ ਵਿੱਚ `inference_model.json` ਬਣਾਏਗਾ। ਇਸ ਫਾਈਲ ਵਿੱਚ Foundry Local ਨੂੰ ਦੱਸਿਆ ਜਾਂਦਾ ਹੈ ਕਿ Qwen3 ਲਈ ਯੂਜ਼ਰ ਇਨਪੁੱਟ ਨੂੰ ਸਹੀ ਖਾਸ ਟੋਕਨਾਂ ਵਿੱਚ ਕਿਵੇਂ ਲਪੇਟਣਾ ਹੈ।

> **ਮਹੱਤਵਪੂਰਨ:** `inference_model.json` ਵਿੱਚ `"Name"` ਫੀਲਡ (ਇਸ ਸਕ੍ਰਿਪਟ ਵਿੱਚ `qwen3-0.6b` ਸੈਟ ਕੀਤੀ ਗਈ) ਉਹ **ਮਾਡਲ ਅਲਿਆਸ** ਹੈ ਜੋ ਤੁਸੀਂ ਭਵਿੱਖ ਦੇ ਸਾਰੇ ਕਮਾਂਡਾਂ ਅਤੇ API ਕਾਲਾਂ ਵਿੱਚ ਵਰਤੋਂਗੇ। ਜੇ ਤੁਸੀਂ ਨਾਮ ਬਦਲਦੇ ਹੋ, ਤਾਂ ਅਭਿਆਸ 6–10 ਵਿੱਚ ਵੀ ਇਹ ਨਾਮ ਅਪਡੇਟ ਕਰੋ।

#### ਕਦਮ 3: ਸੰਰਚਨਾ ਦੀ ਜਾਂਚ ਕਰੋ

`models/qwen3/inference_model.json` ਖੋਲ੍ਹੋ ਅਤੇ ਪੱਕਾ ਕਰੋ ਕਿ ਇਸ ਵਿੱਚ `Name` ਫੀਲਡ ਅਤੇ ਇੱਕ `PromptTemplate` ਵਸਤੂ ਦਿੱਤੀ ਗਈ ਹੈ ਜਿਸ ਵਿੱਚ `assistant` ਅਤੇ `prompt` ਕੀਜ਼ ਹਨ। ਪ੍ਰੋਮਪਟ ਟੈਮਪਲੇਟ ਵਿੱਚ ਖਾਸ ਟੋਕਨ ਜਿਵੇਂ `<|im_start|>` ਅਤੇ `<|im_end|>` ਸ਼ਾਮਿਲ ਹੋਣੇ ਚਾਹੀਦੇ ਹਨ (ਟੋਕਨਾਂ ਦੀ ਬਿਲਕੁਲ ਸਹੀ ਕਿਸਮ ਮਾਡਲ ਦੇ ਚੈਟ ਟੈਮਪਲੇਟ 'ਤੇ ਨਿਰਭਰ ਕਰਦੀ ਹੈ)।

> **ਦਸਤਕ ਨਾਮਾ ਵਿਕਲਪ:** ਜੇ ਤੁਸੀਂ ਸਕ੍ਰਿਪਟ ਚਲਾਉਣਾ ਨਹੀਂ ਚਾਹੁੰਦੇ, ਤਾਂ ਫਾਈਲ ਹੱਥੋਂ ਵੀ ਬਣਾ ਸਕਦੇ ਹੋ। ਮੁੱਖ ਗੱਲ ਇਹ ਹੈ ਕਿ `prompt` ਫੀਲਡ ਵਿੱਚ `{Content}` ਨਾਲ ਮਾਡਲ ਦਾ ਪੂਰਾ ਚੈਟ ਟੈਮਪਲੇਟ ਹੋਵੇ ਜੋ ਯੂਜ਼ਰ ਦੇ ਸੁਨੇਹੇ ਲਈ ਪਲੇਸਹੋਲਡਰ ਹੈ।

---

### ਅਭਿਆਸ 5: ਮਾਡਲ ਡਾਇਰੈਕਟਰੀ ਦੀ ਬਣਤਰ ਦੀ ਜਾਂਚ ਕਰੋ
ਮਾਡਲ ਬਿਲਡਰ ਸਾਰੀਆਂ ਕੰਪਾਇਲ ਕੀਤੀਆਂ ਫਾਇਲਾਂ ਨੂੰ ਸਿੱਧਾ ਤੁਹਾਡੇ ਦਿੱਤੇ ਗਏ ਆਉਟਪੁੱਟ ਡਾਇਰੈਕਟਰੀ ਵਿੱਚ ਰੱਖਦਾ ਹੈ। ਪੁਸ਼ਟੀ ਕਰੋ ਕਿ ਆਖਰੀ ਸਰਚਨਾ ਠੀਕ ਹੈ:

```bash
ls models/qwen3
```

ਡਾਇਰੈਕਟਰੀ ਵਿੱਚ ਹੇਠਾਂ ਦਿੱਤੀਆਂ ਫਾਇਲਾਂ ਹੋਣੀਆਂ ਚਾਹੀਦੀਆਂ ਹਨ:

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

> **ਨੋਟ:** ਕੁਝ ਹੋਰ ਕੰਪਾਈਲੇਸ਼ਨ ਟੂਲਜ਼ ਦੇ ਉਲਟ, ਮਾਡਲ ਬਿਲਡਰ ਨੇਸਟਡ ਸਬਡਾਇਰੈਕਟਰियाँ ਨਹੀਂ ਬਣਾਉਂਦਾ। ਸਾਰੀਆਂ ਫਾਇਲਾਂ ਸਿੱਧਾ ਆਉਟਪੁੱਟ ਫੋਲਡਰ ਵਿੱਚ ਹੀ ਹੁੰਦੀਆਂ ਹਨ, ਜੋ ਬਿਲਕੁਲ Foundry Local ਦੀ ਉਮੀਦ ਹੈ।

---

### ਅਭਿਆਸ 6: ਮਾਡਲ ਨੂੰ Foundry Local ਕੈਸ਼ ਵਿੱਚ ਸ਼ਾਮਲ ਕਰੋ

Foundry Local ਨੂੰ ਦੱਸੋ ਕਿ ਉਹ ਤੁਹਾਡੇ ਕੰਪਾਇਲ ਕੀਤੇ ਮਾਡਲ ਨੂੰ ਕਿੱਥੇ ਲੱਭੇ ਕਿ ਡਾਇਰੈਕਟਰੀ ਨੂੰ ਉਸਦੇ ਕੈਸ਼ ਵਿੱਚ ਸ਼ਾਮਲ ਕਰੋ:

```bash
foundry cache cd models/qwen3
```

ਪੁਸ਼ਟੀ ਕਰੋ ਕਿ ਮਾਡਲ ਕੈਸ਼ ਵਿੱਚ ਦਿੱਸਦਾ ਹੈ:

```bash
foundry cache ls
```

ਤੁਹਾਨੂੰ ਆਪਣੇ ਕਸਟਮ ਮਾਡਲ ਨੂੰ ਪਹਿਲਾਂ ਕੈਸ਼ ਕੀਤੇ ਮਾਡਲਾਂ ਦੇ ਨਾਲ ਸੂਚੀਬੱਧ ਦੇਖਣਾ ਚਾਹੀਦਾ ਹੈ (ਜਿਵੇਂ ਕੀ `phi-3.5-mini` ਜਾਂ `phi-4-mini`)।

---

### ਅਭਿਆਸ 7: CLI ਨਾਲ ਕਸਟਮ ਮਾਡਲ ਚਲਾਓ

ਆਪਣੇ ਨਵੇਂ ਕੰਪਾਇਲ ਕੀਤੇ ਮਾਡਲ (ਜੋ ਕਿ `qwen3-0.6b` `inference_model.json` ਵਿੱਚ `Name` ਫੀਲਡ ਤੋਂ ਆਉਂਦਾ ਹੈ) ਨਾਲ ਇਕ ਇੰਟਰਐਕਟਿਵ ਚੈਟ ਸੈਸ਼ਨ ਸ਼ੁਰੂ ਕਰੋ:

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` ਫਲੈਗ ਵਧੇਰੇ ਡਾਇਗਨੋਸਟਿਕ ਜਾਣਕਾਰੀ ਦਿਖਾਉਂਦਾ ਹੈ, ਜੋ ਪਹਿਲੀ ਵਾਰੀ ਕਸਟਮ ਮਾਡਲ ਦੀ ਜਾਂਚ ਕਰਦੇ ਸਮੇਂ ਮਦਦਗਾਰ ਹੁੰਦਾ ਹੈ। ਜੇ ਮਾਡਲ ਸਫਲਤਾਪੂਰਵਕ ਲੋਡ ਹੋ ਜਾਂਦਾ ਹੈ ਤਾਂ ਤੁਹਾਨੂੰ ਇੱਕ ਇੰਟਰਐਕਟਿਵ ਪ੍ਰਾਂਪਟ ਦਿਖੇਗਾ। ਕੁਝ ਸੁਨੇਹੇ ਟਾਈਪ ਕਰੋ:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

ਸੈਸ਼ਨ ਖਤਮ ਕਰਨ ਲਈ `exit` ਟਾਈਪ ਕਰੋ ਜਾਂ `Ctrl+C` ਦਬਾਓ।

> **ਟ੍ਰਬਲਸ਼ੂਟਿੰਗ:** ਜੇ ਮਾਡਲ ਲੋਡ ਕਰਨ ਵਿੱਚ ਨਾਕਾਮ ਰਹਿੰਦਾ ਹੈ ਤਾਂ ਇਹ ਚੀਜ਼ਾਂ ਚੈੱਕ ਕਰੋ:
> - `genai_config.json` ਫਾਇਲ ਮਾਡਲ ਬਿਲਡਰ ਵੱਲੋਂ ਬਣਾਈ ਗਈ ਹੈ।
> - `inference_model.json` ਫਾਇਲ ਮੌਜੂਦ ਹੈ ਅਤੇ ਵੈਧ JSON ਹੈ।
> - ONNX ਮਾਡਲ ਫਾਇਲਾਂ ਸਹੀ ਡਾਇਰੈਕਟਰੀ ਵਿੱਚ ਹਨ।
> - ਤੁਹਾਡੀ ਕੋਲ ਕਾਫ਼ੀ ਰੈਮ ਮੌਜੂਦ ਹੈ (Qwen3-0.6B int4 ਲਈ ਲਗਭਗ 1 GB ਲੋੜੀਂਦਾ ਹੈ)।
> - Qwen3 ਇੱਕ reasoning ਮਾਡਲ ਹੈ ਜੋ `<think>` ਟੈਗਜ਼ ਪੈਦਾ ਕਰਦਾ ਹੈ। ਜੇ ਤੁਹਾਨੂੰ ਜਵਾਬਾਂ ਵਿੱਚ `<think>...</think>` ਨਜ਼ਰ ਆਉਂਦਾ ਹੈ ਤਾਂ ਇਹ ਸਧਾਰਨ ਧਰਤੀ ਹੈ। `inference_model.json` ਵਿੱਚ ਪ੍ਰਾਂਪਟ ਟੈਮਪਲੇਟ ਨੂੰ ਸੋਚਣ ਵਾਲਾ ਆਉਟਪੁੱਟ ਦਬਾਅ ਕਰਨ ਲਈ ਸੈੱਟ ਕੀਤਾ ਜਾ ਸਕਦਾ ਹੈ।

---

### ਅਭਿਆਸ 8: REST API ਰਾਹੀਂ ਕਸਟਮ ਮਾਡਲ ਨੂੰ ਕਵੈਰੀ ਕਰੋ

ਜੇ ਤੁਸੀਂ ਅਭਿਆਸ 7 ਵਿੱਚ ਇੰਟਰਐਕਟਿਵ ਸੈਸ਼ਨ ਤੋਂ ਬਾਹਰ ਨਿਕਲ ਗਏ ਹੋ, ਤਾਂ ਮਾਡਲ ਹੁਣ ਲੋਡ ਨਹੀਂ ਹੋ ਸਕਦਾ। Foundry Local ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ ਅਤੇ ਪਹਿਲਾਂ ਮਾਡਲ ਲੋਡ ਕਰੋ:

```bash
foundry service start
foundry model load qwen3-0.6b
```

ਦਿਸਾਈਏ ਕਿ ਸੇਵਾ ਕਿਸ ਪੋਰਟ `ਤੇ ਚੱਲ ਰਹੀ ਹੈ:

```bash
foundry service status
```

ਫਿਰ ਇਕ ਬੇਨਤੀ ਭੇਜੋ (ਜੇ `5273` ਤੁਹਾਡੇ ਵਾਸਤੇ ਵੱਖਰਾ ਹੈ ਤਾਂ ਉਸ ਨੂੰ ਬਦਲੋ):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows ਨੋਟ:** ਉਪਰੋਕਤ `curl` ਕਮਾਂਡ ਬੈਸ਼ ਸਿੰਟੈਕਸ ਵਰਤਦੀ ਹੈ। Windows ਵਿੱਚ ਬਜਾਏ ਇਸਦੇ PowerShell `Invoke-RestMethod` ਕਮਾਂਡ ਵਰਤੋਂ।

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

### ਅਭਿਆਸ 9: OpenAI SDK ਨਾਲ ਕਸਟਮ ਮਾਡਲ ਵਰਤੋ

ਤੁਸੀਂ ਆਪਣੇ ਕਸਟਮ ਮਾਡਲ ਨਾਲ ਉਹੀ OpenAI SDK ਕੋਡ ਬਿਲਕੁਲ ਵਰਤ ਸਕਦੇ ਹੋ ਜੋ ਤੁਸੀਂ ਪਹਿਲਾਂ ਬਿਲਟ-ਇਨ ਮਾਡਲਾਂ ਲਈ ਵਰਤਿਆ ਸੀ (ਦੇਖੋ [ਭਾਗ 3](part3-sdk-and-apis.md))। ਕੇਵਲ ਫਰਕ ਮਾਡਲ ਦਾ ਨਾਮ ਹੈ।

<details>
<summary><b>ਪਾਇਥਨ</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # ਫਾਊਂਡਰੀ ਲੋਕਲ API ਕੁੰਜੀਆਂ ਦੀ ਪੁਸ਼ਟੀ ਨਹੀਂ ਕਰਦਾ
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
<summary><b>ਜਾਵਾਸਕ੍ਰਿਪਟ</b></summary>

```javascript
import OpenAI from "openai";

const client = new OpenAI({
  baseURL: "http://localhost:5273/v1",
  apiKey: "foundry-local", // ਫਾਊਂਡਰੀ ਲੋਕਲ API ਕੁੰਜੀਆਂ ਦੀ ਪੁਸ਼ਟੀ ਨਹੀਂ ਕਰਦਾ
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

> **ਮੁੱਖ ਗੱਲ:** Foundry Local ਇੱਕ OpenAI-ਸੰਗਤ APIs ਦਿੰਦਾ ਹੈ, ਇਸ ਲਈ ਜੋ ਕੋਈ ਵੀ ਕੋਡ ਬਿਲਟ-ਇਨ ਮਾਡਲਾਂ ਨਾਲ ਕਾਮ ਕਰਦਾ ਹੈ, ਉਹ ਤੁਹਾਡੇ ਕਸਟਮ ਮਾਡਲਾਂ ਨਾਲ ਵੀ ਕਾਮ ਕਰੇਗਾ। ਤੁਹਾਨੂੰ ਸਿਰਫ਼ `model` ਪੈਰਾਮੀਟਰ ਬਦਲਣਾ ਹੈ।

---

### ਅਭਿਆਸ 10: Foundry Local SDK ਨਾਲ ਕਸਟਮ ਮਾਡਲ ਦੀ ਜਾਂਚ ਕਰੋ

ਪੂਰਵ ਲੈਬਾਂ ਵਿੱਚ ਤੁਸੀਂ Foundry Local SDK ਨੂੰ ਸੇਵਾ ਸ਼ੁਰੂ ਕਰਨ, ਐਂਡਪਾਇੰਟ ਖੋਜਣ ਅਤੇ ਮਾਡਲਾਂ ਨੂੰ ਆਪਣੇ ਆਪ ਮੈਨੇਜ ਕਰਨ ਲਈ ਵਰਤਿਆ। ਤੁਸੀਂ ਕੇਵਲ ਕਸਟਮ ਕੰਪਾਇਲ ਕੀਤੇ ਮਾਡਲ ਨਾਲ ਜਿਵੇਂ ਕਰਦੇ ਹੋ ਉਵੇਂ ਕਰ ਸਕਦੇ ਹੋ। SDK ਸੇਵਾ ਸ਼ੁਰੂਆਤ ਅਤੇ ਐਂਡਪਾਇੰਟ ਖੋਜ ਨੂੰ ਮੈਨੇਜ ਕਰਦਾ ਹੈ, ਇਸ ਲਈ ਤੁਹਾਡੇ ਕੋਡ ਨੂੰ `localhost:5273` ਹਾਰਡਕੋਡ ਕਰਨ ਦੀ ਲੋੜ ਨਹੀਂ।

> **ਨੋਟ:** ਕੇੜਾ SDK ਇੰਸਟਾਲ ਹੈ ਇਹ ਯਕੀਨੀ ਬਣਾਓ:
> - **ਪਾਇਥਨ:** `pip install foundry-local openai`
> - **ਜਾਵਾਸਕ੍ਰਿਪਟ:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` ਅਤੇ `OpenAI` NuGet ਪੈਕੇਜ ਸ਼ਾਮਲ ਕਰੋ
>
> ਹਰ ਸਕ੍ਰਿਪਟ ਫਾਇਲ ਨੂੰ **ਰੇਪੋਜ਼ਿਟਰੀ ਰੂਟ ਵਿੱਚ ਸੰਭਾਲੋ** (ਜਿੱਥੇ `models/` ਫੋਲਡਰ ਹੈ).

<details>
<summary><b>ਪਾਇਥਨ</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# ਕਦਮ 1: ਫਾਊਂਡਰੀ ਲੋਕਲ ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ ਅਤੇ ਕਸਟਮ ਮਾਡਲ ਲੋਡ ਕਰੋ
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# ਕਦਮ 2: ਕਸਟਮ ਮਾਡਲ ਲਈ ਕੈਸ਼ ਚੈੱਕ ਕਰੋ
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# ਕਦਮ 3: ਮਾਡਲ ਨੂੰ ਮੈਮੋਰੀ ਵਿੱਚ ਲੋਡ ਕਰੋ
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# ਕਦਮ 4: SDK-ਖੋਜਿਆ ਗਿਆ ਐਂਡਪੌਇੰਟ ਵਰਤ ਕੇ OpenAI ਕਲਾਇੰਟ ਬਣਾਓ
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# ਕਦਮ 5: ਸਟ੍ਰੀਮਿੰਗ ਚੈਟ ਪੂਰਾ ਕਰਨ ਦੀ ਬੇਨਤੀ ਭੇਜੋ
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

ਇਸਨੂੰ ਚਲਾਓ:

```bash
python foundry_sdk_custom_model.py
```

</details>

<details>
<summary><b>ਜਾਵਾਸਕ੍ਰਿਪਟ</b></summary>

```javascript
import { OpenAI } from "openai";
import { FoundryLocalManager } from "foundry-local-sdk";

const modelAlias = "qwen3-0.6b";

// ਕਦਮ 1: ਫਾਊਂਡਰੀ ਲੋਕਲ ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ਕਦਮ 2: ਕੈਟਾਲੌਗ ਤੋਂ ਕਸਟਮ ਮੌਡਲ ਪ੍ਰਾਪਤ ਕਰੋ
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// ਕਦਮ 3: ਮੌਡਲ ਨੂੰ ਮੈਮੋਰੀ ਵਿੱਚ ਲੋਡ ਕਰੋ
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// ਕਦਮ 4: SDK-ਖੋਜੇ ਗਏ ਐਂਡਪੋਇੰਟ ਦੀ ਵਰਤੋਂ ਕਰਕੇ OpenAI ਕਲੇਂਟ ਬਣਾਓ
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// ਕਦਮ 5: ਸਟ੍ਰੀਮਿੰਗ ਚੈਟ ਪੂਰੇ ਕਰਨ ਦੀ ਬੇਨਤੀ ਭੇਜੋ
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

ਇਸਨੂੰ ਚਲਾਓ:

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

> **ਮੁੱਖ ਗੱਲ:** Foundry Local SDK ਐਂਡਪਾਇੰਟ ਨੂੰ ਡਾਇਨੈਮਿਕ ਢੰਗ ਨਾਲ ਲੱਭਦਾ ਹੈ, ਇਸ ਲਈ ਤੁਸੀਂ ਕਿਸੇ ਵੀ ਪੋਰਟ ਨੰਬਰ ਨੂੰ ਹਾਰਡਕੋਡ ਨਹੀਂ ਕਰਦੇ। ਇਹ ਪ੍ਰੋਡਕਸ਼ਨ ਐਪਲੀਕੇਸ਼ਨਾਂ ਲਈ ਸਿਫਾਰਸ਼ੀ ਤਰੀਕਾ ਹੈ। ਤੁਹਾਡਾ ਕਸਟਮ ਕੰਪਾਇਲ ਕੀਤਾ ਮਾਡਲ SDK ਰਾਹੀਂ ਬਿਲਟ-ਇਨ ਕੈਟਲਾਗ ਮਾਡਲਾਂ ਨਾਲ ਇਕੋ ਜਿਹਾ ਕੰਮ ਕਰਦਾ ਹੈ।

---

## ਕੰਪਾਇਲ ਕਰਨ ਲਈ ਮਾਡਲ ਦੀ ਚੋਣ

Qwen3-0.6B ਇਸ ਲੈਬ ਵਿੱਚ ਉਦਾਹਰਨ ਵਜੋਂ ਵਰਤਿਆ ਗਿਆ ਹੈ ਕਿਉਂਕਿ ਇਹ ਛੋਟਾ, ਤੇਜ਼ ਅਤੇ Apache 2.0 ਲਾਇਸੈਂਸ ਵਿੱਚ ਉਪਲਬਧ ਹੈ। ਪਰ ਤੁਸੀਂ ਹੋਰ ਕਈ ਮਾਡਲ ਵੀ ਕੰਪਾਇਲ ਕਰ ਸਕਦੇ ਹੋ। ਕੁਝ ਸੁਝਾਅ ਇੱਥੇ ਹਨ:

| ਮਾਡਲ | Hugging Face ID | ਪੈਰਾਮੀਟਰ | ਲਾਇਸੈਂਸ | ਨੋਟਸ |
|-------|-----------------|------------|---------|-------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | Apache 2.0 | ਬਹੁਤ ਛੋਟਾ, ਤੇਜ਼ ਕੰਪਾਈਲ, ਟੈਸਟ ਲਈ ਚੰਗਾ |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | Apache 2.0 | ਵਧੀਆ ਗੁਣਵੱਤਾ, ਫਿਰ ਵੀ ਤੇਜ਼ ਕੰਪਾਈਲ ਹੁੰਦਾ ਹੈ |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | Apache 2.0 | ਮਜ਼ਬੂਤ ਗੁਣਵੱਤਾ, ਜ਼ਿਆਦਾ ਰੈਮ ਦੀ ਲੋੜ |
| Llama 3.2 1B Instruct | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face ‘ਤੇ ਲਾਇਸੈਂਸ ਸਵੀਕਾਰ ਕਰਨਾ ਲਾਜ਼ਮੀ |
| Mistral 7B Instruct | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | Apache 2.0 | ਉੱਚ ਗੁਣਵੱਤਾ, ਵੱਡਾ ਡਾਊਨਲੋਡ ਤੇ ਲੰਬਾ ਕੰਪਾਇਲ ਸਮਾਂ |
| Phi-3 Mini | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Foundry Local ਕੈਟਲਾਗ ਵਿੱਚ ਪਹਿਲਾਂ ਹੀ ਹੈ (ਤੁਲਨਾ ਲਈ ਲਾਭਦਾਇਕ) |

> **ਲਾਇਸੈਂਸ ਯਾਦ ਦਿਵਾਈ:** ਮਾਡਲ ਵਰਤੋਂ ਤੋਂ ਪਹਿਲਾਂ ਹਮੇਸ਼ਾਂ Hugging Face ‘ਤੇ ਮਾਡਲ ਦੀ ਲਾਇਸੈਂਸ ਚੈੱਕ ਕਰੋ। ਕੁਝ ਮਾਡਲ (ਜਿਵੇਂ Llama) ਲਈ ਤੁਹਾਨੂੰ ਲਾਇਸੈਂਸ ਸਹਿਮਤੀ ਦੇਣੀ ਪੈਂਦੀ ਹੈ ਅਤੇ `huggingface-cli login` ਕਰਕੇ ਪ੍ਰਮਾਣਿਕਤਾ ਲੈਣੀ ਪੈਂਦੀ ਹੈ।

---

## ਧਾਰਨਾਵਾਂ: ਕਦੋਂ ਕਸਟਮ ਮਾਡਲ ਵਰਤਣੇ

| ਸੰਦਰਭ | ਕਿਉਂ ਆਪਣਾ ਕੰਪਾਇਲ ਕਰਨਾ? |
|----------|----------------------------|
| **ਜੋ ਮਾਡਲ ਤੁਹਾਨੂੰ ਚਾਹੀਦਾ ਹੈ ਕੈਟਲਾਗ ਵਿੱਚ ਨਹੀਂ ਹੈ** | Foundry Local ਕੈਟਲਾਗ ਕੁਸ਼ਲਤਾਪੂਰਕ ਬਣਾਇਆ ਗਿਆ ਹੈ। ਜੇ ਮਾਡਲ ਨਹੀਂ ਮਿਲਦਾ ਤਾਂ ਖੁਦ ਕੰਪਾਇਲ ਕਰੋ। |
| **ਫਾਈਨ-ਟਿਊਨਡ ਮਾਡਲ** | ਜੇ ਤੁਸੀਂ ਕਿਸੇ ਖਾਸ ਡੋਮੇਨ ਦਾ ਡੇਟਾ ਵਰਤਕੇ ਮਾਡਲ ਫਾਈਨ-ਟਿਊਨ ਕੀਤਾ ਹੈ ਤਾਂ ਆਪਣੀਆਂ ਵੈਟਸ ਕੰਪਾਇਲ ਕਰੋ। |
| **ਖਾਸ ਕਵਾਂਤੀਕਰਨ ਲੋੜਾਂ** | ਤੁਸੀਂ ਕੋਈ ਉਹ ਧਰਾਂਤੀ ਜਾਂ ਪ੍ਰਿਸੀਜ਼ਨ ਰਣਨੀਤੀ ਚਾਹੁੰਦੇ ਹੋ ਜੋ ਕੈਟਲਾਗ ਦੇ ਡਿਫਾਲਟ ਤੋਂ ਵੱਖ ਹੈ। |
| **ਨਵੇਂ ਮਾਡਲ ਰਿਲੀਜ਼** | ਜਦ ਨਵਾ ਮਾਡਲ Hugging Face ‘ਤੇ ਆਉਂਦਾ ਹੈ, ਉਹ ਸ਼ਾਇਦ_FOUND_LOCAL ਕੈਟਲਾਗ ਵਿੱਚ ਨਹੀਂ ਹੁੰਦਾ। ਆਪਣਾ ਕੰਪਾਇਲ ਕਰਨ ਨਾਲ ਤੁਰੰਤ ਮਿਲਦਾ ਹੈ। |
| **ਰਿਸਰਚ ਅਤੇ ਪ੍ਰਯੋਗ** | ਉੱਥੇ ਵੱਖ-ਵੱਖ ਮਾਡਲ ਬਣਾਵਟਾਂ, ਆਕਾਰਾਂ ਜਾਂ ਕਨਫਿਗਰੇਸ਼ਨਾਂ ਨੂੰ ਸਥਾਨਕ ਤੌਰ ‘ਤੇ ਟੈਸਟ ਕਰਨਾ। |

---

## ਸੰਖੇਪ

ਇਸ ਲੈਬ ਵਿੱਚ ਤੁਸੀਂ ਸਿੱਖਿਆ:

| ਕਦਮ | ਤੁਸੀਂ ਕੀ ਕੀਤਾ |
|------|---------------|
| 1 | ONNX Runtime GenAI ਮਾਡਲ ਬਿਲਡਰ ਇੰਸਟਾਲ ਕੀਤਾ |
| 2 | Hugging Face ਤੋਂ `Qwen/Qwen3-0.6B` ਕੰਪਾਈਲ ਕਰਕੇ ਇਕ ਉਤਕ੍ਰਿਸ਼ਟ ONNX ਮਾਡਲ ਬਣਾਇਆ |
| 3 | ਇੱਕ `inference_model.json` ਚੈਟ-ਟੈਂਪਲੇਟ ਕੰਫਿਗਰੇਸ਼ਨ ਫਾਇਲ ਬਣਾਈ |
| 4 | ਕੰਪਾਇਲ ਕੀਤਾ ਮਾਡਲ Foundry Local ਕੈਸ਼ ਵਿੱਚ ਸ਼ਾਮਲ ਕੀਤਾ |
| 5 | CLI ਰਾਹੀਂ ਇੰਟਰਐਕਟਿਵ ਚੈਟ ਚਲਾਈ |
| 6 | OpenAI-ਸੰਘਤ REST API ਰਾਹੀਂ ਮਾਡਲ ਨੂੰ ਕਵੈਰੀ ਕੀਤਾ |
| 7 | Python, JavaScript ਅਤੇ C# ਨਾਲ OpenAI SDK ਵਰਤ ਕੇ ਕੁਨੈਕਟ ਕੀਤਾ |
| 8 | Foundry Local SDK ਨਾਲ ਕਸਟਮ ਮਾਡਲ ਨੂੰ ਆਖਰ-ਅੰਤ ਤੱਕ ਟੈਸਟ ਕੀਤਾ |

ਮੁੱਖ ਤੱਥ ਇਹ ਹੈ ਕਿ **ਕੋਈ ਵੀ ਟ੍ਰਾਂਸਫਾਰਮਰ ਆਧਾਰਿਤ ਮਾਡਲ Foundry Local ਵਿੱਚ ਚੱਲ ਸਕਦਾ ਹੈ** ਜਦੋਂ ਉਹ ONNX ਫਾਰਮੈਟ ਵਿੱਚ ਕੰਪਾਈਲ ਹੋ ਜਾਵੇ। OpenAI-ਸੰਗਤ API ਦਾ ਮਤਲਬ ਹੈ ਕਿ ਤੁਹਾਡਾ ਮੌਜੂਦਾ ਐਪਲੀਕੇਸ਼ਨ ਕੋਡ ਬਿਨਾਂ ਬਦਲਾਅ ਦੇ ਕੰਮ ਕਰਦਾ ਹੈ; ਸਿਰਫ਼ ਮਾਡਲ ਦਾ ਨਾਮ ਬਦਲਣਾ ਹੁੰਦਾ ਹੈ।

---

## ਮੁੱਖ ਗੱਲਾਂ

| ਧਾਰਣਾ | ਵੇਰਵਾ |
|---------|---------|
| ONNX Runtime GenAI ਮਾਡਲ ਬਿਲਡਰ | Hugging Face ਮਾਡਲਾਂ ਨੂੰ ONNX ਫਾਰਮੈਟ ਵਿੱਚ ਇਕ ਕਮਾਂਡ ਨਾਲ ਕਵਾਂਤੀਕਰਨ ਸਮੇਤ परिवर्तित ਕਰਦਾ ਹੈ |
| ONNX ਫਾਰਮੈਟ | Foundry Local ਨੂੰ ONNX ਮਾਡਲਾਂ ਦੀ ਲੋੜ ਹੈ ਜਿਨ੍ਹਾਂ ਦੇ ਨਾਲ ONNX Runtime GenAI ਕੰਫਿਗਰੇਸ਼ਨ ਹੁੰਦੀ ਹੈ |
| ਚੈਟ ਟੈਮਪਲੇਟ | `inference_model.json` ਫਾਇਲ Foundry Local ਨੂੰ ਦੱਸਦੀ ਹੈ ਕਿ ਕਿਸ ਤਰ੍ਹਾਂ ਮਾਡਲ ਲਈ ਪ੍ਰਾਂਪਟਸ ਬਣਾਉਣੇ ਹਨ |
| ਹਾਰਡਵੇਅਰ ਟਾਰਗਟ | CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), ਜਾਂ WebGPU ਲਈ ਕੰਪਾਇਲ ਕਰੋ ਹਾਰਡਵੇਅਰ ਦੇ ਅਨੁਸਾਰ |
| ਕਵਾਂਤੀਕਰਨ | ਘੱਟ ਪ੍ਰਿਸੀਜ਼ਨ (int4) ਸਾਈਜ਼ ਘਟਾਉਂਦੀ ਅਤੇ ਗਤੀ ਵਧਾਉਂਦੀ ਹੈ ਪਰ ਕੁਝ ਗਲਤੀ ਦਾ ਖ਼ਤਰਾ ਹੁੰਦਾ ਹੈ; fp16 GPUs 'ਤੇ ਉੱਚ ਗੁਣਵੱਤਾ ਬਣਾਈ ਰੱਖਦੀ ਹੈ |
| API ਸੰਗਤਤਾ | ਕাস্টਮ ਮਾਡਲ ਉਹੀ OpenAI-ਸੰਗਤ API ਵਰਤਦੇ ਹਨ ਜੋ ਬਿਲਟ-ਇਨ ਮਾਡਲਾਂ ਲਈ ਹੈ |
| Foundry Local SDK | SDK ਸੇਵਾ ਸ਼ੁਰੂ ਕਰਨ, ਐਂਡਪਾਇੰਟ ਖੋਜਣ ਅਤੇ ਮਾਡਲ ਲੋਡ ਕਰਨ ਦਾ ਕੰਮ ਆਪਣੇ ਆਪ ਕਰਦਾ ਹੈ, ਦੋਹਾਂ ਕੈਟਲਾਗ ਅਤੇ ਕਸਟਮ ਮਾਡਲਾਂ ਲਈ |

---

## ਹੋਰ ਪੜ੍ਹਾਈ

| ਸਰੋਤ | ਲਿੰਕ |
|----------|-------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local ਕਸਟਮ ਮਾਡਲ ਗਾਈਡ | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 ਮਾਡਲ ਪਰਿਵਾਰ | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive ਦਸਤਾਵੇਜ਼ | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## ਅਗਲੇ ਕਦਮ

ਅਗਲੇ ਹਿੱਸੇ [ਭਾਗ 11: ਟੂਲ ਕਾਲਿੰਗ ਵਿਥ ਲੋਕਲ ਮਾਡਲਜ਼](part11-tool-calling.md) 'ਤੇ ਜਾਰੀ ਰੱਖੋ ਤਾਂ ਜੋ ਤੁਸੀਂ ਸਿਖ ਸਕੋ ਕਿ ਆਪਣੀ ਸਥਾਨਕ ਮਾਡਲਾਂ ਨੂੰ ਬਾਹਰੀ ਫੰਕਸ਼ਨਾਂ ਨੂੰ ਕਾਲ਼ ਕਰਨ ਲਈ ਕਿਵੇਂ ਯੋਗ ਬਨਾਉਣਾ ਹੈ।

[← ਭਾਗ 9: Whisper ਵੋਇਸ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ](part9-whisper-voice-transcription.md) | [ਭਾਗ 11: ਟੂਲ ਕਾਲਿੰਗ →](part11-tool-calling.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ਅਸਫਲਤਾ ਸੂਚਨਾ**:  
ਇਹ ਦਸਤਾਵੇਜ਼ ਏਆਈ ਅਨੁਵਾਦ ਸੇਵਾ [Co-op Translator](https://github.com/Azure/co-op-translator) ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਅਨੁਵਾਦਿਤ ਕੀਤਾ ਗਿਆ ਹੈ। ਜਦੋਂ ਕਿ ਅਸੀਂ ਸਹੀਤਾ ਲਈ ਕੋਸ਼ਿਸ਼ ਕਰਦੇ ਹਾਂ, ਕਿਰਪਾ ਕਰਕੇ ਧਿਆਨ ਰੱਖੋ ਕਿ ਸਵੈਚਲਿਤ ਅਨੁਵਾਦਾਂ ਵਿੱਚ ਗਲਤੀਆਂ ਜਾਂ ਅਣਪਛਾਤੀਆਂ ਹੋ ਸਕਦੀਆਂ ਹਨ। ਮੂਲ ਦਸਤਾਵੇਜ਼ ਆਪਣੇ ਮੂਲ ਭਾਸ਼ਾ ਵਿੱਚ ਪ੍ਰਮਾਣਿਕ ਸਰੋਤ ਮੰਨਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ। ਜਰੂਰੀ ਜਾਣਕਾਰੀ ਲਈ, ਪੇਸ਼ੇਵਰ ਮਨੁੱਖੀ ਅਨੁਵਾਦ ਦੀ ਸਿਫਾਰਿਸ਼ ਕੀਤੀ ਜਾਂਦੀ ਹੈ। ਇਸ ਅਨੁਵਾਦ ਦੀ ਵਰਤੋਂ ਨਾਲ ਪੈਦਾ ਹੋਣ ਵਾਲੀ ਕਿਸੇ ਵੀ ਗਲਤਫਹਮੀ ਜਾਂ ਗਲਤ ਵਿਵਰਣ ਲਈ ਅਸੀਂ ਜ਼ਿੰਮੇਵਾਰ ਨਹੀਂ ਹਾਂ।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->