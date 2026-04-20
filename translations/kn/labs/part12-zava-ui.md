![Foundry Local](https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg)

# ಭಾಗ 12: ಜಾವಾ ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್‌ಗಾಗಿ ವೆಬ್ UI ನಿರ್ಮಾಣ

> **ಗೋಲ:** ಜಾವಾ ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್‌ಗೆ ಬ್ರೌಸರ್ ಆಧಾರಿತ ಫ್ರಂಟ್ ಎಂಡ್ ಸೇರಿಸಿ, ಇದರಿಂದ ನೀವು ಬೃಹತ್ ಏಜೆಂಟ್ ಪೈಪ್ಲೈನ್‌ನ ಕಾರ್ಯಾಚರಣೆಯನ್ನು ರಿಯಲ್ ಟೈಮ್‌ನಲ್ಲಿ ವೀಕ್ಷಿಸಬಹುದು, ಲೈವ್ ಏಜೆಂಟ್ ಸ್ಥಿತಿ ಬ್ಯಾಡ್ಜ್‌ಗಳು ಮತ್ತು ಸ್ಟ್ರೀಮ್ ಪ್ರবন্ধಿತ ಲೇಖನ ಪಠ್ಯವನ್ನು ಒಬ್ಬಲ್ಲೊಂದು ಸ್ಥಳೀಯ ವೆಬ್ ಸರ್ವರ್‌ನಿಂದ ಕಾರ್ಯಗತಗೊಳಿಸಿ.

ನೀವು [ಭಾಗ 7](part7-zava-creative-writer.md) ನಲ್ಲಿ ಜಾವಾ ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್ ಅನ್ನು **CLI ಅಪ್ಲಿಕೇಶನ್** (ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್, C#) ಮತ್ತು **ಹೆಡ್‌ಲೆಸ್ API** (ಪೈಥಾನ್) ಯಾಗಿ ಅನ್ವೇಷಿಸಿದ್ದಿರಿ. ಈ ಲ್ಯಾಬ್‌ನಲ್ಲಿ, ನೀವು ಸರ್ವರ್ ಫ್ರಂಟ್ ಎಂಡ್ ಅನ್ನು ಪ್ರತಿಯೊಂದು ಬ್ಯಾಕಎಂಡ್ ಗೆ ಸಂಪರ್ಕಿಸಿ, ಬಳಕೆದಾರರು ಟರ್ಮಿನಲ್ ಬದಲು ಬ್ರೌಸರ್ ಮೂಲಕ ಪೈಪ್ಲೈನ್ ಜೊತೆ ಸಂವಾದ ಮಾಡಬಹುದು.

---

## ನೀವು ಏನನ್ನು ಕಲಿಯುತ್ತೀರಿ

| ಗುರಿ | ವಿವರಣೆ |
|-----------|-------------|
| ಬ್ಯಾಕಎಂಡ್‌ನಿಂದ ಸ್ಥಿರ ಫೈಲ್‌ಗಳನ್ನು ಸರ್ವ್ ಮಾಡಿ | ನಿಮ್ಮ API ಮಾರ್ಗದ ಪಕ್ಕದಲ್ಲಿ HTML/CSS/JS ಡೈರೆಕ್ಟರಿ ಅನ್ನು ಮೋಂಟು ಮಾಡಿ |
| ಬ್ರೌಸರ್‌ನಲ್ಲಿ ಸ್ಟ್ರೀಮ್ ಆಗುವ NDJSON ಸೇವಿಸಿ | ಹೊಸ ಲೈನಿನಿಂದ ವಿಭಜಿಸಲ್ಪಟ್ಟ JSON ಓದવા Fetch API ಹಾಗೂ `ReadableStream` ಬಳಸಿ |
| ಏಕೀಕೃತ ಸ್ಟ್ರೀಮಿಂಗ್ ಪ್ರೋಟೋಕಾಲ್ | ಪೈಥಾನ್, ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮತ್ತು C# ಬ್ಯಾಕೆಂಡ್‌ಗಳು ಒಂದೇ ಸಂದೇಶದ ಸ್ವರೂಪವನ್ನು ಹೊರಬಿಡುವಂತೆ ಖಚಿತಪಡಿಸು |
| ಪ್ರೋಗ್ರೆಸಿವ್ UI ನವೀಕರಣಗಳು | ಏಜೆಂಟ್ ಸ್ಥಿತಿ ಬ್ಯಾಡ್ಜ್‌ಗಳನ್ನು ಮತ್ತು ಲೇಖನ ಪಠ್ಯವನ್ನು ಟೋಕೆನ್ ಮೂಲಕ ಸ್ಟ್ರೀಮ್‌ನಲ್ಲಿ ನವೀಕರಿಸು |
| CLI ಅಪ್ಲಿಕೇಶನ್‌ಗೆ HTTP ಲೇಯರ್ನ್ನು ಸೇರಿಸಿ | ಲಾಜಿಕ್ ಅನ್ನು Express ಶೈಲಿಯ ಸರ್ವರ್ (JS) ಅಥವಾ ASP.NET ಕೊರ್ ಮಿನಿಮಲ್ API (C#) ನಲ್ಲಿ ವೆರಪಿಂಗ್ ಮಾಡು |

---

## ವಾಸ್ತುಶಿಲ್ಪ

UI ಒಂದು ಜೋಡಿ ಸ್ಥಿರ ಫೈಲ್‌ಗಳಾಗಿವೆ (`index.html`, `style.css`, `app.js`) ಹಾಗು ಎಲ್ಲಾ ಮೂರು ಬ್ಯಾಕೆಂಡ್‌ಗಳಿಂದ ಹಂಚಿಕೊಳ್ಳಲಾಗುತ್ತದೆ. ಪ್ರತಿ ಬ್ಯಾಕೆಂಡ್ ಎರಡೂ ಗಮನಾರ್ಹ ಮಾರ್ಗಗಳನ್ನು ಬಹಿರಂಗಪಡಿಸುತ್ತದೆ:

![Zava UI ವಾಸ್ತುಶಿಲ್ಪ — ಮೂವರು ಬ್ಯಾಕೆಂಡ್‌ಗಳಿಗೆ ಹಂಚಿಕೆಯ ಫ್ರಂಟ್ ಎಂಡ್](../../../images/part12-architecture.svg)

| ಮಾರ್ಗ | ವಿಧಾನ | ಉದ್ದೇಶ |
|-------|--------|---------|
| `/` | GET | ಸ್ಥಿರ UI ಸರ್ವ್ ಮಾಡುತ್ತದೆ |
| `/api/article` | POST | ಬಹು ಏಜೆಂಟ್ ಪೈಪ್ಲೈನ್ ಕಾರ್ಯಗತಗೊಳಿಸಿ ಮತ್ತು NDJSON ಸ್ಟ್ರೀಮ್ ಮಾಡು |

ಫ್ರಂಟ್ ಎಂಡ್ JSON ದೇಹ ಅನ್ನು ಕಳುಹಿಸುತ್ತದೆ ಮತ್ತು ಪ್ರತಿಕ್ರಿಯೆಯನ್ನು ಹೊಸ ಲೈನ್-ವಿಭಜಿತ JSON ಸಂದೇಶಗಳ ಸ್ಟ್ರೀಮಾಗಿ ಓದುತ್ತದೆ. ಪ್ರತಿ ಸಂದೇಶಕ್ಕೆ UI ಸೂಕ್ತ ಫಲಕವನ್ನು ನವೀಕರಿಸಲು ಬಳಸುವ `type` ಕ್ಷೇತ್ರವಿದೆ:

| ಸಂದೇಶದ ಪ್ರಕಾರ | ಅರ್ಥ |
|-------------|---------|
| `message` | ಸ್ಥಿತಿ ನವೀಕರಣ (ಉದಾ: "ಸಂಶೋಧಕ ಏಜೆಂಟ್ ಕಾರ್ಯಾರಂಭ...") |
| `researcher` | ಸಂಶೋಧನಾ ಫಲಿತಾಂಶಗಳು ಸಿದ್ಧವಿವೆ |
| `marketing` | ಉತ್ಪನ್ನ ಹುಡುಕಾಟ ಫಲಿತಾಂಶಗಳು ಸಿದ್ಧವಿವೆ |
| `writer` | ಲೇಖಕ ಆರಂಭಿಸಿದ ಅಥವಾ ಮುಗಿಸಿದ (ದಾಖಲೆ `{ start: true }` ಅಥವಾ `{ complete: true }` ಹೊಂದಿದೆ) |
| `partial` | ಲೇಖಕರಿಂದ ಒಬ್ಬ ಟೋಕನ್ ಸ್ಟ್ರೀಮ್ ಆಗಿ ಬಂದಿದೆ (ದಾಖಲೆ `{ text: "..." }`) |
| `editor` | ಸಂಪಾದಕ ತೀರ್ಮಾನ ಸಿದ್ಧವಾಗಿದೆ |
| `error` | ಏನೋ ದೋಷವಾಯಿತು |

![ಬ್ರೌಸರ್‌ನಲ್ಲಿ ಸಂದೇಶ ಪ್ರಕಾರಗಳ ಮಾರ್ಗದರ್ಶಿಕೆ](../../../images/part12-message-types.svg)

![ಸ್ಟ್ರೀಮಿಂಗ್ ಕ್ರಮ — ಬ್ರೌಸರ್ ರಿಂದ ಬ್ಯಾಕೆಂಡ್ ಸಂವಹನ](../../../images/part12-streaming-sequence.svg)

---

## ಪೂರ್ವಾಪೇಕ್ಷಿತ ಅಗತ್ಯಗಳು

- [ಭಾಗ 7: ಜಾವಾ ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್](part7-zava-creative-writer.md) ಪೂರ್ಣಗೊಳಿಸಿರಬೇಕು
- Foundry Local CLI ಸ್ಥಾಪಿತವಾಗಿದೆ ಮತ್ತು `phi-3.5-mini` ಮಾದರಿ ಡೌನ್ಲೋಡ್ ಮಾಡಲಾಗಿದೆ
- ಆಧುನಿಕ ವೆಬ್ ಬ್ರೌಸರ್ (Chrome, Edge, Firefox ಅಥವಾ Safari)

---

## ಹಂಚಲಾಗಿರುವ UI

ಯಾವುದೇ ಬ್ಯಾಕೆಂಡ್ ಕೋಡ್ ಸ್ಪರ್ಶಿಸುವ ಮೊದಲು, ಮೂರು ಭಾಷಾ ಟ್ರ್ಯಾಕ್ ನ್ನು ಬಳಸಿ ಇರುವ ಫ್ರಂಟ್ ಎಂಡ್ ಅನ್ನು ಪರಿಶೀಲಿಸಿ. ಈ ಫೈಲ್‌ಗಳು `zava-creative-writer-local/ui/` ನಲ್ಲಿ ಇವೆ:

| ಫೈಲ್ | ಉದ್ದೇಶ |
|------|---------|
| `index.html` | ಪುಟ ವಿನ್ಯಾಸ: ಇನ್ಪುಟ್ ಫಾರ್ಮ್, ಏಜೆಂಟ್ ಸ್ಥಿತಿ ಬ್ಯಾಡ್ಜ್‌ಗಳು, ಲೇಖನ ಔಟ್ಪುಟ್ ಪ್ರದೇಶ, ಮುರಿಯಬಹುದಾದ ವಿವರ ಫಲಕಗಳು |
| `style.css` | ಸ್ಥಿತಿ ಬ್ಯಾಜ್ ವರ್ಣ ಅವಸ್ಥೆಗಳ (ನಿರೀಕ್ಷಣೆ, ಕಾರ್ಯನಿರ್ವಹಣೆ, ಪೂರ್ಣ, ದೋಷ) ಮೂಲಭೂತ ಶೈಲಿ |
| `app.js` | Fetch ಕರೆ, `ReadableStream` ಲೈನ್ ಓದುಗ, ಮತ್ತು DOM ನವೀಕರಣ ಲಾಜಿಕ್ |

> **ಉಪಾಯ:** `index.html` ಅನ್ನು ನೇರವಾಗಿ ಬ್ರೌಸರ್‌ನಲ್ಲಿ ತೆರೆಯಿರಿ ಮತ್ತು ವಿನ್ಯಾಸವನ್ನು ಪೂರ್ವನೋಟ ಮಾಡಿ. ಯಾವುದೇ ಕಾರ್ಯಾಚರಣೆ ಇಲ್ಲದೆ ಇರುವುದರಿಂದ, ಬ್ಯಾಕೆಂಡ್ ಇಲ್ಲದೆ ಇದನ್ನು ನೋಡಬಹುದು.

### ಸ್ಟ್ರೀಮ್ ಓದುಗ ಹೇಗೆ ಕಾರ್ಯನಿರ್ವಹಿಸುತ್ತದೆ

`app.js` ನಲ್ಲಿ ಪ್ರಮುಖ ಕಾರ್ಯ ಪ್ರತಿಕ್ರಿಯೆಯ ದೇಹವನ್ನು ತುಣುಕು ತುಣುಕುಗಳಲ್ಲಿ ಓದಿತು, նոր ಸಾಲಿನ ಮಿತಿಗಳಲ್ಲಿ ವಿಭಜಿಸುತ್ತದೆ:

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
    buffer = lines.pop(); // ಅಪೂರಣಾದ ಕೊನೆಯ ಸಾಲನ್ನು ಉಳಿಸಿ

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

ಪ್ರತಿ ವಿಶ್ಲೇಷಿಸಿದ ಸಂದೇಶವನ್ನು `handleMessage()` ಗೆ ಕಳುಹಿಸುವುದರಿಂದ, ಇದು `msg.type` ಆಧರಿಸಿ ಸಂಬಂಧಿಸಿದ DOM ಅಂಶಗಳನ್ನು ನವೀಕರಿಸುತ್ತದೆ.

---

## ಅಭ್ಯಾಸಗಳು

### ಅಭ್ಯಾಸ 1: Python ಬ್ಯಾಕೆಂಡ್‌ ಅನ್ನು UI ಜೊತೆ ಚಾಲನೆ ಮಾಡುವುದು

ಪೈಥಾನ್ (FastAPI) ಆವೃತ್ತಿಗೆ ಈಗಾಗಲೇ ಸ್ಟ್ರೀಮಿಂಗ್ API ಎಂಡ್ಪಾಯಿಂಟ್ ಇದೆ. ಒಂದೇ ಬದಲಾವಣೆ ಎಂದರೆ `ui/` ಫೋಲ್ಡರ್ ಅನ್ನು ಸ್ಥಿರ ಫೈಲ್‌ನಂತೆ ಮೋಂಟು ಮಾಡುವುದು.

**1.1** ಪೈಥಾನ್ API ಡೈರೆಕ್ಟರಿಯಲ್ಲಿ ಹೋಗಿ ಅವಶ್ಯಕತೆಗಳನ್ನು ಸ್ಥಾಪಿಸಿ:

```bash
cd zava-creative-writer-local/src/api
pip install -r requirements.txt
```

```powershell
cd zava-creative-writer-local\src\api
pip install -r requirements.txt
```

**1.2** ಸರ್ವರ್ ಆರಂಭಿಸಿ:

```bash
uvicorn main:app --reload --port 8000
```

```powershell
uvicorn main:app --reload --port 8000
```

**1.3** ಬ್ರೌಸರ್‌ನಲ್ಲಿ `http://localhost:8000` ತೆರೆಯಿರಿ. ನೀವು ಮೂರು ಪಠ್ಯ ಕ್ಷೇತ್ರಗಳೊಂದಿಗೆ ಜಾವಾ ಕ್ರಿಯೇಟಿವ್ ರೈಟರ್ UI ಕಾಣಬಹುದು ಮತ್ತು "Generate Article" ಬಟನ್.

**1.4** ಡೀಫಾಲ್ಟ್ ಮೌಲ್ಯಗಳೊಂದಿಗೆ **Generate Article** ಕ್ಲಿಕ್ ಮಾಡಿ. ಪ್ರತಿಯೊಂದು ಏಜೆಂಟ್ ಕಾರ್ಯ ಪೂರ್ಣಗೊಳ್ಳುವಂತೆ, "Waiting" ನಿಂದ "Running" ನಂತರ "Done" ಆಗಿ ಏಜೆಂಟ್ ಸ್ಥಿತಿ ಬ್ಯಾಡ್ಜ್‌ಗಳು ಬದಲಾಗುತ್ತದೆ ಮತ್ತು ಲೇಖನ ಪಠ್ಯವು ಟೋಕನ್ ಮೂಲಕ ಔಟ್ಪುಟ್ ಫಲಕದಲ್ಲಿ ಸ್ಟ್ರೀಮ್ ಆಗುತ್ತದೆ.

> **ದೋಷ ನಿವಾರಣೆ:** ಪುಟದಲ್ಲಿ UI ಬದಲು JSON ಪ್ರತಿಕ್ರಿಯೆ ಕಾಣಿಸಿದರೆ, ಸ್ಥಿರ ಫೈಲನ್ನು ಮೋಂಟು ಮಾಡುತ್ತಿರುವ ನವೀಕರಿಸಿದ `main.py` ಯಲ್ಲಿ ಕಾರ್ಯಾಚರಣೆ ಮಾಡುವ ನಿಜವಾಗಿದ್ದನ್ನ ನೋಡಿಕೊಳ್ಳಿ. `/api/article` ಎಂಡ್ಪಾಯಿಂಟ್ ಮೂಲ ಮಾರ್ಗದಲ್ಲಿ ಕೆಲಸ ಮಾಡುತ್ತದೆ; ಸ್ಥಿರ ಫೈಲ್ ಮೋಂಟು ಬೇರೆಯ ಮಾರ್ಗಗಳಲ್ಲಿ UI ನೀಡುತ್ತದೆ.

**ಇದು ಹೇಗೆ ಕಾರ್ಯಗತಗೊಳ್ಳುತ್ತದೆ:** ನವೀಕರಿಸಿದ `main.py` ಕೆಳಗಿನ ಸಾಲನ್ನು ಸೇರಿಸುತ್ತದೆ:

```python
app.mount("/", StaticFiles(directory=str(ui_dir), html=True), name="ui")
```

ಇದು `zava-creative-writer-local/ui/` ಯಿಂದ ಪ್ರತಿಯೊಂದು ಫೈಲನ್ನೂ ಸ್ಥಿರ ಆಸ್ತಿ ಆಗಿ ಸರ್ವ್ ಮಾಡುತ್ತದೆ, `index.html` ನ್ನು ಡೀಫಾಲ್ಟ್ ಡಾಕ್ಯುಮೆಂಟ್ ಆಗಿ ಬಳಸುತ್ತದೆ. `/api/article` POST ಮಾರ್ಗ ಸ್ಥಿರ ಮೋಂಟು ಮಾಡುತ್ತಿದ್ದ ಮಾರ್ಗಕ್ಕಿಂತ ಮೊದಲು ನೋಂದಾಯಿಸಲಾಗುತ್ತದೆ, ಆದ್ದರಿಂದ ಆದ್ಯತೆ ಪಡೆದುಕೊಳ್ಳುತ್ತದೆ.

---

### ಅಭ್ಯಾಸ 2: ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಆವೃತ್ತಿಗೆ ವೆಬ್ ಸರ್ವರ್ ಸೇರಿಸಿ

ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಆವೃತ್ತಿ ಪ್ರಸ್ತುತ CLI ಅಪ್ಲಿಕೇಶನ್ (`main.mjs`). ಹೊಸ ಫೈಲ್ `server.mjs` ಕೂಡಾ ಏಜೆಂಟ್ ಮಾಡ್ಯೂಲ್‌ಗಳನ್ನು HTTP ಸರ್ವರ್‌ಹಿಂದೆ ವೆರಪ್ ಮಾಡುತ್ತದೆ ಮತ್ತು ಹಂಚಿಕೊಂಡ UI ನ್ನು ಸರ್ವ್ ಮಾಡುತ್ತದೆ.

**2.1** ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಡೈರೆಕ್ಟರಿಯಲ್ಲಿ ಹೋಗಿ ಅವಶ್ಯಕತೆಗಳನ್ನು ಸ್ಥಾಪಿಸಿ:

```bash
cd zava-creative-writer-local/src/javascript
npm install
```

```powershell
cd zava-creative-writer-local\src\javascript
npm install
```

**2.2** ವೆಬ್ ಸರ್ವರ್ ಪ್ರಾರಂಭಿಸಿ:

```bash
node server.mjs
```

```powershell
node server.mjs
```

ನೀವು ಇದನ್ನು ಕಾಣಬಹುದು:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:3000
```

**2.3** ಬ್ರೌಸರ್‌ನಲ್ಲಿ `http://localhost:3000` ತೆರೆಯಿರಿ ಮತ್ತು **Generate Article** ಕ್ಲಿಕ್ ಮಾಡಿ. ಆಧಾರಭೂತವಾಗಿ ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಬ್ಯಾಕೆಂಡ್ ಜೊತೆಗೆ ಸಹ UI ಕಾರ್ಯನಿರ್ವಹಿಸುತ್ತದೆ.

**ಕೋಡ್ ಅಧ್ಯಯನ:** `server.mjs` ತೆರೆಯಿರಿ ಮತ್ತು ಪ್ರಮುಖ ಪ್ರಕಾರಗಳನ್ನು ಗಮನಿಸಿ:

- **ಸ್ಥಿರ ಫೈಲ್ ಸಂಕಲನ** Node.js ನಲ್ಲಿನ ಅಂತರ್ಸಿದ್ಧ `http`, `fs`, ಮತ್ತು `path` ಮಾಡ್ಯೂಲ್‌ಗಳ ಬಳಕೆ ಇಲ್ಲದೆ ಬಾಹ್ಯ ಫ್ರೇಮ್ವರ್ಕ್ ಅಗತ್ಯವಿಲ್ಲದೆ.
- **ಮಾರ್ಗ ಸಂಚಲನ ರಕ್ಷಣಾ**  ವಿನಂತಿಸಿದ ಮಾರ್ಗವನ್ನು ಸಾಮಾನ್ಯೀಕರಿಸಿ ಮತ್ತು ಇದು `ui/` ಡೈರೆಕ್ಟರಿಯೊಳಗೆ ಉಳಿಯುವುದನ್ನು ಪರಿಶೀಲಿಸುತ್ತದೆ.
- **NDJSON ಸ್ಟ್ರೀಮಿಂಗ್** ನಲ್ಲಿ `sendLine()` ಸಹಾಯಕರನ್ನ ಬಳಸಿ ಪ್ರತಿ वस्तುವನ್ನು ಸರಣಿಬದ್ಧಗೊಳಿಸಿ, ಅಂತರ್ನಿರ್ಮಿತ ಹೊಸ ಸಾಲುಗಳನ್ನು ತೆಗೆಯುತ್ತದೆ, ಎಲ್ಲಾ ರೇಖಿಕೆಯ newline ಅನ್ನು ಸೇರಿಸುವ ಮೂಲಕ.
- **ಏಜೆಂಟ್ ಕ್ರಮಬದ್ಧತೆ** ಅಸ್ತಿತ್ವದಲ್ಲಿರುವ `researcher.mjs`, `product.mjs`, `writer.mjs`, ಮತ್ತು `editor.mjs` ಮಾಡ್ಯೂಲ್‌ಗಳನ್ನು ಬದಲಾವಣೆಗಳಿಲ್ಲದೆ ಬಳಸುತ್ತದೆ.

<details>
<summary>server.mjs ನ ಕೀ ಭಾಗ</summary>

```javascript
function sendLine(res, obj) {
  res.write(JSON.stringify(obj).replace(/\n/g, "") + "\n");
}

// ಸಂಶೋಧಕ
sendLine(res, { type: "message", message: "Starting researcher agent task...", data: {} });
let researchResult = await research(researchContext, feedback);
sendLine(res, { type: "researcher", message: "Completed researcher task", data: researchResult });

// ಬರಹಗಾರ (ಸ್ಟ್ರೀಮಿಂಗ್)
for await (const token of write(...)) {
  sendLine(res, { type: "partial", message: "token", data: { text: token } });
}
```

</details>

---

### ಅಭ್ಯಾಸ 3: C# ಆವೃತ್ತಿಗೆ ಮಿನಿಮಲ್ API ಸೇರಿಸಿ

C# ಆವೃತ್ತಿ ಪ್ರಸ್ತುತ ಕಾನ್ಸೋల్ ಅಪ್ಲಿಕೇಶನ್ ಆಗಿದೆ. ಹೊಸ ಪ್ರೊಜೆಕ್ಟ್ `csharp-web` ASP.NET ಕೊರ್ ಮಿನಿಮಲ್ API ಗಳನ್ನು ಬಳಸಿ ಅದೇ ಪೈಪ್ಲೈನನ್ನು ವೆಬ್ ಸೇವೆ ರೂಪದಲ್ಲಿ ಹೊರತುಪಡಿಸುತ್ತದೆ.

**3.1** C# ವೆಬ್ ಪ್ರೊಜೆಕ್ಟ್‌ಗೆ ಹೋಗಿ ಪ್ಯಾಕೇಜ್‌ಗಳನ್ನು ರೀಸ್ಟೋರ್ ಮಾಡಿ:

```bash
cd zava-creative-writer-local/src/csharp-web
dotnet restore
```

```powershell
cd zava-creative-writer-local\src\csharp-web
dotnet restore
```

**3.2** ವೆಬ್ ಸರ್ವರ್ ಕಾರ್ಯಗತಗೊಳಿಸಿ:

```bash
dotnet run
```

```powershell
dotnet run
```

ನೀವು ಇದನ್ನು ನೋಡಬಹುದು:

```
Starting Foundry Local service...
Model already downloaded: phi-3.5-mini
Loading model: phi-3.5-mini...
Model ready: ...

Zava Creative Writer UI is running at http://localhost:5000
```

**3.3** ಬ್ರೌಸರ್‌ನಲ್ಲಿ `http://localhost:5000` ತೆರೆಯಿರಿ ಮತ್ತು **Generate Article** ಕ್ಲಿಕ್ ಮಾಡಿ.

**ಕೋಡ್ ಅಧ್ಯಯನ:** `csharp-web` ಡೈರೆಕ್ಟರಿಯ `Program.cs` ತೆರೆಯಿರಿ ಮತ್ತು ಗಮನಿಸಿ:

- ಪ್ರೊಜೆಕ್ಟ್ ಫೈಲ್ `Microsoft.NET.Sdk` ಬದಲು `Microsoft.NET.Sdk.Web` ಉಪಯೋಗಿಸುತ್ತದೆ, ಇದು ASP.NET ಕೊರ್ ಬೆಂಬಲ ಸೇರಿಸುತ್ತದೆ.
- ಸ್ಥಿರ ಫೈಲ್‌ಗಳನ್ನು `UseDefaultFiles` ಮತ್ತು `UseStaticFiles` ಮೂಲಕ ಹಂಚಿಕೊಳ್ಳುತ್ತದೆ ಮತ್ತು ಹಂಚಿಕೊಂಡ `ui/` ಡೈರೆಕ್ಟರಿಯನ್ನು ಸೂಚಿಸುತ್ತದೆ.
- `/api/article` ಎಂಡ್ಪಾಯಿಂಟ್ NDJSON ಸಾಲುಗಳನ್ನು ನೇರವಾಗಿ `HttpContext.Response` ಗೆ ಬರೆಯುತ್ತದೆ ಮತ್ತು ಪ್ರತಿ ಸಾಲಿನ ನಂತರ ಫ್ಲಷ್ ಮಾಡುತ್ತದೆ, ರಿಯಲ್ ಟೈಮ್ ಸ್ಟ್ರೀಮಿಂಗ್.
- ಎಲ್ಲ ಏಜೆಂಟ್ ಲಾಜಿಕ್ (`RunResearcher`, `RunProductSearch`, `RunEditor`, `BuildWriterMessages`) ಕಾನ್ಸೋಲ್ ಆವೃತ್ತಿಯಂತೆಯೇ ಇರುತ್ತದೆ.

<details>
<summary>csharp-web/Program.cs ನ ಕೀ ಭಾಗ</summary>

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

### ಅಭ್ಯಾಸ 4: ಏಜೆಂಟ್ ಸ್ಥಿತಿ ಬ್ಯಾಡ್ಜ್‌ಗಳನ್ನು ಅನ್ವೇಷಿಸಿ

ಈಗ ಕಾರ್ಯನಿರ್ವಹಿಸುವ UI ಇದೆ, ಫ್ರಂಟ್ ಎಂಡ್ ಸಿದ್ಧಪಡಿಸುವ ರೀತಿಯನ್ನು ನೋಡಿ.

**4.1** `zava-creative-writer-local/ui/app.js` ನಿಮ್ಮ ಸಂಪಾದಕದಲ್ಲಿ ತೆರೆಯಿರಿ.

**4.2** `handleMessage()` ಫಂಕ್ಷನ್ ಹುಡುಕಿ. ಸಂದೇಶ ಪ್ರಕಾರಗಳನ್ನು DOM ನವೀಕರಣಗಳಿಗೆ ಮ್ಯಾಪ್ ಮಾಡುವ ರೀತ್ಯ ಗಮನಿಸಿ:

| ಸಂದೇಶ ಪ್ರಕಾರ | UI ಕ್ರಿಯೆ |
|-------------|-----------|
| "researcher" ಪದವನ್ನು ಒಳಗೊಂಡ `message` | ರಿಸರ್ಚರ್ ಬ್ಯಾಡ್ಜ್ ಅನ್ನು "Running"ಗೆ ಹೊಂದಿಸುತ್ತದೆ |
| `researcher` | ರಿಸರ್ಚರ್ ಬ್ಯಾಡ್ಜ್ "Done" ಆಗುತ್ತದೆ ಮತ್ತು ರಿಸರ್ಚ್ ಫಲಿತಾಂಶ ಫलक ನ ಮಾಹಿತಿ ತುಂಬುತ್ತದೆ |
| `marketing` | ಉತ್ಪನ್ನ ಹುಡುಕಾಟ ಬ್ಯಾಡ್ಜ್ "Done" ಆಗುತ್ತದೆ ಮತ್ತು ಪ್ರೊಡಕ್ಟ್ ಮ್ಯಾಚ್ ಫलकನ್ನು ತುಂಬುತ್ತದೆ |
| `writer` ನಲ್ಲಿ `data.start` | ಬರಹಗಾರ ಬ್ಯಾಡ್ಜ್ "Running" ಆಗುತ್ತದೆ ಮತ್ತು ಲೇಖನ ಔಟ್ಪುಟ್ ತೆರವುಗೊಳ್ಳುತ್ತದೆ |
| `partial` | ಲೇಖನ ಔಟ್ಪುಟ್ ಗೆ ಟೋಕನ್ ಪಠ್ಯವನ್ನು ಸೇರಿದಂತೆ ಮಾಡಿ |
| `writer` ನಲ್ಲಿ `data.complete` | ಬರಹಗಾರ ಬ್ಯಾಡ್ಜ್ "Done" ಆಗುತ್ತದೆ |
| `editor` | ಸಂಪಾದಕ ಬ್ಯಾಡ್ಜ್ "Done" ಆಗುತ್ತದೆ ಮತ್ತು ಸಂಪಾದಕ ಪ್ರತಿಕ್ರಿಯೆ ಪ್ಯಾನ್ ತುಂಬುತ್ತದೆ |

**4.3** ಲೇಖನ ಕೆಳಗೆ ಉಳ್ಳ ಮುರಿಯಬಹುದಾದ "Research Results", "Product Matches", ಮತ್ತು "Editor Feedback" ಫಲಕಗಳನ್ನು ತೆರೆಯಲು ತೆರೆಯಿರಿ, ಪ್ರತಿ ಏಜೆಂಟ್ ತಯಾರಿಸಿದ ಕಚ್ಚಾ JSON ಪರಿಶೀಲಿಸಲು.

---

### ಅಭ್ಯಾಸ 5: UI ಕಸ್ಟಮೈಸ್ ಮಾಡಿ (ವಿಸ್ತರಣೆ)

ಕೆಳಗಿನ ಸುಧಾರಣೆಗಳಲ್ಲಿ ಒಂದರ ಮೇಲು ಅಥವಾ ಹೆಚ್ಚು ಪ್ರಯತ್ನಿಸಿ:

**5.1 ಪದಗಳ ಎಣಿಕೆ ಸೇರಿಸಿ.** ಬರಹಗಾರ ಮುಗಿಸಿದ ನಂತರ, ಔಟ್‌ಪುಟ್ ಫಲಕ ಕೆಳಗೆ ಲೇಖನ ಪದಎಣಿಕೆಯನ್ನು ಪ್ರದರ್ಶಿಸಿ. ಇದು `handleMessage` ನಲ್ಲಿ `type === "writer"` ಮತ್ತು `data.complete` ನಿಜವಾದಾಗ ಲೆಕ್ಕಿಸಬಹುದು:

```javascript
case "writer":
  if (data && data.complete) {
    setAgentState(statusWriter, "done");
    const words = articleOutput.textContent.trim().split(/\s+/).length;
    articleOutput.textContent += "\n\n[Word count: " + words + "]";
  }
  break;
```

**5.2 ಪುನಃಪ್ರಯತ್ನ ಸೂಚಕ ಸೇರಿಸಿ.** ಸಂಪಾದಕರು ತಿದ್ದುಪಡಿಯನ್ನು ಕೇಳಿದಾಗ ಪೈಪ್ಲೈನ್ ಮರುಚಲಾಯಿಸುತ್ತವೆ. ಸ್ಥಿತಿ ಫಲಕದಲ್ಲಿ "Revision 1" ಅಥವಾ "Revision 2" ಬ್ಯಾನರ್ ತೋರಿಸಿ. `message` ಪ್ರಕಾರದಲ್ಲಿ "Revision" ಇರುವ ಸಂದೇಶವನ್ನು ಕೇಳಿ ಹೊಸ DOM ಅಂಶ ನವೀಕರಿಸಿ.

**5.3 ಡಾರ್ಕ್ ಮೋಡ್.** ಟೊಗಲ್ ಬಟನ್ ಮತ್ತು `<body>` ಗೆ `.dark` ಕ್ಲಾಸ್ ಸೇರಿಸಿ. `style.css` ನಲ್ಲಿ `body.dark` ಸೆಳೆಕ್ಟರ್ ಮೂಲಕ ಹಿನ್ನೆಲೆ, ಪಠ್ಯ ಮತ್ತು ಫಲಕ ಬಣ್ಣಗಳನ್ನು ಮೀರಳಿಗೆ ತರುವಂತೆ ಮಾಡಿ.

---

## ಸಾರಾಂಶ

| ನೀವು ಏನು ಮಾಡಿದಿರಿ | ಹೇಗೆ |
|-------------|-----|
| Python ಬ್ಯಾಕೆಂಡ್‌ನಿಂದ UI ಸರ್ವ್ ಮಾಡಲಾಗಿದೆ | FastAPI ನಲ್ಲಿ `StaticFiles` ಬಳಸಿ `ui/` ಫೋಲ್ಡರ್ ಮೋಂಟು ಮಾಡಲಾಗಿದೆ |
| ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಆವೃತ್ತಿಗೆ HTTP ಸರ್ವರ್ ಸೇರಿಸಲಾಗಿದೆ | Node.js ಅಂತರ್ಸಿದ್ಧ `http` ಮಾಡ್ಯೂಲ್ ಬಳಸಿ `server.mjs` ನಿರ್ಮಿಸಲಾಗಿದೆ |
| C# ಆವೃತ್ತಿಗೆ ವೆಬ್ API ಸೇರಿಸಲಾಗಿದೆ | ASP.NET Core ಮಿನಿಮಲ್ API ಗಳೊಂದಿಗೆ `csharp-web` ಪ್ರೊಜೆಕ್ಟ್ ರಚಿಸಲಾಗಿದೆ |
| ಬ್ರೌಸರ್‌ನಲ್ಲಿ ಸ್ಟ್ರೀಮಿಂಗ್ NDJSON ಸೇವಿಸಲಾಗಿದೆ | `fetch()` ಜೊತೆ `ReadableStream` ಮತ್ತು ಸಾಲುಬಂದ JSON ವಿಶ್ಲೇಷಣೆ ಬಳಸಿ |
| UI ನ್ನು ರಿಯಲ್ ಟೈಮ್‌ನಲ್ಲಿ ನವೀಕರಿಸಲಾಗಿದೆ | ಸಂದೇಶ ಪ್ರಕಾರಗಳನ್ನು DOM ನವೀಕರಣಕ್ಕೆ ಮ್ಯಾಪ್ ಮಾಡಲಾಗಿದೆ (ಬ್ಯಾಡ್ಜ್‌ಗಳು, ಪಠ್ಯ, ಮುರಿಯಬಹುದಾದ ಫಲಕಗಳು) |

---

## ಮುಖ್ಯ ಪಾಠಗಳು

1. **ಹಂಚಿಕೊಳ್ಳಲಾದ ಸ್ಥಿರ ಫ್ರಂಟ್ ಎಂಡ್** ಯಾವುದೇ ಬ್ಯಾಕೆಂಡ್ ಜೊತೆಗೆ ಕೆಲಸ ಮಾಡಬಹುದು ಅದು ಅದೇ ಸ್ಟ್ರೀಮಿಂಗ್ ಪ್ರೋಟೋಕಾಲ್ ನ್ನು ಬೆಂಬಲಿಸುವಂತೆ, OpenAI-ಸಮ್ಮತ API ಮಾದರಿಯ ಮೂಲ್ಯವನ್ನು ಘನಪಡಿಸುತ್ತದೆ.
2. **ಹೊಸ ಲೈನ್-ವಿಭಜಿತ JSON (NDJSON)** ಒಂದು ಸರಳ ಸ್ಟ್ರೀಮಿಂಗ್ ಫಾರ್ಮಾಟ್ ಆಗಿದ್ದು, ಬ್ರೌಸರ್ `ReadableStream` API ನೊಂದಿಗೆ ನೈಜವಾಗಿ ಕೆಲಸ ಮಾಡುತ್ತದೆ.
3. **Python ಆವೃತ್ತಿಗೆ ಕನಿಷ್ಟ ಬದಲಾವಣೆ ಬೇಕು** ಏಕೆಂದರೆ ಅದಕ್ಕೆ ಈಗಾಗಲೇ FastAPI ಎಂಡ್ಪಾಯಿಂಟ್ ಇತ್ತು; ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮತ್ತು C# ಆವೃತ್ತಿಗೆ ಸಣ್ಣ HTTP ವೆರಪರ್ ಬೇಕಾಯಿತು.
4. UI ನ್ನು **ವ್ಯಾನಿಲ್ಲಾ HTML/CSS/JS** ಆಗಿರಿಸುವುದರಿಂದ ಬಿಲ್ಡ್ ಟೂಲ್‌ಗಳು, ಫ್ರೇಮ್ವರ್ಕ್ ಅವಲಂಬನೆಗಳು ಮತ್ತು ಕಾರ್ಯಾಗಾರ ಕಲಿಕೆಗೆ ಹೆಚ್ಚುವರಿ ಗೊಂದಲ ಇಲ್ಲ.
5. ಅವುಗಳೇ ಏಜೆಂಟ್ ಮಾಡ್ಯೂಲ್‌ಗಳು (Researcher, Product, Writer, Editor) ಯಾವುದೇ ಬದಲಾವಣೆಯಿಲ್ಲದೆ ಮತ್ತೆ ಬಳಸಲಾಗುತ್ತದೆ; ಕೇವಲ ಸಾಗಣೆಲೋಕ ಮಾತ್ರ ಬದಲಾಗುತ್ತದೆ.

---

## ಮುಂದಿನ ಓದು

| ಸಂಪನ್ಮೂಲ | ಲಿಂಕ್ |
|----------|------|
| MDN: Readable Streams ಬಳಕೆ | [developer.mozilla.org/en-US/docs/Web/API/ReadableStream](https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream) |
| FastAPI ಸ್ಥಿರ ಫೈಲ್ಸ್ | [fastapi.tiangolo.com/tutorial/static-files](https://fastapi.tiangolo.com/tutorial/static-files/) |
| ASP.NET Core ಸ್ಥಿರ ಫೈಲ್ಸ್ | [learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files) |
| NDJSONವಿವರಣೆ | [ndjson.org](https://ndjson.org) |
| Foundry Local | [foundrylocal.ai](https://foundrylocal.ai) |

---

[ಭಾಗ 13: ಕಾರ್ಯಾಗಾರ ಪೂರ್ಣ](part13-workshop-complete.md) ಗೆ ಮುಂದುವರೆಯಿರಿ, ಇಲ್ಲಿ ನೀವು ಈ ಕಾರ್ಯಾಗಾರದಲ್ಲಿ ನಿರ್ಮಿಸಿದ ಎಲ್ಲವನ್ನೂ ಸಂಕ್ಷೇಪವಾಗಿ ಕಾಣಬಹುದು.

---
[← ಭಾಗ 11: ಉಪಕರಣ ಕರೆ ಮಾಡುವಿಕೆ](part11-tool-calling.md) | [ಭಾಗ 13: ಕಾರ್ಯಾಗಾರ ಪೂರ್ಣ →](part13-workshop-complete.md)