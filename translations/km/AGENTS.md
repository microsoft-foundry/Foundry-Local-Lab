# សេចក្ដីណែនាំអំពីភ្នាក់ងារកូដ

ឯកសារនេះផ្តល់បរិបទសម្រាប់ភ្នាក់ងារកូដ AI (GitHub Copilot, Copilot Workspace, Codex, ជាដើម) ដែលធ្វើការនៅក្នុងរ៉ីភូស៊ីទូរនេះ។

## ទិដ្ឋភាពទូទៅគម្រោង

នេះជាកម្មវិធីបណ្តុះបណ្តាលប្រើប្រាស់ដៃបានសម្រាប់សាងសង់កម្មវិធី AI ជាមួយ [Foundry Local](https://foundrylocal.ai) — ជាបរិស្ថានកំណត់រត់ស្រាលដែលទាញយក គ្រប់គ្រង និងបម្រើម៉ូឌែលភាសាទាំងមូលនៅលើឧបករណ៍តាមរយៈ API ដែលអាចផ្គូផ្គងជាមួយ OpenAI។ កម្មវិធីបណ្តុះបណ្តាលមានមគ្គុទេសក៍លាបជំហាន-ដោយ-ជំហាន និងឧទាហរណ៌កូដអាចរត់បានក្នុងភាសា Python, JavaScript, និង C#។

## រចនាសម្ព័ន្ធរ៉ីភូស៊ីទូរ

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

## ព័ត៌មានអំពីភាសា និងស៊ុមហ្វ្រេមវ៉ដែល

### Python
- **ទីតាំង៖** `python/`, `zava-creative-writer-local/src/api/`
- **ការពឹងផ្អែក៖** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **កញ្ចប់សំខាន់ៗ៖** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **កំណែអប្បបរមា៖** Python 3.9+
- **ការរត់៖** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **ទីតាំង៖** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **ការពឹងផ្អែក៖** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **កញ្ចប់សំខាន់ៗ៖** `foundry-local-sdk`, `openai`
- **ប្រព័ន្ធមូឌុល៖** ផ្នែកមូឌុល ES (`.mjs` ឯកសារ, `"type": "module"`)
- **កំណែអប្បបរមា៖** Node.js 18+
- **ការរត់៖** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **ទីតាំង៖** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **ឯកសារគម្រោង៖** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **កញ្ចប់សំខាន់ៗ៖** `Microsoft.AI.Foundry.Local` (មិនមែន Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — សព្វគ្រប់ជាមួយ QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **គោលដៅ៖** .NET 9.0 (TFM នៃលក្ខខណ្ឌ: `net9.0-windows10.0.26100` លើ Windows, `net9.0` ទូទៅ)
- **ការរត់៖** `cd csharp && dotnet run [chat|rag|agent|multi]`

## ច្បាប់ការសរសេរកូដ

### ទូទៅ
- កូដឧទាហរណ៍ទាំងអស់ជា **ឯកសារតែមួយផ្ទាល់ខ្លួន** — មិនប្រើបណ្ណាល័យ ឬសារសហគ្រិនផ្សេងទេ។
- ឧទាហរណ៍នីមួយៗដំណើរការឯករាជ្យបន្ទាប់ពីដំឡើងការពឹងផ្អែករបស់ខ្លួន។
- Key API តែងតែតម្លៃជា `"foundry-local"` — Foundry Local ប្រើវាជាតំរូវការ។
- URL មូលដ្ឋានប្រើ `http://localhost:<port>/v1` — ព្រិក​port មានការប្រែប្រួល និងរកឃើញនៅពេលរត់តាម SDK (`manager.urls[0]` ក្នុង JS, `manager.endpoint` ក្នុង Python)។
- SDK Foundry Local គ្រប់គ្រងការចាប់ផ្តើមសេវា និងការរកឃើញ endpoint; ត្រូវចូលចិត្ដប្រើលំនាំ SDK ជំនួសការប្រើ port រឹងរាំង។

### Python
- ប្រើ `openai` SDK ជាមួយ `OpenAI(base_url=..., api_key="not-required")`។
- ប្រើ `FoundryLocalManager()` ពី `foundry_local` សម្រាប់គ្រប់គ្រងជីវិតសេវាកម្មដោយ SDK។
- Streaming៖ ធ្វើការ iterate លើ `stream` វត្ថុជាមួយ `for chunk in stream:`។
- មិនមានការបញ្ជាក់ប្រភេទក្នុងឯកសារឧទាហរណ៍ (រក្សាការខ្លីសង្ខេបដល់អ្នករៀនរបស់សិក្ខាគមន៍)។

### JavaScript
- វិធីសាស្រ្តមូឌុល ES៖ `import ... from "..."`។
- ប្រើ `OpenAI` ពី `"openai"` និង `FoundryLocalManager` ពី `"foundry-local-sdk"`។
- លំនាំចាប់ផ្តើម SDK៖ `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`។
- Streaming៖ `for await (const chunk of stream)`.
- ប្រើ `await` នៅកម្រិតកំពូលទាំងមូល។

### C#
- បានបើក Nullable, មាន implicit usings, .NET 9។
- ប្រើ `FoundryLocalManager.StartServiceAsync()` សម្រាប់គ្រប់គ្រងជីវិតសេវាកម្មដោយ SDK។
- Streaming៖ `CompleteChatStreaming()` ជាមួយ `foreach (var update in completionUpdates)`។
- គ្លី `csharp/Program.cs` ជា CLI router ប្រើបញ្ជូនទៅ static method `RunAsync()`។

### ការហៅឧបករណ៍
- មានតែម៉ូឌែលជាក់លាក់ប៉ុណ្ណោះគាំទ្រការហៅឧបករណ៍៖ គ្រួសារ **Qwen 2.5** (`qwen2.5-*`) និង **Phi-4-mini** (`phi-4-mini`)។
- សៀគ្វីឧបករណ៍អនុវត្តតាមទ្រង់ទ្រាយសំរាប់ហៅមុខងារ JSON របស់ OpenAI (`type: "function"`, `function.name`, `function.description`, `function.parameters`)។
- ការពិភាក្សា​ប្រើលំនាំជាច្រើនជំហាន៖ user → assistant (tool_calls) → tool (results) → assistant (final answer)។
- `tool_call_id` ក្នុងសារ​លទ្ធផល​ឧបករណ៍ត្រូវតែរំពឹងទៅលើ `id` ពីការហៅឧបករណ៍របស់ម៉ូឌែល។
- Python ប្រើ OpenAI SDK ត្រង់ៗ; JavaScript ប្រើ SDK native `ChatClient` (`model.createChatClient()`); C# ប្រើ OpenAI SDK ជាមួយ `ChatTool.CreateFunctionTool()`។

### ChatClient (Native SDK Client)
- JavaScript៖ `model.createChatClient()` ផ្ទេរវិញ `ChatClient` មាន `completeChat(messages, tools?)` និង `completeStreamingChat(messages, callback)`។
- C#៖ `model.GetChatClientAsync()` ផ្ទេរវិញ `ChatClient` ស្តង់ដា ដែលអាចប្រើបានបើគ្មានការនាំចូលដំបូន្មាន OpenAI NuGet។
- Python គ្មាន ChatClient native — ប្រើ OpenAI SDK ជាមួយ `manager.endpoint` និង `manager.api_key`។
- **សំខាន់**៖ JavaScript របៀប `completeStreamingChat` ប្រើលំនាំ **callback** មិនមែន iteration async ទេ។

### ម៉ូឌែលអនុវត្តការត្រួតពិនិត្យសន្មត់
- `phi-4-mini-reasoning` ដាក់គំនិតរបស់វានៅក្នុងឯកសារ `<think>...</think>` មុនចម្លើយចុងក្រោយ។
- វាស់វែង Tag ដើម្បីបំបែកការត្រួតពិនិត្យពីចម្លើយពេលចាំបាច់។

## មគ្គុទេសក៍លាប

ឯកសារលាបនៅក្នុង `labs/` ជា Markdown។ វាមានរចនាសម្ព័ន្ធស្រាលល្អលំអិត៖
- រូបភាពតំណាងផ្នែកកំពូល
- ចំណងជើង និងការអំពាវនាវគ្រួសារ
- ទិដ្ឋភាពទូទៅ, គោលបំណងរៀន, មុនរៀន
- ពិពណ៌នាធាតុគំនិតជាមួយគំនូសតាង
- ធ្វើលំហាត់លេខរៀងជាមួយកូដ និងលទ្ធផលរំពឹងទុក
- តារាងសម្រាប់សង្ខេប, ចំណុចសំខាន់, អានបន្ថែម
- តំណរគ្រប់គ្រងទៅផ្នែកបន្ទាប់

នៅពេលកែសម្រួលមាតិកាលាប៖
- រក្សាប្រភេទកំណត់រចនាសម្ព័ន្ធ Markdown និងស្រទាប់ផ្នែកដែលមាន។
- ប្រើប្លុកកូដសម្រាប់ភាសា (`python`, `javascript`, `csharp`, `bash`, `powershell`)។
- ផ្តល់យ៉ាងទាំងពីររបស់ bash និង PowerShell សម្រាប់ការបញ្ជូលព្រោះ OS មានតួនាទី។
- ប្រើ `> **Note:**`, `> **Tip:**`, និង `> **Troubleshooting:**` ដើម្បីបង្ហាញការបញ្ជូន។
- តារាងប្រើទ្រង់ទ្រាយបាញ់ប៉ាយ `| Header | Header |`។

## ពាក្យបញ្ជារការសាងសង្រ្គោះ និងសាកល្បង

| សកម្មភាព | ពាក្យបញ្ជា |
|--------|---------|
| **ឧទាហរណ៍ Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **ឧទាហរណ៍ JS** | `cd javascript && npm install && node <script>.mjs` |
| **ឧទាហរណ៍ C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (វែប)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (វែប)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **បង្កើតគំនូសតាង** | `npx mmdc -i <input>.mmd -o <output>.svg` (​តម្រូវការដំឡើងរចនាសម្ព័ន្ធ root `npm install`) |

## ការពឹងផ្អែកខាងក្រៅ

- **Foundry Local CLI** ត្រូវតែដំឡើងនៅលើម៉ាស៊ីនអ្នកអភិវឌ្ឍ (`winget install Microsoft.FoundryLocal` ឬ `brew install foundrylocal`)។
- **សេវាកម្ម Foundry Local** រត់នៅក្នុងកន្លែង ហើយបង្ហាញ REST API ដែលផ្គូផ្គងជាមួយ OpenAI លើ port ដែលប្រែប្រាស់។
- មិនមានសេវាកម្ម cloud, key API, ឬជាវ Azure ត្រូវការដើម្បីរត់ឧទាហរណ៍ណាមួយទេ។
- ផ្នែកទី 10 (ម៉ូឌែលអ៊ុយសសិច) ត្រូវការបន្ថែម `onnxruntime-genai` និងទាញយកទំងន់ម៉ូឌែលពី Hugging Face។

## ឯកសារដែលមិនគួរផ្ញើផ្សេង

`.gitignore` នឹងដកចេញ (ហើយមើលថាជាទូទៅ):
- `.venv/` — បរិស្ថាន Python virtual
- `node_modules/` — ឯកសារ npm
- `models/` — លទ្ធផលម៉ូឌែល ONNX បកប្រែ (ឯកសារប្រូងចំរាយធំ ដែលបង្កើតដោយផ្នែកទី 10)
- `cache_dir/` — កម្រងទាញយកម៉ូឌែល Hugging Face
- `.olive-cache/` — កំណត់ការងារម៉ីក្រូសូហ្វអុលីវ
- `samples/audio/*.wav` — ឧទាហរណ៍សម្លេងបង្កើតឡើង (បង្កើតឡើងវិញតាម `python samples/audio/generate_samples.py`)
- ការដាក់ចំណែកបង្កើត Python ជាស្តង់ដារ (`__pycache__/`, `*.egg-info/`, `dist/`, ល។)

## អាជ្ញាប័ណ្ណ

MIT — មើល `LICENSE`។