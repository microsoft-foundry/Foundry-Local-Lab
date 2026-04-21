# Maelekezo ya Wakala wa Kupanua Msimbo

Faili hii inatoa muktadha kwa maajenti wa AI wa kupanua msimbo (GitHub Copilot, Copilot Workspace, Codex, n.k.) wanaofanya kazi katika hifadhidata hii.

## Muhtasari wa Mradi

Huu ni **mafunzo ya vitendo** ya kujenga programu za AI kwa kutumia [Foundry Local](https://foundrylocal.ai) — wakati mdogo huchukua, kusimamia, na kuhudumia mifano ya lugha kabisa kwenye kifaa kupitia API inayolingana na OpenAI. Warsha hii ina mwongozo wa maabara hatua kwa hatua na mifano ya msimbo inayoweza kuendeshwa katika Python, JavaScript, na C#.

## Muundo wa Hifadhidata

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

## Maelezo ya Lugha na Mfumo

### Python
- **Mahali:** `python/`, `zava-creative-writer-local/src/api/`
- **Tegemezi:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Vipakuzi muhimu:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Toleo la chini:** Python 3.9+
- **Endesha:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Mahali:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Tegemezi:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Vipakuzi muhimu:** `foundry-local-sdk`, `openai`
- **Mfumo wa moduli:** Moduli za ES (`.mjs` files, `"type": "module"`)
- **Toleo la chini:** Node.js 18+
- **Endesha:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Mahali:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Faili za Mradi:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Vipakuzi muhimu:** `Microsoft.AI.Foundry.Local` (siyo-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset yenye QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Lengo:** .NET 9.0 (TFM sharti: `net9.0-windows10.0.26100` kwa Windows, `net9.0` mahali pengine)
- **Endesha:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Mazoea ya Uandishi wa Msimbo

### Kwa Jumla
- Mifano yote ya msimbo ni **mifano ya faili moja yenye kujitegemea** — hakuna maktaba za matumizi ya pamoja au uliyapo.
- Kila mfano unaendesha kivyake baada ya kusakinisha tegemezi zake.
- Funguo za API huwekwa daima kuwa `"foundry-local"` — Foundry Local hutumia hii kama kielekezi.
- URL msingi hutumia `http://localhost:<port>/v1` — lango ni zuri na hutambuliwa wakati wa utekelezaji kupitia SDK (`manager.urls[0]` kwa JS, `manager.endpoint` kwa Python).
- SDK ya Foundry Local huduathiri kuanzishwa kwa huduma na kugundua lango; tumia mifumo ya SDK badala ya kutumia lango lililowekwa moja kwa moja.

### Python
- Tumia SDK ya `openai` kwa `OpenAI(base_url=..., api_key="not-required")`.
- Tumia `FoundryLocalManager()` kutoka `foundry_local` kwa usimamizi wa mzunguko wa huduma kupitia SDK.
- Upitishaji wa mfululizo: pitia kitu cha `stream` kwa `for chunk in stream:`.
- Hakuna maelezo ya aina katika faili za mfano (wahi mifano fupi kwa watunzi wa warsha).

### JavaScript
- Msamiati wa moduli za ES: `import ... from "..."`.
- Tumia `OpenAI` kutoka `"openai"` na `FoundryLocalManager` kutoka `"foundry-local-sdk"`.
- Mfumo wa kuanzisha SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Upitishaji wa mfululizo: `for await (const chunk of stream)`.
- `await` ya ngazi ya juu hutumika kila mahali.

### C#
- Nullable imewezeshwa, usajili wa viendeshaji vya implisit, .NET 9.
- Tumia `FoundryLocalManager.StartServiceAsync()` kwa usimamizi wa mzunguko kupitia SDK.
- Upitishaji wa mfululizo: `CompleteChatStreaming()` kwa `foreach (var update in completionUpdates)`.
- Faili kuu `csharp/Program.cs` ni ratiba ya CLI inayochukua na kuwasilisha kwa njia za statiki `RunAsync()`.

### Kufanya Kuitwa kwa Zana
- Mifano fulani tu inaunga mkono kuitwa kwa zana: familia ya **Qwen 2.5** (`qwen2.5-*`) na **Phi-4-mini** (`phi-4-mini`).
- Mipangilio ya zana inafuata muundo wa OpenAI wa JSON ya kuitwa kwa kazi (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Mazungumzo hutumia mpangilio wa mizunguko mingi: mtumiaji → msaidizi (itoo_kwa_zana) → zana (matokeo) → msaidizi (jibu la mwisho).
- `tool_call_id` katika ujumbe wa matokeo ya zana lazima lingane na `id` kutoka kwa kuitwa kwa zana ya mfano.
- Python hutumia moja kwa moja SDK ya OpenAI; JavaScript hutumia kwa asili SDK ya `ChatClient` (`model.createChatClient()`); C# hutumia SDK ya OpenAI na `ChatTool.CreateFunctionTool()`.

### ChatClient (Mteja wa SDK Asili)
- JavaScript: `model.createChatClient()` hurejesha `ChatClient` yenye `completeChat(messages, tools?)` na `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` hurejesha `ChatClient` wa kawaida unaotumika bila kuingiza kifurushi cha OpenAI NuGet.
- Python haina ChatClient asilia — tumia SDK ya OpenAI na `manager.endpoint` na `manager.api_key`.
- **Muhimu:** JavaScript `completeStreamingChat` hutumia **mfumo wa callback**, si iteresheni ya async.

### Mifano ya Fikira
- `phi-4-mini-reasoning` huzingira mawazo yake na lebo `<think>...</think>` kabla ya jibu la mwisho.
- Changanua lebo hizo ili kutenganisha fikira na jibu inapohitajika.

## Mwongozo wa Maabara

Faili za maabara ziko katika `labs/` kama Markdown. Zinafuata muundo thabiti:
- Picha ya kichwa cha nembo
- Kichwa na malengo ya wito
- Muhtasari, Malengo ya Kujifunza, Mipango ya awali
- Sehemu za maelezo ya dhana zenye michoro
- Mazoea yaliyo nambari na sehemu za msimbo na matokeo yanayotarajiwa
- Jedwali la muhtasari, Matokeo Muhimu, Usomaji Zaidi
- Kiungo cha urambazaji kwa sehemu inayofuata

Unapobadilisha yaliyomo maabara:
- Hifadhi mtindo wa muundo wa Markdown uliopo na mfuatano wa sehemu.
- Sehemu za msimbo lazima zielezee lugha (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Toa toleo la bash na PowerShell kwa amri za shell zinapoathiriwa na OS.
- Tumia mitindo ya maelekezo `> **Note:**`, `> **Tip:**`, na `> **Troubleshooting:**`.
- Jedwali litumie muundo wa mabano `| Header | Header |`.

## Amri za Kujenga na Kupima

| Kitendo | Amri |
|--------|---------|
| **Mifano ya Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **Mifano ya JS** | `cd javascript && npm install && node <script>.mjs` |
| **Mifano ya C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (mtandao)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (mtandao)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Tengeneza michoro** | `npx mmdc -i <input>.mmd -o <output>.svg` (inahitaji root `npm install`) |

## Tegemezi za Nje

- **Foundry Local CLI** lazima isakinishwe kwenye kompyuta ya msanidi programu (`winget install Microsoft.FoundryLocal` au `brew install foundrylocal`).
- **Huduma ya Foundry Local** inaendeshwa mahali na hutoa API ya REST inayolingana na OpenAI kwenye lango zuri.
- Hakuna huduma za wingu, funguo za API, au usajili wa Azure unahitajika kuendesha mfano wowote.
- Sehemu ya 10 (mifano maalum) inahitaji pia `onnxruntime-genai` na huchukua uzito wa mfano kutoka Hugging Face.

## Faili Ambazo Hazipaswi Kuwekwa Rejistarini

`.gitignore` inapaswa kuzuia (na inazuia kwa sehemu kubwa):
- `.venv/` — mazingira pepe ya Python
- `node_modules/` — tegemezi za npm
- `models/` — matokeo ya kificho cha mfano wa ONNX (faili kubwa za binary, zilizozalishwa na Sehemu ya 10)
- `cache_dir/` — kache ya kupakua mfano wa Hugging Face
- `.olive-cache/` — saraka ya kazi ya Microsoft Olive
- `samples/audio/*.wav` — mifano ya sauti iliyozalishwa (huundwa tena kwa `python samples/audio/generate_samples.py`)
- Vifungo vya kawaida vya kijengwa vya Python (`__pycache__/`, `*.egg-info/`, `dist/`, n.k.)

## Leseni

MIT — ona `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Tangazo la Kutokuzwa**:  
Hati hii imetafsiriwa kwa kutumia huduma ya tafsiri ya AI [Co-op Translator](https://github.com/Azure/co-op-translator). Ingawa tunajitahidi kuhakikisha usahihi, tafadhali fahamu kwamba tafsiri za kiotomatiki zinaweza kuwa na makosa au upungufu wa usahihi. Hati ya asili katika lugha yake ya asili inapaswa kuchukuliwa kama chanzo halali. Kwa taarifa muhimu, tafsiri ya kitaalamu ya binadamu inapendekezwa. Hatuna wajibu wa makosa yoyote ya kuelewana au makosa yanayotokana na matumizi ya tafsiri hii.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->