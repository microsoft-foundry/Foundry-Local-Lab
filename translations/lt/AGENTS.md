# Kodo agento instrukcijos

Šis failas pateikia kontekstą AI kodo agentams (GitHub Copilot, Copilot Workspace, Codex ir pan.), dirbantiems šiame saugykloje.

## Projekto apžvalga

Tai **praktinis dirbtuvinių darbų seminaras**, skirtas AI programų kūrimui naudojant [Foundry Local](https://foundrylocal.ai) — lengvą vykdymo aplinką, kuri atsisiunčia, valdo ir aptarnauja kalbos modelius pilnai lokaliai per OpenAI suderinamą API. Seminare yra žingsnis po žingsnio laboratoriniai vadovai ir paleidžiamų pavyzdžių kodas Python, JavaScript ir C# kalbomis.

## Saugyklos struktūra

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

## Kalbų ir karkasų informacija

### Python
- **Vieta:** `python/`, `zava-creative-writer-local/src/api/`
- **Priklausomybės:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Pagrindinės bibliotekos:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Minimaliai reikalinga versija:** Python 3.9+
- **Paleidimas:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Vieta:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Priklausomybės:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Pagrindinės bibliotekos:** `foundry-local-sdk`, `openai`
- **Modulių sistema:** ES moduliai (`.mjs` failai, `"type": "module"`)
- **Minimaliai reikalinga versija:** Node.js 18+
- **Paleidimas:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Vieta:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Projekto failai:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Pagrindinės paketai:** `Microsoft.AI.Foundry.Local` (ne Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — papildoma su QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Tikslinė platforma:** .NET 9.0 (sąlyginis TFM: `net9.0-windows10.0.26100` Windows, `net9.0` kitur)
- **Paleidimas:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Kodo rašymo konvencijos

### Bendra
- Visi kodo pavyzdžiai yra **vieno failo savarankiški pavyzdžiai** — be bendrų pagalbinių bibliotekų ar abstrakcijų.
- Kiekvienas pavyzdys paleidžiamas savarankiškai po priklausomybių įdiegimo.
- API raktai visada yra nustatyti į `"foundry-local"` — Foundry Local tai naudoja kaip vietos užpildą.
- Baziniai URL yra `http://localhost:<port>/v1` — portas dinamiškas ir randamas vykdymo metu per SDK (`manager.urls[0]` JavaScripte, `manager.endpoint` Python).
- Foundry Local SDK tvarko paslaugos paleidimą ir galinių taškų atradimą; rinkitės SDK šablonus vietoj hardkoduotų portų.

### Python
- Naudokite `openai` SDK su `OpenAI(base_url=..., api_key="not-required")`.
- Naudokite `FoundryLocalManager()` iš `foundry_local` SDK valdomam paslaugos ciklui.
- Srautas: iteruokite per `stream` objektą su `for chunk in stream:`.
- Pavyzdžiuose nenaudokite tipo anotacijų (kad pavyzdžiai būtų trumpi ir aiškūs seminaro dalyviams).

### JavaScript
- ES modulio sintaksė: `import ... from "..."`.
- Naudokite `OpenAI` iš `"openai"` ir `FoundryLocalManager` iš `"foundry-local-sdk"`.
- SDK inicializavimo šablonas: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Srautas: `for await (const chunk of stream)`.
- Visoje programoje naudojamas top-level `await`.

### C#
- Nullable įjungtas, implicit usings, .NET 9.
- Naudokite `FoundryLocalManager.StartServiceAsync()` SDK valdomam gyvenimo ciklui.
- Srautas: `CompleteChatStreaming()` su `foreach (var update in completionUpdates)`.
- Pagrindinis `csharp/Program.cs` yra CLI maršrutizatorius, kviečiantis statinius `RunAsync()` metodus.

### Įrankių kvietimas
- Įrankių kvietimus palaiko tik tam tikri modeliai: **Qwen 2.5** šeima (`qwen2.5-*`) ir **Phi-4-mini** (`phi-4-mini`).
- Įrankių schemos atitinka OpenAI funkcijų kvietimo JSON formatą (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Pokalbis vyksta daugiažingsniu režimu: vartotojas → asistentas (tool_calls) → įrankis (rezultatai) → asistentas (galutinis atsakymas).
- `tool_call_id` įrankio rezultatų žinutėse turi atitikti `id`, gautą iš modeliui skirtos įrankio užklausos.
- Python naudoja OpenAI SDK tiesiogiai; JavaScript naudoja SDK gimtąjį `ChatClient` (`model.createChatClient()`); C# naudoja OpenAI SDK su `ChatTool.CreateFunctionTool()`.

### ChatClient (gimtasis SDK klientas)
- JavaScript: `model.createChatClient()` grąžina `ChatClient` su metodais `completeChat(messages, tools?)` ir `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` grąžina standartinį `ChatClient`, kurį galima naudoti be OpenAI NuGet paketo importavimo.
- Python neturi gimtojo ChatClient — naudokite OpenAI SDK su `manager.endpoint` ir `manager.api_key`.
- **Svarbu:** JavaScript `completeStreamingChat` naudoja **callback šabloną**, o ne async iteraciją.

### Loginio samprotavimo modeliai
- `phi-4-mini-reasoning` apgaubia samprotavimą `<think>...</think>` žymomis prieš galutinį atsakymą.
- Jei reikia, išskirkite žymas, kad atskirti samprotavimą nuo atsakymo.

## Laboratoriniai vadovai

Laboratorijų failai yra `labs/` kaip Markdown. Jie turi nuoseklią struktūrą:
- Logotipo antraštės paveikslėlis
- Pavadinimas ir tikslo išskyrimas
- Apžvalga, mokymosi tikslai, išankstinės sąlygos
- Koncepcijų paaiškinimų skyriai su diagramomis
- Numeruoti pratimai su kodo blokais ir laukiamu rezultatu
- Santraukos lentelė, pagrindinės išvados, tolesnis skaitymas
- Navigacijos nuoroda į kitą dalį

Redaguojant laboratorijų turinį:
- Išlaikykite esamą Markdown formatavimo stilių ir skyrių hierarchiją.
- Kodo blokai turi nurodyti kalbą (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Pateikite tiek bash, tiek PowerShell variantus komandų, kai tai svarbu OS.
- Naudokite išskyrimus `> **Note:**`, `> **Tip:**`, `> **Troubleshooting:**`.
- Lentelės naudoja vamzdelio formato (`| Antraštė | Antraštė |`).

## Kūrimo ir testavimo komandos

| Veiksmas | Komanda |
|--------|---------|
| **Python pavyzdžiai** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS pavyzdžiai** | `cd javascript && npm install && node <script>.mjs` |
| **C# pavyzdžiai** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Diagramų generavimas** | `npx mmdc -i <input>.mmd -o <output>.svg` (reikalauja root `npm install`) |

## Išorinės priklausomybės

- **Foundry Local CLI** turi būti įdiegtas kūrėjo mašinoje (`winget install Microsoft.FoundryLocal` arba `brew install foundrylocal`).
- **Foundry Local paslauga** veikia lokaliai ir pateikia OpenAI suderinamą REST API ant dinaminio porto.
- Nereikia jokių debesų paslaugų, API raktų ar Azure prenumeratos bet kurio pavyzdžio paleidimui.
- 10 dalis (adaptuoti modeliai) papildomai reikalauja `onnxruntime-genai` ir atsisiunčia modelio svorius iš Hugging Face.

## Failai, kurie neturi būti įtraukti į versijos valdymą

`.gitignore` turėtų (ir dauguma jos taip ir daro) išskirti:
- `.venv/` — Python virtualios aplinkos
- `node_modules/` — npm priklausomybės
- `models/` — kompiliuotas ONNX modelio išvestis (didelio dydžio binariniai failai, generuoja 10 dalis)
- `cache_dir/` — Hugging Face modelio atsisiuntimo talpykla
- `.olive-cache/` — Microsoft Olive darbo katalogas
- `samples/audio/*.wav` — sugeneruoti garso pavyzdžiai (pergeneruojami komanda `python samples/audio/generate_samples.py`)
- Standartiniai Python kūrimo artefaktai (`__pycache__/`, `*.egg-info/`, `dist/` ir kt.)

## Licencija

MIT — žr. `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Atsakomybės apribojimas**:  
Šis dokumentas buvo išverstas naudojant AI vertimo paslaugą [Co-op Translator](https://github.com/Azure/co-op-translator). Nors siekiame tikslumo, atkreipkite dėmesį, kad automatiniai vertimai gali turėti klaidų ar netikslumų. Originalus dokumentas jo gimtąja kalba turėtų būti laikomas autoritetingu šaltiniu. Dėl svarbios informacijos rekomenduojamas profesionalus žmogaus vertimas. Mes neatsakome už bet kokius nesusipratimus ar neteisingas interpretacijas, kylančias dėl šio vertimo naudojimo.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->