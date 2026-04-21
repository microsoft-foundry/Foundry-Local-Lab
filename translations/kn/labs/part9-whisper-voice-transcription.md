![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ಭಾಗ 9: וויס್ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ವಿತ್ ವಿಜಯ್ ಮತ್ತು Foundry Local

> **ಗೋಲ್:** Foundry Local ಮೂಲಕ ಸ್ಥಳೀಯವಾಗಿ ಓಪನ್‌ಎಐ ವಿಜಯ್ ಮಾದರಿಯನ್ನು ಬಳಸಿಕೊಂಡು ಆಡಿಯೋ ಫೈಲ್‌ಗಳನ್ನು ಸಂಪೂರ್ಣ ಡಿವೈಸ್‌ನಲ್ಲಿ ಟ್ರಾನ್ಸ್ಕ್ರೈಬ್ ಮಾಡುವುದು, ಯಾವುದೇ ಕ್ಲೌಡ್ ಅಗತ್ಯವಿಲ್ಲ.

## ಅವಲೋಕನ

Foundry Local ಪಠ್ಯ ಉತ್ಪಾದನೆಗೆ ಮಾತ್ರವಲ್ಲ; ಇದು **ಸ್ಪೀಚ್-ಟು-ಟೆಕ್ಸ್ಟ್** ಮಾದರಿಗಳಿಗೂ ಬೆಂಬಲ ನೀಡುತ್ತದೆ. ಈ ಪ್ರಯೋಗಶಾಲೆಯಲ್ಲಿ ನೀವು **OpenAI Whisper Medium** ಮಾದರಿಯನ್ನು ಬಳಸಿ ನಿಮ್ಮ ಯಂತ್ರದಲ್ಲಿ ಸಂಪೂರ್ಣವಾಗಿ ಆಡಿಯೋ ಫೈಲ್‌ಗಳನ್ನು ಟ್ರಾನ್ಸ್ಕ್ರೈಬ್ ಮಾಡುತ್ತೀರಿ. ಇದು Zava ಗ್ರಾಹಕ ಸೇವಾ ಕರೆಗಳು, ಉತ್ಪನ್ನ ವಿಮರ್ಶೆ ಧ್ವನಿ ದಾಖಲೆಗಳು, ಅಥವಾ ವರ್ಕ್‌ಶಾಪ್ ಯೋಜನೆ ಸೆಷನ್‌ಗಳು ಮುಂತಾದ ಸನ್ನಿವೇಶಗಳಿಗೆ ಸೂಕ್ತವಾಗಿದೆ, ಇವುಗಳಲ್ಲಿ ಆಡಿಯೋ ಡೇಟಾ ನಿಮ್ಮ ಸಾಧನದ ಹೊರ ಹೋಗಬಾರದು.

---

## ಕಲಿಕೆ ಉದ್ದೇಶಗಳು

ಈ ಪ್ರಯೋಗಶಾಲೆಯ ಅಂತ್ಯದಲ್ಲಿ ನೀವು ಶಕ್ತರಾಗಿರುತ್ತೀರಿ:

- ವಿಜಯ್ ಸ್ಪೀಚ್-ಟು-ಟೆಕ್ಸ್ಟ್ ಮಾದರಿಯನ್ನು ಮತ್ತು ಅದರ ಸಾಮರ್ಥ್ಯಗಳನ್ನು ಅರ್ಥಮಾಡಿಕೊಳ್ಳುವುದು
- Foundry Local ಬಳಸಿ ವಿಜಯ್ ಮಾದರಿಯನ್ನು ಹೊಳೆಸಿ ನಡಿಸುವುದು
- Foundry Local SDK ಬಳಸಿ ಪೈಥನ್, ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮತ್ತು C# ನಲ್ಲಿ ಆಡಿಯೋ ಫೈಲ್‌ಗಳನ್ನು ಟ್ರಾನ್ಸ್ಕ್ರೈಬ್ ಮಾಡುವುದು
- ಡಿವೈಸ್‌ನಲ್ಲಿ ಸಂಪೂರ್ಣವಾಗಿ ಸಂಭವಿಸುವ ಸರಳ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಸೇವೆಯನ್ನು ರಚಿಸುವುದು
- Foundry Local ನಲ್ಲಿ ಚಾಟ್/ಪಠ್ಯ ಮಾದರಿ ಮತ್ತು ಆಡಿಯೋ ಮಾದರಿಗಳ ನಡುವಿನ ವೈರಾಗ್ಯಗಳನ್ನು ಅರ್ಥಮಾಡಿಕೊಳ್ಳುವುದು

---

## ಪೂರ್ವಾವಶ್ಯಕತೆಗಳು

| ಅಗತ್ಯ | ವಿವರಗಳು |
|-------------|---------|
| **Foundry Local CLI** | ಆವೃತ್ತಿ **0.8.101 ಅಥವಾ ಮೇಲಿನದು** (ವಿಜಯ್ ಮಾದರಿಗಳು v0.8.101 ರಿಂದ ಲಭ್ಯವಿದೆ) |
| **OS** | ವಿಜಯಂಗಳ 10/11 (x64 ಅಥವಾ ARM64) |
| **ಭಾಷಾ ರನ್‌ಟೈಮ್** | **Python 3.9+** ಮತ್ತು/ಅಥವಾ **Node.js 18+** ಮತ್ತು/ಅಥವಾ **.NET 9 SDK** ([.NET ಡೌನ್‌ಲೋಡ್](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **ಪೂರ್ಣಗೊಂಡದ್ದು** | [ಭಾಗ 1: ಪ್ರಾರಂಭಿಸುವುದು](part1-getting-started.md), [ಭಾಗ 2: Foundry Local SDK ಆಳವಾದ ಅಧ್ಯಯನ](part2-foundry-local-sdk.md), ಮತ್ತು [ಭಾಗ 3: SDKಗಳು ಮತ್ತು APIs](part3-sdk-and-apis.md) |

> **ಗಮನಿಸಿ:** ವಿಜಯ್ ಮಾದರಿಗಳು **SDK** ಮೂಲಕ ಡೌನ್‌ಲೋಡ್ ಮಾಡಬೇಕು (CLI ಅಲ್ಲ). CLI ಆಡಿಯೋ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಎಂಡ್ಫಾಯಿಂಟ್‌ಗೆ ಬೆಂಬಲ ನೀಡುವುದಿಲ್ಲ. ನಿಮ್ಮ ಆವೃತ್ತಿಯನ್ನು ಪರಿಶೀಲಿಸಿ:
> ```bash
> foundry --version
> ```

---

## ಸಂಯೋಜನೆ: ಹೇಗೆ ವಿಜಯ್ Foundry Local ಜೊತೆಗೆ ಕೆಲಸ ಮಾಡುತ್ತದೆ

OpenAI Whisper ಮಾದರಿ ವಿವಿಧ ಆಡಿಯೋಗಳ ದೊಡ್ಡ ಡೇಟಾಸೆಟ್‌ನಲ್ಲಿ ತರಬೇತಿಗೊಂಡ ಹೆಚ್ಚಿನ ಸಾಮಾನ್ಯ ಉದ್ದೇಶದ ಸ್ಪೀಚ್ ರೆಕಗ್ನಿಷನ್ ಮಾದರಿ. Foundry Local ಮೂಲಕ ನಡಿಸಲು:

- ಮಾದರಿ **ಸಂಪೂರ್ಣವಾಗಿ ನಿಮ್ಮ CPU ಮೇಲೆ ನಡಿಸುತ್ತದೆ** - GPU ಅಗತ್ಯವಿಲ್ಲ
- ಆಡಿಯೋ ನಿಮ್ಮ ಸಾಧನದ ಹೊರಗೆ ಹೋಗುವುದಿಲ್ಲ - **ಪೂರ್ಣ ಗೌಪ್ಯತೆ**
- Foundry Local SDK ಮಾದರಿ ಡೌನ್‌ಲೋಡ್ ಮತ್ತು ಕ್ಯಾಶೆ ನಿರ್ವಹಣೆಯನ್ನು ಹ್ಯಾಂಡಲ್ ಮಾಡುತ್ತದೆ
- **ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮತ್ತು C#** SDK ನಲ್ಲಿ ಒಳಗೊಂಡ `AudioClient` ಒದಗಿಸುತ್ತದೆ, ಇದು ಸಂಪೂರ್ಣ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಪ್ರಕ್ರಿಯೆಯನ್ನು ನಿರ್ವಹಿಸುತ್ತದೆ — ಯಾವುದೇ ONNX ಮೊದಲು ಸ್ಥಾಪನೆ ಅಗತ್ಯವಿಲ್ಲ
- **Python** ಮಾದರಿ ನಿರ್ವಹಣೆಗೆ SDK ಬಳಸುತ್ತದೆ ಮತ್ತು ಎನ್‌ಕೋಡರ್/ಡಿಕೋಡರ್ ONNX ಮಾದರಿಗಳೊಂದಿಗೆ ನೇರ ಇನ್ಫರೆನ್ಸ್‌ಗೆ ONNX Runtime ಅನ್ನು ಬಳಸುತ್ತದೆ

### ಪೈಪ್‌ಲೈನ್ ಹೇಗೆ ಕೆಲಸ ಮಾಡುತ್ತದೆ (ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮತ್ತು C#) — SDK AudioClient

1. **Foundry Local SDK** ವಿಜಯ್ ಮಾದರಿಯನ್ನು ಡೌನ್‌ಲೋಡ್ ಮಾಡಿ ಕ್ಯಾಶೆ ಮಾಡುತ್ತದೆ
2. `model.createAudioClient()` (JS) ಅಥವಾ `model.GetAudioClientAsync()` (C#) ಮೂಲಕ `AudioClient` ಸೃಷ್ಟಿಸಲಾಗುತ್ತದೆ
3. `audioClient.transcribe(path)` (JS) ಅಥವಾ `audioClient.TranscribeAudioAsync(path)` (C#) ಸಂಪೂರ್ಣ ಪೈಪ್‌ಲೈನ್ ಅನ್ನು ಅಂತರಂಗದಲ್ಲಿ ನಿರ್ವಹಿಸುತ್ತದೆ — ಆಡಿಯೋ ಪೂರ್ವ ಪ್ರಕ್ರಿಯೆ, ಎನ್‌ಕೋಡರ್, ಡಿಕೋಡರ್ ಮತ್ತು ಟೋಕನ್ ಡಿಕೋಡಿಂಗ್
4. `AudioClient` ನಲ್ಲಿ `settings.language` ಗುಣಲಕ್ಷಣವನ್ನು (`"en"` ಇಂಗ್ಲಿಷ್‌ಗಾಗಿ) ನಿಖರ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್‌ಗಾಗಿ ಹೊಂದಿಸಲಾಗಿದೆ

### ಪೈಪ್‌ಲೈನ್ ಹೇಗೆ ಕೆಲಸ ಮಾಡುತ್ತದೆ (Python) — ONNX Runtime

1. **Foundry Local SDK** ವಿಜಯ್ ONNX ಮಾದರಿ ಫೈಲ್‌ಗಳನ್ನು ಡೌನ್‌ಲೋಡ್ ಮಾಡಿ ಕ್ಯಾಶೆ ಮಾಡುತ್ತದೆ
2. **ಆಡಿಯೋ ಪೂರ್ವ ಪ್ರಕ್ರಿಯೆ** WAV ಆಡಿಯೋವನ್ನು ಮೆಲ್ ಸ್ಪೆಕ್ಟ್ರೋಗ್ರಾಂಗೆ மாற்றುತ್ತದೆ (80 ಮೆಲ್ ಬಿನ್‌ಗಳು x 3000 ಫ್ರೆಂಗಳು)
3. **ಎನ್‌ಕೋಡರ್** ಮೆಲ್ ಸ್ಪೆಕ್ಟ್ರೋಗ್ರಾಂವನ್ನು ಪ್ರಕ್ರಿಯೆ ಮಾಡಿ ಹಿದ್ಡನ್ ಸ್ಟೇಟ್ಸ್ ಮತ್ತು ಕ್ರಾಸ್-ಅಟೆಂಶನ್ ಕೀ/ವೆಲ್ಯೂ ಟೆನ್ಸರ್‌ಗಳನ್ನು ಉತ್ಪಾದಿಸುತ್ತದೆ
4. **ಡಿಕೋಡರ್** ಸ್ವಯಂಪ್ರೇರಿತವಾಗಿ ಓಡುತ್ತದೆ, ಒಂದು ಟೋಕನನ್ನು ಒಂದೆರಡೇ ಸಮಯದಲ್ಲಿ ರಚಿಸಿ ತದಂತ್ಯಾ ಟೆಕ್ಸ್ಟ್ ಅಂತ್ಯ ಟೋಕನನ್ನು ಉತ್ಪಾದಿಸುವವರೆಗೆ
5. **ಟೋಕನೈಸರ್** ಹಾದಿನಡೆ ಟೋಕನ್ IDಗಳನ್ನು ಓದಲು ಸುಲಭವಾಗಿರುವ ಪಠ್ಯಕ್ಕೆ ಹಿಂತಿರುಗಿಸುತ್ತದೆ

### ವಿಜಯ್ ಮಾದರಿ ವಿವಿಧತೆಗಳು

| ಜೊಕೆ | ಮಾದರಿ ಐಡಿ | ಸಾಧನ | ಗಾತ್ರ | ವಿವರಣೆ |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU ವೇಗಗೊಳಿಸಿದ (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU ಸಾಧನಗಳಿಗೆ ಶಿಫಾರಸು ಮಾಡಲಾಗಿದೆ |

> **ಗಮನಿಸಿ:** ಡೀಫಾಲ್ಟ್‌ನಲ್ಲಿ ಚಾಟ್ ಮಾದರಿಗಳನ್ನು ಪಟ್ಟಿ ಮಾಡುತ್ತಿರುವುದರಿಂದ ಭಿನ್ನ, ವಿಜಯ್ ಮಾದರಿಗಳನ್ನು `automatic-speech-recognition` ಟಾಸ್ಕ್ ಅಡಿ ವರ್ಗೀಕರಿಸಲಾಗುತ್ತದೆ. ವಿವರಗಳಿಗಾಗಿ `foundry model info whisper-medium` ಬಳಸಿ.

---

## ಪ್ರಯೋಗಶಾಲೆ ವ್ಯಾಯಾಮಗಳು

### ವ್ಯಾಯಾಮ 0 - ಮಾದರಿ ಆಡಿಯೋ ಫೈಲ್‌ಗಳನ್ನು ಪಡೆಯಿರಿ

ಈ ಪ್ರಯೋಗಶಾಲೆಯಲ್ಲಿ Zava DIY ಉತ್ಪನ್ನ ಪರಿಕಲ್ಪನೆಗಳ ಆಧಾರಿತ ಪೂರ್ವ-ನಿರ್ಮಿತ WAV ಫೈಲ್‌ಗಳಿವೆ. ಸೇರಿಸಿದ ಸ್ಕ್ರಿಪ್ಟ್‌ನಿಂದ ಅವುಗಳನ್ನು ರಚಿಸಿ:

```bash
# ಧಾರಣೆ ಮೂಲದಿಂದ - ಮೊದಲು .venv ರಚಿಸಿ ಮತ್ತು ಸಕ್ರಿಯಗೊಳಿಸಿ
python -m venv .venv

# ವಿಂಡೋಸ್ (ಪವರ್‌ಶೆಲ್):
.venv\Scripts\Activate.ps1
# ಮ್ಯಾಕ್‌ಒಎಸ್:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

ಇದು `samples/audio/` ನಲ್ಲಿ ಆರು WAV ಫೈಲ್‌ಗಳನ್ನು ರಚಿಸುತ್ತದೆ:

| ಫೈಲ್ | ಸನ್ನಿವೇಶ |
|------|----------|
| `zava-customer-inquiry.wav` | ಗ್ರಾಹಕರು **Zava ProGrip Cordless Drill** ಬಗ್ಗೆ ಕೇಳುತ್ತಿದ್ದಾರೆ |
| `zava-product-review.wav` | ಗ್ರಾಹಕರು **Zava UltraSmooth Interior Paint** ಕುರಿತು ವಿಮರ್ಶೆ ಮಾಡುತ್ತಿದ್ದಾರೆ |
| `zava-support-call.wav` | **Zava TitanLock Tool Chest** ಕುರಿತು ಬೆಂಬಲ ಕರೆ |
| `zava-project-planning.wav` | DIYಯರ್ **Zava EcoBoard Composite Decking** ಬಳಸಿ ಡೆಕ್ ಯೋಜನೆ ಮಾಡುತ್ತಿದ್ದಾರೆ |
| `zava-workshop-setup.wav` | **ಎಲ್ಲಾ ಐದು Zava ಉತ್ಪನ್ನಗಳ** ಬಳಕೆ ಮೂಲಕ ವರ್ಕ್‌ಶಾಪ್ ವ್ಯಾಖ್ಯಾನ |
| `zava-full-project-walkthrough.wav` | **ಎಲ್ಲಾ Zava ಉತ್ಪನ್ನಗಳ** ವಿನ್ಯಾಸ ಮತ್ತು ದೀರ್ಘ ಅವಧಿಯ ಕ್ಯಾರೇಜ್ ನವೀಕರಣ (~4 ನಿಮಿಷ, ದೀರ್ಘ ಆಡಿಯೋ ಪರೀಕ್ಷೆ) |

> **ಟಿಪ್:** ನಿಮ್ಮದೇ WAV/MP3/M4A ಫೈಲ್‌ಗಳನ್ನೂ ಬಳಸಬಹುದು, ಅಥವಾ ವಿಂಡೋಸ್ ವಾಯ್ಸ್ ರೆಕಾರ್ಡರ್ ಬಳಸಿ ನಿಮ್ಮ ಧ್ವನಿಯನ್ನು ದಾಖಲಿಸಬಹುದು.

---

### ವ್ಯಾಯಾಮ 1 - SDK ಬಳಸಿ ವಿಜಯ್ ಮಾದರಿಯನ್ನು ಡೌನ್‌ಲೋಡ್ ಮಾಡಿ

ಹೊಸ Foundry Local ಆವೃತ್ತಿಗಳಲ್ಲಿ ವಿಜಯ್ ಮಾದರಿಗಳೊಂದಿಗೆ CLI ಅನುಕೂಲತೆಗಳ ಕೊರತೆಯಿಂದ SDK ಬಳಸಿ ಮಾದರಿಯನ್ನು ಡೌನ್‌ಲೋಡ್ ಮತ್ತು ಲೋಡ್ ಮಾಡಿ. ನಿಮ್ಮ ಭಾಷೆಯನ್ನು ಆಯ್ಕೆ ಮಾಡಿ:

<details>
<summary><b>🐍 Python</b></summary>

**SDK ಅನ್ನು ಸ್ಥಾಪಿಸಿ:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ
manager = FoundryLocalManager()
manager.start_service()

# ಕ್ಯಾಟಲಾಗ್ ಮಾಹಿತಿಯನ್ನು ಪರಿಶೀಲಿಸಿ
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# ಈಗಾಗಲೇ ಸಂಗ್ರಹಿಸಲಾಗಿದೆ ಎಂಬುದನ್ನು ಪರಿಶೀಲಿಸಿ
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# ಮಾದರಿಯನ್ನು ಮೆಮರಿಯಲ್ಲಿ ಲೋಡ್ ಮಾಡಿ
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

`download_whisper.py` ಎಂಬ ಫೈಲ್ ಆಗಿ ಉಳಿಸಿ ಫಲಿತಾಂಶವಾಗಿ ಚಾಲನೆ ಮಾಡಿ:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK ಅನ್ನು ಸ್ಥಾಪಿಸಿ:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// ವ್ಯವಸ್ಥಾಪಕರನ್ನು ರಚಿಸಿ ಮತ್ತು ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ಕ್ಯಾಟಲಾಗ್‌ನಿಂದ ಮಾದರಿಯನ್ನು ಪಡೆಯಿರಿ
const catalog = manager.catalog;
const model = await catalog.getModel(alias);
console.log(`Model: ${model.id}`);

if (model.isCached) {
  console.log("Whisper model already downloaded.");
} else {
  console.log("Downloading Whisper model (this may take several minutes)...");
  await model.download();
  console.log("Download complete.");
}

// ಮಾದರಿಯನ್ನು ಸ್ಮರಣೆ ಇಲ್ಲಿ ಲೋಡ್ ಮಾಡಿ
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

`download-whisper.mjs` ಎಂದು ಉಳಿಸಿ ನಡಿಸಿ:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK ಅನ್ನು ಸ್ಥಾಪಿಸಿ:**
```bash
dotnet add package Microsoft.AI.Foundry.Local
```

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

var alias = "whisper-medium";

// Start the service
Console.WriteLine("Starting Foundry Local service...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "FoundryLocalSamples",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// Get model from catalog
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(alias, default);
Console.WriteLine($"Model: {model.Id}");

// Check if already cached
var isCached = await model.IsCachedAsync(default);

if (isCached)
{
    Console.WriteLine("Whisper model already downloaded.");
}
else
{
    Console.WriteLine("Downloading Whisper model (this may take several minutes)...");
    await model.DownloadAsync(null, default);
    Console.WriteLine("Download complete.");
}

// Load the model into memory
await model.LoadAsync(default);
Console.WriteLine($"Whisper model loaded: {model.Id}");
```

</details>

> **SDK ಏಕೆ CLI ಬದಲು?** Foundry Local CLI ವಿಜಯ್ ಮಾದರಿಗಳನ್ನು ನೇರವಾಗಿ ಡೌನ್‌ಲೋಡ್ ಅಥವಾ ಸರ್ವ್ ಮಾಡಲು ಬೆಂಬಲ ನೀಡುವುದಿಲ್ಲ. SDK ಮೂಲಕ ಆಡಿಯೋ ಮಾದರಿಗಳನ್ನು ಪ್ರೋಗ್ರಾಮ್ಯಾಟಿಕಾಗಿ ಡೌನ್‌ಲೋಡ್ ಮತ್ತು ನಿರ್ವಹಿಸಲು ವಿಶ್ವಾಸಾರ್ಹ ವಿಧಾನವನ್ನು ಒದಗಿಸಲಾಗುತ್ತದೆ. ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮತ್ತು C# SDKಗಳಲ್ಲಿ ಸಂಪೂರ್ಣ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಪೈಪ್‌ಲೈನ್ ನಿರ್ವಹಿಸುವ ಒಳಗೊಂಡ `AudioClient` ಇರುತ್ತದೆ. Python ನೇರ ಇನ್ಫರೆನ್ಸ್‌ಗೆ ONNX Runtime ಬಳಸುತ್ತದೆ.

---

### ವ್ಯಾಯಾಮ 2 - ವಿಜಯ್ SDK ಅನ್ನು ಅರ್ಥಮಾಡಿಕೊಳ್ಳಿ

ವಿಜಯ್ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಭಾಷೆಯ ಪ್ರಕಾರ ವಿಭಿನ್ನ ವಿಧಾನಗಳನ್ನು ಬಳಸುತ್ತದೆ. **ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮತ್ತು C#** Foundry Local SDK ನಲ್ಲಿ `AudioClient` ಒಳಗೊಂಡಿವೆ, ಇದು ಸಂಪೂರ್ಣ ಪೈಪ್‌ಲೈನ್ (ಆಡಿಯೋ ಪೂರ್ವ ಪ್ರಕ್ರಿಯೆ, ಎನ್‌ಕೋಡರ್, ಡಿಕೋಡರ್, ಟೋಕನ್ ಡಿಕೋಡಿಂಗ್) ಒಬ್ಬಲಗೆ ನಿರ್ವಹಿಸುತ್ತದೆ. **Python** Foundry Local SDK ಬಳಸುತ್ತದೆ ಮಾದರಿ ನಿರ್ವಹಣೆಗೆ ಮತ್ತು ONNX Runtime ನೇರ ಇನ್ಫರೆನ್ಸ್‌ಗೆ ಎನ್‌ಕೋಡರ್/ಡಿಕೋಡರ್ ONNX ಮಾದರಿಗಳೊಂದಿಗೆ.

| ಘಟಕ | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK ಪ್ಯಾಕೇಜ್ಗಳು** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **ಮಾದರಿ ನಿರ್ವಹಣೆ** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **ವೈಶಿಷ್ಟ್ಯ ಪ್ರತ್ಯಗೊಳಿಸುವಿಕೆ** | `WhisperFeatureExtractor` + `librosa` | SDK `AudioClient` ನಿಂದ ನಿರ್ವಹಣೆ | SDK `AudioClient` ನಿಂದ ನಿರ್ವಹಣೆ |
| **ಇನ್ಫರೆನ್ಸ್** | `ort.InferenceSession` (ಎನ್‌ಕೋಡರ್ + ಡಿಕೋಡರ್) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **ಟೋಕನ್ ಡಿಕೋಡಿಂಗ್** | `WhisperTokenizer` | SDK `AudioClient` ನಿಂದ ನಿರ್ವಹಣೆ | SDK `AudioClient` ನಿಂದ ನಿರ್ವಹಣೆ |
| **ಭಾಷಾ ಸೆಟ್ಟಿಂಗ್** | ಡಿಕೋಡರ್ ಟೋಕನ್ ಗಳಲ್ಲಿ `forced_ids` ಮೂಲಕ ಸೆಟ್ ಮಾಡಿ | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **ಇನ್ಪುಟ್** | WAV ಫೈಲ್ ಮಾರ್ಗ | WAV ಫೈಲ್ ಮಾರ್ಗ | WAV ಫೈಲ್ ಮಾರ್ಗ |
| **ಔಟ್‌ಪುಟ್** | ಡಿಕೋಡ್ ಮಾಡಿದ ಪಠ್ಯ<string> | `result.text` | `result.Text` |

> **ಮುಖ್ಯ:** ಯಾವಾಗಲೂ `AudioClient` ನಲ್ಲಿನ ಭಾಷೆಯನ್ನು ಸೆಟ್ ಮಾಡಬೇಕು (ಉದಾ: ಇಂಗ್ಲಿಷ್‌ಗಾಗಿ `"en"`). ಯಾವುದೇ ಸ್ಪಷ್ಟ ಭಾಷಾ ಸೆಟ್ಟಿಂಗ್ ಇಲ್ಲದಿರುವುದರಿಂದ ಮಾದರಿ ತಪ್ಪು ಅಥವಾ ಅರ್ಥವಿಲ್ಲದ ಔಟ್‌ಪುಟ್ ನೀಡಬಹುದು.

> **SDK ಮಾದರಿಗಳು:** Python ನಲ್ಲಿ `FoundryLocalManager(alias)` ಮೂಲಕ ಬೂಟ್‌ಸ್ಟ್ರಾಪ್ ಮಾಡಿ ನಂತರ `get_cache_location()` ಮೂಲಕ ONNX ಮಾದರಿ ಫೈಲ್‌ಗಳನ್ನು ಹುಡುಕಿ. ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮತ್ತು C# SDKಗಳ ಆಂತರಿಕ `AudioClient` ಅನ್ನು `model.createAudioClient()` (JS) ಅಥವಾ `model.GetAudioClientAsync()` (C#) ಮೂಲಕ ಪಡೆಯುತ್ತೀರಿ — ಇದು ಸಂಪೂರ್ಣ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಪೈಪ್‌ಲೈನ್ ಅನ್ನು ನಿರ್ವಹಿಸುತ್ತದೆ. ಸಂಪೂರ್ಣ ವಿವರಗಳಿಗೆ [ಭಾಗ 2: Foundry Local SDK ಆಳವಾದ ಅಧ್ಯಯನ](part2-foundry-local-sdk.md) ನೋಡಿ.

---

### ವ್ಯಾಯಾಮ 3 - ಸರಳ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಅಪ್ಲಿಕೇಶನ್ ನಿರ್ಮಿಸಿ

ನಿಮ್ಮ ಭಾಷಾ ಟ್ರ್ಯಾಕ್ ಆಯ್ಕೆಮಾಡಿ ಒಂದು ಸುಗ್ಗಳಿದ ಅಪ್ಲಿಕೇಶನ್ ನಿರ್ಮಿಸಿ, ಅದು ಒಂದು ಆಡಿಯೋ ಫೈಲ್ ಅನ್ನು ಟ್ರಾನ್ಸ್ಕ್ರೈಬ್ ಮಾಡುತ್ತದೆ.

> **ಬೆಂಬಲಿತ ಆಡಿಯೋ ಸ್ವರೂಪಗಳು:** WAV, MP3, M4A. ಉತ್ಕೃಷ್ಟ ಫಲಿತಾಂಶಕ್ಕಾಗಿ 16kHz ಸಾಂಪಲ್ ದರದ WAV ಫೈಲ್‌ಗಳನ್ನು ಬಳಸಿ.

<details>
<summary><h3>Python ಟ್ರ್ಯಾಕ್</h3></summary>

#### ಸೆಟಪ್

```bash
cd python
python -m venv venv

# ವರ್ಚುವಲ್ ಪರಿಸರವನ್ನು ಸಕ್ರಿಯಗೊಳಿಸಿ:
# ವಿಂಡೋಸ್ (ಪವರ್‌ಶೆಲ್):
venv\Scripts\Activate.ps1
# ಮ್ಯಾಕ್‌ಒಎಸ್:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಕೋಡ್

`foundry-local-whisper.py` ಎಂಬ ಫೈಲ್ ಸೃಷ್ಟಿಸಿ:

```python
import sys
import os
import numpy as np
import onnxruntime as ort
import librosa
from transformers import WhisperFeatureExtractor, WhisperTokenizer
from foundry_local import FoundryLocalManager

model_alias = "whisper-medium"
audio_file = sys.argv[1] if len(sys.argv) > 1 else "sample.wav"

if not os.path.exists(audio_file):
    print(f"Audio file not found: {audio_file}")
    sys.exit(1)

# ಹಂತ 1: ಬುಟ್‌ಸ್ಟ್ರಾಪ್ - ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸುತ್ತದೆ, ಡೌನ್‌ಲೋಡ್ ಮಾಡುತ್ತದೆ ಮತ್ತು ಮಾದರಿಯನ್ನು ಲೋಡ್ ಮಾಡುತ್ತದೆ
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# ಕ್ಯಾಶ್ ಮಾಡಿದ ONNX ಮಾದರಿ файಲ್‌ಗಳಿಗೆ ಮಾರ್ಗವನ್ನು ನಿರ್ಮಿಸು
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# ಹಂತ 2: ONNX ಸೆಷನ್‌ಗಳನ್ನು ಮತ್ತು ಫೀಚರ್ ಎಕ್ಸ್ಟ್ರಾಕ್ಟರ್ ಅನ್ನು ಲೋಡ್ ಮಾಡು
encoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_encoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
decoder = ort.InferenceSession(
    os.path.join(model_dir, "whisper-medium_decoder_fp32.onnx"),
    providers=["CPUExecutionProvider"]
)
fe = WhisperFeatureExtractor.from_pretrained(model_dir)
tokenizer = WhisperTokenizer.from_pretrained(model_dir)

# ಹಂತ 3: ಮೆಲ್ ಸ್ಪೆಕ್ಟ್ರೋಗ್ರಾಂ ಫೀಚರ್‌ಗಳನ್ನು ನಿರ್ಗಮಿಸಿ
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# ಹಂತ 4: ಎನ್ಕೋಡರ್ ಓಡಿಸಿ
enc_out = encoder.run(None, {"audio_features": input_features})
# ಮೊದಲ تولید скрытые состояния; ಉಳಿದವು ಕ್ರಾಸ್-ಅಟೆನ್ಷನ್ KV ಜೋಡಿಗಳು
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# ಹಂತ 5: ಆಟೋರೆಗ್ರೆಸಿವ್ ಡಿಕೋಡಿಂಗ್
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, ಟ್ರಾನ್ಸ್ಕ್ರೈಬ್, ಟೈಮ್ಸ್ಟ್ಯಾಂಪ್ಗಳು ಇಲ್ಲದೆ
input_ids = np.array([initial_tokens], dtype=np.int32)

# ಖಾಲಿ ಸೆಲ್ಫ್-ಅಟೆನ್ಷನ್ KV ಕ್ಯಾಷ್
self_kv = {}
for i in range(24):
    self_kv[f"past_key_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)
    self_kv[f"past_value_self_{i}"] = np.zeros((1, 16, 0, 64), dtype=np.float32)

generated = []
for _ in range(448):
    feeds = {"input_ids": input_ids, **cross_kv, **self_kv}
    outputs = decoder.run(None, feeds)
    logits = outputs[0]
    next_token = int(np.argmax(logits[0, -1, :]))

    if next_token == 50257:  # ಪಠ್ಯದ ಅಂತರ್ಯಾಮ
        break
    generated.append(next_token)

    # ಸೆಲ್ಫ್-ಅಟೆನ್ಷನ್ KV ಕ್ಯಾಷ್ ನವೀಕರಿಸಿ
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### ನಡಿಸಿ

```bash
# ಜಾವಾ ಉತ್ಪನ್ನ ಪರिदೃಶ್ಯವನ್ನು ಲಿಪ್ಯಂತರಿಸಿ
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# ಅಥವಾ ಇತರರನ್ನು ಪ್ರಯತ್ನಿಸಿ:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### ಪ್ರಮುಖ Python ಅಂಶಗಳು

| ವಿಧಾನ | ಉದ್ದೇಶ |
|--------|---------|
| `FoundryLocalManager(alias)` | ಬೂಟ್‌ಸ್ಟ್ರಾಪ್: ಸೇವೆ ಪ್ರಾರಂಭಿಸಿ, ಡೌನ್‌ಲೋಡ್ ಮಾಡಿ, ಮಾದರಿಯನ್ನು ಲೋಡ್ ಮಾಡುವುದು |
| `manager.get_cache_location()` | ಆನ್‌ಎನ್‍ಎಕ್ಸ‌ಸ್ಕೋಪ್ ಮಾಡಲಾದ ಮಾದರಿ ಫೈಲ್‌ಗಳ ಮಾರ್ಗ ಪಡೆಯುವುದು |
| `WhisperFeatureExtractor.from_pretrained()` | ಮೆಲ್ ಸ್ಪೆಕ್ಟ್ರೋಗ್ರಾಂ ವೈಶಿಷ್ಟ್ಯ ಉಪಕರಣವನ್ನು ಲೋಡ್ ಮಾಡುವುದು |
| `ort.InferenceSession()` | ಎನ್‌ಕೋಡರ್ ಮತ್ತು ಡಿಕೋಡರ್ ನ ONNX ರನ್‌ಟೈಮ್ ಸೆಷನ್ ಗಳೆ ನಿರ್ಮಿಸುವುದು |
| `tokenizer.decode()` | ಔಟ್‌ಪುಟ್ ಟೋಕನ್ IDಗಳನ್ನು ಪಠ್ಯಕ್ಕೆ ಪರಿವರ್ತಿಸುವುದು |

</details>

<details>
<summary><h3>JavaScript ಟ್ರ್ಯಾಕ್</h3></summary>

#### ಸೆಟಪ್

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಕೋಡ್

`foundry-local-whisper.mjs` ಎಂಬ ಫೈಲ್ ಸೃಷ್ಟಿಸಿ:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// ಸ್ಟೆಪ್ 1: ಬುಟ್‌ಸ್ಟ್ರಾಪ್ - ವ್ಯವಸ್ಥಾಪಕರನ್ನು ರಚಿಸಿ, ಸೇವೆಯನ್ನು ಪ್ರಾರಂಭಿಸಿ ಮತ್ತು ಮಾದರಿಯನ್ನು ಲೋಡ್ ಮಾಡಿ
console.log(`Initialising Foundry Local with model: ${modelAlias}...`);
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

const catalog = manager.catalog;
const model = await catalog.getModel(modelAlias);

if (!model.isCached) {
  console.log("Downloading Whisper model...");
  await model.download();
}
await model.load();

// ಸ್ಟೆಪ್ 2: ಒಂದು ಧ್ವನಿ ಗ್ರಾಹಕರನ್ನು ರಚಿಸಿ ಮತ್ತು ಲಿಪ್ಯಂತರಣೆ ಮಾಡಿ
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// ಸ್ವಚ್ಛತೆ
await model.unload();
```

> **ಗಮನಿಸಿ:** Foundry Local SDK ಒಳಗೊಂಡ `AudioClient` ಅನ್ನು `model.createAudioClient()` ಮೂಲಕ ಒದಗಿಸುತ್ತದೆ, ಇದು ಸಂಪೂರ್ಣ ONNX ಇನ್ಫರೆನ್ಸ್ ಪೈಪ್‌ಲೈನ್ ಅನ್ನು ಆಂತರಿಕವಾಗಿ ನಿರ್ವಹಿಸುತ್ತದೆ — ನೀವು `onnxruntime-node` ಅನ್ನು ಆಮದು ಮಾಡಲು ಅಗತ್ಯವಿಲ್ಲ. ಯಾವಾಗಲೂ `audioClient.settings.language = "en"` ಎಂದು English ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ನಿಖರತೆಗೆ ಸೆಟ್ ಮಾಡಿ.

#### ನಡಿಸಿ

```bash
# Zava ಉತ್ಪನ್ನ ಪರಿಕರವಾಗಿದೆ
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# ಅಥವಾ ಇತರರನ್ನು ಪ್ರಯತ್ನಿಸಿ:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### ಪ್ರಮುಖ JavaScript ಅಂಶಗಳು

| ವಿಧಾನ | ಉದ್ದೇಶ |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | ಮ್ಯಾನೇಜರ್ ಸಿಂಗಲ್ಟನ್ ಅನ್ನು ರಚಿಸಿ |
| `await catalog.getModel(alias)` | ಕ್ಯಾಟಲಾಗ್‌ನಿಂದ ಮಾದರಿ ಪಡೆಯಿರಿ |
| `model.download()` / `model.load()` | ವಿಜಯ್ ಮಾದರಿಯನ್ನು ಡೌನ್‌ಲೋಡ್ ಮತ್ತು ಲೋಡ್ ಮಾಡಿ |
| `model.createAudioClient()` | ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್‌ಗೆ ಆಡಿಯೋ ಕ್ಲೈಂಟ್ ಸೃಷ್ಟಿಸಿ |
| `audioClient.settings.language = "en"` | ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಭಾಷೆಯನ್ನು ಸೆಟ್ ಮಾಡಿ (ನಿಖರ ಔಟ್‌ಪುಟ್‌ಗೆ ಅಗತ್ಯ) |
| `audioClient.transcribe(path)` | ಆಡಿಯೋ ಫೈಲ್ ಟ್ರಾನ್ಸ್ಕ್ರೈಬ್ ಮಾಡಿ, `{ text, duration }` ಅನ್ನು ಮರಳಿಸುತ್ತದೆ |

</details>

<details>
<summary><h3>C# ಟ್ರ್ಯಾಕ್</h3></summary>

#### ಸೆಟಪ್

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **ಗಮನಿಸಿ:** C# ಟ್ರ್ಯಾಕ್ `Microsoft.AI.Foundry.Local` ಪ್ಯಾಕೇಜ್ ಬಳಸುತ್ತದೆ, ಇದು `model.GetAudioClientAsync()` ಮೂಲಕ ಒಳಗೊಂಡ `AudioClient` ಅನ್ನು ನೀಡುತ್ತದೆ. ಇದು ಪ್ರಕ್ರಿಯೆಯಲ್ಲಿ ಸಂಪೂರ್ಣ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಪೈಪ್‌ಲೈನ್ ಅನ್ನು ನಿರ್ವಹಿಸುತ್ತದೆ — ಬೇರೆ ONNX Runtime ಸೆಟಪ್ ಅಗತ್ಯವಿಲ್ಲ.

#### ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಕೋಡ್

`Program.cs` ಫೈಲ್ iç Tibet ನ್ನು ಬದಲಿಸಿರಿ:

```csharp
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.Logging.Abstractions;

// --- Configuration ---
var modelAlias = "whisper-medium";
var audioFile = args.Length > 0 ? args[0] : "sample.wav";

if (!File.Exists(audioFile))
{
    Console.WriteLine($"Audio file not found: {audioFile}");
    Console.WriteLine("Usage: dotnet run <path-to-audio-file>");
    return;
}

// --- Step 1: Initialize Foundry Local ---
Console.WriteLine("Initializing Foundry Local...");
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// --- Step 2: Load the Whisper model ---
Console.WriteLine($"Loading model: {modelAlias}...");
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync(modelAlias, default);

// Download if needed
var isCached = await model.IsCachedAsync(default);
if (!isCached)
{
    Console.WriteLine("Downloading model...");
    await model.DownloadAsync(null, default);
}

// Load model into memory
Console.WriteLine("Loading model into memory...");
await model.LoadAsync(default);

// --- Step 3: Transcribe audio ---
Console.WriteLine($"Transcribing: {audioFile}");

var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en";

var result = await audioClient.TranscribeAudioAsync(audioFile);

Console.WriteLine("\n--- Transcription ---");
Console.WriteLine(result.Text);
Console.WriteLine("---------------------");
```

#### ನಡಿಸಿ

```bash
# Zava ಉತ್ಪನ್ನ ಪರಿಸ್ಥಿತಿಯನ್ನು ಲಿಪ್ಯಂತರಿಸಿ
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# ಅಥವಾ ಇತರ ಪ್ರಯತ್ನಿಸಿ:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### ಪ್ರಮುಖ C# ಅಂಶಗಳು

| ವಿಧಾನ | ಉದ್ದೇಶ |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | Foundry Local ನ್ನು ಸಂರಚನೆಯೊಂದಿಗೆ ಆರಂಭಿಸಿ |
| `catalog.GetModelAsync(alias)` | ಕ್ಯಾಟಲಾಗ್ನಿಂದ ಮಾದರಿ ಪಡೆಯಿರಿ |
| `model.DownloadAsync()` | ವಿಜಯ್ ಮಾದರಿಯನ್ನು ಡೌನ್‌ಲೋಡ್ ಮಾಡಿ |
| `model.GetAudioClientAsync()` | AudioClient (ChatClient ಅಲ್ಲ!) ಪಡೆಯಿರಿ |
| `audioClient.Settings.Language = "en"` | ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಭಾಷೆ ಸೆಟ್ ಮಾಡಿ (ನಿಖರ ಔಟ್‌ಪುಟ್‌ಗೆ ಅಗತ್ಯ) |
| `audioClient.TranscribeAudioAsync(path)` | ಆಡಿಯೋ ಫೈಲ್ ಟ್ರಾನ್ಸ್ಕ್ರೈಬ್ ಮಾಡಿ |
| `result.Text` | ಟ್ರಾನ್ಸ್ಕ್ರೈಬ್ ಮಾಡಿದ ಪಠ್ಯ |
> **C# vs Python/JS:** C# SDK ಒಳಗೊಂಡಿದೆ `AudioClient` ಆಂತರಿಕ ಪ್ರಕ್ರಿಯೆಯ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್‌ಗೆ `model.GetAudioClientAsync()` ಮೂಲಕ, JavaScript SDK ಅನುಕೂಲದಂತೆ. Python ನೇರವಾಗಿ ONNX Runtime ಅನ್ನು ಬಳಸಿಕೊಂಡು ಕ್ಯಾಶ್ ಮಾಡಲಾದ ಎನ್ಕೋಡರ್/ಡಿಕೋಡರ್ ಮಾದರಿಗಳ ವಿರುದ್ಧ ಅಂದಾಜು ಮಾಡುತ್ತದೆ.

</details>

---

### ಅಭ್ಯಾಸ 4 - ಎಲ್ಲಾ Zava ಮಾದರಿಗಳನ್ನು ಬ್ಯಾಚ್ ಮೂಲಕ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡಿರಿ

ನೀವು ಈಗ ಕಾರ್ಯನಿರ್ವಹಿಸುತ್ತಿರುವ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಆಪ್ ಹೊಂದಿದ್ದೀರಿ, ಎಲ್ಲಾ ಐದು Zava ಮಾದರಿ ಫೈಲ್‌ಗಳನ್ನು ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡಿ ಮತ್ತು ಫಲಿತಾಂಶಗಳನ್ನು ಹೋಲಿಸಿ.

<details>
<summary><h3>Python ಟ್ರ್ಯಾಕ್</h3></summary>

ಪೂರ್ಣ ಮಾದರಿ `python/foundry-local-whisper.py` ಈಗಾಗಲೇ ಬ್ಯಾಚ್ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಬೆಂಬಲಿಸುತ್ತದೆ. ಬಾಹ್ಯ ವಾರ್ತೆಗಳಿಲ್ಲದೆ ಕಾರ್ಯಗತಗೊಳಿಸಿದಾಗ, ಇದು `samples/audio/` ನಲ್ಲಿ ಎಲ್ಲಾ `zava-*.wav` ಫೈಲ್‌ಗಳನ್ನು ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡುತ್ತದೆ:

```bash
cd python
python foundry-local-whisper.py
```

ಮಾದರಿ `FoundryLocalManager(alias)` ಅನ್ನು ಪ್ರಾರಂಭಿಸಲು ಉಪಯೋಗಿಸಿಕೊಂಡು, ಬಳಿಕ ಪ್ರತಿ ಫೈಲ್‌ಗೆ ಎನ್ಕೋಡರ್ ಮತ್ತು ಡಿಕೋಡರ್ ONNX ಸೆಷನ್ಗಳನ್ನು ಕಾರ್ಯಗತಗೊಳಿಸುತ್ತದೆ.

</details>

<details>
<summary><h3>JavaScript ಟ್ರ್ಯಾಕ್</h3></summary>

ಪೂರ್ಣ ಮಾದರಿ `javascript/foundry-local-whisper.mjs` ಈಗಾಗಲೇ ಬ್ಯಾಚ್ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಬೆಂಬಲಿಸುತ್ತದೆ. ಬಾಹ್ಯ ವಾರ್ತೆಗಳಿಲ್ಲದೆ ಕಾರ್ಯಗತಗೊಳಿಸಿದಾಗ, ಇದು `samples/audio/` ನಲ್ಲಿ ಎಲ್ಲಾ `zava-*.wav` ಫೈಲ್‌ಗಳನ್ನು ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡುತ್ತದೆ:

```bash
cd javascript
node foundry-local-whisper.mjs
```

ಮಾದರಿ `FoundryLocalManager.create()` ಮತ್ತು `catalog.getModel(alias)` ಉಪಯೋಗಿಸಿ SDK ಅನ್ನು ಪ್ರಾರಂಭಿಸಿ, ನಂತರ `AudioClient` (`settings.language = "en"` ನೊಂದಿಗೆ) ಮೂಲಕ ಪ್ರತಿ ಫೈಲ್ ಅನ್ನು ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡುತ್ತದೆ.

</details>

<details>
<summary><h3>C# ಟ್ರ್ಯಾಕ್</h3></summary>

ಪೂರ್ಣ ಮಾದರಿ `csharp/WhisperTranscription.cs` ಈಗಾಗಲೇ ಬ್ಯಾಚ್ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಬೆಂಬಲಿಸುತ್ತದೆ. ನಿರ್ದಿಷ್ಟ ಫೈಲ್ ವಾದನೆಯನ್ನು ನೀಡದಿದ್ದರೆ, ಇದು `samples/audio/` ನಲ್ಲಿ ಎಲ್ಲಾ `zava-*.wav` ಫೈಲ್‌ಗಳನ್ನು ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡುತ್ತದೆ:

```bash
cd csharp
dotnet run whisper
```

ಮಾದರಿ `FoundryLocalManager.CreateAsync()` ಮತ್ತು SDK ನ `AudioClient` (`Settings.Language = "en"` ಜೊತೆಗೆ) ಆಂತರಿಕ ಪ್ರಕ್ರಿಯೆಯ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್‌ಗೆ ಉಪಯೋಗಿಸುತ್ತದೆ.

</details>

**ನೋಟಿಸುವುದು ಏನು:** `samples/audio/generate_samples.py` ನ ಮೂಲ ಪಠ್ಯದೊಂದಿಗೆ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಔಟ್‌ಪುಟ್ ಹೋಲಿಸಿರಿ. "Zava ProGrip" ಹಾಗು "brushless motor" ಅಥವಾ "composite decking" ಹೋಲಿಯಾದ ತಾಂತ್ರಿಕ ಪದಗಳನ್ನು Whisper ಎಷ್ಟರಮಟ್ಟಿಗೆ ನಿಖರವಾಗಿ ಹಿಡಿಯುತ್ತದೆ?

---

### ಅಭ್ಯಾಸ 5 - ಪ್ರಮುಖ ಕೋಡ್ ಮಾದರಿಗಳನ್ನು ಅರ್ಥಮಾಡಿಕೊಳ್ಳಿ

ಮೂರು ಭಾಷೆಗಳಲ್ಲಿಯೂ Whisper ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಚಾಟ್ ಪೂರ್ಣತೆಗಳಿಗಿಂತ ಹೇಗೆ ವ್ಯತ್ಯಾಸ ಹೊಂದಿದೆ ಎಂದು ಅಧ್ಯಯನ ಮಾಡಿ:

<details>
<summary><b>Python - ಚಾಟ್‌ಗೆ ಇದ್ದ ಪ್ರಮುಖ ವ್ಯತ್ಯಾಸಗಳು</b></summary>

```python
# ಚಾಟ್ ಪೂರ್ಣಗೊಳ್ಳುವಿಕೆ (ಭಾಗಗಳು 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# ಧ್ವನಿ ಲಿಪ್ಯಂತರಣ (ಈ ಭಾಗ):
# OpenAI ಕ್ಲಯಿಂಟ್ ಬದಲಾಗಿ ನೇರವಾಗಿ ONNX ರನ್‌ಟೈಮ್ ಅನ್ನು ಬಳಸುತ್ತದೆ
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... ಸ್ವಯಂಪ್ರೇರಿತ ಡಿಕೋಡರ್ ಲೂಪ್ ...
print(tokenizer.decode(generated_tokens))
```

**ಪ್ರಮುಖ ತಿಳಿವು:** ಚಾಟ್ ಮಾದರಿಗಳು OpenAI-ಅನುಕುಲ API ಅನ್ನು `manager.endpoint` ಮೂಲಕ ಬಳಸುತ್ತವೆ. Whisper ನೇರವಾಗಿ ONNX Runtime ಬಳಸಿ ಕ್ಯಾಶ್ಡ್ ONNX ಮಾದರಿಗಳ ವಿರುದ್ಧ ಅಂದಾಜು ಮಾಡುತ್ತದೆ.

</details>

<details>
<summary><b>JavaScript - ಚಾಟ್‌ಗೆ ಇದ್ದ ಪ್ರಮುಖ ವ್ಯತ್ಯಾಸಗಳು</b></summary>

```javascript
// ಚಾಟ್ ಪೂರ್ಣಗೊಳಿಸುವಿಕೆ (ಭಾಗಗಳು ೨-೬):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// ಧ್ವನಿ ಲಿಪ್ಯಂತರಣೆ (ಈ ಭಾಗ):
// SDK ನ ಒಳಗೊಂಡ AudioClient ಅನ್ನು ಬಳಸುತ್ತದೆ
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // ಉತ್ತಮ ಫಲಿತಾಂಶಗಳಿಗಾಗಿ ಯಾವಾಗಲೂ ಭಾಷೆಯನ್ನು ಹೊಂದಿಸಿ
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**ಪ್ರಮುಖ ತಿಳಿವು:** ಚಾಟ್ ಮಾದರಿಗಳು OpenAI-ಅನುಕುಲ API ಅನ್ನು `manager.urls[0] + "/v1"` ಮೂಲಕ ಬಳಸುತ್ತವೆ. Whisper ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ SDKಯ `AudioClient` ಬಳಸಿ, ಇದು `model.createAudioClient()` ಮೂಲಕ ಪಡೆಯಲಾಗುತ್ತದೆ. ಸ್ವಯಂಎಂಕಿತ ತಪ್ಪುವಿಕೆ ತಪ್ಪಿಸಲು `settings.language` ನ್ನು ಹೊಂದಿಸಿ.

</details>

<details>
<summary><b>C# - ಚಾಟ್‌ಗೆ ಇದ್ದ ಪ್ರಮುಖ ವ್ಯತ್ಯಾಸಗಳು</b></summary>

C# ವಿಧಾನ SDK ನ ಆಂತರಿಕ `AudioClient` ಬಳಸುತ್ತದೆ ಆಂತರಿಕ ಪ್ರಕ್ರಿಯೆಯ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್‌ಗೆ:

**ಮಾದರಿ ಆರಂಭಿಕರಣ:**

```csharp
// 1. Create the manager with configuration
await FoundryLocalManager.CreateAsync(
    new Configuration
    {
        AppName = "WhisperDemo",
        Web = new Configuration.WebService { Urls = "http://127.0.0.1:0" }
    }, NullLogger.Instance, default);
var manager = FoundryLocalManager.Instance;
await manager.StartWebServiceAsync(default);

// 2. Get model from catalog, download, and load
var catalog = await manager.GetCatalogAsync(default);
var model = await catalog.GetModelAsync("whisper-medium", default);
await model.DownloadAsync(null, default);
await model.LoadAsync(default);
```

**ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**ಪ್ರಮುಖ ತಿಳಿವು:** C# ನಲ್ಲಿ `FoundryLocalManager.CreateAsync()` ಬಳಸುತ್ತಾರ ಮತ್ತು ನೇರವಾಗಿ `AudioClient` ಪಡೆದುಕೊಳ್ಳುತ್ತಾರೆ — ONNX Runtime ಸೆಟ್‌ಅಪ್ ಅಗತ್ಯವಿಲ್ಲ. ಸ್ವಯಂ ಎಂಕಿತ ತಪ್ಪುಗಳನ್ನು ತಪ್ಪಿಸಲು `Settings.Language` ಹೊಂದಿಸಿ.

</details>

> **ಸಂಗ್ರಹ:** Python ಮಾದರಿ ನಿರ್ವಹಣೆಗೆ Foundry Local SDK ಹಾಗು ಎನ್ಕೋಡರ್/ಡಿಕೋಡರ್ ಮಾದರಿಗಳ ಮೇಲ್ಗೆ ನೇರ ONNX Runtime ಒಳಗೊಂಡಿದೆ. JavaScript ಮತ್ತು C# ಎರಡೂ SDK ಆಂತರಿಕ `AudioClient` ಬಳಸುತ್ತಾರೆ — ಕ್ಲೈಂಟ್ ಅನ್ನು ಸೃಷ್ಟಿಸಿ, ಭಾಷೆಯನ್ನು ಸೆಟ್ ಮಾಡಿ, ನಂತರ `transcribe()` / `TranscribeAudioAsync()` ಕರೆಮಾಡಿ. ನಿಖರ ಫಲಿತಾಂಶಗಳಿಗಾಗಿ ಎಚ್ಚರಿಕೆಯಿಂದ AudioClient ಭಾಷಾ ಗುಣವನ್ನು ಎಂದಾದರೂ ಸೆಟ್ ಮಾಡಿರಿ.

---

### ಅಭ್ಯಾಸ 6 - ಪ್ರಯೋಗ ಮಾಡಿ

ನಿಮ್ಮ ಅರ್ಥವನ್ನು ಭದ್ರಗೊಳಿಸಲು ಈ ಬದಲಾವಣೆಗಳನ್ನು ಪ್ರಯತ್ನಿಸಿ:

1. **ವಿವಿಧ ಧ್ವನಿ ಫೈಲ್‌ಗಳನ್ನು ಪ್ರಯತ್ನಿಸಿ** - Windows Voice Recorder ಬಳಸಿ ನಿಮ್ಮ ಮಾತನ್ನು ದಾಖಲಿಸಿ, WAV ಆಗಿ ಉಳಿಸಿ ಮತ್ತು ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡಿ

2. **ಮಾದರಿ ವಿಭಿನ್ನತೆಗಳನ್ನು ಹೋಲಿಸಿ** - ನಿಮಗೆ NVIDIA GPU ಇದ್ದರೆ, CUDA ವಿವಿಧತೆಯನ್ನು ಪ್ರಯತ್ನಿಸಿ:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   CPU ವಿಭಿನ್ನತೆಯ ಸಿಗ್ನಲ್ ಸ್ಪೀಡ್ ಹೋಲಿಸಿ.

3. **ಔಟ್‌ಪುಟ್ ಫಾರ್ಮ್ಯಾಟ್ ಸೇರಿಸಿ** - JSON ಪ್ರತಿಕ್ರಿಯೆಯಲ್ಲಿ ಸೇರಬಹುದಾಗಿದೆ:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **REST API ನಿರ್ಮಿಸಿ** - ನಿಮ್ಮ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಕೋಡ್ ಅನ್ನು ವೆಬ್ ಸರ್ವರ್‌ನಲ್ಲಿ ಮಡಿಸಿ:

   | ಭಾಷೆ | ಫ್ರೇಮ್‌ವರ್ಕ್ | ಉದಾಹರಣೆ |
   |----------|-----------|--------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` + `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` + `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` + `IFormFile` |

5. **ಬಹು-ತಿರುವು ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಜೊತೆಗೆ** - ಭಾಗ 4 ನ ಚಾಟ್ ಏಜೆಂಟ್ ಜೊತೆ ಜೊತೆಗೆ ಬಳಸಿ: ಮೊದಲು ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡಿ, ನಂತರ ಪಠ್ಯವನ್ನು ಏಜೆಂಟ್ ಗೆ ವಿಶ್ಲೇಷಣೆ ಅಥವಾ ಸಾರಾಂಶಕ್ಕಾಗಿ ನೀಡಿ.

---

## SDK ಆಡಿಯೋ API ರೆಫರೆನ್ಸ್

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — `AudioClient` ಉದಾಹರಣೆ ರಚಿಸುತ್ತದೆ
> - `audioClient.settings.language` — ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಭಾಷೆಯನ್ನು ಸೆಟ್ ಮಾಡುವುದು (ಉದಾ: `"en"`)
> - `audioClient.settings.temperature` — بےقاعدگي ನಿಯಂತ್ರಣ (ಐಚ್ಛಿಕ)
> - `audioClient.transcribe(filePath)` — ಫೈಲ್ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡುತ್ತದೆ, `{ text, duration }` ನೀಡುತ್ತದೆ
> - `audioClient.transcribeStreaming(filePath, callback)` — ಕಾಲ್‌ಬ್ಯಾಕ್ ಮೂಲಕ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಬಿತ್ತರಣೆಗಳನ್ನು ಸ್ಟ್ರೀಮ್ ಮಾಡುವುದು
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — `OpenAIAudioClient` ಉದಾಹರಣೆ ರಚಿಸುತ್ತದೆ
> - `audioClient.Settings.Language` — ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಭಾಷೆಯನ್ನು ಸೆಟ್ ಮಾಡುವುದು (ಉದಾ: `"en"`)
> - `audioClient.Settings.Temperature` — بےقاعدگي ನಿಯಂತ್ರಣ (ಐಚ್ಛಿಕ)
> - `await audioClient.TranscribeAudioAsync(filePath)` — ಫೈಲ್ ಅನ್ನು ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡುತ್ತದೆ, `.Text` ಹೊಂದಿರುವ ವಸ್ತುವನ್ನು ನೀಡುತ್ತದೆ
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ಬಿತ್ತರಣೆಗಳ IAsyncEnumerable ನೀಡುತ್ತದೆ

> **ಸಂಕೇತ:** ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡುವ ಮೊದಲು ಎಂದಾದರೂ ಭಾಷೆ ಗುಣವನ್ನು ಸೆಟ್ ಮಾಡಬೇಕು. ಇಲ್ಲದಿದ್ದರೆ, Whisper ಮಾದರಿ ಸ್ವಯಂಎಂಕಿತವನ್ನು ಪ್ರಯತ್ನಿಸುತ್ತದೆ, ಇದು ಗದ್ದಲದ ಔಟ್‌ಪುಟ್ (ಪಠ್ಯದ ಬದಲು ಒಂದೇ ಬದಲಾವಣೆ ಅಕ್ಷರ) ನೀಡಬಹುದು.

---

## ಹೋಲಿಕೆ: ಚಾಟ್ ಮಾದರಿಗಳು vs. Whisper

| ಅಂಶ | ಚಾಟ್ ಮಾದರಿಗಳು (ಭಾಗಗಳು 3-7) | Whisper - Python | Whisper - JS / C# |
|--------|------------------------------|--------------------|----------------------|
| **ಕಾರ್ಯ ಪ್ರಕಾರ** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **ಇನ್ಪುಟ್** | ಪಠ್ಯ ಸಂದೇಶಗಳು (JSON) | ಧ್ವನಿ ಫೈಲ್‌ಗಳು (WAV/MP3/M4A) | ಧ್ವನಿ ಫೈಲ್‌ಗಳು (WAV/MP3/M4A) |
| **ಔಟ್‌ಪುಟ್** | ಸೃಷ್ಟಿಸಲ್ಪಟ್ಟ ಪಠ್ಯ (ಸ್ಟ್ರೀಮ್ ಆಗಿ) | ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಪಠ್ಯ (ಪೂರ್ಣ) | ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಪಠ್ಯ (ಪೂರ್ಣ) |
| **SDK ಪ್ಯಾಕೇಜ್** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API ವಿಧಾನ** | `client.chat.completions.create()` | ONNX Runtime ನೇರವಾಗಿ | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **ಭಾಷೆ ಸೆಟ್ಟಿಂಗ್** | ಅನ್ವಯಿಸುವುದಿಲ್ಲ | ಡಿಕೋಡರ್ ಪ್ರಾಂಪ್ಟ್ ಟೋಕನ್ಗಳು | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **ಸ್ಟ್ರೀಮಿಂಗ್** | ಹೌದು | ಇಲ್ಲ | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **ಗೌಪ್ಯತೆ ಪ್ರಯೋಜನ** | ಕೋಡ್/ಡೇಟಾ ಸ್ಥಳೀಯವಾಗಿರುತ್ತದೆ | ಧ್ವನಿ ಡೇಟಾ ಸ್ಥಳೀಯವಾಗಿರುತ್ತದೆ | ಧ್ವನಿ ಡೇಟಾ ಸ್ಥಳೀಯವಾಗಿರುತ್ತದೆ |

---

## ಪ್ರಮುಖ ಅಂಶಗಳು

| ಕಲಿಕೆ | ನೀವು ಕಲಿತದ್ದು |
|---------|-----------------|
| **Whisper ಸಾಧನದ ಮೇಲೆ** | ಮಾತನ್ನು ಪಠ್ಯಕ್ಕೆ ಮಾರಾಟ ಸಂಪೂರ್ಣವಾಗಿ ಆನ್-ಡಿವೈಸಾಗಿ ನಡೆಯುತ್ತದೆ, Zava ಗ್ರಾಹಕರ ಕರೆಗಳು ಮತ್ತು ಉತ್ಪನ್ನ ವಿಮರ್ಶೆಗಳನ್ನು ಸಾಧನದ ಮೇಲೆ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಟ್ ಮಾಡಲು ಉತ್ತಮ |
| **SDK AudioClient** | JavaScript ಹಾಗೂ C# SDKಗಳbuilt-in `AudioClient` ಒದಗಿಸುತ್ತವೆ, ಇದು ಸಂಪೂರ್ಣ ಟ್ರಾನ್ಸ್ಕ್ರಿಪ್ಷನ್ ನಿಯಂತ್ರಣವನ್ನು ಒಮ್ಮೆ ಕರೆಯುವ ಮೂಲಕ ನಿಭಾಯಿಸುತ್ತದೆ |
| **ಭಾಷೆ ಸೆಟ್ಟಿಂಗ್** | AudioClient ಭಾಷೆಯನ್ನು (ಉದಾ: `"en"`) ಯಾವತ್ತೂ ಸೆಟ್ ಮಾಡಿರಿ — ಇಲ್ಲದಿದ್ದರೆ ಸ್ವಯಂಎಂಕಿತದಿಂದ ಗದ್ದಲದ ಔಟ್‌ಪುಟ್ ಬರುತ್ತದೆ |
| **Python** | `foundry-local-sdk` ಮಾದರಿ ನಿರ್ವಹಣೆಗೆ + `onnxruntime` + `transformers` + `librosa` ನೇರ ONNX ಅಂದಾಜಿಗೆ ಬಳಸುತ್ತದೆ |
| **JavaScript** | `foundry-local-sdk` ಮತ್ತು `model.createAudioClient()` ಬಳಸಿ — `settings.language` ಸೆಟ್ ಮಾಡಿ, ನಂತರ `transcribe()` ಕರೆಯಿರಿ |
| **C#** | `Microsoft.AI.Foundry.Local` ಮತ್ತು `model.GetAudioClientAsync()` ಬಳಸಿ — `Settings.Language` ಸೆಟ್ ಮಾಡಿ, ನಂತರ `TranscribeAudioAsync()` ಕರೆಯಿರಿ |
| **ಸ್ಟ್ರೀಮಿಂಗ್ ಬೆಂಬಲ** | JS ಮತ್ತು C# SDKಗಳು `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` ಅನ್ನು ಚಂಕ್-ಮಾರ್ಗದಿಂದ ಔಟ್‌ಪುಟ್‌ಗೆ ನೀಡುತ್ತವೆ |
| **CPU-ಗುರ್ತಿಸುಲಭ** | CPU ಆವೃತ್ತಿ (3.05 GB) ಯಾವುದೇ Windows ಸಾಧನದಲ್ಲಿ GPU ಇಲ್ಲದೇ ಕಾರ್ಯನಿರ್ವಹಿಸುತ್ತದೆ |
| **ಗೌಪ್ಯತೆ-ಪ್ರಥಮ** | Zava ಗ್ರಾಹಕ ಸಂವಾದಗಳು ಹಾಗೂ ಸ್ವಂತಿಗತ ಉತ್ಪನ್ನ ಡೇಟಾವನ್ನು ಸಾಧನದ ಮೇಲೆ ಇರಿಸಿಕೊಳ್ಳಲು ಪರಿಪೂರಕ |

---

## ಸಂಪನ್ಮೂಲಗಳು

| ಸಂಪನ್ಮೂಲ | ಲಿಂಕ್ |
|----------|-------|
| Foundry Local ದಸ್ತಾವೇಜುಗಳು | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK ರೆಫರೆನ್ಸ್ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper ಮಾದರಿ | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local ವೆಬ್‌ಸೈಟ್ | [foundrylocal.ai](https://foundrylocal.ai) |

---

## ಮುಂದಿನ ಹಂತ

[ಭಾಗ 10: ಕಸ್ಟಮ್ ಅಥವಾ Hugging Face ಮಾದರಿಗಳನ್ನು ಬಳಸುವುದು](part10-custom-models.md) ಗೆ ಮುಂದುವರಿಸಿ, ನಿಮ್ಮ ಸ್ವಂತ ಮಾದರಿಗಳನ್ನು Hugging Face ನಿಂದ ಸಂಗ್ರಹಿಸಿ Foundry Local ಮೂಲಕ ಚಲಾಯಿಸಿ.