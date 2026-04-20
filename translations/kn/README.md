<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="ಫೌಂಡ್ರಿ ಲೋಕಲ್" width="280" />
</p>

# ಫೌಂಡ್ರಿ ಲೋಕಲ್ ಕಾರ್ಯಾಗಾರ - ಡಿವೈಸ್ ಮೇಲೆ ಎಐ ಆ್ಯಪ್ಸ್ ನಿರ್ಮಿಸಿ

ನಿಮ್ಮ ಸ್ವಂತ ಯಂತ್ರದಲ್ಲಿ ಭಾಷಾ ಮಾದರಿಗಳನ್ನು ಚಲಾಯಿಸುವುದು ಮತ್ತು [Foundry Local](https://foundrylocal.ai) ಮತ್ತು [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) ಬಳಸಿ ಬುದ್ಧಿವಂತ ಆ್ಯಪ್ಲಿಕೇಶನ್‌ಗಳನ್ನು ರಚಿಸುವ ಕಾರ್ಯನಿರ್ವಹಣಾ ಕಾರ್ಯಾಗಾರ.

> **ಫೌಂಡ್ರಿ ಲೋಕಲ್ ಏನು?** ಫೌಂಡ್ರಿ ಲೋಕಲ್ ಎಂಬುದು ನಿಮ್ಮ ಹಾರ್ಡ್‌ವೇರ್‌ನಲ್ಲಿ ಸಂಪೂರ್ಣವಾಗಿ ಭಾಷಾ ಮಾದರಿಗಳನ್ನು ಡೌನ್ಲೋಡ್ ಮಾಡಿ, ನಿರ್ವಹಿಸಿ, ಸೇವೆ ನೀಡಲು ಅವಕಾಶದ ತೂಕ ಕಡಿಮೆ ಇರುವ ರನ್‌ಟೈಂ. ಇದು **OpenAI-ಸಂಯೋಜಿತ API** ಅನ್ನು ಒದಗಿಸುತ್ತದೆ ಆದ್ದರಿಂದ OpenAI ಮಾತನಾಡುವ ಯಾವುದೇ ಟೂಲ್ ಅಥವಾ SDK ಸಂಪರ್ಕಿಸಬಹುದು - ಯಾವುದೇ ಕ್ಲೌಡ್ ಖಾತೆ ಅಗತ್ಯವಿಲ್ಲ.

---

## ಕಲಿಕೆಯ ಗುರಿಗಳು

ಈ ಕಾರ್ಯಾಗಾರದ ಕೊನೆಯಲ್ಲಿ ನೀವು ಸಾಮರ್ಥ್ಯ ಹೊಂದಿರುತ್ತೀರಿ:

| # | ಗುರಿ |
|---|-----------|
| 1 | ಫೌಂಡ್ರಿ ಲೋಕಲ್ ಅನ್ನು ಸ್ಥಾಪಿಸಿ ಮತ್ತು CLI ಮೂಲಕ ಮಾದರಿಗಳನ್ನು ನಿರ್ವಹಿಸಿ |
| 2 | ಪ್ರೋಗ್ರಾಮ್ಯಾಟಿಕ್ ಮಾದರಿ ನಿರ್ವಹಣೆಗೆ ಫೌಂಡ್ರಿ ಲೋಕಲ್ SDK API ನಲ್ಲಿ ಪರಿಣತಿ ಹೊಂದಿ |
| 3 | Python, JavaScript, ಮತ್ತು C# SDK ಗಳನ್ನು ಬಳಸಿ ಲೋಕಲ್ ಇನ್ಫರೆನ್ಸ್ ಸರ್ವರ್ ಗೆ ಸಂಪರ್ಕಿಸು |
| 4 | ನಿಮ್ಮದೇ ಡೇಟಾದಲ್ಲಿ ಉತ್ತರಗಳನ್ನು ನೆಲೆಗೊಳಿಸುವ Retrieval-Augmented Generation (RAG) ಪೈಪ್‌ಲೈನ್ ರಚಿಸಿ |
| 5 | ಸ್ಥಿರ ನಿರ್ದೇಶನಗಳು ಮತ್ತು ವ್ಯಕ್ತಿತ್ವಗಳೊಂದಿಗೆ AI ಏಜೆಂಟ್‌ಗಳನ್ನು ರಚಿಸಿ |
| 6 | ಪ್ರತಿಕ್ರಿಯೆ ಲೂಪ್‌ಗಳೊಂದಿಗೆ ಬಹು ಏಜೆಂಟ್ ವರ್ಕ್‌ಫ್ಲೋಗಳನ್ನು ವ್ಯವಸ್ಥೆಮಾಡಿ |
| 7 | ಉತ್ಪಾದನಾ 캡್‌ಸ್ಟೋನ್ ಆ್ಯಪ್ ಅನ್ನು ಅನ್ವೇಷಿಸಿ - ಜಾವಾ ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್ |
| 8 | ಗೋಲ್ಡನ್ ಡೇಟಾಸೆಟ್‌ಗಳೊಂದಿಗೆ ಮತ್ತು LLM-ಜೆಜೆಯುಡ್ ಸ್ಕೋರಿಂಗ್ ಜೊತೆಗೆ ಮೌಲ್ಯಮಾಪನ ಫ್ರೇಂವರ್ಕ್‌ಗಳನ್ನು ನಿರ್ಮಿಸಿ |
| 9 | ಫೌಂಡ್ರಿ ಲೋಕಲ್ SDK ಬಳಸಿ ಡಿವೈಸ್‌ನಲ್ಲಿ ಸ್ಪೀಚ್-ಟುಟೆಕ್ಸ್ಟ್ ವೈಶ್ಪರ್ ಉಪಯೋಗಿಸಿ ಧ್ವನಿ ಲಿಖಿತ ರೂಪಾಂತರಣೆಯನ್ನಾಗಿಸು |
| 10 | ONNX ರನ್‌ಟೈಮ್ GenAI ಮತ್ತು ಫೌಂಡ್ರಿ ಲೋಕಲ್ ಬಳಸಿ ಕಸ್ಟಮ್ ಅಥವಾ Hugging Face ಮಾದರಿಗಳನ್ನು ಸಂकलಿಸಿ ಮತ್ತು ರನ್ ಮಾಡಿ |
| 11 | ಲೂಕಲ್ ಮಾದರಿಗಳಿಗೆ ಟೂಲ್-ಕಾಲಿಂಗ್ ಮಾದರಿಯನ್ನು ಬಳಸಿ ಬಾಹ್ಯ ಕಾರ್ಯಗಳನ್ನು ಕರೆ ಮಾಡಲು ಅನುಮತಿಸಿ |
| 12 | ರಿಯಲ್-ಟೈಮ್ ಸ್ಟ್ರೀಮಿಂಗ್‌ನೊಂದಿಗೆ ಜಾವಾ ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್‌ಗಾಗಿ ಬ್ರೌಸರ್ ಆಧಾರಿತ UI ನಿರ್ಮಿಸಿ |

---

## ಅಗತ್ಯತೆಗಳು

| ಅಗತ್ಯ | ವಿವರಗಳು |
|-------------|---------|
| **Hardware** | ಕನಿಷ್ಟ 8 GB RAM (16 GB ಶಿಫಾರಸು); AVX2-ಸಮರ್ಥ CPU ಅಥವಾ ಬೆಂಬಲಿತ GPU |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025, ಅಥವಾ macOS 13+ |
| **Foundry Local CLI** | ವಿಂಡೋಸ್‌ಗೆ `winget install Microsoft.FoundryLocal` ಅಥವಾ macOS ಗೆ `brew tap microsoft/foundrylocal && brew install foundrylocal`. ವಿವರಗಳಿಗೆ [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) ನೋಡಿ. |
| **Language runtime** | **Python 3.9+** ಮತ್ತು/ಅಥವಾ **.NET 9.0+** ಮತ್ತು/ಅಥವಾ **Node.js 18+** |
| **Git** | ಈ ರೆಪೊ ಸಂಸ್ಥೆಯನ್ನು ಕ್ಲೋನ್ ಮಾಡಲು |

---

## ಪ್ರಾರಂಭಿಸುವುದು

```bash
# 1. ರೆಪೊಸಿಟರಿಯನ್ನು ಕ್ಲೋನ್ ಮಾಡಿ
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. ಫೌಂಡ્રી ಲೋಕಲ್ ಸ್ಥಾಪಿಸಿದ್ದೇನೆ ಎಂದು ಪರಿಶೀಲಿಸಿ
foundry model list              # ಲಭ್ಯವಿರುವ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡಿ
foundry model run phi-3.5-mini  # ಸಂವಾದಾತ್ಮಕ ಚಾಟ್ ಪ್ರಾರಂಭಿಸಿ

# 3. ನಿಮ್ಮ ಭಾಷಾ ಪಥವನ್ನು ಆಯ್ಕೆಮಾಡಿ (ಪೂರ್ಣ ಸ್ಥಾಪನೆಗಾಗಿ ಭಾಗ 2 ಪ್ರಯೋಗಾಲಯ ನೋಡಿ)
```

| ಭಾಷೆ | ದ್ರುತ ಪ್ರಾರಂಭ |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ಕಾರ್ಯಾಗಾರದ ಭಾಗಗಳು

### ಭಾಗ 1: ಫೌಂಡ್ರಿ ಲೋಕಲ್‌ನೊಂದಿಗೆ ಪ್ರಾರಂಭಿಸುವುದು

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- ಫೌಂಡ್ರಿ ಲೋಕಲ್ ಎಂದರೆ ಏನು ಮತ್ತು ಅದು ಹೇಗೆ ಕೆಲಸ ಮಾಡುತ್ತದೆ
- ವಿಂಡೋಸ್ ಮತ್ತು ಮ್ಯಾಕ್‌ಒಎಸ್‌ನಲ್ಲಿ CLI ಇನ್‌ಸ್ಟಾಲ್ ಮಾಡುವುದು
- ಮಾದರಿಗಳನ್ನು ಅನ್ವೇಷಣೆ - ಪಟ್ಟಿ, ಡೌನ್ಲೋಡ್, ಚಲಾಯಿಸುವುದು
- ಮಾದರಿ ಮೂಲನಾಮಗಳು ಮತ್ತು ಡೈನಾಮಿಕ್ ಪೋರ್ಟ್‌ಗಳನ್ನು ಅರ್ಥಮಾಡಿಕೊಳ್ಳುವುದು

---

### ಭಾಗ 2: ಫೌಂಡ್ರಿ ಲೋಕಲ್ SDK ಆಳವಾದ ಅಧ್ಯಯನ

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- ಅಪ್ಲಿಕೇಶನ್ ಅಭಿವೃದ್ಧಿಗಾಗಿ CLI ಬದಲು SDK ಯನ್ನೇ ಬಳಸಬೇಕಾಗಿರುವುದು ಯಾಕೆ
- Python, JavaScript, ಮತ್ತು C# ಗಾಗಿ ಸಂಪೂರ್ಣ SDK API ಉಲ್ಲೇಖ
- ಸೇವೆ ನಿರ್ವಹಣೆ, ಕ್ಯಾಟಲಾಗ್ ಬ್ರೌಸಿಂಗ್, ಮಾದರಿ ಜೀವನಚರಿತ್ರೆ (ಡೌನ್ಲೋಡ್, ಲೋಡ್, ಅನ್ಲೋಡ್)
- ದ್ರುತ ಪ್ರಾರಂಭ ಮಾದರಿಗಳು: Python ಕನ್ಸ್ಟ್ರಕ್ಟರ್ ಬೂಟ್‌ಸ್ಟ್ರಾಪ್, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` ಮೆಟಾಡೇಟಾ, ಮೂಲನಾಮಗಳು ಮತ್ತು ಹಾರ್ಡ್‌ವೇರ್-ಉತ್ತಮ ಮಾದರಿ ಆಯ್ಕೆ

---

### ಭಾಗ 3: SDK ಗಳು ಮತ್ತು API ಗಳು

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript, ಮತ್ತು C# ರಿಂದ ಫೌಂಡ್ರಿ ಲೋಕಲ್ ಗೆ ಸಂಪರ್ಕಿಸುವುದು
- ಫೌಂಡ್ರಿ ಲೋಕಲ್ SDK ಬಳಸಿ ಸೇವೆಯನ್ನು ಪ್ರೋಗ್ರಾಮ್ಯಾಟಿಕ್ ನಿರ್ವಹಣೆ
- OpenAI-ಸಂಯೋಜಿತ API ಮೂಲಕ ಸ್ಟ್ರೀಮಿಂಗ್ ಚಾಟ್ ಪೂರ್ಣತೆ
- ಪ್ರತಿ ಭಾಷೆಗೆ SDK ವಿಧಾನ ಉಲ್ಲೇಖ

**ಕೋಡ್ ಉದಾಹರಣೆಗಳು:**

| ಭಾಷೆ | ಕಡತ | ವಿವರಣೆ |
|----------|------|-------------|
| Python | `python/foundry-local.py` | ಮೂಲಭೂತ ಸ್ಟ್ರೀಮಿಂಗ್ ಚಾಟ್ |
| C# | `csharp/BasicChat.cs` | .NET ಬಳಸಿ ಸ್ಟ್ರೀಮಿಂಗ್ ಚಾಟ್ |
| JavaScript | `javascript/foundry-local.mjs` | Node.js ಬಳಸಿ ಸ್ಟ್ರೀಮಿಂಗ್ ಚಾಟ್ |

---

### ಭಾಗ 4: Retrieval-Augmented Generation (RAG)

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG ಏನು ಮತ್ತು ಏಕೆ ಪ್ರಾಮುಖ್ಯತೆ ಹೊಂದಿದೆ
- ಮೆಮರಿ ಆಧಾರಿತ ಜ್ಞಾನ ಆಧಾರ ನಿರ್ಮಾಣ
- ಸ್ಕೋರಿಂಗ್ ಜೊತೆಗೆ ಕ್ಯೀವರ್ಡ್-ಒವರ್‌ಲ್ಯಾಪ್ ರಿಟ್ರೀವಲ್
- ನೆಲೆಗೊಂಡ ಸಿಸ್ಟಮ್ ಪ್ರೊಂಪ್ಟ್ಗಳ ಸಂಯೋಜನೆ
- ಡಿವೈಸ್ ಮೇಲೆ ಸಂಪೂರ್ಣ RAG ಪೈಪ್‌ಲೈನ್ ಚಾಲನೆ

**ಕೋಡ್ ಉದಾಹಣೆಗಳು:**

| ಭಾಷೆ | ಕಡತ |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### ಭಾಗ 5: AI ಏಜೆಂಟ್‌ಗಳ ನಿರ್ಮಾಣ

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI ಏಜೆಂಟ್ ಎಂದು ಏನು (ಸಾಧಾರಣ LLM ಕಾಲ್‌ಗೆ ವಿರುದ್ಧವಾಗಿ)
- `ChatAgent` ಮಾದರಿ ಮತ್ತು Microsoft Agent Framework
- ಸಿಸ್ಟಮ್ ಸೂಚನೆಗಳು, ವ್ಯಕ್ತಿತ್ವಗಳು ಮತ್ತು ಬಹು-ತಿರುವು ಸಂಭಾಷಣೆಗಳು
- ಏಜೆಂಟ್‌ಗಳಿಂದ ರಚಿಸಲಾದ ಗುಚ್ಛಿತ ಔಟ್ಪುಟ್ (JSON)

**ಕೋಡ್ ಉದಾಹರಣೆಗಳು:**

| ಭಾಷೆ | ಕಡತ | ವಿವರಣೆ |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework ಜೋಡಿಸಿದ್ದು ಒಂಟಿ ಏಜೆಂಟ್ |
| C# | `csharp/SingleAgent.cs` | ಒಂಟಿ ಏಜೆಂಟ್ (ChatAgent ಮಾದರಿ) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | ಒಂಟಿ ಏಜೆಂಟ್ (ChatAgent ಮಾದರಿ) |

---

### ಭಾಗ 6: ಬಹು-ಏಜೆಂಟ್ ವರ್ಕ್‌ಫ್ಲೋಗಳು

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- ಬಹು ಏಜೆಂಟ್ ಪೈಪ್‌ಲೈನ್‌ಗಳು: ಸಂಶೋಧಕ → ಲೇಖಕ → ಸಂಪಾದಕ
- ಕ್ರಮವಾಗಿ ವ್ಯವಸ್ಥೆಮಾಡುವಿಕೆ ಮತ್ತು ಪ್ರತಿಕ್ರಿಯೆ ಲೂಪ್ಗಳು
- ಹಂಚಿಕೆಯಾಗಿರುವ ಸಂರಚನೆ ಮತ್ತು ಒಪ್ಪಂದಗಳ ವಿನ್ಯಾಸ
- ನಿಮ್ಮದೇ ಬಹು ಏಜೆಂಟ್ ವರ್ಕ್‌ಫ್ಲೋ ವಿನ್ಯಾಸಗೊಳ್ಳುವುದು

**ಕೋಡ್ ಉದಾಹರಣೆಗಳು:**

| ಭಾಷೆ | ಕಡತ | ವಿವರಣೆ |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | ಮೂರು ಏಜೆಂಟ್ ಪೈಪ್‌ಲೈನ್ |
| C# | `csharp/MultiAgent.cs` | ಮೂರು ಏಜೆಂಟ್ ಪೈಪ್‌ಲೈನ್ |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | ಮೂರು ಏಜೆಂಟ್ ಪೈಪ್‌ಲೈನ್ |

---

### ಭಾಗ 7: ಜಾವಾ ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್ - ಕ್ಯಾಪ್‌ಸ್ಟೋನ್ ಆ್ಯಪ್ಲಿಕೇಶನ್

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 ವಿಶೇಷತೆಯುತ ಏಜೆಂಟ್‌ಗಳೊಂದಿಗೆ ಉದ್ಯಮಮಟ್ಟದ ಬಹು-ಏಜೆಂಟ್ ಆ್ಯಪ್
- ಮೌಲ್ಯಮಾಪಕನ ನಿರ್ಭರ ಪ್ರತಿಕ್ರಿಯೆ ಲೂಪ್ಗಳೊಂದಿಗೆ ಕ್ರಮವಾಗಿ ಪೈಪ್‌ಲೈನ್
- ಸ್ಟ್ರೀಮಿಂಗ್ ಔಟ್ಪುಟ್, ಉತ್ಪನ್ನ ಕ್ಯಾಟಲಾಗ್ ಹುಡುಕಾಟ, ರಚಿತ JSON ಹಸ್ತಾಂತರ
- Python (FastAPI), JavaScript (Node.js CLI), C# (.NET ಕನ್ಸೋಲ್) ನಲ್ಲಿ ಸಂಪೂರ್ಣ ಅನ್ವಯಿಕೆ

**ಕೋಡ್ ಉದಾಹರಣೆಗಳು:**

| ಭಾಷೆ | ಡೈರೆಕ್ಟರಿ | ವಿವರಣೆ |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | ಓರ್ಕೆಸ್ಟ್ರೇಟರ್ ಜೊತೆಗೆ FastAPI ವೆಬ್ ಸೇವೆ |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI ಆ್ಯಪ್ಲಿಕೇಶನ್ |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 ಕನ್ಸೋಲ್ ಆ್ಯಪ್ಲಿಕೇಶನ್ |

---

### ಭಾಗ 8: ಮೌಲ್ಯಮಾಪನೆ ಆಧಾರಿತ ಅಭಿವೃದ್ಧಿ

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- ಗೋಲ್ಡನ್ ಡೇಟಾಸೆಟ್ ಬಳಸಿ AI ಏಜೆಂಟ್‌ಗಳಿಗೆ ಸಕಾಲಿಕ ಮೌಲ್ಯಮಾಪನ ವ್ಯವಸ್ಥೆ ನಿರ್ಮಿಸಿ
- ನಿಯಮಾಧಾರಿತ ಪರಿಶೀಲನೆಗಳು (ದೈರ್ಜ, ಅರ್ಹತೆ, ನಿಷೇಧಿತ ಪದಗಳು) + LLM-ಜೆಜೆಯುಡ್ ಸ್ಕೋರಿಂಗ್
- ಸಂದರ್ಭ ವಿವರಣೆಗಳ ಸಮಗ್ರ ಸ್ಕೋರ್ಕಾರ್ಡ್‌ಗಳೊಂದಿಗೆ ಕಡೆ-ಕೆಡೆ ಹೋಲಿಕೆ
- ಭಾಗ 7 ನಿರ್ದಿಷ್ಟ ಜಾವಾ ಸಂಪಾದಕರ ಏಜೆಂಟ್ ಮಾದರಿಯನ್ನು ಆಫ್‌ಲೈನ್ ಪರೀಕ್ಷಾ ವ್ಯವಸ್ಥೆಗೆ ವಿಸ್ತರಿಸಲಾಗಿದೆ
- Python, JavaScript, ಮತ್ತು C# ಟ್ರ್ಯಾಕ್‌ಗಳು

**ಕೋಡ್ ಉದಾಹರಣೆಗಳು:**

| ಭಾಷೆ | ಕಡತ | ವಿವರಣೆ |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | ಮೌಲ್ಯಮಾಪನ ಫ್ರೇಂವರ್ಕ್ |
| C# | `csharp/AgentEvaluation.cs` | ಮೌಲ್ಯಮಾಪನ ಫ್ರೇಂವರ್ಕ್ |
| JavaScript | `javascript/foundry-local-eval.mjs` | ಮೌಲ್ಯಮಾಪನ ಫ್ರೇಂವರ್ಕ್ |

---

### ಭಾಗ 9: ವೈಶ್ಪರ್ ಬಳಸಿ ಧ್ವನಿ ಲಿಖಿತ ರೂಪಾಂತರ

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- OpenAI ವೈಶ್ಪರ್ ಅನ್ನು ಲೋಕಲ್‌ನಲ್ಲಿ ರನ್ ಮಾಡಿ ಸ್ಪೀಚ್-ಟು-ಟೆಕ್ಸ್ಟ್ ರೂಪಾಂತರಣೆ
- ಗೌಪ್ಯತೆ-ಪ್ರಥಮ ಧ್ವನಿ ಪ್ರೊಸೆಸಿಂಗ್ - ಧ್ವನಿ ಯಾವಾಗಲೂ ನಿಮ್ಮ ಸಾಧನದಲ್ಲಿರುತ್ತದೆ
- Python, JavaScript ಮತ್ತು C# ಟ್ರ್ಯಾಕ್‌ಗಳು, `client.audio.transcriptions.create()` (Python/JS) ಮತ್ತು `AudioClient.TranscribeAudioAsync()` (C#)
- ಕೈಗೊಳ್ಳುವ ಅಭ್ಯಾಸಕ್ಕೆ ಜಾವಾ-ಥೀಮ್‌ಡ್ ನೇರ ಮಾಹಿತಿ ಆಡಿಯೋ ಕಡತದೊಂದಿಗೆ

**ಕೋಡ್ ಉದಾಹರಣೆಗಳು:**

| ಭಾಷೆ | ಕಡತ | ವಿವರಣೆ |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | ವೈಶ್ಪರ್ ಧ್ವನಿ ರೂಪಾಂತರ |
| C# | `csharp/WhisperTranscription.cs` | ವೈಶ್ಪರ್ ಧ್ವನಿ ರೂಪಾಂತರ |
| JavaScript | `javascript/foundry-local-whisper.mjs` | ವೈಶ್ಪರ್ ಧ್ವನಿ ರೂಪಾಂತರ |

> **ಸೂಚನೆ:** ಈ ಲ್ಯಾಬ್ ಪ್ರೋಗ್ರಾಮ್ಯಾಟಿಕ್ ಡೌನ್ಲೋಡ್ ಮತ್ತು ವೈಶ್ಪರ್ ಮಾದರಿಯ ಲೋಡ್ ಗಾಗಿ **ಫೌಂಡ್ರಿ ಲೋಕಲ್ SDK** ಬಳಕೆ ಮಾಡುತ್ತದೆ, ನಂತರ ಧ್ವನಿಯನ್ನು ಲೋಕಲ್ OpenAI-ಸಂಯೋಜಿತ ಎಂಡ್‌ಪಾಯಿಂಟ್ ಗೆ ಕಳುಹಿಸುತ್ತದೆ ಧ್ವನಿ ರೂಪಾಂತರಣೆಗೆ. ವೈಶ್ಪರ್ ಮಾದರಿ (`whisper`) ಫೌಂಡ್ರಿ ಲೋಕಲ್ ಕ್ಯಾಟಲಾಗ್‌ನಲ್ಲಿ ಇದೆ ಮತ್ತು ಸಂಪೂರ್ಣವಾಗಿ ಡಿವೈಸ್‌ನಲ್ಲಿ ನಡೆಯುತ್ತದೆ - ಯಾವುದೇ ಕ್ಲೌಡ್ API ಕೀಲಿಗಳು ಅಥವಾ ಜಾಲ ಆಕ್ಸೆಸ್ಗೆ ಅಗತ್ಯವಿಲ್ಲ.

---

### ಭಾಗ 10: ಕಸ್ಟಮ್ ಅಥವಾ Hugging Face ಮಾದರಿಗಳನ್ನು ಉಪಯೋಗಿಸುವುದು

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ONNX ರನ್‌ಟೈಮ್ GenAI ಮಾದರಿ ಬಿಲ್ಡರ್ ಬಳಸಿ Hugging Face ಮಾದರಿಗಳನ್ನೂ ಆಪ್ಟಿಮೈಸ್ ಆಗಿರುವ ONNX ಫಾರ್ಮ್ಯಾಟ್ ಗೆ ಸಂಯೋಜನೆ
- ಹಾರ್ಡ್‌ವೇರ್-ನಿರ್ದಿಷ್ಟ ಸಂಯೋಜನೆ (CPU, NVIDIA GPU, DirectML, WebGPU) ಮತ್ತು ಕ್ವಾಂಟೈಸೇಶನ್ (int4, fp16, bf16)
- ಫೌಂಡ್ರಿ ಲೋಕಲ್‌ಗೆ ಚಾಟ್-ಟೆಂಪ್ಲೇಟ್ ಕಾನ್ಫಿಗರೇಶನ್ ಕಡತಗಳ ರಚನೆ
- ಸಂಯೋಜಿತ ಮಾದರಿಗಳನ್ನು ಫೌಂಡ್ರಿ ಲೋಕಲ್ ಕ್ಯಾಶ್‌ಗೆ ಸೇರಿಸುವುದು
- CLI, REST API ಮತ್ತು OpenAI SDK ಮೂಲಕ ಕಸ್ಟಮ್ ಮಾದರಿಗಳನ್ನು ರನ್ ಮಾಡುವುದು
- ಉದಾಹರಣೆ ಉಲ್ಲೇಖ: Qwen/Qwen3-0.6B ಅನ್ನು ಅಂತ್ಯದಿಂದ ಅಂತ್ಯಕ್ಕೆ ಸಂಯೋಜನೆ ಮಾಡುವುದು

---

### ಭಾಗ 11: ಲೋಕಲ್ ಮಾದರಿಗಳೊಂದಿಗೆ ಟೂಲ್ ಕಾಲಿಂಗ್

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- ಲೋಕಲ್ ಮಾದರಿಗಳಿಗೆ ಬಾಹ್ಯ ಕಾರ್ಯಗಳನ್ನು ಕರೆಮಾಡಲು ಅನುಮತಿಸಿ (ಟೂಲ್/ಕಾರ್ಯ ಕಾಲಿಂಗ್)
- OpenAI ಕಾರ್ಯ-ಕಾಲಿಂಗ್ ಫಾರ್ಮ್ಯಾಟ್ ಬಳಸಿ ಟೂಲ್ ಸ್ಕೀಮಾಗಳ ನಿರೂಪಣೆ
- ಬಹು-ತಿರುವು ಟೂಲ್-ಕಾಲಿಂಗ್ ಸಂಭಾಷಣೆಯ ಪ್ರವಾಹ ನಿರ್ವಹಣೆ
- ಸ್ಥಳೀಯವಾಗಿ ಟೂಲ್ ಕರೆಗಳನ್ನು ಕಾರ್ಯಗತಗೊಳಿಸಿ ಮತ್ತು ಫಲಿತಾಂಶಗಳನ್ನು ಮಾದರಿಗೆ ಹಿಂತಿರುಗಿಸಿ
- ಟೂಲ್ ಕಾಲಿಂಗ್ ಪರಿಸ್ಥಿತಿಗಳಿಗೆ ಸರಿಯಾದ ಮಾದರಿಯನ್ನು ಆರಿಸಿಕೊಳ್ಳಿ (Qwen 2.5, Phi-4-mini)
- ಟೂಲ್ ಕರೆಗಾಗಿ SDK ನ ಹೆಸರಿನ `ChatClient` ಅನ್ನು ಬಳಸಿ (JavaScript)

**ಕೋಡ್ ಉದಾಹರಣೆಗಳು:**

| ಭಾಷೆ | ಕಡತ | ವಿವರಣೆ |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | ಹವಾಮಾನ/ಜನಸಂಖ್ಯೆ ಟೂಲ್ಗಳೊಂದಿಗೆ ಟೂಲ್ ಕಾಲಿಂಗ್ |
| C# | `csharp/ToolCalling.cs` | .NET ಬಳಸಿ ಟೂಲ್ ಕಾಲಿಂಗ್ |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient ಬಳಸಿ ಟೂಲ್ ಕಾಲಿಂಗ್ |

---

### ಭಾಗ 12: ಜಾವಾ ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್‌ಗೆ ವೆಬ್ UI ರಚನೆ

**ಲ್ಯಾಬ್ ಗೈಡ್:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- ಜಾವಾ ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್‌ಗೆ ಬ್ರೌಸರ್ ಆಧಾರಿತ ಫ್ರಂಟ್‌ಎಂಡ್ ಸೇರಿಸಿ
- Python (FastAPI), JavaScript (Node.js HTTP) ಮತ್ತು C# (ASP.NET ಕೋರ್) ಮೂಲಕ ಹಂಚಿಕೊಂಡ UI ಸೇವೆ
- ಬ್ರೌಸರ್‌ನಲ್ಲಿ Fetch API ಮತ್ತು ReadableStream ಉಪಯೋಗಿಸಿ ಸ್ಟ್ರೀಮಿಂಗ್ NDJSON ಸೇವಿಸು
- ಲೈವ್ ಏಜೆಂಟ್ ಸ್ಥಿತಿಬಿಂದುಗಳು ಮತ್ತು ರಿಯಲ್-ಟೈಮ್ ಲೇಖನ ಪಠ್ಯ ಸ್ಟ್ರೀಮಿಂಗ್

**ಕೋಡ್ (ಹಂಚಿಕೊಂಡ UI):**

| ಕಡತ | ವಿವರಣೆ |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | ಪುಟ ವಿನ್ಯಾಸ |
| `zava-creative-writer-local/ui/style.css` | ಶೈಲಿ |
| `zava-creative-writer-local/ui/app.js` | ಸ್ಟ್ರೀಮ್ ಓದುಗ ಮತ್ತು DOM ನವೀಕರಣ ತರ್ಕ |

**ಬ್ಯಾಕೆಂಡ್ ಹೆಚ್ಚುವರಿ:**

| ಭಾಷೆ | ಕಡತ | ವಿವರಣೆ |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | ಸ್ಥಿರ UI ಸೇವೆಗೆ ಪರಿಷ್ಕೃತ |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ಓರ್ಕೆಸ್ಟ್ರೇಟರ್ ಅನ್ನು ಮುಚ್ಚುವ ಹೊಸ HTTP ಸರ್ವರ್ |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | ಹೊಸ ASP.NET ಕೋರ್ ಕನಿಷ್ಠ API ಪ್ರಾಜೆಕ್ಟ್ |

---

### ಭಾಗ 13: ಕಾರ್ಯಾಗಾರ ಸಂಪೂರ್ಣವಾಗಿದೆ
**ಲಾಗ್ ಮಾರ್ಗದರ್ಶಿ:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- ನೀವು ನಿರ್ಮಿಸಿರುವ ಎಲ್ಲಾ 12 ಭಾಗಗಳ ಸಾರಾಂಶ
- ನಿಮ್ಮ ಅಪ್ಲಿಕೇಶನ್‌ಗಳನ್ನು ವಿಸ್ತರಿಸಲು ಮುಂದಿನ ಆಲೋಚನೆಗಳು
- ಸಂಪನ್ಮೂಲಗಳು ಮತ್ತು ಡಾಕ್ಯುಮೆಂಟೇಶನ್‌ಗೆ ಲಿಂಕ್‌ಗಳು

---

## ಯೋಜನಾ ರಚನೆ

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## ಸಂಪನ್ಮೂಲಗಳು

| ಸಂಪನ್ಮೂಲ | ಲಿಂಕ್ |
|----------|------|
| ಫೌಂಡ್ರಿ ಲೋಕಲ್ ವೆಬ್ಸೈಟ್ | [foundrylocal.ai](https://foundrylocal.ai) |
| ಮಾದರಿ ಕ್ಯಾಟಲಾಗ್ | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| ಫೌಂಡ್ರಿ ಲೋಕಲ್ GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| ಪ್ರಾರಂಭಿಕ ಮಾರ್ಗದರ್ಶಿ | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| ಫೌಂಡ್ರಿ ಲೋಕಲ್ SDK ದಾಖಲಾತಿ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| ಮೈಕ್ರೋಸಾಫ್ಟ್ ಏಜೆಂಟ್ ಫ್ರೇಮ್ವರ್ಕ್ | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX ರನ್ಟೈಮ್ GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## ಪರವಾನಗಿ

ಈ ಕಾರ್ಯಾಗಾರ ವಸ್ತು ಶೈಕ್ಷಣಿಕ ಉದ್ದೇಶಗಳಿಗಾಗಿ ಒದಗಿಸಲಾಗಿದೆ.

---

**ಸಂತೋಷಪೂರ್ವಕ ನಿರ್ಮಾಣ! 🚀**