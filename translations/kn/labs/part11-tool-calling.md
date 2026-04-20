![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ಭಾಗ 11: ಸ್ಥಳೀಯ ಮಾದರಿಗಳೊಂದಿಗೆ ಟूल್ ಕರೆಮಾಡುವುದು

> **ಗೋಲ್:** ನಿಮ್ಮ ಸ್ಥಳೀಯ ಮಾದರಿ ಬಾಹ್ಯ ಕಾರ್ಯಗಳು (ಟೂಲ್ಸ್) ಕರೆಮಾಡುವ ಮೂಲಕ ನಿಜ-ಸಮಯದ ಡೇಟಾವನ್ನು ಪಡೆದು, ಲೆಕ್ಕಾಚಾರಗಳನ್ನು ನೆರವೇರಿಸಲು ಅಥವಾ APIs ಜೊತೆಗೆ ಸಂವಹನ ಮಾಡಲು ಸಾದ್ಯ ಮಾಡುವುದಾಗಿದೆ — ಅದು ನಿಮ್ಮ ಸಾಧನದಲ್ಲಿ ಖಾಸಗಿ ರೀತಿಯಲ್ಲಿ ಕಾರ್ಯನಿರ್ವಹಿಸುತ್ತದೆ.

## ಟೂಲ್ ಕರೆಮಾಡುವುದು ಎಂದರೆ ಏನು?

ಟೂಲ್ನ ಕರೆಮಾಡುವುದನ್ನು (ಇನ್ನೂ **ಫಂಕ್ಷನ್ ಕರೆಮಾಡುವುದು** ಎಂದು ಕರೆಯಲಾಗುತ್ತದೆ) ಭಾಷಾ ಮಾದರಿ ನೀವು ನೀಡಿದ ಕಾರ್ಯಗಳನ್ನು ನಿರ್ವಹಿಸುವಿಕೆಗೆ ವಿನಂತಿಸುವಿಕೆಯನ್ನು ಸಾದ್ಯ ಮಾಡುವುದು. ಉತ್ತರವನ್ನು ಊಹಿಸುವ ಬದಲು, ಮಾದರಿ ಯಾವಾಗ ಟೂಲ್ನ ಸಹಾಯ ಮಾಡಲು ಸಾಧ್ಯವೆಂದು ಗುರುತಿಸಿ ನಿಮ್ಮ ಕೋಡ್ ಕಾರ್ಯನಿರ್ವಹಿಸಬೇಕಾದವು ಎಂಬ ವಿನಂತಿಯನ್ನು ಕಟ್ಟಿಕೊಳ್ಳುತ್ತದೆ. ನಿಮ್ಮ ಅಪ್ಲಿಕೇಶನ್ ಆ ಕಾರ್ಯವನ್ನು ನಿರ್ವಹಿಸಿ ಫಲಿತಾಂಶವನ್ನು ಮರಳಿ ಕಳುಹಿಸುತ್ತದೆ ಮತ್ತು ಮಾದರಿ ಆ ಮಾಹಿತಿಯನ್ನು ತನ್ನ ಕೊನೆಯಲ್ಲಿ ಉತ್ತರದಲ್ಲಿರಿಸುತ್ತದೆ.

![ಟೂಲ್-ಕರೆಮಾಡುವ ಹರಿವು](../../../images/tool-calling-flow.svg)

ಈ ಮಾದರಿಯನ್ನು ನಿಯೋಜಿಸುವುದರಿಂದ ವಿದ್ಯುತ್ತಿನ ಏಜೆಂಟ್ ಗಳು ಇವುಗಳನ್ನು ಮಾಡಬಹುದು:

- **ನೇರ ಡೇಟಾವನ್ನು ಹುಡುಕುವುದು** (ಹವಾಮಾನ, ಸ್ಟಾಕ್ ಬೆಲೆಗಳು, ಡೇಟಾಬೇಸ್ ವಿಚಾರಣೆಗಳು)
- **ವಿಸ್ತೃತ ಲೆಕ್ಕಾಚಾರಗಳನ್ನು ನೆರವೇರಿಸುವುದು** (ಗಣಿತ, ಘಟಕ ಪರಿವರ್ತನೆಗಳು)
- **ಕ್ರಿಯೆಗಳು ಕೈಗೊಳ್ಳುವುದು** (ಇಮೇಲ್ ಕಳುಹಿಸುವುದು, ಟಿಕ್ಕೆಟ್ಸ್ ಸೃಷ್ಟಿಸುವುದು, ದಾಖಲೆಗಳನ್ನು ನವೀಕರಿಸುವುದು)
- **ಖಾಸಗಿ ವ್ಯವಸ್ಥೆಗಳಿಗೆ ಪ್ರವೇಶ** (ಅಂತರಂಗ API ಗಳು, ಫೈಲ್ ವ್ಯವಸ್ಥೆಗಳು)

---

## ಟೂಲ್ ಕರೆಮಾಡುವುದು ಹೇಗೆ ಕೆಲಸ ಮಾಡುತ್ತದೆ

ಟೂಲ್ನ ಕರೆ ಮಾಡುವ ಹರಿವು ನಾಲ್ಕು ಹಂತಗಳನ್ನು ಹೊಂದಿದೆ:

| ಹಂತ | ಏನಾಗುತ್ತದೆ |
|-------|-------------|
| **1. ಟೂಲ್ಗಳನ್ನು ನಿರ್ದಿಷ್ಟಮಾಡಿ** | JSON Schema ಉಪಯೋಗಿಸಿ ಲಭ್ಯವಿರುವ ಕಾರ್ಯಗಳನ್ನು ವಿವರಣೆ ಮಾಡಿ — ಹೆಸರು, ವಿವರಣೆ, ಮತ್ತು ಪರಿಮಾಣಗಳು |
| **2. ಮಾದರಿ ನಿರ್ಧರಿಸುತ್ತದೆ** | ಮಾದರಿಗೆ ನಿಮ್ಮ ಸಂದೇಶ ಮತ್ತು ಟೂಲ್ಗಳ ವಿವರಣೆಗಳು ಬರುತ್ತವೆ. ಟೂಲ್ ಸಹಾಯ ಮಾಡಬಹುದಾದರೆ, ಅದು `tool_calls` ಎಂಬ ಪ್ರತಿಕ್ರಿಯೆಯನ್ನು ಪೂರ್ವ ನಿಯತ ಉತ್ತರದ ಬದಲು ನೀಡುತ್ತದೆ |
| **3. ಸ್ಥಳೀಯವಾಗಿ ಕಾರ್ಯನಿರ್ವಹಿಸಿ** | ನಿಮ್ಮ ಕೋಡ್ ಟೂಲ್ನ ಕರೆಡೆ, ಕಾರ್ಯವನ್ನು ನಡೆಸಿ, ಫಲಿತಾಂಶವನ್ನು ಸಂಗ್ರಹಿಸುತ್ತದೆ |
| **4. ಕೊನೆಯ ಉತ್ತರ** | ನೀವು ಟೂಲ್ನ ಫಲಿತಾಂಶವನ್ನು ಮಾದರಿಗೆ ಕಳುಹಿಸುತ್ತೀರಿ, ಮತ್ತು ಮಾದರಿ ಕೊನೆಗೆ ಉತ್ತರವನ್ನೂ ನೀಡುತ್ತದೆ |

> **ಮುಖ್ಯಾಂಶ:** ಮಾದರಿ ಎಂದಿಗೂ ಕೋಡ್ ಅನ್ನು ನಿರ್ವಹಿಸುವುದಿಲ್ಲ. ಅದು ಕೇವಲ ಟೂಲ್ನ ಕರೆಮಾಡಲು ವಿನಂತಿಸುವುದು. ಆ ವಿನಂತಿಯನ್ನು ಗೌರವಿಸುವುದು ನಿಮ್ಮ ಅಪ್ಲಿಕೇಶನ್ ನಿರ್ಧರಿಸುವುದು — ಇದು ನಿಮ್ಮನ್ನು ಪೂರ್ಣ ನಿಯಂತ್ರಣದಲ್ಲಿ ಇಡುತ್ತದೆ.

---

## ಯಾವ ಮಾದರಿಗಳು ಟೂಲ್ನ ಕರೆಮಾಡುವಿಕೆಯನ್ನು ಬೆಂಬಲಿಸುತ್ತವೆ?

ಎಲ್ಲಾ ಮಾದರಿಗಳು ಟೂಲ್ ಕರೆಮಾಡುವುದನ್ನು ಬೆಂಬಲಿಸುವುದಿಲ್ಲ. ಇಟ್ಟುಕೊಂಡಿರುವ ಫೌಂಡ್ರಿ ಲೋಕಲ್ ಕ್ಯಾಟಲಾಗ್ ನಲ್ಲಿ, ಕೆಳಗಿನ ಮಾದರಿಗಳು ಟೂಲ್ಕೋರಿಂಗ್ ಸಾಮರ್ಥ್ಯ ಹೊಂದಿವೆ:

| ಮಾದರಿ | ಗಾತ್ರ | ಟೂಲ್ನ ಕರೆಮಾಡುವುದು |
|-------|--------|:---:|
| qwen2.5-0.5b | 822 MB | ✅ |
| qwen2.5-1.5b | 1.8 GB | ✅ |
| qwen2.5-7b | 6.3 GB | ✅ |
| qwen2.5-14b | 11.3 GB | ✅ |
| qwen2.5-coder-0.5b | 822 MB | ✅ |
| qwen2.5-coder-1.5b | 1.8 GB | ✅ |
| qwen2.5-coder-7b | 6.3 GB | ✅ |
| qwen2.5-coder-14b | 11.3 GB | ✅ |
| phi-4-mini | 4.6 GB | ✅ |
| phi-3.5-mini | 2.6 GB | ❌ |
| phi-4 | 10.4 GB | ❌ |

> **ಸೂಚನೆ:** ಈ ಪ್ರಯೋಗಾಲಯಕ್ಕಾಗಿ ನಾವು **qwen2.5-0.5b** ಅನ್ನು ಬಳಸುತ್ತೇವೆ — ಇದು ಚಿಕ್ಕದಾಗಿದ್ದು (822 MB ಡೌನ್‌ಲೋಡ್), ವೇಗವಾದುದು ಮತ್ತು ವಿಶ್ವಸನೀಯ ಟೂಲ್-ಕಾಲ್ ಬೆಂಬಲವಿದೆ.

---

## ಕಲಿಯುವ ಗುರಿಗಳು

ಈ ಪ್ರಯೋಗಾಲಯದ ಕೊನೆಯಲ್ಲಿ ನೀವು ಈ ವಿಷಯಗಳು ಅರ್ಥಮಾಡಿಕೊಳ್ಳಬಹುದು:

- ಟೂಲ್ನ ಕರೆಮಾಡುವ ಮಾದರಿಯನ್ನು ವಿವರಿಸಿ ಮತ್ತು ಏಕೆ ಇದು AI ಏಜೆಂಟ್ ಗಳಿಗೆ ಅಗತ್ಯ
- OpenAI function-calling ಫಾರ್ಮ್ಯಾಟ್ ಬಳಸಿ ಟೂಲ schemas ರಚಿಸಿ
- ಬಹು-ಚರಂಡಿಯಾದ ಟೂಲ್-ಕಾಲ್ ಸಂವಾದ ಹರಿವನ್ನು ನಿಭಾಯಿಸಿ
- ಟೂಲ್ಕಾಲ್ ಗಳನ್ನು ಸ್ಥಳೀಯವಾಗಿ ನಿರ್ವಹಿಸಿ ಮತ್ತು ಫಲಿತಾಂಶಗಳನ್ನು ಮಾದರಿಗೆ ಹಿಂತಿರುಗಿಸಿ
- ಟೂಲ್-ಕಾಲ್ ಘಟನಾಕ್ರಮಗಳಿಗೆ ಸರಿಯಾದ ಮಾದರಿಯನ್ನು ಆರಿಸಿಕೊಳ್ಳಿ

---

## ಪೂರ್ವಶರತ್‌ಗಳು

| ಅಗತ್ಯವಿರುವುದು | ವಿವರಗಳು |
|-------------|---------|
| **Foundry Local CLI** | ಇನ್‌ಸ್ಟಾಲ್ ಆಗಿದ್ದು ನಿಮ್ಮ `PATH` ನಲ್ಲಿ ([ಭಾಗ 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript ಅಥವಾ C# SDK ಇನ್‌ಸ್ಟಾಲ್ ಆಗಿರಬೇಕು ([ಭಾಗ 2](part2-foundry-local-sdk.md)) |
| **ಒಂದು ಟೂಲ್-ಕಾಲ್ ಮಾಡಬಲ್ಲ ಮಾದರಿ** | qwen2.5-0.5b (ಸ್ವಯಂಚಾಲಿತ ಡೌನ್‌ಲೋಡ್ ಆಗುತ್ತದೆ) |

---

## ವ್ಯಾಯಾಮಗಳು

### ವ್ಯಾಯಾಮ 1 — ಟೂಲ್-ಕಾಲ್ ಹರಿವನ್ನು ಅರ್ಥಮಾಡಿಕೊಳ್ಳಿ

ಕೋಡ್ ಬರೆಯುವ ಮೊದಲು ಈ ಸರಣಿಚಿತ್ರಣವನ್ನ ಅಧ್ಯಯನ ಮಾಡಿ:

![ಟೂಲ್-ಕಾಲ್ ಸರಣಿಚಿತ್ರಣ](../../../images/tool-calling-sequence.svg)

**ಮುಖ್ಯ ಗಮನಗಳು:**

1. ನೀವು ಟೂಲ್ಗಳನ್ನು ಮುಂಚಿತವಾಗಿ JSON Schema ಅಂಶಗಳಾಗಿ ವಿವರಿಸುತ್ತೀರಿ
2. ಮಾದರಿಯ ಪ್ರತಿಕ್ರಿಯೆಯು ಸಾಮಾನ್ಯ ವಿಷಯದ ಬದಲು `tool_calls` ಒಳಗೊಂಡಿದೆ
3. ಪ್ರತಿ ಟೂಲ್ನ ಕರೆಗೆ ವೇರೊಬ್ಬ `id` ಇರುತ್ತದೆ ಇದು ಫಲಿತಾಂಶಗಳನ್ನು ಹಿಂತಿರುಗಿಸುವಾಗ ಉಲ್ಲೇಖಿಸಬೇಕಾಗುತ್ತದೆ
4. ಮಾದರಿ ಹಿಂದಿನ ಎಲ್ಲಾ ಸಂದೇಶಗಳು ಮತ್ತು ಟೂಲ್ ಫಲಿತಾಂಶಗಳೊಂದಿಗೆ ಕೊನೆಯ ಉತ್ತರವನ್ನು ರಚಿಸುತ್ತದೆ
5. ಒಬ್ಬ ಪ್ರತಿಕ್ರಿಯೆಯಲ್ಲಿ ಹಲವು ಟೂಲ್ನ ಕರೆಗಳು ಸಂಭವಿಸಬಹುದು

> **ಚರ್ಚೆ:** ಮಾದರಿ ಕಾರ್ಯಗಳನ್ನು ನೇರವಾಗಿ ನಿರ್ವಹಿಸುವುದರ ಬದಲು ಟೂಲ್ನ ಕರೆಗಳನ್ನು ಮರಳಿಸುವುದಕ್ಕೆ ಏಕೆ ಕಾರಣ? ಇದು ಯಾವ ಭದ್ರತಾ ನಿಟ್ಟಿನಲ್ಲಿ ಸಹಾಯಕ?

---

### ವ್ಯಾಯಾಮ 2 — ಟೂಲ್ಸ್ Schema ಗಳನ್ನು ನಿರ್ದಿಷ್ಟಮಾಡುವುದು

ಟೂಲ್ಗಳನ್ನು OpenAI function-calling ಫಾರ್ಮ್ಯಾಟ್ ಉಪಯೋಗಿಸಿ ನಿರ್ದಿಷ್ಟಪಡಿಸಲಾಗುತ್ತದೆ. ಪ್ರತಿ ಟೂಲ್ ಅಗತ್ಯವಿರುತ್ತದೆ:

- **`type`**: ಸದಾ `"function"`
- **`function.name`**: ವಿವರಣಾತ್ಮಕ ಕಾರ್ಯದ ಹೆಸರು (ಹೆಚ್ಚು ಪಟ್ಟಿ: `get_weather`)
- **`function.description`**: ಸ್ಪಷ್ಟ ವಿವರಣೆ — ಮಾದರಿ ಯಾವಾಗ ಟೂಲ್ ಕರೆದರೆ ಎಂದು ಈ ಹಿಂದೆ ನಿರ್ಧರಿಸುತ್ತದೆ
- **`function.parameters`**: ನಿರೀಕ್ಷಿತ arguments ಗಳನ್ನು ವಿವರಿಸುವ JSON Schema ಅಂಶ

```json
{
  "type": "function",
  "function": {
    "name": "get_weather",
    "description": "Get the current weather for a given city",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. London"
        }
      },
      "required": ["city"]
    }
  }
}
```

> **ಟೂಲ್ ವಿವರಣೆಗಳಿಗೆ ಉತ್ತಮ ಪದ್ಧತಿಗಳು:**
> - ನಿಖರವಾಗಿರಿ: "ನಿರ್ದಿಷ್ಟ ನಗರದ ಹವಾಮಾನ ಪಡೆಯಿರಿ" ಅನ್ನು "ಹವಾಮಾನ ಪಡೆಯಿರಿ" ಹೋಲಿಸಿ ಚೆನ್ನಾಗಿದೆ
> - ಪ್ಯಾರಾಮೀಟರ್ ಗಳನ್ನು ಸ್ಪಷ್ಟವಾಗಿ ವಿವರಣೆ ಮಾಡಿ: ಮಾದರಿ ಸರಿಯಾದ ಮೌಲ್ಯಗಳನ್ನು ತುಂಬಲು ಇದನ್ನು ಓದುತ್ತದೆ
> - ಅಗತ್ಯ ಮತ್ತು ಐಚ್ಛಿಕ ಪ್ಯಾರಾಮೀಟರ್ ಗಳನ್ನು ಗುರುತಿಸಿ — ಇದು ಮಾದರಿಗೆ ಕೇಳಬೇಕಾದುದನ್ನು ನಿರ್ಧರಿಸಲು ಸಹಾಯಕ

---

### ವ್ಯಾಯಾಮ 3 — ಟೂಲ್-ಕಾಲ್ ಉದಾಹರಣೆಗಳನ್ನು ನಿರ್ವಹಿಸಿ

ಪ್ರತಿ ಭಾಷೆ ಉದಾಹರಣೆಯು ಎರಡು ಟೂಲ್ಗಳನೆ ನೀಡುತ್ತದೆ (`get_weather` ಮತ್ತು `get_population`), ಟೂಲ್ ಬಳಕೆ ಪ್ರಚೋದಿಸುವ ಪ್ರಶ್ನೆಯನ್ನು ಕಳುಹಿಸುತ್ತದೆ, ಸ್ಥಳೀಯವಾಗಿ ಟೂಲ್ ಕಾರ್ಯನಿರ್ವಹಿಸಿ ಫಲಿತಾಂಶವನ್ನು ಹಿಂದಿರುಗಿಸುತ್ತದೆ.

<details>
<summary><strong>🐍 Python</strong></summary>

**ಪೂರ್ವಶರತ್‌ಗಳು:**
```bash
cd python
python -m venv venv

# ವಿಂಡೋಸ್ (ಪವರ್‌ಶೆಲ್):
venv\Scripts\Activate.ps1
# ಮ್ಯಾಕ್‌ಒಎಸ್ / ಲಿನಕ್ಸ್:
source venv/bin/activate

pip install -r requirements.txt
```

**ನಿರ್ವಹಿಸಿ:**
```bash
python foundry-local-tool-calling.py
```

**ನಿರೀಕ್ಷಿತ ಔಟ್ಪುಟ್:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ಕೋಡ್ ವಿವರಿಕೆ** (`python/foundry-local-tool-calling.py`):

```python
# ಫಂಕ್ಷನ್_SCHEMA ಗಳ ಪಟ್ಟಿಯಾಗಿ ಉಪಕರಣಗಳನ್ನು ವ್ಯಾಖ್ಯಾನಿಸಿ
tools = [
    {
        "type": "function",
        "function": {
            "name": "get_weather",
            "description": "Get the current weather for a given city",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {"type": "string", "description": "The city name"}
                },
                "required": ["city"]
            }
        }
    }
]

# ಉಪಕರಣಗಳೊಂದಿಗೆ ಕಳುಹಿಸಿ — ಮಾದರಿ ವಿಷಯದ ಬದಲು tool_calls ಅನ್ನು ಹಿಂತಿರುಗಿಸಬಹುದು
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# ಮಾದರಿ ಉಪಕರಣವನ್ನು ಕರೆ ಮಾಡಬೇಕಾದರೆ ಪರಿಶೀಲಿಸಿ
if response.choices[0].message.tool_calls:
    # ಉಪಕರಣವನ್ನು ನಿರ್ವಹಿಸಿ ಮತ್ತು ಫಲಿತಾಂಶವನ್ನು ಹಿಂದಿರುಗಿಸಿ
    ...
```

</details>

<details>
<summary><strong>🟨 JavaScript (Node.js)</strong></summary>

**ಪೂರ್ವಶರತ್‌ಗಳು:**
```bash
cd javascript
npm install
```

**ನಿರ್ವಹಿಸಿ:**
```bash
node foundry-local-tool-calling.mjs
```

**ನಿರೀಕ್ಷಿತ ಔಟ್ಪುಟ್:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ಕೋಡ್ ವಿವರಿಕೆ** (`javascript/foundry-local-tool-calling.mjs`):

ಈ ಉದಾಹರಣೆ OpenAI SDK ಬದಲು ಫೌಂಡ್ರಿ ಲೋಕಲ್ SDK ನ ಮೂಲ `ChatClient` ಉಪಯೋಗಿಸುತ್ತದೆ, ಮತ್ತು `createChatClient()` ವಿಧಾನವು ಸೌಕರ್ಯವನ್ನು ಒದಗಿಸುತ್ತದೆ:

```javascript
// ಮಾದರಿ ವಸ್ತುವಿನ ನೇರವಾಗಿ ChatClient ಪಡೆಯಿರಿ
const chatClient = model.createChatClient();

// ಉಪಕರಣಗಳೊಂದಿಗೆ ಕಳುಹಿಸಿ — ChatClient OpenAI-ಸಮ್ಮತಿತ ದಾಖಲೆಯನ್ನು ನಿರ್ವಹಿಸುತ್ತದೆ
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// ಉಪಕರಣ ಕರೆಗಳನ್ನು ಪರಿಶೀಲಿಸಿ
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // ಉಪಕರಣಗಳನ್ನು ಕಾರ್ಯಗತಗೊಳಿಸಿ ಮತ್ತು ಫಲಿತಾಂಶಗಳನ್ನು ಹಿಂದಿರುಗಿಸಿ ಕಳುಹಿಸಿ
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**ಪೂರ್ವಶರತ್‌ಗಳು:**
```bash
cd csharp
dotnet restore
```

**ನಿರ್ವಹಿಸಿ:**
```bash
dotnet run toolcall
```

**ನಿರೀಕ್ಷಿತ ಔಟ್ಪುಟ್:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ಕೋಡ್ ವಿವರಿಕೆ** (`csharp/ToolCalling.cs`):

C# ನಲ್ಲಿ `ChatTool.CreateFunctionTool` ಸಹಾಯಕವನ್ನು ಟೂಲ್ಗಳನ್ನು ನಿರ್ಧರಿಸಲು ಬಳಸುತ್ತದೆ:

```csharp
ChatTool getWeatherTool = ChatTool.CreateFunctionTool(
    functionName: "get_weather",
    functionDescription: "Get the current weather for a given city",
    functionParameters: BinaryData.FromString("""
    {
        "type": "object",
        "properties": {
            "city": { "type": "string", "description": "The city name" }
        },
        "required": ["city"]
    }
    """));

var options = new ChatCompletionOptions();
options.Tools.Add(getWeatherTool);

// Check FinishReason to see if tools were called
if (completion.Value.FinishReason == ChatFinishReason.ToolCalls)
{
    // Execute tools and send results back
    ...
}
```

</details>

---

### ವ್ಯಾಯಾಮ 4 — ಟೂಲ್-ಕಾಲ್ ಸಂವಾದ ಹರಿವು

ಸಂದೇಶ ರಚನೆಯನ್ನು ಅರ್ಥಮಾಡಿಕೊಳ್ಳುವುದು ಮುಖ್ಯ. ಇಲ್ಲಿ ಪ್ರತಿ ಹಂತದಲ್ಲಿ `messages` ಸರಣಿಯ ಪೂರ್ಣ ಹರಿವು ಇದೆ:

**ಹಂತ 1 — ಪ್ರಾಥಮಿಕ ವಿನಂತಿ:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**ಹಂತ 2 — ಮಾದರಿ `tool_calls` (ವಿಷಯವಿಲ್ಲದೆ) ನೊಂದಿಗೆ ಪ್ರತಿಕ್ರಿಯಿಸುತ್ತದೆ:**
```json
{
  "role": "assistant",
  "tool_calls": [
    {
      "id": "call_abc123",
      "type": "function",
      "function": {
        "name": "get_weather",
        "arguments": "{\"city\": \"London\"}"
      }
    }
  ]
}
```

**ಹಂತ 3 — ನೀವು ಸಹಾಯಕ ಸಂದೇಶ AND ಟೂಲ್ ಫಲಿತಾಂಶ ಸೇರಿಸುತ್ತೀರಿ:**
```json
[
  {"role": "system", "content": "..."},
  {"role": "user", "content": "What is the weather like in London?"},
  {"role": "assistant", "tool_calls": [...]},
  {
    "role": "tool",
    "tool_call_id": "call_abc123",
    "content": "{\"city\": \"London\", \"temperature\": \"18°C\", \"condition\": \"Partly cloudy\"}"
  }
]
```

**ಹಂತ 4 — ಮಾದರಿ ಟೂಲ್ ಫಲಿತಾಂಶ ಬಳಸಿ ಕೊನೆ ಉತ್ತರ ನೀಡುತ್ತದೆ.**

> **ಮುಖ್ಯ:** ಟೂಲ್ ಸಂದೇಶದಲ್ಲಿನ `tool_call_id` ಟೂಲ್ ಕರೆ `id` ನೊಂದಿಗೆ ಹೊಂದಿಕೊಳ್ಳಬೇಕು. ಇದು ಮಾದರಿಗೆ ಫಲಿತಾಂಶಗಳನ್ನು ಆಹ್ವಾನಗಳೊಂದಿಗೆ ಹೊಂದಾಣಿಕೆ ಮಾಡಲು ಸಹಾಯ ಮಾಡುತ್ತದೆ.

---

### ವ್ಯಾಯಾಮ 5 — ಬಹು ಟೂಲ್ ಕರೆಗಳು

ಒಂದು ಪ್ರತಿಕ್ರಿಯೆಯಲ್ಲಿ ಬಹು ಟೂಲ್ ಕರೆಗಳನ್ನು ಮಾದರಿ ವಿನಂತಿಸಬಹುದು. ಬಳಕೆದಾರ ಸಂದೇಶವನ್ನು ಬದಲಾಯಿಸಿ ಹಲವಾರು ಕರೆಗಳಿಗೆ ಪ್ರಚೋದಿಸಿ:

```python
# ಪೈಥಾನ್‌ನಲ್ಲಿ — ಬಳಕೆದಾರ ಸಂದೇಶವನ್ನು ಬದಲಾವಣೆ ಮಾಡಿ:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್‌ನಲ್ಲಿ — ಬಳಕೆದಾರ ಸಂದೇಶವನ್ನು ಬದಲಿಸಿ:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

ಮಾದರಿ ಎರಡು `tool_calls` — ಒಂದು `get_weather` ಮತ್ತು ಇನ್ನೊಂದು `get_population` ಮರಳಿಸಬೇಕು. ನಿಮ್ಮ ಕೋಡ್ ಈಗಾಗಲೇ ಎಲ್ಲಾ ಟೂಲ್ಗಳ ಕರೆಗಳನ್ನು ಲೂಪ್ ಮೂಲಕ ನಿಭಾಯಿಸುತ್ತದೆ.

> **ಪ್ರಯತ್ನಿಸಿ:** ಬಳಕೆದಾರ ಸಂದೇಶವನ್ನು ಬದಲಿಸಿ ಮತ್ತು ಉದಾಹರಣೆಯನ್ನು ಮತ್ತೆ ನಡೆಯಿಸಿ. ಮಾದರಿ ಇಬ್ಬರೂ ಟೂಲ್ಗಳನ್ನು ಕರೆಮಾಡುತ್ತದೆಯೇ?

---

### ವ್ಯಾಯಾಮ 6 — ನಿಮ್ಮ ಸ್ವಂತ ಟೂಲ್ ಸೇರಿಸಿ

ಒಂದು ಉದಾಹರಣೆಗೆ ಹೊಸ ಟೂಲನ್ನು ಸಲಕರಣೆ ಮಾಡಿ. ಉದಾಹರಣೆಗೆ, `get_time` ಟೂಲನ್ನು ಸೇರಿಸಿ:

1. ಟೂಲಿನ schema ಅನ್ನು ನಿರ್ಧರಿಸಿ:
```json
{
  "type": "function",
  "function": {
    "name": "get_time",
    "description": "Get the current time in a given city's timezone",
    "parameters": {
      "type": "object",
      "properties": {
        "city": {
          "type": "string",
          "description": "The city name, e.g. Tokyo"
        }
      },
      "required": ["city"]
    }
  }
}
```

2. ಕಾರ್ಯನಿರ್ವಹಿಸುವ ಲಾಜಿಕ್ ಅನ್ನು ಸೇರಿಸಿ:
```python
# ಪೈಥಾನ್
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # ವಾಸ್ತವಿಕ ಆಪ್‌ನಲ್ಲಿ, ಸಮಯ ವಲಯ ಪುಸ್ತಕಾಲಯವನ್ನು ಉಪಯೋಗಿಸಿ
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... ಇತ್ತೀಚಿನ ಸಾಧನಗಳು ...
```

3. `tools` ಸರಣಿಗೆ ಟೂಲ್ ಸೇರಿಸಿ ಮತ್ತು "ಟೋಕಿಯೋ ಸಮಯ ಏನು?" ಎಂಬ ಪ್ರಶ್ನೆಯನ್ನು ಪರೀಕ್ಷಿಸಿ.

> **ಚುನೈತಿ:** `convert_temperature` ಅನ್ನು ಸೇರಿಸಿ, ಇದು ಸೆಲ್ಸಿಯಸ್ ಮತ್ತು ಫರೆನ್‌ಹೀಟ್ ನಡುವಿನ ಪರಿವರ್ತನೆ ಮಾಡುತ್ತದೆ. "100°F ಅನ್ನು ಸೆಲ್ಸಿಯಸ್ ಗೆ ಪರಿವರ್ತಿಸಿ" ಎಂದು ಪರೀಕ್ಷಿಸಿ.

---

### ವ್ಯಾಯಾಮ 7 — SDK ಇದರ ChatClient ಮೂಲಕ ಟೂಲ್ನ ಕರೆ (JavaScript)

JavaScript ಉದಾಹರಣೆ ಈಗಾಗಲೇ SDK ಮೂಲ `ChatClient` ಅನ್ನು ಉಪಯೋಗಿಸುತ್ತದೆ, OpenAI SDK ಬದಲಿಗೆ. ಇದು ಸುಲಭತೆಯನ್ನು ಒದಗಿಸುತ್ತದೆ, OpenAI ಕ್ಲೈಯೆಂಟ್ ಸ್ವತಃ ರಚಿಸುವ ಅಗತ್ಯವಿಲ್ಲದೆ:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient ಮಾದರಿ ವಸ್ತುವಿನಿಂದ ನೇರವಾಗಿ ರಚಿಸಲಾಗಿದೆ
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat ಎರಡನೆಯ ಪರಾಮಿತಿಯಾಗಿ ಸಾಧನಗಳನ್ನು ಸ್ವೀಕರಿಸುತ್ತದೆ
const response = await chatClient.completeChat(messages, tools);
```

Python ಇವುಗಳೊಂದಿಗೆ ಹೋಲಿಸಬಹುದು, ಇದು OpenAI SDK ಸ್ಪಷ್ಟವಾಗಿ ಬಳಸುತ್ತದೆ:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

ಎರಡೂ ವಿಧಾನಗಳು ಮಾನ್ಯ. `ChatClient` ಸೌಕರ್ಯವಂತವಾಗಿದೆ; OpenAI SDK ನಿಮ್ಮಿಗೆ OpenAI ಯ ಎಲ್ಲ ಪರಿಮಾಣಗಳನ್ನು ಪ್ರವೇಶಿಸಲು ಅವಕಾಶ ಕೊಡುತ್ತದೆ.

> **ಪ್ರಯತ್ನಿಸಿ:** JavaScript ಉದಾಹರಣೆಯಲ್ಲಿ OpenAI SDK ಅನ್ನು ಬದಲಿಗೆ `ChatClient` ಬಳಸುವಂತೆ ಬದಲಾಯಿಸಿ. ನೀವು `import OpenAI from "openai"` ಮಾಡಬೇಕು ಮತ್ತು `manager.urls[0]` ನಿಂದ ಎಂಡ್ಪಾಯಿಂಟ್ ಬಳಸಿಕೊಂಡು ಕ್ಲೈಯೆಂಟ್ ರಚಿಸಬೇಕು.

---

### ವ್ಯಾಯಾಮ 8 — tool_choice ಅರ್ಥಮಾಡಿಕೊಳ್ಳುವುದು

`tool_choice` ಪರಿಮಾಣ ಮಾದರಿ ಟೂಲ್ನನ್ನು ಕೇವಲ ಬಳಸಬೇಕೆಂದು ಅಥವಾ ಫ್ರೀಯಾಗಿ ಆಯ್ಕೆ ಮಾಡುವುದನ್ನು ನಿಯಂತ್ರಿಸುತ್ತದೆ:

| ಮೌಲ್ಯ | ನಡತೆಯು |
|-------|-----------|
| `"auto"` | ಮಾದರಿ ತಾನೇ ಟೂಲ್ ಕರೆಮಾಡುವುದನ್ನು ನಿರ್ಧರಿಸುತ್ತದೆ (ಪೂರ್ವನಿಗದಿ) |
| `"none"` | ಮಾದರಿ ಟೂಲ್ಗಳನ್ನು ಕರೆಮಾಡುವುದಿಲ್ಲ, ಒದಗಿಸಿದರೂ ಸಹ |
| `"required"` | ಮಾದರಿ ಕನಿಷ್ಠ ಒಂದು ಟೂಲ್ ಕರೆದಿರಬೇಕು |
| `{"type": "function", "function": {"name": "get_weather"}}` | ಮಾದರಿ ಸೂಚಿಸಿದ ಟೂಲ್ ಅನ್ನು ಕರೆದಿರಬೆಕು |

Python ಉದಾಹರಣೆಯಲ್ಲಿ ಪ್ರತಿಯೊಂದು ಆಯ್ಕೆಯನ್ನು ಪ್ರಯತ್ನಿಸಿ:

```python
# ಮಾದರಿಯನ್ನು get_weather ಅನ್ನು ಕರೆ ಮಾಡಲು ಬಲವಂತಗೊಳಿಸಿ
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **ಗಮನಿಸಿ:** ಎಲ್ಲಾ ಮಾದರಿಗಳು ಎಲ್ಲಾ `tool_choice` ಆಯ್ಕೆಗಳನ್ನು ಬೆಂಬಲಿಸದಿರಬಹುದು. `"required"` ಇಂಬಳಿಕೆ ಮಾದರಿ ಬೆಂಬಲಿಸದಿದ್ದಲ್ಲಿ, ಇದು ಈ ಸೆಟ್ಟಿಂಗ್ ಅನ್ನು ನಿರ್ಲಕ್ಷಿಸಿ `"auto"` ರಿತಿಯಲ್ಲಿ ನಡತೆಯಿರಬಹುದು.

---

## ಸಾಮಾನ್ಯ ಜಾರಿವುಗಳು

| ಸಮಸ್ಯೆ | ಪರಿಹಾರ |
|---------|----------|
| ಮಾದರಿ ಎಂದಿಗೂ ಟೂಲ್ಸ್ ಕರೆದಿಲ್ಲ | ನೀವು ಟೂಲ್-ಕಾಲ್ ಮಾಡುವ ಮಾದರಿ ಬಳಸುತ್ತಿರುವುದನ್ನು ಖಚಿತಪಡಿಸಿಕೊಳ್ಳಿ (ಉದಾಹರಣೆ: qwen2.5-0.5b). ಮೇಲಿನ ಪಟ್ಟಿಯನ್ನು ಪರಿಶೀಲಿಸಿ. |
| `tool_call_id` ಸ್ಪಷ್ಟವಿಲ್ಲ | ಯಾವಾಗಲೂ ಟೂಲಿನ ಕರೆ ಪ್ರತಿಕ್ರಿಯೆಯ `id` ಬಳಿಸಿ, ಮಾರುಕಟ್ಟೆಯ ಮೌಲ್ಯವಲ್ಲ |
| ಮಾದರಿ `arguments` ನಲ್ಲಿ ದೋಷಪೂರ್ಣ JSON ನೀಡುತ್ತದೆ | ಸಣ್ಣ ಮಾದರಿಗಳು ಕೆಲವೊಮ್ಮೆ ಅಮಾನ್ಯ JSON ಉತ್ಪಾದಿಸುತ್ತವೆ. `JSON.parse()` ಅನ್ನು try/catch ನಲ್ಲಿ ಆವರಿಸಿ |
| ಮಾದರಿ ಅಸ್ತಿತ್ವದಲ್ಲದ ಟೂಲ್ ಕರೆಮಾಡುತ್ತದೆ | ನಿಮ್ಮ `execute_tool` ಕಾರ್ಯದಲ್ಲಿ ಡಿಫಾಲ್ಟ್ ಹಿಡಿತಗಾರರನನು ಸೇರಿಸಿ |
| ಅನುಸ್ಥಾನಾತ್ಮಕ ಟೂಲ್-ಕಾಲ್ ಲೂಪ್ | ಗರಿಷ್ಠ ಸುತ್ತುಗಳ (ಉದಾ. 5) ಗರಿ ನಿಗದಿಪಡಿಸಿ ಓಡಿ ಹೊರಡುವ ಲೂಪ್ನ ತಡೆಗೆ |

---

## ಮುಖ್ಯ ಸಂಗತಿಗಳು

1. **ಟೂಲ್ನ ಕರೆಮಾಡುವುದು** ಮಾದರಿಗೆ ಉತ್ತರಗಳನ್ನು ಊಹಿಸುವ ಬದಲಾಗಿ ಕಾರ್ಯ ನಿರ್ವಹಿಸಲು ವಿನಂತಿಸುವಿಕೆಯಾಗಿದ್ದು
2. ಮಾದರಿ **ಯಾವುದೇ ಕೋಡ್ ಕಾರ್ಯನಿರ್ವಹಿಸುವುದಿಲ್ಲ**; ಆನ್ಮ ರನ್ ಮಾಡುವುದನ್ನು ನಿಮ್ಮ ಅಪ್ಲಿಕೇಶನ್ ನಿರ್ಧರಿಸುತ್ತದೆ
3. ಟೂಲ್ಗಳು **JSON Schema** ಆಧಾರಿತ OpenAI function-calling ಮಾದರಿಯನ್ನು ಪಾಲಿಸುತ್ತವೆ
4. ಸಂವಾದವು ಬಹು-ಚರಂಡಿ ಮಾದರಿಯನ್ನು ಅನುಸರಿಸುತ್ತದೆ: ಬಳಕೆದಾರ, ನಂತರ ಸಹಾಯಕ (tool_calls), ನಂತರ ಟೂಲ್ (ಫಲಿತಾಂಶ), ನಂತರ ಸಹಾಯಕ (ಕೊನೆಯ ಉತ್ತರ)
5. ಸಹಾಯಮಾಡುವ ಮಾದರಿಯನ್ನು ಎಲ್ಲಾ ಸಂದರ್ಭಗಳಿಗೂ ಅಳವಡಿಸಿ (Qwen 2.5, Phi-4-mini)
6. SDK ಯ `createChatClient()` OpenAI ಕೃತ್ರಿಮ ಕ್ಲೈಯೆಂಟ್ ರಚಿಸುವ ಅಗತ್ಯವಿಲ್ಲದೆ ಟೂಲ್ನ ಕರೆಗಳನ್ನು ಮಾಡಲು ಸುಲಭ ವಿಧಾನ ನೀಡುತ್ತದೆ

---

ಮುಂದುವರೆದು [ಭಾಗ 12: Zava ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್ ಗೆ ವೆಬ್ ಯೂಐ ನಿರ್ಮಾಣ](part12-zava-ui.md) ಗೆ ಹೋಗಿ, ಬಹು ಏಜೆಂಟ್ ಪೈಪ್ಲೈನ್ ಗೆ ನಿಜ-ಸಮಯ ಸ್ಟ್ರೀಮಿಂಗ್ ಇರುವ ಬ್ರೌಸರ್ ಆಧಾರದ ಮುಂಭಾಗವನ್ನು ಸೇರಿಸಿ.

---

[← ಭಾಗ 10: ಕಸ್ಟಮ್ ಮಾದರಿಗಳು](part10-custom-models.md) | [ಭಾಗ 12: Zava ರೈಟರ್ ಯೂಐ →](part12-zava-ui.md)