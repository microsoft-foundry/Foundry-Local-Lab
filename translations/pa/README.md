<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local ਵਰਕਸ਼ਾਪ - ਡਿਵਾਈਸ 'ਤੇ AI ਐਪ ਬਣਾਓ

ਆਪਣੇ ਮਸ਼ੀਨ ਉੱਤੇ ਲੈਂਗਵੇਜ ਮਾਡਲ ਚਲਾਉਣ ਅਤੇ [Foundry Local](https://foundrylocal.ai) ਅਤੇ [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) ਨਾਲ ਬੁੱਧਿਮਾਨ ਐਪਲੀਕੇਸ਼ਨਾਂ ਬਣਾਉਣ ਲਈ ਇੱਕ ਹੱਥ-ਓਣ ਵਾਲਾ ਵਰਕਸ਼ਾਪ।

> **Foundry Local ਕੀ ਹੈ?** Foundry Local ਇੱਕ ਹਲਕਾ ਫੁਲਕਾ ਰਨਟਾਈਮ ਹੈ ਜੋ ਤੁਹਾਨੂੰ ਸਿਰਫ ਆਪਣੇ ਹਾਰਡਵੇਅਰ 'ਤੇ ਭਾਸ਼ਾ ਮਾਡਲ ਡਾਊਨਲੋਡ, ਪ੍ਰਬੰਧਨ ਅਤੇ ਸਰਵ ਕਰਨ ਦੀ ਆਗਿਆ ਦਿੰਦਾ ਹੈ। ਇਹ ਇੱਕ **OpenAI-ਕੰਪੈਟਿਬਲ API** ਪ੍ਰਦਾਨ ਕਰਦਾ ਹੈ ਤਾਂ ਜੋ ਕੋਈ ਵੀ ਜੰਤਰ ਜਾਂ SDK ਜੋ OpenAI ਨਾਲ ਗੱਲ ਕਰਦਾ ਹੈ ਜੁੜ ਸਕਦਾ ਹੈ - ਕਿਸੇ ਕਲਾਉਡ ਖਾਤੇ ਦੀ ਲੋੜ ਨਹੀਂ।

### 🌐 ਬਹੁ-ਭਾਸ਼ਾਈ ਸਮਰਥਨ

#### GitHub Action ਰਾਹੀਂ ਸਹਾਇਤ (ਆਟੋਮੈਟਿਕ ਅਤੇ ਸਦਾ ਅਪ-ਟੂ-ਡੇਟ)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](../hr/README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](./README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **ਸਥਾਨਕ ਤੌਰ 'ਤੇ ਕਲੋਨ ਕਰਨਾ ਪਸੰਦ ਹੈ?**
>
> ਇਸ ਰਿਪੋਜ਼ਟਰੀ ਵਿੱਚ 50+ ਭਾਸ਼ਾ ਅਨੁਵਾਦ ਹਨ ਜੋ ਡਾਊਨਲੋਡ ਦਾ ਆਕਾਰ ਬਹੁਤ ਵਧਾਉਂਦੇ ਹਨ। ਬਿਨਾਂ ਅਨੁਵਾਦਾਂ ਦੇ ਕਲੋਨ ਕਰਨ ਲਈ, sparse checkout ਵਰਤੋ:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (ਵਿੰਡੋਜ਼):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> ਇਸ ਨਾਲ ਤੁਹਾਨੂੰ ਕੋਰਸ ਪੂਰਾ ਕਰਨ ਲਈ ਸਭ ਕੁਝ ਤੇਜ਼ ਡਾਊਨਲੋਡ ਕਰਕੇ ਮਿਲੇਗਾ।
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## ਸਿੱਖਣ ਦੇ ਲਕਸ਼

ਇਸ ਵਰਕਸ਼ਾਪ ਦੇ ਅੰਤ ਤੱਕ ਤੁਸੀਂ ਸਮਰੱਥ ਹੋਵੋਗੇ:

| # | ਲਕਸ਼ |
|---|-----------|
| 1 | Foundry Local ਨੂੰ ਇੰਸਟਾਲ ਕਰੋ ਅਤੇ CLI ਨਾਲ ਮਾਡਲ ਪ੍ਰਬੰਧਨ ਕਰੋ |
| 2 | Foundry Local SDK API ਨੂੰ ਕਾਰਜਕਾਰੀ ਮਾਡਲ ਪ੍ਰਬੰਧਨ ਲਈ ਮਾਹਰ ਬਣਾਓ |
| 3 | Python, JavaScript, ਅਤੇ C# SDKs ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਸਥਾਨਕ ਇਨਫਰੈਂਸ ਸਰਵਰ ਨਾਲ ਜੁੜੋ |
| 4 | ਇੱਕ Retrieval-Augmented Generation (RAG) ਪਾਈਪਲਾਈਨ ਬਣਾਓ ਜੋ ਤੁਹਾਡੇ ਆਪਣੇ ਡੇਟਾ ਵਿੱਚ ਜਵਾਬ ਧਰ੍ਹਾ ਕਰਦੀ ਹੈ |
| 5 | ਸਥਾਈ ਹਦਾਇਤਾਂ ਅਤੇ ਪੇਰਸੋਨाज़ ਵਾਲੇ AI ਏਜੰਟ ਬਣਾਓ |
| 6 | ਫੀਡਬੈਕ ਲੂਪਿਸ਼ਨ ਵਾਲੇ ਬਹੁ-ਏਜੰਟ ਵਰਕਫਲੋਜ਼ ਦਾ ਸੁਤੰਤਰ ਬਣਾਓ |
| 7 | ਇੱਕ ਉਤਪਾਦਨ ਕੈਪਸਟੋਨ ਐਪ - Zava Creative Writer ਦੀ ਪੜਚੋਲ ਕਰੋ |
| 8 | ਸੁਵਰਨ ਡੇਟਾਸੈੱਟ ਅਤੇ LLM-as-judge ਸਕੋਰਿੰਗ ਨਾਲ ਮੁਲਾਂਕਣ ਫਰੇਮਵਰਕ ਬਣਾਓ |
| 9 | Whisper ਨਾਲ ਆਡੀਓ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਕਰੋ - Foundry Local SDK ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਡਿਵਾਈਸ 'ਤੇ ਸਪੀਚ-ਟੂ-ਟੈਕਸਟ |
| 10 | ONNX Runtime GenAI ਅਤੇ Foundry Local ਨਾਲ ਕਸਟਮ ਜਾਂ Hugging Face ਮਾਡਲਾਂ ਨੂੰ ਕੰਪਾਈਲ ਅਤੇ ਚਲਾਓ |
| 11 | ਟੂਲ-ਕਾਲਿੰਗ ਪੈਟਰਨ ਨਾਲ ਸਥਾਨਕ ਮਾਡਲਾਂ ਨੂੰ ਬਾਹਰੀ ਫੰਕਸ਼ਨਾਂ ਨੂੰ ਕਾਲ ਕਰਨ ਦੇ ਯੋਗ ਬਣਾਓ |
| 12 | Zava Creative Writer ਲਈ ਬ੍ਰਾਉਜ਼ਰ-ਅਧਾਰਿਤ UI ਬਣਾਓ ਅਤੇ ਰੀਅਲ-ਟਾਈਮ ਸਟ੍ਰੀਮਿੰਗ ਸ਼ਾਮਲ ਕਰੋ |

---

## ਲੋੜਵੰਦ ਤੱਤ

| ਲੋੜ | ਵੇਰਵਾ |
|-------------|---------|
| **ਹਾਰਡਵੇਅਰ** | ਘੱਟੋ-ਘੱਟ 8 GB ਰੈਮ (16 GB ਸਿਫ਼ਾਰਸ਼ ਕੀਤੀ); AVX2-ਯੋਗ CPU ਜਾਂ ਸਮਰਥਿਤ GPU |
| **ਓਐਸ** | Windows 10/11 (x64/ARM), Windows Server 2025, ਜਾਂ macOS 13+ |
| **Foundry Local CLI** | Windows ਲਈ `winget install Microsoft.FoundryLocal` ਜਾਂ macOS ਲਈ `brew tap microsoft/foundrylocal && brew install foundrylocal` ਰਾਹੀਂ ਇੰਸਟਾਲ ਕਰੋ। ਵੇਰਵੇ ਲਈ [getting started guide](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) ਨੂੰ ਵੇਖੋ। |
| **ਭਾਸ਼ਾ ਰਨਟਾਈਮ** | **Python 3.9+** ਅਤੇ/ਜਾਂ **.NET 9.0+** ਅਤੇ/ਜਾਂ **Node.js 18+** |
| **Git** | ਇਸ ਰਿਪੋਜ਼ਟਰੀ ਨੂੰ ਕਲੋਨ ਕਰਨ ਲਈ |

---

## ਸ਼ੁਰੂਆਤ

```bash
# 1. ਰਿਪੋਜ਼ਿਟਰੀ ਨੂੰ ਕਲੋਨ ਕਰੋ
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. ਪੱਕਾ ਕਰੋ ਕਿ Foundry Local ਇੰਸਟਾਲ ਹੈ
foundry model list              # ਉਪਲਬਧ ਮਾਡਲਾਂ ਦੀ ਸੂਚੀ ਬਣਾਓ
foundry model run phi-3.5-mini  # ਇਕ ਇੰਟਰੈਕਟਿਵ ਚੈਟ ਸ਼ੁਰੂ ਕਰੋ

# 3. ਆਪਣੀ ਭਾਸ਼ਾ ਟਰੈਕ ਚੁਣੋ (ਪੂਰੇ ਸੈਟਅੱਪ ਲਈ ਭਾਗ 2 ਲੈਬ ਵੇਖੋ)
```

| ਭਾਸ਼ਾ | ਸ਼ੁਰੂਆਤ ਤੇਜ਼ੀ ਨਾਲ |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## ਵਰਕਸ਼ਾਪ ਹਿੱਸੇ

### ਹਿੱਸਾ 1: Foundry Local ਦੇ ਨਾਲ ਸ਼ੁਰੂਆਤ

**ਲੈਬ ਗਾਈਡ:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Foundry Local ਕੀ ਹੈ ਅਤੇ ਇਹ ਕਿਵੇਂ ਕੰਮ ਕਰਦਾ ਹੈ
- Windows ਅਤੇ macOS 'ਤੇ CLI ਇੰਸਟਾਲ ਕਰਨਾ
- ਮਾਡਲਾਂ ਦੀ ਪੜਚੋਲ - ਲਿਸਟਿੰਗ, ਡਾਊਨਲੋਡ, ਚਲਾਉਣਾ
- ਮਾਡਲ ਉਪਨਾਮ ਅਤੇ ਗਤੀਸ਼ੀਲ ਪੋਰਟ ਦੀ ਸਮਝ

---

### ਹਿੱਸਾ 2: Foundry Local SDK ਗਹਿਰਾਈ ਨਾਲ

**ਲੈਬ ਗਾਈਡ:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- ਐਪਲੀਕੇਸ਼ਨ ਵਿਕਾਸ ਲਈ CLI ਦੇ ਬਜਾਏ SDK ਦੀ ਵਰਤੋਂ ਕਿਉਂ
- Python, JavaScript ਅਤੇ C# ਲਈ ਪੂਰਾ SDK API ਰੈਫਰੈਂਸ
- ਸਰਵਿਸ ਪ੍ਰਬੰਧਨ, ਕੈਟਲੋਗ ਬ੍ਰਾਊਜ਼ਿੰਗ, ਮਾਡਲ ਲਾਈਫਸਾਈਕਲ (ਡਾਊਨਲੋਡ, ਲੋਡ, ਅਨਲੋਡ)
- ਤੁਰੰਤ ਸ਼ੁਰੂਆਤ ਪੈਟਰਨ: Python ਨਿਰਮਾਤਾ ਬੂਟਸਟ੍ਰੈਪ, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` ਮੈਟਾਡੇਟਾ, ਉਪਨਾਮ ਅਤੇ ਹਾਰਡਵੇਅਰ-ਉਪਯੋਗਤਮ ਮਾਡਲ ਚੋਣ

---

### ਹਿੱਸਾ 3: SDKs ਅਤੇ APIs

**ਲੈਬ ਗਾਈਡ:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Python, JavaScript ਅਤੇ C# ਨਾਲ Foundry Local ਨਾਲ ਜੁੜਨਾ
- Foundry Local SDK ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਸਰਵਿਸ ਨੂੰ ਕਾਰਜਕਾਰੀ ਤੌਰ 'ਤੇ ਪ੍ਰਬੰਧਿਤ ਕਰਨਾ
- OpenAI-ਕੰਪੈਟਿਬਲ API ਰਾਹੀਂ ਸਟ੍ਰੀਮਿੰਗ ਚੈਟ ਕੰਪਲੀਸ਼ਨ
- ਹਰ ਭਾਸ਼ਾ ਲਈ SDK ਢੰਗ ਰੈਫਰੈਂਸ

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ | ਵੇਰਵਾ |
|----------|------|-------------|
| Python | `python/foundry-local.py` | ਬੁਨਿਆਦੀ ਸਟ੍ਰੀਮਿੰਗ ਚੈਟ |
| C# | `csharp/BasicChat.cs` | .NET ਨਾਲ ਸਟ੍ਰੀਮਿੰਗ ਚੈਟ |
| JavaScript | `javascript/foundry-local.mjs` | Node.js ਨਾਲ ਸਟ੍ਰੀਮਿੰਗ ਚੈਟ |

---

### ਹਿੱਸਾ 4: Retrieval-Augmented Generation (RAG)

**ਲੈਬ ਗਾਈਡ:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- RAG ਕੀ ਹੈ ਅਤੇ ਇਸ ਦੀ ਮਹੱਤਤਾ
- ਯਾਦ ਵਿੱਚ ਇੱਕ ਗਿਆਨ ਆਧਾਰ ਬਣਾਉਣਾ
- ਕੀਵਰਡ-ਓਵਰਲੈਪ ਰੀਟਰੀਵਲ ਨਾਲ ਸਕੋਰਿੰਗ
- ਅਧਾਰਿਤ ਸਿਸਟਮ ਪ੍ਰਾਮਪਟ ਬਣਾਉਣਾ
- ਡਿਵਾਈਸ 'ਤੇ ਪੂਰੀ RAG ਪਾਈਪਲਾਈਨ ਚਲਾਉਣਾ

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### ਹਿੱਸਾ 5: AI ਏਜੰਟ ਬਣਾਉਣਾ

**ਲੈਬ ਗਾਈਡ:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- AI ਏਜੰਟ ਕੀ ਹੈ (ਕੱਚਾ LLM ਕਾਲ ਵਿਰੁੱਧ)
- `ChatAgent` ਪੈਟਰਨ ਅਤੇ Microsoft Agent Framework
- ਸਿਸਟਮ ਨਿਰਦੇਸ਼, ਪ੍ਰਸੋਨਾ ਅਤੇ ਬਹੁ-ਟਰਨ ਗੱਲਬਾਤਾਂ
- ਏਜੰਟਾਂ ਤੋਂ ਸੰਰਚਿਤ ਆਉਟਪੁੱਟ (JSON)

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ | ਵੇਰਵਾ |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Agent Framework ਨਾਲ ਇਕੱਲਾ ਏਜੰਟ |
| C# | `csharp/SingleAgent.cs` | ਇਕੱਲਾ ਏਜੰਟ (ChatAgent ਪੈਟਰਨ) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | ਇਕੱਲਾ ਏਜੰਟ (ChatAgent ਪੈਟਰਨ) |

---

### ਹਿੱਸਾ 6: ਬਹੁ-ਏਜੰਟ ਵਰਕਫਲੋ

**ਲੈਬ ਗਾਈਡ:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- ਬਹੁ-ਏਜੰਟ ਪਾਈਪਲਾਈਨ: ਖੋਜਕਾਰ → ਲੇਖਕ → ਸੰਪਾਦਕ
- ਕ੍ਰਮਿਕ ਸਹਿਯੋਜਨ ਅਤੇ ਫੀਡਬੈਕ ਲੂਪ
- ਸਾਂਝਾ ਕਾਨਫਿਗਰੇਸ਼ਨ ਅਤੇ ਸੰਰਚਿਤ ਹੈਂਡ-ਆਫ
- ਆਪਣਾ ਬਹੁ-ਏਜੰਟ ਵਰਕਫਲੋ ਡਿਜ਼ਾਈਨ ਕਰਨਾ

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ | ਵੇਰਵਾ |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | ਤਿੰਨ-ਏਜੰਟ ਪਾਈਪਲਾਈਨ |
| C# | `csharp/MultiAgent.cs` | ਤਿੰਨ-ਏਜੰਟ ਪਾਈਪਲਾਈਨ |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | ਤਿੰਨ-ਏਜੰਟ ਪਾਈਪਲਾਈਨ |

---

### ਹਿੱਸਾ 7: Zava Creative Writer - ਕੈਪਸਟੋਨ ਐਪਲੀਕੇਸ਼ਨ

**ਲੈਬ ਗਾਈਡ:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- 4 ਖਾਸ ਏਜੰਟਾਂ ਨਾਲ ਇੱਕ ਉਤਪਾਦਨ ਰੂਪ ਦਾ ਬਹੁ-ਏਜੰਟ ਐਪ
- ਮੁਲਾਂਕਣ-ਪ੍ਰੇਰਿਤ ਫੀਡਬੈਕ ਲੂਪ ਦੇ ਨਾਲ ਕ੍ਰਮਿਕ ਪਾਈਪਲਾਈਨ
- ਸਟ੍ਰੀਮਿੰਗ ਆਉਟਪੁੱਟ, ਉਤਪਾਦ ਕੈਟਲੋਗ ਖੋਜ, ਸੰਰਚਿਤ JSON ਹੈਂਡ-ਆਫ
- Python (FastAPI), JavaScript (Node.js CLI), ਅਤੇ C# (.NET ਕੰਸੋਲ) ਵਿੱਚ ਪੂਰਾ ਕਾਰਯਾਨਵयन

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਡਾਇਰੈਕਟਰੀ | ਵੇਰਵਾ |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI ਵੈੱਬ ਸਰਵਿਸ ਨਾਲ ਸੰਚਾਲਕ |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI ਐਪਲੀਕੇਸ਼ਨ |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 ਕੰਸੋਲ ਐਪਲੀਕੇਸ਼ਨ |

---

### ਹਿੱਸਾ 8: ਮੁਲਾਂਕਣ-ਅਧਾਰਤ ਵਿਕਾਸ

**ਲੈਬ ਗਾਈਡ:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- ਸੁਵਰਨ ਡੇਟਾਸੈੱਟ ਨਾਲ AI ਏਜੰਟਾਂ ਲਈ ਪ੍ਰਣਾਲੀਬੱਧ ਮੁਲਾਂਕਣ ਫਰੇਮਵਰਕ ਬਣਾਓ
- ਨਿਯਮ-ਅਧਾਰਿਤ ਜਾਂਚਾਂ (ਲੰਬਾਈ, ਕੀਵਰਡ ਕਵਰੇਜ, ਮਨਾਹੀਸ਼ੁਦਾ ਸ਼ਰਤਾਂ) + LLM-as-judge ਸਕੋਰਿੰਗ
- ਪ੍ਰਾਮਪਟ ਵੈਰੀਅੰਟਾਂ ਦੀ ਇੱਕੱਠੀ ਸਕੋਰਕਾਰਡ ਸਹਿਤ ਬੁਝਾਰੇ-ਵੱਲਾ ਟੁਲਨਾ
- ਹਿੱਸਾ 7 ਤੋਂ Zava Editor ਏਜੰਟ ਪੈਟਰਨ ਨੂੰ ਬਾਹਰਲੀ ਪਰਖ ਸੂਟ ਵਿੱਚ ਤਬਦੀਲ ਕਰਦਾ ਹੈ
- Python, JavaScript ਅਤੇ C# ਟ੍ਰੈਕ

**ਕੋਡ ਨਮੂਨੇ:**

| ਭਾਸ਼ਾ | ਫਾਇਲ | ਵੇਰਵਾ |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | ਮੁਲਾਂਕਣ ਫਰੇਮਵਰਕ |
| C# | `csharp/AgentEvaluation.cs` | ਮੁਲਾਂਕਣ ਫਰੇਮਵਰਕ |
| JavaScript | `javascript/foundry-local-eval.mjs` | ਮੁਲਾਂਕਣ ਫਰੇਮਵਰਕ |

---

### ਹਿੱਸਾ 9: Whisper ਨਾਲ ਵੋਇਸ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ

**ਲੈਬ ਗਾਈਡ:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- ਸਪੀਚ-ਟੂ-ਟੈਕਸਟ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਲਈ OpenAI Whisper ਜੋ ਲੋਕਲ ਤੌਰ ਤੇ ਚਲ੍ਹਦਾ ਹੈ  
- ਪ੍ਰਾਈਵੇਸੀ-ਫਰਸਟ ਆਡੀਓ ਪ੍ਰੋਸੈੱਸਿੰਗ - ਆਡੀਓ ਕਦੇ ਵੀ ਤੁਹਾਡੇ ਡਿਵਾਈਸ ਤੋਂ ਬਾਹਰ ਨਹੀਂ ਜਾਂਦਾ  
- Python, JavaScript, ਅਤੇ C# ਟਰੈਕਸ `client.audio.transcriptions.create()` (Python/JS) ਅਤੇ `AudioClient.TranscribeAudioAsync()` (C#) ਨਾਲ  
- ਹਨਸੀ-ਹਥੋਂ ਅਭਿਆਸ ਲਈ Zava-ਥੀਮ ਵਾਲੇ ਨਮੂਨਾ ਆਡੀਓ ਫਾਇਲਾਂ ਸ਼ਾਮਿਲ ਹਨ  

**ਕੋਡ ਨਮੂਨੇ:**  

| Language | File | Description |  
|----------|------|-------------|  
| Python | `python/foundry-local-whisper.py` | Whisper ਵਾਇਸ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ |  
| C# | `csharp/WhisperTranscription.cs` | Whisper ਵਾਇਸ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ |  
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper ਵਾਇਸ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ |  

> **Note:** ਇਹ ਲੈਬ **Foundry Local SDK** ਦੀ ਵਰਤੋਂ ਕਰਦਾ ਹੈ ਜੋ ਪ੍ਰੋਗਰਾਮੈਟਿਕ ਤੌਰ ਤੇ Whisper ਮਾਡਲ ਨੂੰ ਡਾਊਨਲੋਡ ਅਤੇ ਲੋਡ ਕਰਦਾ ਹੈ, ਫਿਰ ਆਡੀਓ ਨੂੰ ਲੋਕਲ OpenAI-ਸੰਗਤ ਐਂਡਪੌਇੰਟ ਨੂੰ ਟ੍ਰਾਂਸਕ੍ਰਿਪਸ਼ਨ ਲਈ ਭੇਜਦਾ ਹੈ। Whisper ਮਾਡਲ (`whisper`) Foundry Local ਕੈਟਾਲੌਗ ਵਿੱਚ ਦਿੱਤਾ ਗਿਆ ਹੈ ਅਤੇ ਪੂਰੀ ਤਰ੍ਹਾਂ ਡਿਵਾਈਸ ਤੇ ਚਲਦਾ ਹੈ - ਕੋਈ ਕਲਾਊਡ API ਚਾਬੀਆਂ ਜਾਂ ਨੈੱਟਵਰਕ ਐਕਸੈਸ ਦੀ ਲੋੜ ਨਹੀਂ।  

---  

### ਭਾਗ 10: ਕਸਟਮ ਜਾਂ Hugging Face ਮਾਡਲਾਂ ਦੀ ਵਰਤੋਂ  

**Lab guide:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)  

- Hugging Face ਮਾਡਲਾਂ ਨੂੰ ONNX ਰਨਟਾਈਮ GenAI ਮਾਡਲ ਬਿੱਲਡਰ ਦੀ ਵਰਤੋਂ ਨਾਲ ਅਤਿ ਉੱਨਤ ONNX ਫਾਰਮੈਟ ਵਿੱਚ ਕંપਾਈਲ ਕਰਨਾ  
- ਹਾਰਡਵੇਅਰ-ਵਿਸ਼ੇਸ਼ ਕંપਾਈਲੇਸ਼ਨ (CPU, NVIDIA GPU, DirectML, WebGPU) ਅਤੇ ਕੁਆੰਟੀਜੇਸ਼ਨ (int4, fp16, bf16)  
- Foundry Local ਲਈ ਚੈਟ-ਟੈਂਪਲੇਟ ਕਨਫਿਗਰੇਸ਼ਨ ਫਾਇਲਾਂ ਬਣਾਉਣਾ  
- ਕંપਾਈਲ ਕੀਤੇ ਮਾਡਲਾਂ ਨੂੰ Foundry Local ਕੈਸ਼ ਵਿੱਚ ਸ਼ਾਮਿਲ ਕਰਨਾ  
- CLI, REST API, ਅਤੇ OpenAI SDK ਰਾਹੀਂ ਕਸਟਮ ਮਾਡਲ ਚਲਾਉਣਾ  
- ਸੰਦਰਭ ਉਦਾਹਰਨ: Qwen/Qwen3-0.6B ਦਾ ਅੰਤ-ਤੱਕ ਕંપਾਈਲੇਸ਼ਨ  

---  

### ਭਾਗ 11: ਲੋਕਲ ਮਾਡਲਾਂ ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ  

**Lab guide:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)  

- ਬਾਹਰੀ ਫੰਕਸ਼ਨਜ਼ ਨੂੰ ਕਾਲ ਕਰਨ ਲਈ ਲੋਕਲ ਮਾਡਲਾਂ ਨੂੰ ਯੋਗ ਬਣਾਓ (ਟੂਲ/ਫੰਕਸ਼ਨ ਕਾਲਿੰਗ)  
- OpenAI ਫੰਕਸ਼ਨ-ਕਾਲਿੰਗ ਫਾਰਮੈਟ ਵਰਤ ਕੇ ਟੂਲ ਸਕੀਮਾਵਾਂ ਪਰਿਭਾਸ਼ਿਤ ਕਰੋ  
- ਬਹੁ-ਮੁੜ ਫੇਰ ਵਾਲੇ ਟੂਲ-ਕਾਲਿੰਗ ਗੱਲਬਾਤ ਦਾ ਪ੍ਰਬੰਧ ਕਰਨ  
- ਟੂਲ ਕਾਲਾਂ ਨੂੰ ਲੋਕਲ ਰੂਪ ਵਿੱਚ ਚਲਾਉਣਾ ਅਤੇ ਮਾਡਲ ਲਈ ਨਤੀਜੇ ਵਾਪਸ ਭੇਜਣਾ  
- ਟੂਲ-ਕਾਲਿੰਗ ਸਿੱਦਾਂਤ ਲਈ ਸਹੀ ਮਾਡਲ ਦੀ ਚੋਣ (Qwen 2.5, Phi-4-mini)  
- SDK ਦੇ ਆਪਣੀ ਜਨਮਦਾਤਾ `ChatClient` ਨੂੰ JavaScript ਵਿੱਚ ਟੂਲ ਕਾਲਿੰਗ ਲਈ ਵਰਤੋ  

**ਕੋਡ ਨਮੂਨੇ:**  

| Language | File | Description |  
|----------|------|-------------|  
| Python | `python/foundry-local-tool-calling.py` | ਮੌਸਮ/ਆਬਾਦੀ ਟੂਲਜ਼ ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ |  
| C# | `csharp/ToolCalling.cs` | .NET ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ |  
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | ChatClient ਨਾਲ ਟੂਲ ਕਾਲਿੰਗ |  

---  

### ਭਾਗ 12: Zava Creative Writer ਲਈ ਵੈੱਬ UI ਤਿਆਰ ਕਰਨਾ  

**Lab guide:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)  

- Zava Creative Writer ਲਈ ਬ੍ਰਾਊਜ਼ਰ-ਅਧਾਰਿਤ ਫਰੰਟ ਐਂਡ ਜੋੜੋ  
- Python (FastAPI), JavaScript (Node.js HTTP), ਅਤੇ C# (ASP.NET Core) ਤੋਂ ਸਾਂਝਾ UI ਸਰਵ ਕਰੋ  
- ਬ੍ਰਾਊਜ਼ਰ ਵਿੱਚ Fetch API ਅਤੇ ReadableStream ਦੇ ਨਾਲ ਸਟਰੀਮਿੰਗ NDJSON ਖਪਤ ਕਰੋ  
- ਲਾਈਵ ਏਜੰਟ ਸਥਿਤੀ ਬੈਜ ਅਤੇ ਅਸਲੀ ਸਮੇਂ ਲੇਖ ਟੈਕਸਟ ਸਟਰੀਮਿੰਗ  

**ਕੋਡ (ਸਾਂਝਾ UI):**  

| File | Description |  
|------|-------------|  
| `zava-creative-writer-local/ui/index.html` | ਪੇਜ ਲੇਆਉਟ |  
| `zava-creative-writer-local/ui/style.css` | ਸਟਾਈਲਿੰਗ |  
| `zava-creative-writer-local/ui/app.js` | ਸਟਰੀਮ ਰੀਡਰ ਅਤੇ DOM ਅਪਡੇਟ ਲਾਜਿਕ |  

**ਬੈਕਐਂਡ ਵਿਸਥਾਰ:**  

| Language | File | Description |  
|----------|------|-------------|  
| Python | `zava-creative-writer-local/src/api/main.py` | ਸਟੈਟਿਕ UI ਸਰਵ ਕਰਨ ਲਈ ਅਪਡੇਟ ਕੀਤਾ |  
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | ਨਵਾਂ HTTP ਸਰਵਰ ਜੋ ਆਰਕੀਸਟ੍ਰੇਟਰ ਨੂੰ ਰੈਪਰ ਕਰਦਾ ਹੈ |  
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | ਨਵਾਂ ASP.NET Core ਮਿਨੀਮਲ API ਪ੍ਰੋਜੈਕਟ |  

---  

### ਭਾਗ 13: ਵਰਕਸ਼ਾਪ ਮੁਕੰਮਲ  

**Lab guide:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)  

- ਤੁਹਾਡੇ ਦੁਆਰਾ ਸਭ 12 ਭਾਗਾਂ ਵਿੱਚ ਬਣਾਈਆਂ ਗਈਆਂ ਚੀਜ਼ਾਂ ਦਾ ਸਾਰਾਂਸ਼  
- ਤੁਹਾਡੀਆਂ ਐਪਲੀਕੇਸ਼ਨਾਂ ਵਧਾਉਣ ਲਈ ਹੋਰ ਵਿਚਾਰ  
- ਸਾਧਨਾਂ ਅਤੇ ਦਸਤਾਵੇਜ਼ਾਂ ਲਈ ਲਿੰਕਾਂ  

---  

## ਪ੍ਰੋਜੈਕਟ ਦੀ ਬਣਤਰ  

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

| Resource | Link |  
|----------|------|  
| Foundry Local ਵੈੱਬਸਾਈਟ | [foundrylocal.ai](https://foundrylocal.ai) |  
| ਮਾਡਲ ਕੈਟਾਲੌਗ | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |  
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |  
| ਸ਼ੁਰੂਆਤ ਗਾਈਡ | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |  
| Foundry Local SDK ਰੈਫਰੰਸ | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |  
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |  
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |  
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |  

---  

## ਲਾਇਸੈਂਸ  

ਇਹ ਵਰਕਸ਼ਾਪ ਸਮੱਗਰੀ ਸਿੱਖਿਆ ਲਈ ਮੁਹੱਈਆ ਕੀਤੀ ਗਈ ਹੈ।  

---  

**ਸ਼ੁਭਕਾਮਨਾਵਾਂ ਬਣਾਉਣ ਲਈ! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ਅਸਵੀਕਾਰੋਪਤਰ**:
ਇਹ ਦਸਤਾਵੇਜ਼ AI ਅਨੁਵਾਦ ਸੇਵਾ [Co-op Translator](https://github.com/Azure/co-op-translator) ਦੀ ਵਰਤੋਂ ਕਰਕੇ ਅਨੁਵਾਦਿਤ ਕੀਤਾ ਗਿਆ ਹੈ। ਜਦੋਂ ਕਿ ਅਸੀਂ ਸਹੀਤਾ ਲਈ ਯਤਨ ਕਰਦੇ ਹਾਂ, ਕਿਰਪਾ ਕਰਕੇ ਜਾਣੋ ਕਿ ਆਟੋਮੈਟਿਕ ਅਨੁਵਾਦਾਂ ਵਿੱਚ ਗਲਤੀਆਂ ਜਾਂ ਅਸਥਿਰਤਾਵਾਂ ਹੋ ਸਕਦੀਆਂ ਹਨ। ਮੂਲ ਦਸਤਾਵੇਜ਼ ਨੂੰ ਇਸ ਦੀ ਮੂਲ ਭਾਸ਼ਾ ਵਿੱਚ ਪ੍ਰਮਾਣਿਕ ਸਰੋਤ سمجھਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ। ਮਹੱਤਵਪੂਰਨ ਜਾਣਕਾਰੀ ਲਈ, ਪੇਸ਼ੇਵਰ ਮਨੁੱਖੀ ਅਨੁਵਾਦ ਦੀ ਸਿਫਾਰਿਸ਼ ਕੀਤੀ ਜਾਂਦੀ ਹੈ। ਅਸੀਂ ਇਸ ਅਨੁਵਾਦ ਦੀ ਵਰਤੋਂ ਕਾਰਨ ਹੋਣ ਵਾਲੀਆਂ ਕਿਸੇ ਵੀ ਗਲਤਫਹਿਮੀਆਂ ਜਾਂ ਗਲਤ ਅਰਥ ਲੱਗਣ ਲਈ ਜ਼ਿੰਮੇਵਾਰ ਨਹੀਂ ਹਾਂ।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->