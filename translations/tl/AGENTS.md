# Mga Tagubilin para sa Coding Agent

Ang file na ito ay nagbibigay ng konteksto para sa mga AI coding agent (GitHub Copilot, Copilot Workspace, Codex, atbp.) na nagtatrabaho sa repositoryong ito.

## Pangkalahatang-ideya ng Proyekto

Ito ay isang **hands-on workshop** para sa paggawa ng mga AI application gamit ang [Foundry Local](https://foundrylocal.ai) — isang magaan na runtime na nagda-download, namamahala, at nagsisilbi ng mga language model nang buong-lokal gamit ang isang OpenAI-compatible API. Kasama sa workshop ang mga step-by-step na lab guide at mga runnable code sample sa Python, JavaScript, at C#.

## Estruktura ng Repositoryo

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

## Detalye ng Wika at Framework

### Python
- **Lokasyon:** `python/`, `zava-creative-writer-local/src/api/`
- **Dependencies:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Pangunahing packages:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Min version:** Python 3.9+
- **Paandar:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Lokasyon:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Dependencies:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Pangunahing packages:** `foundry-local-sdk`, `openai`
- **Module system:** ES modules (`.mjs` files, `"type": "module"`)
- **Min version:** Node.js 18+
- **Paandar:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Lokasyon:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Mga file ng proyekto:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Pangunahing packages:** `Microsoft.AI.Foundry.Local` (non-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset na may QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Target:** .NET 9.0 (conditional TFM: `net9.0-windows10.0.26100` sa Windows, `net9.0` sa ibang lugar)
- **Paandar:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Mga Convention sa Coding

### Pangkalahatan
- Lahat ng code sample ay **self-contained na halimbawa sa iisang file** — walang shared utility libraries o abstractions.
- Ang bawat sample ay tumatakbo nang independent pagkatapos mai-install ang sariling dependencies.
- Laging naka-set ang API keys sa `"foundry-local"` — ginagamit ito ng Foundry Local bilang placeholder.
- Base URLs ay gumagamit ng `http://localhost:<port>/v1` — ang port ay dynamic at nadidiscover sa runtime gamit ang SDK (`manager.urls[0]` sa JS, `manager.endpoint` sa Python).
- Ang Foundry Local SDK ang nag-aasikaso ng pagsisimula ng service at pagdiskubre ng endpoint; mas mainam gamitin ang mga pattern ng SDK kaysa hard-coded ports.

### Python
- Gamitin ang `openai` SDK na may `OpenAI(base_url=..., api_key="not-required")`.
- Gamitin ang `FoundryLocalManager()` mula sa `foundry_local` para sa SDK-managed na lifecycle ng service.
- Streaming: i-iterate ang `stream` object gamit ang `for chunk in stream:`.
- Walang type annotations sa mga sample file (panatilihing maigsi para sa mga mag-aaral sa workshop).

### JavaScript
- ES module syntax: `import ... from "..."`.
- Gamitin ang `OpenAI` mula sa `"openai"` at `FoundryLocalManager` mula sa `"foundry-local-sdk"`.
- SDK init pattern: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Ginagamit ang top-level `await` sa buong codes.

### C#
- Nullable enabled, implicit usings, .NET 9.
- Gamitin ang `FoundryLocalManager.StartServiceAsync()` para sa SDK-managed lifecycle.
- Streaming: `CompleteChatStreaming()` gamit ang `foreach (var update in completionUpdates)`.
- Ang pangunahing `csharp/Program.cs` ay isang CLI router na nagdi-dispatch sa mga static na `RunAsync()` methods.

### Pagtawag sa Tool
- Tanging ilang mga modelo lang ang sumusuporta sa pagtawag ng tool: **Qwen 2.5** family (`qwen2.5-*`) at **Phi-4-mini** (`phi-4-mini`).
- Ang mga tool schema ay sumusunod sa OpenAI function-calling JSON format (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Ang pag-uusap ay gumagamit ng multi-turn pattern: user → assistant (tool_calls) → tool (resulta) → assistant (final na sagot).
- Ang `tool_call_id` sa mga mensahe ng resulta ng tool ay dapat tumugma sa `id` mula sa tool call ng modelo.
- Sa Python ay ginagamit ang OpenAI SDK nang direkta; sa JavaScript ay ang native na `ChatClient` ng SDK (`model.createChatClient()`); sa C# ay ang OpenAI SDK gamit ang `ChatTool.CreateFunctionTool()`.

### ChatClient (Native SDK Client)
- JavaScript: `model.createChatClient()` ay nagbabalik ng `ChatClient` na may `completeChat(messages, tools?)` at `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` ay nagbabalik ng standard `ChatClient` na maaaring gamitin nang hindi kinakalangang i-import ang OpenAI NuGet package.
- Wala sa Python ang native ChatClient — gamitin ang OpenAI SDK sa pamamagitan ng `manager.endpoint` at `manager.api_key`.
- **Mahahalaga:** Ang JavaScript `completeStreamingChat` ay gumagamit ng **callback pattern**, hindi async iteration.

### Mga Reasoning Model
- Ang `phi-4-mini-reasoning` ay inilalagay ang pag-iisip nito sa mga tag na `<think>...</think>` bago ang pinal na sagot.
- I-parse ang mga tag na ito upang paghiwalayin ang pag-iisip mula sa sagot kung kinakailangan.

## Mga Gabay sa Lab

Ang mga file para sa lab ay nasa `labs/` bilang Markdown. May konsistent na estruktura:
- Logo header image
- Pamagat at goal na pahayag
- Pangkalahatan, Mga Layunin sa Pagkatuto, Mga Kinakailangan
- Mga seksyong paliwanag ng konsepto na may mga diagram
- Mga numbered na pagsasanay na may mga code block at inaasahang output
- Table ng buod, Key Takeaways, Karagdagang Babasahin
- Link sa susunod na bahagi

Kapag nag-eedit ng content ng lab:
- Panatilihin ang umiiral na Markdown formatting style at hierarchy ng seksyon.
- Ang mga code block ay dapat mag-specify ng wika (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Magbigay ng parehong bash at PowerShell na variant para sa mga shell command kung mahalaga ang OS.
- Gumamit ng callout styles na `> **Note:**`, `> **Tip:**`, at `> **Troubleshooting:**`
- Ang mga talahanayan ay gumagamit ng `| Header | Header |` pipe format.

## Mga Command para sa Build at Test

| Aksyon | Command |
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
| **Generate diagrams** | `npx mmdc -i <input>.mmd -o <output>.svg` (kailangan ng root `npm install`) |

## Mga Panlabas na Dependensiya

- Kailangang mai-install ang **Foundry Local CLI** sa makina ng developer (`winget install Microsoft.FoundryLocal` o `brew install foundrylocal`).
- Ang **Foundry Local service** ay tumatakbo nang lokal at nag-eexpose ng OpenAI-compatible REST API sa isang dynamic na port.
- Walang kinakailangang cloud services, API keys, o Azure subscriptions para mapatakbo ang anumang sample.
- Ang Part 10 (custom models) ay dagdag na nangangailangan ng `onnxruntime-genai` at nagda-download ng model weights mula sa Hugging Face.

## Mga File na Hindi Dapat I-commit

Ang `.gitignore` ay dapat mag-exclude (at ginagawa para sa karamihan):
- `.venv/` — mga virtual environment ng Python
- `node_modules/` — mga npm dependency
- `models/` — mga compiled ONNX model output (malalaking binary file, na-generate sa Part 10)
- `cache_dir/` — Hugging Face model download cache
- `.olive-cache/` — Microsoft Olive working directory
- `samples/audio/*.wav` — mga generated audio sample (muling pina-produce gamit ang `python samples/audio/generate_samples.py`)
- Mga standard Python build artifacts (`__pycache__/`, `*.egg-info/`, `dist/`, atbp.)

## Lisensya

MIT — tingnan ang `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Pagsasabi ng Pagtatangi**:  
Ang dokumentong ito ay isinalin gamit ang AI translation service na [Co-op Translator](https://github.com/Azure/co-op-translator). Bagaman nagsusumikap kaming maging tumpak, pakatandaan na ang mga awtomatikong pagsasalin ay maaaring magkaroon ng mga pagkakamali o di-tumpak na impormasyon. Ang orihinal na dokumento sa orihinal nitong wika ang dapat ituring na opisyal na pinagkukunan. Para sa mga kritikal na impormasyon, inirerekomenda ang propesyonal na pagsasaling-tao. Hindi kami mananagot sa anumang maling pagkaunawa o maling interpretasyon na maaaring magmula sa paggamit ng pagsasaling ito.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->