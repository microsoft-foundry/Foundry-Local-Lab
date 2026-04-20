# Instrucțiuni pentru Agentul de Codare

Acest fișier oferă context pentru agenții AI de codare (GitHub Copilot, Copilot Workspace, Codex, etc.) care lucrează în acest depozit.

## Prezentare Generală a Proiectului

Acesta este un **atelier practic** pentru construirea aplicațiilor AI cu [Foundry Local](https://foundrylocal.ai) — un runtime ușor care descarcă, gestionează și servește modele de limbaj complet local, printr-un API compatibil OpenAI. Atelierul include ghiduri pas-cu-pas și mostre de cod rulabile în Python, JavaScript și C#.

## Structura Depozitului

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

## Detalii despre Limbaje & Framework-uri

### Python
- **Locație:** `python/`, `zava-creative-writer-local/src/api/`
- **Dependențe:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Pachete cheie:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Versiunea minimă:** Python 3.9+
- **Rulare:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Locație:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Dependențe:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Pachete cheie:** `foundry-local-sdk`, `openai`
- **Sistem de module:** Module ES (`.mjs` fișiere, `"type": "module"`)
- **Versiunea minimă:** Node.js 18+
- **Rulare:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Locație:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Fișiere proiect:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Pachete cheie:** `Microsoft.AI.Foundry.Local` (non-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset cu QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Țintă:** .NET 9.0 (TFM condițional: `net9.0-windows10.0.26100` pe Windows, `net9.0` altundeva)
- **Rulare:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Convenții de Codare

### General
- Toate exemplele de cod sunt **exemple autonome într-un singur fișier** — fără biblioteci utilitare partajate sau abstractizări.
- Fiecare exemplu rulează independent după instalarea dependențelor proprii.
- Cheile API sunt setate întotdeauna la `"foundry-local"` — Foundry Local folosește aceasta ca valoare de substitut.
- URL-urile de bază folosesc `http://localhost:<port>/v1` — portul este dinamic și descoperit la runtime prin SDK (`manager.urls[0]` în JS, `manager.endpoint` în Python).
- SDK-ul Foundry Local gestionează pornirea serviciului și descoperirea endpoint-ului; preferați tiparele SDK în locul porturilor hardcodate.

### Python
- Folosiți SDK `openai` cu `OpenAI(base_url=..., api_key="not-required")`.
- Folosiți `FoundryLocalManager()` din `foundry_local` pentru ciclul de viață gestionat al serviciului prin SDK.
- Streaming: iterați asupra obiectului `stream` cu `for chunk in stream:`.
- Nu se folosesc adnotări de tip în fișierele exemple (pentru concizie la cursanți).

### JavaScript
- Sintaxă modul ES: `import ... from "..."`.
- Folosiți `OpenAI` din `"openai"` și `FoundryLocalManager` din `"foundry-local-sdk"`.
- Tipar inițializare SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- `await` de nivel înalt este folosit pe tot parcursul.

### C#
- Nullable activat, using-uri implicite, .NET 9.
- Folosiți `FoundryLocalManager.StartServiceAsync()` pentru ciclul de viață gestionat prin SDK.
- Streaming: `CompleteChatStreaming()` cu `foreach (var update in completionUpdates)`.
- `csharp/Program.cs` principal este un router CLI care distribuie la metode statice `RunAsync()`.

### Apelul de Unelte (Tool Calling)
- Doar anumite modele suportă apelul de unelte: familia **Qwen 2.5** (`qwen2.5-*`) și **Phi-4-mini** (`phi-4-mini`).
- Schemele uneltelor urmează formatul JSON OpenAI pentru funcții (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Conversația folosește un pattern multi-turn: utilizator → asistent (tool_calls) → unealtă (rezultate) → asistent (răspuns final).
- `tool_call_id` în mesajele cu rezultatul uneltei trebuie să corespundă `id` din apelul de unealtă al modelului.
- Python folosește SDK-ul OpenAI direct; JavaScript folosește `ChatClient` nativ SDK (`model.createChatClient()`); C# folosește SDK-ul OpenAI cu `ChatTool.CreateFunctionTool()`.

### ChatClient (Client SDK Nativ)
- JavaScript: `model.createChatClient()` returnează un `ChatClient` cu `completeChat(messages, tools?)` și `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` returnează un `ChatClient` standard, folosit fără importul pachetului NuGet OpenAI.
- Python nu are un ChatClient nativ — folosiți SDK-ul OpenAI cu `manager.endpoint` și `manager.api_key`.
- **Important:** JavaScript `completeStreamingChat` folosește un **pattern callback**, nu iterație asincronă.

### Modele de Raționament
- `phi-4-mini-reasoning` încadrează gândirea între tagurile `<think>...</think>` înainte de răspunsul final.
- Parcurgeți tagurile pentru a separa raționamentul de răspuns când este necesar.

## Ghiduri de Laborator

Fișierele de laborator se află în `labs/` în format Markdown. Urmează o structură consecventă:
- Imagine logo în antet
- Titlu și scop
- Prezentare generală, Obiective de învățare, Precondiții
- Secțiuni de explicații cu diagrame
- Exerciții numerotate cu blocuri de cod și ieșiri așteptate
- Tabel sumar, Puncte cheie, Lecturi suplimentare
- Link de navigare către partea următoare

La editarea conținutului de laborator:
- Menține stilul de formatare Markdown și ierarhia secțiunilor.
- Blocurile de cod trebuie să specifice limbajul (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Oferiți variante bash și PowerShell pentru comenzile shell când sistemul de operare contează.
- Folosiți apeluri de tip `> **Note:**`, `> **Tip:**`, și `> **Depanare:**`.
- Tabelele respectă formatul pipe `| Antet | Antet |`.

## Comenzi de Construire & Testare

| Acțiune | Comandă |
|--------|---------|
| **Exemple Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **Exemple JS** | `cd javascript && npm install && node <script>.mjs` |
| **Exemple C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generare diagrame** | `npx mmdc -i <input>.mmd -o <output>.svg` (necesită `npm install` la nivel root) |

## Dependențe Externe

- **CLI Foundry Local** trebuie instalat pe mașina dezvoltatorului (`winget install Microsoft.FoundryLocal` sau `brew install foundrylocal`).
- **Serviciul Foundry Local** rulează local și expune un API REST OpenAI-compatibil pe un port dinamic.
- Nu sunt necesare servicii cloud, chei API sau abonamente Azure pentru a rula vreun exemplu.
- Partea 10 (modele custom) necesită suplimentar `onnxruntime-genai` și descarcă greutăți de model de pe Hugging Face.

## Fișiere Care Nu Trebuie Comise

Fișierul `.gitignore` exclude (și exclude pentru majoritatea):
- `.venv/` — medii virtuale Python
- `node_modules/` — dependențe npm
- `models/` — ieșiri compilate modele ONNX (fișiere binare mari, generate în Partea 10)
- `cache_dir/` — cache pentru descărcarea modelelor Hugging Face
- `.olive-cache/` — director de lucru Microsoft Olive
- `samples/audio/*.wav` — mostre audio generate (regenerate cu `python samples/audio/generate_samples.py`)
- Articole standard de construcție Python (`__pycache__/`, `*.egg-info/`, `dist/`, etc.)

## Licență

MIT — vezi `LICENSE`.