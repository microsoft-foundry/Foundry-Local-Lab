![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ਭਾਗ 11: ਸਥਾਨਕ ਮਾਡਲਾਂ ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ

> **ਲਕੜੀ:** ਤੁਹਾਡੇ ਸਥਾਨਕ ਮਾਡਲ ਨੂੰ ਬਾਹਰੀ ਫੰਕਸ਼ਨਾਂ (ਟੂਲਾਂ) ਨੂੰ ਕਾਲ ਕਰਨ ਦੇ ਯੋਗ ਬਣਾਓ ਤਾਂ ਜੋ ਇਹ ਅਸਲੀ ਸਮੇਂ ਦਾ ਡਾਟਾ ਪ੍ਰਾਪਤ ਕਰ ਸਕੇ, ਗਣਨਾਵਾਂ ਕਰ ਸਕੇ, ਜਾਂ APIs ਨਾਲ ਇੰਟਰਐਕਟ ਕਰ ਸਕੇ — ਸਾਰੇ ਤੁਹਾਡੇ ਡਿਵਾਈਸ 'ਤੇ ਨਿੱਜੀ ਤੌਰ 'ਤੇ ਚੱਲ ਰਹੇ।

## ਟੂਲ ਕਾਲਿੰਗ ਕੀ ਹੈ?

ਟੂਲ ਕਾਲਿੰਗ (ਜਿਸਨੂੰ **ਫੰਕਸ਼ਨ ਕਾਲਿੰਗ** ਵੀ ਕਿਹਾ ਜਾਂਦਾ ਹੈ) ਇੱਕ ਭਾਸ਼ਾ ਮਾਡਲ ਨੂੰ ਉਹ ਫੰਕਸ਼ਨਾਂ ਚਲਾਉਣ ਦੀ ਬੇਨਤੀ ਕਰਨ ਦਿੰਦਾ ਹੈ ਜੋ ਤੁਸੀਂ ਪਰਿਭਾਸ਼ਿਤ ਕਰਦੇ ਹੋ। ਜਵਾਬ ਦਾ ਅੰਦਾਜ਼ਾ ਲਗਾਉਣ ਦੀ ਬਜਾਏ, ਮਾਡਲ ਸਮਝਦਾ ਹੈ ਕਿ ਕਦੋਂ ਟੂਲ ਮਦਦਗਾਰ ਹੋਵੇਗਾ ਅਤੇ ਤੁਹਾਡੇ ਕੋਡ ਲਈ ਫੰਕਸ਼ਨ ਚਲਾਉਣ ਦੇ ਲਿਏ ਬਣਤਰਵਾਰ ਬੇਨਤੀ ਵਾਪਸ ਕਰਦਾ ਹੈ। ਤੁਹਾਡੀ ਐਪਲੀਕੇਸ਼ਨ ਫੰਕਸ਼ਨ ਚਲਾਉਂਦੀ ਹੈ, ਨਤੀਜਾ ਵਾਪਸ ਭੇਜਦੀ ਹੈ, ਅਤੇ ਮਾਡਲ ਉਸ ਜਾਣਕਾਰੀ ਨੂੰ ਆਪਣੇ ਅੰਤਿਮ ਜਵਾਬ ਵਿੱਚ ਸ਼ਾਮਲ ਕਰਦਾ ਹੈ।

![Tool-calling flow](../../../images/tool-calling-flow.svg)

ਇਹ ਪੈਟਰਨ ਉਹਨਾਂ ਏਜੰਟਾਂ ਦੇ ਨਿਰਮਾਣ ਲਈ ਬਹੁਤ ਜਰੂਰੀ ਹੈ ਜੋ:

- **ਲਾਈਵ ਡਾਟਾ ਵੇਖਦੇ ਹਨ** (ਮੌਸਮ, ਸਟਾਕ ਕੀਮਤਾਂ, ਡਾਟਾਬੇਸ ਕਵੈਰੀਜ਼)
- **ਸਹੀ ਗਣਨਾਵਾਂ ਕਰਦੇ ਹਨ** (ਗਣਿਤ, ਇੱਕਾਈ ਬਦਲਾਅ)
- **ਕਾਰਵਾਈ ਕਰਦੇ ਹਨ** (ਈਮੇਲ ਭੇਜਣਾ, ਟਿਕਟ ਬਣਾਉਣਾ, ਰਿਕਾਰਡ ਅੱਪਡੇਟ ਕਰਨਾ)
- **ਨਿੱਜੀ ਸਿਸਟਮਾਂ ਤੱਕ ਪਹੁੰਚਦੇ ਹਨ** (ਅੰਦਰੂਨੀ APIs, ਫਾਈਲ ਸਿਸਟਮ)

---

## ਟੂਲ ਕਾਲਿੰਗ ਕਿਵੇਂ ਕੰਮ ਕਰਦਾ ਹੈ

ਟੂਲ ਕਾਲਿੰਗ ਪ੍ਰਕਿਰਿਆ ਵਿੱਚ ਚਾਰ ਚਰਣ ਹੁੰਦੇ ਹਨ:

| ਚਰਣ | ਕੀ ਹੁੰਦਾ ਹੈ |
|-------|-------------|
| **1. ਟੂਲਾਂ ਨੂੰ ਪਰਿਭਾਸ਼ਿਤ ਕਰੋ** | ਤੁਸੀਂ ਉਪਲਬਧ ਫੰਕਸ਼ਨਾਂ ਦਾ ਵੇਰਵਾ JSON Schema ਦੀ ਵਰਤੋਂ ਨਾਲ ਦਿੰਦੇ ਹੋ — ਨਾਮ, ਵਰਣਨ, ਅਤੇ ਪੈਰਾਮੀਟਰ |
| **2. ਮਾਡਲ ਫ਼ੈਸਲਾ ਕਰਦਾ ਹੈ** | ਮਾਡਲ ਤੁਹਾਡਾ ਮੈਸੇਜ ਅਤੇ ਟੂਲ ਪਰਿਭਾਸ਼ਾਵਾਂ ਪ੍ਰਾਪਤ ਕਰਦਾ ਹੈ। ਜੇ ਕਿਸੇ ਟੂਲ ਦੀ ਜ਼ਰੂਰਤ ਹੋਵੇ, ਤਾਂ ਇਹ ਟੈਕਸਟ ਜਵਾਬ ਦੇ ਬਦਲੇ `tool_calls` ਰਿਸਪਾਂਸ ਵਾਪਸ ਕਰਦਾ ਹੈ |
| **3. ਸਥਾਨਕ ਤੌਰ 'ਤੇ ਚਲਾਓ** | ਤੁਹਾਡਾ ਕੋਡ ਟੂਲ ਕਾਲ ਨੂੰ ਪਾਰਸ ਕਰਦਾ ਹੈ, ਫੰਕਸ਼ਨ ਚਲਾਉਂਦਾ ਹੈ, ਅਤੇ ਨਤੀਜਾ ਇਕੱਤਰ ਕਰਦਾ ਹੈ |
| **4. ਅੰਤਿਮ ਜਵਾਬ** | ਤੁਸੀਂ ਟੂਲ ਦਾ ਨਤੀਜਾ ਮਾਡਲ ਨੂੰ ਭੇਜਦੇ ਹੋ, ਜੋ ਆਪਣਾ ਅੰਤਿਮ ਜਵਾਬ ਤਿਆਰ ਕਰਦਾ ਹੈ |

> **ਮੁੱਖ ਗੱਲ:** ਮਾਡਲ ਕਦੇ ਵੀ ਕੋਡ ਨਹੀਂ ਚਲਾਉਂਦਾ। ਇਹ ਸਿਰਫ਼ ਬੇਨਤੀ ਕਰਦਾ ਹੈ ਕਿ ਟੂਲ ਕਾਲ ਕੀਤਾ ਜਾਵੇ। ਤੁਹਾਡੀ ਐਪਲੀਕੇਸ਼ਨ ਫੈਸਲਾ ਕਰਦੀ ਹੈ ਕਿ ਇਸ ਬੇਨਤੀ ਨੂੰ ਮਨਜ਼ੂਰ ਕਰਨਾ ਹੈ ਕਿ ਨਹੀਂ — ਇਸ ਤਰ੍ਹਾਂ ਤੁਸੀਂ ਪੂਰੀ ਕਾਬੂ ਵਿੱਚ ਰਹਿੰਦੇ ਹੋ।

---

## ਕਿਹੜੇ ਮਾਡਲ ਟੂਲ ਕਾਲਿੰਗ ਦਾ ਸਮਰਥਨ ਕਰਦੇ ਹਨ?

ਹਰ ਮਾਡਲ ਟੂਲ ਕਾਲਿੰਗ ਦਾ ਸਮਰਥਨ ਨਹੀਂ ਕਰਦਾ। ਮੌਜੂਦਾ Foundry Local ਕੈਟਾਲੌਗ ਵਿੱਚ, ਹੇਠਾਂ ਦਿੱਤੇ ਮਾਡਲਾਂ ਕੋਲ ਟੂਲ ਕਾਲਿੰਗ ਯੋਗਤਾ ਹੈ:

| ਮਾਡਲ | ਸਾਈਜ਼ | ਟੂਲ ਕਾਲਿੰਗ |
|-------|------|:---:|
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

> **ਸੁਝਾਅ:** ਇਸ ਲੈਬ ਲਈ ਅਸੀਂ **qwen2.5-0.5b** ਵਰਤਦੇ ਹਾਂ — ਇਹ ਛੋਟਾ ਹੈ (822 MB ਡਾਊਨਲੋਡ), ਤੇਜ਼ ਹੈ, ਅਤੇ ਇਸਦੇ ਕੋਲ ਭਰੋਸੇਮੰਦ ਟੂਲ ਕਾਲਿੰਗ ਸਮਰਥਨ ਹੈ।

---

## ਸਿੱਖਣ ਦੇ ਉਦੇਸ਼

ਇਸ ਲੈਬ ਦੇ ਅੰਤ ਤੱਕ ਤੁਸੀਂ ਸਮਰੱਥ ਹੋਵੋਗੇ:

- ਟੂਲ ਕਾਲਿੰਗ ਪੈਟਰਨ ਸਮਝਾਓ ਅਤੇ ਕਿਉਂ ਇਹ AI ਏਜੰਟਾਂ ਲਈ ਅਹੰਕਾਰਾ ਹੈ
- OpenAI ਫੰਕਸ਼ਨ-ਕਾਲਿੰਗ ਫਾਰਮੈਟ ਦੀ ਵਰਤੋਂ ਨਾਲ ਟੂਲ ਸਕੀਮਾਵਾਂ ਪਰਿਭਾਸ਼ਿਤ ਕਰੋ
- ਮਲਟੀ-ਟਰਨ ਟੂਲ ਕਾਲਿੰਗ ਸੰਵਾਦ ਦਰਹ ਦਾ ਹੇਣਡਲ ਕਰੋ
- ਟੂਲ ਕਾਲਾਂ ਸਥਾਨਕ ਤੌਰ 'ਤੇ ਚਲਾਓ ਅਤੇ ਨਤੀਜੇ ਮਾਡਲ ਨੂੰ ਵਾਪਸ ਭੇਜੋ
- ਟੂਲ ਕਾਲਿੰਗ ਲਈ ਸਹੀ ਮਾਡਲ ਚੁਣੋ

---

## ਲਾਜ਼ਮੀ ਗੱਲਾਂ

| ਲੋੜ | ਵੇਰਵਾ |
|-------------|---------|
| **Foundry Local CLI** | ਇੰਸਟਾਲ ਕੀਤਾ ਹੋਇਆ ਅਤੇ ਤੁਹਾਡੇ `PATH` ਤੇ ([Part 1](part1-getting-started.md)) |
| **Foundry Local SDK** | Python, JavaScript, ਜਾਂ C# SDK ਇੰਸਟਾਲ ਹੋਇਆ ([Part 2](part2-foundry-local-sdk.md)) |
| **ਟੂਲ ਕਾਲਿੰਗ ਮਾਡਲ** | qwen2.5-0.5b (ਆਪਣੇ ਆਪ ਡਾਉਨਲੋਡ ਹੋਵੇਗਾ) |

---

## ਅਭਿਆਸ

### ਅਭਿਆਸ 1 — ਟੂਲ-ਕਾਲਿੰਗ ਪ੍ਰਕਿਰਿਆ ਨੂੰ ਸਮਝੋ

ਕੋਡ ਲਿਖਣ ਤੋਂ ਪਹਿਲਾਂ, ਇਸ ਕ੍ਰਮ ਚਿੱਤਰ ਨੂੰ ਪੜ੍ਹੋ:

![Tool-calling sequence diagram](../../../images/tool-calling-sequence.svg)

**ਮੁੱਖ ਨਿਰੀਖਣ:**

1. ਤੁਸੀਂ ਟੂਲਾਂ ਨੂੰ ਪਹਿਲਾਂ ਹੀ JSON Schema ਆਬਜੈਕਟ ਵਜੋਂ ਪਰਿਭਾਸ਼ਿਤ ਕਰਦੇ ਹੋ
2. ਮਾਡਲ ਦੇ ਜਵਾਬ ਵਿੱਚ ਸਧਾਰਣ ਸਮੱਗਰੀ ਦੀ ਬਜਾਏ `tool_calls` ਹੁੰਦੇ ਹਨ
3. ਹਰ ਟੂਲ ਕਾਲ ਦਾ ਵਿਲੱਖਣ `id` ਹੁੰਦਾ ਹੈ ਜਿਸਨੂੰ ਨਤੀਜੇ ਵਾਪਸ ਕਰਨ ਵੇਲੇ ਹਵਾਲਾ ਦੇਣਾ ਲਾਜ਼ਮੀ ਹੈ
4. ਜਦ ਮਾਡਲ ਅੰਤਿਮ ਜਵਾਬ ਦਿੰਦਾ ਹੈ ਤਾਂ ਇਹ ਸਾਰੇ ਪਿਛਲੇ ਸੁਨੇਹੇਾਂ ਅਤੇ ਟੂਲ ਦੇ ਨਤੀਜਿਆਂ ਨੂੰ ਵੇਖਦਾ ਹੈ
5. ਇੱਕ ਜਵਾਬ ਵਿੱਚ ਕਈ ਟੂਲ ਕਾਲਾਂ ਹੋ ਸਕਦੀਆਂ ਹਨ

> **ਚਰਚਾ:** ਮਾਡਲ ਸਿੱਧੇ ਫੰਕਸ਼ਨ ਚਲਾਉਣ ਦੀ ਬਜਾਏ ਟੂਲ ਕਾਲ ਕਿਉਂ ਵਾਪਸ ਕਰਦਾ ਹੈ? ਇਹ ਸੁਰੱਖਿਆ ਲਈ ਕਿਹੜੇ ਲਾਭ ਪ੍ਰਦਾਨ ਕਰਦਾ ਹੈ?

---

### ਅਭਿਆਸ 2 — ਟੂਲ ਸਕੀਮਾਵਾਂ ਦੀ ਪਰਿਭਾਸ਼ਾ

ਟੂਲ ਨੂੰ OpenAI ਫੰਕਸ਼ਨ-ਕਾਲਿੰਗ ਫਾਰਮੈਟ ਦੀ ਵਰਤੋਂ ਨਾਲ ਪਰਿਭਾਸ਼ਿਤ ਕੀਤਾ ਜਾਂਦਾ ਹੈ। ਹਰ ਟੂਲ ਲਈ ਲੋੜ ਹੈ:

- **`type`**: ਹਮੇਸ਼ਾਂ `"function"`
- **`function.name`**: ਇੱਕ ਵਰਣਨਾਤਮਕ ਫੰਕਸ਼ਨ ਨਾਮ ( ਜਿਵੇਂ `get_weather` )
- **`function.description`**: ਵਿਸਥਾਰ ਨਾਲ ਵਰਣਨ — ਮਾਡਲ ਇਸਨੂੰ ਟੂਲ ਕਾਲ ਕਰਨ ਦੀ ਲੋੜ ਸਮਝਣ ਲਈ ਵਰਤਦਾ ਹੈ
- **`function.parameters`**: JSON Schema ਆਬਜੈਕਟ जो अपेक्षित arguments ਦਾ ਵੇਰਵਾ ਦਿੰਦਾ ਹੈ

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

> **ਟੂਲ ਵਰਣਨਾਂ ਲਈ ਵਧੀਆ ਅਭਿਆਸ:**
> - ਵਿਸਥਾਰਕ ਹੋਵੋ: "ਕਿਸੇ ਸ਼ਹਿਰ ਲਈ ਮੌਜੂਦਾ ਮੌਸਮ ਲਵੋ" ਵਧੀਆ ਹੈ "ਮੌਸਮ ਲਵੋ" ਨਾਲੋਂ
> - ਪੈਰਾਮੀਟਰਾਂ ਨੂੰ ਸਪਸ਼ਟ ਬਿਆਨ ਕਰੋ: ਮਾਡਲ ਇਨ੍ਹਾਂ ਵਰਣਨਾਂ ਨੂੰ ਸਹੀ ਮੁੱਲ ਭਰਨ ਲਈ ਪੜ੍ਹਦਾ ਹੈ
> - ਲਾਜ਼ਮੀ ਅਤੇ ਵਿਕਲਪਿਕ ਪੈਰਾਮੀਟਰਾਂ ਨੂੰ ਨਿਸ਼ਾਨ ਦਿਓ — ਇਸ ਨਾਲ ਮਾਡਲ ਨੂੰ ਪੁੱਛਣਾ ਕੀ ਹੈ ਫੈਸਲਾ ਕਰਨ ਵਿੱਚ ਮਦਦ ਮਿਲਦੀ ਹੈ

---

### ਅਭਿਆਸ 3 — ਟੂਲ-ਕਾਲਿੰਗ ਉਦਾਹਰਣ ਚਲਾਓ

ਹਰ ਭਾਸ਼ਾ ਸੈਂਪਲ ਦੋ ਟੂਲਾਂ ਨੂੰ ਪਰਿਭਾਸ਼ਿਤ ਕਰਦਾ ਹੈ (`get_weather` ਅਤੇ `get_population`), ਇੱਕ ਪ੍ਰਸ਼ਨ ਭੇਜਦਾ ਹੈ ਜੋ ਟੂਲ ਦੀ ਵਰਤੋਂ ਕਰਦਾ ਹੈ, ਟੂਲ ਸਥਾਨਕ ਤੌਰ 'ਤੇ ਚਲਾਉਂਦਾ ਹੈ, ਅਤੇ ਨਤੀਜੇ ਨੂੰ ਵਾਪਸ ਭੇਜ ਕੇ ਅੰਤਿਮ ਜਵਾਬ ਪ੍ਰਾਪਤ ਕਰਦਾ ਹੈ।

<details>
<summary><strong>🐍 ਪਾਇਥਨ</strong></summary>

**ਲਾਜ਼ਮੀ ਗੱਲਾਂ:**
```bash
cd python
python -m venv venv

# Windows (PowerShell):
venv\Scripts\Activate.ps1
# macOS / ਲਿਨਕਸ:
source venv/bin/activate

pip install -r requirements.txt
```

**ਚਲਾਓ:**
```bash
python foundry-local-tool-calling.py
```

**ਉਮੀਦਵਾਰ ਆਉਟਪੁੱਟ:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({'city': 'London'})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ਕੋਡ ਵਿਵਰਣ** (`python/foundry-local-tool-calling.py`):

```python
# ਟੂਲਜ਼ ਨੂੰ ਫੰਕਸ਼ਨ ਸਕੀਮਾਵਾਂ ਦੀ ਸੂਚੀ ਵਜੋਂ ਪਰਿਭਾਸ਼ਿਤ ਕਰੋ
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

# ਟੂਲਜ਼ ਨਾਲ ਭੇਜੋ — ਮਾਡਲ ਸਮੱਗਰੀ ਦੇ ਬਜਾਏ ਟੂਲ_ਕਾਲ ਵਾਪਸ ਕਰ ਸਕਦਾ ਹੈ
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice="auto",
)

# ਜਾਂਚੋ ਕਿ ਮਾਡਲ ਟੂਲ ਕਾਲ ਕਰਨਾ ਚਾਹੁੰਦਾ ਹੈ ਜਾਂ ਨਹੀਂ
if response.choices[0].message.tool_calls:
    # ਟੂਲ ਨੂੰ ਚਲਾਉ ਅਤੇ ਨਤੀਜਾ ਵਾਪਸ ਭੇਜੋ
    ...
```

</details>

<details>
<summary><strong>🟨 ਜਾਵਾਸਕ੍ਰਿਪਟ (Node.js)</strong></summary>

**ਲਾਜ਼ਮੀ ਗੱਲਾਂ:**
```bash
cd javascript
npm install
```

**ਚਲਾਓ:**
```bash
node foundry-local-tool-calling.mjs
```

**ਉਮੀਦਵਾਰ ਆਉਟਪੁੱਟ:**
```
Starting Foundry Local service...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ਕੋਡ ਵਿਵਰਣ** (`javascript/foundry-local-tool-calling.mjs`):

ਇਹ ਉਦਾਹਰਣ ਮੂਲ Foundry Local SDK ਦੇ `ChatClient` ਦੀ ਵਰਤੋਂ ਕਰਦਾ ਹੈ, ਨਾ ਕਿ OpenAI SDK, ਜੋ ਸਹੂਲਤ ਦਾ ਸਾਵਧਾਨੀ ਪੇਸ਼ ਕਰਦਾ ਹੈ `createChatClient()` ਮੈਥਡ:

```javascript
// ਮਾਡਲ ਔਬਜੈਕਟ ਤੋਂ ਸਿੱਧਾ ChatClient ਪ੍ਰਾਪਤ ਕਰੋ
const chatClient = model.createChatClient();

// ਟੂਲਜ਼ ਨਾਲ ਭੇਜੋ — ChatClient OpenAI-ਅਨੁਕੂਲ ਫਾਰਮੇਟ ਨੂੰ ਹੈਂਡਲ ਕਰਦਾ ਹੈ
const response = await chatClient.completeChat(messages, tools);
const assistantMessage = response.choices[0].message;

// ਟੂਲ ਕਾਲਾਂ ਲਈ ਜਾਂਚ ਕਰੋ
if (assistantMessage.tool_calls && assistantMessage.tool_calls.length > 0) {
    // ਟੂਲ ਚਲਾਓ ਅਤੇ ਨਤੀਜੇ ਵਾਪਸ ਭੇਜੋ
    ...
}
```

</details>

<details>
<summary><strong>🟦 C# (.NET)</strong></summary>

**ਲਾਜ਼ਮੀ ਗੱਲਾਂ:**
```bash
cd csharp
dotnet restore
```

**ਚਲਾਓ:**
```bash
dotnet run toolcall
```

**ਉਮੀਦਵਾਰ ਆਉਟਪੁੱਟ:**
```
Starting Foundry Local service...
Loading model: qwen2.5-0.5b...
User: What is the weather like in London?

Model requested 1 tool call(s):
  → get_weather({"city":"London"})

Final response:
The current weather in London is 18°C and partly cloudy.
```

**ਕੋਡ ਵਿਵਰਣ** (`csharp/ToolCalling.cs`):

C# ਵਿੱਚ ਟੂਲ ਬਨਾਉਣ ਲਈ `ChatTool.CreateFunctionTool` ਸਹਾਇਕ ਵਰਤੀ ਜਾਂਦੀ ਹੈ:

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

### ਅਭਿਆਸ 4 — ਟੂਲ ਕਾਲਿੰਗ ਸੰਵਾਦ ਕ੍ਰਮ

ਸੁਨੇਹਿਆਂ ਦੀ ਬਣਤਰ ਨੂੰ ਸਮਝਣਾ ਬਹੁਤ ਜਰੂਰੀ ਹੈ। ਹੇਠਾਂ ਹਰ ਚਰਣ ਵਿੱਚ `messages` ਐਰੇ ਪੂਰੀ ਤਰ੍ਹਾਂ ਦਿੱਤਾ ਗਿਆ ਹੈ:

**ਚਰਣ 1 — ਪਹਿਲੀ ਬੇਨਤੀ:**
```json
[
  {"role": "system", "content": "You are a helpful assistant. Use the provided tools."},
  {"role": "user", "content": "What is the weather like in London?"}
]
```

**ਚਰਣ 2 — ਮਾਡਲ ਟੂਲ ਕਾਲਾਂ ਵਾਪਸ ਕਰਦਾ ਹੈ (ਸਮੱਗਰੀ ਨਹੀਂ):**
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

**ਚਰਣ 3 — ਤੁਸੀਂ অ্যੱਸਿਸਟੈਂਟ ਸੁਨੇਹਾ ਅਤੇ ਟੂਲ ਨਤੀਜਾ ਸ਼ਾਮਲ ਕਰਦੇ ਹੋ:**
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

**ਚਰਣ 4 — ਮਾਡਲ ਟੂਲ ਨਤੀਜੇ ਨਾਲ ਅੰਤਿਮ ਜਵਾਬ ਦਿੰਦਾ ਹੈ।**

> **ਜ਼ਰੂਰੀ:** ਟੂਲ ਸੁਨੇਹੇ ਵਿੱਚ `tool_call_id` ਨੂੰ ਟੂਲ ਕਾਲ ਦਾ `id` ਮਿਲਦਾ ਹੋਣਾ ਚਾਹੀਦਾ ਹੈ। ਇਸ ਤਰ੍ਹਾਂ ਮਾਡਲ ਨਤੀਜੇ ਨੂੰ ਬੇਨਤੀਆਂ ਨਾਲ ਜੋੜਦਾ ਹੈ।

---

### ਅਭਿਆਸ 5 — ਕਈ ਟੂਲ ਕਾਲਾਂ

ਇੱਕ ਮਾਡਲ ਇੱਕ ਜਵਾਬ ਵਿੱਚ ਕਈ ਟੂਲ ਕਾਲਾਂ ਦੀ ਬੇਨਤੀ ਕਰ ਸਕਦਾ ਹੈ। ਉਪਭੋਗਤਾ ਸੁਨੇਹਾ ਬਦਲ ਕੇ ਕਈ ਕਾਲਾਂ ਚਲਾਉਣ ਦੀ ਕੋਸ਼ਿਸ਼ ਕਰੋ:

```python
# Python ਵਿੱਚ — ਯੂਜ਼ਰ ਮੈਸੇਜ ਬਦਲੋ:
messages = [
    {"role": "system", "content": "You are a helpful assistant. Use the provided tools to answer questions."},
    {"role": "user", "content": "What is the weather and population of London?"},
]
```

```javascript
// ਜਾਵਾਸਕ੍ਰਿਪਟ ਵਿੱਚ - ਯੂਜ਼ਰ ਸੁਨੇਹਾ ਬਦਲੋ:
const messages = [
  { role: "system", content: "You are a helpful assistant. Use the provided tools to answer questions." },
  { role: "user", content: "What is the weather and population of London?" },
];
```

ਮਾਡਲ ਦੋ `tool_calls` ਵਾਪਸ ਕਰੇਗਾ — ਇੱਕ `get_weather` ਲਈ ਅਤੇ ਦੂਜਾ `get_population` ਲਈ। ਤੁਹਾਡਾ ਕੋਡ ਪਹਿਲਾਂ ਹੀ ਸਾਰੇ ਟੂਲ ਕਾਲਾਂ ਨੂੰ ਚੱਕਰ ਵਿੱਚ ਲੈ ਕੇ ਸੰਭਾਲਦਾ ਹੈ।

> **ਕੋਸ਼ਿਸ਼ ਕਰੋ:** ਉਪਯੋਗਕਰਤਾ ਸੁਨੇਹਾ ਬਦਲੋ ਅਤੇ ਮੁੜ ਸੈਂਪਲ ਚਲਾਓ। ਕੀ ਮਾਡਲ ਦੋਹਾਂ ਟੂਲਾਂ ਨੂੰ ਕਾਲ ਕਰਦਾ ਹੈ?

---

### ਅਭਿਆਸ 6 — ਆਪਣਾ ਟੂਲ ਸ਼ਾਮਲ ਕਰੋ

ਕਿਸੇ ਵੀ ਸੈਂਪਲ ਵਿੱਚ ਹੋਰ ਇੱਕ ਟੂਲ ਵਧਾਓ। ਉਦਾਹਰਣ ਵਜੋਂ, ਇੱਕ `get_time` ਟੂਲ ਸ਼ਾਮਲ ਕਰੋ:

1. ਟੂਲ ਸਕੀਮਾ ਪਰਿਭਾਸ਼ਿਤ ਕਰੋ:
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

2. ਚਲਾਉਣ ਦੀ ਲੋਜਿਕ ਸ਼ਾਮਲ ਕਰੋ:
```python
# ਪਾਈਥਨ
def execute_tool(name, arguments):
    if name == "get_time":
        city = arguments.get("city", "Unknown")
        # ਇੱਕ ਅਸਲ ਐਪ ਵਿੱਚ, ਇੱਕ ਸਮਾਂ ਖੇਤਰ ਲਾਇਬ੍ਰੇਰੀ ਦੀ ਵਰਤੋਂ ਕਰੋ
        return json.dumps({"city": city, "time": "14:30 GMT"})
    # ... ਮੌਜੂਦਾ ਟੂਲ ...
```

3. `tools` ਐਰੇ ਵਿੱਚ ਟੂਲ ਜੋੜੋ ਅਤੇ ਇਸ ਪ੍ਰਸ਼ਨ ਨਾਲ ਟੈਸਟ ਕਰੋ: "ਟੋਕਯੋ ਵਿੱਚ ਸਮਾਂ ਕਿੰਨਾ ਹੈ?"

> **ਚੁਣੌਤੀ:** ਇੱਕ ਐਸਾ ਟੂਲ ਸ਼ਾਮਲ ਕਰੋ ਜੋ ਗਣਨਾ ਕਰਦਾ ਹੋਵੇ, ਜਿਵੇਂ `convert_temperature` ਜੋ ਸੈਲਸੀਅਸ ਅਤੇ ਫ਼ਰਨਹਾਈਟ ਵਿੱਚ ਬਦਲ ਕਰਦਾ ਹੈ। ਇਸ ਨਾਲ ਟੈਸਟ ਕਰੋ: "100°F ਨੂੰ ਸੈਲਸੀਅਸ ਵਿੱਚ ਬਦਲੋ।"

---

### ਅਭਿਆਸ 7 — SDK ਦੇ ChatClient ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ (ਜਾਵਾਸਕ੍ਰਿਪਟ)

ਜਾਵਾਸਕ੍ਰਿਪਟ ਸੈਂਪਲ ਪਹਿਲਾਂ ਹੀ SDK ਦੇ ਮੂਲ `ChatClient` ਦੀ ਵਰਤੋਂ ਕਰਦਾ ਹੈ, ਨਾ ਕਿ OpenAI SDK ਸੀਧੇ:

```javascript
import { FoundryLocalManager } from "foundry-local-sdk";

// ChatClient ਸਿੱਧਾ ਮਾਡਲ ਆਬਜੈਕਟ ਤੋਂ ਬਣਾਇਆ ਜਾਂਦਾ ਹੈ
const model = await manager.catalog.getModel("qwen2.5-0.5b");
await model.load();
const chatClient = model.createChatClient();

// completeChat ਦੂਜੇ ਪੈਰਾਮੀਟਰ ਵਜੋਂ ਟੂਲਜ਼ ਨੂੰ ਸਵੀਕਾਰਦਾ ਹੈ
const response = await chatClient.completeChat(messages, tools);
```

ਇਸਦੀ ਤੁਲਨਾ Python ਢੰਗ ਨਾਲ ਕਰੋ ਜੋ ਖੁੱਲ੍ਹੇ OpenAI SDK ਦੀ ਵਰਤੋਂ ਕਰਦਾ ਹੈ:

```python
client = openai.OpenAI(base_url=manager.endpoint, api_key=manager.api_key)
response = client.chat.completions.create(model=model_id, messages=messages, tools=tools)
```

ਦੋਹਾਂ ਪੈਟਰਨ ਸਹੀ ਹਨ। `ChatClient` ਜ਼ਿਆਦਾ ਸੁਵਿਧਾਜਨਕ ਹੈ; OpenAI SDK ਤੁਹਾਨੂੰ OpenAI ਦੇ ਪੂਰੇ ਪੈਰਾਮੀਟਰ ਦੀ ਪਹੁੰਚ ਦਿੰਦਾ ਹੈ।

> **ਕੋਸ਼ਿਸ਼ ਕਰੋ:** ਜਾਵਾਸਕ੍ਰਿਪਟ ਸੈਂਪਲ ਨੂੰ `ChatClient` ਦੀ ਬਜਾਏ OpenAI SDK ਵਰਤੋਂ ਲਈ ਸੋਧੋ। ਤੁਹਾਨੂੰ `import OpenAI from "openai"` ਦੀ ਲੋੜ ਹੋਵੇਗੀ ਅਤੇ `manager.urls[0]` ਤੋਂ ਐਂਡਪਾਇੰਟ ਦੇ ਨਾਲ ਕਲਾਇੰਟ ਬਣਾਉਣਾ ਹੋਵੇਗਾ।

---

### ਅਭਿਆਸ 8 — tool_choice ਨੂੰ ਸਮਝਣਾ

`tool_choice` ਪੈਰਾਮੀਟਰ ਨਿਰਧਾਰਤ ਕਰਦਾ ਹੈ ਕਿ ਮਾਡਲ ਨੂੰ *ਜਰੂਰੀ* ਟੂਲ ਵਰਤਣਾ ਹੈ ਜਾਂ ਆਪਣੀ ਪਸੰਦ ਨਾਲ ਚੁਣ ਸਕਦਾ ਹੈ:

| ਮੁੱਲ | ਵਰਤਾਵ |
|-------|-----------|
| `"auto"` | ਮਾਡਲ ਫੈਸਲਾ ਕਰਦਾ ਹੈ ਕਿ ਟੂਲ ਕਾਲ ਕਰਨੀ ਹੈ ਜਾਂ ਨਹੀਂ (ਮੂਲ ਤੌਰ) |
| `"none"` | ਮਾਡਲ ਕਿਸੇ ਵੀ ਟੂਲ ਨੂੰ ਕਾਲ ਨਹੀਂ ਕਰੇਗਾ, ਭਾਵੇਂ ਦਿੱਤਾ ਗਿਆ ਹੋਵੇ |
| `"required"` | ਮਾਡਲ ਨੂੰ ਘੱਟੋ-ਘੱਟ ਇੱਕ ਟੂਲ ਕਾਲ ਕਰਨੀ ਜ਼ਰੂਰੀ ਹੈ |
| `{"type": "function", "function": {"name": "get_weather"}}` | ਮਾਡਲ ਨੂੰ ਨਿਰਧਾਰਿਤ ਟੂਲ ਕਾਲ ਕਰਨੀ ਜ਼ਰੂਰੀ ਹੈ |

ਪਾਇਥਨ ਸੈਂਪਲ ਵਿੱਚ ਹਰ ਵਿਕਲਪ ਨੂੰ ਕੋਸ਼ਿਸ਼ ਕਰੋ:

```python
# ਮਾਡਲ ਨੂੰ ਜ਼ਬਰਦਸਤੀ get_weather ਕਾਲ ਕਰਨ ਲਈ मजबੂਰ ਕਰੋ
response = client.chat.completions.create(
    model=model_id,
    messages=messages,
    tools=tools,
    tool_choice={"type": "function", "function": {"name": "get_weather"}},
)
```

> **ਨੋਟ:** ਹਰ ਮਾਡਲ ਹਰ `tool_choice` ਵਿਕਲਪ ਨੂੰ ਸਮਰਥਨ ਨਹੀਂ ਕਰਦਾ। ਜੇ ਕੋਈ ਮਾਡਲ `"required"` ਨੂੰ ਸਮਰਥਨ ਨਹੀਂ ਕਰਦਾ, ਤਾਂ ਇਹ ਸੈਟਿੰਗ ਨੂੰ ਅਣਡਿੱਠਾ ਕਰਕੇ `"auto"` ਵਾਂਗ ਵਰਤੋਂ ਕਰ ਸਕਦਾ ਹੈ।

---

## ਆਮ ਗ਼ਲਤੀਆਂ

| ਸਮੱਸਿਆ | ਹੱਲ |
|---------|----------|
| ਮਾਡਲ ਕਦੇ ਵੀ ਟੂਲ ਕਾਲ ਨਹੀਂ ਕਰਦਾ | ਯਕੀਨੀ ਬਣਾਓ ਕਿ ਤੁਸੀਂ ਟੂਲ-ਕਾਲਿੰਗ ਮਾਡਲ ਵਰਤ ਰਹੇ ਹੋ (ਜਿਵੇਂ qwen2.5-0.5b)। ਉਪਰ ਦਿੱਤੀ ਸਾਰਣੀ ਚੈੱਕ ਕਰੋ। |
| `tool_call_id` ਮਿਲਦਾ ਨਹੀਂ | ਹਮੇਸ਼ਾਂ ਟੂਲ ਕਾਲ ਜਵਾਬ ਵਿੱਚੋਂ `id` ਵਰਤੋਂ, ਸਖ਼ਤ ਕੋਡ ਕੀਤੀ ਵੈਲਿਊ ਨਾ ਕਰੋ |
| ਮਾਡਲ `arguments` ਵਿੱਚ ਗਲਤ JSON ਵਾਪਸ ਕਰਦਾ ਹੈ | ਛੋਟੇ ਮਾਡਲ ਕਦੇ ਕਦੇ ਗਲਤ JSON ਬਣਾਉਂਦੇ ਹਨ। `JSON.parse()` ਨੂੰ try/catch ਵਿੱਚ ਰੱਖੋ |
| ਮਾਡਲ ਕਿਸੇ ਅਜਿਹੇ ਟੂਲ ਨੂੰ ਕਾਲ ਕਰਦਾ ਹੈ ਜੋ ਮੌਜੂਦ ਨਹੀਂ | ਆਪਣੇ `execute_tool` ਫੰਕਸ਼ਨ ਵਿੱਚ ਡਿਫੌਲਟ ਹੈਂਡਲਰ ਸ਼ਾਮਲ ਕਰੋ |
| ਟੂਲ ਕਾਲਿੰਗ ਅਨੰਤ ਲੂਪ | ਜ਼ਿਆਦਾ ਚਕਰ (ਉਦਾਹਰਨ ਲਈ 5) ਸੀਮਤ ਕਰੋ ਤਾਂ ਜੋ ਲੂਪ ਨਾ ਫੈਲਣ ਪਾਏ |

---

## ਮੁੱਖ ਸਿੱਖਣੀਆਂ

1. **ਟੂਲ ਕਾਲਿੰਗ** ਮਾਡਲਾਂ ਨੂੰ ਜਵਾਬਾਂ ਦਾ ਅੰਦਾਜ਼ਾ ਲਗਾਉਣ ਦੀ ਬਜਾਏ ਫੰਕਸ਼ਨ ਚਲਾਉਣ ਦੀ ਬੇਨਤੀ ਕਰਨ ਦਿੰਦਾ ਹੈ
2. ਮਾਡਲ ਕਦੇ ਵੀ ਕੋਡ ਨਹੀਂ ਚਲਾਉਂਦਾ; ਤੁਹਾਡੀ ਐਪਲੀਕੇਸ਼ਨ ਤੈਅ ਕਰਦੀ ਹੈ ਕਿ ਕੀ ਚਲਾਉਣਾ ਹੈ
3. ਟੂਲ JSON Schema ਆਬਜੈਕਟ ਵਜੋਂ ਬਨਾਏ ਜਾਂਦੇ ਹਨ ਜੋ OpenAI ਫੰਕਸ਼ਨ-ਕਾਲਿੰਗ ਫਾਰਮੈਟ ਦੀ ਪਾਲਣਾ ਕਰਦੇ ਹਨ
4. ਸੰਵਾਦ ਇੱਕ ਮਲਟੀ-ਟਰਨ ਪੈਟਰਨ ਵਰਤਦਾ ਹੈ: ਯੂਜ਼ਰ, ਫਿਰ ਸਹਾਇਕ (tool_calls), ਫਿਰ ਟੂਲ (ਨਤੀਜੇ), ਫਿਰ ਸਹਾਇਕ (ਅੰਤਿਮ ਜਵਾਬ)
5. ਹਮੇਸ਼ਾਂ ਉਹ ਮਾਡਲ ਵਰਤੋ ਜੋ ਟੂਲ ਕਾਲਿੰਗ ਨੂੰ ਸਹਾਰਦਾ ਹੈ (Qwen 2.5, Phi-4-mini)
6. SDK ਦਾ `createChatClient()` ਇੱਕ ਸੌਖਾ ਤਰੀਕਾ ਹੈ ਜੋ ਬਿਨਾਂ OpenAI ਕਲੀਐਂਟ ਬਣਾਏ ਟੂਲ ਕਾਲਿੰਗ ਦੀ ਬੇਨਤੀ ਕਰਨ ਦਿੰਦਾ ਹੈ

---

[ਭਾਗ 12: ਜਾਵਾ ਕਰੀਏਟਿਵ ਰਾਈਟਰ ਲਈ ਵੈੱਬ UI ਬਣਾਉਣਾ](part12-zava-ui.md) ਤੇ ਜਾਰੀ ਰੱਖੋ ਤਾਂ ਜੋ ਮਲਟੀ-ਏਜੰਟ ਪਾਈਪਲਾਈਨ ਲਈ ਅਸਲੀ ਸਮੇਂ ਸਟ੍ਰੀਮਿੰਗ ਵਾਲਾ ਬਰਾਊਜ਼ਰ-ਅਧਾਰਤ ਫਰੰਟ ਐਂਡ ਸ਼ਾਮਲ ਕੀਤਾ ਜਾ ਸਕੇ।

---

[← ਭਾਗ 10: ਕਸਟਮ ਮਾਡਲ](part10-custom-models.md) | [ਭਾਗ 12: ਜਾਵਾ ਰਾਈਟਰ UI →](part12-zava-ui.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ਅਸਵੀਕਾਰਕਥਾ**:
ਇਹ ਦਸਤਾਵੇਜ਼ AI ਤਰਜਮਾ ਸੇਵਾ [Co-op Translator](https://github.com/Azure/co-op-translator) ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਤਰਜਮਾ ਕੀਤਾ ਗਿਆ ਹੈ। ਜਦੋਂ ਕਿ ਅਸੀਂ ਸਹੀਅਤ ਲਈ ਕੋਸ਼ਿਸ਼ ਕਰਦੇ ਹਾਂ, ਕਿਰਪਾ ਧਿਆਨ ਰੱਖੋ ਕਿ ਆਟੋਮੇਟਿਕ ਤਰਜਮਿਆਂ ਵਿੱਚ ਗਲਤੀਆਂ ਜਾਂ ਅਸਪਸ਼ਟਤਾਵਾਂ ਹੋ ਸਕਦੀਆਂ ਹਨ। ਮੂਲ ਦਸਤਾਵੇਜ਼ ਆਪਣੇ ਮੂਲ ਭਾਸ਼ਾ ਵਿੱਚ ਅਧਿਕਾਰਤ ਸਰੋਤ ਸਮਝਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ। ਮਹੱਤਵਪੂਰਨ ਜਾਣਕਾਰੀ ਲਈ, ਪੇਸ਼ੇਵਰ ਮਨੁੱਖੀ ਤਰਜਮੇ ਦੀ ਸਿਫਾਰਸ਼ ਕੀਤੀ ਜਾਂਦੀ ਹੈ। ਅਸੀਂ ਇਸ ਤਰਜਮੇ ਦੀ ਵਰਤੋਂ ਤੋਂ ਉਪਜੀ ਕਿਸੇ ਵੀ ਗਲਤਫਹਿਮੀ ਜਾਂ ਗਲਤ ਵਿਆਖਿਆਵਾਂ ਲਈ ਜਿੰਮੇਵਾਰ ਨਹੀਂ ਹਾਂ।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->