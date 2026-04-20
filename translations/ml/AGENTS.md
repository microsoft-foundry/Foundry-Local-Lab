# കോഡിങ് ഏജന്റ് നിർദ്ദേശങ്ങൾ

ഈ ഫയൽ ഈ റിപ്പോസിറ്ററിയിൽ പ്രവർത്തിക്കുന്ന AI കോഡിങ് ഏജന്റുകൾക്കായി (GitHub Copilot, Copilot Workspace, Codex, മറ്റു) പ്രാസംഗികമായ വിവരങ്ങൾ നൽകുന്നു.

## പ്രോജക്ട് അവലോകനം

ഇത് [Foundry Local](https://foundrylocal.ai) ഉപയോഗിച്ച് AI അപ്ലിക്കേഷനുകൾ നിർമ്മിക്കുന്നതിനുള്ള **ഹാൻഡ്‌സ്-ഓൺ വർക് ഷോപ്പ്** ആണ് — OpenAI-സമ്മതിയുള്ള API വഴി ഫലപ്രദമായ ഭാഷാ മോഡലുകൾ പൂർണ്ണമായും ഡിവൈസിൽ തന്നെ ഡൗൺലോഡ് ചെയ്ത്, മാനേജുചെയ്ത്, സേവനം നൽകുന്ന ഒരു ലൈറ്റ് വെയിറ്റ് റൺടൈം. വർക് ഷോപ്പിൽ പൈതൺ, ജാവാസ്ക്രിപ്റ്റ്, സിയും C# ൽ പ്രവർത്തിക്കാവുന്ന സ്റ്റെപ്പ്-ബൈ-സ്റ്റെപ്പ് ലാബ് ഗൈഡുകളും കോഡ് സാംപിളുകളും ഉൾപ്പെടുന്നു.

## റിപ്പോസിറ്ററി ഘടന

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

## ഭാഷ & ഫ്രെയിംവർക്കിന്റെ വിശദാംശങ്ങൾ

### Python
- **സ്ഥലം:** `python/`, `zava-creative-writer-local/src/api/`
- **അനുബന്ധങ്ങൾ:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **പ്രമുഖ പാക്കേജുകൾ:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **കുറഞ്ഞ പതിപ്പ്:** Python 3.9+
- **ഓടിക്കുക:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **സ്ഥലം:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **അനുബന്ധങ്ങൾ:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **പ്രമുഖ പാക്കേജുകൾ:** `foundry-local-sdk`, `openai`
- **മോഡ്യൂൾ സിസ്റ്റം:** ES മോഡ്യൂളുകൾ (`.mjs` ഫയലുകൾ, `"type": "module"`)
- **കുറഞ്ഞ പതിപ്പ്:** Node.js 18+
- **ഓടിക്കുക:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **സ്ഥലം:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **പ്രോജക്ട് ഫയലുകൾ:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **പ്രമുഖ പാക്കേജുകൾ:** `Microsoft.AI.Foundry.Local` (Windows അല്ലാത്തത്), `Microsoft.AI.Foundry.Local.WinML` (Windows — QNN EP ഉൾಗೊಂಡ സൂപർസെറ്റ്), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **ലക്ഷ്യം:** .NET 9.0 (കണ്ടീഷണൽ TFM: Windows-ൽ `net9.0-windows10.0.26100`, മറ്റ് സ്ഥലങ്ങളിൽ `net9.0`)
- **ഓടിക്കുക:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## കോഡിങ് നിബന്ധനകൾ

### പൊതുവായി
- എല്ലാ കോഡ് സാംപിളുകളും **സ്വയംമാത്രമായ ഏകഫയൽ ഉദാഹരണങ്ങൾ** — പങ്കുവെക്കുന്ന യുടിലിറ്റി ലൈബ്രറികളും ആബ്സ്ട്രാക്ഷനുകളും ഇല്ല.
- ഓരോ സാംപിളും താരതമ്യേന സ്വതന്ത്രമായും അതിന്റെ സ്വപൂർണ്ണമെന്ന_dependencies_ ഇൻസ്റ്റാൾ ചെയ്ത ശേഷം പ്രവർത്തിക്കുന്നു.
- API കീകൾ എല്ലാ സമയത്തും `"foundry-local"` ആയി സജ്ജീകരിച്ചിരിക്കുന്നു — Foundry Local ഇത് പ്ലേസ്ഹോൾഡർ ആയി ഉപയോഗിക്കുന്നു.
- ബേസ് URLs `http://localhost:<port>/v1` എന്നതാണ് — പോർട്ട് ഡൈനാമിക് ആണ്, SDK വഴി റൺടൈം-ൽ കണ്ടെത്തപ്പെടുന്നു (`manager.urls[0]` ജാവാസ്ക്രിപ്റ്റിൽ, `manager.endpoint` പൈത്തൺ-ൽ).
- Foundry Local SDK സേവന തുടങ്ങലും എൻഡ്‌പോയിന്റ് കണ്ടെത്തലും കൈകാര്യം ചെയ്യുന്നു; ഹാർഡ്-കോഡ് ചെയ്ത പോർട്ടുകൾക്കുപരി SDK പാറ്റേണുകൾ മുൻഗണന നൽകുക.

### Python
- `openai` SDK ഉപയോഗിക്കുക, `OpenAI(base_url=..., api_key="not-required")` എന്ന രീതിയിൽ.
- SDK-മാനേജുചെയ്ത സേവന ലൈഫ്‌സൈക്കിൾക്കായി `foundry_local`-ൽ നിന്ന് `FoundryLocalManager()` ഉപയോഗിക്കുക.
- സ്റ്റ്രീമിംഗ്: `stream` ഒബ്ജക്റ്റ് ഒഴിച്ച് `for chunk in stream:` എന്ന രീതിയിൽ ഇറ്ററേറ്റ് ചെയ്യുക.
- സാംപിള് ഫയലുകളിൽ ടൈപ്പ് അനോട്ടേഷനുകൾ ഒഴിവാക്കുക (വർക് ഷോപ്പ് പഠനാർത്ഥികൾക്കായി സാംപിളുകൾ ചുരുക്കമായി സൂക്ഷിക്കുക).

### JavaScript
- ES മോഡ്യൂൾ സിന്റാക്സ്: `import ... from "..."`.
- `"openai"`-യിൽ നിന്നുള്ള `OpenAI`യും `"foundry-local-sdk"`-ല് നിന്നുള്ള `FoundryLocalManager`ഉം ഉപയോഗിക്കുക.
- SDK ഇൻഷിയലൈസേഷൻ പാറ്റേൺ: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- സ്റ്റ്രീമിംഗ്: `for await (const chunk of stream)` എന്ന രീതിയിൽ.
- മുകളിലത്തെ ലെവൽ `await` വ്യാപകമായി ഉപയോഗിക്കുന്നു.

### C#
- നുള്ളബിൾ എനേബിൾഡ്, ഇംപ്ലിസിറ്റ് യുസിംഗ്സ്, .NET 9.
- SDK-മാനേജുചെയ്ത ലൈഫ് സൈക്കിൾക്കായി `FoundryLocalManager.StartServiceAsync()` ഉപയോഗിക്കുക.
- സ്റ്റ്രീമിംഗ്: `CompleteChatStreaming()` ഉപയോഗിച്ച് `foreach (var update in completionUpdates)`.
- പ്രധാന `csharp/Program.cs` CLI റൂട്ടർ ആയി കൃത്യമായ `RunAsync()` സ്റ്റാറ്റിക് മെത്തഡുകൾ കോവിഡ് ചെയ്യുന്നുണ്ട്.

### ഉപകരണ കോൾ ചെയ്യൽ
- ചില മോഡലുകൾ മാത്രം ടൂൾ കോൾ ചെയ്യൽ പിന്തുണയ്ക്കുന്നു: **Qwen 2.5** കുടുംബം (`qwen2.5-*`)യും **Phi-4-mini** (`phi-4-mini`)യും.
- ടൂൾ സ്കീമകൾ OpenAI ഫങ്ഷൻ-കോൾ ചെയ്യൽ JSON ഫോർമാറ്റ് പിന്തുടരുന്നു (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- സംവാദം മൾട്ടി-ടേൺ പാറ്റേണിൽ ആണ്: ഉപയോക്താവ് → അസിസ്റ്റന്റ്(ടൂൾ_കോൾസ്) → ടൂൾ(ഫലം) → അസിസ്റ്റന്റ്(അവസാന ഉത്തരത്തെ).
- ടൂൾ ഫലം സന്ദേശങ്ങളിൽ ഉള്ള `tool_call_id` മോഡൽ ടൂൾ കോൾ `id`-യുമായി ഒത്തുപോകണം.
- പൈത്തൺ നേരിട്ടുള്ള OpenAI SDK ഉപയോഗിക്കുന്നു; ജാവാസ്ക്രിപ്റ്റ് SDK ന്റെ നേറ്റിവ് `ChatClient` (`model.createChatClient()`); C# OpenAI SDK യും `ChatTool.CreateFunctionTool()` ഉം ഉപയോഗിക്കുന്നു.

### ChatClient (നേറ്റിവ് SDK ക്ലയന്റുകൾ)
- ജാവാസ്ക്രിപ്റ്റ്: `model.createChatClient()` ഒരു `ChatClient` മടങ്ങി നൽകുന്നു, ഇതിൽ `completeChat(messages, tools?)` ഒപ്പം `completeStreamingChat(messages, callback)` ഉണ്ട്.
- C#: `model.GetChatClientAsync()` ഒരു സാധാരണ `ChatClient` മടങ്ങി നൽകുന്നു, OpenAI NuGet പാക്കേജ് ഇറക്കുമതി ചെയ്യാതെയും ഉപയോഗിക്കാവുന്നതാണ്.
- പൈത്തൺ നേരിട്ടുള്ള ChatClient ഇല്ല — OpenAI SDK ഒരു `manager.endpoint` ഒപ്പം `manager.api_key` ഉപയോഗിക്കുക.
- **പ്രധാനമായത്:** ജാവാസ്ക്രിപ്റ്റ് `completeStreamingChat` **കോൾബാക്ക് പാറ്റേൺ** ഉപയോഗിക്കുന്നു, അസിങ്ക് ഇറ്ററേഷന് അല്ല.

###  റീസണിംഗ് മോഡലുകൾ
- `phi-4-mini-reasoning` അതിന്റെ ചിന്തനം `<think>...</think>` ടാഗുകളിൽ മുറിച്ച് നൽകുന്നു അവസാന മറുപടിക്ക് മുൻപ്.
- 필요ത്തിന് ഈ ടാഗുകൾ വേഗം തിരിച്ചറിയാനും ഉത്തരത്തിൽ നിന്ന് വേർതിരിക്കാനുമാകും.

## ലാബ് ഗൈഡുകൾ

ലാബ് ഫയലുകൾ `labs/` 폴더യിൽ മാർക്ക്ഡൗണാക്കി സജ്ജീകരിച്ചിട്ടുണ്ട്. അവ ഒരു നിരന്തര ഘടന പിന്തുടരുന്നു:
- ലോഗോ ഹെഡർ ചിത്രം
- തലക്കെട്ട്, ലക്ഷ്യം വിളിപ്പിക്കൽ
- അവലോകനം, പഠന ലക്ഷ്യങ്ങൾ, മുൻകൂർഅറിയിപ്പുകൾ
- ആശയ വിശദീകരണ വിഭാഗങ്ങൾ ചിത്രങ്ങളോടൊപ്പം
- നമ്പർ ചെയ്ത പാഠങ്ങൾ കോഡ് ബ്ലോക്കുകളും പ്രതീക്ഷിച്ച ഔട്ട്‌പുട്ടും ചേർന്ന്
- സംഗ്രഹ ടേബിൾ, പ്രധാന ആശയങ്ങൾ, കൂടുതൽ വായന
- അടുത്ത ഭാഗത്തിലേക്കുള്ള നാവിഗേഷൻ ലിങ്ക്

ലാബ് ഉള്ളടക്കം തിരുത്തുമ്പോൾ:
- നിലവിലുള്ള മാർക്ക്ഡൗൺ ഫോർമാറ്റിംഗ് ശൈലിയും വിഭാഗീയതയും പരിപാലിക്കുക.
- കോഡ് ബ്ലോക്കുകൾക്ക് ഭാഷ വ്യക്തമാക്കണം (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- ഓപ്പറേറ്റിംഗ് സിസ്റ്റത്തിന്റെ അനുസൃത shell കമാൻഡുകൾക്ക് ബാഷും പവർഷെല്ലും ഇരുവിധം നൽകുക.
- `> **Note:**`, `> **Tip:**`, `> **Troubleshooting:**` തുടങ്ങിയ കാൾഔട്ട് ശൈലികൾ ഉപയോഗിക്കുക.
- പട്ടികകൾ പൈപ്പ് ഫോർമാറ്റിൽ എഴുതുക (`| Header | Header |`).

## ബിൽഡ് & ടെസ്റ്റ് കമാൻഡുകൾ

| പ്രവർത്തി | കമാൻഡ് |
|--------|---------|
| **Python സാംപിളുകൾ** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS സാംപിളുകൾ** | `cd javascript && npm install && node <script>.mjs` |
| **C# സാംപിളുകൾ** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (വെബ്)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (വെബ്)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **ഡയഗ്രാമുകൾ സൃഷ്ടിക്കുക** | `npx mmdc -i <input>.mmd -o <output>.svg` (റൂട്ട് `npm install` ആവശ്യമാണ്) |

## പുറം ആശ്രിതങ്ങൾ

- **Foundry Local CLI** ഡെവലപ്പറുടെ മെഷീനിൽ ഇൻസ്റ്റാൾ ചെയ്തിരിക്കണം (`winget install Microsoft.FoundryLocal` അല്ലെങ്കിൽ `brew install foundrylocal`).
- **Foundry Local സേവനം** ലൊക്കലായി പ്രവർത്തിച്ച് ഡൈനാമിക് പോർട്ടിൽ OpenAI-സമ്മതിയുള്ള REST API നൽകുന്നു.
- ഒരു സാംപിള് പോലും ഓടിക്കാനായി ക്ലൗഡ് സേവനങ്ങളും API കീകളും Azure സബ്സ്ക്രിപ്ഷനുകളും ആവശ്യവുമില്ല.
- പാൽട്ട് 10 (കസ്റ്റം മോഡലുകൾ) ഓണക്സ്റ്റെൻറ്ടൈം-ജനായി `onnxruntime-genai` ആവശ്യമാണ്, ഹഗ്ഗിംഗ് ഫെയ്‌സിൽ നിന്നുള്ള മോഡൽ വെയ്റ്റുകളും ഡൗൺലോഡ് ചെയ്യപ്പെടും.

## കമ്മിറ്റ് ചെയ്യരുതാത്ത ഫയലുകൾ

`.gitignore` താഴെപ്പറയുന്നവ ഒഴിവാക്കും (മിക്കവാറും ഇതു ചെയ്യുമ്പോഴും):
- `.venv/` — പൈതൺ വെർച്വൽ എൻവയോൺമെന്റുകൾ
- `node_modules/` — npm അനുബന്ധങ്ങൾ
- `models/` — ONNX മോഡൽ കംപൈൽഡ് ഔട്ട്‌പുട്ട് (വലിയ ബൈനറി ഫയലുകൾ, Part 10-ൽ സൃഷ്ടിച്ചതായി)
- `cache_dir/` — ഹഗ്ഗിംഗ് ഫെയ്‌സ് മോഡൽ ഡൗൺലോഡ് കാഷെ
- `.olive-cache/` — Microsoft Olive പ്രവർത്തന ഡയറക്ടറി
- `samples/audio/*.wav` — സൃഷ്ടിച്ച ഓഡിയോ സാംപിളുകൾ (`python samples/audio/generate_samples.py` വഴി പുന:സൃഷ്ടിക്കാവുന്നതുള്ളവ)
- സ്റ്റാൻഡേർഡ് പൈത്തൺ ബിൽഡ് ആർട്ടിഫാക്റ്റ്സ് (`__pycache__/`, `*.egg-info/`, `dist/` തുടങ്ങിയവ)

## ലൈസൻസ്

MIT — കാണുക `LICENSE`.