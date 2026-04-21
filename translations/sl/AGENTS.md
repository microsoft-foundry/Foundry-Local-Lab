# Navodila za kodirnega agenta

Ta datoteka nudi kontekst za AI kodirne agente (GitHub Copilot, Copilot Workspace, Codex itd.), ki delajo v tem repozitoriju.

## Pregled projekta

To je **praktična delavnica** za izdelavo AI aplikacij z [Foundry Local](https://foundrylocal.ai) — lahko runtime, ki prenese, upravlja in streže jezikovne modele povsem na napravi preko API-ja združljivega z OpenAI. Delavnica vključuje vodnike po korakih in zagnljive primere kode v Pythonu, JavaScriptu in C#.

## Struktura repozitorija

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

## Podrobnosti o jeziku in ogrodju

### Python
- **Lokacija:** `python/`, `zava-creative-writer-local/src/api/`
- **Odvisnosti:** `python/requirements.txt`, `zava-creative-writer-local/src/api/requirements.txt`
- **Ključne knjižnice:** `foundry-local-sdk`, `openai`, `agent-framework-foundry-local`, `fastapi`, `uvicorn`
- **Min različica:** Python 3.9+
- **Zagon:** `cd python && pip install -r requirements.txt && python foundry-local.py`

### JavaScript
- **Lokacija:** `javascript/`, `zava-creative-writer-local/src/javascript/`
- **Odvisnosti:** `javascript/package.json`, `zava-creative-writer-local/src/javascript/package.json`
- **Ključne knjižnice:** `foundry-local-sdk`, `openai`
- **Modulni sistem:** ES moduli (`.mjs` datoteke, `"type": "module"`)
- **Min različica:** Node.js 18+
- **Zagon:** `cd javascript && npm install && node foundry-local.mjs`

### C#
- **Lokacija:** `csharp/`, `zava-creative-writer-local/src/csharp/`
- **Projektne datoteke:** `csharp/csharp.csproj`, `zava-creative-writer-local/src/csharp/ZavaCreativeWriter.csproj`
- **Ključne knjižnice:** `Microsoft.AI.Foundry.Local` (ne-Windows), `Microsoft.AI.Foundry.Local.WinML` (Windows — superskup z QNN EP), `OpenAI`, `Microsoft.Agents.AI.OpenAI`
- **Cilj:** .NET 9.0 (pogojni TFM: `net9.0-windows10.0.26100` na Windows, `net9.0` drugje)
- **Zagon:** `cd csharp && dotnet run [chat|rag|agent|multi]`

## Pravila kodiranja

### Splošno
- Vsi primeri kode so **samo-vsebujoči primeri ene datoteke** — brez skupnih pomožnih knjižnic ali abstrakcij.
- Vsak primer se izvaja neodvisno po namestitvi svojih odvisnosti.
- API ključi so vedno nastavljeni na `"foundry-local"` — Foundry Local uporablja to kot nadomestni znak.
- Osnovni URL naslovi so `http://localhost:<port>/v1` — vrata so dinamična in odkrita med izvajanjem preko SDK (`manager.urls[0]` v JS, `manager.endpoint` v Pythonu).
- Foundry Local SDK upravlja z zagonom storitve in odkrivanjem končne točke; raje uporabljajte SDK vzorce kot trdo kodirane porte.

### Python
- Uporabljajte `openai` SDK z `OpenAI(base_url=..., api_key="not-required")`.
- Uporabljajte `FoundryLocalManager()` iz `foundry_local` za življenjski cikel storitve v upravljanju SDK.
- Za pretočni prenos: iterirajte preko objekta `stream` s konstrukcijo `for chunk in stream:`.
- Brez tipizacij v vzorčnih datotekah (primeri naj bodo kratki za udeležence delavnice).

### JavaScript
- Sintaksa ES modula: `import ... from "..."`.
- Uporabljajte `OpenAI` iz `"openai"` in `FoundryLocalManager` iz `"foundry-local-sdk"`.
- Vzorec inicializacije SDK: `FoundryLocalManager.create({ appName })` → `FoundryLocalManager.instance` → `manager.startWebService()` → `await catalog.getModel(alias)`.
- Za pretočni prenos: `for await (const chunk of stream)`.
- Uporablja se nivojski `await` po celotnem kodu.

### C#
- Omogočen nullable, implicitni usings, .NET 9.
- Uporabljajte `FoundryLocalManager.StartServiceAsync()` za SDK upravljanje življenjskega cikla.
- Za pretočni prenos: `CompleteChatStreaming()` s `foreach (var update in completionUpdates)`.
- Glavna `csharp/Program.cs` je CLI usmerjevalnik, ki kliče statične metode `RunAsync()`.

### Klic orodij
- Samo določeni modeli podpirajo klice orodij: družina **Qwen 2.5** (`qwen2.5-*`) in **Phi-4-mini** (`phi-4-mini`).
- Sheme orodij uporabljajo OpenAI JSON format klicanja funkcij (`type: "function"`, `function.name`, `function.description`, `function.parameters`).
- Pogovor uporablja vzorec z več zavoji: uporabnik → asistent (klic_orodja) → orodje (rezultati) → asistent (končni odgovor).
- `tool_call_id` v sporočilih z rezultati orodja mora ustrezati `id` klica orodja modela.
- Python uporablja OpenAI SDK neposredno; JavaScript uporablja izvorni SDK `ChatClient` (`model.createChatClient()`); C# uporablja OpenAI SDK s `ChatTool.CreateFunctionTool()`.

### ChatClient (nativni SDK odjemalec)
- JavaScript: `model.createChatClient()` vrne `ChatClient` z metodama `completeChat(messages, tools?)` in `completeStreamingChat(messages, callback)`.
- C#: `model.GetChatClientAsync()` vrne standardni `ChatClient`, ki ga lahko uporabite brez uvoza OpenAI NuGet paketa.
- Python nima nativnega ChatClienta — uporabite OpenAI SDK z `manager.endpoint` in `manager.api_key`.
- **Pomembno:** JavaScript `completeStreamingChat` uporablja **callback vzorec**, ne asinhrono iteriranje.

### Modeli za razmišljanje
- `phi-4-mini-reasoning` ovije svoje razmišljanje v `<think>...</think>` oznake pred končnim odgovorom.
- Po potrebi analizirajte oznake, da ločite razmišljanje od odgovora.

## Vodniki laboratorija

Datoteke laboratorija so v `labs/` kot Markdown in sledijo dosledni strukturi:
- Slika z glavo logotipa
- Naslov in ciljni poziv
- Pregled, cilji učenja, predpogoji
- Razlagalne sekcije s shemami
- Oštevilčeni izzivi s kodo in pričakovanim izhodom
- Povzetna tabela, ključne ugotovitve, dodatno branje
- Povezava za nadaljevanje na naslednji del

Pri urejanju vsebine laboratorija:
- Ohranjajte obstoječi slog oblikovanja Markdown in hierarhijo sekcij.
- Bloki kode naj vsebujejo specifikacijo jezika (`python`, `javascript`, `csharp`, `bash`, `powershell`).
- Za ukaze v terminalu zagotovite tako bash kot PowerShell različice, kjer je pomembno za OS.
- Uporabljajte sloge klicev `> **Note:**`, `> **Tip:**`, in `> **Troubleshooting:**`.
- Tabele uporabljajo `| Header | Header |` format s cevjo.

## Ukazi za gradnjo in testiranje

| Dejanje | Ukaz |
|--------|---------|
| **Python primeri** | `cd python && pip install -r requirements.txt && python <script>.py` |
| **JS primeri** | `cd javascript && npm install && node <script>.mjs` |
| **C# primeri** | `cd csharp && dotnet run [chat\|rag\|agent\|multi\|eval\|whisper\|toolcall]` |
| **Zava Python** | `cd zava-creative-writer-local/src/api && pip install -r requirements.txt && uvicorn main:app` |
| **Zava JS** | `cd zava-creative-writer-local/src/javascript && npm install && node main.mjs` |
| **Zava JS (splet)** | `cd zava-creative-writer-local/src/javascript && npm install && node server.mjs` |
| **Zava C#** | `cd zava-creative-writer-local/src/csharp && dotnet run` |
| **Zava C# (splet)** | `cd zava-creative-writer-local/src/csharp-web && dotnet run` |
| **Foundry Local CLI** | `foundry model list`, `foundry model run <model>`, `foundry service status` |
| **Generiranje diagramov** | `npx mmdc -i <input>.mmd -o <output>.svg` (zahteva globalno `npm install`) |

## Zunanji odvisniki

- **Foundry Local CLI** mora biti nameščen na razvijalčevem računalniku (`winget install Microsoft.FoundryLocal` ali `brew install foundrylocal`).
- **Foundry Local služba** teče lokalno in ponuja OpenAI združljiv REST API na dinamičnem portu.
- Ni potrebnih nobenih oblačnih storitev, API ključev ali Azure naročnin za zagon primerov.
- Del 10 (lastni modeli) dodatno zahteva `onnxruntime-genai` in prenaša uteži modela iz Hugging Face.

## Datoteke, ki jih ne smete vključiti v repozitorij

`.gitignore` izključuje (in večinoma že izključuje):
- `.venv/` — Python virtualna okolja
- `node_modules/` — npm odvisnosti
- `models/` — izhod prevajanih ONNX modelov (velike binarne datoteke, generirane v Delu 10)
- `cache_dir/` — predpomnilnik prenosa modelov Hugging Face
- `.olive-cache/` — delovni imenik Microsoft Olive
- `samples/audio/*.wav` — generirani zvočni primeri (ponovno generirani preko `python samples/audio/generate_samples.py`)
- Standardni Python artefakti gradnje (`__pycache__/`, `*.egg-info/`, `dist/`, itd.)

## Licenca

MIT — glej `LICENSE`.

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Omejitev odgovornosti**:  
Ta dokument je bil preveden z uporabo storitve za strojno prevajanje [Co-op Translator](https://github.com/Azure/co-op-translator). Čeprav si prizadevamo za natančnost, upoštevajte, da avtomatizirani prevodi lahko vsebujejo napake ali netočnosti. Izvirni dokument v svojem izvirnem jeziku velja za avtoritativni vir. Za kritične informacije priporočamo strokovni človeški prevod. Ne odgovarjamo za morebitna nesporazume ali napačne interpretacije, ki izhajajo iz uporabe tega prevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->