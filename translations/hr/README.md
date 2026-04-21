<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Radionica - Izrada AI Aplikacija na Uređaju

Praktična radionica za pokretanje jezičnih modela na vašem vlastitom računalu i izradu inteligentnih aplikacija s [Foundry Local](https://foundrylocal.ai) i [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Što je Foundry Local?** Foundry Local je lagano runtime okruženje koje vam omogućuje preuzimanje, upravljanje i posluživanje jezičnih modela u potpunosti na vašem hardveru. Izlaže **OpenAI-kompatibilan API** tako da se bilo koji alat ili SDK koji podržava OpenAI može povezati – bez potrebe za cloud računom.

---

## Ciljevi učenja

Do kraja ove radionice moći ćete:

| # | Cilj |
|---|-----------|
| 1 | Instalirati Foundry Local i upravljati modelima putem CLI-a |
| 2 | Ovladati Foundry Local SDK API-em za programsko upravljanje modelima |
| 3 | Povezati se na lokalni inference server koristeći Python, JavaScript i C# SDK-e |
| 4 | Izgraditi Retrieval-Augmented Generation (RAG) pipeline koji temelji odgovore na vlastitim podacima |
| 5 | Kreirati AI agente s trajnim uputama i osobnostima |
| 6 | Orkestrirati višeagentne radne tokove s petljama povratnih informacija |
| 7 | Istražiti proizvodnu završnu aplikaciju - Zava Creative Writer |
| 8 | Izraditi okvire za evaluaciju sa zlatnim skupovima podataka i ocjenjivanjem LLM-as-judge |
| 9 | Transkribirati audio s Whisperom - govornu pretvorbu u tekst na uređaju koristeći Foundry Local SDK |
| 10 | Kompajlirati i pokretati prilagođene ili Hugging Face modele pomoću ONNX Runtime GenAI i Foundry Local |
| 11 | Omogućiti lokalnim modelima pozivanje vanjskih funkcija obrascem pozivanja alata |
| 12 | Izgraditi UI baziran na pregledniku za Zava Creative Writer s streamingom u stvarnom vremenu |

---

## Preduvjeti

| Zahtjev | Detalji |
|-------------|---------|
| **Hardver** | Minimalno 8 GB RAM (preporučeno 16 GB); CPU s AVX2 podrškom ili podržana GPU kartica |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025 ili macOS 13+ |
| **Foundry Local CLI** | Instalirajte preko `winget install Microsoft.FoundryLocal` (Windows) ili `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Pogledajte [voditelj za početak](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) za detalje. |
| **Jezično runtime okruženje** | **Python 3.9+** i/ili **.NET 9.0+** i/ili **Node.js 18+** |
| **Git** | Za kloniranje ovog repozitorija |

---

## Početak

```bash
# 1. Klonirajte spremište
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Provjerite je li Foundry Local instaliran
foundry model list              # Popis dostupnih modela
foundry model run phi-3.5-mini  # Pokrenite interaktivni chat

# 3. Odaberite svoj jezični smjer (pogledajte laboratorij 2. dijela za cjelokupnu postavu)
```

| Jezik | Brzi početak |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Dijelovi radionice

### Dio 1: Početak rada s Foundry Local

**Vodič za lab:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Što je Foundry Local i kako radi
- Instalacija CLI-a na Windows i macOS
- Istraživanje modela - popisivanje, preuzimanje, pokretanje
- Razumijevanje aliasa modela i dinamičkih portova

---

### Dio 2: Duboki pregled Foundry Local SDK-a

**Vodič za lab:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Zašto koristiti SDK umjesto CLI-a za razvoj aplikacija
- Potpuna SDK API referenca za Python, JavaScript i C#
- Upravljanje servisima, pregled kataloga, životni ciklus modela (preuzimanje, učitavanje, uklanjanje)
- Obrasci brzog početka: Python konstruktor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- `FoundryModelInfo` metapodaci, aliasi i odabir modela optimiziranog za hardver

---

### Dio 3: SDK-i i API-ji

**Vodič za lab:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Povezivanje na Foundry Local iz Python, JavaScript i C#
- Korištenje Foundry Local SDK-a za programsko upravljanje servisom
- Streamanje chat završetaka putem OpenAI-kompatibilnog API-ja
- Referenca metoda SDK-a za svaki jezik

**Primjeri koda:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Osnovni streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat s .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat pomoću Node.js |

---

### Dio 4: Retrieval-Augmented Generation (RAG)

**Vodič za lab:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Što je RAG i zašto je važan
- Izrada baze znanja u memoriji
- Dohvat podataka na temelju preklapanja ključnih riječi s bodovanjem
- Sastavljanje utemeljenih sistemskih upita
- Pokretanje kompletnog RAG pipeline-a na uređaju

**Primjeri koda:**

| Jezik | Datoteka |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Dio 5: Izrada AI Agenata

**Vodič za lab:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Što je AI agent (nasuprot izravnog poziva LLM-a)
- Obranik `ChatAgent` i Microsoft Agent Framework
- Sistemske upute, osobnosti i višekratni razgovori
- Strukturirani izlaz (JSON) od agenata

**Primjeri koda:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Jedan agent s Agent Frameworkom |
| C# | `csharp/SingleAgent.cs` | Jedan agent (ChatAgent obrazac) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Jedan agent (ChatAgent obrazac) |

---

### Dio 6: Višeagentni Radni Tokovi

**Vodič za lab:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Višeagentni pipeline: Istraživač → Pisac → Urednik
- Sekvencijalna orkestracija i petlje povratnih informacija
- Dijeljena konfiguracija i strukturirani prijelazi
- Dizajnirajte vlastiti višeagentni radni tok

**Primjeri koda:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline s tri agenta |
| C# | `csharp/MultiAgent.cs` | Pipeline s tri agenta |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline s tri agenta |

---

### Dio 7: Zava Creative Writer - Završna Aplikacija

**Vodič za lab:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Proizvodna višestruka aplikacija s 4 specijalizirana agenta
- Sekvencijalni pipeline s petljama povratnih informacija vođenim ocjenjivačem
- Streaming izlaz, pretraživanje kataloga proizvoda, strukturirani JSON prijelazi
- Potpuna implementacija u Pythonu (FastAPI), JavaScriptu (Node.js CLI) i C# (.NET konzola)

**Primjeri koda:**

| Jezik | Direktorij | Opis |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI web servis s orkestratorom |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI aplikacija |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konzolna aplikacija |

---

### Dio 8: Razvoj vođen evaluacijom

**Vodič za lab:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Izraditi sustavni okvir za evaluaciju AI agenata koristeći zlatne skupove podataka
- Provjere temeljene na pravilima (duljina, pokrivenost ključnih riječi, zabranjeni pojmovi) + ocjenjivanje LLM-as-judge
- Usporedba prompt varijanti uz agregirane kartice ocjena
- Proširuje obrazac agenta Zava Editor iz Dijela 7 u offline test paket
- Python, JavaScript i C# smjerovi

**Primjeri koda:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Okvir za evaluaciju |
| C# | `csharp/AgentEvaluation.cs` | Okvir za evaluaciju |
| JavaScript | `javascript/foundry-local-eval.mjs` | Okvir za evaluaciju |

---

### Dio 9: Glasovna transkripcija sa Whisperom

**Vodič za lab:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)

- Pretvorba govora u tekst koristeći OpenAI Whisper lokalno
- Obrada zvuka usredotočena na privatnost – zvuk nikada ne napušta uređaj
- Python, JavaScript i C# smjerovi s `client.audio.transcriptions.create()` (Python/JS) i `AudioClient.TranscribeAudioAsync()` (C#)
- Uključuje Zava-tematske primjere audio datoteka za praktičnu vježbu

**Primjeri koda:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Whisper glasovna transkripcija |
| C# | `csharp/WhisperTranscription.cs` | Whisper glasovna transkripcija |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Whisper glasovna transkripcija |

> **Napomena:** Ovaj lab koristi **Foundry Local SDK** za programsko preuzimanje i učitavanje Whisper modela, zatim šalje audio lokalnoj OpenAI-kompatibilnoj točki za transkripciju. Whisper model (`whisper`) je naveden u Foundry Local katalogu i radi u potpunosti na uređaju – bez ključeva za cloud API ni mrežnog pristupa.

---

### Dio 10: Korištenje prilagođenih ili Hugging Face modela

**Vodič za lab:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompajliranje Hugging Face modela u optimizirani ONNX format koristeći ONNX Runtime GenAI graditelj modela
- Kompilacija specifična za hardver (CPU, NVIDIA GPU, DirectML, WebGPU) i kvantizacija (int4, fp16, bf16)
- Izrada chat-template konfiguracijskih datoteka za Foundry Local
- Dodavanje kompajliranih modela u Foundry Local cache
- Pokretanje prilagođenih modela putem CLI-a, REST API-ja i OpenAI SDK-a
- Referentni primjer: kompletna kompilacija Qwen/Qwen3-0.6B

---

### Dio 11: Pozivanje alata s lokalnim modelima

**Vodič za lab:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Omogućite lokalnim modelima pozivanje vanjskih funkcija (pozivanje alata/funkcija)
- Definirajte sheme alata koristeći OpenAI format za pozivanje funkcija
- Upravljanje dijalogom s višekratnim pozivom alata
- Izvršite pozive alata lokalno i vratite rezultate modelu
- Odaberite pravi model za scenarije pozivanja alata (Qwen 2.5, Phi-4-mini)
- Koristite ugrađeni SDK `ChatClient` za pozivanje alata (JavaScript)

**Primjeri koda:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Pozivanje alata za vrijeme/popis stanovništva |
| C# | `csharp/ToolCalling.cs` | Pozivanje alata s .NET |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Pozivanje alata s ChatClient |

---

### Dio 12: Gradnja web UI za Zava Creative Writer

**Vodič za lab:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Dodajte sučelje bazirano na pregledniku za Zava Creative Writer
- Poslužite dijeljeni UI iz Pythona (FastAPI), JavaScripta (Node.js HTTP) i C# (ASP.NET Core)
- Koristite streaming NDJSON u pregledniku s Fetch API i ReadableStream
- Statusni oznake agenta uživo i streaming teksta članka u stvarnom vremenu

**Kod (dijeljeni UI):**

| Datoteka | Opis |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Izgled stranice |
| `zava-creative-writer-local/ui/style.css` | Stiliziranje |
| `zava-creative-writer-local/ui/app.js` | Logika stream readera i ažuriranja DOM-a |

**Dodaci na backendu:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Ažurirano za posluživanje statičkog UI |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Novi HTTP server koji omotava orkestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Novi ASP.NET Core minimalni API projekt |

---

### Dio 13: Radionica završena
**Vodič za laboratorij:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Sažetak svega što ste izgradili kroz svih 12 dijelova
- Dodatne ideje za proširenje vaših aplikacija
- Linkovi na resurse i dokumentaciju

---

## Struktura projekta

```
├── python/                        # Python examples
│   ├── foundry-local.py           # Basic chat
│   ├── foundry-local-with-agf.py  # Single agent (AGF)
│   ├── foundry-local-rag.py       # RAG pipeline
│   ├── foundry-local-multi-agent.py # Multi-agent workflow
│   ├── foundry-local-eval.py      # Agent evaluation framework
│   ├── foundry-local-whisper.py   # Whisper voice transcription
│   ├── foundry-local-tool-calling.py # Tool/function calling
│   └── requirements.txt
├── csharp/                        # C# examples
│   ├── Program.cs                 # CLI router (chat|rag|agent|multi|eval|whisper|toolcall)
│   ├── BasicChat.cs               # Basic chat
│   ├── RagPipeline.cs             # RAG pipeline
│   ├── SingleAgent.cs             # Single agent (ChatAgent pattern)
│   ├── MultiAgent.cs              # Multi-agent workflow
│   ├── AgentEvaluation.cs         # Agent evaluation framework
│   ├── WhisperTranscription.cs    # Whisper voice transcription
│   ├── ToolCalling.cs             # Tool/function calling
│   └── csharp.csproj
├── javascript/                    # JavaScript examples
│   ├── foundry-local.mjs          # Basic chat
│   ├── foundry-local-with-agent.mjs # Single agent
│   ├── foundry-local-rag.mjs     # RAG pipeline
│   ├── foundry-local-multi-agent.mjs # Multi-agent workflow
│   ├── foundry-local-eval.mjs     # Agent evaluation framework
│   ├── foundry-local-whisper.mjs  # Whisper voice transcription
│   ├── foundry-local-tool-calling.mjs # Tool/function calling
│   └── package.json
├── zava-creative-writer-local/ # Production multi-agent app
│   ├── ui/                        # Shared browser UI (Part 12)
│   │   ├── index.html             # Page layout
│   │   ├── style.css              # Styling
│   │   └── app.js                 # Stream reader and DOM updates
│   └── src/
│       ├── api/                   # Python FastAPI service
│       │   ├── main.py            # FastAPI server (serves UI)
│       │   ├── orchestrator.py    # Pipeline coordinator
│       │   ├── foundry_config.py  # Shared Foundry Local config
│       │   ├── requirements.txt
│       │   └── agents/            # Researcher, Product, Writer, Editor
│       ├── javascript/            # Node.js CLI and web server
│       │   ├── main.mjs           # CLI entry point
│       │   ├── server.mjs         # HTTP server with UI (Part 12)
│       │   ├── foundryConfig.mjs
│       │   └── package.json
│       ├── csharp/                # .NET 9 console app
│       │   ├── Program.cs
│       │   └── ZavaCreativeWriter.csproj
│       └── csharp-web/            # .NET 9 web API (Part 12)
│           ├── Program.cs
│           └── ZavaCreativeWriterWeb.csproj
├── labs/                          # Step-by-step lab guides
│   ├── part1-getting-started.md
│   ├── part2-foundry-local-sdk.md
│   ├── part3-sdk-and-apis.md
│   ├── part4-rag-fundamentals.md
│   ├── part5-single-agents.md
│   ├── part6-multi-agent-workflows.md
│   ├── part7-zava-creative-writer.md
│   ├── part8-evaluation-led-development.md
│   ├── part9-whisper-voice-transcription.md
│   ├── part10-custom-models.md
│   ├── part11-tool-calling.md
│   ├── part12-zava-ui.md
│   └── part13-workshop-complete.md
├── samples/
│   └── audio/                     # Zava-themed WAV files for Part 9
│       ├── generate_samples.py    # TTS script (pyttsx3) to create WAVs
│       └── README.md              # Sample descriptions
├── AGENTS.md                      # Coding agent instructions
├── package.json                   # Root devDependency (mermaid-cli)
├── LICENSE                        # MIT licence
└── README.md
```

---

## Resursi

| Resurs | Link |
|----------|------|
| Foundry Local web stranica | [foundrylocal.ai](https://foundrylocal.ai) |
| Katalog modela | [foundrylocal.ai/models](https://www.foundrylocal.ai/models) |
| Foundry Local GitHub | [github.com/microsoft/foundry-local](https://github.com/microsoft/foundry-local) |
| Vodič za početak | [Microsoft Learn - Foundry Local](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) |
| Foundry Local SDK Referenca | [Microsoft Learn - SDK Reference](https://learn.microsoft.com/en-us/azure/foundry-local/reference/reference-sdk) |
| Microsoft Agent Framework | [Microsoft Learn - Agent Framework](https://learn.microsoft.com/en-us/agent-framework/) |
| OpenAI Whisper | [github.com/openai/whisper](https://github.com/openai/whisper) |
| ONNX Runtime GenAI | [github.com/microsoft/onnxruntime-genai](https://github.com/microsoft/onnxruntime-genai) |

---

## Licenca

Ovaj materijal za radionicu je namijenjen u edukacijske svrhe.

---

**Sretno s izgradnjom! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Odricanje od odgovornosti**:
Ovaj dokument je preveden korištenjem AI usluge za prevođenje [Co-op Translator](https://github.com/Azure/co-op-translator). Iako nastojimo osigurati točnost, molimo imajte na umu da automatski prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na njegovom izvornom jeziku treba se smatrati autoritativnim izvorom. Za kritične informacije preporuča se profesionalni ljudski prijevod. Ne snosimo odgovornost za bilo kakve nesporazume ili kriva tumačenja koja proizlaze iz korištenja ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->