![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ਭਾਗ 9: ਵਿਸਪਰ ਅਤੇ ਫਾਊਂਡਰੀ ਲੋਕਲ ਨਾਲ ਵੌਇਸ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ

> **ਲਕੜੀ:** ਫਾਊਂਡਰੀ ਲੋਕਲ ਦੇ ਜ਼ਰੀਏ ਸਥਾਨਕ ਤੌਰ 'ਤੇ ਚੱਲ ਰਹੇ OpenAI Whisper ਮਾਡਲ ਨੂੰ ਵਰਤ ਕੇ ਆਡੀਓ ਫਾਈਲਾਂ ਦਾ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਕਰੋ - ਸਿਰਫ ਡਿਵਾਈਸ ਤੇ, ਕਿਸੇ ਕਲਾਉਡ ਦੀ ਲੋੜ ਨਹੀਂ।

## ਓਵਰਵਿਊ

ਫਾਊਂਡਰੀ ਲੋਕਲ ਸਿਰਫ ਟੈਕਸਟ ਜੈਨਰੇਸ਼ਨ ਲਈ ਨਹੀਂ ਹੈ; ਇਹ **ਸਪੀਚ-ਟੂ-ਟੈਕਸਟ** ਮਾਡਲ ਨੂੰ ਵੀ ਸਹਿਯੋਗ ਦਿੰਦਾ ਹੈ। ਇਸ ਲੈਬ ਵਿੱਚ ਤੁਸੀਂ **OpenAI Whisper Medium** ਮਾਡਲ ਨੂੰ ਇਸਤਮਾਲ ਕਰਕੇ ਆਪਣੇ ਮਸ਼ੀਨ 'ਤੇ ਪੂਰੀ ਤਰ੍ਹਾਂ ਆਡੀਓ ਫਾਈਲਾਂ ਦਾ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਕਰੋਗੇ। ਇਹ ਉਹਨਾਂ ਹਾਲਾਤਾਂ ਲਈ ਵਧੀਆ ਹੈ ਜਿਵੇਂ ਕਿ ਜਾਵਾ ਗ੍ਰਾਹਕ ਸੇਵਾ ਕਾਲਾਂ, ਉਤਪਾਦ ਸਮੀਖਿਆ ਰਿਕਾਰਡਿੰਗਾਂ, ਜਾਂ ਵਰਕਸ਼ਾਪ ਯੋਜਨਾ ਸੈਸ਼ਨਾਂ ਦਾ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਜਿੱਥੇ ਆਡੀਓ ਡੇਟਾ ਕਦੇ ਵੀ ਤੁਹਾਡੇ ਡਿਵਾਈਸ ਤੋਂ ਬਾਹਰ ਨਹੀਂ ਜਾਣਾ ਚਾਹੀਦਾ।


---

## ਸਿੱਖਣ ਦੇ ਲਕੜੀ

ਇਸ ਲੈਬ ਦੇ ਆਖਿਰ 'ਤੇ ਤੁਸੀਂ ਸਮਰੱਥ ਹੋਵੋਗੇ:

- ਵਿਸਪਰ ਸਪੀਚ-ਟੂ-ਟੈਕਸਟ ਮਾਡਲ ਅਤੇ ਇਸਦੀ ਸਮਰੱਥਾ ਨੂੰ ਸਮਝਣਾ
- ਫਾਊਂਡਰੀ ਲੋਕਲ ਦੀ ਵਰਤੋਂ ਨਾਲ ਵਿਸਪਰ ਮਾਡਲ ਨੂੰ ਡਾਊਨਲੋਡ ਕਰਨਾ ਅਤੇ ਚਲਾਉਣਾ
- ਫਾਊਂਡਰੀ ਲੋਕਲ SDK ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਆਡੀਓ ਫਾਈਲਾਂ ਦਾ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ Python, JavaScript, ਅਤੇ C# ਵਿੱਚ ਕਰਨਾ
- ਪੂਰੀ ਤਰ੍ਹਾਂ ਡਿਵਾਈਸ 'ਤੇ ਚਲਣ ਵਾਲੀ ਸਰਲ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਸਰਵਿਸ ਬਣਾਉਣਾ
- ਫਾਊਂਡਰੀ ਲੋਕਲ ਵਿੱਚ ਚੈਟ/ਟੈਕਸਟ ਮਾਡਲਾਂ ਅਤੇ ਆਡੀਓ ਮਾਡਲਾਂ ਵਿੱਚ ਅੰਤਰ ਸਮਝਣਾ

---

## ਪਹਿਲਾਂ ਤੋਂ ਲੋੜੀਂਦਾ

| ਲੋੜ | ਵੇਰਵਾ |
|-------------|---------|
| **Foundry Local CLI** | ਵਰਜ਼ਨ **0.8.101 ਜਾਂ ਉੱਪਰ** (ਵਿਸਪਰ ਮਾਡਲ v0.8.101 ਤੋਂ ਉਪਲਬਧ ਹਨ) |
| **ਆਪਰੇਟਿੰਗ ਸਿਸਟਮ** | Windows 10/11 (x64 ਜਾਂ ARM64) |
| **ਭਾਸ਼ਾ ਰਨਟਾਈਮ** | **Python 3.9+** ਅਤੇ/ਜਾਂ **Node.js 18+** ਅਤੇ/ਜਾਂ **.NET 9 SDK** ([ਡਾਊਨਲੋਡ .NET](https://dotnet.microsoft.com/download/dotnet/9.0)) |
| **ਮੁਕੰਮਲ ਕੀਤਾ** | [ਭਾਗ 1: ਸ਼ੁਰੂਆਤ](part1-getting-started.md), [ਭਾਗ 2: ਫਾਊਂਡਰੀ ਲੋਕਲ SDK ਡੀਪ ਡਾਈਵ](part2-foundry-local-sdk.md), ਅਤੇ [ਭਾਗ 3: SDK ਅਤੇ APIs](part3-sdk-and-apis.md) |

> **ਨੋਟ:** ਵਿਸਪਰ ਮਾਡਲਾਂ ਨੂੰ **SDK** ਰਾਹੀਂ ਡਾਊਨਲੋਡ ਕਰਨਾ ਲਾਜ਼ਮੀ ਹੈ (CLI ਨਹੀਂ)। CLI ਆਡੀਓ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਐਂਡਪੌਇੰਟ ਸਹਿਯੋਗ ਨਹੀਂ ਕਰਦਾ। ਆਪਣਾ ਵਰਜ਼ਨ ਜਾਣਚ ਕਰਨ ਲਈ:
> ```bash
> foundry --version
> ```

---

## ਕਾਂਸੈਪਟ: ਵਿਸਪਰ ਕਿਵੇਂ ਫਾਊਂਡਰੀ ਲੋਕਲ ਨਾਲ ਕੰਮ ਕਰਦਾ ਹੈ

OpenAI Whisper ਮਾਡਲ ਇੱਕ ਜਨਰਲ-ਪਰਪਜ਼ ਸਪੀਚ ਰਿਕਗਨੀਸ਼ਨ ਮਾਡਲ ਹੈ ਜੋ ਵਿਭਿੰਨ ਆਡੀਓ ਦੇ ਵੱਡੇ ਡੇਟਾਸੈੱਟ 'ਤੇ ਟ੍ਰੇਨ ਕੀਤਾ ਗਿਆ ਹੈ। ਜਦੋਂ ਫਾਊਂਡਰੀ ਲੋਕਲ ਰਾਹੀਂ ਚਲਾਇਆ ਜਾਂਦਾ ਹੈ:

- ਮਾਡਲ **ਸਿਰਫ ਤੁਹਾਡੇ CPU ਤੇ ਚਲਦਾ ਹੈ** - ਕਿਸੇ GPU ਦੀ ਲੋੜ ਨਹੀਂ
- ਆਡੀਓ ਤੁਹਾਡੇ ਡਿਵਾਈਸ ਨੂੰ ਕਦੇ ਨਹੀਂ ਛੱਡਦਾ - **ਪੂਰੀ ਗੋਪਨੀਯਤਾ**
- ਫਾਊਂਡਰੀ ਲੋਕਲ SDK ਮਾਡਲ ਡਾਊਨਲੋਡ ਅਤੇ ਕੈਸ਼ ਮੈਨੇਜਮੈਂਟ ਸੰਭਾਲਦਾ ਹੈ
- **JavaScript ਅਤੇ C#** SDK ਵਿੱਚ ਬਿਲਟ-ਇਨ `AudioClient` ਹੁੰਦਾ ਹੈ ਜੋ ਪੂਰੀ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਪਾਈਪਲਾਈਨ ਨੂੰ ਸਵੀਕਾਰ ਕਰਦਾ ਹੈ — ਕਿਸੇ ਵੀ ਮੈਨੂਅਲ ONNX ਸੈਟਅਪ ਦੀ ਲੋੜ ਨਹੀਂ
- **Python** SDK ਦਾ ਉਪਯੋਗ ਕਰਦਾ ਹੈ ਮਾਡਲ ਪ੍ਰਬੰਧਨ ਲਈ ਅਤੇ ONNX Runtime ਨੂੰ ਡਾਇਰੈਕਟ ਇਨਫਰੇਂਸ ਲਈ ਐਂਕੋਡਰ/ਡਿਕੋਡਰ ONNX ਮਾਡਲਾਂ ਉੱਤੇ

### ਪਾਈਪਲਾਈਨ ਕਿਵੇਂ ਕੰਮ ਕਰਦੀ ਹੈ (JavaScript ਅਤੇ C#) — SDK AudioClient

1. **ਫਾਊਂਡਰੀ ਲੋਕਲ SDK** ਵਿਸਪਰ ਮਾਡਲ ਡਾਊਨਲੋਡ ਅਤੇ ਕੈਸ਼ ਕਰਦਾ ਹੈ
2. `model.createAudioClient()` (JS) ਜਾਂ `model.GetAudioClientAsync()` (C#) ਇੱਕ `AudioClient` ਬਣਾਂਦਾ ਹੈ
3. `audioClient.transcribe(path)` (JS) ਜਾਂ `audioClient.TranscribeAudioAsync(path)` (C#) ਪੂਰੀ ਪਾਈਪਲਾਈਨ ਅੰਦਰੂਨੀ ਤੌਰ 'ਤੇ ਸੰਭਾਲਦਾ ਹੈ — ਆਡੀਓ ਪ੍ਰੀਪ੍ਰੋਸੈਸਿੰਗ, ਐਂਕੋਡਰ, ਡਿਕੋਡਰ, ਅਤੇ ਟੋਕਨ ਡਿਕੋਡਿੰਗ
4. `AudioClient` ਇੱਕ `settings.language` ਦੇ ਪ੍ਰਾਪਰਟੀ (ਅੰਗਰੇਜ਼ੀ ਲਈ `"en"` ਸੈੱਟ) ਦੇ ਕੇ ਸਹੀ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਦਿਸ਼ਾ ਦਿੰਦਾ ਹੈ

### ਪਾਈਪਲਾਈਨ ਕਿਵੇਂ ਕੰਮ ਕਰਦੀ ਹੈ (Python) — ONNX Runtime

1. **ਫਾਊਂਡਰੀ ਲੋਕਲ SDK** ਵਿਸਪਰ ONNX ਮਾਡਲ ਫਾਈਲਾਂ ਡਾਊਨਲੋਡ ਅਤੇ ਕੈਸ਼ ਕਰਦਾ ਹੈ
2. **ਆਡੀਓ ਪ੍ਰੀਪ੍ਰੋਸੈਸਿੰਗ** WAV ਆਡੀਓ ਨੂੰ ਇੱਕ ਮੇਲ ਸਪੈਕਟਰੋਗ੍ਰਾਮ (80 ਮੇਲ ਬਿਨ × 3000 ਫਰੇਮ) ਵਿੱਚ ਬਦਲਦਾ ਹੈ
3. **ਐਂਕੋਡਰ** ਮੇਲ ਸਪੈਕਟਰੋਗ੍ਰਾਮ ਨੂੰ ਪ੍ਰੋਸੈਸ ਕਰਦਾ ਹੈ ਅਤੇ ਹਿਡਨ ਸਟੇਟ ਸਾਥੇ ਕ੍ਰਾਸ-ਐਟੈਂਸ਼ਨ ਕੀ/ਵੈਲਿਊ ਟੈਂਸਰ ਨਿਕਾਲਦਾ ਹੈ
4. **ਡਿਕੋਡਰ** ਔਟੋਰੇਗ੍ਰੈਸੀਵਲ ਚਲਦਾ ਹੈ, ਇੱਕ ਵਾਰੀ ਇੱਕ ਟੋਕਨ ਬਣਾਉਂਦਾ ਹੈ ਜਦ ਤੱਕ ਅੰਤ-ਟੈਕਸਟ ਟੋਕਨ ਨਹੀਂ ਮਿਲਦਾ
5. **ਟੋਕਨਾਈਜ਼ਰ** ਆਉਟਪੁਟ ਟੋਕਨ IDs ਨੂੰ ਵਾਪਸ ਪੜ੍ਹਨਯੋਗ ਟੈਕਸਟ ਵਿੱਚ ਡਿਕੋਡ ਕਰਦਾ ਹੈ

### ਵਿਸਪਰ ਮਾਡਲ ਵੈਰੀਅੰਟ

| ਉਪਨਾਮ | ਮਾਡਲ ID | ਡਿਵਾਈਸ | ਆਕਾਰ | ਵੇਰਵਾ |
|-------|----------|--------|------|-------------|
| `whisper-medium` | `openai-whisper-medium-cuda-gpu:1` | GPU | 1.53 GB | GPU-ਸਹਾਇਤ (CUDA) |
| `whisper-medium` | `openai-whisper-medium-generic-cpu:1` | CPU | 3.05 GB | CPU ਲਈ ਢਾਲੂ (ਜ਼ਿਆਦਾਤਰ ਡਿਵਾਈਸਾਂ ਲਈ ਸੁਝਾਏ ਗਏ) |

> **ਨੋਟ:** ਚੈਟ ਮਾਡਲਾਂ ਦੇ ਵਿਰੋਧ ਵਿੱਚ ਜੋ ਡਿਫਾਲਟ ਤੌਰ ਤੇ ਸੂਚੀਕ੍ਰਿਤ ਹੁੰਦੇ ਹਨ, ਵਿਸਪਰ ਮਾਡਲਾਂ ਨੂੰ `automatic-speech-recognition` ਟਾਸਕ ਹੇਠ ਕੈਟੇਗਰਾਈਜ਼ ਕੀਤਾ ਗਿਆ ਹੈ। ਵੇਰਵਾ ਵੇਖਣ ਲਈ `foundry model info whisper-medium` ਵਰਤੋਂ।

---

## ਲੈਬ ਪ੍ਰਯੋਗ

### ਪ੍ਰਯੋਗ 0 - ਸੈਂਪਲ ਆਡੀਓ ਫਾਈਲਾਂ ਪ੍ਰਾਪਤ ਕਰੋ

ਇਸ ਲੈਬ ਵਿੱਚ Zava DIY ਉਤਪਾਦ ਸਥਿਤੀਆਂ ਵਿੱਚ ਅਧਾਰਿਤ ਤਿਆਰ ਕੀਤੇ WAV ਫਾਈਲਾਂ ਹਨ। ਸ਼ਾਮਲ ਸਕ੍ਰਿਪਟ ਨਾਲ ਇਹ ਬਣਾਓ:

```bash
# ਰਿਪੋ ਰੂਟ ਤੋਂ - ਪਹਿਲਾਂ ਇੱਕ .venv ਬਣਾਓ ਅਤੇ ਸਖਿਆ ਕਰੋ
python -m venv .venv

# ਵਿੰਡੋਜ਼ (ਪਾਵਰਸ਼ੈੱਲ):
.venv\Scripts\Activate.ps1
# ਮੈਕਓਐਸ:
source .venv/bin/activate

pip install openai
python samples/audio/generate_samples.py
```

ਇਸ ਨਾਲ `samples/audio/` ਵਿੱਚ ਛੇ WAV ਫਾਈਲਾਂ ਬਣਦੀਆਂ ਹਨ:

| ਫਾਈਲ | ਸਥਿਤੀ |
|------|----------|
| `zava-customer-inquiry.wav` | ਗਾਹਕ **Zava ProGrip Cordless Drill** ਬਾਰੇ ਪੁੱਛਦਾ ਹੈ |
| `zava-product-review.wav` | ਗਾਹਕ **Zava UltraSmooth Interior Paint** ਦੀ ਸਮੀਖਿਆ ਕਰਦਾ ਹੈ |
| `zava-support-call.wav` | ਸਹਾਇਤਾ ਕਾਲ ਬਾਰੇ **Zava TitanLock Tool Chest** |
| `zava-project-planning.wav` | DIYਰ ਵਰਗਾ ਡੈੱਕ ਯੋਜਨਾ ਬਣਾਣਾ **Zava EcoBoard Composite Decking** ਨਾਲ |
| `zava-workshop-setup.wav` | ਇੱਕ ਵਰਕਸ਼ਾਪ ਦੀ ਵਾਕਥਰੂ ਜਿਸ ਵਿੱਚ **ਪੰਜਵੇਂ Zava ਉਤਪਾਦ** ਵਰਤੇ ਗਏ |
| `zava-full-project-walkthrough.wav` | ਲੰਮੀ ਗੈਰਾਜ ਨਵੀਨਤਾ ਵਾਕਥਰੂ ਜਿਸ ਵਿੱਚ **ਸਾਰੇ Zava ਉਤਪਾਦ** (~4 ਮਿੰਟ, ਲੰਬੇ ਆਡੀਓ ਟੈਸਟ ਲਈ) |

> **ਟਿਪ:** ਤੁਸੀਂ ਆਪਣੀਆਂ WAV/MP3/M4A ਫਾਈਲਾਂ ਦਾ ਵੀ ਇਸਤਮਾਲ ਕਰ ਸਕਦੇ ਹੋ, ਜਾਂ Windows Voice Recorder ਨਾਲ ਖੁਦ ਨੂੰ ਰਿਕਾਰਡ ਕਰੋ।

---

### ਪ੍ਰਯੋਗ 1 - SDK ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਵਿਸਪਰ ਮਾਡਲ ਡਾਊਨਲੋਡ ਕਰੋ

ਨਵੀਆਂ ਫਾਊਂਡਰੀ ਲੋਕਲ ਵਰਜ਼ਨਾਂ ਵਿੱਚ ਵਿਸਪਰ ਮਾਡਲਾਂ ਨਾਲ CLI ਅਸੰਗਤਤਾ ਕਾਰਨ, ਮਾਡਲ ਡਾਊਨਲੋਡ ਕਰਨ ਅਤੇ ਲੋਡ ਕਰਨ ਲਈ **SDK** ਦਾ ਇਸਤਮਾਲ ਕਰੋ। ਆਪਣੀ ਭਾਸ਼ਾ ਚੁਣੋ:

<details>
<summary><b>🐍 Python</b></summary>

**SDK ਇੰਸਟਾਲ ਕਰੋ:**
```bash
pip install foundry-local-sdk
```

```python
from foundry_local import FoundryLocalManager

alias = "whisper-medium"

# ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ
manager = FoundryLocalManager()
manager.start_service()

# ਕੇਟਾਲੌਗ ਜਾਣਕਾਰੀ ਜਾਂਚੋ
info = manager.get_model_info(alias)
print(f"Model: {info.id}")
print(f"Task:  {info.task}")

# ਚੈੱਕ ਕਰੋ ਕਿ ਪਹਿਲਾਂ ਹੀ ਕੈਸ਼ ਕੀਤੀ ਗਈ ਹੈ
cached = manager.list_cached_models()
is_cached = any(m.id == info.id for m in cached) if info else False

if is_cached:
    print("Whisper model already downloaded.")
else:
    print("Downloading Whisper model (this may take several minutes)...")
    manager.download_model(alias)
    print("Download complete.")

# ਮਾਡਲ ਨੂੰ ਮੈਮੋਰੀ ਵਿੱਚ ਲੋਡ ਕਰੋ
manager.load_model(alias)
print(f"Whisper model loaded. Endpoint: {manager.endpoint}")
```

ਇਸ ਨੂੰ `download_whisper.py` ਵਜੋਂ ਸੰਭਾਲੋ ਅਤੇ ਚਲਾਓ:
```bash
python download_whisper.py
```

</details>

<details>
<summary><b>📘 JavaScript</b></summary>

**SDK ਇੰਸਟਾਲ ਕਰੋ:**
```bash
npm install foundry-local-sdk
```

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

const alias = "whisper-medium";

// ਮੈਨੇਜਰ ਬਣਾਓ ਅਤੇ ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ
FoundryLocalManager.create({ appName: "WhisperDemo" });
const manager = FoundryLocalManager.instance;
await manager.startWebService();

// ਕੈਟਾਲਾਗ ਤੋਂ ਮਾਡਲ ਲਵੋ
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

// ਮਾਡਲ ਨੂੰ ਯਾਦ ਵਿੱਚ ਲੋਡ ਕਰੋ
await model.load();
console.log(`Whisper model loaded. Service URL: ${manager.urls[0]}`);
```

ਇਸ ਨੂੰ `download-whisper.mjs` ਵਜੋਂ ਸੰਭਾਲੋ ਅਤੇ ਚਲਾਓ:
```bash
node download-whisper.mjs
```

</details>

<details>
<summary><b>💜 C#</b></summary>

**SDK ਇੰਸਟਾਲ ਕਰੋ:**
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

> **CLI ਦੀ ਥਾਂ SDK ਕਿਉਂ?** ਫਾਊਂਡਰੀ ਲੋਕਲ CLI ਵਿਸਪਰ ਮਾਡਲਾਂ ਨੂੰ ਸਿੱਧਾ ਡਾਊਨਲੋਡ ਜਾਂ ਸਰਵ ਕਰਨਾ ਸਹਿਯੋਗ ਨਹੀਂ ਕਰਦਾ। SDK ਆਡੀਓ ਮਾਡਲਾਂ ਨੂੰ ਪ੍ਰੋਗਰਾਮਿੰਗ ਰਾਹੀਂ ਡਾਊਨਲੋਡ ਅਤੇ ਮੈਨੇਜ ਕਰਨ ਦਾ ਭਰੋਸੇਯੋਗ ਤਰੀਕਾ ਦਿੰਦਾ ਹੈ। JavaScript ਅਤੇ C# SDKs ਵਿੱਚ ਬਿਲਟ-ਇਨ `AudioClient` ਹੁੰਦਾ ਹੈ ਜੋ ਪੂਰੀ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਪਾਈਪਲਾਈਨ ਨੂੰ ਸੰਭਾਲਦਾ ਹੈ। Python ONNX Runtime ਨਾਲ ਕੈਸ਼ ਕੀਤੀ ਮਾਡਲ ਫਾਈਲਾਂ ਉੱਤੇ ਡਾਇਰੈਕਟ ਇਨਫਰੇਂਸ ਲਈ ਵਰਤਦਾ ਹੈ।

---

### ਪ੍ਰਯੋਗ 2 - ਵਿਸਪਰ SDK ਨੂੰ ਸਮਝੋ

ਵਿਸਪਰ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਭਾਸ਼ਾ ਅਨੁਸਾਰ ਵੱਖ-ਵੱਖ ਤਰੀਕੇ ਵਰਤਦਾ ਹੈ। **JavaScript ਅਤੇ C#** ਵਿੱਚ ਫਾਊਂਡਰੀ ਲੋਕਲ SDK ਇੱਕ ਬਿਲਟ-ਇਨ `AudioClient` ਮੁਹੱਈਆ ਕਰਦਾ ਹੈ ਜੋ ਪੂਰੀ ਪਾਈਪਲਾਈਨ (ਆਡੀਓ ਪ੍ਰੀਪ੍ਰੋਸੈਸਿੰਗ, ਐਂਕੋਡਰ, ਡਿਕੋਡਰ, ਟੋਕਨ ਡਿਕੋਡਿੰਗ) ਨੂੰ ਇੱਕ ਸਿੰਗਲ ਮੈਥਡ ਕਾਲ ਵਿੱਚ ਸੰਭਾਲਦਾ ਹੈ। **Python** ਫਾਊਂਡਰੀ ਲੋਕਲ SDK ਨੂੰ ਮਾਡਲ ਪ੍ਰਬੰਧਨ ਲਈ ਅਤੇ ਐਂਕੋਡਰ/ਡਿਕੋਡਰ ONNX ਮਾਡਲਾਂ ਉਤੇ ਡਾਇਰੈਕਟ ਇਨਫਰੇਂਸ ਲਈ ONNX Runtime ਨੂੰ ਵਰਤਦਾ ਹੈ।

| ਭਾਗ | Python | JavaScript | C# |
|-----------|--------|------------|----|
| **SDK ਪੈਕੇਜ** | `foundry-local-sdk`, `onnxruntime`, `transformers`, `librosa` | `foundry-local-sdk` | `Microsoft.AI.Foundry.Local` |
| **ਮਾਡਲ ਪ੍ਰਬੰਧਨ** | `FoundryLocalManager(alias)` | `FoundryLocalManager.create()` + `catalog.getModel()` | `FoundryLocalManager.CreateAsync()` + catalog |
| **ਫੀਚਰ ਐਕਸਟ੍ਰੈਕਸ਼ਨ** | `WhisperFeatureExtractor` + `librosa` | SDK ਦਾ `AudioClient` ਸੰਭਾਲਦਾ ਹੈ | SDK ਦਾ `AudioClient` ਸੰਭਾਲਦਾ ਹੈ |
| **ਇਨਫਰੇਂਸ** | `ort.InferenceSession` (ਐਂਕੋਡਰ + ਡਿਕੋਡਰ) | `audioClient.transcribe()` | `audioClient.TranscribeAudioAsync()` |
| **ਟੋਕਨ ਡਿਕੋਡਿੰਗ** | `WhisperTokenizer` | SDK ਦਾ `AudioClient` ਸੰਭਾਲਦਾ ਹੈ | SDK ਦਾ `AudioClient` ਸੰਭਾਲਦਾ ਹੈ |
| **ਭਾਸ਼ਾ ਸੈਟਿੰਗ** | ਡਿਕੋਡਰ ਟੋਕਨ ਵਿੱਚ `forced_ids` ਦੇ ਨਾਲ | `audioClient.settings.language = "en"` | `audioClient.Settings.Language = "en"` |
| **ਇੰਪੁਟ** | WAV ਫਾਈਲ ਰਾਹ | WAV ਫਾਈਲ ਰਾਹ | WAV ਫਾਈਲ ਰਾਹ |
| **ਆਉਟਪੁਟ** | ਡਿਕੋਡ ਕੀਤਾ ਹੋਇਆ ਟੈਕਸਟ ਸਟ੍ਰਿੰਗ | `result.text` | `result.Text` |

> **ਜ਼ਰੂਰੀ:** ਹਮੇਸ਼ਾਂ `AudioClient` 'ਤੇ ਭਾਸ਼ਾ ਸੈੱਟ ਕਰੋ (ਉਦਾਹਰਨ: ਅੰਗਰੇਜ਼ੀ ਲਈ `"en"`). ਬਿਨਾ ਸਪਸ਼ਟ ਭਾਸ਼ਾ ਸੈੱਟਿੰਗ ਦੇ, ਮਾਡਲ ਗਲਤ ਨਤੀਜੇ ਦੇ ਸਕਦਾ ਹੈ ਕਿਉਂਕਿ ਇਹ ਭਾਸ਼ਾ ਆਟੋ-ਡੇਟੈਕਟ ਕਰਨ ਦੀ ਕੋਸ਼ਿਸ਼ ਕਰਦਾ ਹੈ।

> **SDK ਪੈਟਰਨ:** Python `FoundryLocalManager(alias)` ਦੀ ਵਰਤੋਂ ਕਰਦਾ ਹੈ ਬੂਟਸਟ੍ਰੈਪ ਲਈ, ਫਿਰ `get_cache_location()` ਨਾਲ ONNX ਮਾਡਲ ਫਾਈਲਾਂ ਲੱਭਦਾ ਹੈ। JavaScript ਅਤੇ C# SDK ਦੇ ਬਿਲਟ-ਇਨ `AudioClient` ਨੂੰ ਵਰਤਦੇ ਹਨ — ਜੋ ਕਿ `model.createAudioClient()` (JS) ਜਾਂ `model.GetAudioClientAsync()` (C#) ਰਾਹੀਂ ਮਿਲਦਾ ਹੈ — ਜੋ ਸਾਰੀ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਪਾਈਪਲਾਈਨ ਸੰਭਾਲਦਾ ਹੈ। ਪੂਰੇ ਵੇਰਵੇ ਲਈ ਦੇਖੋ [ਭਾਗ 2: ਫਾਊਂਡਰੀ ਲੋਕਲ SDK ਡੀਪ ਡਾਈਵ](part2-foundry-local-sdk.md)।

---

### ਪ੍ਰਯੋਗ 3 - ਇੱਕ ਸਰਲ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਐਪ ਬਣਾਓ

ਆਪਣੀ ਭਾਸ਼ਾ ਚੁਣੋ ਅਤੇ ਇੱਕ ਛੋਟਾ ਐਪਲੀਕੇਸ਼ਨ ਬਣਾਉ ਜੋ ਆਡੀਓ ਫਾਈਲ ਦਾ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਕਰਦਾ ਹੋਵੇ।

> **ਸਹਿਯੋਗ ਪ੍ਰਾਪਤ ਆਡੀਓ ਫਾਰਮੈਟ:** WAV, MP3, M4A। ਸਭ ਤੋਂ ਵਧੀਆ ਨਤੀਜੇ ਲਈ 16kHz ਸੈਂਪਲ ਰੇਟ ਵਾਲੀਆਂ WAV ਫਾਈਲਾਂ ਵਰਤੋਂ।

<details>
<summary><h3>Python ਟ੍ਰੈਕ</h3></summary>

#### ਸੈਟਅਪ

```bash
cd python
python -m venv venv

# ਵਰਚੁਅਲ ਵਾਤਾਵਰਣ ਨੂੰ ਸਰਗਰਮ ਕਰੋ:
# ਵਿਂਡੋਜ਼ (ਪਾਵਰਸ਼ੈੱਲ):
venv\Scripts\Activate.ps1
# ਮੈਕਓਐਸ:
source venv/bin/activate

pip install foundry-local-sdk onnxruntime transformers librosa
```

#### ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਕੋਡ

ਇੱਕ ਫਾਈਲ ਬਣਾਓ `foundry-local-whisper.py`:

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

# ਕਦਮ 1: ਬੂਟਸਟ੍ਰੈਪ - ਸੇਵਾ ਸ਼ੁਰੂ ਕਰਦਾ ਹੈ, ਡਾਊਨਲੋਡ ਕਰਦਾ ਹੈ, ਅਤੇ ਮਾਡਲ ਲੋਡ ਕਰਦਾ ਹੈ
print(f"Initialising Foundry Local with model: {model_alias}...")
manager = FoundryLocalManager(model_alias)
model_info = manager.get_model_info(model_alias)
cache_location = manager.get_cache_location()

# ਕੈਸ਼ਡ ONNX ਮਾਡਲ ਫਾਇਲਾਂ ਦਾ ਪਾਥ ਬਣਾਓ
model_dir = os.path.join(
    cache_location, "Microsoft",
    model_info.id.replace(":", "-"), "cpu-fp32"
)

# ਕਦਮ 2: ONNX ਸੈਸ਼ਨਾਂ ਅਤੇ ਫੀਚਰ ਐਕਸਟ੍ਰੈਕਟਰ ਲੋਡ ਕਰੋ
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

# ਕਦਮ 3: ਮੇਲ ਸਪੈਕਟੋਗਰਾਮ ਫੀਚਰ ਨਿਕਾਲੋ
audio, _ = librosa.load(audio_file, sr=16000)
features = fe(audio, sampling_rate=16000, return_tensors="np")
input_features = features.input_features.astype(np.float32)

# ਕਦਮ 4: ਐਂਕੋਡਰ ਚਲਾਓ
enc_out = encoder.run(None, {"audio_features": input_features})
# ਪਹਿਲਾ ਆਉਟਪੁੱਟ ਲੁਕੇ ਹੋਏ ਸਟੇਟ ਹਨ; ਬਾਕੀ ਕ੍ਰਾਸ-ਅਟੈਂਸ਼ਨ KV ਜੋੜੇ ਹਨ
cross_kv = {
    f"past_key_cross_{i}": enc_out[1 + 2 * i]
    for i in range(24)
}
cross_kv.update({
    f"past_value_cross_{i}": enc_out[2 + 2 * i]
    for i in range(24)
})

# ਕਦਮ 5: ਆਟੋਰੇਗ੍ਰੈਸੀਵ ਡਿਕੋਡਿੰਗ
initial_tokens = [50258, 50259, 50359, 50363]  # sot, en, ਟ੍ਰਾਂਸਕ੍ਰਾਈਬ, ਨੋਟਾਈਮਸਟੈਂਪਸ
input_ids = np.array([initial_tokens], dtype=np.int32)

# ਖਾਲੀ ਸੈਲਫ-ਅਟੈਂਸ਼ਨ KV ਕੈਸ਼
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

    if next_token == 50257:  # ਟੈਕਸਟ ਦਾ ਅੰਤ
        break
    generated.append(next_token)

    # ਸੈਲਫ-ਅਟੈਂਸ਼ਨ KV ਕੈਸ਼ ਅੱਪਡੇਟ ਕਰੋ
    for i in range(24):
        self_kv[f"past_key_self_{i}"] = outputs[1 + 2 * i]
        self_kv[f"past_value_self_{i}"] = outputs[2 + 2 * i]
    input_ids = np.array([[next_token]], dtype=np.int32)

print(tokenizer.decode(generated, skip_special_tokens=True))
```

#### ਚਲਾਓ

```bash
# ਜ਼ਾਵਾ ਉਤਪਾਦ ਦ੍ਰਿਸ਼ ਨੂੰ ਲਿਖੋ
python foundry-local-whisper.py ../samples/audio/zava-customer-inquiry.wav

# ਜਾਂ ਹੋਰ ਕੋਸ਼ਿਸ਼ ਕਰੋ:
python foundry-local-whisper.py ../samples/audio/zava-product-review.wav
python foundry-local-whisper.py ../samples/audio/zava-workshop-setup.wav
```

#### ਮੁੱਖ Python ਬਿੰਦੂ

| ਮੈਥਡ | ਮਕਸਦ |
|--------|---------|
| `FoundryLocalManager(alias)` | ਬੂਟਸਟ੍ਰੈਪ: ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ, ਡਾਊਨਲੋਡ ਕਰੋ, ਅਤੇ ਮਾਡਲ ਲੋਡ ਕਰੋ |
| `manager.get_cache_location()` | ਕੈਸ਼ ਕੀਤੀਆਂ ONNX ਮਾਡਲ ਫਾਈਲਾਂ ਲਈ ਰਾਹ ਪ੍ਰਾਪਤ ਕਰੋ |
| `WhisperFeatureExtractor.from_pretrained()` | ਮੇਲ ਸਪੈਕਟਰੋਗ੍ਰਾਮ ਫੀਚਰ ਐਕਸਟ੍ਰੈਕਟਰ ਲੋਡ ਕਰੋ |
| `ort.InferenceSession()` | ਐਂਕੋਡਰ ਅਤੇ ਡਿਕੋਡਰ ਲਈ ONNX Runtime ਸੈਸ਼ਨ ਬਣਾਓ |
| `tokenizer.decode()` | ਆਉਟਪੁਟ ਟੋਕਨ IDs ਨੂੰ ਮੁੜ ਪੜ੍ਹਨਯੋਗ ਟੈਕਸਟ ਵਿੱਚ ਬਦਲੋ |

</details>

<details>
<summary><h3>JavaScript ਟ੍ਰੈਕ</h3></summary>

#### ਸੈਟਅਪ

```bash
cd javascript
npm install foundry-local-sdk onnxruntime-node
```

#### ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਕੋਡ

ਇੱਕ ਫਾਈਲ ਬਣਾਓ `foundry-local-whisper.mjs`:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";
import fs from "node:fs";

const modelAlias = "whisper-medium";
const audioFile = process.argv[2] || "sample.wav";

if (!fs.existsSync(audioFile)) {
  console.error(`Audio file not found: ${audioFile}`);
  process.exit(1);
}

// ਕਦਮ 1: ਬੂਟਸਟ੍ਰੈਪ - ਮੈਨੇਜਰ ਬਣਾਓ, ਸੇਵਾ ਸ਼ੁਰੂ ਕਰੋ, ਅਤੇ ਮਾਡਲ ਲੋਡ ਕਰੋ
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

// ਕਦਮ 2: ਇੱਕ ਆਡੀਓ ਕਲਾਇੰਟ ਬਣਾਓ ਅਤੇ ਟ੍ਰਾਂਸਕ੍ਰਾਈਬ ਕਰੋ
const audioClient = model.createAudioClient();
audioClient.settings.language = "en";

console.log(`Transcribing: ${audioFile}`);
const result = await audioClient.transcribe(audioFile);

console.log("\n--- Transcription ---");
console.log(result.text);
console.log("---------------------");

// ਸਾਫ਼-ਸਫਾਈ
await model.unload();
```

> **ਨੋਟ:** ਫਾਊਂਡਰੀ ਲੋਕਲ SDK `model.createAudioClient()` ਰਾਹੀਂ ਇੱਕ ਬਿਲਟ-ਇਨ `AudioClient` ਮੁਹੱਈਆ ਕਰਦਾ ਹੈ ਜੋ ਪੂਰੀ ONNX ਇਨਫਰੇਂਸ ਪਾਈਪਲਾਈਨ ਨੂੰ ਅੰਦਰੂਨੀ ਤੌਰ 'ਤੇ ਸੰਭਾਲਦਾ ਹੈ — ਕਿਸੇ `onnxruntime-node` ਇੰਪੋਰਟ ਦੀ ਲੋੜ ਨਹੀਂ। ਹਮੇਸ਼ਾਂ `audioClient.settings.language = "en"` ਸੈੱਟ ਕਰੋ ਤਾਂ ਜੋ ਅੰਗਰੇਜ਼ੀ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਸਹੀ ਹੋਵੇ।

#### ਚਲਾਓ

```bash
# ਇੱਕ ਜਾਵਾ ਉਤਪਾਦ ਸਥਿਤੀ ਲਿਖੋ
node foundry-local-whisper.mjs ../samples/audio/zava-customer-inquiry.wav

# ਜਾਂ ਹੋਰ ਕੋਸ਼ਿਸ़ਸ਼ ਕਰੋ:
node foundry-local-whisper.mjs ../samples/audio/zava-support-call.wav
node foundry-local-whisper.mjs ../samples/audio/zava-project-planning.wav
```

#### ਮੁੱਖ JavaScript ਬਿੰਦੂ

| ਮੈਥਡ | ਮਕਸਦ |
|--------|---------|
| `FoundryLocalManager.create({ appName })` | ਮੈਨੇਜਰ ਸਿੰਗਲਟਨ ਬਣਾਓ |
| `await catalog.getModel(alias)` | ਕੈਟਾਲੌਗ ਤੋਂ ਮਾਡਲ ਹਾਸਲ ਕਰੋ |
| `model.download()` / `model.load()` | ਵਿਸਪਰ ਮਾਡਲ ਡਾਊਨਲੋਡ ਅਤੇ ਲੋਡ ਕਰੋ |
| `model.createAudioClient()` | ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਲਈ ਆਡੀਓ ਕਲੀਐਂਟ ਬਣਾਓ |
| `audioClient.settings.language = "en"` | ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਭਾਸ਼ਾ ਸੈੱਟ ਕਰੋ (ਨਤੀਜੇ ਲਈ ਜ਼ਰੂਰੀ) |
| `audioClient.transcribe(path)` | ਆਡੀਓ ਫਾਈਲ ਦਾ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਕਰੋ, ਵਾਪਸੀ `{ text, duration }` |

</details>

<details>
<summary><h3>C# ਟ੍ਰੈਕ</h3></summary>

#### ਸੈਟਅਪ

```bash
mkdir whisper-demo
cd whisper-demo
dotnet new console --framework net9.0
dotnet add package Microsoft.AI.Foundry.Local
```

> **ਨੋਟ:** C# ਟ੍ਰੈਕ `Microsoft.AI.Foundry.Local` ਪੈਕੇਜ ਵਰਤਦਾ ਹੈ ਜੋ ਬਿਲਟ-ਇਨ `AudioClient` ਪ੍ਰਦਾਨ ਕਰਦਾ ਹੈ `model.GetAudioClientAsync()` ਰਾਹੀਂ। ਇਹ ਸਾਰੀ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਪਾਈਪਲਾਈਨ ਨੂੰ ਇਨ-ਪ੍ਰੋਸੇਸ ਸੰਭਾਲਦਾ ਹੈ — ਕੋਈ ਵੱਖਰਾ ONNX Runtime ਸੈਟਅਪ ਲੋੜੀਂਦਾ ਨਹੀਂ।

#### ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਕੋਡ

`Program.cs` ਦੀ ਸਮੱਗਰੀ ਬਦਲੋ:

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

#### ਚਲਾਓ

```bash
# ਜਾਵਾ ਉਤਪਾਦ ਸਥਿਤੀ ਨੂੰ ਟ੍ਰਾਂਸਕ੍ਰਾਈਬ ਕਰੋ
dotnet run -- ..\samples\audio\zava-customer-inquiry.wav

# ਜਾਂ ਹੋਰ ਕੋਸ਼ਿਸ਼ ਕਰੋ:
dotnet run -- ..\samples\audio\zava-product-review.wav
dotnet run -- ..\samples\audio\zava-workshop-setup.wav
```

#### ਮੁੱਖ C# ਬਿੰਦੂ

| ਮੈਥਡ | ਮਕਸਦ |
|--------|---------|
| `FoundryLocalManager.CreateAsync(config)` | ਫਾਊਂਡਰੀ ਲੋਕਲ ਕੋਨਫਿਗਰ ਕਰਕੇ ਸ਼ੁਰੂ ਕਰੋ |
| `catalog.GetModelAsync(alias)` | ਕੈਟਾਲੌਗ ਤੋਂ ਮਾਡਲ ਪ੍ਰਾਪਤ ਕਰੋ |
| `model.DownloadAsync()` | ਵਿਸਪਰ ਮਾਡਲ ਡਾਊਨਲੋਡ ਕਰੋ |
| `model.GetAudioClientAsync()` | AudioClient ਲਵੋ (ChatClient ਨਹੀਂ!) |
| `audioClient.Settings.Language = "en"` | ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਭਾਸ਼ਾ ਸੈੱਟ ਕਰੋ (ਸਹੀ ਨਤੀਜੇ ਲਈ ਜ਼ਰੂਰੀ) |
| `audioClient.TranscribeAudioAsync(path)` | ਆਡੀਓ ਫਾਈਲ ਟ੍ਰਾਂਸਕ੍ਰਾਈਬ ਕਰੋ |
| `result.Text` | ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕੀਤਾ ਹੋਇਆ ਟੈਕਸਟ |
> **C# ਵਿਰੀ Python/JS:** C# SDK ਇੱਕ ਅੰਦਰੂਨੀ `AudioClient` ਮੁਹੱਈਆ ਕਰਵਾਉਂਦਾ ਹੈ ਜੋ `model.GetAudioClientAsync()` ਰਾਹੀਂ ਪ੍ਰਕਿਰਿਆ ਅੰਦਰ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਲਈ ਹੈ, ਜੋ JavaScript SDK ਵਾਂਗ ਹੈ। Python ਸਿੱਧਾ ONNX Runtime ਵਰਤਦਾ ਹੈ ਜੋ ਕੈਸ਼ ਕੀਤੇ ਇੰਕੋਡਰ/ਡਿਕੋਡਰ ਮਾਡਲਾਂ ਮੂਹੰਮਦਾ ਹੈ।

</details>

---

### ਮੁਸ਼ਕਲ 4 - ਸਾਰੇ Zava ਸੈਂਪਲਾਂ ਦੀ ਬੈਚ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਕਰੋ

ਹੁਣ ਜਦੋਂ ਤੁਹਾਡੇ ਕੋਲ ਇੱਕ ਕੰਮ ਕਰਨ ਵਾਲਾ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਐਪ ਹੈ, ਸਾਰੇ ਪੰਜ Zava ਸੈਂਪਲ ਫਾਈਲਾਂ ਨੂੰ ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕਰੋ ਅਤੇ ਨਤੀਜੇ ਤੁਲਨਾ ਕਰੋ।

<details>
<summary><h3>Python ਟ੍ਰੈਕ</h3></summary>

ਪੂਰਾ ਸੈਂਪਲ `python/foundry-local-whisper.py` ਪਹਿਲਾਂ ਹੀ ਬੈਚ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨਦਾ ਸਹਿਯੋਗ ਕਰਦਾ ਹੈ। ਜਦੋਂ ਇਸਨੂੰ ਬਿਨਾਂ ਕਿਸੇ ਆਰਗੁਮੈਂਟ ਦੇ ਚਲਾਇਆ ਜਾਂਦਾ ਹੈ, ਇਹ ਸਾਰੇ `zava-*.wav` ਫਾਈਲਾਂ ਨੂੰ `samples/audio/` ਵਿੱਚ ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕਰਦਾ ਹੈ:

```bash
cd python
python foundry-local-whisper.py
```

ਸੈਂਪਲ `FoundryLocalManager(alias)` ਨੂੰ ਬੂਟਸਟ੍ਰੈਪ ਲਈ ਵਰਤਦਾ ਹੈ, ਫਿਰ ਹਰ ਫਾਈਲ ਲਈ ਇੰਕੋਡਰ ਅਤੇ ਡਿਕੋਡਰ ONNX ਸੈਸ਼ਨ ਦੌੜਾਉਂਦਾ ਹੈ।

</details>

<details>
<summary><h3>JavaScript ਟ੍ਰੈਕ</h3></summary>

ਪੂਰਾ ਸੈਂਪਲ `javascript/foundry-local-whisper.mjs` ਪਹਿਲਾਂ ਹੀ ਬੈਚ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਸਹਿਯੋਗ ਕਰਦਾ ਹੈ। ਜਦੋਂ ਇਸਨੂੰ ਬਿਨਾਂ ਕਿਸੇ ਆਰਗੁਮੈਂਟ ਦੇ ਚਲਾਇਆ ਜਾਂਦਾ ਹੈ, ਇਹ ਸਾਰੇ `zava-*.wav` ਫਾਈਲਾਂ ਨੂੰ `samples/audio/` ਵਿੱਚ ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕਰਦਾ ਹੈ:

```bash
cd javascript
node foundry-local-whisper.mjs
```

ਸੈਂਪਲ `FoundryLocalManager.create()` ਅਤੇ `catalog.getModel(alias)` ਨੂੰ SDK ਸ਼ੁਰੂ ਕਰਨ ਲਈ ਵਰਤਦਾ ਹੈ, ਫਿਰ `AudioClient` (`settings.language = "en"` ਨਾਲ) ਨੂੰ ਵਰਤ ਕੇ ਹਰ ਫਾਈਲ ਨੂੰ ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕਰਦਾ ਹੈ।

</details>

<details>
<summary><h3>C# ਟ੍ਰੈਕ</h3></summary>

ਪੂਰਾ ਸੈਂਪਲ `csharp/WhisperTranscription.cs` ਪਹਿਲਾਂ ਹੀ ਬੈਚ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਸਹਿਯੋਗ ਕਰਦਾ ਹੈ। ਜਦੋਂ ਕਿਸੇ ਖਾਸ ਫਾਈਲ ਦੇ ਆਰਗੁਮੈਂਟ ਦੇ ਬਿਨਾਂ ਚਲਾਇਆ ਜਾਂਦਾ ਹੈ, ਇਹ ਸਾਰੇ `zava-*.wav` ਫਾਈਲਾਂ ਨੂੰ `samples/audio/` ਵਿੱਚ ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕਰਦਾ ਹੈ:

```bash
cd csharp
dotnet run whisper
```

ਸੈਂਪਲ `FoundryLocalManager.CreateAsync()` ਅਤੇ SDK ਦਾ `AudioClient` (`Settings.Language = "en"` ਨਾਲ) ਪ੍ਰਕਿਰਿਆ ਅੰਦਰ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਲਈ ਵਰਤਦਾ ਹੈ।

</details>

**ਦੇਖਣ ਲਈ ਕੀ ਹੈ:** ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਆਉਟਪੁੱਟ ਨੂੰ ਮੂਲ ਲਿਖਤ ਨਾਲ `samples/audio/generate_samples.py` ਵਿੱਚ ਤੁਲਨਾ ਕਰੋ। Whisper ਕਿੰਨੀ ਸਹੀ ਤਰ੍ਹਾਂ "Zava ProGrip" ਵਰਗੇ ਉਤਪਾਦ ਨਾਮਾਂ ਅਤੇ "brushless motor" ਜਾਂ "composite decking" ਵਰਗੇ ਤਕਨੀਕੀ ਸ਼ਬਦਾਂ ਨੂੰ ਕੈਪਚਰ ਕਰਦਾ ਹੈ?

---

### ਮੁਸ਼ਕਲ 5 - ਮੁੱਖ ਕੋਡ ਪੈਟਰਨਾਂ ਨੂੰ ਸਮਝੋ

ਧਿਆਨ ਨਾਲ ਦੇਖੋ ਕਿ Whisper ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਤਿੰਨੋਂ ਭਾਸ਼ਾਵਾਂ ਵਿੱਚ ਚੈਟ ਕੌਮਪਲੀਸ਼ਨਾਂ ਤੋਂ ਕਿਵੇਂ ਵੱਖਰਾ ਹੈ:

<details>
<summary><b>Python - ਚੈਟ ਤੋਂ ਮੁੱਖ ਅੰਤਰ</b></summary>

```python
# ਚੈਟ ਪੂਰਾ ਕਰਨ ਦੀ ਪ੍ਰਕਿਰਿਆ (ਭਾਗ 2-6):
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
stream = client.chat.completions.create(
    model=model_id,
    messages=[{"role": "user", "content": "Hello"}],
    stream=True,
)

# ਆਡੀਓ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ (ਇਹ ਭਾਗ):
# OpenAI ਕਲਾਇੰਟ ਦੀ ਬਜਾਏ ਸਿੱਧਾ ONNX Runtime ਦੀ ਵਰਤੋਂ ਕਰਦਾ ਹੈ
encoder = ort.InferenceSession(encoder_path, providers=["CPUExecutionProvider"])
decoder = ort.InferenceSession(decoder_path, providers=["CPUExecutionProvider"])

audio, _ = librosa.load("audio.wav", sr=16000)
features = feature_extractor(audio, sampling_rate=16000, return_tensors="np")
enc_out = encoder.run(None, {"audio_features": features.input_features})
# ... ਆਟੋਰੇਗ੍ਰੈਸੀਵ ਡੀਕੋਡਰ ਲੂਪ ...
print(tokenizer.decode(generated_tokens))
```

**ਮੁੱਖ ਜਾਣਕਾਰੀ:** ਚੈਟ ਮਾਡਲ OpenAI-ਅਨੁਕੂਲ API ਰਾਹੀਂ `manager.endpoint` ਦੀ ਵਰਤੋਂ ਕਰਦੇ ਹਨ। Whisper SDK ਨੂੰ ਵਰਤਦਾ ਹੈ ਜੋ ਕੈਸ਼ ਕੀਤੇ ONNX ਮਾਡਲ ਫਾਈਲਾਂ ਲੱਭਦਾ ਹੈ, ਫਿਰ ONNX Runtime ਨਾਲ ਸਿੱਧਾ ਇੰਫ਼ਰੰਸ ਚਲਾਉਂਦਾ ਹੈ।

</details>

<details>
<summary><b>JavaScript - ਚੈਟ ਤੋਂ ਮੁੱਖ ਅੰਤਰ</b></summary>

```javascript
// ਚੈਟ ਪੂਰਨਤਾ (ਭਾਗ 2-6):
const client = new OpenAI({ baseURL: manager.urls[0] + "/v1", apiKey: "foundry-local" });
const stream = await client.chat.completions.create({
  model: model.id,
  messages: [{ role: "user", content: "Hello" }],
  stream: true,
});

// ਆਡੀਓ ਲਿਖਤਬੱਧ ਕਰਨਾ (ਇਹ ਭਾਗ):
// SDK ਦੇ ਬਿਲਟ-ਇਨ AudioClient ਦੀ ਵਰਤੋਂ ਕਰਦਾ ਹੈ
const audioClient = model.createAudioClient();
audioClient.settings.language = "en"; // ਸਭ ਤੋਂ ਵਧੀਆ ਨਤੀਜੇ ਲਈ ਹਮੇਸ਼ਾ ਭਾਸ਼ਾ ਸੈੱਟ ਕਰੋ
const result = await audioClient.transcribe("audio.wav");
console.log(result.text);
```

**ਮੁੱਖ ਜਾਣਕਾਰੀ:** ਚੈਟ ਮਾਡਲ OpenAI-ਅਨੁਕੂਲ API ਰਾਹੀਂ `manager.urls[0] + "/v1"` ਦੀ ਵਰਤੋਂ ਕਰਦੇ ਹਨ। Whisper ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ SDK ਦੇ `AudioClient` ਨੂੰ ਵਰਤਦਾ ਹੈ, ਜੋ `model.createAudioClient()` ਤੋਂ ਮਿਲਦਾ ਹੈ। ਗਲਤ ਨਤੀਜੇ ਤੋਂ ਬਚਣ ਲਈ `settings.language` ਸੈੱਟ ਕਰੋ।

</details>

<details>
<summary><b>C# - ਚੈਟ ਤੋਂ ਮੁੱਖ ਅੰਤਰ</b></summary>

C# ਤਰੀਕਾ SDK ਦਾ ਬਣਾਇਆ ਹੋਇਆ `AudioClient` ਵਰਤਦਾ ਹੈ ਜੋ ਪ੍ਰਕਿਰਿਆ ਅੰਦਰ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਲਈ ਹੈ:

**ਮਾਡਲ ਸ਼ੁਰੂਆਤ:**

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

**ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ:**

```csharp
// Get the audio client (not a chat client!)
var audioClient = await model.GetAudioClientAsync();
audioClient.Settings.Language = "en"; // Always set language for best results

// Transcribe - returns an object with a .Text property
var response = await audioClient.TranscribeAudioAsync(filePath);
Console.WriteLine(response.Text);
```

**ਮੁੱਖ ਜਾਣਕਾਰੀ:** C# `FoundryLocalManager.CreateAsync()` ਵਰਤਦਾ ਹੈ ਅਤੇ ਸਿੱਧਾ `AudioClient` ਪ੍ਰਾਪਤ ਕਰਦਾ ਹੈ — ਕਿਸੇ ONNX Runtime ਸੈਟਅਪ ਦੀ ਲੋੜ ਨਹੀਂ। ਗਲਤ ਨਤੀਜੇ ਤੋਂ ਬਚਣ ਲਈ `Settings.Language` ਸੈੱਟ ਕਰੋ।

</details>

> **ਸਾਰ:** Python ਮਾਡਲ ਪ੍ਰਬੰਧਨ ਲਈ Foundry Local SDK ਅਤੇ encoder/decoder ਮਾਡਲਾਂ ਫਿਰ ONNX Runtime ਨਾਲ ਸਿੱਧਾ ਇੰਫਰੰਸ ਲਈ ਵਰਤਦਾ ਹੈ। JavaScript ਅਤੇ C# ਦੋਹਾਂ SDK ਦੇ ਅੰਦਰੂਨੀ `AudioClient` ਨੂੰ ਵਰਤਦੇ ਹਨ ਜੋ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਨੁੰ ਸੌਖਾ ਕਰਦਾ ਹੈ — ਕਲਾਇੰਟ ਬਣਾਓ, ਭਾਸ਼ਾ ਸੈੱਟ ਕਰੋ, ਅਤੇ `transcribe()` / `TranscribeAudioAsync()` ਨੂੰ ਕਾਲ ਕਰੋ। ਸਹੀ ਨਤੀਜੇ ਲਈ ਹਮੇਸ਼ਾਂ AudioClient ’ਤੇ ਭਾਸ਼ਾ ਗੁਣ ਸੈੱਟ ਕਰੋ।

---

### ਮੁਸ਼ਕਲ 6 - ਅਨੁਭਵ

ਆਪਣੇ ਜਾਣੂ ਨੂੰ ਡੂੰਘਾ ਕਰਨ ਲਈ ਇਹ ਤਬਦੀਲੀਆਂ ਕੋਸ਼ਿਸ਼ ਕਰੋ:

1. **ਵੱਖ-ਵੱਖ ਆਡੀਓ ਫਾਈਲਾਂ ਕੋਸ਼ਿਸ਼ ਕਰੋ** - ਆਪਣੇ ਆਪ ਨੂੰ ਵੀਡੀਉ Windows Voice Recorder ਨਾਲ ਰਿਕਾਰਡ ਕਰੋ, WAV ਫਾਰਮੈਟ ਵਿੱਚ ਸੰਭਾਲੋ ਅਤੇ ਉਸਨੂੰ ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕਰੋ।

2. **ਮਾਡਲ ਵੈਰੀਅੰਟਾਂ ਦੀ ਤੁਲਨਾ ਕਰੋ** - ਜੇ ਤੁਹਾਡੇ ਕੋਲ NVIDIA GPU ਹੈ, CUDA ਵੈਰੀਅੰਟ ਕੋਸ਼ਿਸ਼ ਕਰੋ:
   ```bash
   foundry model download whisper-medium --device GPU
   ```
   ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਗਤੀ ਨੂੰ CPU ਵੈਰੀਅੰਟ ਨਾਲ ਤੁਲਨਾ ਕਰੋ।

3. **ਆਉਟਪੁੱਟ ਫਾਰਮੈਟਿੰਗ ਸ਼ਾਮਿਲ ਕਰੋ** - JSON ਜਵਾਬ ਵਿੱਚ ਇਹ ਸ਼ਾਮਿਲ ਹੋ ਸਕਦਾ ਹੈ:
   ```json
   {
     "text": "Welcome to Zava Home Improvement. I'd like to learn more about the ProGrip Cordless Drill.",
     "language": "en",
     "duration": 10.5
   }
   ```

4. **REST API ਬਣਾਓ** - ਆਪਣਾ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਕੋਡ ਇੱਕ ਵੈੱਬ ਸਰਵਰ ਵਿੱਚ ਲਪੇਟੋ:

   | ਭਾਸ਼ਾ | ਫ੍ਰੇਮਵਰਕ | ਉਦਾਹਰਨ |
   |--------|----------|---------|
   | Python | FastAPI | `@app.post("/v1/audio/transcriptions")` ਨਾਲ `UploadFile` |
   | JavaScript | Express.js | `app.post("/v1/audio/transcriptions")` ਨਾਲ `multer` |
   | C# | ASP.NET Minimal API | `app.MapPost("/v1/audio/transcriptions")` ਨਾਲ `IFormFile` |

5. **ਮਲਟੀ-ਟਰਨ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਨਾਲ** - ਭਾਗ 4 ਵਿੱਚੋਂ ਚੈਟ ਏਜੰਟ ਨਾਲ Whisper ਨੂੰ ਮਿਲਾਓ: ਪਹਿਲਾਂ ਆਡੀਓ ਨੂੰ ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕਰੋ, ਫਿਰ ਟੈਕਸਟ ਨੂੰ ਏਜੰਟ ਨੂੰ ਵਿਸ਼ਲੇਸ਼ਣ ਜਾਂ ਸਾਰ ਲਈ ਭੇਜੋ।

---

## SDK ਆਡੀਓ API ਸੰਦਰਭ

> **JavaScript AudioClient:**
> - `model.createAudioClient()` — ਇੱਕ `AudioClient` ਇੰਸਟੈਂਸ ਬਣਾਉਂਦਾ ਹੈ
> - `audioClient.settings.language` — ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਭਾਸ਼ਾ ਸੈੱਟ ਕਰੋ (ਜਿਵੇਂ `"en"`)
> - `audioClient.settings.temperature` — ਬੇਤ੍ਰਤੀ ਦਰ ਨੂੰ ਨਿਯੰਤਰਿਤ ਕਰੋ (ਵਿਕਲਪਿਕ)
> - `audioClient.transcribe(filePath)` — ਇੱਕ ਫਾਈਲ ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕਰੋ, `{ text, duration }` ਵਾਪਸ ਕਰਦਾ ਹੈ
> - `audioClient.transcribeStreaming(filePath, callback)` — ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਟੁਕੜੇ ਕਾਲਬੈਕ ਰਾਹੀਂ ਸਟ੍ਰੀਮ ਕਰੋ
>
> **C# AudioClient:**
> - `await model.GetAudioClientAsync()` — ਇੱਕ `OpenAIAudioClient` ਇੰਸਟੈਂਸ ਬਣਾਉਂਦਾ ਹੈ
> - `audioClient.Settings.Language` — ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਭਾਸ਼ਾ ਸੈੱਟ ਕਰੋ (ਜਿਵੇਂ `"en"`)
> - `audioClient.Settings.Temperature` — ਬੇਤ੍ਰਤੀ ਦਰ ਨੂੰ ਨਿਯੰਤਰਿਤ ਕਰੋ (ਵਿਕਲਪਿਕ)
> - `await audioClient.TranscribeAudioAsync(filePath)` — ਇੱਕ ਫਾਈਲ ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕਰੋ, `.Text` ਵਾਲਾ ਆਬਜੈਕਟ ਵਾਪਸ ਕਰਦਾ ਹੈ
> - `audioClient.TranscribeAudioStreamingAsync(filePath)` — ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਟੁਕੜਿਆਂ ਦਾ `IAsyncEnumerable` ਵਾਪਸ ਕਰਦਾ ਹੈ

> **ਟਿਪ:** ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕਰਨ ਤੋਂ ਪਹਿਲਾਂ ਹਮੇਸ਼ਾਂ ਭਾਸ਼ਾ ਗੁਣ ਸੈੱਟ ਕਰੋ। ਇਸਦੇ ਬਿਨਾਂ, Whisper ਮਾਡਲ ਆਟੋ-ਡਿਟੈਕਸ਼ਨ ਕਰਦਾ ਹੈ, ਜੋ ਗੜਬੜ ਵਾਲਾ ਨਤੀਜਾ ਦੇ ਸਕਦਾ ਹੈ (ਇੱਕ ਟੈਕਸਟ ਦੀ ਥਾਂ ਇੱਕ ਇਕੱਤਰ ਬਦਲ ਪ੍ਰਤੀਕ)।

---

## ਤੁਲਨਾ: ਚੈਟ ਮਾਡਲਾਂ ਵਿ. Whisper

| ਪਿਹਲੂ | ਚੈਟ ਮਾਡਲ (ਭਾਗ 3-7) | Whisper - Python | Whisper - JS / C# |
|-------|---------------------|-----------------|-------------------|
| **ਕਾਰਜ ਕਿਸਮ** | `chat` | `automatic-speech-recognition` | `automatic-speech-recognition` |
| **ਇਨਪੁੱਟ** | ਟੈਕਸਟ ਸੰਦਰਭ (JSON) | ਆਡੀਓ ਫਾਈਲਾਂ (WAV/MP3/M4A) | ਆਡੀਓ ਫਾਈਲਾਂ (WAV/MP3/M4A) |
| **ਆਉਟਪੁੱਟ** | ਬਣਾਇਆ ਟੈਕਸਟ (ਸਟ੍ਰੀਮ ਕੀਤਾ) | ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕੀਤਾ ਟੈਕਸਟ (ਸੰਪੂਰਨ) | ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕੀਤਾ ਟੈਕਸਟ (ਸੰਪੂਰਨ) |
| **SDK ਪੈਕੇਜ** | `openai` + `foundry-local-sdk` | `foundry-local-sdk` + `onnxruntime` | `foundry-local-sdk` (JS) / `Microsoft.AI.Foundry.Local` (C#) |
| **API ਢੰਗ** | `client.chat.completions.create()` | ONNX Runtime ਸਿੱਧਾ | `audioClient.transcribe()` (JS) / `audioClient.TranscribeAudioAsync()` (C#) |
| **ਭਾਸ਼ਾ ਸੈਟਿੰਗ** | N/A | ਡਿਕੋਡਰ ਪ੍ਰਾਂਪਟ ਟੋਕਨ | `audioClient.settings.language` (JS) / `audioClient.Settings.Language` (C#) |
| **ਸਟ੍ਰੀਮਿੰਗ** | ਹਾਂ | ਨਹੀਂ | `transcribeStreaming()` (JS) / `TranscribeAudioStreamingAsync()` (C#) |
| **ਪ੍ਰਾਇਵੇਸੀ ਲਾਭ** | ਕੋਡ/ਡਾਟਾ ਸਥਾਨਕ ਰਹਿੰਦਾ ਹੈ | ਆਡੀਓ ਡਾਟਾ ਸਥਾਨਕ ਰਹਿੰਦਾ ਹੈ | ਆਡੀਓ ਡਾਟਾ ਸਥਾਨਕ ਰਹਿੰਦਾ ਹੈ |

---

## ਮੁੱਖ ਨਤੀਜੇ

| ਧਾਰਨਾ | ਤੁਸੀਂ ਕੀ ਸਿੱਖਿਆ |
|---------|-----------------|
| **Whisper ਡਿਵਾਈਸ ਤੇ** | ਸਪੀਚ-ਲੈਖ ਹੋਮਾਜੀ ਸਥਾਨਕ ਤੌਰ ਤੇ ਚਲਦਾ ਹੈ, Zava ਗਾਹਕ ਕਾਲਾਂ ਅਤੇ ਉਤਪਾਦ ਸਮੀਖਿਆਵਾਂ ਦਾ ਸਥਾਨਕ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਲਈ ਉਤਕ੍ਰਿਸ਼ਟ |
| **SDK AudioClient** | JavaScript ਅਤੇ C# SDKs ਇੱਕ ਅੰਦਰੂਨੀ `AudioClient` ਦਿੰਦੇ ਹਨ ਜੋ ਪੂਰਨ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਪ੍ਰਕਿਰਿਆ ਇੱਕ ਕਾਲ ਵਿੱਚ ਸੰਭਾਲਦਾ ਹੈ |
| **ਭਾਸ਼ਾ ਸੈਟਿੰਗ** | ਹਮੇਸ਼ਾਂ AudioClient ਭਾਸ਼ਾ ਸੈੱਟ ਕਰੋ (ਜਿਵੇਂ `"en"`) — ਇਸਦੇ ਬਿਨਾਂ, ਆਟੋ-ਡਿਟੈਕਸ਼ਨ ਗੜਬੜ ਵਾਲਾ ਨਤੀਜਾ ਦੇ ਸਕਦਾ ਹੈ |
| **Python** | ਮਾਡਲ ਪ੍ਰਬੰਧਨ ਲਈ `foundry-local-sdk` + ਸਿੱਧਾ ONNX ਇੰਫਰੰਸ ਲਈ `onnxruntime` + `transformers` + `librosa` ਵਰਤਦਾ ਹੈ |
| **JavaScript** | `foundry-local-sdk` ਨੂੰ `model.createAudioClient()` ਨਾਲ ਵਰਤਦਾ ਹੈ — `settings.language` ਸੈੱਟ ਕਰੋ, ਫਿਰ `transcribe()` ਕਾਲ ਕਰੋ |
| **C#** | `Microsoft.AI.Foundry.Local` ਨੂੰ `model.GetAudioClientAsync()` ਨਾਲ ਵਰਤਦਾ ਹੈ — `Settings.Language` ਸੈੱਟ ਕਰੋ, ਫਿਰ `TranscribeAudioAsync()` ਕਾਲ ਕਰੋ |
| **ਸਟਰੀਮਿੰਗ ਸਹਿਯੋਗ** | JS ਅਤੇ C# SDKs ਵਿੱਚ `transcribeStreaming()` / `TranscribeAudioStreamingAsync()` ਵੀ ਹੁੰਦੇ ਹਨ ਜੋ ਟੁਕੜਿਆਂ ਵਾਰੀ ਆਉਟਪੁੱਟ ਲਈ |
| **CPU-ਆਧਾਰਿਤ** | CPU ਵੈਰੀਅੰਟ (3.05 GB) ਕਿਸੇ ਵੀ Windows ਡਿਵਾਈਸ ਤੇ GPU ਦੇ ਬਿਨਾਂ ਚੱਲਦਾ ਹੈ |
| **ਪ੍ਰਾਇਵੇਸੀ-ਪਹਿਲਾਂ** | Zava ਗਾਹਕ ਸੰਵਾਦਾਂ ਅਤੇ ਮਾਲਕੀ ਉਤਪਾਦ ਡਾਟਾ ਸਥਾਨਕ ਰੱਖਣ ਲਈ ਬਹੁਤ ਵਧੀਆ |

---

## ਸਾਧਨ

| ਸਾਧਨ | ਲਿੰਕ |
|--------|------|
| Foundry Local ਦਸਤਾਵੇਜ਼ | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK ਸੰਦਰਭ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| OpenAI Whisper ਮਾਡਲ | [github.com/openai/whisper](https://github.com/openai/whisper) |
| Foundry Local ਵੈੱਬਸਾਈਟ | [foundrylocal.ai](https://foundrylocal.ai) |

---

## ਅਗਲਾ ਕਦਮ

ਆਪਣੇ ਮਾਡਲਾਂ ਨੂੰ Hugging Face ਤੋਂ ਇਕੱਠਾ ਕਰਕੇ Foundry Local ਰਾਹੀਂ ਚਲਾਉਣ ਲਈ [ਭਾਗ 10: ਕਸਟਮ ਜਾਂ Hugging Face ਮਾਡਲਾਂ ਦੀ ਵਰਤੋਂ](part10-custom-models.md) ਤੇ ਜਾਰੀ ਰੱਖੋ।

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ਅਸਵੀਕਾਰੋक्ति**:  
ਇਹ ਦਸਤਾਵੇਜ਼ ਏਆਈ ਅਨੁਵਾਦ ਸੇਵਾ [Co-op Translator](https://github.com/Azure/co-op-translator) ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਅਨੁਵਾਦ ਕੀਤਾ ਗਿਆ ਹੈ। ਹਾਲਾਂਕਿ ਅਸੀਂ ਸਹੀਤਾ ਲਈ ਕੋਸ਼ਿਸ਼ ਕਰਦੇ ਹਾਂ, ਕਿਰਪਾ ਕਰਕੇ ਧਿਆਨ ਰੱਖੋ ਕਿ ਸਵੈਚਾਲਿਤ ਅਨੁਵਾਦਾਂ ਵਿੱਚ ਗਲਤੀਆਂ ਜਾਂ ਅਸਮੀਖਿਆ ਹੋ ਸਕਦੀ ਹੈ। ਮੂਲ ਦਸਤਾਵੇਜ਼ ਨੂੰ ਇਸ ਦੀ ਮੂਲ ਭਾਸ਼ਾ ਵਿੱਚ ਪ੍ਰਮੁੱਖ ਸਰੋਤ ਮੰਨਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ। ਮਹੱਤਵਪੂਰਣ ਜਾਣਕਾਰੀ ਲਈ, ਪੇਸ਼ਾਵਰ ਮਨੁੱਖੀ ਅਨੁਵਾਦ ਦੀ ਸਿਫ਼ਾਰਿਸ਼ ਕੀਤੀ ਜਾਂਦੀ ਹੈ। ਅਸੀਂ ਇਸ ਅਨੁਵਾਦ ਦੀ ਵਰਤੋਂ ਤੋਂ ਪੈਦਾ ਹੋਣ ਵਾਲੀਆਂ ਕਿਸੇ ਵੀ ਗਲਤਫਹਮੀਆਂ ਜਾਂ ਭ੍ਰਮਾਂ ਲਈ ਜ਼ਿੰਮੇਵਾਰ ਨਹੀਂ ਹਾਂ।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->