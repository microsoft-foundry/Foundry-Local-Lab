# ಕೋಡಿಂಗ್ ಏಜೆಂಟ್ ಸೂಚನೆಗಳು

ಈ ಕಡತವು ಈ ಸಂಗ್ರಹಣೆಯಲ್ಲಿ (GitHub Copilot, Copilot Workspace, Codex ಇತ್ಯಾದಿ) ಕಾರ್ಯನಿರ್ವಹಿಸುವ AI ಕೋಡಿಂಗ್ ಏಜೆಂಟ್‌ಗಳಿಗೆ ಪ್ರContextನ ನೀಡುತ್ತದೆ.

## ಯೋಜನೆ ಪ್ರವೀಕ್ಷೆ

ಇದು [Foundry Local](https://foundrylocal.ai) ನೊಂದಿಗೆ AI ಅಪ್ಲಿಕೇಶನ್‌ಗಳನ್ನು ನಿರ್ಮಿಸುವ **ಪ್ರಾಯೋಗಿಕ ಕಾರ್ಯಾಗಾರ** — ಒಂದು ಲೈಟ್ವೆಟ್ ರನ್‌ಟೈಂ, ಇದು ಸಂಪೂರ್ಣವಾಗಿ ಸಾಧನದ ಮೇಲೆ OpenAI-ಸಂಯೋಜಿತ API ಮೂಲಕ ಭಾಷಾ ಮಾದರಿಗಳನ್ನು ಡೌನ್ಲೋಡ್, ನಿರ್ವಹಣೆ ಮತ್ತು ಸೇವೆ ನೀಡುತ್ತದೆ. ಈ ಕಾರ್ಯಾಗಾರದಲ್ಲಿ ಹಂತರಾತ್ ಲ್ಯಾಬ್ ಗೈಡ್‌ಗಳು ಮತ್ತು ಪೈಥಾನ್, ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ ಮತ್ತು C# ನಲ್ಲಿ ಚಾಲನೆಯಲ್ಲುವ ಕೋಡ್ ಉದಾಹರಣೆಗಳನ್ನು ಒಳಗೊಂಡಿದೆ.

## ಸಂಗ್ರಹಣೆ ರಚನೆ

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

## ಭಾಷೆ ಮತ್ತು ഫ്രೇಮ್‌ವರ್ಕ್ ವಿವರಗಳು

### ಪೈಥಾನ್
- **ಸ್ಥಳ:** `python/`, `zava-creative-writer-local/src/api/`
- **ನಿರಭವ:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **ಪ್ರಮುಖ ಪ್ಯಾಕೇಜ್‌ಗಳು:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **ಕನಿಷ್ಠ ಆವೃತ್ತಿ:** Python 3.9+
- **ಚಾಲನೆ:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್
- **ಸ್ಥಳ:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **ನಿರಭವ:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **ಪ್ರಮುಖ ಪ್ಯಾಕೇಜ್‌ಗಳು:** `foundry-local-sdk`, `openai`
- **ಮಾಡ್ಯೂಲ್ ಸಿಸ್ಟಂ:** ES ಮಾಡ್ಯೂಲ್ಸ್ (`.mjs` ಫೈಲ್‌ಗಳು, `"type": "module"`)
- **ಕನಿಷ್ಠ ಆವೃತ್ತಿ:** Node.js 18+
- **ಚಾಲನೆ:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **ಸ್ಥಳ:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **ಯೋಜನೆ ಫೈಲ್‌ಗಳು:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **ಪ್ರಮುಖ ಪ್ಯಾಕೇಜ್‌ಗಳು:** `Microsoft.AI.Foundry.Local` (ನೋನ್-ವಿಂಡೋಸ್), `Microsoft.AI.Foundry.Local.WinML` (ವಿಂಡೋಸ್ — QNN EP ಜೊತೆಗೆ ಸಪರ್ ಸೆಟ್), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **ಲಕ್ಷ್ಯ:** .NET 9.0 (ಶರತ್ತು ಎಫ್‌ಎಮ್: ವಿಂಡೋಸ್‌ನಲ್ಲಿ `net9.0-windows10.0.26100`, ಬಾಕಿ ಸ್ಥಳಗಳಲ್ಲಿ `net9.0`)
- **ಚಾಲನೆ:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## ಕೋಡಿಂಗ್ ಪದ್ಧತಿಗಳು

### ಸಾಮಾನ್ಯ
- ಎಲ್ಲಾ ಕೋಡ್ ಉದಾಹರಣೆಗಳು **ಸ್ವತಂತ್ರ ಏಕ-ಫೈಲ್ ಉದಾಹರಣೆಗಳು** — ಹಂಚಿಕೊಳ್ಳುವ ಉಪಯುಕ್ತ ಗ್ರಂಥಾಲಯಗಳು ಅಥವಾ ಅವಲಂಬನೆಗಳಿಲ್ಲ.
- ಪ್ರತಿಯೊಂದು ಉದಾಹರಣೆಯು ಅದರ ಸ್ವಂತ ಅವಲಂಬನೆಗಳನ್ನು ಸ್ಥಾಪಿಸಿದ ನಂತರ ಸ್ವತಂತ್ರವಾಗಿ ಚಾಲನೆಯಲ್ಲಿರುತ್ತದೆ.
- API ಕೀಲಿಗಳು ಯಾವಾಗಲೂ `"foundry-local"` ಆಗಿ ಹೊಂದಿಸಲಾಗುತ್ತದೆ — Foundry Local ಇದನ್ನು ಪ್ಲೇಸ್‌ಹೋಲ್ಡರ್ ಆಗಿ ಬಳಸುತ್ತದೆ.
- ಬೇಸ್ URL ಗಳು `http://localhost:<port>/v1` ಅನ್ನು ಬಳಸುತ್ತವೆ — ಪೋರ್ಟ್ ಡೈನಾಮಿಕ್ ಆಗಿದ್ದು SDK ಮೂಲಕ ರನ್‌ಟೈಂನಲ್ಲಿ ಕಂಡುಹಿಡಿಕೆಯಾಗುತ್ತದೆ (`manager.urls[0]` ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್‌ನಲ್ಲಿ, `manager.endpoint` ಪೈಥಾನ್‌ನಲ್ಲಿ).
- Foundry Local SDK ಸೇವೆಯ ಪ್ರಾರಂಭ ಮತ್ತು ಎಂಡ್‌ಪಾಯಿಂಟ್ ಪತ್ತೆಯನ್ನು ನಿಭಾಯಿಸುತ್ತದೆ; ಸಹಾಯಕರಾಗಿ SDK ಮಾದರಿಯನ್ನು ಹಾರ್ಡ್-ಕೊಡ್ ಪೋರ್ಟ್‌ಗಳಿಗಿಂತ ಮೇಲುಗೈಕೊಡಿ.

### ಪೈಥಾನ್
- `openai` SDK ಅನ್ನು `OpenAI(base_url=..., api_key="not-required")` ಜೊತೆಗೆ ಬಳಸಿರಿ.
- SDK ನಿರ್ವಹಿಸಲಾದ ಸೇವಾ ಜೀವನಚಕ್ರಕ್ಕಾಗಿ `foundry_local` ನಿಂದ `FoundryLocalManager()` ಬಳಸಿ.
- ಸ್ಟ್ರೀಮಿಂಗ್: `stream` ವಸ್ತುವನ್ನು `for chunk in stream:` ಮೂಲಕ ಪುನರಾವರ್ತಿಸಿ.
- ನಮೂನೆ ಫೈಲ್‌ಗಳಲ್ಲಿ ಪ್ರಕಾರ ಸೂಚನೆಗಳು ಇಲ್ಲ (ಕಾರಣ ಕಾರ್ಯಾಗಾರ ಕಲಿಕೆಗೆ ಉದಾಹರಣೆಗಳನ್ನು ಸಂಕ್ಷಿಪ್ರ ಮಾಡಲು).

### ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್
- ES ಮಾಡ್ಯೂಲ್_Syntax: `import ... from "..."`.
- `"openai"` ನಿಂದ `OpenAI` ಮತ್ತು `"foundry-local-sdk"` ನಿಂದ `FoundryLocalManager` ಬಳಸಿ.
- SDK ಪ್ರಾರಂಭ ಮಾದರಿ: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- ಸ್ಟ್ರೀಮಿಂಗ್: `for await (const chunk of stream)`.
- ಮೇಲ್ಭಾಗದ `await` ಪ್ರತಿ ಸ್ಥಳದಲ್ಲಿಯೂ ಬಳಸಲಾಗಿದೆ.

### C#
- Nullable ಸಕ್ರಿಯ, ನಿಶ್ಚಿತ ಉಪಯೋಗಗಳು, .NET 9.
- SDK ನಿರ್ವಹಿಸಲಾದ ಜೀವನಚಕ್ರಕ್ಕಾಗಿ `FoundryLocalManager.StartServiceAsync()` ಬಳಸಿ.
- ಸ್ಟ್ರೀಮಿಂಗ್: `CompleteChatStreaming()` ಜೊತೆಗೆ `foreach (var update in completionUpdates)`.
- ಮುಖ್ಯ `csharp/Program.cs` ಒಂದು CLI ರೋಟರ್ ಆಗಿದ್ದು ಸ್ಥಿರ `RunAsync()` ವಿಧಾನಗಳಿಗೆ ವಿನ್ಯಾಸವನ್ನು ಹಂಚುತ್ತದೆ.

### ಉಪಕರಣ ಕರೆಯುವಿಕೆ
- ಸ್ಥಾನಮಾನ ಮಾಡ ಮತ್ತು ಉಪಕರಣ ಕರೆಯಲು ಮಾತ್ರ ಕೆಲವು ಮಾದರಿಗಳು ಅವಕಾಶ ನೀಡುತ್ತವೆ: **Qwen 2.5** ಕುಟುಂಬ (`qwen2.5-*`) ಮತ್ತು **Phi-4-mini** (`phi-4-mini`).
- ಉಪಕರಣ schemas OpenAI ಕಾರ್ಯ-ಕರೆ JSON ಸ್ವರೂಪವನ್ನು ಅನುಸರಿಸುತ್ತವೆ (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- ಸಂಭಾಷಣೆ ಬಹು-ತಿರುವಿನ ಮಾದರಿಯನ್ನು ಅನುಸರಿಸುತ್ತದೆ: ಬಳಕೆದಾರ → ಸಹಾಯಕ (tool_calls) → ಉಪಕರಣ (ಫಲಿತಾಂಶ) → ಸಹಾಯಕ (ಅಂತಿಮ ಉತ್ತರ).
- ಉಪಕರಣ ಫಲಿತಾಂಶ ಸಂದೇಶಗಳಲ್ಲಿ `tool_call_id` ಮಾದರಿಯ ಉಪಕರಣ ಕರೆಯ ಶೀರ್ಷಿಕೆಯ `id` ಗೆ ಹೊಂದಾಣಿಕೆ ಮಾಡಬೇಕು.
- ಪೈಥಾನ್ ನೇರವಾಗಿ OpenAI SDK ಬಳಸಿ; ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ SDK ನ ಸ್ಥಳೀಕೃತ `ChatClient` (`model.createChatClient()`); C# OpenAI SDK ಜೊತೆಗೆ `ChatTool.CreateFunctionTool()` ಬಳಸುತ್ತದೆ.

### ChatClient (ಸ್ಥಳೀಯ SDK ಕ್ಲೈಂಟ್)
- ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್: `model.createChatClient()` ಒಂದು `ChatClient` ಅನ್ನು ಹಿಂತಿರುಗಿಸುತ್ತದೆ, ಇದರಲ್ಲಿ `completeChat(messages, tools?)` ಮತ್ತು `completeStreamingChat(messages, callback)` ಇರುವವು.
- C#: `model.GetChatClientAsync()` ಒಂದು ಸರ್ವಜನಿಕ `ChatClient` ಅನ್ನು ಹಿಂತಿರುಗಿಸುತ್ತದೆ, OpenAI NuGet ಪ್ಯಾಕೇಜ್ ಆಮದು ಮಾಡಿಕೊಳ್ಳದೆ ಬಳಕೆ ಮಾಡಬಹುದಾಗಿದೆ.
- ಪೈಥಾನ್‌ಗೆ ಸ್ಥಳೀಯ ChatClient ಇಲ್ಲ — OpenAI SDK ಅನ್ನು `manager.endpoint` ಮತ್ತು `manager.api_key` ಜೊತೆ ಬಳಸಿ.
- **ಮುಖ್ಯ:** ಜಾವಾಸ್ಕ್ರಿಪ್ಟ್ `completeStreamingChat` ಒಂದು **ಕಾಲ್‌ಬ್ಯಾಕ್ ಮಾದರಿ** ಬಳಸುತ್ತದೆ, ಐಡಿಟರ್ ಆಗಿಲ್ಲ.

### ವಿವರಣೆ ಮಾದರಿಗಳು
- `phi-4-mini-reasoning` ತನ್ನ ಚಿಂತನೆಗಳನ್ನು ಅಂತಿಮ ಉತ್ತರಕ್ಕೂ ಮುಂಚೆ `<think>...</think>` ಟ್ಯಾಗ್‌ಗಳೊಳಗೆ ಮುಚ್ಚುತ್ತದೆ.
- ಆ ಟ್ಯಾಗ್‌ಗಳನ್ನು ವಿಶ್ಲೇಷಿಸಿ ವಿಚಾರಚಿಂತನೆ ಮತ್ತು ಉತ್ತರವನ್ನು ಬೇರ್ಪಡಿಸಬಹುದು.

## ಲ್ಯಾಬ್ ಮಾರ್ಗದರ್ಶಿಗಳು

ಲ್ಯಾಬ್ ಫೈಲ್‌ಗಳು `labs/` ನಲ್ಲಿ ಮಾರ್ಕ್‌ಡೌನ್ ಆಗಿವೆ. ಅವು ಸ್ಥಿರವಾದ ರಚನೆ ಅನುಸರಿಸುತ್ತವೆ:
- ಲೋಗೋ ಶಿರೋನಾಮ ಚಿತ್ರ
- ಶೀರ್ಷಿಕೆ ಮತ್ತು ಗುರಿ ಕರೆ
- ಅವಲೋಕನ, ಕಲಿಕಾ ಉದ್ದಿಷ್ಟಗಳು, ಪೂರ್ವಾಪೇಕ್ಷೆಗಳು
- ಕಲಿಕಾ ಕಲ್ಪನೆ ವಿಭಾಗಗಳು ಚಿತ್ರಗಳೊಂದಿಗೆ
- ಕ್ರಮಾಂಕಿತ ವ್ಯಾಯಾಮಗಳು ಕೋಡ್ ಬ್ಲಾಕ್ ಮತ್ತು ನಿರೀಕ್ಷಿತ ಔಟ್‌ಪುಟ್
- ಸಾರಾಂಶ ಟೇಬಲ್, ಮುಖ್ಯ ಪಾಠಗಳು, ಹೆಚ್ಚಿನ ಓದು
- ಮುಂದಿನ ಭಾಗಕ್ಕೆ ನ್ಯಾವಿಗೇಶನ್ ಲಿಂಕ್

ಲ್ಯಾಬ್ ಕಂಟೆಂಟ್ ಸಂಪಾದಿಸುವಾಗ:
- ಇದ್ದ ಮಾರ್ಕ್‌ಡೌನ್ ಫಾರ್ಮ್ಯಾಟಿಂಗ್ ಶೈಲಿ ಮತ್ತು ವಿಭಾಗ ಹಿರarchy ಉಳಿಸಿ.
- ಕೋಡ್ ಬ್ಲಾಕ್‌ಗಳು ಭಾಷೆಯ ನಾಮಕರಣವನ್ನು ಹೊಂದಿರಬೇಕು (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- ಶೆಲ್ ಕಮಾಂಡ್‌ಗಳಿಗಾಗಿ OS ಅಗತ್ಯವಿರುವಾಗ ಎರಡೂ bash ಮತ್ತು PowerShell ಪರ್ಯಾಯಗಳನ್ನು ನೀಡಿ.
- `> **Note:**`, `> **Tip:**`, ಮತ್ತು `> **Troubleshooting:**` ಕರೆಗೋಳು ಶೈಲಿಗಳನ್ನು ಬಳಸಿರಿ.
- ಟೇಬಲ್‌ಗಳು `| Header | Header |` ಪೈಪ್ ಸ್ವರೂಪ ಬಳಸುತ್ತವೆ.

## ನಿರ್ಮಾಣ ಮತ್ತು ಪರೀಕ್ಷೆ ಕಮಾಂಡ್‌ಗಳು

| ಕ್ರಿಯೆ | ಕಮಾಂಡ್ |
|--------|---------|
| **Python ಉದಾಹರಣೆಗಳು** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS ಉದಾಹರಣೆಗಳು** | `cd javascript && npm install && node <script>.mjs` |
| **C# ಉದಾಹರಣೆಗಳು** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (ವೆಬ್)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (ವೆಬ್)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **ಚಿತ್ರರಚನೆ ರಚನೆ** | `npx mmdc -i <input>.mmd -o <output>.svg` (root ನಲ್ಲಿ `npm install` ಅಗತ್ಯ) |

## ಹೊರಗಿನ ಅವಲಂಬನೆಗಳು

- **Foundry Local CLI** ಅನ್ನು ಡೆವಲಪರ್ ಯಂತ್ರದಲ್ಲಿ ಸ್ಥಾಪಿಸಿರಬೇಕು (`winget install Microsoft.FoundryLocal` ಅಥವಾ `brew install foundrylocal`).
- **Foundry Local ಸೇವೆ** ಸ್ಥಳೀಯವಾಗಿ ಕಾರ್ಯನಿರ್ವಹಿಸಿ OpenAI-ಸಂಯೋಜಿತ REST API ನ ಡೈನಾಮಿಕ್ ಪೋರ್ಟ್ ಅನ್ನು ಬಹಿರಂಗಪಡಿಸುತ್ತದೆ.
- ಯಾವುದೇ ಮಾದರಿ ಚಾಲನೆ ಮಾಡಲು ಮೆಘ ಸೇವೆಗಳು, API ಕೀಿಗಳು ಅಥವಾ Azure ಸದಸ್ಯತೆಗಳ ಅಗತ್ಯವಿಲ್ಲ.
- ಭಾಗ 10 (ಕಸ್ಟಮ್ ಮಾದರಿಗಳು) ಹೆಚ್ಚಾಗಿ `onnxruntime-genai` ಮತ್ತು Hugging Face ನಿಂದ ಮಾದರಿ ತೂಕಗಳನ್ನು ಡೌನ್ಲೋಡ್ ಮಾಡುವುದು ಬೇಕಾಗುತ್ತದೆ.

## ಕಡತಗಳು ಸಂಗ್ರಹಿಸುವುದಕ್ಕೆ ಅನುಕೂಲವಿಲ್ಲ

`.gitignore` ಹೆಚ್ಚಿನ ಭಾಗ ಕ್ಕೆ ಹೊರತುಪಡಿಸುತ್ತದೆ (ಮತ್ತು ಬಹಳಷ್ಟು ಮಾಡುತ್ತದೆ):
- `.venv/` — ಪೈಥಾನ್ ವರ್ಚ್ಯುಯಲ್ ಪರಿಸರ
- `node_modules/` — npm ಅವಲಂಬನೆಗಳು
- `models/` — ಸಂಯೋಜಿತ ONNX ಮಾದರಿ ಔಟ್ಪುಟ್ (ದೊಡ್ಡ ಬೈನರಿ ಫೈಲ್‌ಗಳು, ಭಾಗ 10 ರಿಂದ ರಚನೆ)
- `cache_dir/` — Hugging Face ಮಾದರಿ ಡೌನ್ಲೋಡ್ ಕ್ಯಾಶೆ
- `.olive-cache/` — Microsoft Olive ಕಾರ್ಯಾಚರಣೆ ಡೈರೆಕ್ಟರಿ
- `samples/audio/*.wav` — ತಯಾರಿಸಲಾದ ಧ್ವನಿ ಮಾದರಿಗಳು (`python samples/audio/generate_samples.py` ಮೂಲಕ ಪುನರ್ ರಚನೆ)
- ಮಾನ್ಯ ಪೈಥಾನ್ ನಿರ್ಮಾಣ ಆర్టಿಫ್ಯಾಕ್ಟ್‌ಗಳು (`__pycache__/`, `*.egg-info/`, `dist/` ಇತ್ಯಾದಿ)

## ಪರವಾನಗಿ

MIT — ನೋಡಲು `LICENSE`.