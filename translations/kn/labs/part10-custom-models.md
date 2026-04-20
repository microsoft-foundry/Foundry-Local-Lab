![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ಭಾಗ 10: Foundry Local ನೊಂದಿಗೆ ಕಸ್ಟಮ್ ಅಥವಾ Hugging Face ಮಾದರಿಗಳನ್ನು ಬಳಸುವುದು

> **ಗೋಲು:** Foundry Local ಗೆ ಅಗತ್ಯವಿರುವ ಆಪ್ಟಿಮೈಜ್ಡ್ ONNX ಫಾರ್ಮ್ಯಾಟ್‌ಗೆ Hugging Face ಮಾದರಿಯನ್ನು ಸಂಯೋಜಿಸಿ, ಅದನ್ನು ಚಾಟ್ ಟೆಂಪ್ಲೇಟ್‌ನೊಂದಿಗೆ ಕಾನ್‌ಫಿಗರ್ ಮಾಡಿ, ಸ್ಥಳೀಯ ಕ್ಯಾಶ್‌ಗೆ ಸೇರಿಸಿ, ಮತ್ತು CLI, REST API, ಮತ್ತು OpenAI SDK ಅನ್ನು ಬಳಸಿಕೊಂಡು ಅದರ ವಿರುದ್ಧ ಇನ್ಫರೆನ್ಸ್ ಅನ್ನು ನಡೆಸಿ.

## ಅವಲೋಕನ

Foundry Local ಪೂರ್ವ-ಸಂಯೋಜಿತ ಮಾದರಿಗಳ ಸಂಕಲನದೊಂದಿಗೆ ಬರುವುದಾದರೆ, ನೀವು ಆ ಪಟ್ಟಿಯೊಳಗೆ ಮಾತ್ರ ಸೀಮಿತರಾಗುವುದಿಲ್ಲ. [Hugging Face](https://huggingface.co/) ನಲ್ಲಿ ಲಭ್ಯವಿರುವ ಯಾವುದೇ ಟ್ರಾನ್ಸ್‌ಫಾರ್ಮರ್ ಆಧಾರಿತ ಭಾಷಾ ಮಾದರಿ (ಅಥವಾ PyTorch / Safetensors ಫಾರ್ಮ್ಯಾಟ್‌ನಲ್ಲಿ ಸ್ಥಳೀಯವಾಗಿ ಸಂಗ್ರಹಿತ) ಆಪ್ಟಿಮೈಜ್ಡ್ ONNX ಮಾದರಿಯಾಗಿ ಸಂಯೋಜಿಸಿ Foundry Local ಮುಖಾಂತರ ಸೇವೆ ಮಾಡಬಹುದು.

ಸಂಯೋಜನಾ ಪೈಪ್ಲೈನ್ ONNX Runtime GenAI Model Builder ಅನ್ನು ಬಳಸುತ್ತದೆ, ಇದು `onnxruntime-genai` ಪ್ಯಾಕೇಜ್‌ನೊಂದಿಗೆ ಒಳಗೊಂಡಿರುವ ಕಮಾಂಡ್ ಲೈನ್ ಸಲಕರಣೆ. ಮಾದರಿ બિલ್ಡರ್ ಭಾರವಾದ ಕೆಲಸಗಳನ್ನೆಲ್ಲ ನಿರ್ವಹಿಸುತ್ತದೆ: ಮೂಲ ತೂಕಗಳನ್ನು ಡೌನ್‌ಲೋಡ್ ಮಾಡುವುದು, ಅವುಗಳನ್ನ ONNX ಫಾರ್ಮ್ಯಾಟ್‌ಗೆ ಪರಿವರ್ತಿಸುವುದು, ಕ್ವಾಂಟೈಸೇಶನ್ (int4, fp16, bf16) ಅನ್ವಯಿಸುವುದು, ಮತ್ತು Foundry Local ನಿರೀಕ್ಷಿಸುವ ಚಾಟ್ ಟೆಂಪ್ಲೇಟ್ ಮತ್ತು ಟೋಕನೈಜರ್ ಸೇರಿದಂತೆ ಕಾನ್ಫಿಗರೇಷನ್ ಫೈಲ್‌ಗಳನ್ನು ನೀಡುವುದು.

ಈ ಪ್ರಯೋಗಾಲಯದಲ್ಲಿ ನೀವು Hugging Face ನಿಂದ **Qwen/Qwen3-0.6B** ಅನ್ನು ಸಂಯೋಜಿಸಿ, ಅದನ್ನು Foundry Local ನಲ್ಲಿ ನೊಂದಾಯಿಸಿ, ಮತ್ತು ಸಂಪೂರ್ಣವಾಗಿ ನಿಮ್ಮ ಸಾಧನದ ಮೇಲೆ ಚಾಟ್ ಮಾಡುತ್ತೀರಿ.

---

## ಕಲಿಕೆಯ ಉದ್ದೇಶಗಳು

ಈ ಪ್ರಯೋಗಾಲಯದ ಕೊನೆಯಲ್ಲಿ ನೀವು ಸಾಧ್ಯವಾಗುವುದು:

- ಕಸ್ಟಮ್ ಮಾದರಿ ಸಂಯೋಜನೆಯು ಏಕೆ ಉಪಯುಕ್ತ ಮತ್ತು ನೀವು ಯಾವಾಗ ಅದನ್ನು ಬೇಕಾಗಬಹುದು ಎಂದು ವಿವರಿಸುವುದು
- ONNX Runtime GenAI ಮಾದರಿ બિલ್ಡರ್ ಅನ್ನು ಸ್ಥಾಪಿಸುವುದು
- ಒಂದು ಕಮಾಂಡ್ ಮೂಲಕ Hugging Face ಮಾದರಿಯನ್ನು ಆಪ್ಟಿಮೈಜ್ಡ್ ONNX ಫಾರ್ಮ್ಯಾಟ್‌ಗೆ ಸಂಯೋಜಿಸುವುದು
- ಮುಖ್ಯ ಸಂಯೋಜನಾ ನಿಯಮಕಗಳನ್ನು (ಕಾರ್ಯನಿರ್ವಹಣಾ ಪೂರೈಕೆದಾರ, ಪ್ರಿಸಿಷನ್) ಅರ್ಥಮಾಡಿಕೊಳ್ಳುವುದು
- `inference_model.json` ಚಾಟ್ ಟೆಂಪ್ಲೇಟ್ ಕಾನ್ಫಿಗರೇಷನ್ ಫೈಲ್ ಸೃಷ್ಟಿಸುವುದು
- ಸಂಯೋಜಿತ ಮಾದರಿಯನ್ನು Foundry Local ಕ್ಯಾಶ್‌ಗೆ ಸೇರಿಸುವುದು
- CLI, REST API, ಮತ್ತು OpenAI SDK ಅನ್ನು ಬಳಸಿ ಕಸ್ಟಮ್ ಮಾದರಿಯ ವಿರುದ್ಧ ಇನ್ಫರೆನ್ಸ್ ನಡೆಸುವುದು

---

## ಪೂರ್ವಾಪೇಕ್ಷಿತಗಳು

| ಅಗತ್ಯತೆ | ವಿವರಗಳು |
|-------------|---------|
| **Foundry Local CLI** | ನಿಮ್ಮ `PATH` ನಲ್ಲಿ ಸ್ಥಾಪಿತವಾಗಿದ್ದು ([ಭಾಗ 1](part1-getting-started.md)) |
| **Python 3.10+** | ONNX Runtime GenAI ಮಾದರಿ билдರ್‌ಗೆ ಅಗತ್ಯವಿದೆ |
| **pip** | Python ಪ್ಯಾಕೇಜ್ ನಿರ್ವಹಕ |
| **ಡಿಸ್ಕ್ ಸ್ಪೇಸ್** | ಮೂಲ ಮತ್ತು ಸಂಯೋಜಿತ ಮಾದರಿ ಫೈಲ್ ಗಳಿಗೆ ಕನಿಷ್ಠ 5 GB ಉಚಿತ ಸ್ಥಳ |
| **Hugging Face ಖಾತೆ** | ಕೆಲವು ಮಾದರಿಗಳಿಗೆ ಡೌನ್‌ಲೋಡ್ ಮಾಡುವುದು ಮುಂಚೆ ಪರವಾನಗಿ ಅಂಗೀಕಾರ ಅಗತ್ಯವಿರಬಹುದು. Qwen3-0.6B ಅಪ್ಪಾಚಿ 2.0 ಪರವಾನಗಿ ಹೊಂದಿದ್ದು ಉಚಿತವಾಗಿ ಲಭ್ಯವಾಗಿದೆ. |

---

## ಪರಿಸರ ಸಂಯೋಜನೆ

ಮಾದರಿ ಸಂಯೋಜನೆಗೆ(PyTorch, ONNX Runtime GenAI, Transformers) ಕೆಲವು ದೊಡ್ಡ Python ಪ್ಯಾಕೇಜುಗಳು ಬೇಕಾಗುತ್ತವೆ. ನಿಮ್ಮ ಸಿಸ್ಟಮ್ Python ಅಥವಾ ಇತರ ಪ್ರಾಜೆಕ್ಟ್‌ಗಳಿಗೆ ವ್ಯತ್ಯಯ ಉಂಟಾಗದಂತೆ ವಿಭಿನ್ನತನದ ವರ್ಚುಯಲ್ ಪರಿಸರವನ್ನು ರಚಿಸಿ.

```bash
# ಸಂಗ್ರಹ ರುಟ್‌ನಿಂದ
python -m venv .venv
```

ಪರಿಸರವನ್ನು ಸಕ್ರಿಯಗೊಳಿಸಿ:

**Windows (PowerShell):**
```powershell
.venv\Scripts\Activate.ps1
```

**macOS / Linux:**
```bash
source .venv/bin/activate
```

ಪಿಪ್ ನವೀಕರಿಸಿ, ಅವಲಂಬನೆ ಪರಿಹಾರ ಸಮಸ್ಯೆಗಳನ್ನು ತಡೆಗಟ್ಟಲು:

```bash
python -m pip install --upgrade pip
```

> **ಒತ್ತಾಯ:** ಹಳೆಯ ಪ್ರಯೋಗಾಲಯದಿಂದ `.venv` ಇದ್ದರೆ, ಅದನ್ನು ಮರುಬಳಕೆ ಮಾಡಬಹುದು. ಕೇವಲ ಮುಂದುವರಿಯುವುದಕ್ಕೆ ಮುನ್ನ ಅದು ಸಕ್ರಿಯವಾಗಿದೆ ಎಂಬುದನ್ನು ಖಚಿತಪಡಿಸಿಕೊಳ್ಳಿ.

---

## ಕಲ್ಪನೆ: ಸಂಯೋಜನಾ ಪೈಪ್ಲೈನ್

Foundry Local ಮಾದರಿಗಳನ್ನು ONNX ಫಾರ್ಮ್ಯಾಟ್‌ನೊಂದಿಗೆ ONNX Runtime GenAI ಕಾನ್ಫಿಗರೇಷನ್ ನಲ್ಲಿ ಬೇಕಾಗುತ್ತದೆ. Hugging Face ನಲ್ಲಿ ಬಹುತೇಕ ಓಪನ್ ಸೋರ್ಸ್ ಮಾದರಿಗಳು PyTorch ಅಥವಾ Safetensors ತೂಕಗಳನ್ನಾಗಿ ವಿತರಿಸಲಾಗುತ್ತವೆ, ಆದ್ದರಿಂದ ಪರಿವರ್ತನೆಯ ಹಂತ ಅಗತ್ಯವಿದೆ.

![Custom model compilation pipeline](../../../images/custom-model-pipeline.svg)

### ಮಾದರಿ બિલ್ಡರ್ ಏನು ಮಾಡುತ್ತದೆ?

1. Hugging Face ನಿಂದ ಮೂಲ ಮಾದರಿಯನ್ನು ಡೌನ್‌ಲೋಡ್ ಮಾಡುತ್ತದೆ (ಅಥವಾ ಸ್ಥಳೀಯ ಪಥದಿಂದ ಓದುವದು).
2. PyTorch / Safetensors ತೂಕಗಳನ್ನು ONNX ಫಾರ್ಮ್ಯಾಟ್‌ಗೆ ಪರಿವರ್ತಿಸುತ್ತದೆ.
3. ಸ್ಮೃತಿಯನ್ನು ಕಡಿಮೆ ಮಾಡುವುದು ಮತ್ತು ಸ್ರಾವ ವೇಗವನ್ನು ಸುಧಾರಿಸುವುದುಕ್ಕಾಗಿ ಕ್ವಾಂಟೈಸೇಷನ್ (ಉದಾಹರಣೆ: int4) ಅನ್ವಯಿಸುತ್ತದೆ.
4. Foundry Local ಗೆ ಲೋಡ್ ಮತ್ತು ಸೇವೆ ಮಾಡಲು ಅಗತ್ಯವಿರುವ ONNX Runtime GenAI ಕಾನ್ಫಿಗರೇಷನ್ (`genai_config.json`), ಚಾಟ್ ಟೆಂಪ್ಲೇಟ್ (`chat_template.jinja`), ಮತ್ತು ಎಲ್ಲಾ ಟೋಕನೈಸರ್ ಫೈಲುಗಳನ್ನು ಜಾರಿಗೊಳಿಸುತ್ತದೆ.

### ONNX Runtime GenAI Model Builder ಮತ್ತು Microsoft Olive

ನೀವು **Microsoft Olive** ನ ಉಲ್ಲೇಖಗಳನ್ನು ಕಂಡು ಮಟ್ಟಬಹುದು, ಮಾದರಿ ಆಪ್ಟಿಮೈಜೇಶನ್‌ಗೆ ಪರ್ಯಾಯ ಸಾಧನವಾಗಿರಬಹುದು. ಎರಡೂ ಟೂಲ್ಗಳು ONNX ಮಾದರಿಗಳನ್ನು ಉತ್ಪಾದಬಹುದು, ಆದರೆ ಅವು ವಿಭಿನ್ನ ಉದ್ದೇಶಗಳು ಮತ್ತು ವ್ಯತ್ಯಾಸಗಳನ್ನು ಹೊಂದಿವೆ:

| | **ONNX Runtime GenAI Model Builder** | **Microsoft Olive** |
|---|---|---|
| **ಪ್ಯಾಕೇಜ್** | `onnxruntime-genai` | `olive-ai` |
| **ಪ್ರಧಾನ್ಯ ಉದ್ದೇಶ** | ONNX Runtime GenAI ಇನ್ಫರೆನ್ಸ್‌ಗಾಗಿ ಜನರೇಟಿವ್ AI ಮಾದರಿಗಳನ್ನು ಪರಿವರ್ತನೆ ಮತ್ತು ಕ್ವಾಂಟೈಸೇಷನ್ ಮಾಡುವುದು | ಅನೇಕ ಬેકೆಂಡ್‌ಗಳು ಮತ್ತು ಹಾರ್ಡ್‌ವೇರ್ ಗುರಿಗಳಿಗಾಗಿ ಸಂಪೂರ್ಣ ಮಾದರಿ ಆಪ್ಟಿಮೈಜೇಶನ್ ಫ್ರೇಮ್ವರ್ಕ್ |
| **ಬಳಕೆ ಸುಲಭತೆ** | ಏಕ ಕಮಾಂಡ್ — ಸ್ಟೆಪ್-ವನ್ ಪರಿವರ್ತನೆ ಮತ್ತು ಕ್ವಾಂಟೈಸೇಷನ್ | ವರ್ಕ್‌ಫ್ಲೋ ಆಧಾರಿತ — YAML/JSON ಹೊಂದಾಣಿಕೆ ದೊರಕುವ ಬಹು ಹಂತದ ಪೈಪ್ಲೈನ್‌ಗಳು |
| **ಔಟ್‌ಪುಟ್ ಫಾರ್ಮ್ಯಾಟ್** | ONNX Runtime GenAI ಫಾರ್ಮ್ಯಾಟ್ (Foundry Local ಗೆ ಸಿದ್ಧ) | ವರ್ಕ್‌ಫ್ಲೋನ ಮೇಲೆ ಅವಲಂಬಿಸಿ ಸಾಮಾನ್ಯ ONNX, ONNX Runtime GenAI ಅಥವಾ ಇತರೆ ಫಾರ್ಮ್ಯಾಟ್‌ಗಳು |
| **ಹಾರ್ಡ್‌ವೇರ್ ಗುರಿಗಳು** | CPU, CUDA, DirectML, TensorRT RTX, WebGPU | CPU, CUDA, DirectML, TensorRT, Qualcomm QNN ಮತ್ತು ಇತರೆ ಅನೇಕ ಗುರಿಗಳು |
| **ಕ್ವಾಂಟೈಸೇಷನ್ ಆಯ್ಕೆಗಳು** | int4, int8, fp16, fp32 | int4 (AWQ, GPTQ, RTN), int8, fp16 ಜೊತೆಗೆ ಗ್ರಾಫ್ ಆಪ್ಟಿಮೈಸೇಷನ್, ಲೇಯರ್ ವೈಸ್ ಟ್ಯೂನಿಂಗ್ |
| **ಮಾದರಿ ವ್ಯಾಪ್ತಿ** | ಜನರೇಟಿವ್ AI ಮಾದರಿ (LLMs, SLMs) | ಯಾವುದೇ ONNXಗೆ ಪರಿವರ್ತಿಸಬಹುದಾದ ಮಾದರಿ (ವೀಶನ್, NLP, ಔಡಿಯೋ, ಮಲ್ಟಿಮೋಡಲ್) |
| **ಉತ್ತಮವಾಗಿದೆ** | ಸ್ಥಳೀಯ ಇನ್ಫರೆನ್ಸ್ ಗಾಗಿ ವೇಗವಾಗಿ ಏಕ-мಾದರಿ ಸಂಯೋಜನೆ | ಸೂಕ್ಷ್ಮ ನಿಯಂತ್ರಣ ಬೇಕಾಗುವ ಉತ್ಪಾದನಾ ಪೈಪ್ಲೈನ್‌ಗಳು |
| **ಆಶ್ರಿತತೆ ಅಳತೆ** | ಮಧ್ಯಮ (PyTorch, Transformers, ONNX Runtime) | ದೊಡ್ಡದು (Olive ಫ್ರೇಮ್ವರ್ಕ್ ಸೇರಿಸಲಾಗಿದೆ, ಕಾರ್ಯ ಪ್ರವಾಹ ಪ್ರಕಾರ ಐಚ್ಛಿಕ ಹೆಚ್ಚುವರಿ) |
| **Foundry Local ಸಂಯೋಜನೆ** | ನೇರ — ಔಟ್‌ಪುಟ್ ತ್ವರಿತವಾಗಿ ಹೊಂದಿಕೊಳ್ಳುವ | `--use_ort_genai` ಫ್ಲ್ಯಾಗ್ ಮತ್ತು ಹೆಚ್ಚುವರಿ ಹೊಂದಾಣಿಕೆ ಅಗತ್ಯ |

> **ಏಕೆ ಈ ಪ್ರಯೋಗಾಲಯದಲ್ಲಿ Model Builder ಬಳಕೆ ಮಾಡಲಾಗಿದೆ:** ಒಬ್ಬ Hugging Face ಮಾದರಿಯನ್ನು ಸಂಯೋಜಿಸುವ ಮತ್ತು ಅದನ್ನು Foundry Local ನೊಂದಿಗೆ ನೊಂದಾಯಿಸುವ ಕಾರ್ಯಕ್ಕೆ Model Builder ಸೂಲಭವುತ್ತೀರುವ ಮತ್ತು ವಿಶ್ವಾಸಾರ್ಹ మార್ಗವಾಗಿದೆ. ಇದು Foundry Local ನಿರೀಕ್ಷಿಸುವ ನಿಖರ ಔಟ್‌ಪುಟ್ ಫಾರ್ಮ್ಯಾಟ್ ಅನ್ನು ಒಂದು ಕಮಾಂಡ್‌ನಲ್ಲಿ ನಿರ್ಮಿಸುತ್ತದೆ. ನಂತರ ನೀವು ಹೆಚ್ಚು ಜಟಿಲ ಆಪ್ಟಿಮೈಸೇಶನ್ ವೈಶಿಷ್ಟ್ಯಗಳನ್ನು ಬೇಕಾದರೆ — ಉದಾಹರಣೆಗೆ, ನಿಖರತೆ-ಏಕೆ ಎಚ್ಚರಿಕೆಯಿರುವ ಕ್ವಾಂಟೈಸೇಷನ್, ಗ್ರಾಫ್ ಸರ್ಜರಿ, ಬಹು ಹಂತದ ಟ್ಯೂನಿಂಗ್ — Olive ಒಂದು ಶಕ್ತಿಶಾಲಿ ಆಯ್ಕೆಯಾಗಿದೆ. وڌيڪ ಮಾಹಿತಿಗೆ [Microsoft Olive ಡಾಕ್ಯುಮೆಂಟೇಶನ್](https://microsoft.github.io/Olive/) ನೋಡಿ.

---

## ಪ್ರಯೋಗಾಲಯಾಭ್ಯಾಸಗಳು

### ಅಭ್ಯಾಸ 1: ONNX Runtime GenAI Model Builder ಅನ್ನು ಸ್ಥಾಪಿಸುವುದು

ONNX Runtime GenAI ಪ್ಯಾಕೇಜ್ ಅನ್ನು ಸ್ಥಾಪಿಸಿ, ಇದರಲ್ಲಿ ಮಾದರಿ билдರ್ ಟೂಲ್ ಒಳಗೊಂಡಿದೆ:

```bash
pip install onnxruntime-genai
```

ಸ್ಥಾಪನೆಯನ್ನು ಪರಿಶೀಲಿಸಲು ಮತ್ತು ಮಾದರಿ билдರ್ ಲಭ್ಯವಿದೆಯೇ ಎಂದು ನೋಡಲು:

```bash
python -m onnxruntime_genai.models.builder --help
```

ನೀವು `-m` (ಮಾದರಿ ಹೆಸರು), `-o` (ಔಟ್‌ಪುಟ್ ಪಥ), `-p` (ಪ್ರಿಸಿಷನ್), ಮತ್ತು `-e` (ಕಾರ್ಯನಿರ್ವಹಣಾ ಪೂರೈಕೆದಾರ) ಮೊದಲಾದ ನಿಯಮಕಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡುವ ಸಹಾಯ ಔಟ್‌ಪುಟ್ ನೋಡಬೇಕು.

> **ಸೂಚನೆ:** ಮಾದರಿ билдರ್ PyTorch, Transformers ಮತ್ತು ಬೇರೆ ಅನೇಕ ಪ್ಯಾಕೇಜ್‌ಗಳ ಮೇಲೆ ಅವಲಂಬಿತವಾಗಿದೆ. ಸ್ಥಾಪನೆಗೆ ಕೆಲವು ನಿಮಿಷಗಳು ಬೇಕಾಗಬಹುದು.

---

### ಅಭ್ಯಾಸ 2: CPU ಗಾಗಿ Qwen3-0.6B ಸಂಯೋಜಿಸುವುದು

ಕೆಳಗಿನ ಕಮಾಂಡ್ ಅನ್ನು ಚಲಾಯಿಸಿ Qwen3-0.6B ಮಾದರಿಯನ್ನು Hugging Face ನಿಂದ ಡೌನ್‌ಲೋಡ್ ಮಾಡಿ ಮತ್ತು int4 ಕ್ವಾಂಟೈಸೇಷನ್ ಬಳಸಿಕೊಂಡು CPU ಇನ್ಫರೆನ್ಸ್ ಗಾಗಿ ಸಂಯೋಜಿಸಿ:

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

#### ಪ್ರತಿ ನಿಯಮನದ ಪರಿಸ್ಥಿತಿ

| ನಿಯಮಕ | ಉದ್ದೇಶ | ಉಪಯೋಗಿಸಿದ ಮೌಲ್ಯ |
|-----------|---------|------------|
| `-m` | Hugging Face ಮಾದರಿ ID ಅಥವಾ ಸ್ಥಳೀಯ ಡೈರೆಕ್ಟರಿ ಪಥ | `Qwen/Qwen3-0.6B` |
| `-o` | ಸಂಯೋಜಿತ ONNX ಮಾದರಿ ಉಳಿಸಲಾದ ಡೈರೆಕ್ಟರಿ | `models/qwen3` |
| `-p` | ಸಂಯೋಜನೆಯ ಸಮಯದಲ್ಲಿ ಅನ್ವಯಿಸಿದ ಕ್ವಾಂಟೈಸೇಷನ್ ಪ್ರಿಸಿಷನ್ | `int4` |
| `-e` | ONNX Runtime ಕಾರ್ಯನಿರ್ವಹಣಾ ಪೂರೈಕೆದಾರ (ಲಕ್ಷ್ಯ ಹಾರ್ಡ್‌ವೇರ್) | `cpu` |
| `--extra_options hf_token=false` | Hugging Face ಪ್ರಮಾಣೀಕರಣವನ್ನು ತಪ್ಪಿಸಿಕೊಳ್ಳುವುದು (ಸಾರ್ವಜನಿಕ ಮಾದರಿಗಾಗಿ ಚುಕ್ಕಾಣಿ) | `hf_token=false` |

> **ಇದಲ್ಲಿಗೆ ಎಷ್ಟು ಕಾಲ ತೆಗೆದುಕೊಳ್ಳುತ್ತದೆ?** ಸಂಯೋಜನೆ ಸಮಯವು ನಿಮ್ಮ ಹಾರ್ಡ್‌ವೇರ್ ಮತ್ತು ಮಾದರಿ ಗಾತ್ರದ ಮೇಲೆ ಅವಲಂಬಿಸಿದೆ. ಆಧುನಿಕ CPU ಮೇಲೆ Qwen3-0.6B ಗೆ int4 ಕ್ವಾಂಟೈಸೇಷನ್ ಅಂದಾಜಾಗಿ 5 ರಿಂದ 15 ನಿಮಿಷಗಳವರೆಗೆ ಸಂಭವಿಸುತ್ತದೆ. ದೊಡ್ಡ ಮಾದರಿಗಳು ಹೀಗಕ್ಕಿಂತ ಹೆಚ್ಚು ಸಮಯವೆತ್ತಬಹುದು.

ಕಮಾಂಡ್ ಪೂರ್ಣಗೊಂಡ ನಂತರ `models/qwen3` ಡೈರೆಕ್ಟರಿಯಲ್ಲಿ ಸಂಯೋಜಿತ ಮಾದರಿ ಫೈಲ್‌ಗಳನ್ನು ನೋಡಬಹುದು. ಔಟ್‌ಪುಟ್ ಪರಿಶೀಲಿಸಿ:

```bash
ls models/qwen3
```

ನೀವು ತಳಗಿನ ಫೈಲ್‌ಗಳನ್ನು ನೋಡಬೇಕು:
- `model.onnx` ಮತ್ತು `model.onnx.data` — ಸಂಯೋಜಿತ ಮಾದರಿ ತೂಕಗಳು
- `genai_config.json` — ONNX Runtime GenAI ಕಾನ್ಫಿಗರೇಷನ್
- `chat_template.jinja` — ಮಾದರಿಯ ಚಾಟ್ ಟೆಂಪ್ಲೇಟ್ (ಸ್ವಯಂಚಾಲಿತ)
- `tokenizer.json`, `tokenizer_config.json` — ಟೋಕನೈಸರ್ ಫೈಲ್‌ಗಳು
- ಇತರೆ ಶಬ್ದಕೋಶ ಮತ್ತು ಕಾನ್ಫಿಗರೇಷನ್ ಫೈಲ್‌ಗಳು

---

### ಅಭ್ಯಾಸ 3: GPU ಗಾಗಿ ಸಂಯೋಜಿಸುವುದು (ಐಚ್ಛಿಕ)

ನಿಮ್ಮ ಬಳಿ CUDA ಬೆಂಬಲವಿರುವ NVIDIA GPU ಇದ್ದರೆ, ವೇಗವಾದ ಇನ್ಫರೆನ್ಸ್ ಗಾಗಿ GPU-ಆಪ್ಟಿಮೈಸ್ ಮಾಡಿದ ಪ್ರಕಾರವನ್ನು ಸಂಯೋಜಿಸಬಹುದು:

```bash
python -m onnxruntime_genai.models.builder \
    -m Qwen/Qwen3-0.6B \
    -o models/qwen3-gpu \
    -p fp16 \
    -e cuda \
    --extra_options hf_token=false
```

> **ಸೂಚನೆ:** GPU ಸಂಯೋಜನೆಗೆ `onnxruntime-gpu` ಮತ್ತು ಕಾರ್ಯ ನಿರ್ವಹಿಸುವ CUDA ಸ್ಥಾಪನೆಯು ಬೇಕು. ಇವುಗಳుంటಿಲ್ಲದಿದ್ದರೆ, ಮಾದರಿ билдರ್ ದೋಷ ವರದಿ ಮಾಡುತ್ತದೆ. ನೀವು ಈ ಅಭ್ಯಾಸವನ್ನು ಬಿಟ್ಟು CPU ಆವೃತ್ತಿ ಉಪಯೋಗಿಸಬಹುದು.

#### ಹಾರ್ಡ್‌ವೇರ್-ನಿರ್ದಿಷ್ಟ ಸಂಯೋಜನಾ ಉಲ್ಲೇಖ

| ಗುರಿ | ಕಾರ್ಯನಿರ್ವಹಣಾ ಪೂರೈಕೆದಾರ (`-e`) | ಶಿಫಾರಸ್ಯ ಮಾಡಲಾದ ಪ್ರಿಸಿಷನ್ (`-p`) |
|--------|---------------------------|------------------------------|
| CPU | `cpu` | `int4` |
| NVIDIA GPU | `cuda` | `fp16` ಅಥವಾ `int4` |
| DirectML (Windows GPU) | `dml` | `fp16` ಅಥವಾ `int4` |
| NVIDIA TensorRT RTX | `NvTensorRtRtx` | `fp16` |
| WebGPU | `webgpu` | `int4` |

#### ಪ್ರಿಸಿಷನ್ ವ್ಯಾಪ್ತಿ

| ಪ್ರಿಸಿಷನ್ | ಗಾತ್ರ | ವೇಗ | ಗುಣಮಟ್ಟ |
|-----------|------|-------|---------|
| `fp32` | ಅತಿ ದೊಡ್ಡದು | ಸಡಿಲವೇಗ | ಅತಿ ಹೆಚ್ಚು ನಿಖರತೆ |
| `fp16` | ದೊಡ್ಡದು | ವೇಗವಾಗಿ (GPU) | ಬಹಳ ಚೆನ್ನಾದ ನಿಖರತೆ |
| `int8` | ಸಣ್ಣದು | ವೇಗ | ಸ್ವಲ್ಪ ನಿಖರತೆ ನಷ್ಟ |
| `int4` | ಅತಿ ಸಣ್ಣದು | ಅತಿ ವೇಗವಾಗಿ | ಮಧ್ಯಮ ನಿಖರತೆ ನಷ್ಟ |

ಬಹುಶಃ ಸ್ಥಳೀಯ ಅಭಿವೃದ್ಧಿಗಾಗಿ, CPU ಮೇಲೆ `int4` ವೇಗ ಮತ್ತು ಸಂಪನ್ಮೂಲ ಬಳಕೆಯ ಸೂಕ್ತ ಸಮತೋಲನವನ್ನು ಒದಗಿಸುತ್ತದೆ. ಉತ್ಪಾದನಾ ಗುಣಮಟ್ಟದ ಫಲಿತಾಂಶಕ್ಕಾಗಿ CUDA GPU ಮೇಲೆ `fp16` ಶಿಫಾರಸ್ಸಾಗಿದೆ.

---

### ಅಭ್ಯಾಸ 4: ಚಾಟ್ ಟೆಂಪ್ಲೇಟ್ ಕಾನ್ಫಿಗರೇಷನ್ ಸೃಷ್ಟಿಸುವುದು

ಮಾದರಿ билдರ್ ಸ್ವತಃ `chat_template.jinja` ಫೈಲ್ ಮತ್ತು ಔಟ್‌ಪುಟ್ ಡೈರೆಕ್ಟರಿಯಲ್ಲಿ `genai_config.json` ಫೈಲ್ ಸೃಷ್ಟಿಸುತ್ತದೆ. ಆದರೆ, Foundry Local ಗೆ ಮಾದರಿಯ ಪಾರ್ಥnership_prompt ವಿನ್ಯಾಸವನ್ನು ಅರ್ಥಮಾಡಿಕೊಳ್ಳಲು `inference_model.json` ಫೈಲ್ ಕೂಡ ಬೇಕಾಗುತ್ತದೆ. ಇದೇ ಫೈಲ್ ಮಾದರಿಯ ಹೆಸರು ಮತ್ತು ಬಳಕೆದಾರ ಸಂದೇಶಗಳನ್ನು ಸರಿಯಾದ ವಿಶೇಷ ಟೋಕನ್ಗಳೊಂದಿಗೆ ಮುಚ್ಚುವ ಪ್ರಾಂಪ್ಟ್ ಟೆಂಪ್ಲೇಟನ್ನು ವ್ಯಾಖ್ಯಾನಿಸುತ್ತದೆ.

#### ಹಂತ 1: ಸಂಯೋಜಿತ ಔಟ್‌ಪುಟ್ ಪರಿಶೀಲನೆ

ಸಂಯೋಜಿತ ಮಾದರಿ ಡೈರೆಕ್ಟರಿಯ ವಿಷಯಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ:

```bash
ls models/qwen3
```

ನೀವು ಕೆಳಗಿನಂತೆ ಫೈಲ್‌ಗಳನ್ನು ನೋಡಬಹುದು:
- `model.onnx` ಮತ್ತು `model.onnx.data` — ಸಂಯೋಜಿತ ಮಾದರಿ ತೂಕಗಳು
- `genai_config.json` — ONNX Runtime GenAI ಕಾನ್ಫಿಗರೇಷನ್ (ಸ್ವಯಂಚಾಲಿತ)
- `chat_template.jinja` — ಮಾದರಿಯ ಚಾಟ್ ಟೆಂಪ್ಲೇಟ್ (ಸ್ವಯಂಚಾಲಿತ)
- `tokenizer.json`, `tokenizer_config.json` — ಟೋಕನೈಸರ್ ಫೈಲ್‌ಗಳು
- ಇತರ ಕಾನ್ಫಿಗರೇಷನ್ ಮತ್ತು ಶಬ್ದಕೋಶ ಫೈಲ್‌ಗಳು

#### ಹಂತ 2: inference_model.json ಫೈಲ್ ರಚನೆ

`inference_model.json` ಫೈಲ್ Foundry Local ಗೆ ಪ್ರಾಂಪ್ಟ್ಗಳ ರಚಿಸುವ ವಿಧಾನ ತಿಳಿಸುತ್ತದೆ. Python ಸ್ಕ್ರಿಪ್ಟ್ `generate_chat_template.py` ಅನ್ನು ರೆಪೊ ರೂಟ್ ಜಾಗದಲ್ಲಿ (ಅಂದರೆ ನಿಮ್ಮ `models/` ಫೋಲ್ಡರ್ ಇರುವ ಅದೇ ಫೋಲ್ಡರಿನಲ್ಲಿ) ಸೃಷ್ಟಿಸಿ:

```python
"""Generate an inference_model.json chat template for Foundry Local."""

import json
from transformers import AutoTokenizer

MODEL_PATH = "models/qwen3"

tokenizer = AutoTokenizer.from_pretrained(MODEL_PATH)

# ಚಾಟ್ ಟೆಂಪ್ಲೇಟನ್ನು ತೆಗೆದುಕೊಳ್ಳಲು ಕನಿಷ್ಠ ಸಂವಾದವನ್ನು ನಿರ್ಮಿಸಿ
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

# inference_model.json ರಚನೆಯನ್ನು ನಿರ್ಮಿಸಿ
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

ರೋಗಾ ರೂಟ್‌ನಿಂದ ಸ್ಕ್ರಿಪ್ಟ್ ಚಲಾಯಿಸಿ:

```bash
python generate_chat_template.py
```

> **ಸೂಚನೆ:** `transformers` ಪ್ಯಾಕೇಜ್ ಈಗಾಗಲೇ `onnxruntime-genai` ಅವಲಂಬಿತೆಯಲ್ಲಿ ಸ್ಥಾಪಿತವಾಗಿದೆ. ನೀವು `ImportError` ಕಾಣಿಸಿದರೆ ಮೊದಲಿಗೆ `pip install transformers` ಕೈಗೊಳ್ಳಿ.

ಸ್ಕ್ರಿಪ್ಟ್ `models/qwen3` ಡೈರೆಕ್ಟರಿಯ ಒಳಗೆ ಒಂದು `inference_model.json` ಫೈಲ್ ಅನ್ನು ತಯಾರಿಸುತ್ತದೆ. ಈ ಫೈಲ್ Foundry Local ಗೆ Qwen3 ಗೆ ಹೊಂದಿ ಬಳಕೆದಾರ ಇನ್‌ಪುಟ್ ಅನ್ನು ಸರಿಯಾದ ವಿಶೇಷ ಟೋಕನ್ಗಳೊಂದಿಗೆ ಮುಚ್ಚುವುದು ಹೇಗೆ ಎನ್ನುವುದನ್ನು ತಿಳಿಸುತ್ತದೆ.

> **ಮುಖ್ಯ:** `inference_model.json` ನಲ್ಲಿರುವ `"Name"` ಕ್ಷೇತ್ರ (ಈ ಸ್ಕ್ರಿಪ್ಟ್‌ನಲ್ಲಿ `qwen3-0.6b` ಗೆ ಹೊಂದಿಸಲಾಗಿದೆ) ನಿಮ್ಮ ಎಲ್ಲಾ ಮುಂದಿನ ಕಮಾಂಡ್‌ಗಳು ಮತ್ತು API ಕರೆಗಳಲ್ಲಿ ಬಳಸಬೇಕಾದ ಮಾದರಿ ಉರುಪು. ನೀವು ಈ ಹೆಸರನ್ನು ಬದಲಾಯಿಸಿದರೆ, ಅಭ್ಯಾಸಗಳು 6–10 ನಲ್ಲಿ ಮಾದರಿ ಹೆಸರನ್ನು ತಕ್ಕಂತೆ ನವೀಕರಿಸಿ.

#### ಹಂತ 3: ಕಾನ್ಫಿಗರೇಷನ್ ಪರಿಶೀಲನೆ

`models/qwen3/inference_model.json` ತೆರೆದು ಅದು `Name` ಕ್ಷೇತ್ರ ಮತ್ತು `assistant` ಮತ್ತು `prompt` ಕೀಲಿಗಳುಳ್ಳ `PromptTemplate` ವಸ್ತುವನ್ನು ಹೊಂದಿದೆಯೇ ಎಂದು ದೃಢೀಕರಿಸಿ. ಪ್ರಾಂಪ್ಟ್ ಟೆಂಪ್ಲೇಟಿನಲ್ಲಿ `<|im_start|>` ಮತ್ತು `<|im_end|>` ನಂತಹ ವಿಶೇಷ ಟೋಕನ್ಗಳು ಇರಬೇಕಾಗಿವೆ (ಮಾದರಿಯ ಚಾಟ್ ಟೆಂಪ್ಲೇಟ್ ಆಧರಿಸಿ).

> **ಹಸ್ತಚಾಲಿತ ಪರ್ಯಾಯ:** ನೀವು ಸ್ಕ್ರಿಪ್ಟ್ ಓಡಿಸುವದಕ್ಕೆ ಇಚ್ಛೆಪಡದಿದ್ದರೆ, ಫೈಲ್ ಅನ್ನು ಕೈಯಿಂದ ರಚಿಸಬಹುದು. ಪ್ರಮುಖ ಅವಶ್ಯಕತೆ ಎಂದರೆ `prompt` ಕ್ಷೇತ್ರವು `{Content}` ಎಂಬ ಬಳಕೆದಾರ ಸಂದೇಶದ ಪ್ಲೇಸ್‌ಹೋಲ್ಡರ್ ಸೇರಿದಂತೆ ಮಾದರಿಯ ಸಂಪೂರ್ಣ ಚಾಟ್ ಟೆಂಪ್ಲೇಟ್ ಇರಬೇಕು.

---

### ಅಭ್ಯಾಸ 5: ಮಾದರಿ ಡೈರೆಕ್ಟರಿ ರಚನೆ ರಚನೆ ಪರಿಶೀಲನೆ
ಮಾದರಿ ನಿರ್ಮಾಪಕವು ಎಲ್ಲಾ ಸಂಯೋಜಿಸಲಾಗಿದ ಫೈಲ್‌ಗಳನ್ನು ನೀವು ಸೂಚಿಸಿರುವ output ಡೈರಕ್ಟರಿಯಲ್ಲೇ ನೇರವಾಗಿ ಇರಿಸುತ್ತದೆ. ಅಂತಿಮ ರಚನೆಯನ್ನು ಸರಿಯಾಗಿ ತೋರಿಸುತ್ತಿದೆಯೇ ಎಂದು ಪರಿಶೀಲಿಸಿ:

```bash
ls models/qwen3
```

ಡೈರಕ್ಟರಿಯಲ್ಲಿ ಕೆಳಗಿನ ಫೈಲ್‌ಗಳು ಇರಬೇಕು:

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

> **ಗಮನಿಸಿ:** ಕೆಲವು ಇತರ ಸಂಯೋಜನಾ ಉಪಕರಣಗಳಂತೆ, ಮಾದರಿ ನಿರ್ಮಾಪಕವು ನೆಸ್ಟ್ ಆದ ಉಪಡೈರಕ್ಟರಿಗಳನ್ನು ರಚಿಸುವುದಿಲ್ಲ. ಎಲ್ಲಾ ಫೈಲ್‌ಗಳು ನೇರವಾಗಿ output ಫೋಲ್ಡರ್‌ನಲ್ಲಿ ಇರುತ್ತವೆ, ಇದು Foundry Local ನ ನಿರೀಕ್ಷೆಗಗೆ ಸರಿಹೊಂದುತ್ತದೆ.

---

### ಅಭ್ಯಾಸ 6: Foundry Local ಕ್ಯಾಸೆಗೆ ಮಾದರಿಯನ್ನು ಸೇರ್ಪಡೆಮಾಡಿ

Foundry Local ಗೆ ನಿಮ್ಮ ಸಂಯೋಜಿಸಲಾದ ಮಾದರಿಯ ಡೈರಕ್ಟರಿಯನ್ನು ಅದರ ಕ್ಯಾಸೆಗೆ ಸೇರಿಸಿ:

```bash
foundry cache cd models/qwen3
```

ಮಾದರಿ ಕ್ಯಾಸೆ‌ನಲ್ಲಿ ಸರಿಯಾಗಿ ಕಾಣುತ್ತಿದೆಯೇ ಎಂದು ಪರಿಶೀಲಿಸಿ:

```bash
foundry cache ls
```

ನೀವು ನಿಮ್ಮ ಕಸ್ಟಮ್ ಮಾದರಿಯನ್ನು ಮೊದಲು ಕ್ಯಾಸೆ ಮಾಡಲಾದ ಮಾದರಿಗಳು (ಉದಾ: `phi-3.5-mini` ಅಥವಾ `phi-4-mini`) ಜೊತೆಗೆ ಪಟ್ಟಿಯಲ್ಲಿ ಕಾಣಬೇಕು.

---

### ಅಭ್ಯಾಸ 7: CLI ಮೂಲಕ ಕಸ್ಟಮ್ ಮಾದರಿ ಚಾಲನೆಮಾಡಿ

ನೀವು ಹೊಸದಾಗಿ ಸಂಯೋಜಿಸಿರುವ ಮಾದರಿಯೊಂದಿಗೆ ಸಂವಾದ ಆಧಾರಿತ ಚಾಟ್ ಸೆಷನನ್ನು ಪ್ರಾರಂಭಿಸಿ (`qwen3-0.6b` ಅಲಯಾಸ್ `inference_model.json` ಲ்<Name>‌ಕ್ಷೇತ್ರದಲ್ಲಿ ನೀವು ನೀಡಿದ ಹೆಸರಿನಿಂದ ಬಂದಿದೆ):

```bash
foundry model run qwen3-0.6b --verbose
```

`--verbose` ಧ್ವಜವು ಹೆಚ್ಚುವರಿ ವಿಶ್ಲೇಷಣೆ ಮಾಹಿತಿಯನ್ನು ತೋರಿಸುತ್ತದೆ, ಇದು ಕಸ್ಟಮ್ ಮಾದರಿಯನ್ನು ಮೊದಲ ಬಾರಿ ಪರೀಕ್ಷಿಸುವಾಗ ಸಹಾಯಕವಾಗಿದೆ. ಮಾದರಿ ಯಶಸ್ವಿಯಾಗಿ ಲೋಡ್ ಆದರೆ, ಸಂವಾದ ಪ್ರಾಂಪ್ಟ್ ಕಾಣುತ್ತದೆ. ಕೆಲವು ಸಂದೇಶಗಳನ್ನು ಪ್ರಯತ್ನಿಸಿ:

```
You: What is the capital of France?
You: Write a short poem about the ocean.
You: Explain quantum computing in simple terms.
```

ಸೆಷನ್ ಮುಗಿಸಲು `exit` ಟೈಪ್ ಮಾಡಿ ಅಥವಾ `Ctrl+C` ಒತ್ತಿರಿ.

> **ಸಮಸ್ಯೆ ಪರಿಹಾರ:** ಮಾದರಿ ಲೋಡ್ ಆಗದೆ ಇದ್ದರೆ, ಕೆಳಗಿನವುಗಳನ್ನು ಪರಿಶೀಲಿಸಿ:
> - `genai_config.json` ಫೈಲ್ ಮಾದರಿ ನಿರ್ಮಾಪಕದಿಂದ ರಚಿಸಲಾಗಿದೆ.
> - `inference_model.json` ಫೈಲ್ ಅಸ್ತಿತ್ವದಲ್ಲಿದೆ ಮತ್ತು ಮಾನ್ಯ JSON ಆಗಿದೆ.
> - ONNX ಮಾದರಿ ಫೈಲ್‌ಗಳು ಸರಿಯಾದ ಡೈರಕ್ಟರಿಯಲ್ಲಿವೆ.
> - ನೀವು ಸಾಕಷ್ಟು RAM ಹೊಂದಿದ್ದೀರಿ (Qwen3-0.6B int4ಗೆ ಪ್ರಾಯೋಗಿಕವಾಗಿ 1 GB ಬೇಕು).
> - Qwen3 ಒಂದು ತರ್ಕ ಮಾಡುವ ಮಾದರಿ ಆಗಿದ್ದು, `<think>` ಟ್ಯಾಗ್‌ಗಳನ್ನು ಉತ್ಪಾದಿಸುತ್ತದೆ. ಉತ್ತರಗಳ ಮೊದಲು `<think>...</think>` ಕಾಣಿಸಿಕೊಂಡರೆ, ಇದು ಸಾಮಾನ್ಯ ವರ್ತನೆ. `inference_model.json`ನ ಪ್ರಾಂಪ್ಟ್ ಟೆಂಪ್ಲೇಟನ್ನು ಪರಿಷ್ಕರಿಸಿ ಧಾರ್ಮಿಕ ಚಿಂತನೆ output‌ನ್ನು ಅಡ್ಡಗಟ್ಟಬಹುದು.

---

### ಅಭ್ಯಾಸ 8: REST API ಮೂಲಕ ಕಸ್ಟಮ್ ಮಾದರಿಗೆ ಪ್ರಶ್ನೆಮಾಡಿ

ನೀವು ಅಭ್ಯಾಸ 7 ರಲ್ಲಿ ಸಂವಾದ ಸೆಷನನ್ನು ನಿರ್ಗಮಿಸಿದಿದ್ದರೆ, ಮಾದರಿ ಲೋಡ್ ಆಗಿರಲಾರದು. ಮೊದಲು Foundry Local ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ ಮತ್ತು ಮಾದರಿಯನ್ನು ಲೋಡ್ ಮಾಡಿ:

```bash
foundry service start
foundry model load qwen3-0.6b
```

ಸೇವೆಯು ಯಾವ ಪೋರ್ಟ್‌ನಲ್ಲಿ ನಡೆಯುತ್ತಿದೆ ಎಂದು ಪರಿಶೀಲಿಸಿ:

```bash
foundry service status
```

ನಂತರ ವಿನಂತಿಯನ್ನು ಕಳುಹಿಸಿ (`5273` ನಿಮ್ಮ ನಿಜವಾದ ಪೋರ್ಟ್‌ನಿಂದ ಬದಲಾಗಬಹುದು):

```bash
curl -X POST http://localhost:5273/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{"model": "qwen3-0.6b", "messages": [{"role": "user", "content": "What are three interesting facts about honeybees?"}], "temperature": 0.7, "max_tokens": 200}'
```

> **Windows ಸೂಚನೆ:** ಮೇಲಿನ `curl` ಕಮಾಂಡ್ ಬಾಷ್ ಸಿಂಟ್ಯಾಕ್ಸ್ ಅನ್ನು ಬಳಸುತ್ತದೆ. ವಿಂಡೋಸ್‌ನಲ್ಲಿ, ಬದಲು PowerShellನಲ್ಲಿ `Invoke-RestMethod` ಅನ್ನು ಬಳಸಿ.

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

### ಅಭ್ಯಾಸ 9: OpenAI SDK ಮೂಲಕ ಕಸ್ಟಮ್ ಮಾದರಿ ಬಳಸಿ

ನೀವು ಕಸ್ಟಮ್ ಮಾದರಿ ಸಂಪರ್ಕ ಮಾಡಲು ನೇರವಾಗಿ ಆಪ್ಯಾಯಿತ OpenAI SDK ಕೋಡ್ ಅನ್ನು ಬಳಸಬಹುದು (ಭಾಗ 3 ಈಜು ನೋಡಿ: [Part 3](part3-sdk-and-apis.md)). ಯ唯 ನೀವ ಮಾದರಿ ಹೆಸರನ್ನು ಮಾತ್ರ ಬದಲಾಯಿಸಬೇಕು.

<details>
<summary><b>Python</b></summary>

```python
"""Chat with a custom-compiled model via Foundry Local."""

from openai import OpenAI

client = OpenAI(
    base_url="http://localhost:5273/v1",
    api_key="foundry-local",  # ಫೌಂಡ್ರಿ ಸ್ಥಳೀಯವಾಗಿ API ಕೀಲಿಗಳನ್ನು ಪರಿಶೀಲಿಸುವುದಿಲ್ಲ
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
  apiKey: "foundry-local", // ಫೌಂಡ್ರಿ ಲೋಕಲ್ API ಕೀಗಳನ್ನು ಸತ್ಯಾಪಿಸಿಕೊಂಡಿಲ್ಲ
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

> **ಪ್ರಮುಖ ವಿಷಯ:** Foundry Local ಒಂದು OpenAI-ಸಮರ್ಪಕ API ಅನ್ನು ನೀಡುತ್ತದೆ, ಅಂದರೆ ನಿರ್ಮಿತ ಮಾದರಿಗಳೊಂದಿಗೆ ಕಾರ್ಯನಿರ್ವಹಿಸುವ ಯಾವುದೇ ಕೋಡ್ ನಿಮ್ಮ ಕಸ್ಟಮ್ ಮಾದರಿಗಳೊಂದಿಗೆ ಕೂಡ ಕೆಲಸಮಾಡುತ್ತದೆ. ನಿಮಗೆ ಕೇವಲ `model` ಪ್ಯಾರಾಮೀಟರ್ ಬದಲಾಯಿಸುವುದು ಸಾಕು.

---

### ಅಭ್ಯಾಸ 10: Foundry Local SDK ಮೂಲಕ ಕಸ್ಟಮ್ ಮಾದರಿಯನ್ನು ಪರೀಕ್ಷಿಸಿ

ಹಿಂದಿನ ಲ್ಯಾಬ್‌ಗಳಲ್ಲಿ ನೀವು Foundry Local SDK ಬಳಸಿ ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ, ಎಂಡ್‌ಪಾಯಿಂಟ್ ಹುಡುಕಿ, ಮತ್ತು ಮಾದರಿಗಳನ್ನು ಸ್ವಯಂಚಾಲಿತವಾಗಿ ನಿರ್ವಹಿಸಿದ್ದೀರಿ. ನೀವು ಅದೇ ರೀತಿಯಲ್ಲಿ ನಿಮ್ಮ ಕಸ್ಟಮ್-ಸಂಯೋಜಿತ ಮಾದರಿಯನ್ನು ಬಳಸಬಹುದು. SDK ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸುವುದು ಮತ್ತು ಎಂಡ್‌ಪಾಯಿಂಟ್ ಕಂಡುಹಿಡಿಯುವುದನ್ನು ನಿಯಮಿತವಾಗಿ ನಿರ್ವಹಿಸುತ್ತದೆ, ಆದ್ದರಿಂದ ನೇರವಾಗಿ `localhost:5273` ಅನ್ನು ಕೋಡ್ ಮಾಡಿ ಹಾಕಬೇಕಾಗಿಲ್ಲ.

> **ಗಮನಿಸಿ:** ಈ ಉದಾಹರಣೆಗಳನ್ನು ನಡೆಸುವ ಮೊದಲು Foundry Local SDK ಇನ್‌ಸ್ಟಾಲ್‌ ಆಗಿರಲಿ:
> - **Python:** `pip install foundry-local openai`
> - **JavaScript:** `npm install foundry-local-sdk openai`
> - **C#:** `Microsoft.AI.Foundry.Local` ಮತ್ತು `OpenAI` NuGet ಪ್ಯಾಕೇಜುಗಳನ್ನು ಸೇರಿಸಿ
>
> ಪ್ರತಿ ಸ್ಕ್ರಿಪ್ಟ್ ಫೈಲ್ ಅನ್ನು **ರೆಪೊರಿಟರಿ ರೂಟ್‌ನಲ್ಲಿ** ಉಳಿಸಿ (ನಿಮ್ಮ `models/` ಫೋಲ್ಡರ್ ಇದ್ದ ಡೈರಕ್ಟರಿ).

<details>
<summary><b>Python</b></summary>

```python
"""Test a custom-compiled model using the Foundry Local SDK."""

import sys
from foundry_local import FoundryLocalManager
from openai import OpenAI

model_alias = "qwen3-0.6b"

# ಹಂತ 1: ಫೌಂಡ್ರೀ ಲೋಕಲ್ ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ ಮತ್ತು ಕಸ್ಟಮ್ ಮಾದರಿಯನ್ನು ಲೋಡ್ ಮಾಡಿ
print("Starting Foundry Local service...")
manager = FoundryLocalManager()
manager.start_service()

# ಹಂತ 2: ಕಸ್ಟಮ್ ಮಾದರಿಗಾಗಿ ಕ್ಯಾಶೆ ಪರಿಶೀಲಿಸಿ
cached = manager.list_cached_models()
print(f"Cached models: {[m.id for m in cached]}")

# ಹಂತ 3: ಮಾದರಿಯನ್ನು ಮೆಮರಿಗೆ ಲೋಡ್ ಮಾಡಿ
print(f"Loading model: {model_alias}...")
manager.load_model(model_alias)

# ಹಂತ 4: SDK-ಹುಡುಕಿದ ಎಂಡ್ಪಾಯಿಂಟ್ ಬಳಸಿ OpenAI ಕ್ಲೈಂಟ್ ರಚಿಸಿ
client = OpenAI(
    base_url=manager.endpoint,
    api_key=manager.api_key,
)

# ಹಂತ 5: ಸ್ಟ್ರೀಮಿಂಗ್ ಚಾಟ್ ಪೂರ್ಣಗೊಳಿಸುವಿಕೆ ವಿನಂತಿಯನ್ನು ಕಳುಹಿಸಿ
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

ನಡಿಸಿ:

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

// ಹಂತ 1: ಫೌಂಡ्री ಲೋಕಲ್ ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ
console.log("Starting Foundry Local service...");
FoundryLocalManager.create({ appName: "CustomModelDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ಹಂತ 2: ಕ್ಯಾಟಲೋಗ್‌ನಿಂದ ಕಸ್ಟಮ್ ಮಾದರಿಯನ್ನು ಪಡೆಯಿರಿ
const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);
console.log(`Model: ${model.alias} (cached: ${model.isCached})`);

// ಹಂತ 3: ಮಾದರಿಯನ್ನು ಮೆಮೊರಿಯಲ್ಲಿ ಲೋಡ್ ಮಾಡಿ
console.log(`Loading model: ${modelAlias}...`);
await model.load();
console.log("Loaded model:", model.id);

// ಹಂತ 4: SDK-ಗೆ ಪತ್ತೆಯಾದ ಎಂಡ್‌ಪಾಯಿಂಟ್ ಬಳಸಿ OpenAI ಕ್ಲೈಯಂಟ್ ರಚಿಸಿ
const client = new OpenAI({
  baseURL: manager.urls[0] + "/v1",
  apiKey: "foundry-local",
});

// ಹಂತ 5: ಸ್ಟ್ರೀಮಿಂಗ್ ಚಾಟ್ ಪೂರ್ಣಗೊಳ್ಳುವಿಕೆ ವಿನಂತಿಯನ್ನು ಕಳುಹಿಸಿ
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

ನಡಿಸಿ:

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

> **ಪ್ರಮುಖ ವಿಷಯ:** Foundry Local SDK ಎಂಡ್‌ಪಾಯಿಂಟ್ ಅನ್ನು ಡೈನಾಮಿಕ್ ಆಗಿ ಕಂಡುಹಿಡಿಯುತ್ತದೆ, ಆದ್ದರಿಂದ ನೀವು ಎಂದಿಗೂ ಪೋರ್ಟ್ ಸಂಖ್ಯೆಯನ್ನು ನೇರವಾಗಿ ಹಾರ್ಡ್-ಕೋಡ್ ಮಾಡಬೇಡಿ. ಇದು ಉತ್ಪಾದನೆ ಅನ್ವಯ ಮಾಡಿದಲ್ಲಿ ಶಿಫಾರಸು ಮಾಡಲಾದ ವಿಧಾನ. ನಿಮ್ಮ ಕಸ್ಟಮ್-ಸಂಯೋಜಿತ ಮಾದರಿ SDK ಮೂಲಕ ನಿರ್ಮಿತ ಕ್ಯಾಟಲಾಗ್ ಮಾದರಿಗಳಂತೆ ಒಂದೇ ರೀತಿಯಾಗಿ ಕಾರ್ಯನಿರ್ವಹಿಸುತ್ತದೆ.

---

## ಸಂಯೋಜಿಸಲು ಮಾದರಿ ಆಯ್ಕೆಮಾಡುವುದು

Qwen3-0.6B ಈ ಲ್ಯಾಬ್‌ನಲ್ಲಿ ಮಾದರಿ ಉದಾಹರಣೆಯಾಗಿ ಬಳಸಲಾಗಿದೆ ಏಕೆಂದರೆ ಇದು ಸಣ್ಣದು, ವೇಗವಾಗಿ ಸಂಯೋಜಿಸಲಾಗುತ್ತದೆ ಮತ್ತು ಅಪ್ಪಾಚೆ 2.0 ಲೈಸೆನ್ಸ್ ಅಡಿಯಲ್ಲಿ ಉಚಿತವಾಗಿದೆ. ಆದರೆ ನೀವು ಬೇರೆ ಹಲವು ಮಾದರಿಗಳನ್ನು ಕೂಡ ಸಂಯೋಜಿಸಬಹುದು. ಕೆಲವು ಸಲಹೆಗಳು ಇಲ್ಲಿ ಇವೆ:

| ಮಾದರಿ | Hugging Face ಐಡಿ | ಪರಿಮಾಣಗಳು | ಲೈಸೆನ್ಸ್ | ಟಿಪ್ಪಣಿಗಳು |
|-------|-----------------|------------|---------|-------------|
| Qwen3-0.6B | `Qwen/Qwen3-0.6B` | 0.6B | ಅಪಾಚೆ 2.0 | ಅತ್ಯಂತ ಸಣ್ಣದು, ಚಾಲನೆ ಶೀಘ್ರ, ಪರೀಕ್ಷೆಗೆ ಉತ್ತಮ |
| Qwen3-1.7B | `Qwen/Qwen3-1.7B` | 1.7B | ಅಪಾಚೆ 2.0 | ಉತ್ತಮ ಗುಣಮಟ್ಟ, ಇನ್ನೂ ವೇಗವಾಗಿ ಸಂಯೋಜಿಸುವುದು |
| Qwen3-4B | `Qwen/Qwen3-4B` | 4B | ಅಪಾಚೆ 2.0 | ಬಲಿಷ್ಠ ಗುಣಮಟ್ಟ, ಹೆಚ್ಚು RAM ಬೇಕು |
| Llama 3.2 1B ಇನ್ಸ್ಟ್ರಕ್ಟ್ | `meta-llama/Llama-3.2-1B-Instruct` | 1B | Llama 3.2 | Hugging Face ನಿಯಮಾನುಸರಿಸಿ ಲೈಸೆನ್ಸ್ ಸ್ವೀಕಾರ ಅಗತ್ಯವಿದೆ |
| Mistral 7B ಇನ್ಸ್ಟ್ರಕ್ಟ್ | `mistralai/Mistral-7B-Instruct-v0.3` | 7B | ಅಪಾಚೆ 2.0 | ಉನ್ನತ ಗುಣಮಟ್ಟ, ದೊಡ್ಡ ಡೌನ್‌ಲೋಡ್ ಮತ್ತು ಹೆಚ್ಚು ಸಮಯದ ಸಂಯೋಜನೆ |
| Phi-3 ಮಿನಿ | `microsoft/Phi-3-mini-4k-instruct` | 3.8B | MIT | Foundry Local ಕ್ಯಾಟಲಾಗ್‌ನಲ್ಲಿ ಈಗಾಗಲೇ ಇದೆ (ತೂಕದ ಹೋಲಿಕೆಗೆ ಉಪಯುಕ್ತ) |

> **ಲೈಸೆನ್ಸ್ ಸ್ಮರಣಿಕೆ:** ಬಳಸುವುದಕ್ಕಿಂತ ಮುಂಚೆ ಎಲ್ಲಾದರೂ Hugging Face ನಲ್ಲಿ ಮಾದರಿಯ ಲೈಸೆನ್ಸ್ ಅನ್ನು ಪರಿಶೀಲಿಸಿ. ಕೆಲವು ಮಾದರಿಗಳು (Llama ಮುಂತಾದವು) ಲೈಸೆನ್ಸ್ ಒಪ್ಪಿಕೊಳ್ಳುವ ಮತ್ತು `huggingface-cli login` ಮೂಲಕ ಪ್ರಮಾಣೀಕರಿಸುವ ಅಗತ್ಯವಿದೆ.

---

## ಕಲ್ಪನೆಗಳು: ಯಾವಾಗ ಕಸ್ಟಮ್ ಮಾದರಿಗಳನ್ನು ಬಳಸಬೇಕು

| ಸಂದರ್ಭ | ನೀವು ಏಕೆ ನಿಮ್ಮದೇ ಸೃಷ್ಟಿ ಮಾಡಬೇಕು? |
|----------|-------------------------|
| **ನಿಮ್ಮ ಬೇಕಾದ ಮಾದರಿ ಕ್ಯಾಟ್ಲಾಗ್‌ನಲ್ಲಿ ಇಲ್ಲ** | Foundry Local ಕ್ಯಾಟ್ಲಾಗ್ ನಿರ್ದಿಷ್ಟೃಪಿಸಿದರು. ನಿಮಗೆ ಬೇಕಾದ ಮಾದರಿ ಇದ್ದರೆ ಅದು ಲיסט್ ಮಾಡದಿದ್ದರೆ ನೀವು ಸ್ವತಃ ಸಂಯೋಜಿಸಿ. |
| **ಫೈನ್-ಟ್ಯೂನ್ ಮಾಡಿದ ಮಾದರಿಗಳು** | ನೀವು ವೈಯಕ್ತಿಕ ಮಾಹಿತಿ ಆಧಾರಿತ ಹಾಗು ಫೈನ್-ಟ್ಯೂನ್ ಮಾಡಿದ ಮಾದರಿಗಳಿದ್ದರೆ ನಿಮ್ಮದೇ ತೂಕಗಳನ್ನು ಸಂಯೋಜಿಸಬೇಕು. |
| **ನಿರ್ದಿಷ್ಟ ಪ್ರಮಾಣೀಕರಣ ಅಗತ್ಯಗಳು** | ನೀವು ಕ್ಯಾಟ್ಲಾಗ್‌ಗಿಂತ ಬೇರೆಯಾದ ನಿರ್ದಿಷ್ಟ ಪ್ರಮಾಣೀಕರಣ ಅಥವಾ ನಿಖರತೆ ನಿರ್ವಹಣೆಯನ್ನು ಬೇಕಾಗಬಹುದು. |
| **ಹೊಸ ಮಾದರಿ ಬಿಡುಗಡೆಗಳು** | Hugging Face ನಲ್ಲಿ ಹೊಸ ಮಾದರಿ ಬಿಡುಗಡೆಯಾಗುತ್ತಿದ್ದಾಗ, ಅದು ಇನ್ನೂ Foundry Local ಕಟ್‌ಲಾಗ್‌ನಲ್ಲಿ ಇಲ್ಲದಿರಬಹುದು. ಸ್ವತಃ ಸಂಯೋಜಿಸುವ ಮೂಲಕ ತ್ವರಿತ ಪ್ರವೇಶ ದೊರೆಯುತ್ತದೆ. |
| **ಸಂಶೋಧನೆ ಮತ್ತು ಪ್ರಯೋಗ** | ಉತ್ಪಾದನಾ ಆಯ್ಕೆಗೆ ಹೋಗುವ ಮೊದಲು ವಿವಿಧ ಮಾದರಿ ವಿನ್ಯಾಸಗಳು, ಗಾತ್ರಗಳು ಅಥವಾ ಸಂರಚನೆಗಳನ್ನು ಸ್ಥಳೀಯ ಪ್ರಯೋಗ ಮಾಡುವುದಕ್ಕೆ. |

---

## ಸಾರಾಂಶ

ಈ ಲ್ಯಾಬ್‌ನಲ್ಲಿ ನೀವು ಕಲಿತದ್ದು:

| ಹಂತ | ನೀವು ಮಾಡಿದದ್ದು |
|------|---------------|
| 1 | ONNX Runtime GenAI ಮಾದರಿ ನಿರ್ಮಾಪಕವನ್ನು ಇನ್‌ಸ್ಟಾಲ್ ಮಾಡಿದರು |
| 2 | Hugging Face ನಿಂದ `Qwen/Qwen3-0.6B` ಅನ್ನು ಸಂಯೋಜಿಸಿ ಪರಿಪೂರ್ಣ ONNX ಮಾದರಿಯಾಗಿ மாற்றಿದರು |
| 3 | `inference_model.json` ಚಾಟ್-ಟೆಂಪ್ಲೇಟು ಸಂರಚನಾ ಫೈಲನ್ನು ಸೃಷ್ಟಿಸಿದರು |
| 4 | ಸಂಯೋಜಿಸಿದ ಮಾದರಿಯನ್ನು Foundry Local ಕ್ಯಾಸೆಗೆ ಸೇರಿಸಿದ್ದರು |
| 5 | CLI ಮೂಲಕ ಕಸ್ಟಮ್ ಮಾದರಿಯೊಂದಿಗೆ ಸಂವಾದ ಚಾಟ್ ನಡೆಸಿದರು |
| 6 | OpenAI-ಅನುಕೂಲಿತ REST API ಮೂಲಕ ಮಾದರಿಯನ್ನು ಪ್ರಶ್ನಿಸಿದರು |
| 7 | Python, JavaScript, C# ಮೂಲಕ OpenAI SDK ಮೂಲಕ ಸಂಪರ್ಕಿಸಿದರು |
| 8 | Foundry Local SDK ಮೂಲಕ ಕಸ್ಟಮ್ ಮಾದರಿಯನ್ನು ಮುಗಿಯುವವರೆಗೆ ಪರೀಕ್ಷಿಸಿದರು |

ಪ್ರಮುಖ ವಿಷಯ: **ಯಾವುದೇ ಟ್ರಾನ್ಸ್‌ಫಾರ್ಮರ್ ಆಧಾರಿತ ಮಾದರಿಯನ್ನು Foundry Local ನಲ್ಲಿ ONNX ರೂಪಾಂತರಗೊಂಡ ನಂತರ ನಡೆಯಿಸಬಹುದು**. OpenAI ಅನುಕೂಲಿತ API ಅರ್ಥವಾಯ್ತು ನಿಮಗೆ ಈಗಿರುವ ಅಪ್ಲಿಕೇಶನ್ ಕೋಡ್ ಬದಲಾಯಿಸದೆ ನಿಮ್ಮ ಕಸ್ಟಮ್ ಮಾದರಿಗೆ ಬಳಸಬಹುದು; ಕೇವಲ `model` ಹೆಸರು ಬದಲಾಯಿಸುವುದು ಸಾಕು.

---

## ಮುಖ್ಯ ಅಂಶಗಳು

| ಕಲ್ಪನೆ | ವಿವರ |
|---------|--------|
| ONNX Runtime GenAI Model Builder | Hugging Face ಮಾದರಿಗಳನ್ನು ONNX ಫಾರ್ಮಾಟ್‌ಗೆ ಒಪ್ಪಿಸುವುದು ಒಟ್ಟು ಒಂದೇ ಆಜ್ಞೆಯಲ್ಲಿ ಪ್ರಮಾಣೀಕರಣದೊಂದಿಗೆ |
| ONNX ಫಾರ್ಮಾಟ್ | Foundry Local ಗೆ ONNX ಮಾದರಿಗಳು ಮತ್ತು ONNX Runtime GenAI ಸಂರಚನೆಯ ಅಗತ್ಯವಿದೆ |
| ಚಾಟ್ ಟೆಂಪ್ಲೇಟುಗಳು | `inference_model.json` ಫೈಲ್ Foundry Local ಗೆ ಪ್ರಾಂಪ್ಟ್ ಶೈಲಿಯನ್ನು ಹೇಳುತ್ತದೆ |
| ಹಾರ್ಡ್‌ವೇರ್ ಗುರಿ | CPU, NVIDIA GPU (CUDA), DirectML (Windows GPU), ಅಥವಾ WebGPU ಪ್ರಕಾರ ಸಂಯೋಜನೆ ಮಾಡಿ |
| ಪ್ರಮಾಣೀಕರಣ | ಕಡಿಮೆ ನಿಖರತೆ (int4) ಗಾತ್ರವನ್ನು ಕಡಿಮೆ ಮಾಡುತ್ತದೆ ಮತ್ತು ವೇಗವನ್ನು ಹೆಚ್ಚಿಸುತ್ತದೆ, fp16 ಹೆಚ್ಚುವರಿ ಗುಣಮಟ್ಟವನ್ನು ಹೊಂದಿದೆ |
| API ಹೊಂದಿಕತೆ | ಕಸ್ಟಮ್ ಮಾದರಿಗಳು ನಿರ್ಮಿತ ಮಾದರಿಗಳಂತೆ ಇಡಿಯ OpenAI ನಅನುಕೂಲಿತ API ಅನ್ನು ಬಳಸುತ್ತವೆ |
| Foundry Local SDK | SDK ಸೇವೆ ಪ್ರಾರಂಭಿಸುವಿಕೆ, ಎಂಡ್‌ಪಾಯಿಂಟ್ ಗುರುತಿಸುವಿಕೆ, ಮತ್ತು ಮಾದರಿ ಲೋಡಿಂಗ್ ಅನ್ನು ಸ್ವಯಂಚಾಲಿತವಾಗಿ ನಿರ್ವಹಿಸುತ್ತದೆ |

---

## ಮುಂದಿನ ಓದು

| ಸಂಪನ್ಮೂಲ | ಲಿಂಕ್ |
|----------|-------|
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |
| Foundry Local ಕಸ್ಟಮ್ ಮಾದರಿ ಮಾರ್ಗದರ್ಶಿ | [Compile Models for Foundry Local](https://github.com/microsoft/Foundry-Local/blob/main/docs/how-to/compile-models-for-foundry-local.md) |
| Qwen3 ಮಾದರಿ ಕುಟುಂಬ | [huggingface.co/Qwen](https://huggingface.co/Qwen) |
| Olive ಡಾಕ್ಯುಮೆಂಟೇಶನ್ | [microsoft.github.io/Olive](https://microsoft.github.io/Olive) |

---

## ಮುಂದಿನ ಹಂತಗಳು

ನಿಮ್ಮ ಸ್ಥಳೀಯ ಮಾದರಿಗಳಿಂದ ಹೊರಗಿನ ಕಾರ್ಯಗಳನ್ನು ಕರೆಸಲು ಹೇಗೆ ಸಕ್ರಿಯಗೊಳಿಸುವುದನ್ನು ಕಲಿಯಲು [Part 11: Tool Calling with Local Models](part11-tool-calling.md) ಬಳಸಿ ಮುಂದುವರಿಯಿರಿ.

[← Part 9: Whisper Voice Transcription](part9-whisper-voice-transcription.md) | [Part 11: Tool Calling →](part11-tool-calling.md)