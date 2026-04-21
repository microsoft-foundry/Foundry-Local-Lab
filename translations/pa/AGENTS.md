# ਕੋਡਿੰਗ ਏਜੰਟ ਹਦਾਇਤਾਂ

ਇਹ ਫਾਇਲ ਇਸ ਰਿਪੋਜ਼ਿਟਰੀ ਵਿੱਚ ਕੰਮ ਕਰਨ ਵਾਲੇ ਏਆਈ ਕੋਡਿੰਗ ਏਜੰਟਾਂ (GitHub Copilot, Copilot Workspace, Codex, ਆਦਿ) ਲਈ ਸੰਦਰਭ ਪ੍ਰਦਾਨ ਕਰਦੀ ਹੈ।

## ਪ੍ਰੋਜੈਕਟ ਦਾ ਓਵਰਵਿਊ

ਇਹ [Foundry Local](https://foundrylocal.ai) ਨਾਲ ਏਆਈ ਐਪਲੀਕੇਸ਼ਨ ਬਣਾਉਣ ਲਈ ਇੱਕ **ਹੱਥ-ਅਨ ਵਰਕਸ਼ਾਪ** ਹੈ — ਇੱਕ ਹਲਕਾ ਰਨਟਾਈਮ ਜੋ ਬਿਲਕੁਲ ਡਿਵਾਈਸ 'ਤੇ ਹੀ ਭਾਸ਼ਾ ਮਾਡਲਾਂ ਨੂੰ ਡਾਊਨਲੋਡ, ਪਰਬੰਧਿਤ ਅਤੇ ਸਰਵ ਕਰਦਾ ਹੈ ਜਿਸ ਨੂੰ OpenAI-ਕੰਪੈਟਿਬਲ API ਰਾਹੀਂ ਐਕਸੈੱਸ ਕੀਤਾ ਜਾ ਸਕਦਾ ਹੈ। ਵਰਕਸ਼ਾਪ ਵਿੱਚ ਪੈਦਲ-ਦਰ-ਪੈਦਲ ਲੈਬ ਗਾਈਡਾਂ ਅਤੇ ਪਾਇਥਨ, ਜਾਵਾਸਕ੍ਰਿਪਟ ਅਤੇ C# ਵਿੱਚ ਚਲਾਉਣ ਯੋਗ ਕੋਡ ਸੈਮਪਲ ਸ਼ਾਮਿਲ ਹਨ।

## ਰਿਪੋਜ਼ਿਟਰੀ ਦੀ ਸੰਰਚਨਾ

```
├── labs/                              # Markdown lab guides (Parts 1–13)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app + Part 12 UI (Python/JS/C#)
│   ├── ui/                            # Shared browser UI (vanilla HTML/CSS/JS)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service (serves UI)
│       ├── javascript/                # Node.js CLI + HTTP server (server.mjs)
│       ├── csharp/                    # .NET console multi-agent app
│       └── csharp-web/                # .NET ASP.NET Core minimal API (serves UI)
├── samples/audio/                     # Part 9 sample WAV files + generator script
├── images/                            # Diagrams referenced by lab guides
├── README.md                          # Workshop overview and navigation
├── KNOWN-ISSUES.md                    # Known issues and workarounds
├── package.json                       # Root devDependency (mermaid-cli for diagrams)
└── AGENTS.md                          # This file
```

## ਭਾਸ਼ਾ ਅਤੇ ਫ੍ਰੇਮਵਰਕ ਵੇਰਵੇ

### ਪਾਇਥਨ
- **ਟਿਕਾਣਾ:** `python/`, `zava-creative-writer-local/src/api/`
- **ਨਿਰਭਰਤਾਵਾਂ:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **ਮੁੱਖ ਪੈਕੇਜ:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **కమీਨੇ ਹੋਣ ਵਾਲੀ ਸੰස්ਕਰਣ:** Python 3.9+
- **ਚਲਾਉਣਾ:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### ਜਾਵਾਸਕ੍ਰਿਪਟ
- **ਟਿਕਾਣਾ:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **ਨਿਰਭਰਤਾਵਾਂ:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **ਮੁੱਖ ਪੈਕੇਜ:** `foundry-local-sdk`, `openai`
- **ਮੋਡੀਊਲ ਸਿਸਟਮ:** ES ਮੋਡੀਊਲਜ਼ (`.mjs` ਫਾਇਲਾਂ, `"type": "module"`)
- **ਘੱਟੋ-ਘੱਟ ਸੰਸਕਰਣ:** Node.js 18+
- **ਚਲਾਉਣਾ:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **ਟਿਕਾਣਾ:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **ਪ੍ਰੋਜੈਕਟ ਫਾਇਲਾਂ:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **ਮੁੱਖ ਪੈਕੇਜ:** `Microsoft.AI.Foundry.Local` (ਗੈਰ-ਵਿੰਡੋਜ਼), `Microsoft.AI.Foundry.Local.WinML` (ਵਿੰਡੋਜ਼ — QNN EP ਸਮੇਤ ਸੁਪਰਸੈੱਟ), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **ਲਕੜੀ:** .NET 9.0 (ਸ਼ਰਤੀ TFM: ਵਿੰਡੋਜ਼ ਲਈ `net9.0-windows10.0.26100`, ਹੋਰ ਥਾਵਾਂ ਲਈ `net9.0`)
- **ਚਲਾਉਣਾ:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## ਕੋਡਿੰਗ ਰਿਵਾਜ਼

### ਆਮ
- ਸਾਰੇ ਕੋਡ ਸੈਮਪਲ **ਖੁਦ-ਮੁਖਤਾਰ ਇਕ ਫਾਇਲ ਉਦਾਹਰਨਾਂ** ਹਨ — ਕੋਈ ਸਾਂਝੀ ਸਹੂਲਤ ਵਾਲੀ ਲਾਇਬ੍ਰੇਰੀਜ਼ ਜਾਂ ਅਭਿਸ਼ਕਤੀ ਨਹੀਂ।
- ਹਰ ਇਕ ਸੈਮਪਲ ਆਪਣੀਆਂ ਆਪਣੀਆਂ ਨਿਰਭਰਤਾਵਾਂ ਲਗਾਉਣ ਤੋਂ ਬਾਅਦ ਸੁਤੰਤਰ ਰੂਪ ਵਿੱਚ ਚੱਲਦਾ ਹੈ।
- API ਕੁੰਜੀਆਂ ਸਦਾ `"foundry-local"` ਤੇ ਸੈੱਟ ਕੀਤੀਆਂ ਜਾਂਦੀਆਂ ਹਨ — Foundry Local ਇਸਨੂੰ ਪਲੇਸਹੋਲਡਰ ਵਜੋਂ ਵਰਤਦਾ ਹੈ।
- ਬੇਸ URL `http://localhost:<port>/v1` ਹੁੰਦੇ ਹਨ — ਪੋਰਟ ਡਾਇਨਾਮਿਕ ਹੈ ਅਤੇ SDK ਰਾਹੀਂ ਰਨਟਾਈਮ 'ਤੇ ਖੋਜਿਆ ਜਾਂਦਾ ਹੈ (`manager.urls[0]` ਜੇ JS ਵਿੱਚ, `manager.endpoint` ਜੇ ਪਾਇਥਨ ਵਿੱਚ)।
- Foundry Local SDK ਸੇਵਾ ਸ਼ੁਰੂਆਤ ਅਤੇ ਏਂਡਪੌਇੰਟ ਖੋਜ ਦੀ ਸੰਭਾਲ ਕਰਦਾ ਹੈ; ਸਖਤ-ਕੋਡ ਪੋਰਟਾਂ ਦੀ ਥਾਂ SDK ਪੈਟਰਨ ਨੂੰ ਤਰਜੀਹ ਦਿਓ।

### ਪਾਇਥਨ
- `openai` SDK ਨਾਲ `OpenAI(base_url=..., api_key="not-required")` ਵਰਤੋ।
- SDK-ਸੰਚਾਲਿਤ ਸੇਵਾ ਲਾਈਫਸਾਈਕਲ ਲਈ `foundry_local` ਤੋਂ `FoundryLocalManager()` ਵਰਤੋ।
- ਸਟ੍ਰੀਮਿੰਗ: `for chunk in stream:` ਨਾਲ ਸਟ੍ਰੀਮ ਆਬਜੈਕਟ ਨੂੰ ਇਟਰੇਟ ਕਰੋ।
- ਨਮੂਨਾ ਫਾਇਲਾਂ ਵਿੱਚ ਕਿਸੇ ਕਿਸਮ ਦੀ ਐਨੋਟੇਸ਼ਨ ਨਾ ਕਰੋ (ਵਰਕਸ਼ਾਪ ਸਿੱਖਣ ਵਾਲਿਆਂ ਲਈ ਨਮੂਨੇ ਸੰਖੇਪ ਰੱਖੋ)।

### ਜਾਵਾਸਕ੍ਰਿਪਟ
- ES ਮੋਡੀਊਲ ਸੰਟੈਕਸ: `import ... from "..."`।
- `"openai"` ਤੋਂ `OpenAI` ਅਤੇ `"foundry-local-sdk"` ਤੋਂ `FoundryLocalManager` ਵਰਤੋ।
- SDK ਸ਼ੁਰੂਆਤ ਦਾ ਪੈਟਰਨ: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`।
- ਸਟ੍ਰੀਮਿੰਗ: `for await (const chunk of stream)`।
- ਟਾਪ-ਲੈਵਲ `await` ਸਾਰਾ ਵਰਤਿਆ ਗਿਆ ਹੈ।

### C#
- Nullable.enabled, implicit usings, .NET 9 ਵਰਤੋ।
- SDK-ਸੰਚਾਲਿਤ ਜੀਵਨਚੱਕਰ ਲਈ `FoundryLocalManager.StartServiceAsync()` ਵਰਤੋ।
- ਸਟ੍ਰੀਮਿੰਗ: `CompleteChatStreaming()` ਵਿੱਚ `foreach (var update in completionUpdates)` ਵਰਤੋ।
- ਮੁੱਖ `csharp/Program.cs` CLI ਰਾਊਟਰ ਹੈ ਜੋ ਸਥਿਰ `RunAsync()` ਮੈਥਡਾਂ ਨੂੰ ਡਿਸਪੈਚ ਕਰਦਾ ਹੈ।

### ਟੂਲ ਕਾਲਿੰਗ
- ਸਿਰਫ਼ ਕੁਝ ਮਾਡਲ ਟੂਲ ਕਾਲਿੰਗ ਦਾ ਸਮਰਥਨ ਕਰਦੇ ਹਨ: **Qwen 2.5** ਪਰਿਵਾਰ (`qwen2.5-*`) ਅਤੇ **Phi-4-mini** (`phi-4-mini`)।
- ਟੂਲ ਸਕੀਮਾਂ OpenAI ਫੰਕਸ਼ਨ ਕਾਲਿੰਗ JSON ਫਾਰਮੈਟ ਨੂੰ ਫਾਲੋ ਕਰਦੀਆਂ ਹਨ (`type: "function"`, `function.name`, `function.description`, `function.parameters`)।
- ਗੱਲਬਾਤ ਮਲਟੀ-ਟਰਨ ਪੈਟਰਨ ਵਰਤਦੀ ਹੈ: ਯੂਜ਼ਰ → ਸਹਾਇਕ (tool_calls) → ਟੂਲ (ਨਤੀਜੇ) → ਸਹਾਇਕ (ਅੰਤਿਮ ਜਵਾਬ)।
- ਟੂਲ ਨਤੀਜੇ ਸੁਨੇਹਿਆਂ ਵਿੱਚ `tool_call_id` ਮਾਡਲ ਦੇ ਟੂਲ ਕਾਲ ਦੇ `id` ਨਾਲ ਮੇਲ ਖਾਣਾ ਚਾਹੀਦਾ ਹੈ।
- ਪਾਇਥਨ ਸਿੱਧਾ OpenAI SDK ਵਰਤਦਾ ਹੈ; ਜਾਵਾਸਕ੍ਰਿਪਟ SDK ਦਾ ਪ੍ਰਾਚੀਨ `ChatClient` ਵਰਤਦਾ ਹੈ (`model.createChatClient()`); C# OpenAI SDK ਨਾਲ `ChatTool.CreateFunctionTool()` ਵਰਤਦਾ ਹੈ।

### ChatClient (ਨੇਟੀਵ SDK ਕਲਾਇੰਟ)
- ਜਾਵਾਸਕ੍ਰਿਪਟ: `model.createChatClient()` ਇੱਕ `ChatClient` ਵਾਪਸ ਕਰਦਾ ਹੈ ਜਿਸਦੇ ਕੋਲ `completeChat(messages, tools?)` ਅਤੇ `completeStreamingChat(messages, callback)` ਹੁੰਦੇ ਹਨ।
- C#: `model.GetChatClientAsync()` ਇੱਕ ਸਧਾਰਨ `ChatClient` ਵਾਪਸ ਕਰਦਾ ਹੈ ਜੋ ਬਿਨਾਂ OpenAI NuGet ਪੈਕੇਜ ਇੰਪੋਰਟ ਕੀਤੇ ਵਰਤਿਆ ਜਾ ਸਕਦਾ ਹੈ।
- ਪਾਇਥਨ ਕੋਲ ਕੋਈ ਨੇਟੀਵ ChatClient ਨਹੀਂ — OpenAI SDK ਵਰਤੋਂ `manager.endpoint` ਅਤੇ `manager.api_key` ਨਾਲ ਕਰੋ।
- **ਮਹੱਤਵਪੂਰਨ:** ਜਾਵਾਸਕ੍ਰਿਪਟ `completeStreamingChat` ਵਿੱਚ **ਕਾਲਬੈਕ ਪੈਟਰਨ** ਵਰਤਿਆ ਜਾਂਦਾ ਹੈ, ਨਾਂ ਕਿ ਐਸਿੰਕ ਇਟਰੇਸ਼ਨ।

### ਤਰਕਸ਼ੀਲ ਮਾਡਲ
- `phi-4-mini-reasoning` ਆਪਣੇ ਵਿਚਾਰਾਂ ਨੂੰ ਅੰਤਿਮ ਜਵਾਬ ਤੋਂ ਪਹਿਲਾਂ `<think>...</think>` ਟੈਗਾਂ ਵਿੱਚ ਢਕਦਾ ਹੈ।
- ਜੇ ਲੋੜ ਹੋਵੇ ਤਾਂ ਜਵਾਬ ਤੋਂ ਤਰਕਸ਼ੀਲ ਭਾਗ ਨੂੰ ਵੱਖ ਕਰਨਾ।

## ਲੈਬ ਗਾਈਡਜ਼

ਲੈਬ ਫਾਇਲਾਂ `labs/` ਵਿੱਚ ਮਾਰਕਡਾਊਨ ਦੇ ਰੂਪ ਵਿੱਚ ਹਨ। ਇਹ ਇੱਕ ਲਗਾਤਾਰ ਸੰਰਚਨਾ ਦਾ ਅਨੁਸਰਣ ਕਰਦੀਆਂ ਹਨ:
- ਲੋਗੋ ਹੈਡਰ ਇਮੇਜ
- ਸਿਰਲੇਖ ਅਤੇ ਟੀਚਾ ਕਾਲਆਉਟ
- ਓਵਰਵਿਊ, ਸਿਖਣ ਦੇ ਉਦੇਸ਼, ਪੂਰਵ-ਆਵਸ਼ਕਤਾਵਾਂ
- ਧਾਰਨਾ ਸਮਝਾਉਣ ਵਾਲੇ ਭਾਗ ਚਿੱਤਰਾਂ ਸਮੇਤ
- ਨੰਬਰਵਾਰ ਅਭਿਆਸ ਕੋਡ ਬਲਾਕਾਂ ਅਤੇ ਉਮੀਦ ਕੀਤੀ ਅਉਟਪੁੱਟ ਦੇ ਨਾਲ
- ਸਾਰਣੀ, ਮੁੱਖ ਸਿੱਖਣ ਵਾਲੀਆਂ ਗੱਲਾਂ, ਅਗਲੀ ਪੜ੍ਹਾਈ
- ਅਗਲੇ ਭਾਗ ਲਈ ਨੈਵੀਗੇਸ਼ਨ ਲਿੰਕ

ਲੇਬ ਕਾਂਟੈਂਟ ਨੂੰ ਸੰਪਾਦਿਤ ਕਰਦਿਆਂ:
- ਮੌਜੂਦਾ ਮਾਰਕਡਾਊਨ ਫਾਰਮੇਟਿੰਗ ਅੰਦਾਜ਼ਾ ਅਤੇ ਭਾਗ ਮੁੜ ਬਣਤਰ ਨੂੰ ਬਰਕਰਾਰ ਰੱਖੋ।
- ਕੋਡ ਬਲਾਕਾਂ ਨੂੰ ਭਾਸ਼ਾ ਬਰਾਬਰ ਦਰਜ ਕਰੋ (`python`, `javascript`, `csharp`, `bash`, `powershell`)।
- ਜੇ OS ਅਹੰਕਾਰ ਵਿੱਚ ਕਮਾਂਡ ਦੀ ਵਰਤਾ ਈਹਦੇ ਦਾ ਫਰਕ ਪੈਂਦਾ ਹੈ ਤਾਂ ਬੈਸ਼ ਅਤੇ ਪੁਆਰਸ਼ੈਲ ਦੋਵੇਂ ਵਰਜ਼ਨ ਦਿਓ।
- ਕਾਲਆਉਟ ਸ਼ੈਲੀਆਂ ਲਈ `> **Note:**`, `> **Tip:**`, ਅਤੇ `> **Troubleshooting:**` ਵਰਤੋ।
- ਟੇਬਲਾਂ ਲਈ `| ਮਥਾ | ਮਥਾ |` ਪਾਈਪ ਫਾਰਮੈਟ ਵਰਤੋ।

## ਬਿਲਡ ਅਤੇ ਟੈਸਟ ਕਮਾਂਡਸ

| ਕਾਰਵਾਈ | ਕਮਾਂਡ |
|--------|---------|
| **ਪਾਇਥਨ ਸੈਮਪਲ** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS ਸੈਮਪਲ** | `cd javascript && npm install && node <script>.mjs` |
| **C# ਸੈਮਪਲ** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (ਵੈੱਬ)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (ਵੈੱਬ)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **ਚਿੱਤਰ ਬਣਾਓ** | `npx mmdc -i <input>.mmd -o <output>.svg` (ਰੂਟ ਲਈ `npm install` ਦੀ ਲੋੜ ਹੈ) |

## ਬਾਹਰੀ ਨਿਰਭਰਤਾਵਾਂ

- **Foundry Local CLI** ਵਿਕਾਸਕਾਰ ਦੀ ਮਸ਼ੀਨ 'ਤੇ ਇੰਸਟਾਲ ਹੋਣਾ ਲਾਜ਼ਮੀ ਹੈ (`winget install Microsoft.FoundryLocal` ਜਾਂ `brew install foundrylocal`)।
- **Foundry Local ਸੇਵਾ** ਲੋਕਲ ਚੱਲਦੀ ਹੈ ਅਤੇ ਡਾਇਨਾਮਿਕ ਪੋਰਟ 'ਤੇ OpenAI-ਕੰਪੈਟਿਬਲ REST API ਪ੍ਰਦਾਨ ਕਰਦੀ ਹੈ।
- ਕਿਸੇ ਵੀ ਨਮੂਨੇ ਨੂੰ ਚਲਾਉਣ ਲਈ ਕੋਈ ਕਲਾਉਡ ਸੇਵਾ, API ਕੁੰਜੀਆਂ ਜਾਂ Azure سبسکرਿਪਸ਼ਨਜ਼ ਦੀ ਲੋੜ ਨਹੀਂ ਹੈ।
- ਹਿੱਸਾ 10 (ਕਸਟਮ ਮਾਡਲ) ਲਈ ਵਾਧੂ `onnxruntime-genai` ਦੀ ਲੋੜ ਅਤੇ Hugging Face ਤੋਂ ਮਾਡਲ ਵਜ਼ਨ ਡਾਊਨਲੋਡ ਹੋਣੇ ਚਾਹੀਦੇ ਹਨ।

## ਫਾਇਲਾਂ ਜੋ ਕਮਿਟ ਨਹੀਂ ਕੀਤੀਆਂ ਜਾਣੀਆਂ ਚਾਹੀਦੀਆਂ

`.gitignore` ਅਕਸਰ ਹੇਠਾਂ ਦਿੱਤੀਆਂ ਚੀਜ਼ਾਂ ਨੂੰ ਬਾਹਰ ਰੱਖਦਾ ਹੈ (ਅਤੇ ਜ਼ਿਆਦਾ ਦੇ ਲਈ ਕਰਦਾ ਹੈ):
- `.venv/` — ਪਾਇਥਨ ਵਰਚੁਅਲ ਐਨਵਾਇਰਨਮੈਂਟ
- `node_modules/` — npm ਨਿਰਭਰਤਾਵਾਂ
- `models/` — ਕੰਪਾਇਲਡ ONNX ਮਾਡਲ ਆਉਟਪੁੱਟ (ਵੱਡੇ ਬਾਈਨਰੀ ਫਾਇਲ, ਹਿੱਸਾ 10 ਦੁਆਰਾ ਬਣਾਏ ਗਏ)
- `cache_dir/` — Hugging Face ਮਾਡਲ ਡਾਊਨਲੋਡ ਕੈਸ਼
- `.olive-cache/` — Microsoft Olive ਵਰਕਿੰਗ ਡਾਇਰੈਕਟਰੀ
- `samples/audio/*.wav` — ਜਨਰੇਟ ਕੀਤੇ ਆਡੀਓ ਸੈਮਪਲ (ਦੁਬਾਰਾ `python samples/audio/generate_samples.py` ਨਾਲ ਬਣਾਏ ਜਾਂਦੇ ਹਨ)
- ਸਧਾਰਣ ਪਾਇਥਨ ਬਿਲਡ ਆਰਟੀਫੈਕਟ (`__pycache__/`, `*.egg-info/`, `dist/`, ਆਦਿ)

## ਲਾਇਸੰਸ

MIT — ਵੇਖੋ `LICENSE`।

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**ਅਸਵੀਕਾਰੋਪਤਰ**:
ਇਹ ਦਸਤਾਵੇਜ਼ ਏਆਈ ਅਨੁਵਾਦ ਸੇਵਾ [Co-op Translator](https://github.com/Azure/co-op-translator) ਦੀ ਵਰਤੋਂ ਨਾਲ ਅਨੁਵਾਦ ਕੀਤਾ ਗਿਆ ਹੈ। ਜਦੋਂ ਕਿ ਅਸੀਂ ਸਹੀਤਾ ਲਈ ਯਤਨ ਕਰਦੇ ਹਾਂ, ਕਿਰਪਾ ਕਰਕੇ ਧਿਆਨ ਰੱਖੋ ਕਿ ਸਵੈਚਾਲਿਤ ਅਨੁਵਾਦਾਂ ਵਿੱਚ ਗਲਤੀਆਂ ਜਾਂ ਅਨੁਚਿਤਤਾ ਹੋ ਸਕਦੀ ਹੈ। ਮੂਲ ਦਸਤਾਵੇਜ਼ ਆਪਣੇ ਮੂਲ ਭਾਸ਼ਾ ਵਿੱਚ ਪ੍ਰਮਾਣਿਕ ਸਰੋਤ ਮੰਨਿਆ ਜਾਣਾ ਚਾਹੀਦਾ ਹੈ। ਗੰਭੀਰ ਜਾਣਕਾਰੀ ਲਈ, ਪੇਸ਼ੇਵਰ ਮਨੁੱਖੀ ਅਨੁਵਾਦ ਦੀ ਸਿਫਾਰਸ਼ ਕੀਤੀ ਜਾਂਦੀ ਹੈ। ਅਸੀਂ ਇਸ ਅਨੁਵਾਦ ਦੀ ਵਰਤੋਂ ਤੋਂ ਉਤਪੰਨ ਕਿਸੇ ਵੀ ਗਲਤਫਹਮੀ ਜਾਂ ਗਲਤ ਵਿਆਖਿਆ ਲਈ ਜ਼ਿੰਮੇਵਾਰ ਨਹੀਂ ਹਾਂ।
<!-- CO-OP TRANSLATOR DISCLAIMER END -->