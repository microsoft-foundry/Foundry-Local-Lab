# Istruzioni per l'Agente di Codifica

Questo file fornisce il contesto per gli agenti di codifica AI (GitHub Copilot, Copilot Workspace, Codex, ecc.) che lavorano in questo repository.

## Panoramica del Progetto

Questo è un **workshop pratico** per costruire applicazioni AI con [Foundry Local](https://foundrylocal.ai) — un runtime leggero che scarica, gestisce e serve modelli di linguaggio interamente sul dispositivo tramite un’API compatibile OpenAI. Il workshop include guide passo-passo e esempi di codice eseguibili in Python, JavaScript e C#.

## Struttura del Repository

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

## Dettagli su Linguaggi e Framework

### Python
- **Posizione:** `python/`, `zava-creative-writer-local/src/api/`
- **Dipendenze:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Pacchetti chiave:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Versione minima:** Python 3.9+
- **Esecuzione:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Posizione:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Dipendenze:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Pacchetti chiave:** `foundry-local-sdk`, `openai`
- **Sistema di moduli:** Moduli ES (`.mjs` files, `"type": "module"`)
- **Versione minima:** Node.js 18+
- **Esecuzione:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Posizione:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **File progetto:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Pacchetti chiave:** `Microsoft.AI.Foundry.Local` (non-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset con QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Target:** .NET 9.0 (TFM condizionale: `net9.0-windows10.0.26100` su Windows, `net9.0` altrove)
- **Esecuzione:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Convenzioni di Codifica

### Generale
- Tutti gli esempi di codice sono **esempi autonomi in un singolo file** — nessuna libreria o astrazione condivisa.
- Ogni esempio gira indipendentemente dopo aver installato le proprie dipendenze.
- Le chiavi API sono sempre impostate su `"foundry-local"` — Foundry Local usa questo come segnaposto.
- Gli URL base usano `http://localhost:<port>/v1` — la porta è dinamica e scoperta a runtime tramite l’SDK (`manager.urls[0]` in JS, `manager.endpoint` in Python).
- Il Foundry Local SDK gestisce l’avvio del servizio e la scoperta dell’endpoint; preferire i pattern SDK rispetto alle porte hard-coded.

### Python
- Usa l’SDK `openai` con `OpenAI(base_url=..., api_key="not-required")`.
- Usa `FoundryLocalManager()` da `foundry_local` per il ciclo di vita gestito dal SDK.
- Streaming: itera sull’oggetto `stream` con `for chunk in stream:`.
- Nessuna annotazione di tipo nei file di esempio (mantenere gli esempi concisi per i partecipanti al workshop).

### JavaScript
- Sintassi modulo ES: `import ... from "..."`.
- Usa `OpenAI` da `"openai"` e `FoundryLocalManager` da `"foundry-local-sdk"`.
- Pattern di inizializzazione SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Usato `await` a livello top-level ovunque.

### C#
- Nullable abilitato, implicit usings, .NET 9.
- Usa `FoundryLocalManager.StartServiceAsync()` per il ciclo di vita gestito dal SDK.
- Streaming: `CompleteChatStreaming()` con `foreach (var update in completionUpdates)`.
- Il principale `csharp/Program.cs` è un router CLI che richiama metodi statici `RunAsync()`.

### Chiamata a Tool
- Solo certi modelli supportano chiamate a tool: famiglia **Qwen 2.5** (`qwen2.5-*`) e **Phi-4-mini** (`phi-4-mini`).
- Gli schemi dei tool seguono il formato JSON di OpenAI per il function-calling (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- La conversazione usa un pattern multi-turno: utente → assistente (tool_calls) → tool (risultati) → assistente (risposta finale).
- Il `tool_call_id` nei messaggi di risultato del tool deve corrispondere all’`id` dalla chiamata a tool del modello.
- Python usa direttamente l’SDK OpenAI; JavaScript usa il `ChatClient` nativo dell’SDK (`model.createChatClient()`); C# usa l’SDK OpenAI con `ChatTool.CreateFunctionTool()`.

### ChatClient (Client SDK Nativo)
- JavaScript: `model.createChatClient()` restituisce un `ChatClient` con `completeChat(messages, tools?)` e `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` restituisce un `ChatClient` standard che può essere usato senza importare il pacchetto NuGet OpenAI.
- Python non ha un ChatClient nativo — usa l’SDK OpenAI con `manager.endpoint` e `manager.api_key`.
- **Importante:** in JavaScript `completeStreamingChat` usa un **pattern callback**, non iterazione asincrona.

### Modelli di Ragionamento
- `phi-4-mini-reasoning` incapsula il suo pensiero in tag `<think>...</think>` prima della risposta finale.
- Fare il parsing dei tag per separare ragionamento e risposta quando necessario.

## Guide Laboratorio

I file dei laboratori sono in `labs/` in Markdown. Seguono una struttura coerente:
- Immagine header logo
- Titolo e callout obiettivo
- Panoramica, Obiettivi di apprendimento, Prerequisiti
- Sezioni di spiegazione concetti con diagrammi
- Esercizi numerati con blocchi di codice e output previsto
- Tabella riassuntiva, Key Takeaways, Letture aggiuntive
- Link di navigazione alla parte successiva

Quando modifichi contenuti di laboratorio:
- Mantieni lo stile di formattazione Markdown esistente e la gerarchia delle sezioni.
- I blocchi di codice devono specificare il linguaggio (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Fornisci varianti bash e PowerShell per i comandi shell in base al sistema operativo.
- Usa callout stile `> **Note:**`, `> **Tip:**`, e `> **Troubleshooting:**`.
- Le tabelle usano il formato a pipe `| Header | Header |`.

## Comandi di Build & Test

| Azione | Comando |
|--------|---------|
| **Esempi Python** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **Esempi JS** | `cd javascript && npm install && node <script>.mjs` |
| **Esempi C#** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Genera diagrammi** | `npx mmdc -i <input>.mmd -o <output>.svg` (richiede root `npm install`) |

## Dipendenze Esterne

- **Foundry Local CLI** deve essere installato sul computer dello sviluppatore (`winget install Microsoft.FoundryLocal` o `brew install foundrylocal`).
- **Il servizio Foundry Local** gira localmente ed espone un’API REST compatibile OpenAI su una porta dinamica.
- Non sono necessari servizi cloud, chiavi API o abbonamenti Azure per eseguire qualunque esempio.
- La Parte 10 (modelli personalizzati) richiede inoltre `onnxruntime-genai` e scarica i pesi dei modelli da Hugging Face.

## File Che Non Devono Essere Committati

Il `.gitignore` deve escludere (ed esclude per la maggior parte):
- `.venv/` — ambienti virtuali Python
- `node_modules/` — dipendenze npm
- `models/` — output modelli ONNX compilati (file binari grandi, generati dalla Parte 10)
- `cache_dir/` — cache download modelli Hugging Face
- `.olive-cache/` — directory di lavoro Microsoft Olive
- `samples/audio/*.wav` — esempi audio generati (rigenerati via `python samples/audio/generate_samples.py`)
- Articoli standard di build Python (`__pycache__/`, `*.egg-info/`, `dist/`, ecc.)

## Licenza

MIT — vedi `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Avvertenza**:  
Questo documento è stato tradotto utilizzando il servizio di traduzione automatica [Co-op Translator](https://github.com/Azure/co-op-translator). Pur impegnandoci per l'accuratezza, si prega di notare che le traduzioni automatizzate possono contenere errori o inesattezze. Il documento originale nella sua lingua nativa deve essere considerato la fonte autorevole. Per informazioni critiche, si raccomanda una traduzione professionale umana. Non siamo responsabili per eventuali incomprensioni o interpretazioni errate derivanti dall'uso di questa traduzione.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->