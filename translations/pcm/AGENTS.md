# Coding Agent Instructions

Dis file dey provide context for AI coding agents (GitHub Copilot, Copilot Workspace, Codex, etc.) wey dey work for dis repository.

## Project Overview

Dis na **hands-on workshop** for building AI applications wit [Foundry Local](https://foundrylocal.ai) — na lightweight runtime wey dey download, manage, and serve language models totally on-device via OpenAI-compatible API. Di workshop get step-by-step lab guides and runnable code samples for Python, JavaScript, and C#.

## Repository Structure

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

## Language & Framework Details

### Python
- **Location:** `python/`, `zava-creative-writer-local/src/api/`
- **Dependencies:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Key packages:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Min version:** Python 3.9+
- **Run:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Location:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Dependencies:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Key packages:** `foundry-local-sdk`, `openai`
- **Module system:** ES modules (`.mjs` files, `"type": "module"`)
- **Min version:** Node.js 18+
- **Run:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Location:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Project files:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Key packages:** `Microsoft.AI.Foundry.Local` (non-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset with QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Target:** .NET 9.0 (conditional TFM: `net9.0-windows10.0.26100` on Windows, `net9.0` elsewhere)
- **Run:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Coding Conventions

### General
- All code samples be **self-contained single-file examples** — no shared utility libraries or abstractions.
- Each sample dey run independently after e install its own dependencies.
- API keys always set to `"foundry-local"` — Foundry Local dey use dis as placeholder.
- Base URLs dey use `http://localhost:<port>/v1` — di port na dynamic and dem dey find am at runtime via di SDK (`manager.urls[0]` for JS, `manager.endpoint` for Python).
- The Foundry Local SDK de handle service startup and endpoint finding; na SDK patterns dem dey prefer instead to hard-code ports.

### Python
- Use `openai` SDK wit `OpenAI(base_url=..., api_key="not-required")`.
- Use `FoundryLocalManager()` from `foundry_local` for SDK-manage service lifecycle.
- Streaming: dey iterator over `stream` object with `for chunk in stream:`.
- No type annotations inside sample files (make samples brief for workshop learners).

### JavaScript
- ES module syntax: `import ... from "..."`.
- Use `OpenAI` from `"openai"` and `FoundryLocalManager` from `"foundry-local-sdk"`.
- SDK init pattern: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Top-level `await` dey used everywhere.

### C#
- Nullable enabled, implicit usings, .NET 9.
- Use `FoundryLocalManager.StartServiceAsync()` for SDK-manage lifecycle.
- Streaming: `CompleteChatStreaming()` with `foreach (var update in completionUpdates)`.
- The main `csharp/Program.cs` be CLI router wey dey dispatch to static `RunAsync()` methods.

### Tool Calling
- Only some models dey support tool calling: **Qwen 2.5** family (`qwen2.5-*`) and **Phi-4-mini** (`phi-4-mini`).
- Tool schemas follow OpenAI function-calling JSON format (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Di conversation dey take multi-turn pattern: user → assistant (tool_calls) → tool (results) → assistant (final answer).
- The `tool_call_id` for tool result messages must match di `id` from di model's tool call.
- Python use di OpenAI SDK directly; JavaScript use di SDK's native `ChatClient` (`model.createChatClient()`); C# use OpenAI SDK wit `ChatTool.CreateFunctionTool()`.

### ChatClient (Native SDK Client)
- JavaScript: `model.createChatClient()` dey return `ChatClient` with `completeChat(messages, tools?)` and `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` dey return standard `ChatClient` wey you fit use without importing OpenAI NuGet package.
- Python no get native ChatClient — use OpenAI SDK wit `manager.endpoint` and `manager.api_key`.
- **Important:** JavaScript `completeStreamingChat` dey use **callback pattern**, no be async iteration.

### Reasoning Models
- `phi-4-mini-reasoning` dey wrap its thinking in `<think>...</think>` tags before final answer.
- Parse di tags to separate reasoning from di answer if e necessary.

## Lab Guides

Lab files dey `labs/` as Markdown. Dem follow consistent structure:
- Logo header image
- Title and goal callout
- Overview, Learning Objectives, Prerequisites
- Concept explanation sections wit diagrams
- Numbered exercises with code blocks and expected output
- Summary table, Key Takeaways, Further Reading
- Navigation link to di next part

When you dey edit lab content:
- Make sure say you preserve di existing Markdown formatting style and section hierarchy.
- Code blocks must specify language (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Provide bash and PowerShell variants for shell commands if OS matter.
- Use `> **Note:**`, `> **Tip:**`, and `> **Troubleshooting:**` callout styles.
- Tables dey use di `| Header | Header |` pipe format.

## Build & Test Commands

| Action | Command |
|--------|---------|
| **Python samples** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS samples** | `cd javascript && npm install && node <script>.mjs` |
| **C# samples** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generate diagrams** | `npx mmdc -i <input>.mmd -o <output>.svg` (requires root `npm install`) |

## External Dependencies

- **Foundry Local CLI** must dey installed for developer machine (`winget install Microsoft.FoundryLocal` or `brew install foundrylocal`).
- **Foundry Local service** dey run locally and e dey expose OpenAI-compatible REST API on dynamic port.
- No cloud services, API keys, or Azure subscriptions dey required to run any sample.
- Part 10 (custom models) additionally need `onnxruntime-genai` and e dey download model weights from Hugging Face.

## Files That Should Not Be Committed

The `.gitignore` suppose exclude (and e dey exclude for most):
- `.venv/` — Python virtual environments
- `node_modules/` — npm dependencies
- `models/` — compiled ONNX model output (big binary files, generated by Part 10)
- `cache_dir/` — Hugging Face model download cache
- `.olive-cache/` — Microsoft Olive working directory
- `samples/audio/*.wav` — generated audio samples (dem regenerate am via `python samples/audio/generate_samples.py`)
- Standard Python build artifacts (`__pycache__/`, `*.egg-info/`, `dist/`, etc.)

## Licence

MIT — see `LICENSE`.