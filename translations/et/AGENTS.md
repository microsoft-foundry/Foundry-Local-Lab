# Kodeerimisagendi juhised

See fail annab konteksti AI koodiaagentidele (GitHub Copilot, Copilot Workspace, Codex jne), kes töötavad selles hoidlas.

## Projekti ülevaade

See on **praktiline töötuba** AI rakenduste loomiseks kasutades [Foundry Local](https://foundrylocal.ai) — kerget käituskeskkonda, mis laadib alla, haldab ja teenindab keelemudeleid täielikult seadmes OpenAI-ga ühilduva API kaudu. Töötuba sisaldab samm-sammult juhendeid ja käideldavaid koodinäiteid Pythonis, JavaScriptis ja C#-s.

## Hoidla struktuur

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

## Keeled ja raamistikud

### Python
- **Asukoht:** `python/`, `zava-creative-writer-local/src/api/`
- **Sõltuvused:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Põhipaketid:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Minimaalne versioon:** Python 3.9+
- **Käivitus:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Asukoht:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Sõltuvused:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Põhipaketid:** `foundry-local-sdk`, `openai`
- **Moodulisüsteem:** ES moodulid (`.mjs` failid, `"type": "module"`)
- **Minimaalne versioon:** Node.js 18+
- **Käivitus:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Asukoht:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Projekti failid:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Põhipaketid:** `Microsoft.AI.Foundry.Local` (mitte-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — QNN EP-ga laiendatud), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Sihtplatvorm:** .NET 9.0 (tingimuslik TFM: Windowsil `net9.0-windows10.0.26100`, mujal `net9.0`)
- **Käivitus:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Kodeerimise tavad

### Üldine
- Kõik koodinäited on **iseseisvad ühe failiga näited** — ei ole jagatud abiteeke ega abstraktsioone.
- Iga näide käivitatakse iseseisvalt pärast oma sõltuvuste paigaldamist.
- API võtmed on alati seatud väärtusele `"foundry-local"` — Foundry Local kasutab seda kohatäitjana.
- Põhi-URL-id kasutavad `http://localhost:<port>/v1` — port on dünaamiline ja leitakse käivitamisel SDK kaudu (`manager.urls[0]` JavaScriptis, `manager.endpoint` Pythonis).
- Foundry Local SDK haldab teenuse käivitamist ja lõpp-punktide leidmist; eelistada SDK mustreid eelkõige teiste kõvasti kodeeritud portide asemel.

### Python
- Kasutatakse `openai` SDK-d koos `OpenAI(base_url=..., api_key="not-required")`.
- SDK-poolse teenuse elutsükli haldamiseks kasutatakse `FoundryLocalManager()` klassi moodulist `foundry_local`.
- Voogedastus: iteratsioon `stream` objekti üle `for chunk in stream:`.
- Näidetes ei ole tüübisildistusi (et näited oleksid selged ja lihtsad).

### JavaScript
- ES mooduli süntaks: `import ... from "..."`.
- Kasutatakse `OpenAI` paketist `"openai"` ja `FoundryLocalManager` paketist `"foundry-local-sdk"`.
- SDK käivitamise muster: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Voogedastus: `for await (const chunk of stream)`.
- Taseülene `await` on kogu koodis laialt kasutusel.

### C#
- Nullable on lubatud, automaatsed `using` direktiivid, .NET 9.
- SDK hallatud elutsükli jaoks kasutatakse `FoundryLocalManager.StartServiceAsync()`.
- Voogedastus: `CompleteChatStreaming()` koos `foreach (var update in completionUpdates)`.
- Peamine fail `csharp/Program.cs` on käsurea liides, mis suunab päringud staatilistele `RunAsync()` meetoditele.

### Tööriistade kasutamine
- Tööriistakõnet toetavad ainult teatud mudelid: **Qwen 2.5** perekond (`qwen2.5-*`) ja **Phi-4-mini** (`phi-4-mini`).
- Tööriista skeemid järgivad OpenAI funktsioonikõne JSON formaati (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Vestlus on mitmetasandiline muster: kasutaja → assistent (tööriistakõned) → tööriist (tulemused) → assistent (lõplik vastus).
- Tööriista tulemuste sõnumites peab `tool_call_id` vastama mudeli tööriistakõne `id`-le.
- Python kasutab OpenAI SDK-d otse; JavaScript SDK sisseehitatud `ChatClient` (via `model.createChatClient()`); C# kasutab OpenAI SDK-d koos `ChatTool.CreateFunctionTool()`.

### ChatClient (natiivne SDK klient)
- JavaScript: `model.createChatClient()` tagastab `ChatClient` objekti, millel on meetodid `completeChat(messages, tools?)` ja `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` tagastab tavalise `ChatClient`, mida saab kasutada ilma OpenAI NuGet paketita.
- Pythonil natiivset ChatClienti ei ole — kasutatakse OpenAI SDK-t koos `manager.endpoint` ja `manager.api_key`.
- **Oluline:** JavaScripti `completeStreamingChat` kasutab **callback-mustrit**, mitte asünkroonset iteratsiooni.

### Loogikasse seotud mudelid
- `phi-4-mini-reasoning` pakendab oma mõtlemisprotsessi `<think>...</think>` siltide vahele enne lõplikku vastust.
- Vajadusel tuleb sildid välja sõeluda, et eraldada põhjendus ja vastus.

## Laborijuhtumid

Laborifailid on `labs/` kaustas Markdown vormingus. Suhtuvad järgmiselt:
- Logo päise pilt
- Pealkiri ja eesmärgi esiletõstmine
- Ülevaade, õpieesmärgid, eeltingimused
- Kontseptsioonide selgitused diagrammidega
- Numbrilised harjutused koos koodiblokkide ja ootuspärase väljundiga
- Kokkuvõtte tabel, peamised õppetunnid, edasine lugemine
- Navigeerimislink järgmise osa juurde

Laborisisu muutmisel:
- Säilita olemasolev Markdown vormindus ja jaotiste hierarhia.
- Koodiblokid määratle keele järgi (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Kontrolli, et shell-käskude puhul on nii bash kui PowerShell variandid seal, kus OS eristub.
- Kasuta väljaesiletõstetud vorme `> **Note:**`, `> **Tip:**`, `> **Troubleshooting:**`.
- Tabelid on `| Pealkiri | Pealkiri |` toruvormingus.

## Koosta & testi käsud

| Tegevus | Käsk |
|--------|---------|
| **Python näited** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS näited** | `cd javascript && npm install && node <script>.mjs` |
| **C# näited** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (veeb)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (veeb)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Diagrammide genereerimine** | `npx mmdc -i <input>.mmd -o <output>.svg` (nõuab globaalselt `npm install`) |

## Välised sõltuvused

- **Foundry Local CLI** peab olema arendaja masinas paigaldatud (`winget install Microsoft.FoundryLocal` või `brew install foundrylocal`).
- **Foundry Local teenus** töötab lokaalselt ja avaldab OpenAI-ga ühilduva REST API dünaamilisel pordil.
- Pilveteenuseid, API võtmeid ega Azure tellimusi pole ühegi näite käivitamiseks vaja.
- Osa 10 (kohandatud mudelid) vajab lisaks `onnxruntime-genai` ja laadib mudeli kaalud alla Hugging Face'ist.

## Failid, mida ei tohiks versioonihaldusesse panna

Fail `.gitignore` välistab (ja välistab enamikku):
- `.venv/` — Python virtuaalkeskkonnad
- `node_modules/` — npm sõltuvused
- `models/` — kompileeritud ONNX mudeliväljundid (suured binaarfailid, genereeritud Osa 10 poolt)
- `cache_dir/` — Hugging Face mudelite vahemälu
- `.olive-cache/` — Microsoft Olive töökataloog
- `samples/audio/*.wav` — genereeritud helinäited (uuesti genereeritav käsuga `python samples/audio/generate_samples.py`)
- Standard Python ehituse vahefailid (`__pycache__/`, `*.egg-info/`, `dist/` jne)

## Litsents

MIT — vaata faili `LICENSE`.