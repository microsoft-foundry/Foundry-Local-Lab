<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local ਵਰਕਸ਼ਾਪ - ਡਿਵਾਈਸ 'ਤੇ ਏਆਈ ਐਪ ਬਣਾਓ

ਆਪਣੇ ਮਸ਼ੀਨ 'ਤੇ ਭਾਸ਼ਾ ਮਾਡਲ ਚਲਾਉਣ ਅਤੇ [Foundry Local](https://foundrylocal.ai) ਅਤੇ [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) ਨਾਲ ਬੁੱਧੀਮਾਨ ਐਪਲੀਕੇਸ਼ਨ ਬਣਾਉਣ ਲਈ ਇੱਕ ਹੱਥ-ਇਨ-ਹੱਥ ਵਰਕਸ਼ਾਪ।

> **Foundry Local ਕੀ ਹੈ?** Foundry Local ਇੱਕ ਹਲਕਾ ਰਨਟਾਈਮ ਹੈ ਜੋ ਤੁਹਾਡੇ ਹਾਰਡਵੇਅਰ 'ਤੇ ਭਾਸ਼ਾ ਮਾਡਲਸ ਨੂੰ ਡਾਊਨਲੋਡ, ਪ੍ਰਬੰਧਤ ਅਤੇ ਸਰਵ ਕਰਨ ਦੀ ਸਹੂਲਤ ਦਿੰਦਾ ਹੈ। ਇਹ ਇੱਕ **OpenAI-ਅਨੁਕੂਲ API** ਨੂੰ ਖੋਲ੍ਹਦਾ ਹੈ ਤਾਂ ਜੋ ਕੋਈ ਵੀ ਟੂਲ ਜਾਂ SDK ਜੋ OpenAI ਨਾਲ ਗੱਲ ਕਰਦਾ ਹੈ, ਜੁੜ ਸਕਦਾ ਹੈ - ਕੋਈ ਕਲਾਉਡ ਖਾਤਾ ਲੋੜ ਨਹੀਂ। 

---

## ਸਿੱਖਣ ਦੇ ਉਦੇਸ਼

ਇਸ ਵਰਕਸ਼ਾਪ ਦੇ ਅੰਤ ਵਿੱਚ ਤੁਸੀਂ ਸਕੋਗੇ:

| # | ਉਦੇਸ਼ |
|---|-----------|
| 1 | Foundry Local ਇੰਸਟਾਲ ਕਰੋ ਅਤੇ CLI ਨਾਲ ਮਾਡਲਾਂ ਦਾ ਪ੍ਰਬੰਧਨ ਕਰੋ |
| 2 | Foundry Local SDK API ਨੂੰ ਕਾਸ਼ਤਰੀ ਮਾਡਲ ਪ੍ਰਬੰਧਨ ਲਈ ਮਾਸਟਰ ਕਰੋ |
| 3 | Python, JavaScript ਅਤੇ C# SDKs ਦੇ ਜ਼ਰੀਏ ਲੌਕਲ ਇਨਫਰੰਸ ਸਰਵਰ ਨਾਲ ਜੁੜੋ |
| 4 | ਇੱਕ RAG (Retrieval-Augmented Generation) ਪਾਈਪਲਾਈਨ ਬਣਾਓ ਜੋ ਤੁਹਾਡੇ ਆਪਣੇ ਡਾਟਾ ਵਿੱਚ ਜਵਾਬਾਂ ਨੂੰ ਅਧਾਰਿਤ ਕਰਦੀ ਹੈ |
| 5 | ਸਥਾਈ ਨਿਰਦੇਸ਼ ਅਤੇ ਪੈਰਸੋਨਾ ਵਾਲੇ AI ਏਜੰਟ ਬਣਾਓ |
| 6 | ਫੀਡਬੈਕ ਲੂਪਾਂ ਨਾਲ ਬਹੁ-ਏਜੰਟ ਵਰਕਫਲੋਜ਼ ਸੰਚਾਲਿਤ ਕਰੋ |
| 7 | ਇੱਕ ਪ੍ਰੋਡਕਸ਼ਨ ਕੈਪਸਟੋਨ ਐਪ ਵੇਖੋ – Zava Creative Writer |
| 8 | ਸੋਨੇ ਦੇ ਡਾਟਾਸੈੱਟ ਅਤੇ LLM-ਐਜ਼-ਜੱਜ ਸਕੋਰਿੰਗ ਨਾਲ ਮੁਲਾਂਕਣ ਫਰੇਮਵਰਕ ਬਣਾਓ |
| 9 | Whisper ਨਾਲ ਆਡੀਓ ਟ੍ਰਾਂਸਕ੍ਰਿਪਟ ਕਰੋ - Foundry Local SDK ਨਾਲ ਡਿਵਾਈਸ 'ਤੇ ਸਪੀਚ-ਟੂ-ਟੈਕਸਟ |
| 10 | ONNX Runtime GenAI ਅਤੇ Foundry Local ਨਾਲ ਕਸਟਮ ਜਾਂ Hugging Face ਮਾਡਲਾਂ ਨੂੰ ਕੰਪਾਈਲ ਅਤੇ ਚਲਾਓ |
| 11 | ਟੂਲ-ਕਾਲਿੰਗ ਪੈਟਰਨ ਨਾਲ ਬਾਹਰੀ ਫੰਕਸ਼ਨਾਂ ਨੂੰ ਕਾਲ ਕਰਨ ਲਈ ਲੌਕਲ ਮਾਡਲਸ ਨੂੰ ਯੋਗ ਬਣਾਓ |
| 12 | Zava Creative Writer ਲਈ ਬ੍ਰਾਊਜ਼ਰ-ਅਧਾਰਿਤ UI ਬਣਾਓ ਜਿਸ ਵਿੱਚ ਰੀਅਲ-ਟਾਈਮ ਸਟੀਮਿੰਗ ਹੈ |

---

## ਜ਼ਰੂਰੀ ਸ਼ਰਤਾਂ

| ਜ਼ਰੂਰਤ | ਵੇਰਵਾ |
|-------------|---------|
| **ਹਾਰਡਵੇਅਰ** | ਘੱਟੋ-ਘੱਟ 8 GB RAM (16 GB ਸਿਫਾਰਸ਼ ਕੀਤੀ); AVX2-ਯੋਗ CPU ਜਾਂ ਸਹਾਇਤਾਕਾਰੀ GPU |
| **ਓਐਸ** | Windows 10/11 (x64/ARM), Windows Server 2025, ਜਾਂ macOS 13+ |
| **Foundry Local CLI** | `winget install Microsoft.FoundryLocal` (Windows) ਜਾਂ `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS) ਰਾਹੀਂ ਇੰਸਟਾਲ ਕਰੋ। ਵਿਸਥਾਰ ਲਈ [ਸ਼ੁਰੂਆਤ ਕਰਨ ਦੀ ਗਾਈਡ](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) ਵੇਖੋ। |
| **ਭਾਸ਼ਾ ਰਨਟਾਈਮ** | **Python 3.9+** ਅਤੇ/ਜਾਂ **.NET 9.0+** ਅਤੇ/ਜਾਂ **Node.js 18+** |
| **Git** | ਇਸ ਰਿਪੋਜ਼ਟਰੀ ਨੂੰ ਕਲੋਨ ਕਰਨ ਲਈ |

---

## ਸ਼ੁਰੂ ਕਰਨਾ

```bash
# 1. ਰਿਪੋਜ਼ਟਰੀ ਕਲੋਨ ਕਰੋ
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. ਯਕੀਨੀ ਬਣਾਓ ਕਿ ਫਾੜਨਰੀ ਲੋਕਲ ਇੰਸਟਾਲ ਹੈ
foundry model list              # ਉਪਲਬਧ ਮਾਡਲ ਸੂਚੀਬੱਧ ਕਰੋ
foundry model run phi-3.5-mini  # ਇਕ ਇੰਟਰਐਕਟਿਵ ਚੈਟ ਸ਼ੁਰੂ ਕਰੋ

# 3. ਆਪਣੀ ਭਾਸ਼ਾ ਟਰੈਕ ਚੁਣੋ (ਪੂਰੇ ਸੈਟਅਪ ਲਈ ਹਿੱਸਾ 2 ਦੀ ਪ੍ਰਯੋਗਸ਼ਾਲਾ ਦੇਖੋ)
```

| ਭਾਸ਼ਾ | ਸ਼ੁਰੂਆਤ |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ਵਰਕਸ਼ਾਪ ਦੇ ਹਿੱਸੇ

### ਭਾਗ 1: Foundry Local ਨਾਲ ਸ਼ੁਰੂਆਤ

**ਲੈਬ ਗਾਈਡ:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local ਕੀ ਹੈ ਅਤੇ ਇਹ ਕਿਵੇਂ ਕੰਮ ਕਰਦਾ ਹੈ
- Windows ਅਤੇ macOS 'ਤੇ CLI ਇੰਸਟਾਲ ਕਰਨਾ
- ਮਾਡਲ ਨੂੰ ਖੋਜਣਾ - ਸੂਚੀ ਬਣਾਉਣਾ, ਡਾਊਨਲੋਡ ਕਰਨਾ, ਚਲਾਉਣਾ
- ਮਾਡਲ ਘੁਸਪੈਠ ਅਤੇ ਡਾਇਨਾਮਿਕ ਪੋਰਟਾਂ ਨੂੰ ਸਮਝਣਾ

---

### ਭਾਗ 2: Foundry Local SDK ਦੀ ਡੂੰਘੀ ਸਮਝ

**ਲੈਬ ਗਾਈਡ:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- ਕਾਰਜ ਵਿਕਾਸ ਲਈ CLI ਦੀ ਥਾਂ SDK ਕਿਉਂ ਵਰਤੋਂ
- Python, JavaScript ਅਤੇ C# ਲਈ ਪੂਰਾ SDK API ਸੰਦਰਭ
- ਸੇਵਾ ਪ੍ਰਬੰਧਨ, ਕੈਟਲੌਗ ਬਰਾਊਜ਼ਿੰਗ, ਮਾਡਲ ਲਾਈਫਸਾਈਕਲ (ਡਾਊਨਲੋਡ, ਲੋਡ, ਅਨਲੋਡ)
- ਤੁਰੰਤ ਸ਼ੁਰੂਆਤ ਪੈਟਰਨ: Python ਕੰਸਟ੍ਰਕਟਰ ਬੂਟਸਟ੍ਰੈਪ, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` ਮੈਟਾਡੇਟਾ, ਘੁਸਪੈਠਾਂ ਅਤੇ ਹਾਰਡਵੇਅਰ-ਅਨੁਕੂਲ ਮਾਡਲ ਚੋਣ

---

### ਭਾਗ 3: SDKs ਅਤੇ APIs

**ਲੈਬ ਗਾਈਡ:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript ਅਤੇ C# ਤੋਂ Foundry Local ਨਾਲ ਕਨੈਕਟ ਕਰਨਾ
- Foundry Local SDK ਦੀ ਵਰਤੋਂ ਨਾਲ ਸੇਵਾ ਦਾ ਪ੍ਰੋਗਰਾਮੈਟਿਕ ਪ੍ਰਬੰਧਨ
- OpenAI-ਅਨੁਕੂਲ API ਰਾਹੀਂ ਸਟੀਮਿੰਗ ਚੈਟ ਕੰਪਲੀਸ਼ਨ
- ਹਰ ਭਾਸ਼ਾ ਲਈ SDK ਵਿਧੀ ਸੰਦਰਭ

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ | ਵੇਰਵਾ |
|----------|------|-------------|
| Python | `python/foundry-local.py` | ਬੇਸਿਕ ਸਟੀਮਿੰਗ ਚੈਟ |
| C# | `csharp/BasicChat.cs` | .NET ਨਾਲ ਸਟੀਮਿੰਗ ਚੈਟ |
| JavaScript | `javascript/foundry-local.mjs` | Node.js ਨਾਲ ਸਟੀਮਿੰਗ ਚੈਟ |

---

### ਭਾਗ 4: Retrieval-Augmented Generation (RAG)

**ਲੈਬ ਗਾਈਡ:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG ਕੀ ਹੈ ਅਤੇ ਇਸਦੀ ਮਹੱਤਤਾ ਕੀ ਹੈ
- ਇਕ ਇਨ-ਮੇਮੋਰੀ ਨਿਜਾਣਕਾਰੀ ਬੇਸ ਬਣਾਉਣਾ
- ਸਕੋਰਿੰਗ ਨਾਲ ਕੀਵਰਡ-ਓਵਰਲੈਪ ਰੀਟਰਿਵਲ
- ਗ੍ਰਾਊਂਡਡ ਸਿਸਟਮ ਪ੍ਰਾਂਪਟਸ ਬਣਾਉਣਾ
- ਡਿਵਾਈਸ 'ਤੇ ਪੂਰੀ RAG ਪਾਈਪਲਾਈਨ ਚਲਾਉਣਾ

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### ਭਾਗ 5: AI ਏਜੰਟ ਬਣਾਉਣਾ

**ਲੈਬ ਗਾਈਡ:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI ਏਜੰਟ ਕੀ ਹੈ (ਇੱਕ ਸਿੱਧੇ LLM ਕਾਲ ਦੇ ਵਿਰੁੱਧ)
- `ChatAgent` ਪੈਟਰਨ ਅਤੇ Microsoft Agent Framework
- ਸਿਸਟਮ ਨਿਰਦੇਸ਼, ਪੈਰਸੋਨਾ, ਅਤੇ ਬਹੁ-ਮੁੜਕਲੇ ਗੱਲਬਾਤਾਂ
- ਏਜੰਟ ਤੋਂ ਸੰਰਚਿਤ ਆਉਟਪੁੱਟ (JSON)

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ | ਵੇਰਵਾ |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework ਨਾਲ ਸਿੰਗਲ ਏਜੰਟ |
| C# | `csharp/SingleAgent.cs` | ਸਿੰਗਲ ਏਜੰਟ (ChatAgent ਪੈਟਰਨ) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | ਸਿੰਗਲ ਏਜੰਟ (ChatAgent ਪੈਟਰਨ) |

---

### ਭਾਗ 6: ਬਹੁ-ਏਜੰਟ ਵਰਕਫਲੋਜ਼

**ਲੈਬ ਗਾਈਡ:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- ਬਹੁ-ਏਜੰਟ ਪਾਈਪਲਾਈਨ: ਰਿਸਰਚਰ → ਲੇਖਕ → ਸੰਪਾਦਕ
- ਲੜੀਵਾਰ ਓਰਕੇਸਟ੍ਰੇਸ਼ਨ ਅਤੇ ਫੀਡਬੈਕ ਲੂਪ
- ਸਾਂਝੀ ਸੈਟਿੰਗ ਅਤੇ ਸੰਰਚਿਤ ਹੱਥ-ਆਰਪਣ
- ਆਪਣਾ ਬਹੁ-ਏਜੰਟ ਵਰਕਫਲੋ ਡਿਜ਼ਾਈਨ ਕਰਨਾ

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ | ਵੇਰਵਾ |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | ਤਿੰਨ-ਏਜੰਟ ਪਾਈਪਲਾਈਨ |
| C# | `csharp/MultiAgent.cs` | ਤਿੰਨ-ਏਜੰਟ ਪਾਈਪਲਾਈਨ |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | ਤਿੰਨ-ਏਜੰਟ ਪਾਈਪਲਾਈਨ |

---

### ਭਾਗ 7: Zava Creative Writer - ਕੈਪਸਟੋਨ ਐਪਲੀਕੇਸ਼ਨ

**ਲੈਬ ਗਾਈਡ:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- ਚਾਰ ਖਾਸ ਏਜੰਟਾਂ ਵਾਲਾ ਪ੍ਰੋਡਕਸ਼ਨ-ਸਟਾਈਲ ਬਹੁ-ਏਜੰਟ ਐਪ
- ਮੁਲਾਂਕਣਕਾਰ-ਸੰਚालित ਫੀਡਬੈਕ ਲੂਪਾਂ ਵਾਲੀ ਲੜੀਵਾਰ ਪਾਈਪਲਾਈਨ
- ਸਟੀਮਿੰਗ ਆਉਟਪੁੱਟ, ਉਤਪਾਦ ਕੈਟਲੌਗ ਖੋਜ, ਸੰਰਚਿਤ JSON ਹੱਥ-ਆਰਪਣ
- Python (FastAPI), JavaScript (Node.js CLI), ਅਤੇ C# (.NET ਕਨਸੋਲ) ਵਿੱਚ ਪੂਰੀ ਨਿਰਮਾਣ

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਡਾਇਰੈਕਟਰੀ | ਵੇਰਵਾ |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI ਵੈਬ ਸੇਵਾ ਜਿਸ ਵਿੱਚ ਓਰਕੇਸਟਰੇਟਰ ਹੈ |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI ਐਪਲੀਕੇਸ਼ਨ |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 ਕਨਸੋਲ ਐਪਲੀਕੇਸ਼ਨ |

---

### ਭਾਗ 8: ਮੁਲਾਂਕਣ-ਅਧਾਰਿਤ ਵਿਕਾਸ

**ਲੈਬ ਗਾਈਡ:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- ਸੋਨੇ ਦੇ ਡਾਟਾਸੈੱਟ ਨਾਲ AI ਏਜੰਟਾਂ ਲਈ ਵਿਧਾਨਕ ਮੁਲਾਂਕਣ ਫਰੇਮਵਰਕ ਬਣਾਉਣਾ
- ਨਿਯਮ-ਆਧਾਰਿਤ ਚੈੱਕ (ਲੰਬਾਈ, ਕੀਵਰਡ ਕਵਰੇਜ, ਮਨਾਹੀ ਸ਼ਬਦ) + LLM-ਐਜ਼-ਜੱਜ ਸਕੋਰਿੰਗ
- ਪ੍ਰਾਂਪਟ ਦੀ ਇਕ-ਦੂਜੇ ਨਾਲ ਤੁਲਨਾ ਨਾਲ ਸੰਗ੍ਰਹਿਤ ਸਕੋਰਕਾਰਡਸ
- ਭਾਗ 7 ਦੇ Zava Editor ਏਜੰਟ ਪੈਟਰਨ ਨੂੰ ਆਫਲਾਈਨ ਟੈਸਟ ਸਿਟ ਵਿੱਚ ਵਧਾਉਣਾ
- Python, JavaScript ਅਤੇ C# ਟ੍ਰੈਕਸ

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ | ਵੇਰਵਾ |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | ਮੁਲਾਂਕਣ ਫਰੇਮਵਰਕ |
| C# | `csharp/AgentEvaluation.cs` | ਮੁਲਾਂਕਣ ਫਰੇਮਵਰਕ |
| JavaScript | `javascript/foundry-local-eval.mjs` | ਮੁਲਾਂਕਣ ਫਰੇਮਵਰਕ |

---

### ਭਾਗ 9: Whisper ਨਾਲ ਆਵਾਜ਼ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ

**ਲੈਬ ਗਾਈਡ:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- ਖੁਦ ਡਿਵਾਈਸ 'ਤੇ OpenAI Whisper ਨਾਲ ਸਪੀਚ-ਟੂ-ਟੈਕਸਟ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ
- ਪਰਦੇਦਾਰੀ-ਪਹਿਲਾਂ ਆਵਾਜ਼ ਪ੍ਰੋਸੈਸਿੰਗ - ਆਡੀਓ ਕਦੇ ਵੀ ਤੁਹਾਡੇ ਡਿਵਾਈਸ ਤੋਂ ਬਾਹਰ ਨਹੀਂ ਜਾਂਦੀ
- Python, JavaScript, ਅਤੇ C# ਟ੍ਰੈਕਸ ਨਾਲ `client.audio.transcriptions.create()` (Python/JS) ਅਤੇ `AudioClient.TranscribeAudioAsync()` (C#)
- ਹੱਥ-ਇਨ-ਹੱਥ ਅਭਿਆਸ ਲਈ Zava-ਥੀਮ ਵਾਲੀਆਂ ਆਡੀਓ ਫਾਇਲਾਂ ਸ਼ਾਮਲ

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ | ਵੇਰਵਾ |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper ਆਵਾਜ਼ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ |
| C# | `csharp/WhisperTranscription.cs` | Whisper ਆਵਾਜ਼ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper ਆਵਾਜ਼ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ |

> **ਟਿੱਪਣੀ:** ਇਹ ਲੈਬ **Foundry Local SDK** ਦੀ ਵਰਤੋਂ ਕਰਦਾ ਹੈ Whisper ਮਾਡਲ ਨੂੰ ਪ੍ਰੋਗਰਾਮੈਟਿਕ ਤੌਰ 'ਤੇ ਡਾਊਨਲੋਡ ਅਤੇ ਲੋਡ ਕਰਨ ਲਈ, ਫਿਰ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਲਈ ਆਡੀਓ ਨੂੰ ਲੋਕਲ OpenAI-ਅਨੁਕੂਲ ਐਂਡਪੌਇੰਟ 'ਤੇ ਭੇਜਦਾ ਹੈ। Whisper ਮਾਡਲ (`whisper`) Foundry Local ਕੈਟਲੌਗ ਵਿੱਚ ਦਿੱਤਾ ਗਿਆ ਹੈ ਅਤੇ ਪੂਰੀ ਤਰ੍ਹਾਂ ਡਿਵਾਈਸ 'ਤੇ ਚਲਦਾ ਹੈ - ਕੋਈ ਕਲਾਉਡ API ਕੁੰਜੀਆਂ ਜਾਂ ਨੈੱਟਵਰਕ ਪਹੁੰਚ ਲੋੜੀਂਦੀ ਨਹੀਂ।

---

### ਭਾਗ 10: ਕਸਟਮ ਜਾਂ Hugging Face ਮਾਡਲ ਦੀ ਵਰਤੋਂ

**ਲੈਬ ਗਾਈਡ:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- ONNX ਰਨਟਾਈਮ GenAI ਮਾਡਲ ਬਿਲਡਰ ਨਾਲ Hugging Face ਮਾਡਲਾਂ ਨੂੰ ਓਪਟੀਮਾਈਜ਼ ਕੀਤਾ ONNX ਫਾਰਮੈਟ ਵਿੱਚ ਕੰਪਾਈਲ ਕਰਨਾ
- ਹਾਰਡਵੇਅਰ-ਖਾਸ ਕੰਪਾਈਲੇਸ਼ਨ (CPU, NVIDIA GPU, DirectML, WebGPU) ਅਤੇ ਕੁਵਾਂਟਾਈਜ਼ੇਸ਼ਨ (int4, fp16, bf16)
- Foundry Local ਲਈ ਚੈਟ-ਟੈਪਲੇਟ ਸੰਰਚਨਾ ਫਾਇਲਾਂ ਬਣਾਉਣਾ
- ਕੰਪਾਈਲ ਕੀਤੇ ਮਾਡਲਾਂ ਨੂੰ Foundry Local ਕੈਸ਼ ਵਿੱਚ ਜੋੜਨਾ
- CLI, REST API ਅਤੇ OpenAI SDK ਰਾਹੀਂ ਕਸਟਮ ਮਾਡਲਸ ਚਲਾਉਣਾ
- ਉਦਾਹਰਣ: Qwen/Qwen3-0.6B ਦਾ ਸਿਰੋਂ-ਸਿਰ ਤੱਕ ਕੰਪਾਈਲ ਕਰਨਾ

---

### ਭਾਗ 11: ਲੌਕਲ ਮਾਡਲਸ ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ

**ਲੈਬ ਗਾਈਡ:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- ਬਾਹਰੀ ਫੰਕਸ਼ਨਾਂ ਨੂੰ ਕਾਲ ਕਰਨ ਲਈ ਲੌਕਲ ਮਾਡਲਸ ਯੋਗ ਕਰਨਾ (ਟੂਲ/ਫੰਕਸ਼ਨ ਕਾਲਿੰਗ)
- OpenAI ਫੰਕਸ਼ਨ-ਕਾਲਿੰਗ ਫਾਰਮੈਟ ਰਾਹੀਂ ਟੂਲ ਸਕੀਮਾਂ ਪਰਿਭਾਸ਼ਿਤ ਕਰਨਾ
- ਬਹੁ-ਮੁੜਕਲੇ ਟੂਲ ਕਾਲਿੰਗ ਗੱਲਬਾਤ ਦਾ ਪ੍ਰਬੰਧਨ
- ਟੂਲ ਕਾਲਾਂ ਨੂੰ ਲੋਕਲ ਤੌਰ 'ਤੇ ਚਲਾਉਣਾ ਅਤੇ ਨਤੀਜੇ ਮਾਡਲ ਨੂੰ ਵਾਪਸ ਦੇਣਾ
- ਟੂਲ ਕਾਲਿੰਗ ਸਿਨਾਰੀਓ ਲਈ ਸਹੀ ਮਾਡਲ ਚੁਣੋ (Qwen 2.5, Phi-4-mini)
- ਟੂਲ ਕਾਲਿੰਗ ਲਈ SDK ਦਾ ਮੂਲ `ChatClient` ਵਰਤੋਂ (JavaScript)

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ | ਵੇਰਵਾ |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | ਮੌਸਮ/ਆਬਾਦੀ ਟੂਲਸ ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ |
| C# | `csharp/ToolCalling.cs` | .NET ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ |

---

### ਭਾਗ 12: Zava Creative Writer ਲਈ ਵੈੱਬ UI ਬਣਾਉਣਾ

**ਲੈਬ ਗਾਈਡ:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Zava Creative Writer ਲਈ ਬ੍ਰਾਊਜ਼ਰ ਅਧਾਰਿਤ ਫਰੰਟ ਐਂਡ ਸ਼ਾਮਲ ਕਰੋ
- Python (FastAPI), JavaScript (Node.js HTTP), ਅਤੇ C# (ASP.NET Core) ਤੋਂ ਸਾਂਝਾ UI ਸਰਵ ਕਰੋ
- ਬ੍ਰਾਊਜ਼ਰ ਵਿੱਚ Fetch API ਅਤੇ ReadableStream ਰਾਹੀਂ ਸਟੀਮਿੰਗ NDJSON ਵਰਤੋ
- ਲਾਈਵ ਏਜੰਟ ਸਥਿਤੀ ਬੈਜ ਅਤੇ ਰੀਅਲ-ਟਾਈਮ ਲੇਖ ਟੈਕਸਟ ਸਟੀਮਿੰਗ

**ਕੋਡ (ਸਾਂਝਾ UI):**

| ਫਾਇਲ | ਵੇਰਵਾ |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | ਪੇਜ਼ ਦਾ ਲੇਆਊਟ |
| `zava-creative-writer-local/ui/style.css` | ਸਟਾਈਲਿੰਗ |
| `zava-creative-writer-local/ui/app.js` | ਸਟੀਮ ਪੜ੍ਹਨ ਵਾਲਾ ਅਤੇ DOM ਅੱਪਡੇਟ ਲਾਜਿਕ |

**ਬੈਕਐਂਡ ਜੋੜ-ਤੋੜ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ | ਵੇਰਵਾ |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | ਸਥਿਰ UI ਸਰਵ ਕਰਨ ਲਈ ਅਪਡੇਟ |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ਨਵਾਂ HTTP ਸਰਵਰ ਜੋ ਓਰਕੇਸਟਰੇਟਰ ਨੂੰ ਘੇਰਨ ਵਾਲਾ ਹੈ |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | ਨਵਾਂ ASP.NET Core ਮਿਨੀਮਲ API ਪ੍ਰੋਜੈਕਟ |

---

### ਭਾਗ 13: ਵਰਕਸ਼ਾਪ ਮੁਕੰਮਲ
**ਲੈਬ ਗਾਈਡ:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- ਸਭ 12 ਹਿੱਸਿਆਂ ਵਿੱਚ ਤੁਸੀਂ ਜੋ ਕੁਝ ਬਣਾਇਆ ਹੈ ਉਸ ਦਾ ਸਾਰਾਂਸ਼
- ਤੁਹਾਡੇ ਐਪਲੀਕੇਸ਼ਨਾਂ ਨੂੰ ਵਧਾਉਣ ਲਈ ਹੋਰ ਵਿਚਾਰ
- ਸਰੋਤ ਅਤੇ ਦਸਤਾਵੇਜ਼ੀਕਰਨ ਲਈ ਲਿੰਕ

---

## ਪ੍ਰੋਜੈਕਟ ਦੀ ਸੰਰਚਨਾ

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

## ਸਰੋਤ

| ਸਰੋਤ | ਲਿੰਕ |
|----------|------|
| Foundry Local ਵੈੱਬਸਾਈਟ | [foundrylocal.ai](https://foundrylocal.ai) |
| ਮਾਡਲ ਕੈਟਾਲਾਗ | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| ਸ਼ੁਰੂਆਤ ਕਰਨ ਲਈ ਮਾਰਗਦਰਸ਼ਕ | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK ਰੈਫਰੈਂਸ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## ਲਾਇਸੈਂਸ

ਇਹ ਵਰਕਸ਼ਾਪ ਸਮੱਗਰੀ ਸਿੱਖਿਆ ਪ੍ਰਯੋਜਨਾਂ ਲਈ ਪ੍ਰਦਾਨ ਕੀਤੀ ਗਈ ਹੈ।

---

**ਖੁਸ਼ ਰਹੋ ਬਨਾਉਂਦੇ ਰਹੋ! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ਅਸਵੀਕਾਰੋਨਾ**:
ਇਹ ਦਸਤਾਵੇਜ਼ AI ਅਨੁਵਾਦ ਸੇਵਾ [Co-op Translator](https://github.com/Azure/co-op-translator) ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਅਨੁਵਾਦ ਕੀਤਾ ਗਿਆ ਹੈ। ਜਦੋਂ ਕਿ ਅਸੀਂ ਸਹੀਤਾ ਲਈ ਯਤਨਸ਼ੀਲ ਹਾਂ, ਕਿਰਪਾ ਕਰਕੇ ਧਿਆਨ ਵਿੱਚ ਰੱਖੋ ਕਿ ਸਵੈਚਲਿਤ ਅਨੁਵਾਦਾਂ ਵਿੱਚ ਗਲਤੀਆਂ ਜਾਂ ਅਸੂਚਿਤਤਾਵਾਂ ਹੋ ਸਕਦੀਆਂ ਹਨ। ਮੂਲ ਦਸਤਾਵੇਜ਼ ਆਪਣੇ ਮੂਲ ਭਾਸ਼ਾ ਵਿੱਚ ਪ੍ਰਮਾਣਿਕ ਸ੍ਰੋਤ ਵਜੋਂ ਮੰਨਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ। ਮਹੱਤਵਪੂਰਨ ਜਾਣਕਾਰੀ ਲਈ ਪੇਸ਼ੇਵਰ ਮਨੁੱਖੀ ਅਨੁਵਾਦ ਦੀ ਸਿਫ਼ਾਰਸ਼ ਕੀਤੀ ਜਾਂਦੀ ਹੈ। ਅਸੀਂ ਇਸ ਅਨੁਵਾਦ ਦੇ ਇਸਤੇਮਾਲ ਕਰਕੇ ਹੋਣ ਵਾਲੀਆਂ ਕੋਈ ਵੀ ਗਲਤਫ਼ਹਮੀਆਂ ਜਾਂ ਭ੍ਰਮਾਂ ਲਈ ਜਿੰਮੇਵਾਰ ਨਹੀਂ ਹਾਂ।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->