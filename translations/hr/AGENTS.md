# Upute za kodirajućeg agenta

Ova datoteka pruža kontekst za AI kodirajuće agente (GitHub Copilot, Copilot Workspace, Codex, itd.) koji rade u ovom spremištu.

## Pregled projekta

Ovo je **praksa radionica** za izradu AI aplikacija s [Foundry Local](https://foundrylocal.ai) — laganim runtime okruženjem koje preuzima, upravlja i posluje jezičnim modelima potpuno na uređaju putem OpenAI-kompatibilnog API-ja. Radionica uključuje vodiče korak-po-korak i izvršive primjere koda u Pythonu, JavaScriptu i C#.

## Struktura spremišta

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

## Detalji o jeziku i okviru

### Python
- **Lokacija:** `python/`, `zava-creative-writer-local/src/api/`
- **Ovisnosti:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Ključni paketi:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Minimalna verzija:** Python 3.9+
- **Pokretanje:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Lokacija:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Ovisnosti:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Ključni paketi:** `foundry-local-sdk`, `openai`
- **Sustav modula:** ES moduli (`.mjs` datoteke, `"type": "module"`)
- **Minimalna verzija:** Node.js 18+
- **Pokretanje:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Lokacija:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Projektne datoteke:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Ključni paketi:** `Microsoft.AI.Foundry.Local` (non-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superset s QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Cilj:** .NET 9.0 (uvjetni TFM: `net9.0-windows10.0.26100` na Windowsima, `net9.0` drugdje)
- **Pokretanje:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Pravila kodiranja

### Općenito
- Svi primjeri koda su **samostalni i u jednoj datoteci** — bez zajedničkih pomoćnih biblioteka ili apstrakcija.
- Svaki primjer se pokreće neovisno nakon instaliranja vlastitih ovisnosti.
- API ključevi su uvijek postavljeni na `"foundry-local"` — Foundry Local koristi ovo kao privremeni ključ.
- Osnovni URL-ovi koriste `http://localhost:<port>/v1` — port je dinamičan i otkriva se pri izvođenju putem SDK-a (`manager.urls[0]` u JS-u, `manager.endpoint` u Pythonu).
- Foundry Local SDK upravlja pokretanjem servisa i otkrivanjem endpointa; preferirajte SDK obrasce umjesto hardkodiranih portova.

### Python
- Koristite `openai` SDK s `OpenAI(base_url=..., api_key="not-required")`.
- Koristite `FoundryLocalManager()` iz `foundry_local` za upravljanje životnim ciklusom servisa putem SDK-a.
- Streaming: iterirajte preko `stream` objekta sa `for chunk in stream:`.
- Nema tipizacija u primjerima (da bi primjeri bili sažetiji za polaznike radionice).

### JavaScript
- ES modul sintaksa: `import ... from "..."`.
- Koristite `OpenAI` iz `"openai"` i `FoundryLocalManager` iz `"foundry-local-sdk"`.
- SDK inicijalizacijski obrazac: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Streaming: `for await (const chunk of stream)`.
- Top-level `await` se koristi posvuda.

### C#
- Nullable omogućen, implicitne using direktive, .NET 9.
- Koristite `FoundryLocalManager.StartServiceAsync()` za upravljanje životnim ciklusom putem SDK-a.
- Streaming: `CompleteChatStreaming()` s `foreach (var update in completionUpdates)`.
- Glavna datoteka `csharp/Program.cs` je CLI preusmjerivač koji poziva statične `RunAsync()` metode.

### Pozivanje alata
- Alate podržavaju samo neke verzije modela: **Qwen 2.5** serija (`qwen2.5-*`) i **Phi-4-mini** (`phi-4-mini`).
- Šeme alata slijede OpenAI JSON format za pozivanje funkcija (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Razgovor koristi višekratni tok: korisnik → asistent (tool_calls) → alat (rezultati) → asistent (konačni odgovor).
- `tool_call_id` u porukama alata mora se podudarati s `id` iz poziva modela za alat.
- Python koristi OpenAI SDK direktno; JavaScript koristi SDK-ov nativni `ChatClient` (`model.createChatClient()`); C# koristi OpenAI SDK s `ChatTool.CreateFunctionTool()`.

### ChatClient (Nativni SDK klijent)
- JavaScript: `model.createChatClient()` vraća `ChatClient` s metodama `completeChat(messages, tools?)` i `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` vraća standardni `ChatClient` koji se može koristiti bez uvoza OpenAI NuGet paketa.
- Python nema nativni ChatClient — koristi se OpenAI SDK s `manager.endpoint` i `manager.api_key`.
- **Važno:** JavaScript `completeStreamingChat` koristi **callback obrazac**, ne async iteraciju.

### Razmišljajući modeli
- `phi-4-mini-reasoning` obavija svoje razmišljanje u `<think>...</think>` oznake prije konačnog odgovora.
- Oznake se parsiraju za odvajanje razmišljanja od odgovora ako je potrebno.

## Vodiči za laboratorijske vježbe

Laboratorijske datoteke su u `labs/` kao Markdown. Imaju stalnu strukturu:
- Logo kao zaglavlje slike
- Naslov i ciljni poziv
- Pregled, Ciljevi učenja, Preduvjeti
- Sekcije s konceptima i dijagramima
- Numerirane vježbe s blokovima koda i očekivanim ispisom
- Sažetna tablica, Ključni zaključci, Daljnje čitanje
- Navigacijska poveznica na sljedeći dio

Kod uređivanja sadržaja laboratorijskih vježbi:
- Očuvajte postojeći Markdown format i hijerarhiju sekcija.
- Blokovi koda moraju specificirati jezik (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Dajte i bash i PowerShell varijante za naredbe ljuske gdje računa OS.
- Koristite stilove poziva `> **Note:**`, `> **Tip:**`, i `> **Troubleshooting:**`.
- Tablice koriste cjevasti format `| Zaglavlje | Zaglavlje |`.

## Naredbe za izgradnju i testiranje

| Radnja | Naredba |
|--------|---------|
| **Python primjeri** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS primjeri** | `cd javascript && npm install && node <script>.mjs` |
| **C# primjeri** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (web)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (web)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generiranje dijagrama** | `npx mmdc -i <input>.mmd -o <output>.svg` (zahtijeva root `npm install`) |

## Vanjske ovisnosti

- **Foundry Local CLI** mora biti instaliran na razvojnog računalu (`winget install Microsoft.FoundryLocal` ili `brew install foundrylocal`).
- **Foundry Local servis** radi lokalno i izlaže OpenAI-kompatibilni REST API na dinamičkom portu.
- Nije potrebna nijedna cloud usluga, API ključevi ili Azure pretplate za pokretanje primjera.
- Dio 10 (prilagođeni modeli) dodatno zahtijeva `onnxruntime-genai` i preuzima težine modela s Hugging Facea.

## Datoteke koje se ne bi trebale komitirati

`.gitignore` bi trebao isključiti (i već isključuje uglavnom):
- `.venv/` — Python virtualna okruženja
- `node_modules/` — npm paketi
- `models/` — kompajlirane ONNX model datoteke (velike binarne datoteke, generirane u dijelu 10)
- `cache_dir/` — keš preuzimanja modela s Hugging Facea
- `.olive-cache/` — radni direktorij Microsoft Olive
- `samples/audio/*.wav` — generirani audio primjeri (obnavljaju se putem `python samples/audio/generate_samples.py`)
- Standardni Python build artefakti (`__pycache__/`, `*.egg-info/`, `dist/`, itd.)

## Licenca

MIT — vidi `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Odricanje od odgovornosti**:
Ovaj dokument je preveden korištenjem AI prevoditeljskog servisa [Co-op Translator](https://github.com/Azure/co-op-translator). Iako težimo točnosti, imajte na umu da automatski prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na izvornom jeziku treba smatrati službenim i autoritativnim izvorom. Za kritične informacije preporučuje se profesionalni ljudski prijevod. Ne snosimo odgovornost za bilo kakva nerazumijevanja ili pogrešne interpretacije nastale uporabom ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->