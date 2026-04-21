<p align="center">
  <img src="https://www.foundrylocal.ai/logos/foundry-local-logo-color.svg" alt="Foundry Local" width="280" />
</p>

# Foundry Local Radionica - Izgradite AI aplikacije na uređaju

Praktična radionica za pokretanje jezičnih modela na vašem vlastitom računalu i izgradnju inteligentnih aplikacija s [Foundry Local](https://foundrylocal.ai) i [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/).

> **Što je Foundry Local?** Foundry Local je lagano runtime okruženje koje vam omogućuje da preuzimate, upravljate i poslujete jezične modele u potpunosti na vašem hardveru. Izlaže **OpenAI-kompatibilan API** tako da se bilo koji alat ili SDK koji podržava OpenAI može povezati - nije potreban cloud račun.

### 🌐 Podrška za više jezika

#### Podržano putem GitHub akcije (Automatski i uvijek ažurno)

<!-- CO-OP TRANSLATOR LANGUAGES TABLE START -->
[Arabic](../ar/README.md) | [Bengali](../bn/README.md) | [Bulgarian](../bg/README.md) | [Burmese (Myanmar)](../my/README.md) | [Chinese (Simplified)](../zh-CN/README.md) | [Chinese (Traditional, Hong Kong)](../zh-HK/README.md) | [Chinese (Traditional, Macau)](../zh-MO/README.md) | [Chinese (Traditional, Taiwan)](../zh-TW/README.md) | [Croatian](./README.md) | [Czech](../cs/README.md) | [Danish](../da/README.md) | [Dutch](../nl/README.md) | [Estonian](../et/README.md) | [Finnish](../fi/README.md) | [French](../fr/README.md) | [German](../de/README.md) | [Greek](../el/README.md) | [Hebrew](../he/README.md) | [Hindi](../hi/README.md) | [Hungarian](../hu/README.md) | [Indonesian](../id/README.md) | [Italian](../it/README.md) | [Japanese](../ja/README.md) | [Kannada](../kn/README.md) | [Khmer](../km/README.md) | [Korean](../ko/README.md) | [Lithuanian](../lt/README.md) | [Malay](../ms/README.md) | [Malayalam](../ml/README.md) | [Marathi](../mr/README.md) | [Nepali](../ne/README.md) | [Nigerian Pidgin](../pcm/README.md) | [Norwegian](../no/README.md) | [Persian (Farsi)](../fa/README.md) | [Polish](../pl/README.md) | [Portuguese (Brazil)](../pt-BR/README.md) | [Portuguese (Portugal)](../pt-PT/README.md) | [Punjabi (Gurmukhi)](../pa/README.md) | [Romanian](../ro/README.md) | [Russian](../ru/README.md) | [Serbian (Cyrillic)](../sr/README.md) | [Slovak](../sk/README.md) | [Slovenian](../sl/README.md) | [Spanish](../es/README.md) | [Swahili](../sw/README.md) | [Swedish](../sv/README.md) | [Tagalog (Filipino)](../tl/README.md) | [Tamil](../ta/README.md) | [Telugu](../te/README.md) | [Thai](../th/README.md) | [Turkish](../tr/README.md) | [Ukrainian](../uk/README.md) | [Urdu](../ur/README.md) | [Vietnamese](../vi/README.md)

> **Radije želite klonirati lokalno?**
>
> Ovo spremište uključuje više od 50 prijevoda jezika što značajno povećava veličinu preuzimanja. Za kloniranje bez prijevoda koristite sparse checkout:
>
> **Bash / macOS / Linux:**
> ```bash
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone '/*' '!translations' '!translated_images'
> ```
>
> **CMD (Windows):**
> ```cmd
> git clone --filter=blob:none --sparse https://github.com/microsoft-foundry/Foundry-Local-Lab.git
> cd Foundry-Local-Lab
> git sparse-checkout set --no-cone "/*" "!translations" "!translated_images"
> ```
>
> Ovo vam pruža sve što vam treba za dovršetak tečaja s puno bržim preuzimanjem.
<!-- CO-OP TRANSLATOR LANGUAGES TABLE END -->

---

## Ciljevi učenja

Do kraja ove radionice moći ćete:

| # | Cilj |
|---|-----------|
| 1 | Instalirati Foundry Local i upravljati modelima putem CLI-a |
| 2 | Ovladati Foundry Local SDK API-jem za programsko upravljanje modelima |
| 3 | Povezati se s lokalnim inference serverom koristeći Python, JavaScript i C# SDK-ove |
| 4 | Izgraditi Retrieval-Augmented Generation (RAG) pipeline koji temelji odgovore na vašim vlastitim podacima |
| 5 | Kreirati AI agente s trajnim uputama i osobnostima |
| 6 | Orkestrirati tijekove rada s više agenata i petlje povratne informacije |
| 7 | Istražiti produkcijsku završnu aplikaciju - Zava Creative Writer |
| 8 | Izgraditi evaluacijske okvire s zlatnim skupovima podataka i LLM-om kao sucem |
| 9 | Transkribirati audio s Whisper - pretvaranje govora u tekst na uređaju koristeći Foundry Local SDK |
| 10 | Kompilirati i pokretati prilagođene ili Hugging Face modele uz ONNX Runtime GenAI i Foundry Local |
| 11 | Omogućiti lokalnim modelima da pozivaju vanjske funkcije uz uzorak tool-calling |
| 12 | Izgraditi UI baziran na pregledniku za Zava Creative Writer sa streamingom u stvarnom vremenu |

---

## Preduvjeti

| Zahtjev | Detalji |
|-------------|---------|
| **Hardver** | Minimalno 8 GB RAM-a (preporučeno 16 GB); CPU s AVX2 podrškom ili podržana GPU kartica |
| **OS** | Windows 10/11 (x64/ARM), Windows Server 2025 ili macOS 13+ |
| **Foundry Local CLI** | Instalirajte putem `winget install Microsoft.FoundryLocal` (Windows) ili `brew tap microsoft/foundrylocal && brew install foundrylocal` (macOS). Pogledajte [vodič za početak](https://learn.microsoft.com/en-us/azure/foundry-local/get-started) za detalje. |
| **Jezik runtime** | **Python 3.9+** i/ili **.NET 9.0+** i/ili **Node.js 18+** |
| **Git** | Za kloniranje ovog spremišta |

---

## Početak rada

```bash
# 1. Klonirajte repozitorij
git clone https://github.com/microsoft-foundry/foundry-local-lab.git
cd foundry-local-lab

# 2. Provjerite je li Foundry Local instaliran
foundry model list              # Nabrojite dostupne modele
foundry model run phi-3.5-mini  # Pokrenite interaktivni chat

# 3. Odaberite svoj jezični smjer (pogledajte Laboratorij dio 2 za potpunu instalaciju)
```

| Jezik | Brzi početak |
|----------|-------------|
| **Python** | `cd python && pip install -r requirements.txt && python foundry-local.py` |
| **C#** | `cd csharp && dotnet run` |
| **JavaScript** | `cd javascript && npm install && node foundry-local.mjs` |

---

## Dijelovi radionice

### Dio 1: Početak s Foundry Local

**Vodič laboratorija:** [`labs/part1-getting-started.md`](labs/part1-getting-started.md)

- Što je Foundry Local i kako funkcionira
- Instalacija CLI-ja na Windows i macOS
- Istraživanje modela - popis, preuzimanje, pokretanje
- Razumijevanje aliasa modela i dinamičkih portova

---

### Dio 2: Dubinska analiza Foundry Local SDK-a

**Vodič laboratorija:** [`labs/part2-foundry-local-sdk.md`](labs/part2-foundry-local-sdk.md)

- Zašto koristiti SDK umjesto CLI za razvoj aplikacija
- Potpuna referenca SDK API-ja za Python, JavaScript i C#
- Upravljanje servisima, pregled kataloga, životni ciklus modela (preuzimanje, učitavanje, isključivanje)
- Uzorci brzog početka: Python konstruktor bootstrap, JavaScript `init()`, C# `CreateAsync()`
- Metapodaci `FoundryModelInfo`, aliasi i odabir modela optimiziranog za hardver

---

### Dio 3: SDK-ovi i API-ji

**Vodič laboratorija:** [`labs/part3-sdk-and-apis.md`](labs/part3-sdk-and-apis.md)

- Povezivanje s Foundry Local iz Python, JavaScript i C#
- Korištenje Foundry Local SDK-a za programsko upravljanje servisom
- Streaming chat dovršetaka putem OpenAI-kompatibilnog API-ja
- Referenca metoda SDK-a za svaki jezik

**Primjeri koda:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local.py` | Osnovni streaming chat |
| C# | `csharp/BasicChat.cs` | Streaming chat s .NET |
| JavaScript | `javascript/foundry-local.mjs` | Streaming chat s Node.js |

---

### Dio 4: Retrieval-Augmented Generation (RAG)

**Vodič laboratorija:** [`labs/part4-rag-fundamentals.md`](labs/part4-rag-fundamentals.md)

- Što je RAG i zašto je važan
- Izrada baze znanja u memoriji
- Dohvat preko preklapanja ključnih riječi s bodovanjem
- Sastavljanje sustavskih promptova s utemeljenjem
- Pokretanje kompletne RAG pipeline na uređaju

**Primjeri koda:**

| Jezik | Datoteka |
|----------|------|
| Python | `python/foundry-local-rag.py` |
| C# | `csharp/RagPipeline.cs` |
| JavaScript | `javascript/foundry-local-rag.mjs` |

---

### Dio 5: Izgradnja AI agenata

**Vodič laboratorija:** [`labs/part5-single-agents.md`](labs/part5-single-agents.md)

- Što je AI agent (nasuprot sirovom pozivu LLM-a)
- Uzorak `ChatAgent` i Microsoft Agent Framework
- Sustavne upute, osobnosti i višekratni razgovori
- Strukturirani izlaz (JSON) od agenata

**Primjeri koda:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-with-agf.py` | Pojedinačni agent s Agent Frameworkom |
| C# | `csharp/SingleAgent.cs` | Pojedinačni agent (ChatAgent uzorak) |
| JavaScript | `javascript/foundry-local-with-agent.mjs` | Pojedinačni agent (ChatAgent uzorak) |

---

### Dio 6: Tijekovi rada s više agenata

**Vodič laboratorija:** [`labs/part6-multi-agent-workflows.md`](labs/part6-multi-agent-workflows.md)

- Pipeline s više agenata: Istraživač → Pisac → Urednik
- Sekvencijalna orkestracija i petlje povratne informacije
- Dijeljene konfiguracije i strukturirani prijenosi
- Dizajnirajte vlastiti tijek rada s više agenata

**Primjeri koda:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-multi-agent.py` | Pipeline s tri agenta |
| C# | `csharp/MultiAgent.cs` | Pipeline s tri agenta |
| JavaScript | `javascript/foundry-local-multi-agent.mjs` | Pipeline s tri agenta |

---

### Dio 7: Zava Creative Writer - završna aplikacija

**Vodič laboratorija:** [`labs/part7-zava-creative-writer.md`](labs/part7-zava-creative-writer.md)

- Produkcijska multi-agent aplikacija s 4 specijalizirana agenta
- Sekvencijalni pipeline s evaluatorom koji pokreće petlje povratne informacije
- Streaming izlaz, pretraživanje kataloga proizvoda, strukturirani JSON prijenosi
- Potpuna implementacija u Pythonu (FastAPI), JavaScriptu (Node.js CLI) i C# (.NET konzola)

**Primjeri koda:**

| Jezik | Direktorij | Opis |
|----------|-----------|-------------|
| Python | `zava-creative-writer-local/src/api/` | FastAPI web servis s orkestratorom |
| JavaScript | `zava-creative-writer-local/src/javascript/` | Node.js CLI aplikacija |
| C# | `zava-creative-writer-local/src/csharp/` | .NET 9 konzolna aplikacija |

---

### Dio 8: Razvoj vođen evaluacijom

**Vodič laboratorija:** [`labs/part8-evaluation-led-development.md`](labs/part8-evaluation-led-development.md)

- Izgradite sustavni evaluacijski okvir za AI agente koristeći zlatne skupove podataka
- Provjere temeljene na pravilima (duljina, pokrivenost ključnih riječi, zabranjeni pojmovi) + bodovanje LLM-om kao sucem
- Usporedba varijanti promptova jedan do drugog s agregiranim rezultatima
- Proširuje uzorak Zava Editor agenta iz Dijela 7 u offline testni paket
- Trake za Python, JavaScript i C#

**Primjeri koda:**

| Jezik | Datoteka | Opis |
|----------|------|-------------|
| Python | `python/foundry-local-eval.py` | Evaluacijski okvir |
| C# | `csharp/AgentEvaluation.cs` | Evaluacijski okvir |
| JavaScript | `javascript/foundry-local-eval.mjs` | Evaluacijski okvir |

---

### Dio 9: Transkripcija glasa s Whisper

**Vodič laboratorija:** [`labs/part9-whisper-voice-transcription.md`](labs/part9-whisper-voice-transcription.md)
- Pretvaranje govora u tekst koristeći OpenAI Whisper koji se izvršava lokalno
- Obrada audio zapisa s naglaskom na privatnost – audio nikada ne napušta vaš uređaj
- Python, JavaScript i C# primjeri s `client.audio.transcriptions.create()` (Python/JS) i `AudioClient.TranscribeAudioAsync()` (C#)
- Uključene Zava-tematske uzorke audio datoteka za praktičnu vježbu

**Primjeri koda:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-whisper.py` | Pretvaranje glasa u tekst Whisperom |
| C# | `csharp/WhisperTranscription.cs` | Pretvaranje glasa u tekst Whisperom |
| JavaScript | `javascript/foundry-local-whisper.mjs` | Pretvaranje glasa u tekst Whisperom |

> **Napomena:** Ovaj laboratorij koristi **Foundry Local SDK** za programsko preuzimanje i učitavanje Whisper modela, zatim šalje audio na lokalnu OpenAI-kompatibilnu točku za transkripciju. Whisper model (`whisper`) naveden je u Foundry Local katalogu i u potpunosti radi lokalno – nisu potrebni API ključevi za cloud niti mrežni pristup.

---

### Dio 10: Korištenje prilagođenih ili Hugging Face modela

**Vodič za laboratorij:** [`labs/part10-custom-models.md`](labs/part10-custom-models.md)

- Kompiliranje Hugging Face modela u optimizirani ONNX format koristeći ONNX Runtime GenAI model builder
- Kompilacija prilagođena hardveru (CPU, NVIDIA GPU, DirectML, WebGPU) i kvantizacija (int4, fp16, bf16)
- Izrada konfiguracijskih datoteka za chat predloške za Foundry Local
- Dodavanje kompiliranih modela u Foundry Local cache
- Pokretanje prilagođenih modela putem CLI, REST API i OpenAI SDK
- Referentni primjer: krajnji proces kompilacije Qwen/Qwen3-0.6B

---

### Dio 11: Pozivanje alata s lokalnim modelima

**Vodič za laboratorij:** [`labs/part11-tool-calling.md`](labs/part11-tool-calling.md)

- Omogućavanje lokalnim modelima pozivanje vanjskih funkcija (pozivanje alata/funkcija)
- Definiranje shema alata koristeći OpenAI format poziva funkcija
- Upravljanje višekratnim tijekom razgovora za pozivanje alata
- Izvršavanje poziva alata lokalno i vraćanje rezultata modelu
- Odabir pravog modela za scenarije pozivanja alata (Qwen 2.5, Phi-4-mini)
- Korištenje nativnog `ChatClient` SDK-a za pozivanje alata (JavaScript)

**Primjeri koda:**

| Language | File | Description |
|----------|------|-------------|
| Python | `python/foundry-local-tool-calling.py` | Pozivanje alata za vremensku prognozu i populaciju |
| C# | `csharp/ToolCalling.cs` | Pozivanje alata u .NET-u |
| JavaScript | `javascript/foundry-local-tool-calling.mjs` | Pozivanje alata s ChatClient |

---

### Dio 12: Izrada web korisničkog sučelja za Zava Creative Writer

**Vodič za laboratorij:** [`labs/part12-zava-ui.md`](labs/part12-zava-ui.md)

- Dodavanje pregledničkog korisničkog sučelja za Zava Creative Writer
- Poslužiti zajedničko korisničko sučelje iz Pythona (FastAPI), JavaScripta (Node.js HTTP) i C# (ASP.NET Core)
- Potrošnja streaming NDJSON u pregledniku putem Fetch API i ReadableStream
- Statusne značke uživo za agente i streaming teksta članaka u stvarnom vremenu

**Kod (zajedničko korisničko sučelje):**

| File | Description |
|------|-------------|
| `zava-creative-writer-local/ui/index.html` | Raspored stranice |
| `zava-creative-writer-local/ui/style.css` | Stiliziranje |
| `zava-creative-writer-local/ui/app.js` | Čitač streama i logika za ažuriranje DOM-a |

**Dodatci na backendu:**

| Language | File | Description |
|----------|------|-------------|
| Python | `zava-creative-writer-local/src/api/main.py` | Ažurirano za posluživanje statičkog sučelja |
| JavaScript | `zava-creative-writer-local/src/javascript/server.mjs` | Novi HTTP poslužitelj koji omotava orkestrator |
| C# | `zava-creative-writer-local/src/csharp-web/Program.cs` | Novi minimalni ASP.NET Core API projekt |

---

### Dio 13: Završetak radionice

**Vodič za laboratorij:** [`labs/part13-workshop-complete.md`](labs/part13-workshop-complete.md)

- Sažetak svega što ste napravili kroz svih 12 dijelova
- Daljnje ideje za proširenje vaših aplikacija
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

| Resource | Link |
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

Ovaj materijal radionice je osmišljen u edukacijske svrhe.

---

**Sretno u izgradnji! 🚀**

---

<!-- CO-OP TRANSLATOR DISCLAIMER START -->
**Odricanje od odgovornosti**:
Ovaj dokument je preveden korištenjem AI prevoditeljskog servisa [Co-op Translator](https://github.com/Azure/co-op-translator). Iako nastojimo postići točnost, imajte na umu da automatski prijevodi mogu sadržavati pogreške ili netočnosti. Izvorni dokument na izvornom jeziku treba smatrati autoritativnim izvorom. Za kritične informacije preporučuje se profesionalni prijevod od strane čovjeka. Ne snosimo odgovornost za bilo kakve nesporazume ili kriva tumačenja koja proizlaze iz korištenja ovog prijevoda.
<!-- CO-OP TRANSLATOR DISCLAIMER END -->