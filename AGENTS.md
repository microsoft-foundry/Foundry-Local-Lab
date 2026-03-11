# Coding Agent Instructions

This file provides context for AI coding agents (GitHub Copilot, Copilot Workspace, Codex, etc.) working in this repository.

## Project Overview

This is a **hands-on workshop** for building AI applications with [Foundry Local](https://foundrylocal.ai) — a lightweight runtime that downloads, manages, and serves language models entirely on-device via an OpenAI-compatible API. The workshop includes step-by-step lab guides and runnable code samples in Python, JavaScript, and C#.

## Repository Structure

```
├── labs/                              # Markdown lab guides (Parts 1–11)
├── python/                            # Python code samples (Parts 2–6, 8–9, 11)
├── javascript/                        # JavaScript/Node.js code samples (Parts 2–6, 8–9, 11)
├── csharp/                            # C# / .NET 9 code samples (Parts 2–6, 8–9, 11)
├── zava-creative-writer-local/        # Part 7 capstone app (Python/JS/C#)
│   └── src/
│       ├── api/                       # Python FastAPI multi-agent service
│       ├── javascript/                # Node.js CLI multi-agent app
│       └── csharp/                    # .NET console multi-agent app
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
- **Key packages:** `Microsoft.AI.Foundry.Local`, `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Target:** .NET 9.0
- **Run:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Coding Conventions

### General
- All code samples are **self-contained single-file examples** — no shared utility libraries or abstractions.
- Each sample runs independently after installing its own dependencies.
- API keys are always set to `"foundry-local"` — Foundry Local uses this as a placeholder.
- Base URLs use `http://localhost:<port>/v1` — the port is dynamic and discovered at runtime via the SDK (`manager.urls[0]` in JS, `manager.endpoint` in Python).
- The Foundry Local SDK handles service startup and endpoint discovery; prefer SDK patterns over hard-coded ports.

### Python
- Use `openai` SDK with `OpenAI(base_url=..., api_key="not-required")`.
- Use `FoundryLocalManager()` from `foundry_local` for SDK-managed service lifecycle.
- Streaming: iterate over `stream` object with `for chunk in stream:`.
- No type annotations in sample files (keep samples concise for workshop learners).

### JavaScript
- ES module syntax: `import ... from "..."`.
- Use `OpenAI` from `"openai"` and `FoundryLocalManager` from `"foundry-local-sdk"`.
- SDK init pattern: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Top-level `await` is used throughout.

### C#
- Nullable enabled, implicit usings, .NET 9.
- Use `FoundryLocalManager.StartServiceAsync()` for SDK-managed lifecycle.
- Streaming: `CompleteChatStreaming()` with `foreach (var update in completionUpdates)`.
- The main `csharp/Program.cs` is a CLI router dispatching to static `RunAsync()` methods.

### Tool Calling
- Only certain models support tool calling: **Qwen 2.5** family (`qwen2.5-*`) and **Phi-4-mini** (`phi-4-mini`).
- Tool schemas follow the OpenAI function-calling JSON format (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- The conversation uses a multi-turn pattern: user → assistant (tool_calls) → tool (results) → assistant (final answer).
- The `tool_call_id` in tool result messages must match the `id` from the model's tool call.
- Python uses the OpenAI SDK directly; JavaScript uses the SDK's native `ChatClient` (`model.createChatClient()`); C# uses the OpenAI SDK with `ChatTool.CreateFunctionTool()`.

### ChatClient (Native SDK Client)
- JavaScript: `model.createChatClient()` returns a `ChatClient` with `completeChat(messages, tools?)` and `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` returns a standard `ChatClient` that can be used without importing the OpenAI NuGet package.
- Python does not have a native ChatClient — use the OpenAI SDK with `manager.endpoint` and `manager.api_key`.
- **Important:** JavaScript `completeStreamingChat` uses a **callback pattern**, not async iteration.

### Reasoning Models
- `phi-4-mini-reasoning` wraps its thinking in `<think>...</think>` tags before the final answer.
- Parse the tags to separate reasoning from the answer when needed.

## Lab Guides

Lab files are in `labs/` as Markdown. They follow a consistent structure:
- Logo header image
- Title and goal callout
- Overview, Learning Objectives, Prerequisites
- Concept explanation sections with diagrams
- Numbered exercises with code blocks and expected output
- Summary table, Key Takeaways, Further Reading
- Navigation link to the next part

When editing lab content:
- Maintain the existing Markdown formatting style and section hierarchy.
- Code blocks must specify the language (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Provide both bash and PowerShell variants for shell commands where OS matters.
- Use `> **Note:**`, `> **Tip:**`, and `> **Troubleshooting:**` callout styles.
- Tables use the `| Header | Header |` pipe format.

## Build & Test Commands

| Action | Command |
|--------|---------|
| **Python samples** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS samples** | `cd javascript && npm install && node <script>.mjs` |
| **C# samples** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generate diagrams** | `npx mmdc -i <input>.mmd -o <output>.svg` (requires root `npm install`) |

## External Dependencies

- **Foundry Local CLI** must be installed on the developer's machine (`winget install Microsoft.FoundryLocal` or `brew install foundrylocal`).
- **Foundry Local service** runs locally and exposes an OpenAI-compatible REST API on a dynamic port.
- No cloud services, API keys, or Azure subscriptions are required to run any sample.
- Part 10 (custom models) additionally requires `onnxruntime-genai` and downloads model weights from Hugging Face.

## Files That Should Not Be Committed

The `.gitignore` should exclude (and does for most):
- `.venv/` — Python virtual environments
- `node_modules/` — npm dependencies
- `models/` — compiled ONNX model output (large binary files, generated by Part 10)
- `cache_dir/` — Hugging Face model download cache
- `.olive-cache/` — Microsoft Olive working directory
- `samples/audio/*.wav` — generated audio samples (regenerated via `python samples/audio/generate_samples.py`)
- Standard Python build artifacts (`__pycache__/`, `*.egg-info/`, `dist/`, etc.)

## Licence

MIT — see `LICENSE`.
