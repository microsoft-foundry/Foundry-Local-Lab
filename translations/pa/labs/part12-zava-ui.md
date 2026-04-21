![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ਭਾਗ 12: ਜਾਵਾ ਕ੍ਰੀਏਟਿਵ ਰਾਈਟਰ ਲਈ ਵੈੱਬ UI ਬਣਾਉਣਾ

> **ਲਕੜੀ:** ਜਾਵਾ ਕ੍ਰੀਏਟਿਵ ਰਾਈਟਰ ਲਈ ਇੱਕ ਬ੍ਰਾਊਜ਼ਰ-ਅਧਾਰਿਤ ਫਰੰਟ ਐਂਡ ਸ਼ਾਮِل ਕਰੋ ਤਾਂ ਜੋ ਤੁਸੀਂ ਮਲਟੀ-ਏਜੰਟ ਪਾਈਪਲਾਈਨ ਨੂੰ ਰੀਅਲ ਟਾਈਮ ਵਿੱਚ ਦੇਖ ਸਕੋ, ਲਾਈਵ ਏਜੰਟ ਸਥਿਤੀ ਬੈਜ ਅਤੇ ਸਟ੍ਰੀਮ ਕੀਤੀ ਲੇਖ ਸਮੱਗਰੀ, ਸਾਰੇ ਇੱਕ ਸਥਾਨਕ ਵੈੱਬ ਸਰਵਰ ਤੋਂ ਸਰਵ ਕੀਤੇ ਜਾਣ।

[ਭਾਗ 7](part7-zava-creative-writer.md) ਵਿੱਚ ਤੁਸੀਂ ਜਾਵਾ ਕ੍ਰੀਏਟਿਵ ਰਾਈਟਰ ਨੂੰ ਇੱਕ **CLI ਐਪਲੀਕੇਸ਼ਨ** (JavaScript, C#) ਅਤੇ ਇੱਕ **headless API** (Python) ਵਜੋਂ ਖੋਜਿਆ ਸੀ। ਇਸ ਲੈਬ ਵਿੱਚ ਤੁਹਾਡੇ ਕੋਲ ਇੱਕ ਸਾਂਝੀ **ਵੈਨਿਲਾ HTML/CSS/JavaScript** ਫਰੰਟ ਐਂਡ ਨੂੰ ਹਰ ਬੈਕਏਂਡ ਨਾਲ ਜੁੜਨ ਦੀ ਸਮਰੱਥਾ ਹੋਵੇਗੀ ਤਾਂ ਜੋ ਯੂਜ਼ਰਜ਼ ਟਰਮੀਨਲ ਦੀ ਜਗ੍ਹਾ ਬ੍ਰਾਊਜ਼ਰ ਦੁਆਰਾ ਪਾਈਪਲਾਈਨ ਨਾਲ ਇੰਟਰਐਕਟ ਕਰ ਸਕਣ।

---

## ਤੁਸੀਂ ਕੀ ਸਿੱਖੋਗੇ

| ਉਦੇਸ਼ | ਵਰਣਨ |
|-----------|-------------|
| ਬੈਕਏਂਡ ਤੋਂ ਸਥਿਰ ਫਾਈਲਾਂ ਸਰਵ ਕਰਨਾ | ਆਪਣੇ API ਰੂਟ ਦੇ ਨਾਲ HTML/CSS/JS ਡਾਇਰੈਕਟਰੀ ਮਾਊਂਟ ਕਰੋ |
| ਬ੍ਰਾਊਜ਼ਰ ਵਿੱਚ ਸਟ੍ਰੀਮਿੰਗ NDJSON ਖਪਤ ਕਰਨਾ | `ReadableStream` ਨਾਲ ਫੈਚ API ਦੀ ਵਰਤੋਂ ਕਰੋ ਨਿਊਲਾਈਨ-ਡਿਲਿਮਿਟਿਡ JSON ਪੜ੍ਹਨ ਲਈ |
| ਇਕਜੁੱਟ ਸਟ੍ਰੀਮਿੰਗ ਪ੍ਰੋਟੋਕੋਲ | ਯਕੀਨੀ ਬਣਾਓ ਕਿ Python, JavaScript ਅਤੇ C# ਬੈਕਏਂਡ ਇੱਕੋ ਮੈਸੇਜ ਫਾਰਮੈਟ ਜਾਰੀ ਕਰਦੇ ਹਨ |
| ਪ੍ਰਗਟ UI ਅਪਡੇਟਸ | ਏਜੰਟ ਸਥਿਤੀ ਬੈਜ ਅਤੇ ਲੇਖ ਪਾਠ ਨੂੰ ਟੋਕਨ-ਦਰ-ਟੋਕਨ ਸਟ੍ਰੀਮ ਕਰੋ |
| CLI ਐਪ ਵਿੱਚ HTTP ਲੇਅਰ ਸ਼ਾਮਿਲ ਕਰੋ | ਮੌਜੂਦਾ ਔਰਕਸਟਰੇਟਰ ਲਾਜਿਕ ਨੂੰ Express-ਸ਼ੈਲੀ ਸਰਵਰ (JS) ਜਾਂ ASP.NET Core ਮਿਨੀਮਲ API (C#) ਵਿੱਚ ਲਪੇਟੋ |

---

## ਆਰਕੀਟੈਕਚਰ

UI ਇੱਕ ਸਥਿਰ ਫਾਈਲਾਂ ਦਾ ਸੈੱਟ ਹੈ (`index.html`, `style.css`, `app.js`) ਜੋ ਸਾਰੇ ਤਿੰਨ ਬੈਕਏਂਡਾਂ ਦੁਆਰਾ ਸਾਂਝਾ ਕੀਤਾ ਜਾਂਦਾ ਹੈ। ਹਰ ਬੈਕਏਂਡ ਇੱਕੋ ਦੋ ਰੂਟ ਐਕਸਪੋਜ਼ ਕਰਦਾ ਹੈ:

![Zava UI architecture — shared front end with three backends](../../../images/part12-architecture.svg)

| ਰੂਟ | ਵਿਧੀ | ਮਕਸਦ |
|-------|--------|---------|
| `/` | GET | ਸਥਿਰ UI ਸਰਵ ਕਰਦਾ ਹੈ |
| `/api/article` | POST | ਮਲਟੀ-ਏਜੰਟ ਪਾਈਪਲਾਈਨ ਚਲਾਉਂਦਾ ਅਤੇ NDJSON ਸਟ੍ਰੀਮ ਕਰਦਾ ਹੈ |

ਫਰੰਟ ਐਂਡ ਇੱਕ JSON ਬਾਡੀ ਭੇਜਦਾ ਹੈ ਅਤੇ ਜਵਾਬ ਨੂੰ ਨਿਊਲਾਈਨ-ਡਿਲਿਮਿਟਿਡ JSON ਮੈਸੇਜਾਂ ਦੇ ਸਟ੍ਰੀਮ ਵਜੋਂ ਪੜ੍ਹਦਾ ਹੈ। ਹਰੇਕ ਮੈਸੇਜ ਵਿੱਚ ਇੱਕ `type` ਫੀਲਡ ਹੁੰਦੀ ਹੈ ਜਿਸ ਨੂੰ UI ਸਹੀ ਪੈਨਲ ਨੂੰ ਅਪਡੇਟ ਕਰਨ ਲਈ ਵਰਤਦਾ ਹੈ:

| ਮੈਸੇਜ ਪ੍ਰਕਾਰ | ਮਤਲਬ |
|-------------|---------|
| `message` | ਸਥਿਤੀ ਅਪਡੇਟ (ਜੈਵਾਂ "ਚਰਚਾ ਕਰਨ ਵਾਲੇ ਏਜੰਟ ਟਾਸਕ ਚਾਲੂ ਹੋ ਰਿਹਾ ਹੈ...") |
| `researcher` | ਖੋਜ ਨਤੀਜੇ ਤਿਆਰ ਹਨ |
| `marketing` | ਉਤਪਾਦ ਖੋਜ ਨਤੀਜੇ ਤਿਆਰ ਹਨ |
| `writer` | ਲੇਖਕ ਸ਼ੁਰੂ ਹੋਇਆ ਜਾਂ ਖਤਮ ਹੋਇਆ (`{ start: true }` ਜਾਂ `{ complete: true }` ਸੰਝੇਦਾਰ) |
| `partial` | ਲੇਖਕ ਤੋਂ ਇੱਕ ਟੋਕਨ ਸਟ੍ਰੀਮ ਕੀਤਾ ਗਿਆ (`{ text: "..." }` ਸ਼ਾਮਿਲ) |
| `editor` | ਸੰਪਾਦਕ ਫੈਸਲਾ ਤਿਆਰ ਹੈ |
| `error` | ਕੁਝ ਗਲਤ ਹੋਇਆ |

![Message type routing in the browser](../../../images/part12-message-types.svg)

![Streaming sequence — Browser to Backend communication](../../../images/part12-streaming-sequence.svg)

---

## ਤਿਆਰੀਆਂ

- ਸਮਾਪਤ ਕਰੋ [ਭਾਗ 7: ਜਾਵਾ ਕ੍ਰੀਏਟਿਵ ਰਾਈਟਰ](part7-zava-creative-writer.md)
- Foundry Local CLI ਇੰਸਟਾਲ ਕੀਤਾ ਅਤੇ `phi-3.5-mini` ਮਾਡਲ ਡਾਊਨਲੋਡ ਕੀਤਾ
- ਇੱਕ ਆਧੁਨਿਕ ਵੈੱਬ ਬ੍ਰਾਊਜ਼ਰ (Chrome, Edge, Firefox, ਜਾਂ Safari)

---

## ਸਾਂਝਾ UI

ਕੋਈ ਵੀ ਬੈਕਏਂਡ ਕੋਡ ਛੂਹਣ ਤੋਂ ਪਹਿਲਾਂ, ਉਸ ਫਰੰਟ ਐਂਡ ਨੂੰ ਵੇਖੋ ਜੋ ਸਾਰੇ ਤਿੰਨ ਭਾਸ਼ਾ ਟ੍ਰੈਕ ਵਰਤਣਗੇ। ਫਾਈਲਾਂ ਹਨ `zava-creative-writer-local/ui/` ਵਿੱਚ:

| ਫਾਈਲ | ਮਕਸਦ |
|------|---------|
| `index.html` | ਪੇਜ ਵਿਵਸਥਾ: ਇਨਪੁੱਟ ਫਾਰਮ, ਏਜੰਟ ਸਥਿਤੀ ਬੈਜ, ਲੇਖ ਨਿਰੀਕਸ਼ਣ ਇਲਾਕਾ, ਸਮੇਟਣਯੋਗ ਵਿਸਥਾਰ ਪੈਨਲ |
| `style.css` |ਘੱਟ ਤੋਂ ਘੱਟ ਸਟਾਈਲਿੰਗ ਸਥਿਤੀ ਬੈਜ ਦੇ ਰੰਗ ਰਾਜ (ਇੰਤਜ਼ਾਰ, ਚੱਲ ਰਿਹਾ, ਮੁਕੰਮਲ, ਗਲਤੀ) ਲਈ |
| `app.js` | ਫੈਚ ਕਾਲ, `ReadableStream` ਲਾਈਨ ਪੜ੍ਹਨ ਵਾਲਾ, ਅਤੇ DOM ਅਪਡੇਟ ਲੋਜਿਕ |

> **ਸੁਝਾਅ:** ਆਪਣੇ ਬ੍ਰਾਊਜ਼ਰ ਵਿੱਚ ਸਿੱਧਾ `index.html` ਖੋਲ੍ਹੋ ਤਾਂ ਜੋ ਵਿਵਸਥਾ ਦਾ ਪ੍ਰੀਵਿਊ ਕਰ ਸਕੋ। ਕਿਉਂਕਿ ਕੋਈ ਬੈਕਏਂਡ ਨਹੀਂ ਹੈ, ਕੁਝ ਕਾਮ ਨਹੀਂ ਕਰੇਗਾ, ਪਰ ਤੁਸੀਂ ਡھانਚਾ ਵੇਖ ਸਕਦੇ ਹੋ।

### ਸਟ੍ਰੀਮ ਪੜ੍ਹਨ ਵਾਲਾ ਕਿਵੇਂ ਕੰਮ ਕਰਦਾ ਹੈ

`app.js` ਵਿੱਚ ਮੁੱਖ ਫੰਕਸ਼ਨ ਜਵਾਬ ਦੇ ਬਾਡੀ ਨੂੰ ਖੰਡ-ਖੰਡ ਪੜ੍ਹਦਾ ਹੈ ਅਤੇ ਨਿਊਲਾਈਨ ਹੱਦਾਂ 'ਤੇ ਵੰਡਦਾ ਹੈ:

```javascript
async function readStream(response) {
  const reader = response.body.getReader();
  const decoder = new TextDecoder();
  let buffer = "";

  while (true) {
    const { done, value } = await reader.read();
    if (done) break;

    buffer += decoder.decode(value, { stream: true });
    const lines = buffer.split("\n");
    buffer = lines.pop(); // ਅਧੂਰੀ ਅੰਤਲੀ ਲਾਈਨ ਨੂੰ ਰੱਖੋ

    for (const line of lines) {
      const trimmed = line.trim();
      if (!trimmed) continue;
      try {
        const msg = JSON.parse(trimmed);
        if (msg && msg.type) handleMessage(msg);
      } catch { /* skip non-JSON lines */ }
    }
  }
}
```

ਪਾਰਸ ਕੀਤੇ ਹਰੇਕ ਸਨੇਸ਼ ਨੂੰ `handleMessage()` ਨੂੰ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ, ਜੋ `msg.type` ਆਧਾਰ 'ਤੇ ਸਬੰਧਿਤ DOM ਤੱਤ ਨੂੰ ਅਪਡੇਟ ਕਰਦਾ ਹੈ।

---

## ਅਭਿਆਸ

### ਅਭਿਆਸ 1: Python ਬੈਕਏਂਡ UI ਨਾਲ ਚਲਾਉਣਾ

Python (FastAPI) ਵਰਜਨ ਕੋਲ ਪਹਿਲਾਂ ਹੀ ਸਟ੍ਰੀਮਿੰਗ API ਐਂਡਪੋਇੰਟ ਹੈ। ਕੇਵਲ ਬਦਲਾਅ `ui/` ਫੋਲਡਰ ਨੂੰ ਸਥਿਰ ਫਾਈਲਾਂ ਵਜੋਂ ਮਾਊਂਟ ਕਰਨਾ ਹੈ।

**1.1** Python API ਡਾਇਰੈਕਟਰੀ ਵਿੱਚ ਜਾਓ ਅਤੇ ਨਿਰਭਰਤਾਵਾਂ ਇੰਸਟਾਲ ਕਰੋ:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** ਸਰਵਰ ਸ਼ੁਰੂ ਕਰੋ:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** `http://localhost:8000` ਤੇ ਆਪਣੇ ਬ੍ਰਾਊਜ਼ਰ ਨੂੰ ਖੋਲ੍ਹੋ। ਜਾਵਾ ਕ੍ਰੀਏਟਿਵ ਰਾਈਟਰ UI ਅੱਗੇ ਤਿੰਨ ਪਾਠ ਖੇਤਰਾਂ ਅਤੇ ਇੱਕ "Generate Article" ਬਟਨ ਵੇਖਣਾ ਚਾਹੀਦਾ ਹੈ।

**1.4** ਡਿਫਾਲਟ ਮੁੱਲਾਂ ਨਾਲ **Generate Article** 'ਤੇ ਕਲਿੱਕ ਕਰੋ। ਦੇਖੋ ਕਿ ਏਜੰਟ ਸਥਿਤੀ ਬੈਜ "Waiting" ਤੋਂ "Running" ਅਤੇ ਫਿਰ "Done" ਵਿੱਚ ਕਿਵੇਂ ਬਦਲਦੇ ਹਨ ਜਿਵੇਂ ਹਰ ਏਜੰਟ ਆਪਣਾ ਕੰਮ ਮੁਕੰਮਲ ਕਰਦਾ ਹੈ ਅਤੇ ਲੇਖ ਟੋਕਨ-ਬਾਈ-ਟੋਕਨ ਨਿਰੀਕਸ਼ਣ ਪੈਨਲ ਵਿੱਚ ਸਟ੍ਰੀਮ ਹੁੰਦਾ ਹੈ।

> **ਮੱਸਲੇ ਨਾਲ ਨਜਿੱਠਣਾ:** ਜੇ ਪੇਜ UI ਦੀ ਬਜਾਏ ਇੱਕ JSON ਜਵਾਬ ਦਿਖਾਉਂਦਾ ਹੈ, ਯਕੀਨੀ ਬਣਾਓ ਕਿ ਤੁਸੀਂ ਅਪਡੇਟ ਕੀਤਾ `main.py` ਚਲਾ ਰਹੇ ਹੋ ਜੋ ਸਥਿਰ ਫਾਈਲਾਂ ਮਾਊਂਟ ਕਰਦਾ ਹੈ। `/api/article` ਐਂਡਪੋਇੰਟ ਆਪਣੇ ਮੂਲ ਪਾਥ 'ਤੇ ਅਜੇ ਵੀ ਕੰਮ ਕਰਦਾ ਹੈ; ਸਥਿਰ ਫਾਈਲ ਮਾਊਂਟ ਹਰ ਹੋਰ ਰੂਟ ਤੇ UI ਸਰਵ ਕਰਦਾ ਹੈ।

**ਇਹ ਕਿਵੇਂ ਕੰਮ ਕਰਦਾ ਹੈ:** ਅਪਡੇਟ ਕੀਤਾ `main.py` ਹੇਠਾਂ ਇੱਕ ਲਾਈਨ ਜੋੜਦਾ ਹੈ:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

ਇਹ `zava-creative-writer-local/ui/` ਤੋਂ ਹਰ ਫਾਈਲ ਨੂੰ ਇੱਕ ਸਥਿਰ ਐਸੈੱਟ ਵਜੋਂ ਸਰਵ ਕਰਦਾ ਹੈ, `index.html` ਡਿਫਾਲਟ ਦਸਤਾਵੇਜ਼ ਵਜੋਂ। `/api/article` POST ਰੂਟ ਸਥਿਰ ਮਾਊਂਟ ਨਹੀਂ ਹੋਣ ਤੋਂ ਪਹਿਲਾਂ ਰਜਿਸਟਰ ਕੀਤਾ ਗਿਆ ਹੈ, ਇਸ ਲਈ ਇਹ ਤਰਜੀਹੀ ਹੈ।

---

### ਅਭਿਆਸ 2: JavaScript ਵਰਜਨ ਵਿੱਚ ਵੈੱਬ ਸਰਵਰ ਸ਼ਾਮਲ ਕਰੋ

JavaScript ਵਰਜਨ ਇਸ ਸਮੇਂ CLI ਐਪਲੀਕੇਸ਼ਨ (`main.mjs`) ਹੈ। ਇੱਕ ਨਵੀਂ ਫਾਈਲ, `server.mjs`, HTTP ਸਰਵਰ ਦੇ ਪਿੱਛੇ ਉਹੀ ਏਜੰਟ ਮਾਡਿਊਲ ਲਪੇਟ ਕੇ ਸਾਂਝਾ UI ਸਰਵ ਕਰਦੀ ਹੈ।

**2.1** JavaScript ਡਾਇਰੈਕਟਰੀ ਵਿੱਚ ਜਾਓ ਅਤੇ ਨਿਰਭਰਤਾਵਾਂ ਇੰਸਟਾਲ ਕਰੋ:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** ਵੈੱਬ ਸਰਵਰ ਸ਼ੁਰੂ ਕਰੋ:

```bash
node server.mjs
```

```powershell
node server.mjs
```

ਤੁਹਾਨੂੰ ਇਹ ਵੇਖਣਾ ਚਾਹੀਦਾ ਹੈ:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** ਆਪਣੇ ਬ੍ਰਾਊਜ਼ਰ ਵਿੱਚ `http://localhost:3000` ਖੋਲ੍ਹੋ ਅਤੇ **Generate Article** 'ਤੇ ਕਲਿੱਕ ਕਰੋ। ਇੱਕੋ UI JavaScript ਬੈਕਏਂਡ ਦੇ ਖਿਲਾਫ ਵੀ ਬਿਲਕੁਲ ਠੀਕ ਕੰਮ ਕਰਦਾ ਹੈ।

**ਕੋਡ ਸਿੱਖੋ:** `server.mjs` ਖੋਲ੍ਹੋ ਅਤੇ ਮੁੱਖ ਪੈਟਰਨ ਨੋਟ ਕਰੋ:

- **ਸਥਿਰ ਫਾਈਲ ਸਰਵਿੰਗ** ਲਈ Node.js ਦੇ ਆਧਾਰਭੂਤ `http`, `fs`, ਅਤੇ `path` ਮਾਡਿਊਲਾਂ ਦੀ ਵਰਤੋਂ ਹੁੰਦੀ ਹੈ, ਬਾਹਰੀ ਫਰੇਮਵਰਕ ਦੀ ਲੋੜ ਨਹੀਂ।
- **ਪਾਥ-ਟ੍ਰੈਵਰਸਲ ਸੁਰੱਖਿਆ** ਮੰਗੀ ਗਈ ਪਾਥ ਨੂੰ ਆਮੂਕਿਤ ਕਰਦੀ ਹੈ ਅਤੇ ਯਕੀਨੀ ਬਣਾਉਂਦੀ ਹੈ ਕਿ ਇਹ `ui/` ਡਾਇਰੈਕਟਰੀ ਅੰਦਰ ਰਹੇ।
- **NDJSON ਸਟ੍ਰੀਮਿੰਗ** ਲਈ `sendLine()` ਹੈਲਪਰ ਵਰਤਦਾ ਹੈ ਜੋ ਹਰ ਓਬਜੈਕਟ ਨੂੰ ਸੀਰੀਅਲਾਈਜ਼ ਕਰਦਾ ਹੈ, ਅੰਦਰੂਨੀ ਨਿਊਲਾਈਨ ਹਟਾਂਦਾ ਹੈ ਅਤੇ ਪਿੱਛੇ ਨਿਊਲਾਈਨ ਸ਼ਾਮਿਲ ਕਰਦਾ ਹੈ।
- **ਏਜੰਟ ਔਰਕਸਟਰੇਸ਼ਨ** ਪੁਰਾਣੇ `researcher.mjs`, `product.mjs`, `writer.mjs`, ਅਤੇ `editor.mjs` ਮਾਡਿਊਲਾਂ ਨੂੰ ਬਿਨਾਂ ਬਦਲਾਅ ਦੇ ਦੁਬਾਰਾ ਵਰਤਦਾ ਹੈ।

<details>
<summary>server.mjs ਤੋਂ ਮੁੱਖ ਉਧਾਰਣ</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// ਸਿਆਣਪੁਰਖ
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// ਲੇਖਕ (ਸਟ੍ਰੀਮਿੰਗ)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### ਅਭਿਆਸ 3: C# ਵਰਜਨ ਵਿੱਚ ਮਿਨੀਮਲ API ਸ਼ਾਮਿਲ ਕਰੋ

C# ਵਰਜਨ ਇਸ ਵੇਲੇ ਕਨਸੋਲ ਐਪਲੀਕੇਸ਼ਨ ਹੈ। ਇੱਕ ਨਵਾਂ ਪ੍ਰੋਜੈਕਟ, `csharp-web`, ASP.NET Core ਮਿਨੀਮਲ APIs ਵਰਤਦਾ ਹੈ ਜੋ ਇੱਕੋ ਪਾਈਪਲਾਈਨ ਨੂੰ ਵੈੱਬ ਸੇਵਾ ਵਜੋਂ ਐਕਸਪੋਜ਼ ਕਰਦਾ ਹੈ।

**3.1** C# ਵੈੱਬ ਪ੍ਰੋਜੈਕਟ ਵਿੱਚ ਜਾਓ ਅਤੇ ਪੈਕੇਜ ਰਿਸਟੋਰ ਕਰੋ:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** ਵੈੱਬ ਸਰਵਰ ਚਲਾਓ:

```bash
dotnet run
```

```powershell
dotnet run
```

ਤੁਹਾਨੂੰ ਇਹ ਵੇਖਣਾ ਚਾਹੀਦਾ ਹੈ:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** `http://localhost:5000` ਆਪਣੇ ਬ੍ਰਾਊਜ਼ਰ ਵਿੱਚ ਖੋਲ੍ਹੋ ਅਤੇ **Generate Article** 'ਤੇ ਕਲਿੱਕ ਕਰੋ।

**ਕੋਡ ਸਿੱਖੋ:** `csharp-web` ਡਾਇਰੈਕਟਰੀ ਵਿੱਚ `Program.cs` ਖੋਲ੍ਹੋ ਅਤੇ ਨੋਟ ਕਰੋ:

- ਪ੍ਰੋਜੈਕਟ ਫਾਈਲ `Microsoft.NET.Sdk` ਦੀ ਬਜਾਏ `Microsoft.NET.Sdk.Web` ਵਰਤਦੀ ਹੈ ਜੋ ASP.NET Core ਸਮਰਥਨ ਸ਼ਾਮਲ ਕਰਦੀ ਹੈ।
- ਸਥਿਰ ਫਾਈਲਾਂ `UseDefaultFiles` ਅਤੇ `UseStaticFiles` ਦੇ ਜ਼ਰੀਏ ਸਾਂਝੀ `ui/` ਡਾਇਰੈਕਟਰੀ 'ਤੇ ਸਰਵ ਹੁੰਦੀਆਂ ਹਨ।
- `/api/article` ਐਂਡਪੋਇੰਟ NDJSON ਲਾਈਨਾਂ ਸਿੱਧਾ `HttpContext.Response` ਨੂੰ ਲਿਖਦਾ ਹੈ ਅਤੇ ਹਰ ਲਾਈਨ ਤੋਂ ਬਾਅਦ ਫਲਸ਼ ਕਰਦਾ ਹੈ ਤਾਂ ਜੋ ਰੀਅਲ ਟਾਈਮ ਸਟ੍ਰੀਮਿੰਗ ਹੋ ਸਕੇ।
- ਸਾਰਾ ਏਜੰਟ ਲਾਜਿਕ (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) ਕਨਸੋਲ ਵਰਜਨ ਵਾਂਗ ਕਰਵਾਇਆ ਜਾਂਦਾ ਹੈ।

<details>
<summary>csharp-web/Program.cs ਤੋਂ ਮੁੱਖ ਉਧਾਰਣ</summary>

```csharp
app.MapPost("/api/article", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/event-stream; charset=utf-8";

    async Task SendLine(object obj)
    {
        var json = JsonSerializer.Serialize(obj).Replace("\n", "") + "\n";
        await ctx.Response.WriteAsync(json);
        await ctx.Response.Body.FlushAsync();
    }

    // Researcher
    await SendLine(new { type = "message", message = "Starting researcher agent task...", data = new { } });
    var researchResult = RunResearcher(body.Research, feedback);
    await SendLine(new { type = "researcher", message = "Completed researcher task", data = (object)researchResult });

    // Writer (streaming)
    foreach (var update in completionUpdates)
    {
        if (update.ContentUpdate.Count > 0)
        {
            var text = update.ContentUpdate[0].Text;
            await SendLine(new { type = "partial", message = "token", data = new { text } });
        }
    }
});
```

</details>

---

### ਅਭਿਆਸ 4: ਏਜੰਟ ਸਥਿਤੀ ਬੈਜ ਦੀ ਖੋਜ ਕਰੋ

ਹੁਣ ਜਦ ਤੁਹਾਡੇ ਕੋਲ ਕੰਮ ਕਰਨ ਵਾਲਾ UI ਹੈ, ਦੇਖੋ ਕਿ ਫਰੰਟ ਐਂਡ ਸਥਿਤੀ ਬੈਜ ਨੂੰ ਕਿਵੇਂ ਅਪਡੇਟ ਕਰਦਾ ਹੈ।

**4.1** `zava-creative-writer-local/ui/app.js` ਖੋਲ੍ਹੋ।

**4.2** `handleMessage()` ਫੰਕਸ਼ਨ ਲੱਭੋ। ਦੇਖੋ ਕਿ ਕਿਵੇਂ ਇਹ ਮੈਸੇਜ ਕਿਸਮਾਂ ਨੂੰ DOM ਅਪਡੇਟ ਨਾਲ ਜੋੜਦਾ ਹੈ:

| ਮੈਸੇਜ ਪ੍ਰਕਾਰ | UI ਕਾਰਵਾਈ |
|-------------|-----------|
| `"researcher"` ਸ਼ਾਮਿਲ `message` | ਖੋਜਕਾਰ ਬੈਜ ਨੂੰ "ਚੱਲ ਰਿਹਾ" ਸੈਟ ਕਰਦਾ ਹੈ |
| `researcher` | ਖੋਜਕਾਰ ਬੈਜ ਨੂੰ "ਮੁਕੰਮਲ" ਕਰਦਾ ਹੈ ਅਤੇ ਖੋਜ ਨਤੀਜੇ ਪੈਨਲ ਭਰਦਾ ਹੈ |
| `marketing` | ਉਤਪਾਦ ਖੋਜ ਬੈਜ ਨੂੰ "ਮੁਕੰਮਲ" ਕਰਦਾ ਹੈ ਅਤੇ ਪ੍ਰੋਡਕਟ ਮੇਚਜ਼ ਪੈਨਲ ਭਰਦਾ ਹੈ |
| `writer` ਜਿਸ ਵਿੱਚ `data.start` ਹੈ | ਲੇਖਕ ਬੈਜ ਨੂੰ "ਚੱਲ ਰਿਹਾ" ਕਰਦਾ ਹੈ ਅਤੇ ਲੇਖ ਨਿਰੀਕਸ਼ਣ ਸਾਫ਼ ਕਰਦਾ ਹੈ |
| `partial` | ਲੇਖ ਨਿਰੀਕਸ਼ਣ ਵਿੱਚ ਟੋਕਨ ਪਾਠ ਜੋੜਦਾ ਹੈ |
| `writer` ਜਿਸ ਵਿੱਚ `data.complete` ਹੈ | ਲੇਖਕ ਬੈਜ ਨੂੰ "ਮੁਕੰਮਲ" ਕਰਦਾ ਹੈ |
| `editor` | ਸੰਪਾਦਕ ਬੈਜ "ਮੁਕੰਮਲ" ਕਰਦਾ ਹੈ ਅਤੇ ਸੰਪਾਦਕ ਪ੍ਰਤੀਕਿਰਿਆ ਪੈਨਲ ਭਰਦਾ ਹੈ |

**4.3** ਲੇਖ ਖੇਤਰ ਹੇਠਾਂ ਸਮੇਟਣਯੋਗ "ਖੋਜ ਨਤੀਜੇ", "ਉਤਪਾਦ ਮੇਚਜ਼", ਅਤੇ "ਸੰਪਾਦਕ ਪ੍ਰਤੀਕਿਰਿਆ" ਪੈਨਲਾਂ ਖੋਲ੍ਹੋ ਅਤੇ ਹਰ ਏਜੰਟ ਦੁਆਰਾ ਬਣਾਈ ਗਈ ਕੱਚੀ JSON ਨੂੰ ਜਾਅਜ਼ ਹੋਵੋ।

---

### ਅਭਿਆਸ 5: UI ਨੂੰ حسب-ਜ਼ਰੂਰਤ ਬਦਲੋ (ਵਾਧੂ)

ਇਹ ਵਿਸ਼ੇਸ਼ਤਾਵਾਂ ਵਿੱਚੋਂ ਇੱਕ ਜਾਂ ਵੱਧ ਕੋਸ਼ਿਸ਼ ਕਰੋ:

**5.1 ਸ਼ਬਦ ਗਿਣਤੀ ਸ਼ਾਮਲ ਕਰੋ।** ਲੇਖਕ ਖਤਮ ਹੋਣ ਤੋਂ ਬਾਅਦ, ਨਿਰੀਕਸ਼ਣ ਪੈਨਲ ਹੇਠਾਂ ਲੇਖ ਸ਼ਬਦ ਗਿਣਤੀ ਦਿਖਾਓ। ਤੁਸੀਂ ਇਹ `handleMessage` ਵਿੱਚ ਕਰ ਸਕਦੇ ਹੋ ਜਦ `type === "writer"` ਅਤੇ `data.complete` ਸਚ ਹੈ:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 ਇਕ ਦੁਬਾਰਾ ਕੋਸ਼ਿਸ਼ ਦਰਸਾਉਣ ਵਾਲਾ ਸੂਚਕ ਸ਼ਾਮਲ ਕਰੋ।** ਜਦੋਂ ਸੰਪਾਦਕ ਸੋਧ ਮੰਗਦਾ ਹੈ, ਪਾਈਪਲਾਈਨ ਮੁੜ ਚਲਾਉਂਦਾ ਹੈ। ਸਥਿਤੀ ਪੈਨਲ ਵਿੱਚ "Revision 1" ਜਾਂ "Revision 2" ਬੈਨਰ ਦਿਖਾਓ। `"Revision"` ਸ਼ਾਮਿਲ `message` ਟਾਈਪ ਸੁਣੋ ਅਤੇ ਨਵਾਂ DOM ਤੱਤ ਅਪਡੇਟ ਕਰੋ।

**5.3 ਡਾਰਕ ਮੋਡ।** ਇੱਕ ਟੌਗਲ ਬਟਨ ਅਤੇ `<body>` ਨੂੰ `.dark` ਕਲਾਸ ਅਡ ਕਰੋ। `style.css` ਵਿੱਚ `body.dark` ਸੰਕੇਤਕ ਨਾਲ ਪਿਛੋਕੜ, ਪਾਠ ਅਤੇ ਪੈਨਲ ਦੇ ਰੰਗਾਂ ਨੂੰ ਬਦਲੋ।

---

## ਸਾਰਾਂਸ਼

| ਤੁਸੀਂ ਕੀ ਕੀਤਾ | ਕਿਵੇਂ |
|-------------|-----|
| Python ਬੈਕਏਂਡ ਤੋਂ UI ਸਰਵ ਕੀਤਾ | FastAPI ਵਿੱਚ `StaticFiles` ਨਾਲ `ui/` ਫੋਲਡਰ ਮਾਊਂਟ ਕੀਤਾ |
| JavaScript ਵਰਜਨ ਵਿੱਚ HTTP ਸਰਵਰ ਜੋੜਿਆ | Node.js ਦੇ ਆਧਾਰਭੂਤ `http` ਮਾਡਿਊਲ ਨਾਲ `server.mjs` ਬਣਾਇਆ |
| C# ਵਰਜਨ ਵਿੱਚ ਵੈੱਬ API ਸ਼ਾਮਿਲ ਕੀਤਾ | ASP.NET Core ਮਿਨੀਮਲ APIs ਨਾਲ ਨਵਾਂ `csharp-web` ਪ੍ਰੋਜੈਕਟ ਬਣਾਇਆ |
| ਬ੍ਰਾਊਜ਼ਰ ਵਿੱਚ ਸਟ੍ਰੀਮਿੰਗ NDJSON ਖਪਤ ਕੀਤੀ | `fetch()` ਦੇ ਨਾਲ `ReadableStream` ਅਤੇ ਖੇਤਰ-ਬਾਈ-ਖੇਤਰ JSON ਪੜ੍ਹਾਈ ਵਰਤੀ |
| UI ਨੂੰ ਰੀਅਲ ਟਾਈਮ ਵਿੱਚ ਅਪਡੇਟ ਕੀਤਾ | ਮੈਸੇਜ ਕਿਸਮਾਂ ਨੂੰ DOM ਅਪਡੇਟ (ਬੈਜ, ਪਾਠ, ਸਮੇਟਣਯੋਗ ਪੈਨਲ) ਨਾਲ ਜੋੜਿਆ |

---

## ਮੁੱਖ ਸਿੱਖਣ ਯੋਗ ਗੱਲਾਂ

1. ਇੱਕ **ਸਾਂਝਾ ਸਥਿਰ ਫਰੰਟ ਐਂਡ** ਕਿਸੇ ਵੀ ਬੈਕਏਂਡ ਨਾਲ ਕੰਮ ਕਰ ਸਕਦਾ ਹੈ ਜੋ ਇੱਕੋ ਸਟ੍ਰੀਮਿੰਗ ਪ੍ਰੋਟੋਕੋਲ ਬੋਲਦਾ ਹੈ, ਜੋ OpenAI-ਅਨੁਕੂਲ API ਪੈਟਰਨ ਦੀ ਵੈਲਯੂ ਨੂੰ ਮਜ਼ਬੂਤ ਕਰਦਾ ਹੈ।
2. **ਨਿਊਲਾਈਨ-ਡਿਲਿਮਿਟਿਡ JSON (NDJSON)** ਇੱਕ ਸਿਧਾ ਸਟ੍ਰੀਮਿੰਗ ਫਾਰਮੈਟ ਹੈ ਜੋ ਬ੍ਰਾਊਜ਼ਰ `ReadableStream` API ਨਾਲ ਕੁਦਰਤੀ ਤੌਰ ਤੇ ਕੰਮ ਕਰਦਾ ਹੈ।
3. **Python ਵਰਜਨ** ਨੂੰ ਸਭ ਤੋਂ ਘੱਟ ਬਦਲਾਅ ਦੀ ਲੋੜ ਸੀ ਕਿਉਂਕਿ ਇਸ ਕੋਲ ਪਹਿਲਾਂ ਹੀ FastAPI ਐਂਡਪੋਇੰਟ ਸੀ; JavaScript ਅਤੇ C# ਵਰਜਨਾਂ ਨੂੰ ਇਕ ਧੀਮੀ HTTP ਲੈਅਰ ਲੱਗੀ।
4. UI ਨੂੰ **ਵੈਨਿਲਾ HTML/CSS/JS** ਵਜੋਂ ਰੱਖਣਾ ਵਰਕਸ਼ਾਪ ਸਿੱਖਣ ਵਾਲਿਆਂ ਲਈ ਬਿਲਡ ਟੂਲਜ਼, ਫਰੇਮਵਰਕ ਡਿਪੈਂਡੈਂਸੀਜ਼, ਅਤੇ ਵਾਧੂ ਜਟਿਲਤਾ ਤੋਂ ਬਚਾਉਂਦਾ ਹੈ।
5. ਇੱਕੋ ਏਜੰਟ ਮਾਡਿਊਲਾਂ (ਖੋਜਕਾਰ, ਉਤਪਾਦ, ਲੇਖਕ, ਸੰਪਾਦਕ) ਬਿਨਾਂ ਤਬਦੀਲੀ ਦੇ ਦੁਬਾਰਾ ਵਰਤੇ ਗਏ; ਸਿਰਫ਼ ਟ੍ਰਾਂਸਪੋਰਟ ਲੇਅਰ ਬਦਲਿਆ।

---

## ਹੋਰ ਪੜ੍ਹਾਈ

| ਝੜਪ | ਲਿੰਕ |
|----------|------|
| MDN: Readable Streams ਦੀ ਵਰਤੋਂ | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI ਸਥਿਰ ਫਾਈਲਾਂ | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core ਸਥਿਰ ਫਾਈਲਾਂ | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSON ਵਿਸ਼ੇਸ਼ਤਾ | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

[ਭਾਗ 13: ਵਰਕਸ਼ਾਪ ਮੁਕੰਮਲ](part13-workshop-complete.md) ਵੱਲ ਜਾਰੀ ਰੱਖੋ ਸਾਰੀ ਵਰਕਸ਼ਾਪ ਵਿੱਚ ਤੁਸੀਂ ਜੋ ਕੁਝ ਬਣਾਇਆ ਉਸਸ ਦਾ ਸਾਰਾਂਸ਼ ਲਈ।

---
[← ਭਾਗ 11: ਟੂਲ ਕਾਲਿੰਗ](part11-tool-calling.md) | [ਭਾਗ 13: ਵਰਕਸ਼ਾਪ ਪੂਰਾ →](part13-workshop-complete.md)

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ਡਿਸਕਲੇਮਰ**:  
ਇਹ ਦਸਤਾਵੇਜ਼ ਏਆਈ ਅਨੁਵਾਦ ਸੇਵਾ [Co-op Translator](https://github.com/Azure/co-op-translator) ਦੀ ਵਰਤੋਂ ਕਰ ਕੇ ਅਨੁਵਾਦ ਕੀਤਾ ਗਿਆ ਹੈ। ਜਦੋਂ ਕਿ ਅਸੀਂ ਸਹੀਤਾ ਲਈ ਕੋਸ਼ਿਸ਼ ਕਰਦੇ ਹਾਂ, ਕ੍ਰਿਪਾ ਕਰਕੇ ਧਿਆਨ ਵਿੱਚ ਰੱਖੋ ਕਿ ਸਵੈਚਾਲਿਤ ਅਨੁਵਾਦ ਵਿੱਚ ਤ੍ਰੁੱਟੀਆਂ ਜਾਂ ਅਣਸਹੀ ਸੂਚਨਾਵਾਂ ਹੋ ਸਕਦੀਆਂ ਹਨ। ਮੂਲ ਦਸਤਾਵੇਜ਼ ਆਪਣੇ ਮੂਲ ਭਾਸ਼ਾ ਵਿੱਚ ਪ੍ਰਮਾਣਿਕ ਸਰੋਤ ਮੰਨਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ। ਜਰੂਰੀ ਜਾਣਕਾਰੀ ਲਈ, ਪੇਸ਼ੇਵਰ ਮਾਨਵ ਅਨੁਵਾਦ ਦੀ ਸਿਫਾਰਸ਼ ਕੀਤੀ ਜਾਂਦੀ ਹੈ। ਅਸੀਂ ਇਸ ਅਨੁਵਾਦ ਦੇ ਇਸਤੇਮਾਲ ਤੋਂ ਪੈਦਾ ਹੋਣ ਵਾਲੀਆਂ ਕਿਸੇ ਵੀ ਗਲਤਫਹਿਮੀਆਂ ਜਾਂ ਭੁਲੇਖਿਆਂ ਲਈ ਜ਼ਿੰਮੇਵਾਰ ਨਹੀਂ ਹਾਂ।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->